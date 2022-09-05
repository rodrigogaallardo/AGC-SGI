

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_DatosLocal
    {
        public Encomienda_DatosLocal()
        {
            this.Encomienda_Certificado_Sobrecarga = new HashSet<Encomienda_Certificado_Sobrecarga>();
        }
    
        public int id_encomiendadatoslocal { get; set; }
        public int id_encomienda { get; set; }
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
        public bool sobrecarga_corresponde_dl { get; set; }
        public Nullable<int> sobrecarga_tipo_observacion { get; set; }
        public Nullable<int> sobrecarga_requisitos_opcion { get; set; }
        public string sobrecarga_art813_inciso { get; set; }
        public string sobrecarga_art813_item { get; set; }
        public Nullable<int> cantidad_operarios_dl { get; set; }
        public Nullable<double> local_venta { get; set; }
        public Nullable<bool> cumple_ley_962 { get; set; }
        public Nullable<bool> salubridad_especial { get; set; }
        public Nullable<bool> eximido_ley_962 { get; set; }
        public Nullable<bool> ampliacion_superficie { get; set; }
        public Nullable<decimal> superficie_cubierta_amp { get; set; }
        public Nullable<decimal> superficie_descubierta_amp { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public bool dj_certificado_sobrecarga { get; set; }
        public bool estacionamientoBicicleta_dl { get; set; }
    
        public virtual ICollection<Encomienda_Certificado_Sobrecarga> Encomienda_Certificado_Sobrecarga { get; set; }
        public virtual Encomienda Encomienda { get; set; }
    }
}
