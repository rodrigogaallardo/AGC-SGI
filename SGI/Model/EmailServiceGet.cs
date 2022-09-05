using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    public class Registros
    {
        public int Total { get; set; }
        public List<EmailServiceGet> Emails { get; set; }
    }
    public class EmailServiceGet
    {
        public int id_email { get; set; }
        public int id_estado { get; set; }
        public int id_tipo_email { get; set; }
        public Nullable<int> cant_intentos { get; set; }
        public Nullable<int> cant_max_intentos { get; set; }
        public Nullable<int> prioridad { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public Nullable<System.DateTime> fecha_envio { get; set; }
        public string email { get; set; }
        public string Cc { get; set; }
        public string Cco { get; set; }
        public string asunto { get; set; }
        public string html { get; set; }
        public virtual Email_EstadosDTO Estado { get; set; }
    }
}