

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Titulares_PersonasFisicas
    {
        public SSIT_Solicitudes_Titulares_PersonasFisicas()
        {
            this.SSIT_Solicitudes_Firmantes_PersonasFisicas = new HashSet<SSIT_Solicitudes_Firmantes_PersonasFisicas>();
        }
    
        public int id_personafisica { get; set; }
        public int id_solicitud { get; set; }
        public string Apellido { get; set; }
        public string Nombres { get; set; }
        public int id_tipodoc_personal { get; set; }
        public string Nro_Documento { get; set; }
        public string Cuit { get; set; }
        public int id_tipoiibb { get; set; }
        public string Ingresos_Brutos { get; set; }
        public string Calle { get; set; }
        public int Nro_Puerta { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public int Id_Localidad { get; set; }
        public string Codigo_Postal { get; set; }
        public string TelefonoMovil { get; set; }
        public string Sms { get; set; }
        public string Email { get; set; }
        public bool MismoFirmante { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<System.DateTime> LastupdateDate { get; set; }
        public string Telefono { get; set; }
        public string Torre { get; set; }
    
        public virtual Localidad Localidad { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Firmantes_PersonasFisicas> SSIT_Solicitudes_Firmantes_PersonasFisicas { get; set; }
        public virtual TipoDocumentoPersonal TipoDocumentoPersonal { get; set; }
        public virtual TiposDeIngresosBrutos TiposDeIngresosBrutos { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
    }
}
