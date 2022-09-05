

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_Titulares_PersonasJuridicas_PersonasFisicas
    {
        public int id_titular_pj { get; set; }
        public int id_solicitud { get; set; }
        public int id_personajuridica { get; set; }
        public string Apellido { get; set; }
        public string Nombres { get; set; }
        public int id_tipodoc_personal { get; set; }
        public string Nro_Documento { get; set; }
        public string Email { get; set; }
        public int id_firmante_pj { get; set; }
        public bool firmante_misma_persona { get; set; }
    
        public virtual TipoDocumentoPersonal TipoDocumentoPersonal { get; set; }
        public virtual Transf_Firmantes_PersonasJuridicas Transf_Firmantes_PersonasJuridicas { get; set; }
        public virtual Transf_Titulares_PersonasJuridicas Transf_Titulares_PersonasJuridicas { get; set; }
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
    }
}
