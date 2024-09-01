using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
using System.IO;
using System.Drawing;
using System.Net;
namespace DMS.Shared
{
    public partial class DocumentDashBoard_SSL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManger = ScriptManager.GetCurrent(this.Page);
            scriptManger.RegisterPostBackControl(this.ibtnExport);
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                DMS.BusinessLogic.Document.SelectSSLDocument(out dt, DateTime.Today, DateTime.Today);
                if (dt.Rows.Count > 0)
                {
                    grvUploadedDocs.Visible = true;
                    grvUploadedDocs.DataSource = dt;
                    grvUploadedDocs.DataBind();
                    ViewState["data"] = dt;
                }
                else
                {
                    grvUploadedDocs.Visible = false;
                }

            }
        }

        protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            txtDate.Text = "";
            txtToDate.Text = "";
            grvUploadedDocs.Visible = false;
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (txtDate.Text == "")
            {
                UserSession.DisplayMessage(this, "Kindly enter date", MainMasterPage.MessageType.Warning);
            }
            else if (Convert.ToDateTime(txtDate.Text) > DateTime.Today)
            {
                UserSession.DisplayMessage(this, "Kindly enter valid date", MainMasterPage.MessageType.Warning);
            }
            else
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DMS.BusinessLogic.Document.SelectSSLDocument(out dt, Convert.ToDateTime(txtDate.Text), Convert.ToDateTime(txtToDate.Text));
                if (dt.Rows.Count > 0)
                {
                    grvUploadedDocs.Visible = true;
                    grvUploadedDocs.DataSource = dt;
                    grvUploadedDocs.DataBind();                    
                    ViewState["data"] = dt;
                }
                else
                {
                    grvUploadedDocs.Visible = false;
                }
            }
        }

        protected void ibtnExport_Click(object sender, ImageClickEventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "DocumentSearchResult" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);            
            grvUploadedDocs.GridLines = GridLines.Both;
            grvUploadedDocs.HeaderStyle.Font.Bold = true;
            grvUploadedDocs.AllowPaging = false;
            //if (UserSession.FilterData == null)
            //    gvwDocument.DataSource = UserSession.GridData;
            //else
            grvUploadedDocs.DataSource = (DataTable)ViewState["data"];

            grvUploadedDocs.DataBind();
            grvUploadedDocs.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void grvUploadedDocs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower().Trim() == "documentsearch")
            {

                int intRowIndex = Convert.ToInt32(e.CommandArgument);
                UserSession.MetaDataID = Convert.ToInt32(grvUploadedDocs.DataKeys[intRowIndex].Values["MetaDataID"]);
                string strDocumentID = grvUploadedDocs.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                //string strStatus = grvUploadedDocs.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                string strStatus = grvUploadedDocs.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                string DocName = grvUploadedDocs.DataKeys[intRowIndex].Values["DocumentName"].ToString().Trim();
                Session["DocId"] = strDocumentID;
                Session["DocumentName"] = DocName;
                Response.Redirect("../MetaData/ApprovedDocument.aspx?DOCID=" + strDocumentID, false);
               // Response.Redirect("../MetaData/DocumentViewer.aspx?DOCID=" + strDocumentID, false);
                //Response.Redirect("../MetaData/DocumentEntry.aspx?DOCID=" + strDocumentID, false);
            }
            if (e.CommandName.ToLower().Trim() == "download")
            {
                int intRowIndex = Convert.ToInt32(e.CommandArgument);
                UserSession.MetaDataID = Convert.ToInt32(grvUploadedDocs.DataKeys[intRowIndex].Values["MetaDataID"]);
                string strDocumentID = grvUploadedDocs.DataKeys[intRowIndex].Values["ID"].ToString().Trim();
                //string strStatus = grvUploadedDocs.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                string strStatus = grvUploadedDocs.DataKeys[intRowIndex].Values["DocumentStatusID"].ToString().Trim();
                string DocName = grvUploadedDocs.DataKeys[intRowIndex].Values["DocumentName"].ToString().Trim();
                string DocPath = grvUploadedDocs.DataKeys[intRowIndex].Values["DocumentPath"].ToString().Trim();
                Session["DocumentName"] = DocName;
                Session["DocumentPath"] = DocPath;
                Response.Redirect("../UserControl/PdfHandler.ashx?PageID=download");
            }
        }

        protected void grvUploadedDocs_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
   (e.Row.RowState == DataControlRowState.Normal ||
    e.Row.RowState == DataControlRowState.Alternate))
            {
                CheckBox chkBxSelect = (CheckBox)e.Row.Cells[1].FindControl("chkBxSelect");
                CheckBox chkBxHeader = (CheckBox)this.grvUploadedDocs.HeaderRow.FindControl("chkBxHeader");
                chkBxSelect.Attributes["onclick"] = string.Format
                                                       (
                                                          "javascript:ChildClick(this,'{0}');",
                                                          chkBxHeader.ClientID
                                                       );
            }
        }

        protected void ibtnDownload_Click(object sender, ImageClickEventArgs e)
        {
            foreach (GridViewRow gvrow in grvUploadedDocs.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.Cells[1].FindControl("chkBxSelect");
               
                //string str = gvrow.Cells[2].Text;
                if (chk != null & chk.Checked)
                {
                    //gvProducts.SelectedDataKey["ProductID"].ToString();
                    
                    //Session["DocumentName"] = gvrow.Cells[2].Text;
                    //Session["DocumentPath"] = gvrow.Cells[4].Text;
                   // Response.Redirect("../UserControl/PdfHandler.ashx?PageID=download");
                    Download(gvrow.Cells[2].Text, gvrow.Cells[4].Text);
                }
            }
            UserSession.DisplayMessage(this, "Selected Files Have been downloaded in the folder SSL Downloads created in D Drive of your system! ", MainMasterPage.MessageType.Success);
        }
        protected void Download(string DocName,string DocPath)
        {
        
            if (!Directory.Exists(@"D:\"))
            {
                UserSession.DisplayMessage(this, "D Drive is not present on your system! Files cannot be downloaded!!", MainMasterPage.MessageType.Error);
                return;
            }
            else
            {
                string subPath = @"D:\SSL Downloads\"; // your code goes here

                bool exists = System.IO.Directory.Exists(subPath);

                if (!exists)
                    System.IO.Directory.CreateDirectory(subPath);
                Services.DMSUploadDownloadWebService objDMSWebService = new Services.DMSUploadDownloadWebService();
                byte[] byteFileByte = objDMSWebService.DownloadDocumentFromServer(DocPath);
                Stream objWriteStream = File.Create(Path.Combine(subPath, DocName));
                objWriteStream.Write(byteFileByte, 0, byteFileByte.Length);
                objWriteStream.Close();
                objWriteStream.Dispose();
            }
        }
          
    }
}