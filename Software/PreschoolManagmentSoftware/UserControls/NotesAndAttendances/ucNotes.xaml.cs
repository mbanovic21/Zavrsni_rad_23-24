﻿using BusinessLogicLayer;
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

namespace PreschoolManagmentSoftware.UserControls.NotesAndAttendances
{
    /// <summary>
    /// Interaction logic for ucNotes.xaml
    /// </summary>
    public partial class ucNotes : UserControl
    {
        private Child _child { get; set; }
        private NoteServices _noteServices = new NoteServices();
        private ucChildrenTracking _previousControl = new ucChildrenTracking();
        public ucNotes(ucChildrenTracking ucChildrenTracking, Child child)
        {
            InitializeComponent();
            _child = child;
            _previousControl = ucChildrenTracking;
        }

        private void ucNotesAdministrating_Loaded(object sender, RoutedEventArgs e)
        {
            txtFirstAndLastName.Text = $"{_child.FirstName} {_child.LastName}";
            LoadNotes();
        }

        private async void LoadNotes()
        {
            await Task.Run(() => _noteServices.GetNotesByChild(_child));
        }

        private void btnAddNote_Click(object sender, RoutedEventArgs e)
        {
            var ucAddNote = new ucAddNote(this, _child);
            contentSidebarAddNote = ucAddNote;
        }

        public void OpenSidebar()
        {
            // Pronalaženje animacija
            var slideInAnimation = FindResource("SlideInAnimationAddNote") as Storyboard;

            var sidebarAddNote = (Border)FindName("sidebarAddNote");

            if (sidebarAddNote.Visibility == Visibility.Collapsed)
            {
                sidebarAddNote.Visibility = Visibility.Visible;
                slideInAnimation.Begin(sidebarAddNote);
            }
        }

        public void CloseSidebar()
        {
            var slideOutAnimation = FindResource("SlideOutAnimationAddNote") as Storyboard;

            var sidebarAddNote = (Border)FindName("sidebarAddNote");

            if (sidebarAddNote.Visibility == Visibility.Visible)
            {
                // sakrij bočnu traku uz animaciju slajdanja s lijeva na desno
                slideOutAnimation.Completed += (s, _) => sidebarAddNote.Visibility = Visibility.Collapsed;
                slideOutAnimation.Begin(sidebarAddNote);
            }

            txtHeader.Margin = new Thickness(7, -47, 0, 20);
            _previousControl.btnCloseSidebarNotes.Visibility = Visibility.Visible;
        }

        private void btnDeleteNote_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
