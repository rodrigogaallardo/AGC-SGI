

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_DocumentosAdjuntos
    {
        public int id_docadjunto { get; set; }
        public int id_encomienda { get; set; }
        public int id_tdocreq { get; set; }
        public string tdocreq_detalle { get; set; }
        public int id_tipodocsis { get; set; }
        public int id_file { get; set; }
        public bool generadoxSistema { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public string nombre_archivo { get; set; }
        public Nullable<System.DateTime> fechaPresentado { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        public virtual TiposDeDocumentosSistema TiposDeDocumentosSistema { get; set; }
        public virtual TiposDeDocumentosRequeridos TiposDeDocumentosRequeridos { get; set; }
        public virtual Encomienda Encomienda { get; set; }
    }
}
