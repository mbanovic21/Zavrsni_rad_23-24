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

namespace PreschoolManagmentSoftware.UserControls.WeeklySchedule
{
    /// <summary>
    /// Interaction logic for ucEmployeeActivitiesSidebar.xaml
    /// </summary>
    public partial class ucEmployeeActivitiesSidebar : UserControl
    {
        private string _daysName { get; set; }
        private string _date { get; set; }
        private ucWeeklyScheduleEmployee _ucWeeklyScheduleEmployee { get; set; }
        private DailyActivityServices _dailyActivityServices = new DailyActivityServices();
        public ucEmployeeActivitiesSidebar(ucWeeklyScheduleEmployee ucWeeklyScheduleEmployee ,string daysName, string date)
        {
            InitializeComponent();
            _ucWeeklyScheduleEmployee = ucWeeklyScheduleEmployee;
            _daysName = daysName;
            _date = date;
        }

        private void ucEmployeeActivities_Loaded(object sender, RoutedEventArgs e)
        {
            textHeader.Text = $"{_daysName}, {_date}";
            RefreshGUI();
        }

        public async void RefreshGUI()
        {
            dgvEmployeesActivities.ItemsSource = await Task.Run(() => _dailyActivityServices.GetAllActivitiesByDate(_date));
            HideColumns();
        }

        private void HideColumns()
        {
            var columnsToHide = new List<string>
            {
                "Days",
                "Resources"
            };

            foreach (string columnName in columnsToHide)
            {
                var column = dgvEmployeesActivities.Columns.FirstOrDefault(c => c.Header.ToString() == columnName);
                if (column != null)
                {
                    column.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void btnAddNewActivitie_Click(object sender, RoutedEventArgs e)
        {
            var ucAddNewActvity = new ucAddNewActivity(this, _daysName, _date);
            contentSidebarAddNewActivity.Content = ucAddNewActvity;
            OpenSidebar();
            await Task.Delay(500);
            _ucWeeklyScheduleEmployee.btnBackToEmployeeActivitiesSidebar.Visibility = Visibility.Visible;
        }

        private async void btnEditActivitie_Click(object sender, RoutedEventArgs e)
        {
            var activity = dgvEmployeesActivities.SelectedItem as DailyActivity;
            if (activity != null)
            {
                var ucEditActivity = new ucEditActivity(this, _daysName, _date, activity);
                contentSidebarAddNewActivity.Content = ucEditActivity;
                OpenSidebar();
                await Task.Delay(500);
                _ucWeeklyScheduleEmployee.btnBackToEmployeeActivitiesSidebar.Visibility = Visibility.Visible;
            } else
            {
                MessageBox.Show("Odaberite dnevnu aktivnost!");
            }
        }

        private void btnDeleteActivitie_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCloseSidebarAddNewActivity_Click(object sender, RoutedEventArgs e)
        {
            _ucWeeklyScheduleEmployee.btnBackToEmployeeActivitiesSidebar.Visibility = Visibility.Collapsed;
            CloseSidebar();
        }

        public void OpenSidebar()
        {
            // Pronalaženje animacija
            var slideInAnimation = FindResource("SlideInAnimationAddNewActivity") as Storyboard;

            var sidebarAddNewActivity = (Border)FindName("sidebarAddNewActivity");

            if (sidebarAddNewActivity.Visibility == Visibility.Collapsed)
            {
                sidebarAddNewActivity.Visibility = Visibility.Visible;
                slideInAnimation.Begin(sidebarAddNewActivity);
            }
        }

        public void CloseSidebar()
        {
            var slideOutAnimation = FindResource("SlideOutAnimationAddNewActivity") as Storyboard;

            var sidebarAddNewActivity = (Border)FindName("sidebarAddNewActivity");

            if (sidebarAddNewActivity.Visibility == Visibility.Visible)
            {
                // sakrij bočnu traku uz animaciju slajdanja s lijeva na desno
                slideOutAnimation.Completed += (s, _) => sidebarAddNewActivity.Visibility = Visibility.Collapsed;
                slideOutAnimation.Begin(sidebarAddNewActivity);
            }
        }
    }
}
