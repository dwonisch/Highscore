using System;

namespace HighScore.ViewModel {
    public class HighScoreConfiguration {
        public HighScoreConfiguration() {
            StartDate = new DateTime(2013, 11, 5).Date;
            EndDate = new DateTime(2013, 12, 13).Date;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
