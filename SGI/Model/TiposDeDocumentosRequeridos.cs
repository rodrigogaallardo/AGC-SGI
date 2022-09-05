

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TiposDeDocumentosRequeridos
    {
        public TiposDeDocumentosRequeridos()
        {
            this.CPadron_DocumentosAdjuntos = new HashSet<CPadron_DocumentosAdjuntos>();
            this.Encomienda_DocumentosAdjuntos = new HashSet<Encomienda_DocumentosAdjuntos>();
            this.Rel_TiposDeDocumentosRequeridos_ENG_Tareas = new HashSet<Rel_TiposDeDocumentosRequeridos_ENG_Tareas>();
            this.Rel_TipoTramite_TiposDeDocumentosRequeridos = new HashSet<Rel_TipoTramite_TiposDeDocumentosRequeridos>();
            this.Rubros_TiposDeDocumentosRequeridos = new HashSet<Rubros_TiposDeDocumentosRequeridos>();
            this.SGI_Tarea_Calificar_ObsDocs = new HashSet<SGI_Tarea_Calificar_ObsDocs>();
            this.SGI_Tarea_Documentos_Adjuntos = new HashSet<SGI_Tarea_Documentos_Adjuntos>();
            this.Transf_DocumentosAdjuntos = new HashSet<Transf_DocumentosAdjuntos>();
            this.Rubros_TiposDeDocumentosRequeridos_Zonas = new HashSet<Rubros_TiposDeDocumentosRequeridos_Zonas>();
            this.Rubros_Config_Incendio_TiposDeDocumentosRequeridos = new HashSet<Rubros_Config_Incendio_TiposDeDocumentosRequeridos>();
            this.RubrosCN_TiposDeDocumentosRequeridos = new HashSet<RubrosCN_TiposDeDocumentosRequeridos>();
            this.SSIT_DocumentosAdjuntos = new HashSet<SSIT_DocumentosAdjuntos>();
        }
    
        public int id_tdocreq { get; set; }
        public string nombre_tdocreq { get; set; }
        public string observaciones_tdocreq { get; set; }
        public bool baja_tdocreq { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public bool RequiereDetalle { get; set; }
        public bool visible_en_SSIT { get; set; }
        public bool visible_en_SGI { get; set; }
        public Nullable<int> tamanio_maximo_mb { get; set; }
        public string formato_archivo { get; set; }
        public string acronimo_SADE { get; set; }
        public bool visible_en_Obs { get; set; }
        public Nullable<int> id_tipdocsis { get; set; }
        public bool verificar_firma_digital { get; set; }
        public bool visible_en_AT { get; set; }
        public string ffcc_SADE { get; set; }
    
        public virtual ICollection<CPadron_DocumentosAdjuntos> CPadron_DocumentosAdjuntos { get; set; }
        public virtual ICollection<Encomienda_DocumentosAdjuntos> Encomienda_DocumentosAdjuntos { get; set; }
        public virtual ICollection<Rel_TiposDeDocumentosRequeridos_ENG_Tareas> Rel_TiposDeDocumentosRequeridos_ENG_Tareas { get; set; }
        public virtual ICollection<Rel_TipoTramite_TiposDeDocumentosRequeridos> Rel_TipoTramite_TiposDeDocumentosRequeridos { get; set; }
        public virtual ICollection<Rubros_TiposDeDocumentosRequeridos> Rubros_TiposDeDocumentosRequeridos { get; set; }
        public virtual ICollection<SGI_Tarea_Calificar_ObsDocs> SGI_Tarea_Calificar_ObsDocs { get; set; }
        public virtual ICollection<SGI_Tarea_Documentos_Adjuntos> SGI_Tarea_Documentos_Adjuntos { get; set; }
        public virtual TiposDeDocumentosSistema TiposDeDocumentosSistema { get; set; }
        public virtual ICollection<Transf_DocumentosAdjuntos> Transf_DocumentosAdjuntos { get; set; }
        public virtual ICollection<Rubros_TiposDeDocumentosRequeridos_Zonas> Rubros_TiposDeDocumentosRequeridos_Zonas { get; set; }
        public virtual ICollection<Rubros_Config_Incendio_TiposDeDocumentosRequeridos> Rubros_Config_Incendio_TiposDeDocumentosRequeridos { get; set; }
        public virtual ICollection<RubrosCN_TiposDeDocumentosRequeridos> RubrosCN_TiposDeDocumentosRequeridos { get; set; }
        public virtual ICollection<SSIT_DocumentosAdjuntos> SSIT_DocumentosAdjuntos { get; set; }
    }
}
