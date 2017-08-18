using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Net.Mail;
using System.Threading;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        Boolean isRunning = false;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Thread thread = new Thread(SendEmail);
            isRunning = true;
            thread.Start();
        }

        protected override void OnStop()
        {
            isRunning = false;
        }

        protected void SendEmail()
        {
            // Configure these parameters
            String smtpServer = "<mailserver>"; // Smtp server address, e.g. smtp.gmail.com
            int smtpPort = 4711;  // set smtp server port number here 
            String username = "<username>"; // login username, for gmail this is your email address
            String password = "<password>"; // login password
            Boolean enableSSL = true; // toggle SSL on/off depending on what your email service provider supports
            String toEmailAddress = "<toEmailAddress>"; // recipient email
            String fromEmailAddress = "<fromEmailAddress>"; // sender email
            int sendFrequencyInMinutes = 5;
            // configuration stop 

            while (isRunning)
            {
                try
                {
                    MailMessage mailMessage = new MailMessage();
                    SmtpClient smtpClient = new SmtpClient(smtpServer);
                    smtpClient.Port = smtpPort;
                    smtpClient.Credentials = new System.Net.NetworkCredential(username, password);
                    smtpClient.EnableSsl = enableSSL;

                    mailMessage.To.Add(toEmailAddress);
                    mailMessage.From = new MailAddress(fromEmailAddress);
                    mailMessage.Subject = "Indeed Sample " + DateTime.Now.ToString();
                    mailMessage.Body = "This is a sample";

                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    // Do something useful here
                }

                // Rest for a while
                Thread.Sleep(sendFrequencyInMinutes * 60 * 1000);
            }
        }
    }
}
