using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;

namespace DMS.Repository
{
    public partial class RepositoryView : System.Web.UI.Page
    {
        #region Private Memeber
        Utility objUtility = new Utility();
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["Type"] != null)
                {
                    if(Request["Type"].ToString().Trim()=="0")
                    {
                        Log.AuditLog(HttpContext.Current, "Repository Created", "RepositoryView");
                        UserSession.DisplayMessage(this, "Repository Is Created Successfully .", MainMasterPage.MessageType.Success);
                    }
                    else if (Request["Type"].ToString().Trim() == "1")
                    {
                        Log.AuditLog(HttpContext.Current, "Repository Updated", "RepositoryView(Updated)");
                        UserSession.DisplayMessage(this, "Repository Is Updated Successfully .", MainMasterPage.MessageType.Success);
                    }
                }
                UserSession.GridData = null;
                UserSession.IsCreateUser = 0;
                LoadGridData();

                

                if (Request["ID"] != null)
                {
                    switch (Request["ID"].ToString().Trim())
                    {
                        case "1":
                            ibtnAddNew.Visible = true;
                            gvwRepository.Columns[7].Visible = false;
                            lblTitle.Text = "Create Repository";
                            break;

                        case "2":
                             ibtnAddNew.Visible = false;
                            gvwRepository.Columns[7].Visible = true;
                            lblTitle.Text = "Update Repository";
                            break;

                        case "3":
                            gvwRepository.Columns[7].Visible = false;
                            ibtnAddNew.Visible=false;
                            lblTitle.Text = "View Repository";
                            break;
                    }
                }
                Log.AuditLog(HttpContext.Current, "Visit", "Repository View");
            }
        }

        protected void gvwRepository_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "editrepository")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.IsCreateRepository = Convert.ToInt32(gvwRepository.DataKeys[intRowIndex].Value);
                    Response.Redirect("../Repository/RepositoryCreation.aspx",false);
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwRepository_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvwRepository_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwRepository.PageIndex = e.NewPageIndex;
                    gvwRepository.DataSource = UserSession.GridData;
                    gvwRepository.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }

        }

        protected void gvwRepository_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    gvwRepository.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    gvwRepository.DataBind();
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
                UserSession.IsCreateRepository = 0;
                Response.Redirect("../Repository/RepositoryCreation.aspx", false);
            }
            catch(Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }
         #endregion

        #region Method
        private void LoadGridData()
        {
            try
            {
                RepositoryManager objRepositoryManager = new RepositoryManager();
                DataTable objDataTable = new DataTable();
                objUtility.Result = objRepositoryManager.SelectRepository(out objDataTable);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        gvwRepository.DataSource = objDataTable;
                        gvwRepository.DataBind();
                        UserSession.GridData = objDataTable;
                        break;

                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                        break;

                    case Utility.ResultType.Error:
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
        #endregion        

    }
}