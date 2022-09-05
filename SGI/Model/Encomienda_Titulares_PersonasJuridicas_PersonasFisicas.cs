

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Titulares_PersonasJuridicas_PersonasFisicas
    {
        public int id_titular_pj { get; set; }
        public int id_encomienda { get; set; }
        public int id_personajuridica { get; set; }
        public string Apellido { get; set; }
        public string Nombres { get; set; }
        public int id_tipodoc_personal { get; set; }
        public string Nro_Documento { get; set; }
        public string Email { get; set; }
        public int id_firmante_pj { get; set; }
        public bool firmante_misma_persona { get; set; }
    
        public virtual Encomienda_Firmantes_PersonasJuridicas Encomienda_Firmantes_PersonasJuridicas { get; set; }
        public virtual TipoDocumentoPersonal TipoDocumentoPersonal { get; set; }
        public virtual Encomienda_Titulares_PersonasJuridicas Encomienda_Titulares_PersonasJuridicas { get; set; }
        public virtual Encomienda Encomienda { get; set; }
    }
}
