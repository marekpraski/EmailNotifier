﻿
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
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace EmailNotifier
{
    public partial class MainForm : Form
    {
        private enum EmailListType { newEmails, allEmails, none}

        private TabControl emailDisplayTabControl;
        private Label infoLabel;
        private string infoButtonMessage = "";

        private Dictionary<string, EmailAccount> emailAccountDict = new Dictionary<string, EmailAccount>();
        private Dictionary<string, List<IEmailMessage>> checkedEmailsDict = new Dictionary<string, List<IEmailMessage>>();

        //słownik przechowujący maile zaznaczone przez użytkownika do skasowania, resetowany jest każdorazowo po operacji wczytywania/kasowania maili
        private Dictionary<string, List<IEmailMessage>> emailsToBeDeletedDict = new Dictionary<string, List<IEmailMessage>>();

        private EmailListType emailsDisplayed = EmailListType.none;
        private bool newEmailsReceived = false;
        private bool emailsDeletedFromServer = false;   //jeżeli true, to podczas zamykania programu zapisuję dane do pliku mimo braku nowych wiadomości,
                                                        //bo samo kasowanie wiadomości z serwera bez pobierania nowych wiadomości nie powoduje zapisywania danych do pliku

        private IEmailService emailService = null;

        private CancellationTokenSource cancellationTokenSource;



        public MainForm()
        {
            InitializeComponent();
            initialSetup();
            this.cancellationTokenSource = new CancellationTokenSource();

            //w RELEASE startuje zminimalizowany
#if DEBUG
#else
            if (emailAccountDict.Count > 0)
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
            setDefaultStatusLabel();
            setTimers();
        }


        private void assertDataDirectoryExists()
        {
            Directory.CreateDirectory(ProgramSettings.fileSavePath);
        }

 

        // jeżeli istnieje plik z danymi, czyta konfigurację kont z pliku
        private void setupMailboxes()
        {
            string emailDataFile = ProgramSettings.fileSavePath + ProgramSettings.emailDataFileName;
            if (File.Exists(emailDataFile))
            {                
                readDataFromFile(emailDataFile);
            }
        }


        private void setTimers()
        {
            //zamieniam minuty na milisekundy, nie mniej niż 5 minut
            this.checkEmailsTimer.Interval = ProgramSettings.checkEmailTimespan * 60000 < 300000 ? 300000 : ProgramSettings.checkEmailTimespan * 60000;

            //zamieniam sekundy na milisekundy, nie mniej niż 20 sekund
            this.toggleNotifyiconTimer.Interval = ProgramSettings.showNotificationTimespan * 1000 < 20000 ? 20000 : ProgramSettings.showNotificationTimespan * 1000;
        }


        // czyta dane kont pocztowych z pliku
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

                this.emailAccountDict = dataBundle.mailBoxes;
                this.emailsToBeDeletedDict = dataBundle.emailsToBeDeletedDict;
                ProgramSettings.checkEmailTimespan = dataBundle.checkEmailTimespan;
                ProgramSettings.numberOfEmailsKept = dataBundle.numberOfEmailsKept;
                ProgramSettings.showNotificationTimespan = dataBundle.showNotificationTimespan;
                //ProgramSettings.numberOfEmailsAtSetup = dataBundle.

            }
            catch (System.IO.InvalidDataException exc)
            {
                MyMessageBox.display("błąd odczytu z pliku danych\r\n" + exc.Message + "\r\n" + exc.StackTrace, MyMessageBoxType.Error);
            }
            catch (System.InvalidCastException ex)
            {

                MyMessageBox.display("błąd odczytu z pliku danych\r\n" + ex.Message + "\r\n" + ex.StackTrace, MyMessageBoxType.Error);
            }
        }

        // uruchamia okno konfiguracji kont mailowych i przypisuje metodę do zdarzenia zapisu w tym oknie
        private void configureEmailAccounts()
        {
            ConfigurationForm configForm = new ConfigurationForm(generateAccountConfigurationsDict(emailAccountDict));    //przesyłam słownik konfiguracji kont, żeby można było anulować zmiany
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

        #region Region status Label

        private void setDefaultStatusLabel()
        {
            if (emailAccountDict.Count == 0)
            {
                statusLabel.Visible = true;
                statusLabel.Text = "no email accounts defined";
            }
            else
            {
                statusLabel.Visible = false;
            }
        }



        private delegate void StatusLabelHandler(string txt);

        private void setStatusLabelText(string txt)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new StatusLabelHandler(setStatusLabelText), txt);
            }
            else
            {
                this.statusLabel.Visible = true;
                this.statusLabel.Text = txt;
            }
        }

        #endregion

        #region Region - interakcja użytkownika w głównym pasku i oknie programu

        // minimalizacja okna powoduje pojawienie się ikony na pasku oraz uruchomienie timera odpowiedzialnego za automatyczne sprawdzanie poczty
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
            else
            {
                this.ShowInTaskbar = true;
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MyMessageBox.display("This will exit the application, monitoring of email accounts will stop. Proceed?", MyMessageBoxType.YesNo) == MyMessageBoxResults.Yes)
            {
                this.cancellationTokenSource.Cancel();
                if (emailsDeletedFromServer)
                    saveDataToFile();
                if (emailsDisplayed != EmailListType.none)
                    closeEmailsDisplayWindow();
            }
            else
            {
                e.Cancel = true;
            }
        }


        // powrót głównego okna do normalnej postaci oraz zatrzymanie timera odpowiedzialnego za automatyczne sprawdzanie poczty
        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
            checkEmailsTimer.Stop();
            stopUserNotification();
            if (newEmailsReceived)
            {
                displayNewEmails();
                newEmailsReceived = false;
            }
        }


        //
        //pasek narzędziowy
        //

        private void EditAccountsButton_Click(object sender, EventArgs e)
        {
            configureEmailAccounts();
        }


        // ręczne sprawdzanie poczty
        private async void checkForEmailsButton_Click(object sender, EventArgs e)
        {
            try
            {                
                if (emailsDisplayed != EmailListType.none)
                {
                    closeEmailsDisplayWindow();
                }
                if (await checkForEmails())
                {
                    saveDataToFile();
                    displayNewEmails();
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
            displayNewEmails();
        }


        private void ShowAllEmailsButton_Click(object sender, EventArgs e)
        {
            displayAllEmails();
        }


        private void HideEmailsButton_Click(object sender, EventArgs e)
        {
            closeEmailsDisplayWindow();
            emailsDisplayed = EmailListType.none;
        }

        private void SaveToFileButon_Click(object sender, EventArgs e)
        {
            string textToPrint = DateTime.Now.ToString() + "\r\n\r\n";
            foreach (EmailAccount account in emailAccountDict.Values)
            {
                textToPrint += account.ToString();
            }
            saveTextToFile(textToPrint, ProgramSettings.fileSavePath + ProgramSettings.emailTextSummaryFileName);
            MyMessageBox.displayAndClose("saved",1);
        }


        private void ClearEmailsButton_Click(object sender, EventArgs e)
        {
            if (MyMessageBox.display("Deleting all emails from the data file. Account setting will not be changed. Proceed?", MyMessageBoxType.YesNo) == MyMessageBoxResults.Yes)
            {
                foreach (EmailAccount account in emailAccountDict.Values)
                {
                    account.clearData();
                }
            }
            saveDataToFile();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            //ProgramSettings ps = new ProgramSettings();
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.saveSettingsEvent += setProgramSettings;
            settingsForm.ShowDialog();
            settingsForm.saveSettingsEvent -= setProgramSettings;
        }


        private void InfoButton_Click(object sender, EventArgs e)
        {
            MyMessageBox.display(infoButtonMessage);
        }

        private void ListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listView = sender as ListView;
            if(listView.SelectedItems.Count == 1)
                displayEmailBody(listView);
        }

        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                markEmailsForDeletion(sender, e);
            }
            if(e.KeyCode == Keys.Down)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }


        //zdarzenie działa tylko gdy są zaznaczone jakieś emaile do skasowania
        //bo obsługuje ono przypadek, gdy ktoś odfajkuje nową wiadomość, którą poprzednio 
        //zaznaczył do skasowania, to jest ona odznaczana do skasowania, 
        //tj. żeby nie można było z serwera usunąć wiadomości, która nie jest zaznaczona jako przeczytana
        //zdarzenie aktywowane jest naciśnięciem klawisza 'delete'
        private void ListView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ListView listview = sender as ListView;
            ListViewItem item = listview.Items[e.Index];
            if (item.Checked && item.ImageIndex >= 0)        //tzn zaznaczona do skasowania, w tej akcji kliknięcia ją odznaczam
            {
                string accountName = listview.Name;
                EmailAccount account;
                emailAccountDict.TryGetValue(accountName, out account);

                List<IEmailMessage> emailsToDelete;
                emailsToBeDeletedDict.TryGetValue(accountName, out emailsToDelete);

                IEmailMessage email = item.Tag as IEmailMessage;

                markEmailAsNotToBeDeleted(emailsToDelete, email, account);
                item.ImageIndex = -1;
                listview.Refresh();
            }        
        }

        #endregion

        #region Region - interakcja użytkownika - przechwytywanie zdarzeń z innych okien programu



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
                if (emailAccountDict.ContainsKey(accountName))
                {
                    emailAccountDict.TryGetValue(accountName, out account);
                    account.configuration = accountConfig;
                }
                else
                {
                    EmailAccount newAccount = new EmailAccount();
                    newAccount.configuration = accountConfig;
                    newAccount.name = accountName;
                    emailAccountDict.Add(accountName, newAccount);
                }
            }

            // usuwam konto które jest w słowniku kont a nie ma go w słowniku otrzymanym z okna konfiguracji
            string[] oldAccountNames = new string[emailAccountDict.Keys.Count];
            emailAccountDict.Keys.CopyTo(oldAccountNames, 0);
            foreach (string accountName in oldAccountNames)
            {
                if (!args.emailAccountConfigs.ContainsKey(accountName))
                {
                    emailAccountDict.Remove(accountName);
                }
            }
            setDefaultStatusLabel();
            configForm.Close();
        }

        #endregion

        #region Region - automat sprawdzający pocztę

        private async void CheckEmailsTimer_Tick(object sender, EventArgs e)
        {
            if (await checkForEmails())
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

        #region Region - czytanie i kasowanie emaili z serwisu

        //tworzy nową instancję serwisu w zależności od jego rodzaju
        private void getEmailService(IEmailAccountConfiguration emailConfiguration)
        {
            emailService = null;
            switch (emailConfiguration.receiveServer.serverType)
            {
                case ServerType.POP:
                    emailService = new EmailServicePop(emailConfiguration);
                    break;
                case ServerType.IMAP:
                    emailService = new EmailServiceImap(emailConfiguration);
                    break;
            }
        }


        //główna metoda uruchamiana ręcznie oraz przez timer
        private async Task<bool> checkForEmails()
        {
            bool emailsReceived = false;

            if (!NetworkInterface.GetIsNetworkAvailable())       //nowe wiadomości sprawdzam tylko wtedy gdy jest internet
            {
                handleEmailServiceException(new EmailServiceException("brak internetu"));
            }
            else
            {
                foreach (string accountName in this.emailAccountDict.Keys)
                {
                    try
                    {
                        EmailAccount account;
                        emailAccountDict.TryGetValue(accountName, out account);
                        IEmailAccountConfiguration emailConfiguration = account.configuration;
                        getEmailService(emailConfiguration);

                        var token = cancellationTokenSource.Token;
                        Task t = Task.Factory.StartNew(()=> 
                        {
                            if (token.IsCancellationRequested)
                                throw new OperationCanceledException();
                            tryGetEmails(ref emailsReceived, account);
                        },token);
                        await t;

                    }
                    catch (EmailServiceException e)
                    {
                        handleEmailServiceException(e, accountName);
                    }
                    catch(System.IO.IOException ex)
                    {
                        handleEmailServiceException(ex, accountName);
                    }
                }
            }
            setDefaultStatusLabel();
            return emailsReceived;
        }

 
        private void tryGetEmails(ref bool emailsReceived, EmailAccount account)
        {
            setStatusLabelText("checking for emails: " + account.name);

            if (account.allEmailsList.Count == 0)
            {
                if (getEmails(account, ProgramSettings.numberOfEmailsAtSetup))
                    emailsReceived = true;
            }
            else
            {
                IEmailMessage newestEmail = account.allEmailsList.First.Value;

                if (getAndDeleteEmails(account, newestEmail))
                    emailsReceived = true;
            }
        }


        //metoda czytająca z serwera tylko gdy konto jest nowododane, tzn użyta jest jednorazowo dla każdego konta
        //dostarcza pierwszą paczkę emaili "na start"
        //zwraca true jeżeli z serwera załadowane zostaną jakieś wiadomości
        private bool getEmails(EmailAccount account, int numberOfEmails = 4)
        {

            LinkedList<IEmailMessage> emails = emailService.ReceiveEmails(numberOfEmails);

            if (emails.Count > 0)
            {
                account.addEmails(emails);
                return true;
            }
            return false;
        }


        //metoda czytająca z serwera wiadomości nowsze od przekazanej jako parametr
        //równocześnie kasuje z serwera zaznaczone wiadomości
        //zwraca true jeżeli z serwera załadowane zostaną jakieś wiadomości
        private bool getAndDeleteEmails(EmailAccount account, IEmailMessage email)
        {
            List<IEmailMessage> emailsToDelete;
            emailsToBeDeletedDict.TryGetValue(account.name, out emailsToDelete);
            LinkedList<IEmailMessage> newEmails = emailService.ReceiveAndDelete(email, emailsToDelete);

            if(emailsToDelete != null)
            {
                account.markEmailsDeletedFromServer(emailsToDelete);
                emailsToBeDeletedDict.Remove(account.name);
                emailsDeletedFromServer = true;
            }
            if (newEmails.Count > 0)
            {
                account.addEmails(newEmails);
            }
            return account.hasNewEmails;
        }


        private void handleEmailServiceException(Exception e, string accountName = "")
        {
            MyMessageBox.displayAndClose("błąd konta  " + accountName + "\r\n" + e.Message + "    " + e.InnerException + "\r\nsee the errorlog file for details", 5);
            if (!ProgramSettings.enableLogFile)
                return;
            string errorLogFileName = "emailNotifierError.log";
            string errorText = DateTime.Now.ToString() + "\r\n" + e.Message + "\r\n" + e.InnerException + "\r\n" + e.Source + "\r\n" + e.StackTrace + "\r\n" + "\r\n";
            saveTextToFile(errorText, errorLogFileName);
        }


        #endregion

        #region Region - wyświetlanie emaili



        private void displayNewEmails()
        {
            if(emailsDisplayed != EmailListType.none)
            {
                closeEmailsDisplayWindow();
            }

            infoButtonMessage = "Click on a message to read the content\r\n" +
                                "Press 'delete' to mark checked messages for deletion from server\r\n" +
                                "Press 'delete' again to unmark checked messages\r\n" +
                                "Messages will be deleted from server during the next check for new messages\r\n" +
                                "The deleted messages may still be recovered for a limited time (depends on the server settings)\r\n" +
                                "directly from the server, from the Trash\r\n" +
                                "Just check messages you want to mark as read but still keep them on server";

            emailsDisplayed = EmailListType.newEmails;
            displayEmails();
            hideEmailsButton.Enabled = true;
            showNewEmailsButton.Enabled = false;
            showAllEmailsButton.Enabled = true;
        }


        private void displayAllEmails()
        {
            if (emailsDisplayed != EmailListType.none)
            {
                closeEmailsDisplayWindow();
            }

            infoButtonMessage = "Click on a message to read the content\r\n" +
                                "Press 'delete' to mark checked messages for deletion from server\r\n" +
                                "Press 'delete' again to unmark checked messages\r\n" +
                                "Messages will be deleted from server during the next check for new messages\r\n" +
                                "The deleted messages may still be recovered for a limited time (depends on the server settings)\r\n" +
                                "directly from the server, from the Trash\r\n";

            emailsDisplayed = EmailListType.allEmails;
            displayEmails();
            hideEmailsButton.Enabled = true;
            showNewEmailsButton.Enabled = true;
            showAllEmailsButton.Enabled = false;
        }


        private void displayEmails()
        {
            //
            //parametry do obliczenia rozmiarów okna
            //

            int maxTabPageHeigth = 500;         //globalna max. dopuszczalna wysokość
            int maxActualTabPageHeigth = 0;     //wysokość najwyższej utworzonej stronicy
            int listviewWidth = 1000 + 30;          //suma szerokości kolumn + margines

            int messagePacketHeight;
            int displayedEmailNumber;

            this.SuspendLayout();

            addInfoLabel();
            addTabControl();

            foreach (string accountName in emailAccountDict.Keys)
            {
                EmailAccount account;
                emailAccountDict.TryGetValue(accountName, out account);
                LinkedList<IEmailMessage> emailsList = null;

                switch (this.emailsDisplayed)
                {
                    case EmailListType.allEmails:
                        emailsList = account.allEmailsList;
                        break;
                    case EmailListType.newEmails:
                        emailsList = account.newEmailsList;
                        break;
                }

                if (emailsList.Count > 0)
                {
                    TabPage tabPage = generateBlankTabPage();
                    ListView listView = generateBlankListview(listviewWidth);
                    listView.Name = account.name;

                    int emailIndex = 0;    //licznik wiadomości; posłuży do skrócenia listy do tej liczby wiadomości, którą użytkownik zdefiniował w konfiguracji

                    displayedEmailNumber = 0;       //kolejny numer wyświetlonej wiadomości służy do obliczenia wielkości okna
                    messagePacketHeight = 0;
                    foreach (var emailMessage in emailsList)
                    {
                        if (!emailMessage.deletedFromServer && emailMessage.visible)        //nagłówki emaili skasowanych z serwera pozostawiam na liście, ale nie chcę ich wyświetlać
                        {
                            string[] emailDataRow = new string[] { emailMessage.DateTime.ToString(), emailMessage.FromAddress, emailMessage.Subject };

                            ListViewItem listRow = new ListViewItem(emailDataRow);
                            listRow.Name = emailMessage.Id;
                            listRow.Tag = emailMessage;
                            listRow.ImageIndex = emailMessage.markedForDeletion ? 0 : -1;
                            listView.Items.Add(listRow);

                            displayedEmailNumber++;
                        }
                        emailIndex = displayedEmailNumber <= ProgramSettings.numberOfEmailsKept ? emailIndex +1 : emailIndex;  //zatrzymuję licznik gdy dojdę do max. liczby wiadomości
                    }
                    //uruchamiam funkcję usuwającą nadliczbowe emaile z mailboxa
                    if (emailsDisplayed == EmailListType.allEmails && displayedEmailNumber > ProgramSettings.numberOfEmailsKept)
                        account.trimEmailDisplayList(emailIndex);
                    //
                    //wysokość listy wiadomości
                    //
                    messagePacketHeight += displayedEmailNumber * (listView.Font.Height + 5);        //liczę mnożąc liczbę wierszy przez wysokość jednego wiersza, dodając odstępy między wierszami
                    listView.Height = messagePacketHeight + 30;         //dodaję margines
                    int maxHeigth =  listView.Height > maxTabPageHeigth ? maxTabPageHeigth : listView.Height;
                    listView.Height = maxHeigth;
                    //
                    //wysokość okna zakładki
                    //
                    tabPage.Text = accountName;
                    tabPage.Width = listView.Width + 3;
                    tabPage.Height = listView.Height + 20;              //dodaję margines
                    tabPage.Controls.Add(listView);

                    maxActualTabPageHeigth = maxActualTabPageHeigth > tabPage.Height ? maxActualTabPageHeigth : tabPage.Height;

                    emailDisplayTabControl.Controls.Add(tabPage);
                    tabPage.ResumeLayout();
                }
            }

            emailDisplayTabControl.Height = maxActualTabPageHeigth + 10;
            emailDisplayTabControl.Width = listviewWidth + 10;
            emailDisplayTabControl.ResumeLayout();

            this.Height = emailDisplayTabControl.Height + 70;
            this.Width = emailDisplayTabControl.Width + 20;
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



        private void addInfoLabel()
        {
            infoLabel = new Label();
            infoLabel.AutoSize = true;
            infoLabel.Location = new System.Drawing.Point(300, 5);
            infoLabel.Size = new System.Drawing.Size(35, 13);
            infoLabel.BackColor = System.Drawing.Color.Transparent;
            infoLabel.Font = new System.Drawing.Font("Arial", 15);
            infoLabel.ForeColor = System.Drawing.Color.Black;

            switch (this.emailsDisplayed)
            {
                case EmailListType.allEmails:
                    infoLabel.Text = "all emails";            
                    break;
                case EmailListType.newEmails:
                    infoLabel.Text = "new emails";
                    infoLabel.ForeColor = Color.Red;
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
            listView.ShowItemToolTips = true;
            listView.FullRowSelect = true;
            listView.MultiSelect = false;
            listView.Scrollable = true;
            listView.Location = new System.Drawing.Point(0, 0);
            listView.Size = new System.Drawing.Size(listviewWidth, 500);        //wysokość i tak jest później korygowana w zależności od liczby wiadomości
            listView.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
            listView.TabIndex = 3;
            listView.UseCompatibleStateImageBehavior = false;
            listView.View = View.Details;

            //zdarzenia potrzebne do renderowania oraz kontroli zachowania checkboxu w nagłówku listview 
            listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(listView_ColumnClick);
            listView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(listView_DrawColumnHeader);
            listView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(listView_DrawItem);
            listView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(listView_DrawSubItem);

            //do wyświetlania treści wiadomości po zaznaczeniu wiadomości na liście
            listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(ListView_MouseDoubleClick);

            //do zaznaczania wiadomości do skasowania
            listView.KeyDown += new System.Windows.Forms.KeyEventHandler(ListView_KeyDown);

            columnHeader1.Width = 150;
            columnHeader1.Text = "         Date";
            columnHeader2.Width = 300;
            columnHeader2.Text = "From";
            columnHeader3.Width = 550;
            columnHeader3.Text = "Subject";

            //ikonki przy wiadomościach na liście
            ImageList listViewIcons = new ImageList();
            listViewIcons.Images.Add(Properties.Resources.trash_16);
            listView.SmallImageList = listViewIcons;

            return listView;
        }


        // otwieranie okna z treścią wiadomości
        private void displayEmailBody(ListView listView)
        {
            ListViewItem selected = listView.SelectedItems[0];
            IEmailMessage selectedMessage = selected.Tag as IEmailMessage;

            string msgText = selectedMessage.Content != null ? selectedMessage.Content : "the email is empty";
            MyMessageBox.display(msgText);
        }



        // zaznaczanie wiadomości do skasowania, metoda jest uruchamiana tylko dla jednego mailboxa
        //bo senderem jest lista, która jest osobna dla każdego mailboxa
        private void markEmailsForDeletion(object sender, KeyEventArgs e)
        {
            ListView listView = sender as ListView;

            activateListviewItemCheckEvent(listView);

            ListView.CheckedListViewItemCollection checkedItems = listView.CheckedItems;
            if (checkedItems.Count > 0)
            {
                string accountName = listView.Name;

                List<IEmailMessage> emailsToDelete;
                if (emailsToBeDeletedDict.ContainsKey(accountName))
                {
                    emailsToBeDeletedDict.TryGetValue(accountName, out emailsToDelete);
                }
                else
                {
                    emailsToDelete = new List<IEmailMessage>();
                    emailsToBeDeletedDict.Add(accountName, emailsToDelete);
                }

                foreach (ListViewItem item in checkedItems)
                {
                    item.ImageIndex = item.ImageIndex < 0 ? 0 : -1;
                    toggleDeleteEmail(item, emailsToDelete, accountName);
                }
                listView.Refresh();
            }
        }


        private void activateListviewItemCheckEvent(ListView listView)
        {
            //aktywuję to zdarzenie dopiero teraz, gdy zaznaczam jakieś emaile do skasowania
            //bo obsługuje ono przypadek, gdy ktoś odfajkuje nową wiadomość, którą poprzednio 
            //zaznaczył do skasowania, to jest ona odznaczana do skasowania, 
            //tj. żeby nie można było z serwera usunąć wiadomości, która nie jest zaznaczona jako przeczytana
            listView.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(ListView_ItemCheck);
        }


        private void toggleDeleteEmail(ListViewItem item, List<IEmailMessage> emailsToDelete, string accountName)
        {
            IEmailMessage email = item.Tag as IEmailMessage;
            EmailAccount account;
            emailAccountDict.TryGetValue(accountName, out account);

            if (emailsToDelete.Contains(email))
            {
                markEmailAsNotToBeDeleted(emailsToDelete, email, account);
            }
            else
            {
                account.markEmailDelete(email);
                emailsToDelete.Add(email);
            }
        }


        private void markEmailAsNotToBeDeleted(List<IEmailMessage> emailsToDelete, IEmailMessage email, EmailAccount account)
        {
            account.markEmailDoNotDelete(email);
            emailsToDelete.Remove(email);
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




        // zamyka okno wyświetlania emaili, aktualizując nowe emaile w razie potrzeby i zapisując zmiany na dysk
        private void closeEmailsDisplayWindow()
        {
            infoButtonMessage = "";

            //wychwytuję zaznaczone emaile w każdej zakładce, ale tylko wtedy gdy wyświetlona była lista nowych maili
            if (emailsDisplayed == EmailListType.newEmails)
            {
                Control.ControlCollection tabControlContent = emailDisplayTabControl.Controls;
                checkedEmailsDict.Clear();
                int numberOfCheckedEmails = 0;                  //jeżeli zero, to nie wywołuję funkcji aktualizacji

                foreach (Control c in tabControlContent)    //tzn dla każdego konta mailowego
                {
                    TabPage page = c as TabPage;
                    string accountName = page.Text;
                    Control.ControlCollection tabPageContent = page.Controls;
                    ListView emailsDisplay = tabPageContent[0] as ListView;             //każdy tab zawiera tylko jedną listę
                    ListView.CheckedListViewItemCollection checkedItems = emailsDisplay.CheckedItems;
                    List<IEmailMessage> checkedEmails = new List<IEmailMessage>();
                    foreach (ListViewItem item in checkedItems)
                    {
                        IEmailMessage message = item.Tag as IEmailMessage;
                        checkedEmails.Add(message);
                    }
                    checkedEmailsDict.Add(accountName, checkedEmails);
                    numberOfCheckedEmails += checkedEmails.Count;
                }
                if (numberOfCheckedEmails > 0)
                {                                                   //aktualizuję słownik i zapisuję zmiany na dysk
                    updateEmailLists();
                    saveDataToFile();
                }
            }


            //dopiero po wychwyceniu zaznaczonych emaili zamykam okno
            emailDisplayTabControl.Dispose();
            infoLabel.Dispose();
            setMainFormStartSettings();
        }

        private void setMainFormStartSettings()
        {
            this.Width = 310;
            this.Height = 83;
            hideEmailsButton.Enabled = false;
            showNewEmailsButton.Enabled = true;
            showAllEmailsButton.Enabled = true;
        }



        #endregion

        #region Region - zapisywanie ustawień i danych



        private void setProgramSettings(object sender, SettingsArgs args)
        {
            ProgramSettings.numberOfEmailsKept = args.emailNumberKept;
            ProgramSettings.checkEmailTimespan = args.emailCheckTimespan;
            ProgramSettings.showNotificationTimespan = args.notificationBubbleTimespan;
            ProgramSettings.numberOfEmailsAtSetup = args.emailNumberAtSetup;
            ProgramSettings.enableLogFile = args.enableLogFile;
            saveTextToFile(ProgramSettings.settingsToText(), "emailNotifierSettings.txt");
            setTimers();
        }


        private void updateEmailLists()
        {
            foreach (string accountName in checkedEmailsDict.Keys)
            {
                var account = emailAccountDict[accountName];
                var emails = checkedEmailsDict[accountName];
                account.updateNewEmailsList(emails);
            }
        }


        private void saveDataToFile(string fileName)
        {
            DataBundle dataBundle = new DataBundle(emailAccountDict, emailsToBeDeletedDict)
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
            emailsDeletedFromServer = false;
        }

        #endregion


        private void saveTextToFile(string text, string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Append))
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(text);
                writer.Close();
            }
        }

    }
}
