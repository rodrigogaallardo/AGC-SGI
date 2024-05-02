using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelLibrary.BinaryFileFormat;
using ExternalService.Class;
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
    public partial class CatalogoDistritosIndex : System.Web.UI.Page
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
                string IdGrupoDistritoRequest = (Request.QueryString["IdGrupoDistrito"] == null) ? "" : Request.QueryString["IdGrupoDistrito"].ToString();

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
               // btnBuscar_Click(null, null);
            }
        }

        public void CargarCatalogoDistritos()
        {

            DGHP_Entities entities = new DGHP_Entities();
            int IdGrupoDistrito = int.Parse(ddlGrupoDistricto.SelectedValue);
            List<Ubicaciones_CatalogoDistritos> Ubicaciones_CatalogoDistritosList = (from t in entities.Ubicaciones_CatalogoDistritos
                                                                                     where  t.IdGrupoDistrito== IdGrupoDistrito
                                                                                     select t).ToList();
            gridView.DataSource = Ubicaciones_CatalogoDistritosList;
            gridView.DataBind();

        }
        public List<Ubicaciones_GruposDistritos> GetUbicaciones_GruposDistritosList()
        {

            DGHP_Entities entities = new DGHP_Entities();

            List<Ubicaciones_GruposDistritos> Ubicaciones_GruposDistritosList = (from tareas in entities.Ubicaciones_GruposDistritos
                                                                                 select tareas).ToList();

            return Ubicaciones_GruposDistritosList;

        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            id_object = ((Button)sender).ToolTip;
            string script = "$('#frmEliminarLog').modal('show');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int IdDistrito = int.Parse(((Button)sender).ToolTip);
            Response.Redirect("~/Operaciones/CatalogoDistritosForm.aspx?IdGrupoDistrito="+ hdIdGrupoDistrito.Value + "&IdDistrito=" + IdDistrito);

        }


        protected void btnNuevo_Click(object sender, EventArgs e)
        {


            Response.Redirect("~/Operaciones/CatalogoDistritosForm.aspx?IdGrupoDistrito="+ hdIdGrupoDistrito.Value + "&IdDistrito=0");
        }

        protected void ddlGrupoDistricto_SelectedIndexChanged(object sender, EventArgs e)
        {
            DGHP_Entities entities = new DGHP_Entities();
            int IdGrupoDistrito = int.Parse(ddlGrupoDistricto.SelectedValue);
            List<Ubicaciones_CatalogoDistritos> Ubicaciones_CatalogoDistritosList = (from t in entities.Ubicaciones_CatalogoDistritos
                                                                                     where t.IdGrupoDistrito == IdGrupoDistrito
                                                                                     select t).ToList();
            hdIdGrupoDistrito.Value = ddlGrupoDistricto.SelectedValue;

            gridView.DataSource = Ubicaciones_CatalogoDistritosList;
            gridView.DataBind();
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/DistritosIndex.aspx");
        }

        private void Eliminar()
        {
            int IdDistrito = int.Parse(id_object);
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
                            Ubicaciones_CatalogoDistritos obj = ctx.Ubicaciones_CatalogoDistritos.FirstOrDefault(x => x.IdDistrito == IdDistrito);
                            if (obj != null)
                            {
                                foreach (Ubicaciones_CatalogoDistritos_Zonas ubicaciones_CatalogoDistritos_Zonas in ctx.Ubicaciones_CatalogoDistritos_Zonas)
                                {
                                    if (ubicaciones_CatalogoDistritos_Zonas.IdDistrito == obj.IdDistrito)
                                    {
                                        foreach (Ubicaciones_CatalogoDistritos_Subzonas ubicaciones_CatalogoDistritos_Subzonas in ctx.Ubicaciones_CatalogoDistritos_Subzonas)
                                        {
                                            if (ubicaciones_CatalogoDistritos_Subzonas.IdZona == ubicaciones_CatalogoDistritos_Zonas.IdZona)
                                                ctx.Ubicaciones_CatalogoDistritos_Subzonas.Remove(ubicaciones_CatalogoDistritos_Subzonas);
                                        }
                                        ctx.Ubicaciones_CatalogoDistritos_Zonas.Remove(ubicaciones_CatalogoDistritos_Zonas);
                                    }
                                }
                                ctx.Ubicaciones_CatalogoDistritos.Remove(obj);
                            }
                            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "D", 4015);
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
            this.CargarCatalogoDistritos();
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