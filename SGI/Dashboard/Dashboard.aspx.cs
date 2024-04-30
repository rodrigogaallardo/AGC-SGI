using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;


namespace SGI.Dashboard
{
    public partial class Dashboard : System.Web.UI.Page
    {

        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


            }

        }


        protected override void OnUnload(EventArgs e)
        {
            FinalizarEntity();
            base.OnUnload(e);
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }

        #endregion

        #region entity

        private DGHP_Entities db = null;
        private AGC_FilesEntities dbFiles = null;

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }
        }

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
            {
                this.dbFiles.Dispose();
                this.dbFiles = null;
            }
        }

        #endregion

        public class data
        {
            public string label { get; set; }
            public int value { get; set; }
            public string link_detalle { get; set; }
            public string icon { get; set; }
            public string color { get; set; }
            public string userid { get; set; }
            public int id_tarea { get; set; }

            public data()
            {
            }

            public data(string label, int value)
            {
                this.label = label;
                this.value = value;
            }

        }
        //De acuerdo al usuario determina que datos mostrara en el grafico.
        //Con los cambios mios

        public static int[] getTareasCalificarPorUsuario()
        {
            DGHP_Entities db = new DGHP_Entities();

            Guid userid = Functions.GetUserId();

            var user = db.aspnet_Users.Where(x => x.UserId == userid);
            List<Dashboard.circuitos> circuitos = (from rel in db.ENG_Rel_Perfiles_Tareas
                                                   join p in db.SGI_Perfiles on rel.id_perfil equals p.id_perfil
                                                   join tar in db.ENG_Tareas on rel.id_tarea equals tar.id_tarea
                                                   join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                                                   where p.aspnet_Users2.Where(x => x.UserId == userid).Count() > 0
                                                              &&
                                                              (
                                                                  tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Asignacion_Calificador
                                                                  ||
                                                                  tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Asignacion_Calificador2
                                                              //||
                                                              //tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Revision_SubGerente
                                                              //||
                                                              //tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Revision_SubGerente2
                                                              //||
                                                              //tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Calificar
                                                              )

                                                   select new Dashboard.circuitos
                                                   {
                                                       cod_circuito = cir.cod_circuito
                                                   }).Distinct().ToList();

            List<int> tareas_calificar = Tareas(circuitos);
            return tareas_calificar.ToArray();

        }
        private class circuitos
        {
            public string cod_circuito { get; set; }
            public string cod { get; set; }
        }

        private static List<int> Tareas(List<Dashboard.circuitos> circuitos)
        {
            List<int> tareas_calificar = new List<int>();
            foreach (var cir in circuitos)
            {
                switch (cir.cod_circuito)
                {
                    case "SCP":
                    case "SCP2":
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP_Calificar);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP_Calificar_Nuevo);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP2_Calificar);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP2_Asignacion_al_Calificador_Subgerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP2_Asignacion_al_Calificador_Gerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP2_Redist_Asignacion_al_Calificador_Subgerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP2_Redist_Asignacion_al_Calificador_Gerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP_Asignar_Calificador_Gerente_Nuevo);
                        break;

                    case "SSP":
                    case "SSP2":
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SSP_Calificar);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SSP_Calificar_Nuevo);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SSP2_Asignacion_al_Calificador_Subgerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SSP2_Asignacion_al_Calificador_Gerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SSP2_Calificar_Tramite);
                        break;

                    case "ESPAR":
                    case "ESPAR2":
                        tareas_calificar.Add((int)Constants.ENG_Tareas.ESPAR_Calificar_1);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.ESPAR_Calificar_1_Nuevo);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.ESPAR2_Asignacion_del_Calificador);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.ESPAR2_Redist_Asignacion_del_Calificador);
                        break;

                    case "ESPECIAL":
                    case "ESPECIAL2":
                        tareas_calificar.Add((int)Constants.ENG_Tareas.ESP_Calificar_1);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.ESP_Calificar_1_Nuevo);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.ESP_Calificar_2_Nuevo);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.ESP_Calificar_2_Nuevo);
                        break;

                    case "TRANSF":
                        tareas_calificar.Add((int)Constants.ENG_Tareas.TR_Calificar);
                        break;

                    case "SCP3":
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP3_Calificar);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP3_Asignacion_al_Calificador_Subgerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP3_Asignacion_al_Calificador_Gerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP3_Redist_Asignacion_al_Calificador_Subgerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP3_Redist_Asignacion_al_Calificador_Gerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP3_Asignar_Calificador_SubGerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP3_Asignar_Calificador_Gerente);
                        break;

                    case "SCP4":
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP4_Calificar);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP4_Asignacion_al_Calificador_Subgerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP4_Asignacion_al_Calificador_Gerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP4_Redist_Asignacion_al_Calificador_Subgerente);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP4_Redist_Asignacion_al_Calificador_Gerente);
                        break;

                    case "SCP5":
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP5_Calificar);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP5_Asignacion_del_Calificador);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.SCP5_Redist_Asignacion_del_Calificador);
                        break;

                    case "ESCU-HP":
                        tareas_calificar.Add((int)Constants.ENG_Tareas.ESCU_HP_Asignacion_del_Calificador);
                        tareas_calificar.Add((int)Constants.ENG_Tareas.ESCU_HP_Redist_Asignacion_del_Calificador);
                        break;
                    default:

                        break;
                }
            }

            return tareas_calificar.Distinct().ToList();
        }


        internal static int[] getTareasCalificarPorUsuarioTR()
        {
            DGHP_Entities db = new DGHP_Entities();

            Guid userid = Functions.GetUserId();

            var user = db.aspnet_Users.Where(x => x.UserId == userid);

            var circuitos = (from rel in db.ENG_Rel_Perfiles_Tareas
                             join p in db.SGI_Perfiles on rel.id_perfil equals p.id_perfil
                             join tar in db.ENG_Tareas on rel.id_tarea equals tar.id_tarea
                             join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                             where p.aspnet_Users2.Where(x => x.UserId == userid).Count() > 0
                                        && tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Calificar
                                        && (cir.id_circuito == (int)Constants.ENG_Circuitos.TRANSF || cir.id_circuito == (int)Constants.ENG_Circuitos.TRANSF_NUEVO)
                             select new
                             {
                                 cir.cod_circuito
                             }).Distinct().ToList();

            int[] tareas_calificar = null;

            foreach (var cir in circuitos)
            {
                switch (cir.cod_circuito)
                {
                    case "TRANSF":
                        if (tareas_calificar == null)
                            tareas_calificar = new int[2] { (int)Constants.ENG_Tareas.TR_Calificar, (int)Constants.ENG_Tareas.TRM_Calificar };
                        else
                            tareas_calificar.Concat(new int[2] { (int)Constants.ENG_Tareas.TR_Calificar,(int)Constants.ENG_Tareas.TRM_Calificar });
                        break;
                    default:
                        break;
                }
            }
            if (tareas_calificar == null)
                tareas_calificar = new int[0] { };

            return tareas_calificar;
        }

        internal static int[] getTareasAsignarPorUsuarioTR()
        {
            DGHP_Entities db = new DGHP_Entities();

            Guid userid = Functions.GetUserId();

            //var user = db.aspnet_Users.Where(x => x.UserId == userid);

            var circuitos = (from rel in db.ENG_Rel_Perfiles_Tareas
                             join p in db.SGI_Perfiles on rel.id_perfil equals p.id_perfil
                             join tar in db.ENG_Tareas on rel.id_tarea equals tar.id_tarea
                             join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                             where p.aspnet_Users2.Where(x => x.UserId == userid).Count() > 0
                                        && tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Asignacion_Calificador
                                        && cir.id_circuito == (int)Constants.ENG_Circuitos.TRANSF
                             select new
                             {
                                 cir.cod_circuito
                             }).Distinct().ToList();

            int[] tareas_asignar = null;

            foreach (var cir in circuitos)
            {
                switch (cir.cod_circuito)
                {
                    case "TRANSF":
                        if (tareas_asignar == null)
                            tareas_asignar = new int[2] { (int)Constants.ENG_Tareas.TR_Asignar_Calificador, (int)Constants.ENG_Tareas.TRM_Asignar_Calificador};
                        else
                            tareas_asignar.Concat(new int[2] { (int)Constants.ENG_Tareas.TR_Asignar_Calificador, (int)Constants.ENG_Tareas.TRM_Asignar_Calificador });
                        break;
                    default:
                        break;
                }
            }

            if (tareas_asignar == null)
                tareas_asignar = new int[0] { };
            return tareas_asignar;
        }

        //De acuerdo al usuario determina que datos mostrara en el grafico.
        public static int[] getTareasAsignarPorUsuario()
        {
            DGHP_Entities db = new DGHP_Entities();

            Guid userid = Functions.GetUserId();

            var circuitos = (from rel in db.ENG_Rel_Perfiles_Tareas
                             join p in db.SGI_Perfiles on rel.id_perfil equals p.id_perfil
                             join tar in db.ENG_Tareas on rel.id_tarea equals tar.id_tarea
                             join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                             where p.aspnet_Users2.Where(x => x.UserId == userid).Count() > 0
                                        && tar.cod_tarea.ToString().Substring(tar.cod_tarea.ToString().Length - 2, 2) == Constants.ENG_Tipos_Tareas.Asignacion_Calificador
                             select new
                             {
                                 cir.cod_circuito
                             }).Distinct().ToList();

            int[] tareas_asignar = null;

            foreach (var cir in circuitos)
            {
                switch (cir.cod_circuito)
                {
                    case "SCP":
                    case "SCP2":
                        if (tareas_asignar == null)
                            tareas_asignar = new int[3] { (int)Constants.ENG_Tareas.SCP_Asignar_Calificador, (int)Constants.ENG_Tareas.SCP_Asignar_Calificador_Gerente_Nuevo, (int)Constants.ENG_Tareas.SCP_Asignar_Calificador_SubGerente_Nuevo };
                        else
                            tareas_asignar = tareas_asignar.Concat(new int[3] { (int)Constants.ENG_Tareas.SCP_Asignar_Calificador, (int)Constants.ENG_Tareas.SCP_Asignar_Calificador_Gerente_Nuevo, (int)Constants.ENG_Tareas.SCP_Asignar_Calificador_SubGerente_Nuevo }).ToArray();
                        break;
                    case "SSP":
                    case "SSP2":
                        if (tareas_asignar == null)
                            tareas_asignar = new int[4] { (int)Constants.ENG_Tareas.SSP_Asignar_Calificador, (int)Constants.ENG_Tareas.SSP_Asignar_Calificador_Gerente_Nuevo, (int)Constants.ENG_Tareas.SSP_Asignar_Calificador_SubGerente_Nuevo, (int)Constants.ENG_Tareas.SSP2_Asignacion_al_Calificador_Gerente };
                        else
                            tareas_asignar = tareas_asignar.Concat(new int[4] { (int)Constants.ENG_Tareas.SSP_Asignar_Calificador, (int)Constants.ENG_Tareas.SSP_Asignar_Calificador_Gerente_Nuevo, (int)Constants.ENG_Tareas.SSP_Asignar_Calificador_SubGerente_Nuevo, (int)Constants.ENG_Tareas.SSP2_Asignacion_al_Calificador_Gerente }).ToArray();
                        break;
                    case "ESPAR":
                    case "ESPAR2":
                        if (tareas_asignar == null)
                            tareas_asignar = new int[3] { (int)Constants.ENG_Tareas.ESPAR_Calificar_1, (int)Constants.ENG_Tareas.ESPAR_Asignar_Calificador, (int)Constants.ENG_Tareas.ESPAR_Asignar_Calificador_Nuevo };
                        else
                            tareas_asignar = tareas_asignar.Concat(new int[3] { (int)Constants.ENG_Tareas.ESPAR_Calificar_1, (int)Constants.ENG_Tareas.ESPAR_Asignar_Calificador, (int)Constants.ENG_Tareas.ESPAR_Asignar_Calificador_Nuevo }).ToArray();
                        break;
                    case "ESPECIAL":
                    case "ESPECIAL2":
                        if (tareas_asignar == null)
                            tareas_asignar = new int[3] { (int)Constants.ENG_Tareas.ESPAR_Calificar_1, (int)Constants.ENG_Tareas.ESP_Asignar_Calificador, (int)Constants.ENG_Tareas.ESP_Asignar_Calificador_Nuevo };
                        else
                            tareas_asignar = tareas_asignar.Concat(new int[3] { (int)Constants.ENG_Tareas.ESPAR_Calificar_1, (int)Constants.ENG_Tareas.ESP_Asignar_Calificador, (int)Constants.ENG_Tareas.ESP_Asignar_Calificador_Nuevo }).ToArray();
                        break;
                    case "TRANSF":
                        if (tareas_asignar == null)
                            tareas_asignar = new int[1] { (int)Constants.ENG_Tareas.TR_Asignar_Calificador };
                        else
                            tareas_asignar = tareas_asignar.Concat(new int[1] { (int)Constants.ENG_Tareas.TR_Asignar_Calificador }).ToArray();
                        break;
                    case "SCP3":
                        if (tareas_asignar == null)
                            tareas_asignar = new int[2] {(int)Constants.ENG_Tareas.SCP3_Asignar_Calificador_Gerente, (int)Constants.ENG_Tareas.SCP3_Asignar_Calificador_SubGerente };
                        else
                            tareas_asignar = tareas_asignar.Concat(new int[2] { (int)Constants.ENG_Tareas.SCP3_Asignar_Calificador_Gerente, (int)Constants.ENG_Tareas.SCP3_Asignar_Calificador_SubGerente }).ToArray();
                        break;
                    case "SCP4":
                        if (tareas_asignar == null)
                            tareas_asignar = new int[2] { (int)Constants.ENG_Tareas.SCP4_Asignar_Calificador_Gerente, (int)Constants.ENG_Tareas.SCP4_Asignar_Calificador_SubGerente };
                        else
                            tareas_asignar = tareas_asignar.Concat(new int[2] { (int)Constants.ENG_Tareas.SCP4_Asignar_Calificador_Gerente, (int)Constants.ENG_Tareas.SCP4_Asignar_Calificador_SubGerente }).ToArray();
                        break;
                    case "SCP5":
                        if (tareas_asignar == null)
                            tareas_asignar = new int[1] { (int)Constants.ENG_Tareas.SCP5_Asignar_Calificador };
                        else
                            tareas_asignar = tareas_asignar.Concat(new int[1] { (int)Constants.ENG_Tareas.SCP5_Asignar_Calificador }).ToArray();
                        break;
                    default:
                    break;
                }
            }

            if (tareas_asignar == null)
                tareas_asignar = new int[0] { };
            return tareas_asignar.Distinct().ToArray();

        }

        [System.Web.Services.WebMethod]
        public static List<data> getDatos_solicitudes_asignadas_calif(string par1, string par2)
        {
            List<data> lista = new List<data>();
            DGHP_Entities db = null;

            try
            {
                int[] tareas_calificar = getTareasCalificarPorUsuario();

                db = new DGHP_Entities();
                Guid userid = Functions.GetUserId();
                var qEquipo =
                       (

                           from eq in db.ENG_EquipoDeTrabajo
                           join usr in db.SGI_Profiles on eq.Userid equals usr.userid
                           join mem in db.aspnet_Membership on usr.userid equals mem.UserId
                           where eq.Userid_Responsable == userid
                               && mem.IsApproved == true
                           select
                            usr.userid

                       ).ToList();

                var tramites_asignados_hab =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals hab.id_tramitetarea
                        join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                        join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                        join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                        join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                        join sol in db.SSIT_Solicitudes on hab.id_solicitud equals sol.id_solicitud
                        where
                        qEquipo.Contains(per.userid)
                        && tt.FechaCierre_tramitetarea == null
                        && sol.id_estado != (int)Constants.Solicitud_Estados.Anulado
                        group tt by new
                        {
                            tt.UsuarioAsignado_tramitetarea,
                            per.Apellido,
                            per.Nombres
                        } into grupo_tt
                        where grupo_tt.Any(tt => tareas_calificar.Contains(tt.id_tarea)) // Excluir los registros que no cumplan la condición
                        
                        select new
                        {
                            userid = grupo_tt.Key.UsuarioAsignado_tramitetarea,
                            apellido = grupo_tt.Key.Apellido,
                            nombre = grupo_tt.Key.Nombres,
                            cant = grupo_tt.Count() // Sumar la cantidad de cada grupo
                        }
                        ).OrderByDescending(r => r.cant).ToList();
                
                var tramites_asignados_Transf =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join transf in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals transf.id_tramitetarea
                        join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                        join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                        join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                        join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                        join sol in db.SSIT_Solicitudes on transf.id_solicitud equals sol.id_solicitud
                        where
                        qEquipo.Contains(per.userid)
                        && tt.FechaCierre_tramitetarea == null
                        && sol.id_estado != (int)Constants.Solicitud_Estados.Anulado
                        group tt by new
                        {
                            tt.UsuarioAsignado_tramitetarea,
                            per.Apellido,
                            per.Nombres
                        } into grupo_tt
                        where grupo_tt.Any(tt => tareas_calificar.Contains(tt.id_tarea)) // Excluir los registros que no cumplan la condición
                        select new
                        {
                            userid = grupo_tt.Key.UsuarioAsignado_tramitetarea,
                            apellido = grupo_tt.Key.Apellido,
                            nombre = grupo_tt.Key.Nombres,
                            cant = grupo_tt.Count() // Sumar la cantidad de cada grupo
                        }
                        ).OrderByDescending(r => r.cant).ToList();
                var tramites_asignados_cpadron =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join cpn in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals cpn.id_tramitetarea
                        join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                        join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                        join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                        join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                        join sol in db.SSIT_Solicitudes on cpn.id_cpadron equals sol.id_solicitud
                        where
                        qEquipo.Contains(per.userid)
                        && tt.FechaCierre_tramitetarea == null
                        && sol.id_estado != (int)Constants.Solicitud_Estados.Anulado
                        group tt by new
                        {
                            tt.UsuarioAsignado_tramitetarea,
                            per.Apellido,
                            per.Nombres
                        } into grupo_tt
                        where grupo_tt.Any(tt => tareas_calificar.Contains(tt.id_tarea)) // Excluir los registros que no cumplan la condición
                        select new
                        {
                            userid = grupo_tt.Key.UsuarioAsignado_tramitetarea,
                            apellido = grupo_tt.Key.Apellido,
                            nombre = grupo_tt.Key.Nombres,
                            cant = grupo_tt.Count() // Sumar la cantidad de cada grupo
                        }
                        ).OrderByDescending(r => r.cant).ToList();

                var tramites_asignados_totales = tramites_asignados_hab
                                                .Concat(tramites_asignados_Transf)
                                                .Concat(tramites_asignados_cpadron)
                                                .GroupBy(t => new { t.userid, t.apellido, t.nombre }) // Agrupe por usuario
                                                .Select(g => new
                                                {
                                                    userid = g.Key.userid,
                                                    apellido = g.Key.apellido,
                                                    nombre = g.Key.nombre,
                                                    cant = g.Sum(item => item.cant) // Suma los conteos de cada grupo
                                                })
                                                .OrderByDescending(r => r.cant) // Ordenar por conteo descendente
                                                .ToList();


                data datos = null;
                foreach (var item in tramites_asignados_totales)
                {
                    datos = new data("", 0);
                    datos.label = item.apellido + ", " + item.nombre;
                    datos.value = item.cant;
                    datos.userid = item.userid.ToString();
                    lista.Add(datos);
                }

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
            }


            return lista;
        }

        [System.Web.Services.WebMethod]
        public static List<data> getDatos_solicitudes_asignadas_califTR(string par1, string par2)
        {
            List<data> lista = new List<data>();
            DGHP_Entities db = null;

            try
            {
                int[] tareas_calificar = getTareasCalificarPorUsuarioTR();

                db = new DGHP_Entities();

                var tramites_asignados =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                        join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                        join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                        join cir in db.ENG_Circuitos on tar.id_circuito equals cir.id_circuito
                        where tareas_calificar.Contains(tt.id_tarea) && tt.FechaCierre_tramitetarea == null
                        group tt by new
                        {
                            t.nombre_tarea,
                            tt.UsuarioAsignado_tramitetarea,
                            per.Apellido,
                            per.Nombres
                        } into grupo_tt
                        orderby grupo_tt.Count() descending
                        select new
                        {
                            nombre_tarea = grupo_tt.Key.nombre_tarea,
                            userid = grupo_tt.Key.UsuarioAsignado_tramitetarea,
                            apellido = grupo_tt.Key.Apellido,
                            nombre = grupo_tt.Key.Nombres,
                            cant = grupo_tt.Count()
                        }
                    ).ToList();

                data datos = null;
                foreach (var item in tramites_asignados)
                {
                    datos = new data("", 0);
                    datos.label = item.apellido + ", " + item.nombre;
                    datos.value = item.cant;
                    datos.userid = item.userid.ToString();
                    lista.Add(datos);
                }

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
            }


            return lista;
        }


        [System.Web.Services.WebMethod]
        public static List<data> getDatos_solicitudes_pendientes_calif(string par1, string par2)
        {
            List<data> lista = new List<data>();
            DGHP_Entities db = null;

            try
            {
                int[] tareas_calificar = getTareasCalificarPorUsuario();
                int[] tareas_asignar = getTareasAsignarPorUsuario();

                db = new DGHP_Entities();

                var tramites_asignados =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        where 
                            tareas_calificar.Contains(tt.id_tarea) &&
                            tt.FechaCierre_tramitetarea == null &&
                            tt.UsuarioAsignado_tramitetarea != null
                        select tt
                    ).Count();

                data datos = null;

                Control ctl = new Control();
                datos = new data("Tr&aacute;mites Asignados", (int)tramites_asignados);
                datos.link_detalle = ctl.ResolveUrl("~/Dashboard/SolicitudesAsignadasCalificar.aspx");
                lista.Add(datos);
                ctl.Dispose();

                var tramites_sin_asignar =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        where
                            tareas_asignar.Contains(tt.id_tarea)
                            && tt.FechaCierre_tramitetarea == null
                            && tt.UsuarioAsignado_tramitetarea == null
                        select tt
                    ).Count();

                datos = new data("Sin Asignar", (int)tramites_sin_asignar);
                datos.link_detalle = "";
                lista.Add(datos);

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
            }

            return lista;
        }

        [System.Web.Services.WebMethod]
        public static List<data> getDatos_solicitudes_pendientes_califTR(string par1, string par2)
        {
            List<data> lista = new List<data>();
            DGHP_Entities db = null;

            try
            {
                int[] tareas_calificar = getTareasCalificarPorUsuarioTR();
                int[] tareas_asignar = getTareasAsignarPorUsuarioTR();

                db = new DGHP_Entities();

                var tramites_asignados =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        where tareas_calificar.Contains(tt.id_tarea) &&
                            tt.FechaCierre_tramitetarea == null &&
                            tt.UsuarioAsignado_tramitetarea != null
                        select tt
                    ).Count();

                data datos = null;

                Control ctl = new Control();
                datos = new data("Tr&aacute;mites Asignados", (int)tramites_asignados);
                datos.link_detalle = ctl.ResolveUrl("~/Dashboard/SolicitudesAsignadasCalificarTR.aspx");
                lista.Add(datos);
                ctl.Dispose();

                var tramites_sin_asignar =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        where tareas_asignar.Contains(tt.id_tarea) && tt.FechaCierre_tramitetarea == null
                        select tt
                    ).Count();

                datos = new data("Sin Asignar", (int)tramites_sin_asignar);
                datos.link_detalle = "";
                lista.Add(datos);

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
            }

            return lista;
        }

        [System.Web.Services.WebMethod]
        public static List<data> getDatos_solicitudes_pendientes(string tareaId)
        {
            List<data> lista = new List<data>();
            DGHP_Entities db = null;

            try
            {
                int[] tareas_calificar = new int[1] { Int32.Parse(tareaId) };
                int[] tareas_asignar = new int[1] { Int32.Parse(tareaId) };

                db = new DGHP_Entities();

                var tramites_asignados =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        where tareas_calificar.Contains(tt.id_tarea) &&
                            tt.FechaCierre_tramitetarea == null &&
                            tt.UsuarioAsignado_tramitetarea != null
                        select tt
                    ).Count();

                data datos = null;

                Control ctl = new Control();
                datos = new data("Tr&aacute;mites Asignados", (int)tramites_asignados);
                datos.link_detalle = ctl.ResolveUrl("~/Dashboard/TareasZonificacionAsignadas.aspx");
                lista.Add(datos);
                ctl.Dispose();

                var tramites_sin_asignar =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        where tareas_asignar.Contains(tt.id_tarea) && tt.FechaCierre_tramitetarea == null
                        select tt
                    ).Count();

                datos = new data("Sin Asignar", (int)tramites_sin_asignar);
                datos.link_detalle = "";
                lista.Add(datos);

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
            }

            return lista;
        }

        [System.Web.Services.WebMethod]
        public static List<data> getDatos_solicitudes_asignadas(string tareaId)
        {
            List<data> lista = new List<data>();
            DGHP_Entities db = null;

            try
            {
                int[] tareas_calificar = new int[1] { Int32.Parse(tareaId) };

                db = new DGHP_Entities();

                var tramites_asignados =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                        join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                        where tareas_calificar.Contains(tt.id_tarea) && tt.FechaCierre_tramitetarea == null
                        group tt by new
                        {
                            tt.id_tarea,
                            t.nombre_tarea,
                            tt.UsuarioAsignado_tramitetarea,
                            per.Apellido,
                            per.Nombres
                        } into grupo_tt
                        orderby grupo_tt.Count() descending
                        select new
                        {
                            nombre_tarea = grupo_tt.Key.nombre_tarea,
                            userid = grupo_tt.Key.UsuarioAsignado_tramitetarea,
                            apellido = grupo_tt.Key.Apellido,
                            nombre = grupo_tt.Key.Nombres,
                            cant = grupo_tt.Count(),
                            id_tarea = grupo_tt.Key.id_tarea
                        }
                    ).ToList();

                data datos = null;
                foreach (var item in tramites_asignados)
                {
                    datos = new data("", 0);
                    datos.label = item.apellido + ", " + item.nombre;
                    datos.value = item.cant;
                    datos.userid = item.userid.ToString();
                    datos.id_tarea = item.id_tarea;
                    lista.Add(datos);
                }

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
            }


            return lista;
        }

        [System.Web.Services.WebMethod]
        public static List<data> getDatos_solicitudes_asignadasTR(string tareaId)
        {
            List<data> lista = new List<data>();
            DGHP_Entities db = null;

            try
            {
                int[] tareas_calificar = new int[1] { Int32.Parse(tareaId) };

                db = new DGHP_Entities();

                var tramites_asignados =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join per in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals per.userid
                        join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                        where tareas_calificar.Contains(tt.id_tarea) && tt.FechaCierre_tramitetarea == null &&
                        t.ENG_Circuitos.id_circuito == (int)Constants.ENG_Circuitos.TRANSF
                        group tt by new
                        {
                            tt.id_tarea,
                            t.nombre_tarea,
                            tt.UsuarioAsignado_tramitetarea,
                            per.Apellido,
                            per.Nombres
                        } into grupo_tt
                        orderby grupo_tt.Count() descending
                        select new
                        {
                            nombre_tarea = grupo_tt.Key.nombre_tarea,
                            userid = grupo_tt.Key.UsuarioAsignado_tramitetarea,
                            apellido = grupo_tt.Key.Apellido,
                            nombre = grupo_tt.Key.Nombres,
                            cant = grupo_tt.Count(),
                            id_tarea = grupo_tt.Key.id_tarea
                        }
                    ).ToList();

                data datos = null;
                foreach (var item in tramites_asignados)
                {
                    datos = new data("", 0);
                    datos.label = item.apellido + ", " + item.nombre;
                    datos.value = item.cant;
                    datos.userid = item.userid.ToString();
                    datos.id_tarea = item.id_tarea;
                    lista.Add(datos);
                }

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
            }


            return lista;
        }
    }
}