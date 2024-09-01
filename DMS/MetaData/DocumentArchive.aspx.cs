using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;
using System.Configuration;
using System.Data.SqlClient;

namespace DMS.MetaData
{
    public partial class DocumentArchive : System.Web.UI.Page
    {
        #region Private Memeber
        Utility objUtility = new Utility();
        SqlConnection con = new SqlConnection(Utility.ConnectionString);
        string strDocumentID;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnExportData);

            if (!IsPostBack)
            {
                DocumentArchiveBind();
                Log.AuditLog(HttpContext.Current, "Visit", "DocumentArchive");
            }
        }

        protected void gvwDocumentArchive_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwDocumentArchive.PageIndex = e.NewPageIndex;
                    if (UserSession.FilterData == null)
                        gvwDocumentArchive.DataSource = UserSession.GridData;
                    else
                        gvwDocumentArchive.DataSource = UserSession.FilterData;

                    gvwDocumentArchive.DataBind();
                }
            }
            catch (Exception ex)
            {
                //UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void gvwDocumentArchive_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "restore")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    // UserSession.MetaDataID = Convert.ToInt32(gvwDocumentArchive.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwDocumentArchive.DataKeys[intRowIndex].Values["ID"].ToString().Trim();

                    con.Open();
                    SqlCommand cmd = new SqlCommand("Update Document set Status=1 where ID=" + strDocumentID, con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                    DocumentArchiveBind();

                    Log.DocumentAuditLog(HttpContext.Current, "Document Restored", "DocumentArchive", Convert.ToInt32(strDocumentID));

                }
                if (e.CommandName.ToLower().Trim() == "permanetdelete")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    // UserSession.MetaDataID = Convert.ToInt32(gvwDocumentArchive.DataKeys[intRowIndex].Values["MetaDataID"]);
                    strDocumentID = gvwDocumentArchive.DataKeys[intRowIndex].Values["ID"].ToString().Trim();

                    con.Open();
                    SqlCommand cmd = new SqlCommand("Update Document set Status= -1 where ID=" + strDocumentID, con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                    DocumentArchiveBind();
                    Log.DocumentAuditLog(HttpContext.Current, "Document Permanent Deleted", "DocumentArchive", Convert.ToInt32(strDocumentID));

                }
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
                dt = (DataTable)Session["DocumentArchive"];
                ExportToExcel(dt, string.Format("DocumentArchiveReport{0:dd MMMM yyyy HH mm tt}", DateTime.Now));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        public void DocumentArchiveBind()
        {
            DataTable dt = new DataTable();
            dt = DocumentArchiveDetails();
            Session["DocumentArchive"] = dt;
            gvwDocumentArchive.DataSource = dt;
            gvwDocumentArchive.DataBind();
            UserSession.GridData = dt;
            lblNoofRecords.Text = "No. of Documents :" + Convert.ToString(dt.Rows.Count);
        }

        public DataTable DocumentArchiveDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                string strCon = ConfigurationManager.AppSettings["ConnectionString"];

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    using (SqlCommand cmd = new SqlCommand("Sp_S_DocumentArchiveDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
            return dt;
        }

        private void ExportToExcel(DataTable objTable, string fileName)
        {
            string attachment = "attachment; filename=" + fileName + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";

            objTable.Columns.Remove("DocumentPath");
            objTable.Columns.Remove("CreatedOn");
            objTable.Columns.Remove("CreatedBy");

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

            Log.AuditLog(HttpContext.Current, "Document Archive Report Downloaded", "DocumentArchive");

            Response.End();
        }

    }
}