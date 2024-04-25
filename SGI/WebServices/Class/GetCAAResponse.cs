using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalService.Class
{
    public class GetCAAResponse
    {
        public int id_solicitud { get; set; }
        public string codigoSeguridad { get; set; }
        public DateTime fechaIngreso { get; set; }
        public int id_tipotramite { get; set; }
        public string tipotramite { get; set; }
        public int id_estado { get; set; }
        public string estado { get; set; }
        public int id_tipocertificado { get; set; }
        public string codigo_tipocertificado { get; set; }
        public string nombre_tipocertificado { get; set; }
        public string nroCertificado { get; set; }
        public DateTime? fechaVencCertificado { get; set; }
        public object nroActuacion { get; set; }
        public object anioActuacion { get; set; }
        public object tipoActoAdministrativo { get; set; }
        public object nroActoAdministrativo { get; set; }
        public object entidadActoAdministrativo { get; set; }
        public object anioActoAdministrativo { get; set; }
        public bool exentoBUI { get; set; }
        public string nroExpedienteSADE { get; set; }
        public int? id_tad { get; set; }    //como en los ambientes de prueba esto es null hay que dejarlo asi
        public bool esCur { get; set; }
        public string nroGEDOCertificado { get; set; }
        public DateTime createDate { get; set; }
        public string createUser { get; set; }
        public Pago[] pagos { get; set; }
        public Historialestado[] historialEstados { get; set; }
        public Formulario formulario { get; set; }
        public Certificado certificado { get; set; }
    }
    public class Titularespersonasjuridica
    {
        public int id_personajuridica { get; set; }
        public int id_caa { get; set; }
        public int id_TipoSociedad { get; set; }
        public string tipoSociedad { get; set; }
        public string razon_Social { get; set; }
        public string cuit { get; set; }
        public int id_tipoiibb { get; set; }
        public string tipoiibb { get; set; }
        public string nro_IIBB { get; set; }
        public string calle { get; set; }
        public int nroPuerta { get; set; }
        public string piso { get; set; }
        public string depto { get; set; }
        public int id_localidad { get; set; }
        public string localidad { get; set; }
        public int id_provincia { get; set; }
        public string provincia { get; set; }
        public string codigo_Postal { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
    }

    public class Mixtura
    {
        public int id_caaubicacionmixtura { get; set; }
        public int idZonaMixtura { get; set; }
    }
}