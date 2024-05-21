using BusinessLogicLayer;
using BusinessLogicLayer.DBServices;
using BusinessLogicLayer.EmailServices;
using EntityLayer;
using EntityLayer.Entities;
using Microsoft.Win32;
using PreschoolManagmentSoftware.Static_Classes;
using SecurityLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interaction logic for ucEmployeeEditProfile.xaml
    /// </summary>
    public partial class ucEmployeeEditProfile : UserControl
    {
        private string _selectedImagePath { get; set; }
        private User _user { get; set; }
        private User _updatedUser { get; set; }
        private ucEmployeeAdministrating _ucEmployeeAdministrating { get ; set; }
        private AutenticationManager _autenticationManager = new AutenticationManager();
        private UserServices _userServices = new UserServices();
        public ucEmployeeEditProfile(User user, ucEmployeeAdministrating ucEmployeeAdministrating)
        {
            InitializeComponent();
            _user = user;
            _ucEmployeeAdministrating = ucEmployeeAdministrating;
        }

        //OnInit
        private void ucEditProfile_Loaded(object sender, RoutedEventArgs e)
        {
            var _selectedImagePath = BitmapImageConverter.ConvertByteArrayToBitmapImage(_user.ProfileImage);
            var email = _user.Email;
            var atIndex = email.IndexOf('@');
            var emailUsername = email.Substring(0, atIndex);
            var emailDomain = email.Substring(atIndex);

            textFirstname.Text = _user.FirstName;
            textLastname.Text = _user.LastName;
            textUsername.Text = _user.Username;
            textPIN.Text = _user.PIN;
            textEmail.Text = $"{emailUsername}\n{emailDomain}";
            textTelephone.Text = _user.Telephone;
            dpDateOfBirth.SelectedDate = DateTime.Parse(_user.DateOfBirth);
            string UserGenderWithoutSpaces = _user.Sex.Trim().Replace(" ", "");
            Console.WriteLine(UserGenderWithoutSpaces);
            if (UserGenderWithoutSpaces == "Muški")
            {
                rbMale.IsChecked = true;
                rbFemale.IsChecked = false;
            } else if (UserGenderWithoutSpaces == "Ženski")
            {
                rbMale.IsChecked = false;
                rbFemale.IsChecked = true;
            }

            if (_user.Id_role == 1)
            {
                rbAdmin.IsChecked = true;
                rbUser.IsChecked = false;
            } else if (_user.Id_role == 2)
            {
                rbAdmin.IsChecked = false;
                rbUser.IsChecked = true;
            }

            imgProfile.Source = _selectedImagePath;
            _updatedUser = _user;
        }

        //Profile picture
        private void btnEditPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedImagePath = openFileDialog.FileName;
                imgProfile.Source = new BitmapImage(new Uri(_selectedImagePath));
                btnDeleteImage.Visibility = Visibility.Visible;
            }
        }

        private void btnDeleteImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetInitialProfileImage();
            btnDeleteImage.Visibility = Visibility.Collapsed;
            _selectedImagePath = null;
        }

        private void SetInitialProfileImage()
        {
            var gender = GetSelectedGender();
            string imageName = gender == "Ženski" ? "female-user-white.png" : "male-user-white.png";
            string imagePath = "pack://application:,,,/Media/Images/" + imageName;
            imgProfile.Source = new BitmapImage(new Uri(imagePath));
        }

        //FirstName
        private void textFirstname_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
                placeholderFirstName.Text = _user.FirstName;
                placeholderFirstName.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(firstName))
            {
                if (string.IsNullOrEmpty(firstName)) return;
                txtLastname.Clear();
                MessageBox.Show("Ime može sadržavati samo slova.");
                return;
            }
        }

        //LastName
        private void textLastname_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
                placeholderLastName.Text = _user.LastName;
                placeholderLastName.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(lastName))
            {
                if (string.IsNullOrEmpty(lastName)) return;
                txtLastname.Clear();
                MessageBox.Show("Prezime može sadržavati samo slova.");
                return;
            }
        }

        //Username
        private void textUsername_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
                MessageBox.Show("Korisničko ime može sadržavati samo mala slova i brojeve.");
                txtUsername.Clear();
                return;
            }
        }

        //PIN
        private void textPIN_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
                placeholderPIN.Text = _user.PIN;
                placeholderPIN.Visibility = Visibility.Visible;
            }

            if (!AreAllDigits(PIN))
            {
                if (string.IsNullOrWhiteSpace(PIN)) return;
                MessageBox.Show("OIB mora sadržavati samo znamenke.");
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

        //Email
        private void textEmail_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
                placeholderEmail.Text = _user.Email;
                placeholderEmail.Visibility = Visibility.Visible;
            }
        }

        //Telephone
        private void textTelephone_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
                placeholderTelephone.Text = _user.Telephone;
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

        //Gender
        private void rbGender_Checked(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (_selectedImagePath == null)
                {
                    SetInitialProfileImage();
                }
                return;
            }
        }

        private string GetSelectedGender()
        {
            return rbFemale.IsChecked == true ? "Ženski" : "Muški";
        }

        //Role
        private int GetSelectedRole()
        {
            return rbUser.IsChecked == true ? 2 : 1;
        }

        //Password
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
            var generatedPassword = _autenticationManager.GeneratePassword();
            txtPassword.Clear();
            txtPassword.Text = generatedPassword;
        }

        //btnSave
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRegistration())
            {
                return;
            }

            string newImage;
            var firstname = string.IsNullOrWhiteSpace(txtFirstname.Text) ? _user.FirstName : txtFirstname.Text;
            string lastName = string.IsNullOrWhiteSpace(txtLastname.Text) ? _user.LastName : txtLastname.Text;
            string username = string.IsNullOrWhiteSpace(txtUsername.Text) ? _user.Username : txtUsername.Text;
            string PIN = string.IsNullOrWhiteSpace(txtPIN.Text) ? _user.PIN : txtPIN.Text;
            string email = string.IsNullOrWhiteSpace(txtEmail.Text) ? _user.Email : txtEmail.Text;
            string telephone = string.IsNullOrWhiteSpace(txtTelephone.Text) ? _user.Telephone : txtTelephone.Text;
            string date = dpDateOfBirth.Text ?? _user.DateOfBirth;
            string gender = GetSelectedGender();
            var role = GetSelectedRole();
            var password = string.IsNullOrWhiteSpace(txtPassword.Text) ? _user.Password : txtPassword.Text;
            var oldSalt = string.IsNullOrWhiteSpace(txtPassword.Text) ? _user.Salt : null;
            (string hashedPassword, string salt) = !string.IsNullOrWhiteSpace(txtPassword.Text) ?_autenticationManager.HashPasswordAndGetSalt(txtPassword.Text) : (password, oldSalt);  

            if (!string.IsNullOrEmpty(_selectedImagePath))
            {
                newImage = _selectedImagePath;
            } else
            {
                string imageName = gender == "Ženski" ? "female-user-white.png" : "male-user-white.png";
                //string projectPath = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\";
                newImage = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\" + imageName;
            }

            _updatedUser = new User()
            {
                Id = _user.Id,
                ProfileImage = BitmapImageConverter.ConvertBitmapImageToByteArray(newImage),
                PIN = PIN,
                FirstName = firstname,
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

            var isUpdated  = await Task.Run(() => _userServices.isUpdated(_updatedUser));

            if (isUpdated)
            {
                // refresh dgv in backgorund when upload
                _ucEmployeeAdministrating.RefreshGUI();

                var result = MessageBox.Show($"Korisnik {firstname} {lastName} je uspješno ažuriran! Želite li obavijestiti ažuriranog korisnika putem e-pošte?", "Obavijest", MessageBoxButton.YesNo);

                _user = _updatedUser;

                if (result == MessageBoxResult.Yes)
                {
                    var subject = "Vaši podaci su uspješno ažurirani!";
                    var emailNotifier = new UserUpdateEmailNotifier();
                    var isEmailSent = emailNotifier.SendUploadEmail(PIN, firstname, lastName, date, gender, email, telephone, username, password, subject);
                    if (!isEmailSent)
                    {
                        MessageBox.Show("Došlo je do pogreške prilikom slanja e-pošte!\nMolimo vas provjerite je li unesena ispravna adresa e-pošte.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                BackToProfile();
            } else
            {
                MessageBox.Show("Došlo je do pogreške!");
            }

        }

        //validation
        private bool ValidateRegistration()
        {
            var PIN = txtPIN.Text;
            var telephone = txtTelephone.Text;

            if (!string.IsNullOrWhiteSpace(PIN))
            {
                if (PIN.Length < 11 || PIN.Length > 11)
                {
                    MessageBox.Show("OIB mora imati 11 znamenki.");
                    txtPIN.Clear();
                    return false;
                }
            }
            
            if (!string.IsNullOrWhiteSpace(telephone))
            {
                if (telephone.Length < 10 || telephone.Length > 14)
                {
                    MessageBox.Show("Molimo unesite valjani broj telefona.");
                    txtTelephone.Clear();
                    return false;
                }
            }   

            return true;
        }

        //Provjere
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

        //back to profile
        private void btnBackToProfile_Click(object sender, RoutedEventArgs e)
        {
            BackToProfile();
        }

        private void BackToProfile()
        {
            var ucProfileSidebar = new ucEmployeeProfileSidebar(_user, _ucEmployeeAdministrating);
            ucProfileSidebar.refreshData();
            _ucEmployeeAdministrating.contentSidebarProfile.Content = ucProfileSidebar;
        }
    }
}
