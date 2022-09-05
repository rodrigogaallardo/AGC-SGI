using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public partial class Configurar_mail : BasePage
    {
        DGHP_Entities db = null;
        string v_email_address;
        string v_display_name;
        string v_mailserver_name;
        int v_port;
        string v_username;
        string v_password;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosConfigMail();
                CargarGridPrioridades();
                CargarGridEstadoJob();
            }
        }

        protected void CargarGridEstadoJob()
        {
            db = new DGHP_Entities();
            var q = (
                        from dStado in db.sysjobs_vista
                        where dStado.name == "j_Proceso_Envio_mail"
                        select dStado.Estado).ToList();
            txtEstadoJob.Text = Convert.ToString(q.FirstOrDefault());
            btnGuardarJob.Text = (txtEstadoJob.Text == "Activo") ? "Inactivar" : "Activar";
            txtEstadoJob.ForeColor = (Convert.ToString(q.FirstOrDefault()) == "Activo") ? System.Drawing.Color.Blue : System.Drawing.Color.Red;

        }
        protected void CargarGridPrioridades()
        {
            db = new DGHP_Entities();
            var q = (
                        from dmail in db.Envio_Mail_Prioridades
                        select new clsItemGrillaMailPrioridades()
                        {
                            Prior_ID = dmail.ID_Prioridad,
                            Prior_Desde = dmail.Hora_Desde,
                            Prior_Hasta = dmail.Hora_Hasta,
                            Prior_Reenvio = dmail.Tiempo_Reenvio,
                            Prior_Observacion = dmail.Obervacion
                        }
                    ).ToList();
            gvPrioridades.DataSource = q;
            gvPrioridades.DataBind();
            updPrior.Update();

        }

        protected void LimpiarEdit()
        {
            txtDesde.Text = "";
            txtHasta.Text = "";
            txtReenvio.Text = "";
            txtObservacion.Text = string.Empty;
        }
        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            LimpiarEdit();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("grdEditActual_Prioridad");
            dt.Columns.Add("grdEditActual_Desde");
            dt.Columns.Add("grdEditActual_Hasta");
            dt.Columns.Add("grdEditActual_Reenvio");
            dt.Columns.Add("grdEditActual_Observacion");
            DataRow _ravi = dt.NewRow();

            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            int index = Convert.ToInt32(gvr.RowIndex);
            GridViewRow gvrow = gvPrioridades.Rows[index];
            _ravi["grdEditActual_Prioridad"] = HttpUtility.HtmlDecode(gvrow.Cells[1].Text).ToString();
            _ravi["grdEditActual_Desde"] = HttpUtility.HtmlDecode(gvrow.Cells[2].Text);
            _ravi["grdEditActual_Hasta"] = HttpUtility.HtmlDecode(gvrow.Cells[3].Text);
            _ravi["grdEditActual_Reenvio"] = HttpUtility.HtmlDecode(gvrow.Cells[4].Text);
            _ravi["grdEditActual_Observacion"] = HttpUtility.HtmlDecode(gvrow.Cells[5].Text);

            dt.Rows.Add(_ravi);
            grdEditActual.DataSource = dt;
            grdEditActual.DataBind();
            upEdit.Update();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#editModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScript", sb.ToString(), false);

        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            LimpiarEdit();
        }
        protected void btnGuardarPrior_Click(object sender, EventArgs e)
        {

            int V_Prioridad;
            TimeSpan V_Desde;
            TimeSpan V_Hasta;
            int V_Reenvio;
            string V_Observacion;


            if (string.IsNullOrEmpty(txtDesde.Text) || string.IsNullOrEmpty(txtHasta.Text) || string.IsNullOrEmpty(txtReenvio.Text))
            {
                lblError.Text = "Debe llenar todos los campos.";
                this.EjecutarScript(updmpeInfo, "showfrmError();");
                return;
            };

            V_Prioridad = Convert.ToInt32(grdEditActual.Rows[0].Cells[0].Text);
            V_Desde = TimeSpan.Parse(txtDesde.Text);
            V_Hasta = TimeSpan.Parse(txtHasta.Text);
            V_Reenvio = Convert.ToInt32(txtReenvio.Text);
            V_Observacion = txtObservacion.Text;

            db = new DGHP_Entities();
            db.mail_cfg_Actualizar_Prioridad(V_Prioridad, V_Desde, V_Hasta, V_Reenvio, V_Observacion);
            LimpiarEdit();
            CargarGridPrioridades();
        }

        protected void btnGuardarJob_OnClick(object sender, EventArgs e)
        {

            db = new DGHP_Entities();
            bool EstadoJob;
            EstadoJob = (txtEstadoJob.Text == "Activo") ? true : false;
            db.mail_cfg_Actualizar_Job(EstadoJob);

            if (EstadoJob)
            {
                btnGuardarJob.Text = "Inactivar";
                txtEstadoJob.Text = "Activo";
                txtEstadoJob.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                btnGuardarJob.Text = "Activar";
                txtEstadoJob.Text = "Inactivo";
                txtEstadoJob.ForeColor = System.Drawing.Color.Red;
            }

            updEstadoBtnGuardar.Update();
            updEstadoJob.Update();

        }


        public void CargarDatosConfigMail()
        {
            db = new DGHP_Entities();
            var cfgMail = db.Emails_Perfil_Config.ToList();

            grdDatosServer.DataSource = cfgMail;
            grdDatosServer.DataBind();
            updPnlServer.Update();
        }


        protected void gvPrioridades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        private void CargarDatosEditarConfig(int id_profile)
        {
            db = new DGHP_Entities();
            var cfgMail = db.Emails_Perfil_Config.Where(x => x.id_profile == id_profile).FirstOrDefault();
            limpiarDatos();
            if (cfgMail != null)
            {
                txtProfileName.Enabled = false;
                txtProfileName.Text = cfgMail.profile_name;
                txtDisplayName.Text = cfgMail.display_name;
                txtEmail.Text = cfgMail.email_address;
                txtSMTP.Text = cfgMail.smtp;
                txtPuerto.Text = Convert.ToString(cfgMail.port);
                txtUsuario.Text = cfgMail.username;
                txtContrasena.Text = cfgMail.password;

                if (cfgMail.is_default)
                {
                    rbEsDefault.Items[0].Selected = true;
                    rbEsDefault.Items[1].Selected = false;
                }
                else
                {
                    rbEsDefault.Items[1].Selected = true;
                    rbEsDefault.Items[0].Selected = false;
                }

                updConfigurarMail.Update();
            }
        }

        private void limpiarDatos()
        {
            txtProfileName.Text = "";
            txtDisplayName.Text = "";
            txtEmail.Text = "";
            txtSMTP.Text = "";
            txtPuerto.Text = "";
            txtUsuario.Text = "";
            txtContrasena.Text = "";
            txtUsuarioNuevo.Text = "";
        }

        protected void btnAceptarConfigurarMail_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            var cfgMail = db.Emails_Perfil_Config.Where(x => x.profile_name == txtProfileName.Text).FirstOrDefault();

            bool esDefault = rbEsDefault.Items[0].Selected;
            int puerto;
            int.TryParse(txtPuerto.Text, out puerto);
            if (string.IsNullOrEmpty(hid_modificando.Value))
            {
                if (cfgMail != null)
                {
                    lblError.Text = "Nombre de perfil ya registrado.";
                    this.EjecutarScript(updPnlServer, "hidefrmConfigurarMail();showfrmError();");
                }
                else
                    db.Insert_Emails_Perfil_Config(txtProfileName.Text, esDefault, txtDisplayName.Text, txtEmail.Text, txtSMTP.Text, puerto, txtUsuario.Text, txtContrasena.Text);
            }
            else
            {
                db.Update_Emails_Perfil_Config(txtProfileName.Text, esDefault, txtDisplayName.Text, txtEmail.Text, txtSMTP.Text, puerto, txtUsuario.Text, txtContrasena.Text);
                lblMsj.Text = "Datos del servidor Actualizados.";
                this.EjecutarScript(updMsj, "hidefrmConfigurarMail();showfrmMsj();");
            }
            CargarDatosConfigMail();
            this.EjecutarScript(updPnlServer, "hidefrmConfigurarMail();");
        }

        protected void btnAgregarCfg_Click(object sender, EventArgs e)
        {
            limpiarDatos();
            txtProfileName.Enabled = true;
            this.EjecutarScript(updPnlServer, "showfrmConfigurarMail();");
        }

        protected void btnEditarConfig_Click(object sender, EventArgs e)
        {
            LinkButton btnEditarConfig = (LinkButton)sender;
            int id_profile = int.Parse(btnEditarConfig.CommandArgument);
            hid_modificando.Value = Convert.ToString(id_profile);
            limpiarDatos();
            CargarDatosEditarConfig(id_profile);
            updPnlServer.Update();
            this.EjecutarScript(updPnlServer, "showfrmConfigurarMail();");

        }

        protected void btnEliminarConfig_Click(object sender, EventArgs e)
        {
            LinkButton btnEliminarConfig = (LinkButton)sender;
            int id_profile = int.Parse(btnEliminarConfig.CommandArgument);
            db = new DGHP_Entities();
            db.Delete_Emails_Perfil_Config(id_profile);
            CargarDatosConfigMail();
        }

        private void CargarDatosRelUsuario(int id_profile)
        {
            db = new DGHP_Entities();
            var rel_perfiles = db.Emails_Perfil_Relacion_Config.Where(x => x.id_profile == id_profile).ToList();
            hid_id_perfil.Value = Convert.ToString(id_profile);
            grdVerUsuario.DataSource = rel_perfiles;
            grdVerUsuario.DataBind();
        }

        protected void btnVerUsuarios_Click(object sender, EventArgs e)
        {
            LinkButton btnVerUsuarios = (LinkButton)sender;
            int id_profile = int.Parse(btnVerUsuarios.CommandArgument);
            limpiarDatos();
            CargarDatosRelUsuario(id_profile);
            updPnlServer.Update();
            this.EjecutarScript(updPnlServer, "showfrmVerUsuario();");

        }

        protected void btnEliminarRelUsuario_Click(object sender, EventArgs e)
        {
            LinkButton btnEliminarRelUsuario = (LinkButton)sender;
            int id_rel = int.Parse(btnEliminarRelUsuario.CommandArgument);
            db = new DGHP_Entities();
            int id_profile;
            int.TryParse(hid_id_perfil.Value, out id_profile);
            db.Delete_Emails_Perfil_Relacion_Config(id_rel);
            CargarDatosRelUsuario(id_profile);
        }

        protected void lnkAgregarUsuarioNuevo_Click(object sender, EventArgs e)
        {
            int id_profile = 0;
            int.TryParse(hid_id_perfil.Value, out id_profile);
            db = new DGHP_Entities();
            string usuario = txtUsuarioNuevo.Text.Trim();
            if (db.Emails_Perfil_Relacion_Config.Where(x => x.ws_username == usuario).Any())
            {
                lblError.Text = "Usuario ya asignado a otro perfil.";
                this.EjecutarScript(updAgregarUsuarioPopOver, "showfrmError();");
            }
            else
            {
                if (!db.aspnet_Users.Where(x => x.UserName == usuario && x.ApplicationId == Constants.WebServices.ApplicationId).Any())
                {
                    lblError.Text = "Usuario no registrado como usuario de servicio.";
                    this.EjecutarScript(updAgregarUsuarioPopOver, "showfrmError();");
                }
                else
                {
                    db.Insert_Emails_Perfil_Relacion_Config(id_profile, usuario);
                }
            }
            limpiarDatos();
            CargarDatosRelUsuario(id_profile);
        }

    }
}