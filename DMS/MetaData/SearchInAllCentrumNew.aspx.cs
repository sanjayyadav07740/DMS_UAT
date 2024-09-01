using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using DMS.BusinessLogic;
using System.Net.NetworkInformation;
using System.Net;

namespace DMS.Shared
{
    public partial class SearchInAllCentrumNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

            if (Request.UrlReferrer != null)
            {
                ViewState["LASTPAGEURL"] = Request.UrlReferrer.ToString().ToLower();

                if (ViewState["LASTPAGEURL"].ToString().Contains("viewdocumentforsearch"))
                {
                    gvDocument.Visible = true;
                    gvDocument.DataSource = Session["Result"];
                    gvDocument.DataBind();
                }
            } gvDocSearch.Visible = false;

        }

        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {
            LoadGridDataByCriteria();
            
        }

        private void LoadGridDataByCriteria()
        {
            try
            {
                //  int repository = emodModule.SelectedRepository;
                string SearchTextPan = txtSearchInAllPan.Text;
                string SearchTextCust = txtSearchInAllCust.Text;
                DataTable dtSearchResult = new DataTable();
                DataTable dtChkCentrumMaster = new DataTable();
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                if (txtSearchInAllPan.Text != "" && txtSearchInAllCust.Text==string.Empty)
                {
                    DbParameter[] objDbParameter = new DbParameter[1];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "@SearchTextPan";
                    objDbParameter[0].Value = SearchTextPan;
                    dtSearchResult = DataHelper.ExecuteDataTableForProcedure("SP_S_SearchInAllWithoutCentrumMaster", null, objDbParameter);
                    //Check if PanNo is present in the Centrum Master                  
                   
                }
                else if (txtSearchInAllCust.Text != "" && txtSearchInAllPan.Text == string.Empty)
                {
                    DbParameter[] objDbParameter = new DbParameter[1];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "@SearchTextCust";
                    objDbParameter[0].Value = SearchTextCust;
                    dtSearchResult = DataHelper.ExecuteDataTableForProcedure("SP_S_SearchInAllWithoutCentrumMaster", null, objDbParameter);
                }

                else if (txtSearchInAllCust.Text != "" && txtSearchInAllPan.Text != "")
                {
                    DbParameter[] objDbParameter = new DbParameter[2];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "@SearchTextPan";
                    objDbParameter[0].Value = SearchTextPan;

                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "@SearchTextCust";
                    objDbParameter[0].Value = SearchTextCust;
                    dtSearchResult = DataHelper.ExecuteDataTableForProcedure("SP_S_SearchInAllWithoutCentrumMaster", null, objDbParameter);
                    //Check if PanNo is present in the Centrum Master                  

                }
                
                Session["Result"] = dtSearchResult;

                gvDocument.Visible = true;
                gvDocument.DataSource = dtSearchResult;
                gvDocument.DataBind();
               
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }          

        }

        protected void gvDocSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDocSearch.PageIndex = e.NewPageIndex;
                gvDocSearch.DataSource = Session["Result"];
                gvDocSearch.DataBind();
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }

        }

        protected void gvDocSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "viewdetail")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    string pan_no = gvDocSearch.DataKeys[intRowIndex].Values["FolderName"].ToString().Trim();
                    // string Metateplatename = gvDocSearch.DataKeys[intRowIndex].Values["Branch"].ToString().Trim();//need intvalue here
                    string Department = gvDocSearch.DataKeys[intRowIndex].Values["MetatemplateName"].ToString().Trim();//need intvalue here
                    // string folder = gvDocSearch.DataKeys[intRowIndex].Values["Type_Of_Document"].ToString().Trim();//need intvalue here
                    //string[] foldername = folder.Split(' ');
                    DataTable dtDocTable = new DataTable();
                    DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                    DbParameter[] objDbParameter = new DbParameter[2];

                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "@pan_no";
                    objDbParameter[0].Value = pan_no;

                    objDbParameter[1] = objDbProviderFactory.CreateParameter();
                    objDbParameter[1].ParameterName = "@Department";
                    objDbParameter[1].Value = Department;

                    dtDocTable = DataHelper.ExecuteDataTableForProcedure("SP_S_SelectDocOnPanNoDepartment", null, objDbParameter);
                    Session["DocData"] = dtDocTable;
                    if (dtDocTable.Rows.Count == 0)
                    {
                        // UserSession.DisplayMessage(this, "Sorry, Documents are not uploded against this Pan No.", MainMasterPage.MessageType.Warning);
                        gvDocument.Visible = true;
                        gvDocument.DataSource = dtDocTable;
                        gvDocument.DataBind();
                    }
                    else
                    {
                        gvDocument.Visible = true;
                        gvDocument.DataSource = dtDocTable;
                        gvDocument.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void gvDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDocument.PageIndex = e.NewPageIndex;
                gvDocument.DataSource = Session["DocData"];
                gvDocument.DataBind();
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }

        }

        protected void gvDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "viewdetaildoc")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    int docId = Convert.ToInt32(gvDocument.DataKeys[intRowIndex].Values["ID"]);
                    string DocName = gvDocument.DataKeys[intRowIndex].Values["DocumentName"].ToString().Trim();
                    Session["DocId"] = docId;
                    //enter in audit log
                    //string strHostName = "";
                    //strHostName = System.Net.Dns.GetHostName();

                    //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                    //IPAddress[] addr = ipEntry.AddressList;
                    Report objReport = new Report();
                    string IPAddress = GetIPAddress();
                    //DateTime Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                    //DateTime Logintime = Convert.ToDateTime(DateTime.Now);
                    DateTime LoginDate = DateTime.Today;
                    string Activity = "Document Viewing";

                    string MacAddress = GetMacAddress();
                    objReport.InsertAuditLog(IPAddress, MacAddress, Activity, DocName, UserSession.UserID);

                    Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + docId, false);
                    // Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + docId, false);
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
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
    }
}