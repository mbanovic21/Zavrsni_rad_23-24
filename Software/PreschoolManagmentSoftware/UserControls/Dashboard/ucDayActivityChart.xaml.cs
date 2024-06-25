using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BusinessLogicLayer.DBServices;
using EntityLayer.Entities;
using EntityLayer;

namespace PreschoolManagmentSoftware.UserControls.Dashboard
{
    /// <summary>
    /// Interaction logic for ucDayActivityChart.xaml
    /// </summary>
    public partial class ucDayActivityChart : UserControl
    {
        private DailyActivityServices _activityServices = new DailyActivityServices();

        public ucDayActivityChart()
        {
            InitializeComponent();
            InitializeChart();
            LoadCartesianChart();
        }

        private void InitializeChart()
        {
            cartesianChart.AxisX.Add(new Axis
            {
                Title = "Dan u tjednu",
                Labels = new[] { "Ponedjeljak", "Utorak", "Srijeda", "Četvrtak", "Petak", "Subota", "Nedjelja" }
            });

            cartesianChart.AxisY.Add(new Axis
            {
                Title = "Broj aktivnosti",
                LabelFormatter = value => value.ToString()
            });
        }

        private async void LoadCartesianChart()
        {
            var activities = new List<(string EmployeeName, string DayOfWeek, int ActivityCount)>();

            if (LoggedInUser.User.Id_role == 1)
            {
                radioButtonsChart.Visibility = Visibility.Visible;
                if (rbForAll.IsChecked == true)
                {
                    activities = await Task.Run(() => _activityServices.GetEmployeeActivities());
                } else
                {
                    activities = await Task.Run(() => _activityServices.GetEmployeeActivitiesByUserId(LoggedInUser.User.Id));
                }
            } else
            {
                activities = await Task.Run(() => _activityServices.GetEmployeeActivitiesByUserId(LoggedInUser.User.Id));
            }

            if (activities == null || !activities.Any())
            {
                MessageBox.Show("Nema podataka za prikaz.");
                return;
            }

            UpdateChart(activities);
        }

        private void UpdateChart(List<(string EmployeeName, string DayOfWeek, int ActivityCount)> activities)
        {
            // Clear the existing series
            cartesianChart.Series.Clear();

            // Group data by employee and day
            var employeeGroups = activities
                .GroupBy(a => a.EmployeeName)
                .Select(g => new
                {
                    EmployeeName = g.Key,
                    Activities = g.ToList()
                });

            var seriesCollection = new SeriesCollection();

            foreach (var employee in employeeGroups)
            {
                var values = new ChartValues<int>();

                // Create values for days of the week (from Monday to Sunday)
                var daysOfWeek = new[] { "Ponedjeljak", "Utorak", "Srijeda", "Četvrtak", "Petak", "Subota", "Nedjelja" };
                foreach (var day in daysOfWeek)
                {
                    var activityForDay = employee.Activities.FirstOrDefault(a => a.DayOfWeek == day);
                    values.Add(activityForDay != default ? activityForDay.ActivityCount : 0);
                }

                var lineSeries = new LineSeries
                {
                    Title = employee.EmployeeName,
                    Values = values,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 10
                };

                seriesCollection.Add(lineSeries);
            }

            cartesianChart.Series = seriesCollection;
        }

        private void rbView_Checked(object sender, RoutedEventArgs e)
        {
            LoadCartesianChart();
        }
    }
}
