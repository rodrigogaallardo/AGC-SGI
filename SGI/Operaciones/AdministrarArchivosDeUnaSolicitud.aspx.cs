
using Microsoft.Ajax.Utilities;
using DocumentFormat.OpenXml.Wordprocessing;
using RestSharp;
using SGI.Model;
using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using ws_ExpedienteElectronico;
using SGI.GestionTramite.Controls;
using ws_solicitudes;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SGI.Operaciones
{
    public partial class AdministrarArchivosDeUnaSolicitud : BasePage
    {
        public int id_grupo_tramite
        {
            get
            {
                int ret = (ViewState["_grupo_tramite"] != null ? Convert.ToInt32(ViewState["_grupo_tramite"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_grupo_tramite"] = value;
            }

        }
        private string _sistema_SADE;
        private string sistema_SADE
        {
            get
            {
                if (string.IsNullOrEmpty(_sistema_SADE))
                {
                    _sistema_SADE = "SGI";
                }
                return _sistema_SADE;
            }
        }
        private string _url_servicio_EE;
        private string url_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_url_servicio_EE))
                {
                    _url_servicio_EE = Parametros.GetParam_ValorChar("SGI.Url.Service.ExpedienteElectronico");
                }
                return _url_servicio_EE;
            }
        }
        private string _username_servicio_EE;
        private string username_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_username_servicio_EE))
                {
                    _username_servicio_EE = Parametros.GetParam_ValorChar("SGI.UserName.Service.ExpedienteElectronico");
                }
                return _username_servicio_EE;
            }
        }
        private string _pass_servicio_EE;
        private string pass_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_pass_servicio_EE))
                {
                    _pass_servicio_EE = Parametros.GetParam_ValorChar("SGI.Pwd.Service.ExpedienteElectronico");
                }
                return _pass_servicio_EE;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            btnAgregarArchivo.Enabled = false;
            String last_id = String.Empty;
            if (Session["LastID"] != null)
            {
                txtBuscarSolicitud.Text = Session["LastID"].ToString();
                Session["LastID"] = null;
                btnBuscarSolicitud_Click(sender, e);
            }
            viewDropDownList.Visible = false;
            viewValorExpediente.Visible = false;
            myAccordion.Visible = false;
        }
        protected void btnBuscarSolicitud_Click(object sender, EventArgs e)
        {
            gridViewArchivosSolic.Visible = false;
            gridViewArchivosTransf.Visible = false;
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out int idSolicitud);
            if (couldParse)
            {
                DGHP_Entities entities = new DGHP_Entities();
                int tipotramite = (from solic in entities.SSIT_Solicitudes
                                   where solic.id_solicitud == idSolicitud
                                   select solic.id_tipotramite).Union(from trans in entities.Transf_Solicitudes
                                                                      where trans.id_solicitud == idSolicitud
                                                                      select trans.id_tipotramite).FirstOrDefault();
                if (tipotramite == (int)Constants.TipoDeTramite.Transferencia)
                {
                    this.id_grupo_tramite = (int)Constants.GruposDeTramite.TR;
                    gridViewArchivosTransf.Visible = true;
                    gridViewArchivosTransf.DataBind();
                }
                else
                {
                    //aca estamos ignorando cpadron
                    this.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;
                    gridViewArchivosSolic.Visible = true;
                    gridViewArchivosSolic.DataBind();
                }
            }
            viewDropDownList.Visible = true;
            viewValorExpediente.Visible = true;
            myAccordion.Visible = true;
            fillInfoSade(idSolicitud, couldParse);
            updResultados.Update();
            EjecutarScript(updResultados, "showResultado();");
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, string.Empty, url, txtObservacionesSolicitante.Text, "D");

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, string.Empty, url, string.Empty, "D");


        }
        public List<itemDocumentoModulo> CargarSolicitudConArchivos(int startRowIndex, int maximumRows, out int totalRowCount)
        {
            totalRowCount = 0;
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out int idSolicitud);
            if (couldParse)
            {
                DGHP_Entities entities = new DGHP_Entities();
                List<itemDocumentoModulo> documentos = new List<itemDocumentoModulo>();
                IQueryable<SSIT_DocumentosAdjuntos> archivosDeLaSolicitud = from archivos in entities.SSIT_DocumentosAdjuntos
                                                                            where archivos.id_solicitud == idSolicitud
                                                                            select archivos;
                IQueryable<Encomienda_SSIT_Solicitudes> encomiendas = (from encomiendaSol in entities.Encomienda_SSIT_Solicitudes
                                                                         where encomiendaSol.id_solicitud == idSolicitud
                                                                         select encomiendaSol);
                IQueryable<Encomienda_DocumentosAdjuntos> archivosEncomiendaHabilitacion = (
                                                from archivos in entities.Encomienda_DocumentosAdjuntos
                                                where encomiendas.Any(e => e.id_encomienda == archivos.id_encomienda)
                                                select archivos);
                btnAgregarArchivo.Enabled = true;
                totalRowCount = archivosDeLaSolicitud.Count();
                totalRowCount += archivosEncomiendaHabilitacion.Count();
                archivosDeLaSolicitud = archivosDeLaSolicitud.OrderBy(o => o.id_file).Skip(startRowIndex).Take(maximumRows);
                archivosEncomiendaHabilitacion = archivosEncomiendaHabilitacion.OrderBy(o => o.id_file).Skip(startRowIndex).Take(maximumRows);
                foreach (var archivo in archivosDeLaSolicitud)
                {
                    documentos.Add(new itemDocumentoModulo
                    {
                        id_doc_adj = archivo.id_docadjunto,
                        id_docadjunto = archivo.id_docadjunto,
                        nombre_archivo = archivo.nombre_archivo,
                        id_file = archivo.id_file,
                        id_solicitud = idSolicitud,
                        TiposDeDocumentosRequeridos = archivo.TiposDeDocumentosRequeridos,
                        TiposDeDocumentosSistema = archivo.TiposDeDocumentosSistema,
                        nombre_tdocreq = archivo.TiposDeDocumentosRequeridos.nombre_tdocreq,
                        tdocreq_detalle = archivo.tdocreq_detalle,
                        generadoxSistema = archivo.generadoxSistema,
                        CreateDate = archivo.CreateDate,
                        CreateUser = archivo.CreateUser,
                        id_tipodocsis = archivo.TiposDeDocumentosRequeridos.id_tipdocsis,
                        fechaPresentado = archivo.fechaPresentado,
                        ExcluirSubidaSADE = archivo.ExcluirSubidaSADE
                    });
                }
                foreach (var archivo in archivosEncomiendaHabilitacion)
                {
                    documentos.Add(new itemDocumentoModulo
                    {
                        id_doc_adj = archivo.id_docadjunto,
                        id_docadjunto = archivo.id_docadjunto,
                        nombre_archivo = archivo.nombre_archivo,
                        id_file = archivo.id_file,
                        id_solicitud = idSolicitud,
                        TiposDeDocumentosRequeridos = archivo.TiposDeDocumentosRequeridos,
                        TiposDeDocumentosSistema = archivo.TiposDeDocumentosSistema,
                        nombre_tdocreq = archivo.TiposDeDocumentosRequeridos.nombre_tdocreq,
                        tdocreq_detalle = archivo.tdocreq_detalle,
                        generadoxSistema = archivo.generadoxSistema,
                        CreateDate = archivo.CreateDate,
                        CreateUser = archivo.CreateUser,
                        id_tipodocsis = archivo.TiposDeDocumentosRequeridos.id_tipdocsis,
                        fechaPresentado = archivo.fechaPresentado,
                        ExcluirSubidaSADE = false
                    });
                }
                pnlCantidadRegistros.Visible = true;
                if (totalRowCount > 1)
                {
                    lblCantRegistros.Text = string.Format("{0} Documentos Adjuntos", totalRowCount);
                }
                else if (totalRowCount == 1)
                {
                    lblCantRegistros.Text = string.Format("{0} Documentos Adjuntos", totalRowCount);
                }
                else
                {
                    pnlCantidadRegistros.Visible = false;
                }
                updResultados.Update();
                return documentos.ToList();
            }
            else
            {
                return null;
            }
        }
        public List<itemDocumentoModulo> CargarTransferenciasConArchivos(int startRowIndex, int maximumRows, out int totalRowCount)
        {
            totalRowCount = 0;
            List<itemDocumentoModulo> documentos = new List<itemDocumentoModulo>();
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out int idSolicitud);
            if (couldParse)
            {
                DGHP_Entities entities = new DGHP_Entities();
                IQueryable<Transf_DocumentosAdjuntos> archivosDeLaTransf = from archivos in entities.Transf_DocumentosAdjuntos
                                                                           where archivos.id_solicitud == idSolicitud
                                                                           select archivos;

                IQueryable<Encomienda_Transf_Solicitudes> encomiendas = (from encomiendaTfSol in entities.Encomienda_Transf_Solicitudes
                                                      where encomiendaTfSol.id_solicitud == idSolicitud
                                                      select encomiendaTfSol);
                IQueryable<Encomienda_DocumentosAdjuntos> archivosEncomiendaTransf = (
                                                from archivos in entities.Encomienda_DocumentosAdjuntos
                                                where encomiendas.Any(e => e.id_encomienda == archivos.id_encomienda)
                                                select archivos);

                btnAgregarArchivo.Enabled = true;
                totalRowCount = archivosDeLaTransf.Count();
                totalRowCount += archivosEncomiendaTransf.Count();
                archivosDeLaTransf = archivosDeLaTransf.OrderBy(o => o.id_file).Skip(startRowIndex).Take(maximumRows);
                archivosEncomiendaTransf = archivosEncomiendaTransf.OrderBy(o => o.id_file).Skip(startRowIndex).Take(maximumRows);
                foreach (var archivo in archivosDeLaTransf)
                {
                    documentos.Add(new itemDocumentoModulo
                    {
                        id_doc_adj = archivo.id_docadjunto,
                        id_docadjunto = archivo.id_docadjunto,
                        nombre_archivo = archivo.nombre_archivo,
                        id_file = archivo.id_file,
                        id_solicitud = idSolicitud,
                        TiposDeDocumentosRequeridos = archivo.TiposDeDocumentosRequeridos,
                        TiposDeDocumentosSistema = archivo.TiposDeDocumentosSistema,
                        nombre_tdocreq = archivo.TiposDeDocumentosRequeridos.nombre_tdocreq,
                        tdocreq_detalle = archivo.tdocreq_detalle,
                        generadoxSistema = archivo.generadoxSistema,
                        CreateDate = archivo.CreateDate,
                        CreateUser = archivo.CreateUser,
                        id_tipodocsis = archivo.TiposDeDocumentosRequeridos.id_tipdocsis
                    });
                }
                foreach (var archivo in archivosEncomiendaTransf)
                {
                    documentos.Add(new itemDocumentoModulo
                    {
                        id_doc_adj = archivo.id_docadjunto,
                        id_docadjunto = archivo.id_docadjunto,
                        nombre_archivo = archivo.nombre_archivo,
                        id_file = archivo.id_file,
                        id_solicitud = idSolicitud,
                        TiposDeDocumentosRequeridos = archivo.TiposDeDocumentosRequeridos,
                        TiposDeDocumentosSistema = archivo.TiposDeDocumentosSistema,
                        nombre_tdocreq = archivo.TiposDeDocumentosRequeridos.nombre_tdocreq,
                        tdocreq_detalle = archivo.tdocreq_detalle,
                        generadoxSistema = archivo.generadoxSistema,
                        CreateDate = archivo.CreateDate,
                        CreateUser = archivo.CreateUser,
                        id_tipodocsis = archivo.TiposDeDocumentosRequeridos.id_tipdocsis
                    });
                }
                pnlCantidadRegistros.Visible = true;
                if (totalRowCount > 1)
                {
                    lblCantRegistros.Text = string.Format("{0} Documentos Adjuntos", totalRowCount);
                }
                else if (totalRowCount == 1)
                {
                    lblCantRegistros.Text = string.Format("{0} Documentos Adjuntos", totalRowCount);
                }
                else
                {
                    pnlCantidadRegistros.Visible = false;
                }
                updResultados.Update();
                return documentos.ToList();
            }
            else
            {
                return null;
            }
        }

        protected void gridViewArchivosSolic_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkEliminar = (LinkButton)e.Row.FindControl("lnkEliminarDocSolic");
                LinkButton lnkSubir = (LinkButton)e.Row.FindControl("lnkDocSadeSolic");
                lnkEliminar.Visible = true;
                lnkSubir.Visible = true;

                bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out int idSolicitud);
                Label labelIdFile = (Label)e.Row.FindControl("labelIdFile");
                int IdFile = int.Parse(labelIdFile.Text);
                Label labelNroDeGedo = (Label)e.Row.FindControl("labelNroDeGedo");
                string nroGedo = LoadNumeroGedo(IdFile, idSolicitud);
                labelNroDeGedo.Text = nroGedo;
                if (!nroGedo.IsNullOrWhiteSpace())
                {
                    lnkEliminar.Visible = false;
                    lnkSubir.Visible = false;
                }

            }
        }

        protected void gridViewArchivosTransf_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkEliminar = (LinkButton)e.Row.FindControl("lnkEliminarDocTrans");
                LinkButton lnkSubir = (LinkButton)e.Row.FindControl("lnkDocSadeSolic");
                lnkEliminar.Visible = true;
                bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out int idTransferencia);
                Label labelIdFile = (Label)e.Row.FindControl("labelIdFile");
                int IdFile = int.Parse(labelIdFile.Text);
                Label labelNroDeGedo = (Label)e.Row.FindControl("labelNroDeGedo");
                string nroGedo = LoadNumeroGedo(IdFile, idTransferencia);
                labelNroDeGedo.Text = nroGedo;
                if (!nroGedo.IsNullOrWhiteSpace())
                {
                    lnkEliminar.Visible = false;
                    lnkSubir.Visible = false;
                }
            }
        }

        protected void lnkEliminarDocSolic_Command(object sender, EventArgs e)
        {
            using (var ctx = new DGHP_Entities())
            {
                using (var tran = ctx.Database.BeginTransaction())
                {
                    try
                    {

                        LinkButton lnkEliminar = (LinkButton)sender;
                        int id_docadjunto = Convert.ToInt32(lnkEliminar.CommandArgument);
                        int id_file = Convert.ToInt32(lnkEliminar.CommandName);
                        using (var ftx = new AGC_FilesEntities())
                        {
                            Files file = (from f in ftx.Files
                                          where f.id_file == id_file
                                          select f).FirstOrDefault();
                            ftx.Files.Remove(file);
                            ftx.SaveChanges();
                        }

                        ctx.SSIT_DocumentosAdjuntos_Del(id_docadjunto);
                        tran.Commit();

                        string script = "$('#frmEliminarLog').modal('show');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);

                    }
                    catch (Exception ex)
                    {
                        LogError.Write(ex, "Error en transaccion. SSIT_DocumentosAdjuntos_Del-AdministrarArchivosDeUnaSolicitud-gridViewArchivosSolic_RowDeleting");
                        throw ex;
                    }
                }

            }
            gridViewArchivosSolic.EditIndex = -1;
            btnBuscarSolicitud_Click(sender, e);
        }

        protected void lnkEliminarDocTrans_Command(object sender, EventArgs e)
        {
            using (var ctx = new DGHP_Entities())
            {
                using (var tran = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        LinkButton lnkEliminar = (LinkButton)sender;
                        int id_docadjunto = Convert.ToInt32(lnkEliminar.CommandArgument);
                        int id_file = Convert.ToInt32(lnkEliminar.CommandName);
                        using (var ftx = new AGC_FilesEntities())
                        {
                            Files file = (from f in ftx.Files
                                          where f.id_file == id_file
                                          select f).FirstOrDefault();
                            ftx.Files.Remove(file);
                            ftx.SaveChanges();
                        }
                        ctx.Transf_DocumentosAdjuntos_Eliminar(id_docadjunto);
                        tran.Commit();

                        string script = "$('#frmEliminarLog').modal('show');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
                    }
                    catch (Exception ex)
                    {
                        LogError.Write(ex, "Error en transaccion. Transf_DocumentosAdjuntos_Eliminar-AdministrarArchivosDeUnaSolicitud-gridViewArchivosSolic_RowDeleting");
                        throw ex;
                    }
                }

            }
            gridViewArchivosTransf.EditIndex = -1;
            btnBuscarSolicitud_Click(sender, e);
        }

        private void fillInfoSade(int idSolicitud, bool couldParse)
        {
            string ExpedienteE = string.Empty;
            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
            ws_ExpedienteElectronico.consultaExpedienteResponseDetallado ExpedienteElectronico;

            serviceEE.Url = this.url_servicio_EE;
            if (!couldParse)
            {
                Exception exS = new Exception("La solicitud ingresada no se pudo convertir en un entero");
                LogError.Write(exS);
                throw exS;
            }
            using (var ctx = new DGHP_Entities())
            {
                using (var tran = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        int id_paquete = 0;
                        if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.HAB)
                        {
                            SSIT_Solicitudes Solicitud =
                            (from solicitudes in ctx.SSIT_Solicitudes
                             where solicitudes.id_solicitud == idSolicitud
                             select solicitudes).FirstOrDefault();
                            id_paquete = Functions.GetPaqueteFromSolicitud(idSolicitud);
                            hid_paquete.Value = Convert.ToString(id_paquete);
                            if (Solicitud.NroExpedienteSade.IsNullOrWhiteSpace())
                            {
                                if (id_paquete > 0)
                                    ExpedienteE = serviceEE.GetExpedienteByPaquete(this.username_servicio_EE, this.pass_servicio_EE, id_paquete);
                                else
                                {
                                    //Si no tiene id_paquete entonces me salgo pues obviamente no tiene EE
                                    return;
                                }
                            }
                            else
                                ExpedienteE = Solicitud.NroExpedienteSade;
                        }
                        else
                        {
                            Transf_Solicitudes transferencia =
                            (from transferencias in ctx.Transf_Solicitudes
                             where transferencias.id_solicitud == idSolicitud
                             select transferencias).FirstOrDefault();
                            id_paquete = Functions.GetPaqueteFromSolicitud(idSolicitud);
                            hid_paquete.Value = Convert.ToString(id_paquete);
                            if (transferencia.NroExpedienteSade.IsNullOrWhiteSpace())
                            {
                                if (id_paquete > 0)
                                    ExpedienteE = serviceEE.GetExpedienteByPaquete(this.username_servicio_EE, this.pass_servicio_EE, id_paquete);
                                else
                                {
                                    //Si no tiene id_paquete entonces me salgo pues obviamente no tiene EE
                                    return;
                                }
                            }
                            else
                                ExpedienteE = transferencia.NroExpedienteSade;
                        }
                        
                        
                        //Request a pasarela con el EE
                        txtExpedienteElectronicoValor.Text = "";
                        txtEstadoValor.Text = "";
                        txtUsuarioValor.Text = "";
                        txtReparticionValor.Text = "";
                        txtSectorValor.Text = "";
                        ExpedienteElectronico = serviceEE.consultarExpedienteDetallado(this.username_servicio_EE, this.pass_servicio_EE, ExpedienteE);
                        if(ExpedienteElectronico != null)
                        {
                            txtExpedienteElectronicoValor.Text = ExpedienteElectronico.codigoEE;
                            txtEstadoValor.Text = ExpedienteElectronico.estado;
                            txtUsuarioValor.Text = ExpedienteElectronico.usuarioCaratulador;
                            txtReparticionValor.Text = ExpedienteElectronico.reparticionDestino;
                            txtSectorValor.Text = ExpedienteElectronico.sectorDestino;
                        }
                        else
                        {
                            myAccordion.Visible = false;
                        }
                        
                        
                        loadUsersFromSector(ExpedienteElectronico);
                    }
                    catch
                    (Exception ex)
                    {
                        LogError.Write(ex);
                        //throw (ex);
                    }
                }
            }
        }

        private void loadUsersFromSector(ws_ExpedienteElectronico.consultaExpedienteResponseDetallado ExpedienteElectronico)
        {
            if(ExpedienteElectronico != null)
            {
                if(!ExpedienteElectronico.usuarioDestino.IsNullOrWhiteSpace())
                {
                    DGHP_Entities db = new DGHP_Entities();
                    var usuarios = (from pro in db.SGI_Profiles
                                    join mem in db.aspnet_Membership on pro.userid equals mem.UserId
                                    where pro.UserName_SADE.ToUpper() == ExpedienteElectronico.usuarioDestino.Trim().ToUpper()
                                     && !mem.IsLockedOut && pro.UserName_SADE.Length > 0
                                    select pro).ToList();
                    var distinctUsuarios = usuarios
                            .GroupBy(pro => pro.UserName_SADE)
                            .Select(g => g.First())
                            .ToList();
                    ddlUsuario.DataSource = distinctUsuarios;
                    ddlUsuario.DataTextField = "UserName_SADE";
                    ddlUsuario.DataValueField = "userid";
                    ddlUsuario.DataBind();
                }
                else
                if(!ExpedienteElectronico.sectorDestino.IsNullOrWhiteSpace())
                {
                    DGHP_Entities db = new DGHP_Entities();
                    var usuarios = (from pro in db.SGI_Profiles
                                   join mem in db.aspnet_Membership on pro.userid equals mem.UserId
                                   where pro.Sector_SADE.ToUpper() == ExpedienteElectronico.sectorDestino.Trim().ToUpper()
                                    && !mem.IsLockedOut && pro.UserName_SADE.Length > 0
                                   select pro).ToList();
                    var distinctUsuarios = usuarios
                            .GroupBy(pro => pro.UserName_SADE)
                            .Select(g => g.First())
                            .ToList();
                    if (distinctUsuarios == null || distinctUsuarios.Count() == 0)
                    {
                        Exception solEx = new Exception($"Expediente {ExpedienteElectronico.codigoEE}, No se encontraron usuarios del sector {ExpedienteElectronico.sectorDestino} en la base de datos. Error.");
                        LogError.Write(solEx);
                        throw solEx;
                    }
                    else
                    {
                        ddlUsuario.DataSource = distinctUsuarios;
                        ddlUsuario.DataTextField = "UserName_SADE";
                        ddlUsuario.DataValueField = "userid";
                        ddlUsuario.DataBind();
                    }
                    db.Dispose();
                }
            }
        }

        protected void lnkSubirDocSadeSolic_Command(object sender, EventArgs e)
        {
            Guid userid_sgi = Functions.GetUserId();
            using (var ctx = new DGHP_Entities())
            {
                using (var tran = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        LinkButton lnkDocSadeSolic = (LinkButton)sender;
                        int id_docadjunto = Convert.ToInt32(lnkDocSadeSolic.CommandArgument);
                        int id_file = Convert.ToInt32(lnkDocSadeSolic.CommandName);
                        int id_file_selected = 0;
                        byte[] content_file = null;
                        int.TryParse(txtBuscarSolicitud.Text, out int idSolicitud);
                        string descTramite = $"Subido por modulo Id Archivo {id_file}";
                        string nroExpediente = txtExpedienteElectronicoValor.Text;
                        string usuario = ddlUsuario.SelectedValue;
                        int id_paquete = Convert.ToInt32(hid_paquete.Value);
                        Guid userid_tarea;
                        bool parseado = Guid.TryParse(usuario, out userid_tarea);
                        if (!parseado)
                            throw new Exception("Usuario SADE invalido");
                        using (var ftx = new AGC_FilesEntities())
                        {
                            Files file = (from f in ftx.Files
                                          where f.id_file == id_file
                                          select f).FirstOrDefault();
                            if (file != null)
                            {
                                id_file_selected = file != null ? file.id_file : 0;
                                content_file = file.content_file;
                                subirDocumento( idSolicitud, this.id_grupo_tramite, content_file, nroExpediente, 
                                                id_docadjunto, id_paquete, id_file, descTramite, userid_tarea);
                            }
                            else
                            {
                                Exception exceptionFile = new Exception("El archivo no existe en la base de files.");
                                LogError.Write(exceptionFile);
                                throw exceptionFile;
                            }
                            
                            
                        }
                        
                        
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        LogError.Write(ex, "Error en transaccion. SSIT_DocumentosAdjuntos_Del-AdministrarArchivosDeUnaSolicitud-gridViewArchivosSolic_RowSubirSade");
                        throw ex;
                    }
                }
            }
            gridViewArchivosSolic.EditIndex = -1;
            btnBuscarSolicitud_Click(sender, e);
        }

        protected void btnAgregarArchivo_Click(object sender, EventArgs e)
        {
            Session["LastID"] = txtBuscarSolicitud.Text;
            Response.Redirect("~/Operaciones/AgregarArchivo.aspx?id=" + txtBuscarSolicitud.Text);
        }

        protected void gridViewArchivosSolic_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewArchivosSolic.PageIndex = e.NewPageIndex;
        }

        protected void gridViewArchivosTransf_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewArchivosTransf.PageIndex = e.NewPageIndex;
        }

        protected void cmdPageSolic(object sender, EventArgs e)
        {
            LinkButton cmdPageSolic = (LinkButton)sender;
            gridViewArchivosSolic.PageIndex = int.Parse(cmdPageSolic.Text) - 1;
        }

        protected void cmdPageTransf(object sender, EventArgs e)
        {
            LinkButton cmdPageTransf = (LinkButton)sender;
            gridViewArchivosTransf.PageIndex = int.Parse(cmdPageTransf.Text) - 1;
        }

        protected void cmdAnteriorSolic_Click(object sender, EventArgs e)
        {
            gridViewArchivosSolic.PageIndex = gridViewArchivosSolic.PageIndex - 1;
        }

        protected void cmdAnteriorTransf_Click(object sender, EventArgs e)
        {
            gridViewArchivosTransf.PageIndex = gridViewArchivosTransf.PageIndex - 1;
        }

        protected void cmdSiguienteSolic_Click(object sender, EventArgs e)
        {
            gridViewArchivosSolic.PageIndex = gridViewArchivosSolic.PageIndex + 1;
        }

        protected void cmdSiguienteTransf_Click(object sender, EventArgs e)
        {
            gridViewArchivosTransf.PageIndex = gridViewArchivosTransf.PageIndex + 1;
        }

        protected void gridViewArchivosSolic_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)gridViewArchivosSolic;
            GridViewRow fila = (GridViewRow)grid.BottomPagerRow;
            if (fila != null)
            {
                LinkButton btnAnterior = (LinkButton)fila.Cells[0].FindControl("cmdAnteriorSolic");
                LinkButton btnSiguiente = (LinkButton)fila.Cells[0].FindControl("cmdSiguienteSolic");
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
                    LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPageSolic" + i.ToString());
                    btn.Visible = false;
                }
                if (grid.PageIndex == 0 || grid.PageCount <= 10)
                {
                    // Mostrar 10 botones o el máximo de páginas
                    for (int i = 1; i <= 10; i++)
                    {
                        if (i <= grid.PageCount)
                        {
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPageSolic" + i.ToString());
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
                    LinkButton btnPage10 = (LinkButton)fila.Cells[0].FindControl("cmdPageSolic10");
                    btnPage10.Visible = true;
                    btnPage10.Text = Convert.ToString(grid.PageIndex + 1);
                    // Ubica los 9 botones hacia la izquierda
                    for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                    {
                        CantBucles++;
                        if (i >= 0)
                        {
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPageSolic" + Convert.ToString(10 - CantBucles));
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
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPageSolic" + Convert.ToString(10 + CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                        }
                    }
                }
                LinkButton cmdPageSolic;
                string btnPage = "";
                for (int i = 1; i <= 19; i++)
                {
                    btnPage = "cmdPageSolic" + i.ToString();
                    cmdPageSolic = (LinkButton)fila.Cells[0].FindControl(btnPage);
                    if (cmdPageSolic != null)
                        cmdPageSolic.CssClass = "btn";
                }
                // busca el boton por el texto para marcarlo como seleccionado
                string btnText = Convert.ToString(grid.PageIndex + 1);
                foreach (System.Web.UI.Control ctl in fila.Cells[0].FindControl("pnlpagerSolic").Controls)
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

        protected void gridViewArchivosTransf_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)gridViewArchivosTransf;
            GridViewRow fila = (GridViewRow)grid.BottomPagerRow;
            if (fila != null)
            {
                LinkButton btnAnterior = (LinkButton)fila.Cells[0].FindControl("cmdAnteriorTransf");
                LinkButton btnSiguiente = (LinkButton)fila.Cells[0].FindControl("cmdSiguienteTransf");
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
                    LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPageTransf" + i.ToString());
                    btn.Visible = false;
                }
                if (grid.PageIndex == 0 || grid.PageCount <= 10)
                {
                    // Mostrar 10 botones o el máximo de páginas
                    for (int i = 1; i <= 10; i++)
                    {
                        if (i <= grid.PageCount)
                        {
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPageTransf" + i.ToString());
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
                    LinkButton btnPage10 = (LinkButton)fila.Cells[0].FindControl("cmdPageTransf10");
                    btnPage10.Visible = true;
                    btnPage10.Text = Convert.ToString(grid.PageIndex + 1);
                    // Ubica los 9 botones hacia la izquierda
                    for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                    {
                        CantBucles++;
                        if (i >= 0)
                        {
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPageTransf" + Convert.ToString(10 - CantBucles));
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
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPageTransf" + Convert.ToString(10 + CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                        }
                    }
                }
                LinkButton cmdPageTransf;
                string btnPage = "";
                for (int i = 1; i <= 19; i++)
                {
                    btnPage = "cmdPageTransf" + i.ToString();
                    cmdPageTransf = (LinkButton)fila.Cells[0].FindControl(btnPage);
                    if (cmdPageTransf != null)
                        cmdPageTransf.CssClass = "btn";
                }
                // busca el boton por el texto para marcarlo como seleccionado
                string btnText = Convert.ToString(grid.PageIndex + 1);
                foreach (System.Web.UI.Control ctl in fila.Cells[0].FindControl("pnlpagerTransf").Controls)
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
        /*
        private Encomienda_DocumentosAdjuntos LoadFilesEncomienda(int id_solicitud)
        {
            Encomienda_DocumentosAdjuntos archivos = new Encomienda_DocumentosAdjuntos();
            try
            {
                using (var db = new DGHP_Entities())
                {
                    archivos = (//Encomienda_DocumentosAdjuntos
                                    from doc in db.Transf_DocumentosAdjuntos
                                    join user in db.Usuario on doc.CreateUser equals user.UserId into us
                                    from u in us.DefaultIfEmpty()
                                    join prof in db.SGI_Profiles on doc.CreateUser equals prof.userid into pr
                                    from p in pr.DefaultIfEmpty()
                                    where doc.id_solicitud == id_solicitud //&& doc.CreateDate <= ultimaPresentacion
                                    select new Encomienda_DocumentosAdjuntos
                                    {
                                        nombre = doc.tdocreq_detalle != null && doc.tdocreq_detalle != "" ?
                                                doc.tdocreq_detalle : (doc.id_tdocreq != 0 ? doc.TiposDeDocumentosRequeridos.nombre_tdocreq : doc.TiposDeDocumentosSistema.nombre_tipodocsis),
                                        id_file = doc.id_file,
                                        id_solicitud = doc.id_solicitud,
                                        url = null,
                                        Fecha = doc.CreateDate,
                                        UserName = (u != null ? u.Apellido + ", " + u.Nombre : (p != null ? p.Apellido + ", " + p.Nombres : ""))
                                    }).Union(
                                    from doc in db.CPadron_DocumentosAdjuntos
                                    join sol in db.Transf_Solicitudes on doc.id_cpadron equals sol.id_cpadron
                                    join user in db.Usuario on doc.CreateUser equals user.UserId into us
                                    from u in us.DefaultIfEmpty()
                                    join prof in db.SGI_Profiles on doc.CreateUser equals prof.userid into pr
                                    from p in pr.DefaultIfEmpty()
                                    where sol.id_solicitud == id_solicitud //&& doc.id_tipodocsis == (int)Constants.TiposDeDocumentosSistema.INFORMES_CPADRON
                                                                           //&& doc.CreateDate <= ultimaPresentacion
                                    select new Encomienda_DocumentosAdjuntos
                                    {
                                        nombre = doc.id_tdocreq != 0 ? doc.TiposDeDocumentosRequeridos.nombre_tdocreq : doc.TiposDeDocumentosSistema.nombre_tipodocsis,
                                        id_file = doc.id_file,
                                        id_solicitud = doc.id_cpadron,
                                        url = null,
                                        Fecha = doc.CreateDate,
                                        UserName = (u != null ? u.Apellido + ", " + u.Nombre : (p != null ? p.Apellido + ", " + p.Nombres : ""))
                                    }).ToList();
                }
                return archivos;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }*/

        private string LoadNumeroGedo(int id_file, int id_solicitud)
        {
            string numeroGedo = "";
            try
            {
                using (var db = new DGHP_Entities())
                {
                    db.Database.CommandTimeout = 300;
                    var SgiSadeProceso = (from ssp in db.SGI_SADE_Procesos
                                          join tth in db.SGI_Tramites_Tareas_HAB on ssp.id_tramitetarea equals tth.id_tramitetarea
                                          where tth.id_solicitud == id_solicitud && ssp.id_file == id_file
                                          select ssp).Union(
                               from ssp in db.SGI_SADE_Procesos
                               join ttt in db.SGI_Tramites_Tareas_TRANSF on ssp.id_tramitetarea equals ttt.id_tramitetarea
                               where ttt.id_solicitud == id_solicitud && ssp.id_file == id_file
                               select ssp).FirstOrDefault();

                    if (SgiSadeProceso != null && SgiSadeProceso.resultado_ee != null)
                        numeroGedo = SgiSadeProceso.resultado_ee?.ToString();
                    else
                    {
                        numeroGedo = "";

                        using (var ee = new EE_Entities())
                        {
                            var documento = (from documentos in ee.wsEE_Documentos
                                             where documentos.id_file == id_file
                                             select documentos).FirstOrDefault();
                            if (documento != null)
                                numeroGedo = documento.numeroGEDO;
                        }

                    }

                }
            }
            catch (Exception)
            {
                numeroGedo = "";
            }
            return numeroGedo;
        }

        private ParametrosSADE GetParametrosSADE(Guid userId, int id_docadjunto, 
                                        string descripcion_tramite, int id_file, int id_grupo_tramite)
        {
            //deberia recibir el tipo de solicitud (transf o hab) y buscar asi los tipos correspondientes
            ParametrosSADE parametros = new ParametrosSADE();
            DGHP_Entities db = new DGHP_Entities();
            TiposDeDocumentosRequeridos tiposDeDocumentosRequeridos = new TiposDeDocumentosRequeridos();
            if(id_grupo_tramite == (int)Constants.GruposDeTramite.TR)
            {
                tiposDeDocumentosRequeridos =
                            (from tipoDocReq in db.TiposDeDocumentosRequeridos
                             join tf_docs in db.Transf_DocumentosAdjuntos on tipoDocReq.id_tdocreq equals tf_docs.id_tdocreq
                             where tf_docs.id_docadjunto == id_docadjunto
                             select tipoDocReq).FirstOrDefault();
            }
            else
            {
                tiposDeDocumentosRequeridos =
                            (from tipoDocReq in db.TiposDeDocumentosRequeridos
                             join ssit_docs in db.SSIT_DocumentosAdjuntos on tipoDocReq.id_tdocreq equals ssit_docs.id_tdocreq
                             where ssit_docs.id_docadjunto == id_docadjunto
                             select tipoDocReq).FirstOrDefault();
            }
            
            if(tiposDeDocumentosRequeridos == null)
            {
                tiposDeDocumentosRequeridos =
                            (from tipoDocReq in db.TiposDeDocumentosRequeridos
                             join enc_docs in db.Encomienda_DocumentosAdjuntos on tipoDocReq.id_tdocreq equals enc_docs.id_tdocreq
                             where enc_docs.id_docadjunto == id_docadjunto
                             select tipoDocReq).FirstOrDefault();

            }    

            parametros.Usuario_SADE = Functions.GetUsernameSADE(userId);
            parametros.Acronimo_SADE = tiposDeDocumentosRequeridos.acronimo_SADE;
            parametros.Tabla_Origen = ""; 
            parametros.formato_archivo = tiposDeDocumentosRequeridos.formato_archivo;
            parametros.Ffcc_SADE = tiposDeDocumentosRequeridos.ffcc_SADE;
            if (tiposDeDocumentosRequeridos.id_tdocreq > 0)
                parametros.descripcion_tramite = $"Subido por modulo {tiposDeDocumentosRequeridos.nombre_tdocreq} (id de archivo {id_file})";
            else
                parametros.descripcion_tramite = $"Subido por modulo (id de archivo {id_file})";
            return parametros;
        }

        private async void subirDocumento(  int id_solicitud,int id_grupo_tramite, byte[] documento,
                                            string nro_expediente,int id_docadjunto, int id_paquete,
                                            int id_file, string descripcion_tramite, Guid userid)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            AGC_FilesEntities dbFiles = new AGC_FilesEntities();
            dbFiles.Database.CommandTimeout = 300;
            int id_devolucion_ee = -1;
            bool huboError = false;

            try
            {

                // Recupero los parámetros de la tarea de proceso

                ParametrosSADE parametros = GetParametrosSADE(userid, id_docadjunto, descripcion_tramite, id_file, id_grupo_tramite);
                string username_SADE = "";
                string Tabla_Origen = "";
                string Acronimo_SADE = "";
                string formato_archivo = "";
                string Ffcc_SADE = null;
                string nombre_archivo = "";
                
                try { username_SADE = parametros.Usuario_SADE; }
                catch (Exception) 
                {
                    Exception exUserSade = new Exception("No se pudo conseguir el usuario de SADE");
                    LogError.Write(exUserSade);
                    throw exUserSade;
                }

                try { Tabla_Origen = parametros.Tabla_Origen; }
                catch (Exception) { Tabla_Origen = ""; }

                try { Acronimo_SADE = parametros.Acronimo_SADE; }
                catch (Exception) { Acronimo_SADE = Functions.GetParametroCharEE("EE.acronimo.pdf.sin.firma"); }

                try { formato_archivo = parametros.formato_archivo; }
                catch (Exception) { formato_archivo = "pdf"; }

                try { Ffcc_SADE = parametros.Ffcc_SADE; }
                catch (Exception) { Ffcc_SADE = null; }

                
                // Subir y relacionar documento en servicio
                // ---------------------------------------
                ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                //serviceEE.Url = this.url_servicio_EE;
                serviceEE.Url = this.url_servicio_EE;
                if (username_SADE.Length <= 0)
                    throw new Exception("Su usuario no posee configurado el nombre de usuario del sistema SADE.");

                try
                {
                    string identificacion_documento = string.Format("Nro. de trámite: {0}, Nro. de documento: {1}", id_solicitud, id_file);

                    bool EnviarEmbebido = false;

                    // identifica si es un pdf y si tiene los permisos correctos y si está firmado.
                    try
                    {
                        using (var pdf = new iTextSharp.text.pdf.PdfReader(documento))
                        {
                            if (!pdf.IsOpenedWithFullPermissions || (serviceEE.isPdfFirmado(ref documento) && Acronimo_SADE == Functions.GetParametroCharEE("EE.acronimo.pdf.sin.firma")))
                                EnviarEmbebido = true;
                        }

                    }
                    catch (Exception)
                    {
                        // si no es pdf va embebido siempre
                        EnviarEmbebido = true;
                    }

                    if (Ffcc_SADE == null)
                    {
                        if (EnviarEmbebido)
                        {
                            var nom = db.SSIT_DocumentosAdjuntos.Where(x => x.id_file == id_file).FirstOrDefault();

                            if (nom == null)
                                nombre_archivo = identificacion_documento + ".txt";
                            else
                                nombre_archivo = nom.nombre_archivo;

                            id_devolucion_ee = serviceEE.Subir_Documentos_Embebidos_ConAcroAndTipo(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, documento,
                                                    identificacion_documento, parametros.descripcion_tramite, this.sistema_SADE, username_SADE, Acronimo_SADE, "txt", nombre_archivo);
                        }
                        else
                            id_devolucion_ee = serviceEE.Subir_Documento_ConAcroAndTipo(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, documento,
                                                    identificacion_documento, parametros.descripcion_tramite, this.sistema_SADE, username_SADE, Acronimo_SADE, formato_archivo, false);
                    }
                    else
                    {
                        string formulario_json = await FormulariosControlados.getFormulario(Ffcc_SADE, id_solicitud);
                        if (EnviarEmbebido)
                            id_devolucion_ee = serviceEE.Subir_Documentos_Embebidos_ConAcroAndTipo_ffcc(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, documento,
                                                    identificacion_documento, parametros.descripcion_tramite, this.sistema_SADE, username_SADE, Acronimo_SADE, "txt", identificacion_documento, formulario_json);
                        else
                            id_devolucion_ee = serviceEE.Subir_Documento_ConAcroAndTipo_ffcc(this.username_servicio_EE, this.pass_servicio_EE, id_paquete, documento,
                                                    identificacion_documento, parametros.descripcion_tramite, this.sistema_SADE, username_SADE, Acronimo_SADE, formato_archivo, formulario_json);
                    }
                    //TODO: Mostrar algun cartel o algo indicando que se generó

                }
                catch (Exception ex)
                {
                    //TODO: Mostrar algun cartel o algo indicando que se NO se generó
                    id_devolucion_ee = -1;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    serviceEE.Dispose();
                }

            }
            catch (Exception ex)
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                huboError = true;
                string error = ex.Message;
                //db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, error, userid);
            }
            finally
            {
                // Actualiza el resultado del servicio en la tabla de procesos.
                // -----------------------------------------------------------
                if (huboError)
                    LogError.Write(new Exception("No se pudo subir el archivo a SADE"));
                    //db.SGI_SADE_Procesos_update(id_tarea_proc, realizado_en_pasarela, string.Empty, id_devolucion_ee, resultado_ee, userid);

                db.Dispose();
                dbFiles.Dispose();
            }

        }
        
    }

    internal class ParametrosSADE
    {
        public string Usuario_SADE { get; set; }
        public string Tabla_Origen { get; set; }
        public string Acronimo_SADE { get; set; }
        public string formato_archivo { get; set; }
        public string Ffcc_SADE { get; set; }
        public string descripcion_tramite { get; set; } //Esto no es un parametro pero lo meto aca por simplicidad
    }
}