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

namespace DMS.User
{
    public partial class UserAccessRights : System.Web.UI.Page
    {

        #region Private Members
        RoleManager objRoleManager = new RoleManager();
        UserManager objUserManager = new UserManager();
        Utility objUtility = new Utility();
        public int RepositoryId = 0;
        #endregion

        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tvwUserAccessRights.Attributes.Add("onclick", "OnTreeClick(event)");
                if (Request["UserID"] != null && Request["RoleID"] != null)
                {
                    ViewState["UserID"] = Request["UserID"];
                    ViewState["RoleID"] = Request["RoleID"];
                }
                if (Request["RepositoryId"] != null)
                {
                    ViewState["RepositoryId"] = Request["RepositoryId"];
                    RepositoryId = Convert.ToInt32(ViewState["RepositoryId"]);
                }
                BindTree();
                Log.AuditLog(HttpContext.Current, "Visit", "User Access Rights");
            }

        }

        protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
        {
            DbTransaction objDbTransaction = BusinessLogic.Utility.GetTransaction;
            try
            {

                objUtility.Result = objUserManager.DeleteUserPermission(Convert.ToInt32(ViewState["UserID"]), objDbTransaction, BusinessLogic.UserPermission.DeleteIDType.UserID);

                switch (objUtility.Result)
                {

                    case Utility.ResultType.Success:

                        DataTable objDataTable = new DataTable();
                        objDataTable.Columns.Add("UserId", typeof(int));
                        objDataTable.Columns.Add("RepositoryID", typeof(int));
                        objDataTable.Columns.Add("MetaTemplateID", typeof(int));
                        objDataTable.Columns.Add("CategoryID", typeof(int));
                        objDataTable.Columns.Add("FolderID", typeof(int));


                        foreach (TreeNode objTreenode in tvwUserAccessRights.CheckedNodes)
                        {
                            string strPath = objTreenode.ValuePath.ToLower();


                            if (strPath.Contains("folder"))
                            {
                                if (strPath.Contains("folder/"))
                                {
                                    string strFolder = strPath.Split('/').LastOrDefault();

                                    DataRow drNewRow = objDataTable.NewRow();
                                    drNewRow["UserID"] = Convert.ToInt32(ViewState["UserID"]);
                                    drNewRow["RepositoryID"] = strPath.Split(new string[] { "repository/" }, StringSplitOptions.None).LastOrDefault().Split('/').FirstOrDefault();
                                    drNewRow["MetaTemplateID"] = strPath.Split(new string[] { "metatemplate/" }, StringSplitOptions.None).LastOrDefault().Split('/').FirstOrDefault();
                                    drNewRow["CategoryID"] = 0;
                                    drNewRow["FolderID"] = strFolder;
                                    objDataTable.Rows.Add(drNewRow);

                                }
                            }

                            else if (strPath.Contains("category"))
                            {
                                if (strPath.Contains("category/"))
                                {
                                    string strCategory = strPath.Split('/').LastOrDefault();
                                    DataRow drNewRow = objDataTable.NewRow();
                                    drNewRow["UserID"] = Convert.ToInt32(ViewState["UserID"]);
                                    drNewRow["RepositoryID"] = strPath.Split(new string[] { "repository/" }, StringSplitOptions.None).LastOrDefault().Split('/').FirstOrDefault();
                                    drNewRow["MetaTemplateID"] = strPath.Split(new string[] { "metatemplate/" }, StringSplitOptions.None).LastOrDefault().Split('/').FirstOrDefault();
                                    drNewRow["CategoryID"] = strCategory;
                                    drNewRow["FolderID"] = 0;
                                    objDataTable.Rows.Add(drNewRow);
                                }
                            }

                            else if (strPath.Contains("metatemplate"))
                            {
                                if (strPath.Contains("metatemplate/"))
                                {
                                    string strMetatemplate = strPath.Split('/').LastOrDefault();
                                    DataRow drNewRow = objDataTable.NewRow();
                                    drNewRow["UserID"] = Convert.ToInt32(ViewState["UserID"]);
                                    drNewRow["RepositoryID"] = strPath.Split(new string[] { "repository/" }, StringSplitOptions.None).LastOrDefault().Split('/').FirstOrDefault();
                                    drNewRow["MetaTemplateID"] = strMetatemplate;
                                    drNewRow["CategoryID"] = 0;
                                    drNewRow["FolderID"] = 0;
                                    objDataTable.Rows.Add(drNewRow);
                                }

                            }

                            else if (strPath.Contains("repository"))
                            {
                                if (strPath.Contains("repository/"))
                                {
                                    string strRepository = strPath.Split('/').LastOrDefault();
                                    DataRow drNewRow = objDataTable.NewRow();
                                    drNewRow["UserID"] = Convert.ToInt32(ViewState["UserID"]);
                                    drNewRow["RepositoryID"] = strRepository;
                                    drNewRow["MetaTemplateID"] = 0;
                                    drNewRow["CategoryID"] = 0;
                                    drNewRow["FolderID"] = 0;
                                    objDataTable.Rows.Add(drNewRow);
                                }

                            }

                        }


                        foreach (DataRow objDataRow in objDataTable.Rows)
                        {
                            BusinessLogic.UserPermission objUserPermission = new BusinessLogic.UserPermission();

                            objUserPermission.UserID = Convert.ToInt32(ViewState["UserID"]);
                            objUserPermission.RepositoryID = Convert.ToInt32(objDataRow["RepositoryID"]);
                            objUserPermission.MetaTemplateID = Convert.ToInt32(objDataRow["MetaTemplateID"]);
                            objUserPermission.FolderID = Convert.ToInt32(objDataRow["FolderID"]);
                            objUserPermission.CategoryID = Convert.ToInt32(objDataRow["CategoryID"]);

                            objUtility.Result = objUserManager.InsertUserPermission(objUserPermission, objDbTransaction);
                            switch (objUtility.Result)
                            {
                                case Utility.ResultType.Error:
                                    objDbTransaction.Rollback();
                                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                    return;
                                    break;
                            }

                        }
                        objDbTransaction.Commit();
                        Response.Redirect("../User/UserView.aspx?Type=2&ID=5", false);
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
            Response.Redirect("../User/UserView.aspx?ID=5", false);
        }
        #endregion

        #region Methods
        private void BindTree()
        {
            try
            {
                int intUserID = Convert.ToInt32(ViewState["UserID"]);

                DataTable dtUserSelect = new DataTable();
                objUserManager.SelectUser(out dtUserSelect, intUserID);
                if (dtUserSelect.Rows.Count > 0)
                {
                    lblPermissionforUser.Text = "Access Rights for User : " + dtUserSelect.Rows[0]["UserName"].ToString();
                    lblPermissionforRole.Text = "Access Rights for Role : " + dtUserSelect.Rows[0]["RoleName"].ToString();
                }

                DataTable objUserPermission;
                objUtility.Result = objUserManager.SelectUserPermission(out objUserPermission, intUserID);
                DataTable objRepositoryDataTable = new DataTable();
                if (objUtility.Result == Utility.ResultType.Error)
                { 
                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return; 
                }
                //Added for checking if user do not have permission then tree view shuld not display
                #region added by Vivek 15-11-17
                   
                //else if (objUtility.Result == Utility.ResultType.Success)
                //{
                    RepositoryManager objRepositoryManager = new RepositoryManager();
                //Added Extra parameter by Vivek
                    objUtility.Result = objRepositoryManager.SelectRepositoryForTreeview(out objRepositoryDataTable, Convert.ToInt32(ViewState["RoleID"]),RepositoryId);
                //}
                #endregion

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return;

                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning); return;

                }

                MetaTemplateManager objMetaTemplateManager = new MetaTemplateManager();
                DataTable objMetaTemplateDataTable;
                objUtility.Result = objMetaTemplateManager.SelectMetaTemplateForTreeview(out objMetaTemplateDataTable, Convert.ToInt32(ViewState["RoleID"]));
                if (objUtility.Result == Utility.ResultType.Error)
                { UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return; }

                CategoryManager objCategoryManager = new CategoryManager();
                DataTable objCategoryManagerDataTable;
                objUtility.Result = objCategoryManager.SelectCategoryForTreeview(out objCategoryManagerDataTable, Convert.ToInt32(ViewState["RoleID"]));
                if (objUtility.Result == Utility.ResultType.Error)
                { UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return; }

                FolderManager objFolderManager = new FolderManager();
                DataTable objFolderManagerDataTable;
                objUtility.Result = objFolderManager.SelectFolderForTreeView(out objFolderManagerDataTable, Convert.ToInt32(ViewState["RoleID"]));
                if (objUtility.Result == Utility.ResultType.Error)
                { UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return; }


             


                        TreeNode Repository = new TreeNode("Repository");
                        // Repository.ShowCheckBox = false;

                        DataView objRepositoryDataView = new DataView(objRepositoryDataTable);
                        objRepositoryDataView.Sort = "RepositoryName ASC";
                        objRepositoryDataView.RowFilter = "Status = 1 AND ID=RepositoryID";
                        foreach (DataRowView objRepositoryDataRow in objRepositoryDataView)
                        {
                            TreeNode objRepositoryNode = new TreeNode(objRepositoryDataRow["RepositoryName"].ToString(), objRepositoryDataRow["ID"].ToString());

                            if (objUserPermission.Rows.Count == 0)
                            {
                                objRepositoryNode.Checked = true;
                            }
                            else
                            {
                                DataRow[] drRepositoryID = objUserPermission.Select("RepositoryID=" + objRepositoryDataRow["ID"].ToString());
                                if (drRepositoryID.Length > 0)
                                {
                                    objRepositoryNode.Checked = true;
                                }

                            }

                            DataView objMetaTemplateDataView = new DataView(objMetaTemplateDataTable);
                            objMetaTemplateDataView.RowFilter = "RepositoryID=" + Convert.ToInt32(objRepositoryDataRow["ID"]) + " AND ID=MetaTemplateID ";

                            TreeNode MetaTemplate = new TreeNode("MetaTemplate");
                            //  MetaTemplate.ShowCheckBox = false;

                            foreach (DataRowView objMetaTemplateDataRow in objMetaTemplateDataView)
                            {
                                TreeNode objMetaTemplateNode = new TreeNode(objMetaTemplateDataRow["MetaTemplateName"].ToString(), objMetaTemplateDataRow["ID"].ToString());
                                if (objUserPermission.Rows.Count == 0)
                                {
                                    objMetaTemplateNode.Checked = true;
                                }
                                else
                                {
                                    DataRow[] drMetaTemplateID = objUserPermission.Select("MetaTemplateID=" + objMetaTemplateDataRow["ID"].ToString());
                                    if (drMetaTemplateID.Length > 0)
                                    {
                                        objMetaTemplateNode.Checked = true;
                                    }

                                }


                                TreeNode Category = AddCategory(objUserPermission, objCategoryManagerDataTable, objMetaTemplateDataRow);
                                // Category.ShowCheckBox = false;

                                TreeNode Folder = AddFolder(objUserPermission, objFolderManagerDataTable, objMetaTemplateDataRow);
                                // Folder.ShowCheckBox = false;

                                if (Category.ChildNodes.Count > 0)
                                    objMetaTemplateNode.ChildNodes.Add(Category);

                                if (Folder.ChildNodes.Count > 0)
                                    objMetaTemplateNode.ChildNodes.Add(Folder);

                                MetaTemplate.ChildNodes.Add(objMetaTemplateNode);
                            }

                            if (MetaTemplate.ChildNodes.Count > 0)
                                objRepositoryNode.ChildNodes.Add(MetaTemplate);

                            Repository.ChildNodes.Add(objRepositoryNode);
                        }
                        tvwUserAccessRights.Nodes.Add(Repository);
                       


            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }
        }

        private static TreeNode AddCategory(DataTable objUserPermission, DataTable objCategoryManagerDataTable, DataRowView objMetaTemplateDataRow)
        {
            DataView objCategoryManagerDataView = new DataView(objCategoryManagerDataTable);
            objCategoryManagerDataView.RowFilter = "MetaTemplateID=" + Convert.ToInt32(objMetaTemplateDataRow["ID"]) + "AND ID=CategoryID";


            TreeNode Category = new TreeNode("Category");

            foreach (DataRowView objCategoryManagerDataRow in objCategoryManagerDataView)
            {
                TreeNode objCategoryManagerNode = new TreeNode(objCategoryManagerDataRow["CategoryName"].ToString(), objCategoryManagerDataRow["ID"].ToString());
                if (objUserPermission.Rows.Count == 0)
                {
                    objCategoryManagerNode.Checked = true;
                }
                else
                {
                    DataRow[] drCategoryID = objUserPermission.Select("CategoryID=" + objCategoryManagerDataRow["ID"].ToString());
                    if (drCategoryID.Length > 0)
                    {
                        objCategoryManagerNode.Checked = true;
                    }

                }
                Category.ChildNodes.Add(objCategoryManagerNode);
            }
            return Category;
        }

        private TreeNode AddFolder(DataTable objUserPermission, DataTable objFolderManagerDataTable, DataRowView objMetaTemplateDataRow)
        {

            DataView objFolderManagerDataView = new DataView(objFolderManagerDataTable);
            objFolderManagerDataView.RowFilter = "MetaTemplateID=" + Convert.ToInt32(objMetaTemplateDataRow["ID"]) + " AND ParentFolderID=0 AND ID=FolderID";


            TreeNode Folder = new TreeNode("Folder");

            foreach (DataRowView objFolderDataRow in objFolderManagerDataView)
            {
                TreeNode objFolderNode = new TreeNode(objFolderDataRow["FolderName"].ToString(), objFolderDataRow["ID"].ToString());
                if (objUserPermission.Rows.Count == 0)
                {
                    objFolderNode.Checked = true;
                }
                else
                {
                    DataRow[] drFolderID = objUserPermission.Select("FolderID=" + objFolderDataRow["ID"].ToString());
                    if (drFolderID.Length > 0)
                    {
                        objFolderNode.Checked = true;
                    }
                }
                LoadSubFolder(objUserPermission, objFolderManagerDataTable, Convert.ToInt32(objFolderDataRow["ID"].ToString()), objFolderNode);

                Folder.ChildNodes.Add(objFolderNode);
            }
            return Folder;
        }

        private void LoadSubFolder(DataTable objUserPermission, DataTable objFolderManagerDataTable, int intFolderID, TreeNode objFolderNode)
        {
            try
            {
                DataView objFolderDataView = new DataView(objFolderManagerDataTable);
                objFolderDataView.RowFilter = "ParentFolderID=" + intFolderID;

                foreach (DataRowView objDataRow in objFolderDataView)
                {
                    TreeNode objChildItem = new TreeNode(objDataRow["FolderName"].ToString(), objDataRow["ID"].ToString());
                    if (objUserPermission.Rows.Count == 0)
                    {
                        objChildItem.Checked = true;
                    }
                    else
                    {
                        DataRow[] drFolderID = objUserPermission.Select("FolderID=" + objDataRow["ID"].ToString());
                        if (drFolderID.Length > 0)
                        {
                            objChildItem.Checked = true;
                        }
                    }
                    LoadSubFolder(objUserPermission, objFolderManagerDataTable, Convert.ToInt32(objDataRow["ID"].ToString()), objChildItem);
                    objFolderNode.ChildNodes.Add(objChildItem);
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