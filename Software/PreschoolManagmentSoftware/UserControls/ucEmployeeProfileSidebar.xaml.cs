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
        public User User { get; set; }
        public ucEmployeeProfileSidebar(User user)
        {
            InitializeComponent();
            User = user;
        }

        private void ucEmplyeeSidebarProfile_Loaded(object sender, RoutedEventArgs e)
        {
            var profileImage = BitmapImageConverter.ConvertByteArrayToBitmapImage(User.ProfileImage);
            var email = User.Email;
            var atIndex = email.IndexOf('@');
            var emailUsername = email.Substring(0, atIndex);
            var emailDomain = email.Substring(atIndex);

            imgProfile.Source = profileImage;
            textFirstAndLastName.Text = $"{User.FirstName} {User.LastName}";

            textUsername.Text = User.Username;
            textPIN.Text = User.PIN;
            textEmail.Text = $"{emailUsername}\n{emailDomain}";
            textTelephone.Text = User.Telephone;
            textDateOfBirth.Text = User.DateOfBirth;
            textGender.Text = User.Sex;
            textRole.Text = User.Id_role == 1 ? "Administrator" : "Običan";
        }
    }
}
