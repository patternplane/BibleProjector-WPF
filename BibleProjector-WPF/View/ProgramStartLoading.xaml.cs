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

namespace BibleProjector_WPF
{
    /// <summary>
    /// ProgramStartLoading.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProgramStartLoading : Window
    {
        public BitmapImage loadingImage { get; set; }
        const string LOADING_IMAGE_PATH = ".\\programData\\LoadingImage";

        public ProgramStartLoading()
        {
            this.DataContext = this;
            try
            {
                loadingImage = new BitmapImage();
                loadingImage.BeginInit();
                loadingImage.UriSource = new Uri(System.IO.Path.GetFullPath(LOADING_IMAGE_PATH), UriKind.Absolute);
                loadingImage.CacheOption = BitmapCacheOption.OnLoad;
                loadingImage.EndInit();
            }
            catch { }

            InitializeComponent();
        }
    }
}
