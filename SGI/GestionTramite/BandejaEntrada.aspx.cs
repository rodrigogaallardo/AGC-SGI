

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using System.Web.Services;
using System.Web.Security;
using System.Net;
using System.IO;
using System.Text;
using SGI.Controls;
using System.Data.Entity;


namespace SGI
{
    public partial class BandejaEntrada : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this.Page);
            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updBandejaAsignacion, updBandejaAsignacion.GetType(), "inicializar_componentes", "inicializar_componentes();", true);
                ScriptManager.RegisterStartupScript(updddlTareaAsignacion, updddlTareaAsignacion.GetType(), "init_Js_updddlTareaAsignacion", "init_Js_updddlTareaAsignacion();", true);
            }

            if (!IsPostBack)
            {

                BandejaAsignacion.Visible = false;
                bandejaFilterAsignacion.Visible = false;

                MembershipUser user = Membership.GetUser();
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
                hid_userid.Value = Membership.GetUser().ProviderUserKey.ToString();
                MuestraTiposDeBandejas(userid);

                SiteMaster pmaster = (SiteMaster)this.Page.Master;
                ucMenu mnu = (ucMenu)pmaster.FindControl("mnu");
                mnu.setearMenuActivo(2);

            }

        }

        public List<clsItemBandejaEntrada> GetTramitesBandeja(int startRowIndex, int maximumRows,
                out int totalRowCount, string sortByExpression)
        {
            Guid userid = Functions.GetUserId();
            int cantCompletoSade = 0;
            int cantContinuarSade = 0;
            List<clsItemBandejaEntrada> lstTramites = FiltrarTramites(startRowIndex, maximumRows, sortByExpression, ref cantCompletoSade, ref cantContinuarSade, out totalRowCount);

            pnlBandejaPropiaVacia.Visible = (totalRowCount <= 0);

            lblCantTramitesBandejaPropia.Visible = true;
            lblCantSadeContinuar.Visible = true;
            //lblCantSadeCompletos.Visible = true;
            lblCantTramitesBandejaPropia.Text = "";
            lblCantSadeContinuar.Text = cantContinuarSade.ToString();
            //lblCantSadeCompletos.Text = cantCompletoSade.ToString();

            // Cantidad de tramites en la bandeja
            if (totalRowCount > 1)
                lblCantTramitesBandejaPropia.Text = string.Format("{0} trámites en la bandeja", totalRowCount);
            else if (totalRowCount == 1)
                lblCantTramitesBandejaPropia.Text = string.Format("{0} trámite en la bandeja", totalRowCount);
            else
                lblCantTramitesBandejaPropia.Visible = false;
            /*
            if (cantCompletoSade > 1)
                lblCantSadeCompletos.Text = string.Format("{0} trámites para comenzar y/o finalizar", cantCompletoSade);
            else if (cantCompletoSade == 1)
                lblCantSadeCompletos.Text = string.Format("{0} trámite para comenzar y/o finalizar", cantCompletoSade);
            else
                lblCantSadeCompletos.Visible = false;
            */
            if (cantContinuarSade > 1)
                lblCantSadeContinuar.Text = string.Format("{0} trámites para reintentar", cantContinuarSade);
            else if (cantContinuarSade == 1)
                lblCantSadeContinuar.Text = string.Format("{0} trámite para reintentar", cantContinuarSade);
            else
                lblCantSadeContinuar.Visible = false;

            return lstTramites;
        }

        public List<clsItemBandejaEntradaAsignacion> GetTramitesBandejaAsignacion(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {
            List<clsItemBandejaEntradaAsignacion> lstTramites = new List<clsItemBandejaEntradaAsignacion>();
            totalRowCount = 0;

            Guid userid = Functions.GetUserId();

            lstTramites = FiltrarTramitesAsignacion(startRowIndex, maximumRows, sortByExpression, out totalRowCount);

            pnlBandejaAsignacionVacia.Visible = (totalRowCount <= 0);

            btnSeleccionar.Visible = (totalRowCount > 0);

            lblCantTramitesBandejaAsignacion.Visible = true;
            lblCantTramitesBandejaAsignacion.Text = "";

            // Cantidad de tramites en la bandeja
            if (totalRowCount > 1)
                lblCantTramitesBandejaAsignacion.Text = string.Format("{0} trámites en la bandeja", totalRowCount);
            else if (totalRowCount == 1)
                lblCantTramitesBandejaAsignacion.Text = string.Format("{0} trámite en la bandeja", totalRowCount);
            else
                lblCantTramitesBandejaAsignacion.Visible = false;


            return lstTramites;
        }

        protected void BandejaAsignacionCheck(object sender, EventArgs e)
        {
            bool valor_check = false;
            LinkButton lnk = (LinkButton)sender;

            if (lnk.CommandArgument.Equals("0"))
                valor_check = true;      // Selecciona Todo           

            foreach (GridViewRow row in grdBandejaAsignacion.Rows)
            {
                CheckBox chkSeleccionado = (CheckBox)row.FindControl("chkSeleccionado");
                chkSeleccionado.Checked = valor_check;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "visibilidadBotonAsignar", "visibilidadBotonAsignar()", true);

        }

        protected void btnActualizarBandejaAsignacion_Click(object sender, EventArgs e)
        {
            grdBandejaAsignacion.DataBind();

            // Cuando se selecciona todo y estamos parados en la ultima pagina del grid, el grid despues del databind no llena las filas y dice que no hay filas.
            // Se necesita un segundo databind para que las llene (es un bug del gridview)
            if (grdBandejaAsignacion.Rows.Count == 0 && grdBandejaAsignacion.PageCount > 0)
                grdBandejaAsignacion.DataBind();

            ScriptManager.RegisterClientScriptBlock(updBandejaAsignacion, updBandejaAsignacion.GetType(), "", "mostratMensaje('La actualización se realizó exitosamente.')", true);

        }

        protected void btnActualizarBandejaPropia_Click(object sender, EventArgs e)
        {
            grdBandeja.DataBind();

            // Cuando se selecciona todo y estamos parados en la ultima pagina del grid, el grid despues del databind no llena las filas y dice que no hay filas.
            // Se necesita un segundo databind para que las llene (es un bug del gridview)
            if (grdBandeja.Rows.Count == 0 && grdBandeja.PageCount > 0)
                grdBandeja.DataBind();


            ScriptManager.RegisterClientScriptBlock(updBandejaPropia, updBandejaPropia.GetType(), "", "mostratMensaje('La actualización se realizó exitosamente.')", true);

        }


        protected void lnkTomarTarea_Click(object sender, EventArgs e)
        {
            string urlTarea = "";

            MembershipUser user = Membership.GetUser();
            if (user != null)
            {
                try
                {

                    LinkButton lnkTomarTarea = (LinkButton)sender;
                    GridViewRow row = (GridViewRow)lnkTomarTarea.Parent.Parent;
                    Label lblFechaAsignada = (Label)lnkTomarTarea.Parent.Parent.FindControl("lblFechaAsignada");

                    int id_solicitud = Convert.ToInt32(grdBandeja.DataKeys[row.RowIndex].Values["id_solicitud"]);
                    int id_tramitetarea = int.Parse(lnkTomarTarea.CommandArgument);
                    urlTarea = Convert.ToString(grdBandeja.DataKeys[row.RowIndex].Values["url_tareaTramite"]);
                    string cod_grupotramite = Convert.ToString(grdBandeja.DataKeys[row.RowIndex].Values["cod_grupotramite"]);

                    Engine.TomarTarea(id_tramitetarea, (Guid)user.ProviderUserKey);

                    lblFechaAsignada.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    lnkTomarTarea.Visible = false;

                }
                catch (Exception ex)
                {
                    string msg;
                    if (ex.InnerException != null)
                        msg = string.Format("mostratMensaje('{0}')", ex.InnerException.Message);
                    else
                        msg = string.Format("mostratMensaje('{0}')", ex.Message);
                    ScriptManager.RegisterClientScriptBlock(updBandejaAsignacion, updBandejaAsignacion.GetType(), "", msg, true);
                    grdBandeja.DataBind();
                }
                finally
                {
                    if (urlTarea.Length > 0)
                        Response.Redirect(urlTarea);
                }
            }
            else
                FormsAuthentication.RedirectToLoginPage();

        }

        private void MuestraTiposDeBandejas(Guid userid)
        {

            DGHP_Entities db = new DGHP_Entities();

            List<SGI_Perfiles> perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.ToList();
            var interseccion = (from perfil_usuario in perfiles_usuario select perfil_usuario.id_perfil).Intersect(from config_bandeja in db.ENG_Config_BandejaAsignacion select config_bandeja.id_perfil_asignador.Value).ToList();

            //pnlTipoBandeja.Visible = true;
            if (interseccion.Count > 0)
            {
                idTipoBandeja_txt.Visible = true;
                idTipoBandeja_btn.Visible = true;
            }
            else
            {
                idTipoBandeja_txt.Visible = false;
                idTipoBandeja_btn.Visible = false;
                bandejaFilter.Style.Remove("padding-left");
                bandejaFilter.Style.Add("padding-left", "320px");
            }
            db.Dispose();

        }

        #region "Paging gridview Bandeja Propia"


        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;
            grdBandeja.PageIndex = int.Parse(cmdPage.Text) - 1;

        }
        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdBandeja.PageIndex = grdBandeja.PageIndex - 1;

        }
        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdBandeja.PageIndex = grdBandeja.PageIndex + 1;
        }
        protected void ddlTarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["tarea"] = ddlTarea.SelectedValue;
            ddlTarea.Items.FindByValue((string)ViewState["tarea"]).Selected = true;
            grdBandeja.DataBind();
            //updBandejaPropia.Update();
        }
        protected void ddlTareaAsignacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlTareaAsignacion = (DropDownList)sender;
            ViewState["tareaAsignacion"] = ddlTareaAsignacion.SelectedValue;

            grdBandejaAsignacion.PageIndex = 0;
            grdBandejaAsignacion.DataBind();

            ddlTareaAsignacion.Items.FindByValue((string)ViewState["tareaAsignacion"]).Selected = true;
        }
        protected void ddlAsignacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["asignado"] = ddlAsignacion.SelectedValue;
            ddlAsignacion.ClearSelection();
            ddlAsignacion.Items.FindByValue((string)ViewState["asignado"]).Selected = true;
            grdBandeja.PageIndex = 0;
            grdBandeja.DataBind();
        }
        protected void grdBandeja_DataBound(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(updBandejaPropia, updBandejaPropia.GetType(), "onload", "onload();", true);
            GridView grid = (GridView)grdBandeja;
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

        #region "Paging gridview Asignacion"


        protected void cmdPageAsig(object sender, EventArgs e)
        {
            LinkButton cmdPageAsig = (LinkButton)sender;
            grdBandejaAsignacion.PageIndex = int.Parse(cmdPageAsig.Text) - 1;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "visibilidadBotonAsignar", "visibilidadBotonAsignar()", true);
        }
        protected void cmdAnteriorAsig_Click(object sender, EventArgs e)
        {
            grdBandejaAsignacion.PageIndex = grdBandejaAsignacion.PageIndex - 1;

        }
        protected void cmdSiguienteAsig_Click(object sender, EventArgs e)
        {
            grdBandejaAsignacion.PageIndex = grdBandejaAsignacion.PageIndex + 1;
        }


        protected void grdBandejaAsignacion_DataBound(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(updBandejaAsignacion, updBandejaAsignacion.GetType(), "onload", "onload();", true);

            GridView grid = (GridView)grdBandejaAsignacion;
            GridViewRow fila = (GridViewRow)grid.BottomPagerRow;

            if (fila != null)
            {
                LinkButton btnAnterior = (LinkButton)fila.Cells[0].FindControl("cmdAnteriorAsig");
                LinkButton btnSiguiente = (LinkButton)fila.Cells[0].FindControl("cmdSiguienteAsig");
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
                    LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPageAsig" + i.ToString());
                    btn.Visible = false;
                }

                if (grid.PageIndex == 0 || grid.PageCount <= 10)
                {
                    // Mostrar 10 botones o el máximo de páginas

                    for (int i = 1; i <= 10; i++)
                    {
                        if (i <= grid.PageCount)
                        {
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPageAsig" + i.ToString());
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

                    LinkButton btnPage10 = (LinkButton)fila.Cells[0].FindControl("cmdPageAsig10");
                    btnPage10.Visible = true;
                    btnPage10.Text = Convert.ToString(grid.PageIndex + 1);

                    // Ubica los 9 botones hacia la izquierda
                    for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                    {
                        CantBucles++;
                        if (i >= 0)
                        {
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPageAsig" + Convert.ToString(10 - CantBucles));
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
                            LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPageAsig" + Convert.ToString(10 + CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                        }
                    }
                }
                LinkButton cmdPage;
                string btnPage = "";
                for (int i = 1; i <= 19; i++)
                {
                    btnPage = "cmdPageAsig" + i.ToString();
                    cmdPage = (LinkButton)fila.Cells[0].FindControl(btnPage);
                    if (cmdPage != null)
                        cmdPage.CssClass = "btn";
                }

                // busca el boton por el texto para marcarlo como seleccionado
                string btnText = Convert.ToString(grid.PageIndex + 1);
                foreach (Control ctl in fila.Cells[0].FindControl("pnlpagerAsig").Controls)
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

        protected void btnPropia_click(object sender, EventArgs e)
        {
            BandejaAsignacion.Visible = false;
            bandejaFilterAsignacion.Visible = false;
            bandejaFilter.Visible = true;
            BandejaPropia.Visible = true;
            btnPropia.CssClass = "btn active";
            btnAsignacion.CssClass = "btn";
        }
        protected void btnAsignacion_click(object sender, EventArgs e)
        {
            BandejaAsignacion.Visible = true;
            bandejaFilterAsignacion.Visible = true;
            bandejaFilter.Visible = false;
            BandejaPropia.Visible = false;
            btnAsignacion.CssClass = "btn active";
            btnPropia.CssClass = "btn";
        }
        #region "Filtro"
        private List<clsItemBandejaEntrada> FiltrarTramites(int startRowIndex, int maximumRows, string sortByExpression, ref int cantCompletoSade, ref int cantContinuarSade, out int totalRowCount)
        {

            List<clsItemBandejaEntrada> resultados = new List<clsItemBandejaEntrada>();
            IQueryable<clsItemBandejaEntrada> qFinal = null;
            IQueryable<clsItemBandejaEntrada> qENC = null;
            IQueryable<clsItemBandejaEntrada> qCP = null;
            IQueryable<clsItemBandejaEntrada> qTR = null;

            DGHP_Entities db = new DGHP_Entities();
            Guid userid = Functions.GetUserId();

            //perfiles usuario
            var u = db.aspnet_Users.Where(s => s.UserId == userid).FirstOrDefault();
            List<int> perfiles = u.SGI_PerfilesUsuarios.Select(s => s.id_perfil).ToList();

            // Bandeja de datos Sol
            #region "Consulta Solicitudes"

            qENC = (from perfiles_tareas in db.ENG_Rel_Perfiles_Tareas
                    join tramite_tareas in db.SGI_Tramites_Tareas on perfiles_tareas.id_tarea equals tramite_tareas.id_tarea
                    join tramite_tareas_HAB in db.SGI_Tramites_Tareas_HAB on tramite_tareas.id_tramitetarea equals tramite_tareas_HAB.id_tramitetarea
                    join tarea in db.ENG_Tareas on tramite_tareas.id_tarea equals tarea.id_tarea
                    join sol in db.SSIT_Solicitudes on tramite_tareas_HAB.id_solicitud equals sol.id_solicitud
                    where perfiles.Contains(perfiles_tareas.id_perfil)
                        && tramite_tareas.FechaCierre_tramitetarea == null
                        && (tramite_tareas.UsuarioAsignado_tramitetarea == null || tramite_tareas.UsuarioAsignado_tramitetarea == @userid)
                        && ((tarea.Asignable_tarea == false) || (tarea.Asignable_tarea == true && tramite_tareas.UsuarioAsignado_tramitetarea != null))
                        && sol.id_estado != (int)Constants.Solicitud_Estados.Anulado
                    // && enc.FechaEncomienda == (from en in db.Encomienda where en.Encomienda_SSIT_Solicitudes.FirstOrDefault().id_solicitud == sol.id_solicitud && en.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo select en.FechaEncomienda).Max()

                    select new clsItemBandejaEntrada
                    {
                        cod_grupotramite = Constants.GruposDeTramite.HAB.ToString(),
                        tipoTramite = sol.TipoTramite.descripcion_tipotramite,
                        id_tipoTramite = sol.id_tipotramite,
                        id_tramitetarea = tramite_tareas.id_tramitetarea,
                        FechaInicio_tramitetarea = tramite_tareas.FechaInicio_tramitetarea,
                        FechaAsignacion_tramtietarea = tramite_tareas.FechaAsignacion_tramtietarea,
                        id_solicitud = sol.id_solicitud,
                        direccion = "",
                        id_tarea = tarea.id_tarea,
                        nombre_tarea = tarea.nombre_tarea,
                        asignable_tarea = tarea.Asignable_tarea,
                        tomar_tarea = (tarea.id_tarea != 25 && tarea.id_tarea != 49) ? true : false,//Correcion de solicitudes
                        formulario_tarea = tarea.formulario_tarea,
                        Dias_Transcurridos = 0,
                        Dias_Acumulados = 0,
                        superficie_total = 0,
                        continuar_sade = (from sade_proc in db.SGI_SADE_Procesos
                                          where sade_proc.id_tramitetarea == tramite_tareas.id_tramitetarea
                                          && sade_proc.id_proceso != 1
                                          select sade_proc).Any() ? (
                                         (from sade_proc in db.SGI_SADE_Procesos
                                          where sade_proc.id_tramitetarea == tramite_tareas.id_tramitetarea
                                          && sade_proc.id_proceso != 1
                                          select new
                                          {
                                              pasarela = sade_proc.realizado_en_pasarela ? 1 : 0,
                                              sade = sade_proc.realizado_en_SADE ? 1 : 0
                                          }).Sum(p => p.pasarela - p.sade) == 0 ? 1 : 0) : 1,
                        sade_completo = (from sade_proc in db.SGI_SADE_Procesos
                                         where sade_proc.id_tramitetarea == tramite_tareas.id_tramitetarea
                                         && sade_proc.id_proceso != 1
                                         select sade_proc).All(sade_proc => sade_proc.realizado_en_pasarela &&
                                                                             sade_proc.realizado_en_SADE) ? 1 : 0,
                        cant_observaciones = sol.SSIT_Solicitudes_Observaciones.Count(),
                        url_visorTramite = "~/GestionTramite/VisorTramite.aspx?id={0}",
                        url_tareaTramite = "~/GestionTramite/Tareas/{0}?id={1}",
                        zona_declarada = "",
                        nombre_resultado = (from tt in db.SGI_Tramites_Tareas
                                            join ttt in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals ttt.id_tramitetarea
                                            join r in db.ENG_Resultados on tt.id_resultado equals r.id_resultado
                                            where ttt.id_solicitud == sol.id_solicitud 
                                               && ttt.id_tramitetarea == (from t2 in db.SGI_Tramites_Tareas_HAB
                                                                          join tt2 in db.SGI_Tramites_Tareas on t2.id_tramitetarea equals tt2.id_tramitetarea
                                                                          join ta2 in db.ENG_Tareas on tt2.id_tarea equals ta2.id_tarea
                                                                          where t2.id_solicitud == ttt.id_solicitud 
                                                                          && t2.id_tramitetarea < tramite_tareas.id_tramitetarea 
                                                                          && new[] {"01", "10"}.Contains(ta2.cod_tarea.ToString().Substring(ta2.cod_tarea.ToString().Length - 2, 2))
                                                                          select t2.id_tramitetarea).Max()
                                            select r.nombre_resultado).FirstOrDefault()

        }).Distinct();

            #endregion

            // Consulta de datos CPadron
            #region "Consulta CPadron"
            qCP = (from perfiles_tareas in db.ENG_Rel_Perfiles_Tareas
                   join tramite_tareas in db.SGI_Tramites_Tareas on perfiles_tareas.id_tarea equals tramite_tareas.id_tarea
                   join tramite_tareas_CP in db.SGI_Tramites_Tareas_CPADRON on tramite_tareas.id_tramitetarea equals tramite_tareas_CP.id_tramitetarea
                   join tarea in db.ENG_Tareas on tramite_tareas.id_tarea equals tarea.id_tarea
                   join sol in db.CPadron_Solicitudes on tramite_tareas_CP.id_cpadron equals sol.id_cpadron
                   join eDatos in db.CPadron_DatosLocal on sol.id_cpadron equals eDatos.id_cpadron into zr
                   from ed in zr.DefaultIfEmpty()

                   where perfiles.Contains(perfiles_tareas.id_perfil)
                       && tramite_tareas.FechaCierre_tramitetarea == null
                       && (tramite_tareas.UsuarioAsignado_tramitetarea == null || tramite_tareas.UsuarioAsignado_tramitetarea == @userid)
                       && ((tarea.Asignable_tarea == false) || (tarea.Asignable_tarea == true && tramite_tareas.UsuarioAsignado_tramitetarea != null))
                       && sol.id_estado != (int)Constants.CPadron_EstadoSolicitud.Anulado
                   select new clsItemBandejaEntrada
                   {
                       cod_grupotramite = Constants.GruposDeTramite.CP.ToString(),
                       tipoTramite = sol.TipoTramite.descripcion_tipotramite,
                       id_tipoTramite = sol.id_tipotramite,
                       id_tramitetarea = tramite_tareas.id_tramitetarea,
                       FechaInicio_tramitetarea = tramite_tareas.FechaInicio_tramitetarea,
                       FechaAsignacion_tramtietarea = tramite_tareas.FechaAsignacion_tramtietarea,
                       id_solicitud = sol.id_cpadron,
                       direccion = "",
                       id_tarea = tarea.id_tarea,
                       nombre_tarea = tarea.nombre_tarea,
                       asignable_tarea = tarea.Asignable_tarea,
                       tomar_tarea = tarea.id_tarea != 71 ? true : false,//Correcion de solicitudes
                       formulario_tarea = tarea.formulario_tarea,
                       Dias_Transcurridos = 0,
                       Dias_Acumulados = 0,
                       superficie_total = ed != null ? (ed.superficie_cubierta_dl.Value + ed.superficie_descubierta_dl.Value) : 0,
                       continuar_sade = (from sade_proc in db.SGI_SADE_Procesos
                                         where sade_proc.id_tramitetarea == tramite_tareas.id_tramitetarea
                                         && sade_proc.id_proceso != 1
                                         select sade_proc).Any() ? (
                                         (from sade_proc in db.SGI_SADE_Procesos
                                          where sade_proc.id_tramitetarea == tramite_tareas.id_tramitetarea
                                          && sade_proc.id_proceso != 1
                                          select new
                                          {
                                              pasarela = sade_proc.realizado_en_pasarela ? 1 : 0,
                                              sade = sade_proc.realizado_en_SADE ? 1 : 0
                                          }).Sum(p => p.pasarela - p.sade) == 0 ? 1 : 0) : 1,
                       sade_completo = (from sade_proc in db.SGI_SADE_Procesos
                                        where sade_proc.id_tramitetarea == tramite_tareas.id_tramitetarea
                                        && sade_proc.id_proceso != 1
                                        select sade_proc).All(sade_proc => sade_proc.realizado_en_pasarela &&
                                                                            sade_proc.realizado_en_SADE) ? 1 : 0,
                       cant_observaciones = sol.CPadron_Solicitudes_Observaciones.Count(),
                       url_visorTramite = "~/VisorTramiteCP/{0}",
                       url_tareaTramite = "~/GestionTramite/Tareas/{0}?id={1}",
                       zona_declarada = sol.ZonaDeclarada,
                       nombre_resultado = (from tt in db.SGI_Tramites_Tareas
                                           join ttt in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals ttt.id_tramitetarea
                                           join r in db.ENG_Resultados on tt.id_resultado equals r.id_resultado
                                           where ttt.id_cpadron == sol.id_cpadron 
                                              && ttt.id_tramitetarea == (from t2 in db.SGI_Tramites_Tareas_CPADRON
                                                                         join tt2 in db.SGI_Tramites_Tareas on t2.id_tramitetarea equals tt2.id_tramitetarea
                                                                         join ta2 in db.ENG_Tareas on tt2.id_tarea equals ta2.id_tarea
                                                                         where t2.id_cpadron == ttt.id_cpadron 
                                                                         && t2.id_tramitetarea < tramite_tareas.id_tramitetarea 
                                                                         && new[] {"01", "10"}.Contains(ta2.cod_tarea.ToString().Substring(ta2.cod_tarea.ToString().Length - 2, 2))
                                                                         select t2.id_tramitetarea).Max()
                                           select r.nombre_resultado).FirstOrDefault()

        }).Distinct();
            #endregion

            // Bandeja de datos Transferencias
            #region "Consulta Transferencias"


            qTR = (from perfiles_tareas in db.ENG_Rel_Perfiles_Tareas
                   join tramite_tareas in db.SGI_Tramites_Tareas on perfiles_tareas.id_tarea equals tramite_tareas.id_tarea
                   join tramite_tareas_TR in db.SGI_Tramites_Tareas_TRANSF on tramite_tareas.id_tramitetarea equals tramite_tareas_TR.id_tramitetarea
                   join tarea in db.ENG_Tareas on tramite_tareas.id_tarea equals tarea.id_tarea
                   join sol in db.Transf_Solicitudes on tramite_tareas_TR.id_solicitud equals sol.id_solicitud
                   join cpsol in db.CPadron_Solicitudes on sol.id_cpadron equals cpsol.id_cpadron
                   join eDatos in db.CPadron_DatosLocal on sol.id_cpadron equals eDatos.id_cpadron into zr
                   from ed in zr.DefaultIfEmpty()

                   where perfiles.Contains(perfiles_tareas.id_perfil)
                       && tramite_tareas.FechaCierre_tramitetarea == null
                       && (tramite_tareas.UsuarioAsignado_tramitetarea == null || tramite_tareas.UsuarioAsignado_tramitetarea == @userid)
                       && ((tarea.Asignable_tarea == false) || (tarea.Asignable_tarea == true && tramite_tareas.UsuarioAsignado_tramitetarea != null))
                       && sol.id_estado != (int)Constants.Solicitud_Estados.Anulado
                   select new clsItemBandejaEntrada
                   {
                       cod_grupotramite = Constants.GruposDeTramite.TR.ToString(),
                       tipoTramite = sol.TipoTramite.descripcion_tipotramite,
                       id_tipoTramite = sol.id_tipotramite,
                       id_tramitetarea = tramite_tareas.id_tramitetarea,
                       FechaInicio_tramitetarea = tramite_tareas.FechaInicio_tramitetarea,
                       FechaAsignacion_tramtietarea = tramite_tareas.FechaAsignacion_tramtietarea,
                       id_solicitud = sol.id_solicitud,
                       direccion = "",
                       id_tarea = tarea.id_tarea,
                       nombre_tarea = tarea.nombre_tarea,
                       asignable_tarea = tarea.Asignable_tarea,
                       tomar_tarea = (tarea.id_tarea != 60) ? true : false,//Correcion de solicitudes
                       formulario_tarea = tarea.formulario_tarea,
                       Dias_Transcurridos = 0,
                       Dias_Acumulados = 0,
                       superficie_total = ed != null ? (ed.superficie_cubierta_dl.Value + ed.superficie_descubierta_dl.Value) : 0,
                       continuar_sade = (from sade_proc in db.SGI_SADE_Procesos
                                         where sade_proc.id_tramitetarea == tramite_tareas.id_tramitetarea
                                         && sade_proc.id_proceso != 1
                                         select sade_proc).Any() ? (
                                         (from sade_proc in db.SGI_SADE_Procesos
                                          where sade_proc.id_tramitetarea == tramite_tareas.id_tramitetarea
                                          && sade_proc.id_proceso != 1
                                          select new
                                          {
                                              pasarela = sade_proc.realizado_en_pasarela ? 1 : 0,
                                              sade = sade_proc.realizado_en_SADE ? 1 : 0
                                          }).Sum(p => p.pasarela - p.sade) == 0 ? 1 : 0) : 1,
                       sade_completo = (from sade_proc in db.SGI_SADE_Procesos
                                        where sade_proc.id_tramitetarea == tramite_tareas.id_tramitetarea
                                        && sade_proc.id_proceso != 1
                                        select sade_proc).All(sade_proc => sade_proc.realizado_en_pasarela &&
                                                                            sade_proc.realizado_en_SADE) ? 1 : 0,
                       cant_observaciones = sol.Transf_Solicitudes_Observaciones.Count(),
                       url_visorTramite = "~/VisorTramiteTR/{0}",
                       url_tareaTramite = "~/GestionTramite/Tareas/{0}?id={1}",
                       zona_declarada = cpsol.ZonaDeclarada,
                       nombre_resultado = (from tt in db.SGI_Tramites_Tareas
                                           join ttt in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals ttt.id_tramitetarea
                                           join r in db.ENG_Resultados on tt.id_resultado equals r.id_resultado
                                           where ttt.id_solicitud == sol.id_solicitud 
                                              && ttt.id_tramitetarea == (from t2 in db.SGI_Tramites_Tareas_TRANSF
                                                                         join tt2 in db.SGI_Tramites_Tareas on t2.id_tramitetarea equals tt2.id_tramitetarea
                                                                         join ta2 in db.ENG_Tareas on tt2.id_tarea equals ta2.id_tarea
                                                                         where t2.id_solicitud == ttt.id_solicitud 
                                                                         && t2.id_tramitetarea < tramite_tareas.id_tramitetarea 
                                                                         && new[] {"01", "10"}.Contains(ta2.cod_tarea.ToString().Substring(ta2.cod_tarea.ToString().Length - 2, 2))
                                                                         select t2.id_tramitetarea).Max()
                                           select r.nombre_resultado).FirstOrDefault()
                   }).Distinct();
            #endregion

            var qTareaENC = (from enc in qENC
                             join tareas in db.ENG_Tareas on enc.id_tarea equals tareas.id_tarea
                             join cir in db.ENG_Circuitos on tareas.id_circuito equals cir.id_circuito
                             join sol in db.SSIT_Solicitudes on enc.id_solicitud equals sol.id_solicitud
                             select new
                             {
                                 id_tipoTramite = sol.id_tipotramite,
                                 tipotramite = enc.tipoTramite,
                                 nombre_tarea = enc.nombre_tarea,
                                 id_tarea = enc.id_tarea,
                                 cir.id_circuito,
                                 cir.cod_circuito
                             }).ToList().Distinct();

            var qTareaCP = (from enc in qCP
                            join tareas in db.ENG_Tareas on enc.id_tarea equals tareas.id_tarea
                            join cir in db.ENG_Circuitos on tareas.id_circuito equals cir.id_circuito
                            join sol in db.CPadron_Solicitudes on enc.id_solicitud equals sol.id_cpadron
                            select new
                            {
                                id_tipoTramite = sol.id_tipotramite,
                                tipotramite = enc.tipoTramite,
                                nombre_tarea = enc.nombre_tarea,
                                id_tarea = enc.id_tarea,
                                cir.id_circuito,
                                cir.cod_circuito
                            }).ToList().Distinct();

            var qTareaTR = (from enc in qTR
                            join tareas in db.ENG_Tareas on enc.id_tarea equals tareas.id_tarea
                            join cir in db.ENG_Circuitos on tareas.id_circuito equals cir.id_circuito
                            join sol in db.Transf_Solicitudes on enc.id_solicitud equals sol.id_solicitud
                            select new
                            {
                                id_tipoTramite = sol.id_tipotramite,
                                tipotramite = enc.tipoTramite,
                                nombre_tarea = enc.nombre_tarea,
                                id_tarea = enc.id_tarea,
                                cir.id_circuito,
                                cir.cod_circuito
                            }).ToList().Distinct();

            var qTarea = qTareaENC.Union(qTareaCP).Union(qTareaTR).ToList().Distinct();

            //cargar combos tipo de tramite

            var lttramite = qTarea.Select(x => new { x.id_tipoTramite, x.tipotramite }).Distinct().ToList().OrderBy(x => x.tipotramite);

            List<TipoTramite> lstTTramite = new List<TipoTramite>();
            var tTtramite = new TipoTramite();
            tTtramite.id_tipotramite = 0;
            tTtramite.descripcion_tipotramite = "Todos";
            lstTTramite.Add(tTtramite);

            foreach (var item in lttramite)
            {
                tTtramite = new TipoTramite();
                tTtramite.id_tipotramite = item.id_tipoTramite;
                tTtramite.descripcion_tipotramite = item.tipotramite;
                lstTTramite.Add(tTtramite);
            }

            ddlTipoTramite.DataSource = lstTTramite;
            ddlTipoTramite.DataTextField = "descripcion_tipotramite";
            ddlTipoTramite.DataValueField = "id_tipotramite";
            ddlTipoTramite.DataBind();

            if (ViewState["tipoTramite"] != null)
            {
                string ttramite = ViewState["tipoTramite"].ToString();
                ddlTipoTramite.Items.FindByValue(ttramite).Selected = true;
            }
            else
            {
                ddlTipoTramite.Items[0].Selected = true;
            }

            if (ddlTipoTramite.SelectedItem.Value != "0")
            {
                int idTipoTramite = int.Parse(ddlTipoTramite.SelectedValue);
                qENC = (from res in qENC
                        where res.id_tipoTramite == idTipoTramite
                        select res);
                qCP = (from res in qCP
                       where res.id_tipoTramite == idTipoTramite
                       select res);
                qTR = (from res in qTR
                       where res.id_tipoTramite == idTipoTramite
                       select res);
            }

            List<ENG_Tareas> lstTareas = new List<ENG_Tareas>();
            var itarea = new ENG_Tareas();
            itarea.id_tarea = 0;
            itarea.nombre_tarea = "Todas";
            lstTareas.Add(itarea);

            var filtroTarea = qTarea;

            if (ddlTipoTramite.SelectedItem.Value != "0")
            {
                filtroTarea = qTarea.Where(x => x.id_tipoTramite == Convert.ToInt32(ViewState["tipoTramite"].ToString())).ToList();
            }

            foreach (var item in filtroTarea)
            {
                itarea = new ENG_Tareas();
                itarea.id_tarea = item.id_tarea;

                var grupo = db.ENG_Rel_Circuitos_TiposDeTramite.Where(x => x.id_circuito == item.id_circuito).FirstOrDefault();
                if (grupo != null && grupo.id_grupo_circuito != null)
                    itarea.nombre_tarea = grupo.ENG_Grupos_Circuitos.cod_grupo_circuito + " - " + item.nombre_tarea;
                else
                    itarea.nombre_tarea = item.cod_circuito + " - " + item.nombre_tarea;

                itarea.id_circuito = item.id_circuito;
                lstTareas.Add(itarea);
            }

            ddlTarea.DataSource = lstTareas;
            ddlTarea.DataTextField = "nombre_tarea";
            ddlTarea.DataValueField = "id_tarea";
            ddlTarea.DataBind();

            if (ViewState["tarea"] != null)
            {
                string t = ViewState["tarea"].ToString();
                ddlTarea.Items.FindByValue(t).Selected = true;
            }
            else
            {
                ddlTarea.Items[0].Selected = true;
            }

            if (ddlTarea.SelectedItem.Value != "0")
            {
                if (ddlTipoTramite.SelectedItem.Value == "0")
                {
                    int id_tar = int.Parse(ddlTarea.SelectedValue);
                    qENC = (from res in qENC
                            where res.id_tarea == id_tar
                            select res);
                    qCP = (from res in qCP
                           where res.id_tarea == id_tar
                           select res);
                    qTR = (from res in qTR
                           where res.id_tarea == id_tar
                           select res);
                }
                else
                {
                    int id_tar = int.Parse(ddlTarea.SelectedValue);
                    qENC = (from res in qENC
                            where res.id_tarea == id_tar
                            select res);
                    qCP = (from res in qCP
                           where res.id_tarea == id_tar
                           select res);
                    qTR = (from res in qTR
                           where res.id_tarea == id_tar
                           select res);
                }

            }

            if (ViewState["asignado"] != null)
            {
                string t = ViewState["asignado"].ToString();
                ddlAsignacion.Items.FindByValue(t).Selected = true;
            }
            else
            {
                ddlAsignacion.Items[0].Selected = true;
            }

            if (ddlAsignacion.SelectedItem.Value != "")
            {
                bool asig = bool.Parse(ddlAsignacion.SelectedValue);
                if (asig)
                {
                    qENC = (from res in qENC
                            where res.FechaAsignacion_tramtietarea != null
                            select res);
                    qCP = (from res in qCP
                           where res.FechaAsignacion_tramtietarea != null
                           select res);
                    qTR = (from res in qTR
                           where res.FechaAsignacion_tramtietarea != null
                           select res);
                }
                else
                {
                    qENC = (from res in qENC
                            where res.FechaAsignacion_tramtietarea == null
                            select res);
                    qCP = (from res in qCP
                           where res.FechaAsignacion_tramtietarea == null
                           select res);
                    qTR = (from res in qTR
                           where res.FechaAsignacion_tramtietarea == null
                           select res);
                }

            }


            AddQueryFinal(qENC, ref qFinal);
            AddQueryFinal(qCP, ref qFinal);
            AddQueryFinal(qTR, ref qFinal);

            totalRowCount = qFinal.Count();
            cantCompletoSade = qFinal.Count(x => x.sade_completo == 1);
            cantContinuarSade = qFinal.Count(x => x.continuar_sade == 1 && x.sade_completo != 1);

            if (sortByExpression != null)
            {
                if (sortByExpression.Contains("DESC"))
                {
                    if (sortByExpression.Contains("id_solicitud"))
                        qFinal = qFinal.OrderByDescending(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("Dias_Transcurridos"))
                        qFinal = qFinal.OrderByDescending(o => o.Dias_Transcurridos).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("Dias_Acumulados"))
                        qFinal = qFinal.OrderByDescending(o => o.Dias_Acumulados).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("direccion"))
                        qFinal = qFinal.OrderByDescending(o => o.direccion).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("FechaAsignacion_tarea"))
                        qFinal = qFinal.OrderByDescending(o => o.FechaAsignacion_tramtietarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("FechaInicio_tarea"))
                        qFinal = qFinal.OrderByDescending(o => o.FechaInicio_tramitetarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("nombre_tarea"))
                        qFinal = qFinal.OrderByDescending(o => o.nombre_tarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("superficie_total"))
                        qFinal = qFinal.OrderByDescending(o => o.superficie_total).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("tipoTramite"))
                        qFinal = qFinal.OrderByDescending(o => o.tipoTramite).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("cant_observaciones"))
                        qFinal = qFinal.OrderByDescending(o => o.cant_observaciones).Skip(startRowIndex).Take(maximumRows);
                    else
                        qFinal = qFinal.OrderByDescending(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);
                }
                else
                {
                    if (sortByExpression.Contains("id_solicitud"))
                        qFinal = qFinal.OrderBy(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("Dias_Transcurridos"))
                        qFinal = qFinal.OrderBy(o => o.Dias_Transcurridos).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("Dias_Acumulados"))
                        qFinal = qFinal.OrderBy(o => o.Dias_Acumulados).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("direccion"))
                        qFinal = qFinal.OrderBy(o => o.direccion).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("FechaAsignacion_tarea"))
                        qFinal = qFinal.OrderBy(o => o.FechaAsignacion_tramtietarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("FechaInicio_tarea"))
                        qFinal = qFinal.OrderBy(o => o.FechaInicio_tramitetarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("nombre_tarea"))
                        qFinal = qFinal.OrderBy(o => o.nombre_tarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("superficie_total"))
                        qFinal = qFinal.OrderBy(o => o.superficie_total).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("tipoTramite"))
                        qFinal = qFinal.OrderBy(o => o.tipoTramite).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("cant_observaciones"))
                        qFinal = qFinal.OrderBy(o => o.cant_observaciones).Skip(startRowIndex).Take(maximumRows);
                    else
                        qFinal = qFinal.OrderBy(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);
                }
            }
            else
                qFinal = qFinal.OrderBy(o => o.FechaInicio_tramitetarea).Skip(startRowIndex).Take(maximumRows);

            resultados = qFinal.ToList();

            int nroTrReferencia = 0;
            int.TryParse(Parametros.GetParam_ValorChar("NroTransmisionReferencia"), out nroTrReferencia);
            #region "Domicilios y datos adicionales"
            if (resultados.Count > 0)
            {

                //------------------------------
                //Obtener las Direcciones del ENC
                //-------------------------------
                List<clsItemDireccion> lstDireccionesENC = new List<clsItemDireccion>();
                string[] arrSolicitudesENC = (from r in resultados
                                              where r.cod_grupotramite == Constants.GruposDeTramite.HAB.ToString()
                                              select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesENC.Length > 0)
                    lstDireccionesENC = Shared.GetDireccionesENC(arrSolicitudesENC);

                //------------------------------
                //Obtener las Direcciones del ENC
                //-------------------------------
                List<clsItemDireccion> lstDireccionesCP = new List<clsItemDireccion>();
                string[] arrSolicitudesCP = (from r in resultados
                                             where r.cod_grupotramite == Constants.GruposDeTramite.CP.ToString()
                                             select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesCP.Length > 0)
                    lstDireccionesCP = Shared.GetDireccionesCP(arrSolicitudesCP);

                //------------------------------
                //Obtener las Direcciones del ENC
                //-------------------------------
                List<clsItemDireccion> lstDireccionesTR = new List<clsItemDireccion>();
                string[] arrSolicitudesTR = (from r in resultados
                                             where r.cod_grupotramite == Constants.GruposDeTramite.TR.ToString()
                                             && r.id_solicitud <= nroTrReferencia
                                             select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesTR.Length > 0)
                    lstDireccionesTR = Shared.GetDireccionesTR(arrSolicitudesTR);

                string[] arrSolicitudesTRNuevas = (from r in resultados
                                                   where r.cod_grupotramite == Constants.GruposDeTramite.TR.ToString()
                                                   && r.id_solicitud > nroTrReferencia
                                                   select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesTRNuevas.Length > 0)
                    lstDireccionesTR.AddRange(Shared.GetDireccionesTRNuevas(arrSolicitudesTRNuevas));

                var listGrup = (from g in db.SGI_Tarea_Calificar_ObsGrupo
                                join tt in db.SGI_Tramites_Tareas_HAB on g.id_tramitetarea equals tt.id_tramitetarea
                                select new { tt.id_solicitud }).ToList();

                //------------------------------------------------------------------------
                //Rellena la clase a devolver con los datos que faltaban (Direccion, dias transcurrido)
                //------------------------------------------------------------------------

                foreach (var row in resultados)
                {
                    clsItemDireccion itemDireccion = null;
                    if (row.cod_grupotramite == Constants.GruposDeTramite.HAB.ToString())
                    {
                        itemDireccion = lstDireccionesENC.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);
                        int count = listGrup.Count(x => x.id_solicitud == row.id_solicitud);
                        if (count > 0)
                            row.cant_observaciones = count;
                        var enc = db.Encomienda_SSIT_Solicitudes.Where(x => x.id_solicitud == row.id_solicitud &&
                                   x.Encomienda.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).
                                   OrderByDescending(x => x.Encomienda.id_encomienda).Select(x => x.Encomienda).FirstOrDefault();
                        if (enc != null)
                        {
                            var dl = enc.Encomienda_DatosLocal.FirstOrDefault();
                            if (dl != null)
                                row.superficie_total = dl.superficie_cubierta_dl.Value + dl.superficie_descubierta_dl.Value;


                            row.Rubros = (from er in enc.Encomienda_Rubros
                                          select new clsItemBandejaEntradaRubros
                                          {
                                              cod_rubro = er.cod_rubro,
                                              desc_rubro = er.desc_rubro
                                          }).Union
                                          (from er in enc.Encomienda_RubrosCN
                                           select new clsItemBandejaEntradaRubros
                                           {
                                               cod_rubro = er.CodigoRubro,
                                               desc_rubro = er.NombreRubro
                                           }).ToList();
                        }
                    }
                    else if (row.cod_grupotramite == Constants.GruposDeTramite.CP.ToString())
                    {
                        itemDireccion = lstDireccionesCP.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);

                        row.Rubros = (from cpr in db.CPadron_Rubros.Where(x => x.id_cpadron == row.id_solicitud)
                                      select new clsItemBandejaEntradaRubros
                                      {
                                          cod_rubro = cpr.cod_rubro,
                                          desc_rubro = cpr.desc_rubro
                                      }).ToList();
                    }
                    else if (row.cod_grupotramite == Constants.GruposDeTramite.TR.ToString())
                    {
                        itemDireccion = lstDireccionesTR.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);

                        if (row.id_solicitud > nroTrReferencia)
                        {
                            var enc = db.Encomienda_Transf_Solicitudes.Where(x => x.id_solicitud == row.id_solicitud &&
                                       x.Encomienda.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).
                                       OrderByDescending(x => x.Encomienda.id_encomienda).Select(x => x.Encomienda).FirstOrDefault();
                            if (enc != null)
                            {
                                var dl = enc.Encomienda_DatosLocal.FirstOrDefault();
                                if (dl != null)
                                    row.superficie_total = dl.superficie_cubierta_dl.Value + dl.superficie_descubierta_dl.Value;
                            }
                        }
                        row.Rubros = (from tr in db.Transf_Solicitudes
                                      join cpr in db.CPadron_Rubros on tr.id_cpadron equals cpr.id_cpadron
                                      where tr.id_solicitud == row.id_solicitud
                                      select new clsItemBandejaEntradaRubros
                                      {
                                          cod_rubro = cpr.cod_rubro,
                                          desc_rubro = cpr.desc_rubro
                                      }).Union
                                (from enc in db.Encomienda_Transf_Solicitudes
                                 join encrub in db.Encomienda_RubrosCN on enc.id_encomienda equals encrub.id_encomienda
                                 where enc.id_solicitud == row.id_solicitud
                                 select new clsItemBandejaEntradaRubros
                                 {
                                     cod_rubro = encrub.CodigoRubro,
                                     desc_rubro = encrub.NombreRubro
                                 }
                                        ).ToList();
                    }

                    // Llenado para todos
                    if (itemDireccion != null)
                        row.direccion = (string.IsNullOrEmpty(itemDireccion.direccion) ? "" : itemDireccion.direccion);
                    row.url_visorTramite = string.Format(row.url_visorTramite, row.id_solicitud.ToString());
                    if (row.formulario_tarea != null)
                    {
                        row.url_tareaTramite = string.Format(row.url_tareaTramite, row.formulario_tarea.Substring(0, row.formulario_tarea.IndexOf(".")), row.id_tramitetarea.ToString());
                    }
                    else
                        row.url_tareaTramite = "";
                        row.Dias_Transcurridos = Shared.GetBusinessDays(row.FechaInicio_tramitetarea, DateTime.Now);


                    int firstTramiteTarea = 0;
                    int idTramiteTarea = 0;

                    if (row.cod_grupotramite == Constants.GruposDeTramite.HAB.ToString())
                    {
                        firstTramiteTarea = (from th in db.SGI_Tramites_Tareas_HAB
                                             where th.id_solicitud == row.id_solicitud
                                             select th.id_tramitetarea).Min();

                        idTramiteTarea = (from th in db.SGI_Tramites_Tareas_HAB
                                          where th.id_solicitud == row.id_solicitud
                                          & th.id_tramitetarea > firstTramiteTarea
                                          select th.id_tramitetarea).Min();
                    }
                    else if (row.cod_grupotramite == Constants.GruposDeTramite.CP.ToString())
                    {
                        firstTramiteTarea = (from th in db.SGI_Tramites_Tareas_CPADRON
                                             where th.id_cpadron == row.id_solicitud
                                             select th.id_tramitetarea).Min();

                        idTramiteTarea = (from th in db.SGI_Tramites_Tareas_CPADRON
                                          where th.id_cpadron == row.id_solicitud
                                          & th.id_tramitetarea > firstTramiteTarea
                                          select th.id_tramitetarea).Min();
                    }
                    else if (row.cod_grupotramite == Constants.GruposDeTramite.TR.ToString())
                    {
                        firstTramiteTarea = (from th in db.SGI_Tramites_Tareas_TRANSF
                                             where th.id_solicitud == row.id_solicitud
                                             select th.id_tramitetarea).Min();

                        idTramiteTarea = (from th in db.SGI_Tramites_Tareas_TRANSF
                                          where th.id_solicitud == row.id_solicitud
                                          & th.id_tramitetarea > firstTramiteTarea
                                          select th.id_tramitetarea).Min();
                    }

                    DateTime fechaInicio;

                    if (idTramiteTarea > 0)
                    {
                        fechaInicio = (from t in db.SGI_Tramites_Tareas
                                       where t.id_tramitetarea == idTramiteTarea
                                       select t.FechaInicio_tramitetarea).FirstOrDefault();

                        row.Dias_Acumulados = Shared.GetBusinessDays(fechaInicio, DateTime.Now);
                    }
                    else
                    {
                        row.Dias_Acumulados = 0;
                    }

                }
            }

            #endregion
            db.Dispose();


            return resultados;
        }

        private void AddQueryFinal(IQueryable<clsItemBandejaEntrada> query, ref IQueryable<clsItemBandejaEntrada> qFinal)
        {
            if (query != null)
            {
                if (qFinal != null)
                    qFinal = qFinal.Union(query);
                else
                    qFinal = query;
            }
        }

        private List<clsItemBandejaEntradaAsignacion> FiltrarTramitesAsignacion(int startRowIndex, int maximumRows, string sortByExpression, out int totalRowCount)
        {


            List<clsItemBandejaEntradaAsignacion> resultados = new List<clsItemBandejaEntradaAsignacion>();
            IQueryable<clsItemBandejaEntradaAsignacion> qFinal = null;
            IQueryable<clsItemBandejaEntradaAsignacion> qENC = null;
            IQueryable<clsItemBandejaEntradaAsignacion> qCP = null;
            IQueryable<clsItemBandejaEntradaAsignacion> qTR = null;

            DGHP_Entities db = new DGHP_Entities();
            Guid userid = Functions.GetUserId();

            //perfiles usuario
            var u = db.aspnet_Users.Where(s => s.UserId == userid).FirstOrDefault();
            List<int> perfiles = u.SGI_PerfilesUsuarios.Select(s => s.id_perfil).ToList();

            var lista_tareas_perfil = (from erpt in db.ENG_Rel_Perfiles_Tareas
                                       where perfiles.Contains(erpt.id_perfil)
                                       select new { erpt.id_tarea }).ToList();
            List<int> tareasPerfil = new List<int>();

            lista_tareas_perfil.ForEach(tarea => tareasPerfil.Add(tarea.id_tarea));

            var qTT = (from bandeja in db.ENG_Config_BandejaAsignacion
                       join tt in db.SGI_Tramites_Tareas on bandeja.id_tarea equals tt.id_tarea
                       where
                       perfiles.Contains(bandeja.id_perfil_asignador.Value)
                           && tt.UsuarioAsignado_tramitetarea == null
                           && tt.FechaCierre_tramitetarea == null
                       select new
                       {
                           bandeja.id_perfil_asignado,
                           bandeja.id_perfil_asignador,
                           tt.id_tramitetarea,
                           tt.id_tarea,
                           tt.UsuarioAsignado_tramitetarea,
                           tt.FechaCierre_tramitetarea,
                           tt.FechaAsignacion_tramtietarea,
                           tt.FechaInicio_tramitetarea,
                           tt.CreateUser
                       }).Distinct();

            qTT = (from tt_bandeja in qTT
                   where tareasPerfil.Contains(tt_bandeja.id_tarea)
                   select tt_bandeja).Distinct();

            // Bandeja de datos Encomienda
            #region "Consulta Encomienda"
            qENC = (
                    from tt_bandeja in qTT
                    join tramite_tareas_HAB in db.SGI_Tramites_Tareas_HAB on tt_bandeja.id_tramitetarea equals tramite_tareas_HAB.id_tramitetarea
                    join tarea in db.ENG_Tareas on tt_bandeja.id_tarea equals tarea.id_tarea
                    join sol in db.SSIT_Solicitudes on tramite_tareas_HAB.id_solicitud equals sol.id_solicitud
                    join enc in db.Encomienda on sol.id_solicitud equals enc.Encomienda_SSIT_Solicitudes.FirstOrDefault().id_solicitud
                    where
                        enc.FechaEncomienda == (from en in db.Encomienda where en.Encomienda_SSIT_Solicitudes.FirstOrDefault().id_solicitud == sol.id_solicitud && en.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo select en.FechaEncomienda).Max()
                    select new clsItemBandejaEntradaAsignacion
                    {
                        cod_grupotramite = Constants.GruposDeTramite.HAB.ToString(),
                        tipoTramite = sol.TipoTramite.descripcion_tipotramite,
                        id_tramitetarea = tt_bandeja.id_tramitetarea,
                        FechaInicio_tramitetarea = tt_bandeja.FechaInicio_tramitetarea,
                        FechaAsignacion_tramtietarea = tt_bandeja.FechaAsignacion_tramtietarea,
                        id_solicitud = sol.id_solicitud,
                        direccion = "",
                        id_tarea = tarea.id_tarea,
                        nombre_tarea = tarea.nombre_tarea,
                        asignable_tarea = tarea.Asignable_tarea,
                        tomar_tarea = (tarea.id_tarea != 25 && tarea.id_tarea != 49) ? true : false,//Correcion de solicitudes
                        formulario_tarea = tarea.formulario_tarea,
                        Dias_Transcurridos = 0,
                        Dias_Acumulados = 0,
                        superficie_total = 0,
                        id_perfil_asignador = tt_bandeja.id_perfil_asignador.Value,
                        id_perfil_asignado = tt_bandeja.id_perfil_asignado.Value,
                        url_visorTramite = "~/GestionTramite/VisorTramite.aspx?id={0}",
                        url_tareaTramite = "~/GestionTramite/Tareas/{0}?id={1}"
                    }).Distinct();
            #endregion

            // Consulta de datos CPadron
            #region "Consulta CPadron"
            qCP = (from tt_bandeja in qTT
                   join tramite_tareas_CP in db.SGI_Tramites_Tareas_CPADRON on tt_bandeja.id_tramitetarea equals tramite_tareas_CP.id_tramitetarea
                   join tarea in db.ENG_Tareas on tt_bandeja.id_tarea equals tarea.id_tarea
                   join sol in db.CPadron_Solicitudes on tramite_tareas_CP.id_cpadron equals sol.id_cpadron
                   join eDatos in db.CPadron_DatosLocal on sol.id_cpadron equals eDatos.id_cpadron into zr
                   from ed in zr.DefaultIfEmpty()
                   select new clsItemBandejaEntradaAsignacion
                   {
                       cod_grupotramite = Constants.GruposDeTramite.CP.ToString(),
                       tipoTramite = sol.TipoTramite.descripcion_tipotramite,
                       id_tramitetarea = tt_bandeja.id_tramitetarea,
                       FechaInicio_tramitetarea = tt_bandeja.FechaInicio_tramitetarea,
                       FechaAsignacion_tramtietarea = tt_bandeja.FechaAsignacion_tramtietarea,
                       id_solicitud = sol.id_cpadron,
                       direccion = "",
                       id_tarea = tarea.id_tarea,
                       nombre_tarea = tarea.nombre_tarea,
                       asignable_tarea = tarea.Asignable_tarea,
                       tomar_tarea = tarea.id_tarea != 71 ? true : false,//Correcion de solicitudes
                       formulario_tarea = tarea.formulario_tarea,
                       Dias_Transcurridos = 0,
                       Dias_Acumulados = 0,
                       superficie_total = ed != null ? (ed.superficie_cubierta_dl.Value + ed.superficie_descubierta_dl.Value) : 0,
                       id_perfil_asignador = tt_bandeja.id_perfil_asignador.Value,
                       id_perfil_asignado = tt_bandeja.id_perfil_asignado.Value,
                       url_visorTramite = "~/VisorTramiteCP/{0}",
                       url_tareaTramite = "~/GestionTramite/Tareas/{0}?id={1}"
                   }).Distinct();
            #endregion

            // Bandeja de datos Transferencias
            #region "Consulta Transferencia"
            qTR = (from tt_bandeja in qTT
                   join tramite_tareas_TR in db.SGI_Tramites_Tareas_TRANSF on tt_bandeja.id_tramitetarea equals tramite_tareas_TR.id_tramitetarea
                   join tarea in db.ENG_Tareas on tt_bandeja.id_tarea equals tarea.id_tarea
                   join sol in db.Transf_Solicitudes on tramite_tareas_TR.id_solicitud equals sol.id_solicitud
                   join cpsol in db.CPadron_Solicitudes on sol.id_cpadron equals cpsol.id_cpadron
                   join eDatos in db.CPadron_DatosLocal on sol.id_cpadron equals eDatos.id_cpadron into zr
                   from ed in zr.DefaultIfEmpty()
                   select new clsItemBandejaEntradaAsignacion
                   {
                       cod_grupotramite = Constants.GruposDeTramite.TR.ToString(),
                       tipoTramite = sol.TipoTramite.descripcion_tipotramite,
                       id_tramitetarea = tt_bandeja.id_tramitetarea,
                       FechaInicio_tramitetarea = tt_bandeja.FechaInicio_tramitetarea,
                       FechaAsignacion_tramtietarea = tt_bandeja.FechaAsignacion_tramtietarea,
                       id_solicitud = sol.id_solicitud,
                       direccion = "",
                       id_tarea = tarea.id_tarea,
                       nombre_tarea = tarea.nombre_tarea,
                       asignable_tarea = tarea.Asignable_tarea,
                       tomar_tarea = (tarea.id_tarea != 60) ? true : false,//Correcion de solicitudes
                       formulario_tarea = tarea.formulario_tarea,
                       Dias_Transcurridos = 0,
                       Dias_Acumulados = 0,
                       superficie_total = ed != null ? (ed.superficie_cubierta_dl.Value + ed.superficie_descubierta_dl.Value) : 0,
                       id_perfil_asignador = tt_bandeja.id_perfil_asignador.Value,
                       id_perfil_asignado = tt_bandeja.id_perfil_asignado.Value,
                       url_visorTramite = "~/VisorTramiteTR/{0}",
                       url_tareaTramite = "~/GestionTramite/Tareas/{0}?id={1}"

                   }).Distinct();
            #endregion



            AddQueryFinalAsig(qENC, ref qFinal);
            AddQueryFinalAsig(qCP, ref qFinal);
            AddQueryFinalAsig(qTR, ref qFinal);


            //CargarTareas

            var qTareasCombo = (from res in qTT
                                join tareas in db.ENG_Tareas on res.id_tarea equals tareas.id_tarea
                                join circuito in db.ENG_Circuitos on tareas.id_circuito equals circuito.id_circuito
                                select new clsItemBandejaEntradaAsignacionTarea
                                {

                                    id_tarea = tareas.id_tarea,
                                    nombre_tarea = circuito.nombre_grupo + " - " + tareas.nombre_tarea
                                }).Distinct();


            ddlTareaAsignacion.DataSource = qTareasCombo.ToList();
            ddlTareaAsignacion.DataTextField = "nombre_tarea";
            ddlTareaAsignacion.DataValueField = "id_tarea";
            ddlTareaAsignacion.DataBind();
            ddlTareaAsignacion.Items.Insert(0, new ListItem("Todas", "0"));


            if (ViewState["tareaAsignacion"] != null)
            {
                string t = ViewState["tareaAsignacion"].ToString();
                ddlTareaAsignacion.Items.FindByValue(t).Selected = true;

                if (ddlTareaAsignacion.SelectedItem.Value != "0")
                {
                    int id_tar = int.Parse(ddlTareaAsignacion.SelectedValue);
                    qFinal = (from res in qFinal
                              where res.id_tarea == id_tar
                              select res);
                }

            }

            qFinal = qFinal.Distinct();

            if (sortByExpression != null)
            {
                if (sortByExpression.Contains("DESC"))
                {
                    if (sortByExpression.Contains("id_solicitud"))
                        qFinal = qFinal.OrderByDescending(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("Dias_Transcurridos"))
                        qFinal = qFinal.OrderByDescending(o => o.Dias_Transcurridos).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("Dias_Acumulados"))
                        qFinal = qFinal.OrderByDescending(o => o.Dias_Acumulados).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("direccion"))
                        qFinal = qFinal.OrderByDescending(o => o.direccion).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("FechaAsignacion_tarea"))
                        qFinal = qFinal.OrderByDescending(o => o.FechaAsignacion_tramtietarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("FechaInicio_tarea"))
                        qFinal = qFinal.OrderByDescending(o => o.FechaInicio_tramitetarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("nombre_tarea"))
                        qFinal = qFinal.OrderByDescending(o => o.nombre_tarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("superficie_total"))
                        qFinal = qFinal.OrderByDescending(o => o.superficie_total).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("tipoTramite"))
                        qFinal = qFinal.OrderByDescending(o => o.tipoTramite).Skip(startRowIndex).Take(maximumRows);
                    else
                        qFinal = qFinal.OrderByDescending(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);
                }
                else
                {
                    if (sortByExpression.Contains("id_solicitud"))
                        qFinal = qFinal.OrderBy(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("Dias_Transcurridos"))
                        qFinal = qFinal.OrderBy(o => o.Dias_Transcurridos).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("Dias_Acumulados"))
                        qFinal = qFinal.OrderBy(o => o.Dias_Acumulados).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("direccion"))
                        qFinal = qFinal.OrderBy(o => o.direccion).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("FechaAsignacion_tarea"))
                        qFinal = qFinal.OrderBy(o => o.FechaAsignacion_tramtietarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("FechaInicio_tarea"))
                        qFinal = qFinal.OrderBy(o => o.FechaInicio_tramitetarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("nombre_tarea"))
                        qFinal = qFinal.OrderBy(o => o.nombre_tarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("superficie_total"))
                        qFinal = qFinal.OrderBy(o => o.superficie_total).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("tipoTramite"))
                        qFinal = qFinal.OrderBy(o => o.tipoTramite).Skip(startRowIndex).Take(maximumRows);
                    else
                        qFinal = qFinal.OrderBy(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);
                }
            }
            else
                qFinal = qFinal.OrderBy(o => o.FechaInicio_tramitetarea).Skip(startRowIndex).Take(maximumRows);


            resultados = qFinal.ToList();

            var lista_aux = new List<clsItemBandejaEntradaAsignacion>();
            int nro_de_veces_que_se_repite = 0;

            foreach (var item in resultados)
            {
                if (!lista_aux.Any())
                    lista_aux.Add(item);

                else
                {
                    foreach (var item_aux in lista_aux)
                    {
                        if (item_aux.id_tramitetarea == item.id_tramitetarea)
                            nro_de_veces_que_se_repite++;

                    }

                    if (nro_de_veces_que_se_repite == 0)
                    {
                        lista_aux.Add(item);
                    }
                    else
                    {
                        nro_de_veces_que_se_repite = 0;
                    }
                }
            }

            resultados = lista_aux;
            totalRowCount = resultados.Count();

            #region "Domicilios y datos adicionales"
            if (resultados.Count > 0)
            {

                //------------------------------
                //Obtener las Direcciones del ENC
                //-------------------------------
                List<clsItemDireccion> lstDireccionesENC = new List<clsItemDireccion>();
                string[] arrSolicitudesENC = (from r in resultados
                                              where r.cod_grupotramite == Constants.GruposDeTramite.HAB.ToString()
                                              select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesENC.Length > 0)
                    lstDireccionesENC = Shared.GetDireccionesENC(arrSolicitudesENC);

                //------------------------------
                //Obtener las Direcciones del CP
                //-------------------------------
                List<clsItemDireccion> lstDireccionesCP = new List<clsItemDireccion>();
                string[] arrSolicitudesCP = (from r in resultados
                                             where r.cod_grupotramite == Constants.GruposDeTramite.CP.ToString()
                                             select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesCP.Length > 0)
                    lstDireccionesCP = Shared.GetDireccionesCP(arrSolicitudesCP);

                //------------------------------
                //Obtener las Direcciones del CP
                //-------------------------------
                List<clsItemDireccion> lstDireccionesTR = new List<clsItemDireccion>();
                string[] arrSolicitudesTR = (from r in resultados
                                             where r.cod_grupotramite == Constants.GruposDeTramite.TR.ToString()
                                             select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesTR.Length > 0)
                    lstDireccionesTR = Shared.GetDireccionesTR(arrSolicitudesTR);
                //------------------------------------------------------------------------
                //Rellena la clase a devolver con los datos que faltaban (Direccion, dias transcurrido)
                //------------------------------------------------------------------------
                foreach (var row in resultados)
                {
                    clsItemDireccion itemDireccion = null;
                    if (row.cod_grupotramite == Constants.GruposDeTramite.HAB.ToString())
                    {
                        itemDireccion = lstDireccionesENC.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);

                        var enc = db.Encomienda_SSIT_Solicitudes.Where(x => x.id_solicitud == row.id_solicitud &&
                                   x.Encomienda.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).
                                   OrderByDescending(x => x.Encomienda.id_encomienda).Select(x => x.Encomienda).FirstOrDefault();

                        if (enc != null)
                        {
                            var datosLocal = enc.Encomienda_DatosLocal.FirstOrDefault();
                            if (datosLocal != null)
                            {
                                row.superficie_total = datosLocal.superficie_cubierta_dl.Value + datosLocal.superficie_descubierta_dl.Value;
                            }

                            row.Rubros = (from er in enc.Encomienda_Rubros
                                          select new clsItemBandejaEntradaRubros
                                          {
                                              cod_rubro = er.cod_rubro,
                                              desc_rubro = er.desc_rubro
                                          }).Union
                                          (from er in enc.Encomienda_RubrosCN
                                           select new clsItemBandejaEntradaRubros
                                           {
                                               cod_rubro = er.CodigoRubro,
                                               desc_rubro = er.NombreRubro
                                           }).ToList();
                        }
                    }
                    else if (row.cod_grupotramite == Constants.GruposDeTramite.CP.ToString())
                    {
                        itemDireccion = lstDireccionesCP.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);

                        row.Rubros = (from cpr in db.CPadron_Rubros.Where(x => x.id_cpadron == row.id_solicitud)
                                      select new clsItemBandejaEntradaRubros
                                      {
                                          cod_rubro = cpr.cod_rubro,
                                          desc_rubro = cpr.desc_rubro
                                      }).ToList();
                    }
                    else if (row.cod_grupotramite == Constants.GruposDeTramite.TR.ToString())
                    {
                        itemDireccion = lstDireccionesTR.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);

                        row.Rubros = (from tr in db.Transf_Solicitudes
                                      join cpr in db.CPadron_Rubros on tr.id_cpadron equals cpr.id_cpadron
                                      where tr.id_solicitud == row.id_solicitud
                                      select new clsItemBandejaEntradaRubros
                                      {
                                          cod_rubro = cpr.cod_rubro,
                                          desc_rubro = cpr.desc_rubro
                                      }).Union
                                    (from enc in db.Encomienda_Transf_Solicitudes
                                     join encrub in db.Encomienda_RubrosCN on enc.id_encomienda equals encrub.id_encomienda
                                     where enc.id_solicitud == row.id_solicitud
                                     select new clsItemBandejaEntradaRubros
                                     {
                                         cod_rubro = encrub.CodigoRubro,
                                         desc_rubro = encrub.NombreRubro
                                     }
                                            ).ToList();
                    }

                    // Llenado para todos
                    if (itemDireccion != null)
                        row.direccion = (string.IsNullOrEmpty(itemDireccion.direccion) ? "" : itemDireccion.direccion);
                    row.url_visorTramite = string.Format(row.url_visorTramite, row.id_solicitud.ToString());
                    if (row.formulario_tarea != null)
                        row.url_tareaTramite = string.Format(row.url_tareaTramite, row.formulario_tarea.Substring(0, row.formulario_tarea.IndexOf(".")), row.id_tramitetarea.ToString());
                    else
                        row.url_tareaTramite = "";
                        row.Dias_Transcurridos = Shared.GetBusinessDays(row.FechaInicio_tramitetarea, DateTime.Now);



                    int firstTramiteTarea = 0;
                    int idTramiteTarea = 0;

                    if (row.cod_grupotramite == Constants.GruposDeTramite.HAB.ToString())
                    {
                        firstTramiteTarea = (from th in db.SGI_Tramites_Tareas_HAB
                                             where th.id_solicitud == row.id_solicitud
                                             select th.id_tramitetarea).Min();

                        idTramiteTarea = (from th in db.SGI_Tramites_Tareas_HAB
                                          where th.id_solicitud == row.id_solicitud
                                          & th.id_tramitetarea > firstTramiteTarea
                                          select th.id_tramitetarea).Min();
                    }
                    else if (row.cod_grupotramite == Constants.GruposDeTramite.CP.ToString())
                    {
                        firstTramiteTarea = (from th in db.SGI_Tramites_Tareas_CPADRON
                                             where th.id_cpadron == row.id_solicitud
                                             select th.id_tramitetarea).Min();

                        idTramiteTarea = (from th in db.SGI_Tramites_Tareas_CPADRON
                                          where th.id_cpadron == row.id_solicitud
                                          & th.id_tramitetarea > firstTramiteTarea
                                          select th.id_tramitetarea).Min();
                    }
                    else if (row.cod_grupotramite == Constants.GruposDeTramite.TR.ToString())
                    {
                        firstTramiteTarea = (from th in db.SGI_Tramites_Tareas_TRANSF
                                             where th.id_solicitud == row.id_solicitud
                                             select th.id_tramitetarea).Min();

                        idTramiteTarea = (from th in db.SGI_Tramites_Tareas_TRANSF
                                          where th.id_solicitud == row.id_solicitud
                                          & th.id_tramitetarea > firstTramiteTarea
                                          select th.id_tramitetarea).Min();
                    }

                    DateTime fechaInicio;

                    if (idTramiteTarea > 0)
                    {
                        fechaInicio = (from t in db.SGI_Tramites_Tareas
                                       where t.id_tramitetarea == idTramiteTarea
                                       select t.FechaInicio_tramitetarea).FirstOrDefault();

                        row.Dias_Acumulados = Shared.GetBusinessDays(fechaInicio, DateTime.Now);
                    }
                    else
                    {
                        row.Dias_Acumulados = 0;
                    }

                }
            }

            #endregion
            db.Dispose();


            return resultados;
        }

        private void AddQueryFinalAsig(IQueryable<clsItemBandejaEntradaAsignacion> query, ref IQueryable<clsItemBandejaEntradaAsignacion> qFinal)
        {
            if (query != null)
            {
                if (qFinal != null)
                    qFinal = qFinal.Union(query);
                else
                    qFinal = query;
            }
        }
        #endregion

        protected void ddlTipoTramite_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["tipoTramite"] = ddlTipoTramite.SelectedValue;
            ddlTipoTramite.Items.FindByValue((string)ViewState["tipoTramite"]).Selected = true;
            ddlTarea.Items[0].Selected = true;
            ViewState["tarea"] = null;
            grdBandeja.DataBind();
            //updBandejaPropia.Update();
        }

        /*
        protected void gridViewBandejaSade_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var itemActual = e.Row.DataItem as clsItemBandejaEntrada;
                int rowActual = e.Row.RowIndex;
                HiddenField hidIdTramitetarea = (HiddenField)e.Row.FindControl("hiddenIdTramiteTarea");
                int id_tramitetarea = int.Parse(hidIdTramitetarea.Value);
                int continuarSade = itemActual.continuar_sade;
                int completoSade = itemActual.sade_completo;              

                // Cantidad de tramites en la bandeja
                if (completoSade == 1)
                    lblCantSadeCompletos.Text = string.Format("{0} trámites para finalizar", cantCompletoSade++);
                else if (continuarSade == 1)
                    lblCantSadeContinuar.Text = string.Format("{0} trámites para continuar", cantContinuarSade++);



            }
        }
        */
    }
}