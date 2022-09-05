

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_TraerTipoVerificacion_Result
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> Prioridad { get; set; }
        public string DocumentacionRequerida { get; set; }
    }
}
