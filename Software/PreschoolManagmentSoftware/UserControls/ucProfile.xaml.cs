using BusinessLogicLayer.DBServices;
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

namespace PreschoolManagmentSoftware.UserControls
{
    /// <summary>
    /// Interaction logic for ucProfile.xaml
    /// </summary>
    public partial class ucProfile : UserControl
    {
        private UserServices UserServices = new UserServices();

        public ucProfile()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //kasnije će hapsit od logina
        }

        //Search
        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = txtSearch.Text;
            var placeholderSearch = textSearch;

            if (!string.IsNullOrEmpty(search) && search.Length >= 0)
            {
                placeholderSearch.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderSearch.Visibility = Visibility.Visible;
            }
        }

        //PIN
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
        }

        //FirstName
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
        }

        //LastName
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
        }

        //DateOfBirth
        private void txtDateOfBirth_TextChanged(object sender, TextChangedEventArgs e)
        {
            var dateOfBirth = txtDateOfBirth.Text;
            var placeholderDateOfBirth= textDateOfBirth;

            if (!string.IsNullOrEmpty(dateOfBirth) && dateOfBirth.Length >= 0)
            {
                placeholderDateOfBirth.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderDateOfBirth.Visibility = Visibility.Visible;
            }
        }

        //Gender
        private void txtGender_TextChanged(object sender, TextChangedEventArgs e)
        {
            var gender = txtGender.Text;
            var placeholderGender = textGender;

            if (!string.IsNullOrEmpty(gender) && gender.Length >= 0)
            {
                placeholderGender.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderGender.Visibility = Visibility.Visible;
            }
        }

        //Email
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
        }

        //Username
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
        }

        //Password
        private void txtPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            var password = txtPassword.Text;
            var placeholderPassword = textPassword;

            if (!string.IsNullOrEmpty(password) && password.Length > 0)
            {
                placeholderPassword.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderPassword.Visibility = Visibility.Visible;
            }
        }

        //Role
        private void txtRole_TextChanged(object sender, RoutedEventArgs e)
        {
            var role = txtRole.Text;
            var placeholderRole = textRole;

            if (!string.IsNullOrEmpty(role) && role.Length > 0)
            {
                placeholderRole.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderRole.Visibility = Visibility.Visible;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var search = txtSearch.Text;
            var user = UserServices.GetUserByUsername(search);

            //imgProfile.Source;
            txtPIN.Text = user.PIN;
            txtFirstname.Text = user.FirstName;
            txtLastname.Text = user.LastName;
            txtDateOfBirth.Text = user.DateOfBirth;
            txtGender.Text = user.Sex;
            txtEmail.Text = user.Email;
            txtTelephone.Text = user.Telephone;
            txtUsername.Text = user.Username;
            txtPassword.Text = user.Password;
            var role = user.Id_role == 1 ? "Administrator" : "Običan";
            txtRole.Text = role;   
        }
    }
}
