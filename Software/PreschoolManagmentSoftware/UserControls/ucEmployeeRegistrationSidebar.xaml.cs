﻿using BusinessLogicLayer.DBServices;
using BusinessLogicLayer;
using EntityLayer.Entities;
using Microsoft.Win32;
using SecurityLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace PreschoolManagmentSoftware.UserControls
{
    /// <summary>
    /// Interaction logic for ucEmployeeRegistrationSidebar.xaml
    /// </summary>
    public partial class ucEmployeeRegistrationSidebar : UserControl
    {

        private AutenticationManager AutenticationManager = new AutenticationManager();
        private UserServices UserServices = new UserServices();
        private string selectedImagePath { get; set; }

        public ucEmployeeRegistrationSidebar()
        {
            InitializeComponent();
        }   

        //Profile image
        private void ucRegistration_Loaded(object sender, RoutedEventArgs e)
        {
            SetInitialProfileImage();
        }

        private void SetInitialProfileImage()
        {
            var gender = GetSelectedGender();
            string imageName = gender == "Ženski" ? "female-user.png" : "male-user.png";
            string imagePath = "pack://application:,,,/Media/Images/" + imageName;
            profileImage.Source = new BitmapImage(new Uri(imagePath));
        }

        private void btnDeleteImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetInitialProfileImage();
            btnDeleteImage.Visibility = Visibility.Collapsed;
            selectedImagePath = null;
        }

        private void txtAddProfilePicture_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                profileImage.Source = new BitmapImage(new Uri(selectedImagePath));
                btnDeleteImage.Visibility = Visibility.Visible;
            }
        }

        private void rbGender_Checked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (selectedImagePath == null)
                {
                    SetInitialProfileImage();
                }
                return;
            }
        }

        // Metoda za pretvaranje slike u binarne podatke
        private byte[] ConvertImageToByteArray(string imagePath)
        {
            byte[] imageData;
            using (FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                imageData = new byte[fileStream.Length];
                fileStream.Read(imageData, 0, (int)fileStream.Length);
            }
            return imageData;
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
        private string GetSelectedGender()
        {
            return rbFemale.IsChecked == true ? "Ženski" : "Muški";
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
            //
        }

        private void txtPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            } else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        //btnGeneratePassword
        private void btnGeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            var generatedPassword = AutenticationManager.GeneratePassword();
            txtPassword.Clear();
            txtPassword.Text = generatedPassword;
        }

        //Role
        private int GetSelectedRole()
        {
            return rbUser.IsChecked == true ? 2 : 1;
        }

        //btnRegister
        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRegistration())
            {
                return;
            }

            string imagePathForRegistration;
            var PIN = txtPIN.Text;
            var firstName = txtFirstname.Text;
            var lastName = txtLastname.Text;
            var date = dpDateOfBirth.Text;
            var gender = GetSelectedGender();
            var email = txtEmail.Text;
            var telephone = txtTelephone.Text;
            var username = txtUsername.Text;
            var password = txtPassword.Text;
            (string hashedPassword, string salt) = AutenticationManager.HashPasswordAndGetSalt(password);
            var role = GetSelectedRole();

            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                imagePathForRegistration = selectedImagePath;
            } else
            {
                string imageName = gender == "Ženski" ? "female-user.png" : "male-user.png";
                //string projectPath = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\";
                imagePathForRegistration = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\" + imageName;
            }

            var userForRegistration = new User()
            {
                ProfileImage = ConvertImageToByteArray(imagePathForRegistration),
                PIN = PIN,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = date,
                Sex = gender,
                Email = email,
                Telephone = telephone,
                Username = username,
                Password = hashedPassword,
                Salt = salt,
                Id_role = role,
                Id_Group = null
            };


            var isRegistrated = await Task.Run(() => UserServices.RegistrateUser(userForRegistration));

            if (isRegistrated)
            {
                var result = MessageBox.Show("Novi korisnik je uspješno registriran! Želite li obavijestiti korisnika putem e-pošte?", "Obavijest", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Obavijesti korisnika putem e-pošte
                    var subject = "Uspješno ste registrirani u sustav!";
                    var emailNotifier = new UserRegistrationEmailNotifier();
                    var isEmailSent = emailNotifier.SendRegistrationEmail(firstName, lastName, email, username, password, subject);
                    if (!isEmailSent)
                    {
                        var isRemoved = await Task.Run(() => UserServices.RemoveUser(username, PIN));
                        if (isRemoved)
                        {
                            MessageBox.Show("Došlo je do pogreške prilikom slanja e-pošte!\nMolimo vas provjerite je li unesena ispravna adresa e-pošte.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            } else
            {
                MessageBox.Show("Došlo je do pogreške!");
            }
        }

        //Input validation
        private bool ValidateRegistration()
        {
            var PIN = txtPIN.Text;
            var firstName = txtFirstname.Text;
            var lastName = txtLastname.Text;
            var date = dpDateOfBirth.Text;
            var email = txtEmail.Text;
            var telephone = txtTelephone.Text;
            var username = txtUsername.Text;

            if (string.IsNullOrWhiteSpace(PIN) || PIN.Length < 11 || PIN.Length > 11)
            {
                MessageBox.Show("Please eneter your ID. It must be 11 digits.");
                txtPIN.Clear();
                return false;
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("Please enter your first name.");
                txtFirstname.Clear();
                return false;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Please enter your last name.");
                txtLastname.Clear();
                return false;
            }

            if (string.IsNullOrWhiteSpace(date))
            {
                MessageBox.Show("Molimo unesite datum rođenja.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter your email.");
                return false;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.");
                txtEmail.Clear();
                return false;
            }

            if (string.IsNullOrWhiteSpace(telephone) || telephone.Length < 10 || telephone.Length > 14)
            {
                MessageBox.Show("Please enter a valid telephone number.");
                txtTelephone.Clear();
                return false;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Molimo unesite korisničko ime.");
                txtUsername.Clear();
                return false;
            }

            return true;
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
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            return Regex.IsMatch(email, pattern);
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
