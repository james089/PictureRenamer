using mUserControl_BSC_dll.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

namespace PictureRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static BackgroundWorker renameRoutine = new BackgroundWorker();
        string path;
        public MainWindow()
        {
            InitializeComponent();
            renameRoutine.DoWork += new DoWorkEventHandler(renameRoutine_doWork);
            renameRoutine.ProgressChanged += new ProgressChangedEventHandler(renameRoutine_ProgressChanged);
            renameRoutine.RunWorkerCompleted += new RunWorkerCompletedEventHandler(renameRoutine_WorkerCompleted);
            renameRoutine.WorkerReportsProgress = true;
            renameRoutine.WorkerSupportsCancellation = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            path = TB_path.Text;
        }

        private void TB_path_TextChanged(object sender, TextChangedEventArgs e)
        {
            path = TB_path.Text;
        }

        private void renameRoutine_doWork(object sender, DoWorkEventArgs e)
        {
            int index = 0;
            int num_images = 0;
            DirectoryInfo d = new DirectoryInfo(path);

            DirectoryInfo[] ds = d.GetDirectories();

            foreach (DirectoryInfo _d in ds)
            {
                FileInfo[] files = _d.GetFiles("*.jpg").OrderBy(p => p.CreationTime).ToArray();
                foreach (FileInfo f in files)
                {
                    using (Bitmap b = new Bitmap(f.FullName))
                    {
                        num_images++;
                    }
                }
            }

            foreach (DirectoryInfo _d in ds)
            {
                FileInfo[] files = _d.GetFiles("*.jpg").OrderBy(p => p.CreationTime).ToArray();
                foreach (FileInfo f in files)
                {
                    using (Bitmap b = new Bitmap(f.FullName))
                    {
                        string dateStamp = DateTime.Now.ToString("yyyyMMdd");
                        string timeStamp = DateTime.Now.ToString("HHmmss");
                        b.Save(path + $"\\Img_{dateStamp}_{timeStamp}_{index:D5}.jpg");
                        index++;
                        renameRoutine.ReportProgress((int)((index / (float)num_images) * 100));
                    }
                }
            }
        }

        private void renameRoutine_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            lbl_progress.Text = e.ProgressPercentage + "%";
        }

        private void renameRoutine_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lbl_status.Content = "Done!";
        }

        private void Btn_rename_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(TB_path.Text))
            {
                MessageBox.Show("Invalid path!");
                return;
            }

            if (!renameRoutine.IsBusy)
            {
                lbl_status.Content = "Renaming...";
                renameRoutine.RunWorkerAsync();
            }

        }

    }
}
