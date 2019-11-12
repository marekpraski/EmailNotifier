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
            this.accountConfiguration1 = new EmailNotifier.AccountConfigurationControl();
            this.acceptButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // accountConfiguration1
            // 
            this.accountConfiguration1.Location = new System.Drawing.Point(13, 13);
            this.accountConfiguration1.Name = "accountConfiguration1";
            this.accountConfiguration1.Size = new System.Drawing.Size(369, 149);
            this.accountConfiguration1.TabIndex = 0;
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(151, 168);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(112, 23);
            this.acceptButton.TabIndex = 1;
            this.acceptButton.Text = "zapisz ustawienia";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 204);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.accountConfiguration1);
            this.Name = "ConfigurationForm";
            this.Text = "Konfiguracja";
            this.ResumeLayout(false);

        }

        #endregion

        private AccountConfigurationControl accountConfiguration1;
        private System.Windows.Forms.Button acceptButton;
    }
}