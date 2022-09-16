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
using System.Web.UI;
using System.Web.UI.WebControls;

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
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            int id_doc_adj = 0;
            if (FileUpload1.HasFile)
            {
                Byte[] filebytes = new byte[FileUpload1.PostedFile.ContentLength -1];
                filebytes = FileUpload1.FileBytes;
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
                int id_tdocrec = Convert.ToInt32(dropDownListEditTipoDeDocumentoRequerido.SelectedItem.Value);
                string tdocrec_detalle = txtTdocRecDetalle.Text;
                int id_tipodocsis = Convert.ToInt32(dropDownListEditTipoDeDocumentoSistema.SelectedItem.Value);
                //Llama al Procedure DGHP_Solicitudes.dbo.SSIT_DocumentosAdjuntos_Add
                using (var ctx = new DGHP_Entities())
                {
                    using (var tran = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            ObjectParameter id = new ObjectParameter("id_docadjunto", typeof(int));
                            ctx.SSIT_DocumentosAdjuntos_Add(Convert.ToInt32(txtSolicitud.Text), id_tdocrec, tdocrec_detalle, id_tipodocsis, false, id_file, fileName, createUser, id);
                            id_doc_adj = Convert.ToInt32(id.Value);
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogError.Write(ex, "Error en transaccion. SSIT_DocumentosAdjuntos_Add-AgregarArchivo-btnGuardar_Click");
                            throw ex;
                        }
                    }
                }
            }
            Response.Redirect("~/Operaciones/AdministrarArchivosDeUnaSolicitud.aspx");
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/AdministrarArchivosDeUnaSolicitud.aspx");
        }
    }
}