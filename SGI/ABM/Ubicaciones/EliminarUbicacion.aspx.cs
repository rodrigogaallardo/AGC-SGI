using Newtonsoft.Json;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.ABM.Ubicaciones
{
    public partial class EliminarUbicacion : BasePage
    {
        #region Var
        int idUbicacion = 0;

        private string id_object
        {
            get { return ViewState["_id_object"] != null ? ViewState["_id_object"].ToString() : string.Empty; }
            set { ViewState["_id_object"] = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.idUbicacion = Convert.ToInt32(Request.QueryString["Id"].ToString());
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                LoadComentariosByUbicacion(this.idUbicacion);

                this.EjecutarScript(updDatos, "finalizarCarga();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updDatos, "finalizarCarga();showfrmError();");
            }
        }

        private void LoadComentariosByUbicacion(int idUbicacion)
        {
            using (var db = new DGHP_Entities())
            {
                var u = (from ubi in db.Ubicaciones
                         where ubi.id_ubicacion == idUbicacion
                         select ubi).FirstOrDefault();

                txtObservaciones.Text = u.Observaciones == null ? string.Empty : u.Observaciones.ToString();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ABM/Ubicaciones/AbmUbicaciones.aspx");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            id_object = Request.QueryString["Id"].ToString();
            string script = "$('#frmEliminarLog').modal('show');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
        }

        private void Eliminar()
        {
            int idUbicacion = int.Parse(id_object);
            try
            {
                using (var ctx = new DGHP_Entities())
                {
                    using (var tran = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
                            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
                            var entity = ctx.Ubicaciones.Where(x => x.id_ubicacion == idUbicacion).Select(x => x).FirstOrDefault();
                            entity.baja_logica = true;
                            entity.Observaciones = txtObservaciones.Text;
                            entity.UpdateDate = DateTime.Now;
                            entity.UpdateUser = Functions.GetUserId();
                            ctx.Ubicaciones.Attach(entity);
                            ctx.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                            var Ph = ctx.Ubicaciones_PropiedadHorizontal.Where(x => x.id_ubicacion == entity.id_ubicacion).Select(x => x.id_propiedadhorizontal).ToList();
                            foreach (var id_partidahorizontal in Ph)
                                ctx.Ubicaciones_PropiedadHorizontal_delete(id_partidahorizontal);
                            Model.Ubicaciones obj = ctx.Ubicaciones.FirstOrDefault(x => x.id_ubicacion == idUbicacion);
                            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "D", 1025);
                            ctx.SaveChanges();
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Dispose();
                            LogError.Write(ex, "Error en transaccion.");
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                throw ex;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
            Response.Redirect("~/ABM/Ubicaciones/AbmUbicaciones.aspx");
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Eliminar();
        }
        protected void btnCancelarObservacion_Click(object sender, EventArgs e)
        {
            this.txtObservacionesSolicitante.Text = string.Empty;
            this.Eliminar();
        }
    }
}