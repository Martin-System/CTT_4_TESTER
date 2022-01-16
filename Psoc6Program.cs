using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PP_COM_Wrapper;

namespace CTT_4_TESTER
{
    class Psoc6Program
    {
        //Error constants
        const int S_OK = 0;
        const int E_FAIL = -1;

        //PSoC6 Flash Constants
        const int PSOC6_FLASH_ADDR = 0x10000000; // Base address for Main Flash
        const int PSOC6_WFLASH_ADDR = 0x14000000; // Base address for Work Flash
        const int PSOC6_SFLASH_ADDR = 0x16000000; // Base address for Supervisory Flash

        const int PSOC6_FLASH_SIZE = 0x00100000; // Maximum size of the main flash area
        const int PSOC6_WFLASH_SIZE = 0x8000; // 32KB of Work Flash Area
        const int PSOC6_SFLASH_SIZE = 0x8000; // 32KB of Supervisory Flash Area

        //MiniProgs connectors
        const int connector5pin = 0;
        const int connector10pin = 1;

        static readonly PSoCProgrammerCOM_ObjectClass pp = new PSoCProgrammerCOM_ObjectClass();
        delegate int ProgramDeleg<in T1, T2>(T1 rowId, out T2 strError);

        static string m_sLastError = string.Empty;
        static string m_sPortName = string.Empty;
        static int m_rowsPerFlash;
        static int m_rowSize;

        static readonly string[] m_lSupportedPorts =
        {
            "MiniProg3",
            "MiniProg4",
            "KitProg2",
            "KitProg3",
            "CMSIS-DAP"
        };
        ProgressBar progressBar;
        P4CttTestForm parent;

        public Psoc6Program(ProgressBar progressBar, P4CttTestForm parent)
        {
            this.progressBar = progressBar;
            this.parent = parent;

            //Automatically releases all resources of the COM object if the client application is terminated abnormally
            int clientProcessID = System.Diagnostics.Process.GetCurrentProcess().Id;
            pp._StartSelfTerminator(clientProcessID);
        }

        #region Auxiliary functions

        int GetPowerCapabilities(out bool canSupplyPower, out bool canControlVoltage)
        {
            canSupplyPower = canControlVoltage = false;

            int result = E_FAIL;
            object capabilities;
            int comHr = pp.GetProgrammerCapabilities(out capabilities, out m_sLastError);
            if (SUCCEEDED(comHr) && capabilities != null)
            {
                //capabilities[1] – represents the power abilities of the programmer
                //capabilities[5] – contains a set of internal voltage sources of the programmer that can be supplied
                byte[] caps = capabilities as byte[];
                if (caps != null && caps.Length >= 6)
                {
                    result = S_OK;
                    canSupplyPower = (caps[1] & Convert.ToByte(enumCanPowerDevice.CAN_POWER_DEVICE)) != 0;
                    canControlVoltage =
                        (caps[5] & Convert.ToByte(enumVoltages.VOLT_18V)) != 0 ||
                        (caps[5] & Convert.ToByte(enumVoltages.VOLT_25V)) != 0 ||
                        (caps[5] & Convert.ToByte(enumVoltages.VOLT_33V)) != 0 ||
                        (caps[5] & Convert.ToByte(enumVoltages.VOLT_50V)) != 0;
                }
            }

            return result;
        }

        bool PowerOn(string voltage)
        {
            //Retrieve the power capabilities of the connected programmer
            bool canPowerDevice, canControlVoltage;
            if (!SUCCEEDED(GetPowerCapabilities(out canPowerDevice, out canControlVoltage)))
                return false;

            //canControlVoltage means that programmer can supply different voltages (usually 1.8V, 2.5V, 3.3V, 5.0V)
            if (canControlVoltage)
            {
                pp.SetPowerVoltage(voltage, out m_sLastError);
            }

            //canPowerDevice means that programmer supports Power On/Off feature
            if (canPowerDevice)
            {
                pp.PowerOn(out m_sLastError);
            }

            return true;
        }

        bool PowerOff()
        {
            //Retrieve the power capabilities of the connected programmer
            bool canPowerDevice, canControlVoltage;
            if (!SUCCEEDED(GetPowerCapabilities(out canPowerDevice, out canControlVoltage)))
                return false;

            //canPowerDevice means that programmer supports Power On/Off feature
            if (canPowerDevice)
            {
                pp.PowerOff(out m_sLastError);
            }

            return true;
        }

        bool SUCCEEDED(int hr)
        {
            return hr >= 0;
        }

        int CheckHexAndDeviceCompatibility(out bool result)
        {
            int hr;
            object jtagID;
            byte[] hexJtagID, chipJtagID;
            result = true;

            //Read silicon ID from device
            int familyIdHi, familyIdLo, revisionIdMaj, revisionIdMin, siliconIdHi, siliconIdLo, sromFmVersionMaj, sromFmVersionMin, protectState, lifeCycleStage;
            hr = pp.PSoC6_GetSiliconID(out jtagID, out familyIdHi, out familyIdLo, out revisionIdMaj,
                out revisionIdMin, out siliconIdHi, out siliconIdLo, out sromFmVersionMaj, out sromFmVersionMin,
                out protectState, out lifeCycleStage, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;
            chipJtagID = jtagID as byte[];

            //Read silicon ID from hex
            hr = pp.HEX_ReadJtagID(out jtagID, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;
            hexJtagID = jtagID as byte[];

            //Check compatability 1:1 (HEX ID to CHIP ID)
            for (byte i = 0; i < 4; i++)
            {
                if (i == 2) continue; //ignore revision ID
                if (hexJtagID[i] != chipJtagID[i])
                {
                    result = false;
                    break;
                }
            }

            return S_OK;
        }

        int HEX_GetRegionBounds(int address, int size, int rowSize, out int startRow, out int endRow)
        {
            startRow = endRow = 0;

            int size1;
            int hr = pp.HEX_GetDataSizeInRange(PSOC6_FLASH_ADDR, (uint)(address - 1), out size1, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;
            startRow = size1 / rowSize;

            int size2;
            hr = pp.HEX_GetDataSizeInRange(PSOC6_FLASH_ADDR, (uint)(address + size - 1), out size2, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;
            endRow = size2 / rowSize - 1;

            return hr;
        }

        #endregion
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

        int OpenPort()
        {
            //Open Port - get last (connected) port in the ports list
            object portArray;
            int hr = pp.GetPorts(out portArray, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            string[] ports = portArray as string[];
            if (ports == null || ports.Length <= 0)
            {
                m_sLastError = "Connect any Programmer to PC";
                return -1;
            }

            bool bFound = false;
            for (int i = 0; i < ports.Length; i++)
            {
                if (Array.Find(m_lSupportedPorts, element => ports[i].Contains(element)) != null)
                {
                    m_sPortName = ports[i];
                    bFound = true;
                    break;
                }
            }

            if (!bFound)
            {
                m_sLastError = string.Format("Connect any {0} device to the PC", string.Join("/", m_lSupportedPorts));
                return -1;
            }

            //Port should be opened just once to connect Programmer device (MiniProg3/4,etc).
            //After that you can use Chip-/Programmer- specific APIs as long as you need.
            //No need to repoen port when you need to acquire chip 2nd time, just call Acquire() again.
            //This is true for all other APIs which get available once port is opened.
            //You have to call OpenPort() again if port was closed by ClosePort() method, or 
            //when there is a need to connect to other programmer, or
            //if programmer was physically reconnected to USB-port.

            hr = pp.OpenPort(m_sPortName, out m_sLastError);

            return hr;
        }

        int ClosePort()
        {
            //Power Off the target device
            if (!PowerOff())
                return E_FAIL;

            //Close port when all operations are complete, so it will be available for other applications
            string strError;
            int hr = pp.ClosePort(out strError);

            return hr;
        }

        int InitializePort()
        {
            int hr;

            //Setup Power - "3.3V" and internal for reset mode
            //Note: PSoC6 does not support 5V. Applying 5V to PSoC6 may corrupt the device
            if (!PowerOn("3.3")) return E_FAIL;

            //Set Acquire Mode
            hr = pp.SetAcquireMode("Reset", out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            //Set SWD protocol
            hr = pp.SetProtocol(enumInterfaces.SWD, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            //For MiniProg3/4 - correct connector should be chosen (5-pin or 10-pin)
            pp.SetProtocolConnector(connector10pin, out m_sLastError); //10-pin connector
            pp.SetProtocolClock(enumFrequencies.FREQ_03_0, out m_sLastError); //3.0 MHz clock on SWD bus

            return hr;
        }

        int EraseFlash()
        {
            // Erase Main Flash
            Console.WriteLine("Erasing of Main Flash Started...");
            int hr = pp.PSoC6_EraseAll(out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;
            Console.WriteLine("Erasing of Main Flash Succeeded");

            //Also Flash can be erased via PSoC6_EraseRow API
            //Erase entire Work Flash row by row
            Console.WriteLine("Erasing of Work Flash Started...");
            for (int rowAddr = PSOC6_WFLASH_ADDR; rowAddr < PSOC6_WFLASH_ADDR + PSOC6_WFLASH_SIZE; rowAddr += m_rowSize)
            {
                hr = pp.PSoC6_EraseRow(rowAddr, out m_sLastError);

                parent.SetProgressBar(progressBar, (rowAddr - PSOC6_WFLASH_ADDR) * 100 / (PSOC6_WFLASH_ADDR + PSOC6_WFLASH_SIZE) / m_rowSize);
                if (!SUCCEEDED(hr)) return hr;
            }
            Console.WriteLine("Erasing of Work Flash Succeeded");

            //Do not erase Supervisory Flash region, since there stored device configuration data
            //PSoC6 device may be corrupted if SFlash will be erased

            return hr;
        }

        int ProgramRegionFromHex(ProgramDeleg<int, string> ProgramOp, int address, int size)
        {
            //Determine first and last row IDs that are present in hex file
            int startRow, endRow;
            int hr = HEX_GetRegionBounds(address, size, m_rowSize, out startRow, out endRow);
            if (!SUCCEEDED(hr)) return hr;

            for (int i = startRow; i <= endRow; i++)
            {
                hr = ProgramOp(i, out m_sLastError);
                parent.SetProgressBar(progressBar, (i-startRow) * 100 / (endRow- startRow) / 2);
                if (!SUCCEEDED(hr)) return hr;
            }

            return hr;
        }

        int VerifyRegion(int address, int size, int sizeLimit, out bool result)
        {
            result = true;

            //Determine first and last row IDs that are present in hex file
            int startRow, endRow;
            int hr = HEX_GetRegionBounds(address, size, m_rowSize, out startRow, out endRow);
            if (!SUCCEEDED(hr)) return hr;

            int processedRowCount = 0;
            for (int i = startRow; i <= endRow; i++)
            {
                int verResult;
                hr = pp.PSoC6_VerifyRowFromHex(i, out verResult, out m_sLastError);
                if (verResult == 0)
                {
                    m_sLastError = "Verification failed on " + i + " row.";
                    result = false;
                    return hr;
                }

                parent.SetProgressBar(progressBar, 50 + (i-startRow) * 100 / (endRow - startRow) / 2);

                if (++processedRowCount * m_rowSize >= sizeLimit) break;
            }

            return hr;
        }

        int VerifyFlash(out bool result)
        {
            result = true;

            //Verify Main Flash region
            Console.WriteLine("Verification of Main Flash Started...");
            int hr = VerifyRegion(PSOC6_FLASH_ADDR, PSOC6_WFLASH_ADDR - PSOC6_FLASH_ADDR, PSOC6_FLASH_SIZE, out result);
            if (!SUCCEEDED(hr) || !result) return E_FAIL;
            Console.WriteLine("Verification of Main Flash Succeeded");

            //Verify Work Flash region
            Console.WriteLine("Verification of Work Flash Started...");
            hr = VerifyRegion(PSOC6_WFLASH_ADDR, PSOC6_WFLASH_SIZE, PSOC6_WFLASH_SIZE, out result);
            if (!SUCCEEDED(hr) || !result) return E_FAIL;
            Console.WriteLine("Verification of Work Flash Succeeded");

            //Verify Supervisory Flash region
            Console.WriteLine("Verification of Supervisory Flash Started...");
            hr = VerifyRegion(PSOC6_SFLASH_ADDR, PSOC6_SFLASH_SIZE, PSOC6_SFLASH_SIZE, out result);
            if (!SUCCEEDED(hr) || !result) return E_FAIL;
            Console.WriteLine("Verification of Supervisory Flash Succeeded");

            return hr;
        }

        int ProgramFlash()
        {
            // Use ProgramRow for Main Flash in erased state
            //If Main Flash was not erased or we need to program WFLASH or SFLASH - use WriteRow SROM API
            //It internally erases row prior to programming

            //Program Main Flash
            Console.WriteLine("Programming of Main Flash Started...");
            int hr = ProgramRegionFromHex(pp.PSoC6_ProgramRowFromHex, PSOC6_FLASH_ADDR, PSOC6_WFLASH_ADDR - PSOC6_FLASH_ADDR);
            if (!SUCCEEDED(hr)) return hr;
            Console.WriteLine("Programming of Main Flash Succeeded");

            //Program Work Flash
            Console.WriteLine("Programming of Work Flash Started...");
            hr = ProgramRegionFromHex(pp.PSoC6_WriteRowFromHex, PSOC6_WFLASH_ADDR, PSOC6_WFLASH_SIZE);
            if (!SUCCEEDED(hr)) return hr;
            Console.WriteLine("Programming of Work Flash Succeeded");

            //Program Supervisory Flash
            Console.WriteLine("Programming of Supervisory Flash Started...");
            hr = ProgramRegionFromHex(pp.PSoC6_WriteRowFromHex, PSOC6_SFLASH_ADDR, PSOC6_SFLASH_SIZE);
            if (!SUCCEEDED(hr)) return hr;
            Console.WriteLine("Programming of Supervisory Flash Succeeded");

            return hr;
        }

        int AcquireAndCheckDevice(string filePath)
        {
            //Port Initialization
            int hr = InitializePort();
            if (!SUCCEEDED(hr)) return hr;

            //Set Hex File
            int hexImageSize;
            hr = pp.HEX_ReadFile(filePath, out hexImageSize, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            //: 0x00 – autoreset disabled, 0x01 – autoreset enabled
            hr = pp.SetAutoReset(0x01, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            //Acquire Device
            hr = pp.DAP_Acquire(out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            //Check Hex File and Device compatibility
            bool fCompatibility;
            hr = CheckHexAndDeviceCompatibility(out fCompatibility);
            if (!SUCCEEDED(hr)) return hr;
            if (!fCompatibility)
            {
                m_sLastError = "The Hex file does not match the acquired device, please connect the appropriate device";
                return E_FAIL;
            }

            return hr;
        }

        int ProgramAll(string filePath)
        {
            // Acquire device and check compatibility with hex file
            int hr = AcquireAndCheckDevice(filePath);
            if (!SUCCEEDED(hr)) return hr;

            //Get row count and size for erase/program/verify operations
            hr = pp.PSoC6_GetFlashInfo(out m_rowsPerFlash, out m_rowSize, out m_sLastError);
            if (!SUCCEEDED(hr)) return hr;

            // Erase Main and Work Flash regions
            hr = EraseFlash();
            if (!SUCCEEDED(hr)) return hr;

            // Erase Main, Work and Supervisory Flash regions
            hr = ProgramFlash();
            if (!SUCCEEDED(hr)) return hr;

            // Verify actual device data with programmed
            bool result;
            hr = VerifyFlash(out result);
            if (!SUCCEEDED(hr) || !result) return E_FAIL;

            hr = pp.DAP_ReleaseChip(out m_sLastError);
            if (!SUCCEEDED(hr)) 
                return hr;

            return hr;
        }

        public int Execute(string filePath)
        {
            int hr = ProgramAll(filePath);
            
            //Close Port in the end, so port is available for other applications
            //ClosePort();

            return hr;
        }

    }
}
