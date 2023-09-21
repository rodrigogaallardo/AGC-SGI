using RestSharp;
using SGI.StaticClassNameSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Authenticators;

namespace SGI.WebServices
{
    public class ws_FilesRest
    {
        public static int subirArchivo(string nombreArchivo, byte[] archivo)
        {
            string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.File");
            string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.File");
            int idfile = 0;
            Guid guid;
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.File");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/authenticate");
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            string passwordBase64 = Convert.ToBase64String(
                       ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", userName, passWord))
                       );
            request.AddHeader("Authorization", "Basic " + passwordBase64);

            IRestResponse response = clientrest.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                clientrest.BaseUrl = new Uri(url_servicio + "/api/files");
                guid = Guid.Parse(Convert.ToString(response.Headers.Where(p => p.Name == "Token").First().Value));

                request = new RestRequest("?fileName=" + nombreArchivo, Method.POST);
                request.AddHeader("Token", guid.ToString());
                request.AddFile("name", archivo, nombreArchivo);

                response = clientrest.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (int.TryParse(response.Content, out idfile))
                    {
                        //obtengo el id de file
                    }
                }else
                    throw new Exception("No se pudo guardar el archivo.");
            }
            else
                throw new Exception("No se pudo guardar el archivo.");
            return idfile;
        }

        public static byte[] DownloadFile(int idfile, out string fileExtension)
        {
            return descargarArchivo(idfile, out fileExtension);
        }

        public static byte[] DownloadFile(int idfile)
        {
            string fileExtension = "";
            return descargarArchivo(idfile, out fileExtension);
        }

        public static byte[] descargarArchivo(int idfile, out string fileExtension)
        {
            string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.File");
            string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.File");
            Guid guid;
            byte[] archivo = null;
            fileExtension = "";
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.File");
            clientrest.BaseUrl = new Uri(url_servicio + "/api/authenticate");
            RestRequest request = new RestRequest();
            request.Method = Method.POST;
            string passwordBase64 = Convert.ToBase64String(
                       ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", userName, passWord))
                       );
            request.AddHeader("Authorization", "Basic " + passwordBase64);

            IRestResponse response = clientrest.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                clientrest.BaseUrl = new Uri(url_servicio + "/api/files");
                guid = Guid.Parse(Convert.ToString(response.Headers.Where(p => p.Name == "Token").First().Value));
                request = new RestRequest("?IdFile=" + idfile);
                request.Method = Method.GET;

                request.AddHeader("Token", guid.ToString());

                response = clientrest.Execute(request);

                LogError.Write(new Exception(">>>>>>>>>>>>>>DESCARGAR ARCHIVO>>>>>>>>>>>>>>"));
                LogError.Write(new Exception("CLIENT: " + Funciones.GetDataFromClient(clientrest)));
                LogError.Write(new Exception("REQUEST: " + Funciones.GetDataFromRequest(request)));
                LogError.Write(new Exception("RESPONSE: " + Funciones.GetDataFromResponse(response)));
                LogError.Write(new Exception("<<<<<<<<<<<<<<DESCARGAR ARCHIVO<<<<<<<<<<<<<<"));
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("No se ha podido descargar el file en el servicio ");
   
                archivo = response.RawBytes;
                //fileExtension = response.Headers.First(p => p.Name.Equals("Content-Disposition")).Value.ToString().Replace("attachment; filename=", "");
                fileExtension = response.Headers.First(p => p.Name.Equals("Filename")).Value.ToString();

            }
            return archivo;
        }
        public static byte[] descargarArchivo_new(int id_file, out string fileExtension)
        {
            string hostParametro = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.File");
            //string serviceParametro = "/ws.rest.files";
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.File");
            string fileParametro = "/api/files";
            string userParametro = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.File");//ConfigurationManager.AppSettings["NombreParamUser"];
            string passParametro = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.File");//ConfigurationManager.AppSettings["NombreParamPass"];

            string _token = GetToken(userParametro, passParametro);

            //string host = "http://www.dghpsh.agcontrol.gob.ar/test/ws.rest.files";
            string _host = "";
            if (hostParametro.IndexOf("http") < 0)
                _host = "http://" + hostParametro;
            else
                _host = hostParametro;

            var client = new RestClient(_host + fileParametro);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest("?IdFile=" + id_file, Method.GET);
            request.AddParameter("redirect", "false");
            request.AddParameter("redirectUrl", "");
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");

            request.AddHeader("Token", _token);

            IRestResponse response = client.Execute(request);


            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido descargar el file en el servicio ");

            //fileExtension = response.Headers.First(p => p.Name.Equals("Content-Disposition")).Value.ToString().Replace("attachment; filename=", "");
            //fileExtension = Path.GetExtension(fileExtension);
            fileExtension = response.Headers.First(p => p.Name.Equals("Filename")).Value.ToString();
            if (response.ContentLength <= 0)
                LogError.Write(new Exception("RESPONSE error red: " + Funciones.GetDataFromResponse(response)));
            return response.RawBytes;
        }
        /// <summary>
        /// get token 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string GetToken(string user, string pass)
        {
            string hostParametro = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.File");
            string autenticateHostParametro = "/Api/Authenticate";

            //string host = "http://www.dghpsh.agcontrol.gob.ar/test/ws.rest.files";
            string _host = "";
            if (hostParametro.IndexOf("http") < 0)
                _host = "http://" + hostParametro;
            else
                _host = hostParametro;

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
                throw new Exception(string.Format("Error al adquirir el Token con el usuario  {0}.", user));
            return response.Headers.Where(p => p.Name == "Token").First().Value.ToString();
        }
    }
}
