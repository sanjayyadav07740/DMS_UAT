using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;
using System.Data.Common;

namespace DMS.User
{
    public partial class UserPermission : System.Web.UI.Page
    {
        #region Private Members
        UserManager objUserManager = new UserManager();
        RoleManager objRoleManager = new RoleManager();
        Utility objUtility = new Utility();
        #endregion


        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tvwUserPermission.Attributes.Add("onclick", "OnTreeClick(event)");
                if (Request["UserID"] != null && Request["RoleID"] != null)
                {
                    ViewState["UserID"] = Request["UserID"];
                    ViewState["RoleID"] = Request["RoleID"];
                }
                BindTree();
                Log.AuditLog(HttpContext.Current, "Visit", "User Permission");
            }
        }

        protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DbTransaction objDbTransaction = BusinessLogic.Utility.GetTransaction;

                objUtility.Result = objUserManager.DeleteUserModule(Convert.ToInt32(ViewState["UserID"]), objDbTransaction, UserModule.DeleteIDType.UserID);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        foreach (TreeNode objTreenode in tvwUserPermission.CheckedNodes)
                        {
                            UserModule objUserModule = new UserModule();
                            objUserModule.UserID = Convert.ToInt32(ViewState["UserID"]);
                            objUserModule.ModuleID = Convert.ToInt32(objTreenode.Value);

                            objUtility.Result = objUserManager.InsertUserModule(objUserModule, objDbTransaction);

                            if (objUtility.Result == Utility.ResultType.Error)
                            {
                                objDbTransaction.Rollback();
                                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                return;
                            }
                        }
                        objDbTransaction.Commit();
                        Response.Redirect("../User/UserView.aspx?Type=2&ID=4", false);
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

        protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("../User/UserView.aspx?ID=4", false);
        }

        #endregion

        #region Methods

        private void BindTree()
        {
            try
            {
                int intUserID = Convert.ToInt32(ViewState["UserID"]);
                List<int> UserModules = objUserManager.SelectUserModule(intUserID);

                DataTable dtUserSelect = new DataTable();
                objUserManager.SelectUser(out dtUserSelect, intUserID);
                if(dtUserSelect.Rows.Count > 0)
                {
                    lblPermissionforUser.Text = "Permission for User : " + dtUserSelect.Rows[0]["UserName"].ToString();
                    lblPermissionforRole.Text = "Permission for Role : " + dtUserSelect.Rows[0]["RoleName"].ToString();
                }

                DataTable objDataTable = new DataTable();
                objUtility.Result = objRoleManager.SelectRoleModule(out objDataTable, Convert.ToInt32(ViewState["RoleID"]), Utility.Control.Treeview);


                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:

                        DataView objDataView = new DataView(objDataTable);
                        objDataView.RowFilter = "ParentModuleID=0 AND RoleID=" + Convert.ToInt32(ViewState["RoleID"]);
                        objDataView.Sort = "DisplayOrder";

                        foreach (DataRowView objDataRow in objDataView)
                        {
                            TreeNode objParentNode = new TreeNode(objDataRow["ModuleName"].ToString(), objDataRow["ID"].ToString());

                            if (UserModules.Count == 0)
                            {
                                objParentNode.Checked = true;
                            }
                            else
                            {
                                if (UserModules.Contains(Convert.ToInt32(objDataRow["ID"])))
                                    objParentNode.Checked = true;
                            }

                            LoadTreeView(objDataTable, Convert.ToInt32(objDataRow["ID"].ToString()), objParentNode, UserModules);
                            tvwUserPermission.Nodes.Add(objParentNode);
                        }

                        break;
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
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

        private void LoadTreeView(DataTable objDataTable, int intTreeviewID, TreeNode objParentNode, List<int> UserModules)
        {
            try
            {
                DataView objDataView = new DataView(objDataTable);
                objDataView.RowFilter = "ParentModuleID=" + intTreeviewID + "AND RoleID=" + Convert.ToInt32(ViewState["RoleID"]);
                objDataView.Sort = "DisplayOrder";

                foreach (DataRowView objDataRow in objDataView)
                {
                    TreeNode objChildItem = new TreeNode(objDataRow["ModuleName"].ToString(), objDataRow["ID"].ToString());

                    if (UserModules.Count == 0)
                    {
                        objChildItem.Checked = true;
                    }
                    else
                    {
                        if (UserModules.Contains(Convert.ToInt32(objDataRow["ID"])))
                            objChildItem.Checked = true;
                    }

                    LoadTreeView(objDataTable, Convert.ToInt32(objDataRow["ID"].ToString()), objChildItem, UserModules);
                    objParentNode.ChildNodes.Add(objChildItem);
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }

        }



        #endregion

    }
}