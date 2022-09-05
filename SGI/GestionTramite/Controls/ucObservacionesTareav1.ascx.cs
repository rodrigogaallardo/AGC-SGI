using SGI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucObservacionesTareav1 : System.Web.UI.UserControl
    {
        #region cargar inicial

        private DGHP_Entities db = null;
        private class itemGrillaObservaciones
        {
            public int id_ObsDocs { get; set; }
            public int id_ObsGrupo { get; set; }
            public int id_tdocreq { get; set; }
            public string nombre_tdocreq { get; set; }
            public string Observacion_ObsDocs { get; set; }
            public string Respaldo_ObsDocs { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updAgregar, updAgregar.GetType(), "init_Js_upd_ddlDocumento", "init_Js_upd_ddlDocumento();", true);
            }

            this.txtObservaciones.Attributes.Add("maxlength", "8000");
            this.txtRespaldo.Attributes.Add("maxlength", "2000");
        }

        public void LoadData(int id_solicitud, int id_tramitetareaa)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea);
        }

        public void LoadData(int id_grupotramite, int id_solicitud, int id_tramitetarea)
        {
            LoadData(id_grupotramite, id_solicitud, id_tramitetarea, true);
        }
        public void LoadData(int id_grupotramite, int id_solicitud, int id_tramitetarea, bool mostarRespaldo)
        {
            pnlRespaldoNormativo.Visible = mostarRespaldo;
            this.db = new DGHP_Entities();
            try
            {
                this.id_solicitud = id_solicitud;
                this.id_tramitetarea = id_tramitetarea;
                this.id_grupotramite = id_grupotramite;
                if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                {
                    var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    this.id_tipotramite = sol.id_tipotramite;
                    // --------------------------------------------------------------------
                    // Los tipos de tramite de rectificatoria se cambian a habilitación 
                    // ya que rectificatoria en realidad no debería ser un tipo de trámite.
                    // --------------------------------------------------------------------
                    if (this.id_tipotramite == (int)Constants.TipoDeTramite.RectificatoriaHabilitacion)
                        this.id_tipotramite = (int)Constants.TipoDeTramite.Habilitacion;
                }
                else if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
                {
                    var sol = db.CPadron_Solicitudes.FirstOrDefault(x => x.id_cpadron == id_solicitud);
                    this.id_tipotramite = sol.id_tipotramite;
                }
                else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
                {
                    this.id_tipotramite = 2;
                }

                Limpiar();
                Cargar_tipo_documentos_adjuntos(id_tipotramite);
                Cargar_documentos_adjuntos();

                // Se oculta la columna de edición cuando el control está deshabilitado
                // --
                grdObser.Columns[0].Visible = this.Enabled;
                grdObser.Columns[4].Visible = this.Enabled;
                btnShowAgregar.Visible = this.Enabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.db != null)
                    this.db.Dispose();
            }

        }

        internal void LoadData(int id_grupotramite, int id_solicitud, int id_tramitetarea, object mostarRespaldo)
        {
            throw new NotImplementedException();
        }

        private void Cargar_tipo_documentos_adjuntos(int id_tipotramite)
        {
            var q = (
                    from tdoc in db.TiposDeDocumentosRequeridos
                    join rtt in db.Rel_TipoTramite_TiposDeDocumentosRequeridos on tdoc.id_tdocreq equals rtt.id_tdocreq into zr
                    from rel in zr.DefaultIfEmpty()
                    where tdoc.visible_en_Obs == true || rel.id_tipotramite == id_tipotramite
                    select tdoc
                ).Distinct().OrderBy(x=>x.nombre_tdocreq);
            List<TiposDeDocumentosRequeridos> list_doc_adj = q.ToList();

            if (list_doc_adj.Count > 1)
                list_doc_adj.Insert(0, (new TiposDeDocumentosRequeridos
                {
                    id_tdocreq = 0,
                    nombre_tdocreq = "Seleccione",
                    observaciones_tdocreq = "Seleccione",
                    baja_tdocreq = false,
                    RequiereDetalle = false,
                    visible_en_SSIT = false,
                    visible_en_SGI = true
                }));


            ddlDocumento.DataValueField = "id_tdocreq";
            ddlDocumento.DataTextField = "nombre_tdocreq";
            ddlDocumento.DataSource = list_doc_adj;
            ddlDocumento.DataBind();
        }

        private void Cargar_documentos_adjuntos()
        {
            Guid userid = Functions.GetUserId();

            SGI_Tarea_Calificar_ObsGrupo grupo = db.SGI_Tarea_Calificar_ObsGrupo.FirstOrDefault(x => x.id_tramitetarea == this.id_tramitetarea);

            if (grupo == null)
                this.id_ObsGrupo = 0;
            else
                this.id_ObsGrupo = grupo.id_ObsGrupo;

            List<itemGrillaObservaciones> lstObservaciones = new List<itemGrillaObservaciones>();
            if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                lstObservaciones = (from obs in db.SGI_Tarea_Calificar_ObsDocs
                                    join tdocreq in db.TiposDeDocumentosRequeridos on obs.id_tdocreq equals tdocreq.id_tdocreq
                                    join grup in db.SGI_Tarea_Calificar_ObsGrupo on obs.id_ObsGrupo equals grup.id_ObsGrupo
                                    join tt_h in db.SGI_Tramites_Tareas_TRANSF on grup.id_tramitetarea equals tt_h.id_tramitetarea
                                    where tt_h.id_solicitud == this.id_solicitud && obs.Actual == true
                                    select new itemGrillaObservaciones
                                    {
                                        id_ObsDocs = obs.id_ObsDocs,
                                        id_ObsGrupo = obs.id_ObsGrupo,
                                        nombre_tdocreq = tdocreq.nombre_tdocreq,
                                        id_tdocreq = obs.id_tdocreq,
                                        Observacion_ObsDocs = obs.Observacion_ObsDocs,
                                        Respaldo_ObsDocs = obs.Respaldo_ObsDocs
                                    }).ToList();
            }
            else
            {
                lstObservaciones = (from obs in db.SGI_Tarea_Calificar_ObsDocs
                                    join tdocreq in db.TiposDeDocumentosRequeridos on obs.id_tdocreq equals tdocreq.id_tdocreq
                                    join grup in db.SGI_Tarea_Calificar_ObsGrupo on obs.id_ObsGrupo equals grup.id_ObsGrupo
                                    join tt_h in db.SGI_Tramites_Tareas_HAB on grup.id_tramitetarea equals tt_h.id_tramitetarea
                                    where tt_h.id_solicitud == this.id_solicitud && obs.Actual == true
                                    select new itemGrillaObservaciones
                                    {
                                        id_ObsDocs = obs.id_ObsDocs,
                                        id_ObsGrupo = obs.id_ObsGrupo,
                                        nombre_tdocreq = tdocreq.nombre_tdocreq,
                                        id_tdocreq = obs.id_tdocreq,
                                        Observacion_ObsDocs = obs.Observacion_ObsDocs,
                                        Respaldo_ObsDocs = obs.Respaldo_ObsDocs
                                    }).ToList();
            }
            countObservaciones = lstObservaciones.Count;
            grdObser.DataSource = lstObservaciones;
            grdObser.DataBind();
        }

        protected void grdObser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkEditar = (LinkButton)e.Row.FindControl("lnkEditarObser");
                lnkEditar.Visible = this.Enabled;
                LinkButton lnkEliminar = (LinkButton)e.Row.FindControl("lnkEliminarObser");
                lnkEliminar.Visible = this.Enabled;
            }
        }


        private void Limpiar()
        {
            if (ddlDocumento.Items.Count > 0)
                ddlDocumento.SelectedIndex = 0;
            txtObservaciones.Text = "";
            txtRespaldo.Text = "";
        }

        #endregion

        #region Propiedades

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
                    _titulo = "Documentación a Presentar";
                tituloControl.Text = _titulo;
            }
        }

        private int _id_tramitetarea = 0;
        public int id_tramitetarea
        {
            get
            {
                if (_id_tramitetarea == 0)
                {
                    int.TryParse(hid_id_tramitetarea.Value, out _id_tramitetarea);
                }
                return _id_tramitetarea;
            }
            set
            {
                hid_id_tramitetarea.Value = value.ToString();
                _id_tramitetarea = value;
            }
        }

        private int _id_solicitud = 0;
        public int id_solicitud
        {
            get
            {
                if (_id_solicitud == 0)
                {
                    int.TryParse(hid_id_solicitud.Value, out _id_solicitud);
                }
                return _id_solicitud;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
                _id_solicitud = value;
            }
        }

        public int id_grupotramite
        {
            get
            {
                return (ViewState["_id_grupotramite"] != null ? Convert.ToInt32(ViewState["_id_grupotramite"]) : 0);
            }
            set
            {
                ViewState["_id_grupotramite"] = value.ToString();
            }
        }

        public int id_tipotramite
        {
            get
            {
                return (ViewState["_id_tipotramite"] != null ? Convert.ToInt32(ViewState["_id_tipotramite"]) : 0);
            }
            set
            {
                ViewState["_id_tipotramite"] = value.ToString();
            }
        }

        public int countObservaciones
        {
            get
            {
                return (ViewState["_countObservaciones"] != null ? Convert.ToInt32(ViewState["_countObservaciones"]) : 0);
            }
            set
            {
                ViewState["_countObservaciones"] = value.ToString();
            }
        }

        public int id_ObsGrupo
        {
            get
            {
                return (ViewState["_id_ObsGrupo"] != null ? Convert.ToInt32(ViewState["_id_ObsGrupo"]) : 0);
            }
            set
            {
                ViewState["_id_ObsGrupo"] = value.ToString();
            }
        }

        private bool _Enabled;

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(true),
        Description("Devuelve/Establece el estado de los controles contenidos en este control.")]
        public bool Enabled
        {
            get
            {

                if (!bool.TryParse(hid_editable.Value, out _Enabled))
                    _Enabled = false;
                return _Enabled;
            }
            set
            {
                _Enabled = value;
                hid_editable.Value = _Enabled.ToString().ToLower();
                //btnCargarOtroArchivo.Visible = _Enabled;
                //ddl_tipo_doc.Enabled = _Enabled;
                //txtDetalle.Enabled = _Enabled;
                //Tendria q desabilitar el boton
                //aspPanelBoton.Visible = _Enabled;

            }
        }

        #endregion

        #region Eventos

        protected void lnkEliminarObser_Command(object sender, CommandEventArgs e)
        {
            try
            {
                LinkButton lnkEliminar = (LinkButton)sender;
                int id_ObsDocs = Convert.ToInt32(lnkEliminar.CommandArgument);
                this.db = new DGHP_Entities();
                db.SGI_Tarea_Calificar_ObsDocs_Eliminar(id_ObsDocs);
                Cargar_documentos_adjuntos();

                this.db.Dispose();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(updPnlObser, updPnlObser.GetType(),"tda_mostrar_mensaje", "tda_mostrar_mensaje('" + ex.Message + "','')", true);
            }
            finally
            {
                if (this.db != null)
                    this.db.Dispose();
            }
        }

        protected void lnkEditarObser_Command(object sender, CommandEventArgs e)
        {
            try
            {
                LinkButton lnkEliminar = (LinkButton)sender;
                int id_ObsDocs = Convert.ToInt32(lnkEliminar.CommandArgument);
                this.db = new DGHP_Entities();
                Limpiar();
                hid_id_ObsDocs.Value = id_ObsDocs.ToString();
                SGI_Tarea_Calificar_ObsDocs ob = db.SGI_Tarea_Calificar_ObsDocs.FirstOrDefault(x => x.id_ObsDocs == id_ObsDocs);
                ddlDocumento.SelectedValue = ob.id_tdocreq.ToString();
                txtObservaciones.Text = ob.Observacion_ObsDocs;
                txtRespaldo.Text = ob.Respaldo_ObsDocs;

                updAgregar.Update();
                Functions.EjecutarScript(updShowAgregar, "showfrmDatosObser();");

                this.db.Dispose();

            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                ScriptManager.RegisterClientScriptBlock(updPnlObser, updPnlObser.GetType(),"tda_mostrar_mensaje", "tda_mostrar_mensaje('" + ex.Message + "','')", true);
            }
            finally
            {
                if (this.db != null)
                    this.db.Dispose();
            }

        }


        protected void btnShowAgregar_Click(object sender, EventArgs e)
        {
            hid_id_ObsDocs.Value = "0";
            Limpiar();
            updAgregar.Update();
            Functions.EjecutarScript(updShowAgregar, "showfrmDatosObser();");
        }

        private void validarCargar() {
            int id_tipodoc = 0;
            int.TryParse(ddlDocumento.SelectedValue, out id_tipodoc);

            var lstObservaciones = (from obs in db.SGI_Tarea_Calificar_ObsDocs
                                    join tdocreq in db.TiposDeDocumentosRequeridos on obs.id_tdocreq equals tdocreq.id_tdocreq
                                    join grup in db.SGI_Tarea_Calificar_ObsGrupo on obs.id_ObsGrupo equals grup.id_ObsGrupo
                                    join tt_h in db.SGI_Tramites_Tareas_HAB on grup.id_tramitetarea equals tt_h.id_tramitetarea
                                    where tt_h.id_solicitud == this.id_solicitud && obs.Actual == true
                                    && obs.id_tdocreq == id_tipodoc
                                    select new 
                                    {
                                        id_ObsDocs = obs.id_ObsDocs
                                    }).ToList();
            if (lstObservaciones.Count > 0)
                throw new Exception("Ya se cargo un documento para el tipo ingresado.");
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                this.db = new DGHP_Entities();
                bool validar = true;
                int id_ObsDocs = 0;
                int id_tipodoc = 0;
                int.TryParse(ddlDocumento.SelectedValue, out id_tipodoc);

                if (hid_id_ObsDocs.Value != "0")
                {
                    id_ObsDocs = Convert.ToInt32(hid_id_ObsDocs.Value);
                    SGI_Tarea_Calificar_ObsDocs doc = db.SGI_Tarea_Calificar_ObsDocs.FirstOrDefault(x => x.id_ObsDocs == id_ObsDocs);
                    validar = id_tipodoc != doc.id_tdocreq;
                }

                if (validar) validarCargar();
                
                Guid userid = Functions.GetUserId();
                string observaciones = txtObservaciones.Text.Trim();
                string respaldo = txtRespaldo.Text.Trim();
                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        if (hid_id_ObsDocs.Value == "0")
                        {//alta
                            if(this.id_ObsGrupo==0)
                            {
                                ObjectParameter param_id_ObsGrupo = new ObjectParameter("id_ObsGrupo", typeof(int));
                                db.SGI_Tarea_Calificar_ObsGrupo_Agregar(this.id_tramitetarea, userid, param_id_ObsGrupo);
                                this.id_ObsGrupo = Convert.ToInt32(param_id_ObsGrupo.Value);

                            }
                            ObjectParameter param_id_ObsDocs = new ObjectParameter("id_ObsDocs", typeof(int));
                            db.SGI_Tarea_Calificar_ObsDocs_Agregar(id_ObsGrupo, id_tipodoc, observaciones, respaldo, false, true, userid, param_id_ObsDocs);
                        }
                        else
                        {//edit
                            id_ObsDocs = Convert.ToInt32(hid_id_ObsDocs.Value);
                            db.SGI_Tarea_Calificar_ObsDocs_Editar(id_ObsDocs, id_tipodoc, observaciones, respaldo, false, true, userid);
                        }

                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }
                Cargar_documentos_adjuntos();
                Functions.EjecutarScript(updBotonesAgregar, "hidefrmDatosObser();");

            }
            catch (Exception ex)
            {
                lblErrorObs.Text = Functions.GetErrorMessage(ex);
                Functions.EjecutarScript(updBotonesAgregar, "hidefrmDatosObser();showfrmError_ucObser();");

            }
            finally
            {
                if (this.db != null)
                    this.db.Dispose();
            }
        }

        #endregion
    }
}

public class ucObservacionesTareav1 : EventArgs
{
    public int id_doc_adj { get; set; }
}

public class ucObservacionesTareav1Exception : Exception
{
    public ucObservacionesTareav1Exception(string mensaje)
        : base(mensaje, new Exception())
    {
    }
}

