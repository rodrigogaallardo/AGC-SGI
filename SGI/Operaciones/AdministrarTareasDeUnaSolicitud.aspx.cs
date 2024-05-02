using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelLibrary.BinaryFileFormat;
using Newtonsoft.Json;
using SGI.Model;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Operaciones
{
    public partial class AdministrarTareasDeUnaSolicitud : System.Web.UI.Page
    {

        private string id_object
        {
            get { return ViewState["_id_object"] != null ? ViewState["_id_object"].ToString() : string.Empty; }
            set { ViewState["_id_object"] = value; }
        }

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
                CargarSolicitudConTareas();
            }
        }
        public static List<SGI_Tramites_Tareas> tareasDeLaSolicitud;
        public void CargarSolicitudConTareas()
        {
            int idSolicitud;
            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out idSolicitud);

            if (couldParse)
            {
                DGHP_Entities entities = new DGHP_Entities();
                var solic = 0;
                solic = (from sol in entities.Transf_Solicitudes where sol.id_solicitud == idSolicitud select sol.id_solicitud).FirstOrDefault();
                if (solic == 0)
                {
                    solic = (from sol in entities.SSIT_Solicitudes where sol.id_solicitud == idSolicitud select sol.id_solicitud).FirstOrDefault();
                    tareasDeLaSolicitud = (from tareas in entities.SGI_Tramites_Tareas
                                           join ttHab in entities.SGI_Tramites_Tareas_HAB
                                           on tareas.id_tramitetarea equals ttHab.id_tramitetarea
                                           where ttHab.id_solicitud == idSolicitud
                                           select tareas).ToList();
                    hdHAB_TRANSF.Value = "H";
                }
                else
                {
                    tareasDeLaSolicitud = (from tareas in entities.SGI_Tramites_Tareas
                                           join tTransf in entities.SGI_Tramites_Tareas_TRANSF
                                           on tareas.id_tramitetarea equals tTransf.id_tramitetarea
                                           where tTransf.id_solicitud == idSolicitud
                                           select tareas).ToList();
                    hdHAB_TRANSF.Value = "T";
                }

                if (tareasDeLaSolicitud.Count < 1)
                {
                    if (solic == 0)
                    {
                        hdHAB_TRANSF.Value = "";
                        btnNuevo.Enabled = false;//SI NO HAY REG TRANSF NI HAB ESCONTO BOTON NUEVO
                    }
                    else
                    {
                        btnNuevo.Enabled = true;
                    }
                }
                else
                {
                    btnNuevo.Enabled = true;
                }

                hdidSolicitud.Value = idSolicitud.ToString();
                //No permite editar/eliminar tarea si tiene procesos de sade exitosos.  Edicion parcial borrar on permite
                //La tabla que tiene los procesos de sade es SGI_SADE_Procesos

                //cuando sade = true +> edicion parcial
                //y el campo realizado_en_SADE es el que determina si un proceso fue ejecutado con exito
                //siendo 0 para no generado y 1 para generado.con exito
                //Los campos que permitie editar para las tareas con procesos existosos son

                gridView.DataSource = tareasDeLaSolicitud;
                gridView.DataBind();
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
            this.CargarSolicitudConTareas();
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            id_object = ((Button)sender).ToolTip;
            string script = "$('#frmEliminarLog').modal('show');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int idTramiteTarea = int.Parse(((Button)sender).ToolTip);
            Response.Redirect("~/Operaciones/TareasForm.aspx?idTramiteTarea=" + idTramiteTarea + "&idSolicitud=" + hdidSolicitud.Value);

        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridView grid = (GridView)gridView;




                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int id_tramitetarea = -1;
                    if (int.TryParse(DataBinder.Eval(e.Row.DataItem, "id_tramitetarea").ToString(), out id_tramitetarea))
                    {
                        DGHP_Entities entities = new DGHP_Entities();
                        //List<SGI_SADE_Procesos> SGI_SADE_ProcesosList = (from SADE_Procesos in entities.SGI_SADE_Procesos
                        //                                                 where SADE_Procesos.id_tramitetarea == id_tramitetarea

                        //                                                 select SADE_Procesos).ToList();
                        SGI_SADE_Procesos sGI_SADE_Procesos = (from SADE_Procesos in entities.SGI_SADE_Procesos
                                                               where SADE_Procesos.id_tramitetarea == id_tramitetarea
                                                               && SADE_Procesos.realizado_en_SADE == true
                                                               select SADE_Procesos).FirstOrDefault();
                        if (sGI_SADE_Procesos != null)
                        {
                            Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                            Button btnRemove = (Button)e.Row.FindControl("btnRemove");
                            btnRemove.Enabled = false;
                        }


                    }



                   ;





                }

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            int idSolicitud = int.Parse(hdidSolicitud.Value);
            string hAB_tRANSF = hdHAB_TRANSF.Value;
            var id_circuito = 0;
            if (tareasDeLaSolicitud.Count() >= 1)
            {
                id_circuito = tareasDeLaSolicitud.LastOrDefault().ENG_Tareas.ENG_Circuitos.id_circuito;
            }
            Response.Redirect("~/Operaciones/TareasForm.aspx?idTramiteTarea=0" + "&idSolicitud=" + idSolicitud + "&hAB_tRANSF=" + hAB_tRANSF + "&id_circuito=" + id_circuito);
        }

        private void Eliminar()
        {
            int idTramiteTarea = int.Parse(id_object);
            try
            {
                using (var ctx = new DGHP_Entities())
                {
                    using (var tran = ctx.Database.BeginTransaction())
                    {
                        try
                        {
                            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
                            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
                            SGI_Tramites_Tareas obj = ctx.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == idTramiteTarea); 
                            if (obj != null)
                            {
                                List<SGI_Tramites_Tareas_HAB> SGI_Tramites_Tareas_HABList = ctx.SGI_Tramites_Tareas_HAB.Where(tth => tth.id_tramitetarea == idTramiteTarea).ToList();
                                ctx.SGI_Tramites_Tareas_HAB.RemoveRange(SGI_Tramites_Tareas_HABList);
                                List<SGI_Tramites_Tareas_TRANSF> SGI_Tramites_Tareas_TRANSFList = ctx.SGI_Tramites_Tareas_TRANSF.Where(tet => tet.id_tramitetarea == idTramiteTarea).ToList();
                                ctx.SGI_Tramites_Tareas_TRANSF.RemoveRange(SGI_Tramites_Tareas_TRANSFList);
                                ctx.SGI_Tramites_Tareas.Remove(obj);
                            }
                            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "D", 4011);
                            ctx.SaveChanges();
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Dispose();
                            LogError.Write(ex, "Error en transaccion.");
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager sm = ScriptManager.GetCurrent(this);
                string cadena = "No pudo borrarse el Tramite Tarea por restricciones con otras tablas";
                ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", string.Format("alert('{0}');", cadena), true);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
            gridView.EditIndex = -1;
            this.CargarSolicitudConTareas();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Eliminar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.txtObservacionesSolicitante.Text = string.Empty;
            this.Eliminar();
        }
    }
}