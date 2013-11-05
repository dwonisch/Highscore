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
            stream.WriteString(string.Format("<html><head><title>Dartturnier 2013 - {0}</title>", print.Group));
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
            stream.WriteString(string.Format("<div style=\"width: 25em; float:left; position: relative;\"><h3>{0}</h3></div><div style=\"float:right; width=400px;\"><p>powered by:</p><img src=\"logoruki.png\" /></div>", WebUtility.HtmlEncode("ÖKB Gosdorf Dartturnier")));
            stream.WriteString(@"<div style=""float:left; width: 30em; margin-left: auto;  margin-right: auto;""><div>");
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
        private ICommand MainViewCommand;
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

        private ICommand SaveCommand;
        private ICommand PrintMaleCommand;
        private ICommand PrintChildrenCommand;
        private ICommand PrintFemaleCommand;
        private ICommand PrintDayCommand;
        private ICommand UploadCommand;
        private ICommand ShotsCommand;
        private ICommand ShotsPerPlayerCommand;
        private RelayCommand<CancelEventArgs> ClosingCommand;

        public ICommand MainView {
            get {
                if (MainViewCommand == null) {
                    MainViewCommand = new RelayCommand(new Action(() => { CurrentViewModel = new CalendarViewModel(); }));
                }
                return MainViewCommand;
            }
        }

        public ICommand Save {
            get {
                if (SaveCommand == null) {
                    SaveCommand = new RelayCommand(new Action(() => CurrentViewModel.Save()));
                }
                return SaveCommand;
            }
        }
        public ICommand PrintMale {
            get {
                if (PrintMaleCommand == null) {
                    PrintMaleCommand = new RelayCommand(new Action(() => {
                        CurrentViewModel = new PrintViewModel(Database.Value.GetHighscores(false), "Herren");
                    }));
                }
                return PrintMaleCommand;
            }
        }
        public ICommand PrintFemale {
            get {
                if (PrintFemaleCommand == null) {
                    PrintFemaleCommand = new RelayCommand(new Action(() => {
                        CurrentViewModel = new PrintViewModel(Database.Value.GetHighscores(true), "Damen");
                    }));
                }
                return PrintFemaleCommand;
            }
        }
        public ICommand PrintChildren {
            get {
                if (PrintChildrenCommand == null) {
                    PrintChildrenCommand = new RelayCommand(new Action(() => {
                        CurrentViewModel = new PrintViewModel(Database.Value.GetChildrenHighscores(), "Kinder");
                    }));
                }
                return PrintChildrenCommand;
            }
        }

        public ICommand PrintDay {
            get {
                if (PrintDayCommand == null) {
                    PrintDayCommand = new RelayCommand(new Action(() => {
                        CurrentViewModel = new PrintDayViewModel(Database.Value.GetDayScores());
                    }));
                }
                return PrintDayCommand;
            }
        }

        public ICommand Shots {
            get {
                if (ShotsCommand == null) {
                    ShotsCommand = new RelayCommand(new Action(() => {
                        CurrentViewModel = new PrintDayViewModel(Database.Value.GetDayShots());
                    }));
                }
                return ShotsCommand;
            }
        }

        public ICommand ShotsPerPlayer {
            get {
                if (ShotsPerPlayerCommand == null) {
                    ShotsPerPlayerCommand = new RelayCommand(new Action(() => {
                        CurrentViewModel = new PrintViewModel(Database.Value.GetPlayerShots(), "Schützen");
                    }));
                }
                return ShotsPerPlayerCommand;
            }
        }

        public ICommand Upload {
            get {

                if (UploadCommand == null) {
                    UploadCommand = new RelayCommand(new Action(() => {

                        try {
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
                        } catch (Exception ex) {
                            MessageBox.Show(ex.Message);
                        }
                    }));
                }
                return UploadCommand;
            }
        }
        public RelayCommand<CancelEventArgs> Closing {
            get {
                if (ClosingCommand == null) {
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
                }
                return ClosingCommand;
            }
        }
    }

    public static class StreamExtensions {
        public static void WriteString(this Stream stream, string s) {
            var bytes = Encoding.UTF8.GetBytes(s);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}