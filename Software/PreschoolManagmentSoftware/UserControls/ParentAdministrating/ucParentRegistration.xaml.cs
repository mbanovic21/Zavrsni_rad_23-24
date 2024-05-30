using EntityLayer.Entities;
using EntityLayer;
using Microsoft.Win32;
using PreschoolManagmentSoftware.UserControls.ChildrenAdministrating;
using System;
using System.Collections.Generic;
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
using Org.BouncyCastle.Utilities.Encoders;
using System.Net.NetworkInformation;

namespace PreschoolManagmentSoftware.UserControls.ParentAdministrating
{
    /// <summary>
    /// Interaction logic for ucParentRegistration.xaml
    /// </summary>
    public partial class ucParentRegistration : UserControl
    {
        private ucParentRegistration _ucParentRegistration { get; set; }
        private ucChildrenAdministrating _ucChildrenAdministrating { get; set; }
        private string _selectedImagePath { get; set; }
        public ucParentRegistration _previousControl { get; set; }
        public ucChildRegistrationSidebar _forwardControl { get; set; }
        public bool _isLeftArrowVisible { get; set; }
        public bool _isRightArrowVisible { get; set; }
        public List<Parent> _parents = new List<Parent>();
        public ucParentRegistration(List<Parent> parents, ucParentRegistration previousControl, ucChildRegistrationSidebar forwardControl, bool isLeftArrowVisible, bool isRightArrowVisible, ucParentRegistration ucParentRegistration, ucChildrenAdministrating ucChildrenAdministrating)
        {
            InitializeComponent();
            _ucParentRegistration = ucParentRegistration;
            _ucChildrenAdministrating = ucChildrenAdministrating;
            _parents = parents;
            _previousControl = previousControl;
            _forwardControl = forwardControl;
            _isLeftArrowVisible = isLeftArrowVisible;
            _isRightArrowVisible = isRightArrowVisible;
        }

        //leftArrow
        private void btnBackToFIrstParent_Click(object sender, RoutedEventArgs e)
        {
            _previousControl._isRightArrowVisible = true;
            _previousControl._previousControl = this;
            _ucChildrenAdministrating.contentSidebarRegistration.Content = _previousControl;
        }

        //rightArrow
        private void btnBackToSecondParent_Click(object sender, RoutedEventArgs e)
        {
            if(!_isLeftArrowVisible)
            {
                _ucChildrenAdministrating.contentSidebarRegistration.Content = _previousControl;
            } else if (_isLeftArrowVisible)
            {
                _ucChildrenAdministrating.contentSidebarRegistration.Content = _forwardControl;
            }      
        }

        //Profile image
        private void ucChildRegistration_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isLeftArrowVisible) 
            { 
                btnBackToFIrstParent.Visibility = Visibility.Visible;
            }
            
            if (_isRightArrowVisible || _forwardControl != null)
            {
                btnBackToSecondParent.Visibility = Visibility.Visible;
            }

            SetInitialProfileImage();
        }

        private void SetInitialProfileImage()
        {
            var gender = GetSelectedGender();
            string imageName = gender == "Ženski" ? "female-user-white.png" : "male-user-white.png";
            string imagePath = "pack://application:,,,/Media/Images/" + imageName;
            profileImage.Source = new BitmapImage(new Uri(imagePath));
        }

        private void btnDeleteImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetInitialProfileImage();
            btnDeleteImage.Visibility = Visibility.Collapsed;
            _selectedImagePath = null;
        }

        private void txtAddProfilePicture_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedImagePath = openFileDialog.FileName;
                profileImage.Source = new BitmapImage(new Uri(_selectedImagePath));
                btnDeleteImage.Visibility = Visibility.Visible;
            }
        }

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
                MessageBox.Show("OIB mora sadržavati samo znamenke.");
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
                if (string.IsNullOrEmpty(firstName)) return;
                txtLastname.Clear();
                MessageBox.Show("Ime može sadržavati samo slova.");
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
                MessageBox.Show("Prezime može sadržavati samo slova.");
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

        //btnRegister
        private void btnRegister_Click(object sender, RoutedEventArgs e)
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

            if (!string.IsNullOrEmpty(_selectedImagePath))
            {
                imagePathForRegistration = _selectedImagePath;
            } else
            {
                string imageName = gender == "Ženski" ? "female-user-white.png" : "male-user-white.png";
                //string projectPath = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\";
                imagePathForRegistration = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\" + imageName;
            }

            if(_parents.Count < 1)
            {
                var forwardedParent = new Parent()
                {
                    ProfileImage = BitmapImageConverter.ConvertBitmapImageToByteArray(imagePathForRegistration),
                    PIN = PIN,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = date,
                    Sex = gender,
                    Email = email,
                    Telephone = telephone
                };

                _parents.Add(forwardedParent);

                var ucParentRegistration2 = new ucParentRegistration(_parents, this, null, true, false, _ucParentRegistration, _ucChildrenAdministrating);
                _ucChildrenAdministrating.contentSidebarRegistration.Content = ucParentRegistration2;

                HideRegisterShowSave();
            } else
            {
                var nextParent = new Parent()
                {
                    ProfileImage = BitmapImageConverter.ConvertBitmapImageToByteArray(imagePathForRegistration),
                    PIN = PIN,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = date,
                    Sex = gender,
                    Email = email,
                    Telephone = telephone
                };

                _parents.Add(nextParent);

                var ucChildrenRegistration = new ucChildRegistrationSidebar(_ucChildrenAdministrating, this, _parents);
                _ucChildrenAdministrating.contentSidebarRegistration.Content = ucChildrenRegistration;

                HideRegisterShowSave();
            }

            foreach (var parent in _parents)
            {
                Console.WriteLine(parent.FirstName);
            }
        }

        //spremi promjene
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRegistration())
            {
                return;
            }

            if (!_isLeftArrowVisible)
            {
                string imagePathForRegistration;
                var PIN = txtPIN.Text;
                var firstName = txtFirstname.Text;
                var lastName = txtLastname.Text;
                var date = dpDateOfBirth.Text;
                var gender = GetSelectedGender();
                var email = txtEmail.Text;
                var telephone = txtTelephone.Text;

                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    imagePathForRegistration = _selectedImagePath;
                } else
                {
                    string imageName = gender == "Ženski" ? "female-user-white.png" : "male-user-white.png";
                    imagePathForRegistration = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\" + imageName;
                }

                var parent1 = _parents[0];

                parent1.ProfileImage = BitmapImageConverter.ConvertBitmapImageToByteArray(imagePathForRegistration);
                parent1.PIN = PIN;
                parent1.FirstName = firstName;
                parent1.LastName = lastName;
                parent1.DateOfBirth = date;
                parent1.Sex = gender;
                parent1.Email = email;
                parent1.Telephone = telephone;

                _parents[0] = parent1;

            } else
            {
                string imagePathForRegistration;
                var PIN = txtPIN.Text;
                var firstName = txtFirstname.Text;
                var lastName = txtLastname.Text;
                var date = dpDateOfBirth.Text;
                var gender = GetSelectedGender();
                var email = txtEmail.Text;
                var telephone = txtTelephone.Text;

                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    imagePathForRegistration = _selectedImagePath;
                } else
                {
                    string imageName = gender == "Ženski" ? "female-user-white.png" : "male-user-white.png";
                    imagePathForRegistration = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\" + imageName;
                }

                var parent2 = _parents[1];

                parent2.ProfileImage = BitmapImageConverter.ConvertBitmapImageToByteArray(imagePathForRegistration);
                parent2.PIN = PIN;
                parent2.FirstName = firstName;
                parent2.LastName = lastName;
                parent2.DateOfBirth = date;
                parent2.Sex = gender;
                parent2.Email = email;
                parent2.Telephone = telephone;

                _parents[1] = parent2;
            }

            // Ispis imena roditelja za provjeru
            foreach (var parent in _parents)
            {
                Console.WriteLine(parent.FirstName);
            }
        }

        private void LoadElements()
        {

        }

        //hide btnRegister Show save
        private void HideRegisterShowSave()
        {
            btnRegister.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Visible;
        }

        private void ClearFields()
        {
            _selectedImagePath = null;
            txtPIN.Clear();
            txtFirstname.Clear();
            txtLastname.Clear();
            dpDateOfBirth.SelectedDate = null;
            rbFemale.IsChecked = true;
            txtEmail.Clear();
            txtTelephone.Clear();
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

            if (string.IsNullOrWhiteSpace(PIN) || PIN.Length < 11 || PIN.Length > 11)
            {
                MessageBox.Show("Molimo unesite svoj OIB. Mora imati 11 znamenki.");
                txtPIN.Clear();
                return false;
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageBox.Show("Molimo unesite svoje ime.");
                txtFirstname.Clear();
                return false;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Molimo unesite svoje prezime.");
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
                MessageBox.Show("Molimo unesite svoju email adresu.");
                return false;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Molimo unesite valjanu email adresu.");
                txtEmail.Clear();
                return false;
            }

            if (string.IsNullOrWhiteSpace(telephone) || telephone.Length < 10 || telephone.Length > 14)
            {
                MessageBox.Show("Molimo unesite valjani broj telefona.");
                txtTelephone.Clear();
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
    }
}
