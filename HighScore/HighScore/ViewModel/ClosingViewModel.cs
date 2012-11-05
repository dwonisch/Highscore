using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HighScore.ViewModel {
    public class ClosingViewModel : SaveableViewModel {

        public ClosingViewModel() {
            Tasks = new ObservableCollection<ClosingTask>();
            Tasks.Add(new ClosingTask("Datenbanken werden gesichtert", UploadDatabase));
        }

        public void Execute() {
            Tasks.ToList().ForEach(t => t.Execute());
        }

        private string UploadDatabase() {
            string newFileName = string.Format("{0:yyyy-MM-dd-HHmmss}-database.db", DateTime.Now);

            FtpWebRequest ftprequest = (FtpWebRequest)WebRequest.Create("ftp://www.woni.at/" + newFileName);

            ftprequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftprequest.Credentials = new NetworkCredential("u52287998-dart", "dartturnier");


            using (var reader = new StreamReader("database.db")) {
                byte[] fileContents = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                ftprequest.ContentLength = fileContents.Length;

                using (var writer = ftprequest.GetRequestStream()) {
                    writer.Write(fileContents, 0, fileContents.Length);
                }
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
