using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PP_COM_Wrapper;

namespace CTT_4_TESTER
{
    class P4Program
    {
        //Error constants
        protected const int S_OK = 0;
        protected const int E_FAIL = -1;

        //Chip Level Protection constants
        const int CHIP_PROT_VIRGIN = 0x00;
        const int CHIP_PROT_OPEN = 0x01;
        const int CHIP_PROT_PROTECTED = 0x02;
        const int CHIP_PROT_KILL = 0x04;
        const int CHIP_PROT_MASK = 0x0F;

        static PSoCProgrammerCOM_ObjectClass pp = new PSoCProgrammerCOM_ObjectClass();
        static string m_sLastError;
        ProgressBar progressBar;
        P4CttTestForm parent;


        static bool SUCCEEDED(int hr)
        {
            return hr >= 0;
        }

        public P4Program(ProgressBar progressBar, P4CttTestForm parent)
        {
            this.progressBar = progressBar;
            this.parent = parent;
        }
        public string getLastError()
        {
            return m_sLastError;
        }

        public bool Open()
        {
            int hr;
            if (pp == null)
            {
                m_sLastError = "PP not initliased";
                return false;

            }

            //Port Initialization
            hr = OpenPort();
            if (!SUCCEEDED(hr))
            {
                //m_sLastError is intialized in OpenPort
                return false;
            }
            return true;
        }

        static private int OpenPort()
        {
            int hr;
            //Open Port - get last (connected) port in the ports list
            object portArray;
            hr = pp.GetPorts(out portArray, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            string[] ports = portArray as string[];
            if (ports.Length <= 0)
            {
                m_sLastError = "Connect any Programmer to PC";
                return E_FAIL;
            }

            bool bFound = false;
            string portName = "";
            for (int i = 0; i < ports.Length; i++)
            {
                if (ports[i].StartsWith("MiniProg3") || ports[i].StartsWith("TrueTouchBridge") || ports[i].StartsWith("KitProg"))
                {
                    portName = ports[i];
                    bFound = true;
                    break;
                }
            }

            if (!bFound)
            {
                m_sLastError = "Connect any MiniProg3/TrueTouchBridge/KitProg device to the PC";
                return E_FAIL;
            }

            //Port should be opened just once to connect Programmer device (MiniProg1/3,etc).
            //After that you can use Chip-/Programmer- specific APIs as long as you need.
            //No need to repoen port when you need to acquire chip 2nd time, just call Acquire() again.
            //This is true for all other APIs which get available once port is opened.
            //You have to call OpenPort() again if port was closed by ClosePort() method, or 
            //when there is a need to connect to other programmer, or
            //if programmer was physically reconnected to USB-port.
            
            hr = pp.OpenPort(portName, out m_sLastError);
            

            return hr;
        }

        static int ClosePort()
        {
            //Close port when all operations are complete, so it will be available for other applications
            int hr;
            string strError;

            hr = pp.ClosePort(out strError);

            return hr;
        }

        static private int InitializePort()
        {
            int hr;

            //Setup Power - "3.3V" and internal for reset mode
            pp.SetPowerVoltage("3.3", out m_sLastError);
            hr = pp.PowerOn(out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            //Set protocol
            hr = pp.SetProtocol(enumInterfaces.SWD, out m_sLastError); //SWD-protocol
            if (!SUCCEEDED(hr)) return hr;
            //For MiniProg3 - Set protocol, connector and frequency (do not check result, since it will fail for TTBridge - not supported)
            pp.SetProtocolConnector(0, out m_sLastError); //5-pin connector
            pp.SetProtocolClock(enumFrequencies.FREQ_03_0, out m_sLastError); //3.0 MHz clock on SWD bus

            return hr;
        }

        static private int CheckHexAndDeviceCompatibility(out bool result)
        {
            int hr;
            object jtagID;
            byte[] hexJtagID, chipJtagID;
            result = true;

            //Read silicon ID from chip
            hr = pp.PSoC4_GetSiliconID(out jtagID, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;
            chipJtagID = jtagID as byte[];

            //Read silicon ID from hex
            hr = pp.HEX_ReadJtagID(out jtagID, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;
            hexJtagID = jtagID as byte[];

            result = true;
            // Currently this cycle is commented, since the real silicon ID is not generated by PSoC Creator
            for (byte i = 0; i < 4; i++)
            {
                if (i == 2) continue; //ignore revision, 11(AA),12(AB),13(AC), etc
                if (hexJtagID[i] != chipJtagID[i])
                {
                    result = false;
                    break;
                }
            }

            return S_OK;
        }

        static private int PSoC4_GetTotalFlashRowsCount(int flashSize, out int totalRows, out int rowSize)
        {
            int hr, rowsPerArray;
            totalRows = 0;

            hr = pp.PSoC4_GetFlashInfo(out rowsPerArray, out rowSize, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            totalRows = flashSize / rowSize;

            return hr;
        }

        static private int PSoC4_IsChipNotProtected()
        {
            int hr;

            object flashProt, chipProt;
            byte[] data;

            //Chip Level Protection reliably can be read by below API (available in VIRGIN, OPEN, PROTECTED modes)
            //This API uses SROM call - to read current status of CPUSS_PROTECTION register (privileged)
            //This register contains current protection mode loaded from SFLASH during boot-up.
            hr = pp.PSoC4_ReadProtection(out flashProt, out chipProt, out m_sLastError);
            if (!SUCCEEDED(hr)) return E_FAIL; //consider chip as protected if any communication failure

            data = chipProt as byte[];
            //Check Result
            if ((data[0] & CHIP_PROT_PROTECTED) == CHIP_PROT_PROTECTED)
            {
                m_sLastError = "Chip is in PROTECTED mode. Any access to Flash is suppressed.";
                return E_FAIL;
            }

            return S_OK;
        }

        static private int PSoC4_EraseAll()
        {
            int hr;

            //Check chip level protection here. If PROTECTED then move to OPEN by PSoC4_WriteProtection() API.
            //Otherwise use PSoC4_EraseAll() - in OPEN/VIRGIN modes.
            hr = PSoC4_IsChipNotProtected();
            if (SUCCEEDED(hr)) //OPEN mode
            {
                //Erase All - Flash and Protection bits. Still be in OPEN mode.
                hr = pp.PSoC4_EraseAll(out m_sLastError);
            }
            else
            {   //Move to OPEN from PROTECTED. It automatically erases Flash and its Protection bits.
                byte[] flashProt = new byte[] { }; // do not care in PROTECTED mode
                byte[] chipProt = new byte[] { CHIP_PROT_OPEN }; //move to OPEN

                hr = pp.PSoC4_WriteProtection(flashProt, chipProt, out m_sLastError);
                if (!SUCCEEDED(hr)) return hr;

                //Need to reacquire chip here to boot in OPEN mode.
                //ChipLevelProtection is applied only after Reset.
                hr = pp.DAP_Acquire(out m_sLastError);
            }
            return hr;
        }

        private int ProgramFlash(int flashSize)
        {
            int hr = 0, totalRows = 0, rowSize = 0;
            hr = PSoC4_GetTotalFlashRowsCount(flashSize, out totalRows, out rowSize);
            if (!SUCCEEDED(hr)) return hr;

            //Program Flash array
            for (int i = 0; i < totalRows; i++)
            {
                hr = pp.PSoC4_ProgramRowFromHex(i, out m_sLastError);
                parent.SetProgressBar(progressBar, i*100/ totalRows/2);
                if (!SUCCEEDED(hr)) return hr;
            }

            return hr;
        }

        private int PSoC4_VerifyFlash(int flashSize)
        {
            int hr = 0, totalRows = 0, rowSize = 0;
            hr = PSoC4_GetTotalFlashRowsCount(flashSize, out totalRows, out rowSize);
            if (!SUCCEEDED(hr)) return hr;

            //Verify Flash array
            for (int i = 0; i < totalRows; i++)
            {
                int verResult;
                hr = pp.PSoC4_VerifyRowFromHex(i, out verResult, out m_sLastError);
                parent.SetProgressBar(progressBar,50 + i * 100 / totalRows / 2);
                if (!SUCCEEDED(hr)) return hr;
                if (verResult == 0)
                {
                    m_sLastError = "Verification failed on " + i + " row.";
                    return E_FAIL;
                }
            }
            return hr;
        }

        int ProgramAll(string filePath)
        {
            if (pp == null) return E_FAIL;

           // string path = System.IO.Path.GetDirectoryName(Environment.CurrentDirectory);
           // string filePath = path+"\\P4_BLE_RF_App.hex";// path + "\\..\\..\\HEX-Files\\P4_BLE_RF_App.hex";

            int hr;

            //Port Initialization
            hr = InitializePort();
            if (!SUCCEEDED(hr)) return hr;

            // Set Hex File
            int hexImageSize;
            hr = pp.HEX_ReadFile(filePath, out hexImageSize, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            //Read chip level protection from hex and check Chip Level Protection mode
            //If it is VIRGIN then don't allow Programming, since it can destroy chip
            object data;
            hr = pp.HEX_ReadChipProtection(out data, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;
            byte hex_chipProt = (data as byte[])[0];

            if (hex_chipProt == CHIP_PROT_VIRGIN)
            {
                m_sLastError = "Transition to VIRGIN is not allowed. It will destroy the chip. Please contact Cypress if you need this specifically.";
                return E_FAIL;
            }

            //Set Acquire Mode
            pp.SetAcquireMode("Reset", out m_sLastError);

            //Acquire Device
            hr = pp.DAP_Acquire(out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            //Check Hex File and Device compatibility
            bool fCompatibility = true;
            hr = CheckHexAndDeviceCompatibility(out fCompatibility);
            if (!SUCCEEDED(hr)) return hr;
            if (!fCompatibility)
            {
                m_sLastError = "The Hex file does not match the acquired device, please connect the appropriate device";
                return E_FAIL;
            }

            //Erase All
            hr = PSoC4_EraseAll();
            if (!SUCCEEDED(hr)) return hr;

            //Find checksum of Privileged Flash. Will be used in calculation of User CheckSum later
            int checkSum_Privileged = 0;
            hr = pp.PSoC4_CheckSum(0x8000, out checkSum_Privileged, out m_sLastError); //CheckSum All Flash ("Privileged + User" Rows)
            if (!SUCCEEDED(hr)) return hr;

            //Program Flash
            hr = ProgramFlash(hexImageSize);
            if (!SUCCEEDED(hr)) return hr;

            //Verify Rows
            hr = PSoC4_VerifyFlash(hexImageSize);
            if (!SUCCEEDED(hr)) return hr;

            //Protect All arrays
            hr = pp.PSoC4_ProtectAll(out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            //Verify protection ChipLevelProtection and Protection data
            hr = pp.PSoC4_VerifyProtect(out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            //CheckSum verification
            int checkSum_User = 0, checkSum_UserPrivileged;
            short hexChecksum;
            hr = pp.PSoC4_CheckSum(0x8000, out checkSum_UserPrivileged, out m_sLastError); //CheckSum All Flash (Privileged + User)
            if (!SUCCEEDED(hr)) return hr;
            checkSum_User = checkSum_UserPrivileged - checkSum_Privileged; //find checksum of User Flash rows

            hr = pp.HEX_ReadChecksum(out hexChecksum, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;
            checkSum_User &= 0xFFFF;
            int hexChecks = hexChecksum & 0xFFFF;

            if (checkSum_User != hexChecks)
            {
                m_sLastError = "Mismatch of Checksum: Expected 0x" + checkSum_User.ToString("X") + ", Got 0x" + hexChecksum.ToString("X");
                return E_FAIL;
            }
            else
                Console.WriteLine("Checksum 0x" + checkSum_User.ToString("X"));

            //Release PSoC3 device
            hr = pp.DAP_ReleaseChip(out m_sLastError);

            return hr;
        }

        static private int UpgradeBlock()
        {
            if (pp == null) return E_FAIL;

            int hr;

            //Port Initialization
            hr = InitializePort();
            if (!SUCCEEDED(hr)) return hr;

            //Set Acquire Mode
            pp.SetAcquireMode("Reset", out m_sLastError);

            //Acquire Device
            hr = pp.DAP_Acquire(out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            //Write Block, use PSoC4_WriteRow() instead PSoC3_ProgramRow()
            int rowsPerArray, rowSize = 0;
            hr = pp.PSoC4_GetFlashInfo(out rowsPerArray, out rowSize, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            byte[] writeData = new byte[rowSize]; //User and Config area of the Row (256+32)
            for (int i = 0; i < writeData.Length; i++) writeData[i] = (byte)i;
            int rowID = rowSize - 1;
            hr = pp.PSoC4_WriteRow(rowID, writeData, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            //Verify Row - only user area (without Config one)
            object readData;
            hr = pp.PSoC4_ReadRow(rowID, out readData, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;
            byte[] readRow = readData as byte[];
            for (int i = 0; i < readRow.Length; i++) //check 128 bytes
            {
                if (readRow[i] != writeData[i])
                {
                    hr = E_FAIL;
                    break;
                }
            }
            if (!SUCCEEDED(hr))
            {
                m_sLastError = "Verification of User area failed!";
                return hr;
            }

            //Release PSoC4 chip
            hr = pp.DAP_ReleaseChip(out m_sLastError);

            return hr;
        }

        public int Execute(string filePath)
        {

            int hr;
            //Execute Programming of the silicon
            hr = ProgramAll(filePath); //hr = UpgradeBlock();

            return hr;
        }
    }
}
