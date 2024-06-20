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

namespace PreschoolManagmentSoftware.UserControls.NotesAndAttendances
{
    /// <summary>
    /// Interaction logic for ucChildrenTracking.xaml
    /// </summary>
    public partial class ucChildrenTracking : UserControl
    {
        private PreschoolYearServices _preschoolYearServices = new PreschoolYearServices();
        private ChildServices _childServices = new ChildServices();
        private GroupServices _groupServices = new GroupServices();
        public ucChildrenTracking()
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
            cmbYears.ItemsSource = await Task.Run(() => _preschoolYearServices.GetAllYears());

            SetCurrentYear();
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
            if (dgv is DataGrid)
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
            var slideInAnimation = FindResource("SlideInAnimationNotes") as Storyboard;

            var sidebarNotes = (Border)FindName("sidebarNotes");

            if (sidebarNotes.Visibility == Visibility.Collapsed)
            {
                sidebarNotes.Visibility = Visibility.Visible;
                slideInAnimation.Begin(sidebarNotes);
            }
        }

        public void CloseSidebar()
        {
            var slideOutAnimation = FindResource("SlideOutAnimationNotes") as Storyboard;

            var sidebarNotes = (Border)FindName("sidebarNotes");

            if (sidebarNotes.Visibility == Visibility.Visible)
            {
                // sakrij bočnu traku uz animaciju slajdanja s lijeva na desno
                slideOutAnimation.Completed += (s, _) => sidebarNotes.Visibility = Visibility.Collapsed;
                slideOutAnimation.Begin(sidebarNotes);
            }
        }

        private void btnNotes_Click(object sender, RoutedEventArgs e)
        {
            var selectedChild = dgvChildren.SelectedItem as Child;
            if(selectedChild != null)
            {
                var ucNotes = new ucNotes(this, selectedChild);
                contentSidebarNotes.Content = ucNotes;
                OpenSidebar();
            } else
            {
                MessageBox.Show("Odaberite dijete za koje želite vidjeti bilješke!");
            }
        }

        private void btnAttendance_Click(object sender, RoutedEventArgs e)
        {
            var selectedChildren = dgvChildren.SelectedItems.Cast<Child>().ToList();
            if (selectedChildren != null && selectedChildren.Count > 0)
            {
                var ucAttendance = new ucAddAttendance(this, selectedChildren);
                contentSidebarNotes.Content = ucAttendance;
                OpenSidebar();
            } else
            {
                MessageBox.Show("Odaberite barem jedno dijete kako biste unijeli prisustvo!");
            }
        }


        private void btnCloseSidebarNotes_Click(object sender, RoutedEventArgs e)
        {
            CloseSidebar();
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbYears.IsDropDownOpen = true;
        }
    }
}
