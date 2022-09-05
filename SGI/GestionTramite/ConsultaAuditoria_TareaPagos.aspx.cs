using SGI.Model;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI
{
    public partial class ConsultaAuditoria_TareaPagos : BasePage
    {

        #region cargar inicial


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CargaInicial();
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
        }

        private void CargaInicial()
        {
            IniciarEntity();

            int[] list_tareas = new int[2] {(int)Constants.ENG_Tareas.SSP_Generacion_Boleta, (int)Constants.ENG_Tareas.SSP_Revision_Pagos };

            var qTareas =
                (
                    from t in db.ENG_Tareas
                    where list_tareas.Contains( t.id_tarea )
                    select new
                    {
                        t.id_tarea,
                        t.nombre_tarea
                    }
                ).ToList();

            ddlTareas.DataTextField = "nombre_tarea";
            ddlTareas.DataValueField = "id_tarea";
            ddlTareas.DataSource = qTareas;
            ddlTareas.DataBind();

            ddlTareas.Items.Insert(0, new ListItem("Todas", "0"));

            updPnlFiltroBuscar.Update();
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode( this.Title );
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        #endregion

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

        #region buscar tramite


        private void limpiarCampos()
        {

            txtNroSolicitud.Text = "";
            txtNroBoleta.Text = "";
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";

            if (ddlTareas.Items.Count >= 0)
                ddlTareas.SelectedIndex = 0;
        }

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {

            EjecutarScript(updPnlBuscarPagos, "hideResultado();");
            limpiarCampos();
            grdAuditoriaPagos.DataSource = null;
            grdAuditoriaPagos.DataBind();
            updPnlFiltroBuscar.Update();


            pnlResultadobuscar.Visible = false;
            updPnlResultadoBuscar.Update();
        }



        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                elimiarFiltro();
                IniciarEntity();

                ValidarBuscar();
                CargarAuditoria();

                guardarFiltro();

                EjecutarScript(updPnlBuscarPagos, "showResultado();");

            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }



        }

        private int id_solicitud = 0;
        private int nro_boleta = 0;
        private DateTime? fechaDesde;
        private DateTime? fechaHasta;
        private int id_tarea = 0;

        private void ValidarBuscar()
        {
            DateTime fechaDesdeAux;
            DateTime fechaHastaAux;
            int idAux = 0;

            this.id_solicitud = 0;
            this.nro_boleta = 0;
            this.fechaDesde = null;
            this.fechaHasta = null;
            this.id_tarea = 0;
            
            idAux = 0;
            int.TryParse(txtNroSolicitud.Text.Trim(), out idAux);
            this.id_solicitud = idAux;
            
            idAux = 0;
            int.TryParse(txtNroBoleta.Text.Trim(), out idAux);
            this.nro_boleta = idAux;

            if (!string.IsNullOrEmpty(txtFechaDesde.Text))
            {
                if (!DateTime.TryParse(txtFechaDesde.Text.Trim(), out fechaDesdeAux))
                    throw new Exception("Fecha tarea desde inválida.");

                this.fechaDesde = fechaDesdeAux;
            }

            if (!string.IsNullOrEmpty(txtFechaHasta.Text))
            {
                if (!DateTime.TryParse(txtFechaHasta.Text.Trim(), out fechaHastaAux))
                    throw new Exception("Fecha tarea hasta inválida.");

                this.fechaHasta = fechaHastaAux;
            }

            if (this.fechaDesde.HasValue && this.fechaHasta.HasValue && this.fechaDesde > this.fechaHasta)
                throw new Exception("Fecha desde superior a fecha hasta.");
            
            idAux = 0;
            int.TryParse(ddlTareas.SelectedItem.Value, out idAux);
            this.id_tarea = idAux;

            bool hayFiltroPorTramite = false;

            if (this.id_solicitud > 0 || this.nro_boleta > 0 || this.id_tarea >= 0 || this.fechaDesde.HasValue || this.fechaHasta.HasValue)
                hayFiltroPorTramite = true;

            if (!hayFiltroPorTramite)
                throw new Exception("Debe ingresar algún filtro de búsqueda.");

        }

        private void CargarAuditoria()
        {
            recuperarFiltro();

            DateTime fechaAux;
            var q =
                (
                    from log in db.SGI_Tarea_Pagos_log
                    join tt in db.SGI_Tramites_Tareas on log.id_tramitetarea equals tt.id_tramitetarea
                    join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                    join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea 
                    join pago in db.SGI_Solicitudes_Pagos on log.id_pago equals pago.id_pago into query_log from left_sol_pago in query_log.DefaultIfEmpty()

                    orderby tt_hab.id_solicitud, log.id
                    select new
                    {
                        log.id,
                        log.id_tramitetarea,
                        log.id_pago,
                        log.id_estado,
                        log.mensaje,
                        log.sistema,
                        log.fecha_inicio,
                        log.fecha_fin,
                        log.CreateUser,
                        tt.id_tarea,
                        tt_hab.id_solicitud,
                        t.nombre_tarea,
                        left_sol_pago.nro_boleta_unica,
                        monto_pago = ( left_sol_pago.monto_pago == null  ) ? 0: left_sol_pago.monto_pago
                    }

                );

            
            if (this.id_solicitud > 0)
            {
                q = q.Where(x => x.id_solicitud == this.id_solicitud);
            }

            if (this.nro_boleta > 0)
            {
                q = q.Where(x => x.nro_boleta_unica == this.nro_boleta);
            }

            if (this.id_tarea > 0)
            {
                q = q.Where(x => x.id_tarea == this.id_tarea);
            }

            if (this.fechaDesde.HasValue)
            {
                q = q.Where(x => x.fecha_inicio >= this.fechaDesde);
            }

            if (this.fechaHasta.HasValue)
            {
                fechaAux = this.fechaHasta.Value.AddDays(1);
                q = q.Where(x => x.fecha_inicio < fechaAux);
            }

            var lstBoletas = q.ToList();

            grdAuditoriaPagos.DataSource = lstBoletas;
            grdAuditoriaPagos.DataBind();

            int cantFilas = lstBoletas.Count();

            if (cantFilas > 1)
                lblCantRegistros.Text = string.Format("{0} Registros", cantFilas);
            else if (cantFilas == 1)
                lblCantRegistros.Text = string.Format("{0} Registro", cantFilas);
            else
            {
                pnlCantRegistros.Visible = false;
            }

            pnlResultadobuscar.Visible = true;

            updPnlResultadoBuscar.Update();

        }


        #endregion

        #region paginado grilla

        protected void grdAuditoriaPagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdAuditoriaPagos.PageIndex = e.NewPageIndex;
                IniciarEntity();
                CargarAuditoria();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }

        }

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;

            try
            {
                grdAuditoriaPagos.PageIndex = int.Parse(cmdPage.Text) - 1;
                IniciarEntity();
                CargarAuditoria();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {

            try
            {
                grdAuditoriaPagos.PageIndex = grdAuditoriaPagos.PageIndex - 1;
                IniciarEntity();
                CargarAuditoria();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {

            try
            {
                grdAuditoriaPagos.PageIndex = grdAuditoriaPagos.PageIndex + 1;
                IniciarEntity();
                CargarAuditoria();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void grdAuditoriaPagos_DataBound(object sender, EventArgs e)
        {
            try
            {

                GridView grid = (GridView)grdAuditoriaPagos;
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

                    UpdatePanel updPnlPager = (UpdatePanel)fila.Cells[0].FindControl("updPnlPager");
                    if (updPnlPager != null)
                        updPnlPager.Update();



                }

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }


        }

        private void elimiarFiltro()
        {
            ViewState["filtro"] = null;
        }

        private void guardarFiltro()
        {
            string filtro = this.id_solicitud + "|" + this.fechaDesde + "|" + this.fechaHasta + "|" + this.id_tarea;
            ViewState["filtro"] = filtro;

        }

        private void recuperarFiltro()
        {
            if (ViewState["filtro"] == null)
                return;

            string filtro = ViewState["filtro"].ToString();

            string[] valores = filtro.Split('|');

            this.id_solicitud = Convert.ToInt32(valores[0]);

            this.id_tarea = Convert.ToInt32(valores[3]);

            if (string.IsNullOrEmpty(valores[1]))
            {
                this.fechaDesde = null;
            }
            else
            {
                this.fechaDesde = Convert.ToDateTime(valores[1]);
            }
            if (string.IsNullOrEmpty(valores[2]))
            {
                this.fechaHasta = null;
            }
            else
            {
                this.fechaHasta = Convert.ToDateTime(valores[2]);
            }

        }

        #endregion


    }

}