using ExternalService.Class;
using SGI.Model;
using SGI.Webservices.ws_interface_AGC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public class itemDocumentoSSIT
    {
        public int id_doc_adj { get; set; }
        public string nombre { get; set; }
        public int id_file { get; set; }
        public int id_solicitud { get; set; }
        public string url { get; set; }
        public DateTime Fecha { get; set; }
        public string UserName { get; set; }
        public int id_tipodocsis { get; set; }
    }

    public partial class ucListaDocumentosSSIT : System.Web.UI.UserControl
    {
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

        public void LoadData(int id_solicitud)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud);
        }

        public void LoadData(int id_grupotramite, int id_solicitud)
        {
            LoadData(id_grupotramite, id_solicitud, false, 0);
        }
        public async void LoadData(int id_grupotramite, int id_solicitud, bool visibleEliminar, int id_tipodocsis_a_eliminar)
        {
            db = new DGHP_Entities();
            dbFiles = new AGC_FilesEntities();
            this.visibleEliminar = visibleEliminar;
            this.id_tipodocsis = id_tipodocsis_a_eliminar;

            if (id_grupotramite == (int)Constants.GruposDeTramite.HAB)
            {
                List<TipoTramiteCertificados> lstTipoTramiteCertificados = dbFiles.TipoTramiteCertificados.ToList();

                // Obtiene la descripcion de los certificados
                string nom_caa = lstTipoTramiteCertificados.Where(y => y.TipoTramite == (int)Constants.TipoTramiteCertificados.CAA).Select(x => x.Descripcion).FirstOrDefault();
                string nom_acta_notarial = lstTipoTramiteCertificados.Where(y => y.TipoTramite == (int)Constants.TipoTramiteCertificados.ActaNotarial).Select(x => x.Descripcion).FirstOrDefault();

                // Obtiene las encomiendas relacionadas
                List<Int32> lstEncomiendas = (from e in db.Encomienda
                                              join encsol in db.Encomienda_SSIT_Solicitudes on e.id_encomienda equals encsol.id_encomienda
                                              where encsol.id_solicitud == id_solicitud
                                              && e.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                                              select e.id_encomienda).ToList();

                var encomienda = db.Encomienda.Where(x => x.Encomienda_SSIT_Solicitudes.Select(y => y.id_solicitud).FirstOrDefault() == id_solicitud
                        && x.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo).OrderByDescending(x => x.id_encomienda).FirstOrDefault();
                if (encomienda != null)
                {
                    int id_encomienda = encomienda.id_encomienda;

                    var archivos = (//Encomienda_DocumentosAdjuntos
                                    from encdoc in db.Encomienda_DocumentosAdjuntos
                                    join prof in db.Profesional on encdoc.CreateUser equals prof.UserId into us
                                    from u in us.DefaultIfEmpty()
                                    where lstEncomiendas.Contains(encdoc.id_encomienda)
                                    select new itemDocumentoSSIT
                                    {
                                        id_doc_adj = encdoc.id_docadjunto,
                                        nombre = (encdoc.id_tdocreq != 0 ? encdoc.TiposDeDocumentosRequeridos.nombre_tdocreq : encdoc.TiposDeDocumentosSistema.nombre_tipodocsis) + "-" + encdoc.id_encomienda,
                                        id_file = encdoc.id_file,
                                        id_solicitud = encdoc.id_encomienda,
                                        url = null,
                                        Fecha = encdoc.CreateDate,
                                        UserName = u != null ? u.Apellido + ", " + u.Nombre : "",
                                        id_tipodocsis = encdoc.id_tipodocsis
                                    }).Union(//Acta notarial
                                    from acta in db.wsEscribanos_ActaNotarial
                                    where lstEncomiendas.Contains(acta.id_encomienda) && !acta.anulada && acta.id_file != null
                                    select new itemDocumentoSSIT
                                    {
                                        id_doc_adj = acta.id_actanotarial,
                                        nombre = nom_acta_notarial + "-" + acta.id_actanotarial,
                                        id_file = acta.id_file.Value,
                                        id_solicitud = acta.id_encomienda,
                                        url = null,
                                        Fecha = acta.CreateDate,
                                        UserName = "",
                                        id_tipodocsis = 0
                                    }).Union(//SSIT_Documentos adjuntos
                                    from doc in db.SSIT_DocumentosAdjuntos
                                    join user in db.Usuario on doc.CreateUser equals user.UserId into us
                                    from u in us.DefaultIfEmpty()
                                    join prof in db.SGI_Profiles on doc.CreateUser equals prof.userid into pr
                                    from p in pr.DefaultIfEmpty()
                                    where doc.id_solicitud == id_solicitud
                                    select new itemDocumentoSSIT
                                    {
                                        id_doc_adj = doc.id_docadjunto,
                                        nombre = doc.tdocreq_detalle!=null && doc.tdocreq_detalle != "" ?
                                            doc.tdocreq_detalle :(doc.id_tdocreq != 0 ? doc.TiposDeDocumentosRequeridos.nombre_tdocreq : doc.TiposDeDocumentosSistema.nombre_tipodocsis),
                                        id_file = doc.id_file,
                                        id_solicitud = doc.id_solicitud,
                                        url = null,
                                        Fecha = doc.CreateDate,
                                        UserName = (u != null ? u.Apellido + ", " + u.Nombre : (p != null ? p.Apellido + ", " + p.Nombres : "")),
                                        id_tipodocsis = doc.id_tipodocsis
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
                    //DtoCAA[] l = servicio.Get_CAAs_by_Encomiendas(username_servicio, password_servicio, lstEncomiendas.ToArray(), ref ws_resultado_CAA);
                    List<GetCAAsByEncomiendasResponse> list = await GetCAAsByEncomiendas(lstEncomiendas.ToArray());
                    //var List_CAA = list.ToList().Where(x => x.id_estado != (int)Constants.CAA_Estados.Anulado && x.documentos.Any());
                    if(list != null && list.Count() > 0)
                    {
                        var List_CAA = list.ToList().Where(x => x.id_estado != (int)Constants.CAA_Estados.Anulado && x.certificado != null);
                        foreach (var caa in List_CAA)
                        {
                            var item = new itemDocumentoSSIT();
                            //item.nombre = caa.desccorta_tipotramite + "-" + caa.id_caa;
                            item.nombre = caa.tipotramite + "-" + caa.id_solicitud;
                            //item.id_file = caa.Documentos[0].id_file;
                            item.id_file = caa.certificado.idFile;
                            //item.id_solicitud = caa.id_caa;
                            item.id_solicitud = caa.id_solicitud;
                            item.url = string.Format("~/GetPDFFiles/{0}", Functions.ConvertToBase64(item.id_file.ToString()));
                            //item.Fecha = caa.CreateDate;
                            item.Fecha = caa.createDate;
                            item.UserName = "";
                            archivos.Add(item);
                        }
                    }
                    
                    grdDocumentosAdjuntos.DataSource = archivos.OrderBy(x => x.Fecha).ToList();
                    grdDocumentosAdjuntos.DataBind();
                }
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.CP)
            {
                var archivos = (//Encomienda_DocumentosAdjuntos
                                from doc in db.CPadron_DocumentosAdjuntos
                                join user in db.Usuario on doc.CreateUser equals user.UserId into us
                                from u in us.DefaultIfEmpty()
                                join prof in db.SGI_Profiles on doc.CreateUser equals prof.userid into pr
                                from p in pr.DefaultIfEmpty()
                                where doc.id_cpadron == id_solicitud
                                select new itemDocumentoSSIT
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
                grdDocumentosAdjuntos.DataSource = archivos.OrderBy(x => x.Fecha).ToList();
                grdDocumentosAdjuntos.DataBind();
            }
            else if (id_grupotramite == (int)Constants.GruposDeTramite.TR)
            {
                var archivos = (//Encomienda_DocumentosAdjuntos
                                from doc in db.Transf_DocumentosAdjuntos
                                join user in db.Usuario on doc.CreateUser equals user.UserId into us
                                from u in us.DefaultIfEmpty()
                                join prof in db.SGI_Profiles on doc.CreateUser equals prof.userid into pr
                                from p in pr.DefaultIfEmpty()
                                where doc.id_solicitud == id_solicitud
                                select new itemDocumentoSSIT
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
                                where sol.id_solicitud == id_solicitud && doc.id_tipodocsis == (int)Constants.TiposDeDocumentosSistema.INFORMES_CPADRON
                                select new itemDocumentoSSIT
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
                grdDocumentosAdjuntos.DataSource = archivos.OrderBy(x => x.Fecha).ToList();
                grdDocumentosAdjuntos.DataBind();
            }

            db.Dispose();
            dbFiles.Dispose();
        }
        private async Task<List<GetCAAsByEncomiendasResponse>> GetCAAsByEncomiendas(int[] lst_id_Encomiendas)
        {
            ExternalService.ApraSrvRest apraSrvRest = new ExternalService.ApraSrvRest();
            List<GetCAAsByEncomiendasResponse> lstCaa = await apraSrvRest.GetCAAsByEncomiendas(lst_id_Encomiendas.ToList());
            return lstCaa;
        }
    }
}
