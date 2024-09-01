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
using System.IO;
using iTextSharp.text.pdf;
using System.Text;
//using System.Data.OracleClient;
using System.Security.Cryptography;
using System.Net;
using System.Configuration;
using System.Net.NetworkInformation;

namespace DMS.Shared
{
    public partial class MergeDocumentInBetween : System.Web.UI.Page
    {
        //OracleConnection con = new OracleConnection(Utility.ConnectionString);
        string FieldId = "";
        #region Private Memeber
        SqlConnection con = new SqlConnection(Utility.ConnectionString);
        Utility objUtility = new Utility();

        DMS.BusinessLogic.Document objdocument = new DMS.BusinessLogic.Document();
        DataTable objDataTable = new DataTable();
        BusinessLogic.RepositoryManager objRepositoryManager = new BusinessLogic.RepositoryManager();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string urlName = Request.UrlReferrer.ToString();
                string url2 = HttpContext.Current.Request.Url.AbsoluteUri;
                string check = urlName.Split('/').Last();
                if (check.Contains("ViewDocumentForSearch.aspx"))
                {
                    // status = true;
                }
                if (check.Contains("ViewDocumentForSearch.aspx"))
                {
                    DdlBind();
                }
                else if (Session["CurrentUrl"] != check)
                {
                    UserSession.UserSelectionInformation = null;
                }
                else
                {
                    DdlBind();
                }


                //UserSession.MetaDataID = 0;
                //FieldValue = null;
                //ddlField.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--SELECT--", "-1"));

                //try
                //{
                //    if (UserControl.EntityModule.GetPropertiesValue("SelectedMetaTemplate") != "-1")
                //        Utility.LoadField(ddlField, Convert.ToInt32(UserControl.EntityModule.GetPropertiesValue("SelectedMetaTemplate")));
                //}
                //catch { }
                //LoadGridDataFromTemporaryList();
                //Log.AuditLog(HttpContext.Current, "Visit", "MergeDocument");
            }
            ((DropDownList)emodModule.FindControl("ddlMetaTemplateName")).SelectedIndexChanged += new EventHandler(ddlMetaTemplateName_SelectedIndexChanged);
            ((DropDownList)emodModule.FindControl("ddlRepositoryName")).SelectedIndexChanged += new EventHandler(ddlRepositoryName_SelectedIndexChanged);
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnSubmit);
            lblBrowse.Visible = false;
            flUpload.Visible = false;
            lblPageNo.Visible = false;
            txtPageRange.Visible = false;
            ibtnSubmit.Visible = false;
            ibtnCancel.Visible = false;
            lblDrop.Visible = false;
            ddlMerge.Visible = false;

        }

        public void DdlBind()
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
            Log.AuditLog(HttpContext.Current, "Visit", "MergeDocument");
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

        //protected void ddlRepositoryName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        tdDataToSearch.Controls.Clear();
        //        trDataToSearch.Visible = false;
        //        trFromDate.Visible = false;
        //        trToDate.Visible = false;
        //        ddlField.Items.Clear();
        //        ddlField.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--SELECT--", "-1"));
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

        //protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ddlField.SelectedValue != "-1")
        //        {
        //            FieldValue = ddlField.SelectedValue;
        //            if (FieldValue.Split('-')[1].Trim() != "3")
        //            {
        //                trDataToSearch.Visible = true;
        //                tdDataToSearch.Controls.Clear();
        //                Utility.GenerateControlForSearch(tdDataToSearch, ddlField.SelectedValue);
        //                trFromDate.Visible = false;
        //                trToDate.Visible = false;
        //            }
        //            else
        //            {
        //                trDataToSearch.Visible = false;
        //                trFromDate.Visible = true;
        //                trToDate.Visible = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        UserSession.DisplayMessage((Page)this.Parent.Page, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //    }
        //}

        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                Session["CurrentUrl"] = url.Split('/').Last();

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
                       // DepartmentID = emodModule.SelectedDepartMentName
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
                        MetaDataID = emodModule.SelectedMetaDataCode,
                        //DepartmentID = emodModule.SelectedDepartMentName
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
                    string strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                    Log.DocumentAuditLog(HttpContext.Current, "View Document", "MergeDocument", Convert.ToInt32(strDocumentID));
                    switch (strStatus)
                    {
                        case "1":
                            ////Response.Redirect("../MetaData/ApprovedDocument.aspx?DOCID=" + strDocumentID, false);
                            // Response.Redirect("../MetaData/ApprovedDocument_new.aspx?DOCID=" + strDocumentID, false);
                            Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID, false);
                            break;

                        case "2":
                            ////Response.Redirect("../MetaData/RejectedDocument.aspx?DOCID=" + strDocumentID, false);
                           // Log.DocumentAuditLog(HttpContext.Current, "View Document", "MergeDocument", Convert.ToInt32(strDocumentID));
                            Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID, false);
                            break;

                        case "3":
                            Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);//for image viewers
                            //Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID, false);
                            break;

                        case "4":
                            Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);//for image viewers
                           // Response.Redirect("../MetaData/ViewDocumentForSearch.aspx?DOCID=" + strDocumentID, false);
                            break;
                    }

                }
                if (e.CommandName.ToLower().Trim() == "merge")
                {
                    if (ddlMerge.SelectedValue == "0")
                    {
                        lblPageNo.Visible = false;
                        txtPageRange.Visible = false;
                        lblBrowse.Visible = true;
                        flUpload.Visible = true;
                        ibtnSubmit.Visible = true;
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
                        ibtnSubmit.Visible = true;
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
                        ibtnSubmit.Visible = true;
                        ibtnCancel.Visible = true;
                        lblDrop.Visible = true;
                        ddlMerge.Visible = true;
                    }
                    lblBrowse.Visible = true;
                    flUpload.Visible = true;
                    ibtnSubmit.Visible = true;
                    ibtnCancel.Visible = true;
                    lblDrop.Visible = true;
                    ddlMerge.Visible = true;



                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
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
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    Session["DocId"] = strDocumentID;
                    Session["Action"] = "Delete";
                }

            }

            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            #region OldCode
            //System.Data.Common.DbTransaction objDbTransaction = Utility.GetTransaction;
            //string[] inputFiles;
            //string OldFilePath = "";
            //string OldFileName = "";
            ////btnMove.Visible = true;
            ////string NewFile = "";
            //string str = "";
            //string NewFilePath = "";
            //string NewFileName = "";
            //int FileSize = 0;
            //DirectoryInfo SourceDirectory = new DirectoryInfo(Utility.DocumentPath);
            //DirectoryInfo TargetDirectory = new DirectoryInfo(Utility.DocumentArchivePath);
            //DataTable dt = new DataTable();
            //dt = DMS.BusinessLogic.Document.GetDocumentPath(Convert.ToInt32(Session["DocId"]));
            ////OldFilePath = dt.Rows[0]["DocumentPath"].ToString();
            //OldFileName = dt.Rows[0]["DocumentName"].ToString();
            // DecryptFile(dt.Rows[0]["DocumentPath"].ToString(), "D:\\Destination\\" + OldFileName);
            // OldFilePath = "D:\\Destination\\" + OldFileName;

            //FileSize = Convert.ToInt32(dt.Rows[0]["Size"].ToString());
            //str = Path.GetFileNameWithoutExtension(OldFilePath.Substring(19));
            //// str = OldFileName;

            //var filelist = SourceDirectory.GetFiles();
            ////foreach (FileInfo fleinfo in filelist)
            ////{

            //    //if (fleinfo.ToString()== (Utility.Encrypt(str) + ".pdf"))
            //    //{

            //    //    int i = 0;
            //    //    string query = "select MAX(VERSIONNO) as vNo from ARCHIVEDOCUMENT where DOCUMENTGUID like '" + Utility.DocumentArchivePath + @"\" + str + "_" + DateTime.Today.ToString("dd-MM-yyyy") + "%'";
            //    //    DataSet ds = new DataSet();
            //    //    ds = DataHelper.ExecuteDataSet(query);

            //    //    if (ds.Tables[0].Rows[0]["vNo"].ToString() != "")
            //    //    {
            //    //        i = Convert.ToInt32(ds.Tables[0].Rows[0]["vNo"].ToString());
            //    //    }
            //    //    else
            //    //        i = 0;
            //    //    fleinfo.CopyTo(Utility.DocumentArchivePath + @"\" + str + "_" + DateTime.Today.ToString("dd-MM-yyyy") + "_" + i + ".pdf", true);
            //    //    objdocument.DocumentID = Convert.ToInt32(Session["DocId"].ToString());
            //    //    objdocument.Size = Convert.ToInt32(FileSize);
            //    //    objdocument.DocumentPath = Utility.DocumentArchivePath + @"\" + str + "_" + DateTime.Today.ToString("dd-MM-yyyy") + "_" + i + ".pdf";
            //    //    objdocument.UpdatedOn = DateTime.Now;
            //    //    objdocument.UpdatedBy = UserSession.UserID;
            //    //    objdocument.DocumentGuid = str + "_" + DateTime.Today.ToString("dd-MM-yyyy") + "_" + i;
            //    //    objdocument.VersionNo = i + 1;
            //    //    objUtility.Result = DocumentManager.InsertArchiveDocs(objdocument, objDbTransaction);
            //    //    switch (objUtility.Result)
            //    //    {
            //    //        case Utility.ResultType.Failure:
            //    //        case Utility.ResultType.Error:
            //    //            objDbTransaction.Rollback();
            //    //            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            //    //            break;
            //    //    }

            //        if (flUpload.HasFile)
            //        //    NewFile = flUpload.FileName;
            //        //     string fileBasePath = Server.MapPath("~/");
            //        //    string fileName = Path.GetFileName(flUpload.FileName);
            //        //    string fullFilePath = fileBasePath + fileName;
            //        {
            //            string Filename = System.IO.Path.GetFileName(flUpload.PostedFile.FileName);
            //            flUpload.PostedFile.SaveAs(Server.MapPath("~/Others/" + flUpload.FileName));

            //            NewFilePath = Server.MapPath("~/Others/" + flUpload.FileName);
            //            NewFileName = flUpload.FileName;
            //            //if (File.Exists("D:\\Destination\\" + NewFileName))
            //            //{
            //            //    System.IO.File.Move("D:\\Destination\\" + NewFileName, "D:\\Destination\\" + NewFileName + string.Format("{0:dd MMMM yyyy HH mm ss tt }", DateTime.Now) + ".pdf");
            //            //    System.IO.File.Copy(NewFilePath, "D:\\Destination\\" + NewFileName, false);
            //            //}
            //            //else
            //            //{
            //            //    System.IO.File.Copy(NewFilePath, "D:\\Destination\\" + NewFileName, false);
            //            //}
            //        }
            //        //NewFilePath = flUpload.FileName;
            //        // NewFilePath = Server.MapPath(flUpload.FileName);
            //        //NewFileName = Path.GetFileName(flUpload.FileName);
            //        //flUpload.SaveAs(@"D:\MergeTemp\"+NewFileName);
            //        //NewFilePath = @"D:\MergeTemp\" + NewFileName;
            //        // String[] files =OldFilePath,@"D:\Destination\" + NewFileName.Split(",");
            //        if (!Directory.Exists(@"D:\Merged"))
            //        {
            //            Directory.CreateDirectory(@"D:\Merged");
            //        }
            //        string DestFilePath = @"D:\Merged\" + OldFileName;
            //        //string DestFilePath=DecryptFile()
            //        if (Session["Action"].ToString() == "Merge")
            //        {
            //            if (txtPageNo.Text == "")
            //            {
            //                String[] file = { OldFilePath, NewFilePath };
            //                MergeFiles(DestFilePath, file);
            //                //EncryptFile(objDataRow["DocumentPath"].ToString(), Utility.DocumentPath + objDataRow["DocumentGuid"].ToString());

            //            }
            //            else
            //            {
            //                string interPath1 = @"D:\Destination\inter1";
            //                string interPath2 = @"D:\Destination\inter2";
            //                string interPath3 = @"D:\Destination\inter3";
            //                PdfReader pdfReader = new PdfReader(OldFilePath);
            //                int numberOfPages = pdfReader.NumberOfPages;

            //                ExtractPages(OldFilePath, interPath1, 1, Convert.ToInt32(txtPageNo.Text));
            //                ExtractPages(OldFilePath, interPath2, Convert.ToInt32(txtPageNo.Text) + 1, numberOfPages);
            //                string[] file = { interPath1, NewFilePath };
            //                MergeFiles(interPath3, file);
            //                string[] files = { interPath3, interPath2 };
            //                MergeFiles(DestFilePath, files);

            //            }
            //            EncryptFile(DestFilePath, Utility.DocumentPath + dt.Rows[0]["DOCUMENTGUID"].ToString());
            //        }

            //        // string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
            //        //Session["DocId"] = strDocumentID;
            //        else
            //        {
            //            DeletePages(txtPageNo.Text, OldFilePath, DestFilePath, "");
            //        }
            //        FileInfo info = new FileInfo(DestFilePath);
            //        decimal fileSize = (info.Length) / 1024;
            //        PdfReader reader = new PdfReader(DestFilePath);
            //        int No_Of_Pages = reader.NumberOfPages;
            //        PdfReader reader_old = new PdfReader(OldFilePath);
            //        int No_Of_Pages_Old = reader_old.NumberOfPages;
            //        objdocument.DocumentID = Convert.ToInt32(Session["DocId"].ToString());
            //        objdocument.Size = Convert.ToInt32(fileSize);
            //        objdocument.OldPageCount = No_Of_Pages_Old;
            //        objdocument.PageCount = No_Of_Pages;
            //        objdocument.UpdatedBy = UserSession.UserID;
            //        GC.Collect();
            //        GC.WaitForPendingFinalizers();
            //        File.Delete(OldFilePath);
            //        File.Move(DestFilePath, OldFilePath);
            //        string[] filePaths = Directory.GetFiles(@"D:\Destination\");
            //        foreach (string filePath in filePaths)
            //            File.Delete(filePath);
            //        DocumentManager objDocumentManager = new DocumentManager();
            //        objUtility.Result = objDocumentManager.UpdateDocumentForMerging(objdocument, objDbTransaction);
            //        switch (objUtility.Result)
            //        {
            //            case Utility.ResultType.Success:
            //                objDbTransaction.Commit();
            //                UserSession.DisplayMessage(this, "Document Has been merged", MainMasterPage.MessageType.Success);
            //                break;
            //            case Utility.ResultType.Failure:
            //            case Utility.ResultType.Error:
            //                objDbTransaction.Rollback();
            //                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            //                break;
            //        }
            #endregion
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
                    
                    string filepaths = Path.GetDirectoryName(OldFilePath);
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
                                    Log.DocumentAuditLog(HttpContext.Current, "Document Merged", "MergeDocument", objdocument.DocumentID);
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

        protected void ddlMerge_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMerge.SelectedValue == "0")
            {
                lblPageNo.Visible = false;
                txtPageRange.Visible = false;
                lblBrowse.Visible = true;
                flUpload.Visible = true;
                ibtnSubmit.Visible = true;
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
                ibtnSubmit.Visible = true;
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
                ibtnSubmit.Visible = true;
                ibtnCancel.Visible = true;
                lblDrop.Visible = true;
                ddlMerge.Visible = true;
            }
        }

        #region Method
        //private void LoadGridDataByCriteria()
        //{
        //    try
        //    {
        //        Hashtable objTempList = new Hashtable();
        //        if (rdblSearchBy.SelectedValue == "1")
        //        {
        //            if (ddlField.SelectedValue == "-1")
        //            {
        //                UserSession.DisplayMessage(this, "Please Select The Field .", MainMasterPage.MessageType.Warning);
        //                return;
        //            }
        //            DocumentManager objDocumentManager = new DocumentManager();
        //            BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
        //            {
        //                RepositoryID = emodModule.SelectedRepository,
        //                MetaTemplateID = emodModule.SelectedMetaTemplate,
        //                CategoryID = emodModule.SelectedCategory,
        //                FolderID = emodModule.SelectedFolder,
        //                MetaDataID = emodModule.SelectedMetaDataCode
        //            };

        //            string strDataTypeID = ddlField.SelectedValue.Split('-')[1].ToString();
        //            string strEnterFieldValue = string.Empty;
        //            if (strDataTypeID.Trim() != "4" && strDataTypeID.Trim() != "9" && strDataTypeID.Trim() != "3")
        //            {
        //                strEnterFieldValue = ((TextBox)tdDataToSearch.FindControl(ddlField.SelectedValue.Split('-')[0].ToString())).Text;
        //            }
        //            else if (strDataTypeID.Trim() == "4")
        //            {
        //                strEnterFieldValue = ((DropDownList)tdDataToSearch.FindControl(ddlField.SelectedValue.Split('-')[0].ToString())).SelectedItem.Value;
        //            }
        //            else if (strDataTypeID.Trim() == "9")
        //            {
        //                CheckBoxList objCheckBoxList = ((CheckBoxList)tdDataToSearch.FindControl(ddlField.SelectedValue.Split('-')[0].ToString()));
        //                foreach (System.Web.UI.WebControls.ListItem objListItem in objCheckBoxList.Items)
        //                {
        //                    if (objListItem.Selected)
        //                        strEnterFieldValue = strEnterFieldValue + objListItem.Value + ",";
        //                }
        //                if (strEnterFieldValue != string.Empty)
        //                    strEnterFieldValue = strEnterFieldValue.Remove(strEnterFieldValue.LastIndexOf(','));

        //                string[] strArray = strEnterFieldValue.Split(',');
        //                if (strArray.Length > 0)
        //                {
        //                    strEnterFieldValue = string.Empty;
        //                    Array.Sort(strArray);
        //                }

        //                foreach (string strItem in strArray)
        //                {
        //                    strEnterFieldValue = strEnterFieldValue + strItem + ",";
        //                }
        //                if (strEnterFieldValue != string.Empty)
        //                    strEnterFieldValue = strEnterFieldValue.Remove(strEnterFieldValue.LastIndexOf(','));
        //            }

        //            BusinessLogic.DocumentEntry objDocumentEntry = new BusinessLogic.DocumentEntry()
        //            {
        //                FieldID = Convert.ToInt32(ddlField.SelectedValue.Split('-')[0].ToString()),
        //                FieldData = strEnterFieldValue,
        //                FromDate = txtFromDate.Text,
        //                ToDate = txtToDate.Text
        //            };

        //            DataTable objDataTable = new DataTable();
        //            if (strDataTypeID.Trim() != "3")
        //            {
        //                if (ddlCriteria.SelectedValue == "1")
        //                {
        //                    objUtility.Result = objDocumentManager.SelectMetaDataForSearch(out objDataTable, objMetaData, objDocumentEntry, BusinessLogic.MetaData.SearchType.EqualTo);

        //                    objTempList.Add("SEARCHBY",rdblSearchBy.SelectedValue);
        //                    objTempList.Add("METADATA", objMetaData);
        //                    objTempList.Add("DOCUMENTENTRY", objDocumentEntry);
        //                    objTempList.Add("SEARCHTYPE", BusinessLogic.MetaData.SearchType.EqualTo);
        //                    UserSession.TemporaryList = objTempList;
        //                }
        //                else if (ddlCriteria.SelectedValue == "2")
        //                {
        //                    objUtility.Result = objDocumentManager.SelectMetaDataForSearch(out objDataTable, objMetaData, objDocumentEntry, BusinessLogic.MetaData.SearchType.Like);

        //                    objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
        //                    objTempList.Add("METADATA", objMetaData);
        //                    objTempList.Add("DOCUMENTENTRY", objDocumentEntry);
        //                    objTempList.Add("SEARCHTYPE", BusinessLogic.MetaData.SearchType.Like);
        //                    UserSession.TemporaryList = objTempList;
        //                }
        //            }
        //            else
        //            {
        //                objUtility.Result = objDocumentManager.SelectMetaDataForSearch(out objDataTable, objMetaData, objDocumentEntry, BusinessLogic.MetaData.SearchType.DateRange);

        //                objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
        //                objTempList.Add("METADATA", objMetaData);
        //                objTempList.Add("DOCUMENTENTRY", objDocumentEntry);
        //                objTempList.Add("SEARCHTYPE", BusinessLogic.MetaData.SearchType.DateRange);
        //                UserSession.TemporaryList = objTempList;
        //            }

        //            switch (objUtility.Result)
        //            {
        //                case Utility.ResultType.Success:
        //                    gvwDocument.DataSource = objDataTable;
        //                    gvwDocument.DataBind();
        //                    UserSession.FilterData = null;
        //                    UserSession.GridData = objDataTable;
        //                    break;

        //                case Utility.ResultType.Failure:
        //                    gvwDocument.DataSource = objDataTable;
        //                    gvwDocument.DataBind();
        //                    UserSession.FilterData = null;
        //                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
        //                    break;

        //                case Utility.ResultType.Error:
        //                    gvwDocument.DataSource = objDataTable;
        //                    gvwDocument.DataBind();
        //                    UserSession.FilterData = null;
        //                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //                    break;
        //            }
        //        }
        //        else if (rdblSearchBy.SelectedValue == "2")
        //        {
        //            if (txtTextToSeach.Text.Trim() == string.Empty)
        //            {
        //                UserSession.DisplayMessage(this, "Please Enter The Text To Search .", MainMasterPage.MessageType.Warning);
        //                return;
        //            }
        //            DocumentManager objDocumentManager = new DocumentManager();
        //            BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
        //            {
        //                RepositoryID = emodModule.SelectedRepository,
        //                MetaTemplateID = emodModule.SelectedMetaTemplate,
        //                CategoryID = emodModule.SelectedCategory,
        //                FolderID = emodModule.SelectedFolder,
        //                MetaDataID = emodModule.SelectedMetaDataCode
        //            };

        //            BusinessLogic.Document objDocument = new BusinessLogic.Document()
        //            {
        //                Tag = txtTextToSeach.Text.Trim()
        //            };

        //            DataTable objDataTable = new DataTable();
        //            if (ddlCriteria.SelectedValue == "1")
        //            {
        //                objUtility.Result = objDocumentManager.SelectMetaDataForTagSearch(out objDataTable, objMetaData, objDocument, BusinessLogic.MetaData.SearchType.EqualTo);

        //                objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
        //                objTempList.Add("METADATA", objMetaData);
        //                objTempList.Add("DOCUMENT", objDocument);
        //                objTempList.Add("SEARCHTYPE", BusinessLogic.MetaData.SearchType.EqualTo);
        //                objTempList.Add("TEXT", txtTextToSeach.Text.Trim());
        //                UserSession.TemporaryList = objTempList;
        //            }
        //            else if (ddlCriteria.SelectedValue == "2")
        //            {
        //                objUtility.Result = objDocumentManager.SelectMetaDataForTagSearch(out objDataTable, objMetaData, objDocument, BusinessLogic.MetaData.SearchType.Like);

        //                objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
        //                objTempList.Add("METADATA", objMetaData);
        //                objTempList.Add("DOCUMENT", objDocument);
        //                objTempList.Add("SEARCHTYPE", BusinessLogic.MetaData.SearchType.Like);
        //                objTempList.Add("TEXT", txtTextToSeach.Text.Trim());
        //                UserSession.TemporaryList = objTempList;
        //            }

        //            switch (objUtility.Result)
        //            {
        //                case Utility.ResultType.Success:
        //                    gvwDocument.DataSource = objDataTable;
        //                    gvwDocument.DataBind();
        //                    gvwDocument.Visible = true;
        //                    UserSession.FilterData = null;
        //                    UserSession.GridData = objDataTable;
        //                    break;

        //                case Utility.ResultType.Failure:
        //                    gvwDocument.DataSource = objDataTable;
        //                    gvwDocument.DataBind();
        //                    UserSession.FilterData = null;
        //                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
        //                    break;

        //                case Utility.ResultType.Error:
        //                    gvwDocument.DataSource = objDataTable;
        //                    gvwDocument.DataBind();
        //                    UserSession.FilterData = null;
        //                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //                    break;
        //            }
        //        }
        //        else if (rdblSearchBy.SelectedValue == "3")
        //        {
        //            if (txtTextToSeach.Text.Trim() == string.Empty)
        //            {
        //                UserSession.DisplayMessage(this, "Please Enter The Text To Search .", MainMasterPage.MessageType.Warning);
        //                return;
        //            }
        //            string strDocumentID = string.Empty;
        //            strDocumentID = Utility.SearchPageContent(txtTextToSeach.Text.Trim().ToLower());

        //            if (strDocumentID.Trim() == string.Empty)
        //            {
        //                UserSession.DisplayMessage(this, "No Result To Display .", MainMasterPage.MessageType.Warning);
        //                return;
        //            }
        //            DocumentManager objDocumentManager = new DocumentManager();
        //            BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
        //            {
        //                RepositoryID = emodModule.SelectedRepository,
        //                MetaTemplateID = emodModule.SelectedMetaTemplate,
        //                CategoryID = emodModule.SelectedCategory,
        //                FolderID = emodModule.SelectedFolder,
        //                MetaDataID = emodModule.SelectedMetaDataCode
        //            };

        //            DataTable objDataTable = new DataTable();

        //            objUtility.Result = objDocumentManager.SelectMetaDataForContentSearch(out objDataTable, objMetaData, strDocumentID);
        //            objTempList.Add("SEARCHBY", rdblSearchBy.SelectedValue);
        //            objTempList.Add("METADATA", objMetaData);
        //            objTempList.Add("DOCUMENTID", strDocumentID);
        //            objTempList.Add("TEXT", txtTextToSeach.Text.Trim());
        //            UserSession.TemporaryList = objTempList;

        //            switch (objUtility.Result)
        //            {
        //                case Utility.ResultType.Success:
        //                    gvwDocument.DataSource = objDataTable;
        //                    gvwDocument.DataBind();
        //                    UserSession.FilterData = null;
        //                    UserSession.GridData = objDataTable;
        //                    break;

        //                case Utility.ResultType.Failure:
        //                    gvwDocument.DataSource = objDataTable;
        //                    gvwDocument.DataBind();
        //                    UserSession.FilterData = null;
        //                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
        //                    break;

        //                case Utility.ResultType.Error:
        //                    gvwDocument.DataSource = objDataTable;
        //                    gvwDocument.DataBind();
        //                    UserSession.FilterData = null;
        //                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //    }
        //}

        //private void LoadGridDataFromTemporaryList()
        //{
        //    try
        //    {
        //        if (UserSession.TemporaryList != null)
        //        {
        //            if (UserSession.TemporaryList["SEARCHBY"] != null)
        //            {
        //                DataTable objDataTable = new DataTable();
        //                DocumentManager objDocumentManager = new DocumentManager();
        //                if (UserSession.TemporaryList["SEARCHBY"].ToString().Trim() == "1")
        //                {                            
        //                    if (UserSession.TemporaryList["METADATA"] != null && UserSession.TemporaryList["DOCUMENTENTRY"] != null && UserSession.TemporaryList["SEARCHTYPE"] != null)
        //                    {
        //                        if (((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]) == BusinessLogic.MetaData.SearchType.EqualTo)
        //                            ddlCriteria.SelectedIndex = 0;
        //                        else
        //                            ddlCriteria.SelectedIndex = 1;

        //                        rdblSearchBy.SelectedIndex = 0;
        //                        objUtility.Result = objDocumentManager.SelectMetaDataForSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), ((DMS.BusinessLogic.DocumentEntry)UserSession.TemporaryList["DOCUMENTENTRY"]), ((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]));
        //                    }
        //                }
        //                else if (UserSession.TemporaryList["SEARCHBY"].ToString().Trim() == "2")
        //                {
        //                    if (UserSession.TemporaryList["METADATA"] != null && UserSession.TemporaryList["DOCUMENT"] != null && UserSession.TemporaryList["SEARCHTYPE"] != null && UserSession.TemporaryList["TEXT"] != null)
        //                    {
        //                        if (((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]) == BusinessLogic.MetaData.SearchType.EqualTo)
        //                            ddlCriteria.SelectedIndex = 0;
        //                        else
        //                            ddlCriteria.SelectedIndex = 1;

        //                        rdblSearchBy.SelectedIndex = 1;
        //                        txtTextToSeach.Text = UserSession.TemporaryList["TEXT"].ToString();
        //                        objUtility.Result = objDocumentManager.SelectMetaDataForTagSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), ((DMS.BusinessLogic.Document)UserSession.TemporaryList["DOCUMENT"]), ((BusinessLogic.MetaData.SearchType)UserSession.TemporaryList["SEARCHTYPE"]));
        //                    }
        //                }
        //                else if (UserSession.TemporaryList["SEARCHBY"].ToString().Trim() == "3")
        //                {
        //                    if (UserSession.TemporaryList["METADATA"] != null && UserSession.TemporaryList["DOCUMENTID"] != null && UserSession.TemporaryList["TEXT"] != null)
        //                    {
        //                        rdblSearchBy.SelectedIndex = 2;
        //                        txtTextToSeach.Text = UserSession.TemporaryList["TEXT"].ToString();
        //                        objUtility.Result = objDocumentManager.SelectMetaDataForContentSearch(out objDataTable, ((DMS.BusinessLogic.MetaData)UserSession.TemporaryList["METADATA"]), UserSession.TemporaryList["DOCUMENTID"].ToString().ToLower());
        //                    }
        //                }

        //                if (objUtility.Result == Utility.ResultType.Success)
        //                {
        //                    gvwDocument.DataSource = objDataTable;
        //                    gvwDocument.DataBind();
        //                    UserSession.FilterData = null;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //    }
        //}

        //public void Incrementcount()
        //{
        //     DbTransaction objDbTransaction = Utility.GetTransaction;
        //    try
        //    {
        //       // int Value = totalcount();

        //        objdocument.OutPutValuues = totalcount() + 1;
        //        objdocument.MetaDataID = UserSession.MetaDataID;
        //        objUtility.Result = DMS.BusinessLogic.Document.Increasecount(objdocument, objDbTransaction);
        //        objDbTransaction.Commit();
        //    }
        //    catch(Exception ex)
        //    {
        //        objDbTransaction.Rollback();
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //    }
        //}

        //public int totalcount()
        //{
        //    int count = 0;
        //    DbTransaction objDbTransaction = Utility.GetTransaction;
        //    try
        //    {
        //        objdocument.MetaDataID = UserSession.MetaDataID;
        //        objUtility.Result = DMS.BusinessLogic.Document.LastTotalCount(objdocument, objDbTransaction, out count);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return count;
        //}
        #endregion


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
