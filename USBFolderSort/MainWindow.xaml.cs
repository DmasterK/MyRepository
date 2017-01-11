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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace USBFolderSort
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loadRemovableDrives();
        }

        private void btn_refresh_devices_Click(object sender, RoutedEventArgs e)
        {
            btn_refresh_devices.IsEnabled = false;
            loadRemovableDrives();
            btn_refresh_devices.IsEnabled = true;
        }

        private void loadRemovableDrives()
        {
            emptyDrivesList();
            var allDrives = DriveInfo.GetDrives().Where(w=>w.DriveType.Equals(DriveType.Removable) && w.IsReady == true);
            foreach (var d in allDrives)
            {
                lv_all_drives.Items.Add(d.ToString());
            }
        }

        private void emptyDrivesList()
        {
            var cnt = lv_all_drives.Items.Count;
            for (int i = 0; i < cnt; i++)
            {
                lv_all_drives.Items.RemoveAt(i);
            }
        }

        private void btn_sort_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
