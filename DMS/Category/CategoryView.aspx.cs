using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;

namespace DMS.Category
{
    public partial class CategoryView : System.Web.UI.Page
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
                    if (Request["Type"].ToString().Trim() == "0")
                    {
                        Log.AuditLog(HttpContext.Current, "Category Created", "CategoryView");
                        UserSession.DisplayMessage(this, "Category Is Created Successfully .", MainMasterPage.MessageType.Success);
                    }
                    else if (Request["Type"].ToString().Trim() == "1")
                    {
                        Log.AuditLog(HttpContext.Current, "Category Updated", "CategoryView(Updated)");
                        UserSession.DisplayMessage(this, "Category Is Updated Successfully .", MainMasterPage.MessageType.Success);
                    }
                }

                UserSession.GridData = null;
                UserSession.IsCreateCategory = 0;


                if (Request["ID"] != null)
                {
                    switch (Request["ID"].ToString().Trim())
                    {
                        case "1":
                            ibtnAddNew.Visible = true;
                            gvwCategory.Columns[7].Visible = false;
                            lblTitle.Text = "Create Category";
                            break;

                        case "2":
                            ibtnAddNew.Visible = false;
                            gvwCategory.Columns[7].Visible = true;
                            lblTitle.Text = "Update Category";
                            break;

                        case "3":
                            gvwCategory.Columns[7].Visible = false;
                            ibtnAddNew.Visible = false;
                            lblTitle.Text = "View Category";
                            break;
                    }
                }
                try
                {
                    if (UserControl.EntityModule.GetPropertiesValue("SelectedMetaTemplate") != "-1")
                    {
                        LoadGridData(Convert.ToInt32(UserControl.EntityModule.GetPropertiesValue("SelectedMetaTemplate")));
                    }
                }
                catch { }
                Log.AuditLog(HttpContext.Current, "Visit", "Category View");
            }
            ((DropDownList)emodModule.FindControl("ddlMetaTemplateName")).SelectedIndexChanged += new EventHandler(ddlMetaTemplateName_SelectedIndexChanged);
        }

        protected void ibtnAddNew_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                UserSession.IsCreateCategory = 0;
                Response.Redirect("../Category/CategoryCreation.aspx", false);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void gvwCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwCategory.PageIndex = e.NewPageIndex;
                    gvwCategory.DataSource = UserSession.GridData;
                    gvwCategory.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "editcategory")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.IsCreateCategory = Convert.ToInt32(gvwCategory.DataKeys[intRowIndex].Value);
                    Response.Redirect("../Category/CategoryCreation.aspx", false);
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwCategory_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvwCategory_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    gvwCategory.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    gvwCategory.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ddlMetaTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadGridData(emodModule.SelectedMetaTemplate);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        #endregion

        #region Method
        private void LoadGridData(int intMetaTemplateID)
        {
            try
            {
                CategoryManager objCategoryManager = new CategoryManager();
                DataTable objDataTable = new DataTable();
                objUtility.Result = objCategoryManager.SelectCategory(out objDataTable, intMetaTemplateID);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        gvwCategory.DataSource = objDataTable;
                        gvwCategory.DataBind();
                        UserSession.GridData = objDataTable;
                        break;

                    case Utility.ResultType.Failure:
                        gvwCategory.DataSource = null;
                        gvwCategory.DataBind();
                        UserSession.GridData = null;
                        UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                        break;

                    case Utility.ResultType.Error:
                        gvwCategory.DataSource = null;
                        gvwCategory.DataBind();
                        UserSession.GridData = null;
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