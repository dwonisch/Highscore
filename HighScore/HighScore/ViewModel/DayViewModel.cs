using HighScore.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace HighScore.ViewModel {
    public class DayViewModel : SaveableViewModel {
        private DataService database;
        private List<ScoreViewModel> deletedScores = new List<ScoreViewModel>();

        public DayViewModel() {
            Scores = new ObservableCollection<ScoreViewModel>();
            Scores.Add(new ScoreViewModel());
            Scores.CollectionChanged += Scores_CollectionChanged;


        }

        void Scores_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Remove) {
                deletedScores.AddRange(e.OldItems.OfType<ScoreViewModel>());
            }

            RaisePropertyChanged(() => ScoreCount);
            RaisePropertyChanged(() => PlayerCount);
        }

        public DayViewModel(DateTime date, DataService database) {
            Date = date.Date;
            this.database = database;
            Scores = new ObservableCollection<ScoreViewModel>();
            Scores.CollectionChanged += Scores_CollectionChanged;

            foreach (var score in database.GetScores(date)) {
                Scores.Add(new ScoreViewModel(score));
            }

            if(Scores.Count == 0)
                Scores.Add(new ScoreViewModel());
        }

        public DateTime Date { get; set; }

        public ObservableCollection<ScoreViewModel> Scores { get; private set; }

        public int ScoreCount { get { return Scores.Sum(s => s.Count); } }
        public int PlayerCount { get { return Scores.GroupBy(s => s.Name).Count(); } }
        public double Money { get { return ScoreCount * 1.50;}}

        public override void Save() {
            database.DeleteScores(deletedScores.Select(s => s.Score));
            deletedScores.Clear();

            foreach (var score in Scores) {
                score.Date = Date;
            }

            database.SaveScores(Scores.Where(s => !string.IsNullOrEmpty(s.Name)).Select(s => s.Score));
        }
    }
}
