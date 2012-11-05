using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HighScore.Data {
    public class ScoreValue {
        public virtual Guid Id { get; set; }
        public virtual Score Score { get; set; }
        public virtual int Value { get; set; }  
    }
}
