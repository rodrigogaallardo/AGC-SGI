

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoDocumentoPersonal
    {
        public TipoDocumentoPersonal()
        {
            this.Transf_Firmantes_PersonasFisicas = new HashSet<Transf_Firmantes_PersonasFisicas>();
            this.Transf_Firmantes_PersonasJuridicas = new HashSet<Transf_Firmantes_PersonasJuridicas>();
            this.Transf_Titulares_PersonasFisicas = new HashSet<Transf_Titulares_PersonasFisicas>();
            this.Transf_Titulares_PersonasJuridicas_PersonasFisicas = new HashSet<Transf_Titulares_PersonasJuridicas_PersonasFisicas>();
            this.SSIT_Solicitudes_Firmantes_PersonasFisicas = new HashSet<SSIT_Solicitudes_Firmantes_PersonasFisicas>();
            this.SSIT_Solicitudes_Firmantes_PersonasJuridicas = new HashSet<SSIT_Solicitudes_Firmantes_PersonasJuridicas>();
            this.SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas = new HashSet<SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas>();
            this.Encomienda_Firmantes_PersonasFisicas = new HashSet<Encomienda_Firmantes_PersonasFisicas>();
            this.Encomienda_Firmantes_PersonasJuridicas = new HashSet<Encomienda_Firmantes_PersonasJuridicas>();
            this.Encomienda_Titulares_PersonasJuridicas_PersonasFisicas = new HashSet<Encomienda_Titulares_PersonasJuridicas_PersonasFisicas>();
            this.Encomienda_Titulares_PersonasFisicas = new HashSet<Encomienda_Titulares_PersonasFisicas>();
            this.SSIT_Solicitudes_Titulares_PersonasFisicas = new HashSet<SSIT_Solicitudes_Titulares_PersonasFisicas>();
            this.CPadron_Titulares_Solicitud_PersonasFisicas = new HashSet<CPadron_Titulares_Solicitud_PersonasFisicas>();
            this.CPadron_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas = new HashSet<CPadron_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas>();
            this.PersonasInhibidas = new HashSet<PersonasInhibidas>();
            this.Profesional = new HashSet<Profesional>();
            this.Transf_Firmantes_Solicitud_PersonasFisicas = new HashSet<Transf_Firmantes_Solicitud_PersonasFisicas>();
            this.Transf_Firmantes_Solicitud_PersonasJuridicas = new HashSet<Transf_Firmantes_Solicitud_PersonasJuridicas>();
            this.Transf_Titulares_Solicitud_PersonasFisicas = new HashSet<Transf_Titulares_Solicitud_PersonasFisicas>();
            this.Transf_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas = new HashSet<Transf_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas>();
        }
    
        public int TipoDocumentoPersonalId { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
    
        public virtual ICollection<Transf_Firmantes_PersonasFisicas> Transf_Firmantes_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Firmantes_PersonasJuridicas> Transf_Firmantes_PersonasJuridicas { get; set; }
        public virtual ICollection<Transf_Titulares_PersonasFisicas> Transf_Titulares_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Titulares_PersonasJuridicas_PersonasFisicas> Transf_Titulares_PersonasJuridicas_PersonasFisicas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Firmantes_PersonasFisicas> SSIT_Solicitudes_Firmantes_PersonasFisicas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Firmantes_PersonasJuridicas> SSIT_Solicitudes_Firmantes_PersonasJuridicas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas> SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas { get; set; }
        public virtual ICollection<Encomienda_Firmantes_PersonasFisicas> Encomienda_Firmantes_PersonasFisicas { get; set; }
        public virtual ICollection<Encomienda_Firmantes_PersonasJuridicas> Encomienda_Firmantes_PersonasJuridicas { get; set; }
        public virtual ICollection<Encomienda_Titulares_PersonasJuridicas_PersonasFisicas> Encomienda_Titulares_PersonasJuridicas_PersonasFisicas { get; set; }
        public virtual ICollection<Encomienda_Titulares_PersonasFisicas> Encomienda_Titulares_PersonasFisicas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Titulares_PersonasFisicas> SSIT_Solicitudes_Titulares_PersonasFisicas { get; set; }
        public virtual ICollection<CPadron_Titulares_Solicitud_PersonasFisicas> CPadron_Titulares_Solicitud_PersonasFisicas { get; set; }
        public virtual ICollection<CPadron_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas> CPadron_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas { get; set; }
        public virtual ICollection<PersonasInhibidas> PersonasInhibidas { get; set; }
        public virtual ICollection<Profesional> Profesional { get; set; }
        public virtual ICollection<Transf_Firmantes_Solicitud_PersonasFisicas> Transf_Firmantes_Solicitud_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Firmantes_Solicitud_PersonasJuridicas> Transf_Firmantes_Solicitud_PersonasJuridicas { get; set; }
        public virtual ICollection<Transf_Titulares_Solicitud_PersonasFisicas> Transf_Titulares_Solicitud_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas> Transf_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas { get; set; }
    }
}
