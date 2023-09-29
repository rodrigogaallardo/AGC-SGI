using SGI.Model;
using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using System.Linq;
using System.Configuration;

namespace SGI.Reportes
{
    public partial class ImprimirDispoHtml : System.Web.UI.Page
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

            string fechadecreto = Parametros.GetParam_ValorChar("Dec.197");

            CultureInfo culture = new CultureInfo("es-AR");

            if (Convert.ToDateTime(query_tt_fechainicio.fechainicio, culture) <= Convert.ToDateTime(fechadecreto, culture))
                decreto = "93/GCBA/2006";
            else
                decreto = "197/GCBA/2017";

           
            int id_resultado;
            id_resultado = Functions.isResultadoDispo(this.id_solicitud);
            if (id_resultado == (int)Constants.ENG_ResultadoTarea.Aprobado)
            {
                if (this.id_solicitud >= nroSolReferencia)
                {
                    label1 = HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.AprobActEcon.Label1"), decreto));
                    label3 = HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Aprob.Label2"), "1", "Autorización de Actividad Económica"));
                }
                else
                {
                    label1 = HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Aprob.Label1"), decreto));

                    if (id_tipotramite == (int)Constants.TipoDeTramite.Ampliacion_Unificacion)
                    {
                        label3 = HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Aprob.Label2"), "1", getTipoAmpliacion(id_encomienda)));
                    }
                    else
                    {
                        label3 = HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Aprob.Label2"), "1", Functions.GetTipoDeTramiteDesc(id_tipotramite)));
                    }
                }

                label4 = HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.LabelNew"), "2")) + "<br/><br/>" + 
                    HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Label3"), "3"));
            }
            else if (id_resultado == (int)Constants.ENG_ResultadoTarea.Rechazado
                || id_resultado == (int)Constants.ENG_ResultadoTarea.Requiere_Rechazo
                || id_resultado == (int)Constants.ENG_ResultadoTarea.Rechazado_Reconsideracion) //Rechazo
            {
                if (this.id_solicitud >= nroSolReferencia)
                {
                    label1 = HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.RechazoActEcon.Label1"), decreto));
                    label3 = HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.RechazoActEcon.Label2"), "1"));
                }
                else
                {
                    label1 = HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Rechazo.Label1"), decreto));
                    label3 = HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Rechazo.Label2"), "1"));
                }
                label4 = HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Label3"), "2"));
            }
            //var levanta = false;
            if (id_resultado == 1199 || id_resultado == (int)Constants.ENG_ResultadoTarea.Aprobado_Reconsideracion) 
            {
                label1 = considerando;
                label3 = HttpUtility.HtmlEncode(Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Aprueba.Label2")) + "<br/><br/>" +
                    HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Aprob.Label2"), "2", Functions.GetTipoDeTramiteDesc(id_tipotramite)))+ "<br/><br/>" +
                    HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.LabelNew"), "3", Functions.GetTipoDeTramiteDesc(id_tipotramite))) + "<br/><br/>" +
                    HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Aprueba.LabelNew"), "4", Functions.GetTipoDeTramiteDesc(id_tipotramite)));

                label4 = "";
            }
            if (id_resultado == (int)Constants.ENG_ResultadoTarea.Observacion
                || id_resultado == (int)Constants.ENG_ResultadoTarea.Observado_Reconsideracion) 
            {
                label1 = considerando;
             
                label3 = HttpUtility.HtmlEncode(Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Aprueba.Label2")) + "<br/><br/>" +
                         HttpUtility.HtmlEncode(Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Observa.Label2"));
                label4 = HttpUtility.HtmlEncode(string.Format(Parametros.GetParam_ValorChar("SGI.Dispo.Label3"), "3"));
                pnlDatosSolicitud.Visible = false;
            }
            if (id_resultado == (int)Constants.ENG_ResultadoTarea.Desestima
                || id_resultado == (int)Constants.ENG_ResultadoTarea.Rechazo_No_es_extremporaneo) 
            {
               
                label1 = considerando;
              
                label3 = HttpUtility.HtmlEncode(Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Rechaza.Label2"));
                label4 = HttpUtility.HtmlEncode(Parametros.GetParam_ValorChar("SGI.Dispo.Levanta.Rechaza.Label3"));
                pnlDatosSolicitud.Visible = false;
            }

            Label1.Text = label1;
            Label2.Text = label2;
            Literal3.Text = label3;

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
                string sSeccion = (r["Seccion"].ToString() !=""?Convert.ToString(r["Seccion"]):"-");
                string sManzana = (r["Manzana"].ToString() != "" ?Convert.ToString(r["Manzana"]):"-") ;
                string sParcela = (r["Parcela"].ToString() != "" ?Convert.ToString(r["Parcela"]):"-");
                string sNroPartidaMatriz = (r["NroPartidaMatriz"].ToString() !=""?Convert.ToString(r["NroPartidaMatriz"]):"-");
                TableRow tr1 = new TableRow();
                TableCell tc1 = new TableCell();
                TableCell tc2 = new TableCell();
                TableCell tc3 = new TableCell();
                TableCell tc4 = new TableCell();
                tc1.ColumnSpan = 2;
                tc2.ColumnSpan = 2;
                tc3.ColumnSpan = 2;
                tc4.ColumnSpan = 2;
                tc1.Text = "Sección: " + sSeccion;
                tc2.Text = "Manzana: " + sManzana;
                tc3.Text = "Parcela: " + sParcela;
                tc4.Text = "Partida Matriz: " + sNroPartidaMatriz;
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


            if (this.id_solicitud > nroSolReferencia)
            {
                string zonMix = string.Empty;
                var parameter = new System.Data.Entity.Core.Objects.ObjectParameter("result", "varchar(1000)");
                db.GetMixDistritoZonaySubZonaBySolicitud(id_solicitud, parameter);
                zonMix = parameter.Value.ToString();

                //foreach (DataRow r in dsDispo.Tables["Mixturas"].Rows)
                //    zonMix = zonMix + " - " + Convert.ToString(r["Descripcion"]);
                //foreach (DataRow r in dsDispo.Tables["Distritos"].Rows)
                //    zonMix = zonMix + " - " + Convert.ToString(r["Descripcion"]);

                lblZonaDeclarada.Text = "Mixtura / Área Especial: " + zonMix;
                lblPlantashabilitar.Text = "Plantas a autorizar: ";
                lblSuperficieTotal.Text = "Superficie total a autorizar: ";
            }
            else
            {
                lblZonaDeclarada.Text = "Distrito de zonificación: " + zonaDeclarada;
                lblPlantashabilitar.Text = "Plantas a habilitar: ";
                lblSuperficieTotal.Text = "Superficie total a habilitar: ";
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

            Label1.Text = label1;
            Label2.Text = label2;
            Literal3.Text = label3;
            Label4.Text = label4;

            DateTime d = Convert.ToDateTime(fecha_disposicion);
            lblDia.Text = d.ToString("dd MMMM 'de' yyyy", CultureInfo.CreateSpecificCulture("es-ES"));

            lblNroSolicitud.Text = string.Format("{0:###,###,##0}", this.id_solicitud);
            lblNroExpediente.Text = dispo_nro_expediente;

            db.Dispose();
        }
        

        private string getTipoAmpliacion(int id_encomienda)
        {
            string ret = "";
            bool EsAmpliacionSuperficie = false;
            bool EsAmpliacionRubro = false;

            DGHP_Entities db = new DGHP_Entities();

            var datos_local = db.Encomienda_DatosLocal.FirstOrDefault(x => x.id_encomienda == id_encomienda);

            if (datos_local != null)
                EsAmpliacionSuperficie = (datos_local.ampliacion_superficie.HasValue && datos_local.ampliacion_superficie.Value);

             var lstRubros = db.Encomienda_Rubros.Where(x => x.id_encomienda == id_encomienda).Select(s => s.cod_rubro).ToList();
             var lstRubros_ATAnterior = db.Encomienda_Rubros_AT_Anterior.Where(x => x.id_encomienda == id_encomienda).Select(s => s.cod_rubro).ToList();

            EsAmpliacionRubro = lstRubros.Except(lstRubros_ATAnterior).Count() > 0;


            if (EsAmpliacionSuperficie && EsAmpliacionRubro)
                ret = "Ampliación de Rubro y Superficie";
            else if (EsAmpliacionSuperficie)
                ret = "Ampliación de Superficie";
            else if (EsAmpliacionSuperficie)
                ret = "Ampliación de Rubro";
            else
                ret = "Ampliación";

            return ret;

        }        
    }
}