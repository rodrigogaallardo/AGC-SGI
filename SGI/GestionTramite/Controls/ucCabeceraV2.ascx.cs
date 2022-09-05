using System;
using System.Linq;
using SGI.Model;
using System.Transactions;

namespace SGI.GestionTramite.Controls
{
    public partial class ucCabeceraV2 : System.Web.UI.UserControl
    {
        public void LoadData(int id_solicitud)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud);
        }

        public void LoadData(int id_grupotramite, int id_solicitud)
        {
            DGHP_Entities db = new DGHP_Entities();
            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                var objsol = (from sol in db.SSIT_Solicitudes
                         where sol.id_solicitud.Equals(id_solicitud)
                         select  new
                         {
                             sol.id_solicitud,
                             DescripcionEstadoSolicitud = sol.TipoEstadoSolicitud.Descripcion                             
                         }).FirstOrDefault();
                


                lblSolicitud.Text = objsol.id_solicitud.ToString();
                lblEstado.Text = objsol.DescripcionEstadoSolicitud;     
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
            {
                var objsol = (from sol in db.CPadron_Solicitudes
                              join estado in db.CPadron_Estados on sol.id_estado equals estado.id_estado
                              where sol.id_cpadron.Equals(id_solicitud)
                              select new
                              {
                                  sol.id_cpadron,
                                  DescripcionEstadoSolicitud = estado.nom_estado_interno
                              }).FirstOrDefault();
                lblSolicitud.Text = objsol.id_cpadron.ToString();
                lblEstado.Text = objsol.DescripcionEstadoSolicitud;  
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                var objsol = (from sol in db.Transf_Solicitudes
                              where sol.id_solicitud.Equals(id_solicitud)
                              select new
                              {
                                  sol.id_solicitud,
                                  sol.id_cpadron,
                                  DescripcionEstadoSolicitud = sol.TipoEstadoSolicitud.Descripcion
                              }).FirstOrDefault();
                lblSolicitud.Text = objsol.id_solicitud.ToString();
                lblEstado.Text = objsol.DescripcionEstadoSolicitud;               

            }
            db.Dispose();            
        }

    }

    
}