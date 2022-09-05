

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Planos
    {
        public int id_encomienda_plano { get; set; }
        public int id_encomienda { get; set; }
        public int id_file { get; set; }
        public int id_tipo_plano { get; set; }
        public string detalle { get; set; }
        public string nombre_archivo { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> fechaPresentado { get; set; }
    
        public virtual TiposDePlanos TiposDePlanos { get; set; }
        public virtual Encomienda Encomienda { get; set; }
    }
}
