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
                Body = GenerateEmailBody(parent.FirstName, parent.LastName, child.ProfileImage, child.PIN, child.FirstName, child.LastName, child.DateOfBirth, child.Sex, child.Adress, child.Nationality, child.DevelopmentStatus, child.MedicalInformation, child.BirthPlace)
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


        private string GenerateEmailBody(string parentFirstName, string parentLastName, byte[] profileImage, string pin, string firstName, string lastName, string dateOfBirth, string gender, string address, string nationality, string developmentStatus, string medicalInformation, string birthPlace)
        {
            string imageBase64 = Convert.ToBase64String(profileImage);
            string imageSource = $"data:image/png;base64,{imageBase64}";

            return $@"
            <html>
            <body>
                <p>Poštovani/a {parentFirstName} {parentLastName}</p>
                <p>Vaše dijete {firstName} {lastName} je uspješno registrirano u naš sustav.</p>
                <p>Ovdje su detalji registracije:</p>
                <p>Profilna slika:</p>
                <img src='{imageSource}' alt='Profilna slika' />
                <ul>
                    <li><strong>Ime i prezime:</strong> {firstName} {lastName}</li>
                    <li><strong>OIB:</strong> {pin}</li>
                    <li><strong>Datum rođenja:</strong> {dateOfBirth}</li>
                    <li><strong>Spol:</strong> {gender}</li>
                    <li><strong>Adresa:</strong> {address}</li>
                    <li><strong>Nacionalnost:</strong> {nationality}</li>
                    <li><strong>Status razvoja:</strong> {developmentStatus}</li>
                    <li><strong>Medicinske informacije:</strong> {medicalInformation}</li>
                    <li><strong>Mjesto rođenja:</strong> {birthPlace}</li>
                </ul>
                <p>Slobodno se obratite našem timu ako imate bilo kakvih pitanja ili trebate dodatnu podršku.</p>
                <p>S poštovanjem,</p>
                <p>Vaš Administrator</p>
            </body>
            </html>";
        }
    }
}
