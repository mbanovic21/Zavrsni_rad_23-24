using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class EmailService
    {
        private string fromMail = "thesnackalchemist2023@gmail.com";
        private string fromPassword = "dxtylhsnamqyjfex";
        MailMessage Message = new MailMessage();

        public EmailService(string firstName, string lastName, string email, string subject, string body, List<string> attachmentPaths)
        {
            Message.From = new MailAddress(fromMail);
            Message.Subject = subject;
            Message.To.Add(new MailAddress(email));

            // Svi podaci u tijelu e-pošte
            var fullBody = $"First Name: {firstName}<br/>Last Name: {lastName}<br/>{body}";
            Message.Body = "<html><body>" + fullBody + "</body></html>";
            Message.IsBodyHtml = true;

            // Dodavanje privitaka
            foreach (var attachmentPath in attachmentPaths)
            {
                Attachment attachment = new Attachment(attachmentPath);
                Message.Attachments.Add(attachment);
            }

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };

            smtpClient.Send(Message);
        }
    }
}
