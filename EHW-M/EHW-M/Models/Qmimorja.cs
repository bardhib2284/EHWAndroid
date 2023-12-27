using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Qmimorja {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public List<Artikulli> Artikujt;
    }
}
