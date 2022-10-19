using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Ajax.Utilities;
using SGI.Model;
using SGI.Seguridad;
using Syncfusion.DocIO.DLS;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataVisualization.DiagramEnums;
using Syncfusion.Linq;
using Syncfusion.Pdf.Lists;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAlrunsRecord;

namespace SGI.Operaciones
{
    public partial class SolicitudesForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion





            string idSolicitudStr = (Request.QueryString["idSolicitud"] == null) ? "" : Request.QueryString["idSolicitud"].ToString();
            if (String.IsNullOrEmpty(idSolicitudStr))
            {
                Response.Redirect("~/Operaciones/SolicitudesIndex.aspx");
            }
            int idSolicitud = int.Parse(idSolicitudStr);
            hdidSolicitud.Value = idSolicitudStr;

            string SSIT_TRANSF = (Request.QueryString["SSIT_TRANSF"] == null) ? "" : Request.QueryString["SSIT_TRANSF"].ToString();
            hdSSIT_TRANSF.Value = SSIT_TRANSF;


            if (!IsPostBack)
            {
                ddlTipoEstado.DataSource = CargarTipoEstadoSolicitud();
                ddlTipoEstado.DataTextField = "Descripcion";
                ddlTipoEstado.DataValueField = "Id";
                ddlTipoEstado.DataBind();
                if (SSIT_TRANSF == "S")
                {
                    SSIT_Solicitudes sSIT_Solicitudes = CargarSSIT_SolicitudesByIdSolicitud(idSolicitud);
                    calFechaLibrado.Visible = true;
                    chkFecLibrado.Visible = true;
                    lblFecLibrado.Visible = true;
                    ddlTipoEstado.SelectedValue = sSIT_Solicitudes.id_estado.ToString();
                    if (sSIT_Solicitudes.FechaLibrado != null)
                    {
                        calFechaLibrado.SelectedDate = (DateTime)sSIT_Solicitudes.FechaLibrado;
                        calFechaLibrado.VisibleDate = (DateTime)sSIT_Solicitudes.FechaLibrado;

                    }
                    else
                    {
                        calFechaLibrado.SelectedDate = DateTime.Today;
                        calFechaLibrado.VisibleDate = DateTime.Today;
                    }


                }
                else
                {
                    Transf_Solicitudes transf_Solicitudes = CargarTransf_SolicitudesByIdSolicitud(idSolicitud);
                    ddlTipoEstado.SelectedValue = transf_Solicitudes.id_estado.ToString();
                    calFechaLibrado.Visible = false;
                    chkFecLibrado.Visible = false;
                    lblFecLibrado.Visible = false;
                }


                // ddlUsuarioAsignado_tramitetarea_SelectedIndexChanged(null, null);

                //   hdFechaInicio_tramitetarea.Value = calFechaInicio_tramitetarea.SelectedDate.ToShortDateString();

            }

        }

        #region Methods
        public int DropDownListIndex(List<Syncfusion.JavaScript.Web.DropDownListItem> dropdownList, string search)
        {

            int indexVal = dropdownList.Select((item, i) => new { Item = item, Index = i })
                .First(x => x.Item.Value == search).Index;
            return indexVal;
        }



        public List<TipoEstadoSolicitud> CargarTipoEstadoSolicitud()
        {


            DGHP_Entities db = new DGHP_Entities();

            List<TipoEstadoSolicitud> q = (from usu in db.TipoEstadoSolicitud
                                           orderby (usu.Descripcion)
                                           select usu).ToList();
            return q;
        }
        public SSIT_Solicitudes CargarSSIT_SolicitudesByIdSolicitud(int IdSolicitud)
        {


            DGHP_Entities db = new DGHP_Entities();

            SSIT_Solicitudes q = (from s in db.SSIT_Solicitudes
                                  where s.id_solicitud == IdSolicitud
                                  select s).FirstOrDefault();
            return q;
        }
        public Transf_Solicitudes CargarTransf_SolicitudesByIdSolicitud(int IdSolicitud)
        {


            DGHP_Entities db = new DGHP_Entities();

            Transf_Solicitudes q = (from s in db.Transf_Solicitudes
                                    where s.id_solicitud == IdSolicitud
                                    select s).FirstOrDefault();
            return q;
        }
        #endregion

        #region Events


        protected void btnSave_Click(object sender, EventArgs e)
        {
            string SSIT_TRANSF = hdSSIT_TRANSF.Value;
            int idSolicitud = int.Parse(hdidSolicitud.Value);
            SSIT_Solicitudes sSIT_Solicitudes = new SSIT_Solicitudes();
            Transf_Solicitudes transf_Solicitudes = new Transf_Solicitudes();
            DGHP_Entities context = new DGHP_Entities();
            if (SSIT_TRANSF == "S")
            {
                sSIT_Solicitudes = CargarSSIT_SolicitudesByIdSolicitud(idSolicitud);
                sSIT_Solicitudes.id_estado = int.Parse(ddlTipoEstado.SelectedValue);

                if (chkFecLibrado.Checked)
                {
                    sSIT_Solicitudes.FechaLibrado = null;
                }
                else
                {
                    if (sSIT_Solicitudes.FechaLibrado != null)
                    {
                        sSIT_Solicitudes.FechaLibrado = (DateTime)sSIT_Solicitudes.FechaLibrado;
                    }
                }

                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.SSIT_Solicitudes.AddOrUpdate(sSIT_Solicitudes);
                        context.SaveChanges();
                        dbContextTransaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }



            }

            else
            {
                transf_Solicitudes = CargarTransf_SolicitudesByIdSolicitud(idSolicitud);
                transf_Solicitudes.id_estado = int.Parse(ddlTipoEstado.SelectedValue);
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Transf_Solicitudes.AddOrUpdate(transf_Solicitudes);
                        context.SaveChanges();
                        dbContextTransaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                    }
                }
            }


            Response.Redirect("~/Operaciones/SolicitudesIndex.aspx?idSolicitud=" + hdidSolicitud.Value);

        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Operaciones/AdministrarTareasDeUnaSolicitud.aspx?idSolicitud=" + hdidSolicitud.Value);
        }

        #endregion

        protected void calFechaLibrado_SelectionChanged(object sender, EventArgs e)
        {

        }
    }
}