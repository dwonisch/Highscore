using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighScore.Data
{
    public class Score
    {
        private DateTime date;
            
        public virtual int Id { get; set; }
        public virtual DateTime Date { get { return date; } set { date = value.Date; } }
        public virtual Player Player { get; set; }
        public virtual int Count { get; set; }
        public virtual int FirstScore { get; set; }
        public virtual int SecondScore { get; set; }
    }
}
