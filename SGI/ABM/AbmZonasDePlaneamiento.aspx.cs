using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SGI.Model;
using System.Transactions;
using Newtonsoft.Json;

namespace SGI.ABM
{
    public partial class AbmZonasDePlaneamiento : BasePage{

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

        #region load de pagina
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "init_Js_updpnlCrearActu", "init_Js_updpnlCrearActu();", true);

            }


            if (!IsPostBack)
            {
                db = new DGHP_Entities();
                cargarZonasHabilitacion();
            }
        }
        #endregion

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        protected void btnNuevaZonaPlaneamiento_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarDatos();
                updDatos.Update();
                this.EjecutarScript(updpnlBuscar, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "finalizarCarga();showfrmError();");

            }
        }

        private void LimpiarDatosBusqueda()
        {
            txtCodigoZona.Text = "";
            txtNombreZona.Text = "";
            txtZonaHabilitacionReq.SelectedValue = "-1";
        }

        private void LimpiarDatos()
        {
            hid_id_condReq.Value = "0";
            txtEditIdPlanHabil.Value = "0";
            txtCodigoZonaReq.Text = "";
            txtNombreZonaReq.Text = "";
            txtCodigoZonaReq.Enabled = true;
            txtZonaHabilitacionReq.SelectedValue = "-1";
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarZonasPlaneamiento();
                updResultados.Update();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "showfrmError();");
            }
        }

        private void BuscarZonasPlaneamiento()
        {
            db = new DGHP_Entities();
            var q = (from zonas in db.Zonas_Planeamiento
                     join rel_zonasplaneamiento_zonashabilitaciones in db.Rel_ZonasPlaneamiento_ZonasHabilitaciones
                     on zonas.CodZonaPla equals rel_zonasplaneamiento_zonashabilitaciones.CodZonaLey449 into zr
                     from fd in zr.DefaultIfEmpty()
                     join zonas_habilitaciones in db.Zonas_Habilitaciones
                     on fd.CodZonaHab equals zonas_habilitaciones.CodZonaHab into rh
                     from hd in rh.DefaultIfEmpty()
                     select new
                     {
                         id_cond = zonas.id_zonaplaneamiento,
                         codigo_zona = zonas.CodZonaPla,
                         nombre_zona = zonas.DescripcionZonaPla,
                         codigo_habilitacion = hd.CodZonaHab,
                         nombre_habilitacion = hd.DescripcionZonaHab
                     }
                     );
            if (txtNombreZona.Text.Trim().Length > 0)
                q = q.Where(x => x.nombre_zona.Contains(txtNombreZona.Text.Trim()));
            if (txtCodigoZona.Text.Trim().Length > 0)
                q = q.Where(x => x.codigo_zona.Contains(txtCodigoZona.Text.Trim()));
            if (txtZonaHabilitacionList.SelectedItem.Text.Trim().Length > 0 && !txtZonaHabilitacionList.SelectedItem.Text.Trim().Equals("Todas"))
                q = q.Where(x => x.codigo_habilitacion.Equals(txtZonaHabilitacionList.SelectedItem.Value));

            grdResultados.DataSource = q.OrderBy(x => x.codigo_zona).ToList();
            grdResultados.DataBind();

            pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
            lblCantidadRegistros.Text = q.OrderBy(x => x.codigo_zona).ToList().Count.ToString();
            db.Dispose();
        }

        private void CargarDatos(int? id_datos)
        {
           
            db = new DGHP_Entities();
            cargarZonasHabilitacion();
            var dato = (from zonas in db.Zonas_Planeamiento
                        join rel_zonasplaneamiento_zonashabilitaciones in db.Rel_ZonasPlaneamiento_ZonasHabilitaciones
                        on zonas.CodZonaPla equals rel_zonasplaneamiento_zonashabilitaciones.CodZonaLey449 into zr
                        from fd in zr.DefaultIfEmpty()
                        join zonas_habilitaciones in db.Zonas_Habilitaciones
                        on fd.CodZonaHab equals zonas_habilitaciones.CodZonaHab into rh
                        from hd in rh.DefaultIfEmpty() 
                     select new
                     {
                         id_zonaplaneamiento = zonas.id_zonaplaneamiento,
                           codigo_zona = zonas.CodZonaPla,
                           nombre_zona = zonas.DescripcionZonaPla,
                           codigo_habilitacion = fd.CodZonaHab,
                           nombre_habilitacion = hd.DescripcionZonaHab,
                         plan_habil =(int?)fd.id_rel_zonapla_zonahab
                     }
                     ).FirstOrDefault(x => x.id_zonaplaneamiento == id_datos);
                
            if (dato != null)
            {
                hid_id_condReq.Value = id_datos.ToString();
                int? a = dato.plan_habil;
                if (a!= null)
                    txtEditIdPlanHabil.Value = a.ToString();
                txtCodigoZonaReq.Enabled = false;
                txtCodigoZonaReq.Text = dato.codigo_zona;
                txtNombreZonaReq.Text = dato.nombre_zona;
                txtZonaHabilitacionReq.SelectedValue = dato.codigo_habilitacion;
            }
            db.Dispose();
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEditar = (LinkButton)sender;
                int? id_datos = int.Parse(btnEditar.CommandArgument);

                LimpiarDatos();
                CargarDatos(id_datos);
                updDatos.Update();
                this.EjecutarScript(updResultados, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");

            }

        }

        protected void lnkEliminarCondicionReq_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;

                LinkButton lnkEditar = (LinkButton)sender;
                int idZonaPlaneamiento = int.Parse(lnkEditar.CommandArgument);

                db = new DGHP_Entities();

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        db.Zonas_Planeamiento_delete(idZonaPlaneamiento, userid);

                        Tran.Complete();
                        string script = "$('#frmEliminarLog').modal('show');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
                        hid_id_object.Value = idZonaPlaneamiento.ToString();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                BuscarZonasPlaneamiento();
                updResultados.Update();
                this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            db = new DGHP_Entities();
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
                string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();

                int idZonaPlaneamiento = Convert.ToInt32(hid_id_condReq.Value);
                string codigoZona = txtCodigoZonaReq.Text.Trim();
                string nombreZona = txtNombreZonaReq.Text.Trim();
                string codigoZonaHabilitacion = (txtZonaHabilitacionReq.SelectedValue.Trim() == "-1") ? "" : txtZonaHabilitacionReq.SelectedValue.Trim();
                
                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        if (idZonaPlaneamiento == 0)
                        {
                            idZonaPlaneamiento = db.Zonas_Planeamiento_insert(codigoZona, nombreZona, userid, codigoZonaHabilitacion);
                            Zonas_Planeamiento obj = db.Zonas_Planeamiento.FirstOrDefault(x => x.id_zonaplaneamiento == idZonaPlaneamiento);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitantes.Text, "I", 1020);
                        }
                        else
                        {
                            db.Zonas_Planeamiento_update(idZonaPlaneamiento, codigoZona, nombreZona, userid, Convert.ToInt32(txtEditIdPlanHabil.Value), codigoZonaHabilitacion);
                            Zonas_Planeamiento obj = db.Zonas_Planeamiento.FirstOrDefault(x => x.id_zonaplaneamiento == idZonaPlaneamiento);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitantes.Text, "U", 1020);
                        }
                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                BuscarZonasPlaneamiento();
                updResultados.Update();

                this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updBotonesGuardar, "showfrmError();");

            }
            finally
            {
                db.Dispose();
            }

        }


         private void cargarZonasHabilitacion()
        {

            string appName = Constants.ApplicationName;

            var list_zonas = (from zhabilitaciones in db.Zonas_Habilitaciones
                                  select new {
                                                         CodZonaHab = zhabilitaciones.CodZonaHab,
                                                         DescripcionZonaHab = zhabilitaciones.CodZonaHab + " - " + zhabilitaciones.DescripcionZonaHab
                                                     }).ToList();

            var list_zonas_update = (from zhabilitaciones in db.Zonas_Habilitaciones
                                                            select new 
                                                            {
                                                                CodZonaHab = zhabilitaciones.CodZonaHab,
                                                                DescripcionZonaHab = zhabilitaciones.CodZonaHab + " - " + zhabilitaciones.DescripcionZonaHab
                                                            }).ToList();

           Zonas_Habilitaciones itemVacio = new Zonas_Habilitaciones();
           itemVacio.CodZonaHab = "-1";
           itemVacio.DescripcionZonaHab = "Todas";
           list_zonas.Insert(0, new { itemVacio.CodZonaHab, itemVacio.DescripcionZonaHab });

            txtZonaHabilitacionList.DataValueField = "CodZonaHab";
            txtZonaHabilitacionList.DataTextField = "DescripcionZonaHab";
            txtZonaHabilitacionList.DataSource = list_zonas;
            txtZonaHabilitacionList.DataBind();

            Zonas_Habilitaciones itemVacioUpdate = new Zonas_Habilitaciones();
            itemVacioUpdate.CodZonaHab = "-1";
            itemVacioUpdate.DescripcionZonaHab = "Sin vincular a zona de Habilitación";
            list_zonas_update.Insert(0, new { itemVacioUpdate.CodZonaHab, itemVacioUpdate.DescripcionZonaHab });

            txtZonaHabilitacionReq.DataValueField = "CodZonaHab";
            txtZonaHabilitacionReq.DataTextField = "DescripcionZonaHab";
            txtZonaHabilitacionReq.DataSource = list_zonas_update; 
            txtZonaHabilitacionReq.DataBind();
        }


         #region paginado grilla

         private int codZonaPlaneamiento = 0;
         private string nombreZonaPlaneamiento = "";
         private int codZonaHabilitacion = 0;
         

         protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
         {

             try
             {
                 grdResultados.PageIndex = e.NewPageIndex;
                 IniciarEntity();
                 BuscarZonasPlaneamiento();
             }
             catch (Exception ex)
             {
                 Enviar_Mensaje(ex.Message, "");
             }

         }

         protected void cmdPage(object sender, EventArgs e)
         {
             LinkButton cmdPage = (LinkButton)sender;

             try
             {
                 grdResultados.PageIndex = int.Parse(cmdPage.Text) - 1;
                 IniciarEntity();
                 BuscarZonasPlaneamiento();
             }
             catch (Exception ex)
             {
                 Enviar_Mensaje(ex.Message, "");
             }
         }

         protected void cmdAnterior_Click(object sender, EventArgs e)
         {

             try
             {
                 grdResultados.PageIndex = grdResultados.PageIndex - 1;
                 IniciarEntity();
                 BuscarZonasPlaneamiento();
             }
             catch (Exception ex)
             {
                 Enviar_Mensaje(ex.Message, "");
             }
         }

         protected void cmdSiguiente_Click(object sender, EventArgs e)
         {

             try
             {
                 grdResultados.PageIndex = grdResultados.PageIndex + 1;
                 IniciarEntity();
                 BuscarZonasPlaneamiento();
             }
             catch (Exception ex)
             {
                 Enviar_Mensaje(ex.Message, "");
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

         private void elimiarFiltro()
         {
             ViewState["filtro"] = null;
         }

         private void guardarFiltro()
         {
             string filtro = this.codZonaPlaneamiento + "|" + this.nombreZonaPlaneamiento + "|" + this.codZonaHabilitacion;
             ViewState["filtro"] = filtro;

         }

         private void recuperarFiltro()
         {
             if (ViewState["filtro"] == null)
                 return;

             string filtro = ViewState["filtro"].ToString();

             string[] valores = filtro.Split('|');

             this.codZonaPlaneamiento = Convert.ToInt32(valores[0]);
             this.codZonaHabilitacion = Convert.ToInt32(valores[2]);

             if (string.IsNullOrEmpty(valores[1]))
             {
                 this.nombreZonaPlaneamiento = null;
             }
             else
             {
                 this.nombreZonaPlaneamiento = valores[1];
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

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            Zonas_Planeamiento obj = db.Zonas_Planeamiento.FirstOrDefault(x => x.id_zonaplaneamiento == int.Parse(hid_id_object.Value));
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "D", 1020);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            Zonas_Planeamiento obj = db.Zonas_Planeamiento.FirstOrDefault(x => x.id_zonaplaneamiento == int.Parse(hid_id_object.Value));
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, string.Empty, "D", 1020);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
        }
        #endregion
    }
}