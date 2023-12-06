using SGI.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Controls
{
    public partial class VisualizarRubroCUR : System.Web.UI.UserControl
    {
        DGHP_Entities db = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(pnlDatosRubro, pnlDatosRubro.GetType(), "init_updDatos", "init_updDatos();", true);
            ScriptManager.RegisterStartupScript(pnlDatosRubro, pnlDatosRubro.GetType(), "inicializar_dropdownlists", "inicializar_dropdownlists();", true);
            if (!IsPostBack)
            {
                hid_DecimalSeparator.Value = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            }
        }

        public void EjecutarScript(UpdatePanel upd, string scriptName)
        {
            ScriptManager.RegisterStartupScript(upd, upd.GetType(),
                "script", scriptName, true);
        }

        public void CargarDatosUnicaVez()
        {
            CargarCircuitos();
            CargarTiposDeActividades();
            CargarTiposDeTramites();
            CargarTiposEstacionamientos();
            CargarTiposRubrosBicicletas();
            CargarTiposRubrosCargayDescarga();
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

        protected void ddlCondicionesIncendio_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCondicionesIncendio.Text = ddlCondicionesIncendio.SelectedItem.Text;
            lblCondicionesIncendio.Visible = true;
        }

        protected void btnCrearRubroCN_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate();
                if (Page.IsValid)
                {
                    var nuevoRubroCur = new RubrosCN
                    {
                        Codigo = txtCodRubro.Text.Trim(),
                        Keywords = txtToolTip.Text.TrimEnd(),
                        Nombre = txtDescRubro.Text,
                        VigenciaDesde_rubro = DateTime.Now.Date,
                        VigenciaHasta_rubro = string.IsNullOrWhiteSpace(txtFechaVigenciaHasta.Text) ? DateTime.Now.Date : Convert.ToDateTime(txtFechaVigenciaHasta.Text),
                        IdTipoActividad = Convert.ToInt16(ddlTipoActividad.SelectedItem.Value),
                        IdTipoExpediente = Convert.ToInt16(ddlTipoTramite.SelectedItem.Value),
                        IdGrupoCircuito = Convert.ToInt16(ddlCircuito.SelectedItem.Value),
                        LibrarUso = ChkLibrado.Checked,
                        SoloAPRA = ChkSoloApra.Checked,
                        ZonaMixtura1 = txtZonaMixtura1.Text,
                        ZonaMixtura2 = txtZonaMixtura2.Text,
                        ZonaMixtura3 = txtZonaMixtura3.Text,
                        ZonaMixtura4 = txtZonaMixtura4.Text,
                        IdEstacionamiento = Convert.ToInt16(ddlEstacionamiento.SelectedItem.Value),
                        IdBicicleta = Convert.ToInt16(ddlRubrosBicicleta.SelectedItem.Value),
                        IdCyD = Convert.ToInt16(ddlCyD.SelectedItem.Value),
                        Observaciones = txtObservaciones.Text.TrimEnd(),
                        CreateDate = DateTime.Now.Date,
                        CreateUser = Functions.GetUserId(),
                        LastUpdateDate = DateTime.Now.Date,
                        LastUpdateUser = Functions.GetUserId(),
                        Asistentes350 = chkAsistentes350.Checked,
                        idCondicionIncendio = Convert.ToInt16(ddlCondicionesIncendio.SelectedItem.Value),
                        SinBanioPCD = chkSinBanioPCD.Checked,
                    };
                    using (db = new DGHP_Entities())
                    {
                        db.RubrosCN.Add(nuevoRubroCur);
                        db.SaveChanges();
                        Response.Redirect("~/ABM/RubrosCUR/AbmRubrosCUR.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        }

        protected void CusValCodRubroUnique_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;

            using (var db = new DGHP_Entities())
            {
                var q = (from rb in db.RubrosCN
                         select new
                         {
                             rb.Codigo
                         }).Where(x => x.Codigo == txtCodRubro.Text.Trim()).FirstOrDefault();

                if (q != null)
                {
                    CusValCodRubroUnique.ErrorMessage = "Ya existe un rubro con este  mismo codigo, por favor verifique.";
                    args.IsValid = false;
                }
            }            
        }
    }
}