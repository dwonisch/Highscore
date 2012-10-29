using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighScore.Data
{
    public class HighScore
    {
        public virtual int Id { get; set; }
        public virtual Score Score { get; set; }
        public virtual int Value { get; set; }
    }
}
