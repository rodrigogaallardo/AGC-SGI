using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucTitulares : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
               // ScriptManager.RegisterStartupScript(updGrillaTitulares, updGrillaTitulares.GetType(), "init_JS_updGrillaTitulares_Titulares", "init_JS_updGrillaTitulares_Titulares();", true);
                //ScriptManager.RegisterStartupScript(updAgregarPersonaFisica, updAgregarPersonaFisica.GetType(), "init_JS_updGrillaTitulares_Titulares", "init_JS_updGrillaTitulares_Titulares();", true);
                //ScriptManager.RegisterStartupScript(upd_ddlTipoIngresosBrutosPF, upd_ddlTipoIngresosBrutosPF.GetType(), "init_JS_upd_ddlTipoIngresosBrutosPF", "init_JS_upd_ddlTipoIngresosBrutosPF();", true);
                //ScriptManager.RegisterStartupScript(upd_txtIngresosBrutosPF, upd_txtIngresosBrutosPF.GetType(), "init_JS_upd_txtIngresosBrutosPF", "init_JS_upd_txtIngresosBrutosPF();", true);
                //ScriptManager.RegisterStartupScript(updLocalidadPF, updLocalidadPF.GetType(), "init_JS_updLocalidadPF", "init_JS_updLocalidadPF();", true);
                //ScriptManager.RegisterStartupScript(updProvinciasPF, updProvinciasPF.GetType(), "init_JS_updProvinciasPF", "init_JS_updProvinciasPF();", true);
                //ScriptManager.RegisterStartupScript(upd_ddlTipoIngresosBrutosPJ, upd_ddlTipoIngresosBrutosPJ.GetType(), "init_JS_upd_ddlTipoIngresosBrutosPJ", "init_JS_upd_ddlTipoIngresosBrutosPJ();", true);
                //ScriptManager.RegisterStartupScript(upd_txtIngresosBrutosPJ, upd_txtIngresosBrutosPJ.GetType(), "init_JS_upd_txtIngresosBrutosPJ", "init_JS_upd_txtIngresosBrutosPJ();", true);
                //ScriptManager.RegisterStartupScript(updLocalidadPJ, updLocalidadPJ.GetType(), "init_JS_updLocalidadPJ", "init_JS_updLocalidadPJ();", true);
                //ScriptManager.RegisterStartupScript(updProvinciasPJ, updProvinciasPJ.GetType(), "init_JS_updProvinciasPJ", "init_JS_updProvinciasPJ();", true);
                //ScriptManager.RegisterStartupScript(upd_ddlTipoSociedadPJ, upd_ddlTipoSociedadPJ.GetType(), "init_JS_upd_ddlTipoSociedadPJ", "init_JS_upd_ddlTipoSociedadPJ();", true);
                //ScriptManager.RegisterStartupScript(upd_txtRazonSocialPJ, upd_txtRazonSocialPJ.GetType(), "init_JS_upd_txtRazonSocialPJ", "init_JS_upd_txtRazonSocialPJ();", true);
            }


            if (!IsPostBack)
            {
                hid_return_url.Value = Request.Url.AbsoluteUri;                
                CargarDatosTitulares(this.id_solicitud);
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

                CargarDatosTitulares(id_solicitud);
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatos en Tab_Titulares.aspx");
                //Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_Titulares.aspx"));
                throw ex;
            }


        }

        public void CargarDatosTitulares(int id_Solicitud)
        {

            DGHP_Entities db = new DGHP_Entities();

            var lstTitulares = (from pf in db.SSIT_Solicitudes_Titulares_PersonasFisicas
                                where pf.id_solicitud == id_Solicitud
                                select new
                                {
                                    id_persona = pf.id_personafisica,
                                    TipoPersona = "PF",
                                    TipoPersonaDesc = "Persona Física",
                                    ApellidoNomRazon = pf.Apellido + " " + pf.Nombres,
                                    CUIT = pf.Cuit,
                                    Domicilio = pf.Calle + " " + //pf.Nro_Puerta.ToString() +
                                                (pf.Piso.Length > 0 ? " Piso: " + pf.Piso : "") +
                                                (pf.Depto.Length > 0 ? " Depto: " + pf.Depto : "")
                                }).Union(
                                    (from pj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas
                                     where pj.id_solicitud == id_Solicitud
                                     select new
                                     {
                                         id_persona = pj.id_personajuridica,
                                         TipoPersona = "PJ",
                                         TipoPersonaDesc = "Persona Jurídica",
                                         ApellidoNomRazon = pj.Razon_Social,
                                         CUIT = pj.CUIT,
                                         Domicilio = pj.Calle + " " + //(pj.NroPuerta.HasValue ? pj.NroPuerta.Value.ToString() : "") +
                                                    (pj.Piso.Length > 0 ? " Piso: " + pj.Piso : "") +
                                                    (pj.Depto.Length > 0 ? " Depto: " + pj.Depto : "")
                                     })).ToList();


            

            grdTitulares.DataSource = lstTitulares;
            grdTitulares.DataBind();
            
            db.Dispose();
        }

       

        public void EjecutarScript(UpdatePanel objupd, string script)
        {
            ScriptManager.RegisterClientScriptBlock(objupd, objupd.GetType(), "script", script, true);
        }

       

    }
}