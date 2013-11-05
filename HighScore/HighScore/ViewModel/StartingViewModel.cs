using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HighScore.ViewModel {
    public class StartingViewModel : SaveableViewModel {

        public StartingViewModel() {
            Tasks = new ObservableCollection<ClosingTask>();
            Tasks.Add(new ClosingTask("Datenbank wird geladen", DownloadDatabase));
        }

        public void Execute() {
            Parallel.ForEach(Tasks, t => t.Execute());
            if (Completed != null)
                Completed(this, EventArgs.Empty);
        }

        public event EventHandler Completed;

        private void DownloadDatabase() {
            WebClient request = new WebClient();
            request.Credentials = new NetworkCredential("u52287998-dart", "dartturnier");
            byte[] fileData = request.DownloadData("ftp://www.woni.at/database.db");

            if (File.Exists("database.db"))
                File.Delete("database.db");
            FileStream file = File.Create("database.db");
            file.Write(fileData, 0, fileData.Length);
            file.Close();
        }

        public ObservableCollection<ClosingTask> Tasks { get; private set; }

        public override void Save() {
        }
    }
}
