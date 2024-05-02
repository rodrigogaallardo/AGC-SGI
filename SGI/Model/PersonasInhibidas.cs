

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class PersonasInhibidas
    {
        public int id_personainhibida { get; set; }
        public Nullable<int> id_tipodoc_personal { get; set; }
        public Nullable<int> nrodoc_personainhibida { get; set; }
        public int nroorden_personainhibida { get; set; }
        public string cuit_personainhibida { get; set; }
        public string nomape_personainhibida { get; set; }
        public Nullable<System.DateTime> fecharegistro_personainhibida { get; set; }
        public Nullable<System.DateTime> fechavencimiento_personainhibida { get; set; }
        public string autos_personainhibida { get; set; }
        public string juzgado_personainhibida { get; set; }
        public string secretaria_personainhibida { get; set; }
        public int estado_personainhibida { get; set; }
        public Nullable<System.DateTime> fechabaja_personainhibida { get; set; }
        public Nullable<int> operador_personainhibida { get; set; }
        public string observaciones_personainhibida { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public string LastUpdateUser { get; set; }
        public string MotivoLevantamiento { get; set; }
        public Nullable<int> id_tipopersona { get; set; }

        [JsonIgnore]
        public virtual TipoDocumentoPersonal TipoDocumentoPersonal { get; set; }
        [JsonIgnore]
        public virtual TipoPersona TipoPersona { get; set; }
    }
}
