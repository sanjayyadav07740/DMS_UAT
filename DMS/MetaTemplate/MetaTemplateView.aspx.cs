using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;

namespace DMS.MetaTemplate
{
    public partial class MetaTemplateView : System.Web.UI.Page
    {
        #region Private Member
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
                        UserSession.DisplayMessage(this, "MetaTemplate IS  Created Successfully .", MainMasterPage.MessageType.Success);
                    }
                    else if (Request["Type"].ToString().Trim() == "1")
                    {
                        UserSession.DisplayMessage(this, "MetaTemplate  Updated Successfully .", MainMasterPage.MessageType.Success);
                    }
                    else if (Request["Type"].ToString().Trim() == "NoListItem")
                    {
                        UserSession.DisplayMessage(this, "List Type Of Fields Are  Not Available .", MainMasterPage.MessageType.Success);
                    }
                }

                if (Request["ID"] != null)
                {
                    switch (Request["ID"].ToString().Trim())
                    {
                        case "1":
                            ibtnMetaTemplateCreate.Visible = true;
                            gvwMetaTemplate.Columns[8].Visible = false;
                            gvwMetaTemplate.Columns[9].Visible = false;
                            gvwMetaTemplate.Columns[10].Visible = false;
                            lblTitle.Text = "Create MetaTemplate";
                            break;

                        case "2":
                            ibtnMetaTemplateCreate.Visible = false;
                            gvwMetaTemplate.Columns[8].Visible = true;
                            gvwMetaTemplate.Columns[9].Visible = true;
                            gvwMetaTemplate.Columns[10].Visible = false;
                            lblTitle.Text = "Update MetaTemplate";
                            break;

                        case "3":
                            gvwMetaTemplate.Columns[8].Visible = false;
                            gvwMetaTemplate.Columns[9].Visible = false;
                            gvwMetaTemplate.Columns[10].Visible = false;
                            ibtnMetaTemplateCreate.Visible = false;
                            lblTitle.Text = "View MetaTemplate";
                            break;
                    }
                }
                try
                {
                    if (UserControl.EntityModule.GetPropertiesValue("SelectedRepository") != "-1")
                    {
                        LoadGridData(Convert.ToInt32(UserControl.EntityModule.GetPropertiesValue("SelectedRepository")));
                    }
                }
                catch { }
                Log.AuditLog(HttpContext.Current, "Visit", "MetaTemplate View");
            }
            ((DropDownList)emodModule.FindControl("ddlRepositoryName")).SelectedIndexChanged += new EventHandler(ddlRepositoryName_SelectedIndexChanged);
        }

        protected void ibtnMetaTemplateCreate_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                UserSession.IsCreateMetaTemplate = 0;
                Response.Redirect("../MetaTemplate/MetaTemplateCreation.aspx", false);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void gvwMetaTemplate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Trim() == "editmetatemplate")
            {
                try
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.IsCreateMetaTemplate = Convert.ToInt32(gvwMetaTemplate.DataKeys[intRowIndex].Value);
                    Response.Redirect("../MetaTemplate/MetaTemplateCreation.aspx", false);
                }
                catch (Exception ex)
                {
                    LogManager.ErrorLog(Utility.LogFilePath, ex);
                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                }
            }
            else if (e.CommandName.ToLower().Trim() == "editmetatemplatefields")
            {
                try
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    //UserSession.IsCreateMetaTemplateField = Convert.ToInt32(gvwMetaTemplate.DataKeys[intRowIndex].Value);
                    UserSession.MetatemplateID = Convert.ToInt32(gvwMetaTemplate.DataKeys[intRowIndex].Value);
                    UserSession.RepositoryID=Convert.ToInt32(emodModule.SelectedRepository);                    
                    Response.Redirect("../MetaTemplate/MetaTemplateFieldCreation.aspx", false);
                }
                catch (Exception ex)
                {
                    LogManager.ErrorLog(Utility.LogFilePath, ex);
                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                }
            }
            else if (e.CommandName.ToLower().Trim() == "editlistitemfields")
            {
                try
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    //UserSession.IsCreateMetaTemplateField = Convert.ToInt32(gvwMetaTemplate.DataKeys[intRowIndex].Value);
                    UserSession.MetatemplateID = Convert.ToInt32(gvwMetaTemplate.DataKeys[intRowIndex].Value);
                    //UserSession.RepositoryID = Convert.ToInt32(ddlRepositoryName.SelectedItem.Value);
                    Response.Redirect("../MetaTemplate/MetaTemplateListItems.aspx", false);
                }
                catch (Exception ex)
                {
                    LogManager.ErrorLog(Utility.LogFilePath, ex);
                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                }
            }
        }

        protected void gvwMetaTemplate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwMetaTemplate.PageIndex = e.NewPageIndex;
                    gvwMetaTemplate.DataSource = UserSession.GridData;
                    gvwMetaTemplate.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ddlRepositoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadGridData(Convert.ToInt32(emodModule.SelectedRepository));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void gvwMetaTemplate_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    gvwMetaTemplate.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    gvwMetaTemplate.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwMetaTemplate_RowDataBound(object sender, GridViewRowEventArgs e)
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
        #endregion 

        #region Method
        private void LoadGridData(int intRepositoryID)
        {
            try
            {
                MetaTemplateManager objRepositoryManager = new MetaTemplateManager();
                DataTable objDataTable = new DataTable();
                objUtility.Result = objRepositoryManager.SelectMetaTemplateByRepositoryID(out objDataTable, intRepositoryID);
                if (objUtility.Result == Utility.ResultType.Success)
                {
                    gvwMetaTemplate.DataSource = objDataTable;
                    gvwMetaTemplate.DataBind();
                    UserSession.GridData = objDataTable;
                }
                else if (objUtility.Result == Utility.ResultType.Failure)
                {
                    gvwMetaTemplate.DataSource = null;
                    gvwMetaTemplate.DataBind();
                    UserSession.GridData = null;
                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                }
                else if (objUtility.Result == Utility.ResultType.Error)
                {
                    gvwMetaTemplate.DataSource = null;
                    gvwMetaTemplate.DataBind();
                    UserSession.GridData = null;
                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
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