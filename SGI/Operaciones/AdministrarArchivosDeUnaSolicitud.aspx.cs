using Microsoft.Ajax.Utilities;
using RestSharp;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ws_ExpedienteElectronico;

namespace SGI.Operaciones
{
    public partial class AdministrarArchivosDeUnaSolicitud : BasePage
    {
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
                    gridViewArchivosTransf.Visible = true;
                    gridViewArchivosTransf.DataBind();
                }
                else
                {
                    gridViewArchivosSolic.Visible = true;
                    gridViewArchivosSolic.DataBind();
                }
            }
            fillInfoSade(idSolicitud, couldParse);
            updResultados.Update();
            EjecutarScript(updResultados, "showResultado();");
        }
        public List<SSIT_DocumentosAdjuntos> CargarSolicitudConArchivos(int startRowIndex, int maximumRows, out int totalRowCount)
        {
            totalRowCount = 0;
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out int idSolicitud);
            if (couldParse)
            {
                DGHP_Entities entities = new DGHP_Entities();
                IQueryable<SSIT_DocumentosAdjuntos> archivosDeLaSolicitud = from archivos in entities.SSIT_DocumentosAdjuntos
                                                                            where archivos.id_solicitud == idSolicitud
                                                                            select archivos;
                btnAgregarArchivo.Enabled = true;
                totalRowCount = archivosDeLaSolicitud.Count();
                archivosDeLaSolicitud = archivosDeLaSolicitud.OrderBy(o => o.id_file).Skip(startRowIndex).Take(maximumRows);
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
                return archivosDeLaSolicitud.ToList();
            }
            else
            {
                return null;
            }
        }
        public List<Transf_DocumentosAdjuntos> CargarTransferenciasConArchivos(int startRowIndex, int maximumRows, out int totalRowCount)
        {
            totalRowCount = 0;
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out int idSolicitud);
            if (couldParse)
            {
                DGHP_Entities entities = new DGHP_Entities();
                IQueryable<Transf_DocumentosAdjuntos> archivosDeLaTransf = from archivos in entities.Transf_DocumentosAdjuntos
                                                                           where archivos.id_solicitud == idSolicitud
                                                                           select archivos;
                btnAgregarArchivo.Enabled = true;
                totalRowCount = archivosDeLaTransf.Count();
                archivosDeLaTransf = archivosDeLaTransf.OrderBy(o => o.id_file).Skip(startRowIndex).Take(maximumRows);
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
                return archivosDeLaTransf.ToList();
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
                lnkEliminar.Visible = true;
               

                Label labelIdFile = (Label)e.Row.FindControl("labelIdFile");
                int IdFile = int.Parse(labelIdFile.Text);
                using (var ctx = new DGHP_Entities())
                {
                    SGI_SADE_Procesos sGI_SADE_Procesos = (from archivos in ctx.SGI_SADE_Procesos
                                                           where archivos.id_file == IdFile
                                                           select archivos).FirstOrDefault();
                    if(sGI_SADE_Procesos!=null)
                        lnkEliminar.Visible = false;
                }
            }
        }

        protected void gridViewArchivosTransf_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkEliminar = (LinkButton)e.Row.FindControl("lnkEliminarDocTrans");
                lnkEliminar.Visible = true;

                Label labelIdFile = (Label)e.Row.FindControl("labelIdFile");
                int IdFile = int.Parse(labelIdFile.Text);
                using (var ctx = new DGHP_Entities())
                {
                    SGI_SADE_Procesos sGI_SADE_Procesos = (from archivos in ctx.SGI_SADE_Procesos
                                                           where archivos.id_file == IdFile
                                                           select archivos).FirstOrDefault();
                    if (sGI_SADE_Procesos != null)
                        lnkEliminar.Visible = false;
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
                        SSIT_Solicitudes Solicitud =
                            (from solicitudes in ctx.SSIT_Solicitudes
                             where solicitudes.id_solicitud == idSolicitud
                             select solicitudes).FirstOrDefault();
                        if (Solicitud.NroExpedienteSade.IsNullOrWhiteSpace())
                        {
                            ExpedienteE = string.Empty;
                            //TODO: Obtener id_paquete luego armar un
                            // endpoint en pasarela que reciba el paquete  
                            // y con eso devuelva todos los datos de EE
                            // Esto seria para las solicitudes que no 
                            //finalizaron Generar Expediente
                            //consultaExpedienteResponseDetallado ExpedienteElectronico = serviceEE.GetExpedienteByPaquete(this.username_servicio_EE, this.pass_servicio_EE, ExpedienteE);
                        }
                        else
                            ExpedienteE = Solicitud.NroExpedienteSade;
                        //Request a pasarela con el EE
                        consultaExpedienteResponseDetallado ExpedienteElectronico = serviceEE.consultarExpedienteDetallado(this.username_servicio_EE, this.pass_servicio_EE, ExpedienteE);
                        //TODO Setear labels con datos ExpedienteElectronico
                        loadUsersFromSector(ExpedienteElectronico);
                    }
                    catch
                    (Exception ex)
                    {
                        LogError.Write(ex);
                        throw (ex);
                    }
                }
            }
        }

        private void loadUsersFromSector(consultaExpedienteResponseDetallado ExpedienteElectronico)
        {
            if(ExpedienteElectronico != null)
            {
                if(ExpedienteElectronico.sectorDestino.IsNullOrWhiteSpace())
                {
                    DGHP_Entities db = new DGHP_Entities();
                    var usuarios = (from pro in db.SGI_Profiles
                                   join mem in db.aspnet_Membership on pro.userid equals mem.UserId
                                   where pro.Sector_SADE.ToUpper() == ExpedienteElectronico.sectorDestino.Trim().ToUpper()
                                    && !mem.IsLockedOut && pro.UserName_SADE.Length > 0
                                   select pro).ToList();
                    
                    if (usuarios == null || usuarios.Count() == 0)
                    {
                        Exception solEx = new Exception($"Expediente {ExpedienteElectronico.codigoEE}, No se encontraron usuarios del sector {ExpedienteElectronico.sectorDestino} en la base de datos. Error.");
                        LogError.Write(solEx);
                        throw solEx;
                    }
                    else
                    {
                        //TODO: Agregar estos datos al dropdownlist
                    }
                    db.Dispose();
                }
            }
        }

        protected void lnkSubirDocSadeSolic_Command(object sender, EventArgs e)
        {
            using (var ctx = new DGHP_Entities())
            {
                using (var tran = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        LinkButton lnkDocSadeSolic = (LinkButton)sender;
                        int id_docadjunto = Convert.ToInt32(lnkDocSadeSolic.CommandArgument);
                        int id_file = Convert.ToInt32(lnkDocSadeSolic.CommandName);
                        /*
                        using (var ftx = new AGC_FilesEntities())
                        {
                            Files file = (from f in ftx.Files
                                          where f.id_file == id_file
                                          select f).FirstOrDefault();
                        }
                        */
                        //ctx.SSIT_DocumentosAdjuntos_Del(id_docadjunto);
                        //tran.Commit();
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
                foreach (Control ctl in fila.Cells[0].FindControl("pnlpagerSolic").Controls)
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
                foreach (Control ctl in fila.Cells[0].FindControl("pnlpagerTransf").Controls)
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
    }
}