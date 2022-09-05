using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using SGI.Model;

namespace SGI.Account
{
    public partial class PerfilesAsignados : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                try
                {
                    mostrarPanelError(false, "");
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    mostrarPanelError(true, ex.Message);
                }
            }
        }


        private void CargarDatos()
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;

            MembershipProvider mem = null;
            mem = Membership.Provider;
            string appName = mem.ApplicationName;

            DGHP_Entities db = new DGHP_Entities();

            List<ENG_Tareas> list_tarea = new List<ENG_Tareas>();

            List<clsItemPerfilesFunciones> resultado = new List<clsItemPerfilesFunciones>();
            //var list_perfil = perfiles.GetUserIDPerfiles(userId);
            aspnet_Users usuario = (from usr in db.aspnet_Users
                                    where usr.UserId == userId
                                    select usr).FirstOrDefault();

            //var list_perfil = usuario.SGI_PerfilesUsuarios.ToList();

            List<clsUsuariosSade> q = (from usr in db.aspnet_Users.Where(x => x.UserId == userId)
                     join profile in db.SGI_Profiles on usuario.UserId equals profile.userid
                     select new clsUsuariosSade()
                     {
                         usuario_sade = profile.UserName_SADE,
                         reparticion_sade = profile.Reparticion_SADE,
                     }).ToList();

            grdSade.DataSource = q;
            grdSade.DataBind();

        

            List<clsItemPerfilesFunciones> list_perfil = (from usr in db.aspnet_Users.Where(x => x.UserId == userId)
                                                          from usu in usr.SGI_PerfilesUsuarios
                                                          select new clsItemPerfilesFunciones()
                                                          {
                                                              id_perfil = usu.id_perfil,
                                                              nombre_perfil = usu.nombre_perfil,
                                                              descripcion_perfil = usu.descripcion_perfil,
                                                              menues_perfil = "",
                                                          }).ToList();

            foreach (clsItemPerfilesFunciones perfil in list_perfil)
            {


                var q2 = (from men in db.SGI_Menues
                          where men.SGI_Perfiles.Any(x => x.id_perfil == perfil.id_perfil) && !men.pagina_menu.StartsWith("~/")
                          select new clsItemPerfilesFunciones
                          {
                              id_menu = men.id_menu,
                              id_menu_padre = (int)men.id_menu_padre,
                              id_menu_abuelo = 9999,
                              menu_hijo = men.descripcion_menu,
                              menu_padre = "",
                              menu_abuelo = "",
                          });
                int[] q2array = q2.Select(x => (int)x.id_menu_padre).Union(q2.Select(x => x.id_menu)).ToArray();

                var q3 = (from res in q2
                          from men2 in db.SGI_Menues
                          where men2.SGI_Perfiles.Any(x => x.id_perfil == perfil.id_perfil) && men2.id_menu == res.id_menu_padre
                          select new clsItemPerfilesFunciones
                          {
                              id_menu = 9999,
                              id_menu_padre = men2.id_menu,
                              id_menu_abuelo = (int)men2.id_menu_padre,
                              menu_hijo = "",
                              menu_padre = men2.descripcion_menu + " / " + res.menu_hijo,
                              menu_abuelo = "",
                          }).Union

                           (from men1 in db.SGI_Menues.Where(y => !q2array.Contains(y.id_menu))
                            where men1.SGI_Perfiles.Any(x => x.id_perfil == perfil.id_perfil) && men1.pagina_menu.StartsWith("~/")
                                //&& men1.id_menu != res.id_menu && men1.id_menu != res.id_menu_padre
                                    && men1.id_menu_padre.HasValue
                            select new clsItemPerfilesFunciones
                            {
                                id_menu = 9999,
                                id_menu_padre = men1.id_menu,
                                id_menu_abuelo = (int)men1.id_menu_padre,
                                menu_hijo = "",
                                menu_padre = men1.descripcion_menu,
                                menu_abuelo = "",
                            });

                int[] q3array = q3.Select(x => (int)x.id_menu_abuelo).Union(q3.Select(x => (int)x.id_menu_padre)).ToArray();

                var q4 = (from res2 in q3
                          from men3 in db.SGI_Menues
                          where men3.id_menu == res2.id_menu_abuelo
                          select new clsItemPerfilesFunciones
                          {
                              id_menu = 9999,
                              id_menu_padre = 9999,
                              id_menu_abuelo = 9999,
                              menu_hijo = "",
                              menu_padre = "",
                              menu_abuelo = men3.descripcion_menu.StartsWith("Administración") ? ("Operaciones / " + men3.descripcion_menu + " / " + res2.menu_padre) :
                                            men3.descripcion_menu.StartsWith("Buscador de Pagos") ? ("Operaciones / Administración de Pagos / " + men3.descripcion_menu + " / " + res2.menu_padre) :
                                            (men3.descripcion_menu + " / " + res2.menu_padre),
                          }).Union

                           (from men1 in db.SGI_Menues.Where(y => !q3array.Contains((int)y.id_menu_padre) && !q3array.Contains(y.id_menu))
                            where men1.SGI_Perfiles.Any(x => x.id_perfil == perfil.id_perfil) && men1.pagina_menu.StartsWith("~/")
                                //&& men1.id_menu != res.id_menu && men1.id_menu != res.id_menu_padre
                                    && !men1.id_menu_padre.HasValue
                            select new clsItemPerfilesFunciones
                            {
                                id_menu = 9999,
                                id_menu_padre = 9999,
                                id_menu_abuelo = 9999,
                                menu_hijo = "",
                                menu_padre = "",
                                menu_abuelo = men1.descripcion_menu,
                            }).OrderBy(x => x.menu_abuelo);

                //BafycoPerfilesDTO resultado = new BafycoPerfilesDTO();

                perfil.menues_perfil = string.Join("\r\n<br />", q4.Select(x => x.menu_abuelo));

                ENG_Tareas tarea = (from tar in db.ENG_Tareas
                                    join perfil_tarea in db.ENG_Rel_Perfiles_Tareas on tar.id_tarea equals perfil_tarea.id_tarea
                                    where perfil_tarea.id_perfil == perfil.id_perfil
                                    select tar).FirstOrDefault();
                if (tarea != null)
                    list_tarea.Add(tarea);

            }

            //List<ENGTareasDTO> list_tareas = new List<ENGTareasDTO>();
            //ENGTareasDTO tarea;

            //List<BafycoMenuesDTO> list_funciones = new List<BafycoMenuesDTO>();
            //BafycoMenuesDTO funcion;

            //foreach (var perfil in list_perfil)
            //{
            //   tarea = perfiles.GetPerfilIDTareas(perfil.id_perfil);
            // if (tarea != null)//Evita errores en el contenedor de datos
            //   list_tareas.Add(tarea);

            //    funcion = perfiles.GetUserIDFunciones(perfil.id_perfil).FirstOrDefault();
            //    if (funcion != null)
            //        list_funciones.Add(funcion);
            //}

            //var list_roles = roles.GetByUserId(userId);

            //
            grdPerfiles.DataSource = list_perfil;
            grdPerfiles.DataBind();

            //grdTareas.DataSource = list_tareas.ToList();
            //grdTareas.DataBind();

            grdFunciones.DataSource = list_perfil;
            grdFunciones.DataBind();


            //grdTareas.DataSource = list_tarea;
            //grdTareas.DataBind();

            db.Dispose();




        }

        private void mostrarPanelResultado(bool mostrar, string mensaje)
        {
            pnlResultado.Visible = mostrar;
            if (!string.IsNullOrEmpty(mensaje))
                lblMensajeResultado.Text = mensaje;

        }


        private void mostrarPanelError(bool mostrar, string mensaje)
        {
            pnlError.Visible = mostrar;
            if (!string.IsNullOrEmpty(mensaje))
                lblMensajeError.Text = mensaje;
        }


    }
}