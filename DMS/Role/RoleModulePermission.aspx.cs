using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using DMS.BusinessLogic;


namespace DMS.Role
{
    public partial class RoleModulePermission : System.Web.UI.Page
    {
        #region Private Members
        RoleManager objRoleManager = new RoleManager();
        UserManager objUserManager = new UserManager();
        Utility objUtility = new Utility();
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tvwRolePermission.Attributes.Add("onclick", "OnTreeClick(event)");
                if (Request["RoleID"] != null)
                {
                    ViewState["RoleID"] = Request["RoleID"];
                }
                if (Session["TempRoleName"] != null)
                    lbl_RoleName.Text = Session["TempRoleName"].ToString();

                BindTree();
                Log.AuditLog(HttpContext.Current, "Visit", "Role Module Permission");
            }
        }

        protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
        {
            DbTransaction objDbTransaction = BusinessLogic.Utility.GetTransaction;
            try
            {
                objUtility.Result = objRoleManager.DeleteRoleModule(Convert.ToInt32(ViewState["RoleID"]), objDbTransaction);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:

                        foreach (TreeNode objTreenode in tvwRolePermission.CheckedNodes)
                        {
                            RoleModule objRoleModule = new RoleModule();
                            objRoleModule.RoleID = Convert.ToInt32(ViewState["RoleID"]);
                            objRoleModule.ModuleID = Convert.ToInt32(objTreenode.Value);

                            objUtility.Result = objRoleManager.InsertRoleModule(objRoleModule, objDbTransaction);

                            switch (objUtility.Result)
                            {
                                case Utility.ResultType.Error:
                                    objDbTransaction.Rollback();
                                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                    return;
                                    break;
                            }
                        }

                        objUtility.Result = objUserManager.DeleteUserModule(Convert.ToInt32(ViewState["RoleID"]), objDbTransaction, UserModule.DeleteIDType.RoleID);

                        switch (objUtility.Result)
                        {
                            case Utility.ResultType.Error:
                                objDbTransaction.Rollback();
                                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                return;
                                break;
                        }


                        objDbTransaction.Commit();
                        foreach (TreeNode objTreenode in tvwRolePermission.Nodes)
                        {
                            if (objTreenode.Checked == true)
                            {
                                Log.AuditLog(HttpContext.Current, "Role Permission " + objTreenode.Text + " Assigned to " + Session["TempRoleName"] + "", "RoleView");
                            }
                            else
                            {
                                Log.AuditLog(HttpContext.Current, "Role Permission " + objTreenode.Text + " Removed to " + Session["TempRoleName"] + "", "RoleView");
                            }
                        }
                        Response.Redirect("../Role/RoleView.aspx?Type=2&ID=4", false);
                        break;

                    case Utility.ResultType.Error:
                        objDbTransaction.Rollback();
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;

                }

            }
            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }


        }

        protected void ibtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("../Role/RoleView.aspx?ID=4", false);
        }

        #endregion

        #region Methods

        private void BindTree()
        {
            try
            {
                int intRoleId = Convert.ToInt32(ViewState["RoleID"]);
                DataTable dtRoleSelect = new DataTable();
                objRoleManager.SelectRole(out dtRoleSelect, intRoleId);
                if (dtRoleSelect.Rows.Count > 0)
                {
                    lblPermissionforRole.Text = "Permission for Role : " + dtRoleSelect.Rows[0]["RoleName"].ToString();
                }

                DataTable objDataTable = new DataTable();
                objUtility.Result = objRoleManager.SelectRoleModule(out objDataTable, Convert.ToInt32(ViewState["RoleID"]), Utility.Control.Treeview);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:

                        DataView objDataView = new DataView(objDataTable);
                        objDataView.RowFilter = "ParentModuleID=0";
                        objDataView.Sort = "DisplayOrder";

                        foreach (DataRowView objDataRow in objDataView)
                        {
                            TreeNode objParentNode = new TreeNode(objDataRow["ModuleName"].ToString(), objDataRow["ID"].ToString());
                            if (objDataRow["RoleID"].ToString() != "")
                            {
                                objParentNode.Checked = true;
                            }
                            LoadTreeView(objDataTable, Convert.ToInt32(objDataRow["ID"].ToString()), objParentNode);

                            tvwRolePermission.Nodes.Add(objParentNode);
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

        private void LoadTreeView(DataTable objDataTable, int intTreeviewID, TreeNode objParentNode)
        {
            try
            {
                DataView objDataView = new DataView(objDataTable);
                objDataView.RowFilter = "ParentModuleID=" + intTreeviewID;
                objDataView.Sort = "DisplayOrder";

                foreach (DataRowView objDataRow in objDataView)
                {
                    TreeNode objChildItem = new TreeNode(objDataRow["ModuleName"].ToString(), objDataRow["ID"].ToString());
                    if (objDataRow["RoleID"].ToString() != "")
                    {
                        objChildItem.Checked = true;
                    }
                    LoadTreeView(objDataTable, Convert.ToInt32(objDataRow["ID"].ToString()), objChildItem);
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