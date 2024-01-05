using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models
{
    public class NumriPorosive
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int NrPorosise { get; set; }
        public string TIPI { get; set; }
    }
}
