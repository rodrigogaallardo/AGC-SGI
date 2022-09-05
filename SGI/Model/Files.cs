

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Files
    {
        public System.Guid rowid { get; set; }
        public int id_file { get; set; }
        public byte[] content_file { get; set; }
        public string datos_documento_oficial { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string FileName { get; set; }
        public byte[] Md5 { get; set; }
    }
}
