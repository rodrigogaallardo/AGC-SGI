using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SGI.WebServices
{
    public class wsTAD
    {
        public class clsError
        {
            public int? codigo { get; set; }
            public string error { get; set; }
        }

        private class clsCrearTramiteTAD
        {
            public string tipoTramite { get; set; }
            public string cuit { get; set; }
            public string ubicacion { get; set; }
            public string sistemaExterno { get; set; }
            public string idSolicitud { get; set; }
        }
        private class clsActualizarTramiteTAD
        {
            public string idSolicitud { get; set; }
            public string nroExpediente { get; set; }
            public string codTipoTramite { get; set; }
            public string ubicacion { get; set; }
        }

        public static int crearTramiteTAD(string _urlESB, String cuit, string codTrata, string domicilio, string sistemaExterno, int idSolicitud)
        {
            int idTad = 0;
            string uriString = _urlESB + "/tiposTramite/" + codTrata + "/tramites";

            RestClient clientrest = new RestClient();
            clientrest.BaseUrl = new Uri(uriString);
            RestRequest request = new RestRequest();
            request.Method = Method.POST;

            var clsBody = new clsCrearTramiteTAD();
            clsBody.cuit = cuit;
            clsBody.ubicacion = "-";
            clsBody.sistemaExterno = sistemaExterno;
            clsBody.idSolicitud = idSolicitud.ToString();

            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(clsBody), ParameterType.RequestBody);

            IRestResponse response = clientrest.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                var objResult = JsonConvert.DeserializeObject<dynamic>(response.Content);
                idTad = Convert.ToInt32(objResult.idTramite);
            }
            else
            {
                var error = JsonConvert.DeserializeObject<clsError>(response.Content);
                throw new Exception((error.codigo != null ? error.codigo.Value + " - " : "") + error.error);
            }

            return idTad;
        }

        public static void actualizarTramite(string _urlESB, int idTramite, int idSolicitud, string numeroExpediente, string tipoTramite, string ubicacion)
        {
            string uriString = _urlESB + "/tramites/" + idTramite;

            RestClient clientrest = new RestClient();
            clientrest.BaseUrl = new Uri(uriString);
            RestRequest request = new RestRequest();
            request.Method = Method.PUT;

            var clsBody = new clsActualizarTramiteTAD();
            clsBody.codTipoTramite = string.IsNullOrEmpty(tipoTramite) ? string.Empty : tipoTramite;
            clsBody.ubicacion = ubicacion;
            clsBody.nroExpediente = string.IsNullOrEmpty(numeroExpediente) ? string.Empty : numeroExpediente;
            clsBody.idSolicitud = idSolicitud.ToString();

            request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(clsBody), ParameterType.RequestBody);

            IRestResponse response = clientrest.Execute(request);
            //if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
            //{
            //    var error = JsonConvert.DeserializeObject<clsError>(response.Content);
            //    throw new Exception((error.codigo != null ? error.codigo.Value + " - " : "") + error.error);
            //}
        }
    }
}