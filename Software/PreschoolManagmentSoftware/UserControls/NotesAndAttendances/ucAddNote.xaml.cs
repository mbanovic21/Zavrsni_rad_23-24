using BusinessLogicLayer;
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
        private NoteServices _noteServices = new NoteServices();
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
            cmbBehaviour.SelectedIndex = 4;
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
            if (!isValidate())
            {
                return;
            }

            var date = dpDate.Text;
            var behaviour = cmbBehaviour.SelectedValue.ToString();
            var description = new TextRange(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentEnd).Text;

            var note = new Note
            {
                Date = date,
                Behaviour = behaviour,
                Description = description,
                Id_child = _child.Id
            };

            var isAdded = _noteServices.AddNote(note);

            if (isAdded)
            {
                _previousControl.RefreshGUI();

                var result = MessageBox.Show($"Bilješka za dijete {_child.FirstName} {_child.LastName} je uspješno kreirana i dodana u sustav. Želite li dodati istom djetetu još jednu bilješku?", "Dodavanje grupe", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    cmbBehaviour.SelectedIndex = 4;
                    rtxtDescription.Document.Blocks.Clear();
                    rtxtDescription.Selection.Select(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentStart);
                } else _previousControl.CloseSidebar();
            } else
            {
                MessageBox.Show("Greška prilikom dodavanja bilješke.");
            }
        }

        private void btnCloseSidebarNote_Click(object sender, RoutedEventArgs e)
        {
            _previousControl.CloseSidebar();
        }

        private bool isValidate()
        {
            var date = dpDate.Text;
            var description = new TextRange(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentEnd).Text;

            if (string.IsNullOrWhiteSpace(date))
            {
                MessageBox.Show("Molimo unesite datum rođenja.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Unesite opis.");
                rtxtDescription.Document.Blocks.Clear();
                rtxtDescription.Selection.Select(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentStart);
                return false;
            }

            return true;
        }

        private void btnDropdown_Click(object sender, RoutedEventArgs e)
        {
            cmbBehaviour.IsDropDownOpen = true;
        }
    }
}
