

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class aspnet_Applications
    {
        public aspnet_Applications()
        {
            this.aspnet_Membership = new HashSet<aspnet_Membership>();
            this.aspnet_Users = new HashSet<aspnet_Users>();
            this.aspnet_Roles = new HashSet<aspnet_Roles>();
        }
    
        public string ApplicationName { get; set; }
        public string LoweredApplicationName { get; set; }
        public System.Guid ApplicationId { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<aspnet_Membership> aspnet_Membership { get; set; }
        public virtual ICollection<aspnet_Users> aspnet_Users { get; set; }
        public virtual ICollection<aspnet_Roles> aspnet_Roles { get; set; }
    }
}
