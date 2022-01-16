using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CTT_4_TESTER
{
    class Battery
    {
        public float value;
        private string error = null;

        public Battery(MsSerialPort msSerialPort)
        {
            string ret;
            this.error = null;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }
            ret = msSerialPort.sendStrUartCmd("BATTERY\r\n", "#>");
            CheckStringOk(ret);
            if (ret == null)
            {
                throw new Exception("Erreur de communication UART");
            }
            ret = msSerialPort.uartReadStringAndContains("BATTERY", ">");
            CheckString(ret);
        }

        public Battery(string str)
        {
            this.error = null;
            CheckString(str);
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
                if (match.Contains("BATTERY"))
                {
                    string sub = match.Substring(1, match.Length - 2);
                    string[] spl = sub.Split(' ');
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (spl.Length == 2 && float.TryParse(spl[1], NumberStyles.Float, provider, out float level))
                    {
                        value = level;
                    }
                    else
                    {
                        throw new Exception("Error in the STR for Battery " + str);
                    }
                }
                else
                {
                    throw new Exception("Error in the STR for Battery " + str);
                }
            }
        }

        public bool IsValueOk(float toTest, float tolerance)
        {
            if (this.value > (toTest * (1 + tolerance)) || this.value < (toTest * (1 - tolerance)))
            {
                return false;
            }
            else return true;
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
            return "Battery = " + value;
        }
    }
}
