using SGI.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls.CPadron
{
    public partial class Tab_ConformacionesLocal : System.Web.UI.UserControl
    {
        public delegate void EventHandlerConformacionesLocalActualizado(object sender, EventArgs e);
        public event EventHandlerConformacionesLocalActualizado ConformacionesLocalActualizado;

        
        DGHP_Entities db;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updConformacionIngresada, updConformacionIngresada.GetType(), "camposAutonumericos", "camposAutonumericos();", true);

            }


            if (!IsPostBack)
            {
                hid_return_url.Value = Request.Url.AbsoluteUri;
                hid_DecimalSeparator1.Value = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            }
            HabilitarEdicion(true);
        }

        public void HabilitarEdicion(bool valor) {
            txtSupTotal.Enabled = valor;
        }

        private int validar_estado
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_validar_estado.Value, out ret);
                return ret;
            }
            set
            {
                hid_validar_estado.Value = value.ToString();
            }

        }
        private int id_cpadron
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_id_cpadron.Value, out ret);
                return ret;
            }
            set
            {
                hid_id_cpadron.Value = value.ToString();
            }

        }

        public void CargarDatos(int id_cpadron, int validar_estado)
        {
            try
            {
                this.id_cpadron = id_cpadron;
                this.validar_estado = validar_estado;

                CargarCombos(id_cpadron);
                CargarConformacionLocal(id_cpadron);
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatos en Tab_Conformacion_local.aspx");
                //Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_Rubros.aspx"));
                throw ex;
            }

        }

        private void CargarCombos(int id_cpadron)
        {
            db = new DGHP_Entities();
            List<TipoDestino> lista = db.TipoDestino.ToList();

            ddlTipoDestino.DataSource = lista;
            ddlTipoDestino.DataTextField = "nombre";
            ddlTipoDestino.DataValueField = "id";
            ddlTipoDestino.DataBind();
            ddlTipoDestino.SelectedIndex = 0;

            //cargar tabla con flag que indica si es o no obligatorio cargar el detalle
            //el textbox detalle se poni visible o no desde js
            foreach (TipoDestino row in lista)
            {
                TableRow tblRow = new TableRow();

                TableCell celdaValue = new TableCell();
                celdaValue.Text = row.Id.ToString();
                tblRow.Cells.Add(celdaValue);

                TableCell celdaText = new TableCell();
                celdaText.Text = row.RequiereDetalle.ToString().ToLower();
                tblRow.Cells.Add(celdaText);
                tblTipoDestino.Rows.Add(tblRow);
            }

            tblTipoDestino.DataBind();
            updTblTipoDestino.Update();
            cargarPlantas(id_cpadron);
            cargarVentilacion();
            cargarIluminacion();
            cargarTipoSuperficie();
            db.Dispose();

        }

        private void cargarPlantas(int id_cpadron)
        {
            var l = (from cpPlantas in db.CPadron_Plantas
                     join tipo in db.TipoSector on cpPlantas.id_tiposector equals tipo.Id
                     where cpPlantas.id_cpadron == id_cpadron
                     select new
                     {
                         cpPlantas.id_cpadron,
                         cpPlantas.id_cpadrontiposector,
                         cpPlantas.id_tiposector,
                         cpPlantas.TipoSector,
                         cpPlantas.detalle_cpadrontiposector,
                         detalle_planta = tipo.Id == 11 ? cpPlantas.detalle_cpadrontiposector :
                            tipo.Descripcion + " " + cpPlantas.detalle_cpadrontiposector
                     });
            ddlPlanta.DataTextField = "detalle_planta";
            ddlPlanta.DataValueField = "id_cpadrontiposector";

            ddlPlanta.DataSource = l.ToList();
            ddlPlanta.DataBind();

            ddlPlanta.Items.Insert(0, new ListItem("", "0"));
        }

        private void cargarVentilacion()
        {
            ddlVentilacion.DataTextField = "nom_ventilacion";
            ddlVentilacion.DataValueField = "id_ventilacion";

            ddlVentilacion.DataSource = db.tipo_ventilacion.ToList();
            ddlVentilacion.DataBind();

            ddlVentilacion.Items.Insert(0, new ListItem("", "-1"));
            ddlVentilacion.SelectedIndex = 1;
        }

        private void cargarIluminacion()
        {
            ddlIluminacion.DataTextField = "nom_iluminacion";
            ddlIluminacion.DataValueField = "id_iluminacion";

            ddlIluminacion.DataSource = db.tipo_iluminacion.ToList();
            ddlIluminacion.DataBind();

            ddlIluminacion.Items.Insert(0, new ListItem("", "0"));
        }

        private void cargarTipoSuperficie()
        {
            ddlTipoSuperficie.DataTextField = "Nombre";
            ddlTipoSuperficie.DataValueField = "Id";

            ddlTipoSuperficie.DataSource = db.TipoSuperficie.ToList();
            ddlTipoSuperficie.DataBind();

            ddlTipoSuperficie.Items.Insert(0, new ListItem("", "0"));
        }

        private void CargarConformacionLocal(int id_cpadron)
        {
            db = new DGHP_Entities();
            var conformaciones = (from cl in db.CPadron_ConformacionLocal
                                  //join tcer in db.CAA_TiposDeCertificados on caa.id_tipocertificado equals tcer.id_tipocertificado
                                  join a in db.TipoDestino on cl.id_destino equals a.Id into b
                                  from td in b.DefaultIfEmpty()
                                  join d in db.tipo_iluminacion on cl.id_iluminacion equals d.id_iluminacion into c
                                  from ilu in c.DefaultIfEmpty()
                                  join f in db.tipo_ventilacion on cl.id_ventilacion equals f.id_ventilacion into e
                                  from vent in e.DefaultIfEmpty()
                                  join h in db.TipoSuperficie on cl.id_tiposuperficie equals h.Id into g
                                  from tipoSup in g.DefaultIfEmpty()
                                  join plan in db.CPadron_Plantas on cl.id_cpadrontiposector equals plan.id_cpadrontiposector into i
                                  from cpPlantas in i.DefaultIfEmpty()
                                  join l in db.TipoSector on cpPlantas.id_tiposector equals l.Id into k
                                  from tipoSec in k.DefaultIfEmpty()
                                  where cl.id_cpadron == id_cpadron
                                  select new
                                  {
                                      cl.alto_conflocal,
                                      cl.ancho_conflocal,
                                      cl.Detalle_conflocal,
                                      cl.Frisos_conflocal,
                                      cl.id_cpadron,
                                      cl.id_cpadronconflocal,
                                      cl.id_cpadrontiposector,
                                      cl.id_destino,
                                      cl.id_iluminacion,
                                      cl.id_tiposuperficie,
                                      cl.id_ventilacion,
                                      cl.largo_conflocal,
                                      cl.Observaciones_conflocal,
                                      cl.Paredes_conflocal,
                                      cl.Pisos_conflocal,
                                      cl.superficie_conflocal,
                                      cl.Techos_conflocal,
                                      cl.tipo_iluminacion,
                                      cl.tipo_ventilacion,
                                      desc_tipodestino = td.Nombre,
                                      desc_planta = tipoSec.Id == 11 ? cpPlantas.detalle_cpadrontiposector : tipoSec.Nombre + " " + cpPlantas.detalle_cpadrontiposector,
                                      desc_ventilacion = vent.nom_ventilacion,
                                      desc_iluminacion = ilu.nom_iluminacion,
                                      desc_tiposuperficie = tipoSup.Nombre
                                  });

            var lista = conformaciones.ToList();
            grdConformacionLocal.DataSource = lista;
            grdConformacionLocal.DataBind();
            decimal suma = 0;
            if (lista.Count > 0)
            {
                foreach (var r in lista)
                {
                    suma += Convert.ToDecimal(r.superficie_conflocal);
                }
            }
            txtSupTotal.Text = suma.ToString("N2");
            db.Dispose();
        }

        #region Alta, baja y modicacion de datos

        private void CargarDatosConformacion(int id_cpadronconflocal)
        {
            db = new DGHP_Entities();
            hid_conflocal.Value = id_cpadronconflocal.ToString();

            var conf = (from cl in db.CPadron_ConformacionLocal
                        join td in db.TipoDestino on cl.id_destino equals td.Id
                        join h in db.TipoSuperficie on cl.id_tiposuperficie equals h.Id into g
                        from tipoSup in g.DefaultIfEmpty()
                        where cl.id_cpadron == id_cpadron &&
                        cl.id_cpadronconflocal == id_cpadronconflocal
                        select new
                        {
                            cl.alto_conflocal,
                            cl.ancho_conflocal,
                            cl.Detalle_conflocal,
                            cl.Frisos_conflocal,
                            cl.id_cpadron,
                            cl.id_cpadronconflocal,
                            cl.id_cpadrontiposector,
                            cl.id_destino,
                            cl.id_iluminacion,
                            cl.id_tiposuperficie,
                            cl.id_ventilacion,
                            cl.largo_conflocal,
                            cl.Observaciones_conflocal,
                            cl.Paredes_conflocal,
                            cl.Pisos_conflocal,
                            cl.superficie_conflocal,
                            cl.Techos_conflocal,
                            cl.tipo_iluminacion,
                            cl.tipo_ventilacion,
                            desc_tipodestino = td.Nombre,
                            desc_tiposuperficie = tipoSup.Nombre
                        }).FirstOrDefault();

            ddlTipoDestino.SelectedValue = conf.id_destino.ToString();
            ddlPlanta.SelectedValue = (conf.id_cpadrontiposector == null) ? "0" : conf.id_cpadrontiposector.ToString();
            txtAlto.Text = conf.alto_conflocal.ToString();
            txtAncho.Text = conf.ancho_conflocal.ToString();
            txtLargo.Text = conf.largo_conflocal.ToString();
            txtParedes.Text = conf.Paredes_conflocal;
            txtPisos.Text = conf.Pisos_conflocal;
            txtTechos.Text = conf.Techos_conflocal;
            txtFrisos.Text = conf.Frisos_conflocal;
            txtObservaciones.Text = conf.Observaciones_conflocal;
            txtDetalle.Text = conf.Detalle_conflocal;
            txtSuperficie.Text = conf.superficie_conflocal.ToString();

            ddlVentilacion.SelectedValue = (conf.id_ventilacion == null) ? "-1" : conf.id_ventilacion.ToString();
            ddlIluminacion.SelectedValue = (conf.id_iluminacion == null) ? "0" : conf.id_iluminacion.ToString();
            ddlTipoSuperficie.SelectedValue = (conf.id_tiposuperficie == null) ? "-1" : conf.id_tiposuperficie.ToString();
            db.Dispose();
        }

        protected void grdConformacionLocal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int id_cpadronconflocal = int.Parse(e.CommandArgument.ToString());

                switch (e.CommandName)
                {
                    case "EditarDetalle":

                        CargarDatosConformacion(id_cpadronconflocal);
                        updDetalleConformacion.Update();
                        Functions.EjecutarScript(updConformacionIngresada, "cambioTipoSuperficie();showfrmAgregarDetalleConformacion();");
                        break;

                    case "EliminarDetalle":
                        db = new DGHP_Entities();
                        db.CPadron_EliminarConformacionLocal(id_cpadronconflocal, this.id_cpadron, validar_estado);
                        db.Dispose();
                        CargarConformacionLocal(this.id_cpadron);
                        if (ConformacionesLocalActualizado != null)
                            ConformacionesLocalActualizado(sender, e);
                        break;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                Functions.EjecutarScript(updConformacionIngresada, "showfrmError_DetalleConformacion();");
            }
        }

        protected void btnCerrarDetalleConformacion_Click(object sender, EventArgs e)
        {
            LimpiarCamposDetalleConformacion();
            ScriptManager.RegisterClientScriptBlock(updBotonesIngresarDetalleConformacion, updBotonesIngresarDetalleConformacion.GetType(), "ocultarpopupNormativa", "ocultarPopup('pnlAgregarDetalleConformacion');", true);
        }

        private void LimpiarCamposDetalleConformacion()
        {
            hid_conflocal.Value = "0";
            ddlTipoDestino.SelectedIndex = 0;
            ddlPlanta.SelectedIndex = 0;
            txtAlto.Text = "";
            txtAncho.Text = "";
            txtLargo.Text = "";
            txtSuperficie.Text = "";
            txtParedes.Text = "";
            txtPisos.Text = "";
            txtTechos.Text = "";
            txtFrisos.Text = "";
            txtObservaciones.Text = "";
            txtDetalle.Text = "";
            ddlVentilacion.SelectedIndex = 0;
            ddlIluminacion.SelectedIndex = 0;
            ddlTipoSuperficie.SelectedIndex = 0;
        }

        protected void lnkIngresarDetalleConformacion_Click(object sender, EventArgs e)
        {
            LimpiarCamposDetalleConformacion();
            updDetalleConformacion.Update();
            Functions.EjecutarScript(updIngresarDetalleConformacion, "showfrmAgregarDetalleConformacion();");
        }

        #endregion

        protected void btnAceptarConformacion_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            lblError.Text = "";
            int id_conflocal = 0;
            int.TryParse(hid_conflocal.Value, out id_conflocal);
            try
            {
                int id_destino = int.Parse(ddlTipoDestino.SelectedValue);
                decimal largo = 0;
                decimal ancho = 0;
                decimal alto = 0;
                decimal superficie = 0;
                decimal.TryParse(txtLargo.Text, out largo);
                decimal.TryParse(txtAncho.Text, out ancho);
                decimal.TryParse(txtAlto.Text, out alto);
                decimal.TryParse(txtSuperficie.Text, out superficie);
                string paredes = txtParedes.Text.Trim();
                string techos = txtTechos.Text.Trim();
                string pisos = txtPisos.Text.Trim();
                string frisos = txtFrisos.Text.Trim();
                string observaciones = txtObservaciones.Text.Trim();
                Guid userid = Functions.GetUserId();

                int id_cpadron_tipo_sector = Convert.ToInt32(ddlPlanta.SelectedValue);
                int id_ventilacion = Convert.ToInt32(ddlVentilacion.SelectedValue);
                int id_iluminacion = Convert.ToInt32(ddlIluminacion.SelectedValue);
                int id_tiposuperficie = Convert.ToInt32(ddlTipoSuperficie.SelectedValue);

                bool requiereDetalle = false;
                TipoDestino td = db.TipoDestino.FirstOrDefault(x => x.Id == id_destino);
                bool.TryParse(td.RequiereDetalle.ToString().ToLower(), out requiereDetalle);

                string detalle = (requiereDetalle) ? txtDetalle.Text : "";

                if (id_conflocal == 0)
                    db.CPadron_AgregarConformacionLocal(id_cpadron, id_destino, largo, ancho, alto, superficie,
                            paredes, techos, pisos, frisos, observaciones, userid, detalle,
                            id_cpadron_tipo_sector, id_ventilacion, id_iluminacion, id_tiposuperficie, validar_estado);
                else
                    db.CPadron_ActualizarConformacionLocal(id_conflocal, id_cpadron, id_destino, largo, ancho, alto, superficie,
                            paredes, techos, pisos, frisos, observaciones, userid, detalle,
                            id_cpadron_tipo_sector, id_ventilacion, id_iluminacion, id_tiposuperficie, validar_estado);

                LimpiarCamposDetalleConformacion();
                Functions.EjecutarScript(updBotonesIngresarDetalleConformacion, "hidefrmAgregarDetalleConformacion();");

                CargarConformacionLocal(id_cpadron);
                if (ConformacionesLocalActualizado != null)
                    ConformacionesLocalActualizado(sender, e);
            }
            catch (Exception ex)
            {
                db.Dispose();
                lblError.Text = ex.Message;
                Functions.EjecutarScript(updBotonesIngresarDetalleConformacion, "showfrmError_DetalleConformacion();");
            }

        }
    }
}