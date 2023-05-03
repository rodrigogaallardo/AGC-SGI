using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGI.BusinessLogicLayer;
using System.Drawing;
using SGI.Model;
using SGI.DataLayer.Models;
using SGI.BusinessLogicLayer.Constants;

namespace SGI
{
    public partial class AbmCalles : BasePage
    {      
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack && Request.UrlReferrer.AbsolutePath != "/Menu/Items/6")
            {
                LoadData();
                string idCalleStr = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();
                ddlCalles.Text = idCalleStr;
                btnBuscar_OnClick(sender, e);
            }
        }

        #region Entity
        private DGHP_Entities db = null;

        private void IniciarEntity()
        {
            if(this.db == null)
            {
                this.db = new DGHP_Entities();
            }
        }

        private void FinalizarEntity()
        {
            if(this.db != null)
            {
                this.db.Dispose();
                this.db = null;
            }
        }

        #endregion

        private void CargarCalles()
        {
            var lstCalles = (from calle in db.Calles
                             select new
                             {
                                 calle.id_calle,
                                 calle.NombreOficial_calle
                             }).Distinct().OrderBy(x => x.NombreOficial_calle).ToList();

            ddlCalles.DataSource = lstCalles.GroupBy(x => x.NombreOficial_calle).Select(x => x.FirstOrDefault());
            ddlCalles.DataTextField = "NombreOficial_calle";
            ddlCalles.DataValueField = "id_calle";
            ddlCalles.DataBind();

            ddlCalles.Items.Insert(0, "");
        }
        private void LoadData()
        {
            IniciarEntity();
            CargarCalles();
        }
        #region Eventos On Click
        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            ucListaCalle.vaciarGrilla();
        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {

            bool pudo = int.TryParse(ddlCalles.SelectedValue, out int nroCalle);

            ucListaCalle.LoadData(nroCalle);

        }


        protected void btnAgregar_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/AgregarCalle.aspx");
        }

        #endregion


    }

}