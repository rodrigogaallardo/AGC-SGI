using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGI.Operaciones
{
    public class SolicitudesDto
    {
        public string tipo { get; set; }
        public int id_solicitud { get; set; }


        public int id_tipotramite { get; set; }
        public string descripcion_tipotramite { get; set; }

        public int id_tipoexpediente { get; set; }
        public string descripcion_tipoexpediente { get; set; }

        public int id_subtipoexpediente { get; set; }
        public string descripcion_subtipoexpediente { get; set; }

        public int id_estado { get; set; }
        public string estado { get; set; }


        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string CodigoSeguridad { get; set; }
        public DateTime? FechaLibrado { get; set; }





    }
}
