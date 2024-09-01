using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;

namespace DMS.Audit
{
    public partial class AuditViews : System.Web.UI.Page
    {
        #region Private Memeber
        Utility objUtility = new Utility();
        #endregion
        DMS.BusinessLogic.Repository objRepository = new DMS.BusinessLogic.Repository();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {       
               
            }
        }

        private void BindGrid()
        { 
            if (ddlTypeOfdetails.SelectedItem.Value == "1")
            {
               DataTable dt = objRepository.LoadDataRepository();
               grvaudit.DataSource = dt;
               grvaudit.DataBind();
               
            }
            else if (ddlTypeOfdetails.SelectedItem.Value == "2")
            {
                DataTable dt1 = objRepository.LoadDataMetaTemplate();
                grvaudit.DataSource = dt1;
                grvaudit.DataBind();
            }
            else if (ddlTypeOfdetails.SelectedItem.Value == "3")
            {
                DataTable dt2 = objRepository.LoadDataCategory();
                grvaudit.DataSource = dt2;
                grvaudit.DataBind();
            }
            else if (ddlTypeOfdetails.SelectedItem.Value == "4")
            {
                DataTable dt3 = objRepository.LoadDataFolder();
                grvaudit.DataSource = dt3;
                grvaudit.DataBind();
            }
         }

        protected void grvaudit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
            }  
        }

        protected void grvaudit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SHOWLOG")
            {
                PnlPopUp.Visible = true;
                this.Mpextender.Show();
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string value = this.grvaudit.DataKeys[rowIndex].Values["MainID"].ToString();
                if (ddlTypeOfdetails.SelectedItem.Text == "Repository")
                {
                     DataTable dt = objRepository.LoadDataRepositoryLog(Convert.ToInt32(value));
                     grvauditReportLog.DataSource= dt;
                     grvauditReportLog.DataBind();      
                }
                else if (ddlTypeOfdetails.SelectedItem.Text == "Metatemplate")
                {
                    DataTable dt = objRepository.LoadDataMetaTemplateLog(Convert.ToInt32(value));
                    grvauditReportLog.DataSource = dt;
                    grvauditReportLog.DataBind();
                }
                else if (ddlTypeOfdetails.SelectedItem.Text == "Category")
                {
                    DataTable dt = objRepository.LoadDataCategoryLog(Convert.ToInt32(value));
                    grvauditReportLog.DataSource = dt;
                    grvauditReportLog.DataBind();
                }
                else if (ddlTypeOfdetails.SelectedItem.Text == "Folder")
                {
                    DataTable dt = objRepository.LoadDataFolderlog(Convert.ToInt32(value));
                    grvauditReportLog.DataSource = dt;
                    grvauditReportLog.DataBind();
                }
            }
        }

        protected void grvaudit_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvaudit.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void imgbtn_Click(object sender, ImageClickEventArgs e)
        {
            BindGrid();
        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            Mpextender.Hide();
            PnlPopUp.Visible = false;
        }    
    }
}