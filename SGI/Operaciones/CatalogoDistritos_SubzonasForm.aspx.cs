using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Ajax.Utilities;
using SGI.Model;
using SGI.Seguridad;
using Syncfusion.DocIO.DLS;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataVisualization.DiagramEnums;
using Syncfusion.Linq;
using Syncfusion.Pdf.Lists;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAlrunsRecord;

namespace SGI.Operaciones
{
    public partial class CatalogoDistritos_SubzonasForm : System.Web.UI.Page
    {

        public static string IdDistritoRequest;
        public static string IdZonaRequest;
        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion


            string IdSubZonaStr = (Request.QueryString["IdSubZona"] == null) ? "" : Request.QueryString["IdSubZona"].ToString();
            if (String.IsNullOrEmpty(IdSubZonaStr))
            {
                // IdSubZonaStr = "0";
                Response.Redirect("~/Operaciones/CatalogoDistritos_SubzonasIndex.aspx?IdGrupoDistrito=" + ddlGrupoDistricto.SelectedValue + "&IdDistrito=" + ddlCatalogoDistritos.SelectedValue + "&IdZona=" + ddlCatalogoDistritos_Zonas.SelectedValue);

            }
            int IdSubZona = int.Parse(IdSubZonaStr);
            hdIdSubZona.Value = IdSubZonaStr;

            Ubicaciones_CatalogoDistritos_Subzonas ubicaciones_CatalogoDistritos_Subzonas = new Ubicaciones_CatalogoDistritos_Subzonas();
            ubicaciones_CatalogoDistritos_Subzonas = BuscarUbicaciones_CatalogoDistritos_SubZonas(IdSubZona);
            if (!IsPostBack)
            {
                IdDistritoRequest = (Request.QueryString["IdDistrito"] == null) ? "" : Request.QueryString["IdDistrito"].ToString();
                IdZonaRequest = (Request.QueryString["IdZona"] == null) ? "" : Request.QueryString["IdZona"].ToString();

                ddlGrupoDistricto.DataSource = GetUbicaciones_GruposDistritosList();
                ddlGrupoDistricto.DataTextField = "Nombre";
                ddlGrupoDistricto.DataValueField = "IdGrupoDistrito";
                ddlGrupoDistricto.DataBind();
                ddlGrupoDistricto.SelectedIndex = 0;
                ddlGrupoDistricto_SelectedIndexChanged(null, null);

                if (IdSubZona > 0)
                {
                    txtCodigoSubZona.Text = ubicaciones_CatalogoDistritos_Subzonas.CodigoSubZona;

                    Ubicaciones_CatalogoDistritos_Zonas ubicaciones_CatalogoDistritos_Zonas;
                    Ubicaciones_CatalogoDistritos ubicaciones_CatalogoDistritos;
                    using (DGHP_Entities entities = new DGHP_Entities())
                    {

                        ubicaciones_CatalogoDistritos_Zonas = (from x in entities.Ubicaciones_CatalogoDistritos_Zonas
                                                               where x.IdZona == ubicaciones_CatalogoDistritos_Subzonas.IdZona
                                                               select x).FirstOrDefault();


                        ubicaciones_CatalogoDistritos = (from x in entities.Ubicaciones_CatalogoDistritos
                                                         where x.IdDistrito == ubicaciones_CatalogoDistritos_Zonas.IdDistrito
                                                         select x).FirstOrDefault();
                    }

                    ddlGrupoDistricto.SelectedValue = ubicaciones_CatalogoDistritos.IdGrupoDistrito.ToString();
                    ddlGrupoDistricto_SelectedIndexChanged(null, null);
                    ddlGrupoDistricto.Enabled = false;

                    ddlCatalogoDistritos.SelectedValue = ubicaciones_CatalogoDistritos_Zonas.IdDistrito.ToString();
                    ddlCatalogoDistritos.Enabled = false;
                    ddlCatalogoDistritos_SelectedIndexChanged(null, null);


                    ddlCatalogoDistritos_Zonas.SelectedValue = ubicaciones_CatalogoDistritos_Zonas.IdZona.ToString();
                    ddlCatalogoDistritos_Zonas.Enabled = false;

                }
                else
                {
                    string IdGrupoDistritoRequest = (Request.QueryString["IdGrupoDistrito"] == null) ? "" : Request.QueryString["IdGrupoDistrito"].ToString();
                    if (IdGrupoDistritoRequest != "")
                    {
                        ddlGrupoDistricto.SelectedValue = IdGrupoDistritoRequest;
                        ddlGrupoDistricto_SelectedIndexChanged(null, null);
                    }

                }

            }
        }

        #region Methods
        public Ubicaciones_CatalogoDistritos_Subzonas BuscarUbicaciones_CatalogoDistritos_SubZonas(int IdSubZona)
        {
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                Ubicaciones_CatalogoDistritos_Subzonas ubicaciones_CatalogoDistritos_Subzonas = (from x in entities.Ubicaciones_CatalogoDistritos_Subzonas
                                                                                                 where x.IdSubZona == IdSubZona
                                                                                                 select x).FirstOrDefault();
                return ubicaciones_CatalogoDistritos_Subzonas;
            }
        }
        public List<Ubicaciones_GruposDistritos> GetUbicaciones_GruposDistritosList()
        {

            DGHP_Entities entities = new DGHP_Entities();

            List<Ubicaciones_GruposDistritos> Ubicaciones_GruposDistritosList = (from tareas in entities.Ubicaciones_GruposDistritos
                                                                                 select tareas).ToList();

            return Ubicaciones_GruposDistritosList;

        }

        public Ubicaciones_CatalogoDistritos_Subzonas BuscarUbicaciones_CatalogoDistritos_SubZonasPorIdZonaCodigoSubZona(Ubicaciones_CatalogoDistritos_Subzonas ubicaciones_CatalogoDistritos_SubzonasIn)
        {
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                Ubicaciones_CatalogoDistritos_Subzonas ubicaciones_CatalogoDistritos_SubzonasOut = (from x in entities.Ubicaciones_CatalogoDistritos_Subzonas
                                                                                                    where x.IdZona == ubicaciones_CatalogoDistritos_SubzonasIn.IdZona
                                                                                              && x.CodigoSubZona == ubicaciones_CatalogoDistritos_SubzonasIn.CodigoSubZona
                                                                                                    select x).FirstOrDefault();
                return ubicaciones_CatalogoDistritos_SubzonasOut;
            }
        }

        #endregion

        #region Events


        protected void ddlGrupoDistricto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlCatalogoDistritos.DataSource = null;
            //ddlCatalogoDistritos.DataBind();



            DGHP_Entities entities = new DGHP_Entities();
            int IdGrupoDistrito = int.Parse(ddlGrupoDistricto.SelectedValue);
            List<Ubicaciones_CatalogoDistritos> Ubicaciones_CatalogoDistritosList = (from t in entities.Ubicaciones_CatalogoDistritos
                                                                                     where t.IdGrupoDistrito == IdGrupoDistrito
                                                                                     select t).ToList();
            ddlCatalogoDistritos.DataSource = Ubicaciones_CatalogoDistritosList;
            ddlCatalogoDistritos.DataTextField = "Codigo";
            ddlCatalogoDistritos.DataValueField = "IdDistrito";
            ddlCatalogoDistritos.DataBind();

            if (!string.IsNullOrEmpty(IdDistritoRequest))
            {
                ddlCatalogoDistritos.SelectedValue = IdDistritoRequest;
                IdDistritoRequest = string.Empty;
            }

            ddlCatalogoDistritos_SelectedIndexChanged(null, null);
        }

        protected void ddlCatalogoDistritos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlCatalogoDistritos_Zonas.DataSource = null;
            //ddlCatalogoDistritos_Zonas.DataBind();
          
            DGHP_Entities entities = new DGHP_Entities();
            int IdDistrito = int.Parse(ddlCatalogoDistritos.SelectedValue);
            List<Ubicaciones_CatalogoDistritos_Zonas> ubicaciones_CatalogoDistritos_ZonasList = (from t in entities.Ubicaciones_CatalogoDistritos_Zonas
                                                                                                 where t.IdDistrito == IdDistrito
                                                                                                 select t).ToList();
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
                ddlCatalogoDistritos_Zonas.SelectedIndex = -1;

            ddlCatalogoDistritos_Zonas_SelectedIndexChanged(null, null);
        }

        protected void ddlCatalogoDistritos_Zonas_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {



            Ubicaciones_CatalogoDistritos_Subzonas ubicaciones_CatalogoDistritos_Subzonas = new Ubicaciones_CatalogoDistritos_Subzonas();

            DGHP_Entities context = new DGHP_Entities();
            //using (DGHP_Entities entities = new DGHP_Entities())
            //{
            if (ddlCatalogoDistritos_Zonas.SelectedValue == "" || txtCodigoSubZona.Text==String.Empty)
            {
                //ASOSA MENSAJE DE ERROR
                ScriptManager sm = ScriptManager.GetCurrent(this);
                string cadena = "Todos los campos son Obligatorios";
                string script = string.Format("alert('{0}');", cadena);
                ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);
                return;
            }


            int IdZona = int.Parse(ddlCatalogoDistritos_Zonas.SelectedValue);
            int IdSubZona = int.Parse(hdIdSubZona.Value);

            if (IdSubZona > 0)
            {
                ubicaciones_CatalogoDistritos_Subzonas.IdSubZona = IdSubZona;

            }
            else
            {

                ubicaciones_CatalogoDistritos_Subzonas.IdSubZona = context.Ubicaciones_CatalogoDistritos_Subzonas.Max(x => x.IdSubZona) + 1;

            }
            ubicaciones_CatalogoDistritos_Subzonas.IdZona = IdZona;
            ubicaciones_CatalogoDistritos_Subzonas.CodigoSubZona = txtCodigoSubZona.Text.Trim();

            #region Valido si existe Codigo
            Ubicaciones_CatalogoDistritos_Subzonas ubicaciones_CatalogoDistritos_SubzonasAux = BuscarUbicaciones_CatalogoDistritos_SubZonasPorIdZonaCodigoSubZona(ubicaciones_CatalogoDistritos_Subzonas);
            if (ubicaciones_CatalogoDistritos_SubzonasAux != null)
            {
                //ASOSA MENSAJE DE ERROR
                ScriptManager sm = ScriptManager.GetCurrent(this);
                string cadena = "El Codigo " + ubicaciones_CatalogoDistritos_SubzonasAux.CodigoSubZona + " ya Existe para esa Zona";
                string script = string.Format("alert('{0}');", cadena);
                ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);
                return;
            }
            #endregion

            #region Transaccion

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {

                    context.Ubicaciones_CatalogoDistritos_Subzonas.AddOrUpdate(ubicaciones_CatalogoDistritos_Subzonas);


                    context.SaveChanges();
                    dbContextTransaction.Commit();

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw ex;
                }
            }
            #endregion
            Response.Redirect("~/Operaciones/CatalogoDistritos_SubzonasIndex.aspx?IdGrupoDistrito=" + ddlGrupoDistricto.SelectedValue + "&IdDistrito=" + ddlCatalogoDistritos.SelectedValue + "&IdZona=" + ddlCatalogoDistritos_Zonas.SelectedValue);

        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/CatalogoDistritos_SubzonasIndex.aspx?IdGrupoDistrito=" + ddlGrupoDistricto.SelectedValue + "&IdDistrito=" + ddlCatalogoDistritos.SelectedValue + "&IdZona=" + ddlCatalogoDistritos_Zonas.SelectedValue);
        }

        #endregion


    }
}