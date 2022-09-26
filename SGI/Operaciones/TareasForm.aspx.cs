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


            string idTramiteTareaStr = (Request.QueryString["idTramiteTarea"] == null) ? "" : Request.QueryString["idTramiteTarea"].ToString();
            if (String.IsNullOrEmpty(idTramiteTareaStr))
            {
                idTramiteTareaStr = "0";
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
                ddlUsuarioAsignado_tramitetarea.DataSource = CargarTodosLosUsuarios();
                ddlUsuarioAsignado_tramitetarea.DataTextField = "UserName";
                ddlUsuarioAsignado_tramitetarea.DataValueField = "UserId";
                ddlUsuarioAsignado_tramitetarea.DataBind();



                // ddlSFproxima_tarea.DataSource = CargarTodasLasTareas();
                ddlSFproxima_tarea.DataTextField = "nombre_Tarea";
                ddlSFproxima_tarea.DataValueField = "id_Tarea";
                ddlSFproxima_tarea.DataGroupByField = "id_circuito";
                ddlSFproxima_tarea.DataBind();

                //ddlSFtarea.DataSource = CargarTodasLasTareas();
                ddlSFtarea.DataTextField = "nombre_Tarea";
                ddlSFtarea.DataValueField = "id_Tarea";
                ddlSFtarea.DataGroupByField = "id_circuito";
                ddlSFtarea.DataBind();

                ddlCreateUser.DataSource = CargarTodosLosUsuarios();
                ddlCreateUser.DataTextField = "UserName";
                ddlCreateUser.DataValueField = "UserId";
                ddlCreateUser.DataBind();

                using (DGHP_Entities context = new DGHP_Entities())
                {
                    List<ENG_Resultados> ENG_ResultadosList = (from Resultados in context.ENG_Resultados
                                                               select Resultados).ToList();

                    ddlResultado.DataSource = ENG_ResultadosList;
                    ddlResultado.DataTextField = "nombre_resultado";
                    ddlResultado.DataValueField = "id_resultado";
                    ddlResultado.DataBind();
                }

                if (sGI_Tramites_Tareas != null)
                {

                    //aspnet_Users _aspnet_Users = new aspnet_Users();
                    //List<aspnet_Users> aspnet_UsersList = CargarTodosLosUsuarios();
                    //_aspnet_Users = (from aspnet_Users in aspnet_UsersList
                    //                 where aspnet_Users.UserId == sGI_Tramites_Tareas.UsuarioAsignado_tramitetarea
                    //                 select aspnet_Users).FirstOrDefault();


                    //ASOSA No siempre CreateUser este en la lista
                    try
                    {
                        ddlUsuarioAsignado_tramitetarea.SelectedValue = sGI_Tramites_Tareas.UsuarioAsignado_tramitetarea.ToString();
                        ddlCreateUser.SelectedValue = sGI_Tramites_Tareas.CreateUser.ToString();
                        // ddlSFtarea.SelectedIndex = sGI_Tramites_Tareas.id_tarea.ToString();

                        ddlUsuarioAsignado_tramitetarea_SelectedIndexChanged(null, null);
                        ddlSFtarea.SelectedIndex = DropDownListIndex(ddlSFtarea.Items.ToList(), sGI_Tramites_Tareas.id_tarea.ToString());
                        if (sGI_Tramites_Tareas.id_proxima_tarea != null)
                            ddlSFproxima_tarea.SelectedIndex = DropDownListIndex(ddlSFproxima_tarea.Items.ToList(), sGI_Tramites_Tareas.id_proxima_tarea.ToString());
                        //ddlSFproxima_tarea.SelectedItemsValue = sGI_Tramites_Tareas.id_proxima_tarea.ToString();

                        ddlResultado.SelectedValue = sGI_Tramites_Tareas.id_resultado.ToString();
                    }
                    catch (Exception ex)
                    { }



                }


                if (idTramiteTarea > 0)
                {
                    if (sGI_Tramites_Tareas.FechaAsignacion_tramtietarea != null)
                    {
                        calFechaAsignacion_tramtietarea.VisibleDate = (DateTime)sGI_Tramites_Tareas.FechaAsignacion_tramtietarea;
                        calFechaAsignacion_tramtietarea.SelectedDate = (DateTime)sGI_Tramites_Tareas.FechaAsignacion_tramtietarea;
                    }
                    if (sGI_Tramites_Tareas.FechaInicio_tramitetarea != null)
                    {
                        calFechaInicio_tramitetarea.SelectedDate = (DateTime)sGI_Tramites_Tareas.FechaInicio_tramitetarea;
                        calFechaInicio_tramitetarea.VisibleDate = (DateTime)sGI_Tramites_Tareas.FechaInicio_tramitetarea;
                    }
                    if (sGI_Tramites_Tareas.FechaCierre_tramitetarea != null)
                    {
                        calFechaCierre_tramitetarea.SelectedDate = (DateTime)sGI_Tramites_Tareas.FechaCierre_tramitetarea;
                        calFechaCierre_tramitetarea.VisibleDate = (DateTime)sGI_Tramites_Tareas.FechaCierre_tramitetarea;
                    }
                    }
                else
                {
                    calFechaAsignacion_tramtietarea.VisibleDate = DateTime.Today;
                    calFechaAsignacion_tramtietarea.SelectedDate = DateTime.Today;

                    calFechaInicio_tramitetarea.SelectedDate = DateTime.Today;
                    calFechaInicio_tramitetarea.VisibleDate = DateTime.Today;

                    calFechaCierre_tramitetarea.SelectedDate = DateTime.Today;
                    calFechaCierre_tramitetarea.VisibleDate = DateTime.Today;
                    ddlSFproxima_tarea.Enabled = false;
                }

                hdFechaInicio_tramitetarea.Value = calFechaInicio_tramitetarea.SelectedDate.ToShortDateString();
                hdFechaCierre_tramitetarea.Value = calFechaCierre_tramitetarea.SelectedDate.ToShortDateString();
                hdFechaAsignacion_tramtietarea.Value = calFechaAsignacion_tramtietarea.SelectedDate.ToShortDateString();

            }







            DGHP_Entities entities = new DGHP_Entities();
            SGI_SADE_Procesos sGI_SADE_Procesos = (from SADE_Procesos in entities.SGI_SADE_Procesos
                                                   where SADE_Procesos.id_tramitetarea == idTramiteTarea
                                                   && SADE_Procesos.realizado_en_SADE == true
                                                   select SADE_Procesos).FirstOrDefault();




            if (sGI_SADE_Procesos != null && idTramiteTarea > 0)
            {
                ddlResultado.Enabled = false;
                ddlSFtarea.Enabled = false;
            }
            else
            {
                ddlResultado.Enabled = true;
                ddlSFtarea.Enabled = true;
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

        public IEnumerable<ENG_Tareas> CargarTodasLasTareas()
        {
            DGHP_Entities entities = new DGHP_Entities();
            IEnumerable<ENG_Tareas> ENG_TareasList = entities.ENG_Tareas.OrderBy(tarea => tarea.nombre_tarea).ToList();

            return entities.ENG_Tareas.OrderBy(tarea => tarea.ENG_Circuitos.nombre_circuito).ToList();
        }
        #endregion

        #region Events
        protected void calFechaInicio_tramitetarea_SelectionChanged(object sender, EventArgs e)
        {
            lblFecInicio.Text = "Fec.Inicio " + calFechaInicio_tramitetarea.SelectedDate.ToShortDateString();
            hdFechaInicio_tramitetarea.Value = calFechaInicio_tramitetarea.SelectedDate.ToShortDateString();
        }

        protected void calFechaCierre_tramitetarea_SelectionChanged(object sender, EventArgs e)
        {
            lblFecCierre.Text = "Fec.Cierre " + calFechaCierre_tramitetarea.SelectedDate.ToShortDateString();
            hdFechaCierre_tramitetarea.Value = calFechaCierre_tramitetarea.SelectedDate.ToShortDateString();
        }

        protected void calFechaAsignacion_tramtietarea_SelectionChanged(object sender, EventArgs e)
        {
            lblFecAsignacion.Text = "Fec.Asignacion " + calFechaAsignacion_tramtietarea.SelectedDate.ToShortDateString();
            hdFechaAsignacion_tramtietarea.Value = calFechaAsignacion_tramtietarea.SelectedDate.ToShortDateString();
        }

        protected void ddlUsuarioAsignado_tramitetarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            string UsuarioAsignado_tramitetarea = ddlUsuarioAsignado_tramitetarea.SelectedValue.ToString();
            DGHP_Entities entities = new DGHP_Entities();





            var usuario = entities.aspnet_Users.FirstOrDefault(x => x.UserId.ToString() == UsuarioAsignado_tramitetarea);
            List<int> listadoIdTareas = new List<int>();
            foreach (var perfiles in usuario.SGI_PerfilesUsuarios)
                listadoIdTareas.AddRange(perfiles.ENG_Rel_Perfiles_Tareas.Select(perfilTarea => perfilTarea.id_tarea).ToList());



            //List<int> id_perfilList = (from Rel_Usuarios_Perfiles in entities.SGI_Rel_Usuarios_Perfiles
            //                           where Rel_Usuarios_Perfiles.userid.ToString() == UsuarioAsignado_tramitetarea
            //                           select Rel_Usuarios_Perfiles.id_perfil).ToList();



            //int id_perfil = 89;//TODO ASOSA PREGUNTAR entities.SGI_Rel_Usuarios_Perfiles NO EXISTE

            //List<int> id_tareaList = (from Perfiles_Tareas in entities.ENG_Rel_Perfiles_Tareas
            //                          where Perfiles_Tareas.id_perfil == 89 //id_perfil
            //                          select Perfiles_Tareas.id_tarea).ToList();



            //List<ENG_Tareas> ENG_TareasList = entities.ENG_Tareas.Where(x => listadoIdTareas.Contains(x.id_tarea)).OrderBy(x => x.nombre_tarea).ToList();

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

            //DDLGroupedList = ENG_TareasList.Select(x => new DDLGrouped
            //{
            //    DataValueField = x.id_tarea,
            //    DataTextField = x.nombre_tarea,
            //    DataGroupByField = x.id_circuito.ToString()
            //}).ToList();


            ddlSFproxima_tarea.DataSource = DDLGroupedList;
            ddlSFproxima_tarea.DataTextField = "DataTextField";
            ddlSFproxima_tarea.DataValueField = "DataValueField";
            ddlSFproxima_tarea.DataGroupByField = "DataGroupByField";
            ddlSFproxima_tarea.DataBind();

            ddlSFtarea.DataSource = DDLGroupedList;
            ddlSFtarea.DataTextField = "DataTextField";
            ddlSFtarea.DataValueField = "DataValueField";
            ddlSFtarea.DataGroupByField = "DataGroupByField";
            ddlSFtarea.DataBind();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
          


            SGI_Tramites_Tareas tramiteTarea = new SGI_Tramites_Tareas();
            SGI_Tramites_Tareas_HAB sgi_Tramites_Tareas_HAB = new SGI_Tramites_Tareas_HAB();
            SGI_Tramites_Tareas_TRANSF sgi_Tramites_Tareas_TRANSF = new SGI_Tramites_Tareas_TRANSF();

            using (DGHP_Entities entities = new DGHP_Entities())
            {
                int idTramiteTarea = int.Parse(hdidTramiteTarea.Value);


                if (idTramiteTarea == 0)
                {
                    tramiteTarea.id_tramitetarea = entities.SGI_Tramites_Tareas.Max(x => x.id_tramitetarea) + 1;
                    tramiteTarea.id_proxima_tarea = null;
                }
                else
                {
                    tramiteTarea.id_tramitetarea = idTramiteTarea;
                    if (chkproxima_tarea.Checked)
                        tramiteTarea.id_proxima_tarea = null;
                    else    
                        tramiteTarea.id_proxima_tarea = int.Parse(ddlSFproxima_tarea.Value);
                }
                tramiteTarea.FechaInicio_tramitetarea = DateTime.Parse(hdFechaInicio_tramitetarea.Value);
                tramiteTarea.FechaCierre_tramitetarea = DateTime.Parse(hdFechaCierre_tramitetarea.Value);
                if(chkFechaCierre_tramitetarea.Checked)
                    tramiteTarea.FechaCierre_tramitetarea = null;
                tramiteTarea.FechaAsignacion_tramtietarea = DateTime.Parse(hdFechaAsignacion_tramtietarea.Value);

                tramiteTarea.UsuarioAsignado_tramitetarea = Guid.Parse(ddlUsuarioAsignado_tramitetarea.SelectedValue);
                tramiteTarea.CreateUser = Guid.Parse(ddlCreateUser.SelectedValue);



                tramiteTarea.id_tarea = int.Parse(ddlSFtarea.Value);
                tramiteTarea.id_resultado = int.Parse(ddlResultado.SelectedValue);

                entities.SGI_Tramites_Tareas.AddOrUpdate(tramiteTarea);
                #region SGI_Tramites_Tareas_HAB
                if (hdHAB_TRANSF.Value == "H")
                {
                    int idSolicitud = int.Parse(hdidSolicitud.Value);
                    sgi_Tramites_Tareas_HAB.id_tramitetarea = tramiteTarea.id_tramitetarea;
                    sgi_Tramites_Tareas_HAB.id_rel_tt_HAB = entities.SGI_Tramites_Tareas_HAB.Max(x => x.id_rel_tt_HAB) + 1;
                    sgi_Tramites_Tareas_HAB.id_solicitud = idSolicitud;
                    entities.SGI_Tramites_Tareas_HAB.Add(sgi_Tramites_Tareas_HAB);
                }
                if (hdHAB_TRANSF.Value == "T")
                {
                    int idSolicitud = int.Parse(hdidSolicitud.Value);
                    sgi_Tramites_Tareas_TRANSF.id_tramitetarea = tramiteTarea.id_tramitetarea;
                    sgi_Tramites_Tareas_TRANSF.id_rel_tt_TRANSF = entities.SGI_Tramites_Tareas_TRANSF.Max(x => x.id_rel_tt_TRANSF) + 1;
                    sgi_Tramites_Tareas_TRANSF.id_solicitud = idSolicitud;
                    entities.SGI_Tramites_Tareas_TRANSF.Add(sgi_Tramites_Tareas_TRANSF);
                }
                #endregion






                entities.SaveChanges();
                Response.Redirect("~/Operaciones/AdministrarTareasDeUnaSolicitud.aspx?idSolicitud=" + hdidSolicitud.Value);
            }

            #region Transaccion
            DGHP_Entities context = new DGHP_Entities();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {

                    context.SGI_Tramites_Tareas.AddOrUpdate(tramiteTarea);
                    context.SGI_Tramites_Tareas_HAB.Add(sgi_Tramites_Tareas_HAB);


                    // Guardamos todos los cambios
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

        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/AdministrarTareasDeUnaSolicitud.aspx?idSolicitud=" + hdidSolicitud.Value);
        }
        #endregion


    }
}