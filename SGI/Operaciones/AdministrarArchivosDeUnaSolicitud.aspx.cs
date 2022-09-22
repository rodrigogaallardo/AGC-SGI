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

namespace SGI.Operaciones
{
    public partial class AdministrarArchivosDeUnaSolicitud : BasePage
    {
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
            gridViewArchivos.DataBind();
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
        protected void grd_doc_adj_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkEliminar = (LinkButton)e.Row.FindControl("lnkEliminarDocAdj");
                lnkEliminar.Visible = true;
            }
        }
        protected void lnkEliminarDocAdj_Command(object sender, EventArgs e)
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
                        LogError.Write(ex, "Error en transaccion. SSIT_DocumentosAdjuntos_Del-AdministrarArchivosDeUnaSolicitud-gridViewArchivos_RowDeleting");
                        throw ex;
                    }
                }
            }
            gridViewArchivos.EditIndex = -1;
            btnBuscarSolicitud_Click(sender, e);
        }
        protected void btnAgregarArchivo_Click(object sender, EventArgs e)
        {
            Session["LastID"] = txtBuscarSolicitud.Text;
            Response.Redirect("~/Operaciones/AgregarArchivo.aspx?id=" + txtBuscarSolicitud.Text);
        }

        protected void gridViewArchivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) {}
        }

        protected void gridViewArchivos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewArchivos.PageIndex = e.NewPageIndex;
        }

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;
            gridViewArchivos.PageIndex = int.Parse(cmdPage.Text) - 1;
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            gridViewArchivos.PageIndex = gridViewArchivos.PageIndex - 1;
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            gridViewArchivos.PageIndex = gridViewArchivos.PageIndex + 1;
        }

        protected void gridViewArchivos_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)gridViewArchivos;
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
    }
}