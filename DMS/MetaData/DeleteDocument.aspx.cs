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
using iTextSharp.text;
using System.Text;
//using System.Data.OracleClient;
using System.Security.Cryptography;
using System.Net;
using System.Net.NetworkInformation;

namespace DMS.Shared
{
    public partial class DeleteDocument : System.Web.UI.Page
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
            }
            ((DropDownList)emodModule.FindControl("ddlMetaTemplateName")).SelectedIndexChanged += new EventHandler(ddlMetaTemplateName_SelectedIndexChanged);
            ((DropDownList)emodModule.FindControl("ddlRepositoryName")).SelectedIndexChanged += new EventHandler(ddlRepositoryName_SelectedIndexChanged);
           // ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnSubmit);
           
            //lblPageNo.Visible = false;
            //txtPageNo.Visible = false;
           
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
                        string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                        switch (strStatus)
                        {
                            case "1":
                                Response.Redirect("../MetaData/ApprovedDocument.aspx?DOCID=" + strDocumentID, false);
                                break;

                            case "2":
                                Response.Redirect("../MetaData/RejectedDocument.aspx?DOCID=" + strDocumentID, false);
                                break;

                            case "3":
                                Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);
                                break;

                            case "4":
                                Response.Redirect("../MetaData/DocumentVerification.aspx?DOCID=" + strDocumentID, false);
                                break;
                        }

                    }

                    else if (e.CommandName.ToLower().Trim() == "delete")
                    {
                        int intRowIndex = Convert.ToInt32(e.CommandArgument);
                        UserSession.MetaDataID = Convert.ToInt32(gvwDocument.DataKeys[intRowIndex].Values["MetaDataID"]);
                        string strDocumentID = gvwDocument.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                        string strStatus = gvwDocument.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                       // DocumentManager objDocumentManager = new DocumentManager();
                        DataTable dt = DMS.BusinessLogic.Document.GetDocumentPath(Convert.ToInt32(strDocumentID));
                        DateTime CreatedOn = Convert.ToDateTime(dt.Rows[0]["CreatedOn"].ToString());
                        DateTime TodayDate = DateTime.Now;
                        var startDate = new DateTime(TodayDate.Year, TodayDate.Month, 1);
                        if(TodayDate==startDate)
                        {
                            UserSession.DisplayMessage(this, "Sorry.The payroll for this document has started. You are not authorized to delete this document now.", MainMasterPage.MessageType.Error);
                            return;
                        }
                        if(CreatedOn>=startDate && TodayDate>=startDate)
                        {
                            //delete the document
                        }
                        
                    }

            }

            #endregion

            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
            
    }
}
        #endregion