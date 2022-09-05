

namespace SGI.Model
{
    using System;
    
    public partial class Encomienda_Imprimir_Encomienda_Result
    {
        public int id_encomienda { get; set; }
        public System.DateTime FechaEncomienda { get; set; }
        public int nroEncomiendaconsejo { get; set; }
        public string ZonaDeclarada { get; set; }
        public string TipoDeTramite { get; set; }
        public string TipoDeExpediente { get; set; }
        public string SubTipoDeExpediente { get; set; }
        public string MatriculaProfesional { get; set; }
        public string ApellidoProfesional { get; set; }
        public string NombresProfesional { get; set; }
        public string TipoDocProfesional { get; set; }
        public Nullable<int> DocumentoProfesional { get; set; }
        public int id_grupoconsejo { get; set; }
        public string ConsejoProfesional { get; set; }
        public string TipoNormativa { get; set; }
        public string EntidadNormativa { get; set; }
        public string NroNormativa { get; set; }
        public string LogoUrl { get; set; }
        public Nullable<bool> ImpresionDePrueba { get; set; }
        public string PlantasHabilitar { get; set; }
        public string ObservacionesPlantasHabilitar { get; set; }
        public string ObservacionesRubros { get; set; }
        public Nullable<int> id_encomienda_anterior { get; set; }
        public string NroExpediente { get; set; }
        public string NombreLogo { get; set; }
    }
}
