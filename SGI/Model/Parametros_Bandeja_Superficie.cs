

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Parametros_Bandeja_Superficie
    {
        public int id_param { get; set; }
        public int id_circuito { get; set; }
        public int Superficie { get; set; }
        public string RevisaMenor { get; set; }
        public string RevisaMayor { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        public virtual ENG_Circuitos ENG_Circuitos { get; set; }
    }
}
