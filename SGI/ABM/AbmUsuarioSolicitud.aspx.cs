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
using System.IO;
using SGI.WebServices;
using System.Globalization;

namespace SGI.ABM
{
    public partial class AbmUsuarioSolicitud : BasePage
    {
        DGHP_Entities db = null;

        #region load de pagina
        protected void Page_Load(object sender, EventArgs e)
        {


            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updPnlUsuSSIT, updPnlUsuSSIT.GetType(), "init_Js_updPnlUsuSSIT", "init_Js_updPnlUsuSSIT();", true);
            }


            if (!IsPostBack)
            {
                db = new DGHP_Entities();
                CargarTiposDeDocumento();
                CargarTipoTramite();
            }
            cargarPermisos();

            //pnlEditarSolicitud.Visible = editar;

        }
        #endregion

        #region permisos

        private bool editar;
        private void cargarPermisos()
        {
            db = new DGHP_Entities();
            Guid userid = Functions.GetUserId();

            var perfiles_usuario = db.aspnet_Users.FirstOrDefault(x => x.UserId == userid).SGI_PerfilesUsuarios.Select(x => x.nombre_perfil).ToList();

            foreach (var perfil in perfiles_usuario)
            {
                var menu_usuario = db.SGI_Perfiles.FirstOrDefault(x => x.nombre_perfil == perfil).SGI_Menues.Select(x => x.descripcion_menu).ToList();

                if (menu_usuario.Contains("Reasignar Solicitud"))
                {
                    editar = true;
                }
            }
        }

        #endregion
        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }


        private void CargarTiposDeDocumento()
        {
            ddlBusTipoDocumento.DataSource = db.Solicitud_TraerTiposDocumentosPersonales();
            ddlBusTipoDocumento.DataTextField = "nombre";
            ddlBusTipoDocumento.DataValueField = "id";
            ddlBusTipoDocumento.DataBind();

            ddlBusTipoDocumento.Items.Insert(0, new ListItem("CUIT", "8"));

            ddlBusTipoDocumento.Items.Insert(0, new ListItem("Todos", "99"));


        }
        private void CargarTipoTramite()
        {

            var tramites = (from tra in db.TipoTramite
                            where tra.id_tipotramite == 1 || tra.id_tipotramite == 2 || tra.id_tipotramite == 5 || tra.id_tipotramite == 6
                            select tra).OrderBy(x => x.id_tipotramite);

            ddlBusTipoTramite.DataSource = tramites.ToList();
            ddlBusTipoTramite.DataTextField = "descripcion_tipotramite";
            ddlBusTipoTramite.DataValueField = "id_tipotramite";
            ddlBusTipoTramite.DataBind();

            ddlBusTipoTramite.Items.Insert(0, new ListItem("Todos", "0"));


        }


        private void LimpiarDatosBusqueda()
        {
            txtBusNroSolicitud.Text = "";
            txtBusSolicitante.Text = "";
            txtBusTitular.Text = "";
            txtBusNroDocumento.Text = "";
            txtBusNroAnexo.Text = string.Empty;
            ddlBusTipoTramite.ClearSelection();
            ddlBusTipoDocumento.ClearSelection();
        }
        private void LimpiarNuevoUsuario()
        {
            txtResNroSolicitud.Text = "";
            txtResUsername.Text = "";
            txtResApeNom.Text = "";

            txtNuevoUsuarioUser.Text = "";
            txtNuevoUsuarioEmail.Text = "";
        }
        private void LimpiarSubirArchivo()
        {
            hid_file_id.Value = "0";
            hid_filename.Value = "";
            hid_filename_random.Value = "";
            hid_filesize.Value = "";
            lnkUsuSSIT.Text = "";
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Buscar();
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
        private void Buscar()
        {
            db = new DGHP_Entities();
            IQueryable<clsItemUpdateSolicitud> qENC = null;
            IQueryable<clsItemUpdateSolicitud> qSSIT = null;
            IQueryable<clsItemUpdateSolicitud> qCPadron = null;
            IQueryable<clsItemUpdateSolicitud> qTransf = null;

            IQueryable<clsItemUpdateSolicitud> qENC_TIT = null;
            IQueryable<clsItemUpdateSolicitud> qSSIT_TIT = null;
            IQueryable<clsItemUpdateSolicitud> qCPadron_TIT = null;
            IQueryable<clsItemUpdateSolicitud> qTransf_TIT = null;

            IQueryable<clsItemUpdateSolicitud> qFinal = null;
            IQueryable<clsItemUpdateSolicitud> qFinal_TIT = null;
            int BusTipoDoc = 99;
            int BusTipoTramite = 0;
            int BusNroSolicitud = 0;
            int BusNroEncomienda = 0;

            int.TryParse(txtBusNroSolicitud.Text.Trim(), out BusNroSolicitud);
            int.TryParse(txtBusNroAnexo.Text.Trim(), out BusNroEncomienda);
            int.TryParse(ddlBusTipoDocumento.SelectedValue, out BusTipoDoc);
            int.TryParse(ddlBusTipoTramite.SelectedValue, out BusTipoTramite);

            if (BusNroEncomienda > 0 && (BusTipoTramite == (int)Constants.TipoDeTramite.Consulta || BusTipoTramite == (int)Constants.TipoDeTramite.Habilitacion || BusTipoTramite == (int)Constants.TipoDeTramite.RectificatoriaHabilitacion))
            {
                //qENC = (from enc in db.Encomienda.Where(x => x.id_encomienda == BusNroEncomienda)
                //        join prof in db.Profesional on enc.CreateUser equals prof.UserId into profdf
                //        from prof in profdf.DefaultIfEmpty()
                //        join usuarios in db.Usuario on enc.CreateUser equals usuarios.UserId into usudf
                //        from usuarios in usudf.DefaultIfEmpty()
                //        join mem in db.aspnet_Membership on enc.CreateUser equals mem.UserId
                //        join aspuser in db.aspnet_Users on enc.CreateUser equals aspuser.UserId
                //        select new clsItemUpdateSolicitud
                //        {
                //            id_solicitud = enc.id_solicitud ?? 0,
                //            IdEncomienda = enc.id_encomienda,
                //            FechaCreacion = enc.CreateDate,
                //            IdTipoTramite = enc.id_tipotramite,
                //            Solicitante = usuarios != null ? usuarios.Apellido + ", " + usuarios.Nombre : prof.Apellido+ ", "+prof.Nombre,
                //            SolicitanteUsername = string.IsNullOrEmpty(usuarios.UserName) ? aspuser.UserName : usuarios.UserName,
                //            SolicitanteGuid = enc.CreateUser,
                //            TitularNombre = "",
                //            TitularDocumento = "",
                //            TitularTipoDoc = 99,
                //            TitularEmail = "",
                //            TipoTramite = "Anexo Tecnico",
                //        });

                ////Filtros Gestor
                //if (txtBusNroSolicitud.Text.Trim().Length > 0)
                //{
                //    qENC = qENC.Where(x => x.id_solicitud == BusNroSolicitud);
                //}
                //if (txtBusSolicitante.Text.Trim().Length > 0)
                //    qENC = qENC.Where(x => x.Solicitante.ToLower().Contains(txtBusSolicitante.Text.Trim().ToLower()));

                //if (BusTipoTramite > 0)
                //    qENC = qENC.Where(x => x.IdTipoTramite == BusTipoTramite);

                //if (qENC != null)
                //{
                //    qENC_TIT = (from ssit in db.Encomienda.Where(x => qENC.Any(y => y.IdEncomienda == x.id_encomienda))

                //                 join titpj in db.Encomienda_Titulares_PersonasJuridicas on ssit.id_encomienda equals titpj.id_encomienda into titpjdf
                //                 from titpj in titpjdf.DefaultIfEmpty()

                //                 join titpf in db.Encomienda_Titulares_PersonasFisicas on ssit.id_encomienda equals titpf.id_encomienda into titpfdf
                //                 from titpf in titpfdf.DefaultIfEmpty()

                //                 select new clsItemUpdateSolicitud
                //                 {
                //                     id_solicitud = ssit.id_solicitud ?? 0,
                //                     IdEncomienda = ssit.id_encomienda,
                //                     FechaCreacion = ssit.CreateDate,
                //                     IdTipoTramite = ssit.id_tipotramite,
                //                     Solicitante = "",
                //                     SolicitanteUsername = "",
                //                     SolicitanteGuid = ssit.CreateUser,
                //                     TitularNombre = titpj.Razon_Social ?? (titpf.Apellido + ", " + titpf.Nombres),
                //                     TitularDocumento = titpf.Nro_Documento != null ? (titpf.TipoDocumentoPersonal.Nombre + " " + titpf.Nro_Documento) : ("CUIT " + titpj.CUIT),
                //                     TitularTipoDoc = titpf.Nro_Documento.Length > 1 ? (titpf.TipoDocumentoPersonal.TipoDocumentoPersonalId) : 8,
                //                     TitularEmail = titpf.Email ?? titpj.Email,
                //                     TipoTramite = "Anexo Tecnico",
                //                 }).Distinct();


                //    if (txtBusTitular.Text.Trim().Length > 0)
                //    {
                //        qENC_TIT = qENC_TIT.Where(x => x.TitularNombre.ToLower().Contains(txtBusTitular.Text.Trim().ToLower()));
                //    }
                //    if (txtBusNroDocumento.Text.Trim().Length > 0)
                //        qENC_TIT = qENC_TIT.Where(x => x.TitularDocumento.Contains(txtBusNroDocumento.Text.Trim()));

                //    if (ddlBusTipoDocumento.SelectedValue != "99" || ddlBusTipoDocumento.SelectedItem.Text != "Todos")
                //    {
                //        qENC_TIT = qENC_TIT.Where(x => x.TitularTipoDoc == BusTipoDoc);
                //    } 
                //}
                //SSIT
                qSSIT = (from enc in db.Encomienda.Where(x => x.id_encomienda == BusNroEncomienda)//solicitudes in db.SSIT_Solicitudes.Where(x => qENC.Any(y => y.id_solicitud == x.id_solicitud))
                         join encsol in db.Encomienda_SSIT_Solicitudes on enc.id_encomienda equals encsol.id_encomienda
                         join solicitudes in db.SSIT_Solicitudes on encsol.id_solicitud equals solicitudes.id_solicitud
                         join usuarios in db.Usuario on solicitudes.CreateUser equals usuarios.UserId
                         join mem in db.aspnet_Membership on solicitudes.CreateUser equals mem.UserId
                         join aspuser in db.aspnet_Users on solicitudes.CreateUser equals aspuser.UserId
                         select new clsItemUpdateSolicitud
                         {
                             id_solicitud = solicitudes.id_solicitud,
                             IdEncomienda = 0,
                             FechaCreacion = solicitudes.CreateDate,
                             IdTipoTramite = solicitudes.id_tipotramite,
                             Solicitante = usuarios.Apellido + ", " + usuarios.Nombre,
                             SolicitanteUsername = string.IsNullOrEmpty(usuarios.UserName) ? aspuser.UserName : usuarios.UserName,
                             SolicitanteGuid = solicitudes.CreateUser,
                             TitularNombre = "",
                             TitularDocumento = "",
                             TitularTipoDoc = 99,
                             TitularEmail = "",
                             TipoTramite = solicitudes.TipoTramite.descripcion_tipotramite,
                         });

                //Filtros Gestor
                if (BusNroSolicitud > 0)
                {
                    qSSIT = qSSIT.Where(x => x.id_solicitud == BusNroSolicitud);
                }
                if (txtBusSolicitante.Text.Trim().Length > 0)
                    qSSIT = qSSIT.Where(x => x.Solicitante.ToLower().Contains(txtBusSolicitante.Text.Trim().ToLower()));

                if (BusTipoTramite > 0)
                    qSSIT = qSSIT.Where(x => x.IdTipoTramite == BusTipoTramite);

                if (qSSIT != null)
                {
                    qSSIT_TIT = (from res in qSSIT//ssit in db.SSIT_Solicitudes.Where(x => qSSIT.Any(y => y.id_solicitud == x.id_solicitud))
                                 join ssit in db.SSIT_Solicitudes on res.id_solicitud equals ssit.id_solicitud
                                 join titpj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas on ssit.id_solicitud equals titpj.id_solicitud into titpjdf
                                 from titpj in titpjdf.DefaultIfEmpty()

                                 join titpf in db.SSIT_Solicitudes_Titulares_PersonasFisicas on ssit.id_solicitud equals titpf.id_solicitud into titpfdf
                                 from titpf in titpfdf.DefaultIfEmpty()

                                 select new clsItemUpdateSolicitud
                                 {
                                     id_solicitud = ssit.id_solicitud,
                                     IdEncomienda = 0,
                                     FechaCreacion = ssit.CreateDate,
                                     IdTipoTramite = ssit.id_tipotramite,
                                     Solicitante = "",
                                     SolicitanteUsername = "",
                                     SolicitanteGuid = ssit.CreateUser,
                                     TitularNombre = titpj.Razon_Social ?? (titpf.Apellido + ", " + titpf.Nombres),
                                     TitularDocumento = titpf.Nro_Documento != null ? (titpf.TipoDocumentoPersonal.Nombre + " " + titpf.Nro_Documento) : ("CUIT " + titpj.CUIT),
                                     TitularTipoDoc = titpf.Nro_Documento.Length > 1 ? (titpf.TipoDocumentoPersonal.TipoDocumentoPersonalId) : 8,
                                     TitularEmail = titpf.Email ?? titpj.Email,
                                     TipoTramite = ssit.TipoTramite.descripcion_tipotramite,
                                 }).Distinct();


                    if (txtBusTitular.Text.Trim().Length > 0)
                    {
                        qSSIT_TIT = qSSIT_TIT.Where(x => x.TitularNombre.ToLower().Contains(txtBusTitular.Text.Trim().ToLower()));
                    }
                    if (txtBusNroDocumento.Text.Trim().Length > 0)
                        qSSIT_TIT = qSSIT_TIT.Where(x => x.TitularDocumento.Contains(txtBusNroDocumento.Text.Trim()));

                    if (ddlBusTipoDocumento.SelectedValue != "99" || ddlBusTipoDocumento.SelectedItem.Text != "Todos")
                    {
                        qSSIT_TIT = qSSIT_TIT.Where(x => x.TitularTipoDoc == BusTipoDoc);
                    }
                }
            }
            else
            {

                if (BusTipoTramite == (int)Constants.TipoDeTramite.Consulta || BusTipoTramite == (int)Constants.TipoDeTramite.Habilitacion || BusTipoTramite == (int)Constants.TipoDeTramite.RectificatoriaHabilitacion)
                {
                    qSSIT = (from solicitudes in db.SSIT_Solicitudes
                             join usuarios in db.Usuario on solicitudes.CreateUser equals usuarios.UserId
                             join mem in db.aspnet_Membership on solicitudes.CreateUser equals mem.UserId
                             join aspuser in db.aspnet_Users on solicitudes.CreateUser equals aspuser.UserId
                             select new clsItemUpdateSolicitud
                             {
                                 id_solicitud = solicitudes.id_solicitud,
                                 IdEncomienda = 0,
                                 FechaCreacion = solicitudes.CreateDate,
                                 IdTipoTramite = solicitudes.id_tipotramite,
                                 Solicitante = usuarios.Apellido + ", " + usuarios.Nombre,
                                 SolicitanteUsername = string.IsNullOrEmpty(usuarios.UserName) ? aspuser.UserName : usuarios.UserName,
                                 SolicitanteGuid = solicitudes.CreateUser,
                                 TitularNombre = "",
                                 TitularDocumento = "",
                                 TitularTipoDoc = 99,
                                 TitularEmail = "",
                                 TipoTramite = solicitudes.TipoTramite.descripcion_tipotramite,
                             });

                    //Filtros Gestor
                    if (txtBusNroSolicitud.Text.Trim().Length > 0)
                    {
                        qSSIT = qSSIT.Where(x => x.id_solicitud == BusNroSolicitud);
                    }
                    if (txtBusSolicitante.Text.Trim().Length > 0)
                        qSSIT = qSSIT.Where(x => x.Solicitante.ToLower().Contains(txtBusSolicitante.Text.Trim().ToLower()));

                    if (BusTipoTramite > 0)
                        qSSIT = qSSIT.Where(x => x.IdTipoTramite == BusTipoTramite);

                    if (qSSIT != null)
                    {
                        qSSIT_TIT = (from res in qSSIT//ssit in db.SSIT_Solicitudes.Where(x => qSSIT.Any(y => y.id_solicitud == x.id_solicitud))
                                     join ssit in db.SSIT_Solicitudes on res.id_solicitud equals ssit.id_solicitud
                                     join titpj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas on ssit.id_solicitud equals titpj.id_solicitud into titpjdf
                                     from titpj in titpjdf.DefaultIfEmpty()

                                     join titpf in db.SSIT_Solicitudes_Titulares_PersonasFisicas on ssit.id_solicitud equals titpf.id_solicitud into titpfdf
                                     from titpf in titpfdf.DefaultIfEmpty()

                                     select new clsItemUpdateSolicitud
                                     {
                                         id_solicitud = ssit.id_solicitud,
                                         IdEncomienda = 0,
                                         FechaCreacion = ssit.CreateDate,
                                         IdTipoTramite = ssit.id_tipotramite,
                                         Solicitante = "",
                                         SolicitanteUsername = "",
                                         SolicitanteGuid = ssit.CreateUser,
                                         TitularNombre = titpj.Razon_Social ?? (titpf.Apellido + ", " + titpf.Nombres),
                                         TitularDocumento = titpf.Nro_Documento != null ? (titpf.TipoDocumentoPersonal.Nombre + " " + titpf.Nro_Documento) : ("CUIT " + titpj.CUIT),
                                         TitularTipoDoc = titpf.Nro_Documento.Length > 1 ? (titpf.TipoDocumentoPersonal.TipoDocumentoPersonalId) : 8,
                                         TitularEmail = titpf.Email ?? titpj.Email,
                                         TipoTramite = ssit.TipoTramite.descripcion_tipotramite,
                                     }).Distinct();


                        if (txtBusTitular.Text.Trim().Length > 0)
                        {
                            qSSIT_TIT = qSSIT_TIT.Where(x => x.TitularNombre.ToLower().Contains(txtBusTitular.Text.Trim().ToLower()));
                        }
                        if (txtBusNroDocumento.Text.Trim().Length > 0)
                            qSSIT_TIT = qSSIT_TIT.Where(x => x.TitularDocumento.Contains(txtBusNroDocumento.Text.Trim()));

                        if (ddlBusTipoDocumento.SelectedValue != "99" || ddlBusTipoDocumento.SelectedItem.Text != "Todos")
                        {
                            qSSIT_TIT = qSSIT_TIT.Where(x => x.TitularTipoDoc == BusTipoDoc);
                        }
                    }

                }
                if (BusTipoTramite == (int)Constants.TipoDeTramite.Consulta || BusTipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                {
                    qCPadron = (from cp in db.CPadron_Solicitudes
                                join usu in db.Usuario on cp.CreateUser equals usu.UserId
                                join mem in db.aspnet_Membership on cp.CreateUser equals mem.UserId
                                join usr in db.aspnet_Users on cp.CreateUser equals usr.UserId
                                select new clsItemUpdateSolicitud
                                {
                                    id_solicitud = cp.id_cpadron,
                                    IdEncomienda = 0,
                                    FechaCreacion = cp.CreateDate,
                                    IdTipoTramite = cp.id_tipotramite,
                                    Solicitante = usu.Apellido + ", " + usu.Nombre,
                                    SolicitanteUsername = string.IsNullOrEmpty(usu.UserName) ? usr.UserName : usu.UserName,
                                    SolicitanteGuid = cp.CreateUser,
                                    TitularNombre = "",
                                    TitularDocumento = "",
                                    TitularTipoDoc = 99,
                                    TitularEmail = "",
                                    TipoTramite = cp.TipoTramite.descripcion_tipotramite,
                                });

                    //Filtros Gestor
                    if (txtBusNroSolicitud.Text.Trim().Length > 0)
                    {
                        qCPadron = qCPadron.Where(x => x.id_solicitud == BusNroSolicitud);
                    }
                    if (txtBusSolicitante.Text.Trim().Length > 0)
                        qCPadron = qCPadron.Where(x => x.Solicitante.ToLower().Contains(txtBusSolicitante.Text.Trim().ToLower()));


                    if (qCPadron != null)
                    {
                        qCPadron_TIT = (from ssit in db.CPadron_Solicitudes.Where(x => qCPadron.Any(y => y.id_solicitud == x.id_cpadron))

                                        join titpj in db.CPadron_Titulares_Solicitud_PersonasJuridicas on ssit.id_cpadron equals titpj.id_cpadron into titpjdf
                                        from titpj in titpjdf.DefaultIfEmpty()

                                        join titpf in db.CPadron_Titulares_Solicitud_PersonasFisicas on ssit.id_cpadron equals titpf.id_cpadron into titpfdf
                                        from titpf in titpfdf.DefaultIfEmpty()

                                        select new clsItemUpdateSolicitud
                                        {
                                            id_solicitud = ssit.id_cpadron,
                                            IdEncomienda = 0,
                                            FechaCreacion = ssit.CreateDate,
                                            IdTipoTramite = ssit.id_tipotramite,
                                            Solicitante = "",
                                            SolicitanteUsername = "",
                                            SolicitanteGuid = ssit.CreateUser,
                                            TitularNombre = titpj.Razon_Social ?? (titpf.Apellido + ", " + titpf.Nombres),
                                            TitularDocumento = titpf.Nro_Documento != null ? (titpf.TipoDocumentoPersonal.Nombre + " " + titpf.Nro_Documento) : ("CUIT " + titpj.CUIT),
                                            TitularTipoDoc = titpf.Nro_Documento.Length > 1 ? (titpf.TipoDocumentoPersonal.TipoDocumentoPersonalId) : 8,
                                            TitularEmail = titpf.Email ?? titpj.Email,
                                            TipoTramite = ssit.TipoTramite.descripcion_tipotramite,
                                        }).Distinct();


                        if (txtBusTitular.Text.Trim().Length > 0)
                        {
                            qCPadron_TIT = qCPadron_TIT.Where(x => x.TitularNombre.ToLower().Contains(txtBusTitular.Text.Trim().ToLower()));
                        }
                        if (txtBusNroDocumento.Text.Trim().Length > 0)
                            qCPadron_TIT = qCPadron_TIT.Where(x => x.TitularDocumento.Contains(txtBusNroDocumento.Text.Trim()));

                        if (ddlBusTipoDocumento.SelectedValue != "99" || ddlBusTipoDocumento.SelectedItem.Text != "Todos")
                        {
                            qCPadron_TIT = qCPadron_TIT.Where(x => x.TitularTipoDoc == BusTipoDoc);
                        }
                    }
                }

                if (BusTipoTramite == (int)Constants.TipoDeTramite.Consulta || BusTipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                {
                    qTransf = (from transf in db.Transf_Solicitudes
                               join usu in db.Usuario on transf.CreateUser equals usu.UserId
                               join mem in db.aspnet_Membership on transf.CreateUser equals mem.UserId
                               join usr in db.aspnet_Users on transf.CreateUser equals usr.UserId
                               select new clsItemUpdateSolicitud
                               {
                                   id_solicitud = transf.id_solicitud,
                                   IdEncomienda = 0,
                                   FechaCreacion = transf.CreateDate,
                                   IdTipoTramite = transf.id_tipotramite,
                                   Solicitante = usu.Apellido + ", " + usu.Nombre,
                                   SolicitanteUsername = string.IsNullOrEmpty(usu.UserName) ? usr.UserName : usu.UserName,
                                   SolicitanteGuid = transf.CreateUser,
                                   TitularNombre = "",
                                   TitularDocumento = "",
                                   TitularTipoDoc = 99,
                                   TitularEmail = "",
                                   TipoTramite = transf.TipoTramite.descripcion_tipotramite,
                               });

                    //Filtros Gestor
                    if (txtBusNroSolicitud.Text.Trim().Length > 0)
                    {
                        qTransf = qTransf.Where(x => x.id_solicitud == BusNroSolicitud);
                    }
                    if (txtBusSolicitante.Text.Trim().Length > 0)
                        qTransf = qTransf.Where(x => x.Solicitante.ToLower().Contains(txtBusSolicitante.Text.Trim().ToLower()));


                    if (qTransf != null)
                    {
                        qTransf_TIT = (from ssit in db.Transf_Solicitudes.Where(x => qTransf.Any(y => y.id_solicitud == x.id_solicitud))

                                       join titpj in db.Transf_Titulares_PersonasJuridicas on ssit.id_solicitud equals titpj.id_solicitud into titpjdf
                                       from titpj in titpjdf.DefaultIfEmpty()

                                       join titpf in db.Transf_Titulares_PersonasFisicas on ssit.id_solicitud equals titpf.id_solicitud into titpfdf
                                       from titpf in titpfdf.DefaultIfEmpty()

                                       select new clsItemUpdateSolicitud
                                       {
                                           id_solicitud = ssit.id_solicitud,
                                           IdEncomienda = 0,
                                           FechaCreacion = ssit.CreateDate,
                                           IdTipoTramite = ssit.id_tipotramite,
                                           Solicitante = "",
                                           SolicitanteUsername = "",
                                           SolicitanteGuid = ssit.CreateUser,
                                           TitularNombre = titpj.Razon_Social ?? (titpf.Apellido + ", " + titpf.Nombres),
                                           TitularDocumento = titpf.Nro_Documento != null ? (titpf.TipoDocumentoPersonal.Nombre + " " + titpf.Nro_Documento) : ("CUIT " + titpj.CUIT),
                                           TitularTipoDoc = titpf.Nro_Documento.Length > 1 ? (titpf.TipoDocumentoPersonal.TipoDocumentoPersonalId) : 8,
                                           TitularEmail = titpf.Email ?? titpj.Email,
                                           TipoTramite = ssit.TipoTramite.descripcion_tipotramite,
                                       }).Distinct();


                        if (txtBusTitular.Text.Trim().Length > 0)
                        {
                            qTransf_TIT = qTransf_TIT.Where(x => x.TitularNombre.ToLower().Contains(txtBusTitular.Text.Trim().ToLower()));
                        }
                        if (txtBusNroDocumento.Text.Trim().Length > 0)
                            qTransf_TIT = qTransf_TIT.Where(x => x.TitularDocumento.Contains(txtBusNroDocumento.Text.Trim()));

                        if (ddlBusTipoDocumento.SelectedValue != "99" || ddlBusTipoDocumento.SelectedItem.Text != "Todos")
                        {
                            qTransf_TIT = qTransf_TIT.Where(x => x.TitularTipoDoc == BusTipoDoc);
                        }
                    }
                }

            }
            if (BusNroEncomienda > 0)
            {

                //if (qENC != null)
                //    qFinal = (qFinal == null ? qENC : qFinal.Union(qENC));

                if (qSSIT != null)
                    qFinal = (qFinal == null ? qSSIT : qFinal.Union(qSSIT));

                //if (qENC_TIT != null)
                //    qFinal_TIT = (qFinal_TIT == null ? qENC_TIT : qFinal_TIT.Union(qENC_TIT));

                if (qSSIT_TIT != null)
                    qFinal_TIT = (qFinal_TIT == null ? qSSIT_TIT : qFinal_TIT.Union(qSSIT_TIT));

            }
            else
            {

                if (qSSIT != null)
                    qFinal = (qFinal == null ? qSSIT : qFinal.Union(qSSIT));

                if (qCPadron != null)
                    qFinal = (qFinal == null ? qCPadron : qFinal.Union(qCPadron));

                if (qTransf != null)
                    qFinal = (qFinal == null ? qTransf : qFinal.Union(qTransf));


                if (qSSIT_TIT != null)
                    qFinal_TIT = (qFinal_TIT == null ? qSSIT_TIT : qFinal_TIT.Union(qSSIT_TIT));

                if (qCPadron_TIT != null)
                    qFinal_TIT = (qFinal_TIT == null ? qCPadron_TIT : qFinal_TIT.Union(qCPadron_TIT));

                if (qTransf_TIT != null)
                    qFinal_TIT = (qFinal_TIT == null ? qTransf_TIT : qFinal_TIT.Union(qTransf_TIT));


                if (qFinal_TIT != null)
                    qFinal = qFinal.Where(x => qFinal_TIT.Any(y => y.id_solicitud == x.id_solicitud && y.TipoTramite == x.TipoTramite));

            }

            int cant = 0;
            int cantitulares = 0;

            if (qFinal != null)
            {
                cant = qFinal.Count();
                grdResultados.DataSource = qFinal.OrderByDescending(x => x.FechaCreacion).ToList();
                grdResultados.DataBind();
            }
            else
            {
                grdResultados.DataSource = null;
                grdResultados.DataBind();
            }

            if (qFinal_TIT != null)
            {
                cantitulares = qFinal_TIT.Count();

                grdTitulares.DataSource = qFinal_TIT.OrderByDescending(x => x.FechaCreacion).ToList();
                grdTitulares.DataBind();

            }
            else
            {
                grdTitulares.DataSource = null;
                grdTitulares.DataBind();
            }
            pnlCantidadTitulares.Visible = (cantitulares > 0);
            lblCantidadTitulares.Text = cantitulares.ToString();

            pnlCantidadRegistros.Visible = (cant > 0);
            lblCantidadRegistros.Text = cant.ToString();

            db.Dispose();
        }

        //private void Buscar()
        //{
        //    db = new DGHP_Entities();
        //    IQueryable<clsItemUpdateSolicitud> q = null;
        //    IQueryable<clsItemUpdateSolicitud> p = null;
        //    IQueryable<clsItemUpdateSolicitud> qFinal = null;

        //    int BusTipoDoc = 99;
        //    int BusTipoTramite = 99;
        //    int BusNroSolicitud = 0;
        //    int BusNroEncomienda = 0;

        //    q = (from solicitudes in db.SSIT_Solicitudes
        //         join usuarios in db.Usuario on solicitudes.CreateUser equals usuarios.UserId
        //         join mem in db.aspnet_Membership on solicitudes.CreateUser equals mem.UserId
        //         join aspuser in db.aspnet_Users on solicitudes.CreateUser equals aspuser.UserId

        //         select new clsItemUpdateSolicitud
        //         {
        //             id_solicitud = solicitudes.id_solicitud,
        //             Solicitante = usuarios.Apellido + ", " + usuarios.Nombre,
        //             SolicitanteUsername = string.IsNullOrEmpty(usuarios.UserName) ? aspuser.UserName : usuarios.UserName,
        //             SolicitanteGuid = solicitudes.CreateUser,
        //             TitularNombre = "",
        //             TitularDocumento = "",
        //             TitularTipoDoc = 99,
        //             TitularEmail = "",
        //         });

        //    //Filtros Gestor
        //    if (txtBusNroSolicitud.Text.Trim().Length > 0)
        //    {
        //        int.TryParse(txtBusNroSolicitud.Text.Trim(),out BusNroSolicitud);
        //        q = q.Where(x => x.id_solicitud == BusNroSolicitud);
        //    }
        //    if (txtBusSolicitante.Text.Trim().Length > 0)
        //        q = q.Where(x => x.Solicitante.ToLower().Contains(txtBusSolicitante.Text.Trim().ToLower()));


        //    if (q != null)
        //    {
        //        p = (from ssit in db.SSIT_Solicitudes.Where(x => q.Any(y => y.id_solicitud == x.id_solicitud))

        //             join titpj in db.SSIT_Solicitudes_Titulares_PersonasJuridicas on ssit.id_solicitud equals titpj.id_solicitud into titpjdf
        //             from titpj in titpjdf.DefaultIfEmpty()

        //             join titpf in db.SSIT_Solicitudes_Titulares_PersonasFisicas on ssit.id_solicitud equals titpf.id_solicitud into titpfdf
        //             from titpf in titpfdf.DefaultIfEmpty()

        //             select new clsItemUpdateSolicitud
        //             {
        //                 id_solicitud = ssit.id_solicitud,
        //                 Solicitante = "",
        //                 SolicitanteUsername = "",
        //                 SolicitanteGuid = ssit.CreateUser,
        //                 TitularNombre = titpj.Razon_Social ?? (titpf.Apellido+", "+titpf.Nombres),
        //                 TitularDocumento = titpf.Nro_Documento != null ? (titpf.TipoDocumentoPersonal.Nombre+" "+ titpf.Nro_Documento) : ("CUIT " + titpj.CUIT),
        //                 TitularTipoDoc = titpf.Nro_Documento.Length > 1 ? (titpf.TipoDocumentoPersonal.TipoDocumentoPersonalId) : 8,
        //                 TitularEmail = titpf.Email ?? titpj.Email,
        //             }).Distinct();


        //        if (txtBusTitular.Text.Trim().Length > 0)
        //        {
        //            p = p.Where(x => x.TitularNombre.ToLower().Contains(txtBusTitular.Text.Trim().ToLower()));
        //        }
        //        if(txtBusNroDocumento.Text.Trim().Length > 0)
        //            p = p.Where(x => x.TitularDocumento.Contains(txtBusNroDocumento.Text.Trim()));

        //        if(ddlBusTipoDocumento.SelectedValue != "99" || ddlBusTipoDocumento.SelectedItem.Text != "Todos")
        //        {
        //            int.TryParse(ddlBusTipoDocumento.SelectedValue, out BusTipoDoc);
        //            p = p.Where(x => x.TitularTipoDoc == BusTipoDoc);
        //        }


        //    }
        //    if (p != null)
        //        qFinal = q.Where(x => p.Any(y => y.id_solicitud == x.id_solicitud));
        //    else
        //        qFinal = q;

        //    int cant = qFinal.Count();
        //    int cantitulares = p.Count();

        //    grdTitulares.DataSource = p.OrderBy(x => x.TitularNombre).ToList();
        //    grdTitulares.DataBind();

        //    pnlCantidadTitulares.Visible = (cantitulares > 0);
        //    lblCantidadTitulares.Text = cantitulares.ToString();

        //    grdResultados.DataSource = qFinal.OrderBy(x => x.id_solicitud).ToList();
        //    grdResultados.DataBind();

        //    pnlCantidadRegistros.Visible = (cant > 0);
        //    lblCantidadRegistros.Text = cant.ToString();
        //    db.Dispose();
        //}



        #region paginado grilla

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdResultados.PageIndex = e.NewPageIndex;
                grdTitulares.PageIndex = e.NewPageIndex;
                IniciarEntity();
                Buscar();
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
                grdTitulares.PageIndex = int.Parse(cmdPage.Text) - 1;
                IniciarEntity();
                Buscar();
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
                grdTitulares.PageIndex = grdTitulares.PageIndex - 1;
                IniciarEntity();
                Buscar();
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
                grdTitulares.PageIndex = grdTitulares.PageIndex + 1;
                IniciarEntity();
                Buscar();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
            }
        }
        protected void grdResultados_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    LinkButton btnEditar = (LinkButton)e.Row.FindControl("btnEditar");
            //    btnEditar.Visible = editar;

            //}


        }
        protected void grdtitular_DataBound(object sender, EventArgs e)
        {
            try
            {

                GridView grid = (GridView)grdTitulares;
                GridViewRow fila = (GridViewRow)grid.BottomPagerRow;

                if (fila != null)
                {
                    LinkButton btnAnterior = (LinkButton)fila.Cells[0].FindControl("cmdtitAnterior");
                    LinkButton btnSiguiente = (LinkButton)fila.Cells[0].FindControl("cmdtitSiguiente");

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
                        LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdtitPage" + i.ToString());
                        btn.Visible = false;
                    }


                    if (grid.PageIndex == 0 || grid.PageCount <= 10)
                    {
                        // Mostrar 10 botones o el máximo de páginas

                        for (int i = 1; i <= 10; i++)
                        {
                            if (i <= grid.PageCount)
                            {
                                LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdtitPage" + i.ToString());
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

                        LinkButton btnPage10 = (LinkButton)fila.Cells[0].FindControl("cmdtitPage10");
                        btnPage10.Visible = true;
                        btnPage10.Text = Convert.ToString(grid.PageIndex + 1);

                        // Ubica los 9 botones hacia la izquierda
                        for (int i = grid.PageIndex - 1; i >= grid.PageIndex - 9; i--)
                        {
                            CantBucles++;
                            if (i >= 0)
                            {
                                LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdtitPage" + Convert.ToString(10 - CantBucles));
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
                                LinkButton btn = (LinkButton)fila.Cells[0].FindControl("cmdtitPage" + Convert.ToString(10 + CantBucles));
                                btn.Visible = true;
                                btn.Text = Convert.ToString(i + 1);
                            }
                        }



                    }
                    LinkButton cmdPage;
                    string btnPage = "";
                    for (int i = 1; i <= 19; i++)
                    {
                        btnPage = "cmdtitPage" + i.ToString();
                        cmdPage = (LinkButton)fila.Cells[0].FindControl(btnPage);
                        if (cmdPage != null)
                            cmdPage.CssClass = "btn";

                    }


                    // busca el boton por el texto para marcarlo como seleccionado
                    string btnText = Convert.ToString(grid.PageIndex + 1);
                    foreach (Control ctl in fila.Cells[0].FindControl("pnlpagertitulares").Controls)
                    {
                        if (ctl is LinkButton)
                        {
                            LinkButton btn = (LinkButton)ctl;
                            if (btn.Text.Equals(btnText))
                            {
                                btn.CssClass = "btn btn-info";
                            }
                        }
                    }

                    UpdatePanel updPnlPager = (UpdatePanel)fila.Cells[0].FindControl("updpnlpagertitulares");
                    if (updPnlPager != null)
                        updPnlPager.Update();



                }

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }


        }
        protected void grd_DataBound(object sender, EventArgs e)
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
                                btn.CssClass = "btn btn-primary";
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


        private void Enviar_Mensaje(string mensaje, string titulo)
        {
            mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
            if (string.IsNullOrEmpty(titulo))
                titulo = System.Web.HttpUtility.HtmlEncode(this.Title);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                    "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        }
        #endregion

        #region entity

        private void IniciarEntity()
        {
            if (this.db == null)
                this.db = new DGHP_Entities();
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
        private void CargarSolicitud(int IdSolicitud, int IdTipoTramite, int IdEncomienda)
        {
            db = new DGHP_Entities();
            clsItemUpdateSolicitud qSSIT = null;
            //IQueryable<clsItemUpdateSolicitud> qCPadron = null;
            //IQueryable<clsItemUpdateSolicitud> qTransf = null;
            if (IdEncomienda > 0)
            {
                qSSIT = (from enc in db.Encomienda
                         join prof in db.Profesional on enc.CreateUser equals prof.UserId into profdf
                         from prof in profdf.DefaultIfEmpty()
                         join usu in db.Usuario on enc.CreateUser equals usu.UserId into usudf
                         from usu in usudf.DefaultIfEmpty()
                         join aspuser in db.aspnet_Users on enc.CreateUser equals aspuser.UserId
                         where enc.id_encomienda == IdEncomienda
                         select new clsItemUpdateSolicitud
                         {
                             id_solicitud = enc.Encomienda_SSIT_Solicitudes.Select(x => x.id_solicitud).FirstOrDefault(),
                             IdEncomienda = enc.id_encomienda,
                             Solicitante = usu != null ? usu.Apellido + ", " + usu.Nombre : prof.Apellido + ", " + prof.Nombre,
                             SolicitanteUsername = string.IsNullOrEmpty(usu.UserName) ? aspuser.UserName : usu.UserName,
                             TipoTramite = "Anexo Tecnico",
                         }).FirstOrDefault();
            }
            else
            {
                if (IdTipoTramite == (int)Constants.TipoDeTramite.Consulta_Padron)
                {
                    qSSIT = (from ssit in db.CPadron_Solicitudes
                             join usu in db.Usuario on ssit.CreateUser equals usu.UserId
                             join aspuser in db.aspnet_Users on ssit.CreateUser equals aspuser.UserId
                             where ssit.id_cpadron == IdSolicitud
                             select new clsItemUpdateSolicitud
                             {
                                 id_solicitud = ssit.id_cpadron,
                                 IdEncomienda = 0,
                                 Solicitante = usu.Nombre + " " + usu.Apellido,
                                 SolicitanteUsername = string.IsNullOrEmpty(usu.UserName) ? aspuser.UserName : usu.UserName,
                                 TipoTramite = ssit.TipoTramite.descripcion_tipotramite,
                             }).FirstOrDefault();
                }

                else if (IdTipoTramite == (int)Constants.TipoDeTramite.Transferencia)
                {
                    qSSIT = (from ssit in db.Transf_Solicitudes
                             join usu in db.Usuario on ssit.CreateUser equals usu.UserId
                             join aspuser in db.aspnet_Users on ssit.CreateUser equals aspuser.UserId
                             where ssit.id_solicitud == IdSolicitud
                             select new clsItemUpdateSolicitud
                             {
                                 id_solicitud = ssit.id_solicitud,
                                 IdEncomienda = 0,
                                 Solicitante = usu.Nombre + " " + usu.Apellido,
                                 SolicitanteUsername = string.IsNullOrEmpty(usu.UserName) ? aspuser.UserName : usu.UserName,
                                 TipoTramite = ssit.TipoTramite.descripcion_tipotramite,
                             }).FirstOrDefault();
                }
                else
                {
                    qSSIT = (from ssit in db.SSIT_Solicitudes
                             join usu in db.Usuario on ssit.CreateUser equals usu.UserId
                             join aspuser in db.aspnet_Users on ssit.CreateUser equals aspuser.UserId
                             where ssit.id_solicitud == IdSolicitud
                             select new clsItemUpdateSolicitud
                             {
                                 id_solicitud = ssit.id_solicitud,
                                 IdEncomienda = 0,
                                 Solicitante = usu.Nombre + " " + usu.Apellido,
                                 SolicitanteUsername = string.IsNullOrEmpty(usu.UserName) ? aspuser.UserName : usu.UserName,
                                 TipoTramite = ssit.TipoTramite.descripcion_tipotramite,
                             }).FirstOrDefault();

                }
            }

            if (qSSIT != null)
            {
                txtResNroSolicitud.Text = qSSIT.id_solicitud + "";
                txtResApeNom.Text = qSSIT.Solicitante;
                txtResUsername.Text = qSSIT.SolicitanteUsername;
                txtResTipoTramite.Text = qSSIT.TipoTramite;
                if (IdEncomienda > 0)
                {
                    pnlResNroAnexo.Visible = true;
                    txtResNroAnexo.Text = qSSIT.IdEncomienda + "";
                }
                else
                {
                    pnlResNroAnexo.Visible = false;
                }
            }
            db.Dispose();

        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEditar = (LinkButton)sender;

                string[] arg = new string[2];
                arg = btnEditar.CommandArgument.ToString().Split(';');

                int IdSolicitud = 0;
                int IdTipoTramite = 0;
                int IdEncomienda = 0;

                int.TryParse(arg[0], out IdSolicitud);
                int.TryParse(arg[2], out IdTipoTramite);
                int.TryParse(arg[3], out IdEncomienda);

                hid_id_solicitud.Value = btnEditar.CommandArgument.ToString();
                CargarSolicitud(IdSolicitud, IdTipoTramite, IdEncomienda);
                //constancia

                updPnlUsuSSIT.Update();

                updDatos.Update();
                this.EjecutarScript(updResultados, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updResultados, "showfrmError();");

            }

        }

        protected void btnTransferir_Click(object sender, EventArgs e)
        {

            try
            {
                LinkButton btnTransferir = (LinkButton)sender;

                //Guardo los valores de la solicitud: ID, usuario original
                string[] arg = new string[2];
                arg = hid_id_solicitud.Value.Split(';');

                int id_solicitud_origen = 0;
                int IdTipoTramite = 0;
                int IdEncomienda = 0;
                Guid Usuario_Anterior = Guid.Parse(arg[1]);

                int.TryParse(arg[0], out id_solicitud_origen);
                int.TryParse(arg[2], out IdTipoTramite);
                int.TryParse(arg[3], out IdEncomienda);

                //Guardo los valores del nuevo usuario
                string[] newuserarg = new string[4];
                newuserarg = btnTransferir.CommandArgument.ToString().Split(';');

                Guid Usuario_Nuevo = Guid.Parse(newuserarg[0]);

                string Usuario_IsAproved = (newuserarg[1]).ToString();
                bool Usuario_IsLockedOut = bool.Parse(newuserarg[2]);

                Guid Usuario_AplicacionID = Guid.Parse(newuserarg[3]);
                string username = (newuserarg[4]).ToString();//Deberia ser el cuit
                string nombre = (newuserarg[5]).ToString();
                string apellido = (newuserarg[6]).ToString();

                Guid Usuario_Editor = Functions.GetUserId();

                int id_file = 0;
                int.TryParse(hid_file_id.Value, out id_file);

                if (Usuario_IsLockedOut == true)
                {
                    throw new Exception("El usuario seleccionado se encuentra bloqueado.");
                }
                else
                {
                    db = new DGHP_Entities();

                    int tipotramite = 0;
                    int? idTAD = null;

                    // Buscamos la solicitud
                    var sol = db.SSIT_Solicitudes.Where(x => x.id_solicitud == id_solicitud_origen).FirstOrDefault();

                    var tran = db.Transf_Solicitudes.Where(x => x.id_solicitud == id_solicitud_origen).FirstOrDefault();

                    var cp = db.CPadron_Solicitudes.Where(x => x.id_cpadron == id_solicitud_origen).FirstOrDefault();

                    if (sol != null && sol.id_tipotramite == IdTipoTramite)
                    {
                        tipotramite = sol.id_tipotramite;
                        idTAD = sol.idTAD;
                    }
                    else if (tran != null && tran.id_tipotramite == IdTipoTramite)
                    {
                        tipotramite = tran.id_tipotramite;
                        idTAD = tran.idTAD;
                    }
                    else if (cp != null && cp.id_tipotramite == IdTipoTramite)
                    {
                        tipotramite = cp.id_tipotramite;
                        idTAD = cp.idTAD;
                    }

                    // Actualizamos el usuario
                    if (sol != null || tran != null || cp != null)
                    {
                        if (idTAD != null)
                        {

                            string _urlESB = Functions.GetParametroChar("Url.Service.ESB");
                            string trata = Functions.GetParametroChar("Trata.Habilitacion");
                            if (tipotramite == (int)Constants.TipoDeTramite.Ampliacion_Unificacion)
                                trata = Functions.GetParametroChar("Trata.Ampliacion");
                            else if (tipotramite == (int)Constants.TipoDeTramite.RedistribucionDeUso)
                                trata = Functions.GetParametroChar("Trata.RedistribucionDeUso");

                            var list = wsGP.perfilesPorTrata(_urlESB, trata);
                            int p_idPerfilOperador = 0;
                            int p_idPerfilSolicitante = 0;
                            foreach (var p in list)
                            {
                                if (p.nombrePerfil == "TITULAR")
                                    p_idPerfilOperador = p.idPerfil;
                                else if (p.nombrePerfil == "SOLICITANTE")
                                    p_idPerfilSolicitante = p.idPerfil;
                            }

                            clsOperador OperadorSGI = new clsOperador
                            {
                                idPerfil = 4, //OPERADOR
                                cuit = username,
                            };
                            var OperadorGP = wsGP.GetParticipantesxTramite(_urlESB, idTAD.Value)//sol.idTAD.Value)
                                                        .Where(x => x.idPerfil == 4) //OPERADOR
                                                        .Where(x => x.vigenciaParticipante == true).FirstOrDefault();

                            if (OperadorGP != null && OperadorSGI.cuit != OperadorGP.cuit)
                            {
                                wsGP.DesvincularParticipante(_urlESB, idTAD.Value, OperadorGP.cuit, OperadorGP.idPerfil, Constants.SISTEMA, OperadorGP.cuit, OperadorGP.idPerfil);

                                wsGP.vincularSolicitanteSE(_urlESB, idTAD.Value, OperadorGP.cuit, p_idPerfilOperador, OperadorSGI.cuit, p_idPerfilSolicitante,
                                nombre, apellido, Constants.SISTEMA);
                            }
                        }
                        //en nuestra sistema se actualiza siempre...                        
                        db.SSIT_Solicitudes_Actualizar_Usuario(id_solicitud_origen, Usuario_Anterior, Usuario_Nuevo, Usuario_Editor, id_file, IdTipoTramite, IdEncomienda, 0);
                    }

                    Buscar();
                    updResultados.Update();
                    LimpiarNuevoUsuario();
                    LimpiarSubirArchivo();
                    updDatos.Update();
                    updpnlNuevoUsuario.Update();
                    this.EjecutarScript(updpnlNuevoUsuario, "showBusqueda();");
                }

            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlNuevoUsuario, "showfrmError();");

            }

        }

        private byte[] cargarPDF(string arch)
        {
            FileStream fs = new FileStream(arch, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] pdfBytes = br.ReadBytes((Int32)fs.Length);
            br.Close();
            fs.Close();

            if (pdfBytes.Length == 0)
                throw new Exception("El documento está vacio.");


            if (pdfBytes.Length > 8000000)
                throw new Exception("El tamaño máximo permitido es de 8 MB");

            return pdfBytes;
        }

        private string _nombreArchivoFisico;
        public string NombreArchivoFisico
        {
            get
            {
                if (string.IsNullOrEmpty(_nombreArchivoFisico))
                    _nombreArchivoFisico = Constants.PathTemporal + this.RandomArchivo + hid_filename.Value;
                return _nombreArchivoFisico;
            }
            set
            {
                _nombreArchivoFisico = value;
            }
        }

        private string _randomArchivo;
        public string RandomArchivo
        {
            get
            {
                if (string.IsNullOrEmpty(_randomArchivo))
                    _randomArchivo = hid_filename_random.Value;
                return _randomArchivo;
            }
            set
            {
                _randomArchivo = value;
            }
        }

        private void EliminarDocumento(string arch)
        {
            try
            {
                System.IO.File.Delete(arch);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

        }

        protected void btnComenzarCargaArchivo_Click(object sender, EventArgs e)
        {

            try
            {
                Guid userid = Functions.GetUserId();

                //this.db = new DGHP_Entities();

                byte[] file_content = cargarPDF(this.NombreArchivoFisico);

                string file_name = hid_filename.Value;

                //Subir a files
                int id_file = 0;
                id_file = ws_FilesRest.subirArchivo(file_name, file_content);

                hid_file_id.Value = id_file.ToString();

                //this.db.Dispose();
                //cargarDescarArchivo();
                lnkUsuSSIT.Text = hid_filename.Value;

                updPnlUsuSSIT.Update();

                string mensaje = "El documento \"" + file_name + "\" se ha adjuntado correctamente.";
                mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);

            }
            catch (Exception ex)
            {
                if (this.db != null)
                    this.db.Dispose();

                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updPnlCargarArchivo, "showfrmError();");

            }

            EliminarDocumento(this.NombreArchivoFisico);//borra el archivo del disco

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarNuevoUsuario();
            updDatos.Update();
            updpnlNuevoUsuario.Update();
            this.EjecutarScript(updBotonesGuardar, "showBusqueda();");

        }

        protected void lnkNuevoUsuarioBuscar_Click(object sender, EventArgs e)
        {
            string User_nuevo = txtNuevoUsuarioUser.Text.Trim();
            string Email_nuevo = txtNuevoUsuarioEmail.Text.Trim();

            Guid appSGI = new Guid("A2EAEF96-F109-4B62-BC31-53E219C76362");

            try
            {

                db = new DGHP_Entities();
                var usuarios = (from usu in db.Usuario
                                join mem in db.aspnet_Membership on usu.UserId equals mem.UserId
                                join au in db.aspnet_Users on usu.UserId equals au.UserId
                                where mem.ApplicationId == appSGI &&
                                (usu.UserName == User_nuevo || (string.IsNullOrEmpty(usu.UserName) && au.UserName == User_nuevo) || usu.Email == Email_nuevo)
                                select new
                                {
                                    nombre_usuario = string.IsNullOrEmpty(usu.UserName) ? au.UserName : usu.UserName,
                                    nombre_apellido = usu.Nombre + " " + usu.Apellido,
                                    nombre = usu.Nombre,
                                    apellido = usu.Apellido,
                                    usuario_id = usu.UserId,
                                    usuario_email = usu.Email,
                                    usuario_dni = usu.UserDni,
                                    IsApproved = mem.IsApproved,
                                    IsLockedOut = mem.IsLockedOut,
                                    AplicacionID = mem.ApplicationId,

                                });

                grdNuevoUsuario.DataSource = usuarios.ToList();
                grdNuevoUsuario.DataBind();

                db.Dispose();

                updpnlNuevoUsuario.Update();
                this.EjecutarScript(updBotonesGuardar, "showUsuarios();");
            }
            catch (Exception ex)
            {

                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updBotonesGuardar, "showfrmError();");
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarDatosBusqueda();
            updpnlBuscar.Update();
            EjecutarScript(UpdatePanel1, "hideResultado();");
        }

        protected void BtnPDFSolicitante_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Reportes/ImprimirSolicitante.aspx?id=" + grdTitulares.Rows[0].Cells[0].Text);
        }
    }
}