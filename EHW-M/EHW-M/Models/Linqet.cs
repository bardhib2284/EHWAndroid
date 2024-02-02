using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class Linqet {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Linku { get; set; }
        public string Tipi { get; set; }
        public bool IsActive { get; set; }
    }
}
