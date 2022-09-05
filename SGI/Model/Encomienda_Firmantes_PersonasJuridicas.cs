

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Firmantes_PersonasJuridicas
    {
        public Encomienda_Firmantes_PersonasJuridicas()
        {
            this.Encomienda_Titulares_PersonasJuridicas_PersonasFisicas = new HashSet<Encomienda_Titulares_PersonasJuridicas_PersonasFisicas>();
        }
    
        public int id_firmante_pj { get; set; }
        public int id_encomienda { get; set; }
        public int id_personajuridica { get; set; }
        public string Apellido { get; set; }
        public string Nombres { get; set; }
        public int id_tipodoc_personal { get; set; }
        public string Nro_Documento { get; set; }
        public int id_tipocaracter { get; set; }
        public string cargo_firmante_pj { get; set; }
        public string Email { get; set; }
    
        public virtual TipoDocumentoPersonal TipoDocumentoPersonal { get; set; }
        public virtual TiposDeCaracterLegal TiposDeCaracterLegal { get; set; }
        public virtual ICollection<Encomienda_Titulares_PersonasJuridicas_PersonasFisicas> Encomienda_Titulares_PersonasJuridicas_PersonasFisicas { get; set; }
        public virtual Encomienda_Titulares_PersonasJuridicas Encomienda_Titulares_PersonasJuridicas { get; set; }
        public virtual Encomienda Encomienda { get; set; }
    }
}
