using PreschoolManagmentSoftware.Static_Classes;
using PreschoolManagmentSoftware.UserControls;
using PreschoolManagmentSoftware.UserControls.DashboardAndCharts;
using PreschoolManagmentSoftware.UserControls.EmailNotifier;
using PreschoolManagmentSoftware.UserControls.NotesAndAttendances;
using PreschoolManagmentSoftware.UserControls.PreschoolYear;
using PreschoolManagmentSoftware.UserControls.WeeklySchedule;
using SecurityLayer;
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

namespace PreschoolManagmentSoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GuiManager.MainWindow = this;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            } else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Dashboard
        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            var ucDashboard = new ucDashboard();
            contentControl.Content = ucDashboard;
        }

        //Employee administrating
        private void btnEmployeeAdministrating_Click(object sender, RoutedEventArgs e)
        {
            var ucEmployeeAdministrating = new ucEmployeeAdministrating();
            contentControl.Content = ucEmployeeAdministrating;
        }

        //Children administrating
        private void btnChildrenAdministrating_Click(object sender, RoutedEventArgs e)
        {
            var ucChildrenAdministrating = new ucChildrenAdministrating();
            contentControl.Content = ucChildrenAdministrating;
        }

        //WeeklyScheduleAdmin
        private void btnWeeklyScheduleAdmin_Click(object sender, RoutedEventArgs e)
        {
            var ucWeeklyScheduleAdmin = new ucWeeklyScheduleAdmin();
            contentControl.Content = ucWeeklyScheduleAdmin;
        }

        //WeeklyScheduleEmplyoee
        private void btnWeeklyScheduleEmployee_Click(object sender, RoutedEventArgs e)
        {
            var ucWeeklyScheduleEmployee = new ucWeeklyScheduleEmployee();
            contentControl.Content = ucWeeklyScheduleEmployee;
        }

        //PreschoolYearAdministrating
        private void btnWeeklyPreschoolYearAdministrating_Click(object sender, RoutedEventArgs e)
        {
            var ucPreschoolYearAdministrating = new ucPreschoolYearAdministrating();
            contentControl.Content = ucPreschoolYearAdministrating;
        }

        //Notes
        private void btnNotes_Click(object sender, RoutedEventArgs e)
        {
            var ucChildrenTracking = new ucChildrenTracking();
            contentControl.Content = ucChildrenTracking;
        }

        //Email notofier
        private void btnEmailNotifier_Click(object sender, RoutedEventArgs e)
        {
            var ucEmailNotifier = new ucEmailNotifier();
            contentControl.Content = ucEmailNotifier;
        }

        private void borHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
