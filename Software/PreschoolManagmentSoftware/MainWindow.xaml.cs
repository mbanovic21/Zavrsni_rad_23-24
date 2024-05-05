using BusinessLogicLayer.DBServices;
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
        }

        private UserServices userServices = new UserServices();

        private void textUsername_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtUsername.Focus();
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsername.Text) && txtUsername.Text.Length > 0)
            {
                textUsername.Visibility = Visibility.Collapsed;   
            } else
            {
                textUsername.Visibility = Visibility.Visible;
            }
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            } else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter your credentials.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter a username.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter a password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var areCredentialsValid = await Task.Run(() => userServices.AreCredentialsValid(username, password));
            if(areCredentialsValid)
            {
                MessageBox.Show("Successfully logged in!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            } else
            {
                MessageBox.Show("Invalid credentials.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var forgotCredentialsWindow = new Windows.ForgotCredentialsWindow();
            forgotCredentialsWindow.ShowDialog();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
