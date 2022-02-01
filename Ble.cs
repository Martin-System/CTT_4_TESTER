﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CTT_4_TESTER
{
    class Ble
    {
        public int value;
        public int valueAvg;
        private string error = null;

        public Ble(MsSerialPort msSerialPort)
        {
            string ret;
            this.error = null;
            if (msSerialPort.sendStrUartCmd("m\r\n", "#>") == null)
            {
                throw new Exception("Erreur de communication UART");
                //return;
            }
            valueAvg = 0;
            valueAvg = 0;
            int errorTx = 0;
            for (int i = 0; i < (10 + errorTx); i++)
            {                
                ret = msSerialPort.sendStrUartCmd("BLE R\r\n", "#>");
                if (ret == null)
                {
                    throw new Exception("Erreur de communication UART");
                }
                try
                {
                    ret = msSerialPort.uartReadStringAndContains("RSSI_BLE", ">");
                    CheckString(ret);
                    valueAvg += value;
                }
                catch (Exception exc)
                {
                    if (errorTx > 2)
                    {
                        throw exc;
                    }
                    errorTx++;
                }
                Thread.Sleep(100);
            }
            valueAvg /= 10;

            
        }

        public Ble(string str)
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
                if (match.Contains("RSSI_BLE"))
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
                        throw new Exception("Error in the STR for Ble " + str);
                    }
                }
                else
                {
                    throw new Exception("Error in the STR for Ble " + str);
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

        public bool IsValueOk()
        {
            if (error == null)
            {
                if (valueAvg > -80)
                {
                    return true;
                }
                else return false;
            }                
            else
                throw new Exception("Error in the comunication for BLE");
        }

        public string GetError()
        {
            return error;
        }

        public string toString()
        {
            return "BLE = " + valueAvg;
        }
    }
}
