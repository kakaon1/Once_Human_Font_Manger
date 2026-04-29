namespace OnceHumanFontManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            lblTitle = new Label();
            lblGamePath = new Label();
            txtGamePath = new TextBox();
            btnBrowse = new Button();
            btnAutoFind = new Button();
            lblTargetPath = new Label();
            btnApply = new Button();
            btnRestore = new Button();
            btnConvert = new Button();
            btnHelp = new Button();
            lblLog = new Label();
            rtbLog = new RichTextBox();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("맑은 고딕", 12F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 9);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(516, 28);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "ONCE HUMAN 폰트 적용 도구";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblGamePath
            // 
            lblGamePath.Location = new Point(12, 56);
            lblGamePath.Name = "lblGamePath";
            lblGamePath.Size = new Size(70, 24);
            lblGamePath.TabIndex = 1;
            lblGamePath.Text = "게임 경로:";
            lblGamePath.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtGamePath
            // 
            txtGamePath.Location = new Point(84, 56);
            txtGamePath.Name = "txtGamePath";
            txtGamePath.Size = new Size(330, 23);
            txtGamePath.TabIndex = 2;
            txtGamePath.TextChanged += txtGamePath_TextChanged;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(420, 54);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(96, 28);
            btnBrowse.TabIndex = 3;
            btnBrowse.Text = "경로 찾기";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // btnAutoFind
            // 
            btnAutoFind.Location = new Point(420, 88);
            btnAutoFind.Name = "btnAutoFind";
            btnAutoFind.Size = new Size(96, 28);
            btnAutoFind.TabIndex = 4;
            btnAutoFind.Text = "자동 찾기";
            btnAutoFind.UseVisualStyleBackColor = true;
            btnAutoFind.Click += btnAutoFind_Click;
            // 
            // lblTargetPath
            // 
            lblTargetPath.ForeColor = Color.DarkBlue;
            lblTargetPath.Location = new Point(12, 122);
            lblTargetPath.Name = "lblTargetPath";
            lblTargetPath.Size = new Size(516, 22);
            lblTargetPath.TabIndex = 5;
            lblTargetPath.Text = "대상 경로: (미설정)";
            // 
            // btnApply
            // 
            btnApply.BackColor = Color.FromArgb(70, 130, 180);
            btnApply.FlatStyle = FlatStyle.Flat;
            btnApply.ForeColor = Color.White;
            btnApply.Location = new Point(12, 151);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(110, 36);
            btnApply.TabIndex = 6;
            btnApply.Text = "적용";
            btnApply.UseVisualStyleBackColor = false;
            btnApply.Click += btnApply_Click;
            // 
            // btnRestore
            // 
            btnRestore.BackColor = Color.FromArgb(180, 100, 70);
            btnRestore.FlatStyle = FlatStyle.Flat;
            btnRestore.ForeColor = Color.White;
            btnRestore.Location = new Point(130, 151);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(110, 36);
            btnRestore.TabIndex = 7;
            btnRestore.Text = "복원";
            btnRestore.UseVisualStyleBackColor = false;
            btnRestore.Click += btnRestore_Click;
            // 
            // btnConvert
            // 
            btnConvert.BackColor = Color.FromArgb(148, 102, 178);
            btnConvert.FlatStyle = FlatStyle.Flat;
            btnConvert.ForeColor = Color.White;
            btnConvert.Location = new Point(248, 151);
            btnConvert.Name = "btnConvert";
            btnConvert.Size = new Size(110, 36);
            btnConvert.TabIndex = 8;
            btnConvert.Text = "변환";
            btnConvert.UseVisualStyleBackColor = false;
            btnConvert.Click += btnConvert_Click;
            // 
            // btnHelp
            // 
            btnHelp.BackColor = Color.FromArgb(88, 158, 99);
            btnHelp.FlatStyle = FlatStyle.Flat;
            btnHelp.ForeColor = Color.White;
            btnHelp.Location = new Point(406, 151);
            btnHelp.Name = "btnHelp";
            btnHelp.Size = new Size(110, 36);
            btnHelp.TabIndex = 9;
            btnHelp.Text = "도움말";
            btnHelp.UseVisualStyleBackColor = false;
            btnHelp.Click += btnHelp_Click;
            // 
            // lblLog
            // 
            lblLog.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            lblLog.ForeColor = Color.FromArgb(60, 60, 60);
            lblLog.Location = new Point(12, 198);
            lblLog.Name = "lblLog";
            lblLog.Size = new Size(60, 18);
            lblLog.TabIndex = 10;
            lblLog.Text = "■ 로그";
            // 
            // rtbLog
            // 
            rtbLog.BackColor = Color.FromArgb(248, 248, 245);
            rtbLog.BorderStyle = BorderStyle.FixedSingle;
            rtbLog.Font = new Font("맑은 고딕", 9F);
            rtbLog.Location = new Point(12, 218);
            rtbLog.Name = "rtbLog";
            rtbLog.ReadOnly = true;
            rtbLog.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbLog.Size = new Size(516, 130);
            rtbLog.TabIndex = 11;
            rtbLog.Text = "";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(540, 360);
            Controls.Add(rtbLog);
            Controls.Add(lblLog);
            Controls.Add(btnHelp);
            Controls.Add(btnConvert);
            Controls.Add(btnRestore);
            Controls.Add(btnApply);
            Controls.Add(lblTargetPath);
            Controls.Add(btnAutoFind);
            Controls.Add(btnBrowse);
            Controls.Add(txtGamePath);
            Controls.Add(lblGamePath);
            Controls.Add(lblTitle);
            Font = new Font("맑은 고딕", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ONCE HUMAN 폰트매니저 v0.1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblGamePath;
        private System.Windows.Forms.TextBox txtGamePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnAutoFind;
        private System.Windows.Forms.Label lblTargetPath;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.RichTextBox rtbLog;
    }
}
