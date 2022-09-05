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
    public partial class ucTramiteCAA : System.Web.UI.UserControl
    {
        private class itemResultadoBusquedaTramiteCAA
        {
            public int id_solicitud { get; set; }
            public int id_caa { get; set; }
            public int id_encomienda { get; set; }
            public string nombre_tipocertificado { get; set; }
            public DateTime CreateDate { get; set; }
            public string estado_caa { get; set; }
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
                CargarTramiteCAA(id_solicitud);     

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatos en Tab_Rubros.aspx");
                //Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_Rubros.aspx"));
                throw ex;
            }

        }        

        private void CargarTramiteCAA(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();

            // poner todo lo necesario al cargar la página
            List<int> lstEncomiendasSolicitud = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud).Select(y => y.id_encomienda).ToList();

            var lstTramiteCAA = (from caa in db.CAA_Solicitudes
                                 join caaTipo in db.CAA_TiposDeCertificados on caa.id_tipocertificado equals caaTipo.id_tipocertificado
                                 join caaest in db.CAA_Estados on caa.id_estado equals caaest.id_estado
                                 where lstEncomiendasSolicitud.Contains(caa.id_encomienda_agc)
                                 select new itemResultadoBusquedaTramiteCAA
                                          {
                                             id_caa = caa.id_caa,
                                             id_solicitud = caa.id_solicitud,
                                             id_encomienda = caa.id_encomienda_agc,
                                             CreateDate = caa.CreateDate,
                                             estado_caa = caaest.nom_estado_usuario,
                                             nombre_tipocertificado = caaTipo.nombre_tipocertificado
                                          }).ToList();

            grdTramiteCAA.DataSource = lstTramiteCAA;
            grdTramiteCAA.DataBind();

            db.Dispose();
        }

        DGHP_Entities db;        
       
    }
    
}