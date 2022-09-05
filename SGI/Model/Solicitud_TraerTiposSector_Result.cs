

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_TraerTiposSector_Result
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public Nullable<bool> Ocultar { get; set; }
        public Nullable<bool> MuestraCampoAdicional { get; set; }
        public Nullable<int> TamanoCampoAdicional { get; set; }
    }
}
