using SGI.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.ABM
{
    public partial class AbmRubrosCUR : BasePage
    {
        bool puedeEditarRubrosCur = false;
        bool puedeVerRubrosCur = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                //updResultados es el panel contenedor desde el q es llamado la función
                ScriptManager.RegisterStartupScript(updResultados, updResultados.GetType(), "loadPopOverRubro", "loadPopOverRubro();", true);
            }
            CargarPerfilUsuario();
        }

        private void CargarPerfilUsuario()
        {
            MembershipUser usuario = Membership.GetUser();

            if (usuario != null)
            {
                Guid userId = (Guid)usuario.ProviderUserKey;

                using (DGHP_Entities db = new DGHP_Entities())
                {
                    var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userId).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

                    foreach (var perfil in perfiles_usuario)
                    {
                        var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

                        if (menu_usuario.Contains("Editar Rubros CUR") || perfil.Contains("SGI_Administrador"))
                        {
                            puedeEditarRubrosCur = true;
                        }

                        if (menu_usuario.Contains("Visualizar Rubros CUR") || perfil.Contains("SGI_Administrador"))
                        {
                            puedeVerRubrosCur = true;
                        }
                    }
                }
            }
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        #region entity
        private DGHP_Entities db = null;

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

        protected void btnNuevoRubroCUR_Click(object sender, EventArgs e)
        {
            try
            {
                VisualizarRubroCUR.CargarDatosUnicaVez();
                updVisualizarRubro.Update();
                this.EjecutarScript(updpnlBuscar, "showDatos();");
            }
            catch (Exception)
            {

                throw new Exception("Se ha producido un error al crear un nuevo rubro.");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                grdResultados.Visible = true;
                grdResultados.DataBind();
                //Buscar();
                updResultados.Update();

                this.EjecutarScript(updpnlBuscar, "showResultado();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "showfrmError();");
            }
        }

        private void Buscar()
        {
            using (var db = new DGHP_Entities())
            {
                var q = (from rubrosCur in db.RubrosCN
                         join tipoCircuito in db.ENG_Grupos_Circuitos on rubrosCur.IdGrupoCircuito equals tipoCircuito.id_grupo_circuito into tc
                         from egc in tc.DefaultIfEmpty()
                         select new
                         {
                             Id_rubro = rubrosCur.IdRubro,
                             Cod_rubro = rubrosCur.Codigo,
                             Desc_rubro = rubrosCur.Nombre,
                             Cir_rubro = egc.nom_grupo_circuito,
                         });

                if (txtCodigoDescripcionoPalabraClave.Text.Trim().Length > 0)
                    q = q.Where(x => x.Desc_rubro.Contains(txtCodigoDescripcionoPalabraClave.Text.Trim()) ||
                    x.Cod_rubro.Contains(txtCodigoDescripcionoPalabraClave.Text.Trim()) ||
                    x.Cir_rubro.ToLower().Contains(txtCodigoDescripcionoPalabraClave.Text.Trim()));

                var resultados = q.OrderBy(x => x.Cod_rubro).ToList();
                grdResultados.DataSource = resultados;
                grdResultados.DataBind();

                pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
                lblCantidadRegistros.Text = resultados.Count.ToString();
            }
        }

        protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region PopupRubros
                LinkButton lnkSubRubros = (LinkButton)e.Row.FindControl("lnkSubRubros");
                DataList lstRubros = (DataList)e.Row.FindControl("lstSubRubros");
                Panel Rubros = (Panel)e.Row.FindControl("pnlSubRubros");
                int IdRubro = int.Parse(lnkSubRubros.CommandArgument);

                DGHP_Entities db = new DGHP_Entities();
                var subRubros = db.RubrosCN_Subrubros.Where(x => x.Id_rubroCN == IdRubro).ToList();

                if (subRubros != null)
                {
                    var elements = (from sub in subRubros
                                    select new
                                    {
                                        CodigoSubRubro = sub.Id_rubroCNsubrubro,
                                        DescripcionSubRubro = sub.Nombre,
                                    }).ToList();
                    lstRubros.DataSource = elements;
                    lstRubros.DataBind();
                }
                if (subRubros.Count == 0)
                {
                    LinkButton link = (LinkButton)e.Row.FindControl("lnkSubRubros");
                    link.Visible = false;
                }
                if (!this.puedeVerRubrosCur)
                {
                    HyperLink link = (HyperLink)e.Row.FindControl("btnVerRubro");
                    link.Visible = false;
                }

                if (!this.puedeEditarRubrosCur)
                {
                    HyperLink link = (HyperLink)e.Row.FindControl("btnEditarRubro");
                    link.Visible = false;
                }

                var historial = db.RubrosCN_Historial.Where(x => x.IdRubro == IdRubro).ToList();

                if (historial.Count == 0)
                {
                    HyperLink link = (HyperLink)e.Row.FindControl("btnHistorial");
                    link.Visible = false;
                }
                else
                {
                    HyperLink link = (HyperLink)e.Row.FindControl("btnHistorial");
                    link.Visible = true;
                }
                db.Dispose();
                #endregion
            }
        }

        public List<clsItemRubroCUR> GetRubros(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {
            totalRowCount = 0;

            List<clsItemRubroCUR> lstResult = FiltrarRubros(startRowIndex, maximumRows, sortByExpression, out totalRowCount);

            pnlCantidadRegistros.Visible = true;

            if (totalRowCount > 1)
            {
                lblCantidadRegistros.Text = string.Format("{0} Rubros", totalRowCount);
            }
            else if (totalRowCount == 1)
                lblCantidadRegistros.Text = string.Format("{0} Rubro", totalRowCount);
            else
            {
                pnlCantidadRegistros.Visible = false;
            }
            return lstResult;
        }

        private List<clsItemRubroCUR> FiltrarRubros(int startRowIndex, int maximumRows, string sortByExpression, out int totalRowCount)
        {
            DGHP_Entities db = new DGHP_Entities();

            List<clsItemRubroCUR> resultados = new List<clsItemRubroCUR>();

            using (db = new DGHP_Entities())
            {
                var q = (from rubrosCur in db.RubrosCN
                         join tipoCircuito in db.ENG_Grupos_Circuitos on rubrosCur.IdGrupoCircuito equals tipoCircuito.id_grupo_circuito into tc
                         from egc in tc.DefaultIfEmpty()
                         select new clsItemRubroCUR
                         {
                             Id_rubro = rubrosCur.IdRubro,
                             Cod_rubro = rubrosCur.Codigo,
                             Desc_rubro = rubrosCur.Nombre,
                             Cir_rubro = egc.nom_grupo_circuito
                         });

                if (txtCodigoDescripcionoPalabraClave.Text.Trim().Length > 0)
                    q = q.Where(x => x.Cod_rubro.Contains(txtCodigoDescripcionoPalabraClave.Text.Trim()) ||
                    (x.Desc_rubro.Contains(txtCodigoDescripcionoPalabraClave.Text.Trim())) ||
                    (x.Cir_rubro.ToLower().Contains(txtCodigoDescripcionoPalabraClave.Text.Trim())));

                q = q.OrderBy(r => r.Id_rubro).Skip(startRowIndex).Take(maximumRows);
                totalRowCount = q.Count();
                resultados = q.ToList();
            }
            return resultados;
        }


        #region paginado grilla

        protected void grdResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResultados.PageIndex = e.NewPageIndex;
            grdResultados.DataBind();
        }

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;
            grdResultados.PageIndex = int.Parse(cmdPage.Text) - 1;
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdResultados.PageIndex = grdResultados.PageIndex - 1;
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdResultados.PageIndex = grdResultados.PageIndex + 1;
        }

        protected void grdResultados_DataBound(object sender, EventArgs e)
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
                            btn.CssClass = "btn btn-inverse";
                        }
                    }
                }

            }
        }
        #endregion
    }
}