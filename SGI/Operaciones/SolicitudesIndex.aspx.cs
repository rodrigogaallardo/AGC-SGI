using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using SGI.Model;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Operaciones
{
    public partial class SolicitudesIndex : System.Web.UI.Page
    {
        List<TipoEstadoSolicitud> TipoEstadoSolicitudList;
        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion
            if (!IsPostBack)
            {
                string idSolicitudStr = (Request.QueryString["idSolicitud"] == null) ? "" : Request.QueryString["idSolicitud"].ToString();
                txtBuscarSolicitud.Text = idSolicitudStr;

                DGHP_Entities entities = new DGHP_Entities();
                TipoEstadoSolicitudList = (from t in entities.TipoEstadoSolicitud
                                           orderby (t.Descripcion)
                                           select t).ToList();

                CargarSolicitud();
            }
        }

        public void CargarSolicitud()
        {
            gridViewTransf_Solicitudes.DataSource = null;
            gridViewTransf_Solicitudes.DataBind();
            lblMsj.Text = "";
            int idSolicitud;
            hdSSIT_TRANSF.Value = "";
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out idSolicitud);

            if (couldParse)
            {
                DGHP_Entities entities = new DGHP_Entities();



                List<SSIT_Solicitudes> SSIT_SolicitudesList = (from s in entities.SSIT_Solicitudes
                                                               where s.id_solicitud == idSolicitud
                                                               select s).ToList();
                if (SSIT_SolicitudesList.Count > 0)
                {

                    gridViewSSIT_Solicitudes.Visible = true;
                    gridViewTransf_Solicitudes.Visible = false;
                    gridViewSSIT_Solicitudes.DataSource = SSIT_SolicitudesList;
                    gridViewSSIT_Solicitudes.DataBind();
                    hdidSolicitud.Value = idSolicitud.ToString();
                    hdSSIT_TRANSF.Value = "S";
                    return;

                }

                List<Transf_Solicitudes> Transf_SolicitudesList = (from s in entities.Transf_Solicitudes
                                                                   where s.id_solicitud == idSolicitud
                                                                   select s).ToList();

                if (Transf_SolicitudesList.Count > 0)
                {

                    gridViewSSIT_Solicitudes.Visible = false;
                    gridViewTransf_Solicitudes.Visible = true;
                    gridViewTransf_Solicitudes.DataSource = Transf_SolicitudesList;
                    gridViewTransf_Solicitudes.DataBind();
                    hdidSolicitud.Value = idSolicitud.ToString();
                    hdSSIT_TRANSF.Value = "T";
                    return;

                }

                lblMsj.Text = "No hay datos pra esta Solicitud";

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





        protected void btnBuscarSolicitud_Click(object sender, EventArgs e)
        {
            this.CargarSolicitud();
        }

        //protected void btnRemove_Click(object sender, EventArgs e)
        //{
        //    int idTramiteTarea = int.Parse(((Button)sender).ToolTip);
        //    using (DGHP_Entities entities = new DGHP_Entities())
        //    {
        //        SGI_Tramites_Tareas tramiteTarea = entities.SGI_Tramites_Tareas.Where(tarea => tarea.id_tramitetarea == idTramiteTarea).FirstOrDefault();

        //        if (tramiteTarea != null)
        //        {

        //            #region SGI_Tramites_Tareas_HAB
        //            List<SGI_Tramites_Tareas_HAB> SGI_Tramites_Tareas_HABList =
        //              entities.SGI_Tramites_Tareas_HAB.Where(tth => tth.id_tramitetarea == idTramiteTarea).ToList();

        //            entities.SGI_Tramites_Tareas_HAB.RemoveRange(SGI_Tramites_Tareas_HABList);
        //            #endregion

        //            #region SGI_Tramites_Tareas_TRANSF
        //            List<SGI_Tramites_Tareas_TRANSF> SGI_Tramites_Tareas_TRANSFList =
        //           entities.SGI_Tramites_Tareas_TRANSF.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();

        //            entities.SGI_Tramites_Tareas_TRANSF.RemoveRange(SGI_Tramites_Tareas_TRANSFList);
        //            #endregion


        //            try
        //            {
        //                entities.SGI_Tramites_Tareas.Remove(tramiteTarea);
        //                entities.SaveChanges();
        //            }
        //            catch (Exception ex)
        //            {
        //                //ASOSA MENSAJE DE ERROR
        //                ScriptManager sm = ScriptManager.GetCurrent(this);
        //                string cadena = "No pudo borrarse el Tramite Tarea por restricciones con otras tablas";
        //                string script = string.Format("alert('{0}');", cadena);
        //                ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);


        //            }
        //        }


        //        this.CargarSolicitudConTareas();
        //    }
        //}
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int idTramiteTarea = int.Parse(((Button)sender).ToolTip);
            Response.Redirect("~/Operaciones/SolicitudesForm.aspx?idSolicitud=" + hdidSolicitud.Value + "&SSIT_TRANSF=" + hdSSIT_TRANSF.Value);
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;




                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DGHP_Entities entities = new DGHP_Entities();
                    Label lblTipoEstado = (Label)e.Row.FindControl("lblTipoEstado");
                    Label labelidSolicitud = (Label)e.Row.FindControl("labelidSolicitud");
                    int id = int.Parse(lblTipoEstado.Text);
                    TipoEstadoSolicitud TipoEstadoSolicitud = (from t in entities.TipoEstadoSolicitud
                                                               where t.Id == id
                                                               select t).FirstOrDefault();



                    lblTipoEstado.Text = TipoEstadoSolicitud.Descripcion;






                }

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }
        }


    }
}