using MailKit.Net.Pop3;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Configuration;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;

namespace EmailNotifier
{
    public partial class MainForm : Form
    {

        private Dictionary<string, EmailAccount> mailBoxes = new Dictionary<string, EmailAccount>();

      

        public MainForm()
        {
            InitializeComponent();
            initialSetup();
        }



        private void initialSetup()
        {
            setupMailboxes();
        }

        /// <summary>
        /// jeżeli istnieje plik z danymi, czyta konfigurację kont z pliku
        /// w przeciwnym wypadku otwiera okno konfiguracji i tworzy taki plik
        /// </summary>
        private void setupMailboxes()
        {
            string emailDataFile = ProgramSettings.fileSavePath + ProgramSettings.emailDataFileName;
            FileManipulator fm = new FileManipulator();
            if (fm.assertFileExists(emailDataFile))
            {                
                readDataFromFile(emailDataFile);                              
            }
            else
            //jeżeli brak pliku otwieram okno konfiguracji kont pocztowych oraz czytam po kilka maili z każdego i zapisuję wszystko do pliku
            //to będzie punkt startowy do dalszego działania
            {
                updateMailboxes(emailDataFile);
            }
        }

        private void updateMailboxes(string emailDataFile)
        {
            ConfigurationForm configForm = new ConfigurationForm(mailBoxes);
            configForm.saveButtonClickedEvent += configurationFormSaveButtonClicked;
            configForm.ShowDialog();
            configForm.saveButtonClickedEvent -= configurationFormSaveButtonClicked;    //likwiduję zdarzenie żeby zapobiec wyciekowi pamięci

            if (this.mailBoxes.Count > 0)
            {
                foreach (string mailboxName in this.mailBoxes.Keys)
                {
                    EmailAccount mailbox;
                    mailBoxes.TryGetValue(mailboxName, out mailbox);

                    if (mailbox.emailsList.Count == 0)
                    {
                        getMessages(mailbox);       //startową liczbę maili wczytuję tylko dla nowych kont
                    }
                    saveDataToFile(emailDataFile);                    
                }
            }
        }


        #region Region - interakcja użytkownika


        private void Form1_Resize(object sender, EventArgs e)
        {
            //if the form is minimized  
            //hide it from the task bar  
            //and show the system tray icon (represented by the NotifyIcon control)  
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
        }

        private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }



        private void configurationFormSaveButtonClicked(object sender, ConfigurationFormEventArgs args)
        {
           // ZROBIĆ: OBECNIE OPERACJE SĄ ROBIONE BEZPOŚREDNIO NA SŁOWNIKU, czyli "anulowanie zmian" nie istnieje
           //przesyłać kopię słownika i rzeczywiście umożliwić zapisywanie/anulowanie zmian

            //foreach (EmailAccount account in args.emailAccounts)
            //{
            //    if (mailBoxes.ContainsKey(account.name))
            //    {
            //        throw new InvalidEmailAccountException("konto " + account.name + " już istnieje");
            //    }
            //    mailBoxes.Add(account.name, account);
            //}
        }


        //
        //pasek narzędziowy
        //

        private void EditButton_Click(object sender, EventArgs e)
        {
            updateMailboxes(ProgramSettings.fileSavePath + ProgramSettings.emailDataFileName);
        }


        private void checkForEmailsButton_Click(object sender, EventArgs e)
        {
            foreach (string mailboxName in this.mailBoxes.Keys)
            {
                EmailAccount mailbox;
                mailBoxes.TryGetValue(mailboxName, out mailbox);
                IEmailMessage newestEmail = mailbox.hasNewEmails ? mailbox.newEmailsList.First.Value : mailbox.emailsList.First.Value;

                getMessages(mailbox, newestEmail);
                saveDataToFile(ProgramSettings.fileSavePath + ProgramSettings.emailDataFileName);
                displayMessages();
            }
        }


        private void ShowEmailsButton_Click(object sender, EventArgs e)
        {
            displayMessages();
        }



        #endregion


        /// <summary>
        /// czyta dane kont pocztowych z pliku
        /// </summary>
        /// <param name="fileName"></param>
        private void readDataFromFile(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                byte[] buffer = new byte[fileStream.Length];
                using (BinaryReader binReader = new BinaryReader(fileStream))
                {
                    buffer = binReader.ReadBytes(buffer.Length);
                }

                MemoryStream originalStream = new MemoryStream(buffer);
                MemoryStream decompressedStream = new MemoryStream();

                using (GZipStream gzStream = new GZipStream(originalStream, CompressionMode.Decompress))
                {
                    gzStream.CopyTo(decompressedStream);
                }

                originalStream.Close();
                BinaryFormatter formatter = new BinaryFormatter();
                decompressedStream.Position = 0;
                this.mailBoxes = (Dictionary<string,EmailAccount>)formatter.Deserialize(decompressedStream);
                decompressedStream.Close();
            }

        }


       /// <summary>
       /// zapisuje dane (listę kont pocztowych) do skompresowanego pliku binarnego
       /// </summary>
       /// <param name="fileName"></param>
        private void saveDataToFile(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            {
                MemoryStream serializedMemoryStream = new MemoryStream();
                MemoryStream compressedStream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(serializedMemoryStream, this.mailBoxes);

                using (GZipStream gzStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    serializedMemoryStream.WriteTo(gzStream);
                }

                using (BinaryWriter binWriter = new BinaryWriter(fileStream))
                {
                    binWriter.Write(compressedStream.ToArray());
                }

                serializedMemoryStream.Close();
                compressedStream.Close();
            }
        }



        private void displayMessages()
        {
            //
            //parametry do obliczenia rozmiarów okna
            //

            int maxTabPageHeigth = 500;         //globalna max. dopuszczalna wysokość
            int maxActualTabPageHeigth = 0;     //wysokość najwyższej utworzonej stronicy
            int listviewWidth = 1000 + 10;          //suma szerokości kolumn + 10 jako margines

            int messagePacketHeight;
            int emailNumber = 0;

            this.SuspendLayout();

            TabControl tabControl = new TabControl();
            tabControl.SuspendLayout();
            tabControl.Location = new System.Drawing.Point(0, 30);
            tabControl.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(750, 50);
            this.Controls.Add(tabControl);


            foreach (string mailboxName in mailBoxes.Keys)
            {
                messagePacketHeight = 0;
                TabPage tabPage = generateBlankTabPage();
                tabPage.Text = mailboxName;

                ListView listView = generateBlankListview(listviewWidth);

                EmailAccount mailbox;
                mailBoxes.TryGetValue(mailboxName, out mailbox);

                foreach (var emailMessage in mailbox.newEmailsList)
                {
                    string[] emailDataRow = new string[] { emailMessage.messageDateTime.ToString(), emailMessage.FromAddress, emailMessage.Subject };

                    ListViewItem listvieRow = new ListViewItem(emailDataRow);
                    listView.Items.Add(listvieRow);

                    messagePacketHeight += emailNumber * 10;        //liczę mnożąc liczbę wierszy przez wysokość jednego wiersza
                    emailNumber++;
                }

                listView.Height = messagePacketHeight + 10;         //dodaję margines

                tabPage.Width = listView.Width + 3;
                tabPage.Height = listView.Height + 20;              //dodaję margines
                tabPage.Controls.Add(listView);

                tabPage.Height = tabPage.Height > maxTabPageHeigth ? maxTabPageHeigth : tabPage.Height;     //korekta wysokości tabPage, żeby nie przekraczała max
                maxActualTabPageHeigth = maxActualTabPageHeigth > tabPage.Height ? maxActualTabPageHeigth : tabPage.Height;

                tabControl.Controls.Add(tabPage);
                tabPage.ResumeLayout();
            }
            tabControl.Height = maxActualTabPageHeigth + 10;
            tabControl.Width = listviewWidth + 10;
            tabControl.ResumeLayout();

            this.Height = tabControl.Height + 50;
            this.Width = tabControl.Width + 30;
            this.ResumeLayout();
            this.Refresh();
        }

        private ListView generateBlankListview(int listviewWidth)
        {
            ListView listView = new ListView();
            ColumnHeader columnHeader1 = new ColumnHeader();
            ColumnHeader columnHeader2 = new ColumnHeader();
            ColumnHeader columnHeader3 = new ColumnHeader();

            listView.CheckBoxes = true;
            listView.Columns.AddRange(new ColumnHeader[] {
            columnHeader1,
            columnHeader2,
            columnHeader3});
            listView.HideSelection = false;
            listView.Location = new System.Drawing.Point(0, 0);
            listView.Size = new System.Drawing.Size(listviewWidth, 500);
            listView.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
            listView.TabIndex = 3;
            listView.UseCompatibleStateImageBehavior = false;
            listView.View = View.Details;

            columnHeader1.Width = 150;
            columnHeader1.Text = "Date";
            columnHeader2.Width = 250;
            columnHeader2.Text = "From";
            columnHeader3.Width = 600;
            columnHeader3.Text = "Subject";

            return listView;
        }


        private TabPage generateBlankTabPage()
        {
            TabPage tabPage = new TabPage();
            tabPage.Location = new System.Drawing.Point(0, 0);
            tabPage.Padding = new Padding(3);
            tabPage.Size = new System.Drawing.Size(100, 74);
            tabPage.Text = "tabPage1";
            tabPage.UseVisualStyleBackColor = true;
            tabPage.AutoScroll = true;
            tabPage.SuspendLayout();
            return tabPage;
        }

        private void getMessages(EmailAccount mailbox, int numberOfMessages=4)
        {

            IEmailAccountConfiguration emailConfiguration = mailbox.configuration;
            EmailService emailService = new EmailService(emailConfiguration);
            mailbox.addEmail(emailService.ReceiveEmails(numberOfMessages));

        }

        private void getMessages(EmailAccount mailbox, IEmailMessage email)
        {

            IEmailAccountConfiguration emailConfiguration = mailbox.configuration;
            EmailService emailService = new EmailService(emailConfiguration);
            mailbox.addEmail(emailService.ReceiveEmails(email));

        }


    }
}
