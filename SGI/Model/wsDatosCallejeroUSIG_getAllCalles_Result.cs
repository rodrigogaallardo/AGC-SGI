

namespace SGI.Model
{
    using System;
    
    public partial class wsDatosCallejeroUSIG_getAllCalles_Result
    {
        public int Codigo_calle { get; set; }
        public string NombreOficial_calle { get; set; }
        public Nullable<int> AlturaDerechaInicio_calle { get; set; }
        public Nullable<int> AlturaDerechaFin_calle { get; set; }
        public Nullable<int> AlturaIzquierdaInicio_calle { get; set; }
        public Nullable<int> AlturaIzquierdaFin_calle { get; set; }
        public string TipoCalle_calle { get; set; }
        public System.DateTime fecha_operacion { get; set; }
        public string tipo_operacion { get; set; }
    }
}
