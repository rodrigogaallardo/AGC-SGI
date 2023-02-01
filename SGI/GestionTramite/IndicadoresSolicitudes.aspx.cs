using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SGI.Model;
using System.Transactions;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Entity.SqlServer;

namespace SGI.GestionTramite
{
    public partial class IndicadoresSolicitudes : BasePage
    {
        DGHP_Entities db = null;

        #region load de pagina
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "inicializar_controles", "inicializar_controles();", true);
                ScriptManager.RegisterStartupScript(updpnlBloque3, updpnlBloque3.GetType(), "inicializar_controlesB3", "inicializar_controlesB3();", true);
            }

            if (!IsPostBack)
            {
                db = new DGHP_Entities();
                db.Database.CommandTimeout = 300;
                //LimpiarControles();
                CargarTipoTramite();
            }
        }
        #endregion

        private void LimpiarControles()
        {
            ddlBusTipoTramite.ClearSelection();
            ddlBusTipoExpediente.ClearSelection();
            ddlBusSubtipoExpediente.ClearSelection();
            ddlBusCircuito.ClearSelection();

            ddlBusTipoTramiteRev.ClearSelection();
            ddlBusTipoExpedienteRev.ClearSelection();
            ddlBusSubtipoExpedienteRev.ClearSelection();
            ddlBusObservado.ClearSelection();
            txtBusFechaRevisionMes.Text = "";
            txtBusFechaRevisionAnio.Text = "";

            ddlBusCircuito3.ClearSelection();
            updpnlBuscar.Update();
            updpnlBloque2.Update();
            updpnlBloque3.Update();
        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }
        private void CargarTipoTramite()
        {
            db.Database.CommandTimeout = 300;
            var lst_tramite = (from tip in db.TipoTramite
                               where tip.id_tipotramite < 11
                               select tip);

            ddlBusTipoTramite.DataSource = lst_tramite.ToList();
            ddlBusTipoTramite.DataTextField = "descripcion_tipotramite";
            ddlBusTipoTramite.DataValueField = "id_tipotramite";
            ddlBusTipoTramite.DataBind();

            ddlBusTipoTramite.Items.Insert(0, new ListItem("Todos", "99"));

            updpnlBuscar.Update();

            CargarTipoExpediente(99, 1);

            ddlBusTipoTramiteRev.DataSource = lst_tramite.ToList();
            ddlBusTipoTramiteRev.DataTextField = "descripcion_tipotramite";
            ddlBusTipoTramiteRev.DataValueField = "id_tipotramite";
            ddlBusTipoTramiteRev.DataBind();

            ddlBusTipoTramiteRev.Items.Insert(0, new ListItem("Todos", "99"));

            updpnlBloque2.Update();

            CargarTipoExpediente(99, 2);

            var lst_circuitos = (from cir in db.ENG_Circuitos
                                 select new
                                 {
                                     cod_circuito = cir.cod_circuito + " - " + cir.nombre_circuito,
                                     id_circuito = cir.id_circuito
                                 }).Distinct();

            ddlBusCircuito3.DataSource = lst_circuitos.ToList();
            ddlBusCircuito3.DataTextField = "cod_circuito";
            ddlBusCircuito3.DataValueField = "id_circuito";
            ddlBusCircuito3.DataBind();

            ddlBusCircuito3.Items.Insert(0, new ListItem("Todos", "99"));

            updpnlBloque3.Update();
        }

        private void CargarTipoExpediente(int id_tipotramite, int lista)
        {
            db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var lst_expediente = (from exp in db.TipoExpediente
                                  join rel in db.ENG_Rel_Circuitos_TiposDeTramite on exp.id_tipoexpediente equals rel.id_tipoexpediente
                                  where rel.id_tipotramite == id_tipotramite
                                  select exp).Distinct();

            if (lista == 1)
            {
                ddlBusTipoExpediente.DataSource = lst_expediente.ToList();
                ddlBusTipoExpediente.DataTextField = "descripcion_tipoexpediente";
                ddlBusTipoExpediente.DataValueField = "id_tipoexpediente";
                ddlBusTipoExpediente.DataBind();

                ddlBusTipoExpediente.Items.Insert(0, new ListItem("Todos", "99"));

                updpnlBuscar.Update();

                CargarSubtipoExpediente(99, 1);
            }
            if (lista == 2)
            {
                ddlBusTipoExpedienteRev.DataSource = lst_expediente.ToList();
                ddlBusTipoExpedienteRev.DataTextField = "descripcion_tipoexpediente";
                ddlBusTipoExpedienteRev.DataValueField = "id_tipoexpediente";
                ddlBusTipoExpedienteRev.DataBind();

                ddlBusTipoExpedienteRev.Items.Insert(0, new ListItem("Todos", "99"));

                updpnlBloque2.Update();

                CargarSubtipoExpediente(99, 2);
            }
        }
        private void CargarSubtipoExpediente(int id_tipoexpediente, int lista)
        {
            db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var lst_subtipoExpediente = (from sub in db.SubtipoExpediente
                                         join rel in db.ENG_Rel_Circuitos_TiposDeTramite on sub.id_subtipoexpediente equals rel.id_subtipoexpediente
                                         where rel.id_tipoexpediente == id_tipoexpediente
                                         select sub).Distinct();
            if (lista == 1)
            {
                ddlBusSubtipoExpediente.DataSource = lst_subtipoExpediente.ToList();
                ddlBusSubtipoExpediente.DataTextField = "descripcion_subtipoexpediente";
                ddlBusSubtipoExpediente.DataValueField = "id_subtipoexpediente";
                ddlBusSubtipoExpediente.DataBind();

                ddlBusSubtipoExpediente.Items.Insert(0, new ListItem("Todos", "99"));

                updpnlBuscar.Update();

                CargarCircuitos(99, 1);
            }
            if (lista == 2)
            {
                ddlBusSubtipoExpedienteRev.DataSource = lst_subtipoExpediente.ToList();
                ddlBusSubtipoExpedienteRev.DataTextField = "descripcion_subtipoexpediente";
                ddlBusSubtipoExpedienteRev.DataValueField = "id_subtipoexpediente";
                ddlBusSubtipoExpedienteRev.DataBind();

                ddlBusSubtipoExpedienteRev.Items.Insert(0, new ListItem("Todos", "99"));

                updpnlBloque2.Update();

                CargarCircuitos(99, 2);
            }
        }
        private void CargarCircuitos(int id_subtipoexpediente, int lista)
        {
            db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            int id_tipotramite = 0;
            int id_tipoexpediente = 0;

            if (lista == 1)
            {
                id_tipotramite = int.Parse(ddlBusTipoTramite.SelectedValue);
                id_tipoexpediente = int.Parse(ddlBusTipoExpediente.SelectedValue);
            }
            if (lista == 2)
            {
                id_tipotramite = int.Parse(ddlBusTipoTramiteRev.SelectedValue);
                id_tipoexpediente = int.Parse(ddlBusTipoExpedienteRev.SelectedValue);
            }

            var lst_circuitos = (from cir in db.ENG_Circuitos
                                 join rel in db.ENG_Rel_Circuitos_TiposDeTramite on cir.id_circuito equals rel.id_circuito
                                 where rel.id_tipotramite == id_tipotramite
                                     && rel.id_tipoexpediente == id_tipoexpediente
                                     && rel.id_subtipoexpediente == id_subtipoexpediente
                                 select cir).Distinct();

            if (lista == 1)
            {
                ddlBusCircuito.DataSource = lst_circuitos.ToList();
                ddlBusCircuito.DataTextField = "cod_circuito";
                ddlBusCircuito.DataValueField = "id_circuito";
                ddlBusCircuito.DataBind();

                ddlBusCircuito.Items.Insert(0, new ListItem("Todos", "99"));

                updpnlBuscar.Update();
            }
            if (lista == 2)
            {
                ddlBusCircuitoRev.DataSource = lst_circuitos.ToList();
                ddlBusCircuitoRev.DataTextField = "cod_circuito";
                ddlBusCircuitoRev.DataValueField = "id_circuito";
                ddlBusCircuitoRev.DataBind();

                ddlBusCircuitoRev.Items.Insert(0, new ListItem("Todos", "99"));

                updpnlBloque2.Update();
            }
        }

        private int BusFechaRevAnio = 0;
        private int BusFechaRevMes = 0;

        //Filtros
        private int BusTipoTramite = 99;
        private int BusTipoExpediente = 99;
        private int BusSubtipoExpediente = 99;
        private int BusCircuito = 99;

        private int BusTipoTramiteRev = 99;
        private int BusTipoExpedienteRev = 99;
        private int BusSubtipoExpedienteRev = 99;
        private int BusCircuitoRev = 99;

        private int BusCircuito3 = 99;

        private void ValidarBuscar()
        {

            int idAux = 0;
            this.BusFechaRevMes = 0;
            this.BusFechaRevAnio = 0;

            this.BusTipoTramite = 99;
            this.BusTipoExpediente = 99;
            this.BusSubtipoExpediente = 99;
            this.BusCircuito = 99;

            this.BusTipoTramite = int.Parse(ddlBusTipoTramite.SelectedValue);
            this.BusTipoExpediente = int.Parse(ddlBusTipoExpediente.SelectedValue);
            this.BusSubtipoExpediente = int.Parse(ddlBusSubtipoExpediente.SelectedValue);
            this.BusCircuito = int.Parse(ddlBusCircuito.SelectedValue);


        }
        private void ValidarBuscar2()
        {

            int idAux = 0;
            this.BusFechaRevMes = 0;
            this.BusFechaRevAnio = 0;

            this.BusTipoTramiteRev = 99;
            this.BusTipoExpedienteRev = 99;
            this.BusSubtipoExpedienteRev = 99;
            this.BusCircuitoRev = 99;

            this.BusTipoTramiteRev = int.Parse(ddlBusTipoTramiteRev.SelectedValue);
            this.BusTipoExpedienteRev = int.Parse(ddlBusTipoExpedienteRev.SelectedValue);
            this.BusSubtipoExpedienteRev = int.Parse(ddlBusSubtipoExpedienteRev.SelectedValue);
            this.BusCircuitoRev = int.Parse(ddlBusCircuitoRev.SelectedValue);

            //Busqueda / Filtrar Mes y Año Revision
            if (txtBusFechaRevisionMes.Text.Trim().Length > 0)
                this.BusFechaRevMes = int.Parse(txtBusFechaRevisionMes.Text.Trim());

            if (this.BusFechaRevMes > 12)
                throw new Exception("El mes no puede ser mayor a 12 (Diciembre)");

            if (txtBusFechaRevisionAnio.Text.Trim().Length > 0)
            {

                if (txtBusFechaRevisionAnio.Text.Trim().Length < 4)
                    throw new Exception("Debe ingresar 4 digitos para el año ej: " + DateTime.Now.Year);
                else
                    this.BusFechaRevAnio = int.Parse(txtBusFechaRevisionAnio.Text.Trim());

            }

            if (this.BusFechaRevAnio > DateTime.Now.Year)
                throw new Exception("El año ingresado no puede ser superior al presente.");


            if (txtBusFechaRevisionMes.Text.Trim().Length > 0 && txtBusFechaRevisionAnio.Text.Trim().Length < 4)
                throw new Exception("Para que el filtro sea preciso debe ingresar el año correspondiente al mes que ingreso.");

        }

        private void ValidarBuscarB3()
        {
            this.BusCircuito3 = 99;
            this.BusCircuito3 = int.Parse(ddlBusCircuito3.SelectedValue);
            if (this.BusCircuito3 == 99)
                throw new Exception("Debe seleccionar el circuito.");
        }

        private List<clsItemSolicitudes> BuscarConsultaExcel(int startRowIndex, int maximumRows, out int totalRowCount)
        {

            Guid userid = Functions.GetUserId();

            totalRowCount = 0;
            EE_Entities ee = new EE_Entities();
            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;
            ee.Database.CommandTimeout = 300;

            IQueryable<clsItemSolicitudes> q = null;

            var qHB = (from tra in db.SGI_Tramites_Tareas
                       join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea //into trahdf
                       //from trah in trahdf.DefaultIfEmpty()

                       join ssit in db.SSIT_Solicitudes on trah.id_solicitud equals ssit.id_solicitud into ssitdf
                       from ssit in ssitdf.DefaultIfEmpty()

                       join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into resdf
                       from res in resdf.DefaultIfEmpty()

                       join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tardf
                       from tar in tardf.DefaultIfEmpty()

                       join rel in db.ENG_Rel_Circuitos_TiposDeTramite on tar.id_circuito equals rel.id_circuito// into reldf

                       join usr in db.aspnet_Users on tra.UsuarioAsignado_tramitetarea equals usr.UserId into usrdf
                       from usr in usrdf.DefaultIfEmpty()

                       where (this.BusTipoTramite > -1 && this.BusTipoTramite < 99 ? rel.id_tipotramite == this.BusTipoTramite : (trah.id_solicitud > 0))
                       select new clsItemSolicitudes
                       {
                           id_solicitud = trah.id_solicitud,
                           nombre_tarea = tar.nombre_tarea,
                           nombre_resultado = res.nombre_resultado,
                           Fecha_Inicio = tra.FechaInicio_tramitetarea,
                           Fecha_Asignacion = tra.FechaAsignacion_tramtietarea,
                           Fecha_Cierre = tra.FechaCierre_tramitetarea,
                           Dif_ini_cierre = (int?)null,//((TimeSpan?)(tra.FechaInicio_tramitetarea - tra.FechaCierre_tramitetarea)).Days ,
                           Dif_asig_cierre = (int?)null,
                           UserName = usr.UserName,
                           superficie = "0",
                           numero_dispo_GEDO = "",
                           id_paquete = tra.id_tarea == 27 ? db.SGI_Tarea_Generar_Expediente_Procesos.Where(x => x.id_tramitetarea == tra.id_tramitetarea).FirstOrDefault().id_paquete : (int?)null,
                           id_tarea = tra.id_tarea,
                           id_tipotramite = rel.id_tipotramite,
                           id_tipoexpediente = rel.id_tipoexpediente,
                           id_subtipoexpediente = rel.id_subtipoexpediente,
                           id_circuito = tar.id_circuito,
                           desc_circuito = tar.ENG_Circuitos.nombre_circuito,
                           cod_circuito = tar.ENG_Circuitos.cod_circuito,
                       });
            if (qHB != null && qHB.Count() > 0)
                q = qHB;

            if (this.BusTipoTramite == (int)Constants.TipoDeTramite.Transferencia || this.BusTipoTramite == 99)
            {
                var qTR = (from tra in db.SGI_Tramites_Tareas
                           join trah in db.SGI_Tramites_Tareas_TRANSF on tra.id_tramitetarea equals trah.id_tramitetarea //into trahdf
                           //from trah in trahdf.DefaultIfEmpty()

                           join ssit in db.Transf_Solicitudes on trah.id_solicitud equals ssit.id_solicitud into ssitdf
                           from ssit in ssitdf.DefaultIfEmpty()

                           join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into resdf
                           from res in resdf.DefaultIfEmpty()

                           join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tardf
                           from tar in tardf.DefaultIfEmpty()

                           join rel in db.ENG_Rel_Circuitos_TiposDeTramite on tar.id_circuito equals rel.id_circuito //into reldf
                           //from rel in reldf.DefaultIfEmpty()

                           join usr in db.aspnet_Users on tra.UsuarioAsignado_tramitetarea equals usr.UserId into usrdf
                           from usr in usrdf.DefaultIfEmpty()
                           where rel.id_tipotramite == this.BusTipoTramite
                           select new clsItemSolicitudes
                           {
                               id_solicitud = trah.id_solicitud,
                               nombre_tarea = tar.nombre_tarea,
                               nombre_resultado = res.nombre_resultado,
                               Fecha_Inicio = tra.FechaInicio_tramitetarea,
                               Fecha_Asignacion = tra.FechaAsignacion_tramtietarea,
                               Fecha_Cierre = tra.FechaCierre_tramitetarea,
                               Dif_ini_cierre = (int?)null,//((TimeSpan?)(tra.FechaInicio_tramitetarea - tra.FechaCierre_tramitetarea)).Days ,
                               Dif_asig_cierre = (int?)null,
                               UserName = usr.UserName,
                               superficie = "0",
                               numero_dispo_GEDO = "",
                               id_paquete = tra.id_tarea == 27 ? tra.SGI_Tarea_Generar_Expediente_Procesos.Select(x => x.id_paquete).FirstOrDefault() : (int?)null,
                               id_tarea = tra.id_tarea,
                               id_tipotramite = rel.id_tipotramite,
                               id_tipoexpediente = rel.id_tipoexpediente,
                               id_subtipoexpediente = rel.id_subtipoexpediente,
                               id_circuito = tar.id_circuito,
                               desc_circuito = tar.ENG_Circuitos.nombre_circuito,
                               cod_circuito = tar.ENG_Circuitos.cod_circuito,
                           });
                if (q == null || q.Count() < 1)
                    q = qTR;
                else
                    q.Union(qTR);
            }

            if (this.BusTipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron || this.BusTipoTramite == 99)
            {
                var qCP = (from tra in db.SGI_Tramites_Tareas
                           join trah in db.SGI_Tramites_Tareas_CPADRON on tra.id_tramitetarea equals trah.id_tramitetarea //into trahdf
                           //from trah in trahdf.DefaultIfEmpty()

                           join ssit in db.CPadron_Solicitudes on trah.id_cpadron equals ssit.id_cpadron into ssitdf
                           from ssit in ssitdf.DefaultIfEmpty()

                           join res in db.ENG_Resultados on tra.id_resultado equals res.id_resultado into resdf
                           from res in resdf.DefaultIfEmpty()

                           join tar in db.ENG_Tareas on tra.id_tarea equals tar.id_tarea into tardf
                           from tar in tardf.DefaultIfEmpty()

                           join rel in db.ENG_Rel_Circuitos_TiposDeTramite on tar.id_circuito equals rel.id_circuito //into reldf
                           //from rel in reldf.DefaultIfEmpty()

                           join usr in db.aspnet_Users on tra.UsuarioAsignado_tramitetarea equals usr.UserId into usrdf
                           from usr in usrdf.DefaultIfEmpty()
                           where rel.id_tipotramite == this.BusTipoTramite
                           select new clsItemSolicitudes
                           {
                               id_solicitud = trah.id_cpadron,
                               nombre_tarea = tar.nombre_tarea,
                               nombre_resultado = res.nombre_resultado,
                               Fecha_Inicio = tra.FechaInicio_tramitetarea,
                               Fecha_Asignacion = tra.FechaAsignacion_tramtietarea,
                               Fecha_Cierre = tra.FechaCierre_tramitetarea,
                               Dif_ini_cierre = (int?)null,//((TimeSpan?)(tra.FechaInicio_tramitetarea - tra.FechaCierre_tramitetarea)).Days ,
                               Dif_asig_cierre = (int?)null,
                               UserName = usr.UserName,
                               superficie = "0",
                               numero_dispo_GEDO = "",
                               id_paquete = tra.id_tarea == 27 ? tra.SGI_Tarea_Generar_Expediente_Procesos.Select(x => x.id_paquete).FirstOrDefault() : (int?)null,
                               id_tarea = tra.id_tarea,
                               id_tipotramite = rel.id_tipotramite,
                               id_tipoexpediente = rel.id_tipoexpediente,
                               id_subtipoexpediente = rel.id_subtipoexpediente,
                               id_circuito = tar.id_circuito,
                               desc_circuito = tar.ENG_Circuitos.nombre_circuito,
                               cod_circuito = tar.ENG_Circuitos.cod_circuito,
                           });

                if (q == null || q.Count() < 1)
                    q = qCP;
                else
                    q.Union(qCP);
            }


            if (this.BusTipoTramite == 99)
                q = q.Where(x => x.id_tipotramite != 5);

            if (this.BusTipoTramite > -1 && this.BusTipoTramite < 99)
                q = q.Where(x => x.id_tipotramite == this.BusTipoTramite);

            if (this.BusTipoExpediente > -1 && this.BusTipoExpediente < 99)
                q = q.Where(x => x.id_tipoexpediente == this.BusTipoExpediente);

            if (this.BusSubtipoExpediente > -1 && this.BusSubtipoExpediente < 99)
                q = q.Where(x => x.id_subtipoexpediente == this.BusSubtipoExpediente);

            if (this.BusCircuito > -1 && this.BusCircuito < 99)
                q = q.Where(x => x.id_circuito == this.BusCircuito);

            totalRowCount = q.Count();

            q = q.OrderBy(o => o.id_solicitud).ThenBy(x => x.Fecha_Inicio).Skip(startRowIndex).Take(maximumRows);

            var rq = q.ToList();

            foreach (clsItemSolicitudes sit in rq)
            {
                if (sit.id_tarea == 27 && sit.id_paquete != null && sit.id_paquete > 0)
                {
                    var gedo = (from fir in ee.wsEE_TareasDocumentos.Where(x => x.id_paquete == sit.id_paquete)
                                where fir.firmado_en_SADE == true
                                select new clsItemSolicitudes
                                {
                                    numero_dispo_GEDO = fir.numeroGEDO,
                                }).FirstOrDefault();

                    if (gedo != null)
                        sit.numero_dispo_GEDO = gedo.numero_dispo_GEDO;
                }
                sit.superficie = (from enc in db.Encomienda
                                  join d in db.Encomienda_DatosLocal on enc.id_encomienda equals d.id_encomienda
                                  join encsol in db.Encomienda_SSIT_Solicitudes on enc.id_encomienda equals encsol.id_encomienda
                                  where encsol.id_solicitud == sit.id_solicitud && enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                                  orderby enc.id_encomienda descending
                                  select (d.superficie_cubierta_dl + d.superficie_descubierta_dl).ToString()
                                ).FirstOrDefault();
            }

            db.Dispose();
            ee.Dispose();
            return rq;

        }


        private List<clsItemSolicitudes> BuscarConsultaSGOExcel(int startRowIndex, int maximumRows, out int totalRowCount)
        {

            Guid userid = Functions.GetUserId();

            totalRowCount = 0;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var Asignacion = (from tra in db.SGI_Tramites_Tareas

                              join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea into trahdf
                              from trah in trahdf.DefaultIfEmpty()

                              join ssit in db.SSIT_Solicitudes on trah.id_solicitud equals ssit.id_solicitud

                              where (tra.id_tarea == 301 || tra.id_tarea == 302)
                              select new clsItemSolicitudes
                              {
                                  id_solicitud = trah.id_solicitud,
                                  Fecha_Inicio_Asignacion_Calificador = tra.FechaCierre_tramitetarea,
                                  Fecha_inicio_ULTIMA_Revision_HyP = (DateTime?)null,
                                  Fecha_Inicio = tra.FechaInicio_tramitetarea, //Calif_Ini
                                  Fecha_Asignacion = (DateTime?)null,
                                  Fecha_Cierre = tra.FechaCierre_tramitetarea, //Calif_fin
                                  Asignacion_Calificador = tra.id_tarea == 301 ? "Subgerente" :
                                                           tra.id_tarea == 302 ? "Gerente" : "-",
                                  Observado = "",
                                  superficie = "0",
                                  id_tarea = tra.id_tarea,
                                  id_tipotramite = 99,
                                  id_tipoexpediente = 99,
                                  id_subtipoexpediente = 99,
                                  id_circuito = 99,
                                  desc_circuito = "",
                                  cod_circuito = "",
                              });

            var revrhyp = (from tra in db.SGI_Tramites_Tareas
                           join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea

                           where tra.id_tarea == 306 && (tra.FechaInicio_tramitetarea.Month == this.BusFechaRevMes || this.BusFechaRevMes == 0)
                           select new clsItemSolicitudes
                           {
                               id_solicitud = trah.id_solicitud,
                               Fecha_Inicio_Asignacion_Calificador = (DateTime?)null,
                               Fecha_inicio_ULTIMA_Revision_HyP = tra.FechaInicio_tramitetarea,
                               Fecha_Inicio = tra.FechaInicio_tramitetarea, //rhyp_ini
                               Fecha_Asignacion = tra.FechaAsignacion_tramtietarea, //rhyp_Asignacion 
                               Fecha_Cierre = tra.FechaCierre_tramitetarea, //rhyp_Cierre 
                               Asignacion_Calificador = "",
                               Observado = "",
                               superficie = "",
                               id_tarea = tra.id_tarea,
                               id_tipotramite = 99,
                               id_tipoexpediente = 99,
                               id_subtipoexpediente = 99,
                               id_circuito = 99,
                               desc_circuito = "",
                               cod_circuito = "",
                           }).Distinct();

            var revobs = (from tra in db.SGI_Tramites_Tareas
                          join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea

                          where tra.id_tarea == 311
                          select new clsItemSolicitudes
                          {
                              id_solicitud = trah.id_solicitud,
                              Fecha_Inicio_Asignacion_Calificador = (DateTime?)null,
                              Fecha_inicio_ULTIMA_Revision_HyP = (DateTime?)null,
                              Fecha_Inicio = tra.FechaInicio_tramitetarea, //obs_ini
                              Fecha_Asignacion = tra.FechaAsignacion_tramtietarea, //obs_Asignacion
                              Fecha_Cierre = tra.FechaCierre_tramitetarea, //obs_Cierre 
                              Asignacion_Calificador = "",
                              Observado = "",
                              superficie = "",
                              id_tarea = tra.id_tarea,
                              id_tipotramite = 99,
                              id_tipoexpediente = 99,
                              id_subtipoexpediente = 99,
                              id_circuito = 99,
                              desc_circuito = "",
                              cod_circuito = "",
                          }).Distinct();

            var revrfd = (from tra in db.SGI_Tramites_Tareas
                          join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea

                          where tra.id_tarea == 308
                          select new clsItemSolicitudes
                          {
                              id_solicitud = trah.id_solicitud,
                              Fecha_Inicio_Asignacion_Calificador = (DateTime?)null,
                              Fecha_inicio_ULTIMA_Revision_HyP = (DateTime?)null,
                              Fecha_Inicio = tra.FechaInicio_tramitetarea, //rfd_ini
                              Fecha_Asignacion = tra.FechaAsignacion_tramtietarea, //rfd_Asignacion 
                              Fecha_Cierre = tra.FechaCierre_tramitetarea, //rfd_Cierre 
                              Asignacion_Calificador = "",
                              Observado = "",
                              superficie = "",
                              id_tarea = tra.id_tarea,
                              id_tipotramite = 99,
                              id_tipoexpediente = 99,
                              id_subtipoexpediente = 99,
                              id_circuito = 99,
                              desc_circuito = "",
                              cod_circuito = "",
                          }).Distinct();

            var revee = (from tra in db.SGI_Tramites_Tareas
                         join trah in db.SGI_Tramites_Tareas_HAB on tra.id_tramitetarea equals trah.id_tramitetarea

                         where tra.id_tarea == 307
                         select new clsItemSolicitudes
                         {
                             id_solicitud = trah.id_solicitud,
                             Fecha_Inicio_Asignacion_Calificador = (DateTime?)null,
                             Fecha_inicio_ULTIMA_Revision_HyP = (DateTime?)null,
                             Fecha_Inicio = tra.FechaInicio_tramitetarea, //rfd_ini
                             Fecha_Asignacion = tra.FechaAsignacion_tramtietarea, //rfd_Asignacion 
                             Fecha_Cierre = tra.FechaCierre_tramitetarea, //rfd_Cierre 
                             Asignacion_Calificador = "",
                             Observado = "",
                             superficie = "",
                             id_tarea = tra.id_tarea,
                             id_tipotramite = 99,
                             id_tipoexpediente = 99,
                             id_subtipoexpediente = 99,
                             id_circuito = 99,
                             desc_circuito = "",
                             cod_circuito = "",
                         }).Distinct();

            var q = (from asig in Asignacion
                     join rhyp in revrhyp on asig.id_solicitud equals rhyp.id_solicitud into rhypdf
                     from rhyp in rhypdf.DefaultIfEmpty()

                     join obs in revobs on asig.id_solicitud equals obs.id_solicitud into obsdf
                     from obs in obsdf.DefaultIfEmpty()

                     join rfd in revrfd on asig.id_solicitud equals rfd.id_solicitud into rfddf
                     from rfd in rfddf.DefaultIfEmpty()

                     join eed in revee on asig.id_solicitud equals eed.id_solicitud into eedf
                     from eed in eedf.DefaultIfEmpty()

                     join tar in db.ENG_Tareas on asig.id_tarea equals tar.id_tarea into tardf
                     from tar in tardf.DefaultIfEmpty()

                     join rel in db.ENG_Rel_Circuitos_TiposDeTramite on tar.id_circuito equals rel.id_circuito// into reldf

                     select new clsItemSolicitudes
                     {
                         id_solicitud = asig.id_solicitud,
                         Fecha_Inicio_Asignacion_Calificador = asig.Fecha_Inicio_Asignacion_Calificador,
                         Fecha_inicio_ULTIMA_Revision_HyP = rhyp.Fecha_inicio_ULTIMA_Revision_HyP,
                         Fecha_Inicio = rhyp.Fecha_Inicio,
                         Fecha_Asignacion = (DateTime?)null,
                         Fecha_Cierre = rfd.Fecha_Cierre,
                         Asignacion_Calificador = asig.Asignacion_Calificador,
                         Observado = obs.Fecha_Inicio != null ? "Si" : obs.Fecha_Inicio == null ? "No" : "-",
                         superficie = asig.superficie,
                         id_tarea = asig.id_tarea,
                         id_tipotramite = asig.id_tipotramite,
                         id_tipoexpediente = rel.id_tipoexpediente,
                         id_subtipoexpediente = rel.id_subtipoexpediente,
                         id_circuito = tar.id_circuito,
                         desc_circuito = tar.ENG_Circuitos.nombre_circuito,
                         cod_circuito = tar.ENG_Circuitos.cod_circuito,
                     }).Distinct();

            if (ddlBusObservado.SelectedValue == "1")
                q = q.Where(x => x.Observado.Equals("Si"));

            if (ddlBusObservado.SelectedValue == "0")
                q = q.Where(x => x.Observado.Equals("No"));

            if (this.BusTipoTramiteRev == 99)
                q = q.Where(x => x.id_tipotramite != 5);

            if (this.BusTipoTramiteRev > -1 && this.BusTipoTramiteRev < 99)
                q = q.Where(x => x.id_tipotramite == this.BusTipoTramiteRev);

            if (this.BusTipoExpedienteRev > -1 && this.BusTipoExpedienteRev < 99)
                q = q.Where(x => x.id_tipoexpediente == this.BusTipoExpedienteRev);

            if (this.BusSubtipoExpedienteRev > -1 && this.BusSubtipoExpedienteRev < 99)
                q = q.Where(x => x.id_subtipoexpediente == this.BusSubtipoExpedienteRev);

            if (this.BusCircuitoRev > -1 && this.BusCircuitoRev < 99)
                q = q.Where(x => x.id_circuito == this.BusCircuitoRev);

            totalRowCount = q.Count();

            q = q.OrderBy(o => o.id_solicitud).Skip(startRowIndex).Take(maximumRows);

            var rq = q.ToList();
            foreach (clsItemSolicitudes sit in rq)
            {
                sit.superficie = (from enc in db.Encomienda
                                  join d in db.Encomienda_DatosLocal on enc.id_encomienda equals d.id_encomienda
                                  join encsol in db.Encomienda_SSIT_Solicitudes on enc.id_encomienda equals encsol.id_encomienda
                                  where encsol.id_solicitud == sit.id_solicitud && enc.id_estado == (int)Constants.Encomienda_Estados.Aprobada_por_el_consejo
                                  orderby enc.id_encomienda descending
                                  select (d.superficie_cubierta_dl + d.superficie_descubierta_dl).ToString()
                                ).First();
            }

            db.Dispose();
            return rq;

        }

        private List<clsItemHoja1> BuscarConsultaHoja1Excel(int startRowIndex, int maximumRows, out int totalRowCount)
        {

            Guid userid = Functions.GetUserId();

            totalRowCount = 0;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var q = (from e in db.Estadisticas_Hoja1
                     select new 
                     {
                         cod_circuito_origen = e.cod_circuito_origen,
                         CreateDate = e.CreateDate,
                         id_circuito_actual = e.id_circuito_actual.Value,
                         id_solicitud = e.id_solicitud.Value,
                         observaciones = e.Observaciones,
                         observador = e.observador
                     });

            q = q.Where(x => x.id_circuito_actual == this.BusCircuito3);

            totalRowCount = q.Count();

            q = q.OrderBy(o => o.CreateDate).Skip(startRowIndex).Take(maximumRows);
            var rq = q.ToList();
            db.Dispose();
            List<clsItemHoja1> result = new List<clsItemHoja1>();
            clsItemHoja1 h;
            foreach (var item in rq)
            {
                h = new clsItemHoja1();
                h.CreateDate = item.CreateDate != null ? item.CreateDate.Value.ToString("dd/MM/yyyy") : "";
                h.cod_circuito_origen = item.cod_circuito_origen;
                h.id_circuito_actual = item.id_circuito_actual;
                h.id_solicitud = item.id_solicitud;
                h.observaciones = item.observaciones;
                h.observador = item.observador;
                result.Add(h);
            }
            return result;

        }

        private List<clsItemHoja2> BuscarConsultaHoja2Excel(int startRowIndex, int maximumRows, out int totalRowCount)
        {

            Guid userid = Functions.GetUserId();

            totalRowCount = 0;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var q = (from e in db.Estadisticas_Hoja2
                     select new 
                     {
                         CreateDate = e.CreateDate,
                         id_circuito_actual = e.id_circuito_actual.Value,
                         id_solicitud = e.id_solicitud.Value,
                         observador = e.observador,
                         FechaAsignacion_tramitetarea = e.FechaAsignacion_tramitetarea,
                         FechaCierre_tramitetarea = e.FechaCierre_tramitetarea,
                         FechaInicio_tramitetarea = e.FechaInicio_tramitetarea,
                         Observaciones_contribuyente = e.Observaciones_contribuyente,  
                         Documento_Requerido = e.nombre_tdocreq,
                         Documento_Requerido_Observacion = e.Observacion_ObsDocs
                     });

            q = q.Where(x => x.id_circuito_actual == this.BusCircuito3);

            totalRowCount = q.Count();

            q = q.OrderBy(o => o.CreateDate).Skip(startRowIndex).Take(maximumRows);

            var rq = q.ToList();

            db.Dispose();

            List<clsItemHoja2> result = new List<clsItemHoja2>();
            clsItemHoja2 h;
            foreach (var item in rq)
            {
                h = new clsItemHoja2();
                h.CreateDate = item.CreateDate != null ? item.CreateDate.Value.ToString("dd/MM/yyyy") : "";
                h.id_circuito_actual = item.id_circuito_actual;
                h.id_solicitud = item.id_solicitud;
                h.observador = item.observador;
                h.FechaAsignacion_tramitetarea = item.FechaAsignacion_tramitetarea != null ? item.FechaAsignacion_tramitetarea.Value.ToString("dd/MM/yyyy") : "";
                h.FechaCierre_tramitetarea = item.FechaCierre_tramitetarea != null ? item.FechaCierre_tramitetarea.Value.ToString("dd/MM/yyyy") : "";
                h.FechaInicio_tramitetarea = item.FechaInicio_tramitetarea != null ? item.FechaInicio_tramitetarea.Value.ToString("dd/MM/yyyy") : "";
                h.Observaciones_contribuyente = item.Observaciones_contribuyente;
                h.Documento_Requerido = item.Documento_Requerido;
                h.Documento_Requerido_Observacion = item.Documento_Requerido_Observacion;

                result.Add(h);
            }
            return result;
        }

        private List<clsItemHoja3> BuscarConsultaHoja3Excel(int startRowIndex, int maximumRows, out int totalRowCount)
        {

            Guid userid = Functions.GetUserId();

            totalRowCount = 0;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var q = (from e in db.Estadisticas_Hoja3
                     select new 
                     {
                         CreateDate = e.CreateDate,
                         id_circuito_actual = e.id_circuito_actual.Value,
                         id_solicitud = e.id_solicitud.Value,
                         Circuito_Origen = e.Circuito_Origen,
                         Circuito_Tarea = e.Circuito_Tarea,
                         Dif_asig_cierre = e.Dif_asig_cierre,
                         Dif_ini_cierre = e.Dif_ini_cierre,
                         Fecha = e.Fecha,
                         fecha_asignacion = e.fecha_asignacion,
                         fecha_cierre = e.fecha_cierre,
                         fecha_inicio = e.fecha_inicio,
                         hora_asignacion = e.hora_asignacion,
                         hora_cierre = e.hora_cierre,
                         hora_inicio = e.hora_inicio,
                         nombre_proxima_tarea = e.nombre_proxima_tarea,
                         nombre_resultado = e.nombre_resultado,
                         nombre_tarea = e.nombre_tarea,
                         NroDisposicionSADE = e.NroDisposicionSADE,
                         superficie = e.superficie,
                         username = e.username
                     });

            q = q.Where(x => x.id_circuito_actual == this.BusCircuito3);

            totalRowCount = q.Count();

            q = q.OrderBy(o => o.id_solicitud).ThenBy(o => o.fecha_inicio).ThenBy(o => o.hora_inicio)
                .ThenBy(o => o.fecha_asignacion).ThenBy(o => o.hora_asignacion).ThenBy(o => o.fecha_cierre).ThenBy(o => o.hora_cierre)
                .Skip(startRowIndex).Take(maximumRows);

            var rq = q.ToList();

            db.Dispose();
            List<clsItemHoja3> result = new List<clsItemHoja3>();
            clsItemHoja3 h;
            foreach (var item in rq)
            {
                h = new clsItemHoja3();
                h.CreateDate = item.CreateDate != null ? item.CreateDate.Value.ToString("dd/MM/yyyy") : "";
                h.id_circuito_actual = item.id_circuito_actual;
                h.id_solicitud = item.id_solicitud;
                h.Circuito_Origen = item.Circuito_Origen;
                h.Circuito_Tarea = item.Circuito_Tarea;
                h.Dif_asig_cierre = item.Dif_asig_cierre;
                h.Dif_ini_cierre = item.Dif_ini_cierre;
                h.Fecha = item.Fecha != null ? item.Fecha.Value.ToString("dd/MM/yyyy") : "";
                h.fecha_asignacion = item.fecha_asignacion != null ? item.fecha_asignacion.Value.ToString("dd/MM/yyyy") : "";
                h.fecha_cierre = item.fecha_cierre != null ? item.fecha_cierre.Value.ToString("dd/MM/yyyy") : "";
                h.fecha_inicio = item.fecha_inicio != null ? item.fecha_inicio.Value.ToString("dd/MM/yyyy") : "";
                h.hora_asignacion = item.hora_asignacion;
                h.hora_cierre = item.hora_cierre;
                h.hora_inicio = item.hora_inicio;
                h.nombre_proxima_tarea = item.nombre_proxima_tarea;
                h.nombre_resultado = item.nombre_resultado;
                h.nombre_tarea = item.nombre_tarea;
                h.NroDisposicionSADE = item.NroDisposicionSADE;
                h.superficie = item.superficie;
                h.username = item.username;
                result.Add(h);
            }
            return result;

        }

        private List<clsItemHoja6> BuscarConsultaHoja6Excel(int startRowIndex, int maximumRows, out int totalRowCount)
        {

            Guid userid = Functions.GetUserId();

            totalRowCount = 0;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var q = (from e in db.Estadisticas_Hoja6
                     select new 
                     {
                         CreateDate = e.CreateDate,
                         id_circuito_actual = e.id_circuito_actual.Value,
                         id_solicitud = e.id_solicitud.Value,
                         asigCalif_ini = e.asigCalif_ini,
                         avh_cierre = e.avh_cierre,
                         avh_ini = e.avh_ini,
                         Cantidad_Observado = e.Cantidad_Observado,
                         Circuito_Origen = e.Circuito_Origen,
                         dict_cierre = e.dict_cierre,
                         dict_ini = e.dict_ini,
                         Dif_EE_asig_cierre = e.Dif_EE_asig_cierre,
                         Dif_RFD_asig_cierre = e.Dif_RFD_asig_cierre,
                         Observado = e.Observado,
                         rfd_cierre = e.rfd_cierre,
                         rhyp_ini = e.rhyp_ini,
                         dias_dictamen = e.dias_dictamen,
                         es_caducidad = e.es_caducidad

                     });

            q = q.Where(x => x.id_circuito_actual == this.BusCircuito3);

            totalRowCount = q.Count();

            q = q.OrderBy(o => o.id_solicitud).ThenBy(o => o.asigCalif_ini).ThenBy(o => o.avh_ini).ThenBy(o => o.avh_cierre).ThenBy(o => o.dict_ini)
                .ThenBy(o => o.dict_cierre).ThenBy(o => o.rhyp_ini).ThenBy(o => o.rfd_cierre).Skip(startRowIndex).Take(maximumRows);

            var rq = q.ToList();

            db.Dispose();
            List<clsItemHoja6> result = new List<clsItemHoja6>();
            clsItemHoja6 h;
            foreach (var item in rq)
            {
                h = new clsItemHoja6();
                h.CreateDate = item.CreateDate != null ? item.CreateDate.Value.ToString("dd/MM/yyyy") : "";
                h.id_circuito_actual = item.id_circuito_actual;
                h.id_solicitud = item.id_solicitud;
                h.asigCalif_ini = item.asigCalif_ini != null ? item.asigCalif_ini.Value.ToString("dd/MM/yyyy") : "";
                h.avh_cierre = item.avh_cierre != null ? item.avh_cierre.Value.ToString("dd/MM/yyyy") : "";
                h.avh_ini = item.avh_ini != null ? item.avh_ini.Value.ToString("dd/MM/yyyy") : "";
                h.Cantidad_Observado = item.Cantidad_Observado;
                h.Circuito_Origen = item.Circuito_Origen;
                h.dict_cierre = item.dict_cierre != null ? item.dict_cierre.Value.ToString("dd/MM/yyyy") : "";
                h.dict_ini = item.dict_ini != null ? item.dict_ini.Value.ToString("dd/MM/yyyy") : "";
                h.Dif_EE_asig_cierre = item.Dif_EE_asig_cierre;
                h.Dif_RFD_asig_cierre = item.Dif_RFD_asig_cierre;
                h.Observado = item.Observado;
                h.rfd_cierre = item.rfd_cierre != null ? item.rfd_cierre.Value.ToString("dd/MM/yyyy") : "";
                h.rhyp_ini = item.rhyp_ini != null ? item.rhyp_ini.Value.ToString("dd/MM/yyyy") : "";
                h.Dias_Totales_Dictamen = item.dias_dictamen;
                h.es_caducidad = item.es_caducidad;

                result.Add(h);
            }
            return result;
        }

        private List<clsItemHoja7> BuscarConsultaHoja7Excel(int startRowIndex, int maximumRows, out int totalRowCount)
        {

            Guid userid = Functions.GetUserId();

            totalRowCount = 0;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var q = (from e in db.Estadisticas_Hoja7
                     select new
                     {
                         e.id_solicitud,
                         e.cod_circuito,
                         e.Fecha_Inicio_Control_Informe,
                         e.Hora_Inicio_Control_Informe,
                         e.Fecha_Inicio_GenerarExpediente,
                         e.Hora_Inicio_GenerarExpediente,
                         e.Fecha_Fin_Generar_Expediente,
                         e.Hora_Fin_Generar_Expediente,
                         e.Observado_alguna_vez,
                         e.id_circuito_actual
                     });

            q = q.Where(x => x.id_circuito_actual == this.BusCircuito3);

            totalRowCount = q.Count();

            q = q.OrderBy(o => o.id_solicitud).ThenBy(o => o.Fecha_Inicio_Control_Informe).ThenBy(o => o.Hora_Inicio_Control_Informe)
                .ThenBy(o => o.Fecha_Inicio_GenerarExpediente).ThenBy(o => o.Hora_Inicio_GenerarExpediente)
                .ThenBy(o => o.Fecha_Fin_Generar_Expediente).ThenBy(o => o.Hora_Fin_Generar_Expediente)
                .Skip(startRowIndex).Take(maximumRows);

            var rq = q.ToList();

            db.Dispose();
            List<clsItemHoja7> result = new List<clsItemHoja7>();
            clsItemHoja7 h;
            foreach (var item in rq)
            {
                h = new clsItemHoja7();
                h.id_solicitud = item.id_solicitud;
                h.cod_circuito = item.cod_circuito;
                h.Fecha_Inicio_Control_Informe = item.Fecha_Inicio_Control_Informe != null ? item.Fecha_Inicio_Control_Informe.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Inicio_GenerarExpediente = item.Fecha_Inicio_GenerarExpediente != null ? item.Fecha_Inicio_GenerarExpediente.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Fin_Generar_Expediente = item.Fecha_Fin_Generar_Expediente != null ? item.Fecha_Fin_Generar_Expediente.Value.ToString("dd/MM/yyyy") : "";
                h.Hora_Inicio_Control_Informe = item.Hora_Inicio_Control_Informe;
                h.Hora_Inicio_GenerarExpediente = item.Hora_Inicio_GenerarExpediente;
                h.Hora_Fin_Generar_Expediente = item.Hora_Fin_Generar_Expediente;
                h.Observado_alguna_vez = item.Observado_alguna_vez.Value;
                result.Add(h);
            }
            return result;
        }

        private List<clsItemHoja7_2> BuscarConsultaHoja7_2Excel(int startRowIndex, int maximumRows, out int totalRowCount)
        {

            Guid userid = Functions.GetUserId();

            totalRowCount = 0;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var q = (from e in db.Estadisticas_Hoja7_2
                     select new
                     {
                         e.id_solicitud,
                         e.cod_circuito,
                         e.Fecha_Inicio_GenerarExpediente,
                         e.Hora_Inicio_GenerarExpediente,
                         e.Fecha_Fin_Generar_Expediente,
                         e.Hora_Fin_Generar_Expediente,
                         e.Fecha_Fin_Revision_Gerente,
                         e.Hora_Fin_Revision_Gerente,
                         e.Observado_alguna_vez,
                         e.id_circuito_actual
                     });

            q = q.Where(x => x.id_circuito_actual == this.BusCircuito3);

            totalRowCount = q.Count();

            q = q.OrderBy(o => o.id_solicitud).ThenBy(o => o.Fecha_Inicio_GenerarExpediente).ThenBy(o => o.Hora_Inicio_GenerarExpediente)
                .ThenBy(o => o.Fecha_Inicio_GenerarExpediente).ThenBy(o => o.Hora_Inicio_GenerarExpediente)
                .ThenBy(o => o.Fecha_Fin_Generar_Expediente).ThenBy(o => o.Hora_Fin_Generar_Expediente)
                .Skip(startRowIndex).Take(maximumRows);

            var rq = q.ToList();

            db.Dispose();
            List<clsItemHoja7_2> result = new List<clsItemHoja7_2>();
            clsItemHoja7_2 h;
            foreach (var item in rq)
            {
                h = new clsItemHoja7_2();
                h.id_solicitud = item.id_solicitud;
                h.cod_circuito = item.cod_circuito;                
                h.Fecha_Inicio_GenerarExpediente = item.Fecha_Inicio_GenerarExpediente != null ? item.Fecha_Inicio_GenerarExpediente.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Fin_Generar_Expediente = item.Fecha_Fin_Generar_Expediente != null ? item.Fecha_Fin_Generar_Expediente.Value.ToString("dd/MM/yyyy") : "";
                //h.Hora_Inicio_GenerarExpediente = item.Hora_Inicio_GenerarExpediente;
                //h.Hora_Fin_Generar_Expediente = item.Hora_Fin_Generar_Expediente;
                h.Fecha_Fin_Revision_Gerente = item.Fecha_Fin_Revision_Gerente != null ? item.Fecha_Fin_Revision_Gerente.Value.ToString("dd/MM/yyyy") : "";
                h.Observado_alguna_vez = item.Observado_alguna_vez.Value;
                result.Add(h);
            }
            return result;
        }

        private List<clsItemHoja8> BuscarConsultaHoja8Excel(int startRowIndex, int maximumRows, out int totalRowCount)
        {

            Guid userid = Functions.GetUserId();

            totalRowCount = 0;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var q = (from e in db.Estadisticas_Hoja8
                     select new
                     {
                         e.id_solicitud,
                         e.cod_circuito,
                         e.Fecha_Inicio_Asignacion_Calificador,
                         e.Fecha_cierre_Revision_gerente_2,
                         e.Fecha_Cierre_Revision_firma_dispo,
                         e.Fecha_Inicio_Dictamenes,
                         e.Fecha_Cierre_Dictamenes,
                         e.Fecha_Inicio_Revision_Pagos,
                         e.Fecha_Fin_Revision_Pagos,
                         e.Observado,
                         e.Cantidad_Veces_Observado,
                         e.Circuito_Origen,
                         e.id_circuito_actual,
                         e.dias_dictamen
                     });

            q = q.Where(x => x.id_circuito_actual == this.BusCircuito3);

            totalRowCount = q.Count();

            q = q.OrderBy(o => o.id_solicitud).ThenBy(o => o.Fecha_Inicio_Asignacion_Calificador).ThenBy(o => o.Fecha_cierre_Revision_gerente_2)
                .ThenBy(o => o.Fecha_Cierre_Revision_firma_dispo).Skip(startRowIndex).Take(maximumRows);

            var rq = q.ToList();

            db.Dispose();
            List<clsItemHoja8> result = new List<clsItemHoja8>();
            clsItemHoja8 h;
            foreach (var item in rq)
            {
                h = new clsItemHoja8();
                h.id_solicitud = item.id_solicitud;
                h.cod_circuito = item.cod_circuito;
                h.Fecha_Inicio_Asignacion_Calificador = item.Fecha_Inicio_Asignacion_Calificador != null ? item.Fecha_Inicio_Asignacion_Calificador.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_cierre_Revision_gerente_2 = item.Fecha_cierre_Revision_gerente_2 != null ? item.Fecha_cierre_Revision_gerente_2.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Cierre_Revision_firma_dispo = item.Fecha_Cierre_Revision_firma_dispo != null ? item.Fecha_Cierre_Revision_firma_dispo.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Inicio_Dictamenes = item.Fecha_Inicio_Dictamenes != null ? item.Fecha_Inicio_Dictamenes.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Cierre_Dictamenes = item.Fecha_Cierre_Dictamenes != null ? item.Fecha_Cierre_Dictamenes.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Inicio_Revision_Pagos = item.Fecha_Inicio_Revision_Pagos != null ? item.Fecha_Inicio_Revision_Pagos.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Fin_Revision_Pagos = item.Fecha_Fin_Revision_Pagos != null ? item.Fecha_Fin_Revision_Pagos.Value.ToString("dd/MM/yyyy") : "";
                h.Observado = item.Observado;
                h.Cantidad_Veces_Observado = item.Cantidad_Veces_Observado;
                h.Circuito_Origen = item.Circuito_Origen;
                h.Dias_Totales_Dictamen = item.dias_dictamen;
                result.Add(h);
            }
            return result;
        }

        private List<clsItemHoja8_2> BuscarConsultaHoja8_2Excel(int startRowIndex, int maximumRows, out int totalRowCount)
        {

            Guid userid = Functions.GetUserId();

            totalRowCount = 0;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var q = (from e in db.Estadisticas_Hoja8_2
                     select new
                     {
                         e.id_solicitud,
                         e.cod_circuito,
                         e.Fecha_Inicio_Asignacion_Calificador,
                         e.Fecha_Fin_Revision_Subgerente,
                         e.Fecha_Fin_Revision_Gerente,
                         e.Fecha_inicio_Revision_DGHP,
                         e.Fecha_Cierre_Revision_firma_dispo,
                         e.Fecha_Inicio_Dictamenes,
                         e.Fecha_Cierre_Dictamenes,
                         e.Fecha_inicio_Consulta_Adicional,
                         e.Fecha_Cierre_Consulta_Adicional,
                         e.Fecha_inicio_AVH,
                         e.Fecha_Cierre_AVH,
                         e.Observado,
                         e.Cantidad_Veces_Observado,
                         e.Circuito_Origen,
                         e.id_circuito_actual,
                         e.dias_dictamen,
                         e.dias_consulta_adicional
                     });

            q = q.Where(x => x.id_circuito_actual == this.BusCircuito3);

            totalRowCount = q.Count();

            q = q.OrderBy(o => o.id_solicitud).ThenBy(o => o.Fecha_Inicio_Asignacion_Calificador).ThenBy(o => o.Fecha_inicio_Revision_DGHP)
                .ThenBy(o => o.Fecha_Cierre_Revision_firma_dispo).Skip(startRowIndex).Take(maximumRows);

            var rq = q.ToList();

            db.Dispose();
            List<clsItemHoja8_2> result = new List<clsItemHoja8_2>();
            clsItemHoja8_2 h;
            foreach (var item in rq)
            {
                h = new clsItemHoja8_2();
                h.id_solicitud = item.id_solicitud;
                h.cod_circuito = item.cod_circuito;
                h.Fecha_Inicio_Asignacion_Calificador = item.Fecha_Inicio_Asignacion_Calificador != null ? item.Fecha_Inicio_Asignacion_Calificador.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Cierre_Revision_Subgerente = item.Fecha_Fin_Revision_Subgerente != null ? item.Fecha_Fin_Revision_Subgerente.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Cierre_Revision_Gerente = item.Fecha_Fin_Revision_Gerente != null ? item.Fecha_Fin_Revision_Gerente.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Inicio_Revision_DGHP = item.Fecha_inicio_Revision_DGHP != null ? item.Fecha_inicio_Revision_DGHP.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Cierre_Revision_firma_dispo = item.Fecha_Cierre_Revision_firma_dispo != null ? item.Fecha_Cierre_Revision_firma_dispo.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Inicio_Dictamenes = item.Fecha_Inicio_Dictamenes != null ? item.Fecha_Inicio_Dictamenes.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Cierre_Dictamenes = item.Fecha_Cierre_Dictamenes != null ? item.Fecha_Cierre_Dictamenes.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_inicio_Consulta_Adicional = item.Fecha_inicio_Consulta_Adicional != null ? item.Fecha_inicio_Consulta_Adicional.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Cierre_Consulta_Adicional = item.Fecha_Cierre_Consulta_Adicional != null ? item.Fecha_Cierre_Consulta_Adicional.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_inicio_AVH = item.Fecha_inicio_AVH != null ? item.Fecha_inicio_AVH.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_cierre_AVH = item.Fecha_Cierre_AVH != null ? item.Fecha_Cierre_AVH.Value.ToString("dd/MM/yyyy") : "";
                h.Observado = item.Observado;
                h.Cantidad_Veces_Observado = item.Cantidad_Veces_Observado;
                h.Circuito_Origen = item.Circuito_Origen;
                h.Dias_Totales_Dictamen = item.dias_dictamen;
                h.Dias_Totales_Consulta = item.dias_consulta_adicional;
                result.Add(h);
            }
            return result;
        }

        private List<clsItemHoja9> BuscarConsultaHoja9Excel(int startRowIndex, int maximumRows, out int totalRowCount)
        {

            Guid userid = Functions.GetUserId();

            totalRowCount = 0;

            DGHP_Entities db = new DGHP_Entities();
            db.Database.CommandTimeout = 300;

            var q = (from e in db.Estadisticas_Hoja9
                     select new
                     {
                         e.id_circuito_actual,
                         e.cod_circuito,
                         e.id_solicitud,
                         e.FechaInicio_Asig_Calificador,
                         e.Fecha_Inicio_Notif_Caducidad,
                         e.Fecha_Inicio_Rev_DGHP_Caducidad,
                         e.Fecha_Fin_Rev_DGHP_Caducidad,
                         e.Fecha_Fin_Gen_Expediente,
                         e.Fecha_Fin_Rev_Firma
                     });

            q = q.Where(x => x.id_circuito_actual == this.BusCircuito3);

            totalRowCount = q.Count();

            q = q.OrderBy(o => o.id_solicitud).ThenBy(o => o.FechaInicio_Asig_Calificador).ThenBy(o => o.Fecha_Inicio_Notif_Caducidad)
                .Skip(startRowIndex).Take(maximumRows);

            var rq = q.ToList();

            db.Dispose();
            List<clsItemHoja9> result = new List<clsItemHoja9>();
            clsItemHoja9 h;
            foreach (var item in rq)
            {
                h = new clsItemHoja9();
                h.id_circuito_actual = item.id_circuito_actual;
                h.cod_circuito = item.cod_circuito;
                h.id_solicitud = item.id_solicitud;
                h.FechaInicio_Asig_Calificador = item.FechaInicio_Asig_Calificador != null ? item.FechaInicio_Asig_Calificador.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Inicio_Notif_Caducidad = item.Fecha_Inicio_Notif_Caducidad != null ? item.Fecha_Inicio_Notif_Caducidad.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Inicio_Rev_DGHP_Caducidad = item.Fecha_Inicio_Rev_DGHP_Caducidad != null ? item.Fecha_Inicio_Rev_DGHP_Caducidad.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Fin_Rev_DGHP_Caducidad = item.Fecha_Fin_Rev_DGHP_Caducidad != null ? item.Fecha_Fin_Rev_DGHP_Caducidad.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Fin_Gen_Expediente = item.Fecha_Fin_Gen_Expediente != null ? item.Fecha_Fin_Gen_Expediente.Value.ToString("dd/MM/yyyy") : "";
                h.Fecha_Fin_Rev_Firma = item.Fecha_Fin_Rev_Firma != null ? item.Fecha_Fin_Rev_Firma.Value.ToString("dd/MM/yyyy") : "";
                result.Add(h);
            }
            return result;
        }

        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);

            string script_nombre = "mostrarMensaje";
            string script = "mostrarMensaje('" + mensaje + "','" + titulo + "');";

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm != null && sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
            }

        }

        #region entity

        private void IniciarEntity()
        {
            if (this.db == null)
            {
                this.db = new DGHP_Entities();
                db.Database.CommandTimeout = 300;
            }
        }

        private void FinalizarEntity()
        {
            if (this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }
        }

        #endregion


        #region ExportacionExcel


        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                bool exportacion_en_proceso = (Session["exportacion_en_proceso"] != null ? (bool)Session["exportacion_en_proceso"] : false);

                if (exportacion_en_proceso)
                {
                    lblRegistrosExportados.Text = Convert.ToString(Session["progress_data"]);
                    lblExportarError.Text = Convert.ToString(Session["progress_data"]);
                }
                else
                {
                    Timer1.Enabled = false;
                    btnCerrarExportacion.Visible = true;
                    pnlDescargarExcel.Style["display"] = "block";
                    pnlExportandoExcel.Style["display"] = "none";
                    string filename = Session["filename_exportacion"].ToString();
                    filename = HttpUtility.UrlEncode(filename);
                    btnDescargarExcel.NavigateUrl = string.Format("~/Controls/DescargarArchivoTemporal.aspx?fname={0}", filename);
                    Session.Remove("filename_exportacion");
                }
                //Cuando falla la exportacion
                if (Session["progress_data"].ToString().StartsWith("Error:"))
                {
                    Timer1.Enabled = false;
                    btnCerrarExportacion.Visible = true;
                    pnlExportandoExcel.Style["display"] = "none";
                    pnlExportacionError.Style["display"] = "block";
                }
            }
            catch
            {
                Timer1.Enabled = false;
            }

        }

        protected void btnCerrarExportacion_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            Session.Remove("filename_exportacion");
            Session.Remove("progress_data");
            Session.Remove("exportacion_en_proceso");
            pnlExportacionError.Style["display"] = "none";
            pnlDescargarExcel.Style["display"] = "none";
            pnlExportandoExcel.Style["display"] = "block";

            this.EjecutarScript(updExportaExcel, "hidefrmExportarExcel();");

        }

        protected void mostrarTimer(string name)
        {
            btnCerrarExportacion.Visible = false;
            // genera un nombre de archivo aleatorio
            Random random = new Random((int)DateTime.Now.Ticks);
            int NroAleatorio = random.Next(0, 100);
            NroAleatorio = NroAleatorio * random.Next(0, 100);
            name = name + "-{0}.xls";
            string fileName = string.Format(name, NroAleatorio);

            Session["exportacion_en_proceso"] = true;
            Session["progress_data"] = "Preparando exportación.";
            Session["filename_exportacion"] = fileName;

            Timer1.Enabled = true;
        }


        protected void btnDescargarBloque1_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarBuscar();
                mostrarTimer("ExcelTareas");
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ExportarSolicitudesAExcel));
                thread.Start();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                EjecutarScript(updmpeInfo, "showfrmError();");
                Enviar_Mensaje(ex.Message, "");
            }
        }

        private void ExportarSolicitudesAExcel()
        {
            decimal cant_registros_x_vez = 0m;
            int totalRowCount = 0;
            int startRowIndex = 0;
            this.EjecutarScript(updExportaExcel, "showfrmExportarExcel();");
            try
            {
                // Esto se realiza para saber el total y de a cuanto se va mostrar el progreso.
                BuscarConsultaExcel(startRowIndex, 0, out totalRowCount);
                if (totalRowCount < 2000)
                    cant_registros_x_vez = 200m;
                else if (totalRowCount < 7000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 1000m;
                int cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<clsItemSolicitudes> resultados = new List<clsItemSolicitudes>();
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados.AddRange(BuscarConsultaExcel(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount));
                    Session["progress_data"] = string.Format("{0} / {1} registros exportados.", resultados.Count, totalRowCount);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} / {1} registros exportados.", resultados.Count, totalRowCount);
                var lstExportar = (from res in resultados
                                   select new
                                   {
                                       NroSolicitud = res.id_solicitud,
                                       NombreTarea = res.nombre_tarea,
                                       NombreResultado = res.nombre_resultado,
                                       FechaInicio = res.Fecha_Inicio.HasValue ? res.Fecha_Inicio.Value.ToShortDateString() : "",
                                       HoraInicio = res.Fecha_Inicio.HasValue ? res.Fecha_Inicio.Value.ToShortTimeString() : "",
                                       FechaAsignacion = res.Fecha_Asignacion.HasValue ? res.Fecha_Asignacion.GetValueOrDefault().ToShortDateString() : "",
                                       HoraAsignacion = res.Fecha_Asignacion.HasValue ? res.Fecha_Asignacion.GetValueOrDefault().ToShortTimeString() : "",
                                       FechaCierre = res.Fecha_Cierre.HasValue ? res.Fecha_Cierre.GetValueOrDefault().ToShortDateString() : "",
                                       HoraCierre = res.Fecha_Cierre.HasValue ? res.Fecha_Cierre.GetValueOrDefault().ToShortTimeString() : "",
                                       DiferenciaInicioCierre = res.Fecha_Cierre.HasValue && res.Fecha_Inicio.HasValue ? (res.Fecha_Cierre.Value.Date.Subtract(res.Fecha_Inicio.Value).Days - 2 * ((res.Fecha_Cierre.Value.Date.Subtract(res.Fecha_Inicio.Value).Days) / 7)).ToString() : "-",
                                       DiferenciaAsignacionCierre = res.Fecha_Cierre.HasValue && res.Fecha_Asignacion.HasValue ? (res.Fecha_Cierre.Value.Date.Subtract(res.Fecha_Asignacion.Value).Days - 2 * ((res.Fecha_Cierre.Value.Date.Subtract(res.Fecha_Asignacion.Value).Days) / 7)).ToString() : "-",
                                       NombreUsuario = res.UserName,
                                       Superficie = res.superficie,
                                       NroDisposicionGEDO = res.numero_dispo_GEDO,
                                       Area = res.desc_circuito,
                                       CodigoCircuito = res.cod_circuito,
                                   }).ToList();

                //DateTime dt1 = new DateTime(2011, 1, 1);
                //DateTime dt2 = new DateTime(2011, 1, 15);
                //int days = (from d in (Enumerable.Range(0, 1 + dt2.Subtract(dt1).Days).Select(offset => dt1.AddDays(offset)).ToArray())where d.DayOfWeek == DayOfWeek.Monday).Sum;

                // Convierte la lista en un dataset
                DataSet ds = new DataSet();
                DataTable dt = Functions.ToDataTable(lstExportar);
                dt.TableName = "Listado Revisiones";
                ds.Tables.Add(dt);
                string savedFileName = Constants.Path_Temporal + Session["filename_exportacion"].ToString();

                Functions.EliminarArchivosDirectorioTemporal();
                // Utiliza DocumentFormat.OpenXml para exportar a excel
                Model.CreateExcelFile.CreateExcelDocument(ds, savedFileName);
                // quita la variable de session.
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogError.Write(ex.InnerException);
                    Session["progress_data"] = "Error: " + ex.InnerException.Message;
                }
                else
                {
                    LogError.Write(ex);
                    Session["progress_data"] = "Error: " + ex.Message;
                }
            }
        }

        protected void btnDescargarBloque2_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarBuscar2();
                mostrarTimer("ExcelTareasSGO");
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ExportarSolicitudesSGOAExcel));
                thread.Start();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                EjecutarScript(updmpeInfo, "showfrmError();");
                Enviar_Mensaje(ex.Message, "");
            }
        }
        private void ExportarSolicitudesSGOAExcel()
        {
            decimal cant_registros_x_vez = 0m;
            int totalRowCount = 0;
            int startRowIndex = 0;
            this.EjecutarScript(updExportaExcel, "showfrmExportarExcel();");
            try
            {

                // Esto se realiza para saber el total y de a cuanto se va mostrar el progreso.
                BuscarConsultaSGOExcel(startRowIndex, 0, out totalRowCount);
                if (totalRowCount < 2000)
                    cant_registros_x_vez = 200m;
                else if (totalRowCount < 7000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;
                int cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<clsItemSolicitudes> resultados = new List<clsItemSolicitudes>();
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados.AddRange(BuscarConsultaSGOExcel(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados.Count);
                var lstExportar = (from res in resultados
                                   select new
                                   {
                                       NroSolicitud = res.id_solicitud,
                                       FechaInicioAsignacióndeCalificador = res.Fecha_Inicio_Asignacion_Calificador,
                                       FechaUltimoInicioRevisiónDGHyP = res.Fecha_inicio_ULTIMA_Revision_HyP,
                                       AsignacionCalificador = res.Asignacion_Calificador,
                                       FirmaDispoCierre = res.Fecha_Cierre,
                                       Díashábiles = res.Fecha_Inicio_Asignacion_Calificador.HasValue && res.Fecha_inicio_ULTIMA_Revision_HyP.HasValue ?
                                                    (res.Fecha_inicio_ULTIMA_Revision_HyP.Value.Date.Subtract(res.Fecha_Inicio_Asignacion_Calificador.Value).Days - 2 * ((res.Fecha_inicio_ULTIMA_Revision_HyP.Value.Date.Subtract(res.Fecha_Inicio_Asignacion_Calificador.Value).Days) / 7)).ToString() : "-",
                                       Observado = res.Observado,
                                       Superficie = res.superficie,
                                       Area = res.desc_circuito,
                                       CodigoCircuito = res.cod_circuito,
                                   }).ToList();

                //DateTime dt1 = new DateTime(2011, 1, 1);
                //DateTime dt2 = new DateTime(2011, 1, 15);
                //int days = (from d in (Enumerable.Range(0, 1 + dt2.Subtract(dt1).Days).Select(offset => dt1.AddDays(offset)).ToArray())where d.DayOfWeek == DayOfWeek.Monday).Sum;

                // Convierte la lista en un dataset
                DataSet ds = new DataSet();
                DataTable dt = Functions.ToDataTable(lstExportar);
                dt.TableName = "Listado Revisiones";
                ds.Tables.Add(dt);
                string savedFileName = Constants.Path_Temporal + Session["filename_exportacion"].ToString();

                Functions.EliminarArchivosDirectorioTemporal();
                // Utiliza DocumentFormat.OpenXml para exportar a excel
                Model.CreateExcelFile.CreateExcelDocument(ds, savedFileName);
                // quita la variable de session.
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogError.Write(ex.InnerException);
                    Session["progress_data"] = "Error: " + ex.InnerException.Message;
                }
                else
                {
                    LogError.Write(ex);
                    Session["progress_data"] = "Error: " + ex.Message;
                }
            }
        }

        protected void btnDescargarBloque3_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarBuscarB3();
                var n = ddlBusCircuito3.SelectedItem.Text;
                n = n.Substring(0, n.IndexOf("-")-1);
                mostrarTimer(n + "_" + DateTime.Now.ToString("dd-MM-yyyy"));
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ExportarHojasExcel));
                thread.Start();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                EjecutarScript(updmpeInfo, "showfrmError();");
                Enviar_Mensaje(ex.Message, "");
            }
        }

        private void ExportarHojasExcel()
        {
            decimal cant_registros_x_vez = 0m;
            int totalRowCount = 0;
            int startRowIndex = 0;
            this.EjecutarScript(updExportaExcel, "showfrmExportarExcel();");
            try
            {

                // Esto se realiza para saber el total y de a cuanto se va mostrar el progreso.
                BuscarConsultaHoja1Excel(startRowIndex, 0, out totalRowCount);
                if (totalRowCount < 2000)
                    cant_registros_x_vez = 200m;
                else if (totalRowCount < 7000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;
                int cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<clsItemHoja1> resultados1 = new List<clsItemHoja1>();
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados1.AddRange(BuscarConsultaHoja1Excel(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados1.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados1.Count);

                cant_registros_x_vez = 0m;
                totalRowCount = 0;
                startRowIndex = 0;
                BuscarConsultaHoja2Excel(startRowIndex, 0, out totalRowCount);
                if (totalRowCount < 2000)
                    cant_registros_x_vez = 200m;
                else if (totalRowCount < 7000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;
                cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<clsItemHoja2> resultados2 = new List<clsItemHoja2>();
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados2.AddRange(BuscarConsultaHoja2Excel(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados2.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados2.Count);

                cant_registros_x_vez = 0m;
                totalRowCount = 0;
                startRowIndex = 0;
                BuscarConsultaHoja3Excel(startRowIndex, 0, out totalRowCount);
                if (totalRowCount < 2000)
                    cant_registros_x_vez = 200m;
                else if (totalRowCount < 7000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;
                cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<clsItemHoja3> resultados3 = new List<clsItemHoja3>();
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados3.AddRange(BuscarConsultaHoja3Excel(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados3.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados3.Count);

                cant_registros_x_vez = 0m;
                totalRowCount = 0;
                startRowIndex = 0;
                BuscarConsultaHoja6Excel(startRowIndex, 0, out totalRowCount);
                if (totalRowCount < 2000)
                    cant_registros_x_vez = 200m;
                else if (totalRowCount < 7000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;
                cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<clsItemHoja6> resultados6 = new List<clsItemHoja6>();
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados6.AddRange(BuscarConsultaHoja6Excel(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados6.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados6.Count);

                cant_registros_x_vez = 0m;
                totalRowCount = 0;
                startRowIndex = 0;
                BuscarConsultaHoja7Excel(startRowIndex, 0, out totalRowCount);
                if (totalRowCount < 2000)
                    cant_registros_x_vez = 200m;
                else if (totalRowCount < 7000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;
                cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<clsItemHoja7> resultados7 = new List<clsItemHoja7>();
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados7.AddRange(BuscarConsultaHoja7Excel(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados7.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados7.Count);

                cant_registros_x_vez = 0m;
                totalRowCount = 0;
                startRowIndex = 0;
                BuscarConsultaHoja7_2Excel(startRowIndex, 0, out totalRowCount);
                if (totalRowCount < 2000)
                    cant_registros_x_vez = 200m;
                else if (totalRowCount < 7000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;
                cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<clsItemHoja7_2> resultados7_2 = new List<clsItemHoja7_2>();
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados7_2.AddRange(BuscarConsultaHoja7_2Excel(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados7_2.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados7_2.Count);

                cant_registros_x_vez = 0m;
                totalRowCount = 0;
                startRowIndex = 0;
                BuscarConsultaHoja8Excel(startRowIndex, 0, out totalRowCount);
                if (totalRowCount < 2000)
                    cant_registros_x_vez = 200m;
                else if (totalRowCount < 7000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;
                cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<clsItemHoja8> resultados8 = new List<clsItemHoja8>();
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados8.AddRange(BuscarConsultaHoja8Excel(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados8.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados8.Count);

                cant_registros_x_vez = 0m;
                totalRowCount = 0;
                startRowIndex = 0;
                BuscarConsultaHoja8_2Excel(startRowIndex, 0, out totalRowCount);
                if (totalRowCount < 2000)
                    cant_registros_x_vez = 200m;
                else if (totalRowCount < 7000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;
                cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<clsItemHoja8_2> resultados8_2 = new List<clsItemHoja8_2>();
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados8_2.AddRange(BuscarConsultaHoja8_2Excel(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados8_2.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados8_2.Count);

                cant_registros_x_vez = 0m;
                totalRowCount = 0;
                startRowIndex = 0;
                BuscarConsultaHoja9Excel(startRowIndex, 0, out totalRowCount);
                if (totalRowCount < 2000)
                    cant_registros_x_vez = 200m;
                else if (totalRowCount < 7000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;
                cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<clsItemHoja9> resultados9 = new List<clsItemHoja9>();
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados9.AddRange(BuscarConsultaHoja9Excel(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados9.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados9.Count);

                // Convierte la lista en un dataset
                DataSet ds = new DataSet();
                DataTable dt1 = Functions.ToDataTable(resultados1);
                dt1.TableName = "Estadisticas_Hoja1";
                ds.Tables.Add(dt1);

                DataTable dt2 = Functions.ToDataTable(resultados2);
                dt2.TableName = "Estadisticas_Hoja2";
                ds.Tables.Add(dt2);

                DataTable dt3 = Functions.ToDataTable(resultados3);
                dt3.TableName = "Estadisticas_Hoja3";
                ds.Tables.Add(dt3);

                DataTable dt6 = Functions.ToDataTable(resultados6);
                dt6.TableName = "Estadisticas_Hoja6";
                ds.Tables.Add(dt6);

                DataTable dt7 = Functions.ToDataTable(resultados7);
                dt7.TableName = "Estadisticas_Hoja7";
                ds.Tables.Add(dt7);

                DataTable dt7_2 = Functions.ToDataTable(resultados7_2);
                dt7_2.TableName = "Estadisticas_Hoja7.2";
                ds.Tables.Add(dt7_2);

                DataTable dt8 = Functions.ToDataTable(resultados8);
                dt8.TableName = "Estadisticas_Hoja8";
                ds.Tables.Add(dt8);

                DataTable dt8_2 = Functions.ToDataTable(resultados8_2);
                dt8_2.TableName = "Estadisticas_Hoja8.2";
                ds.Tables.Add(dt8_2);

                DataTable dt9 = Functions.ToDataTable(resultados9);
                dt9.TableName = "Estadisticas_Hoja9";
                ds.Tables.Add(dt9);

                string savedFileName = Constants.Path_Temporal + Session["filename_exportacion"].ToString();

                Functions.EliminarArchivosDirectorioTemporal();
                // Utiliza DocumentFormat.OpenXml para exportar a excel
                Model.CreateExcelFile.CreateExcelDocument(ds, savedFileName);
                // quita la variable de session.
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogError.Write(ex.InnerException);
                    Session["progress_data"] = "Error: " + ex.InnerException.Message;
                }
                else
                {
                    LogError.Write(ex);
                    Session["progress_data"] = "Error: " + ex.Message;
                }
            }
        }

        #endregion

        protected void ddlBusTipoTramite_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList lista = (DropDownList)sender;
                if (lista.ID == "ddlBusTipoTramiteRev")
                {
                    if (ddlBusTipoTramiteRev.SelectedValue != "" && ddlBusTipoTramiteRev.SelectedValue != "99")
                    {
                        int id_tipotramite = int.Parse(ddlBusTipoTramiteRev.SelectedValue);
                        CargarTipoExpediente(id_tipotramite, 2);
                    }
                    else
                    {
                        CargarTipoExpediente(99, 2);
                    }
                }
                if (lista.ID == "ddlBusTipoTramite")
                {
                    if (ddlBusTipoTramite.SelectedValue != "" && ddlBusTipoTramite.SelectedValue != "99")
                    {
                        int id_tipotramite = int.Parse(ddlBusTipoTramite.SelectedValue);
                        CargarTipoExpediente(id_tipotramite, 1);
                    }
                    else
                    {
                        CargarTipoExpediente(99, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);

            }

        }

        protected void ddlBusTipoExpediente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList lista = (DropDownList)sender;
                if (lista.ID == "ddlBusTipoExpedienteRev")
                {
                    if (ddlBusTipoExpedienteRev.SelectedValue != "" && ddlBusTipoExpedienteRev.SelectedValue != "99")
                    {
                        int id_tipoexpediente = int.Parse(ddlBusTipoExpedienteRev.SelectedValue);
                        CargarSubtipoExpediente(id_tipoexpediente, 2);
                    }
                    else
                    {
                        CargarSubtipoExpediente(99, 2);
                    }
                }
                if (lista.ID == "ddlBusTipoExpediente")
                {
                    if (ddlBusTipoExpediente.SelectedValue != "" && ddlBusTipoExpediente.SelectedValue != "99")
                    {
                        int id_tipoexpediente = int.Parse(ddlBusTipoExpediente.SelectedValue);
                        CargarSubtipoExpediente(id_tipoexpediente, 1);
                    }
                    else
                    {
                        CargarSubtipoExpediente(99, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);

            }
        }

        protected void ddlBusSubtipoExpediente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList lista = (DropDownList)sender;
                if (lista.ID == "ddlBusSubtipoExpedienteRev")
                {
                    if (ddlBusSubtipoExpedienteRev.SelectedValue != "" && ddlBusSubtipoExpedienteRev.SelectedValue != "99")
                    {
                        int id_subtipoexpediente = int.Parse(ddlBusSubtipoExpedienteRev.SelectedValue);
                        CargarCircuitos(id_subtipoexpediente, 2);
                    }
                    else
                    {
                        CargarCircuitos(99, 2);
                    }
                }
                if (lista.ID == "ddlBusSubtipoExpediente")
                {
                    if (ddlBusSubtipoExpediente.SelectedValue != "" && ddlBusSubtipoExpediente.SelectedValue != "99")
                    {
                        int id_subtipoexpediente = int.Parse(ddlBusSubtipoExpediente.SelectedValue);
                        CargarCircuitos(id_subtipoexpediente, 1);
                    }
                    else
                    {
                        CargarCircuitos(99, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);

            }
        }

    }
}