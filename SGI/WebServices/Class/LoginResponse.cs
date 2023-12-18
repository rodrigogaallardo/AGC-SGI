using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalService.Class
{
    public class LoginResponse
    {
        public string token { get; set; }
        public bool success { get; set; }
        public DateTime expires { get; set; }
        public object errors { get; set; }
    }
}