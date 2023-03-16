using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using System.Linq;
using Elmah;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using System.Data;
using iTextSharp.text.pdf;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace SGI
{

    public class Constants
    {
        public const int SOLICITUDES_NUEVAS_MAYORES_A = 299999;

        public const int EsSolicitud = 200000;
        
        public const string EXTENSION_PDF = "pdf";
        public const string EXTENSION_JPG = "jpg";
        public const string SISTEMA = "SGI";


        public enum TipoResolucionHAB
        {
            //Identifica los distintos tipo de resolución que puede tener un tramite del  grupo de tablas HAB
            // osea  Habilitaciones, Ampliaciones y Redistribuciones de Uso
            Aprobado = 1,
            Rechazado = 2,
            Observado = 3
        }

        public enum BUI_EstadoPago
        {
            SinPagar = 0,
            Pagado = 1,
            Vencido = 2,
            Cancelada = 3,
            Anulada = 4
        }

        public static class CategoriaModificarEstado
        {
            public const string tramiteJava = "Solicitudes que tramitan en JAVA";
            public const string tramiteAnuladoSolicitud = "Solicitud de habilitación anulada";
            public const string tramiteAnuladoTransf = "Solicitud de transferencia anulada";
            public const string tramite200mil = "Solicitudes del circuito 200 mil";
            public const string tramiteTransferencias = "Transferencias";
        }

        public static class grupoCircuito
        {
            public const string SCPCIS = "SCP-CIS";
            public const string SCPR = "SCP-R";
            public const string SCPSE = "SCP-SE";
            public const string SCPESCU = "SCP-ESCU";
            public const string HP = "HP";
            public const string HPESCU = "HP-ESCU";
            public const string SSP = "SSP";
            public const string SSPA = "SSP-A";
        }
        public enum PagosTipoTramite
        {
            HAB = 1,
            CAA = 2
        }

        public enum BUI_MediosDePago
        {
            BoletaUnica = 0,
            PagoElectronico = 1
        }

        public const int SociedadHecho = 2;
        public const string Path_Temporal = @"C:\Temporal\";
        public enum EstadosSolicitudCambioRubros
        {
            EnProceso = 0,
            Anulada = 1,
            Confirmada = 2,
            Aprobada = 3,
            Rechazada = 4
        }

        public enum sqlErrNumber
        {
            ForeingKey = 547,
            UniqueKey = 2601,
            PrimaryKey = 2627
        }

        public const string PathTemporal = "C:\\Temporal\\";

        public static Guid ApplicationId
        {
            get
            {
                return Guid.Parse("5BC28D51-C240-4D79-87B4-27D554686CE3");
            }
        }

        public static string ApplicationName
        {
            get
            {
                RoleProvider roleProvider = System.Web.Security.Roles.Providers["SqlRoleProvider"];
                return roleProvider.ApplicationName;

            }
        }

        public class WebServices
        {
            public static Guid ApplicationId
            {
                get
                {
                    return Guid.Parse("12106F94-0D51-4FAC-B370-2E438E8E7E69");
                }
            }
        }

        public static readonly String IFCI_Empresas_AppName = "IFCI.Empresas";

        public static Guid IFCI_Empresas_AppId
        {
            get
            {
                DGHP_Entities db = new DGHP_Entities();
                Guid id = db.aspnet_Applications.FirstOrDefault(x => x.ApplicationName == ApplicationName).ApplicationId;
                return id;
            }
        }
        public static readonly String ECA_Empresas_AppName = "AscensoresRegistrados.Empresas";

        public static Guid ECA_Empresas_AppId
        {
            get
            {
                DGHP_Entities db = new DGHP_Entities();
                Guid id = db.aspnet_Applications.FirstOrDefault(x => x.ApplicationName == ECA_Empresas_AppName).ApplicationId;
                return id;
            }
        }

        public static class TiposDeDocumentosPlancheta
        {
            public const string PlanchetaCPadron = "PlanchetaCPadron";
        }

        public struct ENG_Tipos_Tareas
        {
            public const string Calificar = "10";
            public const string Calificar2 = "01";
            public const string Calificar3 = "51"; //Reconsideración
            public const string Correccion_Solicitud = "25";
            public const string Enviar_a_DGFyC = "35";
            public const string Revision_Gerente = "12";
            public const string Revision_Gerente2 = "02";
            public const string Revision_Gerente3 = "49"; //Reconsideración
            public const string Revision_SubGerente = "11";
            public const string Revision_SubGerente2 = "03";
            public const string Revision_SubGerente3 = "50"; //Reconsideración
            public const string Asignacion_Calificador = "09";
            public const string Asignacion_Calificador2 = "08";
            public const string Dictamen_Asignacon = "40";
            public const string Dictamen_Realizar = "41";
            public const string Dictamen_Realizar2 = "53";  //Reconsideración
            public const string Dictamen_Revision = "43";
            public const string Dictamen_RevisionGerente2 = "55";  //Reconsideración
            public const string Entregar_Tramite = "23";
            public const string Fin_Tramite = "29";
            public const string Generar_Expediente = "22";
            public const string Revision_DGHyP = "14";
            public const string Revision_DGHyP2 = "15"; //Reconsideración
            public const string Revision_DGHyP3 = "59"; //Reconsideración
            public const string Revision_Firma_Disposicion = "27";
            public const string Revision_Firma_Disposicion2 = "32";
            public const string Solicitud_Habilitacion = "06";
            public const string Verificacion_AVH = "31";
            public const string Visado = "47";
            public const string Revision_Pagos = "21";
            public const string Revision_DGHyP_Caducidad = "17";
            public const string Primer_Gestion_Documental = "62"; //Reconsideración
            public const string Gestion_Documental = "58";//Reconsideración


        }

        public enum ENG_Tipos_Tareas_New
        {
            Correccion_Solicitud = 1,
            Asignacion_Calificador = 2,
            Calificar = 3,
            Dictamen_Asignacon = 4,
            Dictamen_Realizar = 5,
            Dictamen_Revision = 6,
            Entregar_Tramite = 7,
            Enviar_a_DGFyC = 8,
            Fin_Tramite = 9,
            Generar_Expediente = 10,
            Informar_Documento_SADE = 11,
            Revision_DGHyP = 12,
            Revision_Firma_Disposicion = 13,
            Revision_Gerente = 14,
            Revision_SubGerente = 15,
            Solicitud_Habilitacion = 16,
            Verificacion_AVH = 17,
            Visado = 18,
            Generacion_Boleta = 19,
            Revision_Pagos = 20,
            Primer_Calificar = 24,
            Dictamen_Revision_Gerente = 30,
            Dictamen_Revision_SubGerente = 29,
            Dictamen_GEDO = 31,
            Revision_Gerente_1er = 25,
            Revision_SubGerente_1er = 26,
            Revision_Gerente_2 = 27,
            Revision_SubGerente_2 = 28
        }
        public struct ENG_Tipos_Tareas_Transf
        {
            public const string Calificar = "10";
            public const string Calificar2 = "01";
            public const string Correccion_Solicitud = "25";
            public const string Enviar_a_DGFyC = "35";
            public const string Revision_Gerente = "12";
            public const string Revision_Gerente2 = "02";
            public const string Revision_SubGerente = "11";
            public const string Revision_SubGerente2 = "03";
            public const string Asignacion_Calificador = "09";
            public const string Asignacion_Calificador2 = "08";
            public const string Dictamen_Asignacon = "40";
            public const string Dictamen_Realizar = "41";
            public const string Dictamen_Revision = "43";
            public const string Entregar_Tramite = "23";
            public const string Fin_Tramite = "29";
            public const string Generar_Expediente = "22";
            public const string Revision_DGHyP = "14";
            public const string Revision_DGHyP2 = "15";
            public const string Revision_Firma_Disposicion = "27";
            public const string Revision_Firma_Disposicion2 = "32";
            public const string Solicitud_Habilitacion = "06";
            public const string Verificacion_AVH = "31"; //Revision_Gerente2 en transferencia
            public const string Visado = "47";
            public const string Revision_Gerente_CP = "33";
            public const string Control_Informe = "21";
        }

        public enum ENG_Tareas
        {
            // tareas para tramites simples sin lanos
            SSP_Encomienda_Digital = 1,
            SSP_Certificacion_Encomienda = 2,
            SSP_Minuta_Acta_Notarial = 3,
            SSP_Certificado_Aptitud_Ambiental = 4,
            SSP_Solicitud_Habilitacion = 6,
            SSP_Revisión_Pagos_APRA = 8,
            SSP_Asignar_Calificador = 9,
            SSP_Calificar = 10,
            SSP_Revision_SubGerente = 11,
            SSP_Revision_Gerente = 12,
            SSP_Revision_Director = 13,
            SSP_Revision_DGHP = 14,
            SSP_Calificacion_Tecnica_Legal = 15,
            SSP_Revision_Tecnica_Legal = 16,
            SSP_Asignar_Inspector = 18,
            SSP_Resultado_Inspector = 19,
            SSP_Validar_Zonificacion = 20,
            SSP_Revision_Pagos = 21,
            SSP_Generar_Expediente = 22,
            SSP_Entregar_Tramite = 23,
            SSP_Enviar_PVH = 24,
            SSP_Correccion_Solicitud = 25,
            SSP_Generacion_Boleta = 26,
            SSP_Revision_Firma_Disposicion = 27,
            SSP_Aprobados = 28,
            SSP_Fin_Tramite = 134150,
            //Nuevo
            SSP_Solicitud_Habilitacion_Nuevo = 300,
            SSP_Asignar_Calificador_SubGerente_Nuevo = 301,
            SSP_Asignar_Calificador_Gerente_Nuevo = 302,
            SSP_Calificar_Nuevo = 303,
            SSP_Revision_SubGerente_Nuevo = 304,
            SSP_Revision_Gerente_Nuevo = 305,
            SSP_Revision_DGHP_Nuevo = 306,
            SSP_Generar_Expediente_Nuevo = 307,
            SSP_Revision_Firma_Disposicion_Nuevo = 308,
            SSP_Enviar_DGFC_Nuevo = 309,
            SSP_Fin_Tramite_Nuevo = 310,
            SSP_Correccion_Solicitud_Nuevo = 311,
            SSP_Revision_DGHP_2_Nuevo = 313,
            SSP_Revision_Firma_Disposicion_2_Nuevo = 314,
            //version 3
            SSP_Solicitud_Habilitacion_v3 = 700,
            SSP_Generar_Expediente_v3 = 701,
            SSP_Entrega_Tramite_v3 = 703,
            SSP_Fin_Tramite_v3 = 704,

            // tareas para tramites simples con planos
            SCP_Asignar_Calificador = 34,
            SCP_Calificar = 35,
            SCP_Revision_SubGerente = 36,
            SCP_Revision_Gerente = 37,
            SCP_Revision_Director = 38,
            SCP_Revision_DGHP = 39,
            SCP_Calificacion_Tecnica_Legal = 40,
            SCP_Revision_Tecnica_Legal = 41,
            SCP_Asignar_Inspector = 42,
            SCP_Resultado_Inspector = 43,
            SCP_Validar_Zonificacion = 44,
            SCP_Revision_Pagos = 45,
            SCP_Generar_Expediente = 46,
            SCP_Entregar_Tramite = 47,
            SCP_Enviar_PVH = 48,
            SCP_Correccion_Solicitud = 49,
            SCP_Generacion_Boleta = 50,
            SCP_Revision_Firma_Disposicion = 51,
            SCP_Aprobados = 72,
            SCP_Fin_Tramite = 53,
            //Nuevo
            SCP_Solicitud_Habilitacion_Nuevo = 400,
            SCP_Asignar_Calificador_SubGerente_Nuevo = 401,
            SCP_Asignar_Calificador_Gerente_Nuevo = 402,
            SCP_Calificar_Nuevo = 403,
            SCP_Revision_SubGerente_Nuevo = 404,
            SCP_Revision_Gerente_Nuevo = 405,
            SCP_Revision_DGHP_Nuevo = 406,
            SCP_Generar_Expediente_Nuevo = 407,
            SCP_Revision_Firma_Disposicion_Nuevo = 408,
            SCP_Enviar_DGFC_Nuevo = 409,
            SCP_Fin_Tramite_Nuevo = 410,
            SCP_Correccion_Solicitud_Nuevo = 411,
            SCP_Revision_DGHP_2_Nuevo = 413,
            SCP_Revision_Firma_Disposicion_2_Nuevo = 414,

            // tareas para tramites especiales
            ESP_Asignar_Calificador = 101,
            ESP_Calificar_1 = 102,
            ESP_Verificacion_AVH = 103,
            ESP_Revision_SubGerente = 104,
            ESP_Revision_Gerente_1 = 105,
            ESP_Dictamen_Asignar_Profesional = 106,
            ESP_Dictamen_Revisar_Tramite = 107,
            ESP_Dictamen_Revision_SubGerente = 108,
            ESP_Dictamen_Revision_Gerente = 109,
            ESP_Dictamen_GEDO = 110,
            ESP_Generacion_Boleta = 111,
            ESP_Revision_Pagos = 112,
            ESP_Generar_Expediente = 113,
            ESP_Revision_DGHP = 114,
            ESP_Revision_Firma_Disposicion = 115,
            ESP_Aprobados = 116,
            ESP_Entregar_Tramite = 117,
            ESP_Rechazado_SADE = 118,
            ESP_Fin_Tramite = 119,
            ESP_Correccion = 120,
            ESP_Calificar_2 = 121,
            ESP_Revision_Gerente_2 = 122,

            //nuevo
            ESP_Asignar_Calificador_Nuevo = 501,
            ESP_Calificar_1_Nuevo = 502,
            ESP_Revision_SubGerente_1_Nuevo = 503,
            ESP_Revision_Gerente_1_Nuevo = 504,
            ESP_Generar_Ticket_Lisa_Nuevo = 505,
            ESP_Obtener_Ticket_Lisa_Nuevo = 506,
            ESP_Calificar_2_Nuevo = 507,
            ESP_Revision_SubGerente_2_Nuevo = 508,
            ESP_Revision_Gerente_2_Nuevo = 509,
            ESP_Dictamen_Asignar_Profesional_Nuevo = 510,
            ESP_Dictamen_Realizar_Nuevo = 511,
            ESP_Dictamen_Revision_Nuevo = 512,
            ESP_Revision_DGHP_Nuevo = 513,
            ESP_Generar_Expediente_Nuevo = 514,
            ESP_Revision_Firma_Disposicion_Nuevo = 515,
            ESP_Enviar_DGFC_Nuevo = 516,
            ESP_Fin_Tramite_Nuevo = 517,
            ESP_Correccion_Nuevo = 518,
            ESP_Verificacion_AVH_Nuevo = 519,
            ESP_Revision_DGHP_2_Nuevo = 521,
            ESP_Revision_Firma_Disposicion_2_Nuevo = 522,

            // tareas para tramites transferencia
            TR_Correccion_Solicitud = 60,
            TR_Asignar_Calificador = 61,
            TR_Calificar = 62,
            TR_Revision_SubGerente = 63,
            TR_Revision_Gerente_1 = 64,
            TR_Revision_Gerente_2 = 86,
            TR_Dictamen_Asignar_Profesional = 65,
            TR_Dictamen_Revisar_Tramite = 80,
            TR_Dictamen_Revision_SubGerente = 81,
            TR_Dictamen_Revision_Gerente = 82,
            TR_Dictamen_GEDO = 83,
            TR_Generacion_Boleta = 84,
            TR_Revision_Pagos = 85,
            TR_Generar_Expediente = 66,
            TR_Revision_DGHP = 67,
            TR_Revision_Firma_Disposicion = 68,
            TR_Aprobados = 73,
            TR_Entregar_Tramite = 69,
            TR_Fin_Tramite = 70,
            TR_Control_e_Informe = 134216,

            //Transferencia nuevo circuito
            TR_Nueva_Generar_Expediente = 134215,
            TR_Nueva_Calificar = 134219,
            TRM_Revision_Gerente = 134220,
            TRM_Revision_SubGerente = 134221,
            TRM_Calificar = 134219,
            TRM_Revision_DGHP = 134228,
            TRM_Control_Informe = 134216,
            TRM_Asignar_Calificador = 134218,
            TRM_Correccion_Solicitud = 134222,
            TRM_Dictamen_Asignar_Profesional = 134225,
            TRM_Fin_De_Tramite = 134231,
            TRM_Entrega_de_Tramite = 134230,

            // tareas para tramites consulta al padron 
            CP_Solicitud = 54,
            CP_Carga_Informacion = 55,
            CP_Revision_SubGerente = 74,
            CP_Generar_Expediente = 56,
            CP_Fin_Tramite = 57,
            CP_Correccion_Solicitud = 134151,

            // tareas para tramites esparcimiento
            ESPAR_Asignar_Calificador = 201,
            ESPAR_Calificar_1 = 202,
            ESPAR_Verificacion_AVH = 203,
            ESPAR_Calificar_2 = 204,
            ESPAR_Revision_SubGerente = 205,
            ESPAR_Revision_Gerente_1 = 206,
            ESPAR_Dictamen_Asignar_Profesional = 207,
            ESPAR_Dictamen_Revisar_Tramite = 208,
            ESPAR_Dictamen_Revision_SubGerente = 209,
            ESPAR_Dictamen_Revision_Gerente = 210,
            ESPAR_Dictamen_GEDO = 211,
            ESPAR_Revision_Gerente_2 = 212,
            ESPAR_Generacion_Boleta = 213,
            ESPAR_Revision_Pagos = 214,
            ESPAR_Generar_Expediente = 215,
            ESPAR_Revision_DGHP = 216,
            ESPAR_Revision_Firma_Disposicion = 217,
            ESPAR_Aprobados = 218,
            ESPAR_Entregar_Tramite = 219,
            ESPAR_Rechazado_SADE = 220,
            ESPAR_Fin_Tramite = 221,
            ESPAR_Correccion = 222,
            //nuevo
            ESPAR_Asignar_Calificador_Nuevo = 601,
            ESPAR_Calificar_1_Nuevo = 602,
            ESPAR_Revision_SubGerente_1_Nuevo = 603,
            ESPAR_Revision_Gerente_1_Nuevo = 604,
            ESPAR_Generar_Ticket_Lisa_Nuevo = 605,
            ESPAR_Obtener_Ticket_Lisa_Nuevo = 606,
            ESPAR_Calificar_2_Nuevo = 607,
            ESPAR_Revision_SubGerente_2_Nuevo = 608,
            ESPAR_Revision_Gerente_2_Nuevo = 609,
            ESPAR_Dictamen_Asignar_Profesional_Nuevo = 610,
            ESPAR_Dictamen_Realizar_Nuevo = 611,
            ESPAR_Dictamen_Revision_Nuevo = 612,
            ESPAR_Revision_DGHP_Nuevo = 613,
            ESPAR_Generar_Expediente_Nuevo = 614,            
            ESPAR_Revision_Firma_Disposicion_Nuevo = 615,
            ESPAR_Enviar_DGFC_Nuevo = 616,
            ESPAR_Fin_Tramite_Nuevo = 617,
            ESPAR_Correccion_Nuevo = 618,
            ESPAR_Verificacion_AVH_Nuevo = 619,
            ESPAR_Revision_DGHP_2_Nuevo = 621,
            ESPAR_Revision_Firma_Disposicion_2_Nuevo = 622,

            //Escuela
            //ESCU_IP_Generar_Expediente = 801,
            ESCU_IP_Asignar_Calificador = 802,
            ESCU_IP_Calificar_1 = 803,
            ESCU_IP_Revision_SubGerente_1 = 804,
            ESCU_IP_Revision_Gerente_1 = 805,
            ESCU_IP_Verificacion_AVH = 806,
            ESCU_IP_Calificar_2 = 807,
            ESCU_IP_Revision_SubGerente_2 = 808,
            ESCU_IP_Revision_Gerente_2 = 809,
            ESCU_IP_Dictamen_Asignar_Profesional = 810,
            ESCU_IP_Dictamen_Realizar = 811,
            ESCU_IP_Dictamen_Revision = 812,
            ESCU_IP_Revision_DGHP = 813,
            ESCU_IP_Revision_Firma_Disposicion = 814,
            ESCU_IP_Entregar_Tramite = 815,
            ESCU_IP_Enviar_DGFC = 816,
            ESCU_IP_Correccion = 817,
            ESCU_IP_Fin_Tramite = 818,
            ESCU_IP_Visado = 819,
            ESCU_IP_Informar_Dpcimento_SADE = 820,
            ESCU_IP_Verificacion_IFCI = 101665,
            //Escuela
            ESCU_HP_Generar_Expediente = 901,
            ESCU_HP_Asignar_Calificador = 902,
            ESCU_HP_Calificar_1 = 903,
            ESCU_HP_Revision_SubGerente_1 = 904,
            ESCU_HP_Revision_Gerente_1 = 905,
            ESCU_HP_Verificacion_AVH = 906,
            ESCU_HP_Calificar_2 = 907,
            ESCU_HP_Revision_SubGerente_2 = 908,
            ESCU_HP_Revision_Gerente_2 = 909,
            ESCU_HP_Dictamen_Asignar_Profesional = 910,
            ESCU_HP_Dictamen_Realizar = 911,
            ESCU_HP_Dictamen_Revision = 912,
            ESCU_HP_Revision_DGHP = 913,
            ESCU_HP_Revision_Firma_Disposicion = 914,
            ESCU_HP_Entregar_Tramite = 915,
            ESCU_HP_Enviar_DGFC = 916,
            ESCU_HP_Correccion = 917,
            ESCU_HP_Fin_Tramite = 918,
            ESCU_HP_Visado = 919,
            ESCU_HP_Informar_Dpcimento_SADE = 920,
            ESCU_HP_Verificacion_IFCI = 101765,

            ESCU_SCP_Generar_Expediente_ESCU_HSCPES = 801, //Habilitaciones Simples Con Planos Escuelas Seguras
            ESCU_SCP_Generar_Expediente_ESCU_HEHP = 901, //Habilitaciones Escuela - Habilitación Previa
            ESCU_SCP_Generar_Expediente_ASCPES = 1173, //Ampliaciones Simples Con Planos Escuelas Seguras
            ESCU_SCP_Generar_Expediente_AHP = 1191, //Ampliaciones Escuela - Habilitación Previa
            ESCU_SCP_Generar_Expediente_RUSCPES = 1280, //Redistribuciones de Uso Simples Con Planos Escuelas Seguras
            ESCU_SCP_Generar_Expediente_RUHP = 1298, //Redistribuciones de Uso Escuela - Habilitación Previa
            ESCU_SCP_Generar_Expediente_ASCPESV2 = 134429, //Ampliaciones Simples Con Planos Escuelas Seguras v2
            ESCU_SCP_Revision_Gerente_1 = 1177, //Ampliaciones Simples Con Planos Escuelas Seguras 
            ESCU_SCP_Revision_Gerente_2 = 1181, //Ampliaciones Simples Con Planos Escuelas Seguras 
            ESCU_SCP_Redistribucion_Revision_Gerente_1 = 1284, //Ampliaciones Simples Con Planos Escuelas Seguras 
            ESCU_SCP_Redistribucion_Revision_Gerente_2 = 1288, //Ampliaciones Simples Con Planos Escuelas Seguras 


            //HP 
            HP_Calificar = 607,

            //SCP3 - Habilitaciones Simples con Planos Recreación SPC-R
            SCP3_Asignar_Calificador_SubGerente = 1001,
            SCP3_Asignar_Calificador_Gerente = 1002,
            SCP3_Revision_DGHP = 1006,

            //SCP4 - Habilitaciones Simples con Planos Salud y Educación SCP-SE
            SCP4_Asignar_Calificador_SubGerente = 1101,
            SCP4_Asignar_Calificador_Gerente = 1102,

            //SCP5 - Habilitaciones Simples Con Planos Escuelas Seguras SCP-ESCU
            SCP5_Asignar_Calificador = 802,

            SCP3_Calificar = 1003,
            SCP4_Calificar = 1103,
            SCP5_Calificar = 803,
            CP_Control_Informe = 55,
            SCP4_Revision_DGHP = 1106,

            //SSP_AsignacionCalificadorGerente = 1117,
            //Redistribuciones de Uso Escuela - Habilitación Previa - ampliaciones y redistribuciones
            ESCU_HP_Asignacion_del_Calificador = 1192,
            ESCU_HP_Redist_Asignacion_del_Calificador = 1299,

            //Ampliaciones Especiales - Habilitación Previa v2 - ampliaciones y redistribuciones
            ESPAR2_Asignacion_del_Calificador = 1146,
            ESPAR2_Redist_Asignacion_del_Calificador = 1257,
            ESPAR2_Amp_Generar_Expediente_HPV2 = 1270,
            ESPAR2_Redist_Generar_Expediente_HPV2 = 1159,
            ESPAR2_Correccion_Solicitud = 1163,

            //Redistribuciones de Uso Simples con Planos Comercios, Industrias y Servicios - ampliaciones y redistribuciones
            SCP2_Calificar = 1133,
            SCP2_Asignacion_al_Calificador_Subgerente = 1131,
            SCP2_Asignacion_al_Calificador_Gerente = 1132,
            SCP2_Generar_Expediente = 1137,
            SCP2_Redist_Generar_Expediente = 1248,
            SCP2_Redist_Asignacion_al_Calificador_Subgerente = 1242,
            SCP2_Redist_Asignacion_al_Calificador_Gerente = 1243,

            //Ampliaciones Simples con Planos Recreación - ampliaciones y redistribuciones
            SCP3_Asignacion_al_Calificador_Subgerente = 1212,
            SCP3_Asignacion_al_Calificador_Gerente = 1213,
            SCP3_Redist_Asignacion_al_Calificador_Subgerente = 1319,
            SCP3_Redist_Asignacion_al_Calificador_Gerente = 1320,
            SCP3_Redist_Generar_Expediente = 1325,
            SCP3_Amp_Generar_Expediente = 1218,
            SCP3_Generar_Expediente = 1007,

            //Ampliaciones Simples con Planos Salud y Educación - ampliaciones y redistribuciones
            SCP4_Asignacion_al_Calificador_Subgerente = 1227,
            SCP4_Asignacion_al_Calificador_Gerente = 1228,
            SCP4_Redist_Asignacion_al_Calificador_Subgerente = 1334,
            SCP4_Redist_Asignacion_al_Calificador_Gerente = 1335,
            SCP4_Redist_Generar_Expediente = 1340,
            SCP4_Amp_Generar_Expediente = 1233,
            SCP4_Generar_Expediente = 1107,

            //Ampliaciones Simples Con Planos Escuelas Seguras - ampliaciones y redistribuciones
            SCP5_Asignacion_del_Calificador = 1174,
            SCP5_Generar_Expediente = 801,
            SCP5_Redist_Asignacion_del_Calificador = 1281,

            //Ampliaciones Simples v2 - ampliaciones y redistribuciones
            SSP2_Asignacion_al_Calificador_Subgerente = 1116,
            SSP2_Asignacion_al_Calificador_Gerente = 1117,
            SSP2_Generar_Expediente = 1122
        }

        public enum ENG_Circuitos
        {
            SSP = 1,
            SCP = 2,
            ESPECIAL = 3,
            CP = 4,
            TRANSF = 5,
            ESPAR = 6,
            SSP2 = 11,
            SCP2 = 12,
            ESPECIAL2 = 13,
            ESPAR2 = 14,
            SSP3 = 15,
            SCP5 = 16,
            ESCU_HP = 17,
            SCP3 = 18,
            SCP4 = 19,
            AMP_SSP2 = 31,
            AMP_SCP2 = 32,
            AMP_ESPAR2 = 34,
            AMP_SSP3 = 35,
            AMP_SCP5 = 36,
            AMP_ESCU_HP = 37,
            AMP_SCP3 = 38,
            AMP_SCP4 = 39,
            RU_SCP2 = 52,
            RU_ESPAR2 = 54,
            RU_SCP5 = 56,
            RU_ESCU_HP = 57,
            RU_SCP3 = 58,
            RU_SCP4 = 59,
            TRANSF_NUEVO = 7
        }

        public enum ENG_Grupos_Circuitos
        {
            SSP = 1,
            SSPA = 2,
            SCPCIS = 3,
            SCPR = 4,
            SCPSE = 5,
            SCPESCU = 6,
            HP = 7,
            HPESCU = 8,
            TRANSF = 9
        }

        public enum ENG_ResultadoTarea
        {
            Sin_establecer = 0,
            Solicitud_Confirmada = 10,
            Solicitud_Anulada_ = 11,
            Solicitud_Vencida = 12,
            Pago_Realizado = 13,
            Asignación_Realizada = 14,
            Revisar_Zonificacion = 16,
            Pedir_Revision_tecnica_y_legal = 17,
            Pedir_Inspeccion = 18,
            Aprobado = 19,
            Calificar_Pedir_Rectificacion = 20,
            Devolver_al_Calificador = 22,
            Subgerente_Estoy_de_Acuerdo_con_el_Calificador = 23,
            Subgerente_Devolver_al_Calificador = 24,
            Gerente_Estoy_de_Acuerdo_con_el_Calificador = 25,
            Gerente_Devolver_al_Calificador = 26,
            //Estoy_de_Acuerdo_con_el_Calificador = 27,
            //Devolver_al_Calificador= 28,
            //Estoy_de_Acuerdo_con_el_Calificador= 29, 
            Asignar_Calificador_Tecnico_y_Legal = 30,
            Revision_Tecnica_y_Legal_Realizada = 31,
            Asignar_Inspector = 32,
            Inspección_realizada = 33,
            Zonificacion_realizada = 34,
            Pagos_Verificados = 35,
            Expediente_Generado = 36,
            Trámite_Entregado = 37,
            Enviado_PVH = 38,
            Enviar_al_Gestor = 39,
            Boleta_Generada = 40,
            Disposicion_Firmada = 41,
            Revision_Realizada = 43,
            Rechazado = 44,
            Realizado = 55,
            Enviar_al_Gerente = 58,
            Requiere_Rechazo = 60,
            Ratifica_calificacion = 61,
            Retifica = 65,
            No_Retifica = 66,
            Estoy_de_acuerdo_con_el_profesional = 67,
            Devolver_al_profesional = 68,
            Observacion = 77,
            Desestima = 78,
            Rechazo_No_es_extremporaneo = 79,
            Rechazo_In_Limine = 84,
            Devolver_a_Consulta_Padron = 85,
            Enviar_a_AVH = 86,
            Devuelve = 87,
            Devolver_a_DGHyP = 88,
            Requiere_Revisión_Procuración = 89,
            Resuelve_Procuración_Aprueba = 90,
            Resuelve_Procuración_Rechaza = 91,
            Devolver_a_SGO_Gestión_Documental = 92,
            Requiere_Revisión_Tecnica = 93,

            Aprobado_Reconsideracion = 2219,
            Observado_Reconsideracion = 2277,
            Rechazado_Reconsideracion = 2244,
            
        }
        public enum CAA_Estados
        {
            Incompleto = 0,
            Completo = 1,
            Pendiente_Ingreso = 2,
            Ingresado = 3,
            Observado = 4,
            Aprobado = 5,
            Rechazado = 6,
            Anulado = 20
        }

        public enum Estados_Jobs
        {
            Fallo = 0,
            Completado = 1,
            Reintentando = 2,
            Cancelado = 3,
            En_Progreso = 4
        }

        public enum TipoTramiteCertificados
        {
            CAA = 2,
            ActaNotarial = 3,
            Ley257 = 4
        }

        public enum TipoDeTramite
        {
            Consulta = 0,
            Habilitacion = 1,
            Transferencia = 2,//asosa
            Ampliacion_Unificacion = 3,
            RedistribucionDeUso = 4,
            RectificatoriaHabilitacion = 5,
            Consulta_Padron = 6,
            Permiso = 7,
            HabilitacionECIAdecuacion = 3,
            HabilitacionECIHabilitacion = 1,
        }

        public enum TipoDeExpediente
        {
            NoDefinido = 0,
            Simple = 1,
            Especial = 2
        }

        public enum TipoTransmision
        {
            Transmision_Transferencia = 1,
            Transmision_nominacion = 2,
            Transmision_oficio_judicial = 3
        }
        public enum SubtipoDeExpediente
        {
            NoDefinido = 0,
            SinPlanos = 1,
            ConPlanos = 2,
            InspeccionPrevia = 3,
            HabilitacionPrevia = 4
        }

        public enum GruposDeTramite
        {
            HAB = 1,
            CP = 2,
            TR = 3
        }

        public enum Encomienda_Estados
        {
            Incompleta = 0,
            Completa = 1,
            Confirmada = 2,
            Ingresada_al_consejo = 3,
            Aprobada_por_el_consejo = 4,
            Rechazada_por_el_consejo = 5,
            Anulada = 20,
            Vencida = 24
        }

        public enum EE_Procesos
        {
            GeneracionPaquete = 1,
            GeneracionCaratula = 2,
            SubirDocumento = 3,
            DesbloqueoExpediente = 4,
            BloqueoExpediente = 5,
            FirmarDocumento = 6,
            RelacionarDocumento = 7,
            FirmarDocumento_RevisarFirma = 8,
            PdfPlanosAdjuntos = 9,
            PasarExpediente = 10,
            ObtenerCaratula = 11,
            ObtenerDisposicion = 12,
            SubirProvidencia = 13
        }

        public enum SGI_Procesos
        {
            Generacion_Paquete = 1,
            Generacion_Caratula = 2,
            Subir_Documento = 3,
            Desbloqueo_Expediente = 4,
            Bloqueo_Expediente = 5,
            Generacion_disposicion_firma = 6,
            Relacionar_Documento = 7,
            Revisìon_Firma_Dispocision = 8,
            Planos = 9,
            Pase_expediente = 10,
            Obtener_caratula_SADE = 11,
            Obtener_Disposicion = 12,
            Subir_Providencia = 13
        }

        public enum SGI_Procesos_EE
        {
            GEN_PAQUETE = 1,
            GEN_CARATULA = 2,
            SUBIR_DOCUMENTO = 3,
            DESBLOQUEO_EXPEDIENTE = 4,
            BLOQUEO_EXPEDIENTE = 5,
            GEN_TAREA_A_LA_FIRMA = 6,
            RELACIONAR_DOCUMENTO = 7,
            REVISION_DE_FIRMA = 8,
            SUBIR_PLANO = 9,
            GEN_PASE = 10,
            GET_CARATULA = 11,
            GET_DISPOSICION = 12,
            SUBIR_PROVIDENCIA = 13,
            SUBIR_CERTIFICADO = 14,
            SUBIR_OBSERVACIONES = 15,
            RELACIONAR_EXPEDIENTE = 16,
            GET_DOCUMENTO = 17
        }

        public enum Tipo_Providencia
        {
            Calificador_a_SubGerente = 0,
            SubGerente_a_Gerente = 1,
            Gerente_a_DGHyP = 2,
            Gerente_a_AVH = 3,
            Gerente_a_DGFYC = 4
        }

        public enum Pago_EstadoPago
        {
            SinPagar = 0,
            Pagado = 1,
            Vencido = 2
        }

        public enum Pago_MedioPago
        {
            BoletaUnica = 0,
            PagoElectronico = 1
        }

        public enum Solicitud_Estados
        {
            Incompleto = 0,
            Completo = 1,
            Pendiente_de_Ingreso = 2,
            Ingresado = 3,
            Caratulacion = 9,
            Anulado = 20,
            Vencida = 24,
            Visado = 25,
            Autorizado = 26,
            Observado = 27,
            En_trámite = 28,
            Aprobada = 29,
            Rechazada = 30,
            En_validación_PVH = 31,
            Observado_PVH = 32,
            Oblea_aprobada = 33,
            Revocada = 34,
            Pendiente_de_pago = 35,
            Suspendida = 38,
            Baja = 40,
            Caduco = 41,
            Datos_Confirmados = 39,
            RevCaducidad = 42,
            BajaAdm = 43,
            RevRechazo = 45
        }



        public enum TiposDeDocumentosRequeridos
        {
            Sin_Especificar = 0,
            Otros = 15,
            Caratula = 29,
            Providencia = 34,
            Plancheta = 41,
            InformeTipoI = 45,
            InformeTipoII = 46,
            Informe_AVH = 50,
            Comparando = 75,
            Calificacion_Tecnica_y_Legal = 81,
            Informe_Reconsideracion = 100,
            Providencia_GestionDocumental = 101,
            Plano_Visado = 106,
            Plano_Habilitacion = 65,
            Plano_Ampliacion = 108,
            Plano_Redistribucion_Uso = 109,
            Informe_IFCI = 118
        }

        public enum TiposDePlanos
        {
            Plano_Habilitacion = 1,
            Plano_Contra_Incendio = 2,
            Plano_Ventilacion = 3,
            Otro = 4,
            Plano_Mensura = 5,
            Plano_Redistribucion_Uso = 6,
            Plano_Habilitacion_Anterior = 7,
            Plano_Ampliacion = 8
        }

        public enum TiposDeDocumentosSistema
        {
            PLANCHETA_CPADRON = 1,
            ENCOMIENDA_DIGITAL = 2,
            INFORMES_CPADRON = 3,
            CERTIFICADO_CAA = 4,
            DOC_ADJUNTO_CPADRON = 5,
            CARATULA_CPADRON = 6,
            PLANCHETA_TRANSFERENCIA = 7,
            DOC_ADJUNTO_TRANSFERENCIA = 8,
            CARATULA_TRANSFERENCIA = 9,
            ACTUACION_NOTARIAL = 10,
            DISPOSICION_CPADRON = 11,
            DISPOSICION_TRANSFERENCIA = 12,
            OBLEA_SOLICITUD = 13,
            CARATULA_HABILITACION = 14,
            DISPOSICION_HABILITACION = 15,
            BUI_TRANSFERENCIA = 16,
            CERTIFICADO_PRO_TEATRO = 17,
            DOC_ADJUNTO_SSIT = 18,
            DOC_ADJUNTO_ENCOMIENDA = 19,
            SOLICITUD_HABILITACION = 20,
            PLANCHETA_HABILITACION = 21,
            CERTIF_CONSEJO_HABILITACION = 22,
            SOLICITUD_CPADRON = 23,
            CERTIFICADO_HABILITACION = 24,
            PRESENTACION_A_AGREGAR = 25
        }

        public enum CAA_TiposDeDocumentosSistema
        {
            CARATULA_CAA = 5,
            CERTIFICADO_CAA = 4,
            DISPOSICION_CAA = 6,
            DOC_ADJUNTO_CAA = 3,
            RECTIFICATORIA_CAA = 2,
            SOLICITUD_CAA = 1
        }

        public enum TipoDocumentoPersonal
        {
            SE = 0,
            DNI = 1,
            LE = 2,
            LC = 3,
            CI = 4,
            PASAPORTE = 5
        }

        public enum TiposDeUbicacion
        {
            ParcelaComun = 0,
            EstacionDeSubte = 1,
            EstacionDeTren = 2,
            EstacionamientoSubterráneo = 3,
            EstacionDeOmnibus = 4,
            GaleriaComercialSubterranea = 5,
            ObjetoTerritorial = 11
        }

        public enum CPadron_EstadoSolicitud
        {
            Incompleto = 0,
            Completo = 1,
            Confirmado = 2,
            Visado = 3,
            Aprobado = 4,
            Observado = 5,
            Anulado = 20,
            Baja = 40,
            BajaAdm = 43
        }

        public enum NivelesDeAgrupamiento
        {
            General = 0,
            Pagos = 1,
            Caratula = 2
        }

        public enum TipoDocumentacionReq
        {
            DeclaracionJurada = 1,
            DeclaracionJuradaPlanos = 2,
            InspeccionPrevia = 3,
            HabilitacionPrevia = 4
        }

        public static class TipoTramiteDescripcion
        {
            public const string Habilitacion = "Habilitación";
            public const string Transferencia = "Transferencia";
            public const string Ampliacion_Unificacion = "Ampliación / Unificación";
            public const string RedistribucionDeUso = "Redistribución de Uso";
            public const string RectificatoriaHabilitacion = "Rectificatoria de Habilitación";
            public const string HabilitacionECI = "Habilitación ECI";
            public const string AdecuacionECI = "Adecuación ECI";
        }
    }
    public class Functions
    {
        public static int isResultadoDispo(int id_solicitud)
        {
            int ret = 0;

            DGHP_Entities db = new DGHP_Entities();

            var query_ult_corr =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                        where tt_hab.id_solicitud == id_solicitud &&
                        tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Correccion_Solicitud
                        orderby tt.id_tramitetarea descending
                        select new
                        {
                            tt.id_tramitetarea
                        }
                    ).FirstOrDefault();
            int id_tramitetarea_ult_corr = query_ult_corr != null ? query_ult_corr.id_tramitetarea : 0;
            List<int> resultados = new List<int>();
            resultados.Add((int)Constants.ENG_ResultadoTarea.Aprobado);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Requiere_Rechazo);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Rechazado);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Observacion);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Desestima);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Rechazo_No_es_extremporaneo);

            var query_resultado =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Calificar on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado,
                            reconsideracion = 0
                        }

                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_SubGerente on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado,
                            reconsideracion = 0
                        }

                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_Gerente on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado,
                            reconsideracion = 0
                        }

                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_DGHP on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado,
                            reconsideracion = 0
                        }
                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Gestion_Documental on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado,
                            reconsideracion = 1
                        }
                    );

            if (query_resultado.Count() > 0)
            {
                var res = query_resultado.OrderByDescending(x => x.id_tramitetarea).First();
                ret = res.id_resultado;
                if (res.reconsideracion == 1 && res.id_resultado == (int)Constants.ENG_ResultadoTarea.Aprobado)
                    ret = (int)Constants.ENG_ResultadoTarea.Aprobado_Reconsideracion;
                if (res.reconsideracion == 1 && res.id_resultado == (int)Constants.ENG_ResultadoTarea.Observacion)
                    ret = (int)Constants.ENG_ResultadoTarea.Observado_Reconsideracion;
                if (res.reconsideracion == 1 && res.id_resultado == (int)Constants.ENG_ResultadoTarea.Rechazado)
                    ret = (int)Constants.ENG_ResultadoTarea.Rechazado_Reconsideracion;
            }
            db.Dispose();

            return ret;

        }
        public static string ConvertToBase64String(string value)
        {
            byte[] str1Byte = Encoding.ASCII.GetBytes(Convert.ToString(value));
            string base64 = Convert.ToBase64String(str1Byte);
            return base64;
        }

        public static bool isFirmadoPdf(byte[] archivo)
        {
            bool ret = false;
            PdfReader reader = new PdfReader(archivo);
            AcroFields af = reader.AcroFields;
            ret = af.GetSignatureNames().Count > 0;
            reader.Dispose();
            return ret;
        }
        public static void EliminarArchivosDirectorioTemporal()
        {
            //Elimina los archivos con mas de 3 días para mantener el directorio limpio.
            string[] lstArchs = System.IO.Directory.GetFiles(Constants.Path_Temporal);
            foreach (string arch in lstArchs)
            {
                DateTime fechaCreacion = System.IO.File.GetCreationTime(arch);
                if (fechaCreacion < DateTime.Now.AddDays(-3))
                    System.IO.File.Delete(arch);
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            try
            {
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception("Error de exportacion: Intente nuevamente.");
            }
            
        }

        public static void ExportDataSetToExcel(DataSet ds, string destination)
        {
            try
            {
                using (var workbook = SpreadsheetDocument.Create(destination, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();

                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                    foreach (System.Data.DataTable table in ds.Tables)
                    {

                        var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                        var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                        sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                        DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                        string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                        uint sheetId = 1;
                        if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                        {
                            sheetId =
                                sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                        }

                        DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                        sheets.Append(sheet);

                        DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                        List<String> columns = new List<string>();
                        foreach (System.Data.DataColumn column in table.Columns)
                        {
                            columns.Add(column.ColumnName);

                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                            headerRow.AppendChild(cell);
                        }


                        sheetData.AppendChild(headerRow);

                        foreach (System.Data.DataRow dsrow in table.Rows)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                            foreach (String col in columns)
                            {
                                DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                                cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()); //
                                newRow.AppendChild(cell);
                            }

                            sheetData.AppendChild(newRow);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error de exportacion: Intente nuevamente.");
            }            
        }

        public static byte[] StreamToArray(Stream input)
        {
            byte[] buffer = new byte[20 * 1024 * 1024]; // 20 mb
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static Guid GetUserId()
        {
            Guid ret = Guid.Empty;
            try
            {
                ret = (Guid)System.Web.Security.Membership.GetUser().ProviderUserKey;
            }
            catch
            {
                FormsAuthentication.RedirectToLoginPage();
                ret = Guid.Empty;
            }

            return ret;
        }
        public static MembershipUser GetUser()
        {
            return Membership.GetUser();
        }

        public static string GetUserName()
        {
            return Membership.GetUser().UserName;
        }

        public static string GetUsernameSADE(Guid userid)
        {
            string ret = null;

            DGHP_Entities db = new DGHP_Entities();

            ret = (from usu in db.aspnet_Users
                   join app in db.aspnet_Applications on usu.ApplicationId equals app.ApplicationId
                   join profile in db.SGI_Profiles on usu.UserId equals profile.userid
                   where app.ApplicationName == "SGI" && usu.UserId == userid
                   select profile.UserName_SADE).FirstOrDefault();

            db.Dispose();

            return ret;
        }

        public static string GetSectorSADE(Guid userid)
        {
            string ret = null;

            DGHP_Entities db = new DGHP_Entities();

            ret = (from usu in db.aspnet_Users
                   join app in db.aspnet_Applications on usu.ApplicationId equals app.ApplicationId
                   join profile in db.SGI_Profiles on usu.UserId equals profile.userid
                   where app.ApplicationName == "SGI" && usu.UserId == userid
                   select profile.Sector_SADE).FirstOrDefault();

            db.Dispose();

            return ret;
        }

        public static bool EsAmbienteDesa()
        {
            bool esDesa = false;

            if (ConfigurationManager.AppSettings["ambiente"] != null &&
                ConfigurationManager.AppSettings["ambiente"] == "desa")
                esDesa = true;

            return esDesa;

        }

        public static bool EsAmbienteTest()
        {
            bool esDesa = false;

            if (ConfigurationManager.AppSettings["ambiente"] != null &&
                ConfigurationManager.AppSettings["ambiente"] == "test")
                esDesa = true;

            return esDesa;

        }

        public static string Mail_Pruebas
        {
            get
            {
                string ret = "";
                string value = System.Configuration.ConfigurationManager.AppSettings["Mail.Pruebas"];
                if (!string.IsNullOrEmpty(value))
                {
                    ret = value.ToString();
                }

                return ret;
            }

        }

        public static bool EsForzarTarasSade()
        {
            bool esDesa = false;

            if (ConfigurationManager.AppSettings["FORZAR_TAREAS_SADE"] != null &&
                ConfigurationManager.AppSettings["FORZAR_TAREAS_SADE"] == "true")
                esDesa = true;

            return esDesa;

        }
        public static bool CopiarDocumentoDisco()
        {
            bool copiar = false;

            if (ConfigurationManager.AppSettings["Copiar.Documento.Disco"] != null &&
                bool.TryParse(ConfigurationManager.AppSettings["Copiar.Documento.Disco"].ToString(), out copiar))
                copiar = true;

            return copiar;

        }

        public static string GetTipoDeTramiteDesc(int id_tipo_tramite)
        {

            Constants.TipoDeTramite enum_ipo_tramite = (Constants.TipoDeTramite)id_tipo_tramite;

            string ret = "";
            switch (enum_ipo_tramite)
            {
                case Constants.TipoDeTramite.Habilitacion:
                    ret = "Habilitación";
                    break;
                case Constants.TipoDeTramite.Transferencia:
                    ret = "Transferencia";
                    break;
                case Constants.TipoDeTramite.Ampliacion_Unificacion:
                    ret = "Ampliación / Unificación";
                    break;
                case Constants.TipoDeTramite.RedistribucionDeUso:
                    ret = "Redistribución de Uso";
                    break;
                case Constants.TipoDeTramite.RectificatoriaHabilitacion:
                    ret = "Rectificatoria Habilitación";
                    break;
            }
            return ret;

        }

        public static string GetTipoExpedienteDesc(int id_tipoexpediente, int id_subtipoexpediente)
        {
            string ret = "";

            Constants.TipoDeExpediente enum_tipoexpediente = (Constants.TipoDeExpediente)id_tipoexpediente;
            Constants.SubtipoDeExpediente enum_subtipoexpediente = (Constants.SubtipoDeExpediente)id_subtipoexpediente;

            switch (enum_tipoexpediente)
            {
                case Constants.TipoDeExpediente.Simple:
                    ret = "Simple";
                    break;
                case Constants.TipoDeExpediente.Especial:
                    ret = "Especial";
                    break;
                case Constants.TipoDeExpediente.NoDefinido:
                    ret = "Indeterminado";
                    break;
            }

            if (enum_subtipoexpediente != Constants.SubtipoDeExpediente.NoDefinido)
            {
                switch (enum_subtipoexpediente)
                {
                    case Constants.SubtipoDeExpediente.HabilitacionPrevia:
                        ret += " Habilitación Previa";
                        break;
                    case Constants.SubtipoDeExpediente.InspeccionPrevia:
                        ret += " Inspección Previa";
                        break;
                    case Constants.SubtipoDeExpediente.ConPlanos:
                        ret += " (con planos)";
                        break;
                    case Constants.SubtipoDeExpediente.SinPlanos:
                        ret += " (sin planos)";
                        break;
                }

            }

            return ret;

        }

        public static byte[] GetBytesFromUrl(string url)
        {
            byte[] bytes;
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            req.Timeout = 30000;
            System.Net.WebResponse resp = req.GetResponse();


            System.IO.Stream stream = resp.GetResponseStream();

            using (System.IO.BinaryReader br = new System.IO.BinaryReader(stream))
            {
                bytes = br.ReadBytes(1048576);
                br.Close();
            }
            resp.Close();
            return bytes;
        }

        public static string GetUrlSSIT()
        {
            string url = (System.Configuration.ConfigurationManager.AppSettings["Url.Website.SSIT"] == null) ? "http://www.dghpsh.agcontrol.gob.ar/SSIT/" : System.Configuration.ConfigurationManager.AppSettings["Url.Website.SSIT"].ToString();
            return url;
        }
        public static SqlException GetSqlException(Exception ex)
        {
            SqlException ret = null;
            Exception ex2 = ex;
            while (!(ex2 is SqlException) && ex2.InnerException != null)
            {
                ex2 = ex2.InnerException;
            }
            if (ex2 is SqlException)
                ret = (SqlException)ex2;

            return ret;
        }
        public static string GetErrorMessage(Exception ex)
        {
            string ret = ex.Message;


            if (ex.InnerException != null)
                ret = ex.InnerException.Message;

            return ret;
        }

        public static string NVL(string value)
        {
            return (string.IsNullOrEmpty(value) ? "" : value);
        }

        public static int NVL(int? value)
        {
            return (value.HasValue ? value.Value : 0);
        }
        public static decimal NVL(decimal? value)
        {
            return (value.HasValue ? value.Value : 0);
        }
        public static bool ComprobarPermisosPagina(string url)
        {
            bool ret = true;
            DGHP_Entities db = new DGHP_Entities();
            List<int> arrPerfilesUsuario = new List<int>();
            MembershipUser user = Membership.GetUser();
            if (user != null)
            {
                Guid userid = (Guid)user.ProviderUserKey;

                if (user.UserName != "digsis")  // digsis siempre tiene permisos
                {
                    var menu = db.SGI_Menues.FirstOrDefault(x => url.Contains(x.pagina_menu.Replace("~", "").Replace(".aspx", "")));

                    arrPerfilesUsuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.ToList().Select(s => s.id_perfil).ToList();

                    if (menu == null || !menu.SGI_Perfiles.Any(s => arrPerfilesUsuario.Contains(s.id_perfil)))
                    {
                        ret = false;
                    }
                }
            }
            db.Dispose();
            return ret;
        }
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static IHtmlString GetParentBreadcrumb()
        {
            string url = HttpContext.Current.Request.Url.AbsolutePath;

            DGHP_Entities db = new DGHP_Entities();

            MembershipUser user = Membership.GetUser();
            if (user != null)
            {
                Guid userid = (Guid)user.ProviderUserKey;
                var menuParent = new SGI_Menues();

                var q = (from mnu in db.SGI_Menues
                         where mnu.visible == true
                         select new { mnu.id_menu, mnu.pagina_menu, mnu.id_menu_padre }).ToList();

                foreach (var result in q)
                {
                    if (url.Replace("~", "").Replace("aspx.", "").IndexOf(result.pagina_menu.Replace("~", "").Replace("aspx.", "")) != -1)
                    {
                        menuParent = db.SGI_Menues.FirstOrDefault(x => x.id_menu == result.id_menu_padre);
                        break;
                    }
                    menuParent = null;
                }

                string appPath = HttpContext.Current.Request.ApplicationPath;
                if (menuParent != null)
                {
                    if (appPath.Equals("/"))
                        appPath = "";
                    return new HtmlString(" <a href='" + appPath + "" + menuParent.pagina_menu.Replace("~", "") + "/" + menuParent.id_menu + "' class='current'>" + menuParent.descripcion_menu + "</a>");

                }

                db.Dispose();
            }

            return new HtmlString("");
        }

        public static string GetUrlFoto(string seccion, string manzana, string parcela)
        {

            string ret = "";
            string SMP = "";
            int tamaManzana = 3;
            int tamaParcela = 3;

            seccion = seccion.Trim();
            manzana = manzana.Trim();
            parcela = parcela.Trim();


            SMP += seccion.PadLeft(2, Convert.ToChar("0"));
            SMP += "-";

            if (manzana.Length > 0)
            {
                if (!Char.IsNumber(manzana, manzana.Length - 1))
                    tamaManzana = 4;
            }

            SMP += manzana.PadLeft(tamaManzana, Convert.ToChar("0"));
            SMP += "-";

            if (parcela.Length > 0)
            {
                if (!Char.IsNumber(parcela, parcela.Length - 1))
                    tamaParcela = 4;
            }

            SMP += parcela.PadLeft(tamaParcela, Convert.ToChar("0"));

            ret = string.Format("http://fotos.usig.buenosaires.gob.ar/getFoto?smp={0}&i=0&h=200&w=250", SMP);

            return ret;

        }

        public static string GetUrlMapa(string seccion, string manzana, string parcela, string Direccion)
        {

            string ret = "";
            string SMP = "";
            int tamaManzana = 3;
            int tamaParcela = 3;

            seccion = seccion.Trim();
            manzana = manzana.Trim();
            parcela = parcela.Trim();
            Direccion = Direccion.Trim();

            SMP += seccion.PadLeft(2, Convert.ToChar("0"));
            SMP += "-";

            if (manzana.Length > 0)
            {
                if (!Char.IsNumber(manzana, manzana.Length - 1))
                    tamaManzana = 4;
            }

            SMP += manzana.PadLeft(tamaManzana, Convert.ToChar("0"));
            SMP += "-";

            if (parcela.Length > 0)
            {
                if (!Char.IsNumber(parcela, parcela.Length - 1))
                    tamaParcela = 4;
            }

            SMP += parcela.PadLeft(tamaParcela, Convert.ToChar("0"));

            ret = string.Format("http://servicios.usig.buenosaires.gob.ar/LocDir/mapa.phtml?dir={0}&desc={0}&w=400&h=300&punto=5&r=200&smp={1}",
                        Direccion, SMP);
            return ret;

        }

        public static string GetUrlCroquis(string seccion, string manzana, string parcela, string Direccion)
        {

            string ret = "";
            string SMP = "";
            int tamaManzana = 3;
            int tamaParcela = 3;
            seccion = seccion.Trim();
            manzana = manzana.Trim();
            parcela = parcela.Trim();
            Direccion = Direccion.Trim();

            SMP += seccion.PadLeft(2, Convert.ToChar("0"));
            SMP += "-";

            if (manzana.Length > 0)
            {
                if (!Char.IsNumber(manzana, manzana.Length - 1))
                    tamaManzana = 4;
            }

            SMP += manzana.PadLeft(tamaManzana, Convert.ToChar("0"));
            SMP += "-";

            if (parcela.Length > 0)
            {
                if (!Char.IsNumber(parcela, parcela.Length - 1))
                    tamaParcela = 4;
            }

            SMP += parcela.PadLeft(tamaParcela, Convert.ToChar("0"));

            ret = string.Format("http://servicios.usig.buenosaires.gob.ar/LocDir/mapa.phtml?dir={0}&w=400&h=300&punto=5&r=50&smp={1}",
                     Direccion, SMP);
            return ret;

        }


        public static void EjecutarScript(UpdatePanel upd, string scriptName)
        {
            ScriptManager.RegisterStartupScript(upd, upd.GetType(),
                "script", scriptName, true);

        }

        public static void EjecutarScript(Page pag, string scriptName)
        {
            ScriptManager.RegisterClientScriptBlock(pag, pag.GetType(),
                   "script", scriptName, true);

        }

        public static int GetTipoDocSistema(string CodTipoDocSistema)
        {
            int ret = 0;
            DGHP_Entities db = new DGHP_Entities();

            var tipodocsis = db.TiposDeDocumentosSistema.FirstOrDefault(x => x.cod_tipodocsis == CodTipoDocSistema);
            if (tipodocsis != null)
                ret = tipodocsis.id_tipdocsis;

            db.Dispose();

            return ret;

        }


        public static int Get_id_solicitud(int id_tramitetarea)
        {
            int ret = 0;
            DGHP_Entities db = new DGHP_Entities();

            var tt = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            if (tt != null)
            {
                if (tt.SGI_Tramites_Tareas_HAB.Count > 0)
                    ret = tt.SGI_Tramites_Tareas_HAB.FirstOrDefault().id_solicitud;
                else if (tt.SGI_Tramites_Tareas_CPADRON.Count > 0)
                    ret = tt.SGI_Tramites_Tareas_CPADRON.FirstOrDefault().id_cpadron;
                else if (tt.SGI_Tramites_Tareas_TRANSF.Count > 0)
                    ret = tt.SGI_Tramites_Tareas_TRANSF.FirstOrDefault().id_solicitud;
            }

            db.Dispose();

            return ret;

        }

        public static int Get_id_grupotramite(int id_tramitetarea)
        {
            int ret = 0;
            DGHP_Entities db = new DGHP_Entities();

            var tt = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            if (tt != null)
            {
                if (tt.SGI_Tramites_Tareas_HAB.Count > 0)
                    ret = (int)Constants.GruposDeTramite.HAB;
                else if (tt.SGI_Tramites_Tareas_CPADRON.Count > 0)
                    ret = (int)Constants.GruposDeTramite.CP;
                else if (tt.SGI_Tramites_Tareas_TRANSF.Count > 0)
                    ret = (int)Constants.GruposDeTramite.TR;
            }

            db.Dispose();

            return ret;

        }

        public static string ConvertToBase64(string valor)
        {
            string result = "";
            byte[] bvalor = System.Text.Encoding.ASCII.GetBytes(valor);
            result = Convert.ToBase64String(bvalor);

            return result;
        }
        public static string GetParametroChar(string CodParam)
        {
            string ret = "";

            DGHP_Entities db = new DGHP_Entities();

            var param = db.Parametros.FirstOrDefault(x => x.cod_param == CodParam);
            if (param != null)
                ret = param.valorchar_param;

            db.Dispose();

            return ret;

        }
        public static decimal GetParametroNum(string CodParam)
        {
            decimal ret = 0;

            DGHP_Entities db = new DGHP_Entities();

            var param = db.Parametros.FirstOrDefault(x => x.cod_param == CodParam);
            if (param != null)
                ret = param.valornum_param.Value;

            db.Dispose();

            return ret;

        }

        public static string GetParametroCharEE(string CodParam)
        {
            string ret = "";

            EE_Entities db = new EE_Entities();

            var param = db.Parametros.FirstOrDefault(x => x.cod_param == CodParam);
            if (param != null)
                ret = param.valorchar_param;

            db.Dispose();

            return ret;

        }
        public static decimal GetParametroNumEE(string CodParam)
        {
            decimal ret = 0;

            EE_Entities db = new EE_Entities();

            var param = db.Parametros.FirstOrDefault(x => x.cod_param == CodParam);
            if (param != null)
                ret = param.valornum_param.Value;

            db.Dispose();

            return ret;

        }
        public static bool isAprobado(int id_solicitud)
        {
            bool ret = false;

            DGHP_Entities db = new DGHP_Entities();
            string[] arrTareasBuscar = new string[] { Constants.ENG_Tipos_Tareas.Solicitud_Habilitacion.ToString() ,
                                                Constants.ENG_Tipos_Tareas.Correccion_Solicitud.ToString()};

            var query_ult_corr_ini =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                        where tt_hab.id_solicitud == id_solicitud &&
                        arrTareasBuscar.Contains(tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2))
                        orderby tt.id_tramitetarea descending
                        select new
                        {
                            tt.id_tramitetarea
                        }
                    ).FirstOrDefault();
            int id_tramitetarea_ult_corr_ini = query_ult_corr_ini != null ? query_ult_corr_ini.id_tramitetarea : 0;
            List<int> resultados = new List<int>();
            resultados.Add((int)Constants.ENG_ResultadoTarea.Aprobado);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Requiere_Rechazo);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Rechazado);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Calificar_Pedir_Rectificacion);
            

            var query_resultado =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Calificar on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }

                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_SubGerente on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }

                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_Gerente on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }

                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_DGHP on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }
                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Gestion_Documental on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }
                    );

            if (query_resultado.Count() > 0)
            {
                int resultado = query_resultado.OrderByDescending(x => x.id_tramitetarea).First().id_resultado;
                if (resultado == (int)Constants.ENG_ResultadoTarea.Aprobado)
                    ret = true;
            }

            db.Dispose();

            return ret;

        }

        public static Constants.TipoResolucionHAB? GetTipoResolucionHAB(int id_solicitud)
        {

            Constants.TipoResolucionHAB? ret = null;

            DGHP_Entities db = new DGHP_Entities();
            string[] arrTareasBuscar = new string[] { Constants.ENG_Tipos_Tareas.Solicitud_Habilitacion.ToString() ,
                                                Constants.ENG_Tipos_Tareas.Correccion_Solicitud.ToString()};

            var query_ult_corr_ini =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                        where tt_hab.id_solicitud == id_solicitud &&
                        arrTareasBuscar.Contains(tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2))
                        orderby tt.id_tramitetarea descending
                        select new
                        {
                            tt.id_tramitetarea
                        }
                    ).FirstOrDefault();
            int id_tramitetarea_ult_corr_ini = query_ult_corr_ini != null ? query_ult_corr_ini.id_tramitetarea : 0;
            List<int> resultados = new List<int>();
            resultados.Add((int)Constants.ENG_ResultadoTarea.Aprobado);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Requiere_Rechazo);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Rechazado);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Calificar_Pedir_Rectificacion);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Observacion);

            var query_resultado =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Calificar on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }

                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_SubGerente on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }

                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_Gerente on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }

                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_DGHP on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }
                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join th in db.SGI_Tarea_Gestion_Documental on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }
                    );

            if (query_resultado.Count() > 0)
            {
                int resultado = query_resultado.OrderByDescending(x => x.id_tramitetarea).First().id_resultado;
                if (resultado == (int)Constants.ENG_ResultadoTarea.Aprobado)
                    ret = Constants.TipoResolucionHAB.Aprobado;
                else if (resultado == (int)Constants.ENG_ResultadoTarea.Requiere_Rechazo || resultado == (int)Constants.ENG_ResultadoTarea.Rechazado)
                    ret = Constants.TipoResolucionHAB.Rechazado;
                else if (resultado == (int)Constants.ENG_ResultadoTarea.Calificar_Pedir_Rectificacion || resultado == (int)Constants.ENG_ResultadoTarea.Observacion) 
                    ret = Constants.TipoResolucionHAB.Observado;
            }

            db.Dispose();

            return ret;

        }


        public static bool IsAprobadoTransf(int id_solicitud)
        {
            bool ret = false;

            DGHP_Entities db = new DGHP_Entities();
            string[] arrTareasBuscar = new string[] { Constants.ENG_Tipos_Tareas.Solicitud_Habilitacion.ToString() ,
                                                Constants.ENG_Tipos_Tareas.Correccion_Solicitud.ToString()};

            var query_ult_corr_ini =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                        where tt_hab.id_solicitud == id_solicitud &&
                        arrTareasBuscar.Contains(tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2))
                        orderby tt.id_tramitetarea descending
                        select new
                        {
                            tt.id_tramitetarea
                        }
                    ).FirstOrDefault();
            int id_tramitetarea_ult_corr_ini = query_ult_corr_ini != null ? query_ult_corr_ini.id_tramitetarea : 0;
            List<int> resultados = new List<int>();
            resultados.Add((int)Constants.ENG_ResultadoTarea.Aprobado);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Requiere_Rechazo);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Rechazado);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Calificar_Pedir_Rectificacion);

            var query_resultado =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_transf in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_transf.id_tramitetarea
                        join th in db.SGI_Tarea_Calificar on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_transf.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }
                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_transf in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_transf.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_SubGerente on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_transf.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }
                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_transf in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_transf.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_Gerente on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_transf.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }
                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_transf in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_transf.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_DGHP on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_transf.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr_ini
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado
                        }
                    );

            if (query_resultado.Count() > 0)
            {
                int resultado = query_resultado.OrderByDescending(x => x.id_tramitetarea).First().id_resultado;
                if (resultado == (int)Constants.ENG_ResultadoTarea.Aprobado)
                    ret = true;
            }

            db.Dispose();

            return ret;

        }

        public static bool caduco(int id_solicitud)
        {
            bool ret = false;

            DGHP_Entities db = new DGHP_Entities();

            //var query_ult_corr =
            //        (
            //            from tt in db.SGI_Tramites_Tareas
            //            join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
            //            join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
            //            where tt_hab.id_solicitud == id_solicitud &&
            //            tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Correccion_Solicitud //&&
            //                                                                                                                                          // DbFunctions.DiffDays(tt.FechaInicio_tramitetarea, tt.FechaCierre_tramitetarea) > 140 
            //            orderby tt.id_tramitetarea descending
            //            select new
            //            {
            //                tt.id_tramitetarea,
            //                tt.FechaInicio_tramitetarea,
            //                tt.FechaCierre_tramitetarea
            //            }
            //        ).FirstOrDefault();

            //var query_caducidad =
            //       (
            //           from tt in db.SGI_Tramites_Tareas
            //           join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
            //           join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
            //           where tt_hab.id_solicitud == id_solicitud &&
            //           tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Revision_DGHyP_Caducidad
            //           orderby tt.id_tramitetarea descending
            //           select new
            //           {
            //               tt.id_tramitetarea,
            //               tt.FechaInicio_tramitetarea,
            //               tt.FechaCierre_tramitetarea
            //           }
            //       ).FirstOrDefault();

            //var query_notificacion = (from ac in db.SSIT_Solicitudes_AvisoCaducidad where ac.id_solicitud == id_solicitud
            //                          orderby ac.CreateDate descending
            //                          select ac).FirstOrDefault();

            //if (query_ult_corr != null && query_notificacion != null)
            //{
            //    //TimeSpan ts = (DateTime)query_ult_corr.FechaCierre_tramitetarea - (DateTime)query_ult_corr.FechaInicio_tramitetarea;
            //    TimeSpan ts = (DateTime)DateTime.Now - (DateTime)query_notificacion.CreateDate;
            //    if (ts.Days > 45 && query_caducidad != null && query_ult_corr.id_tramitetarea < query_caducidad.id_tramitetarea)
            //        ret = true;
            //}

            var sol = db.SSIT_Solicitudes.Where(x => x.id_solicitud == id_solicitud).FirstOrDefault();
            if (sol.id_estado == (int)Constants.Solicitud_Estados.Caduco ||
                sol.id_estado == (int)Constants.Solicitud_Estados.RevCaducidad)
                ret = true;

            db.Dispose();

            return ret;
        }

        public static int DiasHabiles(DateTime desde, DateTime hasta)
        {
            int dias_habiles = 0;
            DGHP_Entities db = new DGHP_Entities();
            var param = (from p in db.Parametros
                         where p.id_param == 1
                         select new
                         {
                             p.id_param,
                             dias = SqlFunctions.DateDiff("dd", desde, hasta).Value - (SqlFunctions.DateDiff("wk", desde, hasta).Value * 2) + 1
                         });
            dias_habiles = param.First().dias;
            if (desde.DayOfWeek == DayOfWeek.Sunday)
                dias_habiles = dias_habiles - 1;
            return dias_habiles;
        }

        public static int isResultadoDispoTransmision(int id_solicitud)
        {
            int ret = 0;

            DGHP_Entities db = new DGHP_Entities();

            var query_ult_corr =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                        join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                        where tt_tr.id_solicitud == id_solicitud &&
                        tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Correccion_Solicitud
                        orderby tt.id_tramitetarea descending
                        select new
                        {
                            tt.id_tramitetarea
                        }
                    ).FirstOrDefault();
            int id_tramitetarea_ult_corr = query_ult_corr != null ? query_ult_corr.id_tramitetarea : 0;
            List<int> resultados = new List<int>();
            resultados.Add((int)Constants.ENG_ResultadoTarea.Aprobado);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Requiere_Rechazo);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Rechazado);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Observacion);
            resultados.Add((int)Constants.ENG_ResultadoTarea.Rechazo_In_Limine);

            var query_resultado =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                        join th in db.SGI_Tarea_Calificar on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_tr.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado,
                            reconsideracion = 0
                        }

                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_SubGerente on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_tr.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado,
                            reconsideracion = 0
                        }

                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_Gerente on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_tr.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado,
                            reconsideracion = 0
                        }

                    ).Union
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                        join th in db.SGI_Tarea_Revision_DGHP on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_tr.id_solicitud == id_solicitud
                        && tt.id_tramitetarea > id_tramitetarea_ult_corr
                        && resultados.Contains(tt.id_resultado)
                        select new
                        {
                            tt.id_tramitetarea,
                            tt.id_resultado,
                            reconsideracion = 0
                        }
                    );
            if (query_resultado.Count() > 0)
            {
                var res = query_resultado.OrderByDescending(x => x.id_tramitetarea).First();
                ret = res.id_resultado;
            }
            db.Dispose();

            return ret;

        }

        public static bool IsSSPA(int id_tramitetarea)
        {
            try
            {
                DGHP_Entities db = new DGHP_Entities();
                //id_circuito == 15 / Habilitaciones Simples Automaticas v3
                //id_circuito == 35 / Ampliaciones Simples Automaticas v3
                var idCircuito = (from tt in db.SGI_Tramites_Tareas
                                  join et in db.ENG_Tareas on tt.id_tarea equals et.id_tarea
                                  where tt.id_tramitetarea == id_tramitetarea && (et.id_circuito == 15 || et.id_circuito == 35)
                                  select new
                                  {  et.id_circuito }).FirstOrDefault();

                if (idCircuito != null)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }



    }
    public class Usuario
    {


        public static List<SGI_Menues> GetMenuUsuario(Guid userId)
        {
            DGHP_Entities db = new DGHP_Entities();

            var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userId).SGI_PerfilesUsuarios.Select(x => x.id_perfil).ToList();

            //var perfiles_usuario1 = db.aspnet_Users.FirstOrDefault(x => x.UserId == userId).SGI_PerfilesUsuarios.ToList();

            var q = (from menu in db.SGI_Menues
                     where menu.SGI_Perfiles.Any(s => perfiles_usuario.Contains(s.id_perfil))
                     select new
                     {
                         menu.id_menu,
                         menu.descripcion_menu,
                         menu.pagina_menu,
                         menu.iconCssClass_menu,
                         menu.nroOrden
                     }
               ).Distinct().OrderBy(x => x.nroOrden);


            List<SGI_Menues> menu_usuario = new List<SGI_Menues>();
            SGI_Menues menu_p;
            foreach (var item in q.ToList())
            {
                menu_p = new SGI_Menues();

                menu_p.id_menu = (int)item.id_menu;
                menu_p.descripcion_menu = (string)item.descripcion_menu;
                menu_p.pagina_menu = (string)item.pagina_menu + "?id_menu=" + menu_p.id_menu;
                menu_p.iconCssClass_menu = (string)item.iconCssClass_menu;
                menu_p.nroOrden = (int)item.nroOrden;

                menu_usuario.Add(menu_p);
            }


            db.Dispose();

            return menu_usuario;

        }

    }



    #region BasePage

    public class BasePage : System.Web.UI.Page
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden

        public BasePage()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }




        public string GenerarCodigoSeguridad()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            char ch;
            for (int i = 0; i < 6; i++)
            {
                if (i % 2 == 0)
                {
                    ch = Convert.ToChar(random.Next(0, 9).ToString());
                }
                else
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                }
                builder.Append(ch);
            }

            return builder.ToString();
        }



        public static string IPtoDomain(string url)
        {

            string ret = url.Replace("10.20.72.31", "www.dghpsh.agcontrol.gob.ar");
            ret = url.Replace("10.20.72.23", "www.dghpsh.agcontrol.gob.ar");
            ret = url.Replace("azufral.agc", "www.dghpsh.agcontrol.gob.ar");

            return ret;


        }

        public void EjecutarScript(UpdatePanel upd, string scriptName)
        {
            ScriptManager.RegisterStartupScript(upd, upd.GetType(),
                "script", scriptName, true);

        }

        public void EjecutarScript(Page pag, string scriptName)
        {
            ScriptManager.RegisterClientScriptBlock(pag, pag.GetType(),
                   "script", scriptName, true);

        }

        public void EjecutarScriptClient(Page pag, string scriptName)
        {
            Page.ClientScript.RegisterStartupScript(pag.GetType(), "script", scriptName, true);

        }

    }

    #endregion

    #region Utiles

    public class WebUtil
    {
        public static bool ValidarDniCuit(string dni, string cuit)
        {
            bool esValido = false;

            string parteMedio = cuit.Split('-')[1];

            dni = string.Format("{0:00000000}", dni);
            parteMedio = string.Format("{0:00000000}", parteMedio);


            if (Convert.ToInt16(dni.Substring(0, 2)) > 90)
            {
                //verificar que los primeros digitos representes millones
                esValido = true;
            }
            else
            {
                esValido = (dni.Equals(parteMedio));
            }

            return esValido;
        }

        public static bool ValidarCuit(string pcuit)
        {
            bool esValido = false;

            if (string.IsNullOrEmpty(pcuit))
            {
                return esValido;
            }

            string nuevoCuit = pcuit.Replace("_", "");

            string[] parts = nuevoCuit.Split('-');

            if (parts.Length != 3)
            {
                return esValido;
            }

            if (parts[1].Length == 7)
            {
                parts[1] = "0" + parts[1];
            }

            var cuitAuxiliar = parts[0] + parts[1] + parts[2];

            if (cuitAuxiliar.Length < 10)
            {
                return esValido; ;
            }

            if (cuitAuxiliar == "00000000000")
            {
                return esValido;
            }

            int sum = 0;
            int dv = 0;
            int i = 0;
            int factor = 2;
            string caracter = "";
            int Modulo11 = 0;
            int Verificador = 0;
            int CodVer = 0;

            for (i = 9; i >= 0; i--)
            {
                caracter = cuitAuxiliar.Substring(i, 1);
                if (factor > 7)
                {
                    factor = 2;
                }
                sum += int.Parse(caracter) * factor;
                factor++;

            }

            dv = sum / 11;
            Modulo11 = sum - (11 * dv);
            Verificador = 11 - Modulo11;
            if (Modulo11 == 0)
            {
                CodVer = 0;
            }
            else if (Verificador == 10)
            {
                CodVer = 9;
            }
            else
            {
                CodVer = Verificador;
            }

            if (CodVer != int.Parse(cuitAuxiliar.Substring(10, 1)))
            {
                return esValido;
            }

            return true;
        }

        public static bool validarEmail(string email)
        {
            bool esValido = false;
            string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (System.Text.RegularExpressions.Regex.IsMatch(email, expresion))
            {
                if (System.Text.RegularExpressions.Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    esValido = true;
                }
            }

            return esValido;
        }

        public static void EstadoControles(ControlCollection controls, bool estado)
        {
            foreach (Control control in controls)
            {

                if (control is TextBox)
                {
                    TextBox txt = (TextBox)control;
                    txt.ReadOnly = !estado;
                }
                else if (control is DropDownList)
                {
                    DropDownList ddl = (DropDownList)control;
                    ddl.Enabled = estado;
                }
                else if (control is RadioButton)
                {
                    RadioButton radio = (RadioButton)control;
                    radio.Enabled = estado;
                }

                if (control.Controls.Count > 0)
                    EstadoControles(control.Controls, estado);

            }


        }

    }

    #endregion

    #region parametros

    public class Parametros
    {

        public static string GetParam_ValorChar(string codigo_param)
        {
            DGHP_Entities db = null;
            string param = "";

            try
            {
                db = new DGHP_Entities();

                param = db.Parametros.Where(x => x.cod_param.Equals(codigo_param)).Select(x => x.valorchar_param).FirstOrDefault();

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }

            return param;
        }

        public static string GetParam_ValorNum(string codigo_param)
        {
            DGHP_Entities db = null;
            decimal? param = null;

            try
            {
                db = new DGHP_Entities();

                param = db.Parametros.Where(x => x.cod_param.Equals(codigo_param)).Select(x => x.valornum_param).FirstOrDefault();

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }

            if (param.HasValue)
                return param.ToString();
            else
                return "";

        }

        public static string GetParamEE_ValorChar(string codigo_param)
        {
            EE_Entities db = null;
            string param = "";
            try
            {
                db = new EE_Entities();
                param = db.Parametros.Where(x => x.cod_param.Equals(codigo_param)).Select(x => x.valorchar_param).FirstOrDefault();
                db.Dispose();
                if (param == null) param = "";
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }

            return param;
        }

        public static string GetParamEE_ValorNum(string codigo_param)
        {
            DGHP_Entities db = null;
            decimal? param = null;

            try
            {
                db = new DGHP_Entities();

                param = db.Parametros.Where(x => x.cod_param.Equals(codigo_param)).Select(x => x.valornum_param).FirstOrDefault();

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                throw ex;
            }

            if (param.HasValue)
                return param.ToString();
            else
                return "";

        }


    }

    #endregion

    #region documentos
    public class Documentos
    {
        public static bool generarDocumentoInicio(int id_solicitud)
        {
            bool generado = false;
            try
            {
                ws_ssit.WSssit ws = new ws_ssit.WSssit();
                ws.Url = Parametros.GetParam_ValorChar("SSIT.Url") + "WSssit.asmx";
                string user = Parametros.GetParam_ValorChar("SSIT.Username.WebService");
                string pass = Parametros.GetParam_ValorChar("SSIT.Password.WebService");
                generado = ws.generarDocInicioTramite(user, pass, id_solicitud);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return generado;
        }

    }
    #endregion

    #region Encuestas
    public class Encuestas
    {
        public static bool enviarEncuesta(int id_solicitud)
        {
            bool generado = false;
            ws_ssit.WSssit ws = new ws_ssit.WSssit();
            ws.Url = Parametros.GetParam_ValorChar("SSIT.Url") + "WSssit.asmx";
            string user = Parametros.GetParam_ValorChar("SSIT.Username.WebService");
            string pass = Parametros.GetParam_ValorChar("SSIT.Password.WebService");
            generado = ws.enviarEncuesta(user, pass, id_solicitud);
            return generado;
        }
    }
    #endregion

    //public class DGHP
    //{

    //    public static List<string> ExecuteSQL(string sql)
    //    {
    //        DGHP_Entities db = new DGHP_Entities();

    //        List<string> filas = db.Database.SqlQuery<string>(sql).ToList();

    //        db.Dispose();

    //        return filas;

    //    }


    //}



}