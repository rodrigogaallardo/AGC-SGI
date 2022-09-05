

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class sysjobs_historial
    {
        public Nullable<System.DateTime> ultima_ejec { get; set; }
        public string name { get; set; }
        public int run_status { get; set; }
        public string run_status_desc { get; set; }
    }
}
