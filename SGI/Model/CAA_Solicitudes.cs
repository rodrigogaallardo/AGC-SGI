

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CAA_Solicitudes
    {
        public int id_solicitud { get; set; }
        public int id_tipotramite { get; set; }
        public int id_paquete { get; set; }
        public string CodigoSeguridad { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public int id_tipocertificado { get; set; }
        public int id_estado { get; set; }
        public string NroCertificado { get; set; }
        public Nullable<System.DateTime> FechaVencCertificado { get; set; }
        public Nullable<int> NroActuacion { get; set; }
        public Nullable<int> AnioActuacion { get; set; }
        public Nullable<int> TipoActoAdministrativo { get; set; }
        public Nullable<int> NroActoAdministrativo { get; set; }
        public Nullable<int> EntidadActoAdministrativo { get; set; }
        public Nullable<int> AnioActoAdministrativo { get; set; }
        public bool ExentoBUI { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public string NroExpedienteSADE { get; set; }
        public int id_caa { get; set; }
        public int id_encomienda_agc { get; set; }
    }
}
