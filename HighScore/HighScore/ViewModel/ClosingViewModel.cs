using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HighScore.ViewModel {
    public class ClosingViewModel : SaveableViewModel {

        public ClosingViewModel() {
            Tasks = new ObservableCollection<ClosingTask>();
            Tasks.Add(new ClosingTask("Datenbanken werden gesichert", UploadDatabase));
            Tasks.Add(new ClosingTask("Warten", () => {
                Thread.Sleep(5000);
            }));
        }

        public void Execute() {
            Parallel.ForEach(Tasks, t => t.Execute());
            if (Completed != null)
                Completed(this, EventArgs.Empty);
        }

        public event EventHandler Completed;

        private void UploadDatabase() {
            UploadFile("database.db", string.Format("{0:yyyy-MM-dd-HHmmss}-database.db", DateTime.Now));
            UploadFile("database.db", "database.db");
        }

        private static string UploadFile(string fileName, string newFileName) {
            var ftprequest = (FtpWebRequest)WebRequest.Create("ftp://www.woni.at/" + newFileName);

            ftprequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftprequest.Credentials = new NetworkCredential("u52287998-dart", "dartturnier");

            using (var writer = ftprequest.GetRequestStream()) {
                var bytes = File.ReadAllBytes(fileName);
                writer.Write(bytes, 0, bytes.Length);
            }

            FtpWebResponse response = (FtpWebResponse)ftprequest.GetResponse();

            var result = response.StatusDescription;

            response.Close();

            return result;
        }


        public ObservableCollection<ClosingTask> Tasks { get; private set; }

        public override void Save() {
        }
    }
}
