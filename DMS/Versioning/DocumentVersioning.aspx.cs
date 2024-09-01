using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;

namespace DMS.Versioning
{
    public partial class DocumentVersioning : System.Web.UI.Page
    {
        #region Private Memeber
        Utility objUtility = new Utility();
        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnShow);
            if (!IsPostBack)
            {
                if (Request["Type"] != null)
                {
                    if (Request["Type"].ToString().Trim() == "1")
                    {
                        UserSession.DisplayMessage(this, "Document Entry Has Been Done Successfully .", MainMasterPage.MessageType.Success);
                    }
                }
                if (Request["MetaDataCode"] != null)
                {
                    UserSession.DisplayMessage(this, "Document Are Uploaded Successfully With MetaData Code : " + Request["MetaDataCode"].ToString().Trim(), MainMasterPage.MessageType.Success);
                }

                UserSession.GridData = null;
                UserSession.MetaDataID = 0;
            }
        }
                
        protected void gvwDocumentList_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    gvwDocumentList.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    gvwDocumentList.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Utility.SetGridHoverStyle(e);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "documentview")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.MetaDataID = Convert.ToInt32(gvwDocumentList.DataKeys[intRowIndex].Values["MetaDataID"]);
                    string strDocumentID = gvwDocumentList.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                    string strStatus = gvwDocumentList.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
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
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwDocumentList.PageIndex = e.NewPageIndex;
                    gvwDocumentList.DataSource = UserSession.GridData;
                    gvwDocumentList.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnAddNew_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.Redirect("../MetaData/UploadDocuments.aspx", false);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
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

        #endregion Event

        #region Method

        private void LoadGridDataByCriteria()
        {
            try
            {
                DocumentManager objDocumentManager = new DocumentManager();
                DataTable objDataTable = new DataTable();
                if (enuModule.SelectedMetaDataCode == -1)
                {
                    gvwDocumentList.Visible = false;
                   
                    BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData()
                    {
                        RepositoryID = enuModule.SelectedRepository,
                        MetaTemplateID = enuModule.SelectedMetaTemplate,
                        CategoryID = enuModule.SelectedCategory,
                        FolderID = enuModule.SelectedFolder
                    };

                    objUtility.Result = objDocumentManager.SelectMetaDataForGrid(out objDataTable, DocumentManager.Status.TotalUploaded, objMetaData);
                }
                else
                {
                    gvwDocumentList.Visible = true;
                    objUtility.Result = objDocumentManager.SelectAllDocument(out objDataTable, enuModule.SelectedMetaDataCode, DocumentManager.Status.TotalUploaded);
                }

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        if (enuModule.SelectedMetaDataCode == -1)
                        {
                            enuModule.DataSource = objDataTable;
                            enuModule.DataBind();
                        }
                        else
                        {
                            gvwDocumentList.DataSource = objDataTable;
                            gvwDocumentList.DataBind();
                        }
                        UserSession.GridData = objDataTable;
                        break;

                    case Utility.ResultType.Failure:
                        if (enuModule.SelectedMetaDataCode == -1)
                        {
                            //gvwDocument.DataSource = objDataTable;
                            //gvwDocument.DataBind();
                        }
                        else
                        {
                            gvwDocumentList.DataSource = objDataTable;
                            gvwDocumentList.DataBind();
                        }
                        UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                        break;

                    case Utility.ResultType.Error:
                        if (enuModule.SelectedMetaDataCode == -1)
                        {
                            //gvwDocument.DataSource = objDataTable;
                            //gvwDocument.DataBind();
                        }
                        else
                        {
                            gvwDocumentList.DataSource = objDataTable;
                            gvwDocumentList.DataBind();
                        }
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        #endregion Method

        protected void gvwDocumentList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}