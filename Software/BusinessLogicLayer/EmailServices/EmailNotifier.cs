using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Entities;

namespace BusinessLogicLayer.EmailServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    namespace BusinessLogicLayer.EmailServices
    {
        public class EmailNotifier
        {
            private string myMail = "mbanovicb@gmail.com";
            private string myPassword = "dubgsjqxuuiffbzs";
            MailMessage Message = new MailMessage();

            public EmailNotifier(string subject, string body, List<User> employees, List<Parent> parents, List<string> attachmentPaths)
            {
                Message.From = new MailAddress(myMail);
                Message.Subject = subject;

                if (employees != null)
                {
                    foreach (var employee in employees)
                    {
                        Message.To.Add(new MailAddress(employee.Email));
                        var personalizedBody = $"Poštovani {employee.FirstName} {employee.LastName},<br/><br/>{body}<br/><p>Slobodno se obratite našem timu ako imate bilo kakvih pitanja ili trebate dodatnu podršku.</p><p>S poštovanjem,</p><p>Vaš Administrator</p>";
                        SendEmail(personalizedBody, attachmentPaths);
                    }
                }

                if (parents != null)
                {
                    foreach (var parent in parents)
                    {
                        Message.To.Add(new MailAddress(parent.Email));
                        var personalizedBody = $"Poštovani {parent.FirstName} {parent.LastName},<br/><br/>{body}<br/><p>Slobodno se obratite našem timu ako imate bilo kakvih pitanja ili trebate dodatnu podršku.</p><p>S poštovanjem,</p><p>Vaš Administrator</p>";
                        SendEmail(personalizedBody, attachmentPaths);
                    }
                }
            }

            private void SendEmail(string body, List<string> attachmentPaths)
            {
                Message.Body = "<html><body>" + body + "</body></html>";
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
                Message.To.Clear();  // Clear the recipients for the next email
            }
        }
    }
}
