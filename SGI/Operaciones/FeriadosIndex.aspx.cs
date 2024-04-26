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
            int idFeriado = int.Parse(((Button)sender).ToolTip);
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                SGI_Feriados sGI_Feriados = entities.SGI_Feriados.Where(f => f.IdFeriado == idFeriado).FirstOrDefault();

                if (sGI_Feriados != null)
                {
    
                    try
                    {
                        entities.SGI_Feriados.Remove(sGI_Feriados);
                        entities.SaveChanges();
                        string script = "$('#frmEliminarLog').modal('show');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
                        id_object = idFeriado.ToString();
                    }
                    catch (Exception ex)
                    {
                        //ASOSA MENSAJE DE ERROR
                        ScriptManager sm = ScriptManager.GetCurrent(this);
                        string cadena = "No pudo borrarse el Feriado. Intente mas tarde";
                        string script = string.Format("alert('{0}');", cadena);
                        ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);
                    }
                }

                gridView.EditIndex = -1;
                this.CargarFeriados();
            }
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

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            DGHP_Entities db = new DGHP_Entities();
            int value = int.Parse(id_object);
            SGI_Feriados obj = db.SGI_Feriados.FirstOrDefault(x => x.IdFeriado == value);
            db.Dispose();
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "D", 4022);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            DGHP_Entities db = new DGHP_Entities();
            int value = int.Parse(id_object);
            SGI_Feriados obj = db.SGI_Feriados.FirstOrDefault(x => x.IdFeriado == value);
            db.Dispose();
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, string.Empty, "D", 4022);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
        }
    }
}