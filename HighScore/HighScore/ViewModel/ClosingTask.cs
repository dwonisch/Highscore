using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HighScore.ViewModel {
    public class ClosingTask : ViewModelBase {
        private Func<string> task;
        private string text;
        private bool working;
        private bool completed;
        private bool failed;

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

        public bool Failed {
            get { return failed; }
            set {
                if (failed == value) return;
                failed = value;
                RaisePropertyChanged(() => Failed);
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
            Working = true;
            try {
                task.Invoke();
                Completed = true;
            } catch (Exception ex) {
                Failed = true;
            }
            Working = false;
        }
    }
}
