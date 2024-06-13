using BusinessLogicLayer;
using BusinessLogicLayer.DBServices;
using BusinessLogicLayer.EmailServices;
using EntityLayer;
using EntityLayer.Entities;
using Microsoft.Win32;
using PreschoolManagmentSoftware.UserControls.ParentAdministrating;
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
    /// Interaction logic for ucChildRegistrationSidebar.xaml
    /// </summary>
    public partial class ucChildRegistrationSidebar : UserControl
    {
        private AutenticationManager _autenticationManager = new AutenticationManager();
        private ChildServices _childServices = new ChildServices();
        private ParentServices _parentServices = new ParentServices();
        private GroupServices _groupServices = new GroupServices();
        private ucChildrenAdministrating _ucChildrenAdministrating { get; set; }
        private string _selectedImagePath { get; set; }
        public ucChildRegistrationSidebar(ucChildrenAdministrating ucChildrenAdministrating)
        {
            InitializeComponent();
            _ucChildrenAdministrating = ucChildrenAdministrating;
        }

        //Profile image
        private void ucChildRegistration_Loaded(object sender, RoutedEventArgs e)
        {
            SetInitialProfileImage();
            FillComboBoxses();
        }

        public async void FillComboBoxses()
        {
            var mothers = await Task.Run(() => _parentServices.GetMothers());
            var fathers = await Task.Run(() => _parentServices.GetFathers());
            var groups = await Task.Run(() => _groupServices.GetAllGroups());

            cmbSearchMother.Items.Clear();
            cmbSearchFather.Items.Clear();
            cmbSearchGroup.Items.Clear();

            foreach(var mother in mothers)
            {
                cmbSearchMother.Items.Add(mother.ToString());
            }

            foreach (var father in fathers)
            {
                cmbSearchFather.Items.Add(father.ToString());
            }

            foreach (var group in groups)
            {
                cmbSearchGroup.Items.Add(group.ToString());
            }
        }

        private void SetInitialProfileImage()
        {
            var gender = GetSelectedGender();
            string imageName = gender == "Ženski" ? "girl.png" : "boy.png";
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

        //add new mother
        private void btnAddMother_Click(object sender, RoutedEventArgs e)
        {
            var motherControl = new ucParentRegistration(this, _ucChildrenAdministrating);
            _ucChildrenAdministrating.contentSidebarRegistration.Content = motherControl;
        }

        //add new father
        private void btnAddFather_Click(object sender, RoutedEventArgs e)
        {
            var fatherControl = new ucParentRegistration(this, _ucChildrenAdministrating);
            fatherControl.SetManFirst();
            _ucChildrenAdministrating.contentSidebarRegistration.Content = fatherControl;
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

        //Address
        private void textAddress_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtAddress.Focus();
        }

        private void txtAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            var address = txtAddress.Text;
            var placeholderAddresse = textAddress;

            if (!string.IsNullOrEmpty(address) && address.Length >= 0)
            {
                placeholderAddresse.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderAddresse.Visibility = Visibility.Visible;
            }
        }

        //BirthPlace
        private void textBirthPlace_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtBirthPlace.Focus();
        }

        private void txtBirthPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            var birthPlace = txtBirthPlace.Text;
            var placeholderBirthPlace = textBirthPlace;

            if (!string.IsNullOrEmpty(birthPlace) && birthPlace.Length >= 0)
            {
                placeholderBirthPlace.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderBirthPlace.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(birthPlace))
            {
                if (string.IsNullOrEmpty(birthPlace)) return;
                txtBirthPlace.Clear();
                MessageBox.Show("Mjesto rođenja može sadržavati samo slova.");
                return;
            }
        }

        //Nationality
        private void textNationality_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNationality.Focus();
        }

        private void txtNationality_TextChanged(object sender, TextChangedEventArgs e)
        {
            var nationality = txtNationality.Text;
            var placeholderNationality = textNationality;

            if (!string.IsNullOrEmpty(nationality) && nationality.Length >= 0)
            {
                placeholderNationality.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderNationality.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(nationality))
            {
                if (string.IsNullOrEmpty(nationality)) return;
                txtBirthPlace.Clear();
                MessageBox.Show("Nacionalnost može sadržavati samo slova.");
                return;
            }
        }

        //DevelopmentStatus
        private void textDevelopmentStatus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtDevelopmentStatus.Focus();
        }

        private void txtDevelopmentStatus_TextChanged(object sender, TextChangedEventArgs e)
        {
            var developmentStatus = txtDevelopmentStatus.Text;
            var placeholderDevelopmentStatus = textDevelopmentStatus;

            if (!string.IsNullOrEmpty(developmentStatus) && developmentStatus.Length >= 0)
            {
                placeholderDevelopmentStatus.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderDevelopmentStatus.Visibility = Visibility.Visible;
            }
        }

        //Medical information
        private void textMedicalInformation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtMedicalInformation.Focus();
        }

        private void txtMedicalInformation_TextChanged(object sender, TextChangedEventArgs e)
        {
            var medicalInformation = txtMedicalInformation.Text;
            var placeholderMedicalInformation = textMedicalInformation;

            if (!string.IsNullOrEmpty(medicalInformation) && medicalInformation.Length >= 0)
            {
                placeholderMedicalInformation.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderMedicalInformation.Visibility = Visibility.Visible;
            }
        }


        //btnRegister
        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRegistration())
            {
                return;
            }
            var fathersID = int.Parse(cmbSearchFather.SelectedItem.ToString().Split(' ')[0]);
            var mothersID = int.Parse(cmbSearchMother.SelectedItem.ToString().Split(' ')[0]);
            var groupsID = int.Parse(cmbSearchGroup.SelectedItem.ToString().Split(' ')[0]);

            string imagePathForRegistration;
            var PIN = txtPIN.Text;
            var firstName = txtFirstname.Text;
            var lastName = txtLastname.Text;
            var date = dpDateOfBirth.Text;
            var gender = GetSelectedGender();
            var address = txtAddress.Text;
            var birthPlace = txtBirthPlace.Text;
            var nationality = txtNationality.Text;
            var developmentStatus = txtDevelopmentStatus.Text;
            var medicalInformation = txtMedicalInformation.Text;
            var father = await Task.Run(() => _parentServices.GetParentByID(fathersID));
            var mother = await Task.Run(() => _parentServices.GetParentByID(mothersID));
            var parents = new List<Parent>
            {
                mother,
                father
            };

            Console.WriteLine(parents.Count);

            if (!string.IsNullOrEmpty(_selectedImagePath))
            {
                imagePathForRegistration = _selectedImagePath;
            } else
            {
                string imageName = gender == "Ženski" ? "girl.png" : "boy.png";
                //string projectPath = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\";
                imagePathForRegistration = "C:\\Users\\Banek\\Desktop\\FOI\\6. semestar\\Moje\\Zavrsni rad\\Zavrsni_rad_23-24\\Software\\PreschoolManagmentSoftware\\Media\\Images\\" + imageName;
            }

            var child = new Child()
            {
                ProfileImage = BitmapImageConverter.ConvertBitmapImageToByteArray(imagePathForRegistration),
                PIN = PIN,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = date,
                Sex = gender,
                Adress = address,
                BirthPlace = birthPlace,
                Nationality = nationality,
                DevelopmentStatus = developmentStatus,
                MedicalInformation = medicalInformation,
                Id_Group = groupsID,
            };

            var isAdded = await Task.Run(() => _childServices.RegistrateChild(child, parents));

            if (isAdded)
            {
                var caughtChild = await Task.Run(() => _childServices.GetChildByPIN(PIN));

                _ucChildrenAdministrating.RefreshGUI();

                var result = MessageBox.Show("Dijete je uspješno registrirano u sustav! Želite li obavijestiti roditelje putem e-pošte?", "Obavijest", MessageBoxButton.YesNo);

                ClearFields();

                if (result == MessageBoxResult.Yes)
                {
                    // Obavijesti korisnika putem e-pošte
                    var subject = "Poruka potvrde za uspješan upis djeteta u vrtić!";
                    var emailNotifier = new ChildRegistrationEmailNotifier();
                    foreach (var parent in parents)
                    {
                        var isEmailSent = emailNotifier.SendRegistrationEmail(subject, parent, child);
                        if (!isEmailSent)
                        {
                            var isRemoved = await Task.Run(() => _childServices.RemoveChild(caughtChild.Id));
                            //var isParentRemoved = await Task.Run(() => _parentServices.RemoveParents(_parents));
                            if (isRemoved)
                            {
                                MessageBox.Show("Došlo je do pogreške prilikom slanja e-pošte!\nMolimo vas provjerite je li unesena ispravna adresa e-pošte.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            } else
            {
                MessageBox.Show("Pogreška kod registracije roditelja");
            }
        }

        private void ClearFields()
        {
            _selectedImagePath = null;
            txtPIN.Clear();
            txtFirstname.Clear();
            txtLastname.Clear();
            dpDateOfBirth.SelectedDate = null;
            rbFemale.IsChecked = true;
            txtAddress.Clear();
            txtNationality.Clear();
            txtBirthPlace.Clear();
            txtDevelopmentStatus.Clear();
            txtMedicalInformation.Clear();
        }

        //Input validation
        private bool ValidateRegistration()
        {
            var PIN = txtPIN.Text;
            var firstName = txtFirstname.Text;
            var lastName = txtLastname.Text;
            var date = dpDateOfBirth.Text;

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

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbSearchMother.IsDropDownOpen = true;
        }

        private void btnDropdown2_Click(object sender, RoutedEventArgs e)
        {
            cmbSearchFather.IsDropDownOpen = true;
        }

        private void btnDropdown3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
