using SGI.Model;
using SGI.WebServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.ABM.Ubicaciones
{
    public partial class SubdividirUbicacion : BasePage
    {
        #region cargar inicial

        //private int id_ubicacion = -1;
        private bool filtrar = false;
        private int nro_partida_matriz = 0;
        private int nro_partida_horiz = 0;
        private string cod_calle = "0";
        private int nro_puerta = 0;
        private int seccion = 0;
        private string manzana = "";
        private string parcela = "";
        private int id_tipo_ubicacion = -1;
        private int id_sub_tipo_ubicacion = -1;

        private bool editar;
        private bool visualizar;

        bool filtroPHSubGrilla = false;

        private bool editable = true;

        #endregion

        #region permisos

        private void CargarPermisos()
        {
            using (var db = new DGHP_Entities())
            {
                Guid userid = Functions.GetUserId();

                var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

                foreach (var perfil in perfiles_usuario)
                {
                    var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

                    if (menu_usuario.Contains("Visualizar Ubicaciones"))
                        visualizar = true;
                    if (menu_usuario.Contains("Editar Ubicaciones"))
                    {
                        editar = true;
                    }
                }
            }
        }

        #endregion

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }

        // Se obtienen los valores almacenados de los datos de la ubicacion padre
        private void CargarDatosPadre(int id_ubicacion_padre)
        {
            using (var db = new DGHP_Entities())
            {
                var ubi_padre = db.Ubicaciones.Where(x => x.id_ubicacion == id_ubicacion_padre).SingleOrDefault();
                if (ubi_padre == null)
                    return;

                if (ubi_padre.Seccion != null)
                {
                    txtSeccion.Text = ubi_padre.Seccion.ToString();
                    txtSeccion.Enabled = ubi_padre.Seccion <= 0;
                }

                if (ubi_padre.Manzana != null)
                {
                    txtManzana.Text = ubi_padre.Manzana.Trim();
                    txtManzana.Enabled = ubi_padre.Manzana.Trim() == "";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(this);
            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "init_Js_updDatos", "init_Js_updDatos();", true);
            }
            CargarComboCalles();
            CargarComboCallesSub();
            if (!IsPostBack)
            {
                if (Request.Cookies["SubdividirUbicacionSub_IdCalle"] != null)
                {
                    AutocompleteCallesSub.SelectValueByKey = string.Empty;
                }
                if (Request.Cookies["SubdividirUbicacion_IdCalle"] != null)
                {
                    AutocompleteCalles.SelectValueByKey = string.Empty;
                }
                hid_id_ubi_padre.Value = "";
                hid_ids_ubi_sub.Value = "";

                // Verifico si exite id de operacion de subdivision en proceso
                var op = Session["id_operacion"];
                int id_ope = 0;
                if (op != null && op.ToString() != "" && int.TryParse(op.ToString(), out id_ope))
                {
                    int id_ubi_padre = getIdPadreOpSubdivEnProceso(id_ope);
                    if (id_ubi_padre != 0)
                    {
                        hid_id_ubi_padre.Value = id_ubi_padre.ToString();
                        grdParcelaASubdiv.DataBind();

                        var ids = getIdsUbisTemp(id_ope);
                        if (ids != null && ids.Count() > 0)
                        {
                            hid_ids_ubi_sub.Value = "";
                            foreach (var i in ids)
                                hid_ids_ubi_sub.Value += i.ToString() + ",";

                            grdParcelasSubdiv.DataBind();
                        }
                    }
                    else
                        id_ope = 0;
                }
                // Variable id de la operacion
                Session["id_operacion"] = id_ope;

                // Id Ubicación (id_ubicacion) de la Parcela a subdividir 
                Session["id_ubi_padre"] = hid_id_ubi_padre.Value;

                // Colección de Ids (id_ubicacion_temp) de las subdivisiones registradas en la tabla Ubicaciones_temp
                Session["ids_ubi_sub"] = hid_ids_ubi_sub.Value;

                LoadData();
            }
            else
            {
                hid_id_ubi_padre.Value = Session["id_ubi_padre"] != null ? Session["id_ubi_padre"].ToString() : "";
                hid_ids_ubi_sub.Value = Session["ids_ubi_sub"] != null ? Session["ids_ubi_sub"].ToString() : "";
            }

            CargarPermisos();
            btnAgregarParASub.Visible = editar;
            btnAgregarParsSub.Visible = editar;
            pnlParcelasSubdiv.Visible = hid_id_ubi_padre.Value != "";
        }

        #region Funciones

        public void LoadData()
        {
            try
            {
                CargarCombo_tipoUbicacion(true);
                CargarCombo_subTipoUbicacion(0, true);
                CargarCombo_tipoUbicacionABM(true);
                CargarCombo_subTipoUbicacionABM(0, true);
                updPnlFiltroBuscar_ubi_partida.Update();
                updPnlFiltroBuscar_ubi_dom.Update();
                updPnlFiltroBuscar_ubi_smp.Update();
                updPnlFiltroBuscar_ubi_especial.Update();
                updDatos.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LimpiarAgregarParASub()
        {
            txtUbiNroPartida.Text = "";
            AutocompleteCalles.ClearSelection();
            Response.Cookies["SubdividirUbicacion_IdCalle"].Value = string.Empty;
            AutocompleteCallesSub.ClearSelection();
            Response.Cookies["SubdividirUbicacionSub_IdCalle"].Value = string.Empty;
            txtUbiNroPuerta.Text = "";
            txtUbiSeccion.Text = "";
            txtUbiManzana.Text = "";
            txtUbiParcela.Text = "";
            nro_partida_matriz = 0;
            nro_partida_horiz = 0;
            cod_calle = "0";
            nro_puerta = 0;
            seccion = 0;
            manzana = "";
            parcela = "";
            id_tipo_ubicacion = -1;
            id_sub_tipo_ubicacion = -1;
            if (ddlUbiTipoUbicacion.Items.Count >= 0)
                ddlUbiTipoUbicacion.SelectedIndex = 0;

            if (ddlUbiSubTipoUbicacion.Items.Count >= 0)
                ddlUbiSubTipoUbicacion.SelectedIndex = 0;

            pnlResultadoBuscar.Visible = false;
            updPnlFiltroBuscar_ubi_partida.Update();
            updPnlFiltroBuscar_ubi_dom.Update();
            updPnlFiltroBuscar_ubi_smp.Update();
            updPnlFiltroBuscar_ubi_especial.Update();
            updPnlResultadoBuscar.Update();
            EjecutarScript(btn_BuscarPartida, "hideResultado();");
        }

        private void LimpiarCargarParsSub()
        {
            //Limpio los controles del div frmCargarUbicacion

            CargarCombo_tipoUbicacionABM(false);
            CargarCombo_subTipoUbicacionABM(-1, false);
            CargarComboCallesSub();
            CargarComboMixtura();
            CargarCombosDistritos();
            CargarCombos_Barrios();
            CargarCombos_Comisaria();
            CargarCombos_Comunas();

            txtSeccion.Text = "";
            txtManzana.Text = "";
            txtParcela.Text = "";
            txtNroPartida.Text = "";

            ddlUbiTipoUbicacionABM.SelectedIndex = 0;
            ddlUbiSubTipoUbicacionABM.SelectedIndex = 0;
            ddlComisaria.SelectedIndex = 0;
            ddlBarrio.SelectedIndex = 0;
            ddlComuna.SelectedIndex = 0;
            chbEntidadGubernamental.Checked = false;
            chbEdificioProtegido.Checked = false;
            hid_tipo_reqSMP.Value = "1"; // Bit

            DataTable dtc = dtUbicacionesCargadas();
            dtc.Rows.Clear();
            grdCalleSub.DataSource = dtc;
            grdCalleSub.DataBind();

            DataTable dtm = dtMixturasCargadas();
            dtm.Rows.Clear();
            grdMixturas.DataSource = dtm;
            grdMixturas.DataBind();

            ddlGrupoDistritos.SelectedIndex = 0;
            ddlDistritos.SelectedIndex = -1;
            ddlDistritosZonas.SelectedIndex = -1;
            ddlDistritosSubZonas.SelectedIndex = -1;

            DataTable dtd = dtDistritosCargados();
            dtd.Rows.Clear();
            grdDistritos.DataSource = dtd;
            grdDistritos.DataBind();
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
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script);
            }
        }

        private bool ubicacionRepetida(string Calle, int nroPuertaUbi)
        {
            DataTable dt = dtUbicacionesCargadas();

            var existe = (from r in dt.AsEnumerable()
                          where r.Field<string>("calles").Contains(Calle) &&
                                r.Field<int>("nroPuerta") == nroPuertaUbi
                          select r).ToList().Count();

            return existe != 0;
        }

        private string GetNombreCalle(int codCalle, int nroPuerta)
        {
            using (var db = new DGHP_Entities())
            {
                var c = (from cal in db.Calles
                         where cal.Codigo_calle == codCalle
                         && (cal.AlturaIzquierdaInicio_calle <= cal.AlturaDerechaInicio_calle ? cal.AlturaIzquierdaInicio_calle : cal.AlturaDerechaInicio_calle) <= nroPuerta
                         && (cal.AlturaDerechaFin_calle >= cal.AlturaIzquierdaFin_calle ? cal.AlturaDerechaFin_calle : cal.AlturaIzquierdaFin_calle) >= nroPuerta
                         select cal).FirstOrDefault();

                if (c != null)
                    return c.NombreOficial_calle;

                return "";
            }
        }

        private List<string> quitarIdsRepetidos(string[] v)
        {
            List<string> ids = new List<string>();
            foreach (var id in v)
            {
                if (!ids.Contains(id) && id.Trim() != "")
                    ids.Add(id);
            }

            return ids;
        }

        #endregion

        #region Cargar combos

        private void CargarCombo_tipoUbicacion(bool busqueda)
        {
            using (var db = new DGHP_Entities())
            {
                List<TiposDeUbicacion> lista = db.TiposDeUbicacion.Where(x => x.id_tipoubicacion > 0).ToList();

                TiposDeUbicacion tipo_ubi = new TiposDeUbicacion();
                tipo_ubi.id_tipoubicacion = 0;
                tipo_ubi.descripcion_tipoubicacion = (busqueda) ? "(Sin Especificar)" : "Parcela Común";
                lista.Insert(0, tipo_ubi);
                if (busqueda)
                {
                    ddlUbiTipoUbicacion.DataTextField = "descripcion_tipoubicacion";
                    ddlUbiTipoUbicacion.DataValueField = "id_tipoubicacion";
                    ddlUbiTipoUbicacion.DataSource = lista;
                    ddlUbiTipoUbicacion.DataBind();
                }
            }
        }

        private void CargarCombo_subTipoUbicacion(int id_tipoubicacion, Boolean busqueda)
        {
            using (var db = new DGHP_Entities())
            {
                List<SubTiposDeUbicacion> lista = db.SubTiposDeUbicacion
                .Where(x => x.habilitado == true)
                .Where(x => x.id_tipoubicacion == id_tipoubicacion).ToList();

                SubTiposDeUbicacion sub_tipo_ubi = new SubTiposDeUbicacion();
                sub_tipo_ubi.id_subtipoubicacion = 0;
                sub_tipo_ubi.descripcion_subtipoubicacion = (busqueda) ? "Seleccione" : "Ninguno";
                lista.Insert(0, sub_tipo_ubi);
                if (busqueda)
                {
                    ddlUbiSubTipoUbicacion.DataTextField = "descripcion_subtipoubicacion";
                    ddlUbiSubTipoUbicacion.DataValueField = "id_subtipoubicacion";
                    ddlUbiSubTipoUbicacion.DataSource = lista.OrderBy(x => x.descripcion_subtipoubicacion);
                    ddlUbiSubTipoUbicacion.DataBind();
                }
            }
        }

        private void CargarCombosDistritos()
        {
            using (var db = new DGHP_Entities())
            {
                List<Ubicaciones_GruposDistritos> lista = db.Ubicaciones_GruposDistritos.ToList();

                lista.Insert(0, new Ubicaciones_GruposDistritos { IdGrupoDistrito = 0, Nombre = "[Seleccione...]" });

                ddlGrupoDistritos.DataTextField = "Nombre";
                ddlGrupoDistritos.DataValueField = "IdGrupoDistrito";
                ddlGrupoDistritos.DataSource = lista.OrderBy(x => x.Nombre);
                ddlGrupoDistritos.DataBind();
            }
        }

        private void CargarCombo_Distritos(int IdGrupoDistrito, bool value)
        {
            using (var db = new DGHP_Entities())
            {
                List<Ubicaciones_CatalogoDistritos> lista = db.Ubicaciones_CatalogoDistritos.Where(x => x.IdGrupoDistrito == IdGrupoDistrito).ToList();

                lista.Insert(0, new Ubicaciones_CatalogoDistritos { IdDistrito = 0, Codigo = "[Seleccione...]" });

                ddlDistritos.DataTextField = "Codigo";
                ddlDistritos.DataValueField = "IdDistrito";
                ddlDistritos.DataSource = lista.OrderBy(x => x.Codigo);
                ddlDistritos.DataBind();

                ddlDistritos.Enabled = value;

                ddlDistritosZonas.SelectedIndex = -1;
            }
        }

        private void CargarCombo_DistritosZonas(int IdDistrito, bool value)
        {
            using (var db = new DGHP_Entities())
            {
                List<Ubicaciones_CatalogoDistritos_Zonas> lista = db.Ubicaciones_CatalogoDistritos_Zonas.Where(x => x.IdDistrito == IdDistrito).ToList();

                lista.Insert(0, new Ubicaciones_CatalogoDistritos_Zonas { IdZona = 0, CodigoZona = "[Seleccione...]" });

                ddlDistritosZonas.DataTextField = "CodigoZona";
                ddlDistritosZonas.DataValueField = "IdZona";
                ddlDistritosZonas.DataSource = lista.OrderBy(x => x.CodigoZona);
                ddlDistritosZonas.DataBind();
                ddlDistritosZonas.Enabled = value;

                ddlDistritosSubZonas.SelectedIndex = -1;
            }
        }

        private void CargarCombo_DistritosSubZonas(int idZona, bool value)
        {
            using (var db = new DGHP_Entities())
            {
                List<Ubicaciones_CatalogoDistritos_Subzonas> lista = db.Ubicaciones_CatalogoDistritos_Subzonas.Where(x => x.IdZona == idZona).ToList();

                lista.Insert(0, new Ubicaciones_CatalogoDistritos_Subzonas { IdSubZona = 0, CodigoSubZona = "[Seleccione...]" });

                ddlDistritosSubZonas.DataTextField = "CodigoSubZona";
                ddlDistritosSubZonas.DataValueField = "IdSubZona";
                ddlDistritosSubZonas.DataSource = lista.OrderBy(x => x.CodigoSubZona);
                ddlDistritosSubZonas.DataBind();
                ddlDistritosSubZonas.Enabled = value;
            }
        }

        private void CargarComboMixtura()
        {
            using (var db = new DGHP_Entities())
            {
                List<Ubicaciones_ZonasMixtura> lista = db.Ubicaciones_ZonasMixtura.ToList();

                Ubicaciones_ZonasMixtura item = new Ubicaciones_ZonasMixtura();
                item.IdZonaMixtura = 0;
                item.Descripcion = "[Seleccione...]";
                lista.Insert(0, item);
                ddlMixtura.DataTextField = "Descripcion";
                ddlMixtura.DataValueField = "IdZonaMixtura";
                ddlMixtura.DataSource = lista.OrderBy(x => x.Descripcion);
                ddlMixtura.DataBind();
            }
        }

        private void CargarCombos_Comisaria()
        {
            using (var db = new DGHP_Entities())
            {
                List<Comisarias> comisarias = db.Comisarias.Where(a => a.id_comisaria != 54 && a.id_comisaria != 56).ToList();
                ddlComisaria.DataTextField = "nom_comisaria";
                ddlComisaria.DataValueField = "id_comisaria";
                ddlComisaria.DataSource = comisarias.OrderBy(b => b.nom_comisaria);
                ddlComisaria.DataBind();
            }
        }

        private void CargarCombos_Barrios()
        {
            using (var db = new DGHP_Entities())
            {
                List<Barrios> barrios = db.Barrios.Where(a => a.id_barrio != 50).ToList();
                ddlBarrio.DataTextField = "nom_barrio";
                ddlBarrio.DataValueField = "id_barrio";
                ddlBarrio.DataSource = barrios.OrderBy(p => p.nom_barrio);
                ddlBarrio.DataBind();
            }
        }

        private void CargarCombos_Comunas()
        {
            using (var db = new DGHP_Entities())
            {
                List<Comunas> comunas = db.Comunas.Where(a => a.id_comuna != 17).ToList();
                ddlComuna.DataTextField = "nom_comuna";
                ddlComuna.DataValueField = "id_comuna";
                ddlComuna.DataSource = comunas.OrderBy(p => p.nom_comuna);
                ddlComuna.DataBind();
            }
        }

        private void CargarCombo_tipoUbicacionABM(Boolean busqueda)
        {
            using (var db = new DGHP_Entities())
            {
                List<TiposDeUbicacion> lista = db.TiposDeUbicacion.ToList();

                ddlUbiTipoUbicacionABM.DataTextField = "descripcion_tipoubicacion";
                ddlUbiTipoUbicacionABM.DataValueField = "id_tipoubicacion";

                ddlUbiTipoUbicacionABM.DataSource = lista.OrderBy(x => x.id_tipoubicacion);

                ddlUbiTipoUbicacionABM.DataBind();
            }
        }

        private void CargarCombo_subTipoUbicacionABM(int id_tipoubicacion, Boolean busqueda)
        {
            using (var db = new DGHP_Entities())
            {
                List<SubTiposDeUbicacion> lista = db.SubTiposDeUbicacion.Where(x => x.id_tipoubicacion == id_tipoubicacion).ToList();

                SubTiposDeUbicacion sub_tipo_ubi = new SubTiposDeUbicacion();
                sub_tipo_ubi.id_subtipoubicacion = 0;
                sub_tipo_ubi.descripcion_subtipoubicacion = (busqueda) ? "Seleccione" : "Ninguno";
                lista.Insert(0, sub_tipo_ubi);

                ddlUbiSubTipoUbicacionABM.DataTextField = "descripcion_subtipoubicacion";
                ddlUbiSubTipoUbicacionABM.DataValueField = "id_subtipoubicacion";
                ddlUbiSubTipoUbicacionABM.DataSource = lista.OrderBy(x => x.descripcion_subtipoubicacion);
                ddlUbiSubTipoUbicacionABM.DataBind();
            }
        }

        internal class ItemCalle
        {
            public string NombreOficial_calle { get; set; }
            public int Codigo_calle { get; set; }
            //public int? AlturaMin { get; set; }
            //public int? AlturaMax { get; set; }
        }

        private void CargarComboCalles()
        {
            Functions.CargarAutocompleteCalles(AutocompleteCalles);
        }

        private void CargarComboCallesSub()
        {
            Functions.CargarAutocompleteCalles(AutocompleteCallesSub);
        }

        #endregion

        #region Operaciones

        // Devuelve el id de la operacion de subdivision en proceso según el id de la parela a subdividir para el usuario actual
        private int getIdOperacionSubdivEnProceso(int id_ubi_padre)
        {
            // Obtenemos el user 
            Guid userid = Functions.GetUserId();
            using (DGHP_Entities db = new DGHP_Entities())
            {
                // Busco la operación de subdivision según el id de la parcela a subdividir
                var op_det = (from ubiOperacionesDet in db.Ubicaciones_Operaciones_Detalle
                              join op in db.Ubicaciones_Operaciones on ubiOperacionesDet.id_operacion equals op.id_operacion
                              join ac in db.Ubicaciones_Acciones on op.id_accion equals ac.id_accion
                              join est in db.Ubicaciones_Estados on op.id_estado equals est.id_estado
                              where ac.Descripcion.Contains("Subdivisión") && est.Descripcion.Contains("En Proceso")
                              && op.CreateUser == userid && ubiOperacionesDet.id_ubicacion == id_ubi_padre
                              select ubiOperacionesDet).FirstOrDefault();

                // Devuelvo el id de la operacion
                if (op_det != null)
                    return op_det.id_operacion;

                return 0;
            }
        }

        private int getIdPadreOpSubdivEnProceso(int id_opercion)
        {
            if (id_opercion == 0)
                return 0;

            // Obtenemos el user 
            Guid userid = Functions.GetUserId();

            using (var db = new DGHP_Entities())
            {
                // Busco la ubicacion a subdividir
                var op_det = (from ubiOperacionesDet in db.Ubicaciones_Operaciones_Detalle
                              join op in db.Ubicaciones_Operaciones on ubiOperacionesDet.id_operacion equals op.id_operacion
                              join ac in db.Ubicaciones_Acciones on op.id_accion equals ac.id_accion
                              join est in db.Ubicaciones_Estados on op.id_estado equals est.id_estado
                              where op.id_operacion == id_opercion && op.CreateUser == userid
                              && ac.Descripcion.Contains("Subdiv") && est.Descripcion.Contains("En Proceso")
                              && ubiOperacionesDet.id_ubicacion != null && ubiOperacionesDet.id_ubicacion_temp == null
                              select ubiOperacionesDet).FirstOrDefault();

                // Devuelvo el id de la operacion
                if (op_det != null)
                    return (int)op_det.id_ubicacion;
            }
            return 0;
        }

        private int[] getIdsUbisTemp(int id_operacion)
        {
            using (var db = new DGHP_Entities())
            {
                // Busco las ubicaciones temporales vinculadas a una operación
                var ubis_temp = (from ubi in db.Ubicaciones_temp
                                 join op in db.Ubicaciones_Operaciones_Detalle on ubi.id_ubicacion_temp equals op.id_ubicacion_temp
                                 where op.id_operacion == id_operacion && ubi.baja_logica == false
                                 select ubi).ToList();

                // Si no hay resultados
                if (ubis_temp.Count == 0)
                    return null;

                // Genero un array con los ids y los retorno
                int i = 0;
                int a = ubis_temp.Count;
                int[] ids = new int[a];
                foreach (var u in ubis_temp)
                {
                    ids[i] = u.id_ubicacion_temp;
                    i++;
                }

                return ids;
            }
        }

        private int GenerarOperacion(int accion, int estado, Guid uid)
        {
            using (var ctx = new DGHP_Entities())
            {
                // Nueva operación
                var ope = new Ubicaciones_Operaciones
                {
                    id_accion = accion,
                    CreateDate = DateTime.Now,
                    CreateUser = uid,
                    id_estado = estado
                };

                ctx.Ubicaciones_Operaciones.Add(ope);
                var res = ctx.SaveChanges();

                if (res != 0)
                    return ope.id_operacion;
            }

            return 0;
        }

        private int GenerarOperacionSubDivEnProceso()
        {
            // Subdivisión
            int id_acc = 5;

            // Obtenemos el user 
            Guid userid = Functions.GetUserId();

            // En Proceso
            int id_est = 0;

            return GenerarOperacion(id_acc, id_est, userid);
        }

        private int GenerarOpDetalle(int id_ope, int id_ubi = 0, int id_ubi_temp = 0, string detalle = "")
        {
            using (var ctx = new DGHP_Entities())
            {
                Ubicaciones_Operaciones_Detalle op_existe = null;

                // Nueva op detalle
                var op_det = new Ubicaciones_Operaciones_Detalle { id_operacion = id_ope };

                if (id_ubi != 0)
                {
                    op_det.id_ubicacion = id_ubi;
                    op_existe = ctx.Ubicaciones_Operaciones_Detalle.Where(o => o.id_operacion == id_ope && o.id_ubicacion == id_ubi).FirstOrDefault();
                }

                if (id_ubi_temp != 0)
                {
                    op_det.id_ubicacion_temp = id_ubi_temp;
                    op_existe = ctx.Ubicaciones_Operaciones_Detalle.Where(o => o.id_operacion == id_ope && o.id_ubicacion_temp == id_ubi_temp).FirstOrDefault();
                }

                if (detalle != "")
                    op_det.Detalle = detalle;

                // Agregamos y guardamos
                if (op_existe == null)
                {
                    ctx.Ubicaciones_Operaciones_Detalle.Add(op_det);
                    var res = ctx.SaveChanges();
                    if (res != 0)
                        return op_det.id_operacion_det;
                }
                else
                    return op_existe.id_operacion_det;
            }

            return 0;
        }

        private bool ConfirmarOperacion(int id_op)
        {
            using (var ctx = new DGHP_Entities())
            {
                var op = ctx.Ubicaciones_Operaciones.Where(o => o.id_operacion == id_op).FirstOrDefault();
                if (op != null)
                    op.id_estado = 2; // Confirmada
                return ctx.SaveChanges() != 0;
            }
        }

        private int ExisteUbicacionOp(int id_temp, int id_op)
        {
            using (var ctx = new DGHP_Entities())
            {
                // Verificamos si ya esta registrada la ubicacion 
                if (id_op != 0)
                {
                    var op = ctx.Ubicaciones_Operaciones_Detalle.Where(o => o.id_ubicacion_temp == id_temp && o.id_operacion == id_op).FirstOrDefault();
                    if (op != null)
                        return op.id_ubicacion != null ? (int)op.id_ubicacion : 0;
                }
                return 0;
            }
        }

        private bool ActualizarOperacionIdUbicacion(int id_operacion, int id_temp, int id_ubicacion)
        {
            using (var ctx = new DGHP_Entities())
            {
                // Actualizamos el id_ubicacion de la operación 
                if (id_operacion != 0)
                {
                    var op = ctx.Ubicaciones_Operaciones_Detalle.Where(o => o.id_ubicacion_temp == id_temp && o.id_operacion == id_operacion).FirstOrDefault();
                    if (op != null)
                        op.id_ubicacion = id_ubicacion;

                    var res = ctx.SaveChanges();
                    return res > 0;
                }
                return false;
            }
        }

        private bool AnularOperacion(int id_op)
        {
            if (id_op <= 0)
                return false;

            using (var ctx = new DGHP_Entities())
            {
                // Buscamos la operacion para anularla
                var op = ctx.Ubicaciones_Operaciones.Where(o => o.id_operacion == id_op).FirstOrDefault();
                if (op != null)
                {
                    // Buscamos si tiene ubicaciones temporales 
                    var ops_det = ctx.Ubicaciones_Operaciones_Detalle.Where(o => o.id_operacion == id_op && o.id_ubicacion_temp != null).ToList();
                    foreach (var op_d in ops_det)
                    {
                        // Damos de baja las ubicaciones temporales
                        var ubi = ctx.Ubicaciones_temp.Where(u => u.id_ubicacion_temp == op_d.id_ubicacion_temp).SingleOrDefault();
                        if (ubi != null)
                            ubi.baja_logica = true;
                    }

                    op.id_estado = 1; // Anulada

                    var res = ctx.SaveChanges();
                    return res != 0;
                }
            }

            return false;
        }

        #endregion

        #region Ubicaciones

        private bool BajaUbicacion(int id_ubi)
        {
            using (var ctx = new DGHP_Entities())
            {
                var ubi = ctx.Ubicaciones.Where(u => u.id_ubicacion == id_ubi).SingleOrDefault();
                if (ubi != null)
                {
                    ubi.baja_logica = true;

                    //Comunico las bajas a Fachadas
                    string User = Parametros.GetParam_ValorChar("User.Ley257");
                    string Pass = Parametros.GetParam_ValorChar("Pass.Ley257");
                    string URL = Parametros.GetParam_ValorChar("URL.Ley257");
                    var ActionLogin = Parametros.GetParam_ValorChar("Action.Login.Ley257");
                    var ActionDarBajaUbicacion = Parametros.GetParam_ValorChar("Action.DarBajaUbicacion.Ley257");

                    if (!string.IsNullOrEmpty(ActionDarBajaUbicacion))
                    {
                        ws_Ley257 serv = new ws_Ley257();
                        var tokenResponse = serv.Token(URL, ActionLogin, User, Pass);

                        if (tokenResponse.IsSuccess)
                        {
                            var token = (Ley257Token)tokenResponse.Result;
                            var data = new Ley257RequestDarBajaUbicacion
                            {
                                Seccion = (int)ubi.Seccion,
                                Manzana = ubi.Manzana,
                                Parcela = ubi.Parcela,
                                UbicacionID = ubi.id_ubicacion
                            };

                            // Llamo al método DarBajaUbicacion
                            var darBajaResponse = serv.DarBajaUbicacion(token.AccessToken, URL, ActionDarBajaUbicacion, data);

                            if (darBajaResponse.IsSuccess)
                            {
                                string Message = darBajaResponse.Message;
                            }
                            else
                            {
                                string errorMessage = darBajaResponse.Message;
                            }
                        }
                        else
                        {
                            string errorMessage = tokenResponse.Message;
                        }
                    }
                }

                return ctx.SaveChanges() != 0;
            }
        }

        private List<Ubicaciones_Puertas_temp> CrearListaPuertasTemp(int id_ubicacion_temp, GridView grdCalleSub)
        {
            var list = new List<Ubicaciones_Puertas_temp>();

            var id = NuevoIdUbicacionPuertaTemp();
            foreach (GridViewRow item in grdCalleSub.Rows)
            {
                Ubicaciones_Puertas_temp pu = new Ubicaciones_Puertas_temp();
                pu.id_ubic_puerta_temp = id;
                pu.id_ubicacion_temp = id_ubicacion_temp;
                pu.NroPuerta_ubic = Convert.ToInt32(item.Cells[1].Text);
                pu.codigo_calle = Convert.ToInt32(item.Cells[2].Text);
                pu.tipo_puerta = "";
                list.Add(pu);
                id++;
            }
            return list;
        }

        private List<Ubicaciones_Distritos_temp> CrearListaDistritosTemp(int id_ubicacion_temp, GridView grdDistritos)
        {
            var list = new List<Ubicaciones_Distritos_temp>();

            foreach (GridViewRow item in grdDistritos.Rows)
            {
                Ubicaciones_Distritos_temp dis = new Ubicaciones_Distritos_temp();

                dis.id_ubicacion_temp = id_ubicacion_temp;
                dis.IdDistrito = Convert.ToInt16(item.Cells[4].Text);

                int? id_Zona = null;
                int? id_SubZona = null;

                if (item.Cells[5].Text != "0" && item.Cells[5].Text != "&nbsp;")
                {
                    id_Zona = Convert.ToInt16(item.Cells[5].Text);
                }
                if (item.Cells[6].Text != "0" && item.Cells[6].Text != "&nbsp;")
                {
                    id_SubZona = Convert.ToInt16(item.Cells[6].Text);
                }
                dis.IdZona = id_Zona;
                dis.IdSubZona = id_SubZona;
                list.Add(dis);
            }
            return list;
        }

        private List<Ubicaciones_ZonasMixtura> CrearListaMixturas_temp(int id_ubi_temp, GridView grdMixturas)
        {
            var list = new List<Ubicaciones_ZonasMixtura>();

            foreach (GridViewRow item in grdMixturas.Rows)
            {
                list.Add(new Ubicaciones_ZonasMixtura { IdZonaMixtura = Convert.ToInt16(item.Cells[0].Text) });
            }
            return list;
        }

        private int NuevoIdUbicacionPuertaTemp()
        {
            using (var db = new DGHP_Entities())
            {
                if (db.Ubicaciones_Puertas_temp.ToList().Count() == 0)
                    return 1;

                // Buscamos el id max
                int? id_max = db.Ubicaciones_Puertas_temp.Max(p => p.id_ubic_puerta_temp);

                // Devolvemos el max + 1
                if (id_max != null)
                    return (int)(id_max + 1);

                // Error
                return -1;
            }
        }

        private int NuevoIdUbicacionPuerta()
        {
            using (var db = new DGHP_Entities())
            {
                if (db.Ubicaciones_Puertas.ToList().Count() == 0)
                    return 1;

                // Buscamos el id max
                int? id_max = db.Ubicaciones_Puertas.Max(p => p.id_ubic_puerta);

                // Devolvemos el max + 1
                if (id_max != null)
                    return (int)(id_max + 1);

                // Error
                return -1;
            }
        }

        private int NuevoId_ubicacion()
        {
            using (var db = new DGHP_Entities())
            {
                // Buscamos el id max
                int? id_max = db.Ubicaciones.Max(p => p.id_ubicacion);

                // Devolvemos el max + 1
                if (id_max != null)
                    return (int)(id_max + 1);

                // Error
                return -1;
            }
        }

        private int NuevoId_ubicacion_temp()
        {
            using (var db = new DGHP_Entities())
            {
                if (db.Ubicaciones_temp.ToList().Count() == 0)
                    return 1;

                // Buscamos el id max
                int? id_max = db.Ubicaciones_temp.Max(p => p.id_ubicacion_temp);

                // Devolvemos el max + 1
                if (id_max != null)
                    return (int)(id_max + 1);

                // Error
                return -1;
            }
        }

        private int GuardarUbicacionFromTmp(int id_ubi_tmp)
        {
            int result = 0;
            using (var context = new DGHP_Entities())
            {
                var ubi_temp = context.Ubicaciones_temp.Where(u => u.id_ubicacion_temp == id_ubi_tmp).SingleOrDefault();
                if (ubi_temp == null)
                    return 0;

                int idNuevo = NuevoId_ubicacion();
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region Ubicacion
                        var entity = new SGI.Model.Ubicaciones()
                        {
                            id_ubicacion = idNuevo,
                            id_subtipoubicacion = ubi_temp.id_subtipoubicacion,
                            baja_logica = ubi_temp.baja_logica,
                            CantiActualizacionesUSIG = ubi_temp.CantiActualizacionesUSIG,
                            cant_ph = ubi_temp.cant_ph,
                            Circunscripcion = ubi_temp.Circunscripcion,
                            id_comisaria = ubi_temp.id_comisaria,
                            id_areahospitalaria = ubi_temp.id_areahospitalaria,
                            id_barrio = ubi_temp.id_barrio,
                            id_comuna = ubi_temp.id_comuna,
                            id_distritoescolar = ubi_temp.id_distritoescolar,
                            id_regionsanitaria = ubi_temp.id_regionsanitaria,
                            id_zonaplaneamiento = ubi_temp.id_zonaplaneamiento,
                            CreateDate = DateTime.Now,
                            VigenciaDesde = DateTime.Now,
                            CreateUser = Functions.GetUserId(),
                            UpdateDate = DateTime.Now,
                            UpdateUser = Functions.GetUserId(),
                            EsEntidadGubernamental = ubi_temp.EsEntidadGubernamental,
                            EsUbicacionProtegida = ubi_temp.EsUbicacionProtegida,
                            NroPartidaMatriz = ubi_temp.NroPartidaMatriz,
                            Seccion = ubi_temp.Seccion,
                            Manzana = ubi_temp.Manzana,
                            Parcela = ubi_temp.Parcela
                        };

                        #endregion
                        // Agregamos la Ubicacion 
                        context.Ubicaciones.Add(entity);

                        // Agregamos las Puertas 
                        var puertas_tmp = context.Ubicaciones_Puertas_temp.Where(p => p.id_ubicacion_temp == ubi_temp.id_ubicacion_temp).ToList();
                        int id_pue_ini = NuevoIdUbicacionPuerta();
                        foreach (var pue_t in puertas_tmp)
                        {
                            var p = new Ubicaciones_Puertas();

                            p.id_ubic_puerta = id_pue_ini;
                            p.id_ubicacion = idNuevo;
                            p.codigo_calle = pue_t.codigo_calle;
                            p.NroPuerta_ubic = pue_t.NroPuerta_ubic;
                            p.tipo_puerta = pue_t.tipo_puerta;

                            entity.Ubicaciones_Puertas.Add(p);
                            id_pue_ini++;
                        }

                        // Agregamos las Mixturas 
                        entity.Ubicaciones_ZonasMixtura = ubi_temp.Ubicaciones_ZonasMixtura;

                        // Agregamos los Distritos
                        var dist_tmp = context.Ubicaciones_Distritos_temp.Where(d => d.id_ubicacion_temp == ubi_temp.id_ubicacion_temp).ToList();
                        foreach (var item in dist_tmp)
                        {
                            Ubicaciones_Distritos dis = new Ubicaciones_Distritos();

                            dis.id_ubicacion = idNuevo;
                            dis.IdDistrito = item.IdDistrito;
                            dis.IdZona = item.IdZona;
                            dis.IdSubZona = item.IdSubZona;

                            entity.Ubicaciones_Distritos.Add(dis);
                        }

                        // Guardamos todos los cambios
                        context.SaveChanges();
                        dbContextTransaction.Commit();

                        result = idNuevo;
                    }
                    catch (Exception ex)
                    {
                        result = 0;
                        dbContextTransaction.Rollback();
                        lblError.Text = Functions.GetErrorMessage(ex);
                        this.EjecutarScript(updBotonesGuardarCalleSub, "showfrmError();");
                    }
                }
            }

            return result;
        }

        public List<SGI.Model.Shared.ItemUbicacion> GetResultado(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {
            using (var db = new DGHP_Entities())
            {
                totalRowCount = 0;

                List<SGI.Model.Shared.ItemUbicacion> resultados = new List<SGI.Model.Shared.ItemUbicacion>();

                int id = 0;
                if (hid_id_ubi_padre.Value != "" && int.TryParse(hid_id_ubi_padre.Value, out id))
                {
                    var qa = (from qs in db.Ubicaciones
                              join su in db.SubTiposDeUbicacion on qs.id_subtipoubicacion equals su.id_subtipoubicacion
                              join tu in db.TiposDeUbicacion on su.id_tipoubicacion equals tu.id_tipoubicacion
                              join ubiPuertas in db.Ubicaciones_Puertas on qs.id_ubicacion equals ubiPuertas.id_ubicacion into up
                              from ubiPuertas in up.DefaultIfEmpty()
                              join callesNombre in db.Calles on ubiPuertas != null ? ubiPuertas.codigo_calle : 0 equals callesNombre.Codigo_calle into cl
                              from callesNombre in cl.DefaultIfEmpty()
                              where qs.id_ubicacion == id && (ubiPuertas != null ?
                            (ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaIzquierdaInicio_calle || ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaDerechaInicio_calle)
                            && (ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaIzquierdaFin_calle || ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaDerechaFin_calle) : true)
                              select new SGI.Model.Shared.ItemUbicacion
                              {
                                  id_ubicacion = qs.id_ubicacion,
                                  manzana = qs.Manzana,
                                  seccion = qs.Seccion,
                                  parcela = qs.Parcela,
                                  partida = qs.NroPartidaMatriz,
                                  inhibida = qs.Inhibida_Observacion,
                                  id_subtipoubicacion = qs.id_subtipoubicacion,
                                  id_tipoubicacion = su.id_tipoubicacion,
                                  direccion = (callesNombre != null ? callesNombre.NombreOficial_calle + " " : "") + (ubiPuertas != null ? ubiPuertas.NroPuerta_ubic.ToString() : ""),
                                  id_estado_modif_ubi = 0,
                                  baja_logica = qs.baja_logica == false ? "No" : "Si",
                                  codigo_calle = callesNombre != null ? callesNombre.Codigo_calle : 0,
                                  nombre_calle = callesNombre != null ? callesNombre.NombreOficial_calle : "",
                                  nro_puerta = ubiPuertas != null ? ubiPuertas.NroPuerta_ubic : 0
                              }
                    ).Distinct();

                    var u = (from ubi in qa.AsEnumerable()
                             group ubi by new
                             {
                                 ubi.id_ubicacion,
                                 ubi.manzana,
                                 ubi.seccion,
                                 ubi.parcela,
                                 ubi.partida,
                                 ubi.inhibida,
                                 ubi.id_subtipoubicacion,
                                 ubi.id_tipoubicacion,
                                 ubi.id_estado_modif_ubi,
                                 ubi.baja_logica
                             } into ubis

                             select new SGI.Model.Shared.ItemUbicacion
                             {
                                 id_ubicacion = ubis.Key.id_ubicacion,
                                 manzana = ubis.Key.manzana,
                                 seccion = ubis.Key.seccion,
                                 parcela = ubis.Key.parcela,
                                 partida = ubis.Key.partida,
                                 inhibida = ubis.Key.inhibida,
                                 id_subtipoubicacion = ubis.Key.id_subtipoubicacion,
                                 id_tipoubicacion = ubis.Key.id_tipoubicacion,
                                 id_estado_modif_ubi = ubis.Key.id_estado_modif_ubi,
                                 baja_logica = ubis.Key.baja_logica,
                                 direccion = string.Join(" / ", ubis.Select(x => x.direccion))
                             }).SingleOrDefault();

                    if (u != null)
                        resultados.Add(u);

                    totalRowCount = resultados.Count();
                }
                return resultados;
            }
        }

        public List<SGI.Model.Shared.ItemUbicacion> GetResultadosSub(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {
            using (var db = new DGHP_Entities())
            {
                totalRowCount = 0;

                List<SGI.Model.Shared.ItemUbicacion> resultados = new List<SGI.Model.Shared.ItemUbicacion>();

                List<Ubicaciones_temp> lubis = new List<Ubicaciones_temp>();

                var aux = quitarIdsRepetidos(hid_ids_ubi_sub.Value.Split(','));
                foreach (var a in aux)
                {
                    int i = 0;
                    if (int.TryParse(a, out i))
                    {
                        var u = db.Ubicaciones_temp.Where(b => b.id_ubicacion_temp == i).SingleOrDefault();
                        if (u != null)
                            lubis.Add(u);
                    }
                }

                if (lubis.Count > 0)
                {
                    var qa = (from qs in lubis
                              join su in db.SubTiposDeUbicacion on qs.id_subtipoubicacion equals su.id_subtipoubicacion
                              join tu in db.TiposDeUbicacion on su.id_tipoubicacion equals tu.id_tipoubicacion
                              join ubiPuertas in db.Ubicaciones_Puertas_temp on qs.id_ubicacion_temp equals ubiPuertas.id_ubicacion_temp into up
                              from ubiPuertas in up.DefaultIfEmpty()
                              join callesNombre in db.Calles on ubiPuertas != null ? ubiPuertas.codigo_calle : 0 equals callesNombre.Codigo_calle into cl
                              from callesNombre in cl.DefaultIfEmpty()
                              where (ubiPuertas != null ?
                            (ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaIzquierdaInicio_calle || ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaDerechaInicio_calle)
                            && (ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaIzquierdaFin_calle || ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaDerechaFin_calle) : true)
                              select new SGI.Model.Shared.ItemUbicacion
                              {
                                  id_ubicacion = qs.id_ubicacion_temp,
                                  manzana = qs.Manzana,
                                  seccion = qs.Seccion,
                                  parcela = qs.Parcela,
                                  partida = qs.NroPartidaMatriz,
                                  id_subtipoubicacion = qs.id_subtipoubicacion,
                                  id_tipoubicacion = su.id_tipoubicacion,
                                  direccion = (callesNombre != null ? callesNombre.NombreOficial_calle + " " : "") + (ubiPuertas != null ? ubiPuertas.NroPuerta_ubic.ToString() : ""),
                                  codigo_calle = callesNombre != null ? callesNombre.Codigo_calle : 0,
                                  nombre_calle = callesNombre != null ? callesNombre.NombreOficial_calle : "",
                                  nro_puerta = ubiPuertas != null ? ubiPuertas.NroPuerta_ubic : 0
                              }).Distinct();

                    var u = (from ubi in qa.AsEnumerable()
                             group ubi by new
                             {
                                 ubi.id_ubicacion,
                                 ubi.manzana,
                                 ubi.seccion,
                                 ubi.parcela,
                                 ubi.partida,
                                 ubi.inhibida,
                                 ubi.id_subtipoubicacion,
                                 ubi.id_tipoubicacion,
                                 ubi.id_estado_modif_ubi,
                                 ubi.baja_logica
                             } into ubis

                             select new SGI.Model.Shared.ItemUbicacion
                             {
                                 id_ubicacion = ubis.Key.id_ubicacion,
                                 manzana = ubis.Key.manzana,
                                 seccion = ubis.Key.seccion,
                                 parcela = ubis.Key.parcela,
                                 partida = ubis.Key.partida,
                                 inhibida = ubis.Key.inhibida,
                                 id_subtipoubicacion = ubis.Key.id_subtipoubicacion,
                                 id_tipoubicacion = ubis.Key.id_tipoubicacion,
                                 id_estado_modif_ubi = ubis.Key.id_estado_modif_ubi,
                                 baja_logica = ubis.Key.baja_logica,
                                 direccion = string.Join(" / ", ubis.Select(x => x.direccion))
                             }
                          );

                    totalRowCount = u.Count();
                    resultados = u.OrderBy(o => o.direccion).Skip(startRowIndex).Take(maximumRows).ToList();

                    pnlCantidadRegistros.Visible = (resultados.Count > 0);
                    lblCantidadRegistros.Text = totalRowCount.ToString();
                }

                return resultados;
            }
        }

        public List<SGI.Model.Shared.ItemUbicacion> GetResultados(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {
            totalRowCount = 0;

            using (var db = new DGHP_Entities())
            {
                if (!filtrar)
                {
                    pnlResultadoBuscar.Visible = false;
                    updPnlResultadoBuscar.Update();
                    return null;
                }

                // Preparamos el listado de Ubicaciones aplicando los filtros
                var w = db.Ubicaciones.Where(x => x.id_ubicacion > 0);

                bool bajaLogica = false;
                bool.TryParse(ddlBaja.SelectedValue, out bajaLogica);
                w = w.Where(x => x.baja_logica == bajaLogica);

                // Partida Matriz
                if ((this.nro_partida_matriz > 0) && (rbtnUbiPartidaMatriz.Checked))
                {
                    filtroPHSubGrilla = bajaLogica;
                    w = w.Where(x => x.NroPartidaMatriz == this.nro_partida_matriz);
                }

                // Propiedad Horizontal
                bool bajaLogicaPH = false;
                bool.TryParse(ddlPHDadaDBaja.SelectedValue, out bajaLogicaPH);
                if ((this.nro_partida_horiz > 0) && (rbtnUbiPartidaHoriz.Checked))
                {
                    w = (from res in w
                         join ubiPartidas in db.Ubicaciones_PropiedadHorizontal on res.id_ubicacion equals ubiPartidas.id_ubicacion
                         where ubiPartidas.NroPartidaHorizontal == nro_partida_horiz && ubiPartidas.baja_logica == bajaLogicaPH
                         select res);

                    filtroPHSubGrilla = bajaLogicaPH;
                }

                // Codigo de calle y numero de puerta
                int codigo_calle = 0;
                if (this.cod_calle.Trim() != "")
                {
                    codigo_calle = int.Parse(this.cod_calle);
                }

                // SMP
                if (this.seccion > 0)
                    w = w.Where(x => x.Seccion == this.seccion);
                if (this.manzana.Trim().Length > 0)
                    w = w.Where(x => x.Manzana.Contains(this.manzana.Trim()));
                if (this.parcela.Trim().Length > 0)
                    w = w.Where(x => x.Parcela.Contains(this.parcela.Trim()));

                var qa = (from qs in w
                          join su in db.SubTiposDeUbicacion on qs.id_subtipoubicacion equals su.id_subtipoubicacion
                          join tu in db.TiposDeUbicacion on su.id_tipoubicacion equals tu.id_tipoubicacion
                          join ubiPuertas in db.Ubicaciones_Puertas on qs.id_ubicacion equals ubiPuertas.id_ubicacion into up
                          from ubiPuertas in up.DefaultIfEmpty()
                          join callesNombre in db.Calles on ubiPuertas != null ? ubiPuertas.codigo_calle : 0 equals callesNombre.Codigo_calle into cl
                          from callesNombre in cl.DefaultIfEmpty()
                          where (ubiPuertas != null ?
                          (ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaIzquierdaInicio_calle || ubiPuertas.NroPuerta_ubic >= callesNombre.AlturaDerechaInicio_calle)
                            && (ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaIzquierdaFin_calle || ubiPuertas.NroPuerta_ubic <= callesNombre.AlturaDerechaFin_calle) : true)
                            && (codigo_calle > 0 ? callesNombre.Codigo_calle == codigo_calle : true)
                            && (this.nro_puerta > 0 ? ubiPuertas.NroPuerta_ubic == this.nro_puerta : true)
                            && (this.id_sub_tipo_ubicacion >= 0 ? su.id_subtipoubicacion == this.id_sub_tipo_ubicacion : true)
                            && (this.id_tipo_ubicacion >= 0 ? tu.id_tipoubicacion == this.id_tipo_ubicacion : true)

                          select new SGI.Model.Shared.ItemUbicacion
                          {
                              id_ubicacion = qs.id_ubicacion,
                              //manzana = tu.descripcion_tipoubicacion == "Objeto territorial" ? "T" + qs.Manzana : qs.Manzana,
                              manzana = qs.Manzana,
                              seccion = qs.Seccion,
                              //parcela = tu.descripcion_tipoubicacion == "Objeto territorial" ? qs.Parcela + "t" : qs.Parcela,
                              parcela = qs.Parcela,
                              partida = qs.NroPartidaMatriz,
                              inhibida = qs.Inhibida_Observacion,
                              id_subtipoubicacion = qs.id_subtipoubicacion,
                              id_tipoubicacion = su.id_tipoubicacion,
                              direccion = tu.descripcion_tipoubicacion != "Objeto territorial" ?
                                    (callesNombre != null ? callesNombre.NombreOficial_calle + " " : "") + (ubiPuertas != null ? ubiPuertas.NroPuerta_ubic.ToString() : "")
                                    : (callesNombre != null ? callesNombre.NombreOficial_calle + " " : "") + (ubiPuertas != null ? ubiPuertas.NroPuerta_ubic.ToString() + "t" : ""),
                              id_estado_modif_ubi = (db.Ubicaciones_Historial_Cambios.Where(x => x.id_ubicacion.Equals(qs.id_ubicacion)).Count() != 0) ?
                                                     (db.Ubicaciones_Historial_Cambios.Where(x => x.id_ubicacion.Equals(qs.id_ubicacion)).OrderByDescending(o => o.id_ubihistcam).Select(r => r.id_estado_modif).FirstOrDefault()) : -1,
                              baja_logica = qs.baja_logica == false ? "No" : "Si",
                              codigo_calle = callesNombre != null ? callesNombre.Codigo_calle : 0,
                              nombre_calle = callesNombre != null ? callesNombre.NombreOficial_calle : "",
                              nro_puerta = ubiPuertas != null ? ubiPuertas.NroPuerta_ubic : 0
                          }
                       ).Distinct();

                var u = (from ubi in qa.AsEnumerable()
                         group ubi by new
                         {
                             ubi.id_ubicacion,
                             ubi.manzana,
                             ubi.seccion,
                             ubi.parcela,
                             ubi.partida,
                             ubi.inhibida,
                             ubi.id_subtipoubicacion,
                             ubi.id_tipoubicacion,
                             ubi.id_estado_modif_ubi,
                             ubi.baja_logica
                         } into ubis

                         select new SGI.Model.Shared.ItemUbicacion
                         {
                             id_ubicacion = ubis.Key.id_ubicacion,
                             manzana = ubis.Key.manzana,
                             seccion = ubis.Key.seccion,
                             parcela = ubis.Key.parcela,
                             partida = ubis.Key.partida,
                             inhibida = ubis.Key.inhibida,
                             id_subtipoubicacion = ubis.Key.id_subtipoubicacion,
                             id_tipoubicacion = ubis.Key.id_tipoubicacion,
                             id_estado_modif_ubi = ubis.Key.id_estado_modif_ubi,
                             baja_logica = ubis.Key.baja_logica,
                             direccion = string.Join(" / ", ubis.Select(x => x.direccion))
                         });

                totalRowCount = u.Count();
                var resultados = u.OrderBy(o => o.direccion).Skip(startRowIndex).Take(maximumRows).ToList();

                pnlCantidadRegistros.Visible = (resultados.Count > 0);
                lblCantidadRegistros.Text = totalRowCount.ToString();

                return resultados;
            }
        }

        private void Validar_BuscarPorUbicacion()
        {
            this.nro_partida_matriz = 0;
            this.nro_partida_horiz = 0;
            this.cod_calle = "0";
            this.nro_puerta = 0;
            this.seccion = 0;
            this.manzana = "";
            this.parcela = "";
            this.id_tipo_ubicacion = -1;
            this.id_sub_tipo_ubicacion = -1;
            int idAux;

            //filtro por partida
            if (!string.IsNullOrEmpty(txtUbiNroPartida.Text))
            {
                if (rbtnUbiPartidaMatriz.Checked)
                {
                    int.TryParse(txtUbiNroPartida.Text, out idAux);
                    this.nro_partida_matriz = idAux;
                }
                else if (rbtnUbiPartidaHoriz.Checked)
                {
                    int.TryParse(txtUbiNroPartida.Text, out idAux);
                    this.nro_partida_horiz = idAux;
                }
                else
                {
                    throw new Exception("Debe indicar si número ingresado corresponde a partida matriz o a partida horizontal.");
                }
            }

            //filtro por domicilio
            if (!string.IsNullOrEmpty(txtUbiNroPuerta.Text) && ((String.IsNullOrEmpty(Request.Cookies["SubdividirUbicacion_IdCalle"].Value)) ? "" : Request.Cookies["SubdividirUbicacion_IdCalle"].Value) == "")
            {
                throw new Exception("Cuando especifica el número de puerta debe ingresar la calle.");
            }
            if (Request.Cookies["SubdividirUbicacion_IdCalle"] != null)
            {
                this.cod_calle = Request.Cookies["SubdividirUbicacion_IdCalle"].Value;
            }

            int.TryParse(txtUbiNroPuerta.Text.Trim(), out idAux);
            this.nro_puerta = idAux;

            //filtro por smp
            int.TryParse(txtUbiSeccion.Text, out idAux);
            this.seccion = idAux;

            this.manzana = txtUbiManzana.Text.Trim();

            this.parcela = txtUbiParcela.Text.Trim();

            //filtro por tipo subtipo
            int.TryParse(ddlUbiTipoUbicacion.SelectedItem.Value, out idAux);
            this.id_tipo_ubicacion = idAux > 0 ? idAux : -1;

            int.TryParse(ddlUbiSubTipoUbicacion.SelectedItem.Value, out idAux);
            this.id_sub_tipo_ubicacion = idAux > 0 ? idAux : -1;
        }

        #endregion

        #region Busqueda de Ubicacion

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Validar_BuscarPorUbicacion();
                filtrar = true;
                grdResultados.DataBind();
                updPnlResultadoBuscar.Update();

                pnlResultadoBuscar.Visible = true;

                EjecutarScript(updPnlResultadoBuscar, "showResultado();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex, "error al buscar Partidas buscar_Partidas-btnBuscar_OnClick");
                Enviar_Mensaje(ex.Message, "");
            }
        }

        //Grilla resultados
        protected void grdResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdResultados.PageIndex = e.NewPageIndex;
                filtrar = true;
                Validar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;

            try
            {
                grdResultados.PageIndex = int.Parse(cmdPage.Text) - 1;
                filtrar = true;
                Validar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                grdResultados.PageIndex = grdResultados.PageIndex - 1;
                filtrar = true;
                Validar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                grdResultados.PageIndex = grdResultados.PageIndex + 1;
                filtrar = true;
                Validar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }

        protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int id_ubicacion = 0;

                LinkButton btnAgregarUbicacion = (LinkButton)e.Row.FindControl("btnAgregarUbicacion");
                btnAgregarUbicacion.Visible = visualizar;

                Color colorFila = Color.White;
                Color colorBorde = Color.White;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int.TryParse(DataBinder.Eval(e.Row.DataItem, "id_ubicacion").ToString(), out id_ubicacion);
                }
            }
        }

        protected void grdResultados_DataBound(object sender, EventArgs e)
        {
            try
            {
                GridView grid = (GridView)grdResultados;
                GridViewRow fila = (GridViewRow)grid.BottomPagerRow;

                if (fila != null)
                {
                    LinkButton btnAnterior = (LinkButton)fila.Cells[0].FindControl("cmdAnterior");
                    LinkButton btnSiguiente = (LinkButton)fila.Cells[0].FindControl("cmdSiguiente");

                    if (grid.PageIndex == 0)
                        btnAnterior.Visible = false;
                    else
                        btnAnterior.Visible = true;

                    if (grid.PageIndex == grid.PageCount - 1)
                        btnSiguiente.Visible = false;
                    else
                        btnSiguiente.Visible = true;

                    // Ocultar todos los botones con Números de Página
                    for (int i = 1; i <= 19; i++)
                    {
                        LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                        btn.Visible = false;
                    }

                    if (grid.PageIndex == 0 || grid.PageCount <= 10)
                    {
                        // Mostrar 10 botones o el máximo de páginas

                        for (int i = 1; i <= 10; i++)
                        {
                            if (i <= grid.PageCount)
                            {
                                LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + i.ToString());
                                btn.Text = i.ToString();
                                btn.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        // Mostrar 9 botones hacia la izquierda y 9 hacia la derecha
                        // o bien los que sea posible en caso de no llegar a 9

                        int CantBucles = 0;

                        LinkButton btnPage10 = (LinkButton)fila.Cells[0].FindControl("cmdPage10");
                        btnPage10.Visible = true;
                        btnPage10.Text = Convert.ToString(grid.PageIndex + 1);

                        // Ubica los 9 botones hacia la izquierda
                        for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                        {
                            CantBucles++;
                            if (i >= 0)
                            {
                                LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 - CantBucles));
                                btn.Visible = true;
                                btn.Text = Convert.ToString(i + 1);
                            }

                        }

                        CantBucles = 0;
                        // Ubica los 9 botones hacia la derecha
                        for (int i = grid.PageIndex + 1; i <= grid.PageIndex + 9; i++)
                        {
                            CantBucles++;
                            if (i <= grid.PageCount - 1)
                            {
                                LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdPage" + Convert.ToString(10 + CantBucles));
                                btn.Visible = true;
                                btn.Text = Convert.ToString(i + 1);
                            }
                        }
                    }
                    LinkButton cmdPage;
                    string btnPage = "";
                    for (int i = 1; i <= 19; i++)
                    {
                        btnPage = "cmdPage" + i.ToString();
                        cmdPage = (LinkButton)fila.Cells[0].FindControl(btnPage);
                        if (cmdPage != null)
                            cmdPage.CssClass = "btn";
                    }

                    // busca el boton por el texto para marcarlo como seleccionado
                    string btnText = Convert.ToString(grid.PageIndex + 1);
                    foreach (Control ctl in fila.Cells[0].FindControl("pnlpager").Controls)
                    {
                        if (ctl is LinkButton)
                        {
                            LinkButton btn = (LinkButton)ctl;
                            if (btn.Text.Equals(btnText))
                            {
                                btn.CssClass = "btn btn-inverse";
                            }
                        }
                    }

                    UpdatePanel updPnlPager = (UpdatePanel)fila.Cells[0].FindControl("updPnlPager");
                    if (updPnlPager != null)
                        updPnlPager.Update();
                }
            }
            catch (Exception ex)
            {
                string aa = ex.Message;
            }
        }

        private void Validar()
        {
            Validar_BuscarPorUbicacion();
        }



        protected void grdCalleSub_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //permisos
                LinkButton btnEliminarCalleSub = (LinkButton)e.Row.FindControl("btnEliminarCalleSub");
                btnEliminarCalleSub.Visible = this.editable;
            }
        }

        private void guardarFiltro()
        {

        }

        protected void btnAgregarUbicacion_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;

                string id_ubi = btn.CommandArgument.ToString();

                hid_id_ubi_padre.Value = id_ubi;
                Session["id_ubi_padre"] = hid_id_ubi_padre.Value;

                // Verifico si existe una operación de subdivisión en proceso para la ubicación
                int id_op = getIdOperacionSubdivEnProceso(int.Parse(id_ubi));

                // Registramos la nueva operación si no existe
                if (id_op == 0)
                    id_op = GenerarOperacionSubDivEnProceso();
                else
                {
                    var ids = getIdsUbisTemp(id_op);
                    if (ids != null && ids.Count() > 0)
                    {
                        hid_ids_ubi_sub.Value = "";
                        foreach (var i in ids)
                            hid_ids_ubi_sub.Value += i.ToString() + ",";

                        Session["ids_ubi_sub"] = hid_ids_ubi_sub.Value;

                        grdParcelasSubdiv.DataBind();
                    }
                }

                if (id_op > 0)
                {
                    Session["id_operacion"] = id_op;
                    // Registramos la ubicacion
                    int id_op_det = GenerarOpDetalle(id_op, int.Parse(id_ubi));
                    if (id_op_det == 0)
                        throw new Exception("Error al realizar la operación. Intente nevamente.");
                }
                else
                    throw new Exception("Error al realizar la operación. Intente nevamente.");

                pnlParcelasSubdiv.Visible = hid_id_ubi_padre.Value != "";
                grdParcelaASubdiv.DataBind();

                ScriptManager.RegisterStartupScript(updPnlResultadoBuscar, updPnlResultadoBuscar.GetType(), "hidefrmAgregarUbicacion", "hidefrmAgregarUbicacion();", true);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updGridParcelaASubdiv, "showfrmError();");
            }
        }

        #endregion

        #region Controles

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "finalizarCarga", "finalizarCarga();", true);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        protected void btnCargarDatosSub_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "finalizarCarga", "finalizarCarga();", true);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int id_ope = (int)Session["id_operacion"];
                if (id_ope == 0)
                    throw new Exception("No existe operación registrada.");

                // verificamos que hay mas de una parcela para subdividir
                List<string> ids_ubis_tmp = quitarIdsRepetidos(hid_ids_ubi_sub.Value.Split(','));
                if (ids_ubis_tmp.Count() < 2)
                    throw new Exception("Debe agregar al menos 2 parcelas como subdivisiones.");


                // No permitir la creación de unificaciones/divisiones si la sección y manzana son diferentes
                if (!validarSeccionManzana(id_ope))
                    throw new Exception("No fue posible realizar la operación de subdivisión . La sección y/o manzana son diferentes");

                List<int> ids_nuevos = new List<int>();

                // Generamos todas las ubicaciones nuevas 
                foreach (var id_tmp in ids_ubis_tmp)
                {
                    int id_t = 0;
                    int.TryParse(id_tmp, out id_t);

                    // Verificamos que la ubicacion no se encuentre registrada
                    int id_nvo = ExisteUbicacionOp(id_t, id_ope);

                    // Generamos la nueva ubicacion desde temp
                    if (id_t != 0 && id_nvo == 0)
                        id_nvo = GuardarUbicacionFromTmp(id_t);

                    // Guardamos el id y acutalizamos 
                    if (id_nvo != 0)
                    {
                        ids_nuevos.Add(id_nvo);
                        if (!ActualizarOperacionIdUbicacion(id_ope, id_t, id_nvo))
                            throw new Exception("Se realizó la unificación de las parcelas pero se detectaron errores. Intente nuevamente.");
                    }
                }

                // Verificamos que la cantidad de registros insertados sea igual a los temp
                if (ids_nuevos.Count() != ids_ubis_tmp.Count())
                    throw new Exception("No se completó la operación. Intente nuevamente.");


                // Damos de baja la ubicacion padre
                bool baja = BajaUbicacion(int.Parse(hid_id_ubi_padre.Value));
                if (!baja)
                    throw new Exception("No fue posible realizar la baja de la parcela subdividida. Intente nuevamente.");

                if (!ConfirmarOperacion(id_ope))
                    throw new Exception("Error al confirmar la operación. Intente nuevamente.");
                else
                    Session["id_operacion"] = 0; // Liberamos la operación

                Response.Redirect("~/ABM/Ubicaciones/AbmUbicaciones.aspx", false);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updBotonesGuardar, "showfrmError();");
            }
        }

        private bool validarSeccionManzana(int id_ope)
        {
            using (var ctx = new DGHP_Entities())
            {
                //para poder unificar o dividir se debe validar que estos datos sean iguales
                var ubi = ctx.Ubicaciones_Operaciones_Detalle.Where(o => o.id_operacion == id_ope && o.id_ubicacion != null).Select(o => o.id_ubicacion);
                var primerSM = ctx.Ubicaciones.Where(u => ubi.Contains(u.id_ubicacion)).Select(u => new { u.Seccion, u.Manzana }).FirstOrDefault();
                var lstUbicaciones = ctx.Ubicaciones.Where(u => ubi.Contains(u.id_ubicacion)).Select(u => u);

                return lstUbicaciones.All(x => x.Seccion == primerSM.Seccion && x.Manzana == primerSM.Manzana);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // Anula operación generada
            int id_op = (int)Session["id_operacion"];

            //if (id_op != 0 && !AnularOperacion(id_op))
            if (!AnularOperacion(id_op))
                throw new Exception("Error al anular la operación. Intente nuevamente.");
            else
                Session["id_operacion"] = 0; // Liberamos la operación

            Response.Redirect("~/ABM/Ubicaciones/AbmUbicaciones.aspx");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            // Anula operación generada
            int id_op = (int)Session["id_operacion"];
            if (AnularOperacion(id_op))
            {
                // Limpio la id de la parcela a subdividir
                hid_id_ubi_padre.Value = "";
                Session["id_ubi_padre"] = hid_id_ubi_padre.Value;

                Session["id_operacion"] = 0;

                pnlParcelasSubdiv.Visible = false;

                grdParcelaASubdiv.DataBind();
            }
            else
            {
                lblError.Text = "Error al registrar la operación. Intente nuevamente.";
                this.EjecutarScript(updGridParcelaASubdiv, "showfrmError();");
            }
        }

        protected void btnAgregarParASub_Click(object sender, EventArgs e)
        {
            try
            {
                if (hid_id_ubi_padre.Value != "")
                    throw new Exception("Debe eliminar la parcela elegida.");

                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_ubi_dom, updPnlFiltroBuscar_ubi_dom.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updPnlResultadoBuscar, updPnlResultadoBuscar.GetType(), "init_Js_updPnlResultadoBuscar", "init_Js_updPnlResultadoBuscar();", true);
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_ubi_especial, updPnlFiltroBuscar_ubi_especial.GetType(), "init_Js_updPnlFiltroBuscar_ubi_especial", "init_Js_updPnlFiltroBuscar_ubi_especial();", true);
                ScriptManager.RegisterStartupScript(updPnlFiltroBuscar_ubi_dom, updPnlFiltroBuscar_ubi_dom.GetType(), "init_Js_updUbiAgregarUbicacion", "init_Js_updUbiAgregarUbicacion();", true);

                LimpiarAgregarParASub();

                CargarComboCalles();

                ScriptManager.RegisterStartupScript(updGridParcelaASubdiv, updGridParcelaASubdiv.GetType(), "showfrmAgregarUbicacion", "showfrmAgregarUbicacion();", true);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updGridParcelaASubdiv, "showfrmError();");
            }
        }

        protected void btnAgregarParsSub_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(updCargarUbicacion, updCargarUbicacion.GetType(), "init_Js_updDatos", "init_Js_updDatos();", true);
                ScriptManager.RegisterStartupScript(updCargarUbicacion, updCargarUbicacion.GetType(), "init_Js_updAgregarCalleSub", "init_Js_updAgregarCalleSub();", true);

                LimpiarCargarParsSub();
                // Precargamos los datos de la ubicacion padre
                if (hid_id_ubi_padre.Value != "")
                {
                    CargarDatosPadre(int.Parse(hid_id_ubi_padre.Value));
                    var ubicacionFraccionar = hid_id_ubi_padre.Value;
                    CargarCallesyPuertasDeUbicacionFraccionar(ubicacionFraccionar);
                }


                ScriptManager.RegisterStartupScript(updGridParcelasSubdiv, updGridParcelasSubdiv.GetType(), "showfrmCargarUbicacion", "showfrmCargarUbicacion();", true);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updGridParcelasSubdiv, "showfrmError();");
            }
        }

        private void CargarCallesyPuertasDeUbicacionFraccionar(string ubicacionFraccionar)
        {
            using (var db = new DGHP_Entities())
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("calles", typeof(string));
                dt.Columns.Add("nroPuerta", typeof(string));
                dt.Columns.Add("codigo_calle", typeof(int));

                int IdUbicacion;
                int.TryParse(ubicacionFraccionar, out IdUbicacion);

                var ubicacionesPuertas = (from ubi in db.Ubicaciones_Puertas
                                          where ubi.id_ubicacion == IdUbicacion
                                          select ubi).ToList();

                foreach (var puerta in ubicacionesPuertas)
                {
                    DataRow datarw;
                    datarw = dt.NewRow();

                    var nombreCalle = db.Calles.Where(x => x.Codigo_calle == puerta.codigo_calle).Select(y => y.NombreOficial_calle).First();

                    datarw[0] = HttpUtility.HtmlDecode(nombreCalle);
                    if (Shared.esUbicacionEspecialConObjetoTerritorial(IdUbicacion))
                    {
                        datarw[1] = HttpUtility.HtmlDecode(puerta.NroPuerta_ubic.ToString() + 't');
                    }
                    else
                    {
                        datarw[1] = HttpUtility.HtmlDecode(puerta.NroPuerta_ubic.ToString());
                    }
                    datarw[2] = HttpUtility.HtmlDecode(puerta.codigo_calle.ToString());
                    dt.Rows.Add(datarw);
                }

                grdCalleSub.DataSource = dt;
                grdCalleSub.DataBind();
            }
        }

        protected void ddlUbiTipoUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id_tipoubicacion = int.Parse(ddlUbiTipoUbicacion.SelectedValue);
                CargarCombo_subTipoUbicacion(id_tipoubicacion, true);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
            }

            updPnlFiltroBuscar_ubi_especial.Update();
        }

        #endregion

        #region Agregar Subdivision

        private DataTable dtUbicacionesCargadas()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("calles", typeof(string));
            dt.Columns.Add("nroPuerta", typeof(int));
            dt.Columns.Add("codigo_calle", typeof(int));

            foreach (GridViewRow row in grdCalleSub.Rows)
            {
                DataRow datarw;
                datarw = dt.NewRow();
                datarw[0] = HttpUtility.HtmlDecode(row.Cells[0].Text);
                datarw[1] = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[1].Text));
                datarw[2] = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[2].Text));
                dt.Rows.Add(datarw);
            }
            return dt;
        }

        protected void btnCancelCalleSub_Click(object sender, EventArgs e)
        {
            this.EjecutarScript(updCalleSub, "hidefrmAgregarCalleSub();");
            this.EjecutarScript(updCalleSub, "showfrmCargarUbicacion();");
        }

        protected void btnGuardarCalleSub_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dtUbicacionesCargadas();
                int nroPuertaUbi = 0;
                int codCalle = 0;
                int.TryParse(txtNroPuertaSub.Text.Trim(), out nroPuertaUbi);
                int.TryParse(Request.Cookies["SubdividirUbicacionSub_IdCalle"].Value, out codCalle);
                string txtCalle = GetNombreCalle(codCalle, nroPuertaUbi);
                if (txtCalle != "" && !ubicacionRepetida(txtCalle, nroPuertaUbi))
                {
                    DataRow datarw;
                    datarw = dt.NewRow();
                    datarw[0] = txtCalle;
                    datarw[1] = nroPuertaUbi;
                    datarw[2] = codCalle;
                    dt.Rows.Add(datarw);

                    grdCalleSub.DataSource = dt;
                    grdCalleSub.DataBind();

                    this.EjecutarScript(updCalleSub, "hidefrmAgregarCalleSub();");
                    this.EjecutarScript(updCalleSub, "showfrmCargarUbicacion();");
                }
                else
                {
                    txtNroPuertaSub.Text = string.Empty;
                    txtNroPuertaSub.Focus();
                }
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                this.EjecutarScript(updCalleSub, "hidefrmAgregarCalleSub();");
            }
        }

        protected void btnEliminarCalleSub_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEliminarCalleSub = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnEliminarCalleSub.Parent.Parent;
                DataTable dt = dtUbicacionesCargadas();
                dt.Rows.RemoveAt(row.RowIndex);
                grdCalleSub.DataSource = dt;
                grdCalleSub.DataBind();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        protected void ddlUbiTipoUbicacionABM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hid_tipo_reqSMP.Value = "0";
                int id_tipo = int.Parse(ddlUbiTipoUbicacionABM.SelectedValue);

                using (var db = new DGHP_Entities())
                {
                    var tipo = db.TiposDeUbicacion.Where(t => t.id_tipoubicacion == id_tipo).SingleOrDefault();
                    if (tipo != null && tipo.RequiereSMP != null)
                        hid_tipo_reqSMP.Value = (bool)tipo.RequiereSMP ? "1" : "0";
                }

                CargarCombo_subTipoUbicacionABM(id_tipo, false);
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
            }
        }

        protected void ddlGrupoDistritos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdGrupoDistrito = int.Parse(ddlGrupoDistritos.SelectedValue);
                if (IdGrupoDistrito != 0)
                {
                    CargarCombo_Distritos(IdGrupoDistrito, true);
                    ddlDistritosZonas.Enabled = false;
                    ddlDistritosSubZonas.Enabled = false;
                }
                else
                    ddlDistritos.Enabled = false;
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }

            updDatos.Update();
        }
        protected void ddlDistritosZonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdZona = int.Parse(ddlDistritosZonas.SelectedValue);
                if (IdZona != 0)
                {
                    CargarCombo_DistritosSubZonas(IdZona, true);
                }
                else
                    ddlDistritosSubZonas.Enabled = false;
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }

            updDatos.Update();
        }

        protected void ddlDistritos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IdDistrito = int.Parse(ddlDistritos.SelectedValue);
                if (IdDistrito != 0)
                {
                    CargarCombo_DistritosZonas(IdDistrito, true);
                    ddlDistritosSubZonas.Enabled = false;
                }
                else
                    ddlDistritosZonas.Enabled = false;
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }

            updDatos.Update();
        }

        protected void btnAgregarMixtura_Click(object sender, EventArgs e)
        {
            if (ddlMixtura.SelectedIndex != 0)
            {
                try
                {
                    DataTable dt = dtMixturasCargadas();

                    string mixDescripcion = (ddlMixtura.SelectedItem.Text).Trim();

                    string mix = (ddlMixtura.SelectedValue).Trim();

                    if (!MixRepetida(mix, mixDescripcion))
                    {
                        DataRow datarw;
                        datarw = dt.NewRow();

                        datarw[0] = mix;
                        datarw[1] = mixDescripcion;

                        dt.Rows.Add(datarw);

                        grdMixturas.DataSource = dt;
                        grdMixturas.DataBind();
                    }
                    else
                    {
                        ddlMixtura.Focus();
                    }
                }
                catch (Exception ex)
                {
                    LogError.Write(ex);
                    this.EjecutarScript(updCargarUbicacion, "hidefrmCargarUbicacion();");
                }
            }
        }

        private bool MixRepetida(string mix, string mixDescripcion)
        {
            DataTable dt = dtMixturasCargadas();

            var existe = (from r in dt.AsEnumerable()
                          where r.Field<String>("mix").Trim() == mix.Trim() &&
                                r.Field<String>("mixDescripcion").Trim() == mixDescripcion.Trim()
                          select r).ToList().Count();

            return existe != 0;
        }

        private DataTable dtMixturasCargadas()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("mix", typeof(string));
            dt.Columns.Add("mixDescripcion", typeof(string));

            foreach (GridViewRow row in grdMixturas.Rows)
            {
                DataRow datarw;
                datarw = dt.NewRow();
                datarw[0] = HttpUtility.HtmlDecode(row.Cells[0].Text);
                datarw[1] = HttpUtility.HtmlDecode(row.Cells[1].Text);
                dt.Rows.Add(datarw);
            }
            return dt;
        }

        protected void grdMixturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnEliminarMix = (LinkButton)e.Row.FindControl("btnEliminarMixtura");
                btnEliminarMix.Visible = this.editable;
            }
        }

        protected void btnEliminarMixtura_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEliminarMixtura = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnEliminarMixtura.Parent.Parent;
                DataTable dt = dtMixturasCargadas();
                dt.Rows.RemoveAt(row.RowIndex);
                grdMixturas.DataSource = dt;
                grdMixturas.DataBind();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        protected void grdDistritos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnEliminarDis = (LinkButton)e.Row.FindControl("btnEliminarDistrito");
                btnEliminarDis.Visible = this.editable;
            }
        }

        protected void btnAgregarDistrito_Click(object sender, EventArgs e)
        {
            if (ddlGrupoDistritos.SelectedIndex != 0)
            {
                try
                {
                    DataTable dt = dtDistritosCargados();

                    string grupoDistrito = ddlGrupoDistritos.SelectedItem.Text.Trim();
                    string distrito = ddlDistritos.SelectedIndex != -1 ? (ddlDistritos.SelectedIndex != 0 ? ddlDistritos.SelectedItem.Text.Trim() : string.Empty) : string.Empty;
                    string zonas = ddlDistritosZonas.SelectedIndex != -1 ? (ddlDistritosZonas.SelectedIndex != 0 ? ddlDistritosZonas.SelectedItem.Text.Trim() : string.Empty) : string.Empty;
                    string subZonas = ddlDistritosSubZonas.SelectedIndex != -1 ? (ddlDistritosSubZonas.SelectedIndex != 0 ? ddlDistritosSubZonas.SelectedItem.Text.Trim() : string.Empty) : string.Empty;

                    int IdDistrito = ddlDistritos.SelectedIndex != -1 && ddlDistritos.SelectedIndex != 0 ? int.Parse(ddlDistritos.SelectedItem.Value) : 0;

                    var IdZonas = 0;
                    var IdSubZonas = 0;

                    if (!String.IsNullOrEmpty(zonas))
                    {
                        IdZonas = ddlDistritosZonas.SelectedIndex;
                    }
                    if (!String.IsNullOrEmpty(subZonas))
                    {
                        IdSubZonas = ddlDistritosSubZonas.SelectedIndex;
                    }

                    if (!DistritoRepetido(grupoDistrito, distrito, zonas, subZonas))
                    {
                        DataRow datarw;
                        datarw = dt.NewRow();

                        datarw[0] = grupoDistrito;
                        datarw[1] = distrito;
                        datarw[2] = zonas;
                        datarw[3] = subZonas;
                        datarw[4] = IdDistrito;
                        datarw[5] = IdZonas;
                        datarw[6] = IdSubZonas;

                        dt.Rows.Add(datarw);

                        grdDistritos.DataSource = dt;
                        grdDistritos.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    LogError.Write(ex);
                    this.EjecutarScript(updCargarUbicacion, "hidefrmCargarUbicacion();");
                }
            }
        }

        private bool DistritoRepetido(string grupoDistrito, string distrito, string zonas, string subzonas)
        {
            DataTable dt = dtDistritosCargados();
            var existe = (from r in dt.AsEnumerable()
                          where r.Field<String>("grupoDistrito").Trim() == grupoDistrito.Trim() &&
                                r.Field<String>("distrito").Trim() == distrito.Trim() &&
                                r.Field<String>("zonas").Trim() == zonas.Trim() &&
                                r.Field<String>("subzonas").Trim() == subzonas.Trim()
                          select r).ToList().Count();
            return existe != 0;
        }

        private DataTable dtDistritosCargados()
        {
            DataTable dtDistritos = new DataTable();

            dtDistritos.Columns.Add("grupoDistrito", typeof(string));
            dtDistritos.Columns.Add("distrito", typeof(string));
            dtDistritos.Columns.Add("zonas", typeof(string));
            dtDistritos.Columns.Add("subzonas", typeof(string));
            dtDistritos.Columns.Add("IdDistrito", typeof(int));
            dtDistritos.Columns.Add("IdZona", typeof(int));
            dtDistritos.Columns.Add("IdSubZona", typeof(int));

            foreach (GridViewRow row in grdDistritos.Rows)
            {
                DataRow datarw;
                datarw = dtDistritos.NewRow();
                datarw[0] = HttpUtility.HtmlDecode(row.Cells[0].Text);
                datarw[1] = HttpUtility.HtmlDecode(row.Cells[1].Text);
                datarw[2] = HttpUtility.HtmlDecode(row.Cells[2].Text);
                datarw[3] = HttpUtility.HtmlDecode(row.Cells[3].Text);
                datarw[4] = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[4].Text));
                datarw[5] = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[5].Text));
                datarw[6] = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[6].Text));
                dtDistritos.Rows.Add(datarw);
            }
            return dtDistritos;
        }

        protected void btnEliminarDistrito_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEliminarDistrito = (LinkButton)sender;
                GridViewRow row = (GridViewRow)btnEliminarDistrito.Parent.Parent;
                DataTable dt = dtDistritosCargados();
                dt.Rows.RemoveAt(row.RowIndex);
                grdDistritos.DataSource = dt;
                grdDistritos.DataBind();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
            }
        }

        protected void btnCancelarSub_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarCargarParsSub();

                this.EjecutarScript(updDatos, "hidefrmCargarUbicacion();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updGridParcelasSubdiv, "showfrmError();");
            }
        }

        // Obtiene el id de la ubicacion temp cuya operacion relacionada se encuentra en proceso
        private int GetIdTempPendienteNroPartida(int nroPartidaMatriz)
        {
            using (var ctx = new DGHP_Entities())
            {
                var u = (from ut in ctx.Ubicaciones_temp
                         join opd in ctx.Ubicaciones_Operaciones_Detalle on ut.id_ubicacion_temp equals opd.id_ubicacion_temp
                         join op in ctx.Ubicaciones_Operaciones on opd.id_operacion equals op.id_operacion
                         where ut.NroPartidaMatriz == nroPartidaMatriz
                         && ut.baja_logica == false && op.id_estado == 0
                         select ut).SingleOrDefault();

                return u != null ? u.id_ubicacion_temp : 0;
            }
        }

        protected void btnGuardarSub_Click(object sender, EventArgs e)
        {
            bool result = true;
            int idNuevo = 0;

            bool Baja_Logica = false;

            //Subtipo Ubi
            int UbiSubTipo = 0;
            int.TryParse(ddlUbiSubTipoUbicacionABM.SelectedValue, out UbiSubTipo);

            //Nro Partida Matriz
            int UbiNroPartida = 0;
            int.TryParse(txtNroPartida.Text.Trim(), out UbiNroPartida);
            if (UbiNroPartida > 0)
            {
                try
                {
                    int existe = GetIdTempPendienteNroPartida(UbiNroPartida);
                    if (existe > 0)
                    {
                        txtNroPartida.Text = "";
                        throw new Exception("Ya existe una Ubicacion en proceso con el mismo Nro de PartidaMatriz");
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = Functions.GetErrorMessage(ex);
                    this.EjecutarScript(updBotonesGuardarCalleSub, "showfrmError();");
                    ScriptManager.RegisterStartupScript(updBotonesGuardarSub, updBotonesGuardarSub.GetType(), "hidefrmCargarUbicacion", "hidefrmCargarUbicacion();", true);
                    return;
                }
            }

            //Seccion
            int? UbiSeccionValue = null;
            int UbiSeccion = 0;
            int.TryParse(txtSeccion.Text.Trim(), out UbiSeccion);
            if (UbiSeccion > 0)
                UbiSeccionValue = UbiSeccion;

            // Barrio
            int barrio = 0;
            int.TryParse(ddlBarrio.SelectedValue, out barrio);

            // Comisaria
            int comisaria = 0;
            int.TryParse(ddlComisaria.SelectedValue, out comisaria);

            // Comuna
            int comuna = 0;
            int.TryParse(ddlComuna.SelectedValue, out comuna);

            // Obtenemos el nuevo id temp
            idNuevo = NuevoId_ubicacion_temp();

            using (var context = new DGHP_Entities())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        #region Ubicacion
                        var entity = new SGI.Model.Ubicaciones_temp()
                        {
                            id_ubicacion_temp = idNuevo,
                            id_subtipoubicacion = UbiSubTipo,
                            baja_logica = Baja_Logica,
                            CantiActualizacionesUSIG = 0,
                            cant_ph = null,
                            Circunscripcion = null,
                            id_comisaria = comisaria,
                            id_areahospitalaria = 0,
                            id_barrio = barrio,
                            id_comuna = comuna,
                            id_distritoescolar = 0,
                            id_regionsanitaria = 0,
                            id_zonaplaneamiento = 0,
                            CreateDate = DateTime.Now,
                            VigenciaDesde = DateTime.Now,
                            CreateUser = Functions.GetUserId(),
                            UpdateDate = DateTime.Now,
                            UpdateUser = Functions.GetUserId(),
                            EsEntidadGubernamental = this.chbEntidadGubernamental.Checked,
                            EsUbicacionProtegida = this.chbEdificioProtegido.Checked,
                            NroPartidaMatriz = UbiNroPartida,
                            Seccion = UbiSeccionValue,
                            Manzana = this.txtManzana.Text.Trim(),
                            Parcela = this.txtParcela.Text.Trim()
                        };

                        #endregion
                        // Agregamos la Ubicacion temporal
                        context.Ubicaciones_temp.Add(entity);

                        // Agregamos las Puertas temporales
                        List<Ubicaciones_Puertas_temp> listaPuertas = CrearListaPuertasTemp(entity.id_ubicacion_temp, grdCalleSub);
                        entity.Ubicaciones_Puertas_temp = listaPuertas;

                        // Agregamos las Mixturas temporales
                        List<Ubicaciones_ZonasMixtura> listaMix = CrearListaMixturas_temp(entity.id_ubicacion_temp, grdMixturas);
                        foreach (var item in listaMix)
                        {
                            Ubicaciones_ZonasMixtura a = context.Ubicaciones_ZonasMixtura.Find(item.IdZonaMixtura);
                            entity.Ubicaciones_ZonasMixtura.Add(a);
                        }

                        // Agregamos los Distritos temporales
                        List<Ubicaciones_Distritos_temp> listaDistritos = CrearListaDistritosTemp(entity.id_ubicacion_temp, grdDistritos);
                        entity.Ubicaciones_Distritos_temp = listaDistritos;

                        // Guardamos todos los cambios
                        context.SaveChanges();
                        dbContextTransaction.Commit();

                        // Guardamos el id temporal para el guardado final
                        hid_ids_ubi_sub.Value += idNuevo.ToString() + ",";
                        Session["ids_ubi_sub"] += hid_ids_ubi_sub.Value;

                        LimpiarCargarParsSub();
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        dbContextTransaction.Rollback();
                        lblError.Text = Functions.GetErrorMessage(ex);
                        this.EjecutarScript(updBotonesGuardarCalleSub, "showfrmError();");
                        ScriptManager.RegisterStartupScript(updBotonesGuardarSub, updBotonesGuardarSub.GetType(), "hidefrmCargarUbicacion", "hidefrmCargarUbicacion();", true);
                    }
                }
            }

            // Actualizo la grilla de las subdivisiones
            if (result)
            {
                int id_op = (int)Session["id_operacion"];

                int id_op_det = GenerarOpDetalle(id_op, 0, idNuevo, "");
                if (id_op_det == 0)
                {
                    lblError.Text = "Ocurrió un error al registrar la operación. Intente nuevamente.";
                    this.EjecutarScript(updBotonesGuardarCalleSub, "showfrmError();");
                }

                grdParcelasSubdiv.DataBind();

                ScriptManager.RegisterStartupScript(updBotonesGuardarSub, updBotonesGuardarSub.GetType(), "hidefrmCargarUbicacion", "hidefrmCargarUbicacion();", true);
            }
        }

        protected void btnEliminarSub_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string id_ubi_temp = btn.CommandArgument.ToString();

            // Marcamos como dada de baja a la ubicación temporal 
            if (BajaUbiTemp(int.Parse(id_ubi_temp)))
            {
                // Borramos el id temporal de la colección de parcelas
                if (hid_ids_ubi_sub.Value.Contains(id_ubi_temp + ","))
                    hid_ids_ubi_sub.Value = hid_ids_ubi_sub.Value.Replace(id_ubi_temp + ",", "");

                Session["ids_ubi_sub"] = hid_ids_ubi_sub.Value;
            }
            grdParcelasSubdiv.DataBind();
        }

        private bool BajaUbiTemp(int id_ubi_temp)
        {
            using (var ctx = new DGHP_Entities())
            {
                var ubi_tmp = ctx.Ubicaciones_temp.Where(u => u.id_ubicacion_temp == id_ubi_temp).SingleOrDefault();
                if (ubi_tmp != null && !ubi_tmp.baja_logica)
                {
                    ubi_tmp.baja_logica = true;
                    return ctx.SaveChanges() != 0;
                }
            }

            return false;
        }

        protected void btnEditarSub_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            string id_ubi_temp = btn.CommandArgument.ToString();

            Response.Redirect("~/ABM/Ubicaciones/EditarUbicacionTemp.aspx?id=" + id_ubi_temp);
        }

        protected void btnAgregarCalleSub_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(updAgregarCalleSub, updAgregarCalleSub.GetType(), "init_Js_updAgregarCalleSub", "init_Js_updAgregarCalleSub();", true);

                txtNroPuertaSub.Text = string.Empty;

                this.EjecutarScript(updCargarUbicacion, "showfrmAgregarCalleSub();");

                this.EjecutarScript(updCargarUbicacion, "hidefrmCargarUbicacion();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updGridParcelaASubdiv, "showfrmError();");
            }
        }

        #endregion

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarAgregarParASub();
            CargarComboCalles();
        }
        protected void AutocompleteCalles_ValueSelect(object sender, Syncfusion.JavaScript.Web.AutocompleteSelectEventArgs e)
        {
            Response.Cookies["SubdividirUbicacion_IdCalle"].Value = e.Key;
            return;
        }

        protected void AutocompleteCallesSub_ValueSelect(object sender, Syncfusion.JavaScript.Web.AutocompleteSelectEventArgs e)
        {
            Response.Cookies["SubdividirUbicacionSub_IdCalle"].Value = e.Key;
            return;
        }
    }
}