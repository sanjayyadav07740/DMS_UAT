using DMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.Audit
{
    public partial class Audit_Report : System.Web.UI.Page
    { 
        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC"; 
        DocumentManager objDocumentManager = new DocumentManager();
        SqlConnection CON = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        Utility objUtility = new Utility();
        BusinessLogic.User user = new BusinessLogic.User();
        protected void Page_Load(object sender, EventArgs e)
        { 
            try
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnExport);
                // ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnExport1);
                scriptManager.RegisterPostBackControl(this.btnHitCountExp);
               // rdobtnMultipleUser.Checked = true;
                if (!IsPostBack)
                {
                    Log.AuditLog(HttpContext.Current, "Visit", "Audit Report");
                   
                    Session["dtTable"] = null;
                    ddll_Username();
                }

                if (rdobtnSingleUser.Checked)
                {
                    gvAuditReports.Visible = true;
                    gvPageCount.Visible = false;
                    gvAuditReports.DataSource = null;
                    gvAuditReports.DataBind();
                    rdobtnMultipleUser.Checked = false;
                   // rdobtnCountVisit.Checked = false;
                    lblUserName.Visible = true;
                    ddlUserViewer.Visible = true;
                    //gvAuditReports1.Visible = true;
                    //gvAuditReports1.Visible = false;
                    btnSub.Visible = false;
                    btnSub1.Visible = true;
                    btnSubmit1.Visible = true;
                    //btnExport1.Visible = true;
                    btnSubmit.Visible = false;
                    btnExport.Visible = false;
                    btnSub2.Visible = false;
                    btnHitCountExp.Visible = false;
                    btnSub4.Visible = false;
                    btnExportMultiple.Visible = false;
                    btnExportsingle.Visible = false;
                    //txtFrom.Text = string.Empty;
                    //txtTo.Text = string.Empty;
                }
                else if (rdobtnMultipleUser.Checked)
                {
                    gvAuditReports.Visible = true;
                    gvPageCount.Visible = false;
                    gvAuditReports.DataSource = null;
                    gvAuditReports.DataBind();
                    ////pagecountresult.Visible = false;
                    lblUserName.Visible = false;
                    rdobtnSingleUser.Checked = false;
                    ddlUserViewer.Visible = false;
                    //gvAuditReports1.Visible = false;
                    //gvAuditReports1.Visible = true;
                    btnSubmit1.Visible = false;
                    btnExport1.Visible = false;
                    btnSubmit.Visible = true;
                    //btnExport.Visible = true;
                    btnSub1.Visible = false;
                    btnSub.Visible = true;
                    btnSub2.Visible = false;
                    btnHitCountExp.Visible = false;
                    btnSub4.Visible = false;
                    btnExportMultiple.Visible = false;
                    btnExportsingle.Visible = false;
                    //txtFrom.Text = string.Empty;
                    //txtTo.Text = string.Empty;
                }
                if (rdobtnCountVisit.Checked)
                {
                    gvPageCount.Visible = true;
                    gvPageCount.DataSource = null;
                    gvPageCount.DataBind();
                    gvAuditReports.Visible = false;                   
                    rdobtnMultipleUser.Checked = false;
                    lblUserName.Visible = true;
                    ddlUserViewer.Visible = true;
                    btnSub.Visible = false;
                    btnSub1.Visible = false;                    
                    btnSubmit1.Visible = false;
                    btnExport1.Visible = false;
                    btnSubmit.Visible = false;                    
                    btnExport.Visible = false;
                    btnCountHits.Visible = true;
                    //btnHitCountExp.Visible = false;
                    btnSub2.Visible = true;
                    btnSub4.Visible = false;
                    btnExportsingle.Visible = false;
                    //txtTo.Text = string.Empty;
                    //txtFrom.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Occurred.", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ddlUserViewer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTo.Text))
            {
                UserSession.DisplayMessage(this, "Please Enter Valid To Date.", MainMasterPage.MessageType.Warning);
                return;
            }
            Session["SingleUser"] = null;
            Session["MultipleUser"] = null;
            //multiple user report search

            Log.AuditLog(HttpContext.Current, "Multiple User Audit Report", "Audit Report");
          
            DataTable dt = SearchUserProducitvityData();
            gvAuditReports.DataSource = dt;
            Session["MultipleUser"] = "Multiple";
            gvAuditReports.DataBind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //dt = (DataTable)Session["dtTable"];
           
            Log.AuditLog(HttpContext.Current, "Multiple User Audit Report Download ", "Audit Report");
          
            DataTable dt = SearchUserProducitvityData();
            ExportToExcel(dt, "MultiUser_Audit_Report");
        }

        protected void btnSubmit1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTo.Text))
            {
                UserSession.DisplayMessage(this, "Please Enter Valid To Date.", MainMasterPage.MessageType.Warning);
                return;
            }
            //single user search
            Session["MultipleUser"] = null;
            Session["SingleUser"] = null;
            
            Log.AuditLog(HttpContext.Current,"Report Search Of User : " + ddlUserViewer.SelectedItem +"","Audit Report");
           
            DataTable dt = SearchUserProducitvityData1();
            Session["SingleUser"] = "Single";
            gvAuditReports.DataSource = dt;
            gvAuditReports.DataBind();
        }

        protected void btnExport1_Click(object sender, EventArgs e)
        { 
            //DataTable dt1 = new DataTable();
            //dt1 = (DataTable)Session["dtTable1"];
            
            Log.AuditLog(HttpContext.Current, "Report Download Of User : " + ddlUserViewer.SelectedItem + "", "Audit Report");
            
            DataTable dt1 = SearchUserProducitvityData1();            
            ExportToExcel1(dt1, "SingleUser_Audit_Report");
         }

     
        protected void ddll_Username()
        {
            int roleid = Convert.ToInt32(UserSession.RoleID);
           
            string query = "";
            if (roleid == 1)
            {
                query = " SELECT ID,UserName FROM [User] order by UserName";
            }
            else
            {
                query = "SELECT ID,UserName FROM [User] WHERE RoleID= " + roleid + " and Status = 1 order by UserName";
            }
            DataTable objTable = DataHelper.ExecuteDataTable(query, null);            
            ddlUserViewer.DataSource = objTable;
            ddlUserViewer.DataTextField = "UserName";
            ddlUserViewer.DataValueField = "ID";
            ddlUserViewer.DataBind();
            txtTo.Text = string.Empty;
            txtFrom.Text = string.Empty;
            ddlUserViewer.Items.Insert(0, new ListItem("--SELECT--", "0"));
        }
                      
        //protected void gvAuditReports1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvAuditReports1.PageIndex = e.NewPageIndex;
        //    SearchUserProducitvityData();
        //}

        //protected void gvAuditReports1_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    try
        //    {
        //        if (UserSession.GridData != null)
        //        {
        //            if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
        //                ViewState[UserSession.SortExpression] = "ASC";
        //            else
        //                ViewState[UserSession.SortExpression] = "DESC";

        //            gvAuditReports1.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
        //            gvAuditReports1.DataBind();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Occurred .", MainMasterPage.MessageType.Error);
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);

        //    }
        //}

        //protected void gvAuditReports1_RowDataBound(object sender, GridViewRowEventArgs e)
        //{

        //}

        protected void gvAuditReports_Sorted(object sender, EventArgs e)
        {

        }

        protected void gvAuditReports_Sorting(object sender, GridViewSortEventArgs e)
        {

            try
            {
                if (UserSession.GridData != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    gvAuditReports.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    gvAuditReports.DataBind();

                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Occurred .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }
        }

        protected void gvAuditReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {  
           if (Session["SingleUser"]!=null)
           {
               gvAuditReports.PageIndex = e.NewPageIndex;
               gvAuditReports.DataSource = SearchUserProducitvityData1();               
               gvAuditReports.DataBind();
           }
           else
           {               
               gvAuditReports.PageIndex = e.NewPageIndex;
               gvAuditReports.DataSource = SearchUserProducitvityData();
               gvAuditReports.DataBind();
           }
        }

        private DataTable SearchUserProducitvityData()
        {
            //multiple
            //string Date = txtDate.Text.Trim();
            string FrmDate = txtFrom.Text.Trim();
            string d = FrmDate.Split('/')[0];
            string m = FrmDate.Split('/')[1];
            string y = FrmDate.Split('/')[2];
            string SelectedFromDate = y + "-" + m + "-" + d;
            //string SelectedFromDate = txtFrom.Text.Replace('/','-');

            string ToDate = txtTo.Text.Trim();
            string dd = ToDate.Split('/')[0];
            string mm = ToDate.Split('/')[1];
            string yy = ToDate.Split('/')[2];
            string SelectedToDate = yy + "-" + mm + "-" + dd;
            // SelectedToDate = txtTo.Text.Replace('/', '-');

            BusinessLogic.Utility objUtility = new BusinessLogic.Utility();
            ReportManager ObjReportManager = new ReportManager();
            DataTable objDataTable = new DataTable();

            objUtility.Result = objDocumentManager.AuditReports(out objDataTable,Convert.ToString( UserSession.RoleID), SelectedFromDate, SelectedToDate,"");

            if (objUtility.Result == Utility.ResultType.Failure)
            {
                UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                //gvAuditReports.DataSource = objDataTable;
                //gvAuditReports.DataBind();
                return objDataTable;
            }
            else if (objUtility.Result == Utility.ResultType.Error)
            {
                UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                //gvAuditReports.DataSource = objDataTable;
                //gvAuditReports.DataBind();
                return objDataTable;
            }
            if (objDataTable.Rows.Count <= 0)
            {
                UserSession.DisplayMessage(this, "No data exists!", MainMasterPage.MessageType.Success);
                //gvAuditReports.DataSource = objDataTable;
                //gvAuditReports.DataBind();
                return objDataTable;
            }
            else
            {
                btnExport.Visible = true;
                btnExportMultiple.Visible = true;
                return objDataTable;
                //gvAuditReports.DataSource = objDataTable;
                //gvAuditReports.DataBind();
                //UserSession.GridData = objDataTable;
                //Session["dtTable"] = objDataTable;
            }
        }

        public void ExportToExcel(DataTable objDataTable, string strFileName)
        {
            try
            {
               // HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + strFileName.Trim() + ".xls");
                System.Text.StringBuilder objStringBuilder = new System.Text.StringBuilder();
                objStringBuilder.Append("<table style='border:1px solid black;'>");
                objStringBuilder.Append("<tr>");
                foreach (DataColumn objDataColumn in objDataTable.Columns)
                {
                    objStringBuilder.Append("<th style='border:1px solid black;'>");
                    objStringBuilder.Append(objDataColumn.ColumnName);
                    objStringBuilder.Append("</th>");
                }
                objStringBuilder.Append("</tr>");


                foreach (DataRow objDataRow in objDataTable.Rows)
                {
                    objStringBuilder.Append("<tr>");
                    foreach (DataColumn objDataColumn in objDataTable.Columns)
                    {
                        objStringBuilder.Append("<td style='border:1px solid black;'>");
                        objStringBuilder.Append(objDataRow[objDataColumn.ColumnName].ToString() == string.Empty ? "N/A" : objDataRow[objDataColumn.ColumnName].ToString());
                        objStringBuilder.Append("</td>");
                    }
                    objStringBuilder.Append("</tr>");
                }
                objStringBuilder.Append("</table>");
                HttpContext.Current.Response.Write(objStringBuilder.ToString());
                HttpContext.Current.Response.End();

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Occurred.", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        private DataTable SearchUserProducitvityData1()
        {
            //single
            //string Date = txtDate.Text.Trim();

            string FrmDate = txtFrom.Text.Trim();
            string d = FrmDate.Split('/')[0];
            string m = FrmDate.Split('/')[1];
            string y = FrmDate.Split('/')[2];
            string SelectedFromDate = y + "-" + m + "-" + d;
            //string SelectedFromDate = txtFrom.Text.Replace('/','-');

            string ToDate = txtTo.Text.Trim();
            string dd = ToDate.Split('/')[0];
            string mm = ToDate.Split('/')[1];
            string yy = ToDate.Split('/')[2];
            string SelectedToDate = yy + "-" + mm + "-" + dd;
            // SelectedToDate = txtTo.Text.Replace('/', '-');

            string userid = ddlUserViewer.SelectedValue.Trim();

            //if (FrmDate < ToDate)
            //{
                BusinessLogic.Utility objUtility = new BusinessLogic.Utility();
                ReportManager ObjReportManager = new ReportManager();
                DataTable objDataTable = new DataTable();

                objUtility.Result = objDocumentManager.AuditReports(out objDataTable,Convert.ToString(UserSession.RoleID) , SelectedFromDate, SelectedToDate, ddlUserViewer.SelectedValue);
                if (objUtility.Result == Utility.ResultType.Failure)
                {
                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                    //gvAuditReports.DataSource = objDataTable;
                    //gvAuditReports.DataBind();
                    return objDataTable;
                }
                else if (objUtility.Result == Utility.ResultType.Error)
                {
                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                    //gvAuditReports.DataSource = objDataTable;
                    //gvAuditReports.DataBind();
                    return objDataTable;
                }
                if (objDataTable.Rows.Count <= 0)
                {
                    UserSession.DisplayMessage(this, "No data exists!", MainMasterPage.MessageType.Success);
                    //gvAuditReports.DataSource = objDataTable;
                    //gvAuditReports.DataBind();
                    return objDataTable;
                }
                else
                {
                    //gvAuditReports.DataSource = objDataTable;
                    //gvAuditReports.DataBind();
                    btnExport1.Visible = true;
                    btnExportsingle.Visible = true;
                    return objDataTable;
                    //UserSession.GridData = objDataTable;
                    //lblSelectedUser.Text = ddlUserViewer.SelectedItem.Text;
                    //lbltotaldocument.Text = Convert.ToString(objDataTable.Rows.Count);
                    //Session["dtTable1"] = objDataTable;
                }
            //}
        }

        public void ExportToExcel1(DataTable objDataTable, string strFileName)
        {
            try
            {
               // HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + strFileName.Trim() + ".xls");
                System.Text.StringBuilder objStringBuilder1 = new System.Text.StringBuilder();
                objStringBuilder1.Append("<table style='border:1px solid black;'>");
                objStringBuilder1.Append("<tr>");
                foreach (DataColumn objDataColumn in objDataTable.Columns)
                {
                    objStringBuilder1.Append("<th style='border:1px solid black;'>");
                    objStringBuilder1.Append(objDataColumn.ColumnName);
                    objStringBuilder1.Append("</th>");
                }
                objStringBuilder1.Append("</tr>");


                foreach (DataRow objDataRow in objDataTable.Rows)
                {
                    objStringBuilder1.Append("<tr>");
                    foreach (DataColumn objDataColumn in objDataTable.Columns)
                    {
                        objStringBuilder1.Append("<td style='border:1px solid black;'>");
                        objStringBuilder1.Append(objDataRow[objDataColumn.ColumnName].ToString() == string.Empty ? "N/A" : objDataRow[objDataColumn.ColumnName].ToString());
                        objStringBuilder1.Append("</td>");
                    }
                    objStringBuilder1.Append("</tr>");
                }
                objStringBuilder1.Append("</table>");
                HttpContext.Current.Response.Write(objStringBuilder1.ToString());
                HttpContext.Current.Response.End();

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Occurred.", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void btnCountHits_Click(object sender, EventArgs e)
        {
            gvAuditReports.Visible = false;
            gvPageCount.Visible = true;
            btnHitCountExp.Visible = true;
            btnSub4.Visible = true;
            if (string.IsNullOrEmpty(txtTo.Text))
            {
                UserSession.DisplayMessage(this, "Please Enter Valid To Date.", MainMasterPage.MessageType.Warning);
                return;
            }

            Log.AuditLog(HttpContext.Current, "Page Hit Count Checked of User ID : " + ddlUserViewer.SelectedItem + "", "Audit Report");
           
            DataTable dt = PageHitCount();
            gvPageCount.DataSource = dt;
            gvPageCount.DataBind();

        }

        private DataTable PageHitCount()
        {
            //string Date = txtDate.Text.Trim();
            string FrmDate = txtFrom.Text.Trim();
            string d = FrmDate.Split('/')[0];
            string m = FrmDate.Split('/')[1];
            string y = FrmDate.Split('/')[2];
            string SelectedFromDate = y + "-" + m + "-" + d;
            //string SelectedFromDate = txtFrom.Text.Replace('/','-');

            string ToDate = txtTo.Text.Trim();
            string dd = ToDate.Split('/')[0];
            string mm = ToDate.Split('/')[1];
            string yy = ToDate.Split('/')[2];
            string SelectedToDate = yy + "-" + mm + "-" + dd;
            // SelectedToDate = txtTo.Text.Replace('/', '-');

            string userid = ddlUserViewer.SelectedValue.Trim();

            BusinessLogic.Utility objUtility = new BusinessLogic.Utility();
            ReportManager ObjReportManager = new ReportManager();
            DataTable objDataTable = new DataTable();

            objUtility.Result = objDocumentManager.PageHitCount(out objDataTable, ddlUserViewer.SelectedValue, SelectedFromDate, SelectedToDate);
            if (objUtility.Result == Utility.ResultType.Failure)
            {
                UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                //gvPageCount.DataSource = objDataTable;
                //gvPageCount.DataBind();
                return objDataTable;
            }
            else if (objUtility.Result == Utility.ResultType.Error)
            {
                UserSession.DisplayMessage(this, "No Record Found!!!", MainMasterPage.MessageType.Warning);
                //gvPageCount.DataSource = objDataTable;
                //gvPageCount.DataBind();
                return objDataTable;
            }
            if (objDataTable.Rows.Count <= 0)
            {
                UserSession.DisplayMessage(this, "No data exists!", MainMasterPage.MessageType.Success);
                //gvPageCount.DataSource = objDataTable;
                //gvPageCount.DataBind();
                return objDataTable;
            }
            else
            {
                //gvPageCount.DataSource = objDataTable;
                //gvPageCount.DataBind();
                return objDataTable;
                //UserSession.GridData = objDataTable;
                //Session["dtCountTable"] = objDataTable;
            }
        }

        protected void btnHitCountExp_Click(object sender, EventArgs e)
        {
            //DataTable dtCountTable = new DataTable();
            //dtCountTable = (DataTable)Session["dtCountTable"];
            
            Log.AuditLog(HttpContext.Current, "Page Hit Count Checked of User ID : " + ddlUserViewer.SelectedItem + "", "Hit Count Report Download");

            DataTable dtCountTable = PageHitCount();  
            ExportToExcelCount(dtCountTable,"Page_Hit_Count "+DateTime.Now.ToString());
        }

        public void ExportToExcelCount(DataTable objDataTable, string strFileName)
        {
            try
            {
               // HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + strFileName.Trim() + ".xls");
                System.Text.StringBuilder objStringBuilder1 = new System.Text.StringBuilder();
                objStringBuilder1.Append("<table style='border:1px solid black;'>");
                objStringBuilder1.Append("<tr>");
                foreach (DataColumn objDataColumn in objDataTable.Columns)
                {
                    objStringBuilder1.Append("<th style='border:1px solid black;'>");
                    objStringBuilder1.Append(objDataColumn.ColumnName);
                    objStringBuilder1.Append("</th>");
                }
                objStringBuilder1.Append("</tr>");


                foreach (DataRow objDataRow in objDataTable.Rows)
                {
                    objStringBuilder1.Append("<tr>");
                    foreach (DataColumn objDataColumn in objDataTable.Columns)
                    {
                        objStringBuilder1.Append("<td style='border:1px solid black;'>");
                        objStringBuilder1.Append(objDataRow[objDataColumn.ColumnName].ToString() == string.Empty ? "N/A" : objDataRow[objDataColumn.ColumnName].ToString());
                        objStringBuilder1.Append("</td>");
                    }
                    objStringBuilder1.Append("</tr>");
                }
                objStringBuilder1.Append("</table>");
                HttpContext.Current.Response.Write(objStringBuilder1.ToString());
                HttpContext.Current.Response.End();

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Occurred.", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvPageCount_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPageCount.PageIndex = e.NewPageIndex;
            gvPageCount.DataSource= PageHitCount();
            gvPageCount.DataBind();
        }

    }
}