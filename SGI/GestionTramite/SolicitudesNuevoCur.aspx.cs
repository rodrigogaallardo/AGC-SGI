using ExtensionMethods;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public partial class SolicitudesNuevoCur : BasePage
    {
        #region Variables

        private int id_estado = -1;

        private int id_solicitud = 0;

        private DateTime? fechaDesde;

        private DateTime? fechaHasta;


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
        bool hayFiltroBusqueda = false;
        public bool HayFiltro
        {
            get { return hayFiltroBusqueda; }
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


        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {

                ScriptManager.RegisterStartupScript(updPnlResultadoBuscar, updPnlResultadoBuscar.GetType(), "loadPopOverRubro", "loadPopOverRubro();", true);
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),"inicializar_controles", "inicializar_controles();", true);
            }

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            try
            {
                IniciarEntity();
                CargarCombos();
                grdTramites.DataBind();
                EjecutarScript(upd_BuscarTramite, "showResultado();");
                FinalizarEntity();

            }
            catch (Exception ex)
            {
                FinalizarEntity();
                throw ex;
            }
        }

        private void CargarCombos()
        {
            using (db = new DGHP_Entities())
            {
                var estados = (from solEstado in db.TipoEstadoSolicitud
                               join sol in db.SSIT_Solicitudes_Nuevas on solEstado.Id equals sol.id_estado
                               where solEstado.Id == sol.id_estado
                               select new
                               {
                                   Id = solEstado.Id,
                                   Descripcion = solEstado.Descripcion
                               }).Distinct().ToList();

                List<ItemsEstado> ListEstados = new List<ItemsEstado>();
                ItemsEstado ItemsEstado = new ItemsEstado();
                ItemsEstado.Id = -1;
                ItemsEstado.Descripcion = "Todos";

                ListEstados.Insert(0, ItemsEstado);

                foreach (var item in estados)
                {
                    ListEstados.Add(new ItemsEstado(item.Id, item.Descripcion));
                }
                ddlEstado.DataSource = ListEstados;
                ddlEstado.DataTextField = "Descripcion";
                ddlEstado.DataValueField = "Id";
                ddlEstado.DataBind();
            }
        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                IniciarEntity();
                guardarFiltro();
                grdTramites.DataBind();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                LogError.Write(ex, "error al buscar tramites buscar_tramite-btnBuscar_OnClick");
                Enviar_Mensaje(ex.Message, "");
            }
        }

        private void guardarFiltro()
        {
            id_estado = Convert.ToInt16(ddlEstado.SelectedIndex.ToString());

            id_solicitud = !string.IsNullOrEmpty(txtNroSolicitud.Text) ? Convert.ToInt32(txtNroSolicitud.Text) : 0;

            DateTime fechaDesdeAux;
            DateTime fechaHastaAux;

            if (!string.IsNullOrEmpty(txtFechaDesde.Text))
            {
                if (!DateTime.TryParse(txtFechaDesde.Text, out fechaDesdeAux))
                    throw new Exception("Fecha solicitud desde inválida.");

                this.fechaDesde = fechaDesdeAux;
            }

            if (!string.IsNullOrEmpty(txtFechaHasta.Text))
            {
                if (!DateTime.TryParse(txtFechaHasta.Text, out fechaHastaAux))
                    throw new Exception("Fecha solicitud hasta inválida.");

                this.fechaHasta = fechaHastaAux;
            }

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
                ScriptManager.RegisterStartupScript(upd_BuscarTramite, upd_BuscarTramite.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
            }
        }


        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            id_solicitud = 0;
            txtNroSolicitud.Text = "";
            fechaDesde = null;
            txtFechaDesde.Text = "";
            fechaHasta = null;
            txtFechaHasta.Text = "";
            ddlEstado.ClearSelection();
            ddlEstado.Items[0].Selected = true;
            updPnlFiltroBuscar_tramite.Update();
            grdTramites.DataBind();

        }


        public List<clsItemConsultaTramiteNuevoCur> GetTramites(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {

            totalRowCount = 0;

            List<clsItemConsultaTramiteNuevoCur> lstResult = FiltrarTramites(startRowIndex, maximumRows, sortByExpression, out totalRowCount);

            pnlCantRegistros.Visible = true;

            if (totalRowCount > 1)
            {
                lblCantRegistros.Text = string.Format("{0} Trámites", totalRowCount);
            }
            else if (totalRowCount == 1)
                lblCantRegistros.Text = string.Format("{0} Trámite", totalRowCount);
            else
            {
                pnlCantRegistros.Visible = false;
            }
            pnlResultadoBuscar.Visible = true;
            //updPnlResultadoBuscar.Update();
            
            return lstResult;
        }

        private List<clsItemConsultaTramiteNuevoCur> FiltrarTramites(int startRowIndex, int maximumRows, string sortByExpression, out int totalRowCount)
        {
            DGHP_Entities db = new DGHP_Entities();

            List<clsItemConsultaTramiteNuevoCur> resultados = new List<clsItemConsultaTramiteNuevoCur>();

            using (db = new DGHP_Entities())
            {
                IQueryable<clsItemConsultaTramiteNuevoCur> qSOL = null;

                db.Database.CommandTimeout = 300;

                Guid userid = Functions.GetUserId();

                qSOL = (from sol in db.SSIT_Solicitudes_Nuevas
                        join solEstado in db.TipoEstadoSolicitud on sol.id_estado equals solEstado.Id
                        //join relRubros in db.Rel_Rubros_Solicitudes_Nuevas on sol.id_solicitud equals relRubros.id_Solicitud
                        select new clsItemConsultaTramiteNuevoCur
                        {
                            Id_tad = sol.id_Tad ?? 0,
                            Id_solicitud = sol.id_solicitud,
                            id_estado = solEstado.Id,
                            Estado = solEstado.Descripcion,
                            Nombre_RazonSocial = sol.Nombre_RazonSocial,
                            Cuit = sol.Cuit,
                            Nombre_Profesional = sol.Nombre_Profesional,
                            Matricula = sol.Matricula,
                            NroPartidaMatriz = sol.NroPartidaMatriz ?? 0,
                            NroPartidaHorizontal = sol.NroPartidaHorizontal ?? 0,
                            Calle = sol.Nombre_calle,
                            Altura_calle = sol.Altura_calle ?? 0,
                            Piso = sol.Piso,
                            UnidadFuncional = sol.UnidadFuncional,
                            Superficie = sol.Superficie ?? 0,
                            Mixtura = sol.CodZonaHab,
                            Fecha_confirmacion = sol.LastUpdateDate ?? null
                        }
                       );

                totalRowCount = qSOL.Count();

                //filtros fechas
                if (this.fechaDesde.HasValue || this.fechaHasta.HasValue)
                {
                    DateTime? fecha_desde = null;
                    DateTime? fecha_hasta = null;

                    if (this.fechaDesde.HasValue)
                        fecha_desde = this.fechaDesde.Value;
                    else
                        fecha_desde = new DateTime(2000, 1, 1);

                    if (this.fechaHasta.HasValue)
                        fecha_hasta = this.fechaHasta.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    else
                        fecha_hasta = DateTime.Now;

                    qSOL = (from sol in qSOL
                            where (sol.Fecha_confirmacion >= fecha_desde && sol.Fecha_confirmacion <= fecha_hasta)
                            select sol);
                    totalRowCount = qSOL.Count();
                }

                // filtro id_solicitud
                if (id_solicitud != 0)
                {
                    qSOL = (from sol in qSOL
                            where sol.Id_solicitud == this.id_solicitud
                            select sol);
                    totalRowCount = qSOL.Count();
                }

                // filtro Estado de Tramite
                id_estado = Convert.ToInt16(ddlEstado.SelectedValue);
                if (id_estado != -1)
                {
                    qSOL = (from sol in qSOL
                            where sol.id_estado == this.id_estado
                            select sol);
                    totalRowCount = qSOL.Count();
                }
                qSOL = qSOL.OrderBy(s => s.Id_solicitud).Skip(startRowIndex).Take(maximumRows);
                resultados = qSOL.ToList();
            }
            return resultados;
        }

        private List<clsItemConsultaTramiteNuevoCur> FiltrarTramitesXls(int startRowIndex, int maximumRows, string sortByExpression, out int totalRowCount)
        {
            DGHP_Entities db = new DGHP_Entities();

            List<clsItemConsultaTramiteNuevoCur> resultados = new List<clsItemConsultaTramiteNuevoCur>();

            using (db = new DGHP_Entities())
            {
                IQueryable<clsItemConsultaTramiteNuevoCur> qSOL = null;

                db.Database.CommandTimeout = 300;

                Guid userid = Functions.GetUserId();

                qSOL = (from sol in db.SSIT_Solicitudes_Nuevas
                        join solEstado in db.TipoEstadoSolicitud on sol.id_estado equals solEstado.Id
                        join relRubros in db.Rel_Rubros_Solicitudes_Nuevas on sol.id_solicitud equals relRubros.id_Solicitud into rubroSol
                        from rs in rubroSol.DefaultIfEmpty()
                        select new clsItemConsultaTramiteNuevoCur
                        {
                            Id_tad = sol.id_Tad ?? 0,
                            Id_solicitud = sol.id_solicitud,
                            id_estado = solEstado.Id,
                            Estado = solEstado.Descripcion,
                            Nombre_RazonSocial = sol.Nombre_RazonSocial,
                            Cuit = sol.Cuit,
                            Nombre_Profesional = sol.Nombre_Profesional,
                            Matricula = sol.Matricula,
                            NroPartidaMatriz = sol.NroPartidaMatriz ?? 0,
                            NroPartidaHorizontal = sol.NroPartidaHorizontal ?? 0,
                            Calle = sol.Nombre_calle,
                            Altura_calle = sol.Altura_calle ?? 0,
                            Piso = sol.Piso,
                            UnidadFuncional = sol.UnidadFuncional,
                            Superficie = sol.Superficie ?? 0,
                            Mixtura = sol.CodZonaHab,
                            CodigoRubro = (rs != null) ? rs.Codigo: "",
                            DescripcionRubro = (rs != null) ? rs.Descripcion: "",
                            SuperficieRubro = (rs != null) ? rs.Superficie: 0,
                            Fecha_confirmacion = sol.LastUpdateDate ?? null
                        }
                       );

                totalRowCount = qSOL.Count();

                //filtros fechas
                if (this.fechaDesde.HasValue || this.fechaHasta.HasValue)
                {
                    DateTime? fecha_desde = null;
                    DateTime? fecha_hasta = null;

                    if (this.fechaDesde.HasValue)
                        fecha_desde = this.fechaDesde.Value;
                    else
                        fecha_desde = new DateTime(2000, 1, 1);

                    if (this.fechaHasta.HasValue)
                        fecha_hasta = this.fechaHasta.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    else
                        fecha_hasta = DateTime.Now;

                    qSOL = (from sol in qSOL
                            where (sol.Fecha_confirmacion >= fecha_desde && sol.Fecha_confirmacion <= fecha_hasta)
                            select sol);
                    totalRowCount = qSOL.Count();
                }

                // filtro id_solicitud
                if (id_solicitud != 0)
                {
                    qSOL = (from sol in qSOL
                            where sol.Id_solicitud == this.id_solicitud
                            select sol);
                    totalRowCount = qSOL.Count();
                }

                // filtro Estado de Tramite
                id_estado = Convert.ToInt16(ddlEstado.SelectedValue);
                if (id_estado != -1)
                {
                    qSOL = (from sol in qSOL
                            where sol.id_estado == this.id_estado
                            select sol);
                    totalRowCount = qSOL.Count();
                }
                qSOL = qSOL.OrderBy(s => s.Id_solicitud).Skip(startRowIndex).Take(maximumRows);
                resultados = qSOL.ToList();
            }

            return resultados;
        }

        protected void grdBandeja_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                #region PopupRubros
                LinkButton lnkRubros = (LinkButton)e.Row.FindControl("lnkRubros");
                DataList lstRubros = (DataList)e.Row.FindControl("lstRubros");
                Panel Rubros = (Panel)e.Row.FindControl("Rubros");
                int id_solicitud = int.Parse(lnkRubros.CommandArgument);

                DGHP_Entities db = new DGHP_Entities();
                var relRubro = db.Rel_Rubros_Solicitudes_Nuevas.Where(x => x.id_Solicitud == id_solicitud).ToList();

                if (relRubro != null)
                {
                    var elements = (from rr in relRubro
                                    select new
                                    {
                                        Codido = rr.Codigo,
                                        Descripcion = rr.Descripcion,
                                        Superficie = rr.Superficie,
                                    }).ToList();
                    lstRubros.DataSource = elements;
                    lstRubros.DataBind();
                }
                if (relRubro.Count == 0)
                {
                    LinkButton link = (LinkButton)e.Row.FindControl("lnkRubros");
                    link.Visible = false;
                }
                db.Dispose();
                #endregion

                #region OcultarVerSolPDF

                if (row.Cells[3].Text != "En tr&#225;mite")
                {
                    HyperLink link = (HyperLink)e.Row.FindControl("lnkVerDoc");
                    link.Visible = false;
                }
                #endregion
            }
        }

        #region paginado grilla

        protected void grdTramites_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTramites.PageIndex = e.NewPageIndex;
            grdTramites.DataBind();
        }

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;
            grdTramites.PageIndex = int.Parse(cmdPage.Text) - 1;
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdTramites.PageIndex = grdTramites.PageIndex - 1;
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdTramites.PageIndex = grdTramites.PageIndex + 1;
        }

        protected void grdTramites_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)grdTramites;
            GridViewRow fila = (GridViewRow)grid.BottomPagerRow;

            if (fila != null)
            {
                LinkButton btnAnterior = (LinkButton)fila.Cells[0].FindControl("cmdAnterior");
                LinkButton btnSiguiente = (LinkButton)fila.Cells[0].FindControl("cmdSiguiente");

                if (grid.PageIndex == 0)
                    btnAnterior.Visible = false;
                else
                    btnAnterior.Visible = true;

                if (grid.PageIndex == grid.PageCount - 1)
                    btnSiguiente.Visible = false;
                else
                    btnSiguiente.Visible = true;


                // Ocultar todos los botones con Números de Página
                for (int i = 1; i <= 19; i++)
                {
                    LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                    btn.Visible = false;
                }


                if (grid.PageIndex == 0 || grid.PageCount <= 10)
                {
                    // Mostrar 10 botones o el máximo de páginas

                    for (int i = 1; i <= 10; i++)
                    {
                        if (i <= grid.PageCount)
                        {
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                            btn.Text = i.ToString();
                            btn.Visible = true;
                        }
                    }
                }
                else
                {
                    // Mostrar 9 botones hacia la izquierda y 9 hacia la derecha
                    // o bien los que sea posible en caso de no llegar a 9

                    int CantBucles = 0;

                    LinkButton btnPage10 = (LinkButton)fila.Cells[0].FindControl("cmdPage10");
                    btnPage10.Visible = true;
                    btnPage10.Text = Convert.ToString(grid.PageIndex + 1);

                    // Ubica los 9 botones hacia la izquierda
                    for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                    {
                        CantBucles++;
                        if (i >= 0)
                        {
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 - CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                        }

                    }

                    CantBucles = 0;
                    // Ubica los 9 botones hacia la derecha
                    for (int i = grid.PageIndex + 1; i <= grid.PageIndex + 9; i++)
                    {
                        CantBucles++;
                        if (i <= grid.PageCount - 1)
                        {
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 + CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                        }
                    }



                }
                LinkButton cmdPage;
                string btnPage = "";
                for (int i = 1; i <= 19; i++)
                {
                    btnPage = "cmdPage" + i.ToString();
                    cmdPage = (LinkButton)fila.Cells[0].FindControl(btnPage);
                    if (cmdPage != null)
                        cmdPage.CssClass = "btn";

                }


                // busca el boton por el texto para marcarlo como seleccionado
                string btnText = Convert.ToString(grid.PageIndex + 1);
                foreach (Control ctl in fila.Cells[0].FindControl("pnlpager").Controls)
                {
                    if (ctl is LinkButton)
                    {
                        LinkButton btn = (LinkButton)ctl;
                        if (btn.Text.Equals(btnText))
                        {
                            btn.CssClass = "btn btn-inverse";
                        }
                    }
                }

            }
        }

        #endregion

        #region "Exporta a Excel"

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {

            btnCerrarExportacion.Visible = false;
            // genera un nombre de archivo aleatorio
            Random random = new Random((int)DateTime.Now.Ticks);
            int NroAleatorio = random.Next(0, 100);
            NroAleatorio = NroAleatorio * random.Next(0, 100);
            string fileName = string.Format("Grilla-Solicitudes-{0}.xls", NroAleatorio);

            pnlDescargarExcel.Style["display"] = "none";
            pnlExportandoExcel.Style["display"] = "block";

            Session["exportacion_en_proceso"] = true;
            Session["progress_data"] = "Preparando exportación.";
            Session["filename_exportacion"] = fileName;

            lblRegistrosExportados.Text = "Preparando exportación.";
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ExportarGrillaAExcel));
            thread.Start();

            Timer1.Enabled = true;

        }

        private void ExportarGrillaAExcel()
        {

            decimal cant_registros_x_vez = 0m;
            int totalRowCount = 0;
            int startRowIndex = 0;

            try
            {

                // Esto se realiza para saber el total y de a cuanto se va mostrar el progreso.
                FiltrarTramitesXls(startRowIndex, 1, "", out totalRowCount);
                if (totalRowCount < 10000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;

                int cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);

                List<clsItemConsultaTramiteNuevoCur> resultados = new List<clsItemConsultaTramiteNuevoCur>();

                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados.AddRange(FiltrarTramitesXls(startRowIndex, Convert.ToInt32(cant_registros_x_vez), "", out totalRowCount));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados.Count);
                DataTable dt;
                var lstExportar = (from sol in resultados
                                   select new
                                   {
                                       Id_tad = sol.Id_tad,
                                       Id_solicitud = sol.Id_solicitud,
                                       Estado = sol.Estado,
                                       Nombre_RazonSocial = sol.Nombre_RazonSocial,
                                       Cuit = sol.Cuit,
                                       Nombre_Profesional = sol.Nombre_Profesional,
                                       Matricula = sol.Matricula,
                                       NroPartidaMatriz = sol.NroPartidaMatriz,
                                       NroPartidaHorizontal = sol.NroPartidaHorizontal,
                                       Calle = sol.Calle,
                                       Altura_calle = sol.Altura_calle,
                                       Piso = sol.Piso,
                                       UnidadFuncional = sol.UnidadFuncional,
                                       Superficie = sol.Superficie,
                                       Mixtura = sol.Mixtura,
                                       CodigoRubro = sol.CodigoRubro,
                                       DescripcionRubro = sol.DescripcionRubro,
                                       SuperficieRubro = sol.SuperficieRubro,
                                       Fecha_confirmacion = sol.Fecha_confirmacion
                                   }).ToList();
                dt = Functions.ToDataTable(lstExportar);

                // Convierte la lista en un dataset
                DataSet ds = new DataSet();
                dt.TableName = "Solicitudes";
                ds.Tables.Add(dt);

                string savedFileName = Constants.Path_Temporal + Session["filename_exportacion"].ToString();

                Functions.EliminarArchivosDirectorioTemporal();

                // Utiliza DocumentFormat.OpenXml para exportar a excel
                Functions.ExportDataSetToExcel(ds, savedFileName);

                // quita la variable de session.
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
            }
            catch
            {
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
            }
        }


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
        #endregion

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            id_estado = Convert.ToInt16(ddlEstado.SelectedValue);
        }
    }

    internal class ItemsEstado
    {
        public ItemsEstado(int id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }

        public ItemsEstado()
        {
        }

        public int Id { get; set; }

        public string Descripcion { get; set; }
    }
}