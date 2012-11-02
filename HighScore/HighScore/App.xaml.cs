using HighScore.Data;
using System.Windows;

namespace HighScore {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            DataService data = new DataService();

            base.OnStartup(e);
        }
    }
}
