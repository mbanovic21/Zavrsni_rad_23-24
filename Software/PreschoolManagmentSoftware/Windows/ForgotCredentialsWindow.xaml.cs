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

        private void textID_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtID.Focus();
        }

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtID.Text) && txtID.Text.Length > 0)
            {
                textID.Visibility = Visibility.Collapsed;
            } else
            {
                textID.Visibility = Visibility.Visible;
            }
        }

        private void textFirstname_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtFirstname.Focus();
        }

        private void txtFirstname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFirstname.Text) && txtFirstname.Text.Length > 0)
            {
                textFirstname.Visibility = Visibility.Collapsed;
            } else
            {
                textFirstname.Visibility = Visibility.Visible;
            }
        }

        private void textLastname_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtLastname.Focus();
        }

        private void txtLastname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLastname.Text) && txtLastname.Text.Length > 0)
            {
                textLastname.Visibility = Visibility.Collapsed;
            } else
            {
                textLastname.Visibility = Visibility.Visible;
            }
        }

        private void textDateOfBirth_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtDateOfBirth.Focus();
        }

        private void txtDateOfBirth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDateOfBirth.Text) && txtDateOfBirth.Text.Length > 0)
            {
                textDateOfBirth.Visibility = Visibility.Collapsed;
            } else
            {
                textDateOfBirth.Visibility = Visibility.Visible;
            }
        }

        private void textDescription_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rtxtDescription.Focus();
        }

        private void rtxtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (rtxtDescription.Document.Blocks.Count > 0 && rtxtDescription.Document.Blocks.FirstBlock != null)
            {
                // Provjerava je li sadržaj RichTextBox-a prazan
                textDescription.Visibility = rtxtDescription.Document.Blocks.FirstBlock.GetType() == typeof(Paragraph) &&
                                              ((Paragraph)rtxtDescription.Document.Blocks.FirstBlock).Inlines.Count == 0
                    ? Visibility.Visible : Visibility.Collapsed;
            } else
            {
                // Ako je RichTextBox prazan, prikaži tekst upozorenja
                textDescription.Visibility = Visibility.Visible;
            }
        }

        private void SubmitRequest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Your request has been submitted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
