

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class Clanae
    {
        public Clanae()
        {
            this.Rubros = new HashSet<Rubros>();
        }
    
        public int id_clanae { get; set; }
        public string codigo_clanae { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public string descripcion { get; set; }

        [JsonIgnore]
        public virtual ICollection<Rubros> Rubros { get; set; }
    }
}
