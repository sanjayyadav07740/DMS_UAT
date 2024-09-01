using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Net.NetworkInformation;
namespace DMS.Shared
{
    public partial class ViewDocumentForSearch_Website : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.UrlReferrer != null)
                {
                    ViewState["LASTPAGEURL"] = Request.UrlReferrer.ToString().ToLower();
                }

                if (Session["DocId"] != null)
                {
                    string IPAddress_Host = GetIPAddress();
                    string MAC_Address = GetMACAddress();
                    string Activity = "View";
                    DMS.BusinessLogic.Report objReport = new DMS.BusinessLogic.Report();
                   
                    objReport.InsertAuditLog_MhadaWebsite(IPAddress_Host, MAC_Address, Activity, Convert.ToInt32(Session["DocId"].ToString()));
                    //string Tagname =
                    pdfViewer.FilePath = "../Handler/PDFHandler.ashx?DocID=" + Session["DocId"].ToString() + "";

                    pdfViewer.Visible = true;
                    //pdfViewer.FilePath = "../UserControl/PDFHandler.ashx?DOCID=" + Session["DocId"].ToString() + "&DocumentName="+Session["DocName"].ToString()+"";

                }
            }
        }

        //protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        //{
        //    Response.Redirect("../MHADA Website Search/SearchMSIB.aspx",false);
        //}

        public string GetIPAddress()//gets the IP address of host(user in this case)
        {
            string Address = "";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    Address = Convert.ToString(IP);
                }
            }
            return Address;
        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        }
    }
}