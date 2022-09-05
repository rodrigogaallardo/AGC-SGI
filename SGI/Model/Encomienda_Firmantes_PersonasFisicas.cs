

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Firmantes_PersonasFisicas
    {
        public int id_firmante_pf { get; set; }
        public int id_encomienda { get; set; }
        public int id_personafisica { get; set; }
        public string Apellido { get; set; }
        public string Nombres { get; set; }
        public int id_tipodoc_personal { get; set; }
        public string Nro_Documento { get; set; }
        public int id_tipocaracter { get; set; }
        public string Email { get; set; }
    
        public virtual TipoDocumentoPersonal TipoDocumentoPersonal { get; set; }
        public virtual TiposDeCaracterLegal TiposDeCaracterLegal { get; set; }
        public virtual Encomienda_Titulares_PersonasFisicas Encomienda_Titulares_PersonasFisicas { get; set; }
        public virtual Encomienda Encomienda { get; set; }
    }
}
