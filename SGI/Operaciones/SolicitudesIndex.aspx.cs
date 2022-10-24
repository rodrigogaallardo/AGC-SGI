using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using SGI.Model;
using Syncfusion.XlsIO;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGI.Operaciones
{
    public partial class SolicitudesIndex : System.Web.UI.Page
    {
        static List<TipoEstadoSolicitud> TipoEstadoSolicitudList;
        static List<CPadron_Estados> CPadron_EstadosList;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region RedirectToLoginPage
            MembershipUser usu = Membership.GetUser();
            if (usu == null)
                FormsAuthentication.RedirectToLoginPage();
            #endregion
            if (!IsPostBack)
            {
                string idSolicitudStr = (Request.QueryString["idSolicitud"] == null) ? "" : Request.QueryString["idSolicitud"].ToString();
                txtBuscarSolicitud.Text = idSolicitudStr;

                DGHP_Entities entities = new DGHP_Entities();
                TipoEstadoSolicitudList = (from t in entities.TipoEstadoSolicitud
                                           orderby (t.Descripcion)
                                           select t).ToList();

                CPadron_EstadosList = (from t in entities.CPadron_Estados
                                       orderby (t.nom_estado_usuario)
                                       select t).ToList();


                CargarSolicitud();
            }
        }

        public void CargarSolicitud()
        {
          
            lblMsj.Text = "";
            int idSolicitud;

            bool couldParse = int.TryParse(txtBuscarSolicitud.Text, out idSolicitud);

            if (couldParse)
            {
                DGHP_Entities entities = new DGHP_Entities();

                #region MyRegion
                // List<SolicitudesDto>
                List<SolicitudesDto> SolicitudesDtoAuxList = (from s in entities.SSIT_Solicitudes
                                             join tt in entities.TipoTramite on s.id_tipotramite equals tt.id_tipotramite
                                             join te in entities.TipoExpediente on s.id_tipoexpediente equals te.id_tipoexpediente
                                             join ts in entities.SubtipoExpediente on s.id_subtipoexpediente equals ts.id_subtipoexpediente
                                             join tes in entities.TipoEstadoSolicitud on s.id_estado equals tes.Id
                                             join u in entities.aspnet_Users on s.CreateUser equals u.UserId
                                             where s.id_solicitud == idSolicitud
                                             select new SolicitudesDto
                                             {
                                                 tipo = "S",
                                                 id_solicitud = s.id_solicitud,

                                                 id_tipotramite = s.id_tipotramite,
                                                 descripcion_tipotramite = tt.descripcion_tipotramite,

                                                 id_tipoexpediente = s.id_tipoexpediente,
                                                 descripcion_tipoexpediente = te.descripcion_tipoexpediente,

                                                 id_subtipoexpediente = s.id_subtipoexpediente,
                                                 descripcion_subtipoexpediente = ts.descripcion_subtipoexpediente,

                                                 id_estado = s.id_estado,
                                                 estado = tes.Descripcion,

                                                 CreateDate = s.CreateDate,

                                                 CreateUser = u.UserName,

                                                 CodigoSeguridad = s.CodigoSeguridad,

                                                 FechaLibrado = s.FechaLibrado
                                             })
                                   .Union(from s in entities.Transf_Solicitudes
                                          join tt in entities.TipoTramite on s.id_tipotramite equals tt.id_tipotramite
                                          join te in entities.TipoExpediente on s.id_tipoexpediente equals te.id_tipoexpediente
                                          join ts in entities.SubtipoExpediente on s.id_subtipoexpediente equals ts.id_subtipoexpediente
                                          join tes in entities.TipoEstadoSolicitud on s.id_estado equals tes.Id
                                          join u in entities.aspnet_Users on s.CreateUser equals u.UserId
                                          where s.id_solicitud == idSolicitud
                                          select new SolicitudesDto
                                          {
                                              tipo = "T",
                                              id_solicitud = s.id_solicitud,

                                              id_tipotramite = s.id_tipotramite,
                                              descripcion_tipotramite = tt.descripcion_tipotramite,

                                              id_tipoexpediente = s.id_tipoexpediente,
                                              descripcion_tipoexpediente = te.descripcion_tipoexpediente,

                                              id_subtipoexpediente = s.id_subtipoexpediente,
                                              descripcion_subtipoexpediente = ts.descripcion_subtipoexpediente,

                                              id_estado = s.id_estado,
                                              estado = tes.Descripcion,

                                              CreateDate = s.CreateDate,

                                              CreateUser = u.UserName,

                                              CodigoSeguridad = s.CodigoSeguridad,

                                              FechaLibrado = s.CreateDate

                                          })
                                   .Union(from s in entities.CPadron_Solicitudes
                                          join tt in entities.TipoTramite on s.id_tipotramite equals tt.id_tipotramite
                                          join te in entities.TipoExpediente on s.id_tipoexpediente equals te.id_tipoexpediente
                                          join ts in entities.SubtipoExpediente on s.id_subtipoexpediente equals ts.id_subtipoexpediente
                                          join tes in entities.TipoEstadoSolicitud on s.id_estado equals tes.Id
                                          join u in entities.aspnet_Users on s.CreateUser equals u.UserId
                                          where s.id_cpadron == idSolicitud
                                          select new SolicitudesDto
                                          {
                                              tipo = "P",
                                              id_solicitud = s.id_cpadron,

                                              id_tipotramite = s.id_tipotramite,
                                              descripcion_tipotramite = tt.descripcion_tipotramite,

                                              id_tipoexpediente = s.id_tipoexpediente,
                                              descripcion_tipoexpediente = te.descripcion_tipoexpediente,

                                              id_subtipoexpediente = s.id_subtipoexpediente,
                                              descripcion_subtipoexpediente = ts.descripcion_subtipoexpediente,

                                              id_estado = s.id_estado,
                                              estado = tes.Descripcion,

                                              CreateDate = s.CreateDate,

                                              CreateUser = u.UserName,

                                              CodigoSeguridad = s.CodigoSeguridad,

                                              FechaLibrado = s.CreateDate

                                          })
                                   .ToList();


                gridViewSSIT_Solicitudes.Visible = true;
                gridViewSSIT_Solicitudes.DataSource = SolicitudesDtoAuxList;
                gridViewSSIT_Solicitudes.DataBind();
                hdidSolicitud.Value = idSolicitud.ToString();


              
                #endregion

               if(SolicitudesDtoAuxList.Count()==0)
                { 
                lblMsj.Text = "No hay datos pra esta Solicitud";
                }
               else
                {
                    lblMsj.Text = "";
                }

            }
        }







        protected void btnBuscarSolicitud_Click(object sender, EventArgs e)
        {
            this.CargarSolicitud();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string tipo = ((Button)sender).ToolTip;
            Response.Redirect("~/Operaciones/SolicitudesForm.aspx?idSolicitud=" + hdidSolicitud.Value + "&tipo=" + tipo);
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridView grid = (GridView)sender;




                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //DGHP_Entities entities = new DGHP_Entities();
                    Label lblTipoEstado = (Label)e.Row.FindControl("lblTipoEstado");
                    Label labelid_estado = (Label)e.Row.FindControl("labelid_estado");
                    Label labelTipoTramite = (Label)e.Row.FindControl("labelTipoTramite");

                    int id = int.Parse(labelid_estado.Text);

                    if (lblTipoEstado.Text == "P")
                    {
                        CPadron_Estados cPadron_Estados = (from t in CPadron_EstadosList
                                                           where t.id_estado == id
                                                           select t).FirstOrDefault();
                        labelTipoTramite.Text = cPadron_Estados.nom_estado_usuario;
                    }
                    else
                    {
                        TipoEstadoSolicitud tipoEstadoSolicitud = (from t in TipoEstadoSolicitudList
                                                                   where t.Id == id
                                                                   select t).FirstOrDefault();
                        labelTipoTramite.Text = tipoEstadoSolicitud.Descripcion;
                    }



                }

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }
        }


    }
}