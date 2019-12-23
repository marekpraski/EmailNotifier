using System;
using System.Windows.Forms;

namespace EmailNotifier
{
    public partial class SettingsForm : Form
    {
        public delegate void saveSettingsEventHandler(object sender, SettingsArgs args);
        public event saveSettingsEventHandler saveSettingsEvent;

        public int checkEmailTimespan { get; set; } = 15;            //minut 
        public int notificationFrequency { get; set; } = 30;      //sekund
        public int numberOfEmailsKept { get; set; } = 50;            //przeczytanych maili
        public SettingsForm()
        {
            InitializeComponent();
        }

        public SettingsForm(int checkEmailTimespan, int showNotificationTimespan, int numberOfEmailsKept) : this()
        {
            this.checkEmailTimespan = checkEmailTimespan;
            this.notificationFrequency = showNotificationTimespan;
            this.numberOfEmailsKept = numberOfEmailsKept;

            checkEmailTimerTextbox.Text = checkEmailTimespan.ToString();
            notificationTimerTextbox.Text = showNotificationTimespan.ToString();
            numberOfEmailsKeptTextbox.Text = numberOfEmailsKept.ToString();
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
    }
}
