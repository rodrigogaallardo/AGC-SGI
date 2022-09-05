

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_Profiles
    {
        public System.Guid userid { get; set; }
        public string Apellido { get; set; }
        public string Nombres { get; set; }
        public string UserName_SADE { get; set; }
        public string UserName_SIPSA { get; set; }
        public Nullable<System.Guid> CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public string Reparticion_SADE { get; set; }
        public string Sector_SADE { get; set; }
        public string Cuit { get; set; }
    
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual aspnet_Users aspnet_Users1 { get; set; }
        public virtual aspnet_Users aspnet_Users2 { get; set; }
    }
}
