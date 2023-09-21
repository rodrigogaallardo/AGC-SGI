using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Web.Security;
using System.Transactions;

namespace SGI.Model
{

    public class Engine
    {
        public const string Sufijo_Enviar_a_DGFyC = "35";

        public static string Sufijo_Revision_DGHP2 = "15";
        public static string Sufijo_Revision_Firma_dispo2 = "32";

        public static Guid GetUltimoUsuarioAsignado(int id_solicitud, int id_tarea, int id_grupotramite, List<string> codTar)
        {

            DGHP_Entities db = new DGHP_Entities();
            Guid? userid = null;

            int id_cir_actual = (from tar in db.ENG_Tareas
                            join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                            where tar.id_tarea == id_tarea
                           select cir.id_circuito).FirstOrDefault();

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                userid = (from tt in db.SGI_Tramites_Tareas
                          join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                          join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                          join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                          where tt_hab.id_solicitud == id_solicitud && tt.FechaCierre_tramitetarea.HasValue && cir.id_circuito == id_cir_actual &&
                          codTar.Contains(tt.ENG_Tareas.cod_tarea.ToString().Substring(tt.ENG_Tareas.cod_tarea.ToString().Length - 2, 2))
                          orderby tt.FechaCierre_tramitetarea descending
                          select tt.UsuarioAsignado_tramitetarea).FirstOrDefault();
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                userid = (from tt in db.SGI_Tramites_Tareas
                          join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                          join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                          join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                          where tt_tr.id_solicitud == id_solicitud && tt.FechaCierre_tramitetarea.HasValue && cir.id_circuito == id_cir_actual &&
                          codTar.Contains(tt.ENG_Tareas.cod_tarea.ToString().Substring(tt.ENG_Tareas.cod_tarea.ToString().Length - 2, 2))
                          orderby tt.FechaCierre_tramitetarea descending
                          select tt.UsuarioAsignado_tramitetarea).FirstOrDefault();
            }

            Guid ret = Guid.Empty;
            if (userid.HasValue)
                ret = (Guid)userid;

            return ret;

        }

        public static Guid GetUltimoUsuarioAsignadoCP(int id_solicitud, int id_tarea)
        {

            DGHP_Entities db = new DGHP_Entities();
            Guid? userid = (from tt in db.SGI_Tramites_Tareas
                            join tt_cp in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                            where tt_cp.id_cpadron == id_solicitud && tt.id_tarea == id_tarea && tt.FechaCierre_tramitetarea.HasValue
                            orderby tt.FechaCierre_tramitetarea descending
                            select tt.UsuarioAsignado_tramitetarea).FirstOrDefault();

            Guid ret = Guid.Empty;
            if (userid.HasValue)
                ret = (Guid)userid;

            return ret;

        }

        public static bool CheckEditTarea(int id_tramitetarea, Guid userid)
        {
            bool ret = false;

            DGHP_Entities db = new DGHP_Entities();

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            // Si posee el rol para modificar la tarea se continua la validación
            ret = CheckRolTarea(id_tramitetarea, userid);

            // Si la tarea no está cerrada y está asignada al usuario logueado
            if (ret && !tramite_tarea.FechaCierre_tramitetarea.HasValue && tramite_tarea.UsuarioAsignado_tramitetarea == userid)
                ret = true;
            else
                ret = false;

            db.Dispose();

            return ret;

        }
        public static bool PoseeProcesos(int id_tramitetarea)
        {
            bool ret = false;

            DGHP_Entities db = new DGHP_Entities();

            ret = db.SGI_SADE_Procesos.Count(x => x.id_tramitetarea == id_tramitetarea) > 0;
            
            db.Dispose();

            return ret;
        }

        public static bool CheckRolTarea(int id_tramitetarea, Guid userid)
        {
            bool ret = false;

            DGHP_Entities db = new DGHP_Entities();

            SGI_Tramites_Tareas tramite_tarea = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);

            var perfiles_permitidos_tarea = (from perfiles_tarea in db.ENG_Rel_Perfiles_Tareas
                                             join perfiles in db.SGI_Perfiles on perfiles_tarea.id_perfil equals perfiles.id_perfil
                                             where perfiles_tarea.id_tarea == tramite_tarea.id_tarea
                                             select perfiles.nombre_perfil).ToList();

            var perfiles_usuario = db.aspnet_Users.FirstOrDefault(usu => usu.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

            var roles_en_comun = (from perfil1 in perfiles_permitidos_tarea select perfil1).Intersect
                            (from perfil2 in perfiles_usuario select perfil2).ToList();

            // Si posee el rol para modificar la tarea 
            ret = (roles_en_comun.Count > 0);

            db.Dispose();

            return ret;

        }

        public static void TomarTarea(int id_tramitetarea, Guid userid)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.ENG_Asignar_Tarea(id_tramitetarea, userid);
            db.Dispose();
        }

        public static void ReTomarTarea(int id_tt, Guid user)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.ENG_Reasignar_Tarea(id_tt, user);
            db.Dispose();
        }

        public static void AsignarTarea(int id_tramitetarea, Guid userid_a_asignar, Guid userid_asignador)
        {

            DGHP_Entities db = new DGHP_Entities();
            using (TransactionScope tran = new TransactionScope())
            {

                try
                {
                    db.ENG_Bandeja_Asignar(id_tramitetarea, userid_a_asignar, userid_asignador);
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    tran.Dispose();
                    LogError.Write(ex, "Error en transaccion. AsignarTarea");
                    throw ex;
                }
            }
            db.Dispose();

        }
        public static int ReasignarTarea(int id_tramitetarea, Guid? userid_a_asignar)
        {
            int id_tramitetarea_nuevo = 0;

            DGHP_Entities db = new DGHP_Entities();

            try
            {
                id_tramitetarea_nuevo = (int)db.SGI_ResignarTarea(id_tramitetarea, userid_a_asignar).FirstOrDefault();
            }
            catch (Exception ex)
            {
                db.Dispose();
                throw ex;
            }

            db.Dispose();

            return id_tramitetarea_nuevo;

        }

        public static List<Tarea> GetTareasSiguientes(int id_resultado, int id_tarea_actual, int id_tramitetarea)
        {
            // Devuelve la lista de tareas siguientes en base al resultado de la tarea enviados en los parámetros de entrada.

            List<Tarea> ret = new List<Tarea>();

            DGHP_Entities db = new DGHP_Entities();
            ObjectResult<ENG_GetTransicionesxResultado_Result> objResult = db.ENG_GetTransicionesxResultado(id_tarea_actual, id_resultado, id_tramitetarea);
            IEnumerable<ENG_GetTransicionesxResultado_Result> lstResult = objResult.OrderBy(x => x.nombre_tarea).ToList();


            foreach (var item in lstResult)
            {
                ret.Add(Tarea.Get(Convert.ToInt32(item.id_tarea), id_tramitetarea));
            }

            db.Dispose();
            return ret;

        }


        public class Tarea
        {

            public int id_tarea { get; set; }
            public int id_circuito { get; set; }
            public string nombre_circuito { get; set; }
            public int cod_tarea { get; set; }
            public string nombre_tarea { get; set; }
            public List<Resultado> Resultados { get; set; }

            public Tarea()
            {
                this.Resultados = new List<Resultado>();
            }

            public static Tarea Get(int id_tarea, int id_tramitetarea)
            {

                Tarea objret = new Tarea();

                Model.DGHP_Entities db = new Model.DGHP_Entities();
                Model.ENG_Tareas ENG_Tarea = db.ENG_Tareas.FirstOrDefault(x => x.id_tarea == id_tarea);

                objret.id_tarea = ENG_Tarea.id_tarea;
                objret.cod_tarea = ENG_Tarea.cod_tarea;
                objret.nombre_tarea = ENG_Tarea.nombre_tarea;
                objret.id_circuito = ENG_Tarea.id_circuito;
                objret.nombre_circuito = ENG_Tarea.ENG_Circuitos.nombre_circuito;

                // Carga los resultados posibles de una Tarea
                List<Resultado> resultados = (from rel_resultado_tarea in db.ENG_Rel_Resultados_Tareas
                                             join resultado in db.ENG_Resultados on rel_resultado_tarea.id_resultado equals resultado.id_resultado
                                             join tarea in db.ENG_Tareas on rel_resultado_tarea.id_tarea equals tarea.id_tarea
                                             where rel_resultado_tarea.id_tarea == id_tarea
                                             orderby resultado.nro_orden_resultado, tarea.cod_tarea
                                             select new Resultado
                                             {
                                                 id_resultado = resultado.id_resultado,
                                                 nombre_resultado = resultado.nombre_resultado
                                             }).ToList();
                //antes chequeo si es una habilitacion
                if (id_tramitetarea != 0)
                {
                    //tambien chequeo que no es una transferencia vieja)
                    if (objret.id_circuito != (int)Constants.ENG_Circuitos.TRANSF_NUEVO && objret.id_circuito != (int)Constants.ENG_Circuitos.TRANSF) 
                    {
                        int id_solicitud = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea).id_solicitud;
                        int existePlanoIncendio = (from es in db.Encomienda_SSIT_Solicitudes
                                                   join e in db.Encomienda on es.id_encomienda equals e.id_encomienda
                                                   join ep in db.Encomienda_Planos on e.id_encomienda equals ep.id_encomienda
                                                   where es.id_solicitud == id_solicitud
                                                   && ep.id_tipo_plano == (int)Constants.TiposDePlanos.Plano_Contra_Incendio
                                                   && e.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                                                   select es.id_encomiendaSolicitud).Union(from sd in db.SSIT_DocumentosAdjuntos
                                                                                           where sd.id_solicitud == id_solicitud
                                                                                           && sd.id_tdocreq == 66
                                                                                           select sd.id_docadjunto).Count();

                        List<Resultado> borrar = new List<Resultado>();
                        if (existePlanoIncendio == 0)
                        {
                            foreach (Resultado item in resultados)
                            {
                                if (item.id_resultado == 94)
                                    borrar.Add(item);
                            }
                        }
                        resultados.RemoveAll(borrar.Contains);
                    }
                }
                

                objret.Resultados = resultados;
                return objret;

            }



        }


        public class Resultado
        {
            public int id_resultado { get; set; }
            public string nombre_resultado { get; set; }

            public static Resultado Get(int id_resultado)
            {
                DGHP_Entities db = new DGHP_Entities();
                ENG_Resultados ENG_Resultado = db.ENG_Resultados.FirstOrDefault(x => x.id_resultado == id_resultado);
                Resultado ret = new Resultado();
                ret.id_resultado = ENG_Resultado.id_resultado;
                ret.nombre_resultado = ENG_Resultado.nombre_resultado;
                db.Dispose();
                return ret;
            }

            public static List<Resultado> GetAll()
            {
                DGHP_Entities db = new DGHP_Entities();
                var items = db.ENG_Resultados.OrderBy(x => x.nro_orden_resultado).ToList();

                List<Resultado> ret = new List<Resultado>();
                foreach (ENG_Resultados ENG_Resultado in items)
                {
                    Resultado itmResultado = new Resultado();
                    itmResultado.id_resultado = ENG_Resultado.id_resultado;
                    itmResultado.nombre_resultado = ENG_Resultado.nombre_resultado;
                    ret.Add(itmResultado);
                }

                db.Dispose();
                return ret;
            }
        }


        public static void getIdSolicitud_IdGrupoTrabajo(int id_tramitetarea, out int id_solicitud, out int id_grupotramite)
        {
            DGHP_Entities db = new DGHP_Entities();

            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            if (ttHAB != null)
            {
                id_solicitud = ttHAB.id_solicitud;
                id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            }
            else
            {
                SGI_Tramites_Tareas_CPADRON ttCP = db.SGI_Tramites_Tareas_CPADRON.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
                if (ttCP != null)
                {
                    id_solicitud = ttCP.id_cpadron;
                    id_grupotramite = (int)Constants.GruposDeTramite.CP;
                }
                else
                {
                    SGI_Tramites_Tareas_TRANSF ttTR = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
                    if (ttTR != null)
                    {
                        id_solicitud = ttTR.id_solicitud;
                        id_grupotramite = (int)Constants.GruposDeTramite.TR;
                    }
                    else
                    {
                        db.Dispose();
                        throw new Exception(string.Format("No se encontro en la tabla SGI_CPadron_Tareas un registro coincidente con el id = {0}", id_tramitetarea));
                    }
                }
            }
            db.Dispose();
        }

        public static void getIdGrupoTrabajo(int id_solicitud, out int id_grupotramite)
        {
            DGHP_Entities db = new DGHP_Entities();
            SSIT_Solicitudes sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
            SGI_Tramites_Tareas_HAB ttHAB = db.SGI_Tramites_Tareas_HAB.FirstOrDefault(x => x.id_solicitud == id_solicitud);
            bool EsSol = (ttHAB != null || sol != null);
            int id_tipotramite = (sol == null?0:sol.id_tipotramite);

            if (ttHAB != null || id_tipotramite == (int) Constants.TipoDeTramite.Permiso)
            {
                id_grupotramite = (int)Constants.GruposDeTramite.HAB;
            }
            else
            {
                SGI_Tramites_Tareas_TRANSF ttTR = db.SGI_Tramites_Tareas_TRANSF.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                if (ttTR != null)
                {
                    id_grupotramite = (int)Constants.GruposDeTramite.TR;
                }
                else
                {
                    SGI_Tramites_Tareas_CPADRON ttCP = db.SGI_Tramites_Tareas_CPADRON.FirstOrDefault(x => x.id_cpadron == id_solicitud);
                    if (ttCP != null)
                    {
                        id_grupotramite = (int)Constants.GruposDeTramite.CP;
                    }
                    db.Dispose();
                    throw new Exception(string.Format("No se encontro tarea relacionada a la solicitud con id = {0}", id_solicitud));
                }
            }

            db.Dispose();
        }

        public class Transicion
        {
            public int id_transicion { get; set; }
            public Tarea Tarea_Origen { get; set; }
            public Tarea Tarea_Destino { get; set; }
            public string condiciones_transicion { get; set; }
            public string acciones_al_entrar { get; set; }
            public string acciones_al_salir { get; set; }


            public static Transicion Get(int id_transicion)
            {
                DGHP_Entities db = new DGHP_Entities();
                ENG_Transiciones ENG_Transicion = db.ENG_Transiciones.FirstOrDefault(x => x.id_transicion == id_transicion);
                Transicion ret = new Transicion();

                ret.id_transicion = ENG_Transicion.id_transicion;
                ret.Tarea_Origen = Tarea.Get(ENG_Transicion.id_tarea_origen, 0);
                ret.Tarea_Destino = Tarea.Get(ENG_Transicion.id_tarea_destino, 0);
                ret.condiciones_transicion = ENG_Transicion.condiciones_transicion;
                ret.acciones_al_entrar = ENG_Transicion.acciones_al_entrar;
                ret.acciones_al_salir = ENG_Transicion.acciones_al_salir;

                db.Dispose();
                return ret;
            }

            public static List<Transicion> GetAll()
            {
                DGHP_Entities db = new DGHP_Entities();
                var items = db.ENG_Transiciones.ToList();

                List<Transicion> ret = new List<Transicion>();
                foreach (ENG_Transiciones ENG_Transicion in items)
                {
                    Transicion itm = new Transicion();
                    itm.id_transicion = ENG_Transicion.id_transicion;
                    itm.Tarea_Origen = Tarea.Get(ENG_Transicion.id_tarea_origen, 0);
                    itm.Tarea_Destino = Tarea.Get(ENG_Transicion.id_tarea_destino, 0);
                    itm.condiciones_transicion = ENG_Transicion.condiciones_transicion;
                    itm.acciones_al_entrar = ENG_Transicion.acciones_al_entrar;
                    itm.acciones_al_salir = ENG_Transicion.acciones_al_salir;

                    ret.Add(itm);
                }

                db.Dispose();
                return ret;
            }
        }

        internal static bool EsCalificador(Guid userId)
        {
            bool retorno = false;
            using (DGHP_Entities db = new DGHP_Entities())
            {
                var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userId).SGI_PerfilesUsuarios.Select(x => x.id_perfil).ToList();

                var perfiles_calificador = db.SGI_Perfiles.Where(p => p.descripcion_perfil.Contains("Calificador")).Select(x => x.id_perfil).ToList();

                retorno = perfiles_usuario.Where(p => perfiles_calificador.Contains(p)).Any();
            }
            return retorno;
        }
    }
}