using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI
{
    public partial class BuscarPagos : BasePage
    {

        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar, updPnlFiltroBuscar.GetType(),
                    "inicializar_controles", "inicializar_controles();", true);
            }

            if (!IsPostBack)
            {
                limpiarCampos();
                //txtFechaDesde.Text = "01/10/2013";
                cargarCombos();
            }

        }

        private void cargarCombos()
        {
            IniciarEntity();
            List<wsPagos_BoletaUnica_Estados> lista = this.db.wsPagos_BoletaUnica_Estados.ToList();

            wsPagos_BoletaUnica_Estados ws_p = new wsPagos_BoletaUnica_Estados();
            ws_p.id_estadopago = -1;
            ws_p.nom_estadopago = "Todos";
            lista.Insert(0, ws_p);
            ddlEstadoPago.DataTextField = "nom_estadopago";
            ddlEstadoPago.DataValueField = "id_estadopago";
            ddlEstadoPago.DataSource = lista;
            ddlEstadoPago.DataBind();

            FinalizarEntity();
        }

        private void CargarDatos()
        {
            IniciarEntity();

            puedeCambiarEstado = PuedeCambiarEstado();
            if (puedeCambiarEstado) 
                hid_puede_modificar.Value = "true";
            else
                hid_puede_modificar.Value = "false";

            FinalizarEntity();
        }

        private bool puedeCambiarEstado = false;
        private bool editarApra = false;
        private bool editarSGI = false;
        private bool puedeVerHistorial = false;

        private bool PuedeCambiarEstado()
        {
            Guid userid = Functions.GetUserId();
            bool cambiaEstado = false;
            var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

            foreach (var perfil in perfiles_usuario)
            {
                var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

                 if (menu_usuario.Contains("Editar Pagos"))
                    cambiaEstado = true;
                 if (menu_usuario.Contains("APRA"))
                     editarApra = true;
                 if (menu_usuario.Contains("AGC"))
                     editarSGI = true;
                 if (menu_usuario.Contains("Historial de estados"))
                     puedeVerHistorial = true;
               
            }
          
            return cambiaEstado;
        }

        private void Enviar_Mensaje_Buscar(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);

            string script_nombre = "mostrarMensaje";
            string script = "mostrarMensaje('" + mensaje + "','" + titulo + "');";

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm != null && sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar, updPnlFiltroBuscar.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
            }
        }

        private void Enviar_Mensaje_actualizar(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode("Buscar Pagos");

            //ScriptManager sm = ScriptManager.GetCurrent(this);

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }


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
                this.db.Dispose();
        }

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
                this.dbFiles.Dispose();
        }

        #endregion

        #region grilla pagos

        protected void grdPagos_DataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void grdPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                try
                {

                    var datos = (clsItemPagos)e.Row.DataItem;
                    
                    LinkButton lnkHistorial = (LinkButton)e.Row.FindControl("lnkHistorial");
                    Panel pnlHistorial = (Panel)e.Row.FindControl("pnlHistorial");
                    lnkHistorial.Attributes.Add("href", "#" + pnlHistorial.ClientID);
                    
                    int id_pago_BU = int.Parse(lnkHistorial.CommandArgument);

                    HyperLink lnkActualizar = (HyperLink)e.Row.FindControl("lnkActualizar");
                    Panel pnlActualizarPago = (Panel)e.Row.FindControl("pnlActualizarPago");
                    lnkActualizar.NavigateUrl = "#" + pnlActualizarPago.ClientID;

                    Button btnGuardarFechaPago = (Button)e.Row.FindControl("btnGuardarFechaPago");
                    DropDownList ddlEstadoPagoEdit = (DropDownList)e.Row.FindControl("ddlEstadoPagoEdit");
                    cargarComboEstadoPago(ddlEstadoPagoEdit);

                    TextBox txtFechaPago = (TextBox)e.Row.FindControl("txtFechaPago");

                    switch (e.Row.Cells[5].Text.ToLower())
                    {
                        case "pagada":
                            ddlEstadoPagoEdit.SelectedIndex = 2;
                            break;

                        case "pendiente de pago":
                            ddlEstadoPagoEdit.SelectedIndex = 1;
                            break;

                        case "vencida":
                            ddlEstadoPagoEdit.SelectedIndex = 3;
                            break;

                        case "cancelada":
                            ddlEstadoPagoEdit.SelectedIndex = 4;
                            break;

                        case "anulada":
                            ddlEstadoPagoEdit.SelectedIndex = 5;
                            break;

                        default:
                            ddlEstadoPagoEdit.SelectedIndex = 0;
                            break;

                    }

                    txtFechaPago.Text = System.Web.HttpUtility.HtmlEncode(  e.Row.Cells[5].Text ) ;
                    //btnGuardarFechaPago.ValidationGroup = string.Format("val_actualizar_{0}", e.Row.RowIndex);
                    //ddlEstadoPagoEdit.ValidationGroup = btnGuardarFechaPago.ValidationGroup;
                    //txtFechaPago.ValidationGroup = btnGuardarFechaPago.ValidationGroup;
                    //bool permitePermiso = false;

                    //if (datos.UpdateVisible)
                    //    permitePermiso = true;
                    

                    lnkActualizar.Visible = this.puedeCambiarEstado && !e.Row.Cells[2].Text.Contains("APRA");

                    //lnkActualizar.Visible = !(permitePermiso && this.editarSGI);
                    lnkHistorial.Visible = this.puedeVerHistorial;

                    DGHP_Entities db = new DGHP_Entities();
                    var q = (from hist in db.wsPagos_BoletaUnica_HistorialEstados2
                             join estAnt in db.wsPagos_BoletaUnica_Estados on hist.id_estadopago_ant equals estAnt.id_estadopago
                             join estNuevo in db.wsPagos_BoletaUnica_Estados on hist.id_estadopago_nuevo equals estNuevo.id_estadopago
                             where hist.id_pago_bu == id_pago_BU
                             select new {
                                 estadoAnterior = estAnt.nom_estadopago,
                                 estadoNuevo = estNuevo.nom_estadopago,
                                 hist.CreateUser,
                                 hist.CreateDate

                             });

                    Table tHistorial = (Table)e.Row.FindControl("tblHistorial");

                    foreach (var fila in q) {
                        TableRow row = new TableRow();
                        TableCell cellFecha = new TableCell();
                        TableCell cellEstadoAnt = new TableCell();
                        TableCell cellEstadoNuevo = new TableCell();
                        TableCell cellUser = new TableCell();
                        cellFecha.Text = fila.CreateDate.ToString("d");
                        cellEstadoAnt.Text = fila.estadoAnterior;
                        cellEstadoNuevo.Text = fila.estadoNuevo;
                        cellUser.Text = fila.CreateUser;
                        row.Cells.Add(cellFecha);
                        row.Cells.Add(cellEstadoAnt);
                        row.Cells.Add(cellEstadoNuevo);
                        row.Cells.Add(cellUser);
                        tHistorial.Rows.Add(row);
                    }
                    tHistorial.DataBind();
                }
                catch (Exception ex)
                {
                    string aa = ex.Message;
                }

            }

        }
       

       
       
        /*private void cargarPermisos()
        {
            db = new DGHP_Entities();
            Guid userid = Functions.GetUserId();

            var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

            foreach (var perfil in perfiles_usuario)
            {
                var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

                if (menu_usuario.Contains("APRA"))
                    editarApra = true;
                if (menu_usuario.Contains("SGI"))
                {
                    editarSGI = true;
                   
                }
                

            }
        }*/

        
        private void cargarComboEstadoPago(DropDownList ddlEstadoPago){
            DGHP_Entities db = new DGHP_Entities();
            List<wsPagos_BoletaUnica_Estados> lista = db.wsPagos_BoletaUnica_Estados.ToList();

            wsPagos_BoletaUnica_Estados ws_p = new wsPagos_BoletaUnica_Estados();
            ws_p.id_estadopago = -1;
            ws_p.nom_estadopago = "Seleccione";
            lista.Insert(0, ws_p);
            ddlEstadoPago.DataTextField = "nom_estadopago";
            ddlEstadoPago.DataValueField = "id_estadopago";
            ddlEstadoPago.DataSource = lista;
            ddlEstadoPago.DataBind();

            db.Dispose();
        }

        #region paginado grilla

        protected void grdPagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdPagos.PageIndex = e.NewPageIndex;
                IniciarEntity();
                CargarPagos();
                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                Enviar_Mensaje_Buscar(ex.Message, "");
            }

        }

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;

            try
            {
                grdPagos.PageIndex = int.Parse(cmdPage.Text) - 1;
                IniciarEntity();

                CargarPagos();

                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                Enviar_Mensaje_Buscar(ex.Message, "");
            }
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {

            try
            {
                grdPagos.PageIndex = grdPagos.PageIndex - 1;
                IniciarEntity();

                CargarPagos();

                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                Enviar_Mensaje_Buscar(ex.Message, "");
            }
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {

            try
            {
                grdPagos.PageIndex = grdPagos.PageIndex + 1;
                IniciarEntity();
                CargarPagos();
                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                Enviar_Mensaje_Buscar(ex.Message, "");
            }
        }

        protected void grdPagos_DataBound(object sender, EventArgs e)
        {
            try
            {

            GridView grid = (GridView)grdPagos;
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
                if (updPnlPager!= null)
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
            string filtro = this.nroBU + "|" +  this.fechaDesde + "|" + this.fechaHasta + "|" + this.id_estado_pago + "|" + this.idPago
                + "|" + this.fechaPagoDesde + "|" + this.fechaPagoHasta;
            ViewState["filtro"] = filtro;

        }

        private void recuperarFiltro()
        {
            if (ViewState["filtro"] == null)
                return;

            string filtro = ViewState["filtro"].ToString();

            string[] valores = filtro.Split('|');

            this.nroBU = Convert.ToInt32(valores[0]);
            
            this.idPago = Convert.ToInt32(valores[4]);

            this.id_estado_pago = Convert.ToInt32(valores[3]);

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

            if (string.IsNullOrEmpty(valores[5]))
            {
                this.fechaPagoDesde = null;
            }
            else
            {
                this.fechaPagoDesde = Convert.ToDateTime(valores[5]);
            }
            if (string.IsNullOrEmpty(valores[6]))
            {
                this.fechaPagoHasta = null;
            }
            else
            {
                this.fechaPagoHasta = Convert.ToDateTime(valores[6]);
            }

        }

        #endregion

        #endregion

        #region buscar tramite

        private void limpiarCampos()
        {
            txtNroBU.Text = "";
            txtId.Text = "";
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            txtFechaPagoDesde.Text = "";
            txtFechaPagoHasta.Text = "";

            ddlEstadoPago.ClearSelection();

            updPnlFiltroBuscar.Update();
        }

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {

            EjecutarScript(updPnlBuscarPagos, "hideResultado();");
            limpiarCampos();

        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                elimiarFiltro();

                IniciarEntity();

                ValidarBuscar();
                CargarPagos();

                FinalizarEntity();

                guardarFiltro();
                EjecutarScript(updPnlBuscarPagos, "showResultado();");

            }
            catch (Exception ex)
            {
                FinalizarEntity();
                LogError.Write(ex, "error al buscar instalaciones buscar_tramite-btnBuscar_OnClick");
                Enviar_Mensaje_Buscar(ex.Message, "");
            }



        }

        private int nroBU = 0;
        private int idPago = 0;
        private DateTime? fechaDesde;
        private DateTime? fechaHasta;
        private DateTime? fechaPagoDesde;
        private DateTime? fechaPagoHasta;
        private int id_estado_pago = -1;

        private void ValidarBuscar()
        {
            DateTime fechaDesdeAux;
            DateTime fechaHastaAux;
            DateTime fechaPagoDesdeAux;
            DateTime fechaPagoHastaAux;
            int idAux = 0;

            this.fechaDesde = null;
            this.fechaHasta = null;
            this.fechaPagoDesde = null;
            this.fechaPagoHasta = null;
            this.id_estado_pago = -1;

            idAux = 0;
            int.TryParse(txtNroBU.Text.Trim(), out idAux);
            this.nroBU = idAux;

            idAux = 0;
            int.TryParse(txtId.Text.Trim(), out idAux);
            this.idPago = idAux;


            if (!string.IsNullOrEmpty(txtFechaDesde.Text))
            {
                if (!DateTime.TryParse(txtFechaDesde.Text.Trim(), out fechaDesdeAux))
                    throw new Exception("Fecha Creacion Desde inválida.");

                this.fechaDesde = fechaDesdeAux;
            }

            if (!string.IsNullOrEmpty(txtFechaHasta.Text))
            {
                if (!DateTime.TryParse(txtFechaHasta.Text.Trim(), out fechaHastaAux))
                    throw new Exception("Fecha Creacion Hasta inválida.");

                this.fechaHasta = fechaHastaAux;
            }

            if (this.fechaDesde.HasValue && this.fechaHasta.HasValue && this.fechaDesde > this.fechaHasta)
                throw new Exception("Fecha Creacion Desde superior a Fecha Creacion Hasta.");

            if (!string.IsNullOrEmpty(txtFechaPagoDesde.Text))
            {
                if (!DateTime.TryParse(txtFechaPagoDesde.Text.Trim(), out fechaPagoDesdeAux))
                    throw new Exception("Fecha Pado Desde inválida.");

                this.fechaPagoDesde = fechaPagoDesdeAux;
            }

            if (!string.IsNullOrEmpty(txtFechaPagoHasta.Text))
            {
                if (!DateTime.TryParse(txtFechaPagoHasta.Text.Trim(), out fechaPagoHastaAux))
                    throw new Exception("Fecha Pago Hasta inválida.");

                this.fechaPagoHasta = fechaPagoHastaAux;
            }

            if (this.fechaPagoDesde.HasValue && this.fechaPagoHasta.HasValue && this.fechaPagoDesde > this.fechaPagoHasta)
                throw new Exception("Fecha Pago Desde superior a Fecha Pago Hasta.");

            idAux = 0;
            int.TryParse( ddlEstadoPago.SelectedItem.Value, out idAux);
            this.id_estado_pago = idAux;

            bool hayFiltroPorTramite = false;

            if (this.nroBU > 0 || this.idPago > 0 || this.id_estado_pago >= 0 || this.fechaDesde.HasValue || this.fechaHasta.HasValue
                || this.fechaPagoDesde.HasValue || this.fechaPagoHasta.HasValue)
                hayFiltroPorTramite = true;

            if (! hayFiltroPorTramite )
                throw new Exception("Debe ingresar algún filtro de búsqueda.");

        }



        private void CargarPagos()
        {
            recuperarFiltro();

            this.puedeCambiarEstado = PuedeCambiarEstado();

            DateTime fechaAux;
            var q =
                (
                    from bu in db.wsPagos_BoletaUnica
                    join est in db.wsPagos_BoletaUnica_Estados on bu.EstadoPago_BU equals est.id_estadopago
                    join pago in db.wsPagos on bu.id_pago equals pago.id_pago
                    orderby bu.id_pago
                    select new clsItemPagos
                    {
                        apellidoyNombre = pago.ApellidoyNombre,
                        CreateDate = pago.CreateDate,
                        id_pago_BU = bu.id_pago_BU,
                        id_pago = bu.id_pago,
                        Numero_BU = bu.Numero_BU,
                        nroDependencia_BU = bu.NroDependencia_BU,
                        codigoBarras_BU = bu.CodigoBarras_BU,
                        monto_BU = bu.Monto_BU,
                        FechaPago_BU = bu.FechaPago_BU,
                        trazaPago_BU = bu.TrazaPago_BU,
                        EstadoPago_BU = bu.EstadoPago_BU,
                        estado_desc = est.nom_estadopago,
                        verificador_BU = bu.verificador_BU,
                        loginModif = bu.UpdateUser,
                        createUser = pago.CreateUser,
                        UpdateVisible = bu.NroDependencia_BU.ToString().Substring(2) == "44" ? true : false
                        //UpdateVisible = pago.CreateUser != "WS-APRA"?true:false
                    }

                );

            if (this.nroBU > 0)
            {
                q = q.Where(x=>x.Numero_BU == this.nroBU );
            }

            if (this.idPago > 0)
            {
                q = q.Where(x => x.id_pago == this.idPago);
            }

            if (this.id_estado_pago >= 0)
            {
                q = q.Where(x => x.EstadoPago_BU == this.id_estado_pago);
            }

            if (this.fechaDesde.HasValue )
            {
                q = q.Where(x => x.CreateDate >= this.fechaDesde);
            }

            if (this.fechaHasta.HasValue)
            {
                fechaAux = this.fechaHasta.Value.AddDays(1);
                q = q.Where(x => x.CreateDate < fechaAux);
            }

            if (this.fechaPagoDesde.HasValue)
            {
                q = q.Where(x => x.FechaPago_BU >= this.fechaPagoDesde);
            }

            if (this.fechaPagoHasta.HasValue)
            {
                fechaAux = this.fechaPagoHasta.Value.AddDays(1);
                q = q.Where(x => x.FechaPago_BU < fechaAux);
            }

            var lstBoletas = q.ToList();

            grdPagos.DataSource = lstBoletas;
            grdPagos.DataBind();

            int cantFilas = lstBoletas.Count();
            pnlCantRegistros.Visible = true;

            if (cantFilas > 1)
                lblCantRegistros.Text = string.Format("{0} Boletas", cantFilas);
            else if (cantFilas == 1)
                lblCantRegistros.Text = string.Format("{0} Boleta", cantFilas);
            else
            {
                pnlCantRegistros.Visible = false;
            }

            guardarFiltro();

            updPnlResultadoBuscar.Update();
        }

        #endregion

        protected void ddlEstadoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idEstadoPago = Convert.ToInt32(ddlEstadoPago.SelectedValue);
            lblFechaPagoDesde.Visible = idEstadoPago == 1;
            txtFechaPagoDesde.Visible = idEstadoPago == 1;
            lblFechaPagoHasta.Visible = idEstadoPago == 1;
            txtFechaPagoHasta.Visible = idEstadoPago == 1;
            updPnlFiltroBuscar.Update();
        }

        #region actualizar estado de pago

        private int pago_id_pago = 0;
        private DateTime? pago_fecha_pago ;
        private void ValidarActualizar(string strEstado, string strFechaPago)
        {
            this.pago_id_pago = -1;
            this.pago_fecha_pago = null;

            if (String.IsNullOrEmpty(strEstado))
                throw new Exception("Debe seleccionar el estado de pago.");

            int aux = 0;
            if ( ! int.TryParse(strEstado, out aux) )
                throw new Exception("Estado de pago inválido.");
            this.pago_id_pago = aux;

            if (!string.IsNullOrEmpty(strFechaPago))
            {
                DateTime fecha;
                if ( ! DateTime.TryParse(strFechaPago, out fecha) )
                    throw new Exception("Fecha de pago inválida.");

                if (fecha > DateTime.Today)
                    throw new Exception("No puede ingresar una fecha de pago futura.");

                this.pago_fecha_pago = fecha;
            }

            if ( this.pago_id_pago < 0 )
                throw new Exception("Debe seleccionar el estado de pago.");

            if (this.pago_id_pago == 1 && ! this.pago_fecha_pago.HasValue)
                    throw new Exception("Debe indicar la fecha de pago para cambiar a estado pagado.");
            
            if (this.pago_id_pago != 1 && this.pago_fecha_pago.HasValue)
                    throw new Exception("No corresponde ingresar fecha de pago para cambiar a estado pendiente de pago o vencido.");
            

        }

        private void GuardarEstadoPago(int id_pago_BU, int id_estado_pago, DateTime? fecha_pago, Guid userId, string login)
        {

            db.wsPagos_BoletaUnica_ActualizarPago_Manual(id_pago_BU, fecha_pago, id_estado_pago, userId.ToString(), login);

        }

        protected void btnActualizar_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Guid userid = Functions.GetUserId();
                string loginUsuario = System.Web.Security.Membership.GetUser().UserName;
                
                Button btnGuardarFechaPago = (Button)sender;
                GridViewRow row = (GridViewRow)btnGuardarFechaPago.Parent.Parent.Parent.Parent.Parent;
                DropDownList ddlEstadoPagoEdit = (DropDownList)row.FindControl("ddlEstadoPagoEdit");

                TextBox txtFechaPago = (TextBox)row.FindControl("txtFechaPago");

                UpdatePanel updPnlActualizar = (UpdatePanel)row.FindControl("updPnlActualizar");

                int id_pago = Convert.ToInt32(grdPagos.DataKeys[row.RowIndex].Values[1]);

                int id_pago_BU = int.Parse(btnGuardarFechaPago.CommandArgument);
                string idPanelModal = btnGuardarFechaPago.Parent.ClientID;

                ValidarActualizar(ddlEstadoPagoEdit.SelectedItem.Value, txtFechaPago.Text);

                IniciarEntity();

                TransactionScope Tran = new TransactionScope();

                try
                {
                    GuardarEstadoPago(id_pago_BU, this.pago_id_pago, this.pago_fecha_pago, userid, loginUsuario);

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    if (Tran != null) Tran.Dispose();
                    LogError.Write(ex, "error en transaccion. admin pagos-btnActualizar_Command");
                    throw ex;
                }

                CargarPagos();

                FinalizarEntity();


                ScriptManager.RegisterClientScriptBlock(updPnlResultadoBuscar, updPnlResultadoBuscar.GetType(),
                        "inicializarControlesModal", "inicializarControlesModal();", true);
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                Enviar_Mensaje_actualizar(ex.Message, "");
            }

        }

        #endregion
    }
}