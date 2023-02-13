using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.Model;

namespace SGI.GestionTramite.Controls
{
    
    public partial class ucObservacionesTarea : System.Web.UI.UserControl
    {
       
        #region "Propiedades"

        private bool _Enabled;
        private string _Text;
        private Unit _Height;

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(true),
        Description("Devuelve/Establece el estado de habilitación del control.")]
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                txtObservaciones.ReadOnly = !_Enabled;
            }
        }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("Devuelve/Establece el texto de la observación.")]
        public string Text
        {
            get
            {
                _Text = txtObservaciones.Text;
                return _Text;
            }
            set
            {
                 _Text = value;
                 txtObservaciones.Text = _Text;
            }
        }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(80),
        Description("Devuelve/Establece el estado de habilitación del control.")]
        public Unit Height
        {
            get
            {
                _Height = txtObservaciones.Height;
                return _Height;
            }
            set
            {
                _Height = value;
                txtObservaciones.Height = _Height;
            }
        }

        private string _ValidationGroup = "";
        public string ValidationGroup
        {
            get
            {
                return _ValidationGroup;
            }
            set
            {
                _ValidationGroup = value;
            }
        }

        private bool _observacionRequerida = false;
        public bool ValidarRequerido
        {
            get
            {
                return _observacionRequerida;
            }
            set
            {
                _observacionRequerida = value;
                if (_observacionRequerida)
                {
                    rfv_txtObservaciones.ValidationGroup = _ValidationGroup;
                }
                else
                {
                    rfv_txtObservaciones.ValidationGroup = "noValidar";
                }

            }
        }

        private string _labelObservacion = "";
        public string LabelObservacion
        {
            get
            {
                return _labelObservacion;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _labelObservacion = value;
                else
                    _labelObservacion = "Observaciones";
                lblObservacion.Text = _labelObservacion;
            }
        }

        public UpdatePanelUpdateMode UpdateMode
        {
            get { return this.updPnlObservaciones.UpdateMode; }
            set { this.updPnlObservaciones.UpdateMode = value; }
        }

        public void Update()
        {
            this.updPnlObservaciones.Update();
        }
        #endregion


        public int LoadDataContribuyente(int id_grupotramite, int id_solicitud, int id_tramite_tarea)
        {
            int id_tramite_tarea_calificar = 0;

            IniciarEntity();

            int[] tareas = new int[1] { (int)Constants.ENG_Tareas.SSP_Calificar };

            List<TramiteTareaAnteriores> list_tramite_tarea = TramiteTareaAnteriores.BuscarUltimoTramiteTareaPorTarea(id_grupotramite, id_solicitud, id_tramite_tarea + 1, tareas);

            if (list_tramite_tarea.Count > 0)
            {
                id_tramite_tarea_calificar = list_tramite_tarea[0].id_tramitetarea;

                CargarObservacionContribuyente(id_tramite_tarea_calificar);
            }

            FinalizarEntity();

            return id_tramite_tarea_calificar;
        }

        private void CargarObservacionContribuyente(int id_tramite_tarea)
        {

            var q =
                (
                    from calif in db.SGI_Tarea_Calificar
                    where calif.id_tramitetarea == id_tramite_tarea
                    orderby calif.id_calificar descending
                    select new
                    {
                        calif.Observaciones_contribuyente
                    }

                ).ToList().FirstOrDefault();


            if (q != null)
            {
                txtObservaciones.Text = q.Observaciones_contribuyente.Trim();
            }
            else
            {
                txtObservaciones.Text = "";
            }

        }


        public string LoadObservacionesInternas(int id_tramite_tarea, int id_solicitud)
        {
            this.db = new DGHP_Entities();

            try
            {
                List<int> listaTramiteTareas = ListarTramites(id_solicitud);
                
                if (listaTramiteTareas.Any())
                {
                    return CargarObservacionInternaAnterior(id_tramite_tarea, listaTramiteTareas);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.db != null)
                    this.db.Dispose();
            }

        }

        private List<int> ListarTramites(int id_solicitud) 
        {
            var listaTramitesTareasTRANSF = (from ttt in db.SGI_Tramites_Tareas_TRANSF
                                            where (ttt.id_solicitud == id_solicitud)
                                            select ttt.id_tramitetarea).ToList();

            if (!listaTramitesTareasTRANSF.Any()) 
            {
                var listaTramitesTareasHAB = (from ttt in db.SGI_Tramites_Tareas_HAB
                                           where (ttt.id_solicitud == id_solicitud)
                                           select ttt.id_tramitetarea).ToList();

                return listaTramitesTareasHAB;
            }

            return listaTramitesTareasTRANSF;
        } 

        private void CargarObservacionInterna(int id_tramite_tarea)
        {

            var q =
                (
                    from calif in db.SGI_Tarea_Calificar
                    where calif.id_tramitetarea == id_tramite_tarea
                    orderby calif.id_calificar descending
                    select new
                    {
                        calif.Observaciones
                    }

                ).ToList().FirstOrDefault();


            if (q != null)
            {
                txtObservaciones.Text = q.Observaciones.Trim();
            }
            else
            {
                txtObservaciones.Text = "";
            }

        }

        public string CargarObservacionInternaAnterior(int id_tramite_tarea, List<int> listaTramiteTareas)
        {
            string observacionesInternas = "";
            var listaObservacionesInternas = (from calif in db.SGI_Tarea_Calificar
                     where (listaTramiteTareas.Contains(calif.id_tramitetarea) && calif.id_tramitetarea <= id_tramite_tarea)
                     orderby calif.id_calificar ascending
                     select new 
                     {
                         calif.Observaciones_Internas                         
                     }).ToList();


            if (!listaObservacionesInternas.Any()) return observacionesInternas;

            foreach (var item in listaObservacionesInternas)
            {
                observacionesInternas += item.Observaciones_Internas + "\n"; 
            }

            return observacionesInternas;
        }

        public int GuardarObservacionContribuyente(int id_grupotramite, int id_solicitud, int id_tramitetarea, System.Guid userId)
        {
            IniciarEntity();

            // guardar la observacion en la ultima tarea del calificador
            int id_tramite_tarea_calificar = 0;
            int id_calificar = 0;

            IniciarEntity();

            int[] tareas = new int[1] { (int)Constants.ENG_Tareas.SSP_Calificar };

            List<TramiteTareaAnteriores> list_tramite_tarea = TramiteTareaAnteriores.BuscarUltimoTramiteTareaPorTarea(id_grupotramite, id_solicitud, id_tramitetarea + 1, tareas);

            if (list_tramite_tarea.Count > 0)
            {
                id_tramite_tarea_calificar = list_tramite_tarea[0].id_tramitetarea;

                var calificar =
                    (
                        from calif in db.SGI_Tarea_Calificar
                        where calif.id_tramitetarea == id_tramite_tarea_calificar
                        orderby calif.id_calificar descending
                        select new
                        {
                            calif.id_calificar
                        }
                    ).ToList().FirstOrDefault();

                id_calificar = calificar.id_calificar;



                db.SGI_Observacion_Contribuyente_Actualizar(id_calificar, txtObservaciones.Text.Trim(), userId);
            }

            FinalizarEntity();

            return id_tramite_tarea_calificar;
        }

        #region entity

        private DGHP_Entities db = null;

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
                this.db.Dispose();
        }

        #endregion

        public string LoadObservacionesPlancheta(int id_tramite_tarea, int id_solicitud)
        {
            this.db = new DGHP_Entities();

            try
            {
                List<int> listaTramiteTareas = ListarTramites(id_solicitud);

                if (listaTramiteTareas.Any())
                {
                    return CargarObservacionesPlanchetaAnteriores(id_tramite_tarea, listaTramiteTareas);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.db != null)
                    this.db.Dispose();
            }

        }

        public string CargarObservacionesPlanchetaAnteriores(int id_tramite_tarea, List<int> listaTramiteTareas)
        {
            string observacionesPlancheta = "";
            var listaObservacionesPlancheta = (from calif in db.SGI_Tarea_Revision_Gerente
                                              where (listaTramiteTareas.Contains(calif.id_tramitetarea) && calif.id_tramitetarea <= id_tramite_tarea)
                                              orderby calif.id_revision_gerente ascending
                                              select new
                                              {
                                                  calif.observacion_plancheta
                                              }).ToList();

            if (!listaObservacionesPlancheta.Any()) return observacionesPlancheta;

            foreach (var item in listaObservacionesPlancheta)
            {
                observacionesPlancheta += item.observacion_plancheta + "\n";
            }

            return observacionesPlancheta.Trim();
        }
    }
}