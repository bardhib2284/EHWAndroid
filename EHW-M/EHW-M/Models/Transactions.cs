using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Transactions {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int VizitaId { get; set; }
        public int KlientId { get; set; }
    }
}
