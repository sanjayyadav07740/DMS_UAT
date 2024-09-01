using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
using System.Drawing;
//using System.Web.Mail;
using System.Net.Mail;


namespace DMS.Shared
{
    public partial class Forgot_Password : System.Web.UI.Page
    {
        #region Private Member
        Utility objUtility = new Utility();
        UserManager objUserManager = new UserManager();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataTable objDataTableUserName = new DataTable();
                objUtility.Result = objUserManager.SelectUser(out objDataTableUserName, txtUserName.Text);
                if (objDataTableUserName == null || objDataTableUserName.Rows.Count == 0)
                {
                    objUtility.Result = objUserManager.SelectUserByEmailID(out objDataTableUserName, txtEmail.Text);
                }
                if (objUtility.Result == Utility.ResultType.Success)
                {
                    //MailMessage mail = new MailMessage();
                    //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    //mail.From = new MailAddress("sandip.narawade24@gmail.com","Sandip"); //you have to provide your gmail address as from address
                    //mail.To.Add("sandip.narawade24@gmail.com");
                    //mail.Subject = "Your User Name And Password";
                    //mail.Body = "User Name :" + objDataTableUserName.Rows[0]["UserName"].ToString() + " And PAssword :" + objDataTableUserName.Rows[0]["Password"].ToString();

                    //SmtpServer.Host = "10.100.5.203";
                    //SmtpServer.Credentials = new System.Net.NetworkCredential("sandip.narawade24", "sirfsandy"); //you have to provide you gamil username and password
                    //SmtpServer.EnableSsl = true;

                    //SmtpServer.Send(mail);


                    MailMessage Mail = new MailMessage();
                    MailAddress mailFrom = new MailAddress("sandip.narawade24@gmail.com", "Sandip");
                    Mail.From = mailFrom;
                    //Mail.To.Add("sandip.narawade@shcilprojects.com");
                    Mail.To.Add("rahul.walake@shcilprojects.com");
                    Mail.IsBodyHtml = true;
                    Mail.Subject = "Your User Name And Password";
                    Mail.Body = "User Name :" + objDataTableUserName.Rows[0]["UserName"].ToString() + " And PAssword :" + objDataTableUserName.Rows[0]["Password"].ToString(); 
                    SmtpClient client = new SmtpClient();
                    client.Host = "10.100.5.203";
                    client.Send(Mail);

                    txtEmail.Text = "";
                    txtUserName.Text = "";                    
                }
                else if (objUtility.Result == Utility.ResultType.Failure)
                {
                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                }
                else if (objUtility.Result == Utility.ResultType.Error)
                {
                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                }
                //Response.Redirect("../Shared/index.htm", false);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("../Shared/index.htm", false);
        }       
    }
}