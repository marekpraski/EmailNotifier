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
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.deleteAccountButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewAccountButton,
            this.deleteAccountButton,
            this.saveButton});
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
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 190);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ConfigurationForm";
            this.Text = "Konfiguracja";
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
    }
}