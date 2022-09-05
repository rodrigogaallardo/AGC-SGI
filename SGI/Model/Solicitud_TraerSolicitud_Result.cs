

namespace SGI.Model
{
    using System;
    
    public partial class Solicitud_TraerSolicitud_Result
    {
        public string cod_tipotramite { get; set; }
        public string descripcion_tipotramite { get; set; }
        public string cod_tipotramite_ws { get; set; }
        public string cod_tipoexpediente { get; set; }
        public string descripcion_tipoexpediente { get; set; }
        public string cod_tipoexpediente_ws { get; set; }
        public string cod_subtipoexpediente { get; set; }
        public string descripcion_subtipoexpediente { get; set; }
        public string cod_subtipoexpediente_ws { get; set; }
        public int NroSolicitud { get; set; }
        public System.DateTime FechaSolicitud { get; set; }
        public System.Guid Usuario { get; set; }
        public Nullable<int> IdTipoActividad { get; set; }
        public Nullable<int> IdTipoSociedad { get; set; }
        public string ZonaDeclarada { get; set; }
        public Nullable<decimal> Superficie { get; set; }
        public Nullable<int> CantidadOperarios { get; set; }
        public Nullable<int> IdTipoNormativa { get; set; }
        public string NroNormativa { get; set; }
        public Nullable<int> IdEntidadNormativa { get; set; }
        public Nullable<int> AnioNormativa { get; set; }
        public Nullable<int> IdProfesional { get; set; }
        public Nullable<int> MatriculaEscribano { get; set; }
        public Nullable<System.DateTime> FechaIngreso { get; set; }
        public Nullable<System.Guid> UsuarioIngreso { get; set; }
        public string NroCarpeta { get; set; }
        public string NroExpediente { get; set; }
        public int IdEstado { get; set; }
        public string OtraPuerta { get; set; }
        public bool PresentaPlanos { get; set; }
        public Nullable<System.Guid> UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<System.Guid> UsuarioCambioEstado { get; set; }
        public Nullable<System.DateTime> FechaCambioEstado { get; set; }
        public int id_tipotramite { get; set; }
        public int id_tipoexpediente { get; set; }
        public int id_subtipoexpediente { get; set; }
        public Nullable<bool> Valid_deuda_agip { get; set; }
        public Nullable<bool> colegio_escribano { get; set; }
        public string TipoActividad { get; set; }
        public string TipoSociedad { get; set; }
        public string Estado { get; set; }
    }
}
