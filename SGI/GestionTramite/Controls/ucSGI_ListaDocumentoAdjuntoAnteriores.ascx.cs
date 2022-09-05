using System;
using System.Collections.Generic;
using System.Linq;
using SGI.Model;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucSGI_ListaDocumentoAdjuntoAnteriores : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                // ScriptManager.RegisterStartupScript(updPnlDosAdjAnterior, updPnlDosAdjAnterior.GetType(), "init_Js_updPnlInputDatosArch", "init_Js_updPnlInputDatosArch();", true);

            }

        }

        private AGC_FilesEntities dbFiles = null;
        private DGHP_Entities db = null;
        private bool _Enabled;
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
            }
        }
        public void LoadData(int id_solicitud, int id_tramitetarea)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea);
        }

        public void LoadData(int id_grupotramite, int id_solicitud, int id_tramitetarea)
        {
            try
            {
                this.dbFiles = new AGC_FilesEntities();
                this.db = new DGHP_Entities();

                SGI_Tramites_Tareas tramitetareaActual = this.db.SGI_Tramites_Tareas.Where(x => x.id_tramitetarea == id_tramitetarea).FirstOrDefault();

                Cargar_DocumentosAdjuntos(id_grupotramite, id_solicitud, tramitetareaActual);

                this.dbFiles.Dispose();
                this.db.Dispose();

            }
            catch (Exception ex)
            {
                if (this.dbFiles != null)
                    this.dbFiles.Dispose();
                if (this.db != null)
                    this.db.Dispose();
                throw ex;
            }

        }


        private void Cargar_DocumentosAdjuntos(int id_grupotramite, int id_solicitud, SGI_Tramites_Tareas tramitetareaActual)
        {
            this.id_solicitud = id_solicitud;
            this.id_grupotramite = id_grupotramite;
            this.id_tramitetarea = tramitetareaActual.id_tramitetarea;

            Constants.ENG_Tareas eng_tarea;
            Enum.TryParse(tramitetareaActual.id_tarea.ToString(), out eng_tarea);
            
            int[] tareas = TramiteTareaAnteriores.Dependencias_Tarea_DocumentosAdjuntos(eng_tarea);

            List<TramiteTareaAnteriores> list_tramite_tarea = TramiteTareaAnteriores.BuscarTodosTareaTramitesAnteriores(id_grupotramite, id_solicitud, tramitetareaActual.id_tramitetarea, tareas);
            int[] list_id_tramitetarea = list_tramite_tarea.Select(x =>x.id_tramitetarea).ToArray();

            List<TiposDeDocumentosRequeridos> list_tipo_doc = this.db.TiposDeDocumentosRequeridos.ToList();
            
            var list_doc_adj =
                (
                    from adj in db.SGI_Tarea_Documentos_Adjuntos
                    where list_id_tramitetarea.Contains(adj.id_tramitetarea)  // adj.id_tramitetarea IN ( array de id_tramitetarea)
                    orderby adj.id_tramitetarea descending
                    select new
                    {
                        id_doc_adj = adj.id_doc_adj,
                        id_tramitetarea = adj.id_tramitetarea,
                        nombre_tarea = "",
                        id_tdocreq = adj.id_tdocreq,
                        tdoc_adj_detalle = adj.tdoc_adj_detalle,
                        CreateDate = adj.CreateDate,
                        CreateUser = adj.CreateUser
                    }
                ).ToList();

            var lista_final =
                    (
                        from adj in list_doc_adj
                        join tt in list_tramite_tarea on adj.id_tramitetarea equals tt.id_tramitetarea
                        join tdoc in list_tipo_doc on adj.id_tdocreq equals tdoc.id_tdocreq
                        select new
                        {
                            id_doc_adj = adj.id_doc_adj,
                            id_tramitetarea = adj.id_tramitetarea,
                            nombre_tarea = tt.Nombre_tarea,
                            id_tdocreq = adj.id_tdocreq,
                            tdoc_adj_detalle = (string.IsNullOrEmpty(adj.tdoc_adj_detalle) ? tdoc.nombre_tdocreq : adj.tdoc_adj_detalle),
                            CreateDate = adj.CreateDate,
                            CreateUser = adj.CreateUser
                        }
                    ).ToList();


            grd_doc_adj_anteriores.DataSource = lista_final;
            grd_doc_adj_anteriores.DataBind();
            updPnlDosAdjAnterior.Update();

            pnlDosAdjAnterior.Visible = (grd_doc_adj_anteriores.Rows.Count > 0);

            
        }

        protected void grd_doc_adj_anteriores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkEliminar = (LinkButton)e.Row.FindControl("lnkEliminarDocAdj");
                //lnkEliminar.Visible = this.Enabled;
                int id_tdocreq = int.Parse(grd_doc_adj_anteriores.DataKeys[e.Row.RowIndex].Values["id_tdocreq"].ToString());
               // TiposDeDocumentosRequeridos tipo = db.TiposDeDocumentosRequeridos.FirstOrDefault(x => x.id_tdocreq == id_tdocreq);
                if (id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Plano_Visado)
                {
                    lnkEliminar.Visible = this.Enabled;
                }
                else lnkEliminar.Visible = false;
            }
        }
        #region eventos
        public delegate void EventHandlerEliminar(object sender, ucSGI_ListaDocumentoAdjuntoAnterioresEventsArgs e);
        public event EventHandlerEliminar EliminarDocumentoAdjuntoClick;
        protected void lnkEliminarDocAdj_Command(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkEliminar = (LinkButton)sender;
                int id_doc_adj = Convert.ToInt32(lnkEliminar.CommandArgument);
                this.db = new DGHP_Entities();

                this.db.SGI_Tarea_Documentos_Adjuntos_Eliminar(id_doc_adj);

                if (this.EliminarDocumentoAdjuntoClick != null)
                {
                    ucSGI_ListaDocumentoAdjuntoAnterioresEventsArgs args = new ucSGI_ListaDocumentoAdjuntoAnterioresEventsArgs();
                    args.id_doc_adj = id_doc_adj;
                    this.EliminarDocumentoAdjuntoClick(this, args);
                }

                SGI_Tramites_Tareas tramitetareaActual = this.db.SGI_Tramites_Tareas.Where(x => x.id_tramitetarea == this.id_tramitetarea).FirstOrDefault();
                // Cargar_DocumentosAdjuntos(this.id_grupotramite, this.id_solicitud, tramitetareaActual);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();

                ScriptManager.RegisterClientScriptBlock(updPnlDosAdjAnterior, updPnlDosAdjAnterior.GetType(),
                        "tda_mostrar_mensaje", "tda_mostrar_mensaje('" + ex.Message + "','')", true);
            }

        }
        #endregion
        public class ucSGI_ListaDocumentoAdjuntoAnterioresEventsArgs : EventArgs
        {
            public int id_doc_adj { get; set; }
        }
        #region Atributos

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
                    _titulo = "Lista de Documentos Adjuntados Anteriormente";
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
                hid_ldaa_collapse.Value = _collapse.ToString().ToLower();

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

        private int _id_grupotramite = 0;
        public int id_grupotramite
        {
            get
            {
                if (_id_grupotramite == 0)
                {
                    int.TryParse(hid_id_grupotramite.Value, out _id_grupotramite);
                }
                return _id_grupotramite;
            }
            set
            {
                hid_id_grupotramite.Value = value.ToString();
                _id_grupotramite = value;
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
        #endregion

    }

}