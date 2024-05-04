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

        //FirstName
        private void textFirstname_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtFirstname.Focus();
        }

        private void txtFirstname_TextChanged(object sender, TextChangedEventArgs e)
        {
            var firstName = txtFirstname.Text;
            var placeholderFirstName = textFirstname;

            if (!string.IsNullOrEmpty(firstName) && firstName.Length >= 0)
            {
                placeholderFirstName.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderFirstName.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(firstName))
            {
                if (string.IsNullOrWhiteSpace(firstName)) return;
                txtFirstname.Clear();
                MessageBox.Show("First name can only contain letters.");
                return;
            }
        }

        //LastName
        private void textLastname_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtLastname.Focus();
        }

        private void txtLastname_TextChanged(object sender, TextChangedEventArgs e)
        {
            var lastName = txtLastname.Text;
            var placeholderLastName = textLastname;

            if (!string.IsNullOrEmpty(lastName) && lastName.Length >= 0)
            {
                placeholderLastName.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderLastName.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(lastName))
            {
                if (string.IsNullOrEmpty(lastName)) return;
                txtLastname.Clear();
                MessageBox.Show("Last name can only contain letters.");
                return;
            }
        }

        //ID
        private void textID_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtID.Focus();
        }

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ID = txtID.Text;
            var placeholderID = textID;

            if (!string.IsNullOrEmpty(ID) && ID.Length >= 0)
            {
                placeholderID.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderID.Visibility = Visibility.Visible;
            }

            if (!AreAllDigits(ID))
            {
                if (string.IsNullOrWhiteSpace(ID)) return;
                MessageBox.Show("ID must be only digits.");
                return;
            }
        }

        //Email
        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            var email = txtEmail.Text;
            var placeholderEmail = textEmail;

            if (!string.IsNullOrEmpty(email) && email.Length >= 0)
            {
                placeholderEmail.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderEmail.Visibility = Visibility.Visible;
            }
        }

        //Description
        private void textDescription_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rtxtDescription.Focus();
        }

        private void rtxtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            var description = new TextRange(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentEnd).Text;
            var placeholderDescription = textDescription;

            if (string.IsNullOrWhiteSpace(description))
            {
                placeholderDescription.Visibility = Visibility.Visible;
            } else
            {
                placeholderDescription.Visibility = Visibility.Collapsed;
            }
        }

        //Files to upload
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

        //Sumbit button
        private async void AsyncSubmitRequest_Click(object sender, RoutedEventArgs e)
        {
            var firstName = txtFirstname.Text;
            var lastName = txtLastname.Text;
            var ID = txtID.Text;
            var email = txtEmail.Text;
            var description = new TextRange(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentEnd).Text;
            var subject = "Credential Retrieval Request Your Assistance Needed";

            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("Please enter your first name.");
                txtFirstname.Clear();
                return;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Please enter your last name.");
                txtLastname.Clear();
                return;
            }

            if (string.IsNullOrWhiteSpace(ID) || ID.Length < 11 || ID.Length > 11)
            {
                MessageBox.Show("Please eneter your ID. It must be 11 digits.");
                txtID.Clear();
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter your email.");
                return;
            }
            
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.");
                txtEmail.Clear();
                return;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please enter a description.");
                txtFirstname.Clear();
                return;
            }

            await Task.Run(() => new ExternalEmailService(firstName, lastName, email, subject, description, filePaths));
            MessageBox.Show("Notification successfully sent.");
        }

        private bool IsLettersOnly(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == '-');
        }

        private bool AreAllDigits(string value)
        {
            foreach (char digit in value)
            {
                if (!char.IsDigit(digit))
                    return false;
            }
            return true;
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
