

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_LIZA_Ticket
    {
        public SGI_LIZA_Ticket()
        {
            this.SGI_LIZA_Procesos = new HashSet<SGI_LIZA_Procesos>();
        }
    
        public int id_ticket { get; set; }
        public int id_tramitetarea { get; set; }
        public string Alias { get; set; }
        public string UrlSeguimiento { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
    
        public virtual ICollection<SGI_LIZA_Procesos> SGI_LIZA_Procesos { get; set; }
        public virtual SGI_Tramites_Tareas SGI_Tramites_Tareas { get; set; }
    }
}
