

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Firmantes_Solicitud_PersonasJuridicas
    {
        public Transf_Firmantes_Solicitud_PersonasJuridicas()
        {
            this.Transf_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas = new HashSet<Transf_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas>();
        }
    
        public int id_firmante_pj { get; set; }
        public int id_solicitud { get; set; }
        public int id_personajuridica { get; set; }
        public string Apellido { get; set; }
        public string Nombres { get; set; }
        public int id_tipodoc_personal { get; set; }
        public string Nro_Documento { get; set; }
        public int id_tipocaracter { get; set; }
        public string cargo_firmante_pj { get; set; }
        public string Email { get; set; }
        public string Cuit { get; set; }
    
        public virtual TipoDocumentoPersonal TipoDocumentoPersonal { get; set; }
        public virtual TiposDeCaracterLegal TiposDeCaracterLegal { get; set; }
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
        public virtual Transf_Titulares_Solicitud_PersonasJuridicas Transf_Titulares_Solicitud_PersonasJuridicas { get; set; }
        public virtual ICollection<Transf_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas> Transf_Titulares_Solicitud_PersonasJuridicas_PersonasFisicas { get; set; }
    }
}
