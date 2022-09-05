using SGI.App_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using ThoughtWorks.QRCode.Codec;
using SGI.WebServices;

namespace SGI.Model
{
    public class Certificado
    {
        #region habilitaciones
        public static byte[] GenerarPdf(int id_solicitud, string nro_expediente, bool impresionDePrueba)
        {
            byte[] documento = null;

            try
            {
                ws_Reporting reportingService = new ws_Reporting();
                var ReportingEntity = reportingService.GetPDFCertificadoHabilitacion("CertificadoHabilitacion", id_solicitud, "'" + nro_expediente + "'", impresionDePrueba);
                documento = ReportingEntity.Reporte;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return documento;
            /*byte[] documento = null;

            DGHP_Entities db = new DGHP_Entities();

            Stream msPdfDisposicion = null;

            dsImpresionCertificadoHabilitacion dsPlancheta = GenerarDataSet(id_solicitud, nro_expediente, impresionDePrueba);


            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
            try
            {
                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/CertificadoHabilitacion.rpt";
                CrystalReportSource1.ReportDocument.SetDataSource(dsPlancheta);
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
                try
                {
                    CrystalReportSource1.ReportDocument.Close();
                    CrystalReportSource1.ReportDocument.Dispose();
                    CrystalReportSource1.Dispose();
                }
                catch { }

                msPdfDisposicion = null;
                throw ex;
            }
            return documento;*/
        }

        public static dsImpresionCertificadoHabilitacion GenerarDataSet(int id_solicitud, string nro_expediente, bool impresionDePrueba)
        {
            dsImpresionCertificadoHabilitacion ds = new dsImpresionCertificadoHabilitacion();

            DataRow row;
            DGHP_Entities db = new DGHP_Entities();
            AGC_FilesEntities dbFiles = new AGC_FilesEntities();

            var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).
                OrderByDescending(x => x.id_encomienda).FirstOrDefault();


            //-- -------------------------
            //-- armar plantas a habilitar
            //-- -------------------------
            var plantasHabilitar =
                (
                    from tipsec in db.TipoSector
                    join encplan in db.Encomienda_Plantas on tipsec.Id equals encplan.id_tiposector
                    where encplan.id_encomienda == enc.id_encomienda
                    orderby tipsec.Id
                    select new
                    {
                        id_tiposector = tipsec.Id,
                        Seleccionado = (encplan.id_tiposector > 0) ? true : false,
                        tipsec.Descripcion,
                        MuestraCampoAdicional = tipsec.MuestraCampoAdicional,
                        detalle = encplan.detalle_encomiendatiposector,
                        TamanoCampoAdicional = tipsec.TamanoCampoAdicional
                    }

                ).ToList();

            string strPlantasHabilitar = "";
            string separador = "";

            foreach (var item in plantasHabilitar)
            {
                if (string.IsNullOrEmpty(strPlantasHabilitar))
                    separador = "";
                else
                    separador = ", ";

                if (item.MuestraCampoAdicional.HasValue && item.MuestraCampoAdicional.Value)
                {
                    if (item.TamanoCampoAdicional.HasValue && item.TamanoCampoAdicional >= 10)
                        strPlantasHabilitar = strPlantasHabilitar + separador + item.detalle;
                    else
                        strPlantasHabilitar = strPlantasHabilitar + separador + item.Descripcion + ' ' + item.detalle;
                }
                else
                {
                    strPlantasHabilitar = strPlantasHabilitar + separador + item.Descripcion;
                }

            }

            #region cargar datos de solicitud
            var sol = db.SSIT_Solicitudes.First(x => x.id_solicitud == id_solicitud);
            var NroCertificado = sol.NroCertificado;
            if (NroCertificado == null)
            {
                NroCertificado = db.SSIT_Solicitudes.Max(x => x.NroCertificado);
                if (NroCertificado == null)
                    NroCertificado = 1;
                else
                    NroCertificado = NroCertificado + 1;
                sol.NroCertificado = NroCertificado;
                db.SaveChanges();

            }
            DataTable dtDatosPlancheta = ds.Tables["solicitud"];
            row = dtDatosPlancheta.NewRow();
            row["id_solicitud"] = id_solicitud;
            row["nro_expediente"] = nro_expediente;
            row["nro_certtificado"] = NroCertificado.Value;
            row["ImpresionDePrueba"] = impresionDePrueba;
            row["Fecha"] = DateTime.Now;
            dtDatosPlancheta.Rows.Add(row);
            #endregion
            DataTable dtEncomienda = ds.Tables["Encomienda"];
            row = dtEncomienda.NewRow();
            row["id_encomienda"] = enc.id_encomienda;
            row["id_solicitud"] = id_solicitud;
            row["PlantasHabilitar"] = Functions.NVL(strPlantasHabilitar);

            dtEncomienda.Rows.Add(row);

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
                row["id_solicitud"] = item.id_solicitud;
                row["Seccion"] = Functions.NVL(item.Seccion);
                row["Manzana"] = item.Manzana;
                row["Parcela"] = item.Parcela;
                row["NroPartidaMatriz"] = Functions.NVL(item.NroPartidaMatriz);
                row["local_subtipoubicacion"] = item.local_subtipoubicacion;
                row["ZonaParcela"] = item.ZonaParcela;
                row["Direcciones"] = direccion;
                row["DeptoLocal"] = item.DeptoLocal;
                dtUbicaciones.Rows.Add(row);
            }

            // Ubicaciones_PropiedadHorizontal

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
                if (item.NroPartidaHorizontal != null)
                    row["NroPartidaHorizontal"] = item.NroPartidaHorizontal;
                else
                    row["NroPartidaHorizontal"] = DBNull.Value;
                row["Piso"] = item.Piso;
                row["Depto"] = item.Depto;
                dtPH.Rows.Add(row);
            }


            // Titulares
            var query_Titulares1 =
                (
                    from pj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas
                    where pj.id_solicitud == id_solicitud
                    select new
                    {
                        id_persona = pj.id_personajuridica,
                        pj.id_solicitud,
                        TipoPersona = "PJ",
                        RazonSocial = pj.Razon_Social,
                        MuestraEnPlancheta = (pj.Id_TipoSociedad == 2 || pj.Id_TipoSociedad == 32) ? false : true
                    }
                ).ToList();


            DataTable dtTitulares = ds.Tables["Titulares"];


            foreach (var item in query_Titulares1)
            {
                row = dtTitulares.NewRow();

                row["id_persona"] = item.id_persona;
                row["id_solicitud"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["RazonSocial"] = item.RazonSocial.ToUpper();
                row["Apellido"] = "";
                row["Nombres"] = "";
                row["MuestraEnPlancheta"] = item.MuestraEnPlancheta;

                dtTitulares.Rows.Add(row);
            }

            var query_Titulares2 =
                (
                    from pj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas
                    join titpj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                    where titpj.id_solicitud == id_solicitud && (titpj.Id_TipoSociedad == 2 || titpj.Id_TipoSociedad == 32)
                    select new
                    {
                        id_persona = pj.id_firmante_pj,
                        titpj.id_solicitud,
                        TipoPersona = "PF",
                        Apellido = pj.Apellido,
                        Nombres = pj.Nombres,
                        MuestraEnPlancheta = true
                    }
                ).Distinct().ToList();

            foreach (var item in query_Titulares2)
            {
                row = dtTitulares.NewRow();

                row["id_persona"] = item.id_persona;
                row["id_solicitud"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["RazonSocial"] = "";
                row["Apellido"] = item.Apellido;
                row["Nombres"] = item.Nombres;
                row["MuestraEnPlancheta"] = item.MuestraEnPlancheta;
                dtTitulares.Rows.Add(row);
            }

            var query_Titulares3 =
                (
                    from pf in db.SSIT_Solicitudes_Titulares_PersonasFisicas
                    join tdoc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pf.id_solicitud == id_solicitud
                    select new
                    {
                        id_persona = pf.id_personafisica,
                        pf.id_solicitud,
                        TipoPersona = "PF",
                        Apellido = pf.Apellido,
                        Nombres = pf.Nombres,
                        MuestraEnPlancheta = true
                    }
                ).ToList();

            foreach (var item in query_Titulares3)
            {
                row = dtTitulares.NewRow();

                row["id_persona"] = item.id_persona;
                row["id_solicitud"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["RazonSocial"] = "";
                row["Apellido"] = item.Apellido;
                row["Nombres"] = item.Nombres;
                row["MuestraEnPlancheta"] = item.MuestraEnPlancheta;

                dtTitulares.Rows.Add(row);
            }

            // Rubros
            var query_Rubros =
                (
                    from rub in db.Encomienda_Rubros 
                    join tact in db.TipoActividad on rub.id_tipoactividad equals tact.Id
                    join docreq in db.Tipo_Documentacion_Req on rub.id_tipodocreq equals docreq.Id
                    where rub.id_encomienda == enc.id_encomienda
                    select new
                    {
                        rub.id_encomiendarubro,
                        rub.id_encomienda,
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

                row["id_encomiendarubro"] = item.id_encomiendarubro;
                row["id_encomienda"] = item.id_encomienda;
                row["cod_rubro"] = item.cod_rubro;
                row["desc_rubro"] = item.desc_rubro;
                row["EsAnterior"] = item.EsAnterior;
                row["TipoActividad"] = item.TipoActividad;
                row["DocRequerida"] = item.DocRequerida;
                row["SuperficieHabilitar"] = item.SuperficieHabilitar;

                dtRubros.Rows.Add(row);
            }

            // DatosLocal

            var query_DatosLocal =
                (
                    from dl in db.Encomienda_DatosLocal
                    where dl.id_encomienda == enc.id_encomienda
                    select new
                    {
                        dl.id_encomiendadatoslocal,
                        enc.id_encomienda,
                        dl.superficie_cubierta_dl,
                        dl.superficie_descubierta_dl,
                        dl.cantidad_operarios_dl
                    }
                ).ToList();

            DataTable dtDatosLocal = ds.Tables["DatosLocal"];

            foreach (var item in query_DatosLocal)
            {
                row = dtDatosLocal.NewRow();

                row["id_encomiendadatoslocal"] = item.id_encomiendadatoslocal;
                row["id_encomienda"] = item.id_encomienda;
                row["superficie_cubierta_dl"] = item.superficie_cubierta_dl;
                row["superficie_descubierta_dl"] = item.superficie_descubierta_dl;
                if (item.cantidad_operarios_dl.HasValue)
                    row["cantidad_operarios_dl"] = item.cantidad_operarios_dl;
                else
                    row["cantidad_operarios_dl"] = DBNull.Value;
                dtDatosLocal.Rows.Add(row);
            }
            db.Dispose();
            dbFiles.Dispose();
            return ds;
        }
        #endregion

        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
    }
}