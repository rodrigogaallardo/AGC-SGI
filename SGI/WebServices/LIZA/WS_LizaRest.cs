using RestSharp;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SGI.WebServices.LIZA
{
    public class WS_LizaRest
    {
        public static WS_Ticket generarTicket(int id_solicitud, List<Item_doc> archivos)
        {
            WS_Token token = WS_LizaRest.login();
            DGHP_Entities db = new DGHP_Entities();

            SSIT_Solicitudes sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
            SSIT_Solicitudes_Ubicaciones sol_ubi = db.SSIT_Solicitudes_Ubicaciones.FirstOrDefault(x => x.id_solicitud == sol.id_solicitud);

            List<WS_Proceso> listProcesos = WS_LizaRest.ObtencionProcesos(token);

            string nombre_proceso = Functions.GetParametroChar("SGI.LIZA.Nombre.Proceso");
            string nombre_area = Functions.GetParametroChar("SGI.LIZA.Nombre.Area");
            string nombre_origen = Functions.GetParametroChar("SGI.LIZA.Nombre.Origen");
            string nombre_motivo = Functions.GetParametroChar("SGI.LIZA.Nombre.Motivo");
            string nombre_actividad = Functions.GetParametroChar("SGI.LIZA.Nombre.Actividad");

        
            int id_proceso = 0;
            foreach (WS_Proceso pro in listProcesos)
                if (pro.Nombre == nombre_proceso)
                {
                    id_proceso = pro.Id;
                    break;
                }
            if (id_proceso == 0)
                throw new Exception("No se encontro el Proceso");

            WS_Area Areas = WS_LizaRest.ObtencionAreas(token);
            int id_area = 0;
            foreach (WS_Item_Area area in Areas.Areas)
                if (area.Nombre == nombre_area)
                {
                    id_area = area.Id;
                    break;
                }
            if (id_area == 0)
                throw new Exception("No se encontro el Area");

            int id_origen = 0;
            WS_Origen Origenes = WS_LizaRest.ObtencionOrigenes(token, id_area);
            foreach (WS_Item_Origen origen in Origenes.Origenes)
                if (origen.Descripcion == nombre_origen)
                {
                    id_origen = origen.Id;
                    break;
                }
            if (id_origen == 0)
                throw new Exception("No se encontro el Origen");

            List<WS_Motivo> listMotivos = WS_LizaRest.ObtencionMotivos(token, id_origen, id_area);
            int id_motivo = 0;
            foreach (WS_Motivo motivo in listMotivos)
                if (motivo.Nombre == nombre_motivo)
                    id_motivo = motivo.Id;
            if (id_motivo == 0)
                throw new Exception("No se encontro el Motivo");

            List<WS_Actividad> listActividades = WS_LizaRest.ObtencionActividades(token, id_proceso);
            int id_actividad = 0;
            foreach (WS_Actividad act in listActividades)
                if (act.Nombre == nombre_actividad)
                    id_actividad = act.Id;
            if (id_actividad == 0)
                throw new Exception("No se encontro la Actividad");

            WS_Creacion_Ticket ticket = new WS_Creacion_Ticket();
            ticket.GrupoInformableID = null;
            ticket.AreaID = id_area;
            ticket.MotivoID = id_motivo;
            ticket.OrigenID = id_origen;
            ticket.ActividadID = id_actividad;
            ticket.IdentificadorMotivo = 12345;
            ticket.Descripcion = "desc";
            ticket.TipoFechaSip = "Fija";//tiposFechas.Tipos.First();
            ticket.FechaOrigen = "9/9/2016";
            ticket.FechaSip = "9/9/2016";

            WS_Entidad EntidadInspeccionable = new WS_Entidad();
            EntidadInspeccionable.Tipo = "Local";
            EntidadInspeccionable.Seccion = sol_ubi.Ubicaciones.Seccion != null ? sol_ubi.Ubicaciones.Seccion.Value.ToString() : "";
            EntidadInspeccionable.Manzana = sol_ubi.Ubicaciones.Manzana;
            EntidadInspeccionable.Parcela = sol_ubi.Ubicaciones.Parcela;
            EntidadInspeccionable.NroExpediente = string.IsNullOrEmpty(sol.NroExpedienteSade) ? "" : sol.NroExpedienteSade;
            EntidadInspeccionable.NroSolicitud = sol.id_solicitud.ToString();
            EntidadInspeccionable.AnioExpediente = sol.CreateDate.Year.ToString();
            EntidadInspeccionable.UsigUbicacionID = sol_ubi.Ubicaciones.id_ubicacion.ToString();
            ticket.EntidadInspeccionable = EntidadInspeccionable;
            WS_Observacion ob = new WS_Observacion();
            ob.Texto = "Observacion ticket de prueba";
            ob.UsuarioID = token.UsuarioID;
            ticket.ObservacionSip = ob;
            ticket.ObservacionTicket = ob;

            return crearTicket(token, ticket, archivos);

        }

        #region Metodos Rest
        public static WS_Token login()
        {
            string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.LIZA");
            string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.LIZA");
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.LIZA");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/account/login");

            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("nombreUsuario", userName);
            request.AddParameter("clave", passWord);
            IRestResponse<WS_Result_Login> response = clientrest.Execute<WS_Result_Login>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data.Token;
            }
            else
                throw new Exception("Error WS-Liza:" + response.Data.Resultado);
        }

        public static List<WS_Proceso> ObtencionProcesos(WS_Token token)
        {
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.LIZA");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/proceso/procesosHabilitados");
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("usuarioID", token.UsuarioID);
            request.AddParameter("tokenID", token.ID);

            IRestResponse<List<WS_Proceso>> response = clientrest.Execute<List<WS_Proceso>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
                throw new Exception("Error WS-Liza: No se pudieron obtener los procesos.");
        }

        public static WS_Area ObtencionAreas(WS_Token token)
        {
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.LIZA");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/ticket/areas");
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("usuarioID", token.UsuarioID);
            request.AddParameter("tokenID", token.ID);

            IRestResponse<WS_Area> response = clientrest.Execute<WS_Area>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
                throw new Exception("Error WS-Liza: No se pudieron obtener las areas.");
        }

        public static WS_Origen ObtencionOrigenes(WS_Token token, int areaId)
        {
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.LIZA");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/ticket/origenes/" + areaId);//"/api/ticket/origenes/{idArea:int}",
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("usuarioID", token.UsuarioID);
            request.AddParameter("tokenID", token.ID);

            IRestResponse<WS_Origen> response = clientrest.Execute<WS_Origen>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
                throw new Exception("Error WS-Liza: No se pudieron obtener los Oreigenes.");
        }

        public static WS_Tipo_Fecha ObtencionTiposFecha(WS_Token token)
        {
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.LIZA");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/ticket/tiposfecha");
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("usuarioID", token.UsuarioID);
            request.AddParameter("tokenID", token.ID);

            IRestResponse<WS_Tipo_Fecha> response = clientrest.Execute<WS_Tipo_Fecha>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
                throw new Exception("Error WS-Liza: No se pudieron obtener los Tipos de fecha.");
        }

        public static List<WS_Motivo> ObtencionMotivos(WS_Token token, int origenId, int areaId)
        {
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.LIZA");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/ticket/motivos/" + origenId + "/" + areaId); //"/api/ticket/motivos/{idOrigen:int}/{idArea:int}", 
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("usuarioID", token.UsuarioID);
            request.AddParameter("tokenID", token.ID);

            IRestResponse<List<WS_Motivo>> response = clientrest.Execute<List<WS_Motivo>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
                throw new Exception("Error WS-Liza: No se pudieron obtener los motivos.");
        }

        public static List<WS_Grupo> ObtencionGrupos(WS_Token token, int motivoId, int areaId)
        {
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.LIZA");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/motivo/grupos/" + motivoId + "/" + areaId); //("/api/motivo/grupos/{idMotivo:int}/{idArea:int}", 
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("usuarioID", token.UsuarioID);
            request.AddParameter("tokenID", token.ID);

            IRestResponse<List<WS_Grupo>> response = clientrest.Execute<List<WS_Grupo>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
                throw new Exception("Error WS-Liza: No se pudieron obtener los grupos.");
        }

        public static List<WS_Actividad> ObtencionActividades(WS_Token token, int procesoId)
        {
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.LIZA");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/proceso/actividades/" + procesoId); //("/api/proceso/actividades/{idProceso:int}
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("usuarioID", token.UsuarioID);
            request.AddParameter("tokenID", token.ID);

            IRestResponse<List<WS_Actividad>> response = clientrest.Execute<List<WS_Actividad>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
                throw new Exception("Error WS-Liza: No se pudieron obtener las actividades.");
        }

        public static List<WS_Transicion> ObtencionTransiciones(WS_Token token, int actividadId)
        {
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.LIZA");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/actividad/transiciones/" + actividadId); //api/actividad/transiciones/{idActividad:int}", 
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("usuarioID", token.UsuarioID);
            request.AddParameter("tokenID", token.ID);

            IRestResponse<List<WS_Transicion>> response = clientrest.Execute<List<WS_Transicion>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
                throw new Exception("Error WS-Liza: No se pudieron obtener las transiciones.");
        }

        public static WS_Ticket crearTicket(WS_Token token, WS_Creacion_Ticket ticket, List<Item_doc> archivos)
        {
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.LIZA");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/ticket/create");
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("usuarioID", token.UsuarioID);
            request.AddParameter("tokenID", token.ID);
            string serealizabe = SimpleJson.SerializeObject(ticket);
            request.AddParameter("ticket", @serealizabe);
            if(archivos!=null)
                foreach(Item_doc a in archivos)
                    request.AddFile("archivo", a.documento, a.nombre, a.tipo);
            IRestResponse<WS_Ticket> response = clientrest.Execute<WS_Ticket>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
                throw new Exception("Error WS-Liza: No se pudieron crear el ticket.");
        }

        public static WS_Response_Ticket obtencionTicket(string alias)
        {
            WS_Token token = WS_LizaRest.login();
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.LIZA");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/ticket/traer/" + alias); //("/api/ticket/traer/{alias}", ", 
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            request.AddParameter("usuarioID", token.UsuarioID);
            request.AddParameter("tokenID", token.ID);

            IRestResponse<WS_Response_Ticket> response = clientrest.Execute<WS_Response_Ticket>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
                throw new Exception("Error WS-Liza: No se pudieron obtener el ticket.");
        }
        #endregion
    }

    public class WS_Result_Login
    {
        public string Resultado { get; set; }

        public string Mensaje { get; set; }

        public WS_Token Token { get; set; }
    }

    public class WS_Token
    {
        public Guid ID { get; set; }

        public Guid UsuarioID { get; set; }

        public DateTime Vencimiento { get; set; }
    }

    public class WS_Actividad
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public bool Final { get; set; }

        public string TipoActividad { get; set; }

        public string TipoElemento { get; set; }

        public bool CargaDeArchivoObligatoria { get; set; }

        public bool EsPublica { get; set; }

        public string UrlTransiciones { get; set; }
    }

    public class WS_Proceso
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public WS_Actividad ActividadInicio { get; set; }

        public string UrlActividades { get; set; }
    }

    public class WS_Item_Area
    {

        public int Id { get; set; }

        public string Nombre { get; set; }

        public bool EsArea { get; set; }

        public string UrlOrigenes { get; set; }
    }

    public class WS_Area
    {
        public List<WS_Item_Area> Areas { get; set; }
    }

    public class WS_Item_Origen
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public string UrlMotivos { get; set; }
    }

    public class WS_Origen
    {
        public List<WS_Item_Origen> Origenes { get; set; }
    }

    public class WS_Tipo_Fecha
    {
        public List<string> Tipos { get; set; }
    }

    public class WS_Motivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool RequierIdentificador { get; set; }
        public string NombreIdentificador { get; set; }
        public string Validador { get; set; }
        public string Mascara { get; set; }
        public bool RequiereArchivo { get; set; }
        public bool EsInformable { get; set; }
        public bool Seleccionable { get; set; }
        public int CoeficientePrioridad { get; set; }
        public int CoeficienteEsfuerzo { get; set; }
        public string UrlGrupos { get; set; }
    }

    public class WS_Grupo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool EsArea { get; set; }
    }

    public class WS_Transicion
    {
        public int Id { get; set; }
        public WS_Actividad ActividadActual { get; set; }
        public WS_Actividad ActividadSiguiente { get; set; }
        public string Alias { get; set; }
        public bool EsCancelar { get; set; }
    }

    public class WS_Ticket
    {
        public string Alias { get; set; }
        public string UrlSeguimiento { get; set; }
    }

    public class WS_Observacion
    {
        public string Texto { get; set; }
        public Guid UsuarioID { get; set; }
    }

    public class WS_Entidad
    {
        public string Tipo { get; set; }
        public string Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public string NroSolicitud { get; set; }
        public string NroExpediente { get; set; }
        public string AnioExpediente { get; set; }
        public string UsigUbicacionID { get; set; }
    }

    public class WS_Creacion_Ticket
    {
        public int? GrupoInformableID { get; set; }
        public int AreaID { get; set; }
        public int MotivoID { get; set; }
        public int OrigenID { get; set; }
        public int ActividadID { get; set; }
        public int IdentificadorMotivo { get; set; }
        public string Descripcion { get; set; }
        public string TipoFechaSip { get; set; }
        public string FechaOrigen { get; set; }
        public string FechaSip { get; set; }
        public WS_Entidad EntidadInspeccionable { get; set; }
        public WS_Observacion ObservacionSip { get; set; }
        public WS_Observacion ObservacionTicket { get; set; }
    }

    public class WS_Archivo
    {
        public string Descripcion { get; set; }
        public bool EsPdf { get; set; }
        public string Nombre { get; set; }
        public string Thumbnails { get; set; }
        public string Tipo { get; set; }
        public string UrlAdjunto { get; set; }
    }

    public class WS_OpcionesDeRespuesta
    {
        public int Id { get; set; }
        public string Texto { get; set; }
    }

    public class WS_Pregunta
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public string TipoRespuesta { get; set; }
        public List<WS_OpcionesDeRespuesta> OpcionesDeRespuesta { get; set; }
        public string Validaciones { get; set; }
        public string ClaveDataset { get; set; }
        public string Reglas { get; set; }
        public string CondicionShow { get; set; }
        public string ValorPorDefecto { get; set; }
        public bool RequiereFoto { get; set; }
        public int OrdenPosicion { get; set; }
        public string PreguntasRelacionadas { get; set; }
        public string PreguntasRelacionadasConmigo { get; set; }
        public string CategoriasRelacionadasConmigo { get; set; }
        public bool Requerida { get; set; }
        public int PreguntaOriginalId { get; set; }
    }

    public class WS_SubCategorias
    {
        public int Id { get; set; }
        public int IdMultiplicidad { get; set; }
        public string Nombre { get; set; }
        public bool Multiple { get; set; }
    }
    public class WS_Categoria
    {
        public int CategoriaOriginalId { get; set; }
        public string CondicionShow { get; set; }
        public int Id { get; set; }
        public int IdMultiplicidad { get; set; }
        public bool Multiple { get; set; }
        public string Nombre { get; set; }
        public int OrdenPosicion { get; set; }
        public List<WS_Pregunta> Preguntas { get; set; }
        public List<WS_Pregunta> PreguntasRelacionadas { get; set; }
        public List<WS_SubCategorias> SubCategorias { get; set; }
    }

    public class WS_Variable
    {
        public string Nombre { get; set; }
        public bool Oculto { get; set; }
        public string Valor { get; set; }
    }

    public class WS_Checklist
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<WS_Categoria> CategoriaInicial { get; set; }
        public List<WS_Variable> Variables { get; set; }
    }

    public class WS_Domicilio_Image
    {
        public string Descripcion { get; set; }
        public bool EsPdf { get; set; }
        public string Nombre { get; set; }
        public string Thumbnails { get; set; }
        public string Tipo { get; set; }
        public string Titulo { get; set; }
        public string UrlAdjunto { get; set; }
    }
    public class WS_Domicilio
    {
        public int Coordenada_X { get; set; }
        public int Coordenada_Y { get; set; }
        public string DatosEI { get; set; }
        public string DatosUSIG { get; set; }
        public string Manzana { get; set; }
        public string NombreCalle { get; set; }
        public string NumeroPuerta { get; set; }
        public string Parcela { get; set; }
        public string PisoDepartamento { get; set; }
        public string PuertasAlternativas { get; set; }
        public string Seccion { get; set; }
        public string Texto { get; set; }
        public List<WS_Domicilio_Image> ImagenesDomicilio { get; set; }
    }

    public class WS_Response_Ticket
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string DireccionTexto { get; set; }
        public WS_Domicilio Domicilio { get; set; }
        public string Actividad { get; set; }
        public string Antecedentes { get; set; }
        public List<WS_Archivo> Archivos { get; set; }
        public string Area { get; set; }
        public string Autor { get; set; }
        public List<WS_Checklist> Checklist { get; set; }
        public bool Closed { get; set; }
        public string DescripcionSIP { get; set; }
        public string Editor { get; set; }
        public bool EsBorrador { get; set; }
        public string Fecha { get; set; }
        public string FechaDocumentoOrigen { get; set; }
        public string GrupoMotivoInformable { get; set; }
        public string Hitorico { get; set; }
        public string IdMotivo { get; set; }
        public string IdOrigen { get; set; }
        public string Identificador { get; set; }
        public string MensajeAlSolicitante { get; set; }
        public string Motivo { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesSIP { get; set; }
        public string Origen { get; set; }
        public string TipoFecha { get; set; }
    }

    public class Item_doc
    {
        public string nombre { get; set; }

        public string tipo { get; set; }

        public byte[] documento { get; set; }
    }
}
