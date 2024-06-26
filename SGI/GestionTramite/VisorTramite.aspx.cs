﻿using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using Elmah;
using ExcelLibrary.BinaryFileFormat;
using ExternalService.Class;
using SGI.Model;
using Syncfusion.JavaScript.DataVisualization.DiagramEnums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.InsertarCaas;
using static SGI.Constants;
using SGI.GestionTramite.Controls;
using ws_solicitudes;

namespace SGI.GestionTramite
{
    public partial class VisorTramite : System.Web.UI.Page
    {
        private enum TipoCertificadoCAA
        {
            sre = 16,
            sreCC = 17,
            sc = 18,
            cre = 19,
            DDJJ = 120   //TODO: Insertar nueva fila en la base de datos
        }
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id_solicitud = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                ComprobarSolicitud(id_solicitud);
                if (id_solicitud > 0)
                    await CargarDatosTramite(id_solicitud);
                if (id_solicitud < 300000)
                    ucPagos.Visible = false;
                else
                {
                    #region ASOSA BOLETA 0
                    using (var db = new DGHP_Entities())
                    {
                        var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                        int id_tipotramite = sol.id_tipotramite;
                        DateTime BOLETA_0_FECHADESDE = DateTime.Parse(ConfigurationManager.AppSettings["BOLETA_0_FECHADESDE"]);

                        bool flagAGC = true;
                        bool flagAPRA = true;

                        if (DateTime.Now >= BOLETA_0_FECHADESDE)
                        {
                                #region AGC
                                List<SGI.GestionTramite.Controls.ucPagos.clsItemGrillaPagos> lstPagosAGC = ucPagos.PagosAGCList(id_solicitud);
                                if (lstPagosAGC.Count > 0)
                                    flagAGC = true;
                                else
                                    flagAGC = false;
                                #endregion

                                #region APRA
                                List<SGI.GestionTramite.Controls.ucPagos.clsItemGrillaPagos> lstPagosAPRA = await ucPagos.PagosAPRAList(id_solicitud);
                            if (lstPagosAPRA != null && lstPagosAPRA.Count > 0)
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
                    #endregion
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

        private async System.Threading.Tasks.Task CargarDatosTramite(int id_solicitud)
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
                        await CheckIsCAAGenerated(sol.id_solicitud);
                        await ucListaDocumentos.LoadData(sol, ultimaSolicitudPresentada);

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
                        await ucListaDocumentos.LoadData(id_grupotramite, id_solicitud);
                        ucListaRubros.LoadData(id_solicitud);
                    }


                    ucCabecera.LoadData(id_grupotramite, id_solicitud);
                    ucTramitesRelacionados.LoadData(id_solicitud);
                    ucListaTareas.LoadData(id_grupotramite, id_solicitud);
                    ucNotificaciones.LoadData(id_solicitud);
                    await ucPagos.LoadData(id_solicitud);

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

        /// <summary>
        /// Revisa si ya existen CAA generadados en SIPSA
        /// de ser asi los asigna y los guarda en AGC_Files
        /// </summary>
        private async Task<bool> CheckIsCAAGenerated(int id_solicitud)
        {
            bool localhost = false;
            //Buscar encomiendas y agregarlas a la lista
            Functions.GetParametroChar("SSIT.URL");
            string Usuario = ConfigurationManager.AppSettings["UsuarioApraAgc"];
            string Password = ConfigurationManager.AppSettings["PasswordApraAgc"];
            WSssit wSssit = new WSssit();
            if (localhost)
                wSssit.Url = "http://localhost:56469/WSssit.asmx";
            else
                wSssit.Url = Parametros.GetParam_ValorChar("SSIT.Url") + "WSssit.asmx";
            wSssit.InsertarCAA_DocAdjuntosAsync(Usuario, Password, id_solicitud);

            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));

            var startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalSeconds <= 60)
            {
                using (var db = new DGHP_Entities())
                {
                    IQueryable<int> idEncomiendasPresentadas = null;
                    db.Database.CommandTimeout = 300;
                    var ultimaPresentacion = DateTime.Now;
                    if (ultimaPresentacion != null)
                    {
                        idEncomiendasPresentadas = (from rel in db.Encomienda_SSIT_Solicitudes
                                                    join enc in db.Encomienda on rel.id_encomienda equals enc.id_encomienda
                                                    join hist in db.Encomienda_HistorialEstados on enc.id_encomienda equals hist.id_encomienda
                                                    where rel.id_solicitud == id_solicitud
                                                      && (enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo || enc.id_estado == (int)Constants.Encomienda_Estados.Vencida)
                                                       && hist.fecha_modificacion <= ultimaPresentacion
                                                    orderby enc.id_encomienda descending
                                                    select enc.id_encomienda);
                    }

                    var archivos = (from encdoc in db.Encomienda_DocumentosAdjuntos
                                    where idEncomiendasPresentadas.Contains(encdoc.id_encomienda)
                                    select new itemDocumentov1
                                    {
                                        id_doc_adj = encdoc.id_docadjunto,
                                        nombre = (encdoc.id_tdocreq != 0 ? encdoc.TiposDeDocumentosRequeridos.nombre_tdocreq : encdoc.TiposDeDocumentosSistema.nombre_tipodocsis) + "-" + encdoc.id_encomienda,
                                        id_file = encdoc.id_file,
                                        id_tipodocsis = encdoc.id_tipodocsis,
                                        id_solicitud = encdoc.id_encomienda,
                                    }
                    ).ToList();

                    if (archivos.Any(x => x.id_tipodocsis == 4))
                    {
                        return true;
                    }
                }

                // Esperar 3 segundos antes de verificar la condición de nuevo
                await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(3));
            }

            return false;
        }
    }


}