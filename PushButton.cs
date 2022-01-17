using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CTT_4_TESTER
{
    class PushButton
    {
        private string error = null;
        private bool[] switchStatus = new bool[7];
        P4CttTestForm parent;
        public enum SWITCH
        {
            SW_1A,
            SW_1B,
            SW_2A,
            SW_2B,
            SW_CFG,
            SW_DEC,
            SW_INC
        }

        public PushButton(MsSerialPort msSerialPort)
        {
            string ret;
            this.error = null;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }
            ret = msSerialPort.sendStrUartCmd("SWITCH 1\r\n", "#>");
            CheckStringOk(ret);
        }

        public PushButton(string str)
        {
            this.error = null;
            CheckString(str);
        }

        public bool waitStatus(MsSerialPort msSerialPort, P4CttTestForm parent, UInt32 timeout_ms)
        {
            bool bRet = false;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }

            for (uint i = 0; i < (timeout_ms / 100); i++)
            {
                string ret;
                this.error = null;
                
                ret = msSerialPort.sendStrUartCmd("SWITCH 1\r\n", "#>");
                CheckStringOk(ret);
                if (ret == null)
                {
                    throw new Exception("Erreur de communication UART");
                }
                ret = msSerialPort.uartReadStringAndContains("SWITCH", ">");
                if (CheckString(ret))
                {
                    bRet = true;
                    parent.setSwitchState(switchStatus);
                    break;
                }

                parent.setSwitchState(switchStatus);
                Thread.Sleep(50);
            }
            if (bRet == false) throw new Exception("Error switch ");
            return bRet;

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
                    error = "OK";
                }
                else
                {
                    throw new Exception("Error in the STR for Version " + str);
                }
            }
        }

        private bool CheckString(string str)
        {
            uint i = 0;
            string[] substrings = Regex.Matches(str, @"<([A-Za-z0-9 _.-]+)>").Cast<Match>().Select(m => m.Value).ToArray();
            bool ret = true;
            // <VERSION GOLDEN_xxxx yyyyy> ou <VERSION xxxx yyyyy> //xxxx : firmware yyyy Board

            foreach (string match in substrings)
            {
                if (match.Contains("SWITCH"))
                {
                    string sub = match.Substring(1, match.Length - 2);
                    string[] spl = sub.Split(' ');
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (spl.Length == 8)
                        for(uint j = 1; j < 8; j++)
                        {
                            if(uint.TryParse(spl[j], NumberStyles.Float, provider, out uint level))
                            {
                                if (level == 0)
                                {
                                    switchStatus[j-1] = false;
                                    ret = false;
                                }
                                else switchStatus[j-1] = true;
                            }
                            else throw new Exception("Error in the STR for Switch " + str);
                        }
                    else
                    {
                        throw new Exception("Error in the STR for Switch " + str);
                    }
                }
                else
                {
                    throw new Exception("Error in the STR for Switch " + str);
                }
            }
            return ret;
        }

        public bool IsError()
        {
            if (error == null)
                return false;
            else
                return true;
        }

        public string GetError()
        {
            return error;
        }

        public string toString()
        {
            return "Switch = " + switchStatus;
        }
    }
}
