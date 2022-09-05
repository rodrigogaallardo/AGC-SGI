

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Solicitudes_HistorialEstados
    {
        public int id_solhistest { get; set; }
        public int id_solicitud { get; set; }
        public string cod_estado_ant { get; set; }
        public string cod_estado_nuevo { get; set; }
        public string username { get; set; }
        public System.DateTime fecha_modificacion { get; set; }
        public System.Guid usuario_modificacion { get; set; }
    
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
    }
}
