using ExternalService.Class;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ws_solicitudes;

namespace SGI.Operaciones.ViewLayer
{
    public partial class TestBorrar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<EmailTemplates> emailTemplateList = GetEmailTemplateList();
                ListItem emailItem = new ListItem();
                emailItem.Text = "Elija un Template";
                emailItem.Value = "0";
                ddlTemplates.Items.Add(emailItem);
                foreach (EmailTemplates emailTemplates in emailTemplateList)
                {
                    emailItem = new ListItem();
                    emailItem.Text = emailTemplates.Name;
                    emailItem.Value = emailTemplates.Id.ToString();
                    ddlTemplates.Items.Add(emailItem);
                }
            }
        }
        protected void ddlTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void btnGetTemplate_Click(object sender, EventArgs e)
        {
            if (ddlTemplates.SelectedValue == "0")
                return;
            List<EmailTemplates> emailTemplateList = GetEmailTemplateList();

            int Id = int.Parse(ddlTemplates.SelectedValue);
            EmailTemplates emailTemplate = (from u in emailTemplateList
                          .Where(x => x.Id == Id)
                                            select u).FirstOrDefault();

            string template = emailTemplate.Html;
            hdEmailTemplateId.Value = emailTemplate.Id.ToString();
            ScriptManager sm = ScriptManager.GetCurrent(this);

            ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "GetTemplate", "GetTemplate('" + template + "');", true);

            return;
        }
        protected void btnSaveTemplate_Click(object sender, EventArgs e)
        {

            ScriptManager sm = ScriptManager.GetCurrent(this);
            string cadena = "";
            string script = string.Format("alert('{0}');", cadena);
            string newTemplate = hdSaveTemplate.Value.Substring(0, hdSaveTemplate.Value.IndexOf("</div>") + 6);

            List<EmailTemplates> emailTemplateList = new List<EmailTemplates>();
            EmailTemplates emailTemplate;
            using (var db = new DGHP_Entities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        int Id = int.Parse(hdEmailTemplateId.Value);
                        emailTemplate = (from r in db.EmailTemplates
                                    .Where(x => x.Id == Id)
                                         select r).FirstOrDefault();
                        emailTemplate.Html = newTemplate;

                        db.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();

                        cadena = "Error. No se pudo Modificar el Tamplate";
                        script = string.Format("alert('{0}');", cadena);
                        ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);

                    }
                }
            }
            ddlTemplates.SelectedIndex = 0;
            cadena = "El Tamplate se Modifico Correctamente";
            script = string.Format("alert('{0}');", cadena);
            ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "alertScript", script, true);
            // Response.Redirect("~/Operaciones/ViewLayer/TemplateMailsEditor.aspx");
        }
        protected List<EmailTemplates> GetEmailTemplateList()
        {
            List<EmailTemplates> emailTemplateList = new List<EmailTemplates>();

            using (var db = new DGHP_Entities())
            {
                emailTemplateList = (from r in db.EmailTemplates
                                     select r).ToList();
            }
            return emailTemplateList;
        }
    }
}