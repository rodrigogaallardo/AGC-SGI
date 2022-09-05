using SGI.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucDatosLocal : System.Web.UI.UserControl
    {
        private int id_solicitud
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_id_solicitud.Value, out ret);
                return ret;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
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

            HabilitarEdicion(false);
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

        public void CargarDatos(int id_solicitud)
        {

            try
            {
                this.id_solicitud = id_solicitud;
                DGHP_Entities db = new DGHP_Entities();

                int id_encomienda = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                                                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).
                                                    OrderByDescending(y => y.FechaEncomienda).Select(w => w.id_encomienda).FirstOrDefault();

                Encomienda_DatosLocal dl = db.Encomienda_DatosLocal.FirstOrDefault(x => x.id_encomienda == id_encomienda);

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
                CargarMapas(id_encomienda);
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatos en Tab_DatosLocal.aspx");
                Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_DatosLocal.aspx"));
                throw ex;
            }

        }

        public void CargarMapas(int id_encomienda)
        {
            DGHP_Entities db = new DGHP_Entities();

            var q = (from encubic in db.Encomienda_Ubicaciones
                     join ubic in db.Ubicaciones on encubic.id_ubicacion equals ubic.id_ubicacion
                     join encpuer in db.Encomienda_Ubicaciones_Puertas on encubic.id_encomiendaubicacion equals encpuer.id_encomiendaubicacion
                     where encubic.id_encomienda == id_encomienda
                     select new
                     {
                         ubic.Seccion,
                         ubic.Manzana,
                         ubic.Parcela,
                         Direccion = encpuer.nombre_calle + " " + encpuer.NroPuerta.ToString()
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

    }
}