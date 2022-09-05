

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_TraerPersona_Result
    {
        public int Id { get; set; }
        public int IdTipoPersona { get; set; }
        public string RazonSocial { get; set; }
        public string NombreFantasia { get; set; }
        public Nullable<int> IdTipoDocumento { get; set; }
        public Nullable<int> NroDocumento { get; set; }
        public string Cuit { get; set; }
        public string IngresosBrutos { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Calle { get; set; }
        public Nullable<int> NroPuerta { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public string UnidadFuncional { get; set; }
        public Nullable<int> IdLocalidad { get; set; }
        public string codigoPostal { get; set; }
        public string TelefonoArea { get; set; }
        public string TelefonoPrefijo { get; set; }
        public string TelefonoSufijo { get; set; }
        public string TelefonoMovil { get; set; }
        public string Sms { get; set; }
        public string Email { get; set; }
        public Nullable<int> NroSolicitud { get; set; }
        public Nullable<int> Matricula { get; set; }
        public string TipoDocumento { get; set; }
    }
}
