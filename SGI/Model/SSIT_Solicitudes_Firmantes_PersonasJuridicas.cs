

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Firmantes_PersonasJuridicas
    {
        public SSIT_Solicitudes_Firmantes_PersonasJuridicas()
        {
            this.SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas = new HashSet<SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas>();
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
        public virtual ICollection<SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas> SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas { get; set; }
        public virtual SSIT_Solicitudes_Titulares_PersonasJuridicas SSIT_Solicitudes_Titulares_PersonasJuridicas { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
    }
}
