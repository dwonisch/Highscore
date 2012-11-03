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
            this.players = new ObservableCollection<string>(new DataService().GetPlayers());
            
            Score = new Score();
        }

        public ScoreViewModel(Score score) : this() {
            Score = score;
        }

        public ObservableCollection<string> players;
        public Score Score { get; set; }

        public string Name {
            get {
                return Score.Player;
            }
            set {
                if (Score.Player == value) return;
                Score.Player = value;
                OnNotifyPropertyChanged("Name");
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
                OnNotifyPropertyChanged("Count");
            }
        }
        public int HighScore {
            get {
                return Score.FirstScore;
            }
            set {
                if (Score.FirstScore == value) return;

                Score.FirstScore = value;
                OnNotifyPropertyChanged("HighScore");
            }
        }
        public int SecondScore {
            get {
                return Score.SecondScore;
            }
            set {
                if (Score.SecondScore == value) return;

                Score.SecondScore = value;
                OnNotifyPropertyChanged("SecondScore");
            }
        }

        public ICommand MainView { get { return MainViewModel.MainViewCommand; } }


        private void OnNotifyPropertyChanged(string property) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public new event PropertyChangedEventHandler PropertyChanged;
    }
}
