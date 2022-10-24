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
    public partial class CatalogoDistritosForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion


            string IdDistritoStr = (Request.QueryString["IdDistrito"] == null) ? "" : Request.QueryString["IdDistrito"].ToString();
            if (String.IsNullOrEmpty(IdDistritoStr))
            {
                IdDistritoStr = "0";
            }
            int IdDistrito = int.Parse(IdDistritoStr);
            hdIdDistrito.Value = IdDistritoStr;



           




            Ubicaciones_CatalogoDistritos ubicaciones_CatalogoDistritos = new Ubicaciones_CatalogoDistritos();
            ubicaciones_CatalogoDistritos = BuscarUbicaciones_CatalogoDistritos(IdDistrito);
            if (!IsPostBack)
            {
                ddlGrupoDistricto.DataSource = GetUbicaciones_GruposDistritosList();
                ddlGrupoDistricto.DataTextField = "Nombre";
                ddlGrupoDistricto.DataValueField = "IdGrupoDistrito";
                ddlGrupoDistricto.DataBind();

                if (IdDistrito > 0)
                {
                    txtCodigo.Text = ubicaciones_CatalogoDistritos.Codigo;
                    txtDescripcion.Text = ubicaciones_CatalogoDistritos.Descripcion;
                    txtOrden.Text = ubicaciones_CatalogoDistritos.orden.ToString();
                    hdIdGrupoDistrito.Value = ubicaciones_CatalogoDistritos.IdGrupoDistrito.ToString();
                    ddlGrupoDistricto.SelectedValue = ubicaciones_CatalogoDistritos.IdGrupoDistrito.ToString();
                    ddlGrupoDistricto.Enabled = false;
                }
                else
                {
                    string IdGrupoDistritoRequest = (Request.QueryString["IdGrupoDistrito"] == null) ? "" : Request.QueryString["IdGrupoDistrito"].ToString();
                    if (IdGrupoDistritoRequest != "")
                        ddlGrupoDistricto.SelectedValue = IdGrupoDistritoRequest;
                }

            }
        }

        #region Methods
        public Ubicaciones_CatalogoDistritos BuscarUbicaciones_CatalogoDistritos(int IdDistrito)
        {
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                Ubicaciones_CatalogoDistritos ubicaciones_CatalogoDistritos = (from x in entities.Ubicaciones_CatalogoDistritos
                                                                               where x.IdDistrito == IdDistrito
                                                                               select x).FirstOrDefault();
                return ubicaciones_CatalogoDistritos; 
            }
        }
        public Ubicaciones_CatalogoDistritos BuscarUbicaciones_CatalogoDistritosPorCodigo(string Codigo)
        {
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                Ubicaciones_CatalogoDistritos ubicaciones_CatalogoDistritos = (from x in entities.Ubicaciones_CatalogoDistritos
                                                                               where x.Codigo == Codigo
                                                                               select x).FirstOrDefault();
                return ubicaciones_CatalogoDistritos;
            }
        }
        #endregion

        #region Events


        public List<Ubicaciones_GruposDistritos> GetUbicaciones_GruposDistritosList()
        {

            DGHP_Entities entities = new DGHP_Entities();

            List<Ubicaciones_GruposDistritos> Ubicaciones_GruposDistritosList = (from tareas in entities.Ubicaciones_GruposDistritos
                                                                                 select tareas).ToList();

            return Ubicaciones_GruposDistritosList;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region Valido  Codigo y orden
            if (txtCodigo.Text == string.Empty || txtOrden.Text == string.Empty || !(int.TryParse(txtOrden.Text, out _)))
            {
                //ASOSA MENSAJE DE ERROR
                ScriptManager sm = ScriptManager.GetCurrent(this);
                string cadena = "El Codigo y El Orden son Obligatorios (Orden debe ser Numerico)";
                string script = string.Format("alert('{0}');", cadena);
                ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);
                return;
            }
            #endregion


            Ubicaciones_CatalogoDistritos ubicaciones_CatalogoDistritos = new Ubicaciones_CatalogoDistritos();
            
            DGHP_Entities context = new DGHP_Entities();
            //using (DGHP_Entities entities = new DGHP_Entities())
            //{
            int IdGrupoDistrito = int.Parse(hdIdDistrito.Value);


            if (IdGrupoDistrito == 0)
            {
                ubicaciones_CatalogoDistritos.IdDistrito = context.Ubicaciones_CatalogoDistritos.Max(x => x.IdDistrito) + 1;
                ubicaciones_CatalogoDistritos.IdGrupoDistrito = int.Parse(ddlGrupoDistricto.SelectedValue);
            }
            else
            {
                ubicaciones_CatalogoDistritos.IdDistrito = IdGrupoDistrito;
                ubicaciones_CatalogoDistritos.IdGrupoDistrito = int.Parse(hdIdGrupoDistrito.Value);
            }

            ubicaciones_CatalogoDistritos.Codigo = txtCodigo.Text.Trim();
            ubicaciones_CatalogoDistritos.Descripcion = txtDescripcion.Text.Trim();
            ubicaciones_CatalogoDistritos.orden = int.Parse(txtOrden.Text.Trim());
            ubicaciones_CatalogoDistritos.CreateDate = DateTime.Today;

            #region Valido si existe Codigo
            Ubicaciones_CatalogoDistritos ubicaciones_CatalogoDistritosAux =BuscarUbicaciones_CatalogoDistritosPorCodigo(ubicaciones_CatalogoDistritos.Codigo);
            if(ubicaciones_CatalogoDistritosAux!=null)
            {
                //ASOSA MENSAJE DE ERROR
                ScriptManager sm = ScriptManager.GetCurrent(this);
                string cadena = "El Codigo " + ubicaciones_CatalogoDistritos.Codigo + " ya Existe";
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

                    context.Ubicaciones_CatalogoDistritos.AddOrUpdate(ubicaciones_CatalogoDistritos);
                   

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
            Response.Redirect("~/Operaciones/CatalogoDistritosIndex.aspx?IdGrupoDistrito=" + ddlGrupoDistricto.SelectedValue );

        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/CatalogoDistritosIndex.aspx?IdGrupoDistrito=" + ddlGrupoDistricto.SelectedValue);
        }

        #endregion

       
    }
}