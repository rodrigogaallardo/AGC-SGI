using SGI.Model;
using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.ABM.Ubicaciones
{
    public partial class EditarUbicacion : BasePage
    {
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
        int idUbicacion = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "init_Js_updDatos", "init_Js_updDatos();", true);
                ScriptManager.RegisterStartupScript(updBodyAgregarUbicacion, updBodyAgregarUbicacion.GetType(), "init_Js_updBodyAgregarUbicacion", "init_Js_updBodyAgregarUbicacion();", true);
            }
            if (!IsPostBack)
            {
                // Variable id de la operacion de modificación
                Session["id_operacion"] = 0;

                // Id Ubicación temporal (id_ubicacion_temp) de la Parcela a modificar 
                Session["id_ubi_tmp"] = 0;

                // Id Ubicación modificada (id_ubicacion) generada desde la ubicación temp
                Session["id_ubi_nva"] = 0;
            }

            //cargarPermisos();
            if (Request.QueryString["Id"] != null)
            {
                this.idUbicacion = Convert.ToInt32(Request.QueryString["Id"].ToString());
            }
            else
            {
                Response.Redirect("~/ABM/Ubicaciones/AbmUbicaciones.aspx");
            }
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

                LoadUbicacion(this.idUbicacion);

                this.EjecutarScript(updDatos, "finalizarCarga();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updDatos, "finalizarCarga();showfrmError();");
            }
        }

        private void LoadUbicacion(int idUbicacion)
        {
            using (var db = new DGHP_Entities())
            {
                var u = (from ubi in db.Ubicaciones
                         where ubi.id_ubicacion == idUbicacion
                         select ubi).FirstOrDefault();

                var up = (from ubi in db.Ubicaciones_Puertas
                          where ubi.id_ubicacion == idUbicacion
                          select ubi).ToList();

                var mix = (from ubi in db.Ubicaciones_ZonasMixtura
                           from ubiZon in ubi.Ubicaciones.Where(x => x.id_ubicacion == idUbicacion)
                           select ubi).Distinct().ToList();

                var dis = (from d in db.Ubicaciones_Distritos
                           where d.id_ubicacion == idUbicacion
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
                    int idTipoUbi = (int)u.SubTiposDeUbicacion.id_tipoubicacion;

                    var idTipo = db.TiposDeUbicacion.Where(x => x.id_tipoubicacion == idTipoUbi).SingleOrDefault();
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

                if (u.baja_logica)
                {
                    rbtnBajaSi.Checked = true;
                }
                else
                    rbtnBajaNo.Checked = true;

                //CargarCallesyPuertas
                LlenarGridUbicaciones(idUbicacion, up);
                //CargarMixturas
                LlenarGridMixturas(idUbicacion, mix);
                //Cargardistritos
                LlenarGridDistritos(idUbicacion, dis);
            }
        }

        private void LlenarGridDistritos(int idUbicacion, List<Ubicaciones_Distritos> dis)
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

                    var CodDistrito1 = db.Ubicaciones_CatalogoDistritos.Where(x => x.IdDistrito == e.IdDistrito).Select(y => y.Codigo).FirstOrDefault();

                    var CodDistrito = (from ucd in db.Ubicaciones_CatalogoDistritos
                                       where ucd.IdDistrito == e.IdDistrito
                                       select ucd.Codigo + " - " + ucd.Descripcion).FirstOrDefault().ToString();

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

        private void LlenarGridMixturas(int idUbicacion, List<Ubicaciones_ZonasMixtura> mix)
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

                    var mixDescripcion = e.Descripcion;

                    datarw[0] = HttpUtility.HtmlDecode(e.IdZonaMixtura.ToString());
                    datarw[1] = HttpUtility.HtmlDecode(mixDescripcion.ToString());
                    dt.Rows.Add(datarw);
                }

                grdMixturas.DataSource = dt;
                grdMixturas.DataBind();
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
                         select cal).FirstOrDefault();

                if (c != null)
                    return c.NombreOficial_calle;

                return "";
            }
        }

        private void LlenarGridUbicaciones(int idUbicacion, List<Ubicaciones_Puertas> up)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("calles", typeof(string));
            dt.Columns.Add("nroPuerta", typeof(string));
            dt.Columns.Add("codigo_calle", typeof(int));

            foreach (var e in up)
            {
                DataRow datarw;
                datarw = dt.NewRow();

                datarw[0] = HttpUtility.HtmlDecode(GetNombreCalle(e.codigo_calle, e.NroPuerta_ubic));
                if (Shared.esUbicacionEspecialConObjetoTerritorial(idUbicacion))
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

        protected void btnEliminarUbicacion_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEliminarUbicacion = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnEliminarUbicacion.Parent.Parent;
                DataTable dt = dtUbicacionesCargadas();
                dt.Rows.RemoveAt(row.RowIndex);

                if (Shared.esUbicacionEspecialConObjetoTerritorial(this.idUbicacion))
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        item[1] = item[1].ToString() + 't';
                    }
                }

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
            dt.Columns.Add("nroPuerta", typeof(string));
            dt.Columns.Add("codigo_calle", typeof(int));

            foreach (GridViewRow row in grdUbicaciones.Rows)
            {
                DataRow datarw;
                datarw = dt.NewRow();
                datarw[0] = HttpUtility.HtmlDecode(row.Cells[0].Text);
                var numeroPuerta = row.Cells[1].Text;
                if (numeroPuerta.Last() == 't')
                {
                    datarw[1] = numeroPuerta.Substring(0, numeroPuerta.Length - 1);
                }
                else
                {
                    datarw[1] = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[1].Text));
                }
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

                if (Shared.esUbicacionEspecialConObjetoTerritorial(this.idUbicacion))
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        item[1] = item[1].ToString() + 't';
                    }
                }

                grdUbicaciones.DataSource = dt;
                grdUbicaciones.DataBind();
                this.EjecutarScript(updUbicaciones, "hidefrmAgregarUbicacion();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                this.EjecutarScript(updUbicaciones, "hidefrmAgregarUbicacion();");
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updUbicaciones, "showfrmError();");
            }
        }

        private bool ubicacionRepetida(string Calle, int nroPuertaUbi)
        {
            DataTable dt = dtUbicacionesCargadas();

            var existe = (from r in dt.AsEnumerable()
                          where r.Field<String>("calles").Contains(Calle) &&
                                r.Field<String>("nroPuerta") == nroPuertaUbi.ToString()
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
                    CargarCombo_Distritos(IdGrupoDistrito, true);
                else
                    ddlDistritos.Enabled = false;
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
            updDatos.Update();
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
                        int.TryParse(ddlDistritosZonas.SelectedValue, out IdZonas);
                    }
                    if (!String.IsNullOrEmpty(subZonas))
                    {
                        int.TryParse(ddlDistritosSubZonas.SelectedValue, out IdSubZonas);
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
            //}

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
            // Anula operación generada
            int id_op = (int)Session["id_operacion"];

            if (id_op != 0 && !AnularOperacion(id_op))
                throw new Exception("Error al anular la operación. Intente nuevamente.");

            int id_ubi_tmp = (int)Session["id_ubi_tmp"];
            if (id_ubi_tmp != 0)
                BajaUbiTemp(id_ubi_tmp);

            // Liberamos las variables
            Session["id_operacion"] = 0;
            Session["id_ubi_tmp"] = 0;
            Session["id_ubi_nva"] = 0;

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

        private void Continuar_Guardar()
        {
            bool actualizarUbi = false;
            try
            {
                using (var context = new DGHP_Entities())
                {
                    var entity = context.Ubicaciones.Where(x => x.id_ubicacion == this.idUbicacion).FirstOrDefault();
                    /* Se comenta porque estaba borrando las T en objeto Territorial
                    if (Shared.esUbicacionEspecialConObjetoTerritorial(entity.id_ubicacion))
                    {
                        if ("T" == txtManzana.Text.Substring(0, 1).ToUpper())
                        {
                            txtManzana.Text = txtManzana.Text.Substring(1);
                        }
                        if (txtParcela.Text.Substring(txtParcela.Text.Length - 1) == "t")
                        {
                            txtParcela.Text = txtParcela.Text.Substring(0, txtParcela.Text.Length - 1);
                        }
                    }
                    */
                    actualizarUbi = entity != null
                                        && entity.Seccion.ToString() == txtSeccion.Text.Trim()
                                        && entity.Manzana.ToString() == txtManzana.Text.Trim()
                                        && entity.Parcela.ToString() == txtParcela.Text.Trim();
                }

                if (actualizarUbi)
                {
                    ActualizarUbicacion();
                }
                else
                {
                    if ((int)Session["id_ubi_tmp"] == 0 && GuardarUbicacionTmp() == 0)
                        throw new Exception("Error al registrar las modificaciones temporales. Intente nuevamente.");

                    if ((int)Session["id_ubi_nva"] == 0 && GuardarUbicacionFromTmp((int)Session["id_ubi_tmp"]) == 0)
                        throw new Exception("Error al registrar las modificaciones. Intente nuevamente.");

                    int id_op = (int)Session["id_operacion"];
                    if (id_op == 0)
                    {
                        id_op = GenerarOperacionModificacionEnProceso();
                        if (id_op == 0)
                            throw new Exception("Error al registrar la operación. Intente nuevamente.");
                        Session["id_operacion"] = id_op;
                    }

                    if (GenerarOpDetalle(id_op, (int)this.idUbicacion) == 0)
                        throw new Exception("Error al registrar el detalle de la operación. Intente nuevamente.");

                    if (GenerarOpDetalle(id_op, (int)Session["id_ubi_nva"], (int)Session["id_ubi_tmp"], txtObservaciones.Text.Trim()) == 0)
                        throw new Exception("Error al registrar el detalle de la operación. Intente nuevmente.");

                    // Damos de baja la ubicacion padre
                    bool baja = BajaUbicacion(this.idUbicacion);
                    if (!baja)
                        throw new Exception("No fue posible realizar la baja de la parcela subdividida. Intente nuevamente.");

                    if (!ConfirmarOperacion(id_op))
                        throw new Exception("Error al confirmar la operación. Intente nuevamente.");

                    // Liberamos las variables
                    Session["id_operacion"] = 0;
                    Session["id_ubi_tmp"] = 0;
                    Session["id_ubi_nva"] = 0;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updBotonesGuardar, "showfrmError();");
                return;
            }

            Response.Redirect("~/ABM/Ubicaciones/AbmUbicaciones.aspx");
        }

        private bool BajaUbicacion(int id_ubi)
        {
            using (var ctx = new DGHP_Entities())
            {
                var ubi = ctx.Ubicaciones.Where(u => u.id_ubicacion == id_ubi).SingleOrDefault();
                if (ubi != null)
                    ubi.baja_logica = true;

                return ctx.SaveChanges() != 0;
            }
        }

        private bool BajaUbiTemp(int id_ubi_temp)
        {
            using (var ctx = new DGHP_Entities())
            {
                var ubi_tmp = ctx.Ubicaciones_temp.Where(u => u.id_ubicacion_temp == id_ubi_temp).SingleOrDefault();
                if (ubi_tmp != null && !ubi_tmp.baja_logica)
                {
                    ubi_tmp.baja_logica = true;
                    return ctx.SaveChanges() != 0;
                }
            }

            return false;
        }

        private int GuardarUbicacionTmp()
        {
            int idNuevo = 0;

            using (var context = new DGHP_Entities())
            {
                bool Baja_Logica = false;

                //Subtipo Ubi
                int UbiSubTipo = 0;
                int.TryParse(ddlUbiSubTipoUbicacion.SelectedValue, out UbiSubTipo);

                //Nro Partida Matriz
                int? UbiNroPartidaValue = null;
                int UbiNroPartida = 0;
                int.TryParse(txtNroPartida.Text.Trim(), out UbiNroPartida);
                if (UbiNroPartida > 0)
                {
                    UbiNroPartidaValue = UbiNroPartida;

                    var existe = (from ubic in context.Ubicaciones
                                  where ubic.NroPartidaMatriz == UbiNroPartidaValue.Value
                                  && ubic.baja_logica == false && ubic.id_ubicacion != this.idUbicacion
                                  select ubic.id_ubicacion);

                    if (existe != null && existe.Count() > 0)
                    {
                        txtNroPartida.Text = "";
                        throw new Exception("Ya existe una partida con el mismo Nro de PartidaMatriz");
                    }
                }

                //Seccion
                int? UbiSeccionValue = null;
                int UbiSeccion = 0;
                int.TryParse(txtSeccion.Text.Trim(), out UbiSeccion);
                if (UbiSeccion > 0)
                    UbiSeccionValue = UbiSeccion;

                // Barrio
                int barrio = 0;
                int.TryParse(ddlBarrio.SelectedValue, out barrio);

                // Comisaria
                int comisaria = 0;
                int.TryParse(ddlComisaria.SelectedValue, out comisaria);

                // Comuna
                int comuna = 0;
                int.TryParse(ddlComuna.SelectedValue, out comuna);

                idNuevo = NuevoId_ubicacion_temp();
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region Ubicacion
                        var entity = new SGI.Model.Ubicaciones_temp()
                        {
                            id_ubicacion_temp = idNuevo,
                            id_subtipoubicacion = UbiSubTipo,
                            baja_logica = Baja_Logica,
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
                            Observaciones = this.txtObservaciones.Text.Trim()
                        };

                        #endregion
                        // Agregamos la Ubicacion temporal
                        context.Ubicaciones_temp.Add(entity);

                        // Agregamos las Puertas temporales
                        List<Ubicaciones_Puertas_temp> listaPuertas = CrearListaPuertasTemp(entity.id_ubicacion_temp, grdUbicaciones);
                        entity.Ubicaciones_Puertas_temp = listaPuertas;

                        // Agregamos las Mixturas temporales
                        List<Ubicaciones_ZonasMixtura> listaMix = CrearListaMixturas_temp(entity.id_ubicacion_temp, grdMixturas);
                        foreach (var item in listaMix)
                        {
                            Ubicaciones_ZonasMixtura a = context.Ubicaciones_ZonasMixtura.Find(item.IdZonaMixtura);
                            entity.Ubicaciones_ZonasMixtura.Add(a);
                        }
                        // Agregamos los Distritos temporales
                        List<Ubicaciones_Distritos_temp> listaDistritos = CrearListaDistritosTemp(entity.id_ubicacion_temp, grdDistritos);
                        entity.Ubicaciones_Distritos_temp = listaDistritos;

                        // Guardamos todos los cambios
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }
            }

            Session["id_ubi_tmp"] = idNuevo;
            return idNuevo;
        }

        private void ValidarPuertas()
        {
            DGHP_Entities context = new DGHP_Entities();

            var entity = context.Ubicaciones.Where(x => x.id_ubicacion == this.idUbicacion).FirstOrDefault();
            List<Ubicaciones_Puertas> listaPuertasInicial = entity.Ubicaciones_Puertas.ToList();

            List<Ubicaciones_Puertas> listaPuertas = CrearListaPuertas(idUbicacion, grdUbicaciones, listaPuertasInicial);

            //existe direccion y puerta en otra ubicacion
            StringBuilder puertas = new StringBuilder();
            foreach (var item in listaPuertas)
            {
                var existePuerta = (from ubic in context.Ubicaciones
                                    join ubiPuerta in context.Ubicaciones_Puertas on ubic.id_ubicacion equals ubiPuerta.id_ubicacion
                                    where ubiPuerta.NroPuerta_ubic == item.NroPuerta_ubic && ubiPuerta.codigo_calle == item.codigo_calle
                                    && ubic.baja_logica == false && ubic.id_ubicacion != idUbicacion
                                    select ubic).FirstOrDefault();
                if (existePuerta != null)
                    puertas.AppendLine($"Puerta {item.NroPuerta_ubic} . Ya existe la calle y puerta asociada a otra Ubicación (Sección: {existePuerta.Seccion} - Manzana {existePuerta.Manzana} - Parcela: {existePuerta.Parcela}");
            }

            if (puertas.ToString() != "")
            {
                lblAviso.Text = puertas.ToString();
                this.EjecutarScript(updBotonesGuardar, "showfrmAviso();");
            }
            else
                Continuar_Guardar();
        }


        private bool ActualizarUbicacion()
        {
            using (var context = new DGHP_Entities())
            {
                var entity = context.Ubicaciones.Where(x => x.id_ubicacion == this.idUbicacion).FirstOrDefault();

                #region Ubicacion
                bool Baja_Logica = entity.baja_logica;
                //modifico
                bool comunicoBajaFachadas = false;
                if (Baja_Logica == false && this.rbtnBajaSi.Checked == true)
                    comunicoBajaFachadas = true;
                //
                if (this.rbtnBajaSi.Checked == true && this.rbtnBajaNo.Checked == false)
                    entity.baja_logica = true;
                if (this.rbtnBajaNo.Checked == true && this.rbtnBajaSi.Checked == false)
                    entity.baja_logica = false;

                //Subtipo Ubi
                int UbiSubTipo = 0;
                int.TryParse(ddlUbiSubTipoUbicacion.SelectedValue, out UbiSubTipo);
                entity.id_subtipoubicacion = UbiSubTipo;

                //Nro Partida Matriz
                int? UbiNroPartidaValue = null;
                int UbiNroPartida = 0;
                int.TryParse(txtNroPartida.Text.Trim(), out UbiNroPartida);
                if (UbiNroPartida > 0)
                {
                    UbiNroPartidaValue = UbiNroPartida;
                    var existe = (from ubic in context.Ubicaciones
                                  where ubic.NroPartidaMatriz == UbiNroPartidaValue.Value
                                  && ubic.id_ubicacion != entity.id_ubicacion
                                  && ubic.baja_logica == false
                                  select ubic.id_ubicacion);

                    if (existe != null && existe.Count() > 0)
                    {
                        frmerrortitle.Text = "Advertencia";
                        throw new Exception("Ya existe una partida con el mismo Nro de PartidaMatriz");
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

                List<Ubicaciones_Puertas> listaPuertasInicial;
                List<Ubicaciones_ZonasMixtura> listaMixturasInicial;
                List<Ubicaciones_Distritos> listaDistritosInicial;

                listaPuertasInicial = entity.Ubicaciones_Puertas.ToList();
                listaMixturasInicial = entity.Ubicaciones_ZonasMixtura.ToList();
                listaDistritosInicial = entity.Ubicaciones_Distritos.ToList();

                #endregion
                //Actualizar
                List<Ubicaciones_Puertas> listaPuertas = CrearListaPuertas(entity.id_ubicacion, grdUbicaciones, listaPuertasInicial);

                foreach (var item in listaPuertasInicial)
                {
                    if (!listaPuertas.Where(p => p.codigo_calle == item.codigo_calle && p.NroPuerta_ubic == item.NroPuerta_ubic).Any())
                    {
                        entity.Ubicaciones_Puertas.Remove(item);
                        context.Ubicaciones_Puertas.Remove(item);
                    }
                }
                entity.Ubicaciones_Puertas = listaPuertas;

                List<Ubicaciones_Distritos> listaDistritos = CrearListaDistritos(entity.id_ubicacion, grdDistritos, listaDistritosInicial);
                foreach (var item in listaDistritosInicial)
                {
                    if (!listaDistritos.Where(d => d.IdDistrito == item.IdDistrito && d.id_ubicacion == item.id_ubicacion && d.IdZona == item.IdZona && d.IdSubZona == item.IdSubZona).Any())
                    {
                        entity.Ubicaciones_Distritos.Remove(item);
                        context.Ubicaciones_Distritos.Remove(item);
                    }
                }
                entity.Ubicaciones_Distritos = listaDistritos;

                List<Ubicaciones_ZonasMixtura> listaMix = CrearListaMixturas(entity.id_ubicacion, grdMixturas, listaMixturasInicial);
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

                        var User = Parametros.GetParam_ValorChar("User.Ley257");
                        string Pass = Parametros.GetParam_ValorChar("Pass.Ley257");
                        string URL = Parametros.GetParam_ValorChar("URL.Ley257");
                        var ActionLogin = Parametros.GetParam_ValorChar("Action.Login.Ley257");
                        var ActionDarBajaUbicacion = Parametros.GetParam_ValorChar("Action.DarBajaUbicacion.Ley257");
                        if (string.IsNullOrEmpty(ActionDarBajaUbicacion))
                        {
                            comunicoBajaFachadas = false;
                        }

                        if (comunicoBajaFachadas)
                        {
                            ws_Ley257 serv = new ws_Ley257();
                            var tokenResponse = serv.Token(URL, ActionLogin, User, Pass);

                            if (tokenResponse.IsSuccess)
                            {
                                var token = (Ley257Token)tokenResponse.Result;
                                var data = new Ley257RequestDarBajaUbicacion
                                {
                                    Seccion = (int)entity.Seccion,
                                    Manzana = entity.Manzana,
                                    Parcela = entity.Parcela,
                                    UbicacionID = entity.id_ubicacion
                                };

                                // Llamo al método DarBajaUbicacion
                                var darBajaResponse = serv.DarBajaUbicacion(token.AccessToken, URL, ActionDarBajaUbicacion, data);

                                if (darBajaResponse.IsSuccess)
                                {
                                    string Message = darBajaResponse.Message;
                                }
                                else
                                {
                                    string errorMessage = darBajaResponse.Message;
                                }
                            }
                            else
                            {
                                string errorMessage = tokenResponse.Message;
                            }

                        }

                        dbContextTransaction.Dispose();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }
            }
            return true;
        }

        private int GuardarUbicacionFromTmp(int id_ubi_tmp)
        {
            int result = 0;
            using (var context = new DGHP_Entities())
            {
                var ubi_temp = context.Ubicaciones_temp.Where(u => u.id_ubicacion_temp == id_ubi_tmp).SingleOrDefault();
                if (ubi_temp == null)
                    return 0;

                int idNuevo = NuevoId_ubicacion();
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region Ubicacion
                        var entity = new SGI.Model.Ubicaciones()
                        {
                            id_ubicacion = idNuevo,
                            id_subtipoubicacion = ubi_temp.id_subtipoubicacion,
                            baja_logica = ubi_temp.baja_logica,
                            CantiActualizacionesUSIG = ubi_temp.CantiActualizacionesUSIG,
                            cant_ph = ubi_temp.cant_ph,
                            Circunscripcion = ubi_temp.Circunscripcion,
                            id_comisaria = ubi_temp.id_comisaria,
                            id_areahospitalaria = ubi_temp.id_areahospitalaria,
                            id_barrio = ubi_temp.id_barrio,
                            id_comuna = ubi_temp.id_comuna,
                            id_distritoescolar = ubi_temp.id_distritoescolar,
                            id_regionsanitaria = ubi_temp.id_regionsanitaria,
                            id_zonaplaneamiento = ubi_temp.id_zonaplaneamiento,
                            CreateDate = DateTime.Now,
                            VigenciaDesde = DateTime.Now,
                            CreateUser = Functions.GetUserId(),
                            UpdateDate = DateTime.Now,
                            UpdateUser = Functions.GetUserId(),
                            EsEntidadGubernamental = ubi_temp.EsEntidadGubernamental,
                            EsUbicacionProtegida = ubi_temp.EsUbicacionProtegida,
                            NroPartidaMatriz = ubi_temp.NroPartidaMatriz,
                            Seccion = ubi_temp.Seccion,
                            Manzana = ubi_temp.Manzana,
                            Parcela = ubi_temp.Parcela,
                            Observaciones = ubi_temp.Observaciones
                        };

                        #endregion
                        // Agregamos la Ubicacion 
                        context.Ubicaciones.Add(entity);

                        // Agregamos las Puertas 
                        var puertas_tmp = context.Ubicaciones_Puertas_temp.Where(p => p.id_ubicacion_temp == ubi_temp.id_ubicacion_temp).ToList();
                        int id_pue_ini = NuevoIdUbicacionPuerta();
                        foreach (var pue_t in puertas_tmp)
                        {
                            var p = new Ubicaciones_Puertas();

                            p.id_ubic_puerta = id_pue_ini;
                            p.id_ubicacion = idNuevo;
                            p.codigo_calle = pue_t.codigo_calle;
                            p.NroPuerta_ubic = pue_t.NroPuerta_ubic;
                            p.tipo_puerta = pue_t.tipo_puerta;

                            entity.Ubicaciones_Puertas.Add(p);
                            id_pue_ini++;
                        }

                        // Agregamos las Mixturas 
                        entity.Ubicaciones_ZonasMixtura = ubi_temp.Ubicaciones_ZonasMixtura;

                        // Agregamos los Distritos
                        var dist_tmp = context.Ubicaciones_Distritos_temp.Where(d => d.id_ubicacion_temp == ubi_temp.id_ubicacion_temp).ToList();
                        foreach (var item in dist_tmp)
                        {
                            Ubicaciones_Distritos dis = new Ubicaciones_Distritos();

                            dis.id_ubicacion = idNuevo;
                            dis.IdDistrito = item.IdDistrito;
                            dis.IdZona = item.IdZona;
                            dis.IdSubZona = item.IdSubZona;

                            entity.Ubicaciones_Distritos.Add(dis);
                        }

                        // Guardamos todos los cambios
                        context.SaveChanges();
                        dbContextTransaction.Commit();

                        result = idNuevo;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            dbContextTransaction.Rollback();
                        }
                        catch (Exception)
                        {

                        }

                        throw ex;
                    }
                }
            }

            Session["id_ubi_nva"] = result;
            return result;
        }

        private int GenerarOperacion(int accion, int estado, Guid uid)
        {
            using (var ctx = new DGHP_Entities())
            {
                // Nueva operación
                var ope = new Ubicaciones_Operaciones
                {
                    id_accion = accion,
                    CreateDate = DateTime.Now,
                    CreateUser = uid,
                    id_estado = estado
                };

                ctx.Ubicaciones_Operaciones.Add(ope);
                var res = ctx.SaveChanges();

                if (res != 0)
                    return ope.id_operacion;
            }

            return 0;
        }

        private int GenerarOperacionModificacionEnProceso()
        {
            // Modificación
            int id_acc = 3;
            // Obtenemos el user 
            Guid userid = Functions.GetUserId();
            // En Proceso
            int id_est = 0;

            return GenerarOperacion(id_acc, id_est, userid);
        }

        private int GenerarOpDetalle(int id_ope, int id_ubi = 0, int id_ubi_temp = 0, string detalle = "")
        {
            using (var ctx = new DGHP_Entities())
            {
                Ubicaciones_Operaciones_Detalle op_existe = null;
                // Nueva op detalle
                var op_det = new Ubicaciones_Operaciones_Detalle { id_operacion = id_ope };

                if (id_ubi != 0)
                {
                    op_det.id_ubicacion = id_ubi;
                    op_existe = ctx.Ubicaciones_Operaciones_Detalle.Where(o => o.id_operacion == id_ope && o.id_ubicacion == id_ubi).SingleOrDefault();
                }

                if (id_ubi_temp != 0)
                {
                    op_det.id_ubicacion_temp = id_ubi_temp;
                    op_existe = ctx.Ubicaciones_Operaciones_Detalle.Where(o => o.id_operacion == id_ope && o.id_ubicacion_temp == id_ubi_temp).SingleOrDefault();
                }

                if (detalle != "")
                    op_det.Detalle = detalle;

                // Agregamos y guardamos
                if (op_existe == null)
                {
                    ctx.Ubicaciones_Operaciones_Detalle.Add(op_det);
                    var res = ctx.SaveChanges();
                    if (res != 0)
                        return op_det.id_operacion_det;
                }
                else
                    return op_existe.id_operacion_det;
            }

            return 0;
        }

        private bool ConfirmarOperacion(int id_op)
        {
            using (var ctx = new DGHP_Entities())
            {
                var op = ctx.Ubicaciones_Operaciones.Where(o => o.id_operacion == id_op).SingleOrDefault();
                if (op != null)
                    op.id_estado = 2; // Confirmada
                return ctx.SaveChanges() != 0;
            }
        }

        private int ExisteUbicacionOp(int id_temp, int id_op)
        {
            using (var ctx = new DGHP_Entities())
            {
                // Verificamos si ya esta registrada la ubicacion 
                if (id_op != 0)
                {
                    var op = ctx.Ubicaciones_Operaciones_Detalle.Where(o => o.id_ubicacion_temp == id_temp && o.id_operacion == id_op).SingleOrDefault();
                    if (op != null)
                        return op.id_ubicacion != null ? (int)op.id_ubicacion : 0;
                }
                return 0;
            }
        }

        private bool ActualizarOperacionIdUbicacion(int id_operacion, int id_temp, int id_ubicacion)
        {
            using (var ctx = new DGHP_Entities())
            {
                // Actualizamos el id_ubicacion de la operación 
                if (id_operacion != 0)
                {
                    var op = ctx.Ubicaciones_Operaciones_Detalle.Where(o => o.id_ubicacion_temp == id_temp && o.id_operacion == id_operacion).SingleOrDefault();
                    if (op != null)
                        op.id_ubicacion = id_ubicacion;

                    var res = ctx.SaveChanges();
                    return res > 0;
                }
                return false;
            }
        }

        private bool AnularOperacion(int id_op)
        {
            if (id_op <= 0)
                return false;

            using (var ctx = new DGHP_Entities())
            {
                // Buscamos la operacion para anularla
                var op = ctx.Ubicaciones_Operaciones.Where(o => o.id_operacion == id_op).SingleOrDefault();
                if (op != null)
                {
                    // Buscamos si tiene ubicaciones temporales 
                    var ops_det = ctx.Ubicaciones_Operaciones_Detalle.Where(o => o.id_operacion == id_op && o.id_ubicacion_temp != null).ToList();
                    foreach (var op_d in ops_det)
                    {
                        // Damos de baja las ubicaciones temporales
                        var ubi = ctx.Ubicaciones_temp.Where(u => u.id_ubicacion_temp == op_d.id_ubicacion_temp).SingleOrDefault();
                        if (ubi != null)
                            ubi.baja_logica = true;
                    }

                    op.id_estado = 1; // Anulada

                    var res = ctx.SaveChanges();
                    return res != 0;
                }
            }

            return false;
        }

        private List<Ubicaciones_Puertas_temp> CrearListaPuertasTemp(int id_ubicacion_temp, GridView grdCalleSub)
        {
            var list = new List<Ubicaciones_Puertas_temp>();

            var id = NuevoIdUbicacionPuertaTemp();
            foreach (GridViewRow item in grdCalleSub.Rows)
            {
                Ubicaciones_Puertas_temp pu = new Ubicaciones_Puertas_temp();
                pu.id_ubic_puerta_temp = id;
                pu.id_ubicacion_temp = id_ubicacion_temp;
                pu.NroPuerta_ubic = Convert.ToInt32(item.Cells[1].Text);
                pu.codigo_calle = Convert.ToInt32(item.Cells[2].Text);
                pu.tipo_puerta = "";
                list.Add(pu);
                id++;
            }
            return list;
        }

        private List<Ubicaciones_Distritos_temp> CrearListaDistritosTemp(int id_ubicacion_temp, GridView grdDistritos)
        {
            var list = new List<Ubicaciones_Distritos_temp>();

            foreach (GridViewRow item in grdDistritos.Rows)
            {
                Ubicaciones_Distritos_temp dis = new Ubicaciones_Distritos_temp();

                dis.id_ubicacion_temp = id_ubicacion_temp;
                dis.IdDistrito = Convert.ToInt16(item.Cells[4].Text);

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
                dis.IdZona = id_Zona;
                dis.IdSubZona = id_SubZona;
                list.Add(dis);
            }
            return list;
        }

        private List<Ubicaciones_ZonasMixtura> CrearListaMixturas_temp(int id_ubi_temp, GridView grdMixturas)
        {
            var list = new List<Ubicaciones_ZonasMixtura>();
            foreach (GridViewRow item in grdMixturas.Rows)
            {
                list.Add(new Ubicaciones_ZonasMixtura { IdZonaMixtura = Convert.ToInt16(item.Cells[0].Text) });
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

        private int NuevoIdUbicacionPuerta()
        {
            using (var db = new DGHP_Entities())
            {
                int id_max = db.Ubicaciones_Puertas.Max(p => p.id_ubic_puerta) + 1;
                return id_max;
            }
        }

        private int NuevoId_ubicacion()
        {
            using (var db = new DGHP_Entities())
            {
                int id_max = db.Ubicaciones.Max(p => p.id_ubicacion) + 1;
                return id_max;
            }
        }

        private int NuevoId_ubicacion_temp()
        {
            using (var db = new DGHP_Entities())
            {
                if (db.Ubicaciones_temp.ToList().Count() == 0)
                    return 1;
                // Buscamos el id max
                int? id_max = db.Ubicaciones_temp.Max(p => p.id_ubicacion_temp);

                // Devolvemos el max + 1
                if (id_max != null)
                    return (int)(id_max + 1);

                // Error
                return -1;
            }
        }

        private List<Ubicaciones_Distritos> CrearListaDistritos(int id_ubicacion, GridView grdDistritos, List<Ubicaciones_Distritos> listaDistritosInicial)
        {
            var list = new List<Ubicaciones_Distritos>();

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
                    Ubicaciones_Distritos dis = new Ubicaciones_Distritos();
                    dis.id_ubicacion = id_ubicacion;
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

        private List<Ubicaciones_ZonasMixtura> CrearListaMixturas(int id_ubicacion, GridView grdMixturas, List<Ubicaciones_ZonasMixtura> listaMixturasInicial)
        {
            var list = new List<Ubicaciones_ZonasMixtura>();

            foreach (GridViewRow item in grdMixturas.Rows)
            {
                list.Add(new Ubicaciones_ZonasMixtura { IdZonaMixtura = Convert.ToInt16(item.Cells[0].Text) });
            }
            return list;
        }

        private List<Ubicaciones_Puertas> CrearListaPuertas(int id_ubicacion, GridView grdUbicaciones, List<Ubicaciones_Puertas> listaPuertasInicial)
        {
            var list = new List<Ubicaciones_Puertas>();

            int cod_calle = 0;
            int NroPuerta = 0;

            var id = NuevoIdUbicacionPuerta();

            bool borrartNumeroPuerta = Shared.esUbicacionEspecialConObjetoTerritorial(idUbicacion);

            foreach (GridViewRow item in grdUbicaciones.Rows)
            {
                if (borrartNumeroPuerta)
                {
                    NroPuerta = Convert.ToInt32(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1));
                }
                else
                {
                    NroPuerta = Convert.ToInt32(item.Cells[1].Text);
                }

                cod_calle = Convert.ToInt32(item.Cells[2].Text);

                if (Shared.esUbicacionEspecialConObjetoTerritorial(id_ubicacion))
                {
                    NroPuerta = Convert.ToInt32(item.Cells[1].Text.Substring(0, item.Cells[1].Text.Length - 1));
                }
                if (!listaPuertasInicial.Where(x => x.codigo_calle == cod_calle && x.NroPuerta_ubic == NroPuerta).Any())
                {
                    Ubicaciones_Puertas pu = new Ubicaciones_Puertas();
                    pu.id_ubic_puerta = id;
                    pu.id_ubicacion = id_ubicacion;
                    pu.NroPuerta_ubic = NroPuerta;
                    pu.codigo_calle = cod_calle;
                    pu.tipo_puerta = "";

                    list.Add(pu);
                    id++;
                }
                else
                {
                    list.Add(listaPuertasInicial.Where(x => x.codigo_calle == cod_calle && x.NroPuerta_ubic == NroPuerta).SingleOrDefault());
                }
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