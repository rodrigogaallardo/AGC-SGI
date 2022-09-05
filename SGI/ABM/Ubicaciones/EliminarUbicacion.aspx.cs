﻿using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.ABM.Ubicaciones
{
    public partial class EliminarUbicacion : BasePage
    {
        #region Var
        int idUbicacion = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.idUbicacion = Convert.ToInt32(Request.QueryString["Id"].ToString());
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                LoadComentariosByUbicacion(this.idUbicacion);

                this.EjecutarScript(updDatos, "finalizarCarga();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updDatos, "finalizarCarga();showfrmError();");
            }
        }

        private void LoadComentariosByUbicacion(int idUbicacion)
        {
            using (var db = new DGHP_Entities())
            {
                var u = (from ubi in db.Ubicaciones
                         where ubi.id_ubicacion == idUbicacion
                         select ubi).FirstOrDefault();

                txtObservaciones.Text = u.Observaciones == null ? string.Empty : u.Observaciones.ToString();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ABM/Ubicaciones/AbmUbicaciones.aspx");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            using (var context = new DGHP_Entities())
            {
                var idUbicacion = Convert.ToInt32(Request.QueryString["Id"].ToString());

                var entity = context.Ubicaciones.Where(x => x.id_ubicacion == idUbicacion).Select(x => x).FirstOrDefault();
                entity.baja_logica = true;
                entity.Observaciones = txtObservaciones.Text;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateUser = Functions.GetUserId();
                context.Ubicaciones.Attach(entity);
                context.Entry(entity).State = System.Data.Entity.EntityState.Modified;

                var Ph = context.Ubicaciones_PropiedadHorizontal.Where(x => x.id_ubicacion == entity.id_ubicacion).Select(x => x.id_propiedadhorizontal).ToList();

                foreach (var id_partidahorizontal in Ph)
                {
                    context.Ubicaciones_PropiedadHorizontal_delete(id_partidahorizontal);
                }
                context.SaveChanges();
                Response.Redirect("~/ABM/Ubicaciones/AbmUbicaciones.aspx");
            }
        }
    }
}