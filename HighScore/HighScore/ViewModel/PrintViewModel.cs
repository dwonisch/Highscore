﻿using HighScore.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace HighScore.ViewModel {
    public class PrintViewModel : SaveableViewModel {
        public PrintViewModel() {
            Date = DateTime.Now;
            Scores = new ObservableCollection<PrintViewModelItem>();
        }

        public PrintViewModel(IEnumerable<ResultScore> scores, string group) : this() {
            Group = group;
            int lastposition = 0;
            int count = 1;
            ResultScore lastItem = null;
            int color = 0;


            foreach (var score in scores.OrderByDescending(s => s.FirstScore).ThenByDescending(s => s.SecondScore).ThenBy(s => s.Player)) {
                string position;
                if (score.Compare(lastItem)) {
                    count++;
                    position = string.Empty;
                } else {
                    lastposition += count;
                    position = lastposition.ToString();
                    count = 1;
                }
                Scores.Add(new PrintViewModelItem(position, score, color %2 == 0 ? Colors.White : Colors.AliceBlue));

                lastItem = score;
                color++;
            }
        }

        public ObservableCollection<PrintViewModelItem> Scores { get; private set; }
        public DateTime Date { get; private set; }
        public string Group { get; protected set; }

        public override void Save() {
        }
    }

    public class PrintDayViewModel : PrintViewModel {
        public PrintDayViewModel(IEnumerable<ResultScore> scores) {
            Group = "Tagessieger";

            int color = 0;
            foreach (var score in scores.OrderBy(s => s.Date)) {
                Scores.Add(new PrintViewModelItem(score.Date.ToString("dd.MM.yyyy"), score, color % 2 == 0 ? Colors.White : Colors.AliceBlue));
                color++;
            }
        }
    }

    public class PrintViewModelItem {
        public PrintViewModelItem(string position, ResultScore score, Color background) {
            Place = position;
            this.score = score;
            Background = background;
        }

        private ResultScore score;

        public string Place { get; private set; }
        public string Player { get { return score.Player; } }
        public string FirstScore { get { return score.FirstScore == 0 ? string.Empty : score.FirstScore.ToString(); } }
        public string SecondScore { get { return score.SecondScore == 0 ? string.Empty : score.SecondScore.ToString(); } }
        public Color Background { get; private set; }
    }
}
