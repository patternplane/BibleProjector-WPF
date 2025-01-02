using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class ProgramStartLoading : Window, INotifyPropertyChanged
    {
        public bool onReady { get; private set; } = false;

        public BitmapImage loadingImage { get; set; } = null;
        const string LOADING_IMAGE_PATH = ".\\programData\\LoadingImage";

        public string loadingText { get; private set; } = "";
        private const double MAX_PROGRESS_BAR_LENGTH = 94.0;
        public double loadingBarLength { get; private set; } = 0;
        public string Version { get; } = "v25.1.1";

        public ProgramStartLoading()
        {
            this.DataContext = this;
            try
            {
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(System.IO.Path.GetFullPath(LOADING_IMAGE_PATH), UriKind.Absolute);
                b.CacheOption = BitmapCacheOption.OnLoad;
                b.EndInit();

                loadingImage = b;
            }
            catch {

            }

            InitializeComponent();
        }

        public void setLoadingState(string text, int percent)
        {
            this.loadingText = text;
            OnPropertyChanged(nameof(loadingText));
            this.loadingBarLength = MAX_PROGRESS_BAR_LENGTH * percent / 100.0;
            OnPropertyChanged(nameof(loadingBarLength));
        }

        // ================ Initializer ================ 

        private void EH_Loaded(object sender, EventArgs e)
        {
            onReady = true;
        }

        public void InitializeDone(ViewModel.VMMainWindow MainWindowViewModel)
        {
            new MainWindow(MainWindowViewModel).Show();
            this.Close();
        }

        // ================================================ 

        // 원칙 위반 : View와 ViewModel이 혼합된 상태

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
