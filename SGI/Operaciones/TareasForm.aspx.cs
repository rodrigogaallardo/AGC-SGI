using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Ajax.Utilities;
using SGI.Model;
using SGI.Seguridad;
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
using static SGI.Model.Engine;
using static Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAlrunsRecord;

namespace SGI.Operaciones
{
    public partial class TareasForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion

            DGHP_Entities entities = new DGHP_Entities();
            string idTramiteTareaStr = (Request.QueryString["idTramiteTarea"] == null) ? "" : Request.QueryString["idTramiteTarea"].ToString();
            if (String.IsNullOrEmpty(idTramiteTareaStr))
            {
                Response.Redirect("~/Operaciones/AdministrarTareasDeUnaSolicitud.aspx");
            }
            int idTramiteTarea = int.Parse(idTramiteTareaStr);
            hdidTramiteTarea.Value = idTramiteTareaStr;


            string idSolicitudStr = (Request.QueryString["idSolicitud"] == null) ? "" : Request.QueryString["idSolicitud"].ToString();
            if (String.IsNullOrEmpty(idSolicitudStr))
            {
                idSolicitudStr = "0";
            }
            int idSolicitud = int.Parse(idSolicitudStr);
            hdidSolicitud.Value = idSolicitudStr;

            string hAB_tRANSF = (Request.QueryString["hAB_tRANSF"] == null) ? "" : Request.QueryString["hAB_tRANSF"].ToString();
            hdHAB_TRANSF.Value = hAB_tRANSF;

            SGI_Tramites_Tareas sGI_Tramites_Tareas = new SGI_Tramites_Tareas();
            sGI_Tramites_Tareas = BuscarTramiteTarea(idTramiteTarea);
            if (!IsPostBack)
            {
                List<ENG_Circuitos> ENG_CircuitosList = CargarTodasLosCircuitos();
                ddlUsuarioAsignado_tramitetarea.DataSource = CargarTodosLosUsuarios();
                ddlUsuarioAsignado_tramitetarea.DataTextField = "UserName";
                ddlUsuarioAsignado_tramitetarea.DataValueField = "UserId";
                ddlUsuarioAsignado_tramitetarea.DataBind();
                // ddlUsuarioAsignado_tramitetarea_SelectedIndexChanged(null, null);

                ddlResultado.Enabled = false;

                if (sGI_Tramites_Tareas != null)
                {
                    //ASOSA No siempre CreateUser este en la lista
                    try
                    {
                        ddlUsuarioAsignado_tramitetarea.SelectedValue = sGI_Tramites_Tareas.UsuarioAsignado_tramitetarea.ToString();
                        ddlUsuarioAsignado_tramitetarea_SelectedIndexChanged(null, null);
                        // ddlSFtarea.SelectedIndex = sGI_Tramites_Tareas.id_tarea.ToString();

                        ddlUsuarioAsignado_tramitetarea_SelectedIndexChanged(null, null);
                        //ddlSFtarea.SelectedIndex = DropDownListIndex(ddlSFtarea.Items.ToList(), sGI_Tramites_Tareas.id_tarea.ToString());
                        //if (sGI_Tramites_Tareas.id_proxima_tarea != null)
                        //    ddlSFproxima_tarea.SelectedIndex = DropDownListIndex(ddlSFproxima_tarea.Items.ToList(), sGI_Tramites_Tareas.id_proxima_tarea.ToString());
                    }
                    catch (Exception ex)
                    { }

                }

                if (idTramiteTarea > 0)
                {
                    ENG_Tareas eNG_Tareas = (from t in entities.ENG_Tareas
                                             where t.id_tarea == sGI_Tramites_Tareas.id_tarea
                                             select t).FirstOrDefault();

                    ENG_Circuitos ENG_Circuitos = (from c in entities.ENG_Circuitos
                                                   where c.id_circuito == eNG_Tareas.id_circuito
                                                   select c).FirstOrDefault();

                    ddlCircuitoActual.DataSource = ENG_CircuitosList;
                    ddlCircuitoActual.DataTextField = "nombre_circuito";
                    ddlCircuitoActual.DataValueField = "id_circuito";
                    ddlCircuitoActual.DataBind();
                    ddlCircuitoActual.SelectedValue = ENG_Circuitos.id_circuito.ToString();
                    ddlCircuitoActual.Enabled = false;

                    if (sGI_Tramites_Tareas.id_proxima_tarea != null)
                    {
                        ddlproxima_tarea.SelectedValue = sGI_Tramites_Tareas.id_proxima_tarea.ToString();
                    }
                    else
                    {
                        ddlproxima_tarea.SelectedIndex = 0;
                    }

                    if (sGI_Tramites_Tareas.UsuarioAsignado_tramitetarea == null)
                        chkUsuario.Checked = true;
                    else
                        chkUsuario.Checked = false;
                    chkUsuario_CheckedChanged(null, null);

                    if (sGI_Tramites_Tareas.id_proxima_tarea == null)
                        chkproxima_tarea.Checked = true;
                    else
                        chkproxima_tarea.Checked = false;
                    chkproxima_tarea_CheckedChanged(null, null);


                    List<ENG_Tareas> ENG_TareasList = new List<ENG_Tareas>();
                    ENG_TareasList.Add(eNG_Tareas);
                    ddltarea.DataSource = ENG_TareasList;
                    ddltarea.DataTextField = "nombre_Tarea";
                    ddltarea.DataValueField = "id_Tarea";
                    ddltarea.DataBind();
                    ddltarea.Enabled = false;
                    ddltarea.SelectedIndex = 0;
                    ddltarea_SelectedIndexChanged(null, null);

                    ddlResultado.SelectedValue = sGI_Tramites_Tareas.id_resultado.ToString();
                    ddlResultado_SelectedIndexChanged(null, null);

                    if (sGI_Tramites_Tareas.FechaAsignacion_tramtietarea != null)
                    {
                        calFechaAsignacion_tramtietarea.VisibleDate = DateTime.Parse(((DateTime)sGI_Tramites_Tareas.FechaAsignacion_tramtietarea).ToString("dd-MM-yyyy"));
                        calFechaAsignacion_tramtietarea.SelectedDate = DateTime.Parse(((DateTime)sGI_Tramites_Tareas.FechaAsignacion_tramtietarea).ToString("dd-MM-yyyy"));
                    }
                    if (sGI_Tramites_Tareas.FechaInicio_tramitetarea != null)
                    {
                        calFechaInicio_tramitetarea.SelectedDate = DateTime.Parse(((DateTime)sGI_Tramites_Tareas.FechaInicio_tramitetarea).ToString("dd-MM-yyyy"));
                        calFechaInicio_tramitetarea.VisibleDate = DateTime.Parse(((DateTime)sGI_Tramites_Tareas.FechaInicio_tramitetarea).ToString("dd-MM-yyyy"));
                    }

                    if (sGI_Tramites_Tareas.FechaCierre_tramitetarea != null)
                    {
                        calFechaCierre_tramitetarea.SelectedDate = DateTime.Parse(((DateTime)sGI_Tramites_Tareas.FechaCierre_tramitetarea).ToString("dd-MM-yyyy"));
                        calFechaCierre_tramitetarea.VisibleDate = DateTime.Parse(((DateTime)sGI_Tramites_Tareas.FechaCierre_tramitetarea).ToString("dd-MM-yyyy"));
                    }
                    else
                    {
                        calFechaCierre_tramitetarea.SelectedDate = DateTime.Today;
                        calFechaCierre_tramitetarea.VisibleDate = DateTime.Today;
                        chkFechaCierre_tramitetarea.Checked = true;
                    }
                    chkFechaCierre_tramitetarea_CheckedChanged(null, null);
                    hdCreateUser.Value = sGI_Tramites_Tareas.CreateUser.ToString();

                }
                else
                {
                    ddlCircuitoActual.DataSource = ENG_CircuitosList;
                    ddlCircuitoActual.DataTextField = "nombre_circuito";
                    ddlCircuitoActual.DataValueField = "id_circuito";
                    ddlCircuitoActual.DataBind();
                    string id_circuito = (Request.QueryString["id_circuito"] == null) ? "" : Request.QueryString["id_circuito"].ToString();
                    if (String.IsNullOrEmpty(id_circuito))
                    {
                        ddlCircuitoActual.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlCircuitoActual.SelectedValue = id_circuito;
                    }

                    ddlCircuitoActual_SelectedIndexChanged(null, null);

                    ddltarea.SelectedIndex = 0;
                    ddltarea_SelectedIndexChanged(null, null);
                    //ddlResultado.SelectedValue = sGI_Tramites_Tareas.id_resultado.ToString();

                    ddlproxima_tarea.Enabled = false;
                    chkproxima_tarea.Checked = true;
                    chkproxima_tarea.Enabled = false;

                    calFechaAsignacion_tramtietarea.VisibleDate = DateTime.Today;
                    calFechaAsignacion_tramtietarea.SelectedDate = DateTime.Today;

                    calFechaInicio_tramitetarea.SelectedDate = DateTime.Today;
                    calFechaInicio_tramitetarea.VisibleDate = DateTime.Today;

                    calFechaCierre_tramitetarea.SelectedDate = DateTime.Today;
                    calFechaCierre_tramitetarea.VisibleDate = DateTime.Today;
                }

                hdFechaInicio_tramitetarea.Value = calFechaInicio_tramitetarea.SelectedDate.ToShortDateString();
                hdFechaCierre_tramitetarea.Value = calFechaCierre_tramitetarea.SelectedDate.ToShortDateString();
                hdFechaAsignacion_tramtietarea.Value = calFechaAsignacion_tramtietarea.SelectedDate.ToShortDateString();

            }

        }

        #region Methods
        public int DropDownListIndex(List<Syncfusion.JavaScript.Web.DropDownListItem> dropdownList, string search)
        {

            int indexVal = dropdownList.Select((item, i) => new { Item = item, Index = i })
                .First(x => x.Item.Value == search).Index;
            return indexVal;
        }
        public SGI_Tramites_Tareas BuscarTramiteTarea(int idTramiteTarea)
        {
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                SGI_Tramites_Tareas sGI_Tramites_Tareas = (from tareas in entities.SGI_Tramites_Tareas
                                                           where tareas.id_tramitetarea == idTramiteTarea
                                                           select tareas).FirstOrDefault();
                return sGI_Tramites_Tareas; //TODO
            }
        }

        public SGI_SADE_Procesos BuscarProcesos(int idTramiteTarea)
        {
            using (DGHP_Entities entities = new DGHP_Entities())
            {

                SGI_SADE_Procesos sGI_SADE_Procesos = (from SADE_Procesos in entities.SGI_SADE_Procesos
                                                       where SADE_Procesos.id_tramitetarea == idTramiteTarea
                                                       && SADE_Procesos.realizado_en_SADE == true
                                                       select SADE_Procesos).FirstOrDefault();

                return sGI_SADE_Procesos;
            }
        }

        public List<aspnet_Users> CargarTodosLosUsuarios()
        {
            DGHP_Entities db = new DGHP_Entities();
            //List<aspnet_Users> qq = (from mem in db.aspnet_Membership
            //                        join usu in db.aspnet_Users on mem.UserId equals usu.UserId
            //                        join profile in db.SGI_Profiles on usu.UserId equals profile.userid
            //                        where mem.ApplicationId == Constants.ApplicationId
            //                        orderby (usu.UserName)
            //                        select usu).ToList();

            // foreach (var perfiles in usuario.SGI_PerfilesUsuarios)
            //    listadoIdTareas.AddRange(perfiles.ENG_Rel_Perfiles_Tareas.Select(perfilTarea => perfilTarea.id_tarea).ToList());

            List<aspnet_Users> q = (from usu in db.aspnet_Users
                                    join profile in db.SGI_Profiles on usu.UserId equals profile.userid
                                    where usu.ApplicationId == Constants.ApplicationId
                                    && usu.SGI_PerfilesUsuarios.Where(x => x.ENG_Rel_Perfiles_Tareas.Count() > 0).Count() > 0
                                    && usu.aspnet_Membership.IsApproved
                                    orderby (usu.UserName)
                                    select usu).ToList();
            return q;
        }

        public List<ENG_Tareas> CargarTodasLasTareasByid_circuito(int id_circuito)
        {
            DGHP_Entities entities = new DGHP_Entities();

            List<ENG_Tareas> qq = (from mem in entities.ENG_Tareas
                                   where mem.id_circuito == id_circuito
                                   orderby (mem.nombre_tarea)
                                   select mem).ToList();
            return qq;
        }
        public List<ENG_Circuitos> CargarTodasLosCircuitos()
        {
            DGHP_Entities entities = new DGHP_Entities();
            List<ENG_Circuitos> ENG_CircuitosList = entities.ENG_Circuitos.OrderBy(c => c.nombre_circuito).ToList();
            return ENG_CircuitosList;
        }
        #endregion

        #region Events
        protected void calFechaInicio_tramitetarea_SelectionChanged(object sender, EventArgs e)
        {
            hdFechaInicio_tramitetarea.Value = calFechaInicio_tramitetarea.SelectedDate.ToShortDateString();
        }

        protected void calFechaCierre_tramitetarea_SelectionChanged(object sender, EventArgs e)
        {
            hdFechaCierre_tramitetarea.Value = calFechaCierre_tramitetarea.SelectedDate.ToShortDateString();
        }

        protected void calFechaAsignacion_tramtietarea_SelectionChanged(object sender, EventArgs e)
        {
            hdFechaAsignacion_tramtietarea.Value = calFechaAsignacion_tramtietarea.SelectedDate.ToShortDateString();
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {

            SGI_Tramites_Tareas tramiteTarea = new SGI_Tramites_Tareas();
            SGI_Tramites_Tareas_HAB sgi_Tramites_Tareas_HAB = new SGI_Tramites_Tareas_HAB();
            SGI_Tramites_Tareas_TRANSF sgi_Tramites_Tareas_TRANSF = new SGI_Tramites_Tareas_TRANSF();
            DGHP_Entities context = new DGHP_Entities();
            //using (DGHP_Entities entities = new DGHP_Entities())
            //{
            int idTramiteTarea = int.Parse(hdidTramiteTarea.Value);

            if (idTramiteTarea == 0)
            {
                tramiteTarea.id_tramitetarea = context.SGI_Tramites_Tareas.Max(x => x.id_tramitetarea) + 1;
                tramiteTarea.id_proxima_tarea = null;
                tramiteTarea.CreateUser = Guid.Parse(Membership.GetUser().ProviderUserKey.ToString());
            }
            else
            {
                tramiteTarea.id_tramitetarea = idTramiteTarea;
                if (chkproxima_tarea.Checked)
                    tramiteTarea.id_proxima_tarea = null;
                else
                {
                    if (ddlproxima_tarea.SelectedValue == null)
                    {
                        ScriptManager sm = ScriptManager.GetCurrent(this);
                        string cadena = "Debe seleccionar una Proxima Tarea o Checkear 'No Establecer'";
                        string script = string.Format("alert('{0}');", cadena);
                        ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);
                        return;
                    }

                    tramiteTarea.id_proxima_tarea = int.Parse(ddlproxima_tarea.SelectedValue);
                }
                tramiteTarea.CreateUser = Guid.Parse(hdCreateUser.Value);
            }

            tramiteTarea.FechaInicio_tramitetarea = DateTime.Parse(hdFechaInicio_tramitetarea.Value);
            tramiteTarea.FechaCierre_tramitetarea = DateTime.Parse(hdFechaCierre_tramitetarea.Value);
            if (chkFechaCierre_tramitetarea.Checked)
                tramiteTarea.FechaCierre_tramitetarea = null;
            tramiteTarea.FechaAsignacion_tramtietarea = DateTime.Parse(hdFechaAsignacion_tramtietarea.Value);

            if (chkUsuario.Checked == false)
            {
                tramiteTarea.UsuarioAsignado_tramitetarea = Guid.Parse(ddlUsuarioAsignado_tramitetarea.SelectedValue);
            }
            else
            {
                tramiteTarea.UsuarioAsignado_tramitetarea = null;
            }

            tramiteTarea.id_tarea = int.Parse(ddltarea.SelectedValue);
            tramiteTarea.id_resultado = int.Parse(ddlResultado.SelectedValue);
            //}

            #region Transaccion

            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.SGI_Tramites_Tareas.AddOrUpdate(tramiteTarea);
                    #region SGI_Tramites_Tareas_HAB
                    if (hdHAB_TRANSF.Value == "H")
                    {
                        int idSolicitud = int.Parse(hdidSolicitud.Value);
                        sgi_Tramites_Tareas_HAB.id_tramitetarea = tramiteTarea.id_tramitetarea;
                        sgi_Tramites_Tareas_HAB.id_rel_tt_HAB = context.SGI_Tramites_Tareas_HAB.Max(x => x.id_rel_tt_HAB) + 1;
                        sgi_Tramites_Tareas_HAB.id_solicitud = idSolicitud;
                        context.SGI_Tramites_Tareas_HAB.Add(sgi_Tramites_Tareas_HAB);
                    }
                    if (hdHAB_TRANSF.Value == "T")
                    {
                        int idSolicitud = int.Parse(hdidSolicitud.Value);
                        sgi_Tramites_Tareas_TRANSF.id_tramitetarea = tramiteTarea.id_tramitetarea;
                        sgi_Tramites_Tareas_TRANSF.id_rel_tt_TRANSF = context.SGI_Tramites_Tareas_TRANSF.Max(x => x.id_rel_tt_TRANSF) + 1;
                        sgi_Tramites_Tareas_TRANSF.id_solicitud = idSolicitud;
                        context.SGI_Tramites_Tareas_TRANSF.Add(sgi_Tramites_Tareas_TRANSF);
                    }
                    #endregion
                    context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw ex;
                }
            }
            #endregion
            Response.Redirect("~/Operaciones/AdministrarTareasDeUnaSolicitud.aspx?idSolicitud=" + hdidSolicitud.Value);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/AdministrarTareasDeUnaSolicitud.aspx?idSolicitud=" + hdidSolicitud.Value);
        }

        #endregion

        protected void ddlCircuitoActual_SelectedIndexChanged(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            int id_circuito = int.Parse(ddlCircuitoActual.SelectedValue);
            List<ENG_Tareas> eNG_TareasList = new List<ENG_Tareas>();
            eNG_TareasList = (from t in db.ENG_Tareas
                              where t.id_circuito == id_circuito
                              select t).ToList();

            ddltarea.DataSource = eNG_TareasList;
            ddltarea.DataTextField = "nombre_tarea";
            ddltarea.DataValueField = "id_tarea";
            ddltarea.DataBind();
        }

        protected void ddlUsuarioAsignado_tramitetarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            string UsuarioAsignado_tramitetarea = ddlUsuarioAsignado_tramitetarea.SelectedValue.ToString();
            DGHP_Entities entities = new DGHP_Entities();

            var usuario = entities.aspnet_Users.FirstOrDefault(x => x.UserId.ToString() == UsuarioAsignado_tramitetarea);
            List<int> listadoIdTareas = new List<int>();
            foreach (var perfiles in usuario.SGI_PerfilesUsuarios)
                listadoIdTareas.AddRange(perfiles.ENG_Rel_Perfiles_Tareas.Select(perfilTarea => perfilTarea.id_tarea).ToList());

            #region MyRegion
            DGHP_Entities db = new DGHP_Entities();
            List<DDLGrouped> DDLGroupedList = new List<DDLGrouped>();
            DDLGroupedList = (from mem in db.ENG_Tareas
                              join usu in db.ENG_Circuitos
                                             on mem.id_circuito equals usu.id_circuito
                              where listadoIdTareas.Contains(mem.id_tarea)
                              select new DDLGrouped
                              {
                                  DataValueField = mem.id_tarea,
                                  DataTextField = mem.nombre_tarea,
                                  DataGroupByField = usu.nombre_grupo.ToString()
                              }).ToList();
            #endregion
        }



        protected void ddltarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlResultado.Enabled = true;
            using (DGHP_Entities context = new DGHP_Entities())
            {
                //List<ENG_Resultados> ENG_ResultadosList = (from Resultados in context.ENG_Resultados
                //                                           select Resultados).ToList();
                Engine.Tarea tarea = Engine.Tarea.Get(int.Parse(ddltarea.SelectedValue), 0);

                ddlResultado.DataSource = tarea.Resultados;
                ddlResultado.DataTextField = "nombre_resultado";
                ddlResultado.DataValueField = "id_resultado";
                ddlResultado.DataBind();

                ddlResultado_SelectedIndexChanged(null, null);
            }
        }

        protected void ddlResultado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlResultado.SelectedIndex == -1)
                return;
            int id_resultadoSelect = int.Parse(ddlResultado.SelectedValue);
            int id_tramitetarea = int.Parse(hdidTramiteTarea.Value);
            int id_tarea = int.Parse(ddltarea.SelectedValue);

            int id_proxima_tarea;
            if (int.Parse(hdidTramiteTarea.Value) == 0)
                id_proxima_tarea = 0;
            else
            {
                SGI_Tramites_Tareas sGI_Tramites_Tareas = new SGI_Tramites_Tareas();
                sGI_Tramites_Tareas = BuscarTramiteTarea(id_tramitetarea);
                if (sGI_Tramites_Tareas.id_proxima_tarea != null)
                    id_proxima_tarea = (int)sGI_Tramites_Tareas.id_proxima_tarea;
                else
                    id_proxima_tarea = 0;
            }
            CargarProximasTareas(id_resultadoSelect, id_tarea, id_tramitetarea, id_proxima_tarea);
        }
        private void CargarProximasTareas(int id_resultado, int id_tarea, int id_tramitetarea, int id_proxima_tarea)
        {
            List<Tarea> TareaListAux = Engine.GetTareasSiguientes(id_resultado, id_tarea, id_tramitetarea);
            if (TareaListAux.Count() < 1)
                return;
            ddlproxima_tarea.DataSource = TareaListAux;
            ddlproxima_tarea.DataTextField = "nombre_tarea";
            ddlproxima_tarea.DataValueField = "id_tarea";
            ddlproxima_tarea.DataBind();
            //setea el valor leido desde la tarea ya almacenada
            if (id_proxima_tarea > 0)
            {
                Tarea TareaAux = (from t in TareaListAux
                                  where t.id_tarea == id_proxima_tarea
                                  select t).FirstOrDefault();
                if (TareaAux == null)
                {
                    ddlproxima_tarea.SelectedIndex = 0;
                }
                else
                {
                    ddlproxima_tarea.SelectedValue = id_proxima_tarea.ToString();
                }
            }

        }

        protected void chkproxima_tarea_CheckedChanged(object sender, EventArgs e)
        {
            ddlproxima_tarea.Enabled = !chkproxima_tarea.Checked;
        }

        protected void chkUsuario_CheckedChanged(object sender, EventArgs e)
        {
            ddlUsuarioAsignado_tramitetarea.Enabled = !chkUsuario.Checked;
        }

        protected void chkFechaCierre_tramitetarea_CheckedChanged(object sender, EventArgs e)
        {
            calFechaCierre_tramitetarea.Enabled = !chkFechaCierre_tramitetarea.Checked;
        }
    }
}