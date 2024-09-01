using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Exchange.WebServices.Data;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using DMS.BusinessLogic;
namespace DMS.Shared
{
    public partial class SendMail : System.Web.UI.Page
    {
        string DocPath = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            
                getPath();
            
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                service.Url = new Uri("https://spl-exch01.shcilprojects.com/ews/Exchange.asmx");
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender1, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };
                service.UseDefaultCredentials = true;

                EmailMessage message = new EmailMessage(service);
                message.Subject = txtSubject.Text;
                message.Body = txtBody.Text;
                message.ToRecipients.Add(txtTo.Text);

                message.Attachments.AddFileAttachment(DocPath);
                message.SendAndSaveCopy();
                UserSession.DisplayMessage(this, "Mail sent successfully", MainMasterPage.MessageType.Success);
            }
            catch (Exception ex)
            {
                throw ex;
            }
                    
        }

        protected void getPath()
        {
            int DocId = Convert.ToInt32(Session["DocId"].ToString());
            string strQuery = "select DocumentName,DocumentPath from Document where ID='" + DocId + "'";
            DataSet ds = new DataSet();
            ds = DataHelper.ExecuteDataSet(strQuery);
            string DocName = ds.Tables[0].Rows[0]["DocumentName"].ToString();
            DocPath = ds.Tables[0].Rows[0]["DocumentPath"].ToString();
            txtAttachment.Text = DocName;
        }

        protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("../Metadata/SearchDocument.aspx");
        }
    }
}