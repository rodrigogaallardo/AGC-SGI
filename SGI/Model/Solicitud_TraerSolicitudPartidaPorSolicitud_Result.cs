

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_TraerSolicitudPartidaPorSolicitud_Result
    {
        public int Id { get; set; }
        public int NroSolicitud { get; set; }
        public Nullable<int> id_partidahorizontal { get; set; }
        public Nullable<int> NroPartidaHorizontal { get; set; }
        public Nullable<int> id_partidamatriz { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public Nullable<int> IdTipoSector { get; set; }
        public string NombreTipoSector { get; set; }
        public Nullable<int> seccion { get; set; }
        public string manzana { get; set; }
        public string parcela { get; set; }
        public string LEY_449 { get; set; }
        public string LEY_449_2 { get; set; }
        public string LEY_449_3 { get; set; }
        public string DESLINDE { get; set; }
        public string DESLINDE_2 { get; set; }
        public string DESLINDE_3 { get; set; }
        public Nullable<int> CALLE1_F { get; set; }
        public string CALLENOM1 { get; set; }
        public Nullable<int> PUERTA1_1 { get; set; }
        public Nullable<int> PUERTA2_1 { get; set; }
        public Nullable<int> CALLE2_F { get; set; }
        public string CALLENOM2 { get; set; }
        public Nullable<int> PUERTA1_2 { get; set; }
        public Nullable<int> PUERTA2_2 { get; set; }
        public Nullable<int> CALLE3_F { get; set; }
        public string CALLENOM3 { get; set; }
        public Nullable<int> PUERTA1_3 { get; set; }
        public Nullable<int> PUERTA2_3 { get; set; }
        public Nullable<int> CALLE4_F { get; set; }
        public string CALLENOM4 { get; set; }
        public Nullable<int> PUERTA1_4 { get; set; }
        public Nullable<int> PUERTA2_4 { get; set; }
    }
}
