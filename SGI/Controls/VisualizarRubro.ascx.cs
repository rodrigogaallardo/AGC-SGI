using ExcelLibrary.BinaryFileFormat;
using Newtonsoft.Json;
using SGI.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Controls
{
    public partial class VisualizarRubro : System.Web.UI.UserControl
    {
        DGHP_Entities db = null;
        private string colorBordeOriginal = "#8f8c86";

        private string colorEstadoEnPoceso = "#fcd8b8";
        private string colorBordeEnPoceso = "#ea8f3e";
        private string colorEstadoConfirmada = "#cff3d7";
        private string colorBordeConfirmada = "#68be7b";

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "scriptVisualizarRubro", "grdMouseOver();", true);
            ScriptManager.RegisterStartupScript(pnlDatosRubro, pnlDatosRubro.GetType(), "init_updDatos", "init_updDatos();", true);
            if (!IsPostBack)
            {
                hid_DecimalSeparator.Value = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            }
        }


        #region "Métodos Públicos del control"

        public void CargarDatosUnicaVez()
        {
            CargarTiposDeActividades();
            CargarCircuitos();
            CargarClanae();
            CargarTipoDocumentacionRequerida();
            CargarLicAlcohol();
            CargarZonas();
            CargarCondiciones();
            CargarTipoDocRequeridos();

        }

        public void CargarRubro(string cod_rubro)
        {
            db = new DGHP_Entities();
            //this.GetRows_grdInfoRelevante_Eliminar = null;
            this.GetRows_grdCircAutoZona = null;
            this.GetRows_grdInfoRelevante = null;
            this.GetRows_grdDocRequerido = null;
            this.GetRows_grdDocRequeridoZona = null;
            this.GetRows_grdConfIncendio = null;
            DesmarcarControlesModificados();
            optRubroHistorico.Checked = false;
            optRubroActual.Checked = false;
            pnlDatosSolicitudCambio.Visible = false;
            hid_id_estado_modif.Value = "";
            hid_id_rubhistcam.Value = "0";
            hid_id_rubro.Value = "";
            hid_tipo_solicitudcambio.Value = "";
            btnNuevabusqueda.Visible = true;
            btnNuevabusqueda.Text = "Nueva Búsqueda";
            pnlEstados.Visible = false;
            btnGuardar.Visible = false;
            pnlZonasCondicionesEliminadas.Visible = false;
            pnlAccionesAgregarInfoRelevante.Visible = false;
            pnlAccionesAgregarConfIncendio.Visible = false;
            LimpiarTooltips();

            var q = (from rubros in db.Rubros
                     select new
                     {
                         id_rubro = rubros.id_rubro,
                         nomb_rubro = rubros.nom_rubro,
                         cod_rubro = rubros.cod_rubro,
                         bus_rubro = rubros.bus_rubro,
                         es_anterior_rubro = rubros.EsAnterior_Rubro,
                         tooltip_rubro = rubros.tooltip_rubro,
                         id_tipoactividad = rubros.id_tipoactividad,
                         id_tipodocreq = rubros.id_tipodocreq,
                         VigenciaHasta_rubro = rubros.VigenciaHasta_rubro,
                         rubros.Circuito_Automatico,
                         rubros.Uso_Condicionado,
                         rubros.SupMinCargaDescarga,
                         rubros.SupMinCargaDescargaRefII,
                         rubros.SupMinCargaDescargaRefV,
                         rubros.OficinaComercial,
                         rubros.TieneDeposito,
                         rubros.ValidaCargaDescarga,
                         rubros.id_grupo_circuito,
                         rubros.id_licencia_alcohol,
                         rubros.Librar_Uso,
                         rubros.id_clanae,
                         rubros.local_venta
                     }
            );

            if (cod_rubro != null)
                q = q.Where(x => x.cod_rubro.Equals(cod_rubro));

            int id_rubro = 0;
            foreach (var dr in q.ToList())
            {
                id_rubro = int.Parse(dr.id_rubro.ToString());
                txtCodRubro.Text = dr.cod_rubro.ToString();
                txtDescRubro.Text = dr.nomb_rubro.ToString();
                txtBusqueda.Text = dr.bus_rubro.ToString();
                txtToolTip.Text = dr.tooltip_rubro;
                ddlTipoActividad.SelectedValue = dr.id_tipoactividad.ToString();
                ddlTipoDocReq.SelectedValue = dr.id_tipodocreq.ToString();
                if(dr.local_venta != null)
                    txtSalonVentas.Text = Convert.ToString(dr.local_venta);
                ddlCircuito.SelectedValue = dr.id_grupo_circuito.ToString();
                ddlClanae.SelectedValue = dr.id_clanae.ToString();
                if (dr.id_licencia_alcohol != null)
                    ddlRegistroAlc.SelectedValue = dr.id_licencia_alcohol.ToString();

                if (Convert.ToBoolean(dr.es_anterior_rubro))
                    optRubroHistorico.Checked = true;
                else
                    optRubroActual.Checked = true;

                txtFechaVigenciaHasta.Text = "";
                if (dr.VigenciaHasta_rubro != null)
                    txtFechaVigenciaHasta.Text = Convert.ToDateTime(dr.VigenciaHasta_rubro).ToString("dd/MM/yyyy");

                chkCircuitoAutomatico.Checked = dr.Circuito_Automatico;
                chkUsoCondicionado.Checked = dr.Uso_Condicionado;
                chkValidaCargaDescarga.Checked = dr.ValidaCargaDescarga;
                if (dr.SupMinCargaDescarga != null)
                    txtSupMinCargaDescarga.Text = Convert.ToString(dr.SupMinCargaDescarga);
                if (dr.SupMinCargaDescargaRefII != null)
                    txtSupMinCargaDescargaRefII.Text = Convert.ToString(dr.SupMinCargaDescargaRefII);
                if (dr.SupMinCargaDescargaRefV != null)
                    txtSupMinCargaDescargaRefV.Text = Convert.ToString(dr.SupMinCargaDescargaRefV);
                chkOficinaComercial.Checked = dr.OficinaComercial;
                chkTieneDeposito.Checked = dr.TieneDeposito;
                ChkLibrado.Checked = dr.Librar_Uso;

                grdImpactoAmbiental.DataSource = db.Rubros_TraerImpactoAmbientalPorId(id_rubro);
                grdImpactoAmbiental.DataBind();

                DataTable dt = TraerRubros_InformacionRelevante_porIdRubro(id_rubro);

                grdInfoRelevante.DataSource = dt;
                grdInfoRelevante.DataBind();

                DataTable dt2 = TraerRubros_TiposDeDocumentosRequeridos_porIdRubro(id_rubro);

                grdDocReq.DataSource = dt2;
                grdDocReq.DataBind();

                DataTable dt3 = TraerRubros_TiposDeDocumentosRequeridosZona_porIdRubro(id_rubro);

                grdDocReqZona.DataSource = dt3;
                grdDocReqZona.DataBind();

                DataTable dt4 = TraerRubros_Config_Incendio_porIdRubro(id_rubro);

                grdConfIncendio.DataSource = dt4;
                grdConfIncendio.DataBind();

                grdAuto.DataSource = TraerRubros_CircuAuto(id_rubro);
                grdAuto.DataBind();
            }

            CargarZonasRubro(cod_rubro);
            EstadoControles(pnlDatosRubro.Controls, false);
        }

        public void CargarSolicitudCambio(int id_solicitudcambio)
        {
            db = new DGHP_Entities();

            //this.GetRows_grdInfoRelevante_Eliminar = null;
            this.GetRows_grdInfoRelevante = null;
            this.GetRows_grdDocRequerido = null;
            this.GetRows_grdCircAutoZona = null;
            this.GetRows_grdConfIncendio = null;

            int id_estado_modif = 0;
            int tipo_solicitudcambio = 0;

            DesmarcarControlesModificados();
            optRubroHistorico.Checked = false;
            optRubroActual.Checked = false;
            pnlDatosSolicitudCambio.Visible = true;
            LimpiarTooltips();

            btnGuardar.ValidationGroup = "Guardar";
            //tomas aca
            var q = (from rubrosHistorialCambios in db.Rubros_Historial_Cambios
                     select new
                     {
                         id_rubhistcam = rubrosHistorialCambios.id_rubhistcam,
                         id_rubro = rubrosHistorialCambios.id_rubro,
                         nom_rubro = rubrosHistorialCambios.nom_rubro,
                         cod_rubro = rubrosHistorialCambios.cod_rubro,
                         tipo_solicitud_rubhistcam = rubrosHistorialCambios.tipo_solicitud_rubhistcam,
                         id_estado_modif = rubrosHistorialCambios.id_estado_modif,
                         bus_rubro = rubrosHistorialCambios.bus_rubro,
                         tooltip_rubro = rubrosHistorialCambios.tooltip_rubro,
                         id_tipoactividad = rubrosHistorialCambios.id_tipoactividad,
                         id_tipodocreq = rubrosHistorialCambios.id_tipodocreq,
                         rubrosHistorialCambios.id_licencia_alcohol,
                         EsAnterior_Rubro = rubrosHistorialCambios.EsAnterior_Rubro,
                         VigenciaHasta_rubro = rubrosHistorialCambios.VigenciaHasta_rubro,
                         observaciones_rubhistcam = rubrosHistorialCambios.observaciones_rubhistcam,
                         rubrosHistorialCambios.Circuito_Automatico,
                         rubrosHistorialCambios.Uso_Condicionado,
                         rubrosHistorialCambios.SupMinCargaDescarga,
                         rubrosHistorialCambios.SupMinCargaDescargaRefII,
                         rubrosHistorialCambios.SupMinCargaDescargaRefV,
                         rubrosHistorialCambios.TieneDeposito,
                         rubrosHistorialCambios.OficinaComercial,
                         rubrosHistorialCambios.ValidaCargaDescarga,
                         rubrosHistorialCambios.id_grupo_circuito,
                         rubrosHistorialCambios.Librar_Uso,
                         rubrosHistorialCambios.id_clanae,
                         rubrosHistorialCambios.local_venta
                     }
            );

            q = q.Where(x => x.id_rubhistcam.Equals(id_solicitudcambio));


            foreach (var dr in q.ToList())
            {

                int id_rubro = int.Parse(dr.id_rubro.ToString());
                tipo_solicitudcambio = int.Parse(dr.tipo_solicitud_rubhistcam.ToString());
                id_estado_modif = int.Parse(dr.id_estado_modif.ToString());
                hid_id_rubhistcam.Value = dr.id_rubhistcam.ToString();
                hid_id_estado_modif.Value = id_estado_modif.ToString();
                hid_tipo_solicitudcambio.Value = tipo_solicitudcambio.ToString();
                hid_id_rubro.Value = dr.id_rubro.ToString();
                txtCodRubro.Text = dr.cod_rubro;
                txtDescRubro.Text = dr.nom_rubro;
                txtBusqueda.Text = dr.bus_rubro;
                txtToolTip.Text = dr.tooltip_rubro;
                ddlTipoActividad.SelectedValue = dr.id_tipoactividad.ToString();
                ddlTipoDocReq.SelectedValue = dr.id_tipodocreq.ToString();
                if(dr.local_venta != null)
                    txtSalonVentas.Text = Convert.ToString(dr.local_venta);
                if (dr.id_grupo_circuito != null)
                    ddlCircuito.SelectedValue = dr.id_grupo_circuito.ToString();
                if (dr.id_clanae != null)
                    ddlClanae.SelectedValue = dr.id_clanae.ToString();
                ddlRegistroAlc.SelectedValue = (dr.id_licencia_alcohol == null ? 0 : dr.id_licencia_alcohol).ToString();
                lblNroSolicitudCambio.Text = dr.id_rubhistcam.ToString();
                if (dr.tipo_solicitud_rubhistcam.ToString().Equals("0"))
                    lblTipoSolicitud.Text = "Alta de Rubro";
                else
                    lblTipoSolicitud.Text = "Modificación de Rubro";

                if (Convert.ToBoolean(dr.EsAnterior_Rubro))
                    optRubroHistorico.Checked = true;
                else
                    optRubroActual.Checked = true;

                txtFechaVigenciaHasta.Text = "";
                if (dr.VigenciaHasta_rubro != null)
                    txtFechaVigenciaHasta.Text = Convert.ToDateTime(dr.VigenciaHasta_rubro).ToString("dd/MM/yyyy");

                chkCircuitoAutomatico.Checked = dr.Circuito_Automatico;
                chkUsoCondicionado.Checked = dr.Uso_Condicionado;
                chkValidaCargaDescarga.Checked = dr.ValidaCargaDescarga;
                if (dr.SupMinCargaDescarga != null)
                    txtSupMinCargaDescarga.Text = Convert.ToString(dr.SupMinCargaDescarga);
                if (dr.SupMinCargaDescargaRefII != null)
                    txtSupMinCargaDescargaRefII.Text = Convert.ToString(dr.SupMinCargaDescargaRefII);
                if (dr.SupMinCargaDescargaRefV != null)
                    txtSupMinCargaDescargaRefV.Text = Convert.ToString(dr.SupMinCargaDescargaRefV);
                chkOficinaComercial.Checked = dr.OficinaComercial;
                chkTieneDeposito.Checked = dr.TieneDeposito;
                txtObservacionesSolicitudCambio.Text = dr.observaciones_rubhistcam;
                ChkLibrado.Checked = dr.Librar_Uso;

                grdImpactoAmbiental.DataSource = db.Rubros_TraerImpactoAmbientalPorId(id_rubro);
                grdImpactoAmbiental.DataBind();


                // ---------------------------------------------------------
                // Traer los usuarios que modificaron la solicitud de cambio
                // ---------------------------------------------------------
                var q2 = (from hist in db.Rubros_Historial_Cambios_UsuariosIntervinientes
                          join users in db.aspnet_Users on hist.Userid equals users.UserId
                          join user in db.Usuario on users.UserId equals user.UserId into zr
                          from fd in zr.DefaultIfEmpty()

                          select new
                          {
                              id_rubhistcam = hist.id_rubhistcam,
                              UserName = users.UserName,
                              apenom = fd.Apellido == null ? "" : fd.Apellido + " " + fd.Nombre == null ? "" : fd.Nombre,
                              lastUpdateDate = hist.LastUpdateDate
                          }
                );

                q2 = q2.Where(x => x.id_rubhistcam.Equals(id_solicitudcambio));

                grdUsuariosIntervinientes.DataSource = q2.ToList(); ;
                grdUsuariosIntervinientes.DataBind();


            }

            CargarCircAutoCambio(id_solicitudcambio);
            CargarZonasRubroSolicitudCambio(id_solicitudcambio);
            CargarInfoRelevanteRubroSolicitudCambio(id_solicitudcambio);
            CargarDocReqRubroSolicitudCambio(id_solicitudcambio);
            CargarDocReqRubroZonaSolicitudCambio(id_solicitudcambio);
            CargarEstadosPosibles(id_estado_modif);
            CargarConfIncendioRubroSolicitudCambio(id_solicitudcambio);
            CargarPermisos();
            MostrarDatosModificados();
            ddlTipoDocReq_SelectedIndexChanged(null, null);
            if (!chkCircuitoAutomatico.Checked && lnkBtnAccionesAgregarAuto.Visible)
                lnkBtnAccionesAgregarAuto.Visible = false;
            db.Dispose();
        }



        #endregion

        #region "Métodos Privados del control"

        private void LimpiarTooltips()
        {
            // Limpiar los tooltips
            txtCodRubro.Attributes.Remove("title");
            txtDescRubro.Attributes.Remove("title");
            txtBusqueda.Attributes.Remove("title");
            txtToolTip.Attributes.Remove("title");
            ddlTipoActividad.Attributes.Remove("title");
            ddlCircuito.Attributes.Remove("title");
            ddlClanae.Attributes.Remove("title");
            ddlRegistroAlc.Attributes.Remove("title");
            ddlTipoDocReq.Attributes.Remove("title");
            pnlRubroHistorico.Attributes.Remove("title");
            pnlTipoActividad.Attributes.Remove("title");
            pnlTipoDocReq.Attributes.Remove("title");

        }
        private void CargarPermisos()
        {

            string[] RolesPermitidos = { "editar rubros", "aprobar ediciones de rubros" };
            btnGuardar.Visible = false;
            btnNuevabusqueda.Visible = true;
            btnNuevabusqueda.Text = "Nueva Búsqueda";
            pnlAccionesAgregarAuto.Visible = false;
            pnlAgregarZonaCondicion1.Visible = false;
            pnlAccionesAgregarInfoRelevante.Visible = false;
            pnlAccionesAgregarConfIncendio.Visible = false;
            pnlAccionesAgregarDocReq.Visible = false;
            pnlAccionesAgregarDocReqZona.Visible = false;
            pnlEstados.Visible = false;
            int id_estado_modif = 0;
            if (hid_id_estado_modif.Value != "")
                id_estado_modif = int.Parse(hid_id_estado_modif.Value);
            int tipo_solicitudcambio = 0;
            if (hid_tipo_solicitudcambio.Value != "")
                tipo_solicitudcambio = int.Parse(hid_tipo_solicitudcambio.Value);

            EstadoControles(pnlDatosRubro.Controls, false);

            // Si es la solicitud es una modificación se inhabilita la posibilidad de modificar el código de rubro.
            txtCodRubro.ReadOnly = tipo_solicitudcambio.Equals(1);


            MembershipUser usu = Membership.GetUser();
            hid_rol_edicion.Value = "false";

            if (usu != null)
            {
                //string[] sRoles = aspnet_Users. GetRolesForUser(usu.UserName);
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
                List<SGI_Perfiles> perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.ToList();


                foreach (SGI_Perfiles perfil in perfiles_usuario)
                {
                    List<SGI_Menues> tareas = perfil.SGI_Menues.ToList();

                    foreach (SGI_Menues m in tareas)
                    {
                        if (m.descripcion_menu == "Editar Rubros" || m.descripcion_menu == "Aprobar ediciones de Rubros")
                        {
                            hid_rol_edicion.Value = "true";
                        }

                        if (RolesPermitidos.Contains(m.descripcion_menu.ToLower()))
                        {
                            btnGuardar.Visible = true;
                            btnNuevabusqueda.Text = "Cancelar";
                            pnlAgregarZonaCondicion1.Visible = true;
                            pnlAccionesAgregarAuto.Visible = true;
                            pnlAccionesAgregarInfoRelevante.Visible = true;
                            pnlAccionesAgregarConfIncendio.Visible = true;
                            pnlAccionesAgregarDocReq.Visible = true;
                            pnlAccionesAgregarDocReqZona.Visible = true;
                            pnlEstados.Visible = true;
                        }
                    }



                }
            }

            // Si esta en proceso y tenemos perfil de edicion, habilita para editar.
            if (bool.Parse(hid_rol_edicion.Value) && id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.EnProceso)
                EstadoControles(pnlDatosRubro.Controls, true);

        }

        private bool puedeEditarRubro;

        private bool PuedeEditarRubro(string perfil)
        {
            db = new DGHP_Entities();
            /*PERFIL APROBADOR
                - Tiene que poder editar los rubros*/
            if (PuedeAprobarRubro(perfil))
                return true;

            var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

            if (menu_usuario.Contains("Editar Rubros"))
                return true;

            return false;
        }

        private bool puedeVisualizarRubro;

        private bool PuedeVisualizarRubro()
        {
            db = new DGHP_Entities();

            Guid userid = Functions.GetUserId();

            var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

            foreach (var perfil in perfiles_usuario)
            {
                var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

                if (menu_usuario.Contains("Visualizar Rubros"))
                    return true;

            }

            return false;
        }

        private bool PuedeAprobarRubro(string perfil)
        {
            db = new DGHP_Entities();

            var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

            if (menu_usuario.Contains("Aprobar ediciones de Rubros"))
                return true;

            return false;
        }

        private void CargarEstadosPosibles(int id_estado_modif)
        {

            string[] RolesPermitidos = { "sgi_administrador", "edicion_rubros", "aprobar_ediciones_de_rubros" };

            ddlEstado.Items.Clear();
            MembershipUser usu = Membership.GetUser();

            if (usu != null)
            {

                string[] sRoles = Roles.GetRolesForUser(usu.UserName);
                Guid userid = Functions.GetUserId();
                var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

                foreach (string perfil in perfiles_usuario)
                {
                    // se agrega el estado en que se encuentra la solicitud de cambio
                    AgregarEstado(id_estado_modif);

                    if (PuedeEditarRubro(perfil))
                    {

                        if (id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.EnProceso)
                        {
                            AgregarEstado((int)Constants.EstadosSolicitudCambioRubros.Confirmada);
                            AgregarEstado((int)Constants.EstadosSolicitudCambioRubros.Anulada);
                        }
                        else if (id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Confirmada)
                        {
                            AgregarEstado((int)Constants.EstadosSolicitudCambioRubros.Anulada);
                        }

                    }
                    if (PuedeAprobarRubro(perfil))
                    {
                        if (id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Confirmada)
                        {
                            AgregarEstado((int)Constants.EstadosSolicitudCambioRubros.EnProceso);
                            AgregarEstado((int)Constants.EstadosSolicitudCambioRubros.Aprobada);
                            AgregarEstado((int)Constants.EstadosSolicitudCambioRubros.Rechazada);
                            AgregarEstado((int)Constants.EstadosSolicitudCambioRubros.Anulada);
                        }
                        else if (id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.EnProceso)
                        {
                            AgregarEstado((int)Constants.EstadosSolicitudCambioRubros.Anulada);
                        }

                    }

                }
            }
        }
        private void AgregarEstado(int id_estado_modif)
        {
            string nom_estado = "";
            switch (id_estado_modif)
            {
                case (int)Constants.EstadosSolicitudCambioRubros.EnProceso:
                    nom_estado = "En Proceso";
                    break;
                case (int)Constants.EstadosSolicitudCambioRubros.Anulada:
                    nom_estado = "Anulada";
                    break;
                case (int)Constants.EstadosSolicitudCambioRubros.Confirmada:
                    nom_estado = "Confirmada";
                    break;
                case (int)Constants.EstadosSolicitudCambioRubros.Aprobada:
                    nom_estado = "Aprobada";
                    break;
                case (int)Constants.EstadosSolicitudCambioRubros.Rechazada:
                    nom_estado = "Rechazada";
                    break;
            }
            MembershipUser usu = Membership.GetUser();

            if (ddlEstado.Items.FindByValue(id_estado_modif.ToString()) == null)
                //si usu tiene permiso de "Aprobar ediciones de Rubros" y id_estado_modif es APROBADA
                ddlEstado.Items.Add(new ListItem(nom_estado, id_estado_modif.ToString()));
        }

        private void CargarTiposDeActividades()
        {
            ddlTipoActividad.DataSource = TraerTipoActividades();
            ddlTipoActividad.DataTextField = "nombre";
            ddlTipoActividad.DataValueField = "id";
            ddlTipoActividad.DataBind();
        }

        private void CargarCircuitos()
        {
            ddlCircuito.DataSource = TraerCircuitos();
            ddlCircuito.DataTextField = "nombre";
            ddlCircuito.DataValueField = "id";
            ddlCircuito.DataBind();
        }
        private void CargarClanae()
        {
            ddlClanae.DataSource = TraerClanae();
            ddlClanae.DataTextField = "codigo_clanae";
            ddlClanae.DataValueField = "id_clanae";
            ddlClanae.DataBind();
        }
        private void CargarLicAlcohol()
        {
            ddlRegistroAlc.DataSource = TraerLicenciasAlcohol();
            ddlRegistroAlc.DataTextField = "codigo";
            ddlRegistroAlc.DataValueField = "id";
            ddlRegistroAlc.DataBind();
        }

        public DataTable TraerTipoActividades()
        {
            db = new DGHP_Entities();

            var q = (from tipoactividad in db.TipoActividad
                     select new
                     {
                         id = tipoactividad.Id,
                         nombre = tipoactividad.Nombre,
                     }
            );

            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("nombre");

            int rowindex = 0;
            foreach (var item in q)
            {
                dt.Rows.Add(item.id, item.nombre);
                rowindex++;
            }

            return dt;
        }

        public DataTable TraerCircuitos()
        {
            db = new DGHP_Entities();

            var q = (from cir in db.ENG_Grupos_Circuitos
                     select new
                     {
                         id = cir.id_grupo_circuito,
                         nombre = cir.cod_grupo_circuito + " - " + cir.nom_grupo_circuito,
                     }
            );

            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("nombre");

            int rowindex = 0;
            foreach (var item in q)
            {
                dt.Rows.Add(item.id, item.nombre);
                rowindex++;
            }
            return dt;
        }

        public IEnumerable TraerClanae()
        {
            var q = (from c in db.Clanae
                     select new
                     {
                         c.codigo_clanae,
                         c.id_clanae
                     }
           );

            return q.OrderBy(x => x.codigo_clanae).ToList();
        }
        public DataTable TraerLicenciasAlcohol()
        {
            db = new DGHP_Entities();

            var q = (from licAlcohol in db.RAL_Licencias
                     select new
                     {
                         id = licAlcohol.id_licencia_alcohol,
                         codigo = licAlcohol.cod_licencia_alcohol,
                     }
            );

            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("codigo");

            int rowindex = 0;
            dt.Rows.Add(0, "");
            foreach (var item in q)
            {
                dt.Rows.Add(item.id, item.codigo);
                rowindex++;
            }

            return dt;
        }

        private void CargarTipoDocumentacionRequerida()
        {

            ddlTipoDocReq.DataSource = TraerTipodocumentacionRequerida();
            ddlTipoDocReq.DataTextField = "Descripcion";
            ddlTipoDocReq.DataValueField = "id";
            ddlTipoDocReq.DataBind();
        }

        public IEnumerable TraerTipodocumentacionRequerida()
        {
            var q = (from tipodoc in db.Tipo_Documentacion_Req
                     select new
                     {
                         Descripcion = tipodoc.Descripcion,
                         id = tipodoc.Id
                     }
           );

            return q.ToList();
        }

        private void CargarZonas()
        {
            ddlZona.Items.Clear();

            var q = (from zonas in db.Zonas_Habilitaciones
                     select new
                     {
                         CodZonaHab = zonas.CodZonaHab,
                         Zona = zonas.CodZonaHab + " - " + zonas.DescripcionZonaHab
                     }
            );

            ddlZona.DataSource = q.ToList();
            ddlZona.DataTextField = "Zona";
            ddlZona.DataValueField = "CodZonaHab";
            ddlZona.DataBind();

        }

        private void CargarCondiciones()
        {

            ddlCondicion.Items.Clear();

            var q = (from rubros in db.RubrosCondiciones
                     select new
                     {
                         cod_condicion = rubros.cod_condicion,
                         condicion = rubros.cod_condicion + " - " + rubros.nom_condicion == null ? "" : rubros.nom_condicion
                     }
           );

            ddlCondicion.DataSource = q.OrderBy(x => x.cod_condicion).ToList();
            ddlCondicion.DataTextField = "condicion";
            ddlCondicion.DataValueField = "cod_condicion";
            ddlCondicion.DataBind();
        }

        private void CargarZonasRubro(string cod_rubro)
        {
            var q = (from rub in db.RubrosZonasCondiciones
                     join zonahab in db.Zonas_Habilitaciones on rub.cod_ZonaHab equals zonahab.CodZonaHab into zr
                     from fd in zr.DefaultIfEmpty()
                     join cond in db.RubrosCondiciones on rub.cod_condicion equals cond.cod_condicion into ar
                     from ad in ar.DefaultIfEmpty()
                     select new
                     {
                         id_rubzonhabhistcam = "0",
                         cod_rubro = rub.cod_rubro,
                         cod_ZonaHab = rub.cod_ZonaHab,
                         cod_condicion = rub.cod_condicion,
                         Zona = rub.cod_ZonaHab + " - " + fd.DescripcionZonaHab,
                         Condicion = rub.cod_condicion + " - " + ad.nom_condicion,
                         SupMin_condicion = ad.SupMin_condicion,
                         SupMax_condicion = ad.SupMax_condicion
                     }
           );

            if (cod_rubro != null)
                q = q.Where(x => x.cod_rubro.Equals(cod_rubro));

            grdZonasCondiciones.DataSource = q.ToList();
            grdZonasCondiciones.DataBind();

        }

        private void CargarCircAutoCambio(int id_solicitudcambio)
        {
            var q = (from rub in db.Rubros_CircuitoAtomatico_Zonas_Historial_Cambios
                     join zonahab in db.Zonas_Habilitaciones on rub.codZonaHab equals zonahab.CodZonaHab into zr
                     from fd in zr.DefaultIfEmpty()
                     select new
                     {
                         rub.id_rubcircauto,
                         rub.id_rubcircauto_histcam,
                         rub.id_rubhistcam,
                         rub.codZonaHab,
                         zonaDesc = rub.codZonaHab + " - " + fd.DescripcionZonaHab
                     }
            );

            q = q.Where(x => x.id_rubhistcam.Equals(id_solicitudcambio));

            DataTable dt = new DataTable();
            dt.Columns.Add("id_rubcircauto_histcam", typeof(int));
            dt.Columns.Add("id_rubcircauto", typeof(int));
            dt.Columns.Add("codZonaHab", typeof(string));
            dt.Columns.Add("zonaDesc", typeof(string));

            int rowindex = 0;
            foreach (var item in q)
            {
                dt.Rows.Add(item.id_rubcircauto_histcam, item.id_rubcircauto, item.codZonaHab, item.zonaDesc);
                rowindex++;
            }

            grdAuto.DataSource = dt;
            grdAuto.DataBind();

            this.GetRows_grdCircAutoZona = dt;
        }
        private void CargarZonasRubroSolicitudCambio(int id_solicitudcambio)
        {

            var q = (from rub in db.RubrosZonasCondiciones_Historial_Cambios
                     join zonahab in db.Zonas_Habilitaciones on rub.cod_ZonaHab equals zonahab.CodZonaHab into zr
                     from fd in zr.DefaultIfEmpty()
                     join cond in db.RubrosCondiciones on rub.cod_condicion equals cond.cod_condicion into ar
                     from ad in ar.DefaultIfEmpty()
                     select new
                     {
                         id_rubhistcam = rub.id_rubhistcam,
                         id_rubzonhabhistcam = rub.id_rubzonhabhistcam,
                         cod_ZonaHab = rub.cod_ZonaHab,
                         cod_condicion = rub.cod_condicion,
                         Zona = rub.cod_ZonaHab + " - " + fd.DescripcionZonaHab,
                         Condicion = rub.cod_condicion + " - " + ad.nom_condicion,
                         SupMin_condicion = (int?)ad.SupMin_condicion,
                         SupMax_condicion = (int?)ad.SupMax_condicion
                     }
            );

            q = q.Where(x => x.id_rubhistcam.Equals(id_solicitudcambio));

            grdZonasCondiciones.DataSource = q.ToList();
            grdZonasCondiciones.DataBind();

        }
        private void CargarInfoRelevanteRubroSolicitudCambio(int id_solicitudcambio)
        {
            var q = (from rub in db.Rubros_InformacionRelevante_Historial_Cambios
                     select new
                     {
                         rub.id_rubInfRel_histcam,
                         rub.id_rubhistcam,
                         rub.id_rubinf,
                         rub.descripcion_rubinf
                     }
            );

            q = q.Where(x => x.id_rubhistcam.Equals(id_solicitudcambio));

            grdInfoRelevante.DataSource = q.ToList();
            grdInfoRelevante.DataBind();

            DataTable dt = new DataTable();
            dt.Columns.Add("id_rubInfRel_histcam");
            dt.Columns.Add("id_rubhistcam");
            dt.Columns.Add("id_rubinf");
            dt.Columns.Add("descripcion_rubinf");
            dt.Columns.Add("rowindex");

            int rowindex = 0;
            foreach (var item in q)
            {
                dt.Rows.Add(item.id_rubInfRel_histcam, item.id_rubhistcam, item.id_rubinf, item.descripcion_rubinf, rowindex);
                rowindex++;
            }

            GetRows_grdInfoRelevante = dt;
        }

        private void CargarDocReqRubroSolicitudCambio(int id_solicitudcambio)
        {
            var q = (from rub in db.Rubros_TiposDeDocumentosRequeridos_Historial_Cambios
                     join tipodoc in db.TiposDeDocumentosRequeridos on rub.id_tdocreq equals tipodoc.id_tdocreq into zr
                     from fd in zr.DefaultIfEmpty()
                     select new
                     {
                         row = rub,
                         nombre_tdocreq = fd.nombre_tdocreq,
                         observaciones_tdocreq = fd.observaciones_tdocreq,
                         baja_tdocreq = fd.baja_tdocreq,
                         obligatorio_rubtdocreq = rub.obligatorio_rubtdocreq,
                         es_obligatorio = (rub.obligatorio_rubtdocreq == true) ? "Si" : "No"
                     }
            );

            q = q.Where(x => x.row.id_rubhistcam.Equals(id_solicitudcambio));
            q = q.Where(x => x.baja_tdocreq.Equals(false));


            DataTable dt = new DataTable();
            dt.Columns.Add("id_rubDocReq_histcam", typeof(int));
            dt.Columns.Add("id_rubtdocreq", typeof(int));
            dt.Columns.Add("id_tdocreq", typeof(int));
            dt.Columns.Add("nombre_tdocreq", typeof(string));
            dt.Columns.Add("observaciones_tdocreq", typeof(string));
            dt.Columns.Add("obligatorio_rubtdocreq", typeof(string));
            dt.Columns.Add("es_obligatorio", typeof(string));



            int rowindex = 0;
            foreach (var item in q)
            {
                dt.Rows.Add(item.row.id_rubDocReq_histcam, item.row.id_rubtdocreq, item.row.id_tdocreq, item.nombre_tdocreq, item.observaciones_tdocreq, item.obligatorio_rubtdocreq, item.es_obligatorio);
                rowindex++;
            }

            grdDocReq.DataSource = dt;
            grdDocReq.DataBind();

            this.GetRows_grdDocRequerido = dt;
        }

         private void CargarConfIncendioRubroSolicitudCambio(int id_solicitudcambio)
        {
            var q = (from rub in db.Rubros_Config_Incendio_Historial_Cambios
                     join tdoc in db.Rubros_Config_Incendio_TiposDeDocumentosRequeridos_Historial_Cambios
                        on rub.id_rubro_incendio_histcam equals tdoc.id_rubro_incendio_histcam into left
                     from tdoc in left.DefaultIfEmpty()
                     join tipodoc in db.TiposDeDocumentosRequeridos on tdoc.id_tdocreq equals tipodoc.id_tdocreq into zr
                     from fd in zr.DefaultIfEmpty()
                     select new
                     {
                         row = rub,
                         nombre_tdocreq = fd != null ? fd.nombre_tdocreq : "",
                         id_tdocreq = fd != null ? fd.id_tdocreq : 0
                     }
            );

            q = q.Where(x => x.row.id_rubhistcam.Equals(id_solicitudcambio));

            DataTable dt = new DataTable();
            dt.Columns.Add("id_rubro_incendio_histcam", typeof(int));
            dt.Columns.Add("id_rubro", typeof(int));
            dt.Columns.Add("id_rubro_incendio", typeof(int));
            dt.Columns.Add("id_tdocreq", typeof(int));
            dt.Columns.Add("riesgo", typeof(int));
            dt.Columns.Add("DesdeM2", typeof(decimal));
            dt.Columns.Add("HastaM2", typeof(decimal));
            dt.Columns.Add("nombre_tdocreq", typeof(string));

            int rowindex = 0;
            foreach (var item in q)
            {
                dt.Rows.Add(rowindex, item.row.id_rubro_incendio_histcam, item.row.id_rubro_incendio, item.id_tdocreq, item.row.riesgo, 
                    item.row.DesdeM2, item.row.HastaM2, item.nombre_tdocreq);
                rowindex++;
            }

            grdConfIncendio.DataSource = dt;
            grdConfIncendio.DataBind();
            
            this.GetRows_grdConfIncendio = dt;
        }

        private void CargarDocReqRubroZonaSolicitudCambio(int id_solicitudcambio)
        {
            var q = (from rub in db.Rubros_TiposDeDocumentosRequeridos_Zonas_Historial_Cambios
                     join tipodoc in db.TiposDeDocumentosRequeridos on rub.id_tdocreq equals tipodoc.id_tdocreq into zr
                     from fd in zr.DefaultIfEmpty()
                     join zona in db.Zonas_Habilitaciones on rub.codZonaHab equals zona.CodZonaHab into zo
                     from fz in zo.DefaultIfEmpty()
                     select new
                     {
                         row = rub,
                         nombre_tdocreq = fd.nombre_tdocreq,
                         observaciones_tdocreq = fd.observaciones_tdocreq,
                         baja_tdocreq = fd.baja_tdocreq,
                         obligatorio_rubtdocreq = rub.obligatorio_rubtdocreq,
                         es_obligatorio = (rub.obligatorio_rubtdocreq == true) ? "Si" : "No",
                         rub.codZonaHab,
                         zonaDesc = rub.codZonaHab + " - " + fz.DescripcionZonaHab
                     }
            );

            q = q.Where(x => x.row.id_rubhistcam.Equals(id_solicitudcambio));
            q = q.Where(x => x.baja_tdocreq.Equals(false));


            DataTable dt = new DataTable();
            dt.Columns.Add("id_rubDocReqZona_histcam", typeof(int));
            dt.Columns.Add("id_rubtdocreqzona", typeof(int));
            dt.Columns.Add("id_tdocreq", typeof(int));
            dt.Columns.Add("nombre_tdocreq", typeof(string));
            dt.Columns.Add("observaciones_tdocreq", typeof(string));
            dt.Columns.Add("obligatorio_rubtdocreq", typeof(string));
            dt.Columns.Add("es_obligatorio", typeof(string));
            dt.Columns.Add("codZonaHab", typeof(string));
            dt.Columns.Add("zonaDesc", typeof(string));

            int rowindex = 0;
            foreach (var item in q)
            {
                dt.Rows.Add(item.row.id_rubDocReqZona_histcam, item.row.id_rubtdocreqzona, item.row.id_tdocreq, item.nombre_tdocreq,
                    item.observaciones_tdocreq, item.obligatorio_rubtdocreq, item.es_obligatorio, item.codZonaHab, item.zonaDesc);
                rowindex++;
            }

            grdDocReqZona.DataSource = dt;
            grdDocReqZona.DataBind();

            this.GetRows_grdDocRequeridoZona = dt;
        }
        private void DesmarcarControlesModificados()
        {
            txtCodRubro.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            txtCodRubro.BorderWidth = Unit.Pixel(1);
            txtDescRubro.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            txtDescRubro.BorderWidth = Unit.Pixel(1);
            txtBusqueda.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            txtBusqueda.BorderWidth = Unit.Pixel(1);
            txtToolTip.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            txtToolTip.BorderWidth = Unit.Pixel(1);
            ddlTipoActividad.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            ddlTipoActividad.BorderWidth = Unit.Pixel(1);
            txtSalonVentas.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            txtSalonVentas.BorderWidth = Unit.Pixel(1);
            ddlCircuito.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            ddlCircuito.BorderWidth = Unit.Pixel(1);
            ddlClanae.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            ddlClanae.BorderWidth = Unit.Pixel(1);
            ddlTipoDocReq.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            ddlTipoDocReq.BorderWidth = Unit.Pixel(1);
            pnlRubroHistorico.BorderStyle = BorderStyle.None;
            txtFechaVigenciaHasta.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            txtFechaVigenciaHasta.BorderWidth = Unit.Pixel(1);
            chkCircuitoAutomatico.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            chkCircuitoAutomatico.BorderWidth = Unit.Pixel(1);
            chkUsoCondicionado.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            chkUsoCondicionado.BorderWidth = Unit.Pixel(1);
            txtSupMinCargaDescarga.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            txtSupMinCargaDescarga.BorderWidth = Unit.Pixel(1);
            txtSupMinCargaDescargaRefII.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            txtSupMinCargaDescargaRefII.BorderWidth = Unit.Pixel(1);
            txtSupMinCargaDescargaRefV.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            txtSupMinCargaDescargaRefV.BorderWidth = Unit.Pixel(1);
            chkTieneDeposito.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            chkTieneDeposito.BorderWidth = Unit.Pixel(1);
            chkOficinaComercial.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            chkOficinaComercial.BorderWidth = Unit.Pixel(1);
            chkValidaCargaDescarga.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            chkValidaCargaDescarga.BorderWidth = Unit.Pixel(1);
            ChkLibrado.BorderColor = ColorTranslator.FromHtml(colorBordeOriginal);
            ChkLibrado.BorderWidth = Unit.Pixel(1);
        }

        private void MostrarDatosModificados()
        {
            DGHP_Entities db = new DGHP_Entities();

            int id_rubro = 0;
            if (hid_id_rubro.Value != "")
                id_rubro = int.Parse(hid_id_rubro.Value); ;
            int id_solicitudcambio = 0;
            if (hid_id_rubhistcam.Value != "")
                id_solicitudcambio = int.Parse(hid_id_rubhistcam.Value);
            int tipo_solicitudcambio = 0;
            if (hid_tipo_solicitudcambio.Value != "")
                tipo_solicitudcambio = int.Parse(hid_tipo_solicitudcambio.Value);

            string cod_rubro = "";

            // Solo si es modificación colorea los cambios, si es alta, los cambios son todos.
            if (tipo_solicitudcambio.Equals(1))
            {
                #region campos modificados del rubro
                var q = (from rub in db.Rubros
                         join tact in db.TipoActividad on rub.id_tipoactividad equals tact.Id into zr
                         from fd in zr.DefaultIfEmpty()
                         join tdocreq in db.Tipo_Documentacion_Req on rub.id_tipodocreq equals tdocreq.Id into ar
                         from fa in ar.DefaultIfEmpty()
                         select new
                         {
                             id_rubro = rub.id_rubro,
                             cod_rubro = rub.cod_rubro,
                             nom_rubro = rub.nom_rubro,
                             bus_rubro = rub.bus_rubro,
                             id_tipoactividad = rub.id_tipoactividad,
                             nom_tipoactividad = (fd == null ? "" : fd.Descripcion),
                             id_tipodocreq = rub.id_tipodocreq,
                             nom_tipodocreq = (fd == null ? "" : fd.Descripcion),
                             EsAnterior_Rubro = rub.EsAnterior_Rubro,
                             VigenciaDesde_rubro = rub.VigenciaDesde_rubro,
                             VigenciaHasta_rubro = rub.VigenciaHasta_rubro,
                             PregAntenaEmisora = rub.PregAntenaEmisora,
                             SoloAPRA = rub.SoloAPRA,
                             tooltip_rubro = rub.tooltip_rubro,
                             rub.Circuito_Automatico,
                             rub.Uso_Condicionado,
                             rub.SupMinCargaDescarga,
                             rub.SupMinCargaDescargaRefII,
                             rub.SupMinCargaDescargaRefV,
                             rub.TieneDeposito,
                             rub.OficinaComercial,
                             rub.ValidaCargaDescarga,
                             rub.id_grupo_circuito,
                             rub.Librar_Uso,
                             rub.id_clanae,
                             cod_clanae = rub.id_clanae != null ? rub.Clanae.codigo_clanae : "",
                             nom_circuito = (rub.id_grupo_circuito != null ? rub.ENG_Grupos_Circuitos.cod_grupo_circuito + " - " + rub.ENG_Grupos_Circuitos.nom_grupo_circuito : ""),
                             rub.id_licencia_alcohol,
                             nom_licencia_alcohol = (rub.id_licencia_alcohol != null ? rub.RAL_Licencias.cod_licencia_alcohol : ""),
                             rub.local_venta
                         }
                );

                q = q.Where(x => x.id_rubro.Equals(id_rubro));

                foreach (var dr in q.ToList())
                {
                    cod_rubro = dr.cod_rubro;
                    if (!txtCodRubro.Text.Trim().Equals(dr.cod_rubro))
                    {

                        txtCodRubro.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        txtCodRubro.BorderWidth = Unit.Pixel(2);
                        txtCodRubro.Attributes.Add("title", "Valor anterior: " + dr.cod_rubro);
                    }

                    if (!txtDescRubro.Text.Trim().Equals(dr.nom_rubro))
                    {
                        txtDescRubro.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        txtDescRubro.BorderWidth = Unit.Pixel(2);
                        txtDescRubro.Attributes.Add("title", "Valor anterior: " + (dr.nom_rubro == null || dr.nom_rubro.Length.Equals(0) ? "(vacío)" : dr.nom_rubro));
                    }

                    if (!txtBusqueda.Text.Trim().Equals(dr.bus_rubro))
                    {
                        txtBusqueda.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        txtBusqueda.BorderWidth = Unit.Pixel(2);
                        txtBusqueda.Attributes.Add("title", "Valor anterior: " + (dr.bus_rubro == null || dr.bus_rubro.Length.Equals(0) ? "(vacío)" : dr.bus_rubro));
                    }

                    if (!txtToolTip.Text.Trim().Equals(dr.tooltip_rubro))
                    {
                        txtToolTip.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        txtToolTip.BorderWidth = Unit.Pixel(2);

                        txtToolTip.Attributes.Add("title", "Valor anterior: " + (dr.tooltip_rubro == null || dr.tooltip_rubro.Length.Equals(0) ? "(vacío)" : dr.tooltip_rubro));
                    }

                    if (!ddlTipoActividad.SelectedValue.Trim().Equals(dr.id_tipoactividad.ToString()))
                    {
                        ddlTipoActividad.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        ddlTipoActividad.BorderWidth = Unit.Pixel(2);
                        pnlTipoActividad.Attributes.Add("title", "Valor anterior: " + (dr.nom_tipoactividad == null || dr.nom_tipoactividad.ToString().Length.Equals(0) ? "(vacío)" : dr.nom_tipoactividad.ToString()));
                    }
                    string SupSalonVentas = "";
                    if (dr.local_venta != null)
                        SupSalonVentas = Convert.ToString(dr.local_venta);
                    if (!txtSalonVentas.Text.Trim().Equals(SupSalonVentas))
                    {
                        txtSalonVentas.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        txtSalonVentas.BorderWidth = Unit.Pixel(2);
                    }
                    if (!ddlCircuito.SelectedValue.Trim().Equals(dr.id_grupo_circuito.ToString()))
                    {
                        ddlCircuito.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        ddlCircuito.BorderWidth = Unit.Pixel(2);
                        pnlCircuito.Attributes.Add("title", "Valor anterior: " + (dr.nom_circuito.Length.Equals(0) ? "(vacío)" : dr.nom_circuito));
                    }
                    if (!ddlClanae.SelectedValue.Trim().Equals(dr.id_clanae.ToString()))
                    {
                        ddlClanae.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        ddlClanae.BorderWidth = Unit.Pixel(2);
                        pnlClanae.Attributes.Add("title", "Valor anterior: " + (dr.cod_clanae.Length.Equals(0) ? "(vacío)" : dr.cod_clanae));
                    }
                    if (!ddlRegistroAlc.SelectedValue.Trim().Equals(dr.id_licencia_alcohol.ToString()))
                    {
                        ddlRegistroAlc.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        ddlRegistroAlc.BorderWidth = Unit.Pixel(2);
                        pnlRubroHistorico.Attributes.Add("title", "Valor anterior: " + (dr.nom_licencia_alcohol.Length.Equals(0) ? "(vacío)" : dr.nom_licencia_alcohol));
                    }
                    if (!ddlTipoDocReq.SelectedValue.Trim().Equals(dr.id_tipodocreq.ToString()))
                    {
                        ddlTipoDocReq.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        ddlTipoDocReq.BorderWidth = Unit.Pixel(2);
                        pnlTipoDocReq.Attributes.Add("title", "Valor anterior: " + (dr.nom_tipodocreq == null || dr.nom_tipodocreq.ToString().Length.Equals(0) ? "(vacío)" : dr.nom_tipodocreq.ToString()));
                    }

                    bool EsHistorico = false;
                    bool.TryParse(dr.EsAnterior_Rubro.ToString(), out EsHistorico);

                    if (!optRubroHistorico.Checked.Equals(EsHistorico))
                    {
                        pnlRubroHistorico.BorderStyle = BorderStyle.Solid;
                        pnlRubroHistorico.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        pnlRubroHistorico.BorderWidth = Unit.Pixel(2);
                        if (EsHistorico)
                            pnlRubroHistorico.Attributes.Add("title", "Valor anterior: Rubro histórico");
                        else
                            pnlRubroHistorico.Attributes.Add("title", "Valor anterior: Rubro actual");
                    }

                    string FechaVigenciaRubro = "";
                    if (dr.VigenciaHasta_rubro != null)
                    {
                        DateTime Fecha = Convert.ToDateTime(dr.VigenciaHasta_rubro);
                        FechaVigenciaRubro = Fecha.ToString("dd/MM/yyyy");
                    }

                    if (!txtFechaVigenciaHasta.Text.Trim().Equals(FechaVigenciaRubro))
                    {
                        txtFechaVigenciaHasta.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        txtFechaVigenciaHasta.BorderWidth = Unit.Pixel(2);
                    }
                    if (!chkCircuitoAutomatico.Checked.Equals(dr.Circuito_Automatico))
                    {
                        chkCircuitoAutomatico.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        chkCircuitoAutomatico.BorderWidth = Unit.Pixel(2);
                    }
                    if (!chkUsoCondicionado.Checked.Equals(dr.Uso_Condicionado))
                    {
                        chkUsoCondicionado.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        chkUsoCondicionado.BorderWidth = Unit.Pixel(2);
                    }
                    string SupMinCargaDescarga = "";
                    if (dr.SupMinCargaDescarga != null)
                        SupMinCargaDescarga = Convert.ToString(dr.SupMinCargaDescarga);
                    if (!txtSupMinCargaDescarga.Text.Trim().Equals(SupMinCargaDescarga))
                    {
                        txtSupMinCargaDescarga.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        txtSupMinCargaDescarga.BorderWidth = Unit.Pixel(2);
                    }
                    string SupMinCargaDescargaRefII = "";
                    if (dr.SupMinCargaDescargaRefII != null)
                        SupMinCargaDescargaRefII = Convert.ToString(dr.SupMinCargaDescargaRefII);
                    if (!txtSupMinCargaDescargaRefII.Text.Trim().Equals(SupMinCargaDescargaRefII))
                    {
                        txtSupMinCargaDescargaRefII.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        txtSupMinCargaDescargaRefII.BorderWidth = Unit.Pixel(2);
                    }
                    string SupMinCargaDescargaRefV = "";
                    if (dr.SupMinCargaDescargaRefV != null)
                        SupMinCargaDescargaRefV = Convert.ToString(dr.SupMinCargaDescargaRefV);
                    if (!txtSupMinCargaDescargaRefV.Text.Trim().Equals(SupMinCargaDescargaRefV))
                    {
                        txtSupMinCargaDescargaRefV.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        txtSupMinCargaDescargaRefV.BorderWidth = Unit.Pixel(2);
                    }
                    if (!chkTieneDeposito.Checked.Equals(dr.TieneDeposito))
                    {
                        chkTieneDeposito.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        chkTieneDeposito.BorderWidth = Unit.Pixel(2);
                    }
                    if (!chkOficinaComercial.Checked.Equals(dr.OficinaComercial))
                    {
                        chkOficinaComercial.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        chkOficinaComercial.BorderWidth = Unit.Pixel(2);
                    }
                    if (!chkValidaCargaDescarga.Checked.Equals(dr.ValidaCargaDescarga))
                    {
                        chkValidaCargaDescarga.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        chkValidaCargaDescarga.BorderWidth = Unit.Pixel(2);
                    }
                    if (!ChkLibrado.Checked.Equals(dr.Librar_Uso))
                    {
                        ChkLibrado.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        ChkLibrado.BorderWidth = Unit.Pixel(2);
                    }
                }

                #endregion

                #region Condiciones por Zonas

                // -----------------------------------------------------------------
                // Configuracion Original de Zonas y Condiciones
                // -----------------------------------------------------------------
                var q2 = (from rub in db.RubrosZonasCondiciones
                          join cond in db.RubrosCondiciones on rub.cod_condicion equals cond.cod_condicion into zr
                          from fd in zr.DefaultIfEmpty()
                          join zonhab in db.Zonas_Habilitaciones on rub.cod_ZonaHab equals zonhab.CodZonaHab into ar
                          from fa in ar.DefaultIfEmpty()
                          select new
                          {
                              cod_rubro = rub.cod_rubro,
                              cod_Zonahab = rub.cod_ZonaHab,
                              cod_condicion = rub.cod_condicion,
                              Zona = rub.cod_ZonaHab + " - " + fa.DescripcionZonaHab,
                              Condicion = rub.cod_condicion + " - " + fd.nom_condicion,
                              SupMin_condicion = fd.SupMin_condicion,
                              SupMax_condicion = fd.SupMax_condicion
                          }
                );

                if (cod_rubro != null)
                    q2 = q2.Where(x => x.cod_rubro.Equals(cod_rubro));



                var dsConfigOriginal = q2.ToList();

                // -----------------------------------------------------------------
                // Configuraciones de Condiciones por Zonas que han sido modificadas
                // -----------------------------------------------------------------


                foreach (GridViewRow rowGridZC in grdZonasCondiciones.Rows)
                {
                    bool existeFila = false;
                    foreach (var dr in dsConfigOriginal)
                    {
                        if (grdZonasCondiciones.DataKeys[rowGridZC.RowIndex].Values["cod_ZonaHab"].ToString() == dr.cod_Zonahab.ToString() &&
                            grdZonasCondiciones.DataKeys[rowGridZC.RowIndex].Values["cod_condicion"].ToString() == dr.cod_condicion.ToString())
                        {
                            existeFila = true;
                            break;
                        }
                    }


                    if (!existeFila)
                    {
                        rowGridZC.BorderStyle = BorderStyle.Solid;
                        rowGridZC.BorderWidth = Unit.Pixel(2);
                        rowGridZC.BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        rowGridZC.BackColor = ColorTranslator.FromHtml(colorEstadoEnPoceso);
                    }
                }




                // -----------------------------------------------------------------
                // Configuraciones de Condiciones por Zonas que han sido eliminadas
                // -----------------------------------------------------------------

                DataTable dtZonasCondicionesEliminadas = new DataTable();
                dtZonasCondicionesEliminadas.Columns.Add("Zona", typeof(string));
                dtZonasCondicionesEliminadas.Columns.Add("Condicion", typeof(string));
                dtZonasCondicionesEliminadas.Columns.Add("SupMin_condicion", typeof(int));
                dtZonasCondicionesEliminadas.Columns.Add("SupMax_condicion", typeof(int));

                foreach (var dr in dsConfigOriginal)
                {
                    bool existeFila = false;
                    foreach (GridViewRow rowGridZC in grdZonasCondiciones.Rows)
                    {
                        existeFila = false;
                        if (grdZonasCondiciones.DataKeys[rowGridZC.RowIndex].Values["cod_ZonaHab"].ToString() == dr.cod_Zonahab.ToString() &&
                            grdZonasCondiciones.DataKeys[rowGridZC.RowIndex].Values["cod_condicion"].ToString() == dr.cod_condicion.ToString())
                        {
                            existeFila = true;
                            break;
                        }

                    }

                    if (!existeFila)
                    {

                        string Zona = dr.Zona;
                        string Condicion = dr.Condicion;
                        int SupMin_condicion = 0;
                        int SupMax_condicion = 0;
                        int.TryParse(dr.SupMin_condicion.ToString(), out SupMin_condicion);
                        int.TryParse(dr.SupMax_condicion.ToString(), out SupMax_condicion);

                        DataRow datarw = dtZonasCondicionesEliminadas.NewRow();
                        datarw[0] = Zona;
                        datarw[1] = Condicion;
                        datarw[2] = SupMin_condicion;
                        datarw[3] = SupMax_condicion;
                        dtZonasCondicionesEliminadas.Rows.Add(datarw);
                    }

                }

                grdZonasCondicionesEliminadas.DataSource = dtZonasCondicionesEliminadas;
                grdZonasCondicionesEliminadas.DataBind();
                pnlZonasCondicionesEliminadas.Visible = (dtZonasCondicionesEliminadas.Rows.Count > 0);

                #endregion

                #region Informacion relevante

                // -----------------------------------------------------------------
                // Configuraciones de Informacion relevante que han sido modificadas
                // -----------------------------------------------------------------

                DataTable dtOriginal = TraerRubros_InformacionRelevante_porIdRubro(id_rubro); //datos actuales

                DataTable dtEliminada = CopiarEsquemaTabla(dtOriginal);
                //DataTable dtEnGrilla = GetRows_grdInfoRelevante;
                //DataTable dtEliminada = this.GetRows_grdInfoRelevante_Eliminar; 

                int id_rubInfRel_histcam = 0;
                int id_rubinf = 0;
                int iii = 0;
                for (iii = 0; iii < grdInfoRelevante.Rows.Count; iii++)
                {
                    id_rubInfRel_histcam = Convert.ToInt32(grdInfoRelevante.DataKeys[iii].Values[0]);
                    id_rubinf = Convert.ToInt32(grdInfoRelevante.DataKeys[iii].Values[1]);

                    //if (id_rubinf <= 0) // Filas nuevas con id menor o igual a cero
                    if (id_rubinf <= 0) // Filas nuevas con id menor o igual a cero
                    {
                        //pintar fila porque es un nuevo registro
                        grdInfoRelevante.Rows[iii].BorderStyle = BorderStyle.Solid;
                        grdInfoRelevante.Rows[iii].BorderWidth = Unit.Pixel(2);
                        grdInfoRelevante.Rows[iii].BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        grdInfoRelevante.Rows[iii].BackColor = ColorTranslator.FromHtml(colorEstadoEnPoceso);
                    }
                }

                // -----------------------------------------------------------------
                // Configuraciones de informacion relevante que han sido eliminadas
                // -----------------------------------------------------------------
                foreach (DataRow rowOrigen in dtOriginal.Rows)
                {
                    //int idTabla = Convert.ToInt32(rowOrigen["id_rubinf"]);
                    id_rubinf = Convert.ToInt32(rowOrigen["id_rubInfRel_histcam"]);
                    DataRow rowGrilla = BuscarRow_infoRelevante_porIdRubroInfo(id_rubinf);
                    //DataRow rowGrilla = dtEnGrilla.Rows.Find(idTabla);
                    if (rowGrilla == null)
                    {
                        DataRow nueva_dr = dtEliminada.NewRow();
                        for (iii = 0; iii < rowOrigen.ItemArray.Length; iii++)
                        {
                            nueva_dr[iii] = rowOrigen[iii];
                        }

                        dtEliminada.Rows.Add(nueva_dr);
                    }
                }

                grdInfoRelevanteEliminada.DataSource = dtEliminada;
                grdInfoRelevanteEliminada.DataBind();
                pnlInfoRelevanteEliminada.Visible = (grdInfoRelevanteEliminada.Rows.Count > 0);

                #endregion

                #region documentacion requerida

                // -----------------------------------------------------------------
                // Configuraciones de Documentacion requerida que han sido modificadas
                // -----------------------------------------------------------------

                dtOriginal = TraerRubros_TiposDeDocumentosRequeridos_porIdRubro(id_rubro); //datos actuales

                dtEliminada = CopiarEsquemaTabla(dtOriginal);
                //DataTable dtEnGrilla = GetRows_grdInfoRelevante;
                //DataTable dtEliminada = this.GetRows_grdInfoRelevante_Eliminar; 

                int id_rubDocReq_histcam = 0;
                int id_rubtdocreq = 0;
                iii = 0;
                for (iii = 0; iii < grdDocReq.Rows.Count; iii++)
                {
                    id_rubDocReq_histcam = Convert.ToInt32(grdDocReq.DataKeys[iii].Values[0]);
                    id_rubtdocreq = Convert.ToInt32(grdDocReq.DataKeys[iii].Values[1]);

                    if (id_rubtdocreq <= 0) // Filas nuevas con id menor o igual a cero
                    {
                        //pintar fila porque es un nuevo registro
                        grdDocReq.Rows[iii].BorderStyle = BorderStyle.Solid;
                        grdDocReq.Rows[iii].BorderWidth = Unit.Pixel(2);
                        grdDocReq.Rows[iii].BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        grdDocReq.Rows[iii].BackColor = ColorTranslator.FromHtml(colorEstadoEnPoceso);
                    }
                }


                // -----------------------------------------------------------------
                // Configuraciones de informacion relevante que han sido eliminadas
                // -----------------------------------------------------------------
                foreach (DataRow rowOrigen in dtOriginal.Rows)
                {
                    //int idTabla = Convert.ToInt32(rowOrigen["id_rubinf"]);
                    id_rubtdocreq = Convert.ToInt32(rowOrigen["id_rubtdocreq"]);
                    DataRow rowGrilla = BuscarRow_docReq_porIdRubroDocReq(id_rubtdocreq);
                    //DataRow rowGrilla = dtEnGrilla.Rows.Find(idTabla);
                    if (rowGrilla == null)
                    {
                        DataRow nueva_dr = dtEliminada.NewRow();
                        for (iii = 0; iii < rowOrigen.ItemArray.Length; iii++)
                        {
                            nueva_dr[iii] = rowOrigen[iii];
                        }

                        dtEliminada.Rows.Add(nueva_dr);
                    }
                }

                grdDocReqEliminada.DataSource = dtEliminada;
                grdDocReqEliminada.DataBind();
                pnlDocReqEliminada.Visible = (grdDocReqEliminada.Rows.Count > 0);

                #endregion

                #region documentacion requerida Zona

                // -----------------------------------------------------------------
                // Configuraciones de Documentacion requerida que han sido modificadas
                // -----------------------------------------------------------------

                dtOriginal = TraerRubros_TiposDeDocumentosRequeridosZona_porIdRubro(id_rubro); //datos actuales

                dtEliminada = CopiarEsquemaTabla(dtOriginal);

                int id_rubDocReqZona_histcam = 0;
                int id_rubtdocreqzona = 0;
                iii = 0;
                for (iii = 0; iii < grdDocReqZona.Rows.Count; iii++)
                {
                    id_rubDocReqZona_histcam = Convert.ToInt32(grdDocReqZona.DataKeys[iii].Values[0]);
                    id_rubtdocreqzona = Convert.ToInt32(grdDocReqZona.DataKeys[iii].Values[1]);

                    if (id_rubtdocreqzona <= 0) // Filas nuevas con id menor o igual a cero
                    {
                        //pintar fila porque es un nuevo registro
                        grdDocReqZona.Rows[iii].BorderStyle = BorderStyle.Solid;
                        grdDocReqZona.Rows[iii].BorderWidth = Unit.Pixel(2);
                        grdDocReqZona.Rows[iii].BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        grdDocReqZona.Rows[iii].BackColor = ColorTranslator.FromHtml(colorEstadoEnPoceso);
                    }
                }


                // -----------------------------------------------------------------
                // Configuraciones de informacion relevante que han sido eliminadas
                // -----------------------------------------------------------------
                foreach (DataRow rowOrigen in dtOriginal.Rows)
                {
                    //int idTabla = Convert.ToInt32(rowOrigen["id_rubinf"]);
                    id_rubtdocreqzona = Convert.ToInt32(rowOrigen["id_rubtdocreqzona"]);
                    DataRow rowGrilla = BuscarRow_docReqZona_porIdRubroDocReq(id_rubtdocreqzona);
                    //DataRow rowGrilla = dtEnGrilla.Rows.Find(idTabla);
                    if (rowGrilla == null)
                    {
                        DataRow nueva_dr = dtEliminada.NewRow();
                        for (iii = 0; iii < rowOrigen.ItemArray.Length; iii++)
                        {
                            nueva_dr[iii] = rowOrigen[iii];
                        }

                        dtEliminada.Rows.Add(nueva_dr);
                    }
                }

                grdDocReqZonaEliminada.DataSource = dtEliminada;
                grdDocReqZonaEliminada.DataBind();
                pnlDocReqZonaEliminada.Visible = (grdDocReqZonaEliminada.Rows.Count > 0);

                #endregion

                #region circuito automatico

                // -----------------------------------------------------------------
                // Configuraciones de Documentacion requerida que han sido modificadas
                // -----------------------------------------------------------------

                dtOriginal = TraerRubros_CircuAuto(id_rubro); //datos actuales

                dtEliminada = CopiarEsquemaTabla(dtOriginal);

                int id_rubcircauto_histcam = 0;
                int id_rubcircauto = 0;
                iii = 0;
                for (iii = 0; iii < grdAuto.Rows.Count; iii++)
                {
                    id_rubcircauto_histcam = Convert.ToInt32(grdAuto.DataKeys[iii].Values[0]);
                    id_rubcircauto = Convert.ToInt32(grdAuto.DataKeys[iii].Values[1]);

                    if (id_rubcircauto <= 0) // Filas nuevas con id menor o igual a cero
                    {
                        //pintar fila porque es un nuevo registro
                        grdAuto.Rows[iii].BorderStyle = BorderStyle.Solid;
                        grdAuto.Rows[iii].BorderWidth = Unit.Pixel(2);
                        grdAuto.Rows[iii].BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        grdAuto.Rows[iii].BackColor = ColorTranslator.FromHtml(colorEstadoEnPoceso);
                    }
                }


                // -----------------------------------------------------------------
                // Configuraciones de informacion relevante que han sido eliminadas
                // -----------------------------------------------------------------
                foreach (DataRow rowOrigen in dtOriginal.Rows)
                {
                    id_rubcircauto = Convert.ToInt32(rowOrigen["id_rubcircauto"]);
                    DataRow rowGrilla = BuscarRow_CircAuto_porIdRubCircAuto(id_rubcircauto);
                    if (rowGrilla == null)
                    {
                        DataRow nueva_dr = dtEliminada.NewRow();
                        for (iii = 0; iii < rowOrigen.ItemArray.Length; iii++)
                        {
                            nueva_dr[iii] = rowOrigen[iii];
                        }

                        dtEliminada.Rows.Add(nueva_dr);
                    }
                }

                grdAutoEliminada.DataSource = dtEliminada;
                grdAutoEliminada.DataBind();
                grdAutoEliminada.Visible = (grdAutoEliminada.Rows.Count > 0);

                #endregion

                #region configuracion incendio

                // -----------------------------------------------------------------
                // Configuraciones de configuracion incendio que han sido modificadas
                // -----------------------------------------------------------------

                dtOriginal = TraerRubros_Config_Incendio_porIdRubro(id_rubro); //datos actuales

                dtEliminada = CopiarEsquemaTabla(dtOriginal);

                int id_rubro_incendio_histcam = 0;
                int id_rubro_incendio = 0;
                iii = 0;
                for (iii = 0; iii < grdConfIncendio.Rows.Count; iii++)
                {
                    id_rubro_incendio_histcam = Convert.ToInt32(grdConfIncendio.DataKeys[iii].Values[0]);
                    id_rubro_incendio = Convert.ToInt32(grdConfIncendio.DataKeys[iii].Values[1]);

                    if (id_rubro_incendio <= 0) // Filas nuevas con id menor o igual a cero
                    {
                        //pintar fila porque es un nuevo registro
                        grdConfIncendio.Rows[iii].BorderStyle = BorderStyle.Solid;
                        grdConfIncendio.Rows[iii].BorderWidth = Unit.Pixel(2);
                        grdConfIncendio.Rows[iii].BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                        grdConfIncendio.Rows[iii].BackColor = ColorTranslator.FromHtml(colorEstadoEnPoceso);
                    }
                }

                // -----------------------------------------------------------------
                // configuracion incendio que han sido eliminadas
                // -----------------------------------------------------------------
                foreach (DataRow rowOrigen in dtOriginal.Rows)
                {
                    //int idTabla = Convert.ToInt32(rowOrigen["id_rubinf"]);
                    id_rubro_incendio = Convert.ToInt32(rowOrigen["id_rubro_incendio"]);
                    DataRow rowGrilla = BuscarRow_ConfIncendio_porIdRubroIncendio(id_rubro_incendio);
                    //DataRow rowGrilla = dtEnGrilla.Rows.Find(idTabla);
                    if (rowGrilla == null)
                    {
                        DataRow nueva_dr = dtEliminada.NewRow();
                        for (iii = 0; iii < rowOrigen.ItemArray.Length; iii++)
                        {
                            nueva_dr[iii] = rowOrigen[iii];
                        }

                        dtEliminada.Rows.Add(nueva_dr);
                    }
                }

                grdConfIncendioEliminada.DataSource = dtEliminada;
                grdConfIncendioEliminada.DataBind();
                grdConfIncendioEliminada.Visible = (grdConfIncendioEliminada.Rows.Count > 0);

                #endregion
                db.Dispose();
            }

        }

        #endregion

        #region confirmar cambio
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            int id_rubhistcam = 0;
            int id_rubro = 0;
            int tipo_solicitudcambio = 0;
            string cod_rubro = txtCodRubro.Text.Trim();
            string nom_rubro = txtDescRubro.Text.Trim();
            string bus_rubro = txtBusqueda.Text.Trim();
            int id_tipoactividad = 0;
            int id_circuito = 0;
            int id_clanae = 0;
            int id_RegistroAlc = 0;
            int id_tipodocreq = 0;
            bool EsAnterior_Rubro = optRubroHistorico.Checked;
            DateTime? VigenciaHasta_rubro = null;
            string tooltip_rubro = txtToolTip.Text.Trim();
            int id_estado_modif = 0;
            Guid userid = (Guid)Membership.GetUser().ProviderUserKey;

            int.TryParse(hid_id_rubhistcam.Value, out id_rubhistcam);
            int.TryParse(hid_id_rubro.Value, out id_rubro);
            int.TryParse(ddlTipoActividad.SelectedValue, out id_tipoactividad);
            decimal? supSalonVentas = null;
            if (txtSalonVentas.Text.Trim().Length > 0)
                supSalonVentas = Convert.ToDecimal(txtSalonVentas.Text);
            int.TryParse(ddlCircuito.SelectedValue, out id_circuito);
            int.TryParse(ddlClanae.SelectedValue, out id_clanae);
            int.TryParse(ddlTipoDocReq.SelectedValue, out id_tipodocreq);

            int.TryParse(ddlRegistroAlc.SelectedValue, out id_RegistroAlc);

            int.TryParse(ddlEstado.SelectedValue, out id_estado_modif);
            int.TryParse(hid_tipo_solicitudcambio.Value, out tipo_solicitudcambio);

            if (txtFechaVigenciaHasta.Text.Trim().Length > 0)
                VigenciaHasta_rubro = Convert.ToDateTime(txtFechaVigenciaHasta.Text);
            bool circuitoAutomatico = chkCircuitoAutomatico.Checked;
            bool usoCondicionado = chkUsoCondicionado.Checked;
            decimal? supMinCargaDescarga = null;
            if (txtSupMinCargaDescarga.Text.Trim().Length > 0)
                supMinCargaDescarga = Convert.ToDecimal(txtSupMinCargaDescarga.Text);
            decimal? supMinCargaDescargaRefII = null;
            if (txtSupMinCargaDescargaRefII.Text.Trim().Length > 0)
                supMinCargaDescargaRefII = Convert.ToDecimal(txtSupMinCargaDescargaRefII.Text);
            decimal? supMinCargaDescargaRefV = null;
            if (txtSupMinCargaDescargaRefV.Text.Trim().Length > 0)
                supMinCargaDescargaRefV = Convert.ToDecimal(txtSupMinCargaDescargaRefV.Text);
            bool oficinaComercial = chkOficinaComercial.Checked;
            bool tieneDeposito = chkTieneDeposito.Checked;
            bool validaCargaDescarga = chkValidaCargaDescarga.Checked;
            bool librarUso = ChkLibrado.Checked;

            try
            {
                try
                {
                    // Actualizar los detos de la Solicitud de cambio
                    // ----------------------------------------------------------------
                    if (VigenciaHasta_rubro.HasValue)
                        db.Rubros_SolicitudCambio_Actualizar(id_rubhistcam, id_rubro, cod_rubro, nom_rubro, bus_rubro, id_tipoactividad, id_tipodocreq, EsAnterior_Rubro, circuitoAutomatico, usoCondicionado, tieneDeposito, supMinCargaDescarga, supMinCargaDescargaRefII, supMinCargaDescargaRefV, oficinaComercial, validaCargaDescarga, VigenciaHasta_rubro, tooltip_rubro, id_estado_modif, txtObservacionesSolicitudCambio.Text.Trim(), id_circuito, id_RegistroAlc, userid, librarUso, id_clanae);
                    else
                        db.Rubros_SolicitudCambio_Actualizar(id_rubhistcam, id_rubro, cod_rubro, nom_rubro, bus_rubro, id_tipoactividad, id_tipodocreq, EsAnterior_Rubro, circuitoAutomatico, usoCondicionado, tieneDeposito, supMinCargaDescarga, supMinCargaDescargaRefII, supMinCargaDescargaRefV, oficinaComercial, validaCargaDescarga, null, tooltip_rubro, id_estado_modif, txtObservacionesSolicitudCambio.Text.Trim(), id_circuito, id_RegistroAlc, userid, librarUso, id_clanae);

                    // Eliminar el detalle de Zonas Condiciones de la Solicitud de cambio
                    // ----------------------------------------------------------------
                    db.Rubros_SolicitudCambio_EliminarZonasCondiciones(id_rubhistcam);

                    // Agregar el detalle de Zonas Condiciones a la Solicitud de cambio
                    // ----------------------------------------------------------------
                    foreach (GridViewRow row in grdZonasCondiciones.Rows)
                    {
                        string cod_ZonaHab = grdZonasCondiciones.DataKeys[row.RowIndex].Values["cod_ZonaHab"].ToString();
                        string cod_condicion = grdZonasCondiciones.DataKeys[row.RowIndex].Values["cod_condicion"].ToString();
                        db.Rubros_SolicitudCambio_AgergarZonasCondiciones(id_rubhistcam, cod_ZonaHab, cod_condicion);

                    }

                    //circuito automatico
                    db.Rubros_CircuitoAtomatico_Zonas_Historial_Cambios_delete(id_rubhistcam);
                    foreach (DataRow row in this.GetRows_grdCircAutoZona.Rows)
                    {
                        string codZonaHab = row["codZonaHab"].ToString();
                        int id_rubcircauto = Convert.ToInt32(row["id_rubcircauto"]);

                        int id_rubInfRel_histcam = db.Rubros_CircuitoAtomatico_Zonas_Historial_Cambios_insert(id_rubhistcam, id_rubcircauto, codZonaHab);
                    }

                    //informacion relevante
                    db.Rubros_InformacionRelevante_Historial_Cambios_delete(id_rubhistcam);

                    foreach (DataRow row in this.GetRows_grdInfoRelevante.Rows)
                    {
                        string descripcion_rubinf = row["descripcion_rubinf"].ToString();
                        int id_rubinf = Convert.ToInt32(row["id_rubinf"]);

                        int id_rubInfRel_histcam = db.Rubros_InformacionRelevante_Historial_Cambios_insert(id_rubhistcam, id_rubinf, descripcion_rubinf);
                    }

                    //documentacion requerida
                    db.Rubros_TiposDeDocumentosRequeridos_Historial_Cambios_delete(id_rubhistcam);

                    foreach (DataRow row in this.GetRows_grdDocRequerido.Rows)
                    {
                        int id_rubtdocreq = Convert.ToInt32(row["id_rubtdocreq"]);
                        int id_tdocreq = Convert.ToInt32(row["id_tdocreq"]);
                        bool obligatorio_rubtdocreq = Convert.ToBoolean(row["obligatorio_rubtdocreq"]);

                        int id_rubDocReq_histcam = db.Rubros_TiposDeDocumentosRequeridos_Historial_Cambios_insert(id_rubhistcam, id_rubtdocreq, id_tdocreq, obligatorio_rubtdocreq);
                    }

                    //documentacion requerida zona
                    db.Rubros_TiposDeDocumentosRequeridos_Zonas_Historial_Cambios_delete(id_rubhistcam);

                    foreach (DataRow row in this.GetRows_grdDocRequeridoZona.Rows)
                    {
                        int id_rubtdocreq = Convert.ToInt32(row["id_rubtdocreqzona"]);
                        int id_tdocreq = Convert.ToInt32(row["id_tdocreq"]);
                        bool obligatorio_rubtdocreq = Convert.ToBoolean(row["obligatorio_rubtdocreq"]);
                        string codZonaHab = Convert.ToString(row["codZonaHab"]);
                        int id_rubDocReq_histcam = db.Rubros_TiposDeDocumentosRequeridos_Zonas_Historial_Cambios_insert(id_rubhistcam, id_rubtdocreq, id_tdocreq, obligatorio_rubtdocreq, codZonaHab);
                    }

                    //configuracion de incendio
                    db.Rubros_Config_Incendio_TiposDeDocumentosRequeridos_Historial_Cambios_delete(id_rubhistcam);
                    db.Rubros_Config_Incendio_Historial_Cambios_delete(id_rubhistcam);
                    ObjectParameter id_rubro_incendio_histcam_param;
                    foreach (DataRow row in this.GetRows_grdConfIncendio.Rows)
                    {
                        int id_rubro_incendio = Convert.ToInt32(row["id_rubro_incendio"]);
                        int id_tdocreq = Convert.ToInt32(row["id_tdocreq"]);
                        int riesgo = Convert.ToInt32(row["riesgo"]);
                        decimal desdeM2 = Convert.ToDecimal(row["DesdeM2"]);
                        decimal hastaM2 = Convert.ToDecimal(row["HastaM2"]);

                        id_rubro_incendio_histcam_param = new ObjectParameter("id_rubro_incendio_histcam", typeof(int));
                        db.Rubros_Config_Incendio_Historial_Cambios_insert(id_rubhistcam, id_rubro_incendio, riesgo, desdeM2, hastaM2, id_rubro_incendio_histcam_param);
                        if (id_tdocreq > 0)
                        {
                            int id_rubro_incendio_histcam = Convert.ToInt32(id_rubro_incendio_histcam_param.Value); ;
                            db.Rubros_Config_Incendio_TiposDeDocumentosRequeridos_Historial_Cambios_insert(id_rubro_incendio_histcam, id_tdocreq, id_tdocreq);
                        }
                    }

                    // Actualiza el rubro de forma definitiva
                    // --------------------------------------
                    if (id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Aprobada)
                    {
                        db.Rubros_SolicitudCambio_ActualizarRubro(id_rubhistcam, userid);
                    }

                    btnGuardar.Visible = false;
                    btnNuevabusqueda.Visible = false;

                }
                catch(Exception ex)
                {
                    throw;
                }


                GridView grdRubros = (GridView)this.Parent.FindControl("grdRubros");

                // Si es una solicitud de modificacion colorear la fila de la grilla de rubros
                if (tipo_solicitudcambio.Equals(1) && grdRubros != null && grdRubros.Visible)
                {

                    HiddenField hid_grdRubros_rowIndexselected = (HiddenField)this.Parent.FindControl("hid_grdRubros_rowIndexselected");
                    int rowindex = 0;
                    int.TryParse(hid_grdRubros_rowIndexselected.Value, out rowindex);

                    // Vuelve el color de la fila al original sin resaltar
                    if (!(id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Confirmada || id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.EnProceso))
                    {
                        grdRubros.Rows[rowindex].BackColor = ColorTranslator.FromHtml("#C2BDAF");
                        grdRubros.Rows[rowindex].BorderWidth = Unit.Pixel(1);
                        grdRubros.Rows[rowindex].BorderColor = ColorTranslator.FromHtml("#b9b29e");

                        //actualizar datos grilla padre
                        grdRubros.Rows[rowindex].Cells[1].Text = Server.HtmlEncode(nom_rubro);
                        grdRubros.Rows[rowindex].Cells[2].Text = Server.HtmlEncode(ddlTipoActividad.SelectedItem.Text);
                        CheckBox chk = (CheckBox)grdRubros.Rows[0].Cells[3].Controls[0];
                        chk.Checked = optRubroHistorico.Checked;

                    }
                    else if (id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.EnProceso)
                    {
                        grdRubros.Rows[rowindex].BackColor = ColorTranslator.FromHtml(colorEstadoEnPoceso);
                        grdRubros.Rows[rowindex].BorderWidth = Unit.Pixel(1);
                        grdRubros.Rows[rowindex].BorderColor = ColorTranslator.FromHtml(colorBordeEnPoceso);
                    }
                    else if (id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Confirmada)
                    {
                        grdRubros.Rows[rowindex].BackColor = ColorTranslator.FromHtml(colorEstadoConfirmada);
                        grdRubros.Rows[rowindex].BorderWidth = Unit.Pixel(1);
                        grdRubros.Rows[rowindex].BorderColor = ColorTranslator.FromHtml(colorBordeConfirmada);
                    }


                }
                else
                {
                    // Actualiza la grilla de solicitudes de cambio activas
                    GridView grdSolicitudesActivas = (GridView)this.Parent.FindControl("grdSolicitudesActivas");
                    if (grdSolicitudesActivas == null)
                        grdSolicitudesActivas = new GridView();

                    Label lblCantResultados = (Label)this.Parent.FindControl("lblCantidadRegistros");
                    if (lblCantResultados == null)
                        lblCantResultados = new Label();

                    var q = (from rub in db.Rubros_Historial_Cambios
                             join tact in db.TipoActividad on rub.id_tipoactividad equals tact.Id
                             select new
                             {
                                 id_rubhistcam = rub.id_rubhistcam,
                                 tipo_solicitud_rubhistcam = rub.tipo_solicitud_rubhistcam,
                                 tipo_solicitud = (rub.tipo_solicitud_rubhistcam == 0) ? "Alta" : "Modificación",
                                 id_rubro = rub.id_rubro,
                                 cod_rubro = rub.cod_rubro,
                                 nom_rubro = rub.nom_rubro,
                                 TipoActividad = tact.Nombre,
                                 EsAnterior = rub.EsAnterior_Rubro,
                                 id_estado_modif = rub.id_estado_modif,
                                 estado_modif = rub.id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.EnProceso ? "En Proceso" :
                                 (rub.id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Aprobada ? "Aprobada" :
                                 (rub.id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Anulada ? "Anulada" :
                                 (rub.id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Rechazada ? "Rechazada" :
                                 (rub.id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Confirmada ? "Confirmada" : ""))))
                             }
                 );

                    q = q.Where(x => x.id_estado_modif.Equals(0) || x.id_estado_modif.Equals(2));

                    grdSolicitudesActivas.DataSource = q.OrderBy(x => x.EsAnterior).ThenBy(x => x.cod_rubro).ToList();
                    grdSolicitudesActivas.DataBind();

                    lblCantResultados.Text = q.OrderBy(x => x.EsAnterior).ThenBy(x => x.cod_rubro).ToList().Count.ToString();
                }

                Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
                string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
                if (id_rubro == 0)
                {
                    Rubros obj = db.Rubros.FirstOrDefault(x => x.id_rubro == id_rubro);
                    Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "I", 2010);
                }
                else
                {
                    Rubros obj = db.Rubros.FirstOrDefault(x => x.id_rubro == id_rubro);
                    Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "U", 2010);
                }

                this.EjecutarScript(updGuardar, "showBusqueda();");
            }
            catch (Exception ex)
            {

                lblError.Text = ex.Message;
                this.EjecutarScript(updGuardar, "showfrmErrorVisual();");

            }


        }


        #endregion

        #region zonas condiciones

        protected void btnAgregarZonaCondicion_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            int SupMin = 0;
            int SupMax = 0;
            string CodZonaNueva = "";
            string ZonaNueva = "";
            string CodCondicionNueva = "";
            string CondicionNueva = "";

            try
            {

                CodZonaNueva = ddlZona.SelectedValue;
                ZonaNueva = ddlZona.SelectedItem.Text;
                CodCondicionNueva = ddlCondicion.SelectedValue;
                CondicionNueva = ddlCondicion.SelectedItem.Text;

                foreach (GridViewRow row in grdZonasCondiciones.Rows)
                {
                    string cod_ZonaHab = grdZonasCondiciones.DataKeys[row.RowIndex].Values["cod_ZonaHab"].ToString();
                    string cod_condicion = grdZonasCondiciones.DataKeys[row.RowIndex].Values["cod_condicion"].ToString();

                    if (cod_ZonaHab.Equals(CodZonaNueva) && cod_condicion.Equals(CodCondicionNueva))
                    {
                        throw new Exception("La restricción que está queriendo agregar ya se encuentra en la lista de restricciones.");
                    }

                }

                var q = (from rub in db.RubrosCondiciones
                         select new
                         {
                             SupMin_condicion = rub.SupMin_condicion,
                             SupMax_condicion = rub.SupMax_condicion,
                             cod_condicion = rub.cod_condicion
                         }
                );

                if (CodCondicionNueva != null)
                    q = q.Where(x => x.cod_condicion.Equals(CodCondicionNueva));


                if (q.ToList().Count() > 0)
                {
                    int.TryParse(q.ToList().FirstOrDefault().SupMin_condicion.ToString(), out SupMin);
                    int.TryParse(q.ToList().FirstOrDefault().SupMax_condicion.ToString(), out SupMax);
                }


                DataTable dt = GetRows_grdZonasCondiciones();

                DataRow datarw = dt.NewRow();
                datarw[0] = 0;
                datarw[1] = CodZonaNueva;
                datarw[2] = CodCondicionNueva;
                datarw[3] = ZonaNueva;
                datarw[4] = CondicionNueva;
                datarw[5] = SupMin;
                datarw[6] = SupMax;
                dt.Rows.Add(datarw);

                grdZonasCondiciones.DataSource = dt;
                grdZonasCondiciones.DataBind();
                MostrarDatosModificados();
                db.Dispose();
                ScriptManager.RegisterStartupScript(updAgregarZonacondicion, updAgregarZonacondicion.GetType(), "grdMouseOverZonasCondiciones", "grdMouseOverZonasCondiciones();", true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                this.EjecutarScript(updAgregarZonacondicion, "showfrmErrorVisual();");
            }

        }

        protected void btnEliminarZonaCondicion_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            LinkButton btnEliminarZonaCondicion = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btnEliminarZonaCondicion.Parent.Parent;

            DataTable dt = GetRows_grdZonasCondiciones();
            dt.Rows[row.RowIndex].Delete();

            grdZonasCondiciones.DataSource = dt;
            grdZonasCondiciones.DataBind();
            MostrarDatosModificados();
            db.Dispose();

        }

        private DataTable GetRows_grdZonasCondiciones()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id_rubzonhabhistcam", typeof(int));
            dt.Columns.Add("cod_ZonaHab", typeof(string));
            dt.Columns.Add("cod_condicion", typeof(string));
            dt.Columns.Add("Zona", typeof(string));
            dt.Columns.Add("Condicion", typeof(string));
            dt.Columns.Add("SupMin_condicion", typeof(int));
            dt.Columns.Add("SupMax_condicion", typeof(int));

            foreach (GridViewRow row in grdZonasCondiciones.Rows)
            {
                int id_rubzonhabhistcam = Convert.ToInt32(grdZonasCondiciones.DataKeys[row.RowIndex].Values["id_rubzonhabhistcam"]);
                string cod_ZonaHab = grdZonasCondiciones.DataKeys[row.RowIndex].Values["cod_ZonaHab"].ToString();
                string cod_condicion = grdZonasCondiciones.DataKeys[row.RowIndex].Values["cod_condicion"].ToString();
                string Zona = HttpUtility.HtmlDecode(row.Cells[0].Text);
                string Condicion = HttpUtility.HtmlDecode(row.Cells[1].Text);
                int SupMin_condicion = 0;
                int SupMax_condicion = 0;
                int.TryParse(row.Cells[2].Text, out SupMin_condicion);
                int.TryParse(row.Cells[3].Text, out SupMax_condicion);

                DataRow datarw = dt.NewRow();
                datarw[0] = id_rubzonhabhistcam;
                datarw[1] = cod_ZonaHab;
                datarw[2] = cod_condicion;
                datarw[3] = Zona;
                datarw[4] = Condicion;
                datarw[5] = SupMin_condicion;
                datarw[6] = SupMax_condicion;
                dt.Rows.Add(datarw);

            }

            return dt;
        }

        #endregion

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            int id_estado_modif = int.Parse(ddl.SelectedValue);
            if (id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Anulada)
                btnGuardar.ValidationGroup = "";
            else
                btnGuardar.ValidationGroup = "Guardar";

        }

        private void EstadoControles(ControlCollection controls, bool estado)
        {
            foreach (Control control in controls)
            {

                if (control is TextBox)
                {
                    TextBox txt = (TextBox)control;
                    txt.ReadOnly = !estado;
                }
                else if (control is DropDownList)
                {
                    DropDownList ddl = (DropDownList)control;
                    ddl.Enabled = estado;
                }
                else if (control is RadioButton)
                {
                    RadioButton radio = (RadioButton)control;
                    radio.Enabled = estado;
                }
                else if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    checkBox.Enabled = estado;
                }
                if (control.Controls.Count > 0)
                    EstadoControles(control.Controls, estado);

            }

            lnkAgregarZonaCondicion.Visible = estado;
            lnkBtnAccionesAgregarInfoRelevante.Visible = estado;
            lnkBtnAccionesAgregarDocReq.Visible = estado;
            lnkBtnAccionesAgregarDocReqZona.Visible = estado;
            lnkBtnAccionesAgregarAuto.Visible = estado;
            txtFechaVigenciaHasta.Enabled = estado;

        }


        #region informarcion relevante

        public DataTable TraerRubros_InformacionRelevante_porIdRubro(int id_rubro)
        {
            DGHP_Entities db = new DGHP_Entities();

            DataTable dt = new DataTable();
            dt.Columns.Add("id_rubro");
            dt.Columns.Add("id_rubInfRel_histcam");
            dt.Columns.Add("descripcion_rubinf");
            dt.Columns.Add("id_rubinf");

            var q = (from info in db.Rubros_InformacionRelevante
                     select new
                     {
                         id_rubro = info.id_rubro,
                         id_rubInfRel_histcam = info.id_rubinf,
                         descripcion_rubinf = info.descripcion_rubinf,
                         id_rubinf = info.id_rubinf
                     }
                );

            q = q.Where(x => x.id_rubro.Equals(id_rubro));

            var reader = q.GetEnumerator();

            while (reader.MoveNext())
            {
                var rw = reader.Current;
                dt.Rows.Add(rw.id_rubro, rw.id_rubInfRel_histcam, rw.descripcion_rubinf, rw.id_rubinf);
            }

            if (dt.Rows.Count > 0)
            {
                DataColumn[] colClave = new DataColumn[1];
                colClave[0] = dt.Columns["id_rubInfRel_histcam"];
                dt.PrimaryKey = colClave;
            }

            db.Dispose();
            return dt;
        }

        protected void btnGuardarInfoRelevante_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            lblError.Text = "";
            try
            {
                string info = txtEditDescripInfoRelevante.Text;

                if (string.IsNullOrEmpty(info))
                {
                    throw new Exception("Debe cargar la información para el rubro.");
                }

                DataTable dt = GetRows_grdInfoRelevante;
                int id_rubInfRel_histcam = grdInfoRelevante.Rows.Count * -1;
                int id_rubhistcam = (string.IsNullOrEmpty(hid_id_rubhistcam.Value)) ? 0 : Convert.ToInt32(hid_id_rubhistcam.Value);
                int id_rubinf = 0; // grdInfoRelevante.Rows.Count * -1; // los nuevos le asigno la fila en negativo para que sean valores de clave unica en los find


                DataRow row = dt.NewRow();
                row["id_rubInfRel_histcam"] = id_rubInfRel_histcam;
                row["id_rubhistcam"] = id_rubhistcam;
                row["id_rubinf"] = id_rubinf;
                row["descripcion_rubinf"] = info.Trim();
                dt.Rows.Add(row);


                grdInfoRelevante.DataSource = dt;
                grdInfoRelevante.DataBind();

                GetRows_grdInfoRelevante = dt;

                MostrarDatosModificados();
                db.Dispose();
                ScriptManager.RegisterStartupScript(updPnlAgregarInfoRelevante, updPnlAgregarInfoRelevante.GetType(), "grdMouseInf", "grdMouseInf();", true);

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                this.EjecutarScript(updPnlAgregarInfoRelevante, "showfrmErrorVisual();");
            }

        }

        protected void btnEliminarInfoRelevante_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            LinkButton btnEliminar = (LinkButton)sender;
            GridViewRow gv_row = (GridViewRow)btnEliminar.Parent.Parent;
            int id_rubinf = Convert.ToInt32(grdInfoRelevante.DataKeys[gv_row.RowIndex].Values[1]);
            int id_rubInfRel_histcam = int.Parse(btnEliminar.CommandArgument);

            DataTable dt = this.GetRows_grdInfoRelevante;
            DataRow dr = dt.Rows.Find(id_rubInfRel_histcam);
            //DataRow dr = dt.Rows.Find(id_rubinf);

            if (dr != null)
            {
                //solo lo agrego a lista de borrado si es algo que existia en tabla original
                //dtEliminar = this.GetRows_grdInfoRelevante_Eliminar;
                //if (dtEliminar == null)
                //{
                //    dtEliminar = CopiarEsquemaTabla(dt);
                //}

                //if (id_rubinf > 0)
                //{
                //    DataRow nueva_dr = dtEliminar.NewRow();
                //    int iii = 0;
                //    for (iii = 0; iii < dr.ItemArray.Length; iii++)
                //    {
                //        nueva_dr[iii] = dr[iii];
                //    }

                //    dtEliminar.Rows.Add(nueva_dr);
                //}

                dt.Rows.Remove(dr);

                grdInfoRelevante.DataSource = dt;
                grdInfoRelevante.DataBind();

                //this.GetRows_grdInfoRelevante_Eliminar = dtEliminar;
                //this.GetRows_grdInfoRelevante = dt;
            }


            MostrarDatosModificados();
            db.Dispose();
        }

        protected void btnEliminarConfIncendio_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            LinkButton btnEliminar = (LinkButton)sender;
            GridViewRow gv_row = (GridViewRow)btnEliminar.Parent.Parent;
            int id_rubro_incendio_histcam = int.Parse(btnEliminar.CommandArgument);

            DataTable dt = this.GetRows_grdConfIncendio;
            DataRow dr = dt.Rows.Find(id_rubro_incendio_histcam);

            if (dr != null)
            {
                dt.Rows.Remove(dr);

                grdConfIncendio.DataSource = dt;
                grdConfIncendio.DataBind();
            }


            MostrarDatosModificados();
            db.Dispose();
        }
        private DataRow BuscarRow_infoRelevante_porIdRubroInfo(int p_id_rubinf)
        {
            DataTable dt = GetRows_grdInfoRelevante;
            DataRow row = null;

            if (dt == null || dt.Rows.Count == 0)
                return row;

            foreach (DataRow item in dt.Rows)
            {
                int id_rubinf = Convert.ToInt32(item["id_rubinf"]);

                if (id_rubinf == p_id_rubinf)
                {
                    row = item;
                    break;
                }
            }

            return row;
        }

        private DataTable _tabla_info_relevante;
        public DataTable GetRows_grdInfoRelevante
        {
            get
            {
                if (_tabla_info_relevante == null && ViewState["tabla_info_relevante"] != null)
                    _tabla_info_relevante = (DataTable)ViewState["tabla_info_relevante"];

                if (_tabla_info_relevante != null)
                {
                    DataColumn[] colClave = new DataColumn[1];
                    colClave[0] = _tabla_info_relevante.Columns["id_rubInfRel_histcam"];
                    //colClave[0] = _tabla_info_relevante.Columns["id_rubinf"];
                    _tabla_info_relevante.PrimaryKey = colClave;
                }

                return _tabla_info_relevante;
            }
            set
            {
                _tabla_info_relevante = value;

                if (_tabla_info_relevante != null)
                {
                    DataColumn[] colClave = new DataColumn[1];
                    colClave[0] = _tabla_info_relevante.Columns["id_rubInfRel_histcam"];
                    //colClave[0] = _tabla_info_relevante.Columns["id_rubinf"];
                    _tabla_info_relevante.PrimaryKey = colClave;
                }

                ViewState["tabla_info_relevante"] = _tabla_info_relevante;
            }
        }

        private DataTable _tabla_conf_incendio;
        public DataTable GetRows_grdConfIncendio
        {
            get
            {
                if (_tabla_conf_incendio == null && ViewState["tabla_conf_incendio"] != null)
                    _tabla_conf_incendio = (DataTable)ViewState["tabla_conf_incendio"];

                if (_tabla_conf_incendio != null)
                {
                    DataColumn[] colClave = new DataColumn[1];
                    colClave[0] = _tabla_conf_incendio.Columns["id_rubro_incendio_histcam"];
                    //colClave[0] = _tabla_info_relevante.Columns["id_rubinf"];
                    _tabla_conf_incendio.PrimaryKey = colClave;
                }

                return _tabla_conf_incendio;
            }
            set
            {
                _tabla_conf_incendio = value;

                if (_tabla_conf_incendio != null)
                {
                    DataColumn[] colClave = new DataColumn[1];
                    colClave[0] = _tabla_conf_incendio.Columns["id_rubro_incendio_histcam"];
                    //colClave[0] = _tabla_info_relevante.Columns["id_rubinf"];
                    _tabla_conf_incendio.PrimaryKey = colClave;
                }

                ViewState["tabla_conf_incendio"] = _tabla_conf_incendio;
            }
        }
        //private DataTable _tabla_info_relevante_eliminar;
        //public DataTable GetRows_grdInfoRelevante_Eliminar
        //{
        //    get
        //    {
        //        if (_tabla_info_relevante_eliminar == null && ViewState["tabla_info_relevante_eliminar"] != null)
        //            _tabla_info_relevante_eliminar = (DataTable)ViewState["tabla_info_relevante_eliminar"];

        //        if (_tabla_info_relevante_eliminar != null)
        //        {
        //            DataColumn[] colClave = new DataColumn[1];
        //            colClave[0] = _tabla_info_relevante_eliminar.Columns["id_rubinf"];
        //            _tabla_info_relevante_eliminar.PrimaryKey = colClave;
        //        }

        //        return _tabla_info_relevante_eliminar;
        //    }
        //    set
        //    {
        //        _tabla_info_relevante_eliminar = value;
        //        if (_tabla_info_relevante_eliminar != null)
        //        {
        //            DataColumn[] colClave = new DataColumn[1];
        //            colClave[0] = _tabla_info_relevante_eliminar.Columns["id_rubinf"];
        //            _tabla_info_relevante_eliminar.PrimaryKey = colClave;
        //        }

        //        ViewState["tabla_info_relevante_eliminar"] = _tabla_info_relevante_eliminar;
        //    }
        //}

        private DataTable CopiarEsquemaTabla(DataTable dt)
        {
            DataTable nuevaTabla = null;

            if (dt != null && dt.Rows.Count > 0)
            {
                StringWriter sw = new StringWriter();
                dt.TableName = "Tabla";
                dt.WriteXmlSchema(sw);
                nuevaTabla = new DataTable();
                StringReader sr = new StringReader(sw.ToString());
                nuevaTabla.ReadXmlSchema(sr);
            }

            return nuevaTabla;

        }

        #endregion

        #region Documentos requeridos
        public DataTable TraerTiposDeDocumentosRequeridosVigentes()
        {
            var q = (from tipodoc in db.TiposDeDocumentosRequeridos
                     select new
                     {
                         row = tipodoc
                     }
               );

            q = q.Where(x => x.row.baja_tdocreq.Equals(false));

            var reader = q.GetEnumerator();

            DataTable dt = new DataTable();
            dt.Columns.Add("nombre_tdocreq");
            dt.Columns.Add("id_tdocreq");
            while (reader.MoveNext())
            {
                var rw = reader.Current;
                dt.Rows.Add(rw.row.nombre_tdocreq, rw.row.id_tdocreq);
            }

            return dt;
        }

        public void CargarTipoDocRequeridos()
        {
            ddlEditTipoDocReq.Items.Clear();

            DataTable dt = TraerTiposDeDocumentosRequeridosVigentes();

            DataRow row = dt.NewRow();
            row["nombre_tdocreq"] = "Seleccione";
            row["id_tdocreq"] = "0";
            dt.Rows.InsertAt(row, 0);

            ddlEditTipoDocReq.DataSource = dt;
            ddlEditTipoDocReq.DataTextField = "nombre_tdocreq";
            ddlEditTipoDocReq.DataValueField = "id_tdocreq";
            ddlEditTipoDocReq.DataBind();

            ddlEditTipoDocReqZona.DataSource = dt;
            ddlEditTipoDocReqZona.DataTextField = "nombre_tdocreq";
            ddlEditTipoDocReqZona.DataValueField = "id_tdocreq";
            ddlEditTipoDocReqZona.DataBind();

            ddlEditTipoDocReqConfInc.DataSource = dt;
            ddlEditTipoDocReqConfInc.DataTextField = "nombre_tdocreq";
            ddlEditTipoDocReqConfInc.DataValueField = "id_tdocreq";
            ddlEditTipoDocReqConfInc.DataBind();

            var q = (from zonas in db.Zonas_Habilitaciones
                     select new
                     {
                         CodZonaHab = zonas.CodZonaHab,
                         Zona = zonas.CodZonaHab + " - " + zonas.DescripcionZonaHab
                     });
            ddlEditZona.DataSource = q.ToList();
            ddlEditZona.DataTextField = "Zona";
            ddlEditZona.DataValueField = "CodZonaHab";
            ddlEditZona.DataBind();

            ddlEditZonaAuto.DataSource = q.ToList();
            ddlEditZonaAuto.DataTextField = "Zona";
            ddlEditZonaAuto.DataValueField = "CodZonaHab";
            ddlEditZonaAuto.DataBind();
        }

        public DataTable TraerRubros_TiposDeDocumentosRequeridos_porIdRubro(int id_rubro)
        {
            db = new DGHP_Entities();
            var q = (from rubro_doc in db.Rubros_TiposDeDocumentosRequeridos
                     join tipo_doc in db.TiposDeDocumentosRequeridos on rubro_doc.id_tdocreq equals tipo_doc.id_tdocreq
                     select new
                     {
                         rubro_doc.id_rubro,
                         rubro_doc.id_rubtdocreq,
                         rubro_doc.id_tdocreq,
                         tipo_doc.nombre_tdocreq,
                         tipo_doc.observaciones_tdocreq,
                         tipo_doc.baja_tdocreq,
                         obligatorio_rubtdocreq = rubro_doc.obligatorio_rubtdocreq,
                         es_obligatorio = (rubro_doc.obligatorio_rubtdocreq == true) ? "Si" : "No"

                     }
              );

            q = q.Where(x => x.id_rubro.Equals(id_rubro));
            q = q.Where(x => x.baja_tdocreq.Equals(false));



            DataTable dt = new DataTable();
            dt.Columns.Add("id_rubDocReq_histcam", typeof(int));
            dt.Columns.Add("id_rubro", typeof(int));
            dt.Columns.Add("id_rubtdocreq", typeof(int));
            dt.Columns.Add("id_tdocreq", typeof(int));
            dt.Columns.Add("nombre_tdocreq", typeof(string));
            dt.Columns.Add("observaciones_tdocreq", typeof(string));
            dt.Columns.Add("obligatorio_rubtdocreq", typeof(bool));
            dt.Columns.Add("es_obligatorio", typeof(string));

            int rowindex = 0;
            foreach (var item in q)
            {
                dt.Rows.Add(rowindex, item.id_rubro, item.id_rubtdocreq, item.id_tdocreq, item.nombre_tdocreq, item.observaciones_tdocreq, item.obligatorio_rubtdocreq, item.es_obligatorio);
                rowindex++;
            }

            if (dt.Rows.Count > 0)
            {
                DataColumn[] colClave = new DataColumn[1];
                colClave[0] = dt.Columns["id_rubDocReq_histcam"];
                dt.PrimaryKey = colClave;
            }

            return dt;
        }

        public DataTable TraerRubros_CircuAuto(int id_rubro)
        {
            db = new DGHP_Entities();
            var q = (from rub in db.Rubros_CircuitoAtomatico_Zonas
                     select new
                     {
                         rub.id_rubro,
                         rub.id_rubcircauto,
                         rub.codZonaHab,
                         zonaDesc = rub.codZonaHab + " - " + rub.Zonas_Habilitaciones.DescripcionZonaHab
                     }
              );

            q = q.Where(x => x.id_rubro.Equals(id_rubro));

            DataTable dt = new DataTable();
            dt.Columns.Add("id_rubcircauto_histcam", typeof(int));
            dt.Columns.Add("id_rubro", typeof(int));
            dt.Columns.Add("id_rubcircauto", typeof(int));
            dt.Columns.Add("codZonaHab", typeof(string));
            dt.Columns.Add("zonaDesc", typeof(string));

            int rowindex = 0;
            foreach (var item in q)
            {
                dt.Rows.Add(rowindex, item.id_rubro, item.id_rubcircauto, item.codZonaHab, item.zonaDesc);
                rowindex++;
            }

            if (dt.Rows.Count > 0)
            {
                DataColumn[] colClave = new DataColumn[1];
                colClave[0] = dt.Columns["id_rubcircauto_histcam"];
                dt.PrimaryKey = colClave;
            }

            return dt;
        }

        public DataTable TraerRubros_TiposDeDocumentosRequeridosZona_porIdRubro(int id_rubro)
        {
            db = new DGHP_Entities();
            var q = (from rubro_doc in db.Rubros_TiposDeDocumentosRequeridos_Zonas
                     join tipo_doc in db.TiposDeDocumentosRequeridos on rubro_doc.id_tdocreq equals tipo_doc.id_tdocreq
                     select new
                     {
                         rubro_doc.id_rubro,
                         rubro_doc.id_rubtdocreqzona,
                         rubro_doc.id_tdocreq,
                         tipo_doc.nombre_tdocreq,
                         tipo_doc.observaciones_tdocreq,
                         tipo_doc.baja_tdocreq,
                         obligatorio_rubtdocreq = rubro_doc.obligatorio_rubtdocreq,
                         es_obligatorio = (rubro_doc.obligatorio_rubtdocreq == true) ? "Si" : "No",
                         rubro_doc.codZonaHab,
                         zonaDesc = rubro_doc.codZonaHab + " - " + rubro_doc.Zonas_Habilitaciones.DescripcionZonaHab
                     }
              );

            q = q.Where(x => x.id_rubro.Equals(id_rubro));
            q = q.Where(x => x.baja_tdocreq.Equals(false));



            DataTable dt = new DataTable();
            dt.Columns.Add("id_rubDocReqZona_histcam", typeof(int));
            dt.Columns.Add("id_rubro", typeof(int));
            dt.Columns.Add("id_rubtdocreqzona", typeof(int));
            dt.Columns.Add("id_tdocreq", typeof(int));
            dt.Columns.Add("nombre_tdocreq", typeof(string));
            dt.Columns.Add("observaciones_tdocreq", typeof(string));
            dt.Columns.Add("obligatorio_rubtdocreq", typeof(bool));
            dt.Columns.Add("es_obligatorio", typeof(string));
            dt.Columns.Add("codZonaHab", typeof(string));
            dt.Columns.Add("zonaDesc", typeof(string));

            int rowindex = 0;
            foreach (var item in q)
            {
                dt.Rows.Add(rowindex, item.id_rubro, item.id_rubtdocreqzona, item.id_tdocreq, item.nombre_tdocreq, item.observaciones_tdocreq,
                    item.obligatorio_rubtdocreq, item.es_obligatorio, item.codZonaHab, item.zonaDesc);
                rowindex++;
            }

            if (dt.Rows.Count > 0)
            {
                DataColumn[] colClave = new DataColumn[1];
                colClave[0] = dt.Columns["id_rubDocReqZona_histcam"];
                dt.PrimaryKey = colClave;
            }

            return dt;
        }

        public DataTable TraerRubros_Config_Incendio_porIdRubro(int id_rubro)
        {
            db = new DGHP_Entities();
            var q = (from rubro_doc in db.Rubros_Config_Incendio
                     join tdoc in db.Rubros_Config_Incendio_TiposDeDocumentosRequeridos
                        on rubro_doc.id_rubro_incendio equals tdoc.id_rubro_incendio into left
                     from tdoc in left.DefaultIfEmpty()
                     join tipodoc in db.TiposDeDocumentosRequeridos on tdoc.id_tdocreq equals tipodoc.id_tdocreq into zr
                     from fd in zr.DefaultIfEmpty()

                     select new
                     {
                         rubro_doc.id_rubro,
                         rubro_doc.id_rubro_incendio,
                         rubro_doc.riesgo,
                         rubro_doc.DesdeM2,
                         rubro_doc.HastaM2,
                         id_tdocreq = fd!=null?fd.id_tdocreq:0,
                         nombre_tdocreq = fd != null ? fd.nombre_tdocreq : ""
                     }
              );

            q = q.Where(x => x.id_rubro.Equals(id_rubro));

            DataTable dt = new DataTable();
            dt.Columns.Add("id_rubro_incendio_histcam", typeof(int));
            dt.Columns.Add("id_rubro_incendio", typeof(int));
            dt.Columns.Add("id_tdocreq", typeof(int));
            dt.Columns.Add("riesgo", typeof(int));
            dt.Columns.Add("DesdeM2", typeof(decimal));
            dt.Columns.Add("HastaM2", typeof(decimal));
            dt.Columns.Add("nombre_tdocreq", typeof(string));

            int rowindex = 0;
            foreach (var item in q)
            {
                dt.Rows.Add(rowindex, item.id_rubro_incendio, item.id_tdocreq, item.riesgo, item.DesdeM2, item.HastaM2,
                    item.nombre_tdocreq);
                rowindex++;
            }

            if (dt.Rows.Count > 0)
            {
                DataColumn[] colClave = new DataColumn[1];
                colClave[0] = dt.Columns["id_rubro_incendio_histcam"];
                dt.PrimaryKey = colClave;
            }

            return dt;
        }

        public DataTable TraerTiposDeDocumentosRequeridos(int id_tipo_doc_req)
        {
            db = new DGHP_Entities();

            var q = (from tipo_doc in db.TiposDeDocumentosRequeridos
                     select new
                     {
                         tipo_doc.id_tdocreq,
                         tipo_doc.baja_tdocreq,
                         tipo_doc.nombre_tdocreq,
                         tipo_doc.observaciones_tdocreq,
                         tipo_doc.RequiereDetalle,
                         tipo_doc.visible_en_SGI,
                         tipo_doc.visible_en_SSIT
                     }
              );

            q = q.Where(x => x.id_tdocreq.Equals(id_tipo_doc_req));

            DataTable dt = new DataTable();
            dt.Columns.Add("id_tdocreq", typeof(int));
            dt.Columns.Add("baja_tdocreq", typeof(bool));
            dt.Columns.Add("nombre_tdocreq", typeof(string));
            dt.Columns.Add("observaciones_tdocreq", typeof(string));
            dt.Columns.Add("RequiereDetalle", typeof(bool));
            dt.Columns.Add("visible_en_SGI", typeof(bool));
            dt.Columns.Add("visible_en_SSIT", typeof(bool));

            int rowindex = 0;

            foreach (var item in q)
            {
                dt.Rows.Add(item.id_tdocreq, item.baja_tdocreq, item.nombre_tdocreq, item.observaciones_tdocreq, item.RequiereDetalle, item.visible_en_SGI, item.visible_en_SSIT);
                rowindex++;
            }

            return dt;

        }

        protected void btnGuardarDocReq_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            try
            {

                ValidarGuardarDocReq();

                int id_tdocreq = Convert.ToInt32(ddlEditTipoDocReq.SelectedValue);
                bool obligatorio_rubtdocreq = Convert.ToBoolean(ddlEditObligatorio.SelectedValue);

                DataTable dt = GetRows_grdDocRequerido;
                int id_rubInfRel_histcam = grdDocReq.Rows.Count * -1;
                int id_rubtdocreq = (string.IsNullOrEmpty(hid_id_rubtdocreq.Value)) ? 0 : Convert.ToInt32(hid_id_rubtdocreq.Value);

                DataTable dtTipoDoc = TraerTiposDeDocumentosRequeridos(id_tdocreq);

                if (dtTipoDoc.Rows.Count == 0)
                    throw new Exception("El tipo de documento no existe.");

                DataRow row = dt.NewRow();
                row["id_rubDocReq_histcam"] = id_rubInfRel_histcam;
                row["id_rubtdocreq"] = id_rubtdocreq;
                row["id_tdocreq"] = id_tdocreq;
                row["nombre_tdocreq"] = dtTipoDoc.Rows[0]["nombre_tdocreq"];
                row["observaciones_tdocreq"] = dtTipoDoc.Rows[0]["observaciones_tdocreq"];
                row["obligatorio_rubtdocreq"] = obligatorio_rubtdocreq;
                row["es_obligatorio"] = (obligatorio_rubtdocreq) ? "Si" : "No";

                dt.Rows.Add(row);

                grdDocReq.DataSource = dt;
                grdDocReq.DataBind();

                GetRows_grdDocRequerido = dt;

                MostrarDatosModificados();
                ScriptManager.RegisterStartupScript(updPnlAgregarDocReq, updPnlAgregarDocReq.GetType(), "grdMouseDocReq", "grdMouseDocReq();", true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                this.EjecutarScript(updPnlAgregarDocReq, "showfrmErrorVisual();");
            }

        }

        private bool ValidarGuardarDocReq()
        {
            if (string.IsNullOrEmpty(ddlEditTipoDocReq.Text) || ddlEditTipoDocReq.SelectedValue == "0")
                throw new Exception("Debe seleccionar el tipo de documento.");


            if (string.IsNullOrEmpty(ddlEditObligatorio.Text) || ddlEditObligatorio.SelectedValue == "0")
                throw new Exception("Debe seleccionar indicar si la documentación es obligatoria o no.");

            int id_tdocreq = Convert.ToInt32(ddlEditTipoDocReq.SelectedValue);

            if (BuscarRow_docReq_porIdRubroTipoDoc(id_tdocreq) != null)
                throw new Exception("El tipo de documento ya fue ingresado.");


            return true;

        }
        protected void btnGuardarDocReqZona_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            try
            {
                ValidarGuardarDocReqZona();

                int id_tdocreq = Convert.ToInt32(ddlEditTipoDocReqZona.SelectedValue);
                bool obligatorio_rubtdocreq = Convert.ToBoolean(ddlEditObligatorioZona.SelectedValue);
                string codZona = ddlEditZona.SelectedValue;
                string desZona = ddlEditZona.SelectedItem.Text;

                DataTable dt = GetRows_grdDocRequeridoZona;
                int id_rubInfRel_histcam = grdDocReqZona.Rows.Count * -1;
                int id_rubtdocreqzona = (string.IsNullOrEmpty(hid_id_rubtdocreqZona.Value)) ? 0 : Convert.ToInt32(hid_id_rubtdocreq.Value);

                DataTable dtTipoDoc = TraerTiposDeDocumentosRequeridos(id_tdocreq);

                if (dtTipoDoc.Rows.Count == 0)
                    throw new Exception("El tipo de documento no existe.");

                DataRow row = dt.NewRow();
                row["id_rubDocReqZona_histcam"] = id_rubInfRel_histcam;
                row["id_rubtdocreqzona"] = id_rubtdocreqzona;
                row["id_tdocreq"] = id_tdocreq;
                row["nombre_tdocreq"] = dtTipoDoc.Rows[0]["nombre_tdocreq"];
                row["observaciones_tdocreq"] = dtTipoDoc.Rows[0]["observaciones_tdocreq"];
                row["obligatorio_rubtdocreq"] = obligatorio_rubtdocreq;
                row["es_obligatorio"] = (obligatorio_rubtdocreq) ? "Si" : "No";
                row["codZonaHab"] = codZona;
                row["zonaDesc"] = desZona;

                dt.Rows.Add(row);

                grdDocReqZona.DataSource = dt;
                grdDocReqZona.DataBind();

                GetRows_grdDocRequeridoZona = dt;

                MostrarDatosModificados();
                ScriptManager.RegisterStartupScript(updPnlAgregarDocReqZona, updPnlAgregarDocReqZona.GetType(), "grdMouseDocReqZona", "grdMouseDocReqZona();", true);

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                this.EjecutarScript(updPnlAgregarDocReqZona, "showfrmErrorVisual();");
            }

        }
        private bool ValidarGuardarDocReqZona()
        {
            if (string.IsNullOrEmpty(ddlEditTipoDocReqZona.Text) || ddlEditTipoDocReqZona.SelectedValue == "0")
                throw new Exception("Debe seleccionar el tipo de documento.");


            if (string.IsNullOrEmpty(ddlEditObligatorioZona.Text) || ddlEditObligatorioZona.SelectedValue == "0")
                throw new Exception("Debe seleccionar indicar si la documentación es obligatoria o no.");

            if (string.IsNullOrEmpty(ddlEditZona.Text) || ddlEditZona.SelectedValue == "0")
                throw new Exception("Debe seleccionar la zona.");

            int id_tdocreq = Convert.ToInt32(ddlEditTipoDocReqZona.SelectedValue);

            /*if (BuscarRow_docReqZona_porIdRubroTipoDoc(id_tdocreq) != null)
                throw new Exception("El tipo de documento ya fue ingresado.");*/

            return true;
        }

        protected void btnGuardarConfIncendio_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            try
            {
                ValidarGuardarConfIncendio();

                int id_tdocreq = string.IsNullOrEmpty(ddlEditTipoDocReqConfInc.SelectedValue) 
                    ? 0 : Convert.ToInt32(ddlEditTipoDocReqConfInc.SelectedValue);
                string nombre_tdocreq = id_tdocreq==0 ? "" : ddlEditTipoDocReqConfInc.SelectedItem.Text;
                int riesgo = Convert.ToInt32(ddlRiesgo.SelectedValue);
                decimal desde = Convert.ToDecimal(txtDesde.Text);
                decimal hasta = Convert.ToDecimal(txtHasta.Text);

                DataTable dt = GetRows_grdConfIncendio;
                int id_rubro_incendio_histcam = grdConfIncendio.Rows.Count * -1;
                int id_rubro_incendio = (string.IsNullOrEmpty(hid_id_rubro_incendio.Value)) ? 0 : Convert.ToInt32(hid_id_rubro_incendio.Value);

                DataTable dtTipoDoc = TraerTiposDeDocumentosRequeridos(id_tdocreq);

                if (dtTipoDoc.Rows.Count == 0)
                    throw new Exception("El tipo de documento no existe.");

                DataRow row = dt.NewRow();
                row["id_rubro_incendio_histcam"] = id_rubro_incendio_histcam;
                row["id_rubro_incendio"] = id_rubro_incendio;
                row["id_tdocreq"] = id_tdocreq;
                row["riesgo"] = riesgo;
                row["DesdeM2"] = desde;
                row["HastaM2"] = hasta;
                row["nombre_tdocreq"] = nombre_tdocreq;

                dt.Rows.Add(row);

                grdConfIncendio.DataSource = dt;
                grdConfIncendio.DataBind();

                GetRows_grdConfIncendio = dt;

                MostrarDatosModificados();
                ScriptManager.RegisterStartupScript(updPnlAgregarConfIncendio, updPnlAgregarConfIncendio.GetType(), "grdMouseConfIncendio", "grdMouseConfIncendio();", true);

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                this.EjecutarScript(updPnlAgregarConfIncendio, "showfrmErrorVisual();");
            }

        }
        private bool ValidarGuardarConfIncendio()
        {
            //if (string.IsNullOrEmpty(txtRiesgo.Text))
              //  throw new Exception("Debe seleccionar el riesgo.");
            if (string.IsNullOrEmpty(txtDesde.Text))
                throw new Exception("Debe ingresar la sup. desde.");
            if (string.IsNullOrEmpty(txtHasta.Text))
                throw new Exception("Debe ingresar la sup. hasta.");

            return true;
        }
        
        protected void btnGuardarAuto_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            try
            {
                ValidarAuto();

                string codZona = ddlEditZonaAuto.SelectedValue;
                string desZona = ddlEditZonaAuto.SelectedItem.Text;

                DataTable dt = GetRows_grdCircAutoZona;
                int id_rubInfRel_histcam = grdAuto.Rows.Count * -1;
                int id_rubcircauto = (string.IsNullOrEmpty(hid_id_rubcircauto.Value)) ? 0 : Convert.ToInt32(hid_id_rubcircauto.Value);

                DataRow row = dt.NewRow();
                row["id_rubcircauto_histcam"] = id_rubInfRel_histcam;
                row["id_rubcircauto"] = id_rubcircauto;
                row["codZonaHab"] = codZona;
                row["zonaDesc"] = desZona;

                dt.Rows.Add(row);

                grdAuto.DataSource = dt;
                grdAuto.DataBind();

                GetRows_grdCircAutoZona = dt;

                MostrarDatosModificados();
                ScriptManager.RegisterStartupScript(updPnlAgregarAuto, updPnlAgregarAuto.GetType(), "grdMouseOverAuto", "grdMouseOverAuto();", true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                this.EjecutarScript(updPnlAgregarAuto, "showfrmErrorVisual();");
            }

        }
        private bool ValidarAuto()
        {
            if (string.IsNullOrEmpty(ddlEditZonaAuto.Text) || ddlEditZonaAuto.SelectedValue == "0")
                throw new Exception("Debe seleccionar la zona.");
            return true;
        }

        protected void btnEliminarDocReq_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            LinkButton btnEliminar = (LinkButton)sender;
            GridViewRow gv_row = (GridViewRow)btnEliminar.Parent.Parent;
            int id_rubtdocreq = Convert.ToInt32(grdDocReq.DataKeys[gv_row.RowIndex].Values[1]);
            int id_rubDocReq_histcam = int.Parse(btnEliminar.CommandArgument);

            DataTable dt = this.GetRows_grdDocRequerido;
            DataRow dr = dt.Rows.Find(id_rubDocReq_histcam);

            if (dr != null)
            {
                dt.Rows.Remove(dr);

                grdDocReq.DataSource = dt;
                grdDocReq.DataBind();
            }


            MostrarDatosModificados();
            db.Dispose();
        }

        protected void btnEliminarDocReqZona_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            LinkButton btnEliminar = (LinkButton)sender;
            GridViewRow gv_row = (GridViewRow)btnEliminar.Parent.Parent;
            int id_rubtdocreq = Convert.ToInt32(grdDocReqZona.DataKeys[gv_row.RowIndex].Values[1]);
            int id_rubDocReq_histcam = int.Parse(btnEliminar.CommandArgument);

            DataTable dt = this.GetRows_grdDocRequeridoZona;
            DataRow dr = dt.Rows.Find(id_rubDocReq_histcam);

            if (dr != null)
            {
                dt.Rows.Remove(dr);

                grdDocReqZona.DataSource = dt;
                grdDocReqZona.DataBind();
            }


            MostrarDatosModificados();
            db.Dispose();
        }
        protected void btnEliminarAutoZona_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            LinkButton btnEliminar = (LinkButton)sender;
            GridViewRow gv_row = (GridViewRow)btnEliminar.Parent.Parent;
            int id_rubcircauto_histcam = int.Parse(btnEliminar.CommandArgument);

            DataTable dt = this.GetRows_grdCircAutoZona;
            DataRow dr = dt.Rows.Find(id_rubcircauto_histcam);

            if (dr != null)
            {
                dt.Rows.Remove(dr);

                grdAuto.DataSource = dt;
                grdAuto.DataBind();
            }


            MostrarDatosModificados();
            db.Dispose();
        }
        private DataRow BuscarRow_CircAuto_porIdRubCircAuto(int p_id_rubcircauto)
        {
            DataTable dt = this.GetRows_grdCircAutoZona;
            DataRow row = null;

            if (dt == null || dt.Rows.Count == 0)
                return row;

            foreach (DataRow item in dt.Rows)
            {
                int id_rubcircauto = Convert.ToInt32(item["id_rubcircauto"]);

                if (p_id_rubcircauto == id_rubcircauto)
                {
                    row = item;
                    break;
                }
            }
            return row;
        }
        private DataRow BuscarRow_docReq_porIdRubroDocReq(int p_id_rubtdocreq)
        {
            DataTable dt = this.GetRows_grdDocRequerido;
            DataRow row = null;

            if (dt == null || dt.Rows.Count == 0)
                return row;

            foreach (DataRow item in dt.Rows)
            {
                int id_rubtdocreq = Convert.ToInt32(item["id_rubtdocreq"]);

                if (p_id_rubtdocreq == id_rubtdocreq)
                {
                    row = item;
                    break;
                }
            }

            return row;
        }
        private DataRow BuscarRow_docReqZona_porIdRubroDocReq(int p_id_rubtdocreqzona)
        {
            DataTable dt = this.GetRows_grdDocRequeridoZona;
            DataRow row = null;

            if (dt == null || dt.Rows.Count == 0)
                return row;

            foreach (DataRow item in dt.Rows)
            {
                int id_rubtdocreqzona = Convert.ToInt32(item["id_rubtdocreqzona"]);

                if (p_id_rubtdocreqzona == id_rubtdocreqzona)
                {
                    row = item;
                    break;
                }
            }

            return row;
        }
        private DataRow BuscarRow_ConfIncendio_porIdRubroIncendio(int p_id_rubro_incendio)
        {
            DataTable dt = this.GetRows_grdConfIncendio;
            DataRow row = null;

            if (dt == null || dt.Rows.Count == 0)
                return row;

            foreach (DataRow item in dt.Rows)
            {
                int id_rubro_incendio = Convert.ToInt32(item["id_rubro_incendio"]);

                if (p_id_rubro_incendio == id_rubro_incendio)
                {
                    row = item;
                    break;
                }
            }

            return row;
        }
        private DataRow BuscarRow_docReq_porIdRubroTipoDoc(int p_id_tdocreq)
        {
            DataTable dt = this.GetRows_grdDocRequerido;
            DataRow row = null;

            if (dt == null || dt.Rows.Count == 0)
                return row;

            foreach (DataRow item in dt.Rows)
            {
                int id_tdocreq = Convert.ToInt32(item["id_tdocreq"]);

                if (p_id_tdocreq == id_tdocreq)
                {
                    row = item;
                    break;
                }
            }

            return row;
        }
        private DataRow BuscarRow_docReqZona_porIdRubroTipoDoc(int p_id_tdocreq)
        {
            DataTable dt = this.GetRows_grdDocRequeridoZona;
            DataRow row = null;

            if (dt == null || dt.Rows.Count == 0)
                return row;

            foreach (DataRow item in dt.Rows)
            {
                int id_tdocreq = Convert.ToInt32(item["id_tdocreq"]);

                if (p_id_tdocreq == id_tdocreq)
                {
                    row = item;
                    break;
                }
            }

            return row;
        }

        private DataTable _tabla_doc_req;
        public DataTable GetRows_grdDocRequerido
        {
            get
            {
                if (_tabla_doc_req == null && ViewState["tabla_doc_req"] != null)
                    _tabla_doc_req = (DataTable)ViewState["tabla_doc_req"];

                if (_tabla_doc_req != null)
                {
                    DataColumn[] colClave = new DataColumn[1];
                    colClave[0] = _tabla_doc_req.Columns["id_rubDocReq_histcam"];
                    _tabla_doc_req.PrimaryKey = colClave;
                }

                return _tabla_doc_req;
            }
            set
            {
                _tabla_doc_req = value;

                if (_tabla_doc_req != null)
                {
                    DataColumn[] colClave = new DataColumn[1];
                    colClave[0] = _tabla_doc_req.Columns["id_rubDocReq_histcam"];
                    _tabla_doc_req.PrimaryKey = colClave;
                }

                ViewState["tabla_doc_req"] = _tabla_doc_req;
            }
        }

        private DataTable _tabla_doc_req_zona;
        public DataTable GetRows_grdDocRequeridoZona
        {
            get
            {
                if (_tabla_doc_req_zona == null && ViewState["tabla_doc_req_zona"] != null)
                    _tabla_doc_req_zona = (DataTable)ViewState["tabla_doc_req_zona"];

                if (_tabla_doc_req_zona != null)
                {
                    DataColumn[] colClave = new DataColumn[1];
                    colClave[0] = _tabla_doc_req_zona.Columns["id_rubro_incendio_histcam"];
                    _tabla_doc_req_zona.PrimaryKey = colClave;
                }

                return _tabla_doc_req_zona;
            }
            set
            {
                _tabla_doc_req_zona = value;

                if (_tabla_doc_req_zona != null)
                {
                    DataColumn[] colClave = new DataColumn[1];
                    colClave[0] = _tabla_doc_req_zona.Columns["id_rubro_incendio_histcam"];
                    _tabla_doc_req_zona.PrimaryKey = colClave;
                }

                ViewState["tabla_doc_req_zona"] = _tabla_doc_req_zona;
            }
        }
        private DataTable _tabla_circ_auto_zona;
        public DataTable GetRows_grdCircAutoZona
        {
            get
            {
                if (_tabla_circ_auto_zona == null && ViewState["tabla_circ_auto_zona"] != null)
                    _tabla_circ_auto_zona = (DataTable)ViewState["tabla_circ_auto_zona"];

                if (_tabla_circ_auto_zona != null)
                {
                    DataColumn[] colClave = new DataColumn[1];
                    colClave[0] = _tabla_circ_auto_zona.Columns["id_rubcircauto_histcam"];
                    _tabla_circ_auto_zona.PrimaryKey = colClave;
                }

                return _tabla_circ_auto_zona;
            }
            set
            {
                _tabla_circ_auto_zona = value;

                if (_tabla_circ_auto_zona != null)
                {
                    DataColumn[] colClave = new DataColumn[1];
                    colClave[0] = _tabla_circ_auto_zona.Columns["id_rubcircauto_histcam"];
                    _tabla_circ_auto_zona.PrimaryKey = colClave;
                }

                ViewState["tabla_circ_auto_zona"] = _tabla_circ_auto_zona;
            }
        }
        #endregion

        public void EjecutarScript(UpdatePanel upd, string scriptName)
        {
            ScriptManager.RegisterStartupScript(upd, upd.GetType(),
                "script", scriptName, true);

        }

        protected void ddlTipoDocReq_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id_tipodocreq;
            int.TryParse(ddlTipoDocReq.SelectedValue, out id_tipodocreq);
            if (id_tipodocreq == (int)Constants.TipoDocumentacionReq.DeclaracionJurada)
                chkCircuitoAutomatico.Enabled = true;
            else
            {
                chkCircuitoAutomatico.Checked = false;
                chkCircuitoAutomatico.Enabled = false;
            }
        }

        protected void txtSalonVentas_TextChanged(object sender, EventArgs e)
        {
            decimal? supSal = Convert.ToDecimal(txtSalonVentas.Text);
            if (supSal > Convert.ToDecimal(Functions.GetParametroChar("Superficie.Max.Salon.Ventas")))
            {
                ddlCircuito.SelectedValue = Convert.ToString((int)Constants.ENG_Grupos_Circuitos.HP);
            }
            
        }

        protected void chkCircuitoAutomatico_CheckedChanged(object sender, EventArgs e)
        {
            lnkBtnAccionesAgregarAuto.Visible = chkCircuitoAutomatico.Checked;
            if (!chkCircuitoAutomatico.Checked)
            {
                DataTable dt = this.GetRows_grdCircAutoZona;
                if (dt.Rows.Count > 0)
                {
                    dt.Rows.Clear();
                    grdAuto.DataSource = dt;
                    grdAuto.DataBind();
                }
            }
        }
    }


}