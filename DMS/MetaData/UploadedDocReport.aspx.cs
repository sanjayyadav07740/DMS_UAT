using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;
using System.IO;
using ClosedXML.Excel;

namespace DMS.Shared
{
    public partial class UploadedDocReport : System.Web.UI.Page
    {
        #region Private Memeber

        Utility objUtility = new Utility();       
        DocumentManager objDocumentManager = new DocumentManager();

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnExportData);

            if (!IsPostBack)
            {
                UserSession.GridData = null;
                UserSession.MetaDataID = 0;
                Log.AuditLog(HttpContext.Current, "Visit", "UploadedDocReport");
            }
            if (UserSession.RoleID == 1)
            {
                (emodModule.FindControl("ddlMetaDataCode") as DropDownList).Visible = false;
                (emodModule.FindControl("lblMetaDataCode") as Label).Visible = false; //Visible = false;
                (emodModule.FindControl("ddlMetaDataCode") as DropDownList).Enabled = false;
               // (emodModule.FindControl("lblMetaDataCode") as Label).Enabled = false; //Visible = false;     
            }
            else { }       
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

        protected void gvwDocumentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {               
                    gvwDocumentList.PageIndex = e.NewPageIndex;
                    gvwDocumentList.DataSource = Session["ExportData"];
                    gvwDocumentList.DataBind();
               
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {           
            try
            {
                    UploadedDocumentBind();  
              
            }
            catch (Exception ex)
            { 
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }
     
        protected void ibtnExportData_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["ExportData"];
                ExportToExcel(dt, string.Format("DocumentReport{0:dd MMMM yyyy HH mm tt}", DateTime.Now));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }        

        #endregion

        #region Method

        private void UploadedDocumentBind()
        {
            int RepositoryID = emodModule.SelectedRepository;            
            int MetatemplateId = emodModule.SelectedMetaTemplate;
           // int CategoryId = emodModule.SelectedCategory;
            int FolderID = emodModule.SelectedFolder;
            string FromDate = txtFrom.Text;
            string ToDate = txtTo.Text;
            try
            {
                DataTable objDataTable = new DataTable();
                //objUtility.Result = objDocumentManager.UploadedDcoumentReport(out objDataTable, RepositoryID,MetatemplateId,CategoryId,FolderID, FromDate, ToDate);
                objUtility.Result = objDocumentManager.UploadedDcoumentReport(out objDataTable, RepositoryID, MetatemplateId, FolderID, FromDate, ToDate);
                if (objDataTable.Rows.Count > 0)
                {
                    lbldtcount.Text = objDataTable.Rows.Count.ToString();
                    gvwDocumentList.DataSource = objDataTable;
                    Session["ExportData"] = objDataTable;
                    gvwDocumentList.DataBind();
                }
                else
                {
                    lbldtcount.Text = objDataTable.Rows.Count.ToString();
                    gvwDocumentList.DataSource = null;
                    gvwDocumentList.DataBind();
                    UserSession.DisplayMessage(this, "No Data to Display .", MainMasterPage.MessageType.Warning);
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        private void ExportToExcel(DataTable objTable, string fileName)
        {
            string attachment = "attachment; filename=" + fileName + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in objTable.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in objTable.Rows)
            {
                tab = "";
                int docid = Convert.ToInt32(dr["ID"]);
                for (i = 0; i < objTable.Columns.Count; i++)
                {
                    Response.Write(tab + (dr[i].ToString().Replace("\r\n", "")));
                    tab = "\t";
                }
                // Log.DocumentAuditLog(HttpContext.Current, "Document Download", "UploadedDocReport", docid);
                Response.Write("\n");
            }

            if (UserSession.RoleID == 1 || UserSession.RoleID == 131)
            {
                Log.AuditLog(HttpContext.Current, "Download Document Report", "UploadedDocReport");
            }
            else
            {
                DataTable copyDataTable;
                copyDataTable = objTable.Copy();
                copyDataTable.Columns.Remove("RepositoryName");
                copyDataTable.Columns.Remove("DepartmentName");
                copyDataTable.Columns.Remove("MetaTemplateName");
                copyDataTable.Columns.Remove("FolderPath");
                copyDataTable.Columns.Remove("FolderName");
                copyDataTable.Columns.Remove("MetaDataCode");
                copyDataTable.Columns.Remove("DocumentName");
                copyDataTable.Columns.Remove("Size");
                copyDataTable.Columns.Remove("PageCount");
                copyDataTable.Columns.Remove("CreatedOn");
                copyDataTable.Columns.Remove("CreatedBy");

                Log._DocumentAuditLog(HttpContext.Current, "Download Document Report", "UploadedDocReport", copyDataTable);

            }

            Response.End();
        }

        #endregion

    }
}