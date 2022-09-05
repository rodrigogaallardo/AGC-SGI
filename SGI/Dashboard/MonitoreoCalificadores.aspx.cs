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
    public partial class MonitoreoCalificadores : System.Web.UI.Page
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
                CargarComboEquipoDeTrabajo();
                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                throw ex;
            }
        }

        private void CargarComboEquipoDeTrabajo()
        {
            Guid userid = Functions.GetUserId();

            var equipo = (from equ in db.ENG_EquipoDeTrabajo
                          join us in db.aspnet_Users on equ.Userid equals us.UserId
                          join pro in db.SGI_Profiles on us.UserId equals pro.userid
                          where
                            equ.Userid_Responsable == userid
                          orderby pro.Apellido
                          select new
                          {
                              pro.userid,
                              calificadorNombre = (pro.Apellido.ToUpper() + ", " + pro.Nombres.ToUpper()),
                          }).ToList();

            List<Calificador> listCalificadores = new List<Calificador>(); ;

            listCalificadores.Insert(0, new Calificador("0", "Todos"));

            foreach (var item in equipo)
            {
                listCalificadores.Add(new Calificador(item.userid.ToString(), item.calificadorNombre.ToString()));
            }

            ddlCalificador.DataTextField = "CalificadorNombre";
            ddlCalificador.DataValueField = "userid";
            ddlCalificador.DataSource = (from ca in listCalificadores
                                         select new
                                         {
                                             ca.Userid,
                                             ca.CalificadorNombre
                                         }).ToList();
            ddlCalificador.DataBind();
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
            ScriptManager.RegisterStartupScript(updDetalles, updDetalles.GetType(), "hideDetalles", "hideDetalles()", true);
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

                Guid userid = Functions.GetUserId();

                db = new DGHP_Entities();

                //AddDays(-1) PARA LIMITAR LA FECHA AL DÍA ANTERIOR A LA CONSULTA, PARA NO TENER INFORMACIÓN INCOMPLETA AL MOMENTO DE LA BÚSQUEDA.
                if (fechaDesde.Date < fechaHasta.Date)
                {
                    fechaHasta = fechaHasta.AddDays(-1);
                    hdntxtFechaHasta.Value = fechaHasta.ToString();
                }


                var todosLosDelGrupoDelResponable = (from tth in db.SGI_Tramites_Tareas_HAB
                                                     join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                                                     join eq in db.ENG_EquipoDeTrabajo on tt.UsuarioAsignado_tramitetarea equals eq.Userid
                                                     where
                                                       (tth.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Calificar ||
                                                       tth.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Primer_Calificar) &&
                                                       (tth.SGI_Tramites_Tareas.FechaCierre_tramitetarea >= fechaDesde &&
                                                       tth.SGI_Tramites_Tareas.FechaCierre_tramitetarea <= fechaHasta) &&
                                                       eq.Userid_Responsable == userid
                                                     select new
                                                     {
                                                         UsuarioAsignado_tramitetarea = (System.Guid?)tth.SGI_Tramites_Tareas.UsuarioAsignado_tramitetarea,
                                                         tth.id_solicitud
                                                     }).GroupBy(l => l.UsuarioAsignado_tramitetarea).Select(g => new
                                                     {
                                                         cantidad = g.Select(x => x.id_solicitud).Distinct().Count(),
                                                         UsuarioAsignado_tramitetarea = g.Key
                                                     });

                var todosLosDelGrupoDelResponableTram = (from tth in db.SGI_Tramites_Tareas_TRANSF
                                                     join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                                                     join eq in db.ENG_EquipoDeTrabajo on tt.UsuarioAsignado_tramitetarea equals eq.Userid
                                                     where
                                                       (tth.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Calificar ||
                                                       tth.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Primer_Calificar) &&
                                                       (tth.SGI_Tramites_Tareas.FechaCierre_tramitetarea >= fechaDesde &&
                                                       tth.SGI_Tramites_Tareas.FechaCierre_tramitetarea <= fechaHasta) &&
                                                       eq.Userid_Responsable == userid
                                                     select new
                                                     {
                                                         UsuarioAsignado_tramitetarea = (System.Guid?)tth.SGI_Tramites_Tareas.UsuarioAsignado_tramitetarea,
                                                         tth.id_solicitud
                                                     }).GroupBy(l => l.UsuarioAsignado_tramitetarea).Select(g => new
                                                     {
                                                         cantidad = g.Select(x => x.id_solicitud).Distinct().Count(),
                                                         UsuarioAsignado_tramitetarea = g.Key
                                                     });

                todosLosDelGrupoDelResponable = todosLosDelGrupoDelResponable.Union(todosLosDelGrupoDelResponableTram);

                var q = (from tbl_Cantidad in todosLosDelGrupoDelResponable
                         join sgi_prof in db.SGI_Profiles on tbl_Cantidad.UsuarioAsignado_tramitetarea.ToString() equals sgi_prof.userid.ToString()
                         select new
                         {
                             tbl_Cantidad.cantidad,
                             tbl_Cantidad.UsuarioAsignado_tramitetarea,
                             sgi_prof.Apellido,
                             sgi_prof.Nombres
                         });

                if (ddlCalificador.SelectedValue != "0")
                {
                    q = q.Where(x => x.UsuarioAsignado_tramitetarea == new Guid(ddlCalificador.SelectedItem.Value));
                }

                grdCalificadores.DataSource = (from x in q
                                               select new
                                               {
                                                   Calificador = x.Apellido.ToUpper() + ", " + x.Nombres.ToUpper(),
                                                   x.cantidad,
                                                   fechaDesde,
                                                   fechaHasta,
                                                   x.UsuarioAsignado_tramitetarea
                                               }).ToList();
                grdCalificadores.DataBind();


                if (q.ToList().Count > 0)
                {
                    DivCanvas.Style.Add("width", "0");
                    var listaCalif = (from c in q
                                      select new ChartData
                                      {
                                          label = (c.Apellido.ToUpper() + "-" + c.Nombres.ToUpper()),
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
                    ScriptManager.RegisterStartupScript(updDetalles, updDetalles.GetType(), "hideDetalles", "hideDetalles()", true);
                }
                ScriptManager.RegisterStartupScript(updResultado, updResultado.GetType(), "showResultados", "showResultados()", true);
                ScriptManager.RegisterStartupScript(updDetalles, updDetalles.GetType(), "hideDetalles", "hideDetalles()", true);

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

            if (ddlCalificador.Items.Count >= 0)
                ddlCalificador.SelectedIndex = 0;

            txtFechaDesde.Text = String.Empty;
            txtFechaHasta.Text = String.Empty;
            ScriptManager.RegisterStartupScript(updResultado, updResultado.GetType(), "inicializar_fechas", "inicializar_fechas()", true);
            IniciarPaneles();
        }

        protected void id_solicitud_Click(object sender, EventArgs e)
        {
            try
            {
                db = new DGHP_Entities();

                LinkButton btn = (LinkButton)(sender);
                string[] Argumentos = btn.CommandArgument.ToString().Split(new char[] { ',' });

                string UserId = Argumentos[0];
                string Calificador = Argumentos[1] + ", " + Argumentos[2];

                DateTime.TryParse(hdntxtFechaDesde.Value, out fechaDesde);
                DateTime.TryParse(hdntxtFechaHasta.Value, out fechaHasta);

                var q = from tbl in ((from Tram in db.SGI_Tramites_Tareas_HAB
                                      join cir in db.ENG_Circuitos on Tram.SGI_Tramites_Tareas.ENG_Tareas.id_circuito equals cir.id_circuito
                                      where
                                        (Tram.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Calificar ||
                                        Tram.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Primer_Calificar) &&                                        
                                        Tram.SGI_Tramites_Tareas.UsuarioAsignado_tramitetarea == new Guid(UserId)
                                      select new
                                      {
                                          Tram.id_solicitud,
                                          cir.cod_circuito,
                                          Tram.id_tramitetarea,
                                          FechaCierre_tramitetarea = Tram.SGI_Tramites_Tareas.FechaCierre_tramitetarea
                                      }))
                        where
                        tbl.FechaCierre_tramitetarea >= fechaDesde && tbl.FechaCierre_tramitetarea <= fechaHasta
                        group tbl by new
                        {
                            tbl.id_solicitud,
                            tbl.cod_circuito,
                        } into g
                        select new
                        {
                            id_solicitud = g.Key.id_solicitud,
                            g.Key.cod_circuito,
                            FechaCierre_tramitetarea = g.Max(p => p.FechaCierre_tramitetarea),
                            Cantidad_de_Calificaciones = g.Count(p => p.id_tramitetarea != null)
                        };

                var qtr = from tbl in ((from Tram in db.SGI_Tramites_Tareas_TRANSF
                                          join cir in db.ENG_Circuitos on Tram.SGI_Tramites_Tareas.ENG_Tareas.id_circuito equals cir.id_circuito
                                          where
                                            (Tram.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Calificar ||
                                            Tram.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Primer_Calificar) &&
                                            Tram.SGI_Tramites_Tareas.UsuarioAsignado_tramitetarea == new Guid(UserId)
                                          select new
                                          {
                                              Tram.id_solicitud,
                                              cir.cod_circuito,
                                              Tram.id_tramitetarea,
                                              FechaCierre_tramitetarea = Tram.SGI_Tramites_Tareas.FechaCierre_tramitetarea
                                          }))
                            where
                            tbl.FechaCierre_tramitetarea >= fechaDesde && tbl.FechaCierre_tramitetarea <= fechaHasta
                            group tbl by new
                            {
                                tbl.id_solicitud,
                                tbl.cod_circuito,
                            } into g
                            select new
                            {
                                id_solicitud = g.Key.id_solicitud,
                                g.Key.cod_circuito,
                                FechaCierre_tramitetarea = g.Max(p => p.FechaCierre_tramitetarea),
                                Cantidad_de_Calificaciones = g.Count(p => p.id_tramitetarea != null)
                            };
                q = q.Union(qtr);

                grdSolicitudes.DataSource = q.ToList();

                grdSolicitudes.DataBind();
                DetalleCalificador.Text = "Calificador : " + Calificador;
                DetalleFechaDesde.Text = "Fecha Desde : " + fechaDesde.ToShortDateString();
                DetalleFechaHasta.Text = "Fecha Hasta : " + fechaHasta.ToShortDateString();

                ScriptManager.RegisterStartupScript(updResultado, updResultado.GetType(), "showDetalles", "showDetalles()", true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(udpError, udpError.GetType(), "showfrmError", "showfrmError();", true);
            }

        }
    }


    internal class Calificador
    {
        public string Userid { get; set; }

        public string CalificadorNombre { get; set; }

        public Calificador(string userId, string Calificador)
        {
            this.Userid = userId;
            this.CalificadorNombre = Calificador;
        }
    }
}