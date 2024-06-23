using BusinessLogicLayer.DBServices;
using PreschoolManagmentSoftware.UserControls.WeeklySchedule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PreschoolManagmentSoftware.UserControls.Dashboard
{
    /// <summary>
    /// Interaction logic for ucActivities.xaml
    /// </summary>
    public partial class ucActivitiesDashboard : UserControl
    {
        private string _daysName { get; set; }
        private string _date { get; set; }
        private DailyActivityServices _dailyActivityServices = new DailyActivityServices();
        public ucActivitiesDashboard(string daysName, string date)
        {
            InitializeComponent();
            _daysName = daysName;
            _date = date;
        }

        private void activities_Loaded(object sender, RoutedEventArgs e)
        {
            if (_date == null) _date = DateTime.Now.ToString("dd.MM.yyyy.");
            txtHeader.Text = $"{_daysName}, {_date}";
            RefreshGUI();
        }

        public async void RefreshGUI()
        {
            var activities = await Task.Run(() => _dailyActivityServices.GetAllActivitiesByDate(_date));
            dgvEmployeesActivities.ItemsSource = activities;

            if (activities == null || !activities.Any())
            {
                txtNoActivities.Visibility = Visibility.Visible;
                dgvEmployeesActivities.Visibility = Visibility.Collapsed;
            } else
            {
                txtNoActivities.Visibility = Visibility.Collapsed;
                dgvEmployeesActivities.Visibility = Visibility.Visible;
            }

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
    }
}
