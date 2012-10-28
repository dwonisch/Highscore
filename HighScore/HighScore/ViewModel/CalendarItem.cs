using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HighScore.ViewModel
{
    public class CalendarItem : ViewModelBase
    {
        public CalendarItem()
        {
        }

        public CalendarItem(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; set; }
    }
}
