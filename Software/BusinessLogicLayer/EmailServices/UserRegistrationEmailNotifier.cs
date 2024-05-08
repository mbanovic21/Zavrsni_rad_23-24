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
                // Pokušaj slanja e-pošte
                smtpClient.Send(message);

                // Ako uspije slanje, ispiši poruku o uspjehu i vrati 'true'
                Console.WriteLine("Email sent successfully.");
                return true;
            } catch (SmtpFailedRecipientsException ex)
            {
                // Ako se dogodi iznimka SmtpFailedRecipientsException (koja može sadržavati više unutarnjih iznimki)
                // provjeri svaku unutarnju iznimku
                foreach (var innerException in ex.InnerExceptions)
                {
                    // Dobavi statusni kod iz unutarnje iznimke
                    SmtpStatusCode status = innerException.StatusCode;

                    // Provjeri je li poštanski sandučić zauzet ili nedostupan
                    if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                    {
                        // Ako je poštanski sandučić zauzet ili nedostupan, ispiši poruku i vrati 'false'
                        Console.WriteLine("Delivery failed.");
                        return false;
                    } else
                    {
                        // Ako je došlo do drugih problema pri isporuci, ispiši poruku o neuspjeloj isporuci i vrati 'false'
                        Console.WriteLine($"Failed to deliver message to {innerException.FailedRecipient}");
                        return false;
                    }
                }
                // Ako nije pronađena unutarnja iznimka koja odgovara, vrati 'false'
                return false;
            } catch (Exception ex)
            {
                // Ako se dogodi bilo koja druga iznimka, ispiši poruku o iznimci i vrati 'false'
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

