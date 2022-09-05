using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SGI;
using SGI.Model;

namespace SGI.GestionTramite.Controls
{
    public partial class ucListaObservacionesAnteriores : System.Web.UI.UserControl
    {

        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void LoadData(int id_solicitud, int id_tramitetarea, int id_tarea)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea, id_tarea);
        }

        public void LoadData(int id_grupotramite, int id_solicitud, int id_tramitetarea, int id_tarea)
        {
            List<ObservacionAnteriores> lista_observ = ObservacionAnteriores.GetTareaObservacion(id_grupotramite,
                                id_solicitud, id_tramitetarea, TramiteTareaAnteriores.Dependencias_Tarea_ObservacionesV1(id_tarea));
            grdObservTareasAnteriores.DataSource = lista_observ;
            grdObservTareasAnteriores.DataBind();

            pnlObservAnterior.Visible = (grdObservTareasAnteriores.Rows.Count > 0);

 
        }

        protected void grdObservTareasAnteriores_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObservacionAnteriores observ  = (ObservacionAnteriores)e.Row.DataItem;
                DataList dl_observ = (DataList)e.Row.FindControl("dl_observ");

                dl_observ.DataSource = observ.Item;;
                dl_observ.DataBind();
            }

        }

        #endregion

        #region attributos
        private string _titulo = "";
        public string Titulo
        {
            get
            {
                return _titulo;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _titulo = value;
                else
                    _titulo = "Observaciones Tareas Anteriores";
                tituloControl.Text = _titulo;
            }
        }

        private bool _collapse;
        public bool Collapse
        {
            get
            {
                return _collapse;
            }
            set
            {
                _collapse = value;
                hid_loa_collapse.Value = _collapse.ToString().ToLower();

            }
        }

        #endregion

        #region entity

        private DGHP_Entities db = null;
        private AGC_FilesEntities dbFiles = null;

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
                this.db.Dispose();
        }

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
                this.dbFiles.Dispose();
        }

        #endregion
    }





}

public class TramiteTareaAnteriores
{
    public int id_tramitetarea { get; set; }
    public int id_tarea { get; set; }
    public string Nombre_tarea { get; set; }

    public static List<TramiteTareaAnteriores> BuscarUltimoTramiteTareaRevisionDGHyP(int id_grupotramite, int id_solicitud, int id_tramitetarea, int[] tareas)
    {
        DGHP_Entities db = new DGHP_Entities();

        List<TramiteTareaAnteriores> list_tramite_tarea;

        //buscar todas las tareas anteriores a la enviada por paramettro
        //las tareas futuras no mostrarlas porque puede estar 
        //consultando una tarea historica
        var q =
            (
                from tt in db.SGI_Tramites_Tareas
                join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                where tt_hab.id_solicitud == id_solicitud && tt.id_tramitetarea <= id_tramitetarea
                select new TramiteTareaAnteriores
                {
                    id_tarea = tarea.id_tarea,
                    Nombre_tarea = tarea.nombre_tarea,
                    id_tramitetarea = tt.id_tramitetarea
                }

            );
        if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
        {
            q =
                        (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_cp in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                        join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                        where tt_cp.id_cpadron == id_solicitud && tt.id_tramitetarea < id_tramitetarea
                        select new TramiteTareaAnteriores
                        {
                            id_tarea = tarea.id_tarea,
                            Nombre_tarea = tarea.nombre_tarea,
                            id_tramitetarea = tt.id_tramitetarea
                        }

                        );
        }
        else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
        {
            q =
                        (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_cp in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                        join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                        where tt_cp.id_solicitud == id_solicitud && tt.id_tramitetarea < id_tramitetarea
                        select new TramiteTareaAnteriores
                        {
                            id_tarea = tarea.id_tarea,
                            Nombre_tarea = tarea.nombre_tarea,
                            id_tramitetarea = tt.id_tramitetarea
                        }

                        );
        }
        if (tareas != null && tareas.Length > 0)
        {
            q =
                (
                    from datos in q
                    join lista_tareas in tareas.ToList() on datos.id_tarea equals lista_tareas
                    select datos
                );
        }


        list_tramite_tarea = q.ToList().OrderByDescending(x => x.id_tramitetarea).ToList();

        db.Dispose();

        return list_tramite_tarea;

    }

    public static List<TramiteTareaAnteriores> BuscarUltimoTramiteTareaPorTarea(int id_grupotramite, int id_solicitud, int id_tramitetarea, int[] tareas)
    {
        DGHP_Entities db = new DGHP_Entities();

        List<TramiteTareaAnteriores> list_tramite_tarea;

        //buscar todas las tareas anteriores a la enviada por paramettro
        //las tareas futuras no mostrarlas porque puede estar 
        //consultando una tarea historica
        var q =
            (
                from tt in db.SGI_Tramites_Tareas
                join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                where tt_hab.id_solicitud == id_solicitud && tt.id_tramitetarea < id_tramitetarea
                select new TramiteTareaAnteriores
                {
                    id_tarea = tarea.id_tarea,
                    Nombre_tarea = tarea.nombre_tarea,
                    id_tramitetarea = tt.id_tramitetarea
                }

            );
        if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
        {
            q =
                        (
                        from tt in db.SGI_Tramites_Tareas
                            join tt_cp in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                            join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                            where tt_cp.id_cpadron == id_solicitud && tt.id_tramitetarea < id_tramitetarea
                            select new TramiteTareaAnteriores
                            {
                                id_tarea = tarea.id_tarea,
                                Nombre_tarea = tarea.nombre_tarea,
                                id_tramitetarea = tt.id_tramitetarea
                            }

                        );
        }
        else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
        {
            q =
                        (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_cp in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                        join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                        where tt_cp.id_solicitud == id_solicitud && tt.id_tramitetarea < id_tramitetarea
                        select new TramiteTareaAnteriores
                        {
                            id_tarea = tarea.id_tarea,
                            Nombre_tarea = tarea.nombre_tarea,
                            id_tramitetarea = tt.id_tramitetarea
                        }

                        );
        }
        if (tareas != null && tareas.Length > 0)
        {
            q =
                (
                    from datos in q
                    join lista_tareas in tareas.ToList() on datos.id_tarea equals lista_tareas
                    select datos
                );
        }


        list_tramite_tarea = q.ToList().OrderByDescending(x => x.id_tramitetarea).ToList();

        db.Dispose();

        return list_tramite_tarea;

    }

    public static List<TramiteTareaAnteriores> BuscarTodosTareaTramitesAnteriores(int id_grupotramite, int id_solicitud, int id_tramitetarea, int[] tareas)
    {
        DGHP_Entities db = new DGHP_Entities();

        List<TramiteTareaAnteriores> list_tramite_tarea;
        //Para tramites de encomienda
        var q =
            (
                from tt in db.SGI_Tramites_Tareas
                join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                where tt_hab.id_solicitud == id_solicitud && tt.id_tramitetarea < id_tramitetarea
                select new TramiteTareaAnteriores
                {
                    id_tarea =  tt.id_tarea,
                    Nombre_tarea = tarea.nombre_tarea,
                    id_tramitetarea = tt.id_tramitetarea
                }
            );
        if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
        {
            q =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_cp in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                    join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                    where tt_cp.id_cpadron == id_solicitud && tt.id_tramitetarea < id_tramitetarea
                    select new TramiteTareaAnteriores
                    {
                        id_tarea = tt.id_tarea,
                        Nombre_tarea = tarea.nombre_tarea,
                        id_tramitetarea = tt.id_tramitetarea
                    }
                );

        }
        else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
        {
            q =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join tt_cp in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                    join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                    where tt_cp.id_solicitud == id_solicitud && tt.id_tramitetarea < id_tramitetarea
                    select new TramiteTareaAnteriores
                    {
                        id_tarea = tt.id_tarea,
                        Nombre_tarea = tarea.nombre_tarea,
                        id_tramitetarea = tt.id_tramitetarea
                    }
                );

        }

        if (tareas != null && tareas.Length > 0)
        {
            q =
                (
                    from datos in q
                    join lista_tareas in tareas.ToList() on datos.id_tarea equals lista_tareas
                    select datos
                );
        }

        q.OrderByDescending(x => x.id_tramitetarea);

        list_tramite_tarea = q.ToList();

        db.Dispose();

        return list_tramite_tarea;

    }

    public static int[] Dependencias_Tarea_Observaciones(Constants.ENG_Tareas tarea)
    {
        int[] tareas = null;

        switch (tarea)
        {
            #region Sin planos
            case Constants.ENG_Tareas.SSP_Encomienda_Digital:
                break;
            case Constants.ENG_Tareas.SSP_Certificacion_Encomienda:
                break;
            case Constants.ENG_Tareas.SSP_Minuta_Acta_Notarial:
                break;
            case Constants.ENG_Tareas.SSP_Certificado_Aptitud_Ambiental:
                break;
            case Constants.ENG_Tareas.SSP_Solicitud_Habilitacion:
                break;
            case Constants.ENG_Tareas.SSP_Revisión_Pagos_APRA:
                break;
            case Constants.ENG_Tareas.SSP_Asignar_Calificador:
                break;
            case Constants.ENG_Tareas.SSP_Calificar:
            case Constants.ENG_Tareas.SSP_Calificar_Nuevo:
                tareas = new int[9]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Tecnica_Legal,
                            (int)SGI.Constants.ENG_Tareas.SSP_Resultado_Inspector,
                            (int)SGI.Constants.ENG_Tareas.SSP_Validar_Zonificacion,
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Gerente_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_SubGerente:
            case Constants.ENG_Tareas.SSP_Revision_SubGerente_Nuevo:
                tareas = new int[6]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Gerente_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_Gerente:
            case Constants.ENG_Tareas.SSP_Revision_Gerente_Nuevo:
                tareas = new int[9]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Gerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_DGHP,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_DGHP_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_DGHP_2_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_Director:
                tareas = new int[3]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Gerente
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_DGHP:
            case Constants.ENG_Tareas.SSP_Revision_DGHP_Nuevo:
                tareas = new int[7]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Gerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Director
                        };
                break;
            case Constants.ENG_Tareas.SSP_Calificacion_Tecnica_Legal:
                break;
            case Constants.ENG_Tareas.SSP_Revision_Tecnica_Legal:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificacion_Tecnica_Legal
                        };
                break;
            case Constants.ENG_Tareas.SSP_Asignar_Inspector:
                break;
            case Constants.ENG_Tareas.SSP_Resultado_Inspector:
                break;
            case Constants.ENG_Tareas.SSP_Validar_Zonificacion:
                break;
            case Constants.ENG_Tareas.SSP_Revision_Pagos:
                break;
            case Constants.ENG_Tareas.SSP_Generar_Expediente:
                break;
            case Constants.ENG_Tareas.SSP_Entregar_Tramite:
                break;
            case Constants.ENG_Tareas.SSP_Enviar_PVH:
                break;
            case Constants.ENG_Tareas.SSP_Revision_Firma_Disposicion_Nuevo:
                tareas = new int[4]
                        {
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Gerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_DGHP_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SSP_Aprobados:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Firma_Disposicion
                        };
                break;
            case Constants.ENG_Tareas.SSP_Fin_Tramite:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Entregar_Tramite
                        };
                break;
            #endregion
            #region Con planos
            case Constants.ENG_Tareas.SCP_Asignar_Calificador:
                break;
            case Constants.ENG_Tareas.SCP_Calificar:
            case Constants.ENG_Tareas.SCP_Calificar_Nuevo:
                tareas = new int[9]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Tecnica_Legal,
                            (int)SGI.Constants.ENG_Tareas.SCP_Resultado_Inspector,
                            (int)SGI.Constants.ENG_Tareas.SCP_Validar_Zonificacion,
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Gerente_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_SubGerente:
            case Constants.ENG_Tareas.SCP_Revision_SubGerente_Nuevo:
                tareas = new int[6]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Gerente_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_Gerente:
            case Constants.ENG_Tareas.SCP_Revision_Gerente_Nuevo:
                tareas = new int[9]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Gerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_DGHP,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_DGHP_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_DGHP_2_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_Director:
                tareas = new int[3]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Gerente
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_DGHP:
            case Constants.ENG_Tareas.SCP_Revision_DGHP_Nuevo:
                tareas = new int[7]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Director,
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Gerente_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SCP_Calificacion_Tecnica_Legal:
                break;
            case Constants.ENG_Tareas.SCP_Revision_Tecnica_Legal:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificacion_Tecnica_Legal
                        };
                break;
            case Constants.ENG_Tareas.SCP_Asignar_Inspector:
                break;
            case Constants.ENG_Tareas.SCP_Resultado_Inspector:
                break;
            case Constants.ENG_Tareas.SCP_Validar_Zonificacion:
                break;
            case Constants.ENG_Tareas.SCP_Revision_Pagos:
                break;
            case Constants.ENG_Tareas.SCP_Generar_Expediente:
                break;
            case Constants.ENG_Tareas.SCP_Entregar_Tramite:
                break;
            case Constants.ENG_Tareas.SCP_Enviar_PVH:
                break;
            case Constants.ENG_Tareas.SCP_Revision_Firma_Disposicion_Nuevo:
                tareas = new int[4]
                        {
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Gerente_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_DGHP_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SCP_Aprobados:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Firma_Disposicion
                        };
                break;
            case Constants.ENG_Tareas.SCP_Fin_Tramite:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Entregar_Tramite
                        };
                break;
            #endregion
            #region Consulta al padron
            case Constants.ENG_Tareas.CP_Correccion_Solicitud:
                break;
            case Constants.ENG_Tareas.CP_Carga_Informacion:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.CP_Revision_SubGerente
                        };
                break;
            case Constants.ENG_Tareas.CP_Revision_SubGerente:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.CP_Carga_Informacion
                        };
                break;
            case Constants.ENG_Tareas.CP_Generar_Expediente:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.CP_Carga_Informacion
                        };
                break;
            case Constants.ENG_Tareas.CP_Fin_Tramite:
                tareas = new int[2]
                        { 
                            (int)SGI.Constants.ENG_Tareas.CP_Carga_Informacion,
                            (int)SGI.Constants.ENG_Tareas.CP_Generar_Expediente
                        };
                break;
            #endregion
            #region Transferencias
            case Constants.ENG_Tareas.TR_Asignar_Calificador:
                break;
            case Constants.ENG_Tareas.TR_Calificar:
                tareas = new int[4]
                        { 
                            (int)SGI.Constants.ENG_Tareas.TR_Calificar,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_2
                        };
                break;
            case Constants.ENG_Tareas.TR_Revision_SubGerente:
                tareas = new int[4]
                        { 
                            (int)SGI.Constants.ENG_Tareas.TR_Calificar,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_SubGerente
                        };
                break;
            case Constants.ENG_Tareas.TR_Revision_Gerente_1:
                tareas = new int[4]
                        { 
                            (int)SGI.Constants.ENG_Tareas.TR_Calificar,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_SubGerente
                        };
                break;
            case Constants.ENG_Tareas.TR_Revision_Gerente_2:
                tareas = new int[7]
                        { 
                            (int)SGI.Constants.ENG_Tareas.TR_Calificar,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_SubGerente
                        };
                break;
            case Constants.ENG_Tareas.TR_Revision_DGHP:
                tareas = new int[7]
                        { 
                            (int)SGI.Constants.ENG_Tareas.TR_Calificar,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_SubGerente
                        };
                break;
            case Constants.ENG_Tareas.TR_Dictamen_Asignar_Profesional:
                break;
            case Constants.ENG_Tareas.TR_Dictamen_Revisar_Tramite:
                tareas = new int[7]
                        { 
                            (int)SGI.Constants.ENG_Tareas.TR_Calificar,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_SubGerente,
                        };
                break;
            case Constants.ENG_Tareas.TR_Dictamen_Revision_SubGerente:
                tareas = new int[7]
                        { 
                            (int)SGI.Constants.ENG_Tareas.TR_Calificar,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_SubGerente,
                        };
                break;
            case Constants.ENG_Tareas.TR_Dictamen_Revision_Gerente:
                tareas = new int[7]
                        { 
                            (int)SGI.Constants.ENG_Tareas.TR_Calificar,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_SubGerente,
                        };
                break;
            case Constants.ENG_Tareas.TR_Dictamen_GEDO:
                break;
            case Constants.ENG_Tareas.TR_Revision_Pagos:
                break;
            case Constants.ENG_Tareas.TR_Generar_Expediente:
                break;
            case Constants.ENG_Tareas.TR_Entregar_Tramite:
                break;
            case Constants.ENG_Tareas.TR_Aprobados:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.TR_Revision_Firma_Disposicion
                        };
                break;
            case Constants.ENG_Tareas.TR_Fin_Tramite:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.TR_Entregar_Tramite
                        };
                break;
            #endregion
            #region especiales
            case Constants.ENG_Tareas.ESP_Asignar_Calificador:
                break;
            case Constants.ENG_Tareas.ESP_Calificar_1:
            case Constants.ENG_Tareas.ESP_Calificar_1_Nuevo:
                tareas = new int[11]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESP_Calificar_2:
            case Constants.ENG_Tareas.ESP_Calificar_2_Nuevo:
                tareas = new int[12]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Verificacion_AVH,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESP_Revision_SubGerente:
            case Constants.ENG_Tareas.ESP_Revision_SubGerente_1_Nuevo:
            case Constants.ENG_Tareas.ESP_Revision_SubGerente_2_Nuevo:
            case Constants.ENG_Tareas.ESP_Dictamen_Realizar_Nuevo:
                tareas = new int[11]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_2_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESP_Revision_Gerente_1:
            case Constants.ENG_Tareas.ESP_Revision_Gerente_2:
            case Constants.ENG_Tareas.ESP_Revision_Gerente_1_Nuevo:
            case Constants.ENG_Tareas.ESP_Revision_Gerente_2_Nuevo:
                tareas = new int[18]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_DGHP,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_DGHP_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_DGHP_2_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESP_Revision_DGHP:
            case Constants.ENG_Tareas.ESP_Revision_DGHP_Nuevo:
                tareas = new int[16]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Realizar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_Nuevo

                        };
                break;
            case Constants.ENG_Tareas.ESP_Dictamen_Asignar_Profesional:
                break;
            case Constants.ENG_Tareas.ESP_Dictamen_Revisar_Tramite:
                break;
            case Constants.ENG_Tareas.ESP_Dictamen_Revision_SubGerente:
                tareas = new int[8]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_SubGerente,
                        };
                break;
            case Constants.ENG_Tareas.ESP_Dictamen_Revision_Gerente:
                tareas = new int[8]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_SubGerente,
                        };
                break;
            case Constants.ENG_Tareas.ESP_Dictamen_Revision_Nuevo:
                tareas = new int[8]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Realizar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESP_Dictamen_GEDO:
                break;
            case Constants.ENG_Tareas.ESP_Revision_Pagos:
                break;
            case Constants.ENG_Tareas.ESP_Generar_Expediente:
                break;
            case Constants.ENG_Tareas.ESP_Entregar_Tramite:
                break;
            case Constants.ENG_Tareas.ESP_Aprobados:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Firma_Disposicion
                        };
                break;
            case Constants.ENG_Tareas.ESP_Revision_Firma_Disposicion:
            case Constants.ENG_Tareas.ESP_Revision_Firma_Disposicion_Nuevo:
                tareas = new int[12]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_SubGerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_Gerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Realizar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESP_Revision_DGHP_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESP_Fin_Tramite:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESP_Entregar_Tramite
                        };
                break;
            #endregion
            #region ESPARCIEMIENTO
            case Constants.ENG_Tareas.ESPAR_Asignar_Calificador:
                break;
            case Constants.ENG_Tareas.ESPAR_Calificar_1:
                tareas = new int[5]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_2
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Calificar_2:
                tareas = new int[6]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Verificacion_AVH
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Revision_SubGerente:
                tareas = new int[5]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Revision_Gerente_1:
            case Constants.ENG_Tareas.ESPAR_Revision_Gerente_1_Nuevo:
                tareas = new int[14]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_DGHP,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_DGHP_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_DGHP_2_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Revision_Gerente_2:
            case Constants.ENG_Tareas.ESPAR_Revision_Gerente_2_Nuevo:
                tareas = new int[17]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_DGHP,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_DGHP_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_DGHP_2_Nuevo

                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Revision_DGHP:
            case Constants.ENG_Tareas.ESPAR_Revision_DGHP_Nuevo:
                tareas = new int[15]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Realizar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Dictamen_Asignar_Profesional:
                break;
            case Constants.ENG_Tareas.ESPAR_Dictamen_Revisar_Tramite:
                break;
            case Constants.ENG_Tareas.ESPAR_Dictamen_Revision_SubGerente:
                tareas = new int[8]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_SubGerente,
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Gerente:
                tareas = new int[8]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_2,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revisar_Tramite,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_SubGerente,
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Nuevo:
                tareas = new int[8]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Realizar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Dictamen_GEDO:
                break;
            case Constants.ENG_Tareas.ESPAR_Revision_Pagos:
                break;
            case Constants.ENG_Tareas.ESPAR_Generar_Expediente:
                break;
            case Constants.ENG_Tareas.ESPAR_Entregar_Tramite:
                break;
            case Constants.ENG_Tareas.ESPAR_Revision_Firma_Disposicion:
            case Constants.ENG_Tareas.ESPAR_Revision_Firma_Disposicion_Nuevo:
                tareas = new int[9]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_SubGerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_1_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Gerente_2_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Realizar_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Nuevo,
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_DGHP_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Aprobados:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Revision_Firma_Disposicion
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Fin_Tramite:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Entregar_Tramite
                        };
                break;
            #endregion
            default:
                break;
        }

        return tareas;
    }
    
    public static int[] Dependencias_Tarea_ObservacionesV1(int id_tarea)
    {
        DGHP_Entities db = new DGHP_Entities();
        var tarea = db.ENG_Tareas.Where(x => x.id_tarea == id_tarea).First();
        string tipo_tarea = tarea.cod_tarea.ToString();
        tipo_tarea = tipo_tarea.Substring(tipo_tarea.Length - 2);
        string[] tipos_tareas = null;
        switch (tipo_tarea)
        {
            case Constants.ENG_Tipos_Tareas.Calificar:
            case Constants.ENG_Tipos_Tareas.Calificar2:
            case Constants.ENG_Tipos_Tareas.Calificar3:
                tipos_tareas = new string[11]
                        {
                            Constants.ENG_Tipos_Tareas.Calificar,
                            Constants.ENG_Tipos_Tareas.Calificar2,
                            Constants.ENG_Tipos_Tareas.Calificar3,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente2,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente3,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente2,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente3,
                            Constants.ENG_Tipos_Tareas.Revision_DGHyP,
                            Constants.ENG_Tipos_Tareas.Verificacion_AVH
                        };
                break;
            case Constants.ENG_Tipos_Tareas.Revision_SubGerente:
            case Constants.ENG_Tipos_Tareas.Revision_SubGerente2:
                tipos_tareas = new string[13]
                        {
                            Constants.ENG_Tipos_Tareas.Calificar,
                            Constants.ENG_Tipos_Tareas.Calificar2,
                            Constants.ENG_Tipos_Tareas.Calificar3,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente2,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente3,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente2,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente3,
                            Constants.ENG_Tipos_Tareas.Dictamen_Realizar,
                            Constants.ENG_Tipos_Tareas.Dictamen_Revision,
                            Constants.ENG_Tipos_Tareas_Transf.Revision_Gerente_CP,
                            Constants.ENG_Tipos_Tareas_Transf.Control_Informe

                        };
                break;
            case Constants.ENG_Tipos_Tareas.Revision_Gerente:
            case Constants.ENG_Tipos_Tareas.Revision_Gerente2:
                tipos_tareas = new string[14]
                        {
                            Constants.ENG_Tipos_Tareas.Calificar,
                            Constants.ENG_Tipos_Tareas.Calificar2,
                            Constants.ENG_Tipos_Tareas.Calificar3,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente2,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente3,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente2,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente3,
                            Constants.ENG_Tipos_Tareas.Revision_DGHyP,
                            Constants.ENG_Tipos_Tareas.Dictamen_Realizar,
                            Constants.ENG_Tipos_Tareas.Dictamen_Revision,
                            Constants.ENG_Tipos_Tareas_Transf.Revision_Gerente_CP,
                            Constants.ENG_Tipos_Tareas_Transf.Control_Informe

                        };
                break;
            case Constants.ENG_Tipos_Tareas.Revision_DGHyP:
            case Constants.ENG_Tipos_Tareas.Revision_DGHyP2:
                tipos_tareas = new string[12]
                        {
                            Constants.ENG_Tipos_Tareas.Calificar,
                            Constants.ENG_Tipos_Tareas.Calificar2,
                            Constants.ENG_Tipos_Tareas.Calificar3,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente2,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente3,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente2,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente3,
                            Constants.ENG_Tipos_Tareas.Revision_DGHyP,
                            Constants.ENG_Tipos_Tareas.Dictamen_Realizar,
                            Constants.ENG_Tipos_Tareas.Dictamen_Revision
                        };
                break;
            case Constants.ENG_Tipos_Tareas.Revision_Firma_Disposicion:
                tipos_tareas = new string[8]
                        {
                            Constants.ENG_Tipos_Tareas.Calificar,
                            Constants.ENG_Tipos_Tareas.Calificar2,
                            Constants.ENG_Tipos_Tareas.Calificar3,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente,
                            Constants.ENG_Tipos_Tareas.Revision_SubGerente2,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente,
                            Constants.ENG_Tipos_Tareas.Revision_Gerente2,
                            Constants.ENG_Tipos_Tareas.Revision_DGHyP
                        };
                break;
            case Constants.ENG_Tipos_Tareas.Fin_Tramite:
                tipos_tareas = new string[1]
                        {
                            Constants.ENG_Tipos_Tareas.Entregar_Tramite
                        };
                break;
            default:
                break;
        }
        List<int> tareas = new List<int>();
        int cod_tarea;
        List<int> t = new List<int>();
        if (tipos_tareas != null)
        {
            foreach(var cod in tipos_tareas)
            {
                cod_tarea = int.Parse( tarea.id_circuito.ToString() + cod);
                t = db.ENG_Tareas.Where(x => x.cod_tarea.ToString().Substring(x.cod_tarea.ToString().Length - 2, 2) == cod).Select(x => x.id_tarea).ToList();
                if (t != null)
                    tareas.AddRange(t);
            }
        }
        db.Dispose();
        return tareas.ToArray();
    }

    public static int[] Dependencias_Tarea_DocumentosAdjuntos(Constants.ENG_Tareas tarea)
    {
        int[] tareas = null;

        switch (tarea)
        {
            #region SSP
            case Constants.ENG_Tareas.SSP_Encomienda_Digital:
                break;
            case Constants.ENG_Tareas.SSP_Certificacion_Encomienda:
                break;
            case Constants.ENG_Tareas.SSP_Minuta_Acta_Notarial:
                break;
            case Constants.ENG_Tareas.SSP_Certificado_Aptitud_Ambiental:
                break;
            case Constants.ENG_Tareas.SSP_Solicitud_Habilitacion:
                break;
            case Constants.ENG_Tareas.SSP_Revisión_Pagos_APRA:
                break;
            case Constants.ENG_Tareas.SSP_Asignar_Calificador:
                break;
            case Constants.ENG_Tareas.SSP_Calificar:
                tareas = new int[3]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Tecnica_Legal,
                            (int)SGI.Constants.ENG_Tareas.SSP_Resultado_Inspector,
                            (int)SGI.Constants.ENG_Tareas.SSP_Validar_Zonificacion
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_SubGerente:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_Gerente:
                tareas = new int[2]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_Director:
                tareas = new int[3]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Gerente
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_DGHP:
                tareas = new int[4]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Director
                        };
                break;
            case Constants.ENG_Tareas.SSP_Calificacion_Tecnica_Legal:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificacion_Tecnica_Legal
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_Tecnica_Legal:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificacion_Tecnica_Legal
                        };
                break;
            case Constants.ENG_Tareas.SSP_Asignar_Inspector:
                break;
            case Constants.ENG_Tareas.SSP_Resultado_Inspector:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Resultado_Inspector
                        };
                break;
            case Constants.ENG_Tareas.SSP_Validar_Zonificacion:
                break;
            case Constants.ENG_Tareas.SSP_Revision_Pagos:
                break;
            case Constants.ENG_Tareas.SSP_Generar_Expediente:
                break;
            case Constants.ENG_Tareas.SSP_Entregar_Tramite:
                break;
            case Constants.ENG_Tareas.SSP_Enviar_PVH:
                break;
            #endregion
            #region SCP
            case Constants.ENG_Tareas.SCP_Asignar_Calificador:
                break;
            case Constants.ENG_Tareas.SCP_Calificar:
                tareas = new int[3]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Tecnica_Legal,
                            (int)SGI.Constants.ENG_Tareas.SCP_Resultado_Inspector,
                            (int)SGI.Constants.ENG_Tareas.SCP_Validar_Zonificacion
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_SubGerente:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_Gerente:
                tareas = new int[2]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_Director:
                tareas = new int[3]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Gerente
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_DGHP:
                tareas = new int[4]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_SubGerente,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Gerente,
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Director
                        };
                break;
            case Constants.ENG_Tareas.SCP_Calificacion_Tecnica_Legal:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificacion_Tecnica_Legal
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_Tecnica_Legal:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificacion_Tecnica_Legal
                        };
                break;
            case Constants.ENG_Tareas.SCP_Asignar_Inspector:
                break;
            case Constants.ENG_Tareas.SCP_Resultado_Inspector:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Resultado_Inspector
                        };
                break;
            case Constants.ENG_Tareas.SCP_Validar_Zonificacion:
                break;
            case Constants.ENG_Tareas.SCP_Revision_Pagos:
                break;
            case Constants.ENG_Tareas.SCP_Generar_Expediente:
                break;
            case Constants.ENG_Tareas.SCP_Entregar_Tramite:
                break;
            case Constants.ENG_Tareas.SCP_Enviar_PVH:
                break;
            #endregion
            #region CP
            case Constants.ENG_Tareas.CP_Correccion_Solicitud:
                break;
            case Constants.ENG_Tareas.CP_Carga_Informacion:
                break;
            case Constants.ENG_Tareas.CP_Generar_Expediente:
                break;
            case Constants.ENG_Tareas.CP_Fin_Tramite:
                break;
            #endregion

            #region ESP
            case Constants.ENG_Tareas.ESP_Calificar_2:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Verificacion_AVH
                        };
                break;
            case Constants.ENG_Tareas.ESP_Dictamen_Revision_Gerente:
            case Constants.ENG_Tareas.ESP_Dictamen_Revision_SubGerente:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revisar_Tramite
                        };
                break;
            #endregion
            #region ESPAR
            case Constants.ENG_Tareas.ESPAR_Calificar_2:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Verificacion_AVH
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Gerente:
            case Constants.ENG_Tareas.ESPAR_Dictamen_Revision_SubGerente:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revisar_Tramite
                        };
                break;
            #endregion
            #region TRANS
            case Constants.ENG_Tareas.TR_Dictamen_Revision_Gerente:
            case Constants.ENG_Tareas.TR_Dictamen_Revision_SubGerente:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revisar_Tramite
                        };
                break;
            #endregion

            #region SSP NUEVO
            #endregion
            #region SCP NUEVO
            #endregion
            #region ESP NUEVO
            case Constants.ENG_Tareas.ESP_Calificar_2_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Verificacion_AVH_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESP_Dictamen_Revision_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Realizar_Nuevo
                        };
                break;
            #endregion
            #region ESPAR NUEVO
            case Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Verificacion_AVH_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Realizar_Nuevo
                        };
                break;
            #endregion

            default:
                break;
        }

        return tareas;
    }

    public static int[] Dependencias_Tarea_ResultadosAnteriores(Constants.ENG_Tareas tarea)
    {
        int[] tareas = null;

        switch (tarea)
        {
            #region SSP
            case Constants.ENG_Tareas.SSP_Encomienda_Digital:
                break;
            case Constants.ENG_Tareas.SSP_Certificacion_Encomienda:
                break;
            case Constants.ENG_Tareas.SSP_Minuta_Acta_Notarial:
                break;
            case Constants.ENG_Tareas.SSP_Certificado_Aptitud_Ambiental:
                break;
            case Constants.ENG_Tareas.SSP_Solicitud_Habilitacion:
                break;
            case Constants.ENG_Tareas.SSP_Revisión_Pagos_APRA:
                break;
            case Constants.ENG_Tareas.SSP_Asignar_Calificador:
                break;
            case Constants.ENG_Tareas.SSP_Calificar:
                break;
            case Constants.ENG_Tareas.SSP_Revision_SubGerente:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_Gerente:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_Director:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_DGHP:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar
                        };
                break;
            case Constants.ENG_Tareas.SSP_Calificacion_Tecnica_Legal:
                break;
            case Constants.ENG_Tareas.SSP_Revision_Tecnica_Legal:
                break;
            case Constants.ENG_Tareas.SSP_Asignar_Inspector:
                break;
            case Constants.ENG_Tareas.SSP_Resultado_Inspector:
                break;
            case Constants.ENG_Tareas.SSP_Validar_Zonificacion:
                break;
            case Constants.ENG_Tareas.SSP_Revision_Pagos:
                break;
            case Constants.ENG_Tareas.SSP_Generar_Expediente:
                break;
            case Constants.ENG_Tareas.SSP_Entregar_Tramite:
                break;
            case Constants.ENG_Tareas.SSP_Enviar_PVH:
                break;
            case Constants.ENG_Tareas.SSP_Aprobados:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Revision_Firma_Disposicion
                        };
                break;
            case Constants.ENG_Tareas.SSP_Fin_Tramite:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SSP_Entregar_Tramite
                        };
                break;
            #endregion

            #region SCP
            case Constants.ENG_Tareas.SCP_Asignar_Calificador:
                break;
            case Constants.ENG_Tareas.SCP_Calificar:
                break;
            case Constants.ENG_Tareas.SCP_Revision_SubGerente:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_Gerente:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_Director:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_DGHP:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar
                        };
                break;
            case Constants.ENG_Tareas.SCP_Calificacion_Tecnica_Legal:
                break;
            case Constants.ENG_Tareas.SCP_Revision_Tecnica_Legal:
                break;
            case Constants.ENG_Tareas.SCP_Asignar_Inspector:
                break;
            case Constants.ENG_Tareas.SCP_Resultado_Inspector:
                break;
            case Constants.ENG_Tareas.SCP_Validar_Zonificacion:
                break;
            case Constants.ENG_Tareas.SCP_Revision_Pagos:
                break;
            case Constants.ENG_Tareas.SCP_Generar_Expediente:
                break;
            case Constants.ENG_Tareas.SCP_Entregar_Tramite:
                break;
            case Constants.ENG_Tareas.SCP_Enviar_PVH:
                break;
            case Constants.ENG_Tareas.SCP_Aprobados:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Revision_Firma_Disposicion
                        };
                break;
            case Constants.ENG_Tareas.SCP_Fin_Tramite:
                tareas = new int[1]
                        { 
                            (int)SGI.Constants.ENG_Tareas.SCP_Entregar_Tramite
                        };
                break;
            #endregion

            #region CP
            case Constants.ENG_Tareas.CP_Correccion_Solicitud:
                break;
            case Constants.ENG_Tareas.CP_Carga_Informacion:
                break;
            case Constants.ENG_Tareas.CP_Generar_Expediente:
                break;
            case Constants.ENG_Tareas.CP_Fin_Tramite:
                break;
            #endregion

            #region SSP nuevo
            case Constants.ENG_Tareas.SSP_Revision_SubGerente_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_Gerente_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SSP_Revision_DGHP_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.SSP_Calificar_Nuevo
                        };
                break;
            #endregion

            #region SCP nuevo
            case Constants.ENG_Tareas.SCP_Revision_SubGerente_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_Gerente_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.SCP_Revision_DGHP_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.SCP_Calificar_Nuevo
                        };
                break;
            #endregion
          
            #region Especial nuevo
            case Constants.ENG_Tareas.ESP_Revision_SubGerente_1_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESP_Revision_SubGerente_2_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESP_Revision_Gerente_1_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESP_Revision_Gerente_2_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESP_Revision_DGHP_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2_Nuevo
                        };
                break;
            #endregion

            #region Esparcimeinto nuevo
            case Constants.ENG_Tareas.ESPAR_Revision_SubGerente_1_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Revision_SubGerente_2_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Revision_Gerente_1_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Revision_Gerente_2_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo
                        };
                break;
            case Constants.ENG_Tareas.ESPAR_Revision_DGHP_Nuevo:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo
                        };
                break;
            #endregion

            #region TRM
            case Constants.ENG_Tareas.TRM_Revision_Gerente:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.TRM_Calificar
                        };
                break;
            case Constants.ENG_Tareas.TRM_Revision_SubGerente:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.TRM_Calificar
                        };
                break;
            case Constants.ENG_Tareas.TRM_Revision_DGHP:
                tareas = new int[1]
                        {
                            (int)SGI.Constants.ENG_Tareas.TRM_Calificar
                        };
                break;
            #endregion

            default:
                break;
        }

        return tareas;
    }
}

public class ObservacionAnteriores
{

    public ObservacionAnteriores()
    {
        this.Item = new List<Items>();
        
    }

    #region atributos

    public int ID { get; set; }
    public int id_tarea { get; set; }
    public string Nombre_tarea { get; set; }
    public int id_tramitetarea { get; set; }
    public string Observaciones { get; set; }
    public string NotasAdicionales { get; set; }
    public List<Items> Item { get; set; }
    public string UsuarioApeNom { get; set; }
    public DateTime Fecha { get; set; }

    #endregion

    #region metodos estaticos



    public static List<ObservacionAnteriores> GetTareaObservacion(int id_grupotramite, int id_solicitud, int id_tramitetarea, int[] tareas)
    {
        DGHP_Entities db = new DGHP_Entities();
        List<ObservacionAnteriores> list_observ = null;

        try
        {

            List<TramiteTareaAnteriores> lista_tramite = TramiteTareaAnteriores.BuscarUltimoTramiteTareaPorTarea(id_grupotramite, id_solicitud, id_tramitetarea, tareas);
            foreach (var item in lista_tramite)
            {
                if (list_observ == null)
                    list_observ = new List<ObservacionAnteriores>();

                ObservacionAnteriores observ = ObservacionAnteriores.GetTareaObservacion(id_grupotramite, item.id_tarea, item.id_tramitetarea);
                    //item.ultimo_id_tramitetarea);

                if (observ != null) // puede ser que no haya pasado por alguna tarea
                    list_observ.Add(observ);

            }

        }
        catch (Exception ex)
        {
            if (db != null)
                db.Dispose();
            throw ex;
        }

        return list_observ;

    }

    public static ObservacionAnteriores GetTareaObservacion(int id_grupotramite, int id_tarea, int id_tramitetarea)
    {
        bool ver_observ_vacias = false;
        ObservacionAnteriores tareaObserv = null;

        DGHP_Entities db = new DGHP_Entities();
        var ta = db.ENG_Tareas.Where(x => x.id_tarea == id_tarea).First();
        string tipo_tarea = ta.cod_tarea.ToString();
        tipo_tarea = tipo_tarea.Substring(tipo_tarea.Length - 2);

        try
        {
            switch (tipo_tarea)
            {
                #region Asignación de Calificador

                //9 	Asignación de Calificador			Asignar_Calificador.aspx	    SGI_Tarea_Asignar_Calificador
                case Constants.ENG_Tipos_Tareas.Asignacion_Calificador:
                case Constants.ENG_Tipos_Tareas.Asignacion_Calificador2:
                    tareaObserv =
                        (
                            from calificador in db.SGI_Tarea_Asignar_Calificador
                            join tramite in db.SGI_Tramites_Tareas on calificador.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on calificador.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on calificador.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where calificador.id_tramitetarea == id_tramitetarea
                            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(calificador.Observaciones))
                            orderby calificador.id_asignar_calificador descending
                            select new ObservacionAnteriores
                            {
                                ID = calificador.id_asignar_calificador,
                                Observaciones = calificador.Observaciones,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (calificador.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (calificador.LastUpdateDate.HasValue) ? (DateTime)calificador.LastUpdateDate : calificador.CreateDate
                            }
                        ).FirstOrDefault();

                    break;

                #endregion

                #region Calificar Trámite
                //10	Calificar Trámite					Calificar.aspx				    SGI_Tarea_Calificar
                case Constants.ENG_Tipos_Tareas.Calificar:
                case Constants.ENG_Tipos_Tareas.Calificar2:
                    var q_calificar =
                        (
                            from calificar in db.SGI_Tarea_Calificar
                            join tramite in db.SGI_Tramites_Tareas on calificar.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on calificar.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on calificar.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where calificar.id_tramitetarea == id_tramitetarea
                            && (
                                ver_observ_vacias || 
                                (   !ver_observ_vacias && 
                                    (   
                                        !string.IsNullOrEmpty(calificar.Observaciones) ||
                                        !string.IsNullOrEmpty(calificar.Observaciones_Internas) || 
                                        !string.IsNullOrEmpty(calificar.Observaciones_contribuyente) 
                                    ) 
                                )
                               )
                            orderby calificar.id_calificar descending
                            select new  //ObservacionAnteriores
                            {
                                ID = calificar.id_calificar,
                                Nombre_tarea = tarea.nombre_tarea,
                                Observaciones = calificar.Observaciones,
                                Observaciones_contribuyente = calificar.Observaciones_contribuyente,
                                Observaciones_Internas = calificar.Observaciones_Internas,
                                UsuarioApeNom = (calificar.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (calificar.LastUpdateDate.HasValue) ? (DateTime)calificar.LastUpdateDate : calificar.CreateDate
                            }
                        ).FirstOrDefault();

                    if (q_calificar != null)
                    {
                        tareaObserv = new ObservacionAnteriores();

                        tareaObserv.ID = q_calificar.ID;
                        tareaObserv.id_tarea = 0;
                        tareaObserv.Nombre_tarea = q_calificar.Nombre_tarea;
                        tareaObserv.id_tramitetarea = id_tramitetarea;
                        tareaObserv.Observaciones = ""; 
                        tareaObserv.UsuarioApeNom = q_calificar.UsuarioApeNom;
                        tareaObserv.Fecha = q_calificar.Fecha;

                        if (!string.IsNullOrEmpty(q_calificar.Observaciones))
                            tareaObserv.Item.Add(new Items(q_calificar.Observaciones + "<br/>", "Notas adicionales para la disposición:"));

                        if (!string.IsNullOrEmpty(q_calificar.Observaciones_contribuyente))
                            tareaObserv.Item.Add(new Items(q_calificar.Observaciones_contribuyente + "<br/>", "Observaciones al Contribuyente:"));

                        if (!string.IsNullOrEmpty(q_calificar.Observaciones_Internas))
                            tareaObserv.Item.Add(new Items(q_calificar.Observaciones_Internas, "Observaciones internas:"));

                    }

                    break;

                #endregion

                #region Revisión Sub-Gerente
                //11	Revisión Sub-Gerente				Revision_SubGerente.aspx	    SGI_Tarea_Revision_SubGerente
                case Constants.ENG_Tipos_Tareas.Revision_SubGerente:
                case Constants.ENG_Tipos_Tareas.Revision_SubGerente2:
                    var q_subgerente =
                        (
                            from subGerente in db.SGI_Tarea_Revision_SubGerente
                            join tramite in db.SGI_Tramites_Tareas on subGerente.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on subGerente.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on subGerente.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where subGerente.id_tramitetarea == id_tramitetarea
                                && (
                                    ver_observ_vacias || 
                                    !ver_observ_vacias && 
                                        (
                                            !string.IsNullOrEmpty(subGerente.Observaciones) ||
                                            !string.IsNullOrEmpty(subGerente.observacion_plancheta)
                                        )
                                   )
                            orderby subGerente.id_revision_subGerente descending
                            select new  // ObservacionAnteriores
                            {
                                ID = subGerente.id_revision_subGerente,
                                Observaciones = subGerente.Observaciones,
                                observacion_plancheta = subGerente.observacion_plancheta,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (subGerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (subGerente.LastUpdateDate.HasValue) ? (DateTime)subGerente.LastUpdateDate : subGerente.CreateDate
                            }
                        ).FirstOrDefault();

                    if (q_subgerente != null)
                    {
                        tareaObserv = new ObservacionAnteriores();

                        tareaObserv.ID = q_subgerente.ID;
                        tareaObserv.id_tarea = 0;
                        tareaObserv.Nombre_tarea = q_subgerente.Nombre_tarea;
                        tareaObserv.id_tramitetarea = id_tramitetarea;
                        tareaObserv.Observaciones = "";
                        tareaObserv.UsuarioApeNom = q_subgerente.UsuarioApeNom;
                        tareaObserv.Fecha = q_subgerente.Fecha;

                        if (!string.IsNullOrEmpty(q_subgerente.Observaciones))
                            tareaObserv.Item.Add(new Items(q_subgerente.Observaciones + "<br/>", "Observaciones Internas:"));

                        if (!string.IsNullOrEmpty(q_subgerente.observacion_plancheta))
                            tareaObserv.Item.Add(new Items(q_subgerente.observacion_plancheta, "Notas adicionales para la disposición:"));

                    }

                    break;

                #endregion

                #region Revisión Gerente
                //12	Revisión Gerente					Revision_Gerente.aspx		    SGI_Tarea_Revision_Gerente
                case Constants.ENG_Tipos_Tareas.Revision_Gerente:
                case Constants.ENG_Tipos_Tareas.Revision_Gerente2:
                    var q_gerente =

                        (
                            from gerente in db.SGI_Tarea_Revision_Gerente
                            join tramite in db.SGI_Tramites_Tareas on gerente.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on gerente.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on gerente.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where gerente.id_tramitetarea == id_tramitetarea
                                && (
                                    ver_observ_vacias ||
                                    !ver_observ_vacias &&
                                        (
                                            !string.IsNullOrEmpty(gerente.Observaciones) ||
                                            !string.IsNullOrEmpty(gerente.observacion_plancheta)
                                        )
                                   )

                            orderby gerente.id_revision_gerente descending
                            select new 
                            {
                                ID = gerente.id_revision_gerente,
                                Observaciones = gerente.Observaciones,
                                observacion_plancheta = gerente.observacion_plancheta,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (gerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (gerente.LastUpdateDate.HasValue) ? (DateTime)gerente.LastUpdateDate : gerente.CreateDate
                            }
                        ).FirstOrDefault();

                    if (q_gerente != null)
                    {
                        tareaObserv = new ObservacionAnteriores();

                        tareaObserv.ID = q_gerente.ID;
                        tareaObserv.id_tarea = 0;
                        tareaObserv.Nombre_tarea = q_gerente.Nombre_tarea;
                        tareaObserv.id_tramitetarea = id_tramitetarea;
                        tareaObserv.Observaciones = "";
                        tareaObserv.UsuarioApeNom = q_gerente.UsuarioApeNom;
                        tareaObserv.Fecha = q_gerente.Fecha;

                        if (!string.IsNullOrEmpty(q_gerente.Observaciones))
                            tareaObserv.Item.Add(new Items(q_gerente.Observaciones + "<br/>", "Observaciones Internas:"));

                        if (!string.IsNullOrEmpty(q_gerente.observacion_plancheta))
                            tareaObserv.Item.Add(new Items(q_gerente.observacion_plancheta, "Notas adicionales para la disposición:"));

                    }

                    break;

                #endregion

                #region Revisión Director
                //// join tramite in db.SGI_Tramites_Tareas on tarea.id_tramitetarea equals tramite.id_tramitetarea
                //// join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //// Nombre_tarea = tarea.nombre_tarea,

                ////13	Revisión Director					Revision_Director.aspx			SGI_Tarea_Revision_Director
                //case (int)SGI.Constants.ENG_Tareas.SSP_Revision_Director:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Revision_Director:
                //    tareaObserv =
                //        (
                //            from director in db.SGI_Tarea_Revision_Director
                //            join tramite in db.SGI_Tramites_Tareas on director.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on director.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on director.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where director.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(director.Observaciones))
                //            orderby director.id_revision_director descending
                //            select new ObservacionAnteriores
                //            {
                //                ID = director.id_revision_director,
                //                Observaciones = director.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (director.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (director.LastUpdateDate.HasValue) ? (DateTime)director.LastUpdateDate : director.CreateDate
                //                //CreateDate = director.CreateDate,
                //                //CreateUser = director.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = director.LastUpdateDate,
                //                //LastUpdateUser = director.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Revisión DGHyP

                //14	Revisión DGHyP						Revision_DGHP.aspx				SGI_Tarea_Revision_DGHP
                case Constants.ENG_Tipos_Tareas.Revision_DGHyP:
                case Constants.ENG_Tipos_Tareas.Revision_DGHyP2:
                    tareaObserv =
                        (
                            from dghp in db.SGI_Tarea_Revision_DGHP
                            join tramite in db.SGI_Tramites_Tareas on dghp.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on dghp.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on dghp.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where dghp.id_tramitetarea == id_tramitetarea
                            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(dghp.Observaciones))
                            orderby dghp.id_revision_dghp descending
                            select new ObservacionAnteriores
                            {
                                ID = dghp.id_revision_dghp,
                                Observaciones = dghp.Observaciones,
                                NotasAdicionales = dghp.observacion_plancheta,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (dghp.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (dghp.LastUpdateDate.HasValue) ? (DateTime)dghp.LastUpdateDate : dghp.CreateDate
                            }
                        ).FirstOrDefault();

                    if (tareaObserv != null)
                    {
                        tareaObserv.Item.Add(new Items(tareaObserv.Observaciones + "<br/>", "Observaciones Internas:"));
                        tareaObserv.Item.Add(new Items(tareaObserv.NotasAdicionales + "<br/>", "Notas adicionales para la disposición:"));
                    }

                    break;

                #endregion

                #region Calificación Técnica y Legal
                ////15	Calificación Técnica y Legal		Calificacion_Tecnica_Legal.aspx	SGI_Tarea_Calificacion_Tecnica
                //case (int)SGI.Constants.ENG_Tareas.SSP_Calificacion_Tecnica_Legal:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Calificacion_Tecnica_Legal:
                //    tareaObserv =
                //        (
                //            from calificador_legal in db.SGI_Tarea_Calificacion_Tecnica_Legal
                //            join tramite in db.SGI_Tramites_Tareas on calificador_legal.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on calificador_legal.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on calificador_legal.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where calificador_legal.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(calificador_legal.Observaciones))
                //            orderby calificador_legal.id_cal_tec_leg descending
                //            select new ObservacionAnteriores
                //            {
                //                ID = calificador_legal.id_cal_tec_leg,
                //                Observaciones = calificador_legal.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (calificador_legal.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (calificador_legal.LastUpdateDate.HasValue) ? (DateTime)calificador_legal.LastUpdateDate : calificador_legal.CreateDate
                //                //CreateDate = calificador_legal.CreateDate,
                //                //CreateUser = calificador_legal.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = calificador_legal.LastUpdateDate,
                //                //LastUpdateUser = calificador_legal.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Revisión Técnica y Legal
                //16	Revisión Técnica y Legal			Revision_Tecnica_Legal.aspx		SGI_Tarea_Revision_Tecnica_Legal
                //case (int)SGI.Constants.ENG_Tareas.SSP_Revision_Tecnica_Legal:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Revision_Tecnica_Legal:
                //    tareaObserv =
                //        (
                //            from revision_legal in db.SGI_Tarea_Revision_Tecnica_Legal
                //            join tramite in db.SGI_Tramites_Tareas on revision_legal.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on revision_legal.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on revision_legal.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where revision_legal.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(revision_legal.Observaciones))
                //            orderby revision_legal.id_rev_tec_leg descending
                //            select new ObservacionAnteriores
                //            {
                //                ID = revision_legal.id_rev_tec_leg,
                //                Observaciones = revision_legal.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (revision_legal.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (revision_legal.LastUpdateDate.HasValue) ? (DateTime)revision_legal.LastUpdateDate : revision_legal.CreateDate
                //                //CreateDate = revision_legal.CreateDate,
                //                //CreateUser = revision_legal.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = revision_legal.LastUpdateDate,
                //                //LastUpdateUser = revision_legal.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Asignar Inspector
                //18	Asignar Inspector					Asignar_Inspector.aspx			SGI_Tarea_Asignar_Inspector
                //case (int)SGI.Constants.ENG_Tareas.SSP_Asignar_Inspector:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Asignar_Inspector:
                //    tareaObserv =
                //        (
                //            from inspector in db.SGI_Tarea_Asignar_Inspector
                //            join tramite in db.SGI_Tramites_Tareas on inspector.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on inspector.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on inspector.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where inspector.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(inspector.Observaciones))
                //            orderby inspector.id_asignar_inspector descending
                //            select new ObservacionAnteriores
                //            {
                //                ID = inspector.id_asignar_inspector,
                //                Observaciones = inspector.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (inspector.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (inspector.LastUpdateDate.HasValue) ? (DateTime)inspector.LastUpdateDate : inspector.CreateDate
                //                //CreateDate = inspector.CreateDate,
                //                //CreateUser = inspector.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = inspector.LastUpdateDate,
                //                //LastUpdateUser = inspector.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Informe Resultado Inspector
                //19	Informe Resultado Inspector			Resultado_Inspector.aspx		SGI_Tarea_Resultado_Inspector
                //case (int)SGI.Constants.ENG_Tareas.SSP_Resultado_Inspector:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Resultado_Inspector:
                //    tareaObserv =
                //        (
                //            from inspector in db.SGI_Tarea_Resultado_Inspector
                //            join tramite in db.SGI_Tramites_Tareas on inspector.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on inspector.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on inspector.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where inspector.id_tramitetarea == id_tramitetarea
                //            && (  ver_observ_vacias || !ver_observ_vacias  && !string.IsNullOrEmpty(inspector.Observaciones)  )
                //            orderby inspector.id_resultado_inspector descending
                //            select new ObservacionAnteriores
                //            {
                //                ID = inspector.id_resultado_inspector,
                //                Observaciones = inspector.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (inspector.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres: usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (inspector.LastUpdateDate.HasValue) ? (DateTime)inspector.LastUpdateDate : inspector.CreateDate
                //                //CreateDate = inspector.CreateDate,
                //                //CreateUser = inspector.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = inspector.LastUpdateDate,
                //                //LastUpdateUser = inspector.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Validar Zonificación
                //20	Validar Zonificación				Validar_Zonificacion.aspx		SGI_Tarea_Validar_Zonificacion
                //case (int)SGI.Constants.ENG_Tareas.SSP_Validar_Zonificacion:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Validar_Zonificacion:
                //    tareaObserv =
                //        (
                //            from zonificar in db.SGI_Tarea_Validar_Zonificacion
                //            join tramite in db.SGI_Tramites_Tareas on zonificar.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on zonificar.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on zonificar.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where zonificar.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(zonificar.Observaciones))
                //            orderby zonificar.id_validar_zonificacion descending
                //            select new ObservacionAnteriores
                //            {
                //                ID = zonificar.id_validar_zonificacion,
                //                Observaciones = zonificar.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (zonificar.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (zonificar.LastUpdateDate.HasValue) ? (DateTime)zonificar.LastUpdateDate : zonificar.CreateDate
                //                //CreateDate = zonificar.CreateDate,
                //                //CreateUser = zonificar.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = zonificar.LastUpdateDate,
                //                //LastUpdateUser = zonificar.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Revisión Pagos
                //21	Revisión Pagos						Revision_Pagos.aspx				SGI_Tarea_Revision_Pagos
                //case (int)SGI.Constants.ENG_Tareas.SSP_Revision_Pagos:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Revision_Pagos:
                //    tareaObserv =
                //        (
                //            from pagos in db.SGI_Tarea_Revision_Pagos
                //            join tramite in db.SGI_Tramites_Tareas on id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on pagos.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on pagos.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where pagos.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(pagos.Observaciones))
                //            orderby pagos.id_revision_pagos descending
                //            select new ObservacionAnteriores
                //            {
                //                ID = pagos.id_revision_pagos,
                //                Observaciones = pagos.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (pagos.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (pagos.LastUpdateDate.HasValue) ? (DateTime)pagos.LastUpdateDate : pagos.CreateDate
                //                //CreateDate = CreateDate,
                //                //CreateUser = CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = LastUpdateDate,
                //                //LastUpdateUser = LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Generar Expediente
                //22	Generar Expediente					Generar_Expediente.aspx			SGI_Tarea_Generar_Expediente
                case Constants.ENG_Tipos_Tareas.Generar_Expediente:
                    tareaObserv =
                        (
                            from expediente in db.SGI_Tarea_Generar_Expediente
                            join tramite in db.SGI_Tramites_Tareas on expediente.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on expediente.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on expediente.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where expediente.id_tramitetarea == id_tramitetarea
                            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(expediente.Observaciones))
                            orderby expediente.id_generar_expediente descending
                            select new ObservacionAnteriores
                            {
                                ID = expediente.id_generar_expediente,
                                Observaciones = expediente.Observaciones,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (expediente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (expediente.LastUpdateDate.HasValue) ? (DateTime)expediente.LastUpdateDate : expediente.CreateDate
                            }
                        ).FirstOrDefault();

                    break;

                #endregion

                #region Entregar Trámite
                //23	Entregar Trámite					Entregar_Tramite.aspx			SGI_Tarea_Entregar_Tramite
                case Constants.ENG_Tipos_Tareas.Entregar_Tramite:
                    tareaObserv =
                        (
                            from entregar in db.SGI_Tarea_Entregar_Tramite
                            join tramite in db.SGI_Tramites_Tareas on entregar.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on entregar.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on entregar.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where entregar.id_tramitetarea == id_tramitetarea
                            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(entregar.Observaciones))
                            orderby entregar.id_entregar_tramite descending
                            select new ObservacionAnteriores
                            {
                                ID = entregar.id_entregar_tramite,
                                Observaciones = entregar.Observaciones,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (entregar.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (entregar.LastUpdateDate.HasValue) ? (DateTime)entregar.LastUpdateDate : entregar.CreateDate
                            }
                        ).FirstOrDefault();

                    break;

                #endregion

                #region Enviar a PVH
                //24	Enviar a PVH						Enviar_PVH.aspx					SGI_Tarea_Enviar_PVH
                //case (int)SGI.Constants.ENG_Tareas.SSP_Enviar_PVH:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Enviar_PVH:
                //    tareaObserv =
                //        (
                //            from pvh in db.SGI_Tarea_Enviar_PVH
                //            join tramite in db.SGI_Tramites_Tareas on pvh.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on pvh.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on pvh.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where pvh.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(pvh.Observaciones))
                //            orderby pvh.id_enviar_pvh descending
                //            select new ObservacionAnteriores
                //            {
                //                ID = pvh.id_enviar_pvh,
                //                Observaciones = pvh.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (pvh.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (pvh.LastUpdateDate.HasValue) ? (DateTime)pvh.LastUpdateDate : pvh.CreateDate
                //                //CreateDate = pvh.CreateDate,
                //                //CreateUser = pvh.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = pvh.LastUpdateDate,
                //                //LastUpdateUser = pvh.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Carga infromacion
                //case (int)SGI.Constants.ENG_Tareas.CP_Carga_Informacion:
                //    var q_carga =
                //        (
                //            from carga in db.SGI_Tarea_Carga_Tramite
                //            join cpTarea in db.SGI_Tramites_Tareas on carga.id_tramitetarea equals cpTarea.id_tramitetarea
                //            join tarea in db.ENG_Tareas on cpTarea.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on carga.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on carga.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where carga.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias &&
                //                                                (
                //                        !string.IsNullOrEmpty(carga.Observaciones) ||
                //                        !string.IsNullOrEmpty(carga.observaciones_contribuyente)
                //                    )

                //            )
                //            orderby carga.id_carga_tramite descending
                //            select new  //ObservacionAnteriores
                //            {
                //                ID = carga.id_carga_tramite,
                //                Nombre_tarea = tarea.nombre_tarea,
                //                Observaciones = carga.Observaciones,
                //                Observaciones_contribuyente = carga.observaciones_contribuyente,
                //                UsuarioApeNom = (carga.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (carga.LastUpdateDate.HasValue) ? (DateTime)carga.LastUpdateDate : carga.CreateDate
                //            }
                //        ).FirstOrDefault();

                //    if (q_carga != null)
                //    {
                //        tareaObserv = new ObservacionAnteriores();

                //        tareaObserv.ID = q_carga.ID;
                //        tareaObserv.id_tarea = 0;
                //        tareaObserv.Nombre_tarea = q_carga.Nombre_tarea;
                //        tareaObserv.id_tramitetarea = id_tramitetarea;
                //        tareaObserv.Observaciones = "";
                //        tareaObserv.UsuarioApeNom = q_carga.UsuarioApeNom;
                //        tareaObserv.Fecha = q_carga.Fecha;

                //        if (!string.IsNullOrEmpty(q_carga.Observaciones_contribuyente))
                //            tareaObserv.Item.Add(new Items(q_carga.Observaciones_contribuyente + "<br/>", "Observaciones al Contribuyente:"));

                //        if (!string.IsNullOrEmpty(q_carga.Observaciones))
                //            tareaObserv.Item.Add(new Items(q_carga.Observaciones, "Observaciones internas:"));

                //    }

                //    break;

                #endregion

                #region Fin de tramite
                case Constants.ENG_Tipos_Tareas.Fin_Tramite:
                    var q_finCP =
                        (
                            from fin in db.SGI_Tarea_Fin_Tramite
                            join tramite in db.SGI_Tramites_Tareas on fin.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on fin.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on fin.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where fin.id_tramitetarea == id_tramitetarea
                                && (
                                    ver_observ_vacias ||
                                    !ver_observ_vacias &&
                                        (
                                            !string.IsNullOrEmpty(fin.Observaciones)
                                        )
                                   )
                            orderby fin.id_Fin_Tramite descending
                            select new  // ObservacionAnteriores
                            {
                                ID = fin.id_Fin_Tramite,
                                Observaciones = fin.Observaciones,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (fin.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (fin.LastUpdateDate.HasValue) ? (DateTime)fin.LastUpdateDate : fin.CreateDate
                            }
                        ).FirstOrDefault();


                    if (q_finCP != null)
                    {
                        tareaObserv = new ObservacionAnteriores();

                        tareaObserv.ID = q_finCP.ID;
                        tareaObserv.id_tarea = 0;
                        tareaObserv.Nombre_tarea = q_finCP.Nombre_tarea;
                        tareaObserv.id_tramitetarea = id_tramitetarea;
                        tareaObserv.Observaciones = "";
                        tareaObserv.UsuarioApeNom = q_finCP.UsuarioApeNom;
                        tareaObserv.Fecha = q_finCP.Fecha;

                        if (!string.IsNullOrEmpty(q_finCP.Observaciones))
                            tareaObserv.Item.Add(new Items(q_finCP.Observaciones, "Observaciones Internas:"));

                    }
                    break;

                #endregion

                #region Dictamen Revisión Revisar_Tramite
                case Constants.ENG_Tipos_Tareas.Dictamen_Realizar:
                    var q_dic_Revisar_Tramite =
                        (
                            from subGerente in db.SGI_Tarea_Dictamen_Revisar_Tramite
                            join tramite in db.SGI_Tramites_Tareas on subGerente.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on subGerente.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on subGerente.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where subGerente.id_tramitetarea == id_tramitetarea
                                && (
                                    ver_observ_vacias ||
                                    !ver_observ_vacias &&
                                        (
                                            !string.IsNullOrEmpty(subGerente.Observaciones)
                                        )
                                   )
                            orderby subGerente.id_Dictamen_Revisar_Tramite descending
                            select new
                            {
                                ID = subGerente.id_Dictamen_Revisar_Tramite,
                                Observaciones = subGerente.Observaciones,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (subGerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (subGerente.LastUpdateDate.HasValue) ? (DateTime)subGerente.LastUpdateDate : subGerente.CreateDate
                            }
                        ).FirstOrDefault();


                    if (q_dic_Revisar_Tramite != null)
                    {
                        tareaObserv = new ObservacionAnteriores();

                        tareaObserv.ID = q_dic_Revisar_Tramite.ID;
                        tareaObserv.id_tarea = 0;
                        tareaObserv.Nombre_tarea = q_dic_Revisar_Tramite.Nombre_tarea;
                        tareaObserv.id_tramitetarea = id_tramitetarea;
                        tareaObserv.Observaciones = "";
                        tareaObserv.UsuarioApeNom = q_dic_Revisar_Tramite.UsuarioApeNom;
                        tareaObserv.Fecha = q_dic_Revisar_Tramite.Fecha;

                        if (!string.IsNullOrEmpty(q_dic_Revisar_Tramite.Observaciones))
                            tareaObserv.Item.Add(new Items(q_dic_Revisar_Tramite.Observaciones + "<br/>", "Observaciones Internas:"));

                    }

                    break;

                #endregion

                #region Dictamen Realizar Dictamen
                case Constants.ENG_Tipos_Tareas.Dictamen_Revision:
                    var q_dic_revision_dictamen =
                        (
                            from subGerente in db.SGI_Tarea_Dictamen_Revision
                            join tramite in db.SGI_Tramites_Tareas on subGerente.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on subGerente.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on subGerente.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where subGerente.id_tramitetarea == id_tramitetarea
                                && (
                                    ver_observ_vacias ||
                                    !ver_observ_vacias &&
                                        (
                                            !string.IsNullOrEmpty(subGerente.Observaciones)
                                        )
                                   )
                            orderby subGerente.id_dictamen_revision descending
                            select new
                            {
                                ID = subGerente.id_dictamen_revision,
                                Observaciones = subGerente.Observaciones,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (subGerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (subGerente.LastUpdateDate.HasValue) ? (DateTime)subGerente.LastUpdateDate : subGerente.CreateDate
                            }
                        ).FirstOrDefault();


                    if (q_dic_revision_dictamen != null)
                    {
                        tareaObserv = new ObservacionAnteriores();

                        tareaObserv.ID = q_dic_revision_dictamen.ID;
                        tareaObserv.id_tarea = 0;
                        tareaObserv.Nombre_tarea = q_dic_revision_dictamen.Nombre_tarea;
                        tareaObserv.id_tramitetarea = id_tramitetarea;
                        tareaObserv.Observaciones = "";
                        tareaObserv.UsuarioApeNom = q_dic_revision_dictamen.UsuarioApeNom;
                        tareaObserv.Fecha = q_dic_revision_dictamen.Fecha;

                        if (!string.IsNullOrEmpty(q_dic_revision_dictamen.Observaciones))
                            tareaObserv.Item.Add(new Items(q_dic_revision_dictamen.Observaciones + "<br/>", "Observaciones Internas:"));

                    }

                    break;

                #endregion

                #region Dictamen Revisión Sub-Gerente

                //case (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_SubGerente:
                //case (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_SubGerente:
                //case (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_SubGerente:
                //    var q_dic_subgerente =
                //        (
                //            from subGerente in db.SGI_Tarea_Dictamen_Revision_SubGerente
                //            join tramite in db.SGI_Tramites_Tareas on subGerente.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on subGerente.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on subGerente.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where subGerente.id_tramitetarea == id_tramitetarea
                //                && (
                //                    ver_observ_vacias || 
                //                    !ver_observ_vacias && 
                //                        (
                //                            !string.IsNullOrEmpty(subGerente.Observaciones)
                //                        )
                //                   )
                //            orderby subGerente.id_Dictamen_Revision_SubGerente descending
                //            select new
                //            {
                //                ID = subGerente.id_Dictamen_Revision_SubGerente,
                //                Observaciones = subGerente.Observaciones,
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (subGerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (subGerente.LastUpdateDate.HasValue) ? (DateTime)subGerente.LastUpdateDate : subGerente.CreateDate
                //            }
                //        ).FirstOrDefault();


                //    if (q_dic_subgerente != null)
                //    {
                //        tareaObserv = new ObservacionAnteriores();

                //        tareaObserv.ID = q_dic_subgerente.ID;
                //        tareaObserv.id_tarea = 0;
                //        tareaObserv.Nombre_tarea = q_dic_subgerente.Nombre_tarea;
                //        tareaObserv.id_tramitetarea = id_tramitetarea;
                //        tareaObserv.Observaciones = "";
                //        tareaObserv.UsuarioApeNom = q_dic_subgerente.UsuarioApeNom;
                //        tareaObserv.Fecha = q_dic_subgerente.Fecha;

                //        if (!string.IsNullOrEmpty(q_dic_subgerente.Observaciones))
                //            tareaObserv.Item.Add(new Items(q_dic_subgerente.Observaciones + "<br/>", "Observaciones Internas:"));

                //    }

                //    break;

                #endregion

                #region Dictamen Revisión Gerente

                //case (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_Gerente:
                //case (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_Gerente:
                //case (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Gerente:
                //    var q_dict_gerente =

                //        (
                //            from gerente in db.SGI_Tarea_Dictamen_Revision_Gerente
                //            join tramite in db.SGI_Tramites_Tareas on gerente.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on gerente.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on gerente.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where gerente.id_tramitetarea == id_tramitetarea
                //                && (
                //                    ver_observ_vacias ||
                //                    !ver_observ_vacias &&
                //                        (
                //                            !string.IsNullOrEmpty(gerente.Observaciones)
                //                        )
                //                   )

                //            orderby gerente.id_Dictamen_Revision_Gerente descending
                //            select new 
                //            {
                //                ID = gerente.id_Dictamen_Revision_Gerente,
                //                Observaciones = gerente.Observaciones,
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (gerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (gerente.LastUpdateDate.HasValue) ? (DateTime)gerente.LastUpdateDate : gerente.CreateDate
                //            }
                //        ).FirstOrDefault();

                //    if (q_dict_gerente != null)
                //    {
                //        tareaObserv = new ObservacionAnteriores();

                //        tareaObserv.ID = q_dict_gerente.ID;
                //        tareaObserv.id_tarea = 0;
                //        tareaObserv.Nombre_tarea = q_dict_gerente.Nombre_tarea;
                //        tareaObserv.id_tramitetarea = id_tramitetarea;
                //        tareaObserv.Observaciones = "";
                //        tareaObserv.UsuarioApeNom = q_dict_gerente.UsuarioApeNom;
                //        tareaObserv.Fecha = q_dict_gerente.Fecha;

                //        if (!string.IsNullOrEmpty(q_dict_gerente.Observaciones))
                //            tareaObserv.Item.Add(new Items(q_dict_gerente.Observaciones + "<br/>", "Observaciones Internas:"));
                //    }

                //    break;

                #endregion

                #region Verificacion AVH
                case Constants.ENG_Tipos_Tareas.Verificacion_AVH:
                    var q_verificacion_avh =

                        (
                            from gerente in db.SGI_Tarea_Verificacion_AVH
                            join tramite in db.SGI_Tramites_Tareas on gerente.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on gerente.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on gerente.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where gerente.id_tramitetarea == id_tramitetarea
                                && (
                                    ver_observ_vacias ||
                                    !ver_observ_vacias &&
                                        (
                                            !string.IsNullOrEmpty(gerente.Observaciones)
                                        )
                                   )

                            orderby gerente.id_verificacion_AVH descending
                            select new
                            {
                                ID = gerente.id_verificacion_AVH,
                                Observaciones = gerente.Observaciones,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (gerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (gerente.LastUpdateDate.HasValue) ? (DateTime)gerente.LastUpdateDate : gerente.CreateDate
                            }
                        ).FirstOrDefault();

                    if (q_verificacion_avh != null)
                    {
                        tareaObserv = new ObservacionAnteriores();

                        tareaObserv.ID = q_verificacion_avh.ID;
                        tareaObserv.id_tarea = 0;
                        tareaObserv.Nombre_tarea = q_verificacion_avh.Nombre_tarea;
                        tareaObserv.id_tramitetarea = id_tramitetarea;
                        tareaObserv.Observaciones = "";
                        tareaObserv.UsuarioApeNom = q_verificacion_avh.UsuarioApeNom;
                        tareaObserv.Fecha = q_verificacion_avh.Fecha;

                        if (!string.IsNullOrEmpty(q_verificacion_avh.Observaciones))
                            tareaObserv.Item.Add(new Items(q_verificacion_avh.Observaciones + "<br/>", "Observaciones Internas:"));
                    }

                    break;

                #endregion

                #region Control e Informe
                case Constants.ENG_Tipos_Tareas_Transf.Control_Informe:
                    var q_control_informe2 =

                        (
                            from gerente in db.SGI_Tarea_Carga_Tramite
                            join tramite in db.SGI_Tramites_Tareas on gerente.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on gerente.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on gerente.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where gerente.id_tramitetarea == id_tramitetarea
                                && (
                                    ver_observ_vacias ||
                                    !ver_observ_vacias &&
                                        (
                                            !string.IsNullOrEmpty(gerente.Observaciones)
                                        )
                                   )

                            orderby gerente.id_carga_tramite descending
                            select new
                            {
                                ID = gerente.id_carga_tramite,
                                Observaciones = gerente.Observaciones,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (gerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (gerente.LastUpdateDate.HasValue) ? (DateTime)gerente.LastUpdateDate : gerente.CreateDate
                            }
                        );
                    //FirstOrDefault();
                    var q_control_informe = q_control_informe2.FirstOrDefault();
                    if (q_control_informe != null)
                    {
                        tareaObserv = new ObservacionAnteriores();

                        tareaObserv.ID = q_control_informe.ID;
                        tareaObserv.id_tarea = 0;
                        tareaObserv.Nombre_tarea = q_control_informe.Nombre_tarea;
                        tareaObserv.id_tramitetarea = id_tramitetarea;
                        tareaObserv.Observaciones = "";
                        tareaObserv.UsuarioApeNom = q_control_informe.UsuarioApeNom;
                        tareaObserv.Fecha = q_control_informe.Fecha;

                        if (!string.IsNullOrEmpty(q_control_informe.Observaciones))
                            tareaObserv.Item.Add(new Items(q_control_informe.Observaciones + "<br/>", "Observaciones Internas:"));
                    }

                    break;
                #endregion
                default:
                    break;
            }

            if (tareaObserv != null)
            {
                tareaObserv.id_tarea = id_tarea;
                tareaObserv.id_tramitetarea = id_tramitetarea;
                if (tareaObserv.Item.Count == 0)
                    tareaObserv.Item.Add(new Items(tareaObserv.Observaciones, ""));
            }

        }
        catch (Exception ex)
        {
            if (db != null)
                db.Dispose();

            throw ex;
        }
        return tareaObserv;
    }
    #endregion

    public static string Buscar_ObservacionPlancheta(int id_grupotramite, int id_solicitud, int id_tramitetarea)
    {
        DGHP_Entities db = new DGHP_Entities();

        string observ = null;

        int[] tareas = ObtenerTareasConPlancheta();

        TramiteTareaAnteriores tramite_tarea = TramiteTareaAnteriores.BuscarUltimoTramiteTareaPorTarea(id_grupotramite, id_solicitud, id_tramitetarea, tareas).FirstOrDefault();

        if (tramite_tarea != null)
        {
            var q_calificar = (
                from stc in db.SGI_Tarea_Calificar
                where stc.id_tramitetarea == tramite_tarea.id_tramitetarea
                select stc.Observaciones).FirstOrDefault();

            var q_dghp = (
                from strd in db.SGI_Tarea_Revision_DGHP
                where strd.id_tramitetarea == tramite_tarea.id_tramitetarea
                select strd.Observaciones).FirstOrDefault();

            var q_sub_gerente = (
                from strsg in db.SGI_Tarea_Revision_SubGerente
                where strsg.id_tramitetarea == tramite_tarea.id_tramitetarea
                select strsg.observacion_plancheta).FirstOrDefault();

            var q_gerente = (
                from strg in db.SGI_Tarea_Revision_Gerente
                where strg.id_tramitetarea == tramite_tarea.id_tramitetarea
                select strg.observacion_plancheta).FirstOrDefault();

            var q_documental = (
                from strg in db.SGI_Tarea_Gestion_Documental
                where strg.id_tramitetarea == tramite_tarea.id_tramitetarea
                select strg.observacion_plancheta).FirstOrDefault();

            observ = q_calificar ?? q_dghp ?? q_sub_gerente ?? q_gerente ?? q_documental;
        }

        db.Dispose();

        return observ ?? string.Empty;
    }

    public static int[] ObtenerTareasConPlancheta()
    {
        DGHP_Entities db = new DGHP_Entities();

        var tareas = (
            from et in db.ENG_Tareas
            where (et.formulario_tarea.Contains("gerente") || et.formulario_tarea.Contains("calificar") || et.formulario_tarea.Contains("dghp")
            || et.formulario_tarea.Contains("gestion"))
            && !et.formulario_tarea.Contains("dictamen")
            select et.id_tarea).ToArray();
        
        db.Dispose();
        
        return tareas;
    }
}
