using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;
using System.Web.Security;
using System.Net;
using System.Net.NetworkInformation;
namespace DMS
{
    public partial class MainMasterPage : System.Web.UI.MasterPage
    {
        #region Private Members
        UserManager objUserManager = new UserManager();
        RoleManager objRoleManager = new RoleManager();
        Utility objUtility = new Utility();
        #endregion

        #region Enum
        public enum MessageType { Success, Warning, Error };
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserSession.UserID == 0)
            {
                Response.Redirect("~/Shared/LoginForm.aspx");
                return;
            }
            if (!IsPostBack)
            {
                DataTable objDataTable;
                objUtility.Result = objUserManager.SelectUser(out objDataTable, UserSession.UserID);
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        lblUser.Text = objDataTable.Rows[0]["FirstName"].ToString() + " " + objDataTable.Rows[0]["LastName"].ToString() + "<br/>[" + objDataTable.Rows[0]["DisplayName"].ToString() + "]";
                        break;
                    case Utility.ResultType.Failure:
                        DisplayMessage("No Data To Display .", MainMasterPage.MessageType.Warning);
                        break;

                    case Utility.ResultType.Error:
                        DisplayMessage("Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;

                }

                if (UserSession.LastLogin == null)
                {
                    lblDateTime.Text = objUserManager.SelectLastLogin(new LoginDetail() { UserID = UserSession.UserID });
                    UserSession.LastLogin = lblDateTime.Text;
                }
                else
                {
                   lblDateTime.Text = UserSession.LastLogin;
                }

                BindMenu();

            }
            if (Request.UrlReferrer == null)
            {
                Session.Abandon();
                Response.Redirect("~/Shared/LoginForm.aspx");
            }
        }

        //protected void ibtnLogout_Click(object sender, EventArgs e)
        //{

        //}

        protected void ibtnDashBoard_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Shared/DashBoard.aspx");
        }
        #endregion

        #region Method

        public void DisplayMessage(string strMessage, MessageType enumMessageType)
        {
            try
            {
                lblMessage.Visible = true;
                if (enumMessageType == MessageType.Success)
                {
                    lblMessage.Text = strMessage;
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                }
                else if (enumMessageType == MessageType.Warning)
                {
                    lblMessage.Text = strMessage;
                    lblMessage.ForeColor = System.Drawing.Color.Blue;
                }
                else if (enumMessageType == MessageType.Error)
                {
                    lblMessage.Text = strMessage;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
                mpopMessageExtender.Show();
                btnOk.Focus();
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        private void BindMenu()
        {
            try
            {
                DataTable objDataTable = new DataTable();

                if (UserSession.Menu == null)
                {
                    objUtility.Result = objUserManager.SelectUserModule(out objDataTable, Convert.ToInt32(UserSession.UserID));
                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Failure:
                            objDataTable = null;
                            objUtility.Result = objRoleManager.SelectRoleModule(out objDataTable, Convert.ToInt32(UserSession.RoleID), Utility.Control.Menu);
                            break;
                    }
                }

                else
                {
                    objDataTable = UserSession.Menu;
                }


                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        UserSession.Menu = objDataTable;
                        DataView objDataView = new DataView(objDataTable);
                        objDataView.RowFilter = "ParentModuleID=0";
                        objDataView.Sort = "DisplayOrder";

                        foreach (DataRowView objDataRow in objDataView)
                        {
                            MenuItem objMainMenuItem = new MenuItem(objDataRow["ModuleName"].ToString(), objDataRow["ID"].ToString(), null, objDataRow["NavigationUrl"].ToString());
                            LoadMenu(objDataTable, Convert.ToInt32(objDataRow["ID"].ToString()), objMainMenuItem);
                            mnuMain.Items.Add(objMainMenuItem);

                        }
                        break;
                    case Utility.ResultType.Failure:
                        DisplayMessage("No Data To Display .", MainMasterPage.MessageType.Warning);
                        break;

                    case Utility.ResultType.Error:
                        DisplayMessage("Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }

            }
            catch (Exception ex)
            {
                DisplayMessage("Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }
        }

        private void LoadMenu(DataTable objDataTable, int intMenuID, MenuItem objMainMenuItem)
        {
            try
            {
                DataView objDataView = new DataView(objDataTable);
                objDataView.RowFilter = "ParentModuleID=" + intMenuID;
                objDataView.Sort = "DisplayOrder";

                foreach (DataRowView objDataRow in objDataView)
                {
                    MenuItem objSubMenuItem = new MenuItem(objDataRow["ModuleName"].ToString(), objDataRow["ID"].ToString(), null, objDataRow["NavigationUrl"].ToString());
                    LoadMenu(objDataTable, Convert.ToInt32(objDataRow["ID"].ToString()), objSubMenuItem);
                    objMainMenuItem.ChildItems.Add(objSubMenuItem);

                }
            }
            catch (Exception ex)
            {
                DisplayMessage("Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }

        }

        #endregion

        protected void ibtnHome_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ibtnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Report objReport = new Report();

            //InsertAuditLog(null);

            // objReport.UpdateAuditLog(LogoutTime, IPAddress);
            // Report objReport = new Report();
            objReport.InsertAuditLog(GetIPAddress(), GetMacAddress(), "LogOut", "null", UserSession.UserID);
            HttpContext.Current.Session.Abandon();
            Response.Redirect("../Shared/LoginForm.aspx");
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

        public void InsertAuditLog(string DocumentName)
        {
            //insert audit log
            string IPAddress = GetIPAddress();
            string MacAddress = GetMacAddress();
            string Activity = "Logout";
            string DocName = DocumentName;
            int UserId = UserSession.UserID;
            Report objReport = new Report();
            objReport.InsertAuditLog(IPAddress, MacAddress, Activity, DocName, UserId);
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

        //protected void ddlmenu_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlmenu.SelectedItem.Text == "Dashboard")
        //    {
        //        Response.Redirect("../Shared/DashBoard.aspx");

        //    }
        //    else if (ddlmenu.SelectedItem.Text == "nLogout")
        //    {
        //        Report objReport = new Report();

        //        //InsertAuditLog(null);

        //        // objReport.UpdateAuditLog(LogoutTime, IPAddress);
        //        // Report objReport = new Report();
        //        objReport.InsertAuditLog(GetIPAddress(), GetMacAddress(), "LogOut", "null", UserSession.UserID);
        //        HttpContext.Current.Session.Abandon();
        //        Response.Redirect("../Shared/LoginForm.aspx");
        //    }
        //    else if (ddlmenu.SelectedItem.Text == "ChangePassword")
        //    {
        //        Response.Redirect("../Shared/ChangePassword.aspx");
            
        //    }

        //}
    }
}