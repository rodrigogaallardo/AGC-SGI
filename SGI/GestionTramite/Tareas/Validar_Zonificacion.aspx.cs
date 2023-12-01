using SGI.GestionTramite.Controls;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SGI.GestionTramite.Tareas
{
    public partial class Validar_Zonificacion : System.Web.UI.Page
    {

        #region cargar inicial

        // private Constants.ENG_Tareas tarea_pagina = Constants.ENG_Tareas.SSP_Validar_Zonificacion;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0);
                if (id_tramitetarea > 0)
                    await CargarDatosTramite(id_tramitetarea);

            }
        }

        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
        }

        #region entity

        private DGHP_Entities db = null;

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }
        }

        #endregion

        private async Task CargarDatosTramite(int id_tramitetarea)
        {

            Guid userid = Functions.GetUserId();

            this.db = new DGHP_Entities();

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            if (tramite_tarea == null)
            {
                this.db.Dispose();
                throw new Exception(string.Format("No se encontro en la tabla SGI_tramites_tareas un registro coincidente con el id = {0}", id_tramitetarea));
            }

            //Se debe establecer siempre el estado de controles antes del load de los controles
            //----
            bool IsEditable = Engine.CheckEditTarea(id_tramitetarea, userid);
            ucResultadoTarea.Enabled = IsEditable;
            ucObservacionesTarea.Enabled = IsEditable;
            ucDocumentoAdjunto.Enabled = IsEditable;

            int id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            this.id_solicitud = ttHAB.id_solicitud;
            this.TramiteTarea = id_tramitetarea;

            SGI_Tarea_Validar_Zonificacion zonificar = Buscar_Tarea(id_tramitetarea);

            ucCabecera.LoadData(id_grupotramite, this.id_solicitud);
            ucListaRubros.LoadData(this.id_solicitud);
            ucTramitesRelacionados.LoadData(this.id_solicitud);
            await ucListaDocumentos.LoadData(id_grupotramite, this.id_solicitud);
            ucResultadoTarea.LoadData(id_grupotramite, id_tramitetarea, true);
            ucSGI_ListaDocumentoAdjuntoAnteriores.LoadData(id_grupotramite, this.id_solicitud, this.TramiteTarea);
            ucDocumentoAdjunto.LoadData(id_grupotramite, this.id_solicitud, id_tramitetarea);
            string observacion = "";
            if (zonificar != null)
            {
                observacion = zonificar.Observaciones != null ? zonificar.Observaciones : "";
            }
            else
            {
                SGI_Tarea_Validar_Zonificacion zonificarAnt = Buscar_Tarea_Anterior(id_tramitetarea);
                observacion = zonificarAnt.Observaciones != null ? zonificarAnt.Observaciones : "";
            }

            ucObservacionesTarea.Text = observacion;

            CargarDatosSolicitud(this.id_solicitud);

            this.db.Dispose();

        }

        private void CargarDatosSolicitud(int id_solicitud)
        {
            var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();


            List<ENC_Ubicacion> lista_ubi =
                (
                    from zona_pla in db.Zonas_Planeamiento
                    join enc_ubi in db.Encomienda_Ubicaciones on enc.id_encomienda equals enc_ubi.id_encomienda
                    //join enc_puerta in db.Encomienda_Ubicaciones_Puertas on enc_ubi.id_encomiendaubicacion equals enc_puerta.id_encomiendaubicacion
                    join ubi in db.Ubicaciones on enc_ubi.id_ubicacion equals ubi.id_ubicacion
                    orderby enc_ubi.id_encomiendaubicacion

                    where enc_ubi.id_encomienda == enc.id_encomienda
                    select new ENC_Ubicacion
                    {
                        id_encomienda = enc.id_encomienda,
                        ZonaDeclarada = enc.ZonaDeclarada,
                        DescripcionZonaPla = zona_pla.DescripcionZonaPla,
                        NroPartidaMatriz = ubi.NroPartidaMatriz,
                        id_encomiendaubicacion = enc_ubi.id_encomiendaubicacion,
                        Seccion = ubi.Seccion,
                        Manzana = ubi.Manzana,
                        Parcela = ubi.Parcela,
                        id_ubicacion = ubi.id_ubicacion,
                        id_subtipoubicacion = enc_ubi.id_subtipoubicacion
                    }
                ).ToList();


            grdDatosUbicacion.DataSource = lista_ubi;
            grdDatosUbicacion.DataBind();

            //string objResult = db.SGI_GetDireccionEncomienda(q.id_encomienda).FirstOrDefault();


            //buscar partida matriz
            //var ph =
            //    (
            //        from enc_ubi_ph in db.Encomienda_Ubicaciones_PropiedadHorizontal
            //        join ubi_ph in db.Ubicaciones_PropiedadHorizontal on enc_ubi_ph.id_propiedadhorizontal equals ubi_ph.id_propiedadhorizontal
            //        where enc_ubi_ph.id_encomiendaubicacion == q.id_encomiendaubicacion
            //        orderby enc_ubi_ph.id_encomiendaprophorizontal
            //        select new
            //        {
            //            ubi_ph.NroPartidaHorizontal,
            //            ubi_ph.Piso,
            //            ubi_ph.Depto,
            //            ubi_ph.UnidadFuncional
            //        }
            //    ).FirstOrDefault();

            //Crear la tarea validar zonificación.

            //La misma debe mostrar:
            //- Zona Actual 
            //- Nomenclatura catastral ( SMP )
            //- nros de partidas ( matriz y horizontal ) 
            //- mapas de la ubicación ( usig)



        }

        protected void grdDatosUbicacion_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int id_encomiendaubicacion = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id_encomiendaubicacion"));
                int id_ubicacion = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id_ubicacion"));

                DataList lstDatosUbicacion_Puertas = (DataList)e.Row.FindControl("lstDatosUbicacion_Puertas");

                List<ENC_Ubicacion.Puerta> lista_puertas =
                    (
                        from enc_ubi_puerta in db.Encomienda_Ubicaciones_Puertas
                        where enc_ubi_puerta.id_encomiendaubicacion == id_encomiendaubicacion
                        select new ENC_Ubicacion.Puerta
                        {
                            id_encomiendapuerta = enc_ubi_puerta.id_encomiendapuerta,
                            id_encomiendaubicacion = enc_ubi_puerta.id_encomiendaubicacion,
                            codigo_calle = enc_ubi_puerta.codigo_calle,
                            Nombre_calle = enc_ubi_puerta.nombre_calle,
                            NroPuerta = enc_ubi_puerta.NroPuerta
                        }
                    ).ToList();

                lstDatosUbicacion_Puertas.DataSource = lista_puertas;
                lstDatosUbicacion_Puertas.DataBind();

                Panel pnlDatosUbicacion_ph = (Panel)e.Row.FindControl("pnlDatosUbicacion_ph");
                DataList lstDatosUbicacion_ph = (DataList)e.Row.FindControl("lstDatosUbicacion_ph");

                List<ENC_Ubicacion.PH> lista_ph =
                    (
                        from enc_ubi_ph in db.Encomienda_Ubicaciones_PropiedadHorizontal
                        join ubi_ph in db.Ubicaciones_PropiedadHorizontal on enc_ubi_ph.id_propiedadhorizontal equals ubi_ph.id_propiedadhorizontal
                        where enc_ubi_ph.id_encomiendaubicacion == id_encomiendaubicacion
                        select new ENC_Ubicacion.PH
                        {
                            id_propiedadhorizontal = ubi_ph.id_propiedadhorizontal,
                            NroPartidaHorizontal = (ubi_ph.NroPartidaHorizontal == null) ? 0 : (int)ubi_ph.NroPartidaHorizontal,
                            Piso = ubi_ph.Piso,
                            Depto = ubi_ph.Depto,
                            UnidadFuncional = ubi_ph.UnidadFuncional
                        }
                    ).ToList();


                lstDatosUbicacion_ph.DataSource = lista_ph;
                lstDatosUbicacion_ph.DataBind();

                pnlDatosUbicacion_ph.Visible = (lista_ph.Count > 0);
            }

        }

        private int _tramiteTarea = 0;
        public int TramiteTarea
        {
            get
            {
                if (_tramiteTarea == 0)
                {
                    int.TryParse(hid_id_tramitetarea.Value, out _tramiteTarea);
                }
                return _tramiteTarea;
            }
            set
            {
                hid_id_tramitetarea.Value = value.ToString();
                _tramiteTarea = value;
            }
        }

        private int _id_solicitud = 0;
        public int id_solicitud
        {
            get
            {
                if (_id_solicitud == 0)
                {
                    int.TryParse(hid_id_solicitud.Value, out _id_solicitud);
                }
                return _id_solicitud;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
                _id_solicitud = value;
            }
        }

        private SGI_Tarea_Validar_Zonificacion Buscar_Tarea(int id_tramitetarea)
        {
            SGI_Tarea_Validar_Zonificacion zonificar =
                (
                    from zonif in db.SGI_Tarea_Validar_Zonificacion
                    where zonif.id_tramitetarea == id_tramitetarea
                    orderby zonif.id_validar_zonificacion descending
                    select zonif
                ).ToList().FirstOrDefault();

            return zonificar;
        }

        private SGI_Tarea_Validar_Zonificacion Buscar_Tarea_Anterior(int id_tramitetarea)
        {
            SGI_Tarea_Validar_Zonificacion zonificar =
                (
                    from zonif in db.SGI_Tarea_Validar_Zonificacion
                    where zonif.id_tramitetarea < id_tramitetarea
                    orderby zonif.id_validar_zonificacion descending
                    select zonif
                ).ToList().FirstOrDefault();

            return zonificar;
        }
        #endregion


        #region acciones

        private void Redireccionar_VisorTramite()
        {
            int id_tramitetarea = (Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0); string url = Shared.getRedireccionURL(this.id_solicitud, id_tramitetarea);
            Response.Redirect(url, false);
        }

        protected void ucResultadoTarea_CerrarClick(object sender, EventArgs e)
        {
            Redireccionar_VisorTramite();
        }

        private void Validar_Tarea()
        {
        }

        private void Guardar_tarea(int id_tramite_tarea, string observacion, Guid userId)
        {

            SGI_Tarea_Validar_Zonificacion zonificar = Buscar_Tarea(id_tramite_tarea);

            int id_zonificar = 0;
            if (zonificar != null)
                id_zonificar = zonificar.id_validar_zonificacion;

            db.SGI_Tarea_Validar_Zonificacion_Actualizar(id_zonificar, id_tramite_tarea, observacion, userId);

        }

        protected void ucResultadoTarea_GuardarClick(object sender, ucResultadoTareaEventsArgs e)
        {
            try
            {

                Guid userid = Functions.GetUserId();

                this.db = new DGHP_Entities();

                Validar_Tarea();

                using (TransactionScope Tran = new TransactionScope())
                {

                    try
                    {
                        Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);

                        db.SaveChanges();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. validar_zonificacion-ucResultadoTarea_GuardarClick");
                        throw ex;
                    }

                }
                db.Dispose();

                //CargarDatosTramite(this.TramiteTarea);

                Enviar_Mensaje("Se ha guardado la tarea.", "");

                Redireccionar_VisorTramite();
            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                Enviar_Mensaje(ex.Message, "");
            }

        }

        private void Validar_Finalizar()
        {
        }

        protected void ucResultadoTarea_FinalizarTareaClick(object sender, ucResultadoTareaEventsArgs e)
        {

            // Al finalizar esta tarea lo que se hace luego de que el motor cree la tarea nueva es asignarsela
            // al mismo usuario que tuvo la misma tarea la ultima vez

            try
            {
                Guid userid = Functions.GetUserId();
                int id_tramitetarea_nuevo = 0;

                this.db = new DGHP_Entities();

                Validar_Finalizar();

                using (TransactionScope Tran = new TransactionScope())
                {

                    try
                    {
                        Guardar_tarea(this.TramiteTarea, ucObservacionesTarea.Text, userid);
                        db.SaveChanges();

                        id_tramitetarea_nuevo = ucResultadoTarea.FinalizarTarea();

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. valizar_zonificacion-ucResultadoTarea_FinalizarTareaClick");
                        throw ex;
                    }

                }
                db.Dispose();

                Enviar_Mensaje("Se ha finalizado la tarea.", "");

                Redireccionar_VisorTramite();

            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                Enviar_Mensaje(ex.Message, "");
            }

        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode("Validar zonificación");
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostratMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        #endregion


    }
}

public class ENC_Ubicacion : Encomienda_Ubicaciones
{

    public string ZonaDeclarada { get; set; }
    public string DescripcionZonaPla { get; set; }
    public Nullable<int> NroPartidaMatriz { get; set; }
    public Nullable<int> Seccion { get; set; }
    public string Manzana { get; set; }
    public string Parcela { get; set; }

    public class Puerta
    {
        public int id_encomiendapuerta { get; set; }
        public int id_encomiendaubicacion { get; set; }
        public int codigo_calle { get; set; }
        public string Nombre_calle { get; set; }
        public int NroPuerta { get; set; }

    }

    public class PH
    {
        public int id_propiedadhorizontal { get; set; }
        public int NroPartidaHorizontal { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public string UnidadFuncional { get; set; }
        public string DescripcionCompleta
        {
            get
            {
                string desc = "";
                if (!string.IsNullOrEmpty(this.Piso))
                    desc = desc + " Piso: " + this.Piso;

                if (!string.IsNullOrEmpty(this.Depto))
                    desc = desc + " Depto: " + this.Depto;


                if (!string.IsNullOrEmpty(this.UnidadFuncional))
                    desc = desc + " U.F: " + this.UnidadFuncional;

                if (!string.IsNullOrEmpty(desc))
                    desc.Substring(1); //quita el primer espacio

                return desc;
            }
            set { }

        }


    }


    public List<Puerta> GetPuertas()
    {
        DGHP_Entities db = new DGHP_Entities();
        List<Puerta> puertas =
            (
                //from enc_ubi in db.Encomienda_Ubicaciones
                //join enc_ubi_puerta in db.Encomienda_Ubicaciones_Puertas on enc_ubi.id_encomiendaubicacion equals enc_ubi_puerta.id_encomiendaubicacion
                from enc_ubi_puerta in db.Encomienda_Ubicaciones_Puertas
                where enc_ubi_puerta.id_encomiendaubicacion == this.id_encomiendaubicacion
                orderby enc_ubi_puerta.id_encomiendapuerta
                select new Puerta
                {
                    id_encomiendapuerta = enc_ubi_puerta.id_encomiendapuerta,
                    id_encomiendaubicacion = enc_ubi_puerta.id_encomiendaubicacion,
                    codigo_calle = enc_ubi_puerta.codigo_calle,
                    Nombre_calle = enc_ubi_puerta.nombre_calle,
                    NroPuerta = enc_ubi_puerta.NroPuerta
                }
            ).Distinct().ToList();

        return puertas;

    }

    public string GetUrlFoto(int ancho, int alto)
    {

        string ret = "";
        string SMP = "";
        int tamaManzana = 3;
        int tamaParcela = 3;

        string seccion = this.Seccion.ToString().Trim();
        string manzana = this.Manzana.Trim();
        string parcela = this.Parcela.Trim();


        SMP += seccion.PadLeft(2, Convert.ToChar("0"));
        SMP += "-";

        if (manzana.Length > 0)
        {
            if (!Char.IsNumber(manzana, manzana.Length - 1))
                tamaManzana = 4;
        }

        SMP += manzana.PadLeft(tamaManzana, Convert.ToChar("0"));
        SMP += "-";

        if (parcela.Length > 0)
        {
            if (!Char.IsNumber(parcela, parcela.Length - 1))
                tamaParcela = 4;
        }

        SMP += parcela.PadLeft(tamaParcela, Convert.ToChar("0"));

        ret = string.Format("http://fotos.usig.buenosaires.gob.ar/getFoto?smp={0}&i=0&h=200&w=250", SMP);

        return ret;

    }

    public string GetUrlMapa(int ancho, int alto, string descripcion)
    {


        string ret = "";
        string SMP = "";
        int tamaManzana = 3;
        int tamaParcela = 3;

        string seccion = this.Seccion.ToString().Trim();
        string manzana = this.Manzana.Trim();
        string parcela = this.Parcela.Trim();

        Puerta vPuerta = this.GetPuertas().FirstOrDefault();
        string Direccion = "";
        if (vPuerta != null)
            Direccion = vPuerta.Nombre_calle + " " + vPuerta.NroPuerta.ToString();

        SMP += seccion.PadLeft(2, Convert.ToChar("0"));
        SMP += "-";

        if (manzana.Length > 0)
        {
            if (!Char.IsNumber(manzana, manzana.Length - 1))
                tamaManzana = 4;
        }

        SMP += manzana.PadLeft(tamaManzana, Convert.ToChar("0"));
        SMP += "-";

        if (parcela.Length > 0)
        {
            if (!Char.IsNumber(parcela, parcela.Length - 1))
                tamaParcela = 4;
        }

        SMP += parcela.PadLeft(tamaParcela, Convert.ToChar("0"));

        if (!string.IsNullOrEmpty(descripcion))
            ret = string.Format("http://servicios.usig.buenosaires.gov.ar/LocDir/mapa.phtml?dir={0}&desc={2}&w=400&h=200&punto=5&r=200&smp={1}",
                   System.Web.HttpUtility.UrlEncode(Direccion), SMP, descripcion);
        else
            ret = string.Format("http://servicios.usig.buenosaires.gov.ar/LocDir/mapa.phtml?dir={0}&w=400&h=200&punto=5&r=200&smp={1}",
                   System.Web.HttpUtility.UrlEncode(Direccion), SMP);

        return ret;

    }

    public string GetUrlCroquis(int ancho, int alto)
    {

        string ret = "";
        string SMP = "";
        int tamaManzana = 3;
        int tamaParcela = 3;

        string seccion = this.Seccion.ToString().Trim();
        string manzana = this.Manzana.Trim();
        string parcela = this.Parcela.Trim();
        Puerta vPuerta = this.GetPuertas().FirstOrDefault();
        string Direccion = "";
        if (vPuerta != null)
            Direccion = vPuerta.Nombre_calle + " " + vPuerta.NroPuerta.ToString();


        SMP += seccion.PadLeft(2, Convert.ToChar("0"));
        SMP += "-";

        if (manzana.Length > 0)
        {
            if (!Char.IsNumber(manzana, manzana.Length - 1))
                tamaManzana = 4;
        }

        SMP += manzana.PadLeft(tamaManzana, Convert.ToChar("0"));
        SMP += "-";

        if (parcela.Length > 0)
        {
            if (!Char.IsNumber(parcela, parcela.Length - 1))
                tamaParcela = 4;
        }

        SMP += parcela.PadLeft(tamaParcela, Convert.ToChar("0"));

        ret = string.Format("http://servicios.usig.buenosaires.gov.ar/LocDir/mapa.phtml?dir={0}&w=400&h=300&punto=5&r=50&smp={1}",
                 HttpUtility.UrlEncode(Direccion), SMP);

        return ret;

    }

}



