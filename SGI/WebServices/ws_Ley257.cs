using Newtonsoft.Json;
using RestSharp;
using SGI.Model;
using SGI.StaticClassNameSpace;
using System;
using System.Collections.Generic;
using System.Net;

namespace SGI.WebServices
{
    public class ws_Ley257
    {
        // Se creo esta funcion debido a que JsonConvert.DeserializeObject<Ley257Token>(response.Content) producia una excepcion por cambios en el servicio externo
        private Ley257Token ParsearJsonToken(string jsonString)
        {
            int len = jsonString.Length;
            string aux = jsonString;
            int i = 0;
            int f = len - 1;

            while (jsonString[i] != '{')
                i++;

            while (jsonString[f] != '}')
                f--;

            aux = aux.Substring(i + 1, f - 2);

            Dictionary<string, string> data = new Dictionary<string, string>();
            var arr1 = aux.Split(',');
            foreach (var s in arr1)
            {
                var kv = s.Replace("\"", "");
                var arr2 = kv.Split(':');
                var key = arr2[0];
                var val = kv.Replace(key + ":", "");
                data.Add(key, val);
            }

            try
            {
                Ley257Token token = new Ley257Token
                {
                    AccessToken = data["access_token"],
                    TokenType = data["token_type"],
                    ExpiresIn = int.Parse(data["expires_in"]),
                    Issued = data["issued"],
                    Expires = data["expires"],
                    Scope = data["scope"]
                };

                return token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Response Token(
                string urlBase,
                string method,
                string client_id,
                string client_secret)
        {
            try
            {
                if (urlBase.Substring(urlBase.Length - 1) != "/")
                    urlBase = urlBase + "/";

                var url = string.Format("{0}{1}", urlBase, method);

                var client = new RestClient(url);
                RestRequest request = new RestRequest(Method.POST);

                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", $"client_id={client_id}&client_secret={client_secret}", ParameterType.RequestBody);

                var response = client.Execute(request);

                string ParamLogs = string.Format("client_id={0}&client_secret={1}", client_id, client_secret);
                string requestLog = JsonConvert.SerializeObject(request);
                string responseLog = JsonConvert.SerializeObject(response);
                Funciones.Ley257GenerarLog(ParamLogs, requestLog, responseLog, Functions.GetUserId());

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al generar token Ley257.",
                    };
                }

                string jsonString = response.Content;

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = ParsearJsonToken(jsonString),
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public Response DarBajaUbicacion(
            string token,
            string urlBase,
            string method,
            Ley257RequestDarBajaUbicacion data)
        {
            try
            {
                if (urlBase.Substring(urlBase.Length - 1) != "/")
                    urlBase = urlBase + "/";

                var url = string.Format("{0}{1}", urlBase, method);

                var client = new RestClient(url);
                RestRequest request = new RestRequest(Method.DELETE);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("content-type", "application/json; charset=utf-8");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddBody(data);

                var response = client.Execute(request);


                string ParamLogs = JsonConvert.SerializeObject(data);
                string requestLog = JsonConvert.SerializeObject(request);
                string responseLog = JsonConvert.SerializeObject(response);
                Funciones.Ley257GenerarLog(ParamLogs, requestLog, responseLog, Functions.GetUserId());


                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return new Response
                    {
                        IsSuccess = true,
                        Message = "No existen ubicaciones para esta SMP.",
                    };
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al obtener la ubicacion.",
                    };
                }

                var cont = response.Content;


                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    //Result = result,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}