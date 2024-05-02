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
    public partial class CatalogoDistritos_SubzonasIndex : System.Web.UI.Page
    {
        public static string IdDistritoRequest;
        public static string IdZonaRequest;

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
                IdDistritoRequest = (Request.QueryString["IdDistrito"] == null) ? "" : Request.QueryString["IdDistrito"].ToString(); IdZonaRequest = (Request.QueryString["IdZona"] == null) ? "" : Request.QueryString["IdZona"].ToString();
                IdZonaRequest = (Request.QueryString["IdZona"] == null) ? "" : Request.QueryString["IdZona"].ToString();



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
                
            }
        }





        protected void btnRemove_Click(object sender, EventArgs e)
        {
            int IdSubZona = int.Parse(((Button)sender).ToolTip);
            id_object = IdSubZona.ToString();
            string script = "$('#frmEliminarLog').modal('show');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);


            

            

        }


        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int IdSubZona = int.Parse(((Button)sender).ToolTip);
            Response.Redirect("~/Operaciones/CatalogoDistritos_SubZonasForm.aspx?IdGrupoDistrito=" + hdIdGrupoDistrito.Value + "&IdDistrito=" + hdIdDistrito.Value + "&IdZona=" + hdIdZona.Value + "&IdSubZona=" + IdSubZona);

        }


        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/CatalogoDistritos_SubZonasForm.aspx?IdGrupoDistrito=" + hdIdGrupoDistrito.Value + "&IdDistrito=" + hdIdDistrito.Value + "&IdZona=" + hdIdZona.Value + "&IdSubZona=0" );
           
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


            if (IdDistritoRequest != "")
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
            //ddlCatalogoDistritos_Zonas.DataSource = null;
            //ddlCatalogoDistritos_Zonas.DataBind();
            gridView.DataSource = null;
            gridView.DataBind();


            DGHP_Entities entities = new DGHP_Entities();
            int IdDistrito = int.Parse(ddlCatalogoDistritos.SelectedValue);
            List<Ubicaciones_CatalogoDistritos_Zonas> ubicaciones_CatalogoDistritos_ZonasList = (from t in entities.Ubicaciones_CatalogoDistritos_Zonas
                                                                                                 where t.IdDistrito == IdDistrito
                                                                                                 select t).ToList();
            if (ubicaciones_CatalogoDistritos_ZonasList.Count() < 1)
            {
                ddlCatalogoDistritos_Zonas.DataSource = null;
               ddlCatalogoDistritos_Zonas.DataBind();
                ddlCatalogoDistritos_Zonas.Items.Clear();
                
                    btnNuevo.Enabled = false;
               
                return;
            }
            ddlCatalogoDistritos_Zonas.DataSource = ubicaciones_CatalogoDistritos_ZonasList;
            ddlCatalogoDistritos_Zonas.DataTextField = "CodigoZona";
            ddlCatalogoDistritos_Zonas.DataValueField = "IdZona";
            ddlCatalogoDistritos_Zonas.DataBind();
           
            if (!string.IsNullOrEmpty(IdZonaRequest))
            {
                ddlCatalogoDistritos_Zonas.SelectedValue = IdZonaRequest;
                IdZonaRequest = string.Empty;
            }
            else
                ddlCatalogoDistritos_Zonas.SelectedIndex = 0;




            hdIdDistrito.Value = ddlCatalogoDistritos.SelectedValue;

            ddlCatalogoDistritos_Zonas_SelectedIndexChanged(null, null);
        }
        protected void ddlCatalogoDistritos_Zonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridView.DataSource = null;
            gridView.DataBind();



            hdIdZona.Value = ddlCatalogoDistritos_Zonas.SelectedValue;
            btnBuscar_Click(null, null);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gridView.DataSource = null;
            gridView.DataBind();

            DGHP_Entities entities = new DGHP_Entities();
            int IdZona = int.Parse(hdIdZona.Value);
            List<Ubicaciones_CatalogoDistritos_Subzonas> ubicaciones_CatalogoDistritos_SubzonasList = (from t in entities.Ubicaciones_CatalogoDistritos_Subzonas
                                                                                                       where t.IdZona == IdZona
                                                                                                       select t).ToList();
            gridView.DataSource = ubicaciones_CatalogoDistritos_SubzonasList;
            gridView.DataBind();
            if (ddlCatalogoDistritos.SelectedIndex > -1 & ddlCatalogoDistritos_Zonas.SelectedIndex > -1)
                btnNuevo.Enabled = true;
            else
                btnNuevo.Enabled = false;

        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/DistritosIndex.aspx");
        }

        private void Eliminar()
        {
            int IdSubZona = int.Parse(id_object);
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
                            Ubicaciones_CatalogoDistritos_Subzonas obj = ctx.Ubicaciones_CatalogoDistritos_Subzonas.FirstOrDefault(x => x.IdSubZona == IdSubZona);
                            if (obj != null)
                                ctx.Ubicaciones_CatalogoDistritos_Subzonas.Remove(obj);
                            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "D", 4017);
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