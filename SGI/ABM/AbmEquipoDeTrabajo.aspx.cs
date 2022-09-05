using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using SGI.Controls;

namespace SGI
{
    public partial class AbmEquipoDeTrabajo : System.Web.UI.Page
    {

        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm.IsInAsyncPostBack)
                ScriptManager.RegisterStartupScript(updPnlAgregar, updPnlAgregar.GetType(), "init_js", "init_js();", true);

            if (!IsPostBack)
            {
                try
                {
                    mostrarPanelError(false, "");
                    mostrarPanelResultado(false, "");
                    limpiarBuscarEmpleado();
                    db = new DGHP_Entities();
                    CargarDatos();
                    db.Dispose();

                    SiteMaster pmaster = (SiteMaster)this.Page.Master;
                    ucMenu mnu = (ucMenu)pmaster.FindControl("mnu");
                    mnu.setearMenuActivo(4);
                }
                catch (Exception ex)
                {
                    if ( db != null )
                        db.Dispose();
                    mostrarPanelError(true, ex.Message);
                }

            }

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


        DGHP_Entities db = null;

        private void CargarDatos()
        {
            // cargar personal a cargo
            MembershipUser usuario = Membership.GetUser();

            if (usuario != null)
            {


                Guid userId = (Guid)usuario.ProviderUserKey;
                hid_userid.Value = userId.ToString();

                // Roles del usuario
                List<SGI_Perfiles> lst_perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userId).SGI_PerfilesUsuarios.ToList();

                // Obtiene de la configuracion de la bandeja el rol que posee el usuario logueado (intersección)
                List<int> perfilesAsignador = ((from config_bandeja_asig in db.ENG_Config_BandejaAsignacion
                                         select config_bandeja_asig.id_perfil_asignador.Value)
                                         .Intersect(from perfiles_usuario in lst_perfiles_usuario select perfiles_usuario.id_perfil)).ToList();
                hid_RoleId_Asignado.Value = "0";
                if (perfilesAsignador.Count > 0)
                {
                    //Obtiene de la configuracion de la bandeja el rol del empleado para filtar los empleados que tengan dicho rol.
                    List<int> rolesEmpleado = (from config_bandeja_asig in db.ENG_Config_BandejaAsignacion
                                              where perfilesAsignador.Contains(config_bandeja_asig.id_perfil_asignador.Value)
                                              select config_bandeja_asig.id_perfil_asignado.Value).Distinct().ToList();
                    foreach (int perfil in rolesEmpleado)
                    {
                        hid_RoleId_Asignado.Value += ","+perfil.ToString();
                    }
                }
                CargarGrillaEquipoAsignado(userId);

                cargarComboEmpleados();

            }

        }

        public class clsEquipoTrabajo
        {
            public string userid { get; set; }
            public string nombres_apellido { get; set; }
            public string tramites { get; set; }
        }

        private void cargarComboEmpleados()
        {
            MembershipUser usuario = Membership.GetUser();
            Guid userId = (Guid)usuario.ProviderUserKey;
            int? perfil_asignado;
            List<clsEquipoTrabajo> elements = new List<clsEquipoTrabajo>();
            List<int> perfiles = new List<int>();
            string[] valores= hid_RoleId_Asignado.Value.Split(',');
            for (int i = 0; i < valores.Length;i++ ){
                perfil_asignado = int.Parse(valores[i]);
                if(perfil_asignado!=0)
                {

                    SGI_Perfiles RolFiltro = db.SGI_Perfiles.FirstOrDefault(x => x.id_perfil == perfil_asignado);

                    var Empleados =
                                    from usuarios in db.aspnet_Users
                                    join member in db.aspnet_Membership on usuarios.UserId equals member.UserId
                                    join datos_usuario in db.SGI_Profiles on usuarios.UserId equals datos_usuario.userid
                                    where member.IsApproved
                                    orderby datos_usuario.Apellido
                                    select new
                                    {
                                        Roles = usuarios.SGI_PerfilesUsuarios,
                                        Nombres = datos_usuario.Nombres,
                                        Apellido = datos_usuario.Apellido,
                                        userid = usuarios.UserId
                                    };

                    foreach (var empleado in Empleados)
                    {
                        if (empleado.Roles.Contains(RolFiltro))
                        {
                            if (!PerteneceAEquipoAsignado(userId, empleado.userid))
                            {
                                clsEquipoTrabajo item = new clsEquipoTrabajo();
                                item.userid = empleado.userid.ToString().ToUpper();
                                item.nombres_apellido = empleado.Nombres + " " + empleado.Apellido;
                                if (!Contains(elements,item))
                                    elements.Add(item);
                            }
                        }
                    }
                }
            }
            
            hid_ddlEmpleadoB.Value = string.Join(",", elements.Select(x => x.userid).ToArray());

            ddlEmpleado.DataValueField = "userid";
            ddlEmpleado.DataTextField = "nombres_apellido";
            ddlEmpleado.DataSource = elements;
            ddlEmpleado.DataBind();
        }

        private bool Contains(List<clsEquipoTrabajo> elements, clsEquipoTrabajo elm)
        {
            foreach (clsEquipoTrabajo e in elements)
                if (e.userid == elm.userid)
                    return true;
            return false;
        }

        private void CargarGrillaEquipoAsignado(Guid userId)
        {
            List<EquipoTrabajoAsignado> list_equipo = BuscarEquipoAsignado(userId);

            grdEmpleado.DataSource = list_equipo;
            grdEmpleado.DataBind();
        }

        private bool PerteneceAEquipoAsignado(Guid userResponsableId, Guid Userid)
        {
            var q =
                (
                    from equipo in db.ENG_EquipoDeTrabajo
                    join prof in db.SGI_Profiles on equipo.Userid equals prof.userid
                    where equipo.Userid_Responsable == userResponsableId
                    && equipo.Userid == Userid
                    orderby prof.Apellido
                    select new EquipoTrabajoAsignado
                    {
                        id_equipo_trabajo = equipo.id_equipotrabajo,
                        userid = equipo.Userid,
                        Apellido = prof.Apellido,
                        Nombres = prof.Nombres
                    }

                ).ToList();

              return q.Count()>0;
        }


        private List<EquipoTrabajoAsignado> BuscarEquipoAsignado(Guid userId)
        {
            List<EquipoTrabajoAsignado> list_equipo= null;

            var q =
                (
                    from equipo in db.ENG_EquipoDeTrabajo
                    join prof in db.SGI_Profiles on equipo.Userid equals prof.userid
                    where equipo.Userid_Responsable == userId
                    orderby prof.Apellido
                    select new EquipoTrabajoAsignado
                    {
                        id_equipo_trabajo = equipo.id_equipotrabajo,
                        userid = equipo.Userid,
                        Apellido =  prof.Apellido,
                        Nombres = prof.Nombres
                    }

                ).ToList();

            list_equipo = q.ToList();

            return list_equipo;
        }

        #endregion

        #region Acciones Eliminar y agregar

        private void limpiarBuscarEmpleado()
        {
            txtEmpleadoValidar.Text = "";
            hid_ddlEmpleadoB.Value = "";
            //txtEmpleado.Text = "";
        }

        protected void Eliminar_Empleado_Command(object sender, CommandEventArgs e)
        {
            TransactionScope Tran = null;
            try
            {
                mostrarPanelError(false, "");
                mostrarPanelError(false, "");

                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;

                LinkButton lnkEditar = (LinkButton)sender;

                int id_equipo = int.Parse(lnkEditar.CommandArgument);

                db = new DGHP_Entities();
                Tran = new TransactionScope();

                try
                {
                    db.ENG_EquipoDeTrabajo_delete(id_equipo);

                    db.SaveChanges();

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    if (Tran != null)
                        Tran.Dispose();
                    LogError.Write(ex, "error en transaccion. Eliminar usuario al equipo de trabajo");
                    throw ex;
                }
	

                CargarGrillaEquipoAsignado(userid);
                cargarComboEmpleados();
                db.Dispose();

            }
            catch (Exception ex)
            {
                if (db != null)
                    db.Dispose();
                mostrarPanelError(true, ex.Message);
            }

            ScriptManager.RegisterStartupScript(updPnlEmpleado, updPnlEmpleado.GetType(),
                "cargarScript", "EjecutarScript();", true);

        }

        private void ValidarAgregarUsuario(Guid userIdResponsable)
        {
            if ( string.IsNullOrEmpty(hid_ddlEmpleadoB.Value) )
                throw new Exception("Debe seleccionar empleados.");
        }

        protected void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            TransactionScope Tran = null;

            Guid userIdEmpleado; 

            try
            {
                mostrarPanelError(false, "");
  
                Guid userIdResponsable = (Guid)Membership.GetUser().ProviderUserKey;

                db = new DGHP_Entities();

               ValidarAgregarUsuario(userIdResponsable);

                string[] lstUsuario = hid_ddlEmpleadoB.Value.Split(',');

                Tran = new TransactionScope();

                try
                {
                    foreach (string strUsuario in lstUsuario)
                    {
                        Guid.TryParse(strUsuario, out userIdEmpleado);

                        ENG_EquipoDeTrabajo equipo = db.ENG_EquipoDeTrabajo.FirstOrDefault(
                                            x => x.Userid == userIdEmpleado &&
                                            x.Userid_Responsable == userIdResponsable);
                        if (equipo == null)
                            db.ENG_EquipoDeTrabajo_insert(userIdEmpleado, userIdResponsable, userIdResponsable.ToString());
                    }

                    db.SaveChanges();

                    Tran.Complete();
                    Tran.Dispose();
                }
                catch (Exception ex)
                {
                    if ( Tran != null ) 
                        Tran.Dispose();
                    LogError.Write(ex, "Error en transaccion. abmEquipoTrabajo-btnAgregarUsuario_Click");
                    throw ex;
                }
                              
                CargarGrillaEquipoAsignado(userIdResponsable);
                cargarComboEmpleados();

                db.Dispose();

            }
            catch (Exception ex)
            {
                if (Tran != null)
                    Tran.Dispose();
                if (db != null)
                    db.Dispose();
                LogError.Write(ex, "error en transaccion. Agregar usuario al equipo");
                mostrarPanelError(true, ex.Message);
            }

            ScriptManager.RegisterStartupScript(updPnlAgregar, updPnlAgregar.GetType(),
                "cargarScript", "EjecutarScript();", true);

        }

   
        #endregion

    }

    
}

public class EquipoTrabajoAsignado
{
    public int id_equipo_trabajo { get; set; }
    public Guid userid { get; set; }
    public string Apellido { get; set; }
    public string Nombres { get; set; }

}
