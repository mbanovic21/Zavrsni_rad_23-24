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
    /// Interaction logic for ForgotCredentialsWindow.xaml
    /// </summary>
    public partial class ForgotCredentialsWindow : Window
    {
        public ForgotCredentialsWindow()
        {
            InitializeComponent();
        }

        private void SubmitRequest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Your request has been submitted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void textDateOfBirth_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void textLastname_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void textFirstname_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void textID_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
