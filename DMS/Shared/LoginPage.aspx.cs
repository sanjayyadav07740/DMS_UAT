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
    public partial class LoginPage : System.Web.UI.Page
    {
        # region Private Members
        UserManager objUserManager = new UserManager();
        RoleManager objRoleManager = new RoleManager();
        Utility objUtility = new Utility();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserSession.UserID != 0)
            {
               // Response.Redirect("~/Shared/NewDashBoard.aspx", false);
               // return;
            }
            txtusername.Focus();
            this.frmLogin.DefaultButton = btnlogin.UniqueID;


        }

        private int FillSessionsforSharedDocument(string strUserName )
        {

            DataTable objDataTable = new DataTable();
            objUserManager.SelectUserDetails(out objDataTable, strUserName);
            UserSession.UserID = Convert.ToInt32(objDataTable.Rows[0]["ID"]);
            UserSession.RoleID = Convert.ToInt32(objDataTable.Rows[0]["RoleID"]);
            UserSession.RoleType = Convert.ToInt32(objDataTable.Rows[0]["RoleType"] == DBNull.Value ? 0 : objDataTable.Rows[0]["RoleType"]);
            UserSession.UserPassword = Utility.Decrypt(objDataTable.Rows[0]["Password"].ToString());
            UserSession.AccessRights = AccessRights();
            return Convert.ToInt32(objDataTable.Rows[0]["UserTypeID"].ToString());
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

        private DataTable AccessRights()
        {
            DataTable objDataTable;
            objUtility.Result = objUserManager.SelectUserPermission(out objDataTable, Convert.ToInt32(UserSession.UserID));
            switch (objUtility.Result)
            {
                case Utility.ResultType.Failure:
                    objUtility.Result = objRoleManager.SelectRolePermission(out objDataTable, Convert.ToInt32(UserSession.RoleID));
                    break;
            }

            return objDataTable;
        }



        protected void btnlogin_Click(object sender, EventArgs e)
        
        {
            try
            {
                lblError.Visible = false;
                objUtility.Result = objUserManager.AuthenticateUser_ShareDocument(txtusername.Text.Trim(), Utility.Encrypt(txtpassword.Text.Trim()));
                Session["EmailID"] = txtusername.Text;
                Session["Password"] = Utility.Encrypt(txtpassword.Text.Trim());

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        {
                            DataTable objDataTable = new DataTable();
                           // objUserManager.SelectUserDetails(out objDataTable, txtusername.Text.Trim());
                            objUserManager.SelectUserDetails(out objDataTable, txtusername.Text.Trim());
                            int UserID=Convert.ToInt32(objDataTable.Rows[0]["Id"]);
                            Session["SharedUserID"]=UserID;

                            //DateTime ExpiryDate = Convert.ToDateTime(objDataTable.Rows[0]["ToDate"]);
                            //DateTime CurrentDate = DateTime.Now.Date;

                            //if (CurrentDate > ExpiryDate)
                            //{
                            //    lblError.Visible = true;
                            //    lblError.Text = "Credentials Are Expired. Please Contact Administrator.";
                            //}
                            //else
                            //{
                                Report objReport = new Report();
                                string IPAddress = GetIPAddress();
                                //DateTime Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                                //DateTime Logintime = Convert.ToDateTime(DateTime.Now);
                                DateTime LoginDate = DateTime.Today;
                                string Activity = "Login";

                                string MacAddress = GetMacAddress();
                                objReport.InsertAuditLog(IPAddress, MacAddress, Activity, "null", UserSession.UserID);
                                Log.AuditLog(HttpContext.Current, "User LoggedIn", "LoginPage");
                                Response.Redirect("~/Shared/ViewSharedDocuments.aspx", false);
                            //}
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





    }
}