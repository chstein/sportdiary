using Common.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Sporty.Common.Mail
{
    public class MailSender
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MailSender));

        # region singleton definition

        private static readonly MailSender instance = new MailSender();

        static MailSender()
        {
            try
            {
                Log.Info("Create new instance of SmtpClient");
                instance.client = new SmtpClient();
                
                instance.client.EnableSsl = true;
                // Boolean.Parse(AppConfigHelper.GetWebConfigValue(Constants.SmtpuseSSL));
                if (string.IsNullOrEmpty(instance.client.Host))
                {
                    
                    Log.Warn("It seems SmtpClient is not configured with web.config. Set standard values.");
                    instance.client.Host = "smtp.gmail.com";
                    instance.client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    instance.client.Port = 587;
                    instance.client.Credentials = new NetworkCredential("christian@sportdiary.org", "IhmMgdl.1");
                }
                else
                {
                    Log.Info("SmptClient configured correctly");
                    Log.InfoFormat("{0}; {1}; {2}; {3}", instance.client.Host, instance.client.DeliveryMethod, instance.client.Port, instance.client.Credentials);
                }

                //<smtp from="christian@sportdiary.org" deliveryMethod="Network">
                //  <network host="smtp.gmail.com" port="587" userName="christian@sportdiary.org" password="IhmMgdl.1" />
                //</smtp>
                instance.queuedMessages = new Queue<MailMessage>();
            }
            catch (Exception ex)
            {
                Log.Error("Unable to send Mail", ex);
                instance = null;
            }
        }

        public static MailSender Instance
        {
            get { return instance; }
        }

        #endregion

        private SmtpClient client;
        public object locker = new object();
        private Queue<MailMessage> queuedMessages;
        private bool sending;

        public void SendMailAsync(MailMessage mail)
        {
            if (sending)
            {
                lock (locker)
                {
                    queuedMessages.Enqueue(mail);
                }
            }
            else
            {
                ThreadPool.QueueUserWorkItem(SendMail, mail);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendMail(object mailObj)
        {
            sending = true;
            var mail = (MailMessage)mailObj;
            try
            {
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception ex)
            {
                //LogManager.LogException("error sending mail", ex);
            }
            sending = false;
            MailMessage mm = null;
            lock (locker)
            {
                if (queuedMessages.Count != 0)
                {
                    mm = queuedMessages.Dequeue();
                }
            }
            if (mm != null)
            {
                SendMail(mm);
            }
        }
    }
}