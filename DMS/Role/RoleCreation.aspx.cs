using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
using System.Data.Common;
using System.Drawing;


namespace DMS.Role
{
    public partial class RoleCreation : System.Web.UI.Page
    {
        #region Private Members
        BusinessLogic.RoleManager objRoleManager = new BusinessLogic.RoleManager();
        Utility objUtility = new Utility();
        #endregion

        # region Methods

        protected void Page_Load(object sender, EventArgs e)
        {

            if(!IsPostBack)
            {
                this.Page.Form.DefaultButton = ibtnSubmit.UniqueID;
                txtRoleName.Focus();
                LoadDropdown();

                lblTitle.Text = "Create Role";
                if(UserSession.IsCreateRole!=0)
                {
                    LoadControl(UserSession.IsCreateRole);
                    ddlStatus.BackColor = Color.LightSlateGray;
                    ddlStatus.Enabled = false;
                    lblTitle.Text = "Update Role";
                    ddlrepository.Enabled = false;
                }
                Log.AuditLog(HttpContext.Current, "Visit", "Role Creation");
            }
        }

        private void LoadDropdown()
        {
            Utility.LoadRepository(ddlrepository);
            Utility.LoadStatus(ddlStatus);

            DataTable objTable = new DataTable();
            objUtility.Result = DMS.BusinessLogic.Role.SelectRepositoryUsers(out objTable);
            switch (objUtility.Result)
            {
                case Utility.ResultType.Success:
                    cblRoleRights.DataSource = objTable;
                    cblRoleRights.DataTextField = "RoleName";
                    cblRoleRights.DataValueField = "ID";
                    cblRoleRights.DataBind();
                    break;
                case Utility.ResultType.Error:
                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                    break;
            }
        }

        private void LoadControl(int intRoleID)
        {
            try
            {
                DataTable objDataTable = new DataTable();
                objUtility.Result = objRoleManager.SelectRole(out objDataTable, intRoleID);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        txtRoleName.Text = objDataTable.Rows[0]["RoleName"].ToString().Trim();
                        txtDisplayName.Text = objDataTable.Rows[0]["DisplayName"].ToString().Trim();
                        ddlRoleType.SelectedIndex = ddlRoleType.Items.IndexOf(ddlRoleType.Items.FindByValue(objDataTable.Rows[0]["RoleType"].ToString()));
                        if (ddlRoleType.SelectedValue == "2")
                        {
                            trRoleRights.Visible = true;
                        }
                        else
                        {
                            trRoleRights.Visible = false;
                        }
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(objDataTable.Rows[0]["Status"].ToString()));
                        hdfRoleName.Value = txtRoleName.Text;
                        foreach (ListItem objListItem in cblRoleRights.Items)
                        {
                            if (objDataTable.Select("RepositoryUserID=" + Convert.ToInt32(objListItem.Value)).Length > 0)
                            {
                                objListItem.Selected = true;
                            }
                        }
                        break;
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "Sorry ,No Data Is Available For This Role .", MainMasterPage.MessageType.Error);
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

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (UserSession.IsCreateRole == 0 || hdfRoleName.Value.ToLower() != txtRoleName.Text.Trim().ToLower())
                {
                    objUtility.Result = objRoleManager.SelectRole(txtRoleName.Text.Trim());
                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            UserSession.DisplayMessage(this, "Role Name Already Exist .", MainMasterPage.MessageType.Success);
                            return;
                            break;

                        case Utility.ResultType.Error:
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                            break;
                    }
                }

                DMS.BusinessLogic.Role objRole = new DMS.BusinessLogic.Role();

                if (UserSession.IsCreateRole == 0)
                {
                    objRole.RoleName = txtRoleName.Text.Trim();
                    objRole.DisplayName = txtDisplayName.Text.Trim();
                    objRole.RoleType = Convert.ToInt32(ddlRoleType.SelectedValue);
                    objRole.Status = ddlStatus.SelectedValue;
                    objRole.CreatedBy = UserSession.UserID;

                    objUtility.Result = objRoleManager.InsertRole(objRole);
                   
                    objUtility.Result = objRoleManager.InsertRolePermission(objRole.RoleID,Convert.ToInt32(ddlrepository.SelectedValue),0,0,0,UserSession.UserID);
                    
                    if (objRole.RoleType == 3)
                    {
                        foreach (ListItem objListItem in cblRoleRights.Items)
                        {
                            if (objListItem.Selected == true)
                            {
                                DMS.BusinessLogic.Role.InsertRoleRights(new RoleRights() { RepositoryAdminID = objRole.RoleID, RepositoryUserID = Convert.ToInt32(objListItem.Value) });
                            }
                        }
                    }
                }
                else if (UserSession.IsCreateRole != 0)
                {
                    objRole.RoleID = UserSession.IsCreateRole;
                    objRole.RoleName = txtRoleName.Text.Trim();
                    objRole.DisplayName = txtDisplayName.Text.Trim();
                    objRole.RoleType = Convert.ToInt32(ddlRoleType.SelectedValue);
                    objRole.Status = ddlStatus.SelectedValue;
                    objRole.UpdatedBy = UserSession.UserID;

                    objUtility.Result = objRoleManager.UpdateRole(objRole);

                    DMS.BusinessLogic.Role.DeleteRoleRights(new RoleRights() { RepositoryAdminID = objRole.RoleID });
                    if (objRole.RoleType == 2)
                    {
                        foreach (ListItem objListItem in cblRoleRights.Items)
                        {
                            if (objListItem.Selected == true)
                            {
                                DMS.BusinessLogic.Role.InsertRoleRights(new RoleRights() { RepositoryAdminID = objRole.RoleID, RepositoryUserID = Convert.ToInt32(objListItem.Value) });
                            }
                        }
                    }
                }

                    switch(objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            if (UserSession.IsCreateRole == 0)
                            {
                                if (UserSession.RoleID == 202)
                                {
                                    DbTransaction objDbTransaction = BusinessLogic.Utility.GetTransaction;
                                    DMS.BusinessLogic.RolePermission objRolePermission = new RolePermission();
                                    objRolePermission.RoleID = objRole.RoleID;
                                    objRolePermission.RepositoryID = 53;
                                    objRolePermission.MetaTemplateID = 0;
                                    objRolePermission.FolderID = 0;
                                    objRolePermission.CategoryID = 0;
                                    objUtility.Result = DMS.BusinessLogic.RolePermission.Insert(objRolePermission, objDbTransaction);
                                    objDbTransaction.Commit();
                                }
                                Response.Redirect("../Role/RoleView.aspx?Type=0&ID=1", false);
                            }
                            else
                                Response.Redirect("../Role/RoleView.aspx?Type=1&ID=2", false);
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
            if (UserSession.IsCreateRole == 0)
                Response.Redirect("../Role/RoleView.aspx?ID=1", false);
            else
                Response.Redirect("../Role/RoleView.aspx?ID=2", false);
        }

        protected void ddlRoleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRoleType.SelectedValue == "2")
            {
                trRoleRights.Visible = true;
            }
            else
            {
                trRoleRights.Visible = false;
            }
        }
        # endregion
    }
}