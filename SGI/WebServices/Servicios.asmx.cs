using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using SGI.Model;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web.Http;

namespace SGI
{
    /// <summary>
    /// Descripción breve de Servicios
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Servicios : System.Web.Services.WebService
    {
        public class clsEquipoTrabajo
        {
            public string userid { get; set; }
            public string nombres_apellido { get; set; }
            public string tramites { get; set; }
        }
        public class clsRubros
        {
            public string cod_rubro { get; set; }
            public string desc_rubro { get; set; }
            public bool EsAnterior { get; set; }
        }

        [WebMethod]
        public List<clsEquipoTrabajo> GetEquipoDeTrabajo(string userid, string id_perfil_asignado)
        {
            List<clsEquipoTrabajo> elements = new List<clsEquipoTrabajo>();
            DGHP_Entities db = new DGHP_Entities();
            int? id_perfil = int.Parse(id_perfil_asignado);
            Guid Usuario = Guid.Parse(userid);

            SGI_Perfiles RolFiltro = db.SGI_Perfiles.FirstOrDefault(x => x.id_perfil == id_perfil);

            var Empleados = from equipo in db.ENG_EquipoDeTrabajo
                    join usuarios in db.aspnet_Users on equipo.Userid equals usuarios.UserId
                    join datos_usuario in db.SGI_Profiles on usuarios.UserId equals datos_usuario.userid
                    where equipo.Userid_Responsable == Usuario
                    select new { Roles = usuarios.SGI_PerfilesUsuarios, Nombres = datos_usuario.Nombres, Apellido = datos_usuario.Apellido, userid = equipo.Userid };


            foreach (var empleado in Empleados)
            {
                
                if (empleado.Roles.Contains(RolFiltro))
                {
                    clsEquipoTrabajo item = new clsEquipoTrabajo();

                    item.userid = empleado.userid.ToString().ToUpper();
                    item.nombres_apellido = empleado.Nombres + " " + empleado.Apellido;
                    
                    // Cantidad de trámites asignados sin resolver que tiene el empleado.
                    int cantTramitesAsignados = db.SGI_Tramites_Tareas.Count(x => x.UsuarioAsignado_tramitetarea == empleado.userid && !x.FechaCierre_tramitetarea.HasValue);
                    if (cantTramitesAsignados == 0)
                        item.tramites = "Sin trámites pendientes";
                    else if (cantTramitesAsignados == 1)
                        item.tramites = "1 trámite";
                    else
                        item.tramites = string.Format("{0} trámites", cantTramitesAsignados);
                    
                    
                    elements.Add(item);

                }
            }
            
            db.Dispose();

            return elements;
        }

        [WebMethod]
        public bool AsignarEmpleado(string userid_a_asignar,  string ids_tramite_tarea)
        {
            MembershipUser usu = Membership.GetUser();
            Guid userid_asignador = (Guid)usu.ProviderUserKey;
            if (usu != null)
            {
                foreach (string itm in ids_tramite_tarea.Split(",".ToCharArray()))
                {
                    int id_tramitetarea = Convert.ToInt32(itm);

                    DGHP_Entities db = new DGHP_Entities();
                    var tarea_resultado = (from tramite_tareas in db.SGI_Tramites_Tareas
                                           where tramite_tareas.id_tramitetarea == id_tramitetarea
                                           select new
                                           { tramite_tareas.id_tarea }).FirstOrDefault();

                    var rel_resutado_tarea = (from errt in db.ENG_Rel_Resultados_Tareas 
                                      where errt.id_tarea == tarea_resultado.id_tarea
                                      select errt).FirstOrDefault();

                    var transicion = db.ENG_GetTransicionesxResultado(tarea_resultado.id_tarea, rel_resutado_tarea.id_resultado, id_tramitetarea).FirstOrDefault();

                    if (transicion == null)
                        throw new ArgumentException("No se encontro una transicion para la tarea asignada");


                    var usuario = db.aspnet_Users.Where(x => x.UserId.ToString().ToLower() == userid_a_asignar.ToLower()).FirstOrDefault();
                    var perfiles_usuario_a_asignar = usuario.SGI_PerfilesUsuarios.Select(p => p.id_perfil).ToList();


                    var lista_resultado = from erpt in db.ENG_Rel_Perfiles_Tareas
                                          where perfiles_usuario_a_asignar.Contains(erpt.id_perfil)
                                          && transicion.id_tarea == erpt.id_tarea
                                          select new
                                          { erpt.id_tarea };

                    if (!lista_resultado.Any())
                        throw new ArgumentException("El usuario no tiene permisos para tomar esta tarea");


                    Engine.AsignarTarea(id_tramitetarea, Guid.Parse(userid_a_asignar),userid_asignador);
                }
            }
            else
            {
                throw new ArgumentException("El usuario no está logueado");
            }
            return true;
        }

        [WebMethod]
        public List<clsEquipoTrabajo> GetEmpleados(string apellido, string id_perfil_asignado)
        {
            apellido = apellido.ToLower();

            List<clsEquipoTrabajo> elements = new List<clsEquipoTrabajo>();

            DGHP_Entities db = new DGHP_Entities();
            int? perfil_asignado = int.Parse(id_perfil_asignado);

            SGI_Perfiles RolFiltro = db.SGI_Perfiles.FirstOrDefault(x => x.id_perfil == perfil_asignado);

            var Empleados =
                            from usuarios in db.aspnet_Users
                            join datos_usuario in db.SGI_Profiles on usuarios.UserId equals datos_usuario.userid
                            where datos_usuario.Apellido.ToLower().Contains(apellido)
                            orderby datos_usuario.Apellido
                            select new
                            {
                                Roles = usuarios.SGI_Perfiles,
                                Nombres = datos_usuario.Nombres,
                                Apellido = datos_usuario.Apellido,
                                userid = usuarios.UserId
                            };


            foreach (var empleado in Empleados)
            {
                if (empleado.Roles.Contains(RolFiltro))
                {

                    clsEquipoTrabajo item = new clsEquipoTrabajo();

                    item.userid = empleado.userid.ToString().ToUpper();
                    item.nombres_apellido = empleado.Nombres + " " + empleado.Apellido;

                    elements.Add(item);

                }
            }


            if (elements.Count == 0)
            {
                clsEquipoTrabajo item = new clsEquipoTrabajo();

                item.userid = Guid.Empty.ToString();
                item.nombres_apellido = "No se encontraron empleados.";
                elements.Add(item);
            }

            db.Dispose();

            return elements;

        }

        [WebMethod]
        public bool PVH_Enviar(Guid userid, string clave, int id_tramitetarea, ref wsResultado resultado)
        {
            
            bool tramite_enviado = false;

            if (clave != "interno")
                return tramite_enviado;

            wsResultado result = new wsResultado();
            PVH pvh = new PVH(userid);

            try
            {
                pvh.cargarDatos(id_tramitetarea);
                tramite_enviado = pvh.Enviar();
            }
            catch (PVH.SGI_EnviarPVHException ex)
            {
                result.ErrorCode = 10;
                result.ErrorDescription = ex.Message;
            }
            catch (Exception ex)
            {
                result.ErrorCode = 5000;
                result.ErrorDescription = ex.Message;
            }

            resultado = result;

            return tramite_enviado;
        }

        [WebMethod]
        public List<Items> GetCalle(string nombre_calle)
        {
            if (nombre_calle.Length < 3)
                return null;

            nombre_calle = nombre_calle.ToLower();

            DGHP_Entities db = new DGHP_Entities();

            List<Calles> list_calles = db.Calles.Where(x => x.NombreOficial_calle.ToLower().Contains(nombre_calle)).ToList();

            List<Items> elements = new List<Items>();

            foreach (Calles item in list_calles)
	        {
                if (! item.AlturaDerechaInicio_calle.HasValue )
                    item.AlturaDerechaInicio_calle = item.AlturaDerechaInicio_calle = 0;

                if (! item.AlturaIzquierdaFin_calle.HasValue )
                    item.AlturaIzquierdaFin_calle = item.AlturaIzquierdaFin_calle = 0;

                string desc = item.NombreOficial_calle + " - (" + item.AlturaDerechaInicio_calle.ToString() + " - " + item.AlturaIzquierdaFin_calle.ToString() + ")";
                elements.Add(new Items(desc, item.Codigo_calle.ToString()));
	        }


            db.Dispose();

            if (elements.Count == 0)
            {
                elements = new List<Items>();

                elements.Add(new Items("No se encontraron empleados.", ""));
            }

            return elements;

        }

        [WebMethod]
        public async Task<bool> GenerarPDF(string userName, string clave, string sistema, int id_solicitud, int id_tipodocsis)
        {
            bool generada = false;

            //if (clave != "interno")
            //    return generada;

            int id_file = 0;
            int id_tramitetarea = 0;
            byte[] documento = null;
            DGHP_Entities db = new DGHP_Entities();

            try
            {

                ValidarUsuario ctrlUsuario = new ValidarUsuario(userName, clave);
                if (!ctrlUsuario.ValidarClave())
                {
                    throw new Exception("Usuario o clave invalida.");
                }
                if (id_solicitud > 0 && id_tipodocsis == (int)Constants.TiposDeDocumentosSistema.PLANCHETA_HABILITACION)
                {

                    var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    if (sol == null)
                        throw new Exception("La solicitud no existe.");

                    var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

                    if (enc == null)
                        throw new Exception("La encomienda no existe.");

                    // se busca la ultima tarea de la solicitud
                    // --
                    var tramitetarea = (from tt in db.SGI_Tramites_Tareas
                                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                        where tt_hab.id_solicitud == id_solicitud 
                                        orderby tt.FechaInicio_tramitetarea descending
                                        select tt).FirstOrDefault();


                    if (tramitetarea != null)
                    {
                        id_tramitetarea = tramitetarea.id_tramitetarea;
                    }

                    // Obtener el usuario de la revision DGHyP
                    int[] arrTareas = new int[] {(int) Constants.ENG_Tareas.ESP_Revision_DGHP, (int) Constants.ENG_Tareas.ESP_Revision_DGHP_Nuevo, 
                                    (int) Constants.ENG_Tareas.ESPAR_Revision_DGHP, (int) Constants.ENG_Tareas.SCP_Revision_DGHP, (int) Constants.ENG_Tareas.SCP_Revision_DGHP_Nuevo,
                                    (int) Constants.ENG_Tareas.SSP_Revision_DGHP,(int) Constants.ENG_Tareas.SSP_Revision_DGHP_Nuevo, 
                                    (int) Constants.ENG_Tareas.ESP_Revision_Gerente_2, (int) Constants.ENG_Tareas.ESPAR_Revision_Gerente_2, 
                                    (int) Constants.ENG_Tareas.SCP_Revision_Gerente, (int) Constants.ENG_Tareas.SSP_Revision_Gerente};

                    Guid? userid = (from tt in db.SGI_Tramites_Tareas
                                    join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                    join tar in db.SGI_Tarea_Revision_DGHP on tt.id_tramitetarea equals tar.id_tramitetarea
                                    where tt_hab.id_solicitud == id_solicitud
                                    select tt.UsuarioAsignado_tramitetarea)
                                    .Union(
                                        from tt in db.SGI_Tramites_Tareas
                                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                        join tar in db.SGI_Tarea_Revision_Gerente on tt.id_tramitetarea equals tar.id_tramitetarea
                                        where tt_hab.id_solicitud == id_solicitud
                                        select tt.UsuarioAsignado_tramitetarea
                                    )
                                    .Union(
                                        from tt in db.SGI_Tramites_Tareas
                                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                        join tar in db.SGI_Tarea_Revision_SubGerente on tt.id_tramitetarea equals tar.id_tramitetarea
                                        where tt_hab.id_solicitud == id_solicitud
                                        select tt.UsuarioAsignado_tramitetarea
                                    ).FirstOrDefault();
                                    

                    if(!userid.HasValue)
                        throw new Exception("No se encontró el usuario asignado a la tarea de revisión DGHyP.");
                    

                    string nro_expediente = sol.NroExpedienteSade;

                    documento = await Plancheta.GenerarPdfPlanchetahabilitacion(id_solicitud, id_tramitetarea, enc.id_encomienda, nro_expediente, false);

                    id_file = SGI.WebServices.ws_FilesRest.subirArchivo("Plancheta.pdf", documento);

                    System.Data.Entity.Core.Objects.ObjectParameter param_id_docadjunto = new System.Data.Entity.Core.Objects.ObjectParameter("id_docadjunto", typeof(int));
                    db.SSIT_DocumentosAdjuntos_Add(id_solicitud, 0, "", id_tipodocsis, true, id_file, "Plancheta.pdf", userid, param_id_docadjunto);

                    generada = (id_file > 0);
                }

                //if (id_solicitud > 0 && id_tipo_tramite == (int)Constants.TipoTramiteCertificados.Disposicion)
                //{

                //    // disposicion
                //    var queryDispo =
                //        (
                //            from tt in db.SGI_Tramites_Tareas
                //            join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                //            join proc in db.SGI_Tarea_Generar_Expediente_Procesos on tt.id_tramitetarea equals proc.id_tramitetarea
                //            where tt_hab.id_solicitud == id_solicitud && proc.id_proceso == 6
                //            select new
                //            {
                //                tt.id_tramitetarea,
                //                proc.id_paquete,
                //                proc.id_generar_expediente_proc
                //            }
                //        ).FirstOrDefault();

                //    int id_tramitetarea = queryDispo.id_tramitetarea;
                //    int id_paquete = queryDispo.id_paquete;
                //    int id_generar_expediente_proc = queryDispo.id_generar_expediente_proc;

                //    Expediente expe = new Expediente(ctrlUsuario.userId, Constants.ApplicationName); //userid, Constants.ApplicationName);
                //    expe.GenerarDisposicionPdf(id_paquete, ctrlUsuario.userId, ref documento, id_generar_expediente_proc);
                //    expe.Dispose();

                //    if (id_certificado > 0)
                //        generada = true;

                //}

            }
            catch (Exception ex)
            {
                db.Dispose();
                throw ex;
            }

            db.Dispose();

            return generada;
        }

        [WebMethod]
        public bool EnviarMailAviso(Guid userid, string clave, int id_solicitud, int id_proceso)
        {
            bool generado = false;

            if (clave != "interno")
                return generado;

            try
            {

                switch (id_proceso)
                {
                    case 5:
                        Mailer.MailMessages.SendMail_ExpedienteGenerado_v1(id_solicitud);
                        break;
                    case 6:
                        Mailer.MailMessages.SendMail_CorreccionSolicitud_v2(id_solicitud);
                        break;
                    case 7:
                        Mailer.MailMessages.SendMail_BoletaGenerada_v2(id_solicitud);
                        break;
                    case 8:
                        Mailer.MailMessages.SendMail_AprobacionDG_v2(id_solicitud);
                        break;
                }

                generado = true;
            }
            catch (Exception ex)
            {
            }


            return generado;
        }

        [WebMethod]
        public async Task<List<ProcesoExpediente>> ProcesarTareaExpediente(Guid userid, string clave, string sistema, int id_solicitud, int id_tramite_tarea, int id_generar_expediente_proc)
        {

            List<ProcesoExpediente> lstProcesos = null;

            if (clave != "interno")
                return lstProcesos;

            DGHP_Entities db = new DGHP_Entities();

            Expediente procesoExpediente = null;

            try
            {

                procesoExpediente = new Expediente(userid, Constants.ApplicationName);

                await procesoExpediente.Procesar(id_generar_expediente_proc, id_tramite_tarea);

                lstProcesos = procesoExpediente.GetProcesos_porTramiteTarea(id_tramite_tarea);

                procesoExpediente.Dispose();
            }
            catch (Exception ex)
            {
                if (procesoExpediente != null) 
                    procesoExpediente.Dispose();
                db.Dispose();
                throw ex;
            }

            db.Dispose();

            return lstProcesos;
        }

        [WebMethod]
        public string GenerarHtml_Disposicion(string userName, string clave, int id_solicitud, int id_tramitetarea)
        {
            string html_dispo = "";

            try
            {

                ValidarUsuario ctrlUsuario = new ValidarUsuario(userName, clave);
                if (!ctrlUsuario.ValidarClave())
                {
                    throw new Exception("Usuario o clave invalida.");
                }

                string NroExpediente = null;
                PdfDisposicion dispo_html = new PdfDisposicion();
                html_dispo = dispo_html.GenerarHtml_Disposicion(id_solicitud, id_tramitetarea, NroExpediente);
                dispo_html.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return html_dispo;
        }

        [WebMethod]
        public string Transf_GenerarHtml_Disposicion(string userName, string clave, int id_solicitud, int id_tramitetarea)
        {
            string html_dispo = "";

            try
            {

                ValidarUsuario ctrlUsuario = new ValidarUsuario(userName, clave);
                if (!ctrlUsuario.ValidarClave())
                {
                    throw new Exception("Usuario o clave invalida.");
                }

                string NroExpediente = null;
                string str_archivo = File.ReadAllText(Server.MapPath(@"~\Reportes\Transferencias\Disposicion.html"));
                html_dispo = PdfDisposicion.Transf_GenerarHtml_Disposicion(id_solicitud, id_tramitetarea, NroExpediente, str_archivo);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return html_dispo;
        }

        [WebMethod]
        public bool Transf_GenerarPDF(string userName, string clave, string sistema, int id_solicitud, int id_tipodocsis)
        {
            bool generada = false;

            int id_file = 0;
            byte[] documento = null;
            DGHP_Entities db = new DGHP_Entities();

            try
            {

                ValidarUsuario ctrlUsuario = new ValidarUsuario(userName, clave);
                if (!ctrlUsuario.ValidarClave())
                {
                    throw new Exception("Usuario o clave invalida.");
                }
                if (id_solicitud > 0 && id_tipodocsis == (int)Constants.TiposDeDocumentosSistema.PLANCHETA_TRANSFERENCIA)
                {

                    var sol = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    if (sol == null)
                        throw new Exception("La solicitud no existe.");

                    Guid? userid = (from tt in db.SGI_Tramites_Tareas
                                    join tt_hab in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                    join tar in db.SGI_Tarea_Revision_DGHP on tt.id_tramitetarea equals tar.id_tramitetarea
                                    where tt_hab.id_solicitud == id_solicitud
                                    select tt.UsuarioAsignado_tramitetarea)
                                    .Union(
                                        from tt in db.SGI_Tramites_Tareas
                                        join tt_hab in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                        join tar in db.SGI_Tarea_Revision_Gerente on tt.id_tramitetarea equals tar.id_tramitetarea
                                        where tt_hab.id_solicitud == id_solicitud
                                        select tt.UsuarioAsignado_tramitetarea
                                    )
                                    .Union(
                                        from tt in db.SGI_Tramites_Tareas
                                        join tt_hab in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                                        join tar in db.SGI_Tarea_Revision_SubGerente on tt.id_tramitetarea equals tar.id_tramitetarea
                                        where tt_hab.id_solicitud == id_solicitud
                                        select tt.UsuarioAsignado_tramitetarea
                                    ).FirstOrDefault();


                    if (!userid.HasValue)
                        throw new Exception("No se encontró el usuario asignado a la tarea de revisión DGHyP.");


                    string nro_expediente = sol.NroExpedienteSade;

                    documento = Plancheta.Transf_GenerarPdf(id_solicitud, nro_expediente, false);

                    id_file = SGI.WebServices.ws_FilesRest.subirArchivo("Plancheta.pdf", documento);

                    int id_tdocreq = (int)Constants.TiposDeDocumentosRequeridos.Plancheta;
                    string tdocreq_detalle = "";
                    string nombre_archivo = "";
                    System.Data.Entity.Core.Objects.ObjectParameter param_id_docadjunto = new System.Data.Entity.Core.Objects.ObjectParameter("id_docadjunto", typeof(int));

                    db.Transf_DocumentosAdjuntos_Agregar(id_solicitud, id_tdocreq, tdocreq_detalle, id_tipodocsis, true, id_file, nombre_archivo, userid, (int)Constants.NivelesDeAgrupamiento.General, param_id_docadjunto);

                    generada = (id_file > 0);
                }
            }
            catch (Exception ex)
            {
                db.Dispose();
                throw ex;
            }

            db.Dispose();

            return generada;
        }


        [WebMethod]
        public byte[] GenerarPDF_BajaSolicitud(string userName, string clave,int id_tipotramite, int id_solicitud)
        {

            DGHP_Entities db = new DGHP_Entities();
            byte[] documento = new byte[0];
            try
            {

                ValidarUsuario ctrlUsuario = new ValidarUsuario(userName, clave);
                if (!ctrlUsuario.ValidarClave())
                {
                    throw new Exception("Usuario o clave invalida.");
                }

                if (id_tipotramite == (int)Constants.TipoDeTramite.Habilitacion ||
                    id_tipotramite == (int)Constants.TipoDeTramite.Ampliacion_Unificacion ||
                    id_tipotramite == (int)Constants.TipoDeTramite.RedistribucionDeUso)
                {
                    var solBaja = db.SSIT_Solicitudes_Baja.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    if (solBaja != null)
                    {
                        documento = PdfSolicitudBaja.GenerarPDF(id_solicitud, solBaja.TiposMotivoBaja.nombre, solBaja.observaciones);
                    }
                    else
                        throw new Exception("No se encontró el registro de baja desde donde se debe generar el pdf");
                }
                else if(id_tipotramite == (int)Constants.TipoDeTramite.Transferencia)
                {

                    var solBaja = db.Transf_Solicitudes_Baja.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                    if (solBaja != null)
                    {
                        documento = PdfSolicitudBaja.Transf_GenerarPDF(id_solicitud, solBaja.TiposMotivoBaja.nombre, solBaja.observaciones);
                    }
                    else
                        throw new Exception("No se encontró el registro de baja desde donde se debe generar el pdf");
                    
                }

            }
            catch (Exception ex)
            {
                db.Dispose();
                throw ex;
            }
            finally
            {
                db.Dispose();
            }

            return documento;
        }

    }

    public class wsResultado
    {

        int v_ErrorCode = 0;
        string v_Description = "";


        public wsResultado()
        {

            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        public int ErrorCode
        {
            get { return v_ErrorCode; }
            set { v_ErrorCode = value; }
        }

        public string ErrorDescription
        {
            get { return v_Description; }
            set { v_Description = value; }
        }

        public override string ToString()
        {
            return "wsResultado.Codigo: " + this.v_ErrorCode + ";wsResultado.Descripcion: " + v_Description;
        }

        public string ToString(string mensaje)
        {
            return "Detalle: " + mensaje + ";" + this.ToString();
        }
    }

    public class ValidarUsuario
    {
        public string userName { get; set; }
        public string userPwd { get; set; }
        public string[] userRoles { get; set; }
        public Guid userId { get; set; }
        public string userMail { get; set; }
        public bool userBloquedado { get; set; }

        private MembershipProvider membershipProvider = null;
        private RoleProvider roleProvider = null;
        private MembershipUser membershipUser = null;
        private string aplicacion = "";

        public ValidarUsuario(string usuario)
        {
            InicializarDatos();
            this.userName = usuario;
            this.aplicacion = "SGI";
            InicializarAplicacion();
        }

        public ValidarUsuario(string usuario, string clave)
        {
            InicializarDatos();
            this.userName = usuario;
            this.userPwd = clave;
            this.aplicacion = "SGI";
            InicializarAplicacion();
        }

        private void InicializarDatos()
        {
            this.userName = "";
            this.userPwd = "";
            this.userRoles = null;
            //this.mensajeError = "";
            this.userMail = "";
            this.userBloquedado = false;

            this.membershipProvider = null;
            this.roleProvider = null;
            this.membershipUser = null;
        }

        private void InicializarAplicacion()
        {
            switch (aplicacion)
            {
                case "SGI":
                    this.membershipProvider = Membership.Providers["SqlMembershipProvider"];
                    this.roleProvider = System.Web.Security.Roles.Providers["SqlRoleProvider"];
                    break;
            }

            if (!string.IsNullOrEmpty(this.userName))
                this.membershipUser = this.membershipProvider.GetUser(this.userName, false);

        }

        private bool ValidarAutenticarUsuario()
        {
            bool ret = false;

            if (this.membershipUser != null)
            {
                this.userId = (Guid)membershipUser.ProviderUserKey;
                if (this.membershipUser.GetPassword().Equals(this.userPwd))
                {
                    ret = true;
                }
            }
            return ret;
        }

        public bool ValidarClave()
        {
            return ValidarAutenticarUsuario();
        }


        



    }


}
