using DMS.BusinessLogic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.Shared
{
    public partial class SplitDocument : System.Web.UI.Page
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
        static string strDocumentID;
        #endregion

        #region Events

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
                Log.AuditLog(HttpContext.Current, "Visit", "Split Document");
            }
            ((DropDownList)emodModule.FindControl("ddlMetaTemplateName")).SelectedIndexChanged += new EventHandler(ddlMetaTemplateName_SelectedIndexChanged);
            ((DropDownList)emodModule.FindControl("ddlRepositoryName")).SelectedIndexChanged += new EventHandler(ddlRepositoryName_SelectedIndexChanged);
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnSubmit);
            lblBrowse.Visible = false;
            txtPageRange.Visible = false;
            //lblPageNo.Visible = false;
            //txtPageNo.Visible = false;
            ibtnSubmit.Visible = false;
            ibtnCancel.Visible = false;
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
                        MetaDataID = emodModule.SelectedMetaDataCode,
                        //DepartmentID = emodModule.SelectedDepartMentName
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
                        MetaDataID = emodModule.SelectedMetaDataCode,
                        //DepartmentID = emodModule.SelectedDepartMentName
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

                            for (int i = 0; i < gvwDocument.Rows.Count; i++)
                            {
                                if (!gvwDocument.Rows[i].Cells[3].Text.EndsWith(".pdf"))
                                    gvwDocument.Rows[i].Cells[9].Enabled = false;
                            }
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
                        MetaDataID = emodModule.SelectedMetaDataCode,
                       // DepartmentID = emodModule.SelectedDepartMentName
                    };

                    DataTable objDataTable = new DataTable();

                    // objUtility.Result = objDocumentManager.SelectMetaDataForContentSearch(out objDataTable, objMetaData, strDocumentID);  Commentd on 08-02-2021 

                    objUtility.Result = objDocumentManager.SelectMetaDataForContentFolderOptSearch(out objDataTable, objMetaData, strDocumentID);  // Oncliking on None folder content search code

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
                            for (int i = 0; i < gvwDocument.Rows.Count; i++)
                            {
                                if (!gvwDocument.Rows[i].Cells[3].Text.EndsWith(".pdf"))
                                    gvwDocument.Rows[i].Cells[9].Enabled = false;
                            }
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
                // UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
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
                                objUtility.Result = objDocumentManager.SelectMetaDataForContentFolderOptSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), UserSession.TemporaryList["DOCUMENTID"].ToString().ToLower());
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
            #region try
            try
            {
                if (e.CommandName.ToLower().Trim() == "documentsearch")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    string strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    string DocumentPath = gvwDocument.DataKeys[intRowIndex].Values["DocumentPath"].ToString().Trim();

                    Log.DocumentAuditLog(HttpContext.Current, "View Document", "SplitDocument", Convert.ToInt32(strDocumentID));

                    if (DocumentPath.EndsWith(".pdf"))
                        Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID, false);
                    else
                        Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);

                    //string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                    //switch (strStatus)
                    //{
                    //    case "1":
                    //        ////Response.Redirect("../MetaData/ApprovedDocument.aspx?DOCID=" + strDocumentID, false);
                    //        Response.Redirect("../MetaData/ApprovedDocument_new.aspx?DOCID=" + strDocumentID, false);
                    //        break;

                    //    case "2":
                    //        ////Response.Redirect("../MetaData/RejectedDocument.aspx?DOCID=" + strDocumentID, false);
                    //        Log.DocumentAuditLog(HttpContext.Current, "View Document", "SplitSDocument", Convert.ToInt32(strDocumentID));
                    //        Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);
                    //        break;

                    //    case "3":
                    //        ////Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);
                    //        Log.DocumentAuditLog(HttpContext.Current, "View Document", "SplitSDocument", Convert.ToInt32(strDocumentID));
                    //        Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);
                    //        break;

                    //    case "4":
                    //        ////Response.Redirect("../MetaData/DocumentVerification.aspx?DOCID=" + strDocumentID, false);
                    //        Log.DocumentAuditLog(HttpContext.Current, "View Document", "SplitSDocument", Convert.ToInt32(strDocumentID));
                    //        Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);
                    //        break;
                    //}

                }
                if (e.CommandName.ToLower().Trim() == "split")
                {

                    lblBrowse.Visible = true;
                    txtPageRange.Visible = true;
                    //lblPageNo.Visible = true;
                    //txtPageNo.Visible = true;
                    ibtnSubmit.Visible = true;
                    ibtnCancel.Visible = true;
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    Session["Action"] = "Split";
                }

                if (e.CommandName.ToLower().Trim() == "deleting")
                {
                    lblBrowse.Visible = false;
                    txtPageRange.Visible = false;
                    //lblPageNo.Visible = true;
                    //txtPageNo.Visible = true;
                    ibtnSubmit.Visible = true;
                    ibtnCancel.Visible = true;
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
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

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string[] range = txtPageRange.Text.Split('-');
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
                dt = DMS.BusinessLogic.Document.GetDocumentPath(int.Parse(strDocumentID));
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
                string DestinationPath = ConfigurationManager.AppSettings["DMSTemp"].ToString() + "\\" + strDocumentID + ".pdf";
                
                ExtractPages(SourcePath, DestinationPath, FromPage, Topage);

                //removePagesFromPdf(SourcePath, DestinationPath, Pagestokeep.ToArray());

                Log.DocumentAuditLog(HttpContext.Current, "Document Split Successfully", "SplitDocument", Convert.ToInt32(strDocumentID));
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("content-disposition", "attachment;filename=\"" + dt.Rows[0]["DocumentName"].ToString() + "\"");
                Response.TransmitFile(DestinationPath);
                Response.End();

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                // return Utility.ResultType.Error;
            }
            //}
        }

        public void removePagesFromPdf(String sourceFile, String destinationFile, int[] pagesToKeep)
        {
            //Used to pull individual pages from our source
            PdfReader r = new PdfReader(sourceFile);
            //Create our destination file
            using (FileStream fs = new FileStream(destinationFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (iTextSharp.text.Document doc = new iTextSharp.text.Document())
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, fs))
                    {
                        //Open the desitination for writing
                        doc.Open();
                        //Loop through each page that we want to keep
                        foreach (int page in pagesToKeep)
                        {
                            //Add a new blank page to destination document
                            doc.NewPage();
                            //Extract the given page from our reader and add it directly to the destination PDF
                            w.DirectContent.AddTemplate(w.GetImportedPage(r, page), 0, 0);
                        }
                        //Close our document
                        doc.Close();
                    }
                }
            }
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
                        //writer.CopyAcroForm(reader);
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
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                // return Utility.ResultType.Error;
            }
        }

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

        #endregion

    }
}