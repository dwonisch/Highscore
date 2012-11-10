using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HighScore.Data {
    public class Settings {
        public Settings() {
            Zoom = 1;
        }
        public virtual int Id { get; set; }
        public virtual double Zoom { get; set; }
    }
}
