using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using DMS.BusinessLogic;

namespace DMS.Reports
{
    public partial class CentrumBillReport : System.Web.UI.Page
    {

        #region PRIVATE MEMBERS ------

        Utility objUtility = new Utility();
        Document objdocument = new Document();
        SqlConnection con = new SqlConnection(Utility.ConnectionString);

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ibtnExport);
            if (!IsPostBack)
            {                
                LoadDepartment();              
            }
        }

        private void LoadDepartment()
        {
            try
            {
             
                string strQuery = @"Select MetaTemplateName,id from Metatemplate where RepositoryID=57 ";
                SqlCommand cmd = new SqlCommand(strQuery, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                ddldepartment.DataSource = dt;
                //ddldepartment.DataTextField = "department";.
                ddldepartment.DataTextField = "MetaTemplateName";
                ddldepartment.DataValueField = "id";
                ddldepartment.DataBind();
                ddldepartment.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataTable objDataTable = new DataTable();
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
               
                int MetatemplateID = Convert.ToInt32(ddldepartment.SelectedItem.Value);
                ReportManager ObjReportManager = new ReportManager();
                objUtility.Result = ObjReportManager.SelectCentrumBillingReport(out  objDataTable, SelectedFromDate, SelectedToDate, MetatemplateID);
               Session["CentrumBill"]= objDataTable;
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        gvCentrumBill.DataSource = objDataTable;
                        gvCentrumBill.DataBind();                        
                        break;
                    case Utility.ResultType.Failure:

                        break;
                }
            }
            catch (Exception ex) 
            {
             LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void gvCentrumBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCentrumBill.PageIndex = e.NewPageIndex;
            gvCentrumBill.DataSource = Session["CentrumBill"];
            gvCentrumBill.DataBind();
        }

        protected void ibtnExport_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt1 = new DataTable();
            dt1 = (DataTable)Session["CentrumBill"];
            ExportToExcel1(dt1, "Centrum Bill");
        }

        public void ExportToExcel1(DataTable objDataTable, string strFileName)
        {
            try
            {
                HttpContext.Current.Response.ContentType = "application/octet-stream";
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
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured.", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }


    }
}