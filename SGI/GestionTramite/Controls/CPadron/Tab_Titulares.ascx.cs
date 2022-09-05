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
    public partial class Tab_Titulares : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updGrillaTitulares, updGrillaTitulares.GetType(), "init_JS_updGrillaTitulares_Titulares", "init_JS_updGrillaTitulares_Titulares();", true);
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
                CargarTiposDeSociedades();
                CargarTiposDeDocumentoPersonal();
                CargarTiposDeIngresosBrutos();
                CargarProvincias();
                CargarDatosTitulares(this.id_cpadron);
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
        private void CargarTiposDeSociedades()
        {
            DGHP_Entities db = new DGHP_Entities();
            var lstTiposdeSociedad = db.TipoSociedad.ToList();
            ddlTipoSociedadPJ.DataSource = lstTiposdeSociedad;
            ddlTipoSociedadPJ.DataTextField = "Descripcion";
            ddlTipoSociedadPJ.DataValueField = "Id";
            ddlTipoSociedadPJ.DataBind();
            ddlTipoSociedadPJ.Items.Remove(ddlTipoSociedadPJ.Items.FindByValue("1"));   // Elimina la Sociedad Unipersonal
            ddlTipoSociedadPJ.Items.Insert(0, string.Empty);
            db.Dispose();
        }

        private void CargarTiposDeDocumentoPersonal()
        {
            DGHP_Entities db = new DGHP_Entities();
            var lstTipoDocPersonal = db.TipoDocumentoPersonal.ToList();

            ddlTipoDocumentoPF.DataSource = lstTipoDocPersonal;
            ddlTipoDocumentoPF.DataTextField = "Nombre";
            ddlTipoDocumentoPF.DataValueField = "TipoDocumentoPersonalId";
            ddlTipoDocumentoPF.DataBind();
            ddlTipoDocumentoPF.Items.Insert(0, string.Empty);

            //ddlTipoDocumentoTitSH.DataSource = lstTipoDocPersonal;
            //ddlTipoDocumentoTitSH.DataTextField = "Nombre";
            //ddlTipoDocumentoTitSH.DataValueField = "TipoDocumentoPersonalId";
            //ddlTipoDocumentoTitSH.DataBind();
            //ddlTipoDocumentoTitSH.Items.Insert(0, string.Empty);

            //ddlTipoDocumentoFirPJ.DataSource = lstTipoDocPersonal;
            //ddlTipoDocumentoFirPJ.DataTextField = "Nombre";
            //ddlTipoDocumentoFirPJ.DataValueField = "TipoDocumentoPersonalId";
            //ddlTipoDocumentoFirPJ.DataBind();
            //ddlTipoDocumentoFirPJ.Items.Insert(0, string.Empty);

            //ddlTipoDocumentoFirPF.DataSource = lstTipoDocPersonal;
            //ddlTipoDocumentoFirPF.DataTextField = "Nombre";
            //ddlTipoDocumentoFirPF.DataValueField = "TipoDocumentoPersonalId";
            //ddlTipoDocumentoFirPF.DataBind();
            //ddlTipoDocumentoFirPF.Items.Insert(0, string.Empty);
        }

        private void CargarTiposDeIngresosBrutos()
        {
            DGHP_Entities db = new DGHP_Entities();
            var lstTipoIngresosBrutos = db.TiposDeIngresosBrutos.ToList();

            ddlTipoIngresosBrutosPF.DataSource = lstTipoIngresosBrutos;
            ddlTipoIngresosBrutosPF.DataTextField = "nom_tipoiibb";
            ddlTipoIngresosBrutosPF.DataValueField = "id_tipoiibb";
            ddlTipoIngresosBrutosPF.DataBind();
            ddlTipoIngresosBrutosPF.Items.Insert(0, string.Empty);

            ddlTipoIngresosBrutosPJ.DataSource = lstTipoIngresosBrutos;
            ddlTipoIngresosBrutosPJ.DataTextField = "nom_tipoiibb";
            ddlTipoIngresosBrutosPJ.DataValueField = "id_tipoiibb";
            ddlTipoIngresosBrutosPJ.DataBind();
            ddlTipoIngresosBrutosPJ.Items.Insert(0, string.Empty);

            db.Dispose();
        }

        protected void ddlTipoSociedadPJ_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id_tiposociedad = 0;

            if (int.TryParse(ddlTipoSociedadPJ.SelectedValue, out id_tiposociedad))
            {

                DGHP_Entities db = new DGHP_Entities();

                var tiposoc = db.TipoSociedad.Where(x => x.Id == id_tiposociedad);

                if (tiposoc != null && id_tiposociedad == Constants.SociedadHecho)
                {
                    lblRazonSocialPJ.Text = "Nombre de Fantasía:";
                    //pnlAgregarTitularSH.Style["display"] = "block";


                    //grdTitularesSH.DataSource = dtTitularesSHCargados();
                    //grdTitularesSH.DataBind();

                }
                else
                {
                    lblRazonSocialPJ.Text = "Razon Social:";
                    //pnlAgregarTitularSH.Style["display"] = "none";
                }
                updAgregarPersonaJuridica.Update();
                db.Dispose();
            }

        }


        protected void ddlTipoIngresosBrutosPJ_SelectedIndexChanged(object sender, EventArgs e)
        {
            string expresion = "";
            string formatoIIBB = "";
            hid_IngresosBrutosPJ_formato.Value = "";
            hid_IngresosBrutosPJ_formato.Value = "";

            if (ddlTipoIngresosBrutosPJ.SelectedValue.Length > 0)
            {
                DGHP_Entities db = new DGHP_Entities();

                int id_tipoiibb = int.Parse(ddlTipoIngresosBrutosPJ.SelectedValue);
                var lstTiposDeIIBB = db.TiposDeIngresosBrutos.Where(x => x.id_tipoiibb == id_tipoiibb);

                foreach (var item in lstTiposDeIIBB)
                {
                    formatoIIBB = item.formato_tipoiibb;
                    string[] matrizFormato = formatoIIBB.Split(Convert.ToChar("-"));
                    foreach (string elemento in matrizFormato)
                    {
                        if (elemento.Length > 0)
                            expresion += "-\\d{" + elemento.Length + "}";
                    }
                    if (expresion.Length > 0)
                        expresion = expresion.Substring(1);

                    hid_IngresosBrutosPJ_formato.Value = formatoIIBB;
                    hid_IngresosBrutosPJ_expresion.Value = expresion;
                }
            }
            if (expresion.Length == 0)
            {
                txtIngresosBrutosPJ.Text = "";
                txtIngresosBrutosPJ.Enabled = false;
            }
            else
            {
                txtIngresosBrutosPJ.Enabled = true;
            }
        }

        protected void ddlTipoIngresosBrutosPF_SelectedIndexChanged(object sender, EventArgs e)
        {

            string expresion = "";
            string formatoIIBB = "";
            hid_IngresosBrutosPF_formato.Value = "";
            hid_IngresosBrutosPF_formato.Value = "";

            if (ddlTipoIngresosBrutosPF.SelectedValue.Length > 0)
            {
                DGHP_Entities db = new DGHP_Entities();

                int id_tipoiibb = int.Parse(ddlTipoIngresosBrutosPF.SelectedValue);
                var lstTiposDeIIBB = db.TiposDeIngresosBrutos.Where(x => x.id_tipoiibb == id_tipoiibb);

                foreach (var item in lstTiposDeIIBB)
                {
                    formatoIIBB = item.formato_tipoiibb;
                    string[] matrizFormato = formatoIIBB.Split(Convert.ToChar("-"));
                    foreach (string elemento in matrizFormato)
                    {
                        if (elemento.Length > 0)
                            expresion += "-\\d{" + elemento.Length + "}";
                    }
                    if (expresion.Length > 0)
                        expresion = expresion.Substring(1);

                    hid_IngresosBrutosPF_formato.Value = formatoIIBB;
                    hid_IngresosBrutosPF_expresion.Value = expresion;
                }
            }
            if (expresion.Length == 0)
            {
                txtIngresosBrutosPF.Text = "";
                txtIngresosBrutosPF.Enabled = false;
            }
            else
            {
                txtIngresosBrutosPF.Enabled = true;
            }
        }

        protected void ddlProvinciaPJ_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarLocalidades(ddlProvinciaPJ, ddlLocalidadPJ);
        }

        private bool ValidarDatosPantallaPJ()
        {
            bool ret = true;

            int id_personajuridica = 0;
            int.TryParse(hid_id_titular_pj.Value, out id_personajuridica);

            DGHP_Entities db = new DGHP_Entities();

            //Valida si existe una persona física ya ingresada con el mismo numero de CUIT.
            var existeTitular = db.CPadron_Titulares_PersonasJuridicas.Where(x => x.id_cpadron == this.id_cpadron && txtCuitPJ.Text.Trim() == x.CUIT && x.id_personajuridica != id_personajuridica).Count();

            if (existeTitular > 0)
            {
                ValExiste_TitularPJ.Style["display"] = "inline-block";
                ret = false;
            }
            return ret;
        }

        private void CargarDatosTitularPF(int id_personafisica)
        {
            DGHP_Entities db = new DGHP_Entities();

            var pf = db.CPadron_Titulares_PersonasFisicas.FirstOrDefault(x => x.id_personafisica == id_personafisica);

            if (pf != null)
            {

                //if (pf.MismoFirmante)
                //{
                //    optMismaPersona.Checked = true;
                //    optOtraPersona.Checked = false;

                //    txtApellidoFirPF.Text = "";
                //    txtNombresFirPF.Text = "";
                //    ddlTipoDocumentoFirPF.ClearSelection();
                //    txtNroDocumentoFirPF.Text = "";
                //    ddlTipoCaracterLegalFirPF.ClearSelection();
                //}
                //else
                //{

                //    optOtraPersona.Checked = true;
                //    optMismaPersona.Checked = false;

                //    var firmante = db.CPadron_Titulares_PersonasFisicas.FirstOrDefault(x => x.id_personafisica == id_personafisica);
                //    var firmante = db.CAA_Firmantes_PersonasFisicas.FirstOrDefault(x => x.id_personafisica == id_personafisica);

                //    if (firmante != null)
                //    {
                //        txtApellidoFirPF.Text = firmante.Apellido;
                //        txtNombresFirPF.Text = firmante.Nombres;
                //        ddlTipoDocumentoFirPF.SelectedValue = firmante.id_tipodoc_personal.ToString();
                //        txtNroDocumentoFirPF.Text = firmante.Nro_Documento;
                //        ddlTipoCaracterLegalFirPF.SelectedValue = firmante.id_tipocaracter.ToString();

                //    }
                //    pnlOtraPersona.Style["display"] = "block";

                //}

                txtApellidosPF.Text = pf.Apellido;
                txtNombresPF.Text = pf.Nombres;
                txtNroDocumentoPF.Text = pf.Nro_Documento.ToString();
                txtCuitPF.Text = pf.Cuit;
                txtIngresosBrutosPF.Text = pf.Ingresos_Brutos;
                txtCallePF.Text = pf.Calle;
                txtNroPuertaPF.Text = pf.Nro_Puerta.ToString();
                txtPisoPF.Text = pf.Piso;
                txtDeptoPF.Text = pf.Depto;
                txtCPPF.Text = pf.Codigo_Postal;
                txtTelefonoPF.Text = pf.Telefono;
                txtTelefonoMovilPF.Text = pf.TelefonoMovil;
                //txtSmsPF.Text = pf.Sms;
                txtEmailPF.Text = pf.Email;

                ddlTipoDocumentoPF.SelectedValue = pf.id_tipodoc_personal.ToString();
                ddlTipoIngresosBrutosPF.SelectedValue = pf.id_tipoiibb.ToString();
                ddlTipoIngresosBrutosPF_SelectedIndexChanged(ddlTipoIngresosBrutosPF, new EventArgs());

                var localidad = db.Localidad.FirstOrDefault(x => x.Id == pf.Id_Localidad);

                if (localidad != null)
                {
                    ddlProvinciaPF.SelectedValue = localidad.IdProvincia.ToString();
                    CargarLocalidades(ddlProvinciaPF, ddlLocalidadPF);
                    ddlLocalidadPF.SelectedValue = pf.Id_Localidad.ToString();
                }
            }

            db.Dispose();
        }
        private void CargarDatosTitularPJ(int id_personajuridica)
        {
            DGHP_Entities db = new DGHP_Entities();

            var pj = db.CPadron_Titulares_PersonasJuridicas.Where(x => x.id_personajuridica == id_personajuridica).FirstOrDefault();

            if (pj != null)
            {
                hid_id_titular_pj.Value = id_personajuridica.ToString();

                ddlTipoSociedadPJ.SelectedValue = pj.Id_TipoSociedad.ToString();
                txtRazonSocialPJ.Text = pj.Razon_Social;
                txtCuitPJ.Text = pj.CUIT;
                ddlTipoIngresosBrutosPJ.SelectedValue = pj.id_tipoiibb.ToString();
                txtIngresosBrutosPJ.Text = pj.Nro_IIBB;
                txtCallePJ.Text = pj.Calle;
                txtNroPuertaPJ.Text = pj.NroPuerta.ToString();
                txtPisoPJ.Text = pj.Piso;
                txtDeptoPJ.Text = pj.Depto;
                txtCPPJ.Text = pj.Codigo_Postal;
                txtTelefonoPJ.Text = pj.Telefono;
                txtEmailPJ.Text = pj.Email;

                ddlTipoIngresosBrutosPJ_SelectedIndexChanged(ddlTipoIngresosBrutosPJ, new EventArgs());

                var localidad = db.Localidad.FirstOrDefault(x => x.Id == pj.id_localidad);

                if (localidad != null)
                {
                    ddlProvinciaPJ.SelectedValue = localidad.IdProvincia.ToString();
                    CargarLocalidades(ddlProvinciaPJ, ddlLocalidadPJ);
                    ddlLocalidadPJ.SelectedValue = pj.id_localidad.ToString();
                }

                if (pj.Id_TipoSociedad == Constants.SociedadHecho)
                {
                    lblRazonSocialPJ.Text = "Nombre de Fantasía:";
                    //pnlAgregarTitularSH.Style["display"] = "block";
                    //pnlFirmantesPJ.Style["display"] = "none";

                }

            }

            db.Dispose();
        }
        protected void btnShowAgregarTitular_Click(object sender, EventArgs e)
        {
            LinkButton btnAgregarTitular = (LinkButton)sender;

            if (btnAgregarTitular.ID == "btnShowAgregarPF")//PF
            {
                hid_id_titular_pf.Value = "0";
                LimpiarControlesABMPF();
                updAgregarPersonaFisica.Update();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmAgregarPersonaFisica", "$('#frmAgregarPersonaFisica').modal();", true);
            }
            if (btnAgregarTitular.ID == "btnShowAgregarPJ")//PJ
            {
                hid_id_titular_pj.Value = "0";
                LimpiarControlesABMPJ();
                updAgregarPersonaJuridica.Update();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmAgregarPersonaJuridica", "$('#frmAgregarPersonaJuridica').modal();", true);
            }

        }

        private void LimpiarControlesABMPJ()
        {
            txtRazonSocialPJ.Text = "";
            txtCuitPJ.Text = "";
            txtIngresosBrutosPJ.Text = "";
            txtCallePJ.Text = "";
            txtNroPuertaPJ.Text = "";
            txtPisoPJ.Text = "";
            txtDeptoPJ.Text = "";
            txtCPPJ.Text = "";
            txtTelefonoPJ.Text = "";
            txtEmailPJ.Text = "";
            ValExiste_TitularPJ.Style["display"] = "none";

            ddlTipoSociedadPJ.ClearSelection();
            ddlProvinciaPJ.ClearSelection();
            ddlTipoIngresosBrutosPJ.ClearSelection();
            CargarLocalidades(ddlProvinciaPJ, ddlLocalidadPJ);

            //DataTable dtTitSH = dtTitularesSHCargados();

            //dtTitSH.Clear();

            //grdTitularesSH.DataSource = dtTitSH;
            //grdTitularesSH.DataBind();

            lblRazonSocialPJ.Text = "Razon Social:";
            //pnlAgregarTitularSH.Style["display"] = "none";

        }

        protected void btnAceptarTitPJ_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();

            if (ValidarDatosPantallaPJ())
            {
                try
                {

                    Guid userid = (Guid)Functions.GetUserId();

                    int id_personajuridica = 0;
                    int id_tiposociedad = 0;
                    string Razon_Social = txtRazonSocialPJ.Text.Trim();
                    string Cuit = txtCuitPJ.Text.Trim();
                    int id_tipoiibb = 0;
                    string Ingresos_Brutos = txtIngresosBrutosPJ.Text.Trim();
                    string Calle = txtCallePJ.Text.Trim();
                    int Nro_Puerta = 0;
                    string Piso = txtPisoPJ.Text.Trim();
                    string Depto = txtDeptoPJ.Text.Trim();
                    int id_Localidad = 0;
                    string Codigo_Postal = txtCPPJ.Text.Trim();
                    string Telefono = txtTelefonoPJ.Text.Trim();
                    string Email = txtEmailPJ.Text.Trim();

                    int.TryParse(ddlTipoSociedadPJ.SelectedValue, out id_tiposociedad);
                    int.TryParse(ddlTipoIngresosBrutosPJ.SelectedValue, out id_tipoiibb);
                    int.TryParse(txtNroPuertaPJ.Text.Trim(), out Nro_Puerta);
                    int.TryParse(ddlLocalidadPJ.SelectedValue, out id_Localidad);

                    int.TryParse(hid_id_titular_pj.Value, out id_personajuridica);

                    using (TransactionScope Tran = new TransactionScope())
                    {
                        try
                        {

                            if (id_personajuridica > 0)
                                db.CPadron_EliminarPersonaJuridica(this.id_cpadron, id_personajuridica, this.validar_estado);

                            ObjectParameter param_id_personajuridica = new ObjectParameter("id_personajuridica", typeof(int));

                            db.CPadron_AgregarTitularesPersonasJuridicas(id_cpadron, id_tiposociedad, Razon_Social, Cuit,
                                id_tipoiibb, Ingresos_Brutos, Calle, Nro_Puerta, Piso, Depto, id_Localidad, Codigo_Postal,
                                Telefono, Email, userid, param_id_personajuridica, this.validar_estado);

                            id_personajuridica = Convert.ToInt32(param_id_personajuridica.Value);


                            //if (id_tiposociedad == Constants.SociedadHecho)
                            //{

                            //    // Solo Sociedad de Hecho
                            //    DataTable dtFirmantesSH = dtFirmantesSHCargados();
                            //    DataTable dtTitularesSH = dtTitularesSHCargados();

                            //    foreach (DataRow dr in dtFirmantesSH.Rows)
                            //    {
                            //        ObjectParameter param_id_firmante_pj = new ObjectParameter("id_firmante_pj", typeof(int));

                            //        db.CPadron_AgregarFirmantesPersonasJuridicas(id_cpadron, id_personajuridica, dr["Apellidos"].ToString(), dr["Nombres"].ToString(),
                            //                                        int.Parse(dr["id_tipodoc_personal"].ToString()), dr["NroDoc"].ToString(), int.Parse(dr["id_tipocaracter"].ToString()), dr["cargo_firmante"].ToString(),
                            //                                        dr["email"].ToString(),  param_id_firmante_pj);

                            //        int id_firmante_pj = Convert.ToInt32(param_id_firmante_pj.Value);

                            //        // Da te alta el Titular de la SH vinculado al Firmante que se acaba de dar de alta.
                            //        foreach (DataRow rowTitularSH in dtTitularesSH.Rows)
                            //        {
                            //            if ((Guid)rowTitularSH["rowid"] == (Guid)dr["rowid_titular"])
                            //            {

                            //                ObjectParameter param_id_titularpj = new ObjectParameter("id_titular_pj", typeof(int));
                            //                db.CPadron_AgregarTitularesSHPersonasFisicas(id_cpadron, id_personajuridica,  rowTitularSH["Apellidos"].ToString(), rowTitularSH["Nombres"].ToString(),
                            //                                    int.Parse(rowTitularSH["id_tipodoc_personal"].ToString()), rowTitularSH["NroDoc"].ToString(), rowTitularSH["email"].ToString(),
                            //                                     param_id_titularpj);

                            //                break;
                            //            }
                            //        }


                            //    }
                            //}

                            CargarDatosTitulares(this.id_cpadron);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmAgregarPersonaJuridica", "$('#frmAgregarPersonaJuridica').modal('hide');", true);
                            //this.EjecutarScript(updBotonesAgregarPJ, "hidefrmAgregarPersonaJuridica();$('#Req_FirmantesPJ').hide();");

                            Tran.Complete();

                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, ex.Message);
                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updBotonesAgregarPJ, "showfrmError();");
                }
            }
        }

        //protected void btnEditarTitularSH_Click(object sender, EventArgs e)
        //{

        //    LimpiarDatosABMTitularesSH();

        //    LinkButton btn = (LinkButton)sender;
        //    GridViewRow row = (GridViewRow)btn.Parent.Parent;
        //    DataTable dtTitularesSH = dtTitularesSHCargados();
        //    DataTable dtFirmantesSH = dtFirmantesSHCargados();

        //    DataRow drTitularesSH = dtTitularesSH.Rows[row.RowIndex];
        //    DataRow drFirmantesSH = null;

        //    foreach (DataRow rowFirmante in dtFirmantesSH.Rows)
        //    {
        //        if ((Guid)rowFirmante["rowid_titular"] == (Guid)drTitularesSH["rowid"])
        //        {
        //            drFirmantesSH = rowFirmante;
        //            break;
        //        }

        //    }

        //    txtApellidosTitSH.Text = drTitularesSH["Apellidos"].ToString();
        //    txtNombresTitSH.Text = drTitularesSH["Nombres"].ToString();
        //    txtNroDocumentoTitSH.Text = drTitularesSH["NroDoc"].ToString();
        //    ddlTipoDocumentoTitSH.SelectedValue = drTitularesSH["id_tipodoc_personal"].ToString();
        //    txtEmailTitSH.Text = drTitularesSH["email"].ToString();


        //    if (drFirmantesSH != null)
        //    {
        //        bool misma_persona = Convert.ToBoolean(drFirmantesSH["misma_persona"]);
        //        // Primero se deben limpir y luego setear los valores de checked
        //        optMismaPersonaSH.Checked = false;
        //        optOtraPersonaSH.Checked = false;
        //        optMismaPersonaSH.Checked = misma_persona;
        //        optOtraPersonaSH.Checked = !misma_persona;
        //        if (!misma_persona)
        //        {
        //            txtApellidosFirSH.Text = drFirmantesSH["Apellidos"].ToString();
        //            txtNombresFirSH.Text = drFirmantesSH["Nombres"].ToString();
        //            txtNroDocumentoFirSH.Text = drFirmantesSH["NroDoc"].ToString();
        //            ddlTipoDocumentoFirSH.SelectedValue = drFirmantesSH["id_tipodoc_personal"].ToString();
        //            txtEmailFirSH.Text = drFirmantesSH["email"].ToString();
        //            ddlTipoCaracterLegalFirSH.SelectedValue = drFirmantesSH["id_tipocaracter"].ToString();
        //            txtCargoFirSH.Text = drFirmantesSH["cargo_firmante"].ToString();
        //            pnlFirSH.Style["display"] = "block";

        //        }

        //    }

        //    hid_rowindex_titSH.Value = row.RowIndex.ToString();
        //    updABMTitularesSH.Update();

        //    Functions.EjecutarScript(updgrillaTitularesSH, "showfrmAgregarTitularesSH_Titulares();");
        //}

        //private DataTable dtTitularesSHCargados()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("rowid", typeof(Guid));
        //    dt.Columns.Add("Apellidos", typeof(string));
        //    dt.Columns.Add("Nombres", typeof(string));
        //    dt.Columns.Add("TipoDoc", typeof(string));
        //    dt.Columns.Add("NroDoc", typeof(string));
        //    dt.Columns.Add("id_tipodoc_personal", typeof(int));
        //    dt.Columns.Add("email", typeof(string));



        //    foreach (GridViewRow row in grdTitularesSH.Rows)
        //    {
        //        DataRow datarw;
        //        datarw = dt.NewRow();

        //        HiddenField hid_id_tipodoc_grdTitularesSH = (HiddenField)grdTitularesSH.Rows[row.RowIndex].Cells[0].FindControl("hid_id_tipodoc_grdTitularesSH");
        //        HiddenField hid_rowid_grdTitularesSH = (HiddenField)grdTitularesSH.Rows[row.RowIndex].Cells[6].FindControl("hid_rowid_grdTitularesSH");

        //        datarw[0] = Guid.Parse(hid_rowid_grdTitularesSH.Value);
        //        datarw[1] = HttpUtility.HtmlDecode(row.Cells[1].Text);
        //        datarw[2] = HttpUtility.HtmlDecode(row.Cells[2].Text);
        //        datarw[3] = HttpUtility.HtmlDecode(row.Cells[3].Text);
        //        datarw[4] = HttpUtility.HtmlDecode(row.Cells[4].Text);
        //        datarw[5] = int.Parse(hid_id_tipodoc_grdTitularesSH.Value);
        //        datarw[6] = HttpUtility.HtmlDecode(row.Cells[5].Text);

        //        dt.Rows.Add(datarw);

        //    }

        //    return dt;
        //}
        //private DataTable dtFirmantesSHCargados()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("FirmanteDe", typeof(string));
        //    dt.Columns.Add("Apellidos", typeof(string));
        //    dt.Columns.Add("Nombres", typeof(string));
        //    dt.Columns.Add("TipoDoc", typeof(string));
        //    dt.Columns.Add("NroDoc", typeof(string));
        //    dt.Columns.Add("nom_tipocaracter", typeof(string));
        //    dt.Columns.Add("id_tipodoc_personal", typeof(int));
        //    dt.Columns.Add("id_tipocaracter", typeof(int));
        //    dt.Columns.Add("cargo_firmante", typeof(string));
        //    dt.Columns.Add("email", typeof(string));
        //    dt.Columns.Add("rowid", typeof(Guid));
        //    dt.Columns.Add("rowid_titular", typeof(Guid));
        //    dt.Columns.Add("misma_persona", typeof(bool));


        //    foreach (GridViewRow row in grdFirmantesSH.Rows)
        //    {
        //        DataRow datarw;
        //        datarw = dt.NewRow();

        //        HiddenField hid_id_tipodoc_grdFirmantesSH = (HiddenField)grdFirmantesSH.Rows[row.RowIndex].FindControl("hid_id_tipodoc_grdFirmantesSH");
        //        HiddenField hid_id_caracter_grdFirmantesSH = (HiddenField)grdFirmantesSH.Rows[row.RowIndex].FindControl("hid_id_caracter_grdFirmantesSH");
        //        HiddenField hid_rowid_grdFirmantesSH = (HiddenField)grdFirmantesSH.Rows[row.RowIndex].FindControl("hid_rowid_grdFirmantesSH");
        //        HiddenField hid_rowid_titularSH_grdFirmantesSH = (HiddenField)grdFirmantesSH.Rows[row.RowIndex].FindControl("hid_rowid_titularSH_grdFirmantesSH");
        //        HiddenField hid_misma_persona_grdFirmantesSH = (HiddenField)grdFirmantesSH.Rows[row.RowIndex].FindControl("hid_misma_persona_grdFirmantesSH");


        //        datarw[0] = HttpUtility.HtmlDecode(row.Cells[0].Text);
        //        datarw[1] = HttpUtility.HtmlDecode(row.Cells[1].Text);
        //        datarw[2] = HttpUtility.HtmlDecode(row.Cells[2].Text);
        //        datarw[3] = HttpUtility.HtmlDecode(row.Cells[3].Text);
        //        datarw[4] = HttpUtility.HtmlDecode(row.Cells[4].Text);
        //        datarw[5] = HttpUtility.HtmlDecode(row.Cells[5].Text);
        //        datarw[6] = int.Parse(hid_id_tipodoc_grdFirmantesSH.Value);
        //        datarw[7] = int.Parse(hid_id_caracter_grdFirmantesSH.Value);
        //        datarw[8] = HttpUtility.HtmlDecode(row.Cells[6].Text).Trim();
        //        datarw[9] = HttpUtility.HtmlDecode(row.Cells[7].Text).Trim();
        //        datarw[10] = Guid.Parse(hid_rowid_grdFirmantesSH.Value);
        //        datarw[11] = Guid.Parse(hid_rowid_titularSH_grdFirmantesSH.Value);
        //        datarw[12] = Convert.ToBoolean(hid_misma_persona_grdFirmantesSH.Value);
        //        dt.Rows.Add(datarw);

        //    }

        //    return dt;
        //}
        //private void LimpiarDatosABMTitularesSH()
        //{

        //    txtApellidosTitSH.Text = "";
        //    txtNombresTitSH.Text = "";
        //    ddlTipoDocumentoTitSH.ClearSelection();
        //    txtNroDocumentoTitSH.Text = "";
        //    txtEmailTitSH.Text = "";

        //    txtApellidosFirSH.Text = "";
        //    txtNombresFirSH.Text = "";
        //    ddlTipoDocumentoFirSH.ClearSelection();
        //    txtNroDocumentoFirSH.Text = "";
        //    txtEmailFirSH.Text = "";

        //    txtCargoFirSH.Text = "";
        //    pnlCargoFirmanteSH.Style["display"] = "none";
        //    ddlTipoCaracterLegalFirSH.ClearSelection();

        //    optOtraPersonaSH.Checked = false;
        //    optMismaPersonaSH.Checked = true;
        //    pnlFirSH.Style["display"] = "none";

        //}

        //protected void btnAgregarTitularSH_Click(object sender, EventArgs e)
        //{
        //    LimpiarDatosABMTitularesSH();
        //    hid_rowindex_titSH.Value = "";
        //    updABMTitularesSH.Update();
        //    Functions.EjecutarScript(updBotonesAgregarTitularSH, "showfrmAgregarTitularesSH_Titulares();");

        //}
        //private DataTable dtFirmantesCargados()
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Apellidos", typeof(string));
        //    dt.Columns.Add("Nombres", typeof(string));
        //    dt.Columns.Add("TipoDoc", typeof(string));
        //    dt.Columns.Add("NroDoc", typeof(string));
        //    dt.Columns.Add("nom_tipocaracter", typeof(string));
        //    dt.Columns.Add("id_tipodoc_personal", typeof(int));
        //    dt.Columns.Add("email", typeof(string));
        //    dt.Columns.Add("id_tipocaracter", typeof(int));
        //    dt.Columns.Add("cargo_firmante_pj", typeof(string));
        //    dt.Columns.Add("rowindex", typeof(int));


        //    foreach (GridViewRow row in grdFirmantesPJ.Rows)
        //    {
        //        DataRow datarw;
        //        datarw = dt.NewRow();

        //        HiddenField hid_id_tipodoc_grdFirmantes = (HiddenField)grdFirmantesPJ.Rows[row.RowIndex].Cells[0].FindControl("hid_id_tipodoc_grdFirmantes");
        //        HiddenField hid_id_caracter_grdFirmantes = (HiddenField)grdFirmantesPJ.Rows[row.RowIndex].Cells[0].FindControl("hid_id_caracter_grdFirmantes");


        //        datarw[0] = HttpUtility.HtmlDecode(row.Cells[1].Text);
        //        datarw[1] = HttpUtility.HtmlDecode(row.Cells[2].Text);
        //        datarw[2] = HttpUtility.HtmlDecode(row.Cells[3].Text);
        //        datarw[3] = HttpUtility.HtmlDecode(row.Cells[4].Text);
        //        datarw[4] = HttpUtility.HtmlDecode(row.Cells[6].Text);
        //        datarw[5] = int.Parse(hid_id_tipodoc_grdFirmantes.Value);
        //        datarw[6] = HttpUtility.HtmlDecode(row.Cells[5].Text);
        //        datarw[7] = int.Parse(hid_id_caracter_grdFirmantes.Value);
        //        datarw[8] = HttpUtility.HtmlDecode(row.Cells[7].Text).Trim();
        //        datarw[9] = row.RowIndex;

        //        dt.Rows.Add(datarw);

        //    }

        //    return dt;
        //}

        //protected void btnAceptarFirPJ_Click(object sender, EventArgs e)
        //{

        //    DataTable dt = dtFirmantesCargados();
        //    bool Validation = true;

        //    if (hid_rowindex_fir.Value.Length == 0)
        //    {

        //        //Agregar firmante (Persona Jurídica)
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            if (dr["TipoDoc"].ToString().Equals(ddlTipoDocumentoFirPJ.SelectedItem.Text.Trim()) &&
        //                dr["NroDoc"].ToString().Equals(txtNroDocumentoFirPJ.Text.Trim()))
        //            {
        //                ValExiste_TipoNroDocFirPJ.Style["display"] = "inline-block";
        //                updFirmantePJ.Update();
        //                Validation = false;
        //            }
        //        }

        //        if (Validation)
        //        {
        //            dt.Rows.Add(txtApellidosFirPJ.Text.Trim(), txtNombresFirPJ.Text.Trim(), ddlTipoDocumentoFirPJ.SelectedItem.Text.Trim(), txtNroDocumentoFirPJ.Text.Trim(),
        //              ddlTipoCaracterLegalFirPJ.SelectedItem.Text, int.Parse(ddlTipoDocumentoFirPJ.SelectedValue), txtEmailFirPJ.Text.Trim(), int.Parse(ddlTipoCaracterLegalFirPJ.SelectedValue),
        //              txtCargoFirPJ.Text.Trim(), dt.Rows.Count);

        //        }

        //    }
        //    else
        //    {

        //        //Editar firmante (Persona Jurídica)
        //        foreach (DataRow drVal in dt.Rows)
        //        {
        //            if (drVal["TipoDoc"].ToString().Equals(ddlTipoDocumentoFirPJ.SelectedItem.Text.Trim()) &&
        //                drVal["NroDoc"].ToString().Equals(txtNroDocumentoFirPJ.Text.Trim()) &&
        //                drVal["rowindex"].ToString() != hid_rowindex_fir.Value)
        //            {
        //                ValExiste_TipoNroDocFirPJ.Style["display"] = "inline-block";
        //                updFirmantePJ.Update();
        //                Validation = false;
        //            }
        //        }

        //        if (Validation)
        //        {
        //            int rowindex = int.Parse(hid_rowindex_fir.Value);
        //            DataRow dr = dt.Rows[rowindex];

        //            dt.Rows[rowindex]["Apellidos"] = txtApellidosFirPJ.Text.Trim();
        //            dt.Rows[rowindex]["Nombres"] = txtNombresFirPJ.Text.Trim();
        //            dt.Rows[rowindex]["TipoDoc"] = ddlTipoDocumentoFirPJ.SelectedItem.Text;
        //            dt.Rows[rowindex]["NroDoc"] = txtNroDocumentoFirPJ.Text.Trim();
        //            dt.Rows[rowindex]["nom_tipocaracter"] = ddlTipoCaracterLegalFirPJ.SelectedItem.Text;
        //            dt.Rows[rowindex]["id_tipodoc_personal"] = int.Parse(ddlTipoDocumentoFirPJ.SelectedValue);
        //            dt.Rows[rowindex]["email"] = txtEmailFirPJ.Text.Trim();
        //            dt.Rows[rowindex]["id_tipocaracter"] = int.Parse(ddlTipoCaracterLegalFirPJ.SelectedValue);
        //            dt.Rows[rowindex]["cargo_firmante_pj"] = txtCargoFirPJ.Text.Trim();
        //        }


        //    }

        //    grdFirmantesPJ.DataSource = dt;
        //    grdFirmantesPJ.DataBind();
        //    updgrdFirmantesPJ.Update();

        //    if (Validation)
        //        Functions.EjecutarScript(updgrdFirmantesPJ, "hidefrmAgregarFirmantePJ_Titulares();");

        //}

        private void CargarProvincias()
        {
            DGHP_Entities db = new DGHP_Entities();
            var lstProvincias = db.Provincia.OrderBy(x => x.Nombre).ToList();

            ddlProvinciaPJ.DataValueField = "Id";
            ddlProvinciaPJ.DataTextField = "Nombre";
            ddlProvinciaPJ.DataSource = lstProvincias;
            ddlProvinciaPJ.DataBind();
            ddlProvinciaPJ.Items.Insert(0, string.Empty);

            ddlProvinciaPF.DataValueField = "Id";
            ddlProvinciaPF.DataTextField = "Nombre";
            ddlProvinciaPF.DataSource = lstProvincias;
            ddlProvinciaPF.DataBind();
            ddlProvinciaPF.Items.Insert(0, string.Empty);
        }

        private void LimpiarControlesABMPF()
        {

            txtApellidosPF.Text = "";
            txtNombresPF.Text = "";
            txtNroDocumentoPF.Text = "";
            txtCuitPF.Text = "";
            txtIngresosBrutosPF.Text = "";
            txtCallePF.Text = "";
            txtNroPuertaPF.Text = "";
            txtPisoPF.Text = "";
            txtDeptoPF.Text = "";
            txtCPPF.Text = "";
            txtTelefonoPF.Text = "";
            txtTelefonoMovilPF.Text = "";
            txtEmailPF.Text = "";
            ValExiste_TitularPF.Style["display"] = "none";
            ddlTipoDocumentoPF.ClearSelection();
            ddlTipoIngresosBrutosPF.ClearSelection();
            ddlProvinciaPF.ClearSelection();
            CargarLocalidades(ddlProvinciaPF, ddlLocalidadPF);

        }

        private bool ValidarDatosPantallaPF()
        {

            bool ret = true;

            DGHP_Entities db = new DGHP_Entities();

            int id_personafisica = 0;
            int.TryParse(hid_id_titular_pf.Value, out id_personafisica);

            //Valida si existe una persona física ya ingresada con el mismo numero de CUIT.
            bool existeTitular = db.CPadron_Titulares_PersonasFisicas.Count(x => x.id_cpadron == this.id_cpadron && x.Cuit == txtCuitPF.Text.Trim() && x.id_personafisica != id_personafisica) > 0;
            if (existeTitular)
            {
                ValExiste_TitularPF.Style["display"] = "inline-block";
                ret = false;
            }


            db.Dispose();
            return ret;
        }

        protected void btnAceptarTitPF_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();

            if (ValidarDatosPantallaPF())
            {
                try
                {
                    Guid userid = (Guid)Functions.GetUserId();
                    int id_personafisica = 0;
                    string Apellido = txtApellidosPF.Text.Trim();
                    string Nombres = txtNombresPF.Text.Trim();
                    int id_tipodoc_personal = 0;
                    string Nro_Documento = txtNroDocumentoPF.Text.Trim();
                    string Cuit = txtCuitPF.Text.Trim();
                    int id_tipoiibb = 0;
                    string Ingresos_Brutos = txtIngresosBrutosPF.Text.Trim();
                    string Calle = txtCallePF.Text.Trim();
                    int Nro_Puerta = 0;
                    string Piso = txtPisoPF.Text.Trim();
                    string Depto = txtDeptoPF.Text.Trim();
                    int id_Localidad = 0;
                    string Codigo_Postal = txtCPPF.Text.Trim();
                    string TelefonoPF = txtTelefonoPF.Text.Trim();
                    string TelefonoMovil = txtTelefonoMovilPF.Text.Trim();

                    string Email = txtEmailPF.Text.Trim();

                    int.TryParse(ddlTipoDocumentoPF.SelectedValue, out id_tipodoc_personal);
                    int.TryParse(ddlTipoIngresosBrutosPF.SelectedValue, out id_tipoiibb);
                    int.TryParse(txtNroPuertaPF.Text.Trim(), out Nro_Puerta);
                    int.TryParse(ddlLocalidadPF.SelectedValue, out id_Localidad);
                    int.TryParse(hid_id_titular_pf.Value, out id_personafisica);

                    using (TransactionScope Tran = new TransactionScope())
                    {
                        try
                        {
                            if (id_personafisica > 0)
                                db.CPadron_EliminarPersonaFisica(id_cpadron, id_personafisica, this.validar_estado);

                            ObjectParameter param_id_personafisica = new ObjectParameter("id_personafisica", typeof(int));

                            db.CPadron_AgregarTitularesPersonasFisicas(id_cpadron, Apellido, Nombres, id_tipodoc_personal
                                                                , Nro_Documento, Cuit, id_tipoiibb, Ingresos_Brutos, Calle, Nro_Puerta
                                                                , Piso, Depto, id_Localidad, Codigo_Postal, TelefonoPF
                                                                , TelefonoMovil, "", Email, true, userid, param_id_personafisica, this.validar_estado);

                            id_personafisica = Convert.ToInt32(param_id_personafisica.Value);

                            //Firmantes
                            //if (!MismoFirmante)
                            //{
                            //    Apellido = txtApellidoFirPF.Text.Trim();
                            //    Nombres = txtNombresFirPF.Text.Trim();
                            //    Nro_Documento = txtNroDocumentoFirPF.Text.Trim();
                            //    int.TryParse(ddlTipoDocumentoFirPF.SelectedValue, out id_tipodoc_personal);
                            //    int.TryParse(ddlTipoCaracterLegalFirPF.SelectedValue, out TipoCaracterLegalTitular);

                            //}
                            //db.CAA_Firmantes_AgregarPersonasFisicas(id_solicitud, id_personafisica, Apellido, Nombres,
                            //                            id_tipodoc_personal, Nro_Documento, TipoCaracterLegalTitular, string.Empty);

                            CargarDatosTitulares(this.id_cpadron);
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmAgregarPersonaFisica", "$('#frmAgregarPersonaFisica').modal('hide');", true);

                            Tran.Complete();

                        }
                        catch (Exception ex)
                        {
                            Tran.Dispose();
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError.Write(ex, ex.Message);
                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updBotonesAgregarPF, "showfrmError();");
                }
            }

        }
        protected void btnEliminarTitular_Click(object sender, EventArgs e)
        {
            LinkButton btnEliminarTitular = (LinkButton)sender;

            string TipoPersona = btnEliminarTitular.CommandName;
            int id_persona = int.Parse(btnEliminarTitular.CommandArgument);

            hid_tipopersona_eliminar.Value = btnEliminarTitular.CommandName;
            hid_id_persona_eliminar.Value = btnEliminarTitular.CommandArgument;

            if (TipoPersona.Equals("PF"))
            {
                //
            }

            if (TipoPersona.Equals("PJ"))
            {
                //
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmConfirmarEliminar_Titulares", "$('#frmConfirmarEliminar_Titulares').modal();", true);
        }

        protected void btnEditarTitular_Click(object sender, EventArgs e)
        {
            LinkButton btnEditarTitular = (LinkButton)sender;

            string TipoPersona = btnEditarTitular.CommandName;
            int id_persona = int.Parse(btnEditarTitular.CommandArgument);

            if (TipoPersona.Equals("PF"))
            {
                LimpiarControlesABMPF();
                hid_id_titular_pf.Value = id_persona.ToString();
                CargarDatosTitularPF(id_persona);
                updAgregarPersonaFisica.Update();
                //((BasePage)this.Page).EjecutarScript(updGrillaTitulares, "showfrmAgregarPersonaFisica_Titulares();");
                //Functions.EjecutarScript(updGrillaTitulares, "showfrmAgregarPersonaFisica_Titulares();");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmAgregarPersonaFisica", "$('#frmAgregarPersonaFisica').modal();", true);
            }

            if (TipoPersona.Equals("PJ"))
            {
                LimpiarControlesABMPJ();
                hid_id_titular_pj.Value = id_persona.ToString();
                CargarDatosTitularPJ(id_persona);
                updAgregarPersonaJuridica.Update();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmAgregarPersonaJuridica", "$('#frmAgregarPersonaJuridica').modal();", true);
                //Functions.EjecutarScript(updGrillaTitulares, "showfrmAgregarPersonaJuridica_Titulares();");
                //ScriptManager.RegisterStartupScript(updGrillaTitulares, updGrillaTitulares.GetType(), "showfrmAgregarPersonaFisica_Titulares", "showfrmAgregarPersonaFisica_Titulares();", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "showfrmAgregarPersonaFisica_Titulares('this');", true);
                //ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), "showfrmAgregarPersonaFisica_Titulares", "showfrmAgregarPersonaFisica_Titulares();", true);
                //ScriptManager.RegisterClientScriptBlock(updGrillaTitulares, updGrillaTitulares.GetType(), "showfrmAgregarPersonaFisica_Titulares", "showfrmAgregarPersonaFisica_Titulares();", true);
                //ScriptManager.RegisterStartupScript(updGrillaTitulares, updGrillaTitulares.GetType(), "script", "showfrmAgregarPersonaFisica_Titulares();", true);
            }

        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();

            try
            {


                int id_persona = int.Parse(hid_id_persona_eliminar.Value);
                string TipoPersona = hid_tipopersona_eliminar.Value;

                if (TipoPersona.Equals("PF"))
                {
                    db.CPadron_EliminarPersonaFisica(this.id_cpadron, id_persona, this.validar_estado);
                }
                else
                {
                    db.CPadron_EliminarPersonaJuridica(this.id_cpadron, id_persona, this.validar_estado);
                }

                CargarDatosTitulares(this.id_cpadron);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmConfirmarEliminar_Titulares", "$('#frmConfirmarEliminar_Titulares').modal('hide');", true);
                //Functions.EjecutarScript(updConfirmarEliminar, "hideConfirmarEliminar_Titulares();");

            }
            catch (Exception ex)
            {
                LogError.Write(ex, ex.Message);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updConfirmarEliminar, "showfrmError();");
            }
        }

        protected void ddlProvinciaPF_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarLocalidades(ddlProvinciaPF, ddlLocalidadPF);
        }

        //protected void ddlTipoCaracterLegalFirPJ_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DGHP_Entities db = new DGHP_Entities();

        //    int id_tipocaracter = int.Parse(ddlTipoCaracterLegalFirPJ.SelectedValue);

        //    var tc = db.TiposDeCaracterLegal.FirstOrDefault(x => x.id_tipocaracter == id_tipocaracter);
        //    Req_CargoFirPJ.Style["display"] = "none";

        //    if (tc != null && tc.muestracargo_tipocaracter)
        //    {
        //        rowCargoFirmantePJ.Style["display"] = "block";
        //    }
        //    else
        //    {
        //        txtCargoFirPJ.Text = "";
        //        hid_CargosFir_seleccionado.Value = "";
        //        rowCargoFirmantePJ.Style["display"] = "none";
        //    }

        //    db.Dispose();
        //}

        private void ComprobarSolicitud()
        {

            if (Page.RouteData.Values["id"] != null)
            {

                this.id_cpadron = Convert.ToInt32(Page.RouteData.Values["id"].ToString());


                /*Guid userid_solicitud = Functions.GetUserId(id_solicitud);
                Guid userid = Functions.GetUserId();

                if (userid_solicitud != userid)
                    Server.Transfer("~/Errores/Error3002.aspx");
                */


            }
            else
            {
                Server.Transfer("~/Errores/Error3001.aspx");
            }

        }

        public void CargarDatos(int id_cpadron, int validar_estado, bool Editar)
        {
            try
            {

                this.id_cpadron = id_cpadron;
                this.validar_estado = validar_estado;
                this.editar = Editar;

                CargarDatosTitulares(id_cpadron);

                if (!this.editar)
                {
                    box_MostrarTitulares.Visible = true;
                    box_titulares.Visible = false;
                    titulo.Visible = false;
                }
                else
                {
                    box_MostrarTitulares.Visible = false;
                    box_titulares.Visible = true;
                    titulo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatos en Tab_Titulares.aspx");
                //Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_Titulares.aspx"));
                throw ex;
            }


        }

        public void CargarDatosTitulares(int id_cpadron)
        {

            DGHP_Entities db = new DGHP_Entities();

            var lstTitulares = (from pf in db.CPadron_Titulares_PersonasFisicas
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
                                    (from pj in db.CPadron_Titulares_PersonasJuridicas
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


            grdTitularesHab.DataSource = lstTitulares;
            grdTitularesHab.DataBind();

            grdTitulares.DataSource = lstTitulares;
            grdTitulares.DataBind();

            updGrillaTitulares.Update();
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
                                      select l).OrderBy(x => x.Depto).ToList();


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

        public void EjecutarScript(UpdatePanel objupd, string script)
        {
            ScriptManager.RegisterClientScriptBlock(objupd, objupd.GetType(), "script", script, true);
        }

        protected IEnumerable<TiposDeIngresosBrutos> TraerTiposDeIngresosBrutos(int id_tipoiibb)
        {
            DGHP_Entities db = new DGHP_Entities();
            var result = db.TiposDeIngresosBrutos.Where(x => x.id_tipoiibb == id_tipoiibb);
            return result;
        }

        //protected void btnEliminarTitularSH_Click(object sender, EventArgs e)
        //{

        //    LinkButton btn = (LinkButton)sender;
        //    GridViewRow row = (GridViewRow)btn.Parent.Parent;

        //    DataTable dtTitularesSH = dtTitularesSHCargados();
        //    DataTable dtFirmantesSH = dtFirmantesSHCargados();

        //    Guid rowid_titular = (Guid)dtTitularesSH.Rows[row.RowIndex]["rowid"];

        //    dtTitularesSH.Rows.Remove(dtTitularesSH.Rows[row.RowIndex]);

        //    foreach (DataRow rowFirmante in dtFirmantesSH.Rows)
        //    {
        //        if ((Guid)rowFirmante["rowid_titular"] == rowid_titular)
        //        {
        //            dtFirmantesSH.Rows.Remove(rowFirmante);
        //            break;
        //        }
        //    }

        //    grdTitularesSH.DataSource = dtTitularesSH;
        //    grdTitularesSH.DataBind();

        //    grdFirmantesSH.DataSource = dtFirmantesSH;
        //    grdFirmantesSH.DataBind();

        //    updgrillaTitularesSH.Update();

        //}
        //protected void ddlTipoCaracterLegalFirSH_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    int id_tipocaracter = 0;

        //    int.TryParse(ddlTipoCaracterLegalFirSH.SelectedValue, out id_tipocaracter);

        //    if (id_tipocaracter > 0)
        //    {
        //        DGHP_Entities db = new DGHP_Entities();

        //        bool muestracargo_tipocaracter = db.TiposDeCaracterLegal.FirstOrDefault(x => x.id_tipocaracter == id_tipocaracter).muestracargo_tipocaracter;

        //        if (muestracargo_tipocaracter)
        //            pnlCargoFirmanteSH.Style["display"] = "block";
        //        else
        //        {
        //            pnlCargoFirmanteSH.Style["display"] = "none";
        //            txtCargoFirSH.Text = "";
        //        }

        //        db.Dispose();
        //    }
        //    else
        //    {
        //        pnlCargoFirmanteSH.Style["display"] = "none";
        //        txtCargoFirSH.Text = "";
        //    }

        //}

        //protected void btnAceptarTitSH_Click(object sender, EventArgs e)
        //{

        //    DataTable dtTitularesSH = dtTitularesSHCargados();
        //    DataTable dtFirmantesSH = dtFirmantesSHCargados();
        //    string firmanteDe = txtApellidosTitSH.Text.Trim() + " " + txtNombresTitSH.Text.Trim();

        //    if (hid_rowindex_titSH.Value.Length == 0)
        //    {
        //        Guid rowid_titular = Guid.NewGuid();

        //        dtTitularesSH.Rows.Add(rowid_titular, txtApellidosTitSH.Text.Trim(), txtNombresTitSH.Text.Trim(), ddlTipoDocumentoTitSH.SelectedItem.Text.Trim(), txtNroDocumentoTitSH.Text.Trim(),
        //                 int.Parse(ddlTipoDocumentoTitSH.SelectedValue), txtEmailTitSH.Text.Trim());


        //        if (optMismaPersonaSH.Checked)
        //        {

        //            dtFirmantesSH.Rows.Add(firmanteDe, txtApellidosTitSH.Text.Trim(), txtNombresTitSH.Text.Trim(), ddlTipoDocumentoTitSH.SelectedItem.Text.Trim(), txtNroDocumentoTitSH.Text.Trim(),
        //                        "Titular", int.Parse(ddlTipoDocumentoTitSH.SelectedValue), 1, string.Empty, txtEmailTitSH.Text.Trim(), Guid.NewGuid(), rowid_titular, optMismaPersonaSH.Checked);
        //        }
        //        else
        //        {
        //            dtFirmantesSH.Rows.Add(firmanteDe, txtApellidosFirSH.Text.Trim(), txtNombresFirSH.Text.Trim(), ddlTipoDocumentoFirSH.SelectedItem.Text.Trim(), txtNroDocumentoFirSH.Text.Trim(),
        //                        ddlTipoCaracterLegalFirSH.SelectedItem.Text, int.Parse(ddlTipoDocumentoFirSH.SelectedValue),
        //                        int.Parse(ddlTipoCaracterLegalFirSH.SelectedValue), txtCargoFirSH.Text.Trim(), txtEmailFirSH.Text.Trim(), Guid.NewGuid(), rowid_titular, optMismaPersonaSH.Checked);
        //        }

        //    }
        //    else
        //    {
        //        int rowindex = int.Parse(hid_rowindex_titSH.Value);
        //        DataRow drTitularesSH = dtTitularesSH.Rows[rowindex];
        //        DataRow drFirmantesSH = null;

        //        foreach (DataRow rowFirmante in dtFirmantesSH.Rows)
        //        {
        //            if ((Guid)rowFirmante["rowid_titular"] == (Guid)drTitularesSH["rowid"])
        //            {
        //                drFirmantesSH = rowFirmante;
        //                break;
        //            }

        //        }

        //        dtTitularesSH.Rows[rowindex]["Apellidos"] = txtApellidosTitSH.Text.Trim();
        //        dtTitularesSH.Rows[rowindex]["Nombres"] = txtNombresTitSH.Text.Trim();
        //        dtTitularesSH.Rows[rowindex]["TipoDoc"] = ddlTipoDocumentoTitSH.SelectedItem.Text;
        //        dtTitularesSH.Rows[rowindex]["NroDoc"] = txtNroDocumentoTitSH.Text.Trim();
        //        dtTitularesSH.Rows[rowindex]["id_tipodoc_personal"] = int.Parse(ddlTipoDocumentoTitSH.SelectedValue);
        //        dtTitularesSH.Rows[rowindex]["email"] = txtEmailTitSH.Text.Trim();



        //        if (drFirmantesSH != null && optOtraPersonaSH.Checked)
        //        {
        //            drFirmantesSH["Apellidos"] = txtApellidosFirSH.Text.Trim();
        //            drFirmantesSH["Nombres"] = txtNombresFirSH.Text.Trim();
        //            drFirmantesSH["NroDoc"] = txtNroDocumentoFirSH.Text.Trim();
        //            drFirmantesSH["id_tipodoc_personal"] = ddlTipoDocumentoFirSH.SelectedValue;
        //            drFirmantesSH["email"] = txtEmailFirSH.Text.Trim();
        //            drFirmantesSH["nom_tipocaracter"] = ddlTipoCaracterLegalFirSH.SelectedItem.Text;
        //            drFirmantesSH["id_tipocaracter"] = ddlTipoCaracterLegalFirSH.SelectedValue;
        //            drFirmantesSH["cargo_firmante"] = txtCargoFirSH.Text.Trim();
        //            drFirmantesSH["misma_persona"] = false;
        //        }
        //        else
        //        {
        //            drFirmantesSH["Apellidos"] = txtApellidosTitSH.Text.Trim();
        //            drFirmantesSH["Nombres"] = txtNombresTitSH.Text.Trim();
        //            drFirmantesSH["NroDoc"] = txtNroDocumentoTitSH.Text.Trim();
        //            drFirmantesSH["id_tipodoc_personal"] = ddlTipoDocumentoTitSH.SelectedValue;
        //            drFirmantesSH["email"] = txtEmailTitSH.Text.Trim();
        //            drFirmantesSH["nom_tipocaracter"] = "Titular";
        //            drFirmantesSH["id_tipocaracter"] = "1";
        //            drFirmantesSH["cargo_firmante"] = string.Empty;
        //            drFirmantesSH["misma_persona"] = true;
        //        }
        //    }

        //    grdTitularesSH.DataSource = dtTitularesSH;
        //    grdTitularesSH.DataBind();

        //    grdFirmantesSH.DataSource = dtFirmantesSH;
        //    grdFirmantesSH.DataBind();

        //    updgrillaTitularesSH.Update();
        //    Functions.EjecutarScript(updBotonesIngresarTitularesSH, "hidefrmAgregarTitularesSH_Titulares();");
        //}


        //protected void btnEliminarFirmantePJ_Click(object sender, EventArgs e)
        //{
        //    int rowindex = int.Parse(hid_rowindex_eliminar.Value);
        //    DataTable dt = dtFirmantesCargados();

        //    dt.Rows.Remove(dt.Rows[rowindex]);
        //    int i = 0;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        dr["rowindex"] = i;
        //        i++;
        //    }

        //    grdFirmantesPJ.DataSource = dt;
        //    grdFirmantesPJ.DataBind();
        //    updgrdFirmantesPJ.Update();

        //    Functions.EjecutarScript(updConfirmarEliminarFirPJ, "hidefrmConfirmarEliminarFirPJ_Titulares();");
        //}

        //protected void btnEditarFirPJ_Click(object sender, EventArgs e)
        //{

        //    LimpiarControlesFirPJ();

        //    LinkButton btn = (LinkButton)sender;
        //    GridViewRow row = (GridViewRow)btn.Parent.Parent;
        //    DataTable dt = dtFirmantesCargados();

        //    DataRow dr = dt.Rows[row.RowIndex];

        //    txtApellidosFirPJ.Text = dr["Apellidos"].ToString();
        //    txtNombresFirPJ.Text = dr["Nombres"].ToString();
        //    txtNroDocumentoFirPJ.Text = dr["NroDoc"].ToString();
        //    txtEmailFirPJ.Text = dr["Email"].ToString();
        //    ddlTipoDocumentoFirPJ.SelectedValue = dr["id_tipodoc_personal"].ToString();
        //    ddlTipoCaracterLegalFirPJ.SelectedValue = dr["id_tipocaracter"].ToString();

        //    txtCargoFirPJ.Text = dr["cargo_firmante_pj"].ToString();

        //    if (txtCargoFirPJ.Text.Length > 0)
        //    {
        //        rowCargoFirmantePJ.Style["display"] = "block";
        //    }
        //    else
        //    {
        //        rowCargoFirmantePJ.Style["display"] = "none";
        //    }

        //    hid_rowindex_fir.Value = row.RowIndex.ToString();
        //    updFirmantePJ.Update();
        //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmAgregarFirmantePJ_Titulares", "$('#frmAgregarFirmantePJ_Titulares').modal();", true);

        //}

        //protected void btnShowAgregarFirPJ_Click(object sender, EventArgs e)
        //{
        //    LimpiarControlesFirPJ();
        //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmAgregarFirmantePJ_Titulares", "$('#frmAgregarFirmantePJ_Titulares').modal();", true);
        //}

        //private void LimpiarControlesFirPJ()
        //{
        //    hid_CargosFir_seleccionado.Value = "";
        //    hid_rowindex_fir.Value = "";
        //    txtApellidosFirPJ.Text = "";
        //    txtNombresFirPJ.Text = "";
        //    ddlTipoDocumentoFirPJ.ClearSelection();
        //    txtNroDocumentoFirPJ.Text = "";
        //    txtEmailFirPJ.Text = "";
        //    ddlTipoCaracterLegalFirPJ.ClearSelection();
        //    txtCargoFirPJ.Text = "";
        //    rowCargoFirmantePJ.Style["display"] = "none";
        //    Req_CargoFirPJ.Style["display"] = "none";
        //    ValExiste_TipoNroDocFirPJ.Style["display"] = "none";
        //    updFirmantePJ.Update();
        //}

    }
}