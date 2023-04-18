

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CAA_TiposDeCertificados
    {
        public int id_tipocertificado { get; set; }
        public string codigo_tipocertificado { get; set; }
        public string nombre_tipocertificado { get; set; }
        public string descripcion_tipocertificado { get; set; }
        public int id_ImactoAmbiental { get; set; }
        public int puntaje_para_peligrosos { get; set; }
    }
}
