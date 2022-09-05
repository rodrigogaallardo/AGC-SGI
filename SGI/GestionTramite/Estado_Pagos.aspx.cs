using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity.Core.Objects;
using System.Data;
using System.Reflection;

namespace SGI.GestionTramite
{
    public partial class Estado_Pagos : BasePage
    {
        DGHP_Entities db = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlEstados, updPnlEstados.GetType(), "inicializar_controles", "inicializar_controles();", true);
            }
            if (!IsPostBack)
            {
                CargarDatos();
            }
        }
        private void CargarDatos()
        {
            try
            {
                db = new DGHP_Entities();
                List<string> listaJobs = new List<string>();
                listaJobs.Add("j_Pagos_Vencer_Boletas");
                listaJobs.Add("j_Pagos_Evaluar_Vencidos");
                listaJobs.Add("j_Pagos_Evaluar_Pendientes");

                var q = (from jhist in db.sysjobs_historial
                         where listaJobs.Contains(jhist.name) && jhist.run_status == (int)Constants.Estados_Jobs.Completado
                         select jhist).ToList();

                var p = (from pProc in db.wsPagos_Procesados
                         where (pProc.Fecha.Day == DateTime.Now.Day && pProc.Fecha.Month == DateTime.Now.Month && pProc.Fecha.Year == DateTime.Now.Year)
                         group pProc.Job by pProc.Job into g
                         select new pagosProcesados()
                         {
                             Cantidad = g.Count(),
                             Nombre_Job = g.Key
                         }).ToList();

                txtJobEjecVencido.Text = q.Where(x => x.name == "j_Pagos_Vencer_Boletas").Select(x => x.ultima_ejec).FirstOrDefault().ToString();
                txtJobEjecBolVencido.Text = q.Where(x => x.name == "j_Pagos_Evaluar_Vencidos").Select(x => x.ultima_ejec).FirstOrDefault().ToString();
                txtJobEjecBoletas.Text = q.Where(x => x.name == "j_Pagos_Evaluar_Pendientes").Select(x => x.ultima_ejec).FirstOrDefault().ToString();

                txtTotalVencido.Text = p.Where(x => x.Nombre_Job == "j_Pagos_Vencer_Boletas").Select(x => x.Cantidad).FirstOrDefault().ToString();
                txtTotalBolVencido.Text = p.Where(x => x.Nombre_Job == "j_Pagos_Evaluar_Vencidos").Select(x => x.Cantidad).FirstOrDefault().ToString();
                txtTotalBoleta.Text = p.Where(x => x.Nombre_Job == "j_Pagos_Evaluar_Pendientes").Select(x => x.Cantidad).FirstOrDefault().ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private class pagosProcesados
        {
            public int Cantidad { get; set; }
            public string Nombre_Job { get; set; }
        }
        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);

            string script_nombre = "mostrarMensaje";
            string script = "mostrarMensaje('" + mensaje + "','" + titulo + "');";

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm != null && sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlEstados, updPnlEstados.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
            }

        }

        #region ExportacionExcel

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                bool exportacion_en_proceso = (Session["exportacion_en_proceso"] != null ? (bool)Session["exportacion_en_proceso"] : false);

                if (exportacion_en_proceso)
                    lblRegistrosExportados.Text = Convert.ToString(Session["progress_data"]);
                else
                {
                    Timer1.Enabled = false;
                    btnCerrarExportacion.Visible = true;
                    pnlDescargarExcel.Style["display"] = "block";
                    pnlExportandoExcel.Style["display"] = "none";
                    string filename = Session["filename_exportacion"].ToString();
                    filename = HttpUtility.UrlEncode(filename);
                    btnDescargarExcel.NavigateUrl = string.Format("~/Controls/DescargarArchivoTemporal.aspx?fname={0}", filename);
                    Session.Remove("filename_exportacion");
                }
            }
            catch
            {
                Timer1.Enabled = false;
            }

        }

        protected void btnCerrarExportacion_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            Session.Remove("filename_exportacion");
            Session.Remove("progress_data");
            Session.Remove("exportacion_en_proceso");
            pnlDescargarExcel.Style["display"] = "none";
            pnlExportandoExcel.Style["display"] = "block";

            this.EjecutarScript(updExportaExcel, "hidefrmExportarExcel();");

        }

        protected void mostrarTimer(string name)
        {
            btnCerrarExportacion.Visible = false;
            // genera un nombre de archivo aleatorio
            Random random = new Random((int)DateTime.Now.Ticks);
            int NroAleatorio = random.Next(0, 100);
            NroAleatorio = NroAleatorio * random.Next(0, 100);
            name = name + "-{0}.xls";
            string fileName = string.Format(name, NroAleatorio);

            Session["exportacion_en_proceso"] = true;
            Session["progress_data"] = "Preparando exportación.";
            Session["filename_exportacion"] = fileName;

            Timer1.Enabled = true;
        }

        protected void lnkpostvencidasExportar_Click(object sender, EventArgs e)
        {
            try
            {
                //Validaciones
                if (string.IsNullOrEmpty(txtFechaDesde.Text.Trim()))
                    throw new Exception("No se ha ingresado 'Fecha Desde'");

                if (string.IsNullOrEmpty(txtFechaHasta.Text.Trim()))
                    throw new Exception("No se ha ingresado 'Fecha Hasta'");

                if (txtFechaDesde.Text.Trim().Length < 10 || txtFechaDesde.Text.Trim().Length > 10)
                    throw new Exception("La fecha desde es incorrecta, el formato es dd/mm/aaaa");

                if (txtFechaHasta.Text.Trim().Length < 10 || txtFechaHasta.Text.Trim().Length > 10)
                    throw new Exception("La fecha hasta es incorrecta, el formato es dd/mm/aaaa");

                DateTime fechadesde = Convert.ToDateTime(txtFechaDesde.Text.Trim());
                DateTime fechahasta = Convert.ToDateTime(txtFechaHasta.Text.Trim());

                this.EjecutarScript(updExportaExcel, "showfrmExportarExcel();");
                mostrarTimer("BUI-Recuperadas");
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ExportarBUIRecuperadas));
                thread.Start();
            }
            catch (Exception ex)
            {
                this.EjecutarScript(updExportaExcel, "hidefrmExportarExcel();");
                LogError.Write(ex);
                Enviar_Mensaje(ex.Message, "Exportacion Fallida");
                //throw new Exception(ex.Message);
            }
        }

        protected List<clsBUIrecuperadas> generarReporteBUIRecuperadas(int startRowIndex, int maximumRows, out int totalRowCount)
        {
            DGHP_Entities db = new DGHP_Entities();
            
            DateTime fecha_desde = Convert.ToDateTime(txtFechaDesde.Text);
            DateTime fecha_hasta = Convert.ToDateTime(txtFechaHasta.Text);
            int[] EstadosNoPagos = { 2, 3, 4 };

            var q = (from buih in db.wsPagos_BoletaUnica_HistorialEstados2
                     join bui in db.wsPagos_BoletaUnica on buih.id_pago_bu equals bui.id_pago_BU
                     join pag in db.wsPagos on bui.id_pago equals pag.id_pago
                     where EstadosNoPagos.Contains(buih.id_estadopago_ant) && buih.id_estadopago_nuevo == 1 
                     && buih.CreateDate >= fecha_desde && buih.CreateDate <= fecha_hasta
                     select new clsBUIrecuperadas()
                     {
                         Numero_BUI = bui.BUI_Numero,
                         Fecha_Recupero = buih.CreateDate,
                         Fecha_Pago = bui.FechaPago_BU,
                         Sistema = pag.CreateUser,
                     });


            q = q.OrderBy(x => x.Fecha_Recupero);
            List<clsBUIrecuperadas> resultados = new List<clsBUIrecuperadas>();

            if (maximumRows != 0)
            {
                resultados = q.Skip(startRowIndex).Take(maximumRows).ToList();

            }
            else
            {
                resultados = q.ToList();
            }
            totalRowCount = resultados.Count();
            return resultados;
        }

        private void ExportarBUIRecuperadas()
        {
            decimal cant_registros_x_vez = 0m;
            int totalRowCount = 0;
            int startRowIndex = 0;
            try
            {
                // Esto se realiza para saber el total y de a cuanto se va mostrar el progreso.
                generarReporteBUIRecuperadas(startRowIndex, 0, out totalRowCount);
                if (totalRowCount < 10000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;
                int cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<clsBUIrecuperadas> resultados = new List<clsBUIrecuperadas>();
                Session["progress_data"] = "";
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados.AddRange(generarReporteBUIRecuperadas(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados.Count);
                var lstExportar = (from res in resultados
                                   select new
                                   {
                                       NumeroBUI = res.Numero_BUI,
                                       FechadeRecupero = res.Fecha_Recupero,
                                       FechadePago = res.Fecha_Pago,
                                       Sistema = res.Sistema,
                                   }).ToList();

                // Convierte la lista en un dataset
                DataSet ds = new DataSet();
                DataTable dt = Functions.ToDataTable(lstExportar);
                dt.TableName = "BUI recuperadas post-vencimiento";
                ds.Tables.Add(dt);
                string savedFileName = Constants.Path_Temporal + Session["filename_exportacion"].ToString();

                Functions.EliminarArchivosDirectorioTemporal();
                // Utiliza DocumentFormat.OpenXml para exportar a excel
                Model.CreateExcelFile.CreateExcelDocument(ds, savedFileName);
                // quita la variable de session.
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
            }
            catch (Exception ex)
            {
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
            }
        }
        #endregion

    }

}