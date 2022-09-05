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
    public partial class AbmZonasHabilitacion : BasePage
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

        protected void btnNuevaZona_Click(object sender, EventArgs e)
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
            txtCodigoZona.Text = "";
            txtNombreZona.Text = "";
           
        }

        private void LimpiarDatos()
        {
            hid_id_ZonaReq.Value = "0";
            txtCodigoZonaReq.Text = "";
            txtCodigoZonaReq.Enabled = true;
            txtNombreZonaReq.Text = "";
            txtNombreZonaReq.Enabled = true;
           
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarZonas();
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

        private void BuscarZonas()
        {
            db = new DGHP_Entities();
            var q = (from zonas in db.Zonas_Habilitaciones
                     select new
                     {
                         id_zona = zonas.id_zonahabilitaciones,
                         codigo_zona = zonas.CodZonaHab,
                         nombre_zona = zonas.DescripcionZonaHab
                                   
                        
                     }
                     );
            if (txtNombreZona.Text.Trim().Length > 0)
                q = q.Where(x => x.nombre_zona.Contains(txtNombreZona.Text.Trim()));
            if (txtCodigoZona.Text.Trim().Length > 0)
                q = q.Where(x => x.codigo_zona.Contains(txtCodigoZona.Text.Trim()));
           


            grdResultados.DataSource = q.OrderBy(x => x.codigo_zona).ToList();
            grdResultados.DataBind();

            pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
            lblCantidadRegistros.Text = grdResultados.Rows.Count.ToString();
            db.Dispose();
        }

        private void CargarDatos(int id_datos)
        {

            db = new DGHP_Entities();

            var dato = db.Zonas_Habilitaciones.FirstOrDefault(x => x.id_zonahabilitaciones == id_datos);
            if (dato != null)
            {
                hid_id_ZonaReq.Value = id_datos.ToString();
                txtCodigoZonaReq.Text = dato.CodZonaHab;
                txtNombreZonaReq.Text = dato.DescripcionZonaHab;
                
            }
            db.Dispose();
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEditar = (LinkButton)sender;
                int id_datos = int.Parse(btnEditar.CommandArgument);

                txtCodigoZonaReq.Enabled = false;
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

        protected void lnkEliminarZonaReq_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;

                LinkButton lnkEditar = (LinkButton)sender;
                int idZona = int.Parse(lnkEditar.CommandArgument);

                db = new DGHP_Entities();

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        db.Rubros_EliminarRubrosZonasHabilitaciones(idZona, userid);
                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                BuscarZonas();
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

                int idZona = Convert.ToInt32(hid_id_ZonaReq.Value);
                string codigo = txtCodigoZonaReq.Text.Trim();
                string nombre = txtNombreZonaReq.Text.Trim();
               
                

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        if (idZona == 0)
                            db.Rubros_GuardarRubrosZonasHabilitaciones(codigo,nombre,userid);
                        else
                            db.Rubros_ActualizarRubrosZonasHabilitaciones(idZona,codigo, nombre,userid);
                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                BuscarZonas();
                updResultados.Update();
                txtCodigoZonaReq.Enabled = true;
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
    }
}