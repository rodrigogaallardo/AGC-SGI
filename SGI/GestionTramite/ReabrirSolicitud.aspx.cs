using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public partial class ReabrirSolicitud : BasePage
    {
        DGHP_Entities db = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (!IsPostBack)
            {
            }

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlReabrir, updPnlReabrir.GetType(),
                    "inicializar_controles", "inicializar_controles();", true);
            }

        }
        protected void btnReabrir_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    int id_solicitud = int.Parse(txtSolicitud.Text);
                    int estado;
                    int tt = 0;
                    int idtarnew = 0;
                    var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    var trans_sol = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    bool fuePresentada;

                    if (sol == null && trans_sol == null)
                        throw new Exception("No se encontro la solicitud.");
                    else if (sol == null)
                    {
                        estado = trans_sol.id_estado;
                        tt = (int)Constants.TipoDeTramite.Transferencia;
                        fuePresentada = db.Transf_Solicitudes_HistorialEstados.Any(x => x.cod_estado_nuevo == "ETRA");
                    }
                    else
                    {
                        estado = sol.id_estado;
                        tt = (int)Constants.TipoDeTramite.Habilitacion;
                        fuePresentada = db.SSIT_Solicitudes_HistorialEstados.Any(x => x.cod_estado_nuevo == "ETRA");
                    }

                    if (estado != (int)Constants.Solicitud_Estados.Vencida)
                        throw new Exception("La solicitud no esta vencida.");

                    List<SGI_Tramites_Tareas> tareas = ValidarTareas(tt, id_solicitud);

                    if (tareas.Count < 2)
                        throw new Exception("No se puede reabrir el circuito de la solicitud.");

                    var ultima = tareas.First();
                    var antUltima = tareas.GetRange(0, 2).Last();

                    var parameter = new ObjectParameter("circuito", typeof(int));
                    db.SPGetIdCircuitoHAB(id_solicitud, parameter);
                    int idCircuito = (int)parameter.Value;

                    var codtar = (from tar in db.ENG_Tareas
                                  where tar.id_tarea == ultima.id_tarea
                                  select tar.cod_tarea).FirstOrDefault();

                    var codtarant = (from tar in db.ENG_Tareas
                                     where tar.id_tarea == antUltima.id_tarea
                                     select tar.cod_tarea).FirstOrDefault();

                    if (codtar.ToString().Substring(codtar.ToString().Length - 2, 2) != Constants.ENG_Tipos_Tareas.Fin_Tramite)
                        throw new Exception("No se puede reabrir el circuito de la solicitud.");

                    idtarnew = antUltima.id_tarea;

                    Guid userid = Functions.GetUserId();

                    //--crear tarea "Solicitud de habilitacion", estado datos confirmados si la sol nunca fue presentada
                    //--si la sol fue presentada se crear la tarea correccion con estado observado
                    if (!fuePresentada)
                    {
                        idtarnew = (from tar in db.ENG_Tareas
                                    where tar.id_circuito == idCircuito &&
                                          tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Solicitud_Habilitacion
                                    orderby tar.id_tarea ascending
                                    select tar.id_tarea).FirstOrDefault();
                        if (tt != (int)Constants.TipoDeTramite.Transferencia)
                        {
                            db.SSIT_Solicitudes_ActualizarEstado(id_solicitud, (int)Constants.Solicitud_Estados.Datos_Confirmados, userid, null, null);
                        }
                        else
                        {
                            db.Transfe_Solicitudes_ActualizarEstado(id_solicitud, (int)Constants.Solicitud_Estados.Datos_Confirmados, userid, null, null);
                        }
                    }
                    else
                    {
                        idtarnew = (from tar in db.ENG_Tareas
                                    where tar.id_circuito == idCircuito &&
                                          tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Correccion_Solicitud
                                    orderby tar.id_tarea ascending
                                    select tar.id_tarea).FirstOrDefault();
                        if (tt != (int)Constants.TipoDeTramite.Transferencia)
                        {
                            db.SSIT_Solicitudes_ActualizarEstado(id_solicitud, (int)Constants.Solicitud_Estados.Observado, userid, null, null);
                        }
                        else
                        {
                            db.Transfe_Solicitudes_ActualizarEstado(id_solicitud, (int)Constants.Solicitud_Estados.Observado, userid, null, null);
                        }
                    }

                    //Creo la tarea igual a la anterir a Fin de Tramite
                    ObjectParameter param_id_tramitetarea = new ObjectParameter("id_tramitetarea", typeof(int));
                    db.ENG_Crear_Tarea(id_solicitud, idtarnew, userid, param_id_tramitetarea);
                    //Si tiene le asigno el usuario que tenia
                    if (antUltima.UsuarioAsignado_tramitetarea != null)
                    {
                        var id_tramitetarea_nuevo = Convert.ToInt32(param_id_tramitetarea.Value);
                        db.ENG_Asignar_Tarea(id_tramitetarea_nuevo, antUltima.UsuarioAsignado_tramitetarea);
                    }

                    Enviar_Mensaje("El circuito de la solicitud se reabrio correctamente.", "");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    string mensaje = Functions.GetErrorMessage(ex);
                    Enviar_Mensaje(mensaje, "");
                }
                finally
                {
                    txtSolicitud.Text = "";
                    db.Dispose();
                }
            }
        }

        private List<SGI_Tramites_Tareas> ValidarTareas(int tipo, int id_solicitud)
        {
            List<SGI_Tramites_Tareas> tareas;
            if (tipo == (int)Constants.TipoDeTramite.Transferencia)
            {
                tareas = (from tt in db.SGI_Tramites_Tareas
                          join th in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals th.id_tramitetarea
                          where th.id_solicitud == id_solicitud
                          orderby tt.id_tramitetarea descending
                          select tt).ToList();
            }
            else
            {
                tareas = (from tt in db.SGI_Tramites_Tareas
                          join th in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals th.id_tramitetarea
                          where th.id_solicitud == id_solicitud
                          orderby tt.id_tramitetarea descending
                          select tt).ToList();
            }
            return tareas;
        }
        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode("Reabrir Circuito");

            //updPnlGrillaProcesos
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostratMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        private int getIdCircuito(int id_solicitud)
        {


            var lstEnc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo)
                                    .OrderByDescending(z => z.id_encomienda)
                                    .ToList();


            var encDTO = lstEnc.FirstOrDefault();

            int id_circuito = GetIdCircuitoByIdEncomienda(encDTO.id_encomienda);
            if (id_circuito == 0)
                throw new Exception("No se ha encontrado ningún rubro con circuito configurado en la solicitud.");

            //Si es automatica hay que hacer la validacion. Si no valida es Simple sin planos (SSP2)
            if (id_circuito == (int)Constants.ENG_Circuitos.SSP3)
                id_circuito = (int)Constants.ENG_Circuitos.SSP2;

            // Si es una ampliación y el resultado era una Simple sin planos automáticas se envía a Simples sin planos 2 Mantis 115695
            if (encDTO.id_tipotramite == (int)Constants.TipoDeTramite.Ampliacion_Unificacion && id_circuito == (int)Constants.ENG_Circuitos.AMP_SSP3)
                id_circuito = (int)Constants.ENG_Circuitos.AMP_SSP2;
            //Si es simple sin planos y superficie mayor a 500 pasa a simple con planos
            var datosLocal = encDTO.Encomienda_DatosLocal.FirstOrDefault();

            decimal SuperficieTotal = 0;
            bool esAmpliacionSuperficie = (datosLocal.ampliacion_superficie.HasValue ? datosLocal.ampliacion_superficie.Value : false);

            if (esAmpliacionSuperficie)
                SuperficieTotal = datosLocal.superficie_cubierta_amp.Value + datosLocal.superficie_descubierta_amp.Value;
            else
                SuperficieTotal = datosLocal.superficie_cubierta_dl.Value + datosLocal.superficie_descubierta_dl.Value;

            if (id_circuito == (int)Constants.ENG_Circuitos.SSP2 && SuperficieTotal > 500)
                id_circuito = (int)Constants.ENG_Circuitos.SCP2;
            if (id_circuito == (int)Constants.ENG_Circuitos.AMP_SSP2 && SuperficieTotal > 500)
                id_circuito = (int)Constants.ENG_Circuitos.AMP_SSP2;
            return id_circuito;

        }

        public int GetIdCircuitoByIdEncomienda(int id_encomienda)
        {
            // Toma el primer registro segun la prioridad del circuito.
            // ---------------------------------------------------------
            var list = (from er in db.Encomienda_Rubros
                        join enc in db.Encomienda on er.id_encomienda equals enc.id_encomienda
                        join encsol in db.Encomienda_SSIT_Solicitudes on enc.id_encomienda equals encsol.id_encomienda
                        join sol in db.SSIT_Solicitudes on encsol.id_solicitud equals sol.id_solicitud
                        join r in db.Rubros on er.cod_rubro equals r.cod_rubro
                        join gc in db.ENG_Grupos_Circuitos on r.id_grupo_circuito equals gc.id_grupo_circuito
                        join rel in db.ENG_Rel_Circuitos_TiposDeTramite on
                                     new { sol.id_tipotramite, sol.id_tipoexpediente, sol.id_subtipoexpediente } equals
                                     new { rel.id_tipotramite, rel.id_tipoexpediente, rel.id_subtipoexpediente }
                        join cir in db.ENG_Circuitos on rel.id_circuito equals cir.id_circuito
                        where er.id_encomienda == id_encomienda
                             && (!rel.id_grupo_circuito.HasValue || rel.id_grupo_circuito == r.id_grupo_circuito)
                        orderby cir.prioridad descending
                        select cir.id_circuito);
            var id_circuito = list.FirstOrDefault();

            return id_circuito;
        }

    }
}