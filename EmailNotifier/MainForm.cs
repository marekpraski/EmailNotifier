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
using System.Threading.Tasks;

namespace EmailNotifier
{
    public partial class MainForm : Form
    {
        private TabControl emailDisplayTabControl;
        private Label infoLabel;
        private Dictionary<string, EmailAccount> mailBoxes = new Dictionary<string, EmailAccount>();
        private Dictionary<string, List<string>> checkedEmailsDict = new Dictionary<string, List<string>>();

        private EmailListType emailsDisplayed = EmailListType.none;

        private enum EmailListType { newEmails, allEmails, none}


        public MainForm()
        {
            InitializeComponent();
            initialSetup();
        }


        #region Region - start programu i konfiguracja

        private void initialSetup()
        {
            assertDataDirectoryExists();
            setupMailboxes();
        }


        /// <summary>
        /// tworzy katalog na plik z danymni, jeżeli go jeszcze nie ma
        /// </summary>
        private void assertDataDirectoryExists()
        {
            Directory.CreateDirectory(ProgramSettings.fileSavePath);
        }

        /// <summary>
        /// jeżeli istnieje plik z danymi, czyta konfigurację kont z pliku
        /// </summary>
        private void setupMailboxes()
        {
            string emailDataFile = ProgramSettings.fileSavePath + ProgramSettings.emailDataFileName;
            if (File.Exists(emailDataFile))
            {                
                readDataFromFile(emailDataFile);                              
            }
        }


        /// <summary>
        /// uruchamia okno konfiguracji kont mailowych i przypisuje metodę do zdarzenia zapisu w tym oknie
        /// </summary>
        private void configureEmailAccounts()
        {
            ConfigurationForm configForm = new ConfigurationForm(generateAccountConfigurationsDict(mailBoxes));    //przesyłam słownik konfiguracji kont, żeby można było anulować zmiany
            configForm.saveButtonClickedEvent += configurationFormSaveButtonClickedAsync;
            configForm.ShowDialog();
            configForm.saveButtonClickedEvent -= configurationFormSaveButtonClickedAsync;    //likwiduję zdarzenie żeby zapobiec wyciekowi pamięci
        }


        private Dictionary<string, IEmailAccountConfiguration> generateAccountConfigurationsDict(Dictionary<string, EmailAccount> mailBoxes)
        {
            Dictionary<string, IEmailAccountConfiguration> accountConfigDict = new Dictionary<string, IEmailAccountConfiguration>();
            EmailAccount account;
            foreach(string accountName in mailBoxes.Keys)
            {
                mailBoxes.TryGetValue(accountName, out account);
                accountConfigDict.Add(accountName, account.configuration);
            }
            return accountConfigDict;
        }


        /// <summary>
        /// dla nowych kont wczytuję startową liczbę maili i zapisuję do pliku
        /// </summary>
        /// <returns></returns>
        private async Task initialAccountSetupAsync()
        {
            if (this.mailBoxes.Count > 0)
            {
                bool messagesReceived = false;
                foreach (string mailboxName in this.mailBoxes.Keys)
                {
                    EmailAccount mailbox;
                    mailBoxes.TryGetValue(mailboxName, out mailbox);

                    if (mailbox.allEmailsList.Count == 0)
                    {
                        if (await getMessagesAsync(mailbox))
                            messagesReceived = true;
                    }
                }
                if (messagesReceived)
                    saveDataToFile();
            }
        }


        #endregion



        #region Region - interakcja użytkownika


        /// <summary>
        /// minimalizacja okna powoduje pojawienie się ikony na pasku oraz uruchomienie timera odpowiedzialnego za automatyczne sprawdzanie poczty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Resize(object sender, EventArgs e)
        { 
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(1000);
                checkEmailsTimer.Start();
            }
        }


        /// <summary>
        /// powrót głównego okna do normalnej postaci oraz zatrzymanie timera odpowiedzialnego za automatyczne sprawdzanie poczty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
            checkEmailsTimer.Stop();
        }


        /// <summary>
        /// ujednolica zawartość słowników kont: przechowywanego w tym oknie ze słownikiem konfiguracji kont otrzymanym z okna wczytywania konfiguracji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void configurationFormSaveButtonClickedAsync(object sender, ConfigurationFormEventArgs args)
        {
            Form configForm = sender as Form;
            configForm.Hide();

            // aktualizaję konfigurację konta, jeżeli to konto jest w obu słownikach; 
            //dodaję nowe konto jeżeli go nie ma w słowniku kont a jest w słowniku otrzymanym z okna konfiguracji
            foreach (string accountName in args.emailAccountConfigs.Keys)
            {
                IEmailAccountConfiguration accountConfig;
                args.emailAccountConfigs.TryGetValue(accountName, out accountConfig);
                EmailAccount account;
                if (mailBoxes.ContainsKey(accountName))
                {
                    mailBoxes.TryGetValue(accountName, out account);
                    account.configuration = accountConfig;
                }
                else
                {
                    EmailAccount newAccount = new EmailAccount();
                    newAccount.configuration = accountConfig;
                    newAccount.name = accountName;
                    mailBoxes.Add(accountName, newAccount);
                }
            }

            // usuwam konto które jest w słowniku kont a nie ma go w słowniku otrzymanym z okna konfiguracji
            string[] oldAccountNames = new string[mailBoxes.Keys.Count];
            mailBoxes.Keys.CopyTo(oldAccountNames, 0);
            foreach (string accountName in oldAccountNames)
            {
                if (!args.emailAccountConfigs.ContainsKey(accountName))
                {
                    mailBoxes.Remove(accountName);
                }
            }

            await initialAccountSetupAsync();
            configForm.Close();

        }

 


        //
        //pasek narzędziowy
        //

        private void EditAccountsButton_Click(object sender, EventArgs e)
        {
            configureEmailAccounts();
        }


        /// <summary>
        /// ręczne sprawdzanie poczty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkForEmailsButton_Click(object sender, EventArgs e)
        {
            if (checkForEmails())
            {
                saveDataToFile();
                displayNewMessages();
            }
            else
            {
                MyMessageBox.display("no new messages");
            }
        }



        private void ShowNewEmailsButton_Click(object sender, EventArgs e)
        {
            displayNewMessages();
        }


        private void ShowAllEmailsButton_Click(object sender, EventArgs e)
        {
            emailsDisplayed = EmailListType.allEmails;
            displayMessages();
            hideEmailsButton.Enabled = true;
            showNewEmailsButton.Enabled = false;
            showAllEmailsButton.Enabled = false;
        }


        private void HideEmailsButton_Click(object sender, EventArgs e)
        {
            closeEmailsDisplayWindow();
            emailsDisplayed = EmailListType.none;
        }


        /// <summary>
        /// uruchamia funkcję zamykania okna wyświetlania emaili, jeżeli jest ono otwarte, w celu zapisania ewentualnych zmian
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(emailsDisplayed != EmailListType.none)
                closeEmailsDisplayWindow();
        }


        #endregion



        #region Region - automat sprawdzający pocztę

        private void CheckEmailsTimer_Tick(object sender, EventArgs e)
        {
            //if (checkForEmails())
            //{
                //saveDataToFile();
                notifyUser();

            //}
            //notifyIcon.Icon = Properties.Resources.mailBlack;
        }

        private void notifyUser()
        {
            MyMessageBox.displayAndClose("you've got mail!");
        }

        #endregion





        /// <summary>
        /// usuwa maile zaznaczone przez użytkownika ze słownika nowych maili
        /// </summary>
        private void updateNewEmailsDict()
        {
            EmailAccount account = null;
            List<string> emailIds = null;
            foreach(string accountName in checkedEmailsDict.Keys)
            {
                mailBoxes.TryGetValue(accountName, out account);
                checkedEmailsDict.TryGetValue(accountName, out emailIds);
                foreach(string emailId in emailIds)
                {
                    account.removeNewEmail(emailId);
                }
            }   
        }




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

        private void saveDataToFile()
        {
            string fileName = (ProgramSettings.fileSavePath + ProgramSettings.emailDataFileName);
            saveDataToFile(fileName);
        }



        #region Region - wyświetlanie emaili



        private void displayNewMessages()
        {
            emailsDisplayed = EmailListType.newEmails;
            displayMessages();
            hideEmailsButton.Enabled = true;
            showNewEmailsButton.Enabled = false;
            showAllEmailsButton.Enabled = false;
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
            int emailNumber;

            this.SuspendLayout();

            addInfoLabel();
            addTabControl();

            foreach (string mailboxName in mailBoxes.Keys)
            {
                messagePacketHeight = 0;
                emailNumber = 0;
                TabPage tabPage = generateBlankTabPage();
                tabPage.Text = mailboxName;

                ListView listView = generateBlankListview(listviewWidth);

                EmailAccount mailbox;
                mailBoxes.TryGetValue(mailboxName, out mailbox);
                LinkedList<IEmailMessage> emailsList = null;

                switch (this.emailsDisplayed)
                {
                    case EmailListType.allEmails:
                        emailsList = mailbox.allEmailsList;
                        break;
                    case EmailListType.newEmails:
                        emailsList = mailbox.newEmailsList;
                        break;
                }


                foreach (var emailMessage in emailsList)
                {
                    string[] emailDataRow = new string[] { emailMessage.messageDateTime.ToString(), emailMessage.FromAddress, emailMessage.Subject };

                    ListViewItem listRow = new ListViewItem(emailDataRow);
                    listRow.Name = emailMessage.messageId;
                    listView.Items.Add(listRow);

                    messagePacketHeight += emailNumber * 10;        //liczę mnożąc liczbę wierszy przez wysokość jednego wiersza
                    emailNumber++;
                }

                listView.Height = messagePacketHeight + 10;         //dodaję margines

                tabPage.Width = listView.Width + 3;
                tabPage.Height = listView.Height + 20;              //dodaję margines
                tabPage.Controls.Add(listView);

                tabPage.Height = tabPage.Height > maxTabPageHeigth ? maxTabPageHeigth : tabPage.Height;     //korekta wysokości tabPage, żeby nie przekraczała max
                maxActualTabPageHeigth = maxActualTabPageHeigth > tabPage.Height ? maxActualTabPageHeigth : tabPage.Height;

                emailDisplayTabControl.Controls.Add(tabPage);
                tabPage.ResumeLayout();
            }
            emailDisplayTabControl.Height = maxActualTabPageHeigth + 10;
            emailDisplayTabControl.Width = listviewWidth + 10;
            emailDisplayTabControl.ResumeLayout();

            this.Height = emailDisplayTabControl.Height + 130;
            this.Width = emailDisplayTabControl.Width + 30;
            this.ResumeLayout();
            this.Refresh();
        }



        private void addTabControl()
        {
            emailDisplayTabControl = new TabControl();
            emailDisplayTabControl.SuspendLayout();
            emailDisplayTabControl.Location = new System.Drawing.Point(0, 30);
            emailDisplayTabControl.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
            emailDisplayTabControl.SelectedIndex = 0;
            emailDisplayTabControl.Size = new System.Drawing.Size(750, 50);
            this.Controls.Add(emailDisplayTabControl);
        }


        private void addInfoLabel()
        {
            infoLabel = new Label();
            infoLabel.AutoSize = true;
            infoLabel.Location = new System.Drawing.Point(200, 5);
            infoLabel.Size = new System.Drawing.Size(35, 13);
            infoLabel.BackColor = System.Drawing.SystemColors.Control;
            infoLabel.Font = new System.Drawing.Font("Arial", 15);
            infoLabel.ForeColor = System.Drawing.Color.Black;

            switch (this.emailsDisplayed)
            {
                case EmailListType.allEmails:
                    infoLabel.Text = "all emails";
                    break;
                case EmailListType.newEmails:
                    infoLabel.Text = "new emails - check emails to mark them as read";
                    break;
            }
            this.Controls.Add(infoLabel);
            infoLabel.BringToFront();
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


        /// <summary>
        /// zamyka okno wyświetlania emaili, aktualizując nowe emaile w razie potrzeby i zapisując zmiany na dysk
        /// </summary>
        private void closeEmailsDisplayWindow()
        {
            //wychwytuję zaznaczone emaile w każdej zakładce, ale tylko wtedy gdy wyświetlona była lista nowych maili
            if (emailsDisplayed == EmailListType.newEmails)
            {
                Control.ControlCollection tabControlContent = emailDisplayTabControl.Controls;
                checkedEmailsDict.Clear();
                int numberOfCheckedEmails = 0;                  //jeżeli zero, to nie wywołuję funkcji aktualizacji
                foreach (Control c in tabControlContent)
                {
                    TabPage page = c as TabPage;
                    string accountName = page.Text;
                    Control.ControlCollection tabPageContent = page.Controls;
                    ListView emailsDisplay = tabPageContent[0] as ListView;             //każdy tab zawiera tylko jedną listę
                    ListView.CheckedListViewItemCollection checkedEmails = emailsDisplay.CheckedItems;
                    List<string> checkedEmailsIDs = new List<string>();
                    foreach (ListViewItem item in checkedEmails)
                    {
                        checkedEmailsIDs.Add(item.Name);
                    }
                    checkedEmailsDict.Add(accountName, checkedEmailsIDs);
                    numberOfCheckedEmails += checkedEmailsIDs.Count;
                }
                if (numberOfCheckedEmails > 0)
                {                                                   //aktualizuję słownik i zapisuję zmiany na dysk
                    updateNewEmailsDict();
                    saveDataToFile();
                }
            }


            //dopiero po wychwyceniu zaznaczonych emaili zamykam okno
            emailDisplayTabControl.Dispose();
            infoLabel.Dispose();
            this.Width = 230;
            this.Height = 80;
            hideEmailsButton.Enabled = false;
            showNewEmailsButton.Enabled = true;
            showAllEmailsButton.Enabled = true;
        }

        #endregion



        #region Region - czytanie czytanie emaili z serwisu


        private bool checkForEmails()
        {
            bool messagesReceived = false;

            foreach (string mailboxName in this.mailBoxes.Keys)
            {
                EmailAccount mailbox;
                mailBoxes.TryGetValue(mailboxName, out mailbox);
                IEmailMessage newestEmail = mailbox.hasNewEmails ? mailbox.newEmailsList.First.Value : mailbox.allEmailsList.First.Value;

                if (getMessages(mailbox, newestEmail))
                    messagesReceived = true;
            }
            return messagesReceived;
        }


        private async Task<bool> getMessagesAsync(EmailAccount mailbox, int numberOfMessages=4)
        {
            IEmailAccountConfiguration emailConfiguration = mailbox.configuration;
            EmailService emailService = new EmailService(emailConfiguration);
            LinkedList<IEmailMessage> messages = await emailService.ReceiveEmailsAsync(numberOfMessages);
            if(messages.Count > 0)
                mailbox.addEmail(messages);
            return messages.Count > 0;
        }

        private bool getMessages(EmailAccount mailbox, IEmailMessage email)
        {

            IEmailAccountConfiguration emailConfiguration = mailbox.configuration;
            EmailService emailService = new EmailService(emailConfiguration);
            LinkedList<IEmailMessage> messages = emailService.ReceiveEmails(email);
            if (messages.Count > 0)
                mailbox.addEmail(messages);
            return messages.Count > 0;
        }

        #endregion


    }
}
