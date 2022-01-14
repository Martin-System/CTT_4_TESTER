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
    class P4Adc
    {
        InstantAiCtrl aiCtrl;
        double[] m_dataScaled;

        double[] vIn;
        int chanCount = 8;
        int sectionLength = 128;

        TaskCompletionSource<bool> tcs = null;

        public P4Adc(InstantAiCtrl aiCtrl, int deviceNumber)
        {
            this.aiCtrl = aiCtrl;
            this.aiCtrl.SelectedDevice = new DeviceInformation(deviceNumber);

            //int chanCount = this.aiCtrl.Conversion.ChannelCount;
            //int sectionLength = this.aiCtrl.Record.SectionLength;
            m_dataScaled = new double[chanCount * sectionLength];
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

            
            setVin(m_dataScaled);
            System.Diagnostics.Debug.WriteLine("Vin " + getVinAverage());
        }

       
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

        public void setVin(double[] rawData)
        {
            if (vIn == null || vIn.Length < rawData.Length / 8)
            {
                vIn = new double[rawData.Length / 8];
            }
            int j = 0;
            for (int i = 0; i < rawData.Length; i = i + 8)
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
    }
}
