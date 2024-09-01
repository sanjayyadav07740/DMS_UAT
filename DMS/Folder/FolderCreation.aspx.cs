using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;
namespace DMS.Folder
{
    public partial class FolderCreation : System.Web.UI.Page
    {
        #region Private Member
        Utility objUtility = new Utility();
        FolderManager objFolderManager = new FolderManager();
        #endregion

        #region Event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Utility.LoadStatus(ddlStatus);
                txtFolderName.Focus();

                if (UserSession.IsCreateFolder == 0)
                {
                    ClearControl();
                    lblTitle.Text = "Create Folder";
                }
                else if (UserSession.IsCreateFolder != 0)
                {
                    LoadControl(UserSession.IsCreateFolder);
                    lblTitle.Text = "Update Folder";
                }
                Log.AuditLog(HttpContext.Current, "Visit", "Folder Creation");
            }
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                if (UserSession.IsCreateFolder == 0 || hdfFolderName.Value.Trim().ToUpper() != txtFolderName.Text.Trim().ToUpper())
                {
                    if (UserSession.IsCreateFolder == 0)
                    {
                        objUtility.Result = FolderManager.SelectFolder(out dt,txtFolderName.Text.Trim(), emodModule.SelectedMetaTemplate, emodModule.SelectedFolder,emodModule.SelectedCategory);
                    }
                    else if (UserSession.IsCreateFolder != 0)
                    {
                        objUtility.Result = FolderManager.SelectFolder(out dt, txtFolderName.Text.Trim(), Convert.ToInt32(hdfMetaTemplateID.Value), Convert.ToInt32(hdfParentFolderID.Value), emodModule.SelectedCategory);
                    }

                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            if(dt.Rows.Count>0)
                            UserSession.DisplayMessage(this, "Folder Name Is Already Exist .", MainMasterPage.MessageType.Warning);
                            return;
                            break;

                        case Utility.ResultType.Error:
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                            break;
                    }
                }

                if (UserSession.IsCreateFolder == 0)
                {
                    DMS.BusinessLogic.Folder objFolder = new DMS.BusinessLogic.Folder();
                    objFolder.FolderName = txtFolderName.Text.Trim();
                    objFolder.FolderDescription = txtFolderDescription.Text.Trim();
                    objFolder.MetaTemplateID = emodModule.SelectedMetaTemplate;
                    objFolder.ParentFolderID = emodModule.SelectedFolder;
                    objFolder.CreatedBy = UserSession.UserID;
                    objFolder.Status = Convert.ToInt32(ddlStatus.SelectedItem.Value);
                    objFolder.CategoryID = emodModule.SelectedCategory;

                    objUtility.Result = FolderManager.InsertFolder(objFolder);
                    if(emodModule.SelectedRepository == 82)
                    {
                        FolderManager.GetAccesstoFolder(objFolder, emodModule.SelectedRepository, UserSession.RoleID, UserSession.UserID);
                    }
                }
                else if (UserSession.IsCreateFolder != 0)
                {
                    DMS.BusinessLogic.Folder objFolder = new DMS.BusinessLogic.Folder();
                    objFolder.FolderName = txtFolderName.Text.Trim();
                    objFolder.FolderDescription = txtFolderDescription.Text.Trim();
                    objFolder.UpdatedBy = UserSession.UserID;
                    objFolder.Status = Convert.ToInt32(ddlStatus.SelectedItem.Value);
                    objFolder.FolderID = UserSession.IsCreateFolder;

                    objUtility.Result = FolderManager.UpdateFolder(objFolder);
                }

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        if (UserSession.IsCreateFolder == 0)
                            Response.Redirect("../Folder/FolderView.aspx?Type=0&ID=1", false);
                        else
                            Response.Redirect("../Folder/FolderView.aspx?Type=1&ID=2", false);
                        break;

                    case Utility.ResultType.Error:
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

        protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (UserSession.IsCreateFolder == 0)
                    Response.Redirect("../Folder/FolderView.aspx?ID=1", false);
                else
                    Response.Redirect("../Folder/FolderView.aspx?ID=2", false);
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        #endregion

        #region Method
        private void ClearControl()
        {
            txtFolderName.Text = string.Empty;
            txtFolderDescription.Text = string.Empty;
            txtFolderName.Focus();
        }

        private void LoadControl(int intFolderID)
        {
            try
            {
                emodModule.Visible = false;
                DataTable objDataTable = new DataTable();
                objUtility.Result = FolderManager.SelectFolder(out objDataTable, intFolderID);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        txtFolderName.Text = objDataTable.Rows[0]["FolderName"].ToString().Trim();
                        txtFolderDescription.Text = objDataTable.Rows[0]["FolderDescription"].ToString().Trim();
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(objDataTable.Rows[0]["Status"].ToString()));
                        hdfFolderName.Value = txtFolderName.Text.Trim();
                        hdfMetaTemplateID.Value = objDataTable.Rows[0]["MetaTemplateID"].ToString().Trim();
                        hdfParentFolderID.Value = objDataTable.Rows[0]["ParentFolderID"].ToString().Trim();
                        hdfCategoryID.Value = objDataTable.Rows[0]["CategoryID"].ToString().Trim();
                        break;
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "Sorry ,No Data Is Available For This Folder .", MainMasterPage.MessageType.Error);
                        break;
                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }
        #endregion

        
    }
}