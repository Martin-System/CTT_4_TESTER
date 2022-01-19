using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace CTT_4_TESTER
{
    class Sx
    {
        int step = 200;
        int bigStep = 1000;

        int rssiTx, rssiRx, rssiTxAvg, rssiRxAvg;
        public enum CMD
        {
            START_CW,
            STOP_CW,
            ERROR_CW,
            INC_CW,
            INCP_CW,
            DEC_CW,
            DECP_CX,
            PAIRING,
            TEST_RSSI,
            LISTEN
        }

        private string status = "error";

        public Sx(MsSerialPort msSerialPort, CMD cmd)//u=-2:Dec P, u=-1 : Dec u=1 : IncP, u=2 Inc P, u=0, Start
        {
            string ret;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }
            switch (cmd)
            {
                case CMD.START_CW:
                    ret = msSerialPort.sendStrUartCmd("SX C\r\n", "#>"); 
                    CheckStringOk(ret);
                    break;
                case CMD.STOP_CW:
                    ret = msSerialPort.sendStrUartCmd("SX i\r\n", "#>");
                    CheckStringOk(ret);
                    break;
                case CMD.INC_CW:
                    ret = msSerialPort.sendStrUartCmd("SX C " + this.step + "\r\n", "#>");
                    CheckStringOk(ret);
                    break;
                case CMD.INCP_CW:
                    ret = msSerialPort.sendStrUartCmd("SX C " + this.bigStep + "\r\n", "#>");
                    CheckStringOk(ret);
                    break;
                case CMD.DEC_CW:
                    ret = msSerialPort.sendStrUartCmd("SX C " + (-this.step) + "\r\n", "#>");
                    CheckStringOk(ret);
                    break;
                case CMD.DECP_CX:
                    ret = msSerialPort.sendStrUartCmd("SX C " + (-this.bigStep) + "\r\n", "#>");
                    CheckStringOk(ret);
                    break;
                case CMD.PAIRING:
                    ret = msSerialPort.sendStrUartCmd("SX P\r\n", "#>");
                    CheckStringOk(ret);
                    break;
                case CMD.TEST_RSSI:
                    rssiTxAvg = 0;
                    rssiRxAvg = 0;
                    int  errorTx = 0;
                    for(int i = 0; i < (10+errorTx); i++)
                    {
                        ret = msSerialPort.sendStrUartCmd("SX T\r\n", "#>");
                        CheckStringOk(ret);
                        try
                        {
                            ret = msSerialPort.uartReadStringAndContains("SX", ">");
                            CheckStringRssi(ret);
                            rssiTxAvg += rssiTx;
                            rssiRxAvg += rssiRx;
                        }
                        catch(Exception exc)
                        {
                            if (errorTx > 4)
                            {
                                throw exc;
                            }
                            errorTx++;
                        }
                        Thread.Sleep(100);
                    }
                    rssiTxAvg /= 10;
                    rssiRxAvg /= 10;
                    break;
                case CMD.LISTEN:
                    ret = msSerialPort.sendStrUartCmd("SX L\r\n", "#>");
                    CheckStringOk(ret);
                    break;
            }

            
        }

        public Sx(MsSerialPort msSerialPort, double freqMHz)
        {
            double deltaMhz = 869.5 - freqMHz;
            string ret;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }
            ret = msSerialPort.sendStrUartCmd("SX C " + Math.Round(deltaMhz*1e6,0) + "\r\n", "#>");
            CheckStringOk(ret);
        }

            public Sx(string str)
        {
            this.status = "error";
            CheckStringOk(str);
        }

        private void CheckStringOk(string str)
        {
            uint i = 0;
            string[] substrings = Regex.Matches(str, @"<([A-Za-z0-9 _.-]+)>").Cast<Match>().Select(m => m.Value).ToArray();

            // <OK>

            foreach (string match in substrings)
            {
                if (match.Contains("OK"))
                {
                    status = "OK";
                }
                else
                {
                    throw new Exception("Error in the STR for Version " + str);
                }
            }
        }

        private void CheckStringRssi(string str)
        {
            uint i = 0;
            string[] substrings = Regex.Matches(str, @"<([A-Za-z0-9 _.-]+)>").Cast<Match>().Select(m => m.Value).ToArray();

            // <SX RSSI -28 -27> or  <SX ACK_TO>

            foreach (string match in substrings)
            {
                if (match.Contains("SX"))
                {
                    string sub = match.Substring(1, match.Length - 2);
                    string[] spl = sub.Split(' ');
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (spl.Length == 4 && 
                        spl[1].Contains("RSSI") &&
                        int.TryParse(spl[2], NumberStyles.Integer, provider, out int val1) &&
                        int.TryParse(spl[3], NumberStyles.Integer, provider, out int val2))
                    {
                        rssiRx = val1;
                        rssiTx = val2;
                        return;
                    }
                    else if (spl.Length == 2 &&
                             spl[1].Contains("ACK_TO"))
                    {
                        throw new Exception("Time Out");
                    }
                    else
                    {
                        throw new Exception("Error in the STR for Version " + str);
                    }
                }
                else
                {
                    throw new Exception("Error in the STR for Version " + str);
                }
            }
            throw new Exception("Error in the STR for Version " + str);
        }
        public string toStringRSSI ()
        {
            return "SX RX " + rssiRxAvg + "TX " + rssiTxAvg;
        }

        public bool IsRssiOk()
        {
            if (rssiRxAvg > -55 && rssiTxAvg > -55)
            {
                return true;
            }
            else return false;
        }

        public string toString()
        {
            return "SX " + status;
        }
    }
}

