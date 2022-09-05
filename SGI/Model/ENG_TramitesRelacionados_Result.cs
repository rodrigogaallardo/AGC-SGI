

namespace SGI.Model
{
    using System;
    
    public partial class ENG_TramitesRelacionados_Result
    {
        public Nullable<int> id_tramitetarea { get; set; }
        public Nullable<int> id_solicitud { get; set; }
        public Nullable<int> id_tarea { get; set; }
        public Nullable<System.DateTime> FechaInicio_tramitetarea { get; set; }
        public Nullable<System.DateTime> FechaAsignacion_tramtietarea { get; set; }
        public Nullable<int> id_tipotramite { get; set; }
        public Nullable<int> id_tipoexpediente { get; set; }
        public Nullable<int> id_subtipoexpediente { get; set; }
        public string direccion { get; set; }
        public string nombre_tarea { get; set; }
        public Nullable<bool> asignable_tarea { get; set; }
        public string formulario_tarea { get; set; }
        public string descripcion_tipotramite { get; set; }
        public string descripcion_tipoexpediente { get; set; }
        public string descripcion_subtipoexpediente { get; set; }
        public string descripcion_tramite { get; set; }
        public Nullable<int> Dias_Transcurridos { get; set; }
        public Nullable<System.Guid> UsuarioAsignado_tramitetarea { get; set; }
        public string nombre_UsuarioAsignado_tramitetarea { get; set; }
        public Nullable<int> cant_reg { get; set; }
        public string descripcion_estado { get; set; }
    }
}
