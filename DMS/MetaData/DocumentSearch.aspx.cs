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


namespace DMS.Shared
{
    public partial class DocumentSearch : System.Web.UI.Page
    {
        #region Private Memeber
        Utility objUtility = new Utility();
        DocumentManager objDocumentmanager = new DocumentManager();

        Document objdocument = new Document();
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
        #endregion

        #region Events
        protected override void OnInitComplete(EventArgs e)
        {
            if (!IsPostBack)
            {
                UserSession.FilterData = null;
                UserSession.MetaDataID = 0;
                FieldValue = null;
            }
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
                if (((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == -1)
                //if (emodModule.SelectedRepository != -1)
                {
                    if (((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 75 || ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]).RepositoryID == 76)
                        if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76)
                        {

                            if (e.Row.RowType == DataControlRowType.DataRow)
                            {
                                GridViewRow grv1 = gvwDocument.HeaderRow;
                                CheckBox chkHeader = (CheckBox)grv1.FindControl("chkHeader");
                                CheckBox chkChild = (e.Row.FindControl("chkChild") as CheckBox);
                                chkHeader.Visible = true;
                                chkChild.Visible = true;

                            }
                        }
                        else
                        {
                            if (e.Row.RowType == DataControlRowType.DataRow)
                            {
                                GridViewRow grv1 = gvwDocument.HeaderRow;
                                CheckBox chkHeader = (CheckBox)grv1.FindControl("chkHeader");
                                CheckBox chkChild = (e.Row.FindControl("chkChild") as CheckBox);
                                chkHeader.Visible = false;
                                chkChild.Visible = false;

                            }
                        }
                }
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
                    string Activity = "View Document";

                    string MacAddress = GetMacAddress();
                    objReport.InsertAuditLog(IPAddress, MacAddress, Activity, DocName, UserSession.UserID);
                    #endregion
                    
                    if ( ds.Tables[0].Rows[0]["RepositoryName"].ToString() == "IDBI Bank Ltd" || ds.Tables[0].Rows[0]["RepositoryName"].ToString() == "IDBI CPU")
                    {
                        //for image viewer

                        Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);

                    }
                    else if (ds.Tables[0].Rows[0]["RepositoryName"].ToString() == "Indepay Networks Pvt Ltd")
                    {
                        Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);//for image viewer
                    }
                    else if(ds.Tables[0].Rows[0]["RepositoryName"].ToString() == "MHADA")
                    {
                        if(DocName.Substring(DocName.LastIndexOf("."))==".pdf")//for pdf viewer
                        {
                            Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID + " DocumentName=" + DocName, false);
                        }
                        else
                        {
                            Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);//for image viewer
                        }
                    }
                    else
                    {
                        //for jquery viewer
                        if (ds.Tables[0].Rows[0]["RepositoryName"].ToString() == "CIDCO" || ds.Tables[0].Rows[0]["RepositoryName"].ToString() == "Fasttrack Housing Finance Limited" || ds.Tables[0].Rows[0]["RepositoryName"].ToString() == "Pratik Agarwal")
                        {
                            Response.Redirect("../MetaData/SDocumentView.aspx?", false);
                        }
                        else
                        {
                            //for pdf viewer
                            if (DocName.Substring(DocName.LastIndexOf(".")) == ".pdf")//for pdf viewer
                            {
                                Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID + " DocumentName=" + DocName, false);
                            }
                            else
                            {
                                Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);//for image viewers
                            }
                        }
                    }
                   
                }
                else if (e.CommandName.ToLower().Trim() == "renamedocument")
                {

                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                    string DocName = gvwDocument.DataKeys[intRowIndex].Values["DocumentName"].ToString().Trim();
                    HFDocID.Value = strDocumentID;
                    lbloldoc.Text = DocName;
                    txtDocname.Text = DocName;
                    GridViewDetails.Show(); 
 
                }
                else if (e.CommandName.ToLower().Trim() == "deletedocument")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                    string DocName = gvwDocument.DataKeys[intRowIndex].Values["DocumentName"].ToString().Trim();
                    lbldocname.Text = DocName+" ?";
                    HFdociddelete.Value = strDocumentID;
                    mpopDelete.Show();
                }
                else if (e.CommandName.ToLower().Trim() == "downloaddocument")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    DataTable objTable = new DataTable();
                    strDocumentID = intRowIndex.ToString();
                    objTable = DataHelper.ExecuteDataTable("SELECT DocumentName,DocumentPath,PageCount=ISNULL(PageCount,'')+ ' ' + ISNULL(MergedPageCount,'') FROM vwDocument WHERE ID = " + strDocumentID, null);


                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=\"" + objTable.Rows[0]["DocumentName"].ToString().Trim() + "\"");
                    HttpContext.Current.Response.TransmitFile(objTable.Rows[0]["DocumentPath"].ToString().Trim());
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.SuppressContent = true;
                    //Response.Clear();
                    //HttpContext.Current.Response.End();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    
                    


                    //Response.Clear();
                    //Response.ContentType = "application/octet-stream";
                    //Response.AppendHeader("Content-Disposition", "filename=" + e.CommandArgument);
                    //Response.TransmitFile(objTable.Rows[0]["DocumentPath"].ToString().Trim());
                    //Response.End();
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

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvwDocument.EditIndex = e.NewEditIndex;
            gvwDocument.DataSource = UserSession.GridData;
            gvwDocument.DataBind();

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
                Hashtable objTempList = new Hashtable();
                ibtnDownloadFiles.Visible = false;
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
                            if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76)
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
                            if (emodModule.SelectedRepository == 75 || emodModule.SelectedRepository == 76)

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

                    //old code
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
                //new code by sneha
                #region new code
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
                        throw ex;
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

        protected void btnclose_Click(object sender, ImageClickEventArgs e)
        {
            GridViewDetails.Hide();
        }

        protected void ibtnsubmitrename_Click(object sender, ImageClickEventArgs e)
        {
            DbTransaction objDbTransaction = Utility.GetTransaction;

            DocumentRename1 objrename = new DocumentRename1();
            objrename.OldDocumentName = lbloldoc.Text.Trim();
            objrename.NewDocumentName = txtDocname.Text.Trim();
            objrename.DocumentID = Convert.ToInt32(HFDocID.Value);
            objrename.CreatedBy = UserSession.UserID;
            objrename.createdOn = DateTime.Now;
            objrename.Tag = System.IO.Path.GetFileNameWithoutExtension(txtDocname.Text.Trim());


            if (this.objDocumentmanager.DocumentRename(objrename, objDbTransaction) == Utility.ResultType.Success)
            {
                objDbTransaction.Commit();
                LoadGridDataByCriteria();
            }
            else
            {
                objDbTransaction.Rollback();
            }
            
        }

        protected void ibtnYes_Click(object sender, ImageClickEventArgs e)
        {
            if(objDocumentmanager.DocumentDelete(Convert.ToInt32(HFdociddelete.Value), UserSession.UserID)==Utility.ResultType.Success)
            {
                LoadGridDataByCriteria();
            }
        }

        protected void gvwDocument_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton lb = e.Row.FindControl("ibtndownloadgrd") as ImageButton;
                ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(lb);
            }
        }




    }
}