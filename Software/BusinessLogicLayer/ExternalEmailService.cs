using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class ExternalEmailService
    {
        private string toMail = "thesnackalchemist2023@gmail.com";
        private string toPassword = "dxtylhsnamqyjfex";
        MailMessage Message = new MailMessage();

        public ExternalEmailService(string firstName, string lastName, string email, string subject, string body, List<string> attachmentPaths)
        {
            Message.From = new MailAddress(email);
            Message.Subject = subject;
            Message.To.Add(new MailAddress(toMail));

            // Svi podaci u tijelu meilao
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
                Credentials = new NetworkCredential(toMail, toPassword),
                EnableSsl = true
            };

            smtpClient.Send(Message);
        }
    }
}
