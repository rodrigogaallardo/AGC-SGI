using SGI.GestionTramite.Controls.Charts;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public partial class IndicadoresPOA : BasePage
    {
        DGHP_Entities db = null;

        #region load de pagina
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "inicializar_controles", "inicializar_controles();", true);
            }

            if (!IsPostBack)
            {

                db = new DGHP_Entities();

                CargarTipoTramite();
            }
        }
        #endregion


        protected void ddlBusSubtipoExpediente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList lista = (DropDownList)sender;
                if (lista.ID == "ddlBusSubtipoExpediente")
                {
                    Chart.Visible = false;
                    if (ddlBusSubtipoExpediente.SelectedValue != "" && ddlBusSubtipoExpediente.SelectedValue != "99")
                    {
                        int id_subtipoexpediente = int.Parse(ddlBusSubtipoExpediente.SelectedValue);
                        CargarCircuitos(id_subtipoexpediente);
                    }
                    else
                    {
                        CargarCircuitos(99);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);

            }
        }
        private void CargarSubtipoExpediente(int id_tipoexpediente)
        {
            db = new DGHP_Entities();

            var lst_subtipoExpediente = (from sub in db.SubtipoExpediente
                                         join rel in db.ENG_Rel_Circuitos_TiposDeTramite on sub.id_subtipoexpediente equals rel.id_subtipoexpediente
                                         where rel.id_tipoexpediente == id_tipoexpediente
                                         select sub).Distinct();


            ddlBusSubtipoExpediente.DataSource = lst_subtipoExpediente.ToList();
            ddlBusSubtipoExpediente.DataTextField = "descripcion_subtipoexpediente";
            ddlBusSubtipoExpediente.DataValueField = "id_subtipoexpediente";
            ddlBusSubtipoExpediente.DataBind();

            ddlBusSubtipoExpediente.Items.Insert(0, new ListItem("Todos", "99"));

            updpnlBuscar.Update();

            CargarCircuitos(99);


        }

        private void CargarTipoExpediente(int id_tipotramite)
        {
            db = new DGHP_Entities();

            var lst_expediente = (from exp in db.TipoExpediente
                                  join rel in db.ENG_Rel_Circuitos_TiposDeTramite on exp.id_tipoexpediente equals rel.id_tipoexpediente
                                  where rel.id_tipotramite == id_tipotramite
                                  select exp).Distinct();


            ddlBusTipoExpediente.DataSource = lst_expediente.ToList();
            ddlBusTipoExpediente.DataTextField = "descripcion_tipoexpediente";
            ddlBusTipoExpediente.DataValueField = "id_tipoexpediente";
            ddlBusTipoExpediente.DataBind();

            ddlBusTipoExpediente.Items.Insert(0, new ListItem("Todos", "99"));

            updpnlBuscar.Update();

            CargarSubtipoExpediente(99);
        }

        private void CargarTipoTramite()
        {
            var lst_tramite = (from tip in db.TipoTramite
                               where tip.id_tipotramite < 11
                               select tip);

            ddlBusTipoTramite.DataSource = lst_tramite.ToList();
            ddlBusTipoTramite.DataTextField = "descripcion_tipotramite";
            ddlBusTipoTramite.DataValueField = "id_tipotramite";
            ddlBusTipoTramite.DataBind();

            ddlBusTipoTramite.Items.Insert(0, new ListItem("Todos", "99"));

            updpnlBuscar.Update();

        }

        private void CargarCircuitos(int id_subtipoexpediente)
        {
            db = new DGHP_Entities();
            int id_tipotramite = 0;
            int id_tipoexpediente = 0;


            id_tipotramite = int.Parse(ddlBusTipoTramite.SelectedValue);
            id_tipoexpediente = int.Parse(ddlBusTipoExpediente.SelectedValue);


            var lst_circuitos = (from cir in db.ENG_Circuitos
                                 join rel in db.ENG_Rel_Circuitos_TiposDeTramite on cir.id_circuito equals rel.id_circuito
                                 where rel.id_tipotramite == id_tipotramite
                                     && rel.id_tipoexpediente == id_tipoexpediente
                                     && rel.id_subtipoexpediente == id_subtipoexpediente
                                 select cir).Distinct();

            ddlBusCircuito.DataSource = lst_circuitos.ToList();
            ddlBusCircuito.DataTextField = "cod_circuito";
            ddlBusCircuito.DataValueField = "id_circuito";
            ddlBusCircuito.DataBind();

            ddlBusCircuito.Items.Insert(0, new ListItem("Todos", "99"));

            updpnlBuscar.Update();

        }

        private int BusFechaRevMes = 0;
        //Filtros
        private int BusCircuito = 99;

        private void ValidarBuscar()
        {
            this.BusFechaRevMes = 0;

            this.BusCircuito = 99;

            this.BusCircuito = int.Parse(ddlBusCircuito.SelectedValue);
        }
        private void ValidarBuscar2()
        {

            this.BusFechaRevMes = 0;

            //Busqueda / Filtrar Mes y Año Revision

            if (this.BusFechaRevMes > 12)
                throw new Exception("El mes no puede ser mayor a 12 (Diciembre)");

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
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
            }

        }
        private void LimpiarControles()
        {
            ddlBusCircuito.ClearSelection();

            updpnlBuscar.Update();
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }


        protected void ddlBusCircuito_SelectedIndexChanged(object sender, EventArgs e)
        {
            Chart.Visible = false;
        }

        protected void ddlBusTipoTramite_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList lista = (DropDownList)sender;
                if (ddlBusTipoTramite.SelectedValue != "" && ddlBusTipoTramite.SelectedValue != "99")
                {
                    Chart.Visible = false;
                    int id_tipotramite = int.Parse(ddlBusTipoTramite.SelectedValue);
                    CargarTipoExpediente(id_tipotramite);
                }
                else
                {
                    CargarTipoExpediente(99);
                }

            }
            catch (Exception ex)
            {
                LogError.Write(ex);

            }

        }

        protected void ddlBusTipoExpediente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList lista = (DropDownList)sender;

                if (lista.ID == "ddlBusTipoExpediente")
                {
                    Chart.Visible = false;
                    if (ddlBusTipoExpediente.SelectedValue != "" && ddlBusTipoExpediente.SelectedValue != "99")
                    {
                        int id_tipoexpediente = int.Parse(ddlBusTipoExpediente.SelectedValue);
                        CargarSubtipoExpediente(id_tipoexpediente);
                    }
                    else
                    {
                        CargarSubtipoExpediente(99);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);

            }
        }
        #region entity

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

        private void GetDatos(ref IQueryable<ResultadosPOAExcel> ingresados ,ref IQueryable<ResultadosPOAExcel> resueltos )
        {

            

            DateTime dtInicio = DateTime.Parse(txtFechaDesde.Text);
            DateTime dtFin = DateTime.Parse(txtFechaHasta.Text);

            int IdTipoTramite = Convert.ToInt32(ddlBusTipoTramite.SelectedValue);
            int IdTipoExpediente = Convert.ToInt32(ddlBusTipoExpediente.SelectedValue);
            int IdSubTipoExpediente = Convert.ToInt32(ddlBusSubtipoExpediente.SelectedValue);
            int IdCircuito = Convert.ToInt32(ddlBusCircuito.SelectedValue);
            dtFin = dtFin.AddHours(23).AddMinutes(59).AddSeconds(59);

            IndicadoresPOAExcel querys = new IndicadoresPOAExcel();
            var seleccion = (from tt in db.TipoTramite
                             join rel in db.ENG_Rel_Circuitos_TiposDeTramite on tt.id_tipotramite equals rel.id_tipotramite
                             where
                             (rel.id_tipotramite == IdTipoTramite || IdTipoTramite == 99) &&
                             (rel.id_tipoexpediente == IdTipoExpediente || IdTipoExpediente == 99) &&
                             (rel.id_subtipoexpediente == IdSubTipoExpediente || IdSubTipoExpediente == 99) &&
                             (rel.id_circuito == IdCircuito || IdCircuito == 99) 
                             select rel.id_circuito).Distinct().ToList();

            if (seleccion.Contains(1))
            {
                resueltos = querys.GetSSPTotalResueltos(this.db, dtInicio, dtFin);
                ingresados = querys.GetSSPTotalIngresados(this.db, dtInicio, dtFin);
            }
            if (seleccion.Contains(2))
            {
                if (ingresados != null)
                {
                    resueltos.Concat(querys.GetSCPTotalResueltos(this.db, dtInicio, dtFin));
                    ingresados.Concat(querys.GetSCPTotalIngresados(this.db, dtInicio, dtFin));
                }
                else
                {
                    resueltos = querys.GetSCPTotalResueltos(this.db, dtInicio, dtFin);
                    ingresados = querys.GetSCPTotalIngresados(this.db, dtInicio, dtFin);
                }
            }
            if (seleccion.Contains(3))
            {
                if (ingresados != null)
                {
                    resueltos.Concat(querys.GetEspecialTotalResueltos(this.db, dtInicio, dtFin));
                    ingresados.Concat(querys.GetEspecialTotalIngresados(this.db, dtInicio, dtFin));
                }
                else
                {
                    resueltos = querys.GetEspecialTotalResueltos(this.db, dtInicio, dtFin);
                    ingresados = querys.GetEspecialTotalIngresados(this.db, dtInicio, dtFin);
                }
            }
            if (seleccion.Contains(6))
            {
                if (ingresados != null)
                {
                    resueltos = ingresados.Union(querys.GetEsparcimientoTotalResueltos(this.db, dtInicio, dtFin));
                    ingresados = resueltos.Union(querys.GetEsparcimientoTotalIngresados(this.db, dtInicio, dtFin));
                }
                else
                {
                    ingresados = querys.GetEsparcimientoTotalIngresados(this.db, dtInicio, dtFin);
                    resueltos = querys.GetEsparcimientoTotalResueltos(this.db, dtInicio, dtFin);
                }
            }
            if (seleccion.Contains(5))
            {
                if (ingresados != null)
                {
                    ingresados = ingresados.Concat(querys.GetTransferenciaTotalIngresados(this.db, dtInicio, dtFin));
                    resueltos = resueltos.Concat(querys.GetTransferenciaTotalResueltos(this.db, dtInicio, dtFin));
                }
                else
                {
                    ingresados = querys.GetTransferenciaTotalIngresados(this.db, dtInicio, dtFin);
                    resueltos = querys.GetTransferenciaTotalResueltos(this.db, dtInicio, dtFin);
                }
            }
            if (seleccion.Contains(11))
            {
                if (ingresados != null)
                {
                    resueltos = ingresados.Concat(querys.GetSSP2TotalResueltos(this.db, dtInicio, dtFin));
                    ingresados = resueltos.Concat(querys.GetSSP2TotalIngresados(this.db, dtInicio, dtFin));
                }
                else
                {
                    resueltos = querys.GetSSP2TotalResueltos(this.db, dtInicio, dtFin);
                    ingresados = querys.GetSSP2TotalIngresados(this.db, dtInicio, dtFin);
                }
            }
            if (seleccion.Contains(12))
            {
                if (ingresados != null)
                {
                    ingresados = ingresados.Concat(querys.GetSCP2TotalIngresados(this.db, dtInicio, dtFin));
                    resueltos = resueltos.Concat(querys.GetSCP2TotalResueltos(this.db, dtInicio, dtFin));
                }
                else
                {
                    ingresados = querys.GetSCP2TotalIngresados(this.db, dtInicio, dtFin);
                    resueltos = querys.GetSCP2TotalResueltos(this.db, dtInicio, dtFin);
                }
            }
            if (seleccion.Contains(13))
            {
                if (ingresados != null)
                {
                    ingresados = ingresados.Concat(querys.GetIPTotalIngresados(this.db, dtInicio, dtFin));
                    resueltos = resueltos.Concat(querys.GetIPTotalResueltos(this.db, dtInicio, dtFin));
                }
                else
                {
                    ingresados = querys.GetIPTotalIngresados(this.db, dtInicio, dtFin);
                    resueltos = querys.GetIPTotalResueltos(this.db, dtInicio, dtFin);
                }
            }
            if (seleccion.Contains(14))
            {
                if (ingresados != null)
                {
                    ingresados = ingresados.Concat(querys.GetHPTotalIngresados(this.db, dtInicio, dtFin));
                    resueltos = resueltos.Concat(querys.GetHPTotalResueltos(this.db, dtInicio, dtFin));
                }
                else
                {
                    ingresados = querys.GetHPTotalIngresados(this.db, dtInicio, dtFin);
                    resueltos = querys.GetHPTotalResueltos(this.db, dtInicio, dtFin);
                }
            }
            if (seleccion.Contains(15))
            {
                if (ingresados != null)
                {
                    resueltos = ingresados.Concat(querys.GetSSP3TotalResueltos(this.db, dtInicio, dtFin));
                    ingresados = resueltos.Concat(querys.GetSSP3TotalIngresados(this.db, dtInicio, dtFin));
                }
                else
                {
                    resueltos = querys.GetSSP3TotalResueltos(this.db, dtInicio, dtFin);
                    ingresados = querys.GetSSP3TotalIngresados(this.db, dtInicio, dtFin);
                }
            }
            

        }

        protected void btnMostrarGrafico_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarBuscar();
                
                IQueryable<ResultadosPOAExcel> ingresados = null;
                IQueryable<ResultadosPOAExcel> resueltos = null;

                IniciarEntity();
                GetDatos(ref ingresados, ref resueltos);

                if (ingresados != null)
                {
                    ResultadosPOA result = new ResultadosPOA { Ingresados = ingresados.Count(), Resueltos = resueltos.Count() };

                    Chart.Visible = true;
                    Chart.CargarGraficoColumnas(new string[] { "Ingresados", "Resueltos" }, result);
                }

                FinalizarEntity();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                EjecutarScript(updmpeInfo, "showfrmError();");
            }
        }


        protected void btnDescargarBloque1_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();

            try
            {
                ValidarBuscar();

                Chart.Visible = false;

                IQueryable<ResultadosPOAExcel> ingresados = null;
                IQueryable<ResultadosPOAExcel> resueltos = null;
                IniciarEntity();
                GetDatos(ref ingresados, ref resueltos);

                if (ingresados != null)
                {                    
                    ExcelExport1.EjecutarExportacion(new List<IEnumerable<object>>() { ingresados, resueltos }, "Export", new string[] { "Ingresados", "Resueltos" });
                    updpnlBuscar.Update();                    
                }
                
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updmpeInfo, "showfrmError();");
            }
        }
    }
}