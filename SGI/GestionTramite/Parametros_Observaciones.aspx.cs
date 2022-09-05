using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public partial class Parametros_Observaciones : BasePage
    {
        DGHP_Entities db = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos();
            }
        }

        protected void CargarDatos()
        {
            db = new DGHP_Entities();
            var q = (
                        from posb in db.Parametros_Observaciones
                        select new clsItemTxtObservaciones
                        {
                            id_circuito = posb.id_circuito,
                            Cantidad = posb.Cantidad
                        }).ToList();

            for (int i = 0; i < q.Count(); i++)
            {
                switch (q.ElementAt(i).id_circuito)
                {
                    case 11:
                        ddlSSP.SelectedValue = q.ElementAt(i).Cantidad.ToString();
                        break;
                    case 12:
                        ddlSCP.SelectedValue = q.ElementAt(i).Cantidad.ToString();
                        break;
                    case 13:
                        ddlESPE.SelectedValue = q.ElementAt(i).Cantidad.ToString();
                        break;
                    case 14:
                        ddlESPA.SelectedValue = q.ElementAt(i).Cantidad.ToString();
                        break;
                }
            }
            updPnlObservaciones.Update();

        }

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            db.Actualizacion_Parametros_Observaciones(11, Convert.ToInt32(ddlSSP.SelectedValue));
            db.Actualizacion_Parametros_Observaciones(12, Convert.ToInt32(ddlSCP.SelectedValue));
            db.Actualizacion_Parametros_Observaciones(13, Convert.ToInt32(ddlESPE.SelectedValue));
            db.Actualizacion_Parametros_Observaciones(14, Convert.ToInt32(ddlESPA.SelectedValue));

            lblMsj.Text = "Cambios Guardados.";
            this.EjecutarScript(updMsj, "showfrmMsj();");
            return;

        }
    }
}