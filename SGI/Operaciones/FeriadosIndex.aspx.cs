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
    public partial class FeriadosIndex : System.Web.UI.Page
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
                calFechaDesde.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
                calFechaDesde.VisibleDate = new DateTime(DateTime.Now.Year, 1, 1);
                calFechaHasta.SelectedDate = new DateTime(DateTime.Now.Year, 12, 31);
                calFechaHasta.VisibleDate = new DateTime(DateTime.Now.Year, 12, 31);
                CargarFeriados();
            }
        }
        public static List<SGI_Feriados> sGI_FeriadosList;
        public void CargarFeriados()
        {
            gridView.DataSource = null;
            gridView.DataBind();
            if (calFechaDesde.SelectedDate> calFechaHasta.SelectedDate)
            {

                ScriptManager sm = ScriptManager.GetCurrent(this);
                string cadena = "La Fecha Hasta debe ser Mayor o Igual a la Fecha Desde";
                string script = string.Format("alert('{0}');", cadena);
                ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);
                return;
            }



                DGHP_Entities entities = new DGHP_Entities();

                sGI_FeriadosList = (from f in entities.SGI_Feriados
                                                        where f.Fecha >= calFechaDesde.SelectedDate
                                                        && f.Fecha <= calFechaHasta.SelectedDate
                                    select f).ToList();

               

               

              //  hdidSolicitud.Value = idSolicitud.ToString();
               

                gridView.DataSource = sGI_FeriadosList;
                gridView.DataBind();
          
        }

      


        protected void btnBuscarSolicitud_Click(object sender, EventArgs e)
        {
            this.CargarFeriados();
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            id_object = ((Button)sender).ToolTip;
            string script = "$('#frmEliminarLog').modal('show');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
        }
       

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
 
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string Fecha = e.Row.Cells[2].Text;

                    Button btnRemove = (Button)e.Row.Cells[0].Controls[1];
                    btnRemove.OnClientClick = "return confirm('¿Confirma que desea Eliminar el Feriado del  " + Fecha + " ?');";


                }

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
              Response.Redirect("~/Operaciones/FeriadosForm.aspx");
        }

        private void Eliminar()
        {
            int idFeriado = int.Parse(id_object);
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
                            SGI_Feriados obj = ctx.SGI_Feriados.FirstOrDefault(x => x.IdFeriado == idFeriado);
                            if (obj != null)
                                ctx.SGI_Feriados.Remove(obj);
                            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "D", 4022);
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
                string cadena = "No pudo borrarse el Feriado. Intente mas tarde";
                ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", string.Format("alert('{0}');", cadena), true);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
            gridView.EditIndex = -1;
            this.CargarFeriados();
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