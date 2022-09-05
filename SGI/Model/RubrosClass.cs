using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    public class RubrosClass
    {
        public int id_Encomienda { get; set; }
        public int id_EncomiendaRubro { get; set; }
        public string cod_rubro { get; set; }
        public string desc_rubro { get; set; }
        public string nom_tipoactividad { get; set; }
        public bool EsAnterior { get; set; }
        public string Nomenclatura { get; set; }
        public decimal SuperficieHabilitar { get; set; }
    }

    public class SubRubrosClass
    {
        public int id_Encomienda { get; set; }
        public int id_EncomiendaRubro { get; set; }
        public string Nombre { get; set; }
    }

    public class Depositos
    {
        public int idDeposito { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}