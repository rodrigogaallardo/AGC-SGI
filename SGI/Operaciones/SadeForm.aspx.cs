using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Ajax.Utilities;
using SGI.Model;
using SGI.Seguridad;
using SGI.Webservices.SADE.bloqueo_expediente;
using Syncfusion.DocIO.DLS;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataVisualization.DiagramEnums;
using Syncfusion.Linq;
using Syncfusion.Pdf.Lists;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Migrations;
using System.Linq;
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
            }
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

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnDesbloquear_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            EE_Entities dbEE = new EE_Entities();
            AGC_FilesEntities dbFiles = new AGC_FilesEntities();

            try
            {

                if (textBoxDestino.Text.Length == 0)
                    throw new Exception("Debe indicar el nombre de usuario o grupo a donde se enviará el expediente.");


                if (ValidarFilasGrilla())
                {

                    btnProcesar.Enabled = false;
                    //url_servicio = Parametros.GetParam_ValorChar("SGI.Url.Service.ExpedienteElectronico");
                    this.url_servicio = "http://www.dghpsh.agcontrol.gob.ar/test/webservices.agcontrol.gob.ar/ws_ExpedienteElectronico.asmx";
                    //username_servicio = Parametros.GetParam_ValorChar("SGI.UserName.Service.ExpedienteElectronico");
                    //pass_servicio = Parametros.GetParam_ValorChar("SGI.Pwd.Service.ExpedienteElectronico");
                    string reparticion_destino = textBoxReparticionDestino.Text.Trim();

                    var paramSADEHost = dbEE.Parametros.FirstOrDefault(x => x.cod_param == "SADE.Host");

                    if (paramSADEHost == null)
                        throw new Exception("No se encontró el parámetro SADE.Host en la base de Expediente Electrónico.");

                    if (reparticion_destino.Length <= 0)
                        throw new Exception("La repartición de destino no puede estar vacía.");

                    // Recorre todas las solicitudes del repetidor de txtbox
                    // --------------------------------------------
                    for (int i = 0; i < txtBoxRepeater.Items.Count; i++)
                    {
                        TextBox textBox = (TextBox)txtBoxRepeater.Items[i].FindControl("txtBoxSolicitud");
                        int id_solicitud = int.Parse(textBox.Text);

                        bool ErroresEnSolicitud = false;
                        string NroExpediente = "";
                        int id_tramitetarea = 0;
                        int id_paquete = 0;
                        int id_caratula = 0;

                        // Obtiene el id_tramitetarea de la tarea Generar Expediente
                        // --------------------------------------------------------
                        var tramite_tarea = (from tt in db.SGI_Tramites_Tareas
                                             join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                             join tt_proc in db.SGI_Tarea_Generar_Expediente_Procesos on tt.id_tramitetarea equals tt_proc.id_tramitetarea
                                             where tt_hab.id_solicitud == id_solicitud
                                             select new
                                             {
                                                 tt.id_tramitetarea,
                                                 tt_proc.id_paquete,
                                                 id_caratula = tt_proc.id_caratula
                                             }).Union(
                                             from tt in db.SGI_Tramites_Tareas
                                             join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                             join tt_proc in db.SGI_SADE_Procesos on tt.id_tramitetarea equals tt_proc.id_tramitetarea
                                             where tt_hab.id_solicitud == id_solicitud && tt_proc.id_proceso == (int)Constants.EE_Procesos.GeneracionCaratula
                                                && tt_proc.id_devolucion_ee.HasValue
                                             select new
                                             {
                                                 tt.id_tramitetarea,
                                                 tt_proc.id_paquete,
                                                 id_caratula = tt_proc.id_devolucion_ee.Value
                                             }).FirstOrDefault();

                        if (tramite_tarea != null)
                        {
                            id_tramitetarea = tramite_tarea.id_tramitetarea;
                            id_paquete = tramite_tarea.id_paquete;
                            id_caratula = tramite_tarea.id_caratula;
                        }
                        else
                        {
                            lblResult.Text += Environment.NewLine + string.Format("Solicitud {0} -> No existe la tarea de Generación del Expediente.", id_solicitud);
                            ErroresEnSolicitud = true;
                        }

                        ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
                        serviceEE.Url = url_servicio;
                        serviceEE.Url = "http://www.dghpsh.agcontrol.gob.ar/test/webservices.agcontrol.gob.ar/ws_ExpedienteElectronico.asmx";

                        // -------------------------------------------------------
                        // Obtiene los datos de la carátula (Número de Expediente)
                        // -------------------------------------------------------
                        if (!ErroresEnSolicitud)
                        {
                            try
                            {

                                dsInfoPaquete InfoPaquete = serviceEE.Get_Info_Paquete(username_servicio, pass_servicio, id_paquete);
                                foreach (dsInfoPaquete.CaratulaRow row in InfoPaquete.Caratula.Rows)
                                {
                                    if (string.IsNullOrEmpty(row.resultado))
                                    {
                                        lblResult.Text += string.Format("Solicitud {0} -> {1}.", id_solicitud, "No posee caratula generada en SADE.");
                                        ErroresEnSolicitud = true;
                                    }
                                    else
                                    {
                                        NroExpediente = row.resultado;
                                        lblResult.Text += Environment.NewLine + string.Format("NroExpediente: {0}", NroExpediente);
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                lblResult.Text += Environment.NewLine + string.Format("Solicitud {0} -> {1}.", id_solicitud, ex.Message);
                                ErroresEnSolicitud = true;
                            }

                            serviceEE.Dispose();
                        }
                        if (!ErroresEnSolicitud)
                        {
                            try
                            {
                                IBloqueoExpedienteService servicio_bloqueo = new IBloqueoExpedienteService();
                                servicio_bloqueo.Url = paramSADEHost.valorchar_param + "/EEServices/bloqueo-expediente";

                                if (servicio_bloqueo.isBloqueado(NroExpediente))
                                {
                                    servicio_bloqueo.desbloquearExpediente("SGI", NroExpediente);
                                }
                                servicio_bloqueo.Dispose();

                            }
                            catch (Exception ex)
                            {
                                lblResult.Text += Environment.NewLine + string.Format("Solicitud {0} -> No se pudo desbloquear el expediente. Error SADE: {1} ", id_solicitud, ex.Message);
                                ErroresEnSolicitud = true;

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                lblError.Text = ex.Message + " " + ex.StackTrace;
                lblError.Visible = true;

            }
            finally
            {
                db.Dispose();
                dbEE.Dispose();
                dbFiles.Dispose();
            }

        }

        protected void btnBloquear_Click(object sender, EventArgs e)
        {

        }
        protected void btnConsultar_Bloqueo_Click(object sender, EventArgs e)
        {

        }
        protected void btnConsultar_EE_Click(object sender, EventArgs e)
        {

        }
    }
}