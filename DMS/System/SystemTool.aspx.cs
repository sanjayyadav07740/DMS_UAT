using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.IO;

namespace DMS
{
    public partial class SystemTool : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillLoggerData();
            }
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(gvwLogViewer);
        }

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                txtResult.Text = Utility.Encrypt(txtPassword.Text);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                txtResult.Text = Utility.Decrypt(txtPassword.Text);
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        private void FillLoggerData()
        {
            DirectoryInfo objDirectoryInfo = new DirectoryInfo(Utility.LogFilePath);
            FileInfo[] objFileInfo = objDirectoryInfo.GetFiles("*.txt");
            gvwLogViewer.DataSource = objFileInfo.OrderByDescending(n => n.CreationTime).ToList();
            gvwLogViewer.DataBind();
        }

        protected void gvwLogViewer_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvwLogViewer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower() == "download")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    string strFilePath = gvwLogViewer.DataKeys[intRowIndex].Value.ToString();
                    if (File.Exists(strFilePath))
                    {
                        Response.ContentType = "application/octet-stream";
                        Response.AppendHeader("content-disposition", "attachment;filename=" + Path.GetFileName(strFilePath));
                        Response.TransmitFile(strFilePath);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        UserSession.DisplayMessage(this, "File Not Found.", MainMasterPage.MessageType.Warning);
                    }
                }
                else if(e.CommandName.ToLower() == "deletefile")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    string strFilePath = gvwLogViewer.DataKeys[intRowIndex].Value.ToString();
                    if (File.Exists(strFilePath))
                    {
                        File.Delete(strFilePath);
                        FillLoggerData();
                    }
                    else
                    {
                        UserSession.DisplayMessage(this, "File Not Found.", MainMasterPage.MessageType.Warning);
                    }
                }
            }
            catch(Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void gvwLogViewer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Utility.SetGridHoverStyle(e);
        }

        protected void gvwLogViewer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvwLogViewer.PageIndex = e.NewPageIndex;
            FillLoggerData();
        }
    }
}