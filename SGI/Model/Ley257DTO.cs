using Newtonsoft.Json;

namespace SGI.Model
{
    public class Response
    {
        public bool IsSuccess
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public object Result
        {
            get;
            set;
        }
    }

    public class Ley257Token
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "issued")]
        public string Issued { get; set; }

        [JsonProperty(PropertyName = "expires")]
        public string Expires { get; set; }

        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }
    }

    public class Ley257RequestDarBajaUbicacion
    {
        public int Seccion { get; set; }
        public string Manzana { get; set; }
        public string Parcela { get; set; }
        public int UbicacionID { get; set; }
    }


    /*
     {
        "url": "http://test.agcontrol.gob.ar:7256/Cuenta/RedirectInstalaciones?Token=ACY5gkwP0kuTsjeLPT20",
        "success": true,
        "idInteracción": "bb86bc10-4495-47a4-844f-73e906083be8"
    }
     */
}
