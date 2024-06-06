using BusinessLogicLayer.DBServices;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
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
    /// Interaction logic for ucAddEmployeeToScheduleSidebar.xaml
    /// </summary>
    public partial class ucAddEmployeeToScheduleSidebar : UserControl
    {
        private UserServices _userServices = new UserServices();
        private DayService _dayService = new DayService();
        private WeeklyScheduleServices _weeklyScheduleServices = new WeeklyScheduleServices();
        private ucWeeklyScheduleAdmin _ucWeeklyScheduleAdmin = new ucWeeklyScheduleAdmin();
        private string _name { get; set; }
        private string _date { get; set; }
        private string _weekDisplay { get; set; }
        private Day _day { get; set; }
        private Button _clickedButton { get; set; }
        public ucAddEmployeeToScheduleSidebar(string day, string date, string weekDisplay, Button clickedButton, ucWeeklyScheduleAdmin ucWeeklyScheduleAdmin)
        {
            InitializeComponent();
            _name = day;
            _date = date;
            _weekDisplay = weekDisplay;
            _clickedButton = clickedButton;
            _ucWeeklyScheduleAdmin = ucWeeklyScheduleAdmin;
        }

        //Loading users
        public void ucAddNewEmployee_Loaded(object sender, RoutedEventArgs e)
        {
            textHeader.Text = $"{_name}, {_date}";
        }

        public void FillCombobox()
        {
            cmbSearch.Items.Add("Korisničko ime");
            cmbSearch.Items.Add("OIB");
            cmbSearch.Items.Add("Ime");
            cmbSearch.Items.Add("Prezime");
            cmbSearch.Items.Add("Ime i prezime");
            cmbSearch.Items.Add("E-pošta");

            cmbSearch.SelectedIndex = 0;
            cmbSearch.IsDropDownOpen = false;
        }

        // Refresh GUI
        public async Task RefreshGUIAsync()
        {
            var users = await Task.Run(() => _userServices.GetAllUsers());
            await Dispatcher.InvokeAsync(() =>
            {
                dgvEmployees.ItemsSource = users;
                HideColumns();
            });
        }

        //search-selection
        private void cmbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbSearch.IsDropDownOpen = true;
            if (cmbSearch.SelectedIndex == 0)
            {
                textSearch.Text = "Pretraži po korisničkom imenu";
            } else if (cmbSearch.SelectedIndex == 1)
            {
                textSearch.Text = "Pretraži po OIB-u";
            } else if (cmbSearch.SelectedIndex == 2)
            {
                textSearch.Text = "Pretraži po imenu";
            } else if (cmbSearch.SelectedIndex == 3)
            {
                textSearch.Text = "Pretraži po prezimenu";
            } else if (cmbSearch.SelectedIndex == 4)
            {
                textSearch.Text = "Pretraži po imenu i prezimenu";
            } else if (cmbSearch.SelectedIndex == 5)
            {
                textSearch.Text = "Pretraži po e-pošti";
            }
            UpdateData();
        }

        //Searchbar
        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var placeholderSearch = textSearch;
            var pattern = txtSearch.Text.ToLower();

            if (!string.IsNullOrEmpty(pattern) && pattern.Length >= 0)
            {
                placeholderSearch.Visibility = Visibility.Collapsed;
                UpdateData();
            } else
            {
                placeholderSearch.Visibility = Visibility.Visible;
                dgvEmployees.ItemsSource = _userServices.GetAllUsers();
                HideColumns();
            }
        }

        //Add to schedule
        private async void btnAddEmployeeToSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (dgvEmployees.SelectedItems != null && dgvEmployees.SelectedItems.Count > 0)
            {
                var userList = new List<User>();

                foreach (var d in dgvEmployees.SelectedItems)
                {
                    userList.Add(d as User);
                }

                var weeklyScheduleID = await Task.Run(() => _weeklyScheduleServices.GetWeeklySchedulesIDByDates(_weekDisplay));
                var isDateAlredyTaken = await Task.Run (() => _dayService.isDateAlredyTaken(_date, _name));

                if (isDateAlredyTaken)
                {
                    _day = await Task.Run(() => _dayService.getDayByDateAndName(_date, _name));

                    var updatedDay = new Day
                    {
                        Id = _day.Id,
                        Name = _day.Name,
                        Date = _day.Date,
                        Users = userList
                    };

                    foreach (var u in userList)
                    {
                        MessageBox.Show(u.FirstName);
                    }

                    var isDayUpdated = _dayService.isDayUpdated(updatedDay);
                    if (isDayUpdated)
                    {
                        _day = updatedDay;
                        MessageBox.Show("Dan je uspješno ažuriran!");
                    } else
                    {
                        MessageBox.Show("Pogreška kod ažuriranja dana!");
                    }

                } else
                {
                    _day = new Day
                    {
                        Users = userList,
                        Date = _date,
                        Name = _name,
                        Id_WeeklySchedule = weeklyScheduleID,
                    };

                    _dayService.addNewDay(_day);
                }

                var updatedUsers = await Task.Run(() => _dayService.getUsersByDayId(_day.Id));
                var employees = GetEmployeesNames(updatedUsers);
                _clickedButton.Content = employees;
                _clickedButton.Background = new SolidColorBrush(Color.FromRgb(78, 177, 182));
                _clickedButton.FontWeight = FontWeights.SemiBold;
                _clickedButton.FontSize = 15;
                _ucWeeklyScheduleAdmin.CloseSidebar();
               
            } else
            {
                MessageBox.Show("Odaberite barem jednog zaposlenika!");
            }
        }

        public string GetEmployeesNames(List<User> employees)
        {
            StringBuilder employeesBuilder = new StringBuilder();

            foreach (var employee in employees)
            {
                employeesBuilder.Append($"{employee.FirstName} {employee.LastName}, \n");
            }

            // Ukloni zadnji zarez i razmak
            if (employeesBuilder.Length > 0)
            {
                employeesBuilder.Length -= 2; // ukloni zadnja dva znaka ", "
            }

            return employeesBuilder.ToString();
        }

        private void UpdateData()
        {
            var search = txtSearch.Text.ToLower();
            var selectedItem = cmbSearch.SelectedIndex;

            if (string.IsNullOrEmpty(search))
            {
                dgvEmployees.ItemsSource = _userServices.GetAllUsers();
                HideColumns();
                return;
            }

            switch (selectedItem)
            {
                case 0:
                    dgvEmployees.ItemsSource = _userServices.GetUserByUsernamePattern(search);
                    HideColumns();
                    break;
                case 1:
                    dgvEmployees.ItemsSource = _userServices.GetUserByPINPattern(search);
                    HideColumns();
                    break;
                case 2:
                    dgvEmployees.ItemsSource = _userServices.GetUserByFirstNamePattern(search);
                    HideColumns();
                    break;
                case 3:
                    dgvEmployees.ItemsSource = _userServices.GetUserByLastNamePattern(search);
                    HideColumns();
                    break;
                case 4:
                    dgvEmployees.ItemsSource = _userServices.GetUserByFirstNameAndLastNamePattern(search);
                    HideColumns();
                    break;
                case 5:
                    dgvEmployees.ItemsSource = _userServices.GetUserByEmailPattern(search);
                    HideColumns();
                    break;
                default:
                    break;
            }
        }

        public void HideColumns()
        {
            var columnsToHide = new List<string>
            {
                "ProfileImage",
                "Attendances",
                "Group",
                "Role",
                "Days",
                "Password",
                "Salt"
            };

            foreach (string columnName in columnsToHide)
            {
                var column = dgvEmployees.Columns.FirstOrDefault(c => c.Header.ToString() == columnName);
                if (column != null)
                {
                    column.Visibility = Visibility.Collapsed;
                }
            }
        }

        //dropdown
        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbSearch.IsDropDownOpen = true;
        }
    }
}
