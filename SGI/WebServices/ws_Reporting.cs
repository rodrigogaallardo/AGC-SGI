using ExternalService.Class;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SGI.WebServices
{
    public class ws_Reporting
    {
        public string GetToken(string user, string pass)
        {
            string hostParametro = ConfigurationManager.AppSettings["NombreParamHost"];
            string serviceParametro = ConfigurationManager.AppSettings["NombreParamServiceReporting"];
            string autenticateHostParametro = ConfigurationManager.AppSettings["NombreParamHostAutorizacion"];

            //string host = "http://www.dghpsh.agcontrol.gob.ar/test/ws.rest.files";
            string _host = "";
            if (hostParametro.IndexOf("http") < 0)
                _host = "http://" + hostParametro + serviceParametro;
            else
                _host = hostParametro + serviceParametro;

            var client = new RestClient(_host + autenticateHostParametro);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());
            client.Authenticator = new HttpBasicAuthenticator(user, pass);
            var request = new RestRequest(Method.POST);
            request.AddParameter("redirect", "false");
            request.AddParameter("redirectUrl", "");
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(string.Format("No se ha podido loguear en el servicio de Token con el usuario  {0}.", user));
            return response.Headers.Where(p => p.Name == "Token").First().Value.ToString();
        }

        internal ReportingEntity GetPDFSolicitudNueva(int id_solicitud, bool guardar)
        {
            return GetPDFReporte("SolicitudNueva", id_solicitud, guardar);
        }

        private ReportingEntity GetPDFReporte(string restParametro, int id_tramite, bool guardar)
        {
            string hostParametro = ConfigurationManager.AppSettings["NombreParamHost"];
            string serviceParametro = ConfigurationManager.AppSettings["NombreParamServiceReporting"];
            string userParametro = ConfigurationManager.AppSettings["NombreParamUser"];
            string passParametro = ConfigurationManager.AppSettings["NombreParamPass"];
            restParametro = "/api/" + restParametro;

            string _token = GetToken(userParametro, passParametro);

            string _host = "";
            if (hostParametro.IndexOf("http") < 0)
                _host = "http://" + hostParametro + serviceParametro;
            else
                _host = hostParametro + serviceParametro;

            var client = new RestClient(_host + restParametro);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest(string.Format("?id_tramite={0}&guardar={1}", id_tramite, guardar), Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);


            IRestResponse<ReportingEntity> response = client.Execute<ReportingEntity>(request);


            if (response.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                throw new Exception(response.Content);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido obtener el Reporte en PDF.");

            ReportingEntity ret = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportingEntity>(response.Content);

            return ret;

        }

        private ReportingEntity GetPDFReporte(string restParametro, int id_tramite, int id_tipo_informe, bool guardar)
        {
            string hostParametro = ConfigurationManager.AppSettings["NombreParamHost"]; 
            string serviceParametro = ConfigurationManager.AppSettings["NombreParamServiceReporting"];
            string userParametro = ConfigurationManager.AppSettings["NombreParamUser"];
            string passParametro = ConfigurationManager.AppSettings["NombreParamPass"];
            restParametro = "/api/" + restParametro;

            string _token = GetToken(userParametro, passParametro);

            string _host = "";
            if (hostParametro.IndexOf("http") < 0)
                _host = "http://" + hostParametro + serviceParametro;
            else
                _host = hostParametro + serviceParametro;

            var client = new RestClient(_host + restParametro);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest(string.Format("?id_tramite={0}&id_tipoinforme={1}&guardar={2}", id_tramite, id_tipo_informe, guardar), Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);


            IRestResponse<ReportingEntity> response = client.Execute<ReportingEntity>(request);


            if (response.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                throw new Exception(response.Content);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido obtener el Reporte en PDF.");

            ReportingEntity ret = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportingEntity>(response.Content);

            return ret;

        }

        public ReportingEntity GetPDFInformeCPadron(int id_tramite, int id_tipo_informe, bool guardar)
        {
            return GetPDFReporte("InformeCPadron", id_tramite, id_tipo_informe, guardar);
        }
        public ReportingEntity GetPDFTransmisionesInformeCPadron(int id_tramite, int id_tipo_informe, bool guardar)
        {
            return GetPDFReporte("InformeTransmisionesCPadron", id_tramite, id_tipo_informe, guardar);
        }

        public ReportingEntity GetPDFSolicitantePorIdSolicitud(int id_tramite)
        {
            return GetPDFReporte("Solicitante", id_tramite);
        }

        private ReportingEntity GetPDFReporte(string restParametro, int id_tramite)
        {
            string hostParametro = ConfigurationManager.AppSettings["NombreParamHost"];
            string serviceParametro = ConfigurationManager.AppSettings["NombreParamServiceReporting"];
            string userParametro = ConfigurationManager.AppSettings["NombreParamUser"];
            string passParametro = ConfigurationManager.AppSettings["NombreParamPass"];
            restParametro = "/api/" + restParametro;

            string _token = GetToken(userParametro, passParametro);

            string _host = "";
            if (hostParametro.IndexOf("http") < 0)
                _host = "http://" + hostParametro + serviceParametro;
            else
                _host = hostParametro + serviceParametro;

            var client = new RestClient(_host + restParametro);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest(string.Format("?id_tramite={0}", id_tramite), Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);

            IRestResponse<ReportingEntity> response = client.Execute<ReportingEntity>(request);


            if (response.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                throw new Exception(response.Content);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido obtener el Reporte en PDF.");

            ReportingEntity ret = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportingEntity>(response.Content);

            return ret;
        }

        public ReportingEntity GetPDFCertificadoHabilitacion(string restParametro, int id_tramite, string nroExpediente, bool impresionDePrueba)
        {
            string hostParametro = ConfigurationManager.AppSettings["NombreParamHost"]; //"http://www.dghpsh.agcontrol.gob.ar/test"; 
            string serviceParametro = ConfigurationManager.AppSettings["NombreParamServiceReporting"];
            string userParametro = ConfigurationManager.AppSettings["NombreParamUser"];
            string passParametro = ConfigurationManager.AppSettings["NombreParamPass"];
            restParametro = "/api/" + restParametro;

            string _token = GetToken(userParametro, passParametro);

            string _host = "";
            if (hostParametro.IndexOf("http") < 0)
                _host = "http://" + hostParametro + serviceParametro;
            else
                _host = hostParametro + serviceParametro;

            var client = new RestClient(_host + restParametro);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            if (nroExpediente == null)
                nroExpediente = "' '";

            var request = new RestRequest(string.Format("?id_tramite={0}&nro_Expediente={1}&impresionDePrueba={2}", id_tramite, nroExpediente, impresionDePrueba), Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);


            IRestResponse<ReportingEntity> response = client.Execute<ReportingEntity>(request);


            if (response.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                throw new Exception(response.Content);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido obtener el Reporte en PDF.");

            ReportingEntity ret = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportingEntity>(response.Content);

            return ret;

        }
    }
}