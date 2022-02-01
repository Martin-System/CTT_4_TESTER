using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CTT_4_TESTER
{
    class Mac
    {
        public string publicAddress;
        public bool connected = false;

        private string error = null;

        public Mac(MsSerialPort msSerialPort)
        {
            string ret;
            this.error = null;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }
            ret = msSerialPort.sendStrUartCmd("MAC\r\n", "#>");
            CheckStringOk(ret);
            if (ret == null)
            {
                throw new Exception("Erreur de communication UART");
            }
            ret = msSerialPort.uartReadStringAndContains("MAC", ">");
            CheckString(ret);
        }

        public Mac(MsSerialPort msSerialPort, string macAddress)
        {
            string ret;
            this.error = null;
            connected = false;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }
            ret = msSerialPort.sendStrUartCmd("MAC " + macAddress + "\r\n", "#>");
            CheckStringOk(ret);
            if (ret == null)
            {
                throw new Exception("Erreur de communication UART");
            }
            
            Thread.Sleep(1000);
            uint i;
            for (i = 0; i < 5; i++)
            {
                try
                {
                    ret = msSerialPort.uartReadStringAndContains("MAC", ">");
                    break;
                }
                catch (Exception exc)
                {
                    if (i == 4) throw new Exception("error connection BLE", exc);
                }
            }
            CheckString(ret);

            for (i = 0; i < 5; i++) 
            { 
                try
                {
                    ret = msSerialPort.uartReadStringAndContains("CONNECTED", ">");
                    break;
                }
                catch (Exception exc)
                {
                    if (i == 4) throw new Exception("error connection BLE", exc); 
                }
            }
            CheckStringConnected(ret);
            connected = true;
        }

        public Mac(string str)
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
                    throw new Exception("Error in the STR for MAC " + str);
                }
            }
        }

        private void CheckString(string str)
        {
            uint i = 0;
            string[] substrings = Regex.Matches(str, @"<([A-Za-z0-9 _.-]+)>").Cast<Match>().Select(m => m.Value).ToArray();

            // <MAC xxxx> //xxxx : mac address

            foreach (string match in substrings)
            {
                if (match.Contains("MAC"))
                {
                    string sub = match.Substring(1, match.Length - 2);
                    string[] spl = sub.Split(' ');
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (spl.Length == 2)
                    {
                        publicAddress = spl[1];
                    }
                    else
                    {
                        throw new Exception("Error in the STR for Mac " + str);
                    }
                }
                else
                {
                    throw new Exception("Error in the STR for Mac " + str);
                }
            }
        }
        private void CheckStringConnected(string str)
        {
            uint i = 0;
            string[] substrings = Regex.Matches(str, @"<([A-Za-z0-9 _.-]+)>").Cast<Match>().Select(m => m.Value).ToArray();

            // <CONNECTED>

            foreach (string match in substrings)
            {
                if (match.Contains("CONNECTED"))
                {
                connected = true;
                }
                else
                {
                    throw new Exception("Error in the STR for Mac Connection " + str);
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
            return "Mac = " + publicAddress + " connected : " + connected;
        }
    }
}
