using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalService.Class
{
    public class GenerarCAAAutomaticoResponse
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

    public class Formulario
    {
        public int id_caa { get; set; }
        public int id_encomienda_agc { get; set; }
        public string zonaDeclarada { get; set; }
        public string observaciones_rubros { get; set; }
        public string observaciones_plantas { get; set; }
        public bool iniciado_x_AGC { get; set; }
        public int? idZonaMixturaRubros { get; set; }
        public Encomiendaprofesional encomiendaProfesional { get; set; }
        public Rubroscpu[] rubrosCPU { get; set; }
        public Rubroscur[] rubrosCur { get; set; }
        public Normativadto normativaDTO { get; set; }
        public Planta[] plantas { get; set; }
        public Titularespersonasfisica[] titularesPersonasFisicas { get; set; }
        public Titularespersonasjuridica[] titularesPersonasJuridicas { get; set; }
        public Ubicacione[] ubicaciones { get; set; }
        public Datoslocal datosLocal { get; set; }
    }

    public class Encomiendaprofesional
    {
        public string motivoRechazo { get; set; }
        public Estadoivadto estadoIVADTO { get; set; }
        public Profesionaldto profesionalDTO { get; set; }
        public CAAResultadosSCDTO resultadoSCDTO { get; set; }
        public IVADTO indicadoresValoracionAmbiental { get; set; }
    }

    public class Estadoivadto
    {
        public int id_estado_iva { get; set; }
        public string cod_estado_iva { get; set; }
        public string nom_estado_iva { get; set; }
    }

    public class Profesionaldto
    {
        public int id_profesional { get; set; }
        public string cuit { get; set; }
        public string apellido { get; set; }
        public string nombre { get; set; }
        public string nacionalidad { get; set; }
        public string drTelefono { get; set; }
    }

    public class Normativadto
    {
        public int id_CAAtiponormativa { get; set; }
        public int id_tiponormativa { get; set; }
        public string codTipoNormativa { get; set; }
        public string descTipoNormativa { get; set; }
        public int id_entidadnormativa { get; set; }
        public string codEntidadNormativa { get; set; }
        public string descEntidadNormativa { get; set; }
        public string nro_normativa { get; set; }
        public DateTime fecha_normativa { get; set; }
        public bool verificada_SADE { get; set; }
    }

    public class Datoslocal
    {
        public int id_caadatoslocal { get; set; }
        public decimal superficie_cubierta_dl { get; set; }
        public decimal superficie_descubierta_dl { get; set; }
        public decimal dimesion_frente_dl { get; set; }
        public bool lugar_carga_descarga_dl { get; set; }
        public bool estacionamiento_dl { get; set; }
        public bool red_transito_pesado_dl { get; set; }
        public bool sobre_avenida_dl { get; set; }
        public string materiales_pisos_dl { get; set; }
        public string materiales_paredes_dl { get; set; }
        public string materiales_techos_dl { get; set; }
        public string materiales_revestimientos_dl { get; set; }
        public int cantidad_sanitarios_dl { get; set; }
        public decimal frente_dl { get; set; }
        public decimal fondo_dl { get; set; }
        public decimal lateral_izquierdo_dl { get; set; }
        public decimal lateral_derecho_dl { get; set; }
        public bool sobrecarga_corresponde_dl { get; set; }
        public int cantidad_operarios_dl { get; set; }
    }

    public class Rubroscur
    {
        public int id_caarubro { get; set; }
        public int idRubro { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public decimal superficieHabilitar { get; set; }
        public string cod_ImpactoAmbiental { get; set; }
        public string desc_ImpactoAmbiental { get; set; }
        public string letraAnexo { get; set; }
        public decimal? altura { get; set; }
    }

    public class Planta
    {
        public int id_caatiposector { get; set; }
        public int id_tiposector { get; set; }
        public string tiposector { get; set; }
        public string detalle_tiposector { get; set; }
    }

    public class Titularespersonasfisica
    {
        public int id_personafisica { get; set; }
        public string apellido { get; set; }
        public string nombres { get; set; }
        public int id_tipodoc_personal { get; set; }
        public string tipodoc_personal { get; set; }
        public string nro_Documento { get; set; }
        public string cuit { get; set; }
        public int id_tipoiibb { get; set; }
        public string tipoiibb { get; set; }
        public string ingresos_Brutos { get; set; }
        public string calle { get; set; }
        public int nro_Puerta { get; set; }
        public string piso { get; set; }
        public string depto { get; set; }
        public int id_Localidad { get; set; }
        public string localidad { get; set; }
        public int id_Provincia { get; set; }
        public string provincia { get; set; }
        public string codigo_Postal { get; set; }
        public object telefonoArea { get; set; }
        public object telefonoPrefijo { get; set; }
        public object telefonoSufijo { get; set; }
        public string telefonoMovil { get; set; }
        public object sms { get; set; }
        public string email { get; set; }
        public bool mismoFirmante { get; set; }
    }

    public class Ubicacione
    {
        public int id_caaubicacion { get; set; }
        public int id_ubicacion { get; set; }
        public int id_tipoubicacion { get; set; }
        public string tipoubicacion { get; set; }
        public int id_subtipoubicacion { get; set; }
        public string subtipoubicacion { get; set; }
        public string local_subtipoubicacion { get; set; }
        public string deptoLocal_ubicacion { get; set; }
        public int? zonaMixtura { get; set; }
        public string observaciones { get; set; }
        public int? nroPartidaMatriz { get; set; }
        public string seccion { get; set; }
        public string manzana { get; set; }
        public string parcela { get; set; }
        public Distrito[] distritos { get; set; }
        public Mixtura[] mixturas { get; set; }
        public Propiedadeshorizontale[] propiedadesHorizontales { get; set; }
        public Puerta[] puertas { get; set; }
        public object[] coordenadas { get; set; }
    }

    public class Distrito
    {
        public int id_caaubicaciondistrito { get; set; }
        public int idDistrito { get; set; }
        public string codigoDistrito { get; set; }
    }

    public class Propiedadeshorizontale
    {
        public int id_caaprophorizontal { get; set; }
        public int id_propiedadhorizontal { get; set; }
        public int nroPartidaHorizontal { get; set; }
        public string unidadFuncional { get; set; }
    }

    public class Puerta
    {
        public int id_caapuerta { get; set; }
        public int codigo_calle { get; set; }
        public string nombre_calle { get; set; }
        public int nroPuerta { get; set; }
        public object nroPuertaHasta { get; set; }
    }

    public class Certificado
    {
        public int idFile { get; set; }
        public string rowid { get; set; }
        public string fileName { get; set; }
        public string extension { get; set; }
        public string contentType { get; set; }
        public int size { get; set; }
        public string md5 { get; set; }
        public string rawBytes { get; set; }
    }

    public class Pago
    {
        public int id_sol_pago { get; set; }
        public int id_pago { get; set; }
        public string monto_pago { get; set; }
        public int nro_boletaUnica { get; set; }
        public DateTime createDate { get; set; }
    }

    public class Historialestado
    {
        public int id_enchistest { get; set; }
        public int id_solicitud { get; set; }
        public string cod_estado_ant { get; set; }
        public string cod_estado_nuevo { get; set; }
        public DateTime fecha_modificacion { get; set; }
        public string usuario_modificacion { get; set; }
    }

    public class CAAResultadosSCDTO
    {
        public decimal indice_form_A { get; set; }
        public decimal indice_form_B { get; set; }
        public int id_tipocertificado { get; set; }
        public string codigo_tipocertificado { get; set; }
        public string nombre_tipocertificado { get; set; }
    }

    public class IVADTO
    {
        public string cod_form { get; set; }
        public string nombre_form { get; set; }
        public decimal valor { get; set; }
        public object criterio { get; set; }
    }
}
