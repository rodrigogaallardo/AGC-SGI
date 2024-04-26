using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SGI.Model;
using System.Transactions;
using Newtonsoft.Json;
using ExcelLibrary.BinaryFileFormat;
using DocumentFormat.OpenXml.Bibliography;

namespace SGI.ABM
{
    public partial class AbmSectoresSade : BasePage
    {
        DGHP_Entities db = null;

        private string id_object
        {
            get { return ViewState["_id_object"] != null ? ViewState["_id_object"].ToString() : string.Empty; }
            set { ViewState["_id_object"] = value; }
        }

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
            }
        }
        #endregion

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        protected void btnNuevoSector_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarDatos();
                updDatos.Update();
                EjecutarScript(UpdatePanel1, "showDatos();");
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
            txtCodigoSector.Text = "";
            txtNombreSector.Text = "";
            txtReparticionSector.Text = "";

        }

        private void LimpiarDatos()
        {
            hid_id_SectorReq.Value = "0";
            txtCodigoSectorReq.Text = "";
            txtCodigoSectorReq.Enabled = true;
            txtNombreSectorReq.Text = "";
            txtNombreSectorReq.Enabled = true;
            txtReparticionSectorReq.Text = "";
            txtReparticionSectorReq.Enabled = true;

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarSectores();
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

        private void BuscarSectores()
        {
            db = new DGHP_Entities();
            var q = (from Sectores in db.Sectores_SADE
                     select new
                     {
                         id_Sector = Sectores.id_sector,
                         codigo_Sector = Sectores.codigo_sector,
                         Nombre_Sector = Sectores.nombre_sector,
                         Reparticion_Sector = Sectores.reparticion
                     }
                     );
            if (txtNombreSector.Text.Trim().Length > 0)
                q = q.Where(x => x.Nombre_Sector.Contains(txtNombreSector.Text.Trim()));
            if (txtCodigoSector.Text.Trim().Length > 0)
                q = q.Where(x => x.codigo_Sector.Contains(txtCodigoSector.Text.Trim()));
            if (txtReparticionSector.Text.Trim().Length > 0)
                q = q.Where(x => x.Reparticion_Sector.Contains(txtReparticionSector.Text.Trim()));


            grdResultados.DataSource = q.OrderBy(x => x.codigo_Sector).ToList();
            grdResultados.DataBind();

            pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
            lblCantidadRegistros.Text = grdResultados.Rows.Count.ToString();
            db.Dispose();
        }

        private void CargarDatos(int id_datos)
        {

            db = new DGHP_Entities();

            var dato = db.Sectores_SADE.FirstOrDefault(x => x.id_sector == id_datos);
            if (dato != null)
            {
                hid_id_SectorReq.Value = id_datos.ToString();
                txtCodigoSectorReq.Text = dato.codigo_sector;
                txtNombreSectorReq.Text = dato.nombre_sector;
                txtReparticionSectorReq.Text = dato.reparticion;

            }
            db.Dispose();
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEditar = (LinkButton)sender;
                int id_datos = int.Parse(btnEditar.CommandArgument);

                txtCodigoSectorReq.Enabled = false;
                LimpiarDatos();
                CargarDatos(id_datos);
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

        protected void lnkEliminarSectorReq_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;

                LinkButton lnkEditar = (LinkButton)sender;
                int idSector = int.Parse(lnkEditar.CommandArgument);

                db = new DGHP_Entities();

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        db.SectoresSADE_EliminarSector(idSector, userid);

                        Tran.Complete();
                        string script = "$('#frmEliminarLog').modal('show');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
                        id_object = idSector.ToString();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                BuscarSectores();
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

                int idSector = Convert.ToInt32(hid_id_SectorReq.Value);
                string codigo = txtCodigoSectorReq.Text.Trim();
                string Nombre = txtNombreSectorReq.Text.Trim();
                string reparticion = txtReparticionSectorReq.Text.Trim();



                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        
                        if (idSector == 0)
                        {
                            idSector = db.SectoresSADE_GuardarSector(codigo, Nombre, reparticion, userid); 
                            Sectores_SADE obj = db.Sectores_SADE.FirstOrDefault(x => x.id_sector == idSector);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitantes.Text, "I", 1016);
                        }
                        else
                        {
                            db.SectoresSADE_ActualizarSector(idSector, codigo, Nombre, reparticion, userid);
                            Sectores_SADE obj = db.Sectores_SADE.FirstOrDefault(x => x.id_sector == idSector);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitantes.Text, "U", 1016);
                        }
                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                BuscarSectores();
                updResultados.Update();
                txtCodigoSectorReq.Enabled = true;
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
            Sectores_SADE obj = db.Sectores_SADE.FirstOrDefault(x => x.id_sector == value);
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "D", 1016);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            int value = int.Parse(id_object);
            Sectores_SADE obj = db.Sectores_SADE.FirstOrDefault(x => x.id_sector == value);
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, string.Empty, "D", 1016);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
        }
    }
}