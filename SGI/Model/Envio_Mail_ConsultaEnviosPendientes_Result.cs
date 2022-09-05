

namespace SGI.Model
{
    using System;
    
    public partial class Envio_Mail_ConsultaEnviosPendientes_Result
    {
        public int id_proceso { get; set; }
        public string descripcion { get; set; }
        public string cfg_html { get; set; }
        public string config_mail_from { get; set; }
        public string cfg_smtp { get; set; }
        public Nullable<int> cfg_smtp_puerto { get; set; }
        public string cfg_smtp_usuario { get; set; }
        public string cfg_smtp_clave { get; set; }
        public int id_envio_mail { get; set; }
        public Nullable<int> id_origen { get; set; }
        public Nullable<int> prioridad { get; set; }
        public string email { get; set; }
        public Nullable<int> cant_intentos { get; set; }
        public Nullable<int> cant_max_intentos { get; set; }
        public Nullable<System.DateTime> fecha_alta { get; set; }
        public Nullable<int> id_estado { get; set; }
        public string asunto { get; set; }
        public string html { get; set; }
    }
}
