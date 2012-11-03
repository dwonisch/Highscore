using GalaSoft.MvvmLight;
using HighScore.Data;
using System;
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
                return score;
            }
        }

        private string name;
        private bool female;
        private bool child;

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
                RaisePropertyChanged(() => child);
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
                return Score.FirstScore;
            }
            set {
                if (Score.FirstScore == value) return;

                Score.FirstScore = value;
                RaisePropertyChanged(() => HighScore);
            }
        }
        public int SecondScore {
            get {
                return Score.SecondScore;
            }
            set {
                if (Score.SecondScore == value) return;

                Score.SecondScore = value;
                RaisePropertyChanged(() => SecondScore);
            }
        }

        public ICommand MainView { get { return MainViewModel.MainViewCommand; } }
    }
}
