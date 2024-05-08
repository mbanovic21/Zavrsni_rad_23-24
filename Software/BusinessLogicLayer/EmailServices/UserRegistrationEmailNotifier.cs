using System;
using System.Net;
using System.Net.Mail;

namespace BusinessLogicLayer
{
    public class UserRegistrationEmailNotifier
    {
        private readonly string myMail = "mbanovicb@gmail.com";
        private readonly string myPassword = "dubgsjqxuuiffbzs";

        public bool SendRegistrationEmail(string firstName, string lastName, string email, string username, string password, string subject)
        {
            var message = new MailMessage
            {
                From = new MailAddress(myMail),
                Subject = subject,
                IsBodyHtml = true,
                Body = GenerateEmailBody(firstName, lastName, username, password)
            };

            message.To.Add(new MailAddress(email));

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 465,
                Credentials = new NetworkCredential(myMail, myPassword),
                EnableSsl = true
            };

            try
            {
                smtpClient.Send(message);
                Console.WriteLine("Email sent successfully.");
                return true;
            } catch (SmtpFailedRecipientsException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    SmtpStatusCode status = innerException.StatusCode;

                    if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Console.WriteLine("Delivery failed.");
                        return false;
                    } else
                    {
                        Console.WriteLine($"Failed to deliver message to {innerException.FailedRecipient}");
                        return false;
                    }
                }
                return false;
            } catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex}");
                return false;
            }
        }


        private string GenerateEmailBody(string firstName, string lastName, string username, string password)
        {
            return $@"
            <html>
            <body>
                <p>Poštovani/a {firstName} {lastName},</p>
                <p>Dobrodošli u naš tim!</p>
                <p>Ovdje su vaši pristupni podaci za naš sustav:</p>
                <ul>
                    <li><strong>Korisničko ime:</strong> {username}</li>
                    <li><strong>Lozinka:</strong> {password}</li>
                </ul>
                <p>Slobodno se obratite našem timu ako imate bilo kakvih pitanja ili trebate dodatnu podršku.</p>
                <p>S poštovanjem,</p>
                <p>Vaš Administrator</p>
            </body>
            </html>";
        }
    }
}

