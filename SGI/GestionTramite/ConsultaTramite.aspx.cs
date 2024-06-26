﻿using SGI.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using ExtensionMethods;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using SGI.StaticClassNameSpace;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SGI.GestionTramite
{
    public partial class ConsultaTramite : BasePage
    {
        private class cls_ultima_tarea
        {
            public int id_solicitud { get; set; }
            public int id_tramitetarea { get; set; }
        }

        private class cls_sol_histest
        {
            public int id_solicitud { get; set; }
            public DateTime fecha_modificacion { get; set; }
        }

        private class cls_sol_tarea_fec
        {
            public int id_solicitud { get; set; }
            public int id_tarea { get; set; }
            public DateTime fecha_inicio { get; set; }
            public DateTime fecha_cierre { get; set; }
            public int dias { get; set; }
        }

        private class cls_sol_tarea_dias
        {
            public int id_solicitud { get; set; }
            public int diasTot { get; set; }
        }

        private class cls_sol_titulares
        {
            public int id_solicitud { get; set; }
            public string mail_titulares { get; set; }
        }

        private class cls_sol_firmantes
        {
            public int id_solicitud { get; set; }
            public string mail_firmantes { get; set; }
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
            var value = Session["ConsultaTramite_IdCalle"] as string;
            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_tramite, updPnlFiltroBuscar_tramite.GetType(),
                     "inicializar_controles", "inicializar_controles();", true);
            }

            CargarCalles();
            if (!IsPostBack)
            {
                if (Session["ConsultaTramite_IdCalle"] != null)
                {
                    Session.Remove("ConsultaTramite_IdCalle");
                    AutocompleteCalles.SelectValueByKey = string.Empty;
                }
                hid_DecimalSeparator.Value = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                LoadData();
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
            {
                this.db = new DGHP_Entities();
                ((IObjectContextAdapter)this.db).ObjectContext.CommandTimeout = 300;
            }
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
            try
            {
                List<int> id = new List<int>();
                // id.Add(0);
                IniciarEntity();
                CargarCombo_PlanoIncendioe();
                CargarCombo_TipoTramite();
                CargarCombo_TipoExpediente(id);
                CargarCombo_subtipoTramite(0);
                CargarCombo_tareas(id, 0, 0);
                CargarCombos();
              
                CargarCombo_GrupoCircuito();
                updPnlFiltroBuscar_tramite.Update();
                updPnlFiltroBuscar_ubi_dom.Update();
                updPnlFiltroBuscar_ubi_smp.Update();
                updRubros.Update();

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
        private void CargarCombo_PlanoIncendioe()
        {

            ListItem listItem = new ListItem();
            listItem.Text = "Todos";
            listItem.Value = "T";
            ddlPlanoIncendio.Items.Add(listItem);

            listItem = new ListItem();
            listItem.Text = "Con Plano de Incendio ";
            listItem.Value = "C";
            ddlPlanoIncendio.Items.Add(listItem);

            listItem = new ListItem();
            listItem.Text = "Sin Plano de Incendio ";
            listItem.Value = "S";
            ddlPlanoIncendio.Items.Add(listItem);


            if (Session["ddlPlanoIncendio_Value"] != null)
            {
                ddlPlanoIncendio.SelectedValue = Convert.ToString(Session["ddlPlanoIncendio_Value"]);
            }

        }
        private void CargarCombo_TipoTramite()
        {
            List<string> lstOcultarTipoTramite = new List<string>();
            lstOcultarTipoTramite.Add("LIGUE");
            lstOcultarTipoTramite.Add("DESLIGUE");

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

        private void CargarCombo_GrupoCircuito()
        {
            List<ENG_Grupos_Circuitos> list_grupoCircuito = this.db.ENG_Grupos_Circuitos.ToList();

            ENG_Grupos_Circuitos grupoCircuito_todos = new ENG_Grupos_Circuitos();
            grupoCircuito_todos.id_grupo_circuito = 0;
            grupoCircuito_todos.cod_grupo_circuito = "Todos";

            list_grupoCircuito.Insert(0, grupoCircuito_todos);
            ddlGrupoCircuito.DataTextField = "cod_grupo_circuito";
            ddlGrupoCircuito.DataValueField = "id_grupo_circuito";

            ddlGrupoCircuito.DataSource = list_grupoCircuito;
            ddlGrupoCircuito.DataBind();
        }

        private void CargarCombo_TipoExpediente(List<int> idtipoTramite)
        {
            List<TipoExpediente> list_tipoTramite = new List<TipoExpediente>();
            if (idtipoTramite.Count == 0)
                list_tipoTramite = this.db.TipoExpediente.ToList();
            else
            {
                var q = (
                            from rel in db.Rel_TipoTramite_TipoExpediente
                            join tipo in db.TipoExpediente on rel.id_tipoexpediente equals tipo.id_tipoexpediente
                            where idtipoTramite.Contains(rel.id_tipotramite)
                            select tipo
                        );
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

        private void CargarCombo_tareas(List<int> idtipoTramite, int idTipoExpediente, int idSubTipoExpediente)
        {
            List<ENG_Tipos_Tareas> lista = new List<ENG_Tipos_Tareas>();

            ENG_Tipos_Tareas tarea = new ENG_Tipos_Tareas();
            tarea.id_tipo_tarea = 0;
            tarea.nombre = "Todas";

            lista.Add(tarea);

            var qTareas = (from ttar in this.db.ENG_Tipos_Tareas
                           select new
                           {
                               id_tipo_tarea = ttar.id_tipo_tarea,
                               nombre_tarea = ttar.nombre
                           }).ToList();

            foreach (var item in qTareas)
            {
                tarea = new ENG_Tipos_Tareas();
                tarea.id_tipo_tarea = item.id_tipo_tarea;
                tarea.nombre = item.nombre_tarea;

                lista.Add(tarea);
            }

            ddlTarea.DataTextField = "nombre";
            ddlTarea.DataValueField = "id_tipo_tarea";

            ddlTarea.DataSource = lista;
            ddlTarea.DataBind();
        }

        private void CargarCombos()
        {
            List<Zonas_Habilitaciones> zonas = this.db.Zonas_Habilitaciones.ToList();
            Zonas_Habilitaciones zona = new Zonas_Habilitaciones();
            zona.id_zonahabilitaciones = -1;
            zona.CodZonaHab = "Todos";
            zonas.Insert(0, zona);

            ddlZona.DataTextField = "CodZonaHab";
            ddlZona.DataValueField = "id_zonahabilitaciones";

            ddlZona.DataSource = zonas;
            ddlZona.DataBind();

            List<Barrios> barrios = this.db.Barrios.ToList();
            Barrios barrio = new Barrios();
            barrio.id_barrio = -1;
            barrio.nom_barrio = "Todos";
            barrios.Insert(0, barrio);

            ddlBarrio.DataTextField = "nom_barrio";
            ddlBarrio.DataValueField = "id_barrio";

            ddlBarrio.DataSource = barrios;
            ddlBarrio.DataBind();

            List<Comunas> comunas = this.db.Comunas.ToList();
            Comunas comuna = new Comunas();
            comuna.id_comuna = -1;
            comuna.nom_comuna = "Todos";
            comunas.Insert(0, comuna);

            ddlComuna.DataTextField = "nom_comuna";
            ddlComuna.DataValueField = "id_comuna";

            ddlComuna.DataSource = comunas;
            ddlComuna.DataBind();

            List<TipoEstadoSolicitud> estados = this.db.TipoEstadoSolicitud.ToList();

            ddlEstado.DataTextField = "Descripcion";
            ddlEstado.DataValueField = "Id";

            ddlEstado.DataSource = estados;
            ddlEstado.DataBind();
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

        #region buscar tramite

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            EjecutarScript(btn_BuscarTramite, "hideResultado();");
            /* if (ddlTipoTramite.Items.Count >= 0)
                 ddlTipoTramite.SelectedIndex = 0;*/

            if (ddlTipoExpediente.Items.Count >= 0)
                ddlTipoExpediente.SelectedIndex = 0;

            if (ddlSubTipoTramite.Items.Count >= 0)
                ddlSubTipoTramite.SelectedIndex = 0;

            if (ddlTarea.Items.Count >= 0)
                ddlTarea.SelectedIndex = 0;

            if (ddlEstado.Items.Count >= 0)
                ddlEstado.SelectedIndex = 0;

            if (ddlGrupoCircuito.Items.Count >= 0)
                ddlGrupoCircuito.SelectedIndex = 0;

            txtFechaInicioDesde.Text = "";
            txtFechaInicioHasta.Text = "";
            txtFechaIngresoDesde.Text = "";
            txtFechaIngresoHasta.Text = "";
            txtFechaHabilitacionDesde.Text = "";
            txtFechaHabilitacionHasta.Text = "";
            txtFechaLibradoUsoDesde.Text = "";
            txtFechaLibradoUsoHasta.Text = "";
            txtSuperficieDesde.Text = "";
            txtSuperficieHasta.Text = "";
            AutocompleteCalles.ClearSelection();
            Session["ConsultaTramite_IdCalle"] = string.Empty;
            txtUbiNroPuertaDesde.Text = "";
            txtUbiNroPuertaHasta.Text = "";
            ddlVereda.SelectedIndex = 0;
            txtUbiSeccion.Text = "";
            txtUbiManzana.Text = "";
            txtUbiParcela.Text = "";
            txtNroSolicitud.Text = "";

            if (ddlZona.Items.Count >= 0)
                ddlZona.SelectedIndex = 0;

            if (ddlBarrio.Items.Count >= 0)
                ddlBarrio.SelectedIndex = 0;

            if (ddlComuna.Items.Count >= 0)
                ddlComuna.SelectedIndex = 0;


            grdRubrosIngresados.DataSource = null;
            grdRubrosIngresados.DataBind();

            updPnlFiltroBuscar_tramite.Update();
            updPnlFiltroBuscar_ubi_dom.Update();
            updPnlFiltroBuscar_ubi_smp.Update();
            updRubros.Update();

            pnlResultadoBuscar.Visible = false;
            updPnlResultadoBuscar.Update();

        }

        private void CargarCalles()
        {
            Functions.CargarAutocompleteCalles(AutocompleteCalles);
        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                this.ReqCalle.Validate();
                IniciarEntity();

                Validar();

                guardarFiltro();
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                //LogError.Write(ex, "error al buscar tramites buscar_tramite-btnBuscar_OnClick");
                Enviar_Mensaje(ex.Message, "");
            }

        }

        public List<clsItemConsultaTramite> GetTramites(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {

            totalRowCount = 0;

            List<clsItemConsultaTramite> lstResult = FiltrarTramitesSP(startRowIndex, maximumRows, sortByExpression, out totalRowCount);

            pnlCantRegistros.Visible = true;

            if (totalRowCount > 1)
            {
                lblCantRegistros.Text = string.Format("{0} Trámites en hoja", lstResult.Count);//lstResult.Count
                lblTotalRegistros.Text = string.Format("{0} Trámites Totales", totalRowCount);//totalRowCount
            }
            else if (totalRowCount == 1)
            {
                lblCantRegistros.Text = string.Format("{0} Trámite", lstResult.Count);
            }
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
            Validar_BuscarPorUbicacion();
            Validar_BuscarPorRubro();
            if (!hayFiltroPorTramite && !hayFiltroPorUbicacion && !hayFiltroPorRubro)
            {
                throw new Exception("Debe ingresar algún filtro de búsqueda.");
            }
        }

        private int? id_tipo_expediente;
        private int? id_sub_tipo_tramite;
        private int? id_tipo_tarea = null;
        private string estados = "";
        private string tiposTramite = "";
        private string gruposCircuito = "";
        private int? id_solicitud = null;

        private DateTime? fechaInicioDesde;
        private DateTime? fechaInicioHasta;
        private DateTime? fechaIngresoDesde;
        private DateTime? fechaIngresoHasta;
        private DateTime? fechaLibradoUsoDesde;
        private DateTime? fechaLibradoUsoHasta;
        private DateTime? fechaHabilitacionDesde;
        private DateTime? fechaHabilitacionHasta;
        private decimal? superficieDesde;
        private decimal? superficieHasta;

        private bool hayFiltroPorTramite = false;
        private void Validar_BuscarPorTramite()
        {
            this.hayFiltroPorTramite = false;

            DateTime fechaAux;
            int idAux = 0;
            decimal supAux = 0.00m;

            this.id_tipo_expediente = null;
            this.id_sub_tipo_tramite = null;
            this.id_tipo_tarea = null;
            this.id_solicitud = null;

            this.fechaInicioDesde = null;
            this.fechaInicioHasta = null;
            this.fechaIngresoDesde = null;
            this.fechaIngresoHasta = null;
            this.fechaLibradoUsoDesde = null;
            this.fechaLibradoUsoHasta = null;
            this.fechaHabilitacionDesde = null;
            this.fechaHabilitacionHasta = null;

            idAux = 0;
            if (int.TryParse(txtNroSolicitud.Text, out idAux))
                this.id_solicitud = idAux;

            idAux = 0;
            if (int.TryParse(ddlTipoExpediente.SelectedItem.Value, out idAux) && idAux > 0)
                this.id_tipo_expediente = idAux;

            idAux = 0;
            if (int.TryParse(ddlSubTipoTramite.SelectedItem.Value, out idAux) && idAux > 0)
                this.id_sub_tipo_tramite = idAux;

            idAux = 0;
            if (int.TryParse(ddlTarea.SelectedItem.Value, out idAux) && idAux > 0)
                this.id_tipo_tarea = idAux;

            supAux = 0.00m;
            if (decimal.TryParse(txtSuperficieDesde.Text, out supAux))
                this.superficieDesde = supAux;

            supAux = 0.00m;
            if (decimal.TryParse(txtSuperficieHasta.Text, out supAux))
                this.superficieHasta = supAux;

            if (!string.IsNullOrEmpty(txtFechaInicioDesde.Text))
            {
                if (!DateTime.TryParse(txtFechaInicioDesde.Text, out fechaAux))
                    throw new Exception("Fecha tarea desde inválida.");

                this.fechaInicioDesde = fechaAux;
            }

            if (!string.IsNullOrEmpty(txtFechaInicioHasta.Text))
            {
                if (!DateTime.TryParse(txtFechaInicioHasta.Text, out fechaAux))
                    throw new Exception("Fecha tarea hasta inválida.");

                this.fechaInicioHasta = fechaAux;
            }
            if (!string.IsNullOrEmpty(txtFechaIngresoDesde.Text))
            {
                if (!DateTime.TryParse(txtFechaIngresoDesde.Text, out fechaAux))
                    throw new Exception("Fecha tarea hasta inválida.");

                this.fechaIngresoDesde = fechaAux;
            }
            if (!string.IsNullOrEmpty(txtFechaIngresoHasta.Text))
            {
                if (!DateTime.TryParse(txtFechaIngresoHasta.Text, out fechaAux))
                    throw new Exception("Fecha tarea hasta inválida.");

                this.fechaIngresoHasta = fechaAux;
            }
            if (!string.IsNullOrEmpty(txtFechaLibradoUsoDesde.Text))
            {
                if (!DateTime.TryParse(txtFechaLibradoUsoDesde.Text, out fechaAux))
                    throw new Exception("Fecha tarea hasta inválida.");

                this.fechaLibradoUsoDesde = fechaAux;
            }
            if (!string.IsNullOrEmpty(txtFechaLibradoUsoHasta.Text))
            {
                if (!DateTime.TryParse(txtFechaLibradoUsoHasta.Text, out fechaAux))
                    throw new Exception("Fecha tarea hasta inválida.");

                this.fechaLibradoUsoHasta = fechaAux;
            }
            if (!string.IsNullOrEmpty(txtFechaHabilitacionDesde.Text))
            {
                if (!DateTime.TryParse(txtFechaHabilitacionDesde.Text, out fechaAux))
                    throw new Exception("Fecha tarea hasta inválida.");

                this.fechaHabilitacionDesde = fechaAux;
            }
            if (!string.IsNullOrEmpty(txtFechaHabilitacionHasta.Text))
            {
                if (!DateTime.TryParse(txtFechaHabilitacionHasta.Text, out fechaAux))
                    throw new Exception("Fecha tarea hasta inválida.");

                this.fechaHabilitacionHasta = fechaAux;
            }

            if (this.fechaInicioDesde.HasValue && this.fechaInicioHasta.HasValue && this.fechaInicioDesde > this.fechaInicioHasta)
                throw new Exception("Fecha de Inicio desde superior a fecha hasta.");

            if (this.fechaIngresoDesde.HasValue && this.fechaIngresoHasta.HasValue && this.fechaIngresoDesde > this.fechaIngresoHasta)
                throw new Exception("Fecha de Ingreso desde superior a fecha hasta.");

            if (this.fechaLibradoUsoDesde.HasValue && this.fechaLibradoUsoHasta.HasValue && this.fechaLibradoUsoDesde > this.fechaLibradoUsoHasta)
                throw new Exception("Fecha de Librado al Usoe desde superior a fecha hasta.");

            if (this.fechaHabilitacionDesde.HasValue && this.fechaHabilitacionHasta.HasValue && this.fechaHabilitacionDesde > this.fechaHabilitacionHasta)
                throw new Exception("Fecha de Habilitación desde superior a fecha hasta.");

            if (this.superficieDesde.HasValue && this.superficieHasta.HasValue && this.superficieDesde.Value > this.superficieHasta.Value)
                throw new Exception("Superficie desde superior a superficie hasta.");

            this.tiposTramite = hid_tipotramite_selected.Value;
            this.estados = hid_estados_selected.Value;
            this.gruposCircuito = hid_grupocircuito_selected.Value;

            if (!string.IsNullOrEmpty(this.tiposTramite) || this.id_tipo_expediente > 0
                || this.id_sub_tipo_tramite > 0 || this.id_tipo_tarea > 0
                || !string.IsNullOrEmpty(this.estados)
                || this.fechaInicioDesde.HasValue || this.fechaInicioHasta.HasValue
                || this.fechaIngresoDesde.HasValue || this.fechaIngresoHasta.HasValue
                || this.fechaLibradoUsoDesde.HasValue || this.fechaLibradoUsoHasta.HasValue
                || this.fechaHabilitacionDesde.HasValue || this.fechaHabilitacionHasta.HasValue
                || this.superficieDesde.HasValue || this.superficieHasta.HasValue
                || this.id_solicitud > 0)
                this.hayFiltroPorTramite = true;
        }

        private int? id_zona;
        private int? id_barrio;
        private int? id_comuna;
        private int? id_calle;
        private int? nro_calle_desde;
        private int? nro_calle_hasta;
        private int? vereda;
        private int? seccion;
        private string manzana = "";
        private string parcela = "";

        private bool hayFiltroPorUbicacion = false;
        private void Validar_BuscarPorUbicacion()
        {
            this.hayFiltroPorUbicacion = false;

            int idAux = 0;

            this.id_zona = null;
            this.id_barrio = null;
            this.id_comuna = null;
            this.id_calle = null;
            this.nro_calle_desde = null;
            this.nro_calle_hasta = null;
            this.vereda = null;

            this.seccion = null;
            this.manzana = "";
            this.parcela = "";

            //filtro por domicilio
            if ((!string.IsNullOrEmpty(txtUbiNroPuertaDesde.Text) && !string.IsNullOrEmpty(txtUbiNroPuertaHasta.Text))
            && ((string.IsNullOrEmpty(Session["ConsultaTramite_IdCalle"] as string)) ? "" : Session["ConsultaTramite_IdCalle"].ToString()) == "")
            {
                throw new Exception("Cuando especifica el número de puerta debe ingresar la calle.");
            }


            idAux = 0;
            if (Session["ConsultaTramite_IdCalle"] != null)
                int.TryParse(Session["ConsultaTramite_IdCalle"].ToString(), out idAux);
            this.id_calle = idAux;

            idAux = 0;
            if (int.TryParse(ddlZona.SelectedValue, out idAux) && idAux > 0)
                this.id_zona = idAux;

            idAux = 0;
            if (int.TryParse(ddlBarrio.SelectedValue, out idAux) && idAux > 0)
                this.id_barrio = idAux;

            idAux = 0;
            if (int.TryParse(ddlComuna.SelectedValue, out idAux) && idAux > 0)
                this.id_comuna = idAux;



            idAux = 0;
            if (int.TryParse(txtUbiNroPuertaDesde.Text.Trim(), out idAux))
                this.nro_calle_desde = idAux;

            idAux = 0;
            if (int.TryParse(txtUbiNroPuertaHasta.Text.Trim(), out idAux))
                this.nro_calle_hasta = idAux;

            idAux = 0;
            if (int.TryParse(ddlVereda.SelectedValue, out idAux) && idAux > 0)
                this.vereda = idAux;

            //filtro por smp
            idAux = 0;
            if (int.TryParse(txtUbiSeccion.Text, out idAux))
                this.seccion = idAux;

            this.manzana = txtUbiManzana.Text.Trim();

            this.parcela = txtUbiParcela.Text.Trim();


            if (this.id_zona > 0 || this.id_barrio > 0 || this.id_comuna > 0 ||
                this.id_calle > 0 || this.nro_calle_desde > 0 || this.nro_calle_hasta > 0 || this.vereda > 0 ||
                this.seccion > 0 || !string.IsNullOrEmpty(this.manzana) || !string.IsNullOrEmpty(this.parcela))
                this.hayFiltroPorUbicacion = true;
        }

        private string rubros = "";
        private bool hayFiltroPorRubro = false;
        private void Validar_BuscarPorRubro()
        {
            this.hayFiltroPorRubro = false;
            var listRubros = GetRubrosCargados();
            if (listRubros.Count() > 0)
                this.rubros = string.Join(",", listRubros.Select(x => x.cod_rubro));
            else
                this.rubros = "";

            if (!string.IsNullOrEmpty(this.rubros))
                this.hayFiltroPorRubro = true;

        }

        #endregion

        private void guardarFiltro()
        {
            var listRubros = GetRubrosCargados();
            string Srubros = "";
            if (listRubros.Count() > 0)
                Srubros = string.Join(",", listRubros.Select(x => x.cod_rubro));

            FiltrosConsulta filtros = new FiltrosConsulta()
            {
                id_solicitud = this.id_solicitud.ToString(),
                id_tipo_tramite = tiposTramite,
                id_tipo_expediente = id_tipo_expediente.ToString(),
                id_sub_tipo_tramite = id_sub_tipo_tramite.ToString(),
                id_tipo_tarea = id_tipo_tarea.ToString(),
                id_estado = hid_estados_selected.Value,
                fechaInicioDesde = txtFechaInicioDesde.Text,
                fechaInicioHasta = txtFechaInicioHasta.Text,
                fechaIngresoDesde = txtFechaIngresoDesde.Text,
                fechaIngresoHasta = txtFechaIngresoHasta.Text,
                fechaLibradoUsoDesde = txtFechaLibradoUsoDesde.Text,
                fechaLibradoUsoHasta = txtFechaLibradoUsoHasta.Text,
                fechaHabilitacionDesde = txtFechaHabilitacionDesde.Text,
                fechaHabilitacionHasta = txtFechaHabilitacionHasta.Text,
                superficieDesde = txtSuperficieDesde.Text,
                superficieHasta = txtSuperficieHasta.Text,
                id_zona = id_zona.ToString(),
                id_barrio = id_barrio.ToString(),
                id_comuna = id_comuna.ToString(),
                id_calle = id_calle.ToString(),
                nro_calle_desde = txtUbiNroPuertaDesde.Text,
                nro_calle_hasta = txtUbiNroPuertaHasta.Text,
                vereda = vereda.ToString(),
                seccion = txtUbiSeccion.Text,
                manzana = txtUbiManzana.Text,
                parcela = txtUbiParcela.Text,
                rubros = Srubros,
                codGrupoCircuito = hid_grupocircuito_selected.Value
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

                Response.Redirect(string.Format("~/GestionTramite/ConsultarTramite" + "/" + "{0}", guidJson), false);
            }

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

            txtNroSolicitud.Text = filtros.id_solicitud.ToString();
            txtFechaInicioDesde.Text = filtros.fechaInicioDesde;
            txtFechaInicioHasta.Text = filtros.fechaInicioHasta;
            txtFechaIngresoDesde.Text = filtros.fechaIngresoDesde;
            txtFechaIngresoHasta.Text = filtros.fechaIngresoHasta;
            txtFechaLibradoUsoDesde.Text = filtros.fechaLibradoUsoDesde;
            txtFechaLibradoUsoHasta.Text = filtros.fechaLibradoUsoHasta;
            txtFechaHabilitacionDesde.Text = filtros.fechaHabilitacionDesde;
            txtFechaHabilitacionHasta.Text = filtros.fechaHabilitacionHasta;
            txtSuperficieDesde.Text = filtros.superficieDesde;
            txtSuperficieHasta.Text = filtros.superficieHasta;


            IniciarEntity();
            List<int> idTipoTramite = new List<int>();
            idTipoTramite = filtros.id_tipo_tramite.Trim() == "" ? idTipoTramite : filtros.id_tipo_tramite.Split(new char[] { ',' }).ToList().Select(x => int.Parse(x)).ToList();
            CargarCombo_TipoExpediente(idTipoTramite);
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();

            ddlTipoExpediente.SelectedIndex = filtros.id_tipo_expediente != "" ? int.Parse(filtros.id_tipo_expediente) : 0;
            IniciarEntity();
            int idTipoExp = int.Parse(ddlTipoExpediente.SelectedValue);
            CargarCombo_subtipoTramite(idTipoExp);
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();

            ddlSubTipoTramite.SelectedIndex = filtros.id_sub_tipo_tramite != "" ? int.Parse(filtros.id_sub_tipo_tramite) : 0;
            IniciarEntity();
            int idSubTipo = int.Parse(ddlSubTipoTramite.SelectedValue);
            CargarCombo_tareas(idTipoTramite, idTipoExp, idSubTipo);
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();


            ddlTarea.SelectedIndex = filtros.id_tipo_tarea != "" ? int.Parse(filtros.id_tipo_tarea) : 0;
            hid_estados_selected.Value = filtros.id_estado;
            hid_tipotramite_selected.Value = filtros.id_tipo_tramite;
            hid_grupocircuito_selected.Value = filtros.codGrupoCircuito;

            if (String.IsNullOrWhiteSpace(filtros.id_calle))
                AutocompleteCalles.SelectValueByKey = "Todos";
            else
                AutocompleteCalles.SelectValueByKey = filtros.id_calle;

            txtUbiNroPuertaDesde.Text = filtros.nro_calle_desde;
            txtUbiNroPuertaHasta.Text = filtros.nro_calle_hasta;
            ddlVereda.SelectedValue = filtros.vereda;
            txtUbiSeccion.Text = filtros.seccion;
            txtUbiManzana.Text = filtros.manzana;
            txtUbiParcela.Text = filtros.parcela;

            if (!String.IsNullOrWhiteSpace(filtros.id_zona))
                ddlZona.SelectedValue = filtros.id_zona;

            ddlBarrio.SelectedIndex = filtros.id_barrio != "" ? int.Parse(filtros.id_barrio) : 0;
            ddlComuna.SelectedIndex = filtros.id_comuna != "" ? int.Parse(filtros.id_comuna) : 0;

            //Carga rubros
            if (!string.IsNullOrEmpty(filtros.rubros))
            {
                var rubros = filtros.rubros.Split(',').ToList();

                var lstRubros = (from rub in db.Rubros
                                 join tipo in db.TipoActividad on rub.id_tipoactividad equals tipo.Id
                                 where rubros.Contains(rub.cod_rubro)
                                 select new clsItemConsultaRubro
                                 {
                                     id_rubro = rub.id_rubro,
                                     cod_rubro = rub.cod_rubro,
                                     nom_rubro = rub.nom_rubro,
                                     tipo_actividad = tipo.Nombre,
                                     id_subrubro = "",
                                     nom_subrubro = ""
                                 }).Union(from rub in db.RubrosCN
                                          join tipo in db.TipoActividad on rub.IdTipoActividad equals tipo.Id
                                          join subr in db.RubrosCN_Subrubros on rub.IdRubro equals subr.Id_rubroCN into relrub
                                          from rs in relrub.DefaultIfEmpty()
                                          where rubros.Contains(rub.Codigo)
                                          select new clsItemConsultaRubro
                                          {
                                              id_rubro = rub.IdRubro,
                                              cod_rubro = rub.Codigo,
                                              nom_rubro = rub.Nombre,
                                              tipo_actividad = tipo.Nombre,
                                              id_subrubro = (rs != null) ? rs.Id_rubroCNsubrubro.ToString() : "",
                                              nom_subrubro = (rs != null) ? rs.Nombre : ""
                                          }).ToList();

                grdRubrosIngresados.DataSource = lstRubros;
                grdRubrosIngresados.DataBind();
            }

            int idAux = 0;
            if (int.TryParse(filtros.id_solicitud, out idAux))
                this.id_solicitud = idAux;

            this.tiposTramite = filtros.id_tipo_tramite;

            if (int.TryParse(filtros.id_tipo_expediente, out idAux) && idAux > 0)
                this.id_tipo_expediente = idAux;

            if (int.TryParse(filtros.id_sub_tipo_tramite, out idAux) && idAux > 0)
                this.id_sub_tipo_tramite = idAux;

            if (int.TryParse(filtros.id_tipo_tarea, out idAux) && idAux > 0)
                this.id_tipo_tarea = idAux;

            this.estados = filtros.id_estado;

            if (!string.IsNullOrEmpty(filtros.fechaInicioDesde))
                this.fechaInicioDesde = Convert.ToDateTime(filtros.fechaInicioDesde);

            if (!string.IsNullOrEmpty(filtros.fechaInicioHasta))
                this.fechaInicioHasta = Convert.ToDateTime(filtros.fechaInicioHasta);

            if (!string.IsNullOrEmpty(filtros.fechaIngresoDesde))
                this.fechaIngresoDesde = Convert.ToDateTime(filtros.fechaIngresoDesde);

            if (!string.IsNullOrEmpty(filtros.fechaIngresoHasta))
                this.fechaIngresoHasta = Convert.ToDateTime(filtros.fechaIngresoHasta);

            if (!string.IsNullOrEmpty(filtros.fechaLibradoUsoDesde))
                this.fechaLibradoUsoDesde = Convert.ToDateTime(filtros.fechaLibradoUsoDesde);

            if (!string.IsNullOrEmpty(filtros.fechaLibradoUsoHasta))
                this.fechaLibradoUsoHasta = Convert.ToDateTime(filtros.fechaLibradoUsoHasta);

            if (!string.IsNullOrEmpty(filtros.fechaHabilitacionDesde))
                this.fechaHabilitacionDesde = Convert.ToDateTime(filtros.fechaHabilitacionDesde);

            if (!string.IsNullOrEmpty(filtros.fechaHabilitacionHasta))
                this.fechaHabilitacionHasta = Convert.ToDateTime(filtros.fechaHabilitacionHasta);

            decimal supAux = 0.00m;
            if (decimal.TryParse(filtros.superficieDesde, out supAux))
                this.superficieDesde = supAux;

            if (decimal.TryParse(filtros.superficieHasta, out supAux))
                this.superficieHasta = supAux;

            if (int.TryParse(filtros.id_zona, out idAux) && idAux > 0)
                this.id_zona = idAux;

            if (int.TryParse(filtros.id_barrio, out idAux) && idAux > 0)
                this.id_barrio = idAux;

            if (int.TryParse(filtros.id_comuna, out idAux) && idAux > 0)
                this.id_comuna = idAux;

            if (int.TryParse(filtros.id_calle, out idAux) && idAux > 0)
                this.id_calle = idAux;

            if (!string.IsNullOrWhiteSpace(filtros.nro_calle_desde))
                this.nro_calle_desde = int.Parse(filtros.nro_calle_desde);

            if (!string.IsNullOrWhiteSpace(filtros.nro_calle_hasta))
                this.nro_calle_hasta = int.Parse(filtros.nro_calle_hasta);

            if (int.TryParse(filtros.vereda, out idAux) && idAux > 0)
                this.vereda = idAux;

            if (!string.IsNullOrWhiteSpace(filtros.seccion))
                this.seccion = int.Parse(filtros.seccion);


            this.manzana = filtros.manzana;
            this.parcela = filtros.parcela;
            if (!string.IsNullOrWhiteSpace(filtros.id_zona))
                this.id_zona = int.Parse(filtros.id_zona);

            if (!string.IsNullOrWhiteSpace(filtros.id_barrio))
                this.id_barrio = int.Parse(filtros.id_barrio);

            if (!string.IsNullOrWhiteSpace(filtros.id_comuna))
                this.id_comuna = int.Parse(filtros.id_comuna);

            this.rubros = filtros.rubros;

            this.estados = filtros.id_estado;

            this.gruposCircuito = filtros.codGrupoCircuito;

            return true;
        }

        protected void ddlTipoTramite_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniciarEntity();
            List<int> idTipoTramite = new List<int>();
            idTipoTramite = hid_tipotramite_selected.Value.ToString().Trim() == "" ? idTipoTramite : hid_tipotramite_selected.Value.Split(new char[] { ',' }).ToList().Select(x => int.Parse(x)).ToList();
            CargarCombo_TipoExpediente(idTipoTramite);
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();
        }

        protected void ddlTipoExpediente_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniciarEntity();
            int idTipoExp = int.Parse(ddlTipoExpediente.SelectedValue);
            CargarCombo_subtipoTramite(idTipoExp);
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();
        }

        protected void ddlSubTipoTramite_SelectedIndexChanged(object sender, EventArgs e)
        {
            IniciarEntity();
            List<int> idTipoTramite = new List<int>();
            idTipoTramite = hid_tipotramite_selected.Value.ToString().Trim() == "" ? idTipoTramite : hid_tipotramite_selected.Value.Split(new char[] { ',' }).ToList().Select(x => int.Parse(x)).ToList();
            int idTipoExp = int.Parse(ddlTipoExpediente.SelectedValue);
            int idSubTipo = int.Parse(ddlSubTipoTramite.SelectedValue);
            CargarCombo_tareas(idTipoTramite, idTipoExp, idSubTipo);
            FinalizarEntity();
            updPnlFiltroBuscar_tramite.Update();
        }

        #region Filtro rubros

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                this.ReqCalle.Validate();
                if (!this.ReqCalle.IsValid)
                    Session["ConsultaTramite_IdCalle"] = string.Empty;
                ValidadorAgregarRubros.Style["display"] = "none";

                var lstRubros = (from rub in db.Rubros
                                 join tipo in db.TipoActividad on rub.id_tipoactividad equals tipo.Id
                                 where rub.cod_rubro.Contains(txtBuscar.Text.Trim())
                                 || rub.nom_rubro.Contains(txtBuscar.Text.Trim())
                                 select new clsItemConsultaRubro
                                 {
                                     id_rubro = rub.id_rubro,
                                     cod_rubro = rub.cod_rubro,
                                     nom_rubro = rub.nom_rubro,
                                     tipo_actividad = tipo.Nombre,
                                     id_subrubro = "",
                                     nom_subrubro = ""
                                 }).Union(from rub in db.RubrosCN
                                          join tipo in db.TipoActividad on rub.IdTipoActividad equals tipo.Id
                                          join subr in db.RubrosCN_Subrubros on rub.IdRubro equals subr.Id_rubroCN into relrub
                                          from rs in relrub.DefaultIfEmpty()
                                          where rub.Codigo.Contains(txtBuscar.Text.Trim())
                                          || rub.Nombre.Contains(txtBuscar.Text.Trim())
                                          select new clsItemConsultaRubro
                                          {
                                              id_rubro = rub.IdRubro,
                                              cod_rubro = rub.Codigo,
                                              nom_rubro = rub.Nombre,
                                              tipo_actividad = tipo.Nombre,
                                              id_subrubro = (rs != null) ? rs.Id_rubroCNsubrubro.ToString() : "",
                                              nom_subrubro = (rs != null) ? rs.Nombre : ""
                                          })
                                 .ToList();
                grdRubros.DataSource = lstRubros;
                grdRubros.DataBind();

                pnlBuscarRubros.Style["display"] = "none";
                pnlBotonesBuscarRubros.Style["display"] = "none";
                pnlResultadoBusquedaRubros.Style["display"] = "block";
                pnlGrupoAgregarRubros.Style["display"] = "block";

                if (lstRubros.Count == 0)
                    pnlBotonesAgregarRubros.Style["display"] = "none";
                else
                    pnlBotonesAgregarRubros.Style["display"] = "block";

                updBotonesBuscarRubros.Update();
                updBotonesAgregarRubros.Update();

            }
            catch (Exception ex)
            {
                //lblError.Text = Functions.GetErrorMessage(ex);
                Functions.EjecutarScript(updBotonesBuscarRubros, "showfrmError_Rubros();");
            }
            finally
            {
                db.Dispose();
            }
        }

        protected void btnnuevaBusqueda_Click(object sender, EventArgs e)
        {
            pnlResultadoBusquedaRubros.Style["display"] = "none";
            pnlBotonesAgregarRubros.Style["display"] = "none";
            pnlGrupoAgregarRubros.Style["display"] = "none";
            pnlBuscarRubros.Style["display"] = "block";
            pnlBotonesBuscarRubros.Style["display"] = "block";
            BotonesBuscarRubros.Style["display"] = "block";
            txtBuscar.Text = "";
            ValidadorAgregarRubros.Style["display"] = "none";
            txtBuscar.Focus();

            updBotonesBuscarRubros.Update();
            updBotonesAgregarRubros.Update();
        }

        protected void btnIngresarRubros_Click(object sender, EventArgs e)
        {
            List<clsItemConsultaRubro> listDoc = GetRubrosCargados();
            db = new DGHP_Entities();

            int CantRubrosElegidos = 0;

            ValidadorAgregarRubros.Style["display"] = "none";

            foreach (GridViewRow row in grdRubros.Rows)
            {
                CheckBox chkRubroElegido = (CheckBox)row.FindControl("chkRubroElegido");
                if (chkRubroElegido.Checked)
                {
                    CantRubrosElegidos++;
                    clsItemConsultaRubro item = new clsItemConsultaRubro();
                    item.id_rubro = (int)grdRubros.DataKeys[row.RowIndex].Values["id_rubro"];
                    item.cod_rubro = grdRubros.DataKeys[row.RowIndex].Values["cod_rubro"].ToString();
                    item.nom_rubro = grdRubros.DataKeys[row.RowIndex].Values["nom_rubro"].ToString();
                    item.tipo_actividad = grdRubros.DataKeys[row.RowIndex].Values["tipo_actividad"].ToString();
                    item.id_subrubro = (grdRubros.DataKeys[row.RowIndex].Values["id_subrubro"] == null) ? string.Empty : grdRubros.DataKeys[row.RowIndex].Values["id_subrubro"].ToString();
                    item.nom_subrubro = (grdRubros.DataKeys[row.RowIndex].Values["nom_subrubro"] == null) ? string.Empty : grdRubros.DataKeys[row.RowIndex].Values["nom_subrubro"].ToString();

                    listDoc.Add(item);
                }
            }
            grdRubrosIngresados.DataSource = listDoc;
            grdRubrosIngresados.DataBind();
            updRubros.Update();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmAgregarRubros_Rubros", "$('#frmAgregarRubros_Rubros').modal('hide');", true);
        }

        protected void btnEliminarRubro_Click(object sender, EventArgs e)
        {
            List<clsItemConsultaRubro> listDoc = GetRubrosCargados();
            LinkButton btnEliminar = (LinkButton)sender;
            int id_rubro = int.Parse(btnEliminar.CommandArgument);
            foreach (var item in listDoc)
                if (item.id_rubro == id_rubro)
                {
                    listDoc.Remove(item);
                    break;
                }

            grdRubrosIngresados.DataSource = listDoc;
            grdRubrosIngresados.DataBind();
            updRubros.Update();
        }
        private List<clsItemConsultaRubro> GetRubrosCargados()
        {
            List<clsItemConsultaRubro> lstResult = new List<clsItemConsultaRubro>();
            foreach (GridViewRow row in grdRubrosIngresados.Rows)
            {
                clsItemConsultaRubro item = new clsItemConsultaRubro();
                item.id_rubro = (int)grdRubrosIngresados.DataKeys[row.RowIndex].Values["id_rubro"];
                item.cod_rubro = grdRubrosIngresados.DataKeys[row.RowIndex].Values["cod_rubro"].ToString();
                item.nom_rubro = grdRubrosIngresados.DataKeys[row.RowIndex].Values["nom_rubro"].ToString();
                item.tipo_actividad = grdRubrosIngresados.DataKeys[row.RowIndex].Values["tipo_actividad"].ToString();
                item.id_subrubro = (grdRubrosIngresados.DataKeys[row.RowIndex].Values["id_subrubro"] == null) ? string.Empty : grdRubrosIngresados.DataKeys[row.RowIndex].Values["id_subrubro"].ToString();
                item.nom_subrubro = (grdRubrosIngresados.DataKeys[row.RowIndex].Values["nom_subrubro"] == null) ? string.Empty : grdRubrosIngresados.DataKeys[row.RowIndex].Values["nom_subrubro"].ToString();

                lstResult.Add(item);
            }

            return lstResult;
        }
        #endregion

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
        private List<clsItemConsultaTramite> FiltrarTramitesSP(int startRowIndex, int maximumRows, string sortByExpression, out int totalRowCount)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-ES");
            if (!String.IsNullOrWhiteSpace(codigoGuid))
            {
                int idAux = 0;
                if (int.TryParse(txtNroSolicitud.Text, out idAux))
                    this.id_solicitud = idAux;

                this.tiposTramite = hid_tipotramite_selected.Value;

                if (int.TryParse(ddlTipoExpediente.SelectedValue, out idAux) && idAux > 0)
                    this.id_tipo_expediente = idAux;

                if (int.TryParse(ddlSubTipoTramite.SelectedValue, out idAux) && idAux > 0)
                    this.id_sub_tipo_tramite = idAux;

                if (int.TryParse(ddlTarea.SelectedValue, out idAux) && idAux > 0)
                    this.id_tipo_tarea = idAux;

                this.estados = hid_estados_selected.Value;

                if (string.IsNullOrEmpty(txtFechaInicioDesde.Text))
                    this.fechaInicioDesde = null;
                else
                    this.fechaInicioDesde = Convert.ToDateTime(txtFechaInicioDesde.Text);

                if (string.IsNullOrEmpty(txtFechaInicioHasta.Text))
                    this.fechaInicioHasta = null;
                else
                    this.fechaInicioHasta = Convert.ToDateTime(txtFechaInicioHasta.Text);

                if (string.IsNullOrEmpty(txtFechaIngresoDesde.Text))
                    this.fechaIngresoDesde = null;
                else
                    this.fechaIngresoDesde = Convert.ToDateTime(txtFechaIngresoDesde.Text);

                if (string.IsNullOrEmpty(txtFechaIngresoHasta.Text))
                    this.fechaIngresoHasta = null;
                else
                    this.fechaIngresoHasta = Convert.ToDateTime(txtFechaIngresoHasta.Text);

                if (string.IsNullOrEmpty(txtFechaLibradoUsoDesde.Text))
                    this.fechaLibradoUsoDesde = null;
                else
                    this.fechaLibradoUsoDesde = Convert.ToDateTime(txtFechaLibradoUsoDesde.Text);

                if (string.IsNullOrEmpty(txtFechaLibradoUsoHasta.Text))
                    this.fechaLibradoUsoHasta = null;
                else
                    this.fechaLibradoUsoHasta = Convert.ToDateTime(txtFechaLibradoUsoHasta.Text);

                if (string.IsNullOrEmpty(txtFechaHabilitacionDesde.Text))
                    this.fechaHabilitacionDesde = null;
                else
                    this.fechaHabilitacionDesde = Convert.ToDateTime(txtFechaHabilitacionDesde.Text);

                if (string.IsNullOrEmpty(txtFechaHabilitacionHasta.Text))
                    this.fechaHabilitacionHasta = null;
                else
                    this.fechaHabilitacionHasta = Convert.ToDateTime(txtFechaHabilitacionHasta.Text);

                decimal supAux = 0.00m;
                if (decimal.TryParse(txtSuperficieDesde.Text, out supAux))
                    this.superficieDesde = supAux;

                if (decimal.TryParse(txtSuperficieHasta.Text, out supAux))
                    this.superficieHasta = supAux;

                if (int.TryParse(ddlZona.SelectedValue, out idAux) && idAux > 0)
                    this.id_zona = idAux;

                if (int.TryParse(ddlBarrio.SelectedValue, out idAux) && idAux > 0)
                    this.id_barrio = idAux;

                if (int.TryParse(ddlComuna.SelectedValue, out idAux) && idAux > 0)
                    this.id_comuna = idAux;

                if (int.TryParse(AutocompleteCalles.SelectValueByKey, out idAux) && idAux > 0)
                    this.id_calle = idAux;

                if (string.IsNullOrWhiteSpace(txtUbiNroPuertaDesde.Text))
                    this.nro_calle_desde = null;
                else
                    this.nro_calle_desde = int.Parse(txtUbiNroPuertaDesde.Text);

                if (string.IsNullOrWhiteSpace(txtUbiNroPuertaHasta.Text))
                    this.nro_calle_hasta = null;
                else
                    this.nro_calle_hasta = int.Parse(txtUbiNroPuertaHasta.Text);

                if (int.TryParse(ddlVereda.SelectedValue, out idAux) && idAux > 0)
                    this.vereda = idAux;

                if (string.IsNullOrWhiteSpace(txtUbiSeccion.Text))
                    this.seccion = null;
                else
                    this.seccion = int.Parse(txtUbiSeccion.Text);


                this.manzana = txtUbiManzana.Text;
                this.parcela = txtUbiParcela.Text;
                var listRubros = GetRubrosCargados();
                this.rubros = "";
                if (listRubros.Count() > 0)
                    this.rubros = string.Join(",", listRubros.Select(x => x.cod_rubro));

                this.gruposCircuito = hid_grupocircuito_selected.Value;
            }
            else
            {
                totalRowCount = 0;
                return new List<clsItemConsultaTramite>();
            }

            using (DGHP_Entities db = new DGHP_Entities())
            {
                db.Database.CommandTimeout = 300;
                var tramites = new List<clsItemConsultaTramite>();
                string ids_tipo_tramites = null;
                string ids_grupo_circuitos = null;
                string ids_estados = null;

                List<int> tipoTramite = new List<int>();
                if (this.tiposTramite.Trim().Length > 0)
                    tipoTramite = this.tiposTramite.Split(new char[] { ',' }).ToList()
                                                .Where(x => int.Parse(x) > 0)
                                                .Select(s => int.Parse(s)).ToList();

                if (tipoTramite.Contains((int)Constants.TipoDeTramite.Habilitacion) && !tipoTramite.Contains((int)Constants.TipoDeTramite.RectificatoriaHabilitacion))
                    tipoTramite.Add((int)Constants.TipoDeTramite.RectificatoriaHabilitacion);

                ids_tipo_tramites = string.Join(",", tipoTramite.ToArray());

                List<int> grupoCircuito = new List<int>();
                if (this.gruposCircuito.Trim().Length > 0)
                    grupoCircuito = this.gruposCircuito.Split(new char[] { ',' }).ToList()
                                                        .Where(x => int.Parse(x) > 0)
                                                        .Select(s => int.Parse(s)).ToList();

                ids_grupo_circuitos = string.Join(",", grupoCircuito.ToArray());

                List<int> estadosL = new List<int>();
                if (this.estados.Trim().Length > 0)
                    estadosL = this.estados.Split(new char[] { ',' }).ToList()
                                                .Where(x => int.Parse(x) > 0)
                                                .Select(s => int.Parse(s)).ToList();

                ids_estados = this.estados;

                var listRubros = GetRubrosCargados();
                this.rubros = "";
                if (listRubros.Count() > 0)
                    this.rubros = string.Join(",", listRubros.Select(x => x.cod_rubro).Distinct());

                string planoIncendio = ddlPlanoIncendio.SelectedValue;

                var cantResultados = new System.Data.Entity.Core.Objects.ObjectParameter("recordCount", typeof(int));

                List<SGI_ConsultaTramites> resultados = db.ConsultaTramites(
                    id_solicitud,
                    ids_tipo_tramites,
                    id_tipo_expediente,
                    id_sub_tipo_tramite,
                    id_tipo_tarea,
                    ids_estados,
                    fechaInicioDesde,
                    fechaInicioHasta,
                    fechaIngresoDesde,
                    fechaIngresoHasta,
                    fechaLibradoUsoDesde,
                    fechaLibradoUsoHasta,
                    fechaHabilitacionDesde,
                    fechaHabilitacionHasta,
                    superficieDesde,
                    superficieHasta,
                    id_zona,
                    id_barrio,
                    id_comuna,
                    id_calle,
                    nro_calle_desde,
                    nro_calle_hasta,
                    vereda,
                    seccion,
                    manzana,
                    parcela,
                    ids_grupo_circuitos,
                    rubros,
                    planoIncendio,
                    startRowIndex,
                    maximumRows,
                    cantResultados
                    ).ToList();

                totalRowCount = (int)cantResultados.Value;

                var q = (from sol in resultados
                         join gc in db.ENG_Grupos_Circuitos on sol.id_grupo_circuito equals gc.id_grupo_circuito into gcGroup
                         from gc in gcGroup.DefaultIfEmpty()
                         orderby sol.id_solicitud
                         select new
                         {
                             Solicitud = sol,
                             GrupoCircuito = gc
                         }).ToList();

                tramites = q.Select(item => new clsItemConsultaTramite
                {
                    cod_grupotramite = item.GrupoCircuito !=null ? item.GrupoCircuito.cod_grupo_circuito : string.Empty,
                    id_solicitud = item.Solicitud.id_solicitud,
                    id_aux = item.Solicitud.id_encomienda ?? 0,
                    FechaInicio = item.Solicitud.FechaInicio,
                    FechaIngreso = item.Solicitud.FechaIngreso,
                    id_tipotramite = item.Solicitud.id_tipotramite,
                    TipoTramite = item.Solicitud.TipoTramite,
                    id_tipoexpediente = item.Solicitud.id_tipoexpediente,
                    TipoExpediente = item.Solicitud.TipoExpediente,
                    id_subtipoexpediente = item.Solicitud.id_subtipoexpediente,
                    SubTipoExpediente = item.Solicitud.SubTipoExpediente,
                    TipoCAA = item.Solicitud.TipoCAA,
                    TareaActual = item.Solicitud.TareaActual,
                    FechaCreacionTareaActual = item.Solicitud.FechaCreacionTareaActual,
                    FechaAsignacionTareaActual = item.Solicitud.FechaAsignacionTareaActual,
                    GrupoCircuito = item.GrupoCircuito != null ? item.GrupoCircuito.nom_grupo_circuito : string.Empty,
                    Superficie = item.Solicitud.Superficie ?? 0.00m,
                    id_estado = item.Solicitud.id_estado,
                    Estado = item.Solicitud.Estado,
                    Calificador = item.Solicitud.Calificador,
                    ProfesionalAnexoTecnico = item.Solicitud.ProfesionalAnexoTecnico,
                    ProfesionalAnexoNotarial = item.Solicitud.ProfesionalAnexoNotarial,
                    FechaLibrado = item.Solicitud.FechaLibrado,
                    FechaHabilitacion = item.Solicitud.FechaHabilitacion,
                    FechaRechazo = item.Solicitud.FechaRechazo,
                    NumeroExp = item.Solicitud.NumeroExp,
                    Observaciones = item.Solicitud.Observaciones,
                    idCAA = item.Solicitud.idCAA,
                    idSIPSA = item.Solicitud.idSIPSA,
                    idEncomienda = item.Solicitud.id_encomienda,
                    TipoNormativa = item.Solicitud.TipoNormativa,
                    Organismo = item.Solicitud.Organismo,
                    NroNormativa = item.Solicitud.NroNormativa,
                    FechaBaja = item.Solicitud.FechaBaja,
                    FechaCaducidad = item.Solicitud.FechaCaducidad,
                    DiasEnCorreccion = item.Solicitud.DiasEnCorreccion,
                    MailFirmantes = GetCadenaLimpia(item.Solicitud.MailFirmantes),
                    MailTitulares = GetCadenaLimpia(item.Solicitud.MailTitulares),
                    MailUsuarioSSIT = GetCadenaLimpia(item.Solicitud.MailUsuarioSSIT),
                    MailUsuarioTAD = GetCadenaLimpia(item.Solicitud.MailUsuarioTAD),
                    PlantasHabilitar = item.Solicitud.PlantasHabilitar,
                    Usuario = item.Solicitud.Usuario,
                    NombreyApellido = item.Solicitud.NombreyApellido,
                    FechaInicioAT = item.Solicitud.FechaInicioAT,
                    FechaAprobadoAT = item.Solicitud.FechaAprobadoAT,
                    TienePlanoIncendio = item.Solicitud.TienePlanoIncendio,

                    Ubicaciones = new List<clsItemConsultaUbicacion>(),
                    Rubros = new List<clsItemddlRubro>(),
                    Titulares = new List<clsItemConsulta>(),
                    Cuits = new List<clsItemConsulta>(),
                }).ToList();

                //cargo las listas antes del foreach
                var idSolicitudesSet = new HashSet<int>(tramites.Select(r => r.id_solicitud));
                var idTipoTramitesSet = new HashSet<int>(tramites.Select(r => r.id_tipotramite));
                var lstConsTram = db.SGI_ConsultaTramites
                                .Where(x => idSolicitudesSet.Contains(x.id_solicitud) && idTipoTramitesSet.Contains(x.id_tipotramite))
                                .ToList();
                foreach (var r in tramites)
                {
                    var matchingConsTram = lstConsTram
                    .Where(x => x.id_solicitud == r.id_solicitud && x.id_tipotramite == r.id_tipotramite)
                    .ToList();

                    r.Ubicaciones = matchingConsTram
                    .GroupBy(a => new { a.Zona, a.Barrio, a.UnidadFuncional, a.Seccion, a.Manzana, a.Parcela, a.NroPartidaMatriz, a.NroPartidaHorizontal, a.SubTipoUbicacion, a.LocalSubTipoUbicacion })
                    .Select(x => new clsItemConsultaUbicacion()
                    {
                        Zona = x.Key.Zona,
                        Barrio = x.Key.Barrio,
                        UnidadFuncional = x.Key.UnidadFuncional,
                        Seccion = x.Key.Seccion,
                        Manzana = x.Key.Manzana,
                        Parcela = x.Key.Parcela,
                        PartidaMatriz = x.Key.NroPartidaMatriz,
                        PartidaHorizontal = x.Key.NroPartidaHorizontal,
                        SubTipoUbicacion = x.Key.SubTipoUbicacion,
                        LocalSubTipoUbicacion = x.Key.LocalSubTipoUbicacion,

                        Calles = matchingConsTram
                        .GroupBy(a => new { a.nombre_calle, a.NroPuerta })
                        .Select(u => new clsItemConsultaPuerta
                        {
                            calle = u.Key.nombre_calle,
                            puerta = u.Key.NroPuerta ?? 0
                        }).ToList()

                    }).ToList();


                    r.Rubros = matchingConsTram
                        .GroupBy(a => new { a.id_rubro, a.cod_rubro, a.nom_rubro, a.id_subrubro, a.nom_subrubro })
                        .Select(p => new clsItemddlRubro
                        {
                            id_rubro = p.Key.id_rubro ?? 0,
                            cod_rubro = p.Key.cod_rubro,
                            nom_rubro = p.Key.nom_rubro,
                            id_subrubro = p.Key.id_subrubro != null ? p.Key.id_subrubro.ToString() : "",
                            nom_subrubro = p.Key.nom_subrubro
                        }).ToList();

                    r.Titulares = matchingConsTram
                        .GroupBy(a => new { a.Titulares })
                        .Select(x => new clsItemConsulta { value = GetCadenaLimpia(x.Key.Titulares) }).ToList();

                    r.Cuits = matchingConsTram
                        .GroupBy(a => new { a.Cuits })
                        .Select(x => new clsItemConsulta { value = GetCadenaLimpia(x.Key.Cuits) }).ToList();

                    var sol = db.SSIT_Solicitudes.Where(s => s.id_solicitud == r.id_solicitud).FirstOrDefault();

                    if (sol == null)
                    {
                        var trf = db.Transf_Solicitudes.Where(t => t.id_solicitud == r.id_solicitud).FirstOrDefault();
                        r.id_solicitud_ref = trf != null ? trf.idSolicitudRef : null;
                    }
                    else
                    {
                        r.id_solicitud_ref = sol.SSIT_Solicitudes_Origen?.id_solicitud_origen;
                    }

                }

                if (tramites.Count == 0)
                    totalRowCount = 0;

                return tramites;
            }
        }


        private string GetCadenaLimpia(string str)
        {
            string s = "";

            var aux = str.Split(';');

            foreach (var a in aux)
            {
                if (a.Trim().Length > 0 && a.Trim() != ",")
                    s += a + " ";
            }

            return s;
        }

        #endregion

        #region "Exporta a Excel"

        protected void ddlPlanoIncendio_SelectedIndexChanged(object sender, EventArgs e)
        {

            Session["ddlPlanoIncendio_Value"] = ddlPlanoIncendio.SelectedValue.ToString();
        }

        protected void btnExportarExcel_Click(object sender, EventArgs e)
        {

            btnCerrarExportacion.Visible = false;
            //ahora crea el excel con la fecha en su nombre
            string fecha = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            string fileName = string.Format("Grilla-Solicitudes-{0}.xlsx", fecha);

            pnlDescargarExcel.Style["display"] = "none";
            pnlExportandoExcel.Style["display"] = "block";

            Session["exportacion_en_proceso"] = true;
            Session["progress_data"] = "Preparando exportación.";
            Session["filename_exportacion"] = fileName;

            lblRegistrosExportados.Text = "Preparando exportación.";
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ExportarGrillaAExcelAsync));
            thread.Start();
            Timer1.Enabled = true;

        }

        private void ExportarGrillaAExcelAsync()
        {

            decimal cant_registros_x_vez = 0m;
            int totalRowCount = 0;
            int startRowIndex = 0;
            string fechaInicio = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            try
            {
                DateTime startTime = DateTime.Now;
                LogToTemporalFile("Export started at: " + startTime.ToString());
                // Esto se realiza para saber el total y de a cuanto se va mostrar el progreso.

                FiltrarTramitesSP(startRowIndex, 1, "", out totalRowCount);
                if (totalRowCount < 10000)
                    cant_registros_x_vez = 10000m;
                else
                    cant_registros_x_vez = 50000m;

                int cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                
                List<clsItemConsultaTramite> resultados = new List<clsItemConsultaTramite>();

                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados.AddRange(FiltrarTramitesSP(startRowIndex, Convert.ToInt32(cant_registros_x_vez), "", out totalRowCount));
                    Session["progress_data"] = string.Format("{0} trámites exportados.", resultados.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }

                Session["progress_data"] = string.Format("{0} trámites exportados.", resultados.Count);
                DataTable dt;
                if (rbtRubro.Checked)
                {
                    var lstExportar = (from res in resultados
                                       from rub in res.Rubros.DefaultIfEmpty()
                                       select new
                                       {
                                           Solicitud = res.id_solicitud,
                                           SolicitudReferencia = res.id_solicitud_ref,
                                           res.FechaInicio,
                                           res.FechaIngreso,
                                           res.TipoTramite,
                                           res.TipoExpediente,
                                           res.SubTipoExpediente,
                                           res.TipoCAA,
                                           res.TareaActual,
                                           res.FechaCreacionTareaActual,
                                           res.FechaAsignacionTareaActual,
                                           res.GrupoCircuito,
                                           res.Superficie,
                                           res.Estado,
                                           res.FechaLibrado,
                                           res.FechaHabilitacion,
                                           res.FechaRechazo,
                                           res.FechaBaja,
                                           res.FechaCaducidad,
                                           res.NumeroExp,
                                           res.Calificador,
                                           res.ProfesionalAnexoTecnico,
                                           res.ProfesionalAnexoNotarial,
                                           CodigoRubro = rub != null ? rub.cod_rubro : "",
                                           DescripcionRubro = rub != null ? rub.nom_rubro : "",
                                           CodigoSubRubro = rub != null ? rub.id_subrubro : "",
                                           DescripcionSubRubro = rub != null ? rub.nom_subrubro : "",
                                           Zona = string.Join(";", res.Ubicaciones.Select(x => x.Zona)),
                                           Barrio = string.Join(";", res.Ubicaciones.Select(x => x.Barrio)),
                                           UnidadFuncional = string.Join(";", res.Ubicaciones.Select(x => x.UnidadFuncional)),
                                           Seccion = string.Join(";", res.Ubicaciones.Select(x => x.Seccion)),
                                           Manzana = string.Join(";", res.Ubicaciones.Select(x => x.Manzana)),
                                           Parcela = string.Join(";", res.Ubicaciones.Select(x => x.Parcela)),
                                           SubTipoUbicacion = string.Join(";", res.Ubicaciones.Select(x => x.SubTipoUbicacion)),
                                           LocalSubTipoUbicacion = string.Join(";", res.Ubicaciones.Select(x => x.LocalSubTipoUbicacion)),
                                           Partida_Matriz = string.Join(";", res.Ubicaciones.Select(x => x.PartidaMatriz)),
                                           PartidaHorizontal = string.Join(";", res.Ubicaciones.Select(x => x.PartidaHorizontal)),
                                           Calles = string.Join(";", res.Ubicaciones.Select(x => string.Join(";", x.Calles.Select(y => y.calle + " " + y.puerta)))),
                                           Titulares = string.Join(";", res.Titulares.Select(x => x.value)),
                                           Cuits = string.Join(";", res.Cuits.Select(x => x.value)),
                                           Observaciones = res.Observaciones,
                                           idEncomienda = res.idEncomienda,
                                           idCAA = res.idCAA,
                                           idSIPSA = res.idSIPSA,
                                           TipoNormativa = res.TipoNormativa,
                                           Organismo = res.Organismo,
                                           NroNormativa = res.NroNormativa,
                                           DiasEnCorreccion = res.DiasEnCorreccion,
                                           MailFirmantes = GetCadenaLimpia(res.MailFirmantes),
                                           MailTitulares = GetCadenaLimpia(res.MailTitulares),
                                           MailUsuarioSSIT = GetCadenaLimpia(res.MailUsuarioSSIT),
                                           MailUsuarioTAD = GetCadenaLimpia(res.MailUsuarioTAD),
                                           PlantasHabilitar = res.PlantasHabilitar,
                                           Usuario = res.Usuario,
                                           NombreApellido = res.NombreyApellido,
                                           FechaInicioAT = res.FechaInicioAT,
                                           FechaAprobadoAT = res.FechaAprobadoAT,
                                           TienePlanoIncendio = res.TienePlanoIncendio
                                       }).ToList();
                    dt = Functions.ToDataTable(lstExportar);
                }
                else if (rbtDomicilio.Checked)
                {
                    var lstExportar = (from res in resultados
                                       from ubi in res.Ubicaciones.DefaultIfEmpty()
                                       let cal = (ubi != null ? ubi.Calles.FirstOrDefault() : null) // Usar FirstOrDefault() para obtener una única calle o null si no hay
                                       select new
                                       {
                                           Solicitud = res.id_solicitud,
                                           SolicitudReferencia = res.id_solicitud_ref,
                                           res.FechaInicio,
                                           res.FechaIngreso,
                                           res.TipoTramite,
                                           res.TipoExpediente,
                                           res.SubTipoExpediente,
                                           res.TipoCAA,
                                           res.TareaActual,
                                           res.FechaCreacionTareaActual,
                                           res.FechaAsignacionTareaActual,
                                           res.GrupoCircuito,
                                           res.Superficie,
                                           res.Estado,
                                           res.FechaLibrado,
                                           res.FechaHabilitacion,
                                           res.FechaRechazo,
                                           res.FechaBaja,
                                           res.FechaCaducidad,
                                           res.NumeroExp,
                                           Rubros = string.Join(";", res.Rubros.Select(x => x.cod_rubro + ": " + x.nom_rubro)),
                                           Zona = ubi != null ? ubi.Zona ?? "" : "Campo Vacio",
                                           Barrio = ubi != null ? ubi.Barrio ?? "" : "Campo Vacio",
                                           UnidadFuncional = ubi != null ? ubi.UnidadFuncional ?? "" : "Campo Vacio",
                                           Seccion = ubi != null ? ubi.Seccion : 0,
                                           Manzana = ubi != null ? ubi.Manzana ?? "" : "Campo Vacio",
                                           Parcela = ubi != null ? ubi.Parcela ?? "" : "Campo Vacio",
                                           SubTipoUbicacion = ubi != null ? ubi.SubTipoUbicacion ?? "" : "Campo Vacio",
                                           LocalSubTipoUbicacion = ubi != null ? ubi.LocalSubTipoUbicacion ?? "" : "Campo Vacio",
                                           Partida_Matriz = ubi != null ? ubi.PartidaMatriz : 0,
                                           PartidaHorizontal = ubi != null ? ubi.PartidaHorizontal : 0,
                                           Calle = cal != null ? cal.calle ?? "" : "Campo Vacio",
                                           NumeroCalle = cal != null ? cal.puerta : 0,
                                           Titulares = string.Join(";", res.Titulares.Select(x => x.value)),
                                           Cuits = string.Join(";", res.Cuits.Select(x => x.value)),
                                           Observaciones = res.Observaciones,
                                           idEncomienda = res.idEncomienda,
                                           idCAA = res.idCAA,
                                           idSIPSA = res.idSIPSA,
                                           TipoNormativa = res.TipoNormativa,
                                           Organismo = res.Organismo,
                                           NroNormativa = res.NroNormativa,
                                           DiasEnCorreccion = res.DiasEnCorreccion,
                                           MailFirmantes = GetCadenaLimpia(res.MailFirmantes),
                                           MailTitulares = GetCadenaLimpia(res.MailTitulares),
                                           MailUsuarioSSIT = GetCadenaLimpia(res.MailUsuarioSSIT),
                                           MailUsuarioTAD = GetCadenaLimpia(res.MailUsuarioTAD),
                                           PlantasHabilitar = res.PlantasHabilitar,
                                           Usuario = res.Usuario,
                                           NombreApellido = res.NombreyApellido,
                                           FechaInicioAT = res.FechaInicioAT,
                                           FechaAprobadoAT = res.FechaAprobadoAT,
                                           TienePlanoIncendio = res.TienePlanoIncendio
                                       }).ToList();
                    dt = Functions.ToDataTable(lstExportar);
                }
                else
                {
                    var lstExportar = (from res in resultados
                   from rub in res.Rubros.DefaultIfEmpty()
                   from ubi in res.Ubicaciones.DefaultIfEmpty()
                   let cal = ubi != null ? ubi.Calles.FirstOrDefault() : null
                   select new
                   {
                       Solicitud = res.id_solicitud,
                       SolicitudReferencia = res.id_solicitud_ref,
                       res.FechaInicio,
                       res.FechaIngreso,
                       res.TipoTramite,
                       res.TipoExpediente,
                       res.SubTipoExpediente,
                       res.TipoCAA,
                       res.TareaActual,
                       res.FechaCreacionTareaActual,
                       res.FechaAsignacionTareaActual,
                       res.GrupoCircuito,
                       res.Superficie,
                       res.Estado,
                       res.FechaLibrado,
                       res.FechaHabilitacion,
                       res.FechaRechazo,
                       res.FechaBaja,
                       res.FechaCaducidad,
                       res.NumeroExp,
                       CodigoRubro = rub != null ? rub.cod_rubro : "",
                       DescripcionRubro = rub != null ? rub.nom_rubro : "",
                       CodigoSubRubro = rub != null ? rub.id_subrubro : "",
                       DescripcionSubRubro = rub != null ? rub.nom_subrubro : "",
                       Zona = ubi != null ? ubi.Zona ?? "Campo Vacio" : "Campo Vacio",
                       Barrio = ubi != null ? ubi.Barrio ?? "Campo Vacio" : "Campo Vacio",
                       UnidadFuncional = ubi != null ? ubi.UnidadFuncional ?? "Campo Vacio" : "Campo Vacio",
                       Seccion = ubi != null ? ubi.Seccion : 0,
                       Manzana = ubi != null ? ubi.Manzana ?? "Campo Vacio" : "Campo Vacio",
                       Parcela = ubi != null ? ubi.Parcela ?? "Campo Vacio" : "Campo Vacio",
                       SubTipoUbicacion = ubi != null ? ubi.SubTipoUbicacion ?? "Campo Vacio" : "Campo Vacio",
                       LocalSubTipoUbicacion = ubi != null ? ubi.LocalSubTipoUbicacion ?? "Campo Vacio" : "Campo Vacio",
                       Partida_Matriz = ubi != null ? ubi.PartidaMatriz : 0,
                       PartidaHorizontal = ubi != null ? ubi.PartidaHorizontal : 0,
                       Calle = cal != null ? cal.calle ?? "Campo Vacio" : "Campo Vacio",
                       NumeroCalle = cal != null ? cal.puerta : 0,
                       Titulares = string.Join(";", res.Titulares.Select(x => x.value)),
                       Cuits = string.Join(";", res.Cuits.Select(x => x.value)),
                       Observaciones = res.Observaciones,
                       idEncomienda = res.idEncomienda,
                       idCAA = res.idCAA,
                       idSIPSA = res.idSIPSA,
                       TipoNormativa = res.TipoNormativa,
                       Organismo = res.Organismo,
                       NroNormativa = res.NroNormativa,
                       DiasEnCorreccion = res.DiasEnCorreccion,
                       MailFirmantes = GetCadenaLimpia(res.MailFirmantes),
                       MailTitulares = GetCadenaLimpia(res.MailTitulares),
                       MailUsuarioSSIT = GetCadenaLimpia(res.MailUsuarioSSIT),
                       MailUsuarioTAD = GetCadenaLimpia(res.MailUsuarioTAD),
                       PlantasHabilitar = res.PlantasHabilitar,
                       Usuario = res.Usuario,
                       NombreApellido = res.NombreyApellido,
                       FechaInicioAT = res.FechaInicioAT,
                       FechaAprobadoAT = res.FechaAprobadoAT,
                       TienePlanoIncendio = res.TienePlanoIncendio
                   }).ToList();
dt = Functions.ToDataTable(lstExportar);
                }

                // Convierte la lista en un dataset
                DataSet ds = new DataSet();
                dt.TableName = "Solicitudes";
                ds.Tables.Add(dt);
                string path = Constants.Path_Temporal;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string savedFileName = path + Session["filename_exportacion"].ToString();
                Functions.EliminarArchivosDirectorioTemporal();
                Functions.ExportDataSetToExcel(ds, savedFileName);

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AppendHeader("Content-Disposition", "attachment; filename=file.xlsx");

                // Write the Excel file content to the response output stream
                Response.TransmitFile(savedFileName);
                Response.End();

                // quita la variable de session.
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
                Session["progress_data"] = "Exportación completada.";
                string fechaFin = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                LogError.Write(new Exception("Fecha Inicio: " + fechaInicio + " Fecha Final:" + fechaFin));
            }
            catch (TimeoutException tex)
            {
                LogToTemporalFile("Error de exportacion: " + tex.ToString());
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
                LogError.Write(tex);
                throw new Exception("Error de exportacion: Intente nuevamente.");
            }
            catch (Exception ex)
            {
                LogToTemporalFile("Error de exportacion: " + ex.ToString());
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
                LogError.Write(ex);
            }
        }

        private void LogToTemporalFile(string message)
        {
            string fechaAct = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            string FileName = string.Format("log_timer_{0}.txt", fechaAct);
            string path = @"C:\Temporal\";
            string logFilePath = Path.Combine($"{path}", $"{FileName}");

            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                sw.WriteLine(message);
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                bool exportacion_en_proceso = (Session["exportacion_en_proceso"] != null ? (bool)Session["exportacion_en_proceso"] : false);

                if (exportacion_en_proceso)
                    lblRegistrosExportados.Text = Convert.ToString(Session["progress_data"]);
                else
                {
                    Timer1.Enabled = false;
                    btnCerrarExportacion.Visible = true;
                    pnlDescargarExcel.Style["display"] = "block";
                    pnlExportandoExcel.Style["display"] = "none";
                    string filename = Session["filename_exportacion"].ToString();
                    filename = HttpUtility.UrlEncode(filename);
                    btnDescargarExcel.NavigateUrl = string.Format("~/Controls/DescargarArchivoTemporal.aspx?fname={0}", filename);
                    Session.Remove("filename_exportacion");
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                throw new Exception("Error de exportacion: Intente nuevamente.");
                //Timer1.Enabled = false;
            }

        }

        protected void btnCerrarExportacion_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            Session.Remove("filename_exportacion");
            Session.Remove("progress_data");
            Session.Remove("exportacion_en_proceso");
            pnlDescargarExcel.Style["display"] = "none";
            pnlExportandoExcel.Style["display"] = "block";

            this.EjecutarScript(updExportaExcel, "hidefrmExportarExcel();");
        }

        #endregion

        protected void AutocompleteCalles_ValueSelect(object sender, Syncfusion.JavaScript.Web.AutocompleteSelectEventArgs e)
        {
            Session["ConsultaTramite_IdCalle"] = e.Key;
            return;
        }
    }
}
