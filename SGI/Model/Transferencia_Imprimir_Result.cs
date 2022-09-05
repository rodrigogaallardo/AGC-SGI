

namespace SGI.Model
{
    using System;
    
    public partial class Transferencia_Imprimir_Result
    {
        public int id_cpadron { get; set; }
        public string CodigoSeguridad { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string ZonaDeclarada { get; set; }
        public string TipoDeTramite { get; set; }
        public string TipoDeExpediente { get; set; }
        public string SubTipoDeExpediente { get; set; }
        public Nullable<int> MatriculaProfesional { get; set; }
        public string NombreApellidoProfesional { get; set; }
        public string TipoNormativa { get; set; }
        public string EntidadNormativa { get; set; }
        public string NroNormativa { get; set; }
        public Nullable<bool> ImpresionDePrueba { get; set; }
        public string PlantasHabilitar { get; set; }
        public string ObservacionesPlantasHabilitar { get; set; }
    }
}
