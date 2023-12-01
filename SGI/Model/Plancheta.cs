using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

using SGI.App_Data;
using SGI.WebServices;
using SGI.Webservices.ws_interface_AGC;

using ThoughtWorks.QRCode.Codec;
using ExternalService.Class;
using System.Threading.Tasks;

namespace SGI.Model
{
    public class Plancheta
    {
        #region habilitaciones
        public async static Task<byte[]> GenerarPdfPlanchetahabilitacion(int id_solicitud, int id_tramitetarea, int id_encomienda, string nro_expediente, bool impresionDePrueba)
        {
            byte[] documento = null;

            DGHP_Entities db = new DGHP_Entities();

            Stream msPdfDisposicion = null;

            dsImpresionPlanchetaHabilitacion dsPlancheta = await GenerarDataSetPlanchetahabilitacion(id_encomienda, id_solicitud, id_tramitetarea, nro_expediente, impresionDePrueba);


            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
            try
            {
                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/PlanchetaHabilitacion.rpt";
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
            return documento;
        }

        public async static Task<dsImpresionPlanchetaHabilitacion> GenerarDataSetPlanchetahabilitacion(int id_encomienda, int id_solicitud, int id_tramitetarea, string nro_expediente, bool impresionDePrueba)
        {
            dsImpresionPlanchetaHabilitacion ds = new dsImpresionPlanchetaHabilitacion();

            DataRow row;
            string obs_plancheta_gerente = Buscar_ObservacionPlancheta(id_solicitud, id_tramitetarea);
            DGHP_Entities db = new DGHP_Entities();
            AGC_FilesEntities dbFiles = new AGC_FilesEntities();

            //-- -------------------------
            //-- armar plantas a habilitar
            //-- -------------------------
            var plantasHabilitar =
                (
                    from tipsec in db.TipoSector
                    join encplan in db.Encomienda_Plantas on tipsec.Id equals encplan.id_tiposector
                    where encplan.id_encomienda == id_encomienda
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



            #region cargar datos de plancheta

            int[] tareas = new int[12] { (int)SGI.Constants.ENG_Tareas.SSP_Calificar, (int)SGI.Constants.ENG_Tareas.SCP_Calificar,
                                        (int)SGI.Constants.ENG_Tareas.SSP_Calificar_Nuevo, (int)SGI.Constants.ENG_Tareas.SCP_Calificar_Nuevo,
                                        (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1, (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2,
                                        (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1, (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2,
                                        (int)SGI.Constants.ENG_Tareas.ESP_Calificar_1_Nuevo, (int)SGI.Constants.ENG_Tareas.ESP_Calificar_2_Nuevo,
                                        (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_1_Nuevo, (int)SGI.Constants.ENG_Tareas.ESPAR_Calificar_2_Nuevo
            };
            List<TramiteTareaAnteriores> list_tramite_tarea = TramiteTareaAnteriores.BuscarUltimoTramiteTareaPorTarea((int)Constants.GruposDeTramite.HAB,
            id_solicitud, id_tramitetarea, tareas);

            int id_tramitetarea_calif = list_tramite_tarea.Count() > 0 ? list_tramite_tarea[0].id_tramitetarea : 0;
            var q_calif =
                (
                    from tt in db.SGI_Tramites_Tareas
                    join up in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals up.userid
                    where tt.id_tramitetarea == id_tramitetarea_calif
                    select new
                    {
                        up.Nombres,
                        up.Apellido
                    }

                ).FirstOrDefault();

            //ws_Interface_AGC servicio = new ws_Interface_AGC();
            //SGI.Webservices.ws_interface_AGC.wsResultado ws_resultado_CAA = new SGI.Webservices.ws_interface_AGC.wsResultado();

            //servicio.Url = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC");
            //string username_servicio = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.User");
            //string password_servicio = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.Password");
            List<int> ids = new List<int>();
            ids.Add(id_encomienda);
            List<GetCAAsByEncomiendasResponse> listCaa = await GetCAAsByEncomiendas(ids.ToArray());

            //-- -------------------------
            //-- Datos encomienda, tabla 0
            //-- -------------------------

            DataTable dtDatosPlancheta = ds.Tables["datos_plancheta"];
            row = dtDatosPlancheta.NewRow();
            row["id_solicitud"] = id_solicitud;
            row["nro_expediente"] = nro_expediente;
            row["calificador"] = q_calif != null ? q_calif.Apellido + ", " + q_calif.Nombres : "";
            if (listCaa.Count() > 0)
            {
                string anio = Convert.ToString(listCaa[0].fechaIngreso.Year);
                row["nro_cert_ap_amb"] = listCaa[0].id_tipocertificado + ": TRW-" + listCaa[0].id_solicitud + "-APRA-" + anio;
            }
            else
                row["nro_cert_ap_amb"] = "";
            dtDatosPlancheta.Rows.Add(row);

            #endregion

            //-- -------------------------
            //-- Datos encomienda, tabla 0
            //-- -------------------------


            var query_enc =
                (
                    from enc in db.Encomienda
                    join tt in db.TipoTramite on enc.id_tipotramite equals tt.id_tipotramite
                    join te in db.TipoExpediente on enc.id_tipoexpediente equals te.id_tipoexpediente
                    join ste in db.SubtipoExpediente on enc.id_subtipoexpediente equals ste.id_subtipoexpediente
                    join prof in db.Profesional on enc.id_profesional equals prof.Id
                    join tdoc in db.TipoDocumentoPersonal on prof.IdTipoDocumento equals tdoc.TipoDocumentoPersonalId
                    join cprof in db.ConsejoProfesional on enc.id_consejo equals cprof.Id
                    join grucon in db.GrupoConsejos on cprof.id_grupoconsejo equals grucon.id_grupoconsejo

                    join encnorm in db.Encomienda_Normativas on enc.id_encomienda equals encnorm.id_encomienda into left_1
                    from encnorm in left_1.DefaultIfEmpty()
                    join tipnorm in db.TipoNormativa on encnorm.id_tiponormativa equals tipnorm.Id into left_2
                    from tipnorm in left_2.DefaultIfEmpty()
                    join entnorm in db.EntidadNormativa on encnorm.id_entidadnormativa equals entnorm.Id into left_3
                    from entnorm in left_3.DefaultIfEmpty()
                    where enc.id_encomienda == id_encomienda
                    select new
                    {
                        id_encomienda = enc.id_encomienda,
                        FechaEncomienda = enc.FechaEncomienda,
                        nroEncomiendaconsejo = enc.nroEncomiendaconsejo,
                        ZonaDeclarada = enc.ZonaDeclarada,
                        TipoDeTramite = tt.cod_tipotramite,
                        TipoDeExpediente = te.cod_tipoexpediente,
                        SubTipoDeExpediente = ste.cod_subtipoexpediente,
                        MatriculaProfesional = prof.Matricula,
                        ApellidoProfesional = prof.Apellido,
                        NombresProfesional = prof.Nombre,
                        TipoDocProfesional = tdoc.Nombre,
                        DocumentoProfesional = prof.NroDocumento,
                        id_grupoconsejo = grucon.id_grupoconsejo,
                        ConsejoProfesional = grucon.nombre_grupoconsejo,
                        TipoNormativa = tipnorm.Descripcion,
                        EntidadNormativa = entnorm.Descripcion,
                        NroNormativa = encnorm.nro_normativa,
                        LogoUrl = "",
                        ImpresionDePrueba = (enc.id_estado <= 1) ? true : false,
                        PlantasHabilitar = strPlantasHabilitar,
                        ObservacionesPlantasHabilitar = enc.Observaciones_plantas,
                        ObservacionesRubros = enc.Observaciones_rubros,
                        id_encomienda_anterior = 0,
                        NroExpediente = ""
                    }

                ).ToList();

            Control ctl = new Control();

            //Genera los Qr.
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            byte[] str1Byte = Encoding.Default.GetBytes(Convert.ToString(id_encomienda));
            string base64 = Convert.ToBase64String(str1Byte);

            string urlws = SGI.Parametros.GetParam_ValorChar("WebService.Url");

            string urlOblea = string.Format("{0}/{1}/{2}", urlws, "GetTramiteQrSGI", base64);

            //urlOblea = IPtoDomain(urlOblea);
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 7;      // 316 x 316 pixels 
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;

            byte[] imagen_bytes = null;

            if (urlOblea != null)
            {
                System.Drawing.Image imagenQr = qrCodeEncoder.Encode(urlOblea);
                imagen_bytes = imageToByteArray(imagenQr);

            }
            //Fin generacion qr

            DataTable dtEncomienda = ds.Tables["Encomienda"];
            foreach (var item in query_enc)
            {
                row = dtEncomienda.NewRow();

                row["id_encomienda"] = item.id_encomienda;
                row["FechaEncomienda"] = item.FechaEncomienda;
                row["nroEncomiendaconsejo"] = item.nroEncomiendaconsejo;
                row["ZonaDeclarada"] = Functions.NVL(item.ZonaDeclarada);
                row["TipoDeTramite"] = item.TipoDeTramite;
                row["TipoDeExpediente"] = item.TipoDeExpediente;
                row["SubTipoDeExpediente"] = item.SubTipoDeExpediente;
                row["MatriculaProfesional"] = Functions.NVL(item.MatriculaProfesional);
                row["ApellidoProfesional"] = item.ApellidoProfesional;
                row["NombresProfesional"] = item.NombresProfesional;
                row["TipoDocProfesional"] = item.TipoDocProfesional;
                row["DocumentoProfesional"] = Functions.NVL(item.DocumentoProfesional);
                row["id_grupoconsejo"] = item.id_grupoconsejo;
                row["ConsejoProfesional"] = item.ConsejoProfesional;
                row["TipoNormativa"] = Functions.NVL(item.TipoNormativa);
                row["EntidadNormativa"] = Functions.NVL(item.EntidadNormativa);
                row["NroNormativa"] = Functions.NVL(item.NroNormativa);
                row["LogoUrl"] = item.LogoUrl;
                row["ImpresionDePrueba"] = impresionDePrueba;
                row["Logo"] = imagen_bytes;
                row["PlantasHabilitar"] = Functions.NVL(item.PlantasHabilitar);
                row["ObservacionesPlantasHabilitar"] = Functions.NVL(item.ObservacionesPlantasHabilitar);
                row["ObservacionesRubros"] = obs_plancheta_gerente;
                row["id_encomienda_anterior"] = Functions.NVL(item.id_encomienda_anterior);
                row["NroExpediente"] = Functions.NVL(item.NroExpediente);

                dtEncomienda.Rows.Add(row);
            }

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
                        // dbo.Encomienda_Solicitud_DireccionesPartidasPlancheta(enc.id_encomienda,encubic.id_ubicacion) 
                        DeptoLocal = solubic.deptoLocal_ubicacion
                    }
                ).ToList();

            DataTable dtUbicaciones = ds.Tables["Ubicaciones"];


            foreach (var item in query_ubi)
            {
                row = dtUbicaciones.NewRow();

                string sql = "select dbo.SSIT_Solicitud_DireccionesPartidasPlancheta(" + item.id_solicitud + "," + item.id_ubicacion + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                row["id_encomiendaubicacion"] = item.id_solicitudubicacion;
                row["id_encomienda"] = id_encomienda;
                row["id_ubicacion"] = Functions.NVL(item.id_ubicacion);
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

                row["id_encomiendaubicacion"] = item.id_solicitudubicacion;
                row["id_ubicacion"] = Functions.NVL(item.id_ubicacion);

                if (item.NroPartidaHorizontal != null)
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

                row["id_encomiendaubicacion"] = item.id_solicitudubicacion;
                row["id_encomiendapuerta"] = item.id_solicitudpuerta;
                row["id_ubicacion"] = item.id_ubicacion;
                row["Calle"] = item.nombre_calle;
                row["NroPuerta"] = item.NroPuerta;

                dtPuerta.Rows.Add(row);
            }


            // ConformacionLocal

            var query_ConformacionLocal =
                (
                    from enc in db.Encomienda
                    join conf in db.Encomienda_ConformacionLocal on enc.id_encomienda equals conf.id_encomienda
                    join dest in db.TipoDestino on conf.id_destino equals dest.Id
                    where enc.id_encomienda == id_encomienda
                    select new
                    {
                        conf.id_encomiendaconflocal,
                        enc.id_encomienda,
                        Destino = dest.Nombre,
                        conf.largo_conflocal,
                        conf.ancho_conflocal,
                        conf.alto_conflocal,
                        conf.Paredes_conflocal,
                        conf.Techos_conflocal,
                        conf.Pisos_conflocal,
                        conf.Frisos_conflocal,
                        conf.Observaciones_conflocal,
                        conf.Detalle_conflocal
                    }
                ).ToList();

            DataTable dtConformacionLocal = ds.Tables["ConformacionLocal"];

            foreach (var item in query_ConformacionLocal)
            {
                row = dtConformacionLocal.NewRow();

                row["id_encomiendaconflocal"] = item.id_encomiendaconflocal;
                row["id_encomienda"] = item.id_encomienda;
                row["Destino"] = item.Destino;
                row["largo_conflocal"] = item.largo_conflocal;
                row["ancho_conflocal"] = item.ancho_conflocal;
                row["alto_conflocal"] = item.alto_conflocal;
                row["Paredes_conflocal"] = item.Paredes_conflocal;
                row["Techos_conflocal"] = item.Techos_conflocal;
                row["Pisos_conflocal"] = item.Pisos_conflocal;
                row["Frisos_conflocal"] = item.Frisos_conflocal;
                row["Observaciones_conflocal"] = item.Observaciones_conflocal;
                row["Detalle_conflocal"] = item.Detalle_conflocal;

                dtConformacionLocal.Rows.Add(row);
            }


            // Firmantes

            var query_Firmantes1 =
                (
                    from pj in db.SSIT_Solicitudes_Firmantes_PersonasJuridicas
                    join titpj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                    join tcl in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tcl.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pj.id_solicitud == id_solicitud && titpj.Id_TipoSociedad != 2 // Todas las sociedades menos la Sociedad de Hecho
                    && titpj.Id_TipoSociedad != 32 //Sociedad no constituidas regularmente (Sec. 4ª Ley 19550)
                    select new
                    {
                        id_firmante = pj.id_firmante_pj,
                        pj.id_solicitud,
                        TipoPersona = "PJ",
                        Apellido = pj.Apellido,
                        Nombres = pj.Nombres,
                        TipoDoc = tdoc.Nombre,
                        NroDoc = pj.Nro_Documento,
                        CaracterLegal = tcl.nom_tipocaracter,
                        pj.cargo_firmante_pj,
                        FirmanteDe = titpj.Razon_Social
                    }
                ).Distinct().ToList();


            DataTable dtFirmantes = ds.Tables["Firmantes"];

            foreach (var item in query_Firmantes1)
            {
                row = dtFirmantes.NewRow();

                row["id_firmante"] = item.id_firmante;
                row["id_encomienda"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                //row["id_persona"] = null;
                row["Apellido"] = item.Apellido.ToUpper();
                row["Nombres"] = item.Nombres.ToUpper();
                row["TipoDoc"] = item.TipoDoc;
                row["NroDoc"] = item.NroDoc;
                row["CaracterLegal"] = item.CaracterLegal;
                row["cargo_firmante_pj"] = item.cargo_firmante_pj;
                row["FirmanteDe"] = item.FirmanteDe.ToUpper();

                dtFirmantes.Rows.Add(row);
            }

            var query_Firmantes2 =
                (
                    from pj in db.SSIT_Solicitudes_Firmantes_PersonasJuridicas
                    join titpj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                    join titsh in db.SSIT_Solicitudes_Titulares_PersonasJuridicas_PersonasFisicas on pj.id_firmante_pj equals titsh.id_firmante_pj
                    join tcl in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tcl.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pj.id_solicitud == id_solicitud && (titpj.Id_TipoSociedad == 2 // Solo Sociedades de Hecho
                    || titpj.Id_TipoSociedad == 32)//Sociedad no constituidas regularmente (Sec. 4ª Ley 19550)
                    select new
                    {
                        id_firmante = pj.id_firmante_pj,
                        pj.id_solicitud,
                        TipoPersona = "PJ",
                        Apellido = pj.Apellido,
                        Nombres = pj.Nombres,
                        TipoDoc = tdoc.Nombre,
                        NroDoc = pj.Nro_Documento,
                        CaracterLegal = tcl.nom_tipocaracter,
                        pj.cargo_firmante_pj,
                        FirmanteDe = titsh.Apellido + ", " + titsh.Nombres
                    }
                ).Distinct().ToList();


            foreach (var item in query_Firmantes2)
            {
                row = dtFirmantes.NewRow();

                row["id_firmante"] = item.id_firmante;
                row["id_encomienda"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                //row["id_persona"] = null;
                row["Apellido"] = item.Apellido.ToUpper();
                row["Nombres"] = item.Nombres.ToUpper();
                row["TipoDoc"] = item.TipoDoc;
                row["NroDoc"] = item.NroDoc;
                row["CaracterLegal"] = item.CaracterLegal;
                row["cargo_firmante_pj"] = item.cargo_firmante_pj;
                row["FirmanteDe"] = item.FirmanteDe.ToUpper();

                dtFirmantes.Rows.Add(row);
            }

            var query_Firmantes3 =
                (
                    from pf in db.SSIT_Solicitudes_Firmantes_PersonasFisicas
                    join titpf in db.SSIT_Solicitudes_Titulares_PersonasFisicas on pf.id_personafisica equals titpf.id_personafisica
                    join tcl in db.TiposDeCaracterLegal on pf.id_tipocaracter equals tcl.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pf.id_solicitud == id_solicitud
                    select new
                    {
                        id_firmante = pf.id_firmante_pf,
                        pf.id_solicitud,
                        TipoPersona = "PF",
                        Apellido = pf.Apellido,
                        Nombres = pf.Nombres,
                        TipoDoc = tdoc.Nombre,
                        NroDoc = pf.Nro_Documento,
                        CaracterLegal = tcl.nom_tipocaracter,
                        cargo_firmante_pj = "",
                        FirmanteDe = titpf.Apellido + ", " + titpf.Nombres
                    }
                ).ToList();

            foreach (var item in query_Firmantes3)
            {
                row = dtFirmantes.NewRow();

                row["id_firmante"] = item.id_firmante;
                row["id_encomienda"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["id_persona"] = DBNull.Value;
                row["Apellido"] = item.Apellido.ToUpper();
                row["Nombres"] = item.Nombres.ToUpper();
                row["TipoDoc"] = item.TipoDoc;
                row["NroDoc"] = item.NroDoc;
                row["CaracterLegal"] = item.CaracterLegal;
                row["cargo_firmante_pj"] = item.cargo_firmante_pj;
                row["FirmanteDe"] = item.FirmanteDe.ToUpper();

                dtFirmantes.Rows.Add(row);
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
                        TipoSociedad = tsoc.Descripcion,
                        TipoIIBB = tipoiibb.nom_tipoiibb,
                        NroIIBB = pj.Nro_IIBB,
                        pj.CUIT,
                        MuestraEnTitulares = 1,
                        MuestraEnPlancheta = true
                    }
                ).ToList();


            DataTable dtTitulares = ds.Tables["Titulares"];


            foreach (var item in query_Titulares1)
            {
                row = dtTitulares.NewRow();

                row["id_persona"] = item.id_persona;
                row["id_encomienda"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["TipoSociedad"] = item.TipoSociedad;
                row["RazonSocial"] = item.RazonSocial.ToUpper();
                row["Apellido"] = "";
                row["Nombres"] = "";
                row["TipoDoc"] = "";
                row["NroDoc"] = 0;
                row["TipoIIBB"] = item.TipoIIBB;
                row["NroIIBB"] = item.NroIIBB;
                row["Cuit"] = item.CUIT;
                row["MuestraEnTitulares"] = item.MuestraEnTitulares;
                row["MuestraEnPlancheta"] = item.MuestraEnPlancheta;

                dtTitulares.Rows.Add(row);
            }

            var query_Titulares3 =
                (
                    from pf in db.SSIT_Solicitudes_Titulares_PersonasFisicas
                    join tdoc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    join tipoiibb in db.TiposDeIngresosBrutos on pf.id_tipoiibb equals tipoiibb.id_tipoiibb
                    where pf.id_solicitud == id_solicitud
                    select new
                    {
                        id_persona = pf.id_personafisica,
                        pf.id_solicitud,
                        TipoPersona = "PF",
                        Apellido = pf.Apellido,
                        Nombres = pf.Nombres,
                        TipoDoc = tdoc.Nombre,
                        NroDoc = pf.Nro_Documento,
                        TipoIIBB = tipoiibb.nom_tipoiibb,
                        NroIIBB = pf.Ingresos_Brutos,
                        pf.Cuit,
                        MuestraEnTitulares = 1,
                        MuestraEnPlancheta = 1
                    }
                ).ToList();

            foreach (var item in query_Titulares3)
            {
                row = dtTitulares.NewRow();

                row["id_persona"] = item.id_persona;
                row["id_encomienda"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["TipoSociedad"] = "";
                row["RazonSocial"] = "";
                row["Apellido"] = item.Apellido;
                row["Nombres"] = item.Nombres;
                row["TipoDoc"] = item.TipoDoc;
                row["NroDoc"] = item.NroDoc;
                row["TipoIIBB"] = item.TipoIIBB;
                row["NroIIBB"] = item.NroIIBB;
                row["Cuit"] = item.Cuit;
                row["MuestraEnTitulares"] = item.MuestraEnTitulares;
                row["MuestraEnPlancheta"] = item.MuestraEnPlancheta;

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
                        id_encomiendarubro = rub.id_encomiendarubro,
                        id_encomienda = enc.id_encomienda,
                        cod_rubro = rub.cod_rubro,
                        desc_rubro = rub.desc_rubro,
                        EsAnterior = rub.EsAnterior,
                        TipoActividad = tact.Nombre,
                        DocRequerida = docreq.Nomenclatura,
                        rub.SuperficieHabilitar
                    }
                ).Union(
                    from enc in db.Encomienda
                    join rubcn in db.Encomienda_RubrosCN on enc.id_encomienda equals rubcn.id_encomienda
                    join tact in db.TipoActividad on rubcn.IdTipoActividad equals tact.Id
                    join rub in db.RubrosCN on rubcn.IdRubro equals rub.IdRubro
                    join gc in db.ENG_Grupos_Circuitos on rub.IdGrupoCircuito equals gc.id_grupo_circuito
                    where enc.id_encomienda == id_encomienda
                    select new
                    {
                        id_encomiendarubro = rubcn.id_encomiendarubro,
                        id_encomienda = enc.id_encomienda,
                        cod_rubro = rubcn.CodigoRubro,
                        desc_rubro = rubcn.NombreRubro,
                        EsAnterior = false,
                        TipoActividad = tact.Nombre,
                        DocRequerida = gc.cod_grupo_circuito,
                        rubcn.SuperficieHabilitar
                    }
                    ).ToList();

            var query_RubrosAnt = ((from encrub in db.Encomienda_Rubros_AT_Anterior
                                    join tdocreq in db.Tipo_Documentacion_Req on encrub.id_tipodocreq equals tdocreq.Id
                                    join tact in db.TipoActividad on encrub.id_tipoactividad equals tact.Id
                                    where encrub.id_encomienda == id_encomienda
                                    orderby encrub.SuperficieHabilitar descending
                                    select new
                                    {
                                        encrub.id_encomienda,
                                        id_encomiendaRubro = encrub.id_encomiendarubro,
                                        encrub.cod_rubro,
                                        encrub.desc_rubro,
                                        encrub.EsAnterior,
                                        TipoActividad = tact.Nombre,
                                        DocRequerida = "",
                                        encrub.SuperficieHabilitar

                                    }).Union(from encrub in db.Encomienda_RubrosCN_AT_Anterior
                                             join r in db.RubrosCN on encrub.CodigoRubro equals r.Codigo
                                             join tact in db.TipoActividad on encrub.IdTipoActividad equals tact.Id
                                             join gc in db.ENG_Grupos_Circuitos on r.IdGrupoCircuito equals gc.id_grupo_circuito
                                             where encrub.id_encomienda == id_encomienda
                                             orderby encrub.SuperficieHabilitar descending
                                             select new
                                             {
                                                 encrub.id_encomienda,
                                                 id_encomiendaRubro = encrub.id_encomiendarubro,
                                                 cod_rubro = encrub.CodigoRubro,
                                                 desc_rubro = encrub.NombreRubro,
                                                 EsAnterior = true,
                                                 TipoActividad = tact.Nombre,
                                                 DocRequerida = "",
                                                 encrub.SuperficieHabilitar

                                             })).ToList();

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

            //rubros anteriores

            foreach (var item in query_RubrosAnt)
            {
                row = dtRubros.NewRow();

                row["id_encomiendarubro"] = item.id_encomiendaRubro;
                row["id_encomienda"] = item.id_encomienda;
                row["cod_rubro"] = item.cod_rubro;
                row["desc_rubro"] = item.desc_rubro;
                row["EsAnterior"] = item.EsAnterior;
                row["TipoActividad"] = item.TipoActividad;
                row["DocRequerida"] = item.DocRequerida;
                row["SuperficieHabilitar"] = item.SuperficieHabilitar;

                dtRubros.Rows.Add(row);
            }
            ///SubRubros

            var query_SubRubros =
                (
                    from enc in db.Encomienda
                    join rubcn in db.Encomienda_RubrosCN on enc.id_encomienda equals rubcn.id_encomienda
                    join srubcn in db.Encomienda_RubrosCN_Subrubros on rubcn.id_encomiendarubro equals srubcn.Id_EncRubro
                    join srub in db.RubrosCN_Subrubros on srubcn.Id_rubrosubrubro equals srub.Id_rubroCNsubrubro
                    where enc.id_encomienda == id_encomienda
                    select new
                    {
                        Id_EncRubCNSubrubros = srubcn.Id_EncRubCNSubrubros,
                        id_encomiendarubro = rubcn.id_encomiendarubro,
                        CodigoRubro = rubcn.CodigoRubro,
                        Id_rubroCNsubrubro = srub.Id_rubroCNsubrubro,
                        Nombre = srub.Nombre
                    }
                    ).ToList();

            DataTable dtSubRubros = ds.Tables["SubRubros"];

            foreach (var item in query_SubRubros)
            {
                row = dtSubRubros.NewRow();

                row["Id_EncRubCNSubrubros"] = item.Id_EncRubCNSubrubros;
                row["id_encomiendarubro"] = item.id_encomiendarubro;
                row["CodigoRubro"] = item.CodigoRubro;
                row["Id_rubroCNsubrubro"] = item.Id_rubroCNsubrubro;
                row["Nombre"] = item.Nombre;

                dtSubRubros.Rows.Add(row);
            }

            //Mixturas
            var query_Mixturas =
                (
                    from enc in db.Encomienda
                    join ubi in db.Encomienda_Ubicaciones on enc.id_encomienda equals ubi.id_encomienda
                    join mix in db.Encomienda_Ubicaciones_Mixturas on ubi.id_encomiendaubicacion equals mix.id_encomiendaubicacion
                    join ubimix in db.Ubicaciones_ZonasMixtura on mix.IdZonaMixtura equals ubimix.IdZonaMixtura
                    where enc.id_encomienda == id_encomienda
                    select new
                    {
                        descrip = ubimix.Codigo
                    }
                    ).ToList();

            DataTable dtMixturas = ds.Tables["Mixturas_Distritos"];

            foreach (var item in query_Mixturas)
            {
                row = dtMixturas.NewRow();
                row["Descripcion"] = item.descrip;

                dtMixturas.Rows.Add(row);
            }

            //Distritos
            var query_Distritos =
               (
                    from enc in db.Encomienda
                    join ubi in db.Encomienda_Ubicaciones on enc.id_encomienda equals ubi.id_encomienda
                    join dis in db.Encomienda_Ubicaciones_Distritos on ubi.id_encomiendaubicacion equals dis.id_encomiendaubicacion
                    join ubidis in db.Ubicaciones_CatalogoDistritos on dis.IdDistrito equals ubidis.IdDistrito
                    where enc.id_encomienda == id_encomienda
                    select new
                    {
                        descrip = ubidis.Codigo
                    }).ToList();

            foreach (var item in query_Distritos)
            {
                row = dtMixturas.NewRow();
                row["Descripcion"] = item.descrip;

                dtMixturas.Rows.Add(row);
            }

            // DatosLocal

            var query_DatosLocal =
                (
                    from enc in db.Encomienda
                    join dl in db.Encomienda_DatosLocal on enc.id_encomienda equals dl.id_encomienda
                    where enc.id_encomienda == id_encomienda
                    select new
                    {
                        dl.id_encomiendadatoslocal,
                        enc.id_encomienda,
                        dl.superficie_cubierta_dl,
                        dl.superficie_descubierta_dl,
                        dl.dimesion_frente_dl,
                        dl.lugar_carga_descarga_dl,
                        dl.estacionamiento_dl,
                        dl.red_transito_pesado_dl,
                        dl.sobre_avenida_dl,
                        dl.materiales_pisos_dl,
                        dl.materiales_paredes_dl,
                        dl.materiales_techos_dl,
                        dl.materiales_revestimientos_dl,
                        dl.sanitarios_ubicacion_dl,
                        dl.sanitarios_distancia_dl,
                        dl.cantidad_sanitarios_dl,
                        dl.superficie_sanitarios_dl,
                        dl.frente_dl,
                        dl.fondo_dl,
                        dl.lateral_izquierdo_dl,
                        dl.lateral_derecho_dl,
                        dl.sobrecarga_corresponde_dl,
                        dl.sobrecarga_tipo_observacion,
                        dl.sobrecarga_requisitos_opcion,
                        dl.sobrecarga_art813_inciso,
                        dl.sobrecarga_art813_item,
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
                row["dimesion_frente_dl"] = item.dimesion_frente_dl;
                row["lugar_carga_descarga_dl"] = item.lugar_carga_descarga_dl;
                row["estacionamiento_dl"] = item.estacionamiento_dl;
                row["red_transito_pesado_dl"] = item.red_transito_pesado_dl;
                row["sobre_avenida_dl"] = item.sobre_avenida_dl;
                row["materiales_pisos_dl"] = item.materiales_pisos_dl;
                row["materiales_paredes_dl"] = item.materiales_paredes_dl;
                row["materiales_techos_dl"] = item.materiales_techos_dl;
                row["materiales_revestimientos_dl"] = item.materiales_revestimientos_dl;
                row["sanitarios_ubicacion_dl"] = item.sanitarios_ubicacion_dl;
                row["sanitarios_distancia_dl"] = item.sanitarios_distancia_dl;
                row["cantidad_sanitarios_dl"] = item.cantidad_sanitarios_dl;
                row["superficie_sanitarios_dl"] = item.superficie_sanitarios_dl;
                row["frente_dl"] = item.frente_dl;
                row["fondo_dl"] = item.fondo_dl;
                row["lateral_izquierdo_dl"] = item.lateral_izquierdo_dl;
                row["lateral_derecho_dl"] = item.lateral_derecho_dl;
                row["sobrecarga_corresponde_dl"] = item.sobrecarga_corresponde_dl;


                row["sobrecarga_corresponde_dl"] = DBNull.Value;


                if (item.sobrecarga_tipo_observacion.HasValue)
                    row["sobrecarga_tipo_observacion"] = item.sobrecarga_tipo_observacion;
                else
                    row["sobrecarga_tipo_observacion"] = DBNull.Value;


                if (item.sobrecarga_requisitos_opcion.HasValue)
                    row["sobrecarga_requisitos_opcion"] = item.sobrecarga_requisitos_opcion;
                else
                    row["sobrecarga_requisitos_opcion"] = DBNull.Value;

                row["sobrecarga_art813_inciso"] = item.sobrecarga_art813_inciso;
                row["sobrecarga_art813_item"] = item.sobrecarga_art813_item;

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

        private static string Buscar_ObservacionPlancheta(int id_solicitud, int id_tramitetarea)
        {
            DGHP_Entities db = new DGHP_Entities();
            string observ = "";

            var q = (
                from tt in db.SGI_Tramites_Tareas
                join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                join dghp in db.SGI_Tarea_Revision_DGHP on tt.id_tramitetarea equals dghp.id_tramitetarea
                where tt_hab.id_solicitud == id_solicitud && tt.id_tramitetarea <= id_tramitetarea
                && !string.IsNullOrEmpty(dghp.observacion_plancheta)
                select new
                {
                    tt.id_tramitetarea,
                    dghp.observacion_plancheta
                }
                ).Union(
                from tt in db.SGI_Tramites_Tareas
                join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                join gerente in db.SGI_Tarea_Revision_Gerente on tt.id_tramitetarea equals gerente.id_tramitetarea
                where tt_hab.id_solicitud == id_solicitud && tt.id_tramitetarea <= id_tramitetarea
                && !string.IsNullOrEmpty(gerente.observacion_plancheta)
                select new
                {
                    tt.id_tramitetarea,
                    gerente.observacion_plancheta
                }
                ).Union(
                from tt in db.SGI_Tramites_Tareas
                join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                join subgerente in db.SGI_Tarea_Revision_SubGerente on tt.id_tramitetarea equals subgerente.id_tramitetarea
                where tt_hab.id_solicitud == id_solicitud && tt.id_tramitetarea <= id_tramitetarea
                && !string.IsNullOrEmpty(subgerente.observacion_plancheta)
                select new
                {
                    tt.id_tramitetarea,
                    subgerente.observacion_plancheta
                }
                ).Union(
                from tt in db.SGI_Tramites_Tareas
                join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                join cal in db.SGI_Tarea_Calificar on tt.id_tramitetarea equals cal.id_tramitetarea
                where tt_hab.id_solicitud == id_solicitud && tt.id_tramitetarea <= id_tramitetarea
                && !string.IsNullOrEmpty(cal.Observaciones)
                select new
                {
                    tt.id_tramitetarea,
                    observacion_plancheta = cal.Observaciones
                }
                );
            var lista = q.ToList().OrderByDescending(x => x.id_tramitetarea).ToList();
            if (lista != null && lista.Count > 0)
            {
                observ = lista[0].observacion_plancheta;
            }
            db.Dispose();
            return observ;
        }
        #endregion

        #region < Plancheta Transferencias >

        public static string GetCalificador(ref DGHP_Entities db, int id_solicitud)
        {
            var calificador = (from tt in db.SGI_Tramites_Tareas
                               join tth in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tth.id_tramitetarea
                               join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                               join usr in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals usr.userid
                               where tth.id_solicitud == id_solicitud && tarea.cod_tarea.ToString().Substring(tarea.cod_tarea.ToString().Length - 2, 2) == SGI.Constants.ENG_Tipos_Tareas_Transf.Calificar
                               select usr.Apellido + ", " + usr.Nombres).FirstOrDefault();

            return calificador;
        }

        public static int GetUltimaEncomiendaAprobada(ref DGHP_Entities db, int id_solicitud)
        {
            int id_encomienda = (from enc in db.Encomienda
                                 join ets in db.Encomienda_Transf_Solicitudes on enc.id_encomienda equals ets.id_encomienda
                                 where ets.id_solicitud == id_solicitud && enc.id_estado == 4
                                 orderby enc.id_encomienda descending
                                 select ets.id_encomienda).FirstOrDefault();

            return id_encomienda;
        }

        public static void LoadRubros(ref DGHP_Entities db, ref dsImpresionPlanchetaTransf ds, int id_solicitud)
        {
            var query_Rubros = (
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

            DataTable dtRubrosCN = ds.Tables["Rubros"];

            DataRow row;
            foreach (var item in query_Rubros)
            {
                row = dtRubrosCN.NewRow();

                row["id_encomiendarubro"] = item.id_cpadronrubro;
                row["id_encomienda"] = item.id_solicitud;
                row["cod_rubro"] = item.cod_rubro;
                row["desc_rubro"] = item.desc_rubro;
                row["EsAnterior"] = item.EsAnterior;
                row["TipoActividad"] = item.TipoActividad;
                row["DocRequerida"] = item.DocRequerida;
                row["SuperficieHabilitar"] = item.SuperficieHabilitar;

                dtRubrosCN.Rows.Add(row);
            }
        }

        public static void LoadRubrosCN(ref DGHP_Entities db, ref dsImpresionPlanchetaTransf ds, int id_encomienda)
        {
            var query_RubrosCN = (
                                 from enc in db.Encomienda
                                 join rub in db.Encomienda_RubrosCN on enc.id_encomienda equals rub.id_encomienda
                                 join tact in db.TipoActividad on rub.IdTipoActividad equals tact.Id
                                 join r in db.RubrosCN on rub.CodigoRubro equals r.Codigo
                                 join gc in db.ENG_Grupos_Circuitos on r.IdGrupoCircuito equals gc.id_grupo_circuito
                                 where enc.id_encomienda == id_encomienda
                                 select new
                                 {
                                     rub.id_encomiendarubro,
                                     enc.id_encomienda,
                                     rub.CodigoRubro,
                                     desc_rubro = rub.NombreRubro,
                                     EsAnterior = 0,
                                     TipoActividad = tact.Nombre,
                                     DocRequerida = gc.cod_grupo_circuito,
                                     rub.SuperficieHabilitar
                                 }
                             ).ToList();

            DataTable dtRubrosCN = ds.Tables["Rubros"];

            var query_RubrosAnt = ((from encrub in db.Encomienda_Rubros_AT_Anterior
                                    join tdocreq in db.Tipo_Documentacion_Req on encrub.id_tipodocreq equals tdocreq.Id
                                    join tact in db.TipoActividad on encrub.id_tipoactividad equals tact.Id
                                    where encrub.id_encomienda == id_encomienda
                                    orderby encrub.SuperficieHabilitar descending
                                    select new
                                    {
                                        encrub.id_encomienda,
                                        id_encomiendaRubro = encrub.id_encomiendarubro,
                                        encrub.cod_rubro,
                                        encrub.desc_rubro,
                                        encrub.EsAnterior,
                                        TipoActividad = tact.Nombre,
                                        DocRequerida = "",
                                        encrub.SuperficieHabilitar

                                    }).Union(from encrub in db.Encomienda_RubrosCN_AT_Anterior
                                             join r in db.RubrosCN on encrub.CodigoRubro equals r.Codigo
                                             join tact in db.TipoActividad on encrub.IdTipoActividad equals tact.Id
                                             join gc in db.ENG_Grupos_Circuitos on r.IdGrupoCircuito equals gc.id_grupo_circuito
                                             where encrub.id_encomienda == id_encomienda
                                             orderby encrub.SuperficieHabilitar descending
                                             select new
                                             {
                                                 encrub.id_encomienda,
                                                 id_encomiendaRubro = encrub.id_encomiendarubro,
                                                 cod_rubro = encrub.CodigoRubro,
                                                 desc_rubro = encrub.NombreRubro,
                                                 EsAnterior = true,
                                                 TipoActividad = tact.Nombre,
                                                 DocRequerida = "",
                                                 encrub.SuperficieHabilitar

                                             })).ToList();

            DataRow row;
            foreach (var item in query_RubrosCN)
            {
                row = dtRubrosCN.NewRow();

                row["id_encomiendarubro"] = item.id_encomiendarubro;
                row["id_encomienda"] = item.id_encomienda;
                row["cod_rubro"] = item.CodigoRubro;
                row["desc_rubro"] = item.desc_rubro;
                row["EsAnterior"] = item.EsAnterior;
                row["TipoActividad"] = item.TipoActividad;
                row["DocRequerida"] = item.DocRequerida;
                row["SuperficieHabilitar"] = item.SuperficieHabilitar;

                dtRubrosCN.Rows.Add(row);
            }

            //rubros anteriores

            foreach (var item in query_RubrosAnt)
            {
                row = dtRubrosCN.NewRow();

                row["id_encomiendarubro"] = item.id_encomiendaRubro;
                row["id_encomienda"] = item.id_encomienda;
                row["cod_rubro"] = item.cod_rubro;
                row["desc_rubro"] = item.desc_rubro;
                row["EsAnterior"] = item.EsAnterior;
                row["TipoActividad"] = item.TipoActividad;
                row["DocRequerida"] = item.DocRequerida;
                row["SuperficieHabilitar"] = item.SuperficieHabilitar;

                dtRubrosCN.Rows.Add(row);
            }


        }

        public static void LoadSubRubros(ref DGHP_Entities db, ref dsImpresionPlanchetaTransf ds, int id_encomienda)
        {
            var query_SubRubros = (
                                     from enc in db.Encomienda
                                     join rub in db.Encomienda_RubrosCN on enc.id_encomienda equals rub.id_encomienda
                                     join srub in db.Encomienda_RubrosCN_Subrubros on rub.id_encomiendarubro equals srub.Id_EncRubro
                                     join srubcn in db.RubrosCN_Subrubros on srub.Id_rubrosubrubro equals srubcn.Id_rubroCNsubrubro
                                     where enc.id_encomienda == id_encomienda
                                     select new
                                     {
                                         rub.id_encomiendarubro,
                                         rub.CodigoRubro,
                                         srubcn.Id_rubroCNsubrubro,
                                         srubcn.Nombre,
                                         srub.Id_EncRubCNSubrubros
                                     }
                             ).ToList();

            DataTable dtSubRubros = ds.Tables["SubRubros"];
            DataRow row;
            foreach (var item in query_SubRubros)
            {
                row = dtSubRubros.NewRow();

                row["id_encomiendarubro"] = item.id_encomiendarubro;
                row["CodigoRubro"] = item.CodigoRubro;
                row["id_rubroCNsubrubro"] = item.Id_rubroCNsubrubro;
                row["Nombre"] = item.Nombre;
                row["id_EncRubCNSubrubros"] = item.Id_EncRubCNSubrubros;

                dtSubRubros.Rows.Add(row);
            }
        }

        private static string Transf_Buscar_ObservacionPlancheta(int id_solicitud, int id_tramitetarea)
        {
            DGHP_Entities db = new DGHP_Entities();
            string observ = "";

            var q = (
                from tt in db.SGI_Tramites_Tareas
                join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                join dghp in db.SGI_Tarea_Revision_DGHP on tt.id_tramitetarea equals dghp.id_tramitetarea
                where tt_tr.id_solicitud == id_solicitud && tt.id_tramitetarea <= id_tramitetarea
                && !string.IsNullOrEmpty(dghp.observacion_plancheta)
                select new
                {
                    tt.id_tramitetarea,
                    dghp.observacion_plancheta
                }
                ).Union(
                from tt in db.SGI_Tramites_Tareas
                join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                join gerente in db.SGI_Tarea_Revision_Gerente on tt.id_tramitetarea equals gerente.id_tramitetarea
                where tt_tr.id_solicitud == id_solicitud && tt.id_tramitetarea <= id_tramitetarea
                && !string.IsNullOrEmpty(gerente.observacion_plancheta)
                select new
                {
                    tt.id_tramitetarea,
                    gerente.observacion_plancheta
                }
                ).Union(
                from tt in db.SGI_Tramites_Tareas
                join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                join subgerente in db.SGI_Tarea_Revision_SubGerente on tt.id_tramitetarea equals subgerente.id_tramitetarea
                where tt_tr.id_solicitud == id_solicitud && tt.id_tramitetarea <= id_tramitetarea
                && !string.IsNullOrEmpty(subgerente.observacion_plancheta)
                select new
                {
                    tt.id_tramitetarea,
                    subgerente.observacion_plancheta
                }
                ).Union(
                from tt in db.SGI_Tramites_Tareas
                join tt_tr in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_tr.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                join cal in db.SGI_Tarea_Calificar on tt.id_tramitetarea equals cal.id_tramitetarea
                where tt_tr.id_solicitud == id_solicitud && tt.id_tramitetarea <= id_tramitetarea
                && !string.IsNullOrEmpty(cal.Observaciones)
                select new
                {
                    tt.id_tramitetarea,
                    observacion_plancheta = cal.Observaciones
                }
                );
            var lista = q.ToList().OrderByDescending(x => x.id_tramitetarea).ToList();
            if (lista != null && lista.Count > 0)
            {
                observ = lista[0].observacion_plancheta;
            }
            db.Dispose();
            return observ;
        }

        public static dsImpresionPlanchetaTransf Transf_GenerarDataSet(int id_solicitud, string nro_expediente, bool impresionDePrueba)
        {
            DGHP_Entities db = new DGHP_Entities();
            dsImpresionPlanchetaTransf ds = new dsImpresionPlanchetaTransf();
            //ds.EnforceConstraints = false;
            DataRow row;
            string obs_plancheta_gerente = "";
            var q = (
                   from tt in db.SGI_Tramites_Tareas
                   join tt_cp in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                   where tt_cp.id_solicitud == id_solicitud
                   orderby tt.id_tramitetarea descending
                   select new
                   {
                       tt.id_tramitetarea
                   }
                ).FirstOrDefault();
            if (q != null)
                obs_plancheta_gerente = Transf_Buscar_ObservacionPlancheta(id_solicitud, q.id_tramitetarea);

            //-- -------------------------
            //-- armar plantas a habilitar
            //-- -------------------------
            var plantasHabilitar =
                (
                    from tipsec in db.TipoSector
                    join cpplan in db.CPadron_Plantas on tipsec.Id equals cpplan.id_tiposector
                    join sol in db.Transf_Solicitudes on cpplan.id_cpadron equals sol.id_cpadron
                    where sol.id_solicitud == id_solicitud
                    orderby tipsec.Id
                    select new
                    {
                        id_tiposector = tipsec.Id,
                        Seleccionado = (cpplan.id_tiposector > 0) ? true : false,
                        tipsec.Descripcion,
                        MuestraCampoAdicional = tipsec.MuestraCampoAdicional,
                        detalle = cpplan.detalle_cpadrontiposector,
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
                    strPlantasHabilitar = strPlantasHabilitar + separador + item.Descripcion;
            }

            #region cargar datos de plancheta
            var q_calif = (
                   from tt in db.SGI_Tramites_Tareas
                   join tt_cp in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                   join up in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals up.userid
                   where tt_cp.id_solicitud == id_solicitud && tt.id_tarea == (int)SGI.Constants.ENG_Tareas.TR_Calificar
                   orderby tt.id_tramitetarea descending
                   select new
                   {
                       up.Nombres,
                       up.Apellido
                   }
                ).FirstOrDefault();


            //-- -------------------------
            //-- Datos encomienda, tabla 0
            //-- -------------------------

            DataTable dtDatosPlancheta = ds.Tables["datos_plancheta"];
            row = dtDatosPlancheta.NewRow();
            row["id_solicitud"] = id_solicitud;
            row["nro_expediente"] = nro_expediente;

            //0142017: JADHE 53586 - SGI - REQ - TRM - Previsualizar plancheta
            int nroTrReferencia = 0;
            int.TryParse(SGI.Parametros.GetParam_ValorChar("NroTransmisionReferencia"), out nroTrReferencia);
            if (id_solicitud <= nroTrReferencia)
            {
                row["calificador"] = q_calif.Apellido + ", " + q_calif.Nombres;
            }
            else
            {
                row["calificador"] = GetCalificador(ref db, id_solicitud);
            }

            row["nro_cert_ap_amb"] = "";
            dtDatosPlancheta.Rows.Add(row);

            #endregion

            //-- -------------------------
            //-- Datos transferecnias, tabla 0
            //-- -------------------------
            var query_enc =
            (
                from sol in db.Transf_Solicitudes
                join cp in db.CPadron_Solicitudes on sol.id_cpadron equals cp.id_cpadron
                join tt in db.TipoTramite on sol.id_tipotramite equals tt.id_tipotramite
                join te in db.TipoExpediente on sol.id_tipoexpediente equals te.id_tipoexpediente
                join ste in db.SubtipoExpediente on sol.id_subtipoexpediente equals ste.id_subtipoexpediente
                join cpnorm in db.CPadron_Normativas on sol.id_cpadron equals cpnorm.id_cpadron into left_1
                from cpnorm in left_1.DefaultIfEmpty()
                join tipnorm in db.TipoNormativa on cpnorm.id_tiponormativa equals tipnorm.Id into left_2
                from tipnorm in left_2.DefaultIfEmpty()
                join entnorm in db.EntidadNormativa on cpnorm.id_entidadnormativa equals entnorm.Id into left_3
                from entnorm in left_3.DefaultIfEmpty()
                where sol.id_solicitud == id_solicitud
                select new
                {
                    sol.id_solicitud,
                    sol.CreateDate,
                    nroEncomiendaconsejo = 0,
                    ZonaDeclarada = cp.ZonaDeclarada,
                    TipoDeTramite = tt.cod_tipotramite,
                    TipoDeExpediente = te.cod_tipoexpediente,
                    SubTipoDeExpediente = ste.cod_subtipoexpediente,
                    MatriculaProfesional = "",
                    ApellidoProfesional = "",
                    NombresProfesional = "",
                    TipoDocProfesional = "",
                    DocumentoProfesional = 0,
                    id_grupoconsejo = 0,
                    ConsejoProfesional = "",
                    TipoNormativa = tipnorm.Descripcion,
                    EntidadNormativa = entnorm.Descripcion,
                    NroNormativa = cpnorm.nro_normativa,
                    LogoUrl = "",
                    ImpresionDePrueba = (cp.id_estado <= 1) ? true : false,
                    PlantasHabilitar = strPlantasHabilitar,
                    ObservacionesPlantasHabilitar = "",
                    ObservacionesRubros = "",
                    id_encomienda_anterior = 0,
                    NroExpediente = ""
                }

            ).ToList();

            Control ctl = new Control();

            //Genera los Qr.
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            byte[] str1Byte = Encoding.Default.GetBytes(Convert.ToString(id_solicitud));
            string base64 = Convert.ToBase64String(str1Byte);
            str1Byte = Encoding.Default.GetBytes(Convert.ToString((int)Constants.GruposDeTramite.TR));
            string tipoBase64 = Convert.ToBase64String(str1Byte);

            string urlws = SGI.Parametros.GetParam_ValorChar("WebService.Url");

            string urlOblea = string.Format("{0}/{1}/{2}/{3}", urlws, "TramiteQrSGI", base64, tipoBase64);

            //urlOblea = IPtoDomain(urlOblea);
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 7;      // 316 x 316 pixels 
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;

            byte[] imagen_bytes = null;

            if (urlOblea != null)
            {
                System.Drawing.Image imagenQr = qrCodeEncoder.Encode(urlOblea);
                imagen_bytes = imageToByteArray(imagenQr);

            }
            //Fin generacion qr

            DataTable dtEncomienda = ds.Tables["Encomienda"];
            foreach (var item in query_enc)
            {
                row = dtEncomienda.NewRow();

                row["id_encomienda"] = item.id_solicitud;
                row["FechaEncomienda"] = item.CreateDate;
                row["nroEncomiendaconsejo"] = item.nroEncomiendaconsejo;
                row["ZonaDeclarada"] = Functions.NVL(item.ZonaDeclarada);
                row["TipoDeTramite"] = item.TipoDeTramite;
                row["TipoDeExpediente"] = item.TipoDeExpediente;
                row["SubTipoDeExpediente"] = item.SubTipoDeExpediente;
                row["MatriculaProfesional"] = Functions.NVL(item.MatriculaProfesional);
                row["ApellidoProfesional"] = item.ApellidoProfesional;
                row["NombresProfesional"] = item.NombresProfesional;
                row["TipoDocProfesional"] = item.TipoDocProfesional;
                row["DocumentoProfesional"] = Functions.NVL(item.DocumentoProfesional);
                row["id_grupoconsejo"] = item.id_grupoconsejo;
                row["ConsejoProfesional"] = item.ConsejoProfesional;
                row["TipoNormativa"] = Functions.NVL(item.TipoNormativa);
                row["EntidadNormativa"] = Functions.NVL(item.EntidadNormativa);
                row["NroNormativa"] = Functions.NVL(item.NroNormativa);
                row["LogoUrl"] = item.LogoUrl;
                row["ImpresionDePrueba"] = impresionDePrueba;
                row["Logo"] = imagen_bytes;
                row["PlantasHabilitar"] = Functions.NVL(item.PlantasHabilitar);
                row["ObservacionesPlantasHabilitar"] = Functions.NVL(item.ObservacionesPlantasHabilitar);
                row["ObservacionesRubros"] = obs_plancheta_gerente;
                row["id_encomienda_anterior"] = Functions.NVL(item.id_encomienda_anterior);
                row["NroExpediente"] = Functions.NVL(item.NroExpediente);

                dtEncomienda.Rows.Add(row);
            }

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

                string sql = "select dbo.Transf_Solicitud_DireccionesPartidas(" + item.id_solicitud + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                row["id_encomiendaubicacion"] = item.id_cpadronubicacion;
                row["id_encomienda"] = item.id_solicitud;
                row["id_ubicacion"] = Functions.NVL(item.id_ubicacion);
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
                    from sol in db.Transf_Solicitudes
                    join cpubic in db.CPadron_Ubicaciones on sol.id_cpadron equals cpubic.id_cpadron
                    join cpphor in db.CPadron_Ubicaciones_PropiedadHorizontal on cpubic.id_cpadronubicacion equals cpphor.id_cpadronubicacion
                    join phor in db.Ubicaciones_PropiedadHorizontal on cpphor.id_propiedadhorizontal equals phor.id_propiedadhorizontal
                    where sol.id_solicitud == id_solicitud
                    orderby cpubic.id_cpadronubicacion
                    select new
                    {
                        id_encomiendaubicacion = cpubic.id_cpadronubicacion,
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
                row["id_encomiendaubicacion"] = item.id_encomiendaubicacion;
                row["id_ubicacion"] = Functions.NVL(item.id_ubicacion);

                if (item.NroPartidaHorizontal != null)
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
                        id_encomiendaubicacion = cpubic.id_cpadronubicacion,
                        id_encomiendapuerta = cppuer.id_cpadronpuerta,
                        id_encomienda = cpubic.id_cpadron,
                        cpubic.id_ubicacion,
                        Calle = cppuer.nombre_calle,
                        cppuer.NroPuerta
                    }
                ).ToList();

            DataTable dtPuerta = ds.Tables["Puertas"];

            foreach (var item in query_puerta)
            {
                row = dtPuerta.NewRow();

                row["id_encomiendaubicacion"] = item.id_encomiendaubicacion;
                row["id_encomiendapuerta"] = item.id_encomiendapuerta;
                row["id_ubicacion"] = item.id_ubicacion;
                row["Calle"] = item.Calle;
                row["NroPuerta"] = item.NroPuerta;

                dtPuerta.Rows.Add(row);
            }

            // ConformacionLocal
            var query_ConformacionLocal =
                (
                    from sol in db.Transf_Solicitudes
                    join conf in db.CPadron_ConformacionLocal on sol.id_cpadron equals conf.id_cpadron
                    join dest in db.TipoDestino on conf.id_destino equals dest.Id
                    where sol.id_solicitud == id_solicitud
                    select new
                    {
                        id_encomiendaconflocal = conf.id_cpadronconflocal,
                        id_encomienda = sol.id_solicitud,
                        Destino = dest.Nombre,
                        conf.largo_conflocal,
                        conf.ancho_conflocal,
                        conf.alto_conflocal,
                        conf.Paredes_conflocal,
                        conf.Techos_conflocal,
                        conf.Pisos_conflocal,
                        conf.Frisos_conflocal,
                        conf.Observaciones_conflocal,
                        conf.Detalle_conflocal
                    }
                ).ToList();

            DataTable dtConformacionLocal = ds.Tables["ConformacionLocal"];

            foreach (var item in query_ConformacionLocal)
            {
                row = dtConformacionLocal.NewRow();

                row["id_encomiendaconflocal"] = item.id_encomiendaconflocal;
                row["id_encomienda"] = item.id_encomienda;
                row["Destino"] = item.Destino;
                row["largo_conflocal"] = item.largo_conflocal;
                row["ancho_conflocal"] = item.ancho_conflocal;
                row["alto_conflocal"] = item.alto_conflocal;
                row["Paredes_conflocal"] = item.Paredes_conflocal;
                row["Techos_conflocal"] = item.Techos_conflocal;
                row["Pisos_conflocal"] = item.Pisos_conflocal;
                row["Frisos_conflocal"] = item.Frisos_conflocal;
                row["Observaciones_conflocal"] = item.Observaciones_conflocal;
                row["Detalle_conflocal"] = item.Detalle_conflocal;

                dtConformacionLocal.Rows.Add(row);
            }

            // Firmantes
            var query_Firmantes1 =
                (
                    from pj in db.Transf_Firmantes_PersonasJuridicas
                    join titpj in db.Transf_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                    join tcl in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tcl.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pj.id_solicitud == id_solicitud && titpj.Id_TipoSociedad != 2 // Todas las sociedades menos la Sociedad de Hecho
                    && titpj.Id_TipoSociedad != 32 //Sociedad no constituidas regularmente (Sec. 4ª Ley 19550)
                    select new
                    {
                        id_firmante = pj.id_firmante_pj,
                        id_encomienda = pj.id_solicitud,
                        TipoPersona = "PJ",
                        Apellido = pj.Apellido,
                        Nombres = pj.Nombres,
                        TipoDoc = tdoc.Nombre,
                        NroDoc = pj.Nro_Documento,
                        CaracterLegal = tcl.nom_tipocaracter,
                        pj.cargo_firmante_pj,
                        FirmanteDe = titpj.Razon_Social
                    }
                ).Distinct().ToList();


            DataTable dtFirmantes = ds.Tables["Firmantes"];

            foreach (var item in query_Firmantes1)
            {
                row = dtFirmantes.NewRow();

                row["id_firmante"] = item.id_firmante;
                row["id_encomienda"] = item.id_encomienda;
                row["TipoPersona"] = item.TipoPersona;
                row["Apellido"] = item.Apellido.ToUpper();
                row["Nombres"] = item.Nombres.ToUpper();
                row["TipoDoc"] = item.TipoDoc;
                row["NroDoc"] = item.NroDoc;
                row["CaracterLegal"] = item.CaracterLegal;
                row["cargo_firmante_pj"] = item.cargo_firmante_pj;
                row["FirmanteDe"] = item.FirmanteDe.ToUpper();

                dtFirmantes.Rows.Add(row);
            }

            var query_Firmantes2 =
                (
                    from pj in db.Transf_Firmantes_PersonasJuridicas
                    join titpj in db.Transf_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                    join titsh in db.Transf_Titulares_PersonasJuridicas_PersonasFisicas on pj.id_firmante_pj equals titsh.id_firmante_pj
                    join tcl in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tcl.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pj.id_solicitud == id_solicitud && (titpj.Id_TipoSociedad == 2 // Solo Sociedades de Hecho
                    || titpj.Id_TipoSociedad == 32) //Sociedad no constituidas regularmente (Sec. 4ª Ley 19550)
                    select new
                    {
                        id_firmante = pj.id_firmante_pj,
                        pj.id_solicitud,
                        TipoPersona = "PJ",
                        Apellido = pj.Apellido,
                        Nombres = pj.Nombres,
                        TipoDoc = tdoc.Nombre,
                        NroDoc = pj.Nro_Documento,
                        CaracterLegal = tcl.nom_tipocaracter,
                        pj.cargo_firmante_pj,
                        FirmanteDe = titsh.Apellido + ", " + titsh.Nombres
                    }
                ).Distinct().ToList();


            foreach (var item in query_Firmantes2)
            {
                row = dtFirmantes.NewRow();

                row["id_firmante"] = item.id_firmante;
                row["id_encomienda"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                //row["id_persona"] = null;
                row["Apellido"] = item.Apellido.ToUpper();
                row["Nombres"] = item.Nombres.ToUpper();
                row["TipoDoc"] = item.TipoDoc;
                row["NroDoc"] = item.NroDoc;
                row["CaracterLegal"] = item.CaracterLegal;
                row["cargo_firmante_pj"] = item.cargo_firmante_pj;
                row["FirmanteDe"] = item.FirmanteDe.ToUpper();

                dtFirmantes.Rows.Add(row);
            }

            var query_Firmantes3 =
                (
                    from pf in db.Transf_Firmantes_PersonasFisicas
                    join titpf in db.Transf_Titulares_PersonasFisicas on pf.id_personafisica equals titpf.id_personafisica
                    join tcl in db.TiposDeCaracterLegal on pf.id_tipocaracter equals tcl.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pf.id_solicitud == id_solicitud
                    select new
                    {
                        id_firmante = pf.id_firmante_pf,
                        pf.id_solicitud,
                        TipoPersona = "PF",
                        Apellido = pf.Apellido,
                        Nombres = pf.Nombres,
                        TipoDoc = tdoc.Nombre,
                        NroDoc = pf.Nro_Documento,
                        CaracterLegal = tcl.nom_tipocaracter,
                        cargo_firmante_pj = "",
                        FirmanteDe = titpf.Apellido + ", " + titpf.Nombres
                    }
                ).ToList();

            foreach (var item in query_Firmantes3)
            {
                row = dtFirmantes.NewRow();

                row["id_firmante"] = item.id_firmante;
                row["id_encomienda"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["id_persona"] = DBNull.Value;
                row["Apellido"] = item.Apellido.ToUpper();
                row["Nombres"] = item.Nombres.ToUpper();
                row["TipoDoc"] = item.TipoDoc;
                row["NroDoc"] = item.NroDoc;
                row["CaracterLegal"] = item.CaracterLegal;
                row["cargo_firmante_pj"] = item.cargo_firmante_pj;
                row["FirmanteDe"] = item.FirmanteDe.ToUpper();

                dtFirmantes.Rows.Add(row);
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
                        TipoSociedad = tsoc.Descripcion,
                        TipoIIBB = tipoiibb.nom_tipoiibb,
                        NroIIBB = pj.Nro_IIBB,
                        pj.CUIT,
                        MuestraEnTitulares = 1,
                        MuestraEnPlancheta = (pj.Id_TipoSociedad == 2 || pj.Id_TipoSociedad == 32) ? false : true
                    }
                ).ToList();

            DataTable dtTitulares = ds.Tables["Titulares"];

            foreach (var item in query_Titulares1)
            {
                row = dtTitulares.NewRow();

                row["id_persona"] = item.id_persona;
                row["id_encomienda"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["TipoSociedad"] = item.TipoSociedad;
                row["RazonSocial"] = item.RazonSocial.ToUpper();
                row["Apellido"] = "";
                row["Nombres"] = "";
                row["TipoDoc"] = "";
                row["NroDoc"] = 0;
                row["TipoIIBB"] = item.TipoIIBB;
                row["NroIIBB"] = item.NroIIBB;
                row["Cuit"] = item.CUIT;
                row["MuestraEnTitulares"] = item.MuestraEnTitulares;
                row["MuestraEnPlancheta"] = item.MuestraEnPlancheta;

                dtTitulares.Rows.Add(row);
            }

            //0146579: JADHE 57464 - SGI - Dispo con error

            //var query_Titulares2 =
            //    (
            //        from pj in db.Transf_Titulares_PersonasJuridicas_PersonasFisicas
            //        join titpj in db.Transf_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
            //        join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
            //        where titpj.id_solicitud == id_solicitud && (titpj.Id_TipoSociedad == 2 || titpj.Id_TipoSociedad == 32)
            //        select new
            //        {
            //            id_persona = pj.id_firmante_pj,
            //            titpj.id_solicitud,
            //            TipoPersona = "PF",
            //            Apellido = pj.Apellido,
            //            Nombres = pj.Nombres,
            //            TipoDoc = tdoc.Nombre,
            //            NroDoc = pj.Nro_Documento,
            //            MuestraEnTitulares = 0,
            //            MuestraEnPlancheta = 1
            //        }
            //    ).Distinct().ToList();

            //foreach (var item in query_Titulares2)
            //{
            //    row = dtTitulares.NewRow();

            //    row["id_persona"] = item.id_persona;
            //    row["id_encomienda"] = item.id_solicitud;
            //    row["TipoPersona"] = item.TipoPersona;
            //    row["TipoSociedad"] = "";
            //    row["RazonSocial"] = "";
            //    row["Apellido"] = item.Apellido;
            //    row["Nombres"] = item.Nombres;
            //    row["TipoDoc"] = item.TipoDoc;
            //    row["NroDoc"] = item.NroDoc;
            //    row["TipoIIBB"] = "";
            //    row["NroIIBB"] = "";
            //    row["Cuit"] = "";
            //    row["MuestraEnTitulares"] = item.MuestraEnTitulares;
            //    row["MuestraEnPlancheta"] = item.MuestraEnPlancheta;

            //    dtTitulares.Rows.Add(row);
            //}

            var query_Titulares3 =
                (
                    from pf in db.Transf_Titulares_PersonasFisicas
                    join tdoc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    join tipoiibb in db.TiposDeIngresosBrutos on pf.id_tipoiibb equals tipoiibb.id_tipoiibb
                    where pf.id_solicitud == id_solicitud
                    select new
                    {
                        id_persona = pf.id_personafisica,
                        pf.id_solicitud,
                        TipoPersona = "PF",
                        Apellido = pf.Apellido,
                        Nombres = pf.Nombres,
                        TipoDoc = tdoc.Nombre,
                        NroDoc = pf.Nro_Documento,
                        TipoIIBB = tipoiibb.nom_tipoiibb,
                        NroIIBB = pf.Ingresos_Brutos,
                        pf.Cuit,
                        MuestraEnTitulares = 1,
                        MuestraEnPlancheta = 1
                    }
                ).ToList();

            foreach (var item in query_Titulares3)
            {
                row = dtTitulares.NewRow();

                row["id_persona"] = item.id_persona;
                row["id_encomienda"] = item.id_solicitud;
                row["TipoPersona"] = item.TipoPersona;
                row["TipoSociedad"] = "";
                row["RazonSocial"] = "";
                row["Apellido"] = item.Apellido;
                row["Nombres"] = item.Nombres;
                row["TipoDoc"] = item.TipoDoc;
                row["NroDoc"] = item.NroDoc;
                row["TipoIIBB"] = item.TipoIIBB;
                row["NroIIBB"] = item.NroIIBB;
                row["Cuit"] = item.Cuit;
                row["MuestraEnTitulares"] = item.MuestraEnTitulares;
                row["MuestraEnPlancheta"] = item.MuestraEnPlancheta;

                dtTitulares.Rows.Add(row);
            }

            //0142017: JADHE 53586 - SGI - REQ - TRM - Previsualizar plancheta
            if (id_solicitud <= nroTrReferencia)
            {
                LoadRubros(ref db, ref ds, id_solicitud);
            }
            else
            {
                int id_encomienda = GetUltimaEncomiendaAprobada(ref db, id_solicitud);
                LoadRubrosCN(ref db, ref ds, id_encomienda);
                LoadSubRubros(ref db, ref ds, id_encomienda);
            }

            // DatosLocal
            var query_DatosLocal =
                (
                    from sol in db.Transf_Solicitudes
                    join dl in db.CPadron_DatosLocal on sol.id_cpadron equals dl.id_cpadron
                    where sol.id_solicitud == id_solicitud
                    select new
                    {
                        dl.id_cpadrondatoslocal,
                        sol.id_solicitud,
                        dl.superficie_cubierta_dl,
                        dl.superficie_descubierta_dl,
                        dl.dimesion_frente_dl,
                        dl.lugar_carga_descarga_dl,
                        dl.estacionamiento_dl,
                        dl.red_transito_pesado_dl,
                        dl.sobre_avenida_dl,
                        dl.materiales_pisos_dl,
                        dl.materiales_paredes_dl,
                        dl.materiales_techos_dl,
                        dl.materiales_revestimientos_dl,
                        dl.sanitarios_ubicacion_dl,
                        dl.sanitarios_distancia_dl,
                        dl.cantidad_sanitarios_dl,
                        dl.superficie_sanitarios_dl,
                        dl.frente_dl,
                        dl.fondo_dl,
                        dl.lateral_izquierdo_dl,
                        dl.lateral_derecho_dl,
                        dl.cantidad_operarios_dl
                    }
                ).ToList();

            DataTable dtDatosLocal = ds.Tables["DatosLocal"];

            foreach (var item in query_DatosLocal)
            {
                row = dtDatosLocal.NewRow();

                row["id_encomiendadatoslocal"] = item.id_cpadrondatoslocal;
                row["id_encomienda"] = item.id_solicitud;
                row["superficie_cubierta_dl"] = item.superficie_cubierta_dl;
                row["superficie_descubierta_dl"] = item.superficie_descubierta_dl;
                row["dimesion_frente_dl"] = item.dimesion_frente_dl;
                row["lugar_carga_descarga_dl"] = item.lugar_carga_descarga_dl;
                row["estacionamiento_dl"] = item.estacionamiento_dl;
                row["red_transito_pesado_dl"] = item.red_transito_pesado_dl;
                row["sobre_avenida_dl"] = item.sobre_avenida_dl;
                row["materiales_pisos_dl"] = item.materiales_pisos_dl;
                row["materiales_paredes_dl"] = item.materiales_paredes_dl;
                row["materiales_techos_dl"] = item.materiales_techos_dl;
                row["materiales_revestimientos_dl"] = item.materiales_revestimientos_dl;
                row["sanitarios_ubicacion_dl"] = item.sanitarios_ubicacion_dl;
                row["sanitarios_distancia_dl"] = (item.sanitarios_distancia_dl != null) ? item.sanitarios_distancia_dl : 0;
                row["cantidad_sanitarios_dl"] = item.cantidad_sanitarios_dl;
                row["superficie_sanitarios_dl"] = (item.superficie_sanitarios_dl != null) ? item.superficie_sanitarios_dl : 0;
                row["frente_dl"] = (item.frente_dl != null) ? item.frente_dl : 0;
                row["fondo_dl"] = (item.fondo_dl != null) ? item.fondo_dl : 0;
                row["lateral_izquierdo_dl"] = (item.lateral_izquierdo_dl != null) ? item.lateral_izquierdo_dl : 0;
                row["lateral_derecho_dl"] = (item.lateral_derecho_dl != null) ? item.lateral_derecho_dl : 0;
                row["sobrecarga_corresponde_dl"] = false;
                row["sobrecarga_corresponde_dl"] = DBNull.Value;
                row["sobrecarga_tipo_observacion"] = DBNull.Value;
                row["sobrecarga_requisitos_opcion"] = DBNull.Value;
                row["sobrecarga_art813_inciso"] = DBNull.Value;
                row["sobrecarga_art813_item"] = DBNull.Value;
                if (item.cantidad_operarios_dl.HasValue)
                    row["cantidad_operarios_dl"] = item.cantidad_operarios_dl;
                else
                    row["cantidad_operarios_dl"] = DBNull.Value;
                dtDatosLocal.Rows.Add(row);
            }
            db.Dispose();
            return ds;
        }

        public static byte[] Transf_GenerarPdf(int id_solicitud, string nro_expediente, bool impresionDePrueba)
        {

            byte[] documento = null;

            DGHP_Entities db = new DGHP_Entities();

            Stream msPdfDisposicion = null;

            dsImpresionPlanchetaTransf dsPlancheta = null;
            dsPlancheta = Transf_GenerarDataSet(id_solicitud, nro_expediente, impresionDePrueba);

            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
            try
            {
                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/Transferencias/PlanchetaTransferencia.rpt";
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
            return documento;
        }

        #endregion

        #region plancheta cpadron
        public static byte[] CPadron_GenerarPdf(int id_cpadron, int id_tramitetarea, int id_tipoinforme, string codigoseguridad, string observacioninforme, bool impresionDePrueba, int esTR)
        {
            byte[] documento = null;

            try
            {
                ws_Reporting reportingService = new ws_Reporting();
                ExternalService.Class.ReportingEntity ReportingEntity = new ExternalService.Class.ReportingEntity();
                if (esTR == 1)
                {
                    DGHP_Entities db = new DGHP_Entities();
                    int idCpadron = db.Transf_Solicitudes.Where(x => x.id_solicitud == id_cpadron).Select(y => y.id_cpadron).FirstOrDefault();
                    ReportingEntity = reportingService.GetPDFTransmisionesInformeCPadron(idCpadron, id_tipoinforme, impresionDePrueba);
                }
                else
                    ReportingEntity = reportingService.GetPDFInformeCPadron(id_cpadron, id_tipoinforme, impresionDePrueba);
                documento = ReportingEntity.Reporte;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return documento;

        }

        private static string CPadron_Buscar_ObservacionPlancheta(int id_cpadron, int id_tramitetarea, int id_tipoinforme, string observacioninforme)//, int id_tramitetarea)
        {
            DGHP_Entities db = new DGHP_Entities();
            string observ = "";


            var q = (
                from tt in db.SGI_Tarea_Carga_Tramite.Where(x => x.id_tramitetarea == id_tramitetarea)
                join tt_cp in db.SGI_Tramites_Tareas_CPADRON on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                where tt_cp.id_cpadron == id_cpadron //&& tt.id_tramitetarea <= id_tramitetarea
                //&& !string.IsNullOrEmpty(tt.Observaciones_informe)
                select new
                {
                    id_tramitetarea = tt.id_tramitetarea,
                    Observacion_standard = tt.id_tipo_informe != null ? (tt.id_tipo_informe == 1 ? "Se deja constancia que se extiende el presente a fin de posibilitar la solicitud de transferencia de la habilitación tratada, siendo el mismo a mero título informativo, sin que ello implique el otorgamiento de derecho alguno y de ninguna naturaleza."
                                        : "Se deja constancia que se extiende el presente a mero título informativo, sin que ello implique el otorgamiento de derecho alguno y de ninguna naturaleza.")
                                        : "",
                    Observacion_informe = tt.Observaciones_informe,
                }).FirstOrDefault();

            if (q != null)
                observ = "\n" + "- " + q.Observacion_informe + "\n\n" + "- " + q.Observacion_standard;
            if (q == null)
            {
                if (id_tipoinforme == 1)
                {
                    observ = "\n" + "- " + observacioninforme + "\n\n" + "- " + "Se deja constancia que se extiende el presente a fin de posibilitar la solicitud de transferencia de la habilitación tratada, siendo el mismo a mero título informativo, sin que ello implique el otorgamiento de derecho alguno y de ninguna naturaleza.";
                }
                if (id_tipoinforme == 2)
                {
                    observ = "\n" + "- " + observacioninforme + "\n\n" + "- " + "Se deja constancia que se extiende el presente a mero título informativo, sin que ello implique el otorgamiento de derecho alguno y de ninguna naturaleza.";
                }
            }

            db.Dispose();
            return observ;
        }

        #endregion

        private static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        private static async Task<List<GetCAAsByEncomiendasResponse>> GetCAAsByEncomiendas(int[] lst_id_Encomiendas)
        {
            ExternalService.ApraSrvRest apraSrvRest = new ExternalService.ApraSrvRest();
            List<GetCAAsByEncomiendasResponse> lstCaa = await apraSrvRest.GetCAAsByEncomiendas(lst_id_Encomiendas.ToList());
            return lstCaa;
        }
    }
}