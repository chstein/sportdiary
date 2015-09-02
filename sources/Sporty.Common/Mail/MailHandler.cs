using Common.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Sporty.Common.Mail
{
    public static class MailHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MailHandler));

        public static void SendActivationMail(string username, string emailAdress, string activationCode, string absoluteTemplatePath, string applicationServerPath)
        {
            var email = new MailMessage();
            string mailTemplate = "";
            if (File.Exists(absoluteTemplatePath))
            {
                using (var sr = new StreamReader(File.Open(absoluteTemplatePath, FileMode.Open, FileAccess.Read)))
                {
                    mailTemplate = sr.ReadToEnd();
                }
            }

            email.From = new MailAddress("christian@sportdiary.org", "Sportdiary Team");
            email.To.Add(new MailAddress(emailAdress));
            email.Subject = String.Format("{0}, Willkommen bei Sportdiary", username);

            if (mailTemplate.Length > 0)
            {
                email.Body = String.Format(mailTemplate,
                        username,
                        applicationServerPath,
                        activationCode);
                email.IsBodyHtml = true;
            }
            MailSender.Instance.SendMailAsync(email);

        }

        public static void SendCreateMetricMail(string username, string emailAdress, string mailTemplatePath)
        {
            Log.InfoFormat("Start to send Mail to {0} with {1}", emailAdress, mailTemplatePath);
            var email = new MailMessage();
            string mailTemplate = "";
            if (File.Exists(mailTemplatePath))
            {
                using (var sr = new StreamReader(File.Open(mailTemplatePath, FileMode.Open, FileAccess.Read)))
                {
                    mailTemplate = sr.ReadToEnd();
                }
            }

            email.From = new MailAddress("christian@sportdiary.org", "Sportdiary Team");
            email.To.Add(new MailAddress(emailAdress));
            email.Subject = String.Format("Hallo {0}, Zeit für einen neuen Tageswert", username);

            if (mailTemplate.Length > 0)
            {
                email.Body = String.Format(mailTemplate,
                        username);
                email.IsBodyHtml = true;
            }
            Log.InfoFormat("Building Mail complete. {0}; {1}; {2}; {3}", email.From, email.To, email.Subject, email.Body);
            MailSender.Instance.SendMailAsync(email);
            
        }

        public static void SendResetPasswordMail(string username, string emailAdress, object newPassword, string mailTemplatePath)
        {
            var email = new MailMessage();
            string mailTemplate = "";
            if (File.Exists(mailTemplatePath))
            {
                using (var sr = new StreamReader(File.Open(mailTemplatePath, FileMode.Open, FileAccess.Read)))
                {
                    mailTemplate = sr.ReadToEnd();
                }
            }

            email.From = new MailAddress("christian@sportdiary.org", "Sportdiary Team");
            email.To.Add(new MailAddress(emailAdress));
            email.Subject = String.Format("{0}, Dein Passwort wurde zurückgesetzt", username);

            if (mailTemplate.Length > 0)
            {
                email.Body = String.Format(mailTemplate,
                        username,
                        newPassword);
                email.IsBodyHtml = true;
            }
            MailSender.Instance.SendMailAsync(email);

        }
    }
}
