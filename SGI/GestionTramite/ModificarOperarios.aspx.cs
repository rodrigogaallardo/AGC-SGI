using SGI.Model;
using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.GestionTramite.Controls.CPadron;

namespace SGI.GestionTramite
{
    public partial class ModificarTransferencia : BasePage
    {
        DGHP_Entities db = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);

            }
        }

        #region Entity
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
        #endregion

        private int _id_cpadron = 0;
        public int id_cpadron
        {
            get
            {
                if (_id_cpadron == 0)
                {
                    int.TryParse(hid_id_cpadron.Value, out _id_cpadron);
                }
                return _id_cpadron;
            }
            set
            {
                hid_id_cpadron.Value = value.ToString();
                _id_cpadron = value;
            }
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                IniciarEntity();

                if (txtSolicitud.Text.Length == 0 && txtCPadron.Text.Length == 0)
                    throw new Exception("Debe ingresar un dato.");

                int id_solicitud = 0;
                int.TryParse(txtSolicitud.Text.Trim(), out id_solicitud);
                int id_Scpadron = 0;
                int.TryParse(txtCPadron.Text.Trim(), out id_Scpadron);

                if (id_solicitud != 0)
                {
                    var solicitud = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    if (solicitud == null)
                        throw new Exception("No se puede encontrar la solicitud de transferencia.");

                    if (id_Scpadron != 0 && solicitud.id_cpadron != id_Scpadron)
                        throw new Exception("Los datos ingresados son incorrectos.");

                    id_Scpadron = solicitud.id_cpadron;
                    id_solicitud = solicitud.id_solicitud;
                }
                else
                {
                    id_solicitud = id_Scpadron;
                }

                

                var CP_Solicitud = db.CPadron_Solicitudes.FirstOrDefault(x => x.id_cpadron == id_Scpadron);
                if (CP_Solicitud == null)
                    throw new Exception("No se pueden encontrar los datos de la Consulta al Padron.");

                var datos = db.CPadron_DatosLocal.FirstOrDefault(x => x.id_cpadron == id_Scpadron);
                if (datos == null)
                    throw new Exception("No se pueden encontrar los datos.");

                bool IsEditable = (CP_Solicitud.id_estado == (int)Constants.CPadron_EstadoSolicitud.Aprobado);                    
                if(!IsEditable)
                    throw new Exception("La solicitud ingresada no se encuentra Aprobada.");

                id_cpadron = datos.id_cpadron;

                Tabs_Tramite.editar = IsEditable;
                Tabs_Tramite.LoadData(id_cpadron,0, 0);
                string scripts = Tabs_Tramite.scriptCarga(IsEditable, false);

                EjecutarScript(UpdatePanel1, "showResultado();" + scripts);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "showfrmError();");
            }
            finally
            {
                 FinalizarEntity();
            }
        }


        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {

            txtCPadron.Text = "";
            txtSolicitud.Text = "";
            EjecutarScript(UpdatePanel1, "hideResultado();");
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode("Modificar Transferencias y Cpadrón");

           
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostratMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        private void LimpiarDatos()
        {
            txtCPadron.Text = "";
            txtSolicitud.Text = "";
        }

        /*private Transf_Solicitudes Buscar_solicitud(int id, Boolean tipo)
        {
            Transf_Solicitudes solicitud = new Transf_Solicitudes();
            if(tipo)
                solicitud = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id);
            else
                solicitud = db.Transf_Solicitudes.FirstOrDefault(x => x.id_cpadron == id);

            return solicitud;
        }*/

        public void LoadData(int id_cpadron)
        {
            this.id_cpadron = id_cpadron;

            DGHP_Entities db = new DGHP_Entities();

            string nro_expediente = db.CPadron_Solicitudes.Where(x => x.id_cpadron == this.id_cpadron).Select(x => x.nro_expediente_anterior).FirstOrDefault();
           
        }
        
        protected void lnkNroExpSave(object sender, Tabs_TramiteEventsArgs e)
        {
            //ucCabecera.LoadData((int)Constants.GruposDeTramite.CP, this.id_solicitud);
        }
    }
}