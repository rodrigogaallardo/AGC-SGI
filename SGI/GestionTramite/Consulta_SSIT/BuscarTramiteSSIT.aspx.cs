using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using SGI.Controls;
using ExtensionMethods;
using System.Web.Script.Serialization;

namespace SGI
{
    public partial class BuscarTramiteSSIT : BasePage
    {
        private class cls_ultima_tarea
        {
            public int id_solicitud { get; set; }
            public int id_tramitetarea { get; set; }
        }
        private string codigoGuid
        {
            get
            {
                string ret = "";
                if ((Page.RouteData.Values["guidJason"] != null))
                {
                    ret = Page.RouteData.Values["guidJason"].ToString();

                }

                return ret;

            }

        }

        private string estados = "";
        private string tiposTramite = "";
        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {
            string busca = hdUltBtn.Value;

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                //busca establece la funcion de inicio
              
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                 "inicializar_controles0", "inicializar_controles0();", true);
            }

            if (!IsPostBack)
            {
                
                LoadData();
                SiteMaster pmaster = (SiteMaster)this.Page.Master;
                ucMenu mnu = (ucMenu)pmaster.FindControl("mnu");
                mnu.setearMenuActivo(5);

                //------------------------------------------------------------------Si lo tuviera que usar ya instanciado
                ////id_solicitud = txtNroSolicitud.Text;
                //nroExp = txtNroExp.Text;
                ////id_encomida = txtNroEncomienda.Text;
                ////id_tipo_tramite = ddlTipoTramite.SelectedIndex.ToString();
                ////id_tipo_expediente = ddlTipoExpediente.SelectedIndex.ToString();
                ////id_sub_tipo_tramite = ddlSubTipoTramite.SelectedIndex.ToString();
                ////id_tarea = ddlTarea.SelectedIndex.ToString();
                ////id_tarea_cerrada = ddlTareaCerrada.SelectedIndex.ToString();
                ////fechaDesde = txtFechaDesde.Text;
                ////fechaHasta = txtFechaHasta.Text;
                ////fechaCierreDesde = txtFechaCierreDesde.Text;
                ////fechaCierreHasta = txtFechaCierreHasta.Text;
                ////----------------------------------------------------------------Hasta aca filtro por Tramite

                ////rbtnUbiPartidaMatriz = rbtnUbiPartidaMatriz.Checked;
                ////rbtnUbiPartidaHoriz = rbtnUbiPartidaHoriz.Checked;
                ////nro_partida_matriz = txtUbiNroPartida.Text;

                ////id_calle = Convert.ToInt32( ddlCalles.SelectedValue.ToString() );

                ////nro_calle = txtUbiNroPuerta.Text;
                //uf = txtUF.Text;
                //torre = txtTorre.Text;
                //dpto = txtDpto.Text;
                //local = txtLocal.Text;
                ////seccion = txtUbiSeccion.Text;
                //manzana = txtUbiManzana.Text;
                //parcela = txtUbiParcela.Text;
                ////id_tipo_ubicacion = ddlbiTipoUbicacion.SelectedIndex.ToString();
                ////id_sub_tipo_ubicacion = ddlUbiSubTipoUbicacion.SelectedIndex.ToString();
                //rubro_desc = txtRubroCodDesc.Text;
                //tit_razon = txtTitApellido.Text;
                //jsonString = filtros.ToJSON();

                FiltrosConsulta filtros = new FiltrosConsulta()
                {
                    id_solicitud = txtNroSolicitud.Text,
                    id_tipo_tramite = ddlTipoTramite.SelectedValue.ToString(),
                    id_estado = ddlEstado.SelectedValue.ToString(),
                    domicilio = txtDomicilio.Text,
                    nroExp = txtNroExp.Text
                };


                busca = hdUltBtn.Value;
                //busca = hdMyControl.Value;
                
                /*        ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                            "inicializar_controles0", "inicializar_controles0();", true);
                       */

            }

            //IniciarEntityFiles();

            //var doc =
            //    (
            //        from c in dbFiles.Certificados
            //        where c.id_certificado == 117665
            //        select new
            //        {
            //            c.Certificado
            //        }
            //    ).FirstOrDefault();

            //if ( doc!= null)
            //    System.IO.File.WriteAllBytes("C:\\Users\\cnieto.AR.MOST\\Desktop\\borrar\\planos_adjunto.pdf", doc.Certificado);
        }

        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
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
        bool hayFiltroBusqueda = false;
        public bool HayFiltro
        {
            get { return hayFiltroBusqueda; }
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

        public void LoadData()
        {

            //hid_bt_tramite_collapse.Value = "true";

            try
            {
                IniciarEntity();
                CargarCombo_TipoTramite();
                CargarCombo_Estado(-1);
                //CargarCombo_tareas(0, 0, 0);
                updPnlFiltroBuscar_tramite.Update();

                FinalizarEntity();

                
                //grdTramites.DataBind();

               // FinalizarEntity();

                if (!String.IsNullOrWhiteSpace(codigoGuid))
                {
                    recuperarFiltro(codigoGuid);
                    grdTramites.DataBind();

                    FinalizarEntity();

                    EjecutarScript(btn_BuscarTramite, "showResultado();");

                }

                //EjecutarScript(btn_BuscarTramite, "showResultado();");


            }
            catch (Exception ex)
            {
                FinalizarEntity();
                throw ex;
            }

        }

        private void CargarCombo_TipoTramite()
        {
            List<string> lstOcultarTipoTramite = new List<string>();
            lstOcultarTipoTramite.Add("LIGUE");
            lstOcultarTipoTramite.Add("DESLIGUE");
            //lstOcultarTipoTramite.Add("AMPLIACION/UNIFICACION");
            lstOcultarTipoTramite.Add("RECTIF_HABILITACION");
            //lstOcultarTipoTramite.Add("REDISTRIBUCION_USO");

            List<TipoTramite> list_tipoTramite = this.db.TipoTramite.Where(x => !lstOcultarTipoTramite.Contains(x.cod_tipotramite)).ToList();

            TipoTramite tipoTramite_todos = new TipoTramite();
            tipoTramite_todos.id_tipotramite = 0;
            tipoTramite_todos.descripcion_tipotramite = "Todos";

            list_tipoTramite.Insert(0, tipoTramite_todos);
            ddlTipoTramite.DataTextField = "descripcion_tipotramite";
            ddlTipoTramite.DataValueField = "id_tipotramite";

            ddlTipoTramite.DataSource = list_tipoTramite;
            ddlTipoTramite.DataBind();
        }

        private void CargarCombo_Estado(int id_tipotramite)
        {
            IniciarEntity();
            if (id_tipotramite >= 0)
            {
                if (id_tipotramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                {
                    List<CPadron_Estados> lstCpadron = this.db.CPadron_Estados.ToList();

                    CPadron_Estados Estado_cpadron = new CPadron_Estados();
                    Estado_cpadron.id_estado = 0;
                    Estado_cpadron.nom_estado_usuario = "Todos";
                    
                    lstCpadron.Add(Estado_cpadron);

                    ddlEstado.DataSource = lstCpadron;
                    ddlEstado.DataTextField = "nom_estado_usuario";
                    ddlEstado.DataValueField = "id_estado";
                }
                else
                {
                    List<TipoEstadoSolicitud> estados = new List<TipoEstadoSolicitud>();
                    estados = db.TipoEstadoSolicitud.ToList();

                    TipoEstadoSolicitud tipoEstado_todos = new TipoEstadoSolicitud();
                    tipoEstado_todos.Id = 0;
                    tipoEstado_todos.Nombre = "Todos";

                    estados.Add(tipoEstado_todos);

                    ddlEstado.DataSource = estados;
                    ddlEstado.DataTextField = "Descripcion";
                    ddlEstado.DataValueField = "Id";

                }
            }
            ddlEstado.DataBind();
            ddlEstado.Items.Insert(0, new ListItem("(Todos)", "0"));
            FinalizarEntity();
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
                ScriptManager.RegisterStartupScript(btn_BuscarTramite, btn_BuscarTramite.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
            }

        }



        #endregion

        #region buscar tramite
        protected void ddlTipoTramite_SelectedIndexChanged(object sender, EventArgs e)
        {

            int id_tipotramite = 0;
            int.TryParse(ddlTipoTramite.SelectedValue, out id_tipotramite);
            CargarCombo_Estado(id_tipotramite);

        }
        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            EjecutarScript(btn_BuscarTramite, "hideResultado();");
            txtNroSolicitud.Text = "";
            if (ddlTipoTramite.Items.Count >= 0)
                ddlTipoTramite.SelectedIndex = 0;

            if (ddlEstado.Items.Count >= 0)
                ddlEstado.SelectedIndex = 0;

            updPnlFiltroBuscar_tramite.Update();

            pnlResultadoBuscar.Visible = false;
            updPnlResultadoBuscar.Update();

        }
     

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {

                IniciarEntity();

                Validar();

                guardarFiltro();

               // grdTramites.DataBind();

                //FinalizarEntity();
                //EjecutarScript(btn_BuscarTramite, "showResultado();");

            }
            catch (Exception ex)
            {
                FinalizarEntity();
                LogError.Write(ex, "error al buscar tramites buscar_tramite-btnBuscar_OnClick");
                Enviar_Mensaje(ex.Message, "");
            }

        }

        public List<clsItemConsultaSSIT> GetTramites(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {

            totalRowCount = 0;

            //if (!recuperarFiltro(codigoGuid))
            //{
            //    pnlResultadoBuscar.Visible = false;
            //    updPnlResultadoBuscar.Update();
            //    return null;
            //}

            List<clsItemConsultaSSIT> lstResult = FiltrarTramites(startRowIndex, maximumRows, sortByExpression, out totalRowCount);

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
            updPnlResultadoBuscar.Update();

            return lstResult;
        }


        #endregion

        #region paginado grilla

        protected void grdTramites_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTramites.PageIndex = e.NewPageIndex;
        }

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;
            grdTramites.PageIndex = int.Parse(cmdPage.Text) - 1;
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdTramites.PageIndex = grdTramites.PageIndex - 1;
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdTramites.PageIndex = grdTramites.PageIndex + 1;
        }

        protected void grdTramites_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)grdTramites;
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

            }
        }

        #endregion

        #region validaciones

        private void Validar()
        {
            Validar_BuscarPorTramite();
            if (!hayFiltroPorTramite)
            {
                throw new Exception("Debe ingresar algún filtro de búsqueda.");
            }
        }

        private int id_solicitud = 0;
        private int id_encomida = 0;
        private int id_tipo_tramite = 0;
        private int id_estado = 0;
        private string nroExp = "";
        private string domicilio = "";


        private bool hayFiltroPorTramite = false;
        private void Validar_BuscarPorTramite()
        {
            this.hayFiltroPorTramite = false;

            int idAux = 0;

            this.id_solicitud = 0;
            this.id_encomida = 0;
            this.id_tipo_tramite = 0;
            this.nroExp = "";
            
            idAux = 0;
            int.TryParse(txtNroSolicitud.Text, out idAux);
            this.id_solicitud = idAux;           

            idAux = 0;
            int.TryParse(ddlTipoTramite.SelectedItem.Value, out idAux);
            this.tiposTramite = hid_tipotramite_selected.Value;

            idAux = 0;
            int.TryParse(ddlEstado.SelectedItem.Value, out idAux);
            this.id_estado = idAux;


            if (!string.IsNullOrEmpty(txtNroExp.Text))
            {
                this.nroExp = txtNroExp.Text;
            }

            if (!string.IsNullOrEmpty(txtDomicilio.Text))
            {
                this.domicilio = txtDomicilio.Text;
            }

            if (this.id_solicitud > 0 || this.id_encomida > 0 ||
                this.id_tipo_tramite > 0 || this.id_estado > 0 || this.nroExp != "" || this.domicilio != "")
                this.hayFiltroPorTramite = true;

        }       

      
        #endregion
        protected void grdTramites_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                row.Cells[4].Text = row.Cells[4].Text.Replace("\\n", "<br />");
            }

        }    

        #region "Filtro"
        private string parsear(string txExp)
        {
            string nro_Expediente = "";
            if (!string.IsNullOrEmpty(txExp))
            {
                string[] expedien = txExp.Split('-');
                for (int i = 0; i <= expedien.Length - 1; i++)
                {
                    string valor = expedien[i].Trim();
                    nro_Expediente += valor;


                }
                //this.nroExp = txtNroExp.Text;

            }
            return nro_Expediente;

        }
        private List<clsItemConsultaSSIT> FiltrarTramites(int startRowIndex, int maximumRows, string sortByExpression, out int totalRowCount)
        {
            DGHP_Entities db = new DGHP_Entities();           
            

            if (String.IsNullOrWhiteSpace(txtNroSolicitud.Text))
            {
                this.id_solicitud = 0;
            }
            else
            {
                this.id_solicitud = Convert.ToInt32(txtNroSolicitud.Text);
            }
            if (String.IsNullOrWhiteSpace(txtNroExp.Text))
            {
                this.nroExp = "";
            }
            else
            {
                this.nroExp = txtNroExp.Text;
            }
            if (String.IsNullOrWhiteSpace(txtDomicilio.Text))
            {
                this.domicilio = "";
            }
            else
            {
                this.domicilio = txtDomicilio.Text;
            }
            if (String.IsNullOrWhiteSpace(ddlTipoTramite.SelectedValue))
            {
                this.id_tipo_tramite = 0;
            }
            else
            {
                this.id_tipo_tramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);
            }

            if (String.IsNullOrWhiteSpace(ddlEstado.SelectedValue))
            {
                this.id_estado = 0;
            }
            else
            {
                this.id_estado = Convert.ToInt32(ddlEstado.SelectedValue);
            }

            if (this.id_solicitud == 0 &&
                this.nroExp == string.Empty &&
                this.domicilio == string.Empty &&
                this.id_tipo_tramite == 0 &&
                this.id_estado == 0)
            {
                totalRowCount = 0;
                return new List<clsItemConsultaSSIT>();

            }

            List<clsItemConsultaSSIT> resultados = new List<clsItemConsultaSSIT>();
            /*IQueryable<clsItemConsultaSSIT> qFinal = null;
            IQueryable<clsItemConsultaSSIT> qENC = null;
            IQueryable<clsItemConsultaSSIT> qCP = null;
            IQueryable<clsItemConsultaSSIT> qTR = null;*/

            db.Database.CommandTimeout = 300;

            Guid userid = Functions.GetUserId();

            var lstTramites = (from ssit in db.SSIT_Solicitudes
                               select new clsItemConsultaSSIT
                               {
                                   id_solicitud = ssit.id_solicitud,
                                   FechaInicio = ssit.CreateDate,
                                   id_tipotramite = ssit.id_tipotramite,
                                   TipoTramite = ssit.TipoTramite.descripcion_tipotramite,
                                   id_estado = ssit.id_estado,
                                   Estado = ssit.TipoEstadoSolicitud.Descripcion,
                                   Domicilio = "",
                                   NumeroExp = ssit.NroExpedienteSade,
                                   url_visorTramite = "~/GestionTramite/Consulta_SSIT/VisorTramite_SSIT.aspx?id={0}&idTipo={1}"
                               }).Union(
                               (from transf in db.Transf_Solicitudes
                                select new clsItemConsultaSSIT
                                {
                                    id_solicitud = transf.id_solicitud,
                                    FechaInicio = transf.CreateDate,
                                    id_tipotramite = transf.id_tipotramite,
                                    TipoTramite = transf.TipoTramite.descripcion_tipotramite,
                                    id_estado = transf.id_estado,
                                    Estado = transf.TipoEstadoSolicitud.Descripcion,
                                    Domicilio = "",
                                    NumeroExp = transf.NroExpedienteSade,
                                    url_visorTramite = "~/GestionTramite/Consulta_SSIT/VisorTramite_SSIT.aspx?id={0}&idTipo={1}"
                                })).Union(
                                (from cpadron in db.CPadron_Solicitudes
                                 select new clsItemConsultaSSIT
                                 {
                                     id_solicitud = cpadron.id_cpadron,
                                     FechaInicio = cpadron.CreateDate,
                                     id_tipotramite = cpadron.id_tipotramite,
                                     TipoTramite = cpadron.TipoTramite.descripcion_tipotramite,
                                     id_estado = cpadron.id_estado,
                                     Estado = cpadron.CPadron_Estados.nom_estado_usuario,
                                     Domicilio = "",
                                     NumeroExp = cpadron.NroExpedienteSade,
                                     url_visorTramite = "~/GestionTramite/Consulta_SSIT/VisorTramite_SSIT.aspx?id={0}&idTipo={1}"
                                 }));

            if (this.id_solicitud > 0)
                lstTramites = lstTramites.Where(idSol => idSol.id_solicitud == id_solicitud);

            if (this.id_tipo_tramite > 0)
                lstTramites = lstTramites.Where(idTr => idTr.id_tipotramite == this.id_tipo_tramite);

            if (this.id_estado > 0)
                lstTramites = lstTramites.Where(idEst => idEst.id_estado == this.id_estado);

            if (!string.IsNullOrEmpty(this.nroExp))
                lstTramites = lstTramites.Where(nroExp => (nroExp.NumeroExp != null ? nroExp.NumeroExp.Replace(" ", "") : nroExp.NumeroExp) == this.nroExp.Replace(" ", ""));


            totalRowCount = lstTramites.Count();

            lstTramites = lstTramites.OrderByDescending(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);

            resultados = lstTramites.ToList();

            #region "Domicilios y datos adicionales"

           
            if (resultados.Count > 0)
            {

                //------------------------------
                //Obtener las Direcciones del ENC
                //-------------------------------
                List<clsItemDireccion> lstDireccionesENC = new List<clsItemDireccion>();
                string[] arrSolicitudesENC = (from r in resultados
                                              where r.id_tipotramite == (int)Constants.TipoDeTramite.Habilitacion
                                              select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesENC.Length > 0)
                    lstDireccionesENC = Shared.GetDireccionesENC(arrSolicitudesENC);

                //------------------------------
                //Obtener las Direcciones del CP
                //-------------------------------
                List<clsItemDireccion> lstDireccionesCP = new List<clsItemDireccion>();
                string[] arrSolicitudesCP = (from r in resultados
                                             where r.id_tipotramite == (int)Constants.TipoDeTramite.Consulta_Padron
                                             select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesCP.Length > 0)
                    lstDireccionesCP = Shared.GetDireccionesCP(arrSolicitudesCP);

                //------------------------------
                //Obtener las Direcciones del TR
                //-------------------------------
                List<clsItemDireccion> lstDireccionesTR = new List<clsItemDireccion>();
                string[] arrSolicitudesTR = (from r in resultados
                                             where r.id_tipotramite == (int)Constants.TipoDeTramite.Transferencia
                                             select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesTR.Length > 0)
                    lstDireccionesTR = Shared.GetDireccionesTR(arrSolicitudesTR);


                ////------------------------------------------------------------------------
                //Rellena la clase a devolver con los datos que faltaban (Direccion, dias transcurrido)
                //------------------------------------------------------------------------
                foreach (var row in resultados)
                {
                    clsItemDireccion itemDireccion = null;
                    // ENC
                    if (row.id_tipotramite == (int)Constants.TipoDeTramite.Habilitacion)
                        itemDireccion = lstDireccionesENC.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);
                    else if (row.id_tipotramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                        itemDireccion = lstDireccionesCP.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);
                    else if (row.id_tipotramite == (int)Constants.TipoDeTramite.Transferencia)
                        itemDireccion = lstDireccionesTR.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);

                    // Llenado para todos
                    if (itemDireccion != null)
                        row.Domicilio = (string.IsNullOrEmpty(itemDireccion.direccion) ? "" : itemDireccion.direccion);

                    row.url_visorTramite = string.Format(row.url_visorTramite, row.id_solicitud.ToString(), row.id_tipotramite.ToString());                    
                }
            }

            if (!string.IsNullOrEmpty(this.domicilio))
            {
                resultados = resultados.Where(x => x.Domicilio.ToLower().Contains(this.domicilio.ToLower())).ToList();
                totalRowCount = resultados.Count();
            }
           #endregion
            db.Dispose();

            return resultados;
        }

        private bool recuperarFiltro(string idFiltro)
        {


            //string filtro = ViewState["filtro"].ToString();

            //string[] valores = filtro.Split('|');

            DGHP_Entities db = new DGHP_Entities();
            var elements = (from filtrosBase in db.SGI_FiltrosBusqueda
                            where idFiltro == filtrosBase.Id_Busqueda.ToString()
                            select filtrosBase).FirstOrDefault();

            if (elements == null)
                return false;
            string jsonInput = elements.Filtros;

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            FiltrosConsulta filtros = jsonSerializer.Deserialize<FiltrosConsulta>(jsonInput);
            hdUltBtn.Value = elements.botonAccion;

            txtNroSolicitud.Text = filtros.id_solicitud.ToString();
            ddlTipoTramite.SelectedIndex = Convert.ToInt32(filtros.id_tipo_tramite);
            int idTipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);
            CargarCombo_Estado(idTipoTramite);
            ddlEstado.SelectedIndex = Convert.ToInt32(filtros.id_estado);
            txtNroExp.Text = filtros.nroExp;
            txtDomicilio.Text = filtros.domicilio;



            if (String.IsNullOrWhiteSpace(filtros.id_solicitud))
            {
                this.id_solicitud = 0;
            }
            else
            {
                this.id_solicitud = Convert.ToInt32(filtros.id_solicitud);
            }
            if (String.IsNullOrWhiteSpace(filtros.id_tipo_tramite))
            {
                this.id_tipo_tramite = 0;
            }
            else
            {
                this.id_tipo_tramite = Convert.ToInt32(filtros.id_tipo_tramite);
            }
            if (String.IsNullOrWhiteSpace(filtros.id_estado))
            {
                this.id_estado = 0;
            }
            else
            {
                this.id_estado = Convert.ToInt32(filtros.id_estado);
            }

            return true;
        }

        private void guardarFiltro()
        {

            FiltrosConsulta filtros = new FiltrosConsulta()
            {
                id_solicitud = txtNroSolicitud.Text,
                id_tipo_tramite = ddlTipoTramite.SelectedIndex.ToString(),
                id_estado = ddlEstado.SelectedIndex.ToString(),
                domicilio = txtDomicilio.Text,
                nroExp =  txtNroExp.Text
            };
            string jsonString = filtros.ToJSON();

            if (jsonString != "")
            {
                hayFiltroBusqueda = true;
                Guid userid = Functions.GetUserId();
                Guid guidJson = Guid.NewGuid();
                SGI_FiltrosBusqueda filtrosInsertar = new SGI_FiltrosBusqueda();
                filtrosInsertar.Filtros = jsonString;
                filtrosInsertar.Id_Busqueda = guidJson;
                filtrosInsertar.CreateDate = DateTime.Today;
                filtrosInsertar.CreateUser = userid;

                DGHP_Entities db = new DGHP_Entities();
                db.SGI_FiltrosBusqueda.Add(filtrosInsertar);
                db.SaveChanges();

                Response.Redirect(string.Format("~/GestionTramite/Consulta_SSIT/BusquedaTramiteSSIT" + "/" + "{0}", guidJson), false);
            }

        }
        #endregion

    }
}
