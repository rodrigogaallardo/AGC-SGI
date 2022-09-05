

namespace SGI.Model
{
    using System;
    
    public partial class Usuario_TraerLista_Result
    {
        public System.Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Calle { get; set; }
        public Nullable<int> NroPuerta { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public string CodPostal { get; set; }
        public Nullable<int> IdLocalidad { get; set; }
        public Nullable<int> IdProvincia { get; set; }
        public string Movil { get; set; }
        public string TelefonoArea { get; set; }
        public string TelefonoPrefijo { get; set; }
        public string TelefonoSufijo { get; set; }
        public string Sms { get; set; }
    }
}
