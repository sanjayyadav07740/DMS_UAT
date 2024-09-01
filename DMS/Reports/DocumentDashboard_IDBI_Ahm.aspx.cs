using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
using System.Text;

namespace DMS.Reports
{
    public partial class DocumentDashboard_IDBI_Ahm : System.Web.UI.Page
    {
        Utility objUtility = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnShow);
        }

        protected void ibtnShow_Click(object sender, ImageClickEventArgs e)
        {
            if (txtFromDate.Text == "" || txtToDate.Text == "")
            {
                UserSession.DisplayMessage(this, "Please select both From Date and To Date", MainMasterPage.MessageType.Warning);
            }
            else
            {
                int result = DateTime.Compare(Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
                if (result > 0)
                {
                    UserSession.DisplayMessage(this, "From Date is later than To Date. Please select proper date", MainMasterPage.MessageType.Warning);
                }
                else
                {
                    DataTable dt = new DataTable();
                    DMS.BusinessLogic.DocumentManager obj = new DocumentManager();
                    objUtility.Result = obj.SelectReportDocumentIDBI(out dt, txtFromDate.Text, txtToDate.Text);
                    switch (objUtility.Result)
                    {
                        case BusinessLogic.Utility.ResultType.Failure:
                            UserSession.DisplayMessage(this, "No Data To Display.", MainMasterPage.MessageType.Warning);
                            return;
                            break;

                        case BusinessLogic.Utility.ResultType.Error:
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                            break;
                    }
                  
                        csv(dt);
                   // GenerateCSV(dt);
                   
                        //string Filename = string.Format("DocumentReport{0:dd MMMM yyyy HH mm tt}", DateTime.Now);
                        //Utility.ExportToExcel(dt, Filename);
                   
                    
                }
            }
        }
        private void csv(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    HttpContext context = HttpContext.Current;
                    context.Response.Clear();
                    foreach (DataColumn column in dt.Columns)
                    {
                        context.Response.Write(column.ColumnName + ",");
                    }
                    context.Response.Write(Environment.NewLine);
                    foreach (DataRow row in dt.Rows)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");
                        }
                        context.Response.Write(Environment.NewLine);
                    }
                    context.Response.ContentType = "text/csv";
                    string Filename = string.Format("DocumentReport{0:dd MMMM yyyy HH mm tt}", DateTime.Now);
                    Response.ClearHeaders();
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + Filename + ".csv");
                    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        

        protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";

        }
    }
}