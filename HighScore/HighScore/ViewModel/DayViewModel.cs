using HighScore.Data;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HighScore.ViewModel {
    public class DayViewModel : SaveableViewModel {
        private DataService database;

        public DayViewModel() {
            Scores = new ObservableCollection<ScoreViewModel>();
        }

        public DayViewModel(DateTime date, DataService database) {
            Date = date.Date;
            this.database = database;
            Scores = new ObservableCollection<ScoreViewModel>();

            foreach (var score in database.GetScores(date)) {
                Scores.Add(new ScoreViewModel(score, database.GetPlayers()));
            }
        }

        public DateTime Date { get; set; }

        public ObservableCollection<ScoreViewModel> Scores { get; private set; }

        public override void Save() {
            foreach (var score in Scores) {
                score.Date = Date;
            }

            database.SaveScores(Scores.Where(s => !string.IsNullOrEmpty(s.Name)).Select(s => s.Score));
        }
    }
}
