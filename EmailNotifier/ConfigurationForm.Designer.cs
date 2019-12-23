namespace EmailNotifier
{
    partial class ConfigurationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.addNewAccountButton = new System.Windows.Forms.ToolStripButton();
            this.deleteAccountButton = new System.Windows.Forms.ToolStripButton();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.cancelChangesButton = new System.Windows.Forms.ToolStripButton();
            this.portHelpLabel = new System.Windows.Forms.Label();
            this.accountNameHelpLabel = new System.Windows.Forms.Label();
            this.authorisationComboBox = new System.Windows.Forms.ComboBox();
            this.authorisationLabel = new System.Windows.Forms.Label();
            this.accountNameLabel = new System.Windows.Forms.Label();
            this.accountNameComboBox = new System.Windows.Forms.ComboBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.serverUrlTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.serverUrlLabel = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewAccountButton,
            this.deleteAccountButton,
            this.saveButton,
            this.cancelChangesButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(391, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // addNewAccountButton
            // 
            this.addNewAccountButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addNewAccountButton.Image = ((System.Drawing.Image)(resources.GetObject("addNewAccountButton.Image")));
            this.addNewAccountButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addNewAccountButton.Name = "addNewAccountButton";
            this.addNewAccountButton.Size = new System.Drawing.Size(23, 22);
            this.addNewAccountButton.Text = "dodaj nowe konto pocztowe";
            this.addNewAccountButton.Click += new System.EventHandler(this.AddNewAccountButton_Click);
            // 
            // deleteAccountButton
            // 
            this.deleteAccountButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteAccountButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteAccountButton.Image")));
            this.deleteAccountButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteAccountButton.Name = "deleteAccountButton";
            this.deleteAccountButton.Size = new System.Drawing.Size(23, 22);
            this.deleteAccountButton.Text = "usuń konto";
            this.deleteAccountButton.Click += new System.EventHandler(this.DeleteAccountButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
            this.saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(23, 22);
            this.saveButton.Text = "zapisz ustawienia i zamknij okno";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelChangesButton
            // 
            this.cancelChangesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cancelChangesButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelChangesButton.Image")));
            this.cancelChangesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cancelChangesButton.Name = "cancelChangesButton";
            this.cancelChangesButton.Size = new System.Drawing.Size(23, 22);
            this.cancelChangesButton.Text = "cancel changes";
            this.cancelChangesButton.Click += new System.EventHandler(this.CancelChangesButton_Click);
            // 
            // portHelpLabel
            // 
            this.portHelpLabel.AutoSize = true;
            this.portHelpLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.portHelpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.portHelpLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.portHelpLabel.Location = new System.Drawing.Point(111, 82);
            this.portHelpLabel.Name = "portHelpLabel";
            this.portHelpLabel.Size = new System.Drawing.Size(15, 15);
            this.portHelpLabel.TabIndex = 25;
            this.portHelpLabel.Text = "?";
            this.portHelpLabel.MouseEnter += new System.EventHandler(this.PortHelpLabel_MouseEnter);
            // 
            // accountNameHelpLabel
            // 
            this.accountNameHelpLabel.AutoSize = true;
            this.accountNameHelpLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.accountNameHelpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.accountNameHelpLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.accountNameHelpLabel.Location = new System.Drawing.Point(113, 32);
            this.accountNameHelpLabel.Name = "accountNameHelpLabel";
            this.accountNameHelpLabel.Size = new System.Drawing.Size(15, 15);
            this.accountNameHelpLabel.TabIndex = 24;
            this.accountNameHelpLabel.Text = "?";
            this.accountNameHelpLabel.MouseEnter += new System.EventHandler(this.urlHelpLabel_MouseEnter);
            // 
            // authorisationComboBox
            // 
            this.authorisationComboBox.FormattingEnabled = true;
            this.authorisationComboBox.Location = new System.Drawing.Point(132, 105);
            this.authorisationComboBox.Name = "authorisationComboBox";
            this.authorisationComboBox.Size = new System.Drawing.Size(100, 21);
            this.authorisationComboBox.TabIndex = 15;
            // 
            // authorisationLabel
            // 
            this.authorisationLabel.AutoSize = true;
            this.authorisationLabel.Location = new System.Drawing.Point(12, 108);
            this.authorisationLabel.Name = "authorisationLabel";
            this.authorisationLabel.Size = new System.Drawing.Size(97, 13);
            this.authorisationLabel.TabIndex = 16;
            this.authorisationLabel.Text = "TSL authentication";
            // 
            // accountNameLabel
            // 
            this.accountNameLabel.AutoSize = true;
            this.accountNameLabel.Location = new System.Drawing.Point(11, 35);
            this.accountNameLabel.Name = "accountNameLabel";
            this.accountNameLabel.Size = new System.Drawing.Size(102, 13);
            this.accountNameLabel.TabIndex = 18;
            this.accountNameLabel.Text = "email account name";
            // 
            // accountNameComboBox
            // 
            this.accountNameComboBox.FormattingEnabled = true;
            this.accountNameComboBox.Location = new System.Drawing.Point(132, 28);
            this.accountNameComboBox.Name = "accountNameComboBox";
            this.accountNameComboBox.Size = new System.Drawing.Size(240, 21);
            this.accountNameComboBox.TabIndex = 12;
            this.accountNameComboBox.SelectedIndexChanged += new System.EventHandler(this.accountNameComboBox_SelectedIndexChanged);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(132, 153);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(240, 20);
            this.passwordTextBox.TabIndex = 19;
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(132, 130);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(240, 20);
            this.userNameTextBox.TabIndex = 17;
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(132, 79);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(100, 20);
            this.portTextBox.TabIndex = 14;
            // 
            // serverUrlTextBox
            // 
            this.serverUrlTextBox.Location = new System.Drawing.Point(132, 55);
            this.serverUrlTextBox.Name = "serverUrlTextBox";
            this.serverUrlTextBox.Size = new System.Drawing.Size(240, 20);
            this.serverUrlTextBox.TabIndex = 13;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(11, 156);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(52, 13);
            this.passwordLabel.TabIndex = 20;
            this.passwordLabel.Text = "password";
            // 
            // userNameLabel
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Location = new System.Drawing.Point(12, 133);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(29, 13);
            this.userNameLabel.TabIndex = 21;
            this.userNameLabel.Text = "login";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(12, 82);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(25, 13);
            this.portLabel.TabIndex = 22;
            this.portLabel.Text = "port";
            // 
            // serverUrlLabel
            // 
            this.serverUrlLabel.AutoSize = true;
            this.serverUrlLabel.Location = new System.Drawing.Point(12, 58);
            this.serverUrlLabel.Name = "serverUrlLabel";
            this.serverUrlLabel.Size = new System.Drawing.Size(81, 13);
            this.serverUrlLabel.TabIndex = 23;
            this.serverUrlLabel.Text = "POP3 server url";
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 190);
            this.Controls.Add(this.portHelpLabel);
            this.Controls.Add(this.accountNameHelpLabel);
            this.Controls.Add(this.authorisationComboBox);
            this.Controls.Add(this.authorisationLabel);
            this.Controls.Add(this.accountNameLabel);
            this.Controls.Add(this.accountNameComboBox);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.userNameTextBox);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.serverUrlTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.userNameLabel);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.serverUrlLabel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ConfigurationForm";
            this.Text = "Email account configurations";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addNewAccountButton;
        private System.Windows.Forms.ToolStripButton saveButton;
        private System.Windows.Forms.ToolStripButton deleteAccountButton;
        private System.Windows.Forms.ToolStripButton cancelChangesButton;
        private System.Windows.Forms.Label portHelpLabel;
        private System.Windows.Forms.Label accountNameHelpLabel;
        private System.Windows.Forms.ComboBox authorisationComboBox;
        private System.Windows.Forms.Label authorisationLabel;
        private System.Windows.Forms.Label accountNameLabel;
        private System.Windows.Forms.ComboBox accountNameComboBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.TextBox serverUrlTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Label serverUrlLabel;
    }
}