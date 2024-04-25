using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Ajax.Utilities;
using SGI.Model;
using SGI.WebServices;
using Syncfusion.JavaScript;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SGI.Constants;

namespace SGI.Operaciones
{
    public partial class AgregarArchivo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (Request.QueryString["Id"] != null)
            {
                this.txtSolicitud.Text = Request.QueryString["Id"].ToString();
                this.txtSolicitud.Enabled = false;
            }
            else
            {
                Response.Redirect("~/Operaciones/AdministrarArchivosDeUnaSolicitud.aspx");
            }
            if (!IsPostBack)
            {
                DGHP_Entities db = new DGHP_Entities();

                List<SGI.Model.TiposDeDocumentosRequeridos> TiposDeDocumentosRequeridosList = (from usu in db.TiposDeDocumentosRequeridos
                                               orderby (usu.nombre_tdocreq)
                                               select usu).ToList();
                foreach (SGI.Model.TiposDeDocumentosRequeridos tiposDeDocumentosRequeridos in TiposDeDocumentosRequeridosList)
                {
                    tiposDeDocumentosRequeridos.nombre_tdocreq = tiposDeDocumentosRequeridos.nombre_tdocreq + " (" + tiposDeDocumentosRequeridos.formato_archivo + ")";
                }
                dropDownListEditTipoDeDocumentoRequerido.DataSource = TiposDeDocumentosRequeridosList;
                dropDownListEditTipoDeDocumentoRequerido.DataTextField = "nombre_tdocreq";
                dropDownListEditTipoDeDocumentoRequerido.DataValueField = "id_tdocreq";
                dropDownListEditTipoDeDocumentoRequerido.DataBind();
                dropDownListEditTipoDeDocumentoRequerido.SelectedIndex = 0;
                dropDownListEditTipoDeDocumentoRequerido_SelectedIndexChanged(null, null);


                //FileUpload1.Enabled = false;
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int id_doc_adj = 0;
            if (FileUpload1.HasFile)
            {
                #region Valido Extension
                if (dropDownListEditTipoDeDocumentoRequerido.SelectedIndex < 0)
                {               
                    EnviarAlert("Debe Seleccionar un Tipo de Documento Requerido");
                    return;
                }

               
                int id_tdocreq =int.Parse( dropDownListEditTipoDeDocumentoRequerido.SelectedValue);
                SGI.Model.TiposDeDocumentosRequeridos tiposDeDocumentosRequeridos;
                using (var ctx = new DGHP_Entities())
                {
                     tiposDeDocumentosRequeridos = (from t in ctx.TiposDeDocumentosRequeridos
                                                                     where t.id_tdocreq == id_tdocreq
                                                                               select t).FirstOrDefault();
                }

                string _fileName = Server.HtmlEncode(FileUpload1.FileName);
                string extension = System.IO.Path.GetExtension(_fileName);
                string formato_archivo = tiposDeDocumentosRequeridos.formato_archivo;
              
                if ((extension != "." + formato_archivo) )
                {
                    EnviarAlert("El Tipo de documento Requerido debe ser " + "." + formato_archivo);
                    return;
                }

                #endregion

                Byte[] filebytes = new byte[FileUpload1.PostedFile.ContentLength -1];
                filebytes = FileUpload1.FileBytes;
                #region Validar Size y RequiereDetalle
                if (filebytes.Length > tiposDeDocumentosRequeridos.tamanio_maximo_mb * 1024 * 1024)
                {
                    EnviarAlert("El Tipo de documento Requerido debe pesar como maximo " + tiposDeDocumentosRequeridos.tamanio_maximo_mb + "MB");
                    return;
                }

                if (tiposDeDocumentosRequeridos.RequiereDetalle && string.IsNullOrEmpty(txtTdocRecDetalle.Text))
                {
                    EnviarAlert("Para este Tipo de documento Requerido debe ingresar un detalle");
                    return;
                }
                    #endregion

                    Guid createUser = Functions.GetUserId();
                String fileName = FileUpload1.FileName;
                int id_file;
                //Llama al Procedure AGC_Files.dbo.Files_Agregar
                using (var ctx = new AGC_FilesEntities())
                {
                    using (var tran = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            Files file = new Files();
                            file.rowid = Guid.NewGuid();
                            int last_id_file = ctx.Files.OrderByDescending(u => u.id_file).FirstOrDefault().id_file;
                            if (last_id_file.Equals(null)) last_id_file = 0;
                            id_file = last_id_file + 1;
                            file.id_file = id_file;
                            file.content_file = filebytes;
                            file.datos_documento_oficial = "";
                            file.CreateDate = DateTime.Now;
                            file.CreateUser = Functions.GetUserName();
                            file.UpdateDate = DateTime.Now;
                            file.UpdateUser = Functions.GetUserName();
                            file.FileName = fileName;
                            using (var md5 = MD5.Create())
                            {
                                file.Md5 = md5.ComputeHash(filebytes);
                            }
                            ctx.Files.Add(file);
                            ctx.SaveChanges();
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogError.Write(ex, "Error en transaccion. Files_Agregar-AgregarArchivo-btnGuardar_Click");
                            throw ex;
                        }
                    }
                }
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
                string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();

                Functions.InsertarMovimientoUsuario(userid, DateTime.Now, id_file, fileName, url, txtObservacionesSolicitante.Text, "I", 4012);

                int id_tdocrec = Convert.ToInt32(dropDownListEditTipoDeDocumentoRequerido.SelectedItem.Value);
                string tdocrec_detalle = txtTdocRecDetalle.Text;
                int id_tipodocsis = Convert.ToInt32(dropDownListEditTipoDeDocumentoSistema.SelectedItem.Value);
                //Llama al Procedure DGHP_Solicitudes.dbo.SSIT_DocumentosAdjuntos_Add o DGHP_Solicitudes.dbo.Transf_DocumentosAdjuntos_Agregar
                using (var ctx = new DGHP_Entities())
                {
                    using (var tran = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            int.TryParse(txtSolicitud.Text, out int idSolicitud);
                            int tipotramite = (from solic in ctx.SSIT_Solicitudes
                                               where solic.id_solicitud == idSolicitud
                                               select solic.id_tipotramite).Union(from trans in ctx.Transf_Solicitudes
                                                                                  where trans.id_solicitud == idSolicitud
                                                                                  select trans.id_tipotramite).FirstOrDefault();
                            ObjectParameter id = new ObjectParameter("id_docadjunto", typeof(int));
                            if (tipotramite == (int)Constants.TipoDeTramite.Transferencia)
                                ctx.Transf_DocumentosAdjuntos_Agregar(idSolicitud, id_tdocrec, tdocrec_detalle, id_tipodocsis, false, id_file, fileName, createUser, 0, id);
                            else
                                ctx.SSIT_DocumentosAdjuntos_Add(idSolicitud, id_tdocrec, tdocrec_detalle, id_tipodocsis, false, id_file, fileName, createUser, id);
                            id_doc_adj = Convert.ToInt32(id.Value);
                            tran.Commit();
                            Response.Redirect("~/Operaciones/AdministrarArchivosDeUnaSolicitud.aspx");
                        }
                        catch (Exception ex)
                        {
                            LogError.Write(ex, "Error en transaccion. SSIT_DocumentosAdjuntos_Add-AgregarArchivo-btnGuardar_Click");
                            throw ex;
                        }
                    }
                }
            }
            else
                EnviarAlert("No cargo ningun Archivo");

            // Response.Redirect("~/Operaciones/AdministrarArchivosDeUnaSolicitud.aspx");
         
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/AdministrarArchivosDeUnaSolicitud.aspx");
        }
        public void EnviarAlert(string Mensaje)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            string script = string.Format("alert('{0}');", Mensaje);
            ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);
        }

        protected void dropDownListEditTipoDeDocumentoRequerido_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (dropDownListEditTipoDeDocumentoRequerido.SelectedValue!="0") 
            //    FileUpload1.Enabled = true; 
            //else
            //    FileUpload1.Enabled = false;
            FileUpload1.Enabled = true;

            int id_tdocreq = int.Parse(dropDownListEditTipoDeDocumentoRequerido.SelectedValue);
            SGI.Model.TiposDeDocumentosRequeridos tiposDeDocumentosRequeridos;
            using (var ctx = new DGHP_Entities())
            {
                tiposDeDocumentosRequeridos = (from t in ctx.TiposDeDocumentosRequeridos
                                               where t.id_tdocreq == id_tdocreq
                                               select t).FirstOrDefault();
            }

            string _fileName = Server.HtmlEncode(FileUpload1.FileName);
            string extension = System.IO.Path.GetExtension(_fileName);
            string formato_archivo = tiposDeDocumentosRequeridos.formato_archivo;
            hdRequiereDetalle.Value = tiposDeDocumentosRequeridos.RequiereDetalle.ToString();


            FileUpload1.Attributes.Add("accept", "." + formato_archivo);
        }
    }
}