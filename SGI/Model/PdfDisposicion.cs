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
    public class calificador
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
    }
    public class PdfDisposicion
    {


        public PdfDisposicion()
        {
            IniciarEntity();
        }

        public void Dispose()
        {
            FinalizarEntity();
            FinalizarEntityFiles();
        }


        #region entity

        private DGHP_Entities db = null;
        private AGC_FilesEntities dbFiles = null;

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }
        }

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
            {
                this.dbFiles.Dispose();
                this.dbFiles = null;
            }
        }

        #endregion


        public App_Data.dsImpresionDisposicion GenerarDataSetDisposicion(int id_solicitud, int id_tramitetarea, string expediente, bool validarProceso)
        {
            App_Data.dsImpresionDisposicion ds = new App_Data.dsImpresionDisposicion();
            DataRow row;

            string obs_plancheta_gerente = Buscar_ObservacionPlancheta(id_solicitud, id_tramitetarea);

            SGI_Tramites_Tareas tramitetarea = db.SGI_Tramites_Tareas.Where(x => x.id_tramitetarea == id_tramitetarea).FirstOrDefault();

            if (validarProceso)
            {
                var q =
                    (
                    // from expe in db.SGI_Tarea_Generar_Expediente
                        from proceso in db.SGI_Tarea_Generar_Expediente_Procesos
                        where proceso.id_tramitetarea == id_tramitetarea && proceso.id_proceso == 2
                        select new
                        {
                            proceso.id_proceso,
                            proceso.realizado,
                            proceso.resultado_ee,
                            proceso.id_paquete
                        }
                    ).FirstOrDefault();

                if (!q.realizado)  // aun no se genero la caratula y por ende el expediente
                    return null;
            }

            SSIT_Solicitudes sol=db.SSIT_Solicitudes.FirstOrDefault(x=>x.id_solicitud==id_solicitud);
            var encomienda = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
            int id_encomienda = encomienda.id_encomienda;

            // buscar calificador de la tarea 
            //SGI_Tramites_Tareas 

            //-- -------------------------
            //-- Datos Disposicion
            //-- -------------------------

            // buscar tramite tarea de calificador
            var query_tt_calif =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                        join prof in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals prof.userid
                        join th in db.SGI_Tarea_Calificar on tt.id_tramitetarea equals th.id_tramitetarea
                        where tt_hab.id_solicitud == id_solicitud
                        orderby tt.id_tramitetarea descending
                        select new
                        {
                            nombre = prof.Nombres,
                            apellido = prof.Apellido
                        }

                    ).FirstOrDefault();

            DataTable dtDisposicion = ds.Tables["Disposicion"];
            if (expediente == null || expediente == "")
            {
                string nro_expediente_sade = sol.NroExpedienteSade;
                if (nro_expediente_sade != null)
                {
                    string[] datos = nro_expediente_sade.Split('-');
                    if (datos.Length > 2)
                        expediente = Convert.ToString(Convert.ToInt32(datos[2])) + "/" + datos[1];
                }
            }

            row = dtDisposicion.NewRow();

            row["nro_disposicion"] = 0;
            row["expediente"] = expediente;
            row["nro_solicitud"] = id_solicitud;
            row["calificador_nombre"] = query_tt_calif != null ? query_tt_calif.nombre : "";
            row["calificador_apellido"] = query_tt_calif != null ? query_tt_calif.apellido : "";
            row["observacion_disposicion"] = obs_plancheta_gerente;
            row["fecha_disposicion"] = tramitetarea.FechaInicio_tramitetarea;
            row["id_resultado"] = 0;
            dtDisposicion.Rows.Add(row);


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
                        id_tipotramite = enc.id_tipotramite,
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
                   
                ).Distinct().ToList();


            DataTable dtEncomienda = ds.Tables["Encomienda"];

            foreach (var item in query_enc)
            {
                row = dtEncomienda.NewRow();

                row["id_encomienda"] = item.id_encomienda;
                row["FechaEncomienda"] = item.FechaEncomienda;
                row["nroEncomiendaconsejo"] = item.nroEncomiendaconsejo;
                row["ZonaDeclarada"] = item.ZonaDeclarada;
                row["id_tipotramite"] = item.id_tipotramite;
                row["TipoDeTramite"] = item.TipoDeTramite;
                row["TipoDeExpediente"] = item.TipoDeExpediente;
                row["SubTipoDeExpediente"] = item.SubTipoDeExpediente;
                row["MatriculaProfesional"] = item.MatriculaProfesional;
                row["ApellidoProfesional"] = item.ApellidoProfesional;
                row["NombresProfesional"] = item.NombresProfesional;
                row["TipoDocProfesional"] = item.TipoDocProfesional;
                row["DocumentoProfesional"] = item.DocumentoProfesional;
                row["id_grupoconsejo"] = item.id_grupoconsejo;
                row["ConsejoProfesional"] = item.ConsejoProfesional;
                row["TipoNormativa"] = item.TipoNormativa;
                row["EntidadNormativa"] = item.EntidadNormativa;
                row["NroNormativa"] = item.NroNormativa;
                row["LogoUrl"] = item.LogoUrl;
                row["ImpresionDePrueba"] = item.ImpresionDePrueba;
                row["Logo"] = null;
                row["PlantasHabilitar"] = item.PlantasHabilitar;
                row["ObservacionesPlantasHabilitar"] = item.ObservacionesPlantasHabilitar;
                row["ObservacionesRubros"] = item.ObservacionesRubros;
                row["id_encomienda_anterior"] = item.id_encomienda_anterior;
                row["NroExpediente"] = item.NroExpediente;

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
                );

            DataTable dtUbicaciones = ds.Tables["Ubicaciones"];

            foreach (var item in query_ubi.ToList())
            {
                row = dtUbicaciones.NewRow();

                string sql = "select dbo.SSIT_Solicitud_DireccionesPartidasPlancheta(" + item.id_solicitud + "," + item.id_ubicacion + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                row["id_encomiendaubicacion"] = item.id_solicitudubicacion;
                row["id_encomienda"] = id_encomienda;
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
                ).ToList();


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


            // Depositos

            var query_Depositos =
                (from enc in db.Encomienda
                 join ercd in db.Encomienda_RubrosCN_Deposito on enc.id_encomienda equals ercd.id_encomienda
                 join rdc in db.RubrosDepositosCN on ercd.IdDeposito equals rdc.IdDeposito
                 where enc.id_encomienda == id_encomienda
                 select new
                 {
                     id_encomienda = enc.id_encomienda,
                     idRubro = ercd.IdRubro,
                     IdDeposito = rdc.IdDeposito,
                     Codigo = rdc.Codigo,
                     Descripcion = rdc.Descripcion
                 }).ToList();

            DataTable dtDepositos = ds.Tables["EncomiendaDepositos"];

            foreach (var d in query_Depositos)
            {
                row = dtDepositos.NewRow();

                row["id_encomienda"] = d.id_encomienda;
                row["idRubro"] = d.idRubro;
                row["IdDeposito"] = d.IdDeposito;
                row["Codigo"] = d.Codigo;
                row["Descripcion"] = d.Descripcion;

                dtDepositos.Rows.Add(row);
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

            DataTable dtMixturas = ds.Tables["Mixturas"];

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

            DataTable dtDistritos = ds.Tables["Distritos"];

            foreach (var item in query_Distritos)
            {
                row = dtDistritos.NewRow();
                row["Descripcion"] = item.descrip;

                dtDistritos.Rows.Add(row);
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
                        dl.cantidad_operarios_dl,
                        dl.ampliacion_superficie,
                        dl.superficie_cubierta_amp,
                        dl.superficie_descubierta_amp
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

                row["ampliacion_superficie"] = (item.ampliacion_superficie.HasValue ? item.ampliacion_superficie.Value : false);
                if (item.ampliacion_superficie.HasValue && item.ampliacion_superficie.Value)
                {
                    row["superficie_cubierta_amp"] = item.superficie_cubierta_amp;
                    row["superficie_descubierta_amp"] = item.superficie_descubierta_amp;
                }


                dtDatosLocal.Rows.Add(row);

            }

            return ds;


        }

        private static string Buscar_ObservacionPlancheta(int id_solicitud, int id_tramitetarea)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            string observ = "";

            var q = (
                from tt in db.SGI_Tramites_Tareas
                join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                join dghp in db.SGI_Tarea_Revision_DGHP on tt.id_tramitetarea equals dghp.id_tramitetarea
                where tt_hab.id_solicitud == id_solicitud && tt.id_tramitetarea <= id_tramitetarea
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
                select new
                {
                    tt.id_tramitetarea,
                    observacion_plancheta = cal.Observaciones
                }
                );
            var lista = q.ToList().OrderByDescending(x => x.id_tramitetarea).ToList();
            if (lista != null && lista.Count > 0)
            {
                observ = !string.IsNullOrEmpty(lista[0].observacion_plancheta) ? lista[0].observacion_plancheta : "";
            }
            db.Dispose();
            return observ;
        }

        public string Buscar_Considerando_Dispo_RR(int id_solicitud, int id_tramitetarea)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            string considerando = "";

            var q = (
                from tt in db.SGI_Tramites_Tareas
                join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                join dghp in db.SGI_Tarea_Revision_DGHP on tt.id_tramitetarea equals dghp.id_tramitetarea
                join tdc in db.SGI_Tramites_Tareas_Dispo_Considerando on tt.id_tramitetarea equals tdc.id_tramitetarea
                where tt_hab.id_solicitud == id_solicitud && tt.id_tramitetarea <= id_tramitetarea
                select new
                {
                    tt.id_tramitetarea,
                    tdc.considerando_dispo
                }
                ).Union(
                from tt in db.SGI_Tramites_Tareas
                join tt_hab in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tt_hab.id_tramitetarea
                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                join gerente in db.SGI_Tarea_Gestion_Documental on tt.id_tramitetarea equals gerente.id_tramitetarea
                join tdc in db.SGI_Tramites_Tareas_Dispo_Considerando on tt.id_tramitetarea equals tdc.id_tramitetarea
                where tt_hab.id_solicitud == id_solicitud && tt.id_tramitetarea <= id_tramitetarea
                select new
                {
                    tt.id_tramitetarea,
                    tdc.considerando_dispo
                }
                );
            var lista = q.ToList().OrderByDescending(x => x.id_tramitetarea).ToList();
            if (lista != null && lista.Count > 0)
            {
                considerando = !string.IsNullOrEmpty(lista[0].considerando_dispo) ? lista[0].considerando_dispo : "";
            }
            db.Dispose();
            return considerando;
        }

        public int GenerarPDF_Disposicion(int id_solicitud, int id_tramitetarea, Guid UserId, string expediente,
                      int id_paquete, int id_caratula, ref byte[] documento)
        {

            IniciarEntityFiles();

            int nro_tramite = 0;

            Stream msPdfDisposicion = null;
            documento = null;

            App_Data.dsImpresionDisposicion dsDispo =  GenerarDataSetDisposicion(id_solicitud, id_tramitetarea, expediente, true);

            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
            try
            {
                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/Disposicion.rpt";
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
                using (TransactionScope Tran = new TransactionScope())
                {

                    try
                    {
                        nro_tramite = ws_FilesRest.subirArchivo("Disposicion.pdf", documento);

                        int id_tipodocsis = (int)Constants.TiposDeDocumentosSistema.DISPOSICION_HABILITACION;
                        ObjectParameter param_id_docadjunto = new ObjectParameter("id_docadjunto", typeof(int));
                        db.SSIT_DocumentosAdjuntos_Add(id_solicitud, 0, "", id_tipodocsis, true, nro_tramite, "Disposicion.pdf", UserId, param_id_docadjunto);

                        Tran.Complete();
                        Tran.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        LogError.Write(ex, "Error en transaccion. GenerarPDF_Disposicion-id_tramitetarea:" + id_tramitetarea + "-id_solicitud:" + id_solicitud);
                        throw ex;
                    }

                }


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

            return nro_tramite;
        }


        public string GenerarHtml_Disposicion(int id_solicitud, int id_tramitetarea, string expediente)
        {
            string emailHtml = "";
            
            string surl  ="";

            WebRequest request = null;
            WebResponse response = null;

            StreamReader reader = null;

            try
            {
                Control ctl = new Control();
                string urlSGI = Functions.GetParametroChar("SGI.Url");
                int id_resultado;
                id_resultado = Functions.isResultadoDispo(id_solicitud);
                int nroSolReferencia = 0;
                int.TryParse(Functions.GetParametroChar("NroSolicitudReferencia"), out nroSolReferencia);

                SGI_Tramites_Tareas_HAB tarea = db.SGI_Tramites_Tareas_HAB.Where(x => x.id_tramitetarea == id_tramitetarea).FirstOrDefault();

                if ((id_solicitud > nroSolReferencia
                    && (id_resultado == (int)Constants.ENG_ResultadoTarea.Rechazado
                    || id_resultado == (int)Constants.ENG_ResultadoTarea.Requiere_Rechazo
                    || tarea.SSIT_Solicitudes.id_tipotramite == (int)Constants.TipoDeTramite.Ampliacion_Unificacion
                    || tarea.SSIT_Solicitudes.id_tipotramite == (int)Constants.TipoDeTramite.RedistribucionDeUso
                    || id_resultado == (int)Constants.ENG_ResultadoTarea.Aprobado_Reconsideracion
                    || id_resultado == (int)Constants.ENG_ResultadoTarea.Observado_Reconsideracion
                    || id_resultado == (int)Constants.ENG_ResultadoTarea.Rechazado_Reconsideracion))
                    || Functions.caduco(id_solicitud))
                    surl = urlSGI + "/Reportes/ImprimirDispoHtmlNuevoCur.aspx";
                else
                    surl = urlSGI + "/Reportes/ImprimirDispoHtml.aspx";

                ctl.Dispose();

                string url = string.Format("{0}?id_solicitud={1}&id_tramitetarea={2}&nro_expediente={3}", surl, id_solicitud, id_tramitetarea, HttpUtility.UrlEncode(expediente));
      

                request = WebRequest.Create(url);
                response = request.GetResponse();
                var encoding = Encoding.GetEncoding(Functions.GetParametroChar("Server.Encoding"));
                reader = new StreamReader(response.GetResponseStream(), encoding);
                emailHtml = reader.ReadToEnd();

                reader.Dispose();
                response.Dispose();
            }
            catch (Exception ex)
            {
                if ( reader != null ) reader.Dispose();
                if (response != null ) response.Dispose();
                string mensaje = "";
                try
                {
                    if (HttpContext.Current != null)
                    {
                        if (HttpContext.Current.Request != null)
                        {
                            if (HttpContext.Current.Request.Url != null)
                            {
                                mensaje = " HttpContext.Current.Request.Url.Authority  = " + HttpContext.Current.Request.Url.Authority;
                                mensaje = mensaje + " HttpContext.Current.Request.Url.AbsoluteUri  = " + HttpContext.Current.Request.Url.AbsoluteUri;
                                mensaje = mensaje + " HttpContext.Current.Request.Url.AbsolutePath  = " + HttpContext.Current.Request.Url.AbsolutePath;



                            }
                            else
                            {
                                mensaje = "HttpContext.Current.Request.Url  = null";
                            }

                        }
                        else
                        {
                            mensaje = "HttpContext.Current.Request  = null";
                        }


                    }
                    else
                    {
                        mensaje = "HttpContext.Current  = null";
                    }

                }
                catch (Exception ex2)
                {
                    mensaje = "error en cadena de if " + ex2.Message;
                }

                LogError.Write(ex, mensaje);

                throw ex;
            }


            return emailHtml;
        }

        public static dsImpresionDisposicion Transmisiones_GenerarDataSetDisposicion(int id_solicitud, int id_tramitetarea, string expediente)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            dsImpresionDisposicion ds = new dsImpresionDisposicion();
            DataRow row;

            calificador query_tt_calif = new calificador();

            string obs_plancheta_gerente = Transf_Buscar_ObservacionPlancheta(id_solicitud, id_tramitetarea);

            var cir = (from tt in db.SGI_Tramites_Tareas
                       join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                       join circ in db.ENG_Circuitos on tar.id_circuito equals circ.id_circuito
                       where tt.id_tramitetarea == id_tramitetarea
                       select new { circ.id_circuito }).FirstOrDefault();
            //-- -------------------------
            //-- Datos Disposicion
            //-- -------------------------
           query_tt_calif =
               (
                  from tt in db.SGI_Tramites_Tareas
                  join tt_cp in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                  join up in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals up.userid
                  where tt_cp.id_solicitud == id_solicitud && tt.id_tarea == (int)SGI.Constants.ENG_Tareas.TR_Nueva_Calificar
                  orderby tt.id_tramitetarea descending
                  select new calificador
                  {
                      nombre = up.Nombres,
                      apellido = up.Apellido
                  }
               ).FirstOrDefault();

            Transf_Solicitudes soli = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);

            DataTable dtDisposicion = ds.Tables["Disposicion"];
            if (expediente == null || expediente == "")
            {
                string nro_expediente_sade = soli.NroExpedienteSade;
                if (nro_expediente_sade != null)
                {
                    string[] datos = nro_expediente_sade.Split('-');
                    if (datos.Length > 2)
                        expediente = Convert.ToString(Convert.ToInt32(datos[2])) + "/" + datos[1];
                }
            }
            SGI_Tramites_Tareas tramitetarea = db.SGI_Tramites_Tareas.Where(x => x.id_tramitetarea == id_tramitetarea).FirstOrDefault();

            row = dtDisposicion.NewRow();

            row["nro_disposicion"] = 0;
            row["expediente"] = expediente;
            row["nro_solicitud"] = id_solicitud;
            row["calificador_nombre"] = query_tt_calif.nombre;
            row["calificador_apellido"] = query_tt_calif.apellido;
            row["observacion_disposicion"] = obs_plancheta_gerente;
            row["fecha_disposicion"] = tramitetarea.FechaInicio_tramitetarea;

            dtDisposicion.Rows.Add(row);
            
            var encomienda = db.Encomienda.Where(x => x.Encomienda_Transf_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                    && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
            int id_encomienda = encomienda.id_encomienda;
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

            //-- -------------------------
            //-- Datos transferecnias, tabla 0
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
                        id_tipotramite = enc.id_tipotramite,
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
                        NroExpediente = "",
                        InformaModificacion = enc.InformaModificacion
                    }

                ).ToList();

            DataTable dtEncomienda = ds.Tables["Encomienda"];

            foreach (var item in query_enc)
            {
                row = dtEncomienda.NewRow();

                row["id_encomienda"] = item.id_encomienda;
                row["FechaEncomienda"] = item.FechaEncomienda;
                row["nroEncomiendaconsejo"] = item.nroEncomiendaconsejo;
                row["ZonaDeclarada"] = item.ZonaDeclarada;
                row["TipoDeTramite"] = item.TipoDeTramite;
                row["TipoDeExpediente"] = item.TipoDeExpediente;
                row["SubTipoDeExpediente"] = item.SubTipoDeExpediente;
                row["MatriculaProfesional"] = item.MatriculaProfesional;
                row["ApellidoProfesional"] = item.ApellidoProfesional;
                row["NombresProfesional"] = item.NombresProfesional;
                row["TipoDocProfesional"] = item.TipoDocProfesional;
                row["DocumentoProfesional"] = item.DocumentoProfesional;
                row["id_grupoconsejo"] = item.id_grupoconsejo;
                row["ConsejoProfesional"] = item.ConsejoProfesional;
                row["TipoNormativa"] = item.TipoNormativa;
                row["EntidadNormativa"] = item.EntidadNormativa;
                row["NroNormativa"] = item.NroNormativa;
                row["LogoUrl"] = item.LogoUrl;
                row["ImpresionDePrueba"] = item.ImpresionDePrueba;
                row["Logo"] = null;
                row["PlantasHabilitar"] = item.PlantasHabilitar;
                row["ObservacionesPlantasHabilitar"] = item.ObservacionesPlantasHabilitar;
                row["ObservacionesRubros"] = item.ObservacionesRubros;
                row["id_encomienda_anterior"] = item.id_encomienda_anterior;
                row["NroExpediente"] = item.NroExpediente;
                row["InformaModificacion"] = item.InformaModificacion;

                dtEncomienda.Rows.Add(row);
            }

            var query_ubi =
                (
                    from sol in db.Transf_Solicitudes
                    join trubic in db.Transf_Ubicaciones on sol.id_solicitud equals trubic.id_solicitud
                    join mat in db.Ubicaciones on trubic.id_ubicacion equals mat.id_ubicacion
                    join zon1 in db.Zonas_Planeamiento on trubic.id_zonaplaneamiento equals zon1.id_zonaplaneamiento
                    where sol.id_solicitud == id_solicitud
                    orderby trubic.id_transfubicacion
                    select new
                    {
                        trubic.id_transfubicacion,
                        sol.id_solicitud,
                        sol.id_cpadron,
                        trubic.id_ubicacion,
                        mat.Seccion,
                        mat.Manzana,
                        mat.Parcela,
                        NroPartidaMatriz = mat.NroPartidaMatriz,
                        trubic.local_subtipoubicacion,
                        ZonaParcela = zon1.CodZonaPla,
                        DeptoLocal = trubic.deptoLocal_transfubicacion
                    }
                ).ToList();

            DataTable dtUbicaciones = ds.Tables["Ubicaciones"];


            foreach (var item in query_ubi)
            {
                row = dtUbicaciones.NewRow();

                string sql = string.Empty;
                    sql = "select dbo.Transferencia_Solicitud_DireccionesPartidas(" + item.id_solicitud + ")";

                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                row["id_encomiendaubicacion"] = item.id_transfubicacion;
                row["id_encomienda"] = id_encomienda;
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

            
                // Ubicaciones_PropiedadHorizontal
                var query_ph =
                (
                    from sol in db.Transf_Solicitudes
                    join trubic in db.Transf_Ubicaciones on sol.id_solicitud equals trubic.id_solicitud
                    join trphor in db.Transf_Ubicaciones_PropiedadHorizontal on trubic.id_transfubicacion equals trphor.id_transfubicacion
                    join phor in db.Ubicaciones_PropiedadHorizontal on trphor.id_propiedadhorizontal equals phor.id_propiedadhorizontal
                    where sol.id_solicitud == id_solicitud
                    orderby trubic.id_transfubicacion
                    select new
                    {
                        id_encomiendaubicacion = trubic.id_transfubicacion,
                        trubic.id_ubicacion,
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
                    join trubic in db.Transf_Ubicaciones on sol.id_solicitud equals trubic.id_solicitud
                    join trpuer in db.Transf_Ubicaciones_Puertas on trubic.id_transfubicacion equals trpuer.id_transfubicacion
                    where sol.id_solicitud == id_solicitud
                    orderby trubic.id_transfubicacion
                    select new
                    {
                        id_encomiendaubicacion = trubic.id_transfubicacion,
                        id_encomiendapuerta = trpuer.id_transfpuerta,
                        id_encomienda = trubic.id_solicitud,
                        trubic.id_ubicacion,
                        Calle = trpuer.nombre_calle,
                        trpuer.NroPuerta
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
                from pj in db.Transf_Firmantes_PersonasJuridicas
                join titpj in db.Transf_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                join tcl in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tcl.id_tipocaracter
                join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                where pj.id_solicitud == id_solicitud && titpj.Id_TipoSociedad != 2 // Todas las sociedades menos la Sociedad de Hecho
                && titpj.Id_TipoSociedad != 32

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
                from pj in db.Transf_Firmantes_PersonasJuridicas
                join titpj in db.Transf_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                join titsh in db.Transf_Titulares_PersonasJuridicas_PersonasFisicas on pj.id_firmante_pj equals titsh.id_firmante_pj
                join tcl in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tcl.id_tipocaracter
                join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                where pj.id_solicitud == id_solicitud && (titpj.Id_TipoSociedad == 2 // Solo Sociedades de Hecho
                || titpj.Id_TipoSociedad == 32)
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

            // Rubros
            var query_Rubros =(
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

            var query_RubrosAnt = ((from encrub in db.Encomienda_Rubros_AT_Anterior
                                    join tdocreq in db.Tipo_Documentacion_Req on encrub.id_tipodocreq equals tdocreq.Id
                                    join tact in db.TipoActividad on encrub.id_tipoactividad equals tact.Id
                                    where encrub.id_encomienda == encomienda.id_encomienda
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
                                             where encrub.id_encomienda == encomienda.id_encomienda
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

                //https://mantis.grupomost.com/view.php?id=161185
                //dtRubros.Rows.Add(row);
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

            // Depositos

            var query_Depositos =
                (from enc in db.Encomienda
                 join ercd in db.Encomienda_RubrosCN_Deposito on enc.id_encomienda equals ercd.id_encomienda
                 join rdc in db.RubrosDepositosCN on ercd.IdDeposito equals rdc.IdDeposito
                 where enc.id_encomienda == id_encomienda
                 select new
                 {
                     id_encomienda = enc.id_encomienda,
                     idRubro = ercd.IdRubro,
                     IdDeposito = rdc.IdDeposito,
                     Codigo = rdc.Codigo,
                     Descripcion = rdc.Descripcion
                 }).ToList();

            DataTable dtDepositos = ds.Tables["EncomiendaDepositos"];

            foreach (var d in query_Depositos)
            {
                row = dtDepositos.NewRow();

                row["id_encomienda"] = d.id_encomienda;
                row["idRubro"] = d.idRubro;
                row["IdDeposito"] = d.IdDeposito;
                row["Codigo"] = d.Codigo;
                row["Descripcion"] = d.Descripcion;

                dtDepositos.Rows.Add(row);
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

            DataTable dtMixturas = ds.Tables["Mixturas"];

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

            DataTable dtDistritos = ds.Tables["Distritos"];

            foreach (var item in query_Distritos)
            {
                row = dtDistritos.NewRow();
                row["Descripcion"] = item.descrip;

                dtDistritos.Rows.Add(row);
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
                        dl.cantidad_operarios_dl,
                        dl.ampliacion_superficie,
                        dl.superficie_cubierta_amp,
                        dl.superficie_descubierta_amp
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

            //Solicitud anterior
            if(soli.idSolicitudRef.HasValue && soli.idSolicitudRef.Value > 0)
            {
                var stt = db.SGI_Tramites_Tareas_HAB.Where(x => x.id_solicitud == soli.idSolicitudRef.Value).FirstOrDefault();
                if (stt != null)
                {
                    int cod = Convert.ToInt32(Convert.ToString(db.SGI_Tramites_Tareas_HAB.Where(x => x.id_solicitud == soli.idSolicitudRef.Value).FirstOrDefault().SGI_Tramites_Tareas.ENG_Tareas.id_circuito) +
                                    Constants.ENG_Tipos_Tareas.Revision_Firma_Disposicion);
                    var querySolAnt = (from solici in db.SSIT_Solicitudes
                                       join tth in db.SGI_Tramites_Tareas_HAB on solici.id_solicitud equals tth.id_solicitud
                                       where solici.id_solicitud == soli.idSolicitudRef.Value
                                       && tth.SGI_Tramites_Tareas.ENG_Tareas.cod_tarea == cod
                                       select new
                                       {
                                           nrExp = solici.NroExpedienteSade,
                                           fecha = tth.SGI_Tramites_Tareas.FechaCierre_tramitetarea,
                                           idtramitet = tth.id_tramitetarea
                                       }).OrderByDescending(y => y.fecha).FirstOrDefault();
                    DataTable dtSolAnt = ds.Tables["SolAnt"];

                    if (querySolAnt != null)
                    {
                        string obs = Buscar_ObservacionPlancheta(soli.idSolicitudRef.Value, querySolAnt.idtramitet);
                        row = dtSolAnt.NewRow();

                        row["NroExp"] = querySolAnt.nrExp;
                        row["FechaDispo"] = querySolAnt.fecha;
                        row["Observaciones"] = obs;
                        row["CP"] = soli.id_cpadron;

                        dtSolAnt.Rows.Add(row);
                    }
                }
            }
            
            db.Dispose();
            return ds;
        }
        public static dsImpresionDisposicion Transf_GenerarDataSetDisposicion(int id_solicitud, int id_tramitetarea, string expediente)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            dsImpresionDisposicion ds = new dsImpresionDisposicion();
            DataRow row;

            calificador query_tt_calif = new calificador();

            string obs_plancheta_gerente = Transf_Buscar_ObservacionPlancheta(id_solicitud, id_tramitetarea);

            var cir = (from tt in db.SGI_Tramites_Tareas
                       join tar in db.ENG_Tareas on tt.id_tarea equals tar.id_tarea
                       join circ in db.ENG_Circuitos on tar.id_circuito equals circ.id_circuito
                       where tt.id_tramitetarea == id_tramitetarea
                       select new { circ.id_circuito }).FirstOrDefault();
            //-- -------------------------
            //-- Datos Disposicion
            //-- -------------------------
            if (cir.id_circuito == (int)Constants.ENG_Circuitos.TRANSF_NUEVO)
            {
                query_tt_calif =
               (
                  from tt in db.SGI_Tramites_Tareas
                  join tt_cp in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                  join up in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals up.userid
                  where tt_cp.id_solicitud == id_solicitud && tt.id_tarea == (int)SGI.Constants.ENG_Tareas.TR_Nueva_Generar_Expediente
                  orderby tt.id_tramitetarea descending
                  select new calificador
                  {
                      nombre = up.Nombres,
                      apellido = up.Apellido
                  }
               ).FirstOrDefault();
            }
            else
            {
                //buscar tramite tarea de calificador
                query_tt_calif =
                   (
                      from tt in db.SGI_Tramites_Tareas
                      join tt_cp in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tt_cp.id_tramitetarea
                      join up in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals up.userid
                      where tt_cp.id_solicitud == id_solicitud && tt.id_tarea == (int)SGI.Constants.ENG_Tareas.TR_Calificar
                      orderby tt.id_tramitetarea descending
                      select new calificador
                      {
                          nombre = up.Nombres,
                          apellido = up.Apellido
                      }
                   ).FirstOrDefault();

            }


            DataTable dtDisposicion = ds.Tables["Disposicion"];
            if (expediente == null || expediente == "")
            {
                Transf_Solicitudes sol = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);
                string nro_expediente_sade = sol.NroExpedienteSade;
                if (nro_expediente_sade != null)
                {
                    string[] datos = nro_expediente_sade.Split('-');
                    if (datos.Length > 2)
                        expediente = Convert.ToString(Convert.ToInt32(datos[2])) + "/" + datos[1];
                }
            }
            SGI_Tramites_Tareas tramitetarea = db.SGI_Tramites_Tareas.Where(x => x.id_tramitetarea == id_tramitetarea).FirstOrDefault();

            row = dtDisposicion.NewRow();

            row["nro_disposicion"] = 0;
            row["expediente"] = expediente;
            row["nro_solicitud"] = id_solicitud;
            row["calificador_nombre"] = query_tt_calif.nombre;
            row["calificador_apellido"] = query_tt_calif.apellido;
            row["observacion_disposicion"] = obs_plancheta_gerente;
            row["fecha_disposicion"] = tramitetarea.FechaInicio_tramitetarea;

            dtDisposicion.Rows.Add(row);


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
                {
                    strPlantasHabilitar = strPlantasHabilitar + separador + item.Descripcion;
                }

            }

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
                        ZonaDeclarada = sol.ZonaDeclarada,
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
                        ImpresionDePrueba = (sol.id_estado <= 1) ? true : false,
                        PlantasHabilitar = strPlantasHabilitar,
                        ObservacionesPlantasHabilitar = "",
                        ObservacionesRubros = "",
                        id_encomienda_anterior = 0,
                        NroExpediente = "" 
                    }

                ).ToList();

            DataTable dtEncomienda = ds.Tables["Encomienda"];

            foreach (var item in query_enc)
            {
                row = dtEncomienda.NewRow();

                row["id_encomienda"] = item.id_solicitud;
                row["FechaEncomienda"] = item.CreateDate;
                row["nroEncomiendaconsejo"] = item.nroEncomiendaconsejo;
                row["ZonaDeclarada"] = item.ZonaDeclarada;
                row["TipoDeTramite"] = item.TipoDeTramite;
                row["TipoDeExpediente"] = item.TipoDeExpediente;
                row["SubTipoDeExpediente"] = item.SubTipoDeExpediente;
                row["MatriculaProfesional"] = item.MatriculaProfesional;
                row["ApellidoProfesional"] = item.ApellidoProfesional;
                row["NombresProfesional"] = item.NombresProfesional;
                row["TipoDocProfesional"] = item.TipoDocProfesional;
                row["DocumentoProfesional"] = item.DocumentoProfesional;
                row["id_grupoconsejo"] = item.id_grupoconsejo;
                row["ConsejoProfesional"] = item.ConsejoProfesional;
                row["TipoNormativa"] = item.TipoNormativa;
                row["EntidadNormativa"] = item.EntidadNormativa;
                row["NroNormativa"] = item.NroNormativa;
                row["LogoUrl"] = item.LogoUrl;
                row["ImpresionDePrueba"] = item.ImpresionDePrueba;
                row["Logo"] = null;
                row["PlantasHabilitar"] = item.PlantasHabilitar;
                row["ObservacionesPlantasHabilitar"] = item.ObservacionesPlantasHabilitar;
                row["ObservacionesRubros"] = item.ObservacionesRubros;
                row["id_encomienda_anterior"] = item.id_encomienda_anterior;
                row["NroExpediente"] = item.NroExpediente;

                dtEncomienda.Rows.Add(row);
            }

            var query_ubi =
                (
                    from sol in db.Transf_Solicitudes
                    join cpubic in db.CPadron_Ubicaciones on sol.id_cpadron equals cpubic.id_cpadron into cpadronLeft from cp in cpadronLeft.DefaultIfEmpty()
                    join ubi in db.Ubicaciones on cp.id_ubicacion equals ubi.id_ubicacion into ubiLeft from ub in ubiLeft.DefaultIfEmpty()
                    join zon1 in db.Zonas_Planeamiento on cp.id_zonaplaneamiento equals zon1.id_zonaplaneamiento
                    where sol.id_solicitud == id_solicitud
                    orderby cp.id_cpadronubicacion
                    select new
                    {
                        cp.id_cpadronubicacion,
                        sol.id_solicitud,
                        sol.id_cpadron,
                        cp.id_ubicacion,
                        ub.Seccion,
                        ub.Manzana,
                        ub.Parcela,
                        NroPartidaMatriz = ub.NroPartidaMatriz,
                        cp.local_subtipoubicacion,
                        ZonaParcela = zon1.CodZonaPla,
                        DeptoLocal = cp.deptoLocal_cpadronubicacion
                    }
                ).ToList();
            DataTable dtUbicaciones = ds.Tables["Ubicaciones"];


            foreach (var item in query_ubi)
            {
                row = dtUbicaciones.NewRow();

                string sql = string.Empty;
                sql = "select dbo.Transf_Solicitud_DireccionesPartidas(" + item.id_solicitud + ")";

                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                row["id_encomiendaubicacion"] = item.id_cpadronubicacion;
                row["id_encomienda"] = item.id_solicitud;
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
                   join trphor in db.Transf_Ubicaciones_PropiedadHorizontal on cpubic.id_cpadronubicacion equals trphor.id_transfubicacion
                   join phor in db.Ubicaciones_PropiedadHorizontal on trphor.id_propiedadhorizontal equals phor.id_propiedadhorizontal
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
                row["id_ubicacion"] = item.id_ubicacion;

                if (item.NroPartidaHorizontal.HasValue)
                    row["NroPartidaHorizontal"] = item.NroPartidaHorizontal;
                else
                    row["NroPartidaHorizontal"] = DBNull.Value;

                row["Piso"] = item.Piso;
                row["Depto"] = item.Depto;

                dtPH.Rows.Add(row);
            }

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
                    && titpj.Id_TipoSociedad != 32

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
                from pj in db.Transf_Firmantes_PersonasJuridicas
                join titpj in db.Transf_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                join titsh in db.Transf_Titulares_PersonasJuridicas_PersonasFisicas on pj.id_firmante_pj equals titsh.id_firmante_pj
                join tcl in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tcl.id_tipocaracter
                join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                where pj.id_solicitud == id_solicitud && (titpj.Id_TipoSociedad == 2 // Solo Sociedades de Hecho
                || titpj.Id_TipoSociedad == 32)
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

            var query_Titulares2 =
                (
                    from pj in db.Transf_Titulares_PersonasJuridicas_PersonasFisicas
                    join titpj in db.Transf_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                    join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where titpj.id_solicitud == id_solicitud && (titpj.Id_TipoSociedad == 2 || titpj.Id_TipoSociedad == 32)
                    select new
                    {
                        id_persona = pj.id_firmante_pj,
                        titpj.id_solicitud,
                        TipoPersona = "PF",
                        Apellido = pj.Apellido,
                        Nombres = pj.Nombres,
                        TipoDoc = tdoc.Nombre,
                        NroDoc = pj.Nro_Documento,
                        MuestraEnTitulares = 0,
                        MuestraEnPlancheta = 1
                    }
                ).Distinct().ToList();

            foreach (var item in query_Titulares2)
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
                row["TipoIIBB"] = "";
                row["NroIIBB"] = "";
                row["Cuit"] = "";
                row["MuestraEnTitulares"] = item.MuestraEnTitulares;
                row["MuestraEnPlancheta"] = item.MuestraEnPlancheta;

                dtTitulares.Rows.Add(row);
            }

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

                row["id_encomiendarubro"] = item.id_cpadronrubro;
                row["id_encomienda"] = item.id_solicitud;
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
                row["sanitarios_distancia_dl"] = item.sanitarios_distancia_dl;
                row["cantidad_sanitarios_dl"] = item.cantidad_sanitarios_dl;
                row["superficie_sanitarios_dl"] = item.superficie_sanitarios_dl;
                row["frente_dl"] = item.frente_dl;
                row["fondo_dl"] = item.fondo_dl;
                row["lateral_izquierdo_dl"] = item.lateral_izquierdo_dl;
                row["lateral_derecho_dl"] = item.lateral_derecho_dl;
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
        private static string Transf_Buscar_ObservacionPlancheta(int id_solicitud, int id_tramitetarea)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            string observ = "";

            /*if (Functions.isAprobado(id_solicitud))
                observ = (from param in db.Parametros
                          where param.cod_param == "SGI.Notas.Adicionales.Dispo"
                          select param.valorchar_param).FirstOrDefault().ToUpper();*/

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
                observ +=  lista[0].observacion_plancheta.ToString().Replace("\n", "\r\n");
            }
            db.Dispose();
            return observ;
        }
        public static string Transf_GenerarHtml_Disposicion(int id_solicitud, int id_tramitetarea, string expediente, string str_archivo)
        {
            string disposicion_html = str_archivo;
            string lblObservacion = "";

            dsImpresionDisposicion dsDispo = Transf_GenerarDataSetDisposicion(id_solicitud, id_tramitetarea, expediente);

            DataRow row = null;

            row = dsDispo.Tables["Disposicion"].Rows[0];

            string dispo_observacion = HttpUtility.HtmlEncode(Convert.ToString(row["observacion_disposicion"])).Replace("\r\n", "<br>");
            string dispo_calificador = HttpUtility.HtmlEncode(Convert.ToString(row["calificador_apellido"]) + ", " + Convert.ToString(row["calificador_nombre"]));
            string dispo_nro_expediente = HttpUtility.HtmlEncode(Convert.ToString(row["expediente"]));
            string fecha_disposicion = HttpUtility.HtmlEncode(Convert.ToString(row["fecha_disposicion"]));            

            DateTime d = Convert.ToDateTime(fecha_disposicion);
            string lblDia = d.ToString("dd MMMM 'de' yyyy", CultureInfo.CreateSpecificCulture("es-ES"));
            decimal sup_total_habilitar = 0;
            string cantidad_operarios_dl = "";
            if (dsDispo.Tables["DatosLocal"].Rows.Count != 0)
            {
                row = dsDispo.Tables["DatosLocal"].Rows[0];

                decimal superficie_cubierta_dl = Convert.ToDecimal(row["superficie_cubierta_dl"]);
                decimal superficie_descubierta_dl = Convert.ToDecimal(row["superficie_descubierta_dl"]);
                sup_total_habilitar = superficie_cubierta_dl + superficie_descubierta_dl;
                cantidad_operarios_dl = Convert.ToString(row["cantidad_operarios_dl"]);
            }
            row = dsDispo.Tables["Encomienda"].Rows[0];

            string zonaDeclarada = Convert.ToString(row["ZonaDeclarada"]);
            string plantasHabilitar = Convert.ToString(row["PlantasHabilitar"]);

            string lblNroSolicitud = string.Format("{0:###,###,##0}", id_solicitud);
            string lblNroExpediente = dispo_nro_expediente;

            string lblTirulares = "", nombre = "";
            foreach (DataRow t in dsDispo.Tables["Titulares"].Rows)
            {
                if (Convert.ToString(t["TipoPersona"]).Equals("PF"))
                    nombre = Convert.ToString(t["Apellido"]) +", "+Convert.ToString(t["Nombres"]);
                else 
                    nombre = Convert.ToString(t["RazonSocial"]);
                lblTirulares += "<tr><td style='padding:0'><span class='text'>" + nombre + "</span></td></tr>";
            }
            
            row = dsDispo.Tables["Ubicaciones"].Rows[0];

            string zonaParcela = Convert.ToString(row["ZonaParcela"]);

            string lblSeccion = Convert.ToString(row["Seccion"]);
            string lblManzana = Convert.ToString(row["Manzana"]);
            string lblParcela = Convert.ToString(row["Parcela"]);
            string lblPartidaMatriz = Convert.ToString(row["NroPartidaMatriz"]);

            string lblDomicilio = HttpUtility.HtmlEncode(Convert.ToString(row["Direcciones"]));


            string lblPH = "";
            if (dsDispo.Tables["PropiedadHorizontal"].Rows.Count > 0)
            {
                row = dsDispo.Tables["PropiedadHorizontal"].Rows[0];
                lblPH="<table class='table table-condensed'>" +
                      "     <tr>" +
                      "         <td colspan='2'>Partida horizontal: " +Convert.ToString(row["NroPartidaHorizontal"]) +"</td>" +
                      "         <td colspan='2'>Piso: " + HttpUtility.HtmlEncode(Convert.ToString(row["Piso"])) + "</td>" +
                      "         <td colspan='4'>U.F.: " + HttpUtility.HtmlEncode(Convert.ToString(row["Depto"])) + "</td>" +
                      "     </tr>" +
                      "</table>";
            }

            string lblPlantashabilitar = HttpUtility.HtmlEncode(plantasHabilitar);

            string lblZonaDeclarada = zonaDeclarada;
            string lblCantOperarios = cantidad_operarios_dl;

            string lblSuperficieTotal = string.Format("{0:###,###,##0.00} m2.", sup_total_habilitar);
            string lblCalificar = dispo_calificador;

            string lblRubros = "";
            foreach (DataRow r in dsDispo.Tables["Rubros"].Rows)
            {
                lblRubros += "<tr>" +
                          " <td> " + HttpUtility.HtmlEncode(Convert.ToString(r["cod_rubro"])) + "</td>" +
                          " <td> " + HttpUtility.HtmlEncode(Convert.ToString(r["desc_rubro"])) + "</td>" +
                          " <td> " + HttpUtility.HtmlEncode(Convert.ToString(r["DocRequerida"])) + "</td>" +
                          " <td> " + HttpUtility.HtmlEncode(Convert.ToString(r["SuperficieHabilitar"])) + "</td>" +
                          "</tr>";
            }

            
            if(!string.IsNullOrEmpty(dispo_observacion))
                lblObservacion = "<table class='table table-condensed'>" +
                                "   <tr>" +
                                "       <td style='vertical-align:top'><b>Observaciones:</b></td>" +
                                "       <td> " + dispo_observacion + "</td>" +
                                "   </tr>" +
                                "</table>";

            disposicion_html = disposicion_html.Replace("@lblNroExpediente", lblNroExpediente);
            disposicion_html = disposicion_html.Replace("@lblNroSolicitud", lblNroSolicitud);
            disposicion_html = disposicion_html.Replace("@lblDia", lblDia);
            disposicion_html = disposicion_html.Replace("@lblSeccion", lblSeccion);
            disposicion_html = disposicion_html.Replace("@lblManzana", lblManzana);
            disposicion_html = disposicion_html.Replace("@lblParcela", lblParcela);
            disposicion_html = disposicion_html.Replace("@lblPartidaMatriz", lblPartidaMatriz);
            disposicion_html = disposicion_html.Replace("@lblDomicilio", lblDomicilio);
            disposicion_html = disposicion_html.Replace("@lblPlantashabilitar", lblPlantashabilitar);
            disposicion_html = disposicion_html.Replace("@lblZonaDeclarada", lblZonaDeclarada);
            disposicion_html = disposicion_html.Replace("@lblCantOperarios", lblCantOperarios);
            disposicion_html = disposicion_html.Replace("@lblSuperficieTotal", lblSuperficieTotal);
            disposicion_html = disposicion_html.Replace("@lblCalificar", lblCalificar);
            disposicion_html = disposicion_html.Replace("@lblObservacion", lblObservacion);

            disposicion_html = disposicion_html.Replace("@lblTirulares", lblTirulares);
            disposicion_html = disposicion_html.Replace("@lblPH", lblPH);
            disposicion_html = disposicion_html.Replace("@lblRubros", lblRubros);

            disposicion_html = disposicion_html.Replace("@lblSubRubros", null);
            disposicion_html = disposicion_html.Replace("@lblDepositos", null);

            return disposicion_html;
        }
        public static string Transmision_GenerarHtml_Disposicion(int id_solicitud, int id_tramitetarea, string expediente, string str_archivo)
        {
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            string disposicion_html = str_archivo;
            string lblObservacion = "";
            var transmision = db.Transf_Solicitudes.Where(x => x.id_solicitud == id_solicitud).FirstOrDefault();

            dsImpresionDisposicion dsDispo = Transmisiones_GenerarDataSetDisposicion(id_solicitud, id_tramitetarea, expediente);

            bool isRechazo = Functions.isResultadoDispoTransmision(id_solicitud) == (int)Constants.ENG_ResultadoTarea.Aprobado;

            DataRow row = null;

            row = dsDispo.Tables["Disposicion"].Rows[0];

            string dispo_observacion =   HttpUtility.HtmlEncode(Convert.ToString(row["observacion_disposicion"])).Replace("\r\n", "<br>");
            string dispo_calificador = HttpUtility.HtmlEncode(Convert.ToString(row["calificador_apellido"]) + ", " + Convert.ToString(row["calificador_nombre"]));
            string dispo_nro_expediente = HttpUtility.HtmlEncode(Convert.ToString(row["expediente"]));
            string fecha_disposicion = HttpUtility.HtmlEncode(Convert.ToString(row["fecha_disposicion"]));

            DateTime d = Convert.ToDateTime(fecha_disposicion);
            string lblDia = d.ToString("dd MMMM 'de' yyyy", CultureInfo.CreateSpecificCulture("es-ES"));
            decimal sup_total_habilitar = 0;
            string cantidad_operarios_dl = "";
            if (dsDispo.Tables["DatosLocal"].Rows.Count != 0)
            {
                row = dsDispo.Tables["DatosLocal"].Rows[0];

                decimal superficie_cubierta_dl = Convert.ToDecimal(row["superficie_cubierta_dl"]);
                decimal superficie_descubierta_dl = Convert.ToDecimal(row["superficie_descubierta_dl"]);
                sup_total_habilitar = superficie_cubierta_dl + superficie_descubierta_dl;
                cantidad_operarios_dl = Convert.ToString(row["cantidad_operarios_dl"]);
            }
            row = dsDispo.Tables["Encomienda"].Rows[0];

            string considerando = string.Empty;
            string articulo = string.Empty;
            string articulo2 = string.Empty;
            string articulo3 = string.Empty;
            string articulo4 = string.Empty;           

            if (isRechazo)
            {
                //Oficio judicial con modificacion (APROBADO PARCIALMENTE)
                if (transmision.idTipoTransmision == (int)Constants.TipoTransmision.Transmision_oficio_judicial &&
                    Convert.ToBoolean(row["InformaModificacion"]))
                {
                    considerando = db.Disposiciones_Texto.Where(x => x.cod_dispo == "CON.DISPO.TR.OJ.AP").FirstOrDefault().texto;
                    articulo = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART.DISPO.TR.OJ.APRUEBA").FirstOrDefault().texto, "sin");
                    articulo2 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART2.DISPO.TR.AP").FirstOrDefault().texto;
                    articulo3 = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART3o4.DISPO.TR").FirstOrDefault().texto, "3");
                }
                //Oficio judicial sin modificacion(APROBADO TOTALMENTE)
                else if (transmision.idTipoTransmision == (int)Constants.TipoTransmision.Transmision_oficio_judicial &&
                    !Convert.ToBoolean(row["InformaModificacion"]))
                {
                    considerando = db.Disposiciones_Texto.Where(x => x.cod_dispo == "CON.DISPO.TR.OJ.AT").FirstOrDefault().texto;
                    articulo = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART.DISPO.TR.OJ.APRUEBA").FirstOrDefault().texto, "con");
                    articulo2 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART2.DISPO.TR.AT").FirstOrDefault().texto;
                    articulo3 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART3.DISPO.TR.AT").FirstOrDefault().texto;
                    articulo4 = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART3o4.DISPO.TR").FirstOrDefault().texto, "4");
                }
                //Cambio de denominacion con modificacion (APROBADO PARCIALMENTE)
                else if (transmision.idTipoTransmision == (int)Constants.TipoTransmision.Transmision_nominacion &&
                    Convert.ToBoolean(row["InformaModificacion"]))
                {
                    considerando = db.Disposiciones_Texto.Where(x => x.cod_dispo == "CON.DISPO.TR.CD.AP").FirstOrDefault().texto;
                    articulo = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART.DISPO.TR.CD.APRUEBA").FirstOrDefault().texto, "sin");
                    articulo2 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART2.DISPO.TR.AP").FirstOrDefault().texto;
                    articulo3 = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART3o4.DISPO.TR").FirstOrDefault().texto, "3");
                }
                //Cambio de denominacion con modificacion (APROBADO TOTAL)
                else if (transmision.idTipoTransmision == (int)Constants.TipoTransmision.Transmision_nominacion &&
                    !Convert.ToBoolean(row["InformaModificacion"]))
                {
                    considerando = db.Disposiciones_Texto.Where(x => x.cod_dispo == "CON.DISPO.TR.CD.AT").FirstOrDefault().texto;
                    articulo = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART.DISPO.TR.CD.APRUEBA").FirstOrDefault().texto, "con");
                    articulo2 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART2.DISPO.TR.AT").FirstOrDefault().texto;
                    articulo3 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART3.DISPO.TR.AT").FirstOrDefault().texto;
                    articulo4 = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART3o4.DISPO.TR").FirstOrDefault().texto, "4");
                }
                //Transmision con modificacion (APROBADO PARCIALMENTE)
                else if (transmision.idTipoTransmision == (int)Constants.TipoTransmision.Transmision_Transferencia &&
                    Convert.ToBoolean(row["InformaModificacion"]))
                {
                    considerando = db.Disposiciones_Texto.Where(x => x.cod_dispo == "CON.DISPO.TR.TRANSM.AP").FirstOrDefault().texto;
                    articulo = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART.DISPO.TR.TRANSM.APRUEBA").FirstOrDefault().texto, "sin");
                    articulo2 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART2.DISPO.TR.AP").FirstOrDefault().texto;
                    articulo3 = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART3o4.DISPO.TR").FirstOrDefault().texto, "3");
                }
                //Transmision con modificacion (APROBADO TOTAL)
                else if (transmision.idTipoTransmision == (int)Constants.TipoTransmision.Transmision_Transferencia &&
                   !Convert.ToBoolean(row["InformaModificacion"]))
                {
                    considerando = db.Disposiciones_Texto.Where(x => x.cod_dispo == "CON.DISPO.TR.TRANSM.AT").FirstOrDefault().texto;
                    articulo = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART.DISPO.TR.TRANSM.APRUEBA").FirstOrDefault().texto, "con");
                    articulo2 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART2.DISPO.TR.AT").FirstOrDefault().texto;
                    articulo3 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART3.DISPO.TR.AT").FirstOrDefault().texto;
                    articulo4 = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART3o4.DISPO.TR").FirstOrDefault().texto, "4");
                }
            }
            else //Rechazo
            {
                considerando = db.Disposiciones_Texto.Where(x => x.cod_dispo == "CON.DISPO.RECHAZO").FirstOrDefault().texto;
                articulo = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART1.DISPO.RECHAZO").FirstOrDefault().texto;
                articulo2 = db.Disposiciones_Texto.Where(x => x.cod_dispo == "ART2.DISPO.RECHAZO").FirstOrDefault().texto;
            }

            string zonaDeclarada = Convert.ToString(row["ZonaDeclarada"]);
            string plantasHabilitar = Convert.ToString(row["PlantasHabilitar"]);

            string lblNroSolicitud = string.Format("{0:###,###,##0}", id_solicitud);
            string lblNroExpediente = dispo_nro_expediente;

            string lblTirulares = "", nombre = "";
            foreach (DataRow t in dsDispo.Tables["Titulares"].Rows)
            {
                if (Convert.ToString(t["TipoPersona"]).Equals("PF"))
                    nombre = Convert.ToString(t["Apellido"]) + ", " + Convert.ToString(t["Nombres"]);
                else
                    nombre = Convert.ToString(t["RazonSocial"]);
                lblTirulares += "<tr><td style='padding:0'><span class='text'>" + nombre + "</span></td></tr>";
            }

            row = dsDispo.Tables["Ubicaciones"].Rows[0];


            string lblSeccion = Convert.ToString(row["Seccion"]);
            string lblManzana = Convert.ToString(row["Manzana"]);
            string lblParcela = Convert.ToString(row["Parcela"]);
            string lblPartidaMatriz = Convert.ToString(row["NroPartidaMatriz"]);

            string lblDomicilio = HttpUtility.HtmlEncode(Convert.ToString(row["Direcciones"]));


            string lblPH = "";
            if (dsDispo.Tables["PropiedadHorizontal"].Rows.Count > 0)
            {
                row = dsDispo.Tables["PropiedadHorizontal"].Rows[0];
                lblPH = "<table class='table table-condensed'>" +
                      "     <tr>" +
                      "         <td colspan='2'>Partida horizontal: " + Convert.ToString(row["NroPartidaHorizontal"]) + "</td>" +
                      "         <td colspan='2'>Piso: " + HttpUtility.HtmlEncode(Convert.ToString(row["Piso"])) + "</td>" +
                      "         <td colspan='4'>U.F.: " + HttpUtility.HtmlEncode(Convert.ToString(row["Depto"])) + "</td>" +
                      "     </tr>" +
                      "</table>";
            }


            string lblPlantashabilitar = HttpUtility.HtmlEncode(plantasHabilitar);
            
            string zonMix = string.Empty;
            foreach (DataRow r in dsDispo.Tables["Mixturas"].Rows)
                zonMix = zonMix + " - " + Convert.ToString(r["Descripcion"]);
            foreach (DataRow r in dsDispo.Tables["Distritos"].Rows)
                zonMix = zonMix + " - " + Convert.ToString(r["Descripcion"]);

            string lblCantOperarios = cantidad_operarios_dl;

            string lblSuperficieTotal = string.Format("{0:###,###,##0.00} m2.", sup_total_habilitar);
            string lblCalificar = dispo_calificador;

            string lblRubros = "";
            foreach (DataRow r in dsDispo.Tables["Rubros"].Rows)
            {
                lblRubros += "<tr>" +
                          " <td> " + HttpUtility.HtmlEncode(Convert.ToString(r["cod_rubro"])) + "</td>" +
                          " <td> " + HttpUtility.HtmlEncode(Convert.ToString(r["desc_rubro"])) + "</td>" +
                          " <td> " + HttpUtility.HtmlEncode(Convert.ToString(r["DocRequerida"])) + "</td>" +
                          " <td> " + HttpUtility.HtmlEncode(Convert.ToString(r["SuperficieHabilitar"])) + "</td>" +
                          "</tr>";
            }

            string lblSubRubros = " ";
            if (dsDispo.Tables["SubRubros"].Rows.Count > 0)
            {
                lblSubRubros += "<table class=\"table table-condensed\"><tr>" +
                         " <td> Código </td>" +
                         " <td> Descripción </td>" +
                         "</tr>";
                foreach (DataRow r in dsDispo.Tables["SubRubros"].Rows)
                {
                    lblSubRubros += "<tr>" +
                              " <td> " + HttpUtility.HtmlEncode(Convert.ToString(r["CodigoRubro"])) + "</td>" +
                              " <td> " + HttpUtility.HtmlEncode(Convert.ToString(r["Nombre"])) + "</td>" +
                              "</tr>";
                }
                lblSubRubros += "</table>";
            }

            string lblDepositos = " ";
            if (dsDispo.Tables["EncomiendaDepositos"].Rows.Count > 0)
            {
                lblDepositos += "<table class=\"table table-condensed\"><tr>" +
                         " <td> Código </td>" +
                         " <td> Depósito </td>" +
                         "</tr>";
                foreach (DataRow r in dsDispo.Tables["EncomiendaDepositos"].Rows)
                {
                    lblDepositos += "<tr>" +
                              " <td> " + HttpUtility.HtmlEncode(Convert.ToString(r["Codigo"])) + "</td>" +
                              " <td> " + HttpUtility.HtmlEncode(Convert.ToString(r["Descripcion"])) + "</td>" +
                              "</tr>";
                }
                lblDepositos += "</table>";
            }

            string obsAnt = string.Empty;
            if (dsDispo.Tables["SolAnt"].Rows.Count > 0 && !isRechazo)
            {
                row = dsDispo.Tables["SolAnt"].Rows[0];
                DateTime dAnt = Convert.ToDateTime(Convert.ToString(row["fechaDispo"]));
                string fech = dAnt.ToString("dd MMMM 'de' yyyy", CultureInfo.CreateSpecificCulture("es-ES"));
                string nroExp = Convert.ToString(row["NroExp"]);
                string cp = Convert.ToString(row["CP"]);
                string obs = Convert.ToString(row["Observaciones"]);
                obsAnt = string.Format(db.Disposiciones_Texto.Where(x => x.cod_dispo == "OBS.DISPO.TR.OJ.APRUEBA").FirstOrDefault().texto, fech,nroExp,cp, obs);
            }
            
            if (!string.IsNullOrEmpty(dispo_observacion))
                lblObservacion = "<table class='table table-condensed'>" +
                                 "   <tr>" +
                                 "       <td style='vertical-align:top'><b>Observaciones:</b></td>" +
                                 "   </tr>" +
                                "   <tr>" +                                
                                "       <td>" + dispo_observacion + "</td>" +                               
                                "   </tr>" +
                                "   <tr>" +
                                "       <td> " + obsAnt + "</td>" +
                                "   </tr>" +
                                "</table>";

            string lblArticulos = "";
            if(articulo2 != null)
            {
                lblArticulos += "<table class='table table-condensed'>" + 
                                "<tr>" +
                                " <td> " + articulo2 + "</td>" +
                                "</tr>" +
                                "<tr>" +
                                " <td> " + articulo3 + "</td>" +
                                "</tr>"+
                                "<tr>" +
                                " <td> " + articulo4 + "</td>" +
                                "</tr>" +
                                "</table>";
            }

            disposicion_html = disposicion_html.Replace("@lblConsiderando", considerando);
            disposicion_html = disposicion_html.Replace("@lblArticulo", articulo);
            disposicion_html = disposicion_html.Replace("@lblNroExpediente", lblNroExpediente);
            disposicion_html = disposicion_html.Replace("@lblNroSolicitud", lblNroSolicitud);
            disposicion_html = disposicion_html.Replace("@lblDia", lblDia);
            disposicion_html = disposicion_html.Replace("@lblSeccion", lblSeccion);
            disposicion_html = disposicion_html.Replace("@lblManzana", lblManzana);
            disposicion_html = disposicion_html.Replace("@lblParcela", lblParcela);
            disposicion_html = disposicion_html.Replace("@lblPartidaMatriz", lblPartidaMatriz);
            disposicion_html = disposicion_html.Replace("@lblDomicilio", lblDomicilio);
            disposicion_html = disposicion_html.Replace("@lblPlantashabilitar", lblPlantashabilitar);
            disposicion_html = disposicion_html.Replace("@lblZonaDeclarada", zonMix);
            disposicion_html = disposicion_html.Replace("@lblCantOperarios", lblCantOperarios);
            disposicion_html = disposicion_html.Replace("@lblSuperficieTotal", lblSuperficieTotal);
            disposicion_html = disposicion_html.Replace("@lblCalificar", lblCalificar);
            disposicion_html = disposicion_html.Replace("@lblObservacion", lblObservacion);

            disposicion_html = disposicion_html.Replace("@lblTirulares", lblTirulares);
            disposicion_html = disposicion_html.Replace("@lblPH", lblPH);
            disposicion_html = disposicion_html.Replace("@lblRubros", lblRubros);
            disposicion_html = disposicion_html.Replace("@lblSubRubros", lblSubRubros);
            disposicion_html = disposicion_html.Replace("@lblDepositos", lblDepositos);
            disposicion_html = disposicion_html.Replace("@lblArtiFin", lblArticulos);

            db.Dispose();
            return disposicion_html;
        }
    }

}