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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreschoolManagmentSoftware.UserControls
{
    /// <summary>
    /// Interaction logic for ucEmployeeAdministrating.xaml
    /// </summary>
    public partial class ucEmployeeAdministrating : UserControl
    {
        private UserServices userServices = new UserServices();
        public ucEmployeeAdministrating()
        {
            InitializeComponent();
        }

        //Loading users
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //cmb fill
            cmbSearch.Items.Add("Korisničko ime");
            cmbSearch.Items.Add("OIB");
            cmbSearch.Items.Add("Ime");
            cmbSearch.Items.Add("Prezime");
            cmbSearch.Items.Add("Ime i prezime");
            cmbSearch.Items.Add("E-pošta");

            cmbSearch.SelectedIndex = 0;
            cmbSearch.IsDropDownOpen = false;

            //loading users
            dgvEmployees.ItemsSource = userServices.GetAllUsers();
            HideColumns();
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
                textSearch.Text = "Pretraži po korisničkom imenu";
            } else if (cmbSearch.SelectedIndex == 1)
            {
                textSearch.Text = "Pretraži po OIB-u";
            } else if (cmbSearch.SelectedIndex == 2)
            {
                textSearch.Text = "Pretraži po imenu";
            } else if (cmbSearch.SelectedIndex == 3)
            {
                textSearch.Text = "Pretraži po prezimenu";
            } else if (cmbSearch.SelectedIndex == 4)
            {
                textSearch.Text = "Pretraži po imenu i prezimenu";
            } else if (cmbSearch.SelectedIndex == 5)
            {
                textSearch.Text = "Pretraži po e-pošti";
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
                dgvEmployees.ItemsSource = userServices.GetAllUsers();
            }
        }

        private void UpdateData()
        {
            var search = txtSearch.Text.ToLower();
            var selectedItem = cmbSearch.SelectedIndex;

            if (string.IsNullOrEmpty(search))
            {
                dgvEmployees.ItemsSource = userServices.GetAllUsers();
                HideColumns();
                return;
            }

            switch (selectedItem)
            {
                case 0:
                    dgvEmployees.ItemsSource = userServices.GetUserByUsernamePattern(search);
                    HideColumns();
                    break;
                case 1: 
                    dgvEmployees.ItemsSource = userServices.GetUserByPINPattern(search);
                    HideColumns();
                    break;
                case 2:
                    dgvEmployees.ItemsSource = userServices.GetUserByFirstNamePattern(search);
                    HideColumns();
                    break;
                case 3:
                    dgvEmployees.ItemsSource = userServices.GetUserByLastNamePattern(search);
                    HideColumns();
                    break;
                case 4:
                    dgvEmployees.ItemsSource = userServices.GetUserByFirstNameAndLastNamePattern(search);
                    HideColumns();
                    break;
                case 5:
                    dgvEmployees.ItemsSource = userServices.GetUserByEmailPattern(search);
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
                "Attendances",
                "Group",
                "Role",
                "Days",
            };
            
            foreach (string columnName in columnsToHide)
            {
                var column = dgvEmployees.Columns.FirstOrDefault(c => c.Header.ToString() == columnName);
                if (column != null)
                {
                    column.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void btnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            // Pronalaženje animacija
            var slideInAnimation = FindResource("SlideInAnimation") as Storyboard;

            var sidebar = (Border)FindName("sidebar");
            
            if (sidebar.Visibility == Visibility.Collapsed)
            {
                // Ako je bočna traka sakrivena, prikaži je uz animaciju slajdanja s desna na lijevo
                sidebar.Visibility = Visibility.Visible;
                slideInAnimation.Begin(sidebar);
            }
        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            var slideOutAnimation = FindResource("SlideOutAnimation") as Storyboard;

            var sidebar = (Border)FindName("sidebar");

            if (sidebar.Visibility == Visibility.Visible)
            {
                // sakrij bočnu traku uz animaciju slajdanja s lijeva na desno
                slideOutAnimation.Completed += (s, _) => sidebar.Visibility = Visibility.Collapsed;
                slideOutAnimation.Begin(sidebar);
            }
        }
    }
}
