using SGI.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucAnexoNotarial : System.Web.UI.UserControl
    {
        private class itemResultadoAnexoNotarial
        {
            public int id_actanotarial { get; set; }
            public int id_encomienda { get; set; }
            public DateTime CreateDate { get; set; }
            
            public string Escribano { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                //ScriptManager.RegisterStartupScript(updAgregarNormativa, updAgregarNormativa.GetType(), "init_JS_updAgregarNormativa", "init_JS_updAgregarNormativa();", true);
               // ScriptManager.RegisterStartupScript(updBuscarRubros, updBuscarRubros.GetType(), "init_JS_updBuscarRubros", "init_JS_updBuscarRubros();", true);
               // ScriptManager.RegisterStartupScript(updInformacionTramite, updInformacionTramite.GetType(), "init_JS_updInformacionTramite", "init_JS_updInformacionTramite();", true);
            }


            if (!IsPostBack)
            {
                hid_return_url.Value = Request.Url.AbsoluteUri;
                hid_DecimalSeparator.Value = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            }

        }
      
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
        public void CargarDatos(int id_solicitud)
        {
            try
            {


                this.id_solicitud = id_solicitud;
                CargarAnexoNotarial(id_solicitud);     

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatos en Tab_Rubros.aspx");
                //Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_Rubros.aspx"));
                throw ex;
            }

        }        

        private void CargarAnexoNotarial(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();

            // poner todo lo necesario al cargar la página

            List<int> lstEncomiendasSolicitud = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault()  == id_solicitud).Select(y => y.id_encomienda).ToList();

            var lstAnexoNotarial = (from anex in db.wsEscribanos_ActaNotarial
                                           where lstEncomiendasSolicitud.Contains(anex.id_encomienda)
                                    select new itemResultadoAnexoNotarial
                                           {
                                               id_actanotarial = anex.id_actanotarial,
                                               id_encomienda = anex.id_encomienda,
                                               CreateDate = anex.CreateDate,
                                               Escribano = anex.Escribano.ApyNom
                                           }).ToList();

            grdAnexoNotarial.DataSource = lstAnexoNotarial;
            grdAnexoNotarial.DataBind();

            db.Dispose();
        }

        DGHP_Entities db;        
       
    }
    
}