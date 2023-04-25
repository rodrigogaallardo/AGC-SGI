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
using System.Data.Entity.Migrations;

namespace SGI
{
    public partial class EditarCalle : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id_calle = (Request.QueryString["id"] == null) ? "" : Request.QueryString["id"].ToString();

            var idCalle = Convert.ToInt32(id_calle);



            if (!IsPostBack)
            {
                LoadData();
                CargarGrilla(idCalle);
            }
        }

        #region Entity
        private DGHP_Entities db = null;

        private void IniciarEntity()
        {
            if (this.db == null)
            {
                this.db = new DGHP_Entities();
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



        private void LoadData()
        {
            IniciarEntity();
            CargarTipoCalles();
        }




        protected void btnAgregar_OnClick(object sender, EventArgs e)
        {
            var context = new DGHP_Entities();

            #region Validaciones
            if (string.IsNullOrEmpty(ddlTipoCalle.Text))
            {
                lblError.Text = "Debe seleccionar un tipo de calle";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }

            if (string.IsNullOrEmpty(codigoCalle.Text))
            {
                lblError.Text = "Debe ingresar un código de calle";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }

            if (string.IsNullOrEmpty(nombreCalle.Text))
            {
                lblError.Text = "Debe ingresar el nombre de la calle";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }

            if (string.IsNullOrEmpty(altIzqInicio.Text))
            {
                lblError.Text = "Debe ingresar la Altura Izquierda de Inicio";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }

            if (string.IsNullOrEmpty(altIzqFin.Text))
            {
                lblError.Text = "Debe ingresar la Altura Izquierda de Fin";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }

            if (string.IsNullOrEmpty(altDerInicio.Text))
            {
                lblError.Text = "Debe ingresar la Altura Derecha de Inicio";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }

            if (string.IsNullOrEmpty(altDerFin.Text))
            {
                lblError.Text = "Debe ingresar la Altura Derecha de Fin";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }

            #endregion


            #region Carga de Datos
            //Tipo de Calle
            var tipoCalleString = ddlTipoCalle.SelectedValue.ToString();

            //Codigo de Calle
            int codigoCalleInt = 0;
            int.TryParse(codigoCalle.Text, out codigoCalleInt);

            //Nombre de Calle
            var nombreCalleString = nombreCalle.Text;

            //Altura Izquierda Inicio
            int alturaIzquierdaInicio = 0;
            int.TryParse(altIzqInicio.Text, out alturaIzquierdaInicio);

            //Altura Izquierda Fin
            int alturaIzquierdaFin = 0;
            int.TryParse(altIzqFin.Text, out alturaIzquierdaFin);

            //Altura Derecha Inicio
            int alturaDerechaInicio = 0;
            int.TryParse(altDerInicio.Text, out alturaDerechaInicio);

            //Altura Derecha Fin
            int alturaDerechaFin = 0;
            int.TryParse(altDerFin.Text, out alturaDerechaFin);
            #endregion


            #region Bandera Bool
            bool pudo = true;

            if(alturaIzquierdaInicio != alturaDerechaInicio && alturaIzquierdaFin != alturaDerechaFin)
            {
                pudo = true;
            }
            else
            {
                lblError.Text = "La altura de inicio y de fin deben ser distintos";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }

            if (alturaIzquierdaInicio != alturaIzquierdaFin && alturaDerechaInicio != alturaIzquierdaFin)
            {
                pudo = true;
            }
            else
            {
                pudo = false;
                lblError.Text = "La altura de inicio y de fin deben ser distintos";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }

            if (alturaIzquierdaInicio < alturaIzquierdaFin && alturaDerechaInicio < alturaDerechaFin)
            {
                pudo = true;
            }
            else
            {
                pudo = false;
                lblError.Text = "La altura de Inicio debe ser menor a la altura de Fin";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
            }


            //Validacion de si ya existe el codigo de calle y la calle en la base de datos
            var ftx = new DGHP_Entities();

            Calles calle = (from c in ftx.Calles
                            where c.Codigo_calle == codigoCalleInt
                            && c.NombreOficial_calle == nombreCalleString
                            select c).FirstOrDefault();

            if(calle == null)
            {
                pudo = true;
            }
            else
            {           
                    pudo = false;
                    lblError.Text = "El código de calle y/o la calle ya existe";
                    lblError.ForeColor = Color.Red;
                    this.EjecutarScript(updResultados, "showfrmError();");
                    return;
            }

            #endregion

            using (var dbContext = context.Database.BeginTransaction())
            {
                try
                {
                    if (pudo)
                    {
                        //ID
                        var idNuevo = Convert.ToInt32(Request.QueryString["id"]);

                        var entity = new SGI.Model.Calles()

                        {
                            id_calle = idNuevo,
                            Codigo_calle = codigoCalleInt,
                            NombreOficial_calle = nombreCalleString,
                            AlturaIzquierdaInicio_calle = alturaIzquierdaInicio,
                            AlturaIzquierdaFin_calle = alturaIzquierdaFin,
                            AlturaDerechaInicio_calle = alturaDerechaInicio,
                            AlturaDerechaFin_calle = alturaDerechaFin,
                            TipoCalle_calle = tipoCalleString,
                            CreateDate = DateTime.Now,
                            CreateUser = Functions.GetUserId().ToString()
                        };

                        context.Calles.AddOrUpdate(entity);
                        context.SaveChanges();
                        dbContext.Commit();

                        Response.Redirect("AbmCalles.aspx?id=" + idNuevo);
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error al editar la calle";
                    lblError.ForeColor = Color.Red;
                    this.EjecutarScript(updResultados, "showfrmError();");
                }
            }
        }

        protected void btnReturn_OnClick(object sender, EventArgs e)
        {
            var idNuevo = Convert.ToInt32(Request.QueryString["id"]);
            Response.Redirect("ABMCalles.aspx?id=" + idNuevo.ToString());
        }

        protected void CargarTipoCalles()
        {
            var lstTipoCalles = (from tipoCalle in db.Calles
                                 select new
                                 {
                                     tipoCalle.TipoCalle_calle
                                 }).Distinct().OrderBy(x => x.TipoCalle_calle).ToList();

            ddlTipoCalle.DataSource = lstTipoCalles.GroupBy(x => x.TipoCalle_calle).Select(x => x.FirstOrDefault());
            ddlTipoCalle.DataTextField = "TipoCalle_calle";
            ddlTipoCalle.DataBind();

            ddlTipoCalle.Items.Insert(0, "");
        }

        protected void CargarGrilla(int id_calle)
        {

            var ftx = new DGHP_Entities();

                Calles calle = (from c in ftx.Calles
                                where c.id_calle == id_calle
                                select c).FirstOrDefault();

            codigoCalle.Text = calle.Codigo_calle.ToString();
            nombreCalle.Text = calle.NombreOficial_calle.ToString();
            altIzqInicio.Text = calle.AlturaIzquierdaInicio_calle.ToString();
            altIzqFin.Text =    calle.AlturaIzquierdaFin_calle.ToString();
            altDerInicio.Text = calle.AlturaDerechaInicio_calle.ToString();
            altDerFin.Text = calle.AlturaDerechaFin_calle.ToString();
            ddlTipoCalle.Text = calle.TipoCalle_calle.ToString();

        }
    }
}
