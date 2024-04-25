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
    public partial class VerUbicacion : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int idUbicacion = Convert.ToInt32(Request.QueryString["Id"].ToString());

            CargarCombo_tipoUbicacion(true);
            CargarCombo_subTipoUbicacion(true);
            LoadUbicacion(idUbicacion);
            this.EjecutarScript(updDatos, "finalizarCarga();");
        }

        private void CargarCombo_tipoUbicacion(Boolean busqueda)
        {
            using (var db = new DGHP_Entities())
            {
                List<TiposDeUbicacion> lista = db.TiposDeUbicacion.ToList();
                TiposDeUbicacion tipo_ubi = new TiposDeUbicacion();
                tipo_ubi.id_tipoubicacion = -1;
                tipo_ubi.descripcion_tipoubicacion = (busqueda) ? "Seleccione" : "Parcela Común";
                lista.Insert(0, tipo_ubi);
                if (busqueda)
                {
                    ddlbiTipoUbicacion.DataTextField = "descripcion_tipoubicacion";
                    ddlbiTipoUbicacion.DataValueField = "id_tipoubicacion";

                    ddlbiTipoUbicacion.DataSource = lista;
                    ddlbiTipoUbicacion.DataBind();
                }
            }
        }

        private void CargarCombo_subTipoUbicacion(Boolean busqueda)
        {
            using (var db = new DGHP_Entities())
            {
                List<SubTiposDeUbicacion> lista = db.SubTiposDeUbicacion.Where(x => x.habilitado == true).ToList();
                SubTiposDeUbicacion sub_tipo_ubi = new SubTiposDeUbicacion();
                sub_tipo_ubi.id_subtipoubicacion = 0;
                sub_tipo_ubi.descripcion_subtipoubicacion = (busqueda) ? "Seleccione" : "Ninguno";
                lista.Insert(0, sub_tipo_ubi);
                if (busqueda)
                {
                    ddlUbiSubTipoUbicacion.DataTextField = "descripcion_subtipoubicacion";
                    ddlUbiSubTipoUbicacion.DataValueField = "id_subtipoubicacion";
                    ddlUbiSubTipoUbicacion.DataSource = lista;
                    ddlUbiSubTipoUbicacion.DataBind();
                }
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

                if (u.id_subtipoubicacion != null)
                {
                    int idSubTipoUbic = (int)u.SubTiposDeUbicacion.id_subtipoubicacion;
                    int idTipoUbic = (int)u.SubTiposDeUbicacion.id_tipoubicacion;
                    if (Shared.esUbicacionEspecialConObjetoTerritorial(idTipoUbic))
                    {
                        //txtManzana.Text = 'T' + txtManzana.Text;
                        //txtParcela.Text = txtParcela.Text + 't';

                        txtManzana.Text = txtManzana.Text;
                        txtParcela.Text = txtParcela.Text;
                    }

                    ddlUbiSubTipoUbicacion.Items.FindByText(db.SubTiposDeUbicacion.Where(x => x.id_subtipoubicacion == idSubTipoUbic).Select(x => x.descripcion_subtipoubicacion).First()).Selected = true;
                    ddlbiTipoUbicacion.Items.FindByText(db.TiposDeUbicacion.Where(x => x.id_tipoubicacion == idTipoUbic).Select(x => x.descripcion_tipoubicacion).First()).Selected = true;
                }

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
                mix.OrderByDescending(x => x.IdZonaMixtura);

                foreach (var e in mix)
                {
                    DataRow datarw;
                    datarw = dt.NewRow();
                    var mixDescripcion = e.Descripcion;
                    datarw[0] = HttpUtility.HtmlDecode(e.IdZonaMixtura.ToString());
                    datarw[1] = HttpUtility.HtmlDecode(mixDescripcion.ToString());
                    dt.Rows.Add(datarw);
                }

                DataView dv = new DataView(dt);
                dv.Sort = "mix DESC";
                dt = dv.ToTable();
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

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            this.EjecutarScript(updDatos, "finalizarCarga();");
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ABM/Ubicaciones/AbmUbicaciones.aspx");
        }
    }
}