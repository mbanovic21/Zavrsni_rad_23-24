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
    /// Interaction logic for ucEditNote.xaml
    /// </summary>
    public partial class ucEditNote : UserControl
    {
        private Note _note { get; set; }
        private ucNotes _previousControl { get; set; }
        private NoteServices _noteServices = new NoteServices();
        public ucEditNote(ucNotes ucNotes, Note note)
        {
            InitializeComponent();
            _previousControl = ucNotes;
            _note = note;
        }

        //set all
        private void ucEditNotee_Loaded(object sender, RoutedEventArgs e)
        {
            dpDate.Text = _note.Date;
            LoadBehaviours();
            cmbBehaviour.SelectedIndex = GetBehaviourIndex(_note.Behaviour);
            SetRichTextBoxText(rtxtDescription, _note.Description);
        }

        //fill cmb
        private void LoadBehaviours()
        {
            cmbBehaviour.Items.Clear();
            foreach (var behaviour in Enum.GetValues(typeof(Behaviours)))
            {
                cmbBehaviour.Items.Add(behaviour.ToString());
            }
            cmbBehaviour.SelectedIndex = 4;
        }

        //set cmb 
        private int GetBehaviourIndex(string behaviour)
        {
            for (int i = 0; i < cmbBehaviour.Items.Count; i++)
            {
                if (cmbBehaviour.Items[i].ToString() == behaviour)
                {
                    return i;
                }
            }
            return -1;
        }

        //set rtb
        private void SetRichTextBoxText(RichTextBox rtb, string text)
        {
            rtb.Document.Blocks.Clear();
            rtb.Document.Blocks.Add(new Paragraph(new Run(text)));
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
                Id = _note.Id,
                Date = date,
                Behaviour = behaviour,
                Description = description,
                Id_child = _note.Id_child
            };

            var isUpdated = _noteServices.UpdateNote(note);

            if (isUpdated)
            {
                _previousControl.RefreshGUI();

                var result = MessageBox.Show($"Bilješka je uspješno ažurirana. Želite li ažurirati istom djetetu još jednu bilješku?", "Ažuriranje bilješke", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    cmbBehaviour.SelectedIndex = 4;
                    rtxtDescription.Document.Blocks.Clear();
                    rtxtDescription.Selection.Select(rtxtDescription.Document.ContentStart, rtxtDescription.Document.ContentStart);
                } else _previousControl.CloseSidebar();
            } else
            {
                MessageBox.Show("Greška prilikom ažuriranja bilješke.");
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
