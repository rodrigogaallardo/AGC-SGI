using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SGI.Model;
using System.Transactions;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data.Entity.SqlServer;

namespace SGI.ABM
{
    public partial class AbmPersonasInhibidas : BasePage
    {
        enum TipoPersona { Fisica = 1, Juridica = 2 }

        DGHP_Entities db = null;

        #region load de pagina
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "init_Js_updDatos", "init_Js_updDatos();", true);
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);

            }

            if (!IsPostBack)
            {
                db = new DGHP_Entities();
                CargarTiposDeDocumento();
            }
        }
        #endregion

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        protected void btnNueva_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarDatos();
                updDatos.Update();
                EjecutarScript(UpdatePanel1, "Js_autonum();showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "finalizarCarga();showfrmError();");

            }
        }

        private void LimpiarDatosBusqueda()
        {
            ddlTipoDocumento.ClearSelection();
            txtNroDocumento.Text = "";
            txtApellidoyNombre.Text = "";
            txtCuit.Text = "";
        }

        private void LimpiarDatos()
        {
            txtTipoPersona.SelectedValue = string.Empty;
            txtTipoDocumentoReq.Enabled = true;
            txtNroDocumentoReq.Enabled = true;

            hid_id_personainhibidaReq.Value = "0";
            txtTipoDocumentoReq.Text = string.Empty;
            txtNroDocumentoReq.Text = string.Empty;
            txtNombreyApellidoReq.Text = string.Empty;
            txtAutosReq.Text = string.Empty;
            txtCuitReq.Text = string.Empty;
            txtEstadoReq.SelectedValue = string.Empty;
            txtFechaBajaReq.Text = string.Empty;
            txtFechaRegistroReq.Text = string.Empty;
            txtFechaVencimientoReq.Text = string.Empty;
            txtJuzgadoReq.Text = string.Empty;
            txtNroOperadorReq.Text = string.Empty;
            txtNroOrdenReq.Text = string.Empty;
            txtObservacionesReq.Text = string.Empty;
            txtSecretariaReq.Text = string.Empty;
            txtFechaRegistroReq.Text = DateTime.Now.ToShortDateString();

            txtMotivo.Text = string.Empty;
        }

        private void ValidarBuscar()
        {
            if (txtApellidoyNombre.Text.Length > 0 && txtApellidoyNombre.Text.Length < 4)
                throw new Exception("Si ingresa el Apellido y Nombre, el mismo debe contener más de 3 caracteres.");

            Regex rx = new Regex(@"\b([0-9]{2}-[0-9]{8}-[0-9]{1})\b",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Find matches.
            MatchCollection matches = rx.Matches(txtCuit.Text);

            if (txtCuit.Text.Length > 0 && matches.Count == 0)
                throw new Exception("El Nº de CUIT no tiene un formato válido. Ej: 20-25006281-9.");

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarBuscar();
                Buscar();
                updResultados.Update();

                EjecutarScript(UpdatePanel1, "showResultado();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "showfrmError();");
            }
        }



        private void Buscar()
        {
            int BusNroDoc;
            int.TryParse(txtNroDocumento.Text.Trim(), out BusNroDoc);

            int BusTipoDoc;
            int.TryParse(ddlTipoDocumento.SelectedItem.Value, out BusTipoDoc);
            //from u in usergroups
            //join p in UsergroupPrices on u equals p.UsergroupID into gj
            //from x in gj.DefaultIfEmpty()
            db = new DGHP_Entities();
            var q = (from personasInhibidas in db.PersonasInhibidas
                     join tdoc in db.TipoDocumentoPersonal on personasInhibidas.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId into tdoc_join
                     from tdoc in tdoc_join.DefaultIfEmpty()
                     select new clsItemGrillaPersonasInhibidas
                     {
                         id_personainhibida = personasInhibidas.id_personainhibida,
                         id_tipodoc_personal = personasInhibidas.id_tipodoc_personal,
                         documento = tdoc.Nombre + " " + SqlFunctions.StringConvert((double)personasInhibidas.nrodoc_personainhibida),
                         nrodoc_personainhibida = personasInhibidas.nrodoc_personainhibida,
                         nroorden_personainhibida = personasInhibidas.nroorden_personainhibida,
                         cuit_personainhibida = personasInhibidas.cuit_personainhibida,
                         nomape_personainhibida = personasInhibidas.nomape_personainhibida,
                         fecharegistro_personainhibida = personasInhibidas.fecharegistro_personainhibida,
                         fechavencimiento_personainhibida = personasInhibidas.fechavencimiento_personainhibida,
                         autos_personainhibida = personasInhibidas.autos_personainhibida,
                         juzgado_personainhibida = personasInhibidas.juzgado_personainhibida,
                         secretaria_personainhibida = personasInhibidas.secretaria_personainhibida,
                         estado = personasInhibidas.estado_personainhibida,
                         fechabaja_personainhibida = personasInhibidas.fechabaja_personainhibida,
                         operador_personainhibida = personasInhibidas.operador_personainhibida,
                         observaciones_personainhibida = personasInhibidas.observaciones_personainhibida

                     }
                     );
            if (!string.IsNullOrWhiteSpace(txtApellidoyNombre.Text))
                q = q.Where(x => x.nomape_personainhibida.Contains(txtApellidoyNombre.Text.Trim()));
            if (!string.IsNullOrWhiteSpace(ddlTipoDocumento.SelectedValue) && ddlTipoDocumento.SelectedItem.Text.Length > 1)
                q = q.Where(x => x.id_tipodoc_personal == BusTipoDoc);
            if (!string.IsNullOrWhiteSpace(txtNroDocumento.Text))
                q = q.Where(x => x.nrodoc_personainhibida == BusNroDoc);
            if (!string.IsNullOrWhiteSpace(txtCuit.Text))
                q = q.Where(x => x.cuit_personainhibida.Contains(txtCuit.Text.Trim()));

            grdResultados.DataSource = q.OrderBy(x => x.nomape_personainhibida).ToList();
            grdResultados.DataBind();

            pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
            lblCantidadRegistros.Text = grdResultados.Rows.Count.ToString();
            db.Dispose();
        }

        private void CargarDatos(int id_datos)
        {

            db = new DGHP_Entities();
            var dato = (from personaInhibida in db.PersonasInhibidas
                        select new
                        {
                            id_personainhibida = personaInhibida.id_personainhibida,
                            NroDocumento = personaInhibida.nrodoc_personainhibida,
                            TipoDocumento = personaInhibida.TipoDocumentoPersonal,
                            Cuit = personaInhibida.cuit_personainhibida,
                            NroOrden = personaInhibida.nroorden_personainhibida,
                            NombreYApellido = personaInhibida.nomape_personainhibida,
                            FechaRegistro = personaInhibida.fecharegistro_personainhibida,
                            FechaVencimiento = personaInhibida.fechavencimiento_personainhibida,
                            autos = personaInhibida.autos_personainhibida,
                            juzgado = personaInhibida.juzgado_personainhibida,
                            secretaria = personaInhibida.secretaria_personainhibida,
                            estado = personaInhibida.estado_personainhibida,
                            nroOperador = personaInhibida.operador_personainhibida,
                            observaciones = personaInhibida.observaciones_personainhibida,
                            MotivoLevantamiento = personaInhibida.MotivoLevantamiento,
                            TipoPersona = personaInhibida.TipoPersona
                        }
                     ).FirstOrDefault(x => x.id_personainhibida == id_datos);


            if (dato != null)
            {
                hid_id_personainhibidaReq.Value = id_datos.ToString();
                if (dato.TipoDocumento != null)
                    txtTipoDocumentoReq.SelectedValue = Convert.ToString(dato.TipoDocumento.TipoDocumentoPersonalId);

                if (dato.TipoPersona != null)
                    txtTipoPersona.SelectedValue = Convert.ToString(dato.TipoPersona.Id_TipoPersona);
                else
                    txtTipoPersona.ClearSelection();//.SelectedValue = "0";

                txtNroDocumentoReq.Text = Convert.ToString(dato.NroDocumento);
                txtCuitReq.Text = Convert.ToString(dato.Cuit);
                txtNroOrdenReq.Text = Convert.ToString(dato.NroOrden);
                txtNombreyApellidoReq.Text = dato.NombreYApellido;
                txtFechaRegistroReq.Text = Convert.ToString(dato.FechaRegistro);
                txtFechaVencimientoReq.Text = Convert.ToString(dato.FechaVencimiento);
                txtAutosReq.Text = dato.autos;
                txtJuzgadoReq.Text = dato.juzgado;
                txtSecretariaReq.Text = dato.secretaria;
                txtEstadoReq.Text = Convert.ToString(dato.estado);
                txtNroOperadorReq.Text = Convert.ToString(dato.nroOperador);
                txtObservacionesReq.Text = dato.observaciones;
                txtMotivo.Text = dato.MotivoLevantamiento;
                
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
                this.EjecutarScript(updResultados, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");

            }

        }

        protected void lnkEliminarCondicionReq_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;

                LinkButton lnkEditar = (LinkButton)sender;
                int idCondicion = int.Parse(lnkEditar.CommandArgument);

                db = new DGHP_Entities();

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        db.Rubros_EliminarRubrosCondiciones(idCondicion, userid);
                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                Buscar();
                updResultados.Update();
                this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");
            }
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            db = new DGHP_Entities();
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
                string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
                string userName = Membership.GetUser().ProviderName;

                int idPersonaInhibida = Convert.ToInt32(hid_id_personainhibidaReq.Value);
                int? idTipoDocPersonaInhibida = TryParse2(txtTipoDocumentoReq.Text.Trim());

                int? nroDocumentoPersonaInhibida = TryParse2(txtNroDocumentoReq.Text.Trim());
                int? nroOrdenPersonaInhibida = Convert.ToInt32(txtNroOrdenReq.Text.Trim());
                string cuitPersonaInhibida = txtCuitReq.Text.Trim();
                string nomapePersonaInhibida = txtNombreyApellidoReq.Text.Trim();
                DateTime? fechaRegistroPersonaInhibida = null;
                if (txtFechaRegistroReq.Text != "")
                    fechaRegistroPersonaInhibida = Convert.ToDateTime(txtFechaRegistroReq.Text);

                DateTime? fechaVencimientoPersonaInhibida = null;
                if (txtFechaVencimientoReq.Text != "")
                    fechaVencimientoPersonaInhibida = Convert.ToDateTime(txtFechaVencimientoReq.Text);

                string autos = txtAutosReq.Text.Trim();
                string juzgado = txtJuzgadoReq.Text.Trim();
                string secretaria = txtSecretariaReq.Text.Trim();
                int? estado = null;
                if (txtEstadoReq.Text != "")
                    estado = Convert.ToInt32(txtEstadoReq.SelectedValue.Trim());

                DateTime? fechabajaPersonaInhibida = null;
                if (txtFechaBajaReq.Text != "")
                    fechabajaPersonaInhibida = Convert.ToDateTime(txtFechaBajaReq.Text);

                int? operadorPersonaInhibida = null;
                if (txtNroOperadorReq.Text != "")
                    operadorPersonaInhibida = Convert.ToInt32(txtNroOperadorReq.Text.Trim());
                string observacionesPersonaInhibida = txtObservacionesReq.Text.Trim();

                string motivo = txtMotivo.Text.Trim();


                int? idTipoPersona = Convert.ToInt32(txtTipoPersona.Text.Trim());

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        if (idPersonaInhibida == 0)
                        { 
                            db.PersonasInhibidas_insert(idTipoDocPersonaInhibida, nroDocumentoPersonaInhibida, nroOrdenPersonaInhibida, cuitPersonaInhibida, nomapePersonaInhibida, fechaRegistroPersonaInhibida, fechaVencimientoPersonaInhibida, autos, juzgado, secretaria, estado, fechabajaPersonaInhibida, operadorPersonaInhibida, observacionesPersonaInhibida, userName, motivo, idTipoPersona);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, string.Empty, url, txtObservacionesSolicitante.Text, "I");
                        }
                        else
                        {
                            db.PersonasInhibidas_update(idPersonaInhibida, idTipoDocPersonaInhibida, nroDocumentoPersonaInhibida, nroOrdenPersonaInhibida, cuitPersonaInhibida, nomapePersonaInhibida, fechaRegistroPersonaInhibida, fechaVencimientoPersonaInhibida, autos, juzgado, secretaria, estado, fechabajaPersonaInhibida, operadorPersonaInhibida, observacionesPersonaInhibida, userName, motivo, idTipoPersona);
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

                Buscar();
                updResultados.Update();

                this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
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

        int? TryParse2(string s)
        {
            int i;
            if (!int.TryParse(s, out i))
            {
                return null;
            }
            else
            {
                return i;
            }
        }

        private void CargarTiposDeDocumento()
        {
            ddlTipoDocumento.DataSource = db.Solicitud_TraerTiposDocumentosPersonales();
            ddlTipoDocumento.DataTextField = "nombre";
            ddlTipoDocumento.DataValueField = "id";
            ddlTipoDocumento.DataBind();

            ddlTipoDocumento.Items.Insert(0, "");


            txtTipoDocumentoReq.DataSource = db.Solicitud_TraerTiposDocumentosPersonales();
            txtTipoDocumentoReq.DataTextField = "nombre";
            txtTipoDocumentoReq.DataValueField = "id";
            txtTipoDocumentoReq.DataBind();

            txtTipoDocumentoReq.Items.Insert(0, "");

            var TipoPersona = db.TipoPersona.ToList();
            txtTipoPersona.DataSource = TipoPersona;
            txtTipoPersona.DataTextField = "Nombre";
            txtTipoPersona.DataValueField = "Id_TipoPersona";
            txtTipoPersona.DataBind();

            txtTipoDocumentoReq.Items.Insert(0, "");
            txtTipoPersona.Items.Insert(0, "");
        }

        #region paginado grilla

        private int codZonaPlaneamiento = 0;
        private string nombreZonaPlaneamiento = "";
        private int codZonaHabilitacion = 0;


        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdResultados.PageIndex = e.NewPageIndex;
                IniciarEntity();
                Buscar();
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
                grdResultados.PageIndex = int.Parse(cmdPage.Text) - 1;
                IniciarEntity();
                Buscar();
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
                grdResultados.PageIndex = grdResultados.PageIndex - 1;
                IniciarEntity();
                Buscar();
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
                grdResultados.PageIndex = grdResultados.PageIndex + 1;
                IniciarEntity();
                Buscar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void grd_DataBound(object sender, EventArgs e)
        {
            try
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
            string filtro = this.codZonaPlaneamiento + "|" + this.nombreZonaPlaneamiento + "|" + this.codZonaHabilitacion;
            ViewState["filtro"] = filtro;

        }

        private void recuperarFiltro()
        {
            if (ViewState["filtro"] == null)
                return;

            string filtro = ViewState["filtro"].ToString();

            string[] valores = filtro.Split('|');

            this.codZonaPlaneamiento = Convert.ToInt32(valores[0]);
            this.codZonaHabilitacion = Convert.ToInt32(valores[2]);

            if (string.IsNullOrEmpty(valores[1]))
            {
                this.nombreZonaPlaneamiento = null;
            }
            else
            {
                this.nombreZonaPlaneamiento = valores[1];
            }
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }
        #endregion

        #region entity

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

        protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.DataItem != null)
            {
                HyperLink lnkPDF = (HyperLink)e.Row.FindControl("lnkPDF");

                clsItemGrillaPersonasInhibidas rowItem = (clsItemGrillaPersonasInhibidas)e.Row.DataItem;

                string b64 = Functions.ConvertToBase64String(string.Format("{0}", rowItem.id_personainhibida));
                lnkPDF.NavigateUrl = string.Format("~/Reportes/ImprimirPersonasInhibidas.aspx?id={0}", b64);
            }
        }

        protected void txtTipoPersona_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            string opcion = ((int)TipoPersona.Juridica).ToString();
            if (ddl.SelectedItem.Value == opcion)
            {
                txtTipoDocumentoReq.Enabled = false;
                txtNroDocumentoReq.Enabled = false;
                txtTipoDocumentoReq.SelectedValue = string.Empty;
                txtNroDocumentoReq.Text = string.Empty;
            }
            else
            {
                txtTipoDocumentoReq.Enabled = true;
                txtNroDocumentoReq.Enabled = true;
            }
        }
    }
}