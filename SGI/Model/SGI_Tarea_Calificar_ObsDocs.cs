

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Tarea_Calificar_ObsDocs
    {
        public int id_ObsDocs { get; set; }
        public int id_ObsGrupo { get; set; }
        public int id_tdocreq { get; set; }
        public string Observacion_ObsDocs { get; set; }
        public string Respaldo_ObsDocs { get; set; }
        public Nullable<int> id_file { get; set; }
        public Nullable<int> id_certificado { get; set; }
        public Nullable<bool> Decido_no_subir { get; set; }
        public Nullable<bool> Actual { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<int> id_file_sade { get; set; }
    
        public virtual SGI_Tarea_Calificar_ObsDocs SGI_Tarea_Calificar_ObsDocs1 { get; set; }
        public virtual SGI_Tarea_Calificar_ObsDocs SGI_Tarea_Calificar_ObsDocs2 { get; set; }
        public virtual SGI_Tarea_Calificar_ObsGrupo SGI_Tarea_Calificar_ObsGrupo { get; set; }
        public virtual TiposDeDocumentosRequeridos TiposDeDocumentosRequeridos { get; set; }
    }
}
