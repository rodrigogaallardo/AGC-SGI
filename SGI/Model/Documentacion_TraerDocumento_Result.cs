

namespace SGI.Model
{
    using System;
    
    public partial class Documentacion_TraerDocumento_Result
    {
        public int Id { get; set; }
        public int IdDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public string NombreArchivoOriginal { get; set; }
        public int NroSolicitud { get; set; }
        public string Descripcion { get; set; }
        public bool Requerido { get; set; }
    }
}
