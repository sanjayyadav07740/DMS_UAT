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
    public partial class DocumentSearch : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        public enum SearchType { SoleId, Description, Barcode };
        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)this.Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnPolicyReport);
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchDocument();
        }

        private void SearchDocument()
        {
            string UserName = string.Empty;
            if (Session["UserName"] != null)
            {
                UserName = Session["UserName"].ToString();
            }
            Document objDocument = new Document();
            if (ddlSearchType.SelectedValue == "0")
                objDocument.SearchType = "SoleId";
            else if (ddlSearchType.SelectedValue == "1")
                objDocument.SearchType = "Description";
            else if (ddlSearchType.SelectedValue == "2")
                objDocument.SearchType = "UserName";
            else if (ddlSearchType.SelectedValue == "3")
                objDocument.SearchType = "Barcode";
            else
                objDocument.SearchType = "All";
                if (ddlSearchType.SelectedValue == "0")
                {
                    try
                    {
                        int Soleid = Convert.ToInt32(txtDocument.Text);
                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('Sole Id should be number')", true);
                        return;
                    }
                }
                DataTable dt = objDocument.SearchDocument(txtDocument.Text.Trim(), UserName);
                Session["dt"] = dt;
                gvDocSearch.DataSource = dt.DefaultView;
                gvDocSearch.DataBind();
        

          

                
        }

        protected void gvDocSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDocSearch.PageIndex = e.NewPageIndex;
            SearchDocument();
        }

        protected void ibtnReport_Click(object sender, ImageClickEventArgs e)
        {
             

            
            DataTable dtsession = new DataTable();

            dtsession = (DataTable)Session["dt"];
            ExporttoExcel(dtsession);

        }

        private void ExporttoExcel(DataTable dt1)
        {

            if (dt1.Rows.Count > 0)
            {
                string filename = "ReportExcel.xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);

                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt1;
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