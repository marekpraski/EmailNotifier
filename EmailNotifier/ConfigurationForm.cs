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
        public delegate void saveButtonClickedEventHandler(object sender, ConfigurationFormEventArgs args);
        public event saveButtonClickedEventHandler saveButtonClickedEvent;

        private readonly Dictionary<string, EmailAccount> emailAccounts = new Dictionary<string, EmailAccount>();
        private readonly List<EmailAccount> emailAccountsList = new List<EmailAccount>();

        private AccountConfigurationControl accountConfigurationControl;

        public ConfigurationForm()
        {
            InitializeComponent();
        }

        public ConfigurationForm(Dictionary<string, EmailAccount> emailAccounts)
        {
            this.emailAccounts = emailAccounts;
            InitializeComponent();
            setupAccountConfigurationControl();
        }


        #region Region - interakcja użytkownika

        private void AddNewAccountButton_Click(object sender, EventArgs e)
        {
            addOrUpdateAccountConfiguration();
            accountConfigurationControl.Clear();
        }


        private void DeleteAccountButton_Click(object sender, EventArgs e)
        {
            MyMessageBoxResults result = MyMessageBox.display("Usunąć zaznaczone konto?", MessageBoxType.YesNo);
            if(result == MyMessageBoxResults.Yes)
            {
                string accountName = accountConfigurationControl.getAccountName();
                emailAccounts.Remove(accountName);
                accountConfigurationControl.Clear();
            }
        }


        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveButtonClickedEvent != null)
                {
                    if (addOrUpdateAccountConfiguration() || emailAccounts.Count > 0)
                    {
                        ConfigurationFormEventArgs args = new ConfigurationFormEventArgs();
                        args.emailAccounts = this.emailAccounts;
                        saveButtonClickedEvent(this, args);
                    }
                }
                this.Close();

            }
            catch (InvalidEmailAccountException ex)
            {
                
                MyMessageBox.display(ex.message + "\r\n" + ex.Source, MessageBoxType.Error);
            }
        }

        #endregion



        private void setupAccountConfigurationControl()
        {
            this.accountConfigurationControl = new AccountConfigurationControl(emailAccounts);
            this.accountConfigurationControl.Location = new System.Drawing.Point(12, 28);
            this.accountConfigurationControl.Name = "accountConfiguration1";
            this.accountConfigurationControl.Size = new System.Drawing.Size(369, 149);
            this.accountConfigurationControl.TabIndex = 0;

            this.Controls.Add(this.accountConfigurationControl);
        }



        private bool addOrUpdateAccountConfiguration()
        {
            EmailAccount account;
            string accountName = accountConfigurationControl.getAccountName();
            if (emailAccounts.ContainsKey(accountName))
            {
                emailAccounts.TryGetValue(accountName, out account);
                IEmailAccountConfiguration emailConfig = account.configuration;
                setAccountParameters(account, emailConfig);
                return true;
            }

            if (accountName != "" && accountName != null)
            {
                account = new EmailAccount();
                account.name = accountName;
                IEmailAccountConfiguration emailConfig = new EmailAccountConfiguration();
                setAccountParameters(account, emailConfig);

                emailAccounts.Add(accountName, account);

                return true;
            }
            return false;
        }

        private void setAccountParameters(EmailAccount account, IEmailAccountConfiguration emailConfig)
        {
            emailConfig.ReceivePassword = accountConfigurationControl.getPassword();
            emailConfig.ReceivePort = accountConfigurationControl.getPort();
            emailConfig.ReceiveServer = accountConfigurationControl.getServerName();
            emailConfig.ReceiveUsername = accountConfigurationControl.getUserName();
            emailConfig.ReceiveUseAuthorisation = accountConfigurationControl.getAuthorisation();
            account.configuration = emailConfig;
        }



    }
}
