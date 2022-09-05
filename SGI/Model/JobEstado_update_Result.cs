

namespace SGI.Model
{
    using System;
    
    public partial class JobEstado_update_Result
    {
        public Nullable<bool> has_server_access { get; set; }
        public Nullable<bool> is_sysadmin { get; set; }
        public string actual_login_name { get; set; }
    }
}
