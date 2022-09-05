

namespace SGI.Model
{
    using System;
    
    public partial class Profesional_TraerProfesionalPorMatricula_Result
    {
        public int Id { get; set; }
        public Nullable<int> IdConsejo { get; set; }
        public string Matricula { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> IdTipoDocumento { get; set; }
        public Nullable<int> NroDocumento { get; set; }
        public string Calle { get; set; }
        public string NroPuerta { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public string UnidadFuncional { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public string Email { get; set; }
        public string Sms { get; set; }
        public string Telefono { get; set; }
        public string Cuit { get; set; }
        public Nullable<long> IngresosBrutos { get; set; }
        public string Inhibido { get; set; }
    }
}
