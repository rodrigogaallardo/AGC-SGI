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
using System.Web;
using Newtonsoft.Json;

namespace SGI
{
    public partial class AgregarCalle : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
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

        private int NuevoId_Calle()
        {
            using (var db = new DGHP_Entities())
            {
                return db.Calles.Max(p => p.id_calle) + 1;
            }
        }

        protected void btnAgregar_OnClick(object sender, EventArgs e)
        {
            var context = new DGHP_Entities();
            Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
            string url = HttpContext.Current.Request.Url.AbsoluteUri.ToString();


            #region Validaciones
            if (string.IsNullOrEmpty(ddlTipoCalle.Text))
            {
                lblError.Text = "Debe seleccionar un tipo de calle";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }

            if (string.IsNullOrEmpty(codigoCalle.Text))
            {
                lblError.Text = "Debe ingresar un código de calle";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }

            if (string.IsNullOrEmpty(nombreCalle.Text))
            {
                lblError.Text = "Debe ingresar el nombre de la calle";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }

            if (string.IsNullOrEmpty(altIzqInicio.Text))
            {
                lblError.Text = "Debe ingresar la Altura Izquierda de Inicio";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }

            if (string.IsNullOrEmpty(altIzqFin.Text))
            {
                lblError.Text = "Debe ingresar la Altura Izquierda de Fin";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }

            if (string.IsNullOrEmpty(altDerInicio.Text))
            {
                lblError.Text = "Debe ingresar la Altura Derecha de Inicio";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }

            if (string.IsNullOrEmpty(altDerFin.Text))
            {
                lblError.Text = "Debe ingresar la Altura Derecha de Fin";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
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

            if (alturaIzquierdaInicio != alturaDerechaInicio && alturaIzquierdaFin != alturaDerechaFin)
            {
                pudo = true;
            }
            else
            {
                lblError.Text = "La Altura de Inicio y de Fin deben ser distintos";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }

            if (alturaIzquierdaInicio != alturaIzquierdaFin && alturaDerechaInicio != alturaIzquierdaFin)
            {
                pudo = true;
            }
            else
            {
                pudo = false;
                lblError.Text = "La Altura de Inicio y de Fin deben ser distintos";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }

            if (alturaIzquierdaInicio < alturaIzquierdaFin && alturaDerechaInicio < alturaDerechaFin)
            {
                pudo = true;
            }
            else
            {
                pudo = false;
                lblError.Text = "La Altura de Inicio debe ser menor a la Altura de Fin";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }


            //Validacion de si ya existe el codigo de calle y el nombre de la calle
            var ftx = new DGHP_Entities();

            Calles calleCod = (from c in ftx.Calles
                            where c.Codigo_calle == codigoCalleInt
                            select c).FirstOrDefault();                                  
         
            if (calleCod == null)
            {
                pudo = true;
            }
            else
            {
                pudo = false;
                lblError.Text = "El código de Calle ya existe";
                lblError.ForeColor = Color.Red;
                this.EjecutarScript(updResultados, "showfrmError();");
                return;
            }


            Calles nombreOficialCalle = (from c in ftx.Calles
                                  where c.NombreOficial_calle == nombreCalleString
                                  select c).FirstOrDefault();

            if (nombreOficialCalle == null)
            {
                pudo = true;
            }
            else 
            {
                pudo = false;
                lblError.Text = "La calle ya existe";
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
                            var idNuevo = NuevoId_Calle();

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

                            Functions.InsertarMovimientoUsuario(userid, DateTime.Now, null, JsonConvert.SerializeObject(entity), url, txtObservacionesSolicitante.Text, "I", 4013);
                            context.Calles.Add(entity);
                            context.SaveChanges();
                            dbContext.Commit();


                            Response.Redirect("ABMCalles.aspx?id=" + idNuevo);
                        }
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Hubo un error agregando la calle";
                        lblError.ForeColor = Color.Red;
                        this.EjecutarScript(updResultados, "showfrmError();");
                    }
                
            }
        }

        protected void btnReturn_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("ABMCalles.aspx");
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
    }
}
