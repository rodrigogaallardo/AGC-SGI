

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CPadron_Titulares_PersonasJuridicas_PersonasFisicas
    {
        public int id_titular_pj { get; set; }
        public int id_cpadron { get; set; }
        public int id_personajuridica { get; set; }
        public string Apellido { get; set; }
        public string Nombres { get; set; }
        public int id_tipodoc_personal { get; set; }
        public string Email { get; set; }
        public bool firmante_misma_persona { get; set; }
        public string Nro_Documento { get; set; }
    }
}
