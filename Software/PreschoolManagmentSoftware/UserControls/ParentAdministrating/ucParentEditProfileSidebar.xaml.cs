using BusinessLogicLayer.DBServices;
using EntityLayer;
using EntityLayer.Entities;
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

namespace PreschoolManagmentSoftware.UserControls.ParentAdministrating
{
    /// <summary>
    /// Interaction logic for ucParentEditProfileSidebar.xaml
    /// </summary>
    public partial class ucParentEditProfileSidebar : UserControl
    {
        private string _selectedImagePath { get; set; }

        private Parent _updatedParent { get; set; }
        private Parent _parent { get; set; }
        private ucParentProfileSidebar _ucParentProfileSidebar { get; set; }
        private ucChildrenAdministrating _ucChildrenAdministrating { get; set; }
        private ucChildEditProfileSidebar _ucChildEditProfileSidebar { get; set; }

        private ParentServices _parentServices = new ParentServices();
        public ucParentEditProfileSidebar(ucParentProfileSidebar ucParentProfileSidebar, ucChildrenAdministrating ucChildrenAdministrating, ucChildEditProfileSidebar ucChildEditProfileSidebar, Parent parent)
        {
            InitializeComponent();
            _parent = parent;
            _ucChildrenAdministrating = ucChildrenAdministrating;
            _ucParentProfileSidebar = ucParentProfileSidebar;
            _ucChildEditProfileSidebar = ucChildEditProfileSidebar;
        }

        private void ucEditParentProfile_Loaded(object sender, RoutedEventArgs e)
        {
            var _selectedImagePath = BitmapImageConverter.ConvertByteArrayToBitmapImage(_parent.ProfileImage);

            textFirstname.Text = _parent.FirstName;
            textLastname.Text = _parent.LastName;
            textPIN.Text = _parent.PIN;
            dpDateOfBirth.SelectedDate = DateTime.Parse(_parent.DateOfBirth);
            textEmail.Text = _parent.Email;

            string UserGenderWithoutSpaces = _parent.Sex;
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

            imgProfile.Source = _selectedImagePath;

            _updatedParent = _parent;
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
            string imageName = gender == "Ženski" ? "woman-parent.png" : "man-parent.png";
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
                placeholderFirstName.Text = _parent.FirstName;
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
                placeholderLastName.Text = _parent.LastName;
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
                placeholderPIN.Text = _parent.PIN;
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
                placeholderEmail.Visibility = Visibility.Visible;
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

        //Save
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRegistration())
            {
                return;
            }

            string newImage;
            var firstname = string.IsNullOrWhiteSpace(txtFirstname.Text) ? _parent.FirstName : txtFirstname.Text;
            string lastName = string.IsNullOrWhiteSpace(txtLastname.Text) ? _parent.LastName : txtLastname.Text;
            string PIN = string.IsNullOrWhiteSpace(txtPIN.Text) ? _parent.PIN : txtPIN.Text;
            string date = dpDateOfBirth.Text ?? _parent.DateOfBirth;
            string email = string.IsNullOrWhiteSpace(txtEmail.Text) ? _parent.Email : txtEmail.Text;
            string gender = GetSelectedGender();

            if (!string.IsNullOrEmpty(_selectedImagePath))
            {
                newImage = _selectedImagePath;
            } else
            {
                string imageName = gender == "Ženski" ? "woman-parent.png" : "man-parent.png";
                //string projectPath = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\";
                newImage = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\" + imageName;
            }

            _updatedParent = new Parent()
            {
                Id = _parent.Id,
                ProfileImage = BitmapImageConverter.ConvertBitmapImageToByteArray(newImage),
                PIN = PIN,
                FirstName = firstname,
                LastName = lastName,
                DateOfBirth = date,
                Email = email,
                Sex = gender,
            };

            var isUpdated = await Task.Run(() => _parentServices.isUpdated(_updatedParent));

            if (isUpdated)
            {
                MessageBox.Show($"Podaci roditelja {firstname} {lastName} su uspješno ažurirani", "Obavijest", MessageBoxButton.OK);

                _parent = _updatedParent;

                BackToProfile();
            } else
            {
                MessageBox.Show("Došlo je do pogreške!");
            }
        }

        //Validation
        private bool ValidateRegistration()
        {
            var PIN = txtPIN.Text;

            if (!string.IsNullOrWhiteSpace(PIN))
            {
                if (PIN.Length < 11 || PIN.Length > 11)
                {
                    MessageBox.Show("OIB mora imati 11 znamenki.");
                    txtPIN.Clear();
                    return false;
                }
            }

            if (!IsValidEmail(txtEmail.Text) && !string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Email nije validan!");
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

        //back to profile
        private void BtnBackToProfile_Click(object sender, RoutedEventArgs e)
        {
            BackToProfile();
        }

        private void BackToProfile()
        {
            var ucProfileSidebar = new ucParentProfileSidebar(_ucChildEditProfileSidebar, _ucChildrenAdministrating, _parent);
            ucProfileSidebar.refreshData();
            _ucChildrenAdministrating.contentSidebarProfile.Content = ucProfileSidebar;
        }
    }
}
