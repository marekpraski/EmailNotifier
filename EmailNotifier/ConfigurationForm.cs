using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EmailNotifier
{
    public partial class ConfigurationForm : Form
    {
        public delegate void saveButtonClickedEventHandler(object sender, ConfigurationFormEventArgs args);

        public event saveButtonClickedEventHandler saveButtonClickedEvent;

        private readonly Dictionary<string, IEmailAccountConfiguration> accountConfigsDict = new Dictionary<string, IEmailAccountConfiguration>();

        private string[] authorisationChoices = { "yes", "no" };

        private IEmailAccountConfiguration accountConfig;

        public ConfigurationForm()
        {
            InitializeComponent();
        }

        public ConfigurationForm(Dictionary<string, IEmailAccountConfiguration> accountConfigsDict) : this()
        {
            this.accountConfigsDict = accountConfigsDict;
            populateProtocolTypesCombo();
            populateAuthorisationCombo();
            populateAccountNamesCombo();
        }


        #region Region - interakcja użytkownika

        private void AddNewAccountButton_Click(object sender, EventArgs e)
        {
            addOrUpdateAccountConfiguration();
            ClearThisForm();
        }


        private void DeleteAccountButton_Click(object sender, EventArgs e)
        {
            MyMessageBoxResults result = MyMessageBox.display("Usunąć zaznaczone konto?", MyMessageBoxType.YesNo);
            if(result == MyMessageBoxResults.Yes)
            {
                string accountName = getAccountName();
                accountConfigsDict.Remove(accountName);
                ClearThisForm();
            }
        }


        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveButtonClickedEvent != null)
                {
                    if (addOrUpdateAccountConfiguration() || accountConfigsDict.Count > 0)
                    {
                        ConfigurationFormEventArgs args = new ConfigurationFormEventArgs();
                        args.emailAccountConfigs = this.accountConfigsDict;
                        saveButtonClickedEvent(this, args);
                    }
                }

            }
            catch(ArgumentException exc)
            {
                MyMessageBox.display(exc.Message + "\r\n" + exc.InnerException, MyMessageBoxType.Error);
            }
        }

        private void CancelChangesButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void accountNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string accountName = accountNameComboBox.Text;
            accountConfigsDict.TryGetValue(accountName, out accountConfig);

            protocolTypeComboBox.Text = accountConfig.receiveServer.serverType.ToString();
            serverUrlTextBox.Text = accountConfig.receiveServer.url;
            portTextBox.Text = accountConfig.receiveServer.port.ToString();
            authorisationComboBox.SelectedIndex = accountConfig.receiveServer.useAuthorisation == true ? 0 : 1;

            userNameTextBox.Text = accountConfig.username;
            passwordTextBox.Text = accountConfig.password;
        }


        private void urlHelpLabel_MouseEnter(object sender, EventArgs e)
        {
            displayTooltip(sender, "choose from combo or type in new");

        }


        private void PortHelpLabel_MouseEnter(object sender, EventArgs e)
        {
            displayTooltip(sender, "995 - POP3 TSL authentication required \r\n110 - POP3 no TSL authentication\r\n993 - IMAP TSL authentication");
        }



        #endregion




        private bool addOrUpdateAccountConfiguration()
        {
            string accountName = getAccountName();
            if (accountConfigsDict.ContainsKey(accountName))
            {
                accountConfigsDict.TryGetValue(accountName, out accountConfig);
                getAccountConfigurationFromGUI(accountConfig);
                return true;
            }

            if (accountName != "" && accountName != null)
            {
                accountConfig = new EmailAccountConfiguration();
                getAccountConfigurationFromGUI(accountConfig);
                accountConfigsDict.Add(accountName, accountConfig);

                return true;
            }
            return false;
        }


        private void getAccountConfigurationFromGUI(IEmailAccountConfiguration emailConfig)
        {
            var receiveServer = new EmailServer();

            ServerType serverType;
            Enum.TryParse<ServerType>(protocolTypeComboBox.SelectedValue.ToString(), out serverType);
            receiveServer.serverType = serverType;
            
            receiveServer.url = serverUrlTextBox.Text;
            receiveServer.port = verifyInt(portTextBox.Text);
            receiveServer.useAuthorisation = getAuthorisation();

            emailConfig.receiveServer = receiveServer;
            emailConfig.username = userNameTextBox.Text;
            emailConfig.password = passwordTextBox.Text;
        }






        private void populateAccountNamesCombo()
        {
            accountNameComboBox.DataSource = accountConfigsDict.Keys.ToList();
        }

        private void populateAuthorisationCombo()
        {
            authorisationComboBox.DataSource = authorisationChoices;
        }


        private void populateProtocolTypesCombo()
        {
            protocolTypeComboBox.DataSource = Enum.GetValues(typeof(ServerType));
        }



        public bool getAuthorisation()
        {
            return true ? authorisationComboBox.SelectedIndex == 0 : false;
        }

        public string getAccountName()
        {
            return accountNameComboBox.Text;
        }

        public void ClearThisForm()
        {
            populateAccountNamesCombo();        //odświeżam listę w kombo
            accountNameComboBox.Text = "";
            serverUrlTextBox.Text = "";
            userNameTextBox.Text = "";
            authorisationComboBox.SelectedIndex = 0;
            passwordTextBox.Text = "";
            portTextBox.Text = "";
            accountNameComboBox.Focus();
        }


        private int verifyInt(string text)
        {
            int portNr;
            bool parsed = int.TryParse(text, out portNr);

            if (!parsed)
            {
                throw new ArgumentException("Nazwa portu musi być liczbą naturalną");
            }
            return portNr;

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
