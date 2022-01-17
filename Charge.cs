using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;

namespace CTT_4_TESTER
{
    class Charge
    {
        public Charging status;
        private string error = null;

        public enum Charging
        {
            NO_OP,
            WIRE,
            WIRE_CHARGED,
            WPC,
            WPC_CHARGED
        }

        public Charge(MsSerialPort msSerialPort)
        {
            string ret;
            this.error = null;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }
            ret = msSerialPort.sendStrUartCmd("CHARGE\r\n", "#>");
            CheckStringOk(ret);
            if (ret == null)
            {
                throw new Exception("Erreur de communication UART");
            }
            ret = msSerialPort.uartReadStringAndContains("CHARGE", ">");
            CheckString(ret);
        }

        public Charge(string str)
        {
            this.error = null;
            CheckString(str);
        }

        public bool waitStatus(MsSerialPort msSerialPort, Charging status,UInt32 timeout_ms)
        {
            bool bRet = false;

            for (uint i = 0; i < (timeout_ms / 100); i++)
            {
                string ret;
                this.error = null;
                if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
                {
                    throw new Exception("Erreur de communication UART");
                    //return;
                }
                ret = msSerialPort.sendStrUartCmd("CHARGE\r\n", "#>");
                CheckStringOk(ret);
                if (ret == null)
                {
                    throw new Exception("Erreur de communication UART");
                }
                ret = msSerialPort.uartReadStringAndContains("CHARGE", ">");
                CheckString(ret);
                if(this.status == status)
                {
                    bRet = true;
                    break;
                }
                Thread.Sleep(100);
            }
            if(bRet == false) throw new Exception("Error charge : " + status);
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

        private void CheckString(string str)
        {
            uint i = 0;
            string[] substrings = Regex.Matches(str, @"<([A-Za-z0-9 _.-]+)>").Cast<Match>().Select(m => m.Value).ToArray();

            // <VERSION GOLDEN_xxxx yyyyy> ou <VERSION xxxx yyyyy> //xxxx : firmware yyyy Board

            foreach (string match in substrings)
            {
                if (match.Contains("CHARGE"))
                {
                    string sub = match.Substring(1, match.Length - 2);
                    string[] spl = sub.Split(' ');
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (spl.Length == 2 && spl[1].EndsWith("NO_OP"))
                    {
                        status = Charging.NO_OP;
                    }
                    else if (spl.Length == 2 && spl[1].EndsWith("WIRE"))
                    {
                        status = Charging.WIRE;
                    }
                    else if (spl.Length == 2 && spl[1].EndsWith("WIRE_CHARGED"))
                    {
                        status = Charging.WIRE_CHARGED;
                    }
                    else if (spl.Length == 2 && spl[1].EndsWith("WPC"))
                    {
                        status = Charging.WPC;
                    }
                    else if (spl.Length == 2 && spl[1].EndsWith("WPC_CHARGED"))
                    {
                        status = Charging.WPC_CHARGED;
                    }
                    else
                    {
                        throw new Exception("Error in the STR for Charge " + str);
                    }
                }
                else
                {
                    throw new Exception("Error in the STR for Charge " + str);
                }
            }
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
            return "Charge = " + status;
        }
    }
}
