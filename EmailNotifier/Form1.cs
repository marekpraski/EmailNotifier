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
            initialSetup();

        }

        private void initialSetup()
        {
            populateListview();
            initializeEvent();
        }

        private void initializeEvent()
        {
            this.listView1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ListView1_ItemCheck);
            //this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.ListView1_ItemChecked);
        }

        private void populateListview()
        {
            //this.listView1.ItemChecked -= ListView1_ItemChecked;
            ImageList image = new ImageList();
            image.ImageSize = new Size(16, 16);
            image.Images.Add(Properties.Resources.trash_16);
            listView1.SmallImageList = image;

            for (int i = 0; i < 10; i++)
            {
                string[] s = new string[] { "Value " + i, "sub 1 " + i, "sub 2" + i };
                ListViewItem item = new ListViewItem(s);
                item.ToolTipText = i.ToString();
                listView1.Items.Add(item);
            }
            //this.listView1.ItemChecked += ListView1_ItemChecked;

        }

        private void ListView1_MouseClick(object sender, MouseEventArgs e)
        {
            

            //var info = listView.HitTest(e.X, e.Y);
            //var row = info.Item.Index;
            //var col = info.Item.SubItems.IndexOf(info.SubItem);
            //var value = info.Item.SubItems[col].Text;
            //MessageBox.Show(string.Format("R{0}:C{1} val '{2}'", row, col, value));




            
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem selected = listView.SelectedItems[0];
                selected.ImageIndex = 0;

            }
        }

        private void ListView1_MouseDown(object sender, MouseEventArgs e)
        {
            //ListView listView = sender as ListView;
            //try
            //{
            //var info = listView.HitTest(e.X, e.Y);

            //    var row = info.Item.Index;
            //    var col = info.Item.SubItems.IndexOf(info.SubItem);
            //    var value = info.Item.SubItems[col].Text;
            //    MessageBox.Show(string.Format("R{0}:C{1} val '{2}'", row, col, value));

            //}
            //catch (NullReferenceException)
            //{
            //}


            // Hittestinfo of the clicked ListView location
            //ListViewHitTestInfo listViewHitTestInfo = listView.HitTest(e.X, e.Y);

            //// Index of the clicked ListView column
            //int columnIndex = listViewHitTestInfo.Item.SubItems.IndexOf(listViewHitTestInfo.SubItem);
            //MyMessageBox.display(columnIndex.ToString());

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

        private void ListView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void ListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ListView.CheckedListViewItemCollection checkedItems = listView1.CheckedItems;
                foreach (ListViewItem item in checkedItems)
                {
                    item.ImageIndex = 0;
                }
            }
        }

        private void ListView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            MyMessageBox.display("aaa");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
        }

        private void ListView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MyMessageBox.display("aaa");
        }
    }
}
