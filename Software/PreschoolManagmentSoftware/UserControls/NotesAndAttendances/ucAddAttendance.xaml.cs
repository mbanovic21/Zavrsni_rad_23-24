using BusinessLogicLayer.DBServices;
using EntityLayer;
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

namespace PreschoolManagmentSoftware.UserControls.NotesAndAttendances
{
    /// <summary>
    /// Interaction logic for ucAddAttendance.xaml
    /// </summary>
    public partial class ucAddAttendance : UserControl
    {
        private ucChildrenTracking _previousControl { get; set; }
        private List<Child> _children { get; set; }
        private AttendanceServices _attendanceServices = new AttendanceServices();
        public ucAddAttendance(ucChildrenTracking ucChildrenTracking, List<Child> children)
        {
            InitializeComponent();
            _previousControl = ucChildrenTracking;
            _children = children;
        }

        private void ucAttendancee_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGUI();
        }

        private void RefreshGUI()
        {
            dgvChildren.ItemsSource = null;
            dgvChildren.ItemsSource = _children;
            HideColumns();
        }

        //Date
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dpDate.Focus();
            e.Handled = true;
        }

        private void textDate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dpDate.IsDropDownOpen = !dpDate.IsDropDownOpen;
        }

        private void btnAddAttendance_Click(object sender, RoutedEventArgs e)
        {
            if (!isValidate()) return;

            var date = dpDate.Text;

            var attendance = new Attendance
            {
                Date = date,
                isPresent = true,
                Id_User = LoggedInUser.User.Id
            };

            var isAdded = _attendanceServices.AddAttendance(_children, attendance);
            if (isAdded)
            {
                MessageBox.Show("Prisustvo je uspješno dodano i dodijeljeno svoj odabranoj djeci!");
            } else
            {
                MessageBox.Show("Pogreška kod dodavanja i dodjeljivanja prisustva djeci!");
            }
            _previousControl.CloseSidebar();
        }

        private void btnDeleteChild_Click(object sender, RoutedEventArgs e)
        {
            var selectedChildren = dgvChildren.SelectedItems.Cast<Child>().ToList();
            if (selectedChildren != null && selectedChildren.Count > 0)
            {
                foreach (var child in selectedChildren)
                {
                    _children.Remove(child);
                }
                RefreshGUI();
            } else
            {
                MessageBox.Show("Odaberite barem jedno dijete koje želite ukloniti s liste!");
            }
        }


        private bool isValidate()
        {
            var date = dpDate.Text;
           
            if (string.IsNullOrWhiteSpace(date))
            {
                MessageBox.Show("Molimo unesite datum rođenja.");
                return false;
            }

            if (_children.Count < 1)
            {
                MessageBox.Show("Morate imati barem jedno dijete u listi!");
                return false;
            }

            return true;
        }

        private void HideColumns()
        {
            var columnsToHide = new List<string>
            {
                "ProfileImage",
                "Attendances",
                "Notes",
                "Group",
                "Parents"
            };

            foreach (string columnName in columnsToHide)
            {
                var column = dgvChildren.Columns.FirstOrDefault(c => c.Header.ToString() == columnName);
                if (column != null)
                {
                    column.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
