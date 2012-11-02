using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HighScore.Data;
using System;
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
            DataService database = new DataService();

            CurrentViewModel = new CalendarViewModel(database);

            MainView = new RelayCommand(new Action(() => { CurrentViewModel = new CalendarViewModel(database); }));
            Save = new RelayCommand(new Action(() => CurrentViewModel.Save()));
        }

        private SaveableViewModel currentViewModel;

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

        public ICommand MainView {
            get;
            private set;
        }

        public ICommand Save { get; private set; }
    }

    public interface IViewService {
        void ChangeView(SaveableViewModel viewModel);
    }
}