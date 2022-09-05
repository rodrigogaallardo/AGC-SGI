

namespace SGI.Model
{
    using System;
    
    public partial class PruebasEntityFWK_Result
    {
        public int id_rubro { get; set; }
        public string cod_rubro { get; set; }
        public string nom_rubro { get; set; }
        public string bus_rubro { get; set; }
        public int id_tipoactividad { get; set; }
        public int id_tipodocreq { get; set; }
        public bool EsAnterior_Rubro { get; set; }
        public Nullable<System.DateTime> VigenciaDesde_rubro { get; set; }
        public Nullable<System.DateTime> VigenciaHasta_rubro { get; set; }
        public bool PregAntenaEmisora { get; set; }
        public bool SoloAPRA { get; set; }
        public string tooltip_rubro { get; set; }
        public Nullable<double> local_venta { get; set; }
        public Nullable<bool> ley105 { get; set; }
    }
}
