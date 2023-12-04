using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.ABM
{
    public partial class AbmZonificacionParcelasCUR : BasePage
    {

        #region cargar inicial

        private bool filtrar = false;
        private string cod_calle = "0";
        private int nro_puerta_desde = 0;
        private int nro_puerta_hasta = 0;
        private int seccion = 0;
        private string manzana = "";
        private bool editar;
        private bool visualizar;

        #endregion

        #region permisos

        private void CargarPermisos()
        {
            using (var db = new DGHP_Entities())
            {
                Guid userid = Functions.GetUserId();

                var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

                foreach (var perfil in perfiles_usuario)
                {
                    var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

                    if (menu_usuario.Contains("Visualizar Ubicaciones"))
                        visualizar = true;

                    if (menu_usuario.Contains("Actualizar Mixturas y Distritos de Ubicaciones"))
                        editar = true;
                }
            }
        }

        #endregion

        #region load de pagina

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_ubi_dom, updPnlFiltroBuscar_ubi_dom.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updDDLDistritos, updDDLDistritos.GetType(), "init_Js_updpnlBuscarDistritos", "init_Js_updpnlBuscarDistritos();", true);
                
            }

            CargarPermisos();

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }

        public void LoadData()
        {
            try
            {
                hiddenTipoBusq.Value = "1";
                CargarComboCalles();
                CargarComboMixtura();
                CargarCombosDistritos();
                btnBuscar.Enabled = this.visualizar;
                updPnlFiltroBuscar_ubi_dom.Update();
                updPnlFiltroBuscar_ubi_smp.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                this.EjecutarScript(updPnlFiltroBuscar_ubi_dom, "finalizarCarga();");
                this.EjecutarScript(updPnlFiltroBuscar_ubi_smp, "finalizarCarga();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updPnlResultadoBuscar, "finalizarCarga();showfrmError();");
            }
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);

            string script_nombre = "mostrarMensaje";
            string script = "mostrarMensaje('" + mensaje + "','" + titulo + "');";

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm != null && sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(btn_BuscarPartida, btn_BuscarPartida.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
            }
        }

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            ddlUbiCalle.ClearSelection();
            txtPuertaDesde.Text = "";
            txtSeccion.Text = "";
            txtManzana.Text = "";
            txtPuertaHasta.Text = "";
            cod_calle = "0";
            nro_puerta_desde = 0;
            nro_puerta_hasta = 0;
            seccion = 0;
            manzana = "";
            hiddenTipoBusq.Value = "1";

            LimpiarMixtDist();

            updPnlFiltroBuscar_ubi_dom.Update();
            updPnlFiltroBuscar_ubi_smp.Update();
            updPnlResultadoBuscar.Update();
            updMixtDistZona.Update();
            pnlResultadoBuscar.Visible = false;
            EjecutarScript(btn_BuscarPartida, "hideResultado();");
        }

        private void LimpiarMixtDist()
        {
            ddlMixtura.ClearSelection();
            ddlGrupoDistritos.ClearSelection();
            ddlDistritos.ClearSelection();
            ddlDistritosZonas.ClearSelection();
            ddlDistritosSubZonas.ClearSelection();
            grdDistritos.DataSource = null;
            grdMixturas.DataSource = null;
            grdDistritos.DataBind();
            grdMixturas.DataBind();
            updMixtDistZona.Update();
        }

        private void BuscarUbicacion()
        {
            filtrar = true;
            LimpiarMixtDist();
            grdResultados.DataBind();
            updPnlResultadoBuscar.Update();
            pnlResultadoBuscar.Visible = true;            
            EjecutarScript(updPnlResultadoBuscar, "showResultado();");
            btnAgregarDistrito.Visible = this.editar;
            btnAgregarMixtura.Visible = this.editar;
            btnGuardar.Visible = this.editar;
        }

        private void Validar_BuscarPorUbicacion()
        {
            this.cod_calle = "0";
            this.nro_puerta_desde = 0;
            this.nro_puerta_hasta = 0;
            this.seccion = 0;
            this.manzana = "";
            int idAux;

            //filtro por smp
            if (hiddenTipoBusq.Value == "1" && (txtSeccion.Text.Trim() == "" || txtManzana.Text.Trim() == ""))
            {
                throw new Exception("Debe indicar Seccion y Manzana.");
            }

            int.TryParse(txtSeccion.Text, out idAux);
            this.seccion = idAux;
            this.manzana = txtManzana.Text.Trim();

            //filtro por domicilio
            if (hiddenTipoBusq.Value == "2" &&
                (int.Parse(ddlUbiCalle.SelectedValue) <= 0 || string.IsNullOrEmpty(txtPuertaDesde.Text) || string.IsNullOrEmpty(txtPuertaHasta.Text)))
            {
                throw new Exception("Debe indicar Calle, puerta desde y puerta hasta.");
            }

            this.cod_calle = ddlUbiCalle.SelectedValue;
            int.TryParse(txtPuertaDesde.Text.Trim(), out idAux);
            this.nro_puerta_desde = idAux;
            int.TryParse(txtPuertaHasta.Text.Trim(), out idAux);
            this.nro_puerta_hasta = idAux;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Validar_BuscarPorUbicacion();
                BuscarUbicacion();

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "error al buscar Partidas buscar_Partidas-btnBuscar_Click");
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(btn_BuscarPartida, "showfrmError();");
            }
        }

        private void CargarComboCalles()
        {
            using (DGHP_Entities db = new DGHP_Entities())
            {
                var lstCalles = (from cal in db.Calles
                                 select new ItemCalle
                                 {
                                     NombreOficial_calle = cal.NombreOficial_calle,
                                     Codigo_calle = cal.Codigo_calle,
                                     AlturaMin = cal.AlturaIzquierdaInicio_calle <= cal.AlturaDerechaInicio_calle ? cal.AlturaIzquierdaInicio_calle : cal.AlturaDerechaInicio_calle,
                                     AlturaMax = cal.AlturaDerechaFin_calle >= cal.AlturaIzquierdaFin_calle ? cal.AlturaDerechaFin_calle : cal.AlturaIzquierdaFin_calle
                                 }).Distinct().OrderBy(x => x.NombreOficial_calle);

                ddlUbiCalle.DataTextField = "NombreOficial_calle";
                ddlUbiCalle.DataValueField = "Codigo_calle";
                ddlUbiCalle.DataSource = lstCalles.ToList();
                ddlUbiCalle.DataBind();
                ddlUbiCalle.Items.Insert(0, "");
            }
        }

        private string GetNombreCalle(int codCalle, int nroPuerta)
        {
            using (var db = new DGHP_Entities())
            {
                var c = (from cal in db.Calles
                         where cal.Codigo_calle == codCalle
                         && (cal.AlturaIzquierdaInicio_calle <= cal.AlturaDerechaInicio_calle ? cal.AlturaIzquierdaInicio_calle : cal.AlturaDerechaInicio_calle) <= nroPuerta
                         && (cal.AlturaDerechaFin_calle >= cal.AlturaIzquierdaFin_calle ? cal.AlturaDerechaFin_calle : cal.AlturaIzquierdaFin_calle) >= nroPuerta
                         select cal).FirstOrDefault();

                if (c != null)
                    return c.NombreOficial_calle;

                return "";
            }
        }

        internal class ItemCalle
        {
            public string NombreOficial_calle { get; set; }
            public int Codigo_calle { get; set; }
            public int? AlturaMin { get; set; }
            public int? AlturaMax { get; set; }
        }

        private void CargarComboMixtura()
        {
            using (DGHP_Entities db = new DGHP_Entities())
            {
                List<Ubicaciones_ZonasMixtura> lista = db.Ubicaciones_ZonasMixtura.ToList();

                Ubicaciones_ZonasMixtura item = new Ubicaciones_ZonasMixtura();
                item.IdZonaMixtura = 0;
                item.Descripcion = "[Seleccione...]";
                lista.Insert(0, item);
                ddlMixtura.DataTextField = "Descripcion";
                ddlMixtura.DataValueField = "IdZonaMixtura";
                ddlMixtura.DataSource = lista.OrderBy(x => x.Descripcion);
                ddlMixtura.DataBind();
            }
        }

        private void CargarCombosDistritos()
        {
            using (DGHP_Entities db = new DGHP_Entities())
            {
                List<Ubicaciones_GruposDistritos> lista = db.Ubicaciones_GruposDistritos.ToList();

                lista.Insert(0, new Ubicaciones_GruposDistritos { IdGrupoDistrito = 0, Nombre = "[Seleccione...]" });

                ddlGrupoDistritos.DataTextField = "Nombre";
                ddlGrupoDistritos.DataValueField = "IdGrupoDistrito";
                ddlGrupoDistritos.DataSource = lista.OrderBy(x => x.Nombre);
                ddlGrupoDistritos.DataBind();
            }
        }

        protected void ddlGrupoDistritos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdGrupoDistrito = int.Parse(ddlGrupoDistritos.SelectedValue);
                if (IdGrupoDistrito != 0)
                {
                    CargarCombo_Distritos(IdGrupoDistrito, true);
                    CargarCombo_DistritosZonas(-1, false);
                    CargarCombo_DistritosSubZonas(-1, false);
                }
                else
                    ddlDistritos.Enabled = false;
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        private void CargarCombo_Distritos(int IdGrupoDistrito, bool value)
        {
            using (DGHP_Entities db = new DGHP_Entities())
            {
                List<ItemUbicacionesCatalogoDistritos> lista = (from ucd in db.Ubicaciones_CatalogoDistritos
                                                                where ucd.IdGrupoDistrito == IdGrupoDistrito
                                                                select new ItemUbicacionesCatalogoDistritos
                                                                {
                                                                    IdDistrito = ucd.IdDistrito,
                                                                    Codigo = ucd.Codigo + " - " + ucd.Descripcion
                                                                }).ToList();
                lista.Insert(0, new ItemUbicacionesCatalogoDistritos { IdDistrito = 0, Codigo = "[Seleccione...]" });
                ddlDistritos.DataTextField = "Codigo";
                ddlDistritos.DataValueField = "IdDistrito";
                ddlDistritos.DataSource = lista.OrderBy(x => x.Codigo);
                ddlDistritos.DataBind();

                ddlDistritos.Enabled = value;
            }
        }

        protected void ddlDistritosZonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdZona = int.Parse(ddlDistritosZonas.SelectedValue);
                if (IdZona != 0)
                    CargarCombo_DistritosSubZonas(IdZona, true);
                else
                    ddlDistritosSubZonas.Enabled = false;
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        private void CargarCombo_DistritosSubZonas(int idZona, bool value)
        {
            using (DGHP_Entities db = new DGHP_Entities())
            {
                List<Ubicaciones_CatalogoDistritos_Subzonas> lista = db.Ubicaciones_CatalogoDistritos_Subzonas.Where(x => x.IdZona == idZona).ToList();
                lista.Insert(0, new Ubicaciones_CatalogoDistritos_Subzonas { IdSubZona = 0, CodigoSubZona = "[Seleccione...]" });

                ddlDistritosSubZonas.DataTextField = "CodigoSubZona";
                ddlDistritosSubZonas.DataValueField = "IdSubZona";
                ddlDistritosSubZonas.DataSource = lista.OrderBy(x => x.CodigoSubZona);
                ddlDistritosSubZonas.DataBind();
                ddlDistritosSubZonas.Enabled = value;
            }
        }

        protected void ddlDistritos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdDistrito = int.Parse(ddlDistritos.SelectedValue);
                if (IdDistrito != 0)
                {
                    CargarCombo_DistritosZonas(IdDistrito, true);
                    CargarCombo_DistritosSubZonas(-1, false);
                }
                else
                    ddlDistritosZonas.Enabled = false;
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        private void CargarCombo_DistritosZonas(int IdDistrito, bool value)
        {
            using (DGHP_Entities db = new DGHP_Entities())
            {
                List<Ubicaciones_CatalogoDistritos_Zonas> lista = db.Ubicaciones_CatalogoDistritos_Zonas.Where(x => x.IdDistrito == IdDistrito).ToList();
                lista.Insert(0, new Ubicaciones_CatalogoDistritos_Zonas { IdZona = 0, CodigoZona = "[Seleccione...]" });

                ddlDistritosZonas.DataTextField = "CodigoZona";
                ddlDistritosZonas.DataValueField = "IdZona";
                ddlDistritosZonas.DataSource = lista.OrderBy(x => x.CodigoZona);
                ddlDistritosZonas.DataBind();
                ddlDistritosZonas.Enabled = value;
            }
        }

        public class ItemAux
        {
            public int id_ubicacion { get; set; }
            public Nullable<int> partida { get; set; }
            public Nullable<int> seccion { get; set; }
            public string manzana { get; set; }
            public string parcela { get; set; }
            public string mixtura { get; set; }
            public string distritos_zonas { get; set; }
            public string direccion { get; set; }
        }

        public class ItemZonificacionParcelasCUR
        {
            public int id_ubicacion { get; set; }
            public Nullable<int> partidaMatriz { get; set; }
            public Nullable<int> seccion { get; set; }
            public string manzana { get; set; }
            public string parcela { get; set; }
            public string mixturas { get; set; }
            public string distritos_zonas { get; set; }
            public string direccion { get; set; }
        }

        public List<ItemZonificacionParcelasCUR> GetResultados(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {
            totalRowCount = 0;

            using (var db = new DGHP_Entities())
            {
                if (!filtrar)
                {
                    pnlResultadoBuscar.Visible = false;
                    updPnlResultadoBuscar.Update();
                    return null;
                }

                // Preparamos el listado de Ubicaciones aplicando los filtros
                var w = db.Ubicaciones.Where(x => x.id_ubicacion > 0 && x.baja_logica == false);

                // Codigo de calle y numero de puerta
                int codigo_calle = 0;
                if (this.cod_calle.Trim() != "")
                {
                    codigo_calle = int.Parse(this.cod_calle);
                }

                bool par = radioNumeracionPar.Checked;

                bool impar = radioNumeracionImpar.Checked;

                if (this.seccion > 0)
                    w = w.Where(x => x.Seccion == this.seccion);

                if (this.manzana.Trim().Length > 0)
                    w = w.Where(x => x.Manzana.Contains(this.manzana.Trim()));

                var qa = (from qs in w
                          from ubiPuertas in qs.Ubicaciones_Puertas.DefaultIfEmpty()
                          join callesNombre in db.Calles on ubiPuertas != null ? ubiPuertas.codigo_calle : 0 equals callesNombre.Codigo_calle into cl
                          from callesNombre in cl.DefaultIfEmpty()
                          from uzm in qs.Ubicaciones_ZonasMixtura.DefaultIfEmpty()
                          from ud in qs.Ubicaciones_Distritos.DefaultIfEmpty()
                          join ucd in db.Ubicaciones_CatalogoDistritos on ud != null ? ud.IdDistrito : 0 equals ucd.IdDistrito into ucd1
                          from ucd in ucd1.DefaultIfEmpty()
                          join ucz in db.Ubicaciones_CatalogoDistritos_Zonas on ud != null ? ud.IdZona : 0 equals ucz.IdZona into ucz1
                          from ucz in ucz1.DefaultIfEmpty()
                          join ucs in db.Ubicaciones_CatalogoDistritos_Subzonas on ud != null ? ud.IdSubZona : 0 equals ucs.IdSubZona into ucs1
                          from ucs in ucs1.DefaultIfEmpty()
                          join ugd in db.Ubicaciones_GruposDistritos on ucd != null ? ucd.IdGrupoDistrito : 0 equals ugd.IdGrupoDistrito into ugd1
                          from ugd in ugd1.DefaultIfEmpty()
                          where (ubiPuertas != null ?
                            (ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaIzquierdaInicio_calle || ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaDerechaInicio_calle)
                            && (ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaIzquierdaFin_calle || ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaDerechaFin_calle) : true)
                            && (codigo_calle > 0 ?
                                (callesNombre != null && callesNombre.Codigo_calle == codigo_calle && ubiPuertas.NroPuerta_ubic >= this.nro_puerta_desde
                                    && ubiPuertas.NroPuerta_ubic <= this.nro_puerta_hasta
                                    && (par ? ubiPuertas.NroPuerta_ubic % 2 == 0 : true)
                                    && (impar ? ubiPuertas.NroPuerta_ubic % 2 != 0 : true)) : true)

                          select new ItemAux
                          {
                              id_ubicacion = qs.id_ubicacion,
                              manzana = qs.Manzana,
                              seccion = qs.Seccion,
                              parcela = qs.Parcela,
                              partida = qs.NroPartidaMatriz,
                              mixtura = uzm != null ? uzm.Descripcion : "",
                              distritos_zonas = (ugd != null ? ugd.Nombre + " - " : "") +
                                (ucd != null ? ucd.Codigo + " - " : "") +
                                (ucz != null ? ucz.CodigoZona + " - " : "") +
                                (ucs != null ? ucs.CodigoSubZona + " " : ""),
                              direccion = (callesNombre != null ? callesNombre.NombreOficial_calle + " " : "") + (ubiPuertas != null ? ubiPuertas.NroPuerta_ubic.ToString() : "")
                          }
                       ).Distinct();

                var u = (from ubi in qa.AsEnumerable()
                         group ubi by new
                         {
                             ubi.id_ubicacion,
                             ubi.manzana,
                             ubi.seccion,
                             ubi.parcela,
                             ubi.partida
                         } into ubis

                         select new ItemZonificacionParcelasCUR
                         {
                             id_ubicacion = ubis.Key.id_ubicacion,
                             manzana = ubis.Key.manzana,
                             seccion = ubis.Key.seccion,
                             parcela = ubis.Key.parcela,
                             partidaMatriz = ubis.Key.partida,
                             mixturas = string.Join(" / ", ubis.Where(a => a.mixtura.Length > 0).Select(x => x.mixtura).Distinct()),
                             distritos_zonas = string.Join(" / ", ubis.Where(a => a.distritos_zonas.Length > 0).Select(x => x.distritos_zonas).Distinct()),
                             direccion = string.Join(" / ", ubis.Select(x => x.direccion).Distinct())
                         }
                      );

                totalRowCount = u.Count();
                var resultadoPaginacion = u.Skip(startRowIndex).Take(maximumRows).ToList();

                Session["grdResultadoZonificacion"] = u.Take(totalRowCount).ToList();

                pnlCantidadRegistros.Visible = (resultadoPaginacion.Count() > 0);
                lblCantidadRegistros.Text = totalRowCount.ToString();


                return resultadoPaginacion;
            }
        }


        #region "Paging gridview Resultados"
        //Grilla resultados
        protected void grdResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdResultados.PageIndex = e.NewPageIndex;
                filtrar = true;
                Validar_BuscarPorUbicacion();
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
                filtrar = true;
                Validar_BuscarPorUbicacion();
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
                filtrar = true;
                Validar_BuscarPorUbicacion();
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
                filtrar = true;
                Validar_BuscarPorUbicacion();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
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

        private bool MixRepetida(string mix, string mixDescripcion)
        {
            DataTable dt = dtMixturasCargadas();

            var existe = (from r in dt.AsEnumerable()
                          where r.Field<String>("mix") == mix &&
                                r.Field<String>("mixDescripcion") == mixDescripcion
                          select r).ToList().Count();

            return existe != 0;
        }

        private DataTable dtMixturasCargadas()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("mix", typeof(string));
            dt.Columns.Add("mixDescripcion", typeof(string));
            dt.Columns.Add("mixturaAccion", typeof(string));

            foreach (GridViewRow row in grdMixturas.Rows)
            {
                DataRow datarw;
                datarw = dt.NewRow();
                datarw[0] = HttpUtility.HtmlDecode(row.Cells[0].Text);
                datarw[1] = HttpUtility.HtmlDecode(row.Cells[1].Text);
                datarw[2] = HttpUtility.HtmlDecode(row.Cells[2].Text);
                dt.Rows.Add(datarw);
            }
            return dt;
        }

        protected void grdMixturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnEliminarMix = (LinkButton)e.Row.FindControl("btnEliminarMixtura");
                btnEliminarMix.Visible = this.editar;
            }
        }

        protected void btnAgregarMixtura_Click(object sender, EventArgs e)
        {
            if (ddlMixtura.SelectedIndex != 0)
            {
                try
                {
                    DataTable dt = dtMixturasCargadas();
                    string mixDescripcion = (ddlMixtura.SelectedItem.Text).Trim();
                    string mix = (ddlMixtura.SelectedValue).Trim();

                    string mixturaAccion = "Agregar Mixtura";

                    if (!MixRepetida(mix, mixDescripcion))
                    {
                        DataRow datarw;
                        datarw = dt.NewRow();
                        datarw[0] = mix;
                        datarw[1] = mixDescripcion;
                        datarw[2] = mixturaAccion;
                        dt.Rows.Add(datarw);

                        grdMixturas.DataSource = dt;
                        grdMixturas.DataBind();
                    }
                    else
                    {
                        ddlMixtura.Focus();
                    }
                }
                catch (Exception ex)
                {
                    LogError.Write(ex);
                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updMixtura, "showfrmError();");
                }
            }
            ddlMixtura.SelectedIndex = 0;
        }

        protected void btnEliminarMixtura_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEliminarMixtura = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnEliminarMixtura.Parent.Parent;
                DataTable dt = dtMixturasCargadas();
                dt.Rows.RemoveAt(row.RowIndex);
                grdMixturas.DataSource = dt;
                grdMixturas.DataBind();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        protected void btnEliminarMixturasMasivas_Click(object sender, EventArgs e)
        {
            if (ddlMixtura.SelectedIndex != 0)
            {
                try
                {
                    DataTable dt = dtMixturasCargadas();
                    string mixDescripcion = (ddlMixtura.SelectedItem.Text).Trim();
                    string mix = (ddlMixtura.SelectedValue).Trim();

                    string mixturaAccion = "Eliminar Mixtura";

                    if (!MixRepetida(mix, mixDescripcion))
                    {
                        DataRow datarw;
                        datarw = dt.NewRow();
                        datarw[0] = mix;
                        datarw[1] = mixDescripcion;
                        datarw[2] = mixturaAccion;
                        dt.Rows.Add(datarw);

                        grdMixturas.DataSource = dt;
                        grdMixturas.DataBind();
                    }
                    else
                    {
                        ddlMixtura.Focus();
                    }
                }
                catch (Exception ex)
                {
                    LogError.Write(ex);
                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updMixtura, "showfrmError();");
                }
            }
            ddlMixtura.SelectedIndex = 0;
        }


        protected void grdDistritos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnEliminarDis = (LinkButton)e.Row.FindControl("btnEliminarDistrito");
                btnEliminarDis.Visible = this.editar;
            }
        }

        protected void btnAgregarDistrito_Click(object sender, EventArgs e)
        {
            if (ddlGrupoDistritos.SelectedIndex != 0)
            {
                try
                {
                    DataTable dt = dtDistritosCargados();

                    string grupoDistrito = ddlGrupoDistritos.SelectedItem.Text.Trim();
                    string distrito = ddlDistritos.SelectedIndex != -1 ? (ddlDistritos.SelectedIndex != 0 ? ddlDistritos.SelectedItem.Text.Trim() : string.Empty) : string.Empty;
                    string zonas = ddlDistritosZonas.SelectedIndex != -1 ? (ddlDistritosZonas.SelectedIndex != 0 ? ddlDistritosZonas.SelectedItem.Text.Trim() : string.Empty) : string.Empty;
                    string subZonas = ddlDistritosSubZonas.SelectedIndex != -1 ? (ddlDistritosSubZonas.SelectedIndex != 0 ? ddlDistritosSubZonas.SelectedItem.Text.Trim() : string.Empty) : string.Empty;
                    
                    string distritoAccion = "Agregar Distrito";

                    int IdDistrito = 0;
                    int.TryParse(ddlDistritos.SelectedValue, out IdDistrito);

                    var IdZonas = 0;
                    var IdSubZonas = 0;

                    if (!String.IsNullOrEmpty(zonas))
                    {
                        IdZonas = int.Parse(ddlDistritosZonas.SelectedValue);
                    }
                    if (!String.IsNullOrEmpty(subZonas))
                    {
                        IdSubZonas = int.Parse(ddlDistritosSubZonas.SelectedValue);
                    }

                    if (!DistritoRepetido(grupoDistrito, distrito, zonas, subZonas))
                    {
                        DataRow datarw;
                        datarw = dt.NewRow();

                        datarw[0] = grupoDistrito;
                        datarw[1] = distrito;
                        datarw[2] = zonas;
                        datarw[3] = subZonas;
                        datarw[4] = IdDistrito;
                        datarw[5] = IdZonas;
                        datarw[6] = IdSubZonas;
                        datarw[7] = distritoAccion;

                        dt.Rows.Add(datarw);

                        grdDistritos.DataSource = dt;
                        grdDistritos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    LogError.Write(ex);
                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updDistritos, "showfrmError();");
                }
            }
            ddlGrupoDistritos.SelectedIndex = ddlDistritos.SelectedIndex = ddlDistritosZonas.SelectedIndex = ddlDistritosSubZonas.SelectedIndex = 0;
            ddlDistritos.Enabled = ddlDistritosZonas.Enabled = ddlDistritosSubZonas.Enabled = false;
        }

        private bool DistritoRepetido(string grupoDistrito, string distrito, string zonas, string subzonas)
        {
            DataTable dt = dtDistritosCargados();

            var existe = (from r in dt.AsEnumerable()
                          where r.Field<String>("grupoDistrito") == grupoDistrito &&
                                r.Field<String>("distrito") == distrito &&
                                r.Field<String>("zonas") == zonas &&
                                r.Field<String>("subzonas") == subzonas
                          select r).ToList().Count();
            return existe != 0;
        }

        private DataTable dtDistritosCargados()
        {
            DataTable dtDistritos = new DataTable();

            dtDistritos.Columns.Add("grupoDistrito", typeof(string));
            dtDistritos.Columns.Add("distrito", typeof(string));
            dtDistritos.Columns.Add("zonas", typeof(string));
            dtDistritos.Columns.Add("subzonas", typeof(string));
            dtDistritos.Columns.Add("IdDistrito", typeof(int));
            dtDistritos.Columns.Add("IdZona", typeof(int));
            dtDistritos.Columns.Add("IdSubZona", typeof(int));
            dtDistritos.Columns.Add("distritoAccion", typeof(string));

            foreach (GridViewRow row in grdDistritos.Rows)
            {
                int distrito = 0;
                int zona = 0;
                int subZona = 0;
                DataRow datarw;
                datarw = dtDistritos.NewRow();
                datarw[0] = HttpUtility.HtmlDecode(row.Cells[0].Text);
                datarw[1] = HttpUtility.HtmlDecode(row.Cells[1].Text);
                datarw[2] = HttpUtility.HtmlDecode(row.Cells[2].Text);
                datarw[3] = HttpUtility.HtmlDecode(row.Cells[3].Text);
                int.TryParse(HttpUtility.HtmlDecode(row.Cells[4].Text), out distrito);
                datarw[4] = distrito;
                int.TryParse(HttpUtility.HtmlDecode(row.Cells[5].Text), out zona);
                datarw[5] = zona;
                int.TryParse(HttpUtility.HtmlDecode(row.Cells[6].Text), out subZona);
                datarw[6] = subZona;
                datarw[7] = HttpUtility.HtmlDecode(row.Cells[7].Text);

                dtDistritos.Rows.Add(datarw);
            }
            return dtDistritos;
        }

        protected void btnEliminarDistrito_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEliminarDistrito = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnEliminarDistrito.Parent.Parent;
                DataTable dt = dtDistritosCargados();
                dt.Rows.RemoveAt(row.RowIndex);
                grdDistritos.DataSource = dt;
                grdDistritos.DataBind();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        protected void btnEliminarDistritosMasivos_Click(object sender, EventArgs e)
        {
            if (ddlGrupoDistritos.SelectedIndex != 0)
            {
                try
                {
                    DataTable dt = dtDistritosCargados();

                    string grupoDistrito = ddlGrupoDistritos.SelectedItem.Text.Trim();
                    string distrito = ddlDistritos.SelectedIndex != -1 ? (ddlDistritos.SelectedIndex != 0 ? ddlDistritos.SelectedItem.Text.Trim() : string.Empty) : string.Empty;
                    string zonas = ddlDistritosZonas.SelectedIndex != -1 ? (ddlDistritosZonas.SelectedIndex != 0 ? ddlDistritosZonas.SelectedItem.Text.Trim() : string.Empty) : string.Empty;
                    string subZonas = ddlDistritosSubZonas.SelectedIndex != -1 ? (ddlDistritosSubZonas.SelectedIndex != 0 ? ddlDistritosSubZonas.SelectedItem.Text.Trim() : string.Empty) : string.Empty;

                    string distritoAccion = "Eliminar Distrito";

                    int IdDistrito = 0;
                    int.TryParse(ddlDistritos.SelectedValue, out IdDistrito);

                    var IdZonas = 0;
                    var IdSubZonas = 0;

                    if (!String.IsNullOrEmpty(zonas))
                    {
                        IdZonas = int.Parse(ddlDistritosZonas.SelectedValue);
                    }
                    if (!String.IsNullOrEmpty(subZonas))
                    {
                        IdSubZonas = int.Parse(ddlDistritosSubZonas.SelectedValue);
                    }

                    if (!DistritoRepetido(grupoDistrito, distrito, zonas, subZonas))
                    {
                        DataRow datarw;
                        datarw = dt.NewRow();

                        datarw[0] = grupoDistrito;
                        datarw[1] = distrito;
                        datarw[2] = zonas;
                        datarw[3] = subZonas;
                        datarw[4] = IdDistrito;
                        datarw[5] = IdZonas;
                        datarw[6] = IdSubZonas;
                        datarw[7] = distritoAccion;

                        dt.Rows.Add(datarw);

                        grdDistritos.DataSource = dt;
                        grdDistritos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    LogError.Write(ex);
                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updDistritos, "showfrmError();");
                }
            }
            ddlGrupoDistritos.SelectedIndex = ddlDistritos.SelectedIndex = ddlDistritosZonas.SelectedIndex = ddlDistritosSubZonas.SelectedIndex = 0;
            ddlDistritos.Enabled = ddlDistritosZonas.Enabled = ddlDistritosSubZonas.Enabled = false;
        }

        private List<Ubicaciones_Distritos> CrearListaDistritosAgregar(int id_ubicacion, GridView grdDistritos, List<Ubicaciones_Distritos> listaDistritosInicial)
        {
            var list = new List<Ubicaciones_Distritos>();

            foreach (GridViewRow item in grdDistritos.Rows)
            {
                if (item.Cells[7].Text == "Agregar Distrito")
                {

                    int id_distrito = Convert.ToInt16(item.Cells[4].Text);
                    int? id_Zona = null;
                    int? id_SubZona = null;

                    if (item.Cells[5].Text != "0" && item.Cells[5].Text != "&nbsp;")
                    {
                        id_Zona = Convert.ToInt16(item.Cells[5].Text);
                    }
                    if (item.Cells[6].Text != "0" && item.Cells[6].Text != "&nbsp;")
                    {
                        id_SubZona = Convert.ToInt16(item.Cells[6].Text);
                    }

                    if (!listaDistritosInicial.Where(x => x.IdZona == id_Zona && x.IdSubZona == id_SubZona && x.IdDistrito == id_distrito).Any())
                    {
                        Ubicaciones_Distritos dis = new Ubicaciones_Distritos();
                        dis.id_ubicacion = id_ubicacion;
                        dis.IdDistrito = id_distrito;
                        dis.IdZona = id_Zona;
                        dis.IdSubZona = id_SubZona;
                        list.Add(dis);
                    }
                    else
                        list.Add(listaDistritosInicial.Where(x => x.IdZona == id_Zona && x.IdSubZona == id_SubZona && x.IdDistrito == id_distrito).SingleOrDefault());
                }

            }
            return list;
        }


        private List<Ubicaciones_Distritos> CrearListaDistritosEliminar(int id_ubicacion, GridView grdDistritos, List<Ubicaciones_Distritos> listaDistritosInicial)
        {
            var list = new List<Ubicaciones_Distritos>();

            foreach (GridViewRow item in grdDistritos.Rows)
            {
                if (item.Cells[7].Text == "Eliminar Distrito")
                {
                    int id_distrito = Convert.ToInt16(item.Cells[4].Text);
                    int? id_Zona = null;
                    int? id_SubZona = null;

                    if (item.Cells[5].Text != "0" && item.Cells[5].Text != "&nbsp;")
                    {
                        id_Zona = Convert.ToInt16(item.Cells[5].Text);
                    }
                    if (item.Cells[6].Text != "0" && item.Cells[6].Text != "&nbsp;")
                    {
                        id_SubZona = Convert.ToInt16(item.Cells[6].Text);
                    }

                    if (listaDistritosInicial.Where(x => x.IdZona == id_Zona && x.IdSubZona == id_SubZona && x.IdDistrito == id_distrito).Any())
                    {
                        Ubicaciones_Distritos dis = new Ubicaciones_Distritos();
                        dis.id_ubicacion = id_ubicacion;
                        dis.IdDistrito = id_distrito;
                        dis.IdZona = id_Zona;
                        dis.IdSubZona = id_SubZona;
                        list.Add(dis);
                    }
                }
            }
            return list;
        }


        private List<Ubicaciones_ZonasMixtura> CrearListaMixturasAgregar(int id_ubicacion, GridView grdMixturas, List<Ubicaciones_ZonasMixtura> listaMixturasInicial)
        {
            var list = new List<Ubicaciones_ZonasMixtura>();
            foreach (GridViewRow item in grdMixturas.Rows)
            {
                if (item.Cells[2].Text == "Agregar Mixtura")
                {
                    int id_zonamix = Convert.ToInt16(item.Cells[0].Text);
                    if (!listaMixturasInicial.Where(x => x.IdZonaMixtura == id_zonamix).Any())
                        list.Add(new Ubicaciones_ZonasMixtura { IdZonaMixtura = id_zonamix });
                    else
                        list.Add(listaMixturasInicial.Where(x => x.IdZonaMixtura == id_zonamix).SingleOrDefault());
                }
            }
            return list;
        }


        private List<Ubicaciones_ZonasMixtura> CrearListaMixturasEliminar(int id_ubicacion, GridView grdMixturas, List<Ubicaciones_ZonasMixtura> listaMixturasInicial)
        {
            var list = new List<Ubicaciones_ZonasMixtura>();
            foreach (GridViewRow item in grdMixturas.Rows)
            {
                if (item.Cells[2].Text == "Eliminar Mixtura")
                {
                    int id_zonamix = Convert.ToInt16(item.Cells[0].Text);
                    if (listaMixturasInicial.Where(x => x.IdZonaMixtura == id_zonamix).Any())
                        list.Add(new Ubicaciones_ZonasMixtura { IdZonaMixtura = id_zonamix });
                }
            }
            return list;
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            List<ItemZonificacionParcelasCUR> ubicaciones = ((List<ItemZonificacionParcelasCUR>)Session["grdResultadoZonificacion"]);
            try
            {
                if (grdMixturas.Rows.Count == 0 && grdDistritos.Rows.Count == 0)
                    throw new Exception("Debe agregar mixturas o distritos primero.");

                int actualizadas = 0;
                foreach (ItemZonificacionParcelasCUR ubi in ubicaciones)
                {
                    using (var context = new DGHP_Entities())
                    {
                        bool actualizar = false;
                        int id = ubi.id_ubicacion;
                        var entity = context.Ubicaciones.Where(x => x.id_ubicacion == id).FirstOrDefault();
                        if (entity == null)
                            continue;

                        if (grdDistritos.Rows.Count > 0)
                        {
                            foreach (GridViewRow itemDistrito in grdDistritos.Rows)
                            {

                                if (itemDistrito.Cells[7].Text == "Agregar Distrito")
                                {
                                    List<Ubicaciones_Distritos> listaDistritosInicial = entity.Ubicaciones_Distritos.ToList();
                                    List<Ubicaciones_Distritos> listaDistritos = CrearListaDistritosAgregar(entity.id_ubicacion, grdDistritos, new List<Ubicaciones_Distritos>());
                                    foreach (var item in listaDistritos)
                                    {
                                        if (!listaDistritosInicial.Where(d => d.IdDistrito == item.IdDistrito && d.id_ubicacion == item.id_ubicacion && d.IdZona == item.IdZona && d.IdSubZona == item.IdSubZona).Any())
                                        {
                                            entity.Ubicaciones_Distritos.Add(item);
                                            context.Ubicaciones_Distritos.Add(item);
                                            actualizar = true;
                                        }
                                    }
                                }

                                else if (itemDistrito.Cells[7].Text == "Eliminar Distrito")
                                {
                                    List<Ubicaciones_Distritos> listaDistritosInicial = (from ud in entity.Ubicaciones_Distritos where ud.id_ubicacion == id select ud).ToList();
                                    List<Ubicaciones_Distritos> listaDistritos = CrearListaDistritosEliminar(entity.id_ubicacion, grdDistritos, listaDistritosInicial);
                                    if (listaDistritos.Count != 0)
                                    {
                                        foreach (var item in listaDistritos)
                                        {
                                            var distritoEliminar = (from ud in context.Ubicaciones_Distritos where ud.id_ubicacion == item.id_ubicacion select ud).FirstOrDefault();
                                            context.Ubicaciones_Distritos.Remove(distritoEliminar);
                                            actualizar = true;
                                        }
                                    }
                                }
                            }
                        }

                        if (grdMixturas.Rows.Count > 0)
                        {
                            foreach (GridViewRow itemMixtura in grdMixturas.Rows)
                            {
                                if (itemMixtura.Cells[2].Text == "Agregar Mixtura")
                                {
                                    List<Ubicaciones_ZonasMixtura> listaMixturasInicial = entity.Ubicaciones_ZonasMixtura.ToList();
                                    List<Ubicaciones_ZonasMixtura> listaMix = CrearListaMixturasAgregar(entity.id_ubicacion, grdMixturas, new List<Ubicaciones_ZonasMixtura>());
                                    foreach (var item in listaMix)
                                    {
                                        if (!listaMixturasInicial.Where(m => m.IdZonaMixtura == item.IdZonaMixtura).Any())
                                        {
                                            Ubicaciones_ZonasMixtura a = context.Ubicaciones_ZonasMixtura.Find(item.IdZonaMixtura);
                                            entity.Ubicaciones_ZonasMixtura.Add(a);
                                            actualizar = true;
                                        }
                                    }
                                }

                                if (itemMixtura.Cells[2].Text == "Eliminar Mixtura")
                                {
                                    List<Ubicaciones_ZonasMixtura> listaMixturasInicial = entity.Ubicaciones_ZonasMixtura.ToList();
                                    List<Ubicaciones_ZonasMixtura> listaMixturas = CrearListaMixturasEliminar(entity.id_ubicacion, grdMixturas, listaMixturasInicial);
                                    if (listaMixturasInicial.Count != 0)
                                    {
                                        foreach (var item in listaMixturas)
                                        {
                                            Ubicaciones_ZonasMixtura a = context.Ubicaciones_ZonasMixtura.Find(item.IdZonaMixtura);
                                            entity.Ubicaciones_ZonasMixtura.Remove(a);
                                            actualizar = true;
                                        }
                                    }
                                }
                            }
                        }

                        if (!actualizar)
                            continue;
                        context.Database.CommandTimeout = 0;
                        using (var dbContextTransaction = context.Database.BeginTransaction())
                        {
                            try
                            {
                                context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                dbContextTransaction.Commit();
                                actualizadas++;
                            }
                            catch (Exception ex)
                            {
                                dbContextTransaction.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
                grdResultados.DataBind();

                Session["grdResultadoZonificacion"] = null;
                lblMensaje.Text = string.Format("Se actualizaron {0} ubicaciones.", actualizadas.ToString());
                this.EjecutarScript(updBotonesGuardar, "showfrmMensaje()");

                if (actualizadas > 0)
                {
                    btnBuscar_Click(btnBuscar, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updBotonesGuardar, "showfrmError();");
                return;
            }
        }



        protected void btnContinuarAdv_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancelarAdv_Click(object sender, EventArgs e)
        {
            this.EjecutarScript(updfrmAdvBody, "hidefrmAdvertencia();");
        }
    }

    public class ItemUbicacionesCatalogoDistritos
    {
        public int IdDistrito { get; set; }
        public string Codigo { get; set; }
    }






}