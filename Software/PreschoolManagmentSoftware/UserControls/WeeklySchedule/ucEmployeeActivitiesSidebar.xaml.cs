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
        private ucEmployeeActivitiesSidebar _ucEmployeeActivitiesSidebar { get; set; }
        public ucEmployeeActivitiesSidebar(ucEmployeeActivitiesSidebar ucEmployeeActivitiesSidebar, string daysName, string date)
        {
            InitializeComponent();
            _ucEmployeeActivitiesSidebar = ucEmployeeActivitiesSidebar;
            _daysName = daysName;
            _date = date;
        }

        private void ucEmployeeActivities_Loaded(object sender, RoutedEventArgs e)
        {
            textHeader.Text = $"{_daysName}, {_date}";
        }

        private void btnAddNewActivitie_Click(object sender, RoutedEventArgs e)
        {
            //var ucAddNewActivity = ucAddNewActivity();
            //_ucEmployeeActivitiesSidebar.Content = ucAddNewActivity;
        }
    }
}
