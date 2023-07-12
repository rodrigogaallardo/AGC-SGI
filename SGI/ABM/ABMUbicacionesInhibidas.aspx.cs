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
using System.IO;

namespace SGI.ABM
{
    public partial class ABMUbicacionesInhibidas : BasePage
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

                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_ubi_partida, updPnlFiltroBuscar_ubi_partida.GetType(), "inicializar_controles", "inicializar_controles();", true);
                ScriptManager.RegisterStartupScript(updmpeInfo, updmpeInfo.GetType(), "inicializar_controles", "inicializar_controles();", true);
            }
            if (!IsPostBack)
            {
                if (Request.Cookies["AbmUbicacionesInhibidas_IdCalle"] != null)
                {
                    AutocompleteCalles.SelectValueByKey = string.Empty;
                }

                CargarCombo_tipoUbicacion();
                CargarCombo_subTipoUbicacion(-1);
            }
            CargarCalles();//ASOSA SYNCFUSION Poner fuera del postbak
            if (!Functions.ComprobarPermisosPagina("EDITAR_UBICACIONES_INHIBIDAS"))
            {
                lnkAgregarUbicacion.Visible = false;
            }
        }

        private void Buscador()
        {

            List<clsItemGrillaUbicacionesInhibidas> rq = new List<clsItemGrillaUbicacionesInhibidas>();
            DGHP_Entities db = new DGHP_Entities();
            List<int> qENC = new List<int>();
            //búsqueda por numero partida matriz
            Validar_BuscarPorUbicacion();
            //if (hayFiltroPorUbicacion)
            {
                var q = (from ubic in db.Ubicaciones_Inhibiciones
                         join ubi in db.Ubicaciones on ubic.id_ubicacion equals ubi.id_ubicacion
                         select new clsItemGrillaUbicacionesInhibidas()
                         {
                             Tipo = (ubi.NroPartidaMatriz == null && ubi.Seccion == null && ubi.Manzana == "" && ubi.Parcela == "") ? "UE" : "PM",
                             id_ubicacion = ubi.id_ubicacion,
                             id_ubicinhibida = ubic.id_ubicinhibi,
                             NroPartidaMatriz = (int)ubi.NroPartidaMatriz,
                             Seccion = (int)ubi.Seccion,
                             Manzana = ubi.Manzana,
                             Parcela = ubi.Parcela,
                             motivo = ubic.motivo,
                             fecha_inhibicion = ubic.fecha_inhibicion,
                             fecha_vencimiento = ubic.fecha_vencimiento,
                             domicilio = "",
                             id_dgubicacion = 0
                         }).Union

                 (from ubcla in db.Ubicaciones_PropiedadHorizontal_Inhibiciones
                  join prop in db.Ubicaciones_PropiedadHorizontal on ubcla.id_propiedadhorizontal equals prop.id_propiedadhorizontal
                  join ubi in db.Ubicaciones on prop.id_ubicacion equals ubi.id_ubicacion
                  select new clsItemGrillaUbicacionesInhibidas()
                  {
                      Tipo = "PH",
                      id_ubicacion = ubi.id_ubicacion,
                      id_ubicinhibida = ubcla.id_ubicphorinhibi,
                      NroPartidaMatriz = (int)prop.NroPartidaHorizontal,
                      Seccion = (int)ubi.Seccion,
                      Manzana = ubi.Manzana,
                      Parcela = ubi.Parcela,
                      motivo = ubcla.motivo,
                      fecha_inhibicion = ubcla.fecha_inhibicion,
                      fecha_vencimiento = ubcla.fecha_vencimiento,
                      domicilio = "",
                      id_dgubicacion = 0
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
                }

                //busqueda por Domicilio
                if (this.id_calle > 0)
                {
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
                    q = (from res in q
                         join ubic in db.Ubicaciones on res.id_ubicacion equals ubic.id_ubicacion
                         where ubic.Seccion == this.seccion &&
                             (ubic.Manzana.Contains(this.manzana.Trim()) || manzana.Trim().Length == 0) &&
                             (ubic.Parcela.Contains(this.parcela.Trim()) || parcela.Trim().Length == 0)
                         select res);
                }

                //busqueda por Ubicaciones Especiales
                if (this.id_sub_tipo_ubicacion > -1)
                {
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
                    q = (from res in q
                         join ubic in db.Ubicaciones on res.id_ubicacion equals ubic.id_ubicacion
                         join stipubic in db.SubTiposDeUbicacion on ubic.id_subtipoubicacion equals stipubic.id_subtipoubicacion
                         join tipubic in db.TiposDeUbicacion on stipubic.id_tipoubicacion equals tipubic.id_tipoubicacion
                         where tipubic.id_tipoubicacion == this.id_tipo_ubicacion
                         select res);
                }
                q = q.Distinct();
                int totalRowCount = q.Count();
                //q = q.Union(k);
                rq = q.ToList();
                if (rq.Count > 0)
                {

                    List<ItemDireccion> lstDirecciones = new List<ItemDireccion>();
                    string[] arrUbicaciones = rq.Select(x => x.id_ubicacion.ToString()).ToArray();

                    if (arrUbicaciones.Length > 0)
                        lstDirecciones = Shared.getDirecciones(arrUbicaciones);

                    //------------------------------------------------------------------------
                    //Rellena la clase a devolver con los datos que faltaban (Direccion)
                    //------------------------------------------------------------------------
                    foreach (var row in rq)
                    {
                        if (row.id_ubicacion != 0)
                        {
                            ItemDireccion itemDireccion = lstDirecciones.FirstOrDefault(x => x.id_ubicacion == row.id_ubicacion);
                            row.domicilio = itemDireccion.direccion;
                        }
                    }
                }

                grdUbicacionesInhibidas.DataSource = rq.ToList();
                grdUbicacionesInhibidas.DataBind();
                pnlResultadoBuscar.Visible = true;//agregados
                updPnlUbicacionesInhibidas.Update();


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
                //this.EjecutarScript(this, "inicializar_controles();");

                this.EjecutarScript(updPnlUbicacionesInhibidas, "showBusqueda();");
            }

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
            tipo_ubi.descripcion_tipoubicacion = "Todos";
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
            //Insert(hid_accion.Value, hid_ID_ubic.Value, hid_IDs_horizontal.Value);
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
                DateTime? FechaAlta = Convert.ToDateTime(txtFechaAlta.Text);
                FechaAlta = FechaAlta.Value.Add(TimeSpan.Parse(txtHoraAlta.Text));

                DateTime? FechaBaja = null;
                if (txtFechaBaja.Text != "")
                {
                    FechaBaja = Convert.ToDateTime(txtFechaBaja.Text);
                    FechaBaja = FechaBaja.Value.Add(TimeSpan.Parse(txtHoraBaja.Text));
                }

                int id_ubicinhibida = Convert.ToInt32(hid_ID_ubicinhibida.Value);
                int id_tipo = Convert.ToInt32(hid_ID_tipo.Value);
                if (id_tipo == 1)
                    db.Ubicaciones_PropiedadHorizontal_Inhibiciones_update(id_ubicinhibida, null, null, txtMotivo.Text, FechaAlta, FechaBaja, null, null, userid, txtMotivoLevantamiento.Text.Trim());
                else
                    db.Ubicaciones_Inhibiciones_update(id_ubicinhibida, null, txtMotivo.Text, FechaAlta, FechaBaja, null, null, userid, txtMotivoLevantamiento.Text.Trim());
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
                DateTime? FechaAlta = Convert.ToDateTime(txtFechaAlta.Text);
                FechaAlta = FechaAlta.Value.Add(TimeSpan.Parse(txtHoraAlta.Text));

                DateTime? FechaBaja = null;
                if (txtFechaBaja.Text != "")
                {
                    FechaBaja = Convert.ToDateTime(txtFechaBaja.Text);
                    FechaBaja = FechaBaja.Value.Add(TimeSpan.Parse(txtHoraBaja.Text));
                }

                List<int> numbers = new List<int>();
                if (!string.IsNullOrEmpty(hid_IDs_horizontal.Value) && hid_IDs_horizontal.Value.Length > 1)
                {
                    numbers = new List<int>(Array.ConvertAll(hid_IDs_horizontal.Value.Split(';'), Convert.ToInt32));
                }
                if (numbers.Count() == 0)
                {
                    int idubic = Convert.ToInt32(hid_ID_ubic.Value);
                    var q = (db.Ubicaciones_Inhibiciones.FirstOrDefault(x => x.id_ubicacion == idubic));
                    if (q != null)
                    {
                        throw new Exception("Ya esta inhibida la ubicación seleccionada");
                    }
                    db.Ubicaciones_Inhibiciones_insert(idubic, txtMotivo.Text.Trim(), FechaAlta, FechaBaja, null, null, userid, txtMotivoLevantamiento.Text.Trim());
                }
                else
                {
                    int idubic = Convert.ToInt32(hid_ID_ubic.Value);
                    foreach (int id_propiedad_horizontal in numbers)
                    {
                        var q = db.Ubicaciones_PropiedadHorizontal_Inhibiciones.FirstOrDefault(x => x.id_propiedadhorizontal == id_propiedad_horizontal);
                        if (q == null)
                        {
                            db.Ubicaciones_PropiedadHorizontal_Inhibiciones_insert(idubic, id_propiedad_horizontal, txtMotivo.Text, FechaAlta, FechaBaja, null, null, userid, txtMotivoLevantamiento.Text.Trim());
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
                    throw new Exception("La fecha de Alta no puede ser vacia");

                if (txtHoraAlta.Text == "")
                    throw new Exception("La hora de Alta no puede ser vacia");

                if (hid_accion.Value.Contains("insert"))
                    Guardar(userid, db);
                else if (hid_accion.Value.Contains("update"))
                    Modificar(userid, db);
                //LimpiarHidden();
                Limpiar();
                Buscador();
                this.EjecutarScript(updPnlDatosUbic, "hidefrmDatosUbicacion()");

            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updmpeInfo, "hidefrmAgregarUbicacion();hidefrmDatosUbicacion();showfrmError();inicializar_controles();");
            }
        }
        private void CargarCalles()
        {
            Functions.CargarAutocompleteCalles(AutocompleteCalles);
        }

        private void CargarCombo_subTipoUbicacion(int id_tipoubicacion)
        {
            DGHP_Entities db = new DGHP_Entities();
            List<SubTiposDeUbicacion> lista = db.SubTiposDeUbicacion.Where(x => x.id_tipoubicacion == id_tipoubicacion).ToList();

            SubTiposDeUbicacion sub_tipo_ubi = new SubTiposDeUbicacion();
            sub_tipo_ubi.id_subtipoubicacion = -1;
            sub_tipo_ubi.descripcion_subtipoubicacion = "Todos";
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
            hid_ID_ubicinhibida.Value = null;
            hid_IDs_horizontal.Value = null;
            updhidden.Update();
        }
        private void Limpiar()
        {
            txtMotivo.Text = "";
            txtMotivoLevantamiento.Text = "";
            txtFechaAlta.Text = "";
            txtFechaBaja.Text = "";
            txtHoraAlta.Text = "00:00";
            txtHoraBaja.Text = "00:00";

            LimpiarHidden();

          

            txtUbiNroPartida.Text = "";
            AutocompleteCalles.ClearSelection();
            Response.Cookies["AbmUbicacionesInhibidas_IdCalle"].Value = string.Empty;
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
            updPnlUbicacionesInhibidas.Update();
            EjecutarScript(updPnlUbicacionesInhibidas, "hideBusqueda();");
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
            if (!string.IsNullOrEmpty(txtUbiNroPuerta.Text) && ((String.IsNullOrEmpty(Request.Cookies["AbmUbicacionesInhibidas_IdCalle"].Value)) ? "" : Request.Cookies["AbmUbicacionesInhibidas_IdCalle"].Value) == "")
            {
                throw new Exception("Cuando especifica el número de puerta debe ingresar la calle.");
            }
            if ((Request.Cookies["AbmUbicacionesInhibidas_IdCalle"] == null))
            {
                this.id_calle = 0;
            }
            else
            {
                idAux = 0;
                if (Request.Cookies["AbmUbicacionesInhibidas_IdCalle"] != null)
                    int.TryParse(Request.Cookies["AbmUbicacionesInhibidas_IdCalle"].Value, out idAux);//ASOSA SYNCFUSION Hidden
                this.id_calle = idAux;
            }
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
            hid_ID_ubicinhibida.Value = valor.Substring(0, indx);
            hid_accion.Value = "update";

            int tipo = 0;
            int id_ubicinhibida = Convert.ToInt32(valor.Substring(0, indx));
            if (valor.Substring(indx + 1) == "PH")
            {
                tipo = 1;
                hid_ID_tipo.Value = "1";
            }

            updhidden.Update();

            //id_ubicinhibida = Convert.ToInt32(((LinkButton)sender).CommandArgument.ToString());
            //Session["Modificar"] = true;
            //Session["Tipo"] = tipo;
            //Session["id_ubicinhibida"] = id_ubicinhibida;
            if (tipo == 0)
            {
                var q = db.Ubicaciones_Inhibiciones.Where(x => x.id_ubicinhibi == id_ubicinhibida).FirstOrDefault();
                txtMotivo.Text = q.motivo;
                txtMotivoLevantamiento.Text = q.MotivoLevantamiento;
                txtFechaAlta.Text = q.fecha_inhibicion.ToShortDateString();
                txtHoraAlta.Text = q.fecha_inhibicion.ToString("HH:mm");
                txtFechaBaja.Text = (q.fecha_vencimiento == null) ? "" : q.fecha_vencimiento.Value.ToShortDateString();
                txtHoraBaja.Text = (q.fecha_vencimiento == null) ? "" : q.fecha_vencimiento.Value.ToString("HH:mm");
            }
            else
            {
                var q = db.Ubicaciones_PropiedadHorizontal_Inhibiciones.Where(x => x.id_ubicphorinhibi == id_ubicinhibida).FirstOrDefault();
                txtMotivo.Text = q.motivo;
                txtMotivoLevantamiento.Text = q.MotivoLevantamiento;
                txtFechaAlta.Text = q.fecha_inhibicion.ToShortDateString();
                txtHoraAlta.Text = q.fecha_inhibicion.ToString("HH:mm");
                txtFechaBaja.Text = (q.fecha_vencimiento == null) ? "" : q.fecha_vencimiento.Value.ToShortDateString();
                txtHoraBaja.Text = (q.fecha_vencimiento == null) ? "" : q.fecha_vencimiento.Value.ToString("HH:mm");
            }

            this.EjecutarScript(updPnlDatosUbic, "showfrmDatosUbicacion();inicializar_controles();");
            updPnlDatosUbic.Update();
        }


        protected void grdUbicacionesInhibidas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                LinkButton BotonEliminar = (LinkButton)e.Row.FindControl("lnkEliminar");

                HyperLink lnkPDF = (HyperLink)e.Row.FindControl("lnkPDF");

                clsItemGrillaUbicacionesInhibidas rowItem = (clsItemGrillaUbicacionesInhibidas)e.Row.DataItem;

                string b64 = Functions.ConvertToBase64String(string.Format("{0},{1}", rowItem.id_ubicinhibida, rowItem.Tipo));
                lnkPDF.NavigateUrl = string.Format("~/Reportes/ImprimirUbicacionesInhibidas.aspx?id={0}", b64);

                if (((clsItemGrillaUbicacionesInhibidas)e.Row.DataItem).fecha_vencimiento != null)
                    BotonEliminar.Visible = false;

                if (!Functions.ComprobarPermisosPagina("EDITAR_UBICACIONES_INHIBIDAS"))
                {
                    LinkButton BotonModificar = (LinkButton)e.Row.FindControl("lnkModificar");
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
            grdUbicacionesInhibidas.PageIndex = Convert.ToInt16(obj.Text) - 1;
            Buscador();
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdUbicacionesInhibidas.PageIndex = grdUbicacionesInhibidas.PageIndex + 1;
            Buscador();
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdUbicacionesInhibidas.PageIndex = grdUbicacionesInhibidas.PageIndex - 1;
            Buscador();
        }

        protected void grdResultados_DataBound(object sender, EventArgs e)
        {
            GridView grid = grdUbicacionesInhibidas;
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
            grdUbicacionesInhibidas.PageIndex = e.NewPageIndex;
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

        protected void lnkGuardarMotivoLevantamiento_Click(object sender, EventArgs e)
        {
            string id_Tipo = hid_ID_tipo.Value;

            int id_ubicinhibida = 0;
            int.TryParse(hid_ID_ubicinhibida.Value, out id_ubicinhibida);

            Eliminar(id_ubicinhibida, id_Tipo);
        }

        private void Eliminar(int id_ubicinhibida, string tipo)
        {
            Guid userid = SGI.Functions.GetUserId();
            DGHP_Entities db = new DGHP_Entities();

            hid_accion.Value = "update";

            updhidden.Update();

            if (tipo == "PH")
                db.Ubicaciones_PropiedadHorizontal_Inhibiciones_delete(id_ubicinhibida, null, userid, txtMotivoEliminarLevantamiento.Text);
            else
                db.Ubicaciones_Inhibiciones_delete(id_ubicinhibida, null, userid, txtMotivoEliminarLevantamiento.Text);

            Buscador();
            updPnlUbicacionesInhibidas.Update();
        }

        protected void lnkPDF_Click(object sender, EventArgs e)
        {
            string valor = ((LinkButton)sender).CommandArgument.ToString();
            int indx = valor.IndexOf(",");
            string Tipo;
            int id_ubicinhibida = Convert.ToInt32(valor.Substring(0, indx));

            if (valor.Substring(indx + 1) == "PH")
                Tipo = "PH";
            else
                Tipo = "PM";

            byte[] bytes = PDFInhibicion.GetUbicacionesInhibidas(Tipo, id_ubicinhibida);

            //Response.Clear();
            //MemoryStream ms = new MemoryStream(bytes);
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=labtest.pdf");
            //Response.Buffer = true;
            //ms.WriteTo(Response.OutputStream);
            //Response.End();



            Response.Clear();
            Response.Buffer = true;//false;
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + "asdasd.pdf");
            Response.AddHeader("Content-Length", bytes.Length.ToString());
            Response.BinaryWrite(bytes);
            Response.Flush();

            //Response.Clear();
            //Response.ContentType = "application/pdf";
            //Response.AppendHeader("Content-Disposition", string.Format("inline;filename=UbicacionInhibida{0}.pdf", id_ubicinhibida));
            //Response.BufferOutput = true;

            //Response.AddHeader("Content-Length", bytes.Length.ToString());
            //Response.BinaryWrite(bytes);
            //Response.End();

            //Response.ContentType = "application/force-download";
            //Response.AppendHeader("Content-Disposition", string.Format("attachment;filename=UbicacionInhibida{0}.avi", id_ubicinhibida));
            //Response.BinaryWrite(bytes);
            //Response.End();
        }

        protected void AutocompleteCalles_ValueSelect(//ASOSA SYNCFUSION ValueSelect
            object sender,Syncfusion.JavaScript.Web.AutocompleteSelectEventArgs e) 
        {
            Response.Cookies["AbmUbicacionesInhibidas_IdCalle"].Value = e.Key;
            return;
        }
    }
}