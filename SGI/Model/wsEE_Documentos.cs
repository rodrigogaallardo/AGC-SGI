

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class wsEE_Documentos
    {
        public wsEE_Documentos()
        {
            this.wsEE_DocumentosRelacionados = new HashSet<wsEE_DocumentosRelacionados>();
        }
    
        public int id_documento { get; set; }
        public int id_paquete { get; set; }
        public byte[] Documento { get; set; }
        public string motivo_documento { get; set; }
        public string sistema_documento { get; set; }
        public string username_SADE { get; set; }
        public bool subido_SADE { get; set; }
        public string resultado_SADE { get; set; }
        public int cantidad_intentos { get; set; }
        public string numeroGEDO { get; set; }
        public string numeroEspecialGEDO { get; set; }
        public string urlArchivoGeneradoGEDO { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public string identificacion_documento { get; set; }
        public string acronimoTipoDocumento { get; set; }
        public bool embebido { get; set; }
        public string tipo_archivo { get; set; }
        public Nullable<int> id_transaccion { get; set; }
        public Nullable<int> id_file { get; set; }
        public string listaUsuariosDestinatarios { get; set; }
    
        public virtual ICollection<wsEE_DocumentosRelacionados> wsEE_DocumentosRelacionados { get; set; }
    }
}
