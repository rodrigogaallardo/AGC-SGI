﻿using SGI;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls
{
    public partial class ucListaObservacionesAnterioresv1 : System.Web.UI.UserControl
    {
        #region cargar inicial

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void LoadData(int id_solicitud, int id_tramitetarea, int id_tarea)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea, id_tarea);
        }

        public void LoadData(int id_grupotramite, int id_solicitud, int id_tramitetarea, int id_tarea)
        {

            List<ObservacionAnterioresv1> lista_observ = ObservacionAnterioresv1.GetTareaObservacion(id_grupotramite,
                                id_solicitud, id_tramitetarea, TramiteTareaAnteriores.Dependencias_Tarea_ObservacionesV1(id_tarea));
            grdObservTareasAnterioresv1.DataSource = lista_observ;
            grdObservTareasAnterioresv1.DataBind();

            pnlObservAnterior.Visible = (grdObservTareasAnterioresv1.Rows.Count > 0);

 
        }

        protected void grdObservTareasAnteriores_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObservacionAnterioresv1 observ  = (ObservacionAnterioresv1)e.Row.DataItem;
                DataList dl_observ = (DataList)e.Row.FindControl("dl_observ");

                dl_observ.DataSource = observ.Item;;
                dl_observ.DataBind();
            }

        }

        #endregion

        #region attributos
        private string _titulo = "";
        public string Titulo
        {
            get
            {
                return _titulo;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _titulo = value;
                else
                    _titulo = "Observaciones Tareas Anteriores";
                tituloControl.Text = _titulo;
            }
        }

        private bool _collapse;
        public bool Collapse
        {
            get
            {
                return _collapse;
            }
            set
            {
                _collapse = value;
                hid_loa_collapse_v1.Value = _collapse.ToString().ToLower();

            }
        }

        #endregion

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
                this.db.Dispose();
        }

        private void IniciarEntityFiles()
        {
            if (this.dbFiles == null)
                this.dbFiles = new AGC_FilesEntities();
        }

        private void FinalizarEntityFiles()
        {
            if (this.dbFiles != null)
                this.dbFiles.Dispose();
            dbFiles.Database.CommandTimeout = 300;
        }

        #endregion
    }
}

public class ObservacionAnterioresv1
{

    public ObservacionAnterioresv1()
    {
        this.Item = new List<Items>();
        
    }

    #region atributos

    public int ID { get; set; }
    public int id_tarea { get; set; }
    public string Nombre_tarea { get; set; }
    public int id_tramitetarea { get; set; }
    public string Observaciones { get; set; }
    public string NotasAdicionales { get; set; }
    public List<Items> Item { get; set; }
    public string UsuarioApeNom { get; set; }
    public DateTime Fecha { get; set; }

    #endregion

    #region metodos estaticos



    public static List<ObservacionAnterioresv1> GetTareaObservacion(int id_grupotramite, int id_solicitud, int id_tramitetarea, int[] tareas)
    {
        DGHP_Entities db = new DGHP_Entities();
        db.Database.CommandTimeout = 300;
        List<ObservacionAnterioresv1> list_observ = null;

        try
        {

            List<TramiteTareaAnteriores> lista_tramite = TramiteTareaAnteriores.BuscarUltimoTramiteTareaPorTarea(id_grupotramite, id_solicitud, id_tramitetarea, tareas);
            foreach (var item in lista_tramite)
            {
                if (list_observ == null)
                    list_observ = new List<ObservacionAnterioresv1>();

                ObservacionAnterioresv1 observ = ObservacionAnterioresv1.GetTareaObservacion(id_grupotramite, item.id_tarea, item.id_tramitetarea);

                if (observ != null) // puede ser que no haya pasado por alguna tarea
                    list_observ.Add(observ);

            }

        }
        catch (Exception ex)
        {
            if (db != null)
                db.Dispose();
            throw ex;
        }

        return list_observ;

    }

    public static ObservacionAnterioresv1 GetTareaObservacion(int id_grupotramite, int id_tarea, int id_tramitetarea)
    {
        bool ver_observ_vacias = false;
        ObservacionAnterioresv1 tareaObserv = null;

        DGHP_Entities db = new DGHP_Entities();
        db.Database.CommandTimeout = 300;
        var ta = db.ENG_Tareas.Where(x => x.id_tarea == id_tarea).First();
        string tipo_tarea = ta.cod_tarea.ToString();
        tipo_tarea = tipo_tarea.Substring(tipo_tarea.Length - 2);

        try
        {
            switch (tipo_tarea)
            {
                #region Asignación de Calificador
                //9 	Asignación de Calificador			Asignar_Calificador.aspx	    SGI_Tarea_Asignar_Calificador
                case Constants.ENG_Tipos_Tareas.Asignacion_Calificador:
                case Constants.ENG_Tipos_Tareas.Asignacion_Calificador2:
                    tareaObserv =
                        (
                            from calificador in db.SGI_Tarea_Asignar_Calificador
                            join tramite in db.SGI_Tramites_Tareas on calificador.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on calificador.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on calificador.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where calificador.id_tramitetarea == id_tramitetarea
                            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(calificador.Observaciones))
                            orderby calificador.id_asignar_calificador descending
                            select new ObservacionAnterioresv1
                            {
                                ID = calificador.id_asignar_calificador,
                                Observaciones = calificador.Observaciones,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (calificador.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (calificador.LastUpdateDate.HasValue) ? (DateTime)calificador.LastUpdateDate : calificador.CreateDate
                            }
                        ).FirstOrDefault();

                    break;

                #endregion

                #region Calificar Trámite
                case Constants.ENG_Tipos_Tareas.Calificar:
                case Constants.ENG_Tipos_Tareas.Calificar2:
                case Constants.ENG_Tipos_Tareas.Calificar3:
                    var q_calificarN =
                        (
                            from calificar in db.SGI_Tarea_Calificar
                            join tramite in db.SGI_Tramites_Tareas on calificar.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on calificar.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on calificar.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where calificar.id_tramitetarea == id_tramitetarea
                            && (
                                ver_observ_vacias ||
                                (!ver_observ_vacias &&
                                    (
                                        !string.IsNullOrEmpty(calificar.Observaciones) ||
                                        !string.IsNullOrEmpty(calificar.Observaciones_Internas) ||
                                        !string.IsNullOrEmpty(calificar.Observaciones_contribuyente)
                                    )
                                )
                               )
                            orderby calificar.id_calificar descending
                            select new
                            {
                                ID = calificar.id_calificar,
                                Nombre_tarea = tarea.nombre_tarea,
                                Observaciones = calificar.Observaciones,
                                Observaciones_Internas = calificar.Observaciones_Internas,
                                Observaciones_contribuyente = calificar.Observaciones_contribuyente,
                                UsuarioApeNom = (calificar.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (calificar.LastUpdateDate.HasValue) ? (DateTime)calificar.LastUpdateDate : calificar.CreateDate
                            }
                        ).FirstOrDefault();

                    if (q_calificarN != null)
                    {
                        tareaObserv = new ObservacionAnterioresv1();

                        tareaObserv.ID = q_calificarN.ID;
                        tareaObserv.id_tarea = 0;
                        tareaObserv.Nombre_tarea = q_calificarN.Nombre_tarea;
                        tareaObserv.id_tramitetarea = id_tramitetarea;
                        tareaObserv.Observaciones = "";
                        tareaObserv.UsuarioApeNom = q_calificarN.UsuarioApeNom;
                        tareaObserv.Fecha = q_calificarN.Fecha;

                        if (!string.IsNullOrEmpty(q_calificarN.Observaciones))
                            tareaObserv.Item.Add(new Items(q_calificarN.Observaciones + "<br/>", "Notas adicionales para la disposición:"));

                        if (!string.IsNullOrEmpty(q_calificarN.Observaciones_Internas))
                            tareaObserv.Item.Add(new Items(q_calificarN.Observaciones_Internas, "Observaciones internas:"));

                        if (!string.IsNullOrEmpty(q_calificarN.Observaciones_contribuyente))
                            tareaObserv.Item.Add(new Items(q_calificarN.Observaciones_contribuyente + "<br/>", "Observaciones al Contribuyente:"));

                    }

                    var lstObservaciones = (from obs in db.SGI_Tarea_Calificar_ObsDocs
                                            join tdocreq in db.TiposDeDocumentosRequeridos on obs.id_tdocreq equals tdocreq.id_tdocreq
                                            join grup in db.SGI_Tarea_Calificar_ObsGrupo on obs.id_ObsGrupo equals grup.id_ObsGrupo
                                            join file in db.Files on obs.id_file equals file.id_file into fleftjoin
                                            from fil in fleftjoin.DefaultIfEmpty()
                                            join cert in db.vis_Certificados on obs.id_certificado equals cert.id_certificado into cleftjoin
                                            from cer in cleftjoin.DefaultIfEmpty()
                                            join tt_h in db.SGI_Tramites_Tareas_HAB on grup.id_tramitetarea equals tt_h.id_tramitetarea
                                            join calificar in db.SGI_Tarea_Calificar on tt_h.id_tramitetarea equals calificar.id_tramitetarea
                                            join tarea in db.ENG_Tareas on tt_h.SGI_Tramites_Tareas.id_tarea equals tarea.id_tarea
                                            join usrAlta in db.SGI_Profiles on calificar.CreateUser equals usrAlta.userid
                                            join usrMod in db.SGI_Profiles on calificar.LastUpdateUser equals usrMod.userid
                                            into pleftjoin
                                            from prof in pleftjoin.DefaultIfEmpty()
                                            where tt_h.id_tramitetarea == id_tramitetarea && obs.Actual == false
                                            select new
                                            {
                                                ID = calificar.id_calificar,
                                                Nombre_tarea = tarea.nombre_tarea,
                                                UsuarioApeNom = (calificar.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                                Fecha = (calificar.LastUpdateDate.HasValue) ? (DateTime)calificar.LastUpdateDate : calificar.CreateDate,
                                                id_ObsDocs = obs.id_ObsDocs,
                                                id_ObsGrupo = obs.id_ObsGrupo,
                                                nombre_tdocreq = tdocreq.nombre_tdocreq,
                                                id_tdocreq = obs.id_tdocreq,
                                                Observacion_ObsDocs = obs.Observacion_ObsDocs,
                                                Respaldo_ObsDocs = obs.Respaldo_ObsDocs,
                                                id_file = obs.id_file,
                                                id_certificado = obs.id_certificado,
                                                Decido_no_subir = obs.Decido_no_subir.Value
                                            }).Union(from obs in db.SGI_Tarea_Calificar_ObsDocs
                                                     join tdocreq in db.TiposDeDocumentosRequeridos on obs.id_tdocreq equals tdocreq.id_tdocreq
                                                     join grup in db.SGI_Tarea_Calificar_ObsGrupo on obs.id_ObsGrupo equals grup.id_ObsGrupo
                                                     join file in db.Files on obs.id_file equals file.id_file into fleftjoin
                                                     from fil in fleftjoin.DefaultIfEmpty()
                                                     join cert in db.vis_Certificados on obs.id_certificado equals cert.id_certificado into cleftjoin
                                                     from cer in cleftjoin.DefaultIfEmpty()
                                                     join tt_h in db.SGI_Tramites_Tareas_TRANSF on grup.id_tramitetarea equals tt_h.id_tramitetarea
                                                     join calificar in db.SGI_Tarea_Calificar on tt_h.id_tramitetarea equals calificar.id_tramitetarea
                                                     join tarea in db.ENG_Tareas on tt_h.SGI_Tramites_Tareas.id_tarea equals tarea.id_tarea
                                                     join usrAlta in db.SGI_Profiles on calificar.CreateUser equals usrAlta.userid
                                                     join usrMod in db.SGI_Profiles on calificar.LastUpdateUser equals usrMod.userid
                                                     into pleftjoin
                                                     from prof in pleftjoin.DefaultIfEmpty()
                                                     where tt_h.id_tramitetarea == id_tramitetarea && obs.Actual == false
                                                     select new
                                                     {
                                                         ID = calificar.id_calificar,
                                                         Nombre_tarea = tarea.nombre_tarea,
                                                         UsuarioApeNom = (calificar.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                                         Fecha = (calificar.LastUpdateDate.HasValue) ? (DateTime)calificar.LastUpdateDate : calificar.CreateDate,
                                                         id_ObsDocs = obs.id_ObsDocs,
                                                         id_ObsGrupo = obs.id_ObsGrupo,
                                                         nombre_tdocreq = tdocreq.nombre_tdocreq,
                                                         id_tdocreq = obs.id_tdocreq,
                                                         Observacion_ObsDocs = obs.Observacion_ObsDocs,
                                                         Respaldo_ObsDocs = obs.Respaldo_ObsDocs,
                                                         id_file = obs.id_file,
                                                         id_certificado = obs.id_certificado,
                                                         Decido_no_subir = obs.Decido_no_subir.Value
                                                     }).ToList();


                    if (lstObservaciones.Count > 0)
                    {
                        string observ;
                        foreach (var lObs in lstObservaciones)
                        {
                            if (tareaObserv == null)
                            {
                                tareaObserv = new ObservacionAnterioresv1();

                                tareaObserv.ID = lObs.ID;
                                tareaObserv.id_tarea = 0;
                                tareaObserv.Nombre_tarea = lObs.Nombre_tarea;
                                tareaObserv.id_tramitetarea = id_tramitetarea;
                                tareaObserv.Observaciones = "";
                                tareaObserv.UsuarioApeNom = lObs.UsuarioApeNom;
                                tareaObserv.Fecha = lObs.Fecha;
                            }
                            observ = "<b>Tipo de Documento: </b>" + lObs.nombre_tdocreq + "<br/>";
                            observ += "<b>Observación: </b>" + lObs.Observacion_ObsDocs + "<br/>";
                            observ += "<b>Respaldo Normativo:</b>" + lObs.Respaldo_ObsDocs + "<br/>";

                            string link = "";
                            if (lObs.Decido_no_subir)
                                link = "Decide NO subirlo";
                            else {
                               string url = lObs.id_file != null ?
                               string.Format("../../GetPDF/{0}", HttpUtility.UrlEncode(Convert.ToBase64String(Encoding.ASCII.GetBytes(lObs.id_file.ToString()))))
                               : (lObs.id_certificado != 0 ? string.Format("../../ImprimirCertificado/{0}", lObs.id_certificado) : "");
                               string nom = "Documento_Observacion" + (lObs.id_file != null ? lObs.id_file.ToString() : (lObs.id_certificado != 0 ? lObs.id_certificado.ToString() : "")) + ".pdf";
                                link = "<a target=\"_blank\" style =\"padding - right: 10px\" href =\"" + url + "\"><span class=\"text\">" + nom + "</span></a>";
                            }
                            observ += "<b>Documento: </b>" + link;

                            tareaObserv.Item.Add(new Items(observ, ""));
                        }

                    }
                    break;
                #endregion

                #region Revisión Sub-Gerente

                // join tramite in db.SGI_Tramites_Tareas on tarea.id_tramitetarea equals tramite.id_tramitetarea
                // join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                // Nombre_tarea = tarea.nombre_tarea,

                //11	Revisión Sub-Gerente				Revision_SubGerente.aspx	    SGI_Tarea_Revision_SubGerente
                case Constants.ENG_Tipos_Tareas.Revision_SubGerente:
                case Constants.ENG_Tipos_Tareas.Revision_SubGerente2:
                case Constants.ENG_Tipos_Tareas.Revision_SubGerente3:
                    var q_subgerente =
                        (
                            from subGerente in db.SGI_Tarea_Revision_SubGerente
                            join tramite in db.SGI_Tramites_Tareas on subGerente.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on subGerente.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on subGerente.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where subGerente.id_tramitetarea == id_tramitetarea
                                && (
                                    ver_observ_vacias || 
                                    !ver_observ_vacias && 
                                        (
                                            !string.IsNullOrEmpty(subGerente.Observaciones) ||
                                            !string.IsNullOrEmpty(subGerente.observacion_plancheta) ||
                                            !string.IsNullOrEmpty(subGerente.observaciones_contribuyente)
                                        )
                                   )
                            orderby subGerente.id_revision_subGerente descending
                            select new 
                            {
                                ID = subGerente.id_revision_subGerente,
                                Observaciones = subGerente.Observaciones,
                                observacion_plancheta = subGerente.observacion_plancheta,
                                observacion_contribuyente = subGerente.observaciones_contribuyente,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (subGerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (subGerente.LastUpdateDate.HasValue) ? (DateTime)subGerente.LastUpdateDate : subGerente.CreateDate
                            }
                        ).FirstOrDefault();


                    if (q_subgerente != null)
                    {
                        tareaObserv = new ObservacionAnterioresv1();

                        tareaObserv.ID = q_subgerente.ID;
                        tareaObserv.id_tarea = 0;
                        tareaObserv.Nombre_tarea = q_subgerente.Nombre_tarea;
                        tareaObserv.id_tramitetarea = id_tramitetarea;
                        tareaObserv.Observaciones = "";
                        tareaObserv.UsuarioApeNom = q_subgerente.UsuarioApeNom;
                        tareaObserv.Fecha = q_subgerente.Fecha;

                        if (!string.IsNullOrEmpty(q_subgerente.Observaciones))
                            tareaObserv.Item.Add(new Items(q_subgerente.Observaciones + "<br/>", "Observaciones Internas:"));

                        if (!string.IsNullOrEmpty(q_subgerente.observacion_plancheta))
                            tareaObserv.Item.Add(new Items(q_subgerente.observacion_plancheta, "Notas adicionales para la disposición:"));

                        if (!string.IsNullOrEmpty(q_subgerente.observacion_contribuyente))
                            tareaObserv.Item.Add(new Items(q_subgerente.observacion_contribuyente + "<br/>", "Observaciones al Contribuyente:"));

                    }
                    var lstObserSub = (from obs in db.SGI_Tarea_Calificar_ObsDocs
                                            join tdocreq in db.TiposDeDocumentosRequeridos on obs.id_tdocreq equals tdocreq.id_tdocreq
                                            join grup in db.SGI_Tarea_Calificar_ObsGrupo on obs.id_ObsGrupo equals grup.id_ObsGrupo
                                            join file in db.Files on obs.id_file equals file.id_file into fleftjoin
                                            from fil in fleftjoin.DefaultIfEmpty()
                                            join tt_h in db.SGI_Tramites_Tareas_HAB on grup.id_tramitetarea equals tt_h.id_tramitetarea
                                            join calificar in db.SGI_Tarea_Revision_SubGerente on tt_h.id_tramitetarea equals calificar.id_tramitetarea
                                            join tarea in db.ENG_Tareas on tt_h.SGI_Tramites_Tareas.id_tarea equals tarea.id_tarea
                                            join usrAlta in db.SGI_Profiles on calificar.CreateUser equals usrAlta.userid
                                            join usrMod in db.SGI_Profiles on calificar.LastUpdateUser equals usrMod.userid
                                            into pleftjoin
                                            from prof in pleftjoin.DefaultIfEmpty()
                                            where tt_h.id_tramitetarea == id_tramitetarea && obs.Actual == false
                                            select new
                                            {
                                                ID = calificar.id_revision_subGerente,
                                                Nombre_tarea = tarea.nombre_tarea,
                                                UsuarioApeNom = (calificar.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                                Fecha = (calificar.LastUpdateDate.HasValue) ? (DateTime)calificar.LastUpdateDate : calificar.CreateDate,
                                                id_ObsDocs = obs.id_ObsDocs,
                                                id_ObsGrupo = obs.id_ObsGrupo,
                                                nombre_tdocreq = tdocreq.nombre_tdocreq,
                                                id_tdocreq = obs.id_tdocreq,
                                                Observacion_ObsDocs = obs.Observacion_ObsDocs,
                                                Respaldo_ObsDocs = obs.Respaldo_ObsDocs,
                                                id_file = obs.id_file,
                                                id_certificado = obs.id_certificado,
                                                Decido_no_subir = obs.Decido_no_subir.Value
                                            }).ToList();

                    if (lstObserSub.Count == 0)
                    {
                        lstObserSub = (from obs in db.SGI_Tarea_Calificar_ObsDocs
                                       join tdocreq in db.TiposDeDocumentosRequeridos on obs.id_tdocreq equals tdocreq.id_tdocreq
                                       join grup in db.SGI_Tarea_Calificar_ObsGrupo on obs.id_ObsGrupo equals grup.id_ObsGrupo
                                       join file in db.Files on obs.id_file equals file.id_file into fleftjoin
                                       from fil in fleftjoin.DefaultIfEmpty()
                                       join tt_h in db.SGI_Tramites_Tareas_TRANSF on grup.id_tramitetarea equals tt_h.id_tramitetarea
                                       join calificar in db.SGI_Tarea_Revision_SubGerente on tt_h.id_tramitetarea equals calificar.id_tramitetarea
                                       join tarea in db.ENG_Tareas on tt_h.SGI_Tramites_Tareas.id_tarea equals tarea.id_tarea
                                       join usrAlta in db.SGI_Profiles on calificar.CreateUser equals usrAlta.userid
                                       join usrMod in db.SGI_Profiles on calificar.LastUpdateUser equals usrMod.userid
                                       into pleftjoin
                                       from prof in pleftjoin.DefaultIfEmpty()
                                       where tt_h.id_tramitetarea == id_tramitetarea && obs.Actual == false
                                       select new
                                       {
                                           ID = calificar.id_revision_subGerente,
                                           Nombre_tarea = tarea.nombre_tarea,
                                           UsuarioApeNom = (calificar.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                           Fecha = (calificar.LastUpdateDate.HasValue) ? (DateTime)calificar.LastUpdateDate : calificar.CreateDate,
                                           id_ObsDocs = obs.id_ObsDocs,
                                           id_ObsGrupo = obs.id_ObsGrupo,
                                           nombre_tdocreq = tdocreq.nombre_tdocreq,
                                           id_tdocreq = obs.id_tdocreq,
                                           Observacion_ObsDocs = obs.Observacion_ObsDocs,
                                           Respaldo_ObsDocs = obs.Respaldo_ObsDocs,
                                           id_file = obs.id_file,
                                           id_certificado = obs.id_certificado,
                                           Decido_no_subir = obs.Decido_no_subir.Value
                                       }).ToList();
                    }

                    if (lstObserSub.Count > 0)
                    {
                        string observ;
                        foreach (var lObs in lstObserSub)
                        {
                            if (tareaObserv == null)
                            {
                                tareaObserv = new ObservacionAnterioresv1();

                                tareaObserv.ID = lObs.ID;
                                tareaObserv.id_tarea = 0;
                                tareaObserv.Nombre_tarea = lObs.Nombre_tarea;
                                tareaObserv.id_tramitetarea = id_tramitetarea;
                                tareaObserv.Observaciones = "";
                                tareaObserv.UsuarioApeNom = lObs.UsuarioApeNom;
                                tareaObserv.Fecha = lObs.Fecha;
                            }
                            observ = "<b>Tipo de Documento:</b>" + lObs.nombre_tdocreq + "<br/>";
                            observ += "<b>Observación:</b>" + lObs.Observacion_ObsDocs + "<br/>";
                            observ += "<b>Respaldo Normativo:</b>" + lObs.Respaldo_ObsDocs + "<br/>";

                            string link = "";
                            if (lObs.Decido_no_subir)
                                link = "Decide NO subirlo";
                            else {
                                string url = lObs.id_file != null ?
                                string.Format("../../GetPDF/{0}", HttpUtility.UrlEncode(Convert.ToBase64String(Encoding.ASCII.GetBytes(lObs.id_file.ToString()))))
                                : (lObs.id_certificado != 0 ? string.Format("../../ImprimirCertificado/{0}", lObs.id_certificado) : "");
                                string nom = "documento_" + (lObs.id_file != null ? lObs.id_file.ToString() : (lObs.id_certificado != 0 ? lObs.id_certificado.ToString() : "")) + ".pdf";
                                link = "<a target=\"_blank\" style =\"padding - right: 10px\" href =\"" + url + "\"><span class=\"text\">" + nom + "</span></a>";
                            }
                            observ += "<b>Documento: </b>" + link;

                            tareaObserv.Item.Add(new Items(observ, ""));
                        }

                    }

                    break;

                #endregion

                #region Revisión Gerente
                //12	Revisión Gerente					Revision_Gerente.aspx		    SGI_Tarea_Revision_Gerente
                case Constants.ENG_Tipos_Tareas.Revision_Gerente:
                case Constants.ENG_Tipos_Tareas.Revision_Gerente2:
                case Constants.ENG_Tipos_Tareas.Revision_Gerente3:
                    var q_gerente =

                        (
                            from gerente in db.SGI_Tarea_Revision_Gerente
                            join tramite in db.SGI_Tramites_Tareas on gerente.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on gerente.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on gerente.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where gerente.id_tramitetarea == id_tramitetarea
                                && (
                                    ver_observ_vacias ||
                                    !ver_observ_vacias &&
                                        (
                                            !string.IsNullOrEmpty(gerente.Observaciones) ||
                                            !string.IsNullOrEmpty(gerente.observacion_plancheta) ||
                                            !string.IsNullOrEmpty(gerente.observaciones_contribuyente)
                                        )
                                   )

                            orderby gerente.id_revision_gerente descending
                            select new 
                            {
                                ID = gerente.id_revision_gerente,
                                Observaciones = gerente.Observaciones,
                                observacion_plancheta = gerente.observacion_plancheta,
                                observacion_contribuyente = gerente.observaciones_contribuyente,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (gerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (gerente.LastUpdateDate.HasValue) ? (DateTime)gerente.LastUpdateDate : gerente.CreateDate
                            }
                        ).FirstOrDefault();

                    if (q_gerente != null)
                    {
                        tareaObserv = new ObservacionAnterioresv1();

                        tareaObserv.ID = q_gerente.ID;
                        tareaObserv.id_tarea = 0;
                        tareaObserv.Nombre_tarea = q_gerente.Nombre_tarea;
                        tareaObserv.id_tramitetarea = id_tramitetarea;
                        tareaObserv.Observaciones = "";
                        tareaObserv.UsuarioApeNom = q_gerente.UsuarioApeNom;
                        tareaObserv.Fecha = q_gerente.Fecha;

                        if (!string.IsNullOrEmpty(q_gerente.Observaciones))
                            tareaObserv.Item.Add(new Items(q_gerente.Observaciones + "<br/>", "Observaciones Internas:"));

                        if (!string.IsNullOrEmpty(q_gerente.observacion_plancheta))
                            tareaObserv.Item.Add(new Items(q_gerente.observacion_plancheta, "Notas adicionales para la disposición:"));

                        if (!string.IsNullOrEmpty(q_gerente.observacion_contribuyente))
                            tareaObserv.Item.Add(new Items(q_gerente.observacion_contribuyente + "<br/>", "Observaciones al Contribuyente:"));

                    }
                    var lstObserGe = (from obs in db.SGI_Tarea_Calificar_ObsDocs
                                            join tdocreq in db.TiposDeDocumentosRequeridos on obs.id_tdocreq equals tdocreq.id_tdocreq
                                            join grup in db.SGI_Tarea_Calificar_ObsGrupo on obs.id_ObsGrupo equals grup.id_ObsGrupo
                                            join file in db.Files on obs.id_file equals file.id_file into fleftjoin
                                            from fil in fleftjoin.DefaultIfEmpty()
                                            join tt_h in db.SGI_Tramites_Tareas_HAB on grup.id_tramitetarea equals tt_h.id_tramitetarea
                                            join calificar in db.SGI_Tarea_Revision_Gerente on tt_h.id_tramitetarea equals calificar.id_tramitetarea
                                            join tarea in db.ENG_Tareas on tt_h.SGI_Tramites_Tareas.id_tarea equals tarea.id_tarea
                                            join usrAlta in db.SGI_Profiles on calificar.CreateUser equals usrAlta.userid
                                            join usrMod in db.SGI_Profiles on calificar.LastUpdateUser equals usrMod.userid
                                            into pleftjoin
                                            from prof in pleftjoin.DefaultIfEmpty()
                                            where tt_h.id_tramitetarea == id_tramitetarea && obs.Actual == false
                                            select new
                                            {
                                                ID = calificar.id_revision_gerente,
                                                Nombre_tarea = tarea.nombre_tarea,
                                                UsuarioApeNom = (calificar.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                                Fecha = (calificar.LastUpdateDate.HasValue) ? (DateTime)calificar.LastUpdateDate : calificar.CreateDate,
                                                id_ObsDocs = obs.id_ObsDocs,
                                                id_ObsGrupo = obs.id_ObsGrupo,
                                                nombre_tdocreq = tdocreq.nombre_tdocreq,
                                                id_tdocreq = obs.id_tdocreq,
                                                Observacion_ObsDocs = obs.Observacion_ObsDocs,
                                                Respaldo_ObsDocs = obs.Respaldo_ObsDocs,
                                                id_file = obs.id_file,
                                                id_certificado = obs.id_certificado,
                                                Decido_no_subir = obs.Decido_no_subir.Value
                                            }).ToList();

                    if (lstObserGe.Count == 0)
                    {
                        lstObserGe = (from obs in db.SGI_Tarea_Calificar_ObsDocs
                                      join tdocreq in db.TiposDeDocumentosRequeridos on obs.id_tdocreq equals tdocreq.id_tdocreq
                                      join grup in db.SGI_Tarea_Calificar_ObsGrupo on obs.id_ObsGrupo equals grup.id_ObsGrupo
                                      join file in db.Files on obs.id_file equals file.id_file into fleftjoin
                                      from fil in fleftjoin.DefaultIfEmpty()
                                      join tt_h in db.SGI_Tramites_Tareas_TRANSF on grup.id_tramitetarea equals tt_h.id_tramitetarea
                                      join calificar in db.SGI_Tarea_Revision_Gerente on tt_h.id_tramitetarea equals calificar.id_tramitetarea
                                      join tarea in db.ENG_Tareas on tt_h.SGI_Tramites_Tareas.id_tarea equals tarea.id_tarea
                                      join usrAlta in db.SGI_Profiles on calificar.CreateUser equals usrAlta.userid
                                      join usrMod in db.SGI_Profiles on calificar.LastUpdateUser equals usrMod.userid
                                      into pleftjoin
                                      from prof in pleftjoin.DefaultIfEmpty()
                                      where tt_h.id_tramitetarea == id_tramitetarea && obs.Actual == false
                                      select new
                                      {
                                          ID = calificar.id_revision_gerente,
                                          Nombre_tarea = tarea.nombre_tarea,
                                          UsuarioApeNom = (calificar.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                          Fecha = (calificar.LastUpdateDate.HasValue) ? (DateTime)calificar.LastUpdateDate : calificar.CreateDate,
                                          id_ObsDocs = obs.id_ObsDocs,
                                          id_ObsGrupo = obs.id_ObsGrupo,
                                          nombre_tdocreq = tdocreq.nombre_tdocreq,
                                          id_tdocreq = obs.id_tdocreq,
                                          Observacion_ObsDocs = obs.Observacion_ObsDocs,
                                          Respaldo_ObsDocs = obs.Respaldo_ObsDocs,
                                          id_file = obs.id_file,
                                          id_certificado = obs.id_certificado,
                                          Decido_no_subir = obs.Decido_no_subir.Value
                                      }).ToList();
                    }
                    if (lstObserGe.Count > 0)
                    {
                        string observ;
                        foreach (var lObs in lstObserGe)
                        {
                            if (tareaObserv == null)
                            {
                                tareaObserv = new ObservacionAnterioresv1();

                                tareaObserv.ID = lObs.ID;
                                tareaObserv.id_tarea = 0;
                                tareaObserv.Nombre_tarea = lObs.Nombre_tarea;
                                tareaObserv.id_tramitetarea = id_tramitetarea;
                                tareaObserv.Observaciones = "";
                                tareaObserv.UsuarioApeNom = lObs.UsuarioApeNom;
                                tareaObserv.Fecha = lObs.Fecha;
                            }
                            observ = "<b>Tipo de Documento:</b>" + lObs.nombre_tdocreq + "<br/>";
                            observ += "<b>Observación:</b>" + lObs.Observacion_ObsDocs + "<br/>";
                            observ += "<b>Respaldo Normativo:</b>" + lObs.Respaldo_ObsDocs + "<br/>";

                            string link = "";
                            if (lObs.Decido_no_subir)
                                link = "Decide NO subirlo";
                            else {
                                string url = lObs.id_file != null ?
                                string.Format("../../GetPDF/{0}", HttpUtility.UrlEncode(Convert.ToBase64String(Encoding.ASCII.GetBytes(lObs.id_file.ToString()))))
                                : (lObs.id_certificado != 0 ? string.Format("../../ImprimirCertificado/{0}", lObs.id_certificado) : "");
                                string nom = "documento_" + (lObs.id_file != null ? lObs.id_file.ToString() : (lObs.id_certificado != 0 ? lObs.id_certificado.ToString() : "")) + ".pdf";
                                link = "<a target=\"_blank\" style =\"padding - right: 10px\" href =\"" + url + "\"><span class=\"text\">" + nom + "</span></a>";
                            }
                            observ += "<b>Documento: </b>" + link;

                            tareaObserv.Item.Add(new Items(observ, ""));
                        }

                    }

                    break;

                #endregion

                #region Revisión Director
                //13	Revisión Director					Revision_Director.aspx			SGI_Tarea_Revision_Director
                //case (int)SGI.Constants.ENG_Tareas.SSP_Revision_Director:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Revision_Director:
                //    tareaObserv =
                //        (
                //            from director in db.SGI_Tarea_Revision_Director
                //            join tramite in db.SGI_Tramites_Tareas on director.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on director.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on director.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where director.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(director.Observaciones))
                //            orderby director.id_revision_director descending
                //            select new ObservacionAnterioresv1
                //            {
                //                ID = director.id_revision_director,
                //                Observaciones = director.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (director.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (director.LastUpdateDate.HasValue) ? (DateTime)director.LastUpdateDate : director.CreateDate
                //                //CreateDate = director.CreateDate,
                //                //CreateUser = director.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = director.LastUpdateDate,
                //                //LastUpdateUser = director.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Revisión DGHyP
                //14	Revisión DGHyP						Revision_DGHP.aspx				SGI_Tarea_Revision_DGHP
                case Constants.ENG_Tipos_Tareas.Revision_DGHyP:
                case Constants.ENG_Tipos_Tareas.Revision_DGHyP2:
                case Constants.ENG_Tipos_Tareas.Revision_DGHyP3:
                    tareaObserv =
                        (
                            from dghp in db.SGI_Tarea_Revision_DGHP
                            join tramite in db.SGI_Tramites_Tareas on dghp.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on dghp.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on dghp.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where dghp.id_tramitetarea == id_tramitetarea
                            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(dghp.observacion_plancheta))
                            orderby dghp.id_revision_dghp descending
                            select new ObservacionAnterioresv1
                            {
                                ID = dghp.id_revision_dghp,
                                Observaciones = dghp.Observaciones,
                                NotasAdicionales = dghp.observacion_plancheta,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (dghp.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (dghp.LastUpdateDate.HasValue) ? (DateTime)dghp.LastUpdateDate : dghp.CreateDate
                            }
                        ).FirstOrDefault();

                    if (tareaObserv != null)
                    {
                        tareaObserv.Item.Add(new Items(tareaObserv.Observaciones + "<br/>", "Observaciones Internas:"));
                        tareaObserv.Item.Add(new Items(tareaObserv.NotasAdicionales + "<br/>", "Notas adicionales para la disposición:"));
                    }

                    break;

                #endregion

                #region Calificación Técnica y Legal

                //15	Calificación Técnica y Legal		Calificacion_Tecnica_Legal.aspx	SGI_Tarea_Calificacion_Tecnica
                //case (int)SGI.Constants.ENG_Tareas.SSP_Calificacion_Tecnica_Legal:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Calificacion_Tecnica_Legal:
                //    tareaObserv =
                //        (
                //            from calificador_legal in db.SGI_Tarea_Calificacion_Tecnica_Legal
                //            join tramite in db.SGI_Tramites_Tareas on calificador_legal.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on calificador_legal.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on calificador_legal.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where calificador_legal.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(calificador_legal.Observaciones))
                //            orderby calificador_legal.id_cal_tec_leg descending
                //            select new ObservacionAnterioresv1
                //            {
                //                ID = calificador_legal.id_cal_tec_leg,
                //                Observaciones = calificador_legal.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (calificador_legal.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (calificador_legal.LastUpdateDate.HasValue) ? (DateTime)calificador_legal.LastUpdateDate : calificador_legal.CreateDate
                //                //CreateDate = calificador_legal.CreateDate,
                //                //CreateUser = calificador_legal.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = calificador_legal.LastUpdateDate,
                //                //LastUpdateUser = calificador_legal.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Revisión Técnica y Legal

                //16	Revisión Técnica y Legal			Revision_Tecnica_Legal.aspx		SGI_Tarea_Revision_Tecnica_Legal
                //case (int)SGI.Constants.ENG_Tareas.SSP_Revision_Tecnica_Legal:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Revision_Tecnica_Legal:
                //    tareaObserv =
                //        (
                //            from revision_legal in db.SGI_Tarea_Revision_Tecnica_Legal
                //            join tramite in db.SGI_Tramites_Tareas on revision_legal.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on revision_legal.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on revision_legal.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where revision_legal.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(revision_legal.Observaciones))
                //            orderby revision_legal.id_rev_tec_leg descending
                //            select new ObservacionAnterioresv1
                //            {
                //                ID = revision_legal.id_rev_tec_leg,
                //                Observaciones = revision_legal.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (revision_legal.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (revision_legal.LastUpdateDate.HasValue) ? (DateTime)revision_legal.LastUpdateDate : revision_legal.CreateDate
                //                //CreateDate = revision_legal.CreateDate,
                //                //CreateUser = revision_legal.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = revision_legal.LastUpdateDate,
                //                //LastUpdateUser = revision_legal.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Asignar Inspector

                ////18	Asignar Inspector					Asignar_Inspector.aspx			SGI_Tarea_Asignar_Inspector
                //case (int)SGI.Constants.ENG_Tareas.SSP_Asignar_Inspector:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Asignar_Inspector:
                //    tareaObserv =
                //        (
                //            from inspector in db.SGI_Tarea_Asignar_Inspector
                //            join tramite in db.SGI_Tramites_Tareas on inspector.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on inspector.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on inspector.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where inspector.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(inspector.Observaciones))
                //            orderby inspector.id_asignar_inspector descending
                //            select new ObservacionAnterioresv1
                //            {
                //                ID = inspector.id_asignar_inspector,
                //                Observaciones = inspector.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (inspector.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (inspector.LastUpdateDate.HasValue) ? (DateTime)inspector.LastUpdateDate : inspector.CreateDate
                //                //CreateDate = inspector.CreateDate,
                //                //CreateUser = inspector.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = inspector.LastUpdateDate,
                //                //LastUpdateUser = inspector.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Informe Resultado Inspector

                //19	Informe Resultado Inspector			Resultado_Inspector.aspx		SGI_Tarea_Resultado_Inspector
                //case (int)SGI.Constants.ENG_Tareas.SSP_Resultado_Inspector:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Resultado_Inspector:
                //    tareaObserv =
                //        (
                //            from inspector in db.SGI_Tarea_Resultado_Inspector
                //            join tramite in db.SGI_Tramites_Tareas on inspector.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on inspector.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on inspector.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where inspector.id_tramitetarea == id_tramitetarea
                //            && (  ver_observ_vacias || !ver_observ_vacias  && !string.IsNullOrEmpty(inspector.Observaciones)  )
                //            orderby inspector.id_resultado_inspector descending
                //            select new ObservacionAnterioresv1
                //            {
                //                ID = inspector.id_resultado_inspector,
                //                Observaciones = inspector.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (inspector.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres: usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (inspector.LastUpdateDate.HasValue) ? (DateTime)inspector.LastUpdateDate : inspector.CreateDate
                //                //CreateDate = inspector.CreateDate,
                //                //CreateUser = inspector.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = inspector.LastUpdateDate,
                //                //LastUpdateUser = inspector.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Validar Zonificación

                //20	Validar Zonificación				Validar_Zonificacion.aspx		SGI_Tarea_Validar_Zonificacion
                //case (int)SGI.Constants.ENG_Tareas.SSP_Validar_Zonificacion:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Validar_Zonificacion:
                //    tareaObserv =
                //        (
                //            from zonificar in db.SGI_Tarea_Validar_Zonificacion
                //            join tramite in db.SGI_Tramites_Tareas on zonificar.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on zonificar.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on zonificar.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where zonificar.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(zonificar.Observaciones))
                //            orderby zonificar.id_validar_zonificacion descending
                //            select new ObservacionAnterioresv1
                //            {
                //                ID = zonificar.id_validar_zonificacion,
                //                Observaciones = zonificar.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (zonificar.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (zonificar.LastUpdateDate.HasValue) ? (DateTime)zonificar.LastUpdateDate : zonificar.CreateDate
                //                //CreateDate = zonificar.CreateDate,
                //                //CreateUser = zonificar.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = zonificar.LastUpdateDate,
                //                //LastUpdateUser = zonificar.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Revisión Pagos

                //21	Revisión Pagos						Revision_Pagos.aspx				SGI_Tarea_Revision_Pagos
                //case (int)SGI.Constants.ENG_Tareas.SSP_Revision_Pagos:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Revision_Pagos:
                //    tareaObserv =
                //        (
                //            from pagos in db.SGI_Tarea_Revision_Pagos
                //            join tramite in db.SGI_Tramites_Tareas on id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on pagos.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on pagos.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where pagos.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(pagos.Observaciones))
                //            orderby pagos.id_revision_pagos descending
                //            select new ObservacionAnterioresv1
                //            {
                //                ID = pagos.id_revision_pagos,
                //                Observaciones = pagos.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (pagos.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (pagos.LastUpdateDate.HasValue) ? (DateTime)pagos.LastUpdateDate : pagos.CreateDate
                //                //CreateDate = CreateDate,
                //                //CreateUser = CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = LastUpdateDate,
                //                //LastUpdateUser = LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Generar Expediente

                //22	Generar Expediente					Generar_Expediente.aspx			SGI_Tarea_Generar_Expediente
                case Constants.ENG_Tipos_Tareas.Generar_Expediente:
                    tareaObserv =
                        (
                            from expediente in db.SGI_Tarea_Generar_Expediente
                            join tramite in db.SGI_Tramites_Tareas on expediente.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on expediente.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on expediente.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where expediente.id_tramitetarea == id_tramitetarea
                            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(expediente.Observaciones))
                            orderby expediente.id_generar_expediente descending
                            select new ObservacionAnterioresv1
                            {
                                ID = expediente.id_generar_expediente,
                                Observaciones = expediente.Observaciones,
                                //ObservacionesPublica = "",
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (expediente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (expediente.LastUpdateDate.HasValue) ? (DateTime)expediente.LastUpdateDate : expediente.CreateDate
                                //CreateDate = expediente.CreateDate,
                                //CreateUser = expediente.CreateUser,
                                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                                //LastUpdateDate = expediente.LastUpdateDate,
                                //LastUpdateUser = expediente.LastUpdateUser,
                                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                            }
                        ).FirstOrDefault();

                    break;

                #endregion

                #region Entregar Trámite
                //23	Entregar Trámite					Entregar_Tramite.aspx			SGI_Tarea_Entregar_Tramite
                case Constants.ENG_Tipos_Tareas.Entregar_Tramite:
                    tareaObserv =
                        (
                            from entregar in db.SGI_Tarea_Entregar_Tramite
                            join tramite in db.SGI_Tramites_Tareas on entregar.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on entregar.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on entregar.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where entregar.id_tramitetarea == id_tramitetarea
                            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(entregar.Observaciones))
                            orderby entregar.id_entregar_tramite descending
                            select new ObservacionAnterioresv1
                            {
                                ID = entregar.id_entregar_tramite,
                                Observaciones = entregar.Observaciones,
                                //ObservacionesPublica = "",
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (entregar.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (entregar.LastUpdateDate.HasValue) ? (DateTime)entregar.LastUpdateDate : entregar.CreateDate
                                //CreateDate = entregar.CreateDate,
                                //CreateUser = entregar.CreateUser,
                                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                                //LastUpdateDate = entregar.LastUpdateDate,
                                //LastUpdateUser = entregar.LastUpdateUser,
                                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                            }
                        ).FirstOrDefault();

                    break;

                #endregion

                #region Enviar a PVH

                //24	Enviar a PVH						Enviar_PVH.aspx					SGI_Tarea_Enviar_PVH
                //case (int)SGI.Constants.ENG_Tareas.SSP_Enviar_PVH:
                //case (int)SGI.Constants.ENG_Tareas.SCP_Enviar_PVH:
                //    tareaObserv =
                //        (
                //            from pvh in db.SGI_Tarea_Enviar_PVH
                //            join tramite in db.SGI_Tramites_Tareas on pvh.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on pvh.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on pvh.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where pvh.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias && !string.IsNullOrEmpty(pvh.Observaciones))
                //            orderby pvh.id_enviar_pvh descending
                //            select new ObservacionAnterioresv1
                //            {
                //                ID = pvh.id_enviar_pvh,
                //                Observaciones = pvh.Observaciones,
                //                //ObservacionesPublica = "",
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (pvh.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (pvh.LastUpdateDate.HasValue) ? (DateTime)pvh.LastUpdateDate : pvh.CreateDate
                //                //CreateDate = pvh.CreateDate,
                //                //CreateUser = pvh.CreateUser,
                //                //CreateUserApeNom = prof.Apellido + ", " + prof.Nombres,
                //                //LastUpdateDate = pvh.LastUpdateDate,
                //                //LastUpdateUser = pvh.LastUpdateUser,
                //                //LastUpdateUserApeNom = prof.Apellido + ", " + prof.Nombres
                //            }
                //        ).FirstOrDefault();

                //    break;

                #endregion

                #region Carga infromacion
                //case (int)SGI.Constants.ENG_Tareas.CP_Carga_Informacion:
                //    var q_carga =
                //        (
                //            from carga in db.SGI_Tarea_Carga_Tramite
                //            join cpTarea in db.SGI_Tramites_Tareas on carga.id_tramitetarea equals cpTarea.id_tramitetarea
                //            join tarea in db.ENG_Tareas on cpTarea.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on carga.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on carga.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where carga.id_tramitetarea == id_tramitetarea
                //            && (ver_observ_vacias || !ver_observ_vacias &&
                //                                                (
                //                        !string.IsNullOrEmpty(carga.Observaciones) ||
                //                        !string.IsNullOrEmpty(carga.observaciones_contribuyente)
                //                    )

                //            )
                //            orderby carga.id_carga_tramite descending
                //            select new  //ObservacionAnterioresv1
                //            {
                //                ID = carga.id_carga_tramite,
                //                Nombre_tarea = tarea.nombre_tarea,
                //                Observaciones = carga.Observaciones,
                //                Observaciones_contribuyente = carga.observaciones_contribuyente,
                //                UsuarioApeNom = (carga.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (carga.LastUpdateDate.HasValue) ? (DateTime)carga.LastUpdateDate : carga.CreateDate
                //            }
                //        ).FirstOrDefault();

                //    if (q_carga != null)
                //    {
                //        tareaObserv = new ObservacionAnterioresv1();

                //        tareaObserv.ID = q_carga.ID;
                //        tareaObserv.id_tarea = 0;
                //        tareaObserv.Nombre_tarea = q_carga.Nombre_tarea;
                //        tareaObserv.id_tramitetarea = id_tramitetarea;
                //        tareaObserv.Observaciones = "";
                //        tareaObserv.UsuarioApeNom = q_carga.UsuarioApeNom;
                //        tareaObserv.Fecha = q_carga.Fecha;

                //        if (!string.IsNullOrEmpty(q_carga.Observaciones_contribuyente))
                //            tareaObserv.Item.Add(new Items(q_carga.Observaciones_contribuyente + "<br/>", "Observaciones al Contribuyente:"));

                //        if (!string.IsNullOrEmpty(q_carga.Observaciones))
                //            tareaObserv.Item.Add(new Items(q_carga.Observaciones, "Observaciones internas:"));

                //    }

                //    break;

                #endregion

                #region revision Trámite
                //case (int)SGI.Constants.ENG_Tareas.CP_Generar_Expediente:
                //    var q_calificarCP =
                //        (
                //            from revision in db.SGI_Tarea_Generar_Expediente
                //            join tramite in db.SGI_Tramites_Tareas on revision.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on revision.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on revision.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where revision.id_tramitetarea == id_tramitetarea
                //            && (
                //                ver_observ_vacias ||
                //                (!ver_observ_vacias &&
                //                    (
                //                        !string.IsNullOrEmpty(revision.Observaciones)
                //                    )
                //                )
                //               )
                //            orderby revision.id_generar_expediente descending
                //            select new  //ObservacionAnterioresv1
                //            {
                //                ID = revision.id_generar_expediente,
                //                Nombre_tarea = tarea.nombre_tarea,
                //                Observaciones = revision.Observaciones,
                //                UsuarioApeNom = (revision.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (revision.LastUpdateDate.HasValue) ? (DateTime)revision.LastUpdateDate : revision.CreateDate
                //            }
                //        ).FirstOrDefault();

                //    if (q_calificarCP != null)
                //    {
                //        tareaObserv = new ObservacionAnterioresv1();

                //        tareaObserv.ID = q_calificarCP.ID;
                //        tareaObserv.id_tarea = 0;
                //        tareaObserv.Nombre_tarea = q_calificarCP.Nombre_tarea;
                //        tareaObserv.id_tramitetarea = id_tramitetarea;
                //        tareaObserv.Observaciones = "";
                //        tareaObserv.UsuarioApeNom = q_calificarCP.UsuarioApeNom;
                //        tareaObserv.Fecha = q_calificarCP.Fecha;

                //        if (!string.IsNullOrEmpty(q_calificarCP.Observaciones))
                //            tareaObserv.Item.Add(new Items(q_calificarCP.Observaciones, "Observaciones internas:"));
                //    }

                //    break;

                #endregion

                #region Revisión Sub-Gerente CP
                //case (int)SGI.Constants.ENG_Tareas.CP_Fin_Tramite:
                //    var q_subGerenteCP =
                //        (
                //            from fin in db.SGI_Tarea_Fin_Tramite
                //            join tramite in db.SGI_Tramites_Tareas on fin.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on fin.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on fin.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where fin.id_tramitetarea == id_tramitetarea
                //                && (
                //                    ver_observ_vacias ||
                //                    !ver_observ_vacias &&
                //                        (
                //                            !string.IsNullOrEmpty(fin.Observaciones) /*||
                //                            !string.IsNullOrEmpty(fin.Observaciones_contribuyente)*/
                //                        )
                //                   )
                //            orderby fin.id_Fin_Tramite descending
                //            select new  // ObservacionAnterioresv1
                //            {
                //                ID = fin.id_Fin_Tramite,
                //                Observaciones = fin.Observaciones,
                //                //Observaciones_contribuyente = fin.Observaciones_contribuyente,
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (fin.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (fin.LastUpdateDate.HasValue) ? (DateTime)fin.LastUpdateDate : fin.CreateDate
                //            }
                //        ).FirstOrDefault();


                //    if (q_subGerenteCP != null)
                //    {
                //        tareaObserv = new ObservacionAnterioresv1();

                //        tareaObserv.ID = q_subGerenteCP.ID;
                //        tareaObserv.id_tarea = 0;
                //        tareaObserv.Nombre_tarea = q_subGerenteCP.Nombre_tarea;
                //        tareaObserv.id_tramitetarea = id_tramitetarea;
                //        tareaObserv.Observaciones = "";
                //        tareaObserv.UsuarioApeNom = q_subGerenteCP.UsuarioApeNom;
                //        tareaObserv.Fecha = q_subGerenteCP.Fecha;

                //        //if (!string.IsNullOrEmpty(q_subGerenteCP.Observaciones))
                //          //  tareaObserv.Item.Add(new Items(q_subGerenteCP.Observaciones_contribuyente + "<br/>", "Observaciones al Contribuyente:"));

                //        if (!string.IsNullOrEmpty(q_subGerenteCP.Observaciones))
                //            tareaObserv.Item.Add(new Items(q_subGerenteCP.Observaciones, "Observaciones Internas:"));

                //    }
                //    break;

                #endregion

                #region Dictamen Revisión Sub-Gerente

                //case (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_SubGerente:
                //case (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_SubGerente:
                //case (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_SubGerente:
                //    var q_dic_subgerente =
                //        (
                //            from subGerente in db.SGI_Tarea_Dictamen_Revision_SubGerente
                //            join tramite in db.SGI_Tramites_Tareas on subGerente.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on subGerente.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on subGerente.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where subGerente.id_tramitetarea == id_tramitetarea
                //                && (
                //                    ver_observ_vacias || 
                //                    !ver_observ_vacias && 
                //                        (
                //                            !string.IsNullOrEmpty(subGerente.Observaciones)
                //                        )
                //                   )
                //            orderby subGerente.id_Dictamen_Revision_SubGerente descending
                //            select new
                //            {
                //                ID = subGerente.id_Dictamen_Revision_SubGerente,
                //                Observaciones = subGerente.Observaciones,
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (subGerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (subGerente.LastUpdateDate.HasValue) ? (DateTime)subGerente.LastUpdateDate : subGerente.CreateDate
                //            }
                //        ).FirstOrDefault();


                //    if (q_dic_subgerente != null)
                //    {
                //        tareaObserv = new ObservacionAnterioresv1();

                //        tareaObserv.ID = q_dic_subgerente.ID;
                //        tareaObserv.id_tarea = 0;
                //        tareaObserv.Nombre_tarea = q_dic_subgerente.Nombre_tarea;
                //        tareaObserv.id_tramitetarea = id_tramitetarea;
                //        tareaObserv.Observaciones = "";
                //        tareaObserv.UsuarioApeNom = q_dic_subgerente.UsuarioApeNom;
                //        tareaObserv.Fecha = q_dic_subgerente.Fecha;

                //        if (!string.IsNullOrEmpty(q_dic_subgerente.Observaciones))
                //            tareaObserv.Item.Add(new Items(q_dic_subgerente.Observaciones + "<br/>", "Observaciones Internas:"));

                //    }

                //    break;

                #endregion

                #region Dictamen Revisión Gerente

                //case (int)SGI.Constants.ENG_Tareas.TR_Dictamen_Revision_Gerente:
                //case (int)SGI.Constants.ENG_Tareas.ESP_Dictamen_Revision_Gerente:
                //case (int)SGI.Constants.ENG_Tareas.ESPAR_Dictamen_Revision_Gerente:
                //    var q_dict_gerente =

                //        (
                //            from gerente in db.SGI_Tarea_Dictamen_Revision_Gerente
                //            join tramite in db.SGI_Tramites_Tareas on gerente.id_tramitetarea equals tramite.id_tramitetarea
                //            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                //            join usrAlta in db.SGI_Profiles on gerente.CreateUser equals usrAlta.userid
                //            join usrMod in db.SGI_Profiles on gerente.LastUpdateUser equals usrMod.userid
                //            into pleftjoin
                //            from prof in pleftjoin.DefaultIfEmpty()
                //            where gerente.id_tramitetarea == id_tramitetarea
                //                && (
                //                    ver_observ_vacias ||
                //                    !ver_observ_vacias &&
                //                        (
                //                            !string.IsNullOrEmpty(gerente.Observaciones)
                //                        )
                //                   )

                //            orderby gerente.id_Dictamen_Revision_Gerente descending
                //            select new 
                //            {
                //                ID = gerente.id_Dictamen_Revision_Gerente,
                //                Observaciones = gerente.Observaciones,
                //                Nombre_tarea = tarea.nombre_tarea,
                //                UsuarioApeNom = (gerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                //                Fecha = (gerente.LastUpdateDate.HasValue) ? (DateTime)gerente.LastUpdateDate : gerente.CreateDate
                //            }
                //        ).FirstOrDefault();

                //    if (q_dict_gerente != null)
                //    {
                //        tareaObserv = new ObservacionAnterioresv1();

                //        tareaObserv.ID = q_dict_gerente.ID;
                //        tareaObserv.id_tarea = 0;
                //        tareaObserv.Nombre_tarea = q_dict_gerente.Nombre_tarea;
                //        tareaObserv.id_tramitetarea = id_tramitetarea;
                //        tareaObserv.Observaciones = "";
                //        tareaObserv.UsuarioApeNom = q_dict_gerente.UsuarioApeNom;
                //        tareaObserv.Fecha = q_dict_gerente.Fecha;

                //        if (!string.IsNullOrEmpty(q_dict_gerente.Observaciones))
                //            tareaObserv.Item.Add(new Items(q_dict_gerente.Observaciones + "<br/>", "Observaciones Internas:"));
                //    }

                //    break;

                #endregion

                #region Verificacion AVH
                case Constants.ENG_Tipos_Tareas.Verificacion_AVH:
                    var q_verificacion_avh =

                        (
                            from gerente in db.SGI_Tarea_Verificacion_AVH
                            join tramite in db.SGI_Tramites_Tareas on gerente.id_tramitetarea equals tramite.id_tramitetarea
                            join tarea in db.ENG_Tareas on tramite.id_tarea equals tarea.id_tarea
                            join usrAlta in db.SGI_Profiles on gerente.CreateUser equals usrAlta.userid
                            join usrMod in db.SGI_Profiles on gerente.LastUpdateUser equals usrMod.userid
                            into pleftjoin
                            from prof in pleftjoin.DefaultIfEmpty()
                            where gerente.id_tramitetarea == id_tramitetarea
                                && (
                                    ver_observ_vacias ||
                                    !ver_observ_vacias &&
                                        (
                                            !string.IsNullOrEmpty(gerente.Observaciones)
                                        )
                                   )

                            orderby gerente.id_verificacion_AVH descending
                            select new
                            {
                                ID = gerente.id_verificacion_AVH,
                                Observaciones = gerente.Observaciones,
                                Nombre_tarea = tarea.nombre_tarea,
                                UsuarioApeNom = (gerente.LastUpdateUser.HasValue) ? prof.Apellido + ", " + prof.Nombres : usrAlta.Apellido + ", " + usrAlta.Nombres,
                                Fecha = (gerente.LastUpdateDate.HasValue) ? (DateTime)gerente.LastUpdateDate : gerente.CreateDate
                            }
                        ).FirstOrDefault();

                    if (q_verificacion_avh != null)
                    {
                        tareaObserv = new ObservacionAnterioresv1();

                        tareaObserv.ID = q_verificacion_avh.ID;
                        tareaObserv.id_tarea = 0;
                        tareaObserv.Nombre_tarea = q_verificacion_avh.Nombre_tarea;
                        tareaObserv.id_tramitetarea = id_tramitetarea;
                        tareaObserv.Observaciones = "";
                        tareaObserv.UsuarioApeNom = q_verificacion_avh.UsuarioApeNom;
                        tareaObserv.Fecha = q_verificacion_avh.Fecha;

                        if (!string.IsNullOrEmpty(q_verificacion_avh.Observaciones))
                            tareaObserv.Item.Add(new Items(q_verificacion_avh.Observaciones + "<br/>", "Observaciones Internas:"));
                    }

                    break;

                #endregion
                
                default:
                    break;
            }

            if (tareaObserv != null)
            {
                tareaObserv.id_tarea = id_tarea;
                tareaObserv.id_tramitetarea = id_tramitetarea;
                if (tareaObserv.Item.Count == 0)
                    tareaObserv.Item.Add(new Items(tareaObserv.Observaciones, ""));
            }

        }
        catch (Exception ex)
        {
            if (db != null)
                db.Dispose();

            throw ex;
        }
        return tareaObserv;
    }
    #endregion
}
