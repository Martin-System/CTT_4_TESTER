using Automation.BDaq;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CTT_4_TESTER
{
    public partial class P4CttTestForm : Form
    {
        delegate void SetTextCallback(TextBox tbox, string text); /* Delegate for cross-thread invoke on textbox control */
        delegate void SetLabelTextCallback(Label label, string text); /* Delegate for cross-thread invoke on textbox control */
        delegate void EnalbeButtonCallback(Button button, bool val); /* Delegate for cross-thread invoke on textbox control */
        delegate void SetProgressBarCallback(ProgressBar progressBar, int step);
        delegate void EnableCheckBoxCallback(CheckBox progressBar, bool status);
        delegate void SetChartCallback(Chart chart, float[] dataXs, float[] dataYs);

        MsSerialPort msSerialPortToCheck;
        MsSerialPort msSerialPortGolden;
        Psoc6Program p4Program;
        Tenma tenma;
        Spectrum spectrum;

        

        int deviceNumber = 0;
        P4Relay p4Relay;
        P4Adc p4Adc;

        string fileName = String.Empty;
        string comPortCheck = null;

        float[] dataX = null;
        float[] dataY = null;

        string MacAddress;

        public P4CttTestForm()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                MessageBox.Show("cannot initialize\r\n " + e.ToString());
            }

            System.Diagnostics.Trace.WriteLine("start ok");
            msSerialPortToCheck = new MsSerialPort(textBoxUartLog, this);
            msSerialPortGolden = new MsSerialPort(textBoxUartLog, this);
            p4Program = new Psoc6Program(progressBarProgram, this);
            System.Diagnostics.Trace.WriteLine("programm ok");

            if (p4Program.Open() == false)
            {
                SetLabel(labelStatus, p4Program.getLastError());
            }

            tenma = new Tenma();
            System.Diagnostics.Trace.WriteLine("Tenma ok");
            UpdateComPortToChecTenma();
            UI_enableTenma();

            try
            {
                spectrum = new Spectrum();
            }
            catch (Exception exc)
            {
                SetLabel(labelStatus, "" + exc.Message);
            }
        }

        private void P4CttTestForm_Load(object sender, EventArgs e)
        {
            //The default device of project is demo device, users can choose other devices according to their needs. 
            if (!instantDoCtrl1.Initialized)
            {
                MessageBox.Show("No device be selected or device open failed!", "StaticDO");
                this.Close();
                return;
            }
            p4Relay = new P4Relay(instantDoCtrl1, this.deviceNumber);
            this.Text = "Static DO(" + instantDoCtrl1.SelectedDevice.Description + ")";
            ErrorCode err = p4Relay.init();
            if (err != ErrorCode.Success)
            {
                HandleError(err);
            }

            if (!instantAiCtrl1.Initialized)
            {
                MessageBox.Show("No device be selected or device open failed!", "StreamingAI");
                this.Close();
                return;
            }
            p4Adc = new P4Adc(instantAiCtrl1, this.deviceNumber);
        }

        private void HandleError(ErrorCode err)
        {
            if ((err >= ErrorCode.ErrorHandleNotValid) && (err != ErrorCode.Success))
            {
                MessageBox.Show("Sorry ! Some errors happened, the error code is: " + err.ToString());
            }
        }

        public void setSwitchState(bool[] state)
        {
            SetText(textBoxLog, "switch " + state);
            UI_enableSwitch(state);
        }

        public void ClearAndSetText(TextBox tbox, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (tbox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(ClearAndSetText);
                this.Invoke(d, new object[] { tbox, text });
            }
            else
            {
                tbox.Text = text;
            }

        }

        public void SetText(TextBox tbox, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (tbox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { tbox, text });
            }
            else
            {
                tbox.AppendText(text);
            }

        }

        public void SetLabel(Label label, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (label.InvokeRequired)
            {
                SetLabelTextCallback d = new SetLabelTextCallback(SetLabel);
                this.Invoke(d, new object[] { label, text });
            }
            else
            {
                label.Text = text;
            }

        }

        public void SetChartData(Chart chart, float[] dataXs, float[] dataYs)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.

            if (chart.InvokeRequired)
            {
                SetChartCallback d = new SetChartCallback(SetChartData);
                this.Invoke(d, new object[] { chart, dataXs, dataYs });
            }
            else
            {

                chart.Series.Clear();
                if (dataXs == null || dataYs == null) return;
                //chart.Titles.Add("Spectrum");
                chart.ChartAreas["ChartArea"].AxisX.Minimum = 869.48;
                chart.ChartAreas["ChartArea"].AxisX.Maximum = 869.52;
                chart.ChartAreas["ChartArea"].AxisX.Interval = 0.005;
                chart.ChartAreas["ChartArea"].AxisX.MajorGrid.LineColor = Color.White;
                chart.ChartAreas["ChartArea"].AxisY.Minimum = -120;
                chart.ChartAreas["ChartArea"].AxisY.Maximum = -20;
                chart.ChartAreas["ChartArea"].AxisY.Interval = 20;
                chart.ChartAreas["ChartArea"].AxisY.MajorGrid.LineColor = Color.White;
                chart.ChartAreas["ChartArea"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
                chart.ChartAreas["ChartArea"].BackColor = Color.Black;
                Series series = chart.Series.Add("Spectrum");
                series.Color = Color.Red;
                series.ChartType = SeriesChartType.Spline;
                for (int i = 0; i < dataXs.Length; i++)
                {
                    //Math.Round((i * binSize + startFreq) / 1e6,6)
                    series.Points.AddXY(dataXs[i], dataYs[i]);
                }
            }

        }

        public void EnableButton(Button button, bool val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (button.InvokeRequired)
            {
                EnalbeButtonCallback d = new EnalbeButtonCallback(EnableButton);
                this.Invoke(d, new object[] { button, val });
            }
            else
            {
                button.Enabled = val;
            }
        }

        public void SetProgressBar(ProgressBar progressBar, int step)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (progressBar.InvokeRequired)
            {
                SetProgressBarCallback d = new SetProgressBarCallback(SetProgressBar);
                this.Invoke(d, new object[] { progressBar, step });
            }
            else
            {
                progressBar.Value = step;
            }

        }

        public void EnableCheckBox(CheckBox checkBox, bool status)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (checkBox.InvokeRequired)
            {
                EnableCheckBoxCallback d = new EnableCheckBoxCallback(EnableCheckBox);
                this.Invoke(d, new object[] { checkBox, status });
            }
            else
            {
                if (status)
                {
                    checkBox.ForeColor = Color.Green;
                    checkBox.Checked = true;
                }
                else
                {
                    checkBox.ForeColor = Color.Red;
                    checkBox.Checked = false;
                }
            }

        }

        /*********************************************************/
        //ComboBox PORT

        /**********************************************************/

        private void UpdateComPortGolden()
        {
            try
            {
                String[] comPortStr = msSerialPortGolden.GetPortNames();
                foreach (String name in comPortStr)
                {
                    comboBoxComGolden.Items.Add(name);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        private void UpdateComPortToCheck()
        {
            try
            {
                String[] comPortStr = msSerialPortToCheck.GetPortNames();
                foreach (String name in comPortStr)
                {
                    comboBoxComPortToTest.Items.Add(name);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        private void UpdateComPortToChecTenma()
        {
            try
            {
                String[] comPortStr = tenma.GetPortNames();
                foreach (String name in comPortStr)
                {
                    comboBoxTenma.Items.Add(name);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

        }

        private void comboBoxComPortGolden_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxComGolden.SelectedIndex == -1)
            {
                SetText(textBoxLog, "no ComPort For Checking Choose\r\n");
                return;
            }
            else
            {
                string comPortCheck = comboBoxComGolden.Text;
                try
                {
                    msSerialPortGolden.Open(comboBoxComGolden.Text);
                    SetText(textBoxLog, "Port com open : " + comPortCheck + "\r\n");
                    Version sVersion = new Version(msSerialPortGolden);
                    if (!sVersion.golden)
                    {
                        msSerialPortGolden.Close();
                        SetLabel(labelStatus, "Not a Golden board");
                    }
                    else
                    {
                        SetLabel(labelStatus, "Golden detected");
                        UI_enablePortToTest();
                        UpdateComPortToCheck();
                        tenma.Off();
                    }
                }
                catch (Exception exc)
                {
                    SetLabel(labelStatus, "" + exc.Message);
                }
            }
        }

        private void comboBoxComPortToCHeck_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxComPortToTest.SelectedIndex == -1)
            {
                SetText(textBoxLog, "no ComPort For Checking Choose\r\n");
                return;
            }
            else
            {
                comPortCheck = comboBoxComPortToTest.Text;
                msSerialPortToCheck.Open(comboBoxComPortToTest.Text);
                SetText(textBoxLog, "Port com open : " + comPortCheck + "\r\n");
                UI_enableSn();
            }
        }

        private void comboBoxComPortTenmaToCHeck_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTenma.SelectedIndex == -1)
            {
                System.Diagnostics.Debug.WriteLine("No Com Port selected");
                return;
            }
            else
            {
                string comPort = comboBoxTenma.Text;


                try
                {
                    tenma.Open(comPort);
                    SetLabel(labelStatus, "Tenma : " + tenma.getIdentification());
                    Thread.Sleep(200);
                    tenma.setVoltage(4.2f);
                    Thread.Sleep(200);
                    tenma.setCurrent(3f);
                    Thread.Sleep(200);
                    tenma.On();

                    UpdateComPortGolden();
                    UI_enablePortGolden();
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(exc.Message);
                    tenma.Close();
                    MessageBox.Show("Sorry ! Some errors happened, the error code is: " + exc.Message);
                }
            }
        }

        /********************************************************/
        //Choose files to programm
        /********************************************************/

        private void chooseApplicationFile()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(Environment.CurrentDirectory);
                openFileDialog.Filter = "Hex file (*.hex)|*.hex";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                }
            }

            //MessageBox.Show("file name", "File Content at path: " + filePath, MessageBoxButtons.OK);
            textBoxFileName.Text = filePath.Substring(filePath.Length - 40);
            this.fileName = filePath;
        }

        /*********************************************************************/
        //Enable - disable UID          
        /*********************************************************************/

        private void UI_enableTenma()
        {
            comboBoxTenma.Enabled = true;
            comboBoxComGolden.Enabled = false;
            comboBoxComPortToTest.Enabled = false;
            textBoxSn.Enabled = false;
            buttonStart.Enabled = false;
            buttonCwSave.Enabled = false;
            buttonCwDec.Enabled = false;
            buttonCwDecP.Enabled = false;
            buttonCwInc.Enabled = false;
            buttonCwIncP.Enabled = false;
            buttonErrorCW.Enabled = false;
        }
        private void UI_enablePortGolden()
        {
            comboBoxTenma.Enabled = false;
            comboBoxComGolden.Enabled = true;
            comboBoxComPortToTest.Enabled = false;
            textBoxSn.Enabled = false;
            buttonStart.Enabled = false;
            buttonCwSave.Enabled = false;
            buttonCwDec.Enabled = false;
            buttonCwDecP.Enabled = false;
            buttonCwInc.Enabled = false;
            buttonCwIncP.Enabled = false;
            buttonErrorCW.Enabled = false;
        }

        private void UI_enablePortToTest()
        {
            comboBoxTenma.Enabled = false;
            comboBoxComGolden.Enabled = false;
            comboBoxComPortToTest.Enabled = true;
            textBoxSn.Enabled = false;
            buttonStart.Enabled = false;
            buttonCwSave.Enabled = false;
            buttonCwDec.Enabled = false;
            buttonCwDecP.Enabled = false;
            buttonCwInc.Enabled = false;
            buttonCwIncP.Enabled = false;
            buttonErrorCW.Enabled = false;
        }

        private void UI_enableSn()
        {
            comboBoxTenma.Enabled = false;
            comboBoxComGolden.Enabled = false;
            comboBoxComPortToTest.Enabled = false;
            textBoxSn.Enabled = true;
            buttonStart.Enabled = false;
            buttonCwSave.Enabled = false;
            buttonCwDec.Enabled = false;
            buttonCwDecP.Enabled = false;
            buttonCwInc.Enabled = false;
            buttonErrorCW.Enabled = false;
            buttonCwIncP.Enabled = false;
            UI_enableAllSwitchTest(false);
        }
        private void UI_enableStart()
        {
            comboBoxTenma.Enabled = false;
            comboBoxComGolden.Enabled = false;
            comboBoxComPortToTest.Enabled = false;
            textBoxSn.Enabled = true;
            buttonStart.Enabled = true;
            buttonCwSave.Enabled = false;
            buttonCwDec.Enabled = false;
            buttonCwDecP.Enabled = false;
            buttonCwInc.Enabled = false;
            buttonCwIncP.Enabled = false;
            buttonErrorCW.Enabled = false;
        }

        private void UI_enableUnderTest()
        {
            comboBoxTenma.Enabled = false;
            comboBoxComGolden.Enabled = false;
            comboBoxComPortToTest.Enabled = false;
            textBoxSn.Enabled = false;
            buttonStart.Enabled = false;
            buttonCwSave.Enabled = false;
            buttonCwDec.Enabled = false;
            buttonCwDecP.Enabled = false;
            buttonCwInc.Enabled = false;
            buttonCwIncP.Enabled = false;
            buttonErrorCW.Enabled = false;
        }

        private void UI_enableAllSwitchTest(bool status)
        {
            EnableCheckBox(checkBox1A, status);
            EnableCheckBox(checkBox1B, status);
            EnableCheckBox(checkBox2A, status);
            EnableCheckBox(checkBox2B, status);
            EnableCheckBox(checkBoxCfg, status);
            EnableCheckBox(checkBoxDec, status);
            EnableCheckBox(checkBoxInc, status);
        }

        private void UI_enableSwitch(bool[] stat)
        {
            EnableCheckBox(checkBox1A, stat[0]);
            EnableCheckBox(checkBox1B, stat[1]);
            EnableCheckBox(checkBox2A, stat[2]);
            EnableCheckBox(checkBox2B, stat[3]);
            EnableCheckBox(checkBoxCfg, stat[4]);
            EnableCheckBox(checkBoxDec, stat[5]);
            EnableCheckBox(checkBoxInc, stat[6]);
        }

        private bool QuestionToUser(string textToDispaly)
        {
            bool ret = true;
            string message = textToDispaly;
            const string caption = "Tester check";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                ret = false;
            }

            return ret;
        }

        /***************************************************/
        //SERIAL Number modified
        /***************************************************/

        public Boolean parseSn(string strSN, out int sn)
        {
            bool ret = false;

            string strTmp;
            if (textBoxSn.Text.Length < 6)  //from 1 to 5
            {
                strTmp = strSN;
            }
            else
            {
                strTmp = strSN.Substring(strSN.Length - 5, 5);
            }
            if (int.TryParse(strTmp, out sn))
            {
                ret = true;
            }
            return ret;
        }

        private void textBoxSn_TextChanged(object sender, EventArgs e)
        {
            int serial = 0;
            if (textBoxSn.Text.Length == 0)
            {
                UI_enableSn();
                return;
            }
            if (!parseSn(textBoxSn.Text, out serial))
            {
                SetLabel(labelStatus, "Serial doit être un nombre.");
                UI_enableSn();
            }
            else
            {
                SetLabel(labelStatus, "Serial OK :" + serial);
                UI_enableStart();
            }
        }

        /******************************************
         *      BackGround : Test                 *
         *                                        *
         ******************************************/
        private void BgW_Test_DoWork(object sender, DoWorkEventArgs e)
        {
            bool skipStep = false;
            try
            {
                //P4Adc p4Adc;
                //Thread.SetApartmentState(ApartmentState.STA);
                p4Relay.setStandby();
                //SetChartData(chartSpectrum, null, null);
                tenma.setVoltage(4.2f);
                Thread.Sleep(200);
                tenma.setCurrent(3f);
                Thread.Sleep(200);
                tenma.On();

                Thread.Sleep(1000);
                p4Adc.startAdc();
                if (!p4Adc.IsVinOK(3.0))
                {
                    e.Result = "Error Board not in place or not connected to power";
                    return;
                }

                /********************************/
                    //Programm the board
                /********************************/

                if (!skipStep)
                {
                    programmTheBoard();
                    Thread.Sleep(500);
                }

                /********************************/
                    //Check Version 
                /********************************/
                Version sVersion = new Version(msSerialPortToCheck);
                SetText(textBoxLog, "Version : " + sVersion.toString() + "\r\n");
                ClearAndSetText(textBoxBoard, sVersion.board_version);
                ClearAndSetText(textBoxFirmware, sVersion.firmware_version);

                /********************************/
                //Check Switch 
                /********************************/
                QuestionToUser("Press OK en after press every switch of the remote");

                PushButton pushButton = new PushButton(msSerialPortToCheck);
                if (pushButton.waitStatus(msSerialPortToCheck, this, 20000))
                    SetText(textBoxLog, "pushButton" + pushButton.toString() + "\r\n");

                /********************************/
                //Check Battery ADC 
                /********************************/
                SetLabel(labelStatus, "TEST BATTERY");
                Battery sbattery = new Battery(msSerialPortToCheck);

                SetLabel(labelStatus, "BATTERY RECEIVEDY");
                if (sbattery.IsValueOk(4.2f, 0.05f))
                {
                    SetText(textBoxLog, "Battery OK" + sbattery.toString() + "\r\n");
                }
                else
                {
                    SetText(textBoxLog, "Battery NOK" + sbattery.toString() + "\r\n");
                    e.Result = "Error Battery - Check battery measurement : " + sbattery.value;
                    return;
                }
                SetLabel(labelStatus, "BATTERY OK");

                /********************************/
                //  Check SX Carrier Wave
                /********************************/

                Sx.CMD sxCmd = Sx.CMD.START_CW;

                //UI_enableCw();
                SetLabel(labelStatus, "Must centered the central frequency");
                dataY = null;
                dataX = null;
                Sx sSx = new Sx(msSerialPortToCheck, sxCmd);
                SetText(textBoxLog, "SxCarrier " + sSx.toString() + "\r\n");
                Thread.Sleep(100);
                double freqCenterMHz = 0;
                double? peakCenter_dBm = null;
                spectrum.GetPeakSpectrum(ref freqCenterMHz, ref peakCenter_dBm, ref dataX, ref dataY);
                SetChartData(chartSpectrum, dataX, dataY);
                sSx = new Sx(msSerialPortToCheck, freqCenterMHz);
                Thread.Sleep(100);

                spectrum.GetPeakSpectrum(ref freqCenterMHz, ref peakCenter_dBm, ref dataX, ref dataY);
                SetChartData(chartSpectrum, dataX, dataY);
                sSx = new Sx(msSerialPortToCheck, freqCenterMHz);
                Thread.Sleep(100);

                spectrum.GetPeakSpectrum(ref freqCenterMHz, ref peakCenter_dBm, ref dataX, ref dataY);
                SetChartData(chartSpectrum, dataX, dataY);

                if (QuestionToUser("Is Peak central?") == true)
                {
                    SetText(textBoxLog, "RF Calibration OK");
                }
                else
                {
                    SetText(textBoxLog, "RF Calibration NOK");
                    e.Result = "Error RF - Check PCB and restart test";
                    return;
                }

                /********************************/
                //  Check SX : TX and RX RSSI
                /********************************/
                Sx sSxGolden = new Sx(msSerialPortGolden, Sx.CMD.LISTEN);
                SetText(textBoxLog, "Golden Listen" + sSxGolden.toStringRSSI());

                sSx = new Sx(msSerialPortToCheck, Sx.CMD.TEST_RSSI);
                SetText(textBoxLog, "TEST RSSI" + sSx.toStringRSSI());

                if (!sSx.IsRssiOk())
                {
                    SetText(textBoxLog, "Radio RSSI is too low Error");
                    e.Result = " Radio RSSI not correct";
                    return;
                }
                SetLabel(labelStatus, "Radio RSSI OK");

                

                /********************************/
                //Check Battery ADC 
                /********************************/
                Charge charge = new Charge(msSerialPortToCheck);
                SetText(textBoxLog, "charge OK" + charge.toString() + "\r\n");

                if (charge.waitStatus(msSerialPortToCheck, Charge.Charging.NO_OP, 1000))
                    SetText(textBoxLog, "test charge OK " + Charge.Charging.NO_OP + "\r\n");

                p4Relay.setUsbCharging(true);
                if(charge.waitStatus(msSerialPortToCheck, Charge.Charging.WIRE, 2000))
                    SetText(textBoxLog, "test charge OK " + Charge.Charging.WIRE + "\r\n");
                Thread.Sleep(500);

                tenma.setVoltage(4.8f);
                if (charge.waitStatus(msSerialPortToCheck, Charge.Charging.WIRE_CHARGED, 2000))
                    SetText(textBoxLog, "test charge OK " + Charge.Charging.WIRE_CHARGED + "\r\n");
                Thread.Sleep(500);

                tenma.setVoltage(4.2f);

                p4Relay.setUsbCharging(false);
                if (charge.waitStatus(msSerialPortToCheck, Charge.Charging.NO_OP, 1000))
                    SetText(textBoxLog, "test charge OK " + Charge.Charging.NO_OP + "\r\n");
                Thread.Sleep(500);

                p4Relay.setWpcCharging(true);

                QuestionToUser("Make sure the self is correctly aligned on the wireless charger");

                if (charge.waitStatus(msSerialPortToCheck, Charge.Charging.WPC, 5000))
                    SetText(textBoxLog, "test charge OK " + Charge.Charging.WPC + "\r\n");
                Thread.Sleep(500);

                tenma.setVoltage(4.8f);
                if (charge.waitStatus(msSerialPortToCheck, Charge.Charging.WPC_CHARGED, 2000))
                    SetText(textBoxLog, "test charge OK " + Charge.Charging.WPC_CHARGED + "\r\n");
                Thread.Sleep(500);

                tenma.setVoltage(4.2f);
                p4Relay.setWpcCharging(false);
                if (charge.waitStatus(msSerialPortToCheck, Charge.Charging.NO_OP, 5000))
                    SetText(textBoxLog, "test charge OK " + Charge.Charging.NO_OP + "\r\n");
                p4Relay.setStandby();

                Thread.Sleep(1000);


                /********************************/
                //Test FK Communication 
                /********************************/
                QuestionToUser("Press OK and use the Finger Kick");

                Fk fk = new Fk(msSerialPortToCheck);
                if(fk.IsRssiOk())
                    SetText(textBoxLog, "FK" + fk.toString() + "\r\n");
                else
                {
                    SetText(textBoxLog, "FK Radio too low");
                    e.Result = " FK RSSI not correct";
                    return;
                }

                /********************************/
                //Test BUZZER
                /********************************/

                Buzzer sBuzzer = new Buzzer(msSerialPortToCheck, 1);
                SetText(textBoxLog, "BUZZER " + sBuzzer.toString() + "\r\n");

                if (QuestionToUser("Do you heard the buzzer?") == true)
                {
                    SetText(textBoxLog, "Buzzer OK");
                }
                else
                {
                    SetText(textBoxLog, "Buzzer NOK");
                    e.Result = "Error BUZER - Check them and restart test";
                    return;
                }
                sBuzzer = new Buzzer(msSerialPortToCheck, 0);
                SetText(textBoxLog, "BUZZER " + sBuzzer.toString() + "\r\n");
                SetLabel(labelStatus, "BUZZER OK");


                /********************************/
                //Test Vibration
                /********************************/


                Vibration sVib = new Vibration(msSerialPortToCheck, 1);
                SetText(textBoxLog, "VIBRATION " + sVib.toString() + "\r\n");

                if (QuestionToUser("Do you feel the vibration?") == true)
                {
                    SetText(textBoxLog, "vibration OK");
                }
                else
                {
                    SetText(textBoxLog, "vibration NOK");
                    e.Result = "Error Vibration - Check them and restart test";
                    return;
                }
                sVib = new Vibration(msSerialPortToCheck, 0);
                SetText(textBoxLog, "VIBRATION " + sVib.toString() + "\r\n");
                SetLabel(labelStatus, "VIBRATION OK");
            }
            catch (Exception exc)
            {
                /* Return error if an exception occurs */
                e.Result = "" + exc.Message;
                tenma.Off();
            }


        }

        private void BgW_Test_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                SetLabel(labelStatus, (string)e.Result + "\r\nChange SN to enabled a new test");
                //ResetButton();

            }
            else
            {
                SetLabel(labelStatus, "PCB end test - all OK enter the next serial number");
            }
            UI_enableSn();
        }
        static bool stateUSB = false;
        static bool stateWPC = false;
        private void buttonWPC_Click(object sender, EventArgs e)
        {
            if (stateWPC)
            {
                stateWPC = false;
            }
            else stateWPC = true;
            p4Relay.setWpcCharging(stateWPC);
            if (!stateWPC)
            {
                p4Relay.setStandby();
            }

        }

        private void buttonUSB_Click(object sender, EventArgs e)
        {
            if (stateUSB)
            {
                stateUSB = false;
            }
            else stateUSB = true;
            p4Relay.setUsbCharging(stateUSB);
            if (!stateUSB)
            {
                p4Relay.setStandby();
            }
        }

        private void buttonADC_Click(object sender, EventArgs e)
        {
            p4Adc.startAdc();

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (comPortCheck != null)
            {
                if (this.fileName == String.Empty)
                {
                    chooseApplicationFile();
                }
                UI_enableUnderTest();
                BgW_Test.RunWorkerAsync();
                //ResetButton();
            }
            else SetText(textBoxLog, "No Comm Port selected\r\n");

        }

        private void programmTheBoard()
        {
            int hr;

            SetProgressBar(progressBarProgram, 0);
            //chooseApplicationFile();
            SetLabel(labelStatus, "Board in program");
            hr = p4Program.Execute(this.fileName);
            System.Diagnostics.Debug.WriteLine("programming " + hr);
            if (hr != 0)
            {
                SetLabel(labelStatus, p4Program.getLastError());
                throw new Exception("Error during programming");
            }
            else SetLabel(labelStatus, "Board program OK");

            SetProgressBar(progressBarProgram, 0);
        }

        private void buttonProgram_Click(object sender, EventArgs e)
        {
            programmTheBoard();
        }

        private void buttonTenma_Click(object sender, EventArgs e)
        {

            tenma.On();
        }

        private void buttonCharge_Click(object sender, EventArgs e)
        {
            Charge charge = new Charge(msSerialPortToCheck);
            SetText(textBoxLog, "charge OK" + charge.toString() + "\r\n");

            if(charge.waitStatus(msSerialPortToCheck,Charge.Charging.NO_OP,1000))
                SetText(textBoxLog, "charge NO OP OK" + charge.toString() + "\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PushButton pushButton = new PushButton(msSerialPortToCheck);
            if (pushButton.waitStatus(msSerialPortToCheck, this, 20000))
                SetText(textBoxLog, "pushButton" + pushButton.toString() + "\r\n");
        }

        private void buttonSxTx_Click(object sender, EventArgs e)
        {
            Sx sSx = new Sx(msSerialPortToCheck, Sx.CMD.TEST_RSSI);
            SetText(textBoxLog, "TEST RSSI" + sSx.toStringRSSI());

            if (!sSx.IsRssiOk())
            {
                SetText(textBoxLog, "Radio RSSI is too low Error");
                return;
            }
            SetLabel(labelStatus, "Radio RSSI OK");
        }

        private void buttonFk_Click(object sender, EventArgs e)
        {
            QuestionToUser("Press OK and use the Finger Kick");

            Fk fk = new Fk(msSerialPortToCheck);
            if (fk.IsRssiOk())
                SetText(textBoxLog, "FK" + fk.toString() + "\r\n");
            else
            {
                SetText(textBoxLog, "FK Radio too low");
                return;
            }
        }

        private void buttonVib_Click(object sender, EventArgs e)
        {
            Vibration sVib = new Vibration(msSerialPortToCheck, 1);
            SetText(textBoxLog, "VIBRATION " + sVib.toString() + "\r\n");

            if (QuestionToUser("Do you feel the vibration?") == true)
            {
                SetText(textBoxLog, "vibration OK");
            }
            else
            {
                SetText(textBoxLog, "vibration NOK");
                return;
            }
            sVib = new Vibration(msSerialPortToCheck, 0);
            SetText(textBoxLog, "VIBRATION " + sVib.toString() + "\r\n");
            SetLabel(labelStatus, "VIBRATION OK");
        }

        private void buttonBuzzer_Click(object sender, EventArgs e)
        {
            Buzzer sBuzzer = new Buzzer(msSerialPortToCheck, 1);
            SetText(textBoxLog, "BUZZER " + sBuzzer.toString() + "\r\n");

            if (QuestionToUser("Do you heard the buzzer?") == true)
            {
                SetText(textBoxLog, "Buzzer OK");
            }
            else
            {
                SetText(textBoxLog, "Buzzer NOK");
                return;
            }
            sBuzzer = new Buzzer(msSerialPortToCheck, 0);
            SetText(textBoxLog, "BUZZER " + sBuzzer.toString() + "\r\n");
            SetLabel(labelStatus, "BUZZER OK");
        }

        private void buttonGetMac_Click(object sender, EventArgs e)
        {
            Mac sMac = new Mac(msSerialPortToCheck);
            SetText(textBoxLog, "Mac " + sMac.toString() + "\r\n");
            MacAddress = sMac.publicAddress;
        }

        private void buttonBleConnect_Click(object sender, EventArgs e)
        {
            Mac sMac = new Mac(msSerialPortGolden,MacAddress);
            SetText(textBoxLog, "Mac " + sMac.toString() + "\r\n");
        }
    }
}
