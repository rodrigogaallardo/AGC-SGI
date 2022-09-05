

namespace SGI.Model
{
    using System;
    
    public partial class Consejos_Imprimir_Certificacion_EncomiendaAnt_Result
    {
        public int id_encomienda { get; set; }
        public int id_profesional { get; set; }
        public System.DateTime FechaEncomienda { get; set; }
        public string tipoTramite { get; set; }
        public string ApellidoProfesional { get; set; }
        public string NombresProfesional { get; set; }
        public string MatriculaProfesional { get; set; }
        public string ConsejoProfesional { get; set; }
        public string CodigoSeguridad { get; set; }
        public string tipoDocProfesional { get; set; }
        public Nullable<int> nroDocProfesional { get; set; }
    }
}
