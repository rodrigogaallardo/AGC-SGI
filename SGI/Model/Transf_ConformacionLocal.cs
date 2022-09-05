

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transf_ConformacionLocal
    {
        public int id_transfconflocal { get; set; }
        public int id_solicitud { get; set; }
        public int id_destino { get; set; }
        public Nullable<decimal> largo_conflocal { get; set; }
        public Nullable<decimal> ancho_conflocal { get; set; }
        public Nullable<decimal> alto_conflocal { get; set; }
        public string Paredes_conflocal { get; set; }
        public string Techos_conflocal { get; set; }
        public string Pisos_conflocal { get; set; }
        public string Frisos_conflocal { get; set; }
        public string Observaciones_conflocal { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> Updateuser { get; set; }
        public string Detalle_conflocal { get; set; }
        public Nullable<int> id_transftiposector { get; set; }
        public Nullable<int> id_ventilacion { get; set; }
        public Nullable<int> id_iluminacion { get; set; }
        public Nullable<decimal> superficie_conflocal { get; set; }
        public int id_tiposuperficie { get; set; }
    
        public virtual tipo_iluminacion tipo_iluminacion { get; set; }
        public virtual tipo_ventilacion tipo_ventilacion { get; set; }
        public virtual TipoDestino TipoDestino { get; set; }
        public virtual TipoSuperficie TipoSuperficie { get; set; }
        public virtual Transf_Plantas Transf_Plantas { get; set; }
        public virtual Transf_Solicitudes Transf_Solicitudes { get; set; }
    }
}
