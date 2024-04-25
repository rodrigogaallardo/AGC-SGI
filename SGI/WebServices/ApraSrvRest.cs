using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.UI;
using RestSharp;
using ExternalService.Class;
using SGI.Webservices.ws_interface_AGC;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ExternalService
{
    public class ApraSrvRest : Page
    {
        public string Usuario;
        public string Password;
        public string UrlApraAgc;

        public ApraSrvRest()
        {
            Usuario = ConfigurationManager.AppSettings["UsuarioApraAgc"];
            Password = ConfigurationManager.AppSettings["PasswordApraAgc"];
            UrlApraAgc = ConfigurationManager.AppSettings["UrlApraAgc"];
        }

        static HttpClient client = new HttpClient();

        private async Task<TokenResponse> LoginAsync()
        {
            TokenResponse tokenResponse;
            var tokenResponseApplication = System.Web.HttpContext.Current?.Application["TokenResponse"];

            if (tokenResponseApplication != null)
            {
                try
                {
                    tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(tokenResponseApplication.ToString());
                    if (tokenResponse.expires.ToLocalTime().AddMinutes(-10) > DateTime.Now)
                    {
                        return tokenResponse;
                    }
                }
                catch (JsonException jex)
                {
                    tokenResponseApplication = null;
                }

            }
            string result = "";
            string usuario = this.Usuario;
            string password = this.Password;
            string UrlApraAgc = this.UrlApraAgc;

            var query = new Dictionary<string, string>()
            {
                ["usuario"] = usuario,
                ["password"] = password
            };

            var json = JsonConvert.SerializeObject(query);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = UrlApraAgc + "api/Login";
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(300);
            try
            {
                var timeoutTask = Task.Delay(10000);
                var responseTask = client.PostAsync(url, data);

                // Wait for either the HTTP request or the timeout
                var completedTask = await Task.WhenAny(responseTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    throw new TimeoutException("The request timed out.");
                }

                var response = await responseTask;
                //var response = await client.PostAsync(url, data);
                result = await response.Content.ReadAsStringAsync();
                if (System.Web.HttpContext.Current != null)
                {
                    System.Web.HttpContext.Current.Application["TokenResponse"] = result;
                    var borrar = System.Web.HttpContext.Current.Application["TokenResponse"];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            if (System.Web.HttpContext.Current != null)
            {
                tokenResponseApplication = System.Web.HttpContext.Current.Application["TokenResponse"];
                tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(tokenResponseApplication.ToString());
            }
            else
            { tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result); }

            return tokenResponse;
        }

        //Post 
        public async Task<GenerarCAAAutoResponse> GenerarCAAAutomatico(int IdEncomienda, string codSeguridad)
        {
            try
            {
                TokenResponse tokenResponse = await this.LoginAsync();
                string UrlApraAgc = ConfigurationManager.AppSettings["UrlApraAgc"];
                string apiUrl = $"{UrlApraAgc}api/CAA/GenerarCAAAutomatico";

                SolicitudEncomienda obj = new SolicitudEncomienda()
                {
                    idEncomienda = IdEncomienda,
                    codigoSeguridad = codSeguridad
                };

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.token);

                    client.Timeout = TimeSpan.FromSeconds(300);

                    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                    var timeoutTask = Task.Delay(client.Timeout);
                    var responseTask = client.PostAsync(apiUrl, httpContent);
                    await responseTask.ConfigureAwait(false);
                    var completedTask = await Task.WhenAny(responseTask, timeoutTask);
                    try
                    {
                        if (completedTask == timeoutTask)
                        {
                            //LogError.Write(new TimeoutException("The request timed out."), "HTTP request exception in GenerarCAAAutomatico");
                            throw new TimeoutException("The request timed out.");
                        }

                        HttpResponseMessage response = await responseTask;

                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            int id_solicitud_caa = int.Parse(content);
                            return new GenerarCAAAutoResponse
                            {
                                id_solicitud_caa = id_solicitud_caa,
                                ErrorCode = response.StatusCode.ToString(),
                                ErrorDesc = null
                            };
                        }
                        else
                        {
                            //LogError.Write($"Non-successful HTTP response: {response.StatusCode} - {response.ReasonPhrase}");
                            return new GenerarCAAAutoResponse
                            {
                                id_solicitud_caa = 0,
                                ErrorCode = response.StatusCode.ToString(),
                                ErrorDesc = response.ReasonPhrase.ToString()
                            };
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        //LogError.Write(ex, "HTTP request exception in GenerarCAAAutomatico");
                        return new GenerarCAAAutoResponse
                        {
                            id_solicitud_caa = 0,
                            ErrorCode = "HttpRequestException",
                            ErrorDesc = $"Error al realizar la solicitud HTTP: {ex.Message}"
                        };
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                //LogError.Write(ex, "Ocurrió un error en GenerarCAAAutomatico ");
                return new GenerarCAAAutoResponse
                {
                    id_solicitud_caa = 0,
                    ErrorCode = "HttpRequestException",
                    ErrorDesc = $"Ocurrió un error al intentar generar el CAA automatico: {ex.Message}"
                };
            }
        }

        //Gets
        public async Task<GetCAAResponse> GetCaa(int id_solicitud)
        {
            try
            {
                TokenResponse tokenResponse = await this.LoginAsync();
                string UrlApraAgc = ConfigurationManager.AppSettings["UrlApraAgc"];
                string apiUrl = $"{UrlApraAgc}api/CAA/GetCAA?id_solicitud={id_solicitud}";

                var client = new RestClient(apiUrl);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("content-type", "application/json; charset=utf-8");
                request.AddHeader("Authorization", "Bearer " + tokenResponse.token);
                // request.AddBody(data);
                try
                {
                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string content = response.Content;
                        GetCAAResponse getCAAResponse = new GetCAAResponse();
                        getCAAResponse = JsonConvert.DeserializeObject<GetCAAResponse>(content);

                        return getCAAResponse;//JsonConvert.SerializeObject(content);
                    }
                    else
                        return null;//($"La solicitud no fue exitosa. Código de estado: {response.StatusCode}");
                }
                catch (HttpRequestException ex)
                {
                    return null;//($"Error al realizar la solicitud HTTP: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return null;// ($"Error generico al ejecutar  GetCaa. Mensaje: {ex.Message}");
            }
        }

        public async Task<List<GetBUIsCAAResponse>> GetBUIsCAA(int id_solicitud)
        {
            try
            {
                TokenResponse tokenResponse = await this.LoginAsync();
                string UrlApraAgc = ConfigurationManager.AppSettings["UrlApraAgc"];
                string apiUrl = $"{UrlApraAgc}api/CAA/GetBUIsCAA?id_solicitud={id_solicitud}";

                var client = new RestClient(apiUrl);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("content-type", "application/json; charset=utf-8");
                request.AddHeader("Authorization", "Bearer " + tokenResponse.token);
                // request.AddBody(data);
                try
                {
                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string content = response.Content;
                        List<GetBUIsCAAResponse> getBUIsCAAResponseList = new List<GetBUIsCAAResponse>();
                        getBUIsCAAResponseList = JsonConvert.DeserializeObject<List<GetBUIsCAAResponse>>(content);
                        return getBUIsCAAResponseList;
                    }
                    else
                        return null;
                }
                catch (HttpRequestException ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<GetBUIsPagosResponse>> GetBUIsPagos(List<int> id_pagoList)
        {
            try
            {
                TokenResponse tokenResponse = await this.LoginAsync();
                string UrlApraAgc = ConfigurationManager.AppSettings["UrlApraAgc"];
                string apiUrl = $"{UrlApraAgc}api/CAA/GetBUIsPagos";

                var client = new RestClient(apiUrl);
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("content-type", "application/json; charset=utf-8");
                request.AddHeader("Authorization", "Bearer " + tokenResponse.token);

                var data = JsonConvert.SerializeObject(id_pagoList);

                request.AddParameter("application/json", data, ParameterType.RequestBody);
                try
                {
                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string content = response.Content;
                        List<GetBUIsPagosResponse> getBUIsPagosResponseList = new List<GetBUIsPagosResponse>();
                        getBUIsPagosResponseList = JsonConvert.DeserializeObject<List<GetBUIsPagosResponse>>(content);
                        // return JsonConvert.SerializeObject(content);
                        return getBUIsPagosResponseList;
                    }
                    else
                        return null;
                }
                catch (HttpRequestException ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int[]> GetIdPagosCAAsbyEncomiendas(List<int> IdEncomiendaList)
        {
            try
            {
                TokenResponse tokenResponse = await this.LoginAsync();
                string UrlApraAgc = ConfigurationManager.AppSettings["UrlApraAgc"];
                string apiUrl = $"{UrlApraAgc}api/CAA/GetIdPagosCAAbyEncomiendas";

                var client = new RestClient(apiUrl);
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("content-type", "application/json; charset=utf-8");
                request.AddHeader("Authorization", "Bearer " + tokenResponse.token);

                var data = JsonConvert.SerializeObject(IdEncomiendaList);

                request.AddParameter("application/json", data, ParameterType.RequestBody);
                try
                {
                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string content = response.Content;
                        //List<int> getCAAByEncomiendasResponseList = new List<int>();
                        int[] getCAAByEncomiendasResponseList = JsonConvert.DeserializeObject<int[]>(content);
                        //return JsonConvert.SerializeObject(content);
                        return getCAAByEncomiendasResponseList;
                    }
                    else
                        return null;
                }
                catch (HttpRequestException ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<GetCAAsByEncomiendasResponse>> GetCAAsByEncomiendas(List<int> IdEncomiendaList)
        {
            try
            {
                TokenResponse tokenResponse = await this.LoginAsync();
                string UrlApraAgc = ConfigurationManager.AppSettings["UrlApraAgc"];
                string apiUrl = $"{UrlApraAgc}api/CAA/GetCAAsbyEncomiendas";

                var client = new RestClient(apiUrl);
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("content-type", "application/json; charset=utf-8");
                if (tokenResponse != null)
                    request.AddHeader("Authorization", "Bearer " + tokenResponse.token);
                var data = JsonConvert.SerializeObject(IdEncomiendaList);

                request.AddParameter("application/json", data, ParameterType.RequestBody);
                try
                {
                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string content = response.Content;
                        List<GetCAAsByEncomiendasResponse> getCAAsByEncomiendasResponseList = new List<GetCAAsByEncomiendasResponse>();
                        DtoCAA[] l = JsonConvert.DeserializeObject<DtoCAA[]>(content);


                        getCAAsByEncomiendasResponseList = JsonConvert.DeserializeObject<List<GetCAAsByEncomiendasResponse>>(content);
                        return getCAAsByEncomiendasResponseList;
                    }
                    else
                        return null;
                }
                catch (HttpRequestException ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// Recibe id_caa (IdSolicitud) y devuelve verdadero o falso, codigo de error y descripcion de este
        /// </summary>
        /// <param name="IdSolicitud"></param>
        /// <param name="codSeguridad"></param>
        /// <returns></returns>
        public async Task<ValidarCodigoSeguridadResponse> ValidarCodigoSeguridad(int IdSolicitud, string codSeguridad)
        {
            try
            {
                TokenResponse tokenResponse = await this.LoginAsync();
                string UrlApraAgc = ConfigurationManager.AppSettings["UrlApraAgc"];
                string apiUrl = $"{UrlApraAgc}api/CAA/ValidarCodigoSeguridad?id_solicitud={IdSolicitud}&CodigoSeguridad={codSeguridad}";

                var client = new RestClient(apiUrl);
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("content-type", "application/json; charset=utf-8");
                request.AddHeader("Authorization", "Bearer " + tokenResponse.token);

                try
                {
                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string content = response.Content;
                        bool validarCodigoSeguridadResponse = new bool();
                        validarCodigoSeguridadResponse = JsonConvert.DeserializeObject<bool>(content);
                        return new ValidarCodigoSeguridadResponse
                        {
                            EsValido = validarCodigoSeguridadResponse,
                            ErrorCode = response.StatusCode.ToString(),
                            ErrorDesc = null
                        };
                    }
                    else
                        return new ValidarCodigoSeguridadResponse
                        {
                            EsValido = false,
                            ErrorCode = response.StatusCode.ToString(),
                            ErrorDesc = $"La solicitud no fue exitosa. Código de estado: {response.StatusCode}"
                        };
                }
                catch (HttpRequestException ex)
                {
                    return new ValidarCodigoSeguridadResponse
                    {
                        EsValido = false,
                        ErrorCode = "HttpRequestException",
                        ErrorDesc = $"Error al realizar la solicitud HTTP: {ex.Message}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ValidarCodigoSeguridadResponse
                {
                    EsValido = false,
                    ErrorCode = "Error Generico",
                    ErrorDesc = $"Error genérico al ejecutar GenerarCAAAutomatico. Mensaje: {ex.Message}"
                };
            }
        }

        public async Task<AsociarAnexoTecnicoResponse> AsociarAnexoTecnico(int IdSolicitud, string codSeguridad, int IdEncomienda)
        {
            try
            {
                TokenResponse tokenResponse = await this.LoginAsync();
                string UrlApraAgc = ConfigurationManager.AppSettings["UrlApraAgc"];
                string apiUrl = $"{UrlApraAgc}api/CAA/AsociarAnexoTecnico?id_solicitud={IdSolicitud}&CodigoSeguridad={codSeguridad}&id_encomienda={IdEncomienda}";

                var client = new RestClient(apiUrl);
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("content-type", "application/json; charset=utf-8");
                request.AddHeader("Authorization", "Bearer " + tokenResponse.token);

                try
                {
                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string content = response.Content;
                        bool asociarAnexoTecnicoResponse = new bool();
                        asociarAnexoTecnicoResponse = JsonConvert.DeserializeObject<bool>(content);
                        return new AsociarAnexoTecnicoResponse
                        {
                            Asociado = asociarAnexoTecnicoResponse,
                            ErrorCode = response.StatusCode.ToString(),
                            ErrorDesc = null
                        };
                    }
                    else
                        return new AsociarAnexoTecnicoResponse
                        {
                            Asociado = false,
                            ErrorCode = response.StatusCode.ToString(),
                            ErrorDesc = $"La solicitud no fue exitosa. Código de estado: {response.StatusCode}"
                        };
                }
                catch (HttpRequestException ex)
                {
                    return new AsociarAnexoTecnicoResponse
                    {
                        Asociado = false,
                        ErrorCode = "HttpRequestException",
                        ErrorDesc = $"Error al realizar la solicitud HTTP: {ex.Message}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new AsociarAnexoTecnicoResponse
                {
                    Asociado = false,
                    ErrorCode = "Error Generico",
                    ErrorDesc = $"Error generico al ejecutar  GenerarCAAAutomatico. Mensaje: {ex.Message}"
                };
            }
        }
    }

    //Se crean clases para cada post por eventuales cambios en los servicios, en nombres de propiedades o lo que sea
    public class SolicitudEncomienda
    {
        public int idEncomienda { get; set; }
        public string codigoSeguridad { get; set; }
    }

    public class ValCodSeguridadCLS
    {
        public int id_solicitud { get; set; }
        public string CodigoSeguridad { get; set; }
    }

    public class AsocAnexoTecnicoCLS
    {
        public int id_solicitud { get; set; }
        public string CodigoSeguridad { get; set; }
        public int id_encomienda { get; set; }
    }

    public class GetPagosCLS
    {
        public int id_encomienda { get; set; }
    }

    public class TokenResponse
    {
        public string token { get; set; }
        public bool success { get; set; }
        public DateTime expires { get; set; }
        public string[] errors { get; set; }
    }
}