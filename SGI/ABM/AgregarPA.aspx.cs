using SGI.Model;
using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.ABM
{
    public partial class AgregarPA : BasePage
    {
        DGHP_Entities db = null;

        private int[] IDs_Fin_Tramite = { (int)Constants.ENG_Tareas.ESPAR_Fin_Tramite_Nuevo,
                                      (int)Constants.ENG_Tareas.ESP_Fin_Tramite_Nuevo,
                                      (int)Constants.ENG_Tareas.SSP_Fin_Tramite_Nuevo,
                                      (int)Constants.ENG_Tareas.SCP_Fin_Tramite_Nuevo
                                    };
        #region load de pagina
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);

            }

            if (!IsPostBack)
            {
                db = new DGHP_Entities();
                Cargar_tipo_documentos_adjuntos();
            }
        }
        #endregion

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        private void LimpiarDatosBusqueda()
        {
            txtSolicitud.Text = "";
        }

        protected async void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                IniciarEntity();
                

                if (txtSolicitud.Text.Length == 0)
                    throw new Exception("Debe ingresar la solicitud.");

                int id_solicitud;
                int.TryParse(txtSolicitud.Text.Trim(), out id_solicitud);

                var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                if(sol==null)
                    throw new Exception("No se puede encontrar la solicitud.");
                /* if (Functions.isAprobado(id_solicitud))
                     throw new Exception("La solicitud debe estar rechazada.");*/

                var query =
                        (
                            from tt in db.SGI_Tramites_Tareas
                            join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                            where tt_hab.id_solicitud == id_solicitud
                            && ((tt.FechaCierre_tramitetarea == null) || (IDs_Fin_Tramite.Contains(tt.id_tarea)))
                            orderby tt.id_tramitetarea descending
                            select new
                            {
                                tt.id_tarea,
                                tt.ENG_Tareas.cod_tarea,
                                tt.id_tramitetarea,
                                tt.ENG_Tareas.id_circuito
                            }
                        ).FirstOrDefault();                

                if (query == null)
                    throw new Exception("No se puede encontrar la ultima tarea abierta.");
                
                string tipo_tarea = query.cod_tarea.ToString();
                tipo_tarea = tipo_tarea.Substring(tipo_tarea.Length - 2);
                List<string> tareas = BuscarTareas(query.id_circuito);

                int ttarea;
                int.TryParse(tipo_tarea, out ttarea);

                if (Functions.isAprobado(id_solicitud) && (ttarea < 49 || ttarea > 63))
                    throw new Exception("La solicitud debe estar rechazada o encontrarse en alguna tarea de Recurso de Reconsideración : ");

                if (tipo_tarea != Constants.ENG_Tipos_Tareas.Enviar_a_DGFyC && (ttarea < 49 || ttarea > 63))
                    throw new Exception("La solicitud debe estar en la tarea 'Enviar a DGFyC'.");

                int id_grupotramite;
                Engine.getIdGrupoTrabajo(id_solicitud, out id_grupotramite);
                this.id_solicitud = id_solicitud;
                this.id_grupotramite = id_grupotramite;
                id_tarea = query.id_tarea;
                id_tramite_tarea = query.id_tramitetarea;

                ucCabecera.LoadData(id_grupotramite, id_solicitud);
                await ucListaDocumentos.LoadData(id_grupotramite, id_solicitud, true, (int)Constants.TiposDeDocumentosSistema.PRESENTACION_A_AGREGAR);

                updResultados.Update();

                EjecutarScript(UpdatePanel1, "showResultado();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "showfrmError();");
            }
            finally
            {
                FinalizarEntity();
            }
        }

        private List<string> BuscarTareas(int idCircuito)
        {
            List<string> tareas = new List<string>();

            List<string> lstTareas = new List<string>();
            lstTareas.Add(Constants.ENG_Tipos_Tareas.Calificar3);
            lstTareas.Add(Constants.ENG_Tipos_Tareas.Revision_Gerente3);
            lstTareas.Add(Constants.ENG_Tipos_Tareas.Revision_SubGerente3);
            lstTareas.Add(Constants.ENG_Tipos_Tareas.Dictamen_Realizar2);
            lstTareas.Add(Constants.ENG_Tipos_Tareas.Dictamen_RevisionGerente2);
            lstTareas.Add(Constants.ENG_Tipos_Tareas.Revision_DGHyP2);
            lstTareas.Add(Constants.ENG_Tipos_Tareas.Revision_DGHyP3);
            lstTareas.Add(Constants.ENG_Tipos_Tareas.Primer_Gestion_Documental);
            lstTareas.Add(Constants.ENG_Tipos_Tareas.Gestion_Documental);

            tareas = db.ENG_Tareas.Where(x => x.id_circuito == idCircuito &&
                                       lstTareas.Contains(x.cod_tarea.ToString().Substring(x.cod_tarea.ToString().Length - 2))).Select(y => y.nombre_tarea).ToList();
            return tareas;
        }
        private void Cargar_tipo_documentos_adjuntos()
        {

            List<TiposDeDocumentosRequeridos> list_doc_adj =
                (
                    from tdoc in db.TiposDeDocumentosRequeridos
                    where tdoc.visible_en_SSIT == true
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
                _id_solicitud= value;
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

        private int _id_tarea = 0;
        public int id_tarea
        {
            get
            {
                if (_id_tarea == 0)
                {
                    int.TryParse(hid_id_tarea.Value, out _id_tarea);
                }
                return _id_tarea;
            }
            set
            {
                hid_id_tarea.Value = value.ToString();
                _id_tarea = value;
            }
        }

        private int _id_tramite_tarea = 0;
        public int id_tramite_tarea
        {
            get
            {
                if (_id_tramite_tarea == 0)
                {
                    int.TryParse(hid_id_tramite_tarea.Value, out _id_tramite_tarea);
                }
                return _id_tramite_tarea;
            }
            set
            {
                hid_id_tramite_tarea.Value = value.ToString();
                _id_tramite_tarea = value;
            }
        }
        protected async void btnComenzarCargaArchivo_Click(object sender, EventArgs e)
        {
            UpdatePanel updPnlInputDatosArch = (UpdatePanel)btnComenzarCargaArchivo.Parent.Parent;

            int id_doc_adj = 0;
            try
            {
                Guid userid = Functions.GetUserId();

                Byte[] pdfBytes = cargarPDF(this.NombreArchivoFisico);

                ValidarDocumentoAdjunto(pdfBytes);

                using (var ctx = new DGHP_Entities())
                {
                    using (var tran = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            var sol = ctx.SSIT_Solicitudes.First(x => x.id_solicitud == id_solicitud);
                            sol.documentacionPA = true;
                            ctx.SaveChanges();

                            ObjectParameter id = new ObjectParameter("id_docadjunto", typeof(int));
                            int id_file = ws_FilesRest.subirArchivo(this.NombreArchivoFisico, pdfBytes);
                            ctx.SSIT_DocumentosAdjuntos_Add(id_solicitud, this.tipoDoc, this.tipoDocDetalle, (int)Constants.TiposDeDocumentosSistema.PRESENTACION_A_AGREGAR, false, id_file, this.NombreArchivoFisico, userid, id);
                            id_doc_adj = Convert.ToInt32(id.Value);

                            if (IDs_Fin_Tramite.Contains(id_tarea))
                            {
                                var id_circuito = ctx.ENG_Tareas.Where(x => x.id_tarea == id_tarea).Select(s => s.id_circuito).FirstOrDefault();
                                if (id_circuito == 0)
                                    throw new Exception("No se puede encontrar el circuito.");

                                var id_tarea_edgfc = ctx.ENG_Tareas.Where(x => x.id_circuito == id_circuito && x.nombre_tarea == "Enviar a DGFyC").Select(s => s.id_tarea).FirstOrDefault();
                                if (id_tarea_edgfc == 0)
                                    throw new Exception("No se puede encontrar la tarea Enviar a DGFyC para el circuito.");

                                ObjectParameter id_tt = new ObjectParameter("id_tramitetarea_nuevo", typeof(int));
                                ctx.ENG_Finalizar_Tarea(id_tramite_tarea, 0, Convert.ToInt32(id_tarea_edgfc), userid, id_tt);
                            }
                            
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogError.Write(ex, "Error en transaccion. ucSGI_DocumentoAdjunto-btnComenzarCargaArchivo_Click");
                            throw ex;
                        }
                    }
                }

                await ucListaDocumentos.LoadData(id_grupotramite, id_solicitud, true, (int)Constants.TiposDeDocumentosSistema.PRESENTACION_A_AGREGAR);
                ucCabecera.LoadData(id_grupotramite, id_solicitud);
                //updResultados.Update(); Se comento porque no deja generar cargas multiples - Ticket 2250

                string mensaje = "El documento \"" + this.NombreArchivoOriginal + "\" se ha adjuntado correctamente.";
                mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
                string script = string.Format("tda_mostrar_mensaje('{0}','{1}');", mensaje, "Aviso");
                ScriptManager.RegisterStartupScript(updPnlInputDatosArch, updPnlInputDatosArch.GetType(), "tda_mostrar_mensaje", script, true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(updPnlInputDatosArch, updPnlInputDatosArch.GetType(), "showfrmError_ucDocumentoAdjunto", "showfrmError_ucDocumentoAdjunto();", true);
            }

            EliminarDocumento(this.NombreArchivoFisico);
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

            int tipoDoc = 0;
            if (!int.TryParse(ddl_tipo_doc.SelectedValue, out tipoDoc))
                throw new ucSGI_DocumentoAdjuntoException("Tipo de Documento invalido.");

            using (var ctx = new DGHP_Entities())
            {
                TiposDeDocumentosRequeridos doc_adj =
                    ctx.TiposDeDocumentosRequeridos.Where(x => x.id_tdocreq == tipoDoc).FirstOrDefault();

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

                }
            }
        }
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
                db.Dispose();
                updPnlMensajes.Update();
            }
        }
        #region entity
        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }
        }

        #endregion

        protected async void ucListaDocumentos_EliminarListaDocumentosv1Click(object sender, ucListaDocumentosv1EventsArgs e)
        {
            Guid userid = Functions.GetUserId();
            using (var ctx = new DGHP_Entities())
            {
                var list = ctx.SSIT_DocumentosAdjuntos.Where(x => x.id_solicitud == id_solicitud && x.id_tipodocsis == (int)Constants.TiposDeDocumentosSistema.PRESENTACION_A_AGREGAR).ToList();
                if (list.Count() == 0)
                {
                    var sol = ctx.SSIT_Solicitudes.First(x => x.id_solicitud == id_solicitud);
                    sol.documentacionPA = false;
                    ctx.SaveChanges();
                }
                await ucListaDocumentos .LoadData(id_grupotramite, id_solicitud, true, (int)Constants.TiposDeDocumentosSistema.PRESENTACION_A_AGREGAR);
                ucCabecera.LoadData(id_grupotramite, id_solicitud);
                //updResultados.Update(); Se comento porque no deja generar cargas multiples - Ticket 2250
            }
        }
 
    }
}