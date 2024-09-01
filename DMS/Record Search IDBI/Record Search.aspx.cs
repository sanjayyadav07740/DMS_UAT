using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
using System.Collections;

namespace DMS.Record_Search_IDBI
{
    public partial class Record_Search : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        Utility objUtility = new Utility();
        DMS.BusinessLogic.MetaData objMetaData = new DMS.BusinessLogic.MetaData();
        DocumentManager objDocumentManager = new DocumentManager();

        protected void Page_Load(object sender, EventArgs e)
        {

            ((AjaxControlToolkit.ToolkitScriptManager)this.Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnShow);
            ((AjaxControlToolkit.ToolkitScriptManager)this.Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnPolicyReport);
            if (!Page.IsPostBack)
            {
                ((DropDownList)emodModule.FindControl("ddlMetaTemplateName")).SelectedIndexChanged += new EventHandler(ddlMetaTemplateName_SelectedIndexChanged);
                ((DropDownList)emodModule.FindControl("ddlRepositoryName")).SelectedIndexChanged += new EventHandler(ddlRepositoryName_SelectedIndexChanged);
            }
        }


        protected void ddlRepositoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //tdDataToSearch.Controls.Clear();
                //trDataToSearch.Visible = false;
                //trFromDate.Visible = false;
                //trToDate.Visible = false;
                //ddlField.Items.Clear();
                //ddlField.Items.Insert(0, new ListItem("--SELECT--", "-1"));
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
                //Utility.LoadField(ddlField, emodModule.SelectedMetaTemplate);
                //trDataToSearch.Visible = false;
                //trFromDate.Visible = false;
                //trToDate.Visible = false;
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


            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
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
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocument_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwDocument.PageIndex = e.NewPageIndex;
                    gvwDocument.DataSource = UserSession.GridData;
                    gvwDocument.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnShow_Click1(object sender, ImageClickEventArgs e)
        {
            LoadGridDataByCriteria();

        }

        private void LoadGridDataByCriteria()
        {
            DataTable dt = new DataTable();
          
             objMetaData.RepositoryID = emodModule.SelectedRepository;
             objMetaData.MetaTemplateID = emodModule.SelectedMetaTemplate;
             objMetaData.CategoryID = emodModule.SelectedCategory;
             objMetaData.FolderID = emodModule.SelectedFolder;
            dt = objMetaData.GetMetadatadetails();
            UserSession.GridData = dt;
            gvwDocument.DataSource = dt;
            gvwDocument.DataBind();
        }

        
        protected void ibtnPolicyReport_Click(object sender, ImageClickEventArgs e)
        {

            if (UserSession.GridData != null)
            {
                ExporttoExcel(UserSession.GridData);
            }

        }

        
        
        
        private void ExporttoExcel(DataTable dt)
        {

            if (dt.Rows.Count > 0)
            {
                string filename = "ReportExcel.xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();

                //Get the HTML for the control.
                dgGrid.RenderControl(hw);

                if (Request.IsSecureConnection || Utility.IsInternetExplorer(this.Context))
                {
                    Response.ClearHeaders();
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.Clear();
                    Response.Write(tw.ToString());
                    Response.End();
                }
                else
                {
                    this.Context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    this.Context.Response.AppendHeader(@"Pragma", @"no-cache");
                    Response.ClearHeaders();
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.Clear();
                    Response.Write(tw.ToString());
                    Response.End();
                }
            }
        }

       


    }
}
   