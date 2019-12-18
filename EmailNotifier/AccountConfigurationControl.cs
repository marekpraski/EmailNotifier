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
        private string[] authorisationChoices = { "yes", "no" };
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



        private void AccountNameHelpLabel_MouseEnter(object sender, EventArgs e)
        {
            displayTooltip(sender, "choose from combo or type in");

        }

        private void PortHelpLabel_MouseEnter(object sender, EventArgs e)
        {
            displayTooltip(sender, "995 - POP3 TSL authentication required \r\n110 - POP3 no TSL authentication");
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
            int portNr;
            bool parsed = int.TryParse(portTextBox.Text, out portNr);

            if(!parsed)
            {
                throw new ArgumentException("Nazwa portu musi być liczbą naturalną");
            }
            return portNr;
            
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
            if(serverNameTextBox.Text == "")
            {
                throw new ArgumentException("Pole nazwy serwera nie może być puste");
            }
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
            authorisationComboBox.SelectedIndex = 0;
            passwordTextBox.Text = "";
            portTextBox.Text = "";
            accountNameComboBox.Focus();
        }



        private void displayTooltip(object sender, string message)
        {
            Control control = sender as Control;
            // Create the ToolTip and associate with the Form container.
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 100;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(control, message);
        }

    }
}
