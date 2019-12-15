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
    public partial class EmailDisplayControl : UserControl
    {
        public string EmailSubject
        {
            set { subjectTextBox.Text = value; }
        }

        public string EmailSender
        {
            set { senderTextBox.Text = value; }
        }

        public EmailDisplayControl()
        {
            InitializeComponent();            
        }

        private void showTooltip()
        {
            // Create the ToolTip and associate with the Form container.
            ToolTip toolTip1 = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 200;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(subjectTextBox, subjectTextBox.Text);
        }

        private void SubjectTextBox_MouseEnter(object sender, EventArgs e)
        {
            showTooltip();
        }
    }
}
