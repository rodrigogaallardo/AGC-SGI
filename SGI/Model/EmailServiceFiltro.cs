using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    public class EmailServiceFiltro
    {
        public Nullable<int> id_email { get; set; }
        public Nullable<int> id_estado { get; set; }
        public Nullable<int> id_tipo_email { get; set; }
        public Nullable<int> cant_intentos { get; set; }
        public Nullable<int> cant_max_intentos { get; set; }
        public Nullable<int> prioridad { get; set; }
        public string email { get; set; }
        public string Cc { get; set; }
        public string Cco { get; set; }
        public string asunto { get; set; }
        public string html { get; set; }
        public Nullable<System.DateTime> fecha_altaDesde { get; set; }
        public Nullable<System.DateTime> fecha_altaHasta { get; set; }
        public Nullable<System.DateTime> fecha_envioDesde { get; set; }
        public Nullable<System.DateTime> fecha_envioHasta { get; set; }
    }
}