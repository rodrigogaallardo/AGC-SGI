using SGI.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.WebControls;

namespace SGI.Operaciones
{
    public partial class AdministrarTareasDeUnaSolicitud : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarSolicitudConTareas();
            }
        }

        public void CargarSolicitudConTareas()
        {
            int idSolicitud;
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out idSolicitud);

            if (couldParse)
            {
                DGHP_Entities entities = new DGHP_Entities();
                
                IQueryable<SGI_Tramites_Tareas> tareasDeLaSolicitud = from tareas in entities.SGI_Tramites_Tareas
                                                                        join ttHab in entities.SGI_Tramites_Tareas_HAB on tareas.id_tramitetarea equals ttHab.id_tramitetarea
                                                                        where ttHab.id_solicitud == idSolicitud
                                                                        select tareas;

                gridViewTareas.DataSource = tareasDeLaSolicitud.ToList();
                gridViewTareas.DataBind();
            }      
        }

        public IEnumerable<aspnet_Users> CargarTodosLosUsuarios()
        {
            using (DGHP_Entities entities = new DGHP_Entities())
            {
                return entities.aspnet_Users.ToList();
            }
        }

        public IEnumerable<ENG_Tareas> CargarTodasLasTareas()
        {
            DGHP_Entities entities = new DGHP_Entities();
            return entities.ENG_Tareas.OrderBy(tarea => tarea.ENG_Circuitos.nombre_circuito).ToList();
        }

        protected void gridViewTareas_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridViewTareas.EditIndex = e.NewEditIndex;
            this.CargarSolicitudConTareas();
        }

        protected void gridViewTareas_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridViewTareas.EditIndex = -1;
            this.CargarSolicitudConTareas();
        }

        protected void gridViewTareas_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gridViewTareas.Rows[e.RowIndex];
            Label labelIdTramiteTarea = (Label)row.FindControl("labelIdTramiteTarea");
            DropDownList dropDownListTarea = (DropDownList) row.FindControl("dropDownListEditTarea");
            DropDownList dropDownListUsuario = (DropDownList)row.FindControl("dropDownListEditUsuario");
            Calendar calendarFechaCierre = (Calendar)row.FindControl("calendarEditFechaCierre");

            int idTramiteTarea = int.Parse(labelIdTramiteTarea.Text);
            int idTarea = int.Parse(dropDownListTarea.SelectedValue);
            Guid idUsuario = Guid.Parse(dropDownListUsuario.SelectedValue);
            DateTime fechaCierre = calendarFechaCierre.SelectedDate;

            using (DGHP_Entities entities = new DGHP_Entities())
            {
                SGI_Tramites_Tareas tramiteTarea = entities.SGI_Tramites_Tareas.Where(tarea => tarea.id_tramitetarea == idTramiteTarea).FirstOrDefault();

                if (tramiteTarea != null) 
                {
                    tramiteTarea.id_tarea = idTarea;
                    tramiteTarea.FechaCierre_tramitetarea = fechaCierre;

                    if (tramiteTarea.UsuarioAsignado_tramitetarea != idUsuario)
                    {
                        tramiteTarea.FechaAsignacion_tramtietarea = DateTime.Now;
                        tramiteTarea.UsuarioAsignado_tramitetarea = idUsuario;
                    }

                    entities.SaveChanges();
                }
            }

            gridViewTareas.EditIndex = -1;
            this.CargarSolicitudConTareas();
        }

        protected void btnBuscarSolicitud_Click(object sender, EventArgs e)
        {
            this.CargarSolicitudConTareas();
        }
    }
}