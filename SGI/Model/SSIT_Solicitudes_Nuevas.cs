

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SSIT_Solicitudes_Nuevas
    {
        public SSIT_Solicitudes_Nuevas()
        {
            this.Rel_Rubros_Solicitudes_Nuevas = new HashSet<Rel_Rubros_Solicitudes_Nuevas>();
        }
    
        public int id_solicitud { get; set; }
        public int id_estado { get; set; }
        public Nullable<int> id_Tad { get; set; }
        public int id_tipotramite { get; set; }
        public string Nombre_RazonSocial { get; set; }
        public string Cuit { get; set; }
        public string Nombre_Profesional { get; set; }
        public string Matricula { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public Nullable<int> NroPartidaHorizontal { get; set; }
        public string Nombre_calle { get; set; }
        public Nullable<int> Altura_calle { get; set; }
        public string Piso { get; set; }
        public string UnidadFuncional { get; set; }
        public string Descripcion_Actividad { get; set; }
        public Nullable<decimal> Superficie { get; set; }
        public string CodZonaHab { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public Nullable<System.Guid> LastUpdateUser { get; set; }
        public string CodigoSeguridad { get; set; }
    
        public virtual TipoEstadoSolicitud TipoEstadoSolicitud { get; set; }
        public virtual TipoTramite TipoTramite { get; set; }
        public virtual ICollection<Rel_Rubros_Solicitudes_Nuevas> Rel_Rubros_Solicitudes_Nuevas { get; set; }
    }
}
