using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SGI.GestionTramite
{
    public partial class Parametros_Bandeja : BasePage
    {
        DGHP_Entities db = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (!IsPostBack)
            {

                Cargarddl();
                CargarGridParametroRubro(0, 0, "");
                CargarTxtSuperficies();
            }

            if (sm.IsInAsyncPostBack)
            {

                ScriptManager.RegisterStartupScript(updPnlRubro, updPnlRubro.GetType(),
                    "inicializar_controles", "inicializar_controles();", true);
            }

        }
        private void CargarTxtSuperficies()
        {
            db = new DGHP_Entities();
            var q = (
                        from psu in db.Parametros_Bandeja_Superficie
                        select new clsItemTxtSuperficie
                        {
                            id_circuito = psu.id_circuito,
                            Superficie = psu.Superficie,
                            RevisaMenor = psu.RevisaMenor,
                            RevisaMayor = psu.RevisaMayor
                        }
                            ).ToList();

            for (int i = 0; i < q.Count(); i++)
            {
                switch (q.ElementAt(i).id_circuito)
                {  
                    case 11:
                        txtSSP.Text = q.ElementAt(i).Superficie.ToString();
                        ddlMayorSSP.SelectedValue = q.ElementAt(i).RevisaMayor.Trim();
                        ddlMenorSSP.SelectedValue = q.ElementAt(i).RevisaMenor.Trim();
                        break;
                    case 12:
                        txtSCP.Text = q.ElementAt(i).Superficie.ToString();
                        ddlMayorSCP.SelectedValue = q.ElementAt(i).RevisaMayor.Trim();
                        ddlMenorSCP.SelectedValue = q.ElementAt(i).RevisaMenor.Trim();
                        break;
                    case 13:
                        txtESPE.Text = q.ElementAt(i).Superficie.ToString();
                        ddlMayorESPE.SelectedValue = q.ElementAt(i).RevisaMayor.Trim();
                        ddlMenorESPE.SelectedValue = q.ElementAt(i).RevisaMenor.Trim();
                        break;
                    case 14:
                        txtESPA.Text = q.ElementAt(i).Superficie.ToString();
                        ddlMayorESPA.SelectedValue = q.ElementAt(i).RevisaMayor.Trim();
                        ddlMenorESPA.SelectedValue = q.ElementAt(i).RevisaMenor.Trim();
                        break;
                }
            }
            updPnlSuperficie.Update();

        }
        private void Cargarddl()
        {
            try
            {

                db = new DGHP_Entities();
                var q = (
                            from rbo in db.Rubros
                            where rbo.EsAnterior_Rubro == false
                            select new clsItemddlRubro()
                            {
                                id_rubro = rbo.id_rubro,
                                cod_rubro = rbo.cod_rubro + " - " + rbo.nom_rubro
                            }
                            ).ToList();
                ddlRubro.DataSource = q;
                ddlRubro.DataTextField = "cod_rubro";
                ddlRubro.DataValueField = "id_rubro";
                ddlRubro.DataBind();


                q.Add(new clsItemddlRubro() { id_rubro = 0, cod_rubro = "Todos" });
                ddlFiltroRubro.DataSource = q;
                ddlFiltroRubro.DataTextField = "cod_rubro";
                ddlFiltroRubro.DataValueField = "id_rubro";
                ddlFiltroRubro.SelectedIndex = q.Count() - 1;
                ddlFiltroRubro.DataBind();

                var k = (
                            from Tte in db.Tipo_Documentacion_Req
                            select new clsItemddlTTramite()
                            {
                                Id = Tte.Id,
                                TipoTramite = Tte.TipoTramite
                            }).ToList();

                k.Add(new clsItemddlTTramite() { Id = 0, TipoTramite = "Todos" });

                ddlFiltroTipoTramiteRubro.DataSource = k;
                ddlFiltroTipoTramiteRubro.DataTextField = "TipoTramite";
                ddlFiltroTipoTramiteRubro.DataValueField = "Id";
                ddlFiltroTipoTramiteRubro.SelectedIndex = k.Count() - 1;
                ddlFiltroTipoTramiteRubro.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updmpeInfo, "showfrmError();");
                return;
            }



        }

        protected void lnkEliminarRevisa_Command(object sender, CommandEventArgs e)
        {
            LinkButton lnkEliminarParam = (LinkButton)sender;
            int idParam = int.Parse(lnkEliminarParam.CommandArgument);
            try
            {
                db = new DGHP_Entities();
                db.Eliminar_Parametros_Bandeja_Rubro(idParam);
                CargarFiltro();
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updmpeInfo, "showfrmError();");
                return;
            }

        }
        private void CargarGridParametroRubro(int v_TipoTramite, int v_Rubro, string v_Revisa)
        {
            try 
            {
                db = new DGHP_Entities();
                var q = (
                    from prm in db.Parametros_Bandeja_Rubro
                    join rbo in db.Rubros on prm.id_rubro equals rbo.id_rubro
                    join tdr in db.Tipo_Documentacion_Req on rbo.id_tipodocreq equals tdr.Id
                    where rbo.EsAnterior_Rubro == false
                    select new clsItemGrillaParametrosRubro()
                    {
                        id_param = prm.id_param,
                        Descripcion = tdr.TipoTramite,
                        cod_rubro = rbo.cod_rubro + " - " + rbo.nom_rubro,
                        Revisa = prm.Revisa,
                        Id = tdr.Id,
                        id_rubro = rbo.id_rubro
                    }).ToList();

                if (v_TipoTramite > 0)
                {
                    q = q.Where(x => x.Id == v_TipoTramite).ToList();
                }

                if (v_Rubro > 0)
                {
                    q = q.Where(x => x.id_rubro == v_Rubro).ToList();
                }

                if (v_Revisa != "")
                {
                    q = q.Where(x => x.Revisa.Trim() == v_Revisa).ToList();
                }

                grdRubrosParametros.DataSource = q;
                grdRubrosParametros.DataBind();
                updPnlGrdRubro.Update();
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updmpeInfo, "showfrmError();");
                return;
            }
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdRubrosParametros.PageIndex = grdRubrosParametros.PageIndex + 1;
            CargarGridParametroRubro(0, 0,"");
        }
        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdRubrosParametros.PageIndex = grdRubrosParametros.PageIndex - 1;
            CargarGridParametroRubro(0, 0, "");
        }
        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;
            grdRubrosParametros.PageIndex = int.Parse(cmdPage.Text) - 1;
            CargarGridParametroRubro(0, 0, "");
        }



        protected void grdRubrosParametros_RowDataBound(object sender, GridViewRowEventArgs e)
        { }

        protected void grdRubrosParametros_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)grdRubrosParametros;
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
                    // Mostrar 9 botones hacia la izquierda y 9 hacia la derecha o bien los que sea posible en caso de no llegar a 9

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

                UpdatePanel updPnlPager = (UpdatePanel)fila.Cells[0].FindControl("updPnlPager");
                if (updPnlPager != null)
                    updPnlPager.Update();
            }
        }

        protected void btnGuardarSuperficie_OnClick(object sender, EventArgs e)
        {
            try
            { 
                db = new DGHP_Entities();
                db.Actualizacion_Parametros_Bandeja_Superficie(11, Convert.ToInt32(txtSSP.Text), ddlMenorSSP.SelectedValue, ddlMayorSSP.SelectedValue);
                db.Actualizacion_Parametros_Bandeja_Superficie(12, Convert.ToInt32(txtSCP.Text), ddlMenorSCP.SelectedValue, ddlMayorSCP.SelectedValue);
                db.Actualizacion_Parametros_Bandeja_Superficie(13, Convert.ToInt32(txtESPE.Text), ddlMenorESPE.SelectedValue, ddlMayorESPE.SelectedValue);
                db.Actualizacion_Parametros_Bandeja_Superficie(14, Convert.ToInt32(txtESPA.Text), ddlMenorESPA.SelectedValue, ddlMayorESPA.SelectedValue);

                lblMsj.Text = "Cambios Guardados.";
                this.EjecutarScript(updMsj, "showfrmMsj();");
                return;

            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updmpeInfo, "showfrmError();");
                return;
            }
        }
        protected void btnGuardarRubro_OnClick(object sender, EventArgs e)
        {
            try
            { 
                db = new DGHP_Entities();
                db.Actualizacion_Parametros_Bandeja_Rubro(Convert.ToInt32(ddlRubro.SelectedValue), ddlRevisaRubro.SelectedValue);
                CargarFiltro();
                updPnlRubro.Update();
                updPnlFiltroRubro.Update();
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updmpeInfo, "showfrmError();");
                return;
            }
        }

        protected void ddlRubro_SelectedIndexChanged(object sender, EventArgs e) {
            updPnlRubro.Update();
        }
        protected void ddlRevisaRubro_SelectedIndexChanged(object sender, EventArgs e){}

        protected void ddlFiltroTipoTramiteRubro_SelectedIndexChanged(object sender, EventArgs e) 
        {
            CargarFiltro();
            updPnlRubro.Update();
            updPnlFiltroRubro.Update();
        }
        protected void ddlFiltroRubro_SelectedIndexChanged(object sender, EventArgs e) 
        {
            CargarFiltro();
            updPnlRubro.Update();
            updPnlFiltroRubro.Update();
        }
        protected void ddlFiltroRevisa_SelectedIndexChanged(object sender, EventArgs e) 
        {
            CargarFiltro();
            updPnlRubro.Update();
            updPnlFiltroRubro.Update();
        }

        protected void CargarFiltro()
        {

            int Tramite;
            int Rubro;
            string Revisa;

            Tramite = Convert.ToInt32(ddlFiltroTipoTramiteRubro.SelectedValue);
            Rubro = Convert.ToInt32(ddlFiltroRubro.SelectedValue);
            Revisa = ddlFiltroRevisa.SelectedValue == "" ? "" : ddlFiltroRevisa.SelectedValue;

            CargarGridParametroRubro(Tramite, Rubro, Revisa);
        }
    }
}