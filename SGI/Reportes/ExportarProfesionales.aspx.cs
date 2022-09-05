using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Reportes
{
    public partial class ExportarProfesionales : System.Web.UI.Page
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Exportar();

            }
        }
        private String txtApellidoyNombre = "";
        private String txtNroMatricula = "";
        private String txtCuit = "";
        private String txtConsejos = "";
        private String txtPerfiles = "";
        private String txtSubPerfiles = "";
       

        private void Exportar()
        {

            //Guid userid = Functions.GetUserid();
            DGHP_Entities db = new DGHP_Entities();

            this.txtApellidoyNombre = (Request.QueryString["txtApellidoyNombre"] == null) ? "" : Request.QueryString["txtApellidoyNombre"];
            this.txtNroMatricula = (Request.QueryString["txtNroMatricula"] == null) ? "" : Request.QueryString["txtNroMatricula"];
            this.txtCuit = (Request.QueryString["txtCuit"] == null) ? "" : Request.QueryString["txtCuit"];
            this.txtConsejos = (Request.QueryString["txtConsejos"] == null) ? "" : Request.QueryString["txtConsejos"];
            this.txtPerfiles = (Request.QueryString["txtPerfiles"] == null) ? "" : Request.QueryString["txtPerfiles"];
            this.txtSubPerfiles = (Request.QueryString["txtSubPerfiles"] == null) ? "" : Request.QueryString["txtSubPerfiles"];
           

            db = new DGHP_Entities();
            var q = (from profesionales in db.Profesional
                     join usuario in db.aspnet_Users on profesionales.UserId equals usuario.UserId into u
                     from fd in u.DefaultIfEmpty()
                     where profesionales.ConsejoProfesional.id_grupoconsejo != 0
                     select new SGI.Model.Shared.clsProfesional
                     {
                         id_profesional = profesionales.Id,
                         concejoGConsejo = profesionales.ConsejoProfesional.Nombre + ", " + profesionales.ConsejoProfesional.GrupoConsejos.nombre_grupoconsejo,
                         concejo = profesionales.ConsejoProfesional.Nombre,
                         id_concejo = profesionales.ConsejoProfesional.id_grupoconsejo,
                         nro_matricula = profesionales.Matricula.ToString(),
                         nombre_apellido = profesionales.Nombre + " " + profesionales.Apellido,
                         direccion = profesionales.Calle,
                         cuit = profesionales.Cuit,
                         email = profesionales.Email,
                         perfiles = "",
                         perfilesList = (from x in db.ConsejoProfesional_RolesPermitidos
                                         join y in db.aspnet_Roles on x.RoleID equals y.RoleId
                                         where fd.aspnet_Roles.Select(z => z.RoleId).Contains(x.RoleID)
                                         select y).Distinct().ToList(),
                         userId = fd.UserId,
                         usuario = fd.UserName,
                         subperfil = "",
                         subperfilesList = (from x in db.Rel_UsuariosProf_Roles_Clasificacion
                                            join y in db.GrupoConsejos_Roles_Clasificacion on x.id_clasificacion equals y.id_clasificacion
                                            where x.UserID == profesionales.UserId && fd.aspnet_Roles.Select(z => z.RoleId).Contains(x.RoleID)
                                            select x).ToList()
                     }
                     );
            if (txtApellidoyNombre.Trim().Length > 0)
                q = q.Where(x => x.nombre_apellido.Contains(txtApellidoyNombre.Trim()));
            if (txtNroMatricula.Trim().Length > 0)
            {
                //int nromatricula = Convert.ToInt32(txtNroMatricula.Text);
                q = q.Where(x => x.nro_matricula.Contains(txtNroMatricula.Trim()));
            }
            if (txtCuit.Trim().Length > 0)
                q = q.Where(x => x.cuit.Contains(txtCuit.Trim()));
            if (txtConsejos.Trim().Length >0)
            {
                int? c = int.Parse(txtConsejos);
                q = q.Where(x => x.id_concejo == c);
            }

            if (txtPerfiles.Trim().Length > 0)
                q = q.Where(x => x.perfilesList.Select(y => y.RoleId).Contains(new Guid(txtPerfiles)));

            if (txtSubPerfiles.Trim().Length > 0)
            {
                int sp = int.Parse(txtSubPerfiles);
                q = q.Where(x => x.subperfilesList.Select(y => y.id_clasificacion).Contains(sp));
            }

            var resultados = q.OrderBy(x => x.nombre_apellido).ToList();

            foreach (var item in resultados)
            {
                if (item.userId != null)
                {

                    var ri = item.perfilesList;

                    string r = "";
                    foreach (var rol in ri)
                    {
                        string sp = "";
                        r += rol.RoleName;

                        var sri = item.subperfilesList.Where(x => x.RoleID == rol.RoleId);
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
                    if (r.Length > 3 && r.ElementAt(r.Length - 2) == ',')
                        item.perfiles = r.Substring(0, r.Length - 2);
                    else
                        item.perfiles = r;
                }
            }

            grdResultados.DataSource = resultados;
            grdResultados.DataBind();
            //Response.Clear();
            Response.Buffer = true;

            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "inline; filename=Listado-Profesionales.xls");
            Response.Charset = "iso-8859-1";
            Response.ContentEncoding = System.Text.Encoding.Default;
            this.EnableViewState = false;

            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

            // Get the HTML for the control.

            //grdAscensores.RenderControl(hw);
            grdResultados.RenderBeginTag(hw);
            grdResultados.HeaderRow.RenderControl(hw);

            foreach (GridViewRow row in grdResultados.Rows)
            {
                //row.Cells.RemoveAt(5);
                row.RenderControl(hw);
            }
            grdResultados.FooterRow.RenderControl(hw);

            grdResultados.FooterRow.RenderControl(hw);

            // Write the HTML back to the browser.
            Response.Write(tw.ToString());

        }
    }
}