

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Parametros
    {
        public int id_param { get; set; }
        public string cod_param { get; set; }
        public string nom_param { get; set; }
        public string valorchar_param { get; set; }
        public Nullable<decimal> valornum_param { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}
