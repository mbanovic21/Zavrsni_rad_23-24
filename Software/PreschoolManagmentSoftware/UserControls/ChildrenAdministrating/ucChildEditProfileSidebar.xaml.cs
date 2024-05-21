using BusinessLogicLayer.DBServices;
using BusinessLogicLayer.EmailServices;
using EntityLayer;
using EntityLayer.Entities;
using Microsoft.Win32;
using SecurityLayer;
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

namespace PreschoolManagmentSoftware.UserControls.ChildrenAdministrating
{
    /// <summary>
    /// Interaction logic for ucChildEditProfileSidebar.xaml
    /// </summary>
    public partial class ucChildEditProfileSidebar : UserControl
    {
        private string _selectedImagePath { get; set; }
        private Child _child { get; set; }
        private Child _updatedChild { get; set; }
        private ucChildrenAdministrating _ucChildrenAdministrating { get; set; }
        private ChildServices _childServices = new ChildServices();
        public ucChildEditProfileSidebar(Child child, ucChildrenAdministrating ucChildrenAdministrating)
        {
            InitializeComponent();
            _child = child;
            _ucChildrenAdministrating = ucChildrenAdministrating;
        }

        //OnInit
        private void ucEditChildProfile_Loaded(object sender, RoutedEventArgs e)
        {
            var _selectedImagePath = BitmapImageConverter.ConvertByteArrayToBitmapImage(_child.ProfileImage);

            textFirstname.Text = _child.FirstName.Trim().Replace(" ", "");
            textLastname.Text = _child.LastName.Trim().Replace(" ", "");
            textPIN.Text = _child.PIN.Trim().Replace(" ", "");
            dpDateOfBirth.SelectedDate = DateTime.Parse(_child.DateOfBirth);
            textAddress.Text = _child.Adress.Trim().Replace(" ", "");
            textBirthPlace.Text = _child.BirthPlace.Trim().Replace(" ", "");
            textNationality.Text = _child.Nationality.Trim().Replace(" ", "");

            string UserGenderWithoutSpaces = _child.Sex.Trim().Replace(" ", "");
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

            textDevelopmentStatus.Text = _child.DevelopmentStatus.Trim().Replace(" ", "");
            textMedicalInformations.Text = _child.MedicalInformation.Trim().Replace(" ", ""); 

            imgProfile.Source = _selectedImagePath;
            _updatedChild = _child;
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
            string imageName = gender == "Ženski" ? "girl.png" : "boy.png";
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
                placeholderFirstName.Text = _child.FirstName;
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
                placeholderLastName.Text = _child.LastName;
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
                placeholderPIN.Text = _child.PIN;
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

        //Address

        private void textAddress_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtAddress.Focus();
        }

        private void txtAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            var address = txtAddress.Text;
            var placeholderAddress = textAddress;

            if (!string.IsNullOrEmpty(address))
            {
                placeholderAddress.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderAddress.Text = _child.Adress;
                placeholderAddress.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(address))
            {
                if (string.IsNullOrEmpty(address)) return;
                txtLastname.Clear();
                MessageBox.Show("Adresa može sadržavati slova, razmake i brojeve.");
                return;
            }
        }

        //BirthPlace
        private void textBirthPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtBirthPlace.Focus();
        }

        private void txtBirthPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            var birthPlace = txtBirthPlace.Text;
            var placeholderBrthPlace = textBirthPlace;

            if (!string.IsNullOrEmpty(birthPlace))
            {
                placeholderBrthPlace.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderBrthPlace.Text = _child.Adress;
                placeholderBrthPlace.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(birthPlace))
            {
                if (string.IsNullOrEmpty(birthPlace)) return;
                txtLastname.Clear();
                MessageBox.Show("Mjesto rođenja može sadržavati samo slova.");
                return;
            }
        }


        //Nationality
        private void textNationality_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtNationality.Focus();
        }

        private void txtNationality_TextChanged(object sender, TextChangedEventArgs e)
        {
            var nationality = txtNationality.Text;
            var placeholderNationality = textNationality;

            if (!string.IsNullOrEmpty(nationality))
            {
                placeholderNationality.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderNationality.Text = _child.Adress;
                placeholderNationality.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(nationality))
            {
                if (string.IsNullOrEmpty(nationality)) return;
                txtLastname.Clear();
                MessageBox.Show("Nacionalnost može sadržavati samo slova.");
                return;
            }
        }

        //Development status
        private void textDevelopmentStatus_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtDevelopmentStatus.Focus();
        }

        private void txtDevelopmentStatus_TextChanged(object sender, TextChangedEventArgs e)
        {
            var developmentStatus = txtDevelopmentStatus.Text;
            var placeholderDevelopmentStatus = textDevelopmentStatus;

            if (!string.IsNullOrEmpty(developmentStatus))
            {
                placeholderDevelopmentStatus.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderDevelopmentStatus.Text = _child.Adress;
                placeholderDevelopmentStatus.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(developmentStatus))
            {
                if (string.IsNullOrEmpty(developmentStatus)) return;
                txtLastname.Clear();
                MessageBox.Show("Status razvoja može sadržavati samo slova.");
                return;
            }
        }

        //Medical informations
        private void textMedicalInformations_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtMedicalInformations.Focus();
        }

        private void txtMedicalInformations_TextChanged(object sender, TextChangedEventArgs e)
        {
            var medicalInformations = txtMedicalInformations.Text;
            var placeholderMedicalInformations = textMedicalInformations;

            if (!string.IsNullOrEmpty(medicalInformations))
            {
                placeholderMedicalInformations.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderMedicalInformations.Text = _child.Adress;
                placeholderMedicalInformations.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(medicalInformations))
            {
                if (string.IsNullOrEmpty(medicalInformations)) return;
                txtLastname.Clear();
                MessageBox.Show("Medicinske informacije mogu sadržavati samo slova i razmake.");
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

        //Save
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRegistration())
            {
                return;
            }

            string newImage;
            var firstname = string.IsNullOrWhiteSpace(txtFirstname.Text) ? _child.FirstName : txtFirstname.Text;
            string lastName = string.IsNullOrWhiteSpace(txtLastname.Text) ? _child.LastName : txtLastname.Text;
            string PIN = string.IsNullOrWhiteSpace(txtPIN.Text) ? _child.PIN : txtPIN.Text;
            string date = dpDateOfBirth.Text ?? _child.DateOfBirth;
            string address = string.IsNullOrWhiteSpace(txtAddress.Text) ? _child.Adress : txtAddress.Text;
            string birthPlace = string.IsNullOrWhiteSpace(txtBirthPlace.Text) ? _child.BirthPlace : txtBirthPlace.Text;
            string nationality = string.IsNullOrWhiteSpace(txtNationality.Text) ? _child.Nationality : txtNationality.Text;
            string gender = GetSelectedGender();
            string developmentStatus = string.IsNullOrWhiteSpace(txtDevelopmentStatus.Text) ? _child.DevelopmentStatus : txtDevelopmentStatus.Text;
            string medicalInformations = string.IsNullOrWhiteSpace(txtMedicalInformations.Text) ? _child.MedicalInformation : txtMedicalInformations.Text;

            if (!string.IsNullOrEmpty(_selectedImagePath))
            {
                newImage = _selectedImagePath;
            } else
            {
                string imageName = gender == "Ženski" ? "girl.png" : "boy.png";
                //string projectPath = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\";
                newImage = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\" + imageName;
            }

            _updatedChild = new Child()
            {
                Id = _child.Id,
                ProfileImage = BitmapImageConverter.ConvertBitmapImageToByteArray(newImage),
                PIN = PIN,
                FirstName = firstname,
                LastName = lastName,
                DateOfBirth = date,
                Adress = address,
                BirthPlace = birthPlace,
                Nationality = nationality,
                Sex = gender,
                DevelopmentStatus = developmentStatus,
                MedicalInformation = medicalInformations,
                Id_Group = null
            };

            var isUpdated = await Task.Run(() => _childServices.isUpdated(_updatedChild));

            if (isUpdated)
            {
                // refresh dgv in backgorund when upload
                _ucChildrenAdministrating.RefreshGUI();

                MessageBox.Show($"Podaci djeteta {firstname} {lastName} su uspješno ažurirani", "Obavijest", MessageBoxButton.OK);

                _child = _updatedChild;

                /*var result = MessageBox.Show($"Korisnik {firstname} {lastName} je uspješno ažuriran! Želite li obavijestiti ažuriranog korisnika putem e-pošte?", "Obavijest", MessageBoxButton.YesNo);

                _child = _updatedChild;

                if (result == MessageBoxResult.Yes)
                {
                    var subject = "Vaši podaci su uspješno ažurirani!";
                    var emailNotifier = new UserUpdateEmailNotifier();
                    var isEmailSent = emailNotifier.SendUploadEmail(PIN, firstname, lastName, date, gender, email, telephone, username, password, subject);
                    if (!isEmailSent)
                    {
                        MessageBox.Show("Došlo je do pogreške prilikom slanja e-pošte!\nMolimo vas provjerite je li unesena ispravna adresa e-pošte.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }*/
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

            if (!string.IsNullOrWhiteSpace(PIN))
            {
                if (PIN.Length < 11 || PIN.Length > 11)
                {
                    MessageBox.Show("OIB mora imati 11 znamenki.");
                    txtPIN.Clear();
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
        private void BtnBackToProfile_Click(object sender, RoutedEventArgs e)
        {
            BackToProfile();
        }

        private void BackToProfile()
        {
            var ucProfileSidebar = new ucChildProfileSidebar(_child, _ucChildrenAdministrating);
            ucProfileSidebar.refreshData();
            _ucChildrenAdministrating.contentSidebarProfile.Content = ucProfileSidebar;
        }
    }
}
