﻿using BusinessLogicLayer.DBServices;
using EntityLayer.Entities;
using PreschoolManagmentSoftware.Static_Classes;
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
        private UserServices _userServices = new UserServices();
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
            RefreshGUI();
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
                dgvEmployees.ItemsSource = _userServices.GetAllUsers();
                HideColumns();
            }
        }

        private void UpdateData()
        {
            var search = txtSearch.Text.ToLower();
            var selectedItem = cmbSearch.SelectedIndex;

            if (string.IsNullOrEmpty(search))
            {
                dgvEmployees.ItemsSource = _userServices.GetAllUsers();
                HideColumns();
                return;
            }

            switch (selectedItem)
            {
                case 0:
                    dgvEmployees.ItemsSource = _userServices.GetUserByUsernamePattern(search);
                    HideColumns();
                    break;
                case 1: 
                    dgvEmployees.ItemsSource = _userServices.GetUserByPINPattern(search);
                    HideColumns();
                    break;
                case 2:
                    dgvEmployees.ItemsSource = _userServices.GetUserByFirstNamePattern(search);
                    HideColumns();
                    break;
                case 3:
                    dgvEmployees.ItemsSource = _userServices.GetUserByLastNamePattern(search);
                    HideColumns();
                    break;
                case 4:
                    dgvEmployees.ItemsSource = _userServices.GetUserByFirstNameAndLastNamePattern(search);
                    HideColumns();
                    break;
                case 5:
                    dgvEmployees.ItemsSource = _userServices.GetUserByEmailPattern(search);
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

        // Show sidebarProfile and selected emplyee
        private void btnShowProfile_Click(object sender, RoutedEventArgs e)
        {
            // Pronalaženje animacija
            var slideInAnimation = FindResource("SlideInAnimationProfile") as Storyboard;

            var sidebarProfile = (Border)FindName("sidebarProfile");

            if (sidebarProfile.Visibility == Visibility.Collapsed)
            {
                var selectedUser = dgvEmployees.SelectedItem as User;

                if (selectedUser != null)
                {
                    var ucEmplyoeeSidebarProfile = new ucEmployeeProfileSidebar(selectedUser, this);
                    contentSidebarProfile.Content = ucEmplyoeeSidebarProfile;
                    sidebarProfile.Visibility = Visibility.Visible;
                    slideInAnimation.Begin(sidebarProfile);
                } else
                {
                    MessageBox.Show("Molimo odaberite zaposlenika iz tablice.");
                }
            }
        }

        // Refresh sidebarProfile content when choose another employee
        private void dgvEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sidebarProfile = (Border)FindName("sidebarProfile");

            if (sidebarProfile.Visibility == Visibility.Visible)
            {
                var selectedUser = dgvEmployees.SelectedItem as User;

                if (selectedUser != null)
                {
                    var ucEmplyoeeSidebarProfile = new ucEmployeeProfileSidebar(selectedUser, this);
                    contentSidebarProfile.Content = ucEmplyoeeSidebarProfile;
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
        private void btnAddNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            // Pronalaženje animacija
            var slideInAnimation = FindResource("SlideInAnimationRegistration") as Storyboard;

            var sidebarRegistration = (Border)FindName("sidebarRegistration");

            if (sidebarRegistration.Visibility == Visibility.Collapsed)
            {
                var selectedUser = dgvEmployees.SelectedItem as User;

                var ucEmployeeRegistrationSidebar = new ucEmployeeRegistrationSidebar(this);
                contentSidebarRegistration.Content = ucEmployeeRegistrationSidebar;
                
                sidebarRegistration.Visibility = Visibility.Visible;
                slideInAnimation.Begin(sidebarRegistration);
            }
        }

        // Refresh GUI
        public async void RefreshGUI()
        {
            dgvEmployees.ItemsSource = await Task.Run(() => _userServices.GetAllUsers());
            HideColumns();
        }
    }
}
