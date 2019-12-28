
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace EmailNotifier
{
    public partial class MainForm : Form
    {
        private enum EmailListType { newEmails, allEmails, none}

        private TabControl emailDisplayTabControl;
        private Label infoLabel;
        private Dictionary<string, EmailAccount> mailBoxes = new Dictionary<string, EmailAccount>();
        private Dictionary<string, List<string>> checkedEmailsDict = new Dictionary<string, List<string>>();

        private EmailListType emailsDisplayed = EmailListType.none;
        private bool newEmailsReceived = false;


        public MainForm()
        {
            InitializeComponent();
            initialSetup();

            //w RELEASE startuje zminimalizowany
#if DEBUG
#else
            if (mailBoxes.Count > 0)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
#endif
        }


        #region Region - start programu i konfiguracja

        private void initialSetup()
        {
            assertDataDirectoryExists();
            setupMailboxes();
            setTimers();
        }

        private void setTimers()
        {
            //zamieniam minuty na milisekundy, nie mniej niż 5 minut
            this.checkEmailsTimer.Interval = ProgramSettings.checkEmailTimespan * 60000 < 300000 ? 300000 : ProgramSettings.checkEmailTimespan * 60000;

            //zamieniam sekundy na milisekundy, nie mniej niż 20 sekund
            this.toggleNotifyiconTimer.Interval = ProgramSettings.showNotificationTimespan * 1000 < 20000 ? 20000 : ProgramSettings.showNotificationTimespan * 1000;
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
            configForm.saveButtonClickedEvent += configurationFormSaveButtonClicked;
            configForm.ShowDialog();
            configForm.saveButtonClickedEvent -= configurationFormSaveButtonClicked;    //likwiduję zdarzenie żeby zapobiec wyciekowi pamięci
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
                if(emailsDisplayed != EmailListType.none)
                {
                    closeEmailsDisplayWindow();
                }
                Hide();
                notifyIcon.Visible = true;
                notifyIcon.BalloonTipText = ("Click to open");
                notifyIcon.ShowBalloonTip(1000);
                checkEmailsTimer.Start();
            }
        }


        /// <summary>
        /// powrót głównego okna do normalnej postaci oraz zatrzymanie timera odpowiedzialnego za automatyczne sprawdzanie poczty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
            checkEmailsTimer.Stop();
            stopUserNotification();
            if (newEmailsReceived)
            {
                displayNewMessages();
                newEmailsReceived = false;
            }
        }



        /// <summary>
        /// ujednolica zawartość słowników kont: przechowywanego w tym oknie ze słownikiem konfiguracji kont otrzymanym z okna wczytywania konfiguracji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void configurationFormSaveButtonClicked(object sender, ConfigurationFormEventArgs args)
        {
            Form configForm = sender as Form;

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
            try
            {
                if (emailsDisplayed != EmailListType.none)
                {
                    closeEmailsDisplayWindow();
                }
                if (checkForEmails())
                {
                    saveDataToFile();
                    displayNewMessages();
                }
                else
                {
                    MyMessageBox.displayAndClose("no new messages");
                }
            }
            catch(ArgumentException exc)
            {
                MyMessageBox.display(exc.Message, MyMessageBoxType.Error);
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


        private void SettingsButton_Click(object sender, EventArgs e)
        {
            //ProgramSettings ps = new ProgramSettings();
            SettingsForm settingsForm = new SettingsForm(ProgramSettings.checkEmailTimespan, ProgramSettings.showNotificationTimespan, ProgramSettings.numberOfEmailsKept);
            settingsForm.saveSettingsEvent += setProgramSettings;
            settingsForm.ShowDialog();
            settingsForm.saveSettingsEvent -= setProgramSettings;
        }


        /// <summary>
        /// uruchamia funkcję zamykania okna wyświetlania emaili, jeżeli jest ono otwarte, w celu zapisania ewentualnych zmian
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MyMessageBox.display("This will exit the application, monitoring of email accounts will stop. Proceed?", MyMessageBoxType.YesNo) == MyMessageBoxResults.Yes)
            {
                if (emailsDisplayed != EmailListType.none)
                    closeEmailsDisplayWindow();
            }
            else
            {
                e.Cancel = true;
            }
        }


        #endregion



        #region Region - automat sprawdzający pocztę

        private void CheckEmailsTimer_Tick(object sender, EventArgs e)
        {
            if (checkForEmails())
            {
                saveDataToFile();
                this.newEmailsReceived = true;
                startUserNotification();
            }
        }

        private void startUserNotification()
        {
            notifyIcon.BalloonTipText = ("you've got mail!");
            //ShowBalloonTip timeout is deprecated as of Windows Vista. Notification display times are now based on system accessibility settings.
            //Minimum and maximum timeout values are enforced by the operating system and are typically 10 and 30 seconds, respectively, 
            //however this can vary depending on the operating system. (więcej - czytaj dokumentację)

            notifyIcon.ShowBalloonTip(1);
            toggleNotifyiconTimer.Start();
        }


        private void ToggleNotifyiconTimer_Tick(object sender, EventArgs e)
        {
            notifyIcon.ShowBalloonTip(1);           
        }


        private void stopUserNotification()
        {
            toggleNotifyiconTimer.Stop();
        }




        #endregion



        private void setProgramSettings(object sender, SettingsArgs args)
        {
            ProgramSettings.numberOfEmailsKept = args.emailNumberKept;
            ProgramSettings.checkEmailTimespan = args.emailCheckTimespan;
            ProgramSettings.showNotificationTimespan = args.notificationBubbleTimespan;
            setTimers();
        }


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
            DataBundle dataBundle;

            try
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
                    dataBundle = (DataBundle)formatter.Deserialize(decompressedStream);
                    decompressedStream.Close();
                }

                this.mailBoxes = dataBundle.mailBoxes;
                ProgramSettings.checkEmailTimespan = dataBundle.checkEmailTimespan;
                ProgramSettings.numberOfEmailsKept = dataBundle.numberOfEmailsKept;
                ProgramSettings.showNotificationTimespan = dataBundle.showNotificationTimespan;

            }
            catch (System.IO.InvalidDataException exc)
            {
                MyMessageBox.display("błąd odczytu z pliku danych\r\n" + exc.Message +"\r\n"+ exc.StackTrace, MyMessageBoxType.Error);
            }
            catch (System.InvalidCastException ex)
            {

                MyMessageBox.display("błąd odczytu z pliku danych\r\n" + ex.Message + "\r\n" + ex.StackTrace, MyMessageBoxType.Error);
            }
        }


       /// <summary>
       /// zapisuje dane (listę kont pocztowych) do skompresowanego pliku binarnego
       /// </summary>
       /// <param name="fileName"></param>
        private void saveDataToFile(string fileName)
        {
            DataBundle dataBundle = new DataBundle(mailBoxes)
            {
                numberOfEmailsKept = ProgramSettings.numberOfEmailsKept,
                showNotificationTimespan = ProgramSettings.showNotificationTimespan,
                checkEmailTimespan = ProgramSettings.checkEmailTimespan
            };

            using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            {
                MemoryStream serializedMemoryStream = new MemoryStream();
                MemoryStream compressedStream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(serializedMemoryStream, dataBundle);

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

                if (emailsList.Count > 0)
                {
                    TabPage tabPage = generateBlankTabPage();
                    ListView listView = generateBlankListview(listviewWidth);

                    foreach (var emailMessage in emailsList)
                    {
                        string[] emailDataRow = new string[] { emailMessage.messageDateTime.ToString(), emailMessage.FromAddress, emailMessage.Subject };

                        ListViewItem listRow = new ListViewItem(emailDataRow);
                        listRow.Name = emailMessage.messageId;
                        listView.Items.Add(listRow);

                        messagePacketHeight += emailNumber * 6;        //liczę mnożąc liczbę wierszy przez wysokość jednego wiersza
                        emailNumber++;
                    }

                    listView.Height = messagePacketHeight + 20;         //dodaję margines

                    tabPage.Text = mailboxName;
                    tabPage.Width = listView.Width + 3;
                    tabPage.Height = listView.Height + 20;              //dodaję margines
                    tabPage.Controls.Add(listView);

                    tabPage.Height = tabPage.Height > maxTabPageHeigth ? maxTabPageHeigth : tabPage.Height;     //korekta wysokości tabPage, żeby nie przekraczała max
                    maxActualTabPageHeigth = maxActualTabPageHeigth > tabPage.Height ? maxActualTabPageHeigth : tabPage.Height;

                    emailDisplayTabControl.Controls.Add(tabPage);
                    tabPage.ResumeLayout();
                }
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
            listView.HeaderStyle = ColumnHeaderStyle.Clickable;
            listView.OwnerDraw = true;
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

            //zdarzenia potrzebne do renderowania oraz kontroli zachowania checkboxu w nagłówku listview 
            listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(listView_ColumnClick);
            listView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(listView_DrawColumnHeader);
            listView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(listView_DrawItem);
            listView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(listView_DrawSubItem);

            columnHeader1.Width = 150;
            columnHeader1.Text = "         Date";
            columnHeader2.Width = 300;
            columnHeader2.Text = "From";
            columnHeader3.Width = 550;
            columnHeader3.Text = "Subject";

            return listView;
        }


        #region Region - renderowanie checkboxa w nagłowku listview w celu zaznaczania/odznaczania wszystkich emaili równocześnie

        private void listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                //stan checkboxa zapisuję w tagu nagłówka
                //zaraz po wyrenderowaniu tag jest null co metoda Convert.ToBoolean zwraca jako false
                bool isChecked = Convert.ToBoolean(e.Header.Tag);
                CheckBoxState state = isChecked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal;
                TextFormatFlags flags = TextFormatFlags.VerticalCenter;
                e.DrawBackground();
                CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(e.Bounds.Left + 4, e.Bounds.Top + 4), state);//ClientRectangle.Location
                e.DrawText(flags);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void listView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }


        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0)
            {
                ListView listView = sender as ListView;
                bool isChecked = Convert.ToBoolean(listView.Columns[e.Column].Tag);

                listView.Columns[e.Column].Tag = !isChecked;
                foreach (ListViewItem item in listView.Items)
                    item.Checked = !isChecked;

                listView.Invalidate();
            }
        }

        #endregion


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



        #region Region - czytanie emaili z serwisu


        private bool checkForEmails()
        {
            bool messagesReceived = false;

            if (!NetworkInterface.GetIsNetworkAvailable())       //nowe wiadomości sprawdzam tylko wtedy gdy jest internet
            {
                handleEmailServiceException(new EmailServiceException("brak internetu"));
            }
            else
            {
                foreach (string mailboxName in this.mailBoxes.Keys)
                {
                    try
                    {
                        EmailAccount mailbox;
                        mailBoxes.TryGetValue(mailboxName, out mailbox);

                        if (mailbox.allEmailsList.Count == 0)
                        {
                            if (getMessages(mailbox))
                                messagesReceived = true;
                        }
                        else
                        {
                            IEmailMessage newestEmail = mailbox.hasNewEmails ? mailbox.newEmailsList.First.Value : mailbox.allEmailsList.First.Value;

                            if (getMessages(mailbox, newestEmail))
                                messagesReceived = true;
                        }
                    }
                    catch (EmailServiceException e)
                    {
                        handleEmailServiceException(e);
                    }
                }
            }
            return messagesReceived;
        }


        private bool getMessages(EmailAccount mailbox, int numberOfMessages=4)
        {
            IEmailAccountConfiguration emailConfiguration = mailbox.configuration;
            EmailService emailService = new EmailService(emailConfiguration);
            LinkedList<IEmailMessage> messages = emailService.ReceiveEmails(numberOfMessages);
            if (messages.Count > 0)
            {
                mailbox.addEmail(messages);
                return true;
            }
            return false;
        }


        private bool getMessages(EmailAccount mailbox, IEmailMessage email)
        {
                IEmailAccountConfiguration emailConfiguration = mailbox.configuration;
                EmailService emailService = new EmailService(emailConfiguration);
                LinkedList<IEmailMessage> messages = emailService.ReceiveEmails(email);
                if (messages.Count > 0)
                {
                    mailbox.addEmail(messages);
                    return true;
                }
            
            return false;
        }

        private void handleEmailServiceException(Exception e)
        {
            MyMessageBox.displayAndClose(e.Message + "    " + e.InnerException + "\r\nsee the errorlog file for details", 30);
            string errorLogFileName = "emailNotifierError.log";
            using (FileStream file = new FileStream(errorLogFileName, FileMode.Append))
            {
                StreamWriter writer = new StreamWriter(file);
                writer.Write(DateTime.Now.ToString() + "\r\n" + e.Message + "\r\n" + e.InnerException + "\r\n" + e.Source + "\r\n" + e.StackTrace + "\r\n" + "\r\n");
                writer.Close();
            }
        }



        #endregion

    }
}
