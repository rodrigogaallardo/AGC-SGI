using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using System.Web.Security;
using System.Transactions;
using ExcelLibrary.BinaryFileFormat;
using Newtonsoft.Json;



namespace SGI.ABM
{
    public partial class AbmRubrosDepositos : BasePage
    {
        DGHP_Entities db = null;

        private string id_object
        {
            get { return ViewState["_id_object"] != null ? ViewState["_id_object"].ToString() : string.Empty; }
            set { ViewState["_id_object"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
            }
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        public void LoadData()
        {
            try
            {
                CargarComboCategoriaDeposito("");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CargarComboCategoriaDeposito(string categoria)
        {
            using (var db = new DGHP_Entities())
            {
                if (categoria == "")
                {
                    List<RubrosDepositosCategoriasCN> lista = db.RubrosDepositosCategoriasCN.Where(x => x.IdCategoriaDeposito > 0).ToList();
                    RubrosDepositosCategoriasCN rubro_inicial = new RubrosDepositosCategoriasCN();
                    rubro_inicial.IdCategoriaDeposito = 0;
                    rubro_inicial.Descripcion = (categoria == "") ? "Seleccione" : "Ninguna";
                    lista.Insert(0, rubro_inicial);
                    //Combo Inicial
                    txtCategoriaDeposito.DataTextField = "Descripcion";
                    txtCategoriaDeposito.DataValueField = "IdCategoriaDeposito";
                    txtCategoriaDeposito.DataSource = lista;
                    txtCategoriaDeposito.DataBind();

                    //Combo Nuevo Deposito
                    List<RubrosDepositosCategoriasCN> listaNuevoDeposito = db.RubrosDepositosCategoriasCN.Where(x => x.IdCategoriaDeposito > 0).ToList();
                    ddlCategoriaDeposito.DataTextField = "Descripcion";
                    ddlCategoriaDeposito.DataValueField = "IdCategoriaDeposito";
                    ddlCategoriaDeposito.DataSource = listaNuevoDeposito;
                    ddlCategoriaDeposito.DataBind();
                }
                else
                {
                    List<RubrosDepositosCategoriasCN> lista = db.RubrosDepositosCategoriasCN.Where(x => x.IdCategoriaDeposito > 0).ToList();
                    ddlCategoriaDeposito.DataTextField = "Descripcion";
                    ddlCategoriaDeposito.DataValueField = "IdCategoriaDeposito";
                    ddlCategoriaDeposito.DataSource = lista;
                    ddlCategoriaDeposito.DataBind();
                    ddlCategoriaDeposito.SelectedIndex = ddlCategoriaDeposito.Items.IndexOf(ddlCategoriaDeposito.Items.FindByText(categoria));
                }
            }
        }

        private void CargarComboCondicionIncendio(int id_condicion_incendio)
        {
            using (var db = new DGHP_Entities())
            {
                var lista = (from ci in db.CondicionesIncendio
                             where ci.idCondicionIncendio > 0
                             select new
                             {
                                 Id_CondicionIncendio = ci.idCondicionIncendio,
                                 Codigo = "Codigo:" + ci.codigo + " | Superficie:" + ci.superficie.ToString() + " | Subsuelo:" + ci.superficieSubsuelo.ToString()
                             }).ToList();
                ddlCondicionIncendio.DataTextField = "Codigo";
                ddlCondicionIncendio.DataValueField = "Id_CondicionIncendio";
                ddlCondicionIncendio.DataSource = lista;
                ddlCondicionIncendio.DataBind();

                if (id_condicion_incendio != 0)
                {
                    ddlCondicionIncendio.SelectedIndex = ddlCondicionIncendio.Items.IndexOf(ddlCondicionIncendio.Items.FindByValue(id_condicion_incendio.ToString()));
                }
            }
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarDepositos();
                updResultados.Update();
                EjecutarScript(UpdatePanelAcciones, "showResultado();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanelAcciones, "showfrmError();");
            }
        }

        private void BuscarDepositos()
        {
            db = new DGHP_Entities();
            var q = (from depositos in db.RubrosDepositosCN
                     join categoria in db.RubrosDepositosCategoriasCN on depositos.IdCategoriaDeposito equals categoria.IdCategoriaDeposito
                     join condicion in db.CondicionesIncendio on depositos.idCondicionIncendio equals condicion.idCondicionIncendio
                     select new
                     {
                         Id_Deposito = depositos.IdDeposito,
                         Codigo = depositos.Codigo,
                         Descripcion = depositos.Descripcion,
                         Categoria =  categoria.Descripcion,
                         Grado_Molestia = depositos.GradoMolestia,
                         Zona_Mixtura1 = depositos.ZonaMixtura1,
                         Zona_Mixtura2 = depositos.ZonaMixtura2,
                         Zona_Mixtura3 = depositos.ZonaMixtura3,
                         Zona_Mixtura4 = depositos.ZonaMixtura4,
                         Condicion_Incendio = condicion.codigo,
                         Superficie = condicion.superficie,
                         Superficie_Subsuelo = condicion.superficieSubsuelo,
                         Vigencia_Hasta = depositos.VigenciaHasta
                     });

            if (txtCodigoDescripcionDeposito.Text.Trim().Length > 0)
                q = q.Where(x => x.Codigo.Contains(txtCodigoDescripcionDeposito.Text.Trim()) || x.Descripcion.Contains(txtCodigoDescripcionDeposito.Text.Trim()));

            if (txtCategoriaDeposito.SelectedItem.Text.Trim().Length > 0 && txtCategoriaDeposito.SelectedItem.Text != "Seleccione")
                q = q.Where(x => x.Categoria.Contains(txtCategoriaDeposito.SelectedItem.Text.Trim()));

            if (txtDepositoVigente.Text == "Si")
            {
                q = q.Where(x => x.Vigencia_Hasta == null);
            }
            else
            {
                q = q.Where(x => x.Vigencia_Hasta < DateTime.Now);
            }

            grdResultados.DataSource = q.OrderBy(x => x.Codigo).ToList();
            grdResultados.DataBind();
            pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
            lblCantidadRegistros.Text = grdResultados.Rows.Count.ToString();
            db.Dispose();
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEditar = (LinkButton)sender;
                int id_deposito = int.Parse(btnEditar.CommandArgument);
                CargarDatos(id_deposito);
                ddlDepositoVigente.Enabled = true;
                updDatos.Update();
                EjecutarScript(updResultados, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");
            }
        }

        private void CargarDatos(int id_deposito)
        {
            db = new DGHP_Entities();
            var q = (from depositos in db.RubrosDepositosCN
                     join categoria in db.RubrosDepositosCategoriasCN on depositos.IdCategoriaDeposito equals categoria.IdCategoriaDeposito
                     join condicion in db.CondicionesIncendio on depositos.idCondicionIncendio equals condicion.idCondicionIncendio
                     where depositos.IdDeposito == id_deposito
                     select new
                     {
                         Id_Deposito = depositos.IdDeposito,
                         Codigo = depositos.Codigo,
                         Descripcion = depositos.Descripcion,
                         Categoria = categoria.Descripcion,
                         Grado_Molestia = depositos.GradoMolestia,
                         Zona_Mixtura1 = depositos.ZonaMixtura1,
                         Zona_Mixtura2 = depositos.ZonaMixtura2,
                         Zona_Mixtura3 = depositos.ZonaMixtura3,
                         Zona_Mixtura4 = depositos.ZonaMixtura4,
                         Id_Condicion_Incendio = depositos.idCondicionIncendio,
                         Condicion_Incendio = condicion.codigo,
                         Superficie = condicion.superficie,
                         Superficie_Subsuelo = condicion.superficieSubsuelo,
                         Vigencia_Hasta = depositos.VigenciaHasta
                     }
                     ).FirstOrDefault();

            if (q.Vigencia_Hasta == null || q.Vigencia_Hasta >= DateTime.Now)
                ddlDepositoVigente.SelectedIndex = ddlDepositoVigente.Items.IndexOf(ddlDepositoVigente.Items.FindByText("Si"));
            else
                ddlDepositoVigente.SelectedIndex = ddlDepositoVigente.Items.IndexOf(ddlDepositoVigente.Items.FindByText("No"));

            CargarComboCategoriaDeposito(q.Categoria);
            txtCodigoDeposito2.Text = q.Codigo;
            txtCodigoDeposito2.Enabled = false;

            txtDescripcion.Text = q.Descripcion;
            txtGradoMolestia.Text = q.Grado_Molestia;
            txtZonaMixtura1.Text = q.Zona_Mixtura1;
            txtZonaMixtura2.Text = q.Zona_Mixtura2;
            txtZonaMixtura3.Text = q.Zona_Mixtura3;
            txtZonaMixtura4.Text = q.Zona_Mixtura4;
            CargarComboCondicionIncendio(Convert.ToInt32(q.Id_Condicion_Incendio));
            hid_id_deposito.Value = q.Id_Deposito.ToString();
        }

        protected void btnNuevoDeposito_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarDatos();
                CargarComboCategoriaDeposito("");
                CargarComboCondicionIncendio(0);
                txtCodigoDeposito2.Enabled = true;
                ddlDepositoVigente.Enabled = false;
                updDatos.Update();
                EjecutarScript(UpdatePanelAcciones, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanelAcciones, "finalizarCarga();showfrmError();");
            }
        }

        private void LimpiarDatos()
        {
            hid_id_deposito.Value = "0";
            txtCodigoDeposito2.Text = "";
            ddlDepositoVigente.SelectedIndex = ddlDepositoVigente.Items.IndexOf(ddlDepositoVigente.Items.FindByText("Si"));
            txtDescripcion.Text = "";
            ddlCategoriaDeposito.ClearSelection();
            txtGradoMolestia.Text = "";
            txtZonaMixtura1.Text = "";
            txtZonaMixtura2.Text = "";
            txtZonaMixtura3.Text = "";
            txtZonaMixtura4.Text = "";
            ddlCondicionIncendio.ClearSelection();
        }

        protected void lnkEliminarDeposito_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
                string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
                LinkButton lnkEditar = (LinkButton)sender;
                int idDeposito = int.Parse(lnkEditar.CommandArgument);
                db = new DGHP_Entities();
                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        db.SGI_Deposito_EliminarVigencia(idDeposito);

                        Tran.Complete();
                        string script = "$('#frmEliminarLog').modal('show');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
                        id_object = idDeposito.ToString();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }
                BuscarDepositos();
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
            Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();

            try
            {

                int idDeposito = Convert.ToInt32(hid_id_deposito.Value);
                string codigo = txtCodigoDeposito2.Text.Trim();
                string descripcion = txtDescripcion.Text.Trim();
                int id_categoria = Convert.ToInt32(ddlCategoriaDeposito.SelectedValue);
                string grado_molestia = txtGradoMolestia.Text.Trim();
                string zona_mixtura1 = txtZonaMixtura1.Text.Trim();
                string zona_mixtura2 = txtZonaMixtura2.Text.Trim();
                string zona_mixtura3 = txtZonaMixtura3.Text.Trim();
                string zona_mixtura4 = txtZonaMixtura4.Text.Trim();
                int id_condicion_incendio = Convert.ToInt32(ddlCondicionIncendio.SelectedValue);
                string vigencia = ddlDepositoVigente.Text;

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        if (idDeposito == 0)
                        {
                            idDeposito = db.SGI_Deposito_Insertar(codigo, descripcion, id_categoria, grado_molestia, zona_mixtura1, zona_mixtura2, zona_mixtura3, zona_mixtura4, id_condicion_incendio);
                            RubrosDepositosCN obj = db.RubrosDepositosCN.FirstOrDefault(x => x.IdDeposito == idDeposito);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "I", 1015);
                        }
                        else
                        {
                            db.SGI_Deposito_ActualizarInformacion(idDeposito, descripcion, id_categoria, grado_molestia, zona_mixtura1, zona_mixtura1, zona_mixtura3, zona_mixtura4, id_condicion_incendio, vigencia);
                            RubrosDepositosCN obj = db.RubrosDepositosCN.FirstOrDefault(x => x.IdDeposito == idDeposito);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "U", 1015);
                        }
                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }
                BuscarDepositos();
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

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            int value = int.Parse(id_object);
            RubrosDepositosCN obj = db.RubrosDepositosCN.FirstOrDefault(x => x.IdDeposito == value);
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionEliminar.Text, "D", 1015);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            int value = int.Parse(id_object);
            RubrosDepositosCN obj = db.RubrosDepositosCN.FirstOrDefault(x => x.IdDeposito == value);
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, string.Empty, "D", 1015);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
        }

    }
}