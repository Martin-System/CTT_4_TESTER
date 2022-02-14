using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CTT_4_TESTER
{
    class Sn
    {
        int value;

        private string status = "error";

        public Sn(MsSerialPort msSerialPort, int u)//u=0 -> read, else write
        {
            string ret;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }
            if (u == 0)
            {
                ret = msSerialPort.sendStrUartCmd("SN\r\n", "#>");
            }
            else
            {
                ret = msSerialPort.sendStrUartCmd("SN " + u + "\r\n", "#>");
            }


            if (ret == null)
            {
                throw new Exception("Erreur de communication UART");
            }
            CheckString(ret);
        }

        public Sn(string str)
        {
            this.status = "error";
            CheckString(str);
        }

        private void CheckString(string str)
        {
            uint i = 0;
            string[] substrings = Regex.Matches(str, @"<([A-Za-z0-9 _.-]+)>").Cast<Match>().Select(m => m.Value).ToArray();

            // <SN ....>

            foreach (string match in substrings)
            {
                if (match.Contains("SN"))
                {
                    string sub = match.Substring(1, match.Length - 2);
                    string[] spl = sub.Split(' ');
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (spl.Length == 2 && int.TryParse(spl[1], NumberStyles.Integer, provider, out int level))
                    {
                        value = level;
                        status = "OK " + value;
                    }
                    else
                    {
                        throw new Exception("Error in the STR for SN " + str);
                    }
                }
                else
                {
                    throw new Exception("Error in the STR for SN " + str);
                }
            }
        }

        public string toString()
        {
            return "SN " + status;
        }
    }
}
