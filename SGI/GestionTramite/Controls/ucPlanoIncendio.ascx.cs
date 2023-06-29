using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;
using SGI;
using SGI.Model;

namespace SGI.GestionTramite.Controls
{
    public partial class ucPlanoIncendio : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LoadData(int id_tramitetarea, int id_solicitud)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea);
        }

        public void LoadData(int id_grupotramite, int id_solicitud, int id_tramitetarea)
        {
            DGHP_Entities db = new DGHP_Entities();
            var planos = (from th in db.SGI_Tramites_Tareas_HAB
                          join tt in db.SGI_Tramites_Tareas on th.id_tramitetarea equals tt.id_tramitetarea
                          join td in db.SGI_Tarea_Transicion_Enviar_DGFYCO on tt.id_tramitetarea equals td.id_tramitetarea
                          join tp in db.SGI_Tipos_Planos_Incendio on td.id_tipoplanoincendio equals tp.id_tipoplanoincendio
                          join t in db.ENG_Tareas on tt.id_tarea equals t.id_tarea
                          join u in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals u.userid
                          where th.id_solicitud == id_solicitud && th.id_tramitetarea < id_tramitetarea
                          select new
                          {
                              id_planoincendio = tp.id_tipoplanoincendio,
                              nombre_tarea = t.nombre_tarea,
                              nombre_plano = tp.nombre_tipoplanoincendio,
                              fechaPlano = tt.FechaCierre_tramitetarea,
                              UsuarioApeNom = u.Apellido + ", " + u.Nombres
                          }).ToList();
            grdPlanoIncendio.DataSource = planos;
            grdPlanoIncendio.DataBind();
        }

        public class PlanosIncendio
        {
            public PlanosIncendio()
            {
                this.Item = new List<Items>();
            }

            #region atributos

            public int ID { get; set; }
            public int id_tipoplanoincendio { get; set; }
            public string nombre_plano { get; set; }
            public int id_tramitetarea { get; set; }
            public string Descripcion { get; set; }
            public List<Items> Item { get; set; }
            public string UsuarioApeNom { get; set; }
            public DateTime Fecha { get; set; }

            #endregion

        }
    }

}