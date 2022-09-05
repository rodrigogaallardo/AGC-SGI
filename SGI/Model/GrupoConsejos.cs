

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class GrupoConsejos
    {
        public GrupoConsejos()
        {
            this.ConsejoProfesional = new HashSet<ConsejoProfesional>();
            this.ConsejoProfesional_RolesPermitidos = new HashSet<ConsejoProfesional_RolesPermitidos>();
        }
    
        public int id_grupoconsejo { get; set; }
        public string nombre_grupoconsejo { get; set; }
        public string descripcion_grupoconsejo { get; set; }
        public string logo_impresion_grupoconsejo { get; set; }
        public string logo_pantalla_grupoconsejo { get; set; }
    
        public virtual ICollection<ConsejoProfesional> ConsejoProfesional { get; set; }
        public virtual ICollection<ConsejoProfesional_RolesPermitidos> ConsejoProfesional_RolesPermitidos { get; set; }
    }
}
