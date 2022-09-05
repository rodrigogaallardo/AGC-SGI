using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SGI.Mailer
{
    public class MailRest
    {

        public class EmailEntity
        {
            public int IdEmail { get; set; }
            public Guid? Guid { get; set; }
            public int IdOrigen { get; set; }
            public int IdTipoEmail { get; set; }
            public int IdEstado { get; set; }
            public int? CantIntentos { get; set; }
            public int? CantMaxIntentos { get; set; }
            public int? Prioridad { get; set; }
            public DateTime FechaAlta { get; set; }
            public DateTime? FechaEnvio { get; set; }
            public string Email { get; set; }
            public string Cc { get; set; }
            public string Cco { get; set; }
            public string Asunto { get; set; }
            public string Html { get; set; }
            public DateTime? FechaLectura { get; set; }
        }

        public static int SendEmail(EmailEntity entity)
        {
            string userName = Parametros.GetParam_ValorChar("SGI.Username.WebService.Rest.Email");
            string passWord = Parametros.GetParam_ValorChar("SGI.Password.WebService.Rest.Email");
            int idMail = 0;
            Guid guid;
            RestClient clientrest = new RestClient();
            string url_servicio = Parametros.GetParam_ValorChar("SGI.Url.WebService.Rest.Email");
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
                clientrest.BaseUrl = new Uri(url_servicio + "/api/emails");
                guid = Guid.Parse(Convert.ToString(response.Headers.Where(p => p.Name == "Token").First().Value));

                request.AddHeader("Token", guid.ToString());
                request.AddJsonBody(entity);

                response = clientrest.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    if (int.TryParse(response.Content, out idMail))
                    {
                        //obtengo el id de file
                    }
                }
            
            }
            return idMail;
        }
    }
}