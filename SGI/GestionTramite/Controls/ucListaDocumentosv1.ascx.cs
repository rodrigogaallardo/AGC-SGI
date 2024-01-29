using ExternalService.Class;
using SGI.Model;
using SGI.Webservices.ws_interface_AGC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public class itemDocumentov1
    {
        internal int id_tipoDocReq;
        public int id_doc_adj { get; set; }
        public string nombre { get; set; }
        public int id_file { get; set; }
        public int id_solicitud { get; set; }
        public string url { get; set; }
        public DateTime Fecha { get; set; }
        public string UserName { get; set; }
        public int id_tipodocsis { get; set; }
        public string origen { get; set; }
        public string numero_Gedo { get; set; }
    }

    public class itemDocumentoModulo
    {
        internal int id_tipoDocReq;
        public int id_docadjunto { get; set; }
        public int id_doc_adj { get; set; }
        public string nombre_archivo { get; set; }
        public int id_file { get; set; }
        public int id_solicitud { get; set; }
        public string url { get; set; }
        public DateTime Fecha { get; set; }
        public string UserName { get; set; }
        public int? id_tipodocsis { get; set; }
        public string origen { get; set; }
        public string numero_Gedo { get; set; }
        public string nombre_tdocreq { get; set; }
        public string tdocreq_detalle { get; set; }
        public bool generadoxSistema { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.Guid CreateUser { get; set; }
        public Nullable<System.DateTime> fechaPresentado { get; set; }
        public bool ExcluirSubidaSADE { get; set; }
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual TiposDeDocumentosRequeridos TiposDeDocumentosRequeridos { get; set; }
        public virtual TiposDeDocumentosSistema TiposDeDocumentosSistema { get; set; }
    }

    public partial class ucListaDocumentosv1 : System.Web.UI.UserControl
    {
        public delegate void EventHandlerEliminar(object sender, ucListaDocumentosv1EventsArgs e);
        public event EventHandlerEliminar EliminarListaDocumentosv1Click;

        private DGHP_Entities db;
        private AGC_FilesEntities dbFiles;

        public int id_tipodocsis
        {
            get
            {
                return (ViewState["_id_tipodocsis"] != null ? Convert.ToInt32(ViewState["_id_tipodocsis"]) : 0);
            }
            set
            {
                ViewState["_id_tipodocsis"] = value.ToString();
            }
        }

        public bool visibleEliminar
        {
            get
            {
                return (ViewState["_visibleEliminar"] != null ? Convert.ToBoolean(ViewState["_visibleEliminar"]) : false);
            }
            set
            {
                ViewState["_visibleEliminar"] = value.ToString();
            }
        }

        public async Task LoadData(int id_solicitud)
        {
            await LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud);
        }

        public async Task LoadData(int id_grupotramite, int id_solicitud)
        {
            await LoadData(id_grupotramite, id_solicitud, false, 0);
        }

        public async Task LoadData(SSIT_Solicitudes solicitud, DateTime? ultimaPresentacion)
        {
            using (var db = new DGHP_Entities())
            {
                db.Database.CommandTimeout = 300;
                IEnumerable<int> idEncomiendasPresentadas = null;

                if (ultimaPresentacion != null)
                {
                    idEncomiendasPresentadas  = (from rel in db.Encomienda_SSIT_Solicitudes
                                                 join enc in db.Encomienda on rel.id_encomienda equals enc.id_encomienda
                                                 join hist in db.Encomienda_HistorialEstados on enc.id_encomienda equals hist.id_encomienda
                                                 where rel.id_solicitud == solicitud.id_solicitud
                                                   && (enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo || enc.id_estado == (int)Constants.Encomienda_Estados.Vencida)
                                                    && hist.fecha_modificacion <= ultimaPresentacion
                                                 orderby enc.id_encomienda descending
                                                 select enc.id_encomienda);
                }

                using (var dbFiles = new AGC_FilesEntities())
                {
                    dbFiles.Database.CommandTimeout = 300;
                    List<TipoTramiteCertificados> lstTipoTramiteCertificados = dbFiles.TipoTramiteCertificados.ToList();

                    // Obtiene la descripcion de los certificados
                    string nom_caa = lstTipoTramiteCertificados.Where(y => y.TipoTramite == (int)Constants.TipoTramiteCertificados.CAA).Select(x => x.Descripcion).FirstOrDefault();
                    string nom_acta_notarial = lstTipoTramiteCertificados.Where(y => y.TipoTramite == (int)Constants.TipoTramiteCertificados.ActaNotarial).Select(x => x.Descripcion).FirstOrDefault();

                    var q = (       //Encomienda_DocumentosAdjuntos
                                    from encdoc in db.Encomienda_DocumentosAdjuntos
                                    join prof in db.Profesional on encdoc.CreateUser equals prof.UserId into us
                                    from prof in us.DefaultIfEmpty()
                                    where idEncomiendasPresentadas.Contains(encdoc.id_encomienda)
                                    select new itemDocumentov1
                                    {
                                        id_doc_adj = encdoc.id_docadjunto,
                                        nombre = (encdoc.id_tdocreq != 0 ? encdoc.TiposDeDocumentosRequeridos.nombre_tdocreq : encdoc.TiposDeDocumentosSistema.nombre_tipodocsis) + "-" + encdoc.id_encomienda,
                                        id_file = encdoc.id_file,
                                        id_solicitud = encdoc.id_encomienda,
                                        url = null,
                                        Fecha = encdoc.CreateDate,
                                        UserName = prof != null ? prof.Apellido + ", " + prof.Nombre : "",
                                        id_tipodocsis = encdoc.id_tipodocsis,
                                        origen = "Encomienda_DocumentosAdjuntos",
                                        numero_Gedo = ""

                                    }).Union(

                                    //Acta notarial
                                    from acta in db.wsEscribanos_ActaNotarial
                                    where idEncomiendasPresentadas.Contains(acta.id_encomienda) && !acta.anulada && acta.id_file != null
                                    select new itemDocumentov1
                                    {
                                        id_doc_adj = acta.id_actanotarial,
                                        nombre = nom_acta_notarial + "-" + acta.id_actanotarial,
                                        id_file = acta.id_file.Value,
                                        id_solicitud = acta.id_encomienda,
                                        url = null,
                                        Fecha = acta.CreateDate,
                                        UserName = "",
                                        id_tipodocsis = 0,
                                        origen = "wsEscribanos_ActaNotarial",
                                        numero_Gedo = ""
                                    }).Union(

                                    //SSIT_Documentos adjuntos
                                    from doc in db.SSIT_DocumentosAdjuntos
                                    join user in db.Usuario on doc.CreateUser equals user.UserId into us
                                    from u in us.DefaultIfEmpty()
                                    join prof in db.SGI_Profiles on doc.CreateUser equals prof.userid into pr
                                    from p in pr.DefaultIfEmpty()
                                    where doc.id_solicitud == solicitud.id_solicitud
                                    // Se agregan los documentos que no son subudos por el usuaqio id_tdocreq = 0, ya que sino no ve la carátula y la disposición
                                    && (doc.fechaPresentado.HasValue || (doc.id_tdocreq == (int)Constants.TiposDeDocumentosRequeridos.Sin_Especificar))
                                    select new itemDocumentov1
                                    {
                                        id_doc_adj = doc.id_docadjunto,
                                        nombre = doc.tdocreq_detalle != null && doc.tdocreq_detalle != "" ?
                                            doc.tdocreq_detalle : (doc.id_tdocreq != 0 ? doc.TiposDeDocumentosRequeridos.nombre_tdocreq : doc.TiposDeDocumentosSistema.nombre_tipodocsis),
                                        id_file = doc.id_file,
                                        id_solicitud = doc.id_solicitud,
                                        url = null,
                                        Fecha = doc.CreateDate,
                                        UserName = (u != null ? u.Apellido + ", " + u.Nombre : (p != null ? p.Apellido + ", " + p.Nombres : "")),
                                        id_tipodocsis = doc.id_tipodocsis,
                                        origen = "SSIT_DocumentosAdjuntos",
                                        numero_Gedo = ""

                                    }).Union(

                                    //SGI_Tarea_Documentos_Adjuntos
                                    from docadj in db.SGI_Tarea_Documentos_Adjuntos
                                    join tt_hab in db.SGI_Tramites_Tareas_HAB on docadj.id_tramitetarea equals tt_hab.id_tramitetarea
                                    join user in db.SGI_Profiles on docadj.CreateUser equals user.userid into us
                                    from u in us.DefaultIfEmpty()
                                    where tt_hab.id_solicitud == solicitud.id_solicitud
                                    select new itemDocumentov1
                                    {
                                        id_doc_adj = docadj.id_doc_adj,
                                        nombre = docadj.TiposDeDocumentosRequeridos.nombre_tdocreq,
                                        id_file = docadj.id_file,
                                        id_solicitud = solicitud.id_solicitud,
                                        url = null,
                                        Fecha = docadj.CreateDate,
                                        UserName = u != null ? u.Apellido + ", " + u.Nombres : "",
                                        id_tipodocsis = 0,
                                        origen = "SGI_Tarea_Documentos_Adjuntos",
                                        numero_Gedo = ""
                                    }).Union(
                        
                                    //Planos
                                    from encdoc in db.Encomienda_Planos
                                    join prof in db.Profesional on encdoc.CreateUser equals prof.UserId.ToString() into us
                                    from prof in us.DefaultIfEmpty()
                                    where idEncomiendasPresentadas.Contains(encdoc.id_encomienda) 
                                    select new itemDocumentov1
                                    {
                                        id_doc_adj = encdoc.id_encomienda_plano,
                                        nombre = (encdoc.TiposDePlanos.requiere_detalle == true && !string.IsNullOrEmpty(encdoc.detalle) ? encdoc.detalle : encdoc.TiposDePlanos.nombre) + "-" + encdoc.id_encomienda,
                                        id_file = encdoc.id_file,
                                        id_solicitud = encdoc.id_encomienda,
                                        url = null,
                                        Fecha = encdoc.CreateDate,
                                        UserName = prof != null ? prof.Apellido + ", " + prof.Nombre : "",
                                        id_tipodocsis = 0,
                                        origen = "Encomienda_Planos",
                                        numero_Gedo = ""
                                    }).Union(
                        
                                        //SGI_LIZA_Procesos
                                        from p in db.SGI_LIZA_Procesos
                                        join tt in db.SGI_Tramites_Tareas_HAB on p.id_tramitetarea equals tt.id_tramitetarea
                                        join prof in db.Profesional on p.CreateUser equals prof.UserId
                                        where tt.id_solicitud == solicitud.id_solicitud && p.realizado == true 
                                        select new itemDocumentov1
                                        {
                                            id_doc_adj = p.id_liza_proceso,
                                            nombre = p.descripcion,
                                            id_file = p.id_file.Value,
                                            id_solicitud = tt.id_solicitud,
                                            url = null,
                                            Fecha = p.CreateDate,
                                            UserName = prof.Apellido + ", " + prof.Nombre,
                                            id_tipodocsis = 0,
                                            origen = "SGI_LIZA_Procesos",
                                            numero_Gedo = ""

                                        }).Union(
                        
                                        //SGI_Tarea_Calificar_ObsDocs
                                        from o in db.SGI_Tarea_Calificar_ObsDocs
                                        join g in db.SGI_Tarea_Calificar_ObsGrupo on o.id_ObsGrupo equals g.id_ObsGrupo
                                        join th in db.SGI_Tramites_Tareas_HAB on g.id_tramitetarea equals th.id_tramitetarea
                                        join sol in db.SSIT_Solicitudes on th.id_solicitud equals sol.id_solicitud
                                        join file in db.Files on o.id_file equals file.id_file
                                        join user in db.Usuario on sol.CreateUser equals user.UserId
                                        where th.id_solicitud == solicitud.id_solicitud && o.id_file != null && o.CreateDate <= ultimaPresentacion
                                        select new itemDocumentov1
                                        {
                                            id_doc_adj = o.id_ObsDocs,
                                            nombre = "Documento_Observacion" + (o.id_file != null ? o.id_file.ToString() : (o.id_certificado != 0 ? o.id_certificado.ToString() : "")) + ".pdf",
                                            id_file = o.id_file.Value,
                                            id_solicitud = th.id_solicitud,
                                            url = null,
                                            Fecha = file.CreateDate,
                                            UserName = user.Apellido + ", " + user.Nombre,
                                            id_tipodocsis = 0,
                                            origen = "SGI_Tarea_Calificar_ObsDocs",
                                            numero_Gedo = ""

                                        });
                    var archivos = q.ToList();

                    foreach (var arc in archivos)
                    {
                        if (arc.url == null)
                            arc.url = string.Format("~/GetPDFFiles/{0}", Functions.ConvertToBase64(arc.id_file.ToString()));
                    }


                    // Llena los CAAs de acuerdo a las encomiendas vinculadas a la solicitud.
                    // ---------------------------------------------------------------------
                    //ws_Interface_AGC servicio = new ws_Interface_AGC();
                    //SGI.Webservices.ws_interface_AGC.wsResultado ws_resultado_CAA = new SGI.Webservices.ws_interface_AGC.wsResultado();

                    //servicio.Url = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC");
                    //string username_servicio = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.User");
                    //string password_servicio = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.Password");
                    //DtoCAA[] l = servicio.Get_CAAs_by_Encomiendas(username_servicio, password_servicio, idEncomiendasPresentadas.ToArray(), ref ws_resultado_CAA);
                    List<GetCAAsByEncomiendasResponse> l = await GetCAAsByEncomiendas(idEncomiendasPresentadas.ToArray());
                    if(l != null && l.Count > 0)
                    {
                        var List_CAA = l.ToList().Where(x => x.id_estado != (int)Constants.CAA_Estados.Anulado && x.certificado != null);
                        foreach (var caa in List_CAA)
                        {
                            if (caa.createDate <= ultimaPresentacion)
                            {
                                var item = new itemDocumentov1
                                {
                                    nombre = caa.tipotramite + "-" + caa.id_solicitud,
                                    id_file = caa.certificado.idFile,
                                    id_solicitud = caa.id_solicitud,
                                    Fecha = caa.createDate,
                                    UserName = ""
                                };
                                item.url = string.Format("~/GetPDFFiles/{0}", Functions.ConvertToBase64(item.id_file.ToString()));
                                archivos.Add(item);
                            }
                        }
                    }
                    


                    //Agrego el Nº GEDO si es que existe para cada documento.
                    foreach (var item in archivos)
                    {
                        item.numero_Gedo = LoadNumeroGedo(item.id_file, solicitud.id_solicitud);

                        if ((item.id_tipodocsis == 14 || item.id_tipodocsis == 15) && (item.numero_Gedo == ""))
                        {
                            item.numero_Gedo = "---";
                        }
                    }


                    grdDocumentosAdjuntos.Columns[5].Visible = visibleEliminar;
                    grdDocumentosAdjuntos.DataSource = archivos.OrderBy(x => x.Fecha).ToList();
                    grdDocumentosAdjuntos.DataBind();
                }
            }
        }

        private string LoadNumeroGedo(int id_file, int id_solicitud)
        {
            string numeroGedo = "";
            try
            {
                using (var db = new DGHP_Entities())
                {
                    db.Database.CommandTimeout = 300;
                    var SgiSadeProceso = (from ssp in db.SGI_SADE_Procesos
                               join tth in db.SGI_Tramites_Tareas_HAB on ssp.id_tramitetarea equals tth.id_tramitetarea
                               where tth.id_solicitud == id_solicitud && ssp.id_file == id_file
                               select ssp).Union(
                               from ssp in db.SGI_SADE_Procesos
                               join ttt in db.SGI_Tramites_Tareas_TRANSF on ssp.id_tramitetarea equals ttt.id_tramitetarea
                               where ttt.id_solicitud == id_solicitud && ssp.id_file == id_file
                               select ssp).FirstOrDefault();
                    
                    if (SgiSadeProceso != null)
                        numeroGedo = SgiSadeProceso.resultado_ee?.ToString();
                    else
                    {
                        numeroGedo = "";
                        
                        using (var ee = new EE_Entities())
                        {
                            var documento = (from documentos in ee.wsEE_Documentos
                                             where documentos.id_file == id_file
                                             select documentos).FirstOrDefault();
                            if(documento != null)
                                numeroGedo = documento.numeroGEDO;
                        }
                        
                    }
                        
                }
            }
            catch (Exception)
            {
                numeroGedo = "";
            }
            return numeroGedo;
        }

        public async void LoadData(Transf_Solicitudes solicitud, DateTime? ultimaPresentacion)
        {
            using (var db = new DGHP_Entities())
            {
                IQueryable<int> idEncomiendasPresentadas = null;
                db.Database.CommandTimeout = 300;
                if (ultimaPresentacion != null)
                {

                    idEncomiendasPresentadas = (from rel in db.Encomienda_Transf_Solicitudes
                                                join enc in db.Encomienda on rel.id_encomienda equals enc.id_encomienda
                                                join hist in db.Encomienda_HistorialEstados on enc.id_encomienda equals hist.id_encomienda
                                                where rel.id_solicitud == solicitud.id_solicitud
                                                  && (enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo || enc.id_estado == (int)Constants.Encomienda_Estados.Vencida)
                                                   && hist.fecha_modificacion <= ultimaPresentacion
                                                orderby enc.id_encomienda descending
                                                select enc.id_encomienda);
                }

                using (var dbFiles = new AGC_FilesEntities())
                {
                    dbFiles.Database.CommandTimeout = 300;

                    var Trans = Functions.GetParametroNum("NroTransmisionReferencia");
                    if (solicitud.id_solicitud > Trans)
                    {
                        if (idEncomiendasPresentadas.Any())
                        {
                            var archivos = (//Encomienda_DocumentosAdjuntos
                                            from encdoc in db.Encomienda_DocumentosAdjuntos
                                            join prof in db.Profesional on encdoc.CreateUser equals prof.UserId into us
                                            from u in us.DefaultIfEmpty()
                                            where idEncomiendasPresentadas.Contains(encdoc.id_encomienda)
                                            select new itemDocumentov1
                                            {
                                                id_doc_adj = encdoc.id_docadjunto,
                                                nombre = (encdoc.id_tdocreq != 0 ? encdoc.TiposDeDocumentosRequeridos.nombre_tdocreq : encdoc.TiposDeDocumentosSistema.nombre_tipodocsis) + "-" + encdoc.id_encomienda,
                                                id_file = encdoc.id_file,
                                                id_solicitud = encdoc.id_encomienda,
                                                url = null,
                                                Fecha = encdoc.CreateDate,
                                                UserName = u != null ? u.Apellido + ", " + u.Nombre : "",
                                                id_tipodocsis = encdoc.id_tipodocsis
                                            }).Union(//SGI_Dcouemntos_adjuntos
                                            from docadj in db.SGI_Tarea_Documentos_Adjuntos
                                            join tt_hab in db.SGI_Tramites_Tareas_HAB on docadj.id_tramitetarea equals tt_hab.id_tramitetarea
                                            join user in db.SGI_Profiles on docadj.CreateUser equals user.userid into us
                                            from u in us.DefaultIfEmpty()
                                            where tt_hab.id_solicitud == solicitud.id_solicitud && docadj.CreateDate <= ultimaPresentacion
                                            select new itemDocumentov1
                                            {
                                                id_doc_adj = docadj.id_doc_adj,
                                                nombre = docadj.TiposDeDocumentosRequeridos.nombre_tdocreq,
                                                id_file = docadj.id_file,
                                                id_solicitud = solicitud.id_solicitud,
                                                url = null,
                                                Fecha = docadj.CreateDate,
                                                UserName = u != null ? u.Apellido + ", " + u.Nombres : "",
                                                id_tipodocsis = 0
                                            }).Union(//SGI_Tarea_Calificar_ObsDocs
                                                from o in db.SGI_Tarea_Calificar_ObsDocs
                                                join g in db.SGI_Tarea_Calificar_ObsGrupo on o.id_ObsGrupo equals g.id_ObsGrupo
                                                join th in db.SGI_Tramites_Tareas_TRANSF on g.id_tramitetarea equals th.id_tramitetarea
                                                join sol in db.Transf_Solicitudes on th.id_solicitud equals sol.id_solicitud
                                                join file in db.Files on o.id_file equals file.id_file
                                                join user in db.Usuario on sol.CreateUser equals user.UserId
                                                where th.id_solicitud == solicitud.id_solicitud && o.id_file != null && file.CreateDate <= ultimaPresentacion
                                                select new itemDocumentov1
                                                {
                                                    id_doc_adj = o.id_ObsDocs,
                                                    nombre = "Documento_Observacion" + (o.id_file != null ? o.id_file.ToString() : (o.id_certificado != 0 ? o.id_certificado.ToString() : "")) + ".pdf",
                                                    id_file = o.id_file.Value,
                                                    id_solicitud = th.id_solicitud,
                                                    url = null,
                                                    Fecha = file.CreateDate,
                                                    UserName = user.Apellido + ", " + user.Nombre,
                                                    id_tipodocsis = 0
                                                }).Union(//Encomienda_DocumentosAdjuntos
                                                from doc in db.Transf_DocumentosAdjuntos
                                                join user in db.Usuario on doc.CreateUser equals user.UserId into us
                                                from u in us.DefaultIfEmpty()
                                                join prof in db.SGI_Profiles on doc.CreateUser equals prof.userid into pr
                                                from p in pr.DefaultIfEmpty()
                                                where doc.id_solicitud == solicitud.id_solicitud //&& doc.CreateDate <= ultimaPresentacion
                                                select new itemDocumentov1
                                                {
                                                    id_doc_adj = doc.id_docadjunto,
                                                    nombre = doc.tdocreq_detalle != null && doc.tdocreq_detalle != "" ?
                                                            doc.tdocreq_detalle : (doc.id_tdocreq != 0 ? doc.TiposDeDocumentosRequeridos.nombre_tdocreq : doc.TiposDeDocumentosSistema.nombre_tipodocsis),
                                                    id_file = doc.id_file,
                                                    id_solicitud = doc.id_solicitud,
                                                    url = null,
                                                    Fecha = doc.CreateDate,
                                                    UserName = (u != null ? u.Apellido + ", " + u.Nombre : (p != null ? p.Apellido + ", " + p.Nombres : "")),
                                                    id_tipodocsis = 0
                                                }).ToList();

                            foreach (var arc in archivos)
                            {
                                if (arc.url == null)
                                    arc.url = string.Format("~/GetPDFFiles/{0}", Functions.ConvertToBase64(arc.id_file.ToString()));
                            }


                            // Llena los CAAs de acuerdo a las encomiendas vinculadas a la solicitud.
                            // ---------------------------------------------------------------------
                            //ws_Interface_AGC servicio = new ws_Interface_AGC();
                            //SGI.Webservices.ws_interface_AGC.wsResultado ws_resultado_CAA = new SGI.Webservices.ws_interface_AGC.wsResultado();

                            //servicio.Url = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC");
                            //string username_servicio = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.User");
                            //string password_servicio = Functions.GetParametroChar("SIPSA.Url.Webservice.ws_Interface_AGC.Password");
                            //DtoCAA[] l = servicio.Get_CAAs_by_Encomiendas(username_servicio, password_servicio, idEncomiendasPresentadas.ToArray(), ref ws_resultado_CAA);
                            List<GetCAAsByEncomiendasResponse> l = await GetCAAsByEncomiendas(idEncomiendasPresentadas.ToArray());
                            if(l != null && l.Count > 0)
                            {
                                var List_CAA = l.ToList().Where(x => x.id_estado != (int)Constants.CAA_Estados.Anulado && x.certificado != null);
                                foreach (var caa in List_CAA)
                                {
                                    if (caa.createDate <= ultimaPresentacion)
                                    {
                                        var item = new itemDocumentov1
                                        {
                                            nombre = caa.tipotramite + "-" + caa.id_solicitud,
                                            id_file = caa.certificado.idFile,
                                            id_solicitud = caa.id_solicitud,
                                            Fecha = caa.createDate,
                                            UserName = ""
                                        };
                                        item.url = string.Format("~/GetPDFFiles/{0}", Functions.ConvertToBase64(item.id_file.ToString()));
                                        archivos.Add(item);
                                    }
                                }
                            }
                            

                            //Agrego el Nº GEDO si es que existe para cada documento.
                            foreach (var item in archivos)
                            {
                                item.numero_Gedo = LoadNumeroGedo(item.id_file, solicitud.id_solicitud);
                            }

                            grdDocumentosAdjuntos.Columns[5].Visible = visibleEliminar;
                            grdDocumentosAdjuntos.DataSource = archivos.OrderBy(x => x.Fecha).ToList();
                            grdDocumentosAdjuntos.DataBind();
                        }
                    }
                    else
                    {
                        var archivos = (//Encomienda_DocumentosAdjuntos
                                    from doc in db.Transf_DocumentosAdjuntos
                                    join user in db.Usuario on doc.CreateUser equals user.UserId into us
                                    from u in us.DefaultIfEmpty()
                                    join prof in db.SGI_Profiles on doc.CreateUser equals prof.userid into pr
                                    from p in pr.DefaultIfEmpty()
                                    where doc.id_solicitud == solicitud.id_solicitud //&& doc.CreateDate <= ultimaPresentacion
                                    select new itemDocumentov1
                                    {
                                        nombre = doc.tdocreq_detalle != null && doc.tdocreq_detalle != "" ?
                                                doc.tdocreq_detalle : (doc.id_tdocreq != 0 ? doc.TiposDeDocumentosRequeridos.nombre_tdocreq : doc.TiposDeDocumentosSistema.nombre_tipodocsis),
                                        id_file = doc.id_file,
                                        id_solicitud = doc.id_solicitud,
                                        url = null,
                                        Fecha = doc.CreateDate,
                                        UserName = (u != null ? u.Apellido + ", " + u.Nombre : (p != null ? p.Apellido + ", " + p.Nombres : ""))
                                    }).Union(
                                    from doc in db.CPadron_DocumentosAdjuntos
                                    join sol in db.Transf_Solicitudes on doc.id_cpadron equals sol.id_cpadron
                                    join user in db.Usuario on doc.CreateUser equals user.UserId into us
                                    from u in us.DefaultIfEmpty()
                                    join prof in db.SGI_Profiles on doc.CreateUser equals prof.userid into pr
                                    from p in pr.DefaultIfEmpty()
                                    where sol.id_solicitud == solicitud.id_solicitud //&& doc.id_tipodocsis == (int)Constants.TiposDeDocumentosSistema.INFORMES_CPADRON
                                         //&& doc.CreateDate <= ultimaPresentacion
                                    select new itemDocumentov1
                                    {
                                        nombre = doc.id_tdocreq != 0 ? doc.TiposDeDocumentosRequeridos.nombre_tdocreq : doc.TiposDeDocumentosSistema.nombre_tipodocsis,
                                        id_file = doc.id_file,
                                        id_solicitud = doc.id_cpadron,
                                        url = null,
                                        Fecha = doc.CreateDate,
                                        UserName = (u != null ? u.Apellido + ", " + u.Nombre : (p != null ? p.Apellido + ", " + p.Nombres : ""))
                                    }).ToList();
                        foreach (var arc in archivos)
                        {
                            if (arc.url == null)
                                arc.url = string.Format("~/GetPDFFiles/{0}", Functions.ConvertToBase64(arc.id_file.ToString()));
                        }
                        
                        //Agrego el Nº GEDO si es que existe para cada documento.
                        foreach (var item in archivos)
                        {
                            item.numero_Gedo = LoadNumeroGedo(item.id_file, solicitud.id_solicitud);
                        }

                        grdDocumentosAdjuntos.Columns[5].Visible = visibleEliminar;
                        grdDocumentosAdjuntos.DataSource = archivos.OrderBy(x => x.Fecha).ToList();
                        grdDocumentosAdjuntos.DataBind();
                    }
                }
            }
        }

        public async Task LoadData(int id_grupotramite, int id_solicitud, bool visibleEliminar, int id_tipodocsis_a_eliminar)
        {
            this.visibleEliminar = visibleEliminar;
            this.id_tipodocsis = id_tipodocsis_a_eliminar;

            using (var db = new DGHP_Entities())
            {
                db.Database.CommandTimeout = 300;
                var estadosSolPres = db.TipoEstadoSolicitud.Where(e =>
                    e.Id == (int)Constants.Solicitud_Estados.Pendiente_de_Ingreso ||
                    e.Id == (int)Constants.Solicitud_Estados.En_trámite)
                    .Select(e => e.Nombre).ToList();

                switch (id_grupotramite)
                {
                    case (int)Constants.GruposDeTramite.HAB:
                        var sol = db.SSIT_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);

                        var ultimaSolicitudPresentada = sol?.SSIT_Solicitudes_HistorialEstados.Where(h =>
                            estadosSolPres.Contains(h.cod_estado_nuevo)).Select(h => h.fecha_modificacion).OrderByDescending(h => h).FirstOrDefault();

                        await LoadData(sol, ultimaSolicitudPresentada);
                        break;
                    case (int)Constants.GruposDeTramite.TR:
                        var trf = db.Transf_Solicitudes.FirstOrDefault(x => x.id_solicitud == id_solicitud);

                        var ultimaTransmisionPresentada = trf?.Transf_Solicitudes_HistorialEstados.Where(h =>
                            estadosSolPres.Contains(h.cod_estado_nuevo)).Select(h => h.fecha_modificacion).OrderByDescending(h => h).FirstOrDefault();

                        LoadData(trf, ultimaTransmisionPresentada);
                        break;
                    default:
                        var archivos = (//Encomienda_DocumentosAdjuntos
                                    from doc in db.CPadron_DocumentosAdjuntos
                                    join user in db.Usuario on doc.CreateUser equals user.UserId into us
                                    from u in us.DefaultIfEmpty()
                                    join prof in db.SGI_Profiles on doc.CreateUser equals prof.userid into pr
                                    from p in pr.DefaultIfEmpty()
                                    where doc.id_cpadron == id_solicitud
                                    select new itemDocumentov1
                                    {
                                        nombre = doc.id_tdocreq != 0 ? doc.TiposDeDocumentosRequeridos.nombre_tdocreq : doc.TiposDeDocumentosSistema.nombre_tipodocsis,
                                        id_file = doc.id_file,
                                        id_solicitud = doc.id_cpadron,
                                        url = null,
                                        Fecha = doc.CreateDate,
                                        UserName = (u != null ? u.Apellido + ", " + u.Nombre : (p != null ? p.Apellido + ", " + p.Nombres : "")),
                                        numero_Gedo = ""
                                    }).ToList();
                        foreach (var arc in archivos)
                        {
                            if (arc.url == null)
                                arc.url = string.Format("~/GetPDFFiles/{0}", Functions.ConvertToBase64(arc.id_file.ToString()));
                        }

                        //Agrego el Nº GEDO si es que existe para cada documento.
                        foreach (var item in archivos)
                        {
                            item.numero_Gedo = LoadNumeroGedo(item.id_file, id_solicitud);
                        }

                        grdDocumentosAdjuntos.Columns[5].Visible = visibleEliminar;
                        grdDocumentosAdjuntos.DataSource = archivos.OrderBy(x => x.Fecha).ToList();
                        grdDocumentosAdjuntos.DataBind();
                        break;
                }
            }
        }

        protected void lnkEliminarDocAdj_Command(object sender, CommandEventArgs e)
        {
            try
            {
                LinkButton lnkEliminar = (LinkButton)sender;
                int id_doc_adj = Convert.ToInt32(lnkEliminar.CommandArgument);
                this.db = new DGHP_Entities();
                db.Database.CommandTimeout = 300;

                if (this.id_tipodocsis == (int)Constants.TiposDeDocumentosSistema.PRESENTACION_A_AGREGAR)
                {
                    db.SSIT_DocumentosAdjuntos_Del(id_doc_adj);
                }

                ucListaDocumentosv1EventsArgs args = new ucListaDocumentosv1EventsArgs();
                args.id_doc_adj = id_doc_adj;
                this.EliminarListaDocumentosv1Click(this, args);

                db.Dispose();
            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();
                /*ScriptManager.RegisterClientScriptBlock(updPnlDocumentoAdjunto, updPnlDocumentoAdjunto.GetType(),
                        "tda_mostrar_mensaje", "tda_mostrar_mensaje('" + ex.Message + "','')", true);*/
            }

        }

        protected void grdDocumentosAdjuntos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (visibleEliminar)
                {
                    LinkButton lnkEliminarDocAdj = (LinkButton)e.Row.FindControl("lnkEliminarDocAdj");
                    if (grdDocumentosAdjuntos.DataKeys[e.Row.RowIndex].Values["id_tipodocsis"] != null)
                    {
                        int id = Convert.ToInt32(grdDocumentosAdjuntos.DataKeys[e.Row.RowIndex].Values["id_tipodocsis"]);
                        lnkEliminarDocAdj.Visible = id == this.id_tipodocsis;
                    }
                }
            }
        }
        private async Task<List<GetCAAsByEncomiendasResponse>> GetCAAsByEncomiendas(int[] lst_id_Encomiendas)
        {
            ExternalService.ApraSrvRest apraSrvRest = new ExternalService.ApraSrvRest();
            List<GetCAAsByEncomiendasResponse> lstCaa = await apraSrvRest.GetCAAsByEncomiendas(lst_id_Encomiendas.ToList());
            return lstCaa;
        }
    }
}

public class ucListaDocumentosv1EventsArgs : EventArgs
{
    public int id_doc_adj { get; set; }
}