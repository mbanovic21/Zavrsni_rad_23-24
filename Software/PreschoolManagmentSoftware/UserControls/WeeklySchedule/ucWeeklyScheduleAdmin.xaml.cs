﻿using BusinessLogicLayer.DBServices;
using EntityLayer.Entities;
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

            // Set the year for which we want to load all weeks
            int year = 2024;

            // Start with January 1st of the given year
            DateTime startDate = new DateTime(year, 1, 1);

            // Adjust the start date to the first Monday of the year
            startDate = startDate.AddDays((DayOfWeek.Monday + 7 - startDate.DayOfWeek) % 7);

            // Loop through each week of the year
            while (startDate.Year == year)
            {
                // Calculate the end date of the week
                DateTime endDate = startDate.AddDays(6);

                // Display the date range of the week in the ComboBox
                string weekDisplay = $"{startDate:dd.MM.yyyy.} - {endDate:dd.MM.yyyy.}";

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
        }

        private void cmbWeek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbWeek.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is int week)
            {
                var listDay = _dayServices.getDaysByWeeklySchdule(week);
                clearButtonContent();
                fillTheSchedule(listDay);
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
                // Navigacija unazad jednog tjedna
                DateTime previousWeekStartDate = selectedWeekStartDate.AddDays(-7);
                SetWeekComboBoxValue(previousWeekStartDate);
            }
        }

        private void btnRightArrow_Click(object sender, RoutedEventArgs e)
        {
            if (cmbWeek.SelectedItem != null && cmbWeek.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is DateTime selectedWeekStartDate)
            {
                // Navigacija unaprijed jednog tjedna
                DateTime nextWeekStartDate = selectedWeekStartDate.AddDays(7);
                SetWeekComboBoxValue(nextWeekStartDate);
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

                        if (dayName.ToString() == d.Name.ToString())
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
                                    string users = "";
                                    foreach (var user in UsersByDayId)
                                    {
                                        {
                                            users += user.FirstName + " " + user.LastName + ",\n";
                                        }
                                        var DanDatum = d.Name + " - " + d.Date.ToString().Split(' ')[0];
                                        userButton.Content = DanDatum + "\n" + "\n" + users;
                                        userButton.Background = new SolidColorBrush(Color.FromRgb(2, 235, 111));
                                        userButton.FontWeight = FontWeights.Bold;
                                        userButton.FontSize = 25;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void clearButtonContent()
        {
            foreach (var border in scheduleGrid.Children.OfType<Border>())
            {
                Button userButton = border.Child as Button;
                if (userButton != null)
                {
                    userButton.Content = "";
                    userButton.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }
        }

        private void TextBlock_Click(object sender, RoutedEventArgs e)
        {
            if (cmbWeek.SelectedItem != null && cmbWeek.SelectedItem is ComboBoxItem selectedItem)
            {
                _clickedButton = sender as Button;
                Border parentBorder = _clickedButton.Parent as Border;

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
                            string selectedDay = _selectedDayTextBlock.Text;
                            DateTime selectedWeekStartDate = (DateTime)selectedItem.Tag;

                            //var AddEmployeeToScheduleSidebar = 
                        }
                    }
                }
            }
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbWeek.IsDropDownOpen = true;
        }
    }
}
