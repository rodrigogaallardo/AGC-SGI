using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using Elmah;
using ExcelLibrary.BinaryFileFormat;
using SGI.Model;
using Syncfusion.JavaScript.DataVisualization.DiagramEnums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SGI.Constants;

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
                else
                {
                    using (var db = new DGHP_Entities())
                    {
                        var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                        int id_tipotramite = sol.id_tipotramite;
                        string circuito_origen = sol.circuito_origen;
                        DateTime BOLETA_0_FECHADESDE = DateTime.Parse(ConfigurationManager.AppSettings["BOLETA_0_FECHADESDE"]);

                        #region Busco cod_grupo_circuito
                        int id_tramitetarea = db.SGI_Tramites_Tareas_HAB.Where(x => x.id_solicitud == id_solicitud).Min(x => x.id_tramitetarea);

                        int id_tarea = (from u in db.SGI_Tramites_Tareas
                                        where u.id_tramitetarea == id_tramitetarea
                                        select u.id_tarea).FirstOrDefault();

                        int id_circuito = (from u in db.ENG_Tareas
                                           where u.id_tarea == id_tarea
                                           select u.id_circuito).FirstOrDefault();

                        int? id_grupocircuito = (from u in db.ENG_Circuitos
                                                 where u.id_circuito == id_circuito
                                                 select u.id_grupocircuito).FirstOrDefault();

                        //string cod_grupo_circuito = (from u in db.ENG_Grupos_Circuitos
                        //                   where u.id_grupo_circuito == id_grupocircuito
                        //                   select u.cod_grupo_circuito).FirstOrDefault();
                        #endregion

                        bool flagAGC = true;
                        bool flagAPRA = true;

                        if (DateTime.Now >= BOLETA_0_FECHADESDE)
                        {
                            if (id_tipotramite == (int)TipoDeTramite.Habilitacion)
                            {
                                #region AGC
                                List<SGI.GestionTramite.Controls.ucPagos.clsItemGrillaPagos> lstPagosAGC = ucPagos.PagosAGCList(id_solicitud);
                                if (lstPagosAGC.Count > 0)
                                    flagAGC = true;
                                else
                                    flagAGC = false;
                                #endregion

                                #region APRA
                                List<SGI.GestionTramite.Controls.ucPagos.clsItemGrillaPagos> lstPagosAPRA = ucPagos.PagosAPRAList(id_solicitud);
                                if (lstPagosAPRA.Count > 0)
                                    flagAPRA = true;
                                else
                                    flagAPRA = false;
                                #endregion


                                if (!flagAGC & !flagAPRA)
                                {
                                    ucPagos.Visible = false;
                                }
                                else
                                {
                                    if (!flagAGC)
                                    {
                                        ucPagos.CargarPagosAGCVisibility(false);//ESCONDO AGC
                                    }
                                    if (!flagAPRA)
                                    {
                                        ucPagos.CargarPagosAPRAVisibility(false);//ESCONDO APRA
                                    }
                                }
                            }
                        }
                    }
                }
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
                    LogError.Write(ex, "Procedimiento CargarDatosTramite en VisorTramite.aspx");
                    if (ex.InnerException != null)
                        Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", ex.InnerException.Message + Environment.NewLine + ex.InnerException.Source + Environment.NewLine + ex.InnerException.TargetSite));
                    else
                        Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.TargetSite));
                }
            }
        }

    }


}