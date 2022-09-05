using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
namespace SGI.Tramite
{

    public partial class TramiteQr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Page.RouteData.Values["id_encomienda"] != null)
                {
                    string id_encomiendaBase64 = Convert.ToString(Page.RouteData.Values["id_encomienda"]);
                    byte[] encodedDataAsBytes = Convert.FromBase64String(id_encomiendaBase64);
                    string returnValue = Encoding.Default.GetString(encodedDataAsBytes);
                    int id_encomienda = Convert.ToInt32(returnValue);
                    if (id_encomienda > 0)
                        CargarDatosTramite(id_encomienda);
                }
            }

        }

        private void CargarDatosTramite(int id_encomienda)
        {
            DGHP_Entities db = new DGHP_Entities();
            AGC_FilesEntities dbFiles = new AGC_FilesEntities();

            int id_solicitud = db.Encomienda.FirstOrDefault(x => x.id_encomienda == id_encomienda).Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault();
            hdfid_solicitud.Value = Convert.ToString(id_solicitud);

            var q = (from tt in db.SGI_Tramites_Tareas
                     join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                     where tt_hab.id_solicitud == id_solicitud &&
                         tt.id_tarea == 22
                     select new
                     {
                         tt.id_tramitetarea
                     }
                ).FirstOrDefault();

            int id_tramitetarea = q.id_tramitetarea;
            hdfid_tarea.Value = Convert.ToString(id_tramitetarea);

            PdfDisposicion dispo = null;
            App_Data.dsImpresionDisposicion dsDispo = null;
            try
            {
                dispo = new PdfDisposicion();
                dsDispo = dispo.GenerarDataSetDisposicion(id_solicitud, id_tramitetarea, null, true);
                dispo.Dispose();
            }
            catch (Exception ex)
            {
                dispo.Dispose();
                throw ex;
            }


            DataRow row = null;

            row = dsDispo.Tables["Disposicion"].Rows[0];

            string dispo_nro_expediente = HttpUtility.HtmlEncode(Convert.ToString(row["expediente"]));

            lblNroExpediente.Text = dispo_nro_expediente;

            repeater_titulares.DataSource = dsDispo.Tables["Titulares"];
            repeater_titulares.DataBind();

            row = dsDispo.Tables["Ubicaciones"].Rows[0];
            lblSeccion.Text = Convert.ToString(row["Seccion"]);
            lblManzana.Text = Convert.ToString(row["Manzana"]);
            lblParcela.Text = Convert.ToString(row["Parcela"]);
            lblPartidaMatriz.Text = Convert.ToString(row["NroPartidaMatriz"]);

            lblDomicilio.Text = HttpUtility.HtmlEncode(Convert.ToString(row["Direcciones"]));

            grdRubros.DataSource = dsDispo.Tables["Rubros"];
            grdRubros.DataBind();
        }

        protected void linkDisposicion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Reportes/ImprimirDispoHtml.aspx?id_solicitud=" + hdfid_solicitud.Value + "&id_tramitetarea=" + hdfid_tarea.Value);
        }
    }

}