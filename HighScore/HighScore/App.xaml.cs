using HighScore.Data;
using System.Windows;

namespace HighScore {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            DispatcherUnhandledException += (sender, args) => MessageBox.Show(args.Exception.ToString());
               
            base.OnStartup(e);
        }
    }
}
