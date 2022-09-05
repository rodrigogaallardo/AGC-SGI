

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Localidad
    {
        public Localidad()
        {
            this.Transf_Titulares_PersonasFisicas = new HashSet<Transf_Titulares_PersonasFisicas>();
            this.Transf_Titulares_PersonasJuridicas = new HashSet<Transf_Titulares_PersonasJuridicas>();
            this.Encomienda_Titulares_PersonasFisicas = new HashSet<Encomienda_Titulares_PersonasFisicas>();
            this.Encomienda_Titulares_PersonasJuridicas = new HashSet<Encomienda_Titulares_PersonasJuridicas>();
            this.SSIT_Solicitudes_Titulares_PersonasFisicas = new HashSet<SSIT_Solicitudes_Titulares_PersonasFisicas>();
            this.SSIT_Solicitudes_Titulares_PersonasJuridicas = new HashSet<SSIT_Solicitudes_Titulares_PersonasJuridicas>();
            this.CPadron_Titulares_Solicitud_PersonasFisicas = new HashSet<CPadron_Titulares_Solicitud_PersonasFisicas>();
            this.CPadron_Titulares_Solicitud_PersonasJuridicas = new HashSet<CPadron_Titulares_Solicitud_PersonasJuridicas>();
            this.Usuario = new HashSet<Usuario>();
            this.Transf_Titulares_Solicitud_PersonasFisicas = new HashSet<Transf_Titulares_Solicitud_PersonasFisicas>();
            this.Transf_Titulares_Solicitud_PersonasJuridicas = new HashSet<Transf_Titulares_Solicitud_PersonasJuridicas>();
        }
    
        public int Id { get; set; }
        public Nullable<int> IdProvincia { get; set; }
        public Nullable<int> IdDepto { get; set; }
        public string CodDepto { get; set; }
        public string Depto { get; set; }
        public string Cabecera { get; set; }
        public Nullable<double> Area { get; set; }
        public Nullable<double> Perimetro { get; set; }
        public Nullable<double> Clave { get; set; }
        public Nullable<bool> Excluir { get; set; }
    
        public virtual Provincia Provincia { get; set; }
        public virtual ICollection<Transf_Titulares_PersonasFisicas> Transf_Titulares_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Titulares_PersonasJuridicas> Transf_Titulares_PersonasJuridicas { get; set; }
        public virtual ICollection<Encomienda_Titulares_PersonasFisicas> Encomienda_Titulares_PersonasFisicas { get; set; }
        public virtual ICollection<Encomienda_Titulares_PersonasJuridicas> Encomienda_Titulares_PersonasJuridicas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Titulares_PersonasFisicas> SSIT_Solicitudes_Titulares_PersonasFisicas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Titulares_PersonasJuridicas> SSIT_Solicitudes_Titulares_PersonasJuridicas { get; set; }
        public virtual ICollection<CPadron_Titulares_Solicitud_PersonasFisicas> CPadron_Titulares_Solicitud_PersonasFisicas { get; set; }
        public virtual ICollection<CPadron_Titulares_Solicitud_PersonasJuridicas> CPadron_Titulares_Solicitud_PersonasJuridicas { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
        public virtual ICollection<Transf_Titulares_Solicitud_PersonasFisicas> Transf_Titulares_Solicitud_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Titulares_Solicitud_PersonasJuridicas> Transf_Titulares_Solicitud_PersonasJuridicas { get; set; }
    }
}
