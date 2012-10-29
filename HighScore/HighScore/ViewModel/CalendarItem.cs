using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HighScore.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HighScore.ViewModel
{
    public class CalendarItem : SaveableViewModel
    {
        private DataService database;

        public CalendarItem()
        {
            Scores = new ObservableCollection<ScoreViewModel>();
        }

        public CalendarItem(DateTime date, DataService database)
            : this()
        {
            Date = date;
            this.database = database;

            foreach (var score in database.GetScores(date)) {
                Scores.Add(new ScoreViewModel(score, database.GetPlayers()));
            }
        }

        public DateTime Date { get; set; }

        public ObservableCollection<ScoreViewModel> Scores { get; private set; }

        public override void Save()
        {
            database.SaveScores(Scores.Select(s => s.Score));
        }
    }
}
