using System;
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
        delegate void SetChartCallback(Chart chart, float[] dataXs, float[] dataYs);

        MsSerialPort msSerialPortToCheck;
        MsSerialPort msSerialPortGolden;
        P4Program p4Program;
        Tenma tenma;


        string fileName = String.Empty;

        string comPortCheck = null;

        

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
            p4Program = new P4Program(progressBarProgram, this);
            System.Diagnostics.Trace.WriteLine("programm ok");

            if (p4Program.Open() == false)
            {
                SetLabel(labelStatus, p4Program.getLastError());
            }

            tenma = new Tenma();
            System.Diagnostics.Trace.WriteLine("Tenma ok");
            UpdateComPortToChecTenma();
            UI_enableTenma();
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

    }
}
