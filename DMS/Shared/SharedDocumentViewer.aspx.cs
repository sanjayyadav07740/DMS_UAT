using DMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.Shared
{
    public partial class SharedDocumentViewer1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.UrlReferrer != null)
                {
                    ViewState["LASTPAGEURL"] = Request.UrlReferrer.ToString().ToLower();
                }
            }

        }

        protected void ibtnLogout_Click(object sender, ImageClickEventArgs e)
        {
            Report objReport = new Report();

            //InsertAuditLog(null);

            // objReport.UpdateAuditLog(LogoutTime, IPAddress);
            // Report objReport = new Report();
            objReport.InsertAuditLog(GetIPAddress(), GetMacAddress(), "LogOut", "null", UserSession.UserID);
            HttpContext.Current.Session.Abandon();
            Response.Redirect("../Shared/LoginPage.aspx");
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


    }
}