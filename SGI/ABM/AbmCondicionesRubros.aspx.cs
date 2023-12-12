using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SGI.Model;
using System.Transactions;

namespace SGI.ABM
{
    public partial class AbmCondicionesRubros : BasePage
    {
        DGHP_Entities db = null;

        #region load de pagina
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "init_Js_updpnlCrearActu", "init_Js_updpnlCrearActu();", true);

            }


            if (!IsPostBack)
            {
                db = new DGHP_Entities();
                //CargarZonasHabil();
                //  MostrarTitulos();
            }
        }
        #endregion

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        protected void btnNuevaCondicion_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarDatos();
                updDatos.Update();
                this.EjecutarScript(updpnlBuscar, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "finalizarCarga();showfrmError();");

            }
        }

        private void LimpiarDatosBusqueda()
        {
            txtCodigoCondicion.Text = "";
            txtNombreCondicion.Text = "";
            txtSupMinima.Text = "";
            txtSupMaxima.Text = "";
        }

        private void LimpiarDatos()
        {
            hid_id_condReq.Value = "0";
            txtCodigoCondicionReq.Text = "";
            txtNombreCondicionReq.Text = "";
            txtSupMinimaCondicionReq.Text = "";
            txtSupMaximaCondicionReq.Text = "";
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarCondicionesRubros();
                updResultados.Update();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "showfrmError();");
            }
        }

        private void BuscarCondicionesRubros()
        {
            db = new DGHP_Entities();
            var q = (from condiciones in db.RubrosCondiciones
                     select new
                     {
                         id_cond = condiciones.id_condicion,
                         codigo_condicion = condiciones.cod_condicion,
                         nombre_condicion = condiciones.nom_condicion,
                         sup_minima = condiciones.SupMin_condicion,
                         sup_maxima = condiciones.SupMax_condicion              
                        
                     }
                     );
            if (txtNombreCondicion.Text.Trim().Length > 0)
                q = q.Where(x => x.nombre_condicion.Contains(txtNombreCondicion.Text.Trim()));
            if (txtCodigoCondicion.Text.Trim().Length > 0)
                q = q.Where(x => x.codigo_condicion.Contains(txtCodigoCondicion.Text.Trim()));
            if (txtSupMinima.Text.Trim().Length > 0)
                q = q.Where(x => x.sup_minima >= Convert.ToInt32(txtSupMinima.Text));
            if (txtSupMaxima.Text.Trim().Length > 0)
                q = q.Where(x => x.sup_maxima <= Convert.ToInt32(txtSupMaxima.Text));


            grdResultados.DataSource = q.OrderBy(x => x.codigo_condicion).ToList();
            grdResultados.DataBind();

            pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
            lblCantidadRegistros.Text = q.OrderBy(x => x.codigo_condicion).ToList().Count.ToString();
            db.Dispose();
        }

        private void CargarDatos(int id_datos)
        {

            db = new DGHP_Entities();

            var dato = db.RubrosCondiciones.FirstOrDefault(x => x.id_condicion == id_datos);
            if (dato != null)
            {
                hid_id_condReq.Value = id_datos.ToString();
                txtCodigoCondicionReq.Text = dato.cod_condicion;
                txtNombreCondicionReq.Text = dato.nom_condicion;
                txtSupMinimaCondicionReq.Text = Convert.ToString(dato.SupMin_condicion);
                txtSupMaximaCondicionReq.Text = Convert.ToString(dato.SupMax_condicion);
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
                        string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
                        Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, string.Empty, url, string.Empty, "D");

                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                BuscarCondicionesRubros();
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

                int idCondicion = Convert.ToInt32(hid_id_condReq.Value);
                string codigo = txtCodigoCondicionReq.Text.Trim();
                string nombre = txtNombreCondicionReq.Text.Trim();
                int supMinima = Convert.ToInt32(txtSupMinimaCondicionReq.Text.Trim());
                int supMaxima = Convert.ToInt32(txtSupMaximaCondicionReq.Text.Trim());
                

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        if (idCondicion == 0)
                        {
                            db.Rubros_GuardarRubrosCondiciones(codigo, nombre, supMinima, supMaxima, userid);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, string.Empty, url, txtObservacionesSolicitantes.Text, "I");
                        }
                        else
                        {
                            db.Rubros_ActualizarRubrosCondiciones(idCondicion, codigo, nombre, supMinima, supMaxima, userid);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, string.Empty, url, txtObservacionesSolicitantes.Text, "U");
                        }
                            

                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                BuscarCondicionesRubros();
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

        #region paginado grilla

        private int codCondicion = 0;
        private string nombreCondicion = "";
        private int supMinima = 0;
        private int supMaxima = 0;


        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdResultados.PageIndex = e.NewPageIndex;
                IniciarEntity();
                BuscarCondicionesRubros();
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
                BuscarCondicionesRubros();
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
                BuscarCondicionesRubros();
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
                BuscarCondicionesRubros();
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
            string filtro = this.codCondicion + "|" + this.nombreCondicion + "|" + this.supMinima + "|" + this.supMaxima;
            ViewState["filtro"] = filtro;

        }

        private void recuperarFiltro()
        {
            if (ViewState["filtro"] == null)
                return;

            string filtro = ViewState["filtro"].ToString();

            string[] valores = filtro.Split('|');

            this.codCondicion = Convert.ToInt32(valores[0]);
            this.supMinima = Convert.ToInt32(valores[2]);
            this.supMaxima = Convert.ToInt32(valores[3]);

            if (string.IsNullOrEmpty(valores[1]))
            {
                this.nombreCondicion = null;
            }
            else
            {
                this.nombreCondicion = valores[1];
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

    }
}