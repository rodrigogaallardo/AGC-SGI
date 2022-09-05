

namespace SGI.Model
{
    using System;
    
    public partial class ws_Habilitaciones_ConsultarAprobacionRubro_Result
    {
        public string cod_rubro { get; set; }
        public string nom_rubro { get; set; }
        public int id_tipodocreq { get; set; }
        public string nomenclatura { get; set; }
        public int id_tipoactividad { get; set; }
        public string nom_tipoactividad { get; set; }
        public int Validacion_Tipotramite { get; set; }
        public int Validacion_Zona { get; set; }
        public int Validacion_Superficie { get; set; }
        public bool Historico { get; set; }
    }
}
