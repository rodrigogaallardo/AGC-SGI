using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    public class TareaResultadoDTO2
    {
        public Int32? Id_solicitud { get; set; }
        public String Nombre_tarea { get; set; }
        public String Nombre_resultado { get; set; }
        public DateTime? Fecha_Inicio { get; set; }
        public TimeSpan? Hora_Inicio { get; set; }
        public DateTime? Fecha_Asignacion { get; set; }
        public TimeSpan? Hora_Asignacion { get; set; }
        public DateTime? Fecha_Cierre { get; set; }
        public TimeSpan? Hora_Cierre { get; set; }
        public int? Dif_ini_cierre { get; set; }
        public int? Dif_asig_cierre { get; set; }
        public String UserName { get; set; }
        public Decimal? superficie { get; set; }
        public string NroExpedienteSade { get; set; }
        public string CircuitoOrigen { get; set; }
    }

    public class TareaResultadoDTO
    {
        public Int32? Id_solicitud { get; set; }
        public String Nombre_tarea { get; set; }
        public String Nombre_resultado { get; set; }
        public DateTime? Fecha_Inicio { get; set; }
        public TimeSpan? Hora_Inicio { get; set; }
        public DateTime? Fecha_Asignacion { get; set; }
        public TimeSpan? Hora_Asignacion { get; set; }
        public DateTime? Fecha_Cierre { get; set; }
        public TimeSpan? Hora_Cierre { get; set; }
        public int? Dif_ini_cierre { get; set; }
        public int? Dif_asig_cierre { get; set; }
        public String UserName { get; set; }
        public Decimal? superficie { get; set; }
        public string numero_dispo_GEDO { get; set; }
    }
    public class TramiteDTO
    {
        public int? IdSolicitud { get; set; }
        public string Observacion { get; set; }
        public string CircuitoOrigen { get; set; }
        public string Quien { get; set; }
        public DateTime? Fecha { get; set; }
    }
    public class TramiteSSPDTO
    {
        public int? IdSolicitud { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaInicio_tramitetarea { get; set; }
        public DateTime? FechaAsignacion_tramtietarea { get; set; }
        public DateTime? FechaCierre_tramitetarea { get; set; }
        public string Quien { get; set; }
    }
    public class TramiteSSP2DTO
    {
        public int? IdSolicitud { get; set; }
        public int? id_ObsGrupo { get; set; }
        public string Observacion_ObsDocs { get; set; }
        public string Respaldo_ObsDocs { get; set; }
        public DateTime FechaInicio_tramitetarea { get; set; }
        public DateTime? FechaAsignacion_tramtietarea { get; set; }
        public DateTime? FechaCierre_tramitetarea { get; set; }
        public string Quien { get; set; }
        public string CircuitoOrigen { get; set; }
    }

    public class TransferenciaTareaResultadoDTO
    {
        public int IdSolicitud { get; set; }
        public string NombreTarea { get; set; }
        public string NombreResultado { get; set; }
        public DateTime? FechaInicio { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public TimeSpan? HoraAsignacion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public TimeSpan? HoraCierre { get; set; }
        public int? Dif_ini_cierre { get; set; }
        public int? Dif_asig_cierre { get; set; }
        public string UserName { get; set; }
        public decimal? Superficie { get; set; }
    }
    public class ListadoVida
    {
        public int Id_cpadron { get; set; }
        public decimal? superficie { get; set; }
        public DateTime? Fecha_inicio_Solicitud_CP { get; set; }
        public TimeSpan? Hora_inicio_Solicitud_CP { get; set; }
        public DateTime? Fecha_asig_Solicitud_CP { get; set; }
        public TimeSpan? Hora_asig_Solicitud_CP { get; set; }
        public DateTime? Fecha_fin_Solicitud_CP { get; set; }
        public TimeSpan? Hora_fin_Solicitud_CP { get; set; }
        public DateTime? Fecha_inicio_Control_Informes { get; set; }
        public TimeSpan? Hora_inicio_Control_Informes { get; set; }
        public DateTime? Fecha_asig_Control_Informes { get; set; }
        public TimeSpan? Hora_asig_Control_Informes { get; set; }
        public DateTime? Fecha_fin_Control_Informes { get; set; }
        public TimeSpan? Hora_fin_Control_Informes { get; set; }
        public DateTime? Fecha_inicio_Gen_Exp { get; set; }
        public TimeSpan? Hora_inicio_Gen_Exp { get; set; }
        public DateTime? Fecha_asig_Gen_Exp { get; set; }
        public TimeSpan? Hora_asig_Gen_Exp { get; set; }
        public DateTime? Fecha_fin_Gen_Exp { get; set; }
        public TimeSpan? Hora_fin_Gen_Exp { get; set; }
        public DateTime? Fecha_inicio_rev_sgo { get; set; }
        public TimeSpan? Hora_inicio_rev_sgo { get; set; }
        public DateTime? Fecha_asig_rev_sgo { get; set; }
        public TimeSpan? Hora_asig_rev_sgo { get; set; }
        public DateTime? Fecha_fin_rev_sgo { get; set; }
        public TimeSpan? Hora_fin_rev_sgo { get; set; }
        public DateTime? Fecha_inicio_fin_tra { get; set; }
        public TimeSpan? Hora_inicio_fin_tra { get; set; }
        public DateTime? Fecha_asig_fin_tra { get; set; }
        public TimeSpan? Hora_asig_fin_tra { get; set; }
        public DateTime? Fecha_fin_fin_tra { get; set; }
        public TimeSpan? Hora_fin_fin_tra { get; set; }
        public DateTime? observado_alguna_vez { get; set; }
    }

    public class ListadoVidaHabilitacion2
    {
        public int? Id_solicitud { get; set; }
        public decimal? superficie { get; set; }
        public DateTime? Fecha_inicio_ASIGNACION_calif { get; set; }
        public TimeSpan? Hora_inicio_ASIGNACION_calif { get; set; }
        public DateTime? Fecha_fin_calif { get; set; }
        public TimeSpan? Hora_fin_calif { get; set; }
        public DateTime? Fecha_Inicio_Exp { get; set; }
        public TimeSpan? Hora_Inicio_Exp { get; set; }
        public DateTime? Fecha_Asignacion_Exp { get; set; }
        public TimeSpan? Hora_Asignacion_Exp { get; set; }
        public DateTime? Fecha_Cierre_Exp { get; set; }
        public TimeSpan? Hora_Cierre_Exp { get; set; }
        public DateTime? Fecha_Inicio_Rev_Ger { get; set; }
        public TimeSpan? Hora_Inicio_Rev_Ger { get; set; }
        public DateTime? Fecha_Cierre_Rev_Ger { get; set; }
        public TimeSpan? Hora_Cierre_Rev_Ger { get; set; }
        public DateTime? Fecha_Gen_BU_ini { get; set; }
        public DateTime? Fecha_Gen_BU_cierre { get; set; }
        public DateTime? Fecha_Rev_BU_ini { get; set; }
        public DateTime? Fecha_Rev_BU_cierre { get; set; }
        public string observado_alguna_vez { get; set; }
        public DateTime? Fecha_Cierre_Revision { get; set; }
        public DateTime? Fecha_Asign_Revision { get; set; }
        public DateTime? Fecha_Ini_Revision { get; set; }
        public DateTime? Fecha_Cierre_Entrega { get; set; }
        public DateTime? Fecha_Ini_Entrega { get; set; }
        public DateTime? FirmaRDGHP_Inicio { get; set; }
        public DateTime? FirmaRDGHP_Cierre { get; set; }
        public string NroExpedienteSade { get; set; }
        public string CircuitoOrigen { get; set; }
    }
    public class ListadoVidaTransferencia
    {
        public int? Id_solicitud { get; set; }
        public decimal? superficie { get; set; }
        public DateTime? Fecha_inicio_asig_calif { get; set; }
        public TimeSpan? Hora_inicio_asig_calif { get; set ; }
        public DateTime? Fecha_asig_asig_calif { get; set; }
        public TimeSpan? Hora_asig_asig_calif { get; set; }
        public DateTime? Fecha_fin_asig_calif { get; set; }
        public TimeSpan? Hora_fin_asig_calif { get; set; }
        public DateTime? Fecha_inicio_calif { get; set; }
        public TimeSpan? Hora_inicio_calif { get; set; }
        public DateTime? Fecha_asig_calif { get; set; }
        public TimeSpan? Hora_asig_calif { get; set; }
        public DateTime? Fecha_fin_calif { get; set; }
        public DateTime? Column1 { get; set; }
        public DateTime? Fecha_inicio_rev_go_1 { get; set; }
        public TimeSpan? Hora_inicio_rev_go_1 { get; set; }
        public DateTime? Fecha_asig_rev_go_1 { get; set; }
        public TimeSpan? Hora_asig_rev_go_1 { get; set; }
        public DateTime? Fecha_fin_rev_go_1f { get; set; }
        public TimeSpan? Hora_fin_rev_go_1 { get; set; }
        public DateTime? Fecha_inicio_rev_go_2 { get; set; }
        public TimeSpan? Hora_inicio_rev_go_2 { get; set; }
        public DateTime? Fecha_asig_rev_go_2 { get; set; }
        public TimeSpan? Hora_asig_rev_go_2 { get; set; }
        public DateTime? Fecha_fin_rev_go_2f { get; set; }
        public TimeSpan? Hora_fin_rev_go_2 { get; set; }
        public DateTime? Fecha_inicio_dict_asig_prof { get; set; }
        public TimeSpan? Hora_inicio_dict_asig_prof { get; set; }
        public DateTime? Fecha_asig_dict_asig_prof { get; set; }
        public TimeSpan? Hora_asig_dict_asig_prof { get; set; }
        public DateTime? Fecha_fin_dict_asig_proff { get; set; }
        public TimeSpan? Hora_fin_dict_asig_prof { get; set; }
        public DateTime? Fecha_inicio_genexp { get; set; }
        public TimeSpan? Hora_inicio_genexp { get; set; }
        public DateTime? Fecha_asig_genexp { get; set; }
        public TimeSpan? Hora_asig_genexp { get; set; }
        public DateTime? Fecha_fin_genexpf { get; set; }
        public TimeSpan? Hora_fin_genexp { get; set; }
        public DateTime? Fecha_inicio_rev_dghp { get; set; }
        public TimeSpan? Hora_inicio_rev_dghp { get; set; }
        public DateTime? Fecha_asig_rev_dghp { get; set; }
        public TimeSpan? Hora_asig_rev_dghp { get; set; }
        public DateTime? Fecha_fin_rev_dghpf { get; set; }
        public TimeSpan? Hora_fin_rev_dghp { get; set; }
        public DateTime? Fecha_inicio_rev_fir_dispo { get; set; }
        public TimeSpan? Hora_inicio_rev_fir_dispo { get; set; }
        public DateTime? Fecha_asig_rev_fir_dispo { get; set; }
        public TimeSpan? Hora_asig_rev_fir_dispo { get; set; }
        public DateTime? Fecha_fin_rev_fir_dispof { get; set; }
        public TimeSpan? Hora_fin_rev_fir_dispo { get; set; }
        public DateTime? Fecha_inicio_ent_tra { get; set; }
        public TimeSpan? Hora_inicio_ent_tra { get; set; }
        public DateTime? Fecha_asig_ent_tra { get; set; }
        public TimeSpan? Hora_asig_ent_tra { get; set; }
        public DateTime? Fecha_fin_ent_traf { get; set; }
        public TimeSpan? Hora_fin_ent_tra { get; set; }
        public DateTime? Fecha_inicio_fin_tra { get; set; }
        public TimeSpan? Hora_inicio_fin_tra { get; set; }
        public DateTime? Fecha_asig_fin_tra { get; set; }
        public TimeSpan? Hora_asig_fin_tra { get; set; }
        public DateTime? Fecha_fin_fin_traf { get; set; }
        public TimeSpan? Hora_fin_fin_tra { get; set; }
        public DateTime? Fecha_inicio_aprobados { get; set; }
        public TimeSpan? Hora_inicio_aprobados { get; set; }
        public DateTime? Fecha_asig_aprobados { get; set; }
        public TimeSpan? Hora_asig_aprobados { get; set; }
        public DateTime? Fecha_fin_aprobadosf { get; set; }
        public TimeSpan? Hora_fin_aprobados { get; set; }
        public DateTime? Fecha_inicio_dict_rev_tram { get; set; }
        public TimeSpan? Hora_inicio_dict_rev_tram { get; set; }
        public DateTime? Fecha_asig_dict_rev_tram { get; set; }
        public TimeSpan? Hora_asig_dict_rev_tram { get; set; }
        public DateTime? Fecha_fin_dict_rev_tramf { get; set; }
        public TimeSpan? Hora_fin_dict_rev_tram { get; set; }
        public DateTime? Fecha_inicio_dict_rev_sgo { get; set; }
        public TimeSpan? Hora_inicio_dict_rev_sgo { get; set; }
        public DateTime? Fecha_asig_dict_rev_sgo { get; set; }
        public TimeSpan? Hora_asig_dict_rev_sgo { get; set; }
        public DateTime? Fecha_fin_dict_rev_sgof { get; set; }
        public TimeSpan? Hora_fin_dict_rev_sgo { get; set; }
        public DateTime? Fecha_inicio_dict_rev_go { get; set; }
        public TimeSpan? Hora_inicio_dict_rev_go { get; set; }
        public DateTime? Fecha_asig_dict_rev_go { get; set; }
        public TimeSpan? Hora_asig_dict_rev_go { get; set; }
        public DateTime? Fecha_fin_dict_rev_gof { get; set; }
        public TimeSpan? Hora_fin_dict_rev_go { get; set; }
        public DateTime? Fecha_inicio_dict_gedo { get; set; }
        public TimeSpan? Hora_inicio_dict_gedo { get; set; }
        public DateTime? Fecha_asig_dict_gedo { get; set; }
        public TimeSpan? Hora_asig_dict_gedo { get; set; }
        public DateTime? Fecha_fin_dict_gedof { get; set; }
        public TimeSpan? Hora_fin_dict_gedo { get; set; }
        public DateTime? observado_alguna_vez { get; set; }
    }
    public class UltimaRevision
    {
        public Int32? solicitud { get ; set ; }
        public DateTime? Fecha_Inicio_Tarea { get ; set ; }
        public String Asignacion_Calificador { get ; set ; }
        public DateTime? Fecha_inicio_ULTIMA_Revision_HyP { get ; set ; }
        public String Observado { get ; set ; }
    }
    public class UltimaRevisionBis
    {
        public Int32? solicitud { get; set; }
        public DateTime? Fecha_Inicio_Asignacion_Calificador { get; set; }
        public String Asignacion_Calificador { get; set; }
        public DateTime? Fecha_inicio_ULTIMA_Revision_HyP { get; set; }
        public String Observado { get; set; }
        public string CircuitoOrigen { get; set; }
    }
    
    public class UltimaRevisionHoja6
    {
        public Int32? solicitud { get; set; }
        public DateTime? Fecha_Inicio_Tarea { get; set; }
        public String Asignacion_Calificador { get; set; }
        public DateTime? Fecha_inicio_ULTIMA_Revision_HyP { get; set; }
        public DateTime? rfd_Cierre { get; set; }
        public String Observado { get; set; }
        public int?   Dif_EE_asig_cierre {get;set;}
        public int? Dif_RFD_asig_cierre { get; set; }
    }
    public class UltimaRevisionHoja6Bis
    {
        public Int32? solicitud { get; set; }
        public DateTime? Fecha_Inicio_Asignacion_Calificador { get; set; }
        public String Asignacion_Calificador { get; set; }
        public DateTime? Fecha_inicio_ULTIMA_Revision_HyP { get; set; }
        public DateTime? rfd_Cierre { get; set; }
        public String Observado { get; set; }
        public int? Dif_EE_asig_cierre { get; set; }
        public int? Dif_RFD_asig_cierre { get; set; }
        public string CircuitoOrigen { get; set; }
    }
    
    public class Especial
    {
        public Int32? Id_solicitud { get; set; }
        public DateTime? Fecha_inicio_asig_calif { get; set; }
        public TimeSpan? Hora_inicio_asig_calif { get; set; }

        public DateTime? Fecha_asig_asig_calif { get; set; }
        public TimeSpan? Hora_asig_asig_calif { get; set; }

        public DateTime? Fecha_fin_asig_calif { get; set; }
        public TimeSpan? Hora_fin_asig_calif { get; set; }

        public DateTime? Fecha_inicio_visar1 { get; set; }
        public TimeSpan? Hora_inicio_visar1 { get; set; }

        public DateTime? Fecha_asig_visar1 { get; set; }
        public TimeSpan? Hora_asig_visar1 { get; set; }

        public DateTime? Fecha_fin_visar1 { get; set; }
        public TimeSpan? Hora_fin_visar1 { get; set; }

        public DateTime? Fecha_inicio_verif_avh { get; set; }
        public TimeSpan? Hora_inicio_verif_avh { get; set; }

        public DateTime? Fecha_asig_verif_avh { get; set; }
        public TimeSpan? Hora_asig_verif_avh { get; set; }

        public DateTime? Fecha_fin_verif_avh { get; set; }
        public TimeSpan? Hora_fin_verif_avh { get; set; }

        public DateTime? Fecha_asig_rev_sgo { get; set; }
        public TimeSpan? Hora_asig_rev_sgo { get; set; }

        public DateTime? Fecha_fin_rev_sgo { get; set; }
        public TimeSpan? Hora_fin_rev_sgo { get; set; }

        public DateTime? Fecha_inicio_rev_go_1 { get; set; }
        public TimeSpan? Hora_inicio_rev_go_1 { get; set; }

        public DateTime? Fecha_asig_rev_go_1 { get; set; }
        public TimeSpan? Hora_asig_rev_go_1 { get; set; }

        public DateTime? Fecha_fin_rev_go_1 { get; set; }
        public TimeSpan? Hora_fin_rev_go_1 { get; set; }

        public DateTime? Fecha_inicio_dict_asig_prof { get; set; }
        public TimeSpan? Hora_inicio_dict_asig_prof { get; set; }

        public DateTime? Fecha_asig_dict_asig_prof { get; set; }
        public TimeSpan? Hora_asig_dict_asig_prof { get; set; }

        public DateTime? Fecha_fin_dict_asig_prof { get; set; }
        public TimeSpan? Hora_fin_dict_asig_prof { get; set; }

        public DateTime? Fecha_inicio_dict_rev_tram { get; set; }
        public TimeSpan? Hora_inicio_dict_rev_tram { get; set; }

        public DateTime? Fecha_asig_dict_rev_tram { get; set; }
        public TimeSpan? Hora_asig_dict_rev_tram { get; set; }

        public DateTime? Fecha_fin_dict_rev_tram { get; set; }
        public TimeSpan? Hora_fin_dict_rev_tram { get; set; }

        public DateTime? Fecha_inicio_dict_rev_sgo { get; set; }
        public TimeSpan? Hora_inicio_dict_rev_sgo { get; set; }

        public DateTime? Fecha_asig_dict_rev_sgo { get; set; }
        public TimeSpan? Hora_asig_dict_rev_sgo { get; set; }

        public DateTime? Fecha_fin_dict_rev_sgo { get; set; }
        public TimeSpan? Hora_fin_dict_rev_sgo { get; set; }

        public DateTime? Fecha_inicio_dict_rev_go { get; set; }
        public TimeSpan? Hora_inicio_dict_rev_go { get; set; }

        public DateTime? Fecha_asig_dict_rev_go { get; set; }
        public TimeSpan? Hora_asig_dict_rev_go { get; set; }

        public DateTime? Fecha_fin_dict_rev_go { get; set; }
        public TimeSpan? Hora_fin_dict_rev_go { get; set; }

        public DateTime? Fecha_inicio_dict_gedo { get; set; }
        public TimeSpan? Hora_inicio_dict_gedo { get; set; }

        public DateTime? Fecha_asig_dict_gedo { get; set; }
        public TimeSpan? Hora_asig_dict_gedo { get; set; }

        public DateTime? Fecha_fin_dict_gedo { get; set; }
        public TimeSpan? Hora_fin_dict_gedo { get; set; }

        public DateTime? FechaInicio_tramitetarea { get; set; }
        public TimeSpan? HoraInicio_tramitetarea { get; set; }

        public DateTime? Fecha_asig_gen_bol { get; set; }
        public TimeSpan? Hora_asig_gen_bol { get; set; }

        public DateTime? Fecha_fin_gen_bol { get; set; }
        public TimeSpan? Hora_fin_gen_bol { get; set; }

        public DateTime? Fecha_inicio_rev_pag { get; set; }
        public TimeSpan? Hora_inicio_rev_pag { get; set; }

        public DateTime? Fecha_asig_rev_pag { get; set; }
        public TimeSpan? Hora_asig_rev_pag { get; set; }

        public DateTime? Fecha_fin_rev_pag { get; set; }
        public TimeSpan? Hora_fin_rev_pag { get; set; }

        public DateTime? Fecha_inicio_gen_exp { get; set; }
        public TimeSpan? Hora_inicio_gen_exp { get; set; }

        public DateTime? Fecha_asig_gen_exp { get; set; }
        public TimeSpan? Hora_asig_gen_exp { get; set; }

        public DateTime? Fecha_fin_gen_exp { get; set; }
        public TimeSpan? Hora_fin_gen_exp { get; set; }

        public DateTime? Fecha_inicio_rev_dghp { get; set; }
        public TimeSpan? Hora_inicio_rev_dghp { get; set; }

        public DateTime? Fecha_asig_rev_dghp { get; set; }
        public TimeSpan? Hora_asig_rev_dghp { get; set; }

        public DateTime? Fecha_fin_rev_dghp { get; set; }
        public TimeSpan? Hora_fin_rev_dghp { get; set; }

        public DateTime? Fecha_inicio_rev_fir_dispo { get; set; }
        public TimeSpan? Hora_inicio_rev_fir_dispo { get; set; }

        public DateTime? Fecha_asig_rev_fir_dispo { get; set; }
        public TimeSpan? Hora_asig_rev_fir_dispo { get; set; }

        public DateTime? Fecha_fin_rev_fir_dispo { get; set; }
        public TimeSpan? Hora_fin_rev_fir_dispo { get; set; }

        public DateTime? Fecha_inicio_aprobados { get; set; }
        public TimeSpan? Hora_inicio_aprobados { get; set; }

        public DateTime? Fecha_asig_aprobados { get; set; }
        public TimeSpan? Hora_asig_aprobados { get; set; }

        public DateTime? Fecha_fin_aprobados { get; set; }
        public TimeSpan? Hora_fin_aprobados { get; set; }

        public DateTime? Fecha_inicio_ent_tra { get; set; }
        public TimeSpan? Hora_inicio_ent_tra { get; set; }

        public DateTime? Fecha_asig_ent_tra { get; set; }
        public TimeSpan? Hora_asig_ent_tra { get; set; }

        public DateTime? Fecha_fin_ent_tra { get; set; }
        public TimeSpan? Hora_fin_ent_tra { get; set; }

        public DateTime? Fecha_inicio_rechazo_sade { get; set; }
        public TimeSpan? Hora_inicio_rechazo_sade { get; set; }

        public DateTime? Fecha_asig_rechazo_sade { get; set; }
        public TimeSpan? Hora_asig_rechazo_sade { get; set; }

        public DateTime? Fecha_fin_rechazo_sade { get; set; }
        public TimeSpan? Hora_fin_rechazo_sade { get; set; }

        public DateTime? Fecha_inicio_fin_tra { get; set; }
        public TimeSpan? Hora_inicio_fin_tra { get; set; }

        public DateTime? Fecha_asig_fin_tra { get; set; }
        public TimeSpan? Hora_asig_fin_tra { get; set; }

        public DateTime? Fecha_fin_fin_tra { get; set; }
        public TimeSpan? Hora_fin_fin_tra { get; set; }

        public DateTime? Fecha_inicio_visar2 { get; set; }
        public TimeSpan? Hora_inicio_visar2 { get; set; }

        public DateTime? Fecha_asig_visar2 { get; set; }
        public TimeSpan? Hora_asig_visar2 { get; set; }

        public DateTime? Fecha_fin_visar2 { get; set; }
        public TimeSpan? Hora_fin_visar2 { get; set; }

        public DateTime? Fecha_inicio_rev_go_2 { get; set; }
        public TimeSpan? Hora_inicio_rev_go_2 { get; set; }

        public DateTime? Fecha_asig_rev_go_2 { get; set; }
        public TimeSpan? Hora_asig_rev_go_2 { get; set; }

        public DateTime? Fecha_fin_rev_go_2 { get; set; }
        public TimeSpan? Hora_fin_rev_go_2 { get; set; }
    }

    public class ListadoVidaHabilitacion
    {
        public int? Id_solicitud { get; set; }
        public decimal? superficie { get; set; }
        public DateTime? Fecha_inicio_ASIGNACION_calif { get; set; }
        public TimeSpan? Hora_inicio_ASIGNACION_calif { get; set; }
        public DateTime? Fecha_fin_calif { get; set; }
        public TimeSpan? Hora_fin_calif { get; set; }
        public DateTime? Fecha_Inicio_Exp { get; set; }
        public TimeSpan? Hora_Inicio_Exp { get; set; }
        public DateTime? Fecha_Asignacion_Exp { get; set; }
        public TimeSpan? Hora_Asignacion_Exp { get; set; }
        public DateTime? Fecha_Cierre_Exp { get; set; }
        public TimeSpan? Hora_Cierre_Exp { get; set; }
        public DateTime? Fecha_Inicio_Rev_Ger { get; set; }
        public TimeSpan? Hora_Inicio_Rev_Ger { get; set; }
        public DateTime? Fecha_Cierre_Rev_Ger { get; set; }
        public TimeSpan? Hora_Cierre_Rev_Ger { get; set; }
        public DateTime? Fecha_Gen_BU_ini { get; set; }
        public DateTime? Fecha_Gen_BU_cierre { get; set; }
        public DateTime? Fecha_Rev_BU_ini { get; set; }
        public DateTime? Fecha_Rev_BU_cierre { get; set; }
        public DateTime? observado_alguna_vez { get; set; }
        public DateTime? Fecha_Cierre_Revision { get; set; }
        public DateTime? Fecha_Asign_Revision { get; set; }
        public DateTime? Fecha_Ini_Revision { get; set; }
        public DateTime? Fecha_Cierre_Entrega { get; set; }
        public DateTime? Fecha_Ini_Entrega { get; set; }
        public DateTime? FirmaRDGHP_Inicio { get; set; }
        public DateTime? FirmaRDGHP_Cierre { get; set; }
        public string numeroGEDO { get; set; }
    }
}