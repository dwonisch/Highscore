using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighScore.Data
{
    public class Score
    {
        public Score() {
            Values = new List<ScoreValue>();
        }

        private DateTime date;
            
        public virtual int Id { get; set; }
        public virtual DateTime Date { get { return date; } set { date = value.Date; } }
        public virtual Player Player { get; set; }
        public virtual int Count { get; set; }
        public virtual IList<ScoreValue> Values { get; set; }

        public virtual ScoreValue CreateValue() {
            return new ScoreValue() { Score = this };
        }
    }
}
