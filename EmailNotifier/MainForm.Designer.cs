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
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.checkForEmailsButton = new System.Windows.Forms.ToolStripButton();
            this.editAccountsButton = new System.Windows.Forms.ToolStripButton();
            this.showNewEmailsButton = new System.Windows.Forms.ToolStripButton();
            this.showAllEmailsButton = new System.Windows.Forms.ToolStripButton();
            this.hideEmailsButton = new System.Windows.Forms.ToolStripButton();
            this.settingsButton = new System.Windows.Forms.ToolStripButton();
            this.infoButton = new System.Windows.Forms.ToolStripButton();
            this.checkEmailsTimer = new System.Windows.Forms.Timer(this.components);
            this.toggleNotifyiconTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "click to open";
            this.notifyIcon.BalloonTipTitle = "Email Notifier";
            this.notifyIcon.Icon = global::EmailNotifier.Properties.Resources.emailYellow;
            this.notifyIcon.Text = "Email Notifier. Click to open";
            this.notifyIcon.Click += new System.EventHandler(this.NotifyIcon_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForEmailsButton,
            this.editAccountsButton,
            this.showNewEmailsButton,
            this.showAllEmailsButton,
            this.hideEmailsButton,
            this.settingsButton,
            this.infoButton});
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
            // editAccountsButton
            // 
            this.editAccountsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editAccountsButton.Image = ((System.Drawing.Image)(resources.GetObject("editAccountsButton.Image")));
            this.editAccountsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editAccountsButton.Name = "editAccountsButton";
            this.editAccountsButton.Size = new System.Drawing.Size(23, 22);
            this.editAccountsButton.Text = "toolStripButton1";
            this.editAccountsButton.ToolTipText = "edit or add email accounts";
            this.editAccountsButton.Click += new System.EventHandler(this.EditAccountsButton_Click);
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
            // settingsButton
            // 
            this.settingsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.settingsButton.Image = ((System.Drawing.Image)(resources.GetObject("settingsButton.Image")));
            this.settingsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(23, 22);
            this.settingsButton.Text = "toolStripButton1";
            this.settingsButton.ToolTipText = "settings";
            this.settingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // infoButton
            // 
            this.infoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.infoButton.Image = ((System.Drawing.Image)(resources.GetObject("infoButton.Image")));
            this.infoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.infoButton.Name = "infoButton";
            this.infoButton.Size = new System.Drawing.Size(23, 22);
            this.infoButton.Text = "context-dependent help";
            this.infoButton.Click += new System.EventHandler(this.InfoButton_Click);
            // 
            // checkEmailsTimer
            // 
            this.checkEmailsTimer.Tick += new System.EventHandler(this.CheckEmailsTimer_Tick);
            // 
            // toggleNotifyiconTimer
            // 
            this.toggleNotifyiconTimer.Tick += new System.EventHandler(this.ToggleNotifyiconTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(214, 42);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "Email Notifier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton checkForEmailsButton;
        private System.Windows.Forms.ToolStripButton editAccountsButton;
        private System.Windows.Forms.ToolStripButton showNewEmailsButton;
        private System.Windows.Forms.ToolStripButton hideEmailsButton;
        private System.Windows.Forms.ToolStripButton showAllEmailsButton;
        private System.Windows.Forms.Timer checkEmailsTimer;
        private System.Windows.Forms.Timer toggleNotifyiconTimer;
        private System.Windows.Forms.ToolStripButton settingsButton;
        private System.Windows.Forms.ToolStripButton infoButton;
    }
}

