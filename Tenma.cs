using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Globalization;

namespace CTT_4_TESTER
{
    class Tenma
    {
        SerialPort tenma;
        String error = null;

        public Tenma(string portName)
        {
            init();
            // Allow the user to set the appropriate properties.
            tenma.PortName = portName;
        }


        public Tenma()
        {
            init();
        }

        private void init()
        {
            tenma = new SerialPort();
            tenma.BaudRate = 9600;
            tenma.DataBits = 8;
            tenma.StopBits = StopBits.One;
            tenma.Parity = Parity.None;
            tenma.Handshake = Handshake.None;

            // Set the read/write timeouts
            tenma.ReadTimeout = 200;
            tenma.WriteTimeout = 200;
        }

        public void Open(string portName)
        {
            tenma.PortName = portName;
            try
            {
                tenma.Open();
            }
            catch (Exception e)
            {
                throw new Exception("Tenma error open" + e.Message);
            }
        }

        public void Close()
        {
            try
            {
                tenma.Close();
                tenma.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Tenma error close" + e.Message);
            }
        }

        public string[] GetPortNames()
        {
            try
            {
                return SerialPort.GetPortNames();
            }
            catch (Exception e)
            {
                throw new Exception("Tenma GetPortNames" + e.Message);
            }
        }

        private string writeReadCmd(string cmd)
        {
            string rep = "";
            try
            {
                //byte[] toBytes = Encoding.ASCII.GetBytes(cmd);
                tenma.Write(cmd);
                while (1 == 1)
                {
                    int readByte = tenma.ReadByte();

                    if (readByte > 0)
                    {
                        rep += (char)readByte;
                    }
                }
            }
            catch (TimeoutException e)
            {
                if (rep.Equals(""))
                {
                    throw new Exception("" + rep + "\n" + e.Message);
                }
                else
                {
                    return rep;
                }

            }
            catch (Exception e)
            {
                throw new Exception("" + rep + "\n" + e.Message);
            }
        }

        private void writeCmd(string cmd)
        {
            try
            {
                //byte[] toBytes = Encoding.ASCII.GetBytes(cmd);
                tenma.Write(cmd);

            }
            catch (Exception e)
            {
                throw new Exception("" + "\n" + e.Message);
            }
        }

        public string getIdentification()
        {
            try
            {
                return writeReadCmd("*IDN?");
            }
            catch (Exception e)
            {
                throw new Exception("Tenma GetID: " + e.Message);
            }
        }

        public void setCurrent(float current)
        {
            try
            {
                writeCmd("ISET1:" + current.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception e)
            {
                throw new Exception("Tenma Set Current: " + e.Message);
            }
        }
        public float getCurrent()
        {
            try
            {
                string answer = writeReadCmd("ISET1?");
                float current;
                //ISET1:2.225
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (float.TryParse(answer, NumberStyles.Float, provider, out current))
                {
                    return current;
                }
                else throw new Exception("Tenma get Current: error answer : " + answer);
            }
            catch (Exception e)
            {
                throw new Exception("Tenma Get Current: " + e.Message);
            }
        }
        public void setVoltage(float voltage)
        {
            try
            {
                writeCmd("VSET1:" + voltage.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception e)
            {
                throw new Exception("Tenma Set Voltage: " + e.Message);
            }
        }
        public float getVoltage()
        {
            try
            {
                string answer = writeReadCmd("VSET1?");
                float voltage;
                //VSET1:2.225
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (float.TryParse(answer, NumberStyles.Float, provider, out voltage))
                {
                    return voltage;
                }
                else throw new Exception("Tenma get Voltage: error answer : " + answer);
            }
            catch (Exception e)
            {
                throw new Exception("Tenma Get Voltage " + e.Message);
            }
        }

        public float getIOut()
        {
            try
            {
                string answer = writeReadCmd("IOUT1?");
                float current;
                //ISET1:2.225
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (float.TryParse(answer, NumberStyles.Float, provider, out current))
                {
                    return current;
                }
                else throw new Exception("Tenma get IOUT: error answer : " + answer);
            }
            catch (Exception e)
            {
                throw new Exception("Tenma Get IOUT: " + e.Message);
            }
        }

        public float getVOut()
        {
            try
            {
                string answer = writeReadCmd("VOUT1?");
                float voltage;
                //ISET1:2.225
                CultureInfo provider = CultureInfo.InvariantCulture;
                if (float.TryParse(answer, NumberStyles.Float, provider, out voltage))
                {
                    return voltage;
                }
                else throw new Exception("Tenma get VOUT: error answer : " + answer);
            }
            catch (Exception e)
            {
                throw new Exception("Tenma Get VOUT: " + e.Message);
            }
        }
        public void On()
        {
            try
            {
                writeCmd("OUT1");
            }
            catch (Exception e)
            {
                throw new Exception("Tenma ON: " + e.Message);
            }
        }
        public void Off()
        {
            try
            {
                writeCmd("OUT0");
            }
            catch (Exception e)
            {
                throw new Exception("Tenma OFF: " + e.Message);
            }
        }
    }
}
