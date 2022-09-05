using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SGI.Model
{
    [Serializable]
    public class RubrosCN_TiposDeDocumentosRequeridosDTO
    {
        public int id_rubro { get; set; }
        public int id_rubtdocreq { get; set; }
        public int id_tdocreq { get; set; }
        public string nombre_tdocreq { get; set; }
        public string observaciones_tdocreq { get; set; }
        public bool baja_tdocreq { get; set; }
        public bool obligatorio_rubtdocreq { get; set; }
        public string es_obligatorio { get; set; }
        public string Accion { get; set; }
        public string Color { get; set; }
    }
}