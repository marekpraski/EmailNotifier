using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmailNotifier
{
    public partial class ConfigurationForm : Form
    {
        public delegate void acceptButtonClickedEventHandler(object sender, ConfigurationFormEventArgs args);
        public event acceptButtonClickedEventHandler acceptButtonClickedEvent;
        EmailAccount account;

        public ConfigurationForm()
        {
            InitializeComponent();
        }

        private void getConfiguration()
        {
            account = new EmailAccount();
            account.name = accountConfiguration1.getAccountName();

            EmailConfiguration emailConfig = new EmailConfiguration();
            emailConfig.PopPassword = accountConfiguration1.getPassword();
            emailConfig.PopPort = accountConfiguration1.getPort();
            emailConfig.PopServer = accountConfiguration1.getServerName();
            emailConfig.PopUsername = accountConfiguration1.getUserName();
            emailConfig.PopUseTSL = accountConfiguration1.getAuthorisation();

            account.configuration = emailConfig; 
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if(acceptButtonClickedEvent != null)
            {
                getConfiguration();
                ConfigurationFormEventArgs args = new ConfigurationFormEventArgs();
                args.emailAccount = this.account;
                acceptButtonClickedEvent(this, args);
            }
            this.Close();
        }
    }
}
