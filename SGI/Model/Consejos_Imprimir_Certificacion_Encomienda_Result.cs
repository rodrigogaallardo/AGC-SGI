

namespace SGI.Model
{
    using System;
    
    public partial class Consejos_Imprimir_Certificacion_Encomienda_Result
    {
        public int id_encomienda { get; set; }
        public Nullable<System.DateTime> FechaEncomienda { get; set; }
        public int nroEncomiendaconsejo { get; set; }
        public string ConsejoProfesional { get; set; }
        public string ApellidoProfesional { get; set; }
        public string NombresProfesional { get; set; }
        public string MatriculaProfesional { get; set; }
        public string TipoNormativa { get; set; }
        public string EntidadNormativa { get; set; }
        public string NroNormativa { get; set; }
        public string CodigoSeguridad { get; set; }
        public string NombreLogo { get; set; }
        public string PlantasHabilitar { get; set; }
        public string ObservacionesPlantasHabilitar { get; set; }
    }
}
