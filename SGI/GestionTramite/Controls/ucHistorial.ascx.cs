using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucHistorial : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        internal void LoadData(int tt, int id_solicitud)
        {
            if (tt == (int)Constants.TipoDeTramite.Consulta_Padron)
            {
                using (DGHP_Entities db = new DGHP_Entities())
                {
                    var historiales = (from hist in db.CPadron_HistorialEstados
                                       join est in db.TipoEstadoSolicitud on hist.cod_estado_nuevo equals est.Nombre
                                       where
                                         hist.id_cpadron == id_solicitud
                                       select new
                                       {
                                           fecha = hist.fecha_modificacion,
                                           estado = est.Descripcion,
                                       }).ToList();
                    grdHistorial.DataSource = historiales;
                    grdHistorial.DataBind();
                }
            }
            else if (tt == (int)Constants.TipoDeTramite.Transferencia)
            {
                using (DGHP_Entities db = new DGHP_Entities())
                {
                    var historiales = (from hist in db.Transf_Solicitudes_HistorialEstados
                                       join est in db.TipoEstadoSolicitud on hist.cod_estado_nuevo equals est.Nombre
                                       where
                                         hist.id_solicitud == id_solicitud
                                       select new
                                       {
                                           fecha = hist.fecha_modificacion,
                                           estado = est.Descripcion,
                                       }).ToList();
                    grdHistorial.DataSource = historiales;
                    grdHistorial.DataBind();
                }
            }
        }

        internal void LoadData(int id_solicitud)
        {
            using (DGHP_Entities db = new DGHP_Entities())
            {
                var historiales = (from hist in db.SSIT_Solicitudes_HistorialEstados
                                   join est in db.TipoEstadoSolicitud on hist.cod_estado_nuevo equals  est.Nombre
                                   where
                                     hist.id_solicitud == id_solicitud
                                   select new
                                   {
                                       fecha = hist.fecha_modificacion,
                                       estado = est.Descripcion
                                   }).ToList();
                grdHistorial.DataSource = historiales;
                grdHistorial.DataBind();
            }
        }
    }
}