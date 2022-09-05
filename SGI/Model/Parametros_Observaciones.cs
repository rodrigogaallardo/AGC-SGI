

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Parametros_Observaciones
    {
        public int id_param { get; set; }
        public int id_circuito { get; set; }
        public int Cantidad { get; set; }
    
        public virtual ENG_Circuitos ENG_Circuitos { get; set; }
    }
}
