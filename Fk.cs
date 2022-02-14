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
    class Fk
    {
        public float value;
        private string error = null;
        int  rssiRx, rssiRxAvg;
        public Fk(MsSerialPort msSerialPort)
        {
            string ret;
            this.error = null;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }
            rssiRxAvg = 0;
            int errorTx = 0;
            for (int i = 0; i < (10 + errorTx); i++)
            {
                try
                {
                    ret = msSerialPort.uartReadStringAndContains("FK", ">");
                    CheckStringRssi(ret);
                    rssiRxAvg += rssiRx;
                }
                catch (Exception exc)
                {
                    if (errorTx > 4)
                    {
                        throw exc;
                    }
                    errorTx++;
                }
                Thread.Sleep(100);
            }
            rssiRxAvg /= 10;

            while (1==1)
            {
                try
                {
                    ret = msSerialPort.uartReadStringAndContains("FK", ">");
                }
                catch (Exception exc)
                {
                    break;
                }
            }
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

        private void CheckStringRssi(string str)
        {
            uint i = 0;
            string[] substrings = Regex.Matches(str, @"<([A-Za-z0-9 _.-]+)>").Cast<Match>().Select(m => m.Value).ToArray();

            // <FK -27>

            foreach (string match in substrings)
            {
                if (match.Contains("FK"))
                {
                    string sub = match.Substring(1, match.Length - 2);
                    string[] spl = sub.Split(' ');
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (spl.Length == 2 &&
                        int.TryParse(spl[1], NumberStyles.Integer, provider, out int val1))
                    {
                        rssiRx = val1;
                        return;
                    }else
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

        public bool IsRssiOk()
        {
            if (rssiRxAvg > -80)
            {
                return true;
            }
            else return false;
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
            return "FK = " + rssiRxAvg;
        }
    }
}
