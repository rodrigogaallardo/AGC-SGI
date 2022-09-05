

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TipoSociedad
    {
        public TipoSociedad()
        {
            this.Encomienda_Titulares_PersonasJuridicas = new HashSet<Encomienda_Titulares_PersonasJuridicas>();
            this.SSIT_Solicitudes_Titulares_PersonasJuridicas = new HashSet<SSIT_Solicitudes_Titulares_PersonasJuridicas>();
            this.CPadron_Titulares_Solicitud_PersonasJuridicas = new HashSet<CPadron_Titulares_Solicitud_PersonasJuridicas>();
            this.Solicitud = new HashSet<Solicitud>();
            this.Transf_Titulares_Solicitud_PersonasJuridicas = new HashSet<Transf_Titulares_Solicitud_PersonasJuridicas>();
        }
    
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int TitularesMinimo { get; set; }
        public int TitularesMaximo { get; set; }
        public bool RequiereNombreFantasia { get; set; }
        public bool RequiereRazonSocial { get; set; }
        public bool RequiereCuitPropio { get; set; }
    
        public virtual ICollection<Encomienda_Titulares_PersonasJuridicas> Encomienda_Titulares_PersonasJuridicas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Titulares_PersonasJuridicas> SSIT_Solicitudes_Titulares_PersonasJuridicas { get; set; }
        public virtual ICollection<CPadron_Titulares_Solicitud_PersonasJuridicas> CPadron_Titulares_Solicitud_PersonasJuridicas { get; set; }
        public virtual ICollection<Solicitud> Solicitud { get; set; }
        public virtual ICollection<Transf_Titulares_Solicitud_PersonasJuridicas> Transf_Titulares_Solicitud_PersonasJuridicas { get; set; }
    }
}
