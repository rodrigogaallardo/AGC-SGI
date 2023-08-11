using SGI.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.ABM.RubrosCUR
{

    public partial class EditarRubroCur : BasePage
    {
        DGHP_Entities db = null;
        private int IdRubro;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(pnlDatosRubro, pnlDatosRubro.GetType(), "init_updDatos", "init_updDatos();", true);

            var query = Request.QueryString["Id"];
            if (query != null)
            {
                IdRubro = Convert.ToInt16(Request.QueryString["Id"].ToString());
                if (!Page.IsPostBack)
                {
                    hid_DecimalSeparator.Value = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    TraerRubros_InformacionRelevante_porIdRubro(IdRubro);
                    TraerRubros_ConfiguracionIncendio_porIdRubro(IdRubro);
                    CargarDatosUnicaVez(IdRubro);
                    LoadRubroCur(IdRubro);
                }
            }
            else
            {
                Response.Redirect("~/ABM/RubrosCUR/AbmRubrosCUR.aspx");
            }
        }

        private void LoadRubroCur(int idRubro)
        {
            using (var db = new DGHP_Entities())
            {
                var q = (from r in db.RubrosCN
                         where r.IdRubro == idRubro
                         select r).FirstOrDefault();

                if (q != null)
                {
                    txtCodRubro.Text = q.Codigo;
                    txtToolTip.Text = q.Keywords;
                    txtDescRubro.Text = q.Nombre;
                    txtFechaVigenciaHasta.Text = q.VigenciaHasta_rubro == null ? "" : Convert.ToDateTime(q.VigenciaHasta_rubro).Date.ToShortDateString();
                    //combos
                    ddlTipoActividad.SelectedValue = q.IdTipoActividad.ToString();
                    ddlTipoTramite.SelectedValue = q.IdTipoExpediente.ToString();
                    ddlCircuito.SelectedValue = Convert.ToString(q.IdGrupoCircuito ?? 0);
                    ddlEstacionamiento.SelectedValue = Convert.ToString(q.IdEstacionamiento ?? 0);
                    ddlRubrosBicicleta.SelectedValue = Convert.ToString(q.IdBicicleta ?? 0);
                    ddlCyD.SelectedValue = Convert.ToString(q.IdCyD ?? 0);
                    ddlCondicionesIncendio.SelectedValue = Convert.ToString(q.idCondicionIncendio ?? 0);
                    //
                    ChkLibrado.Checked = q.LibrarUso;
                    ChkExpress.Checked = q.CondicionExpress;
                    txtZonaMixtura1.Text = q.ZonaMixtura1;
                    txtZonaMixtura2.Text = q.ZonaMixtura2;
                    txtZonaMixtura3.Text = q.ZonaMixtura3;
                    txtZonaMixtura4.Text = q.ZonaMixtura4;
                    txtObservaciones.Text = q.Observaciones;
                    chkAsistentes350.Checked = q.Asistentes350 ?? false;
                    chkSinBanioPCD.Checked = q.SinBanioPCD ?? false;
                }
            }
        }

        public void CargarDatosUnicaVez(int idRubro)
        {
            CargarCircuitos();
            CargarTiposDeActividades();
            CargarTiposDeTramites();
            CargarTiposEstacionamientos();
            CargarTiposRubrosBicicletas();
            CargarTiposRubrosCargayDescarga();
            CargarTipoDocRequeridos();
            CargarTiposDeDocumentosRequeridos(idRubro);
            CargarInformacionRelevante();
            CargarConfiguracionIncedio();
            CargarCondicionesIncendio();
        }

        protected void btnGuardarRubroCN_Click(object sender, EventArgs e)
        {
            using (var db = new DGHP_Entities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Page.Validate();
                        if (Page.IsValid)
                        {
                            var rubroCur = db.RubrosCN.Where(r => r.IdRubro == IdRubro).First();

                            rubroCur.Codigo = txtCodRubro.Text.Trim();
                            rubroCur.Keywords = txtToolTip.Text.TrimEnd();
                            rubroCur.Nombre = txtDescRubro.Text;
                            rubroCur.VigenciaDesde_rubro = DateTime.Now.Date;
                            rubroCur.VigenciaHasta_rubro = string.IsNullOrWhiteSpace(txtFechaVigenciaHasta.Text) ? new DateTime?() : Convert.ToDateTime(txtFechaVigenciaHasta.Text);
                            rubroCur.IdTipoActividad = Convert.ToInt16(ddlTipoActividad.SelectedItem.Value);
                            rubroCur.IdTipoExpediente = Convert.ToInt16(ddlTipoTramite.SelectedItem.Value);
                            rubroCur.IdGrupoCircuito = Convert.ToInt16(ddlCircuito.SelectedItem.Value);
                            rubroCur.LibrarUso = ChkLibrado.Checked;
                            rubroCur.CondicionExpress = ChkExpress.Checked;
                            rubroCur.ZonaMixtura1 = txtZonaMixtura1.Text;
                            rubroCur.ZonaMixtura2 = txtZonaMixtura2.Text;
                            rubroCur.ZonaMixtura3 = txtZonaMixtura3.Text;
                            rubroCur.ZonaMixtura4 = txtZonaMixtura4.Text;
                            rubroCur.IdEstacionamiento = Convert.ToInt16(ddlEstacionamiento.SelectedItem.Value);
                            rubroCur.IdBicicleta = Convert.ToInt16(ddlRubrosBicicleta.SelectedItem.Value);
                            rubroCur.IdCyD = Convert.ToInt16(ddlCyD.SelectedItem.Value);
                            rubroCur.Observaciones = txtObservaciones.Text.TrimEnd();
                            rubroCur.CreateDate = DateTime.Now.Date;
                            rubroCur.CreateUser = Functions.GetUserId();
                            rubroCur.LastUpdateDate = DateTime.Now.Date;
                            rubroCur.LastUpdateUser = Functions.GetUserId();
                            rubroCur.Asistentes350 = chkAsistentes350.Checked;
                            rubroCur.SinBanioPCD = chkSinBanioPCD.Checked;
                            rubroCur.idCondicionIncendio = Convert.ToInt16(ddlCondicionesIncendio.SelectedItem.Value);

                            SaveDocReq(rubroCur, db);
                            SaveInfoRelevant(rubroCur, db);
                            SaveConfIncendio(rubroCur, db);
                            db.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        EjecutarScript(this, "showModal(" + ex.ToString() + ");");
                        //throw ex;
                    }
                }
                Response.Redirect("~/ABM/RubrosCUR/AbmRubrosCUR.aspx");
            }
        }

        /// <summary>
        /// Saves the document req.
        /// </summary>
        /// <param name="rubroCur">The rubro current.</param>
        private void SaveDocReq(RubrosCN rubroCur, DGHP_Entities db)
        {
            //Salvo los RubrosCN_TiposDeDocumentosRequeridos
            List<RubrosCN_TiposDeDocumentosRequeridosDTO> rubroDdoc = new List<RubrosCN_TiposDeDocumentosRequeridosDTO>();
            rubroDdoc = (List<RubrosCN_TiposDeDocumentosRequeridosDTO>)ViewState["TiposDeDocumentosRequeridos"];
            foreach (RubrosCN_TiposDeDocumentosRequeridosDTO item in rubroDdoc)
            {
                //Inserto los que no existen
                if (item.id_rubtdocreq < 0)
                {
                    RubrosCN_TiposDeDocumentosRequeridos dr = new RubrosCN_TiposDeDocumentosRequeridos();
                    dr.id_rubro = rubroCur.IdRubro;
                    dr.id_tdocreq = item.id_tdocreq;
                    dr.obligatorio_rubtdocreq = item.obligatorio_rubtdocreq;
                    dr.CreateUser = Functions.GetUserId();
                    dr.CreateDate = DateTime.Now.Date;
                    rubroCur.RubrosCN_TiposDeDocumentosRequeridos.Add(dr);
                }
                else
                {
                    //Si existe solo actualizo si es obligatorio
                    var dr = rubroCur.RubrosCN_TiposDeDocumentosRequeridos.Where(x => x.id_rubro == IdRubro && x.id_tdocreq == item.id_tdocreq).First();
                    dr.obligatorio_rubtdocreq = item.obligatorio_rubtdocreq;
                }
            }

            //Elimino los documentos 
            List<string> IdsDeleted = new List<string>();
            if (hidIdDeletedDocReq.Value.ToString().Trim().Length > 0)
            {
                IdsDeleted = hidIdDeletedDocReq.Value.ToString().Split(',').ToList();
            }
            //var delItem = rubroCur.RubrosCN_TiposDeDocumentosRequeridos.Where(x => IdsDeleted.Contains(x.id_rubtdocreq.ToString()));
            foreach (string id_rubtdocreq in IdsDeleted)
            {
                var iid_rubtdocreq = Convert.ToInt32(id_rubtdocreq);
                var iDel = db.RubrosCN_TiposDeDocumentosRequeridos.Where(x => x.id_rubro == rubroCur.IdRubro &&  x.id_rubtdocreq == iid_rubtdocreq).First();
                db.RubrosCN_TiposDeDocumentosRequeridos.Remove(iDel);
            }
        }

        /// <summary>
        /// Saves the information relevant.
        /// </summary>
        /// <param name="rubroCur">The rubro current.</param>
        private void SaveInfoRelevant(RubrosCN rubroCur, DGHP_Entities db)
        {
            //Salvo los RubrosCN_InformacionRelevanteDTO
            List<RubrosCN_InformacionRelevanteDTO> RubrosInf = new List<RubrosCN_InformacionRelevanteDTO>();
            List<RubrosCN_InformacionRelevanteDTO> RubrosInfDel = new List<RubrosCN_InformacionRelevanteDTO>();

            if (ViewState["InfoRelevante"] != null)
            {
                RubrosInf = (List<RubrosCN_InformacionRelevanteDTO>)ViewState["InfoRelevante"];

                foreach (RubrosCN_InformacionRelevanteDTO item in RubrosInf)
                {
                    //Inserto los que no existen
                    if (item.id_rubinf < 0)
                    {
                        RubrosCN_InformacionRelevante dr = new RubrosCN_InformacionRelevante();
                        dr.id_rubro = rubroCur.IdRubro;
                        dr.descripcion_rubinf = item.descripcion_rubinf;
                        dr.CreateUser = Functions.GetUserId();
                        dr.CreateDate = DateTime.Now.Date;
                        rubroCur.RubrosCN_InformacionRelevante.Add(dr);
                    }
                    else
                    {
                        //Si existe solo actualizo si es obligatorio
                        var dr = rubroCur.RubrosCN_InformacionRelevante.Where(x => x.id_rubro == IdRubro && x.id_rubinf == item.id_rubinf).First();
                        dr.descripcion_rubinf = item.descripcion_rubinf;
                    }
                }
            }
            //Elimino 
            if (ViewState["InfoRelevanteDel"] != null)
            {
                RubrosInfDel = (List<RubrosCN_InformacionRelevanteDTO>)ViewState["InfoRelevanteDel"];
                foreach (RubrosCN_InformacionRelevanteDTO item in RubrosInfDel)
                {
                    var iDel = db.RubrosCN_InformacionRelevante.Where(x => x.id_rubinf == item.id_rubinf).First();
                    db.RubrosCN_InformacionRelevante.Remove(iDel);
                }
            }

        }

        private void SaveConfIncendio(RubrosCN rubroCur, DGHP_Entities db)
        {
            //Salvo los RubrosCN_Config_IncendioDTO
            List<RubrosCN_Config_IncendioDTO> RubrosConfInc = new List<RubrosCN_Config_IncendioDTO>();
            List<RubrosCN_Config_IncendioDTO> RubrosConfIncDel = new List<RubrosCN_Config_IncendioDTO>();

            if (ViewState["ConfIncendio"] != null)
            {
                RubrosConfInc = (List<RubrosCN_Config_IncendioDTO>)ViewState["ConfIncendio"];

                foreach (RubrosCN_Config_IncendioDTO item in RubrosConfInc)
                {
                    //Inserto los que no existen
                    if (item.id_rubro_incendio < 0)
                    {
                        RubrosCN_Config_Incendio dr = new RubrosCN_Config_Incendio();
                        dr.id_rubro = rubroCur.IdRubro;
                        dr.DesdeM2 = item.DesdeM2;
                        dr.HastaM2 = item.HastaM2;
                        dr.riesgo = item.riesgo;
                        dr.CreateUser = Functions.GetUserId();
                        dr.CreateDate = DateTime.Now.Date;
                        rubroCur.RubrosCN_Config_Incendio.Add(dr);
                    }
                    else
                    {
                        //Si existe solo actualizo si es obligatorio
                        var dr = rubroCur.RubrosCN_Config_Incendio.Where(x => x.id_rubro == IdRubro && x.id_rubro_incendio == item.id_rubro_incendio).First();
                        dr.DesdeM2 = item.DesdeM2;
                        dr.HastaM2 = item.HastaM2;
                        dr.riesgo = item.riesgo;
                    }
                }
            }
            //Elimino 
            if (ViewState["ConfIncendioDel"] != null)
            {
                RubrosConfIncDel = (List<RubrosCN_Config_IncendioDTO>)ViewState["ConfIncendioDel"];
                foreach (RubrosCN_Config_IncendioDTO item in RubrosConfIncDel)
                {
                    var iDel = db.RubrosCN_Config_Incendio.Where(x => x.id_rubro_incendio == item.id_rubro_incendio).First();
                    db.RubrosCN_Config_Incendio.Remove(iDel);
                }
            }

        }

        private void LimpiarDatos()
        {
            txtCodRubro.Text = string.Empty;
            txtToolTip.Text = string.Empty;
            txtDescRubro.Text = string.Empty;
            txtFechaVigenciaHasta.Text = string.Empty;
            ddlTipoActividad.ClearSelection();
            ddlTipoActividad.Items.FindByText("[POR FAVOR SELECCIONE...]").Selected = true;
            ddlTipoTramite.ClearSelection();
            ddlTipoTramite.Items.FindByText("[POR FAVOR SELECCIONE...]").Selected = true;
            ddlCircuito.ClearSelection();
            ddlCircuito.Items.FindByText("[POR FAVOR SELECCIONE...]").Selected = true;
            ChkLibrado.Checked = false;
            txtZonaMixtura1.Text = string.Empty;
            txtZonaMixtura2.Text = string.Empty;
            txtZonaMixtura3.Text = string.Empty;
            txtZonaMixtura4.Text = string.Empty;
            ddlEstacionamiento.ClearSelection();
            ddlEstacionamiento.Items.FindByText("_").Selected = true;
            ddlRubrosBicicleta.ClearSelection();
            ddlRubrosBicicleta.Items.FindByText("_").Selected = true;
            ddlCyD.ClearSelection();
            ddlCyD.Items.FindByText("_").Selected = true;
            txtObservaciones.Text = string.Empty;
            chkAsistentes350.Checked = false;
            chkSinBanioPCD.Checked = false;

        }

        #region Combos
        private void CargarTiposEstacionamientos()
        {
            ddlEstacionamiento.DataSource = GetTiposDeEstacionamientos();
            ddlEstacionamiento.DataTextField = "Descripcion";
            ddlEstacionamiento.DataValueField = "Id";
            ddlEstacionamiento.DataBind();
        }

        private List<ItemDropDownList> GetTiposDeEstacionamientos()
        {
            var q = (from re in db.RubrosEstacionamientos
                     select new ItemDropDownList
                     {
                         Id = re.IdEstacionamiento,
                         Descripcion = re.Codigo.ToUpper(),
                     }).ToList();
            return q.OrderBy(x => x.Descripcion).ToList();
        }

        private void CargarTiposDeTramites()
        {
            ddlTipoTramite.DataSource = GetTiposDeTramites();
            ddlTipoTramite.DataTextField = "Descripcion";
            ddlTipoTramite.DataValueField = "Id";
            ddlTipoTramite.DataBind();
        }

        private List<ItemDropDownList> GetTiposDeTramites()
        {
            var q = (from tdr in db.TipoExpediente
                     select new ItemDropDownList
                     {
                         Id = tdr.id_tipoexpediente,
                         Descripcion = tdr.descripcion_tipoexpediente.ToUpper(),
                     }).ToList();
            return q.OrderBy(x => x.Id).ToList();
        }

        private void CargarTiposDeActividades()
        {
            ddlTipoActividad.DataSource = GetTiposDeActividades();
            ddlTipoActividad.DataTextField = "Descripcion";
            ddlTipoActividad.DataValueField = "Id";
            ddlTipoActividad.DataBind();
        }


        private List<ItemDropDownList> GetTiposDeActividades()
        {
            db = new DGHP_Entities();

            var q = (from ta in db.TipoActividad
                     select new ItemDropDownList
                     {
                         Id = ta.Id,
                         Descripcion = ta.Nombre.ToUpper(),
                     }).ToList();
            q.Add(new ItemDropDownList { Id = 0, Descripcion = "[POR FAVOR SELECCIONE...]" });
            return q.OrderBy(x => x.Descripcion).ToList();
        }

        private void CargarCircuitos()
        {
            ddlCircuito.DataSource = TraerCircuitos();
            ddlCircuito.DataTextField = "Descripcion";
            ddlCircuito.DataValueField = "Id";
            ddlCircuito.DataBind();
        }

        private List<ItemDropDownList> TraerCircuitos()
        {
            db = new DGHP_Entities();

            var q = (from cir in db.ENG_Grupos_Circuitos
                     select new ItemDropDownList
                     {
                         Id = cir.id_grupo_circuito,
                         Descripcion = cir.cod_grupo_circuito + " - " + cir.nom_grupo_circuito.ToUpper(),
                     }).ToList();
            q.Add(new ItemDropDownList { Id = 0, Descripcion = "[POR FAVOR SELECCIONE...]" });
            return q.OrderBy(x => x.Descripcion).ToList();
        }


        protected void ddlEstacionamiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblEstacionamientoDesc.Text = GetDescripcionRubrosEstacionamientos(ddlEstacionamiento.SelectedItem.Text);
            lblEstacionamientoInfo.Visible = true;
        }

        private string GetDescripcionRubrosEstacionamientos(string value)
        {
            db = new DGHP_Entities();

            var q = (from re in db.RubrosEstacionamientos
                     select new
                     {
                         re.Codigo,
                         re.Descripcion
                     }).Where(x => x.Codigo == value).FirstOrDefault();
            return q.Descripcion;
        }

        protected void ddlBicicleta_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblBicicletaDesc.Text = GetDescripcionRubrosBicicletas(ddlRubrosBicicleta.SelectedItem.Text);
            lblBicicletaInfo.Visible = true;
        }

        private string GetDescripcionRubrosBicicletas(string text)
        {
            db = new DGHP_Entities();

            var q = (from rb in db.RubrosBicicletas
                     select new
                     {
                         rb.Codigo,
                         rb.Descripcion
                     }).Where(x => x.Codigo == text).FirstOrDefault();
            return q.Descripcion;
        }

        private void CargarTiposRubrosBicicletas()
        {
            ddlRubrosBicicleta.DataSource = GetTipoRubrosBicicleta();
            ddlRubrosBicicleta.DataTextField = "Descripcion";
            ddlRubrosBicicleta.DataValueField = "Id";
            ddlRubrosBicicleta.DataBind();
        }

        private List<ItemDropDownList> GetTipoRubrosBicicleta()
        {
            db = new DGHP_Entities();

            var q = (from rb in db.RubrosBicicletas
                     select new ItemDropDownList
                     {
                         Id = rb.IdBicicleta,
                         Descripcion = rb.Codigo.ToUpper(),
                     }).ToList();
            return q.OrderBy(x => x.Descripcion).ToList();
        }

        protected void ddlCyD_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCyD.Text = GetDescripcionRubrosCargayDescarga(ddlCyD.SelectedItem.Text);
            lblCyDInfo.Visible = true;
        }

        private string GetDescripcionRubrosCargayDescarga(string text)
        {
            db = new DGHP_Entities();

            var q = (from rb in db.RubrosCargasyDescargas
                     select new
                     {
                         rb.Codigo,
                         rb.Descripcion
                     }).Where(x => x.Codigo == text).FirstOrDefault();
            return q.Descripcion;
        }

        private List<ItemDropDownList> GetTipoRubrosCargayDescarga()
        {
            db = new DGHP_Entities();

            var q = (from rb in db.RubrosCargasyDescargas
                     select new ItemDropDownList
                     {
                         Id = rb.IdCyD,
                         Descripcion = rb.Codigo.ToUpper(),
                     }).ToList();
            return q.OrderBy(x => x.Descripcion).ToList();
        }

        private void CargarTiposRubrosCargayDescarga()
        {
            ddlCyD.DataSource = GetTipoRubrosCargayDescarga();
            ddlCyD.DataTextField = "Descripcion";
            ddlCyD.DataValueField = "Id";
            ddlCyD.DataBind();
        }
        #endregion


        #region  	0138775: JADHE 53248 - SGI - REQ ABM Rubros CUR - Documentacion obligatoria

        private List<RubrosCN_TiposDeDocumentosRequeridosDTO> CargarTiposDeDocumentosRequeridos(int idRubro, int IdsInserted = 0, bool obligatorio_rubtdocreq = false)
        {
            List<RubrosCN_TiposDeDocumentosRequeridosDTO> rubroDdoc = new List<RubrosCN_TiposDeDocumentosRequeridosDTO>();
            List<string> IdsDeleted = new List<string>();

            if (hidIdDeletedDocReq.Value.ToString().Trim().Length > 0)
            {
                IdsDeleted = hidIdDeletedDocReq.Value.ToString().Split(',').ToList();
            }


            db = new DGHP_Entities();

            if (ViewState["TiposDeDocumentosRequeridos"] == null)
            {
                var q = (from rubro_doc in db.RubrosCN_TiposDeDocumentosRequeridos
                         join tipo_doc in db.TiposDeDocumentosRequeridos on rubro_doc.id_tdocreq equals tipo_doc.id_tdocreq
                         where
                            rubro_doc.id_rubro == idRubro
                            && tipo_doc.baja_tdocreq == false
                         //&& !IdsDeleted.Contains(rubro_doc.id_rubtdocreq.ToString())
                         select new RubrosCN_TiposDeDocumentosRequeridosDTO()
                         {
                             id_rubro = rubro_doc.id_rubro,
                             id_rubtdocreq = rubro_doc.id_rubtdocreq,
                             id_tdocreq = rubro_doc.id_tdocreq,
                             nombre_tdocreq = tipo_doc.nombre_tdocreq,
                             observaciones_tdocreq = tipo_doc.observaciones_tdocreq,
                             baja_tdocreq = tipo_doc.baja_tdocreq,
                             obligatorio_rubtdocreq = rubro_doc.obligatorio_rubtdocreq,
                             es_obligatorio = (rubro_doc.obligatorio_rubtdocreq == true) ? "Si" : "No",
                             Accion = "",
                             Color = ""
                         }
                  );

                rubroDdoc = q.ToList();

            }
            else
            {
                rubroDdoc = (List<RubrosCN_TiposDeDocumentosRequeridosDTO>)ViewState["TiposDeDocumentosRequeridos"];
            }

            //Busco los  Ids nuevos y los inserto
            if (IdsInserted > 0)
            {
                int id_rubTreq = (-1) * (rubroDdoc.Count + 1);
                var q = (from tipo_doc in db.TiposDeDocumentosRequeridos
                         where
                            tipo_doc.id_tdocreq == IdsInserted
                         select new RubrosCN_TiposDeDocumentosRequeridosDTO()
                         {
                             id_rubro = idRubro,
                             id_rubtdocreq = id_rubTreq,
                             id_tdocreq = IdsInserted,
                             nombre_tdocreq = tipo_doc.nombre_tdocreq,
                             observaciones_tdocreq = tipo_doc.observaciones_tdocreq,
                             baja_tdocreq = tipo_doc.baja_tdocreq,
                             obligatorio_rubtdocreq = obligatorio_rubtdocreq,
                             es_obligatorio = (obligatorio_rubtdocreq ? "Si" : "No"),
                             Accion = "I",
                             Color = ""
                         }
                         );
                rubroDdoc.AddRange(q.ToList());
            }

            //Elimino los Ids eliminados
            if (IdsDeleted.Count > 0)
            {
                rubroDdoc.RemoveAll(x => IdsDeleted.Contains(x.id_rubtdocreq.ToString()));
            }

            //los ordeno por Id
            rubroDdoc = rubroDdoc.OrderBy(x => x.id_rubtdocreq).ToList();

            ViewState["TiposDeDocumentosRequeridos"] = rubroDdoc;
            grdDocReq.DataSource = rubroDdoc;
            grdDocReq.DataBind();
            return rubroDdoc;
        }

        protected void btnEliminarDocReq_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();
            LinkButton btnEliminar = (LinkButton)sender;
            GridViewRow gv_row = (GridViewRow)btnEliminar.Parent.Parent;
            int idRubtDocReq = Convert.ToInt32(grdDocReq.DataKeys[gv_row.RowIndex].Values[0]);
            hidIdDeletedDocReq.Value = idRubtDocReq.ToString() + (hidIdDeletedDocReq.Value.Length > 0 ? "," : "") + hidIdDeletedDocReq.Value;

            CargarTiposDeDocumentosRequeridos(IdRubro);
            CargarTiposDeDocumentosRequeridosElim(IdRubro);
        }

        protected void btnEliminarDocReqElim_Click(object sender, EventArgs e)
        {
            List<string> Ids = hidIdDeletedDocReq.Value.ToString().Split(',').ToList();
            DGHP_Entities db = new DGHP_Entities();
            LinkButton btnEliminar = (LinkButton)sender;
            GridViewRow gv_row = (GridViewRow)btnEliminar.Parent.Parent;

            int idRubtDocReq = Convert.ToInt32(grdDocReqEliminada.DataKeys[gv_row.RowIndex].Values[0]);
            Ids.RemoveAt(Ids.IndexOf(idRubtDocReq.ToString()));
            hidIdDeletedDocReq.Value = string.Join(",", new List<string>(Ids).ToArray());

            CargarTiposDeDocumentosRequeridos(IdRubro);
            CargarTiposDeDocumentosRequeridosElim(IdRubro);
        }

        /// <summary>
        /// Adds the delete documentos requeridos.
        /// </summary>
        /// <param name="idRubtDocReq">The identifier rubt document req.</param>
        private void CargarTiposDeDocumentosRequeridosElim(int idRubtDocReq)
        {
            List<string> Ids = hidIdDeletedDocReq.Value.ToString().Split(',').ToList();

            db = new DGHP_Entities();
            var q = (from rubro_doc in db.RubrosCN_TiposDeDocumentosRequeridos
                     join tipo_doc in db.TiposDeDocumentosRequeridos on rubro_doc.id_tdocreq equals tipo_doc.id_tdocreq
                     where
                        rubro_doc.id_rubro == idRubtDocReq
                        && tipo_doc.baja_tdocreq == false
                        && Ids.Contains(rubro_doc.id_rubtdocreq.ToString())
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

            grdDocReqEliminada.DataSource = q.ToList();
            grdDocReqEliminada.DataBind();
            if (q.Count() > 0)
                pnlDocReqEliminada.Visible = true;
            else
                pnlDocReqEliminada.Visible = false;
        }


        protected void lnkBtnAccionesAgregarDocReq_Click(object sender, EventArgs e)
        {
            pnlAgregarDocReqEdit.Visible = true;
        }

        protected void lnkBtnAccionesAgregarInfoRelevante_Click(object sender, EventArgs e)
        {
            pnlAgregarInfoRelevanteEdit.Visible = true;
        }


        protected void btnCancelarInfoRelevante_Click(object sender, EventArgs e)
        {
            pnlAgregarInfoRelevanteEdit.Visible = false;
        }


        protected void btnGuardarDocReq_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            try
            {
                ValidarGuardarDocReq();
                CargarTiposDeDocumentosRequeridos(IdRubro, Convert.ToInt32(ddlEditTipoDocReq.SelectedValue), Convert.ToBoolean(ddlEditObligatorio.SelectedValue));
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                this.EjecutarScript(updPnlAgregarDocReq, "showfrmErrorVisual();");
            }
        }

        protected void btnCancelarDocReq_Click(object sender, EventArgs e)
        {
            pnlAgregarDocReqEdit.Visible = false;
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
        }

        public void CargarConfiguracionIncedio()
        {
            List<RubrosCN_Config_IncendioDTO> RubrosConfInc = (List<RubrosCN_Config_IncendioDTO>)ViewState["ConfIncendio"];

            grdConfIncendio.DataSource = RubrosConfInc;
            grdConfIncendio.DataBind();
        }

        private void CargarCondicionesIncendio()
        {
            ddlCondicionesIncendio.DataSource = GetCondicionesIncendio();
            ddlCondicionesIncendio.DataTextField = "Descripcion";
            ddlCondicionesIncendio.DataValueField = "Id";
            ddlCondicionesIncendio.DataBind();
        }

        private List<ItemDropDownList> GetCondicionesIncendio()
        {
            var items = (from re in db.CondicionesIncendio
                         select new ItemDropDownList
                         {
                             Id = re.idCondicionIncendio,
                             Descripcion = re.codigo.ToUpper(),
                         }).ToList();

            return items.OrderBy(x => x.Id).ToList();
        }

        protected void ddlCondicionesIncendio_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCondicionesIncendio.Text = ddlCondicionesIncendio.SelectedItem.Text;
            lblCondicionesIncendio.Visible = true;
        }

        public void CargarInformacionRelevante()
        {
            List<RubrosCN_InformacionRelevanteDTO> RubrosInf = (List<RubrosCN_InformacionRelevanteDTO>)ViewState["InfoRelevante"];

            grdInfoRelevante.DataSource = RubrosInf;
            grdInfoRelevante.DataBind();
        }

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



        private bool ValidarGuardarDocReq()
        {
            if (string.IsNullOrEmpty(ddlEditTipoDocReq.Text) || ddlEditTipoDocReq.SelectedValue == "0")
                throw new Exception("Debe seleccionar el tipo de documento.");


            if (string.IsNullOrEmpty(ddlEditObligatorio.Text) || ddlEditObligatorio.SelectedValue == "0")
                throw new Exception("Debe seleccionar indicar si la documentación es obligatoria o no.");

            int id_tdocreq = Convert.ToInt32(ddlEditTipoDocReq.SelectedValue);

            //Valido que no exista en el grid de documentos requerido
            if (CargarTiposDeDocumentosRequeridos(IdRubro).Exists(x => x.id_tdocreq == id_tdocreq))
                throw new Exception("El tipo de documento ya fue ingresado.");

            return false;
        }

        private bool ValidarGuardarInf()
        {
            if (string.IsNullOrEmpty(txtEditDescripInfoRelevante.Text))
                throw new Exception("Debe indicar la informacion reelevante del rubro.");

            return false;
        }

        private bool ValidarGuardarConfInc()
        {


            if (string.IsNullOrEmpty(txtDesde.Text))
                throw new Exception("Debe ingresar la sup. desde.");
            if (string.IsNullOrEmpty(txtHasta.Text))
                throw new Exception("Debe ingresar la sup. hasta.");
            return false;
        }

        protected void btnEliminarInfoRelevante_Click(object sender, EventArgs e)
        {
            //Elimino del dto
            List<RubrosCN_InformacionRelevanteDTO> RubrosInf = new List<RubrosCN_InformacionRelevanteDTO>();
            List<RubrosCN_InformacionRelevanteDTO> RubrosInfDel;

            RubrosInf = (List<RubrosCN_InformacionRelevanteDTO>)ViewState["InfoRelevante"];

            if (ViewState["InfoRelevanteDel"] == null)
            {
                RubrosInfDel = new List<RubrosCN_InformacionRelevanteDTO>();
            }
            else
            {
                RubrosInfDel = (List<RubrosCN_InformacionRelevanteDTO>)ViewState["InfoRelevanteDel"];
            }

            if (RubrosInf.Count > 0)
            {
                LinkButton btnEliminar = (LinkButton)sender;
                GridViewRow gv_row = (GridViewRow)btnEliminar.Parent.Parent;
                int id_rubinf = Convert.ToInt32(grdInfoRelevante.DataKeys[gv_row.RowIndex].Values[0]);

                RubrosInfDel.AddRange(RubrosInf.Where(x => x.id_rubinf == id_rubinf));
                RubrosInf = RubrosInf.Where(x => x.id_rubinf != id_rubinf).ToList();

                grdInfoRelevanteEliminada.DataSource = RubrosInfDel;

                ViewState["InfoRelevanteDel"] = RubrosInfDel;
                ViewState["InfoRelevante"] = RubrosInf;

                grdInfoRelevanteEliminada.DataSource = RubrosInfDel;
                grdInfoRelevanteEliminada.DataBind();

                CargarInformacionRelevante();
                pnlInfoRelevanteEliminada.Visible = true;
            }
        }

        protected void btnGuardarInfoRelevante_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarGuardarInf();
                List<RubrosCN_InformacionRelevanteDTO> RubrosInf;
                if (ViewState["InfoRelevante"] == null)
                {
                    //Creo el view state
                    RubrosInf = new List<RubrosCN_InformacionRelevanteDTO>();
                }
                else
                {
                    RubrosInf = (List<RubrosCN_InformacionRelevanteDTO>)ViewState["InfoRelevante"];
                }

                //Agrego el nuevo registro
                RubrosCN_InformacionRelevanteDTO RubroInf = new RubrosCN_InformacionRelevanteDTO();

                int id_rubinf = (-1) * (RubrosInf.Count + 1);
                RubroInf.id_rubro = this.IdRubro;
                RubroInf.id_rubinf = id_rubinf;
                RubroInf.descripcion_rubinf = txtEditDescripInfoRelevante.Text.Trim();
                RubrosInf.Add(RubroInf);
                ViewState["InfoRelevante"] = RubrosInf;
                txtEditDescripInfoRelevante.Text = "";
                CargarInformacionRelevante();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                this.EjecutarScript(updPnlAgregarDocReq, "showfrmErrorVisual();");
            }
        }


        public void TraerRubros_InformacionRelevante_porIdRubro(int id_rubro)
        {
            List<RubrosCN_InformacionRelevanteDTO> RubrosInf = new List<RubrosCN_InformacionRelevanteDTO>();
            DGHP_Entities db = new DGHP_Entities();
            var q = (from info in db.RubrosCN_InformacionRelevante
                     where info.id_rubro == id_rubro
                     select info
                );

            var rubroInfList = q.ToList();

            foreach (var item in rubroInfList)
            {
                RubrosCN_InformacionRelevanteDTO RubroInf = new RubrosCN_InformacionRelevanteDTO();
                RubroInf.id_rubinf = item.id_rubinf;
                RubroInf.id_rubro = item.id_rubro;
                RubroInf.descripcion_rubinf = item.descripcion_rubinf;
                RubroInf.CreateDate = item.CreateDate;
                RubroInf.CreateUser = item.CreateUser;
                RubrosInf.Add(RubroInf);
            }
            ViewState["InfoRelevante"] = RubrosInf;
            db.Dispose();
        }

        public void TraerRubros_ConfiguracionIncendio_porIdRubro(int id_rubro)
        {
            List<RubrosCN_Config_IncendioDTO> RubrosConfInc = new List<RubrosCN_Config_IncendioDTO>();
            DGHP_Entities db = new DGHP_Entities();
            var q = (from info in db.RubrosCN_Config_Incendio
                     where info.id_rubro == id_rubro
                     select info
                );

            var rubroInfList = q.ToList();

            foreach (var item in rubroInfList)
            {
                RubrosCN_Config_IncendioDTO RubroInf = new RubrosCN_Config_IncendioDTO();
                RubroInf.id_rubro_incendio = item.id_rubro_incendio;
                RubroInf.id_rubro = item.id_rubro;
                RubroInf.DesdeM2 = item.DesdeM2;
                RubroInf.HastaM2 = item.HastaM2;
                RubroInf.riesgo = item.riesgo;
                RubroInf.CreateDate = item.CreateDate;
                RubroInf.CreateUser = item.CreateUser;
                RubrosConfInc.Add(RubroInf);
            }
            ViewState["ConfIncendio"] = RubrosConfInc;
            db.Dispose();
        }


        protected void btnEliminarConfIncendio_Click(object sender, EventArgs e)
        {
            //Elimino del dto
            List<RubrosCN_Config_IncendioDTO> RubrosConfInc = new List<RubrosCN_Config_IncendioDTO>();
            List<RubrosCN_Config_IncendioDTO> RubrosConfIncDel = new List<RubrosCN_Config_IncendioDTO>();

            RubrosConfInc = (List<RubrosCN_Config_IncendioDTO>)ViewState["ConfIncendio"];

            if (ViewState["ConfIncendioDel"] == null)
            {
                RubrosConfIncDel = new List<RubrosCN_Config_IncendioDTO>();
            }
            else
            {
                RubrosConfIncDel = (List<RubrosCN_Config_IncendioDTO>)ViewState["ConfIncendioDel"];
            }

            if (RubrosConfInc.Count > 0)
            {
                LinkButton btnEliminar = (LinkButton)sender;
                GridViewRow gv_row = (GridViewRow)btnEliminar.Parent.Parent;
                int id_rubro_incendio = Convert.ToInt32(grdConfIncendio.DataKeys[gv_row.RowIndex].Values[0]);

                RubrosConfIncDel.AddRange(RubrosConfInc.Where(x => x.id_rubro_incendio == id_rubro_incendio));
                RubrosConfInc = RubrosConfInc.Where(x => x.id_rubro_incendio != id_rubro_incendio).ToList();

                grdInfoRelevanteEliminada.DataSource = RubrosConfIncDel;

                ViewState["ConfIncendioDel"] = RubrosConfIncDel;
                ViewState["ConfIncendio"] = RubrosConfInc;

                grdConfIncendioEliminada.DataSource = RubrosConfIncDel;
                grdConfIncendioEliminada.DataBind();

                CargarConfiguracionIncedio();
                pnlConfIncendioEliminada.Visible = true;
            }

        }


        protected void lnkBtnAccionesAgregarConfIncendio_Click(object sender, EventArgs e)
        {
            pnlAgregarConfIncendioEdit.Visible = true;
            ScriptManager.RegisterStartupScript(pnlDatosRubro, pnlDatosRubro.GetType(), "init_updDatos", "init_updDatos();", true);
        }

        protected void btnCancelarConfIncendio_Click(object sender, EventArgs e)
        {
            pnlAgregarConfIncendioEdit.Visible = false;
        }


        protected void btnGuardarConfIncendio_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarGuardarConfInc();
                List<RubrosCN_Config_IncendioDTO> RubrosInf;
                if (ViewState["ConfIncendio"] == null)
                {
                    //Creo el view state
                    RubrosInf = new List<RubrosCN_Config_IncendioDTO>();
                }
                else
                {
                    RubrosInf = (List<RubrosCN_Config_IncendioDTO>)ViewState["ConfIncendio"];
                }

                //Agrego el nuevo registro
                RubrosCN_Config_IncendioDTO RubroInf = new RubrosCN_Config_IncendioDTO();
                int id_rubro_incendio = (-1)*(RubrosInf.Count + 1);
                RubroInf.id_rubro = this.IdRubro;
                RubroInf.id_rubro_incendio = id_rubro_incendio;
                RubroInf.HastaM2 = Convert.ToDecimal(txtHasta.Text.Trim());
                RubroInf.DesdeM2 = Convert.ToDecimal(txtDesde.Text.Trim());
                RubroInf.riesgo = Convert.ToInt32(ddlRiesgo.SelectedValue);
                RubrosInf.Add(RubroInf);
                ViewState["ConfIncendio"] = RubrosInf;

                txtHasta.Text = "";
                txtDesde.Text = "";
                ddlRiesgo.SelectedIndex = 0;
                CargarConfiguracionIncedio();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                this.EjecutarScript(updPnlAgregarDocReq, "showfrmErrorVisual();");
            }
        }
        #endregion
    }
}