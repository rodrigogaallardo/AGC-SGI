

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Calles
    {
        public int id_calle { get; set; }
        public int Codigo_calle { get; set; }
        public string NombreOficial_calle { get; set; }
        public Nullable<int> AlturaIzquierdaInicio_calle { get; set; }
        public Nullable<int> AlturaIzquierdaFin_calle { get; set; }
        public Nullable<int> AlturaDerechaInicio_calle { get; set; }
        public Nullable<int> AlturaDerechaFin_calle { get; set; }
        public string NombreAnterior_calle { get; set; }
        public string TipoCalle_calle { get; set; }
        public string Observaciones_calle { get; set; }
        public Nullable<int> Longitud_calle { get; set; }
        public string NombreGeografico_calle { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string LastUpdateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
    }
}
