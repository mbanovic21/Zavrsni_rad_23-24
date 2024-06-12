using BusinessLogicLayer.DBServices;
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

namespace PreschoolManagmentSoftware.UserControls.PreschoolYear
{
    /// <summary>
    /// Interaction logic for ucAddNewGroup.xaml
    /// </summary>
    public partial class ucAddNewGroup : UserControl
    {
        private GroupServices _groupServices = new GroupServices();
        private ucAddPreschoolYear _prevoiusControl { get; set; }
        public ucAddNewGroup(ucAddPreschoolYear ucAddPreschoolYear)
        {
            InitializeComponent();
            _prevoiusControl = ucAddPreschoolYear;
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
        private void textGroupName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            txtGroupName.Focus();
        }

        private void txtGroupName_TextChanged(object sender, TextChangedEventArgs e)
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
        private void textAge_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            txtAge.Focus();
        }

        private void txtAge_TextChanged(object sender, TextChangedEventArgs e)
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
        private async void btnAddNewGroup_Click(object sender, RoutedEventArgs e)
        {
            if (!isValidate()) return;

            var gruopName = txtGroupName.Text;
            var age = txtAge.Text;

            var group = new Group
            {
                Name = gruopName,
                Age = age
            };

            var isAdded = await Task.Run(() => _groupServices.AddGroup(group));

            if (isAdded)
            {
                RefreshGUI();
                _prevoiusControl.Groups.Add(group);
                Console.WriteLine(_prevoiusControl.Groups.Count.ToString());
                _prevoiusControl.RefreshGUI();
               
                var result = MessageBox.Show("Grupa uspješno dodana u sustav. Želite li dodati još jednu grupu?", "Dodavanje grupe", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    txtGroupName.Clear();
                    txtAge.Clear();
                } else
                {
                    _prevoiusControl.CloseSidebar();
                }
            } else
            {
                MessageBox.Show("Greška prilikom dodavanja grupe.");
            }
        }


        private bool isValidate()
        {
            var groupName = txtGroupName.Text;
            var age = txtAge.Text;

            if (string.IsNullOrWhiteSpace(groupName) || string.IsNullOrWhiteSpace(age)) return false;

            return true;
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
