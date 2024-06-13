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
    /// Interaction logic for ucAddPreschoolYear.xaml
    /// </summary>
    public partial class ucAddPreschoolYear : UserControl
    {
        public List<Group> Groups = new List<Group>();
        private GroupServices _groupServices = new GroupServices();
        private ucPreschoolYearAdministrating _previousControl { get; set; }
        private PreschoolYearServices _preschoolYearServices = new PreschoolYearServices();
        private WeeklyScheduleServices _weeklyScheduleServices = new WeeklyScheduleServices();
        public ucAddPreschoolYear(ucPreschoolYearAdministrating ucPreschoolYearAdministrating)
        {
            InitializeComponent();
            _previousControl = ucPreschoolYearAdministrating;
        }

        private void ucCreatePreschoolYear_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGUI();
        }

        public async void RefreshGUI()
        {
            dgvGroups.ItemsSource = null;
            dgvGroups.ItemsSource = Groups;
            HideColumns(dgvGroups);
            dgvGroupsDB.ItemsSource = await Task.Run(() => _groupServices.GetAllGroups());
            HideColumns(dgvGroupsDB);
        }
        
        //YearsName
        private void textPreschoolYearName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPreschoolYearName.Focus();
        }

        private void txtPreschoolYearName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var PreschoolYearName = txtPreschoolYearName.Text;
            var placeholderPreschoolYearName = textPreschoolYearName;

            if (!string.IsNullOrEmpty(PreschoolYearName) && PreschoolYearName.Length >= 0)
            {
                placeholderPreschoolYearName.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderPreschoolYearName.Visibility = Visibility.Visible;
            }

            if (!IsNumbersAndSlashOnly(PreschoolYearName) || PreschoolYearName.Length > 5)
            {
                if (string.IsNullOrWhiteSpace(PreschoolYearName)) return;
                txtPreschoolYearName.Clear();
                MessageBox.Show("Naziv predškolske godine može sadržavati samo brojeve i znak '/'.\nMora imati 5 znakova i biti u formatu 'yy/yy'!");
                return;
            }
        }

        //remove group from years groups
        private void btnDeleteGroupFromYearsGroups_Click(object sender, RoutedEventArgs e)
        {
            var selectedGroups = dgvGroups.SelectedItems;

            if (selectedGroups != null)
            {
                foreach (var selectedGroup in selectedGroups)
                {
                    Groups.Remove(selectedGroup as Group);
                }
                RefreshGUI();
            } else
            {
                MessageBox.Show("Molimo odabrite barem jednu grupu.");
            }
        }

        //add groups to years groups
        private void btnAddGroupToYear_Click(object sender, RoutedEventArgs e)
        {
            var selectedGroups = dgvGroupsDB.SelectedItems;

            if (selectedGroups != null)
            {
                foreach (var selectedGroup in selectedGroups)
                {
                    Groups.Add(selectedGroup as Group);
                }
                RefreshGUI();
            } else
            {
                MessageBox.Show("Molimo odabrite barem jednu grupu.");
            }
        }

        //create new group
        private void btnAddNewGroup_Click(object sender, RoutedEventArgs e)
        {
            var ucAddNewGroup = new ucAddNewGroup(this);
            contentSidebarAddGroup.Content = ucAddNewGroup;
            txtHeader.Margin = new Thickness(7, -2, 0, 20);
            _previousControl.btnCloseSidebarAddNewPreschoolYear.Visibility = Visibility.Collapsed;
            OpenSidebar();
        }

        //add preschool year
        private async void btnAddNewPreschoolYear_Click(object sender, RoutedEventArgs e)
        {
            var preschoolYearName = txtPreschoolYearName.Text;
            if (!IsValidYearFormat(preschoolYearName))
            {
                MessageBox.Show("Format mora biti 'yy/yy'!");
                txtPreschoolYearName.Clear();
                return;
            }

            if (Groups.Count < 1) 
            {
                MessageBox.Show("Molimo odaberite grupe koje želite u novoj akademskoj godini!");
                return;
            };

            var newPreschoolYear = new PreeschoolYear
            {
                Year = preschoolYearName,
                inProgress = false,
            };

            var isAdded = _preschoolYearServices.AddNewPreschoolYear(newPreschoolYear, Groups);
            if (isAdded)
            {
                //adding start and end dates in db
                var lastTwoYearNumbers = preschoolYearName.Split('/')[1];
                await SetStartAndEndDateWhenCreatingNewPreschoolYear(lastTwoYearNumbers);
                _previousControl.LoadAllYears();
                CloseSidebar();
                MessageBox.Show("Nova predškolska godina uspješno dodana u sustav!");
            } else
            {
                MessageBox.Show("Pogreška prilikom dodavanja nove predškolske godine!");
            }
        }
        
        private async Task SetStartAndEndDateWhenCreatingNewPreschoolYear(string lastTwoYearNumbers)
        {
            await Task.Run(() => _weeklyScheduleServices.SetStartAndEndDateWhenCreatingNewPreschoolYear(lastTwoYearNumbers));
        }

        public void OpenSidebar()
        {
            // Pronalaženje animacija
            var slideInAnimation = FindResource("SlideInAnimationAddGroup") as Storyboard;

            var sidebarAddGroup = (Border)FindName("sidebarAddGroup");

            if (sidebarAddGroup.Visibility == Visibility.Collapsed)
            {
                sidebarAddGroup.Visibility = Visibility.Visible;
                slideInAnimation.Begin(sidebarAddGroup);
            }
        }

        public void CloseSidebar()
        {
            var slideOutAnimation = FindResource("SlideOutAnimationAddGroup") as Storyboard;

            var sidebarAddGroup = (Border)FindName("sidebarAddGroup");

            if (sidebarAddGroup.Visibility == Visibility.Visible)
            {
                // sakrij bočnu traku uz animaciju slajdanja s lijeva na desno
                slideOutAnimation.Completed += (s, _) => sidebarAddGroup.Visibility = Visibility.Collapsed;
                slideOutAnimation.Begin(sidebarAddGroup);
            }

            txtHeader.Margin = new Thickness(7, -47, 0, 20);
            _previousControl.btnCloseSidebarAddNewPreschoolYear.Visibility = Visibility.Visible;
        }

        private bool IsNumbersAndSlashOnly(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c) && c != '/')
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsValidYearFormat(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length != 5)
            {
                return false;
            }

            if (char.IsDigit(text[0]) && char.IsDigit(text[1]) && text[2] == '/' &&
                char.IsDigit(text[3]) && char.IsDigit(text[4]))
            {
                return true;
            }

            return false;
        }

        private void HideColumns(DataGrid dgv)
        {
            var columnsToHide = new List<string>
            {
                "Children",
                "Users",
                "PreeschoolYears"
            };

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
}
