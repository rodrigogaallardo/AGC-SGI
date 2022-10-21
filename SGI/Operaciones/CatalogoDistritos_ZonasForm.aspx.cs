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
    public partial class CatalogoDistritos_ZonasForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion


            string IdZonaStr = (Request.QueryString["IdZona"] == null) ? "" : Request.QueryString["IdZona"].ToString();
            if (String.IsNullOrEmpty(IdZonaStr))
            {
                //IdZonaStr = "0";
                Response.Redirect("~/Operaciones/CatalogoDistritos_ZonasIndex.aspx?IdGrupoDistrito=" + ddlGrupoDistricto.SelectedValue + "&IdDistrito=" + ddlCatalogoDistritos.SelectedValue);

            }
            int IdZona = int.Parse(IdZonaStr);
            hdIdZona.Value = IdZonaStr;


            if (!IsPostBack)
            {
                ddlGrupoDistricto.DataSource = GetUbicaciones_GruposDistritosList();
                ddlGrupoDistricto.DataTextField = "Nombre";
                ddlGrupoDistricto.DataValueField = "IdGrupoDistrito";
                ddlGrupoDistricto.DataBind();
                ddlGrupoDistricto.SelectedIndex = 0;
                ddlGrupoDistricto_SelectedIndexChanged(null, null);

                if (IdZona > 0)
                {
                    Ubicaciones_CatalogoDistritos_Zonas ubicaciones_CatalogoDistritos_Zonas = new Ubicaciones_CatalogoDistritos_Zonas();
                    ubicaciones_CatalogoDistritos_Zonas = BuscarUbicaciones_CatalogoDistritos_Zonas(IdZona);
                    txtCodigoZona.Text = ubicaciones_CatalogoDistritos_Zonas.CodigoZona;


                    Ubicaciones_CatalogoDistritos ubicaciones_CatalogoDistritos;
                    using (DGHP_Entities entities = new DGHP_Entities())
                    {
                        ubicaciones_CatalogoDistritos = (from x in entities.Ubicaciones_CatalogoDistritos
                                                         where x.IdDistrito == ubicaciones_CatalogoDistritos_Zonas.IdDistrito
                                                         select x).FirstOrDefault();
                    }

                    ddlGrupoDistricto.SelectedValue = ubicaciones_CatalogoDistritos.IdGrupoDistrito.ToString();
                    ddlGrupoDistricto_SelectedIndexChanged(null, null);
                    ddlGrupoDistricto.Enabled = false;

                    ddlCatalogoDistritos.SelectedValue = ubicaciones_CatalogoDistritos_Zonas.IdDistrito.ToString();
                    ddlCatalogoDistritos.Enabled = false;
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
        public Ubicaciones_CatalogoDistritos_Zonas BuscarUbicaciones_CatalogoDistritos_Zonas(int IdZona)
        {
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                Ubicaciones_CatalogoDistritos_Zonas ubicaciones_CatalogoDistritos_Zonas = (from x in entities.Ubicaciones_CatalogoDistritos_Zonas
                                                                                           where x.IdZona == IdZona
                                                                                           select x).FirstOrDefault();
                return ubicaciones_CatalogoDistritos_Zonas;
            }
        }
        public List<Ubicaciones_GruposDistritos> GetUbicaciones_GruposDistritosList()
        {

            DGHP_Entities entities = new DGHP_Entities();

            List<Ubicaciones_GruposDistritos> Ubicaciones_GruposDistritosList = (from tareas in entities.Ubicaciones_GruposDistritos
                                                                                 select tareas).ToList();

            return Ubicaciones_GruposDistritosList;

        }

        public Ubicaciones_CatalogoDistritos_Zonas BuscarUbicaciones_CatalogoDistritos_ZonasPorIdDistritoCodigoZona(Ubicaciones_CatalogoDistritos_Zonas ubicaciones_CatalogoDistritos_ZonasIn)
        {
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                Ubicaciones_CatalogoDistritos_Zonas ubicaciones_CatalogoDistritos_ZonasOut = (from x in entities.Ubicaciones_CatalogoDistritos_Zonas
                                                                                              where x.IdDistrito == ubicaciones_CatalogoDistritos_ZonasIn.IdDistrito
                                                                                              && x.CodigoZona == ubicaciones_CatalogoDistritos_ZonasIn.CodigoZona
                                                                                              select x).FirstOrDefault();
                return ubicaciones_CatalogoDistritos_ZonasOut;
            }
        }

        #endregion

        #region Events


        protected void ddlGrupoDistricto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlCatalogoDistritos.DataSource = null;
            //ddlCatalogoDistritos.SelectedIndex = -1;
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

            string IdDistritoRequest = (Request.QueryString["IdDistrito"] == null) ? "" : Request.QueryString["IdDistrito"].ToString();
            if (IdDistritoRequest != "")
                ddlCatalogoDistritos.SelectedValue = IdDistritoRequest;

            ddlCatalogoDistritos_SelectedIndexChanged(null, null);
        }

        protected void ddlCatalogoDistritos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //hdIdDistrito.Value = ddlCatalogoDistritos.SelectedValue;
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {



            Ubicaciones_CatalogoDistritos_Zonas ubicaciones_CatalogoDistritos_Zonas = new Ubicaciones_CatalogoDistritos_Zonas();

            DGHP_Entities context = new DGHP_Entities();
            //using (DGHP_Entities entities = new DGHP_Entities())
            //{
            int IdZona = int.Parse(hdIdZona.Value);
            int IdDistrito = int.Parse(ddlCatalogoDistritos.SelectedValue);

            if (IdZona == 0)
            {
                ubicaciones_CatalogoDistritos_Zonas.IdZona = context.Ubicaciones_CatalogoDistritos_Zonas.Max(x => x.IdZona) + 1;

            }
            else
            {
                ubicaciones_CatalogoDistritos_Zonas.IdZona = IdZona;

            }
            ubicaciones_CatalogoDistritos_Zonas.IdDistrito = int.Parse(ddlCatalogoDistritos.SelectedValue);
            ubicaciones_CatalogoDistritos_Zonas.CodigoZona = txtCodigoZona.Text.Trim();

            #region Valido si existe Codigo
            Ubicaciones_CatalogoDistritos_Zonas ubicaciones_CatalogoDistritos_ZonasAux = BuscarUbicaciones_CatalogoDistritos_ZonasPorIdDistritoCodigoZona(ubicaciones_CatalogoDistritos_Zonas);
            if (ubicaciones_CatalogoDistritos_ZonasAux != null)
            {
                //ASOSA MENSAJE DE ERROR
                ScriptManager sm = ScriptManager.GetCurrent(this);
                string cadena = "El Codigo " + ubicaciones_CatalogoDistritos_ZonasAux.CodigoZona + " ya Existe para ese Distrito";
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

                    context.Ubicaciones_CatalogoDistritos_Zonas.AddOrUpdate(ubicaciones_CatalogoDistritos_Zonas);


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
            Response.Redirect("~/Operaciones/CatalogoDistritos_ZonasIndex.aspx?IdGrupoDistrito=" + ddlGrupoDistricto.SelectedValue + "&IdDistrito=" + ddlCatalogoDistritos.SelectedValue);

        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/CatalogoDistritos_ZonasIndex.aspx?IdGrupoDistrito=" + ddlGrupoDistricto.SelectedValue + "&IdDistrito=" + ddlCatalogoDistritos.SelectedValue);
        }
        #endregion


    }
}