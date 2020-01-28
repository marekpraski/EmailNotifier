using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace EmailNotifier
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();


        }

 

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            if (await getTextboxValue())
                textBox1.Text = "it works";
            timer1.Start();
            textBox1.Refresh();

        }

        private void populateTextbox(string txt)
        {
            textBox1.Text = txt;
            textBox2.Text = txt;
            textBox1.Refresh();
            textBox2.Refresh();
        }

        private async Task<bool> getTextboxValue()
        {
            await Task.Factory.StartNew(() =>
            {
                int j = 0;
                for (int i = 0; i < 1000000000; i++)
                {
                    j++;
                }
            });
            return true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            populateTextbox(DateTime.Now.ToString());
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = "aaa";
            Form2 fm2 = new Form2();
            fm2.ShowDialog();
        }

        //private Task<string> generateText()
        //{
        //    string txt = "it works";
        //    return ;
        //}
    }
}
