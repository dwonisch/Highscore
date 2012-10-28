using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighScore.ViewModel
{
    public class HighScoreConfiguration
    {
        public HighScoreConfiguration()
        {
            StartDate = new DateTime(2012, 11, 7).Date;
            EndDate = new DateTime(2012, 12, 14).Date;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
