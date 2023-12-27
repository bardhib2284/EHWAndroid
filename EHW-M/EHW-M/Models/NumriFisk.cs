using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class NumriFisk {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public int IDN { get; set; }
        public int LevizjeIDN { get; set; }
        public string Depo { get; set; }
        public string TCRCode { get; set; }
        public int Viti { get; set; }
          
    }
}
