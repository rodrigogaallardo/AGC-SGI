using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;
using System.ComponentModel;
using System.Web.Security;
using System.Data;

namespace SGI.GestionTramite.Controls
{
    public class ucSGI_listaPlanoVisadoEventsArgs : EventArgs
    {
    }
    public partial class ucSGI_ListaPlanoVisado : System.Web.UI.UserControl
    {
        public class itemDocumento
        {
            public int id_doc_adj { get; set; }
            public string nombre { get; set; }
            public int id_file { get; set; }
            public int id_solicitud { get; set; }
            public string url { get; set; }
            public int id_tdocreq { get; set; }
            public bool esPlanoVisado { get; set; }
        }



        private DGHP_Entities db;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LoadData(int id_solicitud, int id_tramitetarea)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea);
        }

        private int[] GetTiposPlano()
        {
            int[] tiposDeTramite = new int[3] {
                                                (int)Constants.TiposDePlanos.Plano_Habilitacion,
                                                (int)Constants.TiposDePlanos.Plano_Ampliacion,
                                                (int)Constants.TiposDePlanos.Plano_Redistribucion_Uso
                                              };

            return tiposDeTramite;
        }

        private int[] GetTiposDeDocumentosRequeridos()
        {
            int[] tiposDeDocumentosRequeridos = new int[3] {
                                                            (int)Constants.TiposDeDocumentosRequeridos.Plano_Habilitacion,
                                                            (int)Constants.TiposDeDocumentosRequeridos.Plano_Ampliacion,
                                                            (int)Constants.TiposDeDocumentosRequeridos.Plano_Redistribucion_Uso
                                                           };

            return tiposDeDocumentosRequeridos;
        }

        private int GetTipoDeDocumentoRequerido(int id_solicitud)
        {
            db = new DGHP_Entities();
            int tipoTramite = (from s in db.SSIT_Solicitudes
                               where s.id_solicitud == id_solicitud
                               select s.id_tipotramite).FirstOrDefault();

            switch (tipoTramite)
            {
                case (int)Constants.TipoDeTramite.Habilitacion:
                    return (int)Constants.TiposDeDocumentosRequeridos.Plano_Habilitacion;
                case (int)Constants.TipoDeTramite.Ampliacion_Unificacion:
                    return (int)Constants.TiposDeDocumentosRequeridos.Plano_Ampliacion;
                case (int)Constants.TipoDeTramite.RedistribucionDeUso:
                    return (int)Constants.TiposDeDocumentosRequeridos.Plano_Redistribucion_Uso;
                default:
                    return 0;
            }
        }

        public void LoadData(int id_grupotramite, int id_solicitud, int id_tramitetarea)
        {
            db = new DGHP_Entities();
            this.id_solicitud = id_solicitud;
            this.id_tramitetarea = id_tramitetarea;
            this.id_grupotramite = id_grupotramite;

            int[] tiposPlano = GetTiposPlano();
            int[] tiposDeDocumentosRequeridos = GetTiposDeDocumentosRequeridos();
            int tipoDeDocumentoRequerido = GetTipoDeDocumentoRequerido(id_solicitud);

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
                                select new itemDocumento
                                {
                                    id_doc_adj = encdoc.id_docadjunto,
                                    nombre = (encdoc.id_tdocreq != 0 ? encdoc.TiposDeDocumentosRequeridos.nombre_tdocreq : encdoc.TiposDeDocumentosSistema.nombre_tipodocsis) + "-" + encdoc.id_encomienda,
                                    id_file = encdoc.id_file,
                                    id_solicitud = encdoc.id_encomienda,
                                    url = null,
                                    id_tdocreq = encdoc.id_tdocreq,
                                    esPlanoVisado = false
                                }).Union(//SSIT_Documentos adjuntos
                                from doc in db.SSIT_DocumentosAdjuntos
                                join user in db.Usuario on doc.CreateUser equals user.UserId into us
                                from u in us.DefaultIfEmpty()
                                join prof in db.SGI_Profiles on doc.CreateUser equals prof.userid into pr
                                from p in pr.DefaultIfEmpty()
                                where doc.id_solicitud == id_solicitud
                                select new itemDocumento
                                {
                                    id_doc_adj = doc.id_docadjunto,
                                    nombre = doc.tdocreq_detalle != null && doc.tdocreq_detalle != "" ?
                                        doc.tdocreq_detalle : (doc.id_tdocreq != 0 ? doc.TiposDeDocumentosRequeridos.nombre_tdocreq : doc.TiposDeDocumentosSistema.nombre_tipodocsis),
                                    id_file = doc.id_file,
                                    id_solicitud = doc.id_solicitud,
                                    url = null,
                                    id_tdocreq = doc.id_tdocreq,
                                    esPlanoVisado = false
                                })./*Union(//SGI_Dcouemntos_adjuntos
                                from docadj in db.SGI_Tarea_Documentos_Adjuntos
                                join tt_hab in db.SGI_Tramites_Tareas_HAB on docadj.id_tramitetarea equals tt_hab.id_tramitetarea
                                join user in db.SGI_Profiles on docadj.CreateUser equals user.userid into us
                                from u in us.DefaultIfEmpty()
                                where tt_hab.id_solicitud == id_solicitud
                                select new itemDocumento
                                {
                                    id_doc_adj = docadj.id_doc_adj,
                                    nombre = docadj.TiposDeDocumentosRequeridos.nombre_tdocreq,
                                    id_file = docadj.id_file,
                                    id_solicitud = id_solicitud,
                                    url = null,
                                    id_tdocreq = docadj.TiposDeDocumentosRequeridos.id_tdocreq,
                                    esPlanoVisado = false
                                }).*/Union(//Planos
                                from encdoc in db.Encomienda_Planos
                                join prof in db.Profesional on encdoc.CreateUser equals prof.UserId.ToString() into us
                                from u in us.DefaultIfEmpty()
                                where lstEncomiendas.Contains(encdoc.id_encomienda) && tiposPlano.Contains(encdoc.id_tipo_plano)
                                select new itemDocumento
                                {
                                    id_doc_adj = encdoc.id_encomienda_plano,
                                    nombre = (encdoc.TiposDePlanos.requiere_detalle == true && !string.IsNullOrEmpty(encdoc.detalle) ? encdoc.detalle : encdoc.TiposDePlanos.nombre) + "-" + encdoc.id_encomienda,
                                    id_file = encdoc.id_file,
                                    id_solicitud = encdoc.id_encomienda,
                                    url = null,
                                    id_tdocreq = tipoDeDocumentoRequerido,
                                    esPlanoVisado = false
                                }).Union(//SGI_Tarea_Calificar_ObsDocs
                                    from o in db.SGI_Tarea_Calificar_ObsDocs
                                    join g in db.SGI_Tarea_Calificar_ObsGrupo on o.id_ObsGrupo equals g.id_ObsGrupo
                                    join th in db.SGI_Tramites_Tareas_HAB on g.id_tramitetarea equals th.id_tramitetarea
                                    join sol in db.SSIT_Solicitudes on th.id_solicitud equals sol.id_solicitud
                                    join file in db.Files on o.id_file equals file.id_file
                                    join user in db.Usuario on sol.CreateUser equals user.UserId
                                    where th.id_solicitud == id_solicitud && o.id_file != null
                                    select new itemDocumento
                                    {
                                        id_doc_adj = o.id_ObsDocs,
                                        nombre = "Documento_Observacion" + (o.id_file != null ? o.id_file.ToString() : (o.id_certificado != 0 ? o.id_certificado.ToString() : "")) + ".pdf",
                                        id_file = o.id_file.Value,
                                        id_solicitud = th.id_solicitud,
                                        url = null,
                                        id_tdocreq = o.id_tdocreq,
                                        esPlanoVisado = false
                                    });

                var a = (from arc in archivos
                         join plan in db.Solicitud_planoVisado on new { id = arc.id_solicitud, idDoc = arc.id_doc_adj }
                         equals new { id = plan.id_solicitud, idDoc = plan.id_docAdjunto } into planos
                         from p in planos.DefaultIfEmpty()
                         where tiposDeDocumentosRequeridos.Contains(arc.id_tdocreq)
                         select arc).Distinct().ToList();

                foreach (var arc in a)
                {
                    if (arc.url == null)
                        arc.url = string.Format("~/GetPDFFiles/{0}", Functions.ConvertToBase64(arc.id_file.ToString()));
                    if (arc.esPlanoVisado == false)
                    {
                        var plan = db.Solicitud_planoVisado.Where(x => x.id_tramiteTarea == this.id_tramitetarea && x.id_docAdjunto == arc.id_doc_adj).ToList();
                        if (plan.Count > 0)
                            arc.esPlanoVisado = true;
                        //else if(db.Solicitud_planoVisado.Where(x => x.id_tramiteTarea < this.id_tramitetarea 
                        //        && x.id_docAdjunto == arc.id_doc_adj).Count() > 0)
                        //        arc.esPlanoVisado = true;
                    }
                }

                grd_plan_visado.DataSource = a;
                grd_plan_visado.DataBind();
            }
            db.Dispose();
        }

        protected void chkPlanoVisado_CheckedChanged(Object sender, EventArgs e)
        {
            db = new DGHP_Entities();
            try
            {
                foreach (GridViewRow row in grd_plan_visado.Rows)
                {

                    CheckBox chkSeleccionado = (CheckBox)grd_plan_visado.Rows[row.RowIndex].Cells[0].FindControl("chkPlanoVisado");

                    if (chkSeleccionado.Checked)
                    {
                        var doc = (from docadj in db.SGI_Tarea_Documentos_Adjuntos
                                   join tt_hab in db.SGI_Tramites_Tareas_HAB on docadj.id_tramitetarea equals tt_hab.id_tramitetarea
                                   /*join user in db.SGI_Profiles on docadj.CreateUser equals user.userid into us
                                   from u in us.DefaultIfEmpty()*/
                                   where tt_hab.id_solicitud == this.id_solicitud && tt_hab.id_tramitetarea < tt_hab.id_tramitetarea
                                   select docadj).ToList();
                        if (doc.Count > 0)
                            throw new Exception("Debe eliminar los plano/s adjuntado/s anteriormente.");
                    }
                }
                db.Dispose();
            }
            catch (Exception ex)
            {
                db.Dispose();
                LogError.Write(ex);
                lblError.Text = ex.Message;
                ScriptManager.RegisterClientScriptBlock(updPnlPlanVisado, updPnlPlanVisado.GetType(), "mostrarError", "showfrmError_AnexoTecnico(); ", true);
                //Server.Transfer(string.Format("~/Errores/error3020.aspx?m={0}", "Procedimiento CargarDatos en Tab_Rubros.aspx"));
                //throw ex;
            }
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //ucSGI_listaPlanoVisadoEventsArgs arg = new ucSGI_listaPlanoVisadoEventsArgs();
            //if (this.GuardarClick != null)
            //    this.GuardarClick(this, arg);
            try
            {
                db = new DGHP_Entities();
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
                int id_doc = 0;

                db.Solicitud_planoVisado_Eliminar(this.id_solicitud, this.id_tramitetarea);

                foreach (GridViewRow row in grd_plan_visado.Rows)
                {
                    CheckBox chkPlanoElegido = (CheckBox)row.FindControl("chkPlanoVisado");
                    id_doc = 0;
                    int.TryParse(grd_plan_visado.DataKeys[row.RowIndex].Values["id_doc_adj"].ToString(), out id_doc);
                    if (chkPlanoElegido.Checked)
                    {
                        db.Solicitud_planoVisado_Agregar(this.id_solicitud, this.id_tramitetarea, userid, id_doc);
                    }
                }
                db.Dispose();
            }
            catch (Exception ex)
            {
                db.Dispose();
                throw new Exception(ex.Message);
            }
        }

        private int _id_solicitud = 0;
        public int id_solicitud
        {
            get
            {
                if (_id_solicitud == 0)
                {
                    int.TryParse(hid_id_solicitud.Value, out _id_solicitud);
                }
                return _id_solicitud;
            }
            set
            {
                hid_id_solicitud.Value = value.ToString();
                _id_solicitud = value;
            }
        }

        private int _id_grupotramite = 0;
        public int id_grupotramite
        {
            get
            {
                if (_id_grupotramite == 0)
                {
                    int.TryParse(hid_id_grupotramite.Value, out _id_grupotramite);
                }
                return _id_grupotramite;
            }
            set
            {
                hid_id_grupotramite.Value = value.ToString();
                _id_grupotramite = value;
            }
        }

        private int _id_tramitetarea = 0;
        public int id_tramitetarea
        {
            get
            {
                if (_id_tramitetarea == 0)
                {
                    int.TryParse(hid_id_tramitetarea.Value, out _id_tramitetarea);
                }
                return _id_tramitetarea;
            }
            set
            {
                hid_id_tramitetarea.Value = value.ToString();
                _id_tramitetarea = value;
            }
        }


        private bool _Enabled;

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(true),
        Description("Devuelve/Establece el estado de los controles contenidos en este control.")]
        public bool Enabled
        {
            get
            {

                if (!bool.TryParse(hid_editable.Value, out _Enabled))
                    _Enabled = false;
                return _Enabled;
            }
            set
            {
                _Enabled = value;
                hid_editable.Value = _Enabled.ToString().ToLower();
                grd_plan_visado.Enabled = _Enabled;
                btnGuardar.Visible = _Enabled;
            }
        }

        public int getSeleccionPlanos()
        {
            db = new DGHP_Entities();
            int resultado = 0;

            resultado = db.Solicitud_planoVisado.Where(x => x.id_tramiteTarea == this.id_tramitetarea &&
            x.id_solicitud == this.id_solicitud).ToList().Count;

            db.Dispose();
            return resultado;
        }
    }
}