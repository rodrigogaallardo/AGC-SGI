using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;

namespace SGI.GestionTramite
{
    public partial class Configurar_Pagos : System.Web.UI.Page
    {
        DGHP_Entities db = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlVencimiento, updPnlVencimiento.GetType(), "init_Js_Vencimiento", "init_Js_Vencimiento();MostrarInfo();", true);
                ScriptManager.RegisterStartupScript(updPnlBolVencidas, updPnlBolVencidas.GetType(), "init_Js_BolVencidas", "init_Js_BolVencidas();MostrarInfo();", true);
                ScriptManager.RegisterStartupScript(updPnlEstadoBoletas, updPnlEstadoBoletas.GetType(), "init_Js_EstadoBoletas", "init_Js_EstadoBoletas();MostrarInfo();", true);
            }

            if (!IsPostBack)
            {
                CargaInicial();
            }
        }


        private void CargaInicial()
        {
            db = new DGHP_Entities();
            List<string> listaJobs = new List<string>();
            listaJobs.Add("j_Pagos_Vencer_Boletas");
            listaJobs.Add("j_Pagos_Evaluar_Vencidos");
            listaJobs.Add("j_Pagos_Evaluar_Pendientes");
            listaJobs.Add("j_Pagos_Evaluar_Pagas_Sin_Fecha");
            try
            {
                var q = (from sysJobs in db.sysjobs_vista
                         where listaJobs.Contains(sysJobs.name)
                         select sysJobs).ToList();

                var k = (from pagParam in db.wsPagos_Parametros
                         select pagParam).ToList();

                var j_Pagos_Vencer_Boletas = q.Where(x => x.name == "j_Pagos_Vencer_Boletas").FirstOrDefault();
                var j_Pagos_Evaluar_Vencidos = q.Where(x => x.name == "j_Pagos_Evaluar_Vencidos").FirstOrDefault();
                var j_Pagos_Evaluar_Pendientes = q.Where(x => x.name == "j_Pagos_Evaluar_Pendientes").FirstOrDefault();
                var j_Pagos_Evaluar_Pagas_Sin_Fecha = q.Where(x => x.name == "j_Pagos_Evaluar_Pagas_Sin_Fecha").FirstOrDefault();

                txtHoraInicioVencimiento.Text = j_Pagos_Vencer_Boletas.active_start_time.ToString();
                txtHoraFinVencimiento.Text = j_Pagos_Vencer_Boletas.active_end_time.ToString();
                txtIntervaloVencimiento.Text = j_Pagos_Vencer_Boletas.intervalo_minutos.ToString();

                txtHoraInicioBolVencidas.Text = j_Pagos_Evaluar_Vencidos.active_start_time.ToString();
                txtHoraFinBolVencidas.Text = j_Pagos_Evaluar_Vencidos.active_end_time.ToString();
                txtIntervaloBolVencidas.Text = j_Pagos_Evaluar_Vencidos.intervalo_minutos.ToString();
                txtDiasEvaluarBolVencidas.Text = k.Where(y => y.cod_param == "Dias.Evaluar.Estados.Boletas").FirstOrDefault().valor_param.ToString();

                txtHoraInicioEstadoBoletas.Text = j_Pagos_Evaluar_Pendientes.active_start_time.ToString();
                txtHoraFinEstadoBoletas.Text = j_Pagos_Evaluar_Pendientes.active_end_time.ToString();
                txtIntervaloEstadoBoletas.Text = j_Pagos_Evaluar_Pendientes.intervalo_minutos.ToString();

                txtHoraInicioPagas_Sin_Fecha.Text = j_Pagos_Evaluar_Pagas_Sin_Fecha.active_start_time.ToString();
                txtHoraFinPagas_Sin_Fecha.Text = j_Pagos_Evaluar_Pagas_Sin_Fecha.active_end_time.ToString();
                txtIntervaloPagas_Sin_Fecha.Text = j_Pagos_Evaluar_Pagas_Sin_Fecha.intervalo_minutos.ToString();
                txtDiasEvaluarPagas_Sin_Fecha.Text = k.Where(y => y.cod_param == "Dias.Evaluar.Boletas.Sin.Fecha.Pago").FirstOrDefault().valor_param.ToString();


                txtEstadoVencimiento.Text = j_Pagos_Vencer_Boletas.Estado;
                txtEstadoVencimiento.ForeColor = j_Pagos_Vencer_Boletas.Estado == "Activo" ? System.Drawing.Color.Blue : System.Drawing.Color.Red;
                lnkEstadoVencimiento.Text = (txtEstadoVencimiento.Text == "Activo") ? "Inactivar" : "Activar";

                txtEstadoBolVencidas.Text = j_Pagos_Evaluar_Vencidos.Estado;
                txtEstadoBolVencidas.ForeColor = j_Pagos_Evaluar_Vencidos.Estado == "Activo" ? System.Drawing.Color.Blue : System.Drawing.Color.Red;
                lnkEstadoBolVencidas.Text = (txtEstadoBolVencidas.Text == "Activo") ? "Inactivar" : "Activar";

                txtEstadoPagas_Sin_Fecha.Text = j_Pagos_Evaluar_Pagas_Sin_Fecha.Estado;
                txtEstadoPagas_Sin_Fecha.ForeColor = j_Pagos_Evaluar_Pagas_Sin_Fecha.Estado == "Activo" ? System.Drawing.Color.Blue : System.Drawing.Color.Red;
                lnkEstadoPagas_Sin_Fecha.Text = (txtEstadoPagas_Sin_Fecha.Text == "Activo") ? "Inactivar" : "Activar";

                txtEstadoBoletas.Text = j_Pagos_Evaluar_Pendientes.Estado;
                txtEstadoBoletas.ForeColor = j_Pagos_Evaluar_Pendientes.Estado == "Activo" ? System.Drawing.Color.Blue : System.Drawing.Color.Red;
                lnkEstadoBoletas.Text = (txtEstadoBoletas.Text == "Activo") ? "Inactivar" : "Activar";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }
        protected void btnGuardarVencimiento_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            int inicio;
            int fin;
            int intervalo;

            inicio = Convert.ToInt32(txtHoraInicioVencimiento.Text.Replace(":", "") + "00");
            fin = Convert.ToInt32(txtHoraFinVencimiento.Text.Replace(":", "") + "59");
            intervalo = Convert.ToInt32(txtIntervaloVencimiento.Text);
            try
            {
                db.wsPagos_UpdJobIntervalo("j_Pagos_Vencer_Boletas", inicio, fin, intervalo);
                ScriptManager.RegisterClientScriptBlock(updPnlGuardarVencimiento, updPnlGuardarVencimiento.GetType(), "", "mostratMensaje('La actualización se realizó exitosamente.')", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        protected void btnGuardarBolVencidas_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            int inicio;
            int fin;
            int intervalo;
            string dias;

            inicio = Convert.ToInt32(txtHoraInicioBolVencidas.Text.Replace(":", "") + "00");
            fin = Convert.ToInt32(txtHoraFinBolVencidas.Text.Replace(":", "") + "59");
            intervalo = Convert.ToInt32(txtIntervaloBolVencidas.Text);
            dias = txtDiasEvaluarBolVencidas.Text;

            try
            {
                db.wsPagos_UpdJobIntervalo("j_Pagos_Evaluar_Vencidos", inicio, fin, intervalo);
                db.wsPagos_UpdParametro("Dias.Evaluar.Estados.Boletas", dias);
                ScriptManager.RegisterClientScriptBlock(updPnlGuardarBolVencidas, updPnlGuardarBolVencidas.GetType(), "", "mostratMensaje('La actualización se realizó exitosamente.')", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        protected void btnGuardarEstadoBoletas_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            int inicio;
            int fin;
            int intervalo;
            string dias;

            inicio = Convert.ToInt32(txtHoraInicioEstadoBoletas.Text.Replace(":", "") + "00");
            fin = Convert.ToInt32(txtHoraFinEstadoBoletas.Text.Replace(":", "") + "59");
            intervalo = Convert.ToInt32(txtIntervaloEstadoBoletas.Text);
            
            try
            {
                db.wsPagos_UpdJobIntervalo("j_Pagos_Evaluar_Pendientes", inicio, fin, intervalo);
                ScriptManager.RegisterClientScriptBlock(updPnlGuardarEstadoBoletas, updPnlGuardarEstadoBoletas.GetType(), "", "mostratMensaje('La actualización se realizó exitosamente.')", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        protected void btnGuardarPagas_Sin_Fecha_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            int inicio;
            int fin;
            int intervalo;
            string dias;

            inicio = Convert.ToInt32(txtHoraInicioPagas_Sin_Fecha.Text.Replace(":", "") + "00");
            fin = Convert.ToInt32(txtHoraFinPagas_Sin_Fecha.Text.Replace(":", "") + "59");
            intervalo = Convert.ToInt32(txtIntervaloPagas_Sin_Fecha.Text);
            dias = txtDiasEvaluarPagas_Sin_Fecha.Text;

            try
            {
                db.wsPagos_UpdJobIntervalo("j_Pagos_Evaluar_Pagas_Sin_Fecha", inicio, fin, intervalo);
                db.wsPagos_UpdParametro("Dias.Evaluar.Boletas.Sin.Fecha.Pago", dias);
                ScriptManager.RegisterClientScriptBlock(updPnlGuardarPagas_Sin_Fecha, updPnlGuardarPagas_Sin_Fecha.GetType(), "", "mostratMensaje('La actualización se realizó exitosamente.')", true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

        }

        protected void lnkEstadoVencimiento_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            bool EstadoJob;
            EstadoJob = (txtEstadoVencimiento.Text == "Activo") ? false : true;
            try
            {
                db.wsPagos_UpdJobEstado("j_Pagos_Vencer_Boletas", EstadoJob);
            }
            catch { }

            if (EstadoJob)
            {
                lnkEstadoVencimiento.Text = "Inactivar";
                txtEstadoVencimiento.Text = "Activo";
                txtEstadoVencimiento.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                lnkEstadoVencimiento.Text = "Activar";
                txtEstadoVencimiento.Text = "Inactivo";
                txtEstadoVencimiento.ForeColor = System.Drawing.Color.Red;
            }

            updEstadoJobsVencimiento.Update();
        }

        protected void lnkEstadoBolVencidas_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            bool EstadoJob;
            EstadoJob = (txtEstadoBolVencidas.Text == "Activo") ? false : true;
            try
            {
                db.wsPagos_UpdJobEstado("j_Pagos_Evaluar_Vencidos", EstadoJob);
            }
            catch { }

            if (EstadoJob)
            {
                lnkEstadoBolVencidas.Text = "Inactivar";
                txtEstadoBolVencidas.Text = "Activo";
                txtEstadoBolVencidas.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                lnkEstadoBolVencidas.Text = "Activar";
                txtEstadoBolVencidas.Text = "Inactivo";
                txtEstadoBolVencidas.ForeColor = System.Drawing.Color.Red;
            }

            updEstadoJobsBolVencidas.Update();
        }

        protected void lnkEstadoBoletas_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            bool EstadoJob;
            EstadoJob = (txtEstadoBoletas.Text == "Activo") ? false : true;
            try
            {
                db.wsPagos_UpdJobEstado("j_Pagos_Evaluar_Pendientes", EstadoJob);
            }
            catch { }

            if (EstadoJob)
            {
                lnkEstadoBoletas.Text = "Inactivar";
                txtEstadoBoletas.Text = "Activo";
                txtEstadoBoletas.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                lnkEstadoBoletas.Text = "Activar";
                txtEstadoBoletas.Text = "Inactivo";
                txtEstadoBoletas.ForeColor = System.Drawing.Color.Red;
            }

            updEstadoJobsBoletas.Update();
        }

        protected void lnkEstadoPagas_Sin_Fecha_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            bool EstadoJob;
            EstadoJob = (txtEstadoPagas_Sin_Fecha.Text == "Activo") ? false : true;
            try
            {
                db.wsPagos_UpdJobEstado("j_Pagos_Evaluar_Pagas_Sin_Fecha", EstadoJob);
            }
            catch { }

            if (EstadoJob)
            {
                lnkEstadoPagas_Sin_Fecha.Text = "Inactivar";
                txtEstadoPagas_Sin_Fecha.Text = "Activo";
                txtEstadoPagas_Sin_Fecha.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                lnkEstadoPagas_Sin_Fecha.Text = "Activar";
                txtEstadoPagas_Sin_Fecha.Text = "Inactivo";
                txtEstadoPagas_Sin_Fecha.ForeColor = System.Drawing.Color.Red;
            }
            updEstadoJobsPagas_Sin_Fecha.Update();
        }
    }
}