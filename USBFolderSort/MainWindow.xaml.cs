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
            var allDrives = DriveInfo.GetDrives().Where(w => w.DriveType.Equals(DriveType.Removable) && w.IsReady == true);
            foreach (var d in allDrives)
            {
                lv_all_drives.Items.Add(d.Name + d.VolumeLabel);
            }
            if (lv_all_drives.Items.Count > 0)
            {
                lv_all_drives.SelectedItems.Add(lv_all_drives.Items.GetItemAt(0));
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
            btn_sort.IsEnabled = false;
            sortDrive();
            btn_sort.IsEnabled = true;
        }

        private void sortDrive()
        {
            try
            {
                var usbDrive = lv_all_drives.SelectedValue.ToString();
                usbDrive = usbDrive.Split(':')[0];
                usbDrive += ":\\";

                DirectoryInfo dirInfo = new DirectoryInfo(usbDrive);

                if (dirInfo.GetDirectories().Count() > 1)
                {
                    var allDirectories = dirInfo.GetDirectories();
                    var tmpDirectory = Guid.NewGuid();
                    string newPath = usbDrive + tmpDirectory;
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                        foreach (DirectoryInfo d in allDirectories)
                        {
                            if (!d.Name.Contains("System Volume Information"))
                            {
                                var sourcePath = System.IO.Path.Combine(usbDrive, d.Name);
                                var destinationPath = System.IO.Path.Combine(newPath, d.Name);
                                Directory.Move(sourcePath, destinationPath);
                            }
                        }

                        foreach (DirectoryInfo d in allDirectories.OrderBy(o => o.FullName))
                        {
                            if (!d.Name.Contains("System Volume Information"))
                            {
                                var sourcePath = System.IO.Path.Combine(newPath, d.Name);
                                var destinationPath = System.IO.Path.Combine(usbDrive, d.Name);
                                Directory.Move(sourcePath, destinationPath);

                                Directory.SetCreationTimeUtc(destinationPath, DateTime.UtcNow);
                                Directory.SetLastAccessTimeUtc(destinationPath, DateTime.UtcNow);
                                Directory.SetLastWriteTimeUtc(destinationPath, DateTime.UtcNow);
                            }                            
                        }
                        Directory.Delete(newPath, true);
                    }
                }
                lbl_status.Background = Brushes.Green;
            }
            catch (System.IO.IOException ex)
            {
                lbl_status.Background = Brushes.Red;
            }
        }
    }
}
