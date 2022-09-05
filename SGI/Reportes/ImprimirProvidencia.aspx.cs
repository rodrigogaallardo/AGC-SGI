using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Reportes
{
    public partial class ImprimirProvidencia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DGHP_Entities db= null;
                try
                {
                    int id_tramitetarea = (Request.QueryString["id_tramitetarea"] == null) ? 0 : Convert.ToInt32(Request.QueryString["id_tramitetarea"]);
                    db = new DGHP_Entities();
                    SGI_Tarea_Revision_SubGerente tareaSubGerente = db.SGI_Tarea_Revision_SubGerente.Where(x => x.id_tramitetarea == id_tramitetarea).FirstOrDefault();
                    if (tareaSubGerente != null)
                        lblObservaciones.Text = tareaSubGerente.observacion_providencia;
                    else
                    {
                        SGI_Tarea_Revision_Gerente tareaGerente = db.SGI_Tarea_Revision_Gerente.Where(x => x.id_tramitetarea == id_tramitetarea).FirstOrDefault();
                        lblObservaciones.Text = tareaGerente.observacion_providencia;
                    }
                    lblObservaciones.Text = lblObservaciones.Text.Replace("\n", "<br />");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if(db!=null)
                        db.Dispose();
                }
            }
        }
    }
}
