using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucTramitesRelacionados : System.Web.UI.UserControl
    {
        #region cargar inicial
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public void LoadData(int id_solicitud)
        {
            hddIdSolicitud.Value = id_solicitud.ToString();
            /*IniciarEntity();

            /*ObjectResult<ENG_TramitesRelacionados_Result> objResult = this.db.ENG_TramitesRelacionados(id_solicitud);
//            List<ENG_TramitesRelacionados_Result> lstResult = objResult.ToList();
            grdTramitesRelacionados.DataSource = objResult.ToList();
            grdTramitesRelacionados.DataBind();

            FinalizarEntity();*/
        }
        #endregion

        #region entity

        private DGHP_Entities db = null;

        private void IniciarEntity()
        {
            if (this.db == null)
            {
                this.db = new DGHP_Entities();
                this.db.Database.CommandTimeout = 120;
            }
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

        // El tipo devuelto puede ser modificado a IEnumerable, sin embargo, para ser compatible con paginación y ordenación 
        // , se deben agregar los siguientes parametros:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public List<ENG_TramitesRelacionados_Result> grdTramitesRelacionados_GetData(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {
            IniciarEntity();
            int id_solicitud = 0;
            if (hddIdSolicitud.Value.Count() > 0)
                id_solicitud = Int32.Parse(hddIdSolicitud.Value);
            ObjectParameter param_totalRowCount = new ObjectParameter("cant_reg", typeof(int));
            ObjectResult<ENG_TramitesRelacionados_Result> objResult = this.db.ENG_TramitesRelacionados(id_solicitud,startRowIndex,maximumRows,sortByExpression,param_totalRowCount);
            List<ENG_TramitesRelacionados_Result> tramites = objResult.ToList();
            totalRowCount = Convert.ToInt32(param_totalRowCount.Value);
            FinalizarEntity();
            return tramites;
        }

        #region "Paging gridview Bandeja"
        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;
            grdTramitesRelacionados.PageIndex = int.Parse(cmdPage.Text) - 1;


        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdTramitesRelacionados.PageIndex = grdTramitesRelacionados.PageIndex - 1;

        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdTramitesRelacionados.PageIndex = grdTramitesRelacionados.PageIndex + 1;
        }

        protected void grdTramitesRelacionados_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)grdTramitesRelacionados;
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
                        cmdPage.CssClass = "btn btn-default";

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
                            btn.CssClass = "btn btn-info";
                        }
                    }
                }

            }
        }
        #endregion
    }
}