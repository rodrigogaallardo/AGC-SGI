using System;
using System.Linq;
using System.Web.UI;
using System.Web.Security;
using SGI.Model;
using System.Transactions;


namespace SGI.Account
{
    public partial class EditUser : System.Web.UI.Page
    {

        #region carga inicial

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                try
                {
                    CargarDatos();
                    mostrarPanelResultado(false, "");
                    mostrarPanelError(false, "");
                    mostrarPanelEditarDatos(true);
                }
                catch (Exception ex)
                {
                    mostrarPanelError(true, ex.Message);
                }
            }
        }


        private void CargarDatos()
        {

            MembershipUser usuario = Membership.GetUser();
            Guid userId = (Guid)usuario.ProviderUserKey;

            DGHP_Entities db = new DGHP_Entities();
            SGI_Profiles profile = (from prof in db.SGI_Profiles
                                     where prof.userid == userId
                                     select prof).FirstOrDefault();

            UserName.Text = usuario.UserName;
            Email.Text = usuario.Email;
            Apellido.Text = profile.Apellido;
            Nombre.Text = profile.Nombres;

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

        private void mostrarPanelEditarDatos(bool mostrar)
        {
            pnlEditarDatos.Visible = mostrar;
        }

        #endregion

        protected void ActualizarUsuario(object sender, EventArgs e)
        {
            try
            {
                mostrarPanelError(false, "");
                mostrarPanelResultado(false, "");
                
                ValidarGuardarUsuario();
                guardarUsuario();

                mostrarPanelEditarDatos(false);
                mostrarPanelResultado(true, "Los cambios se han realizado exitosamente.");
            }
            catch (Exception ex)
            {
                mostrarPanelError(true, ex.Message);
            }

        }

        private void ValidarGuardarUsuario()
        {
        }

        private void guardarUsuario()
        {
            
            if (! ModelState.IsValid)
            {
                return;
            }

            MembershipUser usuario = Membership.GetUser();

            DGHP_Entities db = new DGHP_Entities();

            Guid userId = (Guid)usuario.ProviderUserKey;

            string vEmail = Email.Text;
            string vApellido = Apellido.Text.Trim();
            string vNombre = Nombre.Text.Trim();

            aspnet_Users asp_user = db.aspnet_Users.FirstOrDefault(x => x.UserId == userId);

            TransactionScope Tran = new TransactionScope();

            try
            {
                db.SGI_Profiles_update(userId, vApellido, vNombre, asp_user.SGI_Profiles.UserName_SADE, null, asp_user.SGI_Profiles.Reparticion_SADE, userId, asp_user.SGI_Profiles.Sector_SADE, null);
                db.SaveChanges();

                if (!vEmail.Equals(usuario.Email))
                {
                    usuario.Email = vEmail;
                    Membership.UpdateUser(usuario);
                }

                Tran.Complete();
                Tran.Dispose();
            }
            catch (Exception ex)
            {
                if ( Tran != null ) 
                    Tran.Dispose();
                if ( db != null ) 
                    db.Dispose();
                LogError.Write(ex, "error en transaccion. editar acount-guardarUsuario");
                throw ex;
            }

            
            db.Dispose();

        }
    
    }

}