using SGI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls.CPadron
{
    public class ucEliminarEventsArgs : EventArgs
    {
        public int id_cpadronubicacion { get; set; }
    }
    public class ucEditarEventsArgs : EventArgs
    {
        public int id_cpadronubicacion { get; set; }
    }
    public partial class Ubicacion : System.Web.UI.UserControl
    {
        public delegate void EventHandlerEliminar(object sender, ucEliminarEventsArgs e);
        public event EventHandlerEliminar EliminarClick;

        public delegate void EventHandlerEditar(object sender, ucEditarEventsArgs e);
        public event EventHandlerEditar EditarClick;

        private static bool _Editable;

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(true),
        Description("Devuelve/Establece sies posible eliminar las ubicaciones.")]
        public bool Editable
        {
            get
            {

                if (ViewState["Ubicaciones.ascx._Editable"] != null)
                    Editable = Convert.ToBoolean(ViewState["Ubicaciones.ascx._Editable"]);

                return _Editable;
            }
            set
            {
                // Se debe setear la propiedad editable antes de ejecutar la carga de datos
                ViewState["Ubicaciones.ascx._Editable"] = value;
                _Editable = value;
                pnlPlantasHabilitar.Visible = !value;       // solo es visible al no ser editable
            }
        }

        public void CargarDatos(int id_cpadron)
        {

            DGHP_Entities db = new DGHP_Entities();

            lblZona.Text = "";
            hid_id_cpadron.Value = id_cpadron.ToString();


            gridubicacion_db.DataSource = db.CPadron_Ubicaciones.Where(x => x.id_cpadron == id_cpadron).ToList();
            gridubicacion_db.DataBind();

            var lstZonas = (from sol in db.CPadron_Solicitudes
                            join zon in db.Zonas_Planeamiento on sol.ZonaDeclarada equals zon.CodZonaPla into res
                            from zonEmpty in res.DefaultIfEmpty()
                            where sol.id_cpadron == id_cpadron
                            select new
                            {
                                sol.ZonaDeclarada,
                                zonEmpty.DescripcionZonaPla
                            }).ToList();


            // Obtiene la Zona Declarada
            if (lstZonas.Count > 0)
            {
                foreach (var item in lstZonas)
                {
                    lblZona.Text = item.ZonaDeclarada;
                    if (!string.IsNullOrEmpty(item.DescripcionZonaPla))
                        lblZona.Text += " - " + item.DescripcionZonaPla;

                }
            }
            else
            {
                lblZona.Text = "No definida aún.";
            }

            db.Dispose();


        }

        protected void gridubicacion_db_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DGHP_Entities db = new DGHP_Entities();

                int id_solicitud = 0;
                int.TryParse(hid_id_cpadron.Value, out id_solicitud);

                Label lbl_NroPartidaMatriz = (Label)e.Row.FindControl("grd_NroPartidaMatriz_db");
                Label lbl_seccion = (Label)e.Row.FindControl("grd_seccion_db");
                Label lbl_manzana = (Label)e.Row.FindControl("grd_manzana_db");
                Label lbl_parcela = (Label)e.Row.FindControl("grd_parcela_db");
                Label lbl_zonificacion = (Label)e.Row.FindControl("grd_zonificacion_db");
                Image imgFoto = (Image)e.Row.FindControl("imgFotoParcela_db");

                DataList dtlPuertas_db = (DataList)e.Row.FindControl("dtlPuertas_db");
                DataList dtlPartidasHorizontales = (DataList)e.Row.FindControl("dtlPartidaHorizontales_db");
                Label lblEmptyDataPartidasHorizontales = (Label)e.Row.FindControl("lblEmptyDataPartidasHorizontales_db");
                Label lblDeptoLocalvis2 = (Label)e.Row.FindControl("lblDeptoLocalvis2");
                Panel pnlSMP = (Panel)e.Row.FindControl("pnlSMPview");
                Panel pnlPuertas = (Panel)e.Row.FindControl("pnlPuertasview");
                Panel pnlTipoUbicacion = (Panel)e.Row.FindControl("pnlTipoUbicacionview");
                Label lblTipoUbicacion = (Label)e.Row.FindControl("lblTipoUbicacionview");
                Label lblSubTipoUbicacion = (Label)e.Row.FindControl("lblSubTipoUbicacionview");
                Panel pnlPartidasHorizontales = (Panel)e.Row.FindControl("pnlPartidasHorizontalesview");
                Panel pnlDeptoLocal = (Panel)e.Row.FindControl("pnlDeptoLocalview");
                Label lblLocal = (Label)e.Row.FindControl("lblLocalview");
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                LinkButton btnEditar = (LinkButton)e.Row.FindControl("btnEditar");

                btnEliminar.Visible = _Editable;
                btnEditar.Visible = _Editable;

                int id_cpadronubicacion = Convert.ToInt32(gridubicacion_db.DataKeys[e.Row.RowIndex].Value);
                bool RequiereSMP = true;
                int id_tipoubicacion = 0;


                var dUbic = (from cpubic in db.CPadron_Ubicaciones
                                      join ubic in db.Ubicaciones on cpubic.id_ubicacion equals ubic.id_ubicacion
                                      join stubic in db.SubTiposDeUbicacion on cpubic.id_subtipoubicacion equals stubic.id_subtipoubicacion
                                      join tubic in db.TiposDeUbicacion on stubic.id_tipoubicacion equals tubic.id_tipoubicacion
                                      join zonpla in db.Zonas_Planeamiento on cpubic.id_zonaplaneamiento equals zonpla.id_zonaplaneamiento
                                      where cpubic.id_cpadron == id_solicitud
                                      && cpubic.id_cpadronubicacion == id_cpadronubicacion
                                      select new
                                      {
                                          tubic.id_tipoubicacion,
                                          tubic.RequiereSMP,
                                          tubic.descripcion_tipoubicacion,
                                          stubic.descripcion_subtipoubicacion,
                                          ubic.NroPartidaMatriz,
                                          ubic.Seccion,
                                          ubic.Manzana,
                                          ubic.Parcela,
                                          cpubic.deptoLocal_cpadronubicacion,
                                          zonpla.CodZonaPla,
                                          zonpla.DescripcionZonaPla,
                                          cpubic.local_subtipoubicacion
                                      }).FirstOrDefault();




                if (dUbic != null)
                {

                    id_tipoubicacion = dUbic.id_tipoubicacion;

                    if (dUbic.RequiereSMP.HasValue)
                        RequiereSMP = dUbic.RequiereSMP.Value;

                    if (RequiereSMP)
                    {
                        if (dUbic.NroPartidaMatriz.HasValue)
                            lbl_NroPartidaMatriz.Text = dUbic.NroPartidaMatriz.Value.ToString();

                        if (dUbic.Seccion.HasValue)
                            lbl_seccion.Text = dUbic.Seccion.Value.ToString();

                        lbl_manzana.Text = dUbic.Manzana.Trim();
                        lbl_parcela.Text = dUbic.Parcela.Trim();

                    }

                    pnlSMP.Visible = RequiereSMP;

                    if (!id_tipoubicacion.Equals((int)Constants.TiposDeUbicacion.ParcelaComun))
                    {
                        pnlTipoUbicacion.Visible = true;
                        lblTipoUbicacion.Text = dUbic.descripcion_tipoubicacion.Trim();
                        lblSubTipoUbicacion.Text = dUbic.descripcion_subtipoubicacion.Trim();
                        lblLocal.Text = dUbic.local_subtipoubicacion.Trim();
                        pnlDeptoLocal.Visible = !string.IsNullOrEmpty(dUbic.deptoLocal_cpadronubicacion);
                    }
                    else
                        pnlDeptoLocal.Visible = true;       // Solo aparece cuando es parcela común, para el resto lo tomna del campo local que pone al buscar



                    lbl_zonificacion.Text = dUbic.CodZonaPla.Trim() + " - " + dUbic.DescripcionZonaPla.Trim();
                    lblDeptoLocalvis2.Text = (!string.IsNullOrEmpty(dUbic.deptoLocal_cpadronubicacion) ? dUbic.deptoLocal_cpadronubicacion.Trim() : "");

                    string seccion = (dUbic.Seccion.HasValue ? dUbic.Seccion.Value.ToString() : "");
                    imgFoto.ImageUrl = Functions.GetUrlFoto(seccion, dUbic.Manzana.Trim(), dUbic.Parcela.Trim());

                    dtlPuertas_db.DataSource = (from cppuer in db.CPadron_Ubicaciones_Puertas
                                                where cppuer.id_cpadronubicacion == id_cpadronubicacion
                                                select new
                                                {
                                                    cppuer.id_cpadronpuerta,
                                                    DescripcionCompleta = cppuer.nombre_calle + " " + cppuer.NroPuerta.ToString(),
                                                    Calle = cppuer.nombre_calle,
                                                    cppuer.codigo_calle,
                                                    cppuer.NroPuerta

                                                }).ToList();
                    dtlPuertas_db.DataBind();

                    if (dtlPuertas_db.Items.Count == 0)
                        pnlPuertas.Visible = false;

                    dtlPartidasHorizontales.DataSource = (from hor in db.Ubicaciones_PropiedadHorizontal
                                                          join cpphor in db.CPadron_Ubicaciones_PropiedadHorizontal on hor.id_propiedadhorizontal equals cpphor.id_propiedadhorizontal
                                                          where cpphor.id_cpadronubicacion == id_cpadronubicacion
                                                          select new
                                                          {
                                                              cpphor.id_propiedadhorizontal,
                                                              hor.NroPartidaHorizontal,
                                                              hor.Piso,
                                                              hor.Depto,
                                                              DescripcionCompleta = hor.NroPartidaHorizontal.ToString() +
                                                              (hor.Piso.Length > 0 ? " - Piso: " + hor.Piso : "") +
                                                              (hor.Depto.Length > 0 ? " U.F.: " + hor.Depto : "")
                                                          }).ToList();

                    dtlPartidasHorizontales.DataBind();


                    if (dtlPartidasHorizontales.Items.Count > 0)
                    {
                        lblEmptyDataPartidasHorizontales.Visible = false;
                        pnlPartidasHorizontales.Visible = true;
                    }
                    else
                    {
                        lblEmptyDataPartidasHorizontales.Visible = true;
                        pnlPartidasHorizontales.Visible = false;
                    }

                }

                db.Dispose();

                if (!_Editable)
                    CargarPlantasHabilitar(id_solicitud);
            }
        }

        private void CargarPlantasHabilitar(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();

            var lstPlantas = (from tipsec in db.TipoSector
                              join caaplan in db.CPadron_Plantas on tipsec.Id equals caaplan.id_tiposector
                              where caaplan.id_cpadron == id_solicitud
                              orderby tipsec.Id
                              select new
                              {
                                  id_tiposector = tipsec.Id,
                                  Seleccionado = (caaplan.id_tiposector > 0),
                                  tipsec.Descripcion,
                                  tipsec.MuestraCampoAdicional,
                                  detalle = caaplan.detalle_cpadrontiposector,
                                  TamanoCampoAdicional = (tipsec.TamanoCampoAdicional.HasValue ? tipsec.TamanoCampoAdicional.Value : 0)
                              }).Distinct().ToList();


            foreach (var item in lstPlantas)
            {
                int TamanoCampoAdicional = item.TamanoCampoAdicional;
                bool MuestraCampoAdicional = false;
                string separador = "";

                if (item.MuestraCampoAdicional.HasValue)
                    MuestraCampoAdicional = item.MuestraCampoAdicional.Value;


                if (lblPlantasHabilitar.Text.Length == 0)
                    separador = "";
                else
                    separador = ", ";

                if (MuestraCampoAdicional)
                {
                    if (TamanoCampoAdicional >= 10)
                        lblPlantasHabilitar.Text += separador + item.detalle.Trim();
                    else
                        lblPlantasHabilitar.Text += separador + item.Descripcion.Trim() + " " + item.detalle.Trim();
                }
                else
                    lblPlantasHabilitar.Text += separador + item.Descripcion.Trim();
            }

        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            LinkButton btnEliminar = (LinkButton)sender;
            int id_cpadronubicacion = int.Parse(btnEliminar.CommandArgument);

            ucEliminarEventsArgs args = new ucEliminarEventsArgs();
            args.id_cpadronubicacion = id_cpadronubicacion;
            EliminarClick(sender, args);

        }
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            LinkButton btnEditar = (LinkButton)sender;
            int id_cpadronubicacion = int.Parse(btnEditar.CommandArgument);

            ucEditarEventsArgs args = new ucEditarEventsArgs();
            args.id_cpadronubicacion = id_cpadronubicacion;
            EditarClick(sender, args);

        }



    }
}