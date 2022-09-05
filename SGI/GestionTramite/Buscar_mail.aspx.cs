using SGI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

//namespace SGI.GestionTramite
namespace SGI.GestionTramite
{
    public partial class Buscar_mail : BasePage
    {
        DGHP_Entities db = null;
        DateTime FecAltaDesde;
        DateTime FecAltaHasta;
        DateTime FecDesde;
        DateTime FecHasta;
        string Email;
        string Asunto;
        int Estado;
        //int Proceso;
        string idSolicitud;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Cargarddl();
            }
        }

        #region cargar inicial
        private void Cargarddl()
        {
            IniciarEntity();
            var q = (
                        from edo in db.Email_Estados
                        select new NuevosEstados()
                        {
                            id = edo.id_estado,
                            Descripcion = edo.descripcion
                        }).ToList();

            q.Add(new NuevosEstados() { id = 99, Descripcion = "Todos" });
            ddlEstadoMail.DataSource = q;
            ddlEstadoMail.DataTextField = "Descripcion";
            ddlEstadoMail.DataValueField = "id";
            ddlEstadoMail.SelectedIndex = q.Count() - 1;
            ddlEstadoMail.DataBind();

            //var p = (
            //            from proc in db.Emails_Origenes
            //            select new NuevoProceso()
            //            {
            //                id = proc.id_origen,
            //                Descripcion = proc.descripcion
            //            }).ToList();

            //p.Add(new NuevoProceso() { id = 99, Descripcion = "Todos" });
            //ddlProceso.DataSource = p;
            //ddlProceso.DataTextField = "Descripcion";
            //ddlProceso.DataValueField = "id";
            //ddlProceso.SelectedIndex = p.Count() - 1; ;
            //ddlProceso.DataBind();
            FinalizarEntity();
        }
        internal class NuevosEstados
        {
            public int id { get; set; }
            public string Descripcion { get; set; }
        }
        internal class NuevoProceso
        {
            public int id { get; set; }
            public string Descripcion { get; set; }
        }
        private void IniciarEntity()
        {
            if (db == null)
                db = new DGHP_Entities();
        }
        private void FinalizarEntity()
        {
            if (db != null)
                db.Dispose();
        }
        protected void ddlEstadoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        protected void ddlProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        #endregion

        #region botones
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {

                CargarVariables();
                IniciarEntity();
                //grdBuscarMails.PageIndex = 0;
                CargarGridBuscarMails();
                FinalizarEntity();
                EjecutarScript(updPnlBuscarMails, "showResultado();");
            }
            catch (Exception ex)
            {
                FinalizarEntity();
                throw new Exception(ex.Message);
            }
        }
        protected void btnReenviar_OnClick(object sender, EventArgs e)
        {

            db = new DGHP_Entities();

            db.mail_Reenviar_Mail(Convert.ToInt32(hfMailID.Value));

            lblMsjCola.Text = "Mensaje en cola.";
            this.EjecutarScript(updMsjCola, "showfrmCola();");
            return;
        }
        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {

            EjecutarScript(updPnlBuscarMails, "hideResultado();");
            limpiarCampos();
        }
        protected void cmdSiguiente_Click(object sender, EventArgs e)
        {
            grdBuscarMails.PageIndex = grdBuscarMails.PageIndex + 1;
            IniciarEntity();
            CargarVariables();
            CargarGridBuscarMails();
            FinalizarEntity();
        }
        protected void cmdAnterior_Click(object sender, EventArgs e)
        {
            grdBuscarMails.PageIndex = grdBuscarMails.PageIndex - 1;
            IniciarEntity();
            CargarVariables();
            CargarGridBuscarMails();
            FinalizarEntity();
        }
        protected void cmdPage(object sender, EventArgs e)
        {
            LinkButton cmdPage = (LinkButton)sender;
            grdBuscarMails.PageIndex = int.Parse(cmdPage.Text) - 1;
            IniciarEntity();
            CargarVariables();
            CargarGridBuscarMails();
            FinalizarEntity();
        }
        #endregion

        #region funciones
        private void limpiarCampos()
        {
            txtAsunto.Text = "";
            txtMail.Text = "";
            txtEnvioFechaDesde.Text = "";
            txtEnvioFechaHasta.Text = "";
            updPnlFiltroBuscar.Update();
            pnlCantRegistros.Visible = false;
        }
        protected void CargarVariables()
        {
            Email = txtMail.Text.TrimEnd().TrimStart();
            Asunto = txtAsunto.Text.TrimEnd().TrimStart();
            Estado = Int32.Parse(ddlEstadoMail.SelectedValue);
            //Proceso = Int32.Parse(ddlProceso.SelectedValue);
            idSolicitud = txtNroSolicitud.Text.TrimEnd().TrimStart();

            FecAltaDesde = string.IsNullOrEmpty(txtFechaAltaDesde.Text) ? DateTime.MinValue : Convert.ToDateTime(txtFechaAltaDesde.Text);
            FecAltaHasta = string.IsNullOrEmpty(txtFechaAltaHasta.Text) ? DateTime.MaxValue : Convert.ToDateTime(txtFechaAltaHasta.Text).AddDays(1).AddMilliseconds(-1);
            if (FecAltaDesde > FecAltaHasta || FecAltaDesde >= DateTime.Today.AddDays(1))
            {
                lblError.Text = "Fecha Alta Desde no puede ser mayor a Fecha Alta Hasta.";
                this.EjecutarScript(updmpeInfo, "showfrmError();");
                return;
            };

            FecDesde = string.IsNullOrEmpty(txtEnvioFechaDesde.Text) ? DateTime.MinValue : Convert.ToDateTime(txtEnvioFechaDesde.Text);
            FecHasta = string.IsNullOrEmpty(txtEnvioFechaHasta.Text) ? DateTime.MaxValue : Convert.ToDateTime(txtEnvioFechaHasta.Text).AddDays(1).AddMilliseconds(-1);
            if (FecDesde > FecHasta || FecDesde >= DateTime.Today.AddDays(1))
            {
                lblError.Text = "Fecha Desde no puede ser mayor a Fecha Hasta.";
                this.EjecutarScript(updmpeInfo, "showfrmError();");
                return;
            };
        }
        public void CargarGridBuscarMails()
        {
            db = new DGHP_Entities();
            var q = (
            from mail in db.Emails
            join proc in db.Emails_Origenes on mail.id_origen equals proc.id_origen into OrigenMail
            from or in OrigenMail.DefaultIfEmpty()
            join edo in db.Email_Estados on mail.id_estado equals edo.id_estado
            where
                mail.email.Contains(Email)
                //&& (Proceso == 99 ? true : mail.id_origen == Proceso)
                && mail.asunto.Contains(Asunto)
                && ((Estado == 99) ? true : mail.id_estado == Estado)
                && mail.fecha_alta >= FecAltaDesde && mail.fecha_alta <= FecAltaHasta
            orderby mail.id_email ascending

            select new clsItemGrillaBuscarMails()
            {
                Mail_ID = mail.id_email.ToString(),
                Mail_Estado = edo.descripcion,
                Mail_Proceso = or.descripcion,
                Mail_Asunto = mail.asunto,
                Mail_Email = mail.email,
                Mail_FechaAlta = mail.fecha_alta, 
                Mail_FechaEnvio = mail.fecha_envio
            }
            ).ToList();

            if (!String.IsNullOrWhiteSpace(idSolicitud))
            {
                q = (from m in q
                     where m.Mail_Asunto.Contains(idSolicitud.ToString()) 
                     select m).ToList();
            }
            if(FecDesde != DateTime.MinValue)
            {
                q = (from m in q
                     where m.Mail_FechaEnvio >= FecDesde
                     select m).ToList();
            }
            if (FecHasta != DateTime.MaxValue)
            {
                q = (from m in q
                     where m.Mail_FechaEnvio <= FecHasta
                     select m).ToList();
            }
            grdBuscarMails.DataSource = q;
            grdBuscarMails.DataBind();
            var lstResultados = q.ToList();
            int cantFilas = lstResultados.Count();
            pnlCantRegistros.Visible = true;

            if (cantFilas > 1)
                lblCantRegistros.Text = string.Format("{0} Registros", cantFilas);
            else if (cantFilas == 1)
                lblCantRegistros.Text = string.Format("{0} Registros", cantFilas);
            else
            {
                pnlCantRegistros.Visible = false;
            }

            updPnlResultadoBuscar.Update();
        }
        #endregion

        protected void grdBuscarMails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                {
                    /*Llamar el Mensaje Modal*/
                    LinkButton lnkDetalles = (LinkButton)e.Row.FindControl("lnkDetalles");
                    Panel pnlDetalle = (Panel)e.Row.FindControl("pnlDetalle");
                    //lnkDetalles.Attributes.Add("href", "#" + pnlDetalle.ClientID);
                    int idMail = int.Parse(lnkDetalles.CommandArgument);

                    //id_Correo.Value = idMail.ToString();
                    /*Declaro Variables para llenar los campo de la tabla dentro del modal*/
                    TableCell IDCorreo = (TableCell)e.Row.FindControl("IDCorreo");
                    TableCell Email = (TableCell)e.Row.FindControl("Email");
                    TableCell Asunto = (TableCell)e.Row.FindControl("Asunto");
                    TableCell Proceso = (TableCell)e.Row.FindControl("Proceso");
                    TableCell FecAlta = (TableCell)e.Row.FindControl("FecAlta");
                    TableCell FecEnvio = (TableCell)e.Row.FindControl("FecEnvio");
                    TableCell CantInt = (TableCell)e.Row.FindControl("CantInt");
                    TableCell Prioridad = (TableCell)e.Row.FindControl("Prioridad");
                    //TableCell CuerpoHTML = (TableCell)e.Row.FindControl("CuerpoHTML");
                    //Panel CuerpoMsj = (Panel)e.Row.FindControl("CuerpoMsj");
                    System.Web.UI.HtmlControls.HtmlContainerControl Message = (System.Web.UI.HtmlControls.HtmlContainerControl)e.Row.FindControl("Message");

                    IniciarEntity();
                    /*Query LinQ*/
                    db = new DGHP_Entities();
                    var q = (
                                from mail in db.Emails
                                join proc in db.Emails_Origenes on mail.id_origen equals proc.id_origen into OrigenMail
                                from or in OrigenMail.DefaultIfEmpty()
                                join edo in db.Email_Estados on mail.id_estado equals edo.id_estado
                                where
                                    mail.id_email == idMail

                                orderby mail.id_email ascending
                                select new clsItemGrillaBuscarMails()
                                {
                                    Mail_ID = mail.id_email.ToString(),
                                    Mail_Estado = edo.descripcion,
                                    Mail_Proceso = or.descripcion,
                                    Mail_Asunto = mail.asunto,
                                    Mail_Email = mail.email,
                                    Mail_Fecha = (mail.fecha_envio == null) ? mail.fecha_alta : mail.fecha_envio,
                                    Mail_Html = mail.html,
                                    Mail_FechaAlta = mail.fecha_alta,
                                    Mail_FechaEnvio = mail.fecha_envio,
                                    Mail_Intentos = mail.cant_intentos,
                                    Mail_Prioridad = mail.prioridad
                                }).ToList();
                    /*Para asignar valor a los campos de la tabla voy iterando por cada registro*/

                    foreach (var fila in q)
                    {
                        IDCorreo.Text = fila.Mail_ID;
                        Email.Text = fila.Mail_Email;
                        Asunto.Text = fila.Mail_Asunto;
                        Proceso.Text = fila.Mail_Proceso;
                        FecAlta.Text = fila.Mail_FechaAlta.ToString();
                        FecEnvio.Text = fila.Mail_FechaEnvio.ToString();
                        CantInt.Text = fila.Mail_Intentos.ToString();
                        Prioridad.Text = fila.Mail_Prioridad.ToString();
                        //CuerpoHTML.Text = fila.Mail_Html;
                        Message.Attributes["src"] = "~/Handlers/Mail_Handler.ashx?HtmlID=" + fila.Mail_ID;
                    }
                    FinalizarEntity();
                }
            }
        }

        protected void grdBuscarMails_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)grdBuscarMails;
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
                    // Mostrar 9 botones hacia la izquierda y 9 hacia la derecha o bien los que sea posible en caso de no llegar a 9

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
        }


        protected void lnkDetalles_Click(object sender, EventArgs e)
        {
            hfMailID.Value = (sender as LinkButton).CommandArgument;


            string panel = (sender as LinkButton).Parent.FindControl("pnlDetalle").ClientID;
            string script = string.Format("$('#{0}').modal('show');", panel);

            ScriptManager.RegisterStartupScript(updPnlBuscarMails, updPnlBuscarMails.GetType(), "IDMailPanel", script, true);

        }
    }
}