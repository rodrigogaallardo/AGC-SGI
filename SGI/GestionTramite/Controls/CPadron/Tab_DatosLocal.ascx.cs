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
    public partial class Tab_DatosLocal : System.Web.UI.UserControl
    {
        public delegate void EventHandlerDatosLocalActualizada(object sender, EventArgs e);
        public event EventHandlerDatosLocalActualizada DatosLocalActualizada;

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
        private bool editar
        {
            get
            {
                bool ret = false;
                ret = hid_editar.Value.Equals("True") ? true : false;
                return ret;
            }
            set
            {
                hid_editar.Value = value.ToString();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updDatosLocal, updDatosLocal.GetType(), "init_JS_updDatosLocal", "init_JS_updDatosLocal();", true);
            }


            if (!IsPostBack)
            {
                hid_DecimalSeparator.Value = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                hid_return_url.Value = Request.Url.AbsoluteUri;
            }

            HabilitarEdicion(this.editar);            
        }

        public void HabilitarEdicion(bool valor) {
            txtCantidadArtefactosSanitarios.Enabled = valor;
            txtCantOperarios.Enabled = valor;
            txtDimensionFrente.Enabled = valor;
            txtDistanciaSanitarios_dl.Enabled = valor;
            txtFondo.Enabled = valor;
            txtFrente.Enabled = valor;
            txtLatDer.Enabled = valor;
            txtLatIzq.Enabled = valor;
            txtParedes.Enabled = valor;
            txtPisos.Enabled = valor;
            txtRevestimientos.Enabled = valor;
            txtSuperficieCubierta.Enabled = valor;
            txtSuperficieDescubierta.Enabled = valor;
            txtSuperficieSanitarios.Enabled = valor;
            txtSuperficieTotal.Enabled = valor;
            txtTechos.Enabled = valor;
            opt1_no.Enabled = valor;
            opt1_si.Enabled = valor;
            opt2_no.Enabled = valor;
            opt2_si.Enabled = valor;
            opt3_no.Enabled = valor;
            opt3_si.Enabled = valor;
            opt4_no.Enabled = valor;
            opt4_si.Enabled = valor;
            opt5_dentro.Enabled = valor;
            opt5_fuera.Enabled = valor;
        }

        public void CargarDatos(int id_solicitud, int validar_estado, bool Editar)
        {

            try
            {
                this.id_cpadron = id_solicitud;
                this.validar_estado = validar_estado;
                this.editar = Editar;
                DGHP_Entities db = new DGHP_Entities();

                CPadron_DatosLocal dl = db.CPadron_DatosLocal.FirstOrDefault(x => x.id_cpadron == id_solicitud);

                decimal SuperficieCubierta = 0;
                decimal SuperficieDescubierta = 0;
                decimal DimensionFrente = 0;
                bool LugarCargaDescarga = false;
                bool Estacionamiento = false;
                bool RedTransitoPesado = false;
                bool SobreAvenida = false;
                string Pisos = "";
                string Paredes = "";
                string Techos = "";
                string Revestimientos = "";
                int SanitariosUbicacion = 0;
                decimal SanitariosDistancia = 0;
                string CroquisUbicacion = "";
                int CantidadSanitarios = 0;
                decimal SuperficieSanitarios = 0;
                decimal Frente = 0;
                decimal Fondo = 0;
                decimal LateralDerecho = 0;
                decimal LateralIzquierdo = 0;
                int CantidadOperarios = 0;

                if (dl != null)
                {
                    SuperficieCubierta = dl.superficie_cubierta_dl.Value;
                    SuperficieDescubierta = dl.superficie_descubierta_dl.Value;
                    DimensionFrente = dl.dimesion_frente_dl.Value;
                    LugarCargaDescarga = dl.lugar_carga_descarga_dl;
                    Estacionamiento = dl.estacionamiento_dl;
                    RedTransitoPesado = dl.red_transito_pesado_dl;
                    SobreAvenida = dl.sobre_avenida_dl;

                    Pisos = dl.materiales_pisos_dl;
                    Paredes = dl.materiales_paredes_dl;
                    Techos = dl.materiales_techos_dl;
                    Revestimientos = dl.materiales_revestimientos_dl;

                    SanitariosUbicacion = dl.sanitarios_ubicacion_dl;

                    if (SanitariosUbicacion == 2)   //Fuera del local
                        SanitariosDistancia = dl.sanitarios_distancia_dl.Value;

                    CroquisUbicacion = dl.croquis_ubicacion_dl;
                    CantidadSanitarios = dl.cantidad_sanitarios_dl.Value;
                    SuperficieSanitarios = dl.superficie_sanitarios_dl.Value;

                    Frente = dl.frente_dl.Value;
                    Fondo = dl.fondo_dl.Value;
                    LateralIzquierdo = dl.lateral_izquierdo_dl.Value;
                    LateralDerecho = dl.lateral_derecho_dl.Value;
                    CantidadOperarios = dl.cantidad_operarios_dl.Value;
                }


                txtSuperficieCubierta.Text = SuperficieCubierta.ToString();
                txtSuperficieDescubierta.Text = SuperficieDescubierta.ToString();
                txtSuperficieTotal.Text = Convert.ToString(SuperficieCubierta + SuperficieDescubierta);
                txtDimensionFrente.Text = DimensionFrente.ToString();
                txtFrente.Text = Frente.ToString();
                txtFondo.Text = Fondo.ToString();
                txtLatIzq.Text = LateralIzquierdo.ToString();
                txtLatDer.Text = LateralDerecho.ToString();

                opt1_si.Checked = LugarCargaDescarga;
                opt1_no.Checked = !LugarCargaDescarga;
                opt2_si.Checked = Estacionamiento;
                opt2_no.Checked = !Estacionamiento;
                opt3_si.Checked = RedTransitoPesado;
                opt3_no.Checked = !RedTransitoPesado;
                opt4_si.Checked = SobreAvenida;
                opt4_no.Checked = !SobreAvenida;

                txtPisos.Text = Pisos;
                txtParedes.Text = Paredes;
                txtTechos.Text = Techos;
                txtRevestimientos.Text = Revestimientos;


                switch (SanitariosUbicacion)
                {
                    case 1:
                        opt5_dentro.Checked = true;
                        break;
                    case 2:
                        opt5_fuera.Checked = true;
                        txtDistanciaSanitarios_dl.Text = SanitariosDistancia.ToString();
                        break;
                }


                //txtElementosSeleccionados.Text = CroquisUbicacion;
                txtCantidadArtefactosSanitarios.Text = CantidadSanitarios.ToString();
                txtSuperficieSanitarios.Text = SuperficieSanitarios.ToString();
                txtCantOperarios.Text = CantidadOperarios.ToString();
                db.Dispose();
                CargarMapas(id_solicitud);
                if (!this.editar)
                {
                    updBotonesGuardar.Visible = false;
                    updBotonesGuardar.Update();
                    titulo.Visible = false;
                    HabilitarEdicion(false);
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatos en Tab_DatosLocal.aspx");
                Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_DatosLocal.aspx"));
                throw ex;
            }

        }

        public void CargarMapas(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();

            var q = (from cpubic in db.CPadron_Ubicaciones
                     join ubic in db.Ubicaciones on cpubic.id_ubicacion equals ubic.id_ubicacion
                     join cppuer in db.CPadron_Ubicaciones_Puertas on cpubic.id_cpadronubicacion equals cppuer.id_cpadronubicacion
                     where cpubic.id_cpadron == id_solicitud
                     select new
                     {
                         ubic.Seccion,
                         ubic.Manzana,
                         ubic.Parcela,
                         Direccion = cppuer.nombre_calle + " " + cppuer.NroPuerta.ToString()
                     });

            var lstResult = q.ToList();
            imgMapa1.ImageUrl = "~/Content/img/app/ImageNotFound.png";
            imgMapa2.ImageUrl = "~/Content/img/app/ImageNotFound.png";
            foreach (var item in lstResult)
            {
                if (item.Seccion != null)
                {
                    imgMapa1.ImageUrl = Functions.GetUrlMapa(item.Seccion.Value.ToString(), item.Manzana, item.Parcela, item.Direccion);
                    imgMapa2.ImageUrl = Functions.GetUrlCroquis(item.Seccion.Value.ToString(), item.Manzana, item.Parcela, item.Direccion);
                }

            }

            updDatosLocal.Update();

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            Guid userid = Functions.GetUserId();

            try
            {
                decimal SuperficieCubierta;
                decimal SuperficieDescubierta;
                decimal DimensionFrente;
                bool LugarCargaDescarga = false;
                bool Estacionamiento = false;
                bool RedTransitoPesado = false;
                bool SobreAvenida = false;
                string Pisos;
                string Paredes;
                string Techos;
                string Revestimientos;
                int SanitariosUbicacion = 0;
                decimal SanitariosDistancia = 0;
                int CantidadOperarios = 0;

                decimal.TryParse(txtSuperficieCubierta.Text, out SuperficieCubierta);
                decimal.TryParse(txtSuperficieDescubierta.Text, out SuperficieDescubierta);
                decimal.TryParse(txtDimensionFrente.Text, out DimensionFrente);

                LugarCargaDescarga = opt1_si.Checked;
                Estacionamiento = opt2_si.Checked;
                RedTransitoPesado = opt3_si.Checked;
                SobreAvenida = opt4_si.Checked;
                Pisos = txtPisos.Text.Trim();
                Paredes = txtParedes.Text.Trim();
                Techos = txtTechos.Text.Trim();
                Revestimientos = txtRevestimientos.Text.Trim();

                if (opt5_dentro.Checked)
                    SanitariosUbicacion = 1;

                if (opt5_fuera.Checked)
                {
                    SanitariosUbicacion = 2;
                    decimal.TryParse(txtDistanciaSanitarios_dl.Text, out SanitariosDistancia);
                }

                int CantidadSanitarios = 0;
                decimal SuperficieSanitarios = 0;
                decimal frente = 0;
                decimal fondo = 0;
                decimal LateralIzquierdo = 0;
                decimal LateralDerecho = 0;

                int.TryParse(txtCantidadArtefactosSanitarios.Text, out CantidadSanitarios);
                decimal.TryParse(txtSuperficieSanitarios.Text, out SuperficieSanitarios);

                decimal.TryParse(txtFrente.Text, out frente);
                decimal.TryParse(txtFondo.Text, out fondo);
                decimal.TryParse(txtLatIzq.Text, out LateralIzquierdo);
                decimal.TryParse(txtLatDer.Text, out LateralDerecho);

                int.TryParse(txtCantOperarios.Text, out CantidadOperarios);

                db.CPadron_ActualizarDatosLocal(this.id_cpadron, SuperficieCubierta, SuperficieDescubierta,
                                                DimensionFrente, LugarCargaDescarga, Estacionamiento, RedTransitoPesado,
                                                SobreAvenida, Pisos, Paredes, Techos, Revestimientos, SanitariosUbicacion,
                                                SanitariosDistancia, null, CantidadSanitarios, SuperficieSanitarios,
                                                frente, fondo, LateralIzquierdo, LateralDerecho, CantidadOperarios, userid, validar_estado);

                // Hace Fire al eneto de Actualización si el mismo está definido.
                if (DatosLocalActualizada != null)
                    DatosLocalActualizada(sender, e);

            }
            catch (Exception ex)
            {
                //LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                Functions.EjecutarScript(updBotonesGuardar, "frmError_DatosLocal();");
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}