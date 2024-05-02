using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using SGI.Model;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Operaciones
{
    public partial class GruposDistritosIndex : System.Web.UI.Page
    {
        private string id_object
        {
            get { return ViewState["_id_object"] != null ? ViewState["_id_object"].ToString() : string.Empty; }
            set { ViewState["_id_object"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion
            if (!IsPostBack)
            {

                CargarGruposDistritos();
            }
        }

        public void CargarGruposDistritos()
        {

            DGHP_Entities entities = new DGHP_Entities();

            List<Ubicaciones_GruposDistritos> Ubicaciones_GruposDistritosList = (from tareas in entities.Ubicaciones_GruposDistritos
                                                                                 select tareas).ToList();

            gridView.DataSource = Ubicaciones_GruposDistritosList;
            gridView.DataBind();

        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            id_object = ((Button)sender).ToolTip;
            string script = "$('#frmEliminarLog').modal('show');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int IdGrupoDistrito = int.Parse(((Button)sender).ToolTip);
            Response.Redirect("~/Operaciones/GruposDistritosForm.aspx?IdGrupoDistrito=" + IdGrupoDistrito);

        }


        protected void btnNuevo_Click(object sender, EventArgs e)
        {


            Response.Redirect("~/Operaciones/GruposDistritosForm.aspx?IdGrupoDistrito=0");
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/DistritosIndex.aspx");
        }

        private void Eliminar()
        {
            int IdGrupoDistrito = int.Parse(id_object);
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
                            Ubicaciones_GruposDistritos obj = ctx.Ubicaciones_GruposDistritos.FirstOrDefault(x => x.IdGrupoDistrito == IdGrupoDistrito);
                            if (obj != null)
                            {
                                foreach (Ubicaciones_CatalogoDistritos ubicaciones_CatalogoDistritos in ctx.Ubicaciones_CatalogoDistritos)
                                {
                                    if (ubicaciones_CatalogoDistritos.IdGrupoDistrito == IdGrupoDistrito)
                                    {
                                        foreach (Ubicaciones_CatalogoDistritos_Zonas ubicaciones_CatalogoDistritos_Zonas in ctx.Ubicaciones_CatalogoDistritos_Zonas)
                                        {
                                            if (ubicaciones_CatalogoDistritos_Zonas.IdDistrito == ubicaciones_CatalogoDistritos.IdDistrito)
                                            {
                                                foreach (Ubicaciones_CatalogoDistritos_Subzonas ubicaciones_CatalogoDistritos_Subzonas in ctx.Ubicaciones_CatalogoDistritos_Subzonas)
                                                    if (ubicaciones_CatalogoDistritos_Subzonas.IdZona == ubicaciones_CatalogoDistritos_Zonas.IdZona)
                                                        ctx.Ubicaciones_CatalogoDistritos_Subzonas.Remove(ubicaciones_CatalogoDistritos_Subzonas);
                                                ctx.Ubicaciones_CatalogoDistritos_Zonas.Remove(ubicaciones_CatalogoDistritos_Zonas);
                                            }
                                        }
                                        ctx.Ubicaciones_CatalogoDistritos.Remove(ubicaciones_CatalogoDistritos);
                                    }
                                }
                                ctx.Ubicaciones_GruposDistritos.Remove(obj);
                            }
                            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "D", 4024);
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
                ScriptManager sm = ScriptManager.GetCurrent(this);
                string cadena = "No pudo borrarse el Registro por restricciones con otras tablas";
                ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", string.Format("alert('{0}');", cadena), true);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
            gridView.EditIndex = -1;
            this.CargarGruposDistritos();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Eliminar();
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.txtObservacionesSolicitante.Text = string.Empty;
            this.Eliminar();
        }
    }
}