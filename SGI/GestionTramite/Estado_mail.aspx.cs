using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite
{
    public partial class Estado_mail : BasePage
    {
        DGHP_Entities db = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CargarEstados();
            }
        }

        public void CargarEstados()
        {

            db = new DGHP_Entities();
            var q = (from dbMailProceso in db.Envio_Mail_Send_Estado
                     select dbMailProceso.Ultima_Ejecucion).ToList();
            txtProceso.Text = Convert.ToString(q.FirstOrDefault());

            var r = (from dbMailEnviado in db.Emails
                     where dbMailEnviado.id_estado == 2 & dbMailEnviado.fecha_envio != null
                     orderby dbMailEnviado.fecha_envio descending
                     select dbMailEnviado.fecha_envio).ToList();
            txtMailEnviado.Text = Convert.ToString(r.FirstOrDefault());

            DateTime dHasta;
            DateTime dDesde;
            dDesde = Convert.ToDateTime(DateTime.Now.Date);
            dHasta = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var s = (from dbMailTotalHot in db.Emails
                     where dbMailTotalHot.fecha_envio >= dDesde &
                     dbMailTotalHot.fecha_envio <= dHasta
                     select dbMailTotalHot.id_email).Count();
            txtEnviadoHoy.Text = s.ToString();
                        
        }
        
    }
}