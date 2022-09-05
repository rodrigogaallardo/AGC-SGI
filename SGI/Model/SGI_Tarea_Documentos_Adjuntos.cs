

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tarea_Documentos_Adjuntos
    {
        public int id_doc_adj { get; set; }
        public int id_tramitetarea { get; set; }
        public string tdoc_adj_detalle { get; set; }
        public int id_file { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public string nombre_archivo { get; set; }
        public Nullable<int> id_tdocreq { get; set; }
    
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
        public virtual TiposDeDocumentosRequeridos TiposDeDocumentosRequeridos { get; set; }
    }
}
