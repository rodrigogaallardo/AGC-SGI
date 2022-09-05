

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda
    {
        public Encomienda()
        {
            this.Encomienda_ConformacionLocal = new HashSet<Encomienda_ConformacionLocal>();
            this.Encomienda_DatosLocal = new HashSet<Encomienda_DatosLocal>();
            this.Encomienda_DocumentosAdjuntos = new HashSet<Encomienda_DocumentosAdjuntos>();
            this.Encomienda_Firmantes_PersonasFisicas = new HashSet<Encomienda_Firmantes_PersonasFisicas>();
            this.Encomienda_Firmantes_PersonasJuridicas = new HashSet<Encomienda_Firmantes_PersonasJuridicas>();
            this.Encomienda_Normativas = new HashSet<Encomienda_Normativas>();
            this.Encomienda_Planos = new HashSet<Encomienda_Planos>();
            this.Encomienda_Plantas = new HashSet<Encomienda_Plantas>();
            this.Encomienda_Rubros_AT_Anterior = new HashSet<Encomienda_Rubros_AT_Anterior>();
            this.Encomienda_Rubros = new HashSet<Encomienda_Rubros>();
            this.Encomienda_RubrosCN_AT_Anterior = new HashSet<Encomienda_RubrosCN_AT_Anterior>();
            this.Encomienda_RubrosCN = new HashSet<Encomienda_RubrosCN>();
            this.Encomienda_Sobrecargas = new HashSet<Encomienda_Sobrecargas>();
            this.Encomienda_Titulares_PersonasFisicas = new HashSet<Encomienda_Titulares_PersonasFisicas>();
            this.Encomienda_Titulares_PersonasJuridicas = new HashSet<Encomienda_Titulares_PersonasJuridicas>();
            this.Encomienda_Titulares_PersonasJuridicas_PersonasFisicas = new HashSet<Encomienda_Titulares_PersonasJuridicas_PersonasFisicas>();
            this.Encomienda_Ubicaciones = new HashSet<Encomienda_Ubicaciones>();
            this.Rel_Encomienda_Rectificatoria = new HashSet<Rel_Encomienda_Rectificatoria>();
            this.Rel_Encomienda_Rectificatoria1 = new HashSet<Rel_Encomienda_Rectificatoria>();
            this.SSIT_Solicitudes_Encomienda = new HashSet<SSIT_Solicitudes_Encomienda>();
            this.wsEscribanos_ActaNotarial = new HashSet<wsEscribanos_ActaNotarial>();
            this.Encomienda_SSIT_Solicitudes = new HashSet<Encomienda_SSIT_Solicitudes>();
            this.Encomienda_Transf_Solicitudes = new HashSet<Encomienda_Transf_Solicitudes>();
            this.Encomienda_RubrosCN_Deposito = new HashSet<Encomienda_RubrosCN_Deposito>();
        }
    
        public int id_encomienda { get; set; }
        public System.DateTime FechaEncomienda { get; set; }
        public int nroEncomiendaconsejo { get; set; }
        public int id_consejo { get; set; }
        public int id_profesional { get; set; }
        public string ZonaDeclarada { get; set; }
        public int id_tipotramite { get; set; }
        public int id_tipoexpediente { get; set; }
        public int id_subtipoexpediente { get; set; }
        public int id_estado { get; set; }
        public string CodigoSeguridad { get; set; }
        public string Observaciones_plantas { get; set; }
        public string Observaciones_rubros { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public bool Pro_teatro { get; set; }
        public string tipo_anexo { get; set; }
        public bool Servidumbre_paso { get; set; }
        public bool CumpleArticulo521 { get; set; }
        public bool DeclaraOficinaComercial { get; set; }
        public Nullable<bool> Contiene_galeria_paseo { get; set; }
        public Nullable<bool> Consecutiva_Supera_10 { get; set; }
        public string Observaciones_rubros_AT_anterior { get; set; }
        public Nullable<bool> Asistentes350 { get; set; }
        public Nullable<bool> InformaModificacion { get; set; }
        public string DetalleModificacion { get; set; }
        public Nullable<bool> EsActBaile { get; set; }
        public Nullable<bool> EsLuminaria { get; set; }
        public Nullable<bool> ProductosInflamables { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual ICollection<Encomienda_ConformacionLocal> Encomienda_ConformacionLocal { get; set; }
        public virtual ICollection<Encomienda_DatosLocal> Encomienda_DatosLocal { get; set; }
        public virtual ICollection<Encomienda_DocumentosAdjuntos> Encomienda_DocumentosAdjuntos { get; set; }
        public virtual Encomienda_Estados Encomienda_Estados { get; set; }
        public virtual ICollection<Encomienda_Firmantes_PersonasFisicas> Encomienda_Firmantes_PersonasFisicas { get; set; }
        public virtual ICollection<Encomienda_Firmantes_PersonasJuridicas> Encomienda_Firmantes_PersonasJuridicas { get; set; }
        public virtual ICollection<Encomienda_Normativas> Encomienda_Normativas { get; set; }
        public virtual ICollection<Encomienda_Planos> Encomienda_Planos { get; set; }
        public virtual ICollection<Encomienda_Plantas> Encomienda_Plantas { get; set; }
        public virtual Profesional Profesional { get; set; }
        public virtual ICollection<Encomienda_Rubros_AT_Anterior> Encomienda_Rubros_AT_Anterior { get; set; }
        public virtual ICollection<Encomienda_Rubros> Encomienda_Rubros { get; set; }
        public virtual ICollection<Encomienda_RubrosCN_AT_Anterior> Encomienda_RubrosCN_AT_Anterior { get; set; }
        public virtual ICollection<Encomienda_RubrosCN> Encomienda_RubrosCN { get; set; }
        public virtual ICollection<Encomienda_Sobrecargas> Encomienda_Sobrecargas { get; set; }
        public virtual SubtipoExpediente SubtipoExpediente { get; set; }
        public virtual TipoExpediente TipoExpediente { get; set; }
        public virtual TipoTramite TipoTramite { get; set; }
        public virtual ICollection<Encomienda_Titulares_PersonasFisicas> Encomienda_Titulares_PersonasFisicas { get; set; }
        public virtual ICollection<Encomienda_Titulares_PersonasJuridicas> Encomienda_Titulares_PersonasJuridicas { get; set; }
        public virtual ICollection<Encomienda_Titulares_PersonasJuridicas_PersonasFisicas> Encomienda_Titulares_PersonasJuridicas_PersonasFisicas { get; set; }
        public virtual ICollection<Encomienda_Ubicaciones> Encomienda_Ubicaciones { get; set; }
        public virtual ICollection<Rel_Encomienda_Rectificatoria> Rel_Encomienda_Rectificatoria { get; set; }
        public virtual ICollection<Rel_Encomienda_Rectificatoria> Rel_Encomienda_Rectificatoria1 { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Encomienda> SSIT_Solicitudes_Encomienda { get; set; }
        public virtual ICollection<wsEscribanos_ActaNotarial> wsEscribanos_ActaNotarial { get; set; }
        public virtual ICollection<Encomienda_SSIT_Solicitudes> Encomienda_SSIT_Solicitudes { get; set; }
        public virtual ICollection<Encomienda_Transf_Solicitudes> Encomienda_Transf_Solicitudes { get; set; }
        public virtual ICollection<Encomienda_RubrosCN_Deposito> Encomienda_RubrosCN_Deposito { get; set; }
    }
}
