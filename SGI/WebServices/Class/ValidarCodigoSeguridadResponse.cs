using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalService.Class
{
    public class ValidarCodigoSeguridadResponse
    {
        public bool EsValido { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
    }
}