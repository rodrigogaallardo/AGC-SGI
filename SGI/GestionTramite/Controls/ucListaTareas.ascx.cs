using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;

namespace SGI.GestionTramite.Controls
{
    public class clsRowitemTarea
    {
        public int id_tramitetarea { get; set; }
        public int id_solicitud { get; set; }
        public int id_tarea { get; set; }
        public string Descripcion { get; set; }
        public Nullable<DateTime> FechaCreacion { get; set; }
        public Nullable<DateTime> FechaAsignacion { get; set; }
        public Nullable<DateTime> FechaFinalizacion { get; set; }
        public Nullable<Guid> UsuarioAsignado { get; set; }
        public string Username { get; set; }
        public string ApenomUsuario { get; set; }
        public string form_aspx { get; set; }
        public string codigo_circuito { get; set; }
    }
    public partial class ucListaTareas : System.Web.UI.UserControl
    {
        
        public int id_solicitud
        {
            get
            {
                int _id_solicitud = (ViewState["_id_solicitud"] != null ? Convert.ToInt32(ViewState["_id_solicitud"]) : 0);
                
                return _id_solicitud;
            }
            set
            {
                ViewState["_id_solicitud"] = value;
            }
        }

        public int id_grupotramite
        {
            get
            {
                int _id_grupotramite = (ViewState["_id_grupotramite"] != null ? Convert.ToInt32(ViewState["_id_grupotramite"]) : 0);

                return _id_grupotramite;
            }
            set
            {
                ViewState["_id_grupotramite"] = value;
            }
        }

        public void LoadData(int id_solicitud)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud);
        }
        
        public void LoadData(int id_grupotramite, int id_solicitud)
        {
            this.id_solicitud = id_solicitud;
            this.id_grupotramite = id_grupotramite;
        }       
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Refresh()
        {
            grdTareas.DataBind();
        }

        public List<clsRowitemTarea> GetTareas()
        {
            List<clsRowitemTarea> lstTareas = new List<clsRowitemTarea>();
            
            if (this.id_solicitud > 0)
            {
                DGHP_Entities db = new DGHP_Entities();

                if (this.id_grupotramite == (int)Constants.GruposDeTramite.HAB)
                {
                    lstTareas = (from tt in db.SGI_Tramites_Tareas
                                 join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                 join uC in db.Usuario on tt_hab.SSIT_Solicitudes.CreateUser equals uC.UserId
                                 into uCleftjoin
                                 from userC in uCleftjoin.DefaultIfEmpty()
                                 join aspu in db.aspnet_Users on userC.UserId equals aspu.UserId
                                 into aspuleftoin
                                 from asU in aspuleftoin.DefaultIfEmpty()
                                 join p in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals p.userid
                                 into pleftjoin
                                 from prof in pleftjoin.DefaultIfEmpty()
                                 where tt_hab.id_solicitud == this.id_solicitud
                                 orderby tt.FechaInicio_tramitetarea
                                 select new clsRowitemTarea
                                 {
                                     id_tramitetarea = tt.id_tramitetarea,
                                     id_solicitud = tt_hab.id_solicitud,
                                     id_tarea = tt.id_tarea,
                                     Descripcion = tt.ENG_Tareas.nombre_tarea,
                                     FechaCreacion = tt.FechaInicio_tramitetarea,
                                     FechaAsignacion = tt.FechaAsignacion_tramtietarea,
                                     FechaFinalizacion = tt.FechaCierre_tramitetarea,
                                     UsuarioAsignado = tt.UsuarioAsignado_tramitetarea,
                                     Username = (prof != null ? prof.aspnet_Users.UserName : (tt.ENG_Tareas.formulario_tarea == null ? asU.UserName : null)),
                                     ApenomUsuario = (prof != null ? prof.Nombres + " " + prof.Apellido : (tt.ENG_Tareas.formulario_tarea == null ? userC.Nombre + " " + userC.Apellido : null)),
                                     form_aspx = tt.ENG_Tareas.formulario_tarea,
                                     codigo_circuito = tt.ENG_Tareas.ENG_Circuitos.nombre_grupo
                                 }).ToList();
                }
                else if (this.id_grupotramite == (int)Constants.GruposDeTramite.CP)
                {
                    lstTareas = (from tt in db.SGI_Tramites_Tareas
                                 join tt_cp in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                                 join uC in db.Usuario on tt_cp.CPadron_Solicitudes.CreateUser equals uC.UserId
                                 into uCleftjoin
                                 from userC in uCleftjoin.DefaultIfEmpty()
                                 join aspu in db.aspnet_Users on userC.UserId equals aspu.UserId
                                 into aspuleftoin
                                 from asU in aspuleftoin.DefaultIfEmpty()
                                 join p in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals p.userid
                                 into pleftjoin
                                 from prof in pleftjoin.DefaultIfEmpty()

                                 where tt_cp.id_cpadron == this.id_solicitud
                                 orderby tt.FechaInicio_tramitetarea
                                 select new clsRowitemTarea
                                 {
                                     id_tramitetarea = tt.id_tramitetarea,
                                     id_solicitud = tt_cp.id_cpadron,
                                     id_tarea = tt.id_tarea,
                                     Descripcion = tt.ENG_Tareas.nombre_tarea,
                                     FechaCreacion = tt.FechaInicio_tramitetarea,
                                     FechaAsignacion = tt.FechaAsignacion_tramtietarea,
                                     FechaFinalizacion = tt.FechaCierre_tramitetarea,
                                     UsuarioAsignado = tt.UsuarioAsignado_tramitetarea,
                                     Username = (prof != null ? prof.aspnet_Users.UserName : (tt.ENG_Tareas.formulario_tarea == null ? asU.UserName : null)),
                                     ApenomUsuario = (prof != null ? prof.Nombres + " " + prof.Apellido : (tt.ENG_Tareas.formulario_tarea == null ? userC.Nombre + " " + userC.Apellido : null)),
                                     form_aspx = tt.ENG_Tareas.formulario_tarea,
                                     codigo_circuito = tt.ENG_Tareas.ENG_Circuitos.nombre_grupo
                                 }).ToList();

                }
                else if (this.id_grupotramite == (int)Constants.GruposDeTramite.TR)
                {
                    lstTareas = (from tt in db.SGI_Tramites_Tareas
                                 join tt_transf in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_transf.id_tramitetarea
                                 join uC in db.Usuario on tt_transf.Transf_Solicitudes.CreateUser equals uC.UserId
                                 into uCleftjoin
                                 from userC in uCleftjoin.DefaultIfEmpty()
                                 join aspu in db.aspnet_Users on userC.UserId equals aspu.UserId
                                 into aspuleftoin
                                 from asU in aspuleftoin.DefaultIfEmpty()
                                 join p in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals p.userid
                                 into pleftjoin
                                 from prof in pleftjoin.DefaultIfEmpty()
                                 where tt_transf.id_solicitud == this.id_solicitud
                                 orderby tt.id_tramitetarea
                                 select new clsRowitemTarea
                                 {
                                     id_tramitetarea = tt.id_tramitetarea,
                                     id_solicitud = tt_transf.id_solicitud,
                                     id_tarea = tt.id_tarea,
                                     Descripcion = tt.ENG_Tareas.nombre_tarea,
                                     FechaCreacion = tt.FechaInicio_tramitetarea,
                                     FechaAsignacion = tt.FechaAsignacion_tramtietarea,
                                     FechaFinalizacion = tt.FechaCierre_tramitetarea,
                                     UsuarioAsignado = tt.UsuarioAsignado_tramitetarea,
                                     Username = (prof != null ? prof.aspnet_Users.UserName : (tt.ENG_Tareas.formulario_tarea == null ? asU.UserName : null)),
                                     ApenomUsuario = (prof != null ? prof.Nombres + " " + prof.Apellido : (tt.ENG_Tareas.formulario_tarea == null ? userC.Nombre + " " + userC.Apellido : null)),
                                     form_aspx = tt.ENG_Tareas.formulario_tarea,
                                     codigo_circuito = tt.ENG_Tareas.ENG_Circuitos.nombre_grupo
                                 }).ToList();
                }

                db.Dispose();
            }
            
            return lstTareas;


        }

        protected void grdTareas_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                clsRowitemTarea tramiteTarea = (clsRowitemTarea)e.Row.DataItem;

                Label lblTramiteTarea = (Label)e.Row.FindControl("lblTramiteTarea");
                HyperLink lnkTramiteTarea = (HyperLink)e.Row.FindControl("lnkTramiteTarea");

                if (string.IsNullOrEmpty(tramiteTarea.form_aspx))
                    lnkTramiteTarea.Visible = false;
                else
                {
                    lnkTramiteTarea.Visible = true;
                    lnkTramiteTarea.NavigateUrl = string.Format("~/GestionTramite/Tareas/{0}?id={1}", tramiteTarea.form_aspx, tramiteTarea.id_tramitetarea);
                }

                lblTramiteTarea.Visible = !lnkTramiteTarea.Visible;

                //Si la tarea no tiene usuario asignado
                if (grdTareas.DataKeys[e.Row.RowIndex].Values["UsuarioAsignado"] == null)
                {

                    int id_tramite_tarea = Convert.ToInt32(grdTareas.DataKeys[e.Row.RowIndex].Values["id_tramitetarea"]);
                    DateTime? FechaFinalizacion = (DateTime?)grdTareas.DataKeys[e.Row.RowIndex].Values["FechaFinalizacion"];
                    int id_tarea = Convert.ToInt32(grdTareas.DataKeys[e.Row.RowIndex].Values["id_tarea"]);

                    LinkButton lnkTomarTarea = (LinkButton)e.Row.FindControl("lnkTomarTarea");

                    if (!FechaFinalizacion.HasValue)
                    {
                        Guid userid = Functions.GetUserId();
                        // Si tiene perfil para Editar la tarea
                        if (Engine.CheckRolTarea(id_tramite_tarea, userid))
                            lnkTomarTarea.Visible = true;
                    }
                }
            }
        }

        protected void lnkTomarTarea_Click(object sender, EventArgs e)
        {

            Guid userid = Functions.GetUserId();
            string urlTarea = "";

            if (userid != Guid.Empty)
            {
                try
                {

                    LinkButton lnkTomarTarea = (LinkButton)sender;
                    GridViewRow row = (GridViewRow)lnkTomarTarea.Parent.Parent;
                    Label lblFechaAsignada = (Label)lnkTomarTarea.Parent.Parent.FindControl("lblFechaAsignada");
                    Label lblUsuario = (Label)lnkTomarTarea.Parent.Parent.FindControl("lblUsuario");

                    int id_tramitetarea = int.Parse(lnkTomarTarea.CommandArgument);
                    string formulario_tarea = Convert.ToString(grdTareas.DataKeys[row.RowIndex].Values["form_aspx"]);
                    Engine.TomarTarea(id_tramitetarea, userid);
                    urlTarea = string.Format("~/GestionTramite/Tareas/{0}?id={1}", formulario_tarea, id_tramitetarea);
                    

                }
                catch (Exception ex)
                {
                    string msg = string.Format("alert('{0}')", ex.InnerException.Message);
                    ScriptManager.RegisterClientScriptBlock(updTareas, updTareas.GetType(), "", msg, true);
                    grdTareas.DataBind();
                }
                finally
                {
                    if (urlTarea.Length > 0)
                        Response.Redirect(urlTarea);
                }
            }
            else
                FormsAuthentication.RedirectToLoginPage();

        }

    }
}