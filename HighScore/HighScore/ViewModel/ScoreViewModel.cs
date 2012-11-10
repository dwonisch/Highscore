using GalaSoft.MvvmLight;
using HighScore.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace HighScore.ViewModel {
    public class ScoreViewModel : ViewModelBase {

        public ScoreViewModel() {
            dataservice = MainViewModel.Database.Value;
            this.players = new ObservableCollection<string>(dataservice.GetPlayers());

            this.score = new Score();
        }

        public ScoreViewModel(Score score)
            : this() {
            this.score = score;

            if (score.Values.Count > 0)
                score1 = score.Values[0].Value;

            if (score.Values.Count > 1)
                score2 = score.Values[1].Value;

            SetPlayerProperties(score.Player);
        }

        private void SetPlayerProperties(Player player) {
            name = player.Name;
            Child = player.Child;
            Female = player.Female;
        }

        public ObservableCollection<string> players;
        private Score score;
        private DataService dataservice;

        public Score Score {
            get {
                Player player = dataservice.GetPlayer(Name).Item2;
                player.Female = female;
                player.Child = child;
                score.Player = player;

                if (score.Values.Count == 0)
                    score.Values.Add(score.CreateValue());
                if (score.Values.Count == 1)
                    score.Values.Add(score.CreateValue());

                score.Values[0].Value = score1;
                score.Values[1].Value = score2;

                return score;
            }
        }

        private string name;
        private bool female;
        private bool child;
        private int score1;
        private int score2;

        public string Name {
            get {
                return name;
            }
            set {
                if (name == value) return;
                name = value;

                var player = dataservice.GetPlayer(name);
                SetPlayerProperties(player.Item2);
                score.Player = player.Item2;

                RaisePropertyChanged(() => Name);
            }
        }

        public bool Female {
            get { return female; }
            set {
                if (female == value) return;
                female = value;
                RaisePropertyChanged(() => Female);
            }
        }

        public bool Child {
            get { return child; }
            set {
                if (child == value) return;
                child = value;
                RaisePropertyChanged(() => Child);
            }
        }

        public DateTime Date {
            get { return Score.Date; }
            set { Score.Date = value; }
        }

        public ObservableCollection<string> Names {
            get { return players; }
        }

        public int Count {
            get { return Score.Count; }
            set {
                if (Score.Count == value) return;
                Score.Count = value;
                RaisePropertyChanged(() => Count);
            }
        }
        public int HighScore {
            get {
                return score1;
            }
            set {
                if (score1 == value) return;

                score1 = value;
                RaisePropertyChanged(() => HighScore);
            }
        }
        public int SecondScore {
            get {
                return score2;
            }
            set {
                if (score2 == value) return;

                score2 = value;
                RaisePropertyChanged(() => SecondScore);
            }
        }
    }
}
