using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Operaciones
{
    public partial class AdministrarTareasDeUnaSolicitud : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string idSolicitudStr = (Request.QueryString["idSolicitud"] == null) ? "" : Request.QueryString["idSolicitud"].ToString();
                txtBuscarSolicitud.Text = idSolicitudStr;
                CargarSolicitudConTareas();
            }
        }
      
        public void CargarSolicitudConTareas()
        {
            int idSolicitud;
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out idSolicitud);

            if (couldParse)
            {
                DGHP_Entities entities = new DGHP_Entities();

                List<SGI_Tramites_Tareas> tareasDeLaSolicitud = (from tareas in entities.SGI_Tramites_Tareas
                                                                 join ttHab in entities.SGI_Tramites_Tareas_HAB
                                                                 on tareas.id_tramitetarea equals ttHab.id_tramitetarea
                                                                 where ttHab.id_solicitud == idSolicitud
                                                                 select tareas).ToList();

                if(tareasDeLaSolicitud.Count<1)
                {
                    tareasDeLaSolicitud = (from tareas in entities.SGI_Tramites_Tareas
                                           join tTransf in entities.SGI_Tramites_Tareas_TRANSF
                                           on tareas.id_tramitetarea equals tTransf.id_tramitetarea
                                           where tTransf.id_solicitud == idSolicitud
                                           select tareas).ToList();
                    hdHAB_TRANSF.Value = "T";
                }
                else
                {
                    hdHAB_TRANSF.Value = "H";
                }

                if (tareasDeLaSolicitud.Count < 1)
                {
                    hdHAB_TRANSF.Value = "";
                    btnNuevo.Enabled = false;//SI NO HAY REG TRANSF NI HAB ESCONTO BOTON NUEVO
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

                gridViewTareas.DataSource = tareasDeLaSolicitud;
                gridViewTareas.DataBind();
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
                    #region SGI_Tarea_Documentos_Adjuntos
                    List<SGI_Tarea_Documentos_Adjuntos> SGI_Tarea_Documentos_AdjuntosList =
                       entities.SGI_Tarea_Documentos_Adjuntos.Where(tda => tda.id_tramitetarea == idTramiteTarea).ToList();
                    entities.SGI_Tarea_Documentos_Adjuntos.RemoveRange(SGI_Tarea_Documentos_AdjuntosList);

                    #endregion

                    #region SGI_Tarea_Generar_Expediente_Procesos
                    List<SGI_Tarea_Generar_Expediente_Procesos> SGI_Tarea_Generar_Expediente_ProcesosList =
                      entities.SGI_Tarea_Generar_Expediente_Procesos.Where(tgep => tgep.id_tramitetarea == idTramiteTarea).ToList();

                    entities.SGI_Tarea_Generar_Expediente_Procesos.RemoveRange(SGI_Tarea_Generar_Expediente_ProcesosList);

                    #endregion

                    #region SGI_SADE_Procesos
                    List<SGI_SADE_Procesos> SGI_SADE_ProcesosList =
                   entities.SGI_SADE_Procesos.Where(sp => sp.id_tramitetarea == idTramiteTarea).ToList();

                    entities.SGI_SADE_Procesos.RemoveRange(SGI_SADE_ProcesosList);

                    #endregion

                    #region SGI_Tramites_Tareas_HAB
                    List<SGI_Tramites_Tareas_HAB> SGI_Tramites_Tareas_HABList =
                      entities.SGI_Tramites_Tareas_HAB.Where(tth => tth.id_tramitetarea == idTramiteTarea).ToList();

                    entities.SGI_Tramites_Tareas_HAB.RemoveRange(SGI_Tramites_Tareas_HABList);
                    #endregion

                    #region SGI_Tarea_Aprobado
                    List<SGI_Tarea_Aprobado> SGI_Tarea_AprobadoList =
                      entities.SGI_Tarea_Aprobado.Where(ta => ta.id_tramitetarea == idTramiteTarea).ToList();

                    entities.SGI_Tarea_Aprobado.RemoveRange(SGI_Tarea_AprobadoList);
                    #endregion

                    #region SGI_Tarea_Calificar
                    List<SGI_Tarea_Calificar> SGI_Tarea_CalificarList =
                 entities.SGI_Tarea_Calificar.Where(tc => tc.id_tramitetarea == idTramiteTarea).ToList();

                    entities.SGI_Tarea_Calificar.RemoveRange(SGI_Tarea_CalificarList);
                    #endregion

                    #region SGI_Tarea_Revision_DGHP
                          List<SGI_Tarea_Revision_DGHP> SGI_Tarea_Revision_DGHPList =
                entities.SGI_Tarea_Revision_DGHP.Where(tr => tr.id_tramitetarea == idTramiteTarea).ToList();

                    entities.SGI_Tarea_Revision_DGHP.RemoveRange(SGI_Tarea_Revision_DGHPList);
                    #endregion

                    #region SGI_Tarea_Entregar_Tramite
                    List<SGI_Tarea_Entregar_Tramite> SGI_Tarea_Entregar_TramiteList =
             entities.SGI_Tarea_Entregar_Tramite.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

                    entities.SGI_Tarea_Entregar_Tramite.RemoveRange(SGI_Tarea_Entregar_TramiteList);
                    #endregion




                    entities.SGI_Tramites_Tareas.Remove(tramiteTarea);

                    entities.SaveChanges();
                }
            }

            gridViewTareas.EditIndex = -1;
            this.CargarSolicitudConTareas();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int idTramiteTarea = int.Parse(((Button)sender).ToolTip);
            Response.Redirect("~/Operaciones/TareasForm.aspx?idTramiteTarea=" + idTramiteTarea + "&idSolicitud=" + hdidSolicitud.Value);

        }

        protected void gridViewTareas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridView grid = (GridView)gridViewTareas;
              
                


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
                            btnRemove.Enabled= false;
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
            
            Response.Redirect("~/Operaciones/TareasForm.aspx?idTramiteTarea=0" + "&idSolicitud=" + idSolicitud + "&hAB_tRANSF=" + hAB_tRANSF); 
        }
    }
}