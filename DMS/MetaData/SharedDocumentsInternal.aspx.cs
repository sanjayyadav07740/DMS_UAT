using DMS.BusinessLogic;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.Shared
{
    public partial class SharedDocumentsInternal : System.Web.UI.Page
    {
        UserManager objUserManager = new UserManager();
        DocumentShareManager objDocumentShareManager = new DocumentShareManager();
        RoleManager objRoleManager = new RoleManager();
        Utility objUtility = new Utility();
        SqlConnection con = new SqlConnection(Utility.ConnectionString);
       
        int DocumentShareID;
        int AccessTypeID;

        protected void Page_Load(object sender, EventArgs e)
        {

            //if (Session["SharedUserID"] == null)
            //{
            //    Response.Redirect("~/Shared/LoginPage.aspx", false);
            //    return;
            //}
            //else
            //{
                //((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtn_download);
                DataTable dt = new DataTable();
                int UserID = UserSession.UserID;
                DataTable objDataTable = new DataTable();
                objUserManager.SelectUserDetailsByID(out objDataTable, UserID);
                DocumentShareID = Convert.ToInt32(objDataTable.Rows[0]["Id"]);
                AccessTypeID = Convert.ToInt32(objDataTable.Rows[0]["AccessType"]);

                con.Open();
                SqlCommand cmd = new SqlCommand("Select FolderId from document_share_details where Document_Share_Id=" + DocumentShareID, con);
                int FolderID = Convert.ToInt32(cmd.ExecuteScalar());
                Session["FolderID"] = FolderID;
                con.Close();

                if (!IsPostBack)
                {
                    if(FolderID==0)
                    {
                        DocumentBind();
                    }
                    else
                    {
                        DocumentBindForFolderID();
                    }
                    
               // }
               // UserSession.DisplayMessage(this, "Welcome "+EmailId, MainMasterPage.MessageType.Error);
            }
            if(AccessTypeID == 1)
            {
                ibtn_download.Visible = false;
                gvwDocument.Columns[7].Visible = true;
            }
            else if(AccessTypeID == 2)
            {
                ibtn_download.Visible = true;
                gvwDocument.Columns[7].Visible = false;
            }
            else
            {
                ibtn_download.Visible = true;
                gvwDocument.Columns[7].Visible = true;
            }
        }

        public void DocumentBind()
        {            
            DataTable dt = new DataTable();
            //objUtility.Result == Utility.ResultType.Success
            objUtility.Result = objDocumentShareManager.GetDocumentByDocumentShareID(out dt, DocumentShareID);

            UserSession.GridData = null;
            UserSession.GridData = dt;
            gvwDocument.DataSource = dt;
            gvwDocument.DataBind();
        }

        private void DocumentBindForFolderID()
        {
            int FolderID = Convert.ToInt32(Session["FolderID"]);            
            try
            {
                DataTable objDataTable = new DataTable();
                objUtility.Result = objDocumentShareManager.FolderWiseDocumentBind(out objDataTable, FolderID);
                if (objDataTable.Rows.Count > 0)
                {
                    UserSession.GridData = null;
                    UserSession.GridData = objDataTable;
                    gvwDocument.DataSource = objDataTable;
                    gvwDocument.DataBind();
                }
                else
                {
                    gvwDocument.DataSource = null;
                    gvwDocument.DataBind();
                    UserSession.DisplayMessage(this, "No Data to Display .", MainMasterPage.MessageType.Warning);
                }
            }
            catch (Exception ex)
            {
                //UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                Utility.SetGridHoverStyle(e);

                //if (((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 75 || ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 76)
                //    if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76)
                //    {
                //        if (e.Row.RowType == DataControlRowType.DataRow)
                //        {
                //            GridViewRow grv1 = gvwDocument.HeaderRow;
                //            CheckBox chkHeader = (CheckBox)grv1.FindControl("chkHeader");
                //            CheckBox chkChild = (e.Row.FindControl("chkChild") as CheckBox);
                //            chkHeader.Visible = true;
                //            chkChild.Visible = true;

                //        }
                //    }
                //    else
                //    {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridViewRow grv1 = gvwDocument.HeaderRow;
                    CheckBox chkHeader = (CheckBox)grv1.FindControl("chkHeader");
                    CheckBox chkChild = (e.Row.FindControl("chkChild") as CheckBox);
                    chkHeader.Visible = true;
                    chkChild.Visible = true;

                }
                //  }
                // }




                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].ColumnSpan = 6;
                    for (int i = 1; i < 7; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
               // UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "documentsearch")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    string strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["Document_ID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    string DocumentPath = gvwDocument.DataKeys[intRowIndex].Values["DocumentPath"].ToString().Trim();
                    string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();

                    Response.Redirect("../Shared/SharedDocumentViewer.aspx?DOCID=" + strDocumentID, false);
                    

                }
                if (e.CommandName.ToLower().Trim() == "documentsearchimage")
                {

                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    string strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;

                    Response.Redirect("../MetaData/ViewDocument_ImageViewer.aspx?DOCID=" + strDocumentID, false);
                }
            }
            catch (Exception ex)
            {
               // UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvwDocument.PageIndex = e.NewPageIndex;
                if (UserSession.GridData != null)
                {
                    if (UserSession.FilterData == null)
                        gvwDocument.DataSource = UserSession.GridData;
                    else
                        gvwDocument.DataSource = UserSession.FilterData;

                    gvwDocument.DataBind();
                }
                else
                {
                    DocumentBind();
                }
            }
            catch (Exception ex)
            {
             //   UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    if (UserSession.FilterData != null)
                        gvwDocument.DataSource = UserSession.SortedFilterGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    else
                        gvwDocument.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());

                    gvwDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
              //  UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnFilterGrid_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    string strFilterBy = ((DropDownList)gvwDocument.FooterRow.FindControl("ddlFilterGrid")).SelectedValue.Trim();
                    string strFilterText = ((TextBox)gvwDocument.FooterRow.FindControl("txtFilterGrid")).Text.Trim();
                    if (strFilterText == string.Empty)
                    {
                        gvwDocument.DataSource = UserSession.GridData;
                        gvwDocument.DataBind();
                        UserSession.FilterData = null;
                    }
                    else
                    {
                        DataRow[] objRows = null;

                        if (strFilterBy == "1")
                            objRows = UserSession.GridData.Select("DocumentName LIKE '%" + strFilterText + "%'");
                        else if (strFilterBy == "2")
                            objRows = UserSession.GridData.Select("Tag LIKE '%" + strFilterText + "%'");

                        if (objRows.Length > 0)
                        {
                            UserSession.FilterData = objRows.CopyToDataTable();
                            gvwDocument.DataSource = UserSession.FilterData;
                            gvwDocument.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
              //  UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
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

        protected void ibtn_download_Click(object sender, ImageClickEventArgs e)
        {
            //DownloadFiles();
           // //ibtn_download.Visible = false;

            DownloadFile();
        }

        protected void DownloadFiles()
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    //zip.AddDirectoryByName("Files");
                    List<string> lstfile = new List<string>();
                    int counter = 1;
                    foreach (GridViewRow row in gvwDocument.Rows)
                    {
                        CheckBox chk = (CheckBox)(row.FindControl("chkRow"));
                        //if ((row.FindControl("checked") as CheckBox).Checked)
                        if (chk.Checked)
                        {
                            string inputpath = Utility.DocumentPath;
                            string outputpath = Utility.DocumentPath;
                            if (!Directory.Exists(outputpath))
                            {
                                Directory.CreateDirectory(outputpath);
                            }
                            //string FilePath = objDataTable.Rows[0]["DocumentPath"].ToString();
                            string filePath = (string)gvwDocument.DataKeys[row.RowIndex].Values["DocumentPath"];
                            FileInfo objFile = new FileInfo(filePath);

                            if (!File.Exists(outputpath + objFile.Name) && objFile.Extension.ToLower().Contains("pdf"))
                            {

                            }
                            else
                            {
                                if (lstfile.IndexOf((string)gvwDocument.DataKeys[row.RowIndex].Values["DocumentName"]) >= 0)
                                {

                                    FileInfo obj = new FileInfo((string)gvwDocument.DataKeys[row.RowIndex].Values["DocumentName"]);
                                    string filename = Path.GetFileNameWithoutExtension((string)gvwDocument.DataKeys[row.RowIndex].Values["DocumentName"]);
                                    filename += "_" + counter;
                                    counter++;
                                    zip.AddFile(outputpath + objFile.Name).FileName = filename + Path.GetExtension((string)gvwDocument.DataKeys[row.RowIndex].Values["DocumentName"]);
                                    lstfile.Add(filename + Path.GetExtension((string)gvwDocument.DataKeys[row.RowIndex].Values["DocumentName"]));
                                }
                                else
                                {

                                    zip.AddFile(outputpath + objFile.Name).FileName = (string)gvwDocument.DataKeys[row.RowIndex].Values["DocumentName"];
                                    lstfile.Add((string)gvwDocument.DataKeys[row.RowIndex].Values["DocumentName"]);
                                }

                            }
                        }
                        else
                        {

                        }
                    }
                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();
                }

            }
            catch (Exception ex)
            {
                //UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }
        }

        public void DownloadFile()
        {
            int Id = 0;
            DataTable objTable = new DataTable();
            string Filepath = string.Empty;

            string FileName = string.Empty;
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                foreach (GridViewRow row in gvwDocument.Rows)
                {
                    if ((row.FindControl("chkRow") as CheckBox).Checked)
                    {
                        Id = (int)gvwDocument.DataKeys[row.RowIndex].Values["Document_ID"];
                        objTable = DataHelper.ExecuteDataTable("SELECT DocumentName,DocumentPath,PageCount=ISNULL(PageCount,'')+ ' ' + ISNULL(MergedPageCount,'') FROM vwDocument WHERE ID = " + Id, null);
                        Filepath = objTable.Rows[0]["DocumentPath"].ToString();
                        FileName = objTable.Rows[0]["DocumentName"].ToString();
                        zip.AddFile(Filepath).FileName = FileName;

                    }
                }
                if (zip.Count <= 0)
                {
                    return;
                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }
        }

    }
}