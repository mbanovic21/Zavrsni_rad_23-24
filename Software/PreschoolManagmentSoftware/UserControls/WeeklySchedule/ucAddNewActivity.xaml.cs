using BusinessLogicLayer.DBServices;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ucAddNewActivity.xaml
    /// </summary>
    public partial class ucAddNewActivity : UserControl
    {
        private ucEmployeeActivitiesSidebar _ucEmployeeActivitiesSidebar {  get; set; }
        private string _daysName { get; set; }
        private string _date { get; set; }
        private DayService _dayService = new DayService();
        private DailyActivityServices _dailyActivityServices = new DailyActivityServices();
        public ucAddNewActivity(ucEmployeeActivitiesSidebar ucEmployeeActivitiesSidebar, string daysName, string date)
        {
            InitializeComponent();
            _ucEmployeeActivitiesSidebar = ucEmployeeActivitiesSidebar;
            _daysName = daysName;
            _date = date;
        }

        private void ucAddNewActivityy_Loaded(object sender, RoutedEventArgs e)
        {
            textHeader.Text = $"{_daysName}, {_date}";
            cmbLocation.ItemsSource = Enum.GetValues(typeof(Location));
            cmbLocation.SelectedIndex = 0;
        }

        //Activity name
        private void textActivityName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtActivityName.Focus();
        }

        private void txtActivityName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var activityName = txtActivityName.Text;
            var placeholderActivityName = textActivityName;

            if (!string.IsNullOrEmpty(activityName) && activityName.Length >= 0)
            {
                placeholderActivityName.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderActivityName.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(activityName))
            {
                if (string.IsNullOrWhiteSpace(activityName)) return;
                txtActivityName.Clear();
                MessageBox.Show("Aktivnost može sadržavati samo slova.");
                return;
            }
        }

        //StartTime
        private void textStartTime_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtStartTime.Focus();
        }

        private void txtStartTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            var startTime = txtStartTime.Text;
            var placeholderStartTime = textStartTime;

            if (!string.IsNullOrEmpty(startTime) && startTime.Length >= 0)
            {
                placeholderStartTime.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderStartTime.Visibility = Visibility.Visible;
            }

            if (!IsValidTimeCharacters(startTime) || startTime.Length > 5)
            {
                if (string.IsNullOrWhiteSpace(startTime)) return;
                txtStartTime.Clear();
                MessageBox.Show("Vrijeme može sadržavati brojeve, ':' i mora imati 5 znakova.");
                return;
            }
        }

        //EndTime
        private void textEndTime_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtEndTime.Focus();
        }

        private void txtEndTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            var endTime = txtEndTime.Text;
            var placeholderEndTime = textEndTime;

            if (!string.IsNullOrEmpty(endTime) && endTime.Length >= 0)
            {
                placeholderEndTime.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderEndTime.Visibility = Visibility.Visible;
            }

            if (!IsValidTimeCharacters(endTime) || endTime.Length > 5)
            {
                if (string.IsNullOrWhiteSpace(endTime)) return;
                txtEndTime.Clear();
                MessageBox.Show("Vrijeme može sadržavati brojeve, ':' i mora imati 5 znakova.");
                return;
            }
        }

        //Description
        private void textDescription_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rtxtDescription.Focus();
        }

        private void rtxtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            var description = new TextRange(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentEnd).Text;
            var placeholderDescription = textDescription;

            if (string.IsNullOrEmpty(description))
            {
                placeholderDescription.Visibility = Visibility.Visible;
            } else
            {
                placeholderDescription.Visibility = Visibility.Collapsed;
            }
        }

        private async void btnAddNewActivity_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRegistration()) return;

            var day = await Task.Run(() => _dayService.getDayByDateAndName(_date, _daysName));

            var activityName = txtActivityName.Text;
            var startTime = txtStartTime.Text;
            var endTime = txtEndTime.Text;
            var location = cmbLocation.SelectedValue.ToString();
            var description = new TextRange(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentEnd).Text;

            var activity = new DailyActivity()
            {
                Name = activityName,
                StartTime = startTime,
                EndTime = endTime,
                Location = location,
                Description = description
            };

            var isDailyActivityAdded = await Task.Run(() => _dailyActivityServices.AddDailyActivity(activity, day));
            if (isDailyActivityAdded)
            {
                _ucEmployeeActivitiesSidebar.RefreshGUI();
                MessageBox.Show($"Dnevna aktivnost za dan '{_daysName} {_date}' uspješno dodana!");
                ClearFields();
            } else
            {
                MessageBox.Show("Došlo je do greške prilikom dodavanja dnevne aktivnosti!");
            }
        }

        private void ClearFields()
        {
            txtActivityName.Clear();
            txtStartTime.Clear();
            txtEndTime.Clear();
            cmbLocation.SelectedIndex = 0;
            rtxtDescription.Document.Blocks.Clear();
        }

        //Input validation
        private bool ValidateRegistration()
        {
            var activityName = txtActivityName.Text;
            var startTime = txtStartTime.Text;
            var endTime = txtEndTime.Text;
            var description = new TextRange(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentEnd).Text;

            if (string.IsNullOrWhiteSpace(activityName))
            {
                MessageBox.Show("Molimo unesite naziv aktivnosti");
                txtActivityName.Clear();
                return false;
            }

            if (string.IsNullOrWhiteSpace(startTime) || startTime.Length != 5)
            {
                MessageBox.Show("Molimo unesite početno vrijeme aktivnosti. Mora imati 5 znakova!");
                txtStartTime.Clear();
                return false;
            }

            if (string.IsNullOrWhiteSpace(endTime) || endTime.Length != 5)
            {
                MessageBox.Show("Molimo unesite završno vrijeme aktivnosti. Mora imati 5 znakova!");
                txtEndTime.Clear();
                return false;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Molimo opišite novu aktivnost.");
                return false;
            }

            return true;
        }

        public static bool IsValidTimeFormat(string input)
        {
            string pattern = @"^\d{2}:\d{2}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(input);
        }

        public static bool IsValidTimeCharacters(string input)
        {
            string pattern = @"^[\d:]+$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(input);
        }

        private void btnDropdown2_Click(object sender, RoutedEventArgs e)
        {
            cmbResources.IsDropDownOpen = true;
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbLocation.IsDropDownOpen = true;
        }

        private bool IsLettersOnly(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == '-');
        }
    }
}
