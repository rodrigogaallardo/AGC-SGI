

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class wsPagos
    {
        public wsPagos()
        {
            this.wsPagos_BoletaUnica = new HashSet<wsPagos_BoletaUnica>();
            this.SGI_Solicitudes_Pagos = new HashSet<SGI_Solicitudes_Pagos>();
            this.SSIT_Solicitudes_Pagos = new HashSet<SSIT_Solicitudes_Pagos>();
        }
    
        public int id_pago { get; set; }
        public string TipoPersona { get; set; }
        public string TipoDoc { get; set; }
        public string ApellidoyNombre { get; set; }
        public string Documento { get; set; }
        public string Direccion { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public string Localidad { get; set; }
        public string CodPost { get; set; }
        public string Email { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
    
        public virtual ICollection<wsPagos_BoletaUnica> wsPagos_BoletaUnica { get; set; }
        public virtual ICollection<SGI_Solicitudes_Pagos> SGI_Solicitudes_Pagos { get; set; }
        public virtual ICollection<SSIT_Solicitudes_Pagos> SSIT_Solicitudes_Pagos { get; set; }
    }
}
