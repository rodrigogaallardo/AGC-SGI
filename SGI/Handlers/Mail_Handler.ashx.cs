using SGI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGI.Handlers
{
    public class Mail_Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int IDMail = Convert.ToInt32(context.Request.QueryString["HtmlID"]);
            DGHP_Entities db = new DGHP_Entities();
            var q = (from mail in db.Emails where mail.id_email == IDMail select mail.html).FirstOrDefault();
            context.Response.ContentType = "text/html";
            context.Response.Write(q);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}