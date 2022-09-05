using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Dashboard.Controls
{
    public partial class ucSSP_Indicadores : System.Web.UI.UserControl
    {


        public class ucSSP_Indicadores_EventArgs : EventArgs
        {
            public string mensaje{ get; set; }
            public Exception ex { get; set; }
        }


        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {

            IniciarEntity();
            if (!IsPostBack)
            {
                try
                {

                    List<Items> lstPeriodos = getPeriodoIndicadores();

                    ddlPeriodo.DataSource = lstPeriodos;
                    ddlPeriodo.DataTextField = "Texto";
                    ddlPeriodo.DataValueField = "Codigo";
                    ddlPeriodo.DataBind();
                    ddlPeriodo.SelectedIndex = 0;

                    cargar_indicadores(ddlPeriodo.SelectedIndex);
                }
                catch (Exception ex)
                {
                    enviar_error(null, ex);
                }

            }

            


        }

        protected override void OnUnload(EventArgs e)
        {
            Dispose();
            base.OnUnload(e);
        }

        public void Dispose()
        {
            FinalizarEntity();
        }

        private void enviar_error(object sender, Exception ex)
        {
            if (ex != null)
            {
                if (Error != null)
                {
                    ucSSP_Indicadores_EventArgs args = new ucSSP_Indicadores_EventArgs();
                    args.ex = ex;
                    Error(sender, args);
                }
            }
        }

        public static List<Items> getPeriodoIndicadores()
        {
            DateTime fecha_inicial = DateTime.Today;
            int meses = 1 + SmallDay.CalcularMesesDeDiferencia(new DateTime(2014, 3, 1), DateTime.Today);

            List<Items> lista = new List<Items>();
            Items item;


            fecha_inicial = new DateTime(fecha_inicial.Year, fecha_inicial.Month, 1);
            DateTime fecha;


            for (int i = 0; i < meses; i++)
            {
                item = new Items();

                fecha = fecha_inicial.AddMonths(-i);
                item.Codigo = fecha.ToString("dd/MM/yyyy");
                item.Texto = fecha.Year + " - " + SmallDay.NombreMes(fecha.Month);

                lista.Add(item);


            }

            return lista;

        }

        #endregion
      

        #region  propiedades

        private DateTime _fecha_actividad;
        public DateTime Fecha_actividad
        {
            get
            {
                DateTime aux ;
                if (DateTime.TryParse(hid_fecha_actividad.Value, out aux))
                {
                    _fecha_actividad = aux;
                }
                else
                {
                    _fecha_actividad = DateTime.MinValue;
                }

                return _fecha_actividad;
            }
            set
            {
                _fecha_actividad = value;
                hid_fecha_actividad.Value = _fecha_actividad.ToString("dd/mm/yyyy");
            }
        }

        #endregion

        #region  eventos

        public delegate void EventHandler_error_sol_asig(object sender, ucSSP_Indicadores_EventArgs e);
        public event EventHandler_error_sol_asig Error;

        #endregion

        #region entity

        private DGHP_Entities db = null;
        private AGC_FilesEntities dbFiles = null;

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

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
            {
                this.dbFiles.Dispose();
                this.dbFiles = null;
            }
        }

        #endregion

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {

            cargar_indicadores(ddlPeriodo.SelectedIndex);
        }

        private void cargar_indicadores(int index)
        {

            txtNivelActividad.Text = "";

            DateTime fecha;
            decimal nivel_actividad = 0;
            decimal aprobado_hasta_gerente = 0;
            decimal aprobado_hasta_expediente = 0;

            if (DateTime.TryParse(ddlPeriodo.SelectedValue, out fecha))
            {
                nivel_actividad = cargar_indicador_nivel_actividad(fecha);
                aprobado_hasta_gerente = cargar_aprobado_hasta_revision_gerente(fecha);
                aprobado_hasta_expediente = cargar_aprobado_hasta_generar_expediente(fecha);
            }

            txtNivelActividad.Text = string.Format("{0:#0.00}", nivel_actividad);
            txtTramiteRevisionGerente.Text = string.Format("{0:#0.00}", aprobado_hasta_gerente);
            txtTramiteGeneracionExpediente.Text = string.Format("{0:#0.00}", aprobado_hasta_expediente);
            
        }

        public class diasHabiles
        {

            public List<SmallDay> dias { get; set; }
            public DateTime fecha_ini { get; set; }
            public DateTime fecha_fin { get; set; }

            public diasHabiles(DateTime fec_ini, DateTime fec_fin)
            {
                this.fecha_ini = fec_ini;
                this.fecha_fin = fec_fin;

                DGHP_Entities db = new DGHP_Entities();

                // los dias habiles son los labolares
                // son los dias que realizaron alguna tarea manual, el resto corresponden a feriados
                this.dias = new List<SmallDay>();

                int[] tareas_manuales = new int[10] 
                { 
                    (int)Constants.ENG_Tareas.SSP_Validar_Zonificacion, 
                    (int)Constants.ENG_Tareas.SCP_Validar_Zonificacion,
                    (int)Constants.ENG_Tareas.SSP_Asignar_Calificador, 
                    (int)Constants.ENG_Tareas.SCP_Asignar_Calificador,
                    (int)Constants.ENG_Tareas.SSP_Calificar, 
                    (int)Constants.ENG_Tareas.SCP_Calificar,
                    (int)Constants.ENG_Tareas.SSP_Revision_SubGerente,
                    (int)Constants.ENG_Tareas.SCP_Revision_SubGerente,
                    (int)Constants.ENG_Tareas.SSP_Revision_Gerente,
                    (int)Constants.ENG_Tareas.SCP_Revision_Gerente
                };

                var qfechas =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        where tareas_manuales.Contains(tt.id_tarea) &&
                         tt.FechaCierre_tramitetarea != null &&
                        tt.FechaCierre_tramitetarea >= fec_ini && tt.FechaCierre_tramitetarea <= fec_fin
                        orderby tt.FechaCierre_tramitetarea
                        select new 
                        {
                            fecha = tt.FechaCierre_tramitetarea.Value
                        }
                    ).ToList();

                SmallDay dia = null;
                foreach (var item in qfechas)
                {
                    dia = new SmallDay(item.fecha);
                    if (!this.dias.Exists(x => x.fecha == dia.fecha))
                        this.dias.Add(dia);
                }

                db.Dispose();

            }

            public int cantidadDiasHabiles_enPeriodo(DateTime fec_ini, DateTime fec_fin)
            {
                fec_ini = new DateTime(fec_ini.Year, fec_ini.Month, fec_ini.Day);
                fec_fin = new DateTime(fec_fin.Year, fec_fin.Month, fec_fin.Day);
                List<SmallDay> listaDias = this.dias.Where(x => x.fecha >= fec_ini && x.fecha <= fec_fin).ToList();

                if (listaDias == null)
                    return 0;
                else
                    return listaDias.Count();
            }

        }

        private decimal cargar_indicador_nivel_actividad(DateTime fecha)
        {
            decimal indicador = 0;
            DateTime periodo_fecha_ini = SmallDay.PrimerDiaMes(fecha);
            DateTime periodo_fecha_fin = SmallDay.UltimoDiaMes(fecha);
            if (periodo_fecha_fin > DateTime.Today)
                periodo_fecha_fin = new SmallDay(DateTime.Today).fecha;

            //DateTime fecha_filtro_fin = periodo_fecha_fin.AddDays(1);

            // resueltas: cantidad de solicitudes calificadas ( solo aprabadas y pedir rectificacion) 
            // cerradas en el mes evaluado
            int[] tareas = new int[2] { (int)Constants.ENG_Tareas.SSP_Calificar, (int)Constants.ENG_Tareas.SCP_Calificar };
            int[] resultado = new int[2] { (int)Constants.ENG_ResultadoTarea.Aprobado, (int)Constants.ENG_ResultadoTarea.Calificar_Pedir_Rectificacion };

            var cant_resuelto =
                (
                    from tt in db.SGI_Tramites_Tareas
                    where tareas.Contains(tt.id_tarea) &&
                        tt.FechaCierre_tramitetarea != null &&
                        resultado.Contains(tt.id_resultado) &&
                        tt.FechaCierre_tramitetarea >= periodo_fecha_ini &&
                        tt.FechaCierre_tramitetarea < EntityFunctions.AddDays(periodo_fecha_fin, 1)
                    select tt
                  ).Count();

            // cantidad zonificar
            tareas = new int[2] { (int)Constants.ENG_Tareas.SSP_Validar_Zonificacion, (int)Constants.ENG_Tareas.SCP_Validar_Zonificacion };

            var cant_ingresados =
                (
                    from tt in db.SGI_Tramites_Tareas
                    where tareas.Contains(tt.id_tarea) &&
                        tt.FechaInicio_tramitetarea >= periodo_fecha_ini &&
                        tt.FechaInicio_tramitetarea < EntityFunctions.AddDays(periodo_fecha_fin, 1)
                    select tt
                  ).Count();

            //cantidad reingreso por pedido de rectificacion
            tareas = new int[2] { (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud, (int)Constants.ENG_Tareas.SCP_Correccion_Solicitud };

            var cant_reingresada =
                (
                    from tt in db.SGI_Tramites_Tareas
                    where tareas.Contains(tt.id_tarea) &&
                        tt.FechaCierre_tramitetarea >= periodo_fecha_ini &&
                        tt.FechaCierre_tramitetarea < EntityFunctions.AddDays(periodo_fecha_fin, 1)
                    select tt
                  ).Count();

            if ( cant_ingresados + cant_reingresada > 0 )
                indicador = cant_resuelto / (decimal)(cant_ingresados + cant_reingresada);

            return indicador;
        }

        private decimal cargar_aprobado_hasta_revision_gerente(DateTime fecha)
        {
            decimal indicador = 0;
            DateTime periodo_fecha_ini = SmallDay.PrimerDiaMes(fecha);
            DateTime periodo_fecha_fin = SmallDay.UltimoDiaMes(fecha);
            if (periodo_fecha_fin > DateTime.Today)
                periodo_fecha_fin = new SmallDay(DateTime.Today).fecha;

            //DateTime fecha_filtro_fin = periodo_fecha_fin.AddDays(1);
            //// obtener lista de fechas con dias habiles para el periodo
           

            // buscar solicitudes aprobadas
          
            int id_resultado_calificar = (int)Constants.ENG_ResultadoTarea.Aprobado;

            int id_tarea_calificador = (int)Constants.ENG_Tareas.SSP_Calificar;
            int id_tarea_zonificador = (int)Constants.ENG_Tareas.SSP_Validar_Zonificacion;
            int id_tarea_gerente = (int)Constants.ENG_Tareas.SSP_Revision_Gerente;
 
            var tramites_analizados =
                (
                    from grp in
                        (
                        /* ANTES
                            from tt in db.SGI_Tramites_Tareas
                            join zon in db.SGI_Tramites_Tareas on 
                                new { k1 = tt.id_solicitud, k2 = id_tarea_zonificador } equals 
                                new { k1 = zon.id_solicitud, k2 = zon.id_tarea}
                            join ger in db.SGI_Tramites_Tareas on
                                new { k3 = tt.id_solicitud, k4 = id_tarea_gerente } equals
                                new { k3 = ger.id_solicitud, k4 = ger.id_tarea }
                            where ger.FechaCierre_tramitetarea != null && // gerente debe finalizar tarea
                                tt.id_tarea == id_tarea_calificador && // tarea calificar
                                tt.FechaCierre_tramitetarea != null &&
                                tt.id_resultado == id_resultado_calificar && // resuelta por calificador
                                tt.FechaCierre_tramitetarea >= periodo_fecha_ini &&
                                tt.FechaCierre_tramitetarea < EntityFunctions.AddDays(periodo_fecha_fin, 1)
                         */
                            from tt in db.SGI_Tramites_Tareas
                            join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                            join zon_hab in db.SGI_Tramites_Tareas_HAB on tt_hab.id_solicitud equals zon_hab.id_solicitud
                            join zon in db.SGI_Tramites_Tareas on zon_hab.id_tramitetarea equals zon.id_tramitetarea
                            join ger_hab in db.SGI_Tramites_Tareas_HAB on tt_hab.id_solicitud equals ger_hab.id_solicitud
                            join ger in db.SGI_Tramites_Tareas on ger_hab.id_tramitetarea equals ger.id_tramitetarea

                            where zon.id_tarea == id_tarea_zonificador &&
                                ger.id_tarea == id_tarea_gerente &&
                                ger.FechaCierre_tramitetarea != null && // gerente debe finalizar tarea
                                tt.id_tarea == id_tarea_calificador && // tarea calificar
                                tt.FechaCierre_tramitetarea != null &&
                                tt.id_resultado == id_resultado_calificar && // resuelta por calificador
                                tt.FechaCierre_tramitetarea >= periodo_fecha_ini &&
                                tt.FechaCierre_tramitetarea < EntityFunctions.AddDays(periodo_fecha_fin, 1)
                            select new 
                            {
                                tt_hab.id_solicitud,
                                zon.FechaInicio_tramitetarea,
                                ger.FechaCierre_tramitetarea
                            }
                        )
                    group grp by new
                    {
                        grp.id_solicitud
                    } into grupo
                    select new
                    {
                        id_solicitud = grupo.Key.id_solicitud,
                        fecha_ini = grupo.Min(x => x.FechaInicio_tramitetarea),
                        fecha_fin = grupo.Max(x => x.FechaCierre_tramitetarea.Value)
                    }
                  ).ToList();

            if (tramites_analizados == null || tramites_analizados.Count == 0)
                return indicador;

            DateTime fec_habil_ini = (DateTime)tramites_analizados.Min(x => x.fecha_ini);
            DateTime fec_habil_fin = (DateTime)tramites_analizados.Max(x => x.fecha_fin);

            // un tramite aprobado puede haber ingresado el mes pasado o finalizado el siguiente
            //diasHabiles habiles = new diasHabiles(fec_habil_ini, fec_habil_fin); // dias habiles de fechas minima y maxima de tramites analizados
            diasHabiles habiles = new diasHabiles(periodo_fecha_ini, periodo_fecha_fin);  // dias habiles del periodo

            int dia_habiles_por_tramite = 0;
            int cant_tramites = 0;
            int cant = 0;

            foreach (var tramite in tramites_analizados)
            {
                cant = habiles.cantidadDiasHabiles_enPeriodo(tramite.fecha_ini, tramite.fecha_fin);
                if (cant > 0)
                {
                    // los tramites con cero dias empezaron y finalizaron en otros periodos
                    dia_habiles_por_tramite = dia_habiles_por_tramite + cant;
                    cant_tramites++;
                }
            }

            if ( cant_tramites > 0 )
                indicador = dia_habiles_por_tramite / (decimal)cant_tramites;      

            return indicador;

        }


        private decimal cargar_aprobado_hasta_generar_expediente(DateTime fecha)
        {
            decimal indicador = 0;
            DateTime periodo_fecha_ini = SmallDay.PrimerDiaMes(fecha);
            DateTime periodo_fecha_fin = SmallDay.UltimoDiaMes(fecha);
            if (periodo_fecha_fin > DateTime.Today)
                periodo_fecha_fin = new SmallDay(DateTime.Today).fecha;

            //DateTime fecha_filtro_fin = periodo_fecha_fin.AddDays(1);
            //// obtener lista de fechas con dias habiles para el periodo
            //diasHabiles habiles = new diasHabiles(periodo_fecha_ini, periodo_fecha_fin);

            // buscar solicitudes aprobadas

            int id_resultado_calificar = (int)Constants.ENG_ResultadoTarea.Aprobado;

            int id_tarea_calificador = (int)Constants.ENG_Tareas.SSP_Calificar;
            int id_tarea_zonificador = (int)Constants.ENG_Tareas.SSP_Validar_Zonificacion;
            int id_tarea_expediente = (int)Constants.ENG_Tareas.SSP_Generar_Expediente;

            var tramites_analizados =
                (
                    from grp in
                        (
                            /* antes
                            from tt in db.SGI_Tramites_Tareas
                            join zon in db.SGI_Tramites_Tareas on
                                  new { k1 = tt.id_solicitud, k2 = id_tarea_zonificador } equals
                                  new { k1 = zon.id_solicitud, k2 = zon.id_tarea }
                            join expe in db.SGI_Tramites_Tareas on
                                    new { k3 = tt.id_solicitud, k4 = id_tarea_expediente } equals
                                    new { k3 = expe.id_solicitud, k4 = expe.id_tarea }
                            where tt.id_tarea == id_tarea_calificador && // tarea calificar
                                tt.FechaCierre_tramitetarea != null &&
                                tt.id_resultado == id_resultado_calificar && // resuelta por calificador
                                tt.FechaCierre_tramitetarea >= periodo_fecha_ini &&
                                tt.FechaCierre_tramitetarea < EntityFunctions.AddDays(periodo_fecha_fin, 1) &&
                                expe.FechaCierre_tramitetarea != null
                             */
                            from tt in db.SGI_Tramites_Tareas
                            join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                            join zon_hab in db.SGI_Tramites_Tareas_HAB on tt_hab.id_solicitud equals zon_hab.id_solicitud
                            join zon in db.SGI_Tramites_Tareas on zon_hab.id_tramitetarea equals zon.id_tramitetarea
                            join expe_hab in db.SGI_Tramites_Tareas_HAB on tt_hab.id_solicitud equals expe_hab.id_solicitud
                            join expe in db.SGI_Tramites_Tareas on expe_hab.id_tramitetarea equals expe.id_tramitetarea
                            where zon.id_tarea == id_tarea_zonificador &&
                                expe.id_tarea == id_tarea_expediente &&
                                tt.id_tarea == id_tarea_calificador && // tarea calificar
                                tt.FechaCierre_tramitetarea != null &&
                                tt.id_resultado == id_resultado_calificar && // resuelta por calificador
                                tt.FechaCierre_tramitetarea >= periodo_fecha_ini &&
                                tt.FechaCierre_tramitetarea < EntityFunctions.AddDays(periodo_fecha_fin, 1) &&
                                expe.FechaCierre_tramitetarea != null

                            
                            select new
                            {
                                tt_hab.id_solicitud,
                                zon.FechaInicio_tramitetarea,
                                expe.FechaCierre_tramitetarea
                            }
                        )
                    group grp by new
                    {
                        grp.id_solicitud
                    } into grupo
                    select new
                    {
                        id_solicitud = grupo.Key.id_solicitud,
                        fecha_ini = grupo.Min(x => x.FechaInicio_tramitetarea),
                        fecha_fin = grupo.Max(x => x.FechaCierre_tramitetarea.Value)
                    }
                  ).ToList();

            if (tramites_analizados == null || tramites_analizados.Count == 0)
                return indicador;

            int id_tarea_generar_boleta = (int)Constants.ENG_Tareas.SSP_Generacion_Boleta;
            int id_tarea_revisar_boleta = (int)Constants.ENG_Tareas.SSP_Revision_Pagos;

            var tareas_automaticas =
                (
                    from grp_auto in
                        (
                            /*antes
                            from tt_auto in tramites_analizados
                            join boleta in db.SGI_Tramites_Tareas on
                                new { k1 = tt_auto.id_solicitud, k2 = id_tarea_generar_boleta } equals
                                new { k1 = boleta.id_solicitud, k2 = boleta.id_tarea }
                            join revisar_boleta in db.SGI_Tramites_Tareas on
                                new { k3 = tt_auto.id_solicitud, k4 = id_tarea_revisar_boleta } equals
                                new { k3 = revisar_boleta.id_solicitud, k4 = revisar_boleta.id_tarea }
                            where boleta.FechaCierre_tramitetarea != null &&
                                revisar_boleta.FechaCierre_tramitetarea != null
                            select new
                            {
                                tt_auto.id_solicitud,
                                boleta.FechaInicio_tramitetarea,
                                revisar_boleta.FechaCierre_tramitetarea
                            }
                             */
                            from tt_auto in tramites_analizados
                            join boleta_hab in db.SGI_Tramites_Tareas_HAB on tt_auto.id_solicitud equals boleta_hab.id_solicitud
                            join boleta in db.SGI_Tramites_Tareas on boleta_hab.id_tramitetarea equals boleta.id_tramitetarea
                            join revisar_boleta_hab in db.SGI_Tramites_Tareas_HAB on tt_auto.id_solicitud equals revisar_boleta_hab.id_solicitud
                            join revisar_boleta in db.SGI_Tramites_Tareas on revisar_boleta_hab.id_tramitetarea equals revisar_boleta.id_tramitetarea

                            where boleta.id_tarea == id_tarea_generar_boleta &&
                                revisar_boleta.id_tarea == id_tarea_revisar_boleta &&
                                boleta.FechaCierre_tramitetarea != null &&
                                revisar_boleta.FechaCierre_tramitetarea != null
                            select new
                            {
                                tt_auto.id_solicitud,
                                boleta.FechaInicio_tramitetarea,
                                revisar_boleta.FechaCierre_tramitetarea
                            }

                       )
                    group grp_auto by new
                    {
                        grp_auto.id_solicitud
                    } into grupo_auto
                    select new
                    {
                        id_solicitud = grupo_auto.Key.id_solicitud,
                        fecha_ini = grupo_auto.Min(x => x.FechaInicio_tramitetarea),
                        fecha_fin = grupo_auto.Max(x => x.FechaCierre_tramitetarea.Value)
                    }
                  ).ToList();

            DateTime fec_habil_ini = (DateTime)tramites_analizados.Min(x => x.fecha_ini);
            DateTime fec_habil_fin = (DateTime)tramites_analizados.Max(x => x.fecha_fin);

            // un tramite aprobado puede haber ingresado el mes pasado o finalizado el siguiente
            //diasHabiles habiles = new diasHabiles(fec_habil_ini, fec_habil_fin); // dias habiles de fechas minima y maxima de tramites analizados
            diasHabiles habiles = new diasHabiles(periodo_fecha_ini, periodo_fecha_fin);  // dias habiles del periodo

            int dia_habiles_por_tramite = 0;
            int cant_tramites = 0;
            int cant = 0;
            int cant_auto = 0;

            SmallDay fec_ini_auto;
            SmallDay fec_fin_auto;


            foreach (var tramite in tramites_analizados)
            {
                cant = habiles.cantidadDiasHabiles_enPeriodo(tramite.fecha_ini, tramite.fecha_fin);
                if (cant > 0)
                {

                    var tramite_auto = tareas_automaticas.Where(x => x.id_solicitud == tramite.id_solicitud).FirstOrDefault();

                    fec_ini_auto = new SmallDay( tramite_auto.fecha_ini);
                    fec_fin_auto = new SmallDay( tramite_auto.fecha_fin); 

                    if ( fec_fin_auto.Equals(tramite.fecha_fin) ) // no comparar minutos y segundos
                    {
                        // cuando la revision de pagos temino el mismo dia que el expediente
                        // se evaluan los dias habiles hasta el dia anterior
                        fec_fin_auto.fecha = fec_fin_auto.fecha.AddDays(-1);
                    }

                    // buscar dias habiles en los cuales el tramite estuvo en las tareas 
                    // que no dependen de un operador para restar los dias y quedarse 
                    // solo con la cantidad de dias habiles de trabajo operativo
                    cant_auto = habiles.cantidadDiasHabiles_enPeriodo(fec_ini_auto.fecha, fec_fin_auto.fecha);

                    // cuando se usa "dias habiles del periodo" puede pasar que hayan iniciado el tramite antes o finalizado despues del periodo
                    dia_habiles_por_tramite = dia_habiles_por_tramite + cant - cant_auto;
                    cant_tramites++;
                }
            }

            if (cant_tramites > 0)
                indicador = dia_habiles_por_tramite / (decimal)cant_tramites;

            return indicador;

        }

    }

}