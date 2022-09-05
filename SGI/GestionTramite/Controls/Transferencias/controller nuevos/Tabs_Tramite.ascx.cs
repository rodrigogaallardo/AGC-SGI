using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using System.ComponentModel;

namespace SGI.GestionTramite.Controls.Transferencias
{
    public class Tabs_TramiteEventsArgs : EventArgs
    {
        public string nroexpediente { get; set; }
        public int IdSolicitud { get; set; }
    }
    public partial class Tabs_Tramite : System.Web.UI.UserControl
    {
        public int Validar_Estado;

        private int id_solicitud
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_transf_id.Value, out ret);
                return ret;
            }
            set
            {
                hid_transf_id.Value = value.ToString();
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LoadData(int id_solicitud, int Validar_Estado)
        {
            this.id_solicitud = id_solicitud;
            this.Validar_Estado = Validar_Estado;

            DGHP_Entities db = new DGHP_Entities();

            string nro_expediente = db.Transf_Solicitudes.Where(x => x.id_solicitud == this.id_solicitud).Select(x => x.NroExpedienteSade).FirstOrDefault();
            lblNumeroExpediente.Text = nro_expediente;
            txtNumeroExpediente.Text = nro_expediente;
            updDatoExpediente.Update();

            Tab_Ubicaciones.CargarDatos(this.id_solicitud, Validar_Estado);            
            Tab_Titulares.CargarDatos(this.id_solicitud, Validar_Estado, true);
            Tab_TitularesSol.CargarDatos(this.id_solicitud, true);
            updDatoTramiteTR.Update();
        }

        public string scriptCarga(bool habilitarControl, bool visibleConformacionLocal)
        {
            string script = "cargaTabs();" + (habilitarControl ? "habilitarControles(true);" : "habilitarControles(false);");
               // + (visibleConformacionLocal ? "visibleConformacionLocal(true);" : "visibleConformacionLocal(false);");
            return script;
        }

        
    }
}