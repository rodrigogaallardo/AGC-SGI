using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    [Serializable]
    public class RubrosCN_Config_IncendioDTO
    {
        public int id_rubro_incendio { get; set; }
        public int id_rubro { get; set; }
        public int riesgo { get; set; }
        public decimal DesdeM2 { get; set; }
        public decimal HastaM2 { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CreateUser { get; set; }
    }
}