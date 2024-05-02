

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class RubrosCondiciones
    {
        public RubrosCondiciones()
        {
            this.RubrosZonasCondiciones = new HashSet<RubrosZonasCondiciones>();
        }
    
        public int id_condicion { get; set; }
        public string cod_condicion { get; set; }
        public string nom_condicion { get; set; }
        public int SupMin_condicion { get; set; }
        public int SupMax_condicion { get; set; }

        [JsonIgnore]
        public virtual ICollection<RubrosZonasCondiciones> RubrosZonasCondiciones { get; set; }
    }
}
