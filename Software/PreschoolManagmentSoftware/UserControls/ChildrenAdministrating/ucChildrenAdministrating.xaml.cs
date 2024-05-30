using BusinessLogicLayer.DBServices;
using EntityLayer.Entities;
using PreschoolManagmentSoftware.UserControls.ChildrenAdministrating;
using PreschoolManagmentSoftware.UserControls.ParentAdministrating;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreschoolManagmentSoftware.UserControls
{
    /// <summary>
    /// Interaction logic for ucChildrenAdministrating.xaml
    /// </summary>
    public partial class ucChildrenAdministrating : UserControl
    {
        private ChildServices _childServices = new ChildServices();
        public ucChildrenAdministrating()
        {
            InitializeComponent();
        }

        private void ucChildren_Loaded(object sender, RoutedEventArgs e)
        {
            //cmb fill
            cmbSearch.Items.Add("OIB");
            cmbSearch.Items.Add("Ime");
            cmbSearch.Items.Add("Prezime");
            cmbSearch.Items.Add("Ime i prezime");
            cmbSearch.Items.Add("Nacionalnost");
            cmbSearch.Items.Add("Status razvoja");
            cmbSearch.Items.Add("Medicinske informacije");
            cmbSearch.Items.Add("Mjesto rođenja");

            cmbSearch.SelectedIndex = 3;
            cmbSearch.IsDropDownOpen = false;
        }

        //dropdown
        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbSearch.IsDropDownOpen = true;
        }

        //search-selection
        private void cmbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbSearch.IsDropDownOpen = true;
            if (cmbSearch.SelectedIndex == 0)
            {
                textSearch.Text = "Pretraži po OIB-u";
            } else if (cmbSearch.SelectedIndex == 1)
            {
                textSearch.Text = "Pretraži po imenu";
            } else if (cmbSearch.SelectedIndex == 2)
            {
                textSearch.Text = "Pretraži po prezimenu";
            } else if (cmbSearch.SelectedIndex == 3)
            {
                textSearch.Text = "Pretraži po imenu i prezimenu";
            } else if (cmbSearch.SelectedIndex == 4)
            {
                textSearch.Text = "Pretraži po nacionalnosti";
            } else if (cmbSearch.SelectedIndex == 5)
            {
                textSearch.Text = "Pretraži po statusu razvoja";
            } else if (cmbSearch.SelectedIndex == 6)
            {
                textSearch.Text = "Pretraži po med. informacijama";
            } else if (cmbSearch.SelectedIndex == 7)
            {
                textSearch.Text = "Pretraži po mjestu rođenja";
            }
            UpdateData();
        }

        //Searchbar
        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var placeholderSearch = textSearch;
            var pattern = txtSearch.Text.ToLower();

            if (!string.IsNullOrEmpty(pattern) && pattern.Length >= 0)
            {
                placeholderSearch.Visibility = Visibility.Collapsed;
                UpdateData();
            } else
            {
                placeholderSearch.Visibility = Visibility.Visible;
                dgvChildren.ItemsSource = _childServices.GetAllChildren();
                HideColumns();
            }
        }

        private void UpdateData()
        {
            var search = txtSearch.Text.ToLower();
            var selectedItem = cmbSearch.SelectedIndex;

            if (string.IsNullOrEmpty(search))
            {
                dgvChildren.ItemsSource = _childServices.GetAllChildren();
                HideColumns();
                return;
            }

            switch (selectedItem)
            {
                case 0:
                    dgvChildren.ItemsSource = _childServices.GetChildByPINPattern(search);
                    HideColumns();
                    break;
                case 1:
                    dgvChildren.ItemsSource = _childServices.GetChildByFirstNamePattern(search);
                    HideColumns();
                    break;
                case 2:
                    dgvChildren.ItemsSource = _childServices.GetChildByLastNamePattern(search);
                    HideColumns();
                    break;
                case 3:
                    dgvChildren.ItemsSource = _childServices.GetChildByFirstNameAndLastNamePattern(search);
                    HideColumns();
                    break;
                case 4:
                    dgvChildren.ItemsSource = _childServices.GetChildByNationalityPattern(search);
                    HideColumns();
                    break;
                case 5:
                    dgvChildren.ItemsSource = _childServices.GetChildByDevelopmentStatusPattern(search);
                    HideColumns();
                    break;
                case 6:
                    dgvChildren.ItemsSource = _childServices.GetChildByMedicalInformationPattern(search);
                    HideColumns();
                    break;
                case 7:
                    dgvChildren.ItemsSource = _childServices.GetChildByBirthPlacePattern(search);
                    HideColumns();
                    break;
                default:
                    break;
            }
        }

        private void HideColumns()
        {
            var columnsToHide = new List<string>
            {
                "ProfileImage",
                /*"Attendances",
                "Notes",
                "Gruop",*/
                "Parents",

            };

            foreach (string columnName in columnsToHide)
            {
                var column = dgvChildren.Columns.FirstOrDefault(c => c.Header.ToString() == columnName);
                if (column != null)
                {
                    column.Visibility = Visibility.Collapsed;
                }
            }
        }

        // Hide sidebarProfile
        private void btnCloseSidebarProfile_Click(object sender, RoutedEventArgs e)
        {
            HideSidebarProfile();
        }

        public void HideSidebarProfile()
        {
            var slideOutAnimation = FindResource("SlideOutAnimationProfile") as Storyboard;

            var sidebarProfile = (Border)FindName("sidebarProfile");

            if (sidebarProfile.Visibility == Visibility.Visible)
            {
                // sakrij bočnu traku uz animaciju slajdanja s lijeva na desno
                slideOutAnimation.Completed += (s, _) => sidebarProfile.Visibility = Visibility.Collapsed;
                slideOutAnimation.Begin(sidebarProfile);
            }
        }

        // Show sidebarProfile and selected child
        private void btnShowProfile_Click(object sender, RoutedEventArgs e)
        {
            // Pronalaženje animacija
            var slideInAnimation = FindResource("SlideInAnimationProfile") as Storyboard;

            var sidebarProfile = (Border)FindName("sidebarProfile");

            if (sidebarProfile.Visibility == Visibility.Collapsed)
            {
                var selectedChild = dgvChildren.SelectedItem as Child;

                if (selectedChild != null)
                {
                    var ucChildSidebarProfile = new ucChildProfileSidebar(selectedChild, this);
                    contentSidebarProfile.Content = ucChildSidebarProfile;
                    sidebarProfile.Visibility = Visibility.Visible;
                    slideInAnimation.Begin(sidebarProfile);
                } else
                {
                    MessageBox.Show("Molimo odaberite dijete iz tablice.");
                }
            }
        }

        // Refresh sidebarProfile content when choose another child
        private void dgvChildren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sidebarProfile = (Border)FindName("sidebarProfile");

            if (sidebarProfile.Visibility == Visibility.Visible)
            {
                var selectedChild = dgvChildren.SelectedItem as Child;

                if (selectedChild != null)
                {
                    var ucChildSidebarProfile = new ucChildProfileSidebar(selectedChild, this);
                    contentSidebarProfile.Content = ucChildSidebarProfile;
                }
            }
        }

        // Hide registration sideabr
        private void btnCloseSidebarRegistration_Click(object sender, RoutedEventArgs e)
        {
            var slideOutAnimation = FindResource("SlideOutAnimationRegistration") as Storyboard;

            var sidebarRegistration = (Border)FindName("sidebarRegistration");

            if (sidebarRegistration.Visibility == Visibility.Visible)
            {
                // sakrij bočnu traku uz animaciju slajdanja s lijeva na desno
                slideOutAnimation.Completed += (s, _) => sidebarRegistration.Visibility = Visibility.Collapsed;
                slideOutAnimation.Begin(sidebarRegistration);
            }
        }

        // Show registration sidebar
        private void btnAddNewChild_Click(object sender, RoutedEventArgs e)
        {
            // Pronalaženje animacija
            var slideInAnimation = FindResource("SlideInAnimationRegistration") as Storyboard;

            var sidebarRegistration = (Border)FindName("sidebarRegistration");

            if (sidebarRegistration.Visibility == Visibility.Collapsed)
            {
                var ucParentRegistration = new ucParentRegistration(new List<Parent>(), null, null, false, false, null, this);
                contentSidebarRegistration.Content = ucParentRegistration;

                sidebarRegistration.Visibility = Visibility.Visible;
                slideInAnimation.Begin(sidebarRegistration);
            }
        }

        // Refresh GUI
        public async void RefreshGUI()
        {
            dgvChildren.ItemsSource = await Task.Run(() => _childServices.GetAllChildren());
            HideColumns();
        }
    }
}
