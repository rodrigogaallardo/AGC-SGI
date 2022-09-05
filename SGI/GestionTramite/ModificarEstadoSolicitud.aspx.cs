using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public partial class ModificarEstadoSolicitud : System.Web.UI.Page
    {
        DGHP_Entities db = null;
        int id_solicitud;
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (!IsPostBack)
            {
                CargarDDL();
            }

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlEstados, updPnlEstados.GetType(),
                    "inicializar_controles", "inicializar_controles();", true);
            }
        }

        private void CargarDDL()
        {
            var lstValues = typeof(Constants.CategoriaModificarEstado).GetFields(BindingFlags.Static | BindingFlags.Public)
                                 .Where(x => x.IsLiteral && !x.IsInitOnly)
                                 .Select(x => new ListItem(x.GetValue(null).ToString(), x.Name));

            ddlCombo.DataSource = lstValues;
            ddlCombo.DataBind();
        }

        protected void btnReabrir_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            try
            {
                int.TryParse(txtSolicitud.Text, out id_solicitud);
                if (id_solicitud == 0)
                    throw new Exception("La solicitud no pertence a la categoria indicada.");

                var sol1 = db.SSIT_Solicitudes.Where(x => x.id_solicitud == id_solicitud && !string.IsNullOrEmpty(x.NroExpediente)).Any();
                var sol2 = db.Solicitud.Where(x => x.NroSolicitud == id_solicitud).Any();

                bool esJava = sol1 || sol2;

                int.TryParse(txtSolicitud.Text, out this.id_solicitud);

                int? id_estado = null;
                int? id_tipotramite = null;

                var sol = db.SSIT_Solicitudes.Where(x => x.id_solicitud == id_solicitud).FirstOrDefault();
                var transf = db.Transf_Solicitudes.Where(x => x.id_solicitud == id_solicitud).FirstOrDefault();


                switch (ddlCombo.SelectedValue)
                {
                    case Constants.CategoriaModificarEstado.tramite200mil:
                        if (esJava || sol == null)
                            throw new Exception("La solicitud no pertence a la categoria indicada.");

                        id_tipotramite = (int)Constants.TipoDeTramite.Habilitacion;

                        if (id_solicitud > Constants.SOLICITUDES_NUEVAS_MAYORES_A)
                            throw new Exception("La solicitud no pertence a la categoria indicada.");

                        id_estado = (int)Constants.Solicitud_Estados.Pendiente_de_pago;
                        break;

                    case Constants.CategoriaModificarEstado.tramiteTransferencias:
                        if (transf == null)
                            throw new Exception("La solicitud no pertence a la categoria indicada.");

                        id_tipotramite = (int)Constants.TipoDeTramite.Transferencia;

                        id_estado = (int)Constants.Solicitud_Estados.Pendiente_de_pago;

                        break;
                    case Constants.CategoriaModificarEstado.tramiteAnuladoSolicitud:
                        if (sol != null)
                        {
                            if (sol.id_estado != (int)Constants.Solicitud_Estados.Anulado)
                                throw new Exception("La solicitud no pertence a la categoria indicada.");

                            id_tipotramite = (int)Constants.TipoDeTramite.Habilitacion;

                            int estAnt = (from hist in db.SSIT_Solicitudes_HistorialEstados
                                          join est in db.TipoEstadoSolicitud on hist.cod_estado_ant equals est.Nombre
                                          where hist.id_solicitud == id_solicitud
                                          orderby hist.id_solhistest descending
                                          select est.Id).FirstOrDefault();
                            id_estado = estAnt;


                            var sgitt = (from tt in db.SGI_Tramites_Tareas
                                         join tth in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tth.id_tramitetarea
                                         where tth.id_solicitud == id_solicitud
                                         orderby tt.id_tramitetarea descending
                                         select tt).Take(2)
                                         .OrderBy(o => o.id_tramitetarea)
                                         .FirstOrDefault();

                            if (sgitt != null)
                                db.ENG_Crear_Tarea(id_solicitud, sgitt.id_tarea, sgitt.CreateUser, new ObjectParameter("id_tramitetarea", typeof(int)));

                        }
                        else
                            throw new Exception("La solicitud no pertence a la categoria indicada.");

                        break;

                    case Constants.CategoriaModificarEstado.tramiteAnuladoTransf:
                        if (transf != null)
                        {
                            if (transf.id_estado != (int)Constants.Solicitud_Estados.Anulado)
                                throw new Exception("La solicitud no pertence a la categoria indicada.");

                            id_tipotramite = (int)Constants.TipoDeTramite.Transferencia;

                            int estAnt = (from hist in db.Transf_Solicitudes_HistorialEstados
                                          join est in db.TipoEstadoSolicitud on hist.cod_estado_ant equals est.Nombre
                                          where hist.id_solicitud == id_solicitud
                                          orderby hist.id_solhistest descending
                                          select est.Id).FirstOrDefault();
                            id_estado = estAnt;


                            var sgitt = (from tt in db.SGI_Tramites_Tareas
                                         join tth in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tth.id_tramitetarea
                                         where tth.id_solicitud == id_solicitud
                                         orderby tt.id_tramitetarea descending
                                         select tt).Take(2)
                                         .OrderBy(o => o.id_tramitetarea)
                                         .FirstOrDefault();

                            if (sgitt != null)
                                db.ENG_Crear_Tarea(id_solicitud, sgitt.id_tarea, sgitt.CreateUser, new ObjectParameter("id_tramitetarea", typeof(int)));
                        }
                        else
                            throw new Exception("La solicitud no pertence a la categoria indicada.");

                        break;
                    case Constants.CategoriaModificarEstado.tramiteJava:
                        if (!esJava)
                            throw new Exception("La solicitud no pertence a la categoria indicada.");

                        id_tipotramite = (int)Constants.TipoDeTramite.Habilitacion;
                        id_estado = (int)Constants.Solicitud_Estados.Observado;
                        break;
                }


                if (id_estado.HasValue && id_tipotramite.HasValue)
                    db.ActualizarEstadoSolicitud(id_estado.Value, id_tipotramite.Value, id_solicitud);

                Enviar_Mensaje("Se realizo con exito el cambio de estado.", "Cambio de estado");
            }
            catch (Exception ex)
            {
                string mensaje = Functions.GetErrorMessage(ex);
                Enviar_Mensaje(mensaje, "");
            }
            finally
            {
                db.Dispose();
            }
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode("Actualizar estado solicitud");

            //updPnlGrillaProcesos
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostratMensaje('" + mensaje + "','" + titulo + "')", true);
        }
    }
}