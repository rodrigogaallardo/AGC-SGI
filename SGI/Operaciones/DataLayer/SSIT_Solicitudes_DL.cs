using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.xml.simpleparser.handler;
using SGI.BusinessLogicLayer.Enums;
using SGI.DataLayer.Models;
using SGI.Model;

namespace SGI.DataLayer
{
    public class SSIT_Solicitudes_DL : DataLayerBase
    {
        public SSIT_Solicitudes_Model Get(int id_solicitud)
        {
            IniciarEntity();
            var solicitud = (from st in db.SSIT_Solicitudes
                             where st.id_solicitud == id_solicitud
                             select new SSIT_Solicitudes_Model
                             {
                                 id_solicitud = id_solicitud,
                                 id_estado = st.id_estado
                             }).Union(from tr in db.Transf_Solicitudes
                                      where tr.id_solicitud == id_solicitud
                                      select new SSIT_Solicitudes_Model
                                      {
                                          id_solicitud = id_solicitud,
                                          id_estado = tr.id_estado
                                      }).FirstOrDefault();
            FinalizarEntity();
            return solicitud;
        }


        public List<SSIT_Solicitudes_Model> GetNotificacionesSolicitudCaduca(int id_solicitud)
        {
            var solicitudesCaducasNotificadas = (from st in db.SSIT_Solicitudes
                                                 join ted in db.TipoEstadoSolicitud on st.id_estado equals ted.Id
                                                 join sn in db.SSIT_Solicitudes_Notificaciones on st.id_solicitud equals sn.id_solicitud
                                                 from sn2 in db.SSIT_Solicitudes_Notificaciones
                                                 join snm in db.SSIT_Solicitudes_Notificaciones_motivos on sn2.Id_NotificacionMotivo equals snm.IdNotificacionMotivo
                                                 where (snm.IdNotificacionMotivo == (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.AvisoDeCaducidad)
                                                 && (st.id_solicitud == id_solicitud)
                                                 select new SSIT_Solicitudes_Model
                                                 {
                                                     id_solicitud = id_solicitud,
                                                     id_estado = st.id_estado
                                                 }).Union(from tra in db.Transf_Solicitudes
                                                          join ted in db.TipoEstadoSolicitud on tra.id_estado equals ted.Id
                                                          join tn in db.Transf_Solicitudes_Notificaciones on tra.id_solicitud equals tn.id_solicitud
                                                          from tn2 in db.Transf_Solicitudes_Notificaciones
                                                          join tnm in db.SSIT_Solicitudes_Notificaciones_motivos on tn2.Id_NotificacionMotivo equals tnm.IdNotificacionMotivo
                                                          where (tnm.IdNotificacionMotivo == (int)SSIT_Solicitudes_Notificaciones_motivos_Enum.AvisoDeCaducidad)
                                                          && (tra.id_solicitud == id_solicitud)
                                                          select new SSIT_Solicitudes_Model
                                                          {
                                                              id_solicitud = id_solicitud,
                                                              id_estado = tra.id_estado
                                                          }).ToList<SSIT_Solicitudes_Model>();

            return solicitudesCaducasNotificadas;

        }
    }
}