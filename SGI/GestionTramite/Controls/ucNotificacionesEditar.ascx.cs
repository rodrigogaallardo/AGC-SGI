using SGI.Model;
using System;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucNotificacionesEditar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region Entity
        DGHP_Entities db = null;
        private void IniciarEntity()
        {
            if (db == null)
            {
                this.db = new DGHP_Entities();
                this.db.Database.CommandTimeout = 300;
            }
        }
        private void FinalizarEntity()
        {
            if (db != null)
                db.Dispose();
        }
        #endregion

      

    }
}