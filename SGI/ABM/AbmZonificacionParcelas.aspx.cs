using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SGI.Model;
using System.Transactions;
using System.Data;
using System.Data.Entity.Core.Objects;

namespace SGI.ABM
{
    public partial class AbmZonificacionParcelas : BasePage{

        private bool formulario_cargado
        {
            get
            {
                bool ret = false;
                bool.TryParse(hid_formulario_cargado.Value, out ret);
                return ret;
            }
            set
            {
                hid_formulario_cargado.Value = value.ToString();
            }

        }

        DGHP_Entities db = null;

        #region load de pagina
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                

            }


            if (!IsPostBack)
            {
                db = new DGHP_Entities();
                Cargar_Calle("");
                CargarZonas();
            }
        }

    
        #endregion

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        

        private void LimpiarDatosBusqueda()
        {
            txtSeccion.Text = "";
            txtManzana.Text = "";
        
        }

        private void LimpiarDatos()
        {
         
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.formulario_cargado = true;
                grdResultados.PageIndex = 0;
                grdResultados.DataBind();
                EjecutarScript(updpnlBuscar, "showResultado();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "showfrmError();");
            }
        }

        public List<ItemZonificacionParcelas> GetZonificacionParcelas(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {
           
            List<ItemZonificacionParcelas> lstItems = new List<ItemZonificacionParcelas>();
            totalRowCount = 0;
            pnlCantidadRegistros.Visible = true;
            try
            {
                if (this.formulario_cargado)
                {
                    lstItems = filtrarZonificacionParcelas(startRowIndex, maximumRows, sortByExpression, out totalRowCount);
                    int? TotalRegistros = totalRowCount;
                    totalRowCount = (TotalRegistros.HasValue ? (int)TotalRegistros : 0);

                    lblCantidadRegistros.Visible = true;
                    lblCantidadRegistros.Text = "";
                }

            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");

            }
            
            // Cantidad de tramites en la bandeja
            if (totalRowCount > 1)
                lblCantidadRegistros.Text = string.Format("{0} trámites", totalRowCount);
            else if (totalRowCount == 1)
                lblCantidadRegistros.Text = string.Format("{0} trámite", totalRowCount);
            else
            {
                pnlCantidadRegistros.Visible = false;
            }

            updResultados.Update();
            return lstItems;

        }

        private List<ItemZonificacionParcelas> filtrarZonificacionParcelas(int startRowIndex, int maximumRows, string sortByExpression, out int totalRowCount)
        {
            List<ItemZonificacionParcelas> resultados = new List<ItemZonificacionParcelas>();
            db = new DGHP_Entities();
            var q = (from ubicaciones in db.Ubicaciones
                     join ubicaciones_puertas in db.Ubicaciones_Puertas on ubicaciones.id_ubicacion equals ubicaciones_puertas.id_ubicacion
                     join zonas_planeamiento in db.Zonas_Planeamiento on ubicaciones.id_zonaplaneamiento equals zonas_planeamiento.id_zonaplaneamiento
                     where ubicaciones.baja_logica == false
                     select new ItemZonificacionParcelasAux
                     {
                         id_ubicacion = ubicaciones.id_ubicacion,
                         partidaMatriz = ubicaciones.NroPartidaMatriz,
                         seccion = ubicaciones.Seccion,
                         manzana = ubicaciones.Manzana,
                         parcela = ubicaciones.Parcela,
                         zona1 = zonas_planeamiento.CodZonaPla,
                         zona2 = "",
                         zona3 = "",
                         direccion = "",                         
                         vigenciaHasta = ubicaciones.VigenciaHasta,
                         codigoCalle = ubicaciones_puertas.codigo_calle,
                         nroPuerta = ubicaciones_puertas.NroPuerta_ubic
                     }
                     );

            q = q.Where(x => x.vigenciaHasta.Equals(null));

            if (txtCalleList.SelectedItem.Text.Trim().Length > 0 && !txtCalleList.SelectedItem.Text.Trim().Equals("Todas"))
            {
                int vCalle = 0;
                int.TryParse(txtCalleList.SelectedItem.Value, out vCalle);
                q = q.Where(x => x.codigoCalle == vCalle);
            }

            if (txtSeccion.Text.Trim().Length > 0)
            {
                int vSeccion = 0;
                int.TryParse(txtSeccion.Text, out vSeccion);
                q = q.Where(x => x.seccion == vSeccion);
            }

            if (txtManzana.Text.Trim().Length > 0)
                q = q.Where(x => x.manzana.Equals(txtManzana.Text.Trim()));

            if (radioNumeracionPar.Checked)
                q = q.Where(x => x.nroPuerta % 2 == 0);

            if (radioNumeracionImpar.Checked)
                q = q.Where(x => x.nroPuerta % 2 != 0);

            if (txtPuertaHasta.Text.Length > 0)
                if (Convert.ToInt32(txtPuertaHasta.Text) > 0)
                {
                    int desde = Convert.ToInt32(txtPuertaDesde.Text);
                    int hasta = Convert.ToInt32(txtPuertaHasta.Text);

                    q = q.Where(x => x.nroPuerta >= desde);
                    q = q.Where(x => x.nroPuerta <= hasta);
                }

           var qa = (from qs  in q
                  select new ItemZonificacionParcelas
                     {
                         id_ubicacion = qs.id_ubicacion,
                         partidaMatriz = qs.partidaMatriz,
                         seccion = qs.seccion,
                         manzana = qs.manzana,
                         parcela = qs.parcela,
                         zona1 = qs.zona1,
                         zona2 = "",
                         zona3 = "",
                         direccion = "",
                     }
                     ).Distinct();

            totalRowCount = qa.Count();
            resultados = qa.OrderBy(o => o.id_ubicacion).Skip(startRowIndex).Take(maximumRows).ToList();

            if (resultados.Count > 0)
            {

                string[] arrUbicaciones = resultados.Select(s => s.id_ubicacion.ToString()).ToArray();
                List<ItemDireccion> lstDirecciones = Shared.getDirecciones(arrUbicaciones);
                List<ItemZona> lstZonas2 = Shared.getZonas(arrUbicaciones,"Z2");
                List<ItemZona> lstZonas3 = Shared.getZonas(arrUbicaciones, "Z3");
                //------------------------------------------------------------------------
                //Rellena la clase a devolver con los datos que faltaban (Direccion,tarea)
                //------------------------------------------------------------------------
                foreach (var row in resultados)
                {
                    var itemDireccion = lstDirecciones.FirstOrDefault(x => x.id_ubicacion == row.id_ubicacion);
                    if (itemDireccion != null)
                        row.direccion = (string.IsNullOrEmpty(itemDireccion.direccion) ? "" : itemDireccion.direccion);

                    var itemsZona2 = lstZonas2.FirstOrDefault(x => x.id_ubicacion == row.id_ubicacion);
                    if (itemsZona2!=null)
                         row.zona2 = itemsZona2.CodZonaPla;


                    var itemsZona3 = lstZonas3.FirstOrDefault(x => x.id_ubicacion == row.id_ubicacion);
                    if (itemsZona3 != null)
                        row.zona3 = itemsZona3.CodZonaPla;
                }

            }

            pnlCantidadRegistros.Visible = (resultados.Count > 0);
            lblCantidadRegistros.Text = resultados.Count.ToString();
            db.Dispose();

            return resultados;
        }

         protected void cmdBusquedaCalle_Click(object sender, ImageClickEventArgs e)
         {

             if (txtCalleList.Text.Trim().Length > 0)
                 Cargar_Calle(txtCalleList.Text.Trim());

         }

         private void Cargar_Calle(string strCalle)
         {
             db = new DGHP_Entities();
             ListItem cmbitem;

            

             txtCalleList.Items.Clear();

             ObjectResult<Cons_Calle_Result> dsCalles = db.Cons_Calle(txtBusquedaCalle.Text.Trim());

             cmbitem = new ListItem();
             cmbitem.Text = "Todas";
             cmbitem.Value = "0";
             txtCalleList.Items.Insert(0,cmbitem);

             foreach (Cons_Calle_Result drCalle in dsCalles)
             {
                cmbitem = new ListItem();
                cmbitem.Text = drCalle.Nombre + " - (" + drCalle.AlturaInicio.ToString() + " - " + drCalle.AlturaFin.ToString() + ")";
                cmbitem.Value = drCalle.Codigo.ToString();
                txtCalleList.Items.Add(cmbitem);
             }
         }



         protected void imgLimpiarCalle_Click(object sender, ImageClickEventArgs e)
         {
             txtCalleList.Items.Clear();
             txtBusquedaCalle.Text = "";

         }



         #region "Paging gridview Resultados"
         protected void grdResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
         {
             grdResultados.PageIndex = e.NewPageIndex;

         }


         protected void cmdPage(object sender, EventArgs e)
         {
             Button cmdPage = (Button)sender;
             grdResultados.PageIndex = int.Parse(cmdPage.Text) - 1;
             grdResultados.DataBind();

         }
         protected void cmdAnterior_Click(object sender, EventArgs e)
         {
             grdResultados.PageIndex = grdResultados.PageIndex - 1;
             grdResultados.DataBind();
         }
         protected void cmdSiguiente_Click(object sender, EventArgs e)
         {
             grdResultados.PageIndex = grdResultados.PageIndex + 1;
             grdResultados.DataBind();
         }


         protected void grdResultados_DataBound(object sender, EventArgs e)
         {
             GridView grid = grdResultados;
             GridViewRow fila = (GridViewRow)grid.BottomPagerRow;

             if (fila != null)
             {
                 Button btnAnterior = (Button)fila.Cells[0].FindControl("cmdAnterior");
                 Button btnSiguiente = (Button)fila.Cells[0].FindControl("cmdSiguiente");

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
                     Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                     btn.Visible = false;
                 }


                 if (grid.PageIndex == 0 || grid.PageCount <= 10)
                 {
                     // Mostrar 10 botones o el máximo de páginas

                     for (int i = 1; i <= 10; i++)
                     {
                         if (i <= grid.PageCount)
                         {
                             Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + i.ToString());
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

                     Button btnPage10 = (Button)fila.Cells[0].FindControl("cmdPage10");
                     btnPage10.Visible = true;
                     btnPage10.Text = Convert.ToString(grid.PageIndex + 1);

                     // Ubica los 9 botones hacia la izquierda
                     for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                     {
                         CantBucles++;
                         if (i >= 0)
                         {
                             Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 - CantBucles));
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
                             Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 + CantBucles));
                             btn.Visible = true;
                             btn.Text = Convert.ToString(i + 1);
                         }
                     }



                 }
                 Button cmdPage;
                 string btnPage = "";
                 for (int i = 1; i <= 19; i++)
                 {
                     btnPage = "cmdPage" + i.ToString();
                     cmdPage = (Button)fila.Cells[0].FindControl(btnPage);
                     if (cmdPage != null && cmdPage.Visible)
                         cmdPage.CssClass = "btn btn-default";

                 }



                 // busca el boton por el texto para marcarlo como seleccionado
                 string btnText = Convert.ToString(grid.PageIndex + 1);
                 foreach (Control ctl in fila.Cells[0].FindControl("pnlpager").Controls)
                 {
                     if (ctl is Button)
                     {
                         Button btn = (Button)ctl;
                         if (btn.Text.Equals(btnText))
                         {
                             btn.CssClass = "btn btn-info";
                         }
                     }
                 }

             }
         }
         #endregion



         private void CargarZonas()
         {
             ListItem itm = new ListItem("(Seleccione la Zona)", "");

             var datos = (from zonas in db.Zonas_Planeamiento
                         select new
                         {
                             id_zonaplaneamiento = zonas.id_zonaplaneamiento,
                             CodZonaPla = zonas.CodZonaPla,
                             DescripcionZonaPla = zonas.DescripcionZonaPla,
                         }
                    ).OrderBy(x => x.CodZonaPla).ToList();

             foreach (var dato in datos)
             {
                 ListItem cmbitem = new ListItem();
                 cmbitem.Text = dato.CodZonaPla + " - " + dato.DescripcionZonaPla;
                 cmbitem.Value = Convert.ToString(dato.id_zonaplaneamiento);
                 ddlZona.Items.Add(cmbitem);
                 ddlZona2.Items.Add(cmbitem);
                 ddlZona3.Items.Add(cmbitem);
                 
            }

             ddlZona.Items.Insert(0, itm);
             ddlZona2.Items.Insert(0, itm);
             ddlZona3.Items.Insert(0, itm);
         }


         protected void btnActualizar_Click(object sender, EventArgs e)
         {
             db = new DGHP_Entities();
             int vCodCalle = 0;
             int vSeccion = 0;
             string vManzana = "";
             int vPuertaDesde = 0;
             int vPuertaHasta = 0;
             int id_zonaplaneamiento = 0;
             int vTipoNumeracion = 0;    // (0 - Ambas / 1 - Pares / 2 - Impares )
             int CantRegistrosActualizados = 0;
             Guid userid;

             try
             {
                 userid = (Guid)Membership.GetUser().ProviderUserKey;
                 if (!string.IsNullOrEmpty(txtCalleList.SelectedValue))
                     vCodCalle = Convert.ToInt32(txtCalleList.SelectedValue);

                 if (txtSeccion.Text.Trim().Length > 0)
                     vSeccion = Convert.ToInt32(txtSeccion.Text.Trim());

                 if (txtManzana.Text.Trim().Length > 0)
                     vManzana = txtManzana.Text.Trim();

                 if (txtPuertaDesde.Text.Trim().Length > 0)
                     vPuertaDesde = Convert.ToInt32(txtPuertaDesde.Text.Trim());

                 if (txtPuertaHasta.Text.Trim().Length > 0)
                     vPuertaHasta = Convert.ToInt32(txtPuertaHasta.Text.Trim());

                 id_zonaplaneamiento = Convert.ToInt32(ddlZona.SelectedValue);

                 if (radioNumeracionPar.Checked)
                     vTipoNumeracion = 1;
                 if (radioNumeracionImpar.Checked)
                     vTipoNumeracion = 2;

                 using (TransactionScope Tran = new TransactionScope())
                 {
                     try
                     {
                         CantRegistrosActualizados = db.Ubicaciones_ActualizarZonificacion(id_zonaplaneamiento, vCodCalle, vSeccion, vManzana, vPuertaDesde, vPuertaHasta, vTipoNumeracion, userid);
                         Tran.Complete();


                     }
                     catch (Exception ex)
                     {
                         Tran.Dispose();
                         throw ex;
                     }
                 }

                 if (CantRegistrosActualizados > 0)
                 {
                     lblMensaje.Text = string.Format("Se actualizaron {0} registro/s.", lblCantidadRegistros.Text);
                     this.EjecutarScript(updResultados, "showfrmMensaje();");

                     btnBuscar_Click(btnBuscar, new EventArgs());
                 }

             }
             catch (Exception ex)
             {
                 LogError.Write(ex);
                 lblMensaje.Text = Functions.GetErrorMessage(ex);
                 this.EjecutarScript(updResultados, "showfrmMensaje();");
             }

         }
         protected void btnActualizar2_Click(object sender, EventArgs e)
         {
             ActualizarZonas2y3("Z2");

         }
         protected void btnActualizar3_Click(object sender, EventArgs e)
         {
             ActualizarZonas2y3("Z3");
         }

         private void ActualizarZonas2y3(string tipo_ubicacion)
         {
             db = new DGHP_Entities();
             int vCodCalle = 0;
             int vSeccion = 0;
             string vManzana = "";
             int vPuertaDesde = 0;
             int vPuertaHasta = 0;
             int id_zonaplaneamiento = 0;
             int vTipoNumeracion = 0;    // (0 - Ambas / 1 - Pares / 2 - Impares )
             int CantRegistrosActualizados = 0;
             Guid userid;

             try
             {
                 userid = (Guid)Membership.GetUser().ProviderUserKey;
                 if (!string.IsNullOrEmpty(txtCalleList.SelectedValue))
                     vCodCalle = Convert.ToInt32(txtCalleList.SelectedValue);

                 if (txtSeccion.Text.Trim().Length > 0)
                     vSeccion = Convert.ToInt32(txtSeccion.Text.Trim());

                 if (txtManzana.Text.Trim().Length > 0)
                     vManzana = txtManzana.Text.Trim();

                 if (txtPuertaDesde.Text.Trim().Length > 0)
                     vPuertaDesde = Convert.ToInt32(txtPuertaDesde.Text.Trim());

                 if (txtPuertaHasta.Text.Trim().Length > 0)
                     vPuertaHasta = Convert.ToInt32(txtPuertaHasta.Text.Trim());

                 if (tipo_ubicacion == "Z2")
                     id_zonaplaneamiento = Convert.ToInt32(ddlZona2.SelectedValue);
                 else if (tipo_ubicacion == "Z3")
                     id_zonaplaneamiento = Convert.ToInt32(ddlZona3.SelectedValue);

                 if (radioNumeracionPar.Checked)
                     vTipoNumeracion = 1;
                 if (radioNumeracionImpar.Checked)
                     vTipoNumeracion = 2;
                 using (TransactionScope Tran = new TransactionScope())
                 {
                     try
                     {
                         CantRegistrosActualizados = db.Ubicaciones_ActualizarZonificacion_ZonasComplementarias(id_zonaplaneamiento,vCodCalle,vSeccion,vManzana,vPuertaDesde,vPuertaHasta,vTipoNumeracion,tipo_ubicacion,userid);
                         Tran.Complete();
                     }
                     catch (Exception ex)
                     {
                         LogError.Write(ex);
                         lblError.Text = Functions.GetErrorMessage(ex);
                         this.EjecutarScript(updResultados, "showfrmError();");
                         Tran.Dispose();
                     }
                 }
                 
                 if (CantRegistrosActualizados > 0)
                 {

                     lblMensaje.Text = string.Format("Se actualizaron {0} registro/s.", lblCantidadRegistros.Text);
                     this.EjecutarScript(updResultados, "showfrmMensaje();");
                     btnBuscar_Click(btnBuscar, new EventArgs());
                 }

             }
             catch (Exception ex)
             {
                 lblError.Text = ex.Message;
                 this.EjecutarScript(updResultados, "showfrmError();");
             }

         }

    }
}