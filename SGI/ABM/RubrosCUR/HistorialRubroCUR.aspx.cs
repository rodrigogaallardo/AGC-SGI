using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.ABM.RubrosCUR
{
    public partial class HistoriaRubroCUR : System.Web.UI.Page
    {
        int IdRubro;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdRubro = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);

            }
        }

        public List<clsItemHistorialRubroCur> GetHistorial(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {

            totalRowCount = 0;

            List<clsItemHistorialRubroCur> lstResult = FiltrarRegistros(startRowIndex, maximumRows, sortByExpression, out totalRowCount);

            pnlCantRegistros.Visible = true;

            if (totalRowCount > 1)
            {
                lblCantRegistros.Text = string.Format("{0} Registros", totalRowCount);
            }
            else if (totalRowCount == 1)
                lblCantRegistros.Text = string.Format("{0} Registro", totalRowCount);
            else
            {
                pnlCantRegistros.Visible = false;
            }
            pnlResultadoBuscar.Visible = true;

            return lstResult;
        }

        private List<clsItemHistorialRubroCur> FiltrarRegistros(int startRowIndex, int maximumRows, string sortByExpression, out int totalRowCount)
        {
            DGHP_Entities db = new DGHP_Entities();

            List<clsItemHistorialRubroCur> resultados = new List<clsItemHistorialRubroCur>();

            using (db = new DGHP_Entities())
            {
                IQueryable<clsItemHistorialRubroCur> q = null;

                db.Database.CommandTimeout = 300;

                Guid userid = Functions.GetUserId();

                q = (   from h in db.RubrosCN_Historial
                        join tipoCircuito in db.ENG_Grupos_Circuitos on h.IdGrupoCircuito equals tipoCircuito.id_grupo_circuito into tc
                        from egc in tc.DefaultIfEmpty()
                        join tact in db.TipoActividad on h.IdTipoActividad equals tact.Id into ta
                        from tipoact in ta.DefaultIfEmpty()
                        join texp in db.TipoExpediente on h.IdTipoExpediente equals texp.id_tipoexpediente into te
                        from tipoexp in te.DefaultIfEmpty()
                        join est in db.RubrosEstacionamientos on h.IdEstacionamiento equals est.IdEstacionamiento into es
                        from rubest in es.DefaultIfEmpty()
                        join rubbic in db.RubrosBicicletas on h.IdBicicleta equals rubbic.IdBicicleta into bici
                        from rbici in bici.DefaultIfEmpty()
                        join rucyd in db.RubrosCargasyDescargas on h.IdCyD equals rucyd.IdCyD into rubcd
                        from rcd in rubcd.DefaultIfEmpty()
                        join aspu in db.aspnet_Users on h.LastUpdateUser equals aspu.UserId into aspu
                        from aspnu in aspu.DefaultIfEmpty()
                        select new clsItemHistorialRubroCur
                        {
                            IdRubro = h.IdRubro,
                            TipoOperacion = h.tipo_operacion,
                            FechaOperacion = h.fecha_operacion.ToString(),
                            Codigo = h.Codigo,
                            Nombre = h.Nombre,
                            Keywords = h.Keywords,
                            VigenciaDesde_rubro = h.VigenciaDesde_rubro.ToString(),
                            VigenciaHasta_rubro = h.VigenciaHasta_rubro.ToString(),
                            TipoActividad = tipoact.Nombre,
                            TipoExpediente = tipoexp.descripcion_tipoexpediente,
                            GrupoCircuito = egc.cod_grupo_circuito + " - " + egc.nom_grupo_circuito,
                            LibrarUso = h.LibrarUso,
                            ZonaMixtura1 = h.ZonaMixtura1,
                            ZonaMixtura2 = h.ZonaMixtura2,
                            ZonaMixtura3 = h.ZonaMixtura3,
                            ZonaMixtura4 = h.ZonaMixtura4,
                            Estacionamiento = rubest.Codigo,
                            Bicicleta = rbici.Codigo,
                            CyD = rcd.Codigo,
                            Observaciones = h.Observaciones,
                            CreateDate = h.CreateDate.ToString(),
                            LastUpdateDate = h.LastUpdateDate.ToString(),
                            LastUpdateUser = aspnu.UserName.ToString(),
                            Asistentes350 = h.Asistentes350 ?? null,
                            SinBanioPCD = h.SinBanioPCD ?? null,
                        }
                       );

                q = (from r in q
                        where r.IdRubro == this.IdRubro
                        select r);                

                totalRowCount = q.Count();
                q = q.OrderBy(r => r.LastUpdateDate).Skip(startRowIndex).Take(maximumRows);
                resultados = q.ToList();
             }
            return resultados;
        }

        #region paginado grilla

        protected void grdHistorial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdHistorial.PageIndex = e.NewPageIndex;
            grdHistorial.DataBind();
        }

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;
            grdHistorial.PageIndex = int.Parse(cmdPage.Text) - 1;
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdHistorial.PageIndex = grdHistorial.PageIndex - 1;
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdHistorial.PageIndex = grdHistorial.PageIndex + 1;
        }

        protected void grdHistorial_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)grdHistorial;
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
    }
}