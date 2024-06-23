
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

namespace PreschoolManagmentSoftware.UserControls.Dashboard
{
    /// <summary>
    /// Interaction logic for ucDashboard.xaml
    /// </summary>
    public partial class ucDashboard : UserControl
    {
        public ucDashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Loaded(object sender, RoutedEventArgs e)
        {
            LoadWeeklySchedule();
            LoadActivities();
            LoadGroupsChildrenChart();
            LoadDaysActivitiesChart();
            LoadChildrenAttendanceNumber();
        }

        private void LoadWeeklySchedule()
        {
            var ucWeeklyScheduleDashboard = new ucWeeklyScheduleDashboard(this);
            contentControlWeek.Content = ucWeeklyScheduleDashboard;
        }

        private void LoadActivities()
        {
            string todayName = GetDayNameInCroatian(DateTime.Now.DayOfWeek);
            string todayDate = DateTime.Now.ToString("dd.MM.yyyy.");
            var ucActivities = new ucActivitiesDashboard(todayName, todayDate);
            contentControlActivities.Content = ucActivities;
        }

        private string GetDayNameInCroatian(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "Ponedjeljak";
                case DayOfWeek.Tuesday:
                    return "Utorak";
                case DayOfWeek.Wednesday:
                    return "Srijeda";
                case DayOfWeek.Thursday:
                    return "Četvrtak";
                case DayOfWeek.Friday:
                    return "Petak";
                case DayOfWeek.Saturday:
                    return "Subota";
                case DayOfWeek.Sunday:
                    return "Nedjelja";
                default:
                    return "";
            }
        }

        private void LoadGroupsChildrenChart()
        {
            /*var ucGC = new ucGroups_childrenChart();
            contentControlGCchart.Content = ucGC;*/
        }

        private void LoadDaysActivitiesChart()
        {
            /*var ucDA = new ucDayActivityChart();
            contentControlDAchart.Content = ucDA;*/
        }

        private void LoadChildrenAttendanceNumber()
        {
            /*var ucCAN = new ucChildrenAttendanceNumber();
            contentControlNumber.Content = ucCAN;*/
        }
    }
}
