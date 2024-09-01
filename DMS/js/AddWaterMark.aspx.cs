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
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;

namespace DMS.Shared
{
    public partial class AddWaterMark : System.Web.UI.Page
    {
        #region Private Memeber
        Utility objUtility = new Utility();
        DMS.BusinessLogic.Document objdocument = new DMS.BusinessLogic.Document();
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
            if (!IsPostBack)
            {
                UserSession.MetaDataID = 0;
                FieldValue = null;
                ddlField.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--SELECT--", "-1"));

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
                ddlField.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--SELECT--", "-1"));
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
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].ColumnSpan = 7;
                    for (int i = 1; i < 9; i++)
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
                    Session["DocId"] = strDocumentID;
                    //for pdf viewer
                    Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID, false);
                    //for image viewer
                    // Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);
                    //switch (strStatus)
                    //{
                    //    case "1":
                    //        Response.Redirect("../MetaData/ApprovedDocument.aspx?DOCID=" + strDocumentID, false);
                    //        break;

                    //    case "2":
                    //        Response.Redirect("../MetaData/RejectedDocument.aspx?DOCID=" + strDocumentID, false);
                    //        break;

                    //    case "3":
                    //        Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);
                    //        break;

                    //    case "4":
                    //        Response.Redirect("../MetaData/DocumentVerification.aspx?DOCID=" + strDocumentID, false);
                    //        break;
                    //}
                    //Incrementcount();
                }
                if (e.CommandName == "AddWaterMark")
                {
                     int intRowIndex = Convert.ToInt32(e.CommandArgument);
                      strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                   DataTable dt=DMS.BusinessLogic.Document.GetDocumentPath(Convert.ToInt32(strDocumentID));
                     string DocPath="";
                    if(dt.Rows.Count>0)
                       DocPath=dt.Rows[0]["DocumentPath"].ToString();
                    AddWatermarkText(DocPath, @"C:\DMS\DMSDocumentArchive\" + dt.Rows[0]["DocumentName"].ToString(), "test", null, 48, null, 0.3F, 45.0F);
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

      
        
        public static void AddWatermarkText(
                string sourceFile, 
                string outputFile, 
                string watermarkText, 
                iTextSharp.text.pdf.BaseFont watermarkFont, 
              //  void =, 
//                void Nothing, 
                float watermarkFontSize, 
                //void =, 
                //void 48, 
            iTextSharp.text.BaseColor watermarkFontColor,
//                iTextSharp.text.Color watermarkFontColor, 
                //void =, 
               // void Nothing, 
                float watermarkFontOpacity, 
               // void =, 
               // void 0.3F, 
                float watermarkRotation) 
              //  void =, 
             //   void 45.0F) {
        {
        iTextSharp.text.pdf.PdfReader reader = null;
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        iTextSharp.text.pdf.PdfStamper stamper = null;
        iTextSharp.text.pdf.PdfGState gstate = null;
        iTextSharp.text.pdf.PdfContentByte underContent = null;
        iTextSharp.text.Rectangle rect = null;
        int pageCount = 0;
        try {
            reader = new iTextSharp.text.pdf.PdfReader(sourceFile);
            rect = reader.GetPageSizeWithRotation(1);
            stamper = new iTextSharp.text.pdf.PdfStamper(reader, new System.IO.FileStream(outputFile, System.IO.FileMode.Create));
            if ((watermarkFont == null)) {
                watermarkFont = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.HELVETICA, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.NOT_EMBEDDED);
            }
            
            if ((watermarkFontColor == null)) {
                watermarkFontColor = iTextSharp.text.BaseColor.BLUE;
            }
            
            gstate = new iTextSharp.text.pdf.PdfGState();
            gstate.FillOpacity = watermarkFontOpacity;
            gstate.StrokeOpacity = watermarkFontOpacity;
            pageCount = reader.NumberOfPages;
            for (int i = 1; (i <= pageCount); i++) {
                underContent = stamper.GetUnderContent(i);
                // With...
            }
            
            stamper.Close();
            reader.Close();
        }
        catch (Exception ex) {
            throw ex;
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
                    string IpAddress = GetIPAddress();
                    string Activity = "Document Renamed";
                    int UserId = UserSession.UserID;
                    DateTime DateOfActivity = DateTime.Now;
                    string DocName = Convert.ToString(Session["DocumentName"]);
                    string IPAddress = GetIPAddress();
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
        private void LoadGridDataByCriteria()
        {
            try
            {
                Hashtable objTempList = new Hashtable();
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
                        foreach (System.Web.UI.WebControls.ListItem objListItem in objCheckBoxList.Items)
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
                            gvwDocument.DataBind();
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
                            gvwDocument.DataBind();
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
                else if (rdblSearchBy.SelectedValue == "3")
                {
                    if (txtTextToSeach.Text.Trim() == string.Empty)
                    {
                        UserSession.DisplayMessage(this, "Please Enter The Text To Search .", MainMasterPage.MessageType.Warning);
                        return;
                    }
                    string strDocumentID = string.Empty;
                    strDocumentID = Utility.SearchPageContent(txtTextToSeach.Text.Trim().ToLower());

                    if (strDocumentID.Trim() == string.Empty)
                    {
                        UserSession.DisplayMessage(this, "No Result To Display .", MainMasterPage.MessageType.Warning);
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

                    objUtility.Result = objDocumentManager.SelectMetaDataForContentSearch(out objDataTable, objMetaData, strDocumentID);
                    objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
                    objTempList.Add("METADATA", objMetaData);
                    objTempList.Add("DOCUMENTID", strDocumentID);
                    objTempList.Add("TEXT", txtTextToSeach.Text.Trim());
                    UserSession.TemporaryList = objTempList;

                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();
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
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
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

                                rdblSearchBy.SelectedIndex = 0;
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

                                rdblSearchBy.SelectedIndex = 1;
                                txtTextToSeach.Text = UserSession.TemporaryList["TEXT"].ToString();
                                objUtility.Result = objDocumentManager.SelectMetaDataForTagSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), ((DMS.BusinessLogic.Document)UserSession.TemporaryList["DOCUMENT"]), ((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]));
                            }
                        }
                        else if (UserSession.TemporaryList["SEARCHBY"].ToString().Trim() == "3")
                        {
                            if (UserSession.TemporaryList["METADATA"] != null && UserSession.TemporaryList["DOCUMENTID"] != null && UserSession.TemporaryList["TEXT"] != null)
                            {
                                rdblSearchBy.SelectedIndex = 2;
                                txtTextToSeach.Text = UserSession.TemporaryList["TEXT"].ToString();
                                objUtility.Result = objDocumentManager.SelectMetaDataForContentSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), UserSession.TemporaryList["DOCUMENTID"].ToString().ToLower());
                            }
                        }

                        if (objUtility.Result == Utility.ResultType.Success)
                        {
                            gvwDocument.DataSource = objDataTable;
                            gvwDocument.DataBind();
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
                objUtility.Result = DMS.BusinessLogic.Document.Increasecount(objdocument, objDbTransaction);
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
                objUtility.Result = DMS.BusinessLogic.Document.LastTotalCount(objdocument, objDbTransaction, out count);

            }
            catch (Exception ex)
            {

            }
            return count;
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

    }
}