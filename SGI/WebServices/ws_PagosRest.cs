using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SGI.WebServices
{
    public class ws_PagosRest
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
            return idfile;
        }


        /// <summary>
        /// get token 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public string GetToken(string user, string pass)
        {
            string url_servicio = Parametros.GetParam_ValorChar("Pagos.Url.Rest");

            string autenticateHostParametro = "/Api/Authenticate";

            var client = new RestClient(url_servicio + autenticateHostParametro);
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

        public void CancelarPago(int IdPago)
        {
            string url_servicio = Parametros.GetParam_ValorChar("Pagos.Url.Rest");

            string userParametro = Parametros.GetParam_ValorChar("SGI.Pagos.User"); 
            string passParametro = Parametros.GetParam_ValorChar("SGI.Pagos.Password"); 

            string _token = GetToken(userParametro, passParametro);

            var client = new RestClient(url_servicio + "/api/CancelarBoletaUnica");
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest("?IdPago=" + IdPago);
            request.AddParameter("redirect", "false");
            request.AddParameter("redirectUrl", "");
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");

            request.AddHeader("Token", _token);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido cancelar el pago en el servicio.");
        }

        public BUBoletaUnica GetBoletaUnica(int IdPago)
        {
            string url_servicio = Parametros.GetParam_ValorChar("Pagos.Url.Rest");

            string userParametro = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string passParametro = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            string _token = GetToken(userParametro, passParametro);

            var client = new RestClient(url_servicio + "/api/GetBoletaUnica");

            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest("?IdPago=" + IdPago, Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);

            IRestResponse<BUBoletaUnica> response = client.Execute<BUBoletaUnica>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido obtener la boleta en el servicio.");

            BUBoletaUnica ret = Newtonsoft.Json.JsonConvert.DeserializeObject<BUBoletaUnica>(response.Content);

            return ret;
        }

        public List<BUConcepto> GetConcepto(int CodigoConcepto1, int CodigoConcepto2, int CodigoConcepto3)
        {
            string url_servicio = Parametros.GetParam_ValorChar("Pagos.Url.Rest");

            string userParametro = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string passParametro = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            string _token = GetToken(userParametro, passParametro);

            var client = new RestClient(url_servicio + "/api/GetConcepto");
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest("?CodigoConcepto1=" + CodigoConcepto1 + "&CodigoConcepto2=" + CodigoConcepto2 + "&CodigoConcepto3=" + CodigoConcepto3, Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);

            IRestResponse<List<BUConcepto>> response = client.Execute<List<BUConcepto>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido obtener el concepto en el servicio.");

            List<BUConcepto> ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BUConcepto>>(response.Content);

            return ret;
        }

        public List<BUConcepto> GetConceptos()
        {
            string url_servicio = Parametros.GetParam_ValorChar("Pagos.Url.Rest");

            string userParametro = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string passParametro = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            string _token = GetToken(userParametro, passParametro);

            var client = new RestClient(url_servicio + "/api/GetConceptos");

            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);

            IRestResponse<List<BUConcepto>> response = client.Execute<List<BUConcepto>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se han podido obtener los concepto en el servicio.");

            List<BUConcepto> ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BUConcepto>>(response.Content);

            return ret;
        }

        public List<BUIConceptoConfig> GetConceptosConfig()
        {
            string url_servicio = Parametros.GetParam_ValorChar("Pagos.Url.Rest");

            string userParametro = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string passParametro = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            string _token = GetToken(userParametro, passParametro);

            var client = new RestClient(url_servicio + "/api/GetConceptosConfig");

            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);

            IRestResponse<List<BUIConceptoConfig>> response = client.Execute<List<BUIConceptoConfig>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido obtener la config del concepto en el servicio.");

            List<BUIConceptoConfig> ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BUIConceptoConfig>>(response.Content);

            return ret;
        }

        public string GetEstadoPago(int IdPago)
        {
            string url_servicio = Parametros.GetParam_ValorChar("Pagos.Url.Rest");

            string userParametro = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string passParametro = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            string _token = GetToken(userParametro, passParametro);

            var client = new RestClient(url_servicio + "/api/GetEstadoPago");

            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest("?IdPago=" + IdPago, Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido obtener el estado de la boleta en el servicio.");

            return response.Content.Replace("\"", "");
        }

        public string GetEstadoPosteriorAlPago(int IdPago)
        {
            string url_servicio = Parametros.GetParam_ValorChar("Pagos.Url.Rest");

            string userParametro = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string passParametro = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            string _token = GetToken(userParametro, passParametro);

            var client = new RestClient(url_servicio + "/api/GetEstadoPosteriorAlPago");

            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest("?IdPago=" + IdPago, Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido obtener el estado de la boleta en el servicio.");

            return response.Content;
        }

        public byte[] GetPDFBoletaUnica(int IdPago)
        {
            string url_servicio = Parametros.GetParam_ValorChar("Pagos.Url.Rest");

            string userParametro = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string passParametro = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            string _token = GetToken(userParametro, passParametro);

            var client = new RestClient(url_servicio + "/api/GetPDFBoletaUnica");

            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest("?IdPago=" + IdPago, Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido obtener el PDF de la boleta en el servicio.");

            return response.RawBytes;
        }

        public byte[] GetQR(int IdPago)
        {
            string url_servicio = Parametros.GetParam_ValorChar("Pagos.Url.Rest");

            string userParametro = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string passParametro = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            string _token = GetToken(userParametro, passParametro);

            var client = new RestClient(url_servicio + "/api/GetQR");

            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest("?IdPago=" + IdPago, Method.GET);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido obtener el QR de la boleta en el servicio.");

            return response.RawBytes;
        }

        public List<BUBoletaUnica> ObtenerBoletas(List<int> IdPagos)
        {
            string url_servicio = Parametros.GetParam_ValorChar("Pagos.Url.Rest");

            string userParametro = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string passParametro = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            string _token = GetToken(userParametro, passParametro);

            var client = new RestClient(url_servicio + "/api/PostObtenerBoletas");
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest(Method.POST);

            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);
            request.AddJsonBody(IdPagos);

            IRestResponse<List<BUBoletaUnica>> response = client.Execute<List<BUBoletaUnica>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se han podido obtener las boleta en el servicio.");

            List<BUBoletaUnica> ret = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BUBoletaUnica>>(response.Content);

            return ret;
        }

        public BUBoletaUnica GenerarBoleta(BUDatosBoleta DatosBoleta)
        {
            string url_servicio = Parametros.GetParam_ValorChar("Pagos.Url.Rest");

            string userParametro = Parametros.GetParam_ValorChar("SGI.Pagos.User");
            string passParametro = Parametros.GetParam_ValorChar("SGI.Pagos.Password");

            string _token = GetToken(userParametro, passParametro);

            var client = new RestClient(url_servicio + "/api/PostGenerarBoletaUnica");
            client.ClearHandlers();
            client.AddHandler("application/json", new JsonDeserializer());

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json charset=UTF-8");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Token", _token);
            request.AddJsonBody(DatosBoleta);

            IRestResponse<BUBoletaUnica> response = client.Execute<BUBoletaUnica>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new Exception(response.Content);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("No se ha podido generar la boleta en el servicio.");

            BUBoletaUnica ret = Newtonsoft.Json.JsonConvert.DeserializeObject<BUBoletaUnica>(response.Content);

            return ret;
        }

    }

    public class BUBoletaUnica
    {
        public int IdBoletaUnica { get; set; }
        public int IdPago { get; set; }
        public string CodBarras { get; set; }
        public long NroBoletaUnica { get; set; }
        public int Dependencia { get; set; }
        public BUDatosContribuyente Contribuyente { get; set; }
        public decimal MontoTotal { get; set; }
        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; }
        public DateTime? FechaPago { get; set; }
        public DateTime? FechaAnulada { get; set; }
        public DateTime? FechaCancelada { get; set; }
        public string TrazaPago { get; set; }
        public string CodigoVerificador { get; set; }
        public string NroBUI { get; set; }
        public Guid? BUI_ID { get; set; }
    }

    public class BUDatosContribuyente
    {
        public BUTipoPersona TipoPersona { get; set; }
        public string ApellidoyNombre { get; set; }
        public Nullable<BUTipodocumento> TipoDoc { get; set; }
        public string Documento { get; set; }
        public string Direccion { get; set; }
        public string Piso { get; set; }
        public string Departamento { get; set; }
        public string Localidad { get; set; }
        public string CodPost { get; set; }
        public string Email { get; set; }
        public string TipoDocumentoValue { get; set; }
        public string TipoPersonaValue { get; set; }
    }
    public enum BUTipoPersona
    {
        Fisica,
        Juridica
    }
    public enum BUTipodocumento
    {
        DNI,
        CUIT,
        LC,
        CI,
        LE,
        PAS
    }
    public class BUConcepto
    {
        public int idPagoConcepto { get; set; }
        public int IdPago { get; set; }
        public int CodConcepto1 { get; set; }
        public int CodConcepto2 { get; set; }
        public int CodConcepto3 { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal Importe { get; set; }
        public decimal ValorDetalle { get; set; }
        public bool? AdmiteReglas { get; set; }
        public Guid ItemID { get; set; }
    }

    public class BUIDependencia
    {
        public Guid ID { get; set; }
        public string Nombre { get; set; }
        public List<string> Items { get; set; }
    }

    public class BUIConceptoConfig
    {
        public bool AdmiteReglas { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public List<BUIDetalleConcepto> Detalles { get; set; }
        public Guid ID { get; set; }
        public bool TieneCantidadFija { get; set; }
        public bool TieneValorFijo { get; set; }
        public decimal Valor { get; set; }
        public int Vigencia { get; set; }
    }

    public class BUIDetalleConcepto
    {
        public string Descripcion { get; set; }
        public Guid ID { get; set; }
        public Guid ItemID { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
    }

    public class BUDatosBoleta
    {
        public BUDatosContribuyente datosConstribuyente { get; set; }
        public List<BUConcepto> listaConcepto { get; set; }
    }
}
