using BusinessLogicLayer.DBServices;
using EntityLayer.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PreschoolManagmentSoftware.UserControls.EmailNotifier
{
    /// <summary>
    /// Interaction logic for ucEmailNotifier.xaml
    /// </summary>
    public partial class ucEmailNotifier : UserControl
    {
        List<string> filePaths = new List<string>();
        private ParentServices _parentServices = new ParentServices();
        private UserServices _userServices = new UserServices();
        public ucEmailNotifier()
        {
            InitializeComponent();
        }

        //DataGrids
        private void ucEmailNotifierr_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGUI();
        }

        private async void RefreshGUI()
        {
            dgvParents.ItemsSource = await Task.Run(() => _parentServices.GetAllParents());
            HideColumnsParents();
            dgvEmployees.ItemsSource = await Task.Run(() => _userServices.GetAllUsers());
            HideColumnsEmployees();
        }

        //naslov
        private void textSubject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            txtSubject.Focus();
        }

        private void txtSubject_TextChanged(object sender, TextChangedEventArgs e)
        {
            var subject = txtSubject.Text;
            var subjectPlaceholder = textSubject;

            if (string.IsNullOrEmpty(subject))
            {
                subjectPlaceholder.Visibility = Visibility.Visible;
            } else
            {
                subjectPlaceholder.Visibility = Visibility.Collapsed;
            }
        }

        //poruka
        private void textMessage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rtxtMessage.Focus();
        }

        private void rtxtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            var message = new TextRange(rtxtMessage.Document.ContentStart, rtxtMessage.Document.ContentEnd).Text;
            var placeholderMessage = textMessage;

            if (string.IsNullOrEmpty(message))
            {
                placeholderMessage.Visibility = Visibility.Visible;
            } else
            {
                placeholderMessage.Visibility = Visibility.Collapsed;
            }
        }

        //dokumenti
        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog() { Multiselect = true };
            bool? response = openFileDialog.ShowDialog();
            if (response == true)
            {
                imgUpload.Visibility = Visibility.Collapsed;
                textImageDescription.Visibility = Visibility.Collapsed;
                var files = openFileDialog.FileNames;
                filePaths.AddRange(files);
                foreach (var file in files)
                {
                    var filename = System.IO.Path.GetFileName(file);
                    var fileInfo = new FileInfo(file);
                    var ucUpload = new ucUpload()
                    {
                        FileName = filename,
                        FileSize = string.Format("{0} {1}", ((fileInfo.Length / 1024) + 1).ToString("0.0"), "Kb"),
                        UploadProgress = 100,
                        FilePath = file
                    };
                    UploadingFileList.Items.Add(ucUpload);
                }
                //MessageBox.Show("File paths: " + string.Join(", ", filePaths));
            }
        }

        //btnNotify
        private async void btnNotify_Click(object sender, RoutedEventArgs e)
        {
            var selectedParents = new List<Parent>();
            var selectedEmployees = new List<User>();

            foreach (var item in dgvParents.SelectedItems)
            {
                if (item is Parent parent)
                {
                    selectedParents.Add(parent);
                }
            }

            foreach (var item in dgvEmployees.SelectedItems)
            {
                if (item is User employee)
                {
                    selectedEmployees.Add(employee);
                }
            }

            if (!isValidate()) return;

            var subject = txtSubject.Text;
            var message = GetRichTextBoxContent(rtxtMessage);

            try
            {
                var emailNotifier = await Task.Run(() => new BusinessLogicLayer.EmailServices.EmailNotifier(subject, message, selectedEmployees, selectedParents, filePaths));
                MessageBox.Show("Email uspješno poslan!", "Obavijest", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške prilikom slanja emaila: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HideColumnsParents()
        {
            var columnsToHide = new List<string>
            {
                "Id",
                "PIN",
                "ProfileImage",
                "DateOfBirth",
                "Sex",
                "Children",
            };

            HideColumns(dgvParents, columnsToHide);
        }

        private void HideColumnsEmployees()
        {
            var columnsToHide = new List<string>
            {
                "Id",
                "PIN",
                "ProfileImage",
                "DateOfBirth",
                "Sex",
                "Password",
                "Salt",
                "Id_role",
                "Id_Group",
                "Group",
                "Days"
            };

            HideColumns(dgvEmployees, columnsToHide);
        }

        private void HideColumns(DataGrid dgv, List<string> columnsToHide)
        {
            if (dgv is DataGrid)
            {
                foreach (string columnName in columnsToHide)
                {
                    var column = dgv.Columns.FirstOrDefault(c => c.Header.ToString() == columnName);
                    if (column != null)
                    {
                        column.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private string GetRichTextBoxContent(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            string plainText = textRange.Text;

            // Replace new lines with <br/>
            string htmlText = plainText.Replace("\n", "<br/>").Replace(" ", "&nbsp;");

            return htmlText;
        }


        private bool isValidate()
        {
            var subject = txtSubject.Text;
            var message = new TextRange(rtxtMessage.Document.ContentStart, rtxtMessage.Document.ContentEnd).Text;

            if (string.IsNullOrWhiteSpace(subject))
            {
                MessageBox.Show("Unesite naslov");
                return false;
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show("Unesite poruku.");
                rtxtMessage.Document.Blocks.Clear();
                rtxtMessage.Selection.Select(rtxtMessage.Document.ContentStart, rtxtMessage.Document.ContentStart);
                return false;
            }

            if (dgvEmployees.SelectedItems.Count < 1 && dgvParents.SelectedItems.Count < 1)
            {
                MessageBox.Show("Odaberite primatelja e-pošte!\n");
                return false;
            }

            return true;
        }
    }
}
