using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAlrunsRecord;

namespace SGI.Operaciones
{
    public partial class GruposDistritosForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion


            string IdGrupoDistritoStr = (Request.QueryString["IdGrupoDistrito"] == null) ? "" : Request.QueryString["IdGrupoDistrito"].ToString();
            if (String.IsNullOrEmpty(IdGrupoDistritoStr))
            {
                IdGrupoDistritoStr = "0";
            }
            int IdGrupoDistrito = int.Parse(IdGrupoDistritoStr);
            hdIdGrupoDistrito.Value = IdGrupoDistritoStr;






            Ubicaciones_GruposDistritos ubicaciones_GruposDistritos = new Ubicaciones_GruposDistritos();
            ubicaciones_GruposDistritos = BuscarUbicaciones_GruposDistritos(IdGrupoDistrito);
            if (!IsPostBack)
            {
                if (IdGrupoDistrito > 0)
                {
                    txtCodigo.Text = ubicaciones_GruposDistritos.Codigo;
                    txtNombre.Text = ubicaciones_GruposDistritos.Nombre;
                    txtDefinicion.Text = ubicaciones_GruposDistritos.definicion;
                    txtReferencia.Text = ubicaciones_GruposDistritos.referencia;

                }

            }
        }

        #region Methods
        public Ubicaciones_GruposDistritos BuscarUbicaciones_GruposDistritos(int IdGrupoDistrito)
        {
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                Ubicaciones_GruposDistritos ubicaciones_GruposDistritos = (from x in entities.Ubicaciones_GruposDistritos
                                                                           where x.IdGrupoDistrito == IdGrupoDistrito
                                                                           select x).FirstOrDefault();
                return ubicaciones_GruposDistritos; 
            }
        }

        #endregion

        #region Events




        protected void btnSave_Click(object sender, EventArgs e)
        {
            Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();

            Ubicaciones_GruposDistritos ubicaciones_GruposDistritos = new Ubicaciones_GruposDistritos();
            
            DGHP_Entities context = new DGHP_Entities();
            //using (DGHP_Entities entities = new DGHP_Entities())
            //{
            int IdGrupoDistrito = int.Parse(hdIdGrupoDistrito.Value);


            if (IdGrupoDistrito == 0)
                ubicaciones_GruposDistritos.IdGrupoDistrito = context.Ubicaciones_GruposDistritos.Max(x => x.IdGrupoDistrito) + 1;
            else
                ubicaciones_GruposDistritos.IdGrupoDistrito = IdGrupoDistrito;

            ubicaciones_GruposDistritos.Codigo = txtCodigo.Text.Trim();
            ubicaciones_GruposDistritos.Nombre = txtNombre.Text.Trim();
            ubicaciones_GruposDistritos.definicion = txtDefinicion.Text.Trim();
            ubicaciones_GruposDistritos.referencia = txtReferencia.Text.Trim();


            #region Transaccion

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {

                    context.Ubicaciones_GruposDistritos.AddOrUpdate(ubicaciones_GruposDistritos);
                   

                    context.SaveChanges();
                    dbContextTransaction.Commit();
                    if (int.Parse(hdIdGrupoDistrito.Value) == 0)
                        Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, JsonConvert.SerializeObject(ubicaciones_GruposDistritos), url, txtObservacionesSolicitante.Text, "I", 4023);
                    else
                        Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, JsonConvert.SerializeObject(ubicaciones_GruposDistritos), url, txtObservacionesSolicitante.Text, "U", 4023);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw ex;
                }
            }
            #endregion
            Response.Redirect("~/Operaciones/GruposDistritosIndex.aspx");

        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/GruposDistritosIndex.aspx");
        }
        #endregion


    }
}