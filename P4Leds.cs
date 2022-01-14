using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Automation.BDaq;

namespace CTT_4_TESTER
{
    class P4Leds
    {
        InstantAiCtrl aiCtrl;
        double[] m_dataScaled;
        P4Led ledSensor1, ledSensor2;

        double[] vIn;
        int chanCount = 8;
        int sectionLength = 128;

        TaskCompletionSource<bool> tcs = null;

        public P4Leds(InstantAiCtrl aiCtrl, int deviceNumber)
        {
            this.aiCtrl = aiCtrl;
            this.aiCtrl.SelectedDevice = new DeviceInformation(deviceNumber);

            //int chanCount = this.aiCtrl.Conversion.ChannelCount;
            //int sectionLength = this.aiCtrl.Record.SectionLength;
            m_dataScaled = new double[chanCount * sectionLength];
            ledSensor1 = new P4Led(1, 0);
            ledSensor2 = new P4Led(3, 2);
        }

        public void startAdc()
        {
            int i, j;
            for(i=0; i< sectionLength; i++)
            {
                for (j = 0; j < chanCount; j++)
                {
                    if (aiCtrl.Read(j, out m_dataScaled[chanCount * i + j]) != ErrorCode.Success) throw new Exception("Error ADC acquisition");
                }
            }

            ledSensor1.setArrayColor(m_dataScaled);
            ledSensor1.setArrayIntensity(m_dataScaled);
            ledSensor2.setArrayColor(m_dataScaled);
            ledSensor2.setArrayIntensity(m_dataScaled);
            setVin(m_dataScaled);
            System.Diagnostics.Debug.WriteLine("sensor 1 color " + ledSensor1.getColorAverage() + " " + ledSensor1.getColor().ToString());
            System.Diagnostics.Debug.WriteLine("sensor 1 intensity " + ledSensor1.getIntensityAverage());
            System.Diagnostics.Debug.WriteLine("sensor 2 color " + ledSensor2.getColorAverage() + " " + ledSensor2.getColor().ToString());
            System.Diagnostics.Debug.WriteLine("sensor 2 intensity " + ledSensor2.getIntensityAverage());
            System.Diagnostics.Debug.WriteLine("Vin " + getVinAverage());
        }

   /*     public ErrorCode dataReady(object sender, BfdAiEventArgs args)
        {
            
            try
            {
                ErrorCode err = ErrorCode.ErrorFuncNotInited;
                //The WaveformAiCtrl has been disposed.
                if (this.aiCtrl.State == ControlState.Idle)
                {
                    return err;
                }
                if (m_dataScaled.Length < args.Count)
                {
                    m_dataScaled = new double[args.Count];
                }

                
                int chanCount = this.aiCtrl.Conversion.ChannelCount;
                int sectionLength = this.aiCtrl.Record.SectionLength;
                err = this.aiCtrl.GetData(args.Count, m_dataScaled);
                if (err != ErrorCode.Success && err != ErrorCode.WarningRecordEnd)
                {
                    return err;
                }

                System.Diagnostics.Debug.WriteLine(args.Count.ToString());


                if (err == ErrorCode.Success)
                {
                    err = this.aiCtrl.Stop();
                }

                if (err != ErrorCode.Success)
                {
                    return err;
                }

                ledSensor1.setArrayColor(m_dataScaled);
                ledSensor1.setArrayIntensity(m_dataScaled);
                ledSensor2.setArrayColor(m_dataScaled);
                ledSensor2.setArrayIntensity(m_dataScaled);
                setVin(m_dataScaled);
                System.Diagnostics.Debug.WriteLine("sensor 1 color " + ledSensor1.getColorAverage() + " " + ledSensor1.getColor().ToString());
                System.Diagnostics.Debug.WriteLine("sensor 1 intensity " + ledSensor1.getIntensityAverage());
                System.Diagnostics.Debug.WriteLine("sensor 2 color " + ledSensor2.getColorAverage() + " " + ledSensor2.getColor().ToString());
                System.Diagnostics.Debug.WriteLine("sensor 2 intensity " + ledSensor2.getIntensityAverage());
                System.Diagnostics.Debug.WriteLine("Vin " + getVinAverage());
                tcs?.SetResult(true);
                return err;

            }
            catch (System.Exception) { throw; }
            
        }*/
        
        public void waitAdc()
        {
            try
            {
                
                
                System.Diagnostics.Debug.WriteLine("ADC DONE");
            }
            catch (System.Exception e) 
            { 
                System.Diagnostics.Debug.WriteLine(e); 
            }
        }

        public System.Drawing.Color getColor(int sensor)
        {
            if (sensor == 1)
            {
                return ledSensor1.getColor();
            }
            else return ledSensor2.getColor();
        }

        public void setVin(double[] rawData)
        {
            if (vIn == null || vIn.Length < rawData.Length / 8)
            {
                vIn = new double[rawData.Length / 8];
            }
            int j = 0;
            for (int i = 4; i < rawData.Length; i = i + 8)
            {
                vIn[j] = rawData[i];
                j++;
            }
        }

        public double getVinAverage()
        {
            double result = 0;

            for (int i = 0; i < vIn.Length; i++)
            {
                result = result + vIn[i] / vIn.Length;
            }
            return result;
        }

        public bool IsVinOK(double value)
        {
            double toTest = getVinAverage();
            if (toTest > (value * 0.95) && toTest < (value * 1.05)) return true;
            else return false;
        }

        public bool IsLedOK()
        {
            if (ledSensor1.getIntensityAverage() < 0.1 || ledSensor1.getColor() != Color.White)
                return false;

            if (ledSensor2.getIntensityAverage() < 0.1 || ledSensor2.getColor() != Color.White)
                return false; 
            return true;
        }
    }
}
