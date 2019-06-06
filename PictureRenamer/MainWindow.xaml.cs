using mUserControl_BSC_dll.UserControls;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_rename_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(TB_path.Text))
            {
                MessageBox.Show("Invalid path!");
                return;
            }
            int index = 0;
            DirectoryInfo d = new DirectoryInfo(TB_path.Text);

            DirectoryInfo[] ds = d.GetDirectories();

            foreach (DirectoryInfo _d in ds)
            {
                FileInfo[] files = _d.GetFiles("*.jpg").OrderBy(p => p.CreationTime).ToArray();
                foreach (FileInfo f in files)
                {
                    using (Bitmap b = new Bitmap(f.FullName))
                    {
                        b.Save(TB_path.Text + $"\\Img_{index :D5}.jpg");
                        index++;
                    }
                }
            }
            MessageBox.Show("Done!");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
