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
        public AccountConfigurationControl()
        {
            InitializeComponent();
            populateAuthorisationCombo();
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
    }
}
