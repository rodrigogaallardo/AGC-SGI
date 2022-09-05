using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;

namespace SGI.Mailer
{
    public partial class MailSolicitudNuevaPuerta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public SGI.Model.MailSolicitudNuevaPuerta GetData()
        {

            object puserid = Request.QueryString["userid"];
            object pid_ubicacion = Request.QueryString["id_ubicacion"];
            
            if (puserid != null && pid_ubicacion != null)
            {
                Guid userid = Guid.Parse(puserid.ToString());
                int id_ubicacion = Convert.ToInt32(pid_ubicacion);

                DGHP_Entities db = new DGHP_Entities();

                var datos_usuario = (from usu in db.aspnet_Users
                                     join mem in db.aspnet_Membership on usu.UserId equals mem.UserId
                                     join profile in db.SGI_Profiles on usu.UserId equals profile.userid
                                     where usu.UserId == userid
                                     select new {
                                         usu.UserName,
                                         mem.Email,
                                         profile.Apellido,
                                         profile.Nombres
                                     }).FirstOrDefault();
                
                            
                var datos_ubicacion = db.Ubicaciones.FirstOrDefault(x=> x.id_ubicacion == id_ubicacion);

                SGI.Model.MailSolicitudNuevaPuerta result = new SGI.Model.MailSolicitudNuevaPuerta();

                if(datos_usuario != null)
                {
                    result.Username = datos_usuario.UserName;
                    result.Email = datos_usuario.Email;
                    result.Apellido = datos_usuario.Apellido;
                    result.Nombre = datos_usuario.Nombres;
                }
                if (datos_ubicacion != null)
                {
                    result.NroPartidaMatriz = datos_ubicacion.NroPartidaMatriz;
                    result.Seccion = datos_ubicacion.Seccion;
                    result.Manzana = datos_ubicacion.Manzana;
                    result.Parcela = datos_ubicacion.Parcela;

                    var puerta = (from puer in db.Ubicaciones_Puertas
                                  join calle in db.Calles on puer.codigo_calle equals calle.Codigo_calle
                                  where puer.id_ubicacion == id_ubicacion
                                  select new
                                  {
                                      calle.NombreOficial_calle,
                                      puer.NroPuerta_ubic
                                  }).FirstOrDefault();

                    if (puerta != null)
                    {
                        string direccion = string.Format("{0} {1}", puerta.NombreOficial_calle, puerta.NroPuerta_ubic);
                        result.UrlMapa = SGI.Functions.GetUrlMapa(result.Seccion.ToString(), result.Manzana, result.Parcela, direccion);
                    }

                    result.urlFoto = SGI.Functions.GetUrlFoto(result.Seccion.ToString(), result.Manzana, result.Parcela);
                }
               
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}