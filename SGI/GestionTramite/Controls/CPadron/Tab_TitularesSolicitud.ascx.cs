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

namespace SGI.GestionTramite.Controls.CPadron
{
    public partial class Tab_TitularesSolicitud : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updGrillaTitularesSol, updGrillaTitularesSol.GetType(), "init_JS_updGrillaTitulares_TitularesSol", "init_JS_updGrillaTitulares_TitularesSol();", true);
            }


            if (!IsPostBack)
            {
                hid_return_url.Value = Request.Url.AbsoluteUri;
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

        private bool editar
        {
            get
            {
                bool ret = false;
                ret = hid_editar.Value.Equals("True") ? true : false;
                return ret;
            }
            set
            {
                hid_editar.Value = value.ToString();
            }

        }
        private void ComprobarSolicitud()
        {

            if (Page.RouteData.Values["id"] != null)
            {

                this.id_cpadron = Convert.ToInt32(Page.RouteData.Values["id"].ToString());
            }
            else
            {
                Server.Transfer("~/Errores/Error3001.aspx");
            }

        }

        public void CargarDatos(int id_cpadron, bool Editar)
        {
            try
            {

                this.id_cpadron = id_cpadron;
                this.editar = Editar;
                CargarDatosTitulares(id_cpadron);

                if (!this.editar)
                    titulo.Visible = false;
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatos en Tab_TitularesSolicitud.aspx");
                //Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_Titulares.aspx"));
                throw ex;
            }


        }

        public void CargarDatosTitulares(int id_cpadron)
        {

            DGHP_Entities db = new DGHP_Entities();

            var lstTitulares = (from pf in db.CPadron_Titulares_Solicitud_PersonasFisicas
                                where pf.id_cpadron == id_cpadron
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
                                    (from pj in db.CPadron_Titulares_Solicitud_PersonasJuridicas
                                     where pj.id_cpadron == id_cpadron
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


            grdTitularesSol.DataSource = lstTitulares;
            grdTitularesSol.DataBind();

            updGrillaTitularesSol.Update();
            db.Dispose();
        }


       
        private void CargarLocalidades(DropDownList ddlProvincias, DropDownList ddlLocalidades)
        {

            if (ddlProvincias.SelectedIndex > 0)
            {
                DGHP_Entities db = new DGHP_Entities();

                int idProvincia = Convert.ToInt32(ddlProvincias.SelectedValue);

                var lstLocalidades = (from l in db.Localidad
                                      join p in db.Provincia on l.IdProvincia equals p.Id
                                      where l.IdProvincia == idProvincia && l.Excluir == false
                                      orderby p.Nombre
                                      select l).ToList();


                ddlLocalidades.DataValueField = "Id";
                ddlLocalidades.DataTextField = "Depto";
                ddlLocalidades.DataSource = lstLocalidades;
                ddlLocalidades.DataBind();

                db.Dispose();
            }
            else
            {
                ddlLocalidades.Items.Clear();
            }
        }

    }
}