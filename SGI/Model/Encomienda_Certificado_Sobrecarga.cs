

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Encomienda_Certificado_Sobrecarga
    {
        public int id_sobrecarga { get; set; }
        public int id_encomienda_datoslocal { get; set; }
        public int id_tipo_certificado { get; set; }
        public int id_tipo_sobrecarga { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual Encomienda_DatosLocal Encomienda_DatosLocal { get; set; }
    }
}
