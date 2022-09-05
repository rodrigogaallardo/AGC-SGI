

namespace SGI.Model
{
    using System;
    
    public partial class Documentacion_TraerDocumentosFaltantes_Result
    {
        public int Id { get; set; }
        public int IdTipoVerificacion { get; set; }
        public int IdTipoDocumento { get; set; }
        public string Descripcion { get; set; }
        public bool Requerido { get; set; }
    }
}
