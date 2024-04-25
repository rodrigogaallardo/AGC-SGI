using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.BusinessLogicLayer;
using System.Drawing;
using SGI.Model;
using SGI.DataLayer.Models;
using SGI.BusinessLogicLayer.Constants;
using SGI.GestionTramite.Controls;
using Syncfusion.DocIO.DLS;
using System.Data.Entity;
using System.Globalization;
using System.Security.Policy;
using System.Threading;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SGI
{
    public partial class ConsultaMovimientos : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarTodosLosUsuarios();
        }

        #region Entity
        DGHP_Entities db = null;

        private void IniciarEntity()
        {
            if (db == null)
            {
                this.db = new DGHP_Entities();
                this.db.Database.CommandTimeout = 300;
            }
        }

        private void FinalizarEntity()
        {
            if (db != null)
                db.Dispose();
        }
        #endregion

        public void LoadData()
        {
            IniciarEntity();
            #region Carga de Datos
            var usuario = ddlUsuario.SelectedValue.ToString();
            var fechaDesdeString = txtFechaDesde.Text;
            var fechaHastaString = txtFechaHasta.Text;
            var tipoMovimientoString = ddlTipoMov.SelectedValue.ToString();
            var observacion = ddlObservacionSolicitante.SelectedValue.ToString();
            DateTime? fechaDesde;
            DateTime? fechaHasta;
            if (string.IsNullOrEmpty(usuario))
                usuario = null;
            if (DateTime.TryParse(fechaDesdeString, out DateTime fechaDesdeValue))
                fechaDesde = fechaDesdeValue;
            else
                fechaDesde = null;
            if (DateTime.TryParse(fechaHastaString, out DateTime fechaHastaValue))
                fechaHasta = fechaHastaValue.Date.Add(new TimeSpan(23, 59, 0));
            else
                fechaHasta = null;
            if (tipoMovimientoString == "")
                tipoMovimientoString = null;
            #endregion

            var query = from ss in db.SGI_Log_MovimientosUsuario
                        join us in db.aspnet_Users on ss.usuario equals us.UserId 
                        into userGroup
                        from user in userGroup.DefaultIfEmpty()
                        where (usuario == null || user.UserId.ToString() == usuario)
                        && (tipoMovimientoString == null || ss.TipoMovimiento == tipoMovimientoString)
                        && (fechaDesde == null || ss.FechaIngreso >= fechaDesde)
                        && (fechaHasta == null || ss.FechaIngreso <= fechaHasta)
                        && ((observacion == "Todos")
                            || (observacion == "Si" && ss.Observacion_Solicitante != null && ss.Observacion_Solicitante != "")
                            || (observacion == "No" && (ss.Observacion_Solicitante == null || ss.Observacion_Solicitante == "")))
                        orderby user.LoweredUserName ascending, ss.id
                        select new clsItemGrillaBuscarMovimientos
                        {
                            id = ss.id.ToString(),
                            usuario = user.LoweredUserName,
                            FechaIngreso = (DateTime)ss.FechaIngreso,
                            URL = ss.URL,
                            //DatosAdicionales = ss.datosAdicionales,
                            Observacion_Solicitante = ss.Observacion_Solicitante,
                            TipoMovimiento = ss.TipoMovimiento == "I" ? "Agregado" : 
                                             ss.TipoMovimiento == "U" ? "Modificacion" : 
                                             ss.TipoMovimiento == "D" ? "Eliminacion" :
                                             ss.TipoMovimiento == "C" ? "Consulta" :
                                             ss.TipoMovimiento
                        };
            var resultados = query.ToList();
            grdBuscarMovimientos.DataSource = resultados;
            grdBuscarMovimientos.DataBind();
            updPnlMovimientos.Update();
            updPnlMovimientos.Visible = true;
        }

        public void CargarTodosLosUsuarios()
        {
            Guid applicationId = new Guid("5BC28D51-C240-4D79-87B4-27D554686CE3");
            DGHP_Entities db = new DGHP_Entities();
            var usuarios = (from usu in db.aspnet_Users
                            join profile in db.SGI_Profiles on usu.UserId equals profile.userid
                            where usu.ApplicationId == applicationId
                            && usu.SGI_PerfilesUsuarios.Where(x => x.ENG_Rel_Perfiles_Tareas.Count() > 0).Count() > 0
                            && usu.aspnet_Membership.IsApproved
                            orderby (usu.UserName)
                            select usu).ToList();
            db.Dispose();
            ddlUsuario.DataValueField = "userid";
            ddlUsuario.DataTextField = "username";
            ddlUsuario.DataSource = usuarios;
            ddlUsuario.DataBind();
            ddlUsuario.Items.Insert(0, "");
        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            grdBuscarMovimientos.PageIndex = 0;
            LoadData();
        }

        public void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            ddlUsuario.SelectedValue = string.Empty;
            txtFechaDesde.Text = string.Empty;
            txtFechaHasta.Text = string.Empty;
            ddlTipoMov.SelectedValue = "Todos";
            ddlObservacionSolicitante.SelectedValue = "Todos";
            updPnlMovimientos.Visible = false;
            updPnlMovimientos.Update();
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdBuscarMovimientos.PageIndex = e.NewPageIndex;
                LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;
            try
            {
                grdBuscarMovimientos.PageIndex = int.Parse(cmdPage.Text) - 1;
                LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                grdBuscarMovimientos.PageIndex = grdBuscarMovimientos.PageIndex - 1;
                LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                grdBuscarMovimientos.PageIndex = grdBuscarMovimientos.PageIndex + 1;
                LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grd_DataBound(object sender, EventArgs e)
        {
            try
            {
                GridView grid = (GridView)grdBuscarMovimientos;
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
                    foreach (System.Web.UI.Control ctl in fila.Cells[0].FindControl("pnlpager").Controls)
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
                    UpdatePanel updPnlPager = (UpdatePanel)fila.Cells[0].FindControl("updPnlPager");
                    if (updPnlPager != null)
                        updPnlPager.Update();
                }
            }
            catch (Exception ex)
            {
                string aa = ex.Message;
            }
        }
    }
}