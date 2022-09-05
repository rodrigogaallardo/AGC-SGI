

namespace SGI.Model
{
    using System;
    
    public partial class RULH_GENERAR_HABILITACIONES_Result
    {
        public Nullable<int> id { get; set; }
        public Nullable<int> EXPEDIENTE_ID { get; set; }
        public string NRO_EXPEDIENTE { get; set; }
        public Nullable<int> NRO_SOLICITUD { get; set; }
        public Nullable<System.DateTime> FECHA_HABILITACION { get; set; }
        public Nullable<System.DateTime> FECHA_SOLICITUD { get; set; }
        public string TIPO_EXPEDIENTE { get; set; }
        public Nullable<decimal> SUPERFICIE { get; set; }
        public string COD_ZONA { get; set; }
        public string DESCRIPCION_ZONA { get; set; }
        public string CALIFICADOR { get; set; }
        public string NUMERO_DISPOSICION { get; set; }
        public string NRO_NORMATIVA { get; set; }
        public string ANIO_NORMATIVA { get; set; }
        public string ENTIDAD_NORMATIVA { get; set; }
        public string TIPO_NORMATIVA { get; set; }
        public string TIPO_SOCIEDAD { get; set; }
        public Nullable<int> CANTIDAD_OPERARIOS { get; set; }
        public Nullable<System.DateTime> FECHA_ALTA { get; set; }
        public Nullable<System.DateTime> FECHA_MODIFICACION { get; set; }
        public string USUARIO_ID_ALTA { get; set; }
        public string USUARIO_ID_MODIFICACION { get; set; }
        public Nullable<System.DateTime> FECHA_INICIO { get; set; }
        public string OBSERVACIONES_CALIFICADOR { get; set; }
        public string ESTADO { get; set; }
    }
}
