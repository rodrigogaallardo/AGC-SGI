

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CPadron_DatosLocal
    {
        public int id_cpadrondatoslocal { get; set; }
        public int id_cpadron { get; set; }
        public Nullable<decimal> superficie_cubierta_dl { get; set; }
        public Nullable<decimal> superficie_descubierta_dl { get; set; }
        public Nullable<decimal> dimesion_frente_dl { get; set; }
        public bool lugar_carga_descarga_dl { get; set; }
        public bool estacionamiento_dl { get; set; }
        public bool red_transito_pesado_dl { get; set; }
        public bool sobre_avenida_dl { get; set; }
        public string materiales_pisos_dl { get; set; }
        public string materiales_paredes_dl { get; set; }
        public string materiales_techos_dl { get; set; }
        public string materiales_revestimientos_dl { get; set; }
        public int sanitarios_ubicacion_dl { get; set; }
        public Nullable<decimal> sanitarios_distancia_dl { get; set; }
        public string croquis_ubicacion_dl { get; set; }
        public Nullable<int> cantidad_sanitarios_dl { get; set; }
        public Nullable<decimal> superficie_sanitarios_dl { get; set; }
        public Nullable<decimal> frente_dl { get; set; }
        public Nullable<decimal> fondo_dl { get; set; }
        public Nullable<decimal> lateral_izquierdo_dl { get; set; }
        public Nullable<decimal> lateral_derecho_dl { get; set; }
        public Nullable<int> cantidad_operarios_dl { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<decimal> local_venta { get; set; }
        public Nullable<bool> dj_certificado_sobrecarga { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        public virtual CPadron_Solicitudes CPadron_Solicitudes { get; set; }
    }
}
