using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace CTT_4_TESTER

{
    class Spectrum
    {
        uint traceLen = 0;
        int traceLenSA = 0;
        double binSize = 0.0;
        double startFreq = 0.0;
        int id = -1;
        bool isBB = false;
        bool isSA = false;

        public Spectrum()
        {
            isBB = false;
            isSA = false;

            if (openSA())
            {
                isSA = true;
            }
            else if (openBB())
            {
                isBB = true;
            }

            if (isBB) initBB();
            else if (isSA) initSA();

            else if (isBB==false && isSA==false) throw new Exception("Error: Unable to open SPECTRUM");

        }

        private bool openBB()
        {
            bool ret=false;
            bbStatus status = bbStatus.bbNoError;

            Console.Write("Opening spectrum BB, Please Wait\n");
            status = bb_api.bbOpenDevice(ref id);
            if (status != bbStatus.bbNoError)
            {
                Console.Write("Error: Unable to open BB60\n");
                Console.Write(bb_api.bbGetStatusString(status) + "\n");
                //throw new Exception("Error: Unable to open SPECTRUM");
                ret = false;
            }
            else
            {
                Console.Write("Spectrum BB Found\n\n");
                ret = true;
            }

            return ret;
        }

        private bool openSA()
        {
            int[] serials = new int[8];
            int deviceCount = 0;
            saStatus status = sa_api.saGetSerialNumberList(serials, ref deviceCount);
            if (deviceCount < 1)
            {
                Console.WriteLine("No spectrum SA found");
                return false;
            }

            status = sa_api.saOpenDeviceBySerialNumber(ref id, serials[0]);
            if (status < 0)
            {
                Console.WriteLine("saOpenDevice error: " + sa_api.saGetStatusString(status));
                return false;
            }

            return true;
        }

        private void initBB()
        {

            bbStatus status = bbStatus.bbNoError;

            Console.Write("API Version: " + bb_api.bbGetAPIString() + "\n");
            Console.Write("Device Type: " + bb_api.bbGetDeviceName(id) + "\n");
            Console.Write("Serial Number: " + bb_api.bbGetSerialString(id) + "\n");
            Console.Write("Firmware Version: " + bb_api.bbGetFirmwareString(id) + "\n");
            Console.Write("\n");

            float temp = 0.0F, voltage = 0.0F, current = 0.0F;
            bb_api.bbGetDeviceDiagnostics(id, ref temp, ref voltage, ref current);
            Console.Write("Device Diagnostics\n" +
                "Temperature: " + temp.ToString() + " C\n" +
                "USB Voltage: " + voltage.ToString() + " V\n" +
                "USB Current: " + current.ToString() + " mA\n");
            Console.Write("\n");

            Console.Write("Configuring Device For a Sweep\n");
            bb_api.bbConfigureAcquisition(id, bb_api.BB_MIN_AND_MAX, bb_api.BB_LOG_SCALE);
            bb_api.bbConfigureCenterSpan(id, 869.5e6, 200.0e3);
            bb_api.bbConfigureLevel(id, -20.0, bb_api.BB_AUTO_ATTEN);
            bb_api.bbConfigureGain(id, bb_api.BB_AUTO_GAIN);
            bb_api.bbConfigureSweepCoupling(id, 100.0, 100.0, 0.001,
                bb_api.BB_RBW_SHAPE_NUTTALL, bb_api.BB_NO_SPUR_REJECT);
            bb_api.bbConfigureProcUnits(id, bb_api.BB_LOG);

            status = bb_api.bbInitiate(id, bb_api.BB_SWEEPING, 0);
            if (status != bbStatus.bbNoError)
            {
                Console.Write("Error: Unable to initialize BB60\n");
                Console.Write(bb_api.bbGetStatusString(status) + "\n");
                throw new Exception("Error: Unable to open initialize");
            }

            status = bb_api.bbQueryTraceInfo(id, ref traceLen, ref binSize, ref startFreq);

            Console.Write("start freq " + startFreq + "\n");
            Console.Write("binSize " + binSize + "\n");
            Console.Write("traceLen " + traceLen + "\n");
        }

        private void initSA()
        {
            saStatus status;

            // Print off information about device
            saDeviceType deviceType = saDeviceType.saDeviceTypeSA44;
            int serialNumber = 0;
            sa_api.saGetDeviceType(id, ref deviceType);
            sa_api.saGetSerialNumber(id, ref serialNumber);
            Console.WriteLine("Device Type: " + deviceType);
            Console.WriteLine("Serial: " + serialNumber);

            float voltage = 0, temperature = 0;
            sa_api.saQueryDiagnostics(id, ref voltage);
            sa_api.saQueryTemperature(id, ref temperature);
            Console.WriteLine("Voltage: " + voltage + " V");
            Console.WriteLine("Temperature: " + temperature + " C");

            // Configure leveling
            sa_api.saConfigGainAtten(id, sa_api.SA_AUTO_ATTEN, sa_api.SA_AUTO_GAIN, true);
            sa_api.saConfigLevel(id, 0.0);

            // Setup sweep
            sa_api.saConfigCenterSpan(id, 869.5e6, 200.0e3);
            sa_api.saConfigSweepCoupling(id, 100.0, 100.0, false);
            sa_api.saConfigAcquisition(id, sa_api.SA_AVERAGE, sa_api.SA_LOG_SCALE);
            sa_api.saConfigRBWShape(id, sa_api.SA_RBW_SHAPE_FLATTOP);

            status = sa_api.saInitiate(id, sa_api.SA_SWEEPING, 0);
            if (status < 0)
            {
                Console.WriteLine("saInitiate error: " + sa_api.saGetStatusString(status));
                return;
            }

            // int sweepLength = 0;
            // double startFreq = 0, binSize = 0;
            sa_api.saQuerySweepInfo(id, ref traceLenSA, ref startFreq, ref binSize);
        }
        public void GetPeakSpectrum(ref double freqCenterMHz, ref double? peakCenter_dBm, ref float[] freq, ref float[] sweepMax)
        {
            if (isBB) GetPeakSpectrumBB(ref freqCenterMHz, ref peakCenter_dBm, ref freq, ref sweepMax);
            else if (isSA) GetPeakSpectrumSA(ref freqCenterMHz, ref peakCenter_dBm, ref freq, ref sweepMax);

            else if (isBB == false && isSA == false) throw new Exception("Error: No Spectrum open");
          
        }


        private void GetPeakSpectrumBB(ref double freqCenterMHz, ref double? peakCenter_dBm, ref float[] freq, ref float[] sweepMax)
        {
            float[] sweepMin;
            sweepMax = new float[traceLen];
            freq = new float[traceLen];
            sweepMin = new float[traceLen];

            bb_api.bbFetchTrace_32f(id, unchecked((int)traceLen), sweepMin, sweepMax);
            double? maxVal = null; //nullable so this works even if you have all super-low negatives
            int index = -1;
            //chart.Series.Clear();
            //chart.Titles.Add("Spectrum");

            // Series series = chart.Series.Add("Spectrum");
            //series.ChartType = SeriesChartType.Spline;
            for (int i = 0; i < sweepMax.Length; i++)
            {
                double thisNum = sweepMax[i];
                freq[i] = (float)((i * binSize + startFreq) / 1e6);

                // series.Points.AddXY(Math.Round((i * binSize + startFreq) / 1e6,6), sweepMax[i]);

                if (!maxVal.HasValue || thisNum > maxVal.Value)
                {
                    maxVal = thisNum;
                    index = i;
                }
            }

            freqCenterMHz = (index * binSize + startFreq) / 1e6;
            peakCenter_dBm = maxVal;




        }

        private void GetPeakSpectrumSA(ref double freqCenterMHz, ref double? peakCenter_dBm, ref float[] freq, ref float[] sweepMax)
        {
            float[] sweepMin;
            sweepMax = new float[traceLenSA];
            freq = new float[traceLenSA];
            sweepMin = new float[traceLenSA];
            double? maxVal = null; //nullable so this works even if you have all super-low negatives
            int index = -1;

            sa_api.saGetSweep_32f(id, sweepMin, sweepMax);
            // sa_api.saAbort(id);

            for (int i = 0; i < sweepMax.Length; i++)
            {
                double thisNum = sweepMax[i];
                freq[i] = (float)((i * binSize + startFreq) / 1e6);
                // series.Points.AddXY(Math.Round((i * binSize + startFreq) / 1e6,6), sweepMax[i]);

                if (!maxVal.HasValue || thisNum > maxVal.Value)
                {
                    maxVal = thisNum;
                    index = i;
                }
            }

            freqCenterMHz = (index * binSize + startFreq) / 1e6;
            peakCenter_dBm = maxVal;

        }
    }
}
