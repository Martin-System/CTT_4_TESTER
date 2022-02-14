namespace CTT_4_TESTER
{
    partial class P4CttTestForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(P4CttTestForm));
            this.buttonWpc = new System.Windows.Forms.Button();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonErrorCW = new System.Windows.Forms.Button();
            this.comboBoxComGolden = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCwInc = new System.Windows.Forms.Button();
            this.buttonCwIncP = new System.Windows.Forms.Button();
            this.buttonCwDec = new System.Windows.Forms.Button();
            this.buttonCwSave = new System.Windows.Forms.Button();
            this.buttonCwDecP = new System.Windows.Forms.Button();
            this.comboBoxTenma = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxBoard = new System.Windows.Forms.TextBox();
            this.textBoxFirmware = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.progressBarProgram = new System.Windows.Forms.ProgressBar();
            this.textBoxSn = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.textBoxUartLog = new System.Windows.Forms.TextBox();
            this.comboBoxComPortToTest = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BgW_Test = new System.ComponentModel.BackgroundWorker();
            this.buttonUSB = new System.Windows.Forms.Button();
            this.buttonADC = new System.Windows.Forms.Button();
            this.buttonProgram = new System.Windows.Forms.Button();
            this.buttonTenma = new System.Windows.Forms.Button();
            this.buttonCharge = new System.Windows.Forms.Button();
            this.buttonSwitch = new System.Windows.Forms.Button();
            this.groupBoxSwitch = new System.Windows.Forms.GroupBox();
            this.checkBoxInc = new System.Windows.Forms.CheckBox();
            this.checkBoxDec = new System.Windows.Forms.CheckBox();
            this.checkBoxCfg = new System.Windows.Forms.CheckBox();
            this.checkBox2B = new System.Windows.Forms.CheckBox();
            this.checkBox2A = new System.Windows.Forms.CheckBox();
            this.checkBox1B = new System.Windows.Forms.CheckBox();
            this.checkBox1A = new System.Windows.Forms.CheckBox();
            this.chartSpectrum = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.buttonSxTx = new System.Windows.Forms.Button();
            this.buttonFk = new System.Windows.Forms.Button();
            this.buttonVib = new System.Windows.Forms.Button();
            this.buttonBuzzer = new System.Windows.Forms.Button();
            this.instantDoCtrl1 = new Automation.BDaq.InstantDoCtrl(this.components);
            this.instantAiCtrl1 = new Automation.BDaq.InstantAiCtrl(this.components);
            this.buttonGetMac = new System.Windows.Forms.Button();
            this.buttonBleConnect = new System.Windows.Forms.Button();
            this.buttonSN = new System.Windows.Forms.Button();
            this.groupBoxSwitch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSpectrum)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonWpc
            // 
            this.buttonWpc.Location = new System.Drawing.Point(915, 196);
            this.buttonWpc.Name = "buttonWpc";
            this.buttonWpc.Size = new System.Drawing.Size(75, 23);
            this.buttonWpc.TabIndex = 106;
            this.buttonWpc.Text = "buttonWpc";
            this.buttonWpc.UseVisualStyleBackColor = true;
            this.buttonWpc.Click += new System.EventHandler(this.buttonWPC_Click);
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFileName.Enabled = false;
            this.textBoxFileName.Location = new System.Drawing.Point(327, 44);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(422, 20);
            this.textBoxFileName.TabIndex = 105;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(324, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 104;
            this.label4.Text = "File name";
            // 
            // buttonErrorCW
            // 
            this.buttonErrorCW.Location = new System.Drawing.Point(305, 196);
            this.buttonErrorCW.Name = "buttonErrorCW";
            this.buttonErrorCW.Size = new System.Drawing.Size(119, 23);
            this.buttonErrorCW.TabIndex = 103;
            this.buttonErrorCW.Text = "No CW found";
            this.buttonErrorCW.UseVisualStyleBackColor = true;
            // 
            // comboBoxComGolden
            // 
            this.comboBoxComGolden.FormattingEnabled = true;
            this.comboBoxComGolden.Location = new System.Drawing.Point(147, 44);
            this.comboBoxComGolden.Name = "comboBoxComGolden";
            this.comboBoxComGolden.Size = new System.Drawing.Size(121, 21);
            this.comboBoxComGolden.TabIndex = 102;
            this.comboBoxComGolden.SelectedIndexChanged += new System.EventHandler(this.comboBoxComPortGolden_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 101;
            this.label1.Text = "COM Port Golden";
            // 
            // buttonCwInc
            // 
            this.buttonCwInc.Location = new System.Drawing.Point(224, 196);
            this.buttonCwInc.Name = "buttonCwInc";
            this.buttonCwInc.Size = new System.Drawing.Size(35, 23);
            this.buttonCwInc.TabIndex = 100;
            this.buttonCwInc.Text = ">";
            this.buttonCwInc.UseVisualStyleBackColor = true;
            // 
            // buttonCwIncP
            // 
            this.buttonCwIncP.Location = new System.Drawing.Point(264, 196);
            this.buttonCwIncP.Name = "buttonCwIncP";
            this.buttonCwIncP.Size = new System.Drawing.Size(35, 23);
            this.buttonCwIncP.TabIndex = 99;
            this.buttonCwIncP.Text = ">>>";
            this.buttonCwIncP.UseVisualStyleBackColor = true;
            // 
            // buttonCwDec
            // 
            this.buttonCwDec.Location = new System.Drawing.Point(60, 196);
            this.buttonCwDec.Name = "buttonCwDec";
            this.buttonCwDec.Size = new System.Drawing.Size(35, 23);
            this.buttonCwDec.TabIndex = 98;
            this.buttonCwDec.Text = "<";
            this.buttonCwDec.UseVisualStyleBackColor = true;
            // 
            // buttonCwSave
            // 
            this.buttonCwSave.Location = new System.Drawing.Point(100, 196);
            this.buttonCwSave.Name = "buttonCwSave";
            this.buttonCwSave.Size = new System.Drawing.Size(119, 23);
            this.buttonCwSave.TabIndex = 97;
            this.buttonCwSave.Text = "Save CW";
            this.buttonCwSave.UseVisualStyleBackColor = true;
            // 
            // buttonCwDecP
            // 
            this.buttonCwDecP.Location = new System.Drawing.Point(20, 196);
            this.buttonCwDecP.Name = "buttonCwDecP";
            this.buttonCwDecP.Size = new System.Drawing.Size(35, 23);
            this.buttonCwDecP.TabIndex = 96;
            this.buttonCwDecP.Text = "<<<";
            this.buttonCwDecP.UseVisualStyleBackColor = true;
            // 
            // comboBoxTenma
            // 
            this.comboBoxTenma.FormattingEnabled = true;
            this.comboBoxTenma.Location = new System.Drawing.Point(147, 17);
            this.comboBoxTenma.Name = "comboBoxTenma";
            this.comboBoxTenma.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTenma.TabIndex = 95;
            this.comboBoxTenma.SelectedIndexChanged += new System.EventHandler(this.comboBoxComPortTenmaToCHeck_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 13);
            this.label12.TabIndex = 94;
            this.label12.Text = "COM Port TENMA";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(492, 91);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 13);
            this.label11.TabIndex = 93;
            this.label11.Text = "Board";
            // 
            // textBoxBoard
            // 
            this.textBoxBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBoard.Enabled = false;
            this.textBoxBoard.Location = new System.Drawing.Point(547, 91);
            this.textBoxBoard.Name = "textBoxBoard";
            this.textBoxBoard.Size = new System.Drawing.Size(202, 20);
            this.textBoxBoard.TabIndex = 92;
            // 
            // textBoxFirmware
            // 
            this.textBoxFirmware.Enabled = false;
            this.textBoxFirmware.Location = new System.Drawing.Point(547, 117);
            this.textBoxFirmware.Name = "textBoxFirmware";
            this.textBoxFirmware.Size = new System.Drawing.Size(202, 20);
            this.textBoxFirmware.TabIndex = 91;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(491, 117);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 90;
            this.label9.Text = "Firmware";
            // 
            // progressBarProgram
            // 
            this.progressBarProgram.Location = new System.Drawing.Point(147, 162);
            this.progressBarProgram.Name = "progressBarProgram";
            this.progressBarProgram.Size = new System.Drawing.Size(314, 23);
            this.progressBarProgram.TabIndex = 89;
            // 
            // textBoxSn
            // 
            this.textBoxSn.Location = new System.Drawing.Point(100, 120);
            this.textBoxSn.Name = "textBoxSn";
            this.textBoxSn.Size = new System.Drawing.Size(46, 20);
            this.textBoxSn.TabIndex = 88;
            this.textBoxSn.TextChanged += new System.EventHandler(this.textBoxSn_TextChanged);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(22, 162);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(119, 23);
            this.buttonStart.TabIndex = 87;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 86;
            this.label3.Text = "Serial Number";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(18, 284);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(204, 31);
            this.labelStatus.TabIndex = 85;
            this.labelStatus.Text = "Label for Status";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(251, 349);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(238, 157);
            this.textBoxLog.TabIndex = 84;
            // 
            // textBoxUartLog
            // 
            this.textBoxUartLog.Location = new System.Drawing.Point(7, 349);
            this.textBoxUartLog.Multiline = true;
            this.textBoxUartLog.Name = "textBoxUartLog";
            this.textBoxUartLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxUartLog.Size = new System.Drawing.Size(238, 157);
            this.textBoxUartLog.TabIndex = 83;
            // 
            // comboBoxComPortToTest
            // 
            this.comboBoxComPortToTest.FormattingEnabled = true;
            this.comboBoxComPortToTest.Location = new System.Drawing.Point(147, 74);
            this.comboBoxComPortToTest.Name = "comboBoxComPortToTest";
            this.comboBoxComPortToTest.Size = new System.Drawing.Size(121, 21);
            this.comboBoxComPortToTest.TabIndex = 82;
            this.comboBoxComPortToTest.SelectedIndexChanged += new System.EventHandler(this.comboBoxComPortToCHeck_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 81;
            this.label2.Text = "COM Port PCB à tester ";
            // 
            // BgW_Test
            // 
            this.BgW_Test.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BgW_Test_DoWork);
            this.BgW_Test.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BgW_Test_RunWorkerCompleted);
            // 
            // buttonUSB
            // 
            this.buttonUSB.Location = new System.Drawing.Point(915, 225);
            this.buttonUSB.Name = "buttonUSB";
            this.buttonUSB.Size = new System.Drawing.Size(75, 23);
            this.buttonUSB.TabIndex = 107;
            this.buttonUSB.Text = "buttonUSB";
            this.buttonUSB.UseVisualStyleBackColor = true;
            this.buttonUSB.Click += new System.EventHandler(this.buttonUSB_Click);
            // 
            // buttonADC
            // 
            this.buttonADC.Location = new System.Drawing.Point(915, 254);
            this.buttonADC.Name = "buttonADC";
            this.buttonADC.Size = new System.Drawing.Size(75, 23);
            this.buttonADC.TabIndex = 108;
            this.buttonADC.Text = "buttonADC";
            this.buttonADC.UseVisualStyleBackColor = true;
            this.buttonADC.Click += new System.EventHandler(this.buttonADC_Click);
            // 
            // buttonProgram
            // 
            this.buttonProgram.Location = new System.Drawing.Point(915, 284);
            this.buttonProgram.Name = "buttonProgram";
            this.buttonProgram.Size = new System.Drawing.Size(75, 23);
            this.buttonProgram.TabIndex = 109;
            this.buttonProgram.Text = "Program";
            this.buttonProgram.UseVisualStyleBackColor = true;
            this.buttonProgram.Click += new System.EventHandler(this.buttonProgram_Click);
            // 
            // buttonTenma
            // 
            this.buttonTenma.Location = new System.Drawing.Point(915, 313);
            this.buttonTenma.Name = "buttonTenma";
            this.buttonTenma.Size = new System.Drawing.Size(75, 23);
            this.buttonTenma.TabIndex = 110;
            this.buttonTenma.Text = "Tenma";
            this.buttonTenma.UseVisualStyleBackColor = true;
            this.buttonTenma.Click += new System.EventHandler(this.buttonTenma_Click);
            // 
            // buttonCharge
            // 
            this.buttonCharge.Location = new System.Drawing.Point(915, 342);
            this.buttonCharge.Name = "buttonCharge";
            this.buttonCharge.Size = new System.Drawing.Size(75, 23);
            this.buttonCharge.TabIndex = 111;
            this.buttonCharge.Text = "Charge";
            this.buttonCharge.UseVisualStyleBackColor = true;
            this.buttonCharge.Click += new System.EventHandler(this.buttonCharge_Click);
            // 
            // buttonSwitch
            // 
            this.buttonSwitch.Location = new System.Drawing.Point(915, 371);
            this.buttonSwitch.Name = "buttonSwitch";
            this.buttonSwitch.Size = new System.Drawing.Size(75, 23);
            this.buttonSwitch.TabIndex = 112;
            this.buttonSwitch.Text = "Switch";
            this.buttonSwitch.UseVisualStyleBackColor = true;
            this.buttonSwitch.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBoxSwitch
            // 
            this.groupBoxSwitch.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBoxSwitch.Controls.Add(this.checkBoxInc);
            this.groupBoxSwitch.Controls.Add(this.checkBoxDec);
            this.groupBoxSwitch.Controls.Add(this.checkBoxCfg);
            this.groupBoxSwitch.Controls.Add(this.checkBox2B);
            this.groupBoxSwitch.Controls.Add(this.checkBox2A);
            this.groupBoxSwitch.Controls.Add(this.checkBox1B);
            this.groupBoxSwitch.Controls.Add(this.checkBox1A);
            this.groupBoxSwitch.Location = new System.Drawing.Point(759, 201);
            this.groupBoxSwitch.Name = "groupBoxSwitch";
            this.groupBoxSwitch.Size = new System.Drawing.Size(125, 198);
            this.groupBoxSwitch.TabIndex = 113;
            this.groupBoxSwitch.TabStop = false;
            this.groupBoxSwitch.Text = "Switch test";
            // 
            // checkBoxInc
            // 
            this.checkBoxInc.AutoSize = true;
            this.checkBoxInc.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxInc.Location = new System.Drawing.Point(13, 166);
            this.checkBoxInc.Name = "checkBoxInc";
            this.checkBoxInc.Size = new System.Drawing.Size(84, 17);
            this.checkBoxInc.TabIndex = 13;
            this.checkBoxInc.Text = "Rotative Inc";
            this.checkBoxInc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxInc.UseVisualStyleBackColor = true;
            // 
            // checkBoxDec
            // 
            this.checkBoxDec.AutoSize = true;
            this.checkBoxDec.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxDec.Location = new System.Drawing.Point(8, 143);
            this.checkBoxDec.Name = "checkBoxDec";
            this.checkBoxDec.Size = new System.Drawing.Size(89, 17);
            this.checkBoxDec.TabIndex = 12;
            this.checkBoxDec.Text = "Rotative Dec";
            this.checkBoxDec.UseVisualStyleBackColor = true;
            // 
            // checkBoxCfg
            // 
            this.checkBoxCfg.AutoSize = true;
            this.checkBoxCfg.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxCfg.Location = new System.Drawing.Point(19, 120);
            this.checkBoxCfg.Name = "checkBoxCfg";
            this.checkBoxCfg.Size = new System.Drawing.Size(78, 17);
            this.checkBoxCfg.TabIndex = 11;
            this.checkBoxCfg.Text = "Central Cfg";
            this.checkBoxCfg.UseVisualStyleBackColor = true;
            // 
            // checkBox2B
            // 
            this.checkBox2B.AutoSize = true;
            this.checkBox2B.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox2B.Location = new System.Drawing.Point(6, 97);
            this.checkBox2B.Name = "checkBox2B";
            this.checkBox2B.Size = new System.Drawing.Size(91, 17);
            this.checkBox2B.TabIndex = 10;
            this.checkBox2B.Text = "Side Right 2B";
            this.checkBox2B.UseVisualStyleBackColor = true;
            // 
            // checkBox2A
            // 
            this.checkBox2A.AutoSize = true;
            this.checkBox2A.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox2A.Location = new System.Drawing.Point(6, 74);
            this.checkBox2A.Name = "checkBox2A";
            this.checkBox2A.Size = new System.Drawing.Size(91, 17);
            this.checkBox2A.TabIndex = 9;
            this.checkBox2A.Text = "Side Right 2A";
            this.checkBox2A.UseVisualStyleBackColor = true;
            // 
            // checkBox1B
            // 
            this.checkBox1B.AutoSize = true;
            this.checkBox1B.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1B.Location = new System.Drawing.Point(13, 51);
            this.checkBox1B.Name = "checkBox1B";
            this.checkBox1B.Size = new System.Drawing.Size(84, 17);
            this.checkBox1B.TabIndex = 8;
            this.checkBox1B.Text = "Side Left 1B";
            this.checkBox1B.UseVisualStyleBackColor = true;
            // 
            // checkBox1A
            // 
            this.checkBox1A.AutoSize = true;
            this.checkBox1A.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1A.ForeColor = System.Drawing.Color.Black;
            this.checkBox1A.Location = new System.Drawing.Point(13, 28);
            this.checkBox1A.Name = "checkBox1A";
            this.checkBox1A.Size = new System.Drawing.Size(84, 17);
            this.checkBox1A.TabIndex = 7;
            this.checkBox1A.Text = "Side Left 1A";
            this.checkBox1A.UseVisualStyleBackColor = true;
            // 
            // chartSpectrum
            // 
            chartArea1.Name = "ChartArea";
            this.chartSpectrum.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartSpectrum.Legends.Add(legend1);
            this.chartSpectrum.Location = new System.Drawing.Point(7, 531);
            this.chartSpectrum.Name = "chartSpectrum";
            series1.ChartArea = "ChartArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.Name = "SeriesSpectrum";
            this.chartSpectrum.Series.Add(series1);
            this.chartSpectrum.Size = new System.Drawing.Size(1020, 184);
            this.chartSpectrum.TabIndex = 114;
            this.chartSpectrum.Text = "chart1";
            // 
            // buttonSxTx
            // 
            this.buttonSxTx.Location = new System.Drawing.Point(915, 400);
            this.buttonSxTx.Name = "buttonSxTx";
            this.buttonSxTx.Size = new System.Drawing.Size(75, 23);
            this.buttonSxTx.TabIndex = 115;
            this.buttonSxTx.Text = "SX TX";
            this.buttonSxTx.UseVisualStyleBackColor = true;
            this.buttonSxTx.Click += new System.EventHandler(this.buttonSxTx_Click);
            // 
            // buttonFk
            // 
            this.buttonFk.Location = new System.Drawing.Point(915, 429);
            this.buttonFk.Name = "buttonFk";
            this.buttonFk.Size = new System.Drawing.Size(75, 23);
            this.buttonFk.TabIndex = 116;
            this.buttonFk.Text = "Finger Kick";
            this.buttonFk.UseVisualStyleBackColor = true;
            this.buttonFk.Click += new System.EventHandler(this.buttonFk_Click);
            // 
            // buttonVib
            // 
            this.buttonVib.Location = new System.Drawing.Point(915, 458);
            this.buttonVib.Name = "buttonVib";
            this.buttonVib.Size = new System.Drawing.Size(75, 23);
            this.buttonVib.TabIndex = 117;
            this.buttonVib.Text = "Vibration";
            this.buttonVib.UseVisualStyleBackColor = true;
            this.buttonVib.Click += new System.EventHandler(this.buttonVib_Click);
            // 
            // buttonBuzzer
            // 
            this.buttonBuzzer.Location = new System.Drawing.Point(915, 487);
            this.buttonBuzzer.Name = "buttonBuzzer";
            this.buttonBuzzer.Size = new System.Drawing.Size(75, 23);
            this.buttonBuzzer.TabIndex = 118;
            this.buttonBuzzer.Text = "Buzzer";
            this.buttonBuzzer.UseVisualStyleBackColor = true;
            this.buttonBuzzer.Click += new System.EventHandler(this.buttonBuzzer_Click);
            // 
            // instantDoCtrl1
            // 
            this.instantDoCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("instantDoCtrl1._StateStream")));
            // 
            // instantAiCtrl1
            // 
            this.instantAiCtrl1._StateStream = ((Automation.BDaq.DeviceStateStreamer)(resources.GetObject("instantAiCtrl1._StateStream")));
            // 
            // buttonGetMac
            // 
            this.buttonGetMac.Location = new System.Drawing.Point(759, 416);
            this.buttonGetMac.Name = "buttonGetMac";
            this.buttonGetMac.Size = new System.Drawing.Size(75, 23);
            this.buttonGetMac.TabIndex = 119;
            this.buttonGetMac.Text = "GetMac";
            this.buttonGetMac.UseVisualStyleBackColor = true;
            this.buttonGetMac.Click += new System.EventHandler(this.buttonGetMac_Click);
            // 
            // buttonBleConnect
            // 
            this.buttonBleConnect.Location = new System.Drawing.Point(759, 445);
            this.buttonBleConnect.Name = "buttonBleConnect";
            this.buttonBleConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonBleConnect.TabIndex = 120;
            this.buttonBleConnect.Text = "Ble Connect";
            this.buttonBleConnect.UseVisualStyleBackColor = true;
            this.buttonBleConnect.Click += new System.EventHandler(this.buttonBleConnect_Click);
            // 
            // buttonSN
            // 
            this.buttonSN.Location = new System.Drawing.Point(759, 474);
            this.buttonSN.Name = "buttonSN";
            this.buttonSN.Size = new System.Drawing.Size(75, 23);
            this.buttonSN.TabIndex = 121;
            this.buttonSN.Text = "SN";
            this.buttonSN.UseVisualStyleBackColor = true;
            this.buttonSN.Click += new System.EventHandler(this.buttonSN_Click);
            // 
            // P4CttTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 727);
            this.Controls.Add(this.buttonSN);
            this.Controls.Add(this.buttonBleConnect);
            this.Controls.Add(this.buttonGetMac);
            this.Controls.Add(this.buttonBuzzer);
            this.Controls.Add(this.buttonVib);
            this.Controls.Add(this.buttonFk);
            this.Controls.Add(this.buttonSxTx);
            this.Controls.Add(this.chartSpectrum);
            this.Controls.Add(this.groupBoxSwitch);
            this.Controls.Add(this.buttonSwitch);
            this.Controls.Add(this.buttonCharge);
            this.Controls.Add(this.buttonTenma);
            this.Controls.Add(this.buttonProgram);
            this.Controls.Add(this.buttonADC);
            this.Controls.Add(this.buttonUSB);
            this.Controls.Add(this.buttonWpc);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonErrorCW);
            this.Controls.Add(this.comboBoxComGolden);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCwInc);
            this.Controls.Add(this.buttonCwIncP);
            this.Controls.Add(this.buttonCwDec);
            this.Controls.Add(this.buttonCwSave);
            this.Controls.Add(this.buttonCwDecP);
            this.Controls.Add(this.comboBoxTenma);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxBoard);
            this.Controls.Add(this.textBoxFirmware);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.progressBarProgram);
            this.Controls.Add(this.textBoxSn);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.textBoxUartLog);
            this.Controls.Add(this.comboBoxComPortToTest);
            this.Controls.Add(this.label2);
            this.Name = "P4CttTestForm";
            this.Text = "P4 CTT 4 Test";
            this.Load += new System.EventHandler(this.P4CttTestForm_Load);
            this.groupBoxSwitch.ResumeLayout(false);
            this.groupBoxSwitch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSpectrum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonWpc;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonErrorCW;
        private System.Windows.Forms.ComboBox comboBoxComGolden;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCwInc;
        private System.Windows.Forms.Button buttonCwIncP;
        private System.Windows.Forms.Button buttonCwDec;
        private System.Windows.Forms.Button buttonCwSave;
        private System.Windows.Forms.Button buttonCwDecP;
        private System.Windows.Forms.ComboBox comboBoxTenma;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxBoard;
        private System.Windows.Forms.TextBox textBoxFirmware;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ProgressBar progressBarProgram;
        private System.Windows.Forms.TextBox textBoxSn;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.TextBox textBoxUartLog;
        private System.Windows.Forms.ComboBox comboBoxComPortToTest;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker BgW_Test;
        private System.Windows.Forms.Button buttonUSB;
        private System.Windows.Forms.Button buttonADC;
        private System.Windows.Forms.Button buttonProgram;
        private System.Windows.Forms.Button buttonTenma;
        private System.Windows.Forms.Button buttonCharge;
        private System.Windows.Forms.Button buttonSwitch;
        private System.Windows.Forms.GroupBox groupBoxSwitch;
        private System.Windows.Forms.CheckBox checkBoxInc;
        private System.Windows.Forms.CheckBox checkBoxDec;
        private System.Windows.Forms.CheckBox checkBoxCfg;
        private System.Windows.Forms.CheckBox checkBox2B;
        private System.Windows.Forms.CheckBox checkBox2A;
        private System.Windows.Forms.CheckBox checkBox1B;
        private System.Windows.Forms.CheckBox checkBox1A;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSpectrum;
        private System.Windows.Forms.Button buttonSxTx;
        private System.Windows.Forms.Button buttonFk;
        private System.Windows.Forms.Button buttonVib;
        private System.Windows.Forms.Button buttonBuzzer;
        private Automation.BDaq.InstantDoCtrl instantDoCtrl1;
        private Automation.BDaq.InstantAiCtrl instantAiCtrl1;
        private System.Windows.Forms.Button buttonGetMac;
        private System.Windows.Forms.Button buttonBleConnect;
        private System.Windows.Forms.Button buttonSN;
    }
}

