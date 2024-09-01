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
    public partial class RoleAccessRights : System.Web.UI.Page
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
                tvwRoleAccessRights.Attributes.Add("onclick", "OnTreeClick(event)");
                if (Request["RoleID"] != null)
                {
                    ViewState["RoleID"] = Request["RoleID"];
                }
                //Added by Vivek for getting RepositoryID
                if (Request["RepositoryId"] != null || Convert.ToInt32(Request["RepositoryId"]) != 0)
                {
                    ViewState["RepositoryId"] = Request["RepositoryId"];
                    RepositoryId = Convert.ToInt32(ViewState["RepositoryId"]);
                    Session["RoleRepositoryId"] = RepositoryId;
                }
                BindTree();
                Log.AuditLog(HttpContext.Current, "Visit", "RoleAceess Rights");
            }
        }

        protected void ibtnSave_Click(object sender, ImageClickEventArgs e)
        {
            DbTransaction objDbTransaction = BusinessLogic.Utility.GetTransaction;
            try
            {
                objUtility.Result = objRoleManager.UpdateRolePermission(Convert.ToInt32(ViewState["RoleID"]), Convert.ToInt32(Session["RoleRepositoryId"]), UserSession.UserID, objDbTransaction);
                objUtility.Result = objRoleManager.DeleteRolePermission(Convert.ToInt32(ViewState["RoleID"]), Convert.ToInt32(Session["RoleRepositoryId"]), objDbTransaction);

                switch (objUtility.Result)
                {
                                    case Utility.ResultType.Success:

                                    DataTable objDataTable = new DataTable();
                                    objDataTable.Columns.Add("RoleID",typeof(int));
                                    objDataTable.Columns.Add("RepositoryID",typeof(int));
                                    objDataTable.Columns.Add("MetaTemplateID",typeof(int));
                                    objDataTable.Columns.Add("CategoryID",typeof(int));
                                    objDataTable.Columns.Add("FolderID",typeof(int));

                                    foreach (TreeNode objTreenode in tvwRoleAccessRights.CheckedNodes)
                                    {
                                    string strPath = objTreenode.ValuePath.ToLower();


                                    if (strPath.Contains("folder"))
                                    {
                                    if (strPath.Contains("folder/"))
                                    {
                                    string strFolder = strPath.Split('/').LastOrDefault();

                                    DataRow drNewRow = objDataTable.NewRow();
                                    drNewRow["RoleID"] = Convert.ToInt32(ViewState["RoleID"]);
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
                                    drNewRow["RoleID"] = Convert.ToInt32(ViewState["RoleID"]);
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
                                    drNewRow["RoleID"] = Convert.ToInt32(ViewState["RoleID"]);
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
                                    drNewRow["RoleID"] = Convert.ToInt32(ViewState["RoleID"]);
                                    drNewRow["RepositoryID"] = strRepository;
                                    drNewRow["MetaTemplateID"] = 0;
                                    drNewRow["CategoryID"] = 0;
                                    drNewRow["FolderID"] = 0;
                                    objDataTable.Rows.Add(drNewRow);
                                    }

                                    }

                                    }
                                    foreach(DataRow objDataRow in objDataTable.Rows)
                                    {
                                        RolePermission objRolePermission = new RolePermission();
                                        objRolePermission.RoleID = Convert.ToInt32(ViewState["RoleID"]);
                                        objRolePermission.RepositoryID = Convert.ToInt32(objDataRow["RepositoryID"]);
                                        objRolePermission.MetaTemplateID = Convert.ToInt32(objDataRow["MetaTemplateID"]);
                                        objRolePermission.FolderID = Convert.ToInt32(objDataRow["FolderID"]);
                                        objRolePermission.CategoryID = Convert.ToInt32(objDataRow["CategoryID"]);
                                        objRolePermission.UserId = UserSession.UserID;
                                        objUtility.Result = objRoleManager.InsertRolePermission(objRolePermission, objDbTransaction);
                                        switch (objUtility.Result)
                                        {
                                            case Utility.ResultType.Error:
                                                objDbTransaction.Rollback();
                                                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                                                return;
                                                break;
                                        }

                                        objUtility.Result = objUserManager.DeleteUserPermission(Convert.ToInt32(ViewState["RoleID"]), objDbTransaction,UserPermission.DeleteIDType.RoleID);

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
                                    Session["RoleRepositoryId"] = null;
                                    Response.Redirect("../Role/RoleView.aspx?Type=2&ID=5", false);
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
            Response.Redirect("../Role/RoleView.aspx?ID=5", false);
        }

        #endregion

        #region Methods

        private void BindTree()
        {
            try
            {
                RepositoryManager objRepositoryManager = new RepositoryManager();
                
                int intRoleId = Convert.ToInt32(ViewState["RoleID"]);
                DataTable dtRoleSelect = new DataTable();
                objRoleManager.SelectRole(out dtRoleSelect, intRoleId);
                if (dtRoleSelect.Rows.Count > 0)
                {
                    lblPermissionforRole.Text = "Access Rights for Role : " + dtRoleSelect.Rows[0]["RoleName"].ToString();
                }

                DataTable objRepositoryDataTable;
                //objUtility.Result = objRepositoryManager.SelectRepositoryForTreeview(out objRepositoryDataTable, Convert.ToInt32(ViewState["RoleID"]));

                objUtility.Result = objRepositoryManager.SelectRepositoryForTreeview(out objRepositoryDataTable, UserSession.RoleID, RepositoryId);
                if (objRepositoryDataTable.Rows.Count > 0)
                    RepositoryId = Convert.ToInt32(objRepositoryDataTable.Rows[0]["ID"]);
                //commented by Vivek 23-11-2017
                //objUtility.Result = objRepositoryManager.SelectRepositoryForTreeview(out objRepositoryDataTable, UserSession.RoleID);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return;

                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning); return;

                }


                MetaTemplateManager objMetaTemplateManager = new MetaTemplateManager();
                DataTable objMetaTemplateDataTable;
                objUtility.Result = objMetaTemplateManager.SelectMetaTemplateForTreeviewForRepository(out objMetaTemplateDataTable, Convert.ToInt32(ViewState["RoleID"]), RepositoryId);
                if (objUtility.Result == Utility.ResultType.Error)
                { UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return; }

                CategoryManager objCategoryManager = new CategoryManager();
                DataTable objCategoryManagerDataTable;
                objUtility.Result = objCategoryManager.SelectCategoryByRepositoryForTreeview(out objCategoryManagerDataTable, Convert.ToInt32(ViewState["RoleID"]), RepositoryId);
                if (objUtility.Result == Utility.ResultType.Error)
                { UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return; }

                FolderManager objFolderManager = new FolderManager();
                DataTable objFolderManagerDataTable;
                objUtility.Result = objFolderManager.SelectFolderForTreeView(out objFolderManagerDataTable, Convert.ToInt32(ViewState["RoleID"]));
                if (objUtility.Result == Utility.ResultType.Error)
                { UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return; }



                TreeNode Repository = new TreeNode("Repository");

                DataView objRepositoryDataView = new DataView(objRepositoryDataTable);
                objRepositoryDataView.Sort = "RepositoryName ASC";
                objRepositoryDataView.RowFilter = "Status = 1";
                foreach (DataRowView objRepositoryDataRow in objRepositoryDataView)
                {
                    TreeNode objRepositoryNode = new TreeNode(objRepositoryDataRow["RepositoryName"].ToString(), objRepositoryDataRow["ID"].ToString());

                    if (objRepositoryDataRow["RepositoryID"].ToString() != "")
                    {
                        objRepositoryNode.Checked = true;
                    }

                    DataView objMetaTemplateDataView = new DataView(objMetaTemplateDataTable);
                    objMetaTemplateDataView.RowFilter = "RepositoryID=" + Convert.ToInt32(objRepositoryDataRow["ID"]) + "";

                    TreeNode MetaTemplate = new TreeNode("MetaTemplate");

                    foreach (DataRowView objMetaTemplateDataRow in objMetaTemplateDataView)
                    {
                        TreeNode objMetaTemplateNode = new TreeNode(objMetaTemplateDataRow["MetaTemplateName"].ToString(), objMetaTemplateDataRow["ID"].ToString());
                        if (objMetaTemplateDataRow["MetaTemplateID"].ToString() != "")
                        {
                            objMetaTemplateNode.Checked = true;
                        }
                        TreeNode Category = AddCategory(objCategoryManagerDataTable, objMetaTemplateDataRow);

                        TreeNode Folder = AddFolder(objFolderManagerDataTable, objMetaTemplateDataRow);

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
                tvwRoleAccessRights.Nodes.Add(Repository);

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }
        }
        //private void BindTree()
        //{
        //    try
        //    {
        //        RepositoryManager objRepositoryManager = new RepositoryManager();
        //        DataTable objRepositoryDataTable;
        //        //objUtility.Result = objRepositoryManager.SelectRepositoryForTreeview(out objRepositoryDataTable, Convert.ToInt32(ViewState["RoleID"]));
        //        objUtility.Result = objRepositoryManager.SelectRepositoryForTreeview(out objRepositoryDataTable, UserSession.RoleID);

        //        switch (objUtility.Result)
        //        {
        //            case Utility.ResultType.Error:
        //                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return;

        //            case Utility.ResultType.Failure:
        //                UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning); return;

        //        }


        //        MetaTemplateManager objMetaTemplateManager = new MetaTemplateManager();
        //        DataTable objMetaTemplateDataTable;
        //        objUtility.Result = objMetaTemplateManager.SelectMetaTemplateForTreeview(out objMetaTemplateDataTable, Convert.ToInt32(ViewState["RoleID"]));
        //        if (objUtility.Result == Utility.ResultType.Error)
        //        { UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return; }

        //        CategoryManager objCategoryManager = new CategoryManager();
        //        DataTable objCategoryManagerDataTable;
        //        objUtility.Result = objCategoryManager.SelectCategoryForTreeview(out objCategoryManagerDataTable, Convert.ToInt32(ViewState["RoleID"]));
        //        if (objUtility.Result == Utility.ResultType.Error)
        //        { UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return; }

        //        FolderManager objFolderManager = new FolderManager();
        //        DataTable objFolderManagerDataTable;
        //        objUtility.Result = objFolderManager.SelectFolderForTreeView(out objFolderManagerDataTable, Convert.ToInt32(ViewState["RoleID"]));
        //        if (objUtility.Result == Utility.ResultType.Error)
        //        { UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error); return; }



        //        TreeNode Repository = new TreeNode("Repository");

        //        DataView objRepositoryDataView = new DataView(objRepositoryDataTable);
        //        objRepositoryDataView.Sort = "RepositoryName ASC";
        //        objRepositoryDataView.RowFilter = "Status = 1";
        //        foreach (DataRowView objRepositoryDataRow in objRepositoryDataView)
        //        {
        //            TreeNode objRepositoryNode = new TreeNode(objRepositoryDataRow["RepositoryName"].ToString(), objRepositoryDataRow["ID"].ToString());

        //            if (objRepositoryDataRow["RepositoryID"].ToString() != "")
        //            {
        //                objRepositoryNode.Checked = true;
        //            }

        //            DataView objMetaTemplateDataView = new DataView(objMetaTemplateDataTable);
        //            objMetaTemplateDataView.RowFilter = "RepositoryID=" + Convert.ToInt32(objRepositoryDataRow["ID"]) + "";

        //            TreeNode MetaTemplate = new TreeNode("MetaTemplate");

        //            foreach (DataRowView objMetaTemplateDataRow in objMetaTemplateDataView)
        //            {
        //                TreeNode objMetaTemplateNode = new TreeNode(objMetaTemplateDataRow["MetaTemplateName"].ToString(), objMetaTemplateDataRow["ID"].ToString());
        //                if (objMetaTemplateDataRow["MetaTemplateID"].ToString() != "")
        //                {
        //                    objMetaTemplateNode.Checked = true;
        //                }
        //                //DataView objCategoryDataView = new DataView(objCategoryManagerDataTable);
        //                //objCategoryDataView.RowFilter = "MetatemplateID=" + Convert.ToInt32(objMetaTemplateDataRow["ID"]) + "";


        //                TreeNode Category = AddCategory(objCategoryManagerDataTable, objMetaTemplateDataRow);

        //                TreeNode Folder = AddFolder(objFolderManagerDataTable, objMetaTemplateDataRow);

        //                if (Category.ChildNodes.Count > 0)
        //                    objMetaTemplateNode.ChildNodes.Add(Category);

        //                if (Folder.ChildNodes.Count > 0)
        //                    objMetaTemplateNode.ChildNodes.Add(Folder);

        //                MetaTemplate.ChildNodes.Add(objMetaTemplateNode);
        //            }
        //            if (MetaTemplate.ChildNodes.Count > 0)
        //                objRepositoryNode.ChildNodes.Add(MetaTemplate);

        //            Repository.ChildNodes.Add(objRepositoryNode);
        //        }
        //        tvwRoleAccessRights.Nodes.Add(Repository);




        //    }
        //    catch (Exception ex)
        //    {
        //        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
        //        LogManager.ErrorLog(Utility.LogFilePath, ex);

        //    }
        //}

        private TreeNode AddCategory(DataTable objCategoryManagerDataTable, DataRowView objMetaTemplateDataRow)
        {

            DataView objCategoryManagerDataView = new DataView(objCategoryManagerDataTable);
            objCategoryManagerDataView.RowFilter = "MetaTemplateID=" + Convert.ToInt32(objMetaTemplateDataRow["ID"]) + "";


            TreeNode Category = new TreeNode("Category");

            foreach (DataRowView objCategoryManagerDataRow in objCategoryManagerDataView)
            {
                TreeNode objCategoryManagerNode = new TreeNode(objCategoryManagerDataRow["CategoryName"].ToString(), objCategoryManagerDataRow["ID"].ToString());
                if (objCategoryManagerDataRow["CategoryID"].ToString() != "")
                {
                    objCategoryManagerNode.Checked = true;
                }
                Category.ChildNodes.Add(objCategoryManagerNode);
            }

            return Category;
        }

        private TreeNode AddFolder(DataTable objFolderManagerDataTable, DataRowView objMetaTemplateDataRow)
            //DataRowView objMetaTemplateDataRow)
        {

            DataView objFolderManagerDataView = new DataView(objFolderManagerDataTable);
            objFolderManagerDataView.RowFilter = "MetaTemplateID=" + Convert.ToInt32(objMetaTemplateDataRow["ID"]) + " AND ParentFolderID=0 ";
           // objFolderManagerDataView.RowFilter = "CategoryID=" + Convert.ToInt32(objCategoryDataRow["ID"]) + " AND ParentFolderID=0 ";


            TreeNode Folder = new TreeNode("Folder");

            foreach (DataRowView objFolderDataRow in objFolderManagerDataView)
            {
                TreeNode objFolderNode = new TreeNode(objFolderDataRow["FolderName"].ToString(), objFolderDataRow["ID"].ToString());
                if (objFolderDataRow["FolderID"].ToString() != "")
                {
                    objFolderNode.Checked = true;
                }
                LoadSubFolder(objFolderManagerDataTable, Convert.ToInt32(objFolderDataRow["ID"].ToString()), objFolderNode);

                Folder.ChildNodes.Add(objFolderNode);
            }
            return Folder;
        }

        private void LoadSubFolder(DataTable objFolderManagerDataTable, int intFolderID, TreeNode objFolderNode)
        {
            try
            {
                DataView objFolderDataView = new DataView(objFolderManagerDataTable);
                objFolderDataView.RowFilter = "ParentFolderID=" + intFolderID;

                foreach (DataRowView objDataRow in objFolderDataView)
                {
                    TreeNode objChildItem = new TreeNode(objDataRow["FolderName"].ToString(), objDataRow["ID"].ToString());
                    if (objDataRow["FolderID"].ToString() != "")
                    {
                        objChildItem.Checked = true;
                    }
                    LoadSubFolder(objFolderManagerDataTable, Convert.ToInt32(objDataRow["ID"].ToString()), objChildItem);
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