using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.ABM.Ubicaciones
{
    public partial class EditarUbicacionTemp : BasePage
    {
        //#region entity
        //private DGHP_Entities db = null;

        //private void IniciarEntity()
        //{
        //    if (this.db == null)
        //        this.db = new DGHP_Entities();
        //}

        //private void FinalizarEntity()
        //{
        //    if (this.db != null)
        //    {
        //        this.db.Dispose();
        //        this.db = null;
        //    }
        //}
        //#endregion

        #region permisos

        //private bool editar;
        //private bool visualizar;

        //private void cargarPermisos()
        //{            
        //    Guid userid = Functions.GetUserId();
        //    using (var db = new DGHP_Entities())
        //    {
        //        var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

        //        foreach (var perfil in perfiles_usuario)
        //        {
        //            var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

        //            if (menu_usuario.Contains("Visualizar Ubicaciones"))
        //                visualizar = true;
        //            if (menu_usuario.Contains("Editar Ubicaciones"))
        //            {
        //                editar = true;
        //            }
        //        }
        //    }
        //}

        #endregion

        #region Var
        int idUbicacion_temp = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "init_Js_updDatos", "init_Js_updDatos();", true);
                ScriptManager.RegisterStartupScript(updBodyAgregarUbicacion, updBodyAgregarUbicacion.GetType(), "init_Js_updBodyAgregarUbicacion", "init_Js_updBodyAgregarUbicacion();", true);
            }
            //cargarPermisos();
            this.idUbicacion_temp = Convert.ToInt32(Request.QueryString["Id"].ToString());
        }
        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                CargarCombo_tipoUbicacion();
                CargarCombo_subTipoUbicacion(0);
                CargarComboCalles();
                CargarComboMixtura();
                CargarCombosDistritos();
                CargarCombos_Barrios();
                CargarCombos_Comisaria();
                CargarCombos_Comunas();

                LoadUbicacionTemp(this.idUbicacion_temp);

                this.EjecutarScript(updDatos, "finalizarCarga();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updDatos, "finalizarCarga();showfrmError();");
            }
        }

        private void LoadUbicacionTemp(int idUbicacion_temp)
        {
            using (var db = new DGHP_Entities())
            {
                var u = (from ubi in db.Ubicaciones_temp
                         where ubi.id_ubicacion_temp == idUbicacion_temp
                         select ubi).FirstOrDefault();

                var up = (from ubi in db.Ubicaciones_Puertas_temp
                          where ubi.id_ubicacion_temp == idUbicacion_temp
                          select ubi).ToList();

                var mix = (from ubi in db.Ubicaciones_ZonasMixtura
                           from ubiZon in ubi.Ubicaciones_temp.Where(x => x.id_ubicacion_temp == idUbicacion_temp)
                           select ubi).Distinct().ToList();

                var dis = (from d in db.Ubicaciones_Distritos_temp
                           where d.id_ubicacion_temp == idUbicacion_temp
                           select d).ToList();

                int? s = u.Seccion;
                if (s != null)
                    txtSeccion.Text = s.ToString();

                txtManzana.Text = u.Manzana.ToString();

                txtParcela.Text = u.Parcela.ToString();

                txtObservaciones.Text = u.Observaciones == null ? string.Empty : u.Observaciones.ToString();

                int? pm = u.NroPartidaMatriz;
                if (pm != null)
                {
                    txtNroPartida.Text = pm.ToString();
                }

                if (u.id_subtipoubicacion > 0)
                {
                    int idSub = (int)u.id_subtipoubicacion;

                    if (Shared.esUbicacionEspecialConObjetoTerritorial(u.id_subtipoubicacion))
                    {
                        txtManzana.Text = 'T' + txtManzana.Text;

                        txtParcela.Text = txtParcela.Text + 't';
                    }

                    var idTipo = db.TiposDeUbicacion.Where(x => x.id_tipoubicacion == idSub).SingleOrDefault();
                    if (idTipo != null)
                    {
                        ddlUbiTipoUbicacion.SelectedValue = idTipo.id_tipoubicacion.ToString();
                        CargarCombo_subTipoUbicacion(idTipo.id_tipoubicacion);
                        ddlUbiSubTipoUbicacion.SelectedValue = u.id_subtipoubicacion.ToString();
                    }
                }

                if (u.id_barrio != null)
                    ddlBarrio.SelectedValue = u.id_barrio.ToString();

                if (u.id_comisaria != null)
                    ddlComisaria.SelectedValue = u.id_comisaria.ToString();

                if (u.id_comuna != null)
                    ddlComuna.SelectedValue = u.id_comuna.ToString();

                chbEdificioProtegido.Checked = u.EsUbicacionProtegida;
                chbEntidadGubernamental.Checked = u.EsEntidadGubernamental;

                //CargarCallesyPuertas
                LlenarGridUbicaciones(idUbicacion_temp, up);
                //CargarMixturas
                LlenarGridMixturas(idUbicacion_temp, mix);
                //Cargardistritos
                LlenarGridDistritos(idUbicacion_temp, dis);
            }
        }

        private void LlenarGridDistritos(int idUbicacion_temp, List<Ubicaciones_Distritos_temp> dis)
        {
            using (var db = new DGHP_Entities())
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("grupoDistrito", typeof(string));
                dt.Columns.Add("distrito", typeof(string));
                dt.Columns.Add("zonas", typeof(string));
                dt.Columns.Add("subzonas", typeof(string));
                dt.Columns.Add("IdDistrito", typeof(string));
                dt.Columns.Add("IdZona", typeof(string));
                dt.Columns.Add("IdSubZona", typeof(string));

                foreach (var e in dis)
                {
                    DataRow datarw;
                    datarw = dt.NewRow();

                    var GrupoDistrito = db.Ubicaciones_GruposDistritos.Where(x => x.IdGrupoDistrito == e.Ubicaciones_CatalogoDistritos.IdGrupoDistrito).Select(y => y.Nombre).FirstOrDefault();

                    var CodDistrito = db.Ubicaciones_CatalogoDistritos.Where(x => x.IdDistrito == e.IdDistrito).Select(y => y.Codigo).FirstOrDefault();

                    var CodZona = db.Ubicaciones_CatalogoDistritos_Zonas.Where(x => x.IdZona == e.IdZona).Select(y => y.CodigoZona).FirstOrDefault();

                    var CodSubZona = db.Ubicaciones_CatalogoDistritos_Subzonas.Where(x => x.IdSubZona == e.IdSubZona).Select(y => y.CodigoSubZona).FirstOrDefault();

                    datarw[0] = HttpUtility.HtmlDecode(GrupoDistrito);
                    datarw[1] = HttpUtility.HtmlDecode(CodDistrito);
                    datarw[2] = HttpUtility.HtmlDecode(CodZona);
                    datarw[3] = HttpUtility.HtmlDecode(CodSubZona);
                    datarw[4] = HttpUtility.HtmlDecode(e.IdDistrito.ToString());
                    datarw[5] = HttpUtility.HtmlDecode(e.IdZona.ToString());
                    datarw[6] = HttpUtility.HtmlDecode(e.IdSubZona.ToString());

                    dt.Rows.Add(datarw);
                }

                grdDistritos.DataSource = dt;
                grdDistritos.DataBind();
            }
        }

        private void LlenarGridMixturas(int idUbicacion_temp, List<Ubicaciones_ZonasMixtura> mix)
        {
            using (var db = new DGHP_Entities())
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("mix", typeof(string));
                dt.Columns.Add("mixDescripcion", typeof(string));

                foreach (var e in mix)
                {
                    DataRow datarw;
                    datarw = dt.NewRow();

                    var mixDescripcion = e.Descripcion;//db.Ubicaciones_ZonasMixtura.Where(x => x.IdZonaMixtura == e.IdZonaMixtura).Select(y => y.Descripcion).First();

                    datarw[0] = HttpUtility.HtmlDecode(e.IdZonaMixtura.ToString());
                    datarw[1] = HttpUtility.HtmlDecode(mixDescripcion.ToString());
                    dt.Rows.Add(datarw);
                }

                grdMixturas.DataSource = dt;
                grdMixturas.DataBind();
            }
        }

        private void LlenarGridUbicaciones(int idUbicacion_temp, List<Ubicaciones_Puertas_temp> up)
        {
            using (var db = new DGHP_Entities())
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("calles", typeof(string));
                dt.Columns.Add("nroPuerta", typeof(string));
                dt.Columns.Add("codigo_calle", typeof(int));

                foreach (var e in up)
                {
                    DataRow datarw;
                    datarw = dt.NewRow();

                    var nombreCalle = db.Calles.Where(x => x.Codigo_calle == e.codigo_calle).Select(y => y.NombreOficial_calle).First();

                    datarw[0] = HttpUtility.HtmlDecode(nombreCalle);
                    if (Shared.esUbicacionEspecialConObjetoTerritorial(idUbicacion_temp))
                    {
                        datarw[1] = HttpUtility.HtmlDecode(e.NroPuerta_ubic.ToString() + 't');
                    }
                    else
                    {
                        datarw[1] = HttpUtility.HtmlDecode(e.NroPuerta_ubic.ToString());
                    }
                    datarw[2] = HttpUtility.HtmlDecode(e.codigo_calle.ToString());
                    dt.Rows.Add(datarw);
                }

                grdUbicaciones.DataSource = dt;
                grdUbicaciones.DataBind();
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
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updDatos, "finalizarCarga();showfrmError();");
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
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updDatos, "finalizarCarga();showfrmError();");
            }
        }

        #endregion

        #region CargarCombos

        private void CargarCombosDistritos()
        {
            using (DGHP_Entities db = new DGHP_Entities())
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
            using (DGHP_Entities db = new DGHP_Entities())
            {
                List<Ubicaciones_ZonasMixtura> lista = db.Ubicaciones_ZonasMixtura.ToList();

                Ubicaciones_ZonasMixtura item = new Ubicaciones_ZonasMixtura();
                item.IdZonaMixtura = 0;
                item.Descripcion = "[Seleccione...]";
                lista.Insert(0, item);
                ddlMixtura.DataTextField = "Descripcion";
                ddlMixtura.DataValueField = "IdZonaMixtura";
                ddlMixtura.DataSource = lista.OrderBy(x => x.Descripcion);
                ddlMixtura.DataBind();
            }
        }

        private void CargarCombo_tipoUbicacion()
        {
            using (var db = new DGHP_Entities())
            {
                List<TiposDeUbicacion> lista = db.TiposDeUbicacion.ToList();
                ddlUbiTipoUbicacion.DataTextField = "descripcion_tipoubicacion";
                ddlUbiTipoUbicacion.DataValueField = "id_tipoubicacion";
                ddlUbiTipoUbicacion.DataSource = lista;
                ddlUbiTipoUbicacion.DataBind();
            }
        }

        private void CargarCombo_subTipoUbicacion(int id_tipoubicacion)
        {
            using (var db = new DGHP_Entities())
            {
                List<SubTiposDeUbicacion> lista = db.SubTiposDeUbicacion.Where(x => x.id_tipoubicacion == id_tipoubicacion && x.habilitado).ToList();
                ddlUbiSubTipoUbicacion.DataTextField = "descripcion_subtipoubicacion";
                ddlUbiSubTipoUbicacion.DataValueField = "id_subtipoubicacion";
                ddlUbiSubTipoUbicacion.DataSource = lista;
                ddlUbiSubTipoUbicacion.DataBind();
            }
        }

        private void CargarCombo_Distritos(int IdGrupoDistrito, bool value)
        {
            using (var db = new DGHP_Entities())
            {
                List<Ubicaciones_CatalogoDistritos> lista = db.Ubicaciones_CatalogoDistritos.Where(x => x.IdGrupoDistrito == IdGrupoDistrito).ToList();

                lista.Insert(0, new Ubicaciones_CatalogoDistritos { IdDistrito = 0, Codigo = "[Seleccione...]" });

                ddlDistritos.DataTextField = "Codigo";
                ddlDistritos.DataValueField = "IdDistrito";
                ddlDistritos.DataSource = lista.OrderBy(x => x.Codigo);
                ddlDistritos.DataBind();

                ddlDistritos.Enabled = value;
            }
        }

        private void CargarCombo_DistritosSubZonas(int idZona, bool value)
        {
            using (DGHP_Entities db = new DGHP_Entities())
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

        private void CargarCombo_DistritosZonas(int IdDistrito, bool value)
        {
            using (DGHP_Entities db = new DGHP_Entities())
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
            using (DGHP_Entities db = new DGHP_Entities())
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
            using (DGHP_Entities db = new DGHP_Entities())
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
            using (DGHP_Entities db = new DGHP_Entities())
            {
                List<Comunas> comunas = db.Comunas.Where(a => a.id_comuna != 17).ToList();
                ddlComuna.DataTextField = "nom_comuna";
                ddlComuna.DataValueField = "id_comuna";
                ddlComuna.DataSource = comunas.OrderBy(p => p.nom_comuna);
                ddlComuna.DataBind();
            }
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
                                     //AlturaMin = cal.AlturaIzquierdaInicio_calle <= cal.AlturaDerechaInicio_calle ? cal.AlturaIzquierdaInicio_calle : cal.AlturaDerechaInicio_calle,
                                     //AlturaMax = cal.AlturaDerechaFin_calle >= cal.AlturaIzquierdaFin_calle ? cal.AlturaDerechaFin_calle : cal.AlturaIzquierdaFin_calle
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
            //public int? AlturaMin { get; set; }
            //public int? AlturaMax { get; set; }
        }

        #endregion

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
                }
                else
                {
                    txtNroPuerta.Text = string.Empty;
                    txtNroPuerta.Focus();
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                this.EjecutarScript(updUbicaciones, "hidefrmAgregarUbicacion();");
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updUbicaciones, "showfrmError();");
            }
        }

        private string GetNombreCalle(int codCalle, int nroPuerta)
        {
            using (var db = new DGHP_Entities())
            {
                var c = (from cal in db.Calles
                         where cal.Codigo_calle == codCalle
                         && (cal.AlturaIzquierdaInicio_calle <= cal.AlturaDerechaInicio_calle ? cal.AlturaIzquierdaInicio_calle : cal.AlturaDerechaInicio_calle) <= nroPuerta
                         && (cal.AlturaDerechaFin_calle >= cal.AlturaIzquierdaFin_calle ? cal.AlturaDerechaFin_calle : cal.AlturaIzquierdaFin_calle) >= nroPuerta
                         select cal).SingleOrDefault();

                if (c != null)
                    return c.NombreOficial_calle;

                return "";
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

        protected void ddlUbiTipoUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id_tipoubicacion = int.Parse(ddlUbiTipoUbicacion.SelectedValue);
                hid_id_tipo_ubicacion.Value = id_tipoubicacion.ToString();
                CargarCombo_subTipoUbicacion(id_tipoubicacion);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

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
                    ddlDistritos.Enabled = false;
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
            updDatos.Update();
        }

        protected void ddlDistritosZonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdZona = int.Parse(ddlDistritosZonas.SelectedValue);
                if (IdZona != 0)
                    CargarCombo_DistritosSubZonas(IdZona, true);
                else
                    ddlDistritosSubZonas.Enabled = false;
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
            updDatos.Update();
        }

        protected void ddlDistritos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdDistrito = int.Parse(ddlDistritos.SelectedValue);
                if (IdDistrito != 0)
                    CargarCombo_DistritosZonas(IdDistrito, true);
                else
                    ddlDistritosZonas.Enabled = false;
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
            updDatos.Update();
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
                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updUbicaciones, "showfrmError();");
                }
            }
        }

        private bool MixRepetida(string mix, string mixDescripcion)
        {
            DataTable dt = dtMixturasCargadas();

            var existe = (from r in dt.AsEnumerable()
                          where r.Field<String>("mix") == mix &&
                                r.Field<String>("mixDescripcion") == mixDescripcion
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

                    int IdDistrito = 0;
                    int.TryParse(ddlDistritos.SelectedValue, out IdDistrito);

                    var IdZonas = 0;
                    var IdSubZonas = 0;

                    if (!String.IsNullOrEmpty(zonas))
                    {
                        IdZonas = ddlDistritosZonas.SelectedIndex;
                    }
                    if (!String.IsNullOrEmpty(subZonas))
                    {
                        IdSubZonas = ddlDistritosSubZonas.SelectedIndex;
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
                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updUbicaciones, "showfrmError();");
                }
            }
        }

        private bool DistritoRepetido(string grupoDistrito, string distrito, string zonas, string subzonas)
        {
            DataTable dt = dtDistritosCargados();

            var existe = (from r in dt.AsEnumerable()
                          where r.Field<String>("grupoDistrito") == grupoDistrito &&
                                r.Field<String>("distrito") == distrito &&
                                r.Field<String>("zonas") == zonas &&
                                r.Field<String>("subzonas") == subzonas
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

            foreach (GridViewRow row in grdDistritos.Rows)
            {
                int distrito = 0;
                int zona = 0;
                int subZona = 0;
                DataRow datarw;
                datarw = dtDistritos.NewRow();
                datarw[0] = HttpUtility.HtmlDecode(row.Cells[0].Text);
                datarw[1] = HttpUtility.HtmlDecode(row.Cells[1].Text);
                datarw[2] = HttpUtility.HtmlDecode(row.Cells[2].Text);
                datarw[3] = HttpUtility.HtmlDecode(row.Cells[3].Text);
                int.TryParse(HttpUtility.HtmlDecode(row.Cells[4].Text), out distrito);
                datarw[4] = distrito;
                int.TryParse(HttpUtility.HtmlDecode(row.Cells[5].Text), out zona);
                datarw[5] = zona;
                int.TryParse(HttpUtility.HtmlDecode(row.Cells[6].Text), out subZona);
                datarw[6] = subZona;
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
            Response.Redirect(getUrlToRedirect(), false);
        }

        private string getUrlToRedirect()
        {
            var idOp = Session["id_operacion"];
            int id_ope = 0;
            string tipo_op = "";

            if (idOp != null && int.TryParse(idOp.ToString(), out id_ope))
                tipo_op = getTipoOperacion(id_ope);

            if (tipo_op.Contains("Subdiv"))
                return "~/ABM/Ubicaciones/SubdividirUbicacion.aspx";
            else if (tipo_op.Contains("Unif"))
                return "~/ABM/Ubicaciones/UnificarUbicaciones.aspx";
            else
                return "~/ABM/Ubicaciones/AbmUbicaciones.aspx";
        }

        private string getTipoOperacion(int id_ope)
        {
            using (var ctx = new DGHP_Entities())
            {
                var op = ctx.Ubicaciones_Operaciones.Where(o => o.id_operacion == id_ope).SingleOrDefault();
                if (op == null)
                    return "";

                var op_tipo = ctx.Ubicaciones_Acciones.Where(a => a.id_accion == op.id_accion).SingleOrDefault();
                if (op_tipo != null)
                    return op_tipo.Descripcion;

                return "";
            }
        }

        // Obtiene el id de la ubicacion temp cuya operacion relacionada se encuentra en proceso
        private int GetIdTempPendienteNroPartida(int nroPartidaMatriz)
        {
            using (var ctx = new DGHP_Entities())
            {
                var u = (from ut in ctx.Ubicaciones_temp
                         join opd in ctx.Ubicaciones_Operaciones_Detalle on ut.id_ubicacion_temp equals opd.id_ubicacion_temp
                         join op in ctx.Ubicaciones_Operaciones on opd.id_operacion equals op.id_operacion
                         where ut.NroPartidaMatriz == nroPartidaMatriz
                         && ut.baja_logica == false && op.id_estado == 0
                         select ut).SingleOrDefault();

                return u != null ? u.id_ubicacion_temp : 0;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            using (var context = new DGHP_Entities())
            {
                try
                {
                    var entity = context.Ubicaciones_temp.Where(x => x.id_ubicacion_temp == this.idUbicacion_temp).FirstOrDefault();

                    bool Baja_Logica = entity.baja_logica;

                    //Subtipo Ubi
                    int UbiSubTipo = 0;
                    int.TryParse(ddlUbiSubTipoUbicacion.SelectedValue, out UbiSubTipo);
                    entity.id_subtipoubicacion = UbiSubTipo;

                    //Nro Partida Matriz
                    int UbiNroPartida = 0;
                    int.TryParse(txtNroPartida.Text.Trim(), out UbiNroPartida);
                    if (UbiNroPartida > 0)
                    {
                        try
                        {
                            int existe = GetIdTempPendienteNroPartida(UbiNroPartida);
                            if (existe != entity.id_ubicacion_temp && existe != 0)
                            {
                                txtNroPartida.Text = "";
                                throw new Exception("Ya existe una partida con el mismo Nro de PartidaMatriz");
                            }
                        }
                        catch (Exception ex)
                        {
                            lblError.Text = Functions.GetErrorMessage(ex);
                            this.EjecutarScript(updBotonesGuardar, "showfrmError();");
                            return;
                        }
                    }
                    entity.NroPartidaMatriz = UbiNroPartida;

                    //Seccion
                    int? UbiSeccionValue = null;
                    int UbiSeccion = 0;
                    int.TryParse(txtSeccion.Text.Trim(), out UbiSeccion);
                    if (UbiSeccion > 0)
                    {
                        UbiSeccionValue = UbiSeccion;
                        entity.Seccion = UbiSeccion;
                    }

                    // Manzana
                    if (txtManzana.Text.Trim() != "")
                        entity.Manzana = txtManzana.Text.Trim();

                    // Parcela
                    if (txtParcela.Text.Trim() != "")
                        entity.Parcela = txtParcela.Text.Trim();

                    // Barrio
                    int barrio = 0;
                    if (int.TryParse(ddlBarrio.SelectedValue, out barrio) && entity.id_barrio != barrio)
                        entity.id_barrio = barrio;

                    // Comisaria
                    int comisaria = 0;
                    if (int.TryParse(ddlComisaria.SelectedValue, out comisaria) && entity.id_comisaria != comisaria)
                        entity.id_comisaria = comisaria;

                    // Comuna
                    int comuna = 0;
                    if (int.TryParse(ddlComuna.SelectedValue, out comuna) && entity.id_comuna != comuna)
                        entity.id_comuna = comuna;

                    entity.Observaciones = txtObservaciones.Text.Trim();

                    entity.EsEntidadGubernamental = chbEntidadGubernamental.Checked;
                    entity.EsUbicacionProtegida = chbEdificioProtegido.Checked;

                    List<Ubicaciones_Puertas_temp> listaPuertasInicial;
                    List<Ubicaciones_ZonasMixtura> listaMixturasInicial;
                    List<Ubicaciones_Distritos_temp> listaDistritosInicial;

                    listaPuertasInicial = entity.Ubicaciones_Puertas_temp.ToList();
                    listaMixturasInicial = entity.Ubicaciones_ZonasMixtura.ToList();
                    listaDistritosInicial = entity.Ubicaciones_Distritos_temp.ToList();

                    //Actualizar
                    List<Ubicaciones_Puertas_temp> listaPuertas = CrearListaPuertas(entity.id_ubicacion_temp, grdUbicaciones, listaPuertasInicial);
                    foreach (var item in listaPuertasInicial)
                    {
                        if (!listaPuertas.Where(p => p.codigo_calle == item.codigo_calle && p.NroPuerta_ubic == item.NroPuerta_ubic).Any())
                        {
                            entity.Ubicaciones_Puertas_temp.Remove(item);
                            context.Ubicaciones_Puertas_temp.Remove(item);
                        }
                    }
                    entity.Ubicaciones_Puertas_temp = listaPuertas;

                    List<Ubicaciones_Distritos_temp> listaDistritos = CrearListaDistritos(entity.id_ubicacion_temp, grdDistritos, listaDistritosInicial);
                    foreach (var item in listaDistritosInicial)
                    {
                        if (!listaDistritos.Where(d => d.IdDistrito == item.IdDistrito && d.id_ubicacion_temp == item.id_ubicacion_temp && d.IdZona == item.IdZona && d.IdSubZona == item.IdSubZona).Any())
                        {
                            entity.Ubicaciones_Distritos_temp.Remove(item);
                            context.Ubicaciones_Distritos_temp.Remove(item);
                        }
                    }
                    entity.Ubicaciones_Distritos_temp = listaDistritos;

                    List<Ubicaciones_ZonasMixtura> listaMix = CrearListaMixturas(entity.id_ubicacion_temp, grdMixturas, listaMixturasInicial);
                    foreach (var item in listaMixturasInicial)
                    {
                        if (!listaMix.Where(m => m.IdZonaMixtura == item.IdZonaMixtura).Any())
                            entity.Ubicaciones_ZonasMixtura.Remove(item);
                    }
                    foreach (var item in listaMix)
                    {
                        Ubicaciones_ZonasMixtura a = context.Ubicaciones_ZonasMixtura.Find(item.IdZonaMixtura);
                        entity.Ubicaciones_ZonasMixtura.Add(a);
                    }

                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                            lblError.Text = Functions.GetErrorMessage(ex);
                            this.EjecutarScript(updBotonesGuardar, "showfrmError();");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updBotonesGuardar, "showfrmError();");
                    return;
                }

                Response.Redirect(getUrlToRedirect(), false);
            }
        }

        private List<Ubicaciones_Distritos_temp> CrearListaDistritos(int id_ubicacion_temp, GridView grdDistritos, List<Ubicaciones_Distritos_temp> listaDistritosInicial)
        {
            var list = new List<Ubicaciones_Distritos_temp>();

            foreach (GridViewRow item in grdDistritos.Rows)
            {
                int id_distrito = Convert.ToInt16(item.Cells[4].Text);
                int? id_Zona = null;
                int? id_SubZona = null;

                if (item.Cells[5].Text != "0" && item.Cells[5].Text != "&nbsp;")
                {
                    id_Zona = Convert.ToInt16(item.Cells[5].Text);
                }
                if (item.Cells[6].Text != "0" && item.Cells[6].Text != "&nbsp;")
                {
                    id_SubZona = Convert.ToInt16(item.Cells[6].Text);
                }

                if (!listaDistritosInicial.Where(x => x.IdZona == id_Zona && x.IdSubZona == id_SubZona && x.IdDistrito == id_distrito).Any())
                {
                    Ubicaciones_Distritos_temp dis = new Ubicaciones_Distritos_temp();
                    dis.id_ubicacion_temp = id_ubicacion_temp;
                    dis.IdDistrito = id_distrito;
                    dis.IdZona = id_Zona;
                    dis.IdSubZona = id_SubZona;
                    list.Add(dis);
                }
                else
                    list.Add(listaDistritosInicial.Where(x => x.IdZona == id_Zona && x.IdSubZona == id_SubZona && x.IdDistrito == id_distrito).SingleOrDefault());

            }
            return list;
        }

        private List<Ubicaciones_ZonasMixtura> CrearListaMixturas(int id_ubicacion_temp, GridView grdMixturas, List<Ubicaciones_ZonasMixtura> listaMixturasInicial)
        {
            var list = new List<Ubicaciones_ZonasMixtura>();

            foreach (GridViewRow item in grdMixturas.Rows)
            {
                list.Add(new Ubicaciones_ZonasMixtura { IdZonaMixtura = Convert.ToInt16(item.Cells[0].Text) });
            }
            return list;
        }

        private List<Ubicaciones_Puertas_temp> CrearListaPuertas(int id_ubicacion_temp, GridView grdUbicaciones, List<Ubicaciones_Puertas_temp> listaPuertasInicial)
        {
            var list = new List<Ubicaciones_Puertas_temp>();

            int cod_calle = 0;
            int NroPuerta = 0;
            var id = NuevoIdUbicacionPuertaTemp();

            foreach (GridViewRow item in grdUbicaciones.Rows)
            {
                cod_calle = Convert.ToInt32(item.Cells[2].Text);
                NroPuerta = Convert.ToInt32(item.Cells[1].Text);

                if (!listaPuertasInicial.Where(x => x.codigo_calle == cod_calle && x.NroPuerta_ubic == NroPuerta).Any())
                {
                    Ubicaciones_Puertas_temp pu = new Ubicaciones_Puertas_temp();
                    pu.id_ubic_puerta_temp = id;
                    pu.id_ubicacion_temp = id_ubicacion_temp;
                    pu.NroPuerta_ubic = NroPuerta;
                    pu.codigo_calle = cod_calle;
                    pu.tipo_puerta = "";

                    list.Add(pu);
                    id++;
                }
                else
                    list.Add(listaPuertasInicial.Where(x => x.codigo_calle == cod_calle && x.NroPuerta_ubic == NroPuerta).SingleOrDefault());
            }

            return list;
        }

        private int NuevoIdUbicacionPuertaTemp()
        {
            using (var db = new DGHP_Entities())
            {
                if (db.Ubicaciones_Puertas_temp.ToList().Count() == 0)
                    return 1;

                // Buscamos el id max
                int? id_max = db.Ubicaciones_Puertas_temp.Max(p => p.id_ubic_puerta_temp);

                // Devolvemos el max + 1
                if (id_max != null)
                    return (int)(id_max + 1);

                // Error
                return -1;
            }
        }
    }
}