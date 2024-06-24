using BusinessLogicLayer.DBServices;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Documents;

namespace PreschoolManagmentSoftware.UserControls.WeeklySchedule
{
    /// <summary>
    /// Interaction logic for ucEditActivity.xaml
    /// </summary>
    public partial class ucEditActivity : UserControl
    {
        private ucEmployeeActivitiesSidebar _ucEmployeeActivitiesSidebar { get; set; }
        private string _daysName { get; set; }
        private string _date { get; set; }
        private DailyActivity _activity { get; set; }
        private DayService _dayService = new DayService();
        private DailyActivityServices _dailyActivityServices = new DailyActivityServices();

        public ucEditActivity(ucEmployeeActivitiesSidebar ucEmployeeActivitiesSidebar, string daysName, string date, DailyActivity activity)
        {
            InitializeComponent();
            _ucEmployeeActivitiesSidebar = ucEmployeeActivitiesSidebar;
            _daysName = daysName;
            _date = date;
            _activity = activity;
        }

        private void ucEditActivityy_Loaded(object sender, RoutedEventArgs e)
        {
            textHeader.Text = $"Uredi aktivnost";
            cmbLocation.ItemsSource = Enum.GetValues(typeof(Location));
            cmbLocation.SelectedItem = Enum.Parse(typeof(Location), _activity.Location);

            txtActivityName.Text = _activity.Name;
            txtStartTime.Text = _activity.StartTime;
            txtEndTime.Text = _activity.EndTime;
            rtxtDescription.Document.Blocks.Clear();
            rtxtDescription.Document.Blocks.Add(new Paragraph(new Run(_activity.Description)));

            UpdatePlaceholderVisibility();
        }

        // Ažuriranje vidljivosti placeholdera na temelju teksta
        private void UpdatePlaceholderVisibility()
        {
            textActivityName.Visibility = string.IsNullOrEmpty(txtActivityName.Text) ? Visibility.Visible : Visibility.Collapsed;
            textStartTime.Visibility = string.IsNullOrEmpty(txtStartTime.Text) ? Visibility.Visible : Visibility.Collapsed;
            textEndTime.Visibility = string.IsNullOrEmpty(txtEndTime.Text) ? Visibility.Visible : Visibility.Collapsed;
            textDescription.Visibility = string.IsNullOrEmpty(new TextRange(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentEnd).Text.Trim()) ? Visibility.Visible : Visibility.Collapsed;
        }

        // Naziv aktivnosti
        private void textActivityName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtActivityName.Focus();
        }

        private void txtActivityName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
            var activityName = txtActivityName.Text;

            if (!IsLettersOnly(activityName))
            {
                if (string.IsNullOrWhiteSpace(activityName)) return;
                txtActivityName.Clear();
                MessageBox.Show("Naziv aktivnosti može sadržavati samo slova.");
                return;
            }
        }

        // Početno vrijeme
        private void textStartTime_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtStartTime.Focus();
        }

        private void txtStartTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
            var startTime = txtStartTime.Text;

            if (!IsValidTimeCharacters(startTime) || startTime.Length > 5)
            {
                if (string.IsNullOrWhiteSpace(startTime)) return;
                txtStartTime.Clear();
                MessageBox.Show("Vrijeme može sadržavati brojeve, ':' i mora imati 5 znakova.");
                return;
            }
        }

        // Završno vrijeme
        private void textEndTime_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtEndTime.Focus();
        }

        private void txtEndTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
            var endTime = txtEndTime.Text;

            if (!IsValidTimeCharacters(endTime) || endTime.Length > 5)
            {
                if (string.IsNullOrWhiteSpace(endTime)) return;
                txtEndTime.Clear();
                MessageBox.Show("Vrijeme može sadržavati brojeve, ':' i mora imati 5 znakova.");
                return;
            }
        }

        // Opis
        private void textDescription_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rtxtDescription.Focus();
        }

        private void rtxtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
        }

        private async void btnUpdateActivity_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            var day = await Task.Run(() => _dayService.getDayByDateAndName(_date, _daysName));

            _activity.Name = txtActivityName.Text;
            _activity.StartTime = txtStartTime.Text;
            _activity.EndTime = txtEndTime.Text;
            _activity.Location = cmbLocation.SelectedValue.ToString();
            _activity.Description = new TextRange(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentEnd).Text;

            var isDailyActivityUpdated = await Task.Run(() => _dailyActivityServices.UpdateDailyActivity(_activity, day));
            if (isDailyActivityUpdated)
            {
                _ucEmployeeActivitiesSidebar.RefreshGUI();
                _ucEmployeeActivitiesSidebar.CloseSidebar();
                MessageBox.Show($"Dnevna aktivnost za '{_daysName}, {_date}' uspješno ažurirana!");
            } else
            {
                MessageBox.Show("Došlo je do greške prilikom ažuriranja dnevne aktivnosti!");
            }
        }

        // Validacija unosa
        private bool ValidateInput()
        {
            var activityName = txtActivityName.Text;
            var startTime = txtStartTime.Text;
            var endTime = txtEndTime.Text;
            var description = new TextRange(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentEnd).Text;

            if (string.IsNullOrWhiteSpace(activityName))
            {
                MessageBox.Show("Molimo unesite naziv aktivnosti.");
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

        public static bool IsValidTimeCharacters(string input)
        {
            string pattern = @"^[\d:]+$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(input);
        }

        private bool IsLettersOnly(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == '-');
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbLocation.IsDropDownOpen = true;
        }
    }
}
