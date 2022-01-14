using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CTT_4_TESTER
{
    class Version
    {
        public string board_version;
        public string firmware_version;
        public bool golden = false;
        private string error = null;

        public Version(MsSerialPort msSerialPort)
        {
            string ret;
            this.error = null;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }
            ret = msSerialPort.sendStrUartCmd("VERSION\r\n", "#>");
            if (ret == null)
            {
                throw new Exception("Erreur de communication UART");
            }
            CheckString(ret);
        }

        public Version(string str)
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
                if (match.Contains("VERSION"))
                {
                    string sub = match.Substring(1, match.Length - 2);
                    string[] spl = sub.Split(' ');
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (spl.Length == 3)// && UInt32.TryParse(spl[1], NumberStyles.HexNumber, provider, out level))
                    {
                        string strVersion = "" + spl[1];
                        if (strVersion.StartsWith("GOLDEN_"))
                        {
                            strVersion = "" + strVersion.Substring(7);
                            golden = true;
                        }
                        firmware_version = "" + strVersion;
                        board_version = "" + spl[2];
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

        public bool IsWpcBoard()
        {
            bool ret = true;
            if (board_version.Contains("NHT"))ret = false;
            return ret;
        }

        public string toString()
        {
            return "version = " + firmware_version + " GOLDEN :" + golden + " " + board_version;
        }
    }
}
