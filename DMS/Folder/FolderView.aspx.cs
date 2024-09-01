using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;

namespace DMS.Folder
{
    public partial class FolderView : System.Web.UI.Page
    {
        #region Private Member
        Utility objUtility = new Utility();
        FolderManager objFolderManager = new FolderManager();
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["Type"] != null)
                {
                    if (Request["Type"].ToString().Trim() == "0")
                    {
                        Log.AuditLog(HttpContext.Current, "Folder Created", "FolderView");
                        UserSession.DisplayMessage(this, "Folder Is Created Successfully .", MainMasterPage.MessageType.Success);
                    }
                    else if (Request["Type"].ToString().Trim() == "1")
                    {
                        Log.AuditLog(HttpContext.Current, "Folder Updated", "FolderView");
                        UserSession.DisplayMessage(this, "Folder Is Updated Successfully .", MainMasterPage.MessageType.Success);
                    }
                }

                UserSession.GridData = null;
                UserSession.IsCreateFolder = 0;

                if (Request["ID"] != null)
                {
                    switch (Request["ID"].ToString().Trim())
                    {
                        case "1":
                            ibtnAddNew.Visible = true;
                            gvwFolder.Columns[7].Visible = false;
                            lblTitle.Text = "Create Folder";
                            break;

                        case "2":
                            ibtnAddNew.Visible = false;
                            gvwFolder.Columns[7].Visible = true;
                            lblTitle.Text = "Update Folder";
                            break;

                        case "3":
                            gvwFolder.Columns[7].Visible = false;
                            ibtnAddNew.Visible = false;
                            lblTitle.Text = "View Folder";
                            break;
                    }
                }
                Log.AuditLog(HttpContext.Current, "Visit", "Folder View");
            }
            ((TreeView)emodModule.FindControl("tvwFolder")).SelectedNodeChanged += new EventHandler(tvwFolder_SelectedNodeChanged);
            if (((TreeView)emodModule.FindControl("tvwFolder")).Nodes.Count > 0)
            {
                ((TreeView)emodModule.FindControl("tvwFolder")).Nodes[0].Selected = false;
            }
        }

        protected void ibtnAddNew_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                UserSession.IsCreateFolder = 0;
                Response.Redirect("../Folder/FolderCreation.aspx", false);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void gvwFolder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwFolder.PageIndex = e.NewPageIndex;
                    gvwFolder.DataSource = UserSession.GridData;
                    gvwFolder.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwFolder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "editfolder")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.IsCreateFolder = Convert.ToInt32(gvwFolder.DataKeys[intRowIndex].Value);
                    Response.Redirect("../Folder/FolderCreation.aspx", false);
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwFolder_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvwFolder_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    gvwFolder.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    gvwFolder.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void tvwFolder_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                LoadTreeData(emodModule.SelectedFolder, emodModule.SelectedMetaTemplate);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        #endregion

        #region Method
        private void LoadTreeData(int intFolderID, int intMetaTemplateID)
        {
            try
            {
                DataTable objDataTable = new DataTable();
                objUtility.Result = FolderManager.SelectFolderByParentFolderID(out objDataTable, emodModule.SelectedFolder, emodModule.SelectedMetaTemplate);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        UserSession.GridData = objDataTable;
                        gvwFolder.DataSource = objDataTable;
                        gvwFolder.DataBind();
                        break;

                    case Utility.ResultType.Failure:
                        UserSession.GridData = null;
                        gvwFolder.DataSource = null;
                        gvwFolder.DataBind();
                        UserSession.DisplayMessage(this, "No Folder To Display At This Level .", MainMasterPage.MessageType.Warning);
                        break;

                    case Utility.ResultType.Error:
                        UserSession.GridData = null;
                        gvwFolder.DataSource = null;
                        gvwFolder.DataBind();
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        #endregion
    }
}