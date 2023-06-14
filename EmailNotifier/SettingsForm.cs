using System;
using System.Windows.Forms;

namespace EmailNotifier
{
    public partial class SettingsForm : Form
    {
        public event EventHandler<SettingsArgs> saveSettingsEvent;

        private int checkEmailTimespan { get; set; }           //minut 
        private int notificationFrequency { get; set; }      //sekund
        private int numberOfEmailsKept { get; set; }           //przeczytanych maili
        private int emailNumberAtSetup { get; set; }        //emaili wczytywanych na starcie po utworzeniu konta
        public SettingsForm()
        {
            InitializeComponent();
            initialSetup();
        }

        public void initialSetup()
        {
            this.checkEmailTimespan = ProgramSettings.checkEmailTimespan;
            this.notificationFrequency = ProgramSettings.showNotificationTimespan;
            this.numberOfEmailsKept = ProgramSettings.numberOfEmailsKept;
            this.emailNumberAtSetup = ProgramSettings.numberOfEmailsAtSetup;

            checkEmailTimerTextbox.Text = checkEmailTimespan.ToString();
            notificationTimerTextbox.Text = notificationFrequency.ToString();
            numberOfEmailsKeptTextbox.Text = numberOfEmailsKept.ToString();
            numberOfEmailsAtSetupTextBox.Text = emailNumberAtSetup.ToString();
            cbEnableLog.Checked = ProgramSettings.enableLogFile;
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (validateUserInput())
            {
                if (saveSettingsEvent != null)
                {
                    SettingsArgs args = new SettingsArgs();
                    args.emailCheckTimespan = checkEmailTimespan;
                    args.notificationBubbleTimespan = notificationFrequency;
                    args.emailNumberKept = numberOfEmailsKept;
                    args.emailNumberAtSetup = emailNumberAtSetup;
                    args.enableLogFile = cbEnableLog.Checked;
                    saveSettingsEvent(this, args);
                    this.Close();
                }
            }
        }

        private bool validateUserInput()
        {
            try
            {
                checkEmailTimespan = validate(checkEmailTimerTextbox.Text);
                notificationFrequency = validate(notificationTimerTextbox.Text);
                numberOfEmailsKept = validate(numberOfEmailsKeptTextbox.Text);
                emailNumberAtSetup = validate(numberOfEmailsAtSetupTextBox.Text);
            }
            catch (ArgumentException ex)
            {
                MyMessageBox.display(ex.Message, MyMessageBoxType.Error);
            }
            return true;
        }

        private int validate(string textToParse)
        {
            int value;

            if (!int.TryParse(textToParse, out value))
            {
                throw new ArgumentException("only whole numbers are allowed");
            }
            return value;
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

        private void Label2_MouseEnter(object sender, EventArgs e)
        {
            displayTooltip(sender, "5 minutes or more. If less is entered, 5 will be set");
        }

        private void Label6_MouseEnter(object sender, EventArgs e)
        {
            displayTooltip(sender, "20 seconds or more. If less is entered, 20 will be set");
        }

        private void HelpLabel_MouseEnter(object sender, EventArgs e)
        {
            displayTooltip(sender, "if this checkbox is left unchecked, checked new emails are only marked as read");
        }

        private void HelpLabel1_MouseEnter(object sender, EventArgs e)
        {
            displayTooltip(sender, "leave empty to read all emails from server");
        }
    }
}
