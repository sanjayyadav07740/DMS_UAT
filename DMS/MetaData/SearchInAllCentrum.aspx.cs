using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DMS.BusinessLogic;
using System.Data.Common;
using System.Net.NetworkInformation;
using System.Net;

namespace DMS.Shared
{
    public partial class SearchInAllCentrum : System.Web.UI.Page
    {
        #region Private Memeber
        Utility objUtility = new Utility();
        Document objdocument = new Document();
        #endregion

        
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

            }

            if (Request.UrlReferrer != null)
            {
                ViewState["LASTPAGEURL"] = Request.UrlReferrer.ToString().ToLower();

                if (ViewState["LASTPAGEURL"].ToString().Contains("viewdocumentforsearch"))
                {
                    gvDocSearch.Visible = true;
                    gvDocSearch.DataSource = Session["Result"];
                    gvDocSearch.DataBind();
                }
            }            

           //((DropDownList)emodModule.FindControl("ddlMetaTemplateName")).SelectedIndexChanged += new EventHandler(ddlMetaTemplateName_SelectedIndexChanged);
           // ((DropDownList)emodModule.FindControl("ddlRepositoryName")).SelectedIndexChanged += new EventHandler(ddlRepositoryName_SelectedIndexChanged);
        }

        //protected void ddlRepositoryName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        tdDataToSearch.Controls.Clear();
        //        trDataToSearch.Visible = false;
        //        trFromDate.Visible = false;
        //        trToDate.Visible = false;
        //        ddlField.Items.Clear();
        //        ddlField.Items.Insert(0, new ListItem("--SELECT--", "-1"));
        //    }
        //    catch (Exception ex)
        //    {
        //        UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //    }
        //}

        //protected void ddlMetaTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Utility.LoadField(ddlField, emodModule.SelectedMetaTemplate);
        //        trDataToSearch.Visible = false;
        //        trFromDate.Visible = false;
        //        trToDate.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //    }
        //}

        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {
            
            LoadGridDataByCriteria();
            gvDocument.Visible = false;
          
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
                if (txtSearchInAllPan.Text != "")
                {
                    DbParameter[] objDbParameter = new DbParameter[1];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "@SearchTextPan";
                    objDbParameter[0].Value = SearchTextPan;
                    //Check if PanNo is present in the Centrum Master                  
                    dtSearchResult = DataHelper.ExecuteDataTableForProcedure("SP_S_SelectSearchAllCentrumold", null, objDbParameter);
                }
                if (txtSearchInAllCust.Text != "")
                  {
                    DbParameter[] objDbParameter = new DbParameter[1];
                    objDbParameter[0] = objDbProviderFactory.CreateParameter();
                    objDbParameter[0].ParameterName = "@SearchTextCust";
                    objDbParameter[0].Value = SearchTextCust;
                    dtSearchResult = DataHelper.ExecuteDataTableForProcedure("SP_S_SelectSearchAllCentrumCustold", null, objDbParameter);
                }
                Session["Result"] = dtSearchResult;
                //if(dtSearchResult.Rows.Count== 0)
                //{
                //    UserSession.DisplayMessage(this, "Documents for this PAN NO/CUST NAME are not Uploaded.", MainMasterPage.MessageType.Warning);
                //}
                //else
                //{
                    gvDocSearch.DataSource = dtSearchResult;
                    gvDocSearch.DataBind();
                //}

              //  SelectSearchResult()
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
                    string pan_no = gvDocSearch.DataKeys[intRowIndex].Values["Pan_No"].ToString().Trim();
                   // string Metateplatename = gvDocSearch.DataKeys[intRowIndex].Values["Branch"].ToString().Trim();//need intvalue here
                    string Department = gvDocSearch.DataKeys[intRowIndex].Values["Department"].ToString().Trim();//need intvalue here
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

                    dtDocTable = DataHelper.ExecuteDataTableForProcedure("SP_S_SelectSearchAllCentrumDoc", null, objDbParameter);
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

        protected void gvDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if(e.CommandName.ToLower().Trim()=="viewdetaildoc")
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





       

    }
}