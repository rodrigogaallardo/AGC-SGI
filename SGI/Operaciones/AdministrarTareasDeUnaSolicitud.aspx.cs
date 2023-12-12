using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using SGI.Model;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Operaciones
{
    public partial class AdministrarTareasDeUnaSolicitud : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion
            if (!IsPostBack)
            {
                string idSolicitudStr = (Request.QueryString["idSolicitud"] == null) ? "" : Request.QueryString["idSolicitud"].ToString();
                txtBuscarSolicitud.Text = idSolicitudStr;
                CargarSolicitudConTareas();
            }
        }
        public static List<SGI_Tramites_Tareas> tareasDeLaSolicitud;
        public void CargarSolicitudConTareas()
        {
            int idSolicitud;
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out idSolicitud);

            if (couldParse)
            {
                DGHP_Entities entities = new DGHP_Entities();
                var solic = 0;
                solic = (from sol in entities.Transf_Solicitudes where sol.id_solicitud == idSolicitud select sol.id_solicitud).FirstOrDefault();
                if (solic == 0)
                {
                    solic = (from sol in entities.SSIT_Solicitudes where sol.id_solicitud == idSolicitud select sol.id_solicitud).FirstOrDefault();
                    tareasDeLaSolicitud = (from tareas in entities.SGI_Tramites_Tareas
                                           join ttHab in entities.SGI_Tramites_Tareas_HAB
                                           on tareas.id_tramitetarea equals ttHab.id_tramitetarea
                                           where ttHab.id_solicitud == idSolicitud
                                           select tareas).ToList();
                    hdHAB_TRANSF.Value = "H";
                }
                else
                {
                    tareasDeLaSolicitud = (from tareas in entities.SGI_Tramites_Tareas
                                           join tTransf in entities.SGI_Tramites_Tareas_TRANSF
                                           on tareas.id_tramitetarea equals tTransf.id_tramitetarea
                                           where tTransf.id_solicitud == idSolicitud
                                           select tareas).ToList();
                    hdHAB_TRANSF.Value = "T";
                }

                if (tareasDeLaSolicitud.Count < 1)
                {
                    if (solic == 0)
                    {
                        hdHAB_TRANSF.Value = "";
                        btnNuevo.Enabled = false;//SI NO HAY REG TRANSF NI HAB ESCONTO BOTON NUEVO
                    }
                    else
                    {
                        btnNuevo.Enabled = true;
                    }
                }
                else
                {
                    btnNuevo.Enabled = true;
                }

                hdidSolicitud.Value = idSolicitud.ToString();
                //No permite editar/eliminar tarea si tiene procesos de sade exitosos.  Edicion parcial borrar on permite
                //La tabla que tiene los procesos de sade es SGI_SADE_Procesos

                //cuando sade = true +> edicion parcial
                //y el campo realizado_en_SADE es el que determina si un proceso fue ejecutado con exito
                //siendo 0 para no generado y 1 para generado.con exito
                //Los campos que permitie editar para las tareas con procesos existosos son

                gridView.DataSource = tareasDeLaSolicitud;
                gridView.DataBind();
            }
        }

        public IEnumerable<aspnet_Users> CargarTodosLosUsuarios()
        {
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                return entities.aspnet_Users.ToList();
            }
        }

        public IEnumerable<ENG_Tareas> CargarTodasLasTareas()
        {
            DGHP_Entities entities = new DGHP_Entities();
            return entities.ENG_Tareas.OrderBy(tarea => tarea.ENG_Circuitos.nombre_circuito).ToList();
        }





        protected void btnBuscarSolicitud_Click(object sender, EventArgs e)
        {
            this.CargarSolicitudConTareas();
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            int idTramiteTarea = int.Parse(((Button)sender).ToolTip);
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                SGI_Tramites_Tareas tramiteTarea = entities.SGI_Tramites_Tareas.Where(tarea => tarea.id_tramitetarea == idTramiteTarea).FirstOrDefault();

                if (tramiteTarea != null)
                {

                    #region SGI_Tramites_Tareas_HAB
                    List<SGI_Tramites_Tareas_HAB> SGI_Tramites_Tareas_HABList =
                      entities.SGI_Tramites_Tareas_HAB.Where(tth => tth.id_tramitetarea == idTramiteTarea).ToList();

                    entities.SGI_Tramites_Tareas_HAB.RemoveRange(SGI_Tramites_Tareas_HABList);
                    #endregion

                    #region SGI_Tramites_Tareas_TRANSF
                    List<SGI_Tramites_Tareas_TRANSF> SGI_Tramites_Tareas_TRANSFList =
                   entities.SGI_Tramites_Tareas_TRANSF.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    entities.SGI_Tramites_Tareas_TRANSF.RemoveRange(SGI_Tramites_Tareas_TRANSFList);
                    #endregion

                    //          #region SGI_Tarea_Documentos_Adjuntos
                    //          List<SGI_Tarea_Documentos_Adjuntos> SGI_Tarea_Documentos_AdjuntosList =
                    //             entities.SGI_Tarea_Documentos_Adjuntos.Where(tda => tda.id_tramitetarea == idTramiteTarea).ToList();
                    //          entities.SGI_Tarea_Documentos_Adjuntos.RemoveRange(SGI_Tarea_Documentos_AdjuntosList);

                    //          #endregion

                    //          #region SGI_Tarea_Generar_Expediente_Procesos
                    //          List<SGI_Tarea_Generar_Expediente_Procesos> SGI_Tarea_Generar_Expediente_ProcesosList =
                    //            entities.SGI_Tarea_Generar_Expediente_Procesos.Where(tgep => tgep.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Generar_Expediente_Procesos.RemoveRange(SGI_Tarea_Generar_Expediente_ProcesosList);

                    //          #endregion

                    //          #region SGI_SADE_Procesos
                    //          List<SGI_SADE_Procesos> SGI_SADE_ProcesosList =
                    //         entities.SGI_SADE_Procesos.Where(sp => sp.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_SADE_Procesos.RemoveRange(SGI_SADE_ProcesosList);

                    //          #endregion

                    //          #region SGI_Tarea_Aprobado
                    //          List<SGI_Tarea_Aprobado> SGI_Tarea_AprobadoList =
                    //            entities.SGI_Tarea_Aprobado.Where(ta => ta.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Aprobado.RemoveRange(SGI_Tarea_AprobadoList);
                    //          #endregion

                    //          #region SGI_Tarea_Calificar
                    //          List<SGI_Tarea_Calificar> SGI_Tarea_CalificarList =
                    //       entities.SGI_Tarea_Calificar.Where(tc => tc.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Calificar.RemoveRange(SGI_Tarea_CalificarList);
                    //          #endregion

                    //          #region SGI_Tarea_Revision_DGHP
                    //          List<SGI_Tarea_Revision_DGHP> SGI_Tarea_Revision_DGHPList =
                    //entities.SGI_Tarea_Revision_DGHP.Where(tr => tr.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Revision_DGHP.RemoveRange(SGI_Tarea_Revision_DGHPList);
                    //          #endregion

                    //          #region SGI_Tarea_Entregar_Tramite
                    //          List<SGI_Tarea_Entregar_Tramite> SGI_Tarea_Entregar_TramiteList =
                    //   entities.SGI_Tarea_Entregar_Tramite.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Entregar_Tramite.RemoveRange(SGI_Tarea_Entregar_TramiteList);
                    //          #endregion

                    //          #region SGI_Tarea_Dictamen_Realizar_Dictamen
                    //          List<SGI_Tarea_Dictamen_Realizar_Dictamen> SGI_Tarea_Dictamen_Realizar_DictamenList =
                    //         entities.SGI_Tarea_Dictamen_Realizar_Dictamen.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Dictamen_Realizar_Dictamen.RemoveRange(SGI_Tarea_Dictamen_Realizar_DictamenList);
                    //          #endregion

                    //          #region SGI_Tarea_Dictamen_Asignar_Profesional
                    //          List<SGI_Tarea_Dictamen_Asignar_Profesional> SGI_Tarea_Dictamen_Asignar_ProfesionalList =
                    //         entities.SGI_Tarea_Dictamen_Asignar_Profesional.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Dictamen_Asignar_Profesional.RemoveRange(SGI_Tarea_Dictamen_Asignar_ProfesionalList);
                    //          #endregion

                    //          #region SGI_Tarea_Revision_Gerente
                    //          List<SGI_Tarea_Revision_Gerente> SGI_Tarea_Revision_GerenteList =
                    //         entities.SGI_Tarea_Revision_Gerente.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Revision_Gerente.RemoveRange(SGI_Tarea_Revision_GerenteList);
                    //          #endregion

                    //          #region SGI_Tarea_Revision_SubGerente
                    //          List<SGI_Tarea_Revision_SubGerente> SGI_Tarea_Revision_SubGerenteList =
                    //         entities.SGI_Tarea_Revision_SubGerente.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Revision_SubGerente.RemoveRange(SGI_Tarea_Revision_SubGerenteList);
                    //          #endregion

                    //          #region SGI_Tarea_Calificar_ObsGrupo




                    //          List<SGI_Tarea_Calificar_ObsGrupo> SGI_Tarea_Calificar_ObsGrupoList =
                    //         entities.SGI_Tarea_Calificar_ObsGrupo.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          foreach (SGI_Tarea_Calificar_ObsGrupo sgi_Tarea_Calificar_ObsGrupo in SGI_Tarea_Calificar_ObsGrupoList)
                    //          {
                    //              List<SGI_Tarea_Calificar_ObsDocs> SGI_Tarea_Calificar_ObsDocsList =
                    //             entities.SGI_Tarea_Calificar_ObsDocs.Where(tetx => tetx.id_ObsGrupo == sgi_Tarea_Calificar_ObsGrupo.id_ObsGrupo).ToList();

                    //              entities.SGI_Tarea_Calificar_ObsDocs.RemoveRange(SGI_Tarea_Calificar_ObsDocsList);
                    //          }



                    //          entities.SGI_Tarea_Calificar_ObsGrupo.RemoveRange(SGI_Tarea_Calificar_ObsGrupoList);
                    //          #endregion

                    //          #region SGI_LIZA_Procesos
                    //          List<SGI_LIZA_Procesos> SGI_LIZA_ProcesosList =
                    //         entities.SGI_LIZA_Procesos.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_LIZA_Procesos.RemoveRange(SGI_LIZA_ProcesosList);
                    //          #endregion

                    //          #region SGI_LIZA_Ticket
                    //          List<SGI_LIZA_Ticket> SGI_LIZA_TicketList =
                    //         entities.SGI_LIZA_Ticket.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_LIZA_Ticket.RemoveRange(SGI_LIZA_TicketList);
                    //          #endregion

                    //          #region SGI_Solicitudes_Pagos
                    //          List<SGI_Solicitudes_Pagos> SGI_Solicitudes_PagosList =
                    //          entities.SGI_Solicitudes_Pagos.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Solicitudes_Pagos.RemoveRange(SGI_Solicitudes_PagosList);
                    //          #endregion

                    //          #region SGI_Tarea_Asignar_Calificador
                    //          List<SGI_Tarea_Asignar_Calificador> SGI_Tarea_Asignar_CalificadorList =
                    //        entities.SGI_Tarea_Asignar_Calificador.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Asignar_Calificador.RemoveRange(SGI_Tarea_Asignar_CalificadorList);
                    //          #endregion

                    //          #region SGI_Tarea_Asignar_Inspector
                    //          List<SGI_Tarea_Asignar_Inspector> SGI_Tarea_Asignar_InspectorList =
                    //          entities.SGI_Tarea_Asignar_Inspector.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Asignar_Inspector.RemoveRange(SGI_Tarea_Asignar_InspectorList);
                    //          #endregion

                    //          #region SGI_Tarea_Carga_Tramite
                    //          List<SGI_Tarea_Carga_Tramite> SGI_Tarea_Carga_TramiteList =
                    //          entities.SGI_Tarea_Carga_Tramite.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Carga_Tramite.RemoveRange(SGI_Tarea_Carga_TramiteList);
                    //          #endregion

                    //          #region SGI_Tarea_Dictamen_GEDO
                    //          List<SGI_Tarea_Dictamen_GEDO> SGI_Tarea_Dictamen_GEDOList =
                    //          entities.SGI_Tarea_Dictamen_GEDO.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Dictamen_GEDO.RemoveRange(SGI_Tarea_Dictamen_GEDOList);
                    //          #endregion

                    //          #region SGI_Tarea_Dictamen_Revisar_Tramite
                    //          List<SGI_Tarea_Dictamen_Revisar_Tramite> SGI_Tarea_Dictamen_Revisar_TramiteList =
                    //          entities.SGI_Tarea_Dictamen_Revisar_Tramite.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Dictamen_Revisar_Tramite.RemoveRange(SGI_Tarea_Dictamen_Revisar_TramiteList);
                    //          #endregion

                    //          #region SGI_Tarea_Dictamen_Revision_Gerente
                    //          List<SGI_Tarea_Dictamen_Revision_Gerente> SGI_Tarea_Dictamen_Revision_GerenteList =
                    //          entities.SGI_Tarea_Dictamen_Revision_Gerente.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Dictamen_Revision_Gerente.RemoveRange(SGI_Tarea_Dictamen_Revision_GerenteList);
                    //          #endregion

                    //          #region SGI_Tarea_Dictamen_Revision
                    //          List<SGI_Tarea_Dictamen_Revision> SGI_Tarea_Dictamen_RevisionList =
                    //          entities.SGI_Tarea_Dictamen_Revision.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Dictamen_Revision.RemoveRange(SGI_Tarea_Dictamen_RevisionList);
                    //          #endregion

                    //          #region SGI_Tarea_Dictamen_Revision_SubGerente
                    //          List<SGI_Tarea_Dictamen_Revision_SubGerente> SGI_Tarea_Dictamen_Revision_SubGerenteList =
                    //          entities.SGI_Tarea_Dictamen_Revision_SubGerente.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Dictamen_Revision_SubGerente.RemoveRange(SGI_Tarea_Dictamen_Revision_SubGerenteList);
                    //          #endregion

                    //          #region SGI_Tarea_Ejecutiva_NumeroGedo
                    //          List<SGI_Tarea_Ejecutiva_NumeroGedo> SGI_Tarea_Ejecutiva_NumeroGedoList =
                    //          entities.SGI_Tarea_Ejecutiva_NumeroGedo.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Ejecutiva_NumeroGedo.RemoveRange(SGI_Tarea_Ejecutiva_NumeroGedoList);
                    //          #endregion

                    //          #region SGI_Tarea_Ejecutiva
                    //          List<SGI_Tarea_Ejecutiva> SGI_Tarea_EjecutivaList =
                    //          entities.SGI_Tarea_Ejecutiva.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Ejecutiva.RemoveRange(SGI_Tarea_EjecutivaList);
                    //          #endregion

                    //          #region SGI_Tarea_Enviar_AVH
                    //          List<SGI_Tarea_Enviar_AVH> SGI_Tarea_Enviar_AVHList =
                    //          entities.SGI_Tarea_Enviar_AVH.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Enviar_AVH.RemoveRange(SGI_Tarea_Enviar_AVHList);
                    //          #endregion

                    //          #region SGI_Tarea_Enviar_DGFC
                    //          List<SGI_Tarea_Enviar_DGFC> SGI_Tarea_Enviar_DGFCList =
                    //          entities.SGI_Tarea_Enviar_DGFC.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Enviar_DGFC.RemoveRange(SGI_Tarea_Enviar_DGFCList);
                    //          #endregion

                    //          #region SGI_Tarea_Enviar_Procuracion
                    //          List<SGI_Tarea_Enviar_Procuracion> SGI_Tarea_Enviar_ProcuracionList =
                    //          entities.SGI_Tarea_Enviar_Procuracion.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Enviar_Procuracion.RemoveRange(SGI_Tarea_Enviar_ProcuracionList);
                    //          #endregion

                    //          #region SGI_Tarea_Enviar_PVH
                    //          List<SGI_Tarea_Enviar_PVH> SGI_Tarea_Enviar_PVHList =
                    //          entities.SGI_Tarea_Enviar_PVH.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Enviar_PVH.RemoveRange(SGI_Tarea_Enviar_PVHList);
                    //          #endregion

                    //          #region SGI_Tarea_Fin_Tramite
                    //          List<SGI_Tarea_Fin_Tramite> SGI_Tarea_Fin_TramiteList =
                    //          entities.SGI_Tarea_Fin_Tramite.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Fin_Tramite.RemoveRange(SGI_Tarea_Fin_TramiteList);
                    //          #endregion

                    //          #region SGI_Tarea_Generar_Expediente
                    //          List<SGI_Tarea_Generar_Expediente> SGI_Tarea_Generar_ExpedienteList =
                    //          entities.SGI_Tarea_Generar_Expediente.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Generar_Expediente.RemoveRange(SGI_Tarea_Generar_ExpedienteList);
                    //          #endregion

                    //          #region SGI_Tarea_Generar_Ticket_Liza
                    //          List<SGI_Tarea_Generar_Ticket_Liza> SGI_Tarea_Generar_Ticket_LizaList =
                    //          entities.SGI_Tarea_Generar_Ticket_Liza.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Generar_Ticket_Liza.RemoveRange(SGI_Tarea_Generar_Ticket_LizaList);
                    //          #endregion

                    //          #region SGI_Tarea_Informar_Doc_Sade
                    //          List<SGI_Tarea_Informar_Doc_Sade> SGI_Tarea_Informar_Doc_SadeList =
                    //          entities.SGI_Tarea_Informar_Doc_Sade.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Informar_Doc_Sade.RemoveRange(SGI_Tarea_Informar_Doc_SadeList);
                    //          #endregion

                    //          #region SGI_Tarea_Obtener_Ticket_Liza
                    //          List<SGI_Tarea_Obtener_Ticket_Liza> SGI_Tarea_Obtener_Ticket_LizaList =
                    //          entities.SGI_Tarea_Obtener_Ticket_Liza.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Obtener_Ticket_Liza.RemoveRange(SGI_Tarea_Obtener_Ticket_LizaList);
                    //          #endregion

                    //          #region SGI_Tarea_Pagos_log
                    //          List<SGI_Tarea_Pagos_log> SGI_Tarea_Pagos_logList =
                    //          entities.SGI_Tarea_Pagos_log.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Pagos_log.RemoveRange(SGI_Tarea_Pagos_logList);
                    //          #endregion

                    //          #region SGI_Tarea_Rechazo_En_SADE
                    //          List<SGI_Tarea_Rechazo_En_SADE> SGI_Tarea_Rechazo_En_SADEList =
                    //          entities.SGI_Tarea_Rechazo_En_SADE.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Rechazo_En_SADE.RemoveRange(SGI_Tarea_Rechazo_En_SADEList);
                    //          #endregion

                    //          #region SGI_Tarea_Resultado_Inspector
                    //          List<SGI_Tarea_Resultado_Inspector> SGI_Tarea_Resultado_InspectorList =
                    //          entities.SGI_Tarea_Resultado_Inspector.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Resultado_Inspector.RemoveRange(SGI_Tarea_Resultado_InspectorList);
                    //          #endregion

                    //          #region SGI_Tarea_Revision_Dictamenes
                    //          List<SGI_Tarea_Revision_Dictamenes> SGI_Tarea_Revision_DictamenesList =
                    //          entities.SGI_Tarea_Revision_Dictamenes.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Revision_Dictamenes.RemoveRange(SGI_Tarea_Revision_DictamenesList);
                    //          #endregion

                    //          #region SGI_Tarea_Revision_Director
                    //          List<SGI_Tarea_Revision_Director> SGI_Tarea_Revision_DirectorList =
                    //          entities.SGI_Tarea_Revision_Director.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Revision_Director.RemoveRange(SGI_Tarea_Revision_DirectorList);
                    //          #endregion

                    //          #region SGI_Tarea_Revision_Pagos
                    //          List<SGI_Tarea_Revision_Pagos> SGI_Tarea_Revision_PagosList =
                    //          entities.SGI_Tarea_Revision_Pagos.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Revision_Pagos.RemoveRange(SGI_Tarea_Revision_PagosList);
                    //          #endregion

                    //          #region SGI_Tarea_Revision_Tecnica_Legal
                    //          List<SGI_Tarea_Revision_Tecnica_Legal> SGI_Tarea_Revision_Tecnica_LegalList =
                    //          entities.SGI_Tarea_Revision_Tecnica_Legal.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Revision_Tecnica_Legal.RemoveRange(SGI_Tarea_Revision_Tecnica_LegalList);
                    //          #endregion

                    //          #region SGI_Tarea_Validar_Zonificacion
                    //          List<SGI_Tarea_Validar_Zonificacion> SGI_Tarea_Validar_ZonificacionList =
                    //          entities.SGI_Tarea_Validar_Zonificacion.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Validar_Zonificacion.RemoveRange(SGI_Tarea_Validar_ZonificacionList);
                    //          #endregion

                    //          #region SGI_Tarea_Verificacion_AVH
                    //          List<SGI_Tarea_Verificacion_AVH> SGI_Tarea_Verificacion_AVHList =
                    //          entities.SGI_Tarea_Verificacion_AVH.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Verificacion_AVH.RemoveRange(SGI_Tarea_Verificacion_AVHList);
                    //          #endregion

                    //          #region SGI_Tarea_Visado
                    //          List<SGI_Tarea_Visado> SGI_Tarea_VisadoList =
                    //          entities.SGI_Tarea_Visado.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tarea_Visado.RemoveRange(SGI_Tarea_VisadoList);
                    //          #endregion

                    //          #region SGI_Tramites_Tareas_CPADRON
                    //          List<SGI_Tramites_Tareas_CPADRON> SGI_Tramites_Tareas_CPADRONList =
                    //          entities.SGI_Tramites_Tareas_CPADRON.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tramites_Tareas_CPADRON.RemoveRange(SGI_Tramites_Tareas_CPADRONList);
                    //          #endregion

                    //          #region SGI_Tramites_Tareas_Dispo_Considerando
                    //          List<SGI_Tramites_Tareas_Dispo_Considerando> SGI_Tramites_Tareas_Dispo_ConsiderandoList =
                    //          entities.SGI_Tramites_Tareas_Dispo_Considerando.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    //          entities.SGI_Tramites_Tareas_Dispo_Considerando.RemoveRange(SGI_Tramites_Tareas_Dispo_ConsiderandoList);
                    //          #endregion

                    try
                    {
                        entities.SGI_Tramites_Tareas.Remove(tramiteTarea);
                        entities.SaveChanges();
                        string script = "$('#frmEliminarLog').modal('show');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);

                    }
                    catch (Exception ex)
                    {
                        //ASOSA MENSAJE DE ERROR
                        ScriptManager sm = ScriptManager.GetCurrent(this);
                        string cadena = "No pudo borrarse el Tramite Tarea por restricciones con otras tablas";
                        string script = string.Format("alert('{0}');", cadena);
                        ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);


                    }

                }

                gridView.EditIndex = -1;
                this.CargarSolicitudConTareas();
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int idTramiteTarea = int.Parse(((Button)sender).ToolTip);
            Response.Redirect("~/Operaciones/TareasForm.aspx?idTramiteTarea=" + idTramiteTarea + "&idSolicitud=" + hdidSolicitud.Value);

        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridView grid = (GridView)gridView;




                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int id_tramitetarea = -1;
                    if (int.TryParse(DataBinder.Eval(e.Row.DataItem, "id_tramitetarea").ToString(), out id_tramitetarea))
                    {
                        DGHP_Entities entities = new DGHP_Entities();
                        //List<SGI_SADE_Procesos> SGI_SADE_ProcesosList = (from SADE_Procesos in entities.SGI_SADE_Procesos
                        //                                                 where SADE_Procesos.id_tramitetarea == id_tramitetarea

                        //                                                 select SADE_Procesos).ToList();
                        SGI_SADE_Procesos sGI_SADE_Procesos = (from SADE_Procesos in entities.SGI_SADE_Procesos
                                                               where SADE_Procesos.id_tramitetarea == id_tramitetarea
                                                               && SADE_Procesos.realizado_en_SADE == true
                                                               select SADE_Procesos).FirstOrDefault();
                        if (sGI_SADE_Procesos != null)
                        {
                            Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                            Button btnRemove = (Button)e.Row.FindControl("btnRemove");
                            btnRemove.Enabled = false;
                        }


                    }



                   ;





                }

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            int idSolicitud = int.Parse(hdidSolicitud.Value);
            string hAB_tRANSF = hdHAB_TRANSF.Value;
            var id_circuito = 0;
            if (tareasDeLaSolicitud.Count() >= 1)
            {
                id_circuito = tareasDeLaSolicitud.LastOrDefault().ENG_Tareas.ENG_Circuitos.id_circuito;
            }
            Response.Redirect("~/Operaciones/TareasForm.aspx?idTramiteTarea=0" + "&idSolicitud=" + idSolicitud + "&hAB_tRANSF=" + hAB_tRANSF + "&id_circuito=" + id_circuito);
        }


        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, string.Empty, url, txtObservacionesSolicitante.Text, "D");

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, string.Empty, url, string.Empty, "D");


        }
    }
}