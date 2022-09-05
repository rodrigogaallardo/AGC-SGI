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
    public partial class ucRubros : System.Web.UI.UserControl
    {  
        private class itemRubros
        {
            public int id_caarubro { get; set; }
            public string ZonaDeclarada { get; set; }
            public string cod_rubro { get; set; }
            public string desc_rubro { get; set; }
            public int id_ImpactoAmbiental { get; set; }
            public string cod_ImpactoAmbiental { get; set; }
            public string nom_ImpactoAmbiental { get; set; }
            public decimal SuperficieHabilitar { get; set; }
            public string TipoActividadNombre { get; set; }
            public string nombre_tipocertificado { get; set; }
            public string BarrioAntena { get; set; }
            public string RestriccionZona { get; set; }
            public string RestriccionSuperficie { get; set; }
            public string TipoTamite { get; set; }
            public int IdTipodocReq {get;set;}
            public int IdTipoActividad { get; set; }
        }
        private class itemResultadoBusquedaRubros
        {
            public string cod_rubro { get; set; }
            public string nom_rubro { get; set; }
            public bool PregAntenaEmisora { get; set; }
            public bool EsAnterior { get; set; }
            public string TipoActividad { get; set; }
            public string RestriccionZona { get; set; }
            public string RestriccionSuperficie { get; set; }
            public string ZonaDeclarada { get; set; }
            public decimal SuperficieHabilitar { get; set; }
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

        
        private bool editar
        {
            get
            {
                bool ret = false;
                ret = hid_editar.Value.Equals("true") ? true : false;
                return ret;
            }
            set
            {
                hid_editar.Value = value.ToString();
            }

        }

        public void CargarDatos(int id_solicitud)
        {
            try
            {


                this.id_solicitud = id_solicitud;                
                CargarRubros(id_solicitud);     

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatos en Tab_Rubros.aspx");
                //Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_Rubros.aspx"));
                throw ex;
            }

        }        

        private void CargarRubros(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();

            // poner todo lo necesario al cargar la página
            int id_encomienda = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                                                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).
                                                    OrderByDescending(y => y.FechaEncomienda).Select(w => w.id_encomienda).FirstOrDefault();

            var lstRubrosSolicitud = (from sol in db.Encomienda
                                      join encrub in db.Encomienda_Rubros on sol.id_encomienda equals encrub.id_encomienda
                                      join tact in db.TipoActividad on encrub.id_tipoactividad equals tact.Id
                                      join rub in db.Rubros on encrub.cod_rubro equals rub.cod_rubro into leftRubros
                                      from lrub in leftRubros.DefaultIfEmpty()
                                      where sol.id_encomienda == id_encomienda
                                      select new itemRubros
                                      {
                                          id_caarubro = encrub.id_encomiendarubro,
                                          ZonaDeclarada = sol.ZonaDeclarada,
                                          cod_rubro = encrub.cod_rubro,
                                          desc_rubro = encrub.desc_rubro,
                                          SuperficieHabilitar = encrub.SuperficieHabilitar,
                                          TipoActividadNombre = tact.Descripcion,
                                          RestriccionZona = "pregunta.png",
                                          RestriccionSuperficie = "pregunta.png",
                                          TipoTamite = (encrub.id_tipodocreq == 1 ? "DJ" : (encrub.id_tipodocreq == 2 ? "PP" : "IP")),
                                          IdTipodocReq = encrub.id_tipodocreq,
                                          IdTipoActividad = encrub.id_tipoactividad,
                                      }).ToList();


            foreach (var item in lstRubrosSolicitud)
            {
                List<Rubros_ConsRestricciones_Result> lstres = db.Rubros_ConsRestricciones(item.ZonaDeclarada, item.cod_rubro, item.SuperficieHabilitar).ToList();
                foreach (var restriccion in lstres)
                {
                    switch (restriccion.Zona_ok.Value)
                    {
                        case 0:
                            item.RestriccionZona = "imoon imoon-blocked color-red";
                            break;
                        case 1:
                            item.RestriccionZona = "imoon imoon-ok color-green";
                            break;
                        case 2:
                            item.RestriccionZona = "imoon imoon-question color-gray";
                            break;
                    }

                    switch (restriccion.Superficie_ok.Value)
                    {
                        case 0:
                            item.RestriccionSuperficie = "imoon imoon-blocked color-red";
                            break;
                        case 1:
                            item.RestriccionSuperficie = "imoon imoon-ok color-green";
                            break;
                        case 2:
                            item.RestriccionSuperficie = "imoon imoon-question color-gray";
                            break;
                    }
                }
            }
            
            grdRubrosMostrar.DataSource = lstRubrosSolicitud;
            grdRubrosMostrar.DataBind();

            db.Dispose();
        }

        DGHP_Entities db;        
       
    }
    
}