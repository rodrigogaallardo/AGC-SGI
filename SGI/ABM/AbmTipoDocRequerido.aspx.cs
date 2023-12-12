using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SGI.Model;
using System.Transactions;
using System.Web.Configuration;
using System.Configuration;

namespace SGI.ABM
{
    public partial class AbmTipoDocRequerido : BasePage
    {
        DGHP_Entities db = null;

        #region load de pagina
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {

                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "inicializar_controles", "inicializar_controles();", true);
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "init_updDatos", "init_Js_updDatos();", true);

            } 
            if (!IsPostBack)
            {
                db = new DGHP_Entities();
                CargarCombos();//rellena los dropdownlist de filtros
                CargarComboDocumentos();//carga los tipos de documentos
                                        //CargarZonasHabil();
                                        //  MostrarTitulos();

                CargarSeteoMaximoTamanio();


            }
        }
        #endregion

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        protected void btnNuevoTipo_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarDatos();
                txtTamano.Text = "2";
                txtFormato.Text = "pdf";
                txtAcronimoSade.Text = "IFGRA";
                updDatos.Update();
                this.EjecutarScript(UpdatePanel1, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "finalizarCarga();showfrmError();");

            }
        }
        //Agregados rellena los filtros
        private void CargarCombos()
        {
            ListItem opALL = new ListItem("Todos","0");
            ListItem opSI = new ListItem("Sí", "1");
            ListItem opNO = new ListItem("No", "2");

            ddlBajaLogica2.Items.Insert(0, opNO);
            ddlBajaLogica2.Items.Insert(0, opSI);
            ddlBajaLogica2.Items.Insert(0, opALL);

            ddlRequiereDetalle.Items.Insert(0, opNO);
            ddlRequiereDetalle.Items.Insert(0, opSI);
            ddlRequiereDetalle.Items.Insert(0, opALL);

            ddlVisibleSGI.Items.Insert(0, opNO);
            ddlVisibleSGI.Items.Insert(0, opSI);
            ddlVisibleSGI.Items.Insert(0, opALL);

            ddlVisibleSSIT.Items.Insert(0, opNO);
            ddlVisibleSSIT.Items.Insert(0, opSI);
            ddlVisibleSSIT.Items.Insert(0, opALL);

            ddlVisibleAT.Items.Insert(0, opNO);
            ddlVisibleAT.Items.Insert(0, opSI);
            ddlVisibleAT.Items.Insert(0, opALL);

            ddlVisibleObservaciones.Items.Insert(0, opNO);
            ddlVisibleObservaciones.Items.Insert(0, opSI);
            ddlVisibleObservaciones.Items.Insert(0, opALL);

            ddlVerificarFirma.Items.Insert(0, opNO);
            ddlVerificarFirma.Items.Insert(0, opSI);
            ddlVerificarFirma.Items.Insert(0, opALL);
        }
        public void CargarComboDocumentos()
        {
            db = new DGHP_Entities();
            var lstDocumentos = (from doc in db.TiposDeDocumentosRequeridos
                               select new
                               {
                                   doc.id_tdocreq,
                                   doc.nombre_tdocreq
                               }).Distinct().OrderBy(x => x.nombre_tdocreq).ToList();

            ddlNombreDoc.DataSource = lstDocumentos;
            ddlNombreDoc.DataTextField = "nombre_tdocreq";
            ddlNombreDoc.DataValueField = "id_tdocreq";
            ddlNombreDoc.DataBind();
            ListItem NombreDocTodos = new ListItem("Todos", "-1");
            ddlNombreDoc.Items.Insert(0, NombreDocTodos);

            db.Dispose();

        }

        public void CargarSeteoMaximoTamanio()
        {
            //leer key del config para tamaño maximo de archivos
            HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            var maximo = (section.MaxRequestLength / 1024) / 1024;
            lblTamanomax.Text = "El Tamaño maximo del archivo no puede superar los " + maximo.ToString() + " MB.";

            hdnTamanioMaximo.Value = maximo.ToString();
        }

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            LimpiarDatosBusqueda();
            EjecutarScript(UpdatePanel1, "hideResultados();");
        }


        private void LimpiarDatosBusqueda()
        {
            ddlBajaLogica2.ClearSelection();
            ddlRequiereDetalle.ClearSelection();
            ddlNombreDoc.ClearSelection();
            ddlVisibleSGI.ClearSelection();
            ddlVisibleSSIT.ClearSelection();
            ddlVisibleObservaciones.ClearSelection();
            ddlVerificarFirma.ClearSelection();
            txtObservacion.Text = string.Empty;
            updpnlBuscar.Update();
            pnlResultadoBuscar.Visible = false;
        }

        private void LimpiarDatos()
        {
            hid_id_tipoDocReq.Value = "0";
            txtNombreDocReq.Text = "";
            cBoxRequiereDetalle.Checked = false;
            cBoxVisible_en_sgi.Checked = false;
            cBoxVisible_en_ssit.Checked = false;
            cBoxVisible_en_Obs.Checked = false;
            txtEditObserv.Text = "";
            ddlBajaLogica.SelectedIndex = 0;
            txtTamano.Text = "";
            txtFormato.Text = "";
            txtAcronimoSade.Text = "";
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                grdResultados.DataBind();
                pnlResultadoBuscar.Visible = true;
                updPnlResultadoBuscar.Update();
                EjecutarScript(UpdatePanel1, "showResultados();");
                db.Dispose();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "showfrmError();");
            }
        }

        public List<clsTiposdeDocumentosRequeridos> GetResultados(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {

            totalRowCount = 0;

            int aux = 0;

            int intidNombreDoc = -1;
            int intrequieredetalle = 0;
            int intvisiblesgi = 0;
            int intvisiblessit = 0;
            int intvisibleat = 0;
            int intvisibleobs = 0;
            int intbajalogica = 0;
            int intverificar = 0;

            int.TryParse(ddlNombreDoc.SelectedItem.Value, out aux);
            intidNombreDoc = aux;

            int.TryParse(ddlRequiereDetalle.SelectedItem.Value, out aux);
            intrequieredetalle = aux;

            int.TryParse(ddlBajaLogica2.SelectedItem.Value, out aux);
            intbajalogica = aux;

            int.TryParse(ddlVisibleObservaciones.SelectedItem.Value, out aux);
            intvisibleobs = aux;

            int.TryParse(ddlVisibleSGI.SelectedItem.Value, out aux);
            intvisiblesgi = aux;

            int.TryParse(ddlVisibleSSIT.SelectedItem.Value, out aux);
            intvisiblessit = aux;

            int.TryParse(ddlVerificarFirma.SelectedItem.Value, out aux);
            intverificar = aux;

            int.TryParse(ddlVisibleAT.SelectedItem.Value, out aux);
            intvisibleat = aux;

            List<clsTiposdeDocumentosRequeridos> lstResult = new List<clsTiposdeDocumentosRequeridos>();
            IQueryable<clsTiposdeDocumentosRequeridos> q = null;

            db = new DGHP_Entities();


            q = (from tipos in db.TiposDeDocumentosRequeridos
                 select new clsTiposdeDocumentosRequeridos
                 {
                     id_tdocreq = tipos.id_tdocreq,
                     nombre_tdocreq = tipos.nombre_tdocreq,
                     observaciones_tdocreq = tipos.observaciones_tdocreq,
                     RequiereDetalle = (tipos.RequiereDetalle ? "Sí" : "No"),
                     visible_en_SGI = (tipos.visible_en_SGI ? "Sí" : "No"),
                     visible_en_SSIT = (tipos.visible_en_SSIT ? "Sí" : "No"),
                     visible_en_Obs = (tipos.visible_en_Obs ? "Sí" : "No"),
                     visible_en_AT = (tipos.visible_en_AT ? "Sí" : "No"),
                     baja_tdocreq = (tipos.baja_tdocreq ? "Sí" : "No"),
                     verificar_firma_digital = (tipos.verificar_firma_digital ? "Sí" : "No")
                 }).Distinct();


            if (intidNombreDoc > -1)
                q = q.Where(x => x.id_tdocreq == intidNombreDoc);

            if (intrequieredetalle > 0)
                q = q.Where(x => x.RequiereDetalle == ddlRequiereDetalle.SelectedItem.Text);

            if (intbajalogica > 0)
                q = q.Where(x => x.baja_tdocreq == ddlBajaLogica2.SelectedItem.Text);

            if (intvisibleobs > 0)
                q = q.Where(x => x.visible_en_Obs == ddlVisibleObservaciones.SelectedItem.Text);

            if (intvisiblesgi > 0)
                q = q.Where(x => x.visible_en_SGI == ddlVisibleSGI.SelectedItem.Text);

            if (intvisiblessit > 0)
                q = q.Where(x => x.visible_en_SSIT == ddlVisibleSSIT.SelectedItem.Text);
            if (intvisibleat > 0)
                q = q.Where(x => x.visible_en_AT == ddlVisibleAT.SelectedItem.Text);
            
            if (!string.IsNullOrEmpty(txtObservacion.Text.Trim()) && txtObservacion.Text.Length > 1)
                q = q.Where(x => x.observaciones_tdocreq.Contains(txtObservacion.Text.Trim()));

            if (intverificar > 0)
                q = q.Where(x => x.verificar_firma_digital == ddlVerificarFirma.SelectedItem.Text);

            totalRowCount = q.Count();

            q = q.OrderBy(o => o.nombre_tdocreq).Skip(startRowIndex).Take(maximumRows);

            lstResult = q.ToList();

            pnlCantRegistros.Visible = true;

            if (totalRowCount > 1)
            {
                lblCantRegistros.Text = string.Format("{0} Tipos de Documentos Requeridos", totalRowCount);
            }
            else if (totalRowCount == 1)
                lblCantRegistros.Text = string.Format("{0}  Tipos de Documentos Requeridos", totalRowCount);
            else
            {
                pnlCantRegistros.Visible = false;
            }
            pnlResultadoBuscar.Visible = true;
            updPnlResultadoBuscar.Update();

            return lstResult;
        }

        private void CargarDatos(int id_datos)
        {

            db = new DGHP_Entities();           

            var dato = db.TiposDeDocumentosRequeridos.FirstOrDefault(x => x.id_tdocreq == id_datos);
            if (dato != null)
            {
                hid_id_tipoDocReq.Value = id_datos.ToString();

                txtNombreDocReq.Text = dato.nombre_tdocreq;
                cBoxRequiereDetalle.Checked = dato.RequiereDetalle;
                txtEditObserv.Text = dato.observaciones_tdocreq;
                cBoxVisible_en_ssit.Checked = dato.visible_en_SSIT;
                cBoxVisible_en_sgi.Checked = dato.visible_en_SGI;
                cBoxVisible_en_at.Checked = dato.visible_en_AT;
                cBoxVisible_en_Obs.Checked = dato.visible_en_Obs;
                cBoxVerificar_firma.Checked = dato.verificar_firma_digital;
                txtTamano.Text = dato.tamanio_maximo_mb.ToString();
                txtAcronimoSade.Text = dato.acronimo_SADE;
                txtFormato.Text = dato.formato_archivo;

                bool baja = dato.baja_tdocreq;
                if (baja)
                {   //si esta dado de baja lo damos la posibilidad de reactivarlo
                    pnlBajaLogica.Visible = true;
                    ddlBajaLogica.SelectedIndex = 1;
                }
            }
            db.Dispose();
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEditar = (LinkButton)sender;
                int id_datos = int.Parse(btnEditar.CommandArgument);

                LimpiarDatos();
                CargarDatos(id_datos);
                updDatos.Update();
                this.EjecutarScript(updPnlResultadoBuscar, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updPnlResultadoBuscar, "showfrmError();");

            }

        }

        protected void lnkEliminarTipoDocReq_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;

                LinkButton lnkEditar = (LinkButton)sender;
                int idTipoDocReq = int.Parse(lnkEditar.CommandArgument);

                db = new DGHP_Entities();

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        db.TiposDeDocumentosRequeridos_delete(idTipoDocReq, userid);

                        Tran.Complete();
                        string script = "$('#frmEliminarLog').modal('show');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }
                grdResultados.DataBind();

                updPnlResultadoBuscar.Update();
                this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updPnlResultadoBuscar, "showfrmError();");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            db = new DGHP_Entities();

            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
                string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();

                int idTipoDocReq = Convert.ToInt32(hid_id_tipoDocReq.Value);
                string nombre = txtNombreDocReq.Text.Trim();
                bool requiereDetalle = cBoxRequiereDetalle.Checked;
                string observ = txtEditObserv.Text.Trim();
                bool baja = Convert.ToBoolean(ddlBajaLogica.SelectedValue);
                bool visible_en_SSIT = cBoxVisible_en_ssit.Checked;
                bool visible_en_SGI = cBoxVisible_en_sgi.Checked;
                bool visible_en_AT = cBoxVisible_en_at.Checked;
                bool visible_en_OBS = cBoxVisible_en_Obs.Checked;
                bool verificar_firma = cBoxVerificar_firma.Checked;
                int tamano = 0;
                int.TryParse(txtTamano.Text.Trim(), out tamano);
                string formato = txtFormato.Text.Trim();
                string acronimo = txtAcronimoSade.Text.Trim();

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        if (idTipoDocReq == 0)
                        {
                            db.TiposDeDocumentosRequeridos_insert(nombre, observ, userid, requiereDetalle, visible_en_SSIT, visible_en_SGI, visible_en_AT, visible_en_OBS, verificar_firma, tamano, formato, acronimo);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, string.Empty, url, txtObservacionesSolicitante.Text, "I");

                        }
                        else
                        {
                            db.TiposDeDocumentosRequeridos_update(idTipoDocReq, nombre, observ, baja, userid, requiereDetalle, visible_en_SSIT, visible_en_SGI, visible_en_AT, visible_en_OBS, verificar_firma, tamano, formato, acronimo);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, string.Empty, url, txtObservacionesSolicitante.Text, "U");

                        }
                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }


                grdResultados.DataBind();
                CargarComboDocumentos();
                updPnlResultadoBuscar.Update();
                updpnlBuscar.Update();

                this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "showBusqued", "showBusqueda();", true);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updBotonesGuardar, "showfrmError();");

            }
            finally
            {
                db.Dispose();
            }

        }

        #region configuracion de la grilla

        protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //this.db
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }

        }

        protected void grdResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResultados.PageIndex = e.NewPageIndex;
        }

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;
            grdResultados.PageIndex = int.Parse(cmdPage.Text) - 1;
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdResultados.PageIndex = grdResultados.PageIndex - 1;
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdResultados.PageIndex = grdResultados.PageIndex + 1;
        }

        protected void grdResultados_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)grdResultados;
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

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, string.Empty, url, txtObservacionesSolicitanteEliminar.Text, "D");

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, string.Empty, url, string.Empty, "D");


        }

        #endregion
    }
}