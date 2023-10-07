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

namespace SGI.Operaciones.ViewLayer
{
    public partial class TestBorrar : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<EmailTemplate> emailTemplateList = GetEmailTemplateList();

                Session["emailTemplateList"] = JsonConvert.SerializeObject(emailTemplateList); 
                
                ddlTemplates.DataTextField = "name";
                ddlTemplates.DataValueField = "id";
                ddlTemplates.DataSource = emailTemplateList;
                ddlTemplates.DataBind();
            }



        }





        protected void ddlTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnGetTemplate_Click(object sender, EventArgs e)
        {
            List<EmailTemplate> emailTemplateList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmailTemplate>>(Session["emailTemplateList"].ToString());

            EmailTemplate emailTemplate = (from u in emailTemplateList
                          .Where(x => x.id == ddlTemplates.SelectedValue)
                                           select u).FirstOrDefault();


            string template = emailTemplate.html;
            hdEmailTemplateId.Value = emailTemplate.id;
            ScriptManager sm = ScriptManager.GetCurrent(this);

            ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "GetTemplate", "GetTemplate('" + template + "');", true);

            return;
        }

        protected void btnSaveTemplate_Click(object sender, EventArgs e)
        {

           
            string newTemplate = hdSaveTemplate.Value.Substring(0, hdSaveTemplate.Value.IndexOf("</div>") + 6);

            List<EmailTemplate> emailTemplateList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmailTemplate>>(Session["emailTemplateList"].ToString());

            EmailTemplate emailTemplate = (from u in emailTemplateList
                             .Where(x => x.id == hdEmailTemplateId.Value)
                                           select u).FirstOrDefault();
            emailTemplate.html = newTemplate;
            ddlTemplates.DataTextField = "name";
            ddlTemplates.DataValueField = "id";
            ddlTemplates.DataSource = emailTemplateList;
            ddlTemplates.DataBind();

            Session["emailTemplateList"] = JsonConvert.SerializeObject(emailTemplateList);
        }


        protected List<EmailTemplate> GetEmailTemplateList()
        {
            List<EmailTemplate> emailTemplateList = new List<EmailTemplate>();
            EmailTemplate emailTemplate = new EmailTemplate();
            emailTemplate.id = "0";
            emailTemplate.name = "";
            emailTemplate.html = "";
            emailTemplateList.Add(emailTemplate);

            emailTemplate = new EmailTemplate();
            emailTemplate.id = "1";
            emailTemplate.name = "Nombre 1";
            emailTemplate.html = "<div class=\"ql-editor\" contenteditable=\"true\" data-placeholder=\"Description\"><h1>Nombre 2</h1></div>";
            emailTemplateList.Add(emailTemplate);

            emailTemplate = new EmailTemplate();
            emailTemplate.id = "2";
            emailTemplate.name = "Nombre 2";
            emailTemplate.html = "<div class=\"ql-editor\" contenteditable=\"true\" data-placeholder=\"Description\"><h1>Nombre 2</h1></div>";
            emailTemplateList.Add(emailTemplate);

            return emailTemplateList;
        }
    }


    public class EmailTemplate
    {
        public string id { get; set; }
        public string name { get; set; }

        public string html { get; set; }
    }
}