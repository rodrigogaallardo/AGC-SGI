using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    public class PVH
    {
        public class SGI_EnviarPVHException : Exception
        {
            public SGI_EnviarPVHException(string mensaje) : base(mensaje, new Exception()) { }
        }


        //public int id_tramitetarea { get; set; }
        private DGHP_Entities db = null;

        private ws_solicitudes.Solicitudes ws_pvh = null;
        private ws_solicitudes.WSResultado ws_resultado = null;
        private ws_solicitudes.WSNuevaEncomiendaPVH ws_datos_solicitud = null;

        private List<ws_solicitudes.WSRubroEncomienda> list_rubro = null;
        private List<ws_solicitudes.WSActividad> list_actividad = null;
        private List<ws_solicitudes.WSPartida> list_partida = null;
        private List<ws_solicitudes.WSPlanta> list_planta = null;
        private List<ws_solicitudes.WSPersonaEncomienda> list_titulares = null;
        private List<ws_solicitudes.WSLocal> list_conf_local = null;
        private List<ws_solicitudes.WSSobrecarga> list_sobrecarga = null;
        private List<ws_solicitudes.WSFirmanteEncomienda> list_firmante = null;

        private bool datos_cargados = false;
        private int id_tramitetarea;
        private Guid userid;

        public PVH(Guid userid)
        {
            this.datos_cargados = false;
            this.userid = userid;
        }

        public void cargarDatos(int id_tramitetarea)
        {
            this.datos_cargados = false;
            this.id_tramitetarea = id_tramitetarea;

            try
            {

                this.db = new DGHP_Entities();

                ConfigurarWS();

                cargarDatosSolicitud();

                this.db.Dispose();

                this.datos_cargados = true;

            }
            catch (SGI_EnviarPVHException ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                LogError.Write(ex, "No se pudo enviar la solicitud a pvh. id_tramitetarea: " + id_tramitetarea);
                throw ex;

            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                LogError.Write(ex, "No se pudo enviar la solicitud a pvh, error interno. id_tramitetarea: " + id_tramitetarea);
                throw ex;
            }

        }

        public bool Enviar()
        {
            bool enviado = false;
            try
            {
                if (!this.datos_cargados)
                    throw new SGI_EnviarPVHException("Los datos no se cargaron correctamente.");

                string ws_usuario = PVH.getUsuario();
                string ws_clave = PVH.getClave();

                ws_resultado = this.ws_pvh.nuevaEncomiendaPVH(ws_usuario, ws_clave, this.ws_datos_solicitud);

                if (!string.IsNullOrEmpty(ws_resultado.mensaje))
                    throw new SGI_EnviarPVHException(ws_resultado.mensaje + " (Webservice Habilitaciones)");

                enviado = true;
            }
            catch (SGI_EnviarPVHException ex)
            {
                LogError.Write(ex, "No se pudo enviar la solicitud a pvh. id_tramitetarea: " + this.id_tramitetarea);
                throw ex;

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "No se pudo enviar la solicitud a pvh, error interno. id_tramitetarea: " + this.id_tramitetarea);
                throw ex;
            }

            return enviado;
        }

        public static string getClave()
        {
            string clave = (ConfigurationManager.AppSettings["Service.clave.PVH"] == null) ? "am%21ks" : Convert.ToString(ConfigurationManager.AppSettings["Service.clave.PVH"]);

            return clave;
        }

        public static string getUsuario()
        {
            string clave = (ConfigurationManager.AppSettings["Service.usuario.PVH"] == null) ? "SOLICITUDES" : Convert.ToString(ConfigurationManager.AppSettings["Service.usuario.PVH"]);

            return clave;
        }


        public static string getUrl()
        {
            string clave = Convert.ToString(ConfigurationManager.AppSettings["Service.Url.PVH"]);

            return clave;
        }
            

                //string sServer = ConfigurationManager.AppSettings["Proxy.Server"];
                //int iPort = Convert.ToInt32(ConfigurationManager.AppSettings["Proxy.Port"]);
                //string sUsername = ConfigurationManager.AppSettings["Proxy.Username"];
                //string sPass = ConfigurationManager.AppSettings["Proxy.Password"];


        private void cargarDatosSolicitud()
        {
            SGI_Tramites_Tareas_HAB tramite_tarea = Buscar_TramiteTarea(this.id_tramitetarea);

            if ( tramite_tarea == null )
            {
                throw new SGI_EnviarPVHException("El trámite no existe. id_tramitetarea: " + this.id_tramitetarea);
            }

            //if (! tramite_tarea.FechaCierre_tramitetarea.HasValue)
            //    throw new SGI_EnviarPVHException("El trámite no ha sido cerrado. id_tramitetarea: " + this.id_tramitetarea);

            if ( tramite_tarea.SGI_Tramites_Tareas.id_tarea != (int)Constants.ENG_Tareas.SSP_Enviar_PVH && 
                 tramite_tarea.SGI_Tramites_Tareas.id_tarea != (int)Constants.ENG_Tareas.SCP_Enviar_PVH  )
                throw new SGI_EnviarPVHException("El trámite no corresponde a una tarea de enviar a PHV. id_tramitetarea: " + this.id_tramitetarea);

            SSIT_Solicitudes sol = db.SSIT_Solicitudes.Where(x => x.id_solicitud == tramite_tarea.id_solicitud).FirstOrDefault();

            Encomienda enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == sol.id_solicitud && x.id_estado==(int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

            Expediente expe = null;

            string nro_expediente ="";
            try
            {
                expe = new Expediente(this.userid, Constants.ApplicationName);
                DataSet dsInfoPaquete = expe.GetDatosExpediente(tramite_tarea.id_solicitud);
                nro_expediente = expe.datos_caratula_nro_expediente;
                expe.Dispose();
            }
            catch (Exception ex)
            {
                if ( expe != null )
                    expe.Dispose();
                throw ex;
            }

            if (string.IsNullOrEmpty(nro_expediente))
                throw new Exception("No se encontro el expediente. solicitud: " + tramite_tarea.id_solicitud);

            Cargar_Encomienda_ConformacionLocal(enc.id_encomienda);

            Cargar_Encomienda_Ubicaciones(enc.id_encomienda);

            Cargar_Encomienda_Plantas(enc.id_encomienda);

            Cargar_Encomienda_Rubros(enc.id_encomienda);

            this.ws_datos_solicitud.superficieSpecified = true;
            this.ws_datos_solicitud.superficie = 0;

            Cargar_encomienda_datoslocal(enc.id_encomienda);

            Cargar_Encomienda_Titulares_PersonasJuridicas(enc.id_encomienda);

            Cargar_Encomienda_Titulares_PersonasFisicas(enc.id_encomienda);

            Cargar_Encomienda_Firmantes_PersonasJuridicas(enc.id_encomienda);

            Encomienda_Firmantes_PersonasFisicas(enc.id_encomienda);

            Cargar_Profesional(enc.id_encomienda);

            Cargar_Escribano(sol.id_solicitud);

            Cargar_TipoActividad(enc.id_encomienda);

            Cargar_Normativa(enc.id_encomienda);

            Cargar_TipoTramite(sol.id_solicitud);

            if (this.list_rubro.Count > 0)
                this.ws_datos_solicitud.rubros = this.list_rubro.ToArray();

            if (this.list_actividad.Count > 0)
                this.ws_datos_solicitud.actividades = this.list_actividad.ToArray();

            if (this.list_partida.Count > 0)
                this.ws_datos_solicitud.partidas = this.list_partida.ToArray();

            if (this.list_conf_local.Count > 0)
                this.ws_datos_solicitud.conformacionLocal = this.list_conf_local.ToArray();

            if (this.list_planta.Count > 0)
                this.ws_datos_solicitud.plantas = this.list_planta.ToArray();

            if (this.list_titulares.Count > 0)
                this.ws_datos_solicitud.titulares = this.list_titulares.ToArray();

            if (this.list_firmante.Count > 0)
                this.ws_datos_solicitud.firmantes = this.list_firmante.ToArray();

            if (this.list_sobrecarga.Count > 0)
                this.ws_datos_solicitud.sobrecargas = this.list_sobrecarga.ToArray();

            this.ws_datos_solicitud.fechaSolicitud = sol.CreateDate;
            this.ws_datos_solicitud.fechaSolicitudSpecified = true;
          
            this.ws_datos_solicitud.codigoZonaDeclarada = enc.ZonaDeclarada;

            Zonas_Planeamiento zona_plan = db.Zonas_Planeamiento.Where(x => x.CodZonaPla == enc.ZonaDeclarada).FirstOrDefault();

            if (zona_plan != null)
            {
                this.ws_datos_solicitud.descripcionZonaDeclarada = zona_plan.DescripcionZonaPla;
            }

            //Debe enviarse la Fecha de Habilitacion 
            this.ws_datos_solicitud.fechaHabilitacio = tramite_tarea.SGI_Tramites_Tareas.FechaInicio_tramitetarea;
            this.ws_datos_solicitud.fechaHabilitacioSpecified = true;

            this.ws_datos_solicitud.nroSolicitud = sol.id_solicitud;
            this.ws_datos_solicitud.nroSolicitudSpecified = true;

            this.ws_datos_solicitud.observacionPlantas = enc.Observaciones_plantas;
            this.ws_datos_solicitud.observacionRubros = enc.Observaciones_rubros;

            int[] tareas_caificar = new int[2] { (int)Constants.ENG_Tareas.SSP_Calificar, (int)Constants.ENG_Tareas.SCP_Calificar };
            List<TramiteTareaAnteriores> list_tramite_anteriores = TramiteTareaAnteriores.BuscarUltimoTramiteTareaPorTarea((int)Constants.GruposDeTramite.HAB, sol.id_solicitud, id_tramitetarea, tareas_caificar);

            foreach (TramiteTareaAnteriores item in list_tramite_anteriores)
            {
                SGI_Tramites_Tareas tt_calificador = db.SGI_Tramites_Tareas.Where(x => x.id_tramitetarea == item.id_tramitetarea).FirstOrDefault();
                this.ws_datos_solicitud.usernameCalificador = tt_calificador.aspnet_Users.UserName.Trim();
            }

            if ( string.IsNullOrEmpty(this.ws_datos_solicitud.usernameCalificador) )
                throw new SGI_EnviarPVHException("No se ha encontrado al calificador del trámite. id_tramitetarea: " + id_tramitetarea);



            this.ws_datos_solicitud.nroExpediente = nro_expediente;


            //this.solicitud.usernameCalificador = CalificadorAutorizante(id_solicitud);


            // Expedientes Relacionados
            //CargarExpedientesRelacionados(id_solicitud, ref vExpRelacionados);

            //if (vExpRelacionados.Count > 0)
            //    vSolicitudExt.expedientesRelacionados = vExpRelacionados.ToArray();
            // ----



        }

        private SGI_Tramites_Tareas_HAB Buscar_TramiteTarea(int id_tramitetarea)
        {
            SGI_Tramites_Tareas_HAB tramite_tarea = db.SGI_Tramites_Tareas_HAB.Where(x => x.id_tramitetarea == id_tramitetarea).FirstOrDefault();

            return tramite_tarea;
        }

        private void ConfigurarWS()
        {

            ws_pvh = new ws_solicitudes.Solicitudes();

            try
            {
                ws_pvh.Url = PVH.getUrl();

                string sServer = ConfigurationManager.AppSettings["Proxy.Server"];
                int iPort = Convert.ToInt32(ConfigurationManager.AppSettings["Proxy.Port"]);
                string sUsername = ConfigurationManager.AppSettings["Proxy.Username"];
                string sPass = ConfigurationManager.AppSettings["Proxy.Password"];

                System.Net.WebProxy wp = new System.Net.WebProxy(sServer, iPort);
                wp.Credentials = new System.Net.NetworkCredential(sUsername, sPass);
                ws_pvh.Proxy = wp;

            }
            catch
            {
                ws_pvh.Proxy = null;
            }
            ws_pvh.Timeout = 180000;       // 3 minutos


            this.ws_datos_solicitud = new ws_solicitudes.WSNuevaEncomiendaPVH();


        }

        private void Cargar_Encomienda_ConformacionLocal(int id_encomienda)
        {
            List<Encomienda_ConformacionLocal> list_local = db.Encomienda_ConformacionLocal.Where(x => x.id_encomienda == id_encomienda).ToList();

            ws_solicitudes.WSLocal ws_local;

            this.list_conf_local = new List<ws_solicitudes.WSLocal>();

            foreach (Encomienda_ConformacionLocal item in list_local)
            {
                ws_local = new ws_solicitudes.WSLocal();

                ws_local.codDestino = item.id_destino.ToString();

                if (item.alto_conflocal.HasValue)
                {
                    ws_local.alto = Convert.ToDouble(item.alto_conflocal);
                    ws_local.altoSpecified = true;
                }

                if (item.ancho_conflocal.HasValue)
                {
                    ws_local.ancho = Convert.ToDouble(item.ancho_conflocal);
                    ws_local.anchoSpecified = true;
                }

                if (item.largo_conflocal.HasValue)
                {
                    ws_local.largo = Convert.ToDouble(item.largo_conflocal);
                    ws_local.largoSpecified = true;
                }

                ws_local.frisos = item.Frisos_conflocal;
                ws_local.paredes = item.Paredes_conflocal;
                ws_local.pisos = item.Pisos_conflocal;
                ws_local.techos = item.Techos_conflocal;
                ws_local.observaciones = item.Observaciones_conflocal;

                this.list_conf_local.Add(ws_local);
            }

        }

        private void Cargar_Encomienda_Ubicaciones(int id_encomienda)
        {

            var list_ubi_pm =
                    (
                        from encubic in db.Encomienda_Ubicaciones
                        join ubic in db.Ubicaciones on encubic.id_ubicacion equals ubic.id_ubicacion
                        join zonpla in db.Zonas_Planeamiento on encubic.id_zonaplaneamiento equals zonpla.id_zonaplaneamiento
                        where encubic.id_encomienda == id_encomienda
                        select new
                        {
                            ubic.id_ubicacion,
                            ubic.NroPartidaMatriz,
                            ubic.Seccion,
                            ubic.Manzana,
                            ubic.Parcela,
                            zonpla.CodZonaPla,
                            zonpla.DescripcionZonaPla,
                            encubic.deptoLocal_encomiendaubicacion
                        }
                    ).ToList();

            //int index_pm = 0;
            int index_ph = 0;
            int index_puerta = 0;

            ws_solicitudes.WSPartida ws_partida;
            ws_solicitudes.WSPartidaHorizontal ws_ph;
            ws_solicitudes.WSExpedientePartidaPuerta ws_puerta;

            this.list_partida = new List<ws_solicitudes.WSPartida>();

            foreach (var item_pm in list_ubi_pm)
            {

                //partida matriz
                #region cargar datos partida matriz

                ws_partida = new ws_solicitudes.WSPartida();

                if (item_pm.NroPartidaMatriz.HasValue && item_pm.NroPartidaMatriz > 0)
                {
                    ws_partida.numeroPartida = item_pm.NroPartidaMatriz;
                    ws_partida.numeroPartidaSpecified = true;
                }

                if (item_pm.Seccion.HasValue && item_pm.Seccion > 0)
                {
                    ws_partida.seccion = item_pm.Seccion;
                    ws_partida.seccionSpecified = true;
                }

                ws_partida.manzana = item_pm.Manzana;
                ws_partida.parcela = item_pm.Parcela;
                ws_partida.codigoZona = item_pm.CodZonaPla;
                ws_partida.descripcionZona = item_pm.DescripcionZonaPla;
                ws_partida.deptoLocal = item_pm.deptoLocal_encomiendaubicacion;

                #endregion

                // Partidas Horizontales
                #region cargar datos partida horizontal


                var list_ubi_ph =
                    (
                        from encubic in db.Encomienda_Ubicaciones
                        join encphor in db.Encomienda_Ubicaciones_PropiedadHorizontal on encubic.id_encomiendaubicacion equals encphor.id_encomiendaubicacion
                        join phor in db.Ubicaciones_PropiedadHorizontal on encphor.id_propiedadhorizontal equals phor.id_propiedadhorizontal
                        where encubic.id_encomienda == id_encomienda && encubic.id_ubicacion == item_pm.id_ubicacion
                        select new
                        {
                            phor.NroPartidaHorizontal,
                            phor.Piso,
                            phor.Depto,
                            phor.UnidadFuncional
                        }
                    ).ToList();

                index_ph = 0;
                ws_partida.partidasHorizontales = null;
                if (ws_partida.partidasHorizontales == null)
                    ws_partida.partidasHorizontales = new ws_solicitudes.WSPartidaHorizontal[list_ubi_ph.Count];

                foreach (var item_ph in list_ubi_ph)
                {


                    ws_ph = new ws_solicitudes.WSPartidaHorizontal();

                    if (item_ph.NroPartidaHorizontal.HasValue)
                    {
                        ws_ph.numeroPartida = item_ph.NroPartidaHorizontal;
                        ws_ph.numeroPartidaSpecified = true;
                    }

                    ws_ph.piso = item_ph.Piso;
                    ws_ph.depto = "";  //No pasa porque es UF no depto
                    ws_ph.unidadFuncional = item_ph.UnidadFuncional;

                    ws_partida.partidasHorizontales[index_ph] = ws_ph;
                    index_ph++;
                }

                #endregion

                // Puertas
                #region cargar datos puertas

                var list_ubi_puerta =
                    (
                        from encubic in db.Encomienda_Ubicaciones
                        join encpuer in db.Encomienda_Ubicaciones_Puertas on encubic.id_encomiendaubicacion equals encpuer.id_encomiendaubicacion
                        where encubic.id_encomienda == id_encomienda && encubic.id_ubicacion == item_pm.id_ubicacion
                        select new
                        {
                            encpuer.codigo_calle,
                            encpuer.NroPuerta,
                            encpuer.nombre_calle
                        }
                    ).ToList();

                index_puerta = 0;
                ws_partida.puertas = null;
                if (ws_partida.puertas == null)
                    ws_partida.puertas = new ws_solicitudes.WSExpedientePartidaPuerta[list_ubi_puerta.Count];

                foreach (var puerta in list_ubi_puerta)
                {
                    ws_puerta = new ws_solicitudes.WSExpedientePartidaPuerta();

                    ws_puerta.codigoCalle = puerta.codigo_calle;
                    ws_puerta.codigoCalleSpecified = true;
                    ws_puerta.nombreCalle = puerta.nombre_calle;
                    ws_puerta.numeroPuerta = puerta.NroPuerta;
                    ws_puerta.numeroPuertaSpecified = true;

                    ws_partida.puertas[index_puerta] = ws_puerta;
                    index_puerta++;
                }

                #endregion

                this.list_partida.Add(ws_partida);

            }


        }

        private void Cargar_Encomienda_Plantas(int id_encomienda)
        {
            var list_planta =
                (
                    from encubic in db.Encomienda_Plantas
                    where encubic.id_encomienda == id_encomienda
                    select new
                    {
                        encubic.id_tiposector,
                        encubic.detalle_encomiendatiposector
                    }
                ).Distinct().ToList();

            ws_solicitudes.WSPlanta ws_planta = null;

            this.list_planta = new List<ws_solicitudes.WSPlanta>();

            foreach (var item in list_planta)
            {
                ws_planta = new ws_solicitudes.WSPlanta();

                ws_planta.detalle = item.detalle_encomiendatiposector;
                ws_planta.idTipoSector = item.id_tiposector;
                ws_planta.idTipoSectorSpecified = true;

                this.list_planta.Add(ws_planta);
            }

        }

        private void Cargar_Encomienda_Rubros(int id_encomienda)
        {
            var list_rubro =
                (
                    from encrub in db.Encomienda_Rubros
                    join ia in db.ImpactoAmbiental on encrub.id_ImpactoAmbiental equals ia.id_ImpactoAmbiental
                    where encrub.id_encomienda == id_encomienda
                    select new
                    {
                        encrub.id_encomienda,
                        encrub.cod_rubro,
                        encrub.desc_rubro,
                        encrub.EsAnterior,
                        encrub.id_tipoactividad,
                        encrub.id_tipodocreq,
                        encrub.SuperficieHabilitar,
                        encrub.id_ImpactoAmbiental,
                        ia.cod_ImpactoAmbiental,
                        ia.nom_ImpactoAmbiental
                    }
                ).ToList();

            ws_solicitudes.WSRubroEncomienda ws_rubro = null;

            this.list_rubro = new List<ws_solicitudes.WSRubroEncomienda>();

            foreach (var item in list_rubro)
            {

                ws_rubro = new ws_solicitudes.WSRubroEncomienda();

                ws_rubro.codigoRubro = item.cod_rubro;
                ws_rubro.descripcionRubro = item.desc_rubro;
                ws_rubro.esAnterior = item.EsAnterior;
                ws_rubro.esAnteriorSpecified = true;
                ws_rubro.idTipoActividad = item.id_tipoactividad;
                ws_rubro.idTipoActividadSpecified = true;
                ws_rubro.idTipoDocReq = item.id_tipodocreq;
                ws_rubro.idTipoDocReqSpecified = true;
                ws_rubro.impactoAmbiental = item.cod_ImpactoAmbiental;

                if (item.SuperficieHabilitar != null)
                {
                    ws_rubro.superficie = Convert.ToDouble(item.SuperficieHabilitar);
                    ws_rubro.superficieSpecified = true;
                }

                this.list_rubro.Add(ws_rubro);
            }

        }

        private void Cargar_encomienda_datoslocal(int id_encomienda)
        {
            Encomienda_DatosLocal list_datos = db.Encomienda_DatosLocal.Where(x => x.id_encomienda == id_encomienda).FirstOrDefault();

            this.ws_datos_solicitud.datosLocal = new ws_solicitudes.WSDatosLocal();

            //double SuperficieTotal = 0;

            int cantidadSanitarios = 0;
            double dimensionFrente = 0;
            double fondo = 0;
            double frente = 0;
            double lateralDerecho = 0;
            double lateralIzquierdo = 0;
            double sanitariosDistancia = 0;
            int sanitariosUbicacion = 0;
            int sobrecargaRequisitosOpcion = 0;
            double superficieCubierta = 0;
            double superficieDescubierta = 0;
            double superficieSanitarios = 0;
            int cantidadOperarios = 0;

            int.TryParse(list_datos.cantidad_sanitarios_dl.ToString(), out cantidadSanitarios);
            double.TryParse(list_datos.dimesion_frente_dl.ToString(), out dimensionFrente);
            double.TryParse(list_datos.fondo_dl.ToString(), out fondo);
            double.TryParse(list_datos.frente_dl.ToString(), out frente);
            double.TryParse(list_datos.lateral_derecho_dl.ToString(), out lateralDerecho);
            double.TryParse(list_datos.lateral_izquierdo_dl.ToString(), out lateralIzquierdo);
            double.TryParse(list_datos.sanitarios_distancia_dl.ToString(), out sanitariosDistancia);
            int.TryParse(list_datos.sanitarios_ubicacion_dl.ToString(), out sanitariosUbicacion);
            int.TryParse(list_datos.sobrecarga_requisitos_opcion.ToString(), out sobrecargaRequisitosOpcion);
            double.TryParse(list_datos.superficie_cubierta_dl.ToString(), out superficieCubierta);
            double.TryParse(list_datos.superficie_descubierta_dl.ToString(), out superficieDescubierta);
            double.TryParse(list_datos.superficie_sanitarios_dl.ToString(), out superficieSanitarios);
            //SuperficieTotal = superficieCubierta + superficieDescubierta;

            int.TryParse(list_datos.cantidad_operarios_dl.ToString(), out cantidadOperarios);

            this.ws_datos_solicitud.cantOperarios = cantidadOperarios;
            this.ws_datos_solicitud.cantOperariosSpecified = true;

            this.ws_datos_solicitud.datosLocal.cantidadSanitarios = cantidadSanitarios;
            this.ws_datos_solicitud.datosLocal.cantidadSanitariosSpecified = true;
            this.ws_datos_solicitud.datosLocal.croquisUbicacion = list_datos.croquis_ubicacion_dl;
            this.ws_datos_solicitud.datosLocal.dimensionFrente = dimensionFrente;
            this.ws_datos_solicitud.datosLocal.dimensionFrenteSpecified = true;
            this.ws_datos_solicitud.datosLocal.estacionamiento = list_datos.estacionamiento_dl;
            this.ws_datos_solicitud.datosLocal.estacionamientoSpecified = true;
            this.ws_datos_solicitud.datosLocal.fondo = fondo;
            this.ws_datos_solicitud.datosLocal.fondoSpecified = true;
            this.ws_datos_solicitud.datosLocal.frente = dimensionFrente;
            this.ws_datos_solicitud.datosLocal.frenteSpecified = true;
            this.ws_datos_solicitud.datosLocal.lateralDerecho = lateralDerecho;
            this.ws_datos_solicitud.datosLocal.lateralDerechoSpecified = true;
            this.ws_datos_solicitud.datosLocal.lateralIzquierdo = lateralIzquierdo;
            this.ws_datos_solicitud.datosLocal.lateralIzquierdoSpecified = true;
            this.ws_datos_solicitud.datosLocal.lugarCargaDescarga = list_datos.lugar_carga_descarga_dl;
            this.ws_datos_solicitud.datosLocal.lugarCargaDescargaSpecified = true;
            this.ws_datos_solicitud.datosLocal.materialesParedes = list_datos.materiales_paredes_dl;
            this.ws_datos_solicitud.datosLocal.materialesPisos = list_datos.materiales_pisos_dl;
            this.ws_datos_solicitud.datosLocal.materialesRevestimientos = list_datos.materiales_revestimientos_dl;
            this.ws_datos_solicitud.datosLocal.materialesTechos = list_datos.materiales_techos_dl;
            this.ws_datos_solicitud.datosLocal.redTransitoPesado = list_datos.red_transito_pesado_dl;
            this.ws_datos_solicitud.datosLocal.redTransitoPesadoSpecified = true;
            this.ws_datos_solicitud.datosLocal.sanitariosDistancia = sanitariosDistancia;
            this.ws_datos_solicitud.datosLocal.sanitariosDistanciaSpecified = true;
            this.ws_datos_solicitud.datosLocal.sanitariosUbicacion = sanitariosUbicacion;
            this.ws_datos_solicitud.datosLocal.sanitariosUbicacionSpecified = true;
            this.ws_datos_solicitud.datosLocal.sobreAvenida = list_datos.sobre_avenida_dl;
            this.ws_datos_solicitud.datosLocal.sobreAvenidaSpecified = true;
            this.ws_datos_solicitud.datosLocal.sobrecargaArt813Inciso = list_datos.sobrecarga_art813_inciso;
            this.ws_datos_solicitud.datosLocal.sobrecargaArt813Item = list_datos.sobrecarga_art813_item;
            this.ws_datos_solicitud.datosLocal.sobrecargaCorresponde = list_datos.sobrecarga_corresponde_dl;
            this.ws_datos_solicitud.datosLocal.sobrecargaCorrespondeSpecified = true;
            this.ws_datos_solicitud.datosLocal.sobrecargaRequisitosOpcion = sobrecargaRequisitosOpcion;
            this.ws_datos_solicitud.datosLocal.sobrecargaRequisitosOpcionSpecified = true;
            this.ws_datos_solicitud.datosLocal.sobrecargaTipoObservacion = Convert.ToString(list_datos.sobrecarga_tipo_observacion);
            this.ws_datos_solicitud.datosLocal.superficieCubierta = superficieCubierta;
            this.ws_datos_solicitud.datosLocal.superficieCubiertaSpecified = true;
            this.ws_datos_solicitud.datosLocal.superficieDescubierta = superficieDescubierta;
            this.ws_datos_solicitud.datosLocal.superficieDescubiertaSpecified = true;
            this.ws_datos_solicitud.datosLocal.superficieSanitarios = superficieSanitarios;
            this.ws_datos_solicitud.datosLocal.superficieSanitariosSpecified = true;

            // Sobrecargas
            List<Encomienda_Sobrecargas> list_sobreCarga = db.Encomienda_Sobrecargas.Where(x => x.id_encomienda == id_encomienda).ToList();

            ws_solicitudes.WSSobrecarga ws_sobrecarga = null;

            this.list_sobrecarga = new List<ws_solicitudes.WSSobrecarga>();

            foreach (Encomienda_Sobrecargas item in list_sobreCarga)
            {
                ws_sobrecarga = new ws_solicitudes.WSSobrecarga();

                ws_sobrecarga.pesoSobrecarga = item.peso_sobrecarga;
                ws_sobrecarga.pesoSobrecargaSpecified = true;
                ws_sobrecarga.estructuraSobrecarga = item.estructura_sobrecarga;

                this.list_sobrecarga.Add(ws_sobrecarga);
            }

            this.ws_datos_solicitud.superficie = superficieCubierta + superficieDescubierta;
        }

        private void Cargar_Encomienda_Titulares_PersonasJuridicas(int id_encomienda)
        {
            var list_pj =
                (
                    from pj in db.Encomienda_Titulares_PersonasJuridicas
                    join tipiibb in db.TiposDeIngresosBrutos on pj.id_tipoiibb equals tipiibb.id_tipoiibb
                    where pj.id_encomienda == id_encomienda
                    select new
                    {
                        pj.id_personajuridica,
                        pj.Id_TipoSociedad,
                        pj.Razon_Social,
                        pj.CUIT,
                        pj.id_tipoiibb,
                        tipiibb.cod_tipoibb,
                        pj.Nro_IIBB,
                        pj.Calle,
                        pj.NroPuerta,
                        pj.Piso,
                        pj.Depto,
                        pj.id_localidad,
                        pj.Codigo_Postal,
                        pj.Telefono,
                        pj.Email
                    }
                ).ToList();

            ws_solicitudes.WSPersonaEncomienda ws_pers = null;

            if (this.list_titulares == null)
                this.list_titulares = new List<ws_solicitudes.WSPersonaEncomienda>();

            foreach (var item in list_pj)
            {

                ws_pers = new ws_solicitudes.WSPersonaEncomienda();

                ws_pers.personeria = "PERSONA_JURIDICA";

                ws_pers.idTipoSociedad = item.Id_TipoSociedad;
                ws_pers.idTipoSociedadSpecified = true;

                ws_pers.apellido = "";
                ws_pers.calle = item.Calle;
                ws_pers.cuit = item.CUIT;
                ws_pers.email = item.Email;
                ws_pers.codTipoIngresosBrutos = item.cod_tipoibb;
                ws_pers.ingresosBrutos = item.Nro_IIBB;

                ws_pers.nombre = item.Razon_Social;

                ws_pers.nroDocumento = null;
                ws_pers.piso = item.Piso;
                ws_pers.unidadFuncional = item.Depto;


                if (item.NroPuerta.HasValue)
                {
                    ws_pers.puerta = Convert.ToInt32(item.NroPuerta);
                    ws_pers.puertaSpecified = true;
                }

                ws_pers.telefono = item.Telefono;
                ws_pers.tipoDocumento = "SE";

                if (item.id_localidad != null)
                {
                    Localidad loc = db.Localidad.Where(x => x.Id == item.id_localidad && x.Excluir == false).FirstOrDefault();

                    if (loc != null)
                    {
                        ws_pers.localidad = loc.Depto;
                        ws_pers.idProvinciaSpecified = true;
                        ws_pers.idProvincia = Convert.ToInt32(loc.IdProvincia);
                    }
                }

                list_titulares.Add(ws_pers);
            }

        }

        private void Cargar_Encomienda_Titulares_PersonasFisicas(int id_encomienda)
        {
            var list_pf =
                (
                    from pf in db.Encomienda_Titulares_PersonasFisicas
                    join tipiibb in db.TiposDeIngresosBrutos on pf.id_tipoiibb equals tipiibb.id_tipoiibb
                    join tdoc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pf.id_encomienda == id_encomienda
                    select new
                    {
                        pf.id_personafisica,
                        pf.Apellido,
                        pf.Nombres,
                        pf.id_tipodoc_personal,
                        TipoDocumento = tdoc.Nombre,
                        pf.Nro_Documento,
                        pf.Cuit,
                        tipiibb.cod_tipoibb,
                        pf.Ingresos_Brutos,
                        pf.Calle,
                        pf.Nro_Puerta,
                        pf.Piso,
                        pf.Depto,
                        pf.Id_Localidad,
                        pf.Codigo_Postal,
                        pf.Telefono,
                        pf.TelefonoMovil,
                        pf.Sms,
                        pf.Email
                    }
                ).ToList();

            ws_solicitudes.WSPersonaEncomienda ws_pers = null;

            if (this.list_titulares == null)
                this.list_titulares = new List<ws_solicitudes.WSPersonaEncomienda>();

            foreach (var item in list_pf)
            {
                ws_pers = new ws_solicitudes.WSPersonaEncomienda();

                ws_pers.personeria = "PERSONA_FISICA";
                ws_pers.apellido = item.Apellido;
                ws_pers.nombre = item.Nombres;
                ws_pers.tipoDocumento = item.TipoDocumento;
                ws_pers.nroDocumento = item.Nro_Documento.ToString();
                ws_pers.cuit = item.Cuit;
                ws_pers.codTipoIngresosBrutos = item.cod_tipoibb;
                ws_pers.ingresosBrutos = item.Ingresos_Brutos;

                ws_pers.calle = item.Calle;
                ws_pers.piso = item.Piso;
                ws_pers.unidadFuncional = item.Depto;
                ws_pers.email = item.Email;

                if (item.Nro_Puerta != null)
                {
                    ws_pers.puerta = item.Nro_Puerta;
                    ws_pers.puertaSpecified = true;
                }


                ws_pers.telefono = item.Telefono;

                if (item.Id_Localidad != null)
                {
                    Localidad loc = db.Localidad.Where(x => x.Id == item.Id_Localidad && x.Excluir == false).FirstOrDefault();
                    if (loc != null)
                    {
                        ws_pers.localidad = loc.Depto;

                        ws_pers.idProvinciaSpecified = true;
                        ws_pers.idProvincia = Convert.ToInt32(loc.IdProvincia);
                    }
                }


                this.list_titulares.Add(ws_pers);
            }

        }

        private void Cargar_Encomienda_Firmantes_PersonasJuridicas(int id_encomienda)
        {
            var list_pj =
                (
                    from pj in db.Encomienda_Firmantes_PersonasJuridicas
                    join tit in db.Encomienda_Titulares_PersonasJuridicas on pj.id_personajuridica equals tit.id_personajuridica
                    join tc in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tc.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pj.id_encomienda == id_encomienda
                    select new
                    {
                        pj.id_personajuridica,
                        pj.Apellido,
                        pj.Nombres,
                        pj.id_tipodoc_personal,
                        TipoDocumento = tdoc.Nombre,
                        pj.Nro_Documento,
                        tc.cod_tipocaracter,
                        firmanteDeCuit = tit.CUIT

                    }
                ).ToList();

            ws_solicitudes.WSFirmanteEncomienda ws_firm = null;

            if (this.list_firmante == null)
                this.list_firmante = new List<ws_solicitudes.WSFirmanteEncomienda>();

            foreach (var item in list_pj)
            {

                ws_firm = new ws_solicitudes.WSFirmanteEncomienda();

                ws_firm.apellido = item.Apellido;
                ws_firm.nombre = item.Nombres;
                ws_firm.tipoDocumento = item.TipoDocumento;
                ws_firm.nroDocumento = item.Nro_Documento.ToString();
                ws_firm.codTipoCaracterLegal = item.cod_tipocaracter;
                ws_firm.firmanteDePersoneria = "PERSONA_JURIDICA";
                ws_firm.firmanteDeCuit = item.firmanteDeCuit;
                this.list_firmante.Add(ws_firm);
            }

        }

        private void Encomienda_Firmantes_PersonasFisicas(int id_encomienda)
        {
            var list_pj =
                (
                    from pf in db.Encomienda_Firmantes_PersonasFisicas
                    join tit in db.Encomienda_Titulares_PersonasFisicas on pf.id_personafisica equals tit.id_personafisica
                    join tc in db.TiposDeCaracterLegal on pf.id_tipocaracter equals tc.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pf.id_encomienda == id_encomienda
                    select new
                    {
                        pf.id_personafisica,
                        pf.Apellido,
                        pf.Nombres,
                        pf.id_tipodoc_personal,
                        TipoDocumento = tdoc.Nombre,
                        pf.Nro_Documento,
                        tc.cod_tipocaracter,
                        firmanteDeCuit = tit.Cuit
                    }
                ).ToList();


            ws_solicitudes.WSFirmanteEncomienda ws_firm = null;

            if (this.list_firmante == null)
                this.list_firmante = new List<ws_solicitudes.WSFirmanteEncomienda>();

            foreach (var item in list_pj)
            {
                ws_firm = new ws_solicitudes.WSFirmanteEncomienda();

                ws_firm.apellido = item.Apellido;
                ws_firm.nombre = item.Nombres;
                ws_firm.tipoDocumento = item.TipoDocumento.ToString();
                ws_firm.nroDocumento = item.Nro_Documento.ToString();
                ws_firm.codTipoCaracterLegal = item.cod_tipocaracter;
                ws_firm.firmanteDePersoneria = "PERSONA_FISICA";
                ws_firm.firmanteDeCuit = item.firmanteDeCuit;

                this.list_firmante.Add(ws_firm);
            }

        }

        private void Cargar_Profesional(int id_encomienda)
        {

            var list_prof =
                (
                    from enc in db.Encomienda
                    join prof in db.Profesional on enc.id_profesional equals prof.Id
                    join tdoc in db.TipoDocumentoPersonal on prof.IdTipoDocumento equals tdoc.TipoDocumentoPersonalId
                    where enc.id_encomienda == id_encomienda
                    select new
                    {
                        id_profesional = prof.Id,
                        id_consejo = prof.IdConsejo,
                        prof.Matricula,
                        prof.Apellido,
                        prof.Nombre,
                        prof.IdTipoDocumento,
                        TipoDocNombre = tdoc.Nombre,
                        prof.NroDocumento,
                        prof.Calle,
                        prof.NroPuerta,
                        prof.Piso,
                        prof.Depto,
                        prof.UnidadFuncional,
                        prof.Localidad,
                        prof.Provincia,
                        prof.Email,
                        prof.Sms,
                        prof.Telefono,
                        prof.Cuit,
                        prof.IngresosBrutos,
                        prof.Inhibido
                    }
                ).ToList();

            this.ws_datos_solicitud.profesional = new ws_solicitudes.WSPersonaEncomienda();

            foreach (var item in list_prof)
            {

                ConsejoProfesional consejo = db.ConsejoProfesional.Where(x => x.Id == item.id_consejo).FirstOrDefault();

                this.ws_datos_solicitud.profesional.consejo = consejo.Nombre;

                if (consejo == null)
                    throw new SGI_EnviarPVHException("El Profesional ingresado en la solicitud no pertenece a ningún Consejo (Solicitudes).");

                this.ws_datos_solicitud.profesional.matricula = item.Matricula.ToString();
                this.ws_datos_solicitud.profesional.apellido = item.Apellido;
                this.ws_datos_solicitud.profesional.nombre = item.Nombre;
                this.ws_datos_solicitud.profesional.calle = item.Calle;
                this.ws_datos_solicitud.profesional.cuit = item.Cuit;
                this.ws_datos_solicitud.profesional.email = item.Email;

                this.ws_datos_solicitud.profesional.idProvinciaSpecified = true;
                this.ws_datos_solicitud.profesional.idProvincia = 2;

                Provincia prov = db.Provincia.Where(x => x.Nombre == item.Provincia.Trim()).FirstOrDefault();

                if (prov != null)
                    this.ws_datos_solicitud.profesional.idProvincia = prov.Id;

                this.ws_datos_solicitud.profesional.ingresosBrutos = item.IngresosBrutos.ToString();
                this.ws_datos_solicitud.profesional.localidad = item.Localidad;
                this.ws_datos_solicitud.profesional.nroDocumento = item.NroDocumento.ToString();
                this.ws_datos_solicitud.profesional.piso = item.Piso;

                if (item.NroPuerta != null)
                {
                    int nroPuerta = 0;
                    int.TryParse(item.NroPuerta.ToString(), out nroPuerta);
                    this.ws_datos_solicitud.profesional.puerta = nroPuerta;
                    this.ws_datos_solicitud.profesional.puertaSpecified = true;
                }

                this.ws_datos_solicitud.profesional.telefono = item.Telefono;


                this.ws_datos_solicitud.profesional.tipoDocumento = item.TipoDocNombre;

                this.ws_datos_solicitud.profesional.unidadFuncional = item.UnidadFuncional;

            }

        }

        private void Cargar_Escribano(int id_solicitud)
        {

            var list_esc =
                (
                    from sol in db.SSIT_Solicitudes
                    join esc in db.Escribano on sol.MatriculaEscribano equals esc.Matricula
                    where sol.id_solicitud == id_solicitud
                    select new
                    {
                        esc.Matricula,
                        esc.Registro,
                        esc.ApyNom,
                        esc.Cargo,
                        esc.Calle,
                        esc.NroPuerta,
                        esc.Piso,
                        esc.Depto,
                        esc.CodPostal,
                        esc.Localidad,
                        esc.Telefono,
                        esc.Email,
                        esc.Inhibido
                    }
                ).ToList();

            foreach (var item in list_esc)
            {

                if (this.ws_datos_solicitud.escribano == null)
                    this.ws_datos_solicitud.escribano = new ws_solicitudes.WSPersonaEncomienda();

                string Apellido = item.ApyNom.Trim();
                //string Nombre = "";
                string strNroPuerta = item.NroPuerta.Trim();
                int NroPuerta = 0;

                if (strNroPuerta.IndexOf("/") > -1)
                    strNroPuerta = strNroPuerta.Substring(0, strNroPuerta.IndexOf("/") - 1);


                this.ws_datos_solicitud.escribano.matricula = item.Matricula.ToString();
                this.ws_datos_solicitud.escribano.registro = item.Registro.ToString();
                this.ws_datos_solicitud.escribano.apellido = Apellido;
                this.ws_datos_solicitud.escribano.nombre = "";
                this.ws_datos_solicitud.escribano.localidad = "-";         // No se Aclara para Habilitaciones

                this.ws_datos_solicitud.escribano.idProvincia = 2;       // Capital Federal
                this.ws_datos_solicitud.escribano.idProvinciaSpecified = true;

                this.ws_datos_solicitud.escribano.calle = item.Calle.ToString().Trim();
                if (int.TryParse(strNroPuerta, out  NroPuerta))
                {
                    this.ws_datos_solicitud.escribano.puerta = NroPuerta;
                    this.ws_datos_solicitud.escribano.puertaSpecified = true;
                }

                this.ws_datos_solicitud.escribano.piso = item.Piso.ToString().Trim();
                this.ws_datos_solicitud.escribano.unidadFuncional = item.Depto.ToString().Trim();
                this.ws_datos_solicitud.escribano.telefono = item.Telefono.ToString().Trim();
                this.ws_datos_solicitud.escribano.email = item.Email.ToString().Trim();

                this.ws_datos_solicitud.escribano.tipoDocumento = "SE";
                this.ws_datos_solicitud.escribano.nroDocumento = "";
                this.ws_datos_solicitud.escribano.cuit = "0";

                if (item.Inhibido.ToString().ToLower() == "no")
                    this.ws_datos_solicitud.escribano.inhibido = false;
                else
                    this.ws_datos_solicitud.escribano.inhibido = true;

                this.ws_datos_solicitud.escribano.inhibidoSpecified = true;

            }

        }

        private void Cargar_TipoActividad(int id_encomienda)
        {

            var tipo_act =
                (
                    from rub in db.Encomienda_Rubros
                    where rub.id_encomienda == id_encomienda
                    select new
                    {
                        rub.id_tipoactividad
                    }
                ).Distinct().ToList();

            ws_solicitudes.WSActividad ws_act = null;
            list_actividad = new List<ws_solicitudes.WSActividad>();

            foreach (var item in tipo_act)
            {
                ws_act = new ws_solicitudes.WSActividad();

                ws_act.id = item.id_tipoactividad.ToString();
                ws_act.nroDisposicion = "";
                list_actividad.Add(ws_act);
            }

        }

        private void Cargar_Normativa(int id_encomienda)
        {

            Encomienda_Normativas norma = db.Encomienda_Normativas.Where(x => x.id_encomienda == id_encomienda).FirstOrDefault();

            if (norma != null)
            {
                this.ws_datos_solicitud.idTipoNormativa = norma.id_tiponormativa;
                this.ws_datos_solicitud.idTipoNormativaSpecified = true;

                this.ws_datos_solicitud.idEntidadNormativa = Convert.ToInt32(norma.id_entidadnormativa);
                this.ws_datos_solicitud.idEntidadNormativaSpecified = true;
                this.ws_datos_solicitud.nroNormativa = norma.nro_normativa;
            }

        }

        private void Cargar_TipoTramite(int id_solicitud)
        {

            var ssit_sol =
                (
                    from sol in db.SSIT_Solicitudes
                    join tt in db.TipoTramite on sol.id_tipotramite equals tt.id_tipotramite
                    join te in db.TipoExpediente on sol.id_tipoexpediente equals te.id_tipoexpediente
                    join ste in db.SubtipoExpediente on sol.id_subtipoexpediente equals ste.id_subtipoexpediente
                    where sol.id_solicitud == id_solicitud
                    select new
                    {
                        tt.cod_tipotramite_ws,
                        te.cod_tipoexpediente_ws,
                        ste.cod_subtipoexpediente_ws
                    }
                ).FirstOrDefault();


            if (ssit_sol != null)
            {
                this.ws_datos_solicitud.tipoTramite = ssit_sol.cod_tipotramite_ws;
                this.ws_datos_solicitud.tipoExpediente = ssit_sol.cod_tipoexpediente_ws;
                this.ws_datos_solicitud.subtipoExpediente = ssit_sol.cod_subtipoexpediente_ws;
            }

        }



    }


}