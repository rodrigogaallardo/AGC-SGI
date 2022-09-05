

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_TraerRubrosPorSolicitud_Result
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Superficie { get; set; }
        public int Tipo_Actividad { get; set; }
        public int Tipo_Doc_Id { get; set; }
        public string Tipo_Doc_Desc { get; set; }
        public bool EsAnterior { get; set; }
    }
}
