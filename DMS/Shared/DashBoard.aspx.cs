using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;

namespace DMS.Shared
{
    public partial class DashBoard : System.Web.UI.Page
    {
        #region Private Memeber
        Utility objUtility = new Utility();
        string RepositoryLevel = "RepositoryLevel";
        string MetaTemplateLevel = "MetaTemplateLevel";
        string MetaDataLevel = "MetaDataLevel";
        string DocumentStatusLevel = "DocumentStatus";
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                Session[RepositoryLevel] = null;
                Session[MetaTemplateLevel] = null;
                Session[MetaDataLevel] = null;
                Session[DocumentStatusLevel] = null;
                LoadGridData(BusinessLogic.MetaData.DashBoard.RepositoryLevel);
            }
            if (Session[RepositoryLevel] != null)
            {
                if (((DataTable)Session[RepositoryLevel]).Rows.Count != 0)
                {
                    chtRepositoryLevel.Visible = true;
                    chtRepositoryLevel.DataSource = (DataTable)Session[RepositoryLevel];
                    chtRepositoryLevel.DataBind();
                }
                else
                    chtRepositoryLevel.Visible = false;
            }
            else
            {
                chtRepositoryLevel.Visible = false;
            }
            if (Session[MetaTemplateLevel] != null)
            {
                if (((DataTable)Session[MetaTemplateLevel]).Rows.Count != 0)
                {
                    chtMetaTemplateLevel.Visible = true;
                    chtMetaTemplateLevel.DataSource = (DataTable)Session[MetaTemplateLevel];
                    chtMetaTemplateLevel.DataBind();
                }
                else
                    chtMetaTemplateLevel.Visible = false;
            }
            else
            {
                chtMetaTemplateLevel.Visible = false;
            }
            if (Session[DocumentStatusLevel] != null)
            {
                if (((DataTable)Session[DocumentStatusLevel]).Rows.Count != 0)
                {
                    chtMetaDataLevel.Visible = true;
                    chtMetaDataLevel.DataSource = (DataTable)Session[DocumentStatusLevel];
                    chtMetaDataLevel.DataBind();
                }
                else
                    chtMetaDataLevel.Visible = false;
            }
            else
            {
                chtMetaDataLevel.Visible = false;
            }
            Log.AuditLog(HttpContext.Current, "Visit", "DashBoard");
        }

        protected void gvwDocumentRepositoryLevel_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (Session[RepositoryLevel] != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";
                    DataView objDataView = new DataView((DataTable)Session[RepositoryLevel]);
                    objDataView.Sort = e.SortExpression + " " + ViewState[UserSession.SortExpression];
                    gvwDocumentRepositoryLevel.DataSource = objDataView;
                    gvwDocumentRepositoryLevel.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentRepositoryLevel_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvwDocumentRepositoryLevel_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DocumentManager objDocumentManager = new DocumentManager();
            BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData();
            DataTable objDataTable = new DataTable();
            try
            {
                if (e.CommandName.ToLower().Trim() == "repositorylevel")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    objMetaData.RepositoryID = Convert.ToInt32(gvwDocumentRepositoryLevel.DataKeys[intRowIndex].Value);
                    objUtility.Result = objDocumentManager.SelectDocumentDataForDashBoard(out objDataTable, BusinessLogic.MetaData.DashBoard.MetaTemplateLevel, objMetaData);
                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Error:
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                    }
                    gvwDocumentMetaTemplateLevel.DataSource = objDataTable;
                    gvwDocumentMetaTemplateLevel.DataBind();
                    chtMetaTemplateLevel.Visible = true;
                    chtMetaTemplateLevel.DataSource = objDataTable;
                    chtMetaTemplateLevel.DataBind();

                    if (objDataTable.Rows.Count != 0)
                    {
                        lblRepositoryName.Text = gvwDocumentRepositoryLevel.DataKeys[intRowIndex].Values["RepositoryName"].ToString();
                        lblRepositoryName.Visible = true;
                    }
                    else
                    {
                        lblRepositoryName.Visible = false;
                    }

                    Session[MetaTemplateLevel] = objDataTable;
                    Session[DocumentStatusLevel] = null;
                    Session[MetaDataLevel] = null;
                    lblMetaDataCode.Visible = false;
                    gvwDocumentMetaDataLevel.Visible = false;
                    chtMetaDataLevel.Visible = false;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentRepositoryLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (Session[RepositoryLevel] != null)
                {
                    gvwDocumentRepositoryLevel.PageIndex = e.NewPageIndex;
                    gvwDocumentRepositoryLevel.DataSource = (DataTable)Session[RepositoryLevel];
                    gvwDocumentRepositoryLevel.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentMetaTemplateLevel_Sorting(object sender, GridViewSortEventArgs e)
        {
            
            try
            {
                if (Session[MetaTemplateLevel] != null)
                {

                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    DataView objDataView = new DataView((DataTable)Session[MetaTemplateLevel]);
                    objDataView.Sort = e.SortExpression + " " + ViewState[UserSession.SortExpression].ToString();
                    gvwDocumentMetaTemplateLevel.DataSource = objDataView;
                    gvwDocumentMetaTemplateLevel.DataBind();
                    chtMetaTemplateLevel.DataSource = (DataTable)Session[MetaTemplateLevel];
                    chtMetaTemplateLevel.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentMetaTemplateLevel_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvwDocumentMetaTemplateLevel_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DocumentManager objDocumentManager = new DocumentManager();
            BusinessLogic.MetaData objMetaData = new BusinessLogic.MetaData();
            DataTable objDataTable = new DataTable();
            try
            {
               
                if (e.CommandName.ToLower().Trim() == "metatemplatelevel")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    objMetaData.MetaDataID = Convert.ToInt32(gvwDocumentMetaTemplateLevel.DataKeys[intRowIndex].Value);
                    objUtility.Result = objDocumentManager.SelectDocumentDataForDashBoard(out objDataTable, BusinessLogic.MetaData.DashBoard.MetaDataLevel, objMetaData);
                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Error:
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                    }
                    gvwDocumentMetaDataLevel.Visible = true;
                    gvwDocumentMetaDataLevel.DataSource = objDataTable;
                    gvwDocumentMetaDataLevel.DataBind();
                    Session[MetaDataLevel] = objDataTable;
                    if (objDataTable.Rows.Count != 0)
                    {
                        lblMetaDataCode.Text = gvwDocumentMetaTemplateLevel.DataKeys[intRowIndex].Values["MetaDataCode"].ToString();
                        lblMetaDataCode.Visible = true;
                    }
                    else
                    {
                        lblMetaDataCode.Visible = false;
                    }

                    objUtility.Result = objDocumentManager.SelectDocumentDataForDashBoard(out objDataTable, BusinessLogic.MetaData.DashBoard.DocumentStatusLevel, objMetaData);
                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Error:
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                    }
                    chtMetaDataLevel.Visible = true;
                    chtMetaDataLevel.DataSource = objDataTable;
                    chtMetaDataLevel.DataBind();
                    Session[DocumentStatusLevel] = objDataTable;

                    if (Session[MetaTemplateLevel] != null)
                    {
                        chtMetaTemplateLevel.Visible = true;
                        chtMetaTemplateLevel.DataSource = (DataTable)Session[MetaTemplateLevel];
                        chtMetaTemplateLevel.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentMetaTemplateLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                
                if (Session[MetaTemplateLevel] != null)
                {
                    gvwDocumentMetaTemplateLevel.PageIndex = e.NewPageIndex;
                    gvwDocumentMetaTemplateLevel.DataSource = (DataTable)Session[MetaTemplateLevel];
                    gvwDocumentMetaTemplateLevel.DataBind();
                    chtMetaTemplateLevel.DataSource = (DataTable)Session[MetaTemplateLevel]; 
                    chtMetaTemplateLevel.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentMetaDataLevel_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (Session[MetaDataLevel] != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    
                    DataView objDataView = new DataView((DataTable)Session[MetaDataLevel]);
                    objDataView.Sort = e.SortExpression + " " + ViewState[UserSession.SortExpression].ToString();
                    gvwDocumentMetaDataLevel.DataSource = objDataView;
                    gvwDocumentMetaDataLevel.DataBind();
                    if (Session[DocumentStatusLevel] != null)
                    {
                        chtMetaDataLevel.DataSource = (DataTable)Session[DocumentStatusLevel];
                        chtMetaDataLevel.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentMetaDataLevel_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvwDocumentMetaDataLevel_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        protected void gvwDocumentMetaDataLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (Session[MetaDataLevel] != null)
                {
                    gvwDocumentMetaDataLevel.PageIndex = e.NewPageIndex;
                    gvwDocumentMetaDataLevel.DataSource = (DataTable)Session[MetaDataLevel];
                    gvwDocumentMetaDataLevel.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        #endregion

        #region Method
        private void LoadGridData(BusinessLogic.MetaData.DashBoard enumDashBoard)
        {
            try
            {
                DocumentManager objDocumentManager = new DocumentManager();
                DataTable objDataTable = new DataTable();
                objUtility.Result = objDocumentManager.SelectDocumentDataForDashBoard(out objDataTable, enumDashBoard,null);
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        return;
                }
                gvwDocumentRepositoryLevel.DataSource = objDataTable;
                gvwDocumentRepositoryLevel.DataBind();
                Session[RepositoryLevel] = objDataTable;
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

