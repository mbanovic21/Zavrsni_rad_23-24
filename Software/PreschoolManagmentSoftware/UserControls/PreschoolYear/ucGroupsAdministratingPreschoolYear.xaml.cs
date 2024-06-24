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
    /// Interaction logic for ucGroupsAdministratingPreschoolYear.xaml
    /// </summary>
    public partial class ucGroupsAdministratingPreschoolYear : UserControl
    {
        public List<Group> Groups = new List<Group>();
        private GroupServices _groupServices = new GroupServices();
        private ucPreschoolYearAdministrating _previousControl { get; set; }
        private PreschoolYearServices _preschoolYearServices = new PreschoolYearServices();
        private WeeklyScheduleServices _weeklyScheduleServices = new WeeklyScheduleServices();
        public ucGroupsAdministratingPreschoolYear(ucPreschoolYearAdministrating ucPreschoolYearAdministrating)
        {
            InitializeComponent();
            _previousControl = ucPreschoolYearAdministrating;
        }

        private void ucGroupsAdministratingPY_Loaded(object sender, RoutedEventArgs e)
        {
            txtHeaderYear.Text = _previousControl.cmbYears.SelectedValue.ToString();
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

        //create new group
        private void btnAddNewGroup_Click(object sender, RoutedEventArgs e)
        {
            var ucAddNewGroup = new ucAddNewGroupIntoExistingPreschoolYear(this);
            contentSidebarAddGroup.Content = ucAddNewGroup;
            txtHeader.Margin = new Thickness(7, -2, 0, 20);
            _previousControl.btnCloseSidebarAddNewPreschoolYear.Visibility = Visibility.Collapsed;
            OpenSidebar();
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

        private void btnDeleteGroupFromYearsGroups_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddGroupToYear_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
