using SGI.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public partial class IndicadoresKPI : BasePage
    {
        private int id_tarea_origen = 0;
        private int id_tarea_fin = 0;
        private int id_tipoTramite = 0;
        private bool isTareaOrigenPrimera;
        private bool isTareaOrigenFechaInicio;
        private bool isTareaFinPrimera;
        private bool isTareaFinFechaInicio;
        private bool isDiasHabiles;
        private class cls_ultima_tarea
        {
            public int id_solicitud { get; set; }
            public int id_tramitetarea { get; set; }
            public DateTime? fecha_inicio { get; set; }
        }

        #region cargar inicial
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_rubros, updPnlFiltroBuscar_rubros.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updResultados, updResultados.GetType(), "init_Js_updResultados", "init_Js_updResultados();", true);
            }

            if (!IsPostBack)
            {
                CargarCombo();
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

        protected void ddlTipoTramite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlTipoTramite.SelectedValue) == (int)Constants.TipoDeTramite.Consulta_Padron)
            {
                txtFechaCrfdDesde.Text = "";
                txtFechaCrfdHasta.Text = "";
                pnlrfd.Visible = false;
                txtFechaRdgDesde.Text = "";
                txtFechaRdgHasta.Text = "";
                pnlRdg.Visible = false;
            }
            else
            {
                pnlrfd.Visible = true;
                pnlRdg.Visible = true;
            }
        }

        private void CargarCombo()
        {
            IniciarEntity();

            CargarCombo_TipoTramite();
            int idAux = 0;
            int.TryParse(ddlTipoTramite.SelectedItem.Value, out idAux);

            CargarCombo_Circuitos();
            CargarCombo_tareas();


            FinalizarEntity();
        }

        private void CargarCombo_TipoTramite()
        {
            List<string> lstOcultarTipoTramite = new List<string>();
            lstOcultarTipoTramite.Add("LIGUE");
            lstOcultarTipoTramite.Add("DESLIGUE");
            lstOcultarTipoTramite.Add("AMPLIACION/UNIFICACION");
            lstOcultarTipoTramite.Add("RECTIF_HABILITACION");
            lstOcultarTipoTramite.Add("REDISTRIBUCION_USO");
           // lstOcultarTipoTramite.Add("TRANSFERENCIA");
          //  lstOcultarTipoTramite.Add("CONSULTA_PADRON");
            List<TipoTramite> list_tipoTramite = this.db.TipoTramite.Where(x => !lstOcultarTipoTramite.Contains(x.cod_tipotramite)).ToList();
            ddlTipoTramite.DataTextField = "descripcion_tipotramite";
            ddlTipoTramite.DataValueField = "id_tipotramite";
            ddlTipoTramite.DataSource = list_tipoTramite;
            ddlTipoTramite.DataBind();
        }

        private void CargarCombo_tareas()
        {
            List<ENG_Tipos_Tareas> lista = new List<ENG_Tipos_Tareas>();

            ENG_Tipos_Tareas tarea = new ENG_Tipos_Tareas();
            tarea.id_tipo_tarea = 0;
            tarea.nombre = "Todas";
            lista.Add(tarea);

            var qTareas = db.ENG_Tipos_Tareas.ToList();

            lista.AddRange(qTareas);

            ddlTareaFin.DataTextField = "nombre";
            ddlTareaFin.DataValueField = "id_tipo_tarea";
            ddlTareaFin.DataSource = lista;
            ddlTareaFin.DataBind();

            ddlTareaOrigen.DataTextField = "nombre";
            ddlTareaOrigen.DataValueField = "id_tipo_tarea";
            ddlTareaOrigen.DataSource = lista;
            ddlTareaOrigen.DataBind();
        }

        private void CargarCombo_Circuitos()
        {
            List<ENG_Circuitos> lista = new List<ENG_Circuitos>();

            ENG_Circuitos tarea = new ENG_Circuitos();
            tarea.id_circuito = 0;
            tarea.nombre_circuito = "Todos";
            lista.Add(tarea);

            var qCircuitos =
                    (from c in this.db.ENG_Circuitos
                    orderby c.id_circuito
                    select new
                    {
                        c.id_circuito,
                        nombre = c.cod_circuito + " - " + c.nombre_circuito
                    }).ToList().Distinct();

            foreach (var item in qCircuitos)
            {
                tarea = new ENG_Circuitos();
                tarea.id_circuito = item.id_circuito;
                tarea.nombre_circuito = item.nombre;
                lista.Add(tarea);
            }

            ddlCircuito.DataTextField = "nombre_circuito";
            ddlCircuito.DataValueField = "id_circuito";
            ddlCircuito.DataSource = lista;
            ddlCircuito.DataBind();
        }
        #endregion

        public List<clsItemIndicadoresExcel> grdResultados_GetData(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {
            totalRowCount = 0;
            List<clsItemIndicadoresExcel> lstResult = null;
            List<clsItemIndicadoresExcel> lstResultAux = null;

            var lst_solicitudes = filtrar();
            if (lst_solicitudes != null)
            {
                totalRowCount = lst_solicitudes.Count();
                lstResult = GetData(startRowIndex, maximumRows, lst_solicitudes);
            }

            pnlCantidadRegistros.Visible = true;
            if (totalRowCount > 0)
            {
                lstResultAux = GetData(0, lst_solicitudes.Count, lst_solicitudes);
                lblCantidadRegistros.Text = string.Format("{0} Trámites", totalRowCount);
                int dias_totales = lstResultAux.Sum(x => x.dias_totales); //getDiasTotales(lst_solicitudes);
                decimal pro =Convert.ToDecimal(dias_totales) / Convert.ToDecimal(totalRowCount);
                lbl_diastotales.Text = pro.ToString("F");

                int dias = lstResultAux.Sum(x => x.dias_contribuyente);//getDiasTareasExcluidas(lst_solicitudes);
                pro = Convert.ToDecimal(dias_totales - dias) / Convert.ToDecimal(totalRowCount);
                lbl_diastotal_sincorreccion.Text = pro.ToString("F");
                //dias = lstResultAux.Sum(x => x.dias_dictamen + x.dias_avh);//getDiasTiemposMuertos(lst_solicitudes);
                //pro = Convert.ToDecimal(dias_totales - dias) / Convert.ToDecimal(totalRowCount);
                dias = lstResultAux.Sum(x => x.dias_dghyp);//getDiasTiemposMuertos(lst_solicitudes);
                pro = Convert.ToDecimal(dias) / Convert.ToDecimal(totalRowCount);
                lbl_diasDghyp.Text = pro.ToString("F");
                //dias = lstResultAux.Sum(x => x.dias_avh + x.dias_dictamen + x.dias_contribuyente); //getDiasDGHYPcorreccion(lst_solicitudes);
                //pro =  Convert.ToDecimal(dias_totales - dias) / Convert.ToDecimal(totalRowCount);
                dias = lstResultAux.Sum(x => x.dias_dghyp - x.dias_contribuyente); //getDiasDGHYPcorreccion(lst_solicitudes);
                pro = Convert.ToDecimal(dias) / Convert.ToDecimal(totalRowCount);
                lbl_diasDghy_sincorreccion.Text = pro.ToString("F");
                //dias = lstResultAux.Sum(x => x.dias_avh + x.dias_dictamen + x.dias_contribuyente + x.tiempo_muerto);//getDiasTiemposMuertosyCoreccion(lst_solicitudes);
                //pro = Convert.ToDecimal(dias_totales - dias) / Convert.ToDecimal(totalRowCount);
                dias = lstResultAux.Sum(x => x.dias_dghyp - x.dias_contribuyente - x.tiempo_muerto);//getDiasTiemposMuertosyCoreccion(lst_solicitudes);
                pro = Convert.ToDecimal(dias) / Convert.ToDecimal(totalRowCount);
                lbl_diasDghyp_sintiemposmuertos_ni_correcciones.Text = pro.ToString("F");

            }
            else
            {
                pnlCantidadRegistros.Visible = false;
            }
            //pnlResultadoBuscar.Visible = true;
            //updPnlResultadoBuscar.Update();

            return lstResult;
        }

        private IQueryable<cls_ultima_tarea> getListPrimeraTarea(List<int> lst_solicitudes)
        {
            IQueryable<cls_ultima_tarea> lst_Primera_tarea = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                          join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                          where ta.id_tipo_tarea == id_tarea_origen
                                                          && lst_solicitudes.Contains(tt_hab.id_solicitud)
                                                          group tt_hab by tt_hab.id_solicitud into g
                                                          select new cls_ultima_tarea
                                                          {
                                                              id_solicitud = g.Key,
                                                              id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                              fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                          }).Union(from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                                                   join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                                   where ta.id_tipo_tarea == id_tarea_origen
                                                                   && lst_solicitudes.Contains(tt_tr.id_solicitud)
                                                                   group tt_tr by tt_tr.id_solicitud into g
                                                                   select new cls_ultima_tarea
                                                                   {
                                                                       id_solicitud = g.Key,
                                                                       id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                                       fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                                   }).Union(from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                                                            join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                                            where ta.id_tipo_tarea == id_tarea_origen
                                                                            && lst_solicitudes.Contains(tt_cp.id_cpadron)
                                                                            group tt_cp by tt_cp.id_cpadron into g
                                                                            select new cls_ultima_tarea
                                                                            {
                                                                                id_solicitud = g.Key,
                                                                                id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                                                fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                                            });
            if (!isTareaOrigenPrimera)
                lst_Primera_tarea = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                  join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                  where ta.id_tipo_tarea == id_tarea_origen
                                  && lst_solicitudes.Contains(tt_hab.id_solicitud)
                                  group tt_hab by tt_hab.id_solicitud into g
                                  select new cls_ultima_tarea
                                  {
                                      id_solicitud = g.Key,
                                      id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                      fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  }).Union(from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                           join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                           where ta.id_tipo_tarea == id_tarea_origen
                                           && lst_solicitudes.Contains(tt_tr.id_solicitud)
                                           group tt_tr by tt_tr.id_solicitud into g
                                           select new cls_ultima_tarea
                                           {
                                               id_solicitud = g.Key,
                                               id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                               fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                           }).Union(from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                                    join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                    where ta.id_tipo_tarea == id_tarea_origen
                                                    && lst_solicitudes.Contains(tt_cp.id_cpadron)
                                                    group tt_cp by tt_cp.id_cpadron into g
                                                    select new cls_ultima_tarea
                                                    {
                                                        id_solicitud = g.Key,
                                                        id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                                        fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                    });
            return lst_Primera_tarea;
        }

        private IQueryable<cls_ultima_tarea> getListFinTramite(List<int> lst_solicitudes)
        {
            IQueryable<cls_ultima_tarea> lst_fin_tramite = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                        join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                        where ta.id_tipo_tarea == id_tarea_fin
                                                        && lst_solicitudes.Contains(tt_hab.id_solicitud)
                                                        group tt_hab by tt_hab.id_solicitud into g
                                                        select new cls_ultima_tarea
                                                        {
                                                            id_solicitud = g.Key,
                                                            id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                            fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                        }).Union(from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                                                 join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                                 where ta.id_tipo_tarea == id_tarea_fin
                                                                 && lst_solicitudes.Contains(tt_tr.id_solicitud)
                                                                 group tt_tr by tt_tr.id_solicitud into g
                                                                 select new cls_ultima_tarea
                                                                 {
                                                                     id_solicitud = g.Key,
                                                                     id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                                     fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                                 }).Union(from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                                                          join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                                          where ta.id_tipo_tarea == id_tarea_fin
                                                                          && lst_solicitudes.Contains(tt_cp.id_cpadron)
                                                                          group tt_cp by tt_cp.id_cpadron into g
                                                                          select new cls_ultima_tarea
                                                                          {
                                                                              id_solicitud = g.Key,
                                                                              id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                                              fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                                          });
            if (!isTareaFinPrimera)
                lst_fin_tramite = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                           join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                           where ta.id_tipo_tarea == id_tarea_fin
                           && lst_solicitudes.Contains(tt_hab.id_solicitud)
                           group tt_hab by tt_hab.id_solicitud into g
                           select new cls_ultima_tarea
                           {
                               id_solicitud = g.Key,
                               id_tramitetarea = g.Max(s => s.id_tramitetarea),
                               fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                           }).Union(from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                    join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                    where ta.id_tipo_tarea == id_tarea_fin
                                    && lst_solicitudes.Contains(tt_tr.id_solicitud)
                                    group tt_tr by tt_tr.id_solicitud into g
                                    select new cls_ultima_tarea
                                    {
                                        id_solicitud = g.Key,
                                        id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                        fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                    }).Union(from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                             join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                             where ta.id_tipo_tarea == id_tarea_fin
                                             && lst_solicitudes.Contains(tt_cp.id_cpadron)
                                             group tt_cp by tt_cp.id_cpadron into g
                                             select new cls_ultima_tarea
                                             {
                                                 id_solicitud = g.Key,
                                                 id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                                 fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                             });
            return lst_fin_tramite;
        }

        private int getDiasTotales(List<int> lst_solicitudes)
        {
            IniciarEntity();
            bool isDiasHabiles = rbDiasLaborales.Checked;
            int dias = 0;

            IQueryable<cls_ultima_tarea> lst_Primera_tarea = getListPrimeraTarea(lst_solicitudes);
            IQueryable<cls_ultima_tarea> lst_fin_tramite = getListFinTramite(lst_solicitudes);
            

            if (isDiasHabiles)
                try
                {
                    if (id_tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
                        dias = (from sol in db.SSIT_Solicitudes
                                join pri_tar in lst_Primera_tarea on sol.id_solicitud equals pri_tar.id_solicitud
                                join tt_pri in db.SGI_Tramites_Tareas on pri_tar.id_tramitetarea equals tt_pri.id_tramitetarea
                                join rev_tar in lst_fin_tramite on sol.id_solicitud equals rev_tar.id_solicitud
                                join tt_rev in db.SGI_Tramites_Tareas on rev_tar.id_tramitetarea equals tt_rev.id_tramitetarea
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_pri.FechaInicio_tramitetarea, tt_rev.FechaCierre_tramitetarea).Value
                                    - (SqlFunctions.DateDiff("wk", tt_pri.FechaInicio_tramitetarea, tt_rev.FechaCierre_tramitetarea).Value * 2)
                                }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                        dias = (from sol in db.Transf_Solicitudes
                                join pri_tar in lst_Primera_tarea on sol.id_solicitud equals pri_tar.id_solicitud
                                join tt_pri in db.SGI_Tramites_Tareas on pri_tar.id_tramitetarea equals tt_pri.id_tramitetarea
                                join rev_tar in lst_fin_tramite on sol.id_solicitud equals rev_tar.id_solicitud
                                join tt_rev in db.SGI_Tramites_Tareas on rev_tar.id_tramitetarea equals tt_rev.id_tramitetarea
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_pri.FechaInicio_tramitetarea, tt_rev.FechaCierre_tramitetarea).Value
                                    - (SqlFunctions.DateDiff("wk", tt_pri.FechaInicio_tramitetarea, tt_rev.FechaCierre_tramitetarea).Value * 2)
                                }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                        dias = (from sol in db.CPadron_Solicitudes
                                join pri_tar in lst_Primera_tarea on sol.id_cpadron equals pri_tar.id_solicitud
                                join tt_pri in db.SGI_Tramites_Tareas on pri_tar.id_tramitetarea equals tt_pri.id_tramitetarea
                                join rev_tar in lst_fin_tramite on sol.id_cpadron equals rev_tar.id_solicitud
                                join tt_rev in db.SGI_Tramites_Tareas on rev_tar.id_tramitetarea equals tt_rev.id_tramitetarea
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_pri.FechaInicio_tramitetarea, tt_rev.FechaCierre_tramitetarea).Value
                                    - (SqlFunctions.DateDiff("wk", tt_pri.FechaInicio_tramitetarea, tt_rev.FechaCierre_tramitetarea).Value * 2)
                                }).Sum(x => x.dias);
                }
                catch (Exception e)
                {
                    dias = 0;
                }
            else
                try
                {
                    if (id_tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
                        dias = (from sol in db.SSIT_Solicitudes
                            join pri_tar in lst_Primera_tarea on sol.id_solicitud equals pri_tar.id_solicitud
                            join tt_pri in db.SGI_Tramites_Tareas on pri_tar.id_tramitetarea equals tt_pri.id_tramitetarea
                            join rev_tar in lst_fin_tramite on sol.id_solicitud equals rev_tar.id_solicitud
                            join tt_rev in db.SGI_Tramites_Tareas on rev_tar.id_tramitetarea equals tt_rev.id_tramitetarea
                            select new
                            {
                                dias = SqlFunctions.DateDiff("dd", tt_pri.FechaInicio_tramitetarea, tt_rev.FechaCierre_tramitetarea).Value
                            }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                        dias = (from sol in db.Transf_Solicitudes
                                join pri_tar in lst_Primera_tarea on sol.id_solicitud equals pri_tar.id_solicitud
                                join tt_pri in db.SGI_Tramites_Tareas on pri_tar.id_tramitetarea equals tt_pri.id_tramitetarea
                                join rev_tar in lst_fin_tramite on sol.id_solicitud equals rev_tar.id_solicitud
                                join tt_rev in db.SGI_Tramites_Tareas on rev_tar.id_tramitetarea equals tt_rev.id_tramitetarea
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_pri.FechaInicio_tramitetarea, tt_rev.FechaCierre_tramitetarea).Value
                                }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                        dias = (from sol in db.CPadron_Solicitudes
                                join pri_tar in lst_Primera_tarea on sol.id_cpadron equals pri_tar.id_solicitud
                                join tt_pri in db.SGI_Tramites_Tareas on pri_tar.id_tramitetarea equals tt_pri.id_tramitetarea
                                join rev_tar in lst_fin_tramite on sol.id_cpadron equals rev_tar.id_solicitud
                                join tt_rev in db.SGI_Tramites_Tareas on rev_tar.id_tramitetarea equals tt_rev.id_tramitetarea
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_pri.FechaInicio_tramitetarea, tt_rev.FechaCierre_tramitetarea).Value
                                }).Sum(x => x.dias);
                }
                catch (Exception e)
                {
                    dias = 0;
                }
            FinalizarEntity();
            return dias;
        }

        private int getDiasTareasExcluidas(List<int> lst_solicitudes)
        {
            IniciarEntity();
            bool isDiasHabiles = rbDiasLaborales.Checked;
            int dias = 0;
            int id_tipoTramite = 0;

            if (!String.IsNullOrWhiteSpace(ddlTipoTramite.SelectedValue))
                id_tipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);

            List<int> lstTareas = new List<int>();
            lstTareas.Add((int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud);
            lstTareas.Add((int)Constants.ENG_Tipos_Tareas_New.Revision_Pagos);

            IQueryable<cls_ultima_tarea> lst_Primera_tarea = getListPrimeraTarea(lst_solicitudes);
            IQueryable<cls_ultima_tarea> lst_fin_tramite = getListFinTramite(lst_solicitudes);

            if (isDiasHabiles)
                try
                {
                    if (id_tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
                        dias = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                             join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                             join lst_pri in lst_Primera_tarea on tt_hab.id_solicitud equals lst_pri.id_solicitud
                             join lst_fin in lst_fin_tramite on tt_hab.id_solicitud equals lst_fin.id_solicitud
                             where tt_hab.id_tramitetarea >= lst_pri.id_tramitetarea
                             && tt_hab.id_tramitetarea <= lst_fin.id_tramitetarea
                             && lstTareas.Contains(tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                             select new
                            {
                                dias = SqlFunctions.DateDiff("dd", tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                - (SqlFunctions.DateDiff("wk", tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                            }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                        dias = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_tr.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_tr.id_solicitud equals lst_fin.id_solicitud
                                where tt_tr.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_tr.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstTareas.Contains(tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                    - (SqlFunctions.DateDiff("wk", tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                                }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                        dias = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_cp.id_cpadron equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_cp.id_cpadron equals lst_fin.id_solicitud
                                where tt_cp.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_cp.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstTareas.Contains(tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                    - (SqlFunctions.DateDiff("wk", tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                                }).Sum(x => x.dias);
                }
                catch (Exception e)
                {
                    dias = 0;
                }
            else
                try
                {
                    if (id_tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
                        dias = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_hab.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_hab.id_solicitud equals lst_fin.id_solicitud
                                where tt_hab.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_hab.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstTareas.Contains(tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                            {
                                dias = SqlFunctions.DateDiff("dd", tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                            }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                        dias = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_tr.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_tr.id_solicitud equals lst_fin.id_solicitud
                                where tt_tr.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_tr.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstTareas.Contains(tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                        dias = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_cp.id_cpadron equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_cp.id_cpadron equals lst_fin.id_solicitud
                                where tt_cp.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_cp.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstTareas.Contains(tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                }).Sum(x => x.dias);
                }
                catch (Exception e)
                {
                    dias = 0;
                }

            FinalizarEntity();
            return dias;
        }

        private int getDiasTiemposMuertos(List<int> lst_solicitudes)
        {
            IniciarEntity();
            bool isDiasHabiles = rbDiasLaborales.Checked;
            int dias = 0;
            int id_tipoTramite = 0;

            if (!String.IsNullOrWhiteSpace(ddlTipoTramite.SelectedValue))
                id_tipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);

            List<int> lstMuertos = new List<int>();
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Asignacon);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Realizar);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Revision);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Revision_Gerente);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Revision_SubGerente);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_GEDO);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Verificacion_AVH);
           // lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Generacion_Boleta);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Revision_Pagos);

            IQueryable<cls_ultima_tarea> lst_Primera_tarea = getListPrimeraTarea(lst_solicitudes);
            IQueryable<cls_ultima_tarea> lst_fin_tramite = getListFinTramite(lst_solicitudes);

            if (isDiasHabiles)
                try
                {
                    if (id_tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
                        dias = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_hab.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_hab.id_solicitud equals lst_fin.id_solicitud
                                where tt_hab.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_hab.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                            {
                                dias = SqlFunctions.DateDiff("dd", tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                - (SqlFunctions.DateDiff("wk", tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                            }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                        dias = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_tr.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_tr.id_solicitud equals lst_fin.id_solicitud
                                where tt_tr.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_tr.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                    - (SqlFunctions.DateDiff("wk", tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                                }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                        dias = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_cp.id_cpadron equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_cp.id_cpadron equals lst_fin.id_solicitud
                                where tt_cp.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_cp.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                    - (SqlFunctions.DateDiff("wk", tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                                }).Sum(x => x.dias);
                }
                catch (Exception e)
                {
                    dias = 0;
                }

            else
                try
                {
                    if (id_tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
                        dias = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_hab.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_hab.id_solicitud equals lst_fin.id_solicitud
                                where tt_hab.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_hab.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                            {
                                dias = SqlFunctions.DateDiff("dd", tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                            }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                        dias = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_tr.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_tr.id_solicitud equals lst_fin.id_solicitud
                                where tt_tr.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_tr.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                        dias = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_cp.id_cpadron equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_cp.id_cpadron equals lst_fin.id_solicitud
                                where tt_cp.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_cp.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                }).Sum(x => x.dias);
                }
                catch (Exception e)
                {
                    dias = 0;
                }

            FinalizarEntity();
            return dias;
        }

        private int getDiasDGHYPcorreccion(List<int> lst_solicitudes)
        {
            IniciarEntity();
            bool isDiasHabiles = rbDiasLaborales.Checked;
            int dias = 0;
            int id_tipoTramite = 0;

            if (!String.IsNullOrWhiteSpace(ddlTipoTramite.SelectedValue))
                id_tipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);

            List<int> lstMuertos = new List<int>();
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Asignacon);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Realizar);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Revision);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Revision_Gerente);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Revision_SubGerente);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_GEDO);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Verificacion_AVH);
            //lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Generacion_Boleta);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Revision_Pagos);

            IQueryable<cls_ultima_tarea> lst_Primera_tarea = getListPrimeraTarea(lst_solicitudes);
            IQueryable<cls_ultima_tarea> lst_fin_tramite = getListFinTramite(lst_solicitudes);

            if (isDiasHabiles)
                try
                {
                    if (id_tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
                        dias = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_hab.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_hab.id_solicitud equals lst_fin.id_solicitud
                                where tt_hab.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_hab.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                            {
                                dias = SqlFunctions.DateDiff("dd", tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                - (SqlFunctions.DateDiff("wk", tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                            }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                        dias = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_tr.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_tr.id_solicitud equals lst_fin.id_solicitud
                                where tt_tr.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_tr.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                    - (SqlFunctions.DateDiff("wk", tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                                }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                        dias = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_cp.id_cpadron equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_cp.id_cpadron equals lst_fin.id_solicitud
                                where tt_cp.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_cp.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                    - (SqlFunctions.DateDiff("wk", tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                                }).Sum(x => x.dias);
                }
                catch (Exception e)
                {
                    dias = 0;
                }

            else
                try
                {
                    if (id_tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
                        dias = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_hab.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_hab.id_solicitud equals lst_fin.id_solicitud
                                where tt_hab.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_hab.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                            {
                                dias = SqlFunctions.DateDiff("dd", tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                            }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                        dias = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_tr.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_tr.id_solicitud equals lst_fin.id_solicitud
                                where tt_tr.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_tr.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                        dias = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_cp.id_cpadron equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_cp.id_cpadron equals lst_fin.id_solicitud
                                where tt_cp.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_cp.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = SqlFunctions.DateDiff("dd", tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                }).Sum(x => x.dias);
                }
                catch (Exception e)
                {
                    dias = 0;
                }

            FinalizarEntity();
            return dias;
        }

        private int getDiasTiemposMuertosyCoreccion(List<int> lst_solicitudes)
        {
            IniciarEntity();
            bool isDiasHabiles = rbDiasLaborales.Checked;
            int dias = 0;
            int id_tipoTramite = 0;

            if (!String.IsNullOrWhiteSpace(ddlTipoTramite.SelectedValue))
                id_tipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);

            List<int> lstMuertos = new List<int>();
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Asignacon);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Realizar);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Revision);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Revision_Gerente);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Revision_SubGerente);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_GEDO);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Verificacion_AVH);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion);
            //lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Generacion_Boleta);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Revision_Pagos);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud);

            IQueryable<cls_ultima_tarea> lst_Primera_tarea = getListPrimeraTarea(lst_solicitudes);
            IQueryable<cls_ultima_tarea> lst_fin_tramite = getListFinTramite(lst_solicitudes);

            if (isDiasHabiles)
                try
                {
                    if (id_tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
                        dias = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_hab.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_hab.id_solicitud equals lst_fin.id_solicitud
                                where tt_hab.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_hab.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value != (int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente
                                            && tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value != (int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion ? 
                                            SqlFunctions.DateDiff("dd", tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                        - (SqlFunctions.DateDiff("wk", tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2) 
                                        : SqlFunctions.DateDiff("dd", tt_hab.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea != null ? tt_hab.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea : tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                        - (SqlFunctions.DateDiff("wk", tt_hab.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea != null ? tt_hab.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea : tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)

                                }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                        dias = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_tr.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_tr.id_solicitud equals lst_fin.id_solicitud
                                where tt_tr.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_tr.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value != (int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente
                                            && tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value != (int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion ?
                                            SqlFunctions.DateDiff("dd", tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                        - (SqlFunctions.DateDiff("wk", tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                                        : SqlFunctions.DateDiff("dd", tt_tr.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea != null ? tt_tr.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea : tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                    - (SqlFunctions.DateDiff("wk", tt_tr.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea != null ? tt_tr.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea : tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                                }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                        dias = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_cp.id_cpadron equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_cp.id_cpadron equals lst_fin.id_solicitud
                                where tt_cp.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_cp.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value != (int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente
                                            && tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value != (int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion ?
                                            SqlFunctions.DateDiff("dd", tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                        - (SqlFunctions.DateDiff("wk", tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                                        : SqlFunctions.DateDiff("dd", tt_cp.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea != null ? tt_cp.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea : tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                    - (SqlFunctions.DateDiff("wk", tt_cp.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea != null ? tt_cp.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea : tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value * 2)
                                }).Sum(x => x.dias);
                }
                catch (Exception e)
                {
                    dias = 0;
                }
            else
                try
                {
                    if (id_tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
                        dias = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_hab.id_solicitud equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_hab.id_solicitud equals lst_fin.id_solicitud
                                where tt_hab.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_hab.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                            {
                                    dias = tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value != (int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente
                                            && tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value != (int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion ?
                                            SqlFunctions.DateDiff("dd", tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value                                        
                                        : SqlFunctions.DateDiff("dd", tt_hab.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea != null ? tt_hab.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea : tt_hab.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_hab.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                            }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                            dias = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                    join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                    join lst_pri in lst_Primera_tarea on tt_tr.id_solicitud equals lst_pri.id_solicitud
                                    join lst_fin in lst_fin_tramite on tt_tr.id_solicitud equals lst_fin.id_solicitud
                                    where tt_tr.id_tramitetarea >= lst_pri.id_tramitetarea
                                    && tt_tr.id_tramitetarea <= lst_fin.id_tramitetarea
                                    && lstMuertos.Contains(tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                    select new
                                    {
                                        dias = tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value != (int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente
                                            && tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value != (int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion ?
                                            SqlFunctions.DateDiff("dd", tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                        : SqlFunctions.DateDiff("dd", tt_tr.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea != null ? tt_tr.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea : tt_tr.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_tr.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                    }).Sum(x => x.dias);
                    else if (id_tipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                        dias = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                join lst_pri in lst_Primera_tarea on tt_cp.id_cpadron equals lst_pri.id_solicitud
                                join lst_fin in lst_fin_tramite on tt_cp.id_cpadron equals lst_fin.id_solicitud
                                where tt_cp.id_tramitetarea >= lst_pri.id_tramitetarea
                                && tt_cp.id_tramitetarea <= lst_fin.id_tramitetarea
                                && lstMuertos.Contains(tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value)
                                select new
                                {
                                    dias = tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value != (int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente
                                            && tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea.Value != (int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion ?
                                            SqlFunctions.DateDiff("dd", tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                        : SqlFunctions.DateDiff("dd", tt_cp.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea != null ? tt_cp.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea : tt_cp.SGI_Tramites_Tareas.FechaInicio_tramitetarea, tt_cp.SGI_Tramites_Tareas.FechaCierre_tramitetarea).Value
                                }).Sum(x => x.dias);
                }
                catch (Exception e)
                {
                    dias = 0;
                }

            FinalizarEntity();
            return dias;
        }
        private List<clsItemIndicadoresExcel> GetData(int startRowIndex, int maximumRows, List<int> lst_solicitudes)
        {
            IniciarEntity();
            id_tarea_origen = 0;
            id_tarea_fin = 0;
            id_tipoTramite = 0;
            isTareaOrigenPrimera = false;
            isTareaOrigenFechaInicio = false;
            isTareaFinPrimera = false;
            isTareaFinFechaInicio = false;
            isDiasHabiles = false;
            List<clsItemIndicadoresExcel> lst = new List<clsItemIndicadoresExcel>();
            DateTime fechaAux = new DateTime();

            if (!String.IsNullOrWhiteSpace(ddlTipoTramite.SelectedValue))
                id_tipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);

            if (!String.IsNullOrWhiteSpace(ddlTareaOrigen.SelectedValue))
                id_tarea_origen = Convert.ToInt32(ddlTareaOrigen.SelectedValue);

            if (!String.IsNullOrWhiteSpace(ddlTareaFin.SelectedValue))
                id_tarea_fin = Convert.ToInt32(ddlTareaFin.SelectedValue);

            isTareaOrigenPrimera = rbTareaOrigenPrimera.Checked;
            isTareaOrigenFechaInicio = rbTareaOrigenFechaInicio.Checked;
            isTareaFinPrimera = rbTareaFinPrimera.Checked;
            isTareaFinFechaInicio = rbTareaFinFechaInicio.Checked;

            isDiasHabiles = rbDiasLaborales.Checked;

            List<clsItemIndicadores> resultados = new List<clsItemIndicadores>();
           
            var lstSol= lst_solicitudes.Skip(startRowIndex).Take(maximumRows).ToList();
            //////////////////////////////////////////////////Llenado

            List<int> lstDictamen = new List<int>();
            lstDictamen.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Asignacon);
            lstDictamen.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Realizar);
            lstDictamen.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Revision);
            lstDictamen.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Revision_Gerente);
            lstDictamen.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_Revision_SubGerente);
            lstDictamen.Add((int)Constants.ENG_Tipos_Tareas_New.Dictamen_GEDO);

            List<int> lstMuertos = new List<int>();
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente);
            lstMuertos.Add((int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion);
            TimeSpan diferencia;
            int dias_muertos = 0;
            int dias_avh = 0;
            int dias_dictamen = 0;
            int dias_pago = 0;
            int dias_contribuyente = 0;

            #region Habilitaciones
            if (id_tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
            {
                IQueryable<cls_ultima_tarea> lst_revi_firma = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                               join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                               where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion
                                                              // && lstSol.Contains(tt_hab.id_solicitud)
                                                               group tt_hab by tt_hab.id_solicitud into g
                                                               select new cls_ultima_tarea
                                                               {
                                                                   id_solicitud = g.Key,
                                                                   id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                                                   fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                               });


                IQueryable<cls_ultima_tarea> lst_gen_exp = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                            join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                            where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente
                                                            //&& lstSol.Contains(tt_hab.id_solicitud)
                                                            group tt_hab by tt_hab.id_solicitud into g
                                                            select new cls_ultima_tarea
                                                            {
                                                                id_solicitud = g.Key,
                                                                id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                                                fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                            });

                IQueryable<cls_ultima_tarea> lst_Primera_tarea = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                                  where tt_hab.SGI_Tramites_Tareas.ENG_Tareas.formulario_tarea != null
                                                                  && lstSol.Contains(tt_hab.id_solicitud)
                                                                  group tt_hab by tt_hab.id_solicitud into g
                                                                  select new cls_ultima_tarea
                                                                  {
                                                                      id_solicitud = g.Key,
                                                                      id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                                      fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                                  });

                IQueryable<cls_ultima_tarea> lst_Ultima_tarea = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                                 where lstSol.Contains(tt_hab.id_solicitud)
                                                                 group tt_hab by tt_hab.id_solicitud into g
                                                                 select new cls_ultima_tarea
                                                                 {
                                                                     id_solicitud = g.Key,
                                                                     id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                                                     fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                                 });

                IQueryable<cls_ultima_tarea> lst_Correcciones = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                                 join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                                 where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud
                                                                 && lstSol.Contains(tt_hab.id_solicitud)
                                                                 group tt_hab by tt_hab.id_solicitud into g
                                                                 select new cls_ultima_tarea
                                                                 {
                                                                     id_solicitud = g.Key,
                                                                     id_tramitetarea = g.Count(), // se pone la cantidad
                                                                     fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                                 });
                IQueryable<cls_ultima_tarea> lst_origen = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                           join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                           where ta.id_tipo_tarea == id_tarea_origen
                                                           && lstSol.Contains(tt_hab.id_solicitud)
                                                           group tt_hab by tt_hab.id_solicitud into g
                                                           select new cls_ultima_tarea
                                                           {
                                                               id_solicitud = g.Key,
                                                               id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                               fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                           });
                if (!isTareaOrigenPrimera)
                    lst_origen = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                  join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                  where ta.id_tipo_tarea == id_tarea_origen
                                  && lstSol.Contains(tt_hab.id_solicitud)
                                  group tt_hab by tt_hab.id_solicitud into g
                                  select new cls_ultima_tarea
                                  {
                                      id_solicitud = g.Key,
                                      id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                      fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  });

                IQueryable<cls_ultima_tarea> lst_fin = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                            //join res in lst_sol_2 on tt_hab.id_solicitud equals res.id_solicitud
                                                        join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                        where ta.id_tipo_tarea == id_tarea_fin
                                                        && lstSol.Contains(tt_hab.id_solicitud)
                                                        group tt_hab by tt_hab.id_solicitud into g
                                                        select new cls_ultima_tarea
                                                        {
                                                            id_solicitud = g.Key,
                                                            id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                            fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                        });
                if (!isTareaFinPrimera)
                    lst_fin = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                   //join res in lst_sol_2 on tt_hab.id_solicitud equals res.id_solicitud
                               join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                               where ta.id_tipo_tarea == id_tarea_fin
                               && lstSol.Contains(tt_hab.id_solicitud)
                               group tt_hab by tt_hab.id_solicitud into g
                               select new cls_ultima_tarea
                               {
                                   id_solicitud = g.Key,
                                   id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                   fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                               });

                var qSol = (from sol in db.SSIT_Solicitudes
                            join pri_tar in lst_Primera_tarea on sol.id_solicitud equals pri_tar.id_solicitud
                            join tt_pri in db.SGI_Tramites_Tareas on pri_tar.id_tramitetarea equals tt_pri.id_tramitetarea
                            join rev_tar in lst_revi_firma on sol.id_solicitud equals rev_tar.id_solicitud into pleft_revi
                            from rev_tar in pleft_revi.DefaultIfEmpty()
                            join gen_tar in lst_gen_exp on sol.id_solicitud equals gen_tar.id_solicitud into pleft_gen
                            from gen_tar in pleft_gen.DefaultIfEmpty()
                            join ult_tar in lst_Ultima_tarea on sol.id_solicitud equals ult_tar.id_solicitud into pleft_ult_tar
                            from ult_tar in pleft_ult_tar.DefaultIfEmpty()
                            join tt in db.SGI_Tramites_Tareas_HAB on ult_tar.id_tramitetarea equals tt.id_tramitetarea into pleft_tt
                            from tt in pleft_tt.DefaultIfEmpty()
                            join tar in db.ENG_Tareas on tt.SGI_Tramites_Tareas.id_tarea equals tar.id_tarea
                            join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito into pleft_tarea
                            from cir in pleft_tarea.DefaultIfEmpty()
                            join ori in lst_origen on sol.id_solicitud equals ori.id_solicitud
                            join tt_ori in db.SGI_Tramites_Tareas on ori.id_tramitetarea equals tt_ori.id_tramitetarea
                            join fin in lst_fin on sol.id_solicitud equals fin.id_solicitud
                            join tt_fin in db.SGI_Tramites_Tareas on fin.id_tramitetarea equals tt_fin.id_tramitetarea

                            join corr in lst_Correcciones on sol.id_solicitud equals corr.id_solicitud into pleft_corr
                            from corr in pleft_corr.DefaultIfEmpty()

                            select new clsItemIndicadores
                            {
                                cod_grupotramite = Constants.GruposDeTramite.HAB.ToString(),
                                id_tipotramite = sol.id_tipotramite,
                                id_solicitud = sol.id_solicitud,
                                id_tramitetarea_pri = tt_pri.id_tramitetarea,
                                FechaInicio = tt_pri.FechaInicio_tramitetarea,
                                id_tramitetarea_Rfd = rev_tar != null ? rev_tar.id_tramitetarea : 0,
                                FechaCierreRfd = rev_tar != null ? rev_tar.fecha_inicio : null,
                                id_tramitetarea_Ge = gen_tar != null ? gen_tar.id_tramitetarea : 0,
                                FechaCierreGe = gen_tar != null ? gen_tar.fecha_inicio : null,
                                id_tramitetarea_ult = tt.id_tramitetarea,
                                id_circuito = cir.id_circuito,
                                cod_circuito = cir.cod_circuito,
                                isTareaOrigenFechaInicio = isTareaOrigenFechaInicio,
                                isTareaFinFechaInicio = isTareaFinFechaInicio,
                                FechaInicioOrigen = tt_ori.FechaInicio_tramitetarea,
                                FechaFinOrigen = tt_ori.FechaCierre_tramitetarea,
                                TareaOrigen = tt_ori.ENG_Tareas.nombre_tarea,
                                TareaFin = tt_fin.ENG_Tareas.nombre_tarea,
                                FechaInicioFin = tt_fin.FechaInicio_tramitetarea,
                                FechaFinFin = tt_fin.FechaCierre_tramitetarea.Value,
                                cant_obs = corr != null ? corr.id_tramitetarea : 0,
                                dias_dictamen = 0,
                                dias_pago = 0,
                                dias_avh = 0,
                                dias_contribuyente = 0,
                                tiempo_muerto = 0,
                                isDiasHabiles = isDiasHabiles
                            });

                resultados = qSol.OrderBy(o => o.id_solicitud).ToList();

                foreach (var item in resultados)
                {
                    fechaAux = default(DateTime);

                    var lstTareasDictamen = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                             join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                             where lstDictamen.Contains(ta.id_tipo_tarea.Value)
                                             && tt_hab.id_solicitud == item.id_solicitud
                                             orderby tt_hab.id_tramitetarea 
                                             select tt_hab.SGI_Tramites_Tareas);
                    foreach (var tarea in lstTareasDictamen)
                    {
                        if (tarea.FechaCierre_tramitetarea != null)
                        {
                            if (isDiasHabiles)
                            {
                                item.dias_dictamen = item.dias_dictamen + Functions.DiasHabiles(tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value);
                                /*var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                dias_dictamen = (int)param.First().dias;
                                item.dias_dictamen = item.dias_dictamen + (dias_dictamen - (2 * (dias_dictamen / 7)));*/
                                if (tarea.FechaInicio_tramitetarea.ToShortDateString() == fechaAux.ToShortDateString())
                                    //|| tarea.FechaInicio_tramitetarea.ToShortDateString() == tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                    item.dias_dictamen = item.dias_dictamen - 1;
                            }
                            else
                            {
                                /*diferencia = tarea.FechaCierre_tramitetarea.Value - tarea.FechaInicio_tramitetarea;
                                item.dias_dictamen = item.dias_dictamen + diferencia.Days;*/
                                var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                item.dias_dictamen = item.dias_dictamen + (int)param.First().dias;
                            }
                            
                            //if (tarea.FechaInicio_tramitetarea.ToShortDateString() != tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                fechaAux = tarea.FechaCierre_tramitetarea.Value;
                        }
                    }

                    fechaAux = default(DateTime);

                    var lstTareasAvh = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                        join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                        where ta.id_tipo_tarea.Value == (int)Constants.ENG_Tipos_Tareas_New.Verificacion_AVH
                                        && tt_hab.id_solicitud == item.id_solicitud
                                        select tt_hab.SGI_Tramites_Tareas);
                    foreach (var tarea in lstTareasAvh)
                    {
                        if (tarea.FechaCierre_tramitetarea != null)
                        {
                            if (isDiasHabiles)
                            {
                                 item.dias_avh = item.dias_avh + Functions.DiasHabiles(tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value);
                                /*var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                dias_avh = (int)param.First().dias;
                                item.dias_avh = item.dias_avh + (dias_avh - (2 * dias_avh / 7));*/
                                /*if (tarea.FechaInicio_tramitetarea.ToShortDateString() == fechaAux.ToShortDateString())
                                    || tarea.FechaInicio_tramitetarea.ToShortDateString() == tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                    item.dias_avh = item.dias_avh - 1;*/
                            }
                            else
                            {
                                /*diferencia = tarea.FechaCierre_tramitetarea.Value - tarea.FechaInicio_tramitetarea;
                                item.dias_avh = item.dias_avh + diferencia.Days;*/
                                var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                item.dias_avh = item.dias_avh + (int)param.First().dias;
                            }
                            
                            if (tarea.FechaInicio_tramitetarea.ToShortDateString() != tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                fechaAux = tarea.FechaCierre_tramitetarea.Value;
                        }
                    }

                    fechaAux = default(DateTime);
                    var lstTareasPagos = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                  join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                  where ta.id_tipo_tarea.Value == (int)Constants.ENG_Tipos_Tareas_New.Revision_Pagos
                                                  && tt_hab.id_solicitud == item.id_solicitud
                                                  select tt_hab.SGI_Tramites_Tareas);
                    foreach (var tarea in lstTareasPagos)
                    {
                        if (tarea.FechaCierre_tramitetarea != null)
                        {
                            if (isDiasHabiles)
                            {
                                item.dias_pago = item.dias_pago + Functions.DiasHabiles(tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value);
                                /* var param = (from p in db.Parametros
                                              where p.id_param == 1
                                              select new
                                              {
                                                  p.id_param,
                                                  dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                              });
                                 dias_pago = (int)param.First().dias;
                                 item.dias_pago = item.dias_pago + (dias_pago - (2 * (dias_pago / 7)));*/
                                /*if (tarea.FechaInicio_tramitetarea.ToShortDateString() == fechaAux.ToShortDateString())
                                    || tarea.FechaInicio_tramitetarea.ToShortDateString() == tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                    item.dias_pago = item.dias_pago - 1;*/
                            }
                            else
                            {
                                /* diferencia = tarea.FechaCierre_tramitetarea.Value - tarea.FechaInicio_tramitetarea;
                                 item.dias_pago = item.dias_pago + diferencia.Days;*/
                                var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                item.dias_pago = item.dias_pago + (int)param.First().dias;
                            }
                            
                            if (tarea.FechaInicio_tramitetarea.ToShortDateString() != tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                fechaAux = tarea.FechaCierre_tramitetarea.Value;
                        }
                    }

                    fechaAux = default(DateTime);

                    var lstTareasCorreccciones = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                  join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                  where ta.id_tipo_tarea.Value == (int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud
                                                  && tt_hab.id_solicitud == item.id_solicitud
                                                  select tt_hab.SGI_Tramites_Tareas);
                    foreach (var tarea in lstTareasCorreccciones)
                    {
                        if (tarea.FechaCierre_tramitetarea != null)
                        {
                            if (isDiasHabiles)
                            {
                                item.dias_contribuyente = item.dias_contribuyente + Functions.DiasHabiles(tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value);
                                /*var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                dias_contribuyente = (int)param.First().dias;
                                item.dias_contribuyente = item.dias_contribuyente + (dias_contribuyente - (2 * dias_contribuyente / 7));*/
                                /*if (tarea.FechaInicio_tramitetarea.ToShortDateString() == fechaAux.ToShortDateString())
                                    || tarea.FechaInicio_tramitetarea.ToShortDateString() == tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                    item.dias_contribuyente = item.dias_contribuyente - 1;*/
                            }
                            else
                            {
                                /*diferencia = tarea.FechaCierre_tramitetarea.Value - tarea.FechaInicio_tramitetarea;
                                item.dias_contribuyente = item.dias_contribuyente + diferencia.Days;*/
                                var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                item.dias_contribuyente = item.dias_contribuyente + (int)param.First().dias;
                            }
                            
                            if (tarea.FechaInicio_tramitetarea.ToShortDateString() != tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                fechaAux = tarea.FechaCierre_tramitetarea.Value;
                        }
                    }

                    fechaAux = default(DateTime);

                    var lstTareasMuertos = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                            join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                            where lstMuertos.Contains(ta.id_tipo_tarea.Value)
                                            && tt_hab.id_solicitud == item.id_solicitud
                                            orderby tt_hab.id_tramitetarea ascending
                                            select tt_hab.SGI_Tramites_Tareas);
                    foreach (var tarea in lstTareasMuertos)
                    {
                        if (tarea.FechaCierre_tramitetarea != null)
                        {                            
                            if (isDiasHabiles)
                            {
                                item.tiempo_muerto = item.tiempo_muerto + Functions.DiasHabiles(tarea.FechaAsignacion_tramtietarea.Value, tarea.FechaCierre_tramitetarea.Value);
                                /*var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaAsignacion_tramtietarea.Value, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                dias_muertos = (int)param.First().dias;
                                item.tiempo_muerto = item.tiempo_muerto + (dias_muertos - (2 * dias_muertos / 7));*/
                               /* if (tarea.FechaAsignacion_tramtietarea.Value.ToShortDateString() == fechaAux.ToShortDateString())
                                    || tarea.FechaAsignacion_tramtietarea.Value.ToShortDateString() == tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                    item.tiempo_muerto = item.tiempo_muerto - 1;*/
                            }
                            else
                            {
                                /*diferencia = tarea.FechaCierre_tramitetarea.Value - tarea.FechaAsignacion_tramtietarea.Value;
                                item.tiempo_muerto = item.tiempo_muerto + diferencia.Days;*/
                                var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaAsignacion_tramtietarea.Value, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                item.tiempo_muerto = item.tiempo_muerto + (int)param.First().dias;
                            }
                            
                            if (tarea.FechaAsignacion_tramtietarea.Value.ToShortDateString() != tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                fechaAux = tarea.FechaCierre_tramitetarea.Value;
                        }
                    }
                }
            }
            #endregion Habilitaciones

            #region Transferencias
            if (id_tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
            {
                IQueryable<cls_ultima_tarea> lst_revi_firma = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                                               join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                               where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion
                                                               //&& lstSol.Contains(tt_tr.id_solicitud)
                                                               group tt_tr by tt_tr.id_solicitud into g
                                                               select new cls_ultima_tarea
                                                               {
                                                                   id_solicitud = g.Key,
                                                                   id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                                                   fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                               });


                IQueryable<cls_ultima_tarea> lst_gen_exp = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                                            join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                            where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente
                                                            //&& lstSol.Contains(tt_tr.id_solicitud)
                                                            group tt_tr by tt_tr.id_solicitud into g
                                                            select new cls_ultima_tarea
                                                            {
                                                                id_solicitud = g.Key,
                                                                id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                                                fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                            });

                IQueryable<cls_ultima_tarea> lst_Primera_tarea = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                                                  where tt_tr.SGI_Tramites_Tareas.ENG_Tareas.formulario_tarea != null
                                                                  && lstSol.Contains(tt_tr.id_solicitud)
                                                                  group tt_tr by tt_tr.id_solicitud into g
                                                                  select new cls_ultima_tarea
                                                                  {
                                                                      id_solicitud = g.Key,
                                                                      id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                                      fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                                  });

                IQueryable<cls_ultima_tarea> lst_Ultima_tarea = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                                                 where lstSol.Contains(tt_tr.id_solicitud)
                                                                 group tt_tr by tt_tr.id_solicitud into g
                                                                 select new cls_ultima_tarea
                                                                 {
                                                                     id_solicitud = g.Key,
                                                                     id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                                                     fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                                 });

                IQueryable<cls_ultima_tarea> lst_Correcciones = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                                                 join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                                 where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud
                                                                 && lstSol.Contains(tt_tr.id_solicitud)
                                                                 group tt_tr by tt_tr.id_solicitud into g
                                                                 select new cls_ultima_tarea
                                                                 {
                                                                     id_solicitud = g.Key,
                                                                     id_tramitetarea = g.Count(), // se pone la cantidad
                                                                     fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                                 });
                IQueryable<cls_ultima_tarea> lst_origen = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                                           join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                           where ta.id_tipo_tarea == id_tarea_origen
                                                           && lstSol.Contains(tt_tr.id_solicitud)
                                                           group tt_tr by tt_tr.id_solicitud into g
                                                           select new cls_ultima_tarea
                                                           {
                                                               id_solicitud = g.Key,
                                                               id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                               fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                           });
                if (!isTareaOrigenPrimera)
                    lst_origen = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                  join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                  where ta.id_tipo_tarea == id_tarea_origen
                                  && lstSol.Contains(tt_tr.id_solicitud)
                                  group tt_tr by tt_tr.id_solicitud into g
                                  select new cls_ultima_tarea
                                  {
                                      id_solicitud = g.Key,
                                      id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                      fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                  });

                IQueryable<cls_ultima_tarea> lst_fin = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                                            //join res in lst_sol_2 on tt_hab.id_solicitud equals res.id_solicitud
                                                        join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                        where ta.id_tipo_tarea == id_tarea_fin
                                                        && lstSol.Contains(tt_tr.id_solicitud)
                                                        group tt_tr by tt_tr.id_solicitud into g
                                                        select new cls_ultima_tarea
                                                        {
                                                            id_solicitud = g.Key,
                                                            id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                            fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                                                        });
                if (!isTareaFinPrimera)
                    lst_fin = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                   //join res in lst_sol_2 on tt_hab.id_solicitud equals res.id_solicitud
                               join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                               where ta.id_tipo_tarea == id_tarea_fin
                               && lstSol.Contains(tt_tr.id_solicitud)
                               group tt_tr by tt_tr.id_solicitud into g
                               select new cls_ultima_tarea
                               {
                                   id_solicitud = g.Key,
                                   id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                   fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaCierre_tramitetarea)
                               });

                var qSol = (from sol in db.Transf_Solicitudes
                            join pri_tar in lst_Primera_tarea on sol.id_solicitud equals pri_tar.id_solicitud
                            join tt_pri in db.SGI_Tramites_Tareas on pri_tar.id_tramitetarea equals tt_pri.id_tramitetarea
                            join rev_tar in lst_revi_firma on sol.id_solicitud equals rev_tar.id_solicitud into pleft_revi
                            from rev_tar in pleft_revi.DefaultIfEmpty()
                            join gen_tar in lst_gen_exp on sol.id_solicitud equals gen_tar.id_solicitud into pleft_gen
                            from gen_tar in pleft_gen.DefaultIfEmpty()
                            join ult_tar in lst_Ultima_tarea on sol.id_solicitud equals ult_tar.id_solicitud into pleft_ult_tar
                            from ult_tar in pleft_ult_tar.DefaultIfEmpty()
                            join tt in db.SGI_Tramites_Tareas_TRANSF on ult_tar.id_tramitetarea equals tt.id_tramitetarea into pleft_tt
                            from tt in pleft_tt.DefaultIfEmpty()
                            join tar in db.ENG_Tareas on tt.SGI_Tramites_Tareas.id_tarea equals tar.id_tarea
                            join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito into pleft_tarea
                            from cir in pleft_tarea.DefaultIfEmpty()
                            join ori in lst_origen on sol.id_solicitud equals ori.id_solicitud
                            join tt_ori in db.SGI_Tramites_Tareas on ori.id_tramitetarea equals tt_ori.id_tramitetarea
                            join fin in lst_fin on sol.id_solicitud equals fin.id_solicitud
                            join tt_fin in db.SGI_Tramites_Tareas on fin.id_tramitetarea equals tt_fin.id_tramitetarea

                            join corr in lst_Correcciones on sol.id_solicitud equals corr.id_solicitud into pleft_corr
                            from corr in pleft_corr.DefaultIfEmpty()

                            select new clsItemIndicadores
                            {
                                cod_grupotramite = Constants.GruposDeTramite.TR.ToString(),
                                id_tipotramite = sol.id_tipotramite,
                                id_solicitud = sol.id_solicitud,
                                id_tramitetarea_pri = tt_pri.id_tramitetarea,
                                FechaInicio = tt_pri.FechaInicio_tramitetarea,
                                id_tramitetarea_Rfd = rev_tar != null ? rev_tar.id_tramitetarea : 0,
                                FechaCierreRfd = rev_tar != null ? rev_tar.fecha_inicio : null,
                                id_tramitetarea_Ge = gen_tar != null ? gen_tar.id_tramitetarea : 0,
                                FechaCierreGe = gen_tar != null ? gen_tar.fecha_inicio : null,
                                id_tramitetarea_ult = tt.id_tramitetarea,
                                id_circuito = cir.id_circuito,
                                cod_circuito = cir.cod_circuito,
                                isTareaOrigenFechaInicio = isTareaOrigenFechaInicio,
                                isTareaFinFechaInicio = isTareaFinFechaInicio,
                                FechaInicioOrigen = tt_ori.FechaInicio_tramitetarea,
                                FechaFinOrigen = tt_ori.FechaCierre_tramitetarea,
                                TareaOrigen = tt_ori.ENG_Tareas.nombre_tarea,
                                TareaFin = tt_fin.ENG_Tareas.nombre_tarea,
                                FechaInicioFin = tt_fin.FechaInicio_tramitetarea,
                                FechaFinFin = tt_fin.FechaCierre_tramitetarea.Value,
                                cant_obs = corr != null ? corr.id_tramitetarea : 0,
                                dias_dictamen = 0,
                                dias_pago = 0,
                                dias_avh = 0,
                                dias_contribuyente = 0,
                                tiempo_muerto = 0,
                                isDiasHabiles = isDiasHabiles
                            });

                resultados = qSol.OrderBy(o => o.id_solicitud).ToList();

                fechaAux = default(DateTime);

                foreach (var item in resultados)
                {
                    var lstTareasDictamen = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                             join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                             where lstDictamen.Contains(ta.id_tipo_tarea.Value)
                                             && tt_tr.id_solicitud == item.id_solicitud
                                             orderby tt_tr.id_tramitetarea
                                             select tt_tr.SGI_Tramites_Tareas);
                    foreach (var tarea in lstTareasDictamen)
                    {
                        if (tarea.FechaCierre_tramitetarea != null)
                        {
                            if (isDiasHabiles)
                            {
                                item.dias_dictamen = item.dias_dictamen + Functions.DiasHabiles(tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value);
                                /*var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                dias_dictamen = (int)param.First().dias;
                                item.dias_dictamen = item.dias_dictamen + (dias_dictamen - (2 * (dias_dictamen / 7)));*/
                                if (tarea.FechaInicio_tramitetarea.ToShortDateString() == fechaAux.ToShortDateString())
                                    //|| tarea.FechaInicio_tramitetarea.ToShortDateString() == tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                    item.dias_dictamen = item.dias_dictamen - 1;
                            }
                            else
                            {
                                /*diferencia = tarea.FechaCierre_tramitetarea.Value - tarea.FechaInicio_tramitetarea;
                                item.dias_dictamen = item.dias_dictamen + diferencia.Days;*/
                                var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                item.dias_dictamen = item.dias_dictamen + (int)param.First().dias;
                            }

                            //if (tarea.FechaInicio_tramitetarea.ToShortDateString() != tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                            fechaAux = tarea.FechaCierre_tramitetarea.Value;
                        }
                    }

                    fechaAux = default(DateTime);

                    var lstTareasAvh = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                        join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                        where ta.id_tipo_tarea.Value == (int)Constants.ENG_Tipos_Tareas_New.Verificacion_AVH
                                        && tt_tr.id_solicitud == item.id_solicitud
                                        select tt_tr.SGI_Tramites_Tareas);
                    foreach (var tarea in lstTareasAvh)
                    {
                        if (tarea.FechaCierre_tramitetarea != null)
                        {
                            if (isDiasHabiles)
                            {
                                item.dias_avh = item.dias_avh + Functions.DiasHabiles(tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value);
                                /*var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                dias_avh = (int)param.First().dias;
                                item.dias_avh = item.dias_avh + (dias_avh - (2 * dias_avh / 7));*/
                               /* if (tarea.FechaInicio_tramitetarea.ToShortDateString() == fechaAux.ToShortDateString())
                                    || tarea.FechaInicio_tramitetarea.ToShortDateString() == tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                    item.dias_avh = item.dias_avh - 1;*/
                            }
                            else
                            {
                                /*diferencia = tarea.FechaCierre_tramitetarea.Value - tarea.FechaInicio_tramitetarea;
                                item.dias_avh = item.dias_avh + diferencia.Days;*/
                                var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                item.dias_avh = item.dias_avh + (int)param.First().dias;
                            }
                            
                            if (tarea.FechaInicio_tramitetarea.ToShortDateString() != tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                fechaAux = tarea.FechaCierre_tramitetarea.Value;
                        }
                    }

                    fechaAux = default(DateTime);

                    var lstTareasPagos = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                          join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                          where ta.id_tipo_tarea.Value == (int)Constants.ENG_Tipos_Tareas_New.Revision_Pagos
                                          && tt_tr.id_solicitud == item.id_solicitud
                                          select tt_tr.SGI_Tramites_Tareas);
                    foreach (var tarea in lstTareasPagos)
                    {
                        if (tarea.FechaCierre_tramitetarea != null)
                        {
                            if (isDiasHabiles)
                            {
                                item.dias_pago = item.dias_pago + Functions.DiasHabiles(tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value);
                                /*var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                dias_pago = (int)param.First().dias;
                                item.dias_pago = item.dias_pago + (dias_pago - (2 * (dias_pago / 7)));*/
                                /*if (tarea.FechaInicio_tramitetarea.ToShortDateString() == fechaAux.ToShortDateString())
                                    || tarea.FechaInicio_tramitetarea.ToShortDateString() == tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                    item.dias_pago = item.dias_pago - 1;*/
                            }
                            else
                            {
                                /* diferencia = tarea.FechaCierre_tramitetarea.Value - tarea.FechaInicio_tramitetarea;
                                 item.dias_pago = item.dias_pago + diferencia.Days;*/
                                var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                item.dias_pago = item.dias_pago + (int)param.First().dias;
                            }
                            
                            if (tarea.FechaInicio_tramitetarea.ToShortDateString() != tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                fechaAux = tarea.FechaCierre_tramitetarea.Value;
                        }
                    }

                    fechaAux = default(DateTime);

                    var lstTareasCorreccciones = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                                  join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                  where ta.id_tipo_tarea.Value == (int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud
                                                  && tt_tr.id_solicitud == item.id_solicitud
                                                  select tt_tr.SGI_Tramites_Tareas);
                    foreach (var tarea in lstTareasCorreccciones)
                    {
                        if (tarea.FechaCierre_tramitetarea != null)
                        {
                            if (isDiasHabiles)
                            {
                                item.dias_contribuyente = item.dias_contribuyente + Functions.DiasHabiles(tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value);
                                /*var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                dias_contribuyente = (int)param.First().dias;
                                item.dias_contribuyente = item.dias_contribuyente + (dias_contribuyente - (2 * dias_contribuyente / 7));*/
                                /*if (tarea.FechaInicio_tramitetarea.ToShortDateString() == fechaAux.ToShortDateString())
                                    || tarea.FechaInicio_tramitetarea.ToShortDateString() == tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                    item.dias_contribuyente = item.dias_contribuyente - 1;*/
                            }
                            else
                            {
                                /*diferencia = tarea.FechaCierre_tramitetarea.Value - tarea.FechaInicio_tramitetarea;
                                item.dias_contribuyente = item.dias_contribuyente + diferencia.Days;*/
                                var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                item.dias_contribuyente = item.dias_contribuyente + (int)param.First().dias;
                            }
                            
                            if (tarea.FechaInicio_tramitetarea.ToShortDateString() != tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                fechaAux = tarea.FechaCierre_tramitetarea.Value;
                        }
                    }

                    fechaAux = default(DateTime);

                    var lstTareasMuertos = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                            join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                            where lstMuertos.Contains(ta.id_tipo_tarea.Value)
                                            && tt_tr.id_solicitud == item.id_solicitud
                                            select tt_tr.SGI_Tramites_Tareas);
                    foreach (var tarea in lstTareasMuertos)
                    {
                        if (tarea.FechaCierre_tramitetarea != null)
                        {
                            if (isDiasHabiles)
                            {
                                item.tiempo_muerto = item.tiempo_muerto + Functions.DiasHabiles(tarea.FechaAsignacion_tramtietarea.Value, tarea.FechaCierre_tramitetarea.Value);
                                /* var param = (from p in db.Parametros
                                              where p.id_param == 1
                                              select new
                                              {
                                                  p.id_param,
                                                  dias = SqlFunctions.DateDiff("dd", tarea.FechaAsignacion_tramtietarea.Value, tarea.FechaCierre_tramitetarea.Value)
                                              });
                                 dias_muertos = (int)param.First().dias;
                                 item.tiempo_muerto = item.tiempo_muerto + (dias_muertos - (2 * dias_muertos / 7));*/
                               /* if (tarea.FechaAsignacion_tramtietarea.Value.ToShortDateString() == fechaAux.ToShortDateString())
                                    || tarea.FechaAsignacion_tramtietarea.Value.ToShortDateString() == tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                    item.tiempo_muerto = item.tiempo_muerto - 1;*/
                            }
                            else
                            {
                                /*diferencia = tarea.FechaCierre_tramitetarea.Value - tarea.FechaAsignacion_tramtietarea.Value;
                                item.tiempo_muerto = item.tiempo_muerto + diferencia.Days;*/
                                var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaAsignacion_tramtietarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                item.tiempo_muerto = item.tiempo_muerto + (int)param.First().dias;
                            }
                            
                            if (tarea.FechaAsignacion_tramtietarea.Value.ToShortDateString() != tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                fechaAux = tarea.FechaCierre_tramitetarea.Value;
                        }
                        
                    }
                }
            }
            #endregion Ttransferencias

            #region CPadron
            if (id_tipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
            {

                IQueryable<cls_ultima_tarea> lst_gen_exp = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                                            join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                            where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente
                                                           // && lstSol.Contains(tt_cp.id_cpadron)
                                                            group tt_cp by tt_cp.id_cpadron into g
                                                            select new cls_ultima_tarea
                                                            {
                                                                id_solicitud = g.Key,
                                                                id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                                                fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                                                            });

                IQueryable<cls_ultima_tarea> lst_Primera_tarea = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                                                  where tt_cp.SGI_Tramites_Tareas.ENG_Tareas.formulario_tarea != null
                                                                  && lstSol.Contains(tt_cp.id_cpadron)
                                                                  group tt_cp by tt_cp.id_cpadron into g
                                                                  select new cls_ultima_tarea
                                                                  {
                                                                      id_solicitud = g.Key,
                                                                      id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                                      fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                                                                  });

                IQueryable<cls_ultima_tarea> lst_Ultima_tarea = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                                                 where lstSol.Contains(tt_cp.id_cpadron)
                                                                 group tt_cp by tt_cp.id_cpadron into g
                                                                 select new cls_ultima_tarea
                                                                 {
                                                                     id_solicitud = g.Key,
                                                                     id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                                                     fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                                                                 });

                IQueryable<cls_ultima_tarea> lst_Correcciones = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                                                 join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                                 where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud
                                                                 && lstSol.Contains(tt_cp.id_cpadron)
                                                                 group tt_cp by tt_cp.id_cpadron into g
                                                                 select new cls_ultima_tarea
                                                                 {
                                                                     id_solicitud = g.Key,
                                                                     id_tramitetarea = g.Count(), // se pone la cantidad
                                                                     fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                                                                 });
                IQueryable<cls_ultima_tarea> lst_origen = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                                           join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                           where ta.id_tipo_tarea == id_tarea_origen
                                                           && lstSol.Contains(tt_cp.id_cpadron)
                                                           group tt_cp by tt_cp.id_cpadron into g
                                                           select new cls_ultima_tarea
                                                           {
                                                               id_solicitud = g.Key,
                                                               id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                               fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                                                           });
                if (!isTareaOrigenPrimera)
                    lst_origen = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                  join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                  where ta.id_tipo_tarea == id_tarea_origen
                                  && lstSol.Contains(tt_cp.id_cpadron)
                                  group tt_cp by tt_cp.id_cpadron into g
                                  select new cls_ultima_tarea
                                  {
                                      id_solicitud = g.Key,
                                      id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                      fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                                  });

                IQueryable<cls_ultima_tarea> lst_fin = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                                        join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                        where ta.id_tipo_tarea == id_tarea_fin
                                                        && lstSol.Contains(tt_cp.id_cpadron)
                                                        group tt_cp by tt_cp.id_cpadron into g
                                                        select new cls_ultima_tarea
                                                        {
                                                            id_solicitud = g.Key,
                                                            id_tramitetarea = g.Min(s => s.id_tramitetarea),
                                                            fecha_inicio = g.Min(s => s.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                                                        });
                if (!isTareaFinPrimera)
                    lst_fin = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                               join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                               where ta.id_tipo_tarea == id_tarea_fin
                               && lstSol.Contains(tt_cp.id_cpadron)
                               group tt_cp by tt_cp.id_cpadron into g
                               select new cls_ultima_tarea
                               {
                                   id_solicitud = g.Key,
                                   id_tramitetarea = g.Max(s => s.id_tramitetarea),
                                   fecha_inicio = g.Max(s => s.SGI_Tramites_Tareas.FechaInicio_tramitetarea)
                               });

                var qSol = (from sol in db.CPadron_Solicitudes
                            join pri_tar in lst_Primera_tarea on sol.id_cpadron equals pri_tar.id_solicitud
                            join tt_pri in db.SGI_Tramites_Tareas on pri_tar.id_tramitetarea equals tt_pri.id_tramitetarea
                            join gen_tar in lst_gen_exp on sol.id_cpadron equals gen_tar.id_solicitud into pleft_gen
                            from gen_tar in pleft_gen.DefaultIfEmpty()
                            join ult_tar in lst_Ultima_tarea on sol.id_cpadron equals ult_tar.id_solicitud into pleft_ult_tar
                            from ult_tar in pleft_ult_tar.DefaultIfEmpty()
                            join tt in db.SGI_Tramites_Tareas_CPADRON on ult_tar.id_tramitetarea equals tt.id_tramitetarea into pleft_tt
                            from tt in pleft_tt.DefaultIfEmpty()
                            join tar in db.ENG_Tareas on tt.SGI_Tramites_Tareas.id_tarea equals tar.id_tarea
                            join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito into pleft_tarea
                            from cir in pleft_tarea.DefaultIfEmpty()
                            join ori in lst_origen on sol.id_cpadron equals ori.id_solicitud
                            join tt_ori in db.SGI_Tramites_Tareas on ori.id_tramitetarea equals tt_ori.id_tramitetarea
                            join fin in lst_fin on sol.id_cpadron equals fin.id_solicitud
                            join tt_fin in db.SGI_Tramites_Tareas on fin.id_tramitetarea equals tt_fin.id_tramitetarea

                            join corr in lst_Correcciones on sol.id_cpadron equals corr.id_solicitud into pleft_corr
                            from corr in pleft_corr.DefaultIfEmpty()

                            select new clsItemIndicadores
                            {
                                cod_grupotramite = Constants.GruposDeTramite.CP.ToString(),
                                id_tipotramite = sol.id_tipotramite,
                                id_solicitud = sol.id_cpadron,
                                id_tramitetarea_pri = tt_pri.id_tramitetarea,
                                FechaInicio = tt_pri.FechaInicio_tramitetarea,
                                id_tramitetarea_Rfd = 0,
                                FechaCierreRfd = null,
                                id_tramitetarea_Ge = gen_tar != null ? gen_tar.id_tramitetarea : 0,
                                FechaCierreGe = gen_tar != null ? gen_tar.fecha_inicio : null,
                                id_tramitetarea_ult = tt.id_tramitetarea,
                                id_circuito = cir.id_circuito,
                                cod_circuito = cir.cod_circuito,
                                isTareaOrigenFechaInicio = isTareaOrigenFechaInicio,
                                isTareaFinFechaInicio = isTareaFinFechaInicio,
                                FechaInicioOrigen = tt_ori.FechaInicio_tramitetarea,
                                FechaFinOrigen = tt_ori.FechaCierre_tramitetarea,
                                TareaOrigen = tt_ori.ENG_Tareas.nombre_tarea,
                                TareaFin = tt_fin.ENG_Tareas.nombre_tarea,
                                FechaInicioFin = tt_fin.FechaInicio_tramitetarea,
                                FechaFinFin = tt_fin.FechaCierre_tramitetarea.Value,
                                cant_obs = corr != null ? corr.id_tramitetarea : 0,
                                dias_dictamen = 0,
                                dias_pago = 0,
                                dias_avh = 0,
                                dias_contribuyente = 0,
                                tiempo_muerto = 0,
                                isDiasHabiles = isDiasHabiles
                            });

                resultados = qSol.OrderBy(o => o.id_solicitud).ToList();

                foreach (var item in resultados)
                {
                    fechaAux = default(DateTime);
                    var lstTareasCorreccciones = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                                  join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                  where ta.id_tipo_tarea.Value == (int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud
                                                  && tt_cp.id_cpadron == item.id_solicitud
                                                  select tt_cp.SGI_Tramites_Tareas);
                    foreach (var tarea in lstTareasCorreccciones)
                    {
                        if (tarea.FechaCierre_tramitetarea != null)
                        {
                            if (isDiasHabiles)
                            {
                                item.dias_contribuyente = item.dias_contribuyente + Functions.DiasHabiles(tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value);
                                /* var param = (from p in db.Parametros
                                              where p.id_param == 1
                                              select new
                                              {
                                                  p.id_param,
                                                  dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                              });
                                 dias_contribuyente = (int)param.First().dias;
                                 item.dias_contribuyente = item.dias_contribuyente + (dias_contribuyente - (2 * dias_contribuyente / 7));*/
                                /*if (tarea.FechaInicio_tramitetarea.ToShortDateString() == fechaAux.ToShortDateString())
                                    || tarea.FechaInicio_tramitetarea.ToShortDateString() == tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                    item.dias_contribuyente = item.dias_contribuyente - 1;*/
                            }
                            else
                            {
                                /* diferencia = tarea.FechaCierre_tramitetarea.Value - tarea.FechaInicio_tramitetarea;
                                 item.dias_contribuyente = item.dias_contribuyente + diferencia.Days;*/
                                var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaInicio_tramitetarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                item.dias_contribuyente = item.dias_contribuyente + (int)param.First().dias;
                            }
                            
                            if (tarea.FechaInicio_tramitetarea.ToShortDateString() != tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                fechaAux = tarea.FechaCierre_tramitetarea.Value;
                        }
                    }

                    fechaAux = default(DateTime);

                    var lstTareasMuertos = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                            join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                            where lstMuertos.Contains(ta.id_tipo_tarea.Value)
                                            && tt_cp.id_cpadron == item.id_solicitud
                                            select tt_cp.SGI_Tramites_Tareas);
                    foreach (var tarea in lstTareasMuertos)
                    {
                        if (tarea.FechaCierre_tramitetarea != null)
                        {
                            if (isDiasHabiles)
                            {
                                item.tiempo_muerto = item.tiempo_muerto + Functions.DiasHabiles(tarea.FechaAsignacion_tramtietarea.Value, tarea.FechaCierre_tramitetarea.Value);
                                    /*var param = (from p in db.Parametros
                                                 where p.id_param == 1
                                                 select new
                                                 {
                                                     p.id_param,
                                                     dias = SqlFunctions.DateDiff("dd", tarea.FechaAsignacion_tramtietarea.Value, tarea.FechaCierre_tramitetarea.Value)
                                                 });
                                    dias_muertos = (int)param.First().dias;
                                    item.tiempo_muerto = item.tiempo_muerto + (dias_muertos - (2 * dias_muertos / 7));*/
                                /*if (tarea.FechaAsignacion_tramtietarea.Value.ToShortDateString() == fechaAux.ToShortDateString())
                                || tarea.FechaAsignacion_tramtietarea.Value.ToShortDateString() == tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                item.tiempo_muerto = item.tiempo_muerto - 1;*/
                            }
                            else
                            {
                                /* diferencia = tarea.FechaCierre_tramitetarea.Value - tarea.FechaInicio_tramitetarea;
                                 item.tiempo_muerto = item.tiempo_muerto + diferencia.Days;*/
                                var param = (from p in db.Parametros
                                             where p.id_param == 1
                                             select new
                                             {
                                                 p.id_param,
                                                 dias = SqlFunctions.DateDiff("dd", tarea.FechaAsignacion_tramtietarea, tarea.FechaCierre_tramitetarea.Value)
                                             });
                                item.tiempo_muerto = item.tiempo_muerto + (int)param.First().dias;
                            }
                            
                            if (tarea.FechaAsignacion_tramtietarea.Value.ToShortDateString() != tarea.FechaCierre_tramitetarea.Value.ToShortDateString())
                                fechaAux = tarea.FechaCierre_tramitetarea.Value;
                        }
                    }
                }                
            }
            #endregion CPadron

            FinalizarEntity();
            lst = (from res in resultados
                   select new clsItemIndicadoresExcel
                   {
                       id_solicitud = res.id_solicitud,
                       dias_avh = res.dias_avh,
                       dias_contribuyente = res.dias_contribuyente,
                       dias_dghyp = res.dias_dghyp,
                       cant_obs = res.cant_obs,
                       dias_dictamen = res.dias_dictamen,
                       dias_totales = res.dias_totales,
                       FechaInicio = res.FechaInicio != null ? res.FechaInicio.ToShortDateString() : " ",
                       HoraInicio = res.FechaInicio != null ? res.FechaInicio.ToShortTimeString() : " ",
                       Fecha_Firma_Dispo = res.FechaCierreRfd != null ? res.FechaCierreRfd.Value.ToShortDateString() : " ",
                       Hora_Firma_Dispo = res.FechaCierreRfd != null ? res.FechaCierreRfd.Value.ToShortTimeString() : " ",
                       Fecha_Generacion_Expediente = res.FechaCierreGe != null ? res.FechaCierreGe.Value.ToShortDateString() : " ",
                       Hora_Generacion_Expediente = res.FechaCierreGe != null ? res.FechaCierreGe.Value.ToShortTimeString() : " ",
                       Fecha_Tarea_Fin = res.FechaFin != null ? res.FechaFin.ToShortDateString() : " ",
                       Hora_Tarea_Fin = res.FechaFin != null ? res.FechaFin.ToShortTimeString() : " ",
                       Fecha_Tarea_Origen = res.FechaOrigen != null ? res.FechaOrigen.ToShortDateString() : " ",
                       Hora_Tarea_Origen = res.FechaOrigen != null ? res.FechaOrigen.ToShortTimeString() : " ",
                       observado = res.observado,
                       tiempo_muerto = res.tiempo_muerto
                   }).ToList();
            return lst;
        }

        private List<int> filtrar()
        {
            IniciarEntity();
            db.Database.CommandTimeout = 300;

            DateTime? fechaInicioDesde = null;
            DateTime? fechaInicioHasta = null;
            DateTime? fechaCrfdDesde = null;
            DateTime? fechaCrfdHasta = null;
            DateTime? fechaCgeDesde = null;
            DateTime? fechaCgeHasta = null;
            DateTime? fechaRdgDesde = null;
            DateTime? fechaRdgHasta = null;
            DateTime? fechaGrt2Desde = null;
            DateTime? fechaGrt2hasta = null;

            int id_tipoTramite = 0;
            int id_observado = 0;
            int id_tarea_origen = 0;
            int id_tarea_fin = 0;
            bool isTareaOrigenPrimera;
            bool isTareaOrigenFechaInicio;
            bool isTareaFinPrimera;
            bool isTareaFinFechaInicio;
            List<int> lst_final = new List<int>();

            if (!String.IsNullOrWhiteSpace(ddlTipoTramite.SelectedValue))
                id_tipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);

            if (!string.IsNullOrEmpty(txtFechaInicioDesde.Text))
                fechaInicioDesde = Convert.ToDateTime(txtFechaInicioDesde.Text);

            if (!string.IsNullOrEmpty(txtFechaInicioHasta.Text))
                fechaInicioHasta = Convert.ToDateTime(txtFechaInicioHasta.Text).AddDays(1).AddMilliseconds(-1);         

            if (!string.IsNullOrEmpty(txtFechaCrfdDesde.Text))
                fechaCrfdDesde = Convert.ToDateTime(txtFechaCrfdDesde.Text);

            if (!string.IsNullOrEmpty(txtFechaCrfdHasta.Text))
                fechaCrfdHasta = Convert.ToDateTime(txtFechaCrfdHasta.Text).AddDays(1).AddMilliseconds(-1);

            if (!string.IsNullOrEmpty(txtFechaCgeDesde.Text))
                fechaCgeDesde = Convert.ToDateTime(txtFechaCgeDesde.Text);

            if (!string.IsNullOrEmpty(txtFechaCgeHasta.Text))
                fechaCgeHasta = Convert.ToDateTime(txtFechaCgeHasta.Text).AddDays(1).AddMilliseconds(-1);

            if (!string.IsNullOrEmpty(txtFechaRdgDesde.Text))
                fechaRdgDesde = Convert.ToDateTime(txtFechaRdgDesde.Text);

            if (!string.IsNullOrEmpty(txtFechaRdgHasta.Text))
                fechaRdgHasta = Convert.ToDateTime(txtFechaRdgHasta.Text).AddDays(1).AddMilliseconds(-1);

            if (!string.IsNullOrEmpty(txtFechaGrt2Desde.Text))
                fechaGrt2Desde = Convert.ToDateTime(txtFechaGrt2Desde.Text);

            if (!string.IsNullOrEmpty(txtFechaGrt2Hasta.Text))
                fechaGrt2hasta = Convert.ToDateTime(txtFechaGrt2Hasta.Text).AddDays(1).AddMilliseconds(-1);

            if (fechaInicioDesde == null && fechaInicioHasta == null && fechaCrfdDesde == null && fechaCrfdHasta == null &&
                fechaCgeDesde == null && fechaCgeHasta == null && fechaRdgDesde == null && fechaRdgHasta == null &&
                fechaGrt2Desde == null && fechaGrt2hasta == null)
                return null;

            if (!String.IsNullOrWhiteSpace(ddlObservado.SelectedValue))
                id_observado = Convert.ToInt32(ddlObservado.SelectedValue);

            if (!String.IsNullOrWhiteSpace(ddlTareaOrigen.SelectedValue))
                id_tarea_origen = Convert.ToInt32(ddlTareaOrigen.SelectedValue);

            if (!String.IsNullOrWhiteSpace(ddlTareaFin.SelectedValue))
                id_tarea_fin = Convert.ToInt32(ddlTareaFin.SelectedValue);

            isTareaOrigenPrimera = rbTareaOrigenPrimera.Checked;
            isTareaOrigenFechaInicio = rbTareaOrigenFechaInicio.Checked;
            isTareaFinPrimera = rbTareaFinPrimera.Checked;
            isTareaFinFechaInicio = rbTareaFinFechaInicio.Checked;

            List<clsItemIndicadores> resultados = new List<clsItemIndicadores>();
            IQueryable<cls_ultima_tarea> lst_revi_firma = null;
            IQueryable<cls_ultima_tarea> lst_gen_exp = null;
            IQueryable<cls_ultima_tarea> lst_revision_dghyp = null;
            IQueryable<cls_ultima_tarea> lst_soli = null;
            IQueryable<cls_ultima_tarea> lst_revi_grt2 = null;

            db.Database.CommandTimeout = 300;

            #region habilitaciones
            if (id_tipoTramite == (int)Constants.TipoDeTramite.Habilitacion)
            {
                IQueryable<cls_ultima_tarea> lst_Primera_tarea = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                                  where tt_hab.SGI_Tramites_Tareas.ENG_Tareas.formulario_tarea != null
                                                                  group tt_hab by tt_hab.id_solicitud into g
                                                                  select new cls_ultima_tarea
                                                                  {
                                                                      id_solicitud = g.Key,
                                                                      id_tramitetarea = g.Min(s => s.id_tramitetarea)
                                                                  });
                lst_soli = (from res in db.SSIT_Solicitudes
                            join pri_tar in lst_Primera_tarea on res.id_solicitud equals pri_tar.id_solicitud
                            join tt_pri in db.SGI_Tramites_Tareas on pri_tar.id_tramitetarea equals tt_pri.id_tramitetarea
                            where (fechaInicioDesde == null || tt_pri.FechaInicio_tramitetarea >= fechaInicioDesde)
                            && (fechaInicioHasta == null || tt_pri.FechaInicio_tramitetarea <= fechaInicioHasta)
                            && res.id_tipotramite == id_tipoTramite
                            select new cls_ultima_tarea
                            {
                                id_solicitud = res.id_solicitud,
                                id_tramitetarea = tt_pri.id_tramitetarea
                            });


                if (fechaCrfdDesde != null && fechaCrfdHasta != null)
                {

                    lst_revi_firma = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                      join tt_rev in db.SGI_Tramites_Tareas on tt_hab.id_tramitetarea equals tt_rev.id_tramitetarea
                                      join soli in lst_soli on tt_hab.id_solicitud equals soli.id_solicitud
                                      join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                      where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion
                                      && (fechaCrfdDesde == null || tt_rev.FechaCierre_tramitetarea >= fechaCrfdDesde)
                                      && (fechaCrfdHasta == null || tt_rev.FechaCierre_tramitetarea <= fechaCrfdHasta)
                                      group tt_hab by tt_hab.id_solicitud into g
                                      select new cls_ultima_tarea
                                      {
                                          id_solicitud = g.Key,
                                          id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                      });

                }

                if (lst_revi_firma != null)
                    lst_soli = lst_revi_firma;

                if (fechaCgeDesde != null && fechaCgeHasta != null)
                {
                    lst_gen_exp = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                   join tt_gen in db.SGI_Tramites_Tareas on tt_hab.id_tramitetarea equals tt_gen.id_tramitetarea
                                   join soli in lst_soli on tt_hab.id_solicitud equals soli.id_solicitud
                                   join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                   where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente
                                   && (fechaCgeDesde == null || tt_gen.FechaCierre_tramitetarea >= fechaCgeDesde)
                                   && (fechaCgeHasta == null || tt_gen.FechaCierre_tramitetarea <= fechaCgeHasta)
                                   group tt_hab by tt_hab.id_solicitud into g
                                   select new cls_ultima_tarea
                                   {
                                        id_solicitud = g.Key,
                                        id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                   });
                }

                if (lst_gen_exp != null)
                    lst_soli = lst_gen_exp;

                if (fechaRdgDesde != null && fechaRdgHasta != null)
                {
                    lst_revision_dghyp = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                          join tt_rev in db.SGI_Tramites_Tareas on tt_hab.id_tramitetarea equals tt_rev.id_tramitetarea
                                          join soli in lst_soli on tt_hab.id_solicitud equals soli.id_solicitud
                                          join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                          where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Revision_DGHyP
                                          && (fechaRdgDesde == null || tt_rev.FechaInicio_tramitetarea >= fechaRdgDesde)
                                          && (fechaRdgHasta == null || tt_rev.FechaInicio_tramitetarea <= fechaRdgHasta)
                                          group tt_hab by tt_hab.id_solicitud into g
                                          select new cls_ultima_tarea
                                          {
                                                id_solicitud = g.Key,
                                                id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                          });
                }

                if (lst_revision_dghyp != null)
                    lst_soli = lst_revision_dghyp;

                var lst_sol = (from sol in db.SSIT_Solicitudes
                               join solici in lst_soli on sol.id_solicitud equals solici.id_solicitud
                               select sol).Distinct().OrderBy(o => o.id_solicitud);
                

                //Bloque 2
                IQueryable<cls_ultima_tarea> lst_origen = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                                join res in lst_sol on tt_hab.id_solicitud equals res.id_solicitud
                                                                join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                                where ta.id_tipo_tarea == id_tarea_origen
                                                                group tt_hab by tt_hab.id_solicitud into g
                                                                select new cls_ultima_tarea
                                                                {
                                                                    id_solicitud = g.Key,
                                                                    id_tramitetarea = g.Min(s => s.id_tramitetarea)
                                                                });
                     if (!isTareaOrigenPrimera)
                         lst_origen = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                       join res in lst_sol on tt_hab.id_solicitud equals res.id_solicitud
                                       join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                       where ta.id_tipo_tarea == id_tarea_origen
                                       group tt_hab by tt_hab.id_solicitud into g
                                       select new cls_ultima_tarea
                                       {
                                           id_solicitud = g.Key,
                                           id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                       });

                     var lst_sol_2 = (from res in db.SSIT_Solicitudes
                                      join ori in lst_origen on res.id_solicitud equals ori.id_solicitud
                                      join tt_ori in db.SGI_Tramites_Tareas on ori.id_tramitetarea equals tt_ori.id_tramitetarea
                                      where (isTareaOrigenFechaInicio || tt_ori.FechaCierre_tramitetarea != null)
                                      select res.id_solicitud).ToList();

                    
                     //Bloque 3
                     IQueryable<cls_ultima_tarea> lst_fin = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                                 //join res in lst_sol_2 on tt_hab.id_solicitud equals res.id_solicitud
                                                             join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                             where ta.id_tipo_tarea == id_tarea_fin
                                                             && lst_sol_2.Contains(tt_hab.id_solicitud)
                                                             group tt_hab by tt_hab.id_solicitud into g
                                                             select new cls_ultima_tarea
                                                             {
                                                                 id_solicitud = g.Key,
                                                                 id_tramitetarea = g.Min(s => s.id_tramitetarea)
                                                             });
                     if (!isTareaFinPrimera)
                         lst_fin = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                        //join res in lst_sol_2 on tt_hab.id_solicitud equals res.id_solicitud
                                    join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                    where ta.id_tipo_tarea == id_tarea_fin
                                    && lst_sol_2.Contains(tt_hab.id_solicitud)
                                    group tt_hab by tt_hab.id_solicitud into g
                                    select new cls_ultima_tarea
                                    {
                                        id_solicitud = g.Key,
                                        id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                    });

                     var lst_sol_3 = (from res in db.SSIT_Solicitudes
                                      join fin in lst_fin on res.id_solicitud equals fin.id_solicitud
                                      join tt_fin in db.SGI_Tramites_Tareas on fin.id_tramitetarea equals tt_fin.id_tramitetarea
                                      where (isTareaFinFechaInicio || tt_fin.FechaCierre_tramitetarea != null)
                                      select res);
                     //bloque 4
                     var lst_sol_4 = lst_sol_3;

                IQueryable<cls_ultima_tarea> lst_Ultima_tarea;
                     if (hid_circuitos_selected.Value.ToString().Split(Convert.ToChar(",")).Length > 0)
                     {
                         List<int> lstCir = new List<int>();
                         foreach (string id_circuito in hid_circuitos_selected.Value.ToString().Split(Convert.ToChar(",")))
                         {
                             if (!string.IsNullOrEmpty(id_circuito))
                                 lstCir.Add(Convert.ToInt32(id_circuito));
                         }
                         if (lstCir.Count > 0)
                         {
                             lst_Ultima_tarea = (from tt_hab in db.SGI_Tramites_Tareas_HAB
                                                 join res in lst_sol_3 on tt_hab.id_solicitud equals res.id_solicitud
                                                 group tt_hab by tt_hab.id_solicitud into g
                                                 select new cls_ultima_tarea
                                                 {
                                                     id_solicitud = g.Key,
                                                     id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                                 });

                        lst_sol_4 = (from res in db.SSIT_Solicitudes
                                          join ult_tar in lst_Ultima_tarea on res.id_solicitud equals ult_tar.id_solicitud
                                          join tt_hab in db.SGI_Tramites_Tareas_HAB on ult_tar.id_tramitetarea equals tt_hab.id_tramitetarea
                                          join tar in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.id_tarea equals tar.id_tarea
                                          join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito into pleft_tarea
                                          from cir in pleft_tarea.DefaultIfEmpty()
                                          where lstCir.Contains(tar.id_circuito)
                                          select res);
                     }
                     }
                     //bloque 5
                     var lst_sol_5 = lst_sol_4;
                     if (id_observado > 0)
                     {
                         lst_sol_5 = (from res in db.SSIT_Solicitudes
                                      join sol in lst_sol_4 on res.id_solicitud equals sol.id_solicitud
                                      where res.SGI_Tramites_Tareas_HAB.Any(x => x.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea
                                                 == (int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud) == (id_observado == 1)
                                      select res);

                     }
                     lst_final = lst_sol_5.OrderBy(o => o.id_solicitud).Select(x => x.id_solicitud).ToList();
                }
            #endregion habilitaciones

            #region Transferencias
            
            if (id_tipoTramite == (int)Constants.TipoDeTramite.Transferencia)
            {
                IQueryable<cls_ultima_tarea> lst_Primera_tarea = (from tt_hab in db.SGI_Tramites_Tareas_TRANSF
                                                                  where tt_hab.SGI_Tramites_Tareas.ENG_Tareas.formulario_tarea != null
                                                                  group tt_hab by tt_hab.id_solicitud into g
                                                                  select new cls_ultima_tarea
                                                                  {
                                                                      id_solicitud = g.Key,
                                                                      id_tramitetarea = g.Min(s => s.id_tramitetarea)
                                                                  });
                
                lst_soli = (from res in db.Transf_Solicitudes
                                join pri_tar in lst_Primera_tarea on res.id_solicitud equals pri_tar.id_solicitud
                                join tt_pri in db.SGI_Tramites_Tareas on pri_tar.id_tramitetarea equals tt_pri.id_tramitetarea
                                where (fechaInicioDesde == null || tt_pri.FechaInicio_tramitetarea >= fechaInicioDesde)
                                && (fechaInicioHasta == null || tt_pri.FechaInicio_tramitetarea <= fechaInicioHasta)
                                && res.id_tipotramite == id_tipoTramite
                            select new cls_ultima_tarea
                                {
                                id_solicitud = res.id_solicitud,
                                id_tramitetarea = tt_pri.id_tramitetarea
                            });

                

                if (fechaCrfdDesde != null && fechaCrfdHasta != null)
                {

                    lst_revi_firma = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                      join tt_rev in db.SGI_Tramites_Tareas on tt_tr.id_tramitetarea equals tt_rev.id_tramitetarea
                                      join soli in lst_soli on tt_tr.id_solicitud equals soli.id_solicitud
                                      join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                      where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion
                                      && (fechaCrfdDesde == null || tt_rev.FechaCierre_tramitetarea >= fechaCrfdDesde)
                                      && (fechaCrfdHasta == null || tt_rev.FechaCierre_tramitetarea <= fechaCrfdHasta)
                                      group tt_tr by tt_tr.id_solicitud into g
                                      select new cls_ultima_tarea
                                      {
                                          id_solicitud = g.Key,
                                          id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                      });
                }

                if (lst_revi_firma != null)
                    lst_soli = lst_revi_firma;

                if (fechaGrt2Desde != null && fechaGrt2hasta != null)
                {

                    lst_revi_grt2 = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                      join tt_rev in db.SGI_Tramites_Tareas on tt_tr.id_tramitetarea equals tt_rev.id_tramitetarea
                                      join soli in lst_soli on tt_tr.id_solicitud equals soli.id_solicitud
                                      join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                      where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Revision_Firma_Disposicion
                                      && (fechaGrt2Desde == null || tt_rev.FechaCierre_tramitetarea >= fechaGrt2Desde)
                                      && (fechaGrt2hasta == null || tt_rev.FechaCierre_tramitetarea <= fechaGrt2hasta)
                                      group tt_tr by tt_tr.id_solicitud into g
                                      select new cls_ultima_tarea
                                      {
                                          id_solicitud = g.Key,
                                          id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                      });
                }

                if (lst_revi_grt2 != null)
                    lst_soli = lst_revi_grt2;

                if (fechaCgeDesde != null && fechaCgeHasta != null)
                {
                    lst_gen_exp = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                   join tt_gen in db.SGI_Tramites_Tareas on tt_tr.id_tramitetarea equals tt_gen.id_tramitetarea
                                   join soli in lst_soli on tt_tr.id_solicitud equals soli.id_solicitud
                                   join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                   where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente
                                   && (fechaCgeDesde == null || tt_gen.FechaCierre_tramitetarea >= fechaCgeDesde)
                                   && (fechaCgeHasta == null || tt_gen.FechaCierre_tramitetarea <= fechaCgeHasta)
                                   group tt_tr by tt_tr.id_solicitud into g
                                   select new cls_ultima_tarea
                                   {
                                       id_solicitud = g.Key,
                                       id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                   });
                }

                if (lst_gen_exp != null)
                    lst_soli = lst_gen_exp;

                if (fechaRdgDesde != null && fechaRdgHasta != null)
                {
                    lst_revision_dghyp = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                          join tt_rev in db.SGI_Tramites_Tareas on tt_tr.id_tramitetarea equals tt_rev.id_tramitetarea
                                          join soli in lst_soli on tt_tr.id_solicitud equals soli.id_solicitud
                                          join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                          where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Revision_DGHyP
                                          && (fechaRdgDesde == null || tt_rev.FechaInicio_tramitetarea >= fechaRdgDesde)
                                          && (fechaRdgHasta == null || tt_rev.FechaInicio_tramitetarea <= fechaRdgHasta)
                                          group tt_tr by tt_tr.id_solicitud into g
                                          select new cls_ultima_tarea
                                          {
                                              id_solicitud = g.Key,
                                              id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                          });
                }

                if (lst_revision_dghyp != null)
                    lst_soli = lst_revision_dghyp;

                var lst_sol = (from sol in db.Transf_Solicitudes
                               join solici in lst_soli on sol.id_solicitud equals solici.id_solicitud
                               select sol).Distinct().OrderBy(o => o.id_solicitud);
                
                 //Bloque 2
                 IQueryable<cls_ultima_tarea> lst_origen = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                                            join res in lst_sol on tt_tr.id_solicitud equals res.id_solicitud
                                                            join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                            where ta.id_tipo_tarea == id_tarea_origen
                                                            group tt_tr by tt_tr.id_solicitud into g
                                                            select new cls_ultima_tarea
                                                            {
                                                                id_solicitud = g.Key,
                                                                id_tramitetarea = g.Min(s => s.id_tramitetarea)
                                                            });
                 if (!isTareaOrigenPrimera)
                     lst_origen = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                   join res in lst_sol on tt_tr.id_solicitud equals res.id_solicitud
                                   join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                   where ta.id_tipo_tarea == id_tarea_origen
                                   group tt_tr by tt_tr.id_solicitud into g
                                   select new cls_ultima_tarea
                                   {
                                       id_solicitud = g.Key,
                                       id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                   });

                 var lst_sol_2 = (from res in db.Transf_Solicitudes
                                  join ori in lst_origen on res.id_solicitud equals ori.id_solicitud
                                  join tt_ori in db.SGI_Tramites_Tareas on ori.id_tramitetarea equals tt_ori.id_tramitetarea
                                  where (isTareaOrigenFechaInicio || tt_ori.FechaCierre_tramitetarea != null)
                                  select res.id_solicitud).ToList();
                 //Bloque 3
                 IQueryable<cls_ultima_tarea> lst_fin = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                                             //join res in lst_sol_2 on tt_hab.id_solicitud equals res.id_solicitud
                                                         join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                         where ta.id_tipo_tarea == id_tarea_fin
                                                         && lst_sol_2.Contains(tt_tr.id_solicitud)
                                                         group tt_tr by tt_tr.id_solicitud into g
                                                         select new cls_ultima_tarea
                                                         {
                                                             id_solicitud = g.Key,
                                                             id_tramitetarea = g.Min(s => s.id_tramitetarea)
                                                         });
                 if (!isTareaFinPrimera)
                     lst_fin = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                    //join res in lst_sol_2 on tt_hab.id_solicitud equals res.id_solicitud
                                join ta in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                where ta.id_tipo_tarea == id_tarea_fin
                                && lst_sol_2.Contains(tt_tr.id_solicitud)
                                group tt_tr by tt_tr.id_solicitud into g
                                select new cls_ultima_tarea
                                {
                                    id_solicitud = g.Key,
                                    id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                });

                 var lst_sol_3 = (from res in db.Transf_Solicitudes
                                  join fin in lst_fin on res.id_solicitud equals fin.id_solicitud
                                  join tt_fin in db.SGI_Tramites_Tareas on fin.id_tramitetarea equals tt_fin.id_tramitetarea
                                  where (isTareaFinFechaInicio || tt_fin.FechaCierre_tramitetarea != null)
                                  select res);
                 //bloque 4
                 var lst_sol_4 = lst_sol_3;
                 IQueryable<cls_ultima_tarea> lst_Ultima_tarea;
                 if (hid_circuitos_selected.Value.ToString().Split(Convert.ToChar(",")).Length > 0)
                 {
                     List<int> lstCir = new List<int>();
                     foreach (string id_circuito in hid_circuitos_selected.Value.ToString().Split(Convert.ToChar(",")))
                     {
                         if (!string.IsNullOrEmpty(id_circuito))
                             lstCir.Add(Convert.ToInt32(id_circuito));
                     }
                     if (lstCir.Count > 0)
                     {
                         lst_Ultima_tarea = (from tt_tr in db.SGI_Tramites_Tareas_TRANSF
                                             join res in lst_sol_3 on tt_tr.id_solicitud equals res.id_solicitud
                                             group tt_tr by tt_tr.id_solicitud into g
                                             select new cls_ultima_tarea
                                             {
                                                 id_solicitud = g.Key,
                                                 id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                             });
                         lst_sol_4 = (from res in db.Transf_Solicitudes
                                      join ult_tar in lst_Ultima_tarea on res.id_solicitud equals ult_tar.id_solicitud
                                      join tt_tr in db.SGI_Tramites_Tareas_TRANSF on ult_tar.id_tramitetarea equals tt_tr.id_tramitetarea
                                      join tar in db.ENG_Tareas on tt_tr.SGI_Tramites_Tareas.id_tarea equals tar.id_tarea
                                      join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito into pleft_tarea
                                      from cir in pleft_tarea.DefaultIfEmpty()
                                      where lstCir.Contains(tar.id_circuito)
                                      select res);
                     }
                 }
                 //bloque 5
                 var lst_sol_5 = lst_sol_4;
                 if (id_observado > 0)
                 {
                     lst_sol_5 = (from res in db.Transf_Solicitudes
                                  join sol in lst_sol_4 on res.id_solicitud equals sol.id_solicitud
                                  where res.SGI_Tramites_Tareas_TRANSF.Any(x => x.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea
                                             == (int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud) == (id_observado == 1)
                                  select res);

                 }
                 lst_final = lst_sol_5.OrderBy(o => o.id_solicitud).Select(x => x.id_solicitud).ToList();
             }
                #endregion Transferencias

                #region CPadron

                 if (id_tipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                { 
               
                    IQueryable<cls_ultima_tarea> lst_Primera_tarea = (from tt_hab in db.SGI_Tramites_Tareas_CPADRON
                                                                      where tt_hab.SGI_Tramites_Tareas.ENG_Tareas.formulario_tarea != null
                                                                      group tt_hab by tt_hab.id_cpadron into g
                                                                      select new cls_ultima_tarea
                                                                      {
                                                                          id_solicitud = g.Key,
                                                                          id_tramitetarea = g.Min(s => s.id_tramitetarea)
                                                                      });
                    lst_soli = (from res in db.CPadron_Solicitudes
                                    join pri_tar in lst_Primera_tarea on res.id_cpadron equals pri_tar.id_solicitud
                                    join tt_pri in db.SGI_Tramites_Tareas on pri_tar.id_tramitetarea equals tt_pri.id_tramitetarea
                                    where (fechaInicioDesde == null || tt_pri.FechaInicio_tramitetarea >= fechaInicioDesde)
                                    && (fechaInicioHasta == null || tt_pri.FechaInicio_tramitetarea <= fechaInicioHasta)
                                    && res.id_tipotramite == id_tipoTramite
                                select new cls_ultima_tarea
                                {
                                    id_solicitud = res.id_cpadron,
                                    id_tramitetarea = tt_pri.id_tramitetarea
                                });
                

                    if (fechaCgeDesde != null && fechaCgeHasta != null)
                    {
                        lst_gen_exp = (from tt_hab in db.SGI_Tramites_Tareas_CPADRON
                                       join tt_gen in db.SGI_Tramites_Tareas on tt_hab.id_tramitetarea equals tt_gen.id_tramitetarea
                                       join soli in lst_soli on tt_hab.id_cpadron equals soli.id_solicitud
                                       join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                       where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Generar_Expediente
                                       && (fechaCgeDesde == null || tt_gen.FechaCierre_tramitetarea >= fechaCgeDesde)
                                       && (fechaCgeHasta == null || tt_gen.FechaCierre_tramitetarea <= fechaCgeHasta)
                                       group tt_hab by tt_hab.id_cpadron into g
                                       select new cls_ultima_tarea
                                       {
                                           id_solicitud = g.Key,
                                           id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                       });
                }

                if (lst_gen_exp != null)
                    lst_soli = lst_gen_exp;

                if (fechaRdgDesde != null && fechaRdgHasta != null)
                {
                    lst_revision_dghyp = (from tt_hab in db.SGI_Tramites_Tareas_CPADRON
                                          join tt_rev in db.SGI_Tramites_Tareas on tt_hab.id_tramitetarea equals tt_rev.id_tramitetarea
                                          join soli in lst_soli on tt_hab.id_cpadron equals soli.id_solicitud
                                          join ta in db.ENG_Tareas on tt_hab.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                          where ta.id_tipo_tarea == (int)Constants.ENG_Tipos_Tareas_New.Revision_DGHyP
                                          && (fechaRdgDesde == null || tt_rev.FechaInicio_tramitetarea >= fechaRdgDesde)
                                          && (fechaRdgHasta == null || tt_rev.FechaInicio_tramitetarea <= fechaRdgHasta)
                                          group tt_hab by tt_hab.id_cpadron into g
                                          select new cls_ultima_tarea
                                          {
                                              id_solicitud = g.Key,
                                              id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                          });
                }

                if (lst_revision_dghyp != null)
                    lst_soli = lst_revision_dghyp;

                var lst_sol = (from sol in db.CPadron_Solicitudes
                               join solici in lst_soli on sol.id_cpadron equals solici.id_solicitud
                               select sol).Distinct().OrderBy(o => o.id_cpadron);

                
                //Bloque 2
                IQueryable<cls_ultima_tarea> lst_origen = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                                           join res in lst_sol on tt_cp.id_cpadron equals res.id_cpadron
                                                           join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                           where ta.id_tipo_tarea == id_tarea_origen
                                                           group tt_cp by tt_cp.id_cpadron into g
                                                           select new cls_ultima_tarea
                                                           {
                                                               id_solicitud = g.Key,
                                                               id_tramitetarea = g.Min(s => s.id_tramitetarea)
                                                           });
                if (!isTareaOrigenPrimera)
                    lst_origen = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                  join res in lst_sol on tt_cp.id_cpadron equals res.id_cpadron
                                  join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                  where ta.id_tipo_tarea == id_tarea_origen
                                  group tt_cp by tt_cp.id_cpadron into g
                                  select new cls_ultima_tarea
                                  {
                                      id_solicitud = g.Key,
                                      id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                  });

                var lst_sol_2 = (from res in db.CPadron_Solicitudes
                                 join ori in lst_origen on res.id_cpadron equals ori.id_solicitud
                                 join tt_ori in db.SGI_Tramites_Tareas on ori.id_tramitetarea equals tt_ori.id_tramitetarea
                                 where (isTareaOrigenFechaInicio || tt_ori.FechaCierre_tramitetarea != null)
                                 select res.id_cpadron).ToList();
                //Bloque 3
                IQueryable<cls_ultima_tarea> lst_fin = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                                        join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                                                        where ta.id_tipo_tarea == id_tarea_fin
                                                        && lst_sol_2.Contains(tt_cp.id_cpadron)
                                                        group tt_cp by tt_cp.id_cpadron into g
                                                        select new cls_ultima_tarea
                                                        {
                                                            id_solicitud = g.Key,
                                                            id_tramitetarea = g.Min(s => s.id_tramitetarea)
                                                        });
                if (!isTareaFinPrimera)
                    lst_fin = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                               join ta in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.ENG_Tareas.id_tarea equals ta.id_tarea
                               where ta.id_tipo_tarea == id_tarea_fin
                               && lst_sol_2.Contains(tt_cp.id_cpadron)
                               group tt_cp by tt_cp.id_cpadron into g
                               select new cls_ultima_tarea
                               {
                                   id_solicitud = g.Key,
                                   id_tramitetarea = g.Max(s => s.id_tramitetarea)
                               });

                var lst_sol_3 = (from res in db.CPadron_Solicitudes
                                 join fin in lst_fin on res.id_cpadron equals fin.id_solicitud
                                 join tt_fin in db.SGI_Tramites_Tareas on fin.id_tramitetarea equals tt_fin.id_tramitetarea
                                 where (isTareaFinFechaInicio || tt_fin.FechaCierre_tramitetarea != null)
                                 select res);
                //bloque 4
                var lst_sol_4 = lst_sol_3;
                IQueryable<cls_ultima_tarea> lst_Ultima_tarea;
                if (hid_circuitos_selected.Value.ToString().Split(Convert.ToChar(",")).Length > 0)
                {
                    List<int> lstCir = new List<int>();
                    foreach (string id_circuito in hid_circuitos_selected.Value.ToString().Split(Convert.ToChar(",")))
                    {
                        if (!string.IsNullOrEmpty(id_circuito))
                            lstCir.Add(Convert.ToInt32(id_circuito));
                    }
                    if (lstCir.Count > 0)
                    {
                        lst_Ultima_tarea = (from tt_cp in db.SGI_Tramites_Tareas_CPADRON
                                            join res in lst_sol_3 on tt_cp.id_cpadron equals res.id_cpadron
                                            group tt_cp by tt_cp.id_cpadron into g
                                            select new cls_ultima_tarea
                                            {
                                                id_solicitud = g.Key,
                                                id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                            });
                        lst_sol_4 = (from res in db.CPadron_Solicitudes
                                     join ult_tar in lst_Ultima_tarea on res.id_cpadron equals ult_tar.id_solicitud
                                     join tt_cp in db.SGI_Tramites_Tareas_CPADRON on ult_tar.id_tramitetarea equals tt_cp.id_tramitetarea
                                     join tar in db.ENG_Tareas on tt_cp.SGI_Tramites_Tareas.id_tarea equals tar.id_tarea
                                     join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito into pleft_tarea
                                     from cir in pleft_tarea.DefaultIfEmpty()
                                     where lstCir.Contains(tar.id_circuito)
                                     select res);
                    }
                }
                //bloque 5
                var lst_sol_5 = lst_sol_4;
                if (id_observado > 0)
                {
                    lst_sol_5 = (from res in db.CPadron_Solicitudes
                                 join sol in lst_sol_4 on res.id_cpadron equals sol.id_cpadron
                                 where res.SGI_Tramites_Tareas_CPADRON.Any(x => x.SGI_Tramites_Tareas.ENG_Tareas.id_tipo_tarea
                                            == (int)Constants.ENG_Tipos_Tareas_New.Correccion_Solicitud) == (id_observado == 1)
                                 select res);

                }
                lst_final = lst_sol_5.OrderBy(o => o.id_cpadron).Select(x => x.id_cpadron).ToList();
            }
                #endregion CPadron
                return lst_final;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                grdResultados.DataBind();
                updResultados.Update();
                EjecutarScript(btn_BuscarTramite, "showResultado();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updPnlFiltroBuscar_tramite, "showfrmError();");
            }
        }

        #region paginado
        protected void cmdPage(object sender, EventArgs e)
        {
            Button obj = (Button)sender;
            grdResultados.PageIndex = Convert.ToInt16(obj.Text) - 1;
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdResultados.PageIndex = grdResultados.PageIndex + 1;
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdResultados.PageIndex = grdResultados.PageIndex - 1;
        }

        protected void grdResultados_DataBound(object sender, EventArgs e)
        {
            GridView grid = grdResultados;
            GridViewRow fila = (GridViewRow)grid.BottomPagerRow;

            if (fila != null)
            {
                Button btnAnterior = (Button)fila.Cells[0].FindControl("cmdAnterior");
                Button btnSiguiente = (Button)fila.Cells[0].FindControl("cmdSiguiente");

                if (grid.PageIndex == 0)
                    btnAnterior.Visible = false;
                else
                {
                    btnAnterior.Visible = true;
                    btnAnterior.Width = Unit.Parse("100px");
                    btnAnterior.Height = Unit.Parse("40px");
                }

                if (grid.PageIndex == grid.PageCount - 1)
                    btnSiguiente.Visible = false;
                else
                {
                    btnSiguiente.Visible = true;
                    btnSiguiente.Width = Unit.Parse("100px");
                    btnSiguiente.Height = Unit.Parse("40px");
                }


                // Ocultar todos los botones con Números de Página
                for (int i = 1; i <= 19; i++)
                {
                    Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                    btn.Visible = false;
                }


                if (grid.PageIndex == 0 || grid.PageCount <= 10)
                {
                    // Mostrar 10 botones o el máximo de páginas

                    for (int i = 1; i <= 10; i++)
                    {
                        if (i <= grid.PageCount)
                        {
                            Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                            btn.Text = i.ToString();
                            btn.Visible = true;
                            if (i + 1 < 100)     // Esto es para cuando el botón va de 1 a 9 inclusive no sea tan chico
                            {
                                btn.Width = Unit.Parse("40px");
                                btn.Height = Unit.Parse("40px");
                            }
                        }
                    }
                }
                else
                {
                    // Mostrar 9 botones hacia la izquierda y 9 hacia la derecha
                    // o bien los que sea posible en caso de no llegar a 9

                    int CantBucles = 0;

                    Button btnPage10 = (Button)fila.Cells[0].FindControl("cmdPage10");
                    btnPage10.Visible = true;
                    btnPage10.Text = Convert.ToString(grid.PageIndex + 1);

                    // Ubica los 9 botones hacia la izquierda
                    // Linea Original "Previa al cambio": for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                    for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 5; i--)
                    {
                        CantBucles++;
                        if (i >= 0)
                        {
                            Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 - CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                            if (i + 1 < 100)             // Esto es para cuando el botón va de 1 a 9 inclusive no sea tan chico
                            {
                                btn.Width = Unit.Parse("40px");
                                btn.Height = Unit.Parse("40px");
                            }
                        }

                    }

                    CantBucles = 0;
                    // Ubica los 9 botones hacia la derecha
                    // Linea Original "Previa al cambio": for (int i = grid.PageIndex - 1; i <= grid.PageIndex - 9; i--)
                    for (int i = grid.PageIndex + 1; i <= grid.PageIndex + 5; i++)
                    {
                        CantBucles++;
                        if (i <= grid.PageCount - 1)
                        {
                            Button btn = (Button)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 + CantBucles));
                            btn.Visible = true;
                            btn.Text = Convert.ToString(i + 1);
                            if (i + 1 < 100)     // Esto es para cuando el botón va de 1 a 9 inclusive no sea tan chico
                            {
                                btn.Width = Unit.Parse("40px");
                                btn.Height = Unit.Parse("40px");
                            }
                        }
                    }



                }
                Button cmdPage;
                string btnPage = "";
                for (int i = 1; i <= 19; i++)
                {
                    btnPage = "cmdPage" + i.ToString();
                    cmdPage = (Button)fila.Cells[0].FindControl(btnPage);
                    if (cmdPage != null && cmdPage.Visible)
                    {
                        cmdPage.Width = Unit.Parse("40px");
                        cmdPage.Height = Unit.Parse("40px");
                        cmdPage.CssClass = "btn btn-xs btn-default";
                    }
                }



                // busca el boton por el texto para marcarlo como seleccionado
                string btnText = Convert.ToString(grid.PageIndex + 1);
                foreach (Control ctl in fila.Cells[0].FindControl("pnlpager").Controls)
                {
                    if (ctl is Button)
                    {
                        Button btn = (Button)ctl;
                        if (btn.Text.Equals(btnText))
                        {
                            btn.CssClass = "btn btn-info";
                        }
                    }
                }

            }
        }

        protected void grdResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResultados.PageIndex = e.NewPageIndex;
        }
        #endregion

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
            var lst_solicitudes = filtrar();
            int cant_Registros = lst_solicitudes.Count();
            decimal cant_registros_x_vez = 0m;
            int startRowIndex = 0;
            try
            {

                // Esto se realiza para saber el total y de a cuanto se va mostrar el progreso.
                if (cant_Registros < 10000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;

                int cantidad_veces = (int)Math.Ceiling(cant_Registros / cant_registros_x_vez);

                List<clsItemIndicadoresExcel> resultados = new List<clsItemIndicadoresExcel>();

                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados.AddRange(GetData(startRowIndex, Convert.ToInt32(cant_registros_x_vez), lst_solicitudes));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados.Count);
                DataTable dt;
                dt = Functions.ToDataTable(resultados);

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
            catch
            {
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
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

            this.EjecutarScript(updExportaExcel, "hidefrmExportarExcel();");
        }
        #endregion

    }

}
