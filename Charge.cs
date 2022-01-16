using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace P4_TestCollar
{
    class Charge
    {
        public int value;
        private string error = null;

        enum Charging
        {
            NO_CHARGE,
            CHARGING,
            CHARGED
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
            if (ret == null)
            {
                throw new Exception("Erreur de communication UART");
            }
            CheckString(ret);
        }

        public Charge(string str)
        {
            this.error = null;
            CheckString(str);
        }

        private void CheckString(string str)
        {
            uint i = 0;
            string[] substrings = Regex.Matches(str, @"<([A-Za-z0-9 _.-]+)>").Cast<Match>().Select(m => m.Value).ToArray();

            // <VERSION GOLDEN_xxxx yyyyy> ou <VERSION xxxx yyyyy> //xxxx : firmware yyyy Board

            foreach (string match in substrings)
            {
                if (match.Contains("OK"))
                {
                    string sub = match.Substring(1, match.Length - 2);
                    string[] spl = sub.Split(' ');
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (spl.Length == 2 && int.TryParse(spl[1], NumberStyles.Integer, provider, out int level))
                    {
                        value = level;
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
            return "Charge = " + value;
        }
    }
}
