namespace TestFoem
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lbText = new Label();
            tbText = new TextBox();
            lbSpeakerId = new Label();
            tbSpeakerId = new TextBox();
            tpSpeakerSetting = new TabPage();
            btText2File = new Button();
            gbPauseSetting = new GroupBox();
            tbKuTen = new TextBox();
            tbToTen = new TextBox();
            lbKuTen = new Label();
            trcbrKuTen = new TrackBar();
            lbToTen = new Label();
            trcbrToTen = new TrackBar();
            gbVoiceSetting = new GroupBox();
            trcbrIntonation = new TrackBar();
            lbIntonation = new Label();
            tbIntonation = new TextBox();
            tbPitch = new TextBox();
            lbSpeed = new Label();
            tbSpeed = new TextBox();
            lbVolume = new Label();
            tbVolume = new TextBox();
            trcbrSpeed = new TrackBar();
            trcbrPitch = new TrackBar();
            lbPitch = new Label();
            trcbrVolume = new TrackBar();
            btText2Speech = new Button();
            tcTab = new TabControl();
            tpSpeakerSetting.SuspendLayout();
            gbPauseSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trcbrKuTen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trcbrToTen).BeginInit();
            gbVoiceSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trcbrIntonation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trcbrSpeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trcbrPitch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trcbrVolume).BeginInit();
            tcTab.SuspendLayout();
            SuspendLayout();
            // 
            // lbText
            // 
            lbText.AutoSize = true;
            lbText.Location = new Point(14, 71);
            lbText.Name = "lbText";
            lbText.Size = new Size(43, 20);
            lbText.TabIndex = 0;
            lbText.Text = "・Text";
            // 
            // tbText
            // 
            tbText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbText.Location = new Point(14, 95);
            tbText.Margin = new Padding(3, 4, 3, 4);
            tbText.MaxLength = 200;
            tbText.Multiline = true;
            tbText.Name = "tbText";
            tbText.Size = new Size(479, 40);
            tbText.TabIndex = 1;
            // 
            // lbSpeakerId
            // 
            lbSpeakerId.AutoSize = true;
            lbSpeakerId.Location = new Point(14, 12);
            lbSpeakerId.Name = "lbSpeakerId";
            lbSpeakerId.Size = new Size(83, 20);
            lbSpeakerId.TabIndex = 3;
            lbSpeakerId.Text = "・SpeakerId";
            // 
            // tbSpeakerId
            // 
            tbSpeakerId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbSpeakerId.Location = new Point(14, 36);
            tbSpeakerId.Margin = new Padding(3, 4, 3, 4);
            tbSpeakerId.MaxLength = 100;
            tbSpeakerId.Name = "tbSpeakerId";
            tbSpeakerId.Size = new Size(479, 27);
            tbSpeakerId.TabIndex = 4;
            // 
            // tpSpeakerSetting
            // 
            tpSpeakerSetting.Controls.Add(btText2File);
            tpSpeakerSetting.Controls.Add(gbPauseSetting);
            tpSpeakerSetting.Controls.Add(gbVoiceSetting);
            tpSpeakerSetting.Controls.Add(btText2Speech);
            tpSpeakerSetting.Location = new Point(4, 29);
            tpSpeakerSetting.Margin = new Padding(3, 4, 3, 4);
            tpSpeakerSetting.Name = "tpSpeakerSetting";
            tpSpeakerSetting.Padding = new Padding(3, 4, 3, 4);
            tpSpeakerSetting.Size = new Size(472, 432);
            tpSpeakerSetting.TabIndex = 0;
            tpSpeakerSetting.Text = "Speaker Setting";
            tpSpeakerSetting.UseVisualStyleBackColor = true;
            // 
            // btText2File
            // 
            btText2File.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btText2File.Location = new Point(252, 389);
            btText2File.Margin = new Padding(3, 4, 3, 4);
            btText2File.Name = "btText2File";
            btText2File.Size = new Size(103, 31);
            btText2File.TabIndex = 7;
            btText2File.Text = "Text2File";
            btText2File.UseVisualStyleBackColor = true;
            btText2File.Click += btText2File_Click;
            // 
            // gbPauseSetting
            // 
            gbPauseSetting.Controls.Add(tbKuTen);
            gbPauseSetting.Controls.Add(tbToTen);
            gbPauseSetting.Controls.Add(lbKuTen);
            gbPauseSetting.Controls.Add(trcbrKuTen);
            gbPauseSetting.Controls.Add(lbToTen);
            gbPauseSetting.Controls.Add(trcbrToTen);
            gbPauseSetting.Location = new Point(311, 8);
            gbPauseSetting.Margin = new Padding(3, 4, 3, 4);
            gbPauseSetting.Name = "gbPauseSetting";
            gbPauseSetting.Padding = new Padding(3, 4, 3, 4);
            gbPauseSetting.Size = new Size(149, 373);
            gbPauseSetting.TabIndex = 6;
            gbPauseSetting.TabStop = false;
            gbPauseSetting.Text = "Pause Setting";
            // 
            // tbKuTen
            // 
            tbKuTen.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tbKuTen.ImeMode = ImeMode.Off;
            tbKuTen.Location = new Point(85, 335);
            tbKuTen.Margin = new Padding(3, 4, 3, 4);
            tbKuTen.Name = "tbKuTen";
            tbKuTen.Size = new Size(63, 27);
            tbKuTen.TabIndex = 19;
            tbKuTen.Text = "0.7";
            tbKuTen.TextAlign = HorizontalAlignment.Center;
            tbKuTen.Validated += tbKuTen_Validated;
            // 
            // tbToTen
            // 
            tbToTen.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tbToTen.ImeMode = ImeMode.Off;
            tbToTen.Location = new Point(7, 335);
            tbToTen.Margin = new Padding(3, 4, 3, 4);
            tbToTen.Name = "tbToTen";
            tbToTen.Size = new Size(63, 27);
            tbToTen.TabIndex = 16;
            tbToTen.Text = "0.4";
            tbToTen.TextAlign = HorizontalAlignment.Center;
            tbToTen.Validated += tbToTen_Validated;
            // 
            // lbKuTen
            // 
            lbKuTen.AutoSize = true;
            lbKuTen.Location = new Point(78, 33);
            lbKuTen.Name = "lbKuTen";
            lbKuTen.Size = new Size(42, 20);
            lbKuTen.TabIndex = 18;
            lbKuTen.Text = "Long";
            // 
            // trcbrKuTen
            // 
            trcbrKuTen.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            trcbrKuTen.Location = new Point(78, 68);
            trcbrKuTen.Margin = new Padding(3, 4, 3, 4);
            trcbrKuTen.Maximum = 50;
            trcbrKuTen.Name = "trcbrKuTen";
            trcbrKuTen.Orientation = Orientation.Vertical;
            trcbrKuTen.Size = new Size(56, 259);
            trcbrKuTen.TabIndex = 17;
            trcbrKuTen.TickFrequency = 5;
            trcbrKuTen.TickStyle = TickStyle.Both;
            trcbrKuTen.Value = 7;
            trcbrKuTen.Scroll += trcbrKuTen_Scroll;
            // 
            // lbToTen
            // 
            lbToTen.AutoSize = true;
            lbToTen.Location = new Point(7, 33);
            lbToTen.Name = "lbToTen";
            lbToTen.Size = new Size(44, 20);
            lbToTen.TabIndex = 16;
            lbToTen.Text = "Short";
            // 
            // trcbrToTen
            // 
            trcbrToTen.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            trcbrToTen.LargeChange = 4;
            trcbrToTen.Location = new Point(7, 68);
            trcbrToTen.Margin = new Padding(3, 4, 3, 4);
            trcbrToTen.Maximum = 20;
            trcbrToTen.Minimum = 2;
            trcbrToTen.Name = "trcbrToTen";
            trcbrToTen.Orientation = Orientation.Vertical;
            trcbrToTen.Size = new Size(56, 259);
            trcbrToTen.TabIndex = 16;
            trcbrToTen.TickFrequency = 2;
            trcbrToTen.TickStyle = TickStyle.Both;
            trcbrToTen.Value = 4;
            trcbrToTen.Scroll += trcbrToTen_Scroll;
            // 
            // gbVoiceSetting
            // 
            gbVoiceSetting.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            gbVoiceSetting.Controls.Add(trcbrIntonation);
            gbVoiceSetting.Controls.Add(lbIntonation);
            gbVoiceSetting.Controls.Add(tbIntonation);
            gbVoiceSetting.Controls.Add(tbPitch);
            gbVoiceSetting.Controls.Add(lbSpeed);
            gbVoiceSetting.Controls.Add(tbSpeed);
            gbVoiceSetting.Controls.Add(lbVolume);
            gbVoiceSetting.Controls.Add(tbVolume);
            gbVoiceSetting.Controls.Add(trcbrSpeed);
            gbVoiceSetting.Controls.Add(trcbrPitch);
            gbVoiceSetting.Controls.Add(lbPitch);
            gbVoiceSetting.Controls.Add(trcbrVolume);
            gbVoiceSetting.Location = new Point(7, 8);
            gbVoiceSetting.Margin = new Padding(3, 4, 3, 4);
            gbVoiceSetting.Name = "gbVoiceSetting";
            gbVoiceSetting.Padding = new Padding(3, 4, 3, 4);
            gbVoiceSetting.Size = new Size(297, 373);
            gbVoiceSetting.TabIndex = 5;
            gbVoiceSetting.TabStop = false;
            gbVoiceSetting.Text = "Voice Setting";
            // 
            // trcbrIntonation
            // 
            trcbrIntonation.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            trcbrIntonation.Location = new Point(219, 68);
            trcbrIntonation.Margin = new Padding(3, 4, 3, 4);
            trcbrIntonation.Maximum = 200;
            trcbrIntonation.Name = "trcbrIntonation";
            trcbrIntonation.Orientation = Orientation.Vertical;
            trcbrIntonation.Size = new Size(56, 259);
            trcbrIntonation.TabIndex = 15;
            trcbrIntonation.TickFrequency = 2;
            trcbrIntonation.TickStyle = TickStyle.Both;
            trcbrIntonation.Value = 100;
            trcbrIntonation.Scroll += trcbrIntonation_Scroll;
            // 
            // lbIntonation
            // 
            lbIntonation.AutoSize = true;
            lbIntonation.Location = new Point(213, 33);
            lbIntonation.Name = "lbIntonation";
            lbIntonation.Size = new Size(77, 20);
            lbIntonation.TabIndex = 14;
            lbIntonation.Text = "Intonation";
            // 
            // tbIntonation
            // 
            tbIntonation.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tbIntonation.ImeMode = ImeMode.Off;
            tbIntonation.Location = new Point(219, 335);
            tbIntonation.Margin = new Padding(3, 4, 3, 4);
            tbIntonation.Name = "tbIntonation";
            tbIntonation.Size = new Size(63, 27);
            tbIntonation.TabIndex = 13;
            tbIntonation.Text = "1.0";
            tbIntonation.TextAlign = HorizontalAlignment.Center;
            tbIntonation.Validated += tbIntonation_Validated;
            // 
            // tbPitch
            // 
            tbPitch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tbPitch.ImeMode = ImeMode.Off;
            tbPitch.Location = new Point(149, 335);
            tbPitch.Margin = new Padding(3, 4, 3, 4);
            tbPitch.Name = "tbPitch";
            tbPitch.Size = new Size(63, 27);
            tbPitch.TabIndex = 12;
            tbPitch.Text = "1.0";
            tbPitch.TextAlign = HorizontalAlignment.Center;
            tbPitch.Validated += tbPitch_Validated;
            // 
            // lbSpeed
            // 
            lbSpeed.AutoSize = true;
            lbSpeed.Location = new Point(78, 33);
            lbSpeed.Name = "lbSpeed";
            lbSpeed.Size = new Size(51, 20);
            lbSpeed.TabIndex = 11;
            lbSpeed.Text = "Speed";
            // 
            // tbSpeed
            // 
            tbSpeed.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tbSpeed.ImeMode = ImeMode.Off;
            tbSpeed.Location = new Point(78, 335);
            tbSpeed.Margin = new Padding(3, 4, 3, 4);
            tbSpeed.Name = "tbSpeed";
            tbSpeed.Size = new Size(63, 27);
            tbSpeed.TabIndex = 10;
            tbSpeed.Text = "1.0";
            tbSpeed.TextAlign = HorizontalAlignment.Center;
            tbSpeed.Validated += tbSpeed_Validated;
            // 
            // lbVolume
            // 
            lbVolume.AutoSize = true;
            lbVolume.Location = new Point(7, 33);
            lbVolume.Name = "lbVolume";
            lbVolume.Size = new Size(59, 20);
            lbVolume.TabIndex = 9;
            lbVolume.Text = "Volume";
            // 
            // tbVolume
            // 
            tbVolume.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            tbVolume.ImeMode = ImeMode.Off;
            tbVolume.Location = new Point(7, 335);
            tbVolume.Margin = new Padding(3, 4, 3, 4);
            tbVolume.MaxLength = 4;
            tbVolume.Name = "tbVolume";
            tbVolume.Size = new Size(58, 27);
            tbVolume.TabIndex = 8;
            tbVolume.Text = "1.0";
            tbVolume.TextAlign = HorizontalAlignment.Center;
            tbVolume.Validated += tbVolume_Validated;
            // 
            // trcbrSpeed
            // 
            trcbrSpeed.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            trcbrSpeed.Location = new Point(78, 68);
            trcbrSpeed.Margin = new Padding(3, 4, 3, 4);
            trcbrSpeed.Maximum = 400;
            trcbrSpeed.Minimum = 50;
            trcbrSpeed.Name = "trcbrSpeed";
            trcbrSpeed.Orientation = Orientation.Vertical;
            trcbrSpeed.Size = new Size(56, 259);
            trcbrSpeed.TabIndex = 7;
            trcbrSpeed.TickFrequency = 10;
            trcbrSpeed.TickStyle = TickStyle.Both;
            trcbrSpeed.Value = 100;
            trcbrSpeed.Scroll += trcbrSpeed_Scroll;
            // 
            // trcbrPitch
            // 
            trcbrPitch.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            trcbrPitch.Location = new Point(149, 68);
            trcbrPitch.Margin = new Padding(3, 4, 3, 4);
            trcbrPitch.Maximum = 200;
            trcbrPitch.Name = "trcbrPitch";
            trcbrPitch.Orientation = Orientation.Vertical;
            trcbrPitch.Size = new Size(56, 259);
            trcbrPitch.TabIndex = 4;
            trcbrPitch.TickFrequency = 600;
            trcbrPitch.TickStyle = TickStyle.Both;
            trcbrPitch.Value = 100;
            trcbrPitch.Scroll += trcbrPitch_Scroll;
            // 
            // lbPitch
            // 
            lbPitch.AutoSize = true;
            lbPitch.Location = new Point(149, 33);
            lbPitch.Name = "lbPitch";
            lbPitch.Size = new Size(41, 20);
            lbPitch.TabIndex = 3;
            lbPitch.Text = "Pitch";
            // 
            // trcbrVolume
            // 
            trcbrVolume.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            trcbrVolume.Location = new Point(7, 68);
            trcbrVolume.Margin = new Padding(3, 4, 3, 4);
            trcbrVolume.Maximum = 200;
            trcbrVolume.Name = "trcbrVolume";
            trcbrVolume.Orientation = Orientation.Vertical;
            trcbrVolume.Size = new Size(56, 259);
            trcbrVolume.TabIndex = 6;
            trcbrVolume.TickFrequency = 2;
            trcbrVolume.TickStyle = TickStyle.Both;
            trcbrVolume.Value = 100;
            trcbrVolume.Scroll += trcbrVolume_Scroll;
            // 
            // btText2Speech
            // 
            btText2Speech.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btText2Speech.Location = new Point(361, 389);
            btText2Speech.Margin = new Padding(3, 4, 3, 4);
            btText2Speech.Name = "btText2Speech";
            btText2Speech.Size = new Size(103, 31);
            btText2Speech.TabIndex = 2;
            btText2Speech.Text = "Text2Speech";
            btText2Speech.UseVisualStyleBackColor = true;
            btText2Speech.Click += btText2Speech_Click;
            // 
            // tcTab
            // 
            tcTab.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tcTab.Controls.Add(tpSpeakerSetting);
            tcTab.Location = new Point(14, 144);
            tcTab.Margin = new Padding(3, 4, 3, 4);
            tcTab.Name = "tcTab";
            tcTab.SelectedIndex = 0;
            tcTab.Size = new Size(480, 465);
            tcTab.TabIndex = 16;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(507, 625);
            Controls.Add(tcTab);
            Controls.Add(tbSpeakerId);
            Controls.Add(lbSpeakerId);
            Controls.Add(tbText);
            Controls.Add(lbText);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(523, 662);
            Name = "MainForm";
            Text = "TestForm";
            tpSpeakerSetting.ResumeLayout(false);
            gbPauseSetting.ResumeLayout(false);
            gbPauseSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trcbrKuTen).EndInit();
            ((System.ComponentModel.ISupportInitialize)trcbrToTen).EndInit();
            gbVoiceSetting.ResumeLayout(false);
            gbVoiceSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trcbrIntonation).EndInit();
            ((System.ComponentModel.ISupportInitialize)trcbrSpeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)trcbrPitch).EndInit();
            ((System.ComponentModel.ISupportInitialize)trcbrVolume).EndInit();
            tcTab.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbText;
        private TextBox tbText;
        private Label lbSpeakerId;
        private TextBox tbSpeakerId;
        private TabPage tpSpeakerSetting;
        private GroupBox gbPauseSetting;
        private TextBox tbKuTen;
        private TextBox tbToTen;
        private Label lbKuTen;
        private TrackBar trcbrKuTen;
        private Label lbToTen;
        private TrackBar trcbrToTen;
        private GroupBox gbVoiceSetting;
        private TrackBar trcbrIntonation;
        private Label lbIntonation;
        private TextBox tbIntonation;
        private TextBox tbPitch;
        private Label lbSpeed;
        private TextBox tbSpeed;
        private Label lbVolume;
        private TextBox tbVolume;
        private TrackBar trcbrSpeed;
        private TrackBar trcbrPitch;
        private Label lbPitch;
        private TrackBar trcbrVolume;
        private Button btText2Speech;
        private TabControl tcTab;
        private Button btText2File;
    }
}