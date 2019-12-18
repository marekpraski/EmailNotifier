using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmailNotifier
{
    public partial class AccountConfigurationControl : UserControl
    {
        private string[] authorisationChoices = { "tak", "nie" };
        private readonly Dictionary<string, IEmailAccountConfiguration> emailAccountConfigs = new Dictionary<string, IEmailAccountConfiguration>();
        public AccountConfigurationControl(Dictionary<string, IEmailAccountConfiguration> emailAccountConfigs)
        {
            this.emailAccountConfigs = emailAccountConfigs;
            InitializeComponent();
            populateAuthorisationCombo();
            populateAccountNamesCombo();
        }


        #region Region - interakcja użytkownika
        private void AccountNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string accountName = accountNameComboBox.Text;
            IEmailAccountConfiguration accountConfig;
            emailAccountConfigs.TryGetValue(accountName, out accountConfig);

            serverNameTextBox.Text = accountConfig.ReceiveServer;
            portTextBox.Text = accountConfig.ReceivePort.ToString();
            userNameTextBox.Text = accountConfig.ReceiveUsername;
            passwordTextBox.Text = accountConfig.ReceivePassword;
            authorisationComboBox.SelectedIndex = accountConfig.ReceiveUseAuthorisation == true ? 0 : 1;
        }

        #endregion


        private void populateAccountNamesCombo()
        {
            accountNameComboBox.DataSource = emailAccountConfigs.Keys.ToList();
        }

        private void populateAuthorisationCombo()
        {
            authorisationComboBox.DataSource = authorisationChoices;
        }

        public int getPort()
        {
            return int.Parse(portTextBox.Text);
        }

        public string getPassword()
        {
            return passwordTextBox.Text;
        }

        public string getUserName()
        {
            return userNameTextBox.Text;
        }

        public string getServerName()
        {
            return serverNameTextBox.Text;
        }

        public bool getAuthorisation()
        {
            return true ? authorisationComboBox.SelectedIndex == 0 : false;
        }

        public string getAccountName()
        {
            return accountNameComboBox.Text;
        }

        public void Clear()
        {
            populateAccountNamesCombo();        //odświeżam listę w kombo
            accountNameComboBox.Text = "";
            serverNameTextBox.Text = "";
            userNameTextBox.Text = "";
            passwordTextBox.Text = "";
            portTextBox.Text = "";
            accountNameComboBox.Focus();
        }


    }
}
