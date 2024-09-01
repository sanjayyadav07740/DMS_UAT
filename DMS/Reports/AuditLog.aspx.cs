using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
namespace DMS.Reports
{
    public partial class AuditLog : System.Web.UI.Page
    {
       Utility objUtility = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            //BindGrid();
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.ibtnExport);
            if (!IsPostBack)
            {
                int RepositoryId = 0;
                RoleManager objRoleManager = new RoleManager();
                DataSet ds = new DataSet();
                if (UserSession.RoleID != 1)
                {
                    ds = objRoleManager.Select(UserSession.RoleID);
                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            RepositoryId = Convert.ToInt32(ds.Tables[0].Rows[0]["RepositoryID"].ToString());
                            break;
                    }
                }
                else
                {
                    RepositoryId = 0;
                }
                Session["RepId"] = RepositoryId;
                DataSet ds1 = new DataSet();
                ds1 = DMS.BusinessLogic.UserPermission.SelectByRepId(RepositoryId);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    ddlUserName.DataSource = ds1.Tables[0];
                    ddlUserName.DataTextField = "UserName";
                    ddlUserName.DataValueField = "ID";
                    ddlUserName.DataBind();
                }
                ddlUserName.Items.Insert(0, "--Select--");
                //BindGrid(RepositoryId);
                BindGridUser(UserSession.UserID);
            }
        }

        protected void BindGrid(int RepositoryID)
        {
           
            ReportManager objReportManager=new ReportManager();
            DataSet ds = new DataSet();
            ds = objReportManager.SelectAuditLog(RepositoryID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                grvAuditReport.Visible = true;
                grvAuditReport.DataSource = ds;
                grvAuditReport.DataBind();
            }
            else
            {
                UserSession.DisplayMessage(this, "No Record Found", MainMasterPage.MessageType.Success);
                grvAuditReport.Visible = false;
                return;
            }
        }

        protected void BindGridUser(int UserID)
        {

            ReportManager objReportManager = new ReportManager();
            DataSet ds = new DataSet();
            ds = objReportManager.SelectAuditLogUser(UserID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                grvAuditReport.Visible = true;
                grvAuditReport.DataSource = ds;
                grvAuditReport.DataBind();
                Session["dtTable"] = ds.Tables[0];
            }
            else
            {
                UserSession.DisplayMessage(this, "No Record Found For Selected User", MainMasterPage.MessageType.Success);
                grvAuditReport.Visible = false;
                return;
            }
        }

        protected void grvAuditReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvAuditReport.PageIndex = e.NewPageIndex;
            if(ddlUserName.SelectedItem.Text=="--Select--")
            BindGrid(Convert.ToInt32(Session["RepId"]));
            else
                BindGridUser(Convert.ToInt32(ddlUserName.SelectedItem.Value));
        }

        protected void ddlUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridUser(Convert.ToInt32(ddlUserName.SelectedItem.Value));
        }

        protected void ibtnExport_Click(object sender, ImageClickEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["dtTable"];
            ExportToExcel(dt, "Audit Trail");
        }

        public void ExportToExcel(DataTable objDataTable, string strFileName)
        {
            try
            {
                HttpContext.Current.Response.ContentType = "application/octet-stream";
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
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured.", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        
    }
}