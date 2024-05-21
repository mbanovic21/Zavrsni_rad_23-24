using BusinessLogicLayer.DBServices;
using EntityLayer;
using EntityLayer.Entities;
using PreschoolManagmentSoftware.UserControls.ChildrenAdministrating;
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
    /// Interaction logic for ucChildProfileSidebar.xaml
    /// </summary>
    public partial class ucChildProfileSidebar : UserControl
    {
        private Child _child { get; set; }
        private ucChildrenAdministrating _ucChildrenAdministrating{ get; set; }
        private ChildServices _childServices = new ChildServices();
        public ucChildProfileSidebar(Child child, ucChildrenAdministrating ucChildrenAdministrating)
        {
            InitializeComponent();
            _child = child;
            _ucChildrenAdministrating = ucChildrenAdministrating;
        }

        private void ucChildSIdebarProfile_Loaded(object sender, RoutedEventArgs e)
        {
            refreshData();
        }

        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            var ucChildeEditProfile = new ucChildEditProfileSidebar(_child, _ucChildrenAdministrating);
            _ucChildrenAdministrating.contentSidebarProfile.Content = ucChildeEditProfile;
        }

        private async void btnDeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Jeste li sigurni da želite iz sustava obrisati dijete '{_child.FirstName} {_child.LastName}'?", "Obavijest", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var isRemoved = await Task.Run(() => _childServices.RemoveChild(_child.Id));

                if (isRemoved)
                {
                    _ucChildrenAdministrating.HideSidebarProfile();
                    _ucChildrenAdministrating.RefreshGUI();
                    MessageBox.Show("Dijete je uspješno obrisano iz sustava!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                } else
                {
                    MessageBox.Show("Došlo je do greške prilikom brisanja djeteta iz sustava.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void refreshData()
        {
            var profileImage = BitmapImageConverter.ConvertByteArrayToBitmapImage(_child.ProfileImage);

            imgProfile.Source = profileImage;
            textFirstAndLastName.Text = $"{_child.FirstName} {_child.LastName}";

            textPIN.Text = _child.PIN;
            textDateOfBirth.Text = _child.DateOfBirth;
            textAddress.Text = _child.Adress;
            textBirthPlace.Text = _child.BirthPlace;
            textNationality.Text = _child.Nationality;
            textGender.Text = _child.Sex;
            textDevelopmentStatus.Text = _child.DevelopmentStatus;
            textMedicalInformation.Text = _child.MedicalInformation;
        }
    }
}
