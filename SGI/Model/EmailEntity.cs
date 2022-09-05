using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    public class EmailEntity
    {
        public int IdEmail { get; set; }
        public Guid? Guid { get; set; }
        public int IdOrigen { get; set; }
        public int IdTipoEmail { get; set; }
        public int IdEstado { get; set; }
        public int? CantIntentos { get; set; }
        public int? CantMaxIntentos { get; set; }
        public int? Prioridad { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string Email { get; set; }
        public string Cc { get; set; }
        public string Cco { get; set; }
        public string Asunto { get; set; }
        public string Html { get; set; }
        public DateTime? FechaLectura { get; set; }
    }
}