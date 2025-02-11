using System.Windows;
using Jg.wpf.core.Service;

namespace RePKG_WPF2
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceManager.Init(Application.Current.Dispatcher);

            base.OnStartup(e);
        }
    }
}
