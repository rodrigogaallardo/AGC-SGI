using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using SGI.Model;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Operaciones
{
    public partial class CatalogoDistritos_ZonasIndex : System.Web.UI.Page
    {
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
               
                ddlGrupoDistricto_SelectedIndexChanged(null, null);
                btnBuscar_Click(null, null);
            }
        }





        protected void btnRemove_Click(object sender, EventArgs e)
        {
            int IdZona = int.Parse(((Button)sender).ToolTip);
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                Ubicaciones_CatalogoDistritos_Zonas ubicaciones_CatalogoDistritos_Zonas = entities.Ubicaciones_CatalogoDistritos_Zonas.Where(tarea => tarea.IdZona == IdZona).FirstOrDefault();

                if (ubicaciones_CatalogoDistritos_Zonas != null)
                {
                    try
                    {

                        #region Ubicaciones_CatalogoDistritos_Subzonas
                        foreach (Ubicaciones_CatalogoDistritos_Subzonas ubicaciones_CatalogoDistritos_Subzonas in entities.Ubicaciones_CatalogoDistritos_Subzonas)
                        {
                            if (ubicaciones_CatalogoDistritos_Subzonas.IdZona == ubicaciones_CatalogoDistritos_Zonas.IdZona)
                            {
                                entities.Ubicaciones_CatalogoDistritos_Subzonas.Remove(ubicaciones_CatalogoDistritos_Subzonas);
                            }
                        }
                        #endregion


                        entities.Ubicaciones_CatalogoDistritos_Zonas.Remove(ubicaciones_CatalogoDistritos_Zonas);
                        entities.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //ASOSA MENSAJE DE ERROR
                        ScriptManager sm = ScriptManager.GetCurrent(this);
                        string cadena = "No pudo borrarse el Registro por restricciones con otras tablas";
                        string script = string.Format("alert('{0}');", cadena);
                        ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);


                    }
                }

                //  gridView.EditIndex = -1;
                ddlGrupoDistricto.SelectedIndex = 0;
                ddlGrupoDistricto_SelectedIndexChanged(null, null);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int IdZona = int.Parse(((Button)sender).ToolTip);
            Response.Redirect("~/Operaciones/CatalogoDistritos_ZonasForm.aspx?IdZona=" + IdZona);

        }


        protected void btnNuevo_Click(object sender, EventArgs e)
        {


            Response.Redirect("~/Operaciones/CatalogoDistritos_ZonasForm.aspx?IdZona=0");
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
            ddlCatalogoDistritos.DataSource = null;
            ddlCatalogoDistritos.DataBind();
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
            string IdDistritoRequest = (Request.QueryString["IdDistrito"] == null) ? "" : Request.QueryString["IdDistrito"].ToString();

            if (!string.IsNullOrEmpty( IdDistritoRequest) )
                ddlCatalogoDistritos.SelectedValue = IdDistritoRequest;
            else
                ddlCatalogoDistritos.SelectedIndex = 0;
            ddlCatalogoDistritos_SelectedIndexChanged(null, null);
        }

        protected void ddlCatalogoDistritos_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridView.DataSource = null;
            gridView.DataBind();
            hdIdDistrito.Value = ddlCatalogoDistritos.SelectedValue;
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


    }
}