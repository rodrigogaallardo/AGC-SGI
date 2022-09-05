

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Disposiciones_Texto
    {
        public int id_dispotexto { get; set; }
        public string cod_dispo { get; set; }
        public string descripcion { get; set; }
        public string texto { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
