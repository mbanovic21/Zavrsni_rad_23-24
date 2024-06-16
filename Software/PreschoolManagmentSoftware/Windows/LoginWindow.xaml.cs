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
using System.Windows.Shapes;

namespace PreschoolManagmentSoftware.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private AutenticationManager autenticationManager = new AutenticationManager();
        private AuthorizationManager authorizationManager = new AuthorizationManager();

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
                MessageBox.Show("Molimo unesite svoje kreditacije.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Unesite korisničko ime.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Molimo unesite lozinku.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var areCredentialsValid = await Task.Run(() => autenticationManager.AuthenticateUser(username, password));
            if (areCredentialsValid)
            {
                MessageBox.Show("Uspješno ste prijavljeni!", "Uspijeh", MessageBoxButton.OK, MessageBoxImage.Information);
                var mainWindow = new MainWindow();
                Close();
                mainWindow.ShowDialog();
                /*
                var role = authorizationManager.GetRole();
                if(role)
                {
                    var mainWindow = new MainWindow();
                    Close();
                    mainWindow.ShowDialog();
                } else 
                {
                    MessageBox.Show("nije admin!");
                }   
                */
            } else
            {
                MessageBox.Show("Nevažeće kreditacije.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var forgotCredentialsWindow = new ForgotCredentialsWindow();
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
