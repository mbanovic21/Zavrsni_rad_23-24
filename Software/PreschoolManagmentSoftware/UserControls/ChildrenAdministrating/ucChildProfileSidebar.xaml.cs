using BusinessLogicLayer;
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
        private ParentServices _parentServices = new ParentServices();
        private GroupServices _groupServices = new GroupServices();
        private NoteServices _noteServices = new NoteServices();
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
                var message = MessageBox.Show($"Želite li iz sustava obrisati i roditelje za dijete '{_child.FirstName} {_child.LastName}'?", "Obavijest", MessageBoxButton.YesNo);

                if (message == MessageBoxResult.Yes)
                {
                    var isParentRemoved = await Task.Run(() => _parentServices.RemoveParentsByChild(_child));
                    var isChildRemoved = await Task.Run(() => _childServices.RemoveChild(_child.Id));
                    if (isParentRemoved && isChildRemoved)
                    {
                        _ucChildrenAdministrating.HideSidebarProfile();
                        _ucChildrenAdministrating.RefreshGUI();
                        MessageBox.Show("Dijete i njegovi roditelji su uspješno izbrisani iz sustava!", "Obavijest", MessageBoxButton.OK, MessageBoxImage.Information);
                    } else
                    {
                        MessageBox.Show("Došlo je do greške prilikom brisanja roditelja ili dijeteta iz sustava.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                } else
                {
                    var isChildRemoved = await Task.Run(() => _childServices.RemoveChild(_child.Id));
                    if (isChildRemoved)
                    {
                        _ucChildrenAdministrating.HideSidebarProfile();
                        _ucChildrenAdministrating.RefreshGUI();
                        MessageBox.Show("Dijete je uspješno obrisano iz sustava!", "Obavijest", MessageBoxButton.OK, MessageBoxImage.Information);
                    } else
                    {
                        MessageBox.Show("Došlo je do greške prilikom brisanja djeteta iz sustava.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void btnGeneratePDF_Click(object sender, RoutedEventArgs e)
        {
            var parents = await Task.Run(() => _parentServices.GetParentsByChild(_child));
            var group = await Task.Run(() => _groupServices.GetGroupById(_child.Id_Group));
            var notes = await Task.Run(() => _noteServices.GetNotesByChild(_child));
            await Task.Run(() => PDFConverter.GenerateAndOpenChildReport(_child, GetParentsNames(parents), GetGroupName(group), notes));
        }

        public async Task refreshData()
        {
            var profileImage = BitmapImageConverter.ConvertByteArrayToBitmapImage(_child.ProfileImage);
            var group = await Task.Run(() => _groupServices.GetGroupById(_child.Id_Group));
            var parents = await Task.Run(() => _parentServices.GetParentsByChild(_child));

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
            textGroup.Text = GetGroupName(group);
            textParents.Text = GetParentsNames(parents);
        }

        private string GetGroupName(Group group)
        {
            return group.ToString().Split(' ')[1];
        }

        public string GetParentsNames(List<Parent> parents)
        {
            if (parents == null || parents.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder parentsBuilder = new StringBuilder();

            foreach (var parent in parents)
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
