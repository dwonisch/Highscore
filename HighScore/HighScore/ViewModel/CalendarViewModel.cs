using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HighScore.ViewModel {
    public class CalendarViewModel : SaveableViewModel {
        private Data.DataService database;

        public CalendarViewModel() {
            this.database = MainViewModel.Database.Value;
            DaySelected = new RelayCommand<DayViewModel>(c => {
                ViewModelLocator.MainViewModel.ChangeView(c);
            });

            Configuration = new HighScoreConfiguration();

            Days = new ObservableCollection<DayViewModel>();


            DateTime countDate = Configuration.StartDate;
            while (countDate.Date <= Configuration.EndDate) {
                var day = new DayViewModel(countDate, database);
                Days.Add(day);

                if (countDate.Date == DateTime.Now.AddDays(2).Date)
                    SelectedDay = day;
                
                countDate = countDate.AddDays(1);
            }
        }

        private HighScoreConfiguration Configuration { get; set; }
        public ObservableCollection<DayViewModel> Days { get; private set; }
        public DayViewModel SelectedDay { get; set; }
        public ICommand DaySelected { get; set; }

        public override void Save() {
        }
    }
}
