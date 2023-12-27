using SQLite;
using System;

namespace EHWM.Models {
public class Porosite {
    public Guid IDVizita { get; set; }
    public string TitulliPorosise { get; set; }
    public DateTime DataPerLiferim { get; set; }
    [PrimaryKey]
    public Guid IDPorosia { get; set; }
    public DateTime DataPorosise { get; set; }
    public DateTime OraPorosise { get; set; }
    public byte StatusiPorosise { get; set; }
    public string NrPorosise { get; set; }
    public string DeviceID { get; set; }
    public int? SyncStatus { get; set; }
    public bool Aprovuar { get; set; }
    public int? NrDetalet { get; set; }
    public string Longitude { get; set; }
    public string Latitude { get; set; }

    }
}