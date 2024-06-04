using BusinessLogicLayer.DBServices;
using EntityLayer;
using EntityLayer.Entities;
using PreschoolManagmentSoftware.Static_Classes;
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

namespace PreschoolManagmentSoftware.UserControls.ParentAdministrating
{
    /// <summary>
    /// Interaction logic for ucParentProfileSidebar.xaml
    /// </summary>
    public partial class ucParentProfileSidebar : UserControl
    {
        private ParentServices _parentServices = new ParentServices();
        private ChildServices _childServices = new ChildServices();
        private ucChildEditProfileSidebar _ucChildEditProfileSidebar { get; set; }
        private ucChildrenAdministrating _ucChildrenAdministrating { get; set; }
        private Parent _parent { get; set; }
        public ucParentProfileSidebar(ucChildEditProfileSidebar ucChildEditProfileSidebar, ucChildrenAdministrating ucChildrenAdministrating, Parent parent)
        {
            InitializeComponent();
            _parent = parent;
            _ucChildrenAdministrating = ucChildrenAdministrating;
            _ucChildEditProfileSidebar = ucChildEditProfileSidebar;
        }

        private void ucParentSidebarProfile_Loaded(object sender, RoutedEventArgs e)
        {
            refreshData();
        }


        private void btnBackToProfile_Click(object sender, RoutedEventArgs e)
        {
            BackToProfile();
        }

        public async void refreshData()
        {
            var profileImage = BitmapImageConverter.ConvertByteArrayToBitmapImage(_parent.ProfileImage);

            imgProfile.Source = profileImage;
            textFirstAndLastName.Text = $"{_parent.FirstName} {_parent.LastName}";
            var children = await Task.Run(() => _childServices.GetChildrenByParent(_parent));

            textPIN.Text = _parent.PIN;
            textDateOfBirth.Text = _parent.DateOfBirth;
            textEmail.Text = _parent.Email;
            textGender.Text = _parent.Sex;
            textChildren.Text = GetChildrensNames(children);
        }

        public string GetChildrensNames(List<Child> childrens)
        {
            if (childrens == null || childrens.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder childrensBuilder = new StringBuilder();

            foreach (var child in childrens)
            {
                childrensBuilder.Append($"{child.FirstName} {child.LastName}, ");
            }

            // Ukloni zadnji zarez i razmak
            if (childrensBuilder.Length > 0)
            {
                childrensBuilder.Length -= 2; // ukloni zadnja dva znaka ", "
            }

            return childrensBuilder.ToString();
        }

        private void BackToProfile()
        {
            _ucChildrenAdministrating.contentSidebarProfile.Content = _ucChildEditProfileSidebar;
        }

        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            var ucEditProfile = new ucParentEditProfileSidebar(this, _ucChildrenAdministrating, _ucChildEditProfileSidebar, _parent);
            _ucChildrenAdministrating.contentSidebarProfile.Content = ucEditProfile;
        }

        private async void btnGeneratePDF_Click(object sender, RoutedEventArgs e)
        {
            var children = await Task.Run(() => _childServices.GetChildrenByParent(_parent));
            await Task.Run(() => PDFConverter.GenerateAndOpenParentReport(_parent, GetParentsNames(children)));
        }

        public string GetParentsNames(List<Child> children)
        {
            if (children == null || children.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder parentsBuilder = new StringBuilder();

            foreach (var parent in children)
            {
                parentsBuilder.Append($"{parent.FirstName} {parent.LastName}, ");
            }

            // Ukloni zadnji zarez i razmak
            if (parentsBuilder.Length > 0)
            {
                parentsBuilder.Length -= 2; // ukloni zadnja dva znaka ", "
            }

            return parentsBuilder.ToString();
        }
    }
}
