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
            Scores = new List<HighScore>();
        }

        private DateTime date;
            
        public virtual int Id { get; set; }
        public virtual DateTime Date { get { return date; } set { date = value.Date; } }
        public virtual int Count { get; set; }
        public virtual IList<HighScore> Scores { get; set; }
        public virtual Player Player { get; set; }

        public virtual HighScore NewHighScore(int score) { 
            return new HighScore() {Score = this, Value = score};
        }
    }
}
