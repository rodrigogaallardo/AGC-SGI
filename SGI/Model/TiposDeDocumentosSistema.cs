

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TiposDeDocumentosSistema
    {
        public TiposDeDocumentosSistema()
        {
            this.CPadron_DocumentosAdjuntos = new HashSet<CPadron_DocumentosAdjuntos>();
            this.Transf_DocumentosAdjuntos = new HashSet<Transf_DocumentosAdjuntos>();
            this.Encomienda_DocumentosAdjuntos = new HashSet<Encomienda_DocumentosAdjuntos>();
            this.TiposDeDocumentosRequeridos = new HashSet<TiposDeDocumentosRequeridos>();
            this.SSIT_DocumentosAdjuntos = new HashSet<SSIT_DocumentosAdjuntos>();
        }
    
        public int id_tipdocsis { get; set; }
        public string cod_tipodocsis { get; set; }
        public string nombre_tipodocsis { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string acronimo_SADE { get; set; }
        public string ffcc_SADE { get; set; }
    
        public virtual ICollection<CPadron_DocumentosAdjuntos> CPadron_DocumentosAdjuntos { get; set; }
        public virtual ICollection<Transf_DocumentosAdjuntos> Transf_DocumentosAdjuntos { get; set; }
        public virtual ICollection<Encomienda_DocumentosAdjuntos> Encomienda_DocumentosAdjuntos { get; set; }
        public virtual ICollection<TiposDeDocumentosRequeridos> TiposDeDocumentosRequeridos { get; set; }
        public virtual ICollection<SSIT_DocumentosAdjuntos> SSIT_DocumentosAdjuntos { get; set; }
    }
}
