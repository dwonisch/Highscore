using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HighScore.ViewModel {
    public class ClosingTask : ViewModelBase {
        private Func<string> task;
        private string text;
        private string status;
        private bool working;
        private bool completed;

        public ClosingTask(string text, Func<string> task) {
            this.text = text;
            this.task = task;
        }

        public string Text { get { return text; } }
        public bool Completed {
            get { return completed; }
            set {
                if (completed == value) return;
                completed = value;
                RaisePropertyChanged(() => Completed);
            }
        }
        public string Status {
            get { return status; }
            set {
                if (status == value) return;
                status = value;
                RaisePropertyChanged(() => Status);

            }
        }

        public bool Working {
            get { return working; }
            set {
                if (working == value) return;
                working = value;
                RaisePropertyChanged(() => Working);
            }
        }

        public void Execute() {
            working = true;
            status = task.Invoke();
            working = false;
            completed = true;
        }
    }
}
