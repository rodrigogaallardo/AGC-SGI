

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENG_Circuitos
    {
        public ENG_Circuitos()
        {
            this.ENG_Rel_Circuitos_TiposDeTramite = new HashSet<ENG_Rel_Circuitos_TiposDeTramite>();
            this.ENG_Tareas = new HashSet<ENG_Tareas>();
            this.Parametros_Bandeja_Superficie = new HashSet<Parametros_Bandeja_Superficie>();
            this.Parametros_Observaciones = new HashSet<Parametros_Observaciones>();
        }
    
        public int id_circuito { get; set; }
        public string nombre_circuito { get; set; }
        public string cod_circuito { get; set; }
        public Nullable<decimal> version_circuito { get; set; }
        public string descripcion { get; set; }
        public Nullable<int> prioridad { get; set; }
        public bool activo { get; set; }
        public string nombre_grupo { get; set; }
        public Nullable<int> id_grupocircuito { get; set; }
        public Nullable<int> id_gerencia { get; set; }
    
        public virtual ENG_Gerencias ENG_Gerencias { get; set; }
        public virtual ENG_Grupos_Circuitos ENG_Grupos_Circuitos { get; set; }
        public virtual ICollection<ENG_Rel_Circuitos_TiposDeTramite> ENG_Rel_Circuitos_TiposDeTramite { get; set; }
        public virtual ICollection<ENG_Tareas> ENG_Tareas { get; set; }
        public virtual ICollection<Parametros_Bandeja_Superficie> Parametros_Bandeja_Superficie { get; set; }
        public virtual ICollection<Parametros_Observaciones> Parametros_Observaciones { get; set; }
    }
}
