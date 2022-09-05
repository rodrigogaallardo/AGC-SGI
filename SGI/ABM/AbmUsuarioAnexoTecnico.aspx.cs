using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;

namespace SGI.ABM
{
    public partial class ABMUsuarioAnexoTecnico : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                //ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                //ScriptManager.RegisterStartupScript(updPnlUsuSSIT, updPnlUsuSSIT.GetType(), "init_Js_updPnlUsuSSIT", "init_Js_updPnlUsuSSIT();", true);
            }

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");

        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        public void EjecutarScript(UpdatePanel upd, string scriptName)
        {
            ScriptManager.RegisterStartupScript(upd, upd.GetType(),
                "script", scriptName, true);

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Buscar();
                updResultados.Update();
                EjecutarScript(UpdatePanel1, "showResultado();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "showfrmError();");
            }
        }

        private void Buscar()
        {
            DGHP_Entities db = new DGHP_Entities();
            IQueryable<clsItemUsuarioAnexoTecnico> query = null;

            int nro_anexo = 0;
            int nro_sol = 0;

            int.TryParse(txtAnexoTecnico.Text, out nro_anexo);
            int.TryParse(txtSolicitud.Text, out nro_sol);

            query = (from sol in db.SSIT_Solicitudes
                     join encsol in db.Encomienda_SSIT_Solicitudes on sol.id_solicitud equals encsol.id_solicitud
                     join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                     join prof in db.Profesional on enc.id_profesional equals prof.Id
                     join usr in db.aspnet_Users on prof.UserId equals usr.UserId
                     join csj in db.ConsejoProfesional on prof.IdConsejo equals csj.Id
                     join grp in db.GrupoConsejos on csj.id_grupoconsejo equals grp.id_grupoconsejo
                     select new clsItemUsuarioAnexoTecnico
                     {
                         nro_anexo_tecnico = enc.id_encomienda,
                         nro_solicitud = sol.id_solicitud,
                         nombre = prof.Apellido + ", " + prof.Nombre,
                         user_name = usr.UserName,
                         consejo_profesional = grp.nombre_grupoconsejo
                     }).Union(
                    from sol in db.Transf_Solicitudes
                    join encsol in db.Encomienda_Transf_Solicitudes on sol.id_solicitud equals encsol.id_solicitud
                    join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                    join prof in db.Profesional on enc.id_profesional equals prof.Id
                    join usr in db.aspnet_Users on prof.UserId equals usr.UserId
                    join csj in db.ConsejoProfesional on prof.IdConsejo equals csj.Id
                    join grp in db.GrupoConsejos on csj.id_grupoconsejo equals grp.id_grupoconsejo
                    select new clsItemUsuarioAnexoTecnico
                    {
                        nro_anexo_tecnico = enc.id_encomienda,
                        nro_solicitud = sol.id_solicitud,
                        nombre = prof.Apellido + ", " + prof.Nombre,
                        user_name = usr.UserName,
                        consejo_profesional = grp.nombre_grupoconsejo
                    });

            if (nro_anexo > 0 || nro_sol > 0)
            {

                if (nro_anexo > 0)
                {
                    query = (from q in query
                             where q.nro_anexo_tecnico == nro_anexo
                             select q);
                }

                if (nro_sol > 0)
                {
                    query = (from q in query
                             where q.nro_solicitud == nro_sol
                             select q);
                }

            }

            grdResultados.DataSource = query.ToList();
            grdResultados.DataBind();

            db.Dispose();
        }

        private void LimpiarDatosBusqueda()
        {
            txtAnexoTecnico.Text = "";
            txtSolicitud.Text = "";
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarDatosBusqueda();
            updpnlBuscar.Update();
            EjecutarScript(UpdatePanel1, "hideResultado();");
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {

            try
            {
                grdResultados.PageIndex = grdResultados.PageIndex - 1;
                Buscar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdResultados.PageIndex = e.NewPageIndex;
                //IniciarEntity();
                Buscar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }

        }

        protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton btnEditar = (LinkButton)e.Row.FindControl("btnEditar");
                //btnEditar.Visible = editar;

            }


        }

        protected void grd_DataBound(object sender, EventArgs e)
        {
            try
            {

                GridView grid = (GridView)grdResultados;
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
                                btn.CssClass = "btn btn-primary";
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

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;

            try
            {
                grdResultados.PageIndex = int.Parse(cmdPage.Text) - 1;
                //IniciarEntity();
                Buscar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {

            try
            {
                grdResultados.PageIndex = grdResultados.PageIndex + 1;

                Buscar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }
    }
}