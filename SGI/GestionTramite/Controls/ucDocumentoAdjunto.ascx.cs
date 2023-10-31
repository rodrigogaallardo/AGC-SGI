using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using System.Data.Entity.Core.Objects;
using SGI.WebServices;

namespace SGI.GestionTramite.Controls
{
    public partial class ucDocumentoAdjunto : System.Web.UI.UserControl
    {
        #region cargar inicial

        private AGC_FilesEntities dbFiles = null;
        private DGHP_Entities db = null;
        private class itemGrillaFiles
        {
            public int id_file { get; set; }
            public int id_docadjunto { get; set; }
            public string nombre_tdocreq { get; set; }
            public DateTime CreateDate { get; set; }
            public string url { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlInputDatosArch, updPnlInputDatosArch.GetType(), "init_Js_updPnlInputDatosArch", "init_Js_updPnlInputDatosArch();", true);
                ScriptManager.RegisterStartupScript(upd_ddl_tipo_doc, upd_ddl_tipo_doc.GetType(), "init_Js_upd_ddl_tipo_doc", "init_Js_upd_ddl_tipo_doc();", true);
                
            }
        }

        public void LoadData(int id_solicitud, int id_tramitetareaa)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea);
        }

        public void LoadData(int id_grupotramite, int id_solicitud, int id_tramitetarea)
        {
            this.dbFiles = new AGC_FilesEntities();
            this.db = new DGHP_Entities();
            
            try
            {
                this.id_solicitud = id_solicitud;
                this.id_tramitetarea = id_tramitetarea;
                this.id_grupotramite = id_grupotramite;
                if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                {
                    var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    var enc = ( from enco in db.Encomienda
                                join rel in db.Encomienda_SSIT_Solicitudes on enco.id_encomienda equals rel.id_encomienda
                                where enco.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                                orderby enco.id_encomienda descending
                                select enco
                            ).FirstOrDefault();


                    this.id_encomienda = enc.id_encomienda;
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
                    this.id_encomienda = 0;
                    this.id_tipotramite = sol.id_tipotramite;
                }
                else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
                {
                    this.id_tipotramite = 2;
                }

                LimpiarCargarArchivo();
                Cargar_tipo_documentos_adjuntos(id_tipotramite);
                Cargar_documentos_adjuntos(id_grupotramite);

                // Se oculta la columna de edición cuando el control está deshabilitado
                // --
                grdFiles.Columns[3].Visible = this.Enabled;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.dbFiles != null)
                    this.dbFiles.Dispose();
                if (this.db != null)
                    this.db.Dispose();
            }

        }

        private void Cargar_tipo_documentos_adjuntos(int id_tipotramite)
        {
            var q = (   from tdoc in db.TiposDeDocumentosRequeridos
                        join rel in db.Rel_TipoTramite_TiposDeDocumentosRequeridos on tdoc.id_tdocreq equals rel.id_tdocreq
                        where tdoc.visible_en_SGI == true && rel.id_tipotramite == id_tipotramite
                        orderby tdoc.RequiereDetalle, tdoc.nombre_tdocreq
                        select tdoc
                    );

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


            ddl_tipo_doc.DataValueField = "id_tdocreq";
            ddl_tipo_doc.DataTextField = "nombre_tdocreq";

            ddl_tipo_doc.DataSource = list_doc_adj;
            ddl_tipo_doc.DataBind();


        }


        private void Cargar_documentos_adjuntos(int id_grupotramite)
        {
            Guid userid = Functions.GetUserId();
            pnlDocumentosAdjuntos.Visible = false;
            pnlFiles.Visible = false;
            
            if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
            {
                var lstFiles = (from docadj in db.CPadron_DocumentosAdjuntos
                                join tdocreq in db.TiposDeDocumentosRequeridos on docadj.id_tdocreq equals tdocreq.id_tdocreq
                                join files in db.Files on docadj.id_file equals files.id_file
                                where docadj.id_cpadron == this.id_solicitud && docadj.CreateUser == userid
                                select new itemGrillaFiles
                                {
                                    id_file = docadj.id_file,
                                    id_docadjunto = docadj.id_docadjunto,
                                    nombre_tdocreq = tdocreq.nombre_tdocreq + (tdocreq.RequiereDetalle ? " (" + docadj.tdocreq_detalle + ")" : ""),
                                    CreateDate = docadj.CreateDate,
                                    url = ""
                                }
                            ).ToList();

                foreach (var item in lstFiles)
                {
                    string strBase64 = Functions.ConvertToBase64(item.id_file.ToString());
                    item.url = string.Format("~/GetPDF/{0}", System.Web.HttpUtility.UrlEncode(strBase64));
                }
                grdFiles.DataSource = lstFiles;
                grdFiles.DataBind();
                pnlFiles.Visible = true;


            }else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                var lstFiles = (from docadj in db.Transf_DocumentosAdjuntos
                                join tdocreq in db.TiposDeDocumentosRequeridos on docadj.id_tdocreq equals tdocreq.id_tdocreq
                                join files in db.Files on docadj.id_file equals files.id_file
                                where docadj.id_solicitud == this.id_solicitud && docadj.CreateUser == userid
                                select new itemGrillaFiles
                                {
                                    id_file = docadj.id_file,
                                    id_docadjunto = docadj.id_docadjunto,
                                    nombre_tdocreq = tdocreq.nombre_tdocreq + (tdocreq.RequiereDetalle ? " (" + docadj.tdocreq_detalle + ")" : ""),
                                    CreateDate = docadj.CreateDate,
                                    url = ""
                                }
                            ).ToList();

                foreach (var item in lstFiles)
                {
                    string strBase64 = Functions.ConvertToBase64(item.id_file.ToString());
                    item.url = string.Format("~/GetPDF/{0}", System.Web.HttpUtility.UrlEncode(strBase64));
                }
                grdFiles.DataSource = lstFiles;
                grdFiles.DataBind();
                pnlFiles.Visible = true;


            }

        }

        protected void grd_doc_adj_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkEliminar = (LinkButton)e.Row.FindControl("lnkEliminarDocAdj");
                lnkEliminar.Visible = this.Enabled;
            }
        }


        private void LimpiarCargarArchivo()
        {
            if (ddl_tipo_doc.Items.Count > 0)
                ddl_tipo_doc.SelectedIndex = 0;
            txtDetalle.Text = "";
            hid_filename.Value = "";
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
                    _titulo = "Lista de Documentos Adjuntos";
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

        private int _id_encomienda = 0;
        public int id_encomienda
        {
            get
            {
                if (_id_encomienda == 0)
                {
                    int.TryParse(hid_id_encomienda.Value, out _id_encomienda);
                }
                return _id_encomienda;
            }
            set
            {
                hid_id_encomienda.Value = value.ToString();
                _id_encomienda = value;
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

        public void setFileMaxSise(int sise_mb)
        {
            hid_size_max.Value = Convert.ToString(sise_mb * 1024 * 1024);
            val_upload_fileupload_size.Text = string.Format("El tamaño máximo permitido es de {0} MB",sise_mb);
        }

        private bool _Enabled;

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(true),
        Description("Devuelve/Establece el estado de los controles contenidos en este control.")]
        public bool Enabled
        {
            get {

                if (!bool.TryParse(hid_editable.Value, out _Enabled))
                    _Enabled = false;
                return _Enabled; 
            }
            set
            {
                _Enabled = value;
                hid_editable.Value = _Enabled.ToString().ToLower();
                //btnCargarOtroArchivo.Visible = _Enabled;
                ddl_tipo_doc.Enabled = _Enabled;
                txtDetalle.Enabled = _Enabled;
                //Tendria q desabilitar el boton
                aspPanelBoton.Visible = _Enabled;
                
            }
        }

        #endregion

        #region Eventos

        public delegate void EventHandlerGuardar(object sender, ucDocumentoAdjuntoEventsArgs e);
        public event EventHandlerGuardar GuardarDocumentoAdjuntoClick;


        public delegate void EventHandlerEliminar(object sender, ucDocumentoAdjuntoEventsArgs e);
        public event EventHandlerEliminar EliminarDocumentoAdjuntoClick;

        protected void lnkEliminarDocAdj_Command(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkEliminar = (LinkButton)sender;
                int id_doc_adj = Convert.ToInt32(lnkEliminar.CommandArgument);
                this.dbFiles = new AGC_FilesEntities();
                this.db = new DGHP_Entities();

                /*if (this.id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                {
                    this.dbFiles.Eliminar_DocumentosAdjuntos(id_doc_adj);

                }
                else*/
                if (this.id_grupotramite == (int)Constants.GruposDeTramite.CP)
                {
                    db.CPadron_DocumentosAdjuntos_Eliminar(id_doc_adj);
                }
                else if (this.id_grupotramite == (int)Constants.GruposDeTramite.TR)
                {
                    db.Transf_DocumentosAdjuntos_Eliminar(id_doc_adj);
                }

                if (this.EliminarDocumentoAdjuntoClick != null)
                {
                    ucDocumentoAdjuntoEventsArgs args = new ucDocumentoAdjuntoEventsArgs();
                    args.id_doc_adj = id_doc_adj;
                    this.EliminarDocumentoAdjuntoClick(this, args);
                }

                Cargar_documentos_adjuntos(this.id_grupotramite);

                this.dbFiles.Dispose();

            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                if (this.dbFiles != null)
                    this.dbFiles.Dispose();

                ScriptManager.RegisterClientScriptBlock(updPnlDocumentoAdjunto, updPnlDocumentoAdjunto.GetType(),
                        "tda_mostrar_mensaje", "tda_mostrar_mensaje('" + ex.Message + "','')", true);
            }

        }

        protected void btnComenzarCargaArchivo_Click(object sender, EventArgs e)
        {
            UpdatePanel updPnlInputDatosArch = (UpdatePanel)btnComenzarCargaArchivo.Parent.Parent;
            
            int id_doc_adj = 0;
            try
            {
                Guid userid = Functions.GetUserId();
                this.dbFiles = new AGC_FilesEntities();
                this.db = new DGHP_Entities();

                ValidarDocumentoAdjunto();
                
                Byte[] pdfBytes = cargarPDF(this.NombreArchivoFisico);
                
                /*if (this.id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                {
                    id_doc_adj = this.dbFiles.Actualizar_DocumentosAdjuntos(null, this.tipoDoc, id_encomienda, id_solicitud, tipoDocDetalle,
                        pdfBytes, userid.ToString(), "SGI", 0);
                }
                else*/
                if (this.id_grupotramite == (int)Constants.GruposDeTramite.CP)
                {
                    // Agrega el archivo a la tabla CPADRON_Documentos_Adjuntos
                    // --
                    int id_tipodocsis =  Functions.GetTipoDocSistema("DOC_ADJUNTO_CPADRON");
                    ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                    int id_file = ws_FilesRest.subirArchivo(this._nombreArchivoOriginal, pdfBytes);
                    db.CPadron_DocumentosAdjuntos_Agregar(this.id_solicitud, this.tipoDoc, txtDetalle.Text.Trim(), id_tipodocsis, 
                        false, id_file, this._nombreArchivoOriginal, userid, param_id_docadjunto);
                }else if (this.id_grupotramite == (int)Constants.GruposDeTramite.TR)
                {
                    // Agrega el archivo a la tabla CPADRON_Documentos_Adjuntos
                    // --
                    int id_tipodocsis = Functions.GetTipoDocSistema("DOC_ADJUNTO_TRANSFERENCIA");
                    ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                    int id_file = ws_FilesRest.subirArchivo(this._nombreArchivoOriginal, pdfBytes);
                    db.Transf_DocumentosAdjuntos_Agregar(this.id_solicitud, this.tipoDoc, txtDetalle.Text.Trim(), id_tipodocsis,
                        false, id_file, this._nombreArchivoOriginal, userid, (int)Constants.NivelesDeAgrupamiento.General,
                        param_id_docadjunto);
                }
                if (this.GuardarDocumentoAdjuntoClick != null)
                {
                    ucDocumentoAdjuntoEventsArgs args = new ucDocumentoAdjuntoEventsArgs();
                    args.id_doc_adj = id_doc_adj;
                    this.GuardarDocumentoAdjuntoClick(this, args);
                }

                Cargar_documentos_adjuntos(this.id_grupotramite);

                this.db.Dispose();
                this.dbFiles.Dispose();

                string mensaje = "El documento \"" + this.NombreArchivoOriginal + "\" se ha adjuntado correctamente.";
                mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
                string script = string.Format("tda_mostrar_mensaje('{0}','{1}');", mensaje, "Aviso");
                ScriptManager.RegisterStartupScript(updPnlInputDatosArch, updPnlInputDatosArch.GetType(), "tda_mostrar_mensaje", script, true);

            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                if (this.dbFiles != null)
                    this.dbFiles.Dispose();

                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(updPnlInputDatosArch, updPnlInputDatosArch.GetType(), "showfrmError_ucDocumentoAdjunto", "showfrmError_ucDocumentoAdjunto();", true);
                
            }

            EliminarDocumento(this.NombreArchivoFisico);
        }


        #endregion

        #region validaciones

        private MemoryStream cargarPDFMemoria()
        {
            string arch = Server.MapPath("../") + "Files\\" + hid_filename.Value;

            FileStream fileStream = File.OpenRead(arch);

            MemoryStream msDocumento = new MemoryStream();
            msDocumento.SetLength(fileStream.Length);

            fileStream.Read(msDocumento.GetBuffer(), 0, (int)fileStream.Length);

            fileStream.Close();

            return msDocumento;
        }

        private Byte[] cargarPDF(string arch)
        {
            FileStream fs = new FileStream(arch, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Byte[] pdfBytes = br.ReadBytes((Int32)fs.Length);
            br.Close();
            fs.Close();
            int max = Convert.ToInt32(hid_size_max.Value);
            int max_mb = max / 1024 / 1024;
            if (pdfBytes.Length == 0)
                throw new Exception("El documento está vacio.");


            if (pdfBytes.Length > max)
                throw new Exception(String.Format("El tamaño máximo permitido es de {0} MB", max_mb));

            return pdfBytes;
        }

        private void EliminarDocumento(string arch)
        {
            try
            {
                System.IO.File.Delete(arch);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

        }

        private int tipoDoc;
        private string tipoDocDetalle;
        //Permite cargar archivos de nombre con caracteres especiales.
        private string cambiarCharEspeciales(string str) 
        {
            str = str.Replace("ñ", "n");
            str = str.Replace("á", "a");
            str = str.Replace("Á", "A");
            str = str.Replace("é", "e");
            str = str.Replace("É", "E");
            str = str.Replace("í", "i");
            str = str.Replace("Í", "I");
            str = str.Replace("ó", "o");
            str = str.Replace("Ó", "O");
            str = str.Replace("ú", "u");
            str = str.Replace("Ú", "U");
            return str;
        }

        private string _nombreArchivoFisico;
        public string NombreArchivoFisico
        {
            get
            {
                if (string.IsNullOrEmpty(_nombreArchivoFisico))
                    _nombreArchivoFisico = Constants.PathTemporal + this.RandomArchivo + cambiarCharEspeciales(hid_filename.Value);
                return _nombreArchivoFisico;
            }
            set
            {
                _nombreArchivoFisico = value;
            }
        }

        private string _nombreArchivoOriginal;
        public string NombreArchivoOriginal
        {
            get
            {
                if (string.IsNullOrEmpty(_nombreArchivoOriginal))
                {
                    _nombreArchivoOriginal = hid_filename.Value.ToLower();
                }
                return _nombreArchivoOriginal;
            }
            set
            {
                _nombreArchivoOriginal = value;
            }

        }

        private string _randomArchivo;
        public string RandomArchivo
        {
            get
            {
                if (string.IsNullOrEmpty(_randomArchivo))
                    _randomArchivo = hid_filename_random.Value.ToLower();
                return _randomArchivo;
            }
            set
            {
                _randomArchivo = value;
            }
        }

        private void ValidarDocumentoAdjunto()
        {
            this.NombreArchivoFisico = "";
            this.NombreArchivoOriginal = "";
            this.RandomArchivo = "";

            this.tipoDoc = 0;
            this.tipoDocDetalle = "";

            string archFisico = this.NombreArchivoFisico;
            string archOriginal = this.NombreArchivoOriginal;

            if (ddl_tipo_doc.SelectedValue.Equals("0"))
                throw new ucDocumentoAdjuntoException("Debe seleccionar el tipo de documento.");

            if (string.IsNullOrEmpty(archFisico))
            {
                throw new ucDocumentoAdjuntoException("Debe seleccionar el archivo.");
            }

            if (!System.IO.File.Exists(archFisico))
            {
                throw new ucDocumentoAdjuntoException("Debe seleccionar el archivo, el documento no fue recepcionado por el sistema.");
            }

            if (!archOriginal.EndsWith(".pdf", false, null))
                throw new ucDocumentoAdjuntoException("Sólo se pueden adjuntar Documentos '.PDF'.");

            int tipoDoc = 0;
            if (!int.TryParse(ddl_tipo_doc.SelectedValue, out tipoDoc))
                throw new ucDocumentoAdjuntoException("Tipo de Documento invalido.");

            TiposDeDocumentosRequeridos doc_adj =
                this.db.TiposDeDocumentosRequeridos.Where(x => x.id_tdocreq == tipoDoc).FirstOrDefault();

            if (doc_adj == null)
                throw new ucDocumentoAdjuntoException("El tipo de documento no existe.");

            if (doc_adj.RequiereDetalle && string.IsNullOrEmpty(txtDetalle.Text))
                throw new ucDocumentoAdjuntoException("Debe ingresar el detalle para este tipo de documento.");

            this.tipoDoc = tipoDoc;
            this.tipoDocDetalle = (doc_adj.RequiereDetalle) ? txtDetalle.Text : "";

        }
        #endregion

        protected void ddl_tipo_doc_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id_tdocreq = 0;
            if (int.TryParse(ddl_tipo_doc.SelectedValue, out id_tdocreq))
            {
                DGHP_Entities db = new DGHP_Entities();
                var TipoDoc = db.TiposDeDocumentosRequeridos.FirstOrDefault(x => x.id_tdocreq == id_tdocreq);
                if (TipoDoc != null && TipoDoc.RequiereDetalle)
                    ScriptManager.RegisterStartupScript(upd_ddl_tipo_doc, upd_ddl_tipo_doc.GetType(), "showDetalleDocumento", "showDetalleDocumento();", true);
                else
                    ScriptManager.RegisterStartupScript(upd_ddl_tipo_doc, upd_ddl_tipo_doc.GetType(), "hideDetalleDocumento", "hideDetalleDocumento();", true);
                db.Dispose();
            }

        }

    }
}

public class ucDocumentoAdjuntoEventsArgs : EventArgs
{
    public int id_doc_adj { get; set; }
}

public class ucDocumentoAdjuntoException : Exception
{
    public ucDocumentoAdjuntoException(string mensaje)
        : base(mensaje, new Exception())
    {
    }
}

