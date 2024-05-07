using BusinessLogicLayer.DBServices;
using BusinessLogicLayer;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Text.RegularExpressions;

namespace PreschoolManagmentSoftware.UserControls
{
    /// <summary>
    /// Interaction logic for ucRegistration.xaml
    /// </summary>
    public partial class ucRegistration : UserControl
    {
        public ucRegistration()
        {
            InitializeComponent();
        }

        //PIN
        private void textPIN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPIN.Focus();
        }

        private void txtPIN_TextChanged(object sender, TextChangedEventArgs e)
        {
            var PIN = txtPIN.Text;
            var placeholderPIN = textPIN;

            if (!string.IsNullOrEmpty(PIN) && PIN.Length >= 0)
            {
                placeholderPIN.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderPIN.Visibility = Visibility.Visible;
            }

            if (!AreAllDigits(PIN))
            {
                if (string.IsNullOrWhiteSpace(PIN)) return;
                MessageBox.Show("ID must be only digits.");
                return;
            }
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

        //DateOfBirth
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dpDateOfBirth.Focus();
            e.Handled = true;
        }

        private void textDateOfBirth_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dpDateOfBirth.IsDropDownOpen = !dpDateOfBirth.IsDropDownOpen;
        }

        //Gender

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

        //Telephone
        private void textTelephone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtTelephone.Focus();
        }

        private void txtTelephone_TextChanged(object sender, TextChangedEventArgs e)
        {
            var telephone = txtTelephone.Text;
            var placeholderTelephone = textTelephone;

            if (!string.IsNullOrEmpty(telephone) && telephone.Length >= 0)
            {
                placeholderTelephone.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderTelephone.Visibility = Visibility.Visible;
            }

            if (!IsValidTelephone(telephone))
            {
                if (string.IsNullOrWhiteSpace(telephone)) return;
                txtTelephone.Clear();
                MessageBox.Show("Unesite ispravan telefonski broj!");
                return;
            }
        }

        //Username
        private void textUsername_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtUsername.Focus();
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            var username = txtUsername.Text;
            var placeholderUsername = textUsername;

            if (!string.IsNullOrEmpty(username) && username.Length > 0)
            {
                placeholderUsername.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderUsername.Visibility = Visibility.Visible;
            }

            if (!string.IsNullOrWhiteSpace(username) && !IsValidUsername(username))
            {
                MessageBox.Show("Username can only contain lowercase letters and numbers.");
                txtUsername.Clear();
                return;
            }
        }

        //Password
        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            } else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        //Role

        //btnRegister
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var PIN = txtPIN.Text;
            var firstName = txtFirstname.Text;
            var lastName = txtLastname.Text;
            var date = dpDateOfBirth.Text;
            //var gender = sender as RadioButton;
            var email = txtEmail.Text;
            var telephone = txtTelephone.Text;
            var username = txtUsername.Text;
            var password = txtPassword.Password;
            //var role = sender as RadioButton;

            if (string.IsNullOrWhiteSpace(PIN) || PIN.Length < 11 || PIN.Length > 11)
            {
                MessageBox.Show("Please eneter your ID. It must be 11 digits.");
                txtPIN.Clear();
                return;
            }

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

            if (string.IsNullOrWhiteSpace(date))
            {
                MessageBox.Show("Molimo unesite datum rođenja.");
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

            if (string.IsNullOrWhiteSpace(telephone))
            {
                MessageBox.Show("Please enter a valid telephone number.");
                txtTelephone.Clear();
                return;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Molimo unesite korisničko ime.");
                txtUsername.Clear();
                return;
            }
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

        private bool IsValidTelephone(string telephone)
        {
            return Regex.IsMatch(telephone, @"^[\+0-9\s]+$");
        }

        private bool IsValidUsername(string username)
        {
            return Regex.IsMatch(username, @"^[a-z0-9]+$");
        }
    }
}
