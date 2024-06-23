using BusinessLogicLayer.DBServices;
using EntityLayer;
using EntityLayer.Entities;
using PreschoolManagmentSoftware.UserControls.WeeklySchedule;
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

namespace PreschoolManagmentSoftware.UserControls.DashboardAndCharts
{
    /// <summary>
    /// Interaction logic for ucWeeklyScheduleDashboard.xaml
    /// </summary>
    public partial class ucWeeklyScheduleDashboard : UserControl
    {
        private DayService _dayServices = new DayService();
        private WeeklyScheduleServices _weeklyScheduleServices = new WeeklyScheduleServices();
        public ucWeeklyScheduleDashboard()
        {
            InitializeComponent();
        }

        private void WeekDashboard_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCurrentWeek();
        }

        private void LoadCurrentWeek()
        {
            int currentYear = DateTime.Now.Year;
            DateTime startDate = new DateTime(currentYear, 1, 1);

            // Pronađite prvi ponedjeljak u godini
            startDate = startDate.AddDays((DayOfWeek.Monday + 7 - startDate.DayOfWeek) % 7);

            DateTime currentWeekStartDate = startDate;
            while (currentWeekStartDate.Year == currentYear)
            {
                DateTime currentWeekEndDate = currentWeekStartDate.AddDays(6);
                if (currentWeekStartDate <= DateTime.Now && DateTime.Now <= currentWeekEndDate.AddDays(1))
                {
                    var weekDisplay = $"{currentWeekStartDate:dd.MM.yyyy.} - {currentWeekEndDate:dd.MM.yyyy.}";

                    // Update TextBlock with the week display
                    txtCurrentWeek.Text = weekDisplay;
                    // Update the day-specific TextBlocks
                    UpdateSelectedWeekText(currentWeekStartDate);

                    // Get the weekly schedule ID and update the schedule
                    var weeklyScheduleId = _weeklyScheduleServices.GetWeeklySchedulesIDByDates(weekDisplay);

                    if (string.IsNullOrEmpty(weekDisplay)) return;
                    
                    var listDay = _dayServices.getDaysByWeeklySchduleAndUsername(weeklyScheduleId, LoggedInUser.User.Username);

                    clearButtonContent();
                    fillTheSchedule(listDay);

                    // Break the loop as we have found the current week
                    break;
                }

                // Move to the next week
                currentWeekStartDate = currentWeekStartDate.AddDays(7);
            }
        }

        private void UpdateSelectedWeekText(DateTime selectedWeekStartDate)
        {
            txtMonday.Text = $"Pon. {selectedWeekStartDate:dd.MM.}";
            txtTuesday.Text = $"Uto. {selectedWeekStartDate.AddDays(1):dd.MM.}";
            txtWednesday.Text = $"Sri. {selectedWeekStartDate.AddDays(2):dd.MM.}";
            txtThursday.Text = $"Čet. {selectedWeekStartDate.AddDays(3):dd.MM.}";
            txtFriday.Text = $"Pet. {selectedWeekStartDate.AddDays(4):dd.MM.}";
            txtSaturday.Text = $"Sub. {selectedWeekStartDate.AddDays(5):dd.MM.}";
            txtSunday.Text = $"Ned. {selectedWeekStartDate.AddDays(6):dd.MM.}";
        }

        private void fillTheSchedule(List<Day> listday)
        {
            Grid grid = scheduleGrid;

            for (int i = 1; i <= 7 && i < scheduleGrid.ColumnDefinitions.Count; i++)
            {
                TextBlock dayTextBlock = scheduleGrid.Children
                    .OfType<TextBlock>()
                    .FirstOrDefault(tb => Grid.GetRow(tb) == 0 && Grid.GetColumn(tb) == i);

                if (dayTextBlock != null)
                {
                    string dayName = dayTextBlock.Text;
                    //MessageBox.Show(dayName);

                    //Day matchingDay = listday.Find(d => d.Name == dayName);
                    //var strr = "";
                    foreach (var d in listday)
                    {
                        //MessageBox.Show(d.Name + "==" + dayName + "\n");
                        var day = dayName.Split(' ')[0];
                        var dayFullName = GetFullDayName(day);

                        if (dayFullName == d.Name)
                        {
                            Border dayBorder = scheduleGrid.Children
                                .OfType<Border>()
                                .FirstOrDefault(b => Grid.GetRow(b) == 1 && Grid.GetColumn(b) == i);

                            if (dayBorder != null)
                            {
                                var UsersByDayId = _dayServices.getUsersByDayId(d.Id);
                                //MessageBox.Show(UsersByDayId.ToString() + "\n");
                                Button userButton = dayBorder.Child as Button;
                                if (userButton != null)
                                {
                                    //userButton.Content = GetEmployeesNames(UsersByDayId);
                                    userButton.Background = new SolidColorBrush(Color.FromRgb(78, 177, 182));
                                    userButton.FontWeight = FontWeights.SemiBold;
                                    userButton.FontSize = 15;
                                }
                            }
                        }
                    }
                }
            }
        }

        public string GetEmployeesNames(List<User> employees)
        {
            StringBuilder employeesBuilder = new StringBuilder();

            foreach (var employee in employees)
            {
                employeesBuilder.Append($"{employee.FirstName} {employee.LastName}, \n");
            }

            // Ukloni zadnji zarez ako postoji barem jedan zaposlenik
            if (employeesBuilder.Length > 0)
            {
                employeesBuilder.Length--; // Ukloni zadnji znak ','
            }

            return employeesBuilder.ToString();
        }


        private void clearButtonContent()
        {
            foreach (var border in scheduleGrid.Children.OfType<Border>())
            {
                Button userButton = border.Child as Button;
                if (userButton != null)
                {
                    userButton.Content = null;
                    userButton.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                }
            }
        }

        private void TextBlock_Click(object sender, RoutedEventArgs e)
        {
            DateTime currentWeekStartDate = GetCurrentWeekStartDate();

            if (sender is Button clickedButton)
            {
                var parentBorder = clickedButton.Parent as Border;
                if (parentBorder != null)
                {
                    int column = Grid.GetColumn(parentBorder);

                    if (column > 0 && column <= 7)
                    {
                        var selectedDayTextBlock = scheduleGrid.Children.OfType<TextBlock>()
                            .FirstOrDefault(tb => Grid.GetRow(tb) == 0 && Grid.GetColumn(tb) == column);

                        if (selectedDayTextBlock != null)
                        {
                            var selectedDayShort = selectedDayTextBlock.Text.Split(' ')[0];
                            var selectedDaysDate = selectedDayTextBlock.Text.Split(' ')[1];

                            // Pretvaranje kratkog naziva dana u puni naziv dana
                            var fullDayName = GetFullDayName(selectedDayShort);
                            var fullDayDate = GetFullDate(selectedDaysDate);

                            if (clickedButton.Content != null)
                            {
                                // Implementirajte potrebnu funkcionalnost ovdje
                            }
                        }
                    }
                }
            }
        }

        private DateTime GetCurrentWeekStartDate()
        {
            int currentYear = DateTime.Now.Year;
            DateTime startDate = new DateTime(currentYear, 1, 1);

            // Pronađite prvi ponedjeljak u godini
            startDate = startDate.AddDays((DayOfWeek.Monday + 7 - startDate.DayOfWeek) % 7);

            DateTime currentWeekStartDate = startDate;
            while (currentWeekStartDate.Year == currentYear && currentWeekStartDate.AddDays(6) < DateTime.Now)
            {
                currentWeekStartDate = currentWeekStartDate.AddDays(7);
            }

            if (currentWeekStartDate.Year != currentYear)
            {
                // Ako smo prešli u sljedeću godinu, vratimo se na zadnji tjedan trenutne godine
                currentWeekStartDate = currentWeekStartDate.AddDays(-7);
            }

            return currentWeekStartDate;
        }

        private string GetFullDayName(string day)
        {
            switch (day)
            {
                case "Pon.":
                    return "Ponedjeljak";
                case "Uto.":
                    return "Utorak";
                case "Sri.":
                    return "Srijeda";
                case "Čet.":
                    return "Cetvrtak";
                case "Pet.":
                    return "Petak";
                case "Sub.":
                    return "Subota";
                case "Ned.":
                    return "Nedjelja";
                default:
                    return string.Empty;
            }
        }

        private string GetFullDate(string selectedDaysDate)
        {
            var currentYear = DateTime.Now.Year;
            return $"{selectedDaysDate}{currentYear}.";
        }
    }
}
