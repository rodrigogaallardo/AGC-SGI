

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class wsEE_TareasDocumentos
    {
        public int id_tarea_documento { get; set; }
        public int id_paquete { get; set; }
        public byte[] Documento { get; set; }
        public Nullable<int> id_metaDatos { get; set; }
        public string motivo_firma { get; set; }
        public string sistema_firma { get; set; }
        public string username_SADE { get; set; }
        public string username_SADE_emisor { get; set; }
        public Nullable<int> id_firmantes { get; set; }
        public bool tarea_subida_SADE { get; set; }
        public string resultado_tarea_SADE { get; set; }
        public int cantidad_intentos { get; set; }
        public string id_proceso { get; set; }
        public string username_SADE_apoderado { get; set; }
        public bool firmado_en_SADE { get; set; }
        public string numeroGEDO { get; set; }
        public Nullable<int> cantidad_intentos_recuperar_numero { get; set; }
        public string resultado_recuperar_numero { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public string username_SADE_receptor { get; set; }
        public string acronimoTipoDocumento { get; set; }
        public Nullable<System.DateTime> fecha_subida_SADE { get; set; }
        public Nullable<System.DateTime> fecha_firmado_SADE { get; set; }
        public Nullable<int> id_file { get; set; }

        public virtual wsEE_Paquetes wsEE_Paquetes { get; set; }
    }
}
