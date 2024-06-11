﻿using BusinessLogicLayer.DBServices;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreschoolManagmentSoftware.UserControls.WeeklySchedule
{
    /// <summary>
    /// Interaction logic for ucWeeklyScheduleEmployee.xaml
    /// </summary>
    public partial class ucWeeklyScheduleEmployee : UserControl
    {
        private DayService _dayServices = new DayService();
        private WeeklyScheduleServices _weeklyScheduleServices = new WeeklyScheduleServices();
        private List<Day> _days { get; set; } = new List<Day>();
        public ucWeeklyScheduleEmployee()
        {
            InitializeComponent();
        }

        private void weekComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllWeeks();
        }

        private void LoadAllWeeks()
        {
            cmbWeek.Items.Clear();

            // Set the year for which we want to load all weeks
            int currentYear = DateTime.Now.Year;

            // Start with January 1st of the given year
            DateTime startDate = new DateTime(currentYear, 1, 1);

            // Adjust the start date to the first Monday of the year
            startDate = startDate.AddDays((DayOfWeek.Monday + 7 - startDate.DayOfWeek) % 7);

            // Loop through each week of the year
            while (startDate.Year == currentYear)
            {
                // Calculate the end date of the week
                DateTime endDate = startDate.AddDays(6);

                // Display the date range of the week in the ComboBox
                var weekDisplay = $"{startDate:dd.MM.yyyy.} - {endDate:dd.MM.yyyy.}";

                // Create a ComboBoxItem and add it to the ComboBox
                var comboBoxItem = new ComboBoxItem() { Content = weekDisplay, Tag = startDate };
                cmbWeek.Items.Add(comboBoxItem);

                // Check if the current week is the current week of the year
                if (startDate <= DateTime.Now && DateTime.Now <= endDate)
                {
                    comboBoxItem.IsSelected = true;
                }

                // Move to the next week
                startDate = startDate.AddDays(7);
            }

            // Update the selected week text
            if (cmbWeek.SelectedItem != null && cmbWeek.SelectedItem is ComboBoxItem selectedWeekItem && selectedWeekItem.Tag is DateTime selectedWeekStartDate)
            {
                UpdateSelectedWeekText(selectedWeekStartDate);
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


        private void cmbWeek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Provjera je li odabrana stavka null
                if (cmbWeek.SelectedItem is ComboBoxItem selectedItem)
                {
                    var weekDisplay = selectedItem.Content?.ToString();
                    if (string.IsNullOrEmpty(weekDisplay))
                    {
                        // Ako je sadržaj null ili prazan, izađi iz metode
                        return;
                    }

                    // Provjera je li _weeklyScheduleServices inicijaliziran
                    if (_weeklyScheduleServices == null)
                    {
                        // Logiraj ili obradi situaciju gdje je _weeklyScheduleServices null
                        throw new NullReferenceException("_weeklyScheduleServices nije inicijaliziran.");
                    }

                    // Dobivanje ID-a tjednog rasporeda
                    var weeklyScheduleId = _weeklyScheduleServices.GetWeeklySchedulesIDByDates(weekDisplay);

                    // Provjera je li _dayServices inicijaliziran
                    if (_dayServices == null)
                    {
                        // Logiraj ili obradi situaciju gdje je _dayServices null
                        throw new NullReferenceException("_dayServices nije inicijaliziran.");
                    }

                    // Dobivanje dana po ID-u tjednog rasporeda i korisničkog imena
                    var listDay = _dayServices.getDaysByWeeklySchduleAndUsername(weeklyScheduleId, LoggedInUser.User.Username);
                    _days = listDay;
                    if (listDay == null)
                    {
                        // Ako je listDay null, obradi tu situaciju
                        throw new NullReferenceException("Nije moguće dobiti dane za navedeni ID tjednog rasporeda.");
                    }

                    // Čišćenje sadržaja gumba
                    clearButtonContent();

                    // Popunjavanje rasporeda
                    fillTheSchedule(listDay);
                }
            } catch (NullReferenceException ex)
            {
                // Logiraj ili obradi iznimku
                MessageBox.Show($"Dogodila se greška: {ex.Message}");
            } catch (Exception ex)
            {
                // Logiraj ili obradi sve druge iznimke
                MessageBox.Show($"Neočekivana greška: {ex.Message}");
            }
        }


        private void SetWeekComboBoxValue(DateTime selectedWeekStartDate)
        {
            foreach (ComboBoxItem item in cmbWeek.Items)
            {
                if (item.Tag is DateTime weekStartDate && weekStartDate == selectedWeekStartDate)
                {
                    item.IsSelected = true;
                    break;
                }
            }
        }

        private void btnLeftArrow_Click(object sender, RoutedEventArgs e)
        {
            if (cmbWeek.SelectedItem != null && cmbWeek.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is DateTime selectedWeekStartDate)
            {
                DateTime previousWeekStartDate = selectedWeekStartDate.AddDays(-7);
                SetWeekComboBoxValue(previousWeekStartDate);
                UpdateSelectedWeekText(previousWeekStartDate);
            }
        }

        private void btnRightArrow_Click(object sender, RoutedEventArgs e)
        {
            if (cmbWeek.SelectedItem != null && cmbWeek.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is DateTime selectedWeekStartDate)
            {
                DateTime nextWeekStartDate = selectedWeekStartDate.AddDays(7);
                SetWeekComboBoxValue(nextWeekStartDate);
                UpdateSelectedWeekText(nextWeekStartDate);
            }
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
                                    userButton.Content = GetEmployeesNames(UsersByDayId);
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
            if (cmbWeek.SelectedItem != null && cmbWeek.SelectedItem is ComboBoxItem selectedItem)
            {
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
                                var selectedWeek = selectedItem.Content.ToString();
                                var selectedDayShort = selectedDayTextBlock.Text.Split(' ')[0];
                                var selectedDaysDate = selectedDayTextBlock.Text.Split(' ')[1];

                                // Pretvaranje kratkog naziva dana u puni naziv dana
                                var fullDayName = GetFullDayName(selectedDayShort);
                                var fullDayDate = GetFullDate(selectedDaysDate);

                                DateTime selectedWeekStartDate = (DateTime)selectedItem.Tag;

                                if (clickedButton.Content != null)
                                {
                                    var ucEmployeeActivitiesSidebar = new ucEmployeeActivitiesSidebar(this, fullDayName, fullDayDate);
                                    contentSidebarEmployeesActivities.Content = ucEmployeeActivitiesSidebar;
                                    OpenSidebar();
                                }
                            }
                        }
                    }
                }
            }    
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
                    return "Četvrtak";
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

        public void OpenSidebar()
        {
            // Pronalaženje animacija
            var slideInAnimation = FindResource("SlideInAnimationEmployeeActivities") as Storyboard;

            var sidebarEmployeeActivities = (Border)FindName("sidebarEmployeesActivities");

            if (sidebarEmployeeActivities.Visibility == Visibility.Collapsed)
            {
                sidebarEmployeeActivities.Visibility = Visibility.Visible;
                slideInAnimation.Begin(sidebarEmployeeActivities);
            }
        }

        public void CloseSidebar()
        {
            var slideOutAnimation = FindResource("SlideOutAnimationEmployeeActivities") as Storyboard;

            var sidebarEmployeeActivities = (Border)FindName("sidebarEmployeesActivities");

            if (sidebarEmployeeActivities.Visibility == Visibility.Visible)
            {
                // sakrij bočnu traku uz animaciju slajdanja s lijeva na desno
                slideOutAnimation.Completed += (s, _) => sidebarEmployeeActivities.Visibility = Visibility.Collapsed;
                slideOutAnimation.Begin(sidebarEmployeeActivities);
            }
        }

        private void btnCloseSidebarEmployeesActivities_Click(object sender, RoutedEventArgs e)
        {
            CloseSidebar();
        }
    }
}
