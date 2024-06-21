using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using EntityLayer.Entities;
using System.Diagnostics;
using static iTextSharp.text.TabStop;

namespace PreschoolManagmentSoftware.Static_Classes
{
    public static class PDFConverter
    {
        // Child
        public static void GenerateAndOpenChildReport(Child child, string parents, string groupName, List<Note> notes, List<string> attendances)
        {
            try
            {
                // Kreiraj memorijski stream za PDF dokument
                using (MemoryStream ms = new MemoryStream())
                {
                    // Kreiraj dokument
                    Document document = new Document();

                    // Postavi writer koji će pisati u memorijski stream
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);

                    // Otvori dokument za pisanje
                    document.Open();

                    // Dodaj naslov
                    Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    Paragraph titleParagraph = new Paragraph($"Izvještaj za dijete", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(titleParagraph);

                    // Dodaj prazan red
                    document.Add(new Paragraph("\n"));

                    // Dodaj sliku djeteta
                    if (child.ProfileImage != null && child.ProfileImage.Length > 0)
                    {
                        using (MemoryStream imageStream = new MemoryStream(child.ProfileImage))
                        {
                            Image childImage = Image.GetInstance(imageStream);
                            childImage.Alignment = Element.ALIGN_CENTER;
                            childImage.ScaleToFit(150f, 150f); // Adjust size as needed
                            document.Add(childImage);
                        }
                    }

                    // Dodaj prazan red
                    document.Add(new Paragraph("\n"));

                    // Dodaj ime i prezime djeteta
                    var fullName = $"{child.FirstName} {child.LastName}";
                    Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
                    Paragraph nameParagraph = new Paragraph(fullName, boldFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(nameParagraph);

                    // Dodaj prazan red
                    document.Add(new Paragraph("\n"));

                    // Dodaj informacije o djetetu
                    AddChildInfo(document, "OIB: ", child.PIN);
                    AddChildInfo(document, "Datum rodenja: ", child.DateOfBirth);
                    AddChildInfo(document, "Adresa: ", child.Adress);
                    AddChildInfo(document, "Mjesto rodenja: ", child.BirthPlace);
                    AddChildInfo(document, "Nacionalnost: ", child.Nationality);
                    AddChildInfo(document, "Spol: ", child.Sex);
                    AddChildInfo(document, "Status razvoja: ", child.DevelopmentStatus);
                    AddChildInfo(document, "Medicinske informacije: ", child.MedicalInformation);
                    AddChildInfo(document, "Grupa: ", groupName);
                    AddChildInfo(document, "Roditelji: ", parents);

                    // Dodaj prisustva
                    AddAttendancesInfo(document, attendances);

                    // Dodaj bilješke
                    AddNotesInfo(document, notes);

                    // Zatvori dokument
                    document.Close();
                    writer.Close();

                    // Spremi PDF dokument na privremenu lokaciju
                    string tempFilePath = Path.Combine(Path.GetTempPath(), "ChildReport.pdf");
                    File.WriteAllBytes(tempFilePath, ms.ToArray());

                    // Otvori PDF dokument pomoću zadane aplikacije za PDF
                    Process.Start(new ProcessStartInfo(tempFilePath) { UseShellExecute = true });
                }
            } catch (Exception ex)
            {
                // Rukovanje pogreškama
                Console.WriteLine("Error generating or opening PDF: " + ex.Message);
            }
        }

        // Employee
        public static void GenerateAndOpenEmployeeReport(User user)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document();

                    PdfWriter writer = PdfWriter.GetInstance(document, ms);

                    document.Open();

                    Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    Paragraph titleParagraph = new Paragraph($"Izvještaj za zaposlenika", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(titleParagraph);

                    document.Add(new Paragraph("\n"));

                    if (user.ProfileImage != null && user.ProfileImage.Length > 0)
                    {
                        using (MemoryStream imageStream = new MemoryStream(user.ProfileImage))
                        {
                            Image childImage = Image.GetInstance(imageStream);
                            childImage.Alignment = Element.ALIGN_CENTER;
                            childImage.ScaleToFit(150f, 150f);
                            document.Add(childImage);
                        }
                    }

                    document.Add(new Paragraph("\n"));

                    var fullName = $"{user.FirstName} {user.LastName}";
                    Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
                    Paragraph nameParagraph = new Paragraph(fullName, boldFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(nameParagraph);

                    document.Add(new Paragraph("\n"));

                    AddChildInfo(document, "Korisničko ime: ", user.Username);
                    AddChildInfo(document, "OIB: ", user.PIN);
                    AddChildInfo(document, "E-pošta: ", user.Email);
                    AddChildInfo(document, "Telefonski broj: ", user.Telephone);
                    AddChildInfo(document, "Datum rodenja: ", user.DateOfBirth);
                    AddChildInfo(document, "Spol: ", user.Sex);
                    AddChildInfo(document, "Uloga: ", user.Id_role == 1 ? "Administrator" : "Običan");

                    document.Close();
                    writer.Close();

                    string tempFilePath = Path.Combine(Path.GetTempPath(), "EmployeeReport.pdf");
                    File.WriteAllBytes(tempFilePath, ms.ToArray());

                    Process.Start(new ProcessStartInfo(tempFilePath) { UseShellExecute = true });
                }
            } catch (Exception ex)
            {
                Console.WriteLine("Error generating or opening PDF: " + ex.Message);
            }
        }

        // Parent
        public static void GenerateAndOpenParentReport(Parent parent, string children)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document();

                    PdfWriter writer = PdfWriter.GetInstance(document, ms);

                    document.Open();

                    Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    Paragraph titleParagraph = new Paragraph($"Izvještaj za roditelja", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(titleParagraph);

                    document.Add(new Paragraph("\n"));

                    if (parent.ProfileImage != null && parent.ProfileImage.Length > 0)
                    {
                        using (MemoryStream imageStream = new MemoryStream(parent.ProfileImage))
                        {
                            Image childImage = Image.GetInstance(imageStream);
                            childImage.Alignment = Element.ALIGN_CENTER;
                            childImage.ScaleToFit(150f, 150f);
                            document.Add(childImage);
                        }
                    }

                    document.Add(new Paragraph("\n"));

                    var fullName = $"{parent.FirstName} {parent.LastName}";
                    Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
                    Paragraph nameParagraph = new Paragraph(fullName, boldFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(nameParagraph);

                    document.Add(new Paragraph("\n"));

                    AddChildInfo(document, "OIB: ", parent.PIN);
                    AddChildInfo(document, "Datum rodenja: ", parent.DateOfBirth);
                    AddChildInfo(document, "E-pošta: ", parent.Email);
                    AddChildInfo(document, "Spol: ", parent.Sex);
                    AddChildInfo(document, "Djeca: ", children);

                    document.Close();
                    writer.Close();

                    string tempFilePath = Path.Combine(Path.GetTempPath(), "ParentReport.pdf");
                    File.WriteAllBytes(tempFilePath, ms.ToArray());

                    Process.Start(new ProcessStartInfo(tempFilePath) { UseShellExecute = true });
                }
            } catch (Exception ex)
            {
                Console.WriteLine("Error generating or opening PDF: " + ex.Message);
            }
        }

        private static void AddChildInfo(Document document, string label, string info)
        {
            Font labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Font infoFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            Paragraph paragraph = new Paragraph();
            paragraph.Add(new Chunk(label, labelFont));
            paragraph.Add(new Chunk(info, infoFont));
            document.Add(paragraph);
            document.Add(new Paragraph("\n"));
        }

        private static void AddAttendancesInfo(Document document, List<string> attendances)
        {
            Font attendanceTitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Font attendanceInfoFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            if (attendances != null && attendances.Count > 0)
            {
                document.Add(new Paragraph("Prisustva:", attendanceTitleFont));

                // Kreiraj string sa svim datumima prisustva odvojenim zarezom
                string attendancesString = string.Join(", ", attendances);

                // Dodaj prisustva u paragraf
                document.Add(new Paragraph(attendancesString, attendanceInfoFont));
                document.Add(new Paragraph("\n"));
            } else
            {
                document.Add(new Paragraph("Prisustva: ", attendanceTitleFont));
                document.Add(new Paragraph("Nema prisustva.", attendanceInfoFont));
                document.Add(new Paragraph("\n"));
            }
        }

        private static void AddNotesInfo(Document document, List<Note> notes)
        {
            Font noteTitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Font noteInfoFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            if (notes != null && notes.Count > 0)
            {
                document.Add(new Paragraph("Bilješke:", noteTitleFont));
                document.Add(new Paragraph("\n"));

                // Kreiraj tablicu sa 4 kolone
                PdfPTable table = new PdfPTable(4)
                {
                    WidthPercentage = 100
                };

                // Dodaj zaglavlja kolona
                table.AddCell(new PdfPCell(new Phrase("ID bilješke", noteTitleFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Datum", noteTitleFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Ponašanje", noteTitleFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Opis", noteTitleFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                // Dodaj podatke
                foreach (var note in notes)
                {
                    table.AddCell(new PdfPCell(new Phrase(note.Id.ToString(), noteInfoFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(note.Date, noteInfoFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(note.Behaviour, noteInfoFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(note.Description, noteInfoFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                }

                document.Add(table);
                document.Add(new Paragraph("\n"));
            } else
            {
                document.Add(new Paragraph("Bilješke: ", noteTitleFont));
                document.Add(new Paragraph("Nema bilješki.", noteInfoFont));
                document.Add(new Paragraph("\n"));
            }
        }
    }
}