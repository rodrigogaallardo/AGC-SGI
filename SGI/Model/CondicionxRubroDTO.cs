using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    public class CondicionxRubroDTO
    {
        public string CodigoRubro { get; set; }
        public string Descripcion { get; set; }    
        public string Zona { get; set; }
        public string Condicion { get; set; }
        public decimal SupMinima { get; set; }
        public decimal SupMaxima { get; set; }

    }
}