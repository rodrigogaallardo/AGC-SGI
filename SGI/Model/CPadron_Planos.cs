

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CPadron_Planos
    {
        public int id_cpadron_plano { get; set; }
        public int id_cpadron { get; set; }
        public int id_file { get; set; }
        public int id_tipo_plano { get; set; }
        public string detalle { get; set; }
        public string nombre_archivo { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
    }
}
