using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public partial class VisorTramite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id_solicitud = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                ComprobarSolicitud(id_solicitud);
                if (id_solicitud > 0)
                    CargarDatosTramite(id_solicitud);
                if (id_solicitud < 300000) 
                    ucPagos.Visible = false;
            }

        }

        private void ComprobarSolicitud(int id_solicitud)
        {
            if (id_solicitud <= 0)
            {
                Server.Transfer("~/Errores/error3020.aspx");
            }
        }

        private void CargarDatosTramite(int id_solicitud)
        {
            using (var db = new DGHP_Entities())
            {
                try
                {

                    Engine.getIdGrupoTrabajo(id_solicitud, out int id_grupotramite);

                    var estadosSolPres = db.TipoEstadoSolicitud.Where(e =>
                        e.Id == (int)Constants.Solicitud_Estados.Pendiente_de_Ingreso ||
                        e.Id == (int)Constants.Solicitud_Estados.En_trámite)
                        .Select(e => e.Nombre).ToList();

                    if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                    {
                        var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);

                        var ultimaSolicitudPresentada = sol?.SSIT_Solicitudes_HistorialEstados.Where(h =>
                        estadosSolPres.Contains(h.cod_estado_nuevo)).Select(h => h.fecha_modificacion).OrderByDescending(h => h).FirstOrDefault();

                        ucListaRubros.LoadData(sol, ultimaSolicitudPresentada);
                        ucListaDocumentos.LoadData(sol, ultimaSolicitudPresentada);

                    }
                    else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
                    {
                        var trf = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                        var ultimaTransmisionPresentada = trf?.Transf_Solicitudes_HistorialEstados.Where(h =>
                            estadosSolPres.Contains(h.cod_estado_nuevo)).Select(h => h.fecha_modificacion).OrderByDescending(h => h).FirstOrDefault();
                        ucListaRubros.LoadData(trf, ultimaTransmisionPresentada);
                        ucListaDocumentos.LoadData(trf, ultimaTransmisionPresentada);
                    }
                    else
                    {
                        ucListaDocumentos.LoadData(id_grupotramite, id_solicitud);
                        ucListaRubros.LoadData(id_solicitud);
                    }

                    
                    ucCabecera.LoadData(id_grupotramite, id_solicitud);
                    ucTramitesRelacionados.LoadData(id_solicitud);
                    ucListaTareas.LoadData(id_grupotramite, id_solicitud);
                    ucNotificaciones.LoadData(id_solicitud);
                    ucPagos.LoadData(id_solicitud);

                }
                catch (Exception ex)
                {
                    Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", ex.Message));
                }
            }
        }

    }


}