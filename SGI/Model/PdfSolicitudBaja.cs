using SGI.App_Data;
using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;

namespace SGI.Model
{
    public class PdfSolicitudBaja
    {
        public static dsBajaSolicitud GenerarDataSet(int id_solicitud, string motivo, string observaciones)
        {
            DGHP_Entities db = new DGHP_Entities();
            dsBajaSolicitud ds = new dsBajaSolicitud();
            DataRow row;

            SSIT_Solicitudes sol=db.SSIT_Solicitudes.FirstOrDefault(x=>x.id_solicitud==id_solicitud);
            var encomienda = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo)
                    .OrderByDescending(x => x.id_encomienda).FirstOrDefault();
            int id_encomienda = encomienda.id_encomienda;


            DataTable dtDisposicion = ds.Tables["SolicitudBaja"];
            row = dtDisposicion.NewRow();

            row["id_solicitud"] = id_solicitud;
            row["FechaBaja"] = DateTime.Now;
            row["Motivo"] = motivo;
            row["Observaciones"] = observaciones;
            row["Ubicacion"] = "";
            row["NroExpediente"] = sol.NroExpedienteSade;
            row["Superficie"] = encomienda.Encomienda_DatosLocal.First().superficie_cubierta_dl + encomienda.Encomienda_DatosLocal.First().superficie_descubierta_dl;
            dtDisposicion.Rows.Add(row);

            var query_ubi =
                (
                    from solubic in db.SSIT_Solicitudes_Ubicaciones
                    join mat in db.Ubicaciones on solubic.id_ubicacion equals mat.id_ubicacion
                    join zon1 in db.Zonas_Planeamiento on solubic.id_zonaplaneamiento equals zon1.id_zonaplaneamiento
                    where solubic.id_solicitud == id_solicitud
                    orderby solubic.id_solicitudubicacion
                    select new
                    {
                        solubic.id_solicitudubicacion,
                        solubic.id_solicitud,
                        solubic.id_ubicacion,
                        mat.Seccion,
                        mat.Manzana,
                        mat.Parcela,
                        NroPartidaMatriz = mat.NroPartidaMatriz,
                        solubic.local_subtipoubicacion,
                        ZonaParcela = zon1.CodZonaPla,
                        DeptoLocal = solubic.deptoLocal_ubicacion
                    }
                ).ToList();

            DataTable dtUbicaciones = ds.Tables["Ubicaciones"];

            foreach (var item in query_ubi)
            {
                row = dtUbicaciones.NewRow();

                string sql = "select dbo.SSIT_Solicitud_DireccionesPartidasPlancheta(" + item.id_solicitud + "," + item.id_ubicacion + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                row["id_solicitudubicacion"] = item.id_solicitudubicacion;
                row["id_solicitud"] = id_solicitud;
                row["id_ubicacion"] = item.id_ubicacion;
                if(item.Seccion != null)
                    row["Seccion"] = item.Seccion;
                else
                    row["Seccion"] = DBNull.Value;
                row["Manzana"] = item.Manzana;
                row["Parcela"] = item.Parcela;
                if (item.NroPartidaMatriz != null)
                    row["NroPartidaMatriz"] = item.NroPartidaMatriz;
                else
                    row["NroPartidaMatriz"] = DBNull.Value;
                row["local_subtipoubicacion"] = item.local_subtipoubicacion;
                row["ZonaParcela"] = item.ZonaParcela;
                row["Direcciones"] = direccion;
                row["DeptoLocal"] = item.DeptoLocal;
                dtUbicaciones.Rows.Add(row);
            }



            var query_ph =
                (
                    from solubic in db.SSIT_Solicitudes_Ubicaciones
                    join solphor in db.SSIT_Solicitudes_Ubicaciones_PropiedadHorizontal on solubic.id_solicitudubicacion equals solphor.id_solicitudubicacion
                    join phor in db.Ubicaciones_PropiedadHorizontal on solphor.id_propiedadhorizontal equals phor.id_propiedadhorizontal
                    where solubic.id_solicitud == id_solicitud
                    orderby solubic.id_solicitudubicacion
                    select new
                    {
                        solubic.id_solicitudubicacion,
                        solubic.id_ubicacion,
                        NroPartidaHorizontal = phor.NroPartidaHorizontal,
                        phor.Piso,
                        phor.Depto
                    }
                ).ToList();

            DataTable dtPH = ds.Tables["PropiedadHorizontal"];


            foreach (var item in query_ph)
            {
                row = dtPH.NewRow();

                row["id_solicitudubicacion"] = item.id_solicitudubicacion;
                row["id_ubicacion"] = item.id_ubicacion;
                
                if (item.NroPartidaHorizontal.HasValue)
                    row["NroPartidaHorizontal"] = item.NroPartidaHorizontal;
                else
                    row["NroPartidaHorizontal"] = DBNull.Value;
                
                row["Piso"] = item.Piso;
                row["Depto"] = item.Depto;

                dtPH.Rows.Add(row);
            }


            // Puertas

            var query_puerta =
                (
                    from solubic in db.SSIT_Solicitudes_Ubicaciones
                    join solpuer in db.SSIT_Solicitudes_Ubicaciones_Puertas on solubic.id_solicitudubicacion equals solpuer.id_solicitudubicacion
                    where solubic.id_solicitud == id_solicitud
                    orderby solubic.id_solicitudubicacion
                    select new
                    {
                        solubic.id_solicitudubicacion,
                        solpuer.id_solicitudpuerta,
                        solubic.id_ubicacion,
                        solpuer.nombre_calle,
                        solpuer.NroPuerta
                    }
                ).ToList();

            DataTable dtPuerta = ds.Tables["Puertas"];

            foreach (var item in query_puerta)
            {
                row = dtPuerta.NewRow();
                row["id_solicitudubicacion"] = item.id_solicitudubicacion;
                row["id_ubicacion"] = item.id_ubicacion;
                row["Calle"] = item.nombre_calle;
                row["NroPuerta"] = item.NroPuerta;
                dtPuerta.Rows.Add(row);
            }

            // Titulares
            var query_Titulares1 =
                (
                    from pj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas
                    join tsoc in db.TipoSociedad on pj.Id_TipoSociedad equals tsoc.Id
                    join tipoiibb in db.TiposDeIngresosBrutos on pj.id_tipoiibb equals tipoiibb.id_tipoiibb
                    where pj.id_solicitud == id_solicitud
                    select new
                    {
                        id_persona = pj.id_personajuridica,
                        pj.id_solicitud,
                        TipoPersona = "PJ",
                        RazonSocial = pj.Razon_Social,
                        TipoSociedad = tsoc.Descripcion
                    }
                ).ToList();


            DataTable dtTitulares = ds.Tables["Titulares"];


            foreach (var item in query_Titulares1)
            {
                row = dtTitulares.NewRow();

                row["id_persona"] = item.id_persona;
                row["id_solicitud"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["TipoSociedad"] = item.TipoSociedad;
                row["RazonSocial"] = item.RazonSocial.ToUpper();
                row["Apellido"] = "";
                row["Nombres"] = "";
                dtTitulares.Rows.Add(row);
            }

            var query_Titulares3 =
                (
                    from pf in db.SSIT_Solicitudes_Titulares_PersonasFisicas
                    join tipoiibb in db.TiposDeIngresosBrutos on pf.id_tipoiibb equals tipoiibb.id_tipoiibb
                    where pf.id_solicitud == id_solicitud
                    select new
                    {
                        id_persona = pf.id_personafisica,
                        pf.id_solicitud,
                        TipoPersona = "PF",
                        Apellido = pf.Apellido,
                        Nombres = pf.Nombres,
                    }
                ).ToList();

            foreach (var item in query_Titulares3)
            {
                row = dtTitulares.NewRow();
                row["id_persona"] = item.id_persona;
                row["id_solicitud"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["TipoSociedad"] = "";
                row["RazonSocial"] = "";
                row["Apellido"] = item.Apellido;
                row["Nombres"] = item.Nombres;
                dtTitulares.Rows.Add(row);
            }

            // Rubros
            var query_Rubros =
                (
                    from enc in db.Encomienda
                    join rub in db.Encomienda_Rubros on enc.id_encomienda equals rub.id_encomienda
                    join tact in db.TipoActividad on rub.id_tipoactividad equals tact.Id
                    join docreq in db.Tipo_Documentacion_Req on rub.id_tipodocreq equals docreq.Id
                    where enc.id_encomienda == id_encomienda
                    select new
                    {
                        rub.id_encomiendarubro,
                        enc.id_encomienda,
                        rub.cod_rubro,
                        desc_rubro = rub.desc_rubro,
                        rub.EsAnterior,
                        TipoActividad = tact.Nombre,
                        DocRequerida = docreq.Nomenclatura,
                        rub.SuperficieHabilitar
                    }
                ).ToList();

            DataTable dtRubros = ds.Tables["Rubros"];

            foreach (var item in query_Rubros)
            {
                row = dtRubros.NewRow();

                row["id_solicitudrubro"] = item.id_encomiendarubro;
                row["id_solicitud"] = id_solicitud;
                row["cod_rubro"] = item.cod_rubro;
                row["desc_rubro"] = item.desc_rubro;
                row["EsAnterior"] = item.EsAnterior;
                row["TipoActividad"] = item.TipoActividad;
                row["DocRequerida"] = item.DocRequerida;
                row["SuperficieHabilitar"] = item.SuperficieHabilitar;

                dtRubros.Rows.Add(row);
            }

            var query_RubrosCN =
                (
                    from enc in db.Encomienda
                    join rub in db.Encomienda_RubrosCN on enc.id_encomienda equals rub.id_encomienda
                    join r in db.RubrosCN on rub.CodigoRubro equals r.Codigo
                    join tact in db.TipoActividad on rub.IdTipoActividad equals tact.Id
                    join gc in db.ENG_Grupos_Circuitos on r.IdGrupoCircuito equals gc.id_grupo_circuito
                    where enc.id_encomienda == id_encomienda
                    select new
                    {
                        rub.id_encomiendarubro,
                        enc.id_encomienda,
                        cod_rubro = rub.CodigoRubro,
                        desc_rubro = rub.NombreRubro,
                        EsAnterior = false,
                        TipoActividad = tact.Nombre,
                        DocRequerida = gc.cod_grupo_circuito,
                        rub.SuperficieHabilitar
                    }
                 ).ToList();

            foreach (var item in query_RubrosCN)
            {
                row = dtRubros.NewRow();

                row["id_solicitudrubro"] = item.id_encomiendarubro;
                row["id_solicitud"] = id_solicitud;
                row["cod_rubro"] = item.cod_rubro;
                row["desc_rubro"] = item.desc_rubro;
                row["EsAnterior"] = item.EsAnterior;
                row["TipoActividad"] = item.TipoActividad;
                row["DocRequerida"] = item.DocRequerida;
                row["SuperficieHabilitar"] = item.SuperficieHabilitar;

                dtRubros.Rows.Add(row);
            }
            db.Dispose();
            return ds;
        }

        public static byte[] GenerarPDF(int id_solicitud, string motivo, string observaciones)
        {

            Stream msPdfDisposicion = null;
            byte[] documento = null;

            App_Data.dsBajaSolicitud dsDispo = GenerarDataSet(id_solicitud, motivo, observaciones);

            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
            try
            {
                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/BajaSolicitud.rpt";
                CrystalReportSource1.ReportDocument.SetDataSource(dsDispo);
                msPdfDisposicion = CrystalReportSource1.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                //se liberan recursos porque el crystal esta configurado para 65 instancias en registry
                CrystalReportSource1.ReportDocument.Close();

                if (CrystalReportSource1 != null)
                {
                    CrystalReportSource1.ReportDocument.Close();
                    CrystalReportSource1.ReportDocument.Dispose();
                    CrystalReportSource1.Dispose();
                }

                documento = Functions.StreamToArray(msPdfDisposicion);
            }
            catch (Exception ex)
            {
                CrystalReportSource1.ReportDocument.Close();
                CrystalReportSource1.ReportDocument.Dispose();
                CrystalReportSource1.Dispose();
                msPdfDisposicion = null;
                documento = null;
                throw ex;
            }

            return documento;
        }

        public static dsBajaSolicitud Transf_GenerarDataSet(int id_solicitud, string motivo, string observaciones)
        {
            DGHP_Entities db = new DGHP_Entities();
            dsBajaSolicitud ds = new dsBajaSolicitud();
            DataRow row;

            Transf_Solicitudes tranf = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);


            DataTable dtDisposicion = ds.Tables["SolicitudBaja"];
            row = dtDisposicion.NewRow();
            
            row["id_solicitud"] = id_solicitud;
            row["FechaBaja"] = DateTime.Now;
            row["Motivo"] = motivo;
            row["Observaciones"] = observaciones;
            row["Ubicacion"] = "";
            row["NroExpediente"] = tranf.NroExpedienteSade;
            row["Superficie"] = 0;

            if (tranf.CPadron_Solicitudes.CPadron_DatosLocal.Count() >0)
            {
                row["Superficie"] = tranf.CPadron_Solicitudes.CPadron_DatosLocal.FirstOrDefault().superficie_cubierta_dl
                + tranf.CPadron_Solicitudes.CPadron_DatosLocal.FirstOrDefault().superficie_descubierta_dl;
            }

            dtDisposicion.Rows.Add(row);

            var query_ubi =
                 (
                     from sol in db.Transf_Solicitudes
                     join cpubic in db.CPadron_Ubicaciones on sol.id_cpadron equals cpubic.id_cpadron
                     join mat in db.Ubicaciones on cpubic.id_ubicacion equals mat.id_ubicacion
                     join zon1 in db.Zonas_Planeamiento on cpubic.id_zonaplaneamiento equals zon1.id_zonaplaneamiento
                     where sol.id_solicitud == id_solicitud
                     orderby cpubic.id_cpadronubicacion
                     select new
                     {
                         cpubic.id_cpadronubicacion,
                         sol.id_solicitud,
                         sol.id_cpadron,
                         cpubic.id_ubicacion,
                         mat.Seccion,
                         mat.Manzana,
                         mat.Parcela,
                         NroPartidaMatriz = mat.NroPartidaMatriz,
                         cpubic.local_subtipoubicacion,
                         ZonaParcela = zon1.CodZonaPla,
                         DeptoLocal = cpubic.deptoLocal_cpadronubicacion
                     }
                 ).ToList();

            DataTable dtUbicaciones = ds.Tables["Ubicaciones"];

            foreach (var item in query_ubi)
            {
                row = dtUbicaciones.NewRow();

                string sql = "select dbo.CPadron_Solicitud_DireccionesPartidas(" + item.id_cpadron + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                row["id_solicitudubicacion"] = item.id_cpadronubicacion;
                row["id_solicitud"] = id_solicitud;
                row["id_ubicacion"] = item.id_ubicacion;
                if (item.Seccion != null)
                    row["Seccion"] = item.Seccion;
                else
                    row["Seccion"] = DBNull.Value;
                row["Manzana"] = item.Manzana;
                row["Parcela"] = item.Parcela;
                if (item.NroPartidaMatriz != null)
                    row["NroPartidaMatriz"] = item.NroPartidaMatriz;
                else
                    row["NroPartidaMatriz"] = DBNull.Value;
                row["local_subtipoubicacion"] = item.local_subtipoubicacion;
                row["ZonaParcela"] = item.ZonaParcela;
                row["Direcciones"] = direccion;
                row["DeptoLocal"] = item.DeptoLocal;
                dtUbicaciones.Rows.Add(row);
            }

            var query_ph =
                (
                    from sol in db.Transf_Solicitudes
                    join cpubic in db.CPadron_Ubicaciones on sol.id_cpadron equals cpubic.id_cpadron
                    join cpphor in db.CPadron_Ubicaciones_PropiedadHorizontal on cpubic.id_cpadronubicacion equals cpphor.id_cpadronubicacion
                    join phor in db.Ubicaciones_PropiedadHorizontal on cpphor.id_propiedadhorizontal equals phor.id_propiedadhorizontal
                    where sol.id_solicitud == id_solicitud
                    orderby cpubic.id_cpadronubicacion
                    select new
                    {
                        cpubic.id_cpadronubicacion,
                        cpubic.id_ubicacion,
                        NroPartidaHorizontal = phor.NroPartidaHorizontal,
                        phor.Piso,
                        phor.Depto
                    }
                ).ToList();

            DataTable dtPH = ds.Tables["PropiedadHorizontal"];

            foreach (var item in query_ph)
            {
                row = dtPH.NewRow();

                row["id_solicitudubicacion"] = item.id_cpadronubicacion;
                row["id_ubicacion"] = item.id_ubicacion;

                if (item.NroPartidaHorizontal.HasValue)
                    row["NroPartidaHorizontal"] = item.NroPartidaHorizontal;
                else
                    row["NroPartidaHorizontal"] = DBNull.Value;

                row["Piso"] = item.Piso;
                row["Depto"] = item.Depto;

                dtPH.Rows.Add(row);
            }

            // Puertas
            var query_puerta =
                (
                    from sol in db.Transf_Solicitudes
                    join cpubic in db.CPadron_Ubicaciones on sol.id_cpadron equals cpubic.id_cpadron
                    join cppuer in db.CPadron_Ubicaciones_Puertas on cpubic.id_cpadronubicacion equals cppuer.id_cpadronubicacion
                    where sol.id_solicitud == id_solicitud
                    orderby cpubic.id_cpadronubicacion
                    select new
                    {
                        cpubic.id_cpadronubicacion,
                        cpubic.id_cpadron,
                        cpubic.id_ubicacion,
                        cppuer.nombre_calle,
                        cppuer.NroPuerta
                    }
                ).ToList();
            DataTable dtPuerta = ds.Tables["Puertas"];

            foreach (var item in query_puerta)
            {
                row = dtPuerta.NewRow();
                row["id_solicitudubicacion"] = item.id_cpadronubicacion;
                row["id_ubicacion"] = item.id_ubicacion;
                row["Calle"] = item.nombre_calle;
                row["NroPuerta"] = item.NroPuerta;
                dtPuerta.Rows.Add(row);
            }

            // Titulares
            var query_Titulares1 =
                (
                    from pj in db.Transf_Titulares_PersonasJuridicas
                    join tsoc in db.TipoSociedad on pj.Id_TipoSociedad equals tsoc.Id
                    join tipoiibb in db.TiposDeIngresosBrutos on pj.id_tipoiibb equals tipoiibb.id_tipoiibb
                    where pj.id_solicitud == id_solicitud
                    select new
                    {
                        id_persona = pj.id_personajuridica,
                        pj.id_solicitud,
                        TipoPersona = "PJ",
                        RazonSocial = pj.Razon_Social,
                        TipoSociedad = tsoc.Descripcion
                    }
                ).ToList();

            DataTable dtTitulares = ds.Tables["Titulares"];

            foreach (var item in query_Titulares1)
            {
                row = dtTitulares.NewRow();

                row["id_persona"] = item.id_persona;
                row["id_solicitud"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["TipoSociedad"] = item.TipoSociedad;
                row["RazonSocial"] = item.RazonSocial.ToUpper();
                row["Apellido"] = "";
                row["Nombres"] = "";
                dtTitulares.Rows.Add(row);
            }

            var query_Titulares2 =
                (
                    from pj in db.Transf_Titulares_PersonasJuridicas_PersonasFisicas
                    join titpj in db.Transf_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                    where titpj.id_solicitud == id_solicitud && (titpj.Id_TipoSociedad == 2 || titpj.Id_TipoSociedad == 32)
                    select new
                    {
                        id_persona = pj.id_firmante_pj,
                        titpj.id_solicitud,
                        TipoPersona = "PF",
                        Apellido = pj.Apellido,
                        Nombres = pj.Nombres,
                    }
                ).Distinct().ToList();
            foreach (var item in query_Titulares2)
            {
                row = dtTitulares.NewRow();
                row["id_persona"] = item.id_persona;
                row["id_solicitud"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["TipoSociedad"] = "";
                row["RazonSocial"] = "";
                row["Apellido"] = item.Apellido;
                row["Nombres"] = item.Nombres;
                dtTitulares.Rows.Add(row);
            }
            // Rubros
            var query_Rubros =
                (
                    from sol in db.Transf_Solicitudes
                    join rub in db.CPadron_Rubros on sol.id_cpadron equals rub.id_cpadron
                    join tact in db.TipoActividad on rub.id_tipoactividad equals tact.Id
                    join docreq in db.Tipo_Documentacion_Req on rub.id_tipodocreq equals docreq.Id
                    where sol.id_solicitud == id_solicitud
                    select new
                    {
                        rub.id_cpadronrubro,
                        sol.id_solicitud,
                        rub.cod_rubro,
                        desc_rubro = rub.desc_rubro,
                        rub.EsAnterior,
                        TipoActividad = tact.Nombre,
                        DocRequerida = docreq.Nomenclatura,
                        rub.SuperficieHabilitar
                    }
                ).ToList();


            DataTable dtRubros = ds.Tables["Rubros"];

            foreach (var item in query_Rubros)
            {
                row = dtRubros.NewRow();

                row["id_solicitudrubro"] = item.id_cpadronrubro;
                row["id_solicitud"] = id_solicitud;
                row["cod_rubro"] = item.cod_rubro;
                row["desc_rubro"] = item.desc_rubro;
                row["EsAnterior"] = item.EsAnterior;
                row["TipoActividad"] = item.TipoActividad;
                row["DocRequerida"] = item.DocRequerida;
                row["SuperficieHabilitar"] = item.SuperficieHabilitar;

                dtRubros.Rows.Add(row);
            }
            db.Dispose();
            return ds;
        }

        public static byte[] Transf_GenerarPDF(int id_solicitud, string motivo, string observaciones)
        {
            Stream msPdfDisposicion = null;
            byte[] documento = null;

            dsBajaSolicitud dsDispo = Transf_GenerarDataSet(id_solicitud, motivo, observaciones);

            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
            try
            {
                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/BajaSolicitud.rpt";
                CrystalReportSource1.ReportDocument.SetDataSource(dsDispo);
                msPdfDisposicion = CrystalReportSource1.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                //se liberan recursos porque el crystal esta configurado para 65 instancias en registry
                CrystalReportSource1.ReportDocument.Close();

                if (CrystalReportSource1 != null)
                {
                    CrystalReportSource1.ReportDocument.Close();
                    CrystalReportSource1.ReportDocument.Dispose();
                    CrystalReportSource1.Dispose();
                }

                documento = Functions.StreamToArray(msPdfDisposicion);
            }
            catch (Exception ex)
            {
                CrystalReportSource1.ReportDocument.Close();
                CrystalReportSource1.ReportDocument.Dispose();
                CrystalReportSource1.Dispose();
                msPdfDisposicion = null;
                documento = null;
                throw ex;
            }

            return documento;
        }

        internal static byte[] CPadron_GenerarPDF(int id_cpadron, string motivo, string observaciones)
        {
            Stream msPdfDisposicion = null;
            byte[] documento = null;

            dsBajaSolicitud dsDispo = CPadron_GenerarDataSet(id_cpadron, motivo, observaciones);

            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
            try
            {
                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/BajaSolicitud.rpt";
                CrystalReportSource1.ReportDocument.SetDataSource(dsDispo);
                msPdfDisposicion = CrystalReportSource1.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                //se liberan recursos porque el crystal esta configurado para 65 instancias en registry
                CrystalReportSource1.ReportDocument.Close();

                if (CrystalReportSource1 != null)
                {
                    CrystalReportSource1.ReportDocument.Close();
                    CrystalReportSource1.ReportDocument.Dispose();
                    CrystalReportSource1.Dispose();
                }

                documento = Functions.StreamToArray(msPdfDisposicion);
            }
            catch (Exception ex)
            {
                CrystalReportSource1.ReportDocument.Close();
                CrystalReportSource1.ReportDocument.Dispose();
                CrystalReportSource1.Dispose();
                msPdfDisposicion = null;
                documento = null;
                throw ex;
            }

            return documento;
        }

        private static dsBajaSolicitud CPadron_GenerarDataSet(int id_cpadron, string motivo, string observaciones)
        {
            DGHP_Entities db = new DGHP_Entities();
            dsBajaSolicitud ds = new dsBajaSolicitud();
            DataRow row;

            CPadron_Solicitudes cpSol = db.CPadron_Solicitudes.FirstOrDefault(x => x.id_cpadron == id_cpadron);


            DataTable dtDisposicion = ds.Tables["SolicitudBaja"];
            row = dtDisposicion.NewRow();

            row["id_solicitud"] = id_cpadron;
            row["FechaBaja"] = DateTime.Now;
            row["Motivo"] = motivo;
            row["Observaciones"] = observaciones;
            row["Ubicacion"] = "";
            row["NroExpediente"] = cpSol.NroExpedienteSade;
            row["Superficie"] = cpSol.CPadron_DatosLocal.First().superficie_cubierta_dl
                + cpSol.CPadron_DatosLocal.First().superficie_descubierta_dl;
            dtDisposicion.Rows.Add(row);

            var query_ubi =
                 (
                     from sol in db.CPadron_Solicitudes
                     join cpubic in db.CPadron_Ubicaciones on sol.id_cpadron equals cpubic.id_cpadron
                     join mat in db.Ubicaciones on cpubic.id_ubicacion equals mat.id_ubicacion
                     join zon1 in db.Zonas_Planeamiento on cpubic.id_zonaplaneamiento equals zon1.id_zonaplaneamiento
                     where sol.id_cpadron == id_cpadron
                     orderby cpubic.id_cpadronubicacion
                     select new
                     {
                         cpubic.id_cpadronubicacion,
                         sol.id_cpadron,
                         cpubic.id_ubicacion,
                         mat.Seccion,
                         mat.Manzana,
                         mat.Parcela,
                         NroPartidaMatriz = mat.NroPartidaMatriz,
                         cpubic.local_subtipoubicacion,
                         ZonaParcela = zon1.CodZonaPla,
                         DeptoLocal = cpubic.deptoLocal_cpadronubicacion
                     }
                 ).ToList();

            DataTable dtUbicaciones = ds.Tables["Ubicaciones"];

            foreach (var item in query_ubi)
            {
                row = dtUbicaciones.NewRow();

                string sql = "select dbo.CPadron_Solicitud_DireccionesPartidas(" + item.id_cpadron + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                row["id_solicitudubicacion"] = item.id_cpadronubicacion;
                row["id_solicitud"] = id_cpadron;
                row["id_ubicacion"] = item.id_ubicacion;
                if (item.Seccion != null)
                    row["Seccion"] = item.Seccion;
                else
                    row["Seccion"] = DBNull.Value;
                row["Manzana"] = item.Manzana;
                row["Parcela"] = item.Parcela;
                if (item.NroPartidaMatriz != null)
                    row["NroPartidaMatriz"] = item.NroPartidaMatriz;
                else
                    row["NroPartidaMatriz"] = DBNull.Value;
                row["local_subtipoubicacion"] = item.local_subtipoubicacion;
                row["ZonaParcela"] = item.ZonaParcela;
                row["Direcciones"] = direccion;
                row["DeptoLocal"] = item.DeptoLocal;
                dtUbicaciones.Rows.Add(row);
            }

            var query_ph =
                (
                    from sol in db.CPadron_Solicitudes
                    join cpubic in db.CPadron_Ubicaciones on sol.id_cpadron equals cpubic.id_cpadron
                    join cpphor in db.CPadron_Ubicaciones_PropiedadHorizontal on cpubic.id_cpadronubicacion equals cpphor.id_cpadronubicacion
                    join phor in db.Ubicaciones_PropiedadHorizontal on cpphor.id_propiedadhorizontal equals phor.id_propiedadhorizontal
                    where sol.id_cpadron == id_cpadron
                    orderby cpubic.id_cpadronubicacion
                    select new
                    {
                        cpubic.id_cpadronubicacion,
                        cpubic.id_ubicacion,
                        NroPartidaHorizontal = phor.NroPartidaHorizontal,
                        phor.Piso,
                        phor.Depto
                    }
                ).ToList();

            DataTable dtPH = ds.Tables["PropiedadHorizontal"];

            foreach (var item in query_ph)
            {
                row = dtPH.NewRow();

                row["id_solicitudubicacion"] = item.id_cpadronubicacion;
                row["id_ubicacion"] = item.id_ubicacion;

                if (item.NroPartidaHorizontal.HasValue)
                    row["NroPartidaHorizontal"] = item.NroPartidaHorizontal;
                else
                    row["NroPartidaHorizontal"] = DBNull.Value;

                row["Piso"] = item.Piso;
                row["Depto"] = item.Depto;

                dtPH.Rows.Add(row);
            }

            // Puertas
            var query_puerta =
                (
                    from sol in db.CPadron_Solicitudes
                    join cpubic in db.CPadron_Ubicaciones on sol.id_cpadron equals cpubic.id_cpadron
                    join cppuer in db.CPadron_Ubicaciones_Puertas on cpubic.id_cpadronubicacion equals cppuer.id_cpadronubicacion
                    where sol.id_cpadron == id_cpadron
                    orderby cpubic.id_cpadronubicacion
                    select new
                    {
                        cpubic.id_cpadronubicacion,
                        cpubic.id_cpadron,
                        cpubic.id_ubicacion,
                        cppuer.nombre_calle,
                        cppuer.NroPuerta
                    }
                ).ToList();
            DataTable dtPuerta = ds.Tables["Puertas"];

            foreach (var item in query_puerta)
            {
                row = dtPuerta.NewRow();
                row["id_solicitudubicacion"] = item.id_cpadronubicacion;
                row["id_ubicacion"] = item.id_ubicacion;
                row["Calle"] = item.nombre_calle;
                row["NroPuerta"] = item.NroPuerta;
                dtPuerta.Rows.Add(row);
            }

            // Titulares
            var query_Titulares1 =
                (
                    from pj in db.CPadron_Titulares_PersonasJuridicas
                    join tsoc in db.TipoSociedad on pj.Id_TipoSociedad equals tsoc.Id
                    join tipoiibb in db.TiposDeIngresosBrutos on pj.id_tipoiibb equals tipoiibb.id_tipoiibb
                    where pj.id_cpadron == id_cpadron
                    select new
                    {
                        id_persona = pj.id_personajuridica,
                        pj.id_cpadron,
                        TipoPersona = "PJ",
                        RazonSocial = pj.Razon_Social,
                        TipoSociedad = tsoc.Descripcion
                    }
                ).ToList();

            DataTable dtTitulares = ds.Tables["Titulares"];

            foreach (var item in query_Titulares1)
            {
                row = dtTitulares.NewRow();

                row["id_persona"] = item.id_persona;
                row["id_solicitud"] = item.id_cpadron;
                row["TipoPersona"] = item.TipoPersona;
                row["TipoSociedad"] = item.TipoSociedad;
                row["RazonSocial"] = item.RazonSocial.ToUpper();
                row["Apellido"] = "";
                row["Nombres"] = "";
                dtTitulares.Rows.Add(row);
            }

            var query_Titulares2 =
                (
                    from pj in db.Transf_Titulares_PersonasJuridicas_PersonasFisicas
                    join titpj in db.Transf_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                    where titpj.id_solicitud == id_cpadron && (titpj.Id_TipoSociedad == 2 || titpj.Id_TipoSociedad == 32)
                    select new
                    {
                        id_persona = pj.id_firmante_pj,
                        titpj.id_solicitud,
                        TipoPersona = "PF",
                        Apellido = pj.Apellido,
                        Nombres = pj.Nombres,
                    }
                ).Distinct().ToList();
            foreach (var item in query_Titulares2)
            {
                row = dtTitulares.NewRow();
                row["id_persona"] = item.id_persona;
                row["id_solicitud"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["TipoSociedad"] = "";
                row["RazonSocial"] = "";
                row["Apellido"] = item.Apellido;
                row["Nombres"] = item.Nombres;
                dtTitulares.Rows.Add(row);
            }
            // Rubros
            var query_Rubros =
                (
                    from sol in db.CPadron_Solicitudes
                    join rub in db.CPadron_Rubros on sol.id_cpadron equals rub.id_cpadron
                    join tact in db.TipoActividad on rub.id_tipoactividad equals tact.Id
                    join docreq in db.Tipo_Documentacion_Req on rub.id_tipodocreq equals docreq.Id
                    where sol.id_cpadron == id_cpadron
                    select new
                    {
                        rub.id_cpadronrubro,
                        sol.id_cpadron,
                        rub.cod_rubro,
                        desc_rubro = rub.desc_rubro,
                        rub.EsAnterior,
                        TipoActividad = tact.Nombre,
                        DocRequerida = docreq.Nomenclatura,
                        rub.SuperficieHabilitar
                    }
                ).ToList();


            DataTable dtRubros = ds.Tables["Rubros"];

            foreach (var item in query_Rubros)
            {
                row = dtRubros.NewRow();

                row["id_solicitudrubro"] = item.id_cpadronrubro;
                row["id_solicitud"] = id_cpadron;
                row["cod_rubro"] = item.cod_rubro;
                row["desc_rubro"] = item.desc_rubro;
                row["EsAnterior"] = item.EsAnterior;
                row["TipoActividad"] = item.TipoActividad;
                row["DocRequerida"] = item.DocRequerida;
                row["SuperficieHabilitar"] = item.SuperficieHabilitar;

                dtRubros.Rows.Add(row);
            }
            db.Dispose();
            return ds;
        }
    }

}