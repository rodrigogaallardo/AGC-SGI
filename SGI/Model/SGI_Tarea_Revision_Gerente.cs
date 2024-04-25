

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tarea_Revision_Gerente
    {
        public int id_revision_gerente { get; set; }
        public int id_tramitetarea { get; set; }
        public string Observaciones { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public string observacion_plancheta { get; set; }
        public string observacion_providencia { get; set; }
        public string observaciones_contribuyente { get; set; }
        public bool Librar_Uso { get; set; }
        public string Observaciones_LibradoUso { get; set; }
    
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}
