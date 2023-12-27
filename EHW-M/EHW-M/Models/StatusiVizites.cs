using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class StatusiVizites {
        [PrimaryKey]
        public int IDStatusiVizites { get; set; }
        public string Gjendja { get; set; }
        public int SyncStatus { get; set; }
    }
}