using DocumentFormat.OpenXml.Spreadsheet;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucListaCalle : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

        public void LoadData(int id_calle)
        {
            IniciarEntity();

            if (id_calle == 0)
            {
                var all = (from calle in db.Calles

                           orderby calle.id_calle ascending
                           select new clsItemGrillaBuscarCalles()
                           {
                               Calle_Id = id_calle,
                               Calle_Cod = calle.Codigo_calle,
                               Calle_Nombre = calle.NombreOficial_calle,
                               Calle_AlturaIzquierdaInicio = (int)calle.AlturaIzquierdaInicio_calle,
                               Calle_AlturaIzquierdaFin = (int)calle.AlturaIzquierdaFin_calle,
                               Calle_AlturaDerechaInicio = (int)calle.AlturaDerechaInicio_calle,
                               Calle_AlturaDerechaFin = (int)calle.AlturaDerechaFin_calle,
                               Calle_Tipo = calle.TipoCalle_calle
                           }
                           ).ToList();

                grdBuscarCalles.DataSource = all;
                grdBuscarCalles.DataBind();
                updPnlCalles.Visible = true;
                updPnlCalles.Update();
            }
            else
            {


                var q = (from calle in db.Calles
                         where calle.id_calle == id_calle

                         orderby calle.id_calle ascending
                         select new clsItemGrillaBuscarCalles()
                         {
                             Calle_Id = id_calle,
                             Calle_Cod = calle.Codigo_calle,
                             Calle_Nombre = calle.NombreOficial_calle,
                             Calle_AlturaIzquierdaInicio = (int)calle.AlturaIzquierdaInicio_calle,
                             Calle_AlturaIzquierdaFin = (int)calle.AlturaIzquierdaFin_calle,
                             Calle_AlturaDerechaInicio = (int)calle.AlturaDerechaInicio_calle,
                             Calle_AlturaDerechaFin = (int)calle.AlturaDerechaFin_calle,
                             Calle_Tipo = calle.TipoCalle_calle
                         }
                        ).ToList();

                grdBuscarCalles.DataSource = q;
                grdBuscarCalles.DataBind();
                updPnlCalles.Visible = true;
                updPnlCalles.Update();
            }
        }

        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            LinkButton lnkEliminar = (LinkButton)sender;
            int id_calle = Convert.ToInt32(lnkEliminar.CommandName);

            using (var ctx = new DGHP_Entities())
            {
                using (var tran = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        using (var ftx = new DGHP_Entities())
                        {
                            Calles calle = (from c in ftx.Calles
                                            where c.id_calle == id_calle
                                            select c).FirstOrDefault();

                            var entity = new SGI.Model.Calles_Eliminadas()
                            {
                                id_calle = id_calle,
                                Codigo_calle = calle.Codigo_calle,
                                NombreOficial_calle = calle.NombreOficial_calle,
                                AlturaIzquierdaInicio_calle = calle.AlturaIzquierdaInicio_calle,
                                AlturaIzquierdaFin_calle = calle.AlturaIzquierdaFin_calle,
                                AlturaDerechaInicio_calle = calle.AlturaDerechaInicio_calle,
                                AlturaDerechaFin_calle = calle.AlturaDerechaFin_calle,
                                TipoCalle_calle = calle.TipoCalle_calle,
                                CreateDate = DateTime.Now,
                                CreateUser = Functions.GetUserId().ToString()
                            };

                            ftx.Calles_Eliminadas.Add(entity);
                            ftx.Calles.Remove(calle);
                            ftx.SaveChanges();
                        }
                        LoadData(id_calle);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                      throw ex;
                    }
                }
            }
        }

        protected void lnkEditar_Click(object sender, EventArgs e)
        {
            LinkButton lnkEditar = (LinkButton)sender;
            int id_calle = Convert.ToInt32(lnkEditar.CommandName);


            var codigoCalle = (sender as LinkButton).CommandArgument;
            var codigoCalleInt = Int32.Parse(codigoCalle);

            Response.Redirect("EditarCalle.aspx?id=" + id_calle);
            //revolear todos los datos hacia la vista de editar
        }

        public void vaciarGrilla()
        {
            updPnlCalles.Visible = false;
            updPnlCalles.Update();
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdBuscarCalles.PageIndex = e.NewPageIndex;
                LoadData(0);
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
                grdBuscarCalles.PageIndex = int.Parse(cmdPage.Text) - 1;
                LoadData(0);
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
                grdBuscarCalles.PageIndex = grdBuscarCalles.PageIndex - 1;
                LoadData(0);
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
                grdBuscarCalles.PageIndex = grdBuscarCalles.PageIndex + 1;
                LoadData(0);
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

                GridView grid = (GridView)grdBuscarCalles;
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

        protected void gridCalles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkEliminar = (LinkButton)e.Row.FindControl("lnkEliminar");
                lnkEliminar.Visible = true;

               var  codigoCalle = lnkEliminar.CommandArgument;
                var codigoCalleInt = Int32.Parse(codigoCalle);


                using (var ctx = new DGHP_Entities())
                {

                    var calle =
                                 (from c in ctx.AVH_Ubicaciones_Puertas
                                  where c.codigo_calle == codigoCalleInt
                                  select c.codigo_calle)

                                 .Union(from c in ctx.CPadron_Ubicaciones_Puertas
                                        where c.codigo_calle == codigoCalleInt
                                        select c.codigo_calle)

                                 .Union(from c in ctx.DGFYCO_Ubicaciones_Puertas
                                        where c.codigo_calle == codigoCalleInt
                                        select c.codigo_calle)

                                 .Union(from c in ctx.Encomienda_Ubicaciones_Puertas
                                        where c.codigo_calle == codigoCalleInt
                                        select c.codigo_calle)

                                 .Union(from c in ctx.EncomiendaExt_Ubicaciones_Puertas
                                        where c.codigo_calle == codigoCalleInt
                                        select c.codigo_calle)


                                 .Union(from c in ctx.SSIT_Solicitudes_Ubicaciones_Puertas
                                        where c.codigo_calle == codigoCalleInt
                                        select c.codigo_calle)

                                 .Union(from c in ctx.Transf_Ubicaciones_Puertas
                                        where c.codigo_calle == codigoCalleInt
                                        select c.codigo_calle)


                                 .Union(from c in ctx.Ubicaciones_Puertas
                                        where c.codigo_calle == codigoCalleInt
                                        select c.codigo_calle).Count();



                    if (calle > 0)
                        lnkEliminar.Visible = false;
                }

            }
        }

    }
}
