using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using HighScore.Data;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace HighScore.ViewModel {
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase {
        public MainViewModel() {
            DispatcherHelper.Initialize();

            var start = new StartingViewModel();
            start.Completed += (sender, args) => { 
                CurrentViewModel = new CalendarViewModel();
                settings = Database.Value.GetSettings();
                RaisePropertyChanged(() => ScaleValue);
            };
            CurrentViewModel = start;
            Task.Factory.StartNew(() => start.Execute());

            MainViewCommand = new RelayCommand(new Action(() => { CurrentViewModel = new CalendarViewModel(); }));
            Save = new RelayCommand(new Action(() => CurrentViewModel.Save()));
            PrintMale = new RelayCommand(new Action(() => {
                CurrentViewModel = new PrintViewModel(Database.Value.GetHighscores(false), "Herren");
            }));
            PrintFemale = new RelayCommand(new Action(() => {
                CurrentViewModel = new PrintViewModel(Database.Value.GetHighscores(true), "Damen");
            }));
            PrintDay = new RelayCommand(new Action(() => {
                CurrentViewModel = new PrintDayViewModel(Database.Value.GetDayScores());
            }));
            ClosingCommand = new RelayCommand<CancelEventArgs>(new Action<CancelEventArgs>((args) => {
                if (!canClose) {
                    Database.Value.SaveSettings(settings);

                    var viewmodel = new ClosingViewModel();
                    viewmodel.Completed += viewmodel_Completed;
                    CurrentViewModel = viewmodel;

                    Task.Factory.StartNew(() => viewmodel.Execute());

                    args.Cancel = true;
                }
            }));
            Upload = new RelayCommand(new Action(() => {

                if (!Directory.Exists("html"))
                    Directory.CreateDirectory("html");
                if (!File.Exists("html\\index.html"))
                    File.Create("html\\index.html");
                if (File.Exists("html\\men.html"))
                    File.Delete("html\\men.html");
                if (!File.Exists("html\\women.html"))
                    File.Create("html\\women.html");




                using (var stream = File.Create("html\\men.html")) {
                    var print = new PrintViewModel(Database.Value.GetHighscores(false), "Herren");
                    CreateHTML(print, stream);
                }
                UploadDatabase("men.html");

                using (var stream = File.Create("html\\women.html")) {
                    var print = new PrintViewModel(Database.Value.GetHighscores(true), "Damen");
                    CreateHTML(print, stream);

                }
                UploadDatabase("women.html");

                using (var stream = File.Create("html\\index.html")) {
                    var print = new PrintDayViewModel(Database.Value.GetDayScores());
                    CreateHTML(print, stream);

                }
                UploadDatabase("index.html");

            }));
        }

        private string UploadDatabase(string newFileName) {
            var ftprequest = (FtpWebRequest)WebRequest.Create("ftp://www.woni.at/score/" + newFileName);

            ftprequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftprequest.Credentials = new NetworkCredential("u52287998-dart", "dartturnier");


            using (var reader = new StreamReader("html\\" + newFileName)) {
                byte[] fileContents = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                ftprequest.ContentLength = fileContents.Length;

                using (var writer = ftprequest.GetRequestStream()) {
                    writer.Write(fileContents, 0, fileContents.Length);
                }
            }

            FtpWebResponse response = (FtpWebResponse)ftprequest.GetResponse();

            var result = response.StatusDescription;

            response.Close();

            return result;
        }

        private static void CreateHTML(PrintViewModel print, FileStream stream) {
            stream.WriteString(string.Format("<html><head><title>Dartturnier 2012 - {0}</title>", print.Group));
            stream.WriteString(@"<style>

#col1 {
	float:left;
	width:6em;
	position:relative;
	overflow:hidden;
}
#col2 {
	float:left;
	width:15em;
	position:relative;
	overflow:hidden;
}
#col3 {
	float:left;
	width:2em;
	position:relative;
	overflow:hidden;
}
#col4 {
	float:left;
	width:2em;
	position:relative;
	overflow:hidden;
}

</style></head><body>");
            stream.WriteString("<div style=\"  width: 25em;  margin-left: auto;  margin-right: auto;\">");
            stream.WriteString(@"<div>");
            stream.WriteString(@"<a href=""index.html"">Tagessieger</a><span> </span>");
            stream.WriteString(@"<a href=""men.html"">Herren</a><span> </span>");
            stream.WriteString(@"<a href=""women.html"">Damen</a><p /><p />");
            stream.WriteString(@"</div>");



            foreach (var score in print.Scores) {
                stream.WriteString(string.Format("<div><div id=\"col1\">{0}</div><div id=\"col2\">{1}</div><div id=\"col3\">{2}</div><div id=\"col4\">{3}</div><div style=\"clear:both;\"></div></div>", score.Place, WebUtility.HtmlEncode(score.Player), score.FirstScore, score.SecondScore, score.Background));
            }

            stream.WriteString("</div></body></html>");
        }

        void viewmodel_Completed(object sender, EventArgs e) {
            canClose = true;

            DispatcherHelper.CheckBeginInvokeOnUI(() => Application.Current.Shutdown());
        }

        private SaveableViewModel currentViewModel;
        internal static ICommand MainViewCommand;
        private bool canClose;
        private Settings settings;

        internal static Lazy<DataService> Database = new Lazy<DataService>(new Func<DataService>(() => new DataService()));

        public SaveableViewModel CurrentViewModel {
            get {
                return currentViewModel;
            }
            set {
                if (currentViewModel == value)
                    return;
                if (currentViewModel != null) {
                    currentViewModel.Save();
                }

                currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

        public void ChangeView(SaveableViewModel viewModel) {
            CurrentViewModel = viewModel;
        }

        public double ScaleValue {
            get { return settings == null ? 1 : settings.Zoom; }
            set {
                if (settings != null) {
                    if (settings.Zoom == value) return;
                    settings.Zoom = value;
                    RaisePropertyChanged(() => ScaleValue);
                }
            }
        }

        public ICommand MainView { get { return MainViewCommand; } }
        public ICommand Save { get; private set; }
        public ICommand PrintMale { get; private set; }
        public ICommand PrintFemale { get; private set; }
        public ICommand PrintDay { get; private set; }
        public ICommand Upload { get; private set; }
        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }
    }

    public static class StreamExtensions {
        public static void WriteString(this Stream stream, string s) {
            var bytes = Encoding.UTF8.GetBytes(s);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}