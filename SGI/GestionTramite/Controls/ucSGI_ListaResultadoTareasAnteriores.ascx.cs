using System;
using System.Collections.Generic;
using System.Linq;
using SGI.Model;

namespace SGI.GestionTramite.Controls
{
    public partial class ucSGI_ListaResultadoTareasAnteriores : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private DGHP_Entities db = null;
        public void LoadData(int id_solicitud, int id_tramitetarea)
        {
            LoadData((int)Constants.GruposDeTramite.HAB, id_solicitud, id_tramitetarea);
        }

        public void LoadData(int id_grupotramite, int id_solicitud, int id_tramitetarea)
        {
            this.db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            SGI_Tramites_Tareas tt = db.SGI_Tramites_Tareas.FirstOrDefault(x => x.id_tramitetarea == id_tramitetarea);
            buscarResultadoTareasAnteriores(id_grupotramite, id_solicitud, id_tramitetarea, tt.id_tarea);

            this.db.Dispose();
        }


        private void buscarResultadoTareasAnteriores(int id_grupotramite, int id_solicitud, int id_tramitetarea, int id_tarea)
        {
            Constants.ENG_Tareas eng_tarea;
            Enum.TryParse(id_tarea.ToString(), out eng_tarea);
            int[] tareas_resultadoAnterior = TramiteTareaAnteriores.Dependencias_Tarea_ResultadosAnteriores(eng_tarea);

            if (tareas_resultadoAnterior == null || tareas_resultadoAnterior.Length == 0)
                return;

            List<TramiteTareaAnteriores> list_tramite_tarea = TramiteTareaAnteriores.BuscarUltimoTramiteTareaPorTarea(id_grupotramite, id_solicitud, id_tramitetarea, tareas_resultadoAnterior);
            
            TramiteTareaAnteriores ultimaTarea = list_tramite_tarea.OrderByDescending(x => x.id_tramitetarea).FirstOrDefault();
            list_tramite_tarea.Remove(ultimaTarea);
            if (list_tramite_tarea.Count > 0)
            {
                int[] id_tt = list_tramite_tarea.Select(x => x.id_tramitetarea).ToArray();

                var q =
                    (
                        from tt in db.SGI_Tramites_Tareas
                        join resul in db.ENG_Resultados on tt.id_resultado equals resul.id_resultado
                        join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                        join usr in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals usr.userid
                        where id_tt.Contains(tt.id_tramitetarea)
                        select new
                        {
                            id_tramitetarea = tt.id_tramitetarea,
                            id_tarea = tt.id_tarea,
                            id_resultado = tt.id_resultado,
                            FechaCierre_tramitetarea = tt.FechaCierre_tramitetarea,
                            nombre_tarea = tarea.nombre_tarea,
                            nombre_resultado = resul.nombre_resultado,
                            usuario = usr.Apellido + ", " + usr.Nombres

                        }
                    ).ToList();

                grd_doc_adj_anteriores.DataSource = q;
                grd_doc_adj_anteriores.DataBind();
            }

            int[] tipoTarea = new int[6]
                       {
                            (int)Constants.ENG_Tipos_Tareas_New.Revision_Gerente,
                            (int)Constants.ENG_Tipos_Tareas_New.Revision_SubGerente,
                            (int)Constants.ENG_Tipos_Tareas_New.Revision_Gerente_1er,
                            (int)Constants.ENG_Tipos_Tareas_New.Revision_SubGerente_1er,
                            (int)Constants.ENG_Tipos_Tareas_New.Revision_Gerente_2,
                            (int)Constants.ENG_Tipos_Tareas_New.Revision_SubGerente_2
                       };
            var query = (from tt in db.SGI_Tramites_Tareas
                         join tth in db.SGI_Tramites_Tareas_HAB on tt.id_tramitetarea equals tth.id_tramitetarea
                         join resul in db.ENG_Resultados on tt.id_resultado equals resul.id_resultado
                         join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                         join usr in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals usr.userid
                         where tipoTarea.Contains(tarea.id_tipo_tarea.Value) &&
                              tth.id_solicitud == id_solicitud
                         select new
                         {
                             id_tramitetarea = tt.id_tramitetarea,
                             id_tarea = tt.id_tarea,
                             id_resultado = tt.id_resultado,
                             FechaCierre_tramitetarea = tt.FechaCierre_tramitetarea,
                             nombre_tarea = tarea.nombre_tarea,
                             nombre_resultado = resul.nombre_resultado,
                             usuario = usr.Apellido + ", " + usr.Nombres

                         }
                        ).Union(from tt in db.SGI_Tramites_Tareas
                                join tth in db.SGI_Tramites_Tareas_TRANSF on tt.id_tramitetarea equals tth.id_tramitetarea
                                join resul in db.ENG_Resultados on tt.id_resultado equals resul.id_resultado
                                join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                                join usr in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals usr.userid
                                where tipoTarea.Contains(tarea.id_tipo_tarea.Value) &&
                                     tth.id_solicitud == id_solicitud
                                select new
                                {
                                    id_tramitetarea = tt.id_tramitetarea,
                                    id_tarea = tt.id_tarea,
                                    id_resultado = tt.id_resultado,
                                    FechaCierre_tramitetarea = tt.FechaCierre_tramitetarea,
                                    nombre_tarea = tarea.nombre_tarea,
                                    nombre_resultado = resul.nombre_resultado,
                                    usuario = usr.Apellido + ", " + usr.Nombres

                                }
                        );

            if (ultimaTarea != null)
            {
                query = query.Union(
                        from tt in db.SGI_Tramites_Tareas
                        join resul in db.ENG_Resultados on tt.id_resultado equals resul.id_resultado
                        join tarea in db.ENG_Tareas on tt.id_tarea equals tarea.id_tarea
                        join usr in db.SGI_Profiles on tt.UsuarioAsignado_tramitetarea equals usr.userid
                        where tt.id_tramitetarea == ultimaTarea.id_tramitetarea
                        select new
                        {
                            id_tramitetarea = tt.id_tramitetarea,
                            id_tarea = tt.id_tarea,
                            id_resultado = tt.id_resultado,
                            FechaCierre_tramitetarea = tt.FechaCierre_tramitetarea,
                            nombre_tarea = tarea.nombre_tarea,
                            nombre_resultado = resul.nombre_resultado,
                            usuario = usr.Apellido + ", " + usr.Nombres

                        });
            }

            grdTareaAnterior.DataSource = query.OrderBy(x => x.id_tramitetarea).ToList();
            grdTareaAnterior.DataBind();

            pnllistResulTareaAnterior.Visible = (grd_doc_adj_anteriores.Rows.Count > 0 );
            pnllistaResulUltimaTarea.Visible = (grdTareaAnterior.Rows.Count > 0);
        }

        
        #region Atributos

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
                    _titulo = "Resultado Tareas Anteriores";
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
                hid_lta_collapse.Value = _collapse.ToString().ToLower();
            }
        }

        private bool _collapse2;
        public bool Collapse2
        {
            get
            {
                return _collapse2;
            }
            set
            {
                _collapse2 = value;
                hid_lta_collapse2.Value = _collapse2.ToString().ToLower();
            }
        }
        #endregion


    }

}