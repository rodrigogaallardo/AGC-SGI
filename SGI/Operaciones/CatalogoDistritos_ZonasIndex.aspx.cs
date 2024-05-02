using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using SGI.Model;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Operaciones
{
    public partial class CatalogoDistritos_ZonasIndex : System.Web.UI.Page
    {
        public static string IdDistritoRequest;

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
                string IdGrupoDistritoRequest = (Request.QueryString["IdGrupoDistrito"] == null) ? "" : Request.QueryString["IdGrupoDistrito"].ToString();
                IdDistritoRequest = (Request.QueryString["IdDistrito"] == null) ? "" : Request.QueryString["IdDistrito"].ToString();

                ddlGrupoDistricto.DataSource = GetUbicaciones_GruposDistritosList();
                ddlGrupoDistricto.DataTextField = "Nombre";
                ddlGrupoDistricto.DataValueField = "IdGrupoDistrito";
                ddlGrupoDistricto.DataBind();
                if (IdGrupoDistritoRequest != "")
                    ddlGrupoDistricto.SelectedValue = IdGrupoDistritoRequest;
                else
                    ddlGrupoDistricto.SelectedIndex = 0;

                hdIdGrupoDistrito.Value = ddlGrupoDistricto.SelectedValue;
                ddlGrupoDistricto_SelectedIndexChanged(null, null);
                btnBuscar_Click(null, null);
            }
        }





        protected void btnRemove_Click(object sender, EventArgs e)
        {
            id_object = ((Button)sender).ToolTip;
            string script = "$('#frmEliminarLog').modal('show');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int IdZona = int.Parse(((Button)sender).ToolTip);
            Response.Redirect("~/Operaciones/CatalogoDistritos_ZonasForm.aspx?IdGrupoDistrito=" + hdIdGrupoDistrito.Value + "&IdDistrito=" + hdIdDistrito.Value + "&IdZona=" + IdZona);

        }


        protected void btnNuevo_Click(object sender, EventArgs e)
        {


            Response.Redirect("~/Operaciones/CatalogoDistritos_ZonasForm.aspx?IdGrupoDistrito=" + hdIdGrupoDistrito.Value + "&IdDistrito=" + hdIdDistrito.Value + "&IdZona=0");
        }


        public List<Ubicaciones_GruposDistritos> GetUbicaciones_GruposDistritosList()
        {

            DGHP_Entities entities = new DGHP_Entities();

            List<Ubicaciones_GruposDistritos> Ubicaciones_GruposDistritosList = (from tareas in entities.Ubicaciones_GruposDistritos
                                                                                 select tareas).ToList();

            return Ubicaciones_GruposDistritosList;

        }



        protected void ddlGrupoDistricto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlCatalogoDistritos.DataSource = null;
            //ddlCatalogoDistritos.DataBind();
            gridView.DataSource = null;
            gridView.DataBind();


            DGHP_Entities entities = new DGHP_Entities();
            int IdGrupoDistrito = int.Parse(ddlGrupoDistricto.SelectedValue);
            List<Ubicaciones_CatalogoDistritos> Ubicaciones_CatalogoDistritosList = (from t in entities.Ubicaciones_CatalogoDistritos
                                                                                     where t.IdGrupoDistrito == IdGrupoDistrito
                                                                                     select t).ToList();
            ddlCatalogoDistritos.DataSource = Ubicaciones_CatalogoDistritosList;
            ddlCatalogoDistritos.DataTextField = "Codigo";
            ddlCatalogoDistritos.DataValueField = "IdDistrito";
            ddlCatalogoDistritos.DataBind();


            if (!string.IsNullOrEmpty( IdDistritoRequest) )
            { 
                ddlCatalogoDistritos.SelectedValue = IdDistritoRequest;
                IdDistritoRequest = string.Empty;
            }
            else
                ddlCatalogoDistritos.SelectedIndex = 0;

            hdIdGrupoDistrito.Value = ddlGrupoDistricto.SelectedValue;

            ddlCatalogoDistritos_SelectedIndexChanged(null, null);
        }

        protected void ddlCatalogoDistritos_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridView.DataSource = null;
            gridView.DataBind();
            hdIdDistrito.Value = ddlCatalogoDistritos.SelectedValue;
            btnBuscar_Click(null, null);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gridView.DataSource = null;
            gridView.DataBind();

            DGHP_Entities entities = new DGHP_Entities();
            int IdDistrito = int.Parse(hdIdDistrito.Value);
            List<Ubicaciones_CatalogoDistritos_Zonas> ubicaciones_CatalogoDistritos_ZonasList = (from t in entities.Ubicaciones_CatalogoDistritos_Zonas
                                                                                                 where t.IdDistrito == IdDistrito
                                                                                                 select t).ToList();
            gridView.DataSource = ubicaciones_CatalogoDistritos_ZonasList;
            gridView.DataBind();
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/DistritosIndex.aspx");
        }

        private void Eliminar()
        {
            int IdZona = int.Parse(id_object);
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
                            Ubicaciones_CatalogoDistritos_Zonas obj = ctx.Ubicaciones_CatalogoDistritos_Zonas.FirstOrDefault(x => x.IdZona == IdZona);
                            if (obj != null)
                            {
                                foreach (Ubicaciones_CatalogoDistritos_Subzonas ubicaciones_CatalogoDistritos_Subzonas in ctx.Ubicaciones_CatalogoDistritos_Subzonas)
                                    if (ubicaciones_CatalogoDistritos_Subzonas.IdZona == obj.IdZona)
                                        ctx.Ubicaciones_CatalogoDistritos_Subzonas.Remove(ubicaciones_CatalogoDistritos_Subzonas);
                                ctx.Ubicaciones_CatalogoDistritos_Zonas.Remove(obj);
                            }
                            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "D", 4019);
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
            ddlGrupoDistricto.SelectedIndex = 0;
            ddlGrupoDistricto_SelectedIndexChanged(null, null);
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