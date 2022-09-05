

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class sysjobs_vista
    {
        public string name { get; set; }
        public string Estado { get; set; }
        public string active_start_time { get; set; }
        public string active_end_time { get; set; }
        public int freq_subday_interval { get; set; }
        public int freq_subday_type { get; set; }
        public string freq_subday_type_desc { get; set; }
        public Nullable<int> intervalo_minutos { get; set; }
    }
}
