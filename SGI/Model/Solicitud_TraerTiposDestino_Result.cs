

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_TraerTiposDestino_Result
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<bool> RequiereObservaciones { get; set; }
        public bool RequiereDetalle { get; set; }
    }
}
