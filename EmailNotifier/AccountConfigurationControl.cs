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
        private readonly Dictionary<string, EmailAccount> emailAccounts = new Dictionary<string, EmailAccount>();
        public AccountConfigurationControl(Dictionary<string, EmailAccount> emailAccounts)
        {
            this.emailAccounts = emailAccounts;
            InitializeComponent();
            populateAuthorisationCombo();
            populateAccountNamesCombo();
        }


        #region Region - interakcja użytkownika
        private void AccountNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string accountName = accountNameComboBox.Text;
            EmailAccount account;
            emailAccounts.TryGetValue(accountName, out account);

            serverNameTextBox.Text = account.configuration.ReceiveServer;
            portTextBox.Text = account.configuration.ReceivePort.ToString();
            userNameTextBox.Text = account.configuration.ReceiveUsername;
            passwordTextBox.Text = account.configuration.ReceivePassword;
            authorisationComboBox.SelectedIndex = account.configuration.ReceiveUseAuthorisation == true ? 0 : 1;
        }

        #endregion


        private void populateAccountNamesCombo()
        {
            accountNameComboBox.DataSource = emailAccounts.Keys.ToList();
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
