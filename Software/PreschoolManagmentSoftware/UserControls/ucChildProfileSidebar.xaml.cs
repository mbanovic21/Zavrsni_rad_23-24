using BusinessLogicLayer.DBServices;
using EntityLayer;
using EntityLayer.Entities;
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

        }

        private void btnDeleteProfile_Click(object sender, RoutedEventArgs e)
        {

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
