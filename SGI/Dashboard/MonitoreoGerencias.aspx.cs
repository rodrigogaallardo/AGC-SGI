using SGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using System.Web.Services;
using SGI.StaticClassNameSpace;
using System.Text;

namespace SGI.Dashboard
{
    public partial class MonitoreoGerencias : System.Web.UI.Page
    {

        private DateTime fechaDesde;
        private DateTime fechaHasta;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            try
            {
                IniciarEntity();
                CargarComboGerencias();
                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                throw ex;
            }
        }

        private void CargarComboGerencias()
        {
            var gerencias = (from g in db.ENG_Gerencias
                             select new
                             {
                                 g.cod_gerencia,
                                 Descripcion = g.Descripcion,
                             });

            List<GerenciasDTO> listGerencias = new List<GerenciasDTO>(); ;

            listGerencias.Insert(0, new GerenciasDTO("0", "Todos"));

            foreach (var item in gerencias.ToList())
            {
                listGerencias.Add(new GerenciasDTO(item.cod_gerencia, item.Descripcion));
            }

            ddlGerencias.DataTextField = "Descripcion";
            ddlGerencias.DataValueField = "cod_gerencia";
            ddlGerencias.DataSource = listGerencias;
            ddlGerencias.DataBind();
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
                //IniciarPaneles();
                Validar_BuscarPorFechas();
                CargarTramites();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                LogError.Write(ex, "error al buscar tramites");
                Enviar_Mensaje(ex.Message, "");
            }
        }

        private void IniciarPaneles()
        {
            ScriptManager.RegisterStartupScript(updResultado, updResultado.GetType(), "hideResultados", "hideResultados()", true);
        }

        private void CargarTramites()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtFechaDesde.Text))
                {
                    if (!DateTime.TryParse(txtFechaDesde.Text, out fechaDesde))
                        throw new Exception("Fecha desde inválida.");
                    fechaDesde = fechaDesde.AddHours(0).AddMinutes(0).AddSeconds(0);
                    hdntxtFechaDesde.Value = fechaDesde.ToString();
                }

                if (!string.IsNullOrEmpty(txtFechaHasta.Text))
                {
                    if (!DateTime.TryParse(txtFechaHasta.Text, out fechaHasta))
                        throw new Exception("Fecha Hasta inválida.");
                    fechaHasta = fechaHasta.AddHours(23).AddMinutes(59).AddSeconds(59);
                    hdntxtFechaHasta.Value = fechaHasta.ToString();
                }

                //Guid userid = Functions.GetUserId();
                int cod_solic_de = (int)Constants.ENG_Tareas.SSP_Solicitud_Habilitacion;
                string cod_solicitud_de = cod_solic_de.ToString().PadLeft(2, '0');
                int cod_corr_solic = (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud;

                db = new DGHP_Entities();
                var gerencias = (from g in db.ENG_Gerencias select g);

                var Tareas_HAB = (
                    from tth in db.SGI_Tramites_Tareas_HAB
                    join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                    join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                    join c in db.ENG_Circuitos on t.id_circuito equals c.id_circuito
                    join gc in db.ENG_Gerencias on c.id_gerencia equals gc.id_gerencia
                    where
                        tt.FechaInicio_tramitetarea >= fechaDesde && tt.FechaInicio_tramitetarea <= fechaHasta
                        && tt.FechaCierre_tramitetarea == null
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_corr_solic.ToString()
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                    group t.ENG_Circuitos.ENG_Gerencias by
                    new
                    {
                        gc.id_gerencia,
                        gc.cod_gerencia,
                        gc.Descripcion
                    } into g

                    select new
                    {
                        g.Key.cod_gerencia,
                        g.Key.Descripcion,
                        cantidad = g.Count()
                    }
                    );

                var Tareas_TRANSF =
                (
                    from tth in db.SGI_Tramites_Tareas_TRANSF
                    join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                    join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                    join c in db.ENG_Circuitos on t.id_circuito equals c.id_circuito
                    join gc in db.ENG_Gerencias on c.id_gerencia equals gc.id_gerencia
                    where
                        tt.FechaInicio_tramitetarea >= fechaDesde && tt.FechaInicio_tramitetarea <= fechaHasta
                        && tt.FechaCierre_tramitetarea == null
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_corr_solic.ToString()
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                    group t.ENG_Circuitos.ENG_Gerencias by
                    new
                    {
                        gc.id_gerencia,
                        gc.cod_gerencia,
                        gc.Descripcion
                    } into g

                    select new
                    {
                        g.Key.cod_gerencia,
                        g.Key.Descripcion,
                        cantidad = g.Count()
                    }
                    );

                var todosLasTareasUnion = Tareas_HAB.Union(Tareas_TRANSF);
                var todosLasTareas = todosLasTareasUnion.GroupBy(l => l.cod_gerencia).Select(g => new
                {
                    cantidad = g.Select(x => x.cantidad).Sum(),
                    cod_gerencia = g.Key
                });

                var q = (
                        from tt in todosLasTareas
                        join g in gerencias on tt.cod_gerencia equals g.cod_gerencia
                        select new
                        {
                            id_gerencia = g.id_gerencia,
                            cod_gerencia = tt.cod_gerencia,
                            descripcion = g.Descripcion,
                            cantidad = tt.cantidad,
                            fechaDesde = fechaDesde,
                            fechaHasta = fechaHasta
                        });

                if (ddlGerencias.SelectedValue != "0")
                {
                    q = q.Where(x => x.cod_gerencia == ddlGerencias.SelectedItem.Value);
                }

                grdCalificadores.DataSource = q.ToList();
                grdCalificadores.DataBind();


                if (q.ToList().Count > 0)
                {
                    DivCanvas.Style.Add("width", "0");
                    var listaCalif = (from c in q
                                      select new ChartData
                                      {
                                          label = (c.id_gerencia.ToString() + "-" + c.descripcion),
                                          valor = c.cantidad
                                      }).ToList();


                    string datos = String.Join(",", listaCalif.Select(x => x.valor).ToArray());
                    string label = String.Join(",", listaCalif.Select(x => x.label).ToArray());
                    string[] colors = new string[datos.Length];

                    for (int i = 0; i < datos.Length; i++)
                    {
                        colors[i] = String.Format("{0:X6}", ChartColor.Colores[i]);
                    }

                    string colorChart = String.Join(",", colors);

                    string data = string.Format("Graficar('{0}','{1}','{2}');", datos, label, colorChart);
                    ScriptManager.RegisterStartupScript(updResultado, updResultado.GetType(), "Graficar", data, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(updResultado, updResultado.GetType(), "clearChart", "clearChart()", true);
                }
                ScriptManager.RegisterStartupScript(updResultado, updResultado.GetType(), "showResultados", "showResultados()", true);

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(udpError, udpError.GetType(), "showfrmError", "showfrmError();", true);
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
                ScriptManager.RegisterStartupScript(btnBuscar, btnBuscar.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
            }

        }

        private void Validar_BuscarPorFechas()
        {
            if (!string.IsNullOrEmpty(txtFechaDesde.Text))
            {
                if (!DateTime.TryParse(txtFechaDesde.Text, out fechaDesde))
                    throw new Exception("Fecha desde inválida.");
                hdntxtFechaDesde.Value = txtFechaDesde.Text;
            }

            if (!string.IsNullOrEmpty(txtFechaHasta.Text))
            {
                if (!DateTime.TryParse(txtFechaHasta.Text, out fechaHasta))
                    throw new Exception("Fecha Hasta inválida.");
                hdntxtFechaHasta.Value = txtFechaHasta.Text;
            }

            if (fechaDesde != null && fechaHasta != null && this.fechaDesde > this.fechaHasta)
                throw new Exception("Fecha desde superior a fecha hasta.");
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {

            if (ddlGerencias.Items.Count >= 0)
                ddlGerencias.SelectedIndex = 0;

            txtFechaDesde.Text = String.Empty;
            txtFechaHasta.Text = String.Empty;
            ScriptManager.RegisterStartupScript(updResultado, updResultado.GetType(), "inicializar_fechas", "inicializar_fechas()", true);
            IniciarPaneles();
        }

        protected void DetalleGraf_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                string[] Argumentos = btn.CommandArgument.ToString().Split(new char[] { ',' });
                string id_gerencia = "idg=" + Functions.ConvertToBase64String(Argumentos[0]);
                string cod_gerencia = Argumentos[1];

                DateTime.TryParse(hdntxtFechaDesde.Value, out fechaDesde);
                DateTime.TryParse(hdntxtFechaHasta.Value, out fechaHasta);
                string fDesde = "fD=" + Functions.ConvertToBase64String(fechaDesde.ToShortDateString());
                string fHasta = "fH=" + Functions.ConvertToBase64String(fechaHasta.ToShortDateString());

                string sURL = "~/Dashboard/MonitoreoGerenciasDetalle.aspx?" + id_gerencia + "&" + fDesde + "&" + fHasta;
                Response.Redirect(sURL, true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(udpError, udpError.GetType(), "showfrmError", "showfrmError();", true);
            }

        }
    }

    internal class GerenciasDTO
    {
        public string cod_gerencia { get; set; }
        public string Descripcion { get; set; }

        public GerenciasDTO(string cod_gerencia, string Descripcion)
        {
            this.cod_gerencia = cod_gerencia;
            this.Descripcion = Descripcion;
        }
    }
}