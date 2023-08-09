using SGI.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace SGI.ABM
{
    public partial class AbmRubrosCurVisualizar : System.Web.UI.Page
    {
        DGHP_Entities db = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            var query = Request.QueryString["Id"];
            if (query != null)
            {
                int IdRubro = Convert.ToInt16(Request.QueryString["Id"].ToString());
                CargarDatosUnicaVez(IdRubro);
                LoadRubroCur(IdRubro);
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
                    txtFechaVigenciaHasta.Text = q.VigenciaHasta_rubro.ToString();
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
                    ChkSoloApra.Checked = q.SoloAPRA;
                    ChkExpress.Checked = q.CondicionExpress;
                    txtZonaMixtura1.Text = q.ZonaMixtura1;
                    txtZonaMixtura2.Text = q.ZonaMixtura2;
                    txtZonaMixtura3.Text = q.ZonaMixtura3;
                    txtZonaMixtura4.Text = q.ZonaMixtura4;
                    txtObservaciones.Text = q.Observaciones;
                    chkAsistentes350.Checked = q.Asistentes350 ?? false;
                    chkSinBanioPCD.Checked = q.SinBanioPCD ?? false;

                    grdImpactoAmbiental.DataSource = db.Rubros_TraerImpactoAmbientalPorId(idRubro);
                    grdImpactoAmbiental.DataBind();

                }
            }
        }

        public void CargarDatosUnicaVez(int IdRubro)
        {
            CargarCircuitos();
            CargarTiposDeActividades();
            CargarTiposDeTramites();
            CargarTiposEstacionamientos();
            CargarTiposRubrosBicicletas();
            CargarTiposRubrosCargayDescarga();
            CargarTiposDeDocumentosRequeridos(IdRubro);
            CargarInformacionRelevantePorIdRubro(IdRubro);
            CargarConfiguracionIncendio(IdRubro);
            CargarCondicionesIncendio();
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
            q.Add(new ItemDropDownList { Id = 0, Descripcion = "SIN CATEGORÍA" });
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
            q.Add(new ItemDropDownList { Id = 0, Descripcion = "SIN CATEGORÍA" });
            return q.OrderBy(x => x.Descripcion).ToList();
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

        #endregion

        private void CargarTiposDeDocumentosRequeridos(int idRubro)
        {
            db = new DGHP_Entities();
            var q = (from rubro_doc in db.RubrosCN_TiposDeDocumentosRequeridos
                     join tipo_doc in db.TiposDeDocumentosRequeridos on rubro_doc.id_tdocreq equals tipo_doc.id_tdocreq
                     where rubro_doc.id_rubro == idRubro && tipo_doc.baja_tdocreq == false
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

            grdDocReq.DataSource = q.ToList();
            grdDocReq.DataBind();
        }

        public void CargarInformacionRelevantePorIdRubro(int idRubro)
        {
            DGHP_Entities db = new DGHP_Entities();

            var q = (from info in db.RubrosCN_InformacionRelevante
                     where info.id_rubro == idRubro
                     select new
                     {
                         id_rubro = info.id_rubro,
                         id_rubInfRel_histcam = info.id_rubinf,
                         descripcion_rubinf = info.descripcion_rubinf,
                         id_rubinf = info.id_rubinf
                     }
                );

            grdInfoRelevante.DataSource = q.ToList();
            grdInfoRelevante.DataBind();
        }

        public void CargarConfiguracionIncendio(int idRubro)
        {
            DGHP_Entities db = new DGHP_Entities();

            var q = (from rubro_doc in db.RubrosCN_Config_Incendio
                     where rubro_doc.id_rubro == idRubro
                     select new
                     {
                         rubro_doc.id_rubro,
                         rubro_doc.id_rubro_incendio,
                         rubro_doc.riesgo,
                         rubro_doc.DesdeM2,
                         rubro_doc.HastaM2
                     }
              );

            grdConfIncendio.DataSource = q.ToList();
            grdConfIncendio.DataBind();
        }

    }
}