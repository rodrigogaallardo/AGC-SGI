

namespace SGI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SGI_ConsultaTramites
    {
        public int Id { get; set; }
        public int id_solicitud { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public Nullable<System.DateTime> FechaIngreso { get; set; }
        public int id_tipotramite { get; set; }
        public string TipoTramite { get; set; }
        public int id_tipoexpediente { get; set; }
        public string TipoExpediente { get; set; }
        public int id_subtipoexpediente { get; set; }
        public string SubTipoExpediente { get; set; }
        public Nullable<int> id_grupo_circuito { get; set; }
        public string TipoCAA { get; set; }
        public Nullable<int> id_tarea_actual { get; set; }
        public string TareaActual { get; set; }
        public Nullable<System.DateTime> FechaCreacionTareaActual { get; set; }
        public Nullable<System.DateTime> FechaAsignacionTareaActual { get; set; }
        public Nullable<int> id_tramitetar_abierta { get; set; }
        public Nullable<decimal> Superficie { get; set; }
        public int id_estado { get; set; }
        public string Estado { get; set; }
        public string Calificador { get; set; }
        public string ProfesionalAnexoTecnico { get; set; }
        public string ProfesionalAnexoNotarial { get; set; }
        public Nullable<System.DateTime> FechaLibrado { get; set; }
        public Nullable<System.DateTime> FechaHabilitacion { get; set; }
        public Nullable<System.DateTime> FechaRechazo { get; set; }
        public Nullable<System.DateTime> FechaBaja { get; set; }
        public Nullable<System.DateTime> FechaCaducidad { get; set; }
        public string Titulares { get; set; }
        public string Cuits { get; set; }
        public string NumeroExp { get; set; }
        public Nullable<int> Observaciones { get; set; }
        public Nullable<int> id_encomienda { get; set; }
        public Nullable<int> idCAA { get; set; }
        public Nullable<int> idSIPSA { get; set; }
        public string TipoNormativa { get; set; }
        public string Organismo { get; set; }
        public string NroNormativa { get; set; }
        public Nullable<int> DiasEnCorreccion { get; set; }
        public string MailFirmantes { get; set; }
        public string MailTitulares { get; set; }
        public string MailUsuarioSSIT { get; set; }
        public string MailUsuarioTAD { get; set; }
        public string Zona { get; set; }
        public Nullable<int> id_zonahabilitaciones { get; set; }
        public Nullable<int> id_barrio { get; set; }
        public string Barrio { get; set; }
        public Nullable<int> id_comuna { get; set; }
        public string UnidadFuncional { get; set; }
        public Nullable<int> Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public Nullable<int> NroPartidaMatriz { get; set; }
        public Nullable<int> NroPartidaHorizontal { get; set; }
        public string SubTipoUbicacion { get; set; }
        public string LocalSubTipoUbicacion { get; set; }
        public Nullable<int> codigo_calle { get; set; }
        public string nombre_calle { get; set; }
        public Nullable<int> NroPuerta { get; set; }
        public Nullable<int> id_rubro { get; set; }
        public string cod_rubro { get; set; }
        public string nom_rubro { get; set; }
        public Nullable<int> id_subrubro { get; set; }
        public string nom_subrubro { get; set; }
        public string PlantasHabilitar { get; set; }
        public string Usuario { get; set; }
        public string NombreyApellido { get; set; }
        public Nullable<System.DateTime> FechaInicioAT { get; set; }
        public Nullable<System.DateTime> FechaAprobadoAT { get; set; }
        public bool TienePlanoIncendio { get; set; }
    }
}
