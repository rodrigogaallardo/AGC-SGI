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
using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;

namespace SGI.GestionTramite.Controls
{
    public partial class ucSGI_DocumentoAdjunto : System.Web.UI.UserControl
    {
        #region cargar inicial

        private DGHP_Entities db = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlInputDatosArch, updPnlInputDatosArch.GetType(), "init_Js_updPnlInputDatosArch", "init_Js_updPnlInputDatosArch();", true);
                ScriptManager.RegisterStartupScript(upd_ddl_tipo_doc, upd_ddl_tipo_doc.GetType(), "init_Js_upd_ddl_tipo_doc", "init_Js_upd_ddl_tipo_doc();", true);
            }
        }

        public void ocultarDocAdjColumnaEliminar(bool b)
        {
            grd_doc_adj.Columns[3].Visible = !b;
            ddl_tipo_doc.Visible = !b;
            div_input_upload.Visible = !b;
            lbl_tipo_doc.Visible = !b;
        }

        public void LoadData(int id_solicitud, int id_tramitetarea)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea);
        }

        public void LoadData(int id_grupotramite, int id_solicitud, int id_tramitetarea)
        {
            try
            {
                this.id_solicitud = id_solicitud;
                this.id_tramitetarea = id_tramitetarea;
                this.id_grupotramite = id_grupotramite;
                this.db = new DGHP_Entities();
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
                LimpiarCargarArchivo();
                Cargar_tipo_documentos_adjuntos();
                Cargar_documentos_adjuntos(false);
                this.db.Dispose();
            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                throw ex;
            }

        }

        private void Cargar_tipo_documentos_adjuntos()
        {

            List<TiposDeDocumentosRequeridos> list_doc_adj =
                (
                    from tdoc in db.TiposDeDocumentosRequeridos
                    join tdoc_tarea in db.Rel_TiposDeDocumentosRequeridos_ENG_Tareas on tdoc.id_tdocreq equals tdoc_tarea.id_tdocreq
                    join tt in db.SGI_Tramites_Tareas on tdoc_tarea.id_tarea equals tt.id_tarea
                    where tt.id_tramitetarea == this.id_tramitetarea
                    select tdoc
                ).ToList();



            if (list_doc_adj.Count > 1)
                list_doc_adj.Insert(0, (new TiposDeDocumentosRequeridos
                {
                    id_tdocreq = 0,
                    nombre_tdocreq = "Seleccione",
                    baja_tdocreq = false,
                    RequiereDetalle = false,
                    acronimo_SADE = "",
                    formato_archivo = "pdf",
                    tamanio_maximo_mb = 0
                }));

            ddl_tipo_doc.DataValueField = "id_tdocreq";
            ddl_tipo_doc.DataTextField = "nombre_tdocreq";

            ddl_tipo_doc.DataSource = list_doc_adj;
            ddl_tipo_doc.DataBind();
        }

        private List<SGI_Tarea_Documentos_Adjuntos> Cargar_DocumentosAdjuntos(bool cargar_anteriores)
        {

            SGI_Tramites_Tareas tareaActual = this.db.SGI_Tramites_Tareas.Where(x => x.id_tramitetarea == this.id_tramitetarea).FirstOrDefault();

            int[] list_id_tramitetarea = null;
            if (cargar_anteriores)
            {
                if (this.id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                    list_id_tramitetarea = db.SGI_Tramites_Tareas_HAB.Where //buscar todos los id_tramitetarea de la misma solicitud
                            (x => x.id_solicitud == this.id_solicitud &&
                                x.SGI_Tramites_Tareas.id_tarea == tareaActual.id_tarea &&
                                x.id_tramitetarea <= tareaActual.id_tramitetarea
                            ).Select(x => x.id_tramitetarea).ToArray();
                else if (this.id_grupotramite == (int)Constants.GruposDeTramite.CP)
                    list_id_tramitetarea = db.SGI_Tramites_Tareas_CPADRON.Where //buscar todos los id_tramitetarea de la misma solicitud
                            (x => x.id_cpadron == this.id_solicitud &&
                                x.SGI_Tramites_Tareas.id_tarea == tareaActual.id_tarea &&
                                x.id_tramitetarea <= tareaActual.id_tramitetarea
                            ).Select(x => x.id_tramitetarea).ToArray();
                else if (this.id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                    list_id_tramitetarea = db.SGI_Tramites_Tareas_TRANSF.Where //buscar todos los id_tramitetarea de la misma solicitud
                            (x => x.id_solicitud == this.id_solicitud &&
                                x.SGI_Tramites_Tareas.id_tarea == tareaActual.id_tarea &&
                                x.id_tramitetarea <= tareaActual.id_tramitetarea
                            ).Select(x => x.id_tramitetarea).ToArray();

            }
            else
                list_id_tramitetarea = new int[1] { id_tramitetarea };

            List<TiposDeDocumentosRequeridos> list_tipo_doc = this.db.TiposDeDocumentosRequeridos.ToList();

            var list_doc_adj =
                (
                    from adj in db.SGI_Tarea_Documentos_Adjuntos
                    where list_id_tramitetarea.Contains(adj.id_tramitetarea)  // adj.id_tramitetarea IN ( array de id_tramitetarea)
                    select new
                    {
                        id_doc_adj = adj.id_doc_adj,
                        id_tramitetarea = adj.id_tramitetarea,
                        id_tdocreq = adj.id_tdocreq,
                        tdoc_adj_detalle = adj.tdoc_adj_detalle,
                        CreateDate = adj.CreateDate,
                        CreateUser = adj.CreateUser
                    }
                ).ToList();


            List<SGI_Tarea_Documentos_Adjuntos> list_doc_adj_nuevo = new List<SGI_Tarea_Documentos_Adjuntos>();
            SGI_Tarea_Documentos_Adjuntos doc_adj = null;
            foreach (var item in list_doc_adj)
            {

                doc_adj = new SGI_Tarea_Documentos_Adjuntos();

                doc_adj.id_doc_adj = item.id_doc_adj;
                doc_adj.id_tramitetarea = item.id_tramitetarea;
                doc_adj.id_tdocreq = item.id_tdocreq;
                doc_adj.tdoc_adj_detalle = item.tdoc_adj_detalle;
                doc_adj.CreateDate = item.CreateDate;
                doc_adj.CreateUser = item.CreateUser;

                if (string.IsNullOrEmpty(item.tdoc_adj_detalle))
                {
                    var tipo_doc = list_tipo_doc.FirstOrDefault(x => x.id_tdocreq == item.id_tdocreq);
                    doc_adj.tdoc_adj_detalle = tipo_doc.nombre_tdocreq;
                }

                list_doc_adj_nuevo.Add(doc_adj);

            }

            return list_doc_adj_nuevo;
        }

        private void Cargar_documentos_adjuntos(bool cargar_anteriores)
        {
            List<SGI_Tarea_Documentos_Adjuntos> list_doc_adj_nuevo = Cargar_DocumentosAdjuntos(cargar_anteriores);

            grd_doc_adj.DataSource = list_doc_adj_nuevo;
            grd_doc_adj.DataBind();

        }

        protected void grd_doc_adj_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkEliminar = (LinkButton)e.Row.FindControl("lnkEliminarDocAdj");
                lnkEliminar.Visible = this.Enabled;
                int id_tdocreq = int.Parse(grd_doc_adj.DataKeys[e.Row.RowIndex].Values["id_tdocreq"].ToString());
                TiposDeDocumentosRequeridos tipo = db.TiposDeDocumentosRequeridos.FirstOrDefault(x => x.id_tdocreq == id_tdocreq);
                if (tipo.formato_archivo.Equals("jpg"))
                {
                    HyperLink lnkVerDoc = (HyperLink)e.Row.FindControl("lnkVerDoc");
                    lnkVerDoc.CssClass = "btnVerJpg20";
                }
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
                ddl_tipo_doc.Enabled = _Enabled;
                txtDetalle.Enabled = _Enabled;

                //Tendria q desabilitar el boton
                aspPanelBoton.Visible = _Enabled;
            }
        }

        #endregion

        #region Eventos

        public delegate void EventHandlerGuardar(object sender, ucSGI_DocumentoAdjuntoEventsArgs e);
        public event EventHandlerGuardar GuardarDocumentoAdjuntoClick;

        public delegate void EventHandlerEliminar(object sender, ucSGI_DocumentoAdjuntoEventsArgs e);
        public event EventHandlerEliminar EliminarDocumentoAdjuntoClick;

        protected void lnkEliminarDocAdj_Command(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkEliminar = (LinkButton)sender;
                int id_doc_adj = Convert.ToInt32(lnkEliminar.CommandArgument);
                //GridViewRow row = (GridViewRow)lnkEliminar.Parent.Parent;
                this.db = new DGHP_Entities();

                this.db.SGI_Tarea_Documentos_Adjuntos_Eliminar(id_doc_adj);

                if (this.EliminarDocumentoAdjuntoClick != null)
                {
                    ucSGI_DocumentoAdjuntoEventsArgs args = new ucSGI_DocumentoAdjuntoEventsArgs();
                    args.id_doc_adj = id_doc_adj;
                    this.EliminarDocumentoAdjuntoClick(this, args);
                }

                Cargar_documentos_adjuntos(false);
            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();

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
                this.db = new DGHP_Entities();

                Byte[] pdfBytes = cargarPDF(this.NombreArchivoFisico);

                ValidarDocumentoAdjunto(pdfBytes);

                using (TransactionScope Tran = new TransactionScope())
                {

                    try
                    {
                        ObjectParameter id = new ObjectParameter("id_doc_adj", typeof(int));
                        int id_file = ws_FilesRest.subirArchivo(this.NombreArchivoFisico, pdfBytes);
                        this.db.SGI_Tarea_Documentos_Adjuntos_Agregar(this.id_tramitetarea, this.tipoDoc, this.tipoDocDetalle, id_file, this.NombreArchivoFisico, userid, id);
                        id_doc_adj = Convert.ToInt32(id.Value);
                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. ucSGI_DocumentoAdjunto-btnComenzarCargaArchivo_Click");
                        throw ex;
                    }
                }

                if (this.GuardarDocumentoAdjuntoClick != null)
                {
                    ucSGI_DocumentoAdjuntoEventsArgs args = new ucSGI_DocumentoAdjuntoEventsArgs();
                    args.id_doc_adj = id_doc_adj;
                    this.GuardarDocumentoAdjuntoClick(this, args);
                }


                Cargar_documentos_adjuntos(false);

                this.db.Dispose();

                string mensaje = "El documento \"" + this.NombreArchivoOriginal + "\" se ha adjuntado correctamente.";
                mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
                string script = string.Format("tda_mostrar_mensaje('{0}','{1}');", mensaje, "Aviso");
                ScriptManager.RegisterStartupScript(updPnlInputDatosArch, updPnlInputDatosArch.GetType(), "tda_mostrar_mensaje", script, true);

            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();

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

        private string changeSpecialChars(string str)
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
                    _nombreArchivoFisico = Constants.PathTemporal + this.RandomArchivo + changeSpecialChars(hid_filename.Value);
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
                    _nombreArchivoOriginal = hid_filename.Value;
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
                    _randomArchivo = hid_filename_random.Value;
                return _randomArchivo;
            }
            set
            {
                _randomArchivo = value;
            }
        }

        private bool IsPdfFullPermissions(string documento)
        {
            bool ret = true;
            try
            {
                PdfReader pdfReader = new PdfReader(documento);
                pdfReader.Dispose();

                if (!pdfReader.IsOpenedWithFullPermissions)
                    ret = false;


            }
            catch (BadPasswordException)
            {
                ret = false;
            }

            return ret;

        }

        private void ValidarDocumentoAdjunto(Byte[] pdfBytes)
        {
            this.NombreArchivoFisico = "";
            this.NombreArchivoOriginal = "";
            this.RandomArchivo = "";

            this.tipoDoc = 0;
            this.tipoDocDetalle = "";

            string archFisico = this.NombreArchivoFisico;
            string archOriginal = this.NombreArchivoOriginal;

            if (ddl_tipo_doc.SelectedValue.Equals("0"))
                throw new ucSGI_DocumentoAdjuntoException("Debe seleccionar el tipo de documento.");

            if (string.IsNullOrEmpty(archFisico))
            {
                throw new ucSGI_DocumentoAdjuntoException("Debe seleccionar el archivo.");
            }

            if (!System.IO.File.Exists(archFisico))
            {
                throw new ucSGI_DocumentoAdjuntoException("Debe seleccionar el archivo, el documento no fue recepcionado por el sistema.");
            }

            /*if (!archOriginal.EndsWith(".pdf", false, null))
                throw new ucSGI_DocumentoAdjuntoException("Sólo se pueden adjuntar Documentos '.PDF'.");*/

            int tipoDoc = 0;
            if (!int.TryParse(ddl_tipo_doc.SelectedValue, out tipoDoc))
                throw new ucSGI_DocumentoAdjuntoException("Tipo de Documento invalido.");

            TiposDeDocumentosRequeridos doc_adj =
                this.db.TiposDeDocumentosRequeridos.Where(x => x.id_tdocreq == tipoDoc).FirstOrDefault();

            if (doc_adj == null)
                throw new ucSGI_DocumentoAdjuntoException("El tipo de documento no existe.");

            if (doc_adj.RequiereDetalle && string.IsNullOrEmpty(txtDetalle.Text))
                throw new ucSGI_DocumentoAdjuntoException("Debe ingresar el detalle para este tipo de documento.");

            this.tipoDoc = tipoDoc;
            this.tipoDocDetalle = (doc_adj.RequiereDetalle) ? txtDetalle.Text : "";

            if (doc_adj.formato_archivo.ToLower() == Constants.EXTENSION_PDF)
            {
                bool isFirmando = Functions.isFirmadoPdf(pdfBytes);
                if (doc_adj.verificar_firma_digital && !isFirmando)
                    throw new Exception("El Tipo de documento seleccionado requiere que el documento este firmado.");
                else if (!doc_adj.verificar_firma_digital && isFirmando)
                    throw new Exception("El Tipo de documento seleccionado requiere que el documento no este firmado.");

                if (!IsPdfFullPermissions(archFisico))
                    throw new Exception("El documento esta protegido con contraseña o bien tiene un nivel de seguridad no permitido. Por favor verifique las propiedades del PDF.");
            }

            //chequeo planos visados
            if (tipoDoc == (int)Constants.TiposDeDocumentosRequeridos.Plano_Visado)
            {
                var doc = db.Solicitud_planoVisado.Where(x => x.id_solicitud == this.id_solicitud &&
                                                            x.id_tramiteTarea == this.id_tramitetarea).ToList();
                if (doc == null)
                    throw new Exception("Debe Seleccionar el/los plano/s visado/s.");
            }

            //Chequeo los archivos JPEG 
            if (doc_adj.formato_archivo.ToLower() == Constants.EXTENSION_JPG)
            {
                bool isJpeg;
                using (System.Drawing.Image test = System.Drawing.Image.FromFile(archFisico))
                {
                    isJpeg = (test.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg));
                }
                if (!isJpeg)
                    throw new Exception("El formato del archivo no es correcto.");
            }
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

                hid_formato_archivo.Value = TipoDoc.formato_archivo;
                hid_size_max.Value = Convert.ToString(TipoDoc.tamanio_maximo_mb * 1024 * 1024);

                val_upload_fileupload_size.Text = string.Format("El tamaño máximo permitido es de {0} MB", TipoDoc.tamanio_maximo_mb);
                hid_val_upload_fileupload_size.Value = string.Format("El tamaño máximo permitido es de {0} MB", TipoDoc.tamanio_maximo_mb);
               
                db.Dispose();
            }
        }

    }
}

public class ucSGI_DocumentoAdjuntoEventsArgs : EventArgs
{
    public int id_doc_adj { get; set; }
}

public class ucSGI_DocumentoAdjuntoException : Exception
{
    public ucSGI_DocumentoAdjuntoException(string mensaje)
        : base(mensaje, new Exception())
    {
    }
}

