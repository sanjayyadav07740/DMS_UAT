using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Web.Security;
using System.Data;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Net.NetworkInformation;
using System.Configuration;
namespace DMS.Shared
{
    public partial class LoginForm_Old : System.Web.UI.Page
    {

        # region Private Members
        UserManager objUserManager = new UserManager();
        RoleManager objRoleManager = new RoleManager();
        Utility objUtility = new Utility();
        #endregion        

        # region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserSession.UserID != 0)
            {
                Response.Redirect("~/Shared/HomePage.aspx", false);
                return;
            }
            txtUserName.Focus();
            this.frmLogin.DefaultButton = ibtnLogin.UniqueID;
            if(!IsPostBack)
            {
                GenerateVirtualKeyBoard();
            }
          
            txtCaptcha.Text = string.Empty;
            string strCaptchaCode = GetCaptchaCode();
            imgCaptcha.ImageUrl = "../Handler/CaptchaHandler.ashx?Code=" + strCaptchaCode;
            cpvCaptchaCode.ValueToCompare = strCaptchaCode;
        }
       // string IPAddress = GetIPAddress();
        public string GetIPAddress()
        {
            string IPAddress = "";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;

        }

        protected void ibtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Visible = false;
                objUtility.Result = objUserManager.AuthenticateUser(txtUserName.Text.Trim(), Utility.Encrypt(txtPassword.Text.Trim()));
                Session["UserName"] = txtUserName.Text;
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        {
                            int intUserTypeID = FillSessions(txtUserName.Text.Trim());
                            if (Utility.EnabledNamedConcurrentUser == true)
                            {
                                if (((DataTable)Application["LoginUsers"]).Select("UserID=" + UserSession.UserID).Length > 0)
                                {
                                    if (Convert.ToInt32(((DataTable)Application["LoginUsers"]).Select("UserID=" + UserSession.UserID)[0]["TotalLoginCount"]) >= Convert.ToInt32(((DataTable)Application["LoginUsers"]).Select("UserID=" + UserSession.UserID)[0]["MaximumLoginCount"]))
                                    {
                                        lblError.Text = "User Login Limit Already Exceed.";
                                        lblError.Visible = true;
                                        UserSession.UserID = 0;
                                        return;
                                    }
                                    else
                                    {
                                        ((DataTable)Application["LoginUsers"]).Select("UserID=" + UserSession.UserID)[0]["TotalLoginCount"] = Convert.ToInt32(((DataTable)Application["LoginUsers"]).Select("UserID=" + UserSession.UserID)[0]["TotalLoginCount"]) + 1;
                                        ((DataTable)Application["LoginUsers"]).AcceptChanges();
                                    }
                                }
                                else
                                {
                                    ((DataTable)Application["LoginUsers"]).Rows.Add(UserSession.UserID, DateTime.Now, 1, intUserTypeID == 0 ? Utility.MaxNamedUser : Utility.MaxConcurrentUser);
                                    ((DataTable)Application["LoginUsers"]).AcceptChanges();
                                }
                            }

                            LoginDetail objLoginDetail = new LoginDetail();
                            objLoginDetail.UserID = UserSession.UserID;
                            objLoginDetail.LoginTime = DateTime.Now;
                            objLoginDetail.Remark = "Logged In";
                            objUserManager.InsertLoginDetail(objLoginDetail);
                            UserSession.LoginDetailID = objLoginDetail.LoginDetailID;
                            //string strHostName = "";
                            //strHostName = System.Net.Dns.GetHostName();

                            //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                            //IPAddress[] addr = ipEntry.AddressList;
                            Report objReport = new Report();
                            string IPAddress = GetIPAddress();
                            //DateTime Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                            //DateTime Logintime = Convert.ToDateTime(DateTime.Now);
                            DateTime LoginDate = DateTime.Today;
                            string Activity = "Login";

                            string MacAddress = GetMacAddress();
                            objReport.InsertAuditLog(IPAddress, MacAddress, Activity, "null", UserSession.UserID);
                           Response.Redirect("~/Shared/HomePage.aspx", false);
                         

                           //MessageBox.Show(addr[addr.Length - 1].ToString());
                        }
                        break;
                    case Utility.ResultType.Failure:
                        lblError.Text = "Invalid username or password.";
                        lblError.Visible = true;
                        break;
                    case Utility.ResultType.Error:
                        lblError.Text = "Sorry ,Some Error Has Been Occured .";
                        lblError.Visible = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                lblError.Text = "Sorry ,Some Error Has Been Occured .";
                lblError.Visible = true;
            }
        }
        public static string GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress().ToString();
                }
            }
            return null;
        }
        protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("../Shared/LoginForm.aspx");
        }

        protected void ibtnRefresh_Click(object sender, ImageClickEventArgs e)
        {
            txtCaptcha.Text = string.Empty;
            txtCaptcha.Focus();
            string strCaptchaCode = GetCaptchaCode();
            imgCaptcha.ImageUrl = "../Handler/CaptchaHandler.ashx?Code=" + strCaptchaCode;
            cpvCaptchaCode.ValueToCompare = strCaptchaCode;
        }

        private void SendMail(string UserName,string Password,string EmailId)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["SmtpServerPort"]));
                mail.To.Add(EmailId);
                mail.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["EmailIDFrom"].ToString());
                mail.Bcc.Add(ConfigurationManager.AppSettings["EmailIDFrom"].ToString());
                //mail.Subject = "Verification Of Email by WWW.AUCTIONPACL.COM";
                mail.Subject = "EDMS UserName and Password";
                mail.Body = generatRegConfirmMailBody(UserName,Password);
                mail.IsBodyHtml = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailIDFrom"].ToString(), ConfigurationManager.AppSettings["EmailIDFromCredentials"].ToString());
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

                UserSession.DisplayMessage(this, "Mail has been send on your registered mail Id!", MainMasterPage.MessageType.Success);                  
                Response.AppendHeader("Refresh", "10;url=~/Shared/Index.aspx");

               // ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "alert('Mail has been send on your registered mail Id ! Kindly click the link in the mail to facilitate Payment !!');window.location.assign('../Shared/Index.aspx')", true);

            }
            catch (Exception ex)
            {
                string str = ex.Message;
                UserSession.DisplayMessage(this, str, MainMasterPage.MessageType.Error);
                return;
            }
        }

        private string generatRegConfirmMailBody(string UserName,string Password)
        {
            //string link = ConfigurationManager.AppSettings["App_URL"].ToString() + "Metadata/VerifyBidder.aspx?ID=" + DMS.BusinessLogic.Entity.GlobalUser.ToEncrypt(Convert.ToString(ID));
            string Body_Text = @"User Name :" +UserName  + " And Password :" +Password ;

            return Body_Text;
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

               
               
                ////if (objDataTableUserName == null || objDataTableUserName.Rows.Count == 0 && txtEmail.Text.Trim() != "")
                ////{
                ////    objUtility.Result = objUserManager.SelectUserByEmailID(out objDataTableUserName, txtEmail.Text);
                ////}
                //if (objUtility.Result == Utility.ResultType.Success)
                if(txtEmail.Text!="")
                {
                    //if(objDataTableUserName.Rows[0]["EmailID"].ToString()=="")
                    //{
                    //}
                    //else
                    //{
                    DataTable objDataTableUserName = new DataTable();
                    objUtility.Result = objUserManager.SelectUserByEmailID(out objDataTableUserName, txtEmail.Text);
                        SendMail(objDataTableUserName.Rows[0]["UserName"].ToString(), objDataTableUserName.Rows[0]["Password"].ToString(), objDataTableUserName.Rows[0]["EmailID"].ToString());
                    //}
                    //MailMessage Mail = new MailMessage();
                    //MailAddress mailFrom = new MailAddress("sandip.narawade24@gmail.com", "Sandip");
                    //Mail.From = mailFrom;
                    ////Mail.To.Add("sandip.narawade@shcilprojects.com");
                    //Mail.To.Add("ajay.singh_v@shcilprojects.com");
                    //Mail.IsBodyHtml = true;
                    //Mail.Subject = "Your User Name And Password";
                    //Mail.Body = "User Name :" + objDataTableUserName.Rows[0]["UserName"].ToString() + " And Password :" + objDataTableUserName.Rows[0]["Password"].ToString();
                    //SmtpClient client = new SmtpClient();
                    //client.Host = "10.100.5.203";
                    //client.Send(Mail);
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
            finally
            {
                if (objUtility.Result == Utility.ResultType.Failure)
                {
                    lblError.Text = "No Data To Display...";
                }
                else if (objUtility.Result == Utility.ResultType.Error)
                {
                    lblError.Text = "Sorry ,Some Error Has Been Occured ....";
                }
                txtEmail.Text = "";
                txtUserName.Text = "";
                mpopForgotPassword.Hide();
            }

        }       
        #endregion

        # region Methods

        private int FillSessions(string strUserName)
        {
            
            DataTable objDataTable = new DataTable();
            objUserManager.SelectUser(out objDataTable,strUserName);
            UserSession.UserID = Convert.ToInt32(objDataTable.Rows[0]["ID"]);
            UserSession.RoleID = Convert.ToInt32(objDataTable.Rows[0]["RoleID"]);
            UserSession.RoleType = Convert.ToInt32(objDataTable.Rows[0]["RoleType"] == DBNull.Value ? 0 : objDataTable.Rows[0]["RoleType"]);
            UserSession.UserPassword = Utility.Decrypt(objDataTable.Rows[0]["Password"].ToString());
            UserSession.AccessRights = AccessRights();
            return Convert.ToInt32(objDataTable.Rows[0]["UserTypeID"].ToString());
        }

        private DataTable AccessRights()
        {
                DataTable objDataTable;
                objUtility.Result = objUserManager.SelectUserPermission(out objDataTable, Convert.ToInt32(UserSession.UserID));
                    switch (objUtility.Result)
                    {       case Utility.ResultType.Failure:
                            objUtility.Result = objRoleManager.SelectRolePermission(out objDataTable, Convert.ToInt32(UserSession.RoleID));
                            break;
                    }

               return objDataTable;
         }

        private string GetCaptchaCode()
        {
            Random objRandom = new Random();
            string[] strValues = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            
            StringBuilder objCaptchaCode = new StringBuilder(4);
            for (int i = 0; i < 4; i++)
            {
                objCaptchaCode.Append(strValues[objRandom.Next(1, strValues.Length)]);
            }
            return objCaptchaCode.ToString().Trim();
        }

        private void GenerateVirtualKeyBoard()
        {
            StringBuilder objStringBuilder = new StringBuilder();
            string strStyle = " STYLE='BORDER-WIDTH:0px;margin:0PX 0PX 0PX 0PX;padding:0PX 0PX 0PX 0PX;'";
            string strButton = "<input type=\"button\" value=\"{0}\" style=\"WIDTH:20px;HEIGHT:20px;background-repeat:no-repeat;border-width:0px;border-style:none;FONT-SIZE: 13px;background-image:url('../Images/virtualkey-button.png');\" id=\"btnVirKey\" onclick=\"func_click(this.value);\"/>";
            objStringBuilder.Append("<Table " + strStyle + ">");

            objStringBuilder.Append("<Tr " + strStyle + ">");
            objStringBuilder.Append("<Td align='center' " + strStyle + ">");
            objStringBuilder.Append("<Table " + strStyle + ">");
            objStringBuilder.Append("<Tr " + strStyle + ">");
            for (int i = 65; i < 78; i++)
            {
                objStringBuilder.Append("<Td " + strStyle + ">");
                objStringBuilder.Append(string.Format(strButton, Convert.ToChar(i)));
                objStringBuilder.Append("</Td>");
            }
            objStringBuilder.Append("</Tr>");
            objStringBuilder.Append("</Table>");
            objStringBuilder.Append("</Td>");
            objStringBuilder.Append("</Tr>");

            objStringBuilder.Append("<Tr " + strStyle + ">");
            objStringBuilder.Append("<Td align='center' " + strStyle + ">");
            objStringBuilder.Append("<Table " + strStyle + ">");
            objStringBuilder.Append("<Tr " + strStyle + ">");
            for (int i = 78; i < 91; i++)
            {
                objStringBuilder.Append("<Td " + strStyle + ">");
                objStringBuilder.Append(string.Format(strButton, Convert.ToChar(i)));
                objStringBuilder.Append("</Td>");
            }
            objStringBuilder.Append("</Tr>");
            objStringBuilder.Append("</Table>");
            objStringBuilder.Append("</Td>");
            objStringBuilder.Append("</Tr>");


            objStringBuilder.Append("<Tr " + strStyle + ">");
            objStringBuilder.Append("<Td align='center'" + strStyle + ">");
            objStringBuilder.Append("<Table" + strStyle + ">");
            objStringBuilder.Append("<Tr " + strStyle + ">");
            for (int i = 0; i <= 9; i++)
            {
                objStringBuilder.Append("<Td " + strStyle + ">");
                objStringBuilder.Append(string.Format(strButton, i.ToString()));
                objStringBuilder.Append("</Td>");
            }
            objStringBuilder.Append("</Tr>");
            objStringBuilder.Append("</Table>");
            objStringBuilder.Append("</Td>");
            objStringBuilder.Append("</Tr>");


            string[] strSpecialCharacter = new string[] { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "-", "+", "=", "~", "`", "&quot;", ":", ";", "'", ",", ".", "/", "<", ">", "?", "{", "}", "[", "]", "|", "\\" };

            objStringBuilder.Append("<Tr " + strStyle + ">");
            objStringBuilder.Append("<Td align='center'" + strStyle + ">");
            objStringBuilder.Append("<Table" + strStyle + ">");
            objStringBuilder.Append("<Tr " + strStyle + ">");
            for (int i = 0; i <= 10; i++)
            {
                objStringBuilder.Append("<Td " + strStyle + ">");
                objStringBuilder.Append(string.Format(strButton, strSpecialCharacter[i]));
                objStringBuilder.Append("</Td>");
            }
            objStringBuilder.Append("</Tr>");
            objStringBuilder.Append("</Table>");
            objStringBuilder.Append("</Td>");
            objStringBuilder.Append("</Tr>");

            objStringBuilder.Append("<Tr " + strStyle + ">");
            objStringBuilder.Append("<Td align='center'" + strStyle + ">");
            objStringBuilder.Append("<Table" + strStyle + ">");
            objStringBuilder.Append("<Tr " + strStyle + ">");
            for (int i = 11; i <= 21; i++)
            {
                objStringBuilder.Append("<Td " + strStyle + ">");
                objStringBuilder.Append(string.Format(strButton, strSpecialCharacter[i]));
                objStringBuilder.Append("</Td>");
            }
            objStringBuilder.Append("</Tr>");
            objStringBuilder.Append("</Table>");
            objStringBuilder.Append("</Td>");
            objStringBuilder.Append("</Tr>");

            objStringBuilder.Append("<Tr " + strStyle + ">");
            objStringBuilder.Append("<Td align='center'" + strStyle + ">");
            objStringBuilder.Append("<Table" + strStyle + ">");
            objStringBuilder.Append("<Tr " + strStyle + ">");
            for (int i = 22; i <= 31; i++)
            {
                objStringBuilder.Append("<Td " + strStyle + ">");
                objStringBuilder.Append(string.Format(strButton, strSpecialCharacter[i]));
                objStringBuilder.Append("</Td>");
            }
            objStringBuilder.Append("</Tr>");
            objStringBuilder.Append("</Table>");
            objStringBuilder.Append("</Td>");
            objStringBuilder.Append("</Tr>");

            string strClearButton = "<input type=\"button\" value=\"Clear\" style=\"WIDTH:80PX;HEIGHT:27px;background-repeat:no-repeat;border-width:0px;border-style:none;FONT-SIZE: 13px;background-image:url('../Images/virtual-big-but.png');\" onclick=\"func_clear();\"/>";
            string strBackSpaceButton = "<input type=\"button\" value=\"BackSpace\" style=\"WIDTH:80PX;HEIGHT:27px;background-repeat:no-repeat;border-width:0px;border-style:none;FONT-SIZE: 13px;background-image:url('../Images/virtual-big-but.png');\" onclick=\"func_backspace();\"/>";
            string strSpaceButton = "<input type=\"button\" value=\"Space\" style=\"WIDTH:80PX;HEIGHT:27px;background-repeat:no-repeat;border-width:0px;border-style:none;FONT-SIZE: 13px;background-image:url('../Images/virtual-big-but.png');\" onclick=\"func_click(' ');\"/>";
            string strCapsLockButton = "<input type=\"button\" value=\"CapsLock\" style=\"WIDTH:80PX;HEIGHT:27px;background-repeat:no-repeat;border-width:0px;border-style:none;FONT-SIZE: 13px;background-image:url('../Images/virtual-big-but.png');\" onclick=\"func_capslocktoggle();\"/>";
            
            objStringBuilder.Append("<Tr " + strStyle + ">");
            objStringBuilder.Append("<Td align='center'" + strStyle + ">");
            objStringBuilder.Append("<Table" + strStyle + ">");
            objStringBuilder.Append("<Tr " + strStyle + ">");
            objStringBuilder.Append(string.Format("<Td >{0}</Td>", strClearButton));
            objStringBuilder.Append(string.Format("<Td >{0}</Td>", strBackSpaceButton));
            objStringBuilder.Append(string.Format("<Td >{0}</Td>", strSpaceButton));
            objStringBuilder.Append(string.Format("<Td >{0}</Td>", strCapsLockButton));
            objStringBuilder.Append("</Tr>");
            objStringBuilder.Append("</Table>");
            objStringBuilder.Append("</Td>");
            objStringBuilder.Append("</Tr>");

            objStringBuilder.Append("</Table>");
            divVirtualKeyBoard.InnerHtml = objStringBuilder.ToString();

        }
        #endregion
    }
}
