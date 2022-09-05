

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_TraerRubro_Result
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string TipoActividad { get; set; }
        public int Minimo { get; set; }
        public int Maximo { get; set; }
        public int Tipo_Actividad { get; set; }
        public int Tipo_Doc_Id { get; set; }
        public string Tipo_Doc_Desc { get; set; }
    }
}
