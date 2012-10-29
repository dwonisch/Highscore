using HighScore.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighScore.ViewModel
{
    public class ScoreViewModel
    {
        public ScoreViewModel()
        {
            Score = new Score();
        }

        public ScoreViewModel(Score score, IList<Player> players)
        {
            this.players = new ObservableCollection<Player>(players);
            Score = score;
        }

        public ObservableCollection<Player> players;
        public Score Score { get; set; }

        public Player Name
        {
            get
            {
                if (Score.Player == null)
                    Score.Player = new Player();
                return Score.Player;
            }
            set
            {
                if (Score.Player == null)
                    Score.Player = new Player();
                Score.Player = value;
            }
        }

        public ObservableCollection<Player> Names
        {
            get { return players; }
        }

        public int Count
        {
            get { return Score.Count; }
            set { Score.Count = value; }
        }
        public int HighScore
        {
            get
            {
                var score = Score.Scores.FirstOrDefault();
                if (score != null)
                    return score.Value;
                return 0;
            }
            set {
                while (Score.Scores.Count < 2)
                    Score.Scores.Add(Score.NewHighScore(0));
                
                    Score.Scores[0].Value = value;
            }
        }
        public int SecondScore
        {
            get
            {
                var score = Score.Scores.Skip(1).FirstOrDefault();
                if (score != null)
                    return score.Value;
                return 0;
            }
            set
            {
                while(Score.Scores.Count < 2)
                    Score.Scores.Add(Score.NewHighScore(0));
                
                    Score.Scores[1].Value = value;
            }
        }
    }
}
