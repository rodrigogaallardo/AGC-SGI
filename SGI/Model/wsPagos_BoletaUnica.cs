

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class wsPagos_BoletaUnica
    {
        public wsPagos_BoletaUnica()
        {
            this.wsPagos_BoletaUnica_HistorialEstados = new HashSet<wsPagos_BoletaUnica_HistorialEstados>();
        }
    
        public int id_pago_BU { get; set; }
        public int id_pago { get; set; }
        public long Numero_BU { get; set; }
        public Nullable<int> NroDependencia_BU { get; set; }
        public string CodigoBarras_BU { get; set; }
        public decimal Monto_BU { get; set; }
        public Nullable<System.DateTime> FechaPago_BU { get; set; }
        public string TrazaPago_BU { get; set; }
        public int EstadoPago_BU { get; set; }
        public string verificador_BU { get; set; }
        public string UpdateUser { get; set; }
        public System.Guid BUI_ID { get; set; }
        public string BUI_Numero { get; set; }
        public Nullable<System.DateTime> FechaCancelado_BU { get; set; }
        public Nullable<System.DateTime> FechaAnulado_BU { get; set; }
        public string MedioDePagoRectificado { get; set; }
        public string NumeroComprobante { get; set; }
        public string NumeroVoucher { get; set; }
        public string CantidadCuotas { get; set; }
        public string CodigoAutorizacion { get; set; }
        public string LugarDePago { get; set; }
        public Nullable<System.DateTime> FechaPago2 { get; set; }
    
        public virtual ICollection<wsPagos_BoletaUnica_HistorialEstados> wsPagos_BoletaUnica_HistorialEstados { get; set; }
        public virtual wsPagos wsPagos { get; set; }
        public virtual wsPagos_BoletaUnica_Estados wsPagos_BoletaUnica_Estados { get; set; }
    }
}
