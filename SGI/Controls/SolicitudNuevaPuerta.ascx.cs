using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using System.Drawing;
using System.Data;
using System.Data.Entity.Core.Objects;
using SGI.Webservices.Pagos;


namespace SGI.Controls
{
    public partial class SolicitudNuevaPuerta : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updSolicitarNuevaPuerta, updSolicitarNuevaPuerta.GetType(), "init_Js_updSolicitarNuevaPuerta", "init_Js_updSolicitarNuevaPuerta();", true);
            }

        }
        

        public void LoadData(int id_ubicacion)
        {

            DGHP_Entities db = new DGHP_Entities();

            var q = (from ubic in db.Ubicaciones
                     join puer in db.Ubicaciones_Puertas on ubic.id_ubicacion equals puer.id_ubicacion
                     where ubic.id_ubicacion == id_ubicacion
                     select new SGI.Model.Ubicacion
                     {
                         id_ubicacion = ubic.id_ubicacion,
                         id_subtipoubicacion = ubic.id_subtipoubicacion,
                         NroPartidaMatriz = ubic.NroPartidaMatriz,
                         Seccion = ubic.Seccion,
                         Manzana = ubic.Manzana,
                         Parcela = ubic.Parcela,
                         VigenciaDesde = ubic.VigenciaDesde,
                         VigenciaHasta = ubic.VigenciaHasta
                     }).Distinct();

            gridubicacion.DataSource = q.ToList();
            gridubicacion.DataBind();

            db.Dispose();


        }

       
        protected void gridubicacion_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                HiddenField hid_id_ubicacion = (HiddenField)e.Row.FindControl("hid_id_ubicacion");
                DataList lstPuertas = (DataList)e.Row.FindControl("lstPuertas");
                Syncfusion.JavaScript.Web.Autocomplete AutocompleteCalles = (Syncfusion.JavaScript.Web.Autocomplete)e.Row.FindControl("AutocompleteCalles");

                int id_ubicacion = int.Parse(hid_id_ubicacion.Value);


                DGHP_Entities db = new DGHP_Entities();
                SGI.Model.Ubicacion ubicacion = (from ubic in db.Ubicaciones
                                                 where ubic.id_ubicacion == id_ubicacion
                                                 select new SGI.Model.Ubicacion
                                             {
                                                 id_ubicacion = ubic.id_ubicacion,
                                                 id_subtipoubicacion = ubic.id_subtipoubicacion,
                                                 NroPartidaMatriz = ubic.NroPartidaMatriz,
                                                 Seccion = ubic.Seccion,
                                                 Manzana = ubic.Manzana,
                                                 Parcela = ubic.Parcela,
                                                 VigenciaDesde = ubic.VigenciaDesde,
                                                 VigenciaHasta = ubic.VigenciaHasta
                                             }).First();


                lstPuertas.DataSource = ubicacion.GetPuertas();
                lstPuertas.DataBind();

                Functions.CargarAutocompleteCalles(AutocompleteCalles);
               

                
                db.Dispose();



            }

        }

        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {

            if (gridubicacion.Rows.Count > 0)
            {
                Guid userid = SGI.Functions.GetUserId();
                GridViewRow row = gridubicacion.Rows[0];
                DropDownList ddlCalles_NP = (DropDownList)row.FindControl("ddlCalles_NP");
                TextBox txtNroPuerta_NP = (TextBox)row.FindControl("txtNroPuerta_NP");
                HiddenField hid_id_ubicacion = (HiddenField)row.FindControl("hid_id_ubicacion");

                int id_ubicacion = int.Parse(hid_id_ubicacion.Value);
                int NroPuerta = int.Parse(txtNroPuerta_NP.Text);

                Mailer.MailMessages.MailSolicitudNuevaPuerta(userid, id_ubicacion, ddlCalles_NP.SelectedItem.Text, NroPuerta);
                
                btnEnviarMail.Visible = false;
                pnlEnviadoOK.Visible = true;
                //((BasePage)this.Page).EjecutarScript(updSolicitarNuevaPuerta, "hidefrmSolicitarNuevaPuerta();");

            }

        }



    }
}