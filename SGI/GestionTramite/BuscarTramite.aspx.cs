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
    public partial class BuscarTramite : BasePage
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
        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {
            string busca = hdUltBtn.Value;

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                //busca establece la funcion de inicio
                switch (busca)
                {
                    case "porTramite":
                        ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                            "inicializar_controles0", "inicializar_controles0();", true);
                        break;
                    case "porPartida":
                        ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                            "inicializar_controles1", "inicializar_controles1();", true);
                        break;
                    case "porDomicilio":
                        ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                            "inicializar_controles2", "inicializar_controles2();", true);
                        break;
                    case "porSMP":
                        ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                            "inicializar_controles3", "inicializar_controles3();", true);
                        break;
                    case "porUbi":
                        ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                            "inicializar_controles4", "inicializar_controles4();", true);
                        break;
                }
            }

            if (!IsPostBack)
            {
                LoadData();
                SiteMaster pmaster = (SiteMaster)this.Page.Master;
                ucMenu mnu = (ucMenu)pmaster.FindControl("mnu");
                mnu.setearMenuActivo(5);

                
                FiltrosBusqueda filtros = new FiltrosBusqueda()
                {
                    id_solicitud = txtNroSolicitud.Text,
                    nroExp = txtNroExp.Text,
                    id_encomida = txtNroEncomienda.Text,
                    id_tipo_tramite = ddlTipoTramite.SelectedIndex.ToString(),
                    id_tipo_expediente = ddlTipoExpediente.SelectedIndex.ToString(),
                    id_sub_tipo_tramite = ddlSubTipoTramite.SelectedIndex.ToString(),
                    id_tarea = ddlTarea.SelectedIndex.ToString(),
                    id_tarea_cerrada = ddlTareaCerrada.SelectedIndex.ToString(),
                    fechaDesde = txtFechaDesde.Text,
                    fechaHasta = txtFechaHasta.Text,
                    fechaCierreDesde = txtFechaCierreDesde.Text,
                    fechaCierreHasta = txtFechaCierreHasta.Text,
                    ////----------------------------------------------------------------Hasta aca filtro por Tramite

                    rbtnUbiPartidaMatriz = rbtnUbiPartidaMatriz.Checked,
                    rbtnUbiPartidaHoriz = rbtnUbiPartidaHoriz.Checked,

                    nro_partida_matriz = txtUbiNroPartida.Text,
                    id_calle = ddlCalles.SelectedValue.ToString(),
                    nro_calle = txtUbiNroPuerta.Text,
                    uf = txtUF.Text,
                    torre = txtTorre.Text,
                    dpto = txtDpto.Text,
                    local = txtLocal.Text,
                    nro_puerta_desde = txtNroPuertaDesde.Text,
                    nro_puerta_hasta = txtNroPuertaHasta.Text,
                    rbtn_nro_puerta_par = rbtnNroPuertaPar.Checked,
                    rbtn_nro_puerta_impar = rbtnNroPuertaImpar.Checked,
                    rbtn_nro_puerta_ambas = rbtnNroPuertaAmbas.Checked,
                    seccion = txtUbiSeccion.Text,
                    manzana = txtUbiManzana.Text,
                    parcela = txtUbiParcela.Text,
                    id_tipo_ubicacion = ddlbiTipoUbicacion.SelectedIndex.ToString(),
                    id_sub_tipo_ubicacion = ddlUbiSubTipoUbicacion.SelectedIndex.ToString(),
                    rubro_desc = txtRubroCodDesc.Text,
                    tit_razon = txtTitApellido.Text
                };
                string jsonString = filtros.ToJSON();

                if (jsonString != "")
                {
                    if (
                        //rbtnUbiPartidaMatriz.Checked ||
                        //rbtnUbiPartidaHoriz.Checked ||
                        ddlCalles.SelectedValue.ToString() != "" ||
                        nro_calle != 0 ||
                        uf != "" ||
                        torre != "" ||
                        dpto != "" ||
                        local != "" ||
                        (nro_calle_desde != 0 && nro_calle_hasta != 0) ||
                        txtUbiSeccion.Text != "" ||
                        manzana != "" ||
                        parcela != "" ||
                        id_tipo_ubicacion != -1 ||
                        id_sub_tipo_ubicacion != -1 ||
                        rubro_desc != "" ||
                        tit_razon != ""
                        )
                    {
                        hayFiltroBusqueda = true;

                    }
                };

                busca = hdUltBtn.Value;
                //busca = hdMyControl.Value;

                switch (busca)
                {
                    case "porTramite":
                        ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                            "inicializar_controles0", "inicializar_controles0();", true);
                        break;
                    case "porPartida":
                        ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                            "inicializar_controles1", "inicializar_controles1();", true);
                        break;
                    case "porDomicilio":
                        ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                            "inicializar_controles2", "inicializar_controles2();", true);
                        break;
                    case "porSMP":
                        ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                            "inicializar_controles3", "inicializar_controles3();", true);
                        break;
                    case "porUbi":
                        ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                            "inicializar_controles4", "inicializar_controles4();", true);
                        break;
                    default:
                        ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                            "inicializar_controles0", "inicializar_controles0();", true);
                        hdUltBtn.Value = "porTramite";
                        //hdMyControl.Value = "porTramite";
                        break;
                }

            }


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
                CargarCombo_TipoExpediente(0);
                CargarCombo_subtipoTramite(0);
                CargarCombo_tareas();                
                CargarCombo_tareasCerradas();
                CargarCalles();
                CargarCombo_tipoUbicacion();
                CargarCombo_subTipoUbicacion(-1);
                updPnlFiltroBuscar_tramite.Update();
                updPnlFiltroBuscar_ubi_partida.Update();
                updPnlFiltroBuscar_ubi_dom.Update();
                updPnlFiltroBuscar_ubi_smp.Update();
                updPnlFiltroBuscar_ubi_especial.Update();
                updPnlFiltroBuscar_rubros.Update();
                updPnlFiltroBuscar_titulares.Update();

                FinalizarEntity();

                if (!String.IsNullOrWhiteSpace(codigoGuid))
                {
                    recuperarFiltro(codigoGuid);
                    grdTramites.DataBind();

                    FinalizarEntity();

                    EjecutarScript(btn_BuscarTramite, "showResultado();");

                }


            }
            catch (Exception ex)
            {
                FinalizarEntity();
                throw ex;
            }

        }

        //private void CargarCombo_EstadoTramite()
        //{
        //    List<TipoEstadoSolicitud> estados = this.db.TipoEstadoSolicitud.OrderBy(x => x.Descripcion).ToList();

        //    ddlEstado.DataTextField = "Descripcion";
        //    ddlEstado.DataValueField = "Id";

        //    ddlEstado.DataSource = estados;
        //    ddlEstado.DataBind();
        //}

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

        private void CargarCombo_TipoExpediente(int idtipoTramite)
        {
            List<TipoExpediente> list_tipoTramite = new List<TipoExpediente>();
            if (idtipoTramite == 0)
                list_tipoTramite = this.db.TipoExpediente.ToList();
            else
            {
                var q = (
                            from rel in db.ENG_Rel_Circuitos_TiposDeTramite
                            join tipo in db.TipoExpediente on rel.id_tipoexpediente equals tipo.id_tipoexpediente
                            where rel.id_tipotramite == idtipoTramite
                            select tipo
                        ).Distinct();
                list_tipoTramite = q.ToList();
            }

            TipoExpediente tipoTramite_todos = new TipoExpediente();
            tipoTramite_todos.id_tipoexpediente = 0;
            tipoTramite_todos.descripcion_tipoexpediente = "Todos";

            list_tipoTramite.Insert(0, tipoTramite_todos);
            ddlTipoExpediente.DataTextField = "descripcion_tipoexpediente";
            ddlTipoExpediente.DataValueField = "id_tipoexpediente";

            ddlTipoExpediente.DataSource = list_tipoTramite;
            ddlTipoExpediente.DataBind();
        }

        private void CargarCombo_subtipoTramite(int idTipoExpediente)
        {
            var q = idTipoExpediente == 0 ?
                    (
                        from ts in db.Rel_TipoExpediente_SubtipoExpediente
                        join tipo in db.TipoExpediente on ts.id_tipoexpediente equals tipo.id_tipoexpediente
                        join subtipo in db.SubtipoExpediente on ts.id_subtipoexpediente equals subtipo.id_subtipoexpediente
                        select new
                        {
                            Codigo = ts.id_tipo_subtipo,
                            Texto = tipo.descripcion_tipoexpediente + " - " + subtipo.descripcion_subtipoexpediente
                        }
                    ).ToList()
                    :
                    (
                        from ts in db.Rel_TipoExpediente_SubtipoExpediente
                        join tipo in db.TipoExpediente on ts.id_tipoexpediente equals tipo.id_tipoexpediente
                        join subtipo in db.SubtipoExpediente on ts.id_subtipoexpediente equals subtipo.id_subtipoexpediente
                        where ts.id_tipoexpediente == idTipoExpediente
                        select new
                        {
                            Codigo = ts.id_tipo_subtipo,
                            Texto = tipo.descripcion_tipoexpediente + " - " + subtipo.descripcion_subtipoexpediente
                        }
                    ).ToList();

            List<Items> lista_subtipo = new List<Items>();

            lista_subtipo.Insert(0, new Items("Todos", "0"));

            foreach (var item in q)
            {
                lista_subtipo.Add(new Items(item.Texto, item.Codigo));
            }


            ddlSubTipoTramite.DataTextField = "Texto";
            ddlSubTipoTramite.DataValueField = "Codigo";

            ddlSubTipoTramite.DataSource = lista_subtipo;
            ddlSubTipoTramite.DataBind();
        }



        private void CargarCombo_tareas()
        {
            var qTareas =
                    (
                    from t in this.db.ENG_Tareas
                    join c in this.db.ENG_Circuitos on t.id_circuito equals c.id_circuito
                    orderby t.id_circuito
                    select new
                    {
                        id_tarea = t.id_tarea,
                        nombre_tarea = c.ENG_Grupos_Circuitos.cod_grupo_circuito + " - " + t.nombre_tarea,
                    }
                    ).ToList().Distinct();

            ddlTarea.DataTextField = "nombre_tarea";
            ddlTarea.DataValueField = "id_tarea";
            
            ddlTarea.DataSource = qTareas;
            ddlTarea.DataBind();
            ListItem lst = new ListItem()
            {
                Text = "Todas",
                Value = "0"
            };
            ddlTarea.Items.Insert(0,lst);
        }

        private void CargarCombo_tareasCerradas()
        {
            var qTareasCerradas =
                    (
                    from t in this.db.ENG_Tareas
                    join c in this.db.ENG_Circuitos on t.id_circuito equals c.id_circuito
                    
                    orderby t.id_circuito
                    select new
                    {
                        id_tarea = t.id_tarea,
                        nombre_tarea = c.ENG_Grupos_Circuitos.cod_grupo_circuito + " - " + t.nombre_tarea,
                    }
                    ).ToList().Distinct();

            ddlTareaCerrada.DataTextField = "nombre_tarea";
            ddlTareaCerrada.DataValueField = "id_tarea";           

            ddlTareaCerrada.DataSource = qTareasCerradas;
            ddlTareaCerrada.DataBind();
            ListItem lst = new ListItem()
            {
                Text = "Todas",
                Value = "0"
            };            
            ddlTareaCerrada.Items.Insert(0,lst);
        }
        private void CargarCombo_tipoUbicacion()
        {

            List<TiposDeUbicacion> lista = this.db.TiposDeUbicacion.Where(x => x.id_tipoubicacion > 0).ToList();
            TiposDeUbicacion tipo_ubi = new TiposDeUbicacion();
            tipo_ubi.id_tipoubicacion = -1;
            tipo_ubi.descripcion_tipoubicacion = "Todos";
            lista.Insert(0, tipo_ubi);
            ddlbiTipoUbicacion.DataTextField = "descripcion_tipoubicacion";
            ddlbiTipoUbicacion.DataValueField = "id_tipoubicacion";
            ddlbiTipoUbicacion.DataSource = lista;
            ddlbiTipoUbicacion.DataBind();
            updPnlFiltroBuscar_ubi_especial.Update();
        }

        private void CargarCombo_subTipoUbicacion(int id_tipoubicacion)
        {

            List<SubTiposDeUbicacion> lista = this.db.SubTiposDeUbicacion.Where(x => x.id_tipoubicacion == id_tipoubicacion).ToList();

            SubTiposDeUbicacion sub_tipo_ubi = new SubTiposDeUbicacion();
            sub_tipo_ubi.id_subtipoubicacion = -1;
            sub_tipo_ubi.descripcion_subtipoubicacion = "Todos";
            lista.Insert(0, sub_tipo_ubi);

            ddlUbiSubTipoUbicacion.DataTextField = "descripcion_subtipoubicacion";
            ddlUbiSubTipoUbicacion.DataValueField = "id_subtipoubicacion";
            ddlUbiSubTipoUbicacion.DataSource = lista;
            ddlUbiSubTipoUbicacion.DataBind();

        }

        protected void ddlbiTipoUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                IniciarEntity();
                int id_tipoubicacion = int.Parse(ddlbiTipoUbicacion.SelectedValue);
                CargarCombo_subTipoUbicacion(id_tipoubicacion);
                FinalizarEntity();
            }
            catch (Exception)
            {
                FinalizarEntity();
            }

            updPnlFiltroBuscar_ubi_especial.Update();

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

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            EjecutarScript(btn_BuscarTramite, "hideResultado();");
            txtNroSolicitud.Text = "";
            txtNroEncomienda.Text = "";
            if (ddlTipoTramite.Items.Count >= 0)
                ddlTipoTramite.SelectedIndex = 0;

            if (ddlTipoExpediente.Items.Count >= 0)
                ddlTipoExpediente.SelectedIndex = 0;

            if (ddlSubTipoTramite.Items.Count >= 0)
                ddlSubTipoTramite.SelectedIndex = 0;

            //if (ddlEstado.Items.Count >= 0)
            //    ddlEstado.SelectedIndex = 0;

            if (ddlTarea.Items.Count >= 0)
                ddlTarea.SelectedIndex = 0;

            if (ddlTareaCerrada.Items.Count >= 0)
                ddlTareaCerrada.SelectedIndex = 0;

            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            txtFechaCierreDesde.Text = "";
            txtFechaCierreHasta.Text = "";

            txtUbiNroPartida.Text = "";
            ddlCalles.ClearSelection();
            txtUbiNroPuerta.Text = "";
            txtUF.Text = "";
            txtTorre.Text = "";
            txtLocal.Text = "";
            txtDpto.Text = "";
            txtNroPuertaDesde.Text = "";
            txtNroPuertaHasta.Text = "";
            rbtnNroPuertaAmbas.Checked = true;

            txtUbiSeccion.Text = "";
            txtUbiManzana.Text = "";
            txtUbiParcela.Text = "";

            if (ddlbiTipoUbicacion.Items.Count >= 0)
                ddlbiTipoUbicacion.SelectedIndex = 0;

            if (ddlUbiSubTipoUbicacion.Items.Count >= 0)
                ddlUbiSubTipoUbicacion.SelectedIndex = 0;

            txtRubroCodDesc.Text = "";
            txtTitApellido.Text = "";

            updPnlFiltroBuscar_tramite.Update();
            updPnlFiltroBuscar_ubi_partida.Update();
            updPnlFiltroBuscar_ubi_dom.Update();
            updPnlFiltroBuscar_ubi_smp.Update();
            updPnlFiltroBuscar_ubi_especial.Update();
            updPnlFiltroBuscar_rubros.Update();
            updPnlFiltroBuscar_titulares.Update();

            pnlResultadoBuscar.Visible = false;
            updPnlResultadoBuscar.Update();

        }

        private void CargarCalles()
        {
            var lstCalles = (from calle in db.Calles
                             select new
                             {
                                 calle.id_calle,
                                 calle.NombreOficial_calle
                             }).Distinct().OrderBy(x => x.NombreOficial_calle).ToList();

            ddlCalles.DataSource = lstCalles.GroupBy(x => x.NombreOficial_calle).Select(x => x.FirstOrDefault());
            ddlCalles.DataTextField = "NombreOficial_calle";
            ddlCalles.DataValueField = "id_calle";
            ddlCalles.DataBind();

            ddlCalles.Items.Insert(0, "");
        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {

                IniciarEntity();

                Validar();

                guardarFiltro();

                //grdTramites.DataBind();

                //FinalizarEntity();
                //EjecutarScript(btn_BuscarTramite, "showResultado();");

            }
            catch (Exception ex)
            {
                FinalizarEntity();
                //LogError.Write(ex, "error al buscar tramites buscar_tramite-btnBuscar_OnClick");
                Enviar_Mensaje(ex.Message, "");
            }

        }

        public List<clsItemBuscarTramite> GetTramites(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {

            totalRowCount = 0;

            List<clsItemBuscarTramite> lstResult = FiltrarTramites(startRowIndex, maximumRows, sortByExpression, out totalRowCount);

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
            //Validar_BuscarPorEstadoTramite();
            Validar_BuscarPorUbicacion();
            Validar_BuscarPorRubro();
            Validar_BuscarPorTitular();
            if (!hayFiltroPorTramite && !hayFiltroPorUbicacion && !hayFiltroPorRubro && !hayFiltroPorTitular)// && !hayFiltroPorEstadoTramite)
            {
                throw new Exception("Debe ingresar algún filtro de búsqueda.");
            }
        }
        //private bool hayFiltroPorEstadoTramite = false;

        //private void Validar_BuscarPorEstadoTramite()
        //{
        //    this.hayFiltroPorEstadoTramite = false;

        //    //var idAux = 0;
        //    //int.TryParse(ddlEstado.SelectedItem.Value, out idAux);

        //    this.hayFiltroPorEstadoTramite = idAux > 0;
        //}

        private int id_solicitud = 0;
        private int id_encomida = 0;
        private int id_tipo_tramite = 0;
        private int id_tipo_expediente = 0;
        private int id_sub_tipo_tramite = 0;
        private int id_tarea = 0;
        private int id_tarea_cerrada = 0;
        private string nroExp = "";
        private string estados = string.Empty;



        private DateTime? fechaDesde;
        private DateTime? fechaHasta;
        private DateTime? fechaCierreDesde;
        private DateTime? fechaCierreHasta;
        private Decimal? SuperficieDesde;
        private Decimal? SuperficieHasta;

        private bool hayFiltroPorTramite = false;
        private void Validar_BuscarPorTramite()
        {
            this.hayFiltroPorTramite = false;

            DateTime fechaDesdeAux;
            DateTime fechaHastaAux;
            DateTime fechaCierreDesdeAux;
            DateTime fechaCierreHastaAux;
            int idAux = 0;

            this.id_solicitud = 0;
            this.id_encomida = 0;
            this.id_tipo_tramite = 0;
            this.id_tipo_expediente = 0;
            this.id_sub_tipo_tramite = 0;
            this.id_tarea = 0;
            this.nroExp = "";

            this.fechaDesde = null;
            this.fechaHasta = null;
            this.fechaCierreDesde = null;
            this.fechaCierreHasta = null;

            idAux = 0;
            int.TryParse(txtNroSolicitud.Text, out idAux);
            this.id_solicitud = idAux;

            idAux = 0;
            int.TryParse(txtNroEncomienda.Text, out idAux);
            this.id_encomida = idAux;

            idAux = 0;
            int.TryParse(ddlTipoTramite.SelectedItem.Value, out idAux);
            this.id_tipo_tramite = idAux;

            idAux = 0;
            int.TryParse(ddlTipoExpediente.SelectedItem.Value, out idAux);
            this.id_tipo_expediente = idAux;

            idAux = 0;
            int.TryParse(ddlSubTipoTramite.SelectedItem.Value, out idAux);
            this.id_sub_tipo_tramite = idAux;

            idAux = 0;
            int.TryParse(ddlTarea.SelectedItem.Value, out idAux);
            this.id_tarea = idAux;

            idAux = 0;
            int.TryParse(ddlTareaCerrada.SelectedItem.Value, out idAux);
            this.id_tarea_cerrada = idAux;

            if (!string.IsNullOrEmpty(txtFechaDesde.Text))
            {
                if (!DateTime.TryParse(txtFechaDesde.Text, out fechaDesdeAux))
                    throw new Exception("Fecha tarea desde inválida.");

                this.fechaDesde = fechaDesdeAux;
            }

            if (!string.IsNullOrEmpty(txtFechaHasta.Text))
            {
                if (!DateTime.TryParse(txtFechaHasta.Text, out fechaHastaAux))
                    throw new Exception("Fecha tarea hasta inválida.");

                this.fechaHasta = fechaHastaAux;
            }

            if (!string.IsNullOrEmpty(txtFechaCierreDesde.Text))
            {
                if (!DateTime.TryParse(txtFechaCierreDesde.Text, out fechaCierreDesdeAux))
                    throw new Exception("Fecha tarea desde inválida.");

                this.fechaCierreDesde = fechaCierreDesdeAux;
            }

            if (!string.IsNullOrEmpty(txtFechaCierreHasta.Text))
            {
                if (!DateTime.TryParse(txtFechaCierreHasta.Text, out fechaCierreHastaAux))
                    throw new Exception("Fecha tarea hasta inválida.");

                this.fechaCierreHasta = fechaCierreHastaAux;
            }

            if (!string.IsNullOrEmpty(txtNroExp.Text))
            {
                this.nroExp = txtNroExp.Text;
            }

            if (this.fechaDesde.HasValue && this.fechaHasta.HasValue && this.fechaDesde > this.fechaHasta)
                throw new Exception("Fecha desde superior a fecha hasta.");

            if (this.fechaCierreDesde.HasValue && this.fechaCierreHasta.HasValue && this.fechaCierreDesde > this.fechaCierreHasta)
                throw new Exception("Fecha de Cierre desde superior a fecha Cierre hasta.");
            if (this.id_solicitud > 0 || this.id_encomida > 0 ||
                this.id_tipo_tramite > 0 || this.id_tipo_expediente > 0 ||
                this.id_sub_tipo_tramite > 0 || this.id_tarea > 0 ||
                this.id_tarea_cerrada > 0 || this.fechaDesde.HasValue
                || this.fechaHasta.HasValue || this.fechaCierreDesde.HasValue
                || this.fechaCierreHasta.HasValue || this.nroExp != "")
                this.hayFiltroPorTramite = true;

            //this.estados = hid_estados_selected.Value;
        }

        private int nro_partida_matriz = 0;
        private int nro_partida_horiz = 0;
        private int id_calle = 0;
        private int nro_calle = 0;

        private string uf = "";
        private string dpto = "";
        private string local = "";
        private string torre = "";

        private int nro_calle_desde = 0;
        private int nro_calle_hasta = 0;
        private bool nro_calle_par = false;
        private bool nro_calle_impar = false;
        private bool nro_calle_ambas = false;

        private int seccion = 0;
        private string manzana = "";
        private string parcela = "";
        private int id_tipo_ubicacion = -1;
        private int id_sub_tipo_ubicacion = -1;


        private bool hayFiltroPorUbicacion = false;
        private void Validar_BuscarPorUbicacion()
        {
            this.hayFiltroPorUbicacion = false;

            int idAux = 0;

            this.nro_partida_matriz = 0;
            this.nro_partida_horiz = 0;

            this.id_calle = 0;
            this.nro_calle = 0;

            this.seccion = 0;
            this.manzana = "";
            this.parcela = "";

            this.id_tipo_ubicacion = -1;
            this.id_sub_tipo_ubicacion = -1;

            //filtro por partida
            if (!string.IsNullOrEmpty(txtUbiNroPartida.Text))
            {
                idAux = 0;
                if (rbtnUbiPartidaMatriz.Checked)
                {
                    int.TryParse(txtUbiNroPartida.Text, out idAux);
                    this.nro_partida_matriz = idAux;
                }
                else if (rbtnUbiPartidaHoriz.Checked)
                {
                    int.TryParse(txtUbiNroPartida.Text, out idAux);
                    this.nro_partida_horiz = idAux;
                }
                else
                {
                    throw new Exception("Debe indicar si nùmero ingresado corresponde a partida matriz o a partida horizontal.");
                }

            }

            //filtro por domicilio
            if (!string.IsNullOrEmpty(txtUbiNroPuerta.Text) && ddlCalles.SelectedValue == "")
            {
                throw new Exception("Cuando especifica el número de puerta debe ingresar la calle.");
            }

            idAux = 0;
            int.TryParse(ddlCalles.SelectedValue, out idAux);
            this.id_calle = idAux;

            idAux = 0;
            int.TryParse(txtUbiNroPuerta.Text.Trim(), out idAux);
            this.nro_calle = idAux;

            //Unidad Funcional
            this.uf = txtUF.Text;
            //Torre
            this.torre = txtTorre.Text;
            //Departameto
            this.dpto = txtDpto.Text;
            //Local
            this.local = txtLocal.Text;
            //Nro Calle desde
            int.TryParse(txtNroPuertaDesde.Text.Trim(), out idAux);
            this.nro_calle_desde = idAux;
            //Nro Calle hasta
            int.TryParse(txtNroPuertaHasta.Text.Trim(), out idAux);
            this.nro_calle_hasta = idAux;

            //filtro por smp
            idAux = 0;
            int.TryParse(txtUbiSeccion.Text, out idAux);
            this.seccion = idAux;

            this.manzana = txtUbiManzana.Text.Trim();

            this.parcela = txtUbiParcela.Text.Trim();

            //filtro por tipo subtipo
            idAux = -1;
            int.TryParse(ddlbiTipoUbicacion.SelectedItem.Value, out idAux);
            this.id_tipo_ubicacion = idAux;

            idAux = -1;
            int.TryParse(ddlUbiSubTipoUbicacion.SelectedItem.Value, out idAux);
            this.id_sub_tipo_ubicacion = idAux;

            if (this.nro_partida_matriz > 0 || this.nro_partida_horiz > 0 ||
                this.id_calle > 0 || this.nro_calle > 0 ||
                this.seccion > 0 || !string.IsNullOrEmpty(this.manzana) || !string.IsNullOrEmpty(this.parcela) ||
                this.id_tipo_ubicacion > -1 || this.id_sub_tipo_ubicacion > -1 ||
                !string.IsNullOrEmpty(this.uf) || !string.IsNullOrEmpty(this.dpto) ||
                !string.IsNullOrEmpty(this.torre) || !string.IsNullOrEmpty(this.local) ||
                (this.nro_calle_desde > 0 && this.nro_calle_hasta > 0))
                this.hayFiltroPorUbicacion = true;

        }

        private string rubro_desc = "";
        private bool hayFiltroPorRubro = false;
        private void Validar_BuscarPorRubro()
        {
            this.hayFiltroPorRubro = false;

            this.rubro_desc = txtRubroCodDesc.Text.Trim();

            if (!string.IsNullOrEmpty(this.rubro_desc) && this.rubro_desc.Length < 3)
            {
                throw new Exception("Debe ingresar almenos 3 letras.");
            }


            if (!string.IsNullOrEmpty(this.rubro_desc))
                this.hayFiltroPorRubro = true;

        }

        private string tit_razon = "";
        private bool hayFiltroPorTitular = false;
        private void Validar_BuscarPorTitular()
        {
            hayFiltroPorTitular = false;

            this.tit_razon = txtTitApellido.Text.Trim();

            if (!string.IsNullOrEmpty(this.tit_razon) && this.tit_razon.Length < 3)
            {
                throw new Exception("Debe ingresar almenos 3 letras.");
            }


            if (!string.IsNullOrEmpty(this.tit_razon))
                this.hayFiltroPorTitular = true;

        }

        #endregion

        private void guardarFiltro()
        {
            FiltrosBusqueda filtros = new FiltrosBusqueda()
            {
                id_solicitud = txtNroSolicitud.Text,
                nroExp = txtNroExp.Text,
                id_encomida = txtNroEncomienda.Text,
                id_tipo_tramite = ddlTipoTramite.SelectedIndex.ToString(),
                id_tipo_expediente = ddlTipoExpediente.SelectedIndex.ToString(),
                id_sub_tipo_tramite = ddlSubTipoTramite.SelectedIndex.ToString(),
                id_tarea = ddlTarea.SelectedIndex.ToString(),
                //id_estado = hid_estados_selected.Value,
                id_tarea_cerrada = ddlTareaCerrada.SelectedIndex.ToString(),
                fechaDesde = txtFechaDesde.Text,
                fechaHasta = txtFechaHasta.Text,
                rbtnUbiPartidaMatriz = rbtnUbiPartidaMatriz.Checked,
                rbtnUbiPartidaHoriz = rbtnUbiPartidaHoriz.Checked,
                fechaCierreDesde = txtFechaCierreDesde.Text,
                fechaCierreHasta = txtFechaCierreHasta.Text,
                nro_partida_matriz = txtUbiNroPartida.Text,
                id_calle = ddlCalles.SelectedValue.ToString(),
                nro_calle = txtUbiNroPuerta.Text,
                uf = txtUF.Text,
                torre = txtTorre.Text,
                dpto = txtDpto.Text,
                local = txtLocal.Text,
                nro_puerta_desde = txtNroPuertaDesde.Text,
                nro_puerta_hasta = txtNroPuertaHasta.Text,
                rbtn_nro_puerta_par = rbtnNroPuertaPar.Checked,
                rbtn_nro_puerta_impar = rbtnNroPuertaImpar.Checked,
                rbtn_nro_puerta_ambas = rbtnNroPuertaAmbas.Checked,
                seccion = txtUbiSeccion.Text,
                manzana = txtUbiManzana.Text,
                parcela = txtUbiParcela.Text,
                id_tipo_ubicacion = ddlbiTipoUbicacion.SelectedIndex.ToString(),
                id_sub_tipo_ubicacion = ddlUbiSubTipoUbicacion.SelectedIndex.ToString(),
                rubro_desc = txtRubroCodDesc.Text,
                tit_razon = txtTitApellido.Text
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
                filtrosInsertar.botonAccion = hdUltBtn.Value;

                DGHP_Entities db = new DGHP_Entities();
                db.SGI_FiltrosBusqueda.Add(filtrosInsertar);
                db.SaveChanges();

                Response.Redirect(string.Format("~/GestionTramite/BusquedaTramite" + "/" + "{0}", guidJson), false);
            }

        }

        private bool recuperarFiltro(string idFiltro)  
        {
            DGHP_Entities db = new DGHP_Entities();
            var elements = (from filtrosBase in db.SGI_FiltrosBusqueda
                            where idFiltro == filtrosBase.Id_Busqueda.ToString()
                            select filtrosBase).FirstOrDefault();

            if (elements == null)
                return false;
            string jsonInput = elements.Filtros;

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            FiltrosBusqueda filtros = jsonSerializer.Deserialize<FiltrosBusqueda>(jsonInput);
            hdUltBtn.Value = elements.botonAccion;

            txtNroSolicitud.Text = filtros.id_solicitud.ToString();
            txtFechaDesde.Text = filtros.fechaDesde;
            txtFechaCierreDesde.Text = filtros.fechaCierreDesde;
            ddlTipoTramite.SelectedIndex = Convert.ToInt32(filtros.id_tipo_tramite);

            txtNroExp.Text = filtros.nroExp;
            txtNroEncomienda.Text = filtros.id_encomida.ToString();
            txtFechaHasta.Text = filtros.fechaHasta;
            txtFechaCierreHasta.Text = filtros.fechaCierreHasta;

            IniciarEntity();
            int idTipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);
            CargarCombo_TipoExpediente(idTipoTramite);
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();

            ddlTipoExpediente.SelectedIndex = Convert.ToInt32(filtros.id_tipo_expediente);
            IniciarEntity();
            int idTipoExp = Convert.ToInt32(ddlTipoExpediente.SelectedValue);
            CargarCombo_subtipoTramite(idTipoExp);
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();

            ddlSubTipoTramite.SelectedIndex = Convert.ToInt32(filtros.id_sub_tipo_tramite);
            IniciarEntity();
            int idSubTipo = Convert.ToInt32(ddlSubTipoTramite.SelectedValue);
            CargarCombo_tareas();
            //CargarCombo_tareas(idTipoTramite, idTipoExp, idSubTipo);
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();

            //hid_estados_selected.Value = filtros.id_estado;

            ddlTarea.SelectedIndex = Convert.ToInt32(filtros.id_tarea);
            ddlTareaCerrada.SelectedIndex = Convert.ToInt32(filtros.id_tarea_cerrada);

            if (String.IsNullOrWhiteSpace(filtros.id_calle))
            {

                ddlCalles.SelectedValue = "Todos";
            }
            else
            {
                ddlCalles.SelectedValue = filtros.id_calle.ToString();
            }
            txtUbiNroPartida.Text = filtros.nro_partida_matriz;
            txtUbiNroPuerta.Text = filtros.nro_calle;
            txtUF.Text = filtros.uf;
            txtDpto.Text = filtros.dpto;
            txtLocal.Text = filtros.local;
            txtTorre.Text = filtros.torre;
            txtNroPuertaDesde.Text = filtros.nro_puerta_desde;
            txtNroPuertaHasta.Text = filtros.nro_puerta_hasta;
            rbtnNroPuertaPar.Checked = filtros.rbtn_nro_puerta_par;
            rbtnNroPuertaImpar.Checked = filtros.rbtn_nro_puerta_impar;
            rbtnNroPuertaAmbas.Checked = filtros.rbtn_nro_puerta_ambas;
            txtUbiSeccion.Text = filtros.seccion;
            txtUbiManzana.Text = filtros.manzana;
            txtUbiParcela.Text = filtros.parcela;

            ddlbiTipoUbicacion.SelectedIndex = Convert.ToInt32(filtros.id_tipo_ubicacion);
            IniciarEntity();
            int id_tipoubicacion = int.Parse(ddlbiTipoUbicacion.SelectedValue);
            CargarCombo_subTipoUbicacion(id_tipoubicacion);
            FinalizarEntity();
            updPnlFiltroBuscar_ubi_especial.Update();
            txtRubroCodDesc.Text = filtros.rubro_desc;
            txtTitApellido.Text = filtros.tit_razon;
            ddlUbiSubTipoUbicacion.SelectedIndex = Convert.ToInt32(filtros.id_sub_tipo_ubicacion);
            rbtnUbiPartidaMatriz.Checked = filtros.rbtnUbiPartidaMatriz;
            rbtnUbiPartidaHoriz.Checked = filtros.rbtnUbiPartidaHoriz;
            txtRubroCodDesc.Text = filtros.rubro_desc;
            txtTitApellido.Text = filtros.tit_razon;
            if (String.IsNullOrWhiteSpace(filtros.id_solicitud))
            {
                this.id_solicitud = 0;
            }
            else
            {
                this.id_solicitud = Convert.ToInt32(filtros.id_solicitud);
            }
            if (String.IsNullOrWhiteSpace(filtros.nroExp))
            {
                this.nroExp = "";
            }
            else
            {
                this.nroExp = filtros.nroExp;
            }

            if (String.IsNullOrWhiteSpace(filtros.id_encomida))
            {
                this.id_encomida = 0;
            }
            else
            {
                this.id_encomida = Convert.ToInt32(filtros.id_encomida);
            }
            if (String.IsNullOrWhiteSpace(filtros.id_tipo_tramite))
            {
                this.id_tipo_tramite = 0;
            }
            else
            {
                this.id_tipo_tramite = Convert.ToInt32(filtros.id_tipo_tramite);
            }
            if (String.IsNullOrWhiteSpace(filtros.id_tipo_expediente))
            {
                this.id_tipo_expediente = 0;
            }
            else
            {
                this.id_tipo_expediente = Convert.ToInt32(filtros.id_tipo_expediente);
            }
            if (String.IsNullOrWhiteSpace(filtros.id_sub_tipo_tramite))
            {
                this.id_sub_tipo_tramite = 0;
            }
            else
            {
                this.id_sub_tipo_tramite = Convert.ToInt32(filtros.id_sub_tipo_tramite);
            }
            if (String.IsNullOrWhiteSpace(filtros.id_tarea))
            {
                this.id_tarea = 0;
            }
            else
            {
                this.id_tarea = Convert.ToInt32(filtros.id_tarea);
            }
            if (String.IsNullOrWhiteSpace(filtros.id_tarea_cerrada))
            {
                this.id_tarea_cerrada = 0;
            }
            else
            {
                this.id_tarea_cerrada = Convert.ToInt32(filtros.id_tarea_cerrada);
            }

            if (string.IsNullOrEmpty(filtros.fechaDesde))
            {
                this.fechaDesde = null;
            }
            else
            {
                this.fechaDesde = Convert.ToDateTime(filtros.fechaDesde);
            }
            if (string.IsNullOrEmpty(filtros.fechaHasta))
            {
                this.fechaHasta = null;
            }
            else
            {
                this.fechaHasta = Convert.ToDateTime(filtros.fechaHasta);
            }


            if (string.IsNullOrEmpty(filtros.fechaCierreDesde))
            {
                this.fechaCierreDesde = null;
            }
            else
            {
                this.fechaCierreDesde = Convert.ToDateTime(filtros.fechaCierreDesde);
            }
            if (string.IsNullOrEmpty(filtros.fechaCierreHasta))
            {
                this.fechaCierreHasta = null;
            }
            else
            {
                this.fechaCierreHasta = Convert.ToDateTime(filtros.fechaCierreHasta);
            }
            if (rbtnUbiPartidaHoriz.Checked)
            {

                if (string.IsNullOrWhiteSpace(txtUbiNroPartida.Text))
                {
                    this.nro_partida_matriz = 0;
                }
                else
                {
                    this.nro_partida_matriz = Convert.ToInt32(txtUbiNroPartida.Text);
                }

            }
            else
            {

                if (string.IsNullOrWhiteSpace(txtUbiNroPartida.Text))
                {
                    this.nro_partida_horiz = 0;
                }
                else
                {
                    this.nro_partida_horiz = Convert.ToInt32(txtUbiNroPartida.Text);
                }


            }
            if (string.IsNullOrWhiteSpace(filtros.id_calle))
            {
                this.id_calle = 0;
            }
            else
            {
                this.id_calle = Convert.ToInt32(filtros.id_calle);
            }
            if (string.IsNullOrWhiteSpace(filtros.nro_calle))
            {
                this.nro_calle = 0;
            }
            else
            {
                this.nro_calle = Convert.ToInt32(filtros.nro_calle);
            }
            if (string.IsNullOrWhiteSpace(filtros.nro_puerta_desde))
            {
                this.nro_calle_desde = 0;
            }
            else
            {
                this.nro_calle_desde = Convert.ToInt32(filtros.nro_puerta_desde);
            }
            if (string.IsNullOrWhiteSpace(filtros.nro_puerta_hasta))
            {
                this.nro_calle_hasta = 0;
            }
            else
            {
                this.nro_calle_hasta = Convert.ToInt32(filtros.nro_puerta_hasta);
            }
            if (string.IsNullOrWhiteSpace(filtros.uf))
            {
                this.uf = filtros.uf;
            }
            if (string.IsNullOrWhiteSpace(filtros.seccion))
            {
                this.seccion = 0;
            }
            else
            {
                this.seccion = Convert.ToInt32(filtros.seccion);
            }
            this.manzana = filtros.manzana;
            this.parcela = filtros.parcela;
            if (string.IsNullOrWhiteSpace(filtros.id_tipo_ubicacion))
            {
                this.id_tipo_ubicacion = 0;
            }
            else
            {
                this.id_tipo_ubicacion = Convert.ToInt32(filtros.id_tipo_ubicacion);
            }
            if (string.IsNullOrWhiteSpace(filtros.id_sub_tipo_ubicacion))
            {
                this.id_sub_tipo_ubicacion = 0;
            }
            else
            {
                this.id_sub_tipo_ubicacion = Convert.ToInt32(filtros.id_sub_tipo_ubicacion);
            }
            this.rubro_desc = filtros.rubro_desc;
            this.tit_razon = filtros.tit_razon;

            this.estados = filtros.id_estado;

            return true;
        }

        protected void grdTramites_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //this.db
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    IniciarEntity();              

            //    clsItemBuscarTramite result = (clsItemBuscarTramite)e.Row.DataItem;
            //    LinkButton lnkTareasSolicitud = (LinkButton)e.Row.FindControl("lnkTareasSolicitud");
            //    GridView grdTareas = (GridView)e.Row.FindControl("grdTareas");
            //    int id_solicitud = (int)result.id_solicitud;

               
            //    var elements = (from tt in db.SGI_Tramites_Tareas
            //                    join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
            //                    join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea                                
            //                    where tt_hab.id_solicitud == result.id_solicitud
            //                    orderby tt.id_tramitetarea descending
            //                    select new
            //                    {
            //                        tt.id_tramitetarea,
            //                        tt.id_tarea,
            //                        tt.FechaInicio_tramitetarea,
            //                        tt.ENG_Tareas.nombre_tarea,
            //                        tt.ENG_Tareas.formulario_tarea,
            //                        url_tareaTramite = (tt.ENG_Tareas.formulario_tarea != null ? "~/GestionTramite/Tareas/" + tt.ENG_Tareas.formulario_tarea + "?id=" + tt.id_tramitetarea : "")
            //                    }).ToList();

            //    if (elements.Count() == 0)
            //        elements = (from tt in db.SGI_Tramites_Tareas
            //                    join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
            //                    join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
            //                    join cir in db.ENG_Circuitos on t.id_circuito equals cir.id_circuito
            //                    where tt_tr.id_solicitud == result.id_solicitud
            //                    orderby tt.id_tramitetarea descending
            //                    select new
            //                    {
            //                        id_tramitetarea = tt.id_tramitetarea,
            //                        tt.id_tarea,
            //                        FechaInicio_tramitetarea = tt.FechaInicio_tramitetarea,
            //                        t.nombre_tarea,
            //                        t.formulario_tarea,
            //                        url_tareaTramite = (t.formulario_tarea != null ? "~/GestionTramite/Tareas/" + t.formulario_tarea + "?id=" + tt.id_tramitetarea : "")
            //                    }).ToList();
            //    if (elements.Count() == 0)
            //        elements = (from tt in db.SGI_Tramites_Tareas
            //                    join tt_cp in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals tt_cp.id_tramitetarea
            //                    join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
            //                    join cir in db.ENG_Circuitos on t.id_circuito equals cir.id_circuito
            //                    where tt_cp.id_cpadron == result.id_solicitud
            //                    orderby tt.id_tramitetarea descending
            //                    select new
            //                    {
            //                        id_tramitetarea = tt.id_tramitetarea,
            //                        tt.id_tarea,
            //                        FechaInicio_tramitetarea = tt.FechaInicio_tramitetarea,
            //                        t.nombre_tarea,
            //                        t.formulario_tarea,
            //                        url_tareaTramite = (t.formulario_tarea != null ? "~/GestionTramite/Tareas/" + t.formulario_tarea + "?id=" + tt.id_tramitetarea : "")
            //                    }).ToList();

            //    grdTareas.DataSource = elements;
            //    grdTareas.DataBind();
            //}

        }

        protected void ddlTipoTramite_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniciarEntity();
            int idTipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);
            CargarCombo_TipoExpediente(idTipoTramite);
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();
        }

        protected void ddlTipoExpediente_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniciarEntity();
            int idTipoExp = Convert.ToInt32(ddlTipoExpediente.SelectedValue);
            CargarCombo_subtipoTramite(idTipoExp);
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();
        }

        protected void ddlSubTipoTramite_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniciarEntity();
            int idTipoTramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);
            int idTipoExp = Convert.ToInt32(ddlTipoExpediente.SelectedValue);
            int idSubTipo = Convert.ToInt32(ddlSubTipoTramite.SelectedValue);
            CargarCombo_tareas();
            //CargarCombo_tareas(idTipoTramite, idTipoExp, idSubTipo);
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();
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
        private List<clsItemBuscarTramite> FiltrarTramites(int startRowIndex, int maximumRows, string sortByExpression, out int totalRowCount)
        {
            DGHP_Entities db = new DGHP_Entities();
            if (!String.IsNullOrWhiteSpace(codigoGuid))
            {

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

                if (String.IsNullOrWhiteSpace(txtNroEncomienda.Text))
                {
                    this.id_encomida = 0;
                }
                else
                {
                    this.id_encomida = Convert.ToInt32(txtNroEncomienda.Text);
                }
                if (String.IsNullOrWhiteSpace(ddlTipoTramite.SelectedValue))
                {
                    this.id_tipo_tramite = 0;
                }
                else
                {
                    this.id_tipo_tramite = Convert.ToInt32(ddlTipoTramite.SelectedValue);
                }
                if (String.IsNullOrWhiteSpace(ddlTipoExpediente.SelectedValue))
                {
                    this.id_tipo_expediente = 0;
                }
                else
                {
                    this.id_tipo_expediente = Convert.ToInt32(ddlTipoExpediente.SelectedValue);
                }
                if (String.IsNullOrWhiteSpace(ddlSubTipoTramite.SelectedValue))
                {
                    this.id_sub_tipo_tramite = 0;
                }
                else
                {
                    this.id_sub_tipo_tramite = Convert.ToInt32(ddlSubTipoTramite.SelectedValue);
                }
                if (String.IsNullOrWhiteSpace(ddlTarea.SelectedValue))
                {
                    this.id_tarea = 0;
                }
                else
                {
                    this.id_tarea = Convert.ToInt32(ddlTarea.SelectedValue);
                }
                if (String.IsNullOrWhiteSpace(ddlTareaCerrada.SelectedValue))
                {
                    this.id_tarea_cerrada = 0;
                }
                else
                {
                    this.id_tarea_cerrada = Convert.ToInt32(ddlTareaCerrada.SelectedValue);
                }


                //this.estados = hid_estados_selected.Value;

                if (string.IsNullOrEmpty(txtFechaDesde.Text))
                {
                    this.fechaDesde = null;
                }
                else
                {
                    this.fechaDesde = Convert.ToDateTime(txtFechaDesde.Text);
                }
                if (string.IsNullOrEmpty(txtFechaHasta.Text))
                {
                    this.fechaHasta = null;
                }
                else
                {
                    this.fechaHasta = Convert.ToDateTime(txtFechaHasta.Text);
                }


                if (string.IsNullOrEmpty(txtFechaCierreDesde.Text))
                {
                    this.fechaCierreDesde = null;
                }
                else
                {
                    this.fechaCierreDesde = Convert.ToDateTime(txtFechaCierreDesde.Text);
                }
                if (string.IsNullOrEmpty(txtFechaCierreHasta.Text))
                {
                    this.fechaCierreHasta = null;
                }
                else
                {
                    this.fechaCierreHasta = Convert.ToDateTime(txtFechaCierreHasta.Text);
                }
                if (rbtnUbiPartidaHoriz.Checked)
                {

                    if (string.IsNullOrWhiteSpace(txtUbiNroPartida.Text))
                    {
                        this.nro_partida_matriz = 0;
                    }
                    else
                    {
                        this.nro_partida_matriz = Convert.ToInt32(txtUbiNroPartida.Text);
                    }

                }
                else
                {

                    if (string.IsNullOrWhiteSpace(txtUbiNroPartida.Text))
                    {
                        this.nro_partida_horiz = 0;
                    }
                    else
                    {
                        this.nro_partida_horiz = Convert.ToInt32(txtUbiNroPartida.Text);
                    }


                }

                if (string.IsNullOrWhiteSpace(ddlCalles.SelectedValue))
                {
                    this.id_calle = 0;
                }
                else
                {
                    this.id_calle = Convert.ToInt32(ddlCalles.SelectedValue);
                }
                if (string.IsNullOrWhiteSpace(txtUbiNroPuerta.Text))
                {
                    this.nro_calle = 0;
                }
                else
                {
                    this.nro_calle = Convert.ToInt32(txtUbiNroPuerta.Text);
                }
                if (!string.IsNullOrWhiteSpace(txtUF.Text))
                {
                    this.uf = txtUF.Text;
                }
                if (!string.IsNullOrWhiteSpace(txtTorre.Text))
                {
                    this.torre = txtTorre.Text;
                }
                if (!string.IsNullOrWhiteSpace(txtLocal.Text))
                {
                    this.local = txtLocal.Text;
                }
                if (!string.IsNullOrWhiteSpace(txtDpto.Text))
                {
                    this.dpto = txtDpto.Text;
                }
                if (string.IsNullOrWhiteSpace(txtNroPuertaDesde.Text))
                {
                    this.nro_calle_desde = 0;
                }
                else
                {
                    this.nro_calle_desde = Convert.ToInt32(txtNroPuertaDesde.Text);
                }
                if (string.IsNullOrWhiteSpace(txtNroPuertaHasta.Text))
                {
                    this.nro_calle_hasta = 0;
                }
                else
                {
                    this.nro_calle_hasta = Convert.ToInt32(txtNroPuertaHasta.Text);
                }
                this.nro_calle_par = rbtnNroPuertaPar.Checked;
                this.nro_calle_impar = rbtnNroPuertaImpar.Checked;
                this.nro_calle_ambas = rbtnNroPuertaAmbas.Checked;
                //agregar el this puertas ambas checked
                if (string.IsNullOrWhiteSpace(txtUbiSeccion.Text))
                {
                    this.seccion = 0;
                }
                else
                {
                    this.seccion = Convert.ToInt32(txtUbiSeccion.Text);
                }
                this.manzana = txtUbiManzana.Text;
                this.parcela = txtUbiParcela.Text;
                if (string.IsNullOrWhiteSpace(ddlbiTipoUbicacion.SelectedValue))
                {
                    this.id_tipo_ubicacion = 0;
                }
                else
                {
                    this.id_tipo_ubicacion = Convert.ToInt32(ddlbiTipoUbicacion.SelectedValue);
                }
                if (string.IsNullOrWhiteSpace(ddlUbiSubTipoUbicacion.SelectedValue))
                {
                    this.id_sub_tipo_ubicacion = 0;
                }
                else
                {
                    this.id_sub_tipo_ubicacion = Convert.ToInt32(ddlUbiSubTipoUbicacion.SelectedValue);
                }
                this.rubro_desc = txtRubroCodDesc.Text;
                this.tit_razon = txtTitApellido.Text;
            }
            else
            {
                totalRowCount = 0;
                return new List<clsItemBuscarTramite>();

            }

            List<clsItemBuscarTramite> resultados = new List<clsItemBuscarTramite>();
            IQueryable<clsItemBuscarTramite> qFinal = null;
            IQueryable<clsItemBuscarTramite> qSOL = null;
            IQueryable<clsItemBuscarTramite> qCP = null;
            IQueryable<clsItemBuscarTramite> qTR = null;

            db.Database.CommandTimeout = 300;

            Guid userid = Functions.GetUserId();

            // Consulta de Solicitudes
            #region "Consulta Solicitudes"
            IQueryable<cls_ultima_tarea> lst_Ultima_tarea = (from tt in db.SGI_Tramites_Tareas_HAB
                                                             group tt by tt.id_solicitud into g
                                                             select new cls_ultima_tarea
                                                             {
                                                                 id_solicitud = g.Key,
                                                                 id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                                             });
            List<int> tareasFin = new List<int>();
            tareasFin.Add((int)Constants.ENG_Tareas.SCP_Fin_Tramite);
            tareasFin.Add((int)Constants.ENG_Tareas.SSP_Fin_Tramite);
            tareasFin.Add((int)Constants.ENG_Tareas.SCP_Fin_Tramite_Nuevo);
            tareasFin.Add((int)Constants.ENG_Tareas.SSP_Fin_Tramite_Nuevo);
            tareasFin.Add((int)Constants.ENG_Tareas.ESP_Fin_Tramite);
            tareasFin.Add((int)Constants.ENG_Tareas.ESPAR_Fin_Tramite);
            tareasFin.Add((int)Constants.ENG_Tareas.ESP_Fin_Tramite_Nuevo);
            tareasFin.Add((int)Constants.ENG_Tareas.ESPAR_Fin_Tramite_Nuevo);


            qSOL = (from sol in db.SSIT_Solicitudes
                    join ult_tar in lst_Ultima_tarea on sol.id_solicitud equals ult_tar.id_solicitud into pleft_ult_tar
                    from ult_tar in pleft_ult_tar.DefaultIfEmpty()
                    join tt in db.SGI_Tramites_Tareas_HAB on ult_tar.id_tramitetarea equals tt.id_tramitetarea into pleft_tt
                    from tt in pleft_tt.DefaultIfEmpty()
                    join tar in db.ENG_Tareas on tt.SGI_Tramites_Tareas.id_tarea equals tar.id_tarea into pleft_tar
                    from tar in pleft_tar.DefaultIfEmpty()
                    join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito into pleft_tarea
                    from cir in pleft_tarea.DefaultIfEmpty()
                    where (
                        (db.SGI_Tramites_Tareas_HAB.Where(x => x.id_solicitud == sol.id_solicitud && x.SGI_Tramites_Tareas.ENG_Tareas.formulario_tarea != null
                        && !tareasFin.Contains(x.SGI_Tramites_Tareas.ENG_Tareas.id_tarea)).Count() > 0)
                        || sol.id_tipotramite == (int)Constants.TipoDeTramite.Permiso
                        )
                    && sol.id_estado != (int)Constants.Solicitud_Estados.Anulado
                    select new clsItemBuscarTramite
                    {
                        cod_grupotramite = Constants.GruposDeTramite.HAB.ToString(),
                        id_tramitetarea = tt.id_tramitetarea,
                        id_solicitud = sol.id_solicitud,
                        FechaInicio_tarea = tt.SGI_Tramites_Tareas.FechaInicio_tramitetarea,
                        FechaCierre_tarea = tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea,
                        FechaAsignacion_tarea = tt.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea,
                        direccion = "",
                        id_tarea = tt.SGI_Tramites_Tareas.id_tarea,
                        nombre_tarea = tar.nombre_tarea,
                        asignable_tarea = (tar != null ? tar.Asignable_tarea : false),
                        formulario_tarea = tar.formulario_tarea,
                        desc_circuito = tar.ENG_Circuitos.nombre_grupo,
                        superficie_total = 0,
                        url_visorTramite = "~/GestionTramite/VisorTramite.aspx?id={0}",
                        url_tareaTramite = "~/GestionTramite/Tareas/{0}?id={1}",
                        nro_Expediente = sol.NroExpedienteSade,
                        estado = sol.TipoEstadoSolicitud.Descripcion,
                        id_estado = sol.id_estado,
                    }).Distinct();

            // busqueda por datos del trámite
            if (this.id_solicitud > 0)
                qSOL = qSOL.Where(x => x.id_solicitud == this.id_solicitud);

            if (!string.IsNullOrEmpty(this.nroExp))
                qSOL = qSOL.Where(x => x.nro_Expediente.Contains(this.nroExp.Trim()));

            //búsqueda por encomienda
            if (this.id_encomida > 0)
                qSOL = (from res in qSOL
                        join encsol in db.Encomienda_SSIT_Solicitudes on res.id_solicitud equals encsol.id_solicitud
                        join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                        where enc.id_encomienda == id_encomida
                        select res);

            //búsqueda por tipo de tramite
            if (this.id_tipo_tramite > 0)
            {
                qSOL = (from res in qSOL
                        join sol in db.SSIT_Solicitudes on res.id_solicitud equals sol.id_solicitud
                        where sol.id_tipotramite == this.id_tipo_tramite
                        select res);
            }

            //búsqueda por tipo de expediente
            if (this.id_tipo_expediente > 0)
                qSOL = (from res in qSOL
                        join sol in db.SSIT_Solicitudes on res.id_solicitud equals sol.id_solicitud
                        where sol.id_tipoexpediente == this.id_tipo_expediente
                        select res);

            //búsqueda por sub tipo de tramite
            if (this.id_sub_tipo_tramite > 0)
                qSOL = (from res in qSOL
                        join sol in db.SSIT_Solicitudes on res.id_solicitud equals sol.id_solicitud
                        where sol.id_subtipoexpediente == this.id_sub_tipo_tramite
                        select res);

            // busqueda con tarea abierta
            if (this.id_tarea > 0)
                qSOL = (from res in qSOL
                        join tt in db.SGI_Tramites_Tareas_HAB on res.id_solicitud equals tt.id_solicitud
                        where tt.SGI_Tramites_Tareas.id_tarea == this.id_tarea && !tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea.HasValue
                        select res);

            // busqueda con tarea cerrada
            if (this.id_tarea_cerrada > 0)
                qSOL = (from res in qSOL
                        join tt in db.SGI_Tramites_Tareas_HAB on res.id_solicitud equals tt.id_solicitud
                        where tt.SGI_Tramites_Tareas.id_tarea == this.id_tarea_cerrada && tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea.HasValue
                        select res);

            //busqueda por estado
            if (!string.IsNullOrEmpty(this.estados))
            {
                var arrayEstados = this.estados.Split(',').Select(Int32.Parse).ToList();
                qSOL = (from res in qSOL
                        where arrayEstados.Contains(res.id_estado)
                        select res);
            }

            //búsqueda entre fechas
            if (this.fechaDesde.HasValue || this.fechaHasta.HasValue)
            {
                DateTime? fecha_desde = null;
                DateTime? fecha_hasta = null;

                if (this.fechaDesde.HasValue)
                    fecha_desde = this.fechaDesde.Value;
                else
                    fecha_desde = new DateTime(2000, 1, 1);

                if (this.fechaHasta.HasValue)
                    fecha_hasta = this.fechaHasta.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                else
                    fecha_hasta = DateTime.Now;

                qSOL = (from res in qSOL
                        join tt in db.SGI_Tramites_Tareas_HAB on res.id_solicitud equals tt.id_solicitud
                        where (tt.SGI_Tramites_Tareas.FechaInicio_tramitetarea >= fecha_desde && tt.SGI_Tramites_Tareas.FechaInicio_tramitetarea <= fecha_hasta)
                        select res);

            }

            //búsqueda entre fechas de cierre
            if (this.fechaCierreDesde.HasValue || this.fechaCierreHasta.HasValue)
            {
                DateTime? fecha_cierre_desde = null;
                DateTime? fecha_cierre_hasta = null;

                if (this.fechaCierreDesde.HasValue)
                    fecha_cierre_desde = this.fechaCierreDesde.Value;
                else
                    fecha_cierre_desde = new DateTime(2000, 1, 1);

                if (this.fechaCierreHasta.HasValue)
                    fecha_cierre_hasta = this.fechaCierreHasta.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                else
                    fecha_cierre_hasta = DateTime.Now;


                qSOL = (from res in qSOL
                        join tt in db.SGI_Tramites_Tareas_HAB on res.id_solicitud equals tt.id_solicitud
                        where (tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea >= fecha_cierre_desde && tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea <= fecha_cierre_hasta)
                        select res);

            }



            //búsqueda por numero partida matriz
            if (this.nro_partida_matriz > 0)
                qSOL = (from res in qSOL
                        join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                        join ubic in db.Ubicaciones on solubic.id_ubicacion equals ubic.id_ubicacion
                        where ubic.NroPartidaMatriz == this.nro_partida_matriz
                        select res);

            //búsqueda por numero partida horizontal
            if (this.nro_partida_horiz > 0)
                qSOL = (from res in qSOL
                        join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                        join solubicphor in db.SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal on solubic.id_solicitudubicacion equals solubicphor.id_solicitudubicacion
                        join phor in db.Ubicaciones_PropiedadHorizontal on solubicphor.id_propiedadhorizontal equals phor.id_propiedadhorizontal
                        where phor.NroPartidaHorizontal == this.nro_partida_horiz
                        select res);

            //busqueda por Domicilio
            if (this.id_calle > 0)
            {
                int esImpar = 0;
                int esAmbas = 0;
                if (nro_calle > 0)
                {
                    qSOL = (from res in qSOL
                            join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                            join solpuer in db.SSIT_Solicitudes_Ubicaciones_Puertas on solubic.id_solicitudubicacion equals solpuer.id_solicitudubicacion
                            join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                            where c.id_calle == this.id_calle && (solpuer.NroPuerta == this.nro_calle || this.nro_calle == 0)
                            select res);
                }
                else if (nro_calle_desde > 0 || nro_calle_hasta > 0)
                {
                    if (this.nro_calle_impar)
                    {
                        esImpar = 1;
                    }
                    if (this.nro_calle_ambas)
                    {
                        esAmbas = 1;
                    }

                    if (nro_calle_desde > 0 && nro_calle_hasta == 0)
                    {
                        if (esAmbas == 1)
                            qSOL = (from res in qSOL
                                    join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                                    join solpuer in db.SSIT_Solicitudes_Ubicaciones_Puertas on solubic.id_solicitudubicacion equals solpuer.id_solicitudubicacion
                                    join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                    where c.id_calle == this.id_calle && (solpuer.NroPuerta >= this.nro_calle_desde)
                                    select res);
                        else
                            qSOL = (from res in qSOL
                                    join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                                    join solpuer in db.SSIT_Solicitudes_Ubicaciones_Puertas on solubic.id_solicitudubicacion equals solpuer.id_solicitudubicacion
                                    join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                    where c.id_calle == this.id_calle && (solpuer.NroPuerta >= this.nro_calle_desde)
                                    && (solpuer.NroPuerta % 2 == esImpar)
                                    select res);
                    }
                    if (nro_calle_hasta > 0 && nro_calle_desde == 0)
                    {
                        if (esAmbas == 1)
                            qSOL = (from res in qSOL
                                join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                                join solpuer in db.SSIT_Solicitudes_Ubicaciones_Puertas on solubic.id_solicitudubicacion equals solpuer.id_solicitudubicacion
                                join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                where c.id_calle == this.id_calle && (solpuer.NroPuerta <= this.nro_calle_hasta)
                                select res);
                        else
                            qSOL = (from res in qSOL
                                    join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                                    join solpuer in db.SSIT_Solicitudes_Ubicaciones_Puertas on solubic.id_solicitudubicacion equals solpuer.id_solicitudubicacion
                                    join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                    where c.id_calle == this.id_calle && (solpuer.NroPuerta <= this.nro_calle_hasta)
                                    && (solpuer.NroPuerta % 2 == esImpar)
                                    select res);
                    }
                    if(nro_calle_desde > 0 && nro_calle_hasta > 0)
                    {
                        if (esAmbas == 1)
                            qSOL = (from res in qSOL
                                join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                                join solpuer in db.SSIT_Solicitudes_Ubicaciones_Puertas on solubic.id_solicitudubicacion equals solpuer.id_solicitudubicacion
                                join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                where c.id_calle == this.id_calle && (solpuer.NroPuerta >= this.nro_calle_desde && solpuer.NroPuerta <= this.nro_calle_hasta)
                                select res );
                        else
                            qSOL = (from res in qSOL
                                    join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                                    join solpuer in db.SSIT_Solicitudes_Ubicaciones_Puertas on solubic.id_solicitudubicacion equals solpuer.id_solicitudubicacion
                                    join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                    where c.id_calle == this.id_calle && (solpuer.NroPuerta >= this.nro_calle_desde && solpuer.NroPuerta <= this.nro_calle_hasta)
                                    && (solpuer.NroPuerta % 2 == esImpar)
                                    select res);
                    }

                }
                else if(nro_calle_desde == 0 && nro_calle_hasta == 0)
                {
                    if (this.nro_calle_impar)
                    {
                        esImpar = 1;
                    }
                    if (this.nro_calle_ambas)
                    {
                        esAmbas = 1;
                    }
                    if (esAmbas == 1)
                        qSOL = (from res in qSOL
                                join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                                join solpuer in db.SSIT_Solicitudes_Ubicaciones_Puertas on solubic.id_solicitudubicacion equals solpuer.id_solicitudubicacion
                                join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                where c.id_calle == this.id_calle
                                select res);
                    else
                        qSOL = (from res in qSOL
                                join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                                join solpuer in db.SSIT_Solicitudes_Ubicaciones_Puertas on solubic.id_solicitudubicacion equals solpuer.id_solicitudubicacion
                                join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                where c.id_calle == this.id_calle && (solpuer.NroPuerta % 2 == esImpar)
                                select res);

                
                }
            }
            if (!string.IsNullOrEmpty(this.uf))

                qSOL = (from res in qSOL
                        join solubi in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubi.id_solicitud
                        join solubiprop in db.SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal on solubi.id_solicitudubicacion equals solubiprop.id_solicitudubicacion
                        join ubiprop in db.Ubicaciones_PropiedadHorizontal on solubiprop.id_propiedadhorizontal equals ubiprop.id_propiedadhorizontal
                        where ubiprop.UnidadFuncional == this.uf
                        select res);

            if (!string.IsNullOrEmpty(this.dpto))

                qSOL = (from res in qSOL
                        join solubi in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubi.id_solicitud
                        join solubiprop in db.SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal on solubi.id_solicitudubicacion equals solubiprop.id_solicitudubicacion
                        join ubiprop in db.Ubicaciones_PropiedadHorizontal on solubiprop.id_propiedadhorizontal equals ubiprop.id_propiedadhorizontal
                        where ubiprop.Depto == this.dpto
                        select res);

            if (!string.IsNullOrEmpty(this.torre))

                qSOL = (from res in qSOL
                        join solubi in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubi.id_solicitud
                        where solubi.Torre == this.torre
                        select res);

            if (!string.IsNullOrEmpty(this.local))

                qSOL = (from res in qSOL
                        join solubi in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubi.id_solicitud
                        where solubi.Local == this.local
                        select res);


            //busqueda por Sección / Manzana / Parcela
            if (this.seccion > 0)
                qSOL = (from res in qSOL
                        join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                        join ubic in db.Ubicaciones on solubic.id_ubicacion equals ubic.id_ubicacion
                        where ubic.Seccion == this.seccion &&
                            (ubic.Manzana == this.manzana || this.manzana.Length == 0) &&
                            (ubic.Parcela == this.parcela || this.parcela.Length == 0)
                        select res);

            //busqueda por Ubicaciones Especiales
            if (this.id_sub_tipo_ubicacion > -1)
                // se hace en dos partes porque existe el registro sin especificar y tanto sin especificar como (Todos) comparten el mismo id.
                // dejar en dos partes el select, con y sin id_subtipoubicacion
                qSOL = (from res in qSOL
                        join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                        join ubic in db.Ubicaciones on solubic.id_ubicacion equals ubic.id_ubicacion
                        join stipubic in db.SubTiposDeUbicacion on ubic.id_subtipoubicacion equals stipubic.id_subtipoubicacion
                        join tipubic in db.TiposDeUbicacion on stipubic.id_tipoubicacion equals tipubic.id_tipoubicacion
                        where tipubic.id_tipoubicacion == this.id_tipo_ubicacion && stipubic.id_subtipoubicacion == this.id_sub_tipo_ubicacion
                        select res);

            if (this.id_tipo_ubicacion > -1)
                qSOL = (from res in qSOL
                        join solubic in db.SSIT_Solicitudes_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                        join ubic in db.Ubicaciones on solubic.id_ubicacion equals ubic.id_ubicacion
                        join stipubic in db.SubTiposDeUbicacion on ubic.id_subtipoubicacion equals stipubic.id_subtipoubicacion
                        join tipubic in db.TiposDeUbicacion on stipubic.id_tipoubicacion equals tipubic.id_tipoubicacion
                        where tipubic.id_tipoubicacion == this.id_tipo_ubicacion
                        select res);

            // búsqueda por rubros
            if (this.rubro_desc.Length > 0)
            {
                qSOL = (from res in qSOL
                        join encsol in db.Encomienda_SSIT_Solicitudes on res.id_solicitud equals encsol.id_solicitud
                        join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                        join rub in db.Encomienda_Rubros on enc.id_encomienda equals rub.id_encomienda
                        where
                            enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                            && (rub.cod_rubro == this.rubro_desc || rub.desc_rubro.Contains(this.rubro_desc))
                        select res

                        ).Union
                        (from res in qSOL
                         join encsol in db.Encomienda_SSIT_Solicitudes on res.id_solicitud equals encsol.id_solicitud
                         join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                         join rubcn in db.Encomienda_RubrosCN on enc.id_encomienda equals rubcn.id_encomienda
                         where enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo && (rubcn.CodigoRubro == this.rubro_desc || rubcn.NombreRubro.Contains(this.rubro_desc))
                         select res).Union
                        (from res in qSOL
                         join encsol in db.Encomienda_SSIT_Solicitudes on res.id_solicitud equals encsol.id_solicitud
                         join e in db.Encomienda on encsol.id_encomienda equals e.id_encomienda
                         join erc in db.Encomienda_RubrosCN on e.id_encomienda equals erc.id_encomienda
                         join ers in db.Encomienda_RubrosCN_Subrubros on erc.id_encomiendarubro equals ers.Id_EncRubro  //erc.id_encomiendarubro equals ers.Id_EncRubro
                         join rsr in db.RubrosCN_Subrubros on ers.Id_rubrosubrubro equals rsr.Id_rubroCNsubrubro
                         where
                            e.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                            && (erc.id_encomiendarubro == ers.Id_EncRubro)
                            && (erc.CodigoRubro == this.rubro_desc || erc.NombreRubro.Contains(this.rubro_desc) || rsr.Nombre.Contains(this.rubro_desc))
                         select res)
                        ;
            }
            // búsqueda por titulares
            if (this.tit_razon.Length > 0)
            {
                //divdido por espacio y asumo que solo pone como mucho dos nombre y un apellido en cualquier orden
                string[] d = tit_razon.Split(' ');
                string d1 = d[0].Replace(',', ' ').Trim();
                string d2 = d.Length > 1 ? d[1].Replace(',', ' ').Trim() : "";
                string d3 = d.Length > 2 ? d[2].Replace(',', ' ').Trim() : "";
                qSOL = (from res in qSOL
                        join sol in db.SSIT_Solicitudes on res.id_solicitud equals sol.id_solicitud
                        join tit in db.SSIT_Solicitudes_Titulares_PersonasJuridicas on sol.id_solicitud equals tit.id_solicitud
                        where tit.Razon_Social.Contains(this.tit_razon)
                        select res).Union(
                     from res in qSOL
                     join sol in db.SSIT_Solicitudes on res.id_solicitud equals sol.id_solicitud
                     join tit in db.SSIT_Solicitudes_Titulares_PersonasFisicas on sol.id_solicitud equals tit.id_solicitud
                     where (tit.Apellido.Contains(d1) && (d2.Length == 0 || tit.Nombres.Contains(d2)) && (d3.Length == 0 || tit.Nombres.Contains(d3)))
                     || (tit.Apellido.Contains(d1) && (d2.Length == 0 || tit.Apellido.Contains(d2)) && (d3.Length == 0 || tit.Nombres.Contains(d3)))
                     || ((d2.Length == 0 || tit.Apellido.Contains(d2)) && tit.Nombres.Contains(d1) && (d3.Length == 0 || tit.Nombres.Contains(d3)))
                     || ((d2.Length == 0 || tit.Apellido.Contains(d2)) && tit.Apellido.Contains(d1) && (d3.Length == 0 || tit.Nombres.Contains(d3)))
                     || ((d3.Length == 0 || tit.Apellido.Contains(d3)) && tit.Nombres.Contains(d1) && (d2.Length == 0 || tit.Nombres.Contains(d2)))
                     || ((d3.Length == 0 || tit.Apellido.Contains(d3)) && tit.Apellido.Contains(d1) && (d2.Length == 0 || tit.Nombres.Contains(d2)))
                     select res);
            }

            //búsqueda entre superficies
            if (this.SuperficieDesde.HasValue || this.SuperficieHasta.HasValue)
            {
                Decimal? Superficie_Desde = null;
                Decimal? Superficie_Hasta = null;

                if (this.SuperficieDesde.HasValue)
                    Superficie_Desde = this.SuperficieDesde.Value;
                else
                    Superficie_Desde = 0;

                if (this.SuperficieHasta.HasValue)
                    Superficie_Hasta = this.SuperficieHasta.Value;
                else
                    Superficie_Hasta = 99999999;

                qSOL = (from res in qSOL
                        join encsol in db.Encomienda_SSIT_Solicitudes on res.id_solicitud equals encsol.id_solicitud
                        join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                        join encdl in db.Encomienda_DatosLocal on encsol.id_encomienda equals encdl.id_encomienda
                        where (encdl.superficie_cubierta_dl + encdl.superficie_descubierta_dl >= Superficie_Desde &&
                              encdl.superficie_cubierta_dl + encdl.superficie_descubierta_dl <= Superficie_Hasta)
                        select res);

            }

            //si es permiso, solo se muestran los q esten (aprobados) ingresados
            //142398: JADHE 55475 - SGI - PERMISOS MyC: Errores
            if (this.id_tipo_tramite == (int)Constants.TipoDeTramite.Permiso)
            {
                qSOL = (from res in qSOL
                        where res.id_estado == (int)Constants.Solicitud_Estados.Aprobada
                        select res);
            }
            #endregion

            // Consulta de datos CPadron
            #region "Consulta CPadron"
            IQueryable<cls_ultima_tarea> lst_Ultima_tareaCP = (from tt in db.SGI_Tramites_Tareas_CPADRON
                                                               group tt by tt.id_cpadron into g
                                                               select new cls_ultima_tarea
                                                               {
                                                                   id_solicitud = g.Key,
                                                                   id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                                               });

            qCP = (from sol in db.CPadron_Solicitudes
                   join eDatos in db.CPadron_DatosLocal on sol.id_cpadron equals eDatos.id_cpadron into zr
                   from ed in zr.DefaultIfEmpty()
                   join ult_tar in lst_Ultima_tareaCP on sol.id_cpadron equals ult_tar.id_solicitud into pleft_ult_tar
                   from ult_tar in pleft_ult_tar.DefaultIfEmpty()
                   join tt in db.SGI_Tramites_Tareas_CPADRON on ult_tar.id_tramitetarea equals tt.id_tramitetarea into pleft_tt
                   from tt in pleft_tt.DefaultIfEmpty()
                   join tar in db.ENG_Tareas on tt.SGI_Tramites_Tareas.id_tarea equals tar.id_tarea
                   join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito into pleft_tarea
                   from cir in pleft_tarea.DefaultIfEmpty()
                   where db.SGI_Tramites_Tareas_CPADRON.Where(x => x.id_cpadron == sol.id_cpadron && x.SGI_Tramites_Tareas.ENG_Tareas.formulario_tarea != null
                   && x.SGI_Tramites_Tareas.ENG_Tareas.id_tarea != (int)Constants.ENG_Tareas.CP_Fin_Tramite).Count() > 0
                   && sol.id_estado != (int)Constants.CPadron_EstadoSolicitud.Anulado
                   select new clsItemBuscarTramite
                   {
                       cod_grupotramite = Constants.GruposDeTramite.CP.ToString(),
                       id_tramitetarea = tt.id_tramitetarea,
                       id_solicitud = sol.id_cpadron,
                       FechaInicio_tarea = tt.SGI_Tramites_Tareas.FechaInicio_tramitetarea,
                       FechaCierre_tarea = tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea,
                       FechaAsignacion_tarea = tt.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea,
                       direccion = "",
                       id_tarea = tt.SGI_Tramites_Tareas.id_tarea,
                       nombre_tarea = tar.nombre_tarea,
                       asignable_tarea = tar.Asignable_tarea,
                       formulario_tarea = tar.formulario_tarea,
                       desc_circuito = "CP",
                       superficie_total = (ed != null ? (ed.superficie_cubierta_dl.Value + ed.superficie_descubierta_dl.Value) : 0),
                       url_visorTramite = "~/VisorTramiteCP/{0}",
                       url_tareaTramite = "~/GestionTramite/Tareas/{0}?id={1}",
                       nro_Expediente = sol.NroExpedienteSade,
                       estado = sol.CPadron_Estados.nom_estado_interno,
                       id_estado = sol.id_estado,
                   }).Distinct();

            // busqueda por datos del trámite
            if (this.id_solicitud > 0)
                qCP = qCP.Where(x => x.id_solicitud == this.id_solicitud);


            if (!string.IsNullOrEmpty(this.nroExp))
                qCP = qCP.Where(x => x.nro_Expediente.Contains(this.nroExp.Trim()));


            //búsqueda por encomienda
            if (this.id_encomida > 0)
                qCP = qCP.Where(x => x.id_solicitud == 0);

            //búsqueda por tipo de tramite
            if (this.id_tipo_tramite > 0)
            {
                qCP = (from res in qCP
                       join sol in db.CPadron_Solicitudes on res.id_solicitud equals sol.id_cpadron
                       where sol.id_tipotramite == this.id_tipo_tramite
                       select res);
            }

            //búsqueda por tipo de expediente
            if (this.id_tipo_expediente > 0)
                qCP = (from res in qCP
                       join sol in db.CPadron_Solicitudes on res.id_solicitud equals sol.id_cpadron
                       where sol.id_tipoexpediente == this.id_tipo_expediente
                       select res);

            //búsqueda por sub tipo de tramite
            if (this.id_sub_tipo_tramite > 0)
                qCP = (from res in qCP
                       join sol in db.CPadron_Solicitudes on res.id_solicitud equals sol.id_cpadron
                       where sol.id_subtipoexpediente == this.id_sub_tipo_tramite
                       select res);

            // busqueda con tarea abierta
            if (this.id_tarea > 0)
                qCP = (from res in qCP
                       join tt in db.SGI_Tramites_Tareas_CPADRON on res.id_solicitud equals tt.id_cpadron
                       where tt.SGI_Tramites_Tareas.id_tarea == this.id_tarea && !tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea.HasValue
                       select res);

            // busqueda con tarea cerrada
            if (this.id_tarea_cerrada > 0)
                qCP = (from res in qCP
                       join tt in db.SGI_Tramites_Tareas_CPADRON on res.id_solicitud equals tt.id_cpadron
                       where tt.SGI_Tramites_Tareas.id_tarea == this.id_tarea_cerrada && tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea.HasValue
                       select res);


            //búsqueda entre fechas
            if (this.fechaDesde.HasValue || this.fechaHasta.HasValue)
            {
                DateTime? fecha_desde = null;
                DateTime? fecha_hasta = null;

                if (this.fechaDesde.HasValue)
                    fecha_desde = this.fechaDesde.Value;
                else
                    fecha_desde = new DateTime(2000, 1, 1);

                if (this.fechaHasta.HasValue)
                    fecha_hasta = this.fechaHasta.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                else
                    fecha_hasta = DateTime.Now;


                qCP = (from res in qCP
                       join tt in db.SGI_Tramites_Tareas_CPADRON on res.id_solicitud equals tt.id_cpadron
                       where (tt.SGI_Tramites_Tareas.FechaInicio_tramitetarea >= fecha_desde && tt.SGI_Tramites_Tareas.FechaInicio_tramitetarea <= fecha_hasta)
                       select res);
            }


            //búsqueda entre fechas de cierre
            if (this.fechaCierreDesde.HasValue || this.fechaCierreHasta.HasValue)
            {
                DateTime? fecha_cierre_desde = null;
                DateTime? fecha_cierre_hasta = null;

                if (this.fechaCierreDesde.HasValue)
                    fecha_cierre_desde = this.fechaCierreDesde.Value;
                else
                    fecha_cierre_desde = new DateTime(2000, 1, 1);

                if (this.fechaCierreHasta.HasValue)
                    fecha_cierre_hasta = this.fechaCierreHasta.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                else
                    fecha_cierre_hasta = DateTime.Now;


                qCP = (from res in qCP
                       join tt in db.SGI_Tramites_Tareas_CPADRON on res.id_solicitud equals tt.id_cpadron
                       where (tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea >= fecha_cierre_desde && tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea <= fecha_cierre_hasta)
                       select res);
            }



            //búsqueda por numero partida matriz
            if (this.nro_partida_matriz > 0)
                qCP = (from res in qCP
                       join encubic in db.CPadron_Ubicaciones on res.id_solicitud equals encubic.id_cpadron
                       join ubic in db.Ubicaciones on encubic.id_ubicacion equals ubic.id_ubicacion
                       where ubic.NroPartidaMatriz == this.nro_partida_matriz
                       select res);

            //búsqueda por numero partida horizontal
            if (this.nro_partida_horiz > 0)
                qCP = (from res in qCP
                       join encubic in db.CPadron_Ubicaciones on res.id_solicitud equals encubic.id_cpadron
                       join encubicphor in db.CPadron_Ubicaciones_PropiedadHorizontal on encubic.id_cpadronubicacion equals encubicphor.id_cpadronubicacion
                       join phor in db.Ubicaciones_PropiedadHorizontal on encubicphor.id_propiedadhorizontal equals phor.id_propiedadhorizontal
                       where phor.NroPartidaHorizontal == this.nro_partida_horiz
                       select res);

            //busqueda por Domicilio
            if (this.id_calle > 0)
            {
                int esImpar = 0;
                int esAmbas = 0;
                if (nro_calle > 0)
                {
                    qCP = (from res in qCP
                           join solubic in db.CPadron_Ubicaciones on res.id_solicitud equals solubic.id_cpadron
                           join solpuer in db.CPadron_Ubicaciones_Puertas on solubic.id_cpadronubicacion equals solpuer.id_cpadronubicacion
                           join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                           where c.id_calle == this.id_calle && (solpuer.NroPuerta == this.nro_calle || this.nro_calle == 0)
                           select res);
                }
                else if (nro_calle_desde > 0 || nro_calle_hasta > 0)
                {
                    if (this.nro_calle_impar)
                    {
                        esImpar = 1;
                    }
                    if (this.nro_calle_ambas)
                    {
                        esAmbas = 1;
                    }

                    if (nro_calle_desde > 0 && nro_calle_hasta == 0)
                    {
                        if (esAmbas == 1)
                            qCP = (from res in qCP
                                   join solubic in db.CPadron_Ubicaciones on res.id_solicitud equals solubic.id_cpadron
                                   join solpuer in db.CPadron_Ubicaciones_Puertas on solubic.id_cpadronubicacion equals solpuer.id_cpadronubicacion
                                   join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                   where c.id_calle == this.id_calle && (solpuer.NroPuerta >= this.nro_calle_desde)
                                   select res);
                        else
                            qCP = (from res in qCP
                                   join solubic in db.CPadron_Ubicaciones on res.id_solicitud equals solubic.id_cpadron
                                   join solpuer in db.CPadron_Ubicaciones_Puertas on solubic.id_cpadronubicacion equals solpuer.id_cpadronubicacion
                                   join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                   where c.id_calle == this.id_calle && (solpuer.NroPuerta >= this.nro_calle_desde)
                                   && (solpuer.NroPuerta % 2 == esImpar)
                                   select res);
                    }
                    if (nro_calle_hasta > 0 && nro_calle_desde == 0)
                    {
                        if (esAmbas == 1)
                            qCP = (from res in qCP
                                   join solubic in db.CPadron_Ubicaciones on res.id_solicitud equals solubic.id_cpadron
                                   join solpuer in db.CPadron_Ubicaciones_Puertas on solubic.id_cpadronubicacion equals solpuer.id_cpadronubicacion
                                   join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                   where c.id_calle == this.id_calle && (solpuer.NroPuerta <= this.nro_calle_hasta)
                                    select res);
                        else
                            qCP = (from res in qCP
                                   join solubic in db.CPadron_Ubicaciones on res.id_solicitud equals solubic.id_cpadron
                                   join solpuer in db.CPadron_Ubicaciones_Puertas on solubic.id_cpadronubicacion equals solpuer.id_cpadronubicacion
                                   join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                   where c.id_calle == this.id_calle && (solpuer.NroPuerta <= this.nro_calle_hasta)
                                    && (solpuer.NroPuerta % 2 == esImpar)
                                   select res);
                    }
                    if (nro_calle_desde > 0 && nro_calle_hasta > 0)
                    {
                        if (esAmbas == 1)
                            qCP = (from res in qCP
                                   join solubic in db.CPadron_Ubicaciones on res.id_solicitud equals solubic.id_cpadron
                                   join solpuer in db.CPadron_Ubicaciones_Puertas on solubic.id_cpadronubicacion equals solpuer.id_cpadronubicacion
                                   join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                   where c.id_calle == this.id_calle && (solpuer.NroPuerta >= this.nro_calle_desde && solpuer.NroPuerta <= this.nro_calle_hasta)
                                    select res);
                        else
                            qCP = (from res in qCP
                                   join solubic in db.CPadron_Ubicaciones on res.id_solicitud equals solubic.id_cpadron
                                   join solpuer in db.CPadron_Ubicaciones_Puertas on solubic.id_cpadronubicacion equals solpuer.id_cpadronubicacion
                                   join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                                   where c.id_calle == this.id_calle && (solpuer.NroPuerta >= this.nro_calle_desde && solpuer.NroPuerta <= this.nro_calle_hasta)
                                    && (solpuer.NroPuerta % 2 == esImpar)
                                   select res);
                    }

                }
                else if(nro_calle_desde == 0 && nro_calle_hasta == 0)
                {
                    if (this.nro_calle_impar)
                    {
                        esImpar = 1;
                    }
                    if (this.nro_calle_ambas)
                    {
                        esAmbas =1;
                    }
                    if (esAmbas == 1)
                        qCP = (from res in qCP
                               join solubic in db.CPadron_Ubicaciones on res.id_solicitud equals solubic.id_cpadron
                               join solpuer in db.CPadron_Ubicaciones_Puertas on solubic.id_cpadronubicacion equals solpuer.id_cpadronubicacion
                               join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                               where c.id_calle == this.id_calle
                               select res);
                    else
                        qCP = (from res in qCP
                               join solubic in db.CPadron_Ubicaciones on res.id_solicitud equals solubic.id_cpadron
                               join solpuer in db.CPadron_Ubicaciones_Puertas on solubic.id_cpadronubicacion equals solpuer.id_cpadronubicacion
                               join c in db.Calles on solpuer.codigo_calle equals c.Codigo_calle
                               where c.id_calle == this.id_calle && (solpuer.NroPuerta % 2 == esImpar)
                               select res);
                }
            }

            if (!string.IsNullOrEmpty(this.uf))

                qCP = (from res in qCP
                       join solubi in db.CPadron_Ubicaciones on res.id_solicitud equals solubi.id_cpadron
                       join solubiprop in db.CPadron_Ubicaciones_PropiedadHorizontal on solubi.id_cpadronubicacion equals solubiprop.id_cpadronubicacion
                       join ubiprop in db.Ubicaciones_PropiedadHorizontal on solubiprop.id_propiedadhorizontal equals ubiprop.id_propiedadhorizontal
                       where ubiprop.UnidadFuncional == this.uf
                       select res);

            if (!string.IsNullOrEmpty(this.dpto))

                qCP = (from res in qCP
                       join solubi in db.CPadron_Ubicaciones on res.id_solicitud equals solubi.id_cpadron
                       join solubiprop in db.CPadron_Ubicaciones_PropiedadHorizontal on solubi.id_cpadronubicacion equals solubiprop.id_cpadronubicacion
                       join ubiprop in db.Ubicaciones_PropiedadHorizontal on solubiprop.id_propiedadhorizontal equals ubiprop.id_propiedadhorizontal
                       where ubiprop.Depto == this.dpto
                       select res);

            if (!string.IsNullOrEmpty(this.torre))

                qCP = (from res in qCP
                       join solubi in db.CPadron_Ubicaciones on res.id_solicitud equals solubi.id_cpadron
                       where solubi.Torre == this.torre
                       select res);

            if (!string.IsNullOrEmpty(this.local))

                qCP = (from res in qCP
                       join solubi in db.CPadron_Ubicaciones on res.id_solicitud equals solubi.id_cpadron
                       where solubi.Local == this.local
                       select res);

            //busqueda por Sección / Manzana / Parcela
            if (this.seccion > 0)
                qCP = (from res in qCP
                       join encubic in db.CPadron_Ubicaciones on res.id_solicitud equals encubic.id_cpadron
                       join ubic in db.Ubicaciones on encubic.id_ubicacion equals ubic.id_ubicacion
                       where ubic.Seccion == this.seccion &&
                           (ubic.Manzana == this.manzana || this.manzana.Length == 0) &&
                           (ubic.Parcela == this.parcela || this.parcela.Length == 0)
                       select res);

            //busqueda por Ubicaciones Especiales
            if (this.id_sub_tipo_ubicacion > -1)
                // se hace en dos partes porque existe el registro sin especificar y tanto sin especificar como (Todos) comparten el mismo id.
                // dejar en dos partes el select, con y sin id_subtipoubicacion
                qCP = (from res in qCP
                       join encubic in db.CPadron_Ubicaciones on res.id_solicitud equals encubic.id_cpadron
                       join ubic in db.Ubicaciones on encubic.id_ubicacion equals ubic.id_ubicacion
                       join stipubic in db.SubTiposDeUbicacion on ubic.id_subtipoubicacion equals stipubic.id_subtipoubicacion
                       join tipubic in db.TiposDeUbicacion on stipubic.id_tipoubicacion equals tipubic.id_tipoubicacion
                       where tipubic.id_tipoubicacion == this.id_tipo_ubicacion && stipubic.id_subtipoubicacion == this.id_sub_tipo_ubicacion
                       select res);

            if (this.id_tipo_ubicacion > -1)
                qCP = (from res in qCP
                       join encubic in db.CPadron_Ubicaciones on res.id_solicitud equals encubic.id_cpadron
                       join ubic in db.Ubicaciones on encubic.id_ubicacion equals ubic.id_ubicacion
                       join stipubic in db.SubTiposDeUbicacion on ubic.id_subtipoubicacion equals stipubic.id_subtipoubicacion
                       join tipubic in db.TiposDeUbicacion on stipubic.id_tipoubicacion equals tipubic.id_tipoubicacion
                       where tipubic.id_tipoubicacion == this.id_tipo_ubicacion
                       select res);

            // búsqueda por rubros
            if (this.rubro_desc.Length > 0)
                qCP = (from res in qCP
                       join rub in db.CPadron_Rubros on res.id_solicitud equals rub.id_cpadron
                       where rub.cod_rubro == this.rubro_desc || rub.desc_rubro.Contains(this.rubro_desc)
                       select res);

            // búsqueda por titulares
            if (this.tit_razon.Length > 0)
            {
                //divdido por espacio y asumo que solo pone como mucho dos nombre y un apellido en cualquier orden
                string[] d = tit_razon.Split(' ');
                string d1 = d[0].Replace(',', ' ').Trim();
                string d2 = d.Length > 1 ? d[1].Replace(',', ' ').Trim() : "";
                string d3 = d.Length > 2 ? d[2].Replace(',', ' ').Trim() : "";
                qCP = (from res in qCP
                       join tit in db.CPadron_Titulares_PersonasJuridicas on res.id_solicitud equals tit.id_cpadron
                       where tit.Razon_Social.Contains(this.tit_razon)
                       select res).Union(
                     from res in qCP
                     join tit in db.CPadron_Titulares_PersonasFisicas on res.id_solicitud equals tit.id_cpadron
                     where (tit.Apellido.Contains(d1) && (d2.Length == 0 || tit.Nombres.Contains(d2)) && (d3.Length == 0 || tit.Nombres.Contains(d3)))
                     || (tit.Apellido.Contains(d1) && (d2.Length == 0 || tit.Apellido.Contains(d2)) && (d3.Length == 0 || tit.Nombres.Contains(d3)))
                     || ((d2.Length == 0 || tit.Apellido.Contains(d2)) && tit.Nombres.Contains(d1) && (d3.Length == 0 || tit.Nombres.Contains(d3)))
                     || ((d2.Length == 0 || tit.Apellido.Contains(d2)) && tit.Apellido.Contains(d1) && (d3.Length == 0 || tit.Nombres.Contains(d3)))
                     || ((d3.Length == 0 || tit.Apellido.Contains(d3)) && tit.Nombres.Contains(d1) && (d2.Length == 0 || tit.Nombres.Contains(d2)))
                     || ((d3.Length == 0 || tit.Apellido.Contains(d3)) && tit.Apellido.Contains(d1) && (d2.Length == 0 || tit.Nombres.Contains(d2)))
                     select res);

            }

            //busqueda por estado
            if (!string.IsNullOrEmpty(this.estados))
            {
                var arrayEstados = this.estados.Split(',').Select(Int32.Parse).ToList();
                qCP = (from res in qCP
                       where arrayEstados.Contains(res.id_estado)
                       select res);
            }

            //búsqueda entre superficies
            if (this.SuperficieDesde.HasValue || this.SuperficieHasta.HasValue)
            {
                Decimal? Superficie_Desde = null;
                Decimal? Superficie_Hasta = null;

                if (this.SuperficieDesde.HasValue)
                    Superficie_Desde = this.SuperficieDesde.Value;
                else
                    Superficie_Desde = 0;

                if (this.SuperficieHasta.HasValue)
                    Superficie_Hasta = this.SuperficieHasta.Value;
                else
                    Superficie_Hasta = 99999999;

                qCP = (from res in qCP
                       join cpadron in db.CPadron_Solicitudes on res.id_solicitud equals cpadron.id_cpadron
                       join cpdl in db.CPadron_DatosLocal on cpadron.id_cpadron equals cpdl.id_cpadron
                       where (cpdl.superficie_cubierta_dl + cpdl.superficie_descubierta_dl >= Superficie_Desde &&
                             cpdl.superficie_cubierta_dl + cpdl.superficie_descubierta_dl <= Superficie_Hasta)
                       select res);
            }
            #endregion

            // Consulta de Transferencia
            #region "Consulta Transferencia"
            int nroTrReferencia = 0;
            int.TryParse(Parametros.GetParam_ValorChar("NroTransmisionReferencia"), out nroTrReferencia);

            IQueryable<cls_ultima_tarea> lst_Ultima_tareaTR = (from tt in db.SGI_Tramites_Tareas_TRANSF
                                                               group tt by tt.id_solicitud into g
                                                               select new cls_ultima_tarea
                                                               {
                                                                   id_solicitud = g.Key,
                                                                   id_tramitetarea = g.Max(s => s.id_tramitetarea)
                                                               });
            qTR = (from sol in db.Transf_Solicitudes
                   join eDatos in db.CPadron_DatosLocal on sol.id_cpadron equals eDatos.id_cpadron into zr
                   from ed in zr.DefaultIfEmpty()
                   join ult_tar in lst_Ultima_tareaTR on sol.id_solicitud equals ult_tar.id_solicitud into pleft_ult_tar
                   from ult_tar in pleft_ult_tar.DefaultIfEmpty()
                   join tt in db.SGI_Tramites_Tareas_TRANSF on ult_tar.id_tramitetarea equals tt.id_tramitetarea into pleft_tt
                   from tt in pleft_tt.DefaultIfEmpty()
                   join tar in db.ENG_Tareas on tt.SGI_Tramites_Tareas.id_tarea equals tar.id_tarea
                   join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito into pleft_tarea
                   from cir in pleft_tarea.DefaultIfEmpty()
                   where db.SGI_Tramites_Tareas_TRANSF.Where(x => x.id_solicitud == sol.id_solicitud && x.SGI_Tramites_Tareas.ENG_Tareas.formulario_tarea != null
                       && x.SGI_Tramites_Tareas.ENG_Tareas.id_tarea != (int)Constants.ENG_Tareas.TR_Fin_Tramite).Count() > 0
                       && sol.id_estado != (int)Constants.Solicitud_Estados.Anulado
                   select new clsItemBuscarTramite
                   {
                       cod_grupotramite = Constants.GruposDeTramite.TR.ToString(),
                       id_tramitetarea = tt.id_tramitetarea,
                       id_solicitud = sol.id_solicitud,
                       FechaInicio_tarea = tt.SGI_Tramites_Tareas.FechaInicio_tramitetarea,
                       FechaCierre_tarea = tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea,
                       FechaAsignacion_tarea = tt.SGI_Tramites_Tareas.FechaAsignacion_tramtietarea,
                       direccion = "",
                       id_tarea = tt.SGI_Tramites_Tareas.id_tarea,
                       nombre_tarea = tar.nombre_tarea,
                       asignable_tarea = tar.Asignable_tarea,
                       formulario_tarea = tar.formulario_tarea,
                       desc_circuito = "TR",
                       superficie_total = (ed != null ? (ed.superficie_cubierta_dl.Value + ed.superficie_descubierta_dl.Value) : 0),
                       url_visorTramite = "~/VisorTramiteTR/{0}",
                       url_tareaTramite = "~/GestionTramite/Tareas/{0}?id={1}",
                       nro_Expediente = sol.NroExpedienteSade,
                       estado = sol.TipoEstadoSolicitud.Descripcion,
                       id_estado = sol.id_estado,
                   }).Distinct();

            // busqueda por datos del trámite
            if (this.id_solicitud > 0)
                qTR = qTR.Where(x => x.id_solicitud == this.id_solicitud);

            if (!string.IsNullOrEmpty(this.nroExp))
                qTR = qTR.Where(x => x.nro_Expediente.Contains(this.nroExp.Trim()));

            //búsqueda por encomienda
            if (this.id_encomida > 0)
                qTR = qTR.Where(x => x.id_solicitud == 0);

            //búsqueda por tipo de tramite
            if (this.id_tipo_tramite > 0)
            {
                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       where sol.id_tipotramite == this.id_tipo_tramite
                       select res);
            }

            //búsqueda por tipo de expediente
            if (this.id_tipo_expediente > 0)
                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       where sol.id_tipoexpediente == this.id_tipo_expediente
                       select res);

            //búsqueda por sub tipo de tramite
            if (this.id_sub_tipo_tramite > 0)
                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       where sol.id_subtipoexpediente == this.id_sub_tipo_tramite
                       select res);

            // busqueda con tarea abierta
            if (this.id_tarea > 0)
                qTR = (from res in qTR
                       join tt in db.SGI_Tramites_Tareas_TRANSF on res.id_solicitud equals tt.id_solicitud
                       where tt.SGI_Tramites_Tareas.id_tarea == this.id_tarea && !tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea.HasValue
                       select res);

            // busqueda con tarea cerrada
            if (this.id_tarea_cerrada > 0)
                qTR = (from res in qTR
                       join tt in db.SGI_Tramites_Tareas_TRANSF on res.id_solicitud equals tt.id_solicitud
                       where tt.SGI_Tramites_Tareas.id_tarea == this.id_tarea_cerrada && tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea.HasValue
                       select res);

            //búsqueda entre fechas
            if (this.fechaDesde.HasValue || this.fechaHasta.HasValue)
            {
                DateTime? fecha_desde = null;
                DateTime? fecha_hasta = null;

                if (this.fechaDesde.HasValue)
                    fecha_desde = this.fechaDesde.Value;
                else
                    fecha_desde = new DateTime(2000, 1, 1);

                if (this.fechaHasta.HasValue)
                    fecha_hasta = this.fechaHasta.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                else
                    fecha_hasta = DateTime.Now;


                qTR = (from res in qTR
                       join tt in db.SGI_Tramites_Tareas_TRANSF on res.id_solicitud equals tt.id_solicitud
                       where (tt.SGI_Tramites_Tareas.FechaInicio_tramitetarea >= fecha_desde && tt.SGI_Tramites_Tareas.FechaInicio_tramitetarea <= fecha_hasta)
                       select res);

            }

            //búsqueda entre fechas de cierre
            if (this.fechaCierreDesde.HasValue || this.fechaCierreHasta.HasValue)
            {
                DateTime? fecha_cierre_desde = null;
                DateTime? fecha_cierre_hasta = null;

                if (this.fechaCierreDesde.HasValue)
                    fecha_cierre_desde = this.fechaCierreDesde.Value;
                else
                    fecha_cierre_desde = new DateTime(2000, 1, 1);

                if (this.fechaCierreHasta.HasValue)
                    fecha_cierre_hasta = this.fechaCierreHasta.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                else
                    fecha_cierre_hasta = DateTime.Now;


                qTR = (from res in qTR
                       join tt in db.SGI_Tramites_Tareas_TRANSF on res.id_solicitud equals tt.id_solicitud
                       where (tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea >= fecha_cierre_desde && tt.SGI_Tramites_Tareas.FechaCierre_tramitetarea <= fecha_cierre_hasta)
                       select res);

            }

            //búsqueda por numero partida matriz
            if (this.nro_partida_matriz > 0)
                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                       join ubic in db.Ubicaciones on encubic.id_ubicacion equals ubic.id_ubicacion
                       where ubic.NroPartidaMatriz == this.nro_partida_matriz
                       select res).Union(from res in qTR
                                         join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                         where encubic.Ubicaciones.NroPartidaMatriz == this.nro_partida_matriz
                                         select res);

            //búsqueda por numero partida horizontal
            if (this.nro_partida_horiz > 0)
                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                       join encubicphor in db.CPadron_Ubicaciones_PropiedadHorizontal on encubic.id_cpadronubicacion equals encubicphor.id_cpadronubicacion
                       join phor in db.Ubicaciones_PropiedadHorizontal on encubicphor.id_propiedadhorizontal equals phor.id_propiedadhorizontal
                       where phor.NroPartidaHorizontal == this.nro_partida_horiz
                       select res).Union(from res in qTR
                                         join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                         where encubic.Transf_Ubicaciones_PropiedadHorizontal.Where(x => x.id_transfubicacion == encubic.id_transfubicacion).Select(y => y.Ubicaciones_PropiedadHorizontal.NroPartidaHorizontal).Contains(this.nro_partida_horiz)
                                         select res);

            //busqueda por Domicilio
            if (this.id_calle > 0)
            {
                int esImpar = 0;
                int esAmbas = 0;
                if (nro_calle > 0)
                {
                    qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                       join encpuer in db.CPadron_Ubicaciones_Puertas on encubic.id_cpadronubicacion equals encpuer.id_cpadronubicacion
                       join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                       where c.id_calle == this.id_calle && (encpuer.NroPuerta == this.nro_calle || this.nro_calle == 0)
                       select res).Union(from res in qTR
                                         join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                         join encpuer in db.Transf_Ubicaciones_Puertas on encubic.id_transfubicacion equals encpuer.id_transfubicacion
                                         join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                         where c.id_calle == this.id_calle && (encpuer.NroPuerta == this.nro_calle || this.nro_calle == 0)
                                         select res);
                }
                else if (nro_calle_desde > 0 || nro_calle_hasta > 0)
                {
                    if (this.nro_calle_impar)
                    {
                        esImpar = 1;
                    }
                    if (this.nro_calle_ambas)
                    {
                        esAmbas = 1;
                    }

                    if (nro_calle_desde > 0 && nro_calle_hasta == 0)
                    {
                        if (esAmbas == 1)
                            qTR = (from res in qTR
                                   join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                                   join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                                   join encpuer in db.CPadron_Ubicaciones_Puertas on encubic.id_cpadronubicacion equals encpuer.id_cpadronubicacion
                                   join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                   where c.id_calle == this.id_calle && (encpuer.NroPuerta >= this.nro_calle_desde)
                                   select res).Union(from res in qTR
                                                     join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                                     join encpuer in db.Transf_Ubicaciones_Puertas on encubic.id_transfubicacion equals encpuer.id_transfubicacion
                                                     join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                                     where c.id_calle == this.id_calle && (encpuer.NroPuerta >= this.nro_calle_desde)
                                                     select res);
                        else
                            qTR = (from res in qTR
                                   join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                                   join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                                   join encpuer in db.CPadron_Ubicaciones_Puertas on encubic.id_cpadronubicacion equals encpuer.id_cpadronubicacion
                                   join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                   where c.id_calle == this.id_calle && (encpuer.NroPuerta >= this.nro_calle_desde)
                                   && (encpuer.NroPuerta % 2 == esImpar)
                                   select res).Union(from res in qTR
                                                     join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                                     join encpuer in db.Transf_Ubicaciones_Puertas on encubic.id_transfubicacion equals encpuer.id_transfubicacion
                                                     join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                                     where c.id_calle == this.id_calle && (encpuer.NroPuerta >= this.nro_calle_desde)
                                                     && (encpuer.NroPuerta % 2 == esImpar)
                                                     select res);
                    }
                    if (nro_calle_hasta > 0 && nro_calle_desde == 0)
                    {
                        if (esAmbas == 1)
                            qTR = (from res in qTR
                                   join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                                   join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                                   join encpuer in db.CPadron_Ubicaciones_Puertas on encubic.id_cpadronubicacion equals encpuer.id_cpadronubicacion
                                   join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                   where c.id_calle == this.id_calle && (encpuer.NroPuerta <= this.nro_calle_hasta)
                                   select res).Union(from res in qTR
                                                     join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                                     join encpuer in db.Transf_Ubicaciones_Puertas on encubic.id_transfubicacion equals encpuer.id_transfubicacion
                                                     join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                                     where c.id_calle == this.id_calle && (encpuer.NroPuerta <= this.nro_calle_hasta)
                                                     select res);
                        else
                            qTR = (from res in qTR
                                   join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                                   join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                                   join encpuer in db.CPadron_Ubicaciones_Puertas on encubic.id_cpadronubicacion equals encpuer.id_cpadronubicacion
                                   join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                   where c.id_calle == this.id_calle && (encpuer.NroPuerta <= this.nro_calle_hasta)
                                   && (encpuer.NroPuerta % 2 == esImpar)
                                   select res).Union(from res in qTR
                                                     join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                                     join encpuer in db.Transf_Ubicaciones_Puertas on encubic.id_transfubicacion equals encpuer.id_transfubicacion
                                                     join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                                     where c.id_calle == this.id_calle && (encpuer.NroPuerta <= this.nro_calle_hasta)
                                                     && (encpuer.NroPuerta % 2 == esImpar)
                                                     select res);
                    }
                    if (nro_calle_desde > 0 && nro_calle_hasta > 0)
                    {
                        if (esAmbas == 1)
                            qTR = (from res in qTR
                                   join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                                   join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                                   join encpuer in db.CPadron_Ubicaciones_Puertas on encubic.id_cpadronubicacion equals encpuer.id_cpadronubicacion
                                   join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                   where c.id_calle == this.id_calle && (encpuer.NroPuerta >= this.nro_calle_desde && encpuer.NroPuerta <= this.nro_calle_hasta)
                                   select res).Union(from res in qTR
                                                     join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                                     join encpuer in db.Transf_Ubicaciones_Puertas on encubic.id_transfubicacion equals encpuer.id_transfubicacion
                                                     join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                                     where c.id_calle == this.id_calle && (encpuer.NroPuerta >= this.nro_calle_desde && encpuer.NroPuerta <= this.nro_calle_hasta)
                                                     select res);
                        else
                            qTR = (from res in qTR
                                   join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                                   join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                                   join encpuer in db.CPadron_Ubicaciones_Puertas on encubic.id_cpadronubicacion equals encpuer.id_cpadronubicacion
                                   join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                   where c.id_calle == this.id_calle && (encpuer.NroPuerta >= this.nro_calle_desde && encpuer.NroPuerta <= this.nro_calle_hasta)
                                   && (encpuer.NroPuerta % 2 == esImpar)
                                   select res).Union(from res in qTR
                                                     join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                                     join encpuer in db.Transf_Ubicaciones_Puertas on encubic.id_transfubicacion equals encpuer.id_transfubicacion
                                                     join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                                     where c.id_calle == this.id_calle && (encpuer.NroPuerta >= this.nro_calle_desde && encpuer.NroPuerta <= this.nro_calle_hasta)
                                                     && (encpuer.NroPuerta % 2 == esImpar)
                                                     select res);
                    }

                }
                else if (nro_calle_desde == 0 && nro_calle_hasta == 0)
                {
                    if (this.nro_calle_impar)
                    {
                        esImpar = 1;
                    }
                    if (this.nro_calle_ambas)
                    {
                        esAmbas = 1;
                    }
                    if (esAmbas == 1)
                        qTR = (from res in qTR
                               join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                               join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                               join encpuer in db.CPadron_Ubicaciones_Puertas on encubic.id_cpadronubicacion equals encpuer.id_cpadronubicacion
                               join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                               where c.id_calle == this.id_calle 
                               select res).Union(from res in qTR
                                                 join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                                 join encpuer in db.Transf_Ubicaciones_Puertas on encubic.id_transfubicacion equals encpuer.id_transfubicacion
                                                 join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                                 where c.id_calle == this.id_calle
                                                 select res);
                    else
                        qTR = (from res in qTR
                               join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                               join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                               join encpuer in db.CPadron_Ubicaciones_Puertas on encubic.id_cpadronubicacion equals encpuer.id_cpadronubicacion
                               join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                               where c.id_calle == this.id_calle && (encpuer.NroPuerta % 2 == esImpar)
                               select res).Union(from res in qTR
                                                 join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                                 join encpuer in db.Transf_Ubicaciones_Puertas on encubic.id_transfubicacion equals encpuer.id_transfubicacion
                                                 join c in db.Calles on encpuer.codigo_calle equals c.Codigo_calle
                                                 where c.id_calle == this.id_calle && (encpuer.NroPuerta % 2 == esImpar)
                                                 select res);
                }
            }

            if (!string.IsNullOrEmpty(this.uf))

                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       join solubi in db.CPadron_Ubicaciones on sol.id_cpadron equals solubi.id_cpadron
                       join solubiprop in db.CPadron_Ubicaciones_PropiedadHorizontal on solubi.id_cpadronubicacion equals solubiprop.id_cpadronubicacion
                       join ubiprop in db.Ubicaciones_PropiedadHorizontal on solubiprop.id_propiedadhorizontal equals ubiprop.id_propiedadhorizontal
                       where ubiprop.UnidadFuncional == this.uf
                       select res).Union(from res in qTR
                                         join solubi in db.Transf_Ubicaciones on res.id_solicitud equals solubi.id_solicitud
                                         where solubi.Transf_Ubicaciones_PropiedadHorizontal.Where(x => x.id_transfubicacion == solubi.id_transfubicacion).Select(y => y.Ubicaciones_PropiedadHorizontal.UnidadFuncional).Contains(this.uf)
                                         select res);

            if (!string.IsNullOrEmpty(this.dpto))

                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       join solubi in db.CPadron_Ubicaciones on sol.id_cpadron equals solubi.id_cpadron
                       join solubiprop in db.CPadron_Ubicaciones_PropiedadHorizontal on solubi.id_cpadronubicacion equals solubiprop.id_cpadronubicacion
                       join ubiprop in db.Ubicaciones_PropiedadHorizontal on solubiprop.id_propiedadhorizontal equals ubiprop.id_propiedadhorizontal
                       where ubiprop.Depto == this.dpto
                       select res).Union(from res in qTR
                                         join solubi in db.Transf_Ubicaciones on res.id_solicitud equals solubi.id_solicitud
                                         where solubi.Transf_Ubicaciones_PropiedadHorizontal.Where(x => x.id_transfubicacion == solubi.id_transfubicacion).Select(y => y.Ubicaciones_PropiedadHorizontal.Depto).Contains(this.dpto)
                                         select res);

            if (!string.IsNullOrEmpty(this.torre))

                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       join solubic in db.CPadron_Ubicaciones on sol.id_cpadron equals solubic.id_cpadron
                       where solubic.Torre == this.torre
                       select res).Union(from res in qTR
                                         join solubic in db.Transf_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                                         where solubic.Torre == this.torre
                                         select res);

            if (!string.IsNullOrEmpty(this.local))

                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       join solubic in db.CPadron_Ubicaciones on sol.id_cpadron equals solubic.id_cpadron
                       where solubic.Local == this.local
                       select res).Union(from res in qTR
                                         join solubic in db.Transf_Ubicaciones on res.id_solicitud equals solubic.id_solicitud
                                         where solubic.Local == this.local
                                         select res);

            //busqueda por Sección / Manzana / Parcela
            if (this.seccion > 0)
                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                       join ubic in db.Ubicaciones on encubic.id_ubicacion equals ubic.id_ubicacion
                       where ubic.Seccion == this.seccion &&
                           (ubic.Manzana == this.manzana || this.manzana.Length == 0) &&
                           (ubic.Parcela == this.parcela || this.parcela.Length == 0)
                       select res).Union(from res in qTR
                                         join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                         where encubic.Ubicaciones.Seccion == this.seccion &&
                                             (encubic.Ubicaciones.Manzana == this.manzana || this.manzana.Length == 0) &&
                                             (encubic.Ubicaciones.Parcela == this.parcela || this.parcela.Length == 0)
                                         select res);

            //busqueda por Ubicaciones Especiales
            if (this.id_sub_tipo_ubicacion > -1)
                // se hace en dos partes porque existe el registro sin especificar y tanto sin especificar como (Todos) comparten el mismo id.
                // dejar en dos partes el select, con y sin id_subtipoubicacion
                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                       join ubic in db.Ubicaciones on encubic.id_ubicacion equals ubic.id_ubicacion
                       join stipubic in db.SubTiposDeUbicacion on ubic.id_subtipoubicacion equals stipubic.id_subtipoubicacion
                       join tipubic in db.TiposDeUbicacion on stipubic.id_tipoubicacion equals tipubic.id_tipoubicacion
                       where tipubic.id_tipoubicacion == this.id_tipo_ubicacion && stipubic.id_subtipoubicacion == this.id_sub_tipo_ubicacion
                       select res).Union(from res in qTR
                                         join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                         where encubic.Ubicaciones.SubTiposDeUbicacion.id_tipoubicacion == this.id_tipo_ubicacion &&
                                                encubic.Ubicaciones.SubTiposDeUbicacion.id_subtipoubicacion == this.id_sub_tipo_ubicacion
                                         select res);

            if (this.id_tipo_ubicacion > -1)
                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       join encubic in db.CPadron_Ubicaciones on sol.id_cpadron equals encubic.id_cpadron
                       join ubic in db.Ubicaciones on encubic.id_ubicacion equals ubic.id_ubicacion
                       join stipubic in db.SubTiposDeUbicacion on ubic.id_subtipoubicacion equals stipubic.id_subtipoubicacion
                       join tipubic in db.TiposDeUbicacion on stipubic.id_tipoubicacion equals tipubic.id_tipoubicacion
                       where tipubic.id_tipoubicacion == this.id_tipo_ubicacion
                       select res).Union(from res in qTR
                                         join encubic in db.Transf_Ubicaciones on res.id_solicitud equals encubic.id_solicitud
                                         where encubic.Ubicaciones.SubTiposDeUbicacion.id_tipoubicacion == this.id_tipo_ubicacion
                                         select res);

            // búsqueda por rubros
            if (this.rubro_desc.Length > 0)
                qTR = (from res in qTR
                       join sol in db.Transf_Solicitudes on res.id_solicitud equals sol.id_solicitud
                       join rub in db.CPadron_Rubros on sol.id_cpadron equals rub.id_cpadron
                       where rub.cod_rubro == this.rubro_desc || rub.desc_rubro.Contains(this.rubro_desc)
                       select res);

            // búsqueda por titulares
            if (this.tit_razon.Length > 0)
            {
                //divdido por espacio y asumo que solo pone como mucho dos nombre y un apellido en cualquier orden
                string[] d = tit_razon.Split(' ');
                string d1 = d[0].Replace(',', ' ').Trim();
                string d2 = d.Length > 1 ? d[1].Replace(',', ' ').Trim() : "";
                string d3 = d.Length > 2 ? d[2].Replace(',', ' ').Trim() : "";
                qTR = (from res in qTR
                       join tit in db.Transf_Titulares_PersonasJuridicas on res.id_solicitud equals tit.id_solicitud
                       where tit.Razon_Social.Contains(this.tit_razon)
                       select res).Union(
                     from res in qTR
                     join tit in db.Transf_Titulares_PersonasFisicas on res.id_solicitud equals tit.id_solicitud
                     where (tit.Apellido.Contains(d1) && (d2.Length == 0 || tit.Nombres.Contains(d2)) && (d3.Length == 0 || tit.Nombres.Contains(d3)))
                     || (tit.Apellido.Contains(d1) && (d2.Length == 0 || tit.Apellido.Contains(d2)) && (d3.Length == 0 || tit.Nombres.Contains(d3)))
                     || ((d2.Length == 0 || tit.Apellido.Contains(d2)) && tit.Nombres.Contains(d1) && (d3.Length == 0 || tit.Nombres.Contains(d3)))
                     || ((d2.Length == 0 || tit.Apellido.Contains(d2)) && tit.Apellido.Contains(d1) && (d3.Length == 0 || tit.Nombres.Contains(d3)))
                     || ((d3.Length == 0 || tit.Apellido.Contains(d3)) && tit.Nombres.Contains(d1) && (d2.Length == 0 || tit.Nombres.Contains(d2)))
                     || ((d3.Length == 0 || tit.Apellido.Contains(d3)) && tit.Apellido.Contains(d1) && (d2.Length == 0 || tit.Nombres.Contains(d2)))
                     select res);
            }

            //busqueda por estado
            if (!string.IsNullOrEmpty(this.estados))
            {
                var arrayEstados = this.estados.Split(',').Select(Int32.Parse).ToList();
                qTR = (from res in qTR
                       where arrayEstados.Contains(res.id_estado)
                       select res);
            }
            #endregion

            //filtrado por superficie
            if (this.SuperficieDesde.HasValue || this.SuperficieHasta.HasValue)
            {
                Decimal? Superficie_Desde = null;
                Decimal? Superficie_Hasta = null;

                if (this.SuperficieDesde.HasValue)
                    Superficie_Desde = this.SuperficieDesde.Value;
                else
                    Superficie_Desde = 0;

                if (this.SuperficieHasta.HasValue)
                    Superficie_Hasta = this.SuperficieHasta.Value;
                else
                    Superficie_Hasta = 99999999;

                qSOL = (from res in qSOL
                        join encsol in db.Encomienda_Transf_Solicitudes on res.id_solicitud equals encsol.id_solicitud
                        join enc in db.Encomienda on encsol.id_encomienda equals enc.id_encomienda
                        join encdl in db.Encomienda_DatosLocal on encsol.id_encomienda equals encdl.id_encomienda
                        where (encdl.superficie_cubierta_dl + encdl.superficie_descubierta_dl >= Superficie_Desde &&
                              encdl.superficie_cubierta_dl + encdl.superficie_descubierta_dl <= Superficie_Hasta)
                        select res);
            }

            AddQueryFinal(qTR, ref qFinal);
            AddQueryFinal(qCP, ref qFinal);
            AddQueryFinal(qSOL, ref qFinal);
            qFinal = qFinal.Distinct();
            totalRowCount = qFinal.Count();

            if (sortByExpression != null)
            {
                if (sortByExpression.Contains("DESC"))
                {
                    if (sortByExpression.Contains("id_solicitud"))
                        qFinal = qFinal.OrderByDescending(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("direccion"))
                        qFinal = qFinal.OrderByDescending(o => o.direccion).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("FechaAsignacion_tarea"))
                        qFinal = qFinal.OrderByDescending(o => o.FechaAsignacion_tarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("FechaInicio_tarea"))
                        qFinal = qFinal.OrderByDescending(o => o.FechaInicio_tarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("nombre_tarea"))
                        qFinal = qFinal.OrderByDescending(o => o.nombre_tarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("superficie_total"))
                        qFinal = qFinal.OrderByDescending(o => o.superficie_total).Skip(startRowIndex).Take(maximumRows);
                    else
                        qFinal = qFinal.OrderByDescending(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);
                }
                else
                {
                    if (sortByExpression.Contains("id_solicitud"))
                        qFinal = qFinal.OrderBy(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("direccion"))
                        qFinal = qFinal.OrderBy(o => o.direccion).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("FechaAsignacion_tarea"))
                        qFinal = qFinal.OrderBy(o => o.FechaAsignacion_tarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("FechaInicio_tarea"))
                        qFinal = qFinal.OrderBy(o => o.FechaInicio_tarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("nombre_tarea"))
                        qFinal = qFinal.OrderBy(o => o.nombre_tarea).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("superficie_total"))
                        qFinal = qFinal.OrderBy(o => o.superficie_total).Skip(startRowIndex).Take(maximumRows);
                    else
                        qFinal = qFinal.OrderBy(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);
                }
            }
            else
                qFinal = qFinal.OrderBy(o => o.FechaInicio_tarea).Skip(startRowIndex).Take(maximumRows);

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
                //Obtener las Direcciones del CP
                //-------------------------------
                List<clsItemDireccion> lstDireccionesCP = new List<clsItemDireccion>();
                string[] arrSolicitudesCP = (from r in resultados
                                             where r.cod_grupotramite == Constants.GruposDeTramite.CP.ToString()
                                             select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesCP.Length > 0)
                    lstDireccionesCP = Shared.GetDireccionesCP(arrSolicitudesCP);

                //------------------------------
                //Obtener las Direcciones del TR
                //-------------------------------
                List<clsItemDireccion> lstDireccionesTR = new List<clsItemDireccion>();
                string[] arrSolicitudesTR = (from r in resultados
                                             where r.cod_grupotramite == Constants.GruposDeTramite.TR.ToString()
                                             && r.id_solicitud <= nroTrReferencia
                                             select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesTR.Length > 0)
                    lstDireccionesTR = Shared.GetDireccionesTR(arrSolicitudesTR);

                string[] arrSolicitudesTRNuevas = (from r in resultados
                                                   where r.cod_grupotramite == Constants.GruposDeTramite.TR.ToString()
                                                   && r.id_solicitud > nroTrReferencia
                                                   select r.id_solicitud.ToString()).ToArray();

                if (arrSolicitudesTRNuevas.Length > 0)
                    lstDireccionesTR.AddRange(Shared.GetDireccionesTRNuevas(arrSolicitudesTRNuevas));

                ////------------------------------------------------------------------------
                //Rellena la clase a devolver con los datos que faltaban (Direccion, dias transcurrido)
                //------------------------------------------------------------------------
                foreach (var row in resultados)
                {
                    clsItemDireccion itemDireccion = null;
                    // ENC
                    if (row.cod_grupotramite == Constants.GruposDeTramite.HAB.ToString())
                    {
                        itemDireccion = lstDireccionesENC.FirstOrDefault(x => x.id_solicitud == row.id_solicitud);
                        var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == row.id_solicitud
                                                && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                        if (enc != null)
                        {
                            var datos = db.Encomienda_DatosLocal.FirstOrDefault(x => x.id_encomienda == enc.id_encomienda);
                            row.superficie_total = datos.superficie_cubierta_dl.Value + datos.superficie_descubierta_dl.Value;
                        }
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
                }
            }

            #endregion
            db.Dispose();

            return resultados;
        }

        private void AddQueryFinal(IQueryable<clsItemBuscarTramite> query, ref IQueryable<clsItemBuscarTramite> qFinal)
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

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniciarEntity();
            //CargarCombo_EstadoTramite();
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();
        }
    }
}
