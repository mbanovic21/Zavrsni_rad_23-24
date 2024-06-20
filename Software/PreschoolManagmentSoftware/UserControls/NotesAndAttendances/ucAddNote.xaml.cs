using EntityLayer.Entities;
using EntityLayer.Enums;
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
    /// Interaction logic for ucAddNote.xaml
    /// </summary>
    public partial class ucAddNote : UserControl
    {
        private Child _child {  get; set; }
        private ucNotes _previousControl { get; set; }
        public ucAddNote(ucNotes ucNotes, Child child)
        {
            InitializeComponent();
            _previousControl = ucNotes;
            _child = child;
        }

        //cmb fill
        private void ucAddNotee_Loaded(object sender, RoutedEventArgs e)
        {
            cmbBehaviour.ItemsSource = Enum.GetValues(typeof(Behaviours));
            cmbBehaviour.SelectedIndex = 0;
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

        //Description
        private void textDescription_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rtxtDescription.Focus();
        }

        //description
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

        private void btnAddNewNote_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCloseSidebarNote_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbBehaviour.IsDropDownOpen = true;
        }
    }
}
