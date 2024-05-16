using PreschoolManagmentSoftware.Static_Classes;
using PreschoolManagmentSoftware.UserControls;
using SecurityLayer;
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

namespace PreschoolManagmentSoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GuiManager.MainWindow = this;
        }

        //registration
        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            var ucRegistration = new ucRegistration();
            contentControl.Content = ucRegistration;
        }

        //profile
        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            var ucProfile = new ucProfile();
            contentControl.Content = ucProfile;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            } else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnEmployeeAdministrating_Click(object sender, RoutedEventArgs e)
        {
            var ucEmployeeAdministrating = new ucEmployeeAdministrating();
            contentControl.Content = ucEmployeeAdministrating;
        }

        private void borHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
