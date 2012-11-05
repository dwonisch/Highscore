using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HighScore.Data;
using System;
using System.Linq;
using System.Windows.Input;

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
    public class MainViewModel : ViewModelBase, IViewService {
        public MainViewModel() {
            CurrentViewModel = new CalendarViewModel();

            MainViewCommand = new RelayCommand(new Action(() => { CurrentViewModel = new CalendarViewModel(); }));
            Save = new RelayCommand(new Action(() => CurrentViewModel.Save()));
            PrintMale = new RelayCommand(new Action(() => {
                CurrentViewModel = new PrintViewModel(Database.Value.GetHighscores(false), "Herren");
            }));
            PrintFemale = new RelayCommand(new Action(() => {
                CurrentViewModel = new PrintViewModel(Database.Value.GetHighscores(true), "Damen");
            }));
        }

        private SaveableViewModel currentViewModel;
        internal static ICommand MainViewCommand;

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


        private double scaleValue = 1;
        public double ScaleValue { 
            get { return scaleValue; }
            set {
                if (scaleValue == value) return;
                scaleValue = value;
                RaisePropertyChanged(() => ScaleValue);
            }
        }

        public ICommand MainView { get { return MainViewCommand;}}
        public ICommand Save { get; private set; }
        public ICommand PrintMale { get; private set; }
        public ICommand PrintFemale { get; private set; }
    }

    public interface IViewService {
        void ChangeView(SaveableViewModel viewModel);
    }
}