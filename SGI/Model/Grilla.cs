using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    //Agregados
    public partial class clsTiposdeDocumentosRequeridos
    {
        public int id_tdocreq { get; set; }
        public string nombre_tdocreq { get; set; }
        public string observaciones_tdocreq { get; set; }
        public string RequiereDetalle { get; set; }
        public string visible_en_SGI { get; set; }
        public string visible_en_SSIT { get; set; }
        public string visible_en_AT { get; set; }
        public string visible_en_Obs { get; set; }
        public string baja_tdocreq { get; set; }
        public string verificar_firma_digital { get; set; }
    }
    public partial class clsItemUbicacionRIT
    {
        public int id_dgubicacion { get; set; }
        public int anio_vigencia { get; set; }
        public string direccion { get; set; }
        public int cantidad { get; set; }
    }
    public partial class clsItemASRDirecciones
    {
        public int id_dgubicacion { get; set; }
        public string Dado_baja { get; set; }
        public string direccion { get; set; }
    }
    public partial class clsItemASRIndicadoresEmpresaxEstado
    {
        public int id_dgubicacion { get; set; }
        public string RazonSocial_empasc { get; set; }
        public string Email_empasc { get; set; }
        public int Anio { get; set; }
        public string Estado_Pago { get; set; }
        public string Estado_Aceptacion { get; set; }
        public int Cant_elevadores { get; set; }
        public string direccion { get; set; }
        public string Dado_baja { get; set; }
        public string Email_administrador { get; set; }
    }
    public partial class clsItemASRIndicadoresPagosxEmpresaxDia
    {
        public int id_dgubicacion { get; set; }
        public string RazonSocial_empasc { get; set; }
        public string Email_empasc { get; set; }
        public int Anio { get; set; }
        public int id_estado_aceptacion { get; set; }
        public string Estado_Pago { get; set; }
        public string Estado_Aceptacion { get; set; }
        public int Cant_dias_pendiente { get; set; }
        public int Cant_elevadores { get; set; }
        public string direccion { get; set; }
        public string Dado_baja { get; set; }
        public DateTime? Fecha_pago { get; set; }
        public string Email_administrador { get; set; }
    }
    public partial class clsItemASRIndicadoresGeneral
    {
        public int id_dgubicacion { get; set; }
        public int seccion { get; set; }
        public int manzana { get; set; }
        public string parcela { get; set; }
        public int NroRegistro_empasc { get; set; }
        public string RazonSocial_empasc { get; set; }
        public string Email_empasc { get; set; }
        public int Anio { get; set; }
        public DateTime Fecha_Estado_Aceptacion { get; set; }
        public string Estado_Pago { get; set; }
        public string Estado_Aceptacion { get; set; }
        public int Patente_Elevador { get; set; }
        public string direccion { get; set; }
        public string Dado_baja { get; set; }
        public string Nombre_Tipo_Elevador { get; set; }
        public string Email_administrador { get; set; }
    }
    public partial class clsItemASRIndicadoresInspecciones
    {
        public int id_dgubicacion { get; set; }
        public int seccion { get; set; }
        public int manzana { get; set; }
        public string parcela { get; set; }
        public string direccion { get; set; }
        public string Dado_baja { get; set; }
        public int id_empasc { get; set; }
        public string RazonSocial_empasc { get; set; }
        public int Anio { get; set; }
        public int id_elevador { get; set; }
        public int Patente_Elevador { get; set; }
        public string tecnico_ult_informe { get; set; }
        public DateTime fecha_ult_inspeccion { get; set; }
        public string hora_ult_inspeccion { get; set; }
        public string res_ult_informe { get; set; }
        public string obs_ult_informe { get; set; }
        public string Email_empasc { get; set; }
        public string Email_administrador { get; set; }
        public string Email_tecnico { get; set; }
    }
    public partial class clsItemFecha
    {
        public DateTime fecha_inspeccion { get; set; }
        public int id_elevador { get; set; }
    }
    public partial class clsInspeccion
    {
        public int id_inspeccion { get; set; }
        public string nom_inspeccion { get; set; }
        public DateTime fecha_inspeccion { get; set; }
        public string Create_User { get; set; }
        public int id_resinpeccion { get; set; }
    }
    public partial class clsItemASRInspecciones
    {
        public int id_elevador { get; set; }
        public string nombre_tecnico { get; set; }
        public string user_name { get; set; }
        public DateTime fecha_inspeccion { get; set; }
        public string nom_resinspeccion { get; set; }
        public string observacion_inspeccion { get; set; }
        public string resultado_inspeccion { get; set; }
        public string Email { get; set; }
    }
    public class clsItemGrillaProfesionales
    {
        public int id_rel_emp_usu { get; set; }
        public int id_profesional { get; set; }
        public string Matricula { get; set; }
        public string ApeNom { get; set; }
        public string Cuit { get; set; }
        public string Telefono { get; set; }
        public string ConsejoProfesional { get; set; }
        public string Estado { get; set; }
        public DateTime FechaEstado { get; set; }
    }
    public class clsItemPagos
    {
        public string apellidoyNombre { get; set; }
        public DateTime CreateDate { get; set; }
        public int id_pago_BU { get; set; }
        public int id_pago { get; set; }
        public long Numero_BU { get; set; }
        public int? nroDependencia_BU { get; set; }
        public string codigoBarras_BU { get; set; }
        public decimal monto_BU { get; set; }
        public DateTime? FechaPago_BU { get; set; }
        public string trazaPago_BU { get; set; }
        public int EstadoPago_BU { get; set; }
        public string estado_desc { get; set; }
        public string verificador_BU { get; set; }
        public string loginModif { get; set; }
        public string createUser { get; set; }
        public bool UpdateVisible { get; set; }
    }
    public partial class clsItemRIT
    {
        public int id_rit { get; set; }
        public string profesional { get; set; }
        public int anio_vigencia { get; set; }
        public int? nro_patente { get; set; }
        public int? nro_oblea { get; set; }
        public int? id_dgubicacion { get; set; }
        public int? id_tipo_instalacion { get; set; }
        public int? id_profesional { get; set; }
        public string direccion { get; set; }
        public string estado { get; set; }
        public string tipoInsta { get; set; }
    }

    public partial class RITExportarConsultaGeneral_Result
    {
        public int id_rit { get; set; }
        public string profesional { get; set; }
        public int anio_vigencia { get; set; }
        public int? nro_patente { get; set; }

        public int? id_dgubicacion { get; set; }


        public string direccion { get; set; }
        public string estado { get; set; }
        public string tipoInsta { get; set; }
        public string matricula { get; set; }
        public DateTime? fechaAcep { get; set; }
        public string administrador { get; set; }
        public string sub_tipoIntalacion { get; set; }
        public string ConsejoProfesional { get; set; }
        public string BUI { get; set; }
        public string EstadoPago { get; set; }
    }

    public partial class clsItemUbicacionIfci
    {
        public int id_dgubicacion { get; set; }
        public int anio_vigencia { get; set; }
        public string direccion { get; set; }
        public int cantidad { get; set; }
        public string confirmacion { get; set; }
        public string estado { get; set; }
    }


    public partial class clsItemUbicacionASR
    {
        public int id_dgubicacion { get; set; }
        public int anio_vigencia { get; set; }
        public string direccion { get; set; }
        public int cantidad { get; set; }

    }

    public partial class clsItemIFCI
    {
        public int id_ifci { get; set; }
        public string RazonSocial_empici { get; set; }
        public int? id_empici { get; set; }
        public int anio_vigencia { get; set; }
        public int? nro_patente { get; set; }
        public int nro_oblea { get; set; }
        public int id_dgubicacion { get; set; }
        public int id_tipo_instalacion { get; set; }
        public string tipo_instalacion { get; set; }
        public string confirmacion { get; set; }
        public int? id_estado { get; set; }
        public string estado { get; set; }
        public string direccion { get; set; }
    }

    public partial class clsItemASR
    {
        public int id_asc { get; set; }
        public string RazonSocial_empasc { get; set; }
        public int? id_empasc { get; set; }
        public int anio_vigencia { get; set; }
        public int? nro_patente { get; set; }
        public int nro_oblea { get; set; }
        public int id_dgubicacion { get; set; }
        public int id_tipo_elevador { get; set; }
        public string tipo_elevador { get; set; }
        public string nombre_usuario { get; set; }
        public string Cuit_empasc { get; set; }
        public int? NroRegistro_empasc { get; set; }
        public string direccion { get; set; }
        public DateTime? fecha_aceptacion { get; set; }
        public DateTime? fecha_visita { get; set; }
        public string estado_visita { get; set; }
        public string estado_aceptacion { get; set; }
    }

    public partial class clsItemInforme
    {
        public DateTime fecha_informe { get; set; }
        public DateTime fecha_carga_informe { get; set; }
        public string tipo_informe { get; set; }
        public string profesional { get; set; }
        public string matricula { get; set; }
        public string resultado_informe { get; set; }
        public string observacion { get; set; }
        public int id_inspeccion { get; set; }

    }
    public partial class clsIFCIRubros
    {
        public int id_ifci { get; set; }
        public string rubros { get; set; }
    }
    public partial class clsIFCIConsultaGeneral
    {
        public int id_ifci { get; set; }
        public int? seccion { get; set; }
        public string manzana { get; set; }
        public string parcela { get; set; }
        public string direccion { get; set; }
        public string dado_baja { get; set; }
        public int? patente_ifci { get; set; }
        public string nom_tipoinstalacion { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public int? nro_registro_empici { get; set; }
        public string razonsocial_empici { get; set; }
        public string email_empici { get; set; }
        public int? cant_pisos { get; set; }
        public int? cant_subsuelos { get; set; }
        public DateTime create_date { get; set; }
        public string nom_estado_aceptacion { get; set; }
        public string rubro { get; set; }
        public string confirmado { get; set; }
        public decimal? superficie { get; set; }
        public int id_gubicacion { get; set; }
        public int anio_vigencia { get; set; }

    }
    public partial class clsItemGrillaUbicacionesDireccionDGFYCO
    {
        public int id_dgubicacion { get; set; }
        public string direccion { get; set; }
        public string dado_baja { get; set; }
    }

    public partial class clsItemGrillaUbicacionesPuertaDGFYCO
    {
        public int id_dgubicacion { get; set; }
        public string calle { get; set; }
        public string puerta { get; set; }
    }

    public partial class clsItemBandejaEntrada
    {
        public string cod_grupotramite { get; set; }
        public int id_tipoTramite { get; set; }
        public string tipoTramite { get; set; }
        public int id_tramitetarea { get; set; }
        public DateTime FechaInicio_tramitetarea { get; set; }
        public Nullable<System.DateTime> FechaAsignacion_tramtietarea { get; set; }
        public int id_solicitud { get; set; }
        public string direccion { get; set; }
        public int id_tarea { get; set; }
        public string nombre_tarea { get; set; }
        public bool asignable_tarea { get; set; }
        public bool tomar_tarea { get; set; }
        public string formulario_tarea { get; set; }
        public int Dias_Transcurridos { get; set; }
        public int Dias_Acumulados { get; set; }
        public decimal superficie_total { get; set; }
        public string url_visorTramite { get; set; }
        public string url_tareaTramite { get; set; }
        public int cant_observaciones { get; set; }
        public int continuar_sade { get; set; }
        public int sade_completo { get; set; }
        public string zona_declarada { get; set; }
        public List<clsItemBandejaEntradaRubros> Rubros { get; set; }

        public string nombre_resultado { get; set; }
    }

    public partial class clsItemBandejaEntradaRubros
    {
        public string cod_rubro { get; set; }
        public string desc_rubro { get; set; }
    }


    public partial class clsItemBandejaEntradaAsignacion : clsItemBandejaEntrada
    {
        public int id_perfil_asignador { get; set; }
        public int id_perfil_asignado { get; set; }
    }
    public partial class clsItemBandejaEntradaAsignacionTarea
    {
        public int id_tarea { get; set; }
        public string nombre_tarea { get; set; }
    }

    public partial class clsItemReasignar : clsItemBandejaEntrada
    {
        public Guid UsuarioAsignado_tramitetarea { get; set; }

        public string UsuarioAsignado_tramitetarea_username { get; set; }
    }

    public partial class clsItemBuscarTramite
    {
        public int id_estado;
        public string cod_grupotramite { get; set; }
        public int? id_tramitetarea { get; set; }
        public DateTime? FechaInicio_tarea { get; set; }
        public Nullable<System.DateTime> FechaCierre_tarea { get; set; }
        public Nullable<System.DateTime> FechaAsignacion_tarea { get; set; }
        public int id_solicitud { get; set; }
        public string direccion { get; set; }
        public int? id_tarea { get; set; }
        public string desc_circuito { get; set; }
        public string nombre_tarea { get; set; }
        public bool asignable_tarea { get; set; }
        public string formulario_tarea { get; set; }
        public decimal superficie_total { get; set; }
        public string url_visorTramite { get; set; }
        public string url_tareaTramite { get; set; }
        public string nro_Expediente { get; set; }
        public string estado { get; set; }
        public Nullable<System.DateTime> LibradoUso { get; set; }
    }

    public partial class clsItemIndicadores
    {
        #region entity

        private DGHP_Entities db = null;

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }
        }
        #endregion
        public string cod_grupotramite { get; set; }
        public int id_tipotramite { get; set; }
        public int id_tramitetarea_pri { get; set; }
        public int id_tramitetarea_ult { get; set; }
        public DateTime FechaInicio { get; set; }
        public int id_tramitetarea_Rfd { get; set; }
        public Nullable<DateTime> FechaCierreRfd { get; set; }
        public int id_tramitetarea_Ge { get; set; }
        public Nullable<DateTime> FechaCierreGe { get; set; }
        public int id_solicitud { get; set; }
        public int id_circuito { get; set; }
        public string cod_circuito { get; set; }
        public string TareaOrigen { get; set; }
        public bool isTareaOrigenFechaInicio { get; set; }
        public Nullable<DateTime> FechaInicioOrigen { get; set; }
        public Nullable<DateTime> FechaFinOrigen { get; set; }
        public DateTime FechaOrigen
        {
            get
            {
                return isTareaOrigenFechaInicio ? FechaInicioOrigen.Value : FechaFinOrigen.Value;
            }
        }
        public string TareaFin { get; set; }
        public bool isTareaFinFechaInicio { get; set; }
        public Nullable<DateTime> FechaInicioFin { get; set; }
        public Nullable<DateTime> FechaFinFin { get; set; }
        public DateTime FechaFin
        {
            get
            {
                return isTareaFinFechaInicio ? FechaInicioFin.Value : FechaFinFin.Value;
            }
        }
        public bool isDiasHabiles { get; set; }
        public int dias_totales
        {
            get
            {
                TimeSpan diferencia = new TimeSpan(0, 0, 0);
                int dias = 0;
                if (FechaOrigen != null && FechaFin != null
                   /* && FechaInicioOrigen.Value.ToShortDateString() != FechaFinFin.Value.ToShortDateString()*/)
                {
                    if (isDiasHabiles)
                        return Functions.DiasHabiles(FechaOrigen, FechaFin);
                    //diferencia = FechaFin - FechaOrigen;
                    IniciarEntity();
                    var param = (from p in this.db.Parametros
                                 where p.id_param == 1
                                 select new
                                 {
                                     p.id_param,
                                     dias = SqlFunctions.DateDiff("dd", FechaOrigen, FechaFin)
                                 });
                    dias = (int)param.First().dias;
                    FinalizarEntity();
                }
                return dias;
            }
        }
        public int dias_dghyp
        {
            get
            {
                return dias_totales - dias_dictamen - dias_avh - dias_pago;
            }
        }
        public int dias_avh { get; set; }
        public int dias_dictamen { get; set; }
        public int dias_pago { get; set; }
        public int dias_contribuyente { get; set; }
        public int tiempo_muerto { get; set; }
        public int cant_obs { get; set; }
        public String observado
        {
            get
            {
                return cant_obs > 0 ? "SI" : "NO";
            }
        }

    }

    public partial class clsItemIndicadoresExcel
    {
        public int id_solicitud { get; set; }
        public string FechaInicio { get; set; }
        public string HoraInicio { get; set; }
        public string Fecha_Generacion_Expediente { get; set; }
        public string Hora_Generacion_Expediente { get; set; }
        public string Fecha_Firma_Dispo { get; set; }
        public string Hora_Firma_Dispo { get; set; }
        public string Fecha_Tarea_Origen { get; set; }
        public string Hora_Tarea_Origen { get; set; }
        public string Fecha_Tarea_Fin { get; set; }
        public string Hora_Tarea_Fin { get; set; }
        public int dias_totales { get; set; }
        public int dias_dghyp { get; set; }
        public int dias_avh { get; set; }
        public int dias_dictamen { get; set; }
        public int dias_contribuyente { get; set; }
        public int tiempo_muerto { get; set; }
        public String observado { get; set; }
        public int cant_obs { get; set; }

    }

    public partial class clsItemConsultaTramiteNuevoCur
    {
        public int Id_tad { get; set; }
        public int Id_solicitud { get; set; }
        public int id_estado { get; set; }
        public string Estado { get; set; }
        public string Nombre_RazonSocial { get; set; }
        public string Cuit { get; set; }
        public string Nombre_Profesional { get; set; }
        public string Matricula { get; set; }
        public int NroPartidaMatriz { get; set; }
        public int NroPartidaHorizontal { get; set; }
        public string Calle { get; set; }
        public int Altura_calle { get; set; }
        public string Piso { get; set; }
        public string UnidadFuncional { get; set; }
        public decimal Superficie { get; set; }
        public string Mixtura { get; set; }
        public string CodigoRubro { get; set; }
        public string DescripcionRubro { get; set; }
        public decimal SuperficieRubro { get; set; }
        public DateTime? Fecha_confirmacion { get; set; }
    }

    public partial class clsItemConsultaTramite
    {
        public string cod_grupotramite { get; set; }
        public int id_solicitud { get; set; }
        public int? id_solicitud_ref { get; set; }
        public int id_aux { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public int id_tipotramite { get; set; }
        public string TipoTramite { get; set; }
        public int id_tipoexpediente { get; set; }
        public string TipoExpediente { get; set; }
        public int id_subtipoexpediente { get; set; }
        public string SubTipoExpediente { get; set; }
        public string TipoCAA { get; set; }
        public string TareaActual { get; set; }
        public decimal Superficie { get; set; }
        public int id_estado { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaLibrado { get; set; }
        public DateTime? FechaHabilitacion { get; set; }
        public DateTime? FechaRechazo { get; set; }
        public string NumeroExp { get; set; }
        public DateTime? FechaCreacionTareaActual { get; set; }
        public DateTime? FechaAsignacionTareaActual { get; set; }
        public string GrupoCircuito { get; set; }
        public string Calificador { get; set; }
        public string ProfesionalAnexoTecnico { get; set; }
        public string ProfesionalAnexoNotarial { get; set; }
        public int? Observaciones { get; set; }
        public int? idEncomienda { get; set; }
        public int? idCAA { get; set; }
        public int? idSIPSA { get; set; }
        public string TipoNormativa { get; set; }
        public string Organismo { get; set; }
        public string NroNormativa { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public DateTime? FechaBaja { get; set; }
        public int? DiasEnCorreccion { get; set; }
        public string MailTitulares { get; set; }
        public string MailFirmantes { get; set; }
        public string MailUsuarioSSIT { get; set; }
        public string MailUsuarioTAD { get; set; }
        public List<clsItemConsulta> Titulares { get; set; }
        public List<clsItemConsulta> Cuits { get; set; }
        public List<clsItemddlRubro> Rubros { get; set; }
        public List<clsItemConsultaUbicacion> Ubicaciones { get; set; }
        public string PlantasHabilitar { get; set; }
        public string Usuario { get; set; }
        public string NombreyApellido { get; set; }
        public DateTime? FechaInicioAT { get; set; }
        public DateTime? FechaAprobadoAT { get; set; }
        public bool TienePlanoIncendio { get; set; }

    }

    public partial class clsItemConsultaSSIT
    {
        public int id_solicitud { get; set; }
        public DateTime FechaInicio { get; set; }
        public int id_tipotramite { get; set; }
        public string TipoTramite { get; set; }
        public int id_estado { get; set; }
        public string Estado { get; set; }
        public string NumeroExp { get; set; }
        public string Domicilio { get; set; }
        public string url_visorTramite { get; set; }
    }

    public partial class clsItemConsultaUbicacion
    {
        public string Zona { get; set; }
        public string Barrio { get; set; }
        public string UnidadFuncional { get; set; }
        public int? Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public int? PartidaMatriz { get; set; }
        public int? PartidaHorizontal { get; set; }
        public string SubTipoUbicacion { get; set; }
        public string LocalSubTipoUbicacion { get; set; }
        public List<clsItemConsultaPuerta> Calles { get; set; }
    }
    public partial class clsItemConsultaPuerta
    {
        public string calle { get; set; }
        public int puerta { get; set; }
    }
    public partial class clsItemConsultaRubro
    {
        public int id_rubro { get; set; }
        public string cod_rubro { get; set; }
        public string nom_rubro { get; set; }
        public string tipo_actividad { get; set; }
        public string id_subrubro { get; set; }
        public string nom_subrubro { get; set; }
    }
    public partial class clsItemDireccion
    {
        public int id_solicitud { get; set; }
        public string direccion { get; set; }

    }
    public partial class clsItemConsulta
    {
        public string value { get; set; }

    }
    public partial class clsItemPuerta
    {
        public int id_solicitud { get; set; }
        public string calle { get; set; }
        public string puerta { get; set; }
    }
    public partial class clsItemGrillaPagos
    {
        public int id_pago { get; set; }
        public int medio_pago { get; set; }
        public decimal monto_pago { get; set; }
        public DateTime fecha_factura { get; set; }
        public int id_estado_pago { get; set; }
        public string estado_pago { get; set; }
    }
    public partial class clsItemGrillaUbicacionesDireccion
    {
        public int id_dgubicacion { get; set; }
        public string direccion { get; set; }

    }
    public partial class clsItemGrillaEmpresaFecha
    {
        public int id_empascfec { get; set; }
        public int id_empasc { get; set; }
        public DateTime Desde_empascfec { get; set; }
        public DateTime Hasta_empascfec { get; set; }
        public string Descripcion_empascfec { get; set; }
        public bool TienePago { get; set; }
        public Nullable<int> IdUltimoPago { get; set; }
        public string UltimoEstadoPago { get; set; }
        public DateTime? eliminado { get; set; }
    }

    //public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    //{
    //    HashSet<TKey> seenKeys = new HashSet<TKey>(); foreach (TSource element in source)
    //    {
    //        if (seenKeys.Add(keySelector(element)))
    //        { yield return element; }
    //    }
    //}
    public partial class clsItemGrillaBuscarMails
    {
        public int id_solicitud { get; set; }
        public string Mail_ID { get; set; }
        public string Mail_Estado { get; set; }
        public string Mail_Proceso { get; set; }
        public string Mail_Asunto { get; set; }
        public string Mail_Email { get; set; }
        public DateTime? Mail_Fecha { get; set; }
        public string Mail_Html { get; set; }
        public DateTime? Mail_FechaAlta { get; set; }
        public DateTime? Mail_FechaEnvio { get; set; }
        public int? Mail_Intentos { get; set; }
        public int? Mail_Prioridad { get; set; }
        public DateTime? MailFechaNot_FechaSSIT { get; set; }
    }
    public partial class clsItemGrillaBuscarCalles
    {
        public int Calle_Id { get; set; }
        public int Calle_Cod { get; set; }
        public string Calle_Nombre { get; set; }
        public int Calle_AlturaIzquierdaInicio { get; set; }
        public int Calle_AlturaIzquierdaFin { get; set; }
        public int Calle_AlturaDerechaInicio { get; set; }
        public int Calle_AlturaDerechaFin { get; set; }
        public string Calle_Tipo { get; set; }
    }
    public partial class clsItemGrillaDatosServerMail
    {
        public string Mail_Email { get; set; }
        public string Mail_Nombre { get; set; }
        public string Mail_Smtp { get; set; }
        public int Mail_Puerto { get; set; }
        public string Mail_Usuario { get; set; }
        public string Mail_Contrasena { get; set; }

    }
    public partial class clsItemGrillaMailPrioridades
    {
        public int Prior_ID { get; set; }
        public TimeSpan Prior_Desde { get; set; }
        public TimeSpan Prior_Hasta { get; set; }
        public int Prior_Reenvio { get; set; }
        public string Prior_Observacion { get; set; }
    }

    public partial class clsItemddlRubro
    {
        public int id_rubro { get; set; }
        public string cod_rubro { get; set; }
        public string nom_rubro { get; set; }
        public string id_subrubro { get; set; }
        public string nom_subrubro { get; set; }

    }
    public partial class clsItemTxtSuperficie
    {
        public int id_circuito { get; set; }
        public int Superficie { get; set; }
        public string RevisaMenor { get; set; }
        public string RevisaMayor { get; set; }
    }
    public partial class clsItemTxtObservaciones
    {
        public int id_circuito { get; set; }
        public int Cantidad { get; set; }
    }
    public partial class clsItemddlTTramite
    {
        public int Id { get; set; }
        public string TipoTramite { get; set; }
    }
    public partial class clsItemGrillaParametrosRubro
    {
        public int id_param { get; set; }
        public string Descripcion { get; set; }
        public string cod_rubro { get; set; }
        public string Revisa { get; set; }
        public int Id { get; set; }
        public int id_rubro { get; set; }
    }
    public partial class clsItemGrillaUbicacionesClausuradas
    {
        public int id_ubicclausura { get; set; }
        public int id_ubicacion { get; set; }
        public string Tipo { get; set; }
        public int? NroPartidaMatriz { get; set; }
        public int? Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public string motivo { get; set; }
        public string domicilio { get; set; }
        public int id_dgubicacion { get; set; }
        public DateTime? fecha_alta_clausura { get; set; }
        public DateTime? fecha_baja_clausura { get; set; }
    }

    public partial class clsItemGrillaPersonasInhibidas
    {
        public int id_personainhibida { get; set; }
        public int? id_tipodoc_personal { get; set; }
        public string documento { get; set; }
        public int? nrodoc_personainhibida { get; set; }
        public int nroorden_personainhibida { get; set; }
        public string cuit_personainhibida { get; set; }
        public string nomape_personainhibida { get; set; }
        public DateTime? fecharegistro_personainhibida { get; set; }
        public DateTime? fechavencimiento_personainhibida { get; set; }
        public string autos_personainhibida { get; set; }
        public string juzgado_personainhibida { get; set; }
        public string secretaria_personainhibida { get; set; }
        public int estado { get; set; }
        public DateTime? fechabaja_personainhibida { get; set; }
        public int? operador_personainhibida { get; set; }
        public string observaciones_personainhibida { get; set; }
    }

    public partial class clsItemGrillaUbicacionesInhibidas
    {
        public int id_ubicinhibida { get; set; }
        public int id_ubicacion { get; set; }
        public string Tipo { get; set; }
        public int? NroPartidaMatriz { get; set; }
        public int? Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public string motivo { get; set; }
        public string domicilio { get; set; }
        public int id_dgubicacion { get; set; }
        public DateTime? fecha_inhibicion { get; set; }
        public DateTime? fecha_vencimiento { get; set; }
    }

    //Agregado grilla para perfiles asignados
    public partial class clsItemPerfilesFunciones
    {
        public int id_perfil { get; set; }
        public string nombre_perfil { get; set; }
        public string descripcion_perfil { get; set; }
        public string menues_perfil { get; set; }

        public string menues { get; set; }
        public int id_menu { get; set; }
        public int? id_menu_padre { get; set; }
        public int? id_menu_abuelo { get; set; }
        public string menu_abuelo { get; set; }
        public string menu_padre { get; set; }
        public string menu_hijo { get; set; }
    }
    //modelo SADE
    public partial class clsUsuariosSade
    {
        public string usuario_sade { get; set; }
        public string reparticion_sade { get; set; }
    }
    //BUI Recuperadas post-vencimiento
    public partial class clsBUIrecuperadas
    {
        public string Numero_BUI { get; set; }
        public DateTime Fecha_Recupero { get; set; }
        public DateTime? Fecha_Pago { get; set; }
        public string Sistema { get; set; }

        //Auxiliares
        public string EstadoPago_Anterior { get; set; }
        public string EstadoPago_Nuevo { get; set; }
    }
    //SSIT Solicitudes update usuario
    public partial class clsItemUpdateSolicitud
    {
        //Agregado Cambio del solicitante
        public int id_solicitud { get; set; }
        public int? IdEncomienda { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdTipoTramite { get; set; }
        public string TipoTramite { get; set; }
        public Guid Usuario_nuevo { get; set; }
        public Guid AplicacionID { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }

        //Agregado primera consulta
        public string Solicitante { get; set; }
        public Guid SolicitanteGuid { get; set; }
        public string SolicitanteUsername { get; set; }
        public string TitularNombre { get; set; }
        public string TitularDocumento { get; set; }
        public int TitularTipoDoc { get; set; }
        public string TitularEmail { get; set; }
    }
    public partial class clsItemSolicitudes
    {
        public int id_solicitud { get; set; }
        public string nombre_tarea { get; set; }
        public string nombre_resultado { get; set; }
        public DateTime? Fecha_Inicio { get; set; }
        public DateTime? Hora_Inicio { get; set; }
        public string Asignacion_Calificador { get; set; }
        public DateTime? Fecha_Inicio_Asignacion_Calificador { get; set; }
        public DateTime? Fecha_inicio_ULTIMA_Revision_HyP { get; set; }
        public DateTime? Fecha_Asignacion { get; set; }
        public DateTime? Hora_Asignacion { get; set; }
        public DateTime? Fecha_Cierre { get; set; }
        public DateTime? Hora_Cierre { get; set; }
        public int? Dif_ini_cierre { get; set; }
        public int? Dif_asig_cierre { get; set; }
        public string UserName { get; set; }
        public string superficie { get; set; }
        public int id_tarea { get; set; }
        public string numero_dispo_GEDO { get; set; }
        public int? id_paquete { get; set; }
        public string Observado { get; set; }

        //Para filtrar 
        public int id_tipotramite { get; set; }
        public int id_tipoexpediente { get; set; }
        public int id_subtipoexpediente { get; set; }
        public int id_circuito { get; set; }

        public string desc_tipotramite { get; set; }
        public string desc_tipoexpediente { get; set; }
        public string desc_subtipoexpediente { get; set; }
        public string desc_circuito { get; set; }
        public string cod_circuito { get; set; }
    }

    public partial class clsItemUsuarioAnexoTecnico
    {
        public int nro_anexo_tecnico { get; set; }
        public int nro_solicitud { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string user_name { get; set; }
        public string consejo_profesional { get; set; }
    }

    public partial class clsBajas
    {
        public int id_baja { get; set; }
        public int id_solicitud { get; set; }
        public int id_motivo { get; set; }
        public string motivo { get; set; }
        public DateTime fecha { get; set; }
        public string observaciones { get; set; }
        public string usuario { get; set; }
        public string url { get; set; }
        public int tipo { get; set; }
    }

    public partial class clsItemHoja1
    {
        public int id_circuito_actual { get; set; }
        public int id_solicitud { get; set; }
        public string observaciones { get; set; }
        public string observador { get; set; }
        public string cod_circuito_origen { get; set; }
        public string CreateDate { get; set; }
    }
    public partial class clsItemHoja2
    {
        public int id_circuito_actual { get; set; }
        public int id_solicitud { get; set; }
        public string Observaciones_contribuyente { get; set; }
        public string FechaInicio_tramitetarea { get; set; }
        public string FechaAsignacion_tramitetarea { get; set; }
        public string FechaCierre_tramitetarea { get; set; }
        public string observador { get; set; }
        public string CreateDate { get; set; }
        public string Documento_Requerido { get; set; }
        public string Documento_Requerido_Observacion { get; set; }

    }
    public partial class clsItemHoja3
    {
        public int id_circuito_actual { get; set; }
        public int id_solicitud { get; set; }
        public string nombre_tarea { get; set; }
        public string Circuito_Tarea { get; set; }
        public string nombre_resultado { get; set; }
        public string fecha_inicio { get; set; }
        public TimeSpan? hora_inicio { get; set; }
        public string fecha_asignacion { get; set; }
        public TimeSpan? hora_asignacion { get; set; }
        public string fecha_cierre { get; set; }
        public TimeSpan? hora_cierre { get; set; }
        public int? Dif_ini_cierre { get; set; }
        public int? Dif_asig_cierre { get; set; }
        public string username { get; set; }
        public decimal? superficie { get; set; }
        public string NroDisposicionSADE { get; set; }
        public string nombre_proxima_tarea { get; set; }
        public string Fecha { get; set; }
        public string Circuito_Origen { get; set; }
        public string CreateDate { get; set; }
    }

    public partial class clsItemHoja6
    {
        public int id_circuito_actual { get; set; }
        public int id_solicitud { get; set; }
        public string asigCalif_ini { get; set; }
        public string rhyp_ini { get; set; }
        public string rfd_cierre { get; set; }
        public string es_caducidad { get; set; }
        public string dict_ini { get; set; }
        public string dict_cierre { get; set; }
        public string avh_ini { get; set; }
        public string avh_cierre { get; set; }
        public string Observado { get; set; }
        public int? Cantidad_Observado { get; set; }
        public int? Dif_EE_asig_cierre { get; set; }
        public int? Dif_RFD_asig_cierre { get; set; }
        public string Circuito_Origen { get; set; }
        public string CreateDate { get; set; }
        public int? Dias_Totales_Dictamen { get; set; }
    }

    public partial class clsItemHoja7
    {
        public int id_solicitud { get; set; }
        public string cod_circuito { get; set; }
        public string Fecha_Inicio_Control_Informe { get; set; }
        public TimeSpan? Hora_Inicio_Control_Informe { get; set; }
        public string Fecha_Inicio_GenerarExpediente { get; set; }
        public TimeSpan? Hora_Inicio_GenerarExpediente { get; set; }
        public string Fecha_Fin_Generar_Expediente { get; set; }
        public TimeSpan? Hora_Fin_Generar_Expediente { get; set; }
        public bool Observado_alguna_vez { get; set; }
    }

    public partial class clsItemHoja7_2
    {
        public int id_solicitud { get; set; }
        public string cod_circuito { get; set; }
        public string Fecha_Inicio_GenerarExpediente { get; set; }
        //public TimeSpan? Hora_Inicio_GenerarExpediente { get; set; }
        public string Fecha_Fin_Generar_Expediente { get; set; }
        //public TimeSpan? Hora_Fin_Generar_Expediente { get; set; }
        public string Fecha_Fin_Revision_Gerente { get; set; }
        //public TimeSpan? Hora_Fin_Revision_Gerente { get; set; }
        public bool Observado_alguna_vez { get; set; }
    }

    public partial class clsItemHoja8
    {
        public int id_solicitud { get; set; }
        public string cod_circuito { get; set; }
        public string Fecha_Inicio_Asignacion_Calificador { get; set; }
        public string Fecha_cierre_Revision_gerente_2 { get; set; }
        public string Fecha_Cierre_Revision_firma_dispo { get; set; }
        public string Fecha_Inicio_Dictamenes { get; set; }
        public string Fecha_Cierre_Dictamenes { get; set; }
        public string Fecha_Inicio_Revision_Pagos { get; set; }
        public string Fecha_Fin_Revision_Pagos { get; set; }
        public string Observado { get; set; }
        public int? Cantidad_Veces_Observado { get; set; }
        public string Circuito_Origen { get; set; }
        public int? Dias_Totales_Dictamen { get; set; }
    }
    public partial class clsItemHoja8_2
    {
        public int id_solicitud { get; set; }
        public string cod_circuito { get; set; }        
        public string Fecha_Inicio_Asignacion_Calificador { get; set; }
        public string Fecha_Cierre_Revision_Subgerente { get; set; }
        public string Fecha_Cierre_Revision_Gerente { get; set; }
        public string Fecha_Inicio_Revision_DGHP { get; set; }
        public string Fecha_Cierre_Revision_firma_dispo { get; set; }
        public string Fecha_Inicio_Dictamenes { get; set; }
        public string Fecha_Cierre_Dictamenes { get; set; }
        public string Fecha_inicio_Consulta_Adicional { get; set; }
        public string Fecha_Cierre_Consulta_Adicional { get; set; }
        public string Fecha_inicio_AVH { get; set; }
        public string Fecha_cierre_AVH { get; set; }
        public string Observado { get; set; }
        public int? Cantidad_Veces_Observado { get; set; }
        public string Circuito_Origen { get; set; }
        public int? Dias_Totales_Dictamen { get; set; }
        public int? Dias_Totales_Consulta { get; set; }
    }

    public partial class clsItemHoja9
    {
        public int id_circuito_actual { get; set; }
        public string cod_circuito { get; set; }
        public int id_solicitud { get; set; }
        public string FechaInicio_Asig_Calificador { get; set; }
        public string Fecha_Inicio_Notif_Caducidad { get; set; }
        public string Fecha_Inicio_Rev_DGHP_Caducidad { get; set; }
        public string Fecha_Fin_Rev_DGHP_Caducidad { get; set; }
        public string Fecha_Fin_Gen_Expediente { get; set; }
        public string Fecha_Fin_Rev_Firma { get; set; }
    }

    public partial class clsItemRubroCUR
    {
        public int Id_rubro { get; set; }
        public string Cod_rubro { get; set; }
        public string Desc_rubro { get; set; }
        public string Cir_rubro { get; set; }
    }

    public partial class clsItemHistorialRubroCur
    {
        public int IdRubro { get; set; }
        public int IdRubrosCN_historial { get; set; }
        public string TipoOperacion { get; set; }
        public string FechaOperacion { get; set; }
        public string Rubro { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Keywords { get; set; }
        public string VigenciaDesde_rubro { get; set; }
        public string VigenciaHasta_rubro { get; set; }
        public string TipoActividad { get; set; }
        public string TipoExpediente { get; set; }
        public string GrupoCircuito { get; set; }
        public bool LibrarUso { get; set; }
        public string ZonaMixtura1 { get; set; }
        public string ZonaMixtura2 { get; set; }
        public string ZonaMixtura3 { get; set; }
        public string ZonaMixtura4 { get; set; }
        public string Estacionamiento { get; set; }
        public string Bicicleta { get; set; }
        public string CyD { get; set; }
        public string Observaciones { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string LastUpdateDate { get; set; }
        public string LastUpdateUser { get; set; }
        public bool? Asistentes350 { get; set; }
        public bool? SinBanioPCD { get; set; }
    }


}