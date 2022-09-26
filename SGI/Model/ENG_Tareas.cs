

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Tareas
    {
        public ENG_Tareas()
        {
            this.ENG_Rel_GruposDeTareas_Tareas = new HashSet<ENG_Rel_GruposDeTareas_Tareas>();
            this.ENG_Rel_Resultados_Tareas = new HashSet<ENG_Rel_Resultados_Tareas>();
            this.ENG_Rel_Perfiles_Tareas = new HashSet<ENG_Rel_Perfiles_Tareas>();
            this.ENG_Config_BandejaAsignacion = new HashSet<ENG_Config_BandejaAsignacion>();
            this.ENG_Transiciones = new HashSet<ENG_Transiciones>();
            this.ENG_Transiciones1 = new HashSet<ENG_Transiciones>();
            this.SGI_Tramites_Tareas = new HashSet<SGI_Tramites_Tareas>();
            this.SGI_Tramites_Tareas1 = new HashSet<SGI_Tramites_Tareas>();
            this.Rel_TiposDeDocumentosRequeridos_ENG_Tareas = new HashSet<Rel_TiposDeDocumentosRequeridos_ENG_Tareas>();
            this.SGI_Tareas_Pases_Sectores = new HashSet<SGI_Tareas_Pases_Sectores>();
            this.SGI_Tareas_Pases_Sectores1 = new HashSet<SGI_Tareas_Pases_Sectores>();
        }
    
        public int id_tarea { get; set; }
        public int id_circuito { get; set; }
        public int cod_tarea { get; set; }
        public string nombre_tarea { get; set; }
        public string funcion_entrar_tarea { get; set; }
        public string funcion_salir_tarea { get; set; }
        public bool Asignable_tarea { get; set; }
        public string formulario_tarea { get; set; }
        public bool visible_en_configuracion { get; set; }
        public Nullable<int> id_tipo_tarea { get; set; }
    
        public virtual ICollection<ENG_Rel_GruposDeTareas_Tareas> ENG_Rel_GruposDeTareas_Tareas { get; set; }
        public virtual ICollection<ENG_Rel_Resultados_Tareas> ENG_Rel_Resultados_Tareas { get; set; }
        public virtual ICollection<ENG_Rel_Perfiles_Tareas> ENG_Rel_Perfiles_Tareas { get; set; }
        public virtual ICollection<ENG_Config_BandejaAsignacion> ENG_Config_BandejaAsignacion { get; set; }
        public virtual ICollection<ENG_Transiciones> ENG_Transiciones { get; set; }
        public virtual ICollection<ENG_Transiciones> ENG_Transiciones1 { get; set; }
        public virtual ICollection<SGI_Tramites_Tareas> SGI_Tramites_Tareas { get; set; }
        public virtual ICollection<SGI_Tramites_Tareas> SGI_Tramites_Tareas1 { get; set; }
        public virtual ICollection<Rel_TiposDeDocumentosRequeridos_ENG_Tareas> Rel_TiposDeDocumentosRequeridos_ENG_Tareas { get; set; }
        public virtual ICollection<SGI_Tareas_Pases_Sectores> SGI_Tareas_Pases_Sectores { get; set; }
        public virtual ICollection<SGI_Tareas_Pases_Sectores> SGI_Tareas_Pases_Sectores1 { get; set; }
        public virtual ENG_Tipos_Tareas ENG_Tipos_Tareas { get; set; }
        public virtual ENG_Circuitos ENG_Circuitos { get; set; }
    }
}
