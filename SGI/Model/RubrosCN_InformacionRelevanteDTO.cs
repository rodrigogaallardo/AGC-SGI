using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    [Serializable]
    public class RubrosCN_InformacionRelevanteDTO
    {
        public int id_rubinf { get; set; }
        public int id_rubro { get; set; }
        public string descripcion_rubinf { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CreateUser { get; set; }
    }
}