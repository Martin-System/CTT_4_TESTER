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
            this.button1 = new System.Windows.Forms.Button();
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
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(618, 182);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 106;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
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
            this.labelStatus.Location = new System.Drawing.Point(10, 270);
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
            this.comboBoxComPortToTest.SelectedIndexChanged += new System.EventHandler(this.comboBoxComPortTenmaToCHeck_SelectedIndexChanged);
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
            // P4CttTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 727);
            this.Controls.Add(this.button1);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
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
    }
}

