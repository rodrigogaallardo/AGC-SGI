using ExcelLibrary.BinaryFileFormat;
using Newtonsoft.Json;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.ABM.Partidas
{
    public partial class NuevaUbicacion : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "init_Js_updDatos", "init_Js_updDatos();", true);
                ScriptManager.RegisterStartupScript(updBodyAgregarUbicacion, updBodyAgregarUbicacion.GetType(), "init_Js_updBodyAgregarUbicacion", "init_Js_updBodyAgregarUbicacion();", true);
            }
            //cargarPermisos();

            if (!Page.IsPostBack)
            {
                Session["dtDistritos"] = null;
            }
        }

        #region permisos
        //private bool editar;
        //private bool visualizar;
        //private bool aprobar;
        //private bool bajar;

        //private void cargarPermisos()
        //{
        //    Guid userid = Functions.GetUserId();
        //    using (var db = new DGHP_Entities())
        //    {
        //        var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

        //        foreach (var perfil in perfiles_usuario)
        //        {
        //            var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

        //            if (menu_usuario.Contains("Visualizar Partidas"))
        //                visualizar = true;

        //            if (menu_usuario.Contains("Editar Partidas"))
        //            {
        //                editar = true;
        //                bajar = true;
        //            }

        //            if (menu_usuario.Contains("Aprobar edición de partidas"))
        //                aprobar = true;
        //        }
        //    }
        //}

        #endregion

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                CargarCombo_tipoUbicacion(false);
                CargarCombo_subTipoUbicacion(-1, false);
                CargarComboCalles();
                CargarComboMixtura();
                CargarCombosDistritos();
                CargarCombos_Barrios();
                CargarCombos_Comisaria();
                CargarCombos_Comunas();
                this.EjecutarScript(updDatos, "finalizarCarga();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        protected void btnEliminarUbicacion_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEliminarUbicacion = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnEliminarUbicacion.Parent.Parent;
                DataTable dt = dtUbicacionesCargadas();
                dt.Rows.RemoveAt(row.RowIndex);
                grdUbicaciones.DataSource = dt;
                grdUbicaciones.DataBind();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        private bool editable = true;
        protected void grdUbicaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //permisos
                LinkButton btnEliminarUbicacion = (LinkButton)e.Row.FindControl("btnEliminarUbicacion");
                btnEliminarUbicacion.Visible = this.editable;
            }
        }

        private DataTable dtUbicacionesCargadas()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("calles", typeof(string));
            dt.Columns.Add("nroPuerta", typeof(int));
            dt.Columns.Add("codigo_calle", typeof(int));

            foreach (GridViewRow row in grdUbicaciones.Rows)
            {
                DataRow datarw;
                datarw = dt.NewRow();
                datarw[0] = HttpUtility.HtmlDecode(row.Cells[0].Text);
                datarw[1] = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[1].Text));
                datarw[2] = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[2].Text));
                dt.Rows.Add(datarw);
            }
            return dt;
        }
        #region Modales
        protected void btnAgregarUbicacion_Click(object sender, EventArgs e)
        {
            try
            {

                txtNroPuerta.Text = string.Empty;

                this.EjecutarScript(updDatos, "showfrmAgregarUbicacion();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                this.EjecutarScript(updDatos, "showfrmError();");
            }
        }
        #endregion


        #region CargarCombos
        private void CargarCombosDistritos()
        {
            using (var db = new DGHP_Entities())
            {
                List<Ubicaciones_GruposDistritos> lista = db.Ubicaciones_GruposDistritos.ToList();

                lista.Insert(0, new Ubicaciones_GruposDistritos { IdGrupoDistrito = 0, Nombre = "[Seleccione...]" });

                ddlGrupoDistritos.DataTextField = "Nombre";
                ddlGrupoDistritos.DataValueField = "IdGrupoDistrito";
                ddlGrupoDistritos.DataSource = lista.OrderBy(x => x.Nombre);
                ddlGrupoDistritos.DataBind();
            }
        }

        private void CargarComboMixtura()
        {
            using (var db = new DGHP_Entities())
            {
                List<Ubicaciones_ZonasMixtura> lista = db.Ubicaciones_ZonasMixtura.OrderByDescending(x => x.IdZonaMixtura).ToList();

                Ubicaciones_ZonasMixtura item = new Ubicaciones_ZonasMixtura();
                item.IdZonaMixtura = 0;
                item.Descripcion = "[Seleccione...]";
                lista.Insert(0, item);
                ddlMixtura.DataTextField = "Descripcion";
                ddlMixtura.DataValueField = "IdZonaMixtura";
                ddlMixtura.DataSource = lista;
                ddlMixtura.DataBind();
            }
        }

        private void CargarCombo_tipoUbicacion(Boolean busqueda)
        {
            using (var db = new DGHP_Entities())
            {
                List<TiposDeUbicacion> lista = db.TiposDeUbicacion.ToList();

                ddlbiTipoUbicacionABM.DataTextField = "descripcion_tipoubicacion";
                ddlbiTipoUbicacionABM.DataValueField = "id_tipoubicacion";
                ddlbiTipoUbicacionABM.DataSource = lista.OrderBy(x => x.descripcion_tipoubicacion);
                ddlbiTipoUbicacionABM.DataBind();

                //Por defecto esta seleccionado "Parcela Común"
                ddlbiTipoUbicacionABM.SelectedValue = 0.ToString();
            }
        }

        private void CargarCombo_subTipoUbicacion(int id_tipoubicacion, Boolean busqueda)
        {
            using (var db = new DGHP_Entities())
            {
                List<SubTiposDeUbicacion> lista = db.SubTiposDeUbicacion
                .Where(x => x.habilitado == true)
                .Where(x => x.id_tipoubicacion == id_tipoubicacion).ToList();

                SubTiposDeUbicacion sub_tipo_ubi = new SubTiposDeUbicacion();
                sub_tipo_ubi.id_subtipoubicacion = 0;
                sub_tipo_ubi.descripcion_subtipoubicacion = (busqueda) ? "Seleccione" : "Ninguno";
                lista.Insert(0, sub_tipo_ubi);

                ddlUbiSubTipoUbicacionABM.DataTextField = "descripcion_subtipoubicacion";
                ddlUbiSubTipoUbicacionABM.DataValueField = "id_subtipoubicacion";
                ddlUbiSubTipoUbicacionABM.DataSource = lista.OrderBy(x => x.descripcion_subtipoubicacion);
                ddlUbiSubTipoUbicacionABM.DataBind();
            }
        }

        private string GetNombreCalle(int codCalle, int NroPuerta)
        {
            using (var db = new DGHP_Entities())
            {
                var c = (from cal in db.Calles
                         where cal.Codigo_calle == codCalle
                         && (cal.AlturaIzquierdaInicio_calle <= cal.AlturaDerechaInicio_calle ? cal.AlturaIzquierdaInicio_calle : cal.AlturaDerechaInicio_calle) <= NroPuerta
                         && (cal.AlturaDerechaFin_calle >= cal.AlturaIzquierdaFin_calle ? cal.AlturaDerechaFin_calle : cal.AlturaIzquierdaFin_calle) >= NroPuerta
                         select cal).SingleOrDefault();

                if (c != null)
                {
                    return c.NombreOficial_calle;
                }

                return "";
            }
        }

        protected void btnGuardarUbicacion_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dtUbicacionesCargadas();
                int nroPuertaUbi = 0;
                int codCalle = 0;
                int.TryParse(txtNroPuerta.Text.Trim(), out nroPuertaUbi);
                int.TryParse(ddlCalle.SelectedValue, out codCalle);
                string txtCalle = GetNombreCalle(codCalle, nroPuertaUbi);

                if (txtCalle != "" && !ubicacionRepetida(txtCalle, nroPuertaUbi))
                {

                    DataRow datarw;
                    datarw = dt.NewRow();
                    datarw[0] = txtCalle;
                    datarw[1] = nroPuertaUbi;
                    datarw[2] = codCalle;
                    dt.Rows.Add(datarw);
                    grdUbicaciones.DataSource = dt;
                    grdUbicaciones.DataBind();
                    this.EjecutarScript(updUbicaciones, "hidefrmAgregarUbicacion();");
                }
                else
                {
                    txtNroPuerta.Text = string.Empty;
                    txtNroPuerta.Focus();
                    this.EjecutarScript(updUbicaciones, "showErrorAgregarUbicacion();");
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                this.EjecutarScript(updUbicaciones, "hidefrmAgregarUbicacion();");
            }
        }

        private bool ubicacionRepetida(string Calle, int nroPuertaUbi)
        {
            DataTable dt = dtUbicacionesCargadas();

            var existe = (from r in dt.AsEnumerable()
                          where r.Field<String>("calles").Contains(Calle) &&
                                r.Field<int>("nroPuerta") == nroPuertaUbi
                          select r).ToList().Count();

            return existe != 0;
        }



        private void CargarComboCalles()
        {
            using (DGHP_Entities db = new DGHP_Entities())
            {
                var lstCalles = (from cal in db.Calles
                                 select new ItemCalle
                                 {
                                     NombreOficial_calle = cal.NombreOficial_calle,
                                     Codigo_calle = cal.Codigo_calle
                                 }).Distinct().OrderBy(x => x.NombreOficial_calle);

                ddlCalle.DataTextField = "NombreOficial_calle";
                ddlCalle.DataValueField = "Codigo_calle";
                ddlCalle.DataSource = lstCalles.ToList();
                ddlCalle.DataBind();
                ddlCalle.Items.Insert(0, "");
            }
        }

        internal class ItemCalle
        {
            public string NombreOficial_calle { get; set; }
            public int Codigo_calle { get; set; }
        }

        protected void ddlbiTipoUbicacionABM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id_tipoubicacion = int.Parse(ddlbiTipoUbicacionABM.SelectedValue);
                hid_id_tipo_ubicacion.Value = id_tipoubicacion.ToString();
                CargarCombo_subTipoUbicacion(id_tipoubicacion, false);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }
        #endregion

        protected void ddlGrupoDistritos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdGrupoDistrito = int.Parse(ddlGrupoDistritos.SelectedValue);
                if (IdGrupoDistrito != 0)
                {
                    CargarCombo_Distritos(IdGrupoDistrito, true);
                }
                else
                {
                    ddlDistritos.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }

            updDatos.Update();
        }

        private void CargarCombosDistritosZonas()
        {
            using (var db = new DGHP_Entities())
            {
                List<Ubicaciones_CatalogoDistritos_Zonas> lista = db.Ubicaciones_CatalogoDistritos_Zonas.ToList();

                lista.Insert(0, new Ubicaciones_CatalogoDistritos_Zonas { IdZona = 0, CodigoZona = "[Seleccione...]" });

                ddlDistritosZonas.DataTextField = "CodigoZona";
                ddlDistritosZonas.DataValueField = "IdZona";
                ddlDistritosZonas.DataSource = lista.OrderBy(x => x.CodigoZona);
                ddlDistritosZonas.DataBind();
            }
        }

        private void CargarCombo_Distritos(int IdGrupoDistrito, bool value)
        {
            using (var db = new DGHP_Entities())
            {
                var lista = (from catalogoDistritos in db.Ubicaciones_CatalogoDistritos
                             where catalogoDistritos.IdGrupoDistrito == IdGrupoDistrito
                             select new ItemUbicacionesCatalogoDistritos
                             {
                                 IdDistrito = catalogoDistritos.IdDistrito,
                                 Codigo = catalogoDistritos.Codigo,
                                 Descripcion = catalogoDistritos.Codigo + " - " + catalogoDistritos.Descripcion,
                                 Orden = catalogoDistritos.orden.Value
                             }).OrderBy(x => x.Orden).ToList();

                lista.Insert(0, new ItemUbicacionesCatalogoDistritos { IdDistrito = 0, Descripcion = "[Seleccione...]" });

                ddlDistritos.DataTextField = "Descripcion";
                ddlDistritos.DataValueField = "IdDistrito";
                ddlDistritos.DataSource = lista.OrderBy(x => x.Orden);
                ddlDistritos.DataBind();

                ddlDistritos.Enabled = value;

                ddlDistritosZonas.SelectedIndex = 0;
                ddlDistritosSubZonas.SelectedIndex = 0;
            }
        }

        protected void ddlDistritosZonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdZona = int.Parse(ddlDistritosZonas.SelectedValue);
                if (IdZona != 0)
                {
                    CargarCombo_DistritosSubZonas(IdZona, true);
                }
                else
                {
                    ddlDistritosSubZonas.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }

            updDatos.Update();
        }

        private void CargarCombo_DistritosSubZonas(int idZona, bool value)
        {
            using (var db = new DGHP_Entities())
            {
                List<Ubicaciones_CatalogoDistritos_Subzonas> lista = db.Ubicaciones_CatalogoDistritos_Subzonas.Where(x => x.IdZona == idZona).ToList();

                lista.Insert(0, new Ubicaciones_CatalogoDistritos_Subzonas { IdSubZona = 0, CodigoSubZona = "[Seleccione...]" });

                ddlDistritosSubZonas.DataTextField = "CodigoSubZona";
                ddlDistritosSubZonas.DataValueField = "IdSubZona";
                ddlDistritosSubZonas.DataSource = lista.OrderBy(x => x.CodigoSubZona);
                ddlDistritosSubZonas.DataBind();
                ddlDistritosSubZonas.Enabled = value;
            }
        }

        protected void ddlDistritos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdDistrito = int.Parse(ddlDistritos.SelectedValue);
                if (IdDistrito != 0)
                {
                    CargarCombo_DistritosZonas(IdDistrito, true);
                }
                else
                {
                    ddlDistritosZonas.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }

            updDatos.Update();
        }

        private void CargarCombo_DistritosZonas(int IdDistrito, bool value)
        {
            using (var db = new DGHP_Entities())
            {
                List<Ubicaciones_CatalogoDistritos_Zonas> lista = db.Ubicaciones_CatalogoDistritos_Zonas.Where(x => x.IdDistrito == IdDistrito).ToList();

                lista.Insert(0, new Ubicaciones_CatalogoDistritos_Zonas { IdZona = 0, CodigoZona = "[Seleccione...]" });

                ddlDistritosZonas.DataTextField = "CodigoZona";
                ddlDistritosZonas.DataValueField = "IdZona";
                ddlDistritosZonas.DataSource = lista.OrderBy(x => x.CodigoZona);
                ddlDistritosZonas.DataBind();
                ddlDistritosZonas.Enabled = value;
            }
        }

        private void CargarCombos_Comisaria()
        {
            using (var db = new DGHP_Entities())
            {
                List<Comisarias> comisarias = db.Comisarias.Where(a => a.id_comisaria != 54 && a.id_comisaria != 56).ToList();
                ddlComisaria.DataTextField = "nom_comisaria";
                ddlComisaria.DataValueField = "id_comisaria";
                ddlComisaria.DataSource = comisarias.OrderBy(b => b.nom_comisaria);
                ddlComisaria.DataBind();
            }
        }

        private void CargarCombos_Barrios()
        {
            using (var db = new DGHP_Entities())
            {
                List<Barrios> barrios = db.Barrios.Where(a => a.id_barrio != 50).ToList();
                ddlBarrio.DataTextField = "nom_barrio";
                ddlBarrio.DataValueField = "id_barrio";
                ddlBarrio.DataSource = barrios.OrderBy(p => p.nom_barrio);
                ddlBarrio.DataBind();
            }
        }

        private void CargarCombos_Comunas()
        {
            using (var db = new DGHP_Entities())
            {
                List<Comunas> comunas = db.Comunas.Where(a => a.id_comuna != 17).ToList();
                ddlComuna.DataTextField = "nom_comuna";
                ddlComuna.DataValueField = "id_comuna";
                ddlComuna.DataSource = comunas.OrderBy(p => p.nom_comuna);
                ddlComuna.DataBind();
            }
        }

        protected void btnAgregarMixtura_Click(object sender, EventArgs e)
        {
            if (ddlMixtura.SelectedIndex != 0)
            {
                try
                {
                    DataTable dt = dtMixturasCargadas();

                    string mixDescripcion = (ddlMixtura.SelectedItem.Text).Trim();

                    string mix = (ddlMixtura.SelectedValue).Trim();

                    if (!MixRepetida(mix, mixDescripcion))
                    {
                        DataRow datarw;
                        datarw = dt.NewRow();

                        datarw[0] = mix;
                        datarw[1] = mixDescripcion;


                        dt.Rows.Add(datarw);

                        dt.DefaultView.Sort = "mix DESC";
                        dt = dt.DefaultView.ToTable();

                        grdMixturas.DataSource = dt;
                        grdMixturas.DataBind();
                    }
                    else
                    {
                        ddlMixtura.Focus();
                    }

                }
                catch (Exception ex)
                {
                    LogError.Write(ex);
                    this.EjecutarScript(updUbicaciones, "hidefrmAgregarUbicacion();");
                }
            }
        }

        private bool MixRepetida(string mix, string mixDescripcion)
        {
            DataTable dt = dtMixturasCargadas();

            var existe = (from r in dt.AsEnumerable()
                          where r.Field<String>("mix").Trim() == mix.Trim() &&
                                r.Field<String>("mixDescripcion").Trim() == mixDescripcion.Trim()
                          select r).ToList().Count();

            return existe != 0;
        }

        private DataTable dtMixturasCargadas()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("mix", typeof(string));
            dt.Columns.Add("mixDescripcion", typeof(string));

            foreach (GridViewRow row in grdMixturas.Rows)
            {
                DataRow datarw;
                datarw = dt.NewRow();
                datarw[0] = HttpUtility.HtmlDecode(row.Cells[0].Text);
                datarw[1] = HttpUtility.HtmlDecode(row.Cells[1].Text);
                dt.Rows.Add(datarw);
            }
            return dt;
        }

        protected void grdMixturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnEliminarMix = (LinkButton)e.Row.FindControl("btnEliminarMixtura");
                btnEliminarMix.Visible = this.editable;

            }
        }

        protected void grdDistritos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnEliminarDis = (LinkButton)e.Row.FindControl("btnEliminarDistrito");
                btnEliminarDis.Visible = this.editable;

            }
        }

        protected void btnAgregarDistrito_Click(object sender, EventArgs e)
        {
            if (ddlGrupoDistritos.SelectedIndex != 0)
            {
                try
                {
                    DataTable dt = dtDistritosCargados();

                    string grupoDistrito = ddlGrupoDistritos.SelectedItem.Text.Trim();
                    string distrito = ddlDistritos.SelectedIndex != -1 ? (ddlDistritos.SelectedIndex != 0 ? ddlDistritos.SelectedItem.Text.Trim() : string.Empty) : string.Empty;
                    string zonas = ddlDistritosZonas.SelectedIndex != -1 ? (ddlDistritosZonas.SelectedIndex != 0 ? ddlDistritosZonas.SelectedItem.Text.Trim() : string.Empty) : string.Empty;
                    string subZonas = ddlDistritosSubZonas.SelectedIndex != -1 ? (ddlDistritosSubZonas.SelectedIndex != 0 ? ddlDistritosSubZonas.SelectedItem.Text.Trim() : string.Empty) : string.Empty;

                    //int IdDistrito = ddlDistritos.SelectedIndex;
                    int IdDistrito = Convert.ToInt32(ddlDistritos.SelectedValue);

                    var IdZonas = 0;
                    var IdSubZonas = 0;

                    if (!String.IsNullOrEmpty(zonas))
                    {
                        IdZonas = Convert.ToInt32(ddlDistritosZonas.SelectedValue);
                    }
                    if (!String.IsNullOrEmpty(subZonas))
                    {
                        IdSubZonas = Convert.ToInt32(ddlDistritosSubZonas.SelectedValue);
                    }

                    if (!DistritoRepetido(grupoDistrito, distrito, zonas, subZonas))
                    {
                        DataRow datarw;
                        datarw = dt.NewRow();

                        datarw[0] = grupoDistrito;
                        datarw[1] = distrito;
                        datarw[2] = zonas;
                        datarw[3] = subZonas;
                        datarw[4] = IdDistrito;
                        datarw[5] = IdZonas;
                        datarw[6] = IdSubZonas;

                        dt.Rows.Add(datarw);

                        grdDistritos.DataSource = dt;
                        grdDistritos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    LogError.Write(ex);
                    this.EjecutarScript(updUbicaciones, "hidefrmAgregarUbicacion();");
                }
            }
        }

        private bool DistritoRepetido(string grupoDistrito, string distrito, string zonas, string subzonas)
        {
            DataTable dt = dtDistritosCargados();
            var existe = (from r in dt.AsEnumerable()
                          where r.Field<String>("grupoDistrito").Trim() == grupoDistrito.Trim() &&
                                r.Field<String>("distrito").Trim() == distrito.Trim() &&
                                r.Field<String>("zonas").Trim() == zonas.Trim() &&
                                r.Field<String>("subzonas").Trim() == subzonas.Trim()
                          select r).ToList().Count();
            return existe != 0;
        }


        private DataTable dtDistritosCargados()
        {
            DataTable dtDistritos = new DataTable();

            dtDistritos.Columns.Add("grupoDistrito", typeof(string));
            dtDistritos.Columns.Add("distrito", typeof(string));
            dtDistritos.Columns.Add("zonas", typeof(string));
            dtDistritos.Columns.Add("subzonas", typeof(string));
            dtDistritos.Columns.Add("IdDistrito", typeof(int));
            dtDistritos.Columns.Add("IdZona", typeof(int));
            dtDistritos.Columns.Add("IdSubZona", typeof(int));
            //}

            foreach (GridViewRow row in grdDistritos.Rows)
            {
                DataRow datarw;
                datarw = dtDistritos.NewRow();
                datarw[0] = HttpUtility.HtmlDecode(row.Cells[0].Text);
                datarw[1] = HttpUtility.HtmlDecode(row.Cells[1].Text);
                datarw[2] = HttpUtility.HtmlDecode(row.Cells[2].Text);
                datarw[3] = HttpUtility.HtmlDecode(row.Cells[3].Text);
                datarw[4] = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[4].Text));
                datarw[5] = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[5].Text));
                datarw[6] = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[6].Text));
                dtDistritos.Rows.Add(datarw);
            }
            return dtDistritos;
        }

        protected void btnEliminarMixtura_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEliminarMixtura = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnEliminarMixtura.Parent.Parent;
                DataTable dt = dtMixturasCargadas();
                dt.Rows.RemoveAt(row.RowIndex);
                grdMixturas.DataSource = dt;
                grdMixturas.DataBind();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        protected void btnEliminarDistrito_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEliminarDistrito = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnEliminarDistrito.Parent.Parent;
                DataTable dt = dtDistritosCargados();
                dt.Rows.RemoveAt(row.RowIndex);
                grdDistritos.DataSource = dt;
                grdDistritos.DataBind();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ABM/Ubicaciones/AbmUbicaciones.aspx");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ValidarPuertas();
        }

        protected void btnContinuar(object sender, EventArgs e)
        {
            Continuar_Guardar();
        }

        private void ValidarPuertas()
        {
            DGHP_Entities context = new DGHP_Entities();

            var id_ubicacion = NuevoId_ubicacion();

            //GuardarPuerta
            List<Ubicaciones_Puertas> listaPuertas = CrearListaPuertas(id_ubicacion, grdUbicaciones);

            //existe direccion y puerta en otra ubicacion
            StringBuilder puertas = new StringBuilder();
            foreach (var item in listaPuertas)
            {
                var existePuerta = (from ubic in context.Ubicaciones
                                    join ubiPuerta in context.Ubicaciones_Puertas on ubic.id_ubicacion equals ubiPuerta.id_ubicacion
                                    where ubiPuerta.NroPuerta_ubic == item.NroPuerta_ubic && ubiPuerta.codigo_calle == item.codigo_calle
                                    && ubic.baja_logica == false
                                    select ubic).FirstOrDefault();
                if (existePuerta != null)
                {
                    puertas.AppendLine($"Puerta {item.NroPuerta_ubic} . Ya existe la calle y puerta asociada a otra Ubicación (Sección: {existePuerta.Seccion} - Manzana {existePuerta.Manzana} - Parcela: {existePuerta.Parcela}");
                }
            }

            if (puertas.ToString() != "")
            {
                lblAviso.Text = puertas.ToString();
                this.EjecutarScript(updBotonesGuardar, "showfrmAviso();");
            }
            else
            {
                Continuar_Guardar();
            }
        }

        private void Continuar_Guardar()
        {
            bool result = true;
            using (var context = new DGHP_Entities())
            {
                bool Baja_Logica = false;

                //Subtipo Ubi
                int UbiSubTipo = 0;
                int.TryParse(ddlUbiSubTipoUbicacionABM.SelectedValue, out UbiSubTipo);

                //Nro Partida Matriz
                int? UbiNroPartidaValue = null;
                int UbiNroPartida = 0;
                int.TryParse(txtNroPartida.Text.Trim(), out UbiNroPartida);

                if (UbiNroPartida > 0)
                {
                    UbiNroPartidaValue = UbiNroPartida;

                    try
                    {
                        var existe = (from ubic in context.Ubicaciones
                                      where ubic.NroPartidaMatriz == UbiNroPartidaValue.Value
                                      && ubic.baja_logica == false
                                      select ubic.id_ubicacion);

                        if (existe != null && existe.Count() > 0)
                        {
                            txtNroPartida.Text = "";
                            throw new Exception("Ya existe una partida con el mismo Nro de PartidaMatriz");
                        }
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = Functions.GetErrorMessage(ex);
                        this.EjecutarScript(updBotonesGuardarUbicacion, "showfrmError();");
                        return;
                    }
                }

                //Seccion
                int? UbiSeccionValue = null;
                int UbiSeccion = 0;
                int.TryParse(txtSeccion.Text.Trim(), out UbiSeccion);
                if (UbiSeccion > 0)
                {
                    UbiSeccionValue = UbiSeccion;
                }

                // Barrio
                int barrio = 0;
                int.TryParse(ddlBarrio.SelectedValue, out barrio);

                // Comisaria
                int comisaria = 0;
                int.TryParse(ddlComisaria.SelectedValue, out comisaria);

                // Comuna
                int comuna = 0;
                int.TryParse(ddlComuna.SelectedValue, out comuna);

                var idNuevo = NuevoId_ubicacion();
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region Ubicacion

                        var entity = new SGI.Model.Ubicaciones()
                        {
                            id_ubicacion = idNuevo,
                            id_subtipoubicacion = UbiSubTipo,
                            baja_logica = Baja_Logica,
                            Barrios = null,
                            CantiActualizacionesUSIG = 0,
                            cant_ph = null,
                            Circunscripcion = null,
                            id_comisaria = comisaria,
                            id_areahospitalaria = 0,
                            id_barrio = barrio,
                            id_comuna = comuna,
                            id_distritoescolar = 0,
                            id_regionsanitaria = 0,
                            id_zonaplaneamiento = 0,
                            CreateDate = DateTime.Now,
                            VigenciaDesde = DateTime.Now,
                            CreateUser = Functions.GetUserId(),
                            UpdateDate = DateTime.Now,
                            UpdateUser = Functions.GetUserId(),
                            EsEntidadGubernamental = this.chbEntidadGubernamental.Checked,
                            EsUbicacionProtegida = this.chbEdificioProtegido.Checked,
                            NroPartidaMatriz = UbiNroPartida,
                            Seccion = UbiSeccionValue,
                            Manzana = this.txtManzana.Text.Trim(),
                            Parcela = this.txtParcela.Text.Trim(),
                            Observaciones = this.txtObservaciones.Text.Trim(),
                        };
                        #endregion

                        //GuardarPuerta
                        List<Ubicaciones_Puertas> listaPuertas = CrearListaPuertas(entity.id_ubicacion, grdUbicaciones);

                        //existe direccion y puerta en otra ubicacion
                        //StringBuilder puertas = new StringBuilder();
                        //foreach (var item in listaPuertas)
                        //{
                        //    var existePuerta = (from ubic in context.Ubicaciones
                        //                        join ubiPuerta in context.Ubicaciones_Puertas on ubic.id_ubicacion equals ubiPuerta.id_ubicacion
                        //                        where ubiPuerta.NroPuerta_ubic == item.NroPuerta_ubic && ubiPuerta.codigo_calle == item.codigo_calle
                        //                        && ubic.baja_logica == false
                        //                        select ubic).FirstOrDefault();
                        //    if (existePuerta != null)
                        //        puertas.AppendLine($"Puerta {item.NroPuerta_ubic} . Ya existe la calle y puerta asociada a otra Ubicación (Sección: {existePuerta.Seccion} - Manzana {existePuerta.Manzana} - Parcela: {existePuerta.Parcela}");
                        //}

                        //if (puertas.ToString() != "")
                        //    {
                        //        lblAviso.Text = puertas.ToString();
                        //        this.EjecutarScript(updBotonesGuardar, "showfrmAviso();");
                        //    }


                        entity.Ubicaciones_Puertas = listaPuertas;

                        //GuardarMixtura
                        List<Ubicaciones_ZonasMixtura> listaMix = CrearListaMixturas(entity.id_ubicacion, grdMixturas);
                        foreach (var item in listaMix)
                        {
                            Ubicaciones_ZonasMixtura a = context.Ubicaciones_ZonasMixtura.Find(item.IdZonaMixtura);
                            entity.Ubicaciones_ZonasMixtura.Add(a);
                        }

                        context.Ubicaciones.Add(entity);
                        context.SaveChanges();
                        dbContextTransaction.Commit();

                        //GuardarDistrito
                        List<Ubicaciones_Distritos> listaDistritos = CrearListaDistritos(entity.id_ubicacion, grdDistritos);
                        foreach (var item in listaDistritos)
                        {
                            context.Ubicaciones_Distritos_Agregar(item.id_ubicacion, item.IdDistrito, item.IdZona, item.IdSubZona);
                        }
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        dbContextTransaction.Rollback();
                        lblError.Text = Functions.GetErrorMessage(ex);
                        this.EjecutarScript(updBotonesGuardarUbicacion, "showfrmError();");
                        return;
                    }
                }
                Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
                string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
                Model.Ubicaciones obj = context.Ubicaciones.FirstOrDefault(u => u.id_ubicacion == int.Parse(hid_id_ubicacion.Value));
                Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitantes.Text, "I", 1026);
                if (result)
                {
                    Response.Redirect("~/ABM/Ubicaciones/AbmUbicaciones.aspx");
                }
            }
        }

        private List<Ubicaciones_Puertas> CrearListaPuertas(int id_ubicacion, GridView grdUbicaciones)
        {
            var list = new List<Ubicaciones_Puertas>();

            var id = NuevoIdUbicacionPuerta();
            foreach (GridViewRow item in grdUbicaciones.Rows)
            {
                Ubicaciones_Puertas pu = new Ubicaciones_Puertas();
                pu.id_ubic_puerta = id;
                pu.id_ubicacion = id_ubicacion;
                pu.NroPuerta_ubic = Convert.ToInt32(item.Cells[1].Text);
                pu.codigo_calle = Convert.ToInt32(item.Cells[2].Text);
                pu.tipo_puerta = "";
                id++;
                list.Add(pu);
            }
            return list;
        }

        private int NuevoIdUbicacionPuerta()
        {
            using (var db = new DGHP_Entities())
            {
                return db.Ubicaciones_Puertas.Max(p => p.id_ubic_puerta) + 1;
            }
        }

        private List<Ubicaciones_Distritos> CrearListaDistritos(int id_ubicacion, GridView grdDistritos)
        {
            var list = new List<Ubicaciones_Distritos>();

            foreach (GridViewRow item in grdDistritos.Rows)
            {
                Ubicaciones_Distritos dis = new Ubicaciones_Distritos();

                dis.id_ubicacion = id_ubicacion;
                dis.IdDistrito = Convert.ToInt16(item.Cells[4].Text);

                int? id_Zona = null;
                int? id_SubZona = null;

                if (item.Cells[5].Text != "0")
                {
                    id_Zona = Convert.ToInt16(item.Cells[5].Text);
                }
                if (item.Cells[6].Text != "0")
                {
                    id_SubZona = Convert.ToInt16(item.Cells[6].Text);
                }
                dis.IdZona = id_Zona;
                dis.IdSubZona = id_SubZona;
                list.Add(dis);
            }
            return list;
        }

        private int NuevoId_ubicacion()
        {
            using (var db = new DGHP_Entities())
            {
                return db.Ubicaciones.Max(p => p.id_ubicacion) + 1;
            }
        }

        private List<Ubicaciones_ZonasMixtura> CrearListaMixturas(int id_ubicacion, GridView grdMixturas)
        {
            var list = new List<Ubicaciones_ZonasMixtura>();

            foreach (GridViewRow item in grdMixturas.Rows)
            {
                list.Add(new Ubicaciones_ZonasMixtura { IdZonaMixtura = Convert.ToInt16(item.Cells[0].Text) });
            }
            return list;
        }

    }

    public class ItemUbicacionesCatalogoDistritos
    {
        public int IdDistrito { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
    }
}