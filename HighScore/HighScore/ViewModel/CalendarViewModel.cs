using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HighScore.ViewModel {
    public class CalendarViewModel : SaveableViewModel {
        private Data.DataService database;

        public CalendarViewModel(Data.DataService database) {
            this.database = database;
            DaySelected = new RelayCommand<DayViewModel>(c => {
                ViewModelLocator.MainViewModel.ChangeView(c);
            });

            Configuration = new HighScoreConfiguration();

            Days = new ObservableCollection<DayViewModel>();


            DateTime countDate = Configuration.StartDate;
            while (countDate.Date <= Configuration.EndDate) {
                Days.Add(new DayViewModel(countDate, database));
                countDate = countDate.AddDays(1);
            }
        }

        private HighScoreConfiguration Configuration { get; set; }
        public ObservableCollection<DayViewModel> Days { get; private set; }
        public ICommand DaySelected { get; set; }

        public override void Save() {
        }
    }
}
