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

namespace PreschoolManagmentSoftware.UserControls.DashboardAndCharts
{
    /// <summary>
    /// Interaction logic for ucGroups_childrenChart.xaml
    /// </summary>
    public partial class ucGroups_childrenChart : UserControl
    {
        private GroupServices _groupServices = new GroupServices();
        public ucGroups_childrenChart()
        {
            InitializeComponent();
            LoadPieChart();
        }

        private async void LoadPieChart()
        {
            var groups = await Task.Run(() => _groupServices.GetAllGroups());

            // Kreiranje PieSeries za svaku grupu
            var pieSeries = new SeriesCollection();
            foreach (var group in groups)
            {
                pieSeries.Add(new PieSeries
                {
                    Title = group.Name,
                    Values = new ChartValues<int> { _groupServices.GetGruopsMembersByGroupId(group.Id) },
                    DataLabels = true
                });
            }

            // Dodavanje PieSeries u PieChart
            pieChart.Series = pieSeries;
        }
    }
}
