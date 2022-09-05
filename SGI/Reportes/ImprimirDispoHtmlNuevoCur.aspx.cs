using SGI.Model;
using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using System.Linq;
using System.Configuration;
using System.Text;

namespace SGI.Reportes
{
    public partial class ImprimirDispoHtmlNuevoCur : System.Web.UI.Page
    {
        private int id_solicitud = 0;
        private int id_tramitetarea = 0;
        private string nro_expediente;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string strID = (Request.QueryString["id_solicitud"] == null) ? "" : Request.QueryString["id_solicitud"].ToString();
                    if (string.IsNullOrEmpty(strID))
                    {
                        strID = Page.RouteData.Values["id_solicitud"].ToString(); // ejemplo route
                    }
                    this.id_solicitud = (strID == null) ? 0 : Convert.ToInt32(strID);

                    strID = (Request.QueryString["id_tramitetarea"] == null) ? "" : Request.QueryString["id_tramitetarea"].ToString();
                    if (string.IsNullOrEmpty(strID))
                    {
                        strID = Page.RouteData.Values["id_tramitetarea"].ToString(); // ejemplo route
                    }
                    this.id_tramitetarea = (strID == null) ? 0 : Convert.ToInt32(strID);

                    strID = (Request.QueryString["nro_expediente"] == null) ? "" : Request.QueryString["nro_expediente"].ToString();
                    if (string.IsNullOrEmpty(strID))
                    {
                        strID = Page.RouteData.Values["nro_expediente"] == null ? "" : Page.RouteData.Values["nro_expediente"].ToString(); // ejemplo route
                    }
                    this.nro_expediente = (strID == null) ? "" : strID;

                    cargar_datos();
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }
        }

        private void cargar_datos()
        {

            PdfDisposicion dispo = null;
            App_Data.dsImpresionDisposicion dsDispo = null;
            DGHP_Entities db = new DGHP_Entities();
            string decreto = string.Empty;
            string considerando = string.Empty;

            int id_tipotramite = 0;
            int id_encomienda = 0;

            int nroSolReferencia = 0;
            int.TryParse(ConfigurationManager.AppSettings["NroSolicitudReferencia"], out nroSolReferencia);

            try
            {
                dispo = new PdfDisposicion();
                dsDispo = dispo.GenerarDataSetDisposicion(this.id_solicitud, this.id_tramitetarea, this.nro_expediente, false);
                considerando = dispo.Buscar_Considerando_Dispo_RR(this.id_solicitud, this.id_tramitetarea);
                dispo.Dispose();
            }
            catch (Exception ex)
            {
                dispo.Dispose();
                throw ex;
            }

            DataRow row = null;

            row = dsDispo.Tables["Disposicion"].Rows[0];
            DataRow rowEncomienda = dsDispo.Tables["Encomienda"].Rows[0];

            id_tipotramite = Convert.ToInt32(rowEncomienda["id_tipotramite"]);
            id_encomienda = Convert.ToInt32(rowEncomienda["id_encomienda"]);

            string dispo_observacion = HttpUtility.HtmlEncode(Convert.ToString(row["observacion_disposicion"]));
            string dispo_calificador = HttpUtility.HtmlEncode(Convert.ToString(row["calificador_apellido"]) + ", " + Convert.ToString(row["calificador_nombre"]));
            string dispo_nro_expediente = HttpUtility.HtmlEncode(Convert.ToString(row["expediente"]));
            string fecha_disposicion = HttpUtility.HtmlEncode(Convert.ToString(row["fecha_disposicion"]));
            string label1 = "";
            string label2 = "";
            string label3 = "";
            string label4 = "";
            string label5 = "";
            string label6 = "";
            
            string labelArt3 = "";

            var sol = (from s in db.SSIT_Solicitudes where s.id_solicitud == this.id_solicitud select s).FirstOrDefault();
            string TipoTramiteDescripcion = (
                    ((sol != null) && (!(bool)sol.EsECI)) ? sol.TipoTramite.descripcion_tipotramite
                    : (
                        sol.id_tipotramite == (int)Constants.TipoDeTramite.HabilitacionECIHabilitacion
                        ? Constants.TipoTramiteDescripcion.HabilitacionECI
                     : Constants.TipoTramiteDescripcion.AdecuacionECI
                     )
                   );
            label3 = TipoTramiteDescripcion;

            //label3 = db.TipoTramite.Where(x => x.id_tipotramite == id_tipotramite).FirstOrDefault().descripcion_tipotramite;
            //buscar fecha de inicio en SGI
            var query_tt_fechainicio =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        orderby tt.id_tramitetarea ascending
                        select new
                        {
                            fechainicio = tt.FechaInicio_tramitetarea
                        }

                    ).FirstOrDefault();


            int id_resultado;
            id_resultado = Functions.isResultadoDispo(this.id_solicitud);
            if (id_resultado == (int)Constants.ENG_ResultadoTarea.Aprobado)
            {
                label1 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "CON.DISPO.APRUEBA").FirstOrDefault().texto;
                label2 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART1.DISPO.APRUEBA").FirstOrDefault().texto;
                label4 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART2.DISPO.APRUEBA").FirstOrDefault().texto;
                label5 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART3.DISPO.APRUEBA").FirstOrDefault().texto;
                label6 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART4.DISPO.APRUEBA").FirstOrDefault().texto;
            }
            else if (id_resultado == (int)Constants.ENG_ResultadoTarea.Rechazado
                || id_resultado == (int)Constants.ENG_ResultadoTarea.Requiere_Rechazo
                || id_resultado == (int)Constants.ENG_ResultadoTarea.Rechazado_Reconsideracion) //Rechazo
            {
                label1 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "CON.DISPO.RECHAZO").FirstOrDefault().texto;
                label2 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART1.DISPO.RECHAZO").FirstOrDefault().texto;
                label4 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART2.DISPO.RECHAZO").FirstOrDefault().texto;
            }
            else if (Functions.caduco(this.id_solicitud))
            {
                label1 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "CON.DISPO.CADUCO").FirstOrDefault().texto;
                label2 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART1.DISPO.CADUCO").FirstOrDefault().texto;
                label4 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART2.DISPO.CADUCO").FirstOrDefault().texto;
                label5 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART3.DISPO.CADUCO").FirstOrDefault().texto;
            }
            else if (id_resultado == (int)Constants.ENG_ResultadoTarea.Aprobado_Reconsideracion)
            {
                label1 = considerando;

                label2 = Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Aprueba.Label2") + "<br><br>" +
                string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Aprob.Label2"), "2", Functions.GetTipoDeTramiteDesc(id_tipotramite)) + "<br><br>";
                labelArt3 = string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.LabelNew"), "3", Functions.GetTipoDeTramiteDesc(id_tipotramite)) + "<br><br>" +
                            string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Aprueba.LabelNew"), "4", Functions.GetTipoDeTramiteDesc(id_tipotramite));
                
                PanelArtAprobadoReconsideracion.Visible = true;

            }
            else if (id_resultado == (int)Constants.ENG_ResultadoTarea.Observado_Reconsideracion)
            {
                label1 = considerando;

                label2 = Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Aprueba.Label2") + "<br><br>" +
                            Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Observa.Label2");
                label4 = string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Label3"), "3");
                pnlDatosSolicitud.Visible = false;
            }
            else if (id_resultado == (int)Constants.ENG_ResultadoTarea.Desestima
                || id_resultado == (int)Constants.ENG_ResultadoTarea.Rechazo_No_es_extremporaneo) 
            {
                label1 = considerando;

                label2 = Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Rechaza.Label2");
                label4 = Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Rechaza.Label3");
                pnlDatosSolicitud.Visible = false;
            }

            Label1.Text = label1;
            Label2.Text = label2;
            Label3.Text = label3;
            Label8.Text = labelArt3;

            row = dsDispo.Tables["DatosLocal"].Rows[0];

            decimal superficie_cubierta_dl = Convert.ToDecimal(row["superficie_cubierta_dl"]);
            decimal superficie_descubierta_dl = Convert.ToDecimal(row["superficie_descubierta_dl"]);

            if (row["ampliacion_superficie"] != null && Convert.ToBoolean(row["ampliacion_superficie"]))
            {
                superficie_cubierta_dl = Convert.ToDecimal(row["superficie_cubierta_amp"]);
                superficie_descubierta_dl = Convert.ToDecimal(row["superficie_descubierta_amp"]);
            }

            decimal sup_total_habilitar = superficie_cubierta_dl + superficie_descubierta_dl;
            string cantidad_operarios_dl = Convert.ToString(row["cantidad_operarios_dl"]);


            string zonaDeclarada = Convert.ToString(rowEncomienda["ZonaDeclarada"]);
            string plantasHabilitar = Convert.ToString(rowEncomienda["PlantasHabilitar"]);

            lblNroSolicitud.Text = string.Format("{0:###,###,##0}", this.id_solicitud);
            lblNroExpediente.Text = dispo_nro_expediente;

            repeater_titulares.DataSource = dsDispo.Tables["Titulares"];
            repeater_titulares.DataBind();
            int id_encomiendaubicacion;
            int id_encomiendaubicacion2;
            foreach (DataRow r in dsDispo.Tables["Ubicaciones"].Rows)
            {
                TableRow tr1 = new TableRow();
                TableCell tc1 = new TableCell();
                TableCell tc2 = new TableCell();
                TableCell tc3 = new TableCell();
                TableCell tc4 = new TableCell();
                tc1.ColumnSpan = 2;
                tc2.ColumnSpan = 2;
                tc3.ColumnSpan = 2;
                tc4.ColumnSpan = 2;
                tc1.Text = "Sección: " + Convert.ToString(r["Seccion"]);
                tc2.Text = "Manzana: " + Convert.ToString(r["Manzana"]);
                tc3.Text = "Parcela: " + Convert.ToString(r["Parcela"]);
                tc4.Text = "Partida Matriz: " + Convert.ToString(r["NroPartidaMatriz"]);
                tr1.Cells.Add(tc1);
                tr1.Cells.Add(tc2);
                tr1.Cells.Add(tc3);
                tr1.Cells.Add(tc4);
                tblUbicaciones.Rows.Add(tr1);

                TableRow tr2 = new TableRow();
                TableCell tc5 = new TableCell();
                tc5.ColumnSpan = 8;
                tc5.Text = "Domicilio/s: <b>" + HttpUtility.HtmlEncode(Convert.ToString(r["Direcciones"])) + "</b>";
                tr2.Cells.Add(tc5);
                tblUbicaciones.Rows.Add(tr2);

                id_encomiendaubicacion = Convert.ToInt32(r["id_encomiendaubicacion"]);

                foreach (DataRow rh in dsDispo.Tables["PropiedadHorizontal"].Rows)
                {
                    id_encomiendaubicacion2 = Convert.ToInt32(rh["id_encomiendaubicacion"]);
                    if (id_encomiendaubicacion == id_encomiendaubicacion2)
                    {
                        TableRow tr = new TableRow();
                        TableCell tc6 = new TableCell();
                        TableCell tc7 = new TableCell();
                        TableCell tc8 = new TableCell();
                        tc6.ColumnSpan = 2;
                        tc6.Text = "Partida horizontal: " + Convert.ToString(rh["NroPartidaHorizontal"]);
                        tc7.ColumnSpan = 2;
                        tc7.Text = "Piso: " + HttpUtility.HtmlEncode(Convert.ToString(rh["Piso"]));
                        tc8.ColumnSpan = 2;
                        tc8.Text = "U.F.: " + HttpUtility.HtmlEncode(Convert.ToString(rh["Depto"]));
                        tr.Cells.Add(tc6);
                        tr.Cells.Add(tc7);
                        tr.Cells.Add(tc8);
                        tblUbicaciones.Rows.Add(tr);
                    }
                }

            }


            string zonMix = string.Empty;
            foreach (DataRow r in dsDispo.Tables["Mixturas"].Rows)
                zonMix = zonMix + " - " + Convert.ToString(r["Descripcion"]);
            foreach (DataRow r in dsDispo.Tables["Distritos"].Rows)
                zonMix = zonMix + " - " + Convert.ToString(r["Descripcion"]);

            if (Functions.caduco(this.id_solicitud))
            {
                lblCiudResp.Text = "Ciudadano Responsable:";
                lblZonaDeclarada.Text = "Emplazado en: " + zonMix;
                lblPlantashabilitar.Text = "Plantas: ";
                lblSuperficieTotal.Text = "Superficie total: ";
            }
            else
            {
                lblCiudResp.Text = "En favor del Ciudadano Responsable:";
                lblZonaDeclarada.Text = "Mixtura / Área Especial: " + zonMix;
                lblPlantashabilitar.Text = "Plantas a autorizar: ";
                lblSuperficieTotal.Text = "Superficie total a autorizar: ";
            }

            lblPlantashabilitar.Text = lblPlantashabilitar.Text + HttpUtility.HtmlEncode(plantasHabilitar);
            lblCantOperarios.Text = cantidad_operarios_dl;

            lblSuperficieTotal.Text = lblSuperficieTotal.Text + string.Format("{0:###,###,##0.00} m2.", sup_total_habilitar);
            lblCalificar.Text = dispo_calificador;

            grdRubros.DataSource = dsDispo.Tables["Rubros"];
            grdRubros.DataBind();

            grdSubRubros.DataSource = dsDispo.Tables["SubRubros"];
            grdSubRubros.DataBind();
            grdSubRubros.Visible = grdSubRubros.Rows.Count > 0;

            var depos = dsDispo.Tables["EncomiendaDepositos"];
            grdDepositos.DataSource = depos;
            grdDepositos.DataBind();
            grdDepositos.Visible = depos.Rows.Count > 0;
            
            lblObservacion.Text = lblObservacion.Text + dispo_observacion;

            pnlObservacion.Visible = (!string.IsNullOrEmpty(lblObservacion.Text));

            Label1.Text = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("ISO-8859-1"), Encoding.UTF8.GetBytes(label1)));
            Label2.Text = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("ISO-8859-1"), Encoding.UTF8.GetBytes(label2)));
            Label3.Text = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("ISO-8859-1"), Encoding.UTF8.GetBytes(label3)));
            Label4.Text = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("ISO-8859-1"), Encoding.UTF8.GetBytes(label4)));
            Label5.Text = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("ISO-8859-1"), Encoding.UTF8.GetBytes(label5)));
            Label6.Text = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("ISO-8859-1"), Encoding.UTF8.GetBytes(label6)));

            Label8.Text = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("ISO-8859-1"), Encoding.UTF8.GetBytes(labelArt3)));

            DateTime d = Convert.ToDateTime(fecha_disposicion);
            lblDia.Text = d.ToString("dd MMMM 'de' yyyy", CultureInfo.CreateSpecificCulture("es-ES"));

            lblNroSolicitud.Text = string.Format("{0:###,###,##0}", this.id_solicitud);
            lblNroExpediente.Text = dispo_nro_expediente;

            db.Dispose();
        }
    }
}