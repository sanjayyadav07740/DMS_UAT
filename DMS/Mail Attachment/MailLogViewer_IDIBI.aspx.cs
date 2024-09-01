using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data.SqlClient;
using System.Net.Mail;

namespace DMS
{
    public partial class MailLogViewer_IDIBI : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(Utility.ConnectionString);
        private string Filepath;
        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)this.Master.FindControl("tsmManager")).RegisterPostBackControl(btnSubmit);
        }

        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {

            
            string saveFilePath = string.Empty;
            if (filAttach.HasFile)
            {
                try
                {

                    if (filAttach.PostedFile.ContentLength < 3145728)
                    {
                        if (!Directory.Exists(Server.MapPath("~/MailAttachments")))
                            Directory.CreateDirectory(Server.MapPath("~/MailAttachments"));
                        else
                        {
                            string strYear = Convert.ToString(System.DateTime.Now.Year);
                            string strMonth = Convert.ToString(System.DateTime.Now.ToString("MMM"));
                            string strDate = Convert.ToString(System.DateTime.Now.Date.Day);



                            if (!Directory.Exists(Server.MapPath("~/MailAttachments/" + strYear)))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/MailAttachments/" + strYear));
                            }
                            if (!Directory.Exists(Server.MapPath("~/MailAttachments/" + strYear + "/" + strMonth)))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/MailAttachments/" + strYear + "/" + strMonth));
                            }
                            if (!Directory.Exists(Server.MapPath("~/MailAttachments/" + strYear + "/" + strMonth + strDate)))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/MailAttachments/" + strYear + "/" + strMonth + "/" + strDate));
                            }

                            saveFilePath = Server.MapPath("~/MailAttachments/" + strYear + "/" + strMonth + "/" + strDate + "/") + Path.GetFileName(filAttach.PostedFile.FileName);
                            filAttach.PostedFile.SaveAs(saveFilePath);

                            ClientScript.RegisterClientScriptBlock(typeof(Page), "Message", "alert('Attachment Uploaded Successfully...')", true);
                            // edtMailbody.TemplateControl = string.Empty;
                            // edtMailbody.Content = string.Empty;



                            //SqlConnection con = new SqlConnection(Utility.ConnectionString);
                            //con.Open();
                            //string[] allmail = txtTo.Text.Split(';');

                            //foreach (string reciepient in allmail)
                            //{

                            //}
                            Mail_IDBI objinset = new Mail_IDBI();
                            //objinset.Mail_To = txtTo.Text+';'+txtTo.Text;
                            objinset.Mail_To = txtTo.Text;
                         
                            objinset.CC = txtCC.Text;
                            objinset.Subject = txtsubject.Text;
                            objinset.Mail_Body = edtMailbody.Content;
                            //objinset.Attachment = filAttach.PostedFile.FileName;
                            objinset.Attachment = saveFilePath;

                            objinset.CreatedOn = Convert.ToDateTime(System.DateTime.Now);
                            objinset.CretaedBy = Convert.ToInt32(Session["UserId"]);
                            objinset.IsMailSent = 0;
                            objinset.insertmail();



                            
                            txtCC.Text = string.Empty;
                            txtTo.Text = string.Empty;
                            txtsubject.Text = string.Empty;
                            edtMailbody.Content = string.Empty;
                           
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(typeof(Page), "Message", "alert('File size of 3 MB is exceeding the uploading limit.')", true);
                    }

                }

                catch (Exception ex)
                {
                    lblerror.Text = "Some problem occurred while uploading the file. Please try after some time.";
                }


            }
        }

      

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Shared/HomePage.aspx");
        }

    }
}