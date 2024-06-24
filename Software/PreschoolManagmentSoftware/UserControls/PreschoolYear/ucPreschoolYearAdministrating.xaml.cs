using BusinessLogicLayer.DBServices;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreschoolManagmentSoftware.UserControls.PreschoolYear
{
    /// <summary>
    /// Interaction logic for ucPreschoolYearAdministrating.xaml
    /// </summary>
    public partial class ucPreschoolYearAdministrating : UserControl
    {
        private PreschoolYearServices _preschoolYearServices = new PreschoolYearServices();
        private ChildServices _childServices = new ChildServices();
        private GroupServices _groupServices = new GroupServices();
        public ucPreschoolYearAdministrating()
        {
            InitializeComponent();
        }

        private void cmbYears_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllYears();
        }

        //load years into cmb
        public async void LoadAllYears()
        {
            // Prvo provjerite je li ItemsSource već postavljen
            if (cmbYears.ItemsSource != null)
            {
                cmbYears.ItemsSource = null; // Oslobodite ItemsSource
            }

            // Očistite trenutne stavke
            cmbYears.Items.Clear();

            // Postavite novi ItemsSource
            RefreshGUI();

            SetCurrentYear();
        }

        public async void RefreshGUI()
        {
            cmbYears.ItemsSource = await Task.Run(() => _preschoolYearServices.GetAllYears());
        }

        private void SetCurrentYear()
        {
            var currentYear = DateTime.Now.Year.ToString().Substring(2);

            foreach (var year in cmbYears.Items)
            {
                var firstYearFromCMB = year.ToString().Split('/')[0];
                if (firstYearFromCMB == currentYear)
                {
                    cmbYears.SelectedItem = year;
                    break;
                }
            }
        }

        //left arrow cmb
        private void btnLeftArrow_Click(object sender, RoutedEventArgs e)
        {
            if (cmbYears.SelectedItem != null)
            {
                var currentYearFromCMB = cmbYears.SelectedValue.ToString().Split('/')[0];

                if (int.TryParse(currentYearFromCMB, out int currentYear))
                {
                    for (int i = 0; i < cmbYears.Items.Count; i++)
                    {
                        var itemYear = cmbYears.Items[i].ToString().Split('/')[0];

                        if (int.TryParse(itemYear, out int year))
                        {
                            if (year == currentYear && i > 0)
                            {
                                cmbYears.SelectedItem = cmbYears.Items[i - 1];
                                break;
                            }
                        }
                    }
                }
            }
        }

        //dgvGroup fill by selected year
        private async void cmbYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var columnsToHide = new List<string>
                {
                    "Children",
                    "Users",
                    "PreeschoolYears"
                };

                if (cmbYears.SelectedItem is string selectedItem)
                {
                    var year = cmbYears.SelectedValue.ToString();
                    if (string.IsNullOrEmpty(year)) return;

                    dgvGroups.ItemsSource = await GetGroupsForYear(year);
                }
                HideColumns(dgvGroups, columnsToHide);
            } catch (Exception ex)
            {
                MessageBox.Show($"Neočekivana greška: {ex.Message}");
            }
        }

        private async Task<List<Group>> GetGroupsForYear(string year)
        {
            return await Task.Run(() => _preschoolYearServices.GetGroupsForYear(year));
        }

        //dgvChildren fill by selected group
        private void dgvGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedGroup = dgvGroups.SelectedItem as Group;
            var columnsToHide = new List<string>
            {
                "ProfileImage",
                "Attendances",
                "Notes",
                "Group",
                "Parents"
            };

            if (dgvGroups.SelectedItem != null && selectedGroup != null)
            {
                dgvChildren.ItemsSource = _childServices.GetChildrenFromGroup(dgvGroups.SelectedItem as Group);
            }
           
            HideColumns(dgvChildren, columnsToHide);
        }

        //right arrow cmb
        private void btnRightArrow_Click(object sender, RoutedEventArgs e)
        {
            if (cmbYears.SelectedItem != null)
            {
                var currentYearFromCMB = cmbYears.SelectedValue.ToString().Split('/')[0];

                if (int.TryParse(currentYearFromCMB, out int currentYear))
                {
                    for (int i = 0; i < cmbYears.Items.Count; i++)
                    {
                        var itemYear = cmbYears.Items[i].ToString().Split('/')[0];

                        if (int.TryParse(itemYear, out int year))
                        {
                            if (year == currentYear && i < cmbYears.Items.Count - 1)
                            {
                                cmbYears.SelectedItem = cmbYears.Items[i + 1];
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void HideColumns(DataGrid dgv, List<string> columnsToHide)
        {
            if(dgv is DataGrid)
            {
                foreach (string columnName in columnsToHide)
                {
                    var column = dgv.Columns.FirstOrDefault(c => c.Header.ToString() == columnName);
                    if (column != null)
                    {
                        column.Visibility = Visibility.Collapsed;
                    }
                }
            }            
        }

        public void OpenSidebar()
        {
            // Pronalaženje animacija
            var slideInAnimation = FindResource("SlideInAnimationAddNewPreschoolYear") as Storyboard;

            var sidebarAddNewPreschoolYear = (Border)FindName("sidebarAddNewPreschoolYear");

            if (sidebarAddNewPreschoolYear.Visibility == Visibility.Collapsed)
            {
                sidebarAddNewPreschoolYear.Visibility = Visibility.Visible;
                slideInAnimation.Begin(sidebarAddNewPreschoolYear);
            }
        }

        public void CloseSidebar()
        {
            var slideOutAnimation = FindResource("SlideOutAnimationAddNewPreschoolYear") as Storyboard;

            var sidebarAddNewPreschoolYear = (Border)FindName("sidebarAddNewPreschoolYear");

            if (sidebarAddNewPreschoolYear.Visibility == Visibility.Visible)
            {
                // sakrij bočnu traku uz animaciju slajdanja s lijeva na desno
                slideOutAnimation.Completed += (s, _) => sidebarAddNewPreschoolYear.Visibility = Visibility.Collapsed;
                slideOutAnimation.Begin(sidebarAddNewPreschoolYear);
            }
        }

        private void btnAddNewPreschoolYear_Click(object sender, RoutedEventArgs e)
        {
            var ucAddNewPreschoolYear = new ucAddPreschoolYear(this);
            contentSidebarAddNewPreschoolYear.Content = ucAddNewPreschoolYear;
            OpenSidebar();
        }

        private void btnCloseSidebarAddNewPreschoolYear_Click(object sender, RoutedEventArgs e)
        {
            CloseSidebar();
        }

        private void btnGroupAdministrating_Click(object sender, RoutedEventArgs e)
        {
            var ucGAPY = new ucGroupsAdministratingPreschoolYear(this);
            contentSidebarAddNewPreschoolYear.Content = ucGAPY;
            OpenSidebar();
        }
    }
}
