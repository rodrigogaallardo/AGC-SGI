using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGI.Model;
using SGI.BusinessLogicLayer.Enums;
using SGI.BusinessLogicLayer.Constants;
using SGI.DataLayer;
using SGI.DataLayer.Models;
using System.ServiceModel.Security;
using ExcelLibrary.BinaryFileFormat;
using SGI.StaticClassNameSpace;
using ExcelLibrary.BinaryFileFormat;

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

        public static bool NotificarTramite(int id_solicitud, int IdNotificacionMotivo, DateTime fechaNotificacion, out string errorMessage, string asunto, string mensaje)
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
                                     ).Union(from tr in db.Transf_Solicitudes
                                             where tr.id_solicitud == id_solicitud
                                             select new SSIT_Solicitudes_Model
                                             {
                                                 id_solicitud = id_solicitud,
                                                 id_estado = tr.id_estado
                                             }).FirstOrDefault();

                    if (solicitud == null) { notifico = false; throw new Exception(ErrorConstants.ERROR_SOLICITUD_NO_EXISTE); }



                    
                    switch (IdNotificacionMotivo)
                    {
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.InicioHabilitacion:
                            //NO SE VALIDA
                            break;
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.Observado:
                            if (solicitud.id_estado != (int)TipoEstadoSolicitudEnum.Observado)  
                            {
                                notifico = false;
                                throw new Exception(ErrorConstants.ERROR_SOLICITUD_NO_OBSERVADO);
                            }
                            break;
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.Rechazado:
                            if (solicitud.id_estado != (int)TipoEstadoSolicitudEnum.Rechazada)  
                            {
                                notifico = false;
                                throw new Exception(ErrorConstants.ERROR_SOLICITUD_NO_RECHAZADO);
                            }
                            break;
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.Aprobado:
                            if (solicitud.id_estado != (int)TipoEstadoSolicitudEnum.Aprobada)  
                            {
                                notifico = false;
                                throw new Exception(ErrorConstants.ERROR_SOLICITUD_NO_APROBADO);
                            }
                            break;
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.ProximoACaducar:
                            //NO SE VALIDA
                            break;
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.AvisoDeCaducidad:
                            if (solicitud.id_estado != (int)TipoEstadoSolicitudEnum.Caduco) 
                            {
                                notifico = false;
                                throw new Exception(ErrorConstants.ERROR_SOLICITUD_NO_CADUCO);
                            }
                            break;
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.BajaDeSolicitud:
                            if (solicitud.id_estado != (int)TipoEstadoSolicitudEnum.Baja)  
                            {
                                notifico = false;
                                throw new Exception(ErrorConstants.ERROR_SOLICITUD_NO_BAJA);
                            }
                            break;
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.QRDisponible:
                            //NO SE VALIDA
                            break;
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.AnexoTecnicoAnulado:
                            //NO SE VALIDA
                            break;
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.SolicitudConfirmado:
                            //NO SE VALIDA
                            break;
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.LevantamientoDeRechazo:
                            //NO SE VALIDA
                            break;
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.AsignadoAlCalificador:
                            //NO SE VALIDA
                            break;
                        case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.Otras:
                            //NO SE VALIDA
                            break;
                        default:
                            break;
                    }
     
                    var solicitudesNotificadas = (from st in db.SSIT_Solicitudes
                                                 join SSN in db.SSIT_Solicitudes_Notificaciones on st.id_solicitud equals SSN.id_solicitud
                                                 where st.id_solicitud == id_solicitud 
                                                 && SSN.Id_NotificacionMotivo == IdNotificacionMotivo
                                                 && SSN.createDate.Year == DateTime.Now.Year
                                                 && SSN.createDate.Month == DateTime.Now.Month
                                                 && SSN.createDate.Day == DateTime.Now.Day
                                                 select new SSIT_Solicitudes_Model
                                                 {
                                                     id_solicitud = id_solicitud,
                                                     id_estado = st.id_estado
                                                 }).Union(from tr in db.Transf_Solicitudes
                                                          join trn in db.Transf_Solicitudes_Notificaciones on tr.id_solicitud equals trn.id_solicitud
                                                          where tr.id_solicitud == id_solicitud
                                                          && trn.Id_NotificacionMotivo == IdNotificacionMotivo
                                                          && trn.createDate.Year == DateTime.Now.Year
                                                          && trn.createDate.Month == DateTime.Now.Month
                                                          && trn.createDate.Day == DateTime.Now.Day
                                                          select new SSIT_Solicitudes_Model
                                                          {
                                                              id_solicitud = id_solicitud,
                                                              id_estado = tr.id_estado
                                                          }).ToList<SSIT_Solicitudes_Model>();

                    if (solicitudesNotificadas.Count == 0)
                    {
                        List<string> emailsNotificados;
                        switch (IdNotificacionMotivo)
                        {
                          
                            case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.Observado:
                                Mailer.MailMessages.SendMail_ObservacionSolicitud_v2(solicitud.id_solicitud, fechaNotificacion);
                                break;
                            case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.Rechazado:
                                Mailer.MailMessages.SendMail_RechazoSolicitud_v2(solicitud.id_solicitud, fechaNotificacion);
                                break;
                            case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.Aprobado:
                                Mailer.MailMessages.SendMail_AprobadoSolicitud_v2(solicitud.id_solicitud, fechaNotificacion);
                                break;
                             case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.AvisoDeCaducidad:
                                emailsNotificados = Mailer.MailMessages.SendMail_Caducidad_v2(solicitud.id_solicitud, fechaNotificacion);
                               break;
                            case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.BajaDeSolicitud:
                                if (solicitud.id_solicitud > ErrorConstants.EsSolicitud)
                                    Mailer.MailMessages.SendMail_BajaSolicitud_v2(solicitud.id_solicitud, fechaNotificacion);
                                else
                                    Mailer.MailMessages.SendMail_BajaSolicitud_Transf_v2(solicitud.id_solicitud, fechaNotificacion);
                                break;
                            case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.LevantamientoDeRechazo:
                                Mailer.MailMessages.SendMail_LevantamientoRechazo(solicitud.id_solicitud, fechaNotificacion);
                                break;
                            case (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.Otras:
                                Mailer.MailMessages.SendMail_Otros(IdNotificacionMotivo,id_solicitud, fechaNotificacion, asunto, mensaje);
                                break;
                            default:
                                Mailer.MailMessages.SendMail_Generic(solicitud.id_solicitud, IdNotificacionMotivo, fechaNotificacion);
                                break;
                        }

                        //errorMessage = "La solicitud fue enviada con éxito a los siguientes emails: " + String.Join(", ", emailsNotificados);
                        errorMessage = "La solicitud fue enviada con éxito " ;
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
       
        public static List<SSIT_Solicitudes_Notificaciones_motivos> TraerNotificaciones_motivos(out string errorMessage)
        {
            List<SSIT_Solicitudes_Notificaciones_motivos> Notificaciones_motivosList = new List<SSIT_Solicitudes_Notificaciones_motivos>();
            errorMessage = string.Empty;

            IniciarEntity();

            try
            {
                using (DGHP_Entities db = new DGHP_Entities())
                {
                    Notificaciones_motivosList = (from x in db.SSIT_Solicitudes_Notificaciones_motivos
                                                  select x).ToList();

                    if (Notificaciones_motivosList.Count == 0)
                    {
                        errorMessage = "Error al cargar la lista de Motivos de Notificaciones";
                    }    
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                throw new Exception(errorMessage);
            }
            return Notificaciones_motivosList;
        }

        public static List<Calles> TraerCalles(out string errorMessage)
        {
            List<Calles> nombreCalles = new List<Calles>();
            errorMessage = string.Empty;

            IniciarEntity();

            try
            {
                using (DGHP_Entities db = new DGHP_Entities())
                {
                    nombreCalles = (from c in db.Calles
                                    select c).ToList();

                    if (nombreCalles.Count == 0)
                    {
                        errorMessage = "Error al cargar la lista de Calles";
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                throw new Exception(errorMessage);
            }
            return nombreCalles;
        }

    }
}