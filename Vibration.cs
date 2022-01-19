using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CTT_4_TESTER
{
    class Vibration
    {
        private string status = "error";

        public Vibration(MsSerialPort msSerialPort, int u)
        {
            string ret;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }
            if (u == 0)
            {
                ret = msSerialPort.sendStrUartCmd("VIB OFF\r\n", "#>");
            }
            else if (u == 1)
            {
                ret = msSerialPort.sendStrUartCmd("VIB ON\r\n", "#>");
            }
            else throw new Exception("Erreur arg vibration");

            if (ret == null)
            {
                throw new Exception("Erreur de communication UART");
            }
            CheckString(ret);
        }

        public Vibration(string str)
        {
            this.status = "error";
            CheckString(str);
        }

        private void CheckString(string str)
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

        public string toString()
        {
            return "Vibration " + status;
        }
    }
}

