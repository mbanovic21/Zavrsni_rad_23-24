﻿using EntityLayer;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreschoolManagmentSoftware.UserControls
{
    /// <summary>
    /// Interaction logic for ucEmployeeSidebar.xaml
    /// </summary>
    public partial class ucEmployeeSidebar : UserControl
    {
        public User User { get; set; }
        public ucEmployeeSidebar(User user)
        {
            InitializeComponent();
            User = user;
        }

        private void ucEmplyeeSidebarProfile_Loaded(object sender, RoutedEventArgs e)
        {
            var profileImage = BitmapImageConverter.ConvertByteArrayToBitmapImage(User.ProfileImage);
            var firstname = User.FirstName;
            var lastname = User.LastName;

            imgProfile.Source = profileImage;
            textFirstAndLastName.Text = $"{firstname} {lastname}";

            textUsername.Text = User.Username;
            textPIN.Text = User.PIN;
            textDateOfBirth.Text = User.DateOfBirth;
            textGender.Text = User.Sex;
            textRole.Text = User.Id_role == 1 ? "Administrator" : "Običan";
        }
    }
}
