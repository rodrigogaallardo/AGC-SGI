using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.GestionTramite.Controls.CPadron
{
    public partial class Tab_Ubicaciones : System.Web.UI.UserControl
    {
        public delegate void EventHandlerUbicacionActualizada(object sender, EventArgs e);
        public event EventHandlerUbicacionActualizada UbicacionActualizada;

        
        private int validar_estado
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_validar_estado.Value, out ret);
                return ret;
            }
            set
            {
                hid_validar_estado.Value = value.ToString();
            }

        }

        public bool editar
        {
            get
            {
                bool ret = false;
                ret = hid_editar.Value.Equals("True") ? true : false;
                return ret;
            }
            set
            {
                hid_editar.Value = value.ToString();
            }

        }
        private int id_cpadron
        {
            get
            {
                int ret = 0;
                int.TryParse(hid_id_cpadron.Value, out ret);
                return ret;
            }
            set
            {
                hid_id_cpadron.Value = value.ToString();
            }

        }


        public void CargarDatos(int id_cpadron, int Validar)
        {
            this.id_cpadron = id_cpadron;
            this.validar_estado = Validar;
            //updUbicaciones.Visible = this.editar;
            visUbicaciones.Editable = this.editar;
            visUbicaciones.CargarDatos(id_cpadron);
            
            CargarTablaPlantasHabilitar(id_cpadron);
            
        }
        
        protected void visUbicaciones_EliminarClick(object sender, ucEliminarEventsArgs args)
        {

            btnEliminar_Si.CommandArgument = args.id_cpadronubicacion.ToString();

            this.EjecutarScript(updUbicaciones, "showfrmConfirmarEliminar();");

        }
        protected void visUbicaciones_EditarClick(object sender, ucEditarEventsArgs args)
        {

            this.BuscarUbicacion.id_cpadronUbicacion = args.id_cpadronubicacion;
            this.BuscarUbicacion.validar_estado = this.validar_estado;
            this.BuscarUbicacion.id_cpadron_mod = this.id_cpadron;
            ScriptManager.RegisterStartupScript(updAgregarUbicacion, updAgregarUbicacion.GetType(), "showfrmAgregarUbicacion()", "showfrmAgregarUbicacion();", true);
            this.BuscarUbicacion.editar();


        }
        public void EjecutarScript(UpdatePanel objupd, string script)
        {
            ScriptManager.RegisterClientScriptBlock(objupd, objupd.GetType(), "script", script, true);
        }

        protected void btnEliminar_Si_Click(object sender, EventArgs e)
        {

            DGHP_Entities db = new DGHP_Entities();
            try
            {
                Button btnEliminar_Si = (Button)sender;
                int id_cpadronubicacion = int.Parse(btnEliminar_Si.CommandArgument);

                // Eliminar la ubicación.
                db.CPadron_EliminarUbicacion(this.id_cpadron, id_cpadronubicacion, validar_estado);

                // vuelve a cargar los datos.
                visUbicaciones.CargarDatos(this.id_cpadron);
                Functions.EjecutarScript(updUbicaciones, "hidefrmConfirmarEliminar();");


                // Hace Fire al eneto de Actualización de la úbicación si el mismo está definido.
                if (UbicacionActualizada != null)
                    UbicacionActualizada(sender, e);

            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                Functions.EjecutarScript(updConfirmarEliminar, "hidefrmConfirmarEliminar();showfrmError();");
            }
            finally
            {
                db.Dispose();
            }

        }

        protected void BuscarUbicacion_AgregarUbicacionClick(object sender, ref SGI.GestionTramite.Controls.CPadron.BuscarUbicacion.ucAgregarUbicacionEventsArgs e)
        {


            UpdatePanel upd = e.upd;
            Guid userid = Functions.GetUserId();

            DGHP_Entities db = new DGHP_Entities();

            try
            {
                using (TransactionScope tran = new TransactionScope())
                {

                    try
                    {
                        //Alta de la ubicación
                        ObjectParameter param_id_cpadronubicacion = new ObjectParameter("id_cpadronubicacion", typeof(int));
                        db.CPadron_AgregarUbicacion(this.id_cpadron, e.id_ubicacion, e.id_subtipoubicacion, e.local_subtipoubicacion, e.vDeptoLocalOtros, userid, param_id_cpadronubicacion, validar_estado);
                        int id_cpadronubicacion = Convert.ToInt32(param_id_cpadronubicacion.Value);

                        //Alta de las propiedades horizontales
                        foreach (int id_propiedad_horizontal in e.ids_propiedades_horizontales)
                        {
                            db.CPadron_AgregarPropiedadHorizontal(id_cpadronubicacion, id_propiedad_horizontal, this.id_cpadron, validar_estado);
                        }

                        //Alta de puertas
                        foreach (var puerta in e.Puertas)
                        {
                            db.CPadron_AgregarPuerta(id_cpadronubicacion, puerta.codigo_calle, puerta.NroPuerta, this.id_cpadron, validar_estado);
                        }

                        tran.Complete();

                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        throw ex;
                    }

                }

                visUbicaciones.CargarDatos(this.id_cpadron);


                // Hace Fire al eneto de Actualización de la úbicación si el mismo está definido.
                if (UbicacionActualizada != null)
                    UbicacionActualizada(sender, e);

                Functions.EjecutarScript(upd, "hidefrmAgregarUbicacion();");

            }
            catch (Exception ex)
            {
                e.Cancel = true;
                lblError.Text = Functions.GetErrorMessage(ex);
                Functions.EjecutarScript(upd, "hidefrmAgregarUbicacion();showfrmError();");
            }
            finally
            {
                db.Dispose();
            }

        }

        #region "Plantas a habilitar"

        private void CargarTablaPlantasHabilitar(int id_cpadron)
        {

            DGHP_Entities db = new DGHP_Entities();

            var q = (from tipsec in db.TipoSector
                     join caaplan in db.CPadron_Plantas.Where(x => x.id_cpadron == id_cpadron) on tipsec.Id equals caaplan.id_tiposector into res
                     from caaplan_Empty in res.DefaultIfEmpty()
                     where
                        (
                          (tipsec.Ocultar == null || tipsec.Ocultar.Value == false) ||
                          (tipsec.Ocultar == true && caaplan_Empty.id_tiposector > 0)
                        )
                     orderby tipsec.Id   // Muy importante este orden - no cambiar , alteraria la funcionalidad
                     select new
                     {
                         id_tiposector = tipsec.Id,
                         Seleccionado = (caaplan_Empty.id_tiposector != null ? caaplan_Empty.id_tiposector > 0 : false),
                         tipsec.Descripcion,
                         MuestraCampoAdicional = (tipsec.MuestraCampoAdicional.HasValue ? tipsec.MuestraCampoAdicional.Value : false),
                         detalle = string.IsNullOrEmpty(caaplan_Empty.detalle_cpadrontiposector) ? "" : caaplan_Empty.detalle_cpadrontiposector,
                         TamanoCampoAdicional = (tipsec.TamanoCampoAdicional.HasValue ? tipsec.TamanoCampoAdicional.Value : 0),
                         tipsec.Ocultar,
                         id_cpadrontiposector = caaplan_Empty.id_cpadrontiposector == null ? 0 : caaplan_Empty.id_cpadrontiposector

                     });

            var lstResult = q.ToList();


            // La siguiente lçogica lo que hace antes de lelnar la grilla con la consulta es
            // revisar si existen ya "piso" o la planta "otro", estos dos tipos de sectores están marcados como 
            // que requieren campo adicional, si requieren campo adicional quieren decir que se puede llenar mças de una vez, 
            // lo que hace la rutina es agregar uno vacío para que pueda ser llenado.

            DataTable dt = new DataTable();
            dt.Columns.Add("id_tiposector", typeof(int));
            dt.Columns.Add("Seleccionado", typeof(bool));
            dt.Columns.Add("descripcion", typeof(string));
            dt.Columns.Add("MuestraCampoAdicional", typeof(bool));
            dt.Columns.Add("detalle", typeof(string));
            dt.Columns.Add("TamanoCampoAdicional", typeof(int));
            dt.Columns.Add("Ocultar", typeof(bool));
            dt.Columns.Add("id_cpadrontiposector", typeof(int));

            int id_tiposector_ant = 0;
            bool MuestraCampoAdicional_ant = false;
            string descripcion_ant = "";
            string detalle_ant = "";
            int TamanoCampoAdicional_ant = 0;


            if (lstResult.Count > 0)
            {
                id_tiposector_ant = lstResult[0].id_tiposector;
                MuestraCampoAdicional_ant = lstResult[0].MuestraCampoAdicional;
                descripcion_ant = lstResult[0].Descripcion;
                TamanoCampoAdicional_ant = lstResult[0].TamanoCampoAdicional;
                detalle_ant = lstResult[0].detalle;

                DataRow datarw = dt.NewRow();
                foreach (var item in lstResult)
                {

                    if (item.id_tiposector != id_tiposector_ant)
                    {

                        if (MuestraCampoAdicional_ant && detalle_ant.Length > 0)
                        {
                            datarw = dt.NewRow();
                            datarw[0] = id_tiposector_ant;
                            datarw[1] = false;
                            datarw[2] = descripcion_ant;
                            datarw[3] = MuestraCampoAdicional_ant;
                            datarw[4] = "";
                            datarw[5] = TamanoCampoAdicional_ant;
                            datarw[6] = false;
                            datarw[7] = 0;
                            dt.Rows.Add(datarw);
                        }

                    }


                    datarw = dt.NewRow();

                    datarw[0] = item.id_tiposector;
                    datarw[1] = item.Seleccionado;
                    datarw[2] = item.Descripcion;
                    datarw[3] = item.MuestraCampoAdicional;
                    datarw[4] = item.detalle;
                    datarw[5] = item.TamanoCampoAdicional;
                    datarw[6] = false;
                    datarw[7] = item.id_cpadrontiposector;

                    dt.Rows.Add(datarw);

                    id_tiposector_ant = item.id_tiposector;
                    MuestraCampoAdicional_ant = item.MuestraCampoAdicional;
                    descripcion_ant = item.Descripcion;
                    detalle_ant = item.detalle;
                    TamanoCampoAdicional_ant = item.TamanoCampoAdicional;

                }


                if (MuestraCampoAdicional_ant && detalle_ant.Length > 0)
                {
                    datarw = dt.NewRow();
                    datarw[0] = id_tiposector_ant;
                    datarw[1] = false;
                    datarw[2] = descripcion_ant;
                    datarw[3] = MuestraCampoAdicional_ant;
                    datarw[4] = "";
                    datarw[5] = TamanoCampoAdicional_ant;
                    datarw[6] = false;
                    datarw[7] = 0;
                    dt.Rows.Add(datarw);
                }

            }

            // --Fin de la Logica 
            grdPlantasHabilitar.DataSource = dt;
            grdPlantasHabilitar.DataBind();
            updPlantas.Update();


        }
        protected void chkSeleccionado_CheckedChanged(object sender, EventArgs e)
        {

            CheckBox chk = (CheckBox)sender;
            int filaSeleccionada = ((GridViewRow)chk.Parent.Parent).RowIndex;
            GridViewRow rowActual = (GridViewRow)chk.Parent.Parent;
            GridView grdSeleccionPlantas = (GridView)chk.Parent.Parent.Parent.Parent;

            DataTable dt = new DataTable();
            dt.Columns.Add("id_tiposector", typeof(int));
            dt.Columns.Add("Seleccionado", typeof(bool));
            dt.Columns.Add("descripcion", typeof(string));
            dt.Columns.Add("MuestraCampoAdicional", typeof(bool));
            dt.Columns.Add("detalle", typeof(string));
            dt.Columns.Add("TamanoCampoAdicional", typeof(int));
            dt.Columns.Add("Ocultar", typeof(bool));
            dt.Columns.Add("id_cpadrontiposector", typeof(int));

            int id_cpadrontiposector;
            foreach (GridViewRow row in grdSeleccionPlantas.Rows)
            {
                DataRow datarw;
                datarw = dt.NewRow();

                id_cpadrontiposector = 0;
                int.TryParse(grdSeleccionPlantas.DataKeys[row.RowIndex].Values["id_cpadrontiposector"].ToString(), out id_cpadrontiposector);

                CheckBox chkSeleccionado = (CheckBox)grdSeleccionPlantas.Rows[row.RowIndex].Cells[0].FindControl("chkSeleccionado");
                HiddenField hid_id_tiposector = (HiddenField)grdSeleccionPlantas.Rows[row.RowIndex].Cells[0].FindControl("hid_id_tiposector");
                HiddenField hid_descripcion = (HiddenField)grdSeleccionPlantas.Rows[row.RowIndex].Cells[0].FindControl("hid_descripcion");
                TextBox txtDetalle = (TextBox)grdSeleccionPlantas.Rows[row.RowIndex].Cells[1].FindControl("txtDetalle");

                int id_tiposector = 0;
                int.TryParse(hid_id_tiposector.Value, out id_tiposector);


                datarw[0] = id_tiposector;
                datarw[1] = chkSeleccionado.Checked;
                datarw[2] = hid_descripcion.Value;
                datarw[3] = txtDetalle.Visible;
                datarw[4] = txtDetalle.Text.Trim();
                datarw[5] = txtDetalle.MaxLength;
                datarw[6] = false;
                datarw[7] = id_cpadrontiposector;


                if (row.RowIndex == filaSeleccionada)   // si es la fila seleccionada trabajamos con la misma, sino la agregamos directamente
                {

                    if (chk.Checked)
                    {
                        // Si la fila seleccionada está tildada, la agregamos y además agregamos una nueva basada en esta.
                        dt.Rows.Add(datarw);

                        if (txtDetalle.Visible)     // Si muestra un detalle se agrega otra igual
                        {
                            datarw = dt.NewRow();
                            datarw[0] = id_tiposector;
                            datarw[1] = false;
                            datarw[2] = hid_descripcion.Value;
                            datarw[3] = txtDetalle.Visible;
                            datarw[4] = "";
                            datarw[5] = txtDetalle.MaxLength;
                            datarw[6] = false;
                            datarw[7] = 0;

                            dt.Rows.Add(datarw);
                        }
                    }
                    else
                    {
                        // Si está destildada preguntamos si hay mas de una fila como esta, si la hay no la agregamos
                        if (ContarFilasxPlanta(id_tiposector) <= 1)
                            dt.Rows.Add(datarw);
                    }

                }
                else
                {
                    dt.Rows.Add(datarw);
                }
            }

            grdSeleccionPlantas.DataSource = dt;
            grdSeleccionPlantas.DataBind();
            updPlantas.Update();

        }
        private int ContarFilasxPlanta(int id_tiposector)
        {
            int ret = 0;
            foreach (GridViewRow row in grdPlantasHabilitar.Rows)
            {
                HiddenField hid_id_tiposector = (HiddenField)row.FindControl("hid_id_tiposector");
                int id_tiposector_fila = 0;
                int.TryParse(hid_id_tiposector.Value, out id_tiposector_fila);

                if (id_tiposector.Equals(id_tiposector_fila))
                    ret += 1;
            }

            return ret;
        }
        protected void grdPlantasHabilitar_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                TextBox txtDetalle = (TextBox)e.Row.FindControl("txtDetalle");
                CheckBox chkSeleccionado = (CheckBox)e.Row.FindControl("chkSeleccionado");

                if (txtDetalle.Visible)
                {
                    if (txtDetalle.MaxLength * 10 > 250)
                        txtDetalle.Width = Unit.Parse("250px");
                    else
                        txtDetalle.Width = Unit.Parse(Convert.ToString(txtDetalle.MaxLength * 20) + "px");
                }
            }
        }

        #endregion

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DGHP_Entities db = new DGHP_Entities();

            try
            {
                using (TransactionScope tran = new TransactionScope())
                {
                    try
                    {
                        string lista_activos = "";

                        foreach (GridViewRow row in grdPlantasHabilitar.Rows)
                        {

                            CheckBox chkPlanta = (CheckBox)row.FindControl("chkSeleccionado");
                            TextBox txtDetalle = (TextBox)row.FindControl("txtDetalle");
                            HiddenField hid_id_tiposector = (HiddenField)row.FindControl("hid_id_tiposector");
                            int id_tiposector = Convert.ToInt32(hid_id_tiposector.Value);
                            int id_cpadrontiposector = 0;
                            int.TryParse(grdPlantasHabilitar.DataKeys[row.RowIndex].Values["id_cpadrontiposector"].ToString(), out id_cpadrontiposector);

                            if (chkPlanta.Checked)
                            {
                                if (id_cpadrontiposector > 0)
                                    db.CPadron_ActualizarPlantas(id_cpadrontiposector, id_tiposector, txtDetalle.Text.Trim());
                                else
                                {
                                    ObjectParameter param_id_cpadrontiposector = new ObjectParameter("id_cpadrontiposector", typeof(int));
                                    db.CPadron_AgregarPlanta(this.id_cpadron, id_tiposector, txtDetalle.Text.Trim(), param_id_cpadrontiposector, validar_estado);
                                    id_cpadrontiposector = Convert.ToInt32(param_id_cpadrontiposector.Value);
                                }
                                if (string.IsNullOrEmpty(lista_activos))
                                    lista_activos = id_cpadrontiposector.ToString();
                                else
                                    lista_activos = lista_activos + "," + id_cpadrontiposector; // en esta lista quedan los registros que quedan en tabla

                            }


                        }
                        if (!string.IsNullOrEmpty(lista_activos))
                            db.CPadron_EliminarPlantas(this.id_cpadron, lista_activos, validar_estado);
                        else {
                            db.CPadron_EliminarPlantas(this.id_cpadron, "0", validar_estado);
                        }

                        tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        throw ex;
                    }

                }
                // Hace Fire al eneto de Actualización de la úbicación si el mismo está definido.
                if (UbicacionActualizada != null)
                    UbicacionActualizada(sender, e);

            }
            catch (Exception ex)
            {
                lblError.Text = Functions.GetErrorMessage(ex);
                Functions.EjecutarScript(updBotonesGuardar, "showfrmError();");
            }
            finally
            {
                db.Dispose();
            }
        }

    }
}