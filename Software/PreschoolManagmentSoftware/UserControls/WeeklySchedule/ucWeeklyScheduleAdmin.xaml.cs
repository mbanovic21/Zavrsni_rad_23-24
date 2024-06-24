using BusinessLogicLayer.DBServices;
using EntityLayer.Entities;
using PreschoolManagmentSoftware.UserControls.WeeklySchedule;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace PreschoolManagmentSoftware.UserControls
{
    /// <summary>
    /// Interaction logic for ucWeeklyScheduleAdmin.xaml
    /// </summary>
    public partial class ucWeeklyScheduleAdmin : UserControl
    {
        private Button _clickedButton { get; set; }
        private TextBlock _selectedDayTextBlock { get; set; }
        private DayService _dayServices = new DayService();
        private WeeklyScheduleServices _weeklyScheduleServices = new WeeklyScheduleServices();

        public ucWeeklyScheduleAdmin()
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

            int currentYear = DateTime.Now.Year;
            DateTime startDate = new DateTime(currentYear, 1, 1);
            startDate = startDate.AddDays((DayOfWeek.Monday + 7 - startDate.DayOfWeek) % 7);

            while (startDate.Year == currentYear)
            {
                DateTime endDate = startDate.AddDays(6);
                var weekDisplay = $"{startDate:dd.MM.yyyy.} - {endDate:dd.MM.yyyy.}";
                var comboBoxItem = new ComboBoxItem() { Content = weekDisplay, Tag = startDate };
                cmbWeek.Items.Add(comboBoxItem);

                Console.WriteLine(DateTime.Now.ToString());
                Console.WriteLine(endDate.ToString().Split(' ')[0]);

                if (startDate <= DateTime.Now && DateTime.Now <= endDate.AddDays(1))
                {
                    comboBoxItem.IsSelected = true;

                    UpdateSelectedWeekText(startDate);

                    var weekDisplayy = comboBoxItem.Content?.ToString();
                    var weeklyScheduleId = _weeklyScheduleServices.GetWeeklySchedulesIDByDates(weekDisplayy);

                    if (string.IsNullOrEmpty(weekDisplayy))
                    {
                        return;
                    }

                    var listDay = _dayServices.getDaysByWeeklySchdulesID(weeklyScheduleId);

                    clearButtonContent();
                    fillTheSchedule(listDay);
                }
                startDate = startDate.AddDays(7);
            }
        }

        private void UpdateSelectedWeekText(DateTime selectedWeekStartDate)
        {
            DateTime today = DateTime.Today;

            UpdateTextBlock(txtMonday, $"Pon. {selectedWeekStartDate:dd.MM.}", selectedWeekStartDate == today);
            UpdateTextBlock(txtTuesday, $"Uto. {selectedWeekStartDate.AddDays(1):dd.MM.}", selectedWeekStartDate.AddDays(1) == today);
            UpdateTextBlock(txtWednesday, $"Sri. {selectedWeekStartDate.AddDays(2):dd.MM.}", selectedWeekStartDate.AddDays(2) == today);
            UpdateTextBlock(txtThursday, $"Čet. {selectedWeekStartDate.AddDays(3):dd.MM.}", selectedWeekStartDate.AddDays(3) == today);
            UpdateTextBlock(txtFriday, $"Pet. {selectedWeekStartDate.AddDays(4):dd.MM.}", selectedWeekStartDate.AddDays(4) == today);
            UpdateTextBlock(txtSaturday, $"Sub. {selectedWeekStartDate.AddDays(5):dd.MM.}", selectedWeekStartDate.AddDays(5) == today);
            UpdateTextBlock(txtSunday, $"Ned. {selectedWeekStartDate.AddDays(6):dd.MM.}", selectedWeekStartDate.AddDays(6) == today);
        }

        private void UpdateTextBlock(TextBlock textBlock, string text, bool isToday)
        {
            textBlock.Text = text;
            if (isToday)
            {
                textBlock.FontWeight = FontWeights.SemiBold;
            } else
            {
                textBlock.FontWeight = FontWeights.Normal;
            }
        }

        private void cmbWeek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbWeek.SelectedItem is ComboBoxItem selectedItem)
                {
                    var weekDisplay = selectedItem.Content?.ToString();
                    if (string.IsNullOrEmpty(weekDisplay)) return;

                    var weeklyScheduleId = _weeklyScheduleServices.GetWeeklySchedulesIDByDates(weekDisplay);
                    var listDay = _dayServices.getDaysByWeeklySchdulesID(weeklyScheduleId);

                    clearButtonContent();
                    fillTheSchedule(listDay);
                }
            } catch (Exception ex)
            {
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
                    foreach (var d in listday)
                    {
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
                                Button userButton = dayBorder.Child as Button;
                                if (userButton != null)
                                {
                                    if (UsersByDayId.Any())
                                    {
                                        userButton.Content = GetEmployeesNames(UsersByDayId);
                                        userButton.Foreground = new SolidColorBrush(Color.FromRgb(245, 245, 245));
                                        userButton.Background = new SolidColorBrush(Color.FromRgb(78, 177, 182));
                                        userButton.FontWeight = FontWeights.SemiBold;
                                        userButton.FontSize = 15;
                                    } else
                                    {
                                        userButton.Content = string.Empty;
                                        userButton.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                                        userButton.FontWeight = FontWeights.Normal;
                                        userButton.FontSize = 12;
                                    }
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
                    userButton.Content = "";
                    userButton.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                }
            }
        }

        public void OpenSidebar()
        {
            // Pronalaženje animacija
            var slideInAnimation = FindResource("SlideInAnimationAddEmployeeToSchedule") as Storyboard;

            var sidebarAddEmployeeToSchedule = (Border)FindName("sidebarAddEmployeeToSchedule");

            if (sidebarAddEmployeeToSchedule.Visibility == Visibility.Collapsed)
            {
                sidebarAddEmployeeToSchedule.Visibility = Visibility.Visible;
                slideInAnimation.Begin(sidebarAddEmployeeToSchedule);
            }
        }

        public void CloseSidebar()
        {
            var slideOutAnimation = FindResource("SlideOutAnimationAddEmployeeToSchedule") as Storyboard;

            var sidebarAddEmployeeToSchedule = (Border)FindName("sidebarAddEmployeeToSchedule");

            if (sidebarAddEmployeeToSchedule.Visibility == Visibility.Visible)
            {
                // sakrij bočnu traku uz animaciju slajdanja s lijeva na desno
                slideOutAnimation.Completed += (s, _) => sidebarAddEmployeeToSchedule.Visibility = Visibility.Collapsed;
                slideOutAnimation.Begin(sidebarAddEmployeeToSchedule);
            }
        }

        private void btnCloseSidebarAddEmployeeToSchedule_Click(object sender, RoutedEventArgs e)
        {
            CloseSidebar();
        }

        private void TextBlock_Click(object sender, RoutedEventArgs e)
        {
            if (cmbWeek.SelectedItem != null && cmbWeek.SelectedItem is ComboBoxItem selectedItem)
            {
                _clickedButton = sender as Button;
                var parentBorder = _clickedButton.Parent as Border;

                if (parentBorder != null)
                {
                    int column = Grid.GetColumn(parentBorder);

                    if (column > 0 && column <= 7)
                    {
                        _selectedDayTextBlock = scheduleGrid.Children
                            .OfType<TextBlock>()
                            .FirstOrDefault(tb => Grid.GetRow(tb) == 0 && Grid.GetColumn(tb) == column);

                        if (_selectedDayTextBlock != null)
                        {
                            var selectedWeek = selectedItem.Content.ToString();
                            var selectedDayShort = _selectedDayTextBlock.Text.Split(' ')[0];
                            var selectedDaysDate = _selectedDayTextBlock.Text.Split(' ')[1];

                            // Pretvaranje kratkog naziva dana u puni naziv dana
                            var fullDayName = GetFullDayName(selectedDayShort);
                            var fullDayDate = GetFullDate(selectedDaysDate);

                            DateTime selectedWeekStartDate = (DateTime)selectedItem.Tag;

                            var ucAddEmployeeToSchedule = new ucAddEmployeeToScheduleSidebar(fullDayName, fullDayDate, selectedWeek, _clickedButton, this);
                            contentSidebarAddEmployeeToSchedule.Content = ucAddEmployeeToSchedule;

                            OpenSidebar();

                            ucAddEmployeeToSchedule.RefreshGUI();
                            ucAddEmployeeToSchedule.FillCombobox();
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
