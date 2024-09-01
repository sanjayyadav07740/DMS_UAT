using DMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;
using System.Net.Mail;
using System.Configuration;

namespace DMS.Shared
{
    public partial class OTPAuth : System.Web.UI.Page
    {

        # region Private Members
        UserManager objUserManager = new UserManager();
        RoleManager objRoleManager = new RoleManager();
        Utility objUtility = new Utility();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {

            try
            {
                DataTable objdt = new DataTable();

                string otpnum = txtusername.Text.Trim();
                DataTable dt = (Session["UserMail"] as DataTable);
                if (!string.IsNullOrEmpty(txtusername.Text.Trim()))
                {
                    objUserManager.GetOtpDetails(Convert.ToString(dt.Rows[0]["EmailID"]), txtusername.Text.Trim(), out objdt);
                    if (objdt != null)
                    {
                        if (Convert.ToString(objdt.Rows[0]["result"]) == "1")
                        {
                            Response.Redirect("~/Shared/NewDashBoard.aspx");
                        }
                        else if (Convert.ToString(objdt.Rows[0]["result"]) == "time out")
                        {
                            lblError.Text = "OTP Time out.Please Resend the OTP";

                        }
                        else
                        {
                            lblError.Text = "Please provide correct otp number";


                        }
                    }
                    else
                    {
                        lblError.Text = "Internal Error Please Contact to Administrator";
                    }
                }
                else
                {
                    lblError.Text = "Please enter the OTP Number";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        protected void iresend_Click(object sender, EventArgs e)
        {
            try
            {


                DbTransaction objDbTransaction = BusinessLogic.Utility.GetTransaction;
                DataTable dt = (Session["UserMail"] as DataTable);
                int otpnum;
                Random rnum = new Random();
                otpnum = rnum.Next(1000, 9999);

                objUtility.Result=objUserManager.OTPDetails(Convert.ToString(dt.Rows[0]["EmailID"]), Convert.ToString(otpnum), objDbTransaction);
                if (objUtility.Result==Utility.ResultType.Success) 
                {

                    SendMail(@"Dear User,<br /><br /> Here is your OTP number " + Convert.ToString(otpnum) + " for authentication.<br> It will expire in 10 mintunes.<br><br>Thank you,<br>Team DMS </strong> <br /><br />   This is system generated mail <strong> DO NOT REPLY</strong>.", "Login OTP", "DMS Team", Convert.ToString(dt.Rows[0]["EmailID"]));
                }
                
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public bool SendMail(string body, string subject, string DisplayName, params string[] tosend)
        {
            try
            {
                MailMessage Mail = new MailMessage();
                MailAddress mailFrom = new MailAddress(ConfigurationManager.AppSettings["EmailIDFrom"].ToString(), DisplayName);
                Mail.From = mailFrom;
                foreach (string item in tosend)
                {
                    Mail.To.Add(item);
                }
                Mail.IsBodyHtml = true;
                Mail.Subject = subject;
                Mail.Body = body;
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]));
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailIDFrom"].ToString(), ConfigurationManager.AppSettings["EmailIDFromCredentials"].ToString());
                client.EnableSsl = true;
                if (Bypasscertificate == 1)
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                System.Security.Cryptography.X509Certificates.X509Chain chain,
                System.Net.Security.SslPolicyErrors sslPolicyErrors)
                    {
                        return true;
                    };
                }
                client.Send(Mail);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return false;
            }

        }
        public static int Bypasscertificate
        {
            get
            {
                return Convert.ToInt32(ConfigurationSettings.AppSettings["bypasscertificate"]);
            }
        }

        protected void icancel_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Shared/LoginForm.aspx", false);
        }

    }
}