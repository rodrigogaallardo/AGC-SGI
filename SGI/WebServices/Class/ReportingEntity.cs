using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalService.Class
{
    public class ReportingEntity
    {
        public int Id_file { get; set; }
        public string FileName { get; set; }
        public byte[] Reporte { get; set; }
    }
}
