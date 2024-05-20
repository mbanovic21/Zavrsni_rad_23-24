using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.EmailServices
{
    public class UserUpdateEmailNotifier
    {
        private readonly string myMail = "mbanovicb@gmail.com";
        private readonly string myPassword = "dubgsjqxuuiffbzs";

        public bool SendUploadEmail(string PIN, string firstName, string lastName, string date, string gender, string email, string telephone, string username, string password, string subject)
        {
            var message = new MailMessage
            {
                From = new MailAddress(myMail),
                Subject = subject,
                IsBodyHtml = true,
                Body = GenerateEmailBody(PIN, firstName, lastName, date, gender, email, telephone, username, password)
            };

            message.To.Add(new MailAddress(email));

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(myMail, myPassword),
                EnableSsl = true
            };

            try
            {
                smtpClient.Send(message);
                Console.WriteLine("Email sent successfully.");
                return true;
            } catch (SmtpFailedRecipientException ex)
            {
                SmtpStatusCode status = ex.StatusCode;
                if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                {
                    Console.WriteLine("Delivery failed.");
                    return false;
                } else
                {
                    Console.WriteLine($"Failed to deliver message to {ex.FailedRecipient}");
                    return false;
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex}");
                return false;
            }
        }


        private string GenerateEmailBody(string PIN, string firstName, string lastName, string date, string gender, string email, string telephone, string username, string password)
        {
            return $@"
            <html>
            <body>
                <p>Poštovani/a {firstName} {lastName},</p>
                <p>Dobrodošli u naš tim!</p>
                <p>Ovdje su vaši pristupni podaci za naš sustav:</p>
               <ul>
                    <li><strong>OIB:</strong> {PIN}</li>
                    <li><strong>Ime:</strong> {firstName}</li>
                    <li><strong>Prezime:</strong> {lastName}</li>
                    <li><strong>Datum rođenja:</strong> {date}</li>
                    <li><strong>Spol:</strong> {gender}</li>
                    <li><strong>Email:</strong> {email}</li>
                    <li><strong>Telefon:</strong> {telephone}</li>
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
