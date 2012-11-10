using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HighScore.Data {
    public class ResultScore {


        public ResultScore(string player, int firstScore, int secondScore) {
            Player = player;
            FirstScore = firstScore;
            SecondScore = secondScore;
        }
        public ResultScore(DateTime date, string player, int firstScore, int secondScore) : this(player, firstScore, secondScore) {
            Date = date;
        }

        public string Player { get; private set; }
        public int FirstScore { get; private set; }
        public int SecondScore { get; private set; }
        public DateTime Date { get; private set; }

        public bool Compare(ResultScore score){
            if (score == null)
                return false;
            return FirstScore == score.FirstScore && SecondScore == score.SecondScore;
        }
    }
}
