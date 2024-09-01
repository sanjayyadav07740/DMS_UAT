 using DMS.BusinessLogic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.Shared
{
    public partial class MergeTiffDocument : System.Web.UI.Page
    {
        string FieldId = "";
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
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnSubmit);
            lblBrowse.Visible = false;
            flUpload.Visible = false;
            //lblPageNo.Visible = false;
            //txtPageNo.Visible = false;
            ibtnSubmit.Visible = false;
            ibtnCancel.Visible = false;
            lblMergePosition.Visible = false;
            txtMergePosition.Visible = false;
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
            //try
            //{
            //    ddlTypeOfFile.Items.Clear();
            //    string str = "select a.filename as filename from file_master a inner join metatemplate_file b on a.id=b.fileid and b.metatemplateid=" + emodModule.SelectedMetaTemplate + "";
            //    DataSet ds = new DataSet();
            //    ds = DataHelper.ExecuteDataSet(str);
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        ddlTypeOfFile.DataSource = ds.Tables[0];
            //        ddlTypeOfFile.DataTextField = "filename";
            //        ddlTypeOfFile.DataBind();
            //    }
            //    ddlTypeOfFile.Items.Insert(0, "--Select--");
            //}
            //catch (Exception ex)
            //{
            //    UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            //    LogManager.ErrorLog(Utility.LogFilePath, ex);
            //}
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
                            gvwTiffDocument.DataSource = objDataTable;
                            gvwTiffDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.GridData = objDataTable;
                            break;

                        case Utility.ResultType.Failure:
                            gvwTiffDocument.DataSource = objDataTable;
                            gvwTiffDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                            break;

                        case Utility.ResultType.Error:
                            gvwTiffDocument.DataSource = objDataTable;
                            gvwTiffDocument.DataBind();
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
                            gvwTiffDocument.DataSource = objDataTable;
                            gvwTiffDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.GridData = objDataTable;
                            break;

                        case Utility.ResultType.Failure:
                            gvwTiffDocument.DataSource = objDataTable;
                            gvwTiffDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                            break;

                        case Utility.ResultType.Error:
                            gvwTiffDocument.DataSource = objDataTable;
                            gvwTiffDocument.DataBind();
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
                            gvwTiffDocument.DataSource = objDataTable;
                            gvwTiffDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.GridData = objDataTable;
                            break;

                        case Utility.ResultType.Failure:
                            gvwTiffDocument.DataSource = objDataTable;
                            gvwTiffDocument.DataBind();
                            UserSession.FilterData = null;
                            UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                            break;

                        case Utility.ResultType.Error:
                            gvwTiffDocument.DataSource = objDataTable;
                            gvwTiffDocument.DataBind();
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
                            gvwTiffDocument.DataSource = objDataTable;
                            gvwTiffDocument.DataBind();
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

        protected void gvwTiffDocument_Sorting(object sender, GridViewSortEventArgs e)
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
                        gvwTiffDocument.DataSource = UserSession.SortedFilterGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    else
                        gvwTiffDocument.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());

                    gvwTiffDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwTiffDocument_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvwTiffDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            #region try
            try
            {
                if (e.CommandName.ToLower().Trim() == "documentsearch")
                {
                    if (e.CommandName.ToLower().Trim() == "documentsearch")
                    {
                        int intRowIndex = Convert.ToInt32(e.CommandArgument);
                        UserSession.MetaDataID = Convert.ToInt32(gvwTiffDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                        string strDocumentID = gvwTiffDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                        string strStatus = gvwTiffDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                        switch (strStatus)
                        {
                            case "1":
                                Response.Redirect("../MetaData/ApprovedDocument.aspx?DOCID=" + strDocumentID, false);
                                break;

                            case "2":
                                Response.Redirect("../MetaData/RejectedDocument.aspx?DOCID=" + strDocumentID, false);
                                break;

                            case "3":
                                //Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);
                                Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);
                                break;

                            case "4":
                                Response.Redirect("../MetaData/DocumentVerification.aspx?DOCID=" + strDocumentID, false);
                                break;
                        }

                    }
                }
                if (e.CommandName.ToLower().Trim() == "merge")
                {

                    lblBrowse.Visible = true;
                    flUpload.Visible = true;
                    lblMergePosition.Visible = true;
                    txtMergePosition.Visible = true;
                    //lblPageNo.Visible = true;
                    //txtPageNo.Visible = true;
                    ibtnSubmit.Visible = true;
                    ibtnCancel.Visible = true;
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwTiffDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwTiffDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    Session["Action"] = "Merge";
                }

                if (e.CommandName.ToLower().Trim() == "deleting")
                {
                    lblBrowse.Visible = false;
                    flUpload.Visible = false;
                    //lblPageNo.Visible = true; 
                    //txtPageNo.Visible = true;
                    ibtnSubmit.Visible = true;
                    ibtnCancel.Visible = true;
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwTiffDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwTiffDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    Session["Action"] = "Delete";
                }

            }


            #endregion
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
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

        public string GetIPAddress(HttpContext context)
        {
            string ipList = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];

        }

        //private void MergeFilesNew(string[] fileNames, string outFile)
        //{
        //    try
        //    {
        //        // step 1: creation of a document-object
        //        iTextSharp.text.Document document = new iTextSharp.text.Document();

        //        // step 2: we create a writer that listens to the document

        //        PdfCopy writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
        //        if (writer == null)
        //        {
        //            return;
        //        }

        //        // step 3: we open the document
        //        document.Open();

        //        foreach (string fileName in fileNames)
        //        {
        //            // we create a reader for a certain document
        //            PdfReader reader = new PdfReader(fileName);
        //            reader.ConsolidateNamedDestinations();

        //            // step 4: we add content
        //            for (int i = 1; i <= reader.NumberOfPages; i++)
        //            {
        //                PdfImportedPage page = writer.GetImportedPage(reader, i);
        //                writer.AddPage(page);
        //            }

        //            PRAcroForm form = reader.AcroForm;
        //            if (form != null)
        //            {
        //                // writer.CopyAcroForm(reader);
        //            }

        //            reader.Close();
        //        }

        //        // step 5: we close the document and writer
        //        writer.Close();
        //        document.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //        // return Utility.ResultType.Error;
        //    }
        //}

        private void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                string password = @"myKey123"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DecryptFile(string inputFile, string outputFile)
        {

            {
                string password = @"myKey123"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
        }

        public void DeletePages(string pageRange, string SourcePdfPath, string OutputPdfPath, string Password = "")
        {
            List<int> pagesToDelete = new List<int>();
            // check for non-consecutive ranges
            if (pageRange.IndexOf(",") != -1)
            {
                string[] tmpHold = pageRange.Split(',');
                foreach (string nonconseq in tmpHold)
                {
                    // check for ranges
                    if (nonconseq.IndexOf("-") != -1)
                    {
                        string[] rangeHold = nonconseq.Split('-');
                        for (int i = Convert.ToInt32(rangeHold[0]); i <= Convert.ToInt32(rangeHold[1]); i++)
                        {
                            pagesToDelete.Add(i);
                        }
                    }
                    else
                    {
                        pagesToDelete.Add(Convert.ToInt32(nonconseq));
                    }
                }
            }
            else
            {
                // check for ranges
                if (pageRange.IndexOf("-") != -1)
                {
                    string[] rangeHold = pageRange.Split('-');
                    for (int i = Convert.ToInt32(rangeHold[0]); i <= Convert.ToInt32(rangeHold[1]); i++)
                    {
                        pagesToDelete.Add(i);
                    }
                }
                else
                {
                    pagesToDelete.Add(Convert.ToInt32(pageRange));
                }
            }

            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            PdfReader reader = new PdfReader(SourcePdfPath, new System.Text.ASCIIEncoding().GetBytes(Password));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);
                doc.Open();
                doc.AddDocListener(writer);
                for (int p = 1; p <= reader.NumberOfPages; p++)
                {
                    if (pagesToDelete.FindIndex(s => s == p) != -1)
                    {
                        continue;
                    }
                    doc.SetPageSize(reader.GetPageSize(p));
                    doc.NewPage();
                    PdfContentByte cb = writer.DirectContent;
                    PdfImportedPage pageImport = writer.GetImportedPage(reader, p);
                    int rot = reader.GetPageRotation(p);
                    if (rot == 90 || rot == 270)
                        cb.AddTemplate(pageImport, 0, -1.0F, 1.0F, 0, 0, reader.GetPageSizeWithRotation(p).Height);
                    else
                        cb.AddTemplate(pageImport, 1.0F, 0, 0, 1.0F, 0, 0);
                }
                reader.Close();
                doc.Close();
                File.WriteAllBytes(OutputPdfPath, memoryStream.ToArray());
            }
        }

        //public void MergeFiles(string destinationFile, string[] sourceFiles)
        //{
        //    if (System.IO.File.Exists(destinationFile))
        //        System.IO.File.Delete(destinationFile);

        //    string[] sSrcFile;
        //    sSrcFile = new string[2];

        //    string[] arr = new string[2];
        //    for (int i = 0; i <= sourceFiles.Length - 1; i++)
        //    {
        //        if (sourceFiles[i] != null)
        //        {
        //            if (sourceFiles[i].Trim() != "")
        //                arr[i] = sourceFiles[i].ToString();
        //        }
        //    }

        //    if (arr != null)
        //    {
        //        sSrcFile = new string[2];

        //        for (int ic = 0; ic <= arr.Length - 1; ic++)
        //        {
        //            sSrcFile[ic] = arr[ic].ToString();
        //        }
        //    }
        //    try
        //    {
        //        int f = 0;

        //        PdfReader reader = new PdfReader(sSrcFile[f]);
        //        int n = reader.NumberOfPages;
        //        // Response.Write("There are " + n + " pages in the original file.");
        //        iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4);

        //        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationFile, FileMode.Create));

        //        document.Open();
        //        PdfContentByte cb = writer.DirectContent;
        //        PdfImportedPage page;

        //        int rotation;
        //        while (f < sSrcFile.Length)
        //        {
        //            int i = 0;
        //            while (i < n)
        //            {
        //                i++;

        //                document.SetPageSize(PageSize.A4);
        //                document.NewPage();
        //                page = writer.GetImportedPage(reader, i);

        //                rotation = reader.GetPageRotation(i);
        //                if (rotation == 90 || rotation == 270)
        //                {
        //                    cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
        //                }
        //                else
        //                {
        //                    cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
        //                }
        //                //Response.Write("\n Processed page " + i);
        //            }

        //            f++;
        //            if (f < sSrcFile.Length)
        //            {
        //                reader = new PdfReader(sSrcFile[f]);
        //                n = reader.NumberOfPages;
        //                //Response.Write("There are " + n + " pages in the original file.");
        //            }
        //        }
        //        Response.Write("Success");
        //        document.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Response.Write(e.Message);
        //    }

        //}

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

        protected void gvwTiffDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwTiffDocument.PageIndex = e.NewPageIndex;
                    if (UserSession.FilterData == null)
                        gvwTiffDocument.DataSource = UserSession.GridData;
                    else
                        gvwTiffDocument.DataSource = UserSession.FilterData;

                    gvwTiffDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {            
            try
            {
                lblMergePosition.Visible = true;
                txtMergePosition.Visible = true;

                LoadGridDataByCriteria();
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {

            DataTable dt = new DataTable();
            dt = DMS.BusinessLogic.Document.GetDocumentPath(Convert.ToInt32(Session["DocId"]));
            try
            {  
                if (flUpload.HasFile)
                {
                    string Filename = Upload();

                    Dictionary<string, object> DocmangerDbTransactiondisc = MergeTiffFile(Filename, dt);

                    DocumentManager objDocumentManager = (DocumentManager)DocmangerDbTransactiondisc["objDocumentManager"];
                    DbTransaction objDbTransaction = (DbTransaction)DocmangerDbTransactiondisc["objDbTransaction"];
                    //switch (objUtility.Result)
                    //{
                    //    case Utility.ResultType.Success:
                    //        //objDbTransaction.Commit();
                    //        DMS.BusinessLogic.Report objReport = new DMS.BusinessLogic.Report();

                    //        // objUtility.Result = objDocumentManager.InsertMergedDocumentDetailsTIFF(objdocument, objDbTransaction);
                    //        objUtility.Result = objDocumentManager.InsertMergedDocumentDetails(objdocument, objDbTransaction);
                    //        switch (objUtility.Result)
                    //        {
                    //            case Utility.ResultType.Success:
                    //                objReport.InsertAuditLog(GetIPAddress(), GetMacAddress(), "Merging", objdocument.DocumentName, UserSession.UserID);
                    //                objDbTransaction.Commit();
                    //                UserSession.DisplayMessage(this, "Document has been merged", MainMasterPage.MessageType.Success);

                    //                break;
                    //            case Utility.ResultType.Error:

                    //                objDbTransaction.Rollback();
                    //                UserSession.DisplayMessage(this, "Error in InsertMergedDocumentDetailsTIFF function", MainMasterPage.MessageType.Error);
                    //                break;
                    //        }

                    //        break;
                    //    case Utility.ResultType.Failure:
                    //    case Utility.ResultType.Error:
                    //        objDbTransaction.Rollback();
                    //        UserSession.DisplayMessage(this, "Error in UpdateDocumentForMerging", MainMasterPage.MessageType.Error);
                    //        break;
                    //}
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, ex.Message + " in main Submit Button", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }
         
        }

        public string Upload()
        {
            try
            {
                string file1 = Utility.FilesPath;
                if (!Directory.Exists(file1))
                {
                    Directory.CreateDirectory(file1);
                }
                string Filename = System.IO.Path.GetFileName(flUpload.PostedFile.FileName);
                string FilePath = Utility.ArchiveFilePath + Filename;
                //flUpload.SaveAs(Server.MapPath("~/Files/" + Filename));
                flUpload.SaveAs(Utility.FilesPath + Filename);

                int bufferSize = 1024 * 1024;
                using (FileStream fileStream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    //FileStream fs = new FileStream(Server.MapPath("~/Files/" + Filename), FileMode.Open, FileAccess.ReadWrite);
                    FileStream fs = new FileStream(Utility.FilesPath + Filename, FileMode.Open, FileAccess.ReadWrite);
                    fileStream.SetLength(fs.Length);
                    int bytesRead = -1;
                    byte[] bytes = new byte[bufferSize];
                    while ((bytesRead = fs.Read(bytes, 0, bufferSize)) > 0)
                    {
                        fileStream.Write(bytes, 0, bytesRead);
                    }
                    fs.Close();
                    fs.Dispose();
                }
                return Filename;
            }
            catch(Exception ex)
            {
                UserSession.DisplayMessage(this, ex.Message+"in Upload()", MainMasterPage.MessageType.Error);
                return null;
            }
        }

        //public Dictionary<string, object> MergeTiffFile1(string Filename, DataTable dt)
        //{
        //    try
        //    {
        //        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Utility.FilesPath + Filename);

        //        //****************
        //        List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
        //        //Bitmap temp = new Bitmap(bm.Width / 2, bm.Height / 2);
        //        int flUploadpageCount = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
        //        System.Drawing.Bitmap bm1 = new System.Drawing.Bitmap(dt.Rows[0]["DocumentPath"].ToString());
        //        int total1 = bm1.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
        //        int mergePosition = Convert.ToInt32(txtMergePosition.Text.Trim());
        //        File.Copy(dt.Rows[0]["DocumentPath"].ToString(), Utility.DMSTiffBackupDocument + dt.Rows[0]["DOCUMENTGUID"], true); // Save for backup
        //        for (int k = 0; k < total1; ++k)
        //        {
        //            if (mergePosition != 0)
        //            {
        //                if (mergePosition == k)
        //                {
        //                    for (int UploadCount = 0; UploadCount < flUploadpageCount; ++UploadCount)
        //                    {
        //                        bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, UploadCount);

        //                        Bitmap temp = new Bitmap(bm.Width / 2, bm.Height / 2);
        //                        // Bitmap temp = new Bitmap(1024 , 1150);                        
        //                        Graphics gs = Graphics.FromImage(temp);
        //                        gs.InterpolationMode = InterpolationMode.NearestNeighbor;
        //                        gs.DrawImageUnscaled(bm, 0, 0);
        //                        gs.Dispose();
        //                        images.Add(temp);
        //                        //temp.Dispose();
        //                    }
        //                }
        //            }

        //            bm1.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, k);

        //            Bitmap temp1 = new Bitmap(bm1.Width / 2, bm1.Height / 2);
        //            // Bitmap temp1 = new Bitmap(1024, 1150);         
        //            Graphics g = Graphics.FromImage(temp1);
        //            g.InterpolationMode = InterpolationMode.NearestNeighbor;
        //            g.DrawImageUnscaled(bm1, 0, 0);
        //            g.Dispose();
        //            images.Add(temp1);

        //            //temp1.Dispose();
        //            if (k == total1 - 1 && mergePosition == 0)
        //            {
        //                for (int UploadCount = 0; UploadCount < flUploadpageCount; ++UploadCount)
        //                {

        //                    bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, UploadCount);

        //                    Bitmap temp = new Bitmap(bm.Width / 2, bm.Height / 2);
        //                    //Bitmap temp = new Bitmap(1024, 1150);                           
        //                    Graphics gs = Graphics.FromImage(temp);
        //                    gs.InterpolationMode = InterpolationMode.NearestNeighbor;
        //                    gs.DrawImageUnscaled(bm, 0, 0);
        //                    gs.Dispose();
        //                    images.Add(temp);
        //                    //temp.Dispose();
        //                }
        //            }
        //        }

        //        System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.SaveFlag;
        //        ImageCodecInfo encoderInfo = ImageCodecInfo.GetImageEncoders().First(i => i.MimeType == "image/tiff");
        //        EncoderParameters encoderParameters = new EncoderParameters(1);
        //        encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.MultiFrame);
        //        // Save the first frame of the multi page tiff 
        //        Bitmap firstImage = (Bitmap)images[0];
        //        firstImage = (Bitmap)images[0];
        //        bm.Dispose();
        //        bm1.Dispose();
        //        firstImage.Save(dt.Rows[0]["DocumentPath"].ToString(), encoderInfo, encoderParameters);
        //        encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.FrameDimensionPage);

        //        for (int i = 1; i < images.Count; i++)
        //        {
        //            Bitmap img = (Bitmap)images[i];
        //            firstImage.SaveAdd(img, encoderParameters);
        //            // img.Dispose();
        //        }
        //        // temp.Dispose();
        //        // Close out the file 
        //        encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.Flush);
        //        firstImage.SaveAdd(encoderParameters);
        //        System.Drawing.Image Tiff = System.Drawing.Image.FromFile(dt.Rows[0]["DocumentPath"].ToString());
        //        int No_Of_Pages = Tiff.GetFrameCount(FrameDimension.Page);
        //        Tiff.Dispose();
        //        Dictionary<string, object> disc = SaveData(images, dt, flUploadpageCount, No_Of_Pages);

        //        firstImage.Dispose();

        //        //bm.Dispose();
        //        //bm1.Dispose();
        //        return disc;
        //    }
        //    catch (Exception ex)
        //    {
        //        UserSession.DisplayMessage(this, ex.Message + "in MergeTiffFile", MainMasterPage.MessageType.Error);
        //        return null;
        //    }

        //}

        //public Dictionary<string, object> MergeTiffFile(string Filename, DataTable dt)
        //{
        //    try  
        //    { 
        //        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Utility.FilesPath + Filename);
              
        //        List<System.Drawing.Image> images = new List<System.Drawing.Image>();         
        //        int flUploadpageCount = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
        //        System.Drawing.Bitmap bm1 = new System.Drawing.Bitmap(dt.Rows[0]["DocumentPath"].ToString());
        //        int total1 = bm1.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
        //        int mergePosition = Convert.ToInt32(txtMergePosition.Text.Trim());
        //        File.Copy(dt.Rows[0]["DocumentPath"].ToString(), Utility.DMSTiffBackupDocument + dt.Rows[0]["DOCUMENTGUID"], true); // Save for backup
        //        for (int k = 0; k < total1; ++k)
        //        {
        //            if (mergePosition != 0)
        //            {
        //                if (mergePosition == k)
        //                {
        //                    for (int UploadCount = 0; UploadCount < flUploadpageCount; ++UploadCount)
        //                    {
        //                        // save each frame to a bytestream
        //                        bm.SelectActiveFrame(FrameDimension.Page, UploadCount);
        //                        MemoryStream byteStream = new MemoryStream();
        //                        bm.Save(byteStream, ImageFormat.Tiff);
        //                        images.Add(System.Drawing.Image.FromStream(byteStream));                            
        //                    } 
        //                }
        //            }

        //            // save each frame to a bytestream
        //            bm1.SelectActiveFrame(FrameDimension.Page, k);
        //            MemoryStream byteStream1 = new MemoryStream();
        //            bm1.Save(byteStream1, ImageFormat.Tiff);
        //            images.Add(System.Drawing.Image.FromStream(byteStream1));
                  
        //            if (k == total1 - 1 && mergePosition == 0)
        //            {
        //                for (int UploadCount = 0; UploadCount < flUploadpageCount; ++UploadCount)
        //                 {
        //                     // save each frame to a bytestream
        //                     bm.SelectActiveFrame(FrameDimension.Page, UploadCount);
        //                     MemoryStream byteStream = new MemoryStream();
        //                     bm.Save(byteStream, ImageFormat.Tiff);
        //                     images.Add(System.Drawing.Image.FromStream(byteStream));                          
        //                }
        //            }
        //        }
               
        //        System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.SaveFlag;
        //        ImageCodecInfo encoderInfo = ImageCodecInfo.GetImageEncoders().First(i => i.MimeType == "image/tiff");
        //        EncoderParameters encoderParameters = new EncoderParameters(1);
        //        encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.MultiFrame);
        //        // Save the first frame of the multi page tiff 
        //        Bitmap firstImage = (Bitmap)images[0];
        //        firstImage = (Bitmap)images[0];
        //        bm.Dispose();
        //        bm1.Dispose(); 
        //        firstImage.Save(dt.Rows[0]["DocumentPath"].ToString(), encoderInfo, encoderParameters);
        //        encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.FrameDimensionPage);
               
        //        for (int i = 1; i < images.Count; i++)
        //        {
        //            Bitmap img = (Bitmap)images[i];
        //            firstImage.SaveAdd(img, encoderParameters);                 
        //        }
           
        //        // Close out the file 
        //        encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.Flush);
        //        firstImage.SaveAdd(encoderParameters);
        //        System.Drawing.Image Tiff = System.Drawing.Image.FromFile(dt.Rows[0]["DocumentPath"].ToString());
        //        int No_Of_Pages = Tiff.GetFrameCount(FrameDimension.Page);
        //        Tiff.Dispose();
        //        Dictionary<string, object> disc = SaveData(images, dt, flUploadpageCount, No_Of_Pages);

        //        firstImage.Dispose();
              
        //        return disc;
        //    }
        //    catch (Exception ex)
        //    {
        //        UserSession.DisplayMessage(this, ex.Message + "in MergeTiffFile", MainMasterPage.MessageType.Error);
        //        return null;
        //    }

        //}



        public Dictionary<string, object> MergeTiffFile(string Filename, DataTable dt)
        {
            try
            {
                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(Utility.FilesPath + Filename);

                List<System.Drawing.Image> images = new List<System.Drawing.Image>();
                int flUploadpageCount = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                System.Drawing.Bitmap bm1 = new System.Drawing.Bitmap(dt.Rows[0]["DocumentPath"].ToString());
                int total1 = bm1.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                int mergePosition = Convert.ToInt32(txtMergePosition.Text.Trim());
                File.Copy(dt.Rows[0]["DocumentPath"].ToString(), Utility.DMSTiffBackupDocument + dt.Rows[0]["DOCUMENTGUID"], true); // Save for backup
                for (int k = 0; k < total1; ++k)
                {
                    if (mergePosition != 0)
                    {
                        if (mergePosition == k)
                        {
                            for (int UploadCount = 0; UploadCount < flUploadpageCount; ++UploadCount)
                            {
                                // save each frame to a bytestream
                                bm.SelectActiveFrame(FrameDimension.Page, UploadCount);
                                MemoryStream byteStream = new MemoryStream();
                                bm.Save(byteStream, ImageFormat.Tiff);
                                images.Add(System.Drawing.Image.FromStream(byteStream));
                            }
                        }
                    }

                    // save each frame to a bytestream
                    bm1.SelectActiveFrame(FrameDimension.Page, k);
                    MemoryStream byteStream1 = new MemoryStream();
                    bm1.Save(byteStream1, ImageFormat.Tiff);
                    images.Add(System.Drawing.Image.FromStream(byteStream1));

                    if (k == total1 - 1 && mergePosition == 0)
                    {
                        for (int UploadCount = 0; UploadCount < flUploadpageCount; ++UploadCount)
                        {
                            // save each frame to a bytestream
                            bm.SelectActiveFrame(FrameDimension.Page, UploadCount);
                            MemoryStream byteStream = new MemoryStream();
                            bm.Save(byteStream, ImageFormat.Tiff);
                            images.Add(System.Drawing.Image.FromStream(byteStream));
                        }
                    }
                }

                System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.SaveFlag;
                ImageCodecInfo encoderInfo = ImageCodecInfo.GetImageEncoders().First(i => i.MimeType == "image/tiff");
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.MultiFrame);
                // Save the first frame of the multi page tiff 
                Bitmap firstImage = (Bitmap)images[0];
                firstImage = (Bitmap)images[0];
                bm.Dispose();
                bm1.Dispose();
                firstImage.Save(dt.Rows[0]["DocumentPath"].ToString(), encoderInfo, encoderParameters);
                encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.FrameDimensionPage);

                for (int i = 1; i < images.Count; i++)
                {
                    Bitmap img = (Bitmap)images[i];
                    firstImage.SaveAdd(img, encoderParameters);
                }

                // Close out the file 
                encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.Flush);
                firstImage.SaveAdd(encoderParameters);
                System.Drawing.Image Tiff = System.Drawing.Image.FromFile(dt.Rows[0]["DocumentPath"].ToString());
                int No_Of_Pages = Tiff.GetFrameCount(FrameDimension.Page);
                Tiff.Dispose();
                Dictionary<string, object> disc = SaveData(images, dt, flUploadpageCount, No_Of_Pages);

                firstImage.Dispose();

                return disc;
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, ex.Message + "in MergeTiffFile", MainMasterPage.MessageType.Error);
                return null;
            }

        }



        public Dictionary<string, object> SaveData(List<System.Drawing.Image> images, DataTable dt, int flUploadpageCount, int No_Of_Pages)
        {
            try
            { 
                string OldFileName = dt.Rows[0]["DocumentName"].ToString();
                string DocGuid = dt.Rows[0]["DOCUMENTGUID"].ToString();
                int OldPageCount = Convert.ToInt32(dt.Rows[0]["PAGECOUNT"].ToString());
                string OldFilePath = dt.Rows[0]["DocumentPath"].ToString();
                FileInfo info = new FileInfo(dt.Rows[0]["DocumentPath"].ToString());
                decimal fileSize = (info.Length) / 1024;
                objdocument.DocumentID = Convert.ToInt32(Session["DocId"].ToString());
                objdocument.Size = Convert.ToInt32(fileSize);
                objdocument.OldPageCount = OldPageCount;
                objdocument.PageCount = No_Of_Pages;
                objdocument.UpdatedBy = UserSession.UserID;
                objdocument.DocumentName = OldFileName;
                objdocument.DocumentGuid = DocGuid; 
                objdocument.DocumentPath = OldFilePath;
                 objdocument.MergedBy = UserSession.UserID;
                 objdocument.MergedPageCount = No_Of_Pages - OldPageCount;
                              
                //if (dt.Rows[0]["MergedPageCount"].ToString() != "")
                //{
                //    int OriginalMergePageCount = Convert.ToInt32(dt.Rows[0]["MergedPageCount"].ToString());
                //    int TotalMergedPageCount = OriginalMergePageCount + flUploadpageCount;
                //    objdocument.MergedPageCount = TotalMergedPageCount;
                //}
                //else
                //{
                //    objdocument.MergedPageCount = flUploadpageCount;
                //}

                objdocument.IPAddress = GetIPAddress(HttpContext.Current);
                objdocument.UpdatedOn = DateTime.Now;
                objdocument.CreatedOn = DateTime.Now;
                Dictionary<string, object> disc = new Dictionary<string, object>();
                DocumentManager objDocumentManager = new DocumentManager();
                DbTransaction objDbTransaction = Utility.GetTransaction;
                objUtility.Result = objDocumentManager.UpdateDocumentForMerging(objdocument, objDbTransaction);
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        //objDbTransaction.Commit();
                        DMS.BusinessLogic.Report objReport = new DMS.BusinessLogic.Report();

                        // objUtility.Result = objDocumentManager.InsertMergedDocumentDetailsTIFF(objdocument, objDbTransaction);
                        objUtility.Result = objDocumentManager.InsertMergedDocumentDetails(objdocument, objDbTransaction);
                        switch (objUtility.Result)
                        {
                            case Utility.ResultType.Success:
                                objReport.InsertAuditLog(GetIPAddress(HttpContext.Current), GetMacAddress(), "Merging", objdocument.DocumentName, UserSession.UserID);
                                objDbTransaction.Commit();
                                UserSession.DisplayMessage(this, "Document has been merged", MainMasterPage.MessageType.Success);

                                break;
                            case Utility.ResultType.Failure:
                                objDbTransaction.Rollback();
                                break;
                            case Utility.ResultType.Error:

                                objDbTransaction.Rollback();
                                UserSession.DisplayMessage(this, "Error in InsertMergedDocumentDetailsTIFF function", MainMasterPage.MessageType.Error);
                                break;
                        }

                        break;
                    case Utility.ResultType.Failure:
                        objDbTransaction.Rollback();
                        break;
                    case Utility.ResultType.Error:
                        objDbTransaction.Rollback();
                        UserSession.DisplayMessage(this, "Error in UpdateDocumentForMerging", MainMasterPage.MessageType.Error);
                        break;
                }
                //UserSession.DisplayMessage(this, "UpdateDocumentForMerging", MainMasterPage.MessageType.Success);
                images.Clear();
                disc.Add("objDocumentManager", objDocumentManager);
                disc.Add("objDbTransaction", objDbTransaction);
                objdocument.MergedPageCount = flUploadpageCount;
               // disc.Add("objdocument",objdocument);
                return disc;
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, ex.Message, MainMasterPage.MessageType.Error);
                return null;
            }
        }

        protected void ibtnFilterGrid_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    string strFilterBy = ((DropDownList)gvwTiffDocument.FooterRow.FindControl("ddlFilterGrid")).SelectedValue.Trim();
                    string strFilterText = ((TextBox)gvwTiffDocument.FooterRow.FindControl("txtFilterGrid")).Text.Trim();
                    if (strFilterText == string.Empty)
                    {
                        gvwTiffDocument.DataSource = UserSession.GridData;
                        gvwTiffDocument.DataBind();
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
                            gvwTiffDocument.DataSource = UserSession.FilterData;
                            gvwTiffDocument.DataBind();
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

    }
}