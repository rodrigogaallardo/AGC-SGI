

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class wsEE_Caratulas
    {
        public int id_caratula { get; set; }
        public int id_paquete { get; set; }
        public int tipo_persona_caratula { get; set; }
        public string apellido_caratula { get; set; }
        public string nombre_caratula { get; set; }
        public string razon_social_caratula { get; set; }
        public string tipo_doc_caratula { get; set; }
        public Nullable<decimal> nro_doc_caratula { get; set; }
        public string cuit_cuil_caratula { get; set; }
        public string domicilio_caratula { get; set; }
        public string piso_caratula { get; set; }
        public string departamento_caratula { get; set; }
        public string codigo_postal_caratula { get; set; }
        public string email_caratula { get; set; }
        public string telefono_caratula { get; set; }
        public string sistema_caratula { get; set; }
        public string descripcion_caratula { get; set; }
        public string motivo_caratula { get; set; }
        public string motivo_externo_caratula { get; set; }
        public string trata_caratula { get; set; }
        public string username_SADE { get; set; }
        public bool generado_SADE { get; set; }
        public string resultado_SADE { get; set; }
        public int cantidad_intentos { get; set; }
        public string tipo_actuacion_exp { get; set; }
        public Nullable<int> anio_actuacion_exp { get; set; }
        public Nullable<int> nro_actuacion_exp { get; set; }
        public string reparticion_actuacion_exp { get; set; }
        public string reparticion_usuario_exp { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
    }
}
