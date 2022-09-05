using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Data.Entity;

namespace SGI.Reportes.Indicadores
{
    public class Transferencia
    {
        public DGHP_Entities db { get; set; }

        public IQueryable<TramiteDTO> ObservacionesInternasGerSubCalificador(DateTime dtInicio, DateTime dtFin)
        {
            var query = (
                 from sger in db.SGI_Tarea_Revision_SubGerente
                 join tra in db.SGI_Tramites_Tareas on sger.id_tramitetarea equals tra.id_tramitetarea into tra_join
                 from tra in tra_join.DefaultIfEmpty()
                 join trah in db.SGI_Tramites_Tareas_TRANSF on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                 from trah in trah_join.DefaultIfEmpty()
                 join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                 from tar in tar_join.DefaultIfEmpty()
                 where
                   sger.Observaciones != "" &&
                   tar.id_circuito == 5 &&
                   tra.FechaCierre_tramitetarea >= dtInicio && tra.FechaCierre_tramitetarea <= dtFin
                 select new TramiteDTO
                 {
                     IdSolicitud = trah.id_solicitud,
                     Observacion = sger.Observaciones,
                     Quien = "Subgerente",
                     Fecha = DbFunctions.TruncateTime(tra.FechaCierre_tramitetarea)
                 }
             ).Union
             (
                 from ger in db.SGI_Tarea_Revision_Gerente
                 join tra in db.SGI_Tramites_Tareas on ger.id_tramitetarea equals tra.id_tramitetarea into tra_join
                 from tra in tra_join.DefaultIfEmpty()
                 join trah in db.SGI_Tramites_Tareas_TRANSF on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                 from trah in trah_join.DefaultIfEmpty()
                 join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                 from tar in tar_join.DefaultIfEmpty()
                 where
                   ger.Observaciones != "" &&
                   tar.id_circuito == 5 &&
                   tra.FechaCierre_tramitetarea >= dtInicio && tra.FechaCierre_tramitetarea <= dtFin
                 select new TramiteDTO
                 {
                     IdSolicitud = trah.id_solicitud,
                     Observacion = ger.Observaciones,
                     Quien = "Gerente",
                     Fecha = DbFunctions.TruncateTime(tra.FechaCierre_tramitetarea)
                 }
            );

            return query.OrderBy(p => p.IdSolicitud);

        }
        /*--*******OBSERVACIONES AL CONTRIBUYENTE DEL GERENTE/SUBGERENTE/CALIFICADOR***/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<TramiteSSPDTO> ObservacionesContribuyenteGerSubCalificador(DateTime dtInicio, DateTime dtFin)
        {
            var  query =
            (
                from cal in db.SGI_Tarea_Calificar
                join tra in db.SGI_Tramites_Tareas on cal.id_tramitetarea equals tra.id_tramitetarea into tra_join
                from tra in tra_join.DefaultIfEmpty()
                join trah in db.SGI_Tramites_Tareas_TRANSF on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                from trah in trah_join.DefaultIfEmpty()
                join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into res_join
                from res in res_join.DefaultIfEmpty()
                join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                from tar in tar_join.DefaultIfEmpty()
                where
                  tra.id_tarea == 62 &&
                  res.id_resultado == 20 &&
                  tar.id_circuito == 5 &&
                  tra.FechaCierre_tramitetarea >= dtInicio && tra.FechaCierre_tramitetarea <= dtFin
                select new TramiteSSPDTO
                {
                    IdSolicitud = trah.id_solicitud,
                    Observaciones = cal.Observaciones_contribuyente,
                    FechaInicio_tramitetarea = tra.FechaInicio_tramitetarea,
                    FechaAsignacion_tramtietarea = tra.FechaAsignacion_tramtietarea,
                    FechaCierre_tramitetarea = tra.FechaCierre_tramitetarea,
                    Quien = "Calificador"
                }
            ).Union
            (
                from revsg in db.SGI_Tarea_Revision_SubGerente
                join tra in db.SGI_Tramites_Tareas on revsg.id_tramitetarea equals tra.id_tramitetarea into tra_join
                from tra in tra_join.DefaultIfEmpty()
                join trah in db.SGI_Tramites_Tareas_TRANSF on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                from trah in trah_join.DefaultIfEmpty()
                join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into res_join
                from res in res_join.DefaultIfEmpty()
                join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                from tar in tar_join.DefaultIfEmpty()
                where
                    tra.id_tarea == 63 &&
                    res.id_resultado == 20 &&
                    tar.id_circuito == 5 &&
                    tra.FechaCierre_tramitetarea >= dtInicio && tra.FechaCierre_tramitetarea <= dtFin
                select new TramiteSSPDTO
                {
                    IdSolicitud = trah.id_solicitud,
                    Observaciones = revsg.observaciones_contribuyente,
                    FechaInicio_tramitetarea = tra.FechaInicio_tramitetarea,
                    FechaAsignacion_tramtietarea = tra.FechaAsignacion_tramtietarea,
                    FechaCierre_tramitetarea = tra.FechaCierre_tramitetarea,
                    Quien = "Subgerente"
                }
            ).Union
            (
                from revg in db.SGI_Tarea_Revision_Gerente
                join tra in db.SGI_Tramites_Tareas on revg.id_tramitetarea equals tra.id_tramitetarea into tra_join
                from tra in tra_join.DefaultIfEmpty()
                join trah in db.SGI_Tramites_Tareas_TRANSF on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                from trah in trah_join.DefaultIfEmpty()
                join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into res_join
                from res in res_join.DefaultIfEmpty()
                join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                from tar in tar_join.DefaultIfEmpty()
                where
                    (tra.id_tarea == 64 || tra.id_tarea == 86) &&
                    res.id_resultado == 20 &&
                    tar.id_circuito == 5 &&
                    tra.FechaCierre_tramitetarea >= dtInicio && tra.FechaCierre_tramitetarea <= dtFin
                select new TramiteSSPDTO
                {
                    IdSolicitud = trah.id_solicitud,
                    Observaciones = revg.observaciones_contribuyente,
                    FechaInicio_tramitetarea = tra.FechaInicio_tramitetarea,
                    FechaAsignacion_tramtietarea = tra.FechaAsignacion_tramtietarea,
                    FechaCierre_tramitetarea = tra.FechaCierre_tramitetarea,
                    Quien = "Gerente"
                }
                );

            return query.OrderBy(p => p.IdSolicitud);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        public IQueryable<TransferenciaTareaResultadoDTO> ListadoDeTareasYResultados(DateTime dtInicio, DateTime dtFin)
        {
            var q = (from tra in db.SGI_Tramites_Tareas
                     join trah in db.SGI_Tramites_Tareas_TRANSF on tra.id_tramitetarea equals trah.id_tramitetarea
                     join tr in db.Transf_Solicitudes on trah.id_solicitud equals tr.id_solicitud
                     join cp_datloc in db.CPadron_DatosLocal on tr.id_cpadron equals cp_datloc.id_cpadron
                     join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado
                     join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea
                     join usu in db.aspnet_Users on tra.UsuarioAsignado_tramitetarea equals usu.UserId into usu_join
                     from usu in usu_join.DefaultIfEmpty()
                     where
                       tar.id_circuito == 5
                       && tra.FechaInicio_tramitetarea >= dtInicio && tra.FechaInicio_tramitetarea <= dtFin
                     orderby
                       trah.id_solicitud,
                       tra.FechaInicio_tramitetarea
                     select new TransferenciaTareaResultadoDTO
                     {
                         IdSolicitud = trah.id_solicitud,
                         NombreTarea = tar.nombre_tarea,
                         NombreResultado = res.nombre_resultado,
                         FechaInicio = DbFunctions.TruncateTime(tra.FechaInicio_tramitetarea),
                         HoraInicio = DbFunctions.CreateTime(tra.FechaInicio_tramitetarea.Hour, tra.FechaInicio_tramitetarea.Minute, tra.FechaInicio_tramitetarea.Second),
                         FechaAsignacion = tra.FechaAsignacion_tramtietarea,
                         HoraAsignacion = tra.FechaAsignacion_tramtietarea.HasValue ? DbFunctions.CreateTime(tra.FechaAsignacion_tramtietarea.Value.Hour, tra.FechaAsignacion_tramtietarea.Value.Minute, tra.FechaAsignacion_tramtietarea.Value.Second) : null,
                         FechaCierre = DbFunctions.TruncateTime(tra.FechaCierre_tramitetarea),
                         HoraCierre = tra.FechaCierre_tramitetarea.HasValue ? DbFunctions.CreateTime(tra.FechaCierre_tramitetarea.Value.Hour, tra.FechaCierre_tramitetarea.Value.Minute, tra.FechaCierre_tramitetarea.Value.Second) : null,
                         Dif_ini_cierre = DbFunctions.DiffDays(tra.FechaInicio_tramitetarea, tra.FechaCierre_tramitetarea) - (2 * (DbFunctions.DiffDays(tra.FechaInicio_tramitetarea, tra.FechaCierre_tramitetarea) / 7)),
                         Dif_asig_cierre = DbFunctions.DiffDays(tra.FechaAsignacion_tramtietarea, tra.FechaCierre_tramitetarea) - (2 * (DbFunctions.DiffDays(tra.FechaAsignacion_tramtietarea, tra.FechaCierre_tramitetarea) / 7)),
                         UserName = usu.UserName,
                         Superficie = cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl
                     }).OrderBy(p => p.IdSolicitud)
                                .ThenBy(p => p.FechaInicio)
                                        .ThenBy(p => p.HoraInicio)
                                                .ThenBy(p => p.FechaAsignacion)
                                                        .ThenBy(p => p.HoraAsignacion)
                                                                .ThenBy(p => p.FechaCierre)
                                                                        .ThenBy(p => p.HoraCierre);

            return q;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ListadoVidaTransferencia> ListadoDeVida(DateTime dtInicio, DateTime dtFin)
        {
            var query = (
          from asig_calif in (
              (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 61
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               }))
                  join calif in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 62
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals calif.Id_solicitud into calif_join
                  from calif in calif_join.DefaultIfEmpty()
                  join rev_sgo in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 63
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals rev_sgo.Id_solicitud into rev_sgo_join
                  from rev_sgo in rev_sgo_join.DefaultIfEmpty()
                  join rev_go_1 in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 64
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals rev_go_1.Id_solicitud into rev_go_1_join
                  from rev_go_1 in rev_go_1_join.DefaultIfEmpty()
                  join rev_go_2 in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 86
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals rev_go_2.Id_solicitud into rev_go_2_join
                  from rev_go_2 in rev_go_2_join.DefaultIfEmpty()
                  join dict_asig_prof in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 65
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals dict_asig_prof.Id_solicitud into dict_asig_prof_join
                  from dict_asig_prof in dict_asig_prof_join.DefaultIfEmpty()
                  join genexp in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 66
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals genexp.Id_solicitud into genexp_join
                  from genexp in genexp_join.DefaultIfEmpty()
                  join rev_dghp in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 67
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals rev_dghp.Id_solicitud into rev_dghp_join
                  from rev_dghp in rev_dghp_join.DefaultIfEmpty()
                  join rev_fir_dispo in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 68
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals rev_fir_dispo.Id_solicitud into rev_fir_dispo_join
                  from rev_fir_dispo in rev_fir_dispo_join.DefaultIfEmpty()
                  join ent_tra in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 69
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals ent_tra.Id_solicitud into ent_tra_join
                  from ent_tra in ent_tra_join.DefaultIfEmpty()
                  join fin_tra in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 70
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals fin_tra.Id_solicitud into fin_tra_join
                  from fin_tra in fin_tra_join.DefaultIfEmpty()
                  join aprobados in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 73
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals aprobados.Id_solicitud into aprobados_join
                  from aprobados in aprobados_join.DefaultIfEmpty()
                  join dict_rev_tram in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 80
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals dict_rev_tram.Id_solicitud into dict_rev_tram_join
                  from dict_rev_tram in dict_rev_tram_join.DefaultIfEmpty()
                  join dict_rev_sgo in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 81
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals dict_rev_sgo.Id_solicitud into dict_rev_sgo_join
                  from dict_rev_sgo in dict_rev_sgo_join.DefaultIfEmpty()
                  join dict_rev_go in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 82
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals dict_rev_go.Id_solicitud into dict_rev_go_join
                  from dict_rev_go in dict_rev_go_join.DefaultIfEmpty()
                  join dict_gedo in (
                    (from tra0 in db.SGI_Tramites_Tareas
               join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
               from trah1 in trah1_join.DefaultIfEmpty()
               join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
               join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
               where

               tra0.id_tarea == 83
               select new
               {
                   Id_solicitud = (int?)trah1.id_solicitud,
                   inicio_tarea = tra0.FechaInicio_tramitetarea,
                   asig_tarea = tra0.FechaAsignacion_tramtietarea,
                   fin_tarea = tra0.FechaCierre_tramitetarea,
                   superficie = (cp_datloc.superficie_cubierta_dl + cp_datloc.superficie_descubierta_dl)
               })) on asig_calif.Id_solicitud equals dict_gedo.Id_solicitud into dict_gedo_join
                  from dict_gedo in dict_gedo_join.DefaultIfEmpty()
                  join correc_sol in (
                    ((from tra0 in db.SGI_Tramites_Tareas
                join trah1 in db.SGI_Tramites_Tareas_TRANSF on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
                from trah1 in trah1_join.DefaultIfEmpty()
                join transf in db.Transf_Solicitudes on trah1.id_solicitud equals transf.id_solicitud
                join cp_datloc in db.CPadron_DatosLocal on transf.id_cpadron equals cp_datloc.id_cpadron
                where

                tra0.id_tarea == 60
                select new
                {
                    Id_solicitud = (int?)trah1.id_solicitud,
                    corr_solicitud = tra0.FechaInicio_tramitetarea
                }).Distinct())) on asig_calif.Id_solicitud equals correc_sol.Id_solicitud into correc_sol_join
                  from correc_sol in correc_sol_join.DefaultIfEmpty()
                  join trah in db.SGI_Tramites_Tareas_TRANSF on asig_calif.Id_solicitud equals trah.id_solicitud  into trah_join
                  from trah in trah_join.DefaultIfEmpty()
                  join tra in db.SGI_Tramites_Tareas on trah.id_tramitetarea equals tra.id_tramitetarea into tra_join
                  from tra in tra_join.DefaultIfEmpty()
                  join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                  from tar in tar_join.DefaultIfEmpty()
                  where
                  asig_calif.inicio_tarea != null &&
                  tar.id_circuito == 5 &&
                  asig_calif.inicio_tarea >= dtInicio && asig_calif.inicio_tarea <= dtFin
                  group new { asig_calif, calif, rev_go_1, rev_go_2, dict_asig_prof, genexp, rev_dghp, rev_fir_dispo, ent_tra, fin_tra, aprobados, dict_rev_tram, dict_rev_sgo, dict_rev_go, dict_gedo, correc_sol } by new
                  {
                      asig_calif.Id_solicitud,
                      asig_calif.superficie,
                      asig_calif.inicio_tarea,
                      asig_calif.asig_tarea,
                      asig_calif.fin_tarea,
                      Column1 = calif.inicio_tarea,
                      Column2 = calif.asig_tarea,
                      Column3 = calif.fin_tarea,
                      Column4 = rev_go_1.inicio_tarea,
                      Column5 = rev_go_1.asig_tarea,
                      Column6 = rev_go_1.fin_tarea,
                      Column7 = rev_go_2.inicio_tarea,
                      Column8 = rev_go_2.asig_tarea,
                      Column9 = rev_go_2.fin_tarea,
                      Column10 = dict_asig_prof.inicio_tarea,
                      Column11 = dict_asig_prof.asig_tarea,
                      Column12 = dict_asig_prof.fin_tarea,
                      Column13 = genexp.inicio_tarea,
                      Column14 = genexp.asig_tarea,
                      Column15 = genexp.fin_tarea,
                      Column16 = rev_dghp.inicio_tarea,
                      Column17 = rev_dghp.asig_tarea,
                      Column18 = rev_dghp.fin_tarea,
                      Column19 = rev_fir_dispo.inicio_tarea,
                      Column20 = rev_fir_dispo.asig_tarea,
                      Column21 = rev_fir_dispo.fin_tarea,
                      Column22 = ent_tra.inicio_tarea,
                      Column23 = ent_tra.asig_tarea,
                      Column24 = ent_tra.fin_tarea,
                      Column25 = fin_tra.inicio_tarea,
                      Column26 = fin_tra.asig_tarea,
                      Column27 = fin_tra.fin_tarea,
                      Column28 = aprobados.inicio_tarea,
                      Column29 = aprobados.asig_tarea,
                      Column30 = aprobados.fin_tarea,
                      Column31 = dict_rev_tram.inicio_tarea,
                      Column32 = dict_rev_tram.asig_tarea,
                      Column33 = dict_rev_tram.fin_tarea,
                      Column34 = dict_rev_sgo.inicio_tarea,
                      Column35 = dict_rev_sgo.asig_tarea,
                      Column36 = dict_rev_sgo.fin_tarea,
                      Column37 = dict_rev_go.inicio_tarea,
                      Column38 = dict_rev_go.asig_tarea,
                      Column39 = dict_rev_go.fin_tarea,
                      Column40 = dict_gedo.inicio_tarea,
                      Column41 = dict_gedo.asig_tarea,
                      Column42 = dict_gedo.fin_tarea,
                      correc_sol.corr_solicitud
                  } into g
                  orderby
                  g.Key.Id_solicitud
                
                  select new ListadoVidaTransferencia
                  {
                      Id_solicitud = (int?)g.Key.Id_solicitud,
                      superficie = (decimal?)g.Key.superficie,
                      Fecha_inicio_asig_calif = DbFunctions.TruncateTime(g.Key.inicio_tarea),
                      Hora_inicio_asig_calif = DbFunctions.CreateTime(g.Key.inicio_tarea.Hour,g.Key.inicio_tarea.Minute, g.Key.inicio_tarea.Second),
                      Fecha_asig_asig_calif = DbFunctions.TruncateTime(g.Key.asig_tarea),
                      Hora_asig_asig_calif = DbFunctions.CreateTime(g.Key.asig_tarea.Value.Hour, g.Key.asig_tarea.Value.Minute, g.Key.asig_tarea.Value.Second),
                      Fecha_fin_asig_calif = DbFunctions.TruncateTime(g.Key.fin_tarea),
                      Hora_fin_asig_calif = DbFunctions.CreateTime(g.Key.fin_tarea.Value.Hour,g.Key.fin_tarea.Value.Minute, g.Key.fin_tarea.Value.Second),
                      Fecha_inicio_calif = DbFunctions.TruncateTime(g.Key.Column1),
                      Hora_inicio_calif = DbFunctions.CreateTime(g.Key.Column1.Hour, g.Key.Column1.Minute, g.Key.Column1.Second),
                      Fecha_asig_calif = DbFunctions.TruncateTime(g.Key.Column2),
                      Hora_asig_calif = g.Key.Column2.HasValue ? DbFunctions.CreateTime(g.Key.Column2.Value.Hour, g.Key.Column2.Value.Minute, g.Key.Column2.Value.Second) : null,
                      Fecha_fin_calif = DbFunctions.TruncateTime(g.Key.Column3),
                      Column1 = DbFunctions.TruncateTime(g.Key.Column3),
                      Fecha_inicio_rev_go_1 = DbFunctions.TruncateTime(g.Key.Column4),
                      Hora_inicio_rev_go_1 = DbFunctions.CreateTime(g.Key.Column4.Hour, g.Key.Column4.Minute, g.Key.Column4.Second),
                      Fecha_asig_rev_go_1 = DbFunctions.TruncateTime(g.Key.Column5),
                      Hora_asig_rev_go_1 =  g.Key.Column5.HasValue ? DbFunctions.CreateTime(g.Key.Column5.Value.Hour, g.Key.Column5.Value.Minute, g.Key.Column5.Value.Second) : null,
                      Fecha_fin_rev_go_1f = DbFunctions.TruncateTime(g.Key.Column6),
                      Hora_fin_rev_go_1 = g.Key.Column6.HasValue ? DbFunctions.CreateTime(g.Key.Column6.Value.Hour, g.Key.Column6.Value.Minute, g.Key.Column6.Value.Second) : null,
                      Fecha_inicio_rev_go_2 = DbFunctions.TruncateTime(g.Key.Column7),
                      Hora_inicio_rev_go_2 =DbFunctions.CreateTime(g.Key.Column7.Hour, g.Key.Column7.Minute, g.Key.Column7.Second),
                      Fecha_asig_rev_go_2 = DbFunctions.TruncateTime(g.Key.Column8),
                      Hora_asig_rev_go_2 = g.Key.Column8.HasValue ? DbFunctions.CreateTime(g.Key.Column8.Value.Hour, g.Key.Column8.Value.Minute, g.Key.Column8.Value.Second) : null,
                      Fecha_fin_rev_go_2f = DbFunctions.TruncateTime(g.Key.Column9),
                      Hora_fin_rev_go_2 = g.Key.Column9.HasValue ? DbFunctions.CreateTime(g.Key.Column9.Value.Hour, g.Key.Column9.Value.Minute, g.Key.Column9.Value.Second) : null,
                      Fecha_inicio_dict_asig_prof = DbFunctions.TruncateTime(g.Key.Column10),
                      Hora_inicio_dict_asig_prof = DbFunctions.CreateTime(g.Key.Column10.Hour, g.Key.Column10.Minute, g.Key.Column10.Second),
                      Fecha_asig_dict_asig_prof = DbFunctions.TruncateTime(g.Key.Column11),
                      Hora_asig_dict_asig_prof = g.Key.Column11.HasValue ? DbFunctions.CreateTime(g.Key.Column11.Value.Hour, g.Key.Column11.Value.Minute, g.Key.Column11.Value.Second) : null,
                      Fecha_fin_dict_asig_proff = DbFunctions.TruncateTime(g.Key.Column12),
                      Hora_fin_dict_asig_prof = g.Key.Column12.HasValue? DbFunctions.CreateTime(g.Key.Column12.Value.Hour, g.Key.Column12.Value.Minute, g.Key.Column12.Value.Second) : null,
                      Fecha_inicio_genexp = DbFunctions.TruncateTime(g.Key.Column13),
                      Hora_inicio_genexp = DbFunctions.CreateTime(g.Key.Column13.Hour, g.Key.Column13.Minute, g.Key.Column13.Second),
                      Fecha_asig_genexp = DbFunctions.TruncateTime(g.Key.Column14),
                      Hora_asig_genexp = g.Key.Column14.HasValue ? DbFunctions.CreateTime(g.Key.Column14.Value.Hour, g.Key.Column14.Value.Minute, g.Key.Column14.Value.Second) : null,
                      Fecha_fin_genexpf = DbFunctions.TruncateTime(g.Key.Column15),
                      Hora_fin_genexp = g.Key.Column15.HasValue ? DbFunctions.CreateTime(g.Key.Column15.Value.Hour, g.Key.Column15.Value.Minute, g.Key.Column15.Value.Second) : null,
                      Fecha_inicio_rev_dghp = DbFunctions.TruncateTime(g.Key.Column16),
                      Hora_inicio_rev_dghp = DbFunctions.CreateTime(g.Key.Column16.Hour, g.Key.Column16.Minute, g.Key.Column16.Second),
                      Fecha_asig_rev_dghp = DbFunctions.TruncateTime(g.Key.Column17),
                      Hora_asig_rev_dghp = g.Key.Column17.HasValue ? DbFunctions.CreateTime(g.Key.Column17.Value.Hour, g.Key.Column17.Value.Minute, g.Key.Column17.Value.Second) : null,
                      Fecha_fin_rev_dghpf = DbFunctions.TruncateTime(g.Key.Column18),
                      Hora_fin_rev_dghp = g.Key.Column18.HasValue ?  DbFunctions.CreateTime(g.Key.Column18.Value.Hour, g.Key.Column18.Value.Minute, g.Key.Column18.Value.Second) : null,
                      Fecha_inicio_rev_fir_dispo = DbFunctions.TruncateTime(g.Key.Column19),
                      Hora_inicio_rev_fir_dispo = DbFunctions.CreateTime(g.Key.Column19.Hour, g.Key.Column19.Minute, g.Key.Column19.Second),
                      Fecha_asig_rev_fir_dispo = DbFunctions.TruncateTime(g.Key.Column20),
                      Hora_asig_rev_fir_dispo = g.Key.Column20.HasValue ? DbFunctions.CreateTime(g.Key.Column20.Value.Hour, g.Key.Column20.Value.Minute, g.Key.Column20.Value.Second) : null,
                      Fecha_fin_rev_fir_dispof = DbFunctions.TruncateTime(g.Key.Column21),
                      Hora_fin_rev_fir_dispo = g.Key.Column21.HasValue ? DbFunctions.CreateTime(g.Key.Column21.Value.Hour, g.Key.Column21.Value.Minute, g.Key.Column21.Value.Second) : null,
                      Fecha_inicio_ent_tra = DbFunctions.TruncateTime(g.Key.Column22),
                      Hora_inicio_ent_tra = DbFunctions.CreateTime(g.Key.Column22.Hour, g.Key.Column22.Minute, g.Key.Column22.Second),
                      Fecha_asig_ent_tra = DbFunctions.TruncateTime(g.Key.Column23),
                      Hora_asig_ent_tra = g.Key.Column23.HasValue ? DbFunctions.CreateTime(g.Key.Column23.Value.Hour, g.Key.Column23.Value.Minute, g.Key.Column23.Value.Second) : null,
                      Fecha_fin_ent_traf = DbFunctions.TruncateTime(g.Key.Column24),
                      Hora_fin_ent_tra = g.Key.Column24.HasValue ? DbFunctions.CreateTime(g.Key.Column24.Value.Hour, g.Key.Column24.Value.Minute, g.Key.Column24.Value.Second) : null,
                      Fecha_inicio_fin_tra = DbFunctions.TruncateTime(g.Key.Column25),
                      Hora_inicio_fin_tra = DbFunctions.CreateTime(g.Key.Column25.Hour, g.Key.Column25.Minute, g.Key.Column25.Second),
                      Fecha_asig_fin_tra = DbFunctions.TruncateTime(g.Key.Column26),
                      Hora_asig_fin_tra = g.Key.Column26.HasValue ? DbFunctions.CreateTime(g.Key.Column26.Value.Hour, g.Key.Column26.Value.Minute, g.Key.Column26.Value.Second) : null,
                      Fecha_fin_fin_traf = DbFunctions.TruncateTime(g.Key.Column27),
                      Hora_fin_fin_tra = g.Key.Column27.HasValue ? DbFunctions.CreateTime(g.Key.Column27.Value.Hour, g.Key.Column27.Value.Minute, g.Key.Column27.Value.Second) : null,
                      Fecha_inicio_aprobados = DbFunctions.TruncateTime(g.Key.Column28),
                      Hora_inicio_aprobados = DbFunctions.CreateTime(g.Key.Column28.Hour, g.Key.Column28.Minute, g.Key.Column28.Second),
                      Fecha_asig_aprobados = DbFunctions.TruncateTime(g.Key.Column29),
                      Hora_asig_aprobados = g.Key.Column29.HasValue ? DbFunctions.CreateTime(g.Key.Column29.Value.Hour, g.Key.Column29.Value.Minute, g.Key.Column29.Value.Second) : null,
                      Fecha_fin_aprobadosf = DbFunctions.TruncateTime(g.Key.Column30),
                      Hora_fin_aprobados = g.Key.Column30.HasValue ? DbFunctions.CreateTime(g.Key.Column30.Value.Hour, g.Key.Column30.Value.Minute, g.Key.Column30.Value.Second) : null,
                      Fecha_inicio_dict_rev_tram = DbFunctions.TruncateTime(g.Key.Column31),
                      Hora_inicio_dict_rev_tram = DbFunctions.CreateTime(g.Key.Column31.Hour, g.Key.Column31.Minute, g.Key.Column31.Second),
                      Fecha_asig_dict_rev_tram = DbFunctions.TruncateTime(g.Key.Column32),
                      Hora_asig_dict_rev_tram = DbFunctions.CreateTime(g.Key.Column32.Value.Hour, g.Key.Column32.Value.Minute, g.Key.Column32.Value.Second),
                      Fecha_fin_dict_rev_tramf = DbFunctions.TruncateTime(g.Key.Column33),
                      Hora_fin_dict_rev_tram = DbFunctions.CreateTime(g.Key.Column33.Value.Hour, g.Key.Column33.Value.Minute, g.Key.Column33.Value.Second),
                      Fecha_inicio_dict_rev_sgo =DbFunctions.TruncateTime(g.Key.Column34),
                      Hora_inicio_dict_rev_sgo = DbFunctions.CreateTime(g.Key.Column34.Hour, g.Key.Column34.Minute, g.Key.Column34.Second),
                      Fecha_asig_dict_rev_sgo = DbFunctions.TruncateTime(g.Key.Column35),
                      Hora_asig_dict_rev_sgo = DbFunctions.CreateTime(g.Key.Column35.Value.Hour, g.Key.Column35.Value.Minute, g.Key.Column35.Value.Second),
                      Fecha_fin_dict_rev_sgof = DbFunctions.TruncateTime(g.Key.Column36),
                      Hora_fin_dict_rev_sgo = DbFunctions.CreateTime(g.Key.Column36.Value.Hour, g.Key.Column36.Value.Minute, g.Key.Column36.Value.Second),
                      Fecha_inicio_dict_rev_go = DbFunctions.TruncateTime(g.Key.Column37),
                      Hora_inicio_dict_rev_go = DbFunctions.CreateTime(g.Key.Column37.Hour, g.Key.Column37.Minute, g.Key.Column37.Second),
                      Fecha_asig_dict_rev_go = DbFunctions.TruncateTime(g.Key.Column38),
                      Hora_asig_dict_rev_go = DbFunctions.CreateTime(g.Key.Column38.Value.Hour, g.Key.Column38.Value.Minute, g.Key.Column38.Value.Second),
                      Fecha_fin_dict_rev_gof = DbFunctions.TruncateTime(g.Key.Column39),
                      Hora_fin_dict_rev_go = DbFunctions.CreateTime(g.Key.Column39.Value.Hour, g.Key.Column39.Value.Minute, g.Key.Column39.Value.Second),
                      Fecha_inicio_dict_gedo = DbFunctions.TruncateTime(g.Key.Column40),
                      Hora_inicio_dict_gedo = DbFunctions.CreateTime(g.Key.Column40.Hour, g.Key.Column40.Minute, g.Key.Column40.Second),
                      Fecha_asig_dict_gedo = DbFunctions.TruncateTime(g.Key.Column41),
                      Hora_asig_dict_gedo = DbFunctions.CreateTime(g.Key.Column41.Value.Hour, g.Key.Column41.Value.Minute, g.Key.Column41.Value.Second),
                      Fecha_fin_dict_gedof = DbFunctions.TruncateTime(g.Key.Column42),
                      Hora_fin_dict_gedo = DbFunctions.CreateTime(g.Key.Column42.Value.Hour, g.Key.Column42.Value.Minute, g.Key.Column42.Value.Second),
                      observado_alguna_vez = DbFunctions.TruncateTime(g.Key.corr_solicitud)
                  });

            return query;
        } 
    }
}