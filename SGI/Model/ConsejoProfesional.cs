

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConsejoProfesional
    {
        public ConsejoProfesional()
        {
            this.Profesional = new HashSet<Profesional>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Profesion { get; set; }
        public string Calle { get; set; }
        public string NroPuerta { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Contacto { get; set; }
        public Nullable<int> id_consejo_habilitaciones { get; set; }
        public int id_grupoconsejo { get; set; }
    
        public virtual GrupoConsejos GrupoConsejos { get; set; }
        public virtual ICollection<Profesional> Profesional { get; set; }
    }
}
