using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Common;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Ionic.Zip;
using System.Configuration;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace DMS.Shared
{
    public partial class SearchDocument : System.Web.UI.Page
    {
        #region Private Memeber
        Utility objUtility = new Utility();
        Document objdocument = new Document();
        DocumentManager objDocumentManager = new DocumentManager();
        SqlConnection con = new SqlConnection(Utility.ConnectionString);
        string SelectedFieldValue = "SelectedFieldValue";
        #endregion

        #region Properties
        public string FieldValue
        {
            get
            {
                if (Session[SelectedFieldValue] != null)
                    return Session[SelectedFieldValue].ToString();
                else
                    return null;
            }
            set
            {
                Session[SelectedFieldValue] = value;
            }
        }
        int outputValue;
        string strDocumentID;

        private string TIFF_CODEC = "image/tiff";
        private long ENCODING_SCHEME = (long)EncoderValue.CompressionCCITT4;
        public string SelectedPages { get; set; }
        public int NumberOfPages { get; set; }
        public string FilePath { get; set; }
        public bool newfile = true;
        public bool newdocument = true;
        //System.Drawing.Image Document;
        EncoderParameters encoderParams;
        public int SelectedPageCount { get { return Pages.Length; } }

        public int[] Pages
        {
            get
            {
                ArrayList ps = new ArrayList();
                string[] ss = SelectedPages.Split(new char[] { ',', ' ', ';' });
                foreach (string s in ss)
                    if (Regex.IsMatch(s, @"\d+-\d+"))
                    {
                        int start = int.Parse(s.Split(new char[] { '-' })[0]);
                        int end = int.Parse(s.Split(new char[] { '-' })[1]);
                        if (start > end)
                            return new int[] { 0 };
                        while (start <= end)
                        {
                            ps.Add(start - 1);
                            start++;
                        }
                    }
                    else
                    {
                        int i;
                        int.TryParse(s, out i);
                        if (i > 0)
                            ps.Add(int.Parse(s) - 1);
                    }


                return ps.ToArray(typeof(int)) as int[];
            }
        }

        private ImageCodecInfo GetCodecInfo(string codec)
        {
            ImageCodecInfo codecInfo = null;
            foreach (ImageCodecInfo info in ImageCodecInfo.GetImageEncoders())
            {
                if (info.MimeType == codec)
                {
                    codecInfo = info;
                    break;
                }
            }
            return codecInfo;
        }

        #endregion

        #region Events
        protected override void OnInitComplete(EventArgs e)
        {
            if (!IsPostBack)
            {
                UserSession.FilterData = null;
                UserSession.MetaDataID = 0;
                FieldValue = null;

                Log.AuditLog(HttpContext.Current, "Visit", "SearchDocument");
            }

            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnMerge);
            if (FieldValue != null)
            {
                if (FieldValue.Split('-')[1].Trim() != "3")
                {
                    trDataToSearch.Visible = true;
                    tdDataToSearch.Controls.Clear();
                    Utility.GenerateControlForSearch(tdDataToSearch, FieldValue);
                    trFromDate.Visible = false;
                    trToDate.Visible = false;
                }
                else
                {
                    trDataToSearch.Visible = false;
                    trFromDate.Visible = true;
                    trToDate.Visible = true;
                }
            }
            base.OnInitComplete(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnDownloadFiles);
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnSplit);
            //((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(gvwDocument);


            int Id = UserSession.UserID;
            int RId = UserSession.RoleID;
            DataTable dtUData;
            objUtility.Result = objDocumentManager.GetUserData(out dtUData, Id);

            if (!IsPostBack)
            {
                if (Directory.Exists(Server.MapPath("../PDFVIEWERRAW/")))
                {
                    string[] filePaths = Directory.GetFiles(Server.MapPath("../PDFVIEWERRAW/"));
                    foreach (string filePath in filePaths)
                        File.Delete(filePath);
                }
                UserSession.MetaDataID = 0;
                FieldValue = null;
                ddlField.Items.Insert(0, new ListItem("--SELECT--", "-1"));

                try
                {
                    if (UserControl.EntityModule.GetPropertiesValue("SelectedMetaTemplate") != "-1")
                        Utility.LoadField(ddlField, Convert.ToInt32(UserControl.EntityModule.GetPropertiesValue("SelectedMetaTemplate")));
                }
                catch { }

                int intIsViewed = Convert.ToInt32(dtUData.Rows[0]["IsViewed"]);
                if (intIsViewed == 1)
                {
                    gvwDocument.Columns[9].Visible = true;
                }
                else
                {
                    gvwDocument.Columns[9].Visible = false;
                }

                int intIsEdit = Convert.ToInt32(dtUData.Rows[0]["IsEdit"]);
                if (intIsEdit == 1)
                {
                    gvwDocument.Columns[11].Visible = true;
                }
                else
                {
                    gvwDocument.Columns[11].Visible = false;
                }

                int intIsMerge = Convert.ToInt32(dtUData.Rows[0]["IsMerge"]);
                if (intIsMerge == 1)
                {
                    gvwDocument.Columns[12].Visible = true;
                }
                else
                {
                    gvwDocument.Columns[12].Visible = false;
                }

                int intIsSplit = Convert.ToInt32(dtUData.Rows[0]["IsSplit"]);
                if (intIsSplit == 1)
                {
                    gvwDocument.Columns[13].Visible = true;
                }
                else
                {
                    gvwDocument.Columns[13].Visible = false;
                }

                int intIsDelete = Convert.ToInt32(dtUData.Rows[0]["IsDelete"]);
                if (intIsDelete == 1)
                {
                    gvwDocument.Columns[14].Visible = true;
                }
                else
                {
                    gvwDocument.Columns[14].Visible = false;
                }

                LoadGridDataFromTemporaryList();
            }
            ((DropDownList)emodModule.FindControl("ddlMetaTemplateName")).SelectedIndexChanged += new EventHandler(ddlMetaTemplateName_SelectedIndexChanged);
            ((DropDownList)emodModule.FindControl("ddlRepositoryName")).SelectedIndexChanged += new EventHandler(ddlRepositoryName_SelectedIndexChanged);
        }

        protected void ddlRepositoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tdDataToSearch.Controls.Clear();
                trDataToSearch.Visible = false;
                trFromDate.Visible = false;
                trToDate.Visible = false;
                ddlField.Items.Clear();
                ddlField.Items.Insert(0, new ListItem("--SELECT--", "-1"));
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ddlMetaTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Utility.LoadField(ddlField, emodModule.SelectedMetaTemplate);
                trDataToSearch.Visible = false;
                trFromDate.Visible = false;
                trToDate.Visible = false;
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
        {
            DbTransaction objDbTransaction1 = Utility.GetTransaction;
            try
            {
                if (ddlField.SelectedValue != "-1")
                {
                    FieldValue = ddlField.SelectedValue;
                    if (FieldValue.Split('-')[1].Trim() != "3")
                    {
                        trDataToSearch.Visible = true;
                        tdDataToSearch.Controls.Clear();
                        Utility.GenerateControlForSearch(tdDataToSearch, ddlField.SelectedValue);
                        trFromDate.Visible = false;
                        trToDate.Visible = false;
                    }
                    else
                    {
                        trDataToSearch.Visible = false;
                        trFromDate.Visible = true;
                        trToDate.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                LoadGridDataByCriteria();

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
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
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Utility.SetGridHoverStyle(e);
                //this.gvwDocument.Columns[0].Visible = false;

                // CheckBox chkHeader = (e.Row.FindControl("chkHeader") as CheckBox);
                //GridViewRow grv1 = gvwDocument.HeaderRow;
                //CheckBox chkHeader = (CheckBox)grv1.FindControl("chkHeader");

                //chkHeader.Visible = true;

                //if (((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == -1)
                ////if (emodModule.SelectedRepository != -1)
                //{
                //    if (((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 75 || ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 76)
                //        if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76)
                //        {
                //            if (e.Row.RowType == DataControlRowType.DataRow)
                //            {
                //                GridViewRow grv1 = gvwDocument.HeaderRow;
                //                CheckBox chkHeader = (CheckBox)grv1.FindControl("chkHeader");
                //                CheckBox chkChild = (e.Row.FindControl("chkChild") as CheckBox);
                //                chkHeader.Visible = true;
                //                chkChild.Visible = true;

                //            }
                //        }
                //        else
                //        {
                //            if (e.Row.RowType == DataControlRowType.DataRow)
                //            {
                //                GridViewRow grv1 = gvwDocument.HeaderRow;
                //                CheckBox chkHeader = (CheckBox)grv1.FindControl("chkHeader");
                //                CheckBox chkChild = (e.Row.FindControl("chkChild") as CheckBox);
                //                chkHeader.Visible = false;
                //                chkChild.Visible = false;

                //            }
                //        }
                //}

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].ColumnSpan = 7;
                    for (int i = 1; i < 10; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
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
                    strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                    string DocName = gvwDocument.DataKeys[intRowIndex].Values["DocumentName"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    Session["DocumentName"] = DocName;
                    DataSet ds = new DataSet();
                    ds = DMS.BusinessLogic.MetaData.SelectRepName(UserSession.MetaDataID);
                    Session["RepositoryName"] = ds.Tables[0].Rows[0]["RepositoryName"].ToString();

                    Log.DocumentAuditLog(HttpContext.Current, "View Document", "SearchDocument", Convert.ToInt32
                        (strDocumentID));

                    con.Open();
                    SqlCommand cmd = new SqlCommand("Select count(DocumentID) from DocumentEntry where DocumentID=" + strDocumentID, con);
                    int a = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();

                    if (a > 0)
                    {
                        if (DocName.Substring(DocName.LastIndexOf(".")) == ".pdf")
                            Response.Redirect("../MetaData/DocumentVerification_forPDF.aspx?DOCID=" + strDocumentID, false);
                        else
                        {
                            Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);//for image viewers
                        }
                    }
                    else
                    {
                        if (ConfigurationManager.AppSettings["PDFViewer"].ToString() == "1")
                        {
                            Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID, false);
                        }
                        else
                        {
                            Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);//for image viewers
                        }

                        //if (DocName.Substring(DocName.LastIndexOf(".")) == ".pdf")                                    
                        //    Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID, false);
                        //else
                        //{
                        //Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);//for image viewers
                        //}
                    }
                }
                if (e.CommandName.ToLower().Trim() == "split")
                {
                    divMerge.Visible = false;
                    divSplit.Visible = true;
                    lblBrowse.Visible = true;
                    txtPageRange.Visible = true;
                    //lblPageNo.Visible = true;
                    //txtPageNo.Visible = true;
                    ibtnSplit.Visible = true;
                    ibtnCancel.Visible = true;
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    Session["Action"] = "Split";
                }
                if (e.CommandName.ToLower().Trim() == "merge")
                {
                    divSplit.Visible = false;
                    divMerge.Visible = true;
                    if (ddlMerge.SelectedValue == "0")
                    {
                        lblPageNo.Visible = false;
                        txtPageRange.Visible = false;
                        lblBrowse.Visible = true;
                        flUpload.Visible = true;
                        ibtnMerge.Visible = true;
                        ibtnCancel.Visible = true;
                        lblDrop.Visible = true;
                        ddlMerge.Visible = true;
                    }
                    else if (ddlMerge.SelectedValue == "1")
                    {
                        lblPageNo.Visible = true;
                        txtPageRange.Visible = true;
                        lblBrowse.Visible = true;
                        flUpload.Visible = true;
                        ibtnMerge.Visible = true;
                        ibtnCancel.Visible = true;
                        lblDrop.Visible = true;
                        ddlMerge.Visible = true;
                    }
                    else if (ddlMerge.SelectedValue == "2")
                    {
                        lblPageNo.Visible = false;
                        txtPageRange.Visible = false;
                        lblBrowse.Visible = true;
                        flUpload.Visible = true;
                        ibtnMerge.Visible = true;
                        ibtnCancel.Visible = true;
                        lblDrop.Visible = true;
                        ddlMerge.Visible = true;
                    }
                    lblBrowse.Visible = true;
                    flUpload.Visible = true;
                    ibtnMerge.Visible = true;
                    ibtnCancel.Visible = true;
                    lblDrop.Visible = true;
                    ddlMerge.Visible = true;

                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    Session["Action"] = "Merge";
                }
                if (e.CommandName.ToLower().Trim() == "edit")
                {
                    GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;

                    gvwDocument.EditIndex = RowIndex;
                    gvwDocument.DataSource = UserSession.GridData;
                    gvwDocument.DataBind();
                }
                if (e.CommandName.ToLower().Trim() == "delete")
                {
                    string confirmValue = Request.Form["confirm_value"];
                    if (confirmValue == "Yes")
                    {
                        // int intRowIndex = Convert.ToInt32(e.CommandArgument);
                        GridViewRow rowSelect = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        int rowindex = rowSelect.RowIndex;

                        strDocumentID = gvwDocument.DataKeys[rowindex].Values["ID"].ToString().Trim();

                        DateTime myDateTime = DateTime.Now;
                        string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                        con.Open();
                        SqlCommand cmd = new SqlCommand("Update Document set Status=0,UpdatedOn='" + sqlFormattedDate + "',UpdatedBy=" + UserSession.UserID + " where ID=" + strDocumentID, con);
                        cmd.ExecuteNonQuery();

                        con.Close();

                        LoadGridDataByCriteria();

                        Log.DocumentAuditLog(HttpContext.Current, "Document Deleted", "SearchDocument", Convert.ToInt32(strDocumentID));


                        // UserSession.DisplayMessage(this, "Document Has Been Deleted Successfully .", MainMasterPage.MessageType.Success);

                    }

                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwDocument.PageIndex = e.NewPageIndex;
                    if (UserSession.FilterData == null)
                        gvwDocument.DataSource = UserSession.GridData;
                    else
                        gvwDocument.DataSource = UserSession.FilterData;

                    gvwDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
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
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void gvwDocument_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int intRowIndex = Convert.ToInt32(e.RowIndex);
                GridViewRow Row = gvwDocument.Rows[e.RowIndex];
                int DocId = Convert.ToInt32(gvwDocument.DataKeys[e.RowIndex]["ID"].ToString());
                string DocumentName = (Row.FindControl("txtDocName") as TextBox).Text;
                string strDocTag = DocumentName.Substring(0, DocumentName.LastIndexOf("."));
                Session["DocumentName"] = strDocTag;

                bool transanction = false;

                DocumentManager objDocumentmanager = new DocumentManager();
                DbTransaction objDBTransaction = BusinessLogic.Utility.GetTransaction;
                objUtility.Result = objDocumentmanager.UpdateDocumentName(DocId, DocumentName, strDocTag, objDBTransaction);
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        objDBTransaction.Commit();
                        transanction = true;
                        UserSession.DisplayMessage(this, "Document Name is Updated SuccessFully", MainMasterPage.MessageType.Success);
                        break;
                    case Utility.ResultType.Error:
                        objDBTransaction.Rollback();
                        UserSession.DisplayMessage(this, "Sorry, Some Error Has Been Occured.", MainMasterPage.MessageType.Error);
                        break;
                }
                if (transanction == true)
                {
                    //Get MacAddress of Machine
                    DbTransaction objDBTransaction1 = BusinessLogic.Utility.GetTransaction;
                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    // UserManager objUserManager=new UserManager();
                    Report objReport = new Report();
                    string MacAdd = nics[0].GetPhysicalAddress().ToString();
                    string IpAddress = GetIPAddress(HttpContext.Current);
                    string Activity = "Document Renamed";
                    int UserId = UserSession.UserID;
                    DateTime DateOfActivity = DateTime.Now;
                    string DocName = Convert.ToString(Session["DocumentName"]);
                    string IPAddress = GetIPAddress(HttpContext.Current);
                    string MacAddress = GetMacAddress();
                    objReport.InsertAuditLog(IPAddress, MacAddress, Activity, "null", UserSession.UserID);
                    // objUtility.Result = objUserManager.
                    //AuditLogDetails(IpAddress, MacAdd, Activity, DocName, UserId, objDBTransaction1);
                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            objDBTransaction1.Commit();
                            break;
                        case Utility.ResultType.Error:
                            objDBTransaction.Rollback();
                            break;
                    }

                }
                gvwDocument.EditIndex = -1;
                LoadGridDataByCriteria();
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured.", MainMasterPage.MessageType.Error);
            }
        }

        protected void gvwDocument_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvwDocument.EditIndex = -1;
                LoadGridDataByCriteria();
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry, Some Error Has Been Occured.", MainMasterPage.MessageType.Error);
                throw;
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

        #endregion

        #region Method

        //private void LoadGridDataByCriteria_Indepay()
        //{
        //    string strDataTypeID = ddlField.SelectedValue.Split('-')[1].ToString();
        //    string strEnterFieldValue = string.Empty;
        //    strEnterFieldValue = ((TextBox)tdDataToSearch.FindControl(ddlField.SelectedValue.Split('-')[0].ToString())).Text;
        //}

        private void LoadGridDataByCriteria()
        {
            try
            {
                gvwDocument.DataSource = null;
                gvwDocument.DataBind();
                Hashtable objTempList = new Hashtable();
                ibtnDownloadFiles.Visible = false;

                #region MetatemplateField Search

                if (rdblSearchBy.SelectedValue == "1")
                {
                    if (ddlField.SelectedValue == "-1")
                    {
                        UserSession.DisplayMessage(this, "Please Select The Field .", MainMasterPage.MessageType.Warning);
                        return;
                    }
                    DocumentManager objDocumentManager = new DocumentManager();
                    BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                    {
                        RepositoryID = emodModule.SelectedRepository,
                        MetaTemplateID = emodModule.SelectedMetaTemplate,
                        CategoryID = emodModule.SelectedCategory,
                        FolderID = emodModule.SelectedFolder,
                        MetaDataID = emodModule.SelectedMetaDataCode
                    };

                    string strDataTypeID = ddlField.SelectedValue.Split('-')[1].ToString();
                    string strEnterFieldValue = string.Empty;
                    if (strDataTypeID.Trim() != "4" && strDataTypeID.Trim() != "9" && strDataTypeID.Trim() != "3")
                    {
                        strEnterFieldValue = ((TextBox)tdDataToSearch.FindControl(ddlField.SelectedValue.Split('-')[0].ToString())).Text;
                    }
                    else if (strDataTypeID.Trim() == "4")
                    {
                        strEnterFieldValue = ((DropDownList)tdDataToSearch.FindControl(ddlField.SelectedValue.Split('-')[0].ToString())).SelectedItem.Value;
                    }
                    else if (strDataTypeID.Trim() == "9")
                    {
                        CheckBoxList objCheckBoxList = ((CheckBoxList)tdDataToSearch.FindControl(ddlField.SelectedValue.Split('-')[0].ToString()));
                        foreach (ListItem objListItem in objCheckBoxList.Items)
                        {
                            if (objListItem.Selected)
                                strEnterFieldValue = strEnterFieldValue + objListItem.Value + ",";
                        }
                        if (strEnterFieldValue != string.Empty)
                            strEnterFieldValue = strEnterFieldValue.Remove(strEnterFieldValue.LastIndexOf(','));

                        string[] strArray = strEnterFieldValue.Split(',');
                        if (strArray.Length > 0)
                        {
                            strEnterFieldValue = string.Empty;
                            Array.Sort(strArray);
                        }

                        foreach (string strItem in strArray)
                        {
                            strEnterFieldValue = strEnterFieldValue + strItem + ",";
                        }
                        if (strEnterFieldValue != string.Empty)
                            strEnterFieldValue = strEnterFieldValue.Remove(strEnterFieldValue.LastIndexOf(','));
                    }

                    BusinessLogic.DocumentEntry objDocumentEntry = new BusinessLogic.DocumentEntry()
                    {
                        FieldID = Convert.ToInt32(ddlField.SelectedValue.Split('-')[0].ToString()),
                        FieldData = strEnterFieldValue,
                        FromDate = txtFromDate.Text,
                        ToDate = txtToDate.Text
                    };

                    DataTable objDataTable = new DataTable();
                    if (strDataTypeID.Trim() != "3")
                    {
                        if (ddlCriteria.SelectedValue == "1")
                        {
                            objUtility.Result = objDocumentManager.SelectMetaDataForSearch(out objDataTable, objMetaData, objDocumentEntry, BusinessLogic.MetaData.SearchType.EqualTo);

                            objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
                            objTempList.Add("METADATA", objMetaData);
                            objTempList.Add("DOCUMENTENTRY", objDocumentEntry);
                            objTempList.Add("SEARCHTYPE", BusinessLogic.MetaData.SearchType.EqualTo);
                            UserSession.TemporaryList = objTempList;
                        }
                        else if (ddlCriteria.SelectedValue == "2")
                        {
                            objUtility.Result = objDocumentManager.SelectMetaDataForSearch(out objDataTable, objMetaData, objDocumentEntry, BusinessLogic.MetaData.SearchType.Like);

                            objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
                            objTempList.Add("METADATA", objMetaData);
                            objTempList.Add("DOCUMENTENTRY", objDocumentEntry);
                            objTempList.Add("SEARCHTYPE", BusinessLogic.MetaData.SearchType.Like);
                            UserSession.TemporaryList = objTempList;
                        }
                    }
                    else
                    {
                        objUtility.Result = objDocumentManager.SelectMetaDataForSearch(out objDataTable, objMetaData, objDocumentEntry, BusinessLogic.MetaData.SearchType.DateRange);

                        objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
                        objTempList.Add("METADATA", objMetaData);
                        objTempList.Add("DOCUMENTENTRY", objDocumentEntry);
                        objTempList.Add("SEARCHTYPE", BusinessLogic.MetaData.SearchType.DateRange);
                        UserSession.TemporaryList = objTempList;
                    }

                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            gvwDocument.DataSource = objDataTable;
                            if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76 || emodModule.SelectedRepository == 0 || emodModule.SelectedRepository == 1002) // 1002 is used for testing purpose on local
                            {
                                gvwDocument.AllowPaging = false;
                                ibtnDownloadFiles.Visible = true;

                            }
                            else
                            {
                                gvwDocument.AllowPaging = true;
                                ibtnDownloadFiles.Visible = false;
                                gvwDocument.PageSize = 10;
                            }
                            gvwDocument.DataBind();
                            if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76 || emodModule.SelectedRepository == 0 || emodModule.SelectedRepository == 1002) // 1002 is used for testing purpose on local
                                ibtnDownloadFiles.Visible = true;
                            UserSession.TemporaryList.Add("DOWNLOADBTN", ibtnDownloadFiles.Visible);
                            UserSession.FilterData = null;
                            UserSession.GridData = objDataTable;
                            break;

                        case Utility.ResultType.Failure:
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                            break;

                        case Utility.ResultType.Error:
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            break;
                    }
                }

                #endregion

                #region Document Tag

                else if (rdblSearchBy.SelectedValue == "2")
                {
                    if (txtTextToSeach.Text.Trim() == string.Empty)
                    {
                        UserSession.DisplayMessage(this, "Please Enter The Text To Search .", MainMasterPage.MessageType.Warning);
                        return;
                    }
                    DocumentManager objDocumentManager = new DocumentManager();
                    BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                    {
                        RepositoryID = emodModule.SelectedRepository,
                        MetaTemplateID = emodModule.SelectedMetaTemplate,
                        CategoryID = emodModule.SelectedCategory,
                        FolderID = emodModule.SelectedFolder,
                        MetaDataID = emodModule.SelectedMetaDataCode
                    };

                    BusinessLogic.Document objDocument = new BusinessLogic.Document()
                    {
                        Tag = txtTextToSeach.Text.Trim()
                    };

                    DataTable objDataTable = new DataTable();
                    if (ddlCriteria.SelectedValue == "1")
                    {
                        objUtility.Result = objDocumentManager.SelectMetaDataForTagSearch(out objDataTable, objMetaData, objDocument, BusinessLogic.MetaData.SearchType.EqualTo);

                        objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
                        objTempList.Add("METADATA", objMetaData);
                        objTempList.Add("DOCUMENT", objDocument);
                        objTempList.Add("SEARCHTYPE", BusinessLogic.MetaData.SearchType.EqualTo);
                        objTempList.Add("TEXT", txtTextToSeach.Text.Trim());
                        UserSession.TemporaryList = objTempList;
                    }
                    else if (ddlCriteria.SelectedValue == "2")
                    {
                        objUtility.Result = objDocumentManager.SelectMetaDataForTagSearch(out objDataTable, objMetaData, objDocument, BusinessLogic.MetaData.SearchType.Like);

                        objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
                        objTempList.Add("METADATA", objMetaData);
                        objTempList.Add("DOCUMENT", objDocument);
                        objTempList.Add("SEARCHTYPE", BusinessLogic.MetaData.SearchType.Like);
                        objTempList.Add("TEXT", txtTextToSeach.Text.Trim());
                        UserSession.TemporaryList = objTempList;
                    }

                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:

                            gvwDocument.DataSource = objDataTable;
                            if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76 || emodModule.SelectedRepository == 0 || emodModule.SelectedRepository == 1002)// 1002 is used for testing purpose on local
                            {
                                gvwDocument.AllowPaging = false;
                                ibtnDownloadFiles.Visible = true;

                            }
                            else
                            {
                                gvwDocument.AllowPaging = true;
                                ibtnDownloadFiles.Visible = false;
                                gvwDocument.PageSize = 10;
                            }
                            gvwDocument.DataBind();
                            if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76 || emodModule.SelectedRepository == 0 || emodModule.SelectedRepository == 1002)// 1002 is used for testing purpose on local

                                ibtnDownloadFiles.Visible = true;

                            UserSession.TemporaryList.Add("DOWNLOADBTN", ibtnDownloadFiles.Visible);
                            UserSession.FilterData = null;
                            UserSession.GridData = objDataTable;
                            #region insert auditlog

                            //string strHostName = "";
                            //strHostName = System.Net.Dns.GetHostName();

                            //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                            //IPAddress[] addr = ipEntry.AddressList;
                            Report objReport = new Report();
                            string IPAddress = GetIPAddress(HttpContext.Current);
                            //DateTime Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                            //DateTime Logintime = Convert.ToDateTime(DateTime.Now);
                            DateTime LoginDate = DateTime.Today;
                            string Activity = "Search Document";

                            string MacAddress = GetMacAddress();
                            objReport.InsertAuditLog(IPAddress, MacAddress, Activity, "NULL", UserSession.UserID);
                            #endregion
                            break;

                        case Utility.ResultType.Failure:
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();

                            UserSession.FilterData = null;
                            UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                            break;

                        case Utility.ResultType.Error:
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            break;
                    }
                }

                #endregion

                #region Search by Folder Name

                else if (rdblSearchBy.SelectedValue == "4")
                {

                    if (emodModule.SelectedFolder == 0)
                    {
                        UserSession.DisplayMessage(this, "Please Select Folder To Search .", MainMasterPage.MessageType.Warning);
                        return;
                    }
                    DocumentManager objDocumentManager = new DocumentManager();
                    BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                    {
                        RepositoryID = emodModule.SelectedRepository,
                        MetaTemplateID = emodModule.SelectedMetaTemplate,
                        CategoryID = emodModule.SelectedCategory,
                        FolderID = emodModule.SelectedFolder,
                        MetaDataID = emodModule.SelectedMetaDataCode
                    };

                    DataTable objDataTable = new DataTable();
                    objUtility.Result = objDocumentManager.SelectMetaDataForFolderSearch(out objDataTable, objMetaData);

                    objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
                    objTempList.Add("METADATA", objMetaData);
                    objTempList.Add("DOCUMENT", objDocumentManager);
                    objTempList.Add("SEARCHTYPE", BusinessLogic.MetaData.SearchType.EqualTo);
                    objTempList.Add("TEXT", txtTextToSeach.Text.Trim());
                    UserSession.TemporaryList = objTempList;
                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            gvwDocument.DataSource = objDataTable;
                            if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76 || emodModule.SelectedRepository == 0 || emodModule.SelectedRepository == 1002) // 1002 is used for testing purpose on local
                            {
                                gvwDocument.AllowPaging = false;
                                ibtnDownloadFiles.Visible = true;

                            }
                            else
                            {
                                gvwDocument.AllowPaging = true;
                                ibtnDownloadFiles.Visible = false;
                                gvwDocument.PageSize = 10;
                            }
                            gvwDocument.DataBind();

                            UserSession.TemporaryList.Add("DOWNLOADBTN", ibtnDownloadFiles.Visible);
                            UserSession.FilterData = null;
                            UserSession.GridData = objDataTable;

                            break;

                        case Utility.ResultType.Failure:
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();
                            UserSession.FilterData = null;

                            UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                            break;

                        case Utility.ResultType.Error:
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();
                            UserSession.FilterData = null;

                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            break;
                    }

                }

                #endregion

                #region Content Search

                #region old code
                //else if (rdblSearchBy.SelectedValue == "3")
                //{
                //    if (txtTextToSeach.Text.Trim() == string.Empty)
                //    {
                //        UserSession.DisplayMessage(this, "Please Enter The Text To Search .", MainMasterPage.MessageType.Warning);
                //        return;
                //    }
                //    string strDocumentID = string.Empty;
                //    strDocumentID = Utility.SearchPageContent(txtTextToSeach.Text.Trim());

                //    if (strDocumentID.Trim() == string.Empty)
                //    {
                //        UserSession.DisplayMessage(this, "No Result To Display .", MainMasterPage.MessageType.Warning);
                //        return;
                //    }
                //    DocumentManager objDocumentManager = new DocumentManager();
                //    BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                //    {
                //        RepositoryID = emodModule.SelectedRepository,
                //        MetaTemplateID = emodModule.SelectedMetaTemplate,
                //        CategoryID = emodModule.SelectedCategory,
                //        FolderID = emodModule.SelectedFolder,
                //        MetaDataID = emodModule.SelectedMetaDataCode
                //    };

                //    DataTable objDataTable = new DataTable();

                //    objUtility.Result = objDocumentManager.SelectMetaDataForContentSearch(out objDataTable, objMetaData, strDocumentID);
                //    objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
                //    objTempList.Add("METADATA", objMetaData);
                //    objTempList.Add("DOCUMENTID", strDocumentID);
                //    objTempList.Add("TEXT", txtTextToSeach.Text.Trim());
                //    UserSession.TemporaryList = objTempList;

                //    switch (objUtility.Result)
                //    {
                //        case Utility.ResultType.Success:
                //            gvwDocument.DataSource = objDataTable;
                //            gvwDocument.DataBind();
                //            UserSession.FilterData = null;
                //            UserSession.GridData = objDataTable;
                //            break;

                //        case Utility.ResultType.Failure:
                //            gvwDocument.DataSource = objDataTable;
                //            gvwDocument.DataBind();
                //            UserSession.FilterData = null;
                //            UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                //            break;

                //        case Utility.ResultType.Error:
                //            gvwDocument.DataSource = objDataTable;
                //            gvwDocument.DataBind();
                //            UserSession.FilterData = null;
                //            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                //            break;
                //    }
                //}
                #endregion

                #region new code by Sneha

                else if (rdblSearchBy.SelectedValue == "3")
                {
                    try
                    {
                        BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                        {
                            RepositoryID = emodModule.SelectedRepository,
                            MetaTemplateID = emodModule.SelectedMetaTemplate,
                            CategoryID = emodModule.SelectedCategory,
                            FolderID = emodModule.SelectedFolder,
                            MetaDataID = emodModule.SelectedMetaDataCode
                        };
                        DataSet ds = new DataSet();
                        DataSet dsResultDoc = new DataSet();
                        ds = Document.SelectDocForContentSearch(objMetaData.RepositoryID, objMetaData.MetaTemplateID, objMetaData.CategoryID, objMetaData.FolderID);
                        DataSet dsDetails = new DataSet();
                        //for(int i=0;i<ds.Tables[0].Rows.Count;i++)
                        //{
                        dsResultDoc = ReadPdfFile(ds, txtTextToSeach.Text);
                        if (dsResultDoc != null)
                        {

                            //dsDetails = Document.SelectDocDetailsForContentSearch(Convert.ToInt16(dsResultDoc.Tables[0].Rows[0]["DocID"].ToString()));
                            DataTable dtDetails = new DataTable();
                            dtDetails.Columns.Add(new DataColumn("DocID", typeof(int)));
                            for (int j = 0; j < dsResultDoc.Tables[0].Rows.Count; j++)
                            {

                                DataRow dr = dtDetails.NewRow();
                                dr["DocID"] = dsResultDoc.Tables[0].Rows[j]["DocID"].ToString();
                                dtDetails.Rows.Add(dr);


                            }
                            dsDetails.Tables.Add(dtDetails);

                            //}
                            string output = "";
                            for (int i = 0; i < dsDetails.Tables[0].Rows.Count; i++)
                            {
                                output = output + dsDetails.Tables[0].Rows[i]["DocID"].ToString();
                                output += (i < dsDetails.Tables[0].Rows.Count) ? "," : string.Empty;
                            }
                            output = output.Remove(output.Length - 1, 1);
                            DataSet dsFinalResult = new DataSet();
                            if (output != "")
                            {
                                dsFinalResult = Document.SelectDocDetailsForContentSearch(output);
                                if (dsFinalResult.Tables[0].Rows.Count > 0)
                                {
                                    gvwDocument.Visible = true;
                                    gvwDocument.DataSource = dsFinalResult;
                                    if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76)
                                    {
                                        gvwDocument.AllowPaging = false;
                                        ibtnDownloadFiles.Visible = true;

                                    }
                                    else
                                    {
                                        gvwDocument.AllowPaging = true;
                                        ibtnDownloadFiles.Visible = false;
                                        gvwDocument.PageSize = 10;
                                    }
                                    gvwDocument.DataBind();
                                }
                            }
                            dsFinalResult.Clear();
                        }
                        else
                        {
                            gvwDocument.Visible = false;
                            UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                        }
                        ds.Clear();
                        dsResultDoc.Clear();
                        dsDetails.Clear();

                    }
                    catch (Exception ex)
                    {
                        LogManager.ErrorLog(Utility.LogFilePath, ex);
                    }
                }
                #endregion

                #endregion

                #region Field Search

                else if (rdblSearchBy.SelectedValue == "5")
                {
                    if (txtTextToSeach.Text.Trim() == string.Empty)
                    {
                        UserSession.DisplayMessage(this, "Please Enter The Text To Search .", MainMasterPage.MessageType.Warning);
                        return;
                    }

                    int RepositoryID = emodModule.SelectedRepository;
                    int MetatemplateId = emodModule.SelectedMetaTemplate;
                    int FolderID = emodModule.SelectedFolder;
                    string FieldData = txtTextToSeach.Text;

                    try
                    {
                        DataTable objDataTableField = new DataTable();
                        objUtility.Result = objDocumentManager.FieldSearch(out objDataTableField, RepositoryID, MetatemplateId, FolderID, FieldData);
                        if (objDataTableField.Rows.Count > 0)
                        {
                            //lbldtcount.Text = objDataTableField.Rows.Count.ToString();
                            gvwDocument.DataSource = objDataTableField;
                            gvwDocument.AllowPaging = true;
                            ibtnDownloadFiles.Visible = false;
                            gvwDocument.PageSize = 10;
                            gvwDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.GridData = objDataTableField;
                        }
                        else
                        {
                            //lbldtcount.Text = objDataTableField.Rows.Count.ToString();
                            gvwDocument.DataSource = null;
                            gvwDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.ErrorLog(Utility.LogFilePath, ex);
                    }

                }

                #endregion

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        private DataSet ReadPdfFile(DataSet ds, String searthText)
        {
            try
            {
                DataSet dsDoc = new DataSet();
                DataTable dt = new DataTable();
                dsDoc.Clear();
                dt.Clear();
                dt.Columns.Add(new DataColumn("DocName", typeof(string)));
                dt.Columns.Add(new DataColumn("DocID", typeof(int)));

                List<int> pages = new List<int>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (File.Exists(ds.Tables[0].Rows[i]["DocumentPath"].ToString()))
                    {
                        PdfReader pdfReader = new PdfReader(ds.Tables[0].Rows[i]["DocumentPath"].ToString());
                        for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                        {
                            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                            string currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                            if (currentPageText.ToUpper().Contains(searthText.ToUpper()))
                            {
                                DataRow dr = dt.NewRow();
                                dr["DocName"] = ds.Tables[0].Rows[i]["DocumentPath"].ToString();
                                dr["DocID"] = ds.Tables[0].Rows[i]["ID"].ToString();
                                dt.Rows.Add(dr);

                                page = pdfReader.NumberOfPages;
                                break;
                            }

                        }
                    }
                }
                dsDoc.Tables.Add(dt);
                if (dsDoc.Tables[0].Rows.Count > 0)
                    return dsDoc;
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadGridDataFromTemporaryList()
        {
            try
            {
                if (UserSession.TemporaryList != null)
                {
                    if (UserSession.TemporaryList["SEARCHBY"] != null)
                    {
                        DataTable objDataTable = new DataTable();
                        DocumentManager objDocumentManager = new DocumentManager();
                        if (UserSession.TemporaryList["SEARCHBY"].ToString().Trim() == "1")
                        {
                            if (UserSession.TemporaryList["METADATA"] != null && UserSession.TemporaryList["DOCUMENTENTRY"] != null && UserSession.TemporaryList["SEARCHTYPE"] != null)
                            {
                                if (((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]) == BusinessLogic.MetaData.SearchType.EqualTo)
                                    ddlCriteria.SelectedIndex = 0;
                                else
                                    ddlCriteria.SelectedIndex = 1;

                                rdblSearchBy.Items.FindByValue("1").Selected = true;
                                objUtility.Result = objDocumentManager.SelectMetaDataForSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), ((DMS.BusinessLogic.DocumentEntry)UserSession.TemporaryList["DOCUMENTENTRY"]), ((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]));
                            }
                        }
                        else if (UserSession.TemporaryList["SEARCHBY"].ToString().Trim() == "2")
                        {
                            if (UserSession.TemporaryList["METADATA"] != null && UserSession.TemporaryList["DOCUMENT"] != null && UserSession.TemporaryList["SEARCHTYPE"] != null && UserSession.TemporaryList["TEXT"] != null)
                            {
                                if (((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]) == BusinessLogic.MetaData.SearchType.EqualTo)
                                    ddlCriteria.SelectedIndex = 0;
                                else
                                    ddlCriteria.SelectedIndex = 1;

                                rdblSearchBy.Items.FindByValue("2").Selected = true;
                                txtTextToSeach.Text = UserSession.TemporaryList["TEXT"].ToString();
                                objUtility.Result = objDocumentManager.SelectMetaDataForTagSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), ((DMS.BusinessLogic.Document)UserSession.TemporaryList["DOCUMENT"]), ((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]));
                            }
                        }
                        else if (UserSession.TemporaryList["SEARCHBY"].ToString().Trim() == "3")
                        {
                            if (UserSession.TemporaryList["METADATA"] != null && UserSession.TemporaryList["DOCUMENTID"] != null && UserSession.TemporaryList["TEXT"] != null)
                            {
                                rdblSearchBy.Items.FindByValue("3").Selected = true;
                                txtTextToSeach.Text = UserSession.TemporaryList["TEXT"].ToString();
                                objUtility.Result = objDocumentManager.SelectMetaDataForContentSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), UserSession.TemporaryList["DOCUMENTID"].ToString().ToLower());
                            }
                        }

                        else if (UserSession.TemporaryList["SEARCHBY"].ToString().Trim() == "4")
                        {
                            if (UserSession.TemporaryList["METADATA"] != null && UserSession.TemporaryList["DOCUMENT"] != null && UserSession.TemporaryList["SEARCHTYPE"] != null)
                            {
                                if (((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]) == BusinessLogic.MetaData.SearchType.EqualTo)
                                    ddlCriteria.SelectedIndex = 0;
                                else
                                    ddlCriteria.SelectedIndex = 1;
                                rdblSearchBy.Items.FindByValue("4").Selected = true;
                                objUtility.Result = objDocumentManager.SelectMetaDataForFolderSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]));
                                ////(emodModule.FindControl("ddlCategoryName") as DropDownList).SelectedValue = Convert.ToString(((BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).CategoryID);
                                //(emodModule.FindControl("ddlCategoryName") as DropDownList).Items.FindByValue(Convert.ToString(((BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).CategoryID)).Selected = true;
                                //emodModule.ddlCategoryName_SelectedIndexChanged(null, null);

                                //txtTextToSeach.Text = "";
                                //txtTextToSeach.Text = UserSession.TemporaryList["TEXT"].ToString();
                                //objUtility.Result = objDocumentManager.SelectMetaDataForFolderSearch (out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]));
                                //SelectMetaDataForTagSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), ((DMS.BusinessLogic.Document)UserSession.TemporaryList["DOCUMENT"]), ((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]));
                            }
                        }

                        if (objUtility.Result == Utility.ResultType.Success)
                        {
                            //  int i = UserSession.RepositoryID;

                            gvwDocument.DataSource = objDataTable;
                            if (((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 75 || ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 76)
                            {
                                gvwDocument.AllowPaging = false;
                                ibtnDownloadFiles.Visible = true;

                            }
                            else
                            {
                                gvwDocument.AllowPaging = true;
                                ibtnDownloadFiles.Visible = false;
                                gvwDocument.PageSize = 10;
                            }
                            gvwDocument.DataBind();
                            //emodModule.SelectedRepository = ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID;
                            //if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76)
                            //    ibtnDownloadFiles.Visible = true;
                            ibtnDownloadFiles.Visible = (bool)UserSession.TemporaryList["DOWNLOADBTN"];
                            UserSession.FilterData = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public void Incrementcount()
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                // int Value = totalcount();

                objdocument.OutPutValuues = totalcount() + 1;
                objdocument.MetaDataID = UserSession.MetaDataID;
                objUtility.Result = Document.Increasecount(objdocument, objDbTransaction);
                objDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        public int totalcount()
        {
            int count = 0;
            DbTransaction objDbTransaction = Utility.GetTransaction;
            try
            {
                objdocument.MetaDataID = UserSession.MetaDataID;
                objUtility.Result = Document.LastTotalCount(objdocument, objDbTransaction, out count);

            }
            catch (Exception ex)
            {

            }
            return count;
        }

        public string GetIPAddress(HttpContext context)
        {
            string ipList = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];

        }

        protected void ibtnDownloadFiles_Click(object sender, ImageClickEventArgs e)
        {
            DownloadFiles();
        }

        public void DownloadFiles()
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
                    if ((row.FindControl("chkChild") as CheckBox).Checked)
                    {
                        Id = (int)gvwDocument.DataKeys[row.RowIndex].Values["ID"];
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


        #endregion

        protected void rdblSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdblSearchBy.SelectedValue == "1")
            {
                // Utility.LoadField(ddlField, emodModule.SelectedMetaTemplate, emodModule.SelectedRepository);
                txtTextToSeach.Visible = false;
                tdDataToSearch.Visible = true;
                // tdsearch.Visible = true;
                lblTextToSeach.Visible = false;
                trDataToSearch.Visible = true;
                lblDataToSearch.Visible = true;
                fieldtr.Visible = true;
            }
            else
            {
                lblTextToSeach.Visible = true;
                txtTextToSeach.Visible = true;
                tdDataToSearch.Visible = false;
                // tdsearch.Visible = false;
                trDataToSearch.Visible = false;
                txtTextToSeach.Text = string.Empty;
                fieldtr.Visible = false;
                lblDataToSearch.Visible = false;
            }
        }

        protected void ibtnMerge_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (flUpload.HasFile)
                {
                    if (!System.IO.Directory.Exists(Utility.ArchiveFilePath))
                    {
                        System.IO.Directory.CreateDirectory(Utility.ArchiveFilePath);
                    }
                    if (!System.IO.Directory.Exists(Utility.VersionFilePath))
                    {
                        System.IO.Directory.CreateDirectory(Utility.VersionFilePath);
                    }

                    int Size = flUpload.PostedFile.ContentLength;

                    string Filename = System.IO.Path.GetFileName(flUpload.PostedFile.FileName);
                    string FilePath = Utility.ArchiveFilePath + Filename;

                    Stream objFileStream = flUpload.PostedFile.InputStream;
                    byte[] info1 = new byte[flUpload.PostedFile.ContentLength];
                    objFileStream.Read(info1, 0, info1.Length);
                    objFileStream.Close();
                    using (Stream file = File.OpenWrite(FilePath))
                    {
                        file.Write(info1, 0, info1.Length);
                    }

                    PdfReader readermerge = new PdfReader(FilePath);
                    int MergePageCount = readermerge.NumberOfPages;
                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
                    IPAddress[] addr = ipEntry.AddressList;
                    string IPAddress = addr[addr.Length - 1].ToString();
                    readermerge.Close();

                    DataTable dt = new DataTable();
                    string FirstFile = string.Empty;
                    string SecondFile = string.Empty;

                    dt = DMS.BusinessLogic.Document.GetDocumentPath(Convert.ToInt32(Session["DocId"]));

                    string OldFileName = dt.Rows[0]["DocumentName"].ToString();
                    string DocGuid = dt.Rows[0]["DOCUMENTGUID"].ToString();
                    int OldPageCount = Convert.ToInt32(dt.Rows[0]["PAGECOUNT"].ToString());
                    int OldPageSize = Convert.ToInt32(dt.Rows[0]["Size"].ToString());
                    string OldFilePath = dt.Rows[0]["DocumentPath"].ToString();

                    if (ddlMerge.SelectedValue == "2")
                    {
                        string[] filename = { OldFilePath, FilePath };
                        MergeFilesNew(filename, Utility.VersionFilePath + OldFileName);
                    }
                    else if (ddlMerge.SelectedValue == "1")
                    {
                        if (Convert.ToInt32(dt.Rows[0]["PageCount"]) <= Convert.ToInt32(txtPageRange.Text))
                        {
                            UserSession.DisplayMessage(this, "Page Count out of Range..", MainMasterPage.MessageType.Error);
                            return;

                        }
                        else if ((Convert.ToInt32(dt.Rows[0]["PageCount"]) >= Convert.ToInt32(txtPageRange.Text)) && txtPageRange.Text != "0")
                        {
                            SplitDocuments(1, Convert.ToInt32(txtPageRange.Text), OldFilePath, dt, "1");
                            SplitDocuments(Convert.ToInt32(txtPageRange.Text) + 1, Convert.ToInt32(dt.Rows[0]["PageCount"]), "", dt, "2");
                            //merge the files

                            FirstFile = ConfigurationManager.AppSettings["DMSTemp"].ToString() + "\\" + Session["DocId"].ToString() + "_" + "1" + ".pdf";
                            SecondFile = ConfigurationManager.AppSettings["DMSTemp"].ToString() + "\\" + Session["DocId"].ToString() + "_" + "2" + ".pdf";
                            string[] filenames = { FirstFile, FilePath, SecondFile };
                            MergeFilesNew(filenames, Utility.VersionFilePath + OldFileName);
                        }
                        else if (txtPageRange.Text == "0")
                        {
                            UserSession.DisplayMessage(this, "Please enter valid number", MainMasterPage.MessageType.Error);
                            return;

                        }

                    }
                    else if (ddlMerge.SelectedValue == "0")
                    {
                        string[] filenameBegining = { FilePath, OldFilePath };
                        MergeFilesNew(filenameBegining, Utility.VersionFilePath + OldFileName);
                    }

                    PdfReader reader = new PdfReader(Utility.VersionFilePath + OldFileName);
                    int No_Of_Pages = reader.NumberOfPages;
                    FileInfo info = new FileInfo(Utility.VersionFilePath + OldFileName);

                    // decimal fileSize = (info.Length) / 1024;
                    decimal fileSize = (info.Length);

                    //File.Copy(Utility.VersionFilePath + OldFileName, Utility.DocumentPath + objdocument.DocumentGuid, true);

                    string filepaths = System.IO.Path.GetDirectoryName(OldFilePath);
                    File.Copy(filepaths + "\\" + DocGuid, Utility.ArchiveFilePath + DocGuid, true);
                    File.Copy(Utility.VersionFilePath + OldFileName, filepaths + "\\" + DocGuid, true);

                    objdocument.DocumentID = Convert.ToInt32(Session["DocId"].ToString());
                    objdocument.Size = Convert.ToInt32(fileSize);
                    objdocument.OldPageCount = OldPageCount;
                    objdocument.PageCount = OldPageCount + MergePageCount;
                    objdocument.UpdatedBy = UserSession.UserID;
                    objdocument.DocumentName = OldFileName;
                    objdocument.DocumentGuid = DocGuid;
                    objdocument.DocumentPath = OldFilePath;
                    objdocument.UpdatedBy = UserSession.UserID; ;
                    // objdocument.Size = Size;
                    objdocument.MergedPageCount = MergePageCount;
                    objdocument.IPAddress = IPAddress;
                    DocumentManager objDocumentManager = new DocumentManager();
                    DbTransaction objDbTransaction = Utility.GetTransaction;
                    objUtility.Result = objDocumentManager.UpdateDocumentForMergingNEW(objdocument, objDbTransaction);

                    if (FirstFile != string.Empty)
                    {
                        File.Delete(FirstFile);
                        File.Delete(SecondFile);
                    }
                    reader.Close();

                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:

                            DMS.BusinessLogic.Report objReport = new DMS.BusinessLogic.Report();
                            objReport.InsertAuditLog(GetIPAddress(), GetMacAddress(), "Merging", objdocument.DocumentName, UserSession.UserID);
                            objDbTransaction.Commit();
                            Log.DocumentAuditLog(HttpContext.Current, "Document Merged", "SearchDocument", objdocument.DocumentID);
                            UserSession.DisplayMessage(this, "Document has been merged", MainMasterPage.MessageType.Success);
                            break;
                        case Utility.ResultType.Failure:
                            File.Copy(Utility.ArchiveFilePath + DocGuid, Utility.DocumentPath + DocGuid, true);
                            objDbTransaction.Rollback();
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);

                            break;
                        case Utility.ResultType.Error:
                            File.Copy(Utility.ArchiveFilePath + DocGuid, Utility.DocumentPath + DocGuid, true);
                            objDbTransaction.Rollback();
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            break;
                    }
                }
                else
                {
                    UserSession.DisplayMessage(this, "Please Select file to be Mereged !", MainMasterPage.MessageType.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                // return Utility.ResultType.Error;
            }

        }

        protected void ibtnSplit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string[] range = txtRange.Text.Split('-');
                SelectedPages = txtRange.Text;
                List<int> Pagestokeep = new List<int>();
                DataTable dt = new DataTable();
                if (range.Count() == 1 || range.Count() == 0)
                {
                    UserSession.DisplayMessage(this, "Please Enter Valid Page Range .", MainMasterPage.MessageType.Warning);
                    return;
                }
                int FromPage = int.Parse(range[0]);
                int Topage = int.Parse(range[1]);
                if (FromPage > Topage)
                {
                    UserSession.DisplayMessage(this, "From Page should be greater than To page", MainMasterPage.MessageType.Warning);
                    return;
                }
                dt = DMS.BusinessLogic.Document.GetDocumentPath(Convert.ToInt32(Session["DocId"]));
                if (Topage > Convert.ToInt32(dt.Rows[0]["PageCount"]))
                {
                    UserSession.DisplayMessage(this, "Range Doesnt matches with total number of Page of Selected Document.", MainMasterPage.MessageType.Warning);
                    return;
                }

                if (Convert.ToString(dt.Rows[0]["DocumentType"]) == ".pdf" || Convert.ToString(dt.Rows[0]["DocumentType"]) == ".PDF")
                {



                    for (int i = FromPage; i <= Topage; i++)
                    {
                        Pagestokeep.Add(i);
                    }
                    string SourcePath = dt.Rows[0]["DocumentPath"].ToString();
                    string DestinationPath = ConfigurationManager.AppSettings["DMSTemp"].ToString() + "\\" + Convert.ToInt32(Session["DocId"]) + ".pdf";

                    ExtractPages(SourcePath, DestinationPath, FromPage, Topage);

                    //removePagesFromPdf(SourcePath, DestinationPath, Pagestokeep.ToArray());

                    Log.DocumentAuditLog(HttpContext.Current, "Document Splited", "SearchDocument", Convert.ToInt32(Session["DocId"]));
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("content-disposition", "attachment;filename=\"" + dt.Rows[0]["DocumentName"].ToString() + "\"");
                    Response.TransmitFile(DestinationPath);
                    Response.End();
                }
                else if (Convert.ToString(dt.Rows[0]["DocumentType"]) == ".tif" || Convert.ToString(dt.Rows[0]["DocumentType"]) == ".tiff")
                {

                    FilePath = Convert.ToString(dt.Rows[0]["DocumentPath"]);
                    //this.FilePath = path;
                    //Get the frame dimension list from the image of the file and
                    System.Drawing.Image tiffImage = System.Drawing.Image.FromFile(FilePath);
                    //get the globally unique identifier (GUID)
                    Guid objGuid = tiffImage.FrameDimensionsList[0];
                    //create the frame dimension
                    FrameDimension dimension = new FrameDimension(objGuid);
                    //Gets the total number of frames in the .tiff file
                    NumberOfPages = tiffImage.GetFrameCount(dimension);
                    tiffImage.Dispose();

                    Split(ConfigurationManager.AppSettings["DMSTemp"].ToString() + "\\" + Convert.ToString(dt.Rows[0]["DocumentName"]));

                    Log.DocumentAuditLog(HttpContext.Current, "Document Splited", "SearchDocument", Convert.ToInt32(Session["DocId"]));

                    Response.Clear();
                    Response.ContentType = "application/tif";
                    Response.AppendHeader("content-disposition", "attachment;filename=\"" + dt.Rows[0]["DocumentName"].ToString() + "\"");
                    Response.TransmitFile(ConfigurationManager.AppSettings["DMSTemp"].ToString() + "\\" + Convert.ToString(dt.Rows[0]["DocumentName"]));
                    Response.End();


                    // UserSession.DisplayMessage(this, "Document Splited Successfully .", MainMasterPage.MessageType.Success);
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                // return Utility.ResultType.Error;
            }

        }

        public void Split(string splittedFileName)
        {

            //Get its file information
            ImageCodecInfo codecInfo = GetCodecInfo(TIFF_CODEC);
            EncoderParameters encoderParams = new EncoderParameters(2);
            encoderParams.Param[0] = new
              EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
            encoderParams.Param[1] = new
             EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ENCODING_SCHEME);
            //Load the Document
            FileStream fs = new FileStream(this.FilePath, FileMode.OpenOrCreate);
            System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
            //Get file frame/pages
            FrameDimension frameDim = new FrameDimension(image.FrameDimensionsList[0]);
            //Check if selected pages is null or 0 value

            if (SelectedPages != null)
            //if (frompage != null)
            {
                // Delete / Overwrite existing file if updating Splitted Image File
                var file = new FileInfo(splittedFileName);
                if (file.Exists) file.Delete();
                //for each frame/pages create the new document
                for (int i = 0; i < Pages.Length; i++)
                {
                    //check whether selected pages is not greater than the file pages
                    if (Pages.Length >= (i + 1))
                    {
                        //Selected image frame
                        image.SelectActiveFrame(frameDim, Pages[i]);
                        //check whether file is new document
                        if (newfile == true)
                        {
                            image.Save(splittedFileName, codecInfo, encoderParams);
                            newfile = false;
                        }
                        else
                        //append the document depending on the selected frame from the original image
                        {
                            encoderParams.Param[0] = new
                             EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag,
                             (long)EncoderValue.FrameDimensionPage);
                            image.SaveAdd(image, encoderParams);
                        }
                    }
                }
                fs.Close();
            }
            image.Dispose();
        }

        private void MergeFilesNew(string[] fileNames, string outFile)
        {
            try
            {
                // step 1: creation of a document-object
                iTextSharp.text.Document document = new iTextSharp.text.Document();

                // step 2: we create a writer that listens to the document
                PdfCopy writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
                if (writer == null)
                {
                    return;
                }

                // step 3: we open the document
                document.Open();

                foreach (string fileName in fileNames)
                {
                    // we create a reader for a certain document
                    PdfReader reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();

                    // step 4: we add content
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }

                    PRAcroForm form = reader.AcroForm;
                    if (form != null)
                    {
                        // writer.CopyAcroForm(reader);
                        writer.AddDocument(reader);
                    }

                    reader.Close();
                }

                // step 5: we close the document and writer
                writer.Close();
                document.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                //LogManager.ErrorLog(Utility.LogFilePath, ex);
                // return Utility.ResultType.Error;
            }
        }

        private void SplitDocuments(int fromPage, int PageNo, string FilePath, DataTable dt, string FileName)
        {
            int FromPage = fromPage;
            int Topage = PageNo;
            List<int> Pagestokeep = new List<int>();
            //DataTable dt = new DataTable();
            if (FromPage > Topage)
            {
                UserSession.DisplayMessage(this, "From Page should be greater than To page", MainMasterPage.MessageType.Warning);
                return;
            }
            //dt = DMS.BusinessLogic.Document.GetDocumentPath(int.Parse(strDocumentID));
            if (Topage > Convert.ToInt32(dt.Rows[0]["PageCount"]))
            {
                UserSession.DisplayMessage(this, "Range Doesnt matches with total number of Page of Selected Document.", MainMasterPage.MessageType.Warning);
                return;
            }
            for (int i = FromPage; i <= Topage; i++)
            {
                Pagestokeep.Add(i);
            }
            string SourcePath = dt.Rows[0]["DocumentPath"].ToString();
            string DestinationPath = ConfigurationManager.AppSettings["DMSTemp"].ToString() + "\\" + Session["DocId"].ToString() + "_" + FileName + ".pdf";

            //removePagesFromPdf(SourcePath, DestinationPath, Pagestokeep.ToArray());

            ExtractPages(SourcePath, DestinationPath, FromPage, Topage);

            Response.Clear();

        }

        public void ExtractPages(string sourcePdfPath, string outputPdfPath, int startPage, int endPage)
        {
            PdfReader reader = null;
            iTextSharp.text.Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage = null;
            try
            {
                // Intialize a new PdfReader instance with the contents of the source Pdf file:
                reader = new PdfReader(sourcePdfPath);

                // For simplicity, I am assuming all the pages share the same size
                // and rotation as the first page:
                sourceDocument = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(startPage));

                // Initialize an instance of the PdfCopyClass with the source 
                // document and an output file stream:
                pdfCopyProvider = new PdfCopy(sourceDocument,
                    new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

                sourceDocument.Open();

                // Walk the specified range and add the page copies to the output file:
                for (int i = startPage; i <= endPage; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }
                sourceDocument.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlMerge_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMerge.SelectedValue == "0")
            {
                lblPageNo.Visible = false;
                txtPageRange.Visible = false;
                lblBrowse.Visible = true;
                flUpload.Visible = true;
                ibtnMerge.Visible = true;
                ibtnCancel.Visible = true;
                lblDrop.Visible = true;
                ddlMerge.Visible = true;
            }
            else if (ddlMerge.SelectedValue == "1")
            {
                lblPageNo.Visible = true;
                txtPageRange.Visible = true;
                lblBrowse.Visible = true;
                flUpload.Visible = true;
                ibtnMerge.Visible = true;
                ibtnCancel.Visible = true;
                lblDrop.Visible = true;
                ddlMerge.Visible = true;
            }
            if (ddlMerge.SelectedValue == "2")
            {
                lblPageNo.Visible = false;
                txtPageRange.Visible = false;
                lblBrowse.Visible = true;
                flUpload.Visible = true;
                ibtnMerge.Visible = true;
                ibtnCancel.Visible = true;
                lblDrop.Visible = true;
                ddlMerge.Visible = true;
            }
        }

        protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            divMerge.Visible = false;
            divSplit.Visible = false;
            txtPageRange.Text = string.Empty;
            txtRange.Text = string.Empty;
        }

        protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        {
            divMerge.Visible = false;
            divSplit.Visible = false;
            txtPageRange.Text = string.Empty;
            txtRange.Text = string.Empty;
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvwDocument.EditIndex = e.NewEditIndex;
            //this.BindGrid();
        }

        protected void OnUpdate(object sender, EventArgs e)
        {
            DMS.BusinessLogic.DocumentManager objDocManager = new DocumentManager();
            GridViewRow row = (sender as ImageButton).NamingContainer as GridViewRow;
            //string documentname = (row.Cells[2].Controls[0] as TextBox).Text;
            TextBox tx1 = (TextBox)gvwDocument.Rows[row.RowIndex].FindControl("txt_DocumentName");
            if (tx1.Text.Contains("."))
            {
                UserSession.DisplayMessage(this, "Document named should not contain extention!", MainMasterPage.MessageType.Warning);
                return;
            }
            string documentname = tx1.Text;
            int documentid = Convert.ToInt32(gvwDocument.DataKeys[row.RowIndex].Values["ID"]);
            // int DeptID = Convert.ToInt32(gvwDocument.DataKeys[row.RowIndex].Values["DeptID"]);
            //int MetatemplateID = Convert.ToInt32(gvwDocument.DataKeys[row.RowIndex].Values["MetatemplateId"]);
            string DocType = Convert.ToString(gvwDocument.DataKeys[row.RowIndex].Values["DocumentType"]);
            string OlDDocumentName = Convert.ToString(gvwDocument.DataKeys[row.RowIndex].Values["DocumentName"]);
            string NewDocumentName = documentname + DocType;

            DataSet dtset = Document.CheckDocument(NewDocumentName, emodModule.SelectedMetaTemplate);
            if (dtset.Tables[0].Rows.Count > 0)
            {
                UserSession.DisplayMessage(this, "Document Already Exists! Please Update With Another Name.", MainMasterPage.MessageType.Warning);
                return;
            }

            int UpdatedBy = UserSession.UserID;
            objUtility.Result = objDocManager.DocumentRename(documentname, documentid);

            gvwDocument.EditIndex = -1;
            gvwDocument.DataBind();
            // bindgrid();
            LoadGridDataByCriteria();
            Log.DocumentRenameLog(documentid, OlDDocumentName, NewDocumentName, HttpContext.Current);
            Log.DocumentAuditLog(HttpContext.Current, "Document Renamed", "SearchDocument", documentid);

            UserSession.DisplayMessage(this, "Document Rename Successfully.", MainMasterPage.MessageType.Success);
        }

        protected void OnCancel(object sender, EventArgs e)
        {
            gvwDocument.EditIndex = -1;
            gvwDocument.DataBind();
            //bindgrid();
            LoadGridDataByCriteria();
        }

        protected void gvwDocument_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }


    }
}