

namespace SGI.Model
{
    using System;
    
    public partial class Cpadron_Imprimir_Solicitud_Result
    {
        public int IdCpadron { get; set; }
        public string CodigoSeguridad { get; set; }
        public string ZonaDeclarada { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string TipoTramite { get; set; }
        public Nullable<int> NroMatriculaEscribano { get; set; }
        public string NombreEscribano { get; set; }
        public string TipoNormativa { get; set; }
        public string TipoEntidad { get; set; }
        public string NroNormativa { get; set; }
        public string ObservacionesInternas { get; set; }
        public string PlantasHabilitar { get; set; }
    }
}
