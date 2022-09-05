using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    public class EmailServicePOST
    {
        public int? Prioridad { get; set; }
        public string Email { get; set; }
        public string Cc { get; set; }
        public string Cco { get; set; }
        public string Asunto { get; set; }
        public int? IdTipoEmail { get; set; }
        public string Html { get; set; }
    }
}