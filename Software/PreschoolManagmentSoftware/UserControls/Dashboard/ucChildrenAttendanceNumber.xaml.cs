using BusinessLogicLayer.DBServices;
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
    /// Interaction logic for ucChildrenAttendanceNumber.xaml
    /// </summary>
    public partial class ucChildrenAttendanceNumber : UserControl
    {
        private AttendanceServices _attendanceServices = new AttendanceServices();
        public ucChildrenAttendanceNumber()
        {
            InitializeComponent();
        }

        private void ucChildrenNumber_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNumber();
        }

        private async void LoadNumber()
        {
            var currentDate = DateTime.Now.ToString("dd.M.yyyy.");
            txtNumber.Text = await Task.Run(() => _attendanceServices.GetAttendancesCountByDate(currentDate).ToString());
        }
    }
}
