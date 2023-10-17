using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Ajax.Utilities;
using SGI.GestionTramite.Controls;
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
            txtSolicitudId.Text= idSolicitudStr;

            string tipo = (Request.QueryString["tipo"] == null) ? "" : Request.QueryString["tipo"].ToString();
            hdtipo.Value = tipo;


            if (!IsPostBack)
            {
               
                if (tipo == "S")
                {
                    ddlTipoEstado.DataSource = CargarTipoEstadoSolicitud();
                    ddlTipoEstado.DataTextField = "Descripcion";
                    ddlTipoEstado.DataValueField = "Id";
                    ddlTipoEstado.DataBind();

                    SSIT_Solicitudes sSIT_Solicitudes = CargarSSIT_SolicitudesByIdSolicitud(idSolicitud);
                    txtdescripcion_tipotramite.Text = sSIT_Solicitudes.TipoTramite.descripcion_tipotramite;

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
                else if (tipo == "T")
                {
                    ddlTipoEstado.DataSource = CargarTipoEstadoSolicitud();
                    ddlTipoEstado.DataTextField = "Descripcion";
                    ddlTipoEstado.DataValueField = "Id";
                    ddlTipoEstado.DataBind();

                    Transf_Solicitudes transf_Solicitudes = CargarTransf_SolicitudesByIdSolicitud(idSolicitud);
                    txtdescripcion_tipotramite.Text = transf_Solicitudes.TipoTramite.descripcion_tipotramite;
                    ddlTipoEstado.SelectedValue = transf_Solicitudes.id_estado.ToString();
                    calFechaLibrado.Visible = false;
                    chkFecLibrado.Visible = false;
                    lblFecLibrado.Visible = false;
                }
                else
                {
                    ddlTipoEstado.DataSource = CargarCPadron_Estados();
                    ddlTipoEstado.DataTextField = "nom_estado_usuario";
                    ddlTipoEstado.DataValueField = "id_estado";
                    ddlTipoEstado.DataBind();

                    CPadron_Solicitudes cPadron_Solicitudes = CargarCPadron_SolicitudesByIdSolicitud(idSolicitud);
                    txtdescripcion_tipotramite.Text = cPadron_Solicitudes.TipoTramite.descripcion_tipotramite;
                    ddlTipoEstado.SelectedValue = cPadron_Solicitudes.id_estado.ToString();
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
        public List<CPadron_Estados> CargarCPadron_Estados()
        {
            DGHP_Entities db = new DGHP_Entities();

            List<CPadron_Estados> q = (from usu in db.CPadron_Estados
                     orderby (usu.nom_estado_usuario)
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
        public CPadron_Solicitudes CargarCPadron_SolicitudesByIdSolicitud(int IdSolicitud)
        {


            DGHP_Entities db = new DGHP_Entities();

            CPadron_Solicitudes q = (from s in db.CPadron_Solicitudes
                                     where s.id_cpadron == IdSolicitud
                                     select s).FirstOrDefault();
            return q;
        }
        #endregion

        #region Events


        protected void btnSave_Click(object sender, EventArgs e)
        {
            string tipo = hdtipo.Value;
            int idSolicitud = int.Parse(hdidSolicitud.Value);
            SSIT_Solicitudes sSIT_Solicitudes = new SSIT_Solicitudes();
            Transf_Solicitudes transf_Solicitudes = new Transf_Solicitudes();
            CPadron_Solicitudes cPadron_Solicitudes = new CPadron_Solicitudes();
            DGHP_Entities context = new DGHP_Entities();
            if (tipo == "S")
            {
                sSIT_Solicitudes = CargarSSIT_SolicitudesByIdSolicitud(idSolicitud);
                sSIT_Solicitudes.id_estado = int.Parse(ddlTipoEstado.SelectedValue);

                if(sSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Observado)
                {
                    Mailer.MailMessages.SendMail_ObservacionSolicitud1_v2(idSolicitud);
                }

                if(sSIT_Solicitudes.id_estado ==(int)Constants.Solicitud_Estados.Observado_PVH)
                {
                    Mailer.MailMessages.SendMail_ObservacionSolicitud1_v2(idSolicitud);
                }

                if(sSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Aprobada)
                {
                    Mailer.MailMessages.SendMail_AprobadoSolicitud_v2(idSolicitud, DateTime.Now);
                }

                if(sSIT_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Rechazada)
                {
                    Mailer.MailMessages.SendMail_RechazoSolicitud_v2(idSolicitud, DateTime.Now);
                }

                if (chkFecLibrado.Checked)
                {
                    sSIT_Solicitudes.FechaLibrado = null;
                }
                else
                {
                    if (calFechaLibrado.SelectedDate != null)
                    {
                        sSIT_Solicitudes.FechaLibrado = calFechaLibrado.SelectedDate;
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

            else if (tipo == "T")
            {
                transf_Solicitudes = CargarTransf_SolicitudesByIdSolicitud(idSolicitud);
                transf_Solicitudes.id_estado = int.Parse(ddlTipoEstado.SelectedValue);

                if (transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Observado)
                {
                    Mailer.MailMessages.SendMail_ObservacionSolicitud1_v2(idSolicitud);
                }

                if (transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Observado_PVH)
                {
                    Mailer.MailMessages.SendMail_ObservacionSolicitud1_v2(idSolicitud);
                }

                if (transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Aprobada)
                {
                    Mailer.MailMessages.SendMail_AprobadoSolicitud_v2(idSolicitud, DateTime.Now);
                }

                if (transf_Solicitudes.id_estado == (int)Constants.Solicitud_Estados.Rechazada)
                {
                    Mailer.MailMessages.SendMail_RechazoSolicitud_v2(idSolicitud, DateTime.Now);
                }
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
            else
            {
                cPadron_Solicitudes = CargarCPadron_SolicitudesByIdSolicitud(idSolicitud);
                cPadron_Solicitudes.id_estado = int.Parse(ddlTipoEstado.SelectedValue);
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.CPadron_Solicitudes.AddOrUpdate(cPadron_Solicitudes);
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
            Response.Redirect("~/Operaciones/SolicitudesIndex.aspx?idSolicitud=" + hdidSolicitud.Value);
        }

        #endregion

        protected void calFechaLibrado_SelectionChanged(object sender, EventArgs e)
        {

        }
    }
}