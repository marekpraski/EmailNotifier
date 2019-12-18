namespace EmailNotifier
{
    partial class MainForm
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

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.checkForEmailsButton = new System.Windows.Forms.ToolStripButton();
            this.editButton = new System.Windows.Forms.ToolStripButton();
            this.showNewEmailsButton = new System.Windows.Forms.ToolStripButton();
            this.showAllEmailsButton = new System.Windows.Forms.ToolStripButton();
            this.hideEmailsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "nowa poczta";
            this.notifyIcon1.BalloonTipTitle = "Email Notifier";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Email Notifier";
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.NotifyIcon1_DoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForEmailsButton,
            this.editButton,
            this.showNewEmailsButton,
            this.showAllEmailsButton,
            this.hideEmailsButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(214, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // checkForEmailsButton
            // 
            this.checkForEmailsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.checkForEmailsButton.Image = ((System.Drawing.Image)(resources.GetObject("checkForEmailsButton.Image")));
            this.checkForEmailsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.checkForEmailsButton.Name = "checkForEmailsButton";
            this.checkForEmailsButton.Size = new System.Drawing.Size(23, 22);
            this.checkForEmailsButton.Text = "toolStripButton1";
            this.checkForEmailsButton.ToolTipText = "check for new emails on server";
            this.checkForEmailsButton.Click += new System.EventHandler(this.checkForEmailsButton_Click);
            // 
            // editButton
            // 
            this.editButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editButton.Image = ((System.Drawing.Image)(resources.GetObject("editButton.Image")));
            this.editButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(23, 22);
            this.editButton.Text = "toolStripButton1";
            this.editButton.ToolTipText = "edit or add email accounts";
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // showNewEmailsButton
            // 
            this.showNewEmailsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showNewEmailsButton.Image = ((System.Drawing.Image)(resources.GetObject("showNewEmailsButton.Image")));
            this.showNewEmailsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showNewEmailsButton.Name = "showNewEmailsButton";
            this.showNewEmailsButton.Size = new System.Drawing.Size(23, 22);
            this.showNewEmailsButton.Text = "toolStripButton1";
            this.showNewEmailsButton.ToolTipText = "show new emails";
            this.showNewEmailsButton.Click += new System.EventHandler(this.ShowNewEmailsButton_Click);
            // 
            // showAllEmailsButton
            // 
            this.showAllEmailsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showAllEmailsButton.Image = ((System.Drawing.Image)(resources.GetObject("showAllEmailsButton.Image")));
            this.showAllEmailsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showAllEmailsButton.Name = "showAllEmailsButton";
            this.showAllEmailsButton.Size = new System.Drawing.Size(23, 22);
            this.showAllEmailsButton.Text = "show all emails";
            this.showAllEmailsButton.Click += new System.EventHandler(this.ShowAllEmailsButton_Click);
            // 
            // hideEmailsButton
            // 
            this.hideEmailsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.hideEmailsButton.Enabled = false;
            this.hideEmailsButton.Image = ((System.Drawing.Image)(resources.GetObject("hideEmailsButton.Image")));
            this.hideEmailsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.hideEmailsButton.Name = "hideEmailsButton";
            this.hideEmailsButton.Size = new System.Drawing.Size(23, 22);
            this.hideEmailsButton.Text = "hide emails";
            this.hideEmailsButton.Click += new System.EventHandler(this.HideEmailsButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(214, 42);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "Email Notifier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton checkForEmailsButton;
        private System.Windows.Forms.ToolStripButton editButton;
        private System.Windows.Forms.ToolStripButton showNewEmailsButton;
        private System.Windows.Forms.ToolStripButton hideEmailsButton;
        private System.Windows.Forms.ToolStripButton showAllEmailsButton;
    }
}

