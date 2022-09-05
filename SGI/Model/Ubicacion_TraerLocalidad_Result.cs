

namespace SGI.Model
{
    using System;
    
    public partial class Ubicacion_TraerLocalidad_Result
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> IdProvincia { get; set; }
        public string Provincia { get; set; }
    }
}
