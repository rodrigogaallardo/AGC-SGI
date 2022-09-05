using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using static SGI.WebServices.wsTAD;

namespace SGI.WebServices
{
    public class wsGP
    {
        public static List<PerfilDTO> perfilesPorTrata(string _urlESB, string p_trata)
        {
            string uriString = _urlESB + "/tiposTramite/" + p_trata + "/perfiles";
            var client = new RestClient(uriString);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");

            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<clsError>(response.Content);
                throw new Exception((error.codigo != null ? error.codigo.Value + " - " : "") + error.error);
            }

            List<PerfilDTO> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PerfilDTO>>(response.Content);
            return list;
        }
        public static bool asociarExpediente(string _urlESB, int idTAD, string nroExpediente)
        {
            string uriString = _urlESB + "/tramites/" + idTAD + "/expedientes";

            var client = new RestClient(uriString);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest(Method.POST);
            var clsBody = new clsAsociar();
            clsBody.nroExpediente = nroExpediente;
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(clsBody), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = Newtonsoft.Json.JsonConvert.DeserializeObject<clsError>(response.Content);
                throw new Exception((error.codigo != null ? error.codigo.Value + " - " : "") + error.error);
            }
            return true;
        }

        public static void vincularSolicitanteSE(string _urlESB, int p_idTad, string p_CuitOperador, int p_idPerfilOperador,
            string p_CuitSolicitante, int p_idPerfilSolicitante, string p_nombreSolicitante, string p_apellidoSolicitante,
            string p_Sistema)
        {
            string uriString = _urlESB + "/tramites/" + p_idTad + "/solicitantes";
            var client = new RestClient(uriString);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest(Method.POST);
            var clsBody = new clsVincularSolicitanteSE();
            clsBody.operador = new clsOperador();
            clsBody.operador.cuit = p_CuitOperador;
            clsBody.operador.idPerfil =p_idPerfilOperador;
            clsBody.solicitante = new clsSolicitante();
            clsBody.solicitante.cuit = p_CuitSolicitante;
            clsBody.solicitante.idPerfil = p_idPerfilSolicitante;
            clsBody.solicitante.nombre = p_nombreSolicitante;
            clsBody.solicitante.apellido = p_apellidoSolicitante;
            clsBody.Sistema = p_Sistema;
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(clsBody), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                var error = Newtonsoft.Json.JsonConvert.DeserializeObject<clsError>(response.Content);
                throw new Exception((error.codigo != null ? error.codigo.Value + " - " : "") + error.error);
            }
        }

        public static List<clsParticipantes> GetParticipantesxTramite(string _urlESB, int id_tad)
        {
            List<clsParticipantes> result = new List<clsParticipantes>();

            string uriString = _urlESB + "/tramites/" + id_tad.ToString() + "/participaciones";
            var clientrest = new RestClient(uriString);

            RestRequest request = new RestRequest();
            request.Method = Method.GET;

            IRestResponse response = clientrest.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = JsonConvert.DeserializeObject<List<clsParticipantes>>(response.Content);
            }
            else
            {
                clsError error = null;
                try
                {
                    error = JsonConvert.DeserializeObject<clsError>(response.Content);
                }
                catch (Exception)
                {
                    throw new Exception("500 - Error inesperado en el ESB.");
                }
                if (error.codigo == null && error.error == null)
                    throw new Exception("500 - Error inesperado en el ESB.");

                if (error.codigo != 6)
                    throw new Exception((error.codigo != null ? error.codigo.Value + " - " : "") + error.error);
            }
            return result;
        }

        public static void DesvincularParticipante(string _urlESB, int p_idTad, string p_cuitOperador, int p_idPerfilOperador, string p_Sistema,
            string p_cuitParticipante, int p_idPerfilParticipante)
        {
            string uriString = _urlESB + "/tramites/" + p_idTad + "/participaciones?cuitParticipante=" + p_cuitParticipante +
                "&idPerfilParticipante=" + p_idPerfilParticipante + "&funcionario=true";
            var client = new RestClient(uriString);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());


            var request = new RestRequest(Method.DELETE);
            var clsBody = new clsDesvincularParticipante();
            clsBody.sistema = p_Sistema;
            clsBody.operador = new clsOperador();
            clsBody.operador.cuit = p_cuitOperador;
            clsBody.operador.idPerfil = p_idPerfilOperador;
            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(clsBody), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                clsError error = null;
                try
                {
                    error = JsonConvert.DeserializeObject<clsError>(response.Content);
                }
                catch (Exception)
                {
                    throw new Exception("500 - Error inesperado en el ESB.");
                }
                if (error.codigo == null && error.error == null)
                    throw new Exception("500 - Error inesperado en el ESB.");

                throw new Exception((error.codigo != null ? error.codigo.Value + " - " : "") + error.error);
            }
        }

    }

    public class clsAsociar
    {
        public string nroExpediente { get; set; }
    }
    public class clsVincularSolicitanteSE
    {
        public clsOperador operador { get; set; }
        public clsSolicitante solicitante { get; set; }
        public string Sistema { get; set; }
    }

    public class clsOperador
    {
        public string cuit { get; set; }
        public int idPerfil { get; set; }
    }

    public class clsSolicitante
    {
        public string cuit { get; set; }
        public int idPerfil { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }

    }

    public class clsParticipantes
    {
        public int idTAD { get; set; }
        public string cuit { get; set; }
        public int idPerfil { get; set; }
        public bool? vigenciaParticipante { get; set; }
        public bool? accesoGP { get; set; }
        public bool? vistaDetallada { get; set; }
    }
    public class PerfilDTO
    {
        public int idPerfil { get; set; }
        public string nombrePerfil { get; set; }
        public bool perfilObligatorio { get; set; }
    }

    public class clsDesvincularParticipante
    {
        public string sistema { get; set; }
        public clsOperador operador { get; set; }
    }
}
