using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.Security;

namespace SGI.ABM.Partidas
{
    public partial class AbmUbicaciones : BasePage
    {

        #region cargar inicial

        //private int id_ubicacion = -1;
        private int estadoNueva = -1;
        private int estadoEnProceso = 0;
        private bool filtrar = false;
        private int nro_partida_matriz = 0;
        private int nro_partida_horiz = 0;
        private string cod_calle = "0";
        private int nro_puerta = 0;
        private int? seccion = 0;
        private string manzana = "";
        private string parcela = "";
        private int id_tipo_ubicacion = -1;
        private int id_sub_tipo_ubicacion = -1;
        private int estadoAprobada = 3;
        private int codigo_calle = 0;

        private bool editar;
        private bool visualizar;

        bool filtroPHSubGrilla = false;

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
                    if (menu_usuario.Contains("Editar Ubicaciones"))
                    {
                        editar = true;
                    }
                }
            }
        }

        #endregion

        protected class FiltroBusqueda
        {
            public int tipoBusq { get; set; }
            public bool tipoPartidaMatriz { get; set; }
            public bool tipoPartidaHoriz { get; set; }
            public string nroPartida { get; set; }
            public int codCalle { get; set; }
            public int nroPuerta { get; set; }
            public string nomCalle { get; set; }
            public string seccion { get; set; }
            public string manzana { get; set; }
            public string parcela { get; set; }
            public int tipoUbicacion { get; set; }
            public int subtipoUbicacion { get; set; }
            public bool baja_logica { get; set; }
        }

        // Guarda todos los filtros de Busqueda        
        private void GuardarFiltros()
        {
            var filtro = new FiltroBusqueda();

            filtro.tipoBusq = int.Parse(hiddenTipoBusq.Value != "" ? hiddenTipoBusq.Value : "1");
            filtro.tipoPartidaMatriz = rbtnUbiPartidaMatriz.Checked;
            filtro.tipoPartidaHoriz = rbtnUbiPartidaHoriz.Checked;
            filtro.nroPartida = txtUbiNroPartida.Text.Trim();
            filtro.codCalle = int.Parse(ddlUbiCalle.SelectedValue != "" ? ddlUbiCalle.SelectedValue : "0");
            filtro.nroPuerta = int.Parse(txtUbiNroPuerta.Text.Trim() != "" ? txtUbiNroPuerta.Text.Trim() : "0");
            filtro.nomCalle = GetNombreCalle(filtro.codCalle, filtro.nroPuerta);
            filtro.seccion = txtUbiSeccion.Text.Trim();
            filtro.manzana = txtUbiManzana.Text.Trim();
            filtro.parcela = txtUbiParcela.Text.Trim();
            filtro.tipoUbicacion = int.Parse(ddlUbiTipoUbicacion.SelectedValue);
            filtro.subtipoUbicacion = int.Parse(ddlUbiSubTipoUbicacion.SelectedValue);
            filtro.baja_logica = bool.Parse(ddlBaja.SelectedValue);

            Session["BuscUbiFiltro"] = filtro;
        }

        // Se obtienen los valores almacenados de la busqueda anterior
        private void CargarFiltros()
        {
            // Por defecto, su duracion es de 20'
            // Propiedad de SessionState en web.config
            if (Session["BuscUbiFiltro"] != null)
            {
                var filtro = (FiltroBusqueda)Session["BuscUbiFiltro"];

                txtUbiNroPartida.Text = filtro.nroPartida;
                rbtnUbiPartidaMatriz.Checked = filtro.tipoPartidaMatriz;
                rbtnUbiPartidaHoriz.Checked = filtro.tipoPartidaHoriz;
                txtUbiNroPuerta.Text = filtro.nroPuerta.ToString();

                if (filtro.codCalle > 0 && filtro.nomCalle != "")
                {
                    using (var db = new DGHP_Entities())
                    {
                        var lstCalles = (List<ItemCalle>)ddlUbiCalle.DataSource;
                        int i = 0;
                        while ((lstCalles[i].Codigo_calle != filtro.codCalle || lstCalles[i].NombreOficial_calle != filtro.nomCalle) && i < lstCalles.Count)
                            i++;

                        ddlUbiCalle.SelectedIndex = i < lstCalles.Count ? i + 1 : 0; // Sumo 1 porque la lista tiene un elemento mas que el DataSource
                    }
                }

                txtUbiSeccion.Text = filtro.seccion;
                txtUbiManzana.Text = filtro.manzana;
                txtUbiParcela.Text = filtro.parcela;
                ddlBaja.SelectedIndex = filtro.baja_logica ? 1 : 0;
                if (filtro.tipoUbicacion > 0)
                {
                    CargarCombo_subTipoUbicacion(filtro.tipoUbicacion, true);
                    updPnlFiltroBuscar_ubi_especial.Update();
                }
                ddlUbiTipoUbicacion.SelectedValue = filtro.tipoUbicacion.ToString();
                ddlUbiSubTipoUbicacion.SelectedValue = filtro.subtipoUbicacion.ToString();

                if (filtro.tipoPartidaMatriz && filtro.nroPartida != "")
                    this.nro_partida_matriz = int.Parse(filtro.nroPartida);
                if (filtro.tipoPartidaHoriz && filtro.nroPartida != "")
                    this.nro_partida_horiz = int.Parse(filtro.nroPartida);
                this.cod_calle = filtro.codCalle.ToString();
                this.nro_puerta = filtro.nroPuerta;
                this.seccion = filtro.seccion != "" ? int.Parse(filtro.seccion) : 0;
                this.manzana = filtro.manzana;
                this.parcela = filtro.parcela;
                this.id_tipo_ubicacion = filtro.tipoUbicacion;
                this.id_sub_tipo_ubicacion = filtro.subtipoUbicacion;

                this.EjecutarScript(updPnlFiltroBuscar_ubi_dom, "switchear_buscar_ubicacion(" + filtro.tipoBusq.ToString() + ");");
                this.EjecutarScript(updPnlFiltroBuscar_ubi_especial, "switchear_buscar_ubicacion(" + filtro.tipoBusq.ToString() + ");");
                this.EjecutarScript(updPnlFiltroBuscar_ubi_partida, "switchear_buscar_ubicacion(" + filtro.tipoBusq.ToString() + ");");
                this.EjecutarScript(updPnlFiltroBuscar_ubi_smp, "switchear_buscar_ubicacion(" + filtro.tipoBusq.ToString() + ");");
                BuscarUbicacion();
                grdResultados.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updDatosHorizon, updDatosHorizon.GetType(), "init_Js_updDatosHorizon", "init_Js_updDatosHorizon();", true);
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_ubi_dom, updPnlFiltroBuscar_ubi_dom.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updPnlResultadoBuscar, updPnlResultadoBuscar.GetType(), "init_Js_updPnlResultadoBuscar", "init_Js_updPnlResultadoBuscar();", true);
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), " init_Js_updDatos", " init_Js_updDatos();", true);
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_ubi_especial, updPnlFiltroBuscar_ubi_especial.GetType(), "init_Js_updPnlFiltroBuscar_ubi_especial", "init_Js_updPnlFiltroBuscar_ubi_especial();", true);
            }

            CargarPermisos();
            if (!IsPostBack)
            {
                LoadData();
                CargarFiltros();
            }
            btnNuevaPartida.Visible = editar;
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                this.EjecutarScript(updPnlFiltroBuscar_ubi_dom, "finalizarCarga();");
                this.EjecutarScript(updPnlFiltroBuscar_ubi_especial, "finalizarCarga();");
                this.EjecutarScript(updPnlFiltroBuscar_ubi_partida, "finalizarCarga();");
                this.EjecutarScript(updPnlFiltroBuscar_ubi_smp, "finalizarCarga();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updPnlResultadoBuscar, "finalizarCarga();showfrmError();");
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
                                     Codigo_calle = cal.Codigo_calle
                                     //AlturaMin = cal.AlturaIzquierdaInicio_calle <= cal.AlturaDerechaInicio_calle ? cal.AlturaIzquierdaInicio_calle : cal.AlturaDerechaInicio_calle,
                                     //AlturaMax = cal.AlturaDerechaFin_calle >= cal.AlturaIzquierdaFin_calle ? cal.AlturaDerechaFin_calle : cal.AlturaIzquierdaFin_calle
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
            //public int? AlturaMin { get; set; }
            //public int? AlturaMax { get; set; }
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }

        public void LoadData()
        {
            try
            {
                CargarComboCalles();
                CargarCombo_tipoUbicacion(true);
                CargarCombo_subTipoUbicacion(-1, true);

                updPnlFiltroBuscar_ubi_partida.Update();
                updPnlFiltroBuscar_ubi_dom.Update();
                updPnlFiltroBuscar_ubi_smp.Update();
                updPnlFiltroBuscar_ubi_especial.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CargarCombo_tipoUbicacion(bool busqueda)
        {
            using (var db = new DGHP_Entities())
            {
                List<TiposDeUbicacion> lista = db.TiposDeUbicacion.Where(x => x.id_tipoubicacion > 0).ToList();

                TiposDeUbicacion tipo_ubi = new TiposDeUbicacion();
                tipo_ubi.id_tipoubicacion = 0;
                tipo_ubi.descripcion_tipoubicacion = (busqueda) ? "Seleccione" : "Parcela Común";
                lista.Insert(0, tipo_ubi);
                if (busqueda)
                {
                    ddlUbiTipoUbicacion.DataTextField = "descripcion_tipoubicacion";
                    ddlUbiTipoUbicacion.DataValueField = "id_tipoubicacion";

                    ddlUbiTipoUbicacion.DataSource = lista;
                    ddlUbiTipoUbicacion.DataBind();
                }
            }
        }

        private void CargarCombo_subTipoUbicacion(int id_tipoubicacion, bool busqueda)
        {
            using (var db = new DGHP_Entities())
            {
                List<SubTiposDeUbicacion> lista = db.SubTiposDeUbicacion.Where(x => x.id_tipoubicacion == id_tipoubicacion && x.habilitado).ToList();

                SubTiposDeUbicacion sub_tipo_ubi = new SubTiposDeUbicacion();
                sub_tipo_ubi.id_subtipoubicacion = 0;
                sub_tipo_ubi.descripcion_subtipoubicacion = (busqueda) ? "Seleccione" : "Ninguno";
                lista.Insert(0, sub_tipo_ubi);
                if (busqueda)
                {
                    ddlUbiSubTipoUbicacion.DataTextField = "descripcion_subtipoubicacion";
                    ddlUbiSubTipoUbicacion.DataValueField = "id_subtipoubicacion";
                    ddlUbiSubTipoUbicacion.DataSource = lista;
                    ddlUbiSubTipoUbicacion.DataBind();
                }
            }
        }

        protected void ddlUbiTipoUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id_tipoubicacion = int.Parse(ddlUbiTipoUbicacion.SelectedValue);

            CargarCombo_subTipoUbicacion(id_tipoubicacion, true);
            updPnlFiltroBuscar_ubi_especial.Update();
        }

        //protected void ddlUbiTipoUbicacionABM_SelectedIndexChanged(object sender, EventArgs e)
        //{            
        //    updPnlFiltroBuscar_ubi_especial.Update();
        //}

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

        #region buscar Partida

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            btnGuardar.Visible = true;
            btnHoriGuardar.Visible = true;

            btnNuevaBusqueda.Visible = true;
            btnHoriNuevaBusqueda.Visible = true;
        }

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            txtUbiNroPartida.Text = "";
            ddlUbiCalle.ClearSelection();
            txtUbiNroPuerta.Text = "";
            txtUbiSeccion.Text = "";
            txtUbiManzana.Text = "";
            txtUbiParcela.Text = "";
            nro_partida_matriz = 0;
            nro_partida_horiz = 0;
            cod_calle = "0";
            nro_puerta = 0;
            seccion = 0;
            manzana = "";
            parcela = "";
            id_tipo_ubicacion = -1;
            id_sub_tipo_ubicacion = -1;
            if (ddlUbiTipoUbicacion.Items.Count >= 0)
                ddlUbiTipoUbicacion.SelectedIndex = 0;
            if (ddlUbiSubTipoUbicacion.Items.Count >= 0)
                ddlUbiSubTipoUbicacion.SelectedIndex = 0;
            ddlBaja.SelectedIndex = 0;

            pnlResultadoBuscar.Visible = false;
            updPnlFiltroBuscar_ubi_partida.Update();
            updPnlFiltroBuscar_ubi_dom.Update();
            updPnlFiltroBuscar_ubi_smp.Update();
            updPnlFiltroBuscar_ubi_especial.Update();
            updPnlResultadoBuscar.Update();

            if (Session["BusqUbiFiltro"] != null)
                Session["BusqUbiFiltro"] = null;

            EjecutarScript(btn_BuscarPartida, "hideResultado();");
        }

        private void BuscarUbicacion()
        {
            filtrar = true;
            grdResultados.DataBind();
            updPnlResultadoBuscar.Update();
            pnlResultadoBuscar.Visible = true;
            GuardarFiltros();
            EjecutarScript(updPnlResultadoBuscar, "showResultado();");
        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Validar_BuscarPorUbicacion();
                BuscarUbicacion();
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "error al buscar Partidas buscar_Partidas-btnBuscar_OnClick");
                Enviar_Mensaje(ex.Message, "");
            }
        }

        public List<SGI.Model.Shared.ItemUbicacion> GetResultados(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
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
                var w = db.Ubicaciones.Where(x => x.id_ubicacion > 0);

                bool bajaLogica = false;
                bool.TryParse(ddlBaja.SelectedValue, out bajaLogica);
                w = w.Where(x => x.baja_logica == bajaLogica);

                // Partida Matriz
                if ((this.nro_partida_matriz > 0) && (rbtnUbiPartidaMatriz.Checked))
                {
                    filtroPHSubGrilla = bajaLogica;
                    w = w.Where(x => x.NroPartidaMatriz == this.nro_partida_matriz);
                }

                // Propiedad Horizontal
                bool bajaLogicaPH = false;
                bool.TryParse(ddlPHDadaDBaja.SelectedValue, out bajaLogicaPH);
                if ((this.nro_partida_horiz > 0) && (rbtnUbiPartidaHoriz.Checked))
                {
                    w = (from res in w
                         join ubiPartidas in db.Ubicaciones_PropiedadHorizontal on res.id_ubicacion equals ubiPartidas.id_ubicacion
                         where ubiPartidas.NroPartidaHorizontal == nro_partida_horiz && ubiPartidas.baja_logica == bajaLogicaPH
                         select res);

                    filtroPHSubGrilla = bajaLogicaPH;
                }

                //147921: JADHE 57682 - SGI - Error de resultado por busqueda de calle puerta
                // Si se busca por calle se busca la SMP de esa ubicacion_puerta y se filtra con eso
                
                // Codigo de calle y numero de puerta
                //int codigo_calle = 0;
                if (this.codigo_calle > 0 )
                {
                    //busca todas las ubicaciones para la puerta
                    var ub1 = db.Ubicaciones_Puertas.Where(x => x.codigo_calle == this.codigo_calle && x.NroPuerta_ubic == this.nro_puerta).Select(x => x.Ubicaciones).ToList();
   
                    var idUbicaciones = ub1.Select(x => x.id_ubicacion).ToList();
                    w = w.Where(x => idUbicaciones.Contains(x.id_ubicacion));
                }

                // SMP
                if (this.seccion > 0)
                    w = w.Where(x => x.Seccion == this.seccion);
                if (this.manzana.Trim().Length > 0)
                    w = w.Where(x => x.Manzana.Contains(this.manzana.Trim()));
                if (this.parcela.Trim().Length > 0)
                    w = w.Where(x => x.Parcela.Contains(this.parcela.Trim()));

                var qa = (from qs in w
                          join su in db.SubTiposDeUbicacion on qs.id_subtipoubicacion equals su.id_subtipoubicacion
                          join tu in db.TiposDeUbicacion on su.id_tipoubicacion equals tu.id_tipoubicacion
                          join ubiPuertas in db.Ubicaciones_Puertas on qs.id_ubicacion equals ubiPuertas.id_ubicacion into up
                          from ubiPuertas in up.DefaultIfEmpty()
                          join callesNombre in db.Calles on ubiPuertas != null ? ubiPuertas.codigo_calle : 0 equals callesNombre.Codigo_calle into cl
                          from callesNombre in cl.DefaultIfEmpty()
                          where (ubiPuertas != null ?
                          (ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaIzquierdaInicio_calle || ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaDerechaInicio_calle)
                            && (ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaIzquierdaFin_calle || ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaDerechaFin_calle) : true)
                            //&& (this.codigo_calle > 0 ? callesNombre.Codigo_calle == codigo_calle : true)
                            //&& (this.nro_puerta > 0 ? ubiPuertas.NroPuerta_ubic == this.nro_puerta : true)
                            && (this.id_sub_tipo_ubicacion >= 0 ? su.id_subtipoubicacion == this.id_sub_tipo_ubicacion : true)
                            && (this.id_tipo_ubicacion >= 0 ? tu.id_tipoubicacion == this.id_tipo_ubicacion : true)

                          select new SGI.Model.Shared.ItemUbicacion
                          {
                              id_ubicacion = qs.id_ubicacion,
                              //manzana = tu.descripcion_tipoubicacion == "Objeto territorial" ? "T" + qs.Manzana : qs.Manzana,
                              manzana = qs.Manzana,
                              seccion = qs.Seccion,
                              //parcela = tu.descripcion_tipoubicacion == "Objeto territorial" ? qs.Parcela + "t" : qs.Parcela,
                              parcela = qs.Parcela,
                              partida = qs.NroPartidaMatriz,
                              inhibida = qs.Inhibida_Observacion,
                              id_subtipoubicacion = qs.id_subtipoubicacion,
                              id_tipoubicacion = su.id_tipoubicacion,
                              direccion = tu.descripcion_tipoubicacion != "Objeto territorial" ?
                                    (callesNombre != null ? callesNombre.NombreOficial_calle + " " : "") + (ubiPuertas != null ? ubiPuertas.NroPuerta_ubic.ToString() : "")
                                    : (callesNombre != null ? callesNombre.NombreOficial_calle + " " : "") + (ubiPuertas != null ? ubiPuertas.NroPuerta_ubic.ToString() + "t" : ""),
                              id_estado_modif_ubi = (db.Ubicaciones_Historial_Cambios.Where(x => x.id_ubicacion.Equals(qs.id_ubicacion)).Count() != 0) ?
                                                     (db.Ubicaciones_Historial_Cambios.Where(x => x.id_ubicacion.Equals(qs.id_ubicacion)).OrderByDescending(o => o.id_ubihistcam).Select(r => r.id_estado_modif).FirstOrDefault()) : -1,
                              baja_logica = qs.baja_logica == false ? "No" : "Si",
                              codigo_calle = callesNombre != null ? callesNombre.Codigo_calle : 0,
                              nombre_calle = callesNombre != null ? callesNombre.NombreOficial_calle : "",
                              nro_puerta = ubiPuertas != null ? ubiPuertas.NroPuerta_ubic : 0
                          }
                       ).Distinct();

                var u = (from ubi in qa.AsEnumerable()
                         group ubi by new
                         {
                             ubi.id_ubicacion,
                             ubi.manzana,
                             ubi.seccion,
                             ubi.parcela,
                             ubi.partida,
                             ubi.inhibida,
                             ubi.id_subtipoubicacion,
                             ubi.id_tipoubicacion,
                             ubi.id_estado_modif_ubi,
                             ubi.baja_logica
                         } into ubis

                         select new SGI.Model.Shared.ItemUbicacion
                         {
                             id_ubicacion = ubis.Key.id_ubicacion,
                             manzana = ubis.Key.manzana,
                             seccion = ubis.Key.seccion,
                             parcela = ubis.Key.parcela,
                             partida = ubis.Key.partida,
                             inhibida = ubis.Key.inhibida,
                             id_subtipoubicacion = ubis.Key.id_subtipoubicacion,
                             id_tipoubicacion = ubis.Key.id_tipoubicacion,
                             id_estado_modif_ubi = ubis.Key.id_estado_modif_ubi,
                             baja_logica = ubis.Key.baja_logica,
                             direccion = string.Join(" / ", ubis.Select(x => x.direccion))
                         }
                      );

                totalRowCount = u.Count();
                var resultados = u.OrderBy(o => o.direccion).Skip(startRowIndex).Take(maximumRows).ToList();

                pnlCantidadRegistros.Visible = (resultados.Count > 0);
                lblCantidadRegistros.Text = totalRowCount.ToString();

                return resultados;
            }
        }

        private bool editable = true;
        //private void CargarDatos(int id_ubi, int idParHori, int? id_Solic)
        //{
        //    using (var db = new DGHP_Entities())
        //    {
        //        editable = true;
        //        var w = (from ubicacion in db.Ubicaciones_Historial_Cambios
        //                 where ubicacion.id_ubihistcam == id_Solic
        //                 select new
        //                 {
        //                     manzana = ubicacion.Manzana,
        //                     seccion = ubicacion.Seccion,
        //                     parcela = ubicacion.Parcela,
        //                     nroPartida = ubicacion.NroPartidaMatriz,
        //                     id_ubicacion = ubicacion.id_ubicacion,
        //                     subtipo = ubicacion.id_subtipoubicacion,
        //                     tipoSoli = ubicacion.tipo_solicitud,
        //                     zonaPla = ubicacion.id_zonaplaneamiento,
        //                     obsZ = ubicacion.Observaciones,
        //                     obsEst = ubicacion.observaciones_ubihistcam,
        //                     id_comisaria = ubicacion.id_comisaria,
        //                     id_barrio = ubicacion.id_barrio,
        //                     id_estado = ubicacion.id_estado_modif,
        //                     chbEntidad = ubicacion.EsEntidadGubernamental,
        //                     chbEdificio = ubicacion.EsUbicacionProtegida,
        //                     DadodeBaja = ubicacion.baja_logica == null ? false : ubicacion.baja_logica,
        //                 }
        //                  ).FirstOrDefault();

        //        if (w != null)
        //        {
        //            if ((w.tipoSoli == 2) || (!this.editar) || (this.estadoEnProceso != w.id_estado) || (this.estadoNueva == w.id_estado))
        //                editable = false;

        //            CargarCombo_tipoUbicacion(false);

        //            if (w.subtipo != null)
        //            {
        //                var tipo = (from st in db.SubTiposDeUbicacion
        //                            join tip in db.TiposDeUbicacion on st.id_tipoubicacion equals tip.id_tipoubicacion
        //                            where st.id_subtipoubicacion == w.subtipo
        //                            select new
        //                            {
        //                                idTipoUbi = tip.id_tipoubicacion
        //                            }).First();

        //                CargarCombo_subTipoUbicacion(tipo.idTipoUbi, false);
        //            }

        //            var ff = (from ubiZonas in db.Ubicaciones_ZonasComplementarias_Historial_Cambios
        //                      join zonaW in db.Zonas_Planeamiento on ubiZonas.id_zonaplaneamiento equals zonaW.id_zonaplaneamiento
        //                      where (id_ubicacion == ubiZonas.id_ubicacion && ubiZonas.id_ubihistcam == id_Solic)
        //                      select new SGI.Model.Shared.ItemZonasComp
        //                      {
        //                          tipoUbi = ubiZonas.tipo_ubicacion,

        //                          id_zonaplaneamiento = ubiZonas.id_zonaplaneamiento
        //                      }).Distinct();

        //            var lstUbicaciones = (from ubiPuertas in db.Ubicaciones_Puertas_Historial_Cambios
        //                                  join callesNombre in db.Calles on ubiPuertas.codigo_calle equals callesNombre.Codigo_calle
        //                                  where id_Solic == ubiPuertas.id_ubihistcam
        //                                       && (ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaDerechaInicio_calle || ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaIzquierdaInicio_calle)
        //                                       && (ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaDerechaFin_calle || ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaIzquierdaFin_calle)

        //                                  select new
        //                                  {
        //                                      calles = callesNombre.NombreOficial_calle,
        //                                      nroPuerta = ubiPuertas.NroPuerta_ubic

        //                                  }).Distinct().ToList();
        //        }
        //    }
        //}

        private string CargarTipoSol(int tipoS, bool matriz)
        {
            switch (tipoS)
            {
                case -1:
                    return "";
                case 0:

                    return (matriz) ? "Alta de Partida Matriz" : "Alta de Partida Horizontal";

                case 1:
                    return (matriz) ? "Modificación de Partida Matriz" : "Modificación de Partida Horizontal";

                case 2:
                    return (matriz) ? "Baja de Partida Matriz" : "Baja de Partida Horizontal";
            }
            return "";
        }

        //private string GetNombreEstado(int tipoEstado)
        //{
        //    switch (tipoEstado)
        //    {
        //        case 0:
        //            return "En Proceso";
        //        case 1:
        //            return "Anulada";
        //        case 2:
        //            return "Confirmada";
        //        case 3:
        //            return "Aprobada";
        //        case 4:
        //            return "Rechazada";
        //    }

        //    return "";
        //}

        //private bool NumeroPuertaValido(int idCalle, int nroPuertaUbi)
        //{
        //    using (var db = new DGHP_Entities())
        //    {
        //        return (from ca in db.Calles select ca)
        //            .Where(x => (x.id_calle == idCalle))
        //            .Where(x => x.AlturaIzquierdaInicio_calle <= nroPuertaUbi || x.AlturaDerechaInicio_calle <= nroPuertaUbi)
        //            .Where(x => x.AlturaDerechaFin_calle >= nroPuertaUbi || x.AlturaIzquierdaFin_calle >= nroPuertaUbi).Any();
        //    }
        //}

        #endregion

        #region paginado grilla

        //Grilla resultados
        protected void grdResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdResultados.PageIndex = e.NewPageIndex;
                filtrar = true;
                Validar();
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
                Validar();
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
                Validar();
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
                Validar();
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
                LinkButton lnkHorizontales = (LinkButton)e.Row.FindControl("lnkHorizontales");
                GridView grdHH = (GridView)e.Row.FindControl("grdResultadosh");
                int id_ubicacion = int.Parse(lnkHorizontales.CommandArgument);
                List<SGI.Model.Shared.ItemPartidasHorizontales> resu = new List<Shared.ItemPartidasHorizontales>();

                var elements = new List<SGI.Model.Shared.ItemPartidasHorizontales>();

                using (var db = new DGHP_Entities())
                {
                    elements = (from ubiPartidas in db.Ubicaciones_PropiedadHorizontal
                                join ub in db.Ubicaciones on ubiPartidas.id_ubicacion equals ub.id_ubicacion
                                where ubiPartidas.id_ubicacion == id_ubicacion //&& ubiPartidas.baja_logica == filtroPHSubGrilla
                                select new SGI.Model.Shared.ItemPartidasHorizontales
                                {
                                    partidaHor = ubiPartidas.NroPartidaHorizontal,
                                    pisoHor = ubiPartidas.Piso,
                                    deptoHor = ubiPartidas.Depto,
                                    id_propiedadhorizontal = ubiPartidas.id_propiedadhorizontal,
                                    id_estado_modif_ubi_h = -1,
                                    id_ubicacion = ubiPartidas.id_ubicacion,
                                    baja_logica = ubiPartidas.baja_logica == false ? "No" : "Si",
                                }).ToList();

                    resu = elements.OrderBy(x => x.partidaHor).ToList();
                    if (resu.Count() > 0)
                    {
                        foreach (var row in resu)
                        {

                            if (row.partidaHor != null)
                            {
                                var item = (db.Ubicaciones_PropiedadHorizontal_Historial_Cambios.Where(x => x.NroPartidaHorizontal == row.partidaHor).Count() != 0) ?
                                                               (db.Ubicaciones_PropiedadHorizontal_Historial_Cambios.Where(x => x.NroPartidaHorizontal == row.partidaHor).OrderByDescending(o => o.id_phhistcam).Select(r => r.id_estado_modif).FirstOrDefault()) : -1;

                                row.id_estado_modif_ubi_h = item;
                            }
                        }
                    }
                    else
                    {
                        //Eso hace que si no posee partidas horizontales, se oculte el link
                        lnkHorizontales.Visible = false;
                    }
                    grdHH.DataSource = resu;
                    grdHH.DataBind();
                }

                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                btnEliminar.Visible = editar;
                LinkButton btnEditar = (LinkButton)e.Row.FindControl("btnEditar");
                btnEditar.Visible = editar;

                LinkButton btnVer = (LinkButton)e.Row.FindControl("btnVer");
                btnVer.Visible = visualizar;

                Color colorFila = Color.White;
                Color colorBorde = Color.White;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    int.TryParse(DataBinder.Eval(e.Row.DataItem, "id_ubicacion").ToString(), out id_ubicacion);
                    int id_estado_modif = -1;

                    if (int.TryParse(DataBinder.Eval(e.Row.DataItem, "id_estado_modif_ubi").ToString(), out id_estado_modif))
                    {
                        if (id_estado_modif != 0 && id_estado_modif != 2)
                            btnEliminar.Visible = editar;
                    }
                }
            }
        }

        protected void grdResultadosH_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnEliminarHor = (LinkButton)e.Row.FindControl("btnEliminarHor");
                LinkButton btnEditarHor = (LinkButton)e.Row.FindControl("btnEditarHor");
                btnEditarHor.Visible = editar;
                LinkButton btnVerHor = (LinkButton)e.Row.FindControl("btnVerHor");
                btnVerHor.Visible = visualizar;

                int id_estado_modif = -1;
                if (int.TryParse(DataBinder.Eval(e.Row.DataItem, "id_estado_modif_ubi_h").ToString(), out id_estado_modif))
                {
                    if (id_estado_modif != 0 && id_estado_modif != 2)
                        btnEliminarHor.Visible = editar;
                }

                var bajalog = e.Row.Cells[4].Text.ToString();
                if (bajalog == "Si")
                {
                    btnEliminarHor.Visible = false;
                }
            }
        }

        protected void grdResultados_DataBound(object sender, EventArgs e)
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

        #endregion

        private void Validar()
        {
            Validar_BuscarPorUbicacion();
        }

        private void Validar_BuscarPorUbicacion()
        {
            this.nro_partida_matriz = 0;
            this.nro_partida_horiz = 0;
            this.cod_calle = "0";
            this.nro_puerta = 0;
            this.seccion = 0;
            this.manzana = "";
            this.parcela = "";
            this.id_tipo_ubicacion = -1;
            this.id_sub_tipo_ubicacion = -1;
            int idAux;

            //filtro por partida
            if (!string.IsNullOrEmpty(txtUbiNroPartida.Text))
            {
                if (rbtnUbiPartidaMatriz.Checked)
                {
                    int.TryParse(txtUbiNroPartida.Text, out idAux);
                    this.nro_partida_matriz = idAux;
                }
                else if (rbtnUbiPartidaHoriz.Checked)
                {
                    int.TryParse(txtUbiNroPartida.Text, out idAux);
                    this.nro_partida_horiz = idAux;
                }
                else
                {
                    throw new Exception("Debe indicar si número ingresado corresponde a partida matriz o a partida horizontal.");
                }
            }

            //filtro por domicilio
            if (!string.IsNullOrEmpty(txtUbiNroPuerta.Text) && string.IsNullOrEmpty(ddlUbiCalle.SelectedItem.Text))
            {
                if (txtUbiNroPuerta.Text.Trim() != "0")
                {
                    throw new Exception("Cuando especifica el número de puerta debe ingresar la calle.");
                }
            }

            this.cod_calle = ddlUbiCalle.SelectedValue;

            int.TryParse(ddlUbiCalle.SelectedValue, out idAux);
            this.codigo_calle = idAux;

            int.TryParse(txtUbiNroPuerta.Text.Trim(), out idAux);
            this.nro_puerta = idAux;

            //filtro por smp
            int.TryParse(txtUbiSeccion.Text, out idAux);
            this.seccion = idAux;

            this.manzana = txtUbiManzana.Text.Trim();

            this.parcela = txtUbiParcela.Text.Trim();

            //filtro por tipo subtipo
            int.TryParse(ddlUbiTipoUbicacion.SelectedItem.Value, out idAux);
            this.id_tipo_ubicacion = idAux > 0 ? idAux : -1;

            int.TryParse(ddlUbiSubTipoUbicacion.SelectedItem.Value, out idAux);
            this.id_sub_tipo_ubicacion = idAux > 0 ? idAux : -1;
        }

        protected void grdUbicaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //permisos
                LinkButton btnEliminarUbicacion = (LinkButton)e.Row.FindControl("btnEliminarUbicacion");
                btnEliminarUbicacion.Visible = this.editable;
            }
        }

        private bool RecuperarFiltro()
        {
            if (ViewState["filtro"] == null)
                return false;
            return true;
        }

        private void LimpiarCampos(ControlCollection Panel)
        {
            //hid_id_ubicacion.Value = "";
            foreach (Control ctl in Panel)
            {
                if (ctl.Controls.Count > 0)
                    LimpiarCampos(ctl.Controls);

                if (ctl is TextBox)
                {
                    TextBox txt = (TextBox)ctl;
                    txt.Text = "";
                }

                if (ctl is DropDownList)
                {
                    DropDownList ddl = (DropDownList)ctl;
                    ddl.ClearSelection();
                }

                if (ctl is CheckBox)
                {
                    CheckBox chk = (CheckBox)ctl;
                    chk.Checked = false;
                }
            }

            CargarCombo_tipoUbicacion(false);
            CargarCombo_subTipoUbicacion(-1, false);

            grdHistorialHori.DataSource = new DataTable();
            grdHistorialHori.DataBind();
        }

        private SGI.Model.Shared.ItemPartidasAux getPartidaMatriz(int idUbi)
        {
            using (var db = new DGHP_Entities())
            {
                var w = (from ubicacion in db.Ubicaciones
                         where ubicacion.id_ubicacion == idUbi
                         select new SGI.Model.Shared.ItemPartidasAux
                         {
                             manzana = ubicacion.Manzana,
                             seccion = ubicacion.Seccion,
                             parcela = ubicacion.Parcela,
                             partida = ubicacion.NroPartidaMatriz,
                             id_ubicacion = ubicacion.id_ubicacion,
                             inhibida = ubicacion.Inhibida_Observacion,
                             id_subtipoubicacion = ubicacion.id_subtipoubicacion
                         }).FirstOrDefault();
                w.direccion = "";

                string[] arrUbicaciones = new string[1];
                if (w != null && w.id_ubicacion > 0)
                {
                    arrUbicaciones[0] = w.id_ubicacion.ToString();
                    List<ItemDireccion> lstDirecciones = Shared.getDirecciones(arrUbicaciones);

                    //------------------------------------------------------------------------
                    //Rellena la clase a devolver con los datos que faltaban (Direccion,tarea)
                    //------------------------------------------------------------------------

                    var itemDireccion = lstDirecciones.FirstOrDefault(x => x.id_ubicacion == w.id_ubicacion);
                    if (itemDireccion != null)
                        w.direccion = (string.IsNullOrEmpty(itemDireccion.direccion) ? "" : itemDireccion.direccion);
                }
                return w;
            }
        }

        protected void btnNuevaPartida_Click(object sender, EventArgs e)
        {
            try
            {
                this.EjecutarScript(btn_BuscarPartida, "showfrmCargarUbicacion();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(btn_BuscarPartida, "showfrmError();");
            }
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        #region < Partida Horizontal DLF >

        protected void btnEditar_ClickHor(object sender, EventArgs e)
        {
            try
            {
                LimpiarCampos(this.pnlDatosHorizon.Controls);

                LinkButton btnEditarHor = (LinkButton)sender;
                LinkButton btnEliminarHor = (LinkButton)sender;
                int id_ubi = 0;
                string[] arg = new string[3];
                arg = btnEditarHor.CommandArgument.ToString().Split(';');
                id_ubi = int.Parse(arg[0]);
                int id_parH = int.Parse(arg[2]);

                int bajaEditar = 1;
                if (arg[1].Equals("baja"))
                {//es baja
                    arg = btnEliminarHor.CommandArgument.ToString().Split(';');
                    btnEditarHor = null;
                    bajaEditar = 2;
                }

                int idSol = CargarSolicitudPartidaHorizontal(0, bajaEditar, id_ubi, id_parH);

                this.CargarDatosHorizontal(id_ubi, id_parH, idSol);

                updDatosHorizon.Update();
                EjecutarScript(updPnlResultadoBuscar, "showDatosHorizon();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                EjecutarScript(updPnlResultadoBuscar, "showfrmError();");
            }
        }

        private void CargarDatosHorizontalOriginales(int id_ubi, int idParHori, int? id_Solic)
        {
            DGHP_Entities db = new DGHP_Entities();
            editable = false;

            var w = (from ubiPartidas in db.Ubicaciones_PropiedadHorizontal
                     where ubiPartidas.id_propiedadhorizontal == idParHori
                     select new
                     {
                         partidaHor = ubiPartidas.NroPartidaHorizontal,
                         pisoHor = ubiPartidas.Piso,
                         deptoHor = ubiPartidas.Depto,
                         id_propiedadhorizontal = ubiPartidas.id_propiedadhorizontal,
                         id_ubicacion = ubiPartidas.id_ubicacion,
                         chbEntidad = ubiPartidas.EsEntidadGubernamental,
                         obsZ = ubiPartidas.Observaciones
                     }).FirstOrDefault();

            if (w != null)
            {
                SGI.Model.Shared.ItemPartidasAux partidaMatriz = this.getPartidaMatriz(w.id_ubicacion);
                txtHoriNroPartidaM.Text = (partidaMatriz.partida != null) ? partidaMatriz.partida.ToString() : "";
                txtHoriDireccion.Text = partidaMatriz.direccion;
                txtHoriManzana.Text = partidaMatriz.manzana;
                txtHoriSeccion.Text = (partidaMatriz.seccion != null) ? partidaMatriz.seccion.ToString() : "";
                txtHoriParcela.Text = w.partidaHor.ToString();
                txtHoriPiso.Text = w.pisoHor;
                txtHoriPiso.Enabled = editable;
                txtHoriDepto.Text = w.deptoHor;
                txtHoriDepto.Enabled = editable;
                txtHoriNroSol.Text = string.Empty;
                chbHoriEntidadGubernamental.Checked = w.chbEntidad;
                chbHoriEntidadGubernamental.Enabled = editable;
                txtHoriNroPartidaHor.Text = (w.partidaHor != null) ? w.partidaHor.ToString() : "";
                txtHoriNroPartidaHor.Enabled = editable;
                txtHoriObservaciones.Text = w.obsZ;
                txtHoriObservaciones.Enabled = editable;

                divObsH.Visible = false;
                txtHoriObservacionesEst.Text = string.Empty;
                txtHoriObservacionesEst.Enabled = editable;
                btnHoriGuardar.Visible = editable;
                btnHoriNuevaBusqueda.Visible = editable;

                var lstHistorial = (from hist in db.Ubicaciones_PropiedadHorizontal_Historial_Cambios_UsuariosIntervinientes
                                    join usu in db.aspnet_Users on hist.Userid equals usu.UserId
                                    join usuAdi in db.Usuario on usu.UserId equals usuAdi.UserId into zr
                                    from fd in zr.DefaultIfEmpty()
                                    where id_Solic == hist.id_phhistcam
                                    select new
                                    {
                                        UserNameHori = usu.UserName,
                                        ApenomHori = fd.Apellido + " " + fd.Nombre,
                                        LastUpdateDateHori = hist.LastUpdateDate

                                    }
                                  ).Distinct().ToList();

                grdHistorialHori.DataSource = lstHistorial;
                grdHistorialHori.DataBind();
            }

            db.Dispose();
        }

        protected void btnVer_ClickHor(object sender, EventArgs e)
        {
            try
            {
                LimpiarCampos(this.pnlDatosHorizon.Controls);

                txtHoriTipoSolicitud.Text = string.Empty;
                LinkButton btnVerHor = (LinkButton)sender;
                int id_ubi = 0;
                string[] arg = new string[3];
                arg = btnVerHor.CommandArgument.ToString().Split(';');
                id_ubi = int.Parse(arg[0]);
                int id_parH = int.Parse(arg[1]);

                this.CargarDatosHorizontalOriginales(id_ubi, id_parH, 0);
                updDatosHorizon.Update();
                EjecutarScript(updPnlResultadoBuscar, "showDatosHorizon();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                EjecutarScript(updPnlResultadoBuscar, "showfrmError();");
            }
        }

        private bool numeroPuertaValido(int idCalle, int nroPuertaUbi)
        {
            DGHP_Entities db = new DGHP_Entities();

            return (from ca in db.Calles select ca)
                .Where(x => (x.id_calle == idCalle))
                .Where(x => x.AlturaIzquierdaInicio_calle <= nroPuertaUbi || x.AlturaDerechaInicio_calle <= nroPuertaUbi)
                .Where(x => x.AlturaDerechaFin_calle >= nroPuertaUbi || x.AlturaIzquierdaFin_calle >= nroPuertaUbi).Any();
        }
        protected void btnGuardarUbicacion_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dtUbicacionesCargadas();
                int nroPuertaUbi = 0;
                int idCalle = 0;//es necesario ?
                string txtCalle = (ddlCalle.SelectedItem.Text.Split('[')[0]).Trim();

                int.TryParse(txtNroPuerta.Text.Trim(), out nroPuertaUbi);
                int.TryParse(ddlCalle.SelectedValue, out idCalle);//es necesario?
                if (numeroPuertaValido(idCalle, nroPuertaUbi))
                {
                    DataRow datarw;
                    datarw = dt.NewRow();

                    //datarw[0] = ddlCalle.SelectedItem;
                    datarw[0] = txtCalle;
                    datarw[1] = nroPuertaUbi;

                    dt.Rows.Add(datarw);

                    grdUbicaciones.DataSource = dt;
                    grdUbicaciones.DataBind();

                    this.EjecutarScript(updUbicaciones, "hidefrmAgregarUbicacion();");
                }
                else
                {
                    txtNroPuerta.Text = string.Empty;
                    txtNroPuerta.Focus();
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updUbicaciones, "hidefrmAgregarUbicacion();");
            }

        }
        protected void btnAgregarUbicacion_Click(object sender, EventArgs e)
        {
            try
            {
                txtNroPuerta.Text = string.Empty;

                this.EjecutarScript(updDatos, "showfrmAgregarUbicacion();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updDatos, "showfrmError();");
            }
        }
        private DataTable dtUbicacionesCargadas()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("calles", typeof(string));
            dt.Columns.Add("nroPuerta", typeof(int));

            foreach (GridViewRow row in grdUbicaciones.Rows)
            {
                DataRow datarw;
                datarw = dt.NewRow();
                datarw[0] = HttpUtility.HtmlDecode(row.Cells[0].Text);
                datarw[1] = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[1].Text));

                dt.Rows.Add(datarw);
            }

            return dt;
        }
        protected void btnEliminarUbicacion_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEliminarUbicacion = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnEliminarUbicacion.Parent.Parent;
                DataTable dt = dtUbicacionesCargadas();

                dt.Rows.RemoveAt(row.RowIndex);


                grdUbicaciones.DataSource = dt;
                grdUbicaciones.DataBind();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
            }
        }
        protected void ddlbiTipoUbicacionABM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id_tipoubicacion = int.Parse(ddlbiTipoUbicacionABM.SelectedValue);
                hid_id_tipo_ubicacion.Value = id_tipoubicacion.ToString();

                CargarCombo_subTipoUbicacion(id_tipoubicacion, false);
            }
            catch (Exception)
            {

            }

            updPnlFiltroBuscar_ubi_especial.Update();
        }

        

        protected void btnHoriGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnHoriGuardar = (LinkButton)sender;
                int id_estado_modif = 3; //aprobada
                int id_ubiSol = -1;
                int.TryParse(this.txtHoriNroSol.Text.Trim(), out id_ubiSol);

                //Nro Partida Horizontal
                int? UbiPartidaHorValue = null;
                int UbiPartidaHor = 0;
                int.TryParse(txtHoriNroPartidaHor.Text.Trim(), out UbiPartidaHor);

                if (UbiPartidaHor > 0)
                    UbiPartidaHorValue = UbiPartidaHor;

                Guid userid = Functions.GetUserId();

                DGHP_Entities db = new DGHP_Entities();

                db.Ubicaciones_PropiedadHorizontal_SolicitudCambio_Actualizar(id_ubiSol, 0, UbiPartidaHorValue, this.txtHoriPiso.Text, this.txtHoriDepto.Text, this.txtHoriDepto.Text, this.txtHoriObservaciones.Text, this.chbHoriEntidadGubernamental.Checked, id_estado_modif, this.txtHoriObservacionesEst.Text, userid);

                updDatos.Update();
                if (this.estadoAprobada == id_estado_modif)
                {
                    db.Ubicaciones_PropiedadHorizontal_SolicitudCambio_ActualizarPropiedadHorizontal(id_ubiSol, userid);
                }

                Validar_BuscarPorUbicacion();
                filtrar = true;
                grdResultados.DataBind();
                updPnlResultadoBuscar.Update();

                LimpiarCampos(this.pnlDatos.Controls);

                EjecutarScript(updHoriBotonesGuardar, "showBusqueda();");
                db.Dispose();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updHoriBotonesGuardar, "showfrmError();");
            }
        }

        private string cargarTipoSol(int tipoS, bool matriz)
        {
            switch (tipoS)
            {
                case -1:
                    return string.Empty;
                case 0:
                    return (matriz) ? "Alta de Partida Matriz" : "Alta de Partida Horizontal";
                case 1:
                    return (matriz) ? "Modificación de Partida Matriz" : "Modificación de Partida Horizontal";
                case 2:
                    return (matriz) ? "Baja de Partida Matriz" : "Baja de Partida Horizontal";
            }
            return string.Empty;
        }

        private void CargarDatosHorizontal(int id_ubi, int idParHori, int? id_Solic)
        {
            DGHP_Entities db = new DGHP_Entities();
            editable = true;

            var w = (from ubiPartidas in db.Ubicaciones_PropiedadHorizontal_Historial_Cambios
                     where ubiPartidas.id_phhistcam == id_Solic
                     select new
                     {
                         partidaHor = ubiPartidas.NroPartidaHorizontal,
                         pisoHor = ubiPartidas.Piso,
                         deptoHor = ubiPartidas.Depto,
                         id_propiedadhorizontal = ubiPartidas.id_propiedadhorizontal,
                         id_estado_modif_ubi_h = ubiPartidas.id_estado_modif,
                         id_ubicacion = ubiPartidas.id_ubicacion,
                         tipoSoli = ubiPartidas.tipo_solicitud,
                         chbEntidad = ubiPartidas.EsEntidadGubernamental,
                         obsEs = ubiPartidas.observaciones_phhistcam,
                         obsZ = ubiPartidas.Observaciones

                     }).FirstOrDefault();

            if (w != null)
            {
                SGI.Model.Shared.ItemPartidasAux partidaMatriz = this.getPartidaMatriz(w.id_ubicacion);

                if ((w.tipoSoli == 2) ||
                        (!this.editar) ||
                        (this.estadoEnProceso != w.id_estado_modif_ubi_h) ||
                         (this.estadoNueva == w.id_estado_modif_ubi_h))
                    editable = false;
                txtHoriTipoSolicitud.Text = cargarTipoSol(w.tipoSoli, false);
                txtHoriNroPartidaM.Text = (partidaMatriz.partida != null) ? partidaMatriz.partida.ToString() : "";
                txtHoriDireccion.Text = partidaMatriz.direccion;
                txtHoriManzana.Text = partidaMatriz.manzana;
                txtHoriSeccion.Text = (partidaMatriz.seccion != null) ? partidaMatriz.seccion.ToString() : "";
                txtHoriParcela.Text = partidaMatriz.parcela;
                txtHoriPiso.Text = w.pisoHor;
                txtHoriPiso.Enabled = editable;
                txtHoriDepto.Text = w.deptoHor;
                txtHoriDepto.Enabled = editable;
                txtHoriNroSol.Text = id_Solic.ToString();
                chbHoriEntidadGubernamental.Checked = w.chbEntidad;
                chbHoriEntidadGubernamental.Enabled = editable;
                txtHoriNroPartidaHor.Text = (w.partidaHor != null) ? w.partidaHor.ToString() : "";
                txtHoriNroPartidaHor.Enabled = editable;
                txtHoriObservaciones.Text = w.obsZ;
                txtHoriObservaciones.Enabled = true;

                txtHoriObservacionesEst.Text = w.obsEs;
                txtHoriObservacionesEst.Enabled = editable;
                ObserEditHP.Visible = false;

                var lstHistorial = (from hist in db.Ubicaciones_PropiedadHorizontal_Historial_Cambios_UsuariosIntervinientes
                                    join usu in db.aspnet_Users on hist.Userid equals usu.UserId
                                    join usuAdi in db.Usuario on usu.UserId equals usuAdi.UserId into zr
                                    from fd in zr.DefaultIfEmpty()
                                    where id_Solic == hist.id_phhistcam
                                    select new
                                    {
                                        UserNameHori = usu.UserName,
                                        ApenomHori = fd.Apellido + " " + fd.Nombre,
                                        LastUpdateDateHori = hist.LastUpdateDate

                                    }
                                  ).Distinct().ToList();

                grdHistorialHori.DataSource = lstHistorial;
                grdHistorialHori.DataBind();
            }

            db.Dispose();
        }


        private int CargarSolicitudPartidaHorizontal(int pIdSolicitud, int pTipoSolicitud, int pIdUbicacion, int pIdPropiedadHorinzontal)
        {
            using (var db = new DGHP_Entities())
            {
                Guid userid = Functions.GetUserId();

                if (pIdSolicitud == 0)//si es alta
                {
                    try
                    {
                        int? idSol = db.SGI_Ubicaciones_PropiedadHorizontal_GenerarSolicitudCambio(pIdUbicacion, pIdPropiedadHorinzontal, pTipoSolicitud, userid).FirstOrDefault();

                        return idSol.Value;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Se ha producido un error generando la solicitud de cambio de propiedad horizontal.", ex);
                    }
                }
                return -1;
            }
        }

        protected void btnNuevaPartidaHori_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarCampos(this.pnlDatosHorizon.Controls);

                txtHoriTipoSolicitud.Text = CargarTipoSol(0, false);
                LinkButton btnNuevaPartidaHori = (LinkButton)sender;
                int id_ubi = 0;
                string[] arg = new string[1];
                arg = btnNuevaPartidaHori.CommandArgument.ToString().Split(';');
                id_ubi = int.Parse(arg[0]);

                int idSol = CargarSolicitudPartidaHorizontal(0, 0, id_ubi, 0);
                this.CargarDatosHorizontal(id_ubi, 0, idSol);

                updDatosHorizon.Update();
                EjecutarScript(updPnlResultadoBuscar, "showDatosHorizon();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                EjecutarScript(updPnlResultadoBuscar, "showfrmError();");
            }
        }

        #endregion



        protected void btnVer_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string id_ubi = btn.CommandArgument.ToString();

            Response.Redirect("~/ABM/Ubicaciones/verUbicacion.aspx?id=" + id_ubi);
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string id_ubi = btn.CommandArgument.ToString();

            Response.Redirect("~/ABM/Ubicaciones/EditarUbicacion.aspx?id=" + id_ubi);
        }

        /// <summary>
        /// Obtiene los datos de la ubicaciones
        /// </summary>
        /// <param name="id_ubicacion">Id de la Ubicación consultada</param>
        /// <returns></returns>
        public void LlenarGridUbiActual(int id_ubicacion)
        {
            using (var ctx = new DGHP_Entities())
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("PartidaMatriz", typeof(string));
                dt.Columns.Add("Seccion", typeof(string));
                dt.Columns.Add("Manzana", typeof(string));
                dt.Columns.Add("Parcela", typeof(string));
                dt.Columns.Add("FechaAlta", typeof(string));
                dt.Columns.Add("Creador", typeof(string));
                dt.Columns.Add("FechaUltimaActualizacion", typeof(string));
                dt.Columns.Add("Actualizador", typeof(string));
                dt.Columns.Add("baja_logica", typeof(string));

                DataRow datarw;
                datarw = dt.NewRow();

                //var u = (from ubi in ctx.Ubicaciones
                //         join usu1 in ctx.aspnet_Users on ubi.CreateUser equals usu1.UserId into u1
                //         from usu1 in u1.DefaultIfEmpty()
                //         join usu2 in ctx.aspnet_Users on ubi.UpdateUser equals usu2.UserId into u2
                //         from usu2 in u2.DefaultIfEmpty()
                //         where ubi.id_ubicacion == id_ubicacion
                //         select new
                //         {
                //             id_ubicacion = ubi.id_ubicacion.ToString(),
                //             NroPartidaMatriz = ubi.NroPartidaMatriz.ToString(),
                //             Seccion = ubi.Seccion.ToString(),
                //             Manzana = ubi.Manzana,
                //             Parcela = ubi.Parcela,
                //             FechaAlta = ubi.CreateDate != null ? ubi.CreateDate.ToString() : "",
                //             Creador = usu1 != null ? usu1.UserName : "",
                //             FechaUltimaActualizacion = ubi.UpdateDate != null ? ubi.UpdateDate.ToString() : "",
                //             Actualizador = usu2 != null ? usu2.UserName : "",
                //             baja_logica = ubi.baja_logica == false ? "No" : "Si",
                //         }).SingleOrDefault();
                var u = ctx.Ubicaciones.Where(a => a.id_ubicacion == id_ubicacion).SingleOrDefault();
                string creador = GetUserName(u.CreateUser);
                string actualizador = GetUserName(u.UpdateUser);
                if (u != null)
                {
                    datarw[0] = HttpUtility.HtmlDecode(u.id_ubicacion.ToString());
                    datarw[1] = HttpUtility.HtmlDecode(u.NroPartidaMatriz != null ? u.NroPartidaMatriz.ToString() : "");
                    datarw[2] = HttpUtility.HtmlDecode(u.Seccion != null ? u.Seccion.ToString() : "");
                    datarw[3] = HttpUtility.HtmlDecode(u.Manzana);
                    datarw[4] = HttpUtility.HtmlDecode(u.Parcela);
                    datarw[5] = HttpUtility.HtmlDecode(u.CreateDate != null ? u.CreateDate.ToString() : "");
                    datarw[6] = HttpUtility.HtmlDecode(creador);
                    datarw[7] = HttpUtility.HtmlDecode(u.UpdateDate != null ? u.UpdateDate.ToString() : "");
                    datarw[8] = HttpUtility.HtmlDecode(actualizador);
                    datarw[9] = HttpUtility.HtmlDecode(u.baja_logica == false ? "No" : "Si");

                    dt.Rows.Add(datarw);
                }

                grdUbiActual.DataSource = dt;
                grdUbiActual.DataBind();
            }

            LlenarGridUbicacionesAnt(id_ubicacion);
            LlenarGridUbicacionesPost(id_ubicacion);
        }

        /// <summary>
        /// Ubicaciones derivadas de la Ubicación suministrada
        /// </summary>
        /// <param name="id_ubicacion">Id de la Ubicación base</param>
        /// <returns></returns>
        private void LlenarGridUbicacionesPost(int id_ubicacion)
        {
            using (var ctx = new DGHP_Entities())
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("PartidaMatriz", typeof(string));
                dt.Columns.Add("Seccion", typeof(string));
                dt.Columns.Add("Manzana", typeof(string));
                dt.Columns.Add("Parcela", typeof(string));
                dt.Columns.Add("baja_logica", typeof(string));
                dt.Columns.Add("TipoDeOperacion", typeof(string));
                dt.Columns.Add("EstadoDeOperacion", typeof(string));
                dt.Columns.Add("FechaOperacion", typeof(string));
                dt.Columns.Add("Usuario", typeof(string));

                var ops = GetUbicacionesOp(id_ubicacion);
                foreach (var op in ops)
                {
                    var opds = GetUbicacionesOpDet(op.id_operacion);
                    // Obviamos las ubicaciones hermanas
                    var o = opds.Where(a => a.id_ubicacion == id_ubicacion).FirstOrDefault();
                    if (o.id_ubicacion_temp != null)
                        continue;

                    string op_accion = GetAccionDesc(op.id_accion);
                    string op_estado = GetEstadoDesc(op.id_estado);
                    string op_user = GetUserName(op.CreateUser);
                    foreach (var opd in opds)
                    {
                        if (opd.id_ubicacion != id_ubicacion && opd.id_ubicacion_temp != null)
                        {
                            DataRow datarw;
                            datarw = dt.NewRow();

                            var u = ctx.Ubicaciones.Where(a => a.id_ubicacion == opd.id_ubicacion).SingleOrDefault();
                            if (u != null)
                            {
                                datarw[0] = HttpUtility.HtmlDecode(opd.id_ubicacion.ToString());
                                datarw[1] = HttpUtility.HtmlDecode(u.NroPartidaMatriz != null ? u.NroPartidaMatriz.ToString() : "");
                                datarw[2] = HttpUtility.HtmlDecode(u.Seccion != null ? u.Seccion.ToString() : "");
                                datarw[3] = HttpUtility.HtmlDecode(u.Manzana);
                                datarw[4] = HttpUtility.HtmlDecode(u.Parcela);
                                datarw[5] = HttpUtility.HtmlDecode(u.baja_logica == false ? "No" : "Si");
                                datarw[6] = HttpUtility.HtmlDecode(op_accion);
                                datarw[7] = HttpUtility.HtmlDecode(op_estado);
                                datarw[8] = HttpUtility.HtmlDecode(op.CreateDate != null ? op.CreateDate.ToString() : "");
                                datarw[9] = HttpUtility.HtmlDecode(op_user);

                                dt.Rows.Add(datarw);
                            }
                        }
                    }
                }

                grdUbisPosteriores.DataSource = dt;
                grdUbisPosteriores.DataBind();
            }
        }

        /// <summary>
        /// Ubicaciones de las cuales derivó la Ubicación suministrada
        /// </summary>
        /// <param name="id_ubicacion">Ubicación derivada</param>
        /// <returns></returns>
        private void LlenarGridUbicacionesAnt(int id_ubicacion)
        {
            using (var ctx = new DGHP_Entities())
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("PartidaMatriz", typeof(string));
                dt.Columns.Add("Seccion", typeof(string));
                dt.Columns.Add("Manzana", typeof(string));
                dt.Columns.Add("Parcela", typeof(string));
                dt.Columns.Add("baja_logica", typeof(string));
                dt.Columns.Add("TipoDeOperacion", typeof(string));
                dt.Columns.Add("EstadoDeOperacion", typeof(string));
                dt.Columns.Add("FechaOperacion", typeof(string));
                dt.Columns.Add("Usuario", typeof(string));

                var ops = GetUbicacionesOp(id_ubicacion);
                foreach (var op in ops)
                {
                    var opds = GetUbicacionesOpDet(op.id_operacion);
                    // Obviamos las ubicaciones hermanas
                    var o = opds.Where(a => a.id_ubicacion == id_ubicacion).FirstOrDefault();
                    if (o.id_ubicacion_temp == null)
                        continue;

                    string op_accion = GetAccionDesc(op.id_accion);
                    string op_estado = GetEstadoDesc(op.id_estado);
                    string op_user = GetUserName(op.CreateUser);
                    foreach (var opd in opds)
                    {
                        if (opd.id_ubicacion != id_ubicacion && opd.id_ubicacion_temp == null)
                        {
                            DataRow datarw;
                            datarw = dt.NewRow();

                            var u = ctx.Ubicaciones.Where(a => a.id_ubicacion == opd.id_ubicacion).SingleOrDefault();
                            if (u != null)
                            {
                                datarw[0] = HttpUtility.HtmlDecode(opd.id_ubicacion.ToString());
                                datarw[1] = HttpUtility.HtmlDecode(u.NroPartidaMatriz != null ? u.NroPartidaMatriz.ToString() : "");
                                datarw[2] = HttpUtility.HtmlDecode(u.Seccion != null ? u.Seccion.ToString() : "");
                                datarw[3] = HttpUtility.HtmlDecode(u.Manzana);
                                datarw[4] = HttpUtility.HtmlDecode(u.Parcela);
                                datarw[5] = HttpUtility.HtmlDecode(u.baja_logica == false ? "No" : "Si");
                                datarw[6] = HttpUtility.HtmlDecode(op_accion);
                                datarw[7] = HttpUtility.HtmlDecode(op_estado);
                                datarw[8] = HttpUtility.HtmlDecode(op.CreateDate != null ? op.CreateDate.ToString() : "");
                                datarw[9] = HttpUtility.HtmlDecode(op_user);

                                dt.Rows.Add(datarw);
                            }
                        }
                    }
                }

                grdUbisAnteriores.DataSource = dt;
                grdUbisAnteriores.DataBind();
            }
        }

        /// <summary>
        /// Colección de Operaciones relacionadas con una Ubicación
        /// </summary>
        /// <param name="id_ubicacion">Id de la Ubicación</param>
        /// <returns>Colección de Operaciones</returns>
        public List<Ubicaciones_Operaciones> GetUbicacionesOp(int id_ubicacion)
        {
            using (var ctx = new DGHP_Entities())
            {
                var ubi_ops = (from ops in ctx.Ubicaciones_Operaciones
                               join opd in ctx.Ubicaciones_Operaciones_Detalle on ops.id_operacion equals opd.id_operacion
                               where opd.id_ubicacion == id_ubicacion
                               select ops).ToList();

                return ubi_ops;
            }
        }

        /// <summary>
        /// Coleccion de registros detallados de una Operación
        /// </summary>
        /// <param name="id_operacion">Id de la Operación</param>
        /// <returns>Colección de registros detallados</returns>
        public List<Ubicaciones_Operaciones_Detalle> GetUbicacionesOpDet(int id_operacion)
        {
            using (var ctx = new DGHP_Entities())
            {
                var ubi_ops_det = ctx.Ubicaciones_Operaciones_Detalle.Where(a => a.id_operacion == id_operacion).ToList();

                return ubi_ops_det;
            }
        }

        /// <summary>
        /// Obtiene el nombre de usuario
        /// </summary>
        /// <param name="userId">GUID del usuario</param>
        /// <returns>Devuelve el nombre de usuario o una cadena vacia</returns>
        public string GetUserName(Guid? userId)
        {
            if (userId == null)
                return "";

            using (var ctx = new DGHP_Entities())
            {
                var usu = ctx.aspnet_Users.Where(a => a.UserId == userId).SingleOrDefault();

                return usu != null ? usu.UserName : "";
            }
        }

        /// <summary>
        /// Obtiene la decripción de una Acción
        /// </summary>
        /// <param name="id_accion">Id de la Acción</param>
        /// <returns>Decripción de la Acción</returns>
        public string GetAccionDesc(int id_accion)
        {
            using (var ctx = new DGHP_Entities())
            {
                var accion = ctx.Ubicaciones_Acciones.Where(a => a.id_accion == id_accion).FirstOrDefault();

                return accion != null ? accion.Descripcion : "";
            }
        }

        /// <summary>
        /// Obtiene la decripción de un Estado
        /// </summary>
        /// <param name="id_accion">Id del Estado</param>
        /// <returns>Decripción del Estado</returns>
        public string GetEstadoDesc(int id_estado)
        {
            using (var ctx = new DGHP_Entities())
            {
                var est = ctx.Ubicaciones_Estados.Where(a => a.id_estado == id_estado).FirstOrDefault();

                return est != null ? est.Descripcion : "";
            }
        }

        protected void btnHistSMP_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string id_ubi = btn.CommandArgument.ToString();
            LlenarGridUbiActual(int.Parse(id_ubi));

            this.EjecutarScript(updPnlResultadoBuscar, "showfrmHistUbicacionSMP();");
        }

        protected void btnContinuarAdv_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ABM/Ubicaciones/EliminarUbicacion.aspx?id=" + hidAdvIdUbi.Value);
        }

        protected void btnCancelarAdv_Click(object sender, EventArgs e)
        {
            this.EjecutarScript(updfrmAdvBody, "hidefrmAdvertencia();");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            hidAdvIdUbi.Value = btn.CommandArgument.ToString();

            lblAdv.Text = "ATENCION: Tenga en cuenta que esta funcionalidad es únicamente para bajas de parcelas SIN fraccionamiento ni unificación. Si necesita hacer un fraccionamiento o Unificación de parcelas debe acceder a “+ Nueva ubicación” para no afectar el historial de la parcela.";
            this.EjecutarScript(updPnlResultadoBuscar, "showfrmAdvertencia();");
        }

        protected void btnNuevaUbicacion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ABM/Ubicaciones/NuevaUbicacion.aspx");
        }

        protected void btnUnificarUbicacion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ABM/Ubicaciones/UnificarUbicaciones.aspx");
        }

        protected void btnSubdividirUbicacion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ABM/Ubicaciones/SubdividirUbicacion.aspx");
        }
    }
}