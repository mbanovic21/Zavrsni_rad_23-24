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

namespace PreschoolManagmentSoftware.UserControls.PreschoolYear
{
    /// <summary>
    /// Interaction logic for ucPreschoolYearAdministrating.xaml
    /// </summary>
    public partial class ucPreschoolYearAdministrating : UserControl
    {
        private PreschoolYearServices _preschoolYearServices = new PreschoolYearServices();
        public ucPreschoolYearAdministrating()
        {
            InitializeComponent();
        }


        private void cmbYears_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllYears();
        }

        private void LoadAllYears()
        {
            cmbYears.Items.Clear();
            cmbYears.ItemsSource = _preschoolYearServices.GetAllYears();

            SetCurrentYear();
        }

        private void SetCurrentYear()
        {
            var currentYear = DateTime.Now.Year.ToString().Substring(2);

            foreach (var year in cmbYears.Items)
            {
                var firstYearFromCMB = year.ToString().Split('/')[0];
                if (firstYearFromCMB == currentYear)
                {
                    cmbYears.SelectedItem = year;
                    break;
                }
            }
        }

        private void btnAddNewPreschoolYear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgvChildren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgvGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnRightArrow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnLeftArrow_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
