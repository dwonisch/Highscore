using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighScore.Logic
{
    public class CalendarItem
    {
        public CalendarItem(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; set; }
    }
}
