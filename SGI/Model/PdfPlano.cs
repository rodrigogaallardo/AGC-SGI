using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SGI.Model
{
    public class PdfPlano
    {
        public Guid userid { get; set; }
        public string nro_expediente { get; set; }
        public int cant_documentos_adjuntados { get; set; }
        public Stream documento = null;

        public PdfPlano(Guid userid, string nro_expediemte)
        {
            this.userid = userid;
            //this.NombreSistema = Constants.ApplicationName;
            this.nro_expediente = nro_expediemte;
            this.documento = null;

            this.id_encomienda = 0;
            this.id_solicitud = 0;
            this.id_tramitetarea = 0;
            this.cant_documentos_adjuntados = 0;

            IniciarEntity();
            IniciarEntityFiles();
        }

        public void Dispose()
        {
            FinalizarEntity();
            FinalizarEntityFiles();
            FinalizarDocumento();

        }

        private void FinalizarDocumento()
        {
            try
            {
                if (this.documento != null)
                {
                    this.documento.Close();
                    this.documento.Dispose();
                }
            }
            catch { }
            this.documento = null;
        }

        #region entity

        private DGHP_Entities db = null;
        private AGC_FilesEntities dbFiles = null;

        private void IniciarEntity()
        {
               if (this.db == null)
                this.db = new DGHP_Entities();
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

        public class PlanosException : Exception
        {
            public PlanosException(string mensaje)
                : base(mensaje, new Exception()) { }
        }

        private int id_encomienda;
        private int id_solicitud;
        private int id_tramitetarea;

        private App_Data.dsImpresionPlanos GenerarDataSetPlanos()
        {

            App_Data.dsImpresionPlanos ds = new App_Data.dsImpresionPlanos();

            DataRow row;

            //-- -------------------------
            //-- armar plantas a habilitar
            //-- -------------------------
            var plantasHabilitar =
                (
                    from tipsec in db.TipoSector
                    join encplan in db.Encomienda_Plantas on tipsec.Id equals encplan.id_tiposector
                    where encplan.id_encomienda == this.id_encomienda
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

            //int[] tareas = new int[2] { (int)SGI.Constants.ENG_Tareas.SSP_Calificar, (int)SGI.Constants.ENG_Tareas.SCP_Calificar };
            //List<TramiteTareaAnteriores> list_tramite_tarea = TramiteTareaAnteriores.BuscarUltimoTramiteTareaPorTarea(id_solicitud, id_tramitetarea - 1, tareas);

            //int id_tramitetarea_calif = list_tramite_tarea[0].id_tramitetarea;
            //var q_calif =
            //    (
            //        from tt in db.SGI_Tramites_Tareas
            //        join up in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals up.userid
            //        where tt.id_tramitetarea == id_tramitetarea_calif
            //        select new
            //        {
            //            up.Nombres,
            //            up.Apellido
            //        }

            //    ).FirstOrDefault();


            //-- -------------------------
            //-- Datos encomienda, tabla 0
            //-- -------------------------

            DataTable dtDatosPlano = ds.Tables["datos_plano"];
            row = dtDatosPlano.NewRow();
            row["id_solicitud"] = this.id_solicitud;
            row["nro_expediente"] = this.nro_expediente;
            //row["calificador"] = q_calif.Apellido + ", " + q_calif.Nombres;

            dtDatosPlano.Rows.Add(row);

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
                    where enc.id_encomienda == this.id_encomienda
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
                row["ObservacionesRubros"] = "";
                row["id_encomienda_anterior"] = item.id_encomienda_anterior;
                row["NroExpediente"] = item.NroExpediente;

                dtEncomienda.Rows.Add(row);
            }

            var query_ubi =
                (
                    from enc in db.Encomienda
                    join encubic in db.Encomienda_Ubicaciones on enc.id_encomienda equals encubic.id_encomienda
                    join mat in db.Ubicaciones on encubic.id_ubicacion equals mat.id_ubicacion
                    join zon1 in db.Zonas_Planeamiento on encubic.id_zonaplaneamiento equals zon1.id_zonaplaneamiento
                    where enc.id_encomienda == this.id_encomienda
                    orderby encubic.id_encomiendaubicacion
                    select new
                    {
                        encubic.id_encomiendaubicacion,
                        enc.id_encomienda,
                        encubic.id_ubicacion,
                        mat.Seccion,
                        mat.Manzana,
                        mat.Parcela,
                        NroPartidaMatriz = mat.NroPartidaMatriz,
                        encubic.local_subtipoubicacion,
                        ZonaParcela = zon1.CodZonaPla,
                        // dbo.Encomienda_Solicitud_DireccionesPartidasPlancheta(enc.id_encomienda,encubic.id_ubicacion) 
                        DeptoLocal = encubic.deptoLocal_encomiendaubicacion
                    }
                ).ToList();

            DataTable dtUbicaciones = ds.Tables["Ubicaciones"];


            foreach (var item in query_ubi)
            {
                row = dtUbicaciones.NewRow();

                string sql = "select dbo.Encomienda_Solicitud_DireccionesPartidasPlancheta(" + item.id_encomienda + "," + item.id_ubicacion + ")";
                string direccion = db.Database.SqlQuery<string>(sql).FirstOrDefault();

                row["id_encomiendaubicacion"] = item.id_encomiendaubicacion;
                row["id_encomienda"] = item.id_encomienda;
                row["id_ubicacion"] = item.id_ubicacion;
                row["Seccion"] = item.Seccion;
                row["Manzana"] = item.Manzana;
                row["Parcela"] = item.Parcela;
                row["NroPartidaMatriz"] = item.NroPartidaMatriz;
                row["local_subtipoubicacion"] = item.local_subtipoubicacion;
                row["ZonaParcela"] = item.ZonaParcela;
                row["Direcciones"] = direccion;
                row["DeptoLocal"] = item.DeptoLocal;

                dtUbicaciones.Rows.Add(row);

            }


            // Ubicaciones_PropiedadHorizontal

            var query_ph =
                (
                    from encubic in db.Encomienda_Ubicaciones
                    join encphor in db.Encomienda_Ubicaciones_PropiedadHorizontal on encubic.id_encomiendaubicacion equals encphor.id_encomiendaubicacion
                    join phor in db.Ubicaciones_PropiedadHorizontal on encphor.id_propiedadhorizontal equals phor.id_propiedadhorizontal
                    where encubic.id_encomienda == this.id_encomienda
                    orderby encubic.id_encomiendaubicacion
                    select new
                    {
                        encubic.id_encomiendaubicacion,
                        encubic.id_ubicacion,
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
                row["NroPartidaHorizontal"] = item.NroPartidaHorizontal;
                row["Piso"] = item.Piso;
                row["Depto"] = item.Depto;

                dtPH.Rows.Add(row);
            }


            // Puertas

            var query_puerta =
                (
                    from encubic in db.Encomienda_Ubicaciones
                    join encpuer in db.Encomienda_Ubicaciones_Puertas on encubic.id_encomiendaubicacion equals encpuer.id_encomiendaubicacion
                    where encubic.id_encomienda == this.id_encomienda
                    orderby encubic.id_encomiendaubicacion
                    select new
                    {
                        encubic.id_encomiendaubicacion,
                        encpuer.id_encomiendapuerta,
                        encubic.id_encomienda,
                        encubic.id_ubicacion,
                        Calle = encpuer.nombre_calle,
                        encpuer.NroPuerta
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
                    where enc.id_encomienda == this.id_encomienda
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
                    from pj in db.Encomienda_Firmantes_PersonasJuridicas
                    join titpj in db.Encomienda_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                    join tcl in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tcl.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pj.id_encomienda == this.id_encomienda
                    select new
                    {
                        id_firmante = pj.id_firmante_pj,
                        pj.id_encomienda,
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
                    from pf in db.Encomienda_Firmantes_PersonasFisicas
                    join titpf in db.Encomienda_Titulares_PersonasFisicas on pf.id_personafisica equals titpf.id_personafisica
                    join tcl in db.TiposDeCaracterLegal on pf.id_tipocaracter equals tcl.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where pf.id_encomienda == this.id_encomienda
                    select new
                    {
                        id_firmante = pf.id_firmante_pf,
                        pf.id_encomienda,
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

            foreach (var item in query_Firmantes2)
            {
                row = dtFirmantes.NewRow();

                row["id_firmante"] = item.id_firmante;
                row["id_encomienda"] = item.id_encomienda;
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
                    from pj in db.Encomienda_Titulares_PersonasJuridicas
                    join tsoc in db.TipoSociedad on pj.Id_TipoSociedad equals tsoc.Id
                    join tipoiibb in db.TiposDeIngresosBrutos on pj.id_tipoiibb equals tipoiibb.id_tipoiibb
                    where pj.id_encomienda == this.id_encomienda
                    select new
                    {
                        id_persona = pj.id_personajuridica,
                        pj.id_encomienda,
                        TipoPersona = "PJ",
                        RazonSocial = pj.Razon_Social,
                        TipoSociedad = tsoc.Descripcion,
                        TipoIIBB = tipoiibb.nom_tipoiibb,
                        NroIIBB = pj.Nro_IIBB,
                        pj.CUIT,
                        MuestraEnTitulares = 1,
                        MuestraEnPlancheta = (pj.Id_TipoSociedad == 2) ? false : true
                    }
                ).ToList();


            DataTable dtTitulares = ds.Tables["Titulares"];


            foreach (var item in query_Titulares1)
            {
                row = dtTitulares.NewRow();

                row["id_persona"] = item.id_persona;
                row["id_encomienda"] = item.id_encomienda;
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
                    from pj in db.Encomienda_Firmantes_PersonasJuridicas
                    join titpj in db.Encomienda_Titulares_PersonasJuridicas on pj.id_personajuridica equals titpj.id_personajuridica
                    join tcl in db.TiposDeCaracterLegal on pj.id_tipocaracter equals tcl.id_tipocaracter
                    join tdoc in db.TipoDocumentoPersonal on pj.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    where titpj.id_encomienda == this.id_encomienda && titpj.Id_TipoSociedad == 2
                    select new
                    {
                        id_persona = pj.id_firmante_pj,
                        titpj.id_encomienda,
                        TipoPersona = "PF",
                        Apellido = pj.Apellido,
                        Nombres = pj.Nombres,
                        TipoDoc = tdoc.Nombre,
                        NroDoc = pj.Nro_Documento,
                        MuestraEnTitulares = 0,
                        MuestraEnPlancheta = 1
                    }
                ).ToList();

            foreach (var item in query_Titulares2)
            {
                row = dtTitulares.NewRow();

                row["id_persona"] = item.id_persona;
                row["id_encomienda"] = item.id_encomienda;
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
                    from pf in db.Encomienda_Titulares_PersonasFisicas
                    join tdoc in db.TipoDocumentoPersonal on pf.id_tipodoc_personal equals tdoc.TipoDocumentoPersonalId
                    join tipoiibb in db.TiposDeIngresosBrutos on pf.id_tipoiibb equals tipoiibb.id_tipoiibb
                    where pf.id_encomienda == this.id_encomienda
                    select new
                    {
                        id_persona = pf.id_personafisica,
                        pf.id_encomienda,
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
                row["id_encomienda"] = item.id_encomienda;
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
                    where enc.id_encomienda == this.id_encomienda
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
                    from enc in db.Encomienda
                    join dl in db.Encomienda_DatosLocal on enc.id_encomienda equals dl.id_encomienda
                    where enc.id_encomienda == this.id_encomienda
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

                row["sobrecarga_requisitos_opcion"] = item.sobrecarga_requisitos_opcion;
                row["sobrecarga_art813_inciso"] = item.sobrecarga_art813_inciso;
                row["sobrecarga_art813_item"] = item.sobrecarga_art813_item;
                row["cantidad_operarios_dl"] = item.cantidad_operarios_dl;

                dtDatosLocal.Rows.Add(row);

            }

            return ds;
        }

        private void generar_pdf_base(ref Stream msPdfPlano)
        {
            App_Data.dsImpresionPlanos dsPlanos = GenerarDataSetPlanos();

            //dsPlanos.WriteXmlSchema("C:\\Users\\cnieto.AR.MOST\\Desktop\\borrar\\dsImpresionPlanos.xsd");

            CrystalDecisions.Web.CrystalReportSource CrystalReportSource1 = new CrystalDecisions.Web.CrystalReportSource();
            try
            {
                CrystalReportSource1.EnableCaching = false;
                CrystalReportSource1.EnableViewState = false;
                CrystalReportSource1.Report.FileName = "~/Reportes/Planos.rpt";
                CrystalReportSource1.ReportDocument.SetDataSource(dsPlanos);
                msPdfPlano = CrystalReportSource1.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                //se liberan recursos porque el crystal esta configurado para 65 instancias en registry
                CrystalReportSource1.ReportDocument.Close();

                if (CrystalReportSource1 != null)
                {
                    CrystalReportSource1.ReportDocument.Close();
                    CrystalReportSource1.ReportDocument.Dispose();
                    CrystalReportSource1.Dispose();
                }

            }
            catch (Exception ex)
            {
                LogError.Write(ex, "error al generar pdf base para planos adjuntos-id_solicitud:"+ this.id_solicitud);
                try
                {
                    if (CrystalReportSource1 != null)
                    {
                        CrystalReportSource1.ReportDocument.Close();
                        CrystalReportSource1.ReportDocument.Dispose();
                        CrystalReportSource1.Dispose();
                    }
                }
                catch { }

                msPdfPlano = null;
                throw ex;
            }

        }

        private void adjuntar_plano(ref Stream msPdfPlano_origen)
        {

            // Obtiene las encomiendas anteriores si las hay
            List<Int32> lstEncomiendas = (from se in db.SSIT_Solicitudes_Encomienda
                                          where se.id_solicitud == this.id_solicitud
                                          select se.id_encomienda).ToList();
            // agrega a la lista la encomienda actual
            lstEncomiendas.Insert(0, this.id_encomienda);

            var lstPlanos =
                (
                    from plano in db.Encomienda_Planos
                    where lstEncomiendas.Contains(plano.id_encomienda)
                    select new
                    {
                        id = plano.id_encomienda_plano,
                        id_encomienda = plano.id_encomienda,
                        detalle = plano.detalle,
                        nombre_archivo = plano.nombre_archivo,
                        id_file= plano.id_file
                    }
                ).ToList();

            PdfReader reader = null;
            PdfStamper stamp = null;
            string descrip_arch = "";
            string nombre_arch = "";

            try
            {

                this.documento = null;
                reader = new PdfReader(Functions.StreamToArray(msPdfPlano_origen));

                stamp = new PdfStamper(reader, this.documento);

                foreach (var item in lstPlanos)
                {
                    nombre_arch = item.nombre_archivo;
                    descrip_arch = string.IsNullOrEmpty(item.detalle) ? item.nombre_archivo : item.detalle + "-" + item.id_encomienda;

                    var f =
                        (
                        from file in dbFiles.Files
                        where file.id_file == item.id_file
                        select new
                        {
                            file.id_file,
                            file.content_file
                        }).FirstOrDefault();

                    PdfFileSpecification pfs = PdfFileSpecification.FileEmbedded(stamp.Writer, "", nombre_arch, f.content_file);
                    stamp.AddFileAttachment(descrip_arch, pfs);
                }
                
                stamp.Close();
                reader.Close();

                
                this.cant_documentos_adjuntados = lstPlanos.Count;
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "error al generar pdf con planos adjuntos-id_solicitud:"+ this.id_solicitud);
                if (stamp != null) {
                    stamp.Close();
                    stamp.Dispose();
                }
                if (reader != null) {
                    reader.Close();
                    reader.Dispose();
                }
                
                throw ex;
            }

        }

        private void GenerarPdf_Planos()
        {

            if ( string.IsNullOrEmpty(this.nro_expediente) )
            {
                throw new PlanosException("Debe enviar el nro. de expediente.");
            }

            // generar pdf usando crystal
            Stream msPdfPlano_origen = null;
            //MemoryStream msPdfPlano_salida = null;
            bool existen_adjuntos = false;

            try
            {
                generar_pdf_base(ref msPdfPlano_origen);

                adjuntar_plano(ref msPdfPlano_origen);

                if (this.cant_documentos_adjuntados == 0)
                {
                    FinalizarDocumento();
                }

                if ( msPdfPlano_origen != null ) 
                    msPdfPlano_origen.Close();

            }
            catch (Exception ex)
            {
                if (msPdfPlano_origen != null)
                {
                    msPdfPlano_origen.Close();
                    msPdfPlano_origen.Dispose();
                }
                FinalizarDocumento();
                throw ex;
            }


        }

        public void GenerarPdf(int id_solicitud)
        {
            this.id_solicitud = id_solicitud;
            FinalizarDocumento();
            this.cant_documentos_adjuntados = 0;

            var qSol =
                (
                    from sol in db.SSIT_Solicitudes
                    join tt_hab in db.SGI_Tramites_Tareas_HAB on sol.id_solicitud equals tt_hab.id_solicitud
                    join exp in db.SGI_Tarea_Generar_Expediente_Procesos on tt_hab.id_tramitetarea equals exp.id_tramitetarea
                    orderby tt_hab.id_tramitetarea descending
                    where sol.id_solicitud == id_solicitud
                    select new
                    {
                        id_tramitetarea = tt_hab.id_tramitetarea,
                        id_paquete = exp.id_paquete,
                        tt_hab.id_solicitud
                    }
                ).FirstOrDefault();

            if (qSol != null)
            {
                var enc = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();

                this.id_encomienda = enc.id_encomienda;
                this.id_tramitetarea = qSol.id_tramitetarea;

                GenerarPdf_Planos();
            }

        }

    }

}