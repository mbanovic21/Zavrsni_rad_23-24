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

namespace PreschoolManagmentSoftware.UserControls.PreschoolYear
{
    /// <summary>
    /// Interaction logic for ucAddNewGroup.xaml
    /// </summary>
    public partial class ucAddNewGroup : UserControl
    {
        private GroupServices _groupServices = new GroupServices();
        public ucAddNewGroup()
        {
            InitializeComponent();
        }
        
        //dgv fill
        private void ucAddNewGroupp_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGUI();
        }

        private async void RefreshGUI()
        {
            dgvGroups.ItemsSource = await Task.Run(() => _groupServices.GetAllGroups());
        }

        //Group name
        private void txtGroupName_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtGroupName.Focus();
        }

        private void textGroupName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var name = txtGroupName.Text;
            var placeholderGroupName = textGroupName;

            if (!string.IsNullOrEmpty(name) && name.Length >= 0)
            {
                placeholderGroupName.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderGroupName.Visibility = Visibility.Visible;
            }

            if (!IsLettersOnly(name))
            {
                if (string.IsNullOrWhiteSpace(name)) return;
                txtAge.Clear();
                MessageBox.Show("Naziv grupe može sadržavati samo slova!");
                return;
            }
        }

        //Age
        private void txtAge_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtAge.Focus();
        }

        private void textAge_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var age = txtAge.Text;
            var placeholderAge = textAge;

            if (!string.IsNullOrEmpty(age) && age.Length >= 0)
            {
                placeholderAge.Visibility = Visibility.Collapsed;
            } else
            {
                placeholderAge.Visibility = Visibility.Visible;
            }

            if (!IsNumbersAndMinusOnly(age))
            {
                if (string.IsNullOrWhiteSpace(age)) return;
                txtAge.Clear();
                MessageBox.Show("Dobna skupina može sadržavati samo brojeve i znak '-'!");
                return;
            }
        }

        //add new group btn

        private void btnAddNewGroup_Click(object sender, RoutedEventArgs e)
        {

        }

        private bool IsNumbersAndMinusOnly(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c) && c != '-')
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsLettersOnly(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == '-');
        }
    }
}
