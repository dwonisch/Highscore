using GalaSoft.MvvmLight;

namespace HighScore.ViewModel
{
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
    public class MainViewModel : ViewModelBase, IViewService
    {
        public MainViewModel()
        {
            CurrentViewModel = new CalendarViewModel();
        }

        private ViewModelBase currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return currentViewModel;
            }
            set
            {
                if (currentViewModel == value)
                    return;
                currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

        public void ChangeView(ViewModelBase viewModel)
        {
            CurrentViewModel = viewModel;
        }
    }

    public interface IViewService {
        void ChangeView(ViewModelBase viewModel);
    }
}