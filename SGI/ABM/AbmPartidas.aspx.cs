using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using System.Drawing;
using System.Data;
using System.Web;
using System.Collections;

namespace SGI.ABM
{
    public partial class AbmPartidas : BasePage
    {

        #region cargar inicial
        private string colorEstadoEnPoceso = "#fcd8b8";
        private string colorBordeEnPoceso = "#ea8f3e";
        private string colorEstadoConfirmada = "#cff3d7";
        private string colorBordeConfirmada = "#68be7b";

        private int estadoNueva = -1;
        private int estadoEnProceso = 0;
        private int estadoAnulada = 1;
        private int estadoConfirmada = 2;
        private int estadoAprobada = 3;
        private int estadoRechazada = 4;


        bool filtroPHSubGrilla = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updDatosHorizon, updDatosHorizon.GetType(), "init_Js_updDatosHorizon", "init_Js_updDatosHorizon();", true);
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_ubi_dom, updPnlFiltroBuscar_ubi_dom.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updPnlResultadoBuscar, updPnlResultadoBuscar.GetType(), "init_Js_updPnlResultadoBuscar", "init_Js_updPnlResultadoBuscar();", true);
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), " init_Js_updDatos", " init_Js_updDatos();", true);
                ScriptManager.RegisterStartupScript(updBodyAgregarUbicacion, updBodyAgregarUbicacion.GetType(), "init_Js_updBodyAgregarUbicacion", "init_Js_updBodyAgregarUbicacion();", true);
                ScriptManager.RegisterStartupScript(updUbicaciones, updUbicaciones.GetType(), "init_Js_updUbicacion", "init_Js_updUbicacion();", true);
                //ScriptManager.RegisterStartupScript(updHistorial, updHistorial.GetType(), "init_Js_updHistorial", "init_Js_updHistorial();", true);
                //ScriptManager.RegisterStartupScript(updHistorialHori, updHistorialHori.GetType(), "init_Js_updHistorialHori", "init_Js_updHistorialHori();", true);
                ScriptManager.RegisterStartupScript(UpdateZonas, UpdateZonas.GetType(), "init_Js_updZonas", "init_Js_updZonas();", true);
                //ScriptManager.RegisterStartupScript(updDatos, updDatosHorizon.GetType(), " init_Js_updDatosHorizon", " init_Js_updDatosHorizon();", true);
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_ubi_especial, updPnlFiltroBuscar_ubi_especial.GetType(), "init_Js_updPnlFiltroBuscar_ubi_especial", "init_Js_updPnlFiltroBuscar_ubi_especial();", true);
            }
            if (!IsPostBack)
            {
                LoadData();
            }
            cargarPermisos();
            btnNuevaPartida.Visible = editar;


        }
        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                CargarComboCalles();

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
            DGHP_Entities db = new DGHP_Entities();

            var AddCalles = (from cal in db.Calles
                             select new
                             {
                                 cal.AlturaDerechaFin_calle,
                                 cal.AlturaDerechaInicio_calle,
                                 cal.AlturaIzquierdaFin_calle,
                                 cal.AlturaIzquierdaInicio_calle,
                                 cal.NombreOficial_calle,
                                 cal.id_calle,
                             });

            List<ItemCalle> LstICalle = new List<ItemCalle>();

            foreach (var c in AddCalles)
            {
                ItemCalle item = new ItemCalle();

                item.AlturaMax = c.AlturaDerechaFin_calle > c.AlturaIzquierdaFin_calle ? c.AlturaDerechaFin_calle : c.AlturaIzquierdaFin_calle;
                item.AlturaMin = c.AlturaDerechaInicio_calle < c.AlturaIzquierdaInicio_calle ? c.AlturaDerechaInicio_calle : c.AlturaIzquierdaInicio_calle;
                item.Id_calle = c.id_calle;
                item.CallePuerta = c.NombreOficial_calle + " [" + item.AlturaMin + " - " + item.AlturaMax + "]";
                LstICalle.Add(item);
            }

            ddlCalle.DataTextField = "CallePuerta";
            ddlCalle.DataValueField = "Id_calle";
            ddlCalle.DataSource = LstICalle.OrderBy(x => x.CallePuerta).OrderBy(x => x.CallePuerta).ToList();
            ddlCalle.DataBind();
            ddlCalle.Items.Insert(0, "");

            var lstCalles = (from cal in db.Calles
                             select new
                             {
                                 NombreOficial_calle = cal.NombreOficial_calle,
                                 Codigo_calle = cal.Codigo_calle
                             }).Distinct().OrderBy(x => x.NombreOficial_calle);

            ddlUbiCalle.DataTextField = "NombreOficial_calle";
            ddlUbiCalle.DataValueField = "Codigo_calle";
            ddlUbiCalle.DataSource = lstCalles.ToList();
            ddlUbiCalle.DataBind();
            ddlUbiCalle.Items.Insert(0, "");

            db.Dispose();
        }


        internal class ItemCalle
        {
            public string CallePuerta { get; set; }
            public int Id_calle { get; set; }
            public int? AlturaMin { get; set; }
            public int? AlturaMax { get; set; }
        }


        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
        }

        #region entity

        private DGHP_Entities db = null;
        private AGC_FilesEntities dbFiles = null;

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

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
            {
                this.dbFiles.Dispose();
                this.dbFiles = null;

            }
        }

        #endregion

        public void LoadData()
        {

            try
            {
                IniciarEntity();

                CargarCombo_tipoUbicacion(true);
                CargarCombo_subTipoUbicacion(-1, true);


                updPnlFiltroBuscar_ubi_partida.Update();
                updPnlFiltroBuscar_ubi_dom.Update();
                updPnlFiltroBuscar_ubi_smp.Update();
                updPnlFiltroBuscar_ubi_especial.Update();

                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                throw ex;
            }

        }



        private void CargarCombo_tipoUbicacion(Boolean busqueda)
        {
            List<TiposDeUbicacion> lista = this.db.TiposDeUbicacion.Where(x => x.id_tipoubicacion > 0).ToList();

            TiposDeUbicacion tipo_ubi = new TiposDeUbicacion();
            tipo_ubi.id_tipoubicacion = 0;

            tipo_ubi.descripcion_tipoubicacion = (busqueda) ? "Seleccione" : "Parcela Común";
            lista.Insert(0, tipo_ubi);
            if (busqueda)
            {
                ddlbiTipoUbicacion.DataTextField = "descripcion_tipoubicacion";
                ddlbiTipoUbicacion.DataValueField = "id_tipoubicacion";

                ddlbiTipoUbicacion.DataSource = lista;
                ddlbiTipoUbicacion.DataBind();
            }
            else
            {
                ddlbiTipoUbicacionABM.DataTextField = "descripcion_tipoubicacion";
                ddlbiTipoUbicacionABM.DataValueField = "id_tipoubicacion";

                ddlbiTipoUbicacionABM.DataSource = lista.OrderBy(x => x.descripcion_tipoubicacion);
                ddlbiTipoUbicacionABM.DataBind();
            }
        }

        private void CargarCombo_subTipoUbicacion(int id_tipoubicacion, Boolean busqueda)
        {

            List<SubTiposDeUbicacion> lista = this.db.SubTiposDeUbicacion.Where(x => x.id_tipoubicacion == id_tipoubicacion).ToList();

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
            else
            {
                ddlUbiSubTipoUbicacionABM.DataTextField = "descripcion_subtipoubicacion";
                ddlUbiSubTipoUbicacionABM.DataValueField = "id_subtipoubicacion";
                ddlUbiSubTipoUbicacionABM.DataSource = lista.OrderBy(x => x.descripcion_subtipoubicacion);
                ddlUbiSubTipoUbicacionABM.DataBind();
            }

        }


        protected void ddlbiTipoUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                IniciarEntity();
                int id_tipoubicacion = int.Parse(ddlbiTipoUbicacion.SelectedValue);
                CargarCombo_subTipoUbicacion(id_tipoubicacion, true);
                FinalizarEntity();
            }
            catch (Exception)
            {
                FinalizarEntity();
            }

            updPnlFiltroBuscar_ubi_especial.Update();

        }
        protected void ddlbiTipoUbicacionABM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                IniciarEntity();

                int id_tipoubicacion = int.Parse(ddlbiTipoUbicacionABM.SelectedValue);
                hid_id_tipo_ubicacion.Value = id_tipoubicacion.ToString();
                CargarCombo_subTipoUbicacion(id_tipoubicacion, false);
                FinalizarEntity();
            }
            catch (Exception)
            {
                FinalizarEntity();
            }
            updPnlFiltroBuscar_ubi_especial.Update();

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

        #endregion

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
            id_calle = "";
            nro_calle = 0;
            seccion = 0;
            manzana = "";
            parcela = "";
            id_tipo_ubicacion = -1;
            id_sub_tipo_ubicacion = -1;
            if (ddlbiTipoUbicacion.Items.Count >= 0)
                ddlbiTipoUbicacion.SelectedIndex = 0;

            if (ddlUbiSubTipoUbicacion.Items.Count >= 0)
                ddlUbiSubTipoUbicacion.SelectedIndex = 0;
            pnlResultadoBuscar.Visible = false;
            updPnlFiltroBuscar_ubi_partida.Update();
            updPnlFiltroBuscar_ubi_dom.Update();
            updPnlFiltroBuscar_ubi_smp.Update();
            updPnlFiltroBuscar_ubi_especial.Update();
            updPnlResultadoBuscar.Update();
            EjecutarScript(btn_BuscarPartida, "hideResultado();");

        }
        private bool filtrar = false;

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {

                Validar_BuscarPorUbicacion();
                filtrar = true;
                grdResultados.DataBind();
                updPnlResultadoBuscar.Update();

                pnlResultadoBuscar.Visible = true;

                EjecutarScript(updPnlResultadoBuscar, "showResultado();");
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                LogError.Write(ex, "error al buscar Partidas buscar_Partidas-btnBuscar_OnClick");
                Enviar_Mensaje(ex.Message, "");
            }
        }
        private int nro_partida_matriz = 0;
        private int nro_partida_horiz = 0;
        private string id_calle = "";
        private int nro_calle = 0;
        private int seccion = 0;
        private string manzana = "";
        private string parcela = "";
        private int id_tipo_ubicacion = -1;
        private int id_sub_tipo_ubicacion = -1;

        public List<SGI.Model.Shared.ItemPartidasAux> GetResultados(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)

        {
            db = new DGHP_Entities();
            totalRowCount = 0;

            //LimpiarCampos(this.divcmbPartidas.Controls);

            if (!filtrar)
            {
                pnlResultadoBuscar.Visible = false;
                updPnlResultadoBuscar.Update();
                return null;
            }
            List<SGI.Model.Shared.ItemPartidasAux> resultados = new List<SGI.Model.Shared.ItemPartidasAux>();

            var w = (from ubicacion in db.Ubicaciones
                     select new SGI.Model.Shared.ItemPartidas
                     {
                         manzana = ubicacion.Manzana,
                         seccion = ubicacion.Seccion,
                         parcela = ubicacion.Parcela,
                         partida = ubicacion.NroPartidaMatriz,
                         id_ubicacion = ubicacion.id_ubicacion,
                         inhibida = ubicacion.Inhibida_Observacion,
                         id_subtipoubicacion = ubicacion.id_subtipoubicacion,
                         baja_logica = ubicacion.baja_logica == false ? "No" : "Si",
                         codigo_calle = 0,
                         nro_puerta = 0,
                     });

            bool bajaLogica = false;
            bool bajaLogicaPH = false;

            bool.TryParse(ddlBaja.SelectedValue, out bajaLogica);

            bool.TryParse(ddlPHDadaDBaja.SelectedValue, out bajaLogicaPH);

            if (!bajaLogica) //por defecto no se muestran ubicaciones dadas de baja
            {
                w = w.Where(x => x.baja_logica == "No");
            }
            else
            {
                w = w.Where(x => x.baja_logica == "Si");
            }

            if ((this.nro_partida_matriz > 0) && (rbtnUbiPartidaMatriz.Checked))
            {
                filtroPHSubGrilla = bajaLogica;
                w = w.Where(x => x.partida == this.nro_partida_matriz);
            }

            if ((this.nro_partida_horiz > 0) && (rbtnUbiPartidaHoriz.Checked))
            {
                w = (from res in w
                     join ubiPartidas in db.Ubicaciones_PropiedadHorizontal on res.id_ubicacion equals ubiPartidas.id_ubicacion
                     where ubiPartidas.NroPartidaHorizontal == nro_partida_horiz && ubiPartidas.baja_logica == bajaLogicaPH
                     select res);

                filtroPHSubGrilla = bajaLogicaPH;
            }

            if (this.id_calle.Trim().Length > 0)
            {
                if (this.nro_calle > 0)
                    w = (from res in w
                         join ubiPuertas in db.Ubicaciones_Puertas on res.id_ubicacion equals ubiPuertas.id_ubicacion
                         join callesNombre in db.Calles on ubiPuertas.codigo_calle equals callesNombre.Codigo_calle
                         where callesNombre.NombreOficial_calle.Contains(this.id_calle.Trim())
                               && ubiPuertas.NroPuerta_ubic == this.nro_calle
                         select new SGI.Model.Shared.ItemPartidas
                         {
                             manzana = res.manzana,
                             seccion = res.seccion,
                             parcela = res.parcela,
                             partida = res.partida,
                             id_ubicacion = res.id_ubicacion,
                             inhibida = res.inhibida,
                             id_subtipoubicacion = res.id_subtipoubicacion,
                             baja_logica = res.baja_logica,
                             codigo_calle = 0,
                             nro_puerta = 0
                         });
                else
                    w = (from res in w
                         join ubiPuertas in db.Ubicaciones_Puertas on res.id_ubicacion equals ubiPuertas.id_ubicacion
                         join callesNombre in db.Calles on ubiPuertas.codigo_calle equals callesNombre.Codigo_calle
                         where callesNombre.NombreOficial_calle.Contains(this.id_calle.Trim())
                         select new SGI.Model.Shared.ItemPartidas
                         {
                             manzana = res.manzana,
                             seccion = res.seccion,
                             parcela = res.parcela,
                             partida = res.partida,
                             id_ubicacion = res.id_ubicacion,
                             inhibida = res.inhibida,
                             id_subtipoubicacion = res.id_subtipoubicacion,
                             baja_logica = res.baja_logica,
                             codigo_calle = 0,
                             nro_puerta = 0
                         });
            }

            if (this.seccion > 0)
                w = w.Where(x => x.seccion == this.seccion);
            if (this.manzana.Trim().Length > 0)
                w = w.Where(x => x.manzana.Contains(this.manzana.Trim()));
            if (this.parcela.Trim().Length > 0)
                w = w.Where(x => x.parcela.Contains(this.parcela.Trim()));
            if (this.id_tipo_ubicacion != 0)
            {
                w = (from res in w
                     join su in db.SubTiposDeUbicacion on res.id_subtipoubicacion equals su.id_subtipoubicacion
                     join tu in db.TiposDeUbicacion on su.id_tipoubicacion equals tu.id_tipoubicacion
                     where tu.id_tipoubicacion == this.id_tipo_ubicacion
                     select new SGI.Model.Shared.ItemPartidas
                     {
                         manzana = res.manzana,
                         seccion = res.seccion,
                         parcela = res.parcela,
                         partida = res.partida,
                         id_ubicacion = res.id_ubicacion,
                         inhibida = res.inhibida,
                         id_subtipoubicacion = res.id_subtipoubicacion,
                         baja_logica = res.baja_logica,
                         codigo_calle = res.codigo_calle,
                         nro_puerta = res.nro_puerta
                     });

                if (this.id_sub_tipo_ubicacion != -1)
                {
                    w = w.Where(x => x.id_subtipoubicacion == this.id_sub_tipo_ubicacion);
                }
            }
            var qa = (from qs in w
                      select new SGI.Model.Shared.ItemPartidasAux
                      {
                          manzana = qs.manzana,
                          seccion = qs.seccion,
                          parcela = qs.parcela,
                          partida = qs.partida,
                          id_ubicacion = qs.id_ubicacion,
                          inhibida = qs.inhibida,
                          id_subtipoubicacion = qs.id_subtipoubicacion,
                          direccion = "",
                          id_estado_modif_ubi = (db.Ubicaciones_Historial_Cambios.Where(x => x.id_ubicacion.Equals(qs.id_ubicacion)).Count() != 0) ?
                                                 (db.Ubicaciones_Historial_Cambios.Where(x => x.id_ubicacion.Equals(qs.id_ubicacion)).OrderByDescending(o => o.id_ubihistcam).Select(r => r.id_estado_modif).FirstOrDefault()) : -1,
                          baja_logica = qs.baja_logica,
                          codigo_calle = qs.codigo_calle,
                          nro_puerta = qs.nro_puerta
                      }
                     ).Distinct();



            totalRowCount = qa.Count();
            resultados = qa.OrderBy(o => o.codigo_calle).ThenBy(x => x.nro_puerta).Skip(startRowIndex).Take(maximumRows).ToList();

            if (resultados.Count > 0)
            {
                string[] arrUbicaciones = resultados.Select(s => s.id_ubicacion.ToString()).ToArray();
                List<ItemDireccion> lstDirecciones = Shared.getDirecciones(arrUbicaciones);

                //------------------------------------------------------------------------
                //Rellena la clase a devolver con los datos que faltaban (Direccion,tarea)
                //------------------------------------------------------------------------
                foreach (var row in resultados)
                {
                    var itemDireccion = lstDirecciones.FirstOrDefault(x => x.id_ubicacion == row.id_ubicacion);
                    if (itemDireccion != null)
                        row.direccion = (string.IsNullOrEmpty(itemDireccion.direccion) ? "" : itemDireccion.direccion);
                }
            }

            pnlCantidadRegistros.Visible = (resultados.Count > 0);
            //lblCantidadRegistros.Text = resultados.Count.ToString();
            lblCantidadRegistros.Text = totalRowCount.ToString();


            db.Dispose();

            return resultados;
        }


        private bool editable = true;
        private void CargarDatos(int id_ubi, int idParHori, int? id_Solic)
        {
            db = new DGHP_Entities();
            editable = true;
            var w = (from ubicacion in db.Ubicaciones_Historial_Cambios
                     where ubicacion.id_ubihistcam == id_Solic
                     select new
                     {
                         manzana = ubicacion.Manzana,
                         seccion = ubicacion.Seccion,
                         parcela = ubicacion.Parcela,
                         nroPartida = ubicacion.NroPartidaMatriz,
                         id_ubicacion = ubicacion.id_ubicacion,
                         subtipo = ubicacion.id_subtipoubicacion,
                         tipoSoli = ubicacion.tipo_solicitud,
                         zonaPla = ubicacion.id_zonaplaneamiento,
                         obsZ = ubicacion.Observaciones,
                         obsEst = ubicacion.observaciones_ubihistcam,
                         id_comisaria = ubicacion.id_comisaria,
                         id_barrio = ubicacion.id_barrio,
                         id_estado = ubicacion.id_estado_modif,
                         chbEntidad = ubicacion.EsEntidadGubernamental,
                         chbEdificio = ubicacion.EsUbicacionProtegida,
                         DadodeBaja = ubicacion.baja_logica == null ? false : ubicacion.baja_logica,
                     }
                      ).FirstOrDefault();

            if (w != null)
            {

                if ((w.tipoSoli == 2) ||
                    (!this.editar) ||
                    (this.estadoEnProceso != w.id_estado) ||
                     (this.estadoNueva == w.id_estado))
                    editable = false;

                txtManzana.Text = w.manzana;
                txtManzana.Enabled = editable;
                txtParcela.Text = w.parcela;
                txtNroSol.Text = id_Solic.ToString();
                txtParcela.Enabled = editable;
                txtNroPartida.Enabled = editable;
                txtSeccion.Enabled = editable;
                txtTipoSolicitud.Text = cargarTipoSol(w.tipoSoli, true);

                int? a = w.nroPartida;
                if (a != null)
                    txtNroPartida.Text = a.ToString();

                int? b = w.seccion;
                if (b != null)
                    txtSeccion.Text = b.ToString();

                chbEntidadGubernamental.Checked = w.chbEntidad;
                chbEntidadGubernamental.Enabled = editable;
                chbEdificioProtegido.Checked = w.chbEdificio;
                chbEdificioProtegido.Enabled = editable;

                if (w.DadodeBaja == true)
                {
                    rbtnBajaNo.Checked = false;
                    rbtnBajaSi.Checked = true;
                }
                if (w.DadodeBaja == false)
                {
                    rbtnBajaSi.Checked = false;
                    rbtnBajaNo.Checked = true;
                }
                rbtnBajaSi.Enabled = editable;
                rbtnBajaNo.Enabled = editable;

                CargarCombo_tipoUbicacion(false);

                CargarCombos_Barrios();
                CargarCombos_Comisaria();

                if (w.id_comisaria.HasValue)
                    ddlComisaria.SelectedValue = w.id_comisaria.ToString();

                if (w.id_barrio.HasValue)
                    ddlBarrio.SelectedValue = w.id_barrio.ToString();

                if (w.subtipo != null)
                {
                    var tipo = (from st in this.db.SubTiposDeUbicacion
                                join tip in db.TiposDeUbicacion on st.id_tipoubicacion equals tip.id_tipoubicacion
                                where st.id_subtipoubicacion == w.subtipo
                                select new
                                {
                                    idTipoUbi = tip.id_tipoubicacion
                                }).First();
                    ddlbiTipoUbicacionABM.SelectedValue = tipo.idTipoUbi.ToString();
                    hid_id_tipo_ubicacion.Value = tipo.idTipoUbi.ToString();
                    CargarCombo_subTipoUbicacion(tipo.idTipoUbi, false);
                    ddlUbiSubTipoUbicacionABM.SelectedValue = w.subtipo.ToString();
                    ddlbiTipoUbicacionABM.Enabled = editable;
                    ddlUbiSubTipoUbicacionABM.Enabled = editable;
                }

                CargarCombos_ZonasDeslindes();
                ddlZona1.SelectedValue = w.zonaPla.ToString();
                ddlZona1.Enabled = editable;

                var ff = (from ubiZonas in db.Ubicaciones_ZonasComplementarias_Historial_Cambios
                          join zonaW in db.Zonas_Planeamiento on ubiZonas.id_zonaplaneamiento equals zonaW.id_zonaplaneamiento
                          where (id_ubicacion == ubiZonas.id_ubicacion && ubiZonas.id_ubihistcam == id_Solic)
                          select new SGI.Model.Shared.ItemZonasComp
                          {
                              tipoUbi = ubiZonas.tipo_ubicacion,

                              id_zonaplaneamiento = ubiZonas.id_zonaplaneamiento
                          }).Distinct();

                List<SGI.Model.Shared.ItemZonasComp> resul = ff.ToList();
                foreach (var row in resul)
                {
                    switch (row.tipoUbi)
                    {
                        case "Z2":
                            ddlZona2.SelectedValue = row.id_zonaplaneamiento.ToString();

                            break;

                        case "Z3":
                            ddlZona3.SelectedValue = row.id_zonaplaneamiento.ToString();

                            break;

                    }
                }
                ddlZona2.Enabled = editable;
                ddlZona3.Enabled = editable;
                txtObservaciones.Text = w.obsZ;
                List<SGI.Model.ItemEstadosSolicitud> l = cargarEstadosPosibles();
                ddlEstados.DataTextField = "nombre_estado";
                ddlEstados.DataValueField = "id_estado";
                txtObservaciones.Enabled = true;
                ddlEstados.Enabled  = true;
                divEstado.Visible = true;
                ddlEstados.DataSource = l;
                ddlEstados.DataBind();
                ddlEstados.SelectedValue = w.id_estado.ToString();

                ddlComisaria.Enabled = editable;
                ddlBarrio.Enabled = editable;

                txtObservacionesEst.Text = w.obsEst;
                txtObservacionesEst.Enabled = editable;
                var lstUbicaciones = (from ubiPuertas in db.Ubicaciones_Puertas_Historial_Cambios
                                      join callesNombre in db.Calles on ubiPuertas.codigo_calle equals callesNombre.Codigo_calle
                                      where id_Solic == ubiPuertas.id_ubihistcam
                                           && (ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaDerechaInicio_calle || ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaIzquierdaInicio_calle)
                                           && (ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaDerechaFin_calle || ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaIzquierdaFin_calle)

                                      select new
                                      {
                                          calles = callesNombre.NombreOficial_calle,
                                          nroPuerta = ubiPuertas.NroPuerta_ubic

                                      }).Distinct().ToList();

                grdUbicaciones.DataSource = lstUbicaciones;
                grdUbicaciones.DataBind();
                btnAgregarUbicacion.Visible = editable;

                var lstHistorial = (from hist in db.Ubicaciones_Historial_Cambios_UsuariosIntervinientes
                                    join usu in db.aspnet_Users on hist.Userid equals usu.UserId
                                    join usuAdi in db.Usuario on usu.UserId equals usuAdi.UserId into zr
                                    from fd in zr.DefaultIfEmpty()
                                    where id_Solic == hist.id_ubihistcam
                                    select new
                                    {
                                        UserName = usu.UserName,
                                        Apenom = fd.Apellido + " " + fd.Nombre,
                                        LastUpdateDate = hist.LastUpdateDate

                                    }
                                    ).Distinct().ToList();
                grdHistorial.DataSource = lstHistorial;
                grdHistorial.DataBind();
                //btnGuardar.Visible = editable;
                //btnCancelar.Visible = editable;
                //grdUbicaciones.Enabled = editable;
                //btnAgregarUbicacion.Visible = editable;

            }
            db.Dispose();

        }

        private string cargarTipoSol(int tipoS, bool matriz)
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

        private string getNombreEstado(int tipoEstado)
        {

            switch (tipoEstado)
            {

                case 0:
                    return "En Proceso";
                case 1:
                    return "Anulada";
                case 2:
                    return "Confirmada";
                case 3:
                    return "Aprobada";
                case 4:
                    return "Rechazada";


            }
            return "";
        }



        private List<SGI.Model.ItemEstadosSolicitud> cargarEstadosPosibles()
        {
            List<SGI.Model.ItemEstadosSolicitud> estados = new List<SGI.Model.ItemEstadosSolicitud>();
            int x = -1;

            if ((this.editar && (this.id_estado_partida != this.estadoConfirmada))
                || (this.aprobar && this.id_estado_partida == this.estadoConfirmada))
            {
                SGI.Model.ItemEstadosSolicitud it0 = new SGI.Model.ItemEstadosSolicitud();
                it0.id_estado = 0;
                it0.nombre_estado = "En Proceso";
                x++;
                estados.Insert(x, it0);
            }
            if ((this.editar) || (this.aprobar && this.id_estado_partida == this.estadoConfirmada))
            {
                SGI.Model.ItemEstadosSolicitud it1 = new SGI.Model.ItemEstadosSolicitud();
                it1.id_estado = 1;
                it1.nombre_estado = "Anulada";
                x++;
                estados.Insert(x, it1);
            }
            if ((this.editar) || (this.aprobar && this.id_estado_partida == this.estadoConfirmada))
            {
                SGI.Model.ItemEstadosSolicitud it2 = new SGI.Model.ItemEstadosSolicitud();
                it2.id_estado = 2;
                it2.nombre_estado = "Confirmada";
                x++;
                estados.Insert(x, it2);

            }
            if (this.aprobar)
            {
                SGI.Model.ItemEstadosSolicitud it3 = new SGI.Model.ItemEstadosSolicitud();
                it3.id_estado = 3;
                it3.nombre_estado = "Aprobada";
                x++;
                estados.Insert(x, it3);
            }
            if (this.aprobar)
            {
                SGI.Model.ItemEstadosSolicitud it4 = new SGI.Model.ItemEstadosSolicitud();
                it4.id_estado = 4;
                it4.nombre_estado = "Rechazada";
                x++;

                estados.Insert(x, it4);
            }

            return estados;
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

        private bool numeroPuertaValido(int idCalle, int nroPuertaUbi)
        {
            return (from ca in this.db.Calles select ca)
                .Where(x => (x.id_calle == idCalle))
                .Where(x => x.AlturaIzquierdaInicio_calle <= nroPuertaUbi || x.AlturaDerechaInicio_calle <= nroPuertaUbi)
                .Where(x => x.AlturaDerechaFin_calle >= nroPuertaUbi || x.AlturaIzquierdaFin_calle >= nroPuertaUbi).Any();
        }

        private void CargarCombos_Comisaria()
        {
            IEnumerable<SGI.Model.Comisarias> comisarias = (from com in this.db.Comisarias select com).Distinct().OrderBy(p => p.nom_comisaria);
            ddlComisaria.DataTextField = "nom_comisaria";
            ddlComisaria.DataValueField = "id_comisaria";
            ddlComisaria.DataSource = comisarias.ToList();
            ddlComisaria.DataBind();
        }
        private void CargarCombos_Barrios()
        {
            IEnumerable<SGI.Model.Barrios> barrios = (from barr in this.db.Barrios select barr).Distinct().OrderBy(p => p.nom_barrio);
            ddlBarrio.DataTextField = "nom_barrio";
            ddlBarrio.DataValueField = "id_barrio";
            ddlBarrio.DataSource = barrios.ToList();
            ddlBarrio.DataBind();
        }
        private void CargarCombos_ZonasDeslindes()
        {

            List<Shared.ZonasPlaneamientoAux> lista = (from zonas in this.db.Zonas_Planeamiento
                                                       select new Shared.ZonasPlaneamientoAux
                                                       {
                                                           id_zonaplaneamiento = zonas.id_zonaplaneamiento,
                                                           CodZonaPla = zonas.CodZonaPla,
                                                           descripcion = zonas.CodZonaPla + " - " + zonas.DescripcionZonaPla
                                                       }).OrderBy(x => x.CodZonaPla).ToList();


            Shared.ZonasPlaneamientoAux zona = new Shared.ZonasPlaneamientoAux();

            zona.id_zonaplaneamiento = -1;

            zona.descripcion = "Seleccione zona";
            lista.Insert(0, zona);

            ddlZona1.DataTextField = "descripcion";
            ddlZona1.DataValueField = "id_zonaplaneamiento";
            ddlZona1.DataSource = lista;
            ddlZona1.DataBind();

            ddlZona2.DataTextField = "descripcion";
            ddlZona2.DataValueField = "id_zonaplaneamiento";
            ddlZona2.DataSource = lista;
            ddlZona2.DataBind();

            ddlZona3.DataTextField = "descripcion";
            ddlZona3.DataValueField = "id_zonaplaneamiento";
            ddlZona3.DataSource = lista;
            ddlZona3.DataBind();

            UpdateZonas.Update();

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
                //this.EjecutarScript(updFechas, "showfrmError();");
            }

        }

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
                DGHP_Entities db = new DGHP_Entities();
                List<SGI.Model.Shared.ItemPartidasHorizontales> resu = new List<Shared.ItemPartidasHorizontales>();

                var elements = new List<SGI.Model.Shared.ItemPartidasHorizontales>();


                elements = (from ubiPartidas in db.Ubicaciones_PropiedadHorizontal
                            join ub in db.Ubicaciones on ubiPartidas.id_ubicacion equals ub.id_ubicacion
                            where ubiPartidas.id_ubicacion == id_ubicacion && ubiPartidas.baja_logica == filtroPHSubGrilla
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
                //}

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
                db.Dispose();
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                btnEliminar.Visible = false;
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
                        ColorFilaSegunEstado(e.Row, id_estado_modif);
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
                //GridView grdHH = (GridView)e.Row.FindControl("grdResultadosH");
                //grdHH.PageIndex = 0;

                LinkButton btnEliminarHor = (LinkButton)e.Row.FindControl("btnEliminarHor");
                //btnEliminarHor.Visible = false;
                LinkButton btnEditarHor = (LinkButton)e.Row.FindControl("btnEditarHor");
                btnEditarHor.Visible = editar;
                LinkButton btnVerHor = (LinkButton)e.Row.FindControl("btnVerHor");
                btnVerHor.Visible = visualizar;

                int id_estado_modif = -1;
                if (int.TryParse(DataBinder.Eval(e.Row.DataItem, "id_estado_modif_ubi_h").ToString(), out id_estado_modif))
                {
                    ColorFilaSegunEstado(e.Row, id_estado_modif);
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
        private void ColorFilaSegunEstado(GridViewRow Row, int id_estado_modif)
        {



            Color colorFila = Color.White;
            Color colorBorde = Color.White;
            bool cambiarColor = false;
            int borde = 0;

            /*EnProceso = 0,
           Anulada = 1,
           Confirmada = 2,
           Aprobada = 3,
           Rechazada = 4*/
            if (id_estado_modif == 0)
            {
                colorFila = ColorTranslator.FromHtml(colorEstadoEnPoceso);
                colorBorde = ColorTranslator.FromHtml(colorBordeEnPoceso);
                borde = 2;
                cambiarColor = true;

            }
            if (id_estado_modif == 2)
            {
                colorFila = ColorTranslator.FromHtml(colorEstadoConfirmada);
                colorBorde = ColorTranslator.FromHtml(colorBordeConfirmada);
                borde = 2;
                cambiarColor = true;

            }



            if (cambiarColor)
            {
                Row.BorderColor = colorBorde;
                Row.BackColor = colorFila;
                Row.BorderWidth = Unit.Pixel(borde);
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

        #region validaciones

        private void Validar()
        {
            // Validar_BuscarPorPartida();
            Validar_BuscarPorUbicacion();


        }

        private void Validar_BuscarPorUbicacion()
        {
            int idAux = 0;
            this.nro_partida_matriz = 0;
            this.nro_partida_horiz = 0;
            this.id_calle = "";
            this.nro_calle = 0;
            this.seccion = 0;
            this.manzana = "";
            this.parcela = "";
            this.id_tipo_ubicacion = -1;
            this.id_sub_tipo_ubicacion = -1;

            //filtro por partida
            if (!string.IsNullOrEmpty(txtUbiNroPartida.Text))
            {
                idAux = 0;
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
                throw new Exception("Cuando especifica el número de puerta debe ingresar la calle.");
            }

            idAux = 0;
            //int.TryParse(txtUbiCalle.Text, out idAux);
            this.id_calle = ddlUbiCalle.SelectedItem.Text;

            idAux = 0;
            int.TryParse(txtUbiNroPuerta.Text.Trim(), out idAux);
            this.nro_calle = idAux;

            //filtro por smp
            idAux = 0;
            int.TryParse(txtUbiSeccion.Text, out idAux);
            this.seccion = idAux;

            this.manzana = txtUbiManzana.Text.Trim();

            this.parcela = txtUbiParcela.Text.Trim();

            //filtro por tipo subtipo
            idAux = -1;
            int.TryParse(ddlbiTipoUbicacion.SelectedItem.Value, out idAux);
            this.id_tipo_ubicacion = idAux;

            idAux = -1;
            int.TryParse(ddlUbiSubTipoUbicacion.SelectedItem.Value, out idAux);
            this.id_sub_tipo_ubicacion = idAux;
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

        private void guardarFiltro()
        {


        }


        private int idPartida = 0;
        private int? id_ubihistcam = -1;
        private int tipo_solicitudcambio = -1;
        private int id_ubicacion = -1;
        private int id_estado_partida = -1;
        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnVer = (LinkButton)sender;
                int id_ubi = 0;
                string[] arg = new string[2];
                arg = btnVer.CommandArgument.ToString().Split(';');
                id_ubi = int.Parse(arg[0]);
                id_estado_partida = int.Parse(arg[1]);
                this.id_ubicacion = id_ubi;
                //cargarSolicitudPartidaMatriz(0, 1, id_ubicacion);
                int? idAux = this.id_ubihistcam;
                CargarDatosOriginales(id_ubi, id_ubi, this.id_ubihistcam);
                hid_id_ubihistcam.Value = id_ubihistcam.ToString();
                hid_id_ubicacion.Value = id_ubicacion.ToString();
                txtTipoSolicitud.Text = cargarTipoSol(1, true);

                updDatos.Update();
                EjecutarScript(updPnlResultadoBuscar, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                EjecutarScript(updPnlResultadoBuscar, "showfrmError();");

            }

        }
        protected void btnVer_ClickHor(object sender, EventArgs e)
        {

            try
            {
                LimpiarCampos(this.pnlDatosHorizon.Controls);

                txtHoriTipoSolicitud.Text = "";
                LinkButton btnVerHor = (LinkButton)sender;
                int id_ubi = 0;
                string[] arg = new string[3];
                arg = btnVerHor.CommandArgument.ToString().Split(';');
                id_ubi = int.Parse(arg[0]);
                int id_parH = int.Parse(arg[1]);

                //int idSol = cargarSolicitudPartidaHorizontal(0, 1, id_ubi, id_parH);

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
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEditar = (LinkButton)sender;
                LinkButton btnEliminar = (LinkButton)sender;
                int id_ubi = 0;
                string[] arg = new string[3];
                arg = btnEditar.CommandArgument.ToString().Split(';');
                int bajaEditar = 1;
                if (arg[1].Equals("baja"))
                {//es baja
                    arg = btnEliminar.CommandArgument.ToString().Split(';');
                    btnEditar = null;
                    bajaEditar = 2;
                }

                id_ubi = int.Parse(arg[0]);

                id_estado_partida = int.Parse(arg[2]);
                this.id_ubicacion = id_ubi;
                cargarSolicitudPartidaMatriz(0, bajaEditar, id_ubicacion);
                int? idAux = this.id_ubihistcam;

                CargarDatos(id_ubi, id_ubi, this.id_ubihistcam);
                hid_id_ubihistcam.Value = id_ubihistcam.ToString();
                hid_id_ubicacion.Value = id_ubicacion.ToString();


                updDatos.Update();
                EjecutarScript(updPnlResultadoBuscar, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                EjecutarScript(updPnlResultadoBuscar, "showfrmError();");

            }

        }
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



                int idSol = cargarSolicitudPartidaHorizontal(0, bajaEditar, id_ubi, id_parH);

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
        private void CargarDatosHorizontal(int id_ubi, int idParHori, int? id_Solic)
        {
            db = new DGHP_Entities();
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
                //txtHoriParcela.Text = w.partidaHor.ToString();
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

                List<SGI.Model.ItemEstadosSolicitud> l = cargarEstadosPosibles();
                ddlHoriEstadosHori.DataTextField = "nombre_estado";
                ddlHoriEstadosHori.DataValueField = "id_estado";
                ddlHoriEstadosHori.DataSource = l;
                ddlHoriEstadosHori.DataBind();
                ddlHoriEstadosHori.SelectedValue = w.id_estado_modif_ubi_h.ToString();
                ddlHoriEstadosHori.Enabled = true;
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


        private bool recuperarFiltro()
        {
            if (ViewState["filtro"] == null)
                return false;
            return true;

        }

        private void LimpiarCampos(ControlCollection Panel)
        {
            db = new DGHP_Entities();

            hid_id_ubicacion.Value = "";
            hid_id_ubihistcam.Value = "";
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
            txtTipoSolicitud.Text = "";
            txtNroSol.Text = "";
            txtNroPartida.Text = "";
            txtSeccion.Text = "";
            txtManzana.Text = "";
            txtParcela.Text = "";
            //ddlCalle.ClearSelection();
            CargarCombo_tipoUbicacion(false);
            CargarCombo_subTipoUbicacion(-1, false);

            chbEntidadGubernamental.Checked = false;
            CargarCombos_ZonasDeslindes();

            txtObservaciones.Text = "";
            DataTable dt = dtUbicacionesCargadas();
            dt.Rows.Clear();
            grdUbicaciones.DataSource = dt;
            grdUbicaciones.DataBind();
            grdHistorial.DataSource = new DataTable();
            grdHistorial.DataBind();
            grdHistorialHori.DataSource = new DataTable();
            grdHistorialHori.DataBind();


        }

        private SGI.Model.Shared.ItemPartidasAux getPartidaMatriz(int idUbi)
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
            if (w.id_ubicacion != null)
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
        protected void btnNuevaPartida_Click(object sender, EventArgs e)
        {
            try
            {
                CargarCombo_tipoUbicacion(false);
                CargarCombo_subTipoUbicacion(-1, false);
                CargarCombos_ZonasDeslindes();
                CargarCombos_Barrios();
                CargarCombos_Comisaria();
                LimpiarCampos(this.pnlDatos.Controls);
                List<SGI.Model.ItemEstadosSolicitud> l = cargarEstadosPosibles();
                ddlEstados.DataTextField = "nombre_estado";
                ddlEstados.DataValueField = "id_estado";
                ddlEstados.DataSource = l;
                ddlEstados.DataBind();
                txtTipoSolicitud.Text = cargarTipoSol(0, true);
                int id_Solic = cargarSolicitudPartidaMatriz(0, 0, id_ubicacion);
                var lstHistorial = (from hist in db.Ubicaciones_Historial_Cambios_UsuariosIntervinientes
                                    join usu in db.aspnet_Users on hist.Userid equals usu.UserId
                                    join usuAdi in db.Usuario on usu.UserId equals usuAdi.UserId into zr
                                    from fd in zr.DefaultIfEmpty()
                                    where id_Solic == hist.id_ubihistcam
                                    select new
                                    {
                                        UserName = usu.UserName,
                                        Apenom = fd.Apellido + fd.Nombre,
                                        hist.LastUpdateDate

                                    }
                                  ).Distinct().ToList();
                grdHistorial.DataSource = lstHistorial;
                grdHistorial.DataBind();
                updDatos.Update();
                EjecutarScript(btn_BuscarPartida, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                EjecutarScript(updPnlResultadoBuscar, "showfrmError();");

            }

        }
        protected void btnNuevaPartidaHori_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarCampos(this.pnlDatosHorizon.Controls);
                txtHoriTipoSolicitud.Text = cargarTipoSol(0, false);
                LinkButton btnNuevaPartidaHori = (LinkButton)sender;
                int id_ubi = 0;
                string[] arg = new string[1];
                arg = btnNuevaPartidaHori.CommandArgument.ToString().Split(';');
                id_ubi = int.Parse(arg[0]);
                int idSol = cargarSolicitudPartidaHorizontal(0, 0, id_ubi, 0);
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
        private int cargarSolicitudPartidaHorizontal(int pIdSolicitud, int pTipoSolicitud, int pIdUbicacion, int pIdPropiedadHorinzontal)
        {
            IniciarEntity();
            Guid userid = Functions.GetUserId();


            if (pIdSolicitud == 0)//si es alta
            {

                try
                {
                    int? idSol = db.SGI_Ubicaciones_PropiedadHorizontal_GenerarSolicitudCambio(pIdUbicacion, pIdPropiedadHorinzontal, pTipoSolicitud, userid).FirstOrDefault();
                    //txtHoriNroSol.Text = idSol.ToString();
                    return idSol.Value;
                }
                catch (Exception ex)
                {

                    throw new Exception("Se ha producido un error generando la solicitud de cambio de propiedad horizontal.", ex);
                }

            }
            return -1;



        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnGuardar = (LinkButton)sender;
                int id_estado_modif = Convert.ToInt32(ddlEstados.SelectedValue);
                int id_ubiSol = -1;
                int id_ubiUbi = 0;
                int.TryParse(hid_id_ubicacion.Value, out id_ubiUbi);
                int.TryParse(this.txtNroSol.Text.Trim(), out id_ubiSol);


                guardarSolicitud(id_estado_modif, id_ubiSol);
                guardarZonasComplementarias(id_ubiSol, id_ubiUbi);
                guardarPuertas(id_ubiSol, id_ubiUbi);


                db = new DGHP_Entities();

                Guid userid = Functions.GetUserId();
                if (this.estadoAprobada == id_estado_modif)
                    db.Ubicaciones_SolicitudCambio_ActualizarUbicaciones(id_ubiSol, userid);

                Validar_BuscarPorUbicacion();
                filtrar = true;
                grdResultados.DataBind();
                updPnlResultadoBuscar.Update();



                LimpiarCampos(this.pnlDatos.Controls);

                EjecutarScript(updBotonesGuardar, "showBusqueda();");

            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updBotonesGuardar, "showfrmError();");

            }
        }
        protected void btnHoriGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnHoriGuardar = (LinkButton)sender;
                int id_estado_modif = Convert.ToInt32(ddlHoriEstadosHori.SelectedValue);
                int id_ubiSol = -1;
                int.TryParse(this.txtHoriNroSol.Text.Trim(), out id_ubiSol);


                //Nro Partida Horizontal
                int? UbiPartidaHorValue = null;
                int UbiPartidaHor = 0;
                int.TryParse(txtHoriNroPartidaHor.Text.Trim(), out UbiPartidaHor);

                if (UbiPartidaHor > 0)
                    UbiPartidaHorValue = UbiPartidaHor;

                Guid userid = Functions.GetUserId();

                db = new DGHP_Entities();

                db.Ubicaciones_PropiedadHorizontal_SolicitudCambio_Actualizar(id_ubiSol, 0, UbiPartidaHorValue, this.txtHoriPiso.Text, this.txtHoriDepto.Text, this.txtHoriDepto.Text, this.txtHoriObservaciones.Text, this.chbHoriEntidadGubernamental.Checked, id_estado_modif, this.txtHoriObservacionesEst.Text, userid);
                //Ubicaciones_SolicitudCambio_ActualizarUbicaciones
                updDatos.Update();
                if (this.estadoAprobada == id_estado_modif)

                    db.Ubicaciones_PropiedadHorizontal_SolicitudCambio_ActualizarPropiedadHorizontal(id_ubiSol, userid);


                Validar_BuscarPorUbicacion();
                filtrar = true;
                grdResultados.DataBind();
                updPnlResultadoBuscar.Update();

                LimpiarCampos(this.pnlDatos.Controls);

                EjecutarScript(updHoriBotonesGuardar, "showBusqueda();");

            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updHoriBotonesGuardar, "showfrmError();");

            }
        }
        private void guardarPuertas(int? idSol, int idUbi)
        {

            db.Ubicaciones_SolicitudCambio_EliminarPuertas(idSol, Functions.GetUserId());
            foreach (GridViewRow row in grdUbicaciones.Rows)
            {

                string cccc = HttpUtility.HtmlDecode(row.Cells[0].Text);
                int codCalle = this.db.Calles.Where(x => x.NombreOficial_calle == cccc).Select(x => x.Codigo_calle).First();
                db.Ubicaciones_SolicitudCambio_AgergarPuertas(idSol, idUbi, "", codCalle,
                    Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[1].Text)), Functions.GetUserId());
            }
        }

        private void guardarZonasComplementarias(int? idSol, int idUbi)
        {
            db.Ubicaciones_SolicitudCambio_EliminarZonas(idSol, Functions.GetUserId());
            int id_zonap = 0;
            string tipo_ubicacion = "";
            int i = 0;

            for (i = 2; i <= 6; i++)
            {
                id_zonap = 0;
                tipo_ubicacion = "";
                switch (i)
                {
                    case 2:
                        if (ddlZona2.SelectedIndex > 1)
                        {
                            id_zonap = int.Parse(ddlZona2.SelectedValue);
                            tipo_ubicacion = "Z2";
                        }
                        break;
                    case 3:
                        if (ddlZona3.SelectedIndex > 1)
                        {
                            id_zonap = int.Parse(ddlZona3.SelectedValue);
                            tipo_ubicacion = "Z3";
                        }
                        break;
                }

                //Se dan de alta las zonas si se completo el combo correspondiente.
                if (id_zonap > 0)
                    db.Ubicaciones_SolicitudCambio_AgergarZonas(idSol, idUbi, tipo_ubicacion, id_zonap, Functions.GetUserId());
            }

        }

        private void guardarSolicitud(int id_estado_modif, int? idSol)
        {
            bool Baja_Logica = false;

            if (this.rbtnBajaSi.Checked == true && this.rbtnBajaNo.Checked == false)
                Baja_Logica = true;
            if (this.rbtnBajaNo.Checked == true && this.rbtnBajaSi.Checked == false)
                Baja_Logica = false;

            //Subtipo Ubi
            int UbiSubTipo = 0;
            int.TryParse(ddlUbiSubTipoUbicacionABM.SelectedValue, out UbiSubTipo);


            //Nro Partida Matriz
            int? UbiNroPartidaValue = null;
            int UbiNroPartida = 0;
            int.TryParse(txtNroPartida.Text.Trim(), out UbiNroPartida);

            if (UbiNroPartida > 0)
                UbiNroPartidaValue = UbiNroPartida;

            //Seccion
            int? UbiSeccionValue = null;
            int UbiSeccion = 0;
            int.TryParse(txtSeccion.Text.Trim(), out UbiSeccion);
            if (UbiSeccion > 0)
                UbiSeccionValue = UbiSeccion;


            //Validamos la creacion de una nueva partida
            var UbiSol = (from u in db.Ubicaciones_Historial_Cambios.Where(x => x.id_ubihistcam == idSol)
                          select u).FirstOrDefault();

            if (UbiSol != null && UbiSol.tipo_solicitud == 0 &&
                UbiNroPartidaValue.HasValue && UbiSeccionValue.HasValue &&
                this.txtManzana.Text.Trim().Length > 0 && this.txtParcela.Text.Trim().Length > 0)
            {
                var existe = (from ubic in db.Ubicaciones
                              where ubic.NroPartidaMatriz == UbiNroPartidaValue.Value
                                && ubic.Seccion == UbiSeccionValue.Value
                                && ubic.Manzana == this.txtManzana.Text.Trim()
                                && ubic.Parcela == this.txtParcela.Text.Trim()
                              select ubic.id_ubicacion);

                if (existe != null && existe.Count() > 0)
                {
                    frmerrortitle.Text = "Advertencia";
                    throw new Exception("Ya existe una partida con la misma Seccion, Manzana, Parcela y NroPartidaMatriz");
                }


            }

            int id_comisaria = int.Parse(ddlComisaria.SelectedValue);
            int id_barrio = int.Parse(ddlBarrio.SelectedValue);

            db.Ubicaciones_SolicitudCambio_Actualizar(idSol, UbiSubTipo, UbiNroPartidaValue, UbiSeccionValue, this.txtManzana.Text.Trim(), this.txtParcela.Text.Trim(),
                                                      this.chbEntidadGubernamental.Checked, this.chbEdificioProtegido.Checked, Baja_Logica, int.Parse(ddlZona1.SelectedValue), this.txtObservaciones.Text.Trim(),
                                                      id_estado_modif, this.txtObservacionesEst.Text.Trim(), Functions.GetUserId(), id_comisaria, id_barrio);

        }

        private void mostrarExportarDatosCabecera(bool mostrar)
        {


        }

        #endregion

        #region permisos

        private bool editar;
        private bool visualizar;
        private bool aprobar;
        private bool bajar;
        private void cargarPermisos()
        {
            db = new DGHP_Entities();
            Guid userid = Functions.GetUserId();

            var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

            foreach (var perfil in perfiles_usuario)
            {
                var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

                if (menu_usuario.Contains("Visualizar Partidas"))
                    visualizar = true;
                if (menu_usuario.Contains("Editar Partidas"))
                {
                    editar = true;
                    bajar = true;
                }
                if (menu_usuario.Contains("Aprobar edición de partidas"))
                    aprobar = true;

            }
        }

        #endregion

        //ABM partidas
        private int cargarSolicitudPartidaMatriz(int pIdSolicitud, int pTipoSolicitud, int pIdUbicacion)
        {
            IniciarEntity();
            Guid userid = Functions.GetUserId();
            this.id_ubihistcam = pIdSolicitud;
            tipo_solicitudcambio = pTipoSolicitud;
            id_ubicacion = pIdUbicacion;
            if (pIdSolicitud == 0)
            {
                try
                {
                    int? idSol = db.SGI_Ubicaciones_GenerarSolicitudCambio(pIdUbicacion, pTipoSolicitud, userid).FirstOrDefault();
                    id_ubihistcam = idSol;
                    hid_id_ubihistcam.Value = id_ubihistcam.ToString();

                    txtNroSol.Text = id_ubihistcam.ToString();
                    return idSol.Value;
                }
                catch (Exception ex)
                {
                    throw new Exception("Se ha producido un error generando la solicitud de cambio de partida matriz.", ex);
                }
            }
            return -1;
        }
        Boolean activas = false;
        protected void btnSolActivas_Click(object sender, EventArgs e)
        {
            try
            {
                activas = true;
                GrdActivas.DataBind();
                updatePanelActivas.Update();

                panelActivas.Visible = true;

                EjecutarScript(btn_BuscarPartida, "showActivas();");
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                LogError.Write(ex, "error al buscar Partidas buscar_Partidas-btnBuscar_OnClick");
                Enviar_Mensaje(ex.Message, "");
            }
        }
        public List<SGI.Model.Shared.ItemSolicitudesAux> GetActivas(int? startRowIndex, int? maximumRows, out int totalRowCount, string sortByExpression)
        {
            db = new DGHP_Entities();
            totalRowCount = 0;

            if (!activas)
            {
                panelActivas.Visible = false;
                updPnlResultadoBuscar.Update();
                return null;
            }

            List<SGI.Model.Shared.ItemSolicitudesAux> resultados = new List<SGI.Model.Shared.ItemSolicitudesAux>();
            var h = (from ubicacion in db.Ubicaciones_Historial_Cambios
                     where ubicacion.id_estado_modif == 2 || ubicacion.id_estado_modif == 0
                     select new SGI.Model.Shared.ItemSolicitudesAux
                     {
                         direccionAct = "",
                         id_solAct = ubicacion.id_ubihistcam,
                         id_estado_sol_id = ubicacion.id_estado_modif,
                         id_estado_sol = "",
                         id_tipoSolActI = ubicacion.tipo_solicitud,
                         tipoPartidaAct = "Matriz",
                         partidaAct = ubicacion.NroPartidaMatriz,
                         obsAct = ubicacion.observaciones_ubihistcam,
                         idUbis = ubicacion.id_ubicacion,
                         id_tipoSolAct = "",
                         Seccion = ubicacion.Seccion,
                         Manzana = ubicacion.Manzana,
                         Parcela = ubicacion.Parcela
                     }
                        ).ToList();

            var y = (from ubicacion in db.Ubicaciones_PropiedadHorizontal_Historial_Cambios
                     where ubicacion.id_estado_modif == 2 || ubicacion.id_estado_modif == 0
                     select new SGI.Model.Shared.ItemSolicitudesAux
                     {
                         direccionAct = "",
                         id_solAct = ubicacion.id_phhistcam,
                         id_estado_sol_id = ubicacion.id_estado_modif,
                         id_estado_sol = "",
                         id_tipoSolActI = ubicacion.tipo_solicitud,
                         tipoPartidaAct = "Horizontal",
                         partidaAct = ubicacion.NroPartidaHorizontal,
                         obsAct = ubicacion.observaciones_phhistcam,
                         idUbis = ubicacion.id_ubicacion,
                         id_tipoSolAct = "",
                         Seccion = null,
                         Manzana = "",
                         Parcela = ""
                     }
                       ).ToList();
            var lista = h.Union(y).OrderBy(x => x.id_solAct).ToList();
            //------------------------------------------------------------------------
            //Rellena la clase a devolver con los datos que faltaban (Direccion,tarea)
            //------------------------------------------------------------------------
            foreach (var row in lista)
            {
                bool tipo = true;
                if (row.tipoPartidaAct.Equals("Horizontal"))
                    tipo = false;
                int[] arrUbicacionesInt;
                if (tipo)
                    arrUbicacionesInt = db.Ubicaciones_Puertas_Historial_Cambios.Where(x => x.id_ubihistcam.Equals(row.id_solAct)).Select(s => s.id_ubicacion).ToArray();
                else
                    arrUbicacionesInt = db.Ubicaciones_Puertas.Where(x => x.id_ubicacion.Equals(row.idUbis)).Select(s => s.id_ubicacion).ToArray();
                string[] arrUbicaciones = new string[arrUbicacionesInt.Count()];
                for (int i = 0; i < arrUbicacionesInt.Count(); i++)
                {
                    arrUbicaciones[i] = (arrUbicacionesInt[i]).ToString();
                }
                List<ItemDireccion> lstDirecciones = Shared.getDirecciones(arrUbicaciones);
                var itemDireccion = lstDirecciones.FirstOrDefault(x => x.id_ubicacion == row.idUbis);

                row.id_tipoSolAct = cargarTipoSol(row.id_tipoSolActI, tipo);
                row.id_estado_sol = this.getNombreEstado(row.id_estado_sol_id);
                if (itemDireccion != null)
                    row.direccionAct = (string.IsNullOrEmpty(itemDireccion.direccion) ? "" : itemDireccion.direccion);
            }
            resultados = lista;
            pnlCantidadRegistros.Visible = (resultados.Count > 0);
            lblCantidadRegistros.Text = resultados.Count.ToString();

            db.Dispose();

            return resultados;
        }
        protected void btnEditarActivas_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEditarActivas = (LinkButton)sender;
                int id_ubi = -1;
                int id_partidaH = -1;
                int idSolicitud = -1;
                int idTipo = -1;
                string[] arg = new string[3];
                arg = btnEditarActivas.CommandArgument.ToString().Split(';');
                idSolicitud = int.Parse(arg[0]);
                idTipo = int.Parse(arg[1]);
                string horiOmatriz = arg[2];
                string direc = arg[3];
                bool tipohoriOmatriz = true;
                if (horiOmatriz.Equals("Horizontal"))
                    tipohoriOmatriz = false;
                this.id_estado_partida = this.estadoConfirmada;
                hid_id_ubihistcam.Value = idSolicitud.ToString();
                this.id_ubihistcam = idSolicitud;
                this.LimpiarCampos(pnlDatos.Controls);
                if (tipohoriOmatriz)
                {
                    CargarDatos(id_ubi, id_partidaH, idSolicitud);
                    txtTipoSolicitud.Text = cargarTipoSol(idTipo, tipohoriOmatriz);
                    updDatos.Update();
                    EjecutarScript(updatePanelActivas, "showDatos();");
                }
                else
                {
                    txtHoriTipoSolicitud.Text = cargarTipoSol(idTipo, tipohoriOmatriz);
                    this.CargarDatosHorizontal(id_ubi, id_partidaH, idSolicitud);
                    updDatosHorizon.Update();
                    EjecutarScript(updatePanelActivas, "showDatosHorizon();");
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                EjecutarScript(updatePanelActivas, "showfrmError();");
            }
        }

        private void CargarDatosOriginales(int id_ubi, int idParHori, int? id_Solic)
        {
            db = new DGHP_Entities();

            var w = (from ubicacion in db.Ubicaciones
                     where ubicacion.id_ubicacion == id_ubicacion
                     select new
                     {
                         manzana = ubicacion.Manzana,
                         seccion = ubicacion.Seccion,
                         parcela = ubicacion.Parcela,
                         nroPartida = ubicacion.NroPartidaMatriz,
                         id_ubicacion = ubicacion.id_ubicacion,
                         subtipo = ubicacion.id_subtipoubicacion,
                         zonaPla = ubicacion.id_zonaplaneamiento,
                         obsZ = ubicacion.Observaciones,
                         chbEntidad = ubicacion.EsEntidadGubernamental,
                         chbEdificio = ubicacion.EsUbicacionProtegida,
                         chbBajaLogica = ubicacion.baja_logica,
                     }
                      ).FirstOrDefault();

            if (w != null)
            {
                editable = false;
                txtManzana.Text = w.manzana;
                txtManzana.Enabled = editable;
                txtParcela.Text = w.parcela;
                txtNroSol.Text = "";
                txtParcela.Enabled = editable;
                txtNroPartida.Enabled = editable;
                txtSeccion.Enabled = editable;
                int? a = w.nroPartida;
                if (a != null)
                    txtNroPartida.Text = a.ToString();

                int? b = w.seccion;
                if (b != null)
                    txtSeccion.Text = b.ToString();

                chbEntidadGubernamental.Checked = w.chbEntidad;
                chbEntidadGubernamental.Enabled = editable;
                chbEdificioProtegido.Checked = w.chbEdificio;
                chbEdificioProtegido.Enabled = editable;

                if (w.chbBajaLogica == true)
                {
                    rbtnBajaSi.Checked = true;
                    rbtnBajaNo.Checked = false;
                }
                if (w.chbBajaLogica == false)
                {
                    rbtnBajaSi.Checked = false;
                    rbtnBajaNo.Checked = true;
                }
                rbtnBajaNo.Enabled = editable;
                rbtnBajaSi.Enabled = editable;

                CargarCombo_tipoUbicacion(false);

                if (w.subtipo != null)
                {
                    var tipo = (from st in this.db.SubTiposDeUbicacion
                                join tip in db.TiposDeUbicacion on st.id_tipoubicacion equals tip.id_tipoubicacion
                                where st.id_subtipoubicacion == w.subtipo
                                select new
                                {
                                    idTipoUbi = tip.id_tipoubicacion
                                }).First();
                    ddlbiTipoUbicacionABM.SelectedValue = tipo.idTipoUbi.ToString();
                    CargarCombo_subTipoUbicacion(tipo.idTipoUbi, false);
                    ddlUbiSubTipoUbicacionABM.SelectedValue = w.subtipo.ToString();
                    ddlbiTipoUbicacionABM.Enabled = editable;
                    ddlUbiSubTipoUbicacionABM.Enabled = editable;
                }

                CargarCombos_ZonasDeslindes();
                ddlZona1.SelectedValue = w.zonaPla.ToString();
                ddlZona1.Enabled = editable;

                var ff = (from ubiZonas in db.Ubicaciones_ZonasComplementarias
                          join zonaW in db.Zonas_Planeamiento on ubiZonas.id_zonaplaneamiento equals zonaW.id_zonaplaneamiento
                          where id_ubicacion == ubiZonas.id_ubicacion
                          select new SGI.Model.Shared.ItemZonasComp
                          {
                              tipoUbi = ubiZonas.tipo_ubicacion,
                              id_zonaplaneamiento = ubiZonas.id_zonaplaneamiento
                          }).Distinct();

                List<SGI.Model.Shared.ItemZonasComp> resul = ff.ToList();
                foreach (var row in resul)
                {
                    switch (row.tipoUbi)
                    {
                        case "Z2":
                            ddlZona2.SelectedValue = row.id_zonaplaneamiento.ToString();
                            break;
                        case "Z3":
                            ddlZona3.SelectedValue = row.id_zonaplaneamiento.ToString();
                            break;
                    }
                }
                ddlZona2.Enabled = editable;
                ddlZona3.Enabled = editable;
                txtObservaciones.Text = w.obsZ;
                txtObservaciones.Enabled = editable;
                txtObservacionesEst.Text = w.obsZ;
                txtObservacionesEst.Enabled = editable;
                List<SGI.Model.ItemEstadosSolicitud> l = cargarEstadosPosibles();
                ddlEstados.DataTextField = "nombre_estado";
                ddlEstados.DataValueField = "id_estado";
                ddlEstados.DataSource = l;
                ddlEstados.DataBind();
                ddlEstados.SelectedValue = "0";
                //ddlEstados.Enabled = editable;

                divEstado.Visible = editable;

                ddlComisaria.Enabled = editable;
                ddlBarrio.Enabled = editable;

                var lstUbicaciones = (from ubiPuertas in db.Ubicaciones_Puertas
                                      join callesNombre in db.Calles on ubiPuertas.codigo_calle equals callesNombre.Codigo_calle
                                      where id_ubicacion == ubiPuertas.id_ubicacion
                                        && ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaIzquierdaInicio_calle
                                        && ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaIzquierdaFin_calle
                                      select new
                                      {
                                          calles = callesNombre.NombreOficial_calle,
                                          nroPuerta = ubiPuertas.NroPuerta_ubic
                                      }).Distinct().ToList();

                grdUbicaciones.DataSource = lstUbicaciones;
                grdUbicaciones.DataBind();
                btnAgregarUbicacion.Visible = editable;
                btnGuardar.Visible = editable;
                btnNuevaBusqueda.Visible = editable;
                var lstHistorial = (from hist in db.Ubicaciones_Historial_Cambios_UsuariosIntervinientes
                                    join usu in db.aspnet_Users on hist.Userid equals usu.UserId
                                    join usuAdi in db.Usuario on usu.UserId equals usuAdi.UserId into zr
                                    from fd in zr.DefaultIfEmpty()
                                    where id_Solic == hist.id_ubihistcam
                                    select new
                                    {
                                        UserName = usu.UserName,
                                        Apenom = fd.Apellido + fd.Nombre,
                                        hist.LastUpdateDate
                                    }
                                   ).Distinct().ToList();
                grdHistorial.DataSource = lstHistorial;
                grdHistorial.DataBind();
            }
            db.Dispose();
        }
        private void CargarDatosHorizontalOriginales(int id_ubi, int idParHori, int? id_Solic)
        {
            db = new DGHP_Entities();
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
                txtHoriNroSol.Text = "";
                chbHoriEntidadGubernamental.Checked = w.chbEntidad;
                chbHoriEntidadGubernamental.Enabled = editable;
                txtHoriNroPartidaHor.Text = (w.partidaHor != null) ? w.partidaHor.ToString() : "";
                txtHoriNroPartidaHor.Enabled =editable;
                txtHoriObservaciones.Text = w.obsZ;
                txtHoriObservaciones.Enabled = editable;

                List<SGI.Model.ItemEstadosSolicitud> l = cargarEstadosPosibles();
                ddlHoriEstadosHori.DataTextField = "nombre_estado";
                ddlHoriEstadosHori.DataValueField = "id_estado";
                ddlHoriEstadosHori.DataSource = l;
                ddlHoriEstadosHori.DataBind();
                ddlHoriEstadosHori.SelectedValue = "0";
                ddlHoriEstadosHori.Enabled = editable;
                divObsH.Visible = false;
                txtHoriObservacionesEst.Text = "";
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
    }
}
