

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TiposDeIngresosBrutos
    {
        public TiposDeIngresosBrutos()
        {
            this.Transf_Titulares_PersonasFisicas = new HashSet<Transf_Titulares_PersonasFisicas>();
            this.Transf_Titulares_PersonasJuridicas = new HashSet<Transf_Titulares_PersonasJuridicas>();
            this.Encomienda_Titulares_PersonasFisicas = new HashSet<Encomienda_Titulares_PersonasFisicas>();
            this.Encomienda_Titulares_PersonasJuridicas = new HashSet<Encomienda_Titulares_PersonasJuridicas>();
            this.SSIT_Solicitudes_Titulares_PersonasFisicas = new HashSet<SSIT_Solicitudes_Titulares_PersonasFisicas>();
            this.SSIT_Solicitudes_Titulares_PersonasJuridicas = new HashSet<SSIT_Solicitudes_Titulares_PersonasJuridicas>();
            this.CPadron_Titulares_Solicitud_PersonasFisicas = new HashSet<CPadron_Titulares_Solicitud_PersonasFisicas>();
            this.CPadron_Titulares_Solicitud_PersonasJuridicas = new HashSet<CPadron_Titulares_Solicitud_PersonasJuridicas>();
            this.Transf_Titulares_Solicitud_PersonasFisicas = new HashSet<Transf_Titulares_Solicitud_PersonasFisicas>();
            this.Transf_Titulares_Solicitud_PersonasJuridicas = new HashSet<Transf_Titulares_Solicitud_PersonasJuridicas>();
        }
    
        public int id_tipoiibb { get; set; }
        public string cod_tipoibb { get; set; }
        public string nom_tipoiibb { get; set; }
        public string formato_tipoiibb { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual ICollection<Transf_Titulares_PersonasFisicas> Transf_Titulares_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Titulares_PersonasJuridicas> Transf_Titulares_PersonasJuridicas { get; set; }
        public virtual ICollection<Encomienda_Titulares_PersonasFisicas> Encomienda_Titulares_PersonasFisicas { get; set; }
        public virtual ICollection<Encomienda_Titulares_PersonasJuridicas> Encomienda_Titulares_PersonasJuridicas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Titulares_PersonasFisicas> SSIT_Solicitudes_Titulares_PersonasFisicas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Titulares_PersonasJuridicas> SSIT_Solicitudes_Titulares_PersonasJuridicas { get; set; }
        public virtual ICollection<CPadron_Titulares_Solicitud_PersonasFisicas> CPadron_Titulares_Solicitud_PersonasFisicas { get; set; }
        public virtual ICollection<CPadron_Titulares_Solicitud_PersonasJuridicas> CPadron_Titulares_Solicitud_PersonasJuridicas { get; set; }
        public virtual ICollection<Transf_Titulares_Solicitud_PersonasFisicas> Transf_Titulares_Solicitud_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Titulares_Solicitud_PersonasJuridicas> Transf_Titulares_Solicitud_PersonasJuridicas { get; set; }
    }
}
