using SGI.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucUbicacion : System.Web.UI.UserControl
    {
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

        public void CargarDatos(int id_solicitud)
        {

            DGHP_Entities db = new DGHP_Entities();

            lblZona.Text = "";
            hid_id_solicitud.Value = id_solicitud.ToString();


            gridubicacion_db.DataSource = db.SSIT_Solicitudes_Ubicaciones.Where(x => x.id_solicitud == id_solicitud).ToList();
            gridubicacion_db.DataBind();

            var lstZonas = (from sol in db.SSIT_Solicitudes_Ubicaciones
                            join zon in db.Zonas_Planeamiento on sol.id_zonaplaneamiento equals zon.id_zonaplaneamiento into res
                            from zonEmpty in res.DefaultIfEmpty()
                            where sol.id_solicitud == id_solicitud
                            select new
                            {
                                sol.Zonas_Planeamiento,
                                zonEmpty.DescripcionZonaPla
                            }).ToList();


            // Obtiene la Zona Declarada
            if (lstZonas.Count > 0)
            {
                foreach (var item in lstZonas)
                {
                    lblZona.Text = item.Zonas_Planeamiento.CodZonaPla;
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
                int.TryParse(hid_id_solicitud.Value, out id_solicitud);

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

                int id_solubicacion = Convert.ToInt32(gridubicacion_db.DataKeys[e.Row.RowIndex].Value);
                bool RequiereSMP = true;
                int id_tipoubicacion = 0;


                var dUbic = (from solubic in db.SSIT_Solicitudes_Ubicaciones
                             join ubic in db.Ubicaciones on solubic.id_ubicacion equals ubic.id_ubicacion
                             join stubic in db.SubTiposDeUbicacion on solubic.id_subtipoubicacion equals stubic.id_subtipoubicacion
                             join tubic in db.TiposDeUbicacion on stubic.id_tipoubicacion equals tubic.id_tipoubicacion
                             join zonpla in db.Zonas_Planeamiento on solubic.id_zonaplaneamiento equals zonpla.id_zonaplaneamiento
                             where solubic.id_solicitud == id_solicitud
                             && solubic.id_solicitudubicacion == id_solubicacion
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
                                 solubic.Depto,
                                 zonpla.CodZonaPla,
                                 zonpla.DescripcionZonaPla,
                                 solubic.local_subtipoubicacion
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
                        pnlDeptoLocal.Visible = !string.IsNullOrEmpty(dUbic.Depto);
                    }
                    else
                        pnlDeptoLocal.Visible = true;       // Solo aparece cuando es parcela común, para el resto lo tomna del campo local que pone al buscar



                    lbl_zonificacion.Text = dUbic.CodZonaPla.Trim() + " - " + dUbic.DescripcionZonaPla.Trim();
                    lblDeptoLocalvis2.Text = (!string.IsNullOrEmpty(dUbic.local_subtipoubicacion) ? dUbic.local_subtipoubicacion.Trim() : "");

                    string seccion = (dUbic.Seccion.HasValue ? dUbic.Seccion.Value.ToString() : "");
                    imgFoto.ImageUrl = Functions.GetUrlFoto(seccion, dUbic.Manzana.Trim(), dUbic.Parcela.Trim());

                    dtlPuertas_db.DataSource = (from ubiPuertas in db.SSIT_Solicitudes_Ubicaciones_Puertas
                                                join solubic in db.SSIT_Solicitudes_Ubicaciones on ubiPuertas.id_solicitudubicacion equals solubic.id_solicitudubicacion
                                                join su in db.SubTiposDeUbicacion on solubic.id_subtipoubicacion equals su.id_subtipoubicacion
                                                join tu in db.TiposDeUbicacion on su.id_tipoubicacion equals tu.id_tipoubicacion
                                                where ubiPuertas.id_solicitudubicacion == id_solubicacion
                                                select new
                                                {
                                                    ubiPuertas.id_solicitudpuerta,
                                                    DescripcionCompleta = ubiPuertas.nombre_calle + " " + (tu.id_tipoubicacion == 11 ? ubiPuertas.NroPuerta.ToString() + "t" : ubiPuertas.NroPuerta.ToString()),
                                                    Calle = ubiPuertas.nombre_calle,
                                                    ubiPuertas.codigo_calle,
                                                    ubiPuertas.NroPuerta
                                                }).ToList();

                    dtlPuertas_db.DataBind();

                    if (dtlPuertas_db.Items.Count == 0)
                        pnlPuertas.Visible = false;

                    dtlPartidasHorizontales.DataSource = (from hor in db.Ubicaciones_PropiedadHorizontal
                                                          join solphor in db.SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal on hor.id_propiedadhorizontal equals solphor.id_propiedadhorizontal
                                                          where solphor.id_solicitudubicacion == id_solubicacion
                                                          select new
                                                          {
                                                              solphor.id_propiedadhorizontal,
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

                /*if (!_Editable)
                    CargarPlantasHabilitar(id_solicitud);*/
            }
        }

        /*private void CargarPlantasHabilitar(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();

            var lstPlantas = (from tipsec in db.TipoSector
                              join caaplan in db.ssit_so_Plantas on tipsec.Id equals caaplan.id_tiposector
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

        }*/

        /* protected void btnEliminar_Click(object sender, EventArgs e)
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

         }*/



    }
}