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

namespace PreschoolManagmentSoftware.UserControls.PreschoolYear
{
    /// <summary>
    /// Interaction logic for ucAddPreschoolYear.xaml
    /// </summary>
    public partial class ucAddPreschoolYear : UserControl
    {
        private List<Group> _groups;
        private PreschoolYearServices _preschoolYearServices = new PreschoolYearServices();
        public ucAddPreschoolYear(List<Group> groups)
        {
            InitializeComponent();
            _groups = groups;
        }

        private void ucCreatePreschoolYear_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGUI();
        }

        public void RefreshGUI()
        {
            dgvGroups.ItemsSource = _groups;
        }

        private void btnAddExistingGroup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddNewGroup_Click(object sender, RoutedEventArgs e)
        {
            var ucAddNewGroup = new ucAddNewGroup();
            contentSidebarAddGroup.Content = ucAddNewGroup;
            OpenSidebar();
        }

        private void btnAddNewPreschoolYear_Click(object sender, RoutedEventArgs e)
        {
            var preschoolYearName = txtPreschoolYearName.Text;
            if (!IsValidYearFormat(preschoolYearName)) 
            {
                MessageBox.Show("Format mora biti 'yy/yy'!");
                txtPreschoolYearName.Clear();
                return;
            }
        }

        private void textPreschoolYearName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPreschoolYearName.Focus();
        }

        private void txtPreschoolYearName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var PreschoolYearName = txtPreschoolYearName.Text;
            var placeholderPreschoolYearName = textPreschoolYearName;

            if (!string.IsNullOrEmpty(PreschoolYearName) && PreschoolYearName.Length >= 0)
            {
                placeholderPreschoolYearName.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderPreschoolYearName.Visibility = Visibility.Visible;
            }

            if (!IsNumbersAndSlashOnly(PreschoolYearName) || PreschoolYearName.Length > 5)
            {
                if (string.IsNullOrWhiteSpace(PreschoolYearName)) return;
                txtPreschoolYearName.Clear();
                MessageBox.Show("Naziv predškolske godine može sadržavati samo brojeve i znak '/'.\nMora imati 5 znakova i biti u formatu 'yy/yy'!");
                return;
            }
        }

        public void OpenSidebar()
        {
            // Pronalaženje animacija
            var slideInAnimation = FindResource("SlideInAnimationAddGroup") as Storyboard;

            var sidebarAddGroup = (Border)FindName("sidebarAddGroup");

            if (sidebarAddGroup.Visibility == Visibility.Collapsed)
            {
                sidebarAddGroup.Visibility = Visibility.Visible;
                slideInAnimation.Begin(sidebarAddGroup);
            }
        }

        public void CloseSidebar()
        {
            var slideOutAnimation = FindResource("SlideOutAnimationAddGroup") as Storyboard;

            var sidebarAddGroup = (Border)FindName("sidebarAddGroup");

            if (sidebarAddGroup.Visibility == Visibility.Visible)
            {
                // sakrij bočnu traku uz animaciju slajdanja s lijeva na desno
                slideOutAnimation.Completed += (s, _) => sidebarAddGroup.Visibility = Visibility.Collapsed;
                slideOutAnimation.Begin(sidebarAddGroup);
            }
        }

        private bool IsNumbersAndSlashOnly(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c) && c != '/')
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsValidYearFormat(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length != 5)
            {
                return false;
            }

            if (char.IsDigit(text[0]) && char.IsDigit(text[1]) && text[2] == '/' &&
                char.IsDigit(text[3]) && char.IsDigit(text[4]))
            {
                return true;
            }

            return false;
        }
    }
}
