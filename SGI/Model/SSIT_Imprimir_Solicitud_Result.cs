

namespace SGI.Model
{
    using System;
    
    public partial class SSIT_Imprimir_Solicitud_Result
    {
        public int id_solicitud { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string CodigoSeguridad { get; set; }
        public string TipoDeTramite { get; set; }
        public string TipoDeExpediente { get; set; }
        public string SubTipoDeExpediente { get; set; }
    }
}
