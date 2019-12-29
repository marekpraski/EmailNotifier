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
            this.deleteEmailsCheckBox = new System.Windows.Forms.CheckBox();
            this.helpLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numberOfEmailsAtSetupTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.helpLabel1 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "check for email every";
            // 
            // checkEmailTimerTextbox
            // 
            this.checkEmailTimerTextbox.Location = new System.Drawing.Point(122, 55);
            this.checkEmailTimerTextbox.Name = "checkEmailTimerTextbox";
            this.checkEmailTimerTextbox.Size = new System.Drawing.Size(33, 20);
            this.checkEmailTimerTextbox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(161, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "minutes";
            this.label2.MouseEnter += new System.EventHandler(this.Label2_MouseEnter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "keep";
            // 
            // numberOfEmailsKeptTextbox
            // 
            this.numberOfEmailsKeptTextbox.Location = new System.Drawing.Point(45, 114);
            this.numberOfEmailsKeptTextbox.Name = "numberOfEmailsKeptTextbox";
            this.numberOfEmailsKeptTextbox.Size = new System.Drawing.Size(47, 20);
            this.numberOfEmailsKeptTextbox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(98, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "read emails";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "display notification bubble every";
            // 
            // notificationTimerTextbox
            // 
            this.notificationTimerTextbox.Location = new System.Drawing.Point(171, 84);
            this.notificationTimerTextbox.Name = "notificationTimerTextbox";
            this.notificationTimerTextbox.Size = new System.Drawing.Size(33, 20);
            this.notificationTimerTextbox.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label6.Location = new System.Drawing.Point(211, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 8;
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
            // deleteEmailsCheckBox
            // 
            this.deleteEmailsCheckBox.AutoSize = true;
            this.deleteEmailsCheckBox.Location = new System.Drawing.Point(11, 143);
            this.deleteEmailsCheckBox.Name = "deleteEmailsCheckBox";
            this.deleteEmailsCheckBox.Size = new System.Drawing.Size(210, 17);
            this.deleteEmailsCheckBox.TabIndex = 10;
            this.deleteEmailsCheckBox.Text = "delete checked new emails from server";
            this.deleteEmailsCheckBox.UseVisualStyleBackColor = true;
            // 
            // helpLabel
            // 
            this.helpLabel.AutoSize = true;
            this.helpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.helpLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.helpLabel.Location = new System.Drawing.Point(217, 142);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(15, 15);
            this.helpLabel.TabIndex = 11;
            this.helpLabel.Text = "?";
            this.helpLabel.MouseEnter += new System.EventHandler(this.HelpLabel_MouseEnter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(96, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "emails from server at setup";
            // 
            // numberOfEmailsAtSetupTextBox
            // 
            this.numberOfEmailsAtSetupTextBox.Location = new System.Drawing.Point(44, 32);
            this.numberOfEmailsAtSetupTextBox.Name = "numberOfEmailsAtSetupTextBox";
            this.numberOfEmailsAtSetupTextBox.Size = new System.Drawing.Size(47, 20);
            this.numberOfEmailsAtSetupTextBox.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "read";
            // 
            // helpLabel1
            // 
            this.helpLabel1.AutoSize = true;
            this.helpLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.helpLabel1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.helpLabel1.Location = new System.Drawing.Point(228, 32);
            this.helpLabel1.Name = "helpLabel1";
            this.helpLabel1.Size = new System.Drawing.Size(15, 15);
            this.helpLabel1.TabIndex = 15;
            this.helpLabel1.Text = "?";
            this.helpLabel1.MouseEnter += new System.EventHandler(this.HelpLabel1_MouseEnter);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 170);
            this.Controls.Add(this.helpLabel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numberOfEmailsAtSetupTextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.helpLabel);
            this.Controls.Add(this.deleteEmailsCheckBox);
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
        private System.Windows.Forms.CheckBox deleteEmailsCheckBox;
        private System.Windows.Forms.Label helpLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox numberOfEmailsAtSetupTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label helpLabel1;
    }
}