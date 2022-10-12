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
    public partial class GruposDistritosIndex : System.Web.UI.Page
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
            int IdGrupoDistrito = int.Parse(((Button)sender).ToolTip);
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                Ubicaciones_GruposDistritos ubicaciones_GruposDistritos = entities.Ubicaciones_GruposDistritos.Where(tarea => tarea.IdGrupoDistrito == IdGrupoDistrito).FirstOrDefault();

                if (ubicaciones_GruposDistritos != null)
                {
                    try
                    {

                        foreach (Ubicaciones_CatalogoDistritos ubicaciones_CatalogoDistritos in entities.Ubicaciones_CatalogoDistritos)
                        {
                            if (ubicaciones_CatalogoDistritos.IdGrupoDistrito == IdGrupoDistrito)
                            {
                                foreach (Ubicaciones_CatalogoDistritos_Zonas ubicaciones_CatalogoDistritos_Zonas in entities.Ubicaciones_CatalogoDistritos_Zonas)
                                {
                                    if (ubicaciones_CatalogoDistritos_Zonas.IdDistrito == ubicaciones_CatalogoDistritos.IdDistrito)
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
                                    }
                                }
                                entities.Ubicaciones_CatalogoDistritos.Remove(ubicaciones_CatalogoDistritos);
                            }
                        }

                        entities.Ubicaciones_GruposDistritos.Remove(ubicaciones_GruposDistritos);
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

                gridView.EditIndex = -1;
                this.CargarGruposDistritos();
            }
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
    }
}