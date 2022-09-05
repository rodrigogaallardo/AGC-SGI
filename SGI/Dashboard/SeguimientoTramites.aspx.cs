using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.StaticClassNameSpace;

namespace SGI.Dashboard
{
    public partial class SeguimientoTramites : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "hideBotonesCanvas", "hideBotonesCanvas();", true);
                btnBuscar_Click(sender, e);
            }
        }

        #region entity

        private DGHP_Entities db = null;

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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                IniciarEntity();
                CargarTramites();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "showfrmError", "showfrmError();", true);
            }
            finally
            {
                FinalizarEntity();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "finalizarCarga", "finalizarCarga();", true);
            }
        }

        protected void btnGraficoGerencias_Click(object sender, EventArgs e)
        {
            Response.Redirect(hidUrlGrfGerencias.Value);
        }

        protected void btnGraficoTipos_Click(object sender, EventArgs e)
        {
            Response.Redirect(hidUrlGrfTipos.Value);
        }

        private void CargarTramites()
        {
            try
            {
                int cod_corr_solic = (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud;
                int cod_solic_de = (int)Constants.ENG_Tareas.SSP_Solicitud_Habilitacion;
                string cod_solicitud_de = cod_solic_de.ToString().PadLeft(2, '0');
                var q = (
                        from a in (
                            ((
                                from stth in db.SGI_Tramites_Tareas_HAB
                                where
                                  stth.SGI_Tramites_Tareas.FechaCierre_tramitetarea == null &&
                                    (stth.SSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.En_trámite
                                    || stth.SSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.RevCaducidad
                                    || stth.SSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Rechazada)
                                    && stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                                select new
                                {
                                    Tramites = "Solicitudes del lado de la Administración",
                                    Id = stth.SSIT_Solicitudes.id_solicitud
                                }
                            ).Union
                            (
                                from stth in db.SGI_Tramites_Tareas_TRANSF 
                                where
                                  stth.SGI_Tramites_Tareas.FechaCierre_tramitetarea == null &&
                                  (stth.Transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.En_trámite
                                    || stth.Transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.RevCaducidad
                                    || stth.Transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Rechazada)
                                    && stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                                select new
                                {
                                    Tramites = "Solicitudes del lado de la Administración",
                                    Id = stth.Transf_Solicitudes.id_solicitud
                                }
                            ).Union
                            (
                                from stth in db.SGI_Tramites_Tareas_HAB
                                where
                                  stth.SGI_Tramites_Tareas.FechaCierre_tramitetarea == null &&
                                  stth.SSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Observado &&
                                  stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) == cod_corr_solic.ToString()
                                select new
                                {
                                    Tramites = "Solicitudes del lado del Administrado",
                                    Id = stth.SSIT_Solicitudes.id_solicitud
                                }
                            ).Union
                            (
                                from stth in db.SGI_Tramites_Tareas_TRANSF 
                                where
                                  stth.SGI_Tramites_Tareas.FechaCierre_tramitetarea == null &&
                                  stth.Transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Observado &&
                                  stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) == cod_corr_solic.ToString()
                                select new
                                {
                                    Tramites = "Solicitudes del lado del Administrado",
                                    Id = stth.Transf_Solicitudes.id_solicitud
                                }
                            )))
                        group a by new
                        {
                            a.Tramites
                        } into g
                        select new
                        {
                            g.Key.Tramites,
                            Cantidad = g.Count()
                        });

                grdTramites.DataSource = q.ToList();
                grdTramites.DataBind();

                if (q.ToList().Count > 0)
                {
                    var l = (from c in q
                                select new ChartData
                                {
                                    label = (c.Tramites.ToString()),
                                    valor = (int)c.Cantidad
                                }).ToList();


                    var result = l.Select(x => x.valor).ToArray();
                    string datos = String.Join(",", result);
                    string label = String.Join(",", l.Select(x => x.label).ToArray());
                    string[] colors = new string[result.Count()];

                    colors[0] = String.Format("{0:X6}", ChartColor.amarillo);
                    colors[1] = String.Format("{0:X6}", ChartColor.azul);

                    string colorChart = String.Join(",", colors);

                    string script = string.Format("Graficar('{0}','{1}','{2}');", datos, label, colorChart);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Graficar", script, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "clearChart", "clearChart();", true);
                }
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "showResultados", "showResultados();", true); 
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "showfrmError", "showfrmError();", true);
            }
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);

            string script_nombre = "mostrarMensaje";
            string script = "mostrarMensaje('" + mensaje + "','" + titulo + "');";

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm != null && sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "hideBotonesCanvas", "hideBotonesCanvas();", true);
            btnBuscar_Click(sender, e);
        }

        protected void DetalleGraf_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                string[] Argumentos = btn.CommandArgument.ToString().Split(new char[] { ',' });
                string tramites = Functions.ConvertToBase64String(Argumentos[0]);

                var urlGerencias = "../Dashboard/SeguimientoTramitesGerencias.aspx?trm=" + tramites;
                hidUrlGrfGerencias.Value = urlGerencias;

                var urlTipos = "../Dashboard/SeguimientoTramitesTipos.aspx?trm=" + tramites;
                hidUrlGrfTipos.Value = urlTipos;

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "showBotonesCanvas", "showBotonesCanvas();", true);
                btnBuscar_Click(sender, e);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(updfrmerror, updfrmerror.GetType(), "showfrmError", "showfrmError();", true);
            }

        }
    }
}