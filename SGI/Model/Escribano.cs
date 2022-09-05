

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Escribano
    {
        public Escribano()
        {
            this.wsEscribanos_ActaNotarial = new HashSet<wsEscribanos_ActaNotarial>();
            this.Solicitud = new HashSet<Solicitud>();
        }
    
        public int Matricula { get; set; }
        public Nullable<int> Registro { get; set; }
        public string ApyNom { get; set; }
        public Nullable<int> Cargo { get; set; }
        public string Calle { get; set; }
        public string NroPuerta { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public Nullable<int> CodPostal { get; set; }
        public string Localidad { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Inhibido { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        public virtual ICollection<wsEscribanos_ActaNotarial> wsEscribanos_ActaNotarial { get; set; }
        public virtual ICollection<Solicitud> Solicitud { get; set; }
    }
}
