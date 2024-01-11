

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class wsEE_Paquetes
    {
        public wsEE_Paquetes()
        {
            this.wsEE_Caratulas = new HashSet<wsEE_Caratulas>();
            this.wsEE_TareasDocumentos = new HashSet<wsEE_TareasDocumentos>();
        }
    
        public int id_paquete { get; set; }
        public int Estado_paquete { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> CreateUser { get; set; }
        public Nullable<System.DateTime> ExcluidoDeReintentos { get; set; }
    
        public virtual ICollection<wsEE_Caratulas> wsEE_Caratulas { get; set; }
        public virtual ICollection<wsEE_TareasDocumentos> wsEE_TareasDocumentos { get; set; }
    }
}
