namespace EmailNotifier
{
    partial class AccountConfigurationControl
    {
        /// <summary> 
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod wygenerowany przez Projektanta składników

        /// <summary> 
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować 
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.serverNameLabel = new System.Windows.Forms.Label();
            this.portLabel = new System.Windows.Forms.Label();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.serverNameTextBox = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.accountNameComboBox = new System.Windows.Forms.ComboBox();
            this.accountNameLabel = new System.Windows.Forms.Label();
            this.authorisationLabel = new System.Windows.Forms.Label();
            this.authorisationComboBox = new System.Windows.Forms.ComboBox();
            this.accountNameHelpLabel = new System.Windows.Forms.Label();
            this.portHelpLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serverNameLabel
            // 
            this.serverNameLabel.AutoSize = true;
            this.serverNameLabel.Location = new System.Drawing.Point(5, 31);
            this.serverNameLabel.Name = "serverNameLabel";
            this.serverNameLabel.Size = new System.Drawing.Size(67, 13);
            this.serverNameLabel.TabIndex = 9;
            this.serverNameLabel.Text = "POP3 server";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(5, 55);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(25, 13);
            this.portLabel.TabIndex = 8;
            this.portLabel.Text = "port";
            // 
            // userNameLabel
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Location = new System.Drawing.Point(5, 106);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(29, 13);
            this.userNameLabel.TabIndex = 7;
            this.userNameLabel.Text = "login";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(4, 129);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(52, 13);
            this.passwordLabel.TabIndex = 6;
            this.passwordLabel.Text = "password";
            // 
            // serverNameTextBox
            // 
            this.serverNameTextBox.Location = new System.Drawing.Point(125, 28);
            this.serverNameTextBox.Name = "serverNameTextBox";
            this.serverNameTextBox.Size = new System.Drawing.Size(240, 20);
            this.serverNameTextBox.TabIndex = 1;
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(125, 52);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(100, 20);
            this.portTextBox.TabIndex = 2;
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(125, 103);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(240, 20);
            this.userNameTextBox.TabIndex = 4;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(125, 126);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(240, 20);
            this.passwordTextBox.TabIndex = 5;
            // 
            // accountNameComboBox
            // 
            this.accountNameComboBox.FormattingEnabled = true;
            this.accountNameComboBox.Location = new System.Drawing.Point(125, 1);
            this.accountNameComboBox.Name = "accountNameComboBox";
            this.accountNameComboBox.Size = new System.Drawing.Size(240, 21);
            this.accountNameComboBox.TabIndex = 0;
            this.accountNameComboBox.SelectedIndexChanged += new System.EventHandler(this.AccountNameComboBox_SelectedIndexChanged);
            // 
            // accountNameLabel
            // 
            this.accountNameLabel.AutoSize = true;
            this.accountNameLabel.Location = new System.Drawing.Point(4, 8);
            this.accountNameLabel.Name = "accountNameLabel";
            this.accountNameLabel.Size = new System.Drawing.Size(102, 13);
            this.accountNameLabel.TabIndex = 5;
            this.accountNameLabel.Text = "email account name";
            // 
            // authorisationLabel
            // 
            this.authorisationLabel.AutoSize = true;
            this.authorisationLabel.Location = new System.Drawing.Point(5, 81);
            this.authorisationLabel.Name = "authorisationLabel";
            this.authorisationLabel.Size = new System.Drawing.Size(97, 13);
            this.authorisationLabel.TabIndex = 4;
            this.authorisationLabel.Text = "TSL authentication";
            // 
            // authorisationComboBox
            // 
            this.authorisationComboBox.FormattingEnabled = true;
            this.authorisationComboBox.Location = new System.Drawing.Point(125, 78);
            this.authorisationComboBox.Name = "authorisationComboBox";
            this.authorisationComboBox.Size = new System.Drawing.Size(100, 21);
            this.authorisationComboBox.TabIndex = 3;
            // 
            // accountNameHelpLabel
            // 
            this.accountNameHelpLabel.AutoSize = true;
            this.accountNameHelpLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.accountNameHelpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.accountNameHelpLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.accountNameHelpLabel.Location = new System.Drawing.Point(106, 5);
            this.accountNameHelpLabel.Name = "accountNameHelpLabel";
            this.accountNameHelpLabel.Size = new System.Drawing.Size(15, 15);
            this.accountNameHelpLabel.TabIndex = 10;
            this.accountNameHelpLabel.Text = "?";
            this.accountNameHelpLabel.MouseEnter += new System.EventHandler(this.AccountNameHelpLabel_MouseEnter);
            // 
            // portHelpLabel
            // 
            this.portHelpLabel.AutoSize = true;
            this.portHelpLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.portHelpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.portHelpLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.portHelpLabel.Location = new System.Drawing.Point(104, 55);
            this.portHelpLabel.Name = "portHelpLabel";
            this.portHelpLabel.Size = new System.Drawing.Size(15, 15);
            this.portHelpLabel.TabIndex = 11;
            this.portHelpLabel.Text = "?";
            this.portHelpLabel.MouseEnter += new System.EventHandler(this.PortHelpLabel_MouseEnter);
            // 
            // AccountConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.portHelpLabel);
            this.Controls.Add(this.accountNameHelpLabel);
            this.Controls.Add(this.authorisationComboBox);
            this.Controls.Add(this.authorisationLabel);
            this.Controls.Add(this.accountNameLabel);
            this.Controls.Add(this.accountNameComboBox);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.userNameTextBox);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.serverNameTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.userNameLabel);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.serverNameLabel);
            this.Name = "AccountConfigurationControl";
            this.Size = new System.Drawing.Size(369, 149);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serverNameLabel;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox serverNameTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.ComboBox accountNameComboBox;
        private System.Windows.Forms.Label accountNameLabel;
        private System.Windows.Forms.Label authorisationLabel;
        private System.Windows.Forms.ComboBox authorisationComboBox;
        private System.Windows.Forms.Label accountNameHelpLabel;
        private System.Windows.Forms.Label portHelpLabel;
    }
}
