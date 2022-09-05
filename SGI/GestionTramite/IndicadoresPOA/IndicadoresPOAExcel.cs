using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.GestionTramite
{
    public class ResultadosPOAExcel
    {

        public string Area { get; set; }
        public string Circuito { get; set; }
        public int ID { get; set; }
        public string NombreTarea { get; set; }
        public string NombreResultado { get; set; }
        public string ProximaTarea { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaCierre { get; set; }
    }

    public class IndicadoresPOAExcel
    {
        public IQueryable<ResultadosPOAExcel> GetSSPTotalIngresados(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            //revisado
            var query =

            (from solicitudes in (
                (
                     //Ingresados por primera vez:
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into pleft_tarprox
                    from tarprox in pleft_tarprox.DefaultIfEmpty()
                    where
                      tar.id_circuito == (int) Constants.ENG_Circuitos.SSP &&
                      tar.id_tarea == (int) Constants.ENG_Tareas.SSP_Asignar_Calificador &&
                      trah.FechaInicio_tramitetarea >= dtInicio && trah.FechaInicio_tramitetarea <= dtFin &&
                      (tarprox == null || trah.id_proxima_tarea == (int)Constants.ENG_Tareas.SSP_Calificar)
                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                     // Reingresados externos (producto de subsanación de observaciones):
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea
                    where
                      tar.id_circuito == (int)Constants.ENG_Circuitos.SSP &&
                      tar.id_tarea == (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      trah.id_proxima_tarea == (int)Constants.ENG_Tareas.SSP_Calificar
                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                     //Reingresados internos (producto de devoluciones del S.G.O o G.O al calificador):
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea
                    where
                      tar.id_circuito == (int)Constants.ENG_Circuitos.SSP &&
                      (new int[] { (int)Constants.ENG_Tareas.SSP_Revision_SubGerente, (int)Constants.ENG_Tareas.SSP_Revision_Gerente }).Contains(tar.id_tarea) &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      trah.id_proxima_tarea == (int) Constants.ENG_Tareas.SSP_Calificar
                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ))
             select solicitudes);

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetSSPTotalResueltos(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            //revisado
            var query =

              (from solicitudes in (

                   (from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea
                    where
                      tar.id_circuito == (int) Constants.ENG_Circuitos.SSP &&
                      tar.id_tarea == (int)Constants.ENG_Tareas.SSP_Calificar &&
                      (new int[] { (int)Constants.ENG_ResultadoTarea.Aprobado, (int)Constants.ENG_ResultadoTarea.Calificar_Pedir_Rectificacion}).Contains(trah.id_resultado) &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      (new int[] { (int)Constants.ENG_Tareas.SSP_Revision_SubGerente, (int)Constants.ENG_Tareas.SSP_Revision_Gerente, (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud}).Contains(trah.id_proxima_tarea.Value)
                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                    ))
               select solicitudes);

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetSCPTotalIngresados(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =
            (from solicitudes in (
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 2 &&
                      tar.id_tarea == 34 &&
                      trah.FechaInicio_tramitetarea >= dtInicio && trah.FechaInicio_tramitetarea <= dtFin &&
                      (tarprox == null || tarprox.id_tarea == (int) Constants.ENG_Tareas.SCP_Calificar)
                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 2 &&
                      tar.id_tarea == 49 &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      tarprox.id_tarea == (int)Constants.ENG_Tareas.SCP_Calificar
                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 2 &&
                     (new int[] { 36, 37 }).Contains(tar.id_tarea) &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      tarprox.id_tarea == (int)Constants.ENG_Tareas.SCP_Calificar
                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ))
             select solicitudes);

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetSCPTotalResueltos(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =

                (from ttt in db.SGI_Tramites_Tareas_HAB
                 join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                 join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                 join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                 join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                 from result in result_join.DefaultIfEmpty()
                 join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                 from tarprox in tarprox_join.DefaultIfEmpty()
                 where
                   tar.id_circuito == (int) Constants.ENG_Circuitos.SCP &&
                   tar.id_tarea == 35 &&
                   (new int[] { 19, 20 }).Contains(trah.id_resultado) &&
                   trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                   (new int[] { (int)Constants.ENG_Tareas.SCP_Revision_SubGerente, (int)Constants.ENG_Tareas.SCP_Revision_Gerente, (int)Constants.ENG_Tareas.SCP_Correccion_Solicitud }).Contains(trah.id_proxima_tarea.Value)
                 select new ResultadosPOAExcel
                 {
                     Area = "Simple",
                     Circuito = cir.nombre_circuito,
                     ID = ttt.id_solicitud,
                     NombreTarea = tar.nombre_tarea,
                     NombreResultado = result.nombre_resultado,
                     ProximaTarea = tarprox.nombre_tarea,
                     FechaInicio = trah.FechaInicio_tramitetarea,
                     FechaCierre = trah.FechaCierre_tramitetarea
                 });

            return query;

        }

        public IQueryable<ResultadosPOAExcel> GetEspecialTotalIngresados(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =
            (from solicitudes in (
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 3 &&
                      tar.id_tarea == 101 &&
                      trah.FechaInicio_tramitetarea >= dtInicio && trah.FechaInicio_tramitetarea <= dtFin &&
                      (tarprox == null || trah.id_proxima_tarea == (int)Constants.ENG_Tareas.ESP_Calificar_1 )
                    select new ResultadosPOAExcel
                    {
                        Area = "Especial",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 3 &&
                      tar.id_tarea == 120 &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      (trah.id_proxima_tarea == (int)Constants.ENG_Tareas.ESP_Calificar_1 || trah.id_proxima_tarea == (int)Constants.ENG_Tareas.ESP_Calificar_2)
                    select new ResultadosPOAExcel
                    {
                        Area = "Especial",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 3 &&
                      (new int[] { 104, 105, 122 }).Contains(tar.id_tarea) &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      trah.id_resultado == 22 &&
                      (trah.id_proxima_tarea == (int)Constants.ENG_Tareas.ESP_Calificar_1 || trah.id_proxima_tarea == (int)Constants.ENG_Tareas.ESP_Calificar_2)
                    select new ResultadosPOAExcel
                    {
                        Area = "Especial",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 3 &&
                      tar.id_tarea == 103 &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      (trah.id_proxima_tarea == (int)Constants.ENG_Tareas.ESP_Calificar_2)
                    select new ResultadosPOAExcel
                    {
                        Area = "Especial",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ))
             select solicitudes);


            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetEspecialTotalResueltos(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =
                (from ttt in db.SGI_Tramites_Tareas_HAB
                 join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                 join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                 join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                 join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                 from result in result_join.DefaultIfEmpty()
                 join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                 from tarprox in tarprox_join.DefaultIfEmpty()
                 where
                   tar.id_circuito == 3 &&
                   (new int[] { 102, 121 }).Contains(tar.id_tarea) &&
                   (new int[] { 19, 20, 54 }).Contains(trah.id_resultado) &&
                   trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin
                 select new ResultadosPOAExcel
                 {
                     Area = "Especial",
                     Circuito = cir.nombre_circuito,
                     ID = ttt.id_solicitud,
                     NombreTarea = tar.nombre_tarea,
                     NombreResultado = result.nombre_resultado,
                     ProximaTarea = tarprox.nombre_tarea,
                     FechaInicio = trah.FechaInicio_tramitetarea,
                     FechaCierre = trah.FechaCierre_tramitetarea
                 }
                );
            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetEsparcimientoTotalIngresados(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =
            (from solicitudes in (
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 6 &&
                      tar.id_tarea == 201 &&
                      trah.FechaInicio_tramitetarea >= dtInicio && trah.FechaInicio_tramitetarea <= dtFin &&
                      (tarprox == null || trah.id_proxima_tarea == (int)Constants.ENG_Tareas.ESPAR_Calificar_1 )
                    select new ResultadosPOAExcel
                    {
                        Area = "Especial",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 6 &&
                      tar.id_tarea == 222 &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      (trah.id_proxima_tarea == (int)Constants.ENG_Tareas.ESPAR_Calificar_1 || trah.id_proxima_tarea == (int)Constants.ENG_Tareas.ESPAR_Calificar_2)
                    select new ResultadosPOAExcel
                    {
                        Area = "Especial",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 6 &&
                      (new int[] { 205, 206, 212 }).Contains(tar.id_tarea) &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      ttt.SGI_Tramites_Tareas.id_resultado == 22 &&
                      (trah.id_proxima_tarea == (int)Constants.ENG_Tareas.ESPAR_Calificar_1 || trah.id_proxima_tarea == (int)Constants.ENG_Tareas.ESPAR_Calificar_2)
                    select new ResultadosPOAExcel
                    {
                        Area = "Especial",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 6 &&
                      tar.id_tarea == 203 &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      (trah.id_proxima_tarea == (int)Constants.ENG_Tareas.ESPAR_Calificar_2)
                    select new ResultadosPOAExcel
                    {
                        Area = "Especial",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ))
             select solicitudes);

            return query;
        }
        public IQueryable<ResultadosPOAExcel> GetEsparcimientoTotalResueltos(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query = (from ttt in db.SGI_Tramites_Tareas_HAB
                         join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                         join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                         join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                         join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                         from result in result_join.DefaultIfEmpty()
                         join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                         from tarprox in tarprox_join.DefaultIfEmpty()
                         where
               tar.id_circuito == 6 &&
               (new int[] { 202, 204 }).Contains(tar.id_tarea) &&
               (new int[] { 19, 20, 54 }).Contains(trah.id_resultado) &&
               trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin
                         select new ResultadosPOAExcel
                         {
                             Area = "Especial",
                             Circuito = cir.nombre_circuito,
                             ID = ttt.id_solicitud,
                             NombreTarea = tar.nombre_tarea,
                             NombreResultado = result.nombre_resultado,
                             ProximaTarea = tarprox.nombre_tarea,
                             FechaInicio = trah.FechaInicio_tramitetarea,
                             FechaCierre = trah.FechaCierre_tramitetarea
                         }
                            );

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetTransferenciaTotalIngresados(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =
            (from solicitudes in (
                (
                    from ttt in db.SGI_Tramites_Tareas_TRANSF
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 5 &&
                      tar.id_tarea == 61 &&
                      trah.FechaInicio_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      (tarprox == null || trah.id_proxima_tarea == (int)Constants.ENG_Tareas.TR_Calificar)
                    select new ResultadosPOAExcel
                    {
                        Area = "Especial",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                    from ttt in db.SGI_Tramites_Tareas_TRANSF
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 5 &&
                      tar.id_tarea == 60 &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      trah.id_proxima_tarea == (int)Constants.ENG_Tareas.TR_Calificar
                    select new ResultadosPOAExcel
                    {
                        Area = "Especial",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                    from ttt in db.SGI_Tramites_Tareas_TRANSF
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 5 &&
                      (new int[] { 63, 64 }).Contains(tar.id_tarea) &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      trah.id_proxima_tarea == (int)Constants.ENG_Tareas.TR_Calificar
                    select new ResultadosPOAExcel
                    {
                        Area = "Especial",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ))
             select solicitudes);

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetTransferenciaTotalResueltos(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query = (from ttt in db.SGI_Tramites_Tareas_TRANSF
                         join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                         join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                         join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                         join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                         from result in result_join.DefaultIfEmpty()
                         join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                         from tarprox in tarprox_join.DefaultIfEmpty()
                         where
                         tar.id_circuito == 5 && 
                         tar.id_tarea == 62 &&
                          (new int[] { 19, 20 }).Contains(trah.id_resultado) &&
                          trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                          (new int[] { (int)Constants.ENG_Tareas.TR_Revision_SubGerente, 
                                       (int)Constants.ENG_Tareas.TR_Revision_Gerente_1, 
                                       (int)Constants.ENG_Tareas.TR_Revision_Gerente_2,
                                       (int)Constants.ENG_Tareas.TR_Correccion_Solicitud}).Contains(trah.id_proxima_tarea.Value)
                         select new ResultadosPOAExcel
                         {
                             Area = "Especial",
                             Circuito = cir.nombre_circuito,
                             ID = ttt.id_solicitud,
                             NombreTarea = tar.nombre_tarea,
                             NombreResultado = result.nombre_resultado,
                             ProximaTarea = tarprox.nombre_tarea,
                             FechaInicio = trah.FechaInicio_tramitetarea,
                             FechaCierre = trah.FechaCierre_tramitetarea
                         });

            return query;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetSSP2TotalIngresados(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =
            (from solicitudes in (
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 11 &&
                      (new int[] { 301, 302 }).Contains(tar.id_tarea) &&
                      trah.FechaInicio_tramitetarea >= dtInicio && trah.FechaInicio_tramitetarea <= dtFin &&
                      (tarprox == null || trah.id_proxima_tarea == 303) 
                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                     //Reingresados externos (producto de subsanación de observaciones):
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 11 &&
                      tar.id_tarea == 311 &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      trah.id_proxima_tarea == 303
                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                     //Reingresados internos (producto de devoluciones del S.G.O o G.O o DGHyP al calificador):
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 11 &&
                      (new int[] { 304, 305, 306 }).Contains(tar.id_tarea) &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      (new int[] { 62, 56 }).Contains(trah.id_resultado) &&
                      trah.id_proxima_tarea == 303
                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ))
             select solicitudes);

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetSSP2TotalResueltos(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =
            (from ttt in db.SGI_Tramites_Tareas_HAB
             join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
             join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
             join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
             join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado
             join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
             from tarprox in tarprox_join.DefaultIfEmpty()
             where
               tar.id_circuito == 11 &&
               tar.id_tarea == 303 &&
               (new int[] { 19, 20, 60 }).Contains(trah.id_resultado) &&
               trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
               (new int[] { 304, 305, 311 }).Contains(trah.id_proxima_tarea.Value)
             select new ResultadosPOAExcel
             {
                 Area = "Simple",
                 Circuito = cir.nombre_circuito,
                 ID = ttt.id_solicitud,
                 NombreTarea = tar.nombre_tarea,
                 NombreResultado = result.nombre_resultado,
                 ProximaTarea = tarprox.nombre_tarea,
                 FechaInicio = trah.FechaInicio_tramitetarea,
                 FechaCierre = trah.FechaCierre_tramitetarea
             }
                           );

            return query;
        }
        public IQueryable<ResultadosPOAExcel> GetSCP2TotalIngresados(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =
            (from solicitudes in (
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 12 &&
                      (new int[] { 401, 402 }).Contains(tar.id_tarea) &&
                      trah.FechaInicio_tramitetarea >= dtInicio && trah.FechaInicio_tramitetarea <= dtFin &&
                      (tarprox == null || trah.id_proxima_tarea == 403) 

                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 12 &&
                      tar.id_tarea == 411 &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                      trah.id_proxima_tarea == 403
                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ).Concat
                (
                    from ttt in db.SGI_Tramites_Tareas_HAB
                    join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                    join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                    join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                    from result in result_join.DefaultIfEmpty()
                    join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                    from tarprox in tarprox_join.DefaultIfEmpty()
                    where
                      tar.id_circuito == 12 &&
                     (new int[] { 404, 405, 406 }).Contains(tar.id_tarea) &&
                      trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                       (new int[] { 56, 62 }).Contains(ttt.SGI_Tramites_Tareas.id_resultado) &&
                      trah.id_proxima_tarea == 403
                    select new ResultadosPOAExcel
                    {
                        Area = "Simple",
                        Circuito = cir.nombre_circuito,
                        ID = ttt.id_solicitud,
                        NombreTarea = tar.nombre_tarea,
                        NombreResultado = result.nombre_resultado,
                        ProximaTarea = tarprox.nombre_tarea,
                        FechaInicio = trah.FechaInicio_tramitetarea,
                        FechaCierre = trah.FechaCierre_tramitetarea
                    }
                ))
             select solicitudes);

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetSCP2TotalResueltos(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query = (from ttt in db.SGI_Tramites_Tareas_HAB
                         join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                         join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                         join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                         join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado
                         join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea
                         where
                           tar.id_circuito == 12 &&
                           tar.id_tarea == 403 &&
                           (new int[] { 19, 20,60 }).Contains(trah.id_resultado) &&
                           trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                           (new int[] { 404, 405, 411 }).Contains(trah.id_proxima_tarea.Value)

                         select new ResultadosPOAExcel
                         {
                             Area = "Simple",
                             Circuito = cir.nombre_circuito,
                             ID = ttt.id_solicitud,
                             NombreTarea = tar.nombre_tarea,
                             NombreResultado = result.nombre_resultado,
                             ProximaTarea = tarprox.nombre_tarea,
                             FechaInicio = trah.FechaInicio_tramitetarea,
                             FechaCierre = trah.FechaCierre_tramitetarea
                         });

            return query;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetIPTotalIngresados(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =
          (from solicitudes in
              (
                  from ttt in db.SGI_Tramites_Tareas_HAB
                  join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                  join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                  join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                  join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                  from result in result_join.DefaultIfEmpty()
                  join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                  from tarprox in tarprox_join.DefaultIfEmpty()
                  where
                    tar.id_circuito == 13 &&
                    tar.id_tarea == 501 &&
                    trah.FechaInicio_tramitetarea >= dtInicio && trah.FechaInicio_tramitetarea <= dtFin &&
                    (tarprox == null || trah.id_proxima_tarea == 502 || trah.id_proxima_tarea == 507) 
                  select new ResultadosPOAExcel
                  {
                      Area = "Especial",
                      Circuito = cir.nombre_circuito,
                      ID = ttt.id_solicitud,
                      NombreTarea = tar.nombre_tarea,
                      NombreResultado = result.nombre_resultado,
                      ProximaTarea = tarprox.nombre_tarea,
                      FechaInicio = trah.FechaInicio_tramitetarea,
                      FechaCierre = trah.FechaCierre_tramitetarea
                  }
              ).Concat
              (
                  from ttt in db.SGI_Tramites_Tareas_HAB
                  join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                  join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                  join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                  join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                  from result in result_join.DefaultIfEmpty()
                  join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                  from tarprox in tarprox_join.DefaultIfEmpty()
                  where
                    tar.id_circuito == 13 &&
                    tar.id_tarea == 518 &&
                    trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                    (new int[] { 502, 507}).Contains( trah.id_proxima_tarea.Value)
                  select new ResultadosPOAExcel
                  {
                      Area = "Especial",
                      Circuito = cir.nombre_circuito,
                      ID = ttt.id_solicitud,
                      NombreTarea = tar.nombre_tarea,
                      NombreResultado = result.nombre_resultado,
                      ProximaTarea = tarprox.nombre_tarea,
                      FechaInicio = trah.FechaInicio_tramitetarea,
                      FechaCierre = trah.FechaCierre_tramitetarea
                  }
              ).Concat
              (
                  from ttt in db.SGI_Tramites_Tareas_HAB
                  join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                  join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                  join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                  join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                  from result in result_join.DefaultIfEmpty()
                  join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                  from tarprox in tarprox_join.DefaultIfEmpty()
                  where
                    tar.id_circuito == 13 &&
                    (new int[] { 503,504, 508, 509 }).Contains(tar.id_tarea) &&
                    trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                    (new int[] { 502, 507 }).Contains(trah.id_proxima_tarea.Value)

                  select new ResultadosPOAExcel
                  {
                      Area = "Especial",
                      Circuito = cir.nombre_circuito,
                      ID = ttt.id_solicitud,
                      NombreTarea = tar.nombre_tarea,
                      NombreResultado = result.nombre_resultado,
                      ProximaTarea = tarprox.nombre_tarea,
                      FechaInicio = trah.FechaInicio_tramitetarea,
                      FechaCierre = trah.FechaCierre_tramitetarea
                  }
              ).Concat
               (
                   from ttt in db.SGI_Tramites_Tareas_HAB
                   join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                   join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                   join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                   join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                   from result in result_join.DefaultIfEmpty()
                   join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                   from tarprox in tarprox_join.DefaultIfEmpty()
                   where
                     tar.id_circuito == 13 &&
                     tar.id_tarea == 519 &&
                     trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                     (new int[] { 502, 507 }).Contains(trah.id_proxima_tarea.Value)
                   select new ResultadosPOAExcel
                   {
                       Area = "Especial",
                       Circuito = cir.nombre_circuito,
                       ID = ttt.id_solicitud,
                       NombreTarea = tar.nombre_tarea,
                       NombreResultado = result.nombre_resultado,
                       ProximaTarea = tarprox.nombre_tarea,
                       FechaInicio = trah.FechaInicio_tramitetarea,
                       FechaCierre = trah.FechaCierre_tramitetarea
                   }
               )
           select solicitudes);

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetIPTotalResueltos(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query = (from ttt in db.SGI_Tramites_Tareas_HAB
                         join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                         join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                         join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                         join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado
                         join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea
                         where
                           tar.id_circuito == 13 &&
                           (new int[] { 502, 507 }).Contains(tar.id_tarea) &&
                           (new int[] { 19, 20, 63, 64, 60 }).Contains(trah.id_resultado) &&
                           trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                           !(new int[] { 502, 507 }).Contains(trah.id_resultado)
                         select new ResultadosPOAExcel
                         {
                             Area = "Especial",
                             Circuito = cir.nombre_circuito,
                             ID = ttt.id_solicitud,
                             NombreTarea = tar.nombre_tarea,
                             NombreResultado = result.nombre_resultado,
                             ProximaTarea = tarprox.nombre_tarea,
                             FechaInicio = trah.FechaInicio_tramitetarea,
                             FechaCierre = trah.FechaCierre_tramitetarea
                         }
                        );
            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetHPTotalIngresados(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =
           (from solicitudes in (
               (
                   from ttt in db.SGI_Tramites_Tareas_HAB
                   join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                   join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                   join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                   join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                   from result in result_join.DefaultIfEmpty()
                   join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                   from tarprox in tarprox_join.DefaultIfEmpty()
                   where
                     tar.id_circuito == 14 &&
                     tar.id_tarea == 601 &&
                     trah.FechaInicio_tramitetarea >= dtInicio && trah.FechaInicio_tramitetarea <= dtFin &&
                     (tarprox == null || trah.id_proxima_tarea == 602 || trah.id_proxima_tarea == 607)
                   select new ResultadosPOAExcel
                   {
                       Area = "Especial",
                       Circuito = cir.nombre_circuito,
                       ID = ttt.id_solicitud,
                       NombreTarea = tar.nombre_tarea,
                       NombreResultado = result.nombre_resultado,
                       ProximaTarea = tarprox.nombre_tarea,
                       FechaInicio = trah.FechaInicio_tramitetarea,
                       FechaCierre = trah.FechaCierre_tramitetarea
                   }
               ).Concat
               (
                   from ttt in db.SGI_Tramites_Tareas_HAB
                   join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                   join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                   join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                   join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                   from result in result_join.DefaultIfEmpty()
                   join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                   from tarprox in tarprox_join.DefaultIfEmpty()
                   where
                     tar.id_circuito == 14 &&
                     tar.id_tarea == 618 &&
                     trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                     new int[] { 602, 607 }.Contains(tarprox.id_tarea)
                   select new ResultadosPOAExcel
                   {
                       Area = "Especial",
                       Circuito = cir.nombre_circuito,
                       ID = ttt.id_solicitud,
                       NombreTarea = tar.nombre_tarea,
                       NombreResultado = result.nombre_resultado,
                       ProximaTarea = tarprox.nombre_tarea,
                       FechaInicio = trah.FechaInicio_tramitetarea,
                       FechaCierre = trah.FechaCierre_tramitetarea
                   }
               ).Concat
               (
                   from ttt in db.SGI_Tramites_Tareas_HAB
                   join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                   join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                   join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                   join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                   from result in result_join.DefaultIfEmpty()
                   join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                   from tarprox in tarprox_join.DefaultIfEmpty()
                   where
                     tar.id_circuito == 14 &&
                     (new int[] { 603,604, 608, 609 }).Contains(tar.id_tarea) &&
                     trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                     new int[] { 602, 607 }.Contains(tarprox.id_tarea)
                   select new ResultadosPOAExcel
                   {
                       Area = "Especial",
                       Circuito = cir.nombre_circuito,
                       ID = ttt.id_solicitud,
                       NombreTarea = tar.nombre_tarea,
                       NombreResultado = result.nombre_resultado,
                       ProximaTarea = tarprox.nombre_tarea,
                       FechaInicio = trah.FechaInicio_tramitetarea,
                       FechaCierre = trah.FechaCierre_tramitetarea
                   }
               ).Concat
               (
                   from ttt in db.SGI_Tramites_Tareas_HAB
                   join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                   join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                   join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                   join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                   from result in result_join.DefaultIfEmpty()
                   join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea into tarprox_join
                   from tarprox in tarprox_join.DefaultIfEmpty()
                   where
                     tar.id_circuito == 14 &&
                     (new int[] { 619 }).Contains(tar.id_tarea) &&
                     trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                     new int[] { 602, 607 }.Contains(tarprox.id_tarea)
                   select new ResultadosPOAExcel
                   {
                       Area = "Especial",
                       Circuito = cir.nombre_circuito,
                       ID = ttt.id_solicitud,
                       NombreTarea = tar.nombre_tarea,
                       NombreResultado = result.nombre_resultado,
                       ProximaTarea = tarprox.nombre_tarea,
                       FechaInicio = trah.FechaInicio_tramitetarea,
                       FechaCierre = trah.FechaCierre_tramitetarea
                   }
               ))
            select solicitudes);

            return query;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dtInicio"></param>
        /// <param name="dtFin"></param>
        /// <returns></returns>
        public IQueryable<ResultadosPOAExcel> GetHPTotalResueltos(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query = (from ttt in db.SGI_Tramites_Tareas_HAB
                         join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                         join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                         join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                         join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado
                         join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea
                         where
                               tar.id_circuito == 14 &&
                               (new int[] { 602, 607 }).Contains(tar.id_tarea) &&
                               (new int[] { 19, 20, 63, 64, 60 }).Contains(trah.id_resultado) &&
                               trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                                !(new int[] { 602, 607 }).Contains(trah.id_resultado)
                         select new ResultadosPOAExcel
                         {
                             Area = "Especial",
                             Circuito = cir.nombre_circuito,
                             ID = ttt.id_solicitud,
                             NombreTarea = tar.nombre_tarea,
                             NombreResultado = result.nombre_resultado,
                             ProximaTarea = tarprox.nombre_tarea,
                             FechaInicio = trah.FechaInicio_tramitetarea,
                             FechaCierre = trah.FechaCierre_tramitetarea
                         }
                           );

            return query;
        }

        public IQueryable<ResultadosPOAExcel> GetSSP3TotalResueltos(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =
            (from ttt in db.SGI_Tramites_Tareas_HAB
             join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
             join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
             join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
             join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado
             join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea
             where
               tar.id_circuito == 15 &&
               tar.id_tarea == 701 &&
               trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin 
             select new ResultadosPOAExcel
             {
                 Area = "Simple",
                 Circuito = cir.nombre_circuito,
                 ID = ttt.id_solicitud,
                 NombreTarea = tar.nombre_tarea,
                 NombreResultado = result.nombre_resultado,
                 ProximaTarea = tarprox.nombre_tarea,
                 FechaInicio = trah.FechaInicio_tramitetarea,
                 FechaCierre = trah.FechaCierre_tramitetarea
             }
                           );

            return query;
        }
        public IQueryable<ResultadosPOAExcel> GetSSP3TotalIngresados(DGHP_Entities db, DateTime dtInicio, DateTime dtFin)
        {
            var query =
            (from solicitudes in
                 (
                     (
                     from ttt in db.SGI_Tramites_Tareas_HAB
                     join trah in db.SGI_Tramites_Tareas on ttt.id_tramitetarea equals trah.id_tramitetarea
                     join tar in db.ENG_Tareas on trah.id_tarea equals tar.id_tarea
                     join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                     join result in db.ENG_Resultados on trah.id_resultado equals result.id_resultado into result_join
                     from result in result_join.DefaultIfEmpty()
                     join tarprox in db.ENG_Tareas on trah.id_proxima_tarea equals tarprox.id_tarea
                     where
                       tar.id_circuito == 15 &&
                       tar.id_tarea == 700 &&
                       trah.FechaCierre_tramitetarea >= dtInicio && trah.FechaCierre_tramitetarea <= dtFin &&
                       trah.id_proxima_tarea == 701

                     select new ResultadosPOAExcel
                     {
                         Area = "Simple",
                         Circuito = cir.nombre_circuito,
                         ID = ttt.id_solicitud,
                         NombreTarea = tar.nombre_tarea,
                         NombreResultado = result.nombre_resultado,
                         ProximaTarea = tarprox.nombre_tarea,
                         FechaInicio = trah.FechaInicio_tramitetarea,
                         FechaCierre = trah.FechaCierre_tramitetarea
                     }
                     )
                )
             select solicitudes);

            return query;
        }
    }
}