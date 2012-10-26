using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighScore.Data
{
    public class Day
    {
        public Day() {
            Scores = new List<Score>();
        }

        public virtual IList<Score> Scores { get; set; }
    }
}
