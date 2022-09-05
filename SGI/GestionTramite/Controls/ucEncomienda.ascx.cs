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
    public partial class ucEncomienda : System.Web.UI.UserControl
    {
        private class itemResultadoBusquedaEncomiendas
        {
            public int IdEncomienda { get; set; }
            public string tipoAnexo { get; set; }
            public DateTime CreateDate { get; set; }
            public string DescripcionTipoTramite { get; set; }
            public string NomEstado { get; set; }
            public string Profesional { get; set; }
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
                CargarEncomiendas(id_solicitud);     

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatos en Tab_Rubros.aspx");
                //Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_Rubros.aspx"));
                throw ex;
            }

        }        

        private void CargarEncomiendas(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();

            // poner todo lo necesario al cargar la página
            
            var lstEncomiendasSolicitud = (from sol in db.Encomienda
                                           join est in db.Encomienda_Estados on sol.id_estado equals est.id_estado
                                           join encsol in db.Encomienda_SSIT_Solicitudes on sol.id_encomienda equals encsol.id_encomienda
                                           where encsol.id_solicitud == id_solicitud
                                          select new itemResultadoBusquedaEncomiendas
                                          {
                                              IdEncomienda = sol.id_encomienda,
                                              tipoAnexo = sol.tipo_anexo,
                                              CreateDate = sol.CreateDate,
                                              DescripcionTipoTramite =  sol.TipoTramite.descripcion_tipotramite,
                                              NomEstado = est.nom_estado,
                                              Profesional = sol.Profesional.Apellido + ", " + sol.Profesional.Nombre
                                          }).ToList();           

            grdAnexoTecnico.DataSource = lstEncomiendasSolicitud;
            grdAnexoTecnico.DataBind();

            db.Dispose();
        }

        DGHP_Entities db;        
       
    }
    
}