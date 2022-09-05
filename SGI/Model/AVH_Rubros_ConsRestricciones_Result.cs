

namespace SGI.Model
{
    using System;
    
    public partial class AVH_Rubros_ConsRestricciones_Result
    {
        public int id_rubro { get; set; }
        public string cod_rubro { get; set; }
        public string nom_rubro { get; set; }
        public string desc_restriccion { get; set; }
        public Nullable<int> Superficie_ok { get; set; }
        public Nullable<int> Zona_ok { get; set; }
        public int Id_TipoTramite { get; set; }
        public Nullable<int> Id_ImpactoAmbiental { get; set; }
        public bool ConsultaPlaneamiento { get; set; }
    }
}
