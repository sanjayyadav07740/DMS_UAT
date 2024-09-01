using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.IO;
using System.Web.UI.HtmlControls;
namespace DMS.Folder
{
    public partial class EmptyFolder : System.Web.UI.Page
    {
        #region Private Member
        Utility objUtility = new Utility();
        FolderManager objFolderManager = new FolderManager();
        TreeView node = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            imgsubmit.Visible = false;
            ((AjaxControlToolkit.ToolkitScriptManager)this.Master.FindControl("tsmManager")).RegisterPostBackControl(imgsubmit);

            txtcount.Visible = false;
            btnsearch.Visible = false;
            lblcount.Visible = false;
            lbldoccount.Visible = false;
            ((DropDownList)emodModule.FindControl("ddlMetaTemplateName")).SelectedIndexChanged += new EventHandler(ddlMetaTemplateName_SelectedIndexChanged);
            ((DropDownList)emodModule.FindControl("ddlRepositoryName")).SelectedIndexChanged += new EventHandler(ddlRepositoryName_SelectedIndexChanged);
            ((TreeView)emodModule.FindControl("tvwFolder")).SelectedNodeChanged += new EventHandler(tvwFolder_SelectedNodeChanged);
            node = (TreeView)(emodModule.FindControl("tvwFolder"));

        }

        private void LoadGrid(int count)
        {
            try
            {
                int selectedFolder;
                if (node.SelectedValue == "")
                {
                    selectedFolder = 0;
                }
                else
                {
                    selectedFolder = int.Parse(node.SelectedValue);
                }

                DataTable objDataTable = new DataTable();

                objUtility.Result = FolderManager.selectFolderbyMetaID(out objDataTable, emodModule.SelectedRepository, emodModule.SelectedMetaTemplate, selectedFolder, count);

                if (objDataTable.Rows.Count > 0)
                {
                    gvrEmpFolder.DataSource = objDataTable;
                    Session["FolderReport"] = objDataTable;
                    UserSession.GridData = objDataTable;
                    gvrEmpFolder.DataBind();
                    imgsubmit.Visible = true;
                    txtcount.Visible = true;
                    btnsearch.Visible = true;
                    lblcount.Visible = true;
                    lbldoccount.Visible = true;
                    lbldoccount.Text = "TOTAL Count :" + objDataTable.Rows.Count;
                }
                else
                {
                    imgsubmit.Visible = false;
                    gvrEmpFolder.DataSource = null;
                    gvrEmpFolder.DataBind();
                }

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }


        }

        protected void ddlMetaTemplateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                objUtility.Result = Utility.LoadFolder(node, Convert.ToInt32(emodModule.SelectedMetaTemplate));

                gvrEmpFolder.DataSource = null;
                gvrEmpFolder.DataBind();
                txtcount.Visible = false;
                btnsearch.Visible = false;
                imgsubmit.Visible = false;
                lblcount.Visible = false;
                lbldoccount.Visible = false;

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        //private void ExportToExcel()
        //{
        //    try
        //    {
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        Response.Charset = "";
        //        string FileName = "FolderReport" + DateTime.Now + ".xls";
        //        StringWriter strwritter = new StringWriter();
        //        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        //        Response.ContentType = "application/vnd.ms-excel";
        //        HtmlForm frm = new HtmlForm();
        //        gvrEmpFolder.Parent.Controls.Add(frm);
        //        frm.Attributes["runat"] = "server";
        //        frm.Controls.Add(gvrEmpFolder);
        //        frm.RenderControl(htmltextwrtter);
        //        Response.Write(strwritter.ToString());
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {

        //        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);
        //    }
        //}

        private void ExportToExcel(DataTable objTable, string fileName)
        {
            string attachment = "attachment; filename=" + fileName + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";

            objTable.Columns.Remove("FolderId");
            objTable.Columns.Remove("ParentFolderID");
            objTable.Columns.Remove("RepositoryId");
            objTable.Columns.Remove("MetaTemplateID");
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
              
                for (i = 0; i < objTable.Columns.Count; i++)
                {
                    Response.Write(tab + (dr[i].ToString().Replace("\r\n", "")));
                    tab = "\t";
                }
                // Log.DocumentAuditLog(HttpContext.Current, "Document Download", "UploadedDocReport", docid);
                Response.Write("\n");
            }

            Log.AuditLog(HttpContext.Current, "Download Folder Report", "Emptyfolder");

            Response.End();
        }

        protected void ddlRepositoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvrEmpFolder.DataSource = null;
            gvrEmpFolder.DataBind();
            txtcount.Visible = false;
            btnsearch.Visible = false;
            imgsubmit.Visible = false;
            lblcount.Visible = false;
            lbldoccount.Visible = false;
        }


        protected void tvwFolder_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(node.SelectedValue) && node.SelectedValue != null)
                {
                    int one = Convert.ToInt32(node.SelectedValue);
                    LoadTreeData(emodModule.SelectedFolder, emodModule.SelectedMetaTemplate);
                    DropDownList ddlmeta = ((DropDownList)emodModule.FindControl("ddlMetaTemplateName"));
                    if (ddlmeta.SelectedIndex > 0)
                    {
                        if (string.IsNullOrEmpty(txtcount.Text))
                        {

                            LoadGrid(0);
                        }
                        else
                        {
                            LoadGrid(Convert.ToInt32(txtcount.Text));

                        }

                    }
                    else
                    {
                        gvrEmpFolder.DataSource = null;
                        gvrEmpFolder.DataBind();

                    }

                }
                else
                {
                    gvrEmpFolder.DataSource = null;
                    gvrEmpFolder.DataBind();
                    txtcount.Visible = false;
                    btnsearch.Visible = false;
                    imgsubmit.Visible = false;
                    lblcount.Visible = false;
                    lbldoccount.Visible = false;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        private void LoadTreeData(int intFolderID, int intMetaTemplateID)
        {
            try
            {
                DataTable objDataTable = new DataTable();
                objUtility.Result = FolderManager.SelectFolderByParentFolderID(out objDataTable, emodModule.SelectedFolder, emodModule.SelectedMetaTemplate);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        //UserSession.GridData = objDataTable;
                        //gvrEmpFolder.DataSource = objDataTable;
                        //gvrEmpFolder.DataBind();
                        break;

                    case Utility.ResultType.Failure:
                        //UserSession.GridData = null;
                        //gvrEmpFolder.DataSource = null;
                        //gvrEmpFolder.DataBind();
                        UserSession.DisplayMessage(this, "No Folder To Display At This Level .", MainMasterPage.MessageType.Warning);
                        break;

                    case Utility.ResultType.Error:
                        //UserSession.GridData = null;
                        //gvrEmpFolder.DataSource = null;
                        //gvrEmpFolder.DataBind();
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void imgsubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["FolderReport"];
                ExportToExcel(dt, string.Format("FolderReport{0:dd MMMM yyyy HH mm tt}", DateTime.Now));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtcount.Text) && txtcount.Text == "")
            {
                LoadGrid(Convert.ToInt32(0));

            }
            else
            {
                LoadGrid(Convert.ToInt32(txtcount.Text));

            }



        }

        protected void gvrEmpFolder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            imgsubmit.Visible = true;
            txtcount.Visible = true;
            btnsearch.Visible = true;
            lblcount.Visible = true;
            lbldoccount.Visible = true;
            lbldoccount.Visible = true;
            try
            {
                if (UserSession.GridData != null)
                {
                    gvrEmpFolder.PageIndex = e.NewPageIndex;
                    if (UserSession.FilterData == null)
                        gvrEmpFolder.DataSource = UserSession.GridData;
                    else
                        gvrEmpFolder.DataSource = UserSession.FilterData;

                    gvrEmpFolder.DataBind();
                }
            }
            catch (Exception ex)
            {
                //UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }

        }


    }
}
