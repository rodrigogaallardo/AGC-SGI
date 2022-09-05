using SGI.Model;
using SGI.StaticClassNameSpace;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Dashboard
{
    public partial class SeguimientoTramitesTipos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var uriAnt = Request.UrlReferrer;
                hidUrlAnteriror.Value = uriAnt.AbsoluteUri ?? "~/SeguimientoTramites.aspx";

                string trm = HttpUtility.UrlDecode(Request.QueryString["trm"]);
                if (trm.Length > 0)
                {
                    byte[] data = System.Convert.FromBase64String(trm);
                    string tramites = HttpUtility.HtmlDecode(System.Text.ASCIIEncoding.ASCII.GetString(data));

                    lblTramites.Text = tramites.Replace("Administraci?n", "Administración");
                    hidTramites.Value = tramites.Replace("Administraci?n", "Administración");
                    LoadData(tramites);
                }
                else
                {
                    throw new Exception("error en los parametros");
                }
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect(hidUrlAnteriror.Value);
        }

        private void LoadData(string tramites)
        {
            try
            {
                CargarTramites(tramites);
                ScriptManager.RegisterStartupScript(updDetalles, updDetalles.GetType(), "hideDetalles", "hideDetalles();", true);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "showfrmError", "showfrmError();", true);
            }
        }

        public class TramitesTipos
        {
            public int id_tipotramite { get; set; }
            public string Descripcion { get; set; }
            public int Cantidad { get; set; }
        }

        public class TramitesTiposDetalle
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

        public List<TramitesTipos> GetTramites(string tramites)
        {
            using (var ctx = new DGHP_Entities())
            {
                int cod_solic_de = (int)Constants.ENG_Tareas.SSP_Solicitud_Habilitacion;
                string cod_solicitud_de = cod_solic_de.ToString().PadLeft(2, '0');
                var q = new List<TramitesTipos>();
                if (hidTramites.Value.Contains("Administración"))
                {
                    var q1 = (
                        from a in (
                                ((
                                    from stth in ctx.SGI_Tramites_Tareas_HAB
                                    where
                                      stth.SGI_Tramites_Tareas.FechaCierre_tramitetarea == null &&
                                      (stth.SSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.En_trámite
                                    || stth.SSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.RevCaducidad
                                    || stth.SSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Rechazada)
                                    && stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                                    select new
                                    {
                                        Id_tipotramite = stth.SSIT_Solicitudes.TipoTramite.id_tipotramite,
                                        Descripcion_tipotramite = stth.SSIT_Solicitudes.TipoTramite.descripcion_tipotramite,
                                        Id = stth.SSIT_Solicitudes.id_solicitud
                                    }
                                ).Union
                                (
                                    from stth in ctx.SGI_Tramites_Tareas_TRANSF
                                    where
                                      stth.SGI_Tramites_Tareas.FechaCierre_tramitetarea == null &&
                                      (stth.Transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.En_trámite
                                    || stth.Transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.RevCaducidad
                                    || stth.Transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Rechazada)
                                    && stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                                    select new
                                    {
                                        Id_tipotramite = stth.Transf_Solicitudes.TipoTramite.id_tipotramite,
                                        Descripcion_tipotramite = stth.Transf_Solicitudes.TipoTramite.descripcion_tipotramite,
                                        Id = stth.Transf_Solicitudes.id_solicitud
                                    }
                                )))
                        group a by new
                        {
                            a.Id_tipotramite,
                            a.Descripcion_tipotramite
                        } into g
                        select new TramitesTipos()
                        {
                            id_tipotramite = g.Key.Id_tipotramite,
                            Descripcion = g.Key.Descripcion_tipotramite,
                            Cantidad = (int)g.Count()
                        });

                    q = q1.ToList();
                }
                else
                {
                    int cod_corr_solic = (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud;
                    var q1 = (
                        from a in (
                            ((
                                from stth in ctx.SGI_Tramites_Tareas_HAB
                                where
                                  stth.SGI_Tramites_Tareas.FechaCierre_tramitetarea == null &&
                                  stth.SSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Observado &&
                                  stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) == cod_corr_solic.ToString()
                                select new
                                {
                                    Id_tipotramite = stth.SSIT_Solicitudes.TipoTramite.id_tipotramite,
                                    Descripcion_tipotramite = stth.SSIT_Solicitudes.TipoTramite.descripcion_tipotramite,
                                    Id = stth.SSIT_Solicitudes.id_solicitud
                                }
                            ).Union
                            (
                                from stth in ctx.SGI_Tramites_Tareas_TRANSF
                                where
                                  stth.SGI_Tramites_Tareas.FechaCierre_tramitetarea == null &&
                                  stth.Transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Observado &&
                                  stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) == cod_corr_solic.ToString()
                                select new
                                {
                                    Id_tipotramite = stth.Transf_Solicitudes.TipoTramite.id_tipotramite,
                                    Descripcion_tipotramite = stth.Transf_Solicitudes.TipoTramite.descripcion_tipotramite,
                                    Id = stth.Transf_Solicitudes.id_solicitud
                                }
                            )))
                        group a by new
                        {
                            a.Id_tipotramite,
                            a.Descripcion_tipotramite
                        } into g
                        select new TramitesTipos()
                        {
                            id_tipotramite = g.Key.Id_tipotramite,
                            Descripcion = g.Key.Descripcion_tipotramite,
                            Cantidad = (int)g.Count()
                        });

                    q = q1.ToList();
                }

                return q;
            }
        }

        private void Graficar(List<TramitesTipos> tramitesTipos)
        {
            if (tramitesTipos.Count > 0)
            {
                var l = (from c in tramitesTipos
                         select new ChartData
                         {
                             label = (c.id_tipotramite.ToString() + " - " + c.Descripcion),
                             valor = (int)c.Cantidad
                         }).ToList();


                var result = l.Select(x => x.valor).ToArray();
                string datos = String.Join(",", result);
                string label = String.Join(",", l.Select(x => x.label).ToArray());
                string[] colors = new string[result.Count()];

                for (int i = 0; i < result.Count(); i++)
                {
                    colors[i] = String.Format("{0:X6}", ChartColor.Colores[i]);
                }

                string colorChart = String.Join(",", colors);

                string script = string.Format("Graficar('{0}','{1}','{2}');", datos, label, colorChart);

                ScriptManager.RegisterStartupScript(updResultado, updResultado.GetType(), "Graficar", script, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(updResultado, updResultado.GetType(), "clearChart", "clearChart();", true);
            }
        }

        private void CargarTramites(string tramites)
        {
            try
            {
                var q = GetTramites(tramites);

                grdTramites.DataSource = q;
                grdTramites.DataBind();

                // Armamos el gráfico
                Graficar(q);

                ScriptManager.RegisterStartupScript(updResultado, updResultado.GetType(), "showResultados", "showResultados();", true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(updfrmerror, updfrmerror.GetType(), "showfrmError", "showfrmError();", true);
            }
        }

        private List<TramitesTiposDetalle> GetTramitesDetalle(string tramites, int id_tipotramite)
        {
            using (var ctx = new DGHP_Entities())
            {
                int cod_solic_de = (int)Constants.ENG_Tareas.SSP_Solicitud_Habilitacion;
                string cod_solicitud_de = cod_solic_de.ToString().PadLeft(2, '0');
                var q = new List<TramitesTiposDetalle>();
                if (hidTramites.Value.Contains("Administración"))
                {
                    var q1 = (
                        from a in (
                            ((
                                from stth in ctx.SGI_Tramites_Tareas_HAB
                                join tt in ctx.TipoTramite on stth.SSIT_Solicitudes.id_tipotramite equals tt.id_tipotramite
                                join au in ctx.aspnet_Users on (Guid)stth.SGI_Tramites_Tareas.UsuarioAsignado_tramitetarea equals au.UserId into au_join
                                from au in au_join.DefaultIfEmpty()
                                join sshe in ctx.SSIT_Solicitudes_HistorialEstados
                                      on new { stth.SSIT_Solicitudes.id_solicitud, cod_estado_nuevo = "OBSERVADO" }
                                  equals new { sshe.id_solicitud, sshe.cod_estado_nuevo } into sshe_join
                                from sshe in sshe_join.DefaultIfEmpty()
                                where
                                  stth.SGI_Tramites_Tareas.FechaCierre_tramitetarea == null &&
                                (stth.SSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.En_trámite
                                || stth.SSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.RevCaducidad
                                || stth.SSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Rechazada) &&
                                  stth.SSIT_Solicitudes.id_tipotramite == id_tipotramite
                                && stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                                select new
                                {
                                    Id = stth.SSIT_Solicitudes.id_solicitud,
                                    Tipo_Tramite = tt.descripcion_tipotramite,
                                    Tarea_Descripcion = stth.SGI_Tramites_Tareas.ENG_Tareas.nombre_tarea,
                                    Grupo_Cir = stth.SGI_Tramites_Tareas.ENG_Tareas.ENG_Circuitos.nombre_grupo,
                                    Fecha_Ini = ((System.DateTime?)stth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? stth.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                    Usuario = (au.UserName ?? "SIN ASIGNAR"),
                                    Cant_Dias = (int)DbFunctions.DiffDays(((System.DateTime?)stth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? stth.SGI_Tramites_Tareas.FechaInicio_tramitetarea), DateTime.Now),
                                    Obs = (sshe.cod_estado_nuevo ?? "") != "" ? 1 : 0
                                }
                            ).Concat
                            (
                                from stth in ctx.SGI_Tramites_Tareas_TRANSF
                                join tt in ctx.TipoTramite on stth.Transf_Solicitudes.id_tipotramite equals tt.id_tipotramite
                                join au in ctx.aspnet_Users on (Guid)stth.SGI_Tramites_Tareas.UsuarioAsignado_tramitetarea equals au.UserId into au_join
                                from au in au_join.DefaultIfEmpty()
                                join sshe in ctx.Transf_Solicitudes_HistorialEstados
                                      on new { stth.Transf_Solicitudes.id_solicitud, cod_estado_nuevo = "OBSERVADO" }
                                  equals new { sshe.id_solicitud, sshe.cod_estado_nuevo } into sshe_join
                                from sshe in sshe_join.DefaultIfEmpty()
                                where
                                  stth.SGI_Tramites_Tareas.FechaCierre_tramitetarea == null &&
                                (stth.Transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.En_trámite
                                || stth.Transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.RevCaducidad
                                || stth.Transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Rechazada) &&
                                  stth.Transf_Solicitudes.id_tipotramite == id_tipotramite
                                && stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) != cod_solicitud_de
                                select new
                                {
                                    Id = stth.Transf_Solicitudes.id_solicitud,
                                    Tipo_Tramite = tt.descripcion_tipotramite,
                                    Tarea_Descripcion = stth.SGI_Tramites_Tareas.ENG_Tareas.nombre_tarea,
                                    Grupo_Cir = stth.SGI_Tramites_Tareas.ENG_Tareas.ENG_Circuitos.nombre_grupo,
                                    Fecha_Ini = ((System.DateTime?)stth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? stth.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                    Usuario = (au.UserName ?? "SIN ASIGNAR"),
                                    Cant_Dias = (int)DbFunctions.DiffDays(((System.DateTime?)stth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? stth.SGI_Tramites_Tareas.FechaInicio_tramitetarea), DateTime.Now),
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
                        select new TramitesTiposDetalle
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

                    q = q1.ToList();
                }
                else
                {
                    int cod_corr_solic = (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud;
                    var q1 = (
                        from a in (
                            ((
                                from stth in ctx.SGI_Tramites_Tareas_HAB
                                join tt in ctx.TipoTramite on stth.SSIT_Solicitudes.id_tipotramite equals tt.id_tipotramite
                                join sshe in ctx.SSIT_Solicitudes_HistorialEstados
                                      on new { stth.SSIT_Solicitudes.id_solicitud, cod_estado_nuevo = "OBSERVADO" }
                                  equals new { sshe.id_solicitud, sshe.cod_estado_nuevo } into sshe_join
                                from sshe in sshe_join.DefaultIfEmpty()
                                where
                                  stth.SGI_Tramites_Tareas.FechaCierre_tramitetarea == null &&
                                  stth.SSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Observado &&
                                  stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) == cod_corr_solic.ToString() &&
                                  stth.SSIT_Solicitudes.id_tipotramite == id_tipotramite
                                select new
                                {
                                    Id = stth.SSIT_Solicitudes.id_solicitud,
                                    Tipo_Tramite = tt.descripcion_tipotramite,
                                    Tarea_Descripcion = stth.SGI_Tramites_Tareas.ENG_Tareas.nombre_tarea,
                                    Grupo_Cir = stth.SGI_Tramites_Tareas.ENG_Tareas.ENG_Circuitos.nombre_grupo,
                                    Fecha_Ini = ((System.DateTime?)stth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? stth.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                    Usuario = "",
                                    Cant_Dias = (int)DbFunctions.DiffDays(((System.DateTime?)stth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? stth.SGI_Tramites_Tareas.FechaInicio_tramitetarea), DateTime.Now),
                                    Obs = (sshe.cod_estado_nuevo ?? "") != "" ? 1 : 0
                                }
                            ).Concat
                            (
                                from stth in ctx.SGI_Tramites_Tareas_TRANSF
                                join tt in ctx.TipoTramite on stth.Transf_Solicitudes.id_tipotramite equals tt.id_tipotramite
                                join sshe in ctx.Transf_Solicitudes_HistorialEstados
                                      on new { stth.Transf_Solicitudes.id_solicitud, cod_estado_nuevo = "OBSERVADO" }
                                  equals new { sshe.id_solicitud, sshe.cod_estado_nuevo } into sshe_join
                                from sshe in sshe_join.DefaultIfEmpty()
                                where
                                  stth.SGI_Tramites_Tareas.FechaCierre_tramitetarea == null &&
                                stth.Transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Observado &&
                                stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Substring(stth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString().Length - 2, 2) == cod_corr_solic.ToString() &&
                                  stth.Transf_Solicitudes.id_tipotramite == id_tipotramite
                                select new
                                {
                                    Id = stth.Transf_Solicitudes.id_solicitud,
                                    Tipo_Tramite = tt.descripcion_tipotramite,
                                    Tarea_Descripcion = stth.SGI_Tramites_Tareas.ENG_Tareas.nombre_tarea,
                                    Grupo_Cir = stth.SGI_Tramites_Tareas.ENG_Tareas.ENG_Circuitos.nombre_grupo,
                                    Fecha_Ini = ((System.DateTime?)stth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? stth.SGI_Tramites_Tareas.FechaInicio_tramitetarea),
                                    Usuario = "",
                                    Cant_Dias = (int)DbFunctions.DiffDays(((System.DateTime?)stth.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea ?? stth.SGI_Tramites_Tareas.FechaInicio_tramitetarea), DateTime.Now),
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
                        select new TramitesTiposDetalle
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

                    q = q1.ToList();
                }

                return q;
            }
        }

        private void CargarTramitesDetalle(string tramites, int id_tipotramite)
        {
            try
            {
                grdSolicitudes.DataSource = GetTramitesDetalle(tramites, id_tipotramite);
                grdSolicitudes.DataBind();

                ScriptManager.RegisterStartupScript(updDetalles, updDetalles.GetType(), "showDetalles", "showDetalles();", true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(updfrmerror, updfrmerror.GetType(), "showfrmError", "showfrmError();", true);
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

        protected void btnDetalle_Click(object sender, EventArgs e)
        {
            string id_tipo = hidIdTipo.Value;
            string desc = hidDescTipo.Value;
            try
            {
                int id_t = int.Parse(id_tipo);
                string trm = hidTramites.Value;

                lblDescTipo.Text = desc;

                CargarTramitesDetalle(trm, id_t);
                CargarTramites(trm);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(updfrmerror, updfrmerror.GetType(), "showfrmError", "showfrmError();", true);
            }
        }

        protected void DetalleGraf_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                string[] Argumentos = btn.CommandArgument.ToString().Split(new char[] { ',' });
                string id_tipo = Argumentos[0];
                string desc = Argumentos[1];

                hidIdTipo.Value = id_tipo;
                hidDescTipo.Value = desc;
                updHids.Update();

                int id_t = int.Parse(id_tipo);
                string trm = hidTramites.Value;

                lblDescTipo.Text = desc;

                CargarTramitesDetalle(trm, id_t);
                CargarTramites(trm);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(updfrmerror, updfrmerror.GetType(), "showfrmError", "showfrmError();", true);
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
                var solicitudes = GetTramitesDetalle(hidTramites.Value, int.Parse(hidIdTipo.Value));

                if (solicitudes != null)
                {
                    var param = Expression.Parameter(typeof(TramitesTiposDetalle), e.SortExpression);
                    var sortExpression = Expression.Lambda<Func<TramitesTiposDetalle, object>>(Expression.Convert(Expression.Property(param, e.SortExpression), typeof(object)), param);


                    if (GridViewSortDirection == SortDirection.Ascending)
                    {
                        grdSolicitudes.DataSource = solicitudes.AsQueryable<TramitesTiposDetalle>().OrderBy(sortExpression);
                        GridViewSortDirection = SortDirection.Descending;
                    }
                    else
                    {
                        grdSolicitudes.DataSource = solicitudes.AsQueryable<TramitesTiposDetalle>().OrderByDescending(sortExpression);
                        GridViewSortDirection = SortDirection.Ascending;
                    };


                    grdSolicitudes.DataBind();
                }

                CargarTramites(hidTramites.Value);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(updfrmerror, updfrmerror.GetType(), "showfrmError", "showfrmError();", true);
            }
        }


        #region "Exporta a Excel"

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {

            btnCerrarExportacion.Visible = false;
            // genera un nombre de archivo aleatorio
            Random random = new Random((int)DateTime.Now.Ticks);
            int NroAleatorio = random.Next(0, 100);
            NroAleatorio = NroAleatorio * random.Next(0, 100);
            string fileName = string.Format("Grilla-Solicitudes-{0}.xls", NroAleatorio);

            pnlDescargarExcel.Style["display"] = "none";
            pnlExportandoExcel.Style["display"] = "block";

            Session["exportacion_en_proceso"] = true;
            Session["progress_data"] = "Preparando exportación.";
            Session["filename_exportacion"] = fileName;

            lblRegistrosExportados.Text = "Preparando exportación.";
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ExportarGrillaAExcel));
            thread.Start();

            Timer1.Enabled = true;
        }

        private void ExportarGrillaAExcel()
        {

            decimal cant_registros_x_vez = 0m;
            int totalRowCount = 0;
            int startRowIndex = 0;

            try
            {
                string id_tipo = hidIdTipo.Value;
                string desc = hidDescTipo.Value;

                int id_t = int.Parse(id_tipo);
                string trm = hidTramites.Value;

                lblDescTipo.Text = desc;
                
                var q = GetTramitesDetalle(trm, id_t);

                totalRowCount = q.Count;

                // Esto se realiza para saber el total y de a cuanto se va mostrar el progreso.
                if (totalRowCount < 10000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;

                int cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);

                int maximumRows = Convert.ToInt32(cant_registros_x_vez);

                var resultados = new List<TramitesTiposDetalle>();

                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados.AddRange(q.OrderBy(o => o.Solicitud).Skip(startRowIndex).Take(maximumRows).ToList());

                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados.Count);

                DataTable dt = Functions.ToDataTable(resultados);
                
                // Convierte la lista en un dataset
                DataSet ds = new DataSet();
                dt.TableName = "Solicitudes";
                ds.Tables.Add(dt);

                string savedFileName = Constants.Path_Temporal + Session["filename_exportacion"].ToString();

                Functions.EliminarArchivosDirectorioTemporal();

                // Utiliza DocumentFormat.OpenXml para exportar a excel
                Functions.ExportDataSetToExcel(ds, savedFileName);

                // quita la variable de session.
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
            }
            catch (Exception ex)
            {
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");

                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(updfrmerror, updfrmerror.GetType(), "showfrmError", "showfrmError();", true);
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                bool exportacion_en_proceso = (Session["exportacion_en_proceso"] != null ? (bool)Session["exportacion_en_proceso"] : false);

                if (exportacion_en_proceso)
                    lblRegistrosExportados.Text = Convert.ToString(Session["progress_data"]);
                else
                {
                    Timer1.Enabled = false;
                    btnCerrarExportacion.Visible = true;
                    pnlDescargarExcel.Style["display"] = "block";
                    pnlExportandoExcel.Style["display"] = "none";
                    string filename = Session["filename_exportacion"].ToString();
                    filename = HttpUtility.UrlEncode(filename);
                    btnDescargarExcel.NavigateUrl = string.Format("~/Controls/DescargarArchivoTemporal.aspx?fname={0}", filename);
                    Session.Remove("filename_exportacion");
                }
            }
            catch
            {
                //Timer1.Enabled = false;
            }

        }

        protected void btnCerrarExportacion_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            Session.Remove("filename_exportacion");
            Session.Remove("progress_data");
            Session.Remove("exportacion_en_proceso");
            pnlDescargarExcel.Style["display"] = "none";
            pnlExportandoExcel.Style["display"] = "block";

            ScriptManager.RegisterStartupScript(updExportaExcel, updExportaExcel.GetType(), "hidefrmExportarExcel", "hidefrmExportarExcel();", true);

            try
            {
                CargarTramites(hidTramites.Value);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ScriptManager.RegisterStartupScript(updfrmerror, updfrmerror.GetType(), "showfrmError", "showfrmError();", true);
            }
        }
        #endregion

    }
}