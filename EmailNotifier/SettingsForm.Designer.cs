namespace EmailNotifier
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
			this.label1 = new System.Windows.Forms.Label();
			this.checkEmailTimerTextbox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.numberOfEmailsKeptTextbox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.notificationTimerTextbox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.saveButton = new System.Windows.Forms.ToolStripButton();
			this.label7 = new System.Windows.Forms.Label();
			this.numberOfEmailsAtSetupTextBox = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.helpLabel1 = new System.Windows.Forms.Label();
			this.cbEnableLog = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 95);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(108, 13);
			this.label1.TabIndex = 15;
			this.label1.Text = "check for email every";
			// 
			// checkEmailTimerTextbox
			// 
			this.checkEmailTimerTextbox.Location = new System.Drawing.Point(123, 92);
			this.checkEmailTimerTextbox.Name = "checkEmailTimerTextbox";
			this.checkEmailTimerTextbox.Size = new System.Drawing.Size(33, 20);
			this.checkEmailTimerTextbox.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.label2.Location = new System.Drawing.Point(162, 95);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 13);
			this.label2.TabIndex = 14;
			this.label2.Text = "minutes";
			this.label2.MouseEnter += new System.EventHandler(this.Label2_MouseEnter);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 154);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(31, 13);
			this.label3.TabIndex = 13;
			this.label3.Text = "keep";
			// 
			// numberOfEmailsKeptTextbox
			// 
			this.numberOfEmailsKeptTextbox.Location = new System.Drawing.Point(46, 151);
			this.numberOfEmailsKeptTextbox.Name = "numberOfEmailsKeptTextbox";
			this.numberOfEmailsKeptTextbox.Size = new System.Drawing.Size(47, 20);
			this.numberOfEmailsKeptTextbox.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(99, 153);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "read emails";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 124);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(157, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "display notification bubble every";
			// 
			// notificationTimerTextbox
			// 
			this.notificationTimerTextbox.Location = new System.Drawing.Point(172, 121);
			this.notificationTimerTextbox.Name = "notificationTimerTextbox";
			this.notificationTimerTextbox.Size = new System.Drawing.Size(33, 20);
			this.notificationTimerTextbox.TabIndex = 3;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Cursor = System.Windows.Forms.Cursors.Hand;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label6.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.label6.Location = new System.Drawing.Point(212, 124);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(47, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "seconds";
			this.label6.MouseEnter += new System.EventHandler(this.Label6_MouseEnter);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(268, 25);
			this.toolStrip1.TabIndex = 9;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// saveButton
			// 
			this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
			this.saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(23, 22);
			this.saveButton.Text = "save settings";
			this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(97, 72);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(132, 13);
			this.label7.TabIndex = 1;
			this.label7.Text = "emails from server at setup";
			// 
			// numberOfEmailsAtSetupTextBox
			// 
			this.numberOfEmailsAtSetupTextBox.Location = new System.Drawing.Point(45, 69);
			this.numberOfEmailsAtSetupTextBox.Name = "numberOfEmailsAtSetupTextBox";
			this.numberOfEmailsAtSetupTextBox.Size = new System.Drawing.Size(47, 20);
			this.numberOfEmailsAtSetupTextBox.TabIndex = 1;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(12, 72);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(28, 13);
			this.label8.TabIndex = 2;
			this.label8.Text = "read";
			// 
			// helpLabel1
			// 
			this.helpLabel1.AutoSize = true;
			this.helpLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.helpLabel1.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.helpLabel1.Location = new System.Drawing.Point(229, 69);
			this.helpLabel1.Name = "helpLabel1";
			this.helpLabel1.Size = new System.Drawing.Size(15, 15);
			this.helpLabel1.TabIndex = 0;
			this.helpLabel1.Text = "?";
			this.helpLabel1.MouseEnter += new System.EventHandler(this.HelpLabel1_MouseEnter);
			// 
			// cbEnableLog
			// 
			this.cbEnableLog.AutoSize = true;
			this.cbEnableLog.Location = new System.Drawing.Point(12, 182);
			this.cbEnableLog.Name = "cbEnableLog";
			this.cbEnableLog.Size = new System.Drawing.Size(152, 17);
			this.cbEnableLog.TabIndex = 16;
			this.cbEnableLog.Text = "enable logging errors to file";
			this.cbEnableLog.UseVisualStyleBackColor = true;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(12, 29);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(229, 13);
			this.label9.TabIndex = 17;
			this.label9.Text = "saved settings are only valid for current session";
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(268, 215);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.cbEnableLog);
			this.Controls.Add(this.helpLabel1);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.numberOfEmailsAtSetupTextBox);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.notificationTimerTextbox);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.numberOfEmailsKeptTextbox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.checkEmailTimerTextbox);
			this.Controls.Add(this.label1);
			this.Name = "SettingsForm";
			this.Text = "Program settings";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox checkEmailTimerTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox numberOfEmailsKeptTextbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox notificationTimerTextbox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton saveButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox numberOfEmailsAtSetupTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label helpLabel1;
		private System.Windows.Forms.CheckBox cbEnableLog;
		private System.Windows.Forms.Label label9;
	}
}