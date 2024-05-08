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
        private string myMail = "mbanovicb@gmail.com";
        private string myPassword = "dubgsjqxuuiffbzs";
        MailMessage Message = new MailMessage();

        public ExternalEmailService(string firstName, string lastName, string email, string subject, string body, List<string> attachmentPaths)
        {
            Message.From = new MailAddress(myMail);
            Message.Subject = subject;
            Message.To.Add(new MailAddress(myMail));

            // Svi podaci u tijelu meilao
            var fullBody = $"<strong>First Name:</strong> {firstName}<br/><strong>Last Name:</strong> " +
                $"{lastName}<br/><strong>Email:</strong> {email}<br/><strong>Problem description:</strong><br/>{body}";
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
                Credentials = new NetworkCredential(myMail, myPassword),
                EnableSsl = true
            };

            smtpClient.Send(Message);
        }
    }
}
