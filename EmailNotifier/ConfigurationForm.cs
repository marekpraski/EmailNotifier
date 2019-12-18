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

        private readonly Dictionary<string, IEmailAccountConfiguration> emailAccountConfigurations = new Dictionary<string, IEmailAccountConfiguration>();

        private AccountConfigurationControl accountConfigurationControl;

        public ConfigurationForm()
        {
            InitializeComponent();
        }

        public ConfigurationForm(Dictionary<string, IEmailAccountConfiguration> emailAccountConfigs)
        {
            this.emailAccountConfigurations = emailAccountConfigs;
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
                emailAccountConfigurations.Remove(accountName);
                accountConfigurationControl.Clear();
            }
        }


        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveButtonClickedEvent != null)
                {
                    if (addOrUpdateAccountConfiguration() || emailAccountConfigurations.Count > 0)
                    {
                        ConfigurationFormEventArgs args = new ConfigurationFormEventArgs();
                        args.emailAccountConfigs = this.emailAccountConfigurations;
                        saveButtonClickedEvent(this, args);
                    }
                }
                this.Close();

            }
            catch (InvalidEmailAccountException ex)
            {
                
                MyMessageBox.display(ex.message + "\r\n" + ex.Source, MessageBoxType.Error);
            }
            catch(ArgumentException exc)
            {
                MyMessageBox.display(exc.Message, MessageBoxType.Error);
            }
        }

        private void CancelChangesButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion



        private void setupAccountConfigurationControl()
        {
            this.accountConfigurationControl = new AccountConfigurationControl(emailAccountConfigurations);
            this.accountConfigurationControl.Location = new System.Drawing.Point(12, 28);
            this.accountConfigurationControl.Name = "accountConfiguration1";
            this.accountConfigurationControl.Size = new System.Drawing.Size(369, 149);
            this.accountConfigurationControl.TabIndex = 0;

            this.Controls.Add(this.accountConfigurationControl);
        }



        private bool addOrUpdateAccountConfiguration()
        {
            IEmailAccountConfiguration accountConfig;
            string accountName = accountConfigurationControl.getAccountName();
            if (emailAccountConfigurations.ContainsKey(accountName))
            {
                emailAccountConfigurations.TryGetValue(accountName, out accountConfig);
                setAccountParameters(accountConfig);
                return true;
            }

            if (accountName != "" && accountName != null)
            {
                accountConfig = new EmailAccountConfiguration();
                setAccountParameters(accountConfig);
                emailAccountConfigurations.Add(accountName, accountConfig);

                return true;
            }
            return false;
        }

        private void setAccountParameters(IEmailAccountConfiguration emailConfig)
        {
            emailConfig.ReceivePassword = accountConfigurationControl.getPassword();
            emailConfig.ReceivePort = accountConfigurationControl.getPort();
            emailConfig.ReceiveServer = accountConfigurationControl.getServerName();
            emailConfig.ReceiveUsername = accountConfigurationControl.getUserName();
            emailConfig.ReceiveUseAuthorisation = accountConfigurationControl.getAuthorisation();
        }

    }
}
