using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using SGI.App_Data;

namespace SGI.Model
{
    public class PDFInhibicion
    {
        public class clsUbicInhibidas
        {
            public int? Partida { get; set; }
            public int? Seccion { get; set; }
            public string Manzana { get; set; }
            public string Parcela { get; set; }
            public string Motivo { get; set; }
            public DateTime FechaInhibicion { get; set; }
            public string Resultado { get; set; }
            public string Observaciones { get; set; }
            public DateTime? FechaVencimiento { get; set; }
            public string MotivoLevantamiento { get; set; }
            public int id_ubicacion { get; set; }
        }


        private static dsReporteUbicacionesInhibidas GenerarDataSetUbicacionesInhibida(string Tipo, int id_ubicinhibida)
        {
            dsReporteUbicacionesInhibidas ds = new dsReporteUbicacionesInhibidas();

            DataRow row;
            DGHP_Entities db = new DGHP_Entities();

            clsUbicInhibidas ubicInhi = null;

            if (Tipo == "PM")
            {


                ubicInhi = (from inhi in db.Ubicaciones_Inhibiciones
                            join ubi in db.Ubicaciones on inhi.id_ubicacion equals ubi.id_ubicacion
                            where inhi.id_ubicinhibi == id_ubicinhibida
                            select new clsUbicInhibidas
                            {
                                id_ubicacion = ubi.id_ubicacion,
                                Partida = ubi.NroPartidaMatriz,
                                Seccion = ubi.Seccion,
                                Manzana = ubi.Manzana,
                                Parcela = ubi.Parcela,
                                Motivo = inhi.motivo,
                                FechaInhibicion = inhi.fecha_inhibicion,
                                Resultado = inhi.resultado,
                                Observaciones = inhi.observaciones,
                                FechaVencimiento = inhi.fecha_vencimiento,
                                MotivoLevantamiento = inhi.MotivoLevantamiento,
                            }).FirstOrDefault();
            }
            else
            {
                ubicInhi = (from inhi in db.Ubicaciones_PropiedadHorizontal_Inhibiciones
                            join ubiPH in db.Ubicaciones_PropiedadHorizontal on inhi.id_propiedadhorizontal equals ubiPH.id_propiedadhorizontal
                            join ubi in db.Ubicaciones on ubiPH.id_ubicacion equals ubi.id_ubicacion
                            where inhi.id_ubicphorinhibi == id_ubicinhibida
                            select new clsUbicInhibidas
                            {
                                id_ubicacion = ubi.id_ubicacion,
                                Partida = ubiPH.NroPartidaHorizontal,
                                Seccion = ubi.Seccion,
                                Manzana = ubi.Manzana,
                                Parcela = ubi.Parcela,
                                Motivo = inhi.motivo,
                                FechaInhibicion = inhi.fecha_inhibicion,
                                Resultado = inhi.resultado,
                                Observaciones = inhi.observaciones,
                                FechaVencimiento = inhi.fecha_vencimiento,
                                MotivoLevantamiento = inhi.MotivoLevantamiento,
                            }).FirstOrDefault();
            }


            string sqlQuery = "SELECT [dbo].[Ubicaciones_DireccionesPartidas] ({0})";
            Object[] parameters = { ubicInhi.id_ubicacion };
            var Calle = db.Database.SqlQuery<string>(sqlQuery, parameters).FirstOrDefault();



            DataTable dtUbicacionesInhibidas = ds.Tables["Datos"];
            row = dtUbicacionesInhibidas.NewRow();

            row["Tipo"] = Tipo;
            row["Partida"] = ubicInhi.Partida;
            row["Seccion"] = ubicInhi.Seccion;
            row["Manzana"] = ubicInhi.Manzana;
            row["Parcela"] = ubicInhi.Parcela;
            row["Motivo"] = ubicInhi.Motivo;
            row["FechaInhibicion"] = ubicInhi.FechaInhibicion;
            row["Resultado"] = ubicInhi.Resultado;
            row["Observaciones"] = ubicInhi.Observaciones;
            row["FechaVencimiento"] = ubicInhi.FechaVencimiento;
            row["MotivoLevantamiento"] = ubicInhi.MotivoLevantamiento;
            row["Calle"] = Calle;

            dtUbicacionesInhibidas.Rows.Add(row);

            db.Dispose();

            return ds;
        }

        private static dsReportePersonasInhibidas GenerarDataSetPersonaInhibida(int id_personainhibida)
        {
            dsReportePersonasInhibidas ds = new dsReportePersonasInhibidas();

            DataRow row;
            DGHP_Entities db = new DGHP_Entities();

            var domains = (from per in db.PersonasInhibidas
                           join tdp in db.TipoDocumentoPersonal on per.id_tipodoc_personal equals tdp.TipoDocumentoPersonalId into tdp_join
                           from tdp in tdp_join.DefaultIfEmpty()
                           join tp in db.TipoPersona on per.id_tipopersona equals tp.Id_TipoPersona into tp_join
                           from tp in tp_join.DefaultIfEmpty()
                           where per.id_personainhibida == id_personainhibida
                           select new
                           {
                               TipoDoc = tdp.Nombre,
                               NroDoc = per.nrodoc_personainhibida,
                               CUIT = per.cuit_personainhibida,
                               Nombre = per.nomape_personainhibida,
                               FechaRegistro = per.fecharegistro_personainhibida,
                               FechaVencimiento = per.fechavencimiento_personainhibida,
                               Autos = per.autos_personainhibida,
                               Juzgado = per.juzgado_personainhibida,
                               Secretaria = per.secretaria_personainhibida,
                               Estado = per.estado_personainhibida == 1 ? "Activo" : "Dado de baja",
                               FechaBaja = per.fechabaja_personainhibida,
                               Operador = per.operador_personainhibida,
                               Observaciones = per.observaciones_personainhibida,
                               MotivoLevantamiento = per.MotivoLevantamiento,
                               TipoPersona = tp.Nombre
                           }).FirstOrDefault();


            DataTable dtPersonasInhibidas = ds.Tables["Datos"];
            row = dtPersonasInhibidas.NewRow();

            row["TipoDoc"] = domains.TipoDoc;
            row["NroDoc"] = domains.NroDoc;
            row["CUIT"] = domains.CUIT;
            row["Nombre"] = domains.Nombre;
            row["FechaRegistro"] = domains.FechaRegistro;
            row["FechaVencimiento"] = domains.FechaVencimiento;
            row["Autos"] = domains.Autos;
            row["Juzgado"] = domains.Juzgado;
            row["Secretaria"] = domains.Secretaria;
            row["Estado"] = domains.Estado;
            row["FechaBaja"] = domains.FechaBaja;
            row["Operador"] = domains.Operador;
            row["Observaciones"] = domains.Observaciones;
            row["MotivoLevantamiento"] = domains.MotivoLevantamiento;
            row["TipoPersona"] = domains.TipoPersona;

            dtPersonasInhibidas.Rows.Add(row);

            db.Dispose();

            return ds;
        }


        public static byte[] GetPersonasInhibidas(int id_personainhibida)
        {
            byte[] documento = null;

            DGHP_Entities db = new DGHP_Entities();

            Stream msPdfPersonasInhibidas = null;
            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();

            try
            {
                dsReportePersonasInhibidas dsPerInhibidas = GenerarDataSetPersonaInhibida(id_personainhibida);


                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/PersonasInhibidas.rpt";
                CrystalReportSource1.ReportDocument.SetDataSource(dsPerInhibidas);
                msPdfPersonasInhibidas = CrystalReportSource1.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                //se liberan recursos porque el crystal esta configurado para 65 instancias en registry
                CrystalReportSource1.ReportDocument.Close();

                if (CrystalReportSource1 != null)
                {
                    CrystalReportSource1.ReportDocument.Close();
                    CrystalReportSource1.ReportDocument.Dispose();
                    CrystalReportSource1.Dispose();
                }

                documento = Functions.StreamToArray(msPdfPersonasInhibidas);

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

                msPdfPersonasInhibidas = null;
                throw ex;
            }
            return documento;
        }

        public static byte[] GetUbicacionesInhibidas(string Tipo, int id_ubicinhibida)
        {
            byte[] documento = null;

            DGHP_Entities db = new DGHP_Entities();

            Stream msPdfUbicacionesInhibidas = null;
            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();

            try
            {
                dsReporteUbicacionesInhibidas dsUbicInhibidas = GenerarDataSetUbicacionesInhibida(Tipo, id_ubicinhibida);


                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/UbicacionesInhibidas.rpt";
                CrystalReportSource1.ReportDocument.SetDataSource(dsUbicInhibidas);
                msPdfUbicacionesInhibidas = CrystalReportSource1.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                //se liberan recursos porque el crystal esta configurado para 65 instancias en registry
                CrystalReportSource1.ReportDocument.Close();

                if (CrystalReportSource1 != null)
                {
                    CrystalReportSource1.ReportDocument.Close();
                    CrystalReportSource1.ReportDocument.Dispose();
                    CrystalReportSource1.Dispose();
                }

                documento = Functions.StreamToArray(msPdfUbicacionesInhibidas);

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

                msPdfUbicacionesInhibidas = null;
                throw ex;
            }
            return documento;
        }



    }
}