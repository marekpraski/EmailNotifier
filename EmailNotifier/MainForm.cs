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

        private List<EmailAccount> mailBoxes = new List<EmailAccount>();

        //słownik konfiguracji kont mailowych, kluczem jest nazwa konta
        private Dictionary<string, EmailConfiguration> mailboxConfigDict = new Dictionary<string, EmailConfiguration>();        

        public MainForm()
        {
            InitializeComponent();
            initialSetup();
        }


        private void initialSetup()
        {
            setupMailboxes();
            getMaiboxConfigurations();
            //readMessageDictionaries();
        }

        private void setupMailboxes()
        {
            string emailDataFile = ProgramSettings.currentPath + @"\" + ProgramSettings.fileName;
            FileManipulator fm = new FileManipulator();
            if (fm.assertFileExists(emailDataFile))
            {
                using (FileStream fileStream = new FileStream(emailDataFile, FileMode.Open))
                {
                    readDataFromFile(fileStream);
                }                
            }
            else
            {
                ConfigurationForm configForm = new ConfigurationForm();
                configForm.acceptButtonClickedEvent += configurationFormAcceptButtonClicked;
                configForm.ShowDialog();
                using (FileStream fileStream = new FileStream(emailDataFile, FileMode.Create))
                {
                    saveDataToFile(fileStream);
                }
            }
        }

        private void readDataFromFile(FileStream fileStream)
        {
            byte[] buffer = new byte[fileStream.Length];
            using(BinaryReader binReader = new BinaryReader(fileStream))
            {
                buffer = binReader.ReadBytes(buffer.Length);
            }

            MemoryStream originalStream = new MemoryStream(buffer);
            MemoryStream decompressedStream = new MemoryStream();

            using(GZipStream gzStream = new GZipStream(originalStream, CompressionMode.Decompress))
            {
                gzStream.CopyTo(decompressedStream);
            }

            originalStream.Close();
            BinaryFormatter formatter = new BinaryFormatter();
            decompressedStream.Position = 0;
            mailBoxes = (List<EmailAccount>) formatter.Deserialize(decompressedStream);
            decompressedStream.Close();

        }

        private void configurationFormAcceptButtonClicked(object sender, ConfigurationFormEventArgs args)
        {
            mailBoxes.Add(args.emailAccount);
        }

        private void saveDataToFile(FileStream fileStream)
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


        private void getMaiboxConfigurations()
        {
            //foreach (string mailboxName in mailboxConfigDict.Keys)
            //{
            //    getMailboxConfiguration();
            //}
        }

        private void getMailboxConfiguration()
        {
            
        }

        private void readMessageDictionaries()
        {
            throw new NotImplementedException();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            EmailConfiguration emailConfiguration = null;

            foreach (EmailAccount mailbox in mailBoxes)
            {
                emailConfiguration = mailbox.configuration;
                EmailService emailService = new EmailService(emailConfiguration);
                emailService.ReceiveEmail(mailbox);
            }
        }

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
    }
}
