using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SGI.Reportes.Indicadores
{
    public class ConsultaPadron
    {
        public DGHP_Entities db { get; set; }

        public IQueryable<TramiteDTO> ObservacionesInternas(DateTime dtInicio, DateTime dtFin)
        {
            var query =
             (
                    from trah in db.SGI_Tramites_Tareas_CPADRON
                    join tt in db.SGI_Tramites_Tareas on trah.id_tramitetarea equals tt.id_tramitetarea
                    join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                    join sger in db.SGI_Tarea_Revision_SubGerente on tt.id_tramitetarea equals sger.id_tramitetarea
                    where
                      sger.Observaciones != "" &&
                      tar.id_circuito == 4 &&
                      tt.FechaCierre_tramitetarea >= dtInicio 
                      && tt.FechaCierre_tramitetarea <= dtFin
                      orderby trah.id_cpadron
                    select new TramiteDTO
                    {
                        IdSolicitud = trah.id_cpadron,
                        Observacion = sger.Observaciones,
                        Quien = "Subgerente",
                        Fecha = DbFunctions.TruncateTime(tt.FechaCierre_tramitetarea)
                    });

            return query; 
        }

        public IQueryable<TramiteSSPDTO> ObservacionesContribuyenteGerSubCalificador(DateTime dtInicio, DateTime dtFin)
        {
            var query =  (
                    (
                    from cal in db.SGI_Tarea_Carga_Tramite
                    join tra in db.SGI_Tramites_Tareas on cal.id_tramitetarea equals tra.id_tramitetarea into tra_join
                    from tra in tra_join.DefaultIfEmpty()
                    join trah in db.SGI_Tramites_Tareas_CPADRON on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                    from trah in trah_join.DefaultIfEmpty()
                    join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into res_join
                    from res in res_join.DefaultIfEmpty()
                    join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                    from tar in tar_join.DefaultIfEmpty()
                    where
                      tra.id_tarea == 55 &&
                      res.id_resultado == 20 &&
                      tar.id_circuito == 4 &&
                      tra.FechaCierre_tramitetarea >= dtInicio && tra.FechaCierre_tramitetarea <= dtFin
                    select new TramiteSSPDTO
                    {
                        IdSolicitud = trah.id_cpadron,
                        Observaciones = cal.observaciones_contribuyente,
                        FechaInicio_tramitetarea = tra.FechaInicio_tramitetarea,
                        FechaAsignacion_tramtietarea = tra.FechaAsignacion_tramtietarea,
                        FechaCierre_tramitetarea = tra.FechaCierre_tramitetarea,
                        Quien = "Control"
                    }
            ).Union
            (
                    from revsg in db.SGI_Tarea_Revision_SubGerente
                    join tra in db.SGI_Tramites_Tareas on revsg.id_tramitetarea equals tra.id_tramitetarea into tra_join
                    from tra in tra_join.DefaultIfEmpty()
                    join trah in db.SGI_Tramites_Tareas_CPADRON on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                    from trah in trah_join.DefaultIfEmpty()
                    join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into res_join
                    from res in res_join.DefaultIfEmpty()
                    join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                    from tar in tar_join.DefaultIfEmpty()
                    where
                      tra.id_tarea == 74 &&
                      res.id_resultado == 20 &&
                      tar.id_circuito == 4 &&
                      tra.FechaCierre_tramitetarea >= dtInicio && tra.FechaCierre_tramitetarea <= dtFin
                    select new TramiteSSPDTO
                    {
                        IdSolicitud = trah.id_cpadron,
                        Observaciones = revsg.observaciones_contribuyente,
                        FechaInicio_tramitetarea = tra.FechaInicio_tramitetarea,
                        FechaAsignacion_tramtietarea = tra.FechaAsignacion_tramtietarea,
                        FechaCierre_tramitetarea = tra.FechaCierre_tramitetarea,
                        Quien = "Subgerente"
                    }
            ));

            return query.OrderBy(p => p.IdSolicitud);

        }
        public IQueryable<TransferenciaTareaResultadoDTO> ListadoDeTareasYResultados(DateTime dtInicio, DateTime dtFin)
        {
            return (from tra in db.SGI_Tramites_Tareas
                    join trah in db.SGI_Tramites_Tareas_CPADRON on tra.id_tramitetarea equals trah.id_tramitetarea
                    join tr in db.CPadron_Solicitudes on trah.id_cpadron equals tr.id_cpadron
                    join cp_datloc in db.CPadron_DatosLocal on tr.id_cpadron equals cp_datloc.id_cpadron into pleft_cp_datloc
                    from cp_datloc in pleft_cp_datloc.DefaultIfEmpty()
                    join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado
                    join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea
                    join usu in db.aspnet_Users on tra.UsuarioAsignado_tramitetarea equals usu.UserId into usu_join
                    from usu in usu_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 4
                      && tra.FechaInicio_tramitetarea >= dtInicio && tra.FechaInicio_tramitetarea <= dtFin
                      && tr.SGI_Tramites_Tareas_CPADRON.Where(x=>x.SGI_Tramites_Tareas.ENG_Tareas.formulario_tarea != null
                       && x.SGI_Tramites_Tareas.ENG_Tareas.id_tarea != (int)Constants.ENG_Tareas.CP_Fin_Tramite).Count() > 0
                    orderby
                      trah.id_cpadron,
                      tra.FechaInicio_tramitetarea
                    select new TransferenciaTareaResultadoDTO
                    {
                        IdSolicitud = trah.id_cpadron,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = res.nombre_resultado,
                        FechaInicio = DbFunctions.TruncateTime(tra.FechaInicio_tramitetarea),
                        HoraInicio = DbFunctions.CreateTime(tra.FechaInicio_tramitetarea.Hour, tra.FechaInicio_tramitetarea.Minute, tra.FechaInicio_tramitetarea.Second),
                        FechaAsignacion = DbFunctions.TruncateTime(tra.FechaAsignacion_tramtietarea),
                        HoraAsignacion = DbFunctions.CreateTime(tra.FechaAsignacion_tramtietarea.Value.Hour, tra.FechaAsignacion_tramtietarea.Value.Minute, tra.FechaAsignacion_tramtietarea.Value.Second),
                        FechaCierre = DbFunctions.TruncateTime(tra.FechaCierre_tramitetarea),
                        HoraCierre = tra.FechaCierre_tramitetarea.HasValue ?  DbFunctions.CreateTime(tra.FechaCierre_tramitetarea.Value.Hour, tra.FechaCierre_tramitetarea.Value.Minute, tra.FechaCierre_tramitetarea.Value.Second) : null ,
                        Dif_ini_cierre = DbFunctions.DiffDays(tra.FechaInicio_tramitetarea, tra.FechaCierre_tramitetarea) - (2 * (DbFunctions.DiffDays(tra.FechaInicio_tramitetarea, tra.FechaCierre_tramitetarea) / 7)),
                        Dif_asig_cierre = DbFunctions.DiffDays(tra.FechaAsignacion_tramtietarea, tra.FechaCierre_tramitetarea) - (2 * (DbFunctions.DiffDays(tra.FechaAsignacion_tramtietarea, tra.FechaCierre_tramitetarea) / 7)),
                        UserName = usu.UserName,
                        Superficie = cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl
                    }).OrderBy(p => p.IdSolicitud).ThenBy(p => p.FechaInicio)
                                        .ThenBy(p => p.HoraInicio)
                                                .ThenBy(p => p.FechaAsignacion)
                                                        .ThenBy(p => p.HoraAsignacion)
                                                                .ThenBy(p => p.FechaCierre)
                                                                        .ThenBy(p => p.HoraCierre);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        public IQueryable<ListadoVida> ListadoDeVida(DateTime dtInicio, DateTime dtFin)
        {
           var query = (  from solcp in (
            (from tra0 in db.SGI_Tramites_Tareas
             join trah1 in db.SGI_Tramites_Tareas_CPADRON on tra0.id_tramitetarea equals trah1.id_tramitetarea
             join cp_datloc in db.CPadron_DatosLocal on trah1.id_cpadron equals cp_datloc.id_cpadron into cp_datloc_join
             from cp_datloc in cp_datloc_join.DefaultIfEmpty()
             where
               tra0.id_tarea == 54 &&
               tra0.FechaCierre_tramitetarea != null
             select new
             {
                 Id_cpadron = trah1.id_cpadron,
                 inicio_tarea = tra0.FechaInicio_tramitetarea,
                 asig_tarea = tra0.FechaAsignacion_tramtietarea,
                 fin_tarea = tra0.FechaCierre_tramitetarea,
                 superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
             }))
            join continf in (
                (from tra0 in db.SGI_Tramites_Tareas
                 join trah1 in db.SGI_Tramites_Tareas_CPADRON on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
                 from trah1 in trah1_join.DefaultIfEmpty()                 
                 where
                tra0.id_tarea == 55
                 select new
                 {
                     Id_cpadron = trah1.id_cpadron,
                     inicio_tarea = tra0.FechaInicio_tramitetarea,
                     asig_tarea = tra0.FechaAsignacion_tramtietarea,
                     fin_tarea = tra0.FechaCierre_tramitetarea                     
                 })) on solcp.Id_cpadron equals continf.Id_cpadron into continf_join
            from continf in continf_join.DefaultIfEmpty()
            join genexp in (
                (from tra0 in db.SGI_Tramites_Tareas
                 join trah1 in db.SGI_Tramites_Tareas_CPADRON on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
                 from trah1 in trah1_join.DefaultIfEmpty()                 
                 where
                tra0.id_tarea == 56
                 select new
                 {
                     Id_cpadron = trah1.id_cpadron,
                     inicio_tarea = tra0.FechaInicio_tramitetarea,
                     asig_tarea = tra0.FechaAsignacion_tramtietarea,
                     fin_tarea = tra0.FechaCierre_tramitetarea                     
                 })) on solcp.Id_cpadron equals genexp.Id_cpadron into genexp_join
            from genexp in genexp_join.DefaultIfEmpty()
            join rev_sgo in (
                (from tra0 in db.SGI_Tramites_Tareas
                 join trah1 in db.SGI_Tramites_Tareas_CPADRON on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
                 from trah1 in trah1_join.DefaultIfEmpty()                 
                 where
                tra0.id_tarea == 74
                 select new
                 {
                     Id_cpadron = trah1.id_cpadron,
                     inicio_tarea = tra0.FechaInicio_tramitetarea,
                     asig_tarea = tra0.FechaAsignacion_tramtietarea,
                     fin_tarea = tra0.FechaCierre_tramitetarea,                     
                 })) on solcp.Id_cpadron equals rev_sgo.Id_cpadron into rev_sgo_join
            from rev_sgo in rev_sgo_join.DefaultIfEmpty()
            join fin_tra in (
                (from tra0 in db.SGI_Tramites_Tareas
                 join trah1 in db.SGI_Tramites_Tareas_CPADRON on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
                 from trah1 in trah1_join.DefaultIfEmpty()                 
                 where
                tra0.id_tarea == 57
                 select new
                 {
                     Id_cpadron = trah1.id_cpadron,
                     inicio_tarea = tra0.FechaInicio_tramitetarea,
                     asig_tarea = tra0.FechaAsignacion_tramtietarea,
                     fin_tarea = tra0.FechaCierre_tramitetarea                     
                 })) on solcp.Id_cpadron equals fin_tra.Id_cpadron into fin_tra_join
            from fin_tra in fin_tra_join.DefaultIfEmpty()
            join correc_sol in (
                (from tra0 in db.SGI_Tramites_Tareas
                  join trah1 in db.SGI_Tramites_Tareas_CPADRON on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
                  from trah1 in trah1_join.DefaultIfEmpty()
                  where
                    tra0.id_tarea == 134151
                  select new
                  {
                      Id_cpadron = trah1.id_cpadron,
                      corr_solicitud = tra0.FechaInicio_tramitetarea
                  }).Distinct()) on solcp.Id_cpadron equals correc_sol.Id_cpadron into correc_sol_join
            from correc_sol in correc_sol_join.DefaultIfEmpty()
            join trah in db.SGI_Tramites_Tareas_CPADRON on continf.Id_cpadron equals trah.id_cpadron into trah_join
            from trah in trah_join.DefaultIfEmpty()
            join tra in db.SGI_Tramites_Tareas on trah.id_tramitetarea equals tra.id_tramitetarea into tra_join
            from tra in tra_join.DefaultIfEmpty()
            join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
            from tar in tar_join.DefaultIfEmpty()
            where
              continf.inicio_tarea != null &&
              tar.id_circuito == 4 &&
              continf.inicio_tarea >= dtInicio && continf.inicio_tarea <= dtFin
            group new { continf, solcp, genexp, rev_sgo, fin_tra, correc_sol } by new
            {
                continf.Id_cpadron,
                solcp.superficie,
                solcp.inicio_tarea,
                solcp.asig_tarea,
                solcp.fin_tarea,
                Column1 = continf.inicio_tarea,
                Column2 = continf.asig_tarea,
                Column3 = continf.fin_tarea,
                Column4 = genexp.inicio_tarea,
                Column5 = genexp.asig_tarea,
                Column6 = genexp.fin_tarea,
                Column7 = rev_sgo.inicio_tarea,
                Column8 = rev_sgo.asig_tarea,
                Column9 = rev_sgo.fin_tarea,
                Column10 = fin_tra.inicio_tarea,
                Column11 = fin_tra.asig_tarea,
                Column12 = fin_tra.fin_tarea,
                correc_sol.corr_solicitud
            } into g
            orderby
              g.Key.Id_cpadron
            select new ListadoVida
            {
                Id_cpadron = g.Key.Id_cpadron,
                superficie = g.Key.superficie,
                Fecha_inicio_Solicitud_CP = DbFunctions.TruncateTime(g.Key.inicio_tarea),
                Hora_inicio_Solicitud_CP = DbFunctions.CreateTime(g.Key.inicio_tarea.Hour, g.Key.inicio_tarea.Minute,g.Key.inicio_tarea.Second),
                Fecha_asig_Solicitud_CP = DbFunctions.TruncateTime(g.Key.asig_tarea),
                Hora_asig_Solicitud_CP = g.Key.asig_tarea.HasValue ? DbFunctions.CreateTime(g.Key.asig_tarea.Value.Hour,g.Key.asig_tarea.Value.Minute,g.Key.asig_tarea.Value.Second) : null,
                Fecha_fin_Solicitud_CP = DbFunctions.TruncateTime(g.Key.fin_tarea),
                Hora_fin_Solicitud_CP = g.Key.fin_tarea.HasValue ? DbFunctions.CreateTime(g.Key.fin_tarea.Value.Hour,g.Key.fin_tarea.Value.Minute,g.Key.fin_tarea.Value.Second) : null,
                Fecha_inicio_Control_Informes = DbFunctions.TruncateTime(g.Key.Column1),
                Hora_inicio_Control_Informes = DbFunctions.CreateTime(g.Key.Column1.Hour,g.Key.Column1.Minute,g.Key.Column1.Second),
                Fecha_asig_Control_Informes = DbFunctions.TruncateTime(g.Key.Column2),
                Hora_asig_Control_Informes = g.Key.Column2.HasValue ? DbFunctions.CreateTime(g.Key.Column2.Value.Hour,g.Key.Column2.Value.Minute,g.Key.Column2.Value.Second) : null,
                Fecha_fin_Control_Informes = DbFunctions.TruncateTime(g.Key.Column3),
                Hora_fin_Control_Informes = g.Key.Column3.HasValue ?  DbFunctions.CreateTime(g.Key.Column3.Value.Hour,g.Key.Column3.Value.Minute,g.Key.Column3.Value.Second) : null,
                Fecha_inicio_Gen_Exp = DbFunctions.TruncateTime(g.Key.Column4),
                Hora_inicio_Gen_Exp = DbFunctions.CreateTime(g.Key.Column4.Hour,g.Key.Column4.Minute,g.Key.Column4.Second),
                Fecha_asig_Gen_Exp = DbFunctions.TruncateTime(g.Key.Column5),
                Hora_asig_Gen_Exp = g.Key.Column5.HasValue ? DbFunctions.CreateTime(g.Key.Column5.Value.Hour,g.Key.Column5.Value.Minute,g.Key.Column5.Value.Second) : null,
                Fecha_fin_Gen_Exp = DbFunctions.TruncateTime(g.Key.Column6),
                Hora_fin_Gen_Exp = g.Key.Column6.HasValue ? DbFunctions.CreateTime(g.Key.Column6.Value.Hour,g.Key.Column6.Value.Minute,g.Key.Column6.Value.Second) : null,
                Fecha_inicio_rev_sgo = DbFunctions.TruncateTime(g.Key.Column7),
                Hora_inicio_rev_sgo = DbFunctions.CreateTime(g.Key.Column7.Hour,g.Key.Column7.Month,g.Key.Column7.Second),
                Fecha_asig_rev_sgo = DbFunctions.TruncateTime(g.Key.Column8),
                Hora_asig_rev_sgo = g.Key.Column8.HasValue ? DbFunctions.CreateTime(g.Key.Column8.Value.Hour,g.Key.Column8.Value.Minute,g.Key.Column8.Value.Minute) : null,
                Fecha_fin_rev_sgo = DbFunctions.TruncateTime(g.Key.Column9),
                Hora_fin_rev_sgo = g.Key.Column9.HasValue ? DbFunctions.CreateTime(g.Key.Column9.Value.Hour,g.Key.Column9.Value.Minute, g.Key.Column9.Value.Second) : null,
                Fecha_inicio_fin_tra = DbFunctions.TruncateTime(g.Key.Column10),
                Hora_inicio_fin_tra = DbFunctions.CreateTime(g.Key.Column10.Hour, g.Key.Column10.Minute,g.Key.Column10.Second),
                Fecha_asig_fin_tra = DbFunctions.TruncateTime(g.Key.Column11),
                Hora_asig_fin_tra = g.Key.Column11.HasValue ? DbFunctions.CreateTime(g.Key.Column11.Value.Hour, g.Key.Column11.Value.Minute, g.Key.Column11.Value.Second) : null,
                Fecha_fin_fin_tra = DbFunctions.TruncateTime(g.Key.Column12),
                Hora_fin_fin_tra = g.Key.Column12.HasValue ? DbFunctions.CreateTime(g.Key.Column12.Value.Hour,g.Key.Column12.Value.Minute,g.Key.Column12.Value.Second) : null,
                observado_alguna_vez = DbFunctions.TruncateTime(g.Key.corr_solicitud)
            });
            return query;
        }
    }
}