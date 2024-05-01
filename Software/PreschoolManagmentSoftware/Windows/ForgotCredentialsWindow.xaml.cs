using BusinessLogicLayer;
using Microsoft.Win32;
using PreschoolManagmentSoftware.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PreschoolManagmentSoftware.Windows
{
    /// <summary>
    /// Interaction logic for ForgotCredentialsWindow.xaml
    /// </summary>
    public partial class ForgotCredentialsWindow : Window
    {
        List<string> filePaths = new List<string>();

        public ForgotCredentialsWindow()
        {
            InitializeComponent();
        }

        private void textFirstname_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtFirstname.Focus();
        }

        private void txtFirstname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFirstname.Text) && txtFirstname.Text.Length >= 0)
            {
                textFirstname.Visibility = Visibility.Collapsed;
            } else
            {
                textFirstname.Visibility = Visibility.Visible;
            }
        }

        private void textLastname_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtLastname.Focus();
        }

        private void txtLastname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLastname.Text) && txtLastname.Text.Length >= 0)
            {
                textLastname.Visibility = Visibility.Collapsed;
            } else
            {
                textLastname.Visibility = Visibility.Visible;
            }
        }

        private void textID_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtID.Focus();
        }

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtID.Text) && txtID.Text.Length >= 0)
            {
                textID.Visibility = Visibility.Collapsed;
            } else
            {
                textID.Visibility = Visibility.Visible;
            }
        }

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length >= 0)
            {
                textEmail.Visibility = Visibility.Collapsed;
            } else
            {
                textEmail.Visibility = Visibility.Visible;
            }
        }

        private void textDescription_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rtxtDescription.Focus();
        }

         private void rtxtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Provjera je li RichTextBox prazan
            if (string.IsNullOrWhiteSpace(new TextRange(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentEnd).Text))
            {
                // Ako je prazan, prikaži textDescription
                textDescription.Visibility = Visibility.Visible;
            } else
            {
                // Inače, sakrij textDescription
                textDescription.Visibility = Visibility.Collapsed;
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog() { Multiselect = true };
            bool? response = openFileDialog.ShowDialog();
            if (response == true)
            {
                imgUpload.Visibility = Visibility.Collapsed;
                textImageDescription.Visibility = Visibility.Collapsed;
                var files = openFileDialog.FileNames;
                // Čuvanje putanja do datoteka u globalnu listu
                filePaths.AddRange(files);
                foreach (var file in files)
                {
                    var filename = System.IO.Path.GetFileName(file);
                    var fileInfo = new FileInfo(file);
                    var ucUpload = new UserControls.ucUpload()
                    {
                        FileName = filename,
                        FileSize = string.Format("{0} {1}", ((fileInfo.Length / 1024) + 1).ToString("0.0"), "Kb"),
                        UploadProgress = 100,
                        FilePath = file // Postavljanje putanje datoteke
                    };
                    UploadingFileList.Items.Add(ucUpload);
                }
                //MessageBox.Show("File paths: " + string.Join(", ", filePaths));
            }
        }

        public void RemoveItemFromList(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                if (filePaths.Contains(filePath))
                {
                    filePaths.Remove(filePath);
                    if (filePaths.Count == 0)
                    {
                        textImageDescription.Visibility = Visibility.Visible;
                        imgUpload.Visibility = Visibility.Visible;
                    }
                    //MessageBox.Show("Remaining files: " + filePaths.Count.ToString());
                } else
                {
                    MessageBox.Show("File path not found in the list.");
                }
            } else
            {
                MessageBox.Show("File path is empty or null.");
            }
        }


        private void SubmitRequest_Click(object sender, RoutedEventArgs e)
        {
            var firstName = txtFirstname.Text;
            var lastName = txtLastname.Text;
            var ID = txtID.Text;
            var email = txtEmail.Text;
            var description = new TextRange(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentEnd).Text;
            var subject = "Credential Retrieval Request Your Assistance Needed";

            // Provjeri je li prvi name ispravan
            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("Please enter your first name.");
                return;
            }

            // Provjeri je li prezime ispravno
            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Please enter your last name.");
                return;
            }

            // Provjeri je li ID ispravan
            if (string.IsNullOrWhiteSpace(ID))
            {
                MessageBox.Show("Please enter your ID.");
                return;
            }

            // Provjeri je li e-mail adresa ispravna
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.");
                return;
            }

            // Provjeri je li opis ispravan
            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please enter a description.");
                return;
            }

            // Pozovi EmailService samo ako su svi podaci ispravni
            new ExternalEmailService(firstName, lastName, email, subject, description, filePaths);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            } catch
            {
                return false;
            }
        }



        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
