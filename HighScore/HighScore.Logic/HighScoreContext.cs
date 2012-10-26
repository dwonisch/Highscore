using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HighScore.Logic
{
    public class HighScoreContext
    {
        public HighScoreContext() {
            Configuration = new HighScoreConfiguration();

            Days = new List<CalendarItem>();

            DateTime countDate = Configuration.StartDate;
            while (countDate.Date <= Configuration.EndDate)
            {
                Days.Add(new CalendarItem(countDate));
                countDate = countDate.AddDays(1);
            }
        }

        private HighScoreConfiguration Configuration { get; set; }
        public List<CalendarItem> Days { get; private set; }
        public ICommand DaySelected { get; set; }
    }
}
