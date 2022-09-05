

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TiposDeCaracterLegal
    {
        public TiposDeCaracterLegal()
        {
            this.Transf_Firmantes_PersonasFisicas = new HashSet<Transf_Firmantes_PersonasFisicas>();
            this.Transf_Firmantes_PersonasJuridicas = new HashSet<Transf_Firmantes_PersonasJuridicas>();
            this.SSIT_Solicitudes_Firmantes_PersonasFisicas = new HashSet<SSIT_Solicitudes_Firmantes_PersonasFisicas>();
            this.SSIT_Solicitudes_Firmantes_PersonasJuridicas = new HashSet<SSIT_Solicitudes_Firmantes_PersonasJuridicas>();
            this.Encomienda_Firmantes_PersonasFisicas = new HashSet<Encomienda_Firmantes_PersonasFisicas>();
            this.Encomienda_Firmantes_PersonasJuridicas = new HashSet<Encomienda_Firmantes_PersonasJuridicas>();
            this.Transf_Firmantes_Solicitud_PersonasFisicas = new HashSet<Transf_Firmantes_Solicitud_PersonasFisicas>();
            this.Transf_Firmantes_Solicitud_PersonasJuridicas = new HashSet<Transf_Firmantes_Solicitud_PersonasJuridicas>();
        }
    
        public int id_tipocaracter { get; set; }
        public string cod_tipocaracter { get; set; }
        public string nom_tipocaracter { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int disponibilidad_tipocaracter { get; set; }
        public bool muestracargo_tipocaracter { get; set; }
    
        public virtual ICollection<Transf_Firmantes_PersonasFisicas> Transf_Firmantes_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Firmantes_PersonasJuridicas> Transf_Firmantes_PersonasJuridicas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Firmantes_PersonasFisicas> SSIT_Solicitudes_Firmantes_PersonasFisicas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Firmantes_PersonasJuridicas> SSIT_Solicitudes_Firmantes_PersonasJuridicas { get; set; }
        public virtual ICollection<Encomienda_Firmantes_PersonasFisicas> Encomienda_Firmantes_PersonasFisicas { get; set; }
        public virtual ICollection<Encomienda_Firmantes_PersonasJuridicas> Encomienda_Firmantes_PersonasJuridicas { get; set; }
        public virtual ICollection<Transf_Firmantes_Solicitud_PersonasFisicas> Transf_Firmantes_Solicitud_PersonasFisicas { get; set; }
        public virtual ICollection<Transf_Firmantes_Solicitud_PersonasJuridicas> Transf_Firmantes_Solicitud_PersonasJuridicas { get; set; }
    }
}
