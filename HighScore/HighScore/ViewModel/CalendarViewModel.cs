using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HighScore.ViewModel
{
    public class CalendarViewModel : ViewModelBase
    {
        public CalendarViewModel() {
            DaySelected = new RelayCommand<CalendarItem>(c => {
                ViewModelLocator.MainViewModel.ChangeView(c);
            });
            
            Configuration = new HighScoreConfiguration();

            Days = new ObservableCollection<CalendarItem>();

            DateTime countDate = Configuration.StartDate;
            while (countDate.Date <= Configuration.EndDate)
            {
                Days.Add(new CalendarItem(countDate));
                countDate = countDate.AddDays(1);
            }
        }

        private HighScoreConfiguration Configuration { get; set; }
        public ObservableCollection<CalendarItem> Days { get; private set; }
        public ICommand DaySelected { get; set; }
    }
}
