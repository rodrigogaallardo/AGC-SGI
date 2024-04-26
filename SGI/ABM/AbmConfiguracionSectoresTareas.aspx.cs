using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SGI.Model;
using System.Transactions;
using System.Security.Policy;
using Newtonsoft.Json;

namespace SGI.ABM
{
    public partial class AbmConfiguracionSectoresTareas : BasePage
    {
        DGHP_Entities db = null;

        private string id_object
        {
            get { return ViewState["_id_object"] != null ? ViewState["_id_object"].ToString() : string.Empty; }
            set { ViewState["_id_object"] = value; }
        }

        #region load de pagina
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "init_Js_Datos", "init_Js_Datos();", true);

            }


            if (!IsPostBack)
            {
                db = new DGHP_Entities();
            }
        }
        #endregion

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            this.CargarSectores(ddlSectorB);
            this.CargarTareas(ddlTareaOrigenB);
            this.CargarTareas(ddlTareaDestinoB);
            this.CargarEstados(ddlEstadoB);

            this.CargarSectores(ddlSector);
            this.CargarTareas(ddlTareaOrigen);
            this.CargarTareas(ddlTareaDestino);
            this.CargarEstados(ddlEstado);

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        protected void btnNuevoPase_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarDatos();
                updDatos.Update();
                EjecutarScript(UpdatePanel1, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "finalizarCarga();showfrmError();");

            }
        }

        private void LimpiarDatosBusqueda()
        {
            ddlSectorB.ClearSelection();
            ddlTareaOrigenB.ClearSelection();
            ddlTareaDestinoB.ClearSelection();
          

        }

        private void LimpiarDatos()
        {
            hid_id_SectorTareaReq.Value = "0";
            ddlSector.ClearSelection();
            ddlTareaOrigen.ClearSelection();
            ddlTareaDestino.ClearSelection();

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                BuscarPases();
                updResultados.Update();
                EjecutarScript(UpdatePanel1, "showResultado();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(UpdatePanel1, "showfrmError();");
            }
        }

        private void BuscarPases()
        {
           db = new DGHP_Entities();
            int idSector = 0;
            int.TryParse(ddlSectorB.SelectedItem.Value, out idSector);
            int idTOrigen = 0;
            int.TryParse(ddlTareaOrigenB.SelectedItem.Value, out idTOrigen);
            int idTDestino = 0;
            int.TryParse(ddlTareaDestinoB.SelectedItem.Value, out idTDestino);
            int idEstado = 0;
            int.TryParse(ddlEstadoB.SelectedItem.Value, out idEstado);

            var q = (from pases in db.SGI_Tareas_Pases_Sectores
                     join sector in db.Sectores_SADE on pases.id_sector equals sector.id_sector
                     join tarOri in db.ENG_Tareas on pases.id_tarea_origen equals tarOri.id_tarea
                     join c in db.ENG_Circuitos on tarOri.id_circuito equals c.id_circuito
                     join tar in db.ENG_Tareas on pases.id_tarea_destino equals tar.id_tarea
                     into tarDest
                     from tar in tarDest.DefaultIfEmpty()
                     join c2 in db.ENG_Circuitos on tar.id_circuito equals c2.id_circuito
                     into cir
                     from c2 in cir.DefaultIfEmpty()

                     select new
                     {
                         id_sector = pases.id_sector,
                         id_tarea_origen = pases.id_tarea_origen,
                         id_tarea_destino = (pases.id_tarea_destino == null) ? 0 : pases.id_tarea_destino,
                         Sector = sector.nombre_sector,
                         Tarea_Origen = c.cod_circuito + " - " + tarOri.nombre_tarea,
                         Tarea_Destino = (c2 != null ? c2.cod_circuito + " - " + tar.nombre_tarea : ""),
                         id_tarea_sector = pases.id_tarea_sector,
                         id_estado = pases.id_estado,
                         Estado = pases.SADE_Estados_Expedientes != null ? pases.SADE_Estados_Expedientes.nombre : ""
                     });
            if (idSector > 0)
                q = q.Where(x => x.id_sector == idSector);
            if (idTOrigen > 0)
                q = q.Where(x => x.id_tarea_origen == idTOrigen);
            if (idTDestino > 0)
                q = q.Where(x => x.id_tarea_destino == idTDestino);
            if (idEstado > 0)
                q = q.Where(x => x.id_estado == idEstado);

            grdResultados.DataSource = q.OrderBy(x => x.id_tarea_sector).ToList();
            grdResultados.DataBind();

            pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
            lblCantidadRegistros.Text = grdResultados.Rows.Count.ToString();
            db.Dispose();
        }

        private void CargarSectores(DropDownList listaSector)
        {
            DGHP_Entities db = new DGHP_Entities();

            var lstSectores = (from sector in db.Sectores_SADE
                             select sector
                             ).Distinct().OrderBy(x => x.nombre_sector);

            listaSector.DataTextField = "nombre_sector";
            listaSector.DataValueField = "id_sector";
            listaSector.DataSource = lstSectores.ToList();
            listaSector.DataBind();
            listaSector.Items.Insert(0, "Todos");

            db.Dispose();

        }
        private void CargarTareas(DropDownList listaTareas)
        {
            DGHP_Entities db = new DGHP_Entities();
            var lstTareas = (from tarea in db.ENG_Tareas
                             join c in db.ENG_Circuitos on tarea.id_circuito equals c.id_circuito
                             join rel in db.ENG_Rel_Circuitos_TiposDeTramite on c.id_circuito equals rel.id_circuito
                             select new
                             {
                                 id_tarea = tarea.id_tarea,
                                 nombre_tarea = rel.TipoTramite.descripcion_tipotramite + " - " + c.cod_circuito + " - " + tarea.nombre_tarea
                             }
                             ).Distinct().OrderBy(x => x.nombre_tarea);

            listaTareas.DataTextField = "nombre_tarea";
            listaTareas.DataValueField = "id_tarea";
            listaTareas.DataSource = lstTareas.ToList();
            listaTareas.DataBind();
            listaTareas.Items.Insert(0, "Todos");

            db.Dispose();

        }

        private void CargarEstados(DropDownList listaEstados)
        {
            DGHP_Entities db = new DGHP_Entities();
            //tomas quitar comentario
            var lstTareas = db.SADE_Estados_Expedientes.ToList();

            listaEstados.DataTextField = "nombre";
            listaEstados.DataValueField = "id_estado";
            //tomas quitar comentario
            listaEstados.DataSource = lstTareas.ToList();
            listaEstados.DataBind();
            listaEstados.Items.Insert(0, "Todos");

            db.Dispose();

        }

        private int idSector;
        private int idTareaOrigen;
        private int? idTareaDestino;
        private void CargarDatos(int id_datos)
        {

            db = new DGHP_Entities();
            var dato = db.SGI_Tareas_Pases_Sectores.FirstOrDefault(x => x.id_tarea_sector == id_datos);
            if (dato != null)
            {
                hid_id_SectorTareaReq.Value = id_datos.ToString();
                idSector = dato.id_sector;
                idTareaOrigen = dato.id_tarea_origen;
                idTareaDestino = dato.id_tarea_destino;
                ddlSector.SelectedValue =idSector.ToString();
                ddlTareaOrigen.SelectedValue = idTareaOrigen.ToString();
                if(idTareaDestino!=null)
                    ddlTareaDestino.SelectedValue = idTareaDestino.ToString();
            }
            db.Dispose();
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEditar = (LinkButton)sender;
                int id_datos = int.Parse(btnEditar.CommandArgument);
                LimpiarDatos();
                CargarDatos(id_datos);
                updDatos.Update();
                EjecutarScript(updResultados, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");

            }

        }

        protected void lnkEliminarReq_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;

                LinkButton lnkEditar = (LinkButton)sender;
                int idSectorTarea = int.Parse(lnkEditar.CommandArgument);

                db = new DGHP_Entities();

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        db.SectoresSADE_EliminarSectorTarea(idSectorTarea, userid);

                        Tran.Complete();
                        string script = "$('#frmEliminarLog').modal('show');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MostrarModal", script, true);
                        id_object = idSectorTarea.ToString();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                BuscarPases();
                updResultados.Update();
                this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            db = new DGHP_Entities();
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
                string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();


                int idSectorTarea = Convert.ToInt32(hid_id_SectorTareaReq.Value);
                int idSector = 0;
                int.TryParse(ddlSector.SelectedItem.Value, out idSector);
                int idTOrigen = 0;
                int.TryParse(ddlTareaOrigen.SelectedItem.Value, out idTOrigen);
                int idTDestino = 0;
                int.TryParse(ddlTareaDestino.SelectedItem.Value, out idTDestino);
                int? idEstado = null;
                if (ddlEstado.SelectedIndex >0)
                    idEstado = int.Parse(ddlEstado.SelectedItem.Value);

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        if (idSectorTarea == 0)
                        {
                            idSectorTarea = db.SectoresSADE_GuardarSectorTarea(idSector, idTOrigen, idTDestino, idEstado, userid);
                            SGI_Tareas_Pases_Sectores obj = db.SGI_Tareas_Pases_Sectores.FirstOrDefault(x => x.id_tarea_sector == idSectorTarea);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitantes.Text, "I", 1012);
                        }
                        else
                        {
                            db.SectoresSADE_ActualizarSectorTarea(idSectorTarea, idSector, idTOrigen, idTDestino, idEstado, userid);
                            SGI_Tareas_Pases_Sectores obj = db.SGI_Tareas_Pases_Sectores.FirstOrDefault(x => x.id_tarea_sector == idSectorTarea);
                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitantes.Text, "U", 1012);
                        }
                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                BuscarPases();
                updResultados.Update();
               
                this.EjecutarScript(updBotonesGuardar, "showBusqueda();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updBotonesGuardar, "showfrmError();");
            }
            finally
            {
                db.Dispose();
            }

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            int value = int.Parse(id_object);
            SGI_Tareas_Pases_Sectores obj = db.SGI_Tareas_Pases_Sectores.FirstOrDefault(x => x.id_tarea_sector == value);
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, txtObservacionesSolicitante.Text, "D", 1012);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            int value = int.Parse(id_object);
            SGI_Tareas_Pases_Sectores obj = db.SGI_Tareas_Pases_Sectores.FirstOrDefault(x => x.id_tarea_sector == value);
            Functions.InsertarMovimientoUsuario(userId, DateTime.Now, null, JsonConvert.SerializeObject(obj), url, string.Empty, "D", 1012);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "$('#frmEliminarLog').modal('hide');", true);
        }

    }
}