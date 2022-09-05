using SGI.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls.CPadron
{
    public partial class Tab_Planos : System.Web.UI.UserControl
    {
        public delegate void EventHandlerPlanosActualizado(object sender, EventArgs e);
        public event EventHandlerPlanosActualizado PlanosActualizado;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                //ScriptManager.RegisterStartupScript(updAgregarNormativa, updAgregarNormativa.GetType(), "init_JS_updAgregarNormativa", "init_JS_updAgregarNormativa();", true);
                //ScriptManager.RegisterStartupScript(updBuscarRubros, updBuscarRubros.GetType(), "init_JS_updBuscarRubros", "init_JS_updBuscarRubros();", true);
                //ScriptManager.RegisterStartupScript(updInformacionTramite, updInformacionTramite.GetType(), "init_JS_updInformacionTramite", "init_JS_updInformacionTramite();", true);

            }


            if (!IsPostBack)
            {
                hid_return_url.Value = Request.Url.AbsoluteUri;
                hid_DecimalSeparator.Value = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

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

        public void CargarDatos(int id_cpadron)
        {
            try
            {


                this.id_cpadron = id_cpadron;

                //CargarDatosTramite(id_cpadron);
                CargarTiposDePlanos();
                CargarPlanos(id_cpadron);

                //updInformacionTramite.Update();

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatos en Tab_Planos.aspx");
                //Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_Planos.aspx"));
                throw ex;
            }

        }

        private void CargarTiposDePlanos()
        {
            DGHP_Entities db = new DGHP_Entities();
            TipoDropDown.DataValueField = "id_tipo_plano";
            TipoDropDown.DataTextField = "nombre";
            TipoDropDown.DataSource = db.TiposDePlanos.ToList();
            TipoDropDown.DataBind();
            TipoDropDown.Focus();
            db.Dispose();
            cargarDatos(Convert.ToInt32(TipoDropDown.SelectedValue));
        }

        private void cargarDatos(int id_tipoDePlano)
        {
            DGHP_Entities db = new DGHP_Entities();
            var t = (from tipo in db.TiposDePlanos
                     where tipo.id_tipo_plano == id_tipoDePlano
                     select new
                     {
                         tipo.requiere_detalle,
                         tipo.extension,
                         tipo.tamanio_max_mb
                     }).FirstOrDefault();

            bool requiere_detalle = t.requiere_detalle.Value;
            if (requiere_detalle)
            {
                txtDetalle.Visible = true;
                lblDetalle.Visible = true;
            }
            else
            {
                lblDetalle.Visible = false;
                txtDetalle.Visible = false;
            }
            hid_requierre_detalle.Value = requiere_detalle.ToString();
            hid_extension.Value = t.extension;
            hid_tamanio.Value = Convert.ToString(Convert.ToInt32(t.tamanio_max_mb));
            hid_tamanio_max.Value = Convert.ToString(Convert.ToInt32(t.tamanio_max_mb) * 1024 * 1024);
            db.Dispose();
        }

        private void CargarPlanos(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            var list = (from cp in db.CPadron_Planos
                        join t in db.TiposDePlanos on cp.id_tipo_plano equals t.id_tipo_plano
                        where cp.id_cpadron == id_cpadron
                        select new
                        {
                            cp.id_cpadron_plano,
                            cp.id_cpadron,
                            cp.nombre_archivo,
                            cp.CreateDate,
                            cp.CreateUser,
                            detalle = t.requiere_detalle == true ? cp.detalle : t.nombre
                        }).ToList();

            grdPlanos.DataSource = list;
            grdPlanos.DataBind();
            db.Dispose();
        }

        protected void lnkEliminar_Command(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                LinkButton lnkEliminar = (LinkButton)sender;
                int id_cpadron_plano = Convert.ToInt32(lnkEliminar.CommandArgument);
                db.CPadron_EliminarPlanos(id_cpadron_plano);
                CargarPlanos(this.id_cpadron);

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                Functions.EjecutarScript(updPnlCargarPlano, "showfrmError_Planos();");
            }

        }

        public bool existePlano(int id_tipo_plano, string nombre, int id_cpadron)
        {
            DGHP_Entities db = new DGHP_Entities();
            var t = (from cp in db.CPadron_Planos
                     where cp.id_tipo_plano == id_tipo_plano &&
                         cp.nombre_archivo == nombre &&
                         cp.id_cpadron == id_cpadron
                     select new
                     {
                         cp.id_cpadron
                     }).FirstOrDefault();

            return t != null;
        }

        protected void btnCargarPlano_Click(object sender, EventArgs e)
        {
            try
            {
                string random = hid_filename_plano_random.Value;
                int idTipoDePlano = Convert.ToInt32(TipoDropDown.SelectedValue);
                string nombre = hid_filename_plano.Value;
                string savedFileName = Constants.PathTemporal + random + hid_filename_plano.Value;
                if (existePlano(idTipoDePlano, nombre, this.id_cpadron))
                    throw new Exception("El plano que está queriendo ingresar ya se encuentra en la lista.");
                if (!File.Exists(savedFileName))
                    throw new Exception("El archivo no fue transferido al servidor");

                byte[] documento = File.ReadAllBytes(savedFileName);

                File.Delete(savedFileName);

                //Elimina los planos con mas de 2 días para mantener el directorio limpio.
                string[] lstArchs = Directory.GetFiles(Constants.PathTemporal);
                foreach (string arch in lstArchs)
                {
                    DateTime fechaCreacion = File.GetCreationTime(arch);
                    if (fechaCreacion < DateTime.Now.AddDays(-2))
                        File.Delete(arch);
                }


                Guid userid = Functions.GetUserId();
                DGHP_Entities db = new DGHP_Entities();
                try
                {
                    db.CPadron_ActualizarPlanos(0, id_cpadron, idTipoDePlano, txtDetalle.Text.Trim(), documento.ToArray(), nombre, userid.ToString());
                }
                catch (Exception ex)
                {
                    db.Dispose();
                    throw ex;
                }

                CargarPlanos(id_cpadron);
                txtDetalle.Text = "";

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                Functions.EjecutarScript(updPnlCargarPlano, "showfrmError_Planos();");
            }
        }

        protected void TipoDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarDatos(Convert.ToInt32(TipoDropDown.SelectedValue));
        }
    }
}