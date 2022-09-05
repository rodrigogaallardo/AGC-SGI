using SGI.Model;
using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public partial class EximirCAA : BasePage
    {
        DGHP_Entities db = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);

            }
        }

        #region Entity
        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }
        }
        #endregion

        private int _tipoTramite = 0;

        public int tipoTramite
        {
            get
            {
                if (_tipoTramite == 0)
                {
                    int.TryParse(hid_tipoTramite.Value, out _tipoTramite);
                }
                return _tipoTramite;
            }
            set
            {
                hid_tipoTramite.Value = value.ToString();
                _tipoTramite = value;
            }
        }

        private int _id_solicitud;
        public int id_solicitud
        {
            get
            {
                if (_id_solicitud == 0)
                {
                    int.TryParse(hid_id_solicitud.Value, out _id_solicitud);
                }
                return _id_solicitud;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
                _id_solicitud = value;
            }
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {          

            try
            {
                
                int estado = 0;
                string codTarea = "";
                string fechaCierre = "";
                string nroExpediente = "";

                IniciarEntity();

                if (txtSolicitud.Text.Length == 0)
                    throw new Exception("Debe ingresar un dato.");

                
                int.TryParse(txtSolicitud.Text.Trim(), out _id_solicitud);

                var solicitud = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);

                if (solicitud == null)
                {
                    var trasnf = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    if (trasnf == null)
                    {
                        var cp = db.CPadron_Solicitudes.FirstOrDefault(x => x.id_cpadron == id_solicitud);
                        if (cp == null)
                            throw new Exception("No se puede encontrar la solicitud.");
                        else
                        {
                            _tipoTramite = (int)Constants.TipoDeTramite.Consulta_Padron;

                            var ultTarea = (from tth in db.SGI_Tramites_Tareas_CPADRON
                                            join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                                            where tth.id_cpadron == id_solicitud
                                            orderby tth.id_tramitetarea descending
                                            select new
                                            {
                                                codTar = tt.ENG_Tareas.cod_tarea,
                                                fechaFin = tt.FechaCierre_tramitetarea
                                            }).FirstOrDefault();

                            estado = cp.id_estado;
                            codTarea = ultTarea != null ?  ultTarea.codTar.ToString() : "";
                            fechaCierre = ultTarea != null ?  ultTarea.fechaFin.ToString() : "";
                            nroExpediente = cp.NroExpedienteSade;
                        }
                    }
                    else
                    {
                        _tipoTramite = (int)Constants.TipoDeTramite.Transferencia;

                        var ultTarea = (from tth in db.SGI_Tramites_Tareas_TRANSF
                                        join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                                        where tth.id_solicitud == id_solicitud
                                        orderby tth.id_tramitetarea descending
                                        select new
                                        {
                                            codTar = tt.ENG_Tareas.cod_tarea,
                                            fechaFin = tt.FechaCierre_tramitetarea
                                        }).FirstOrDefault();

                        estado = trasnf.id_estado;
                        codTarea = ultTarea != null ?  ultTarea.codTar.ToString() : "";
                        fechaCierre = ultTarea != null ?  ultTarea.fechaFin.ToString() : "";
                        nroExpediente = trasnf.NroExpedienteSade;
                    }
                }
                else
                {
                    _tipoTramite = solicitud.id_tipotramite;

                    var ultTarea = (from tth in db.SGI_Tramites_Tareas_HAB
                                    join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                                    where tth.id_solicitud == id_solicitud
                                    orderby tth.id_tramitetarea descending
                                    select new
                                    {
                                        codTar = tt.ENG_Tareas.cod_tarea,
                                        fechaFin = tt.FechaCierre_tramitetarea
                                    }).FirstOrDefault();

                    estado = solicitud.id_estado;
                    codTarea = ultTarea!= null ? ultTarea.codTar.ToString() : "";
                    fechaCierre = ultTarea != null ?  ultTarea.fechaFin.ToString() : "";
                    nroExpediente = solicitud.NroExpedienteSade;
                }
               

                if(estado != (int)Constants.Solicitud_Estados.Datos_Confirmados && (codTarea.ToString().Substring(codTarea.ToString().Length - 2, 2) != Constants.ENG_Tipos_Tareas.Correccion_Solicitud && fechaCierre != null))
                    throw new Exception("La solicitud no se puede eximir.");

                 lblSol.Text = id_solicitud.ToString();
                 lblTextEncomienda.Text = "Nro. Encomienda";
                 lblEstado.Text = db.TipoEstadoSolicitud.Where(x => x.Id == estado).Select(y => y.Descripcion).First();

                if (nroExpediente != "")
                 {
                     lblExpediente.Visible = true;
                     lblExpediente.Text = nroExpediente;
                 }

                if (tipoTramite == (int)Constants.TipoDeTramite.Habilitacion ||
                    tipoTramite == (int)Constants.TipoDeTramite.Ampliacion_Unificacion)
                {
                    var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                                           && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

                    if (enc != null)
                    {
                        string encomienda_desc = enc.id_encomienda.ToString() + " - " + Functions.GetTipoDeTramiteDesc(solicitud.id_tipotramite) + " " + Functions.GetTipoExpedienteDesc(solicitud.id_tipoexpediente, solicitud.id_subtipoexpediente);
                        string objResult = db.SGI_GetDireccionEncomienda(enc.id_encomienda).FirstOrDefault();

                        var encUbic = enc.Encomienda_Ubicaciones.OrderBy(o => o.id_encomiendaubicacion).FirstOrDefault();

                        lblSuperficieTotal.Text = (enc.Encomienda_DatosLocal.First().superficie_cubierta_dl.Value + enc.Encomienda_DatosLocal.First().superficie_descubierta_dl.Value).ToString();
                        lblEncomienda.Text = encomienda_desc;
                        lblUbicacion.Text = objResult;
                    }

                    var titulares = (from pf in db.SSIT_Solicitudes_Titulares_PersonasFisicas
                                     where pf.id_solicitud == id_solicitud
                                     select new
                                     {
                                         label = pf.Apellido + ", " + pf.Nombres
                                     }).Union(
                                     from pj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas
                                     where pj.id_solicitud == id_solicitud
                                     select new
                                     {
                                         label = pj.Razon_Social
                                     }).ToList();
                    lblTitulares.Text = "";
                    foreach (var tit in titulares)
                        lblTitulares.Text = lblTitulares.Text + tit.label + "; ";
                }
                else if (tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                {

                }

                var a = (from excaa in db.Eximicion_CAA
                         where excaa.id_solicitud == id_solicitud && excaa.id_tipo_tramite == tipoTramite
                         orderby excaa.id_eximicion_caa descending
                         select excaa.eximido).FirstOrDefault();
                ChkEximir.Checked = a != null ? a : false;
                hid_id_solicitud.Value = id_solicitud.ToString();
                hid_tipoTramite.Value = tipoTramite.ToString();
                updResultados.Update();
                EjecutarScript(UpdatePanel1, "showResultado();");
            }            
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "showfrmError();");
            }
            finally
            {
                FinalizarEntity();
            }
        }


        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            txtSolicitud.Text = "";
            EjecutarScript(UpdatePanel1, "hideResultado();");
        }

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnGuardar = (LinkButton)sender;

                db = new DGHP_Entities();

                Guid userid = Functions.GetUserId();

                // db.SSIT_ActualizarEximicionCAA(id_solicitud, Convert.ToInt16(ChkEximir.Checked), tipoTramite, userid);
                db.Insert_Eximicion_CAA(id_solicitud, userid, tipoTramite, ChkEximir.Checked);

                LimpiarDatos();
                EjecutarScript(updResultados, "hideResultado();");
                updResultados.Update();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "showfrmError();");

            }
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode("EximiciÓn de CAA");


            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostratMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        private void LimpiarDatos()
        {
            txtSolicitud.Text = "";
            lblEncomienda.Text = "";
            lblEstado.Text = "";
            lblUbicacion.Text = "";
            lblSuperficieTotal.Text = "";
            lblTitulares.Text = "";
        }

        public void LoadData(int id_cpadron)
        {
           // this.id_cpadron = id_cpadron;

            DGHP_Entities db = new DGHP_Entities();

            //string nro_expediente = db.CPadron_Solicitudes.Where(x => x.id_cpadron == this.id_cpadron).Select(x => x.nro_expediente_anterior).FirstOrDefault();

        }

    }
}