using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Shitjet {
        public int Id { get; set; }
        public Klientet Klienti { get; set; }
        public Qmimorja Qmimorja { get; set; }
        public List<Artikulli> Artikujt { get; set; }
        public double Cmimi { get; set; }
    }
}
