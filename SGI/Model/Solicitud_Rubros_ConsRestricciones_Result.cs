

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_Rubros_ConsRestricciones_Result
    {
        public string cod_rubro { get; set; }
        public string nom_rubro { get; set; }
        public Nullable<int> TipoActividad_ok { get; set; }
        public Nullable<int> Superficie_ok { get; set; }
        public Nullable<int> Zona_ok { get; set; }
    }
}
