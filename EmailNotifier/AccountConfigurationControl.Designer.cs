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
            this.SuspendLayout();
            // 
            // serverNameLabel
            // 
            this.serverNameLabel.AutoSize = true;
            this.serverNameLabel.Location = new System.Drawing.Point(5, 31);
            this.serverNameLabel.Name = "serverNameLabel";
            this.serverNameLabel.Size = new System.Drawing.Size(63, 13);
            this.serverNameLabel.TabIndex = 0;
            this.serverNameLabel.Text = "serwer POP";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(5, 55);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(25, 13);
            this.portLabel.TabIndex = 1;
            this.portLabel.Text = "port";
            // 
            // userNameLabel
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Location = new System.Drawing.Point(5, 106);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(100, 13);
            this.userNameLabel.TabIndex = 2;
            this.userNameLabel.Text = "nazwa użytkownika";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(4, 129);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(34, 13);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "hasło";
            // 
            // serverNameTextBox
            // 
            this.serverNameTextBox.Location = new System.Drawing.Point(111, 28);
            this.serverNameTextBox.Name = "serverNameTextBox";
            this.serverNameTextBox.Size = new System.Drawing.Size(254, 20);
            this.serverNameTextBox.TabIndex = 4;
            // 
            // portTextBox
            // 
            this.portTextBox.Location = new System.Drawing.Point(111, 52);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(100, 20);
            this.portTextBox.TabIndex = 5;
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(111, 103);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(254, 20);
            this.userNameTextBox.TabIndex = 6;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(111, 126);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(254, 20);
            this.passwordTextBox.TabIndex = 7;
            // 
            // accountNameComboBox
            // 
            this.accountNameComboBox.FormattingEnabled = true;
            this.accountNameComboBox.Location = new System.Drawing.Point(111, 1);
            this.accountNameComboBox.Name = "accountNameComboBox";
            this.accountNameComboBox.Size = new System.Drawing.Size(254, 21);
            this.accountNameComboBox.TabIndex = 8;
            // 
            // accountNameLabel
            // 
            this.accountNameLabel.AutoSize = true;
            this.accountNameLabel.Location = new System.Drawing.Point(4, 8);
            this.accountNameLabel.Name = "accountNameLabel";
            this.accountNameLabel.Size = new System.Drawing.Size(68, 13);
            this.accountNameLabel.TabIndex = 9;
            this.accountNameLabel.Text = "nazwa konta";
            // 
            // authorisationLabel
            // 
            this.authorisationLabel.AutoSize = true;
            this.authorisationLabel.Location = new System.Drawing.Point(5, 81);
            this.authorisationLabel.Name = "authorisationLabel";
            this.authorisationLabel.Size = new System.Drawing.Size(102, 13);
            this.authorisationLabel.TabIndex = 10;
            this.authorisationLabel.Text = "uwierzytenienie TSL";
            // 
            // authorisationComboBox
            // 
            this.authorisationComboBox.FormattingEnabled = true;
            this.authorisationComboBox.Location = new System.Drawing.Point(111, 78);
            this.authorisationComboBox.Name = "authorisationComboBox";
            this.authorisationComboBox.Size = new System.Drawing.Size(100, 21);
            this.authorisationComboBox.TabIndex = 11;
            // 
            // AccountConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
    }
}
