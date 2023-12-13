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
using SGI.GestionTramite.Controls;
using Syncfusion.DocIO.DLS;
using System.Data.Entity;

namespace SGI
{
    public partial class ConsultaMovimientos : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }


        #region Entity
        DGHP_Entities db = null;
        private void IniciarEntity()
        {
            if (db == null)
            {
                this.db = new DGHP_Entities();
                this.db.Database.CommandTimeout = 300;
            }
        }
        private void FinalizarEntity()
        {
            if (db != null)
                db.Dispose();
        }
        #endregion

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            IniciarEntity();

            #region Carga de Datos
            var usuario = txtUsuario.Text;

            DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);

            DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text);
            
            var tipoMovimientoString = ddlTipoMov.SelectedValue.ToString();

            var funcionalidad = txtFuncionalidad.Text;

            var observacion = ddlObservacionSolicitante.SelectedValue.ToString();


            int nroSolicitud = 0;
            int.TryParse(txtSolicitud.Text, out nroSolicitud);

            #endregion

            #region Bandera Bool
            bool pudo = true;

            if (fechaDesde != fechaHasta)
            {
                pudo = true;
            }
            else
            {
                pudo = false;
                lblError.Text = "La Fecha Desde debe ser distinta a la Fecha Hasta";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updPnlMovimientos, "showfrmError();");
                return;
            }

            if (fechaDesde < fechaHasta)
            {
                pudo = true;
            }
            else
            {
                pudo = false;
                lblError.Text = "La Fecha Desde debe ser menor a la Fecha Hasta";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updPnlMovimientos, "showfrmError();");
                return;
            }

            #endregion


            if (pudo)
            {
                
            }
        }

        protected void grdBuscarMovimientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {


        }
    }
}