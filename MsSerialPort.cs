using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace P4_TestCollar
{
    class MsSerialPort
    {
        SerialPort _serialPort;
        String error = null;
        int nbrRetryCmdUart = 5;
        TextBox tboxSerialInOut;
        P4CollarTestForm parentForm;

        public MsSerialPort(string portName)
        {
            init();
            // Allow the user to set the appropriate properties.
            _serialPort.PortName = portName;
        }
        public MsSerialPort(TextBox tInOutBox, P4CollarTestForm parent)
        {
            init();
            // Allow the user to set the appropriate properties.
            tboxSerialInOut = tInOutBox;
            parentForm = parent;
        }

        public MsSerialPort()
        {
            init();
        }

        private void init()
        {
            _serialPort = new SerialPort();
            _serialPort.BaudRate = 57600;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Parity = Parity.None;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 4000;
            _serialPort.WriteTimeout = 4000;
        }

        public bool Open(string portName)
        {
            bool status = true;
            _serialPort.PortName = portName;
            try
            {
                _serialPort.Open();
            }
            catch (Exception e)
            {
                error = "error open" + e.Message;
                status = false;
            }
            return status;

        }

        public bool Close()
        {
            bool status = true;
            try
            {
                _serialPort.Close();
                _serialPort.Dispose();
            }
            catch (Exception e)
            {
                error = "error close" + e.Message;
                status = false;
            }
            return status;

        }

        public string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        public string sendRcvUartCmd(string strCmd)
        {
            uint retry;
            string strAnswer;
            error = null;
            retry = 0;
            while (retry < nbrRetryCmdUart)// && backgroundWorker.CancellationPending == false)
            {
                try
                {
                    uartWriteStr(strCmd);
                    strAnswer = uartReadStr();
                    return strAnswer;
                }
                catch (Exception e)
                {
                    error += e;
                    retry++;
                    parentForm.SetText(tboxSerialInOut, "exc :" + e);
                }
            }
            return null;
        }

        public string sendStartUartCmd(string strCmd)
        {
            uint retry;
            string str = null;
            error = null;
            retry = 0;
            while (retry < nbrRetryCmdUart)// && backgroundWorker.CancellationPending == false)
            {
                try
                {
                    uartWriteStr(strCmd);
                    str = uartReadStart();
                    return str;
                }
                catch (Exception e)
                {
                    error += e;
                    retry++;
                }
            }
            return str;
        }

        public string sendStrUartCmd(string strCmd, string strEndAnswer)
        {
            uint retry;
            string str = null;
            error = null;
            retry = 0;
            while (retry < nbrRetryCmdUart)// && backgroundWorker.CancellationPending == false)
            {
                try
                {
                    uartWriteStr(strCmd);
                    str = uartReadString(strEndAnswer);
                    return str;
                }
                catch (Exception e)
                {
                    error += e;
                    retry++;
                    parentForm.SetText(tboxSerialInOut, error);
                }
            }
            return str;
        }

        public string sendStrAndContainsUartCmd(string strCmd, string strContains, string strEndAnswer)
        {
            uint retry;
            string str = null;
            error = null;
            retry = 0;
            while (retry < nbrRetryCmdUart)// && backgroundWorker.CancellationPending == false)
            {
                try
                {
                    uartWriteStr(strCmd);
                    str = uartReadStringAndContains(strContains, strEndAnswer);
                    return str;
                }
                catch (Exception e)
                {
                    error += e;
                    retry++;
                    parentForm.SetText(tboxSerialInOut, error);
                }
            }
            return str;
        }

        public string rcvLineUart()
        {

            string str = null;
            error = null;
            try
            {
                str = uartReadLine();
            }
            catch (Exception e)
            {
                error += e;
            }

            return str;
        }

        public int uartWriteStr(string str)
        {
            int status = 0;
            int len = str.Length;


            // SetText(textBox_StatusLog1, "\r\n write ->" + str +"len :"+len);
            byte[] toBytes = Encoding.ASCII.GetBytes(str);
            try
            {
                int i;
                for (i = 0; i < len; i++)
                {
                    _serialPort.Write(toBytes, i, 1);
                    var t = Task.Run(async delegate
                    {
                        await Task.Delay(10);
                        return 42;
                    });
                    t.Wait();
                }
                parentForm.SetText(tboxSerialInOut, str);

            }
            catch (Exception exc)
            {
                error = " Error in writing data \r\n " + exc.Message + "\r\n";
                parentForm.SetText(tboxSerialInOut, error);
                // status = (int)ReturnCodes.CYRET_ERR_COMM_MASK;
            }


            return status;
        }

        private string uartReadStr()
        {
            string str = "";

            try
            {
                while (1 == 1)
                {
                    int readByte = _serialPort.ReadByte();

                    if (readByte > 0)
                    {
                        char ch = (char)readByte;
                        parentForm.SetText(tboxSerialInOut, "" + ch);
                        if (ch == '\n') str += Environment.NewLine;
                        else str += (char)readByte;

                        if (str.Length > 2 &&
                            str[str.Length - 1] == '>' &&
                            str[str.Length - 2] == '#'
                           )
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                /* Return error if an exception occurs */
                error = " Error in uartReadStr \r\n " + exc.Message + "\r\n";
                parentForm.SetText(tboxSerialInOut, error);
                throw new Exception("uartReadStr ", exc);
            }


            return str;
        }

        private string uartReadLine()
        {
            string str = "";

            try
            {
                while (1 == 1)
                {
                    int readByte = _serialPort.ReadByte();

                    if (readByte > 0)
                    {
                        char ch = (char)readByte;
                        parentForm.SetText(tboxSerialInOut, "" + ch);
                        if (ch == '\n')
                        {
                            str += Environment.NewLine;
                            break;
                        }
                        else str += (char)readByte;

                    }
                }
            }
            catch (Exception exc)
            {
                /* Return error if an exception occurs */
                error = " Error in uartReadLine\r\n " + str + "\r\n" + exc.Message + "\r\n";
                throw new Exception(error, exc);
            }



            return str;
        }

        public string uartReadString(string endStr)
        {
            string str = "";
            string line = "";

            try
            {
                while (1 == 1)
                {
                    int readByte = _serialPort.ReadByte();

                    if (readByte > 0)
                    {
                        char ch = (char)readByte;
                        parentForm.SetText(tboxSerialInOut, "" + ch);
                        if (ch == '\n')
                        {
                            str += Environment.NewLine;
                            line += Environment.NewLine;
                        }
                        else
                        {
                            line += (char)readByte;
                            str += (char)readByte;
                            line = "";
                        }

                        if (str.Length >= endStr.Length)
                        {
                            int i = 0;
                            for (i = 0; i < endStr.Length; i++)
                            {
                                if (str[str.Length - (1 + i)] != endStr[endStr.Length - (1 + i)])
                                {
                                    break;
                                }
                            }
                            if (i == endStr.Length) break;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                /* Return error if an exception occurs */
                error = " Error in uartReadString\r\n " + str + "\r\n" + exc.Message + "\r\n";
                throw new Exception(error, exc);
            }


            return str;
        }

        public string uartReadStringAndContains(string contains, string endStr)
        {
            string str = "";
            string line = "";

            try
            {
                while (1 == 1)
                {
                    int readByte = _serialPort.ReadByte();

                    if (readByte > 0)
                    {
                        char ch = (char)readByte;
                        parentForm.SetText(tboxSerialInOut, "" + ch);
                        if (ch == '\n')
                        {
                            str += Environment.NewLine;
                            line += Environment.NewLine;
                        }
                        else
                        {
                            line += (char)readByte;
                            str += (char)readByte;
                            line = "";
                        }

                        if (str.Contains(contains) && str.Length > endStr.Length)
                        {
                            int i = 0;
                            for (i = 0; i < endStr.Length; i++)
                            {
                                if (str[str.Length - (1 + i)] != endStr[endStr.Length - (1 + i)])
                                {
                                    break;
                                }
                            }
                            if (i == endStr.Length) break;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                /* Return error if an exception occurs */
                error = " Error in uartReadStringAndContains\r\n " + str +"\r\n"+ exc.Message + "\r\n";
                throw new Exception(error, exc);
            }


            return str;
        }

        private string uartReadStart()
        {
            string str = "";
            string line = "";

            try
            {
                while (1 == 1)
                {
                    int readByte = _serialPort.ReadByte();

                    if (readByte > 0)
                    {
                        char ch = (char)readByte;
                        parentForm.SetText(tboxSerialInOut, "" + ch);
                        if (ch == '\n')
                        {
                            str += Environment.NewLine;
                            line += Environment.NewLine;
                        }
                        else
                        {
                            line += (char)readByte;
                            str += (char)readByte;
                            line = "";
                        }

                        if (str.Length >= 9 &&
                            str[str.Length - 7] == '<' &&
                            str[str.Length - 6] == 's' &&
                            str[str.Length - 5] == 't' &&
                            str[str.Length - 4] == 'a' &&
                            str[str.Length - 3] == 'r' &&
                            str[str.Length - 2] == 't' &&
                            str[str.Length - 1] == '>') break;
                    }
                }
            }
            catch (Exception exc)
            {
                /* Return error if an exception occurs */
                error = " Error in uartReadStart\r\n " + exc.Message + "\r\n";
                throw new Exception("uartReadStart ", exc);
            }


            return str;
        }


    }
}
