using Newtonsoft.Json;
using SGI.GestionTramite.Controls;
using SGI.Model;
using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public class DocumentosAdjuntosDTO
    {
        public int id_tdocreq { get; set; }
        public string nombre_tdocreq { get; set; }
        public string tdocreq_detalle { get; set; }
        public int id_file { get; set; }
        public string nombre_archivo { get; set; }
        public Guid rowid { get; set; }
    }


    public partial class BajaSolicitud : BasePage
    {
        private bool formulario_cargado
        {
            get
            {
                return Convert.ToBoolean(hid_formulario_cargado.Value);
            }
            set
            {
                hid_formulario_cargado.Value = value.ToString();
            }
        }


        DGHP_Entities db = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "inicializar_controles", "inicializar_controles();", true);
            }

            if (!IsPostBack)
            {

            }
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            Cargarddl();
            CargaDocumentos.LoadData();
            updpnlBuscar.Update();
            updpnlAgregarDocumentos.Update();
            this.formulario_cargado = true;

            this.EjecutarScript(updCargaInicial, "finalizarCargaBaja();");
        }
        private void Cargarddl()
        {
            IniciarEntity();

            var lista = db.TiposMotivoBaja.ToList();

            ddlMotivoBaja.DataSource = lista;

            ddlMotivoBaja.DataTextField = "nombre";
            ddlMotivoBaja.DataValueField = "id_tipo_motivo_baja";
            ddlMotivoBaja.DataBind();
            ListItem NombreDocTodos = new ListItem("Todos", "-1");
            ddlMotivoBaja.Items.Insert(0, NombreDocTodos);

            ddlMotivoBajaBus.DataSource = lista;
            ddlMotivoBajaBus.DataTextField = "nombre";
            ddlMotivoBajaBus.DataValueField = "id_tipo_motivo_baja";
            ddlMotivoBajaBus.DataBind();
            ddlMotivoBajaBus.Items.Insert(0, NombreDocTodos);

            List<string> lstOcultarTipoTramite = new List<string>();
            lstOcultarTipoTramite.Add("LIGUE");
            lstOcultarTipoTramite.Add("DESLIGUE");
            lstOcultarTipoTramite.Add("AMPLIACION/UNIFICACION");
            lstOcultarTipoTramite.Add("RECTIF_HABILITACION");
            lstOcultarTipoTramite.Add("REDISTRIBUCION_USO");

            List<TipoTramite> list_tipoTramite = this.db.TipoTramite.Where(x => !lstOcultarTipoTramite.Contains(x.cod_tipotramite)).ToList();

            TipoTramite tipoTramite_select = new TipoTramite();
            tipoTramite_select.id_tipotramite = 0;
            tipoTramite_select.descripcion_tipotramite = "Seleccione";

            list_tipoTramite.Insert(0, tipoTramite_select);
            ddlTipoTramite.DataTextField = "descripcion_tipotramite";
            ddlTipoTramite.DataValueField = "id_tipotramite";

            ddlTipoTramite.DataSource = list_tipoTramite;
            ddlTipoTramite.DataBind();

            ddlTipoTramiteBaja.DataSource = list_tipoTramite;
            ddlTipoTramiteBaja.DataTextField = "descripcion_tipotramite";
            ddlTipoTramiteBaja.DataValueField = "id_tipotramite";
            ddlTipoTramiteBaja.DataBind();

            FinalizarEntity();
        }

        protected void btnNuevaBaja_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarDatos();
                updBaja.Update();
                this.EjecutarScript(UpdatePanel1, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "finalizarCarga();showfrmError();");
            }
        }
        private void LimpiarDatos()
        {
            txtSolicitud.Text = "";
            txtObservaciones.Text = "";
            ddlMotivoBaja.SelectedIndex = 0;
            gridAgregados_db.DataSource = new List<DocumentosAdjuntosDTO>();
            gridAgregados_db.DataBind();
            updpnlGrillaDoc.Update();
            ucProcesosSADE.id_grupo_tramite = ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;
            ucProcesosSADE.cargarDatosProcesos(1, false);
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtSolicitudBus.Text = "";
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            ddlMotivoBajaBus.SelectedIndex = 0;
            ddlTipoTramite.SelectedIndex = 0;
            updpnlBuscar.Update();
            EjecutarScript(UpdatePanel1, "hideResultados();");
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                grdResultados.DataBind();
                pnlResultadoBuscar.Visible = true;
                updPnlResultadoBuscar.Update();
                EjecutarScript(UpdatePanel1, "showResultados();");
                db.Dispose();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "showfrmError();");
            }
        }

        public List<clsBajas> GetResultados(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {

            totalRowCount = 0;

            int aux = 0;

            int solicitud = 0;
            int idMotivoBaja = 0;
            int idTipoTramite = 0;
            DateTime? fechaDesde = null;
            DateTime? fechaHasta = null;

            List<clsBajas> resultados = new List<clsBajas>();
            IQueryable<clsBajas> q = null;

            if (this.formulario_cargado)
            {

                int.TryParse(txtSolicitudBus.Text, out aux);
                solicitud = aux;

                int.TryParse(ddlMotivoBajaBus.SelectedItem.Value, out aux);
                idMotivoBaja = aux;

                int.TryParse(ddlTipoTramite.SelectedItem.Value, out aux);
                idTipoTramite = aux;

                if (idTipoTramite == 0)
                {
                    throw new Exception("Debe seleccionar un tipo de tramite.");
                }
                db = new DGHP_Entities();
                if (idTipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
                {
                    q = (from b in db.SSIT_Solicitudes_Baja
                         join u in db.SGI_Profiles on b.CreateUser equals u.userid
                         into j
                         from r in j.DefaultIfEmpty()
                         select new clsBajas
                         {
                             fecha = b.fecha_baja,
                             id_baja = b.id_baja,
                             id_solicitud = b.id_solicitud,
                             id_motivo = b.id_tipo_motivo_baja,
                             motivo = b.TiposMotivoBaja.nombre,
                             observaciones = b.observaciones,
                             usuario = r.Apellido + ", " + r.Nombres,
                             tipo = 0,
                             url = null
                         });
                }
                else if (idTipoTramite == (int)Constants.TipoDeTramite.Permiso)
                {
                    q = (from b in db.SSIT_Solicitudes_Baja
                         join u in db.SGI_Profiles.DefaultIfEmpty() on b.CreateUser equals u.userid
                         join sol in db.SSIT_Solicitudes on b.id_solicitud equals sol.id_solicitud
                         where sol.id_tipotramite == idTipoTramite
                         select new clsBajas
                         {
                             fecha = b.fecha_baja,
                             id_baja = b.id_baja,
                             id_solicitud = b.id_solicitud,
                             id_motivo = b.id_tipo_motivo_baja,
                             motivo = b.TiposMotivoBaja.nombre,
                             observaciones = b.observaciones,
                             usuario = u.Apellido + ", " + u.Nombres,
                             tipo = 0,
                             url = null
                         });
                }
                else if (idTipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                {
                    q = (from b in db.Transf_Solicitudes_Baja
                         join u in db.SGI_Profiles.DefaultIfEmpty() on b.CreateUser equals u.userid
                         //where b.id_solicitud == solicitud
                         select new clsBajas
                         {
                             fecha = b.fecha_baja,
                             id_baja = b.id_baja,
                             id_solicitud = b.id_solicitud,
                             id_motivo = b.id_tipo_motivo_baja,
                             motivo = b.TiposMotivoBaja.nombre,
                             observaciones = b.observaciones,
                             usuario = u.Apellido + ", " + u.Nombres,
                             tipo = 1,
                             url = null
                         });
                }
                else
                {
                    q = (from b in db.Cpadron_Solicitudes_Baja
                         join u in db.SGI_Profiles.DefaultIfEmpty() on b.CreateUser equals u.userid
                         //where b.id_cpadron == solicitud
                         select new clsBajas
                         {
                             fecha = b.fecha_baja,
                             id_baja = b.id_baja,
                             id_solicitud = b.id_cpadron,
                             id_motivo = b.id_tipo_motivo_baja,
                             motivo = b.TiposMotivoBaja.nombre,
                             observaciones = b.observaciones,
                             usuario = u.Apellido + ", " + u.Nombres,
                             tipo = 2,
                             url = null
                         });
                }


                if (solicitud > 0)
                    q = q.Where(x => x.id_solicitud == solicitud);

                if (idMotivoBaja > 0)
                    q = q.Where(x => x.id_motivo == idMotivoBaja);


                DateTime fechaDesdeAux;
                if (!string.IsNullOrEmpty(txtFechaDesde.Text))
                {
                    if (!DateTime.TryParse(txtFechaDesde.Text, out fechaDesdeAux))
                        throw new Exception("Fecha tarea desde inválida.");
                    fechaDesde = fechaDesdeAux;
                }

                DateTime fechaHastaAux;
                if (!string.IsNullOrEmpty(txtFechaHasta.Text))
                {
                    if (!DateTime.TryParse(txtFechaHasta.Text, out fechaHastaAux))
                        throw new Exception("Fecha tarea hasta inválida.");
                    fechaHasta = fechaHastaAux;
                }

                if (fechaDesde.HasValue || fechaHasta.HasValue)
                {
                    DateTime? fecha_desde = null;
                    DateTime? fecha_hasta = null;

                    if (fechaDesde.HasValue)
                        fecha_desde = fechaDesde.Value;
                    else
                        fecha_desde = new DateTime(2000, 1, 1);

                    if (fechaHasta.HasValue)
                        fecha_hasta = fechaHasta.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                    else
                        fecha_hasta = DateTime.Now;
                    q = q.Where(x => x.fecha >= fecha_desde && x.fecha <= fecha_hasta);
                }

                resultados = q.ToList();
                totalRowCount = q.Count();

                foreach (var item in resultados)
                {
                    int id_file = 0;
                    if (item.tipo == 0)
                    {
                        var doc = db.SSIT_DocumentosAdjuntos.Where(x => x.id_solicitud == item.id_solicitud && x.id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Otros).
                            OrderByDescending(x => x.id_docadjunto).FirstOrDefault();
                        if (doc != null)
                            id_file = doc.id_file;
                    }
                    else if (item.tipo == 1)
                    {
                        var doc = db.Transf_DocumentosAdjuntos.Where(x => x.id_solicitud == item.id_solicitud && x.id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Otros).
                            OrderByDescending(x => x.id_docadjunto).FirstOrDefault();
                        if (doc != null)
                            id_file = doc.id_file;
                    }
                    else  //tipo3
                    {
                        var doc = db.CPadron_DocumentosAdjuntos.Where(x => x.id_cpadron == item.id_solicitud && x.id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Otros).
                            OrderByDescending(x => x.id_docadjunto).FirstOrDefault();
                        if (doc != null)
                            id_file = doc.id_file;
                    }
                    item.url = string.Format("~/GetPDFFiles/{0}", Functions.ConvertToBase64(id_file.ToString()));
                }

                pnlCantRegistros.Visible = true;

                if (totalRowCount > 1)
                {
                    lblCantRegistros.Text = string.Format("{0} Tipos de Documentos Requeridos", totalRowCount);
                }
                else if (totalRowCount == 1)
                    lblCantRegistros.Text = string.Format("{0}  Tipos de Documentos Requeridos", totalRowCount);
                else
                {
                    pnlCantRegistros.Visible = false;
                }
                pnlResultadoBuscar.Visible = true;
                updPnlResultadoBuscar.Update();

            }
            return resultados;
        }

        private void AddQueryFinal(IQueryable<clsBajas> query, ref IQueryable<clsBajas> qFinal)
        {
            if (query != null)
            {
                if (qFinal != null)
                    qFinal = qFinal.Union(query);
                else
                    qFinal = query;
            }
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode("Baja Administrativa");

            //updPnlGrillaProcesos
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostratMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        private void IniciarEntity()
        {
            if (db == null)
                db = new DGHP_Entities();
        }
        private void FinalizarEntity()
        {
            if (db != null)
                db.Dispose();
        }

        private void CargarDocumentos()
        {
            updpnlAgregarDocumentos.Update();
            updpnlGrillaDoc.Update();
        }
        protected void CargaDocumentos_SubirDocumentoClick(object sender, ucCargaDocumentosEventsArgs e)
        {
            Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
            try
            {
                //Grabar el documento en la base
                int id_file = ws_FilesRest.subirArchivo(e.nombre_archivo, e.Documento);

                var doc = new DocumentosAdjuntosDTO();
                doc.id_tdocreq = e.id_tdocreq;
                doc.tdocreq_detalle = e.detalle_tdocreq;
                doc.nombre_tdocreq = e.nombre_tdocreq;
                doc.id_file = id_file;
                doc.nombre_archivo = e.nombre_archivo;
                doc.rowid = Guid.NewGuid();

                List<DocumentosAdjuntosDTO> listDoc = GetDocumentosCargados();

                listDoc.Add(doc);

                gridAgregados_db.DataSource = listDoc;
                gridAgregados_db.DataBind();

                CargarDocumentos();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterClientScriptBlock(pnlDatosDocumento, pnlDatosDocumento.GetType(), "mostrarError", "showfrmError(); ", true);
            }
        }

        private List<DocumentosAdjuntosDTO> GetDocumentosCargados()
        {
            List<DocumentosAdjuntosDTO> lstResult = new List<DocumentosAdjuntosDTO>();
            foreach (GridViewRow row in gridAgregados_db.Rows)
            {

                DocumentosAdjuntosDTO item = new DocumentosAdjuntosDTO();
                item.rowid = (Guid)gridAgregados_db.DataKeys[row.RowIndex].Values["rowid"];
                item.id_file = Convert.ToInt32(gridAgregados_db.DataKeys[row.RowIndex].Values["id_file"]);
                item.id_tdocreq = Convert.ToInt32(gridAgregados_db.DataKeys[row.RowIndex].Values["id_tdocreq"]);
                item.tdocreq_detalle = gridAgregados_db.DataKeys[row.RowIndex].Values["tdocreq_detalle"].ToString();
                item.nombre_tdocreq = gridAgregados_db.DataKeys[row.RowIndex].Values["nombre_tdocreq"].ToString();
                item.nombre_archivo = gridAgregados_db.DataKeys[row.RowIndex].Values["nombre_archivo"].ToString();
                lstResult.Add(item);
            }

            return lstResult;
        }

        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkEliminar = (LinkButton)sender;
                Guid rowid = Guid.Parse(lnkEliminar.CommandArgument);

                var lst = GetDocumentosCargados();
                var item_eliminar = lst.FirstOrDefault(x => x.rowid == rowid);
                lst.Remove(item_eliminar);
                Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
                string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
                Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(item_eliminar), url, string.Empty, "D", 3010);
                gridAgregados_db.DataSource = lst;
                gridAgregados_db.DataBind();
                updpnlGrillaDoc.Update();
                ScriptManager.RegisterStartupScript(updpnlGrillaDoc, updpnlGrillaDoc.GetType(), "ScriptOcultarAnularencomienda", "hidefrmConfirmarEliminarDocumento();", true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterClientScriptBlock(pnlDatosDocumento, pnlDatosDocumento.GetType(), "mostrarError", "showfrmError(); ", true);
            }
        }

        #region configuracion de la grilla

        protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //this.db
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }

        }

        protected void grdResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResultados.PageIndex = e.NewPageIndex;
        }

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;
            grdResultados.PageIndex = int.Parse(cmdPage.Text) - 1;
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdResultados.PageIndex = grdResultados.PageIndex - 1;
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdResultados.PageIndex = grdResultados.PageIndex + 1;
        }

        protected void grdResultados_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)grdResultados;
            GridViewRow fila = (GridViewRow)grid.BottomPagerRow;

            if (fila != null)
            {
                LinkButton btnAnterior = (LinkButton)fila.Cells[0].FindControl("cmdAnterior");
                LinkButton btnSiguiente = (LinkButton)fila.Cells[0].FindControl("cmdSiguiente");

                if (grid.PageIndex == 0)
                    btnAnterior.Visible = false;
                else
                    btnAnterior.Visible = true;

                if (grid.PageIndex == grid.PageCount - 1)
                    btnSiguiente.Visible = false;
                else
                    btnSiguiente.Visible = true;


                // Ocultar todos los botones con Números de Página
                for (int i = 1; i <= 19; i++)
                {
                    LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                    btn.Visible = false;
                }


                if (grid.PageIndex == 0 || grid.PageCount <= 10)
                {
                    // Mostrar 10 botones o el máximo de páginas

                    for (int i = 1; i <= 10; i++)
                    {
                        if (i <= grid.PageCount)
                        {
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                            btn.Text = i.ToString();
                            btn.Visible = true;
                        }
                    }
                }
                else
                {
                    // Mostrar 9 botones hacia la izquierda y 9 hacia la derecha
                    // o bien los que sea posible en caso de no llegar a 9

                    int CantBucles = 0;

                    LinkButton btnPage10 = (LinkButton)fila.Cells[0].FindControl("cmdPage10");
                    btnPage10.Visible = true;
                    btnPage10.Text = Convert.ToString(grid.PageIndex + 1);

                    // Ubica los 9 botones hacia la izquierda
                    for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                    {
                        CantBucles++;
                        if (i >= 0)
                        {
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 - CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                        }

                    }

                    CantBucles = 0;
                    // Ubica los 9 botones hacia la derecha
                    for (int i = grid.PageIndex + 1; i <= grid.PageIndex + 9; i++)
                    {
                        CantBucles++;
                        if (i <= grid.PageCount - 1)
                        {
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 + CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                        }
                    }



                }
                LinkButton cmdPage;
                string btnPage = "";
                for (int i = 1; i <= 19; i++)
                {
                    btnPage = "cmdPage" + i.ToString();
                    cmdPage = (LinkButton)fila.Cells[0].FindControl(btnPage);
                    if (cmdPage != null)
                        cmdPage.CssClass = "btn";

                }


                // busca el boton por el texto para marcarlo como seleccionado
                string btnText = Convert.ToString(grid.PageIndex + 1);
                foreach (Control ctl in fila.Cells[0].FindControl("pnlpager").Controls)
                {
                    if (ctl is LinkButton)
                    {
                        LinkButton btn = (LinkButton)ctl;
                        if (btn.Text.Equals(btnText))
                        {
                            btn.CssClass = "btn btn-inverse";
                        }
                    }
                }

            }
        }


        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();


            int aux = 0;
            int idTipoTramite = 0;

            int.TryParse(ddlTipoTramiteBaja.SelectedItem.Value, out aux);
            idTipoTramite = aux;

            if (idTipoTramite == 0)
            {
                throw new Exception("Debe seleccionar un tipo de tramite.");
            }
            else
            {
                try
                {
                    int id_solicitud = int.Parse(txtSolicitud.Text);
                    var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    var tr = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    var cp = db.CPadron_Solicitudes.FirstOrDefault(x => x.id_cpadron == id_solicitud);
                    if (sol == null && tr == null && cp == null)
                        throw new Exception("No se encontro el trámite.");
                    else if ((sol != null && sol.id_estado == (int)Constants.Solicitud_Estados.BajaAdm) ||
                        (tr != null && tr.id_estado == (int)Constants.Solicitud_Estados.BajaAdm) ||
                        (cp != null && cp.id_estado == (int)Constants.Solicitud_Estados.BajaAdm))
                        throw new Exception("El trámite ya fue dada de baja.");

                    List<SGI_Tramites_Tareas> tareas = (from tt in db.SGI_Tramites_Tareas
                                                        join th in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals th.id_tramitetarea
                                                        where th.id_solicitud == id_solicitud
                                                        select tt).Union(
                                                        from tt in db.SGI_Tramites_Tareas
                                                        join th in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals th.id_tramitetarea
                                                        where th.id_solicitud == id_solicitud
                                                        select tt).Union(
                                                        from tt in db.SGI_Tramites_Tareas
                                                        join th in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals th.id_tramitetarea
                                                        where th.id_cpadron == id_solicitud
                                                        select tt
                                                        ).OrderByDescending(x => x.id_tramitetarea).ToList();

                    // Baja de Permisos
                    if ((sol != null && sol.id_tipotramite == (int)Constants.TipoDeTramite.Permiso))
                    {
                        if (sol.id_estado != (int)Constants.Solicitud_Estados.Aprobada)
                            throw new Exception("No se puede dar de baja el trámite, el mismo no se encuentra aprobado.");

                        SGI.Webservices.ws_interface_AGC.ws_Interface_AGC service = new SGI.Webservices.ws_interface_AGC.ws_Interface_AGC();
                        SGI.Webservices.ws_interface_AGC.wsResultado wsResultado = new SGI.Webservices.ws_interface_AGC.wsResultado();
                        service.Url = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC");
                        string userService = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.User");
                        string passService = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.Password");
                        string username_AGC = Functions.GetUserName();

                        var DatosAdic = db.SSIT_Permisos_DatosAdicionales.FirstOrDefault(x => x.IdSolicitud == id_solicitud);
                        if (DatosAdic == null)
                            throw new Exception("No se encontraron los datos adicionales del Permiso.");

                        service.BajaRACMusicaCanto(userService, passService, DatosAdic.id_rac, DateTime.Now, txtObservaciones.Text.Trim(), username_AGC, ref wsResultado);

                        if (wsResultado.ErrorCode != 0)
                            throw new Exception("Servicio de SIPSA: " + wsResultado.ErrorCode + " - " + wsResultado.ErrorDescription);

                        using (TransactionScope Tran = new TransactionScope())
                        {
                            try
                            {

                                List<DocumentosAdjuntosDTO> listDoc = GetDocumentosCargados();
                                foreach (var doc in listDoc)
                                {
                                    ObjectParameter id = new ObjectParameter("id_docadjunto", typeof(int));
                                    this.db.SSIT_DocumentosAdjuntos_Add(id_solicitud, doc.id_tdocreq, doc.tdocreq_detalle, (int)Constants.TiposDeDocumentosSistema.DOC_ADJUNTO_SSIT, false, doc.id_file, doc.nombre_archivo, userid, id);
                                }

                                int id_tipo_motivo_baja = Convert.ToInt32(ddlMotivoBaja.SelectedValue) != -1 ? Convert.ToInt32(ddlMotivoBaja.SelectedValue) : 4; //4 - Otros

                                db.SSIT_Solicitudes_ActualizarEstado(id_solicitud, (int)Constants.Solicitud_Estados.BajaAdm, userid, null, null);
                                db.SSIT_Solicitudes_Baja_Agregar(id_solicitud, id_tipo_motivo_baja, DateTime.Now, txtObservaciones.Text.Trim(), userid);


                                Tran.Complete();
                                txtSolicitud.Text = "";
                                Enviar_Mensaje("La solicitud se dio de baja correctamente.", "");
                                this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
                                ScriptManager.RegisterStartupScript(updBaja, updBaja.GetType(), "showBusqued", "showBusqueda();", true);

                            }
                            catch (Exception ex)
                            {
                                string mensaje = Functions.GetErrorMessage(ex);
                                Enviar_Mensaje(mensaje, "");
                                Tran.Dispose();
                            }
                        }

                    }
                    else
                    {
                        if (tareas.Count < 2)
                            throw new Exception("No se puede dar de baja el trámite.");
                        this.id_solicitud = id_solicitud;
                        var ultima = tareas.First();
                        //List<SGI_Tramites_Tareas> tareasSinCerrar = tareas.FindAll(x => x.FechaCierre_tramitetarea == null);

                       // if (tareasSinCerrar.Count > 1 )
                          //  throw new Exception("No se puede dar de baja el trámite. Posee varias tareas sin cerrar.");

                        var tareasacerrar = tareas.Where(x => x.FechaCierre_tramitetarea == null).ToList();

                        //obtengo la tarea de baja
                        int id_circuito = ultima.ENG_Tareas.id_circuito;
                        int codtf = 0;
                        //circuito 7 tiene una tarea de baja especifica
                        if (id_circuito == 7)
                            codtf = 745;
                        else
                            codtf = id_circuito * 100 + 33;
                        var tfin = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == codtf);
                        bool cargarProcesos = false;
                        int id_tramitetarea_nuevo = 0;
                        using (TransactionScope Tran = new TransactionScope())
                        {
                            try
                            {  
                                //Creo la tarea igual a la anterir a Fin de Tramite
                                db.SaveChanges();
                                ObjectParameter param_id_tramitetarea1 = new ObjectParameter("id_tramitetarea_nuevo", typeof(int));
                                ObjectParameter param_id_tramitetarea = new ObjectParameter("id_tramitetarea_nuevo", typeof(int));

                                //cerrar tareas abiertas
                                foreach (var tarea in tareasacerrar)
                                {
                                    if(tarea.id_tramitetarea != ultima.id_tramitetarea)
                                        db.ENG_Finalizar_Tarea(tarea.id_tramitetarea, 0, 0, userid, param_id_tramitetarea1);
                                }

                                db.ENG_Finalizar_Tarea(ultima.id_tramitetarea, 0, tfin.id_tarea, userid, param_id_tramitetarea);
                                id_tramitetarea_nuevo = Convert.ToInt32(param_id_tramitetarea.Value);
                                db.ENG_Asignar_Tarea(id_tramitetarea_nuevo, userid);
                              
                                List<DocumentosAdjuntosDTO> listDoc = GetDocumentosCargados();
                                foreach (var doc in listDoc)
                                {
                                    ObjectParameter id = new ObjectParameter("id_docadjunto", typeof(int));
                                    if (sol != null)
                                        this.db.SSIT_DocumentosAdjuntos_Add(id_solicitud, doc.id_tdocreq, doc.tdocreq_detalle, (int)Constants.TiposDeDocumentosSistema.DOC_ADJUNTO_SSIT, false, doc.id_file, doc.nombre_archivo, userid, id);
                                    else if (tr != null)
                                    {
                                        this.db.Transf_DocumentosAdjuntos_Agregar(id_solicitud, doc.id_tdocreq, doc.tdocreq_detalle, (int)Constants.TiposDeDocumentosSistema.DOC_ADJUNTO_TRANSFERENCIA, false, doc.id_file, doc.nombre_archivo, userid, (int)Constants.NivelesDeAgrupamiento.General, id);
                                    }
                                    else
                                    {
                                        this.db.CPadron_DocumentosAdjuntos_Agregar(id_solicitud, doc.id_tdocreq, doc.tdocreq_detalle, (int)Constants.TiposDeDocumentosSistema.DOC_ADJUNTO_CPADRON, false, doc.id_file, doc.nombre_archivo, userid, id);
                                    }
                                }
                                int id_tipo_motivo_baja = Convert.ToInt32(ddlMotivoBaja.SelectedValue) != -1 ? Convert.ToInt32(ddlMotivoBaja.SelectedValue) : 4; //4 - Otros
                                if (sol != null)
                                {
                                    byte[] documento = PdfSolicitudBaja.GenerarPDF(id_solicitud, ddlMotivoBaja.SelectedItem.Text, txtObservaciones.Text);

                                    int id_file = ws_FilesRest.subirArchivo("SolicitudBaja.pdf", documento);

                                    int id_tipodocsis = (int)Constants.TiposDeDocumentosSistema.DOC_ADJUNTO_SSIT;
                                    ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                                    db.SSIT_DocumentosAdjuntos_Add(id_solicitud, (int)Constants.TiposDeDocumentosRequeridos.Otros, "Informe de Baja",
                                        id_tipodocsis, true, id_file, "SolicitudBaja.pdf", userid, param_id_docadjunto);
                                    Mailer.MailMessages.SendMail_BajaSolicitud_v2(id_solicitud);

                                    db.SSIT_Solicitudes_ActualizarEstado(id_solicitud, (int)Constants.Solicitud_Estados.BajaAdm, userid, null, null);
                                    db.SSIT_Solicitudes_Baja_Agregar(id_solicitud, id_tipo_motivo_baja, DateTime.Now, txtObservaciones.Text, userid);
                                }
                                else if (tr != null)
                                {
                                    byte[] documento = PdfSolicitudBaja.Transf_GenerarPDF(id_solicitud, ddlMotivoBaja.SelectedItem.Text, txtObservaciones.Text);

                                    int id_file = ws_FilesRest.subirArchivo("SolicitudBaja.pdf", documento);

                                    int id_tipodocsis = (int)Constants.TiposDeDocumentosSistema.DOC_ADJUNTO_TRANSFERENCIA;
                                    ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                                    db.Transf_DocumentosAdjuntos_Agregar(id_solicitud, (int)Constants.TiposDeDocumentosRequeridos.Otros, "Informe de Baja",
                                        id_tipodocsis, true, id_file, "SolicitudBaja.pdf", userid, (int)Constants.NivelesDeAgrupamiento.General, param_id_docadjunto);
                                    Mailer.MailMessages.SendMail_BajaSolicitud_Transf_v2(id_solicitud);

                                    //db.Transf_Solicitudes_ActualizarEstado(id_solicitud, , userid);
                                    tr.id_estado = (int)Constants.Solicitud_Estados.BajaAdm;
                                    tr.LastUpdateDate = DateTime.Now;
                                    tr.LastUpdateUser = userid;
                                    db.SaveChanges();
                                    db.Transf_Solicitudes_Baja_Agregar(id_solicitud, id_tipo_motivo_baja, DateTime.Now, txtObservaciones.Text, userid);
                                }
                                else
                                {
                                    byte[] documento = PdfSolicitudBaja.CPadron_GenerarPDF(id_solicitud, ddlMotivoBaja.SelectedItem.Text, txtObservaciones.Text);

                                    int id_file = ws_FilesRest.subirArchivo("SolicitudBaja.pdf", documento);

                                    int id_tipodocsis = (int)Constants.TiposDeDocumentosSistema.DOC_ADJUNTO_CPADRON;
                                    ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                                    db.CPadron_DocumentosAdjuntos_Agregar(id_solicitud, (int)Constants.TiposDeDocumentosRequeridos.Otros, "Informe de Baja",
                                        id_tipodocsis, true, id_file, "SolicitudBaja.pdf", userid, param_id_docadjunto);
                                    Mailer.MailMessages.SendMail_BajaSolicitud_CPadron_v2(id_solicitud);

                                    //db.Transf_Solicitudes_ActualizarEstado(id_solicitud, , userid);
                                    cp.id_estado = (int)Constants.CPadron_EstadoSolicitud.BajaAdm;
                                    cp.LastUpdateDate = DateTime.Now;
                                    cp.LastUpdateUser = userid;
                                    db.SaveChanges();
                                    db.CPadron_Solicitudes_Baja_Agregar(id_solicitud, id_tipo_motivo_baja, DateTime.Now, txtObservaciones.Text, userid);
                                }

                                //Si tiene proceso en sade subo los doc
                                int id_paquete = (from p in db.SGI_Tarea_Generar_Expediente_Procesos
                                                  join tt in db.SGI_Tramites_Tareas on p.id_tramitetarea equals tt.id_tramitetarea
                                                  join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                                  where tt_hab.id_solicitud == id_solicitud
                                                  select p.id_paquete)
                                    .Union(
                                       from p in db.SGI_SADE_Procesos
                                       join tt in db.SGI_Tramites_Tareas on p.id_tramitetarea equals tt.id_tramitetarea
                                       join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                       where tt_hab.id_solicitud == id_solicitud
                                       select p.id_paquete).FirstOrDefault();

                                if (id_paquete != 0)
                                {
                                    db.SGI_Tarea_Fin_Tramite_GenerarProcesos(id_tramitetarea_nuevo, id_paquete, userid);
                                    cargarProcesos = true;
                                    ucProcesosSADE.Visible = true;
                                }

                                //Creo la tarea fin de tramite cerrada

                                int codtfTramite = id_circuito * 100 + 29;
                                var tFinTramite = db.ENG_Tareas.FirstOrDefault(x => x.cod_tarea == codtfTramite);

                                db.ENG_Finalizar_Tarea(id_tramitetarea_nuevo, 0, tFinTramite.id_tarea, userid, param_id_tramitetarea);
                                db.ENG_Finalizar_Tarea((int)param_id_tramitetarea.Value, 0, 0, userid, param_id_tramitetarea);

                                txtSolicitud.Text = "";
                                Enviar_Mensaje("La solicitud se dio de baja correctamente.", "");
                                this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
                                ScriptManager.RegisterStartupScript(updBaja, updBaja.GetType(), "showBusqueda", "showBusqueda();", true);

                                Tran.Complete();
                                Tran.Dispose();
                            }
                            catch (Exception ex)
                            {
                                string mensaje = Functions.GetErrorMessage(ex);
                                Enviar_Mensaje(mensaje, "");
                                Tran.Dispose();
                            }
                            if (cargarProcesos)
                            {
                                if (sol != null)
                                    ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;
                                else if (tr != null)
                                {
                                    ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.TR;
                                }
                                else
                                {
                                    ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.CP;
                                }
                                ucProcesosSADE.cargarDatosProcesos(id_tramitetarea_nuevo, true);
                            }
                        }
                    }
                    var obj = sol != null ? sol.id_solicitud : tr != null ? tr.id_solicitud : cp != null ? cp.id_cpadron : 0;
                    Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "I", 3010);

                }

                catch (Exception ex)
                {
                    string mensaje = Functions.GetErrorMessage(ex);
                    Enviar_Mensaje(mensaje, "");
                    //LogError.Write(ex);
                    //lblError.Text = Functions.GetErrorMessage(ex);
                    //this.EjecutarScript(updBotonesGuardar, "showfrmError();");
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        private int id_solicitud
        {
            get
            {
                int ret = 0;
                ret = (ViewState["_id_solicitud"] != null ? Convert.ToInt32(ViewState["_id_solicitud"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_id_solicitud"] = value;
            }

        }
        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            IniciarEntity();
            Guid userid = Functions.GetUserId();
            List<SGI_Tramites_Tareas> tareas = (from tt in db.SGI_Tramites_Tareas
                                                join th in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals th.id_tramitetarea
                                                where th.id_solicitud == this.id_solicitud
                                                orderby tt.id_tramitetarea descending
                                                select tt).Union(
                                    from tt in db.SGI_Tramites_Tareas
                                    join th in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals th.id_tramitetarea
                                    where th.id_solicitud == this.id_solicitud
                                    orderby tt.id_tramitetarea descending
                                    select tt
                                    ).ToList();
            var ultima = tareas.First();
            bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == ultima.id_tramitetarea) > 0;
            if (Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(ultima.id_tramitetarea))
            {
                ObjectParameter param_id_tramitetarea = new ObjectParameter("id_tramitetarea_nuevo", typeof(int));
                db.ENG_Finalizar_Tarea(ultima.id_tramitetarea, 0, 0, userid, param_id_tramitetarea);
                db.SaveChanges();
                Enviar_Mensaje("La solicitud se dio de baja correctamente.", "");
                this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
                ScriptManager.RegisterStartupScript(updBaja, updBaja.GetType(), "showBusqued", "showBusqueda();", true);
            }
            FinalizarEntity();
        }

        protected void ddlTipoTramite_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniciarEntity();
            int idTipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);
            FinalizarEntity();
            updpnlBuscar.Update();
        }
    }
}