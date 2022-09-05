using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace SGI.WebServices
{
    public class ws_MailsRest
    {
        public string GetToken(string userName, string passWord)
        {
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.Email");

            string autenticateHostParametro = "/Api/Authenticate";

            var client = new RestClient(url_servicio + autenticateHostParametro);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());
            client.Authenticator = new HttpBasicAuthenticator(userName, passWord);
            var request = new RestRequest(Method.POST);
            request.AddParameter("redirect", "false");
            request.AddParameter("redirectUrl", "");
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception(string.Format("No se ha podido loguear en el servicio de Token con el usuario  {0}.", userName));
            return response.Headers.Where(p => p.Name == "Token").First().Value.ToString();
        }

        public int SendMail(EmailServicePOST correo, string token)
        {
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.Email");

            string hostParametroMail = "/api/Email";

            var client = new RestClient(url_servicio + hostParametroMail);
            client.ClearHandlers();

            var request = new RestRequest(Method.POST);

            if (correo.Asunto.Length >= 300)
            {
                correo.Asunto = correo.Asunto.Substring(0, 296) + "...";
            }            

            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", token);
            request.AddJsonBody(correo);
            

            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("No se ha podido enviar el correo. Error: " + response.StatusCode.ToString());
            }
            return Convert.ToInt32(response.Headers.FirstOrDefault(t => t.Name == "ID").Value.ToString());
        }


        public EmailServiceGet GetMail(int idmail, string token)
        {
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.Email");

            var client = new RestClient(url_servicio + "/Api/mails?id_email=" + idmail);
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest(Method.GET);

            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", token);

            IRestResponse<EmailServiceGet> response = client.Execute<EmailServiceGet>(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("No se han podido obtener el mail en el  servicio. ");
            }
            EmailServiceGet mail = Newtonsoft.Json.JsonConvert.DeserializeObject<EmailServiceGet>(response.Content);

            return mail;
        }
    }
}