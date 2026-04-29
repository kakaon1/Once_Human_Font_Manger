namespace OnceHumanFontManager
{
    partial class HelpForm
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
            this.rtbHelp = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            //
            // rtbHelp
            //
            this.rtbHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(248)))));
            this.rtbHelp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHelp.Font = new System.Drawing.Font("맑은 고딕", 9.5F);
            this.rtbHelp.Location = new System.Drawing.Point(12, 12);
            this.rtbHelp.Name = "rtbHelp";
            this.rtbHelp.ReadOnly = true;
            this.rtbHelp.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbHelp.Size = new System.Drawing.Size(556, 596);
            this.rtbHelp.TabIndex = 0;
            this.rtbHelp.Text = "";
            //
            // HelpForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(580, 620);
            this.Controls.Add(this.rtbHelp);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelpForm";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ONCE HUMAN 폰트매니저 — 도움말";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbHelp;
    }
}
