

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Titulares_PersonasJuridicas
    {
        public SSIT_Solicitudes_Titulares_PersonasJuridicas()
        {
            this.SSIT_Solicitudes_Firmantes_PersonasJuridicas = new HashSet<SSIT_Solicitudes_Firmantes_PersonasJuridicas>();
            this.SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas = new HashSet<SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas>();
        }
    
        public int id_personajuridica { get; set; }
        public int id_solicitud { get; set; }
        public int Id_TipoSociedad { get; set; }
        public string Razon_Social { get; set; }
        public string CUIT { get; set; }
        public int id_tipoiibb { get; set; }
        public string Nro_IIBB { get; set; }
        public string Calle { get; set; }
        public Nullable<int> NroPuerta { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public int id_localidad { get; set; }
        public string Codigo_Postal { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public System.Guid CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public string Torre { get; set; }
    
        public virtual Localidad Localidad { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Firmantes_PersonasJuridicas> SSIT_Solicitudes_Firmantes_PersonasJuridicas { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas> SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas { get; set; }
        public virtual TiposDeIngresosBrutos TiposDeIngresosBrutos { get; set; }
        public virtual TipoSociedad TipoSociedad { get; set; }
        public virtual SSIT_Solicitudes SSIT_Solicitudes { get; set; }
    }
}
