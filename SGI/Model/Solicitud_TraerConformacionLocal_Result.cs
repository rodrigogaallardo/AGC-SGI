

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_TraerConformacionLocal_Result
    {
        public int Id { get; set; }
        public int IdDestino { get; set; }
        public double Largo { get; set; }
        public double Ancho { get; set; }
        public Nullable<double> Alto { get; set; }
        public string Paredes { get; set; }
        public string Techos { get; set; }
        public string Pisos { get; set; }
        public string Frisos { get; set; }
        public string Observaciones { get; set; }
        public int NroSolicitud { get; set; }
        public string Destino { get; set; }
    }
}
