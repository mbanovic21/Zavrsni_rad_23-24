using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Entities;
using EntityLayer;
using System.Windows.Media.Imaging;
using System.IO;

namespace BusinessLogicLayer.EmailServices
{
    public class ChildRegistrationEmailNotifier
    {
        private readonly string myMail = "mbanovicb@gmail.com";
        private readonly string myPassword = "dubgsjqxuuiffbzs";

        public bool SendRegistrationEmail(string subject, Parent parent, Child child)
        {
            var message = new MailMessage
            {
                From = new MailAddress(myMail),
                Subject = subject,
                IsBodyHtml = true,
                Body = GenerateEmailBody(parent.FirstName, parent.LastName, child)
            };

            message.To.Add(new MailAddress(parent.Email));

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


        private string GenerateEmailBody(string parentFirstName, string parentLastName, Child child)
        {
            return $@"
            <html>
            <body>
                <p>Poštovani/a {parentFirstName} {parentLastName}</p>
                <p>Vaše dijete <strong>{child.FirstName} {child.LastName}</strong> je uspješno registrirano u naš sustav.</p>
                <p>Ovdje su detalji registracije:</p>
                <ul>
                    <li><strong>Ime i prezime:</strong> {child.FirstName} {child.LastName}</li>
                    <li><strong>OIB:</strong> {child.PIN}</li>
                    <li><strong>Datum rođenja:</strong> {child.DateOfBirth}</li>
                    <li><strong>Spol:</strong> {child.Sex}</li>
                    <li><strong>Adresa:</strong> {child.Adress}</li>
                    <li><strong>Mjesto rođenja:</strong> {child.BirthPlace}</li>
                    <li><strong>Nacionalnost:</strong> {child.Nationality}</li>
                    <li><strong>Status razvoja:</strong> {child.DevelopmentStatus}</li>
                    <li><strong>Medicinske informacije:</strong> {child.MedicalInformation}</li>>
                </ul>
                <p>Slobodno se obratite našem timu ako imate bilo kakvih pitanja ili trebate dodatnu podršku.</p>
                <p>S poštovanjem,</p>
                <p>Vaš Administrator</p>
            </body>
            </html>";
        }
    }
}
