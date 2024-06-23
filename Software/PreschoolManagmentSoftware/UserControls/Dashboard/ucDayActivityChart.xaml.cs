using LiveCharts.Wpf;
using LiveCharts;
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
using BusinessLogicLayer.DBServices;
using LiveCharts.Definitions.Charts;

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
            LoadCartesianChart();
        }

        private async void LoadCartesianChart()
        {
            var activities = await Task.Run(() => _activityServices.GetEmployeeActivities());

            if (activities == null || !activities.Any())
            {
                MessageBox.Show("Nema podataka za prikaz.");
                return;
            }

            // Ispis podataka na konzolu
            foreach (var activity in activities)
            {
                System.Diagnostics.Debug.WriteLine($"Zaposlenik: {activity.EmployeeName}, Dan: {activity.DayOfWeek}, Broj aktivnosti: {activity.ActivityCount}");
            }

            // Grupiranje podataka po zaposleniku i danu
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

                // Kreiranje vrijednosti za dane u tjednu (od ponedjeljka do nedjelje)
                var daysOfWeek = new[] { "Ponedjeljak", "Utorak", "Srijeda", "Cetvrtak", "Petak", "Subota", "Nedjelja" };
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

            // Postavljanje osi
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
    }
}
