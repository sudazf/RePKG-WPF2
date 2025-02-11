using System.IO;
using RePKG_WPF2.Utility;
using System.Windows;
using RePKG_WPF2.ViewModel;

namespace RePKG_WPF2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();

            FileReleaseUtility.ReleaseTo(Path.GetTempPath());
        }
    }
}
