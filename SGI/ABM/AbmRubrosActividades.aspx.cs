using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SGI.Model;
using System.Transactions;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Data;
using SGI.Controls;
using System.Data.Entity.Core.Objects;
using SGI.GestionTramite.Controls.ExportacionExcel;

namespace SGI.ABM
{
    public partial class AbmRubrosActividades : BasePage
    {
        DGHP_Entities db = null;
        private string colorEstadoEnPoceso = "#fcd8b8";
        private string colorBordeEnPoceso = "#ea8f3e";
        private string colorEstadoConfirmada = "#cff3d7";
        private string colorBordeConfirmada = "#68be7b";

        #region load de pagina

        public bool puede_editar = true;
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnExportar);

            if (sm.IsInAsyncPostBack)
            {
                // ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                //  ScriptManager.RegisterStartupScript(updVisualizarRubro, updVisualizarRubro.GetType(), "init_Js_updDatos", "init_Js_updDatos();", true);

            }

            updResultados.Update();
            db = new DGHP_Entities();
            if (!IsPostBack)
            {
               
                VisualizarRubro.CargarDatosUnicaVez();
                
                /*puedeEditarRubro = PuedeEditarRubro();
                puedeVisualizarRubro = PuedeVisualizarRubro();
                puedeAprobarRubro = PuedeAprobarRubro();

                if (!this.puedeEditarRubro && !this.puedeAprobarRubro)
                {
                    btnVerSolicitudesActivas.Visible = false;
                    btnNuevaPersonaInhibida.Visible = false;
                    this.grdResultados.Columns[4].Visible = false;

                }
                if(this.puedeAprobarRubro)
                    btnVerSolicitudesActivas.Visible = true;*/
            }
            puedeEditarRubro = PuedeEditarRubro();
            puedeVisualizarRubro = PuedeVisualizarRubro();
            puedeAprobarRubro = PuedeAprobarRubro();

            if (!this.puedeEditarRubro && !this.puedeAprobarRubro)
            {
                btnVerSolicitudesActivas.Visible = false;
                btnNuevaPersonaInhibida.Visible = false;
                //this.grdResultados.Columns[4].Visible = false;

            }
            if (this.puedeAprobarRubro)
                btnVerSolicitudesActivas.Visible = true;

           

        }

        #endregion

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        protected void btnVerSolicitudesActivas_Click(object sender, EventArgs e)
        {

            lblCantidadRegistros.Text = "";
            try
            {

                grdResultados.Visible = false;
                grdSolicitudesActivas.Visible = true;

                pnlReferencias.Visible = false;

                DataTable dt = TraerSolicitudesDeCambioActivas();
                grdSolicitudesActivas.DataSource = dt;
                grdSolicitudesActivas.DataBind();
                lblCantidadRegistros.Text = dt.Rows.Count.ToString();
                updResultados.Update();

                this.EjecutarScript(updpnlBuscar, "showResultado();");
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                this.EjecutarScript(updResultados, "showfrmError();");
            }

        }

        public DataTable TraerSolicitudesDeCambioActivas()
        {

            db = new DGHP_Entities();

            var q = (from rubros in db.Rubros_Historial_Cambios
                     join tact in db.TipoActividad
                     on rubros.id_tipoactividad equals tact.Id
                     select new
                     {
                         id_rubhistcam = rubros.id_rubhistcam,
                         tipo_solicitud_rubhistcam = rubros.tipo_solicitud_rubhistcam,
                         tipo_solicitud = (rubros.tipo_solicitud_rubhistcam == 0) ? "Alta" : "Modificación",
                         id_rubro = rubros.id_rubro,
                         cod_rubro = rubros.cod_rubro,
                         nom_rubro = rubros.nom_rubro,
                         tipo_actividad = tact.Nombre,
                         EsAnterior = rubros.EsAnterior_Rubro,
                         estado_modif = rubros.id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.EnProceso ? "En Proceso" :
                                  (rubros.id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Aprobada ? "Aprobada" :
                                  (rubros.id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Anulada ? "Anulada" :
                                  (rubros.id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Rechazada ? "Rechazada" :
                                  (rubros.id_estado_modif == (int)Constants.EstadosSolicitudCambioRubros.Confirmada ? "Confirmada" : ""))))
                     }
                     ).Where(x=> x.estado_modif == "En Proceso" || x.estado_modif == "Confirmada").OrderByDescending(x => x.id_rubhistcam).ThenBy(x => x.cod_rubro).ToList();


            DataTable dt = new DataTable();
            dt.Columns.Add("id_rubhistcam", typeof(int));
            dt.Columns.Add("tipo_solicitud_rubhistcam", typeof(int));
            dt.Columns.Add("tipo_solicitud", typeof(string));
            dt.Columns.Add("id_rubro", typeof(int));
            dt.Columns.Add("cod_rubro", typeof(string));
            dt.Columns.Add("nom_rubro", typeof(string));
            dt.Columns.Add("tipo_actividad", typeof(string));
            dt.Columns.Add("EsAnterior", typeof(bool));
            dt.Columns.Add("estado_modif", typeof(string));

            int rowindex = 0;
            foreach (var item in q)
            {
                dt.Rows.Add(item.id_rubhistcam, item.tipo_solicitud_rubhistcam, item.tipo_solicitud, item.id_rubro, item.cod_rubro, item.nom_rubro, item.tipo_actividad, item.EsAnterior, item.estado_modif);
                rowindex++;
            }
            db.Dispose();

            return dt;
        }


        protected void btnNueva_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            try
            {

                // Genera o recupera la solicitud de cambio
                int id_solicitudCambio = 0;


                try
                {
                    ObjectParameter id_solicitudCambio_param = new ObjectParameter("id_rubhistcam_out", typeof(int));
                    db.Rubros_GenerarSolicitudCambio(0, 0, (Guid)Membership.GetUser().ProviderUserKey, id_solicitudCambio_param);
                    id_solicitudCambio = Convert.ToInt32(id_solicitudCambio_param.Value);
                }
                catch
                {
                    throw new Exception("Se ha producido un error generando la solicitud de cambio.");
                }

                hid_grdRubros_rowIndexselected.Value = "0";
                LimpiarDatos();
                VisualizarRubro.CargarSolicitudCambio(id_solicitudCambio);
                updVisualizarRubro.Update();
                this.EjecutarScript(updpnlBuscar, "showDatos();");

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                this.EjecutarScript(updpnlBuscar, "showfrmError();");
            }
        }

        private void LimpiarDatosBusqueda()
        {
            txtCodigoDescripcionoPalabraClave.Text = "";
        }

        private void LimpiarDatos()
        {
            hid_id_rubroReq.Value = "0";
            /*txtTipoDocumentoReq.Text = "";
            txtNroDocumentoReq.Text = "";
            txtNombreyApellidoReq.Text = "";
            txtAutosReq.Text = "";
            txtCuitReq.Text = "";
            txtEstadoReq.SelectedValue = "";
            txtFechaBajaReq.Text = "";
            txtFechaRegistroReq.Text = "";
            txtFechaVencimientoReq.Text = "";
            txtJuzgadoReq.Text = "";
            txtNroOperadorReq.Text = "";
            txtNroOrdenReq.Text = "";
            txtObservacionesReq.Text = "";
            txtSecretariaReq.Text = "";*/
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                grdResultados.Visible = true;
                grdSolicitudesActivas.Visible = false;
                if (this.puedeEditarRubro || this.puedeAprobarRubro)
                    pnlReferencias.Visible = true           ;
                else
                    pnlReferencias.Visible = false;
                Buscar();


                updResultados.Update();

                this.EjecutarScript(updpnlBuscar, "showResultado();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "showfrmError();");
            }
        }



        private void Buscar()
        {
            db = new DGHP_Entities();

            var q = (from rubros in db.Rubros
                     join tipoactividad in db.TipoActividad
                     on rubros.id_tipoactividad equals tipoactividad.Id into zr
                     from fd in zr.DefaultIfEmpty()
                     select new
                     {
                         id_rubro = rubros.id_rubro,
                         nomb_rubro = rubros.nom_rubro,
                         cod_rubro = rubros.cod_rubro,
                         bus_rubro = rubros.bus_rubro,
                         es_anterior_rubro = rubros.EsAnterior_Rubro,
                         tipo_actividad = fd.Nombre,
                         id_estado_modif = (from rhcambios in db.Rubros_Historial_Cambios
                                            select new
                                            {
                                                rhcambios.id_estado_modif,
                                                rhcambios.id_rubro
                                            }
                             ).Where(x => x.id_rubro.Equals(rubros.id_rubro)).FirstOrDefault()
                     });

            if (txtCodigoDescripcionoPalabraClave.Text.Trim().Length > 0)
                q = q.Where(x => x.nomb_rubro.Contains(txtCodigoDescripcionoPalabraClave.Text.Trim()) || x.cod_rubro.Contains(txtCodigoDescripcionoPalabraClave.Text.Trim()) || x.bus_rubro.ToLower().Contains(txtCodigoDescripcionoPalabraClave.Text.Trim()));

            var resultados = q.OrderBy(x => x.es_anterior_rubro).ThenBy(x => x.cod_rubro).ToList();
            grdResultados.DataSource = resultados;

            grdResultados.DataBind();

            pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
            lblCantidadRegistros.Text = resultados.Count.ToString();
            db.Dispose();
        }

        private void CargarDatos(int id_datos)
        {

            db = new DGHP_Entities();

            /* var dato = (from personaInhibida in db.PersonasInhibidas
                         select new
                         {
                             id_personainhibida = personaInhibida.id_personainhibida,
                             NroDocumento = personaInhibida.nrodoc_personainhibida,
                             TipoDocumento = personaInhibida.TipoDocumentoPersonal,
                             Cuit = personaInhibida.cuit_personainhibida,
                             NroOrden = personaInhibida.nroorden_personainhibida,
                             NombreYApellido = personaInhibida.nomape_personainhibida,
                             FechaRegistro = personaInhibida.fecharegistro_personainhibida,
                             FechaVencimiento =personaInhibida.fechavencimiento_personainhibida,
                             autos = personaInhibida.autos_personainhibida,
                             juzgado = personaInhibida.juzgado_personainhibida,
                             secretaria = personaInhibida.secretaria_personainhibida,
                             estado = personaInhibida.estado_personainhibida,
                             nroOperador = personaInhibida.operador_personainhibida,
                             observaciones = personaInhibida.observaciones_personainhibida
                         }
                      ).FirstOrDefault(x => x.id_personainhibida == id_datos);

            
             if (dato != null)
             {
                 hid_id_rubroReq.Value = id_datos.ToString();
                 txtTipoDocumentoReq.SelectedValue = Convert.ToString(dato.TipoDocumento.TipoDocumentoPersonalId);
                 txtNroDocumentoReq.Text = Convert.ToString(dato.NroDocumento);
                 txtCuitReq.Text = Convert.ToString(dato.Cuit);
                 txtNroOrdenReq.Text = Convert.ToString(dato.NroOrden);
                 txtNombreyApellidoReq.Text = dato.NombreYApellido;
                 txtFechaRegistroReq.Text = Convert.ToString(dato.FechaRegistro);
                 txtFechaVencimientoReq.Text = Convert.ToString(dato.FechaVencimiento);
                 txtAutosReq.Text = dato.autos;
                 txtJuzgadoReq.Text = dato.juzgado;
                 txtSecretariaReq.Text = dato.secretaria;
                 txtEstadoReq.Text = Convert.ToString(dato.estado);
                 txtNroOperadorReq.Text = Convert.ToString(dato.nroOperador);
                 txtObservacionesReq.Text = dato.observaciones;
             }*/

            //this.GetRows_grdInfoRelevante_Eliminar = null;

            /*  this.GetRows_grdInfoRelevante = null;
              this.GetRows_grdDocRequerido = null;

              int id_estado_modif = 0;
              int tipo_solicitudcambio = 0;

               DesmarcarControlesModificados();
               optRubroHistorico.Checked = false;
               optRubroActual.Checked = false;
               pnlDatosSolicitudCambio.Visible = true;
               LimpiarTooltips();

               btnGuardar.ValidationGroup = "Guardar";

               clsQueryBuilder qry = new clsQueryBuilder(Cnn);
               qry.AppendLn("SELECT * FROM Rubros_Historial_Cambios WHERE id_rubhistcam = ?", id_solicitudcambio);

               DataSet ds = qry.Execute();

               foreach (DataRow dr in ds.Tables[0].Rows)
               {

                   int id_rubro = int.Parse(dr["id_rubro"].ToString());
                   tipo_solicitudcambio = int.Parse(dr["tipo_solicitud_rubhistcam"].ToString());
                   id_estado_modif = int.Parse(dr["id_estado_modif"].ToString());
                   hid_id_rubhistcam.Value = dr["id_rubhistcam"].ToString();
                   hid_id_estado_modif.Value = id_estado_modif.ToString();
                   hid_tipo_solicitudcambio.Value = tipo_solicitudcambio.ToString();
                   hid_id_rubro.Value = dr["id_rubro"].ToString();
                   txtCodRubro.Text = dr["cod_rubro"].ToString();
                   txtDescRubro.Text = dr["nom_rubro"].ToString();
                   txtBusqueda.Text = dr["bus_rubro"].ToString();
                   txtToolTip.Text = dr["tooltip_rubro"].ToString();
                   ddlTipoActividad.SelectedValue = dr["id_tipoactividad"].ToString();
                   ddlTipoDocReq.SelectedValue = dr["id_tipodocreq"].ToString();
                   lblNroSolicitudCambio.Text = dr["id_rubhistcam"].ToString();
                   if (dr["tipo_solicitud_rubhistcam"].ToString().Equals("0"))
                       lblTipoSolicitud.Text = "Alta de Rubro";
                   else
                       lblTipoSolicitud.Text = "Modificación de Rubro";

                   if (Convert.ToBoolean(dr["EsAnterior_Rubro"]))
                       optRubroHistorico.Checked = true;
                   else
                       optRubroActual.Checked = true;

                   txtFechaVigenciaHasta.Text = "";
                   if (dr["VigenciaHasta_rubro"] != DBNull.Value)
                       txtFechaVigenciaHasta.Text = Convert.ToDateTime(dr["VigenciaHasta_rubro"]).ToString("dd/MM/yyyy");


                   txtObservacionesSolicitudCambio.Text = dr["observaciones_rubhistcam"].ToString();

                   grdImpactoAmbiental.DataSource = Logic.TraerImpactoAmbientalPorRubro(id_rubro);
                   grdImpactoAmbiental.DataBind();


                   // ---------------------------------------------------------
                   // Traer los usuarios que modificaron la solicitud de cambio
                   // ---------------------------------------------------------
                   qry.Clear();
                   qry.AppendLn("SELECT ");
                   qry.AppendLn("	usu.UserName , ");
                   qry.AppendLn("	IsNull(usuAdi.Apellido,'') + IsNull(' ' + usuAdi.Nombre ,'') as Apenom,");
                   qry.AppendLn("	hist.LastUpdateDate");
                   qry.AppendLn("FROM ");
                   qry.AppendLn("	Rubros_Historial_Cambios_UsuariosIntervinientes hist");
                   qry.AppendLn("	INNER JOIN aspnet_Users usu ON hist.Userid = usu.UserId");
                   qry.AppendLn("	LEFT JOIN Usuario usuAdi ON usu.UserId = usuAdi.UserId");
                   qry.AppendLn("WHERE");
                   qry.AppendLn("	hist.id_rubhistcam = ?", id_solicitudcambio);

                   DataSet dsUsuInt = qry.Execute();

                   grdUsuariosIntervinientes.DataSource = dsUsuInt;
                   grdUsuariosIntervinientes.DataBind();

            
               }

               CargarZonasRubroSolicitudCambio(id_solicitudcambio);
               CargarInfoRelevanteRubroSolicitudCambio(id_solicitudcambio);
               CargarDocReqRubroSolicitudCambio(id_solicitudcambio);
               CargarEstadosPosibles(id_estado_modif);
               CargarPermisos();
               MostrarDatosModificados();*/

            db.Dispose();
        }

        protected void btnVisualizarRubro_Click(object sender, EventArgs e)
        {
            LinkButton btnVisualizarRubro = (LinkButton)sender;
            VisualizarRubro.CargarRubro(btnVisualizarRubro.CommandArgument);
            updVisualizarRubro.Update();
            this.EjecutarScript(updResultados, "showDatos();");
        }

        protected void btnVisualizarSolicitudCambio_Click(object sender, EventArgs e)
        {
            try
            {

                LinkButton btnVisualizarSolicitudCambio = (LinkButton)sender;
                int id_solicitudCambio = int.Parse(btnVisualizarSolicitudCambio.Text);

                VisualizarRubro.CargarSolicitudCambio(id_solicitudCambio);
                updVisualizarRubro.Update();

                this.EjecutarScript(updResultados, "showDatos();");

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                /*AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(updResultados, updResultados.GetType(),
                        "mostrarPopup", "mostrarPopup('pnlError');", true);*/
                this.EjecutarScript(updResultados, "showfrmError();");
            }
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();

            try
            {
                LinkButton btnEditar = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnEditar.Parent.Parent;

                int id_rubro = int.Parse(btnEditar.CommandArgument);

                int id_solicitudCambio = 0;

                try
                {
                    // id_solicitudCambio = db.Rubros_GenerarSolicitudCambio(id_rubro, 1, (Guid)Membership.GetUser().ProviderUserKey);

                    Guid userid = (Guid)Membership.GetUser().ProviderUserKey;


                    ObjectParameter id_solicitudCambio_param = new ObjectParameter("id_rubhistcam_out", typeof(int));

                    db.Rubros_GenerarSolicitudCambio(id_rubro, 1, userid, id_solicitudCambio_param);

                    id_solicitudCambio = Convert.ToInt32(id_solicitudCambio_param.Value);
                }
                catch (Exception ex)
                {
                    throw new Exception("Se ha producido un error generando la solicitud de cambio.");
                }

                var Rubros_Historial_Cambios = (from rhc in db.Rubros_Historial_Cambios
                                                select new
                                                {
                                                    id_estado_modif = rhc.id_estado_modif,
                                                    id_rubhistcam = rhc.id_rubhistcam
                                                }
                     ).FirstOrDefault(x => x.id_rubhistcam == id_solicitudCambio);
                int id_estado_modif = 0;
                if (Rubros_Historial_Cambios != null)
                    id_estado_modif = Convert.ToInt32(Rubros_Historial_Cambios.id_estado_modif);

                ColorFilaSegunEstado(row, id_estado_modif);
                hid_id_rubroReq.Value = row.RowIndex.ToString();


                LimpiarDatos();
                VisualizarRubro.CargarSolicitudCambio(id_solicitudCambio);
                updVisualizarRubro.Update();

                this.EjecutarScript(updResultados, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");

            }

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

        private void ColorFilaSegunEstado(GridViewRow Row, int id_estado_modif)
        {
            //Estados de modificacion: 0 - En Proceso / 1 - Anulada / 2 - Confirmada / 3 - Aprobada / 4 - Rechazada 

            Color colorFila = Color.White;
            Color colorBorde = Color.White;

            if (id_estado_modif.Equals(0))
            {
                colorFila = ColorTranslator.FromHtml(colorEstadoEnPoceso);
                colorBorde = ColorTranslator.FromHtml(colorBordeEnPoceso);
            }

            if (id_estado_modif.Equals(2))
            {
                colorFila = ColorTranslator.FromHtml(colorEstadoConfirmada);
                colorBorde = ColorTranslator.FromHtml(colorBordeConfirmada);
            }

            if (colorFila != Color.White)
            {
                Row.BackColor = colorFila;
                Row.BorderWidth = Unit.Pixel(1);
            }

            if (colorBorde != Color.White)
            {
                Row.BorderColor = colorBorde;
                Row.BorderWidth = Unit.Pixel(2);
            }
        }

        protected void lnkEliminarCondicionReq_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;

                LinkButton lnkEditar = (LinkButton)sender;
                int idCondicion = int.Parse(lnkEditar.CommandArgument);

                db = new DGHP_Entities();

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        db.Rubros_EliminarRubrosCondiciones(idCondicion, userid);
                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                Buscar();
                updResultados.Update();
                // this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");
            }
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            db = new DGHP_Entities();
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
                string userName = Membership.GetUser().ProviderName;

                int idPersonaInhibida = Convert.ToInt32(hid_id_rubroReq.Value);
                /*  int? idTipoDocPersonaInhibida = Convert.ToInt32(txtTipoDocumentoReq.Text.Trim());
                  int? nroDocumentoPersonaInhibida = Convert.ToInt32(txtNroDocumentoReq.Text.Trim());
                  int? nroOrdenPersonaInhibida = Convert.ToInt32(txtNroOrdenReq.Text.Trim());
                  string cuitPersonaInhibida = txtCuitReq.Text.Trim();
                  string nomapePersonaInhibida = txtNombreyApellidoReq.Text.Trim();
                  DateTime? fechaRegistroPersonaInhibida=null;
                  if (txtFechaRegistroReq.Text!="")
                      fechaRegistroPersonaInhibida = Convert.ToDateTime(txtFechaRegistroReq.Text);

                  DateTime? fechaVencimientoPersonaInhibida=null;
                  if (txtFechaRegistroReq.Text != "")
                      fechaVencimientoPersonaInhibida = Convert.ToDateTime(txtFechaVencimientoReq.Text);

                  string autos = txtAutosReq.Text.Trim();
                  string juzgado = txtJuzgadoReq.Text.Trim();
                  string secretaria = txtSecretariaReq.Text.Trim();
                  int? estado = null;
                  if (txtEstadoReq.Text != "")
                      estado = Convert.ToInt32(txtEstadoReq.SelectedValue.Trim());

                  DateTime? fechabajaPersonaInhibida=null;
                  if (txtFechaRegistroReq.Text != "")
                      fechabajaPersonaInhibida = Convert.ToDateTime(txtFechaBajaReq.Text);

                  int? operadorPersonaInhibida = null;
                  if (txtNroOperadorReq.Text != "")
                     operadorPersonaInhibida = Convert.ToInt32(txtNroOperadorReq.Text.Trim());
                  string observacionesPersonaInhibida = txtObservacionesReq.Text.Trim();

                  using (TransactionScope Tran = new TransactionScope())
                  {
                      try
                      {
                          if (idPersonaInhibida == 0)
                              db.PersonasInhibidas_insert(idTipoDocPersonaInhibida, nroDocumentoPersonaInhibida, nroOrdenPersonaInhibida, cuitPersonaInhibida, nomapePersonaInhibida, fechaRegistroPersonaInhibida, fechaVencimientoPersonaInhibida, autos, juzgado, secretaria, estado, fechabajaPersonaInhibida, operadorPersonaInhibida, observacionesPersonaInhibida, userName);
                          else
                              db.PersonasInhibidas_update(idPersonaInhibida,idTipoDocPersonaInhibida, nroDocumentoPersonaInhibida, nroOrdenPersonaInhibida, cuitPersonaInhibida, nomapePersonaInhibida, fechaRegistroPersonaInhibida, fechaVencimientoPersonaInhibida, autos, juzgado, secretaria, estado, fechabajaPersonaInhibida, operadorPersonaInhibida, observacionesPersonaInhibida, userName);

                          Tran.Complete();
                      }
                      catch (Exception ex)
                      {
                          Tran.Dispose();
                          throw ex;
                      }
                  }
                
                  Buscar();
                  updResultados.Update();

                  this.EjecutarScript(updBotonesGuardar, "showBusqueda();");*/
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                // this.EjecutarScript(updBotonesGuardar, "showfrmError();");

            }
            finally
            {
                db.Dispose();
            }

        }

        #region paginado grilla

        private int codZonaPlaneamiento = 0;
        private string nombreZonaPlaneamiento = "";
        private int codZonaHabilitacion = 0;


        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdResultados.PageIndex = e.NewPageIndex;
                IniciarEntity();
                Buscar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }

        }

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;

            try
            {
                grdResultados.PageIndex = int.Parse(cmdPage.Text) - 1;
                IniciarEntity();
                Buscar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {

            try
            {
                grdResultados.PageIndex = grdResultados.PageIndex - 1;
                IniciarEntity();
                Buscar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {

            try
            {
                grdResultados.PageIndex = grdResultados.PageIndex + 1;
                IniciarEntity();
                Buscar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void grd_DataBound(object sender, EventArgs e)
        {
            try
            {

                GridView grid = (GridView)grdResultados;
                GridViewRow fila = (GridViewRow)grid.BottomPagerRow;

                if (fila != null)
                {
                    LinkButton btnAnterior = (LinkButton)fila.Cells[0].FindControl("cmdAnterior");
                    LinkButton btnSiguiente = (LinkButton)fila.Cells[0].FindControl("cmdSiguiente");

                    if (grid.PageIndex == 0)
                        btnAnterior.Visible = false;
                    else
                        btnAnterior.Visible = true;

                    if (grid.PageIndex == grid.PageCount - 1)
                        btnSiguiente.Visible = false;
                    else
                        btnSiguiente.Visible = true;


                    // Ocultar todos los botones con Números de Página
                    for (int i = 1; i <= 19; i++)
                    {
                        LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                        btn.Visible = false;
                    }


                    if (grid.PageIndex == 0 || grid.PageCount <= 10)
                    {
                        // Mostrar 10 botones o el máximo de páginas

                        for (int i = 1; i <= 10; i++)
                        {
                            if (i <= grid.PageCount)
                            {
                                LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                                btn.Text = i.ToString();
                                btn.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        // Mostrar 9 botones hacia la izquierda y 9 hacia la derecha
                        // o bien los que sea posible en caso de no llegar a 9

                        int CantBucles = 0;

                        LinkButton btnPage10 = (LinkButton)fila.Cells[0].FindControl("cmdPage10");
                        btnPage10.Visible = true;
                        btnPage10.Text = Convert.ToString(grid.PageIndex + 1);

                        // Ubica los 9 botones hacia la izquierda
                        for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                        {
                            CantBucles++;
                            if (i >= 0)
                            {
                                LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 - CantBucles));
                                btn.Visible = true;
                                btn.Text = Convert.ToString(i + 1);
                            }

                        }

                        CantBucles = 0;
                        // Ubica los 9 botones hacia la derecha
                        for (int i = grid.PageIndex + 1; i <= grid.PageIndex + 9; i++)
                        {
                            CantBucles++;
                            if (i <= grid.PageCount - 1)
                            {
                                LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 + CantBucles));
                                btn.Visible = true;
                                btn.Text = Convert.ToString(i + 1);
                            }
                        }



                    }
                    LinkButton cmdPage;
                    string btnPage = "";
                    for (int i = 1; i <= 19; i++)
                    {
                        btnPage = "cmdPage" + i.ToString();
                        cmdPage = (LinkButton)fila.Cells[0].FindControl(btnPage);
                        if (cmdPage != null)
                            cmdPage.CssClass = "btn";

                    }


                    // busca el boton por el texto para marcarlo como seleccionado
                    string btnText = Convert.ToString(grid.PageIndex + 1);
                    foreach (Control ctl in fila.Cells[0].FindControl("pnlpager").Controls)
                    {
                        if (ctl is LinkButton)
                        {
                            LinkButton btn = (LinkButton)ctl;
                            if (btn.Text.Equals(btnText))
                            {
                                btn.CssClass = "btn btn-inverse";
                            }
                        }
                    }

                    UpdatePanel updPnlPager = (UpdatePanel)fila.Cells[0].FindControl("updPnlPager");
                    if (updPnlPager != null)
                        updPnlPager.Update();



                }

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }


        }


        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                LinkButton BotonVisualizar = (LinkButton)e.Row.FindControl("btnVisualizarRubro");
                LinkButton BotonModificar = (LinkButton)e.Row.FindControl("btnEditar");

                if (this.puedeEditarRubro || this.puedeAprobarRubro)
                {

                    string dataItem = e.Row.DataItem.ToString();

                    if (dataItem.IndexOf("id_estado_modif = { ") > 0)
                    {
                        int start = dataItem.IndexOf("id_estado_modif = { ") + 38;
                        string id_estado_modif = dataItem.Substring(start, 1);

                        ColorFilaSegunEstado(e.Row, Convert.ToInt32(id_estado_modif));
                    }
                    BotonModificar.Visible = true;
                    BotonVisualizar.Visible = true;
                }

                if (this.puedeVisualizarRubro)
                {
                    BotonVisualizar.Visible = true;
                }
            }
        }

        private void elimiarFiltro()
        {
            ViewState["filtro"] = null;
        }

        private void guardarFiltro()
        {
            string filtro = this.codZonaPlaneamiento + "|" + this.nombreZonaPlaneamiento + "|" + this.codZonaHabilitacion;
            ViewState["filtro"] = filtro;

        }

        private void recuperarFiltro()
        {
            if (ViewState["filtro"] == null)
                return;

            string filtro = ViewState["filtro"].ToString();

            string[] valores = filtro.Split('|');

            this.codZonaPlaneamiento = Convert.ToInt32(valores[0]);
            this.codZonaHabilitacion = Convert.ToInt32(valores[2]);

            if (string.IsNullOrEmpty(valores[1]))
            {
                this.nombreZonaPlaneamiento = null;
            }
            else
            {
                this.nombreZonaPlaneamiento = valores[1];
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
        #endregion

        #region paginado grilla


        protected void grdSolicitudesActivas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdSolicitudesActivas.PageIndex = e.NewPageIndex;
                DataTable dt = TraerSolicitudesDeCambioActivas();
                grdSolicitudesActivas.DataSource = dt;
                grdSolicitudesActivas.DataBind();
                lblCantidadRegistros.Text = dt.Rows.Count.ToString();
                updResultados.Update();

                this.EjecutarScript(updpnlBuscar, "showResultado();");
            }
            catch (Exception ex)
            {
             
            }

        }

        protected void cmdPage2(object sender, EventArgs e)
        {
            LinkButton cmdPage2 = (LinkButton)sender;

            try
            {
                grdSolicitudesActivas.PageIndex = int.Parse(cmdPage2.Text) - 1;
                DataTable dt = TraerSolicitudesDeCambioActivas();
                grdSolicitudesActivas.DataSource = dt;
                grdSolicitudesActivas.DataBind();
                lblCantidadRegistros.Text = dt.Rows.Count.ToString();
                updResultados.Update();

                this.EjecutarScript(updpnlBuscar, "showResultado();");
            }
            catch (Exception ex)
            {
               
            }
        }

        protected void cmdAnterior2_Click(object sender, EventArgs e)
        {

            try
            {
                grdSolicitudesActivas.PageIndex = grdResultados.PageIndex - 1;
                DataTable dt = TraerSolicitudesDeCambioActivas();
                grdSolicitudesActivas.DataSource = dt;
                grdSolicitudesActivas.DataBind();
                lblCantidadRegistros.Text = dt.Rows.Count.ToString();
                updResultados.Update();

                this.EjecutarScript(updpnlBuscar, "showResultado();");
            }
            catch (Exception ex)
            {

            }
        }

        protected void cmdSiguiente2_Click(object sender, EventArgs e)
        {

            try
            {
                grdSolicitudesActivas.PageIndex = grdResultados.PageIndex + 1;
                DataTable dt = TraerSolicitudesDeCambioActivas();
                grdSolicitudesActivas.DataSource = dt;
                grdSolicitudesActivas.DataBind();
                lblCantidadRegistros.Text = dt.Rows.Count.ToString();
                updResultados.Update();

                this.EjecutarScript(updpnlBuscar, "showResultado();");
            }
            catch (Exception ex)
            {
    
            }
        }

        protected void grdSolicitudesActivas_DataBound(object sender, EventArgs e)
        {
            try
            {

                GridView grid = (GridView)grdSolicitudesActivas;
                GridViewRow fila = (GridViewRow)grid.BottomPagerRow;

                if (fila != null)
                {
                    LinkButton btnAnterior = (LinkButton)fila.Cells[0].FindControl("cmdAnterior");
                    LinkButton btnSiguiente = (LinkButton)fila.Cells[0].FindControl("cmdSiguiente");

                    if (grid.PageIndex == 0)
                        btnAnterior.Visible = false;
                    else
                        btnAnterior.Visible = true;

                    if (grid.PageIndex == grid.PageCount - 1)
                        btnSiguiente.Visible = false;
                    else
                        btnSiguiente.Visible = true;


                    // Ocultar todos los botones con Números de Página
                    for (int i = 1; i <= 19; i++)
                    {
                        LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                        btn.Visible = false;
                    }


                    if (grid.PageIndex == 0 || grid.PageCount <= 10)
                    {
                        // Mostrar 10 botones o el máximo de páginas

                        for (int i = 1; i <= 10; i++)
                        {
                            if (i <= grid.PageCount)
                            {
                                LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                                btn.Text = i.ToString();
                                btn.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        // Mostrar 9 botones hacia la izquierda y 9 hacia la derecha
                        // o bien los que sea posible en caso de no llegar a 9

                        int CantBucles = 0;

                        LinkButton btnPage10 = (LinkButton)fila.Cells[0].FindControl("cmdPage10");
                        btnPage10.Visible = true;
                        btnPage10.Text = Convert.ToString(grid.PageIndex + 1);

                        // Ubica los 9 botones hacia la izquierda
                        for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                        {
                            CantBucles++;
                            if (i >= 0)
                            {
                                LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 - CantBucles));
                                btn.Visible = true;
                                btn.Text = Convert.ToString(i + 1);
                            }

                        }

                        CantBucles = 0;
                        // Ubica los 9 botones hacia la derecha
                        for (int i = grid.PageIndex + 1; i <= grid.PageIndex + 9; i++)
                        {
                            CantBucles++;
                            if (i <= grid.PageCount - 1)
                            {
                                LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 + CantBucles));
                                btn.Visible = true;
                                btn.Text = Convert.ToString(i + 1);
                            }
                        }



                    }
                    LinkButton cmdPage;
                    string btnPage = "";
                    for (int i = 1; i <= 19; i++)
                    {
                        btnPage = "cmdPage" + i.ToString();
                        cmdPage = (LinkButton)fila.Cells[0].FindControl(btnPage);
                        if (cmdPage != null)
                            cmdPage.CssClass = "btn";

                    }


                    // busca el boton por el texto para marcarlo como seleccionado
                    string btnText = Convert.ToString(grid.PageIndex + 1);
                    foreach (Control ctl in fila.Cells[0].FindControl("pnlpager").Controls)
                    {
                        if (ctl is LinkButton)
                        {
                            LinkButton btn = (LinkButton)ctl;
                            if (btn.Text.Equals(btnText))
                            {
                                btn.CssClass = "btn btn-inverse";
                            }
                        }
                    }

                    UpdatePanel updPnlPager = (UpdatePanel)fila.Cells[0].FindControl("updPnlPager");
                    if (updPnlPager != null)
                        updPnlPager.Update();



                }

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }


        }




        #endregion

        #region entity

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

        #endregion


        #region permisos
        private bool puedeEditarRubro;
        private bool PuedeEditarRubro()
        {
            db = new DGHP_Entities();
            Guid userid = Functions.GetUserId();
            /*PERFIL APROBADOR
                - Tiene que poder editar los rubros*/
            if (PuedeAprobarRubro())
                return true;

            var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

            foreach (var perfil in perfiles_usuario)
            {
                var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

                if (menu_usuario.Contains("Editar Rubros"))
                    return true;

            }

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

        private bool puedeAprobarRubro;
        private bool PuedeAprobarRubro()
        {
            db = new DGHP_Entities();
            Guid userid = Functions.GetUserId();

            var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

            foreach (var perfil in perfiles_usuario)
            {
                var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

                if (menu_usuario.Contains("Aprobar ediciones de Rubros"))
                    return true;

            }

            return false;
        }
        #endregion


        #region ExportarXLS
        protected void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                Exportar();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "showfrmError();");
            }


        }

        private void Exportar()
        {
            try
            {
                Rubros rub = new Rubros();
                DGHP_Entities db = new DGHP_Entities();

                var hoja1 = TraerRubros();
                var hoja2 = CondicionesxZona();
                var hoja3 = TraerImpactoAmbiental();

                QueryExcel queryExcel = new QueryExcel();
                queryExcel.Page = this;
                queryExcel.queryList = new List<IEnumerable<object>>() { hoja1, hoja2, hoja3 };
                queryExcel.cant_registros_x_vez = 5000;

                mostrarTimer("Rubros");

                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(queryExcel.ExportarSolicitudesAExcel));
                thread.Start();
                this.EjecutarScript(updExportaExcel, "showfrmExportarExcel();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "showfrmError();");
                updpnlBuscar.Update();
            }
        }

        private IQueryable<ImpactoAmbientalDTO> TraerImpactoAmbiental()
        {
            db = new DGHP_Entities();

            var q = (from rub in db.Rubros
                     join rri in db.Rel_Rubros_ImpactoAmbiental on rub.id_rubro equals rri.id_rubro
                     join imp in db.ImpactoAmbiental on rri.id_ImpactoAmbiental equals imp.id_ImpactoAmbiental
                     select new ImpactoAmbientalDTO
                     {
                         CodigoRubro = rub.cod_rubro,
                         Descripcion = rub.nom_rubro,
                         ImpactoAmbiental = imp.nom_ImpactoAmbiental,
                         desdem2 = rri.DesdeM2,
                         hastam2 = rri.HastaM2,
                         LetraAnexo = rri.LetraAnexo
                     }
                     );
            return q;
        }

        private IQueryable<CondicionxRubroDTO> CondicionesxZona()
        {
            db = new DGHP_Entities();

            var q = (from rub in db.Rubros
                     join rzc in db.RubrosZonasCondiciones on rub.cod_rubro equals rzc.cod_rubro
                     join zha in db.Zonas_Habilitaciones on rzc.cod_ZonaHab equals zha.CodZonaHab
                     join ruc in db.RubrosCondiciones on rzc.cod_condicion equals ruc.cod_condicion
                     select new CondicionxRubroDTO
                     {
                         CodigoRubro = rub.cod_rubro,
                         Descripcion = rub.nom_rubro,
                         Zona = rzc.cod_ZonaHab + "-" + zha.DescripcionZonaHab,
                         Condicion =  rzc.cod_condicion + "-" + ruc.nom_condicion,
                         SupMaxima = ruc.SupMax_condicion,
                         SupMinima = ruc.SupMin_condicion                        
                     }
                     );
            return q;
        }

        private IQueryable<RubroDTO> TraerRubros()
        {
            db = new DGHP_Entities();
            var q = (from rubros in db.Rubros
                     join Tac in db.TipoActividad on rubros.id_tipoactividad equals Tac.Id
                     join Tdr in db.Tipo_Documentacion_Req on rubros.id_tipodocreq equals Tdr.Id
                     join Ral in db.RAL_Licencias on rubros.id_licencia_alcohol equals Ral.id_licencia_alcohol into Ral_lic from rall in Ral_lic.DefaultIfEmpty()
                     join Grc in db.ENG_Grupos_Circuitos on rubros.id_grupo_circuito equals Grc.id_grupo_circuito into gru_cir from gru in gru_cir.DefaultIfEmpty()
                     select new RubroDTO
                     {
                         Descripcion = rubros.nom_rubro,
                         CodigoRubro = rubros.cod_rubro,
                         PalabrasClaves = rubros.tooltip_rubro,
                         InfoDescriptiva = rubros.tooltip_rubro,
                         TipoActividad = Tac.Nombre,
                         TipoTramite = Tdr.Descripcion,
                         RegistroAlcohol = rall.cod_licencia_alcohol, 
                         GrupoCircuito = gru.nom_grupo_circuito,
                         Historico = rubros.EsAnterior_Rubro,
                         CircuitoHabAutomatico = rubros.Circuito_Automatico,                       
                         Uso_Condicionado = rubros.Uso_Condicionado,
                         ValidaCargaDescarga = rubros.ValidaCargaDescarga,
                         OficinaComercial = rubros.OficinaComercial,
                         TieneDeposito = rubros.TieneDeposito,
                         SupMinCargaDescarga = rubros.SupMinCargaDescarga,
                         SupMinCargaDescargaRefII = rubros.SupMinCargaDescargaRefII,
                         PorSupMinCargaDescargaRefV = rubros.SupMinCargaDescargaRefV,
                         Librar_Uso = rubros.Librar_Uso                         
                     }
            );
            return q;
        }

        protected void mostrarTimer(string name)
        {
            btnCerrarExportacion.Visible = false;
            // genera un nombre de archivo aleatorio
            //Random random = new Random((int)DateTime.Now.Ticks);
            //int NroAleatorio = random.Next(0, 100);
            //NroAleatorio = NroAleatorio * random.Next(0, 100);
            name = name + ".xlsx";
            string fileName = string.Format(name);

            Session["exportacion_en_proceso"] = true;
            Session["progress_data"] = "Preparando exportación.";
            Session["filename_exportacion"] = fileName;

            Timer1.Enabled = true;
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                bool exportacion_en_proceso = (Session["exportacion_en_proceso"] != null ? (bool)Session["exportacion_en_proceso"] : false);

                if (exportacion_en_proceso)
                {
                    lblRegistrosExportados.Text = Convert.ToString(Session["progress_data"]);
                    lblExportarError.Text = Convert.ToString(Session["progress_data"]);
                    btnDescargarExcel.Visible = false;
                }
                else
                {
                    Timer1.Enabled = false;
                    btnCerrarExportacion.Visible = true;
                    pnlDescargarExcel.Style["display"] = "block";
                    pnlExportandoExcel.Style["display"] = "none";
                    string filename = Session["filename_exportacion"].ToString();
                    filename = HttpUtility.UrlEncode(filename);
                    btnDescargarExcel.Visible = true;
                    btnDescargarExcel.NavigateUrl = string.Format("~/Controls/DescargarArchivoTemporal.aspx?fname={0}", filename);
                    Session.Remove("filename_exportacion");
                }
                //Cuando falla la exportacion
                if (Session["progress_data"].ToString().StartsWith("Error:"))
                {
                    Timer1.Enabled = false;
                    btnCerrarExportacion.Visible = true;
                    pnlExportandoExcel.Style["display"] = "none";
                    pnlExportacionError.Style["display"] = "block";
                }
            }
            catch
            {
                Timer1.Enabled = false;
            }        
        }

        protected void btnCerrarExportacion_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            Session.Remove("filename_exportacion");
            Session.Remove("progress_data");
            Session.Remove("exportacion_en_proceso");
            pnlExportacionError.Style["display"] = "none";
            pnlDescargarExcel.Style["display"] = "none";
            pnlExportandoExcel.Style["display"] = "block";

            this.EjecutarScript(updExportaExcel, "hidefrmExportarExcel();");

        }
        #endregion
    }
}