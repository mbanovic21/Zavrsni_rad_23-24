using EntityLayer.Entities;
using EntityLayer;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreschoolManagmentSoftware.UserControls
{
    /// <summary>
    /// Interaction logic for ucEmployeeProfileSidebar.xaml
    /// </summary>
    public partial class ucEmployeeProfileSidebar : UserControl
    {
        private User _user { get; set; }
        private ucEmployeeAdministrating _ucEmployeeAdministrating { get; set; }

        public ucEmployeeProfileSidebar(User user, ucEmployeeAdministrating ucEmployeeAdministrating)
        {
            InitializeComponent();
            _user = user;
            _ucEmployeeAdministrating = ucEmployeeAdministrating;
        }

        private void ucEmplyeeSidebarProfile_Loaded(object sender, RoutedEventArgs e)
        {
            refreshData();
        }

        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            var ucEmployeeEditProfile = new ucEmployeeEditProfile(_user, _ucEmployeeAdministrating);
            _ucEmployeeAdministrating.contentSidebarProfile.Content = ucEmployeeEditProfile;
        }

        public void refreshData()
        {
            var profileImage = BitmapImageConverter.ConvertByteArrayToBitmapImage(_user.ProfileImage);
            var email = _user.Email;
            var atIndex = email.IndexOf('@');
            var emailUsername = email.Substring(0, atIndex);
            var emailDomain = email.Substring(atIndex);

            imgProfile.Source = profileImage;
            textFirstAndLastName.Text = $"{_user.FirstName} {_user.LastName}";

            textUsername.Text = _user.Username;
            textPIN.Text = _user.PIN;
            textEmail.Text = $"{emailUsername}\n{emailDomain}";
            textTelephone.Text = _user.Telephone;
            textDateOfBirth.Text = _user.DateOfBirth;
            textGender.Text = _user.Sex;
            textRole.Text = _user.Id_role == 1 ? "Administrator" : "Običan";
        }
    }
}
