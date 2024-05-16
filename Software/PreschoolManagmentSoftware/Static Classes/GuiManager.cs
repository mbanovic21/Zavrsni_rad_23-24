using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PreschoolManagmentSoftware.Static_Classes
{
    internal static class GuiManager
    {
        public static MainWindow MainWindow { get; set; }

        private static UserControl currentContent;
        private static UserControl previousContent;

        public static void OpenContent(UserControl userControl)
        {
            previousContent = MainWindow.contentControl.Content as UserControl;
            MainWindow.contentControl.Content = userControl;
            currentContent = MainWindow.contentControl.Content as UserControl;
        }

        public static void CloseControl()
        {
            OpenContent(previousContent);
        }
    }

}
