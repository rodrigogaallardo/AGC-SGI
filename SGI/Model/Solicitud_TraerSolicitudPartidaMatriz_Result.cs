

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_TraerSolicitudPartidaMatriz_Result
    {
        public int Id { get; set; }
        public int NroSolicitud { get; set; }
        public Nullable<int> IdTipoSector { get; set; }
        public Nullable<int> id_partidamatriz { get; set; }
        public Nullable<int> id_partidahorizontal { get; set; }
        public int id_subtipoubicacion { get; set; }
        public string NombreTipoSector { get; set; }
    }
}
