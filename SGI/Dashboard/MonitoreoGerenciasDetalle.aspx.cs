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
using System.Data.Entity;
using System.Linq.Expressions;

namespace SGI.Dashboard
{
    public partial class MonitoreoGerenciasDetalle : System.Web.UI.Page
    {

        private DateTime fechaDesde;
        private DateTime fechaHasta;
        private int id_gerencia = 0;
        private int sumFooterTareas = 0;
        private int sumFooterPersonas = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Cargo los parametros
                CargarParametros();
                LoadData();
            }
        }

        private void CargarParametros()
        {
            string idg = HttpUtility.UrlDecode(Request.QueryString["idg"]);
            string fD = HttpUtility.HtmlDecode(Request.QueryString["fD"]);
            string fH = HttpUtility.HtmlDecode(Request.QueryString["fH"]);
            if (idg.Length > 0 && fD.Length > 0 && fH.Length > 0)
            {
                id_gerencia = Convert.ToInt32(Encoding.ASCII.GetString(Convert.FromBase64String((idg))));
                fechaDesde = Convert.ToDateTime(Encoding.ASCII.GetString(Convert.FromBase64String(fD)));
                fechaHasta = Convert.ToDateTime(Encoding.ASCII.GetString(Convert.FromBase64String(fH)));
                fechaHasta = fechaHasta.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            else
            {
                throw new Exception("error en los parametros");
            }
        }

        private void LoadData()
        {
            try
            {
                IniciarEntity();
                CargarGraficoTareas();
                CargarGraficoPersonas();
                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                throw ex;
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


        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);

            string script_nombre = "mostrarMensaje";
            string script = "mostrarMensaje('" + mensaje + "','" + titulo + "');";

            ScriptManager sm = ScriptManager.GetCurrent(this);

            ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
        }


        public class GraficoTareaItem
        {
            public int id_tarea { get; set; }
            public string cod_tarea { get; set; }
            public string nombre_tarea { get; set; }
            public int cantidad { get; set; }
        }

        public class GraficoPersonaItem
        {
            public Guid? userid { get; set; }
            public string Apellido_nombre { get; set; }
            public int cantidad { get; set; }
        }

        public class TareaPersonaDetalle
        {
            public int Solicitud { get; set; }
            public string Tipo { get; set; }
            public string Tarea { get; set; }
            public string Circuito { get; set; }
            public DateTime Fecha { get; set; }
            public int Dias { get; set; }
            public string Usuario { get; set; }
            public int Observaciones { get; set; }
        }

        protected List<TareaPersonaDetalle> GetGraficoTareasDetalle(string cod_tarea_elegida)
        {
            using (var db = new DGHP_Entities())
            {
                int cod_solic_de = (int)Constants.ENG_Tareas.SSP_Solicitud_Habilitacion;
                string cod_solicitud_de = cod_solic_de.ToString().PadLeft(2, '0');
                int cod_corr_solic = (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud;
                var gerencias = (from g in db.ENG_Gerencias where g.id_gerencia == id_gerencia select g).ToList();

                bt_tramite_tituloControl.Text = gerencias.FirstOrDefault().Descripcion;

                //***********************************
                var q1 = (
                    from a in (
                        ((
                from tth in db.SGI_Tramites_Tareas_HAB
                join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                join c in db.ENG_Circuitos on t.id_circuito equals c.id_circuito
                join gc in db.ENG_Gerencias on c.id_gerencia equals gc.id_gerencia
                join tipot in db.TipoTramite on tth.SSIT_Solicitudes.id_tipotramite equals tipot.id_tipotramite
                join au in db.aspnet_Users on (Guid)tth.SGI_Tramites_Tareas.UsuarioAsignado_tramitetarea equals au.UserId into au_join
                from au in au_join.DefaultIfEmpty()
                join sshe in db.SSIT_Solicitudes_HistorialEstados
                      on new { tth.SSIT_Solicitudes.id_solicitud, cod_estado_nuevo = "OBSERVADO" }
                  equals new { sshe.id_solicitud, sshe.cod_estado_nuevo } into sshe_join
                from sshe in sshe_join.DefaultIfEmpty()
                where
                    gc.id_gerencia == id_gerencia &&
                  tt.FechaInicio_tramitetarea >= fechaDesde && tt.FechaInicio_tramitetarea <= fechaHasta
                  && tt.FechaCierre_tramitetarea == null
                  && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_corr_solic.ToString()
                  && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                  && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) == cod_tarea_elegida

                select new
                {
                    Id = tth.SSIT_Solicitudes.id_solicitud,
                    Tipo_Tramite = tipot.descripcion_tipotramite,
                    Tarea_Descripcion = tth.SGI_Tramites_Tareas.ENG_Tareas.nombre_tarea,
                    Grupo_Cir = tth.SGI_Tramites_Tareas.ENG_Tareas.ENG_Circuitos.nombre_grupo,
                    Fecha_Ini = ((System.DateTime?)tth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? tth.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                    Usuario = (au.UserName ?? "SIN ASIGNAR"),
                    Cant_Dias = (int)DbFunctions.DiffDays(((System.DateTime?)tth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? tth.SGI_Tramites_Tareas.FechaInicio_tramitetarea), DateTime.Now),
                    Obs = (sshe.cod_estado_nuevo ?? "") != "" ? 1 : 0
                }
                    ).Concat
                            (
                    from tth in db.SGI_Tramites_Tareas_TRANSF
                    join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                    join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                    join c in db.ENG_Circuitos on t.id_circuito equals c.id_circuito
                    join gc in db.ENG_Gerencias on c.id_gerencia equals gc.id_gerencia
                    join tipot in db.TipoTramite on tth.Transf_Solicitudes.id_tipotramite equals tipot.id_tipotramite
                    join au in db.aspnet_Users on (Guid)tth.SGI_Tramites_Tareas.UsuarioAsignado_tramitetarea equals au.UserId into au_join
                    from au in au_join.DefaultIfEmpty()
                    join sshe in db.Transf_Solicitudes_HistorialEstados
                          on new { tth.Transf_Solicitudes.id_solicitud, cod_estado_nuevo = "OBSERVADO" }
                      equals new { sshe.id_solicitud, sshe.cod_estado_nuevo } into sshe_join
                    from sshe in sshe_join.DefaultIfEmpty()
                    where
                        gc.id_gerencia == id_gerencia &&
                        tt.FechaInicio_tramitetarea >= fechaDesde && tt.FechaInicio_tramitetarea <= fechaHasta
                        && tt.FechaCierre_tramitetarea == null
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_corr_solic.ToString()
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) == cod_tarea_elegida

                    select new
                    {
                        Id = tth.Transf_Solicitudes.id_solicitud,
                        Tipo_Tramite = tipot.descripcion_tipotramite,
                        Tarea_Descripcion = tth.SGI_Tramites_Tareas.ENG_Tareas.nombre_tarea,
                        Grupo_Cir = tth.SGI_Tramites_Tareas.ENG_Tareas.ENG_Circuitos.nombre_grupo,
                        Fecha_Ini = ((System.DateTime?)tth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? tth.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                        Usuario = (au.UserName ?? "SIN ASIGNAR"),
                        Cant_Dias = (int)DbFunctions.DiffDays(((System.DateTime?)tth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? tth.SGI_Tramites_Tareas.FechaInicio_tramitetarea), DateTime.Now),
                        Obs = (sshe.cod_estado_nuevo ?? "") != "" ? 1 : 0
                    }
                    )))
                    group a by new
                    {
                        a.Id,
                        a.Tipo_Tramite,
                        a.Tarea_Descripcion,
                        a.Grupo_Cir,
                        a.Fecha_Ini,
                        a.Usuario,
                        a.Cant_Dias
                    } into g
                    select new TareaPersonaDetalle
                    {
                        Solicitud = (int)g.Key.Id,
                        Tipo = g.Key.Tipo_Tramite,
                        Tarea = g.Key.Tarea_Descripcion,
                        Circuito = g.Key.Grupo_Cir,
                        Fecha = (DateTime)g.Key.Fecha_Ini,
                        Dias = (int)g.Key.Cant_Dias,
                        Usuario = g.Key.Usuario,
                        Observaciones = (int)g.Sum(p => p.Obs)
                    });

                return q1.ToList();
            }
        }

        protected void CargarGraficoTareas()
        {
            try
            {
                db = new DGHP_Entities();

                int cod_solic_de = (int)Constants.ENG_Tareas.SSP_Solicitud_Habilitacion;
                string cod_solicitud_de = cod_solic_de.ToString().PadLeft(2, '0');
                int cod_corr_solic = (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud;
                var gerencias = (from g in db.ENG_Gerencias where g.id_gerencia == id_gerencia select g).ToList();

                bt_tramite_tituloControl.Text = gerencias.FirstOrDefault().Descripcion;

                //***********************************

                var Tareas_HAB = (
                    from tth in db.SGI_Tramites_Tareas_HAB
                    join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                    join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                    join c in db.ENG_Circuitos on t.id_circuito equals c.id_circuito
                    join gc in db.ENG_Gerencias on c.id_gerencia equals gc.id_gerencia
                    where
                        gc.id_gerencia == id_gerencia &&
                        tt.FechaInicio_tramitetarea >= fechaDesde && tt.FechaInicio_tramitetarea <= fechaHasta
                        && tt.FechaCierre_tramitetarea == null
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_corr_solic.ToString()
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de

                    group t by
                    new
                    {
                        t.id_tarea,
                        t.cod_tarea,
                        t.nombre_tarea,
                    } into g

                    select new GraficoTareaItem
                    {
                        id_tarea = g.Key.id_tarea,
                        cod_tarea = g.Key.cod_tarea.ToString().Substring(g.Key.cod_tarea.ToString().Length - 2, 2),
                        nombre_tarea = g.Key.nombre_tarea,
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
                        gc.id_gerencia == id_gerencia &&
                        tt.FechaInicio_tramitetarea >= fechaDesde && tt.FechaInicio_tramitetarea <= fechaHasta
                        && tt.FechaCierre_tramitetarea == null
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_corr_solic.ToString()
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de

                    group t by
                    new
                    {
                        t.id_tarea,
                        t.cod_tarea,
                        t.nombre_tarea,
                    } into g

                    select new GraficoTareaItem
                    {
                        id_tarea = g.Key.id_tarea,
                        cod_tarea = g.Key.cod_tarea.ToString().Substring(g.Key.cod_tarea.ToString().Length - 2, 2),
                        nombre_tarea = g.Key.nombre_tarea,
                        cantidad = g.Count()
                    }
                    );

                var todosLasTareasUnion = Tareas_HAB.Union(Tareas_TRANSF);

                var todosLasTareas = (from t in todosLasTareasUnion
                                      group t by new { t.cod_tarea, t.nombre_tarea } into g
                                      select new
                                      {
                                          cod_tarea = g.Key.cod_tarea,
                                          nombre_tarea = g.Key.nombre_tarea,
                                          cantidad = g.Select(x => x.cantidad).Sum(),
                                      });
                //***********************************

                var q = (
                        from tt in todosLasTareas
                        select new
                        {
                            id_gerencia = id_gerencia,
                            cod_tarea = tt.cod_tarea,
                            nombre_tarea = tt.nombre_tarea,
                            cantidad = tt.cantidad,
                            fechaDesde = fechaDesde,
                            fechaHasta = fechaHasta
                        });

                grdPorTareas.DataSource = q.ToList().OrderBy(x => x.cantidad);
                grdPorTareas.DataBind();

                if (q.ToList().Count > 0)
                {
                    //DivCanvasTareas.Style.Add("width", "0");
                    var listaCalif = (from c in q
                                      select new ChartData
                                      {
                                          label = c.nombre_tarea,
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

                    string data = string.Format("GraficarTareas('{0}','{1}','{2}');", datos, label, colorChart);
                    ScriptManager.RegisterStartupScript(pnlMain, pnlMain.GetType(), "GraficarTareas", data, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(pnlMain, pnlMain.GetType(), "clearChartTareas", "clearChartTareas()", true);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(udpError, udpError.GetType(), "showfrmError", "showfrmError();", true);
            }

        }

        protected List<TareaPersonaDetalle> GetGraficoPersonasDetalle(Guid? userid_elegido)
        {
            using (var db = new DGHP_Entities())
            {
                int cod_solic_de = (int)Constants.ENG_Tareas.SSP_Solicitud_Habilitacion;
                string cod_solicitud_de = cod_solic_de.ToString().PadLeft(2, '0');
                int cod_corr_solic = (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud;
                var gerencias = (from g in db.ENG_Gerencias where g.id_gerencia == id_gerencia select g).ToList();

                bt_tramite_tituloControl.Text = gerencias.FirstOrDefault().Descripcion;

                //***********************************
                var q1 = (
                    from a in (
                        ((
                from tth in db.SGI_Tramites_Tareas_HAB
                join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                join c in db.ENG_Circuitos on t.id_circuito equals c.id_circuito
                join gc in db.ENG_Gerencias on c.id_gerencia equals gc.id_gerencia
                join tipot in db.TipoTramite on tth.SSIT_Solicitudes.id_tipotramite equals tipot.id_tipotramite
                join au in db.aspnet_Users on (Guid)tth.SGI_Tramites_Tareas.UsuarioAsignado_tramitetarea equals au.UserId into au_join
                from au in au_join.DefaultIfEmpty()
                join sshe in db.SSIT_Solicitudes_HistorialEstados
                      on new { tth.SSIT_Solicitudes.id_solicitud, cod_estado_nuevo = "OBSERVADO" }
                  equals new { sshe.id_solicitud, sshe.cod_estado_nuevo } into sshe_join
                from sshe in sshe_join.DefaultIfEmpty()
                where
                    gc.id_gerencia == id_gerencia &&
                    tt.FechaInicio_tramitetarea >= fechaDesde && tt.FechaInicio_tramitetarea <= fechaHasta
                    && tt.FechaCierre_tramitetarea == null
                  && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_corr_solic.ToString()
                  && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                  && tt.aspnet_Users.SGI_Profiles.userid == userid_elegido

                select new
                {
                    Id = tth.SSIT_Solicitudes.id_solicitud,
                    Tipo_Tramite = tipot.descripcion_tipotramite,
                    Tarea_Descripcion = tth.SGI_Tramites_Tareas.ENG_Tareas.nombre_tarea,
                    Grupo_Cir = tth.SGI_Tramites_Tareas.ENG_Tareas.ENG_Circuitos.nombre_grupo,
                    Fecha_Ini = ((System.DateTime?)tth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? tth.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                    Usuario = (au.UserName ?? "SIN ASIGNAR"),
                    Cant_Dias = (int)DbFunctions.DiffDays(((System.DateTime?)tth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? tth.SGI_Tramites_Tareas.FechaInicio_tramitetarea), DateTime.Now),
                    Obs = (sshe.cod_estado_nuevo ?? "") != "" ? 1 : 0
                }
                    ).Concat
                            (
                    from tth in db.SGI_Tramites_Tareas_TRANSF
                    join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                    join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                    join c in db.ENG_Circuitos on t.id_circuito equals c.id_circuito
                    join gc in db.ENG_Gerencias on c.id_gerencia equals gc.id_gerencia
                    join tipot in db.TipoTramite on tth.Transf_Solicitudes.id_tipotramite equals tipot.id_tipotramite
                    join au in db.aspnet_Users on (Guid)tth.SGI_Tramites_Tareas.UsuarioAsignado_tramitetarea equals au.UserId into au_join
                    from au in au_join.DefaultIfEmpty()
                    join sshe in db.Transf_Solicitudes_HistorialEstados
                          on new { tth.Transf_Solicitudes.id_solicitud, cod_estado_nuevo = "OBSERVADO" }
                      equals new { sshe.id_solicitud, sshe.cod_estado_nuevo } into sshe_join
                    from sshe in sshe_join.DefaultIfEmpty()
                    where
                        gc.id_gerencia == id_gerencia &&
                        tt.FechaInicio_tramitetarea >= fechaDesde && tt.FechaInicio_tramitetarea <= fechaHasta
                        && tt.FechaCierre_tramitetarea == null
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_corr_solic.ToString()
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                      && tt.aspnet_Users.SGI_Profiles.userid == userid_elegido

                    select new
                    {
                        Id = tth.Transf_Solicitudes.id_solicitud,
                        Tipo_Tramite = tipot.descripcion_tipotramite,
                        Tarea_Descripcion = tth.SGI_Tramites_Tareas.ENG_Tareas.nombre_tarea,
                        Grupo_Cir = tth.SGI_Tramites_Tareas.ENG_Tareas.ENG_Circuitos.nombre_grupo,
                        Fecha_Ini = ((System.DateTime?)tth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? tth.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                        Usuario = (au.UserName ?? "SIN ASIGNAR"),
                        Cant_Dias = (int)DbFunctions.DiffDays(((System.DateTime?)tth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? tth.SGI_Tramites_Tareas.FechaInicio_tramitetarea), DateTime.Now),
                        Obs = (sshe.cod_estado_nuevo ?? "") != "" ? 1 : 0
                    }
                    )))
                    group a by new
                    {
                        a.Id,
                        a.Tipo_Tramite,
                        a.Tarea_Descripcion,
                        a.Grupo_Cir,
                        a.Fecha_Ini,
                        a.Usuario,
                        a.Cant_Dias
                    } into g
                    select new TareaPersonaDetalle
                    {
                        Solicitud = (int)g.Key.Id,
                        Tipo = g.Key.Tipo_Tramite,
                        Tarea = g.Key.Tarea_Descripcion,
                        Circuito = g.Key.Grupo_Cir,
                        Fecha = (DateTime)g.Key.Fecha_Ini,
                        Dias = (int)g.Key.Cant_Dias,
                        Usuario = g.Key.Usuario,
                        Observaciones = (int)g.Sum(p => p.Obs)
                    });

                return q1.ToList();
            }
        }

        protected void CargarGraficoPersonas()
        {
            try
            {
                int cod_solic_de = (int)Constants.ENG_Tareas.SSP_Solicitud_Habilitacion;
                string cod_solicitud_de = cod_solic_de.ToString().PadLeft(2, '0');
                int cod_corr_solic = (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud;
                db = new DGHP_Entities();
                ///////////////////////
                //var gerencias = (from g in db.ENG_Gerencias where g.id_gerencia == id_gerencia select g).ToList();

                //**************************************
                var Tareas_HAB = (
                    from tth in db.SGI_Tramites_Tareas_HAB
                    join tt in db.SGI_Tramites_Tareas on tth.id_tramitetarea equals tt.id_tramitetarea
                    join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                    join c in db.ENG_Circuitos on t.id_circuito equals c.id_circuito
                    join gc in db.ENG_Gerencias on c.id_gerencia equals gc.id_gerencia
                    //join u in db.SGI_Profiles.DefaultIfEmpty() on tt.UsuarioAsignado_tramitetarea equals u.userid
                    where
                      gc.id_gerencia == id_gerencia &&
                        tt.FechaInicio_tramitetarea >= fechaDesde && tt.FechaInicio_tramitetarea <= fechaHasta
                        && tt.FechaCierre_tramitetarea == null
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_corr_solic.ToString()
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                    group tt.aspnet_Users.SGI_Profiles by
                    new
                    {
                        tt.aspnet_Users.SGI_Profiles.userid,
                        Apellido_nombre = (tt.aspnet_Users.SGI_Profiles.Nombres + tt.aspnet_Users.SGI_Profiles.Apellido).Length > 0 ?
                        tt.aspnet_Users.SGI_Profiles.Nombres + " " + tt.aspnet_Users.SGI_Profiles.Apellido : "Sin Asignar",
                    } into g

                    select new GraficoPersonaItem
                    {
                        userid = g.Key.userid,
                        Apellido_nombre = g.Key.Apellido_nombre,
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
                    //join u in db.SGI_Profiles.DefaultIfEmpty() on tt.UsuarioAsignado_tramitetarea equals u.userid
                    where
                        gc.id_gerencia == id_gerencia &&
                        tt.FechaInicio_tramitetarea >= fechaDesde && tt.FechaInicio_tramitetarea <= fechaHasta
                        && tt.FechaCierre_tramitetarea == null
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_corr_solic.ToString()
                      && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                    group tt.UsuarioAsignado_tramitetarea by
                    new
                    {
                        tt.aspnet_Users.SGI_Profiles.userid,
                        Apellido_nombre = (tt.aspnet_Users.SGI_Profiles.Nombres + tt.aspnet_Users.SGI_Profiles.Apellido).Length > 0 ?
                        tt.aspnet_Users.SGI_Profiles.Nombres + " " + tt.aspnet_Users.SGI_Profiles.Apellido : "Sin Asignar",
                    } into g

                    select new GraficoPersonaItem
                    {
                        userid = g.Key.userid,
                        Apellido_nombre = g.Key.Apellido_nombre,
                        cantidad = g.Count()
                    }
                    );

                var todosLasTareasUnion = Tareas_HAB.Union(Tareas_TRANSF);

                var todosLasTareas = (from t in todosLasTareasUnion
                                      group t by new { t.userid, t.Apellido_nombre } into g
                                      select new
                                      {
                                          userid = g.Key.userid,
                                          Apellido_nombre = g.Key.Apellido_nombre,
                                          cantidad = g.Select(x => x.cantidad).Sum(),
                                      });
                //**************************************
                var q = (
                        from tt in todosLasTareas
                        select new
                        {
                            id_gerencia = id_gerencia,
                            userid = tt.userid,
                            Apellido_nombre = tt.Apellido_nombre,
                            cantidad = tt.cantidad,
                            fechaDesde = fechaDesde,
                            fechaHasta = fechaHasta
                        });

                grdPorPersonas.DataSource = q.ToList().OrderBy(x => x.cantidad);
                grdPorPersonas.DataBind();

                if (q.ToList().Count > 0)
                {
                    var listaCalif = (from c in q
                                      select new ChartData
                                      {
                                          label = c.Apellido_nombre,
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

                    string data = string.Format("GraficarPersonas('{0}','{1}','{2}');", datos, label, colorChart);
                    ScriptManager.RegisterStartupScript(pnlMain, pnlMain.GetType(), "GraficarPersonas", data, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(pnlMain, pnlMain.GetType(), "clearChartTareas", "clearChartTareas()", true);
                }
                ///////////////////////
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(udpError, udpError.GetType(), "showfrmError", "showfrmError();", true);
            }
        }

        protected void CargarGraficoTareasDetalle(string cod_tarea)
        {
            try
            {
                grdSolicitudes.DataSource = GetGraficoTareasDetalle(cod_tarea);
                grdSolicitudes.DataBind();

                ScriptManager.RegisterStartupScript(updDetalles, updDetalles.GetType(), "showDetalles", "showDetalles();", true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(udpError, udpError.GetType(), "showfrmError", "showfrmError();", true);
            }
        }

        protected void CargarGraficoPersonasDetalle(Guid? userid)
        {
            try
            {
                grdSolicitudes.DataSource = GetGraficoPersonasDetalle(userid);
                grdSolicitudes.DataBind();

                ScriptManager.RegisterStartupScript(updDetalles, updDetalles.GetType(), "showDetalles", "showDetalles();", true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(udpError, udpError.GetType(), "showfrmError", "showfrmError();", true);
            }
        }

        protected void DetalleGrafTarea_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                string[] Argumentos = btn.CommandArgument.ToString().Split(new char[] { ',' });
                string cod_tarea = Argumentos[0];
                string desc = Argumentos[1];

                hidTramites.Value = "tarea";
                hidIdTipo.Value = cod_tarea;
                hidDescTipo.Value = desc;
                updHids.Update();

                lblDescTipo.Text = desc;

                CargarParametros();
                CargarGraficoTareasDetalle(cod_tarea);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(udpError, udpError.GetType(), "showfrmError", "showfrmError();", true);
            }

        }

        protected void DetalleGrafPersona_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                string[] Argumentos = btn.CommandArgument.ToString().Split(new char[] { ',' });

                string userid = Argumentos[0];
                string desc = Argumentos[1];

                hidTramites.Value = "persona";
                hidIdTipo.Value = userid;
                hidDescTipo.Value = desc;
                updHids.Update();

                lblDescTipo.Text = desc;

                CargarParametros();
                if (userid != "")
                    CargarGraficoPersonasDetalle(Guid.Parse(userid));
                else
                    CargarGraficoPersonasDetalle(null);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(udpError, udpError.GetType(), "showfrmError", "showfrmError();", true);
            }

        }

        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;

                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }
        }

        protected void grdSolicitudes_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                var solicitudes = new List<TareaPersonaDetalle>();
                if (hidTramites.Value == "tarea")
                    solicitudes = GetGraficoTareasDetalle(hidIdTipo.Value);
                else
                {
                    if (hidIdTipo.Value != "")
                        solicitudes = GetGraficoPersonasDetalle(Guid.Parse(hidIdTipo.Value));
                    else
                        solicitudes = GetGraficoPersonasDetalle(null);
                }

                if (solicitudes != null)
                {
                    var param = Expression.Parameter(typeof(TareaPersonaDetalle), e.SortExpression);
                    var sortExpression = Expression.Lambda<Func<TareaPersonaDetalle, object>>(Expression.Convert(Expression.Property(param, e.SortExpression), typeof(object)), param);


                    if (GridViewSortDirection == SortDirection.Ascending)
                    {
                        grdSolicitudes.DataSource = solicitudes.AsQueryable<TareaPersonaDetalle>().OrderBy(sortExpression);
                        GridViewSortDirection = SortDirection.Descending;
                    }
                    else
                    {
                        grdSolicitudes.DataSource = solicitudes.AsQueryable<TareaPersonaDetalle>().OrderByDescending(sortExpression);
                        GridViewSortDirection = SortDirection.Ascending;
                    };


                    grdSolicitudes.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(udpError, udpError.GetType(), "showfrmError", "showfrmError();", true);
            }
        }

        protected void grdPorPersonas_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int tot = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "cantidad"));
                sumFooterPersonas += tot;
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = "<div align='center'><strong>Total:<s/trong></div>";
                Label lbl = (Label)e.Row.FindControl("lblTotalPersonas");
                lbl.Text = sumFooterPersonas.ToString();
            }

        }


        protected void grdPorTareas_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int tot = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "cantidad"));
                sumFooterTareas += tot;
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = "<div align='center'><strong>Total:<s/trong></div>";
                Label lbl = (Label)e.Row.FindControl("lbltotalTareas");
                lbl.Text = sumFooterTareas.ToString();
            }

        }
    }
}