using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Ajax.Utilities;
using SGI.Model;
using SGI.Seguridad;
using SGI.Webservices.SADE.bloqueo_expediente;
using Syncfusion.DocIO.DLS;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataVisualization.DiagramEnums;
using Syncfusion.JavaScript.Web;
using Syncfusion.Linq;
using Syncfusion.Pdf.Lists;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Providers.Entities;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ws_ExpedienteElectronico;
using static SGI.Model.Engine;
using static Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAlrunsRecord;

namespace SGI.Operaciones
{
    public partial class SadeForm : System.Web.UI.Page
    {
        #region Parametros
        public int id_grupo_tramite
        {
            get
            {
                int ret = (ViewState["_grupo_tramite"] != null ? Convert.ToInt32(ViewState["_grupo_tramite"]) : 0);
                return ret;
            }
            set
            {
                ViewState["_grupo_tramite"] = value;
            }

        }
        private string _sistema_SADE;
        private string sistema_SADE
        {
            get
            {
                if (string.IsNullOrEmpty(_sistema_SADE))
                {
                    _sistema_SADE = "SGI";
                }
                return _sistema_SADE;
            }
        }
        private string _url_servicio_EE;
        private string url_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_url_servicio_EE))
                {
                    _url_servicio_EE = Parametros.GetParam_ValorChar("SGI.Url.Service.ExpedienteElectronico");
                }
                return _url_servicio_EE;
            }
        }
        private string _username_servicio_EE;
        private string username_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_username_servicio_EE))
                {
                    _username_servicio_EE = Parametros.GetParam_ValorChar("SGI.UserName.Service.ExpedienteElectronico");
                }
                return _username_servicio_EE;
            }
        }
        private string _pass_servicio_EE;
        private string pass_servicio_EE
        {
            get
            {
                if (string.IsNullOrEmpty(_pass_servicio_EE))
                {
                    _pass_servicio_EE = Parametros.GetParam_ValorChar("SGI.Pwd.Service.ExpedienteElectronico");
                }
                return _pass_servicio_EE;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion
            if (!IsPostBack)
            {
                List<string> initialData = new List<string>();
                initialData.Add("");
                txtBoxRepeater.DataSource = initialData;
                txtBoxRepeater.DataBind();
                String last_id = String.Empty;
                if (Session["LastID"] != null)
                {
                    txtBuscarSolicitud.Text = Session["LastID"].ToString();
                    Session["LastID"] = null;
                    btnBuscarSolicitud_Click(sender, e);
                }
                viewDropDownList.Visible = false;
                viewValorExpediente.Visible = true;
                myAccordion.Visible = true;
                lblResult.Text = string.Empty;
            }
        }

        protected void btnBuscarSolicitud_Click(object sender, EventArgs e)
        {
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out int idSolicitud);
            if (couldParse)
            {
                DGHP_Entities entities = new DGHP_Entities();
                int tipotramite = (from solic in entities.SSIT_Solicitudes
                                   where solic.id_solicitud == idSolicitud
                                   select solic.id_tipotramite).Union(from trans in entities.Transf_Solicitudes
                                                                      where trans.id_solicitud == idSolicitud
                                                                      select trans.id_tipotramite).FirstOrDefault();
                if (tipotramite == (int)Constants.TipoDeTramite.Transferencia)
                {
                    this.id_grupo_tramite = (int)Constants.GruposDeTramite.TR;
                }
                else
                {
                    //aca estamos ignorando cpadron
                    this.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;
                }
            }
            viewDropDownList.Visible = true;
            viewValorExpediente.Visible = true;
            myAccordion.Visible = true;
            fillInfoSade(idSolicitud, couldParse);
        }

        protected void AddTextBoxButton_Click(object sender, EventArgs e)
        {
            List<string> data = new List<string>();
            foreach (RepeaterItem item in txtBoxRepeater.Items)
            {
                TextBox textBox = (TextBox)item.FindControl("txtBoxSolicitud");
                data.Add(textBox.Text);
            }
            data.Add("");
            txtBoxRepeater.DataSource = data;
            txtBoxRepeater.DataBind();
        }

        private bool ValidarFilasGrilla()
        {
            bool ret = false;
            if (txtBoxRepeater.Items.Count < 1)
            {
                throw new Exception("No hay solicitudes para procesar.");
            }
                

            for (int i = 0; i < txtBoxRepeater.Items.Count - 1; i++)
            {
                TextBox txtBoxSolicitud = (TextBox)txtBoxRepeater.Items[i].FindControl("txtBoxSolicitud");
                int id_solicitud = 0;

                if (!int.TryParse(txtBoxSolicitud.Text, out id_solicitud))
                    throw new Exception(string.Format("Fila {0}: La solicitud no posee un valor correcto.", i + 1));

                if (id_solicitud <= 0)
                    throw new Exception(string.Format("Fila {0}: La solicitud debe ser mayor a 0.", i + 1));

            }

            ret = true;
            return ret;
        }

        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out int id_solicitud);
            bool ErroresEnSolicitud = false;
            if (couldParse)
            {
                int id_paquete = Convert.ToInt32(hid_paquete.Value);
                if(id_paquete > 0)
                {
                    ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                    serviceEE.Url = this.url_servicio_EE;
                    try
                    {
                        var res = serviceEE.EjecutarPaquete(this.username_servicio_EE, this.pass_servicio_EE, id_paquete);
                    }
                    catch (Exception ex)
                    {
                        lblResult.Text = $"Paquete {id_paquete} -> {ex.Message}.";
                        ErroresEnSolicitud = true;
                    }
                    finally
                    {
                        serviceEE.Dispose();
                    }
                    if (!ErroresEnSolicitud)
                        lblResult.Text = $"Paquete {id_paquete} -> Paquete Ejecutado exitosamente.";
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblResult.Text = string.Empty;
            txtExpedienteElectronicoValor.Text = string.Empty;
            hid_ExpedienteElectronico.Value = string.Empty;
            txtEstadoValor.Text = string.Empty;
            txtUsuarioCaratuladorValor.Text = string.Empty;
            txtUsuarioValor.Text = string.Empty;
            txtReparticionValor.Text = string.Empty;
            txtSectorValor.Text = string.Empty;
            txtBloqueado.Text = string.Empty;

            ddlUsuario.DataSource = null;
            ddlUsuario.DataTextField = "UserName_SADE";
            ddlUsuario.DataValueField = "userid";
            ddlUsuario.DataBind();
        }

        protected void btnPasear_Click(object sender, EventArgs e)
        {
            string expedienteElectronico = hid_ExpedienteElectronico.Value;
            string ExpedienteE = string.Empty;
            string selectedOption = rdoUser.Checked ? rdoUser.Text : rdoGroup.Text;
            bool EsPaseSector = rdoGroup.Checked;
            bool ConDesbloqueo = chkboxDesbloqueo.Checked;
            bool ErroresEnSolicitud = false;
            string textBoxSectorValue = textBoxDestino.Text;
            string textBoxReparticionDestinoValue = textBoxReparticionDestino.Text;
            string textBoxUsuarioDestinoValue = textBoxUsuarioDestino.Text;
            string textBoxEstadoSadeValue = textBoxEstadoSade.Text;

            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
            serviceEE.Url = this.url_servicio_EE;
            int id_paquete = Convert.ToInt32(hid_paquete.Value);
            string motivoDesc = $"Pase a: {selectedOption}, Sector: {textBoxSectorValue}, Reparticion: {textBoxReparticionDestinoValue}";
            string usuario_sade = ddlUsuario.SelectedValue;
            int id_pase = 0;

            Guid userid_tarea;
            bool parseado = Guid.TryParse(usuario_sade, out userid_tarea);
            if (!parseado)
                throw new Exception("Usuario SADE invalido");
            string Usuario_SADE = Functions.GetUsernameSADE(userid_tarea);

            lblResult.Text = motivoDesc;
            if(id_paquete > 0 && !string.IsNullOrEmpty(expedienteElectronico))
            {
                try
                {
                    if (EsPaseSector)
                    {
                        if (string.IsNullOrEmpty(textBoxEstadoSadeValue))
                            id_pase = serviceEE.PasarExpediente_aGrupo(this.username_servicio_EE, this.pass_servicio_EE,
                                id_paquete, "Pase al sector " + textBoxSectorValue, Usuario_SADE, textBoxSectorValue, textBoxReparticionDestinoValue);
                        else if (textBoxEstadoSadeValue == "Guarda Temporal")
                            id_pase = serviceEE.PasarExpediente_aGuardaTemporal(this.username_servicio_EE, this.pass_servicio_EE,
                                id_paquete, "Pase a Guarda Temporal", Usuario_SADE, textBoxEstadoSadeValue);                  
                        else
                            id_pase = serviceEE.PasarExpediente_aGrupo_v2(this.username_servicio_EE, this.pass_servicio_EE,
                                id_paquete, "Pase al sector " + textBoxSectorValue, Usuario_SADE, textBoxSectorValue,
                                textBoxReparticionDestinoValue, textBoxEstadoSadeValue, ConDesbloqueo);
                    }
                    else
                    {
                        id_pase = serviceEE.PasarExpediente_aUsuario(this.username_servicio_EE, this.pass_servicio_EE,
                            id_paquete, "Pase al usuario " + textBoxUsuarioDestinoValue, Usuario_SADE,
                            textBoxUsuarioDestinoValue);
                    }
                    lblResult.Text = id_pase.ToString();
                }
                catch (Exception ex)
                {
                    lblResult.Text = $"Paquete {id_paquete} -> {ex.Message}.";
                    ErroresEnSolicitud = true;
                }
                finally
                {
                    serviceEE.Dispose();
                }
                if (!ErroresEnSolicitud)
                    lblResult.Text = $"Paquete {id_paquete} -> Paquete Ejecutado exitosamente.";
            }
            else
            {
                throw new Exception(string.Format("La solicitud no tiene paquete o expediente"));
            }
            

        }


        private void fillInfoSade(int idSolicitud, bool couldParse)
        {
            string ExpedienteE = string.Empty;
            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
            ws_ExpedienteElectronico.consultaExpedienteResponseDetallado ExpedienteElectronico;

            serviceEE.Url = this.url_servicio_EE;
            if (!couldParse)
            {
                Exception exS = new Exception("La solicitud ingresada no se pudo convertir en un entero");
                LogError.Write(exS);
                throw exS;
            }
            using (var ctx = new DGHP_Entities())
            {
                using (var tran = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        int id_paquete = 0;
                        if (this.id_grupo_tramite == (int)Constants.GruposDeTramite.HAB)
                        {
                            SSIT_Solicitudes Solicitud =
                            (from solicitudes in ctx.SSIT_Solicitudes
                             where solicitudes.id_solicitud == idSolicitud
                             select solicitudes).FirstOrDefault();
                            id_paquete = Functions.GetPaqueteFromSolicitud(idSolicitud);
                            hid_paquete.Value = Convert.ToString(id_paquete);
                            if (Solicitud.NroExpedienteSade.IsNullOrWhiteSpace())
                            {
                                if (id_paquete > 0)
                                    ExpedienteE = serviceEE.GetExpedienteByPaquete(this.username_servicio_EE, this.pass_servicio_EE, id_paquete);
                                else
                                {
                                    //Si no tiene id_paquete entonces me salgo pues obviamente no tiene EE
                                    return;
                                }
                            }
                            else
                                ExpedienteE = Solicitud.NroExpedienteSade;
                            //ExpedienteE = "EX-2024-00022788- -GCABA-AGC"; //para testing
                        }
                        else
                        {
                            Transf_Solicitudes transferencia =
                            (from transferencias in ctx.Transf_Solicitudes
                             where transferencias.id_solicitud == idSolicitud
                             select transferencias).FirstOrDefault();
                            id_paquete = Functions.GetPaqueteFromSolicitud(idSolicitud);
                            hid_paquete.Value = Convert.ToString(id_paquete);
                            if (transferencia.NroExpedienteSade.IsNullOrWhiteSpace())
                            {
                                if (id_paquete > 0)
                                    ExpedienteE = serviceEE.GetExpedienteByPaquete(this.username_servicio_EE, this.pass_servicio_EE, id_paquete);
                                else
                                {
                                    //Si no tiene id_paquete entonces me salgo pues obviamente no tiene EE
                                    return;
                                }
                            }
                            else
                                ExpedienteE = transferencia.NroExpedienteSade;
                        }


                        //Request a pasarela con el EE
                        txtExpedienteElectronicoValor.Text = "";
                        txtEstadoValor.Text = "";
                        txtUsuarioCaratuladorValor.Text = "";
                        txtUsuarioValor.Text = "";
                        txtReparticionValor.Text = "";
                        txtSectorValor.Text = "";
                        ExpedienteElectronico = serviceEE.consultarExpedienteDetallado(this.username_servicio_EE, this.pass_servicio_EE, ExpedienteE);
                        IBloqueoExpedienteService servicio_bloqueo = new IBloqueoExpedienteService();
                        bool isBlocked = servicio_bloqueo.isBloqueado(ExpedienteElectronico.codigoEE);
                        if (ExpedienteElectronico != null)
                        {
                            txtExpedienteElectronicoValor.Text = ExpedienteElectronico.codigoEE;
                            hid_ExpedienteElectronico.Value = ExpedienteElectronico.codigoEE;
                            txtEstadoValor.Text = ExpedienteElectronico.estado;
                            txtUsuarioCaratuladorValor.Text = ExpedienteElectronico.usuarioCaratulador;
                            txtUsuarioValor.Text = ExpedienteElectronico.usuarioDestino;
                            txtReparticionValor.Text = ExpedienteElectronico.reparticionDestino;
                            txtSectorValor.Text = ExpedienteElectronico.sectorDestino;
                            txtBloqueado.Text = (isBlocked ? "Bloqueado" : "Desbloqueado");
                        }
                        else
                        {
                            myAccordion.Visible = false;
                        }


                        loadUsersFromSector(ExpedienteElectronico);
                    }
                    catch
                    (Exception ex)
                    {
                        LogError.Write(ex);
                        //throw (ex);
                    }
                }
            }
        }

        private void loadUsersFromSector(ws_ExpedienteElectronico.consultaExpedienteResponseDetallado ExpedienteElectronico)
        {
            if (ExpedienteElectronico != null)
            {
                if (!ExpedienteElectronico.usuarioDestino.IsNullOrWhiteSpace())
                {
                    DGHP_Entities db = new DGHP_Entities();
                    var usuarios = (from pro in db.SGI_Profiles
                                    join mem in db.aspnet_Membership on pro.userid equals mem.UserId
                                    where pro.UserName_SADE.ToUpper() == ExpedienteElectronico.usuarioDestino.Trim().ToUpper()
                                     && !mem.IsLockedOut && pro.UserName_SADE.Length > 0
                                    select pro).ToList();
                    var distinctUsuarios = usuarios
                            .GroupBy(pro => pro.UserName_SADE)
                            .Select(g => g.First())
                            .ToList();
                    ddlUsuario.DataSource = distinctUsuarios;
                    ddlUsuario.DataTextField = "UserName_SADE";
                    ddlUsuario.DataValueField = "userid";
                    ddlUsuario.DataBind();
                }
                else
                if (!ExpedienteElectronico.sectorDestino.IsNullOrWhiteSpace())
                {
                    DGHP_Entities db = new DGHP_Entities();
                    var usuarios = (from pro in db.SGI_Profiles
                                    join mem in db.aspnet_Membership on pro.userid equals mem.UserId
                                    where pro.Sector_SADE.ToUpper() == ExpedienteElectronico.sectorDestino.Trim().ToUpper()
                                     && !mem.IsLockedOut && pro.UserName_SADE.Length > 0
                                    select pro).ToList();
                    var distinctUsuarios = usuarios
                            .GroupBy(pro => pro.UserName_SADE)
                            .Select(g => g.First())
                            .ToList();
                    if (distinctUsuarios == null || distinctUsuarios.Count() == 0)
                    {
                        Exception solEx = new Exception($"Expediente {ExpedienteElectronico.codigoEE}, No se encontraron usuarios del sector {ExpedienteElectronico.sectorDestino} en la base de datos. Error.");
                        LogError.Write(solEx);
                        throw solEx;
                    }
                    else
                    {
                        ddlUsuario.DataSource = distinctUsuarios;
                        ddlUsuario.DataTextField = "UserName_SADE";
                        ddlUsuario.DataValueField = "userid";
                        ddlUsuario.DataBind();
                    }
                    db.Dispose();
                }
            }
        }

        protected void btnBloquear_Click(object sender, EventArgs e)
        {
            string expedienteElectronico = hid_ExpedienteElectronico.Value;
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out int id_solicitud);
            bool ErroresEnSolicitud = false;
            if (!string.IsNullOrEmpty(expedienteElectronico))
            {
                try
                {
                    IBloqueoExpedienteService servicio_bloqueo = new IBloqueoExpedienteService();
                    //servicio_bloqueo.Url = this.url_servicio_EE;

                    if (!servicio_bloqueo.isBloqueado(expedienteElectronico))
                    {
                        servicio_bloqueo.bloquearExpediente("SGI", expedienteElectronico);
                    }
                    bool isBlocked = servicio_bloqueo.isBloqueado(expedienteElectronico);
                    if (isBlocked)
                    {
                        lblResult.Text = $"El Expediente Electronico {expedienteElectronico} fue Bloqueado";
                        txtBloqueado.Text = "Bloqueado";
                    }
                    else
                    {
                        lblResult.Text = $"El Expediente Electronico {expedienteElectronico} fue Desbloqueado";
                        txtBloqueado.Text = "Desbloqueado";
                    }
                    servicio_bloqueo.Dispose();
                    
                }
                catch (Exception ex)
                {
                    lblError.Text = $"Solicitud {id_solicitud} -> No se pudo bloquear el expediente. Error SADE: {ex.Message} ";
                    ErroresEnSolicitud = true;

                }
            }
        }

        protected void btnDesbloquear_Click(object sender, EventArgs e)
        {
            string expedienteElectronico = hid_ExpedienteElectronico.Value;
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out int id_solicitud);
            bool ErroresEnSolicitud = false;
            if (!string.IsNullOrEmpty(expedienteElectronico))
            {
                try
                {
                    //ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                    //serviceEE.Url = this.url_servicio_EE;
                    //serviceEE.Desbloquear_Expediente();
                    IBloqueoExpedienteService servicio_bloqueo = new IBloqueoExpedienteService();
                    //servicio_bloqueo.Url = this.url_servicio_EE;
                    
                    if (servicio_bloqueo.isBloqueado(expedienteElectronico))
                    {
                        servicio_bloqueo.desbloquearExpediente("SGI", expedienteElectronico);
                    }
                    bool isBlocked = servicio_bloqueo.isBloqueado(expedienteElectronico);
                    if (isBlocked)
                    {
                        lblResult.Text = $"El Expediente Electronico {expedienteElectronico} fue Bloqueado";
                        txtBloqueado.Text = "Bloqueado";
                    }
                    else
                    {
                        lblResult.Text = $"El Expediente Electronico {expedienteElectronico} fue Desbloqueado";
                        txtBloqueado.Text = "Desbloqueado";
                    }
                    servicio_bloqueo.Dispose();
                    

                }
                catch (Exception ex)
                {
                    lblError.Text = $"Solicitud {id_solicitud} -> No se pudo desbloquear el expediente. Error SADE: {ex.Message} ";
                    ErroresEnSolicitud = true;

                }
            }
        }
    }
}