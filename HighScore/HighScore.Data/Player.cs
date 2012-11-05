using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HighScore.Data {
    public class Player {

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Female { get; set; }
        public virtual bool Child { get; set; }

        public override bool Equals(object obj) {
            var player = obj as Player;

            if (player != null)
                return player.Id == Id;

            return base.Equals(obj);
        }

        public override string ToString() {
            return Name;
        }
    }
}
