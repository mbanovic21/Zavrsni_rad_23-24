﻿using System;
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
    /// Interaction logic for ucUpload.xaml
    /// </summary>
    public partial class ucUpload : UserControl
    {
        public ucUpload()
        {
            InitializeComponent();
        }


        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(ucUpload));


        public string FileSize
        {
            get { return (string)GetValue(FileSizeProperty); }
            set { SetValue(FileSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileSizeProperty =
            DependencyProperty.Register("FileSize", typeof(string), typeof(ucUpload));



        public int UploadProgress
        {
            get { return (int)GetValue(UploadProgressProperty); }
            set { SetValue(UploadProgressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UploadProgress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UploadProgressProperty =
            DependencyProperty.Register("UploadProgress", typeof(int), typeof(ucUpload));



        public int UploadSpeed
        {
            get { return (int)GetValue(UploadSpeedProperty); }
            set { SetValue(UploadSpeedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UploadSpeed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UploadSpeedProperty =
            DependencyProperty.Register("UploadSpeed", typeof(int), typeof(ucUpload));



        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var parentItemsControl = ItemsControl.ItemsControlFromItemContainer(this);
            if (parentItemsControl != null)
            {
                var item = parentItemsControl.ItemContainerGenerator.ItemFromContainer(this);
                parentItemsControl.Items.Remove(item);
            } else
            {
                MessageBox.Show("Error occured! Please select a photo to remove!");
            }
        }
    }
}
