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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SGI.Model.Engine;
using static Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAlrunsRecord;

namespace SGI.Operaciones
{
    public partial class FeriadosForm : System.Web.UI.Page
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
                calFecha.SelectedDate = DateTime.Today;
                calFecha.VisibleDate = DateTime.Today;
            }

        }

        #region Methods





        #endregion

        #region Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                string cadena = "Debe Ingresar alguna Descripcion";
                string script = string.Format("alert('{0}');", cadena);
                ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);
                return;
            }

            Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();

            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, string.Empty, url, txtObservacionesSolicitante.Text, "I");

            DGHP_Entities context = new DGHP_Entities();
            
            SGI_Feriados sGI_Feriados = new SGI_Feriados();


            int existe = (from f in context.SGI_Feriados
                          where f.Fecha == calFecha.SelectedDate
                          select f).Count();
            if (existe > 0)
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                string cadena = "Ya existe un feriado para esta Fecha";
                string script = string.Format("alert('{0}');", cadena);
                ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);
                return;
            }

            sGI_Feriados.Descripcion = txtDescripcion.Text;
            sGI_Feriados.CreateDate = DateTime.Now;
            sGI_Feriados.CreateUser = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
            sGI_Feriados.Fecha = calFecha.SelectedDate;






            #region Transaccion

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.SGI_Feriados.AddOrUpdate(sGI_Feriados);

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
            Response.Redirect("~/Operaciones/FeriadosIndex.aspx");

        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/FeriadosIndex.aspx");
        }

        #endregion


    }
}