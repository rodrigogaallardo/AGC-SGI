using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGI.Model;
using SGI.BusinessLogicLayer.Enums;
using SGI.BusinessLogicLayer.Constants;
using SGI.DataLayer;
using SGI.DataLayer.Models;

namespace SGI.BusinessLogicLayer
{
    public class TramitesBLL
    {
        protected static DGHP_Entities db = null;
        protected static AGC_FilesEntities dbFiles = null;

        #region Entidades
        protected static void IniciarEntity()
        {
            if (db == null)
            {
                db = new DGHP_Entities();
            }
        }

        protected static void FinalizarEntity()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }

        protected static void IniciarEntityFiles()
        {
            if (dbFiles == null)
            {
                dbFiles = new AGC_FilesEntities();
            }
        }

        protected static void FinalizarEntityFiles()
        {
            if (dbFiles != null)
            {
                dbFiles.Dispose();
            }
        }
        #endregion

        public static bool NotificarCaducaTramite(int id_solicitud,out string errorMessage)
        {
            bool notifico = false;
            errorMessage = string.Empty;

            IniciarEntity();

            try
            {
                using (DGHP_Entities db = new DGHP_Entities())
                {
                    var solicitud = (from st in db.SSIT_Solicitudes
                                     where st.id_solicitud == id_solicitud
                                     select new SSIT_Solicitudes_Model
                                     {
                                         id_solicitud = id_solicitud,
                                         id_estado = st.id_estado
                                     }
                                     ).FirstOrDefault();

                    if (solicitud == null) { notifico = false; throw new Exception(ErrorConstants.ERROR_SOLICITUD_NO_EXISTE); }
                    if (solicitud.id_estado != (int)TipoEstadoSolicitudEnum.Caduco) { notifico = false; throw new Exception(ErrorConstants.ERROR_SOLICITUD_NO_CADUCO); }

                    var solicitudesCaducasNotificadas = (from st in db.SSIT_Solicitudes
                                                        join SSN in db.SSIT_Solicitudes_Notificaciones on st.id_solicitud equals SSN.id_solicitud
                                                        where st.id_solicitud == id_solicitud
                                                        && SSN.Id_NotificacionMotivo == 6
                                                         select new SSIT_Solicitudes_Model
                                                         {
                                                             id_solicitud = id_solicitud,
                                                             id_estado = st.id_estado
                                                         }).ToList<SSIT_Solicitudes_Model>();

                    if (solicitudesCaducasNotificadas.Count == 0)
                    {
                        List<string> emailsNotificados = Mailer.MailMessages.SendMail_Caducidad_v2(solicitud.id_solicitud);
                        errorMessage = "La solicitud fue enviada con éxito a los siguientes emails: " + String.Join(", ", emailsNotificados);
                        notifico = true;
                    }
                    else
                    {
                        errorMessage = ErrorConstants.ERROR_SOLICITUD_NOTIFICADA;
                        notifico = false;
                        throw new Exception(errorMessage);
                    }
            }
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            notifico = false;
            throw new Exception(errorMessage);
        }
            return notifico;
        }
    }
}