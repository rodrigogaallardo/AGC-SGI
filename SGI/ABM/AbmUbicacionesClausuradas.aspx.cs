using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SGI.Model;
using System.Transactions;
using System.Drawing;
using System.Data;
using System.Data.Entity.Core.Objects;
using SGI.Webservices.Pagos;


namespace SGI.ABM
{
    public partial class AbmUbicacionesClausuradas : BasePage
    {
        private int nro_partida_matriz = 0;
        private int nro_partida_horiz = 0;
        private int id_calle = 0;
        private int nro_calle = 0;

        private int seccion = 0;
        private string manzana = "";
        private string parcela = "";
        private int id_tipo_ubicacion = -1;
        private int id_sub_tipo_ubicacion = -1;


        private bool hayFiltroPorUbicacion = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm.IsInAsyncPostBack)
            {

                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_ubi_partida, updPnlFiltroBuscar_ubi_partida.GetType(),
                    "inicializar_controles", "inicializar_controles();", true);
                ScriptManager.RegisterStartupScript(updmpeInfo, updmpeInfo.GetType(), "inicializar_controles", "inicializar_controles();", true);
            }
            if (!IsPostBack)
            {


                CargarCombo_tipoUbicacion();
                CargarCombo_subTipoUbicacion(-1);
                CargarCalles();

            }
            if (!Functions.ComprobarPermisosPagina("EDITAR_UBICACIONES_CLAUSURADAS"))
            {
                PanelBoton.Visible = false;
            }
        }

        private void Buscador()
        {
            List<clsItemGrillaUbicacionesClausuradas> rq = new List<clsItemGrillaUbicacionesClausuradas>();
            DGHP_Entities db = new DGHP_Entities();
            List<int> qENC = new List<int>();
            //búsqueda por numero partida matriz
            Validar_BuscarPorUbicacion();
            //if (hayFiltroPorUbicacion)

            var q = (from ubic in db.Ubicaciones_Clausuras
                     join ubi in db.Ubicaciones on ubic.id_ubicacion equals ubi.id_ubicacion
                     join dirc in db.Ubicaciones_DireccionesConformadas on ubi.id_ubicacion equals dirc.id_ubicacion
                     select new clsItemGrillaUbicacionesClausuradas()
                     {
                         Tipo = (ubi.NroPartidaMatriz == null && ubi.Seccion == null && ubi.Manzana == "" && ubi.Parcela == "") ? "UE" : "PM",
                         id_ubicacion = ubi.id_ubicacion,
                         id_ubicclausura = ubic.id_ubicclausura,
                         NroPartidaMatriz = (int)ubi.NroPartidaMatriz,
                         Seccion = (int)ubi.Seccion,
                         Manzana = ubi.Manzana,
                         Parcela = ubi.Parcela,
                         motivo = ubic.motivo,
                         fecha_alta_clausura = ubic.fecha_alta_clausura,
                         fecha_baja_clausura = ubic.fecha_baja_clausura,
                         domicilio = dirc.direccion,
                         id_dgubicacion = ubi.id_ubicacion,
                         

                     }).Union

             (from ubcla in db.Ubicaciones_PropiedadHorizontal_Clausuras
              join prop in db.Ubicaciones_PropiedadHorizontal on ubcla.id_propiedadhorizontal equals prop.id_propiedadhorizontal
              join ubi in db.Ubicaciones on prop.id_ubicacion equals ubi.id_ubicacion
              join dirc in db.Ubicaciones_DireccionesConformadas on ubi.id_ubicacion equals dirc.id_ubicacion
              select new clsItemGrillaUbicacionesClausuradas()
              {
                  Tipo = "PH",
                  id_ubicacion = ubi.id_ubicacion,
                  id_ubicclausura = ubcla.id_ubicphorclausura,
                  NroPartidaMatriz = (int)prop.NroPartidaHorizontal,
                  Seccion = (int)ubi.Seccion,
                  Manzana = ubi.Manzana,
                  Parcela = ubi.Parcela,
                  motivo = ubcla.motivo,
                  fecha_alta_clausura = ubcla.fecha_alta_clausura,
                  fecha_baja_clausura = ubcla.fecha_baja_clausura,
                  domicilio = dirc.direccion,
                  id_dgubicacion = ubi.id_ubicacion
              });

            if (this.nro_partida_matriz > 0)
            {
                q = q.Where(x => x.NroPartidaMatriz == this.nro_partida_matriz);
                //k.Clear();
            }

            //búsqueda por numero partida horizontal
            if (this.nro_partida_horiz > 0)
            {
                q = q.Where(x => x.NroPartidaMatriz == this.nro_partida_horiz);
                //k = k.Where(x => x.NroPartidaMatriz == this.nro_partida_horiz);
                //q.Clear();
            }

            //busqueda por Domicilio
            if (this.id_calle > 0)
            {
                //var f = (from ubic in db.Ubicaciones
                //         join puer in db.Ubicaciones_Puertas on ubic.id_ubicacion equals puer.id_ubicacion
                //         where puer.codigo_calle == this.id_calle
                //         select ubic.id_ubicacion);

                //q = q.Where(x => f.Contains(x.id_ubicacion));
                //k = k.Where(x => f.Contains(x.id_ubicacion));
                if (this.nro_calle > 0)
                {
                    q = (from res in q
                         join ubic in db.Ubicaciones on res.id_ubicacion equals ubic.id_ubicacion
                         join puer in db.Ubicaciones_Puertas on ubic.id_ubicacion equals puer.id_ubicacion
                         where puer.codigo_calle == this.id_calle && puer.NroPuerta_ubic == this.nro_calle
                         select res);
                }
                else
                {
                    q = (from res in q
                         join ubic in db.Ubicaciones on res.id_ubicacion equals ubic.id_ubicacion
                         join puer in db.Ubicaciones_Puertas on ubic.id_ubicacion equals puer.id_ubicacion
                         where puer.codigo_calle == this.id_calle
                         select res);
                }
            }

            //busqueda por Sección / Manzana / Parcela
            if (this.seccion > 0)
            {
                //var smp = (from ubic in db.Ubicaciones
                //         where ubic.Seccion == this.seccion &&
                //        (ubic.Manzana == this.manzana || this.manzana.Length == 0) &&
                //        (ubic.Parcela == this.parcela || this.parcela.Length == 0)
                //         select ubic.id_ubicacion);

                //q = q.Where(x => smp.Contains(x.id_ubicacion));
                //k = k.Where(x => smp.Contains(x.id_ubicacion));
                q = (from res in q
                     join ubic in db.Ubicaciones on res.id_ubicacion equals ubic.id_ubicacion
                     where ubic.Seccion == this.seccion &&
                         (ubic.Manzana == this.manzana || manzana.Length == 0) &&
                         (ubic.Parcela == this.parcela || parcela.Length == 0)
                     select res);
            }

            //busqueda por Ubicaciones Especiales
            if (this.id_sub_tipo_ubicacion > -1)
            {

                //var sububi = (from ubic in db.Ubicaciones
                //         join stipubic in db.SubTiposDeUbicacion on ubic.id_subtipoubicacion equals stipubic.id_subtipoubicacion
                //         join tipubic in db.TiposDeUbicacion on stipubic.id_tipoubicacion equals tipubic.id_tipoubicacion
                //         where tipubic.id_tipoubicacion == this.id_tipo_ubicacion && stipubic.id_subtipoubicacion == this.id_sub_tipo_ubicacion
                //         select ubic.id_ubicacion);

                //q = q.Where(x => sububi.Contains(x.id_ubicacion));
                //k = k.Where(x => sububi.Contains(x.id_ubicacion));
                q = (from res in q
                     join ubic in db.Ubicaciones on res.id_ubicacion equals ubic.id_ubicacion
                     join stipubic in db.SubTiposDeUbicacion on ubic.id_subtipoubicacion equals stipubic.id_subtipoubicacion
                     join tipubic in db.TiposDeUbicacion on stipubic.id_tipoubicacion equals tipubic.id_tipoubicacion
                     where tipubic.id_tipoubicacion == this.id_tipo_ubicacion && stipubic.id_subtipoubicacion == this.id_sub_tipo_ubicacion
                     select res);

            }
            // se hace en dos partes porque existe el registro sin especificar y tanto sin especificar como (Todos) comparten el mismo id.
            // dejar en dos partes el select, con y sin id_subtipoubicacion

            if (this.id_tipo_ubicacion > -1)
            {
                //var tiubi = (from ubic in db.Ubicaciones
                //         join stipubic in db.SubTiposDeUbicacion on ubic.id_subtipoubicacion equals stipubic.id_subtipoubicacion
                //         join tipubic in db.TiposDeUbicacion on stipubic.id_tipoubicacion equals tipubic.id_tipoubicacion
                //         where tipubic.id_tipoubicacion == this.id_tipo_ubicacion
                //         select ubic.id_ubicacion);

                //q = q.Where(x => tiubi.Contains(x.id_ubicacion));
                //k = k.Where(x => tiubi.Contains(x.id_ubicacion));
                q = (from res in q
                     join ubic in db.Ubicaciones on res.id_ubicacion equals ubic.id_ubicacion
                     join stipubic in db.SubTiposDeUbicacion on ubic.id_subtipoubicacion equals stipubic.id_subtipoubicacion
                     join tipubic in db.TiposDeUbicacion on stipubic.id_tipoubicacion equals tipubic.id_tipoubicacion
                     where tipubic.id_tipoubicacion == this.id_tipo_ubicacion
                     select res);
            }

            //q = q.Union(k);

            int totalRowCount = q.Count();
            //q = q.Union(k);
            rq = q.ToList();
            //if (rq.Count > 0)
            //{

            //    List<clsItemGrillaUbicacionesDireccionDGFYCO> lstDirecciones = new List<clsItemGrillaUbicacionesDireccionDGFYCO>();
            //    string[] arrUbicaciones = rq.Select(x => x.id_dgubicacion.ToString()).ToArray();

            //    if (arrUbicaciones.Length > 0)
            //        lstDirecciones = Shared.GetDireccionesDG_Ubicacion(arrUbicaciones);

            //    //------------------------------------------------------------------------
            //    //Rellena la clase a devolver con los datos que faltaban (Direccion)
            //    //------------------------------------------------------------------------
            //    foreach (var row in rq)
            //    {
            //        if (row.id_dgubicacion != 0)
            //        {
            //            clsItemGrillaUbicacionesDireccionDGFYCO itemDireccion = lstDirecciones.FirstOrDefault(x => x.id_dgubicacion == row.id_dgubicacion);
            //            row.domicilio = itemDireccion.direccion;
            //        }
            //    }
            //}
            grdUbicacionesClausuradas.DataSource = rq.ToList();
            grdUbicacionesClausuradas.DataBind();
            pnlResultadoBuscar.Visible = true;//agregados
            updPnlUbicacionesClausuradas.Update();


            pnlCantRegistros.Visible = true;

            //int totalRowCount = q.Count();

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
            //this.EjecutarScript(this, "inicializar_controles();");

            this.EjecutarScript(updPnlUbicacionesClausuradas, "showBusqueda();");


        }
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {

                Buscador();

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "error al buscar elevadores buscar_tramite-btnBuscar_OnClick");
                Enviar_Mensaje(ex.Message, "");
            }
        }

        private void CargarCombo_tipoUbicacion()
        {
            DGHP_Entities db = new DGHP_Entities();
            List<TiposDeUbicacion> lista = db.TiposDeUbicacion.Where(x => x.id_tipoubicacion > 0).ToList();

            TiposDeUbicacion tipo_ubi = new TiposDeUbicacion();
            tipo_ubi.id_tipoubicacion = -1;
            tipo_ubi.descripcion_tipoubicacion = "Ninguno";
            lista.Insert(0, tipo_ubi);

            ddlbiTipoUbicacion.DataTextField = "descripcion_tipoubicacion";
            ddlbiTipoUbicacion.DataValueField = "id_tipoubicacion";

            ddlbiTipoUbicacion.DataSource = lista;
            ddlbiTipoUbicacion.DataBind();

            updPnlFiltroBuscar_ubi_especial.Update();

        }

        protected void BuscarUbicacion_AgregarUbicacionClick(object sender, ref SGI.Controls.BuscarUbicacion.ucAgregarUbicacionEventsArgs e)
        {
            UpdatePanel upd = e.upd;
            try
            {

                hid_ID_ubic.Value = e.id_ubicacion.ToString();
                hid_IDs_horizontal.Value = string.Join(";", e.ids_propiedades_horizontales);
                hid_accion.Value = "insert";

                updhidden.Update();

                this.EjecutarScript(upd, "hidefrmAgregarUbicacion();showfrmDatosUbicacion();");

            }
            catch (Exception ex)
            {
                e.Cancel = true;
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(upd, "hidefrmAgregarUbicacion();hidefrmDatosUbicacion();showfrmError();inicializar_controles();");
            }
        }
        public void Insert(string accion, string id_ubic, string ids_horizontal)
        {
            this.hid_accion.Value = "insert";
            this.hid_ID_ubic.Value = id_ubic;
            this.hid_IDs_horizontal.Value = ids_horizontal;

        }

        private void Modificar(Guid userid, DGHP_Entities db)
        {
            try
            {
                DateTime? FechaBaja;
                if (txtFechaBaja.Text == "")
                {
                    FechaBaja = null;
                }
                else
                {
                    FechaBaja = Convert.ToDateTime(txtFechaBaja.Text);
                }

                //int id_ubicclausura = (int)Session["id_ubicclausura"];
                int id_ubicclausura = Convert.ToInt32(hid_ID_ubicclausura.Value);
                int id_tipo = Convert.ToInt32(hid_ID_tipo.Value);
                //if ((int)Session["Tipo"] == 1)
                if (id_tipo == 1)
                {
                    db.SGI_Agregar_Ubicacion_PropiedadHorizontal_Clausuras(id_ubicclausura, null, txtMotivo.Text, Convert.ToDateTime(txtFechaAlta.Text), FechaBaja, userid);
                }
                else
                {
                    db.SGI_Agregar_Ubicacion_Clausurada(id_ubicclausura, null, txtMotivo.Text, Convert.ToDateTime(txtFechaAlta.Text), FechaBaja, userid);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updmpeInfo, "hidefrmAgregarUbicacion();hidefrmDatosUbicacion();showfrmError();inicializar_controles();");
            }
        }
        protected void Guardar(Guid userid, DGHP_Entities db)
        {
            try
            {
                DateTime? FechaBaja;
                if (txtFechaBaja.Text == "")
                {
                    FechaBaja = null;
                }
                else
                {
                    FechaBaja = Convert.ToDateTime(txtFechaBaja.Text);
                }
                //string[] nose = hid_IDs_horizontal.Value.Split(';');
                List<int> numbers = new List<int>();
                if (!string.IsNullOrEmpty(hid_IDs_horizontal.Value) && hid_IDs_horizontal.Value.Length > 1)
                {
                    numbers = new List<int>(Array.ConvertAll(hid_IDs_horizontal.Value.Split(';'), Convert.ToInt32));
                }
                //List<int> numbers = new List<int>(Array.ConvertAll(hid_IDs_horizontal.Value.Split(';'), int.Parse));
                //if (((List<int>)Session["propHorizontales"]).Count == 0)
                if (numbers.Count() == 0)
                {
                    //int idubic = (int)Session["id_ubic"];
                    int idubic = Convert.ToInt32(hid_ID_ubic.Value);
                    var q = (db.Ubicaciones_Clausuras.FirstOrDefault(x => x.id_ubicacion == idubic));
                    if (q != null && q.fecha_baja_clausura == null)
                    {
                        throw new Exception("Ya esta clausurada la ubicación seleccionada");
                    }
                    db.SGI_Agregar_Ubicacion_Clausurada(null, idubic, txtMotivo.Text, Convert.ToDateTime(txtFechaAlta.Text), FechaBaja, userid);
                }
                else
                {
                    //int idubic = (int)Session["id_ubic"];
                    int idubic = Convert.ToInt32(hid_ID_ubic.Value);
                    //foreach (int id_propiedad_horizontal in ((List<int>)Session["propHorizontales"]))
                    foreach (int id_propiedad_horizontal in numbers)
                    {
                        var q = db.Ubicaciones_PropiedadHorizontal_Clausuras.FirstOrDefault(x => x.id_propiedadhorizontal == id_propiedad_horizontal);
                        if (q == null)
                        {
                            db.SGI_Agregar_Ubicacion_PropiedadHorizontal_Clausuras(null, id_propiedad_horizontal, txtMotivo.Text, Convert.ToDateTime(txtFechaAlta.Text), FechaBaja, userid);
                        }
                    }
                }
                this.EjecutarScript(this, "inicializar_controles();");
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updmpeInfo, "hidefrmAgregarUbicacion();hidefrmDatosUbicacion();showfrmError();inicializar_controles();");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Guid userid = SGI.Functions.GetUserId();
                DGHP_Entities db = new DGHP_Entities();

                if (txtFechaAlta.Text == "")
                { throw new Exception("La fecha de Alta no puede ser vacia"); }

                //if (Session["Modificar"] == null)
                if (hid_accion.Value.Contains("insert"))
                {
                    Guardar(userid, db);
                }
                else if (hid_accion.Value.Contains("update"))
                {
                    Modificar(userid, db);
                }
                //LimpiarHidden();
                Limpiar();
                Buscador();
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updmpeInfo, "hidefrmAgregarUbicacion();hidefrmDatosUbicacion();showfrmError();inicializar_controles();");
            }
        }
        private void CargarCalles()
        {
            DGHP_Entities db = new DGHP_Entities();
            var lstCalles = (from calle in db.Calles
                             select new
                             {
                                 calle.Codigo_calle,
                                 calle.NombreOficial_calle
                             }).Distinct().OrderBy(x => x.NombreOficial_calle).ToList();

            ddlCalles.DataSource = lstCalles;
            ddlCalles.DataTextField = "NombreOficial_calle";
            ddlCalles.DataValueField = "Codigo_calle";
            ddlCalles.DataBind();

            ddlCalles.Items.Insert(0, "");
        }

        private void CargarCombo_subTipoUbicacion(int id_tipoubicacion)
        {
            DGHP_Entities db = new DGHP_Entities();
            List<SubTiposDeUbicacion> lista = db.SubTiposDeUbicacion.Where(x => x.id_tipoubicacion == id_tipoubicacion).ToList();

            SubTiposDeUbicacion sub_tipo_ubi = new SubTiposDeUbicacion();
            sub_tipo_ubi.id_subtipoubicacion = -1;
            sub_tipo_ubi.descripcion_subtipoubicacion = "Ninguno";
            lista.Insert(0, sub_tipo_ubi);

            ddlUbiSubTipoUbicacion.DataTextField = "descripcion_subtipoubicacion";
            ddlUbiSubTipoUbicacion.DataValueField = "id_subtipoubicacion";
            ddlUbiSubTipoUbicacion.DataSource = lista;
            ddlUbiSubTipoUbicacion.DataBind();

        }
        protected void ddlbiTipoUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id_tipoubicacion = int.Parse(ddlbiTipoUbicacion.SelectedValue);
            CargarCombo_subTipoUbicacion(id_tipoubicacion);

            updPnlFiltroBuscar_ubi_especial.Update();

        }
        private void LimpiarHidden()
        {
            hid_accion.Value = null;
            hid_ID_tipo.Value = null;
            hid_ID_ubic.Value = null;
            hid_ID_ubicclausura.Value = null;
            hid_IDs_horizontal.Value = null;
            updhidden.Update();
        }
        private void Limpiar()
        {
            txtMotivo.Text = "";
            txtFechaAlta.Text = "";
            txtFechaBaja.Text = "";

            LimpiarHidden();

            //if (Session["id_ubic"] != null)
            //{ Session.Remove("id_ubic"); }

            //if (Session["propHorizontales"] != null)
            //{ Session.Remove("propHorizontales"); }

            //if (Session["id_ubicclausura"] != null)
            //{ Session.Remove("id_ubicclausura"); }

            //if (Session["Modificar"] != null)
            //{ Session.Remove("Modificar"); }

            //if (Session["Tipo"] != null)
            //{ Session.Remove("Tipo"); }

            ddlCalles.ClearSelection();

            txtUbiNroPartida.Text = "";
            ddlCalles.ClearSelection();
            txtUbiNroPuerta.Text = "";

            txtUbiSeccion.Text = "";
            txtUbiManzana.Text = "";
            txtUbiParcela.Text = "";

            if (ddlbiTipoUbicacion.Items.Count >= 0)
                ddlbiTipoUbicacion.SelectedIndex = 0;

            if (ddlUbiSubTipoUbicacion.Items.Count >= 0)
                ddlUbiSubTipoUbicacion.SelectedIndex = 0;

            updPnlFiltroBuscar_ubi_partida.Update();
            updPnlFiltroBuscar_ubi_dom.Update();
            updPnlFiltroBuscar_ubi_smp.Update();
            updPnlFiltroBuscar_ubi_especial.Update();
            updPnlDatosUbic.Update();
            updPnlUbicacionesClausuradas.Update();
            EjecutarScript(updPnlUbicacionesClausuradas, "hideBusqueda();");
        }

        private void Validar_BuscarPorUbicacion()
        {
            this.hayFiltroPorUbicacion = false;

            int idAux = 0;

            this.nro_partida_matriz = 0;
            this.nro_partida_horiz = 0;

            this.id_calle = 0;
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
                    throw new Exception("Debe indicar si nùmero ingersado corresponde a partida matriz o a partida horizontal.");
                }

            }

            //filtro por domicilio
            if (!string.IsNullOrEmpty(txtUbiNroPuerta.Text) && ddlCalles.SelectedValue == "")
            {
                throw new Exception("Cuando especifica el número de puerta debe ingresar la calle.");
            }

            idAux = 0;
            int.TryParse(ddlCalles.SelectedValue, out idAux);
            this.id_calle = idAux;

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

            if (this.nro_partida_matriz > 0 || this.nro_partida_horiz > 0 ||
                this.id_calle > 0 || this.nro_calle > 0 ||
                this.seccion > 0 || !string.IsNullOrEmpty(this.manzana) || !string.IsNullOrEmpty(this.parcela) ||
                this.id_tipo_ubicacion > -1 || this.id_sub_tipo_ubicacion > -1)
                this.hayFiltroPorUbicacion = true;

        }
        protected void lnkModificar_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            string valor = ((LinkButton)sender).CommandArgument.ToString();
            int indx = valor.IndexOf(",");

            hid_ID_tipo.Value = "0";
            hid_ID_ubicclausura.Value = valor.Substring(0, indx);
            hid_accion.Value = "update";

            int tipo = 0;
            int id_ubicclausura = Convert.ToInt32(valor.Substring(0, indx));
            if (valor.Substring(indx + 1) == "PH")
            {
                tipo = 1;
                hid_ID_tipo.Value = "1";
            }

            updhidden.Update();

            //id_ubicclausura = Convert.ToInt32(((LinkButton)sender).CommandArgument.ToString());
            //Session["Modificar"] = true;
            //Session["Tipo"] = tipo;
            //Session["id_ubicclausura"] = id_ubicclausura;
            if (tipo == 0)
            {
                var q = db.Ubicaciones_Clausuras.Where(x => x.id_ubicclausura == id_ubicclausura).FirstOrDefault();
                txtMotivo.Text = q.motivo;
                txtFechaAlta.Text = q.fecha_alta_clausura.ToShortDateString();
                txtFechaBaja.Text = (q.fecha_baja_clausura == null) ? "" : q.fecha_baja_clausura.Value.ToShortDateString();
            }
            else
            {
                var q = db.Ubicaciones_PropiedadHorizontal_Clausuras.Where(x => x.id_ubicphorclausura == id_ubicclausura).FirstOrDefault();
                txtMotivo.Text = q.motivo;
                txtFechaAlta.Text = q.fecha_alta_clausura.ToShortDateString();
                txtFechaBaja.Text = (q.fecha_baja_clausura == null) ? "" : q.fecha_baja_clausura.Value.ToShortDateString();
            }

            this.EjecutarScript(updPnlDatosUbic, "showfrmDatosUbicacion();inicializar_controles();");
            updPnlDatosUbic.Update();
        }

        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            Guid userid = SGI.Functions.GetUserId();
            DGHP_Entities db = new DGHP_Entities();

            string valor = ((LinkButton)sender).CommandArgument.ToString();
            int indx = valor.IndexOf(",");

            hid_ID_tipo.Value = "0";
            hid_ID_ubicclausura.Value = valor.Substring(0, indx);
            hid_accion.Value = "update";

            int tipo = 0;
            int id_ubicclausura = Convert.ToInt32(valor.Substring(0, indx));
            if (valor.Substring(indx + 1) == "PH")
            {
                tipo = 1;
                hid_ID_tipo.Value = "1";
            }
            updhidden.Update();

            if (tipo == 1)
            {
                db.SGI_Eliminar_Ubicacion_PropiedadHorizontal_Clausuras(id_ubicclausura);
            }
            else
            {
                //db.SGI_Eliminar_Ubicacion_Clausurada(id_ubicinhibida);
                db.SGI_Eliminar_Ubicacion_Clausurada(id_ubicclausura);
            }
            Buscador();
            updPnlUbicacionesClausuradas.Update();
            //Session["Tipo"] = tipo;
            //Session["id_ubicinhibida"] = id_ubicinhibida;
            //Eliminar();

        }
        //private void Eliminar()
        //{
        //    DGHP_Entities db = new DGHP_Entities();
        //    int id_ubicclausura = (int)Session["id_ubicclausura"];

        //    if ((int)Session["Tipo"] == 1)
        //    {
        //        db.SGI_Eliminar_Ubicacion_PropiedadHorizontal_Clausuras(id_ubicclausura);
        //    }
        //    else
        //    {
        //        db.SGI_Eliminar_Ubicacion_Clausurada(id_ubicclausura);
        //    }
        //    Buscador();
        //    updPnlUbicacionesClausuradas.Update();
        //}

        protected void grdUbicacionesClausuradas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                if (!Functions.ComprobarPermisosPagina("EDITAR_UBICACIONES_CLAUSURADAS"))
                {
                    LinkButton BotonModificar = (LinkButton)e.Row.FindControl("lnkModificar");
                    LinkButton BotonEliminar = (LinkButton)e.Row.FindControl("lnkEliminar");
                    BotonModificar.Enabled = false;
                    BotonEliminar.Enabled = false;
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }


        protected void cmdPage(object sender, EventArgs e)
        {
            Button obj = (Button)sender;
            grdUbicacionesClausuradas.PageIndex = Convert.ToInt16(obj.Text) - 1;
            Buscador();
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdUbicacionesClausuradas.PageIndex = grdUbicacionesClausuradas.PageIndex + 1;
            Buscador();
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdUbicacionesClausuradas.PageIndex = grdUbicacionesClausuradas.PageIndex - 1;
            Buscador();
        }

        protected void grdResultados_DataBound(object sender, EventArgs e)
        {
            GridView grid = grdUbicacionesClausuradas;
            GridViewRow fila = (GridViewRow)grid.BottomPagerRow;

            if (fila != null)
            {
                Button btnAnterior = (Button)fila.Cells[0].FindControl("cmdAnterior");
                Button btnSiguiente = (Button)fila.Cells[0].FindControl("cmdSiguiente");

                if (grid.PageIndex == 0)
                    btnAnterior.Visible = false;
                else
                {
                    btnAnterior.Visible = true;
                    btnAnterior.Width = Unit.Parse("100px");
                    btnAnterior.Height = Unit.Parse("40px");
                }

                if (grid.PageIndex == grid.PageCount - 1)
                    btnSiguiente.Visible = false;
                else
                {
                    btnSiguiente.Visible = true;
                    btnSiguiente.Width = Unit.Parse("100px");
                    btnSiguiente.Height = Unit.Parse("40px");
                }


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
                            if (i + 1 < 100)     // Esto es para cuando el botón va de 1 a 9 inclusive no sea tan chico
                            {
                                btn.Width = Unit.Parse("40px");
                                btn.Height = Unit.Parse("40px");
                            }
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
                    // Linea Original "Previa al cambio": for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                    for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 5; i--)
                    {
                        CantBucles++;
                        if (i >= 0)
                        {
                            Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 - CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                            if (i + 1 < 100)             // Esto es para cuando el botón va de 1 a 9 inclusive no sea tan chico
                            {
                                btn.Width = Unit.Parse("40px");
                                btn.Height = Unit.Parse("40px");
                            }
                        }

                    }

                    CantBucles = 0;
                    // Ubica los 9 botones hacia la derecha
                    // Linea Original "Previa al cambio": for (int i = grid.PageIndex - 1; i <= grid.PageIndex - 9; i--)
                    for (int i = grid.PageIndex + 1; i <= grid.PageIndex + 5; i++)
                    {
                        CantBucles++;
                        if (i <= grid.PageCount - 1)
                        {
                            Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 + CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                            if (i + 1 < 100)     // Esto es para cuando el botón va de 1 a 9 inclusive no sea tan chico
                            {
                                btn.Width = Unit.Parse("40px");
                                btn.Height = Unit.Parse("40px");
                            }
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
                    {
                        cmdPage.Width = Unit.Parse("40px");
                        cmdPage.Height = Unit.Parse("40px");
                        cmdPage.CssClass = "btn btn-xs btn-default";
                    }
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

        protected void grdResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdUbicacionesClausuradas.PageIndex = e.NewPageIndex;
            Buscador();
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
                ScriptManager.RegisterStartupScript(btn_BuscarTramite, btn_BuscarTramite.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
            }

        }
    }
}