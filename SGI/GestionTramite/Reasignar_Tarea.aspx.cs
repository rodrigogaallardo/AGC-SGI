using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using SGI.Model;
using System.Web.UI.WebControls;
using System.Data.Entity.Core.Objects;
using System.Web.Providers.Entities;

namespace SGI
{
    public partial class Reasignar_Tarea : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm != null && sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlEquipo, updPnlEquipo.GetType(),
                        "rt_incializar_dllEquipo", "rt_incializar_dllEquipo();", true);
            }

            if (!IsPostBack)
            {
                cargarDatos();
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
        }

        private void cargarDatos()
        {
            IniciarEntity();

            Guid userid = Functions.GetUserId();

            hid_ddlEquipo.Value = "";

            List<Servicios.clsEquipoTrabajo> lstEquipoTrabajo = getEquipoTrabajo(userid);

            // carga de combo con usuarios
            ddlEquipo.DataValueField = "userid";
            ddlEquipo.DataTextField = "nombres_apellido";
            ddlEquipo.DataSource = lstEquipoTrabajo;
            ddlEquipo.DataBind();

            if (lstEquipoTrabajo.Count > 0)
                ddlEquipo.Items.Insert(0, new ListItem("", "0"));

            updPnlEquipo.Update();

        }

        private List<Servicios.clsEquipoTrabajo> getEquipoTrabajo(Guid userid)
        {
            List<Servicios.clsEquipoTrabajo> lstEquipoTrabajo = new List<Servicios.clsEquipoTrabajo>();
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                try
                {
                    var qUsuario =
                    (
                        from perf in db.SGI_Profiles
                        where perf.userid == userid
                        select new
                        {
                            perf.Apellido,
                            perf.Nombres,
                            perf.userid
                        }
                    ).FirstOrDefault();

                    var qEquipo =
                        (
                            from eq in db.ENG_EquipoDeTrabajo
                            join usr in db.SGI_Profiles on eq.Userid equals usr.userid
                            join mem in db.aspnet_Membership on usr.userid equals mem.UserId
                            where eq.Userid_Responsable == userid
                                && mem.IsApproved == true
                            select new
                            {
                                usr.Apellido,
                                usr.Nombres,
                                userid = usr.userid
                            }
                        ).ToList();

                    lstEquipoTrabajo = new List<Servicios.clsEquipoTrabajo>();
                    SGI.Servicios.clsEquipoTrabajo equipo = null;

                    foreach (var item in qEquipo)
                    {
                        equipo = new Servicios.clsEquipoTrabajo();
                        equipo.nombres_apellido = item.Apellido + " " + item.Nombres;
                        equipo.userid = item.userid.ToString().ToLower();
                        lstEquipoTrabajo.Add(equipo);
                    }

                    // si puedo asignarlo a otros tambien me agrego a mi
                    if (lstEquipoTrabajo.Count > 0)
                    {
                        if (!lstEquipoTrabajo.Exists(x => x.userid.ToLower() == userid.ToString().ToLower()))
                        {
                            equipo = new Servicios.clsEquipoTrabajo();
                            equipo.userid = qUsuario.userid.ToString().ToLower();
                            equipo.nombres_apellido = qUsuario.Apellido + " " + qUsuario.Nombres;
                            lstEquipoTrabajo.Add(equipo);
                        }
                    }
                    txn.Complete();
                    txn.Dispose();
                }
                catch (Exception ex)
                {
                    txn.Dispose();
                    throw ex;
                }
            }

            return lstEquipoTrabajo;
        }

        private List<Servicios.clsEquipoTrabajo> getEquipoTrabajoTarea(int id_tarea, Guid userid)
        {
            List<Servicios.clsEquipoTrabajo> lstEquipoTrabajo = new List<Servicios.clsEquipoTrabajo>();

            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                try
                {
                    var qUsuario =
                    (
                        from perf in db.SGI_Profiles
                        where perf.userid == userid
                        select new
                        {
                            perf.Apellido,
                            perf.Nombres,
                            perf.userid
                        }
                    ).FirstOrDefault();

                    // buscar roles de usuario

                    var qEquipoTrabajo = (from eq in db.ENG_EquipoDeTrabajo
                                          join usr in db.SGI_Profiles on eq.Userid equals usr.userid
                                          join mem in db.aspnet_Membership on usr.userid equals mem.UserId
                                          where eq.Userid_Responsable == qUsuario.userid && mem.IsApproved
                                          select new Servicios.clsEquipoTrabajo
                                          {
                                              userid = eq.Userid.ToString(),
                                              nombres_apellido = usr.Apellido + " " + usr.Nombres
                                          });

                    lstEquipoTrabajo = qEquipoTrabajo.ToList();
                    txn.Complete();
                    txn.Dispose();
                }
                catch (Exception ex)
                {
                    txn.Dispose();
                    throw ex;
                }
            }

            return lstEquipoTrabajo;
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);

            string script_nombre = "mostrarMensaje";
            string script = "mostrarMensaje('" + mensaje + "','" + titulo + "');";

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm != null && sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
            }

        }

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

        private Guid _usuario_seleccionado;
        private Guid usuario_seleccionado
        {
            get
            {
                _usuario_seleccionado = new Guid();
                if (!string.IsNullOrEmpty(hid_ddlEquipo.Value))
                    Guid.TryParse(hid_ddlEquipo.Value, out _usuario_seleccionado);
                return _usuario_seleccionado;
            }
            set
            {
                _usuario_seleccionado = value;
            }
        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {

            try
            {
                IniciarEntity();
                Guid userid = Functions.GetUserId();

                cargarTramites();

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "error al buscar tramites reasignar_tarea-btnBuscar_OnClick");
                Enviar_Mensaje(ex.Message, "");
            }

        }


        public void cargarTramites()
        {

            int totalRowCount = 0;
            totalRowCount = 0;

            Guid userid_asignado = usuario_seleccionado;

            List<clsItemReasignar> lstResult = FiltrarTramites(userid_asignado);

            totalRowCount = lstResult.Count();
            pnlCantRegistros.Visible = true;

            if (totalRowCount > 1)
            {
                lblCantRegistros.Text = string.Format("{0} Trámites", totalRowCount);
            }
            else if (totalRowCount == 1)
                lblCantRegistros.Text = string.Format("{0} Trámite", totalRowCount);
            else
            {
                pnlCantRegistros.Visible = false;
            }
            pnlResultadoBuscar.Visible = true;

            grdTramites.DataSource = lstResult;
            grdTramites.DataBind();
            updPnlResultadoBuscar.Update();

            //return lstResult;
        }

        protected void btnGuadarUsuario_Command(object sender, CommandEventArgs e)
        {
            try
            {
                LinkButton btnGuadarUsuario = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnGuadarUsuario.Parent.Parent.Parent.Parent;

                int id_tramitetarea_nuevo = 0;
                int id_solicitud = Convert.ToInt32(grdTramites.DataKeys[row.RowIndex].Values[0]);

                DropDownList ddlUsuario = (DropDownList)row.FindControl("ddlUsuario");
                Label lbl_usuarioAsignado = (Label)row.FindControl("lbl_usuarioAsignado");
                Label lbl_id_tramitetarea = (Label)row.FindControl("lbl_id_tramitetarea");
                Label lblUsuarioAsignado = (Label)row.FindControl("lblUsuarioAsignado");
                UpdatePanel updPnlReasignarUsuario = (UpdatePanel)row.FindControl("updPnlReasignarUsuario");


                LinkButton btnEdit = (LinkButton)row.FindControl("btnEdit");
                LinkButton btnCancel = (LinkButton)row.FindControl("btnCancel");


                Guid? usuarioNuevo = null;
                if (ddlUsuario.SelectedIndex != 0)
                    usuarioNuevo = Guid.Parse(ddlUsuario.SelectedValue);

                Guid usuarioAsignado = new Guid();
                Guid.TryParse(lbl_usuarioAsignado.Text, out usuarioAsignado);

                int id_tramitetarea = 0;
                int.TryParse(lbl_id_tramitetarea.Text, out id_tramitetarea);

                IniciarEntity();

                id_tramitetarea_nuevo = reasignar(id_tramitetarea, usuarioNuevo);

                lbl_usuarioAsignado.Text = usuarioNuevo.ToString();
                lbl_id_tramitetarea.Text = id_tramitetarea_nuevo.ToString();
                lblUsuarioAsignado.Text = ddlUsuario.SelectedItem.Text;
                FinalizarEntity();
                updPnlReasignarUsuario.Update();

                    
                ddlUsuario.Style.Add("display", "none");
                btnCancel.Style.Add("display", "none");
                lblUsuarioAsignado.Style.Add("display", "block");
                btnEdit.Style.Add("display", "block");
                btnGuadarUsuario.Style.Add("display", "none");


            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Error al reasignar trámite-reasignar_tarea-btnGuadarUsuario_Command");
                Enviar_Mensaje(ex.Message, "");
            }

        }

        private int reasignar(int id_tramitetarea, Guid? userid)
        {

            int id_tramitetarea_nuevo = 0;
            TransactionScope Tran = new TransactionScope();

            try
            {
                id_tramitetarea_nuevo = Engine.ReasignarTarea(id_tramitetarea, userid);

                Tran.Complete();
                Tran.Dispose();
            }
            catch (Exception ex)
            {
                if (Tran != null) Tran.Dispose();
                LogError.Write(ex, "error en transaccion- reasignar_tarea-reasignar");
                throw ex;
            }

            return id_tramitetarea_nuevo;
        }

        #region "Filtro"
        private List<clsItemReasignar> FiltrarTramites(Guid userid_asignado)
        {

            List<clsItemReasignar> resultados = new List<clsItemReasignar>();
            IQueryable<clsItemReasignar> qFinal = null;
            IQueryable<clsItemReasignar> qENC = null;
            IQueryable<clsItemReasignar> qCP = null;
            IQueryable<clsItemReasignar> qTR = null;

            DGHP_Entities db = new DGHP_Entities();

            List<int> tareasNoReasignable = new List<int>();

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP_Generar_Expediente); //46
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP_Generar_Expediente_Nuevo); //407

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP_Revision_Firma_Disposicion);
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP_Revision_Firma_Disposicion_Nuevo);
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP_Asignar_Calificador_Gerente_Nuevo);

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SSP_Generar_Expediente); //22
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SSP_Generar_Expediente_Nuevo); //307

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SSP_Revision_Firma_Disposicion);
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SSP_Revision_Firma_Disposicion);
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SSP_Revision_Firma_Disposicion_Nuevo);
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SSP_Asignar_Calificador_Gerente_Nuevo);

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SSP_Generar_Expediente_v3); //701

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESP_Generar_Expediente); //113
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESP_Generar_Expediente_Nuevo); //514

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESP_Revision_Firma_Disposicion);
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESP_Revision_Firma_Disposicion_Nuevo);
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESP_Asignar_Calificador_Nuevo);

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESPAR_Generar_Expediente); //215
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESPAR_Generar_Expediente_Nuevo); //614

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESPAR2_Redist_Generar_Expediente_HPV2); //1159
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESPAR2_Amp_Generar_Expediente_HPV2); //1270

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESPAR_Revision_Firma_Disposicion); //
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESPAR_Revision_Firma_Disposicion_Nuevo); //
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESPAR_Asignar_Calificador_Nuevo); //

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP2_Generar_Expediente); // 1137
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP2_Redist_Generar_Expediente); // 1248

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP3_Redist_Asignacion_al_Calificador_Gerente); // 1248

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP3_Redist_Generar_Expediente); // 1325
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP3_Amp_Generar_Expediente); // 1218
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP3_Generar_Expediente); // 1007

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP4_Generar_Expediente); // 1107
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP4_Redist_Generar_Expediente); // 1340
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SCP4_Amp_Generar_Expediente); // 1233            

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESCU_SCP_Generar_Expediente_ASCPESV2); // 134429     
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESCU_SCP_Generar_Expediente_ESCU_HSCPES); // 801     
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESCU_SCP_Generar_Expediente_ASCPES); // 1173     
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESCU_SCP_Generar_Expediente_RUSCPES); // 1280     

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.CP_Generar_Expediente); // 56

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.TR_Generar_Expediente); // 66

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.TR_Revision_Firma_Disposicion); // 

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.TR_Nueva_Generar_Expediente); // 13215

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESCU_HP_Generar_Expediente); //901
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESCU_SCP_Generar_Expediente_AHP); //1191
            tareasNoReasignable.Add((int)Constants.ENG_Tareas.ESCU_SCP_Generar_Expediente_RUHP); //1298

            tareasNoReasignable.Add((int)Constants.ENG_Tareas.SSP2_Generar_Expediente); //1122

            // Bandeja de datos Encomienda
            #region "Consulta Encomienda"
            qENC = (from perfiles_tareas in db.ENG_Rel_Perfiles_Tareas
                    join tramite_tareas in db.SGI_Tramites_Tareas on perfiles_tareas.id_tarea equals tramite_tareas.id_tarea
                    join tramite_tareas_HAB in db.SGI_Tramites_Tareas_HAB on tramite_tareas.id_tramitetarea equals tramite_tareas_HAB.id_tramitetarea
                    join tarea in db.ENG_Tareas on tramite_tareas.id_tarea equals tarea.id_tarea
                    join sol in db.SSIT_Solicitudes on tramite_tareas_HAB.id_solicitud equals sol.id_solicitud
                    join user in db.aspnet_Users on tramite_tareas.UsuarioAsignado_tramitetarea equals user.UserId
                    join usr in db.SGI_Profiles on tramite_tareas.UsuarioAsignado_tramitetarea equals usr.userid


                    where tramite_tareas.UsuarioAsignado_tramitetarea == userid_asignado
                    && tramite_tareas.FechaCierre_tramitetarea == null
                    && ((tarea.Asignable_tarea == false) || (tarea.Asignable_tarea == true && tramite_tareas.UsuarioAsignado_tramitetarea != null))
                    //&& !tareasNoReasignable.Contains(tramite_tareas.id_tarea)
                    && sol.id_estado != (int)Constants.Solicitud_Estados.Anulado
                    select new clsItemReasignar
                    {
                        cod_grupotramite = Constants.GruposDeTramite.HAB.ToString(),
                        tipoTramite = sol.TipoTramite.descripcion_tipotramite,
                        id_tramitetarea = tramite_tareas.id_tramitetarea,
                        FechaInicio_tramitetarea = tramite_tareas.FechaInicio_tramitetarea,
                        FechaAsignacion_tramtietarea = tramite_tareas.FechaAsignacion_tramtietarea,
                        UsuarioAsignado_tramitetarea = tramite_tareas.UsuarioAsignado_tramitetarea.Value,
                        UsuarioAsignado_tramitetarea_username = usr.Apellido + " " + usr.Nombres,
                        id_solicitud = sol.id_solicitud,
                        direccion = "",
                        id_tarea = tarea.id_tarea,
                        nombre_tarea = tarea.nombre_tarea,
                        asignable_tarea = tarea.Asignable_tarea,
                        tomar_tarea = (tarea.id_tarea != 25 && tarea.id_tarea != 49) ? true : false,//Correcion de solicitudes
                        formulario_tarea = tarea.formulario_tarea,
                        Dias_Transcurridos = 0,
                        superficie_total = 0,
                        cant_observaciones = sol.SSIT_Solicitudes_Observaciones.Count(),
                        url_visorTramite = "~/GestionTramite/VisorTramite.aspx?id={0}",
                        url_tareaTramite = "~/GestionTramite/Tareas/{0}?id={1}"
                    }).Distinct();
            #endregion

            // Consulta de datos CPadron
            #region "Consulta CPadron"
            qCP = (from perfiles_tareas in db.ENG_Rel_Perfiles_Tareas
                   join tramite_tareas in db.SGI_Tramites_Tareas on perfiles_tareas.id_tarea equals tramite_tareas.id_tarea
                   join tramite_tareas_CP in db.SGI_Tramites_Tareas_CPADRON on tramite_tareas.id_tramitetarea equals tramite_tareas_CP.id_tramitetarea
                   join tarea in db.ENG_Tareas on tramite_tareas.id_tarea equals tarea.id_tarea
                   join sol in db.CPadron_Solicitudes on tramite_tareas_CP.id_cpadron equals sol.id_cpadron
                   join user in db.aspnet_Users on tramite_tareas.UsuarioAsignado_tramitetarea equals user.UserId
                   join usr in db.SGI_Profiles on tramite_tareas.UsuarioAsignado_tramitetarea equals usr.userid

                   join eDatos in db.CPadron_DatosLocal on sol.id_cpadron equals eDatos.id_cpadron into zr
                   from ed in zr.DefaultIfEmpty()
                   where tramite_tareas.UsuarioAsignado_tramitetarea == userid_asignado
                   && tramite_tareas.FechaCierre_tramitetarea == null
                   && ((tarea.Asignable_tarea == false) || (tarea.Asignable_tarea == true && tramite_tareas.UsuarioAsignado_tramitetarea != null))
                   //&& !tareasNoReasignable.Contains(tramite_tareas.id_tarea)
                   select new clsItemReasignar
                   {
                       cod_grupotramite = Constants.GruposDeTramite.CP.ToString(),
                       tipoTramite = sol.TipoTramite.descripcion_tipotramite,
                       id_tramitetarea = tramite_tareas.id_tramitetarea,
                       FechaInicio_tramitetarea = tramite_tareas.FechaInicio_tramitetarea,
                       FechaAsignacion_tramtietarea = tramite_tareas.FechaAsignacion_tramtietarea,
                       UsuarioAsignado_tramitetarea = tramite_tareas.UsuarioAsignado_tramitetarea.Value,
                       UsuarioAsignado_tramitetarea_username = usr.Apellido + " " + usr.Nombres,
                       id_solicitud = sol.id_cpadron,
                       direccion = "",
                       id_tarea = tarea.id_tarea,
                       nombre_tarea = tarea.nombre_tarea,
                       asignable_tarea = tarea.Asignable_tarea,
                       tomar_tarea = tarea.id_tarea != 71 ? true : false,//Correcion de solicitudes
                       formulario_tarea = tarea.formulario_tarea,
                       Dias_Transcurridos = 0,
                       superficie_total = ed != null ? (ed.superficie_cubierta_dl.Value + ed.superficie_descubierta_dl.Value) : 0,
                       cant_observaciones = sol.CPadron_Solicitudes_Observaciones.Count(),
                       url_visorTramite = "~/VisorTramiteCP/{0}",
                       url_tareaTramite = "~/GestionTramite/Tareas/{0}?id={1}"
                   }).Distinct();
            #endregion

            // Bandeja de datos Transferencias
            #region "Consulta Transferencias"
            qTR = (from perfiles_tareas in db.ENG_Rel_Perfiles_Tareas
                   join tramite_tareas in db.SGI_Tramites_Tareas on perfiles_tareas.id_tarea equals tramite_tareas.id_tarea
                   join tramite_tareas_TR in db.SGI_Tramites_Tareas_TRANSF on tramite_tareas.id_tramitetarea equals tramite_tareas_TR.id_tramitetarea
                   join tarea in db.ENG_Tareas on tramite_tareas.id_tarea equals tarea.id_tarea
                   join sol in db.Transf_Solicitudes on tramite_tareas_TR.id_solicitud equals sol.id_solicitud
                   join user in db.aspnet_Users on tramite_tareas.UsuarioAsignado_tramitetarea equals user.UserId
                   join usr in db.SGI_Profiles on tramite_tareas.UsuarioAsignado_tramitetarea equals usr.userid
                   join eDatos in db.CPadron_DatosLocal on sol.id_cpadron equals eDatos.id_cpadron into zr
                   from ed in zr.DefaultIfEmpty()

                   where tramite_tareas.UsuarioAsignado_tramitetarea == userid_asignado
                   && tramite_tareas.FechaCierre_tramitetarea == null
                   && ((tarea.Asignable_tarea == false) || (tarea.Asignable_tarea == true && tramite_tareas.UsuarioAsignado_tramitetarea != null))
                   //&& !tareasNoReasignable.Contains(tramite_tareas.id_tarea)
                   select new clsItemReasignar
                   {
                       cod_grupotramite = Constants.GruposDeTramite.TR.ToString(),
                       tipoTramite = sol.TipoTramite.descripcion_tipotramite,
                       id_tramitetarea = tramite_tareas.id_tramitetarea,
                       FechaInicio_tramitetarea = tramite_tareas.FechaInicio_tramitetarea,
                       FechaAsignacion_tramtietarea = tramite_tareas.FechaAsignacion_tramtietarea,
                       UsuarioAsignado_tramitetarea = tramite_tareas.UsuarioAsignado_tramitetarea.Value,
                       UsuarioAsignado_tramitetarea_username = usr.Apellido + " " + usr.Nombres,
                       id_solicitud = sol.id_solicitud,
                       direccion = "",
                       id_tarea = tarea.id_tarea,
                       nombre_tarea = tarea.nombre_tarea,
                       asignable_tarea = tarea.Asignable_tarea,
                       tomar_tarea = (tarea.id_tarea != 60) ? true : false,//Correcion de solicitudes
                       formulario_tarea = tarea.formulario_tarea,
                       Dias_Transcurridos = 0,
                       superficie_total = ed != null ? (ed.superficie_cubierta_dl.Value + ed.superficie_descubierta_dl.Value) : 0,
                       cant_observaciones = sol.Transf_Solicitudes_Observaciones.Count(),
                       url_visorTramite = "~/VisorTramiteTR/{0}",
                       url_tareaTramite = "~/GestionTramite/Tareas/{0}?id={1}"
                   }).Distinct();
            #endregion

            var qTareaENC = (from enc in qENC
                             join tareas in db.ENG_Tareas on enc.id_tarea equals tareas.id_tarea
                             join cir in db.ENG_Circuitos on tareas.id_circuito equals cir.id_circuito
                             select new
                             {
                                 nombre = cir.cod_circuito + " - " + enc.nombre_tarea,
                                 id_tarea = enc.id_tarea
                             }).ToList();

            var qTareaCP = (from enc in qCP
                            join tareas in db.ENG_Tareas on enc.id_tarea equals tareas.id_tarea
                            join cir in db.ENG_Circuitos on tareas.id_circuito equals cir.id_circuito
                            select new
                            {
                                nombre = cir.cod_circuito + " - " + enc.nombre_tarea,
                                id_tarea = enc.id_tarea
                            }).ToList();

            var qTareaTR = (from enc in qTR
                            join tareas in db.ENG_Tareas on enc.id_tarea equals tareas.id_tarea
                            join cir in db.ENG_Circuitos on tareas.id_circuito equals cir.id_circuito
                            select new
                            {
                                nombre = cir.cod_circuito + " - " + enc.nombre_tarea,
                                id_tarea = enc.id_tarea
                            }).ToList();

            AddQueryFinal(qENC, ref qFinal);
            AddQueryFinal(qCP, ref qFinal);
            AddQueryFinal(qTR, ref qFinal);

            int totalRowCount = qFinal.Count();

            resultados = qFinal.ToList();

            #region "Domicilios y datos adicionales"
            if (resultados.Count > 0)
            {

                //------------------------------
                //Obtener las Direcciones del ENC
                //-------------------------------
                List<clsItemDireccion> lstDireccionesENC = new List<clsItemDireccion>();
                string[] arrSolicitudesENC = (from r in resultados
                                              where r.cod_grupotramite == Constants.GruposDeTramite.HAB.ToString()
                                              select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesENC.Length > 0)
                    lstDireccionesENC = Shared.GetDireccionesENC(arrSolicitudesENC);

                //------------------------------
                //Obtener las Direcciones del ENC
                //-------------------------------
                List<clsItemDireccion> lstDireccionesCP = new List<clsItemDireccion>();
                string[] arrSolicitudesCP = (from r in resultados
                                             where r.cod_grupotramite == Constants.GruposDeTramite.CP.ToString()
                                             select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesCP.Length > 0)
                    lstDireccionesCP = Shared.GetDireccionesCP(arrSolicitudesCP);

                //------------------------------
                //Obtener las Direcciones del ENC
                //-------------------------------
                List<clsItemDireccion> lstDireccionesTR = new List<clsItemDireccion>();
                string[] arrSolicitudesTR = (from r in resultados
                                             where r.cod_grupotramite == Constants.GruposDeTramite.TR.ToString()
                                             select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesTR.Length > 0)
                    lstDireccionesTR = Shared.GetDireccionesTR(arrSolicitudesTR);

                var listGrup = (from g in db.SGI_Tarea_Calificar_ObsGrupo
                                join tt in db.SGI_Tramites_Tareas_HAB on g.id_tramitetarea equals tt.id_tramitetarea
                                select new { tt.id_solicitud }).ToList();

                //------------------------------------------------------------------------
                //Rellena la clase a devolver con los datos que faltaban (Direccion, dias transcurrido)
                //------------------------------------------------------------------------
                foreach (var row in resultados)
                {
                    clsItemDireccion itemDireccion = null;
                    if (row.cod_grupotramite == Constants.GruposDeTramite.HAB.ToString())
                    {
                        itemDireccion = lstDireccionesENC.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);
                        int count = listGrup.Count(x => x.id_solicitud == row.id_solicitud);
                        if (count > 0)
                            row.cant_observaciones = count;
                        var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == row.id_solicitud
                                                && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                        row.superficie_total = enc.Encomienda_DatosLocal.First().superficie_cubierta_dl.Value + enc.Encomienda_DatosLocal.First().superficie_descubierta_dl.Value;
                    }
                    else if (row.cod_grupotramite == Constants.GruposDeTramite.CP.ToString())
                        itemDireccion = lstDireccionesCP.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);
                    else if (row.cod_grupotramite == Constants.GruposDeTramite.TR.ToString())
                        itemDireccion = lstDireccionesTR.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);

                    // Llenado para todos
                    if (itemDireccion != null)
                        row.direccion = (string.IsNullOrEmpty(itemDireccion.direccion) ? "" : itemDireccion.direccion);
                    row.url_visorTramite = string.Format(row.url_visorTramite, row.id_solicitud.ToString());
                    if (row.formulario_tarea != null)
                        row.url_tareaTramite = string.Format(row.url_tareaTramite, row.formulario_tarea.Substring(0, row.formulario_tarea.IndexOf(".")), row.id_tramitetarea.ToString());
                    else
                        row.url_tareaTramite = "";
                    //row.Dias_Transcurridos = (DateTime.Now - row.FechaInicio_tarea).Days;
                    row.Dias_Transcurridos = Shared.GetBusinessDays(row.FechaInicio_tramitetarea, DateTime.Now);
                }
            }

            #endregion
            db.Dispose();

            return resultados;
        }

        private void AddQueryFinal(IQueryable<clsItemReasignar> query, ref IQueryable<clsItemReasignar> qFinal)
        {
            if (query != null)
            {
                if (qFinal != null)
                    qFinal = qFinal.Union(query);
                else
                    qFinal = query;
            }
        }

        #endregion



        protected void btnCancel_Command(object sender, CommandEventArgs e)
        {
            LinkButton btnCancel = (LinkButton)sender;

            GridViewRow row = (GridViewRow)btnCancel.Parent.Parent.Parent.Parent;

            int id_tramitetarea_nuevo = 0;
            int id_solicitud = Convert.ToInt32(grdTramites.DataKeys[row.RowIndex].Values[0]);

            DropDownList ddlUsuario = (DropDownList)row.FindControl("ddlUsuario");
            Label lbl_usuarioAsignado = (Label)row.FindControl("lbl_usuarioAsignado");
            Label lbl_id_tramitetarea = (Label)row.FindControl("lbl_id_tramitetarea");
            Label lblUsuarioAsignado = (Label)row.FindControl("lblUsuarioAsignado");
            LinkButton btnGuadarUsuario = (LinkButton)row.FindControl("btnGuadarUsuario");
            LinkButton btnEdit = (LinkButton)row.FindControl("btnEdit");

            ddlUsuario.Style.Add("display", "none");
            btnGuadarUsuario.Style.Add("display", "none");
            btnCancel.Style.Add("display", "none");
            lblUsuarioAsignado.Style.Add("display", "block");
            btnEdit.Style.Add("display", "block");
        }

        protected void btnEdit_Command(object sender, CommandEventArgs e)
        {
            LinkButton btnEdit = (LinkButton)sender;


            GridViewRow row = (GridViewRow)btnEdit.Parent.Parent.Parent.Parent;

            int id_tramitetarea_nuevo = 0;
            int id_solicitud = Convert.ToInt32(grdTramites.DataKeys[row.RowIndex].Values[0]);

            DropDownList ddlUsuario = (DropDownList)row.FindControl("ddlUsuario");
            Label lbl_usuarioAsignado = (Label)row.FindControl("lbl_usuarioAsignado");
            Label lbl_id_tramitetarea = (Label)row.FindControl("lbl_id_tramitetarea");
            Label lblUsuarioAsignado = (Label)row.FindControl("lblUsuarioAsignado");
            LinkButton btnGuadarUsuario = (LinkButton)row.FindControl("btnGuadarUsuario");
            LinkButton btnCancel = (LinkButton)row.FindControl("btnCancel");

            ddlUsuario.Style.Add("display", "block");
            //btnGuadarUsuario.Style.Add("display", "hiden");
            btnCancel.Style.Add("display", "block");
            lblUsuarioAsignado.Style.Add("display", "none");
            btnEdit.Style.Add("display", "none");

            IniciarEntity();

            int id_tramitetarea = 0;
            int.TryParse(lbl_id_tramitetarea.Text, out id_tramitetarea);

            string usuarioAsignado = Convert.ToString(lbl_usuarioAsignado.Text);

            Guid userid = Functions.GetUserId();
            List<Servicios.clsEquipoTrabajo> lstEquipoTrabajo_tarea = getEquipoTrabajoTarea(id_tramitetarea, userid);

            ddlUsuario.DataValueField = "userid";
            ddlUsuario.DataTextField = "nombres_apellido";
            ddlUsuario.DataSource = lstEquipoTrabajo_tarea;
            ddlUsuario.DataBind();

            ddlUsuario.Items.Insert(0, new ListItem("Sin Asignación", "0"));

            ddlUsuario.SelectedValue = usuarioAsignado.ToString().ToLower();

        }

        protected void ddlUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlUsuario = (DropDownList)sender;

            GridViewRow row = (GridViewRow)ddlUsuario.Parent.Parent.Parent.Parent;

            Label lbl_usuarioAsignado = (Label)row.FindControl("lbl_usuarioAsignado");

            string usuarioAsignado = Convert.ToString(lbl_usuarioAsignado.Text);

            LinkButton btnGuadarUsuario = (LinkButton)row.FindControl("btnGuadarUsuario");

            if (usuarioAsignado.ToString().ToLower() != ddlEquipo.SelectedItem.Text) 
            {
                btnGuadarUsuario.Style.Add("display", "block");
            }
            else
            {
                btnGuadarUsuario.Style.Add("display", "none");
            }

        }
    }

}