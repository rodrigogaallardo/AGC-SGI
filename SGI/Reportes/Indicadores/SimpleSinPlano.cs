using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SGI.Reportes.Indicadores
{
    public class Habilitaciones
    {
        public DGHP_Entities db { get; set; }
        /// <summary>
        /// Simple sin Plano
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<TramiteDTO> ObservacionesInternasGerSubCalificador(DateTime dtInicio, DateTime dtFin, int IdCircuito)
        {
            var query = (
                 from sger in db.SGI_Tarea_Revision_SubGerente
                 join tra in db.SGI_Tramites_Tareas on sger.id_tramitetarea equals tra.id_tramitetarea into tra_join
                 from tra in tra_join.DefaultIfEmpty()
                 join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                 from trah in trah_join.DefaultIfEmpty()
                 join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                 from tar in tar_join.DefaultIfEmpty()
                 where
                   sger.Observaciones != "" &&
                   tar.id_circuito == IdCircuito &&
                   tra.FechaCierre_tramitetarea >= dtInicio && tra.FechaCierre_tramitetarea <= dtFin
                 select new TramiteDTO
                 {
                     IdSolicitud = trah.id_solicitud,
                     Observacion = sger.Observaciones,
                     Quien = "Subgerente",
                     Fecha = DbFunctions.TruncateTime(tra.FechaCierre_tramitetarea),
                     CircuitoOrigen = trah.SSIT_Solicitudes.circuito_origen
                 }
             ).Union
             (
                 from ger in db.SGI_Tarea_Revision_Gerente
                 join tra in db.SGI_Tramites_Tareas on ger.id_tramitetarea equals tra.id_tramitetarea into tra_join
                 from tra in tra_join.DefaultIfEmpty()
                 join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                 from trah in trah_join.DefaultIfEmpty()
                 join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                 from tar in tar_join.DefaultIfEmpty()                 
                 where
                   ger.Observaciones != "" &&
                   tar.id_circuito == IdCircuito &&
                   tra.FechaCierre_tramitetarea >= dtInicio && tra.FechaCierre_tramitetarea <= dtFin                                      
                 select new TramiteDTO
                 {
                     IdSolicitud = trah.id_solicitud,
                     Observacion = ger.Observaciones,
                     Quien = "Gerente",
                     Fecha = DbFunctions.TruncateTime(tra.FechaCierre_tramitetarea),
                     CircuitoOrigen = trah.SSIT_Solicitudes.circuito_origen
                 } 
                );

            return query.OrderBy(p => p.IdSolicitud);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<TramiteSSPDTO> ObservacionesContribuyenteGerSubCalificador(DateTime dtInicio, DateTime dtFin, int IdCircuito, int[] IdTarea1, int[] IdTarea2, int[] IdTarea3)
        {
            return
            (
                from cal in db.SGI_Tarea_Calificar
                join tra in db.SGI_Tramites_Tareas on cal.id_tramitetarea equals tra.id_tramitetarea into tra_join
                from tra in tra_join.DefaultIfEmpty()
                join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                from trah in trah_join.DefaultIfEmpty()
                join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into res_join
                from res in res_join.DefaultIfEmpty()
                join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                from tar in tar_join.DefaultIfEmpty()
                where
                  IdTarea1.Contains(tra.id_tarea) &&
                  res.id_resultado == 20 &&
                  tar.id_circuito == IdCircuito &&
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
                join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                from trah in trah_join.DefaultIfEmpty()
                join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into res_join
                from res in res_join.DefaultIfEmpty()
                join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                from tar in tar_join.DefaultIfEmpty()
                where
                    IdTarea2.Contains(tra.id_tarea) &&
                    res.id_resultado == 20 &&
                    tar.id_circuito == IdCircuito &&
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
                join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                from trah in trah_join.DefaultIfEmpty()
                join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into res_join
                from res in res_join.DefaultIfEmpty()
                join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                from tar in tar_join.DefaultIfEmpty()
                where
                    IdTarea3.Contains(tra.id_tarea) &&
                    res.id_resultado == 20 &&
                    tar.id_circuito == IdCircuito &&
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
                ).OrderBy(p => p.IdSolicitud);;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<TramiteSSP2DTO> ObservacionesContribuyenteGerSubCalificadorMas300000(DateTime dtInicio, DateTime dtFin, int IdCircuito, int[] IdTarea1, int[] IdTarea2, int[] IdTarea3)
        {
            return
            (
                from cal in db.SGI_Tarea_Calificar
                join tra in db.SGI_Tramites_Tareas on cal.id_tramitetarea equals tra.id_tramitetarea into tra_join
                from tra in tra_join.DefaultIfEmpty()
                join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                from trah in trah_join.DefaultIfEmpty()
                join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into res_join
                from res in res_join.DefaultIfEmpty()
                join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                from tar in tar_join.DefaultIfEmpty()
                join gobs in db.SGI_Tarea_Calificar_ObsGrupo on tra.id_tramitetarea equals gobs.id_tramitetarea into gobs_join
                from gobs in gobs_join.DefaultIfEmpty()
                join dobs in db.SGI_Tarea_Calificar_ObsDocs on gobs.id_ObsGrupo equals dobs.id_ObsGrupo into dobs_join
                from dobs in dobs_join.DefaultIfEmpty()
                where
                  IdTarea1.Contains(tra.id_tarea) &&
                  res.id_resultado == 20 &&
                  tar.id_circuito == IdCircuito &&
                  tra.FechaCierre_tramitetarea >= dtInicio && tra.FechaCierre_tramitetarea <= dtFin
                select new TramiteSSP2DTO
                {
                    IdSolicitud = trah.id_solicitud,
                    id_ObsGrupo = gobs.id_ObsGrupo,
                    Observacion_ObsDocs = dobs.Observacion_ObsDocs,
                    Respaldo_ObsDocs = dobs.Respaldo_ObsDocs,
                    FechaInicio_tramitetarea = tra.FechaInicio_tramitetarea,
                    FechaAsignacion_tramtietarea = tra.FechaAsignacion_tramtietarea,
                    FechaCierre_tramitetarea = tra.FechaCierre_tramitetarea,
                    Quien = "Calificador" ,
                    CircuitoOrigen = trah.SSIT_Solicitudes.circuito_origen
                }
            ).Union
            (
                from revsg in db.SGI_Tarea_Revision_SubGerente
                join tra in db.SGI_Tramites_Tareas on revsg.id_tramitetarea equals tra.id_tramitetarea into tra_join
                from tra in tra_join.DefaultIfEmpty()
                join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                from trah in trah_join.DefaultIfEmpty()
                join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into res_join
                from res in res_join.DefaultIfEmpty()
                join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                from tar in tar_join.DefaultIfEmpty()
                join gobs in db.SGI_Tarea_Calificar_ObsGrupo on tra.id_tramitetarea equals gobs.id_tramitetarea into gobs_join
                from gobs in gobs_join.DefaultIfEmpty()
                join dobs in db.SGI_Tarea_Calificar_ObsDocs on gobs.id_ObsGrupo equals dobs.id_ObsGrupo into dobs_join
                from dobs in dobs_join.DefaultIfEmpty()
                where
                    IdTarea2.Contains(tra.id_tarea) &&
                    res.id_resultado == 20 &&
                    tar.id_circuito == IdCircuito &&
                    tra.FechaCierre_tramitetarea >= dtInicio && tra.FechaCierre_tramitetarea <= dtFin
                select new TramiteSSP2DTO
                {
                    IdSolicitud = trah.id_solicitud,
                    id_ObsGrupo = gobs.id_ObsGrupo,
                    Observacion_ObsDocs = dobs.Observacion_ObsDocs,
                    Respaldo_ObsDocs = dobs.Respaldo_ObsDocs,
                    FechaInicio_tramitetarea = tra.FechaInicio_tramitetarea,
                    FechaAsignacion_tramtietarea = tra.FechaAsignacion_tramtietarea,
                    FechaCierre_tramitetarea = tra.FechaCierre_tramitetarea,
                    Quien = "Subgerente",
                    CircuitoOrigen = trah.SSIT_Solicitudes.circuito_origen
                }
            ).Union
            (
                from revg in db.SGI_Tarea_Revision_Gerente
                join tra in db.SGI_Tramites_Tareas on revg.id_tramitetarea equals tra.id_tramitetarea into tra_join
                from tra in tra_join.DefaultIfEmpty()
                join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                from trah in trah_join.DefaultIfEmpty()
                join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into res_join
                from res in res_join.DefaultIfEmpty()
                join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
                from tar in tar_join.DefaultIfEmpty()
                join gobs in db.SGI_Tarea_Calificar_ObsGrupo on tra.id_tramitetarea equals gobs.id_tramitetarea into gobs_join
                from gobs in gobs_join.DefaultIfEmpty()
                join dobs in db.SGI_Tarea_Calificar_ObsDocs on gobs.id_ObsGrupo equals dobs.id_ObsGrupo into dobs_join
                from dobs in dobs_join.DefaultIfEmpty()
                where
                    IdTarea3.Contains(tra.id_tarea) &&
                    res.id_resultado == 20 &&
                    tar.id_circuito == IdCircuito &&
                    tra.FechaCierre_tramitetarea >= dtInicio && tra.FechaCierre_tramitetarea <= dtFin
                select new TramiteSSP2DTO
                {
                    IdSolicitud = trah.id_solicitud,
                    id_ObsGrupo = gobs.id_ObsGrupo,
                    Observacion_ObsDocs = dobs.Observacion_ObsDocs,
                    Respaldo_ObsDocs = dobs.Respaldo_ObsDocs,
                    FechaInicio_tramitetarea = tra.FechaInicio_tramitetarea,
                    FechaAsignacion_tramtietarea = tra.FechaAsignacion_tramtietarea,
                    FechaCierre_tramitetarea = tra.FechaCierre_tramitetarea,
                    Quien = "Gerente",
                    CircuitoOrigen = trah.SSIT_Solicitudes.circuito_origen
                }
                ).OrderBy(p => p.IdSolicitud);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <param name="IdCircuito"></param>
        /// <param name="IdTarea"></param>
        /// <returns></returns>
        public IQueryable<TareaResultadoDTO> SolicitudesListaTareaResultadosAutomaticas(DateTime dtInicio, DateTime dtFin, int IdCircuito)
        {
            var query = (
            from tra in db.SGI_Tramites_Tareas
            join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea
            join encsol in db.Encomienda_SSIT_Solicitudes on trah.id_solicitud equals encsol.id_solicitud
            join enco in db.Encomienda on encsol.id_encomienda equals enco.id_encomienda            
            join enc in db.Encomienda_DatosLocal on enco.id_encomienda equals enc.id_encomienda
            //join a in (

            //           (
            //           from ssit in db.SSIT_Solicitudes
            //           join enco in db.Encomienda on ssit.id_solicitud equals enco.id_solicitud
            //           join trah in db.SGI_Tramites_Tareas_HAB on ssit.id_solicitud equals trah.id_solicitud
            //           join ttt in db.SGI_Tramites_Tareas on trah.id_tramitetarea equals ttt.id_tramitetarea
            //           join tar in db.ENG_Tareas on ttt.id_tarea equals tar.id_tarea
            //           where enco.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo &&
            //           tar.id_circuito == IdCircuito
            //           orderby enco.id_encomienda descending

            //           group enco by new { enco.id_encomienda } into g
            //           select new
            //           {
            //               id_encomienda = g.Max(p => p.id_encomienda)
            //           })
            //           ) on enco.id_encomienda equals a.id_encomienda into a_join

            join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into res_join
            from res in res_join.DefaultIfEmpty()
            join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tar_join
            from tar in tar_join.DefaultIfEmpty()
            join usu in db.aspnet_Users on new { UserId = tra.UsuarioAsignado_tramitetarea.Value } equals new { UserId = usu.UserId } into usu_join
            from usu in usu_join.DefaultIfEmpty()
            where
              tar.id_circuito == IdCircuito &&
              tra.FechaInicio_tramitetarea >= dtInicio && tra.FechaInicio_tramitetarea <= dtFin
            orderby
              trah.id_solicitud,
              tra.FechaInicio_tramitetarea
            select new TareaResultadoDTO
            {
                Id_solicitud = trah.id_solicitud,
                Nombre_tarea = tar.nombre_tarea,
                Nombre_resultado = res.nombre_resultado,
                Fecha_Inicio = DbFunctions.TruncateTime(tra.FechaInicio_tramitetarea),
                Hora_Inicio = DbFunctions.CreateTime(tra.FechaInicio_tramitetarea.Hour, tra.FechaInicio_tramitetarea.Minute, tra.FechaInicio_tramitetarea.Second),
                Fecha_Asignacion = DbFunctions.TruncateTime(tra.FechaAsignacion_tramtietarea),
                Hora_Asignacion = tra.FechaAsignacion_tramtietarea.HasValue ? DbFunctions.CreateTime(tra.FechaAsignacion_tramtietarea.Value.Hour, tra.FechaAsignacion_tramtietarea.Value.Minute, tra.FechaAsignacion_tramtietarea.Value.Second) : null,
                Fecha_Cierre = DbFunctions.TruncateTime(tra.FechaCierre_tramitetarea),
                Hora_Cierre = tra.FechaCierre_tramitetarea.HasValue ? DbFunctions.CreateTime(tra.FechaCierre_tramitetarea.Value.Hour, tra.FechaCierre_tramitetarea.Value.Minute, tra.FechaCierre_tramitetarea.Value.Second) : null,
                Dif_ini_cierre = DbFunctions.DiffDays(tra.FechaInicio_tramitetarea, tra.FechaCierre_tramitetarea) - (2 * (DbFunctions.DiffDays(tra.FechaInicio_tramitetarea, tra.FechaCierre_tramitetarea) / 7)),
                Dif_asig_cierre = DbFunctions.DiffDays(tra.FechaAsignacion_tramtietarea, tra.FechaCierre_tramitetarea) - (2 * (DbFunctions.DiffDays(tra.FechaAsignacion_tramtietarea, tra.FechaCierre_tramitetarea) / 7)),
                UserName = usu.UserName,
                superficie = enc.superficie_cubierta_dl + enc.superficie_descubierta_dl,
                numero_dispo_GEDO = ""
            });

            return query.OrderBy(p => p.Id_solicitud)
                        .ThenBy(p => p.Fecha_Inicio)
                            .ThenBy(p => p.Hora_Inicio)
                                .ThenBy(p => p.Fecha_Asignacion)
                                    .ThenBy(p => p.Hora_Asignacion)
                                        .ThenBy(p => p.Fecha_Cierre)
                                            .ThenBy(p => p.Hora_Cierre); 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <param name="IdCircuito"></param>
        /// <returns></returns>
        public IQueryable<TareaResultadoDTO> SolicitudesListaTareaResultados(DateTime dtInicio, DateTime dtFin, int IdCircuito, int IdTarea)
        {
            var query = (from tra in db.SGI_Tramites_Tareas
                         join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea
                         join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado
                         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         from trah in trah_join.DefaultIfEmpty()
                         join ssit in db.SSIT_Solicitudes on trah.id_solicitud equals ssit.id_solicitud
                         join encsol in db.Encomienda_SSIT_Solicitudes on ssit.id_solicitud equals encsol.id_solicitud
                         join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda 
                         join daenc in db.Encomienda_DatosLocal on enc.id_encomienda equals daenc.id_encomienda 
                         join usu in db.aspnet_Users on tra.UsuarioAsignado_tramitetarea equals usu.UserId into usu_join
                         from usu in usu_join.DefaultIfEmpty()
                        join firma_dispo in (
                        (
                         from tarea in db.SGI_Tramites_Tareas 
                            join proceso in db.SGI_Tarea_Generar_Expediente_Procesos on tarea.id_tramitetarea equals proceso.id_tramitetarea
                             join firma in db.wsEE_TareasDocumentos on proceso.id_paquete equals firma.id_paquete
                             join trah in db.SGI_Tramites_Tareas_HAB on proceso.id_tramitetarea equals trah.id_tramitetarea into trah_join
                             from trah in trah_join.DefaultIfEmpty()
                         where
                               tarea.id_tarea == IdTarea && firma.firmado_en_SADE
                             group new { trah, tarea , firma } by new
                             {
                                 trah.id_solicitud,
                                 FechaAsignacion_tramtietarea = (DateTime?)tarea.FechaAsignacion_tramtietarea,
                                 firma.numeroGEDO
                             } into g
                             select new
                             {
                                 Id_solicitud = (int?)g.Key.id_solicitud,
                                 FirmaDispo_Asign = (DateTime?)g.Key.FechaAsignacion_tramtietarea,
                                 numeroGEDO = g.Key.numeroGEDO
                             }
                            ).Distinct()) on trah.id_solicitud equals firma_dispo.Id_solicitud into firma_dispo_join
                         from firma_dispo in firma_dispo_join.DefaultIfEmpty()

                         where
                           tar.id_circuito == IdCircuito &&
                           tra.FechaInicio_tramitetarea >= dtInicio && tra.FechaInicio_tramitetarea <= dtFin
                         orderby
                           trah.id_solicitud,
                           tra.FechaInicio_tramitetarea
                         select new TareaResultadoDTO
                         {
                             Id_solicitud = trah.id_solicitud,
                             Nombre_tarea = tar.nombre_tarea,
                             Nombre_resultado = res.nombre_resultado,
                             Fecha_Inicio = DbFunctions.TruncateTime(tra.FechaInicio_tramitetarea),
                             Hora_Inicio = DbFunctions.CreateTime(tra.FechaInicio_tramitetarea.Hour,tra.FechaInicio_tramitetarea.Minute,tra.FechaInicio_tramitetarea.Second),
                             Fecha_Asignacion = DbFunctions.TruncateTime(tra.FechaAsignacion_tramtietarea),
                             Hora_Asignacion = tra.FechaAsignacion_tramtietarea.HasValue ? DbFunctions.CreateTime(  tra.FechaAsignacion_tramtietarea.Value.Hour, tra.FechaAsignacion_tramtietarea.Value.Minute, tra.FechaAsignacion_tramtietarea.Value.Second) : null,
                             Fecha_Cierre = DbFunctions.TruncateTime(tra.FechaCierre_tramitetarea),
                             Hora_Cierre = tra.FechaCierre_tramitetarea.HasValue ? DbFunctions.CreateTime(tra.FechaCierre_tramitetarea.Value.Hour, tra.FechaCierre_tramitetarea.Value.Minute,tra.FechaCierre_tramitetarea.Value.Second) : null,
                             Dif_ini_cierre = DbFunctions.DiffDays(tra.FechaInicio_tramitetarea, tra.FechaCierre_tramitetarea) - (2 * (DbFunctions.DiffDays(tra.FechaInicio_tramitetarea, tra.FechaCierre_tramitetarea) / 7)),
                             Dif_asig_cierre = DbFunctions.DiffDays(tra.FechaAsignacion_tramtietarea, tra.FechaCierre_tramitetarea) - (2 * (DbFunctions.DiffDays(tra.FechaAsignacion_tramtietarea, tra.FechaCierre_tramitetarea) / 7)),                             
                             UserName = usu.UserName,
                             superficie = daenc.superficie_cubierta_dl + daenc.superficie_descubierta_dl,
                             numero_dispo_GEDO = tra.id_tarea == IdTarea ? firma_dispo.numeroGEDO : string.Empty
                          });

            return query.OrderBy(p => p.Id_solicitud)
                        .ThenBy(p => p.Fecha_Inicio)
                            .ThenBy(p => p.Hora_Inicio)
                                .ThenBy(p => p.Fecha_Asignacion)
                                    .ThenBy(p => p.Hora_Asignacion)
                                        .ThenBy(p => p.Fecha_Cierre)
                                            .ThenBy(p => p.Hora_Cierre);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <param name="IdCircuito"></param>
        /// <returns></returns>
        public IQueryable<TareaResultadoDTO2> SolicitudesListaTareaResultados300000(DateTime dtInicio, DateTime dtFin, int IdCircuito)
        {
            int codRevD2 = Convert.ToInt32(IdCircuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_DGHyP2);
            int codRevF2 = Convert.ToInt32(IdCircuito.ToString() + Constants.ENG_Tipos_Tareas.Revision_Firma_Disposicion2);

            var query = (from tra in db.SGI_Tramites_Tareas
                         join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea
                         join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado
                         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         from trah in trah_join.DefaultIfEmpty()
                         join ssit in db.SSIT_Solicitudes on trah.id_solicitud equals ssit.id_solicitud
                         join encsol in db.Encomienda_SSIT_Solicitudes on ssit.id_solicitud equals encsol.id_solicitud
                         join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                         join daenc in db.Encomienda_DatosLocal on enc.id_encomienda equals daenc.id_encomienda
                         join usu in db.aspnet_Users on tra.UsuarioAsignado_tramitetarea equals usu.UserId into usu_join
                         from usu in usu_join.DefaultIfEmpty()

                         where
                           tar.id_circuito == IdCircuito &&
                           tra.FechaInicio_tramitetarea >= dtInicio && tra.FechaInicio_tramitetarea <= dtFin
                         orderby
                           trah.id_solicitud,
                           tra.FechaInicio_tramitetarea
                         select new TareaResultadoDTO2
                         {
                             Id_solicitud = trah.id_solicitud,
                             Nombre_tarea = tar.nombre_tarea + (tar.cod_tarea == codRevD2 || tar.cod_tarea == codRevF2 ? " 2" : ""),
                             Nombre_resultado = res.nombre_resultado,
                             Fecha_Inicio = DbFunctions.TruncateTime(tra.FechaInicio_tramitetarea),
                             Hora_Inicio = DbFunctions.CreateTime(tra.FechaInicio_tramitetarea.Hour, tra.FechaInicio_tramitetarea.Minute, tra.FechaInicio_tramitetarea.Second),
                             Fecha_Asignacion = DbFunctions.TruncateTime(tra.FechaAsignacion_tramtietarea),
                             Hora_Asignacion = tra.FechaAsignacion_tramtietarea.HasValue ? DbFunctions.CreateTime(tra.FechaAsignacion_tramtietarea.Value.Hour, tra.FechaAsignacion_tramtietarea.Value.Minute, tra.FechaAsignacion_tramtietarea.Value.Second) : null,
                             Fecha_Cierre = DbFunctions.TruncateTime(tra.FechaCierre_tramitetarea),
                             Hora_Cierre = tra.FechaCierre_tramitetarea.HasValue ? DbFunctions.CreateTime(tra.FechaCierre_tramitetarea.Value.Hour, tra.FechaCierre_tramitetarea.Value.Minute, tra.FechaCierre_tramitetarea.Value.Second) : null,
                             Dif_ini_cierre = DbFunctions.DiffDays(tra.FechaInicio_tramitetarea, tra.FechaCierre_tramitetarea) - (2 * (DbFunctions.DiffDays(tra.FechaInicio_tramitetarea, tra.FechaCierre_tramitetarea) / 7)),
                             Dif_asig_cierre = DbFunctions.DiffDays(tra.FechaAsignacion_tramtietarea, tra.FechaCierre_tramitetarea) - (2 * (DbFunctions.DiffDays(tra.FechaAsignacion_tramtietarea, tra.FechaCierre_tramitetarea) / 7)),
                             UserName = usu.UserName,
                             superficie = daenc.superficie_cubierta_dl + daenc.superficie_descubierta_dl,
                             NroExpedienteSade = ssit.NroExpedienteSade,
                             CircuitoOrigen = trah.SSIT_Solicitudes.circuito_origen
                         }).OrderBy(p => p.Id_solicitud)
                                 .ThenBy(p => p.Fecha_Inicio)
                                    .ThenBy(p => p.Hora_Inicio)
                                        .ThenBy(p => p.Fecha_Asignacion)
                                            .ThenBy(p => p.Hora_Asignacion)
                                                .ThenBy(p => p.Fecha_Cierre)
                                                    .ThenBy(p => p.Hora_Cierre);
                         

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <param name="IdCircuito"></param>
        /// <returns></returns>
        public IQueryable<ListadoVidaHabilitacion> SolicitudesFechayCircuitoAutomaticas(DateTime dtInicio, DateTime dtFin, int IdCircuito)
        {
            var query = (from trah in db.SGI_Tramites_Tareas_HAB
                         join ssit in db.SSIT_Solicitudes on trah.id_solicitud equals ssit.id_solicitud
                         join encsol in db.Encomienda_SSIT_Solicitudes on ssit.id_solicitud equals encsol.id_solicitud
                         join enco in db.Encomienda on encsol.id_encomienda equals enco.id_encomienda
                         join enc in db.Encomienda_DatosLocal on enco.id_encomienda equals enc.id_encomienda
                         join ttt in db.SGI_Tramites_Tareas on trah.id_tramitetarea equals  ttt.id_tramitetarea
                         join tar in db.ENG_Tareas on ttt.id_tarea equals tar.id_tarea

                         join a in (
                         
                         (
                         from ssit in db.SSIT_Solicitudes
                         join encsol in db.Encomienda_SSIT_Solicitudes on ssit.id_solicitud equals encsol.id_solicitud
                         join  enco in db.Encomienda on encsol.id_encomienda equals enco.id_encomienda
                         join trah in db.SGI_Tramites_Tareas_HAB on ssit.id_solicitud equals trah.id_solicitud
                         join ttt in db.SGI_Tramites_Tareas on trah.id_tramitetarea equals ttt.id_tramitetarea
                         join tar in db.ENG_Tareas on ttt.id_tarea equals tar.id_tarea
                         where enco.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo &&
                         tar.id_circuito == IdCircuito
                         orderby enco.id_encomienda descending

                         group enco by new { enco.id_encomienda } into g
                         select new
                         {
                             id_encomienda = g.Max(p => p.id_encomienda)
                         })
                         ) on enco.id_encomienda equals a.id_encomienda into a_join
                            
                         join b in (
                             (from tra0 in db.SGI_Tramites_Tareas
                              join trah1 in db.SGI_Tramites_Tareas_HAB on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
                              from trah1 in trah1_join.DefaultIfEmpty()
                              where
                    tra0.id_tarea == 701
                              select new
                              {
                                  id_solicitud = (System.Int32?)trah1.id_solicitud,
                                  Exp_ini = tra0.FechaInicio_tramitetarea,
                                  Exp_Cierre = tra0.FechaCierre_tramitetarea,
                                  Exp_Asignacion = tra0.FechaAsignacion_tramtietarea
                              })) on new { id_solicitud = enco.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() } equals new { id_solicitud = (int)(b.id_solicitud) } into b_join
                         from b in b_join.DefaultIfEmpty()
                         join h in (
                             (from tra0 in db.SGI_Tramites_Tareas
                              join trah1 in db.SGI_Tramites_Tareas_HAB on tra0.id_tramitetarea equals trah1.id_tramitetarea into trah1_join
                              from trah1 in trah1_join.DefaultIfEmpty()
                              where
                    tra0.id_tarea == 703
                              select new
                              {
                                  id_solicitud = (System.Int32?)trah1.id_solicitud,
                                  Fecha_Ini_Entrega = tra0.FechaInicio_tramitetarea,
                                  Fecha_Cierre_Entrega = tra0.FechaCierre_tramitetarea
                              })) on new { id_solicitud = enco.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() } equals new { id_solicitud = (int)(h.id_solicitud) } into h_join
                         from h in h_join.DefaultIfEmpty()
                         where
                           tar.id_circuito == IdCircuito
                         group new { trah.SSIT_Solicitudes, b, h, enc } by new
                         {
                             id_solicitud = (System.Int32?)enco.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault(),
                             b.Exp_ini,
                             Column1 = b.Exp_ini,
                             b.Exp_Cierre,
                             Column2 = b.Exp_Cierre,
                             b.Exp_Asignacion,
                             Column3 = b.Exp_Asignacion,
                             h.Fecha_Cierre_Entrega,
                             h.Fecha_Ini_Entrega,
                             enc.superficie_cubierta_dl,
                             enc.superficie_descubierta_dl
                         } into g
                         orderby
                           g.Key.id_solicitud
                         select new ListadoVidaHabilitacion
                         {
                             Id_solicitud = g.Key.id_solicitud,
                             superficie = (g.Key.superficie_cubierta_dl + g.Key.superficie_descubierta_dl),
                             Fecha_inicio_ASIGNACION_calif = null,
                             Hora_inicio_ASIGNACION_calif = null,
                             Fecha_fin_calif = null,
                             Hora_fin_calif = null,
                             Fecha_Inicio_Exp = DbFunctions.TruncateTime(g.Key.Exp_ini),
                             Hora_Inicio_Exp = DbFunctions.CreateTime(g.Key.Exp_ini.Hour, g.Key.Exp_ini.Minute, g.Key.Exp_ini.Second),
                             Fecha_Asignacion_Exp = DbFunctions.TruncateTime(g.Key.Exp_Asignacion),
                             Hora_Asignacion_Exp = DbFunctions.CreateTime(g.Key.Exp_Asignacion.Value.Hour, g.Key.Exp_Asignacion.Value.Minute, g.Key.Exp_Asignacion.Value.Second),
                             Fecha_Cierre_Exp = DbFunctions.TruncateTime(g.Key.Exp_Cierre),
                             Hora_Cierre_Exp = DbFunctions.CreateTime(g.Key.Exp_Cierre.Value.Hour, g.Key.Exp_Cierre.Value.Minute, g.Key.Exp_Cierre.Value.Second),
                             Fecha_Inicio_Rev_Ger = null,
                             Hora_Inicio_Rev_Ger = null,
                             Fecha_Cierre_Rev_Ger = null,
                             Hora_Cierre_Rev_Ger = null,
                             Fecha_Gen_BU_ini = null,
                             Fecha_Gen_BU_cierre = null,
                             Fecha_Rev_BU_ini = null,
                             Fecha_Rev_BU_cierre = null,
                             observado_alguna_vez = null,
                             Fecha_Cierre_Revision = null,
                             Fecha_Asign_Revision = null,
                             Fecha_Ini_Revision = null,
                             Fecha_Cierre_Entrega = (System.DateTime?)g.Key.Fecha_Cierre_Entrega,
                             Fecha_Ini_Entrega = (System.DateTime?)g.Key.Fecha_Ini_Entrega,
                             FirmaRDGHP_Inicio = null,
                             FirmaRDGHP_Cierre = null,
                             numeroGEDO = ""
                         });

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ListadoVidaHabilitacion> SolicitudesFechayCircuito(DateTime dtInicio, DateTime dtFin, int IdCircuito)
        {
            var query = (from a in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea
                                  join ssit in db.SSIT_Solicitudes on trah.id_solicitud equals ssit.id_solicitud
                                  join encsol in db.Encomienda_SSIT_Solicitudes on ssit.id_solicitud equals encsol.id_solicitud
                                  join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                                  join daenc in db.Encomienda_DatosLocal on enc.id_encomienda equals daenc.id_encomienda 
                         
                                  where
                                    tra.id_tarea == 9
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Calif_Ini = tra.FechaInicio_tramitetarea,
                                      Calif_fin = tra.FechaCierre_tramitetarea,
                                      superficie = (System.Decimal?)(daenc.superficie_cubierta_dl + daenc.superficie_descubierta_dl)
                                  }))
                         join b in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 22
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Exp_ini = tra.FechaInicio_tramitetarea,
                                      Exp_Cierre = tra.FechaCierre_tramitetarea,
                                      Exp_Asignacion = tra.FechaAsignacion_tramtietarea
                                  })) on a.Id_solicitud equals b.Id_solicitud into b_join
                         from b in b_join.DefaultIfEmpty()
                         join c in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 12
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Rev_Ger_Cierre = tra.FechaCierre_tramitetarea,
                                      Rev_Ger_ini = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals c.Id_solicitud into c_join
                         from c in c_join.DefaultIfEmpty()
                         join d in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 26
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      BU_ini = tra.FechaInicio_tramitetarea,
                                      BU_Cierre = tra.FechaCierre_tramitetarea
                                  })) on a.Id_solicitud equals d.Id_solicitud into d_join
                         from d in d_join.DefaultIfEmpty()
                         join e in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 21
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      BU_Cierre = tra.FechaCierre_tramitetarea,
                                      BU_ini = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals e.Id_solicitud into e_join
                         from e in e_join.DefaultIfEmpty()
                         join f in
                             (
                                 ((from tra in db.SGI_Tramites_Tareas
                                   join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                   from trah in trah_join.DefaultIfEmpty()
                                   where
                                     tra.id_tarea == 25
                                   select new
                                   {
                                       Id_solicitud = (System.Int32?)trah.id_solicitud,
                                       Cor_Solicitud = tra.FechaInicio_tramitetarea
                                   }).Distinct())) on a.Id_solicitud equals f.Id_solicitud into f_join
                         from f in f_join.DefaultIfEmpty()
                         join g in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 27
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      FirmaDispo_Ini = tra.FechaInicio_tramitetarea,
                                      FirmaDispo_Cierre = tra.FechaCierre_tramitetarea,
                                      FirmaDispo_Asign = tra.FechaAsignacion_tramtietarea
                                  })) on a.Id_solicitud equals g.Id_solicitud into g_join
                         from g in g_join.DefaultIfEmpty()
                         join h in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 23
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Fecha_Ini_Entrega = tra.FechaInicio_tramitetarea,
                                      Fecha_Cierre_Entrega = tra.FechaCierre_tramitetarea
                                  })) on a.Id_solicitud equals h.Id_solicitud into h_join
                         from h in h_join.DefaultIfEmpty()
                         join i in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 14
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      FirmaRDGHP_Cierre = tra.FechaCierre_tramitetarea,
                                      FirmaRDGHP_Inicio = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals i.Id_solicitud into i_join
                         from i in i_join.DefaultIfEmpty()

                             join firma_dispo in 
                        (
                         from tarea in db.SGI_Tramites_Tareas 
                            join proceso in db.SGI_Tarea_Generar_Expediente_Procesos on tarea.id_tramitetarea equals proceso.id_tramitetarea
                             join firma in db.wsEE_TareasDocumentos on proceso.id_paquete equals firma.id_paquete
                             join trah in db.SGI_Tramites_Tareas_HAB on proceso.id_tramitetarea equals trah.id_tramitetarea into trah_join
                             from trah in trah_join.DefaultIfEmpty()
                         where
                               tarea.id_tarea == 27 && firma.firmado_en_SADE
                             group new { trah, tarea , firma } by new
                             {
                                 trah.id_solicitud,
                                 firma.numeroGEDO
                             } into g
                             select new
                             {
                                 Id_solicitud = (int?)g.Key.id_solicitud,
                                 numeroGEDO = g.Key.numeroGEDO
                             }
                            )

                            on a.Id_solicitud equals firma_dispo.Id_solicitud into firma_dispo_join
                         from firma_dispo in firma_dispo_join.DefaultIfEmpty()

                         from tra in db.SGI_Tramites_Tareas
                         join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea
                         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         from trah in trah_join.DefaultIfEmpty()
                         

                         where
                           a.Calif_Ini != null &&
                           a.Calif_Ini >= dtInicio && a.Calif_Ini <= dtFin  &&
                           tar.id_circuito == IdCircuito
                         group new { a, b, c, d, e, f, g, h, i , firma_dispo } by new
                         {
                             a.Id_solicitud,
                             a.Calif_Ini,
                             Column1 = a.Calif_Ini,
                             a.Calif_fin,
                             Column2 = a.Calif_fin,
                             a.superficie,
                             b.Exp_ini,
                             Column3 = b.Exp_ini,
                             b.Exp_Cierre,
                             Column4 = b.Exp_Cierre,
                             b.Exp_Asignacion,
                             Column5 = b.Exp_Asignacion,
                             c.Rev_Ger_Cierre,
                             Column6 = c.Rev_Ger_Cierre,
                             c.Rev_Ger_ini,
                             Column7 = c.Rev_Ger_ini,
                             d.BU_ini,
                             d.BU_Cierre,
                             Column8 = e.BU_ini,
                             Column9 = e.BU_Cierre,
                             Column10 = a.Calif_Ini,
                             Column11 = a.Calif_Ini,
                             f.Cor_Solicitud,
                             g.FirmaDispo_Cierre,
                             g.FirmaDispo_Asign,
                             g.FirmaDispo_Ini,
                             h.Fecha_Cierre_Entrega,
                             h.Fecha_Ini_Entrega,
                             i.FirmaRDGHP_Inicio,
                             i.FirmaRDGHP_Cierre,
                             firma_dispo.numeroGEDO
                         } into g
                         orderby 
                            g.Key.Id_solicitud,
                            g.Key.Calif_Ini,
                            g.Key.Exp_Asignacion,
                            g.Key.Exp_Cierre                            
                         select new ListadoVidaHabilitacion
                         {
                             Id_solicitud = (System.Int32?)g.Key.Id_solicitud,
                             superficie = (System.Decimal?)g.Key.superficie,
                             Fecha_inicio_ASIGNACION_calif = DbFunctions.TruncateTime(g.Key.Calif_Ini),
                             Hora_inicio_ASIGNACION_calif = DbFunctions.CreateTime(g.Key.Calif_Ini.Hour,g.Key.Calif_Ini.Minute, g.Key.Calif_Ini.Second),
                             Fecha_fin_calif = DbFunctions.TruncateTime(g.Key.Calif_fin),
                             Hora_fin_calif = DbFunctions.CreateTime(g.Key.Calif_fin.Value.Hour, g.Key.Calif_fin.Value.Minute, g.Key.Calif_fin.Value.Second),
                             Fecha_Inicio_Exp = DbFunctions.TruncateTime(g.Key.Exp_ini),
                             Hora_Inicio_Exp = DbFunctions.CreateTime(g.Key.Exp_ini.Hour, g.Key.Exp_ini.Minute, g.Key.Exp_ini.Second),                             
                             Fecha_Asignacion_Exp = DbFunctions.TruncateTime(g.Key.Exp_Asignacion),
                             Hora_Asignacion_Exp = g.Key.Exp_Asignacion.HasValue ? DbFunctions.CreateTime(g.Key.Exp_Asignacion.Value.Hour,g.Key.Exp_Asignacion.Value.Minute,g.Key.Exp_Asignacion.Value.Second) : null,
                             Fecha_Cierre_Exp = DbFunctions.TruncateTime(g.Key.Exp_Cierre),
                             Hora_Cierre_Exp = g.Key.Exp_Cierre.HasValue ? DbFunctions.CreateTime(g.Key.Exp_Cierre.Value.Hour,g.Key.Exp_Cierre.Value.Minute,g.Key.Exp_Cierre.Value.Second) : null,
                             Fecha_Inicio_Rev_Ger = DbFunctions.TruncateTime(g.Key.Rev_Ger_ini),                             
                             Hora_Inicio_Rev_Ger = DbFunctions.CreateTime(g.Key.Rev_Ger_ini.Hour, g.Key.Rev_Ger_ini.Minute, g.Key.Rev_Ger_ini.Second),
                             Fecha_Cierre_Rev_Ger = DbFunctions.TruncateTime(g.Key.Rev_Ger_Cierre),
                             Hora_Cierre_Rev_Ger = g.Key.Rev_Ger_Cierre.HasValue ? DbFunctions.CreateTime(g.Key.Rev_Ger_Cierre.Value.Hour, g.Key.Rev_Ger_Cierre.Value.Minute, g.Key.Rev_Ger_Cierre.Value.Second) : null,
                             Fecha_Gen_BU_ini = DbFunctions.TruncateTime(g.Key.BU_ini),
                             Fecha_Gen_BU_cierre = DbFunctions.TruncateTime(g.Key.BU_Cierre),
                             Fecha_Rev_BU_ini = DbFunctions.TruncateTime(g.Key.Column8),
                             Fecha_Rev_BU_cierre = DbFunctions.TruncateTime(g.Key.Column9),
                             observado_alguna_vez = DbFunctions.TruncateTime(g.Key.Cor_Solicitud),                             
                             Fecha_Cierre_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Cierre),
                             Fecha_Asign_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Asign),
                             Fecha_Ini_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Ini),
                             Fecha_Cierre_Entrega = DbFunctions.TruncateTime(g.Key.Fecha_Cierre_Entrega),
                             Fecha_Ini_Entrega = DbFunctions.TruncateTime(g.Key.Fecha_Ini_Entrega),                             
                             FirmaRDGHP_Inicio = DbFunctions.TruncateTime(g.Key.FirmaRDGHP_Inicio),
                             FirmaRDGHP_Cierre = DbFunctions.TruncateTime(g.Key.FirmaRDGHP_Cierre),
                             numeroGEDO = g.Key.numeroGEDO
                         });

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <param name="IdCircuito"></param>
        /// <returns></returns>
        public IQueryable<ListadoVidaHabilitacion> SolicitudesFechayCircuitoSCP(DateTime dtInicio, DateTime dtFin, int IdCircuito)
        {
            var query = (from a in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea
                                  join ssit in db.SSIT_Solicitudes on trah.id_solicitud equals ssit.id_solicitud
                                  join encsol in db.Encomienda_SSIT_Solicitudes on ssit.id_solicitud equals encsol.id_solicitud
                                  join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                                  join daenc in db.Encomienda_DatosLocal on enc.id_encomienda equals daenc.id_encomienda

                                  where
                                    tra.id_tarea == 35
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Calif_Ini = tra.FechaInicio_tramitetarea,
                                      Calif_fin = tra.FechaCierre_tramitetarea,
                                      superficie = (System.Decimal?)(daenc.superficie_cubierta_dl + daenc.superficie_descubierta_dl)
                                  }))
                         join b in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 34
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Exp_ini = tra.FechaInicio_tramitetarea,
                                      Exp_Cierre = tra.FechaCierre_tramitetarea,
                                      Exp_Asignacion = tra.FechaAsignacion_tramtietarea
                                  })) on a.Id_solicitud equals b.Id_solicitud into b_join
                         from b in b_join.DefaultIfEmpty()
                         join c in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 46
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Rev_Ger_Cierre = tra.FechaCierre_tramitetarea,
                                      Rev_Ger_ini = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals c.Id_solicitud into c_join
                         from c in c_join.DefaultIfEmpty()
                         join d in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 37
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      BU_ini = tra.FechaInicio_tramitetarea,
                                      BU_Cierre = tra.FechaCierre_tramitetarea
                                  })) on a.Id_solicitud equals d.Id_solicitud into d_join
                         from d in d_join.DefaultIfEmpty()
                         join e in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 50
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      BU_Cierre = tra.FechaCierre_tramitetarea,
                                      BU_ini = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals e.Id_solicitud into e_join
                         from e in e_join.DefaultIfEmpty()
                         join f in
                             (
                                 ((from tra in db.SGI_Tramites_Tareas
                                   join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                   from trah in trah_join.DefaultIfEmpty()
                                   where
                                     tra.id_tarea == 49
                                   select new
                                   {
                                       Id_solicitud = (System.Int32?)trah.id_solicitud,
                                       Cor_Solicitud = tra.FechaInicio_tramitetarea
                                   }).Distinct())) on a.Id_solicitud equals f.Id_solicitud into f_join
                         from f in f_join.DefaultIfEmpty()
                         join g in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 51
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      FirmaDispo_Ini = tra.FechaInicio_tramitetarea,
                                      FirmaDispo_Cierre = tra.FechaCierre_tramitetarea,
                                      FirmaDispo_Asign = tra.FechaAsignacion_tramtietarea
                                  })) on a.Id_solicitud equals g.Id_solicitud into g_join
                         from g in g_join.DefaultIfEmpty()
                         join h in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 47
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Fecha_Ini_Entrega = tra.FechaInicio_tramitetarea,
                                      Fecha_Cierre_Entrega = tra.FechaCierre_tramitetarea
                                  })) on a.Id_solicitud equals h.Id_solicitud into h_join
                         from h in h_join.DefaultIfEmpty()
                         join i in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 39
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      FirmaRDGHP_Cierre = tra.FechaCierre_tramitetarea,
                                      FirmaRDGHP_Inicio = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals i.Id_solicitud into i_join
                         from i in i_join.DefaultIfEmpty()

                         join firma_dispo in
                             (
                              from tarea in db.SGI_Tramites_Tareas
                              join proceso in db.SGI_Tarea_Generar_Expediente_Procesos on tarea.id_tramitetarea equals proceso.id_tramitetarea
                              join firma in db.wsEE_TareasDocumentos on proceso.id_paquete equals firma.id_paquete
                              join trah in db.SGI_Tramites_Tareas_HAB on proceso.id_tramitetarea equals trah.id_tramitetarea into trah_join
                              from trah in trah_join.DefaultIfEmpty()
                              where
                                    tarea.id_tarea == 51 && firma.firmado_en_SADE
                              group new { trah, tarea, firma } by new
                              {
                                  trah.id_solicitud,
                                  firma.numeroGEDO
                              } into g
                              select new
                              {
                                  Id_solicitud = (int?)g.Key.id_solicitud,
                                  numeroGEDO = g.Key.numeroGEDO
                              }
                                 )

                        on a.Id_solicitud equals firma_dispo.Id_solicitud into firma_dispo_join
                         from firma_dispo in firma_dispo_join.DefaultIfEmpty()

                         from tra in db.SGI_Tramites_Tareas
                         join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea
                         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         from trah in trah_join.DefaultIfEmpty()


                         where
                           a.Calif_Ini != null &&
                           a.Calif_Ini >= dtInicio && a.Calif_Ini <= dtFin &&
                           tar.id_circuito == IdCircuito
                         group new { a, b, c, d, e, f, g, h, i, firma_dispo } by new
                         {
                             a.Id_solicitud,
                             a.Calif_Ini,
                             Column1 = a.Calif_Ini,
                             a.Calif_fin,
                             Column2 = a.Calif_fin,
                             a.superficie,
                             b.Exp_ini,
                             Column3 = b.Exp_ini,
                             b.Exp_Cierre,
                             Column4 = b.Exp_Cierre,
                             b.Exp_Asignacion,
                             Column5 = b.Exp_Asignacion,
                             c.Rev_Ger_Cierre,
                             Column6 = c.Rev_Ger_Cierre,
                             c.Rev_Ger_ini,
                             Column7 = c.Rev_Ger_ini,
                             d.BU_ini,
                             d.BU_Cierre,
                             Column8 = e.BU_ini,
                             Column9 = e.BU_Cierre,
                             Column10 = a.Calif_Ini,
                             Column11 = a.Calif_Ini,
                             f.Cor_Solicitud,
                             g.FirmaDispo_Cierre,
                             g.FirmaDispo_Asign,
                             g.FirmaDispo_Ini,
                             h.Fecha_Cierre_Entrega,
                             h.Fecha_Ini_Entrega,
                             i.FirmaRDGHP_Inicio,
                             i.FirmaRDGHP_Cierre,
                             firma_dispo.numeroGEDO
                         } into g
                         orderby
                           g.Key.Id_solicitud
                         select new ListadoVidaHabilitacion
                         {
                             Id_solicitud = (System.Int32?)g.Key.Id_solicitud,
                             superficie = (System.Decimal?)g.Key.superficie,
                             Fecha_inicio_ASIGNACION_calif = DbFunctions.TruncateTime(g.Key.Calif_Ini),
                             Hora_inicio_ASIGNACION_calif = DbFunctions.CreateTime(g.Key.Calif_Ini.Hour, g.Key.Calif_Ini.Minute, g.Key.Calif_Ini.Second),
                             Fecha_fin_calif = DbFunctions.TruncateTime(g.Key.Calif_fin),
                             Hora_fin_calif = DbFunctions.CreateTime(g.Key.Calif_fin.Value.Hour, g.Key.Calif_fin.Value.Minute, g.Key.Calif_fin.Value.Second),
                             Fecha_Inicio_Exp = DbFunctions.TruncateTime(g.Key.Exp_ini),
                             Hora_Inicio_Exp = DbFunctions.CreateTime(g.Key.Exp_ini.Hour, g.Key.Exp_ini.Minute, g.Key.Exp_ini.Second),
                             Fecha_Asignacion_Exp = DbFunctions.TruncateTime(g.Key.Exp_Asignacion),
                             Hora_Asignacion_Exp = g.Key.Exp_Asignacion.HasValue ? DbFunctions.CreateTime(g.Key.Exp_Asignacion.Value.Hour, g.Key.Exp_Asignacion.Value.Minute, g.Key.Exp_Asignacion.Value.Second) : null,
                             Fecha_Cierre_Exp = DbFunctions.TruncateTime(g.Key.Exp_Cierre),
                             Hora_Cierre_Exp = g.Key.Exp_Cierre.HasValue ? DbFunctions.CreateTime(g.Key.Exp_Cierre.Value.Hour, g.Key.Exp_Cierre.Value.Minute, g.Key.Exp_Cierre.Value.Second) : null,
                             Fecha_Inicio_Rev_Ger = DbFunctions.TruncateTime(g.Key.Rev_Ger_ini),
                             Hora_Inicio_Rev_Ger = DbFunctions.CreateTime(g.Key.Rev_Ger_ini.Hour, g.Key.Rev_Ger_ini.Minute, g.Key.Rev_Ger_ini.Second),
                             Fecha_Cierre_Rev_Ger = DbFunctions.TruncateTime(g.Key.Rev_Ger_Cierre),
                             Hora_Cierre_Rev_Ger = g.Key.Rev_Ger_Cierre.HasValue ? DbFunctions.CreateTime(g.Key.Rev_Ger_Cierre.Value.Hour, g.Key.Rev_Ger_Cierre.Value.Minute, g.Key.Rev_Ger_Cierre.Value.Second) : null,
                             Fecha_Gen_BU_ini = DbFunctions.TruncateTime(g.Key.BU_ini),
                             Fecha_Gen_BU_cierre = DbFunctions.TruncateTime(g.Key.BU_Cierre),
                             Fecha_Rev_BU_ini = DbFunctions.TruncateTime(g.Key.Column8),
                             Fecha_Rev_BU_cierre = DbFunctions.TruncateTime(g.Key.Column9),
                             observado_alguna_vez = DbFunctions.TruncateTime(g.Key.Cor_Solicitud),
                             Fecha_Cierre_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Cierre),
                             Fecha_Asign_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Asign),
                             Fecha_Ini_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Ini),
                             Fecha_Cierre_Entrega = DbFunctions.TruncateTime(g.Key.Fecha_Cierre_Entrega),
                             Fecha_Ini_Entrega = DbFunctions.TruncateTime(g.Key.Fecha_Ini_Entrega),
                             FirmaRDGHP_Inicio = DbFunctions.TruncateTime(g.Key.FirmaRDGHP_Inicio),
                             FirmaRDGHP_Cierre = DbFunctions.TruncateTime(g.Key.FirmaRDGHP_Cierre),
                             numeroGEDO = g.Key.numeroGEDO
                         });

            return query;
        }        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<Especial> SolicitudesFechayCircuitoEspecial()
        {
            var query = (from hoja3 in
                             (
                                 (from tth in db.SGI_Tramites_Tareas_HAB
                                  join encsol in db.Encomienda_SSIT_Solicitudes on tth.id_solicitud equals encsol.id_solicitud
                                  join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                                  where
                                    tth.SGI_Tramites_Tareas.ENG_Tareas.id_circuito == 3
                                  group tth.SSIT_Solicitudes by new
                                  {
                                      Id_solicitud = (System.Int32?)tth.SSIT_Solicitudes.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.Id_solicitud
                                  }))
                         join a in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 101
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_asig_calif = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      fecha_asig_asig_calif = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_asig_calif = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals a.Id_solicitud into a_join
                         from a in a_join.DefaultIfEmpty()
                         join b in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 102
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_visar1 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_visar1 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_visar1 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals b.Id_solicitud into b_join
                         from b in b_join.DefaultIfEmpty()
                         join c in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 103
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_verif_avh = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_verif_avh = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_verif_avh = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals c.Id_solicitud into c_join
                         from c in c_join.DefaultIfEmpty()
                         join d in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 104
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rev_sgo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rev_sgo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rev_sgo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals d.Id_solicitud into d_join
                         from d in d_join.DefaultIfEmpty()
                         join e in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 104
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rev_go_1 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rev_go_1 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rev_go_1 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals e.Id_solicitud into e_join
                         from e in e_join.DefaultIfEmpty()
                         join f in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 106
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_dict_asig_prof = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_dict_asig_prof = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_fin_dict_asig_prof = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals f.Id_solicitud into f_join
                         from f in f_join.DefaultIfEmpty()
                         join g in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 107
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_dict_rev_tram = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_dict_rev_tram = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_dict_rev_tram = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals g.Id_solicitud into g_join
                         from g in g_join.DefaultIfEmpty()
                         join h in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 108
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_dict_rev_sgo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_dict_rev_sgo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_dict_rev_sgo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals h.Id_solicitud into h_join
                         from h in h_join.DefaultIfEmpty()
                         join i in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 109
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_dict_rev_go = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_dict_rev_go = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_dict_rev_go = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals i.Id_solicitud into i_join
                         from i in i_join.DefaultIfEmpty()
                         join j in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 110
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_dict_gedo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_dict_gedo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_dict_gedo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals j.Id_solicitud into j_join
                         from j in j_join.DefaultIfEmpty()
                         join k in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 111
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      FechaInicio_tramitetarea = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_gen_bol = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_gen_bol = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals k.Id_solicitud into k_join
                         from k in k_join.DefaultIfEmpty()
                         join l in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 112
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rev_pag = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rev_pag = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rev_pag = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals l.Id_solicitud into l_join
                         from l in l_join.DefaultIfEmpty()
                         join m in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 113
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_gen_exp = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_gen_exp = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_gen_exp = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals m.Id_solicitud into m_join
                         from m in m_join.DefaultIfEmpty()
                         join n in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 114
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rev_dghp = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rev_dghp = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rev_dghp = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals n.Id_solicitud into n_join
                         from n in n_join.DefaultIfEmpty()
                         join o in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 115
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rev_fir_dispo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rev_fir_dispo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rev_fir_dispo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals o.Id_solicitud into o_join
                         from o in o_join.DefaultIfEmpty()
                         join p in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 116
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_aprobados = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_aprobados = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_aprobados = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals p.Id_solicitud into p_join
                         from p in p_join.DefaultIfEmpty()
                         join q in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 117
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_ent_tra = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_ent_tra = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_ent_tra = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals q.Id_solicitud into q_join
                         from q in q_join.DefaultIfEmpty()
                         join r in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 118
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rechazo_sade = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rechazo_sade = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rechazo_sade = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals r.Id_solicitud into r_join
                         from r in r_join.DefaultIfEmpty()
                         join s in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 119
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_fin_tra = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_fin_tra = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_fin_tra = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals s.Id_solicitud into s_join
                         from s in s_join.DefaultIfEmpty()
                         join t in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 121
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_visar2 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_visar2 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_visar2 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals t.Id_solicitud into t_join
                         from t in t_join.DefaultIfEmpty()
                         join u in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 122
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rev_go_2 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rev_go_2 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rev_go_2 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals u.Id_solicitud into u_join
                         from u in u_join.DefaultIfEmpty()

                         orderby hoja3.Id_solicitud
                         select new Especial
                         {
                             Id_solicitud = (System.Int32?)hoja3.Id_solicitud,
                             Fecha_inicio_asig_calif = DbFunctions.TruncateTime(a.Fecha_inicio_asig_calif),
                             Hora_inicio_asig_calif = a.Fecha_inicio_asig_calif.HasValue ?  DbFunctions.CreateTime(a.Fecha_inicio_asig_calif.Value.Hour, a.Fecha_inicio_asig_calif.Value.Minute, a.Fecha_inicio_asig_calif.Value.Second) : null,
                             Fecha_asig_asig_calif = DbFunctions.TruncateTime(a.fecha_asig_asig_calif),
                             Hora_asig_asig_calif = DbFunctions.CreateTime(a.fecha_asig_asig_calif.Value.Hour, a.fecha_asig_asig_calif.Value.Minute, a.fecha_asig_asig_calif.Value.Second),
                             Fecha_fin_asig_calif = DbFunctions.TruncateTime(a.Fecha_fin_asig_calif),
                             Hora_fin_asig_calif = DbFunctions.CreateTime(a.Fecha_fin_asig_calif.Value.Hour, a.Fecha_fin_asig_calif.Value.Minute, a.Fecha_fin_asig_calif.Value.Second),
                             Fecha_inicio_visar1 = DbFunctions.TruncateTime(b.Fecha_inicio_visar1),
                             Hora_inicio_visar1 = DbFunctions.CreateTime(b.Fecha_inicio_visar1.Value.Hour, b.Fecha_inicio_visar1.Value.Minute, b.Fecha_inicio_visar1.Value.Second),
                             Fecha_asig_visar1 = DbFunctions.TruncateTime(b.Fecha_asig_visar1),
                             Hora_asig_visar1 = DbFunctions.CreateTime(b.Fecha_asig_visar1.Value.Hour, b.Fecha_asig_visar1.Value.Minute, b.Fecha_asig_visar1.Value.Second),
                             Fecha_fin_visar1 = DbFunctions.TruncateTime(b.Fecha_fin_visar1),
                             Hora_fin_visar1 = DbFunctions.CreateTime(b.Fecha_fin_visar1.Value.Hour, b.Fecha_fin_visar1.Value.Minute, b.Fecha_fin_visar1.Value.Second),
                             Fecha_inicio_verif_avh = DbFunctions.TruncateTime(c.Fecha_inicio_verif_avh),
                             Hora_inicio_verif_avh = DbFunctions.CreateTime(c.Fecha_inicio_verif_avh.Value.Hour, c.Fecha_inicio_verif_avh.Value.Minute, c.Fecha_inicio_verif_avh.Value.Second),
                             Fecha_asig_verif_avh = DbFunctions.TruncateTime(c.Fecha_asig_verif_avh),
                             Hora_asig_verif_avh = DbFunctions.CreateTime(c.Fecha_asig_verif_avh.Value.Hour, c.Fecha_asig_verif_avh.Value.Minute, c.Fecha_asig_verif_avh.Value.Second),
                             Fecha_fin_verif_avh = DbFunctions.TruncateTime(c.Fecha_fin_verif_avh),
                             Hora_fin_verif_avh = DbFunctions.CreateTime(c.Fecha_fin_verif_avh.Value.Hour, c.Fecha_fin_verif_avh.Value.Minute, c.Fecha_fin_verif_avh.Value.Second),
                             Fecha_asig_rev_sgo = DbFunctions.TruncateTime(d.Fecha_asig_rev_sgo),
                             Hora_asig_rev_sgo = DbFunctions.CreateTime(d.Fecha_asig_rev_sgo.Value.Hour, d.Fecha_asig_rev_sgo.Value.Minute, d.Fecha_asig_rev_sgo.Value.Second),
                             Fecha_fin_rev_sgo = DbFunctions.TruncateTime(d.Fecha_fin_rev_sgo),
                             Hora_fin_rev_sgo = DbFunctions.CreateTime(d.Fecha_fin_rev_sgo.Value.Hour, d.Fecha_fin_rev_sgo.Value.Minute, d.Fecha_fin_rev_sgo.Value.Second),
                             Fecha_inicio_rev_go_1 = DbFunctions.TruncateTime(e.Fecha_inicio_rev_go_1),
                             Hora_inicio_rev_go_1 = DbFunctions.CreateTime(e.Fecha_inicio_rev_go_1.Value.Hour, e.Fecha_inicio_rev_go_1.Value.Minute, e.Fecha_inicio_rev_go_1.Value.Second),
                             Fecha_asig_rev_go_1 = DbFunctions.TruncateTime(e.Fecha_asig_rev_go_1),
                             Hora_asig_rev_go_1 = DbFunctions.CreateTime(e.Fecha_asig_rev_go_1.Value.Hour, e.Fecha_asig_rev_go_1.Value.Minute, e.Fecha_asig_rev_go_1.Value.Second),
                             Fecha_fin_rev_go_1 = DbFunctions.TruncateTime(e.Fecha_fin_rev_go_1),
                             Hora_fin_rev_go_1 = DbFunctions.CreateTime(e.Fecha_fin_rev_go_1.Value.Hour, e.Fecha_fin_rev_go_1.Value.Minute, e.Fecha_fin_rev_go_1.Value.Second),
                             Fecha_inicio_dict_asig_prof = DbFunctions.TruncateTime(f.Fecha_inicio_dict_asig_prof),
                             Hora_inicio_dict_asig_prof = DbFunctions.CreateTime(f.Fecha_inicio_dict_asig_prof.Value.Hour, f.Fecha_inicio_dict_asig_prof.Value.Minute, f.Fecha_inicio_dict_asig_prof.Value.Second),
                             Fecha_asig_dict_asig_prof = DbFunctions.TruncateTime(f.Fecha_asig_dict_asig_prof),
                             Hora_asig_dict_asig_prof = DbFunctions.CreateTime(f.Fecha_asig_dict_asig_prof.Value.Hour, f.Fecha_asig_dict_asig_prof.Value.Minute, f.Fecha_asig_dict_asig_prof.Value.Second),
                             Fecha_fin_dict_asig_prof = DbFunctions.TruncateTime(f.Fecha_fin_dict_asig_prof),
                             Hora_fin_dict_asig_prof = DbFunctions.CreateTime(f.Fecha_fin_dict_asig_prof.Value.Hour, f.Fecha_fin_dict_asig_prof.Value.Minute, f.Fecha_fin_dict_asig_prof.Value.Second),
                             Fecha_inicio_dict_rev_tram = DbFunctions.TruncateTime(g.Fecha_inicio_dict_rev_tram),
                             Hora_inicio_dict_rev_tram = DbFunctions.CreateTime(g.Fecha_inicio_dict_rev_tram.Value.Hour, g.Fecha_inicio_dict_rev_tram.Value.Minute, g.Fecha_inicio_dict_rev_tram.Value.Second),
                             Fecha_asig_dict_rev_tram = DbFunctions.TruncateTime(g.Fecha_asig_dict_rev_tram),
                             Hora_asig_dict_rev_tram = DbFunctions.CreateTime(g.Fecha_asig_dict_rev_tram.Value.Hour, g.Fecha_asig_dict_rev_tram.Value.Minute, g.Fecha_asig_dict_rev_tram.Value.Second),
                             Fecha_fin_dict_rev_tram = DbFunctions.TruncateTime(g.Fecha_fin_dict_rev_tram),
                             Hora_fin_dict_rev_tram = DbFunctions.CreateTime(g.Fecha_fin_dict_rev_tram.Value.Hour, g.Fecha_fin_dict_rev_tram.Value.Minute, g.Fecha_fin_dict_rev_tram.Value.Second),
                             Fecha_inicio_dict_rev_sgo = DbFunctions.TruncateTime(h.Fecha_inicio_dict_rev_sgo),
                             Hora_inicio_dict_rev_sgo = DbFunctions.CreateTime(h.Fecha_inicio_dict_rev_sgo.Value.Hour, h.Fecha_inicio_dict_rev_sgo.Value.Minute, h.Fecha_inicio_dict_rev_sgo.Value.Second),
                             Fecha_asig_dict_rev_sgo = DbFunctions.TruncateTime(h.Fecha_asig_dict_rev_sgo),
                             Hora_asig_dict_rev_sgo = DbFunctions.CreateTime(h.Fecha_asig_dict_rev_sgo.Value.Hour, h.Fecha_asig_dict_rev_sgo.Value.Minute, h.Fecha_asig_dict_rev_sgo.Value.Second),
                             Fecha_fin_dict_rev_sgo = DbFunctions.TruncateTime(h.Fecha_fin_dict_rev_sgo),
                             Hora_fin_dict_rev_sgo = DbFunctions.CreateTime(h.Fecha_fin_dict_rev_sgo.Value.Hour, h.Fecha_fin_dict_rev_sgo.Value.Minute, h.Fecha_fin_dict_rev_sgo.Value.Second),
                             Fecha_inicio_dict_rev_go = DbFunctions.TruncateTime(i.Fecha_inicio_dict_rev_go),
                             Hora_inicio_dict_rev_go = DbFunctions.CreateTime(i.Fecha_inicio_dict_rev_go.Value.Hour, i.Fecha_inicio_dict_rev_go.Value.Minute, i.Fecha_inicio_dict_rev_go.Value.Second),
                             Fecha_asig_dict_rev_go = DbFunctions.TruncateTime(i.Fecha_asig_dict_rev_go),
                             Hora_asig_dict_rev_go = DbFunctions.CreateTime(i.Fecha_asig_dict_rev_go.Value.Hour, i.Fecha_asig_dict_rev_go.Value.Minute, i.Fecha_asig_dict_rev_go.Value.Second),
                             Fecha_fin_dict_rev_go = DbFunctions.TruncateTime(i.Fecha_fin_dict_rev_go),
                             Hora_fin_dict_rev_go = DbFunctions.CreateTime(i.Fecha_fin_dict_rev_go.Value.Hour, i.Fecha_fin_dict_rev_go.Value.Minute, i.Fecha_fin_dict_rev_go.Value.Second),
                             Fecha_inicio_dict_gedo = DbFunctions.TruncateTime(j.Fecha_inicio_dict_gedo),
                             Hora_inicio_dict_gedo = DbFunctions.CreateTime(j.Fecha_inicio_dict_gedo.Value.Hour, j.Fecha_inicio_dict_gedo.Value.Minute, j.Fecha_inicio_dict_gedo.Value.Second),
                             Fecha_asig_dict_gedo = DbFunctions.TruncateTime(j.Fecha_asig_dict_gedo),
                             Hora_asig_dict_gedo = DbFunctions.CreateTime(j.Fecha_asig_dict_gedo.Value.Hour, j.Fecha_asig_dict_gedo.Value.Minute, j.Fecha_asig_dict_gedo.Value.Second),
                             Fecha_fin_dict_gedo = DbFunctions.TruncateTime(j.Fecha_fin_dict_gedo),
                             Hora_fin_dict_gedo = DbFunctions.CreateTime(j.Fecha_fin_dict_gedo.Value.Hour, j.Fecha_fin_dict_gedo.Value.Minute, j.Fecha_fin_dict_gedo.Value.Second),
                             FechaInicio_tramitetarea = DbFunctions.TruncateTime(k.FechaInicio_tramitetarea),
                             HoraInicio_tramitetarea = DbFunctions.CreateTime(k.FechaInicio_tramitetarea.Value.Hour, k.FechaInicio_tramitetarea.Value.Minute, k.FechaInicio_tramitetarea.Value.Second),
                             Fecha_asig_gen_bol = DbFunctions.TruncateTime(k.Fecha_asig_gen_bol),
                             Hora_asig_gen_bol = DbFunctions.CreateTime(k.Fecha_asig_gen_bol.Value.Hour, k.Fecha_asig_gen_bol.Value.Minute, k.Fecha_asig_gen_bol.Value.Second),
                             Fecha_fin_gen_bol = DbFunctions.TruncateTime(k.Fecha_fin_gen_bol),
                             Hora_fin_gen_bol = DbFunctions.CreateTime(k.Fecha_fin_gen_bol.Value.Hour, k.Fecha_fin_gen_bol.Value.Minute, k.Fecha_fin_gen_bol.Value.Second),
                             Fecha_inicio_rev_pag = DbFunctions.TruncateTime(l.Fecha_inicio_rev_pag),
                             Hora_inicio_rev_pag = DbFunctions.CreateTime(l.Fecha_inicio_rev_pag.Value.Hour, l.Fecha_inicio_rev_pag.Value.Minute, l.Fecha_inicio_rev_pag.Value.Second),
                             Fecha_asig_rev_pag = DbFunctions.TruncateTime(l.Fecha_asig_rev_pag),
                             Hora_asig_rev_pag = DbFunctions.CreateTime(l.Fecha_asig_rev_pag.Value.Hour, l.Fecha_asig_rev_pag.Value.Minute, l.Fecha_asig_rev_pag.Value.Second),
                             Fecha_fin_rev_pag = DbFunctions.TruncateTime(l.Fecha_fin_rev_pag),
                             Hora_fin_rev_pag = DbFunctions.CreateTime(l.Fecha_fin_rev_pag.Value.Hour, l.Fecha_fin_rev_pag.Value.Minute, l.Fecha_fin_rev_pag.Value.Second),
                             Fecha_inicio_gen_exp = DbFunctions.TruncateTime(m.Fecha_inicio_gen_exp),
                             Hora_inicio_gen_exp = DbFunctions.CreateTime(m.Fecha_inicio_gen_exp.Value.Hour, m.Fecha_inicio_gen_exp.Value.Minute, m.Fecha_inicio_gen_exp.Value.Second),
                             Fecha_asig_gen_exp = DbFunctions.TruncateTime(m.Fecha_asig_gen_exp),
                             Hora_asig_gen_exp = DbFunctions.CreateTime(m.Fecha_asig_gen_exp.Value.Hour, m.Fecha_asig_gen_exp.Value.Minute, m.Fecha_asig_gen_exp.Value.Second),
                             Fecha_fin_gen_exp = DbFunctions.TruncateTime(m.Fecha_fin_gen_exp),
                             Hora_fin_gen_exp = DbFunctions.CreateTime(m.Fecha_fin_gen_exp.Value.Hour, m.Fecha_fin_gen_exp.Value.Minute, m.Fecha_fin_gen_exp.Value.Second),
                             Fecha_inicio_rev_dghp = DbFunctions.TruncateTime(n.Fecha_inicio_rev_dghp),
                             Hora_inicio_rev_dghp = DbFunctions.CreateTime(n.Fecha_inicio_rev_dghp.Value.Hour, n.Fecha_inicio_rev_dghp.Value.Minute, n.Fecha_inicio_rev_dghp.Value.Second),
                             Fecha_asig_rev_dghp = DbFunctions.TruncateTime(n.Fecha_asig_rev_dghp),
                             Hora_asig_rev_dghp = DbFunctions.CreateTime(n.Fecha_asig_rev_dghp.Value.Hour, n.Fecha_asig_rev_dghp.Value.Minute, n.Fecha_asig_rev_dghp.Value.Second),
                             Fecha_fin_rev_dghp = DbFunctions.TruncateTime(n.Fecha_fin_rev_dghp),
                             Hora_fin_rev_dghp = DbFunctions.CreateTime(n.Fecha_fin_rev_dghp.Value.Hour, n.Fecha_fin_rev_dghp.Value.Minute, n.Fecha_fin_rev_dghp.Value.Second),
                             Fecha_inicio_rev_fir_dispo = DbFunctions.TruncateTime(o.Fecha_inicio_rev_fir_dispo),
                             Hora_inicio_rev_fir_dispo = DbFunctions.CreateTime(o.Fecha_inicio_rev_fir_dispo.Value.Hour, o.Fecha_inicio_rev_fir_dispo.Value.Minute, o.Fecha_inicio_rev_fir_dispo.Value.Second),
                             Fecha_asig_rev_fir_dispo = DbFunctions.TruncateTime(o.Fecha_asig_rev_fir_dispo),
                             Hora_asig_rev_fir_dispo = DbFunctions.CreateTime(o.Fecha_asig_rev_fir_dispo.Value.Hour, o.Fecha_asig_rev_fir_dispo.Value.Minute, o.Fecha_asig_rev_fir_dispo.Value.Second),
                             Fecha_fin_rev_fir_dispo = DbFunctions.TruncateTime(o.Fecha_fin_rev_fir_dispo),
                             Hora_fin_rev_fir_dispo = DbFunctions.CreateTime(o.Fecha_fin_rev_fir_dispo.Value.Hour, o.Fecha_fin_rev_fir_dispo.Value.Minute, o.Fecha_fin_rev_fir_dispo.Value.Second),
                             Fecha_inicio_aprobados = DbFunctions.TruncateTime(p.Fecha_inicio_aprobados),
                             Hora_inicio_aprobados = DbFunctions.CreateTime(p.Fecha_inicio_aprobados.Value.Hour, p.Fecha_inicio_aprobados.Value.Minute, p.Fecha_inicio_aprobados.Value.Second),
                             Fecha_asig_aprobados = DbFunctions.TruncateTime(p.Fecha_asig_aprobados),
                             Hora_asig_aprobados = DbFunctions.CreateTime(p.Fecha_asig_aprobados.Value.Hour, p.Fecha_asig_aprobados.Value.Minute, p.Fecha_asig_aprobados.Value.Second),
                             Fecha_fin_aprobados = DbFunctions.TruncateTime(p.Fecha_fin_aprobados),
                             Hora_fin_aprobados = DbFunctions.CreateTime(p.Fecha_fin_aprobados.Value.Hour, p.Fecha_fin_aprobados.Value.Minute, p.Fecha_fin_aprobados.Value.Second),
                             Fecha_inicio_ent_tra = DbFunctions.TruncateTime(q.Fecha_inicio_ent_tra),
                             Hora_inicio_ent_tra = DbFunctions.CreateTime(q.Fecha_inicio_ent_tra.Value.Hour, q.Fecha_inicio_ent_tra.Value.Minute, q.Fecha_inicio_ent_tra.Value.Second),
                             Fecha_asig_ent_tra = DbFunctions.TruncateTime(q.Fecha_asig_ent_tra),
                             Hora_asig_ent_tra = DbFunctions.CreateTime(q.Fecha_asig_ent_tra.Value.Hour, q.Fecha_asig_ent_tra.Value.Minute, q.Fecha_asig_ent_tra.Value.Second),
                             Fecha_fin_ent_tra = DbFunctions.TruncateTime(q.Fecha_fin_ent_tra),
                             Hora_fin_ent_tra = DbFunctions.CreateTime(q.Fecha_fin_ent_tra.Value.Hour, q.Fecha_fin_ent_tra.Value.Minute, q.Fecha_fin_ent_tra.Value.Second),
                             Fecha_inicio_rechazo_sade = DbFunctions.TruncateTime(r.Fecha_inicio_rechazo_sade),
                             Hora_inicio_rechazo_sade = DbFunctions.CreateTime(r.Fecha_inicio_rechazo_sade.Value.Hour, r.Fecha_inicio_rechazo_sade.Value.Minute, r.Fecha_inicio_rechazo_sade.Value.Second),
                             Fecha_asig_rechazo_sade = DbFunctions.TruncateTime(r.Fecha_asig_rechazo_sade),
                             Hora_asig_rechazo_sade = DbFunctions.CreateTime(r.Fecha_asig_rechazo_sade.Value.Hour, r.Fecha_asig_rechazo_sade.Value.Minute, r.Fecha_asig_rechazo_sade.Value.Second),
                             Fecha_fin_rechazo_sade = DbFunctions.TruncateTime(r.Fecha_fin_rechazo_sade),
                             Hora_fin_rechazo_sade = DbFunctions.CreateTime(r.Fecha_fin_rechazo_sade.Value.Hour, r.Fecha_fin_rechazo_sade.Value.Minute, r.Fecha_fin_rechazo_sade.Value.Second),
                             Fecha_inicio_fin_tra = DbFunctions.TruncateTime(s.Fecha_inicio_fin_tra),
                             Hora_inicio_fin_tra = DbFunctions.CreateTime(s.Fecha_inicio_fin_tra.Value.Hour, s.Fecha_inicio_fin_tra.Value.Minute, s.Fecha_inicio_fin_tra.Value.Second),
                             Fecha_asig_fin_tra = DbFunctions.TruncateTime(s.Fecha_asig_fin_tra),
                             Hora_asig_fin_tra = DbFunctions.CreateTime(s.Fecha_asig_fin_tra.Value.Hour, s.Fecha_asig_fin_tra.Value.Minute, s.Fecha_asig_fin_tra.Value.Second),
                             Fecha_fin_fin_tra = DbFunctions.TruncateTime(s.Fecha_fin_fin_tra),
                             Hora_fin_fin_tra = DbFunctions.CreateTime(s.Fecha_fin_fin_tra.Value.Hour, s.Fecha_fin_fin_tra.Value.Minute, s.Fecha_fin_fin_tra.Value.Second),
                             Fecha_inicio_visar2 = DbFunctions.TruncateTime(t.Fecha_inicio_visar2),
                             Hora_inicio_visar2 = DbFunctions.CreateTime(t.Fecha_inicio_visar2.Value.Hour, t.Fecha_inicio_visar2.Value.Minute, t.Fecha_inicio_visar2.Value.Second),
                             Fecha_asig_visar2 = DbFunctions.TruncateTime(t.Fecha_asig_visar2),
                             Hora_asig_visar2 = DbFunctions.CreateTime(t.Fecha_asig_visar2.Value.Hour, t.Fecha_asig_visar2.Value.Minute, t.Fecha_asig_visar2.Value.Second),
                             Fecha_fin_visar2 = DbFunctions.TruncateTime(t.Fecha_fin_visar2),
                             Hora_fin_visar2 = DbFunctions.CreateTime(t.Fecha_fin_visar2.Value.Hour, t.Fecha_fin_visar2.Value.Minute, t.Fecha_fin_visar2.Value.Second),
                             Fecha_inicio_rev_go_2 = DbFunctions.TruncateTime(u.Fecha_inicio_rev_go_2),
                             Hora_inicio_rev_go_2 = DbFunctions.CreateTime(u.Fecha_inicio_rev_go_2.Value.Hour, u.Fecha_inicio_rev_go_2.Value.Minute, u.Fecha_inicio_rev_go_2.Value.Second),
                             Fecha_asig_rev_go_2 = DbFunctions.TruncateTime(u.Fecha_asig_rev_go_2),
                             Hora_asig_rev_go_2 = DbFunctions.CreateTime(u.Fecha_asig_rev_go_2.Value.Hour, u.Fecha_asig_rev_go_2.Value.Minute, u.Fecha_asig_rev_go_2.Value.Second),
                             Fecha_fin_rev_go_2 = DbFunctions.TruncateTime(u.Fecha_fin_rev_go_2),
                             Hora_fin_rev_go_2 = DbFunctions.CreateTime(u.Fecha_fin_rev_go_2.Value.Hour, u.Fecha_fin_rev_go_2.Value.Minute, u.Fecha_fin_rev_go_2.Value.Second)
                         });

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<Especial> SolicitudesFechayCircuitoEsparcimiento()
        {
            var query = (from hoja3 in
                             (
                                 (from tth in db.SGI_Tramites_Tareas_HAB
                                  join encsol in db.Encomienda_SSIT_Solicitudes on tth.id_solicitud equals encsol.id_solicitud
                                  join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                                  where
                                    tth.SGI_Tramites_Tareas.ENG_Tareas.id_circuito == 6
                                  group tth.SSIT_Solicitudes by new
                                  {
                                      Id_solicitud = (System.Int32?)tth.SSIT_Solicitudes.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.Id_solicitud
                                  }))
                         join a in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 201
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_asig_calif = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      fecha_asig_asig_calif = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_asig_calif = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals a.Id_solicitud into a_join
                         from a in a_join.DefaultIfEmpty()
                         join b in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 202
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_visar1 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_visar1 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_visar1 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals b.Id_solicitud into b_join
                         from b in b_join.DefaultIfEmpty()
                         join c in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 203
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_verif_avh = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_verif_avh = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_verif_avh = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals c.Id_solicitud into c_join
                         from c in c_join.DefaultIfEmpty()
                         join d in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 205
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rev_sgo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rev_sgo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rev_sgo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals d.Id_solicitud into d_join
                         from d in d_join.DefaultIfEmpty()
                         join e in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 206
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rev_go_1 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rev_go_1 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rev_go_1 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals e.Id_solicitud into e_join
                         from e in e_join.DefaultIfEmpty()
                         join f in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 207
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_dict_asig_prof = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_dict_asig_prof = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_fin_dict_asig_prof = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals f.Id_solicitud into f_join
                         from f in f_join.DefaultIfEmpty()
                         join g in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 208
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_dict_rev_tram = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_dict_rev_tram = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_dict_rev_tram = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals g.Id_solicitud into g_join
                         from g in g_join.DefaultIfEmpty()
                         join h in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 209
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_dict_rev_sgo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_dict_rev_sgo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_dict_rev_sgo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals h.Id_solicitud into h_join
                         from h in h_join.DefaultIfEmpty()
                         join i in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 210
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_dict_rev_go = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_dict_rev_go = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_dict_rev_go = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals i.Id_solicitud into i_join
                         from i in i_join.DefaultIfEmpty()
                         join j in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 211
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_dict_gedo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_dict_gedo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_dict_gedo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals j.Id_solicitud into j_join
                         from j in j_join.DefaultIfEmpty()
                         join k in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 213
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      FechaInicio_tramitetarea = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_gen_bol = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_gen_bol = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals k.Id_solicitud into k_join
                         from k in k_join.DefaultIfEmpty()
                         join l in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 214
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rev_pag = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rev_pag = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rev_pag = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals l.Id_solicitud into l_join
                         from l in l_join.DefaultIfEmpty()
                         join m in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 215
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_gen_exp = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_gen_exp = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_gen_exp = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals m.Id_solicitud into m_join
                         from m in m_join.DefaultIfEmpty()
                         join n in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 216
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rev_dghp = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rev_dghp = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rev_dghp = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals n.Id_solicitud into n_join
                         from n in n_join.DefaultIfEmpty()
                         join o in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 217
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rev_fir_dispo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rev_fir_dispo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rev_fir_dispo = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals o.Id_solicitud into o_join
                         from o in o_join.DefaultIfEmpty()
                         join p in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 218
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_aprobados = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_aprobados = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_aprobados = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals p.Id_solicitud into p_join
                         from p in p_join.DefaultIfEmpty()
                         join q in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 219
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_ent_tra = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_ent_tra = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_ent_tra = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals q.Id_solicitud into q_join
                         from q in q_join.DefaultIfEmpty()
                         join r in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 220
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rechazo_sade = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rechazo_sade = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rechazo_sade = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals r.Id_solicitud into r_join
                         from r in r_join.DefaultIfEmpty()
                         join s in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 221
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_fin_tra = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_fin_tra = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_fin_tra = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals s.Id_solicitud into s_join
                         from s in s_join.DefaultIfEmpty()
                         join t in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 204
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_visar2 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_visar2 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_visar2 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals t.Id_solicitud into t_join
                         from t in t_join.DefaultIfEmpty()
                         join u in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == 212
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      Fecha_inicio_rev_go_2 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                      Fecha_asig_rev_go_2 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea),
                                      Fecha_fin_rev_go_2 = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  })) on hoja3.Id_solicitud equals u.Id_solicitud into u_join
                         from u in u_join.DefaultIfEmpty()
                         orderby hoja3.Id_solicitud
                         select new Especial
                         {
                             Id_solicitud = (System.Int32?)hoja3.Id_solicitud,
                             Fecha_inicio_asig_calif = DbFunctions.TruncateTime(a.Fecha_inicio_asig_calif),
                             Hora_inicio_asig_calif = DbFunctions.CreateTime(a.Fecha_inicio_asig_calif.Value.Hour,a.Fecha_inicio_asig_calif.Value.Minute, a.Fecha_inicio_asig_calif.Value.Second),
                             Fecha_asig_asig_calif = DbFunctions.TruncateTime(a.fecha_asig_asig_calif),
                             Hora_asig_asig_calif = DbFunctions.CreateTime(a.fecha_asig_asig_calif.Value.Hour, a.fecha_asig_asig_calif.Value.Minute, a.fecha_asig_asig_calif.Value.Second),
                             Fecha_fin_asig_calif = DbFunctions.TruncateTime(a.Fecha_fin_asig_calif),
                             Hora_fin_asig_calif = DbFunctions.CreateTime(a.Fecha_fin_asig_calif.Value.Hour, a.Fecha_fin_asig_calif.Value.Minute, a.Fecha_fin_asig_calif.Value.Second),
                             Fecha_inicio_visar1 = DbFunctions.TruncateTime(b.Fecha_inicio_visar1),
                             Hora_inicio_visar1 = DbFunctions.CreateTime(b.Fecha_inicio_visar1.Value.Hour, b.Fecha_inicio_visar1.Value.Minute, b.Fecha_inicio_visar1.Value.Second),
                             Fecha_asig_visar1 = DbFunctions.TruncateTime(b.Fecha_asig_visar1),
                             Hora_asig_visar1 = DbFunctions.CreateTime(b.Fecha_asig_visar1.Value.Hour, b.Fecha_asig_visar1.Value.Minute, b.Fecha_asig_visar1.Value.Second),
                             Fecha_fin_visar1 = DbFunctions.TruncateTime(b.Fecha_fin_visar1),
                             Hora_fin_visar1 = DbFunctions.CreateTime(b.Fecha_fin_visar1.Value.Hour, b.Fecha_fin_visar1.Value.Minute, b.Fecha_fin_visar1.Value.Second),
                             Fecha_inicio_verif_avh = DbFunctions.TruncateTime(c.Fecha_inicio_verif_avh),
                             Hora_inicio_verif_avh = DbFunctions.CreateTime(c.Fecha_inicio_verif_avh.Value.Hour, c.Fecha_inicio_verif_avh.Value.Minute, c.Fecha_inicio_verif_avh.Value.Second),
                             Fecha_asig_verif_avh = DbFunctions.TruncateTime(c.Fecha_asig_verif_avh),
                             Hora_asig_verif_avh = DbFunctions.CreateTime(c.Fecha_asig_verif_avh.Value.Hour, c.Fecha_asig_verif_avh.Value.Minute, c.Fecha_asig_verif_avh.Value.Second),
                             Fecha_fin_verif_avh = DbFunctions.TruncateTime(c.Fecha_fin_verif_avh),
                             Hora_fin_verif_avh = DbFunctions.CreateTime(c.Fecha_fin_verif_avh.Value.Hour, c.Fecha_fin_verif_avh.Value.Minute, c.Fecha_fin_verif_avh.Value.Second),
                             Fecha_asig_rev_sgo = DbFunctions.TruncateTime(d.Fecha_asig_rev_sgo),
                             Hora_asig_rev_sgo = DbFunctions.CreateTime(d.Fecha_asig_rev_sgo.Value.Hour, d.Fecha_asig_rev_sgo.Value.Minute, d.Fecha_asig_rev_sgo.Value.Second),
                             Fecha_fin_rev_sgo = DbFunctions.TruncateTime(d.Fecha_fin_rev_sgo),
                             Hora_fin_rev_sgo = DbFunctions.CreateTime(d.Fecha_fin_rev_sgo.Value.Hour, d.Fecha_fin_rev_sgo.Value.Minute, d.Fecha_fin_rev_sgo.Value.Second),
                             Fecha_inicio_rev_go_1 = DbFunctions.TruncateTime(e.Fecha_inicio_rev_go_1),
                             Hora_inicio_rev_go_1 = DbFunctions.CreateTime(e.Fecha_inicio_rev_go_1.Value.Hour, e.Fecha_inicio_rev_go_1.Value.Minute, e.Fecha_inicio_rev_go_1.Value.Second),
                             Fecha_asig_rev_go_1 = DbFunctions.TruncateTime(e.Fecha_asig_rev_go_1),
                             Hora_asig_rev_go_1 = DbFunctions.CreateTime(e.Fecha_asig_rev_go_1.Value.Hour, e.Fecha_asig_rev_go_1.Value.Minute, e.Fecha_asig_rev_go_1.Value.Second),
                             Fecha_fin_rev_go_1 = DbFunctions.TruncateTime(e.Fecha_fin_rev_go_1),
                             Hora_fin_rev_go_1 = DbFunctions.CreateTime(e.Fecha_fin_rev_go_1.Value.Hour, e.Fecha_fin_rev_go_1.Value.Minute, e.Fecha_fin_rev_go_1.Value.Second),
                             Fecha_inicio_dict_asig_prof = DbFunctions.TruncateTime(f.Fecha_inicio_dict_asig_prof),
                             Hora_inicio_dict_asig_prof = DbFunctions.CreateTime(f.Fecha_inicio_dict_asig_prof.Value.Hour, f.Fecha_inicio_dict_asig_prof.Value.Minute, f.Fecha_inicio_dict_asig_prof.Value.Second),
                             Fecha_asig_dict_asig_prof = DbFunctions.TruncateTime(f.Fecha_asig_dict_asig_prof),
                             Hora_asig_dict_asig_prof = DbFunctions.CreateTime(f.Fecha_asig_dict_asig_prof.Value.Hour, f.Fecha_asig_dict_asig_prof.Value.Minute, f.Fecha_asig_dict_asig_prof.Value.Second),
                             Fecha_fin_dict_asig_prof = DbFunctions.TruncateTime(f.Fecha_fin_dict_asig_prof),
                             Hora_fin_dict_asig_prof = DbFunctions.CreateTime(f.Fecha_fin_dict_asig_prof.Value.Hour, f.Fecha_fin_dict_asig_prof.Value.Minute, f.Fecha_fin_dict_asig_prof.Value.Second),
                             Fecha_inicio_dict_rev_tram = DbFunctions.TruncateTime(g.Fecha_inicio_dict_rev_tram),
                             Hora_inicio_dict_rev_tram = DbFunctions.CreateTime(g.Fecha_inicio_dict_rev_tram.Value.Hour, g.Fecha_inicio_dict_rev_tram.Value.Minute, g.Fecha_inicio_dict_rev_tram.Value.Second),
                             Fecha_asig_dict_rev_tram = DbFunctions.TruncateTime(g.Fecha_asig_dict_rev_tram),
                             Hora_asig_dict_rev_tram = DbFunctions.CreateTime(g.Fecha_asig_dict_rev_tram.Value.Hour, g.Fecha_asig_dict_rev_tram.Value.Minute, g.Fecha_asig_dict_rev_tram.Value.Second),
                             Fecha_fin_dict_rev_tram = DbFunctions.TruncateTime(g.Fecha_fin_dict_rev_tram),
                             Hora_fin_dict_rev_tram = DbFunctions.CreateTime(g.Fecha_fin_dict_rev_tram.Value.Hour, g.Fecha_fin_dict_rev_tram.Value.Minute, g.Fecha_fin_dict_rev_tram.Value.Second),
                             Fecha_inicio_dict_rev_sgo = DbFunctions.TruncateTime(h.Fecha_inicio_dict_rev_sgo),
                             Hora_inicio_dict_rev_sgo = DbFunctions.CreateTime(h.Fecha_inicio_dict_rev_sgo.Value.Hour, h.Fecha_inicio_dict_rev_sgo.Value.Minute, h.Fecha_inicio_dict_rev_sgo.Value.Second),
                             Fecha_asig_dict_rev_sgo = DbFunctions.TruncateTime(h.Fecha_asig_dict_rev_sgo),
                             Hora_asig_dict_rev_sgo = DbFunctions.CreateTime(h.Fecha_asig_dict_rev_sgo.Value.Hour, h.Fecha_asig_dict_rev_sgo.Value.Minute, h.Fecha_asig_dict_rev_sgo.Value.Second),
                             Fecha_fin_dict_rev_sgo = DbFunctions.TruncateTime(h.Fecha_fin_dict_rev_sgo),
                             Hora_fin_dict_rev_sgo = DbFunctions.CreateTime(h.Fecha_fin_dict_rev_sgo.Value.Hour, h.Fecha_fin_dict_rev_sgo.Value.Minute, h.Fecha_fin_dict_rev_sgo.Value.Second),
                             Fecha_inicio_dict_rev_go = DbFunctions.TruncateTime(i.Fecha_inicio_dict_rev_go),
                             Hora_inicio_dict_rev_go = DbFunctions.CreateTime(i.Fecha_inicio_dict_rev_go.Value.Hour, i.Fecha_inicio_dict_rev_go.Value.Minute, i.Fecha_inicio_dict_rev_go.Value.Second),
                             Fecha_asig_dict_rev_go = DbFunctions.TruncateTime(i.Fecha_asig_dict_rev_go),
                             Hora_asig_dict_rev_go = DbFunctions.CreateTime(i.Fecha_asig_dict_rev_go.Value.Hour, i.Fecha_asig_dict_rev_go.Value.Minute, i.Fecha_asig_dict_rev_go.Value.Second),
                             Fecha_fin_dict_rev_go = DbFunctions.TruncateTime(i.Fecha_fin_dict_rev_go),
                             Hora_fin_dict_rev_go = DbFunctions.CreateTime(i.Fecha_fin_dict_rev_go.Value.Hour, i.Fecha_fin_dict_rev_go.Value.Minute, i.Fecha_fin_dict_rev_go.Value.Second),
                             Fecha_inicio_dict_gedo = DbFunctions.TruncateTime(j.Fecha_inicio_dict_gedo),
                             Hora_inicio_dict_gedo = DbFunctions.CreateTime(j.Fecha_inicio_dict_gedo.Value.Hour, j.Fecha_inicio_dict_gedo.Value.Minute, j.Fecha_inicio_dict_gedo.Value.Second),
                             Fecha_asig_dict_gedo = DbFunctions.TruncateTime(j.Fecha_asig_dict_gedo),
                             Hora_asig_dict_gedo = DbFunctions.CreateTime(j.Fecha_asig_dict_gedo.Value.Hour, j.Fecha_asig_dict_gedo.Value.Minute, j.Fecha_asig_dict_gedo.Value.Second),
                             Fecha_fin_dict_gedo = DbFunctions.TruncateTime(j.Fecha_fin_dict_gedo),
                             Hora_fin_dict_gedo = DbFunctions.CreateTime(j.Fecha_fin_dict_gedo.Value.Hour, j.Fecha_fin_dict_gedo.Value.Minute, j.Fecha_fin_dict_gedo.Value.Second),
                             FechaInicio_tramitetarea = DbFunctions.TruncateTime(k.FechaInicio_tramitetarea),
                             HoraInicio_tramitetarea = DbFunctions.CreateTime(k.FechaInicio_tramitetarea.Value.Hour, k.FechaInicio_tramitetarea.Value.Minute, k.FechaInicio_tramitetarea.Value.Second),
                             Fecha_asig_gen_bol = DbFunctions.TruncateTime(k.Fecha_asig_gen_bol),
                             Hora_asig_gen_bol = DbFunctions.CreateTime(k.Fecha_asig_gen_bol.Value.Hour, k.Fecha_asig_gen_bol.Value.Minute, k.Fecha_asig_gen_bol.Value.Second),
                             Fecha_fin_gen_bol = DbFunctions.TruncateTime(k.Fecha_fin_gen_bol),
                             Hora_fin_gen_bol = DbFunctions.CreateTime(k.Fecha_fin_gen_bol.Value.Hour, k.Fecha_fin_gen_bol.Value.Minute, k.Fecha_fin_gen_bol.Value.Second),
                             Fecha_inicio_rev_pag = DbFunctions.TruncateTime(l.Fecha_inicio_rev_pag),
                             Hora_inicio_rev_pag = DbFunctions.CreateTime(l.Fecha_inicio_rev_pag.Value.Hour, l.Fecha_inicio_rev_pag.Value.Minute, l.Fecha_inicio_rev_pag.Value.Second),
                             Fecha_asig_rev_pag = DbFunctions.TruncateTime(l.Fecha_asig_rev_pag),
                             Hora_asig_rev_pag = DbFunctions.CreateTime(l.Fecha_asig_rev_pag.Value.Hour, l.Fecha_asig_rev_pag.Value.Minute, l.Fecha_asig_rev_pag.Value.Second),
                             Fecha_fin_rev_pag = DbFunctions.TruncateTime(l.Fecha_fin_rev_pag),
                             Hora_fin_rev_pag = DbFunctions.CreateTime(l.Fecha_fin_rev_pag.Value.Hour, l.Fecha_fin_rev_pag.Value.Minute, l.Fecha_fin_rev_pag.Value.Second),
                             Fecha_inicio_gen_exp = DbFunctions.TruncateTime(m.Fecha_inicio_gen_exp),
                             Hora_inicio_gen_exp = DbFunctions.CreateTime(m.Fecha_inicio_gen_exp.Value.Hour, m.Fecha_inicio_gen_exp.Value.Minute, m.Fecha_inicio_gen_exp.Value.Second),
                             Fecha_asig_gen_exp = DbFunctions.TruncateTime(m.Fecha_asig_gen_exp),
                             Hora_asig_gen_exp = DbFunctions.CreateTime(m.Fecha_asig_gen_exp.Value.Hour, m.Fecha_asig_gen_exp.Value.Minute, m.Fecha_asig_gen_exp.Value.Second),
                             Fecha_fin_gen_exp = DbFunctions.TruncateTime(m.Fecha_fin_gen_exp),
                             Hora_fin_gen_exp = DbFunctions.CreateTime(m.Fecha_fin_gen_exp.Value.Hour, m.Fecha_fin_gen_exp.Value.Minute, m.Fecha_fin_gen_exp.Value.Second),
                             Fecha_inicio_rev_dghp = DbFunctions.TruncateTime(n.Fecha_inicio_rev_dghp),
                             Hora_inicio_rev_dghp = DbFunctions.CreateTime(n.Fecha_inicio_rev_dghp.Value.Hour, n.Fecha_inicio_rev_dghp.Value.Minute, n.Fecha_inicio_rev_dghp.Value.Second),
                             Fecha_asig_rev_dghp = DbFunctions.TruncateTime(n.Fecha_asig_rev_dghp),
                             Hora_asig_rev_dghp = DbFunctions.CreateTime(n.Fecha_asig_rev_dghp.Value.Hour, n.Fecha_asig_rev_dghp.Value.Minute, n.Fecha_asig_rev_dghp.Value.Second),
                             Fecha_fin_rev_dghp = DbFunctions.TruncateTime(n.Fecha_fin_rev_dghp),
                             Hora_fin_rev_dghp = DbFunctions.CreateTime(n.Fecha_fin_rev_dghp.Value.Hour, n.Fecha_fin_rev_dghp.Value.Minute, n.Fecha_fin_rev_dghp.Value.Second),
                             Fecha_inicio_rev_fir_dispo = DbFunctions.TruncateTime(o.Fecha_inicio_rev_fir_dispo),
                             Hora_inicio_rev_fir_dispo = DbFunctions.CreateTime(o.Fecha_inicio_rev_fir_dispo.Value.Hour, o.Fecha_inicio_rev_fir_dispo.Value.Minute, o.Fecha_inicio_rev_fir_dispo.Value.Second),
                             Fecha_asig_rev_fir_dispo = DbFunctions.TruncateTime(o.Fecha_asig_rev_fir_dispo),
                             Hora_asig_rev_fir_dispo = DbFunctions.CreateTime(o.Fecha_asig_rev_fir_dispo.Value.Hour, o.Fecha_asig_rev_fir_dispo.Value.Minute, o.Fecha_asig_rev_fir_dispo.Value.Second),
                             Fecha_fin_rev_fir_dispo = DbFunctions.TruncateTime(o.Fecha_fin_rev_fir_dispo),
                             Hora_fin_rev_fir_dispo = DbFunctions.CreateTime(o.Fecha_fin_rev_fir_dispo.Value.Hour, o.Fecha_fin_rev_fir_dispo.Value.Minute, o.Fecha_fin_rev_fir_dispo.Value.Second),
                             Fecha_inicio_aprobados = DbFunctions.TruncateTime(p.Fecha_inicio_aprobados),
                             Hora_inicio_aprobados = DbFunctions.CreateTime(p.Fecha_inicio_aprobados.Value.Hour, p.Fecha_inicio_aprobados.Value.Minute, p.Fecha_inicio_aprobados.Value.Second),
                             Fecha_asig_aprobados = DbFunctions.TruncateTime(p.Fecha_asig_aprobados),
                             Hora_asig_aprobados = DbFunctions.CreateTime(p.Fecha_asig_aprobados.Value.Hour, p.Fecha_asig_aprobados.Value.Minute, p.Fecha_asig_aprobados.Value.Second),
                             Fecha_fin_aprobados = DbFunctions.TruncateTime(p.Fecha_fin_aprobados),
                             Hora_fin_aprobados = DbFunctions.CreateTime(p.Fecha_fin_aprobados.Value.Hour, p.Fecha_fin_aprobados.Value.Minute, p.Fecha_fin_aprobados.Value.Second),
                             Fecha_inicio_ent_tra = DbFunctions.TruncateTime(q.Fecha_inicio_ent_tra),
                             Hora_inicio_ent_tra = DbFunctions.CreateTime(q.Fecha_inicio_ent_tra.Value.Hour, q.Fecha_inicio_ent_tra.Value.Minute, q.Fecha_inicio_ent_tra.Value.Second),
                             Fecha_asig_ent_tra = DbFunctions.TruncateTime(q.Fecha_asig_ent_tra),
                             Hora_asig_ent_tra = DbFunctions.CreateTime(q.Fecha_asig_ent_tra.Value.Hour, q.Fecha_asig_ent_tra.Value.Minute, q.Fecha_asig_ent_tra.Value.Second),
                             Fecha_fin_ent_tra = DbFunctions.TruncateTime(q.Fecha_fin_ent_tra),
                             Hora_fin_ent_tra = DbFunctions.CreateTime(q.Fecha_fin_ent_tra.Value.Hour, q.Fecha_fin_ent_tra.Value.Minute, q.Fecha_fin_ent_tra.Value.Second),
                             Fecha_inicio_rechazo_sade = DbFunctions.TruncateTime(r.Fecha_inicio_rechazo_sade),
                             Hora_inicio_rechazo_sade = DbFunctions.CreateTime(r.Fecha_inicio_rechazo_sade.Value.Hour, r.Fecha_inicio_rechazo_sade.Value.Minute, r.Fecha_inicio_rechazo_sade.Value.Second),
                             Fecha_asig_rechazo_sade = DbFunctions.TruncateTime(r.Fecha_asig_rechazo_sade),
                             Hora_asig_rechazo_sade = DbFunctions.CreateTime(r.Fecha_asig_rechazo_sade.Value.Hour, r.Fecha_asig_rechazo_sade.Value.Minute, r.Fecha_asig_rechazo_sade.Value.Second),
                             Fecha_fin_rechazo_sade = DbFunctions.TruncateTime(r.Fecha_fin_rechazo_sade),
                             Hora_fin_rechazo_sade = DbFunctions.CreateTime(r.Fecha_fin_rechazo_sade.Value.Hour, r.Fecha_fin_rechazo_sade.Value.Minute, r.Fecha_fin_rechazo_sade.Value.Second),
                             Fecha_inicio_fin_tra = DbFunctions.TruncateTime(s.Fecha_inicio_fin_tra),
                             Hora_inicio_fin_tra = DbFunctions.CreateTime(s.Fecha_inicio_fin_tra.Value.Hour, s.Fecha_inicio_fin_tra.Value.Minute, s.Fecha_inicio_fin_tra.Value.Second),
                             Fecha_asig_fin_tra = DbFunctions.TruncateTime(s.Fecha_asig_fin_tra),
                             Hora_asig_fin_tra = DbFunctions.CreateTime(s.Fecha_asig_fin_tra.Value.Hour, s.Fecha_asig_fin_tra.Value.Minute, s.Fecha_asig_fin_tra.Value.Second),
                             Fecha_fin_fin_tra = DbFunctions.TruncateTime(s.Fecha_fin_fin_tra),
                             Hora_fin_fin_tra = DbFunctions.CreateTime(s.Fecha_fin_fin_tra.Value.Hour, s.Fecha_fin_fin_tra.Value.Minute, s.Fecha_fin_fin_tra.Value.Second),
                             Fecha_inicio_visar2 = DbFunctions.TruncateTime(t.Fecha_inicio_visar2),
                             Hora_inicio_visar2 = DbFunctions.CreateTime(t.Fecha_inicio_visar2.Value.Hour, t.Fecha_inicio_visar2.Value.Minute, t.Fecha_inicio_visar2.Value.Second),
                             Fecha_asig_visar2 = DbFunctions.TruncateTime(t.Fecha_asig_visar2),
                             Hora_asig_visar2 = DbFunctions.CreateTime(t.Fecha_asig_visar2.Value.Hour, t.Fecha_asig_visar2.Value.Minute, t.Fecha_asig_visar2.Value.Second),
                             Fecha_fin_visar2 = DbFunctions.TruncateTime(t.Fecha_fin_visar2),
                             Hora_fin_visar2 = DbFunctions.CreateTime(t.Fecha_fin_visar2.Value.Hour, t.Fecha_fin_visar2.Value.Minute, t.Fecha_fin_visar2.Value.Second),
                             Fecha_inicio_rev_go_2 = DbFunctions.TruncateTime(u.Fecha_inicio_rev_go_2),
                             Hora_inicio_rev_go_2 = DbFunctions.CreateTime(u.Fecha_inicio_rev_go_2.Value.Hour, u.Fecha_inicio_rev_go_2.Value.Minute, u.Fecha_inicio_rev_go_2.Value.Second),
                             Fecha_asig_rev_go_2 = DbFunctions.TruncateTime(u.Fecha_asig_rev_go_2),
                             Hora_asig_rev_go_2 = DbFunctions.CreateTime(u.Fecha_asig_rev_go_2.Value.Hour, u.Fecha_asig_rev_go_2.Value.Minute, u.Fecha_asig_rev_go_2.Value.Second),
                             Fecha_fin_rev_go_2 = DbFunctions.TruncateTime(u.Fecha_fin_rev_go_2),
                             Hora_fin_rev_go_2 = DbFunctions.CreateTime(u.Fecha_fin_rev_go_2.Value.Hour, u.Fecha_fin_rev_go_2.Value.Minute, u.Fecha_fin_rev_go_2.Value.Second)                          
                         });

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ListadoVidaHabilitacion2> SolicitudesFechayCircuito300000(DateTime dtInicio, DateTime dtFin, int IdCircuito)
        {
            var query = (from a in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea
                                  join ssit in db.SSIT_Solicitudes on trah.id_solicitud equals ssit.id_solicitud
                                  join encsol in db.Encomienda_SSIT_Solicitudes on ssit.id_solicitud equals encsol.id_solicitud
                                  join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                                  join daenc in db.Encomienda_DatosLocal on enc.id_encomienda equals daenc.id_encomienda

                                  where
                                    tra.id_tarea == 301 || tra.id_tarea == 302
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Calif_Ini = tra.FechaInicio_tramitetarea,
                                      Calif_fin = tra.FechaCierre_tramitetarea,
                                      superficie = (System.Decimal?)(daenc.superficie_cubierta_dl + daenc.superficie_descubierta_dl),
                                      NroExpedienteSade = ssit.NroExpedienteSade
                                  }))
                         join b in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 307
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Exp_ini = tra.FechaInicio_tramitetarea,
                                      Exp_Cierre = tra.FechaCierre_tramitetarea,
                                      Exp_Asignacion = tra.FechaAsignacion_tramtietarea
                                  })) on a.Id_solicitud equals b.Id_solicitud into b_join
                         from b in b_join.DefaultIfEmpty()
                         join c in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 305
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Rev_Ger_Cierre = tra.FechaCierre_tramitetarea,
                                      Rev_Ger_ini = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals c.Id_solicitud into c_join
                         from c in c_join.DefaultIfEmpty()
                         //join d in
                         //    (
                         //        (from tra in db.SGI_Tramites_Tareas
                         //         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         //         from trah in trah_join.DefaultIfEmpty()
                         //         where
                         //           tra.id_tarea == 26
                         //         select new
                         //         {
                         //             Id_solicitud = (System.Int32?)trah.id_solicitud,
                         //             BU_ini = tra.FechaInicio_tramitetarea,
                         //             BU_Cierre = tra.FechaCierre_tramitetarea
                         //         })) on a.Id_solicitud equals d.Id_solicitud into d_join
                         //from d in d_join.DefaultIfEmpty()
                         //join e in
                         //    (
                         //        (from tra in db.SGI_Tramites_Tareas
                         //         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         //         from trah in trah_join.DefaultIfEmpty()
                         //         where
                         //           tra.id_tarea == 21
                         //         select new
                         //         {
                         //             Id_solicitud = (System.Int32?)trah.id_solicitud,
                         //             BU_Cierre = tra.FechaCierre_tramitetarea,
                         //             BU_ini = tra.FechaInicio_tramitetarea
                         //         })) on a.Id_solicitud equals e.Id_solicitud into e_join
                         //from e in e_join.DefaultIfEmpty()
                         join f in
                             (
                                 ((from tra in db.SGI_Tramites_Tareas
                                   join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                   from trah in trah_join.DefaultIfEmpty()
                                   where
                                     tra.id_tarea == 311
                                   select new
                                   {
                                       Id_solicitud = (System.Int32?)trah.id_solicitud,
                                       Cor_Solicitud = tra.FechaInicio_tramitetarea
                                   }).Distinct())) on a.Id_solicitud equals f.Id_solicitud into f_join
                         from f in f_join.DefaultIfEmpty()
                         join g in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 308
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      FirmaDispo_Ini = tra.FechaInicio_tramitetarea,
                                      FirmaDispo_Cierre = tra.FechaCierre_tramitetarea,
                                      FirmaDispo_Asign = tra.FechaAsignacion_tramtietarea
                                  })) on a.Id_solicitud equals g.Id_solicitud into g_join
                         from g in g_join.DefaultIfEmpty()
                         join h in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 310
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Fecha_Ini_Entrega = tra.FechaInicio_tramitetarea,
                                      Fecha_Cierre_Entrega = tra.FechaCierre_tramitetarea
                                  })) on a.Id_solicitud equals h.Id_solicitud into h_join
                         from h in h_join.DefaultIfEmpty()
                         join i in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 306
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      FirmaRDGHP_Cierre = tra.FechaCierre_tramitetarea,
                                      FirmaRDGHP_Inicio = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals i.Id_solicitud into i_join
                         from i in i_join.DefaultIfEmpty()

                         join firma_dispo in
                             (
                              from tarea in db.SGI_Tramites_Tareas
                              join proceso in db.SGI_Tarea_Generar_Expediente_Procesos on tarea.id_tramitetarea equals proceso.id_tramitetarea
                              join firma in db.wsEE_TareasDocumentos on proceso.id_paquete equals firma.id_paquete
                              join trah in db.SGI_Tramites_Tareas_HAB on proceso.id_tramitetarea equals trah.id_tramitetarea into trah_join
                              from trah in trah_join.DefaultIfEmpty()
                              where
                                    tarea.id_tarea == 308 && firma.firmado_en_SADE
                              group new { trah, tarea, firma } by new
                              {
                                  trah.id_solicitud,
                                  firma.numeroGEDO
                              } into g
                              select new
                              {
                                  Id_solicitud = (int?)g.Key.id_solicitud,
                                  numeroGEDO = g.Key.numeroGEDO
                              }
                                 )

                        on a.Id_solicitud equals firma_dispo.Id_solicitud into firma_dispo_join
                         from firma_dispo in firma_dispo_join.DefaultIfEmpty()

                         from tra in db.SGI_Tramites_Tareas
                         join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea
                         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         from trah in trah_join.DefaultIfEmpty()


                         where
                           a.Calif_Ini != null &&
                           a.Calif_Ini >= dtInicio && a.Calif_Ini <= dtFin &&
                           tar.id_circuito == IdCircuito
                         group new { a, b, c, f, g, h, i, firma_dispo } by new
                         {
                             a.Id_solicitud,
                             a.Calif_Ini,
                             Column1 = a.Calif_Ini,
                             a.Calif_fin,
                             Column2 = a.Calif_fin,
                             a.superficie,
                             b.Exp_ini,
                             Column3 = b.Exp_ini,
                             b.Exp_Cierre,
                             Column4 = b.Exp_Cierre,
                             b.Exp_Asignacion,
                             Column5 = b.Exp_Asignacion,
                             c.Rev_Ger_Cierre,
                             Column6 = c.Rev_Ger_Cierre,
                             c.Rev_Ger_ini,
                             Column7 = c.Rev_Ger_ini,
                             //d.BU_ini,
                             //d.BU_Cierre,
                             //Column8 = e.BU_ini,
                             //Column9 = e.BU_Cierre,
                             Column10 = a.Calif_Ini,
                             Column11 = a.Calif_Ini,
                             f.Cor_Solicitud,
                             g.FirmaDispo_Cierre,
                             g.FirmaDispo_Asign,
                             g.FirmaDispo_Ini,
                             h.Fecha_Cierre_Entrega,
                             h.Fecha_Ini_Entrega,
                             i.FirmaRDGHP_Inicio,
                             i.FirmaRDGHP_Cierre,
                             a.NroExpedienteSade
                         } into g
                         orderby
                           g.Key.Id_solicitud
                         select new ListadoVidaHabilitacion2
                         {
                             Id_solicitud = (System.Int32?)g.Key.Id_solicitud,
                             superficie = (System.Decimal?)g.Key.superficie,
                             Fecha_inicio_ASIGNACION_calif = DbFunctions.TruncateTime(g.Key.Calif_Ini),
                             Hora_inicio_ASIGNACION_calif = DbFunctions.CreateTime(g.Key.Calif_Ini.Hour, g.Key.Calif_Ini.Minute, g.Key.Calif_Ini.Second),
                             Fecha_fin_calif = DbFunctions.TruncateTime(g.Key.Calif_fin),
                             Hora_fin_calif = DbFunctions.CreateTime(g.Key.Calif_fin.Value.Hour, g.Key.Calif_fin.Value.Minute, g.Key.Calif_fin.Value.Second),
                             Fecha_Inicio_Exp = DbFunctions.TruncateTime(g.Key.Exp_ini),
                             Hora_Inicio_Exp = DbFunctions.CreateTime(g.Key.Exp_ini.Hour, g.Key.Exp_ini.Minute, g.Key.Exp_ini.Second),
                             Fecha_Asignacion_Exp = DbFunctions.TruncateTime(g.Key.Exp_Asignacion),
                             Hora_Asignacion_Exp = g.Key.Exp_Asignacion.HasValue ? DbFunctions.CreateTime(g.Key.Exp_Asignacion.Value.Hour, g.Key.Exp_Asignacion.Value.Minute, g.Key.Exp_Asignacion.Value.Second) : null,
                             Fecha_Cierre_Exp = DbFunctions.TruncateTime(g.Key.Exp_Cierre),
                             Hora_Cierre_Exp = g.Key.Exp_Cierre.HasValue ? DbFunctions.CreateTime(g.Key.Exp_Cierre.Value.Hour, g.Key.Exp_Cierre.Value.Minute, g.Key.Exp_Cierre.Value.Second) : null,
                             Fecha_Inicio_Rev_Ger = DbFunctions.TruncateTime(g.Key.Rev_Ger_ini),
                             Hora_Inicio_Rev_Ger = DbFunctions.CreateTime(g.Key.Rev_Ger_ini.Hour, g.Key.Rev_Ger_ini.Minute, g.Key.Rev_Ger_ini.Second),
                             Fecha_Cierre_Rev_Ger = DbFunctions.TruncateTime(g.Key.Rev_Ger_Cierre),
                             Hora_Cierre_Rev_Ger = g.Key.Rev_Ger_Cierre.HasValue ? DbFunctions.CreateTime(g.Key.Rev_Ger_Cierre.Value.Hour, g.Key.Rev_Ger_Cierre.Value.Minute, g.Key.Rev_Ger_Cierre.Value.Second) : null,
                             //Fecha_Gen_BU_ini = g.Key.BU_ini.Day + "/" + g.Key.BU_ini.Month + "/" + g.Key.BU_ini.Year,
                             //Fecha_Gen_BU_cierre = g.Key.BU_Cierre.HasValue ? g.Key.BU_Cierre.Value.Day + "/" + g.Key.BU_Cierre.Value.Month + "/" + g.Key.BU_Cierre.Value.Year : "",
                             //Fecha_Rev_BU_ini = g.Key.Column8.Day + "/" + g.Key.Column8.Month + "/" + g.Key.Column8.Year,
                             //Fecha_Rev_BU_cierre = g.Key.Column9.HasValue ? g.Key.Column9.Value.Day + "/" + g.Key.Column9.Value.Month + "/" + g.Key.Column9.Value.Year : "",
                             observado_alguna_vez = g.Key.Cor_Solicitud.Day + "/" + g.Key.Cor_Solicitud.Month + "/" + g.Key.Cor_Solicitud.Year,
                             Fecha_Cierre_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Cierre),
                             Fecha_Asign_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Asign),
                             Fecha_Ini_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Ini),
                             Fecha_Cierre_Entrega = DbFunctions.TruncateTime(g.Key.Fecha_Cierre_Entrega),
                             Fecha_Ini_Entrega = DbFunctions.TruncateTime(g.Key.Fecha_Ini_Entrega),
                             FirmaRDGHP_Inicio = DbFunctions.TruncateTime(g.Key.FirmaRDGHP_Inicio),
                             FirmaRDGHP_Cierre = DbFunctions.TruncateTime(g.Key.FirmaRDGHP_Cierre),
                             NroExpedienteSade = g.Key.NroExpedienteSade
                         });

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ListadoVidaHabilitacion2> SolicitudesFechayCircuitoIP(DateTime dtInicio, DateTime dtFin, int IdCircuito)
        {
            var query = (from a in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea
                                  join ssit in db.SSIT_Solicitudes on trah.id_solicitud equals ssit.id_solicitud
                                  join encsol in db.Encomienda_SSIT_Solicitudes on ssit.id_solicitud equals encsol.id_solicitud
                                  join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                                  join daenc in db.Encomienda_DatosLocal on enc.id_encomienda equals daenc.id_encomienda

                                  where
                                    tra.id_tarea == 501
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Calif_Ini = tra.FechaInicio_tramitetarea,
                                      Calif_fin = tra.FechaCierre_tramitetarea,
                                      superficie = (System.Decimal?)(daenc.superficie_cubierta_dl + daenc.superficie_descubierta_dl),
                                      NroExpedienteSade = ssit.NroExpedienteSade
                                  }))
                         join b in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 514
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Exp_ini = tra.FechaInicio_tramitetarea,
                                      Exp_Cierre = tra.FechaCierre_tramitetarea,
                                      Exp_Asignacion = tra.FechaAsignacion_tramtietarea
                                  })) on a.Id_solicitud equals b.Id_solicitud into b_join
                         from b in b_join.DefaultIfEmpty()
                         join c in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    (new int[] { 504,509 }).Contains(tra.id_tarea)
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Rev_Ger_Cierre = tra.FechaCierre_tramitetarea,
                                      Rev_Ger_ini = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals c.Id_solicitud into c_join
                         from c in c_join.DefaultIfEmpty()
                         //join d in
                         //    (
                         //        (from tra in db.SGI_Tramites_Tareas
                         //         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         //         from trah in trah_join.DefaultIfEmpty()
                         //         where
                         //           tra.id_tarea == 26
                         //         select new
                         //         {
                         //             Id_solicitud = (System.Int32?)trah.id_solicitud,
                         //             BU_ini = tra.FechaInicio_tramitetarea,
                         //             BU_Cierre = tra.FechaCierre_tramitetarea
                         //         })) on a.Id_solicitud equals d.Id_solicitud into d_join
                         //from d in d_join.DefaultIfEmpty()
                         //join e in
                         //    (
                         //        (from tra in db.SGI_Tramites_Tareas
                         //         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         //         from trah in trah_join.DefaultIfEmpty()
                         //         where
                         //           tra.id_tarea == 21
                         //         select new
                         //         {
                         //             Id_solicitud = (System.Int32?)trah.id_solicitud,
                         //             BU_Cierre = tra.FechaCierre_tramitetarea,
                         //             BU_ini = tra.FechaInicio_tramitetarea
                         //         })) on a.Id_solicitud equals e.Id_solicitud into e_join
                         //from e in e_join.DefaultIfEmpty()
                         join f in
                             (
                                 ((from tra in db.SGI_Tramites_Tareas
                                   join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                   from trah in trah_join.DefaultIfEmpty()
                                   where
                                     tra.id_tarea == 518
                                   select new
                                   {
                                       Id_solicitud = (System.Int32?)trah.id_solicitud,
                                       Cor_Solicitud = tra.FechaInicio_tramitetarea
                                   }).Distinct())) on a.Id_solicitud equals f.Id_solicitud into f_join
                         from f in f_join.DefaultIfEmpty()
                         join g in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 515
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      FirmaDispo_Ini = tra.FechaInicio_tramitetarea,
                                      FirmaDispo_Cierre = tra.FechaCierre_tramitetarea,
                                      FirmaDispo_Asign = tra.FechaAsignacion_tramtietarea
                                  })) on a.Id_solicitud equals g.Id_solicitud into g_join
                         from g in g_join.DefaultIfEmpty()
                         join h in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 517
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Fecha_Ini_Entrega = tra.FechaInicio_tramitetarea,
                                      Fecha_Cierre_Entrega = tra.FechaCierre_tramitetarea
                                  })) on a.Id_solicitud equals h.Id_solicitud into h_join
                         from h in h_join.DefaultIfEmpty()
                         join i in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 513
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      FirmaRDGHP_Cierre = tra.FechaCierre_tramitetarea,
                                      FirmaRDGHP_Inicio = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals i.Id_solicitud into i_join
                         from i in i_join.DefaultIfEmpty()

                         join firma_dispo in
                             (
                              from tarea in db.SGI_Tramites_Tareas
                              join proceso in db.SGI_Tarea_Generar_Expediente_Procesos on tarea.id_tramitetarea equals proceso.id_tramitetarea
                              join firma in db.wsEE_TareasDocumentos on proceso.id_paquete equals firma.id_paquete
                              join trah in db.SGI_Tramites_Tareas_HAB on proceso.id_tramitetarea equals trah.id_tramitetarea into trah_join
                              from trah in trah_join.DefaultIfEmpty()
                              where
                                    tarea.id_tarea == 515 && firma.firmado_en_SADE
                              group new { trah, tarea, firma } by new
                              {
                                  trah.id_solicitud,
                                  firma.numeroGEDO
                              } into g
                              select new
                              {
                                  Id_solicitud = (int?)g.Key.id_solicitud,
                                  numeroGEDO = g.Key.numeroGEDO
                              }
                                 )

                        on a.Id_solicitud equals firma_dispo.Id_solicitud into firma_dispo_join
                         from firma_dispo in firma_dispo_join.DefaultIfEmpty()

                         from tra in db.SGI_Tramites_Tareas
                         join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea
                         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         from trah in trah_join.DefaultIfEmpty()


                         where
                           a.Calif_Ini != null &&
                           a.Calif_Ini >= dtInicio && a.Calif_Ini <= dtFin &&
                           tar.id_circuito == IdCircuito
                         group new { a, b, c, f, g, h, i, firma_dispo } by new
                         {
                             a.Id_solicitud,
                             a.Calif_Ini,
                             Column1 = a.Calif_Ini,
                             a.Calif_fin,
                             Column2 = a.Calif_fin,
                             a.superficie,
                             b.Exp_ini,
                             Column3 = b.Exp_ini,
                             b.Exp_Cierre,
                             Column4 = b.Exp_Cierre,
                             b.Exp_Asignacion,
                             Column5 = b.Exp_Asignacion,
                             c.Rev_Ger_Cierre,
                             Column6 = c.Rev_Ger_Cierre,
                             c.Rev_Ger_ini,
                             Column7 = c.Rev_Ger_ini,
                             //d.BU_ini,
                             //d.BU_Cierre,
                             //Column8 = e.BU_ini,
                             //Column9 = e.BU_Cierre,
                             Column10 = a.Calif_Ini,
                             Column11 = a.Calif_Ini,
                             f.Cor_Solicitud,
                             g.FirmaDispo_Cierre,
                             g.FirmaDispo_Asign,
                             g.FirmaDispo_Ini,
                             h.Fecha_Cierre_Entrega,
                             h.Fecha_Ini_Entrega,
                             i.FirmaRDGHP_Inicio,
                             i.FirmaRDGHP_Cierre,
                             a.NroExpedienteSade
                         } into g
                         orderby
                           g.Key.Id_solicitud
                         select new ListadoVidaHabilitacion2
                         {
                             Id_solicitud = (System.Int32?)g.Key.Id_solicitud,
                             superficie = (System.Decimal?)g.Key.superficie,
                             Fecha_inicio_ASIGNACION_calif = DbFunctions.TruncateTime(g.Key.Calif_Ini),
                             Hora_inicio_ASIGNACION_calif = DbFunctions.CreateTime(g.Key.Calif_Ini.Hour, g.Key.Calif_Ini.Minute, g.Key.Calif_Ini.Second),
                             Fecha_fin_calif = DbFunctions.TruncateTime(g.Key.Calif_fin),
                             Hora_fin_calif = DbFunctions.CreateTime(g.Key.Calif_fin.Value.Hour, g.Key.Calif_fin.Value.Minute, g.Key.Calif_fin.Value.Second),
                             Fecha_Inicio_Exp = DbFunctions.TruncateTime(g.Key.Exp_ini),
                             Hora_Inicio_Exp = DbFunctions.CreateTime(g.Key.Exp_ini.Hour, g.Key.Exp_ini.Minute, g.Key.Exp_ini.Second),
                             Fecha_Asignacion_Exp = DbFunctions.TruncateTime(g.Key.Exp_Asignacion),
                             Hora_Asignacion_Exp = g.Key.Exp_Asignacion.HasValue ? DbFunctions.CreateTime(g.Key.Exp_Asignacion.Value.Hour, g.Key.Exp_Asignacion.Value.Minute, g.Key.Exp_Asignacion.Value.Second) : null,
                             Fecha_Cierre_Exp = DbFunctions.TruncateTime(g.Key.Exp_Cierre),
                             Hora_Cierre_Exp = g.Key.Exp_Cierre.HasValue ? DbFunctions.CreateTime(g.Key.Exp_Cierre.Value.Hour, g.Key.Exp_Cierre.Value.Minute, g.Key.Exp_Cierre.Value.Second) : null,
                             Fecha_Inicio_Rev_Ger = DbFunctions.TruncateTime(g.Key.Rev_Ger_ini),
                             Hora_Inicio_Rev_Ger = DbFunctions.CreateTime(g.Key.Rev_Ger_ini.Hour, g.Key.Rev_Ger_ini.Minute, g.Key.Rev_Ger_ini.Second),
                             Fecha_Cierre_Rev_Ger = DbFunctions.TruncateTime(g.Key.Rev_Ger_Cierre),
                             Hora_Cierre_Rev_Ger = g.Key.Rev_Ger_Cierre.HasValue ? DbFunctions.CreateTime(g.Key.Rev_Ger_Cierre.Value.Hour, g.Key.Rev_Ger_Cierre.Value.Minute, g.Key.Rev_Ger_Cierre.Value.Second) : null,
                             //Fecha_Gen_BU_ini = g.Key.BU_ini.Day + "/" + g.Key.BU_ini.Month + "/" + g.Key.BU_ini.Year,
                             //Fecha_Gen_BU_cierre = g.Key.BU_Cierre.HasValue ? g.Key.BU_Cierre.Value.Day + "/" + g.Key.BU_Cierre.Value.Month + "/" + g.Key.BU_Cierre.Value.Year : "",
                             //Fecha_Rev_BU_ini = g.Key.Column8.Day + "/" + g.Key.Column8.Month + "/" + g.Key.Column8.Year,
                             //Fecha_Rev_BU_cierre = g.Key.Column9.HasValue ? g.Key.Column9.Value.Day + "/" + g.Key.Column9.Value.Month + "/" + g.Key.Column9.Value.Year : "",
                             observado_alguna_vez = g.Key.Cor_Solicitud.Day + "/" + g.Key.Cor_Solicitud.Month + "/" + g.Key.Cor_Solicitud.Year,
                             Fecha_Cierre_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Cierre),
                             Fecha_Asign_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Asign),
                             Fecha_Ini_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Ini),
                             Fecha_Cierre_Entrega = DbFunctions.TruncateTime(g.Key.Fecha_Cierre_Entrega),
                             Fecha_Ini_Entrega = DbFunctions.TruncateTime(g.Key.Fecha_Ini_Entrega),
                             FirmaRDGHP_Inicio = DbFunctions.TruncateTime(g.Key.FirmaRDGHP_Inicio),
                             FirmaRDGHP_Cierre = DbFunctions.TruncateTime(g.Key.FirmaRDGHP_Cierre),
                             NroExpedienteSade = g.Key.NroExpedienteSade
                         });

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <param name="IdCircuito"></param>
        /// <returns></returns>
        public IQueryable<ListadoVidaHabilitacion2> SolicitudesFechayCircuitoHP(DateTime dtInicio, DateTime dtFin, int IdCircuito)
        {
            var query = (from a in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea
                                  join ssit in db.SSIT_Solicitudes on trah.id_solicitud equals ssit.id_solicitud
                                  join encsol in db.Encomienda_SSIT_Solicitudes on ssit.id_solicitud equals encsol.id_solicitud
                                  join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                                  join daenc in db.Encomienda_DatosLocal on enc.id_encomienda equals daenc.id_encomienda

                                  where
                                    tra.id_tarea == 601
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Calif_Ini = tra.FechaInicio_tramitetarea,
                                      Calif_fin = tra.FechaCierre_tramitetarea,
                                      superficie = (System.Decimal?)(daenc.superficie_cubierta_dl + daenc.superficie_descubierta_dl),
                                      NroExpedienteSade = ssit.NroExpedienteSade
                                  }))
                         join b in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 614
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Exp_ini = tra.FechaInicio_tramitetarea,
                                      Exp_Cierre = tra.FechaCierre_tramitetarea,
                                      Exp_Asignacion = tra.FechaAsignacion_tramtietarea
                                  })) on a.Id_solicitud equals b.Id_solicitud into b_join
                         from b in b_join.DefaultIfEmpty()
                         join c in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    (new int[] { 604, 609 }).Contains(tra.id_tarea)
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Rev_Ger_Cierre = tra.FechaCierre_tramitetarea,
                                      Rev_Ger_ini = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals c.Id_solicitud into c_join
                         from c in c_join.DefaultIfEmpty()
                         //join d in
                         //    (
                         //        (from tra in db.SGI_Tramites_Tareas
                         //         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         //         from trah in trah_join.DefaultIfEmpty()
                         //         where
                         //           tra.id_tarea == 26
                         //         select new
                         //         {
                         //             Id_solicitud = (System.Int32?)trah.id_solicitud,
                         //             BU_ini = tra.FechaInicio_tramitetarea,
                         //             BU_Cierre = tra.FechaCierre_tramitetarea
                         //         })) on a.Id_solicitud equals d.Id_solicitud into d_join
                         //from d in d_join.DefaultIfEmpty()
                         //join e in
                         //    (
                         //        (from tra in db.SGI_Tramites_Tareas
                         //         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         //         from trah in trah_join.DefaultIfEmpty()
                         //         where
                         //           tra.id_tarea == 21
                         //         select new
                         //         {
                         //             Id_solicitud = (System.Int32?)trah.id_solicitud,
                         //             BU_Cierre = tra.FechaCierre_tramitetarea,
                         //             BU_ini = tra.FechaInicio_tramitetarea
                         //         })) on a.Id_solicitud equals e.Id_solicitud into e_join
                         //from e in e_join.DefaultIfEmpty()
                         join f in
                             (
                                 ((from tra in db.SGI_Tramites_Tareas
                                   join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                   from trah in trah_join.DefaultIfEmpty()
                                   where
                                     tra.id_tarea == 618
                                   select new
                                   {
                                       Id_solicitud = (System.Int32?)trah.id_solicitud,
                                       Cor_Solicitud = tra.FechaInicio_tramitetarea
                                   }).Distinct())) on a.Id_solicitud equals f.Id_solicitud into f_join
                         from f in f_join.DefaultIfEmpty()
                         join g in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 615
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      FirmaDispo_Ini = tra.FechaInicio_tramitetarea,
                                      FirmaDispo_Cierre = tra.FechaCierre_tramitetarea,
                                      FirmaDispo_Asign = tra.FechaAsignacion_tramtietarea
                                  })) on a.Id_solicitud equals g.Id_solicitud into g_join
                         from g in g_join.DefaultIfEmpty()
                         join h in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 617
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Fecha_Ini_Entrega = tra.FechaInicio_tramitetarea,
                                      Fecha_Cierre_Entrega = tra.FechaCierre_tramitetarea
                                  })) on a.Id_solicitud equals h.Id_solicitud into h_join
                         from h in h_join.DefaultIfEmpty()
                         join i in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 613
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      FirmaRDGHP_Cierre = tra.FechaCierre_tramitetarea,
                                      FirmaRDGHP_Inicio = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals i.Id_solicitud into i_join
                         from i in i_join.DefaultIfEmpty()

                         join firma_dispo in
                             (
                              from tarea in db.SGI_Tramites_Tareas
                              join proceso in db.SGI_Tarea_Generar_Expediente_Procesos on tarea.id_tramitetarea equals proceso.id_tramitetarea
                              join firma in db.wsEE_TareasDocumentos on proceso.id_paquete equals firma.id_paquete
                              join trah in db.SGI_Tramites_Tareas_HAB on proceso.id_tramitetarea equals trah.id_tramitetarea into trah_join
                              from trah in trah_join.DefaultIfEmpty()
                              where
                                    tarea.id_tarea == 615 && firma.firmado_en_SADE
                              group new { trah, tarea, firma } by new
                              {
                                  trah.id_solicitud,
                                  firma.numeroGEDO
                              } into g
                              select new
                              {
                                  Id_solicitud = (int?)g.Key.id_solicitud,
                                  numeroGEDO = g.Key.numeroGEDO
                              }
                                 )

                        on a.Id_solicitud equals firma_dispo.Id_solicitud into firma_dispo_join
                         from firma_dispo in firma_dispo_join.DefaultIfEmpty()

                         from tra in db.SGI_Tramites_Tareas
                         join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea
                         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         from trah in trah_join.DefaultIfEmpty()


                         where
                           a.Calif_Ini != null &&
                           a.Calif_Ini >= dtInicio && a.Calif_Ini <= dtFin &&
                           tar.id_circuito == IdCircuito
                         group new { a, b, c, f, g, h, i, firma_dispo } by new
                         {
                             a.Id_solicitud,
                             a.Calif_Ini,
                             Column1 = a.Calif_Ini,
                             a.Calif_fin,
                             Column2 = a.Calif_fin,
                             a.superficie,
                             b.Exp_ini,
                             Column3 = b.Exp_ini,
                             b.Exp_Cierre,
                             Column4 = b.Exp_Cierre,
                             b.Exp_Asignacion,
                             Column5 = b.Exp_Asignacion,
                             c.Rev_Ger_Cierre,
                             Column6 = c.Rev_Ger_Cierre,
                             c.Rev_Ger_ini,
                             Column7 = c.Rev_Ger_ini,
                             //d.BU_ini,
                             //d.BU_Cierre,
                             //Column8 = e.BU_ini,
                             //Column9 = e.BU_Cierre,
                             Column10 = a.Calif_Ini,
                             Column11 = a.Calif_Ini,
                             f.Cor_Solicitud,
                             g.FirmaDispo_Cierre,
                             g.FirmaDispo_Asign,
                             g.FirmaDispo_Ini,
                             h.Fecha_Cierre_Entrega,
                             h.Fecha_Ini_Entrega,
                             i.FirmaRDGHP_Inicio,
                             i.FirmaRDGHP_Cierre,
                             a.NroExpedienteSade
                         } into g
                         orderby
                           g.Key.Id_solicitud
                         select new ListadoVidaHabilitacion2
                         {
                             Id_solicitud = (System.Int32?)g.Key.Id_solicitud,
                             superficie = (System.Decimal?)g.Key.superficie,
                             Fecha_inicio_ASIGNACION_calif = DbFunctions.TruncateTime(g.Key.Calif_Ini),
                             Hora_inicio_ASIGNACION_calif = DbFunctions.CreateTime(g.Key.Calif_Ini.Hour, g.Key.Calif_Ini.Minute, g.Key.Calif_Ini.Second),
                             Fecha_fin_calif = DbFunctions.TruncateTime(g.Key.Calif_fin),
                             Hora_fin_calif = DbFunctions.CreateTime(g.Key.Calif_fin.Value.Hour, g.Key.Calif_fin.Value.Minute, g.Key.Calif_fin.Value.Second),
                             Fecha_Inicio_Exp = DbFunctions.TruncateTime(g.Key.Exp_ini),
                             Hora_Inicio_Exp = DbFunctions.CreateTime(g.Key.Exp_ini.Hour, g.Key.Exp_ini.Minute, g.Key.Exp_ini.Second),
                             Fecha_Asignacion_Exp = DbFunctions.TruncateTime(g.Key.Exp_Asignacion),
                             Hora_Asignacion_Exp = g.Key.Exp_Asignacion.HasValue ? DbFunctions.CreateTime(g.Key.Exp_Asignacion.Value.Hour, g.Key.Exp_Asignacion.Value.Minute, g.Key.Exp_Asignacion.Value.Second) : null,
                             Fecha_Cierre_Exp = DbFunctions.TruncateTime(g.Key.Exp_Cierre),
                             Hora_Cierre_Exp = g.Key.Exp_Cierre.HasValue ? DbFunctions.CreateTime(g.Key.Exp_Cierre.Value.Hour, g.Key.Exp_Cierre.Value.Minute, g.Key.Exp_Cierre.Value.Second) : null,
                             Fecha_Inicio_Rev_Ger = DbFunctions.TruncateTime(g.Key.Rev_Ger_ini),
                             Hora_Inicio_Rev_Ger = DbFunctions.CreateTime(g.Key.Rev_Ger_ini.Hour, g.Key.Rev_Ger_ini.Minute, g.Key.Rev_Ger_ini.Second),
                             Fecha_Cierre_Rev_Ger = DbFunctions.TruncateTime(g.Key.Rev_Ger_Cierre),
                             Hora_Cierre_Rev_Ger = g.Key.Rev_Ger_Cierre.HasValue ? DbFunctions.CreateTime(g.Key.Rev_Ger_Cierre.Value.Hour, g.Key.Rev_Ger_Cierre.Value.Minute, g.Key.Rev_Ger_Cierre.Value.Second) : null,
                             //Fecha_Gen_BU_ini = g.Key.BU_ini.Day + "/" + g.Key.BU_ini.Month + "/" + g.Key.BU_ini.Year,
                             //Fecha_Gen_BU_cierre = g.Key.BU_Cierre.HasValue ? g.Key.BU_Cierre.Value.Day + "/" + g.Key.BU_Cierre.Value.Month + "/" + g.Key.BU_Cierre.Value.Year : "",
                             //Fecha_Rev_BU_ini = g.Key.Column8.Day + "/" + g.Key.Column8.Month + "/" + g.Key.Column8.Year,
                             //Fecha_Rev_BU_cierre = g.Key.Column9.HasValue ? g.Key.Column9.Value.Day + "/" + g.Key.Column9.Value.Month + "/" + g.Key.Column9.Value.Year : "",
                             observado_alguna_vez = g.Key.Cor_Solicitud.Day + "/" + g.Key.Cor_Solicitud.Month + "/" + g.Key.Cor_Solicitud.Year,
                             Fecha_Cierre_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Cierre),
                             Fecha_Asign_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Asign),
                             Fecha_Ini_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Ini),
                             Fecha_Cierre_Entrega = DbFunctions.TruncateTime(g.Key.Fecha_Cierre_Entrega),
                             Fecha_Ini_Entrega = DbFunctions.TruncateTime(g.Key.Fecha_Ini_Entrega),
                             FirmaRDGHP_Inicio = DbFunctions.TruncateTime(g.Key.FirmaRDGHP_Inicio),
                             FirmaRDGHP_Cierre = DbFunctions.TruncateTime(g.Key.FirmaRDGHP_Cierre),
                             NroExpedienteSade = g.Key.NroExpedienteSade
                         });

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <param name="IdCircuito"></param>
        /// <returns></returns>
        public IQueryable<ListadoVidaHabilitacion2> SolicitudesFechayCircuitoSCP300000(DateTime dtInicio, DateTime dtFin, int IdCircuito)
        {
            var query = (from a in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea
                                  join ssit in db.SSIT_Solicitudes on trah.id_solicitud equals ssit.id_solicitud
                                  join encsol in db.Encomienda_SSIT_Solicitudes on ssit.id_solicitud equals encsol.id_solicitud
                                  join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                                  join daenc in db.Encomienda_DatosLocal on enc.id_encomienda equals daenc.id_encomienda

                                  where
                                    tra.id_tarea == 401 || tra.id_tarea == 402
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Calif_Ini = tra.FechaInicio_tramitetarea,
                                      Calif_fin = tra.FechaCierre_tramitetarea,
                                      superficie = (System.Decimal?)(daenc.superficie_cubierta_dl + daenc.superficie_descubierta_dl),
                                      NroExpedienteSade = ssit.NroExpedienteSade,
                                      CircuitoOrigen = ssit.circuito_origen
                                  }))
                         join b in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 407
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Exp_ini = tra.FechaInicio_tramitetarea,
                                      Exp_Cierre = tra.FechaCierre_tramitetarea,
                                      Exp_Asignacion = tra.FechaAsignacion_tramtietarea
                                  })) on a.Id_solicitud equals b.Id_solicitud into b_join
                         from b in b_join.DefaultIfEmpty()
                         join c in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 405
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Rev_Ger_Cierre = tra.FechaCierre_tramitetarea,
                                      Rev_Ger_ini = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals c.Id_solicitud into c_join
                         from c in c_join.DefaultIfEmpty()
                         //join d in
                         //    (
                         //        (from tra in db.SGI_Tramites_Tareas
                         //         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         //         from trah in trah_join.DefaultIfEmpty()
                         //         where
                         //           tra.id_tarea == 26
                         //         select new
                         //         {
                         //             Id_solicitud = (System.Int32?)trah.id_solicitud,
                         //             BU_ini = tra.FechaInicio_tramitetarea,
                         //             BU_Cierre = tra.FechaCierre_tramitetarea
                         //         })) on a.Id_solicitud equals d.Id_solicitud into d_join
                         //from d in d_join.DefaultIfEmpty()
                         //join e in
                         //    (
                         //        (from tra in db.SGI_Tramites_Tareas
                         //         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         //         from trah in trah_join.DefaultIfEmpty()
                         //         where
                         //           tra.id_tarea == 21
                         //         select new
                         //         {
                         //             Id_solicitud = (System.Int32?)trah.id_solicitud,
                         //             BU_Cierre = tra.FechaCierre_tramitetarea,
                         //             BU_ini = tra.FechaInicio_tramitetarea
                         //         })) on a.Id_solicitud equals e.Id_solicitud into e_join
                         //from e in e_join.DefaultIfEmpty()
                         join f in
                             (
                                 ((from tra in db.SGI_Tramites_Tareas
                                   join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                   from trah in trah_join.DefaultIfEmpty()
                                   where
                                     tra.id_tarea == 411
                                   select new
                                   {
                                       Id_solicitud = (System.Int32?)trah.id_solicitud,
                                       Cor_Solicitud = tra.FechaInicio_tramitetarea
                                   }).Distinct())) on a.Id_solicitud equals f.Id_solicitud into f_join
                         from f in f_join.DefaultIfEmpty()
                         join g in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 408
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      FirmaDispo_Ini = tra.FechaInicio_tramitetarea,
                                      FirmaDispo_Cierre = tra.FechaCierre_tramitetarea,
                                      FirmaDispo_Asign = tra.FechaAsignacion_tramtietarea
                                  })) on a.Id_solicitud equals g.Id_solicitud into g_join
                         from g in g_join.DefaultIfEmpty()
                         join h in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 410
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      Fecha_Ini_Entrega = tra.FechaInicio_tramitetarea,
                                      Fecha_Cierre_Entrega = tra.FechaCierre_tramitetarea
                                  })) on a.Id_solicitud equals h.Id_solicitud into h_join
                         from h in h_join.DefaultIfEmpty()
                         join i in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == 406
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)trah.id_solicitud,
                                      FirmaRDGHP_Cierre = tra.FechaCierre_tramitetarea,
                                      FirmaRDGHP_Inicio = tra.FechaInicio_tramitetarea
                                  })) on a.Id_solicitud equals i.Id_solicitud into i_join
                         from i in i_join.DefaultIfEmpty()

                         join firma_dispo in
                             (
                              from tarea in db.SGI_Tramites_Tareas
                              join proceso in db.SGI_Tarea_Generar_Expediente_Procesos on tarea.id_tramitetarea equals proceso.id_tramitetarea
                              join firma in db.wsEE_TareasDocumentos on proceso.id_paquete equals firma.id_paquete
                              join trah in db.SGI_Tramites_Tareas_HAB on proceso.id_tramitetarea equals trah.id_tramitetarea into trah_join
                              from trah in trah_join.DefaultIfEmpty()
                              where
                                    tarea.id_tarea == 408 && firma.firmado_en_SADE
                              group new { trah, tarea, firma } by new
                              {
                                  trah.id_solicitud,
                                  firma.numeroGEDO
                              } into g
                              select new
                              {
                                  Id_solicitud = (int?)g.Key.id_solicitud,
                                  numeroGEDO = g.Key.numeroGEDO
                              }
                                 )

                        on a.Id_solicitud equals firma_dispo.Id_solicitud into firma_dispo_join
                         from firma_dispo in firma_dispo_join.DefaultIfEmpty()

                         from tra in db.SGI_Tramites_Tareas
                         join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea
                         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         from trah in trah_join.DefaultIfEmpty()


                         where
                           a.Calif_Ini != null &&
                           a.Calif_Ini >= dtInicio && a.Calif_Ini <= dtFin &&
                           tar.id_circuito == IdCircuito
                         group new { a, b, c, f, g, h, i, firma_dispo } by new
                         {
                             a.Id_solicitud,
                             a.Calif_Ini,
                             Column1 = a.Calif_Ini,
                             a.Calif_fin,
                             Column2 = a.Calif_fin,
                             a.superficie,
                             b.Exp_ini,
                             Column3 = b.Exp_ini,
                             b.Exp_Cierre,
                             Column4 = b.Exp_Cierre,
                             b.Exp_Asignacion,
                             Column5 = b.Exp_Asignacion,
                             c.Rev_Ger_Cierre,
                             Column6 = c.Rev_Ger_Cierre,
                             c.Rev_Ger_ini,
                             Column7 = c.Rev_Ger_ini,
                             //d.BU_ini,
                             //d.BU_Cierre,
                             //Column8 = e.BU_ini,
                             //Column9 = e.BU_Cierre,
                             Column10 = a.Calif_Ini,
                             Column11 = a.Calif_Ini,
                             f.Cor_Solicitud,
                             g.FirmaDispo_Cierre,
                             g.FirmaDispo_Asign,
                             g.FirmaDispo_Ini,
                             h.Fecha_Cierre_Entrega,
                             h.Fecha_Ini_Entrega,
                             i.FirmaRDGHP_Inicio,
                             i.FirmaRDGHP_Cierre,
                             a.NroExpedienteSade,
                             a.CircuitoOrigen
                         } into g
                         orderby
                           g.Key.Id_solicitud
                         select new ListadoVidaHabilitacion2
                         {
                             Id_solicitud = (System.Int32?)g.Key.Id_solicitud,
                             superficie = (System.Decimal?)g.Key.superficie,
                             Fecha_inicio_ASIGNACION_calif = DbFunctions.TruncateTime(g.Key.Calif_Ini),
                             Hora_inicio_ASIGNACION_calif = DbFunctions.CreateTime(g.Key.Calif_Ini.Hour, g.Key.Calif_Ini.Minute, g.Key.Calif_Ini.Second),
                             Fecha_fin_calif = DbFunctions.TruncateTime(g.Key.Calif_fin),
                             Hora_fin_calif = DbFunctions.CreateTime(g.Key.Calif_fin.Value.Hour, g.Key.Calif_fin.Value.Minute, g.Key.Calif_fin.Value.Second),
                             Fecha_Inicio_Exp = DbFunctions.TruncateTime(g.Key.Exp_ini),
                             Hora_Inicio_Exp = DbFunctions.CreateTime(g.Key.Exp_ini.Hour, g.Key.Exp_ini.Minute, g.Key.Exp_ini.Second),
                             Fecha_Asignacion_Exp = DbFunctions.TruncateTime(g.Key.Exp_Asignacion),
                             Hora_Asignacion_Exp = g.Key.Exp_Asignacion.HasValue ? DbFunctions.CreateTime(g.Key.Exp_Asignacion.Value.Hour, g.Key.Exp_Asignacion.Value.Minute, g.Key.Exp_Asignacion.Value.Second) : null,
                             Fecha_Cierre_Exp = DbFunctions.TruncateTime(g.Key.Exp_Cierre),
                             Hora_Cierre_Exp = g.Key.Exp_Cierre.HasValue ? DbFunctions.CreateTime(g.Key.Exp_Cierre.Value.Hour, g.Key.Exp_Cierre.Value.Minute, g.Key.Exp_Cierre.Value.Second) : null,
                             Fecha_Inicio_Rev_Ger = DbFunctions.TruncateTime(g.Key.Rev_Ger_ini),
                             Hora_Inicio_Rev_Ger = DbFunctions.CreateTime(g.Key.Rev_Ger_ini.Hour, g.Key.Rev_Ger_ini.Minute, g.Key.Rev_Ger_ini.Second),
                             Fecha_Cierre_Rev_Ger = DbFunctions.TruncateTime(g.Key.Rev_Ger_Cierre),
                             Hora_Cierre_Rev_Ger = g.Key.Rev_Ger_Cierre.HasValue ? DbFunctions.CreateTime(g.Key.Rev_Ger_Cierre.Value.Hour, g.Key.Rev_Ger_Cierre.Value.Minute, g.Key.Rev_Ger_Cierre.Value.Second) : null,
                             //Fecha_Gen_BU_ini = g.Key.BU_ini.Day + "/" + g.Key.BU_ini.Month + "/" + g.Key.BU_ini.Year,
                             //Fecha_Gen_BU_cierre = g.Key.BU_Cierre.HasValue ? g.Key.BU_Cierre.Value.Day + "/" + g.Key.BU_Cierre.Value.Month + "/" + g.Key.BU_Cierre.Value.Year : "",
                             //Fecha_Rev_BU_ini = g.Key.Column8.Day + "/" + g.Key.Column8.Month + "/" + g.Key.Column8.Year,
                             //Fecha_Rev_BU_cierre = g.Key.Column9.HasValue ? g.Key.Column9.Value.Day + "/" + g.Key.Column9.Value.Month + "/" + g.Key.Column9.Value.Year : "",
                             observado_alguna_vez = g.Key.Cor_Solicitud.Day + "/" + g.Key.Cor_Solicitud.Month + "/" + g.Key.Cor_Solicitud.Year,
                             Fecha_Cierre_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Cierre),
                             Fecha_Asign_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Asign),
                             Fecha_Ini_Revision = DbFunctions.TruncateTime(g.Key.FirmaDispo_Ini),
                             Fecha_Cierre_Entrega = DbFunctions.TruncateTime(g.Key.Fecha_Cierre_Entrega),
                             Fecha_Ini_Entrega = DbFunctions.TruncateTime(g.Key.Fecha_Ini_Entrega),
                             FirmaRDGHP_Inicio = DbFunctions.TruncateTime(g.Key.FirmaRDGHP_Inicio),
                             FirmaRDGHP_Cierre = DbFunctions.TruncateTime(g.Key.FirmaRDGHP_Cierre),
                             NroExpedienteSade = g.Key.NroExpedienteSade,
                             CircuitoOrigen=g.Key.CircuitoOrigen
                         });

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<UltimaRevisionBis> Hoja5(int[] tareas1, int tarea2, int tarea3, int Subgerente, int Gerente)
        {
            
            var query = (
            from a in
                (
                    (from trah in db.SGI_Tramites_Tareas_HAB
                     where
                       (tareas1).Contains(trah.SGI_Tramites_Tareas.id_tarea)
                     select new
                     {
                         trah.id_solicitud,
                         trah.SSIT_Solicitudes.circuito_origen,
                         Calif_Ini = (System.DateTime?)trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea,
                         Id_tarea = (System.Int32?)trah.SGI_Tramites_Tareas.id_tarea
                     }))
            join b in
                (
                    (from trah in db.SGI_Tramites_Tareas_HAB
                     where
                       trah.SGI_Tramites_Tareas.id_tarea == tarea2
                     group new { trah, trah.SGI_Tramites_Tareas } by new
                     {
                         trah.id_solicitud
                     } into g
                     select new
                     {
                         Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                         rhyp_ini = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                     })) on a.id_solicitud equals b.Id_solicitud into b_join
            from b in b_join.DefaultIfEmpty()
            join c in
                (
                    (from trah in db.SGI_Tramites_Tareas_HAB
                     where
                       trah.SGI_Tramites_Tareas.id_tarea == tarea3
                     group new { trah, trah.SGI_Tramites_Tareas } by new
                     {
                         trah.id_solicitud
                     } into g
                     select new
                     {
                         Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                         obs_ini = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                     })) on a.id_solicitud equals c.Id_solicitud into c_join
            from c in c_join.DefaultIfEmpty()
            group new { a, b, c } by new
            {
                a.id_solicitud,
                a.Calif_Ini,
                a.Id_tarea,
                b.rhyp_ini,
                c.obs_ini,
                a.circuito_origen
            } into g
            orderby
              g.Key.id_solicitud
            select new UltimaRevisionBis
            {
                solicitud = (System.Int32?)g.Key.id_solicitud,
                Fecha_Inicio_Asignacion_Calificador = (System.DateTime?)g.Key.Calif_Ini,
                Asignacion_Calificador = g.Key.Id_tarea == Subgerente ? "Subgerente" : g.Key.Id_tarea == Gerente ? "Gerente" : null,
                Fecha_inicio_ULTIMA_Revision_HyP = (System.DateTime?)g.Key.rhyp_ini, 
                Observado = g.Key.obs_ini != null ? "Si" : g.Key.obs_ini == null ? "No" : null,
                CircuitoOrigen=g.Key.circuito_origen
            }).OrderBy(p => p.solicitud);
            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<UltimaRevision> Hoja5(int IdTarea1, int IdTarea2, int IdTarea3)
        {
            var query = (from a in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == IdTarea1
                                  select new
                                  {
                                      trah.id_solicitud,
                                      inicio_tarea = (System.DateTime?)trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea,
                                      Id_tarea = (System.Int32?)trah.SGI_Tramites_Tareas.id_tarea
                                  }))
                         join b in
                             (
                                 (from tra in db.SGI_Tramites_Tareas
                                  join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                                  from trah in trah_join.DefaultIfEmpty()
                                  where
                                    tra.id_tarea == IdTarea2
                                  group new { trah, tra } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      rhyp_ini = (System.DateTime?)g.Max(p => p.tra.FechaInicio_tramitetarea)
                                  })) on a.id_solicitud equals b.Id_solicitud into b_join
                         from b in b_join.DefaultIfEmpty()
                         join c in
                             (
                                 (from trah in db.SGI_Tramites_Tareas_HAB
                                  where
                                    trah.SGI_Tramites_Tareas.id_tarea == IdTarea3
                                  group new { trah, trah.SGI_Tramites_Tareas } by new
                                  {
                                      trah.id_solicitud
                                  } into g
                                  select new
                                  {
                                      Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                                      obs_ini = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                                  })) on a.id_solicitud equals c.Id_solicitud into c_join
                         from c in c_join.DefaultIfEmpty()
                         group new { a, b, c } by new
                         {
                             a.id_solicitud,
                             a.inicio_tarea,
                             a.Id_tarea,
                             b.rhyp_ini,
                             c.obs_ini
                         } into g
                         orderby
                           g.Key.id_solicitud
                         select new UltimaRevision
                         {
                             solicitud = (System.Int32?)g.Key.id_solicitud,
                             Fecha_Inicio_Tarea = (System.DateTime?)g.Key.inicio_tarea,
                             Asignacion_Calificador = g.Key.Id_tarea == 501 ? "Subgerente" : null,
                             Fecha_inicio_ULTIMA_Revision_HyP = (System.DateTime?)g.Key.rhyp_ini,
                             Observado = g.Key.obs_ini != null ? "Si" : g.Key.obs_ini == null ? "No" : null
                         });

            return query;
        }
        public IQueryable<UltimaRevisionHoja6> Hoja6(int IdTarea1, int IdTarea2, int IdTarea3, int IdTarea4, int IdTarea5, int Subgerente)
        {
            var query = (
                from a in
                    (
                     (from trah in db.SGI_Tramites_Tareas_HAB
                      where
                        trah.SGI_Tramites_Tareas.id_tarea == IdTarea1
                      select new
                      {
                          trah.id_solicitud,
                          inicio_tarea = (System.DateTime?)trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea,
                          Id_tarea = (System.Int32?)trah.SGI_Tramites_Tareas.id_tarea
                      }))
                join b in
                    (
                        (from trah in db.SGI_Tramites_Tareas_HAB
                         where
                           trah.SGI_Tramites_Tareas.id_tarea == IdTarea2
                         group new { trah, trah.SGI_Tramites_Tareas } by new
                         {
                             trah.id_solicitud
                         } into g
                         select new
                         {
                             Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                             rhyp_ini = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                         })) on a.id_solicitud equals b.Id_solicitud into b_join
                from b in b_join.DefaultIfEmpty()
                join c in
                    (
                        (from trah in db.SGI_Tramites_Tareas_HAB
                         where
                           trah.SGI_Tramites_Tareas.id_tarea == IdTarea3
                         group new { trah, trah.SGI_Tramites_Tareas } by new
                         {
                             trah.id_solicitud
                         } into g
                         select new
                         {
                             Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                             obs_ini = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                         })) on a.id_solicitud equals c.Id_solicitud into c_join
                from c in c_join.DefaultIfEmpty()
                join d in
                    (
                        (from tra in db.SGI_Tramites_Tareas
                         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         from trah in trah_join.DefaultIfEmpty()
                         where
                           tra.id_tarea == IdTarea4
                         select new
                         {
                             Id_solicitud = (System.Int32?)trah.id_solicitud,
                             rfd_Cierre = tra.FechaCierre_tramitetarea,
                             rfd_Asignacion = tra.FechaAsignacion_tramtietarea
                         })) on a.id_solicitud equals d.Id_solicitud into d_join
                from d in d_join.DefaultIfEmpty()
                join e in
                    (
                        (from tra in db.SGI_Tramites_Tareas
                         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                         from trah in trah_join.DefaultIfEmpty()
                         where
                           tra.id_tarea == IdTarea5
                         select new
                         {
                             Id_solicitud = (System.Int32?)trah.id_solicitud,
                             ee_ini = tra.FechaInicio_tramitetarea,
                             ee_Cierre = tra.FechaCierre_tramitetarea,
                             ee_Asignacion = tra.FechaAsignacion_tramtietarea
                         })) on a.id_solicitud equals e.Id_solicitud into e_join
                from e in e_join.DefaultIfEmpty()
                group new { a, b, d, c, e } by new
                {
                    a.id_solicitud,
                    a.inicio_tarea,
                    a.Id_tarea,
                    b.rhyp_ini,
                    d.rfd_Cierre,
                    c.obs_ini,
                    e.ee_Asignacion,
                    e.ee_Cierre,
                    d.rfd_Asignacion
                } into g
                orderby
                  g.Key.id_solicitud

                select new UltimaRevisionHoja6
                {
                    solicitud = (System.Int32?)g.Key.id_solicitud,
                    Fecha_Inicio_Tarea = (System.DateTime?)g.Key.inicio_tarea,
                    Asignacion_Calificador = g.Key.Id_tarea == Subgerente ? "Subgerente" : null,
                    Fecha_inicio_ULTIMA_Revision_HyP = (System.DateTime?)g.Key.rhyp_ini,
                    rfd_Cierre = (System.DateTime?)g.Key.rfd_Cierre,
                    Observado = g.Key.obs_ini != null ? "Si" : g.Key.obs_ini == null ? "No" : null,
                    Dif_EE_asig_cierre = DbFunctions.DiffDays(g.Key.ee_Asignacion, g.Key.ee_Cierre) - (2 * (DbFunctions.DiffDays(g.Key.ee_Asignacion, g.Key.ee_Cierre) / 7)),
                    Dif_RFD_asig_cierre = DbFunctions.DiffDays(g.Key.rfd_Asignacion, g.Key.rfd_Cierre) - (2 * (DbFunctions.DiffDays(g.Key.rfd_Asignacion, g.Key.rfd_Cierre) / 7))
                });

            return query.OrderBy(p => p.solicitud);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<UltimaRevisionHoja6Bis> Hoja6(int[] tareas1, int IdTarea2, int IdTarea3, int IdTarea4, int IdTarea5, int Subgerente, int Gerente)
        {
            var query = (
            from a in
            (
             (from trah in db.SGI_Tramites_Tareas_HAB
              where
                (tareas1).Contains(trah.SGI_Tramites_Tareas.id_tarea)
              select new
              {
                  trah.id_solicitud,
                  trah.SSIT_Solicitudes.circuito_origen,
                  Calif_Ini = (System.DateTime?)trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea,
                  Id_tarea = (System.Int32?)trah.SGI_Tramites_Tareas.id_tarea
              }))
             join b in
                 (
                     (from trah in db.SGI_Tramites_Tareas_HAB
                      where
                        trah.SGI_Tramites_Tareas.id_tarea == IdTarea2
                      group new { trah, trah.SGI_Tramites_Tareas } by new
                      {
                          trah.id_solicitud
                      } into g
                      select new
                      {
                          Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                          rhyp_ini = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                      })) on a.id_solicitud equals b.Id_solicitud into b_join
             from b in b_join.DefaultIfEmpty()
             join c in
                 (
                     (from trah in db.SGI_Tramites_Tareas_HAB
                      where
                        trah.SGI_Tramites_Tareas.id_tarea == IdTarea3
                      group new { trah, trah.SGI_Tramites_Tareas } by new
                      {
                          trah.id_solicitud
                      } into g
                      select new
                      {
                          Id_solicitud = (System.Int32?)g.Key.id_solicitud,
                          obs_ini = (System.DateTime?)g.Max(p => p.trah.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                      })) on a.id_solicitud equals c.Id_solicitud into c_join
             from c in c_join.DefaultIfEmpty()
             join d in
                 (
                     (from tra in db.SGI_Tramites_Tareas
                      join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                      from trah in trah_join.DefaultIfEmpty()
                      where
                        tra.id_tarea == IdTarea4
                      select new
                      {
                          Id_solicitud = (System.Int32?)trah.id_solicitud,
                          rfd_Cierre = tra.FechaCierre_tramitetarea,
                          rfd_Asignacion = tra.FechaAsignacion_tramtietarea
                      })) on a.id_solicitud equals d.Id_solicitud into d_join
             from d in d_join.DefaultIfEmpty()
             join e in
                 (
                     (from tra in db.SGI_Tramites_Tareas
                      join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trah_join
                      from trah in trah_join.DefaultIfEmpty()
                      where
                        tra.id_tarea == IdTarea5
                      select new
                      {
                          Id_solicitud = (System.Int32?)trah.id_solicitud,
                          ee_ini = tra.FechaInicio_tramitetarea,
                          ee_Cierre = tra.FechaCierre_tramitetarea,
                          ee_Asignacion = tra.FechaAsignacion_tramtietarea
                      })) on a.id_solicitud equals e.Id_solicitud into e_join
             from e in e_join.DefaultIfEmpty()
             group new { a, b, d, c, e } by new
             {
                 a.id_solicitud,
                 a.Calif_Ini,
                 a.Id_tarea,
                 b.rhyp_ini,
                 d.rfd_Cierre,
                 c.obs_ini,
                 e.ee_Asignacion,
                 e.ee_Cierre,                 
                 d.rfd_Asignacion,
                 a.circuito_origen               
             } into g
             orderby
               g.Key.id_solicitud

            select new UltimaRevisionHoja6Bis
             {
                 solicitud = (System.Int32?)g.Key.id_solicitud,
                 Fecha_Inicio_Asignacion_Calificador = (System.DateTime?)g.Key.Calif_Ini,
                 Asignacion_Calificador = g.Key.Id_tarea == Subgerente ? "Subgerente" : g.Key.Id_tarea == Gerente ? "Gerente" : null,
                 Fecha_inicio_ULTIMA_Revision_HyP = (System.DateTime?)g.Key.rhyp_ini,
                 rfd_Cierre = (System.DateTime?)g.Key.rfd_Cierre,
                 Observado = g.Key.obs_ini != null ? "Si" : g.Key.obs_ini == null ? "No" : null,
                 Dif_EE_asig_cierre = DbFunctions.DiffDays(g.Key.ee_Asignacion, g.Key.ee_Cierre) - (2 * (DbFunctions.DiffDays(g.Key.ee_Asignacion, g.Key.ee_Cierre) / 7)),
                 Dif_RFD_asig_cierre = DbFunctions.DiffDays(g.Key.rfd_Asignacion, g.Key.rfd_Cierre) - (2 * (DbFunctions.DiffDays(g.Key.rfd_Asignacion, g.Key.rfd_Cierre) / 7)),
                 CircuitoOrigen=g.Key.circuito_origen
             });

            return query;
        }
    }
}