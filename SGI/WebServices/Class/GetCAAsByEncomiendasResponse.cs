using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalService.Class
{
    public class GetCAAsByEncomiendasResponse
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
        public DateTime fechaVencCertificado { get; set; }
        public int? nroActuacion { get; set; }
        public int? anioActuacion { get; set; }
        public int? tipoActoAdministrativo { get; set; }
        public int? nroActoAdministrativo { get; set; }
        public int? entidadActoAdministrativo { get; set; }
        public int? anioActoAdministrativo { get; set; }
        public bool exentoBUI { get; set; }
        public string nroExpedienteSADE { get; set; }
        public int? id_tad { get; set; }
        public bool esCur { get; set; }
        public string nroGEDOCertificado { get; set; }
        public DateTime createDate { get; set; }
        public string createUser { get; set; }
        public Pago[] pagos { get; set; }
        public Historialestado[] historialEstados { get; set; }
        public Formulario formulario { get; set; }
        public Certificado certificado { get; set; }
    }

    public class Resultadoscdto
    {
        public int indice_form_A { get; set; }
        public int indice_form_B { get; set; }
        public int id_tipocertificado { get; set; }
        public string codigo_tipocertificado { get; set; }
        public string nombre_tipocertificado { get; set; }
    }

    public class Indicadoresvaloracionambiental
    {
        public string cod_form { get; set; }
        public string nombre_form { get; set; }
        public int valor { get; set; }
        public string criterio { get; set; }
    }

    public class Rubroscpu
    {
        public int id_caarubro { get; set; }
        public string cod_rubro { get; set; }
        public string desc_rubro { get; set; }
        public decimal? superficieHabilitar { get; set; }
        public bool antenaEmisora { get; set; }
        public string cod_ImpactoAmbiental { get; set; }
        public string desc_ImpactoAmbiental { get; set; }
        public DateTime createDate { get; set; }
    }

    public class Coordenada
    {
        public int id_caaubic_coordenada { get; set; }
        public int latitud_grados { get; set; }
        public int latitud_minutos { get; set; }
        public int latitud_segundos { get; set; }
        public int longitud_grados { get; set; }
        public int longitud_minutos { get; set; }
        public int longitud_segundos { get; set; }
    }
}
