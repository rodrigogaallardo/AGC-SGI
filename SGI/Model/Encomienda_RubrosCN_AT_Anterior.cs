

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_RubrosCN_AT_Anterior
    {
        public int id_encomiendarubro { get; set; }
        public int id_encomienda { get; set; }
        public int IdRubro { get; set; }
        public string CodigoRubro { get; set; }
        public string NombreRubro { get; set; }
        public int IdTipoActividad { get; set; }
        public int IdTipoExpediente { get; set; }
        public decimal SuperficieHabilitar { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> CreateUser { get; set; }
        public Nullable<int> idImpactoAmbiental { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual ImpactoAmbiental ImpactoAmbiental { get; set; }
        public virtual TipoActividad TipoActividad { get; set; }
        public virtual TipoExpediente TipoExpediente { get; set; }
        public virtual Encomienda Encomienda { get; set; }
        public virtual RubrosCN RubrosCN { get; set; }
    }
}
