

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Conceptos_BUI_Independientes
    {
        public int id_concepto { get; set; }
        public string keycode { get; set; }
        public int cod_concepto_1 { get; set; }
        public int cod_concepto_2 { get; set; }
        public int cod_concepto_3 { get; set; }
        public string descripcion_concepto { get; set; }
        public bool admite_reglas { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
