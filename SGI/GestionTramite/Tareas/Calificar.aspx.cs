using RestSharp.Extensions;
using SGI.GestionTramite.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using CheckBox = System.Web.UI.WebControls.CheckBox;

namespace SGI.GestionTramite.Tareas
{
    public partial class Calificar : BasePage
    {

        #region cargar inicial

        //private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Calificar;

        protected void Page_Load(object sender, EventArgs e)
        {
            UcObservacionesLibrarUso.Enabled = true;
            if (!IsPostBack)
            {
                int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                if (id_tramitetarea > 0)
                    CargarDatosTramite(id_tramitetarea);
                chbLibrarUso.CheckedChanged += ChbLibrarUso_CheckedChanged;
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
        }

        private void CargarDatosTramite(int id_tramitetarea)
        {

            Guid userid = Functions.GetUserId();

            IniciarEntity();

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            this.Title = "Tarea: " + tramite_tarea.ENG_Tareas.nombre_tarea;

            if (tramite_tarea == null)
            {
                FinalizarEntity();
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }


            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;
            ucObservacionContribuyente.Enabled = IsEditable;
            ucObservacionesInternas.Enabled = IsEditable;
            ucSGI_DocumentoAdjunto.Enabled = IsEditable;
            ucObservaciones.Enabled = IsEditable;
            chbLibrarUso.Enabled = IsEditable;
            ucSGI_ListaPlanoVisado.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;
            int id_circuito = ttHAB.SGI_Tramites_Tareas.ENG_Tareas.id_circuito;

            SGI_Tarea_Calificar calificar = Buscar_Tarea(id_tramitetarea);

            ucListaObservacionesAnteriores.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucListaObservacionesAnterioresv1.LoadData(id_grupotramite, this.id_solicitud, tramite_tarea.id_tramitetarea, tramite_tarea.id_tarea);
            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucSGI_ListaDocumentoAdjuntoAnteriores.LoadData(id_grupotramite, this.id_solicitud, this.TramiteTarea);
            ucSGI_DocumentoAdjunto.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea);
            ucSGI_ListaPlanoVisado.LoadData(this.id_solicitud, this.TramiteTarea);
            bool mostarRespaldo = id_circuito == (int)Constants.ENG_Circuitos.AMP_ESCU_HP ||
                id_circuito == (int)Constants.ENG_Circuitos.RU_ESCU_HP ||
                id_circuito == (int)Constants.ENG_Circuitos.ESCU_HP ||
                id_circuito == (int)Constants.ENG_Circuitos.AMP_SCP5 ||
                id_circuito == (int)Constants.ENG_Circuitos.RU_SCP5 ||
                id_circuito == (int)Constants.ENG_Circuitos.SCP5
                ? false : true;
            ucObservaciones.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea, mostarRespaldo);

            ucPreviewDocumentos.Visible = true;
            ucPreviewDocumentos.LoadData(this.id_solicitud);
            this.id_tarea = ttHAB.SGI_Tramites_Tareas.id_tarea;

            ucSGI_ListaPlanoVisado.Visible = ((tramite_tarea.ENG_Tareas.cod_tarea == Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Calificar) ||
                                              tramite_tarea.ENG_Tareas.cod_tarea == Convert.ToInt32(id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Calificar2)) &&
                                              tramite_tarea.ENG_Tareas.ENG_Circuitos.nombre_grupo != Constants.grupoCircuito.SSP &&
                                              tramite_tarea.ENG_Tareas.ENG_Circuitos.nombre_grupo != Constants.grupoCircuito.SSPA);

            string tipo_tarea = ttHAB.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea.ToString();
            tipo_tarea = tipo_tarea.Substring(tipo_tarea.Length - 2);

            //mantis 0118156: JADHE 42929 - SGI - Observaciones anteriores
            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                // Oculta las Observaciones Viejas  y deja las Nuevas
                ucListaObservacionesAnteriores.Visible = false;
                ucObservacionContribuyente.Visible = false;
                ucListaRubros.Visible = true;
                ucTramitesRelacionados.Visible = true;
            }
            else
            {
                // Oculta las Observaciones Nuevas y deja las Viejas
                ucObservaciones.Visible = false;
                ucListaObservacionesAnterioresv1.Visible = false;
                ucListaRubros.Visible = false;
                ucTramitesRelacionados.Visible = false;
            }

            if (calificar != null)
            {
                ucObservacionesTarea.Text = calificar.Observaciones.Trim();
                ucObservacionContribuyente.Text = calificar.Observaciones_contribuyente.Trim();
                ucObservacionesInternas.Text = calificar.Observaciones_Internas != null ? calificar.Observaciones_Internas.Trim() : "";
                UcObservacionesLibrarUso.Text = calificar.Observaciones_LibradoUso != null ? calificar.Observaciones_LibradoUso.Trim() : "";
                chbLibrarUso.Checked = calificar.Librar_Uso;
            }
            else
            {
                ucObservacionesTarea.Text = ObservacionAnteriores.Buscar_ObservacionPlancheta((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea);
                ucObservacionContribuyente.Text = "";
                ucObservacionesInternas.Text = "";
                UcObservacionesLibrarUso.Text = ObservacionAnteriores.Buscar_ObservacionLibradoUso((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea);
                chbLibrarUso.Checked = false;
            }
            if (!string.IsNullOrEmpty(UcObservacionesLibrarUso.Text))
            {
                UcObservacionesLibrarUso.Enabled = false;
            }
            pnl_Librar_Uso.Visible = false;

            // Si tiene Normativa y el calificador aprobo el trámite y es cualquier tareas de circuito 2 se debe generar el Qr
            if (tipo_tarea == Constants.ENG_Tipos_Tareas.Calificar || tipo_tarea == Constants.ENG_Tipos_Tareas.Calificar2)
            {
                SSIT_Solicitudes sol = new SSIT_Solicitudes();
                sol = db.SSIT_Solicitudes.Where(x => x.id_solicitud == this.id_solicitud).FirstOrDefault();

                var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

                bool LiberadoAlUsoRubro = isLiberadoAlUsoRubro(enc.id_encomienda);
                bool ubicacionEspecial = isUbicacionEspecial(enc.id_encomienda, "U");
                bool tieneNormativas = TieneNormativas(enc.id_encomienda);
                bool condicionIncendioOk = TienePlanoDeIncendio(this.id_solicitud, enc.id_encomienda);
                bool esZonaAHP = isUbicacionEspecial(enc.id_encomienda, "APH");
                bool acogeBeneficios = EncomiendaAcogeBeneficiosUERESGP(enc.id_encomienda); 
                bool esHabilitacionPrevia = tramite_tarea.ENG_Tareas.ENG_Circuitos.id_grupocircuito == (int)Constants.ENG_Grupos_Circuitos.HP || 
                                            tramite_tarea.ENG_Tareas.ENG_Circuitos.id_grupocircuito == (int)Constants.ENG_Grupos_Circuitos.HPESCU;

                var datosLocal = enc.Encomienda_DatosLocal.FirstOrDefault();
                var esInmuebleCatalogo = EsInmuebleCatalogado(enc.id_encomienda);

                bool librado = false;
                
                if (condicionIncendioOk || tieneNormativas || ubicacionEspecial || esInmuebleCatalogo || esZonaAHP || acogeBeneficios || esHabilitacionPrevia)
                {
                    pnl_Librar_Uso.Visible = true;
                }
                var fechalibrado = sol.FechaLibrado;
                var estaLibrado = false;
                if (calificar != null)
                    estaLibrado = calificar.Librar_Uso;
                if (fechalibrado != null || estaLibrado)
                {
                    librado = true;
                }
                if (librado || LiberadoAlUsoRubro)
                    chbLibrarUso.Checked = true;
                else
                    chbLibrarUso.Checked = false;
                if (chbLibrarUso.Visible && !chbLibrarUso.Enabled)
                {
                    chbLibrarUso.Checked = librado;
                }
            }

            if (tramite_tarea.id_tarea == (int)Constants.ENG_Tareas.ESCU_HP_Calificar_1 ||
                tramite_tarea.id_tarea == (int)Constants.ENG_Tareas.ESCU_HP_Calificar_2 ||
                tramite_tarea.id_tarea == (int)Constants.ENG_Tareas.ESCU_IP_Calificar_1 ||
                tramite_tarea.id_tarea == (int)Constants.ENG_Tareas.ESCU_IP_Calificar_2)
            {
                ucProcesosSADE.id_grupo_tramite = (int)Constants.GruposDeTramite.HAB;
                ucProcesosSADE.cargarDatosProcesos(id_tramitetarea, IsEditable);
                ucResultadoTarea.btnFinalizar_Enabled = IsEditable;
                if (IsEditable)
                    ucResultadoTarea.btnFinalizar_Enabled = Functions.EsForzarTarasSade() || !ucProcesosSADE.hayProcesosPendientesSADE(id_tramitetarea);
            }
            if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Aprobado)
            {
                ucObservacionesTarea.Text = Parametros.GetParam_ValorChar("SGI.Notas.Adicionales.Dispo") + "\n\r" + ucObservacionesTarea.Text;
                ucObservacionesTarea.Update();
            }
            FinalizarEntity();
        }

        private bool TienePlanoDeIncendio(int id_solicitud, int id_encomienda)
        {
            int tipoPlanoIncendio = 2;
            int tipoDocReqSol = 66;
            bool planoIncEnc = (from ep in db.Encomienda_Planos
                                where ep.id_encomienda == id_encomienda
                                && ep.id_tipo_plano == tipoPlanoIncendio
                                select ep).Any();

            bool planoIncSol = (from sd in db.SSIT_DocumentosAdjuntos
                                where sd.id_solicitud == id_solicitud
                                && sd.id_tdocreq == tipoDocReqSol
                                select sd).Any();

            return (planoIncEnc || planoIncSol);
        }

        private bool TieneNormativas(int id_encomienda)
        {
            return (from encoNorm in db.Encomienda_Normativas
                    where encoNorm.id_encomienda == id_encomienda
                    select encoNorm.id_tiponormativa).Count() > 0;
        }

        private bool isUbicacionEspecial(int id_encomienda, string codigo)
        {
            return (from encubic in db.Encomienda_Ubicaciones
                    join encubicDist in db.Encomienda_Ubicaciones_Distritos on encubic.id_encomiendaubicacion equals encubicDist.id_encomiendaubicacion
                    join cat in db.Ubicaciones_CatalogoDistritos on encubicDist.IdDistrito equals cat.IdDistrito
                    join gd in db.Ubicaciones_GruposDistritos on cat.IdGrupoDistrito equals gd.IdGrupoDistrito
                    where encubic.id_encomienda == id_encomienda && gd.Codigo == codigo
                    select gd.Codigo).Count() > 0;
        }
        private bool isLiberadoAlUsoRubro(int id_encomienda)
        {
            int cant_rubros_librar;
            cant_rubros_librar = (
                from encrub in db.Encomienda_Rubros
                join rub in db.Rubros on encrub.cod_rubro equals rub.cod_rubro
                where encrub.id_encomienda == id_encomienda && rub.Librar_Uso
                select encrub.cod_rubro
                ).Union(
                from encrub in db.Encomienda_RubrosCN
                join rub in db.RubrosCN on encrub.IdRubro equals rub.IdRubro
                where encrub.id_encomienda == id_encomienda && rub.LibrarUso
                select encrub.CodigoRubro).Count();
            int cant_rubros;
            cant_rubros = (
                from encrub in db.Encomienda_Rubros
                join rub in db.Rubros on encrub.cod_rubro equals rub.cod_rubro
                where encrub.id_encomienda == id_encomienda
                select encrub.cod_rubro
                ).Union(
                from encrub in db.Encomienda_RubrosCN
                join rub in db.RubrosCN on encrub.IdRubro equals rub.IdRubro
                where encrub.id_encomienda == id_encomienda
                select encrub.CodigoRubro).Count();
            return cant_rubros_librar == cant_rubros;
        }

        private bool EncomiendaAcogeBeneficiosUERESGP(int id_encomienda)
        {
            return (bool)(from enc in db.Encomienda
                          where enc.id_encomienda == id_encomienda
                          select enc.AcogeBeneficios).FirstOrDefault();
        }

        protected void ChbLibrarUso_CheckedChanged(object sender, EventArgs e)
        {
            UcObservacionesLibrarUso.Enabled = chbLibrarUso.Checked;
            if (!chbLibrarUso.Checked)
            {
                UcObservacionesLibrarUso.Text = "";
            }
        }

        private int _tramiteTarea = 0;
        public int TramiteTarea
        {
            get
            {
                if (_tramiteTarea == 0)
                {
                    int.TryParse(hid_id_tramitetarea.Value, out _tramiteTarea);
                }
                return _tramiteTarea;
            }
            set
            {
                hid_id_tramitetarea.Value = value.ToString();
                _tramiteTarea = value;
            }
        }

        public int id_tarea
        {
            get
            {
                return (ViewState["_id_tarea"] != null ? Convert.ToInt32(ViewState["_id_tarea"]) : 0);
            }
            set
            {
                ViewState["_id_tarea"] = value.ToString();
            }
        }

        private int _id_solicitud = 0;
        public int id_solicitud
        {
            get
            {
                if (_id_solicitud == 0)
                {
                    int.TryParse(hid_id_solicitud.Value, out _id_solicitud);
                }
                return _id_solicitud;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
                _id_solicitud = value;
            }
        }

        private SGI_Tarea_Calificar Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Calificar calificar =
                (
                    from calif in db.SGI_Tarea_Calificar
                    where calif.id_tramitetarea == id_tramitetarea
                    orderby calif.id_calificar descending
                    select calif
                ).ToList().FirstOrDefault();

            return calificar;
        }

        #endregion


        #region acciones 

        private void Redireccionar_VisorTramite()
        {
            int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0); string url = Shared.getRedireccionURL(this.id_solicitud, id_tramitetarea);
            Response.Redirect(url, false);
        }

        protected void ucResultadoTarea_CerrarClick(object sender, EventArgs e)
        {
            Redireccionar_VisorTramite();
        }


        private void Validar_Tarea()
        {
        }

        private void Guardar_tarea(bool finalizar, int id_solicitud, int id_tramite_tarea, string observacion, string observContribuyente,
            string observInternas, string observaciones_LibradoUso, bool librar_uso, Guid userId)
        {

            SGI_Tarea_Calificar calificar = Buscar_Tarea(id_tramite_tarea);

            int id_calificar = 0;
            if (calificar != null)
                id_calificar = calificar.id_calificar;

            int id = db.SGI_Tarea_Calificar_Actualizar(id_calificar, id_tramite_tarea, observacion, observContribuyente, observInternas, null, observaciones_LibradoUso, librar_uso, userId);

            if (finalizar && !string.IsNullOrEmpty(observContribuyente))
                db.SSIT_Solicitudes_AgregarObservaciones(id_solicitud, observContribuyente, userId);

        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {

                Guid userid = Functions.GetUserId();

                IniciarEntity();

                Validar_Tarea();

                using (TransactionScope Tran = new TransactionScope())
                {

                    try
                    {

                        Guardar_tarea(false, this.id_solicitud, this.TramiteTarea, ucObservacionesTarea.Text, ucObservacionContribuyente.Text,
                            ucObservacionesInternas.Text, UcObservacionesLibrarUso.Text, chbLibrarUso.Checked, userid);

                        db.SaveChanges();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. calificar-ucResultadoTarea_GuardarClick");
                        throw ex;
                    }

                }
                FinalizarEntity();

                Enviar_Mensaje("Se ha guardado la tarea.", "");

                Redireccionar_VisorTramite();

            }
            catch (Exception ex)
            {
                FinalizarEntity();
                Enviar_Mensaje(ex.Message, "");
            }

        }

        protected void ucSGI_listaPlanoVisado_GuardarClick(object sender, ucSGI_listaPlanoVisadoEventsArgs e)
        {
            try
            {
                db = new DGHP_Entities();
                Guid userid = Functions.GetUserId();
                int id_doc = 0;

                db.Solicitud_planoVisado_Eliminar(this.id_solicitud, this.TramiteTarea);
                GridView grd_plan_visado = (GridView)ucSGI_ListaPlanoVisado.FindControl("grd_plan_visado");
                foreach (GridViewRow row in grd_plan_visado.Rows)
                {
                    CheckBox chkPlanoElegido = (CheckBox)row.FindControl("chkPlanoVisado");
                    id_doc = 0;
                    int.TryParse(grd_plan_visado.DataKeys[row.RowIndex].Values["id_doc_adj"].ToString(), out id_doc);
                    if (chkPlanoElegido.Checked)
                    {
                        db.Solicitud_planoVisado_Agregar(this.id_solicitud, this.TramiteTarea, userid, id_doc);
                    }
                }
                db.Dispose();
            }
            catch (Exception ex)
            {
                db.Dispose();
                throw new Exception(ex.Message);
            }

        }

        private void Validar_Finalizar()
        {
            //Si el resultado es 'Realizado' estamos en la tarea 'Solicitud en Espera' no debe evaluar las condiciones propuestas.
            if (ucResultadoTarea.getIdResultadoTarea() != (int)Constants.ENG_ResultadoTarea.Realizado)
            {
                if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Calificar_Pedir_Rectificacion && ucObservaciones.countObservaciones == 0)
                    throw new Exception("Debe especificar la Documentacion a Presentar.");
                else if (ucResultadoTarea.getIdResultadoTarea() != (int)Constants.ENG_ResultadoTarea.Calificar_Pedir_Rectificacion && ucObservaciones.countObservaciones != 0)
                    throw new Exception("No se debe especificar la Documentacion a Presentar.");
            }

            if (this.id_tarea == (int)Constants.ENG_Tareas.ESP_Calificar_1 ||
               this.id_tarea == (int)Constants.ENG_Tareas.ESPAR_Calificar_1 ||
               this.id_tarea == (int)Constants.ENG_Tareas.ESP_Calificar_2 ||
               this.id_tarea == (int)Constants.ENG_Tareas.ESPAR_Calificar_2 ||
               this.id_tarea == (int)Constants.ENG_Tareas.ESP_Calificar_1_Nuevo ||
               this.id_tarea == (int)Constants.ENG_Tareas.ESPAR_Calificar_1_Nuevo ||
               this.id_tarea == (int)Constants.ENG_Tareas.ESP_Calificar_2_Nuevo ||
               this.id_tarea == (int)Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo ||
               this.id_tarea == (int)Constants.ENG_Tareas.ESCU_HP_Calificar_1 ||
               this.id_tarea == (int)Constants.ENG_Tareas.ESCU_HP_Calificar_2 ||
               this.id_tarea == (int)Constants.ENG_Tareas.ESCU_IP_Calificar_1 ||
               this.id_tarea == (int)Constants.ENG_Tareas.ESCU_IP_Calificar_2)
            {
                if (ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.ESP_Verificacion_AVH ||
                    ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.ESPAR_Verificacion_AVH ||
                    ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.ESP_Verificacion_AVH_Nuevo ||
                    ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.ESPAR_Verificacion_AVH_Nuevo ||
                    ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.ESCU_HP_Verificacion_AVH ||
                    ucResultadoTarea.getIdProximaTarea() == (int)Constants.ENG_Tareas.ESCU_IP_Verificacion_AVH)
                {
                    var list_doc_adj =
                        (
                            from adj in db.SGI_Tarea_Documentos_Adjuntos
                            where adj.id_tramitetarea == this.TramiteTarea
                            && adj.id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Informe_AVH
                            select new
                            {
                                adj
                            }
                        );
                    if (list_doc_adj.Count() == 0)
                        throw new Exception("Debe subir el Informe de AVH.");
                }
            }

            SSIT_Solicitudes sol = new SSIT_Solicitudes();
            sol = db.SSIT_Solicitudes.Where(x => x.id_solicitud == this.id_solicitud).FirstOrDefault();

            if (pnl_Librar_Uso.Visible)
            {

                if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Aprobado
                    && !chbLibrarUso.Checked
                    && sol.id_tipoexpediente == (int)Constants.TipoDeExpediente.Simple
                    )
                    throw new Exception("Es obligatorio tildar Librar uso.");
            }

            var tarea = db.ENG_Tareas.Where(x => x.id_tarea == id_tarea).First();

            //143479: JADHE YYYYY - SGI - Plano visado en Escuelas
            List<int> listCircuitosEscuelas = new List<int>();
            listCircuitosEscuelas.Add((int)Constants.ENG_Circuitos.SCP5);
            listCircuitosEscuelas.Add((int)Constants.ENG_Circuitos.ESCU_HP);
            listCircuitosEscuelas.Add((int)Constants.ENG_Circuitos.AMP_SCP5);
            listCircuitosEscuelas.Add((int)Constants.ENG_Circuitos.AMP_ESCU_HP);
            listCircuitosEscuelas.Add((int)Constants.ENG_Circuitos.RU_SCP5);
            listCircuitosEscuelas.Add((int)Constants.ENG_Circuitos.RU_ESCU_HP);

            if (!listCircuitosEscuelas.Contains(tarea.id_circuito))
            {
                if (tarea.cod_tarea == Convert.ToInt32(tarea.id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Calificar) ||
               tarea.cod_tarea == Convert.ToInt32(tarea.id_circuito.ToString() + Constants.ENG_Tipos_Tareas.Calificar2))
                {
                    if (ucSGI_ListaPlanoVisado.getSeleccionPlanos() <= 0 &&
                        ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Aprobado &&
                        tarea.ENG_Circuitos.nombre_grupo != Constants.grupoCircuito.SSP &&
                        tarea.ENG_Circuitos.nombre_grupo != Constants.grupoCircuito.SSPA)
                    {
                        throw new Exception("Debe seleccionar el archivo correspondiente a Plano Visado.");
                    }
                }
            }
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {

            try
            {
                Guid userid = Functions.GetUserId();
                int id_tramitetarea_nuevo = 0;

                IniciarEntity();

                Validar_Finalizar();

                Guardar_tarea(true, this.id_solicitud, this.TramiteTarea, ucObservacionesTarea.Text, ucObservacionContribuyente.Text,
                    ucObservacionesInternas.Text, UcObservacionesLibrarUso.Text, chbLibrarUso.Checked, userid);

                bool hayProcesosGenerados = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == TramiteTarea) > 0;

                bool sinProceso = false;
                //Esto con los nuevos circuitos no ira 
                if (this.id_tarea != (int)Constants.ENG_Tareas.ESCU_HP_Calificar_1 &&
                    this.id_tarea != (int)Constants.ENG_Tareas.ESCU_HP_Calificar_2 &&
                    this.id_tarea != (int)Constants.ENG_Tareas.ESCU_IP_Calificar_1 &&
                    this.id_tarea != (int)Constants.ENG_Tareas.ESCU_IP_Calificar_2)
                {
                    sinProceso = true;
                }
                if (!sinProceso && !hayProcesosGenerados)
                {
                    if (this.id_tarea == (int)Constants.ENG_Tareas.ESCU_HP_Calificar_1 ||
                        this.id_tarea == (int)Constants.ENG_Tareas.ESCU_IP_Calificar_1)
                        db.SGI_HAB_GenerarProcesos_SADE_v4_Calificador(this.TramiteTarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    else
                        db.SGI_HAB_GenerarProcesos_SADE_v4(this.TramiteTarea, ucResultadoTarea.getIdProximaTarea(), userid);
                    ucResultadoTarea.btnFinalizar_Enabled = false;
                    ucProcesosSADE.cargarDatosProcesos(this.TramiteTarea, true);
                }
                else if (Functions.EsForzarTarasSade() || sinProceso || !ucProcesosSADE.hayProcesosPendientesSADE(this.TramiteTarea))
                {
                    //Se saco la transicion porque cuando genera la oblea se queda sin tiempo
                    //using (TransactionScope Tran = new TransactionScope())
                    //{
                    try
                    {
                        db.SaveChanges();

                        id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                        var plano = db.Solicitud_planoVisado.Where(x => x.id_tramiteTarea == TramiteTarea).ToList();
                        if (plano != null &&
                            ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Aprobado)
                        {
                            foreach (var item in plano)
                                db.Solicitud_planoVisado_Agregar(this.id_solicitud, id_tramitetarea_nuevo, userid, item.id_docAdjunto);
                        }
                        var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == this.id_solicitud);

                        // Si tiene Normativa y el calificador aprobo el trámite y es cualquier tareas de circuito 2 se debe generar el Qr
                        if (chbLibrarUso.Checked && sol.FechaLibrado == null)
                        {
                            if (Documentos.generarDocumentoInicio(this.id_solicitud))
                            {
                                try
                                {
                                    db.SSIT_Solicitudes_Set_FechaLibrado(id_solicitud);
                                    var cmd = db.Database.Connection.CreateCommand();
                                    cmd.CommandText = string.Format("EXEC SSIT_Solicitudes_Historial_LibradoUso_INSERT {0} {0} '{0}'", id_solicitud, 1, userid);
                                    cmd.CommandTimeout = 120;
                                    try
                                    {
                                        db.Database.Connection.Open();
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception exe)
                                    {
                                        throw exe;
                                    }
                                    finally
                                    {
                                        db.Database.Connection.Close();
                                        cmd.Dispose();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogError.Write(ex, "Error actualizando la Fecha de Librado al uso.");
                                }
                                try
                                {
                                    Encuestas.enviarEncuesta(id_solicitud);
                                }
                                catch (Exception ex)
                                {
                                    LogError.Write(ex, "Error en ws encuesta");
                                }
                                Mailer.MailMessages.SendMail_DisponibilzarQR_v2(this.id_solicitud);
                            }

                            // Si el estado es suspendida se pasa a en trámite.
                            if (sol.id_estado == (int)Constants.Solicitud_Estados.Suspendida)
                            {
                                db.SSIT_Solicitudes_ActualizarEstado(this.id_solicitud, (int)Constants.Solicitud_Estados.En_trámite, userid, sol.NroExpediente, sol.telefono);
                            }
                        }
                        else if (chbLibrarUso.Checked == false && sol.FechaLibrado != null)
                        {
                            sol.FechaLibrado = null;
                            db.SSIT_Solicitudes.AddOrUpdate(sol);
                            db.SaveChanges();
                            var cmd = db.Database.Connection.CreateCommand();
                            cmd.CommandText = string.Format("EXEC SSIT_Solicitudes_Historial_LibradoUso_INSERT {0} {0} '{0}'", id_solicitud, 0, userid);
                            cmd.CommandTimeout = 120;
                            try
                            {
                                db.Database.Connection.Open();
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception exe)
                            {
                                throw exe;
                            }
                            finally
                            {
                                db.Database.Connection.Close();
                                cmd.Dispose();
                            }
                        }

                        string mensaje_envio_mail = "";
                        try
                        {
                            var cod_tar = db.ENG_Tareas.Where(x => x.id_tarea == e.id_proxima_tarea).Select(y => y.cod_tarea).FirstOrDefault();

                            if (e.id_proxima_tarea == (int)Constants.ENG_Tareas.SSP_Correccion_Solicitud ||
                                e.id_proxima_tarea == (int)Constants.ENG_Tareas.SCP_Correccion_Solicitud)
                                Mailer.MailMessages.SendMail_CorreccionSolicitud_v2(this.id_solicitud);
                            if (cod_tar.ToString().Substring(cod_tar.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Correccion_Solicitud)
                                Mailer.MailMessages.SendMail_ObservacionSolicitud_v2(this.id_solicitud);
                            if (cod_tar.ToString().Substring(cod_tar.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Revision_SubGerente)
                                Mailer.MailMessages.SendMail_Calificar_v2(this.id_solicitud);
                        }
                        catch (Exception ex)
                        {
                            mensaje_envio_mail = ex.Message;
                        }

                        Enviar_Mensaje("Se ha finalizado la tarea.", "");
                        Redireccionar_VisorTramite();
                    }
                    catch (Exception ex)
                    {
                        //Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. revision_dghp-ucResultadoTarea_FinalizarTareaClick");
                        throw ex;
                    }

                    //}
                }
                else
                {
                    Enviar_Mensaje("No es posible avanzar la tarea si la misma no se encuentra realizada en SADE.", "");
                }
                FinalizarEntity();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                string message = ex.Message;
                if (ex.InnerException != null)
                    message = ex.InnerException.Message;
                Enviar_Mensaje(message, "");
            }
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        protected void ucProcesosSADE_FinalizadoEnSADE(object sender, EventArgs e)
        {
            // Cuando se cierra el modal de procesos si no hay pendientes en SADE se dispara esta accion
            //ucResultadoTarea_FinalizarTareaClick(sender, new ucResultadoTareaEventsArgs());
        }

        protected void ucResultadoTarea_ResultadoSelectedIndexChanged(object sender, ucResultadoTareaEventsArgs e)
        {
            IniciarEntity();
            if (ucResultadoTarea.getIdResultadoTarea() == (int)Constants.ENG_ResultadoTarea.Aprobado)
            {
                ucObservacionesTarea.Text = Parametros.GetParam_ValorChar("SGI.Notas.Adicionales.Dispo") + "\n\r" + ucObservacionesTarea.Text;
                ucObservacionesTarea.Update();
            }
            else
            {
                ucObservacionesTarea.Text = ucObservacionesTarea.Text.Replace(Parametros.GetParam_ValorChar("SGI.Notas.Adicionales.Dispo"), "");
                ucObservacionesTarea.Update();
            }
            FinalizarEntity();
        }

        #endregion

        #region entity

        private DGHP_Entities db = null;
        private AGC_FilesEntities dbFiles = null;

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

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
            {
                this.dbFiles.Dispose();
                this.dbFiles = null;
            }
        }

        #endregion


        public bool EsInmuebleCatalogado(int IdEncomienda)
        {
            return db.Encomienda_Ubicaciones.Any(encubic => encubic.id_encomienda == IdEncomienda && encubic.InmuebleCatalogado == true);
        }

    }
}
