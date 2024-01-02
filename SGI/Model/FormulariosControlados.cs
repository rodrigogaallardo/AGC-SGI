using ExternalService.Class;
using RestSharp;
using SGI.Webservices.ws_interface_AGC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SGI.Model
{
    public class FormulariosControlados
    {
        public async static Task<string> getFormulario(string nombre_formulario, int id_solicitud)
        {
            ws_ExpedienteElectronico.ws_ExpedienteElectronico serviceEE = new ws_ExpedienteElectronico.ws_ExpedienteElectronico();
            serviceEE.Url = SGI.Parametros.GetParam_ValorChar("SGI.Url.Service.ExpedienteElectronico");
            string username_servicio_EE = SGI.Parametros.GetParam_ValorChar("SGI.UserName.Service.ExpedienteElectronico");
            string pass_servicio_EE = SGI.Parametros.GetParam_ValorChar("SGI.Pwd.Service.ExpedienteElectronico");
            string formulario_json=serviceEE.getConfiguracionFFCC(username_servicio_EE, pass_servicio_EE, nombre_formulario);
            
            //cargo el objeto generico (con todos los datos de los formularios)
            ffcc_DTO ffcc = await cargaGenerica(id_solicitud);
            var lista = SimpleJson.DeserializeObject<List<nodoDTO>>(formulario_json);

            List<nodoDTO_R> lista_a_enviar = new List<nodoDTO_R>();
            nodoDTO_R nt;
            foreach (nodoDTO nodo in lista)
            {
                nt = new nodoDTO_R();
                nt.id_ffcc = nodo.id_ffcc;
                nt.id = nodo.id;
                
                //recupero el dato segun el mapeo
                nt.value = getValue(nodo, ffcc);

                if (!string.IsNullOrEmpty(nt.value))
                    lista_a_enviar.Add(nt);

            }
            string json = SimpleJson.SerializeObject(lista_a_enviar.ToList());
            return json;
        }

        private static string getValue(nodoDTO nodo, ffcc_DTO ffcc)
        {
            string nombre_componente = nodo.nombre_componente;
            string value = "";
            if (nombre_componente == "calle")
                value = ffcc.ubicacion.calle;
            else if (nombre_componente == "altura")
                value = ffcc.ubicacion.altura;
            else if (nombre_componente == "barrio")
                value = ffcc.ubicacion.barrio;
            else if (nombre_componente == "comuna")
                value = ffcc.ubicacion.comuna;
            else if (nombre_componente == "seccion")
                value = ffcc.ubicacion.seccion;
            else if (nombre_componente == "manzana")
                value = ffcc.ubicacion.manzana;
            else if (nombre_componente == "parcela")
                value = ffcc.ubicacion.parcela;
            else if (nombre_componente == "nro_expediente" || nombre_componente == "num_ex"
                    || nombre_componente == "num_ex_obra" || nombre_componente == "num_expediente_obra")
                value = ffcc.expediente.nro_expediente;
            else if (nombre_componente == "tipo_obra")
                value = ffcc.tipo_obra;
            else if (nombre_componente == "destino")
                value = ffcc.destino;
            else if (nombre_componente == "metros_cuadrados" || nombre_componente == "superficie" 
                    || nombre_componente== "superficie_total")
                value = ffcc.expediente.superficie;
            else if (nombre_componente == "tipo_tramite")
                value = ffcc.tipo_tramite;
            else if (nombre_componente == "nombre_1_tit")
                value = ffcc.propietario.nombre;
            else if (nombre_componente == "apellido_1_tit")
                value = ffcc.propietario.apellido;
            else if (nombre_componente == "tipo_docum_tit")
                value = getValueItem(nodo.items, ffcc.propietario.tipo_doc);
            else if (nombre_componente == "num_docum_tit")
                value = ffcc.propietario.nro_doc;
            else if (nombre_componente == "cuit_tit")
                value = ffcc.propietario.cuit;
            else if (nombre_componente == "telefono_tit")
                value = ffcc.propietario.telefono;
            else if (nombre_componente == "email_tit")
                value = ffcc.propietario.mail;
            else if (nombre_componente == "rubro" || nombre_componente== "lista_rubros")
                value = ffcc.expediente.rubros;
            else if (nombre_componente == "ubicacio")
                value = ffcc.expediente.ubicacion;
            else if (nombre_componente == "zona")
                value = ffcc.expediente.zona;
            else if (nombre_componente == "dom_est_calle" || nombre_componente == "dom_caba_calle")
                value = ffcc.ubicacion.calle + " " + ffcc.ubicacion.altura;
            else if (nombre_componente == "dom_est_dpto")
                value = ffcc.ubicacion.depto;
            else if (nombre_componente == "dom_est_piso")
                value = ffcc.ubicacion.piso;
            else if (nombre_componente == "dom_est_cp")
                value = ffcc.ubicacion.codigo_postal;
            else if (nombre_componente == "num_certificado")
                value = ffcc.nro_certificado;
            else if (nombre_componente == "nombre_1_prof" || nombre_componente == "nombre")
                value = ffcc.profesional.nombre;
            else if (nombre_componente == "apellido_1_prof" || nombre_componente == "apellido")
                value = ffcc.profesional.apellido;
            else if (nombre_componente == "direccion")
                value = ffcc.profesional.direccion;
            else if (nombre_componente == "tipo_docum_prof")
                value = getValueItem(nodo.items, ffcc.profesional.tipo_doc);
            else if (nombre_componente == "num_docum_prof")
                value = ffcc.profesional.nro_doc;
            else if (nombre_componente == "cuit_prof")
                value = ffcc.profesional.cuit;
            else if (nombre_componente == "telefono_prof")
                value = ffcc.profesional.telefono;
            else if (nombre_componente == "email_prof" || nombre_componente == "email")
                value = ffcc.profesional.mail;
            else if (nombre_componente == "rol_profesional")
                value = ffcc.profesional.rol;
            else if (nombre_componente == "consejo")
                value = ffcc.profesional.consejo;
            else if (nombre_componente == "matricula")
                value = ffcc.profesional.matricula;
            else if (nombre_componente == "aclaraciones" || nombre_componente == "observaciones")
                value = ffcc.Observaciones;
            else if (nombre_componente == "id_documento")
                value = ffcc.nro_documento;
            else if (nombre_componente == "usuario" || nombre_componente== "usuario_accion")
                value = ffcc.usuario;
            else if (nombre_componente == "sistema_externo")
                value = ffcc.sistema_externo;
            else if (nombre_componente == "sistema")
                value = ffcc.sistema;
            else if (nombre_componente == "nombre_apellido_docente" || nombre_componente == "nombre_apellido_director"
                    || nombre_componente == "nombre_apellido")
                value = ffcc.nomyapel;
            else if (nombre_componente == "timestamp" || nombre_componente == "fecha_inicio_obra")
                value = ffcc.fecha_alta;
            else if (nombre_componente == "fecha_inspeccion_excavacion" || nombre_componente== "fecha_inspeccion")
                value = ffcc.fecha_inspeccion;
            else if (nombre_componente == "numo_encomienda" || nombre_componente== "encomienda")
                value = ffcc.nro_encomienda;
            else if (nombre_componente == "resultado_inspeccion")
                value = ffcc.resultado_inspeccion;
            else if (nombre_componente == "estado_origen")
                value = ffcc.estado_origen;
            else if (nombre_componente == "estado_destino")
                value = ffcc.estado_origen;
            else if (nombre_componente == "fecha_reinicio")
                value = ffcc.fecha_reinicio;
            //TODO
            //faltan los otros campos
            return value;
        }

        private static string getValueItem(List<string> items, string value)
        {
            foreach (string i in items)
                if (i.Contains(value))
                    return i;
            return "";
        }

        private async static Task<ffcc_DTO> cargaGenerica(int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud 
                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo)
                    .OrderByDescending(x => x.id_encomienda).FirstOrDefault();
            var sol = db.SSIT_Solicitudes.Where(x => x.id_solicitud == id_solicitud).First();
            var ubic = db.SSIT_Solicitudes_Ubicaciones.Where(x => x.id_solicitud == id_solicitud).First();
            var puerta = ubic.SSIT_Solicitudes_Ubicaciones_Puertas.FirstOrDefault();
            var ph = ubic.SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal.FirstOrDefault();
            string sql = "select dbo.SSIT_Solicitud_DireccionesPartidasPlancheta(" + id_solicitud + "," + ubic.id_ubicacion + ")";
            string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

            var q = new ffcc_DTO
            {
                tipo_obra="Local",
                destino = "habilitacion",
                estado_destino = "",
                estado_origen = "",
                expediente = new expediente_DTO
                {
                    nro_expediente = sol.NroExpedienteSade,
                    rubros = string.Join(";", db.Encomienda_Rubros.Where(x => x.id_encomienda == enc.id_encomienda).Select(x => x.cod_rubro).ToList()),
                    superficie = Convert.ToString(enc.Encomienda_DatosLocal.FirstOrDefault().superficie_cubierta_dl + enc.Encomienda_DatosLocal.FirstOrDefault().superficie_descubierta_dl),
                    ubicacion = direccion,
                    zona = enc.ZonaDeclarada
                },
                ubicacion = new ubicacion_DTO
                {
                    altura = puerta != null ? puerta.NroPuerta.ToString() : "",
                    barrio = ubic.Ubicaciones != null && ubic.Ubicaciones.Barrios != null ? ubic.Ubicaciones.Barrios.nom_barrio : "",
                    calle = puerta != null ? puerta.nombre_calle : "",
                    codigo_postal = "",
                    comuna = ubic.Ubicaciones != null && ubic.Ubicaciones.Comunas != null ? ubic.Ubicaciones.Comunas.nom_comuna : "",
                    manzana = ubic.Ubicaciones != null ? ubic.Ubicaciones.Manzana : "",
                    parcela = ubic.Ubicaciones != null ? ubic.Ubicaciones.Parcela : "",
                    seccion = ubic.Ubicaciones != null ? ubic.Ubicaciones.Seccion.ToString() : "",
                    piso=ph!=null?ph.Ubicaciones_PropiedadHorizontal.Piso:"",
                    depto=ph != null ? ph.Ubicaciones_PropiedadHorizontal.Depto:"",
                },
                fecha_alta = sol.CreateDate.ToString(),
                fecha_inspeccion = "",
                resultado_inspeccion = "",
                fecha_reinicio = "",
                nomyapel = "",
                nro_documento = "",
                nro_encomienda = enc.id_encomienda.ToString(),
                Observaciones = "",
                profesional = new entidadPF_DTO
                {
                    apellido = enc.Profesional.Apellido,
                    nombre = enc.Profesional.Nombre,
                    consejo = enc.Profesional.ConsejoProfesional.Nombre,
                    cuit = enc.Profesional.Cuit,
                    direccion = "",
                    mail = enc.Profesional.Email,
                    matricula = enc.Profesional.Matricula,
                    rol = "",
                    telefono = enc.Profesional.Telefono,
                    nro_doc = enc.Profesional.NroDocumento.ToString(),
                    tipo_doc = "DNI"
                },
                sistema = "SGI",
                sistema_externo = "SGI",
                //tarea = new tarea_DTO { proxima_tarea = "",resultado = ""},
                tipo_tramite = "Inscripción",
                titularPF = db.SSIT_Solicitudes_Titulares_PersonasFisicas.Where(x => x.id_solicitud == id_solicitud)
                    .Select(x => new entidadPF_DTO
                    {
                        apellido = x.Apellido,
                        nombre = x.Nombres,
                        consejo = "",
                        cuit = x.Cuit,
                        direccion = "",
                        mail = x.Email,
                        matricula = "",
                        rol = "",
                        telefono = x.Telefono
                    }).ToList(),

                titularPJ = db.SSIT_Solicitudes_Titulares_PersonasJuridicas.Where(x => x.id_solicitud == id_solicitud)
                    .Select(x => new entidadPJ_DTO
                    {
                        razon_social = x.Razon_Social,
                        consejo = "",
                        cuit = x.CUIT,
                        direccion = "",
                        mail = x.Email,
                        matricula = "",
                        rol = "",
                        telefono = x.Telefono
                    }).ToList(),
                propietario= db.SSIT_Solicitudes_Titulares_PersonasFisicas.Where(x => x.id_solicitud == id_solicitud)
                    .Select(x => new entidadPF_DTO
                    {
                        apellido = x.Apellido,
                        nombre = x.Nombres,
                        consejo = "",
                        cuit = x.Cuit,
                        direccion = "",
                        mail = x.Email,
                        matricula = "",
                        rol = "",
                        telefono = x.Telefono,
                        nro_doc=x.Nro_Documento,
                        tipo_doc=x.TipoDocumentoPersonal.Nombre
                    }).FirstOrDefault(),
                usuario = ""
            };

            //ws_Interface_AGC servicio = new ws_Interface_AGC();
            //SGI.Webservices.ws_interface_AGC.wsResultado ws_resultado_CAA = new SGI.Webservices.ws_interface_AGC.wsResultado();

            //servicio.Url = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC");
            //string username_servicio = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.User");
            //string password_servicio = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.Password");
            List<int> ids = new List<int>();
            ids.Add(enc.id_encomienda);
            //DtoCAA[] listCaa = servicio.Get_CAAs_by_Encomiendas(username_servicio, password_servicio, ids.ToArray(), ref ws_resultado_CAA);
            List<GetCAAsByEncomiendasResponse> listCaa = await GetCAAsByEncomiendas(ids.ToArray());
            if (listCaa != null && listCaa.Count() > 0)
            {
                string anio = Convert.ToString(listCaa[0].fechaIngreso.Year);
                q.nro_certificado = listCaa[0].id_tipocertificado + ": TRW-" + listCaa[0].id_solicitud + "-APRA-" + anio;
            }
            db.Dispose();
            return q;
        }

        private async static Task<List<GetCAAsByEncomiendasResponse>> GetCAAsByEncomiendas(int[] lst_id_Encomiendas)
        {
            ExternalService.ApraSrvRest apraSrvRest = new ExternalService.ApraSrvRest();
            List<GetCAAsByEncomiendasResponse> lstCaa = await apraSrvRest.GetCAAsByEncomiendas(lst_id_Encomiendas.ToList());
            return lstCaa;
        }
    }

    #region DTO's
    class nodoDTO
    {
        public int id_ffcc { get; set; }
        public string nombre_ffcc { get; set; }
        public int id { get; set; }
        public string nombre_componente { get; set; }
        public string etiqueta_componente { get; set; }
        public bool obligatorio { get; set; }
        public int ordern { get; set; }
        public string tipo_dato { get; set; }
        public int? maxlength { get; set; }
        public string constraint { get; set; }
        public List<subNodoDTO> componentes { get; set; }
        public List<String> items { get; set; }
    }
    class subNodoDTO
    {
        public int id { get; set; }
        public string nombre_componente { get; set; }
        public string etiqueta_componente { get; set; }
        public bool obligatorio { get; set; }
        public int ordern { get; set; }
        public string tipo_dato { get; set; }
        public int? maxlength { get; set; }
        public string constraint { get; set; }
    }
    class nodoDTO_R
    {
        public int id_ffcc { get; set; }
        public int id { get; set; }
        public string value { get; set; }
        public List<nodoDTO_R> componentes { get; set; }
    }

    public class ffcc_DTO
    {
        public string nro_encomienda { get; set; }
        public string nro_certificado { get; set; }
        public string sistema { get; set; }
        public string sistema_externo { get; set; }
        public string nro_documento { get; set; }
        public string nomyapel { get; set; }
        public string fecha_alta { get; set; }
        public string fecha_inspeccion { get; set; }
        public string resultado_inspeccion { get; set; }
        public string Observaciones { get; set; }
        public string estado_origen { get; set; }
        public string estado_destino { get; set; }
        public string fecha_reinicio { get; set; }
        public ubicacion_DTO ubicacion { get; set; }
        public entidadPF_DTO propietario { get; set; }
        public string usuario { get; set; }
        public List<entidadPF_DTO> titularPF { get; set; }
        public List<entidadPJ_DTO> titularPJ { get; set; }
        public entidadPF_DTO profesional { get; set; }
        public tarea_DTO tarea { get; set; }
        public expediente_DTO expediente { get; set; }
        public string tipo_obra { get; set; }
        public string tipo_tramite { get; set; }
        public string destino { get; set; }
    }
    public class ubicacion_DTO
    {
        public string calle { get; set; }
        public string altura { get; set; }
        public string barrio { get; set; }
        public string comuna { get; set; }
        public string seccion { get; set; }
        public string manzana { get; set; }
        public string parcela { get; set; }
        public string codigo_postal { get; set; }
        public string piso { get; set; }
        public string depto { get; set; }
    }
    public class entidadPF_DTO
    {
        public string rol { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string cuit { get; set; }
        public string direccion { get; set; }
        public string mail { get; set; }
        public string telefono { get; set; }
        public string consejo { get; set; }
        public string matricula { get; set; }
        public string tipo_doc { get; set; }
        public string nro_doc { get; set; }
    }
    public class entidadPJ_DTO
    {
        public string rol { get; set; }
        public string razon_social { get; set; }
        public string cuit { get; set; }
        public string direccion { get; set; }
        public string mail { get; set; }
        public string telefono { get; set; }
        public string consejo { get; set; }
        public string matricula { get; set; }
    }
    public class tarea_DTO
    {
        public string resultado { get; set; }
        public string proxima_tarea { get; set; }
    }
    public class expediente_DTO
    {
        public string nro_expediente { get; set; }
        public string ubicacion { get; set; }
        public string zona { get; set; }
        public string superficie { get; set; }
        public string rubros { get; set; }
    }
    #endregion

}