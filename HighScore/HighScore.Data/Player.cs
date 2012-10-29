using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighScore.Data
{
    public class Player
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string SecondName { get; set; }
        public virtual IList<Score> Scores { get; set; }

        public virtual string Name
        {
            get
            {
                if (string.IsNullOrEmpty(SecondName))
                    return FirstName;
                return FirstName + " " + SecondName;
            }
            set
            {
                var names = value.Split(new[] { ' ' }, 2);
                FirstName = names[0];
                if (names.Length > 1)
                    SecondName = names[1];
                else
                    SecondName = string.Empty;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Player)
                return ((Player)obj).Id == Id;
            return base.Equals(obj);
        }
    }
}
