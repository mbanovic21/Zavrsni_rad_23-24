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
            //loading users
            dgvEmployees.ItemsSource = userServices.GetAllUsers();

            //cmb fill
            cmbSearch.Items.Add("Korisničko ime");
            cmbSearch.Items.Add("OIB");
            cmbSearch.Items.Add("Ime");
            cmbSearch.Items.Add("Prezime");
            cmbSearch.Items.Add("Ime i prezime");
            cmbSearch.Items.Add("E-pošta");

            cmbSearch.SelectedIndex = 0;
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
                return;
            }

            switch (selectedItem)
            {
                case 0:
                    dgvEmployees.ItemsSource = userServices.GetUserByUsernamePattern(search);
                    textSearch.Text = "Pretraži po korisničkom imenu";
                    break;
                case 1:
                    dgvEmployees.ItemsSource = userServices.GetUserByPINPattern(search);
                    textSearch.Text = "Pretraži po OIB-u";
                    break;
                case 2:
                    dgvEmployees.ItemsSource = userServices.GetUserByFirstNamePattern(search);
                    textSearch.Text = "Pretraži po imenu";
                    break;
                case 3:
                    dgvEmployees.ItemsSource = userServices.GetUserByLastNamePattern(search);
                    textSearch.Text = "Pretraži po prezimenu";
                    break;
                case 4:
                    dgvEmployees.ItemsSource = userServices.GetUserByFirstNameAndLastNamePattern(search);
                    textSearch.Text = "Pretraži po imenu i prezimenu";
                    break;
                case 5:
                    dgvEmployees.ItemsSource = userServices.GetUserByEmailPattern(search);
                    textSearch.Text = "Pretraži po e-pošti";
                    break;
                default:
                    break;
            }
        }
    }
}
