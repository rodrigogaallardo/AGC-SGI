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

namespace SGI.ABM
{
    public partial class AbmProfesionales : BasePage
    {
        DGHP_Entities db = null;

        #region load de pagina
        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);

            if (sm.IsInAsyncPostBack)
            {
                ScriptManager.RegisterStartupScript(updpnlBuscar, updpnlBuscar.GetType(), "init_Js_updpnlBuscar", "init_Js_updpnlBuscar();", true);
                ScriptManager.RegisterStartupScript(updDatos, updDatos.GetType(), "init_Js_updpnlCrearActu", "init_Js_updpnlCrearActu();", true);

            }
            
            if (!IsPostBack)
            {
                db = new DGHP_Entities();
                CargarPerfiles();
                CargarConsejos();
            }
        }
        #endregion

        private void CargarConsejos()
        {
            Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
            var lst = (from consejo in db.GrupoConsejos
                       where consejo.nombre_grupoconsejo != "SE"
                               select new
                               {
                                   consejo.id_grupoconsejo,
                                   consejo.nombre_grupoconsejo
                               }).Distinct().OrderBy(x => x.nombre_grupoconsejo).ToList();

            ddlConsejos.DataSource = lst;
            ddlConsejos.DataTextField = "nombre_grupoconsejo";
            ddlConsejos.DataValueField = "id_grupoconsejo";
            ddlConsejos.DataBind();
            ddlConsejos.Items.Insert(0, "");
        }

        private void CargarPerfiles()
        {
            Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
            var lstPerfiles = (from cp in db.ConsejoProfesional_RolesPermitidos 
                               join roles in db.aspnet_Roles on cp.RoleID equals roles.RoleId
                             select new
                             {
                                 roles.RoleName,
                                 roles.RoleId
                             }).Distinct().OrderBy(x => x.RoleName).ToList();

            ddlPerfiles.DataSource = lstPerfiles;
            ddlPerfiles.DataTextField = "RoleName";
            ddlPerfiles.DataValueField = "RoleId";
            ddlPerfiles.DataBind();
            ddlPerfiles.Items.Insert(0, "Todos");
        }
        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            EjecutarScript(updResultados, "LimpiarBusqueda();");
            txtNroMatricula.Text = string.Empty;
            txtCuit.Text = "";
            txtApellidoyNombre.Text = "";
            ddlConsejos.ClearSelection();
            ddlPerfiles.ClearSelection();
            ddlSubperfiles.ClearSelection();
            txtUsuario.Text = "";
            txtEmail.Text = "";
            ddlDadoBaja.ClearSelection();
            pnlResultadoBuscar.Visible = false;
            updpnlBuscar.Update();
            updResultados.Update();
        }

        private void CargarSubPerfiles(Guid? roleid)
        {
            db = new DGHP_Entities();
            var lstSubperfiles = new List<GrupoConsejos_Roles_Clasificacion>();
            if (roleid != null)
               lstSubperfiles= db.GrupoConsejos_Roles_Clasificacion.Where(x => x.RoleID == roleid).ToList();
            
            ddlSubperfiles.DataSource = lstSubperfiles;
            ddlSubperfiles.DataTextField = "descripcion_clasificacion";
            ddlSubperfiles.DataValueField = "id_clasificacion";
            ddlSubperfiles.DataBind();

            ddlSubperfiles.Items.Insert(0, "");

            updpnlBuscar.Update();

            db.Dispose();
        }

        protected void ddlPerfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlPerfiles.SelectedItem.Text != "Todos" && ddlPerfiles.SelectedItem.Value != "0")
                {
                    Guid id_role = Guid.Parse(ddlPerfiles.SelectedItem.Value);
                    CargarSubPerfiles(id_role);
                }
                else
                {
                    CargarSubPerfiles(null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error para sub-perfiles");
            }



        }

        protected void btnCargarDatos_Click(object sender, EventArgs e)
        {

            this.EjecutarScript(updpnlBuscar, "finalizarCarga();");
        }

        protected void btnNuevaCondicion_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarDatos();
                updDatos.Update();
                this.EjecutarScript(updpnlBuscar, "showDatos();");
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                lblError.Text = Functions.GetErrorMessage(ex);
                this.EjecutarScript(updpnlBuscar, "finalizarCarga();showfrmError();");

            }
        }

        private void LimpiarDatosBusqueda()
        {
            txtNroMatricula.Text = "";
            txtApellidoyNombre.Text = "";
            txtCuit.Text = "";
            txtUsuario.Text = "";
            txtEmail.Text = "";
        }

        private void LimpiarDatos()
        {
            hid_id_profesionalReq.Value = "0";
            txtConsejoReq.Text = "";
            txtNroMatriculaReq.Text = "";
            txtApellidoReq.Text = "";
            txtUsuario.Text = "";
            txtEmail.Text = "";
            txtNombreReq.Text = "";
            
        }

        private string BusMatricula = string.Empty;
        private string BusCuit = string.Empty;
        private string BusNomyApe = string.Empty;
        private int BusConsejo = 0;
        private string BusPerfil = string.Empty;
        private int BusSubperfil = 0;
        private string BusUsername = string.Empty;
        private string BusEmail = string.Empty;
        private string BusDadoBaja = string.Empty;
        private string BusInhibido = string.Empty;

        private bool hayFiltro = false;

        private void ValidarBuscar()
        {
            this.hayFiltro = false;

            int idAux = 0;

            if (txtApellidoyNombre.Text.Length > 0 && txtApellidoyNombre.Text.Length < 4)
                throw new Exception("Si ingresa el Apellido y Nombre, el mismo debe contener más de 3 caracteres.");

            Regex rx = new Regex(@"\b([0-9]{2}-[0-9]{8}-[0-9]{1})\b",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Find matches.
            MatchCollection matches = rx.Matches(txtCuit.Text);

            if (txtCuit.Text.Length > 0 && matches.Count == 0)
                throw new Exception("El Nº de CUIT no tiene un formato válido. Ej: 20-25006281-9.");

            //Busqueda / Filtrar Matricula
            this.BusMatricula = "";
            this.BusMatricula = txtNroMatricula.Text.Trim();

            //Busqueda / Filtrar Apellido y Nombre
            this.BusNomyApe = "";
            this.BusNomyApe = txtApellidoyNombre.Text.Trim();

            //Busqueda/ Filtrar Consejo
            idAux = 0;
            int.TryParse(ddlConsejos.SelectedItem.Value, out idAux);
            this.BusConsejo = idAux;

            //idAux = 0;
            //int.TryParse(ddlSubperfiles.SelectedItem.Value, out idAux);
            //this.idsubperfil = idAux;

            this.BusCuit = "";
            this.BusCuit = txtCuit.Text.Trim();

            //Busqueda/ Filtrar Perfil
            this.BusPerfil = "";
            if (ddlPerfiles.SelectedItem.Text != "Todos")
                this.BusPerfil = ddlPerfiles.SelectedItem.Text.Trim();

            //Busqueda / Filtrar subperfil
            idAux = 0;
            this.BusSubperfil = 0;
            if (ddlSubperfiles.SelectedItem != null || !string.IsNullOrEmpty(ddlSubperfiles.SelectedValue))
            {
                int.TryParse(ddlSubperfiles.SelectedItem.Value, out idAux);
                this.BusSubperfil = idAux;
            }


            this.BusUsername = "";
            this.BusUsername = txtUsuario.Text.Trim();

            this.BusEmail = "";
            this.BusEmail = txtEmail.Text.Trim();


            this.BusDadoBaja = "";
            if (!string.IsNullOrEmpty(ddlDadoBaja.SelectedItem.Text))
                this.BusDadoBaja = ddlDadoBaja.SelectedItem.Text.Trim();

            this.BusInhibido = "";
            if (!string.IsNullOrEmpty(ddlinhibido.SelectedItem.Text))
                this.BusInhibido = ddlinhibido.SelectedItem.Text.Trim();


            if (this.BusConsejo > 0 || this.BusSubperfil > 0 || !string.IsNullOrEmpty(this.BusNomyApe) || !string.IsNullOrEmpty(this.BusCuit) || this.BusPerfil.Length > 0 || this.BusSubperfil > 0
                || !string.IsNullOrEmpty(this.BusMatricula) || !string.IsNullOrEmpty(this.BusUsername) || !string.IsNullOrEmpty(this.BusEmail) || !string.IsNullOrEmpty(this.BusDadoBaja) || !string.IsNullOrEmpty(this.BusInhibido))
                this.hayFiltro = true;

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarBuscar();
                guardarFiltro2();
                grdResultados.DataBind();
            }
            catch (Exception ex)
            {
                LogError.Write(ex);
                //lblError.Text = Functions.GetErrorMessage(ex);
                //this.EjecutarScript(updpnlBuscar, "showfrmError();");
                this.Enviar_Mensaje(ex.Message, "");
            }
        }

        public List<Shared.clsProfesional> GetProfesionales(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {

            totalRowCount = 0;

            if (!recuperarFiltro2())
            {
                pnlResultadoBuscar.Visible = false;
                updResultados.Update();
                return null;
            }

            List<Shared.clsProfesional> lstTramites = BuscarProfesionales(startRowIndex, maximumRows, out totalRowCount, sortByExpression);

            pnlCantidadRegistros.Visible = true;

            if (totalRowCount > 1)
            {
                lblCantidadRegistros.Text = string.Format("{0} Profesionales", totalRowCount);
            }
            else if (totalRowCount == 1)
                lblCantidadRegistros.Text = string.Format("{0} Profesionales", totalRowCount);
            else
            {
                pnlCantidadRegistros.Visible = false;
            }
            mostrarExportarDatosCabecera(totalRowCount > 0);
            pnlResultadoBuscar.Visible = true;
            updResultados.Update();

            EjecutarScript(updResultados, "showBusqueda();");

            return lstTramites;
            
        }

        private List<Shared.clsProfesional> BuscarProfesionales(int startRowIndex, int maximumRows, out int totalRowCount, string sortByExpression)
        {
            Guid userid = Functions.GetUserId();

            totalRowCount = 0;

            db = new DGHP_Entities();
            var q = (from profesionales in db.Profesional
                     join tipodoc in db.TipoDocumentoPersonal on profesionales.IdTipoDocumento equals tipodoc.TipoDocumentoPersonalId into d
                     from tipodoc in d.DefaultIfEmpty()

                     join usuario in db.aspnet_Users on profesionales.UserId equals usuario.UserId into u
                     from fd in u.DefaultIfEmpty()
                     where profesionales.ConsejoProfesional.id_grupoconsejo != 0
                     select new Shared.clsProfesional
                     {
                         id_profesional = profesionales.Id,
                         concejoGConsejo = profesionales.ConsejoProfesional.Nombre+ ", " +profesionales.ConsejoProfesional.GrupoConsejos.nombre_grupoconsejo,
                            concejo = profesionales.ConsejoProfesional.GrupoConsejos.nombre_grupoconsejo,
                         id_concejo = profesionales.ConsejoProfesional.id_grupoconsejo,
                         nro_matricula = profesionales.Matricula.ToString(),
                         nombre_apellido = profesionales.Apellido + ", " + profesionales.Nombre,
                         direccion = profesionales.Calle,
                         NroPuerta = profesionales.NroPuerta,
                         cuit = profesionales.Cuit,
                         email = profesionales.Email,
                         Piso = profesionales.Piso,
                         Depto = profesionales.Depto,
                         Localidad = profesionales.Localidad,
                         Provincia = profesionales.Provincia,
                         Sms = profesionales.Sms,
                         Telefono = profesionales.Telefono,
                         NroDocumento = profesionales.NroDocumento,
                         TipoDocumento = tipodoc.Nombre,
                         DadoBaja = profesionales.BajaLogica ? "Si" : "No",
                         Inhibido = profesionales.Inhibido ?? "No",
                         perfiles ="",
                         perfilesList =(from x in db.ConsejoProfesional_RolesPermitidos
                                        join y in db.aspnet_Roles on x.RoleID equals y.RoleId
                                        where fd.aspnet_Roles.Select(z=>z.RoleId).Contains(x.RoleID)
                                        select y).Distinct().ToList(),
                         userId=fd.UserId,
                         usuario=fd.UserName,
                         subperfil ="",
                         subperfilesList=(from x in db.Rel_UsuariosProf_Roles_Clasificacion
                                          join y in db.GrupoConsejos_Roles_Clasificacion on x.id_clasificacion equals y.id_clasificacion
                                          where  x.UserID==profesionales.UserId && fd.aspnet_Roles.Select(z=>z.RoleId).Contains(x.RoleID)
                                          select x).ToList()
                         }
                     );

            //Filtro Apellido y Nombre
            if (this.BusNomyApe.Length > 0)
            {
                /*Se cambia por pedido de mantis 0089419 SGI - Catalogo - Consulta de profesinales*/
                string[] valores = this.BusNomyApe.Trim().Split(' ');
                for (int i = 0; i <= valores.Length - 1; i++)
                {
                    string valor = valores[i];
                    q = q.Where(x => x.nombre_apellido.Contains(valor.Trim()));
                }

            }
            //Filtro NºMatricula
            if (this.BusMatricula.Length > 0)
            {
                //idem anterior
                //int nromatricula = Convert.ToInt32(txtNroMatricula.Text);
                q = q.Where(x => x.nro_matricula == this.BusMatricula);
            }

            //Filtro Username
            if (this.BusUsername.Length > 0)
                q = q.Where(x => x.usuario == this.BusUsername);

            //Filtro "Consejo"
            if (this.BusConsejo > 0)
                q = q.Where(x => x.id_concejo == this.BusConsejo);

            //Filtro Email
            if (this.BusEmail.Length > 0)
            {
                q = q.Where(x => x.email == this.BusEmail);
            }

            //Filtro Cuit
            if (this.BusCuit.Length > 0)
                q = q.Where(x => x.cuit.Contains(this.BusCuit.Trim()) || x.cuit.Contains(this.BusCuit.Trim().Replace("-", "")));

            if (!string.IsNullOrEmpty(this.BusPerfil) && this.BusPerfil.Length > 0)
                q = q.Where(x => x.perfilesList.Select(y => y.RoleName).Contains(this.BusPerfil));

            if (this.BusSubperfil > 0)
            {
                q = q.Where(x => x.subperfilesList.Select(y => y.id_clasificacion).Contains(this.BusSubperfil));
            }

            //Filtro dado de baja
            if (this.BusDadoBaja.Length > 0 && this.BusDadoBaja != "Todos")
            {
                q = q.Where(x => x.DadoBaja.Contains(this.BusDadoBaja));
            }

            //Filtro Inhibido
            if (BusInhibido.Length > 0 && BusInhibido != "Todos")
            {
                q = q.Where(x => x.Inhibido.ToLower().Contains(BusInhibido));
            }

            totalRowCount = q.Count();

            if (sortByExpression != null)
            {
                if (sortByExpression.Contains("DESC"))
                {
                    if (sortByExpression.Contains("concejo"))
                        q = q.OrderByDescending(o => o.concejo).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("matricula"))
                        q = q.OrderByDescending(o => o.nro_matricula).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("nombre"))
                        q = q.OrderByDescending(o => o.nombre_apellido).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("cuit"))
                        q = q.OrderByDescending(o => o.cuit).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("direccion"))
                        q = q.OrderByDescending(o => o.direccion).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("usuario"))
                        q = q.OrderByDescending(o => o.usuario).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("email"))
                        q = q.OrderByDescending(o => o.email).Skip(startRowIndex).Take(maximumRows);
                }
                else
                {
                    if (sortByExpression.Contains("concejo"))
                        q = q.OrderBy(o => o.concejo).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("matricula"))
                        q = q.OrderBy(o => o.nro_matricula).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("nombre"))
                        q = q.OrderBy(o => o.nombre_apellido).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("cuit"))
                        q = q.OrderBy(o => o.cuit).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("direccion"))
                        q = q.OrderBy(o => o.direccion).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("usuario"))
                        q = q.OrderBy(o => o.usuario).Skip(startRowIndex).Take(maximumRows);
                    else if (sortByExpression.Contains("email"))
                        q = q.OrderBy(o => o.email).Skip(startRowIndex).Take(maximumRows);
                }
            }else
                q = q.OrderBy(o => o.nro_matricula).Skip(startRowIndex).Take(maximumRows);

            var resultados =  q.ToList();

            foreach (var item in resultados)
            {
                if (item.userId!=null)
                {
                   
                    var ri = item.perfilesList;

                    string r = "";
                    foreach (var rol in ri)
                    {
                        string sp = "";
                        r += rol.RoleName;

                        var sri = item.subperfilesList.Where(x=>x.RoleID==rol.RoleId);
                        if (sri != null && sri.Count() > 0)
                        {
                            r += "(";
                            foreach (var srol in sri)
                            {
                                r += srol.GrupoConsejos_Roles_Clasificacion.descripcion_clasificacion;
                            }
                            r += "), ";
                        }
                        else
                        {
                            r += ", ";
                        }

                    }
                    if(r.Length>3 && r.ElementAt(r.Length-2)==',')
                        item.perfiles = r.Substring(0, r.Length - 2);
                    else
                        item.perfiles = r;
                }
            }

            //pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
            //grdResultados.Visible = (grdResultados.Rows.Count > 0);

            //lblCantidadRegistros.Text = resultados.Count().ToString();
            //mostrarExportarDatosCabecera(grdResultados.Rows.Count > 0);
            db.Dispose();

            return resultados;

        }
        //private List<Shared.clsProfesional> BuscarProfesionales()
        //{
        //    db = new DGHP_Entities();
        //    var q = (from profesionales in db.Profesional
        //             join usuario in db.aspnet_Users on profesionales.UserId equals usuario.UserId into u
        //             from fd in u.DefaultIfEmpty()
        //             where profesionales.ConsejoProfesional.id_grupoconsejo != 0
        //             select new Shared.clsProfesional
        //             {
        //                 id_profesional = profesionales.Id,
        //                 concejoGConsejo = profesionales.ConsejoProfesional.Nombre + ", " + profesionales.ConsejoProfesional.GrupoConsejos.nombre_grupoconsejo,
        //                 concejo = profesionales.ConsejoProfesional.GrupoConsejos.nombre_grupoconsejo,
        //                 id_concejo = profesionales.ConsejoProfesional.id_grupoconsejo,
        //                 nro_matricula = profesionales.Matricula.ToString(),
        //                 nombre_apellido = profesionales.Nombre + " " + profesionales.Apellido,
        //                 direccion = profesionales.Calle,
        //                 cuit = profesionales.Cuit,
        //                 email = profesionales.Email,
        //                 DadoBaja = profesionales.BajaLogica ? "Si" : "No",
        //                 perfiles = "",
        //                 perfilesList = (from x in db.ConsejoProfesional_RolesPermitidos
        //                                 join y in db.aspnet_Roles on x.RoleID equals y.RoleId
        //                                 where fd.aspnet_Roles.Select(z => z.RoleId).Contains(x.RoleID)
        //                                 select y).Distinct().ToList(),
        //                 userId = fd.UserId,
        //                 usuario = fd.UserName,
        //                 subperfil = "",
        //                 subperfilesList = (from x in db.Rel_UsuariosProf_Roles_Clasificacion
        //                                    join y in db.GrupoConsejos_Roles_Clasificacion on x.id_clasificacion equals y.id_clasificacion
        //                                    where x.UserID == profesionales.UserId && fd.aspnet_Roles.Select(z => z.RoleId).Contains(x.RoleID)
        //                                    select x).ToList()
        //             }
        //             );

        //    //Filtro Apellido y Nombre
        //    if (this.BusNomyApe.Length > 0)
        //    {
        //        /*Se cambia por pedido de mantis 0089419 SGI - Catalogo - Consulta de profesinales*/
        //        string[] valores = this.BusNomyApe.Trim().Split(' ');
        //        for (int i = 0; i <= valores.Length - 1; i++)
        //        {
        //            string valor = valores[i];
        //            q = q.Where(x => x.nombre_apellido.Contains(valor.Trim()));
        //        }

        //    }
        //    //Filtro NºMatricula
        //    if (this.BusMatricula.Length > 0)
        //    {
        //        //idem anterior
        //        //int nromatricula = Convert.ToInt32(txtNroMatricula.Text);
        //        q = q.Where(x => x.nro_matricula == this.BusMatricula);
        //    }

        //    //Filtro Username
        //    if (this.BusUsername.Length > 0)
        //        q = q.Where(x => x.usuario == this.BusUsername);

        //    //Filtro "Consejo"
        //    if (this.BusConsejo > 0)
        //        q = q.Where(x => x.id_concejo == this.BusConsejo);

        //    //Filtro Email
        //    if (this.BusEmail.Length > 0)
        //    {
        //        q = q.Where(x => x.email == this.BusEmail);
        //    }

        //    //Filtro Cuit
        //    if (this.BusCuit.Length > 0)
        //        q = q.Where(x => x.cuit.Contains(this.BusCuit.Trim()) || x.cuit.Contains(this.BusCuit.Trim().Replace("-", "")));


        //    if (!string.IsNullOrEmpty(this.BusPerfil) && this.BusPerfil.Length > 0)
        //        q = q.Where(x => x.perfilesList.Select(y => y.RoleName).Contains(this.BusPerfil));

        //    if (this.BusSubperfil > 0)
        //    {
        //        q = q.Where(x => x.subperfilesList.Select(y => y.id_clasificacion).Contains(this.BusSubperfil));
        //    }


        //    //Filtro dado de baja
        //    if (this.BusDadoBaja.Length > 0 && this.BusDadoBaja != "Todos")
        //    {
        //        q = q.Where(x => x.DadoBaja.Contains(this.BusDadoBaja));
        //    }

        //    var resultados = q.OrderBy(x => x.nro_matricula).ToList();

        //    foreach (var item in resultados)
        //    {
        //        if (item.userId != null)
        //        {

        //            var ri = item.perfilesList;

        //            string r = "";
        //            foreach (var rol in ri)
        //            {
        //                string sp = "";
        //                r += rol.RoleName;

        //                var sri = item.subperfilesList.Where(x => x.RoleID == rol.RoleId);
        //                if (sri != null && sri.Count() > 0)
        //                {
        //                    r += "(";
        //                    foreach (var srol in sri)
        //                    {
        //                        r += srol.GrupoConsejos_Roles_Clasificacion.descripcion_clasificacion;
        //                    }
        //                    r += "), ";
        //                }
        //                else
        //                {
        //                    r += ", ";
        //                }

        //            }
        //            if (r.Length > 3 && r.ElementAt(r.Length - 2) == ',')
        //                item.perfiles = r.Substring(0, r.Length - 2);
        //            else
        //                item.perfiles = r;
        //        }
        //    }

        //    //pnlCantidadRegistros.Visible = (grdResultados.Rows.Count > 0);
        //    //grdResultados.Visible = (grdResultados.Rows.Count > 0);

        //    //lblCantidadRegistros.Text = resultados.Count().ToString();
        //    //mostrarExportarDatosCabecera(grdResultados.Rows.Count > 0);
        //    db.Dispose();
            
        //    return resultados;

        //}
        protected void grdResultados_Sorting(object sender, GridViewSortEventArgs e) 
        {
            // Change default sort order to Descending for certain columns.
            switch (e.SortExpression)
            {
                case "concejo":
                    if (e.SortExpression != grdResultados.SortExpression)
                    {
                        e.SortDirection = SortDirection.Descending;
                    }
                    break;
                case "matricula":
                    // Current sort column is different than previous sort column
                    if (e.SortExpression != grdResultados.SortExpression)
                    {
                        e.SortDirection = SortDirection.Descending;
                    }
                    break;
                case "nombre":
                    // Current sort column is different than previous sort column
                    if (e.SortExpression != grdResultados.SortExpression)
                    {
                        e.SortDirection = SortDirection.Descending;
                    }
                    break;
                case "cuit":
                    // Current sort column is different than previous sort column
                    if (e.SortExpression != grdResultados.SortExpression)
                    {
                        e.SortDirection = SortDirection.Descending;
                    }
                    break;
                case "direccion":
                    // Current sort column is different than previous sort column
                    if (e.SortExpression != grdResultados.SortExpression)
                    {
                        e.SortDirection = SortDirection.Descending;
                    }
                    break;
                case "perfiles":
                    // Current sort column is different than previous sort column
                    if (e.SortExpression != grdResultados.SortExpression)
                    {
                        e.SortDirection = SortDirection.Descending;
                    }
                    break;
                case "usuario":
                    // Current sort column is different than previous sort column
                    if (e.SortExpression != grdResultados.SortExpression)
                    {
                        e.SortDirection = SortDirection.Descending;
                    }
                    break;
                case "email":
                    // Current sort column is different than previous sort column
                    if (e.SortExpression != grdResultados.SortExpression)
                    {
                        e.SortDirection = SortDirection.Descending;
                    }
                    break;   
                default:
                    break;
            }
        }
        private void CargarDatos(int id_datos)
        {

            db = new DGHP_Entities();
            /* qry.AppendLn("SELECT tipoDoc.nombre as tipoDoc_nom, con.Nombre as consejo_nom, prof.*");
        qry.AppendLn("FROM profesional prof");
        qry.AppendLn("INNER JOIN TipoDocumentoPersonal tipoDoc ON prof.IdTipoDocumento = tipoDoc.TipoDocumentoPersonalId");
        qry.AppendLn("INNER JOIN Consejoprofesional con ON prof.IdConsejo = con.Id");
        qry.AppendLn("WHERE prof.Id = ?", id_profesional);
             */
            var dato = (from profesionales in db.Profesional.Where(x => x.Id == id_datos)
                        join tipoDocumento in db.TipoDocumentoPersonal on profesionales.IdTipoDocumento equals tipoDocumento.TipoDocumentoPersonalId into zr
                        from fd in zr.DefaultIfEmpty()
                        join consejoProfesional in db.ConsejoProfesional on profesionales.IdConsejo equals consejoProfesional.Id into rh
                        from hd in rh.DefaultIfEmpty()
                        select new
                        {
                            id_profesional = profesionales.Id,
                            UserID = profesionales.UserId,
                            ConsejoProfesional = profesionales.ConsejoProfesional,
                            Matricula = profesionales.Matricula,
                            Apellido = profesionales.Apellido,
                            Nombre = profesionales.Nombre,
                            NroDocumento = profesionales.NroDocumento,
                            TipoDocumento = fd.Nombre,
                            Calle = profesionales.Calle,
                            NroPuerta = profesionales.NroPuerta,
                            Piso = profesionales.Piso,
                            Depto = profesionales.Depto,
                            Provincia = profesionales.Provincia,
                            Localidad = profesionales.Localidad,
                            Email = profesionales.Email,
                            Sms = profesionales.Sms,
                            Telefono = profesionales.Telefono,
                            Cuit = profesionales.Cuit,
                            IngresosBrutos = profesionales.IngresosBrutos,
                            MatriculaMetrogas = profesionales.MatriculaMetrogas,
                            CategoriaMetrogas = profesionales.CategoriaMetrogas,
                        }
                     ).FirstOrDefault(x => x.id_profesional == id_datos);

            
            if (dato != null)
            {

                var perfiles = (from x in db.ConsejoProfesional_RolesPermitidos
                                join y in db.aspnet_Roles on x.RoleID equals y.RoleId
                                join fd2 in db.aspnet_Users on dato.UserID equals fd2.UserId
                                join rel in db.Rel_UsuariosProf_Roles_Clasificacion on dato.UserID equals rel.UserID into subr
                                from rel in subr.DefaultIfEmpty()
                                join cls in db.GrupoConsejos_Roles_Clasificacion on rel.id_clasificacion equals cls.id_clasificacion into subp
                                from cls in subp.DefaultIfEmpty()
                                where fd2.aspnet_Roles.Select(z => z.RoleId).Contains(x.RoleID)
                                select new
                                {
                                    Perfiles = cls.descripcion_clasificacion == null ? (y.RoleName + " ") : (y.RoleName + " (" + cls.descripcion_clasificacion + ") "),
                                    
                                }).Distinct();

                hid_id_profesionalReq.Value = id_datos.ToString();
                txtConsejoReq.Text = dato.ConsejoProfesional.Nombre;
                txtNroMatriculaReq.Text = dato.Matricula.ToString();
                txtApellidoReq.Text = dato.Apellido;
                txtNombreReq.Text = dato.Nombre;
                txtTipoYNroDocReq.Text = Convert.ToString(dato.TipoDocumento)+ " "+dato.NroDocumento;
                txtCalleReq.Text = dato.Calle;
                txtNroReq.Text = dato.NroPuerta;
                txtPisoReq.Text = dato.Piso;
                txtDeptoReq.Text = dato.Depto;
                txtProvinciaReq.Text = dato.Provincia;
                txtLocalidadReq.Text = dato.Localidad;
                txtEmailReq.Text = dato.Email;
                txtSmsReq.Text = dato.Sms;
                txtTelefonoReq.Text = dato.Telefono;
                txtCuitReq.Text = dato.Cuit;
                txtNroIngresosBrutosReq.Text = Convert.ToString(dato.IngresosBrutos);
                txtMatriculaMetrogas.Text = dato.MatriculaMetrogas != null ? Convert.ToString(dato.MatriculaMetrogas) : "-";
                txtCategoriaMetrogas.Text = dato.CategoriaMetrogas != null ? Convert.ToString(dato.CategoriaMetrogas) : "-";

                if(perfiles != null)
                    txtPerfilReq.Text = string.Join(" ", perfiles.Select(x => x.Perfiles));
                
            }
            db.Dispose();
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnEditar = (LinkButton)sender;
                int id_datos = int.Parse(btnEditar.CommandArgument);
                btnExportarInformes.NavigateUrl = "~/Reportes/ImprimirProfesional.aspx?id=" + id_datos.ToString(); 
                LimpiarDatos();
                CargarDatos(id_datos);
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

        protected void lnkEliminarCondicionReq_Command(object sender, CommandEventArgs e)
        {
            try
            {
                Guid userid = (Guid)Membership.GetUser().ProviderUserKey;

                LinkButton lnkEditar = (LinkButton)sender;
                int idCondicion = int.Parse(lnkEditar.CommandArgument);

                db = new DGHP_Entities();

                using (TransactionScope Tran = new TransactionScope())
                {
                    try
                    {
                        db.Rubros_EliminarRubrosCondiciones(idCondicion, userid);
                        Tran.Complete();
                    }
                    catch (Exception ex)
                    {
                        Tran.Dispose();
                        throw ex;
                    }
                }

                grdResultados.DataBind();
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
        
        private void mostrarExportarDatosCabecera(bool mostrar)
        {


            btnExportarCabecera.Visible = mostrar;
        }

        #region paginado grilla

        private int codZonaPlaneamiento = 0;
        private string nombreZonaPlaneamiento = "";
        private int codZonaHabilitacion = 0;


        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grdResultados.PageIndex = e.NewPageIndex;
                IniciarEntity();
                grdResultados.DataBind();
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
                IniciarEntity();
                grdResultados.DataBind();
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
                IniciarEntity();
                grdResultados.DataBind();
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
                IniciarEntity();
                grdResultados.DataBind();
            }
            catch (Exception ex)
            {
                Enviar_Mensaje(ex.Message, "");
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
                                btn.CssClass = "btn btn-inverse";
                            }
                        }
                    }

                    UpdatePanel updPnlPager = (UpdatePanel)fila.Cells[0].FindControl("updPnlPager");
                    if (updPnlPager != null)
                        updPnlPager.Update();



                }
                //grdResultados.SelectMethod = "GetProfesionales";

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }


        }

        private void elimiarFiltro()
        {
            ViewState["filtro"] = null;
        }

        private void guardarFiltro()
        {
            string filtro = this.codZonaPlaneamiento + "|" + this.nombreZonaPlaneamiento + "|" + this.codZonaHabilitacion;
            ViewState["filtro"] = filtro;

        }
        private void guardarFiltro2()
        {
            string filtro =

            this.BusMatricula + "|" +
            this.BusCuit + "|" +
            this.BusNomyApe + "|" +
            this.BusConsejo + "|" +
            this.BusPerfil + "|" +
            this.BusSubperfil + "|" +
            this.BusUsername + "|" +
            this.BusEmail + "|" +
            this.BusDadoBaja + "|" +
            this.BusInhibido;
            ViewState["filtro"] = filtro;

        }

        private bool recuperarFiltro2()
        {
            if (ViewState["filtro"] == null)
                return false;

            string filtro = ViewState["filtro"].ToString();

            string[] valores = filtro.Split('|');

            this.BusMatricula = valores[0];
            this.BusCuit = valores[1];
            this.BusNomyApe = valores[2];
            this.BusConsejo = Convert.ToInt32(valores[3]);
            this.BusPerfil = valores[4];
            this.BusSubperfil = Convert.ToInt32(valores[5]);
            this.BusUsername = valores[6];
            this.BusEmail = valores[7];
            this.BusDadoBaja = valores[8];
            this.BusInhibido = valores[9];

            return true;
        }
        private bool recuperarFiltro()
        {
            if (ViewState["filtro"] == null)
                return false;

            string filtro = ViewState["filtro"].ToString();

            string[] valores = filtro.Split('|');

            this.codZonaPlaneamiento = Convert.ToInt32(valores[0]);
            this.codZonaHabilitacion = Convert.ToInt32(valores[2]);

            if (string.IsNullOrEmpty(valores[1]))
            {
                this.nombreZonaPlaneamiento = null;
            }
            else
            {
                this.nombreZonaPlaneamiento = valores[1];
            }

            return true;
        }

        //private void Enviar_Mensaje(string mensaje, string titulo)
        //{
        //    mensaje = System.Web.HttpUtility.HtmlEncode(mensaje);
        //    if (string.IsNullOrEmpty(titulo))
        //        titulo = System.Web.HttpUtility.HtmlEncode(this.Title);
        //    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
        //            "mostrarMensaje", "mostrarMensaje('" + mensaje + "','" + titulo + "')", true);
        //}

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
                ScriptManager.RegisterStartupScript(btn_BuscarTramite, btn_BuscarTramite.GetType(), script_nombre, script, true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), script_nombre, script, true);
            }

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


        #region ExportacionExcel


        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                bool exportacion_en_proceso = (Session["exportacion_en_proceso"] != null ? (bool)Session["exportacion_en_proceso"] : false);

                if (exportacion_en_proceso)
                    lblRegistrosExportados.Text = Convert.ToString(Session["progress_data"]);
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

        protected void btnExportarProfesionalesAExcel_Click(object sender, EventArgs e)
        {
            mostrarTimer("Profesionales");
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(ExportarProfesionalesAExcel));
            thread.Start();
        }

        private void ExportarProfesionalesAExcel()
        {
            decimal cant_registros_x_vez = 0m;
            int totalRowCount = 0;
            int startRowIndex = 0;
            try
            {
                //Primero busca los filtros
                ValidarBuscar();
                // Esto se realiza para saber el total y de a cuanto se va mostrar el progreso.
                BuscarProfesionales(startRowIndex, 0, out totalRowCount, "nombre_apellido");
                if (totalRowCount < 20000)
                    cant_registros_x_vez = 1000m;
                else
                    cant_registros_x_vez = 5000m;
                int cantidad_veces = (int)Math.Ceiling(totalRowCount / cant_registros_x_vez);
                List<Shared.clsProfesional> resultados = new List<Shared.clsProfesional>();
                for (int i = 1; i <= cantidad_veces; i++)
                {
                    resultados.AddRange(BuscarProfesionales(startRowIndex, Convert.ToInt32(cant_registros_x_vez), out totalRowCount, "nombre_apellido"));
                    Session["progress_data"] = string.Format("{0} registros exportados.", resultados.Count);
                    startRowIndex += Convert.ToInt32(cant_registros_x_vez);
                }
                Session["progress_data"] = string.Format("{0} registros exportados.", resultados.Count);
                var lstExportar = (from res in resultados
                                   select new
                                   {
                                       Profesional = res.nombre_apellido,
                                       Matricula = res.nro_matricula,
                                       Consejo = res.concejo,
                                       TipoDocumento = res.TipoDocumento,
                                       NroDocumento = res.NroDocumento,
                                       Cuit = res.cuit,
                                       Direccion = res.direccion + " " + res.NroPuerta + ", " + res.Piso + " " + res.Depto,
                                       Perfiles = res.perfiles,
                                       Username = res.usuario,
                                       Email = res.email,
                                       Provincia = res.Provincia,
                                       Localidad = res.Localidad,
                                       Sms = res.Sms,
                                       Telefono = res.Telefono,
                                       DadodeBaja = res.DadoBaja,
                                       Inhibido = res.Inhibido,
                                   }).ToList();

                // Convierte la lista en un dataset
                DataSet ds = new DataSet();
                DataTable dt = Functions.ToDataTable(lstExportar);
                dt.TableName = "Profesionales - Listado";
                ds.Tables.Add(dt);
                string savedFileName = Constants.Path_Temporal + Session["filename_exportacion"].ToString();

                Functions.EliminarArchivosDirectorioTemporal();
                // Utiliza DocumentFormat.OpenXml para exportar a excel
                Model.CreateExcelFile.CreateExcelDocument(ds, savedFileName);
                // quita la variable de session.
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");
            }
            catch
            {
                //LogError.Write(ex);
                //lblError.Text = Functions.GetErrorMessage(ex);
                //this.EjecutarScript(updPnlResultadoBuscar, "showfrmError();");
                Session.Remove("progress_data");
                Session.Remove("exportacion_en_proceso");

            }
        }

        #endregion

    }
}