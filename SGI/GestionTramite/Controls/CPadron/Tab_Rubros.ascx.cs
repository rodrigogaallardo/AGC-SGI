using SGI.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls.CPadron
{
    public partial class Tab_Rubros : System.Web.UI.UserControl
    {
        public delegate void EventHandlerRubrosActualizado(object sender, EventArgs e);
        public event EventHandlerRubrosActualizado RubrosActualizado;

       

        private class zonasPla
        {
            public string CodZonaPla { get; set; }
            public string DescripcionZonaPla { get; set; }
        }
        private class itemRubros
        {
            public int id_caarubro { get; set; }
            public string ZonaDeclarada { get; set; }
            public string cod_rubro { get; set; }
            public string desc_rubro { get; set; }
            public int id_ImpactoAmbiental { get; set; }
            public string cod_ImpactoAmbiental { get; set; }
            public string nom_ImpactoAmbiental { get; set; }
            public decimal SuperficieHabilitar { get; set; }
            public string TipoActividadNombre { get; set; }
            public string nombre_tipocertificado { get; set; }
            public string BarrioAntena { get; set; }
            public string RestriccionZona { get; set; }
            public string RestriccionSuperficie { get; set; }
            public string TipoTamite { get; set; }
            public int IdTipodocReq {get;set;}
            public int IdTipoActividad { get; set; }
        }
        private class itemResultadoBusquedaRubros
        {
            public string cod_rubro { get; set; }
            public string nom_rubro { get; set; }
            public bool PregAntenaEmisora { get; set; }
            public bool EsAnterior { get; set; }
            public string TipoActividad { get; set; }
            public string RestriccionZona { get; set; }
            public string RestriccionSuperficie { get; set; }
            public string ZonaDeclarada { get; set; }
            public decimal SuperficieHabilitar { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this.Page);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updAgregarNormativa, updAgregarNormativa.GetType(), "init_JS_updAgregarNormativa", "init_JS_updAgregarNormativa();", true);
                ScriptManager.RegisterStartupScript(updBuscarRubros, updBuscarRubros.GetType(), "init_JS_updBuscarRubros", "init_JS_updBuscarRubros();", true);
                ScriptManager.RegisterStartupScript(updInformacionTramite, updInformacionTramite.GetType(), "init_JS_updInformacionTramite", "init_JS_updInformacionTramite();", true);
            }


            if (!IsPostBack)
            {
                hid_return_url.Value = Request.Url.AbsoluteUri;
                hid_DecimalSeparator.Value = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            }
            HabilitarEdicion(true);

        }

        public void HabilitarEdicion(bool valor) {
            ddlZonaDeclarada.Enabled = valor;
        }

        private int validar_estado
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_validar_estado.Value, out ret);
                return ret;
            }
            set
            {
                hid_validar_estado.Value = value.ToString();
            }

        }
        private int id_cpadron
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_id_cpadron.Value, out ret);
                return ret;
            }
            set
            {
                hid_id_cpadron.Value = value.ToString();
            }

        }

        private int id_encomienda
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_id_encomienda.Value, out ret);
                return ret;
            }
            set
            {
                hid_id_encomienda.Value = value.ToString();
            }

        }


        private bool editar
        {
            get
            {
                bool ret = false;
                ret = hid_editar.Value.Equals("True") ? true : false;
                return ret;
            }
            set
            {
                hid_editar.Value = value.ToString();
            }

        }

        public void CargarDatos(int id_cpadron, int id_encomienda, int validar_estado, bool Editar)
        {
            try
            {


                this.id_cpadron = id_cpadron;
                this.id_encomienda = id_encomienda;
                this.validar_estado = validar_estado;
                this.editar = Editar;

                CargarZonas(id_cpadron);
                CargarDatosTramite(id_cpadron);
                CargarRubros(id_cpadron);

                CargarDocumentosRequeridos();
                CargarTipoActividad();



                if (!this.editar)
                {
                    box_editarRubros.Visible = false;
                    box_infoRubros.Visible = false;
                    titulo.Visible = false;
                }
                else
                {
                    box_MostrarRubros.Visible = false;
                }
                updInformacionTramite.Update();
                updRubros.Update();
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "Procedimiento CargarDatos en Tab_Rubros.aspx");
                //Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_Rubros.aspx"));
                throw ex;
            }

        }


        private void CargarDatosTramite(int id_cpadron)
        {
            DGHP_Entities db = new DGHP_Entities();
            var datloc = db.CPadron_DatosLocal.FirstOrDefault(x => x.id_cpadron == id_cpadron);
            if (datloc != null)
            {
                decimal sup = (datloc.superficie_cubierta_dl.HasValue ? datloc.superficie_cubierta_dl.Value : 0) +
                                        (datloc.superficie_descubierta_dl.HasValue ? datloc.superficie_descubierta_dl.Value : 0);
                lblSuperficieLocal.Text = Convert.ToString(sup) + " m2";
                hid_Superficie_Local.Value = Convert.ToString(sup);
            }

            db.Dispose();

        }

        private void CargarZonas(int id_cpadron)
        {
            DGHP_Entities db = new DGHP_Entities();
            List<zonasPla> lstZonasCombo = new List<zonasPla>();
            string codZonaDeclarada = "";

            var sol = db.CPadron_Solicitudes.FirstOrDefault(x => x.id_cpadron == this.id_cpadron);

            if (sol != null)
            {
                codZonaDeclarada = sol.ZonaDeclarada;
            }

            var lstZonasUbic = (from zona in db.Zonas_Planeamiento
                                orderby zona.CodZonaPla
                                select new zonasPla
                                {
                                    CodZonaPla = zona.CodZonaPla,
                                    DescripcionZonaPla = zona.CodZonaPla + " - " + zona.DescripcionZonaPla
                                }).ToList();


            ddlZonaDeclarada.DataSource = lstZonasUbic;
            ddlZonaDeclarada.DataTextField = "DescripcionZonaPla";
            ddlZonaDeclarada.DataValueField = "CodZonaPla";
            ddlZonaDeclarada.DataBind();

            ddlZonaDeclarada.SelectedValue = codZonaDeclarada;

            db.Dispose();

        }


        private void CargarRubros(int id_cpadron)
        {
            DGHP_Entities db = new DGHP_Entities();

            // poner todo lo necesario al cargar la página

            var lstRubrosSolicitud = (from sol in db.CPadron_Solicitudes
                                      join caarub in db.CPadron_Rubros on sol.id_cpadron equals caarub.id_cpadron
                                      join tact in db.TipoActividad on caarub.id_tipoactividad equals tact.Id
                                      //join tcer in db.CAA_TiposDeCertificados on sol.id_tipocertificado equals tcer.id_tipocertificado
                                      //join ia in db.ImpactoAmbiental on caarub.id_ImpactoAmbiental equals ia.id_ImpactoAmbiental
                                      //join barrio in db.CAA_Barrios_ViviendaColectiva on caarub.id_barriovcol equals barrio.id_barriovcol
                                      join rub in db.Rubros on caarub.cod_rubro equals rub.cod_rubro into leftRubros
                                      from lrub in leftRubros.DefaultIfEmpty()
                                      where sol.id_cpadron == id_cpadron
                                      select new itemRubros
                                      {
                                          id_caarubro = caarub.id_cpadronrubro,
                                          ZonaDeclarada = sol.ZonaDeclarada,
                                          cod_rubro = caarub.cod_rubro,
                                          desc_rubro = caarub.desc_rubro,
                                          //id_ImpactoAmbiental = caarub.id_ImpactoAmbiental.Value,
                                          //cod_ImpactoAmbiental = ia.cod_ImpactoAmbiental,
                                          //nom_ImpactoAmbiental = ia.nom_ImpactoAmbiental,
                                          SuperficieHabilitar = caarub.SuperficieHabilitar,
                                          TipoActividadNombre = tact.Descripcion,
                                          RestriccionZona = "pregunta.png",
                                          RestriccionSuperficie = "pregunta.png",
                                          TipoTamite = (caarub.id_tipodocreq == 1 ? "DJ" : (caarub.id_tipodocreq == 2 ? "PP" : "IP")),
                                          IdTipodocReq = caarub.id_tipodocreq,
                                          IdTipoActividad = caarub.id_tipoactividad,
                                      }).ToList();

            var lstRubrosCNSolicitud =  (from sol in db.CPadron_Solicitudes
                                        join caarub in db.CPadron_RubrosCN on sol.id_cpadron equals caarub.id_cpadron
                                        join tact in db.TipoActividad on caarub.id_tipoactividad equals tact.Id
                                        join rub in db.RubrosCN on caarub.cod_rubro equals rub.Codigo into leftRubros
                                        from lrub in leftRubros.DefaultIfEmpty()
                                        where sol.id_cpadron == id_cpadron
                                        select new itemRubros
                                        {
                                            id_caarubro = caarub.id_cpadronrubrocn,
                                            ZonaDeclarada = sol.ZonaDeclarada,
                                            cod_rubro = caarub.cod_rubro,
                                            desc_rubro = caarub.desc_rubro,
                                            SuperficieHabilitar = caarub.SuperficieHabilitar,
                                            TipoActividadNombre = tact.Descripcion,
                                            RestriccionZona = "pregunta.png",
                                            RestriccionSuperficie = "pregunta.png",
                                            TipoTamite = (caarub.id_tipodocreq == 1 ? "DJ" : (caarub.id_tipodocreq == 2 ? "PP" : "IP")),
                                            IdTipodocReq = caarub.id_tipodocreq,
                                            IdTipoActividad = caarub.id_tipoactividad,
                                        }).ToList();

            var lstRubrosCNEncomeinda = (from r in db.RubrosCN
                                        join er in db.Encomienda_RubrosCN on r.IdRubro equals er.IdRubro
                                        join ta in db.TipoActividad on r.IdTipoActividad equals ta.Id
                                        join e in db.Encomienda on er.id_encomienda equals e.id_encomienda
                                        join edl in db.Encomienda_DatosLocal on e.id_encomienda equals edl.id_encomienda
                                        join ets in db.Encomienda_Transf_Solicitudes on e.id_encomienda equals ets.id_encomienda
                                        join ts in db.Transf_Solicitudes on ets.id_solicitud equals ts.id_solicitud
                                        where ts.id_cpadron == id_cpadron
                                        select new itemRubros
                                        {
                                            id_caarubro = r.IdRubro,
                                            ZonaDeclarada = e.ZonaDeclarada,
                                            cod_rubro = r.Codigo,
                                            desc_rubro = r.Nombre,
                                            SuperficieHabilitar = (edl.superficie_cubierta_dl + edl.superficie_descubierta_dl).Value,
                                            TipoActividadNombre = ta.Descripcion,
                                            RestriccionZona = "pregunta.png",
                                            RestriccionSuperficie = "pregunta.png",
                                            TipoTamite = (e.id_tipotramite == 1 ? "DJ" : (e.id_tipotramite == 2 ? "PP" : "IP")),
                                            IdTipodocReq = 1,
                                            IdTipoActividad = ta.Id,
                                        }).ToList().Distinct();

            foreach (var item in lstRubrosSolicitud)
            {
                List<Rubros_ConsRestricciones_Result> lstres = db.Rubros_ConsRestricciones(item.ZonaDeclarada, item.cod_rubro, item.SuperficieHabilitar).ToList();
                foreach (var restriccion in lstres)
                {
                    switch (restriccion.Zona_ok.Value)
                    {
                        case 0:
                            item.RestriccionZona = "imoon imoon-blocked color-red";
                            break;
                        case 1:
                            item.RestriccionZona = "imoon imoon-ok color-green";
                            break;
                        case 2:
                            item.RestriccionZona = "imoon imoon-question color-gray";
                            break;
                    }

                    switch (restriccion.Superficie_ok.Value)
                    {
                        case 0:
                            item.RestriccionSuperficie = "imoon imoon-blocked color-red";
                            break;
                        case 1:
                            item.RestriccionSuperficie = "imoon imoon-ok color-green";
                            break;
                        case 2:
                            item.RestriccionSuperficie = "imoon imoon-question color-gray";
                            break;
                    }
                }
            }

            lstRubrosSolicitud.AddRange(lstRubrosCNSolicitud);
            lstRubrosSolicitud.AddRange(lstRubrosCNEncomeinda);

            grdRubrosIngresados.DataSource = lstRubrosSolicitud.Distinct();
            grdRubrosIngresados.DataBind();

            grdRubrosMostrar.DataSource = lstRubrosSolicitud.Distinct();
            grdRubrosMostrar.DataBind();

            db.Dispose();
            updRubros.Update();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                ValidadorAgregarRubros.Style["display"] = "none";

                decimal dSuperficieDeclarada = 0;
                string zonaDeclarada = "";
                //string[] roles = Roles.GetRolesForUser();
                bool soloAPRA = true;

                decimal.TryParse(txtSuperficie.Text, out dSuperficieDeclarada);

                var sol = db.CPadron_Solicitudes.FirstOrDefault(x => x.id_cpadron == this.id_cpadron);
                if (sol != null)
                    zonaDeclarada = (!string.IsNullOrEmpty(sol.ZonaDeclarada) ? sol.ZonaDeclarada : "");


                // para la carga especial se permiten todos los Rubros
                soloAPRA = false;

                List<Rubros_RubrosCN_PorBusquedaCPadron_Result> lstRubrosEncontrados = db.Rubros_RubrosCN_PorBusquedaCPadron(txtBuscar.Text.Trim(), false, soloAPRA).ToList();

                List<itemResultadoBusquedaRubros> lstRubros = (from ruben in lstRubrosEncontrados
                                                               select new itemResultadoBusquedaRubros
                                                               {
                                                                   ZonaDeclarada = zonaDeclarada,
                                                                   cod_rubro = ruben.cod_rubro,
                                                                   nom_rubro = ruben.nom_rubro,
                                                                   SuperficieHabilitar = dSuperficieDeclarada,
                                                                   TipoActividad = ruben.TipoActividad,
                                                                   PregAntenaEmisora = ruben.PregAntenaEmisora.Value,
                                                                   RestriccionZona = "pregunta.png",
                                                                   RestriccionSuperficie = "pregunta.png"
                                                               }).ToList();

                foreach (var item in lstRubros)
                {
                    List<Rubros_ConsRestricciones_Result> lstres = db.Rubros_ConsRestricciones(zonaDeclarada, item.cod_rubro, dSuperficieDeclarada).ToList();

                    foreach (var restriccion in lstres)
                    {
                        switch (restriccion.Zona_ok.Value)
                        {
                            case 0:
                                item.RestriccionZona = "imoon imoon-blocked color-red";
                                break;
                            case 1:
                                item.RestriccionZona = "imoon imoon-ok color-green";
                                break;
                            case 2:
                                item.RestriccionZona = "imoon imoon-question color-gray";
                                break;
                        }

                        switch (restriccion.Superficie_ok.Value)
                        {
                            case 0:
                                item.RestriccionSuperficie = "imoon imoon-blocked color-red";
                                break;
                            case 1:
                                item.RestriccionSuperficie = "imoon imoon-ok color-green";
                                break;
                            case 2:
                                item.RestriccionSuperficie = "imoon imoon-question color-gray";
                                break;
                        }
                    }
                }

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
                lblError.Text = Functions.GetErrorMessage(ex);
                Functions.EjecutarScript(updBotonesBuscarRubros, "showfrmError_Rubros();");
            }
            finally
            {
                db.Dispose();
            }
        }

       
        protected void btnnuevaBusqueda_Click(object sender, EventArgs e)
        {
            txtSuperficie.Text = hid_Superficie_Local.Value;
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

        DGHP_Entities db;

        protected void btnIngresarRubros_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();

            decimal dSuperficie = 0;
            int CantRubrosElegidos = 0;
            bool AntenaEmisora = false;
            int id_barriovcol = 0;

            ValidadorAgregarRubros.Style["display"] = "none";

            if (ddlZonaDeclarada.SelectedValue.Trim().Length.Equals(0))
            {
                lblValidadorAgregarRubros.Text = "Para ingresar rubros en el trámite es necesario haber seleccionado la Zona antes de ingresar un rubro.";
                ValidadorAgregarRubros.Style["display"] = "inline-block";
                return;
            }

            // Verifica si se selecciono algún rubro.
            foreach (GridViewRow row in grdRubros.Rows)
            {
                CheckBox chkRubroElegido = (CheckBox)row.FindControl("chkRubroElegido");
                if (chkRubroElegido.Checked)
                    CantRubrosElegidos++;
            }

            if (CantRubrosElegidos == 0)
            {
                lblValidadorAgregarRubros.Text = "Debe seleccionar los rubros/actividades que desea ingresar en el trámite.";
                ValidadorAgregarRubros.Style["display"] = "inline-block";
                return;
            }


            try
            {

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {

                        foreach (GridViewRow row in grdRubros.Rows)
                        {
                            CheckBox chkRubroElegido = (CheckBox)row.FindControl("chkRubroElegido");
                            DropDownList ddlBarrio = (DropDownList)row.FindControl("ddlBarrio");
                            RadioButton optAntenaSI = (RadioButton)row.FindControl("optAntenaSI");
                            RadioButton optAntenaNO = (RadioButton)row.FindControl("optAntenaNO");
                            Panel pnlAntenas = (Panel)row.FindControl("pnlAntenas");
                            Panel pnlBarrio = (Panel)row.FindControl("pnlBarrio");

                            if (chkRubroElegido.Checked)
                            {

                                string scod_rubro = grdRubros.DataKeys[row.RowIndex].Values["cod_rubro"].ToString();
                                decimal.TryParse(grdRubros.DataKeys[row.RowIndex].Values["SuperficieHabilitar"].ToString(), out dSuperficie);

                                if (!pnlAntenas.Style["display"].Equals("none"))
                                    AntenaEmisora = optAntenaSI.Checked;

                                if (!pnlBarrio.Style["display"].Equals("none"))
                                    id_barriovcol = int.Parse(ddlBarrio.SelectedValue);

                                db.CPadron_AgregarRubro(this.id_cpadron, scod_rubro, dSuperficie, this.validar_estado);

                            }
                        }
                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                CargarDatosTramite(this.id_cpadron);
                updInformacionTramite.Update();

                CargarRubros(this.id_cpadron);

                // Hace Fire al evento de Actualización si el mismo está definido.
                if (RubrosActualizado != null)
                    RubrosActualizado(sender, e);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmAgregarRubros_Rubros", "$('#frmAgregarRubros_Rubros').modal('hide');", true);
                //actualizar(updBotonesAgregarRubros, "hidefrmAgregarRubros_Rubros();");
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                Functions.EjecutarScript(updBotonesAgregarRubros, "showfrmError_Rubros();");

            }
            finally
            {
                db.Dispose();
            }

        }

        protected void ddlZonaDeclarada_SelectedIndexChanged(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.CPadron_ActualizarZonaDeclarada(this.id_cpadron, ddlZonaDeclarada.SelectedValue);
            db.Dispose();
            CargarRubros(this.id_cpadron);

        }

        protected void btnEliminarRubro_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            try
            {

                int id_cpadronrubro = int.Parse(hid_id_caarubro_eliminar.Value);
                db.CPadron_EliminarRubro(id_cpadronrubro, this.validar_estado);

                //rubros de la encomienda
                if(hid_id_encomienda.Value !="" || hid_id_encomienda.Value != "0")
                    db.EncomiendaRubroCN_EliminarRubro(int.Parse(hid_id_encomienda.Value), this.id_cpadron, id_cpadronrubro);

                CargarRubros(this.id_cpadron);
                CargarDatosTramite(this.id_cpadron);

                updInformacionTramite.Update();

                // Hace Fire al evento de Actualización si el mismo está definido.
                if (RubrosActualizado != null)
                    RubrosActualizado(sender, e);

                //actualizar(updRubros, "hidefrmConfirmarEliminarRubro_Rubros();");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmConfirmarEliminarRubro_Rubros", "$('#frmConfirmarEliminarRubro_Rubros').modal('hide');", true);
            }
            catch (Exception ex)
            {
                LogError.Write(ex, ex.Message);
                lblError.Text = Functions.GetErrorMessage(ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmError_Rubros", "$('#frmError_Rubros').modal('show');", true);
            }
            finally
            {
                db.Dispose();
            }
        }

        private void actualizar(UpdatePanel up, string scriptName)
        {
            var objSol = (from sol in db.CPadron_Solicitudes
                          where sol.id_cpadron == id_cpadron
                          select new
                          {
                              sol.id_subtipoexpediente
                          }).FirstOrDefault();
            string visibleConformacionLocal = "visibleConformacionLocal(false);";
            if (objSol.id_subtipoexpediente == (int)Constants.SubtipoDeExpediente.SinPlanos)
            {
                visibleConformacionLocal = "visibleConformacionLocal(true);";
            }
            Functions.EjecutarScript(updRubros, visibleConformacionLocal + scriptName);
        }
        private void CargarDocumentosRequeridos()
        {
            DGHP_Entities db = new DGHP_Entities();
            var query = (from tip in db.Tipo_Documentacion_Req
                         select tip);

            ddlTipoDocReq_runc.DataTextField = "Descripcion";
            ddlTipoDocReq_runc.DataValueField = "Id";

            ddlTipoDocReq_runc.DataSource = query.ToList();
            ddlTipoDocReq_runc.DataBind();
            ddlTipoDocReq_runc.Items.Insert(0, string.Empty);
            db.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        private void CargarTipoActividad()
        {
            DGHP_Entities db = new DGHP_Entities();
            var query = (from tip in db.TipoActividad
                         select tip);

            ddlTipoActividad_runc.DataTextField = "Descripcion";
            ddlTipoActividad_runc.DataValueField = "Id";

            ddlTipoActividad_runc.DataSource = query.ToList();
            ddlTipoActividad_runc.DataBind();
            ddlTipoActividad_runc.Items.Insert(0, string.Empty);//new ListItem("(Seleccionar)", "99"));
            db.Dispose();
        }
        protected void lnkSupHabSave_Click(object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            LinkButton lnkSupHabSave = (LinkButton)sender;

            System.Web.UI.HtmlControls.HtmlInputText input = (System.Web.UI.HtmlControls.HtmlInputText)lnkSupHabSave.NamingContainer.FindControl("txtSupHabNew");

            int edit_id_cpadron = 0;
            int edit_id_cpadronrubro = 0;
            decimal edit_SupHabNew = 0;

            int edit_id_tipodocreq = 0;
            int edit_id_tipoactividad = 0;

            string[] arg = new string[3];
            arg = lnkSupHabSave.Attributes["data-rubro"].ToString().Split(';');
            
            string edit_desc_rubro = arg[0];
            int.TryParse(arg[1], out edit_id_tipodocreq);
            int.TryParse(arg[2], out edit_id_tipoactividad);

            string edit_cod_rubro = lnkSupHabSave.CommandName ?? "";
            
            int.TryParse(lnkSupHabSave.CommandArgument, out edit_id_cpadronrubro);
            decimal.TryParse(input.Value, out edit_SupHabNew);

            int.TryParse(hid_id_cpadron.Value, out edit_id_cpadron);

            try
            {
                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        CPadron_Rubros Rubro_Edit = db.CPadron_Rubros.Where(x => x.id_cpadronrubro == edit_id_cpadronrubro).FirstOrDefault();
                        Rubro_Edit.SuperficieHabilitar = edit_SupHabNew;

                        db.SaveChanges();
                        db.Entry(Rubro_Edit).Reload();

                        //db.CPadron_EliminarRubro(edit_id_cpadronrubro);
                        ////Rubro no contemplado
                        //if (edit_cod_rubro == "888888")
                        //{
                        //    CPadron_Rubros consultaPadronRubrosDTO = new CPadron_Rubros();

                        //    consultaPadronRubrosDTO.id_cpadronrubro = db.CPadron_Rubros.Select(x => x.id_cpadronrubro).Max() + 1;
                        //    consultaPadronRubrosDTO.id_cpadron = this.id_cpadron;
                        //    consultaPadronRubrosDTO.id_tipodocreq = edit_id_tipodocreq;
                        //    consultaPadronRubrosDTO.id_tipoactividad = edit_id_tipoactividad;
                        //    consultaPadronRubrosDTO.SuperficieHabilitar = edit_SupHabNew;
                        //    consultaPadronRubrosDTO.desc_rubro = edit_desc_rubro;
                        //    consultaPadronRubrosDTO.cod_rubro = "888888";
                        //    consultaPadronRubrosDTO.CreateDate = DateTime.Now;
                        //    consultaPadronRubrosDTO.id_ImpactoAmbiental = 3;//buscar enumeracion-- Sujeto a Categorización

                        //    db.CPadron_Rubros.Add(consultaPadronRubrosDTO);
                        //    db.SaveChanges();

                        //}
                        //else
                        //{
                        //    //Rubro contemplado
                        //    db.CPadron_AgregarRubro(edit_id_cpadron, edit_cod_rubro, edit_SupHabNew);
                        //}

                        CargarRubros(this.id_cpadron);

                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, ex.Message);
                        lblError.Text = Functions.GetErrorMessage(ex);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmError_Rubros", "$('#frmError_Rubros').modal('show');", true);
                    }
                }
                //updRubros.Update();
            }
            catch (Exception ex)
            {
                LogError.Write(ex, ex.Message);
                lblError.Text = Functions.GetErrorMessage(ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmError_Rubros", "$('#frmError_Rubros').modal('show');", true);

            }
            finally
            {
                db.Dispose();
            }

        }

        protected void btnAgregarRubroUsoNoContemplado_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            try
            {
                int id_tipoactividad = int.Parse(ddlTipoActividad_runc.SelectedValue);
                int id_tipodocreq = int.Parse(ddlTipoDocReq_runc.SelectedValue);
                decimal superficie = 0;
                decimal.TryParse(txtSuperficieRubro_runc.Text, out superficie);


                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {

                        CPadron_Rubros consultaPadronRubrosDTO = new CPadron_Rubros();

                        consultaPadronRubrosDTO.id_cpadronrubro = db.CPadron_Rubros.Select(x => x.id_cpadronrubro).Max() + 1;
                        consultaPadronRubrosDTO.id_cpadron = this.id_cpadron;
                        consultaPadronRubrosDTO.id_tipodocreq = id_tipodocreq;
                        consultaPadronRubrosDTO.id_tipoactividad = id_tipoactividad;
                        consultaPadronRubrosDTO.SuperficieHabilitar = superficie;
                        consultaPadronRubrosDTO.desc_rubro = txtDesc_runc.Text;
                        consultaPadronRubrosDTO.cod_rubro = "888888";
                        consultaPadronRubrosDTO.CreateDate = DateTime.Now;
                        consultaPadronRubrosDTO.id_ImpactoAmbiental = 3;//buscar enumeracion-- Sujeto a Categorización

                        db.CPadron_Rubros.Add(consultaPadronRubrosDTO);
                        db.SaveChanges();

                        CargarRubros(this.id_cpadron);

                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, ex.Message);
                        lblError.Text = Functions.GetErrorMessage(ex);
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmError_Rubros", "$('#frmError_Rubros').modal('show');", true);
                    }
                }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmAgregarActividades", "$('#frmAgregarActividades').modal('hide');", true);
                //ScriptManager.RegisterClientScriptBlock(updBotonesAgregarRubros, updBotonesAgregarRubros.GetType(), "hidefrmAgregarRubroUsoNoContemplado", "hidefrmAgregarRubroUsoNoContemplado();", true);


            }
            catch (Exception ex)
            {
                LogError.Write(ex, ex.Message);
                lblError.Text = Functions.GetErrorMessage(ex);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "frmError_Rubros", "$('#frmError_Rubros').modal('show');", true);
                //this.EjecutarScript(updBotonesAgregarRubros, "showfrmError();");
            }

            finally
            {
                db.Dispose();
            }
        }
    }
    
}