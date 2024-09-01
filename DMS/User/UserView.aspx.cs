using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace DMS.User
{
    public partial class UserView : System.Web.UI.Page
    {
        #region Private Member
        Utility objUtility = new Utility();
        DocumentManager objDocumentManager = new DocumentManager();
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            ((AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("tsmManager")).RegisterPostBackControl(ibtnExportData);

            if(!IsPostBack)
            {
                if (Request["Type"] != null)
                {
                    //if (Request["Type"].ToString().Trim() == "0")
                    //{
                    //    Log.AuditLog(HttpContext.Current, "User Created", "UserCreation");
                    //    UserSession.DisplayMessage(this, "User Is Created Successfully .", MainMasterPage.MessageType.Success);
                    //}
                    //else
                    if (Request["Type"].ToString().Trim() == "1")
                    {
                        Log.AuditLog(HttpContext.Current, "User Updated", "UserCreation(For Updation)");
                        UserSession.DisplayMessage(this, "User Is Updated Successfully .", MainMasterPage.MessageType.Success);
                    }
                    //else if (Request["Type"].ToString().Trim() == "2")
                    //{
                    //    Log.AuditLog(HttpContext.Current, "Permissions Assigned", "UserPermission");
                    //    UserSession.DisplayMessage(this, "Permissions has been Assigned Successfully.", MainMasterPage.MessageType.Success);
                    //}
                }

                if (Request["ID"] != null)
                {
                    switch (Request["ID"].ToString().Trim())
                    {
                        case "1":
                            ibtnCreateUser.Visible = true;
                            ibtnExportData.Visible = false;
                            gvwUsers.Columns[14].Visible = true;
                            gvwUsers.Columns[15].Visible = false;
                            gvwUsers.Columns[16].Visible = false;
                            gvwUsers.Columns[17].Visible = false;
                            gvwUsers.Columns[18].Visible = false;
                            gvwUsers.Columns[19].Visible = false;
                            gvwUsers.Columns[20].Visible = false;
                            gvwUsers.Columns[21].Visible = false;
                            gvwUsers.Columns[22].Visible = false;
                            gvwUsers.Columns[23].Visible = false;
                            lblTitle.Text = "Create User";
                            break;
                        case "2":
                            ibtnCreateUser.Visible = false;
                            ibtnExportData.Visible = false;
                            gvwUsers.Columns[14].Visible = true;
                            gvwUsers.Columns[15].Visible = true;
                            gvwUsers.Columns[16].Visible = false;
                            gvwUsers.Columns[17].Visible = false;
                            //gvwUsers.Columns[18].Visible = false;
                            //gvwUsers.Columns[19].Visible = false;
                            //gvwUsers.Columns[20].Visible = false;
                            //gvwUsers.Columns[21].Visible = false;
                            //gvwUsers.Columns[22].Visible = false;
                            //gvwUsers.Columns[23].Visible = false;
                            gvwUsers.Columns[18].Visible = true;
                            gvwUsers.Columns[19].Visible = true;
                            gvwUsers.Columns[20].Visible = true;
                            gvwUsers.Columns[21].Visible = true;
                            gvwUsers.Columns[22].Visible = true;
                            gvwUsers.Columns[23].Visible = true;
                            lblTitle.Text = "Update User";
                            break;
                        case "3":
                            ibtnCreateUser.Visible = false;
                            ibtnExportData.Visible = true;
                            gvwUsers.Columns[14].Visible = true;
                            gvwUsers.Columns[15].Visible = false;
                            gvwUsers.Columns[16].Visible = false;
                            gvwUsers.Columns[17].Visible = false;
                            gvwUsers.Columns[18].Visible = true;
                            gvwUsers.Columns[19].Visible = true;
                            gvwUsers.Columns[20].Visible = true;
                            gvwUsers.Columns[21].Visible = true;
                            gvwUsers.Columns[22].Visible = true;
                            gvwUsers.Columns[23].Visible = true;
                            lblTitle.Text = "View User";
                           
                            break;
                        case "4":
                            ibtnCreateUser.Visible = false;
                            ibtnExportData.Visible = false;
                            gvwUsers.Columns[14].Visible = false;
                            gvwUsers.Columns[15].Visible = false;
                            gvwUsers.Columns[16].Visible = true;
                             gvwUsers.Columns[17].Visible = false;
                            gvwUsers.Columns[18].Visible = false;
                            gvwUsers.Columns[19].Visible = false;
                            gvwUsers.Columns[20].Visible = false;
                            gvwUsers.Columns[21].Visible = false;
                            gvwUsers.Columns[22].Visible = false;
                            gvwUsers.Columns[23].Visible = false;
                            lblTitle.Text = "User Module Permission";
                            break;
                        case "5":
                            ibtnCreateUser.Visible = false;
                            ibtnExportData.Visible = false;
                            gvwUsers.Columns[14].Visible = false;
                            gvwUsers.Columns[15].Visible = false;
                            gvwUsers.Columns[16].Visible = false ;
                             gvwUsers.Columns[17].Visible = true;
                            gvwUsers.Columns[18].Visible = false;
                            gvwUsers.Columns[19].Visible = false;
                            gvwUsers.Columns[20].Visible = false;
                            gvwUsers.Columns[21].Visible = false;
                            gvwUsers.Columns[22].Visible = false;
                            gvwUsers.Columns[23].Visible = false;                            
                            lblTitle.Text = "User Access Rights";
                            break;

                    }
                }
               

                UserSession.GridData = null;
                UserSession.IsCreateUser = 0;
                LoadGridData();
                ExportUserDetails();

                Log.AuditLog(HttpContext.Current, "Visit", "User View");
            }
        }

        protected void ibtnCreateUser_Click(object sender, ImageClickEventArgs e)
        {
                UserSession.IsCreateUser = 0;
                Response.Redirect("../User/UserCreation.aspx", false);
                                          
        }

        protected void gvwUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToLower().Trim() == "edituser")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    UserSession.IsCreateUser = Convert.ToInt32(gvwUsers.DataKeys[intRowIndex].Value);
                    Response.Redirect("~/User/UserCreation.aspx", false);
                }

                if (e.CommandName.ToLower().Trim() == "editpermission")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    int intUserID = Convert.ToInt32(gvwUsers.DataKeys[intRowIndex].Values[0]);
                    int intRoleID = Convert.ToInt32(gvwUsers.DataKeys[intRowIndex].Values[1]);
                    Response.Redirect("../User/UserPermission.aspx?UserID=" + intUserID + "&RoleID=" + intRoleID, false);
                }
                if (e.CommandName.ToLower().Trim() == "editaccessrights")
                {
                    int intRowIndex = Convert.ToInt32(e.CommandArgument);
                    int intUserID = Convert.ToInt32(gvwUsers.DataKeys[intRowIndex].Values[0]);
                    int intRoleID = Convert.ToInt32(gvwUsers.DataKeys[intRowIndex].Values[1]);
                    Response.Redirect("../User/UserAccessRights.aspx?UserID=" + intUserID + "&RoleID=" + intRoleID, false);
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
               
            }
                       
        }

        protected void gvwUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Utility.SetGridHoverStyle(e);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkDownload = (CheckBox)e.Row.FindControl("cbDownload");
                CheckBox chkView = (CheckBox)e.Row.FindControl("cbView");
                CheckBox chkEdit = (CheckBox)e.Row.FindControl("cbEdit");
                CheckBox chkMerge = (CheckBox)e.Row.FindControl("cbMerge");
                CheckBox chkSplit = (CheckBox)e.Row.FindControl("cbSplit");
                CheckBox chkDelete = (CheckBox)e.Row.FindControl("cbDelete");

                int id1 = Convert.ToInt32(gvwUsers.DataKeys[e.Row.RowIndex].Values[0]);
                string group1 = gvwUsers.DataKeys[e.Row.RowIndex].Values[3].ToString();
                if (group1 == "1")
                {
                    chkDownload.Checked = true;
                }

                int id3 = Convert.ToInt32(gvwUsers.DataKeys[e.Row.RowIndex].Values[0]);
                string group3 = gvwUsers.DataKeys[e.Row.RowIndex].Values[4].ToString();
                if (group3 == "1")
                {
                    chkView.Checked = true;
                }

                int id4 = Convert.ToInt32(gvwUsers.DataKeys[e.Row.RowIndex].Values[0]);
                string group4 = gvwUsers.DataKeys[e.Row.RowIndex].Values[5].ToString();
                if (group4 == "1")
                {
                    chkEdit.Checked = true;
                }

                int id5 = Convert.ToInt32(gvwUsers.DataKeys[e.Row.RowIndex].Values[0]);
                string group5 = gvwUsers.DataKeys[e.Row.RowIndex].Values[6].ToString();
                if (group5 == "1")
                {
                    chkMerge.Checked = true;
                }

                int id6 = Convert.ToInt32(gvwUsers.DataKeys[e.Row.RowIndex].Values[0]);
                string group6 = gvwUsers.DataKeys[e.Row.RowIndex].Values[7].ToString();
                if (group6 == "1")
                {
                    chkSplit.Checked = true;
                }

                int id7 = Convert.ToInt32(gvwUsers.DataKeys[e.Row.RowIndex].Values[0]);
                string group7 = gvwUsers.DataKeys[e.Row.RowIndex].Values[8].ToString();
                if (group7 == "1")
                {
                    chkDelete.Checked = true;
                }

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].ColumnSpan = 8;
                for (int i = 1; i < 14; i++)
                {
                    e.Row.Cells[i].Visible = false;
                }
            }

        }

        protected void gvwUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {                
            try
            {
                gvwUsers.PageIndex = e.NewPageIndex;
                LoadGridData();
                lblNoofRecords.Text = "No. of Users :" + Convert.ToString(UserSession.GridData.Rows.Count);

                // Commented on 04-08-2021

                //if(UserSession.GridData!=null)
                //{
                //    gvwUsers.PageIndex= e.NewPageIndex;
                //    gvwUsers.DataSource = UserSession.GridData;
                //    gvwUsers.DataBind();
                //    lblNoofRecords.Text = "No. of Users :" + Convert.ToString(UserSession.GridData.Rows.Count); 
                //}

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
           
        }

        protected void gvwUsers_Sorting(object sender, GridViewSortEventArgs e)
        {
             try
            {
                if(UserSession.GridData!=null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    gvwUsers.DataSource = UserSession.SortedGridData(e.SortExpression,ViewState[UserSession.SortExpression].ToString());
                    gvwUsers.DataBind();
           
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                         
            }
         
        }

        #endregion

        # region Methods

        private void LoadGridData()
        {
            UserManager objUserManager = new UserManager();
            DataTable objDataTable = new DataTable();

            objUtility.Result = objUserManager.SelectUser(out objDataTable);
            
            switch (objUtility.Result)
            {
                case Utility.ResultType.Success:
                    gvwUsers.DataSource = objDataTable;
                    gvwUsers.DataBind();
                    UserSession.GridData = objDataTable;
                    lblNoofRecords.Text = "No. of Users :" + Convert.ToString(objDataTable.Rows.Count); 
                    break;
                case Utility.ResultType.Failure:
                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                    lblNoofRecords.Text = "No. of Users : 0"; 
                    break;
                case Utility.ResultType.Error:
                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                    lblNoofRecords.Text = "No. of Users : 0"; 
                    break;
            }
        }

        public void ExportUserDetails()
        {
            DataTable dt = new DataTable();
            dt = ExportUserList();
            Session["ExportUserList"] = dt;
        }

        public DataTable ExportUserList()
        {
            DataTable dt = new DataTable();
            try
            {
                string strCon = ConfigurationManager.AppSettings["ConnectionString"];

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    using (SqlCommand cmd = new SqlCommand("Sp_S_ExportUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }
                
        protected void ibtnFilterGrid_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                Session["Filter"] = true;
                if (UserSession.GridData != null)
                {
                    string strFilterText = ((TextBox)gvwUsers.FooterRow.FindControl("txtFilterGrid")).Text.Trim();
                    if (strFilterText == string.Empty)
                    {
                        gvwUsers.DataSource = UserSession.GridData;
                        gvwUsers.DataBind();
                        UserSession.FilterData = null;
                    }
                    else
                    {
                        DataRow[] objRows = null;
                        objRows = UserSession.GridData.Select("USERNAME LIKE '%" + strFilterText + "%'");
                        if (objRows.Length > 0)
                        {
                            UserSession.FilterData = objRows.CopyToDataTable();
                            gvwUsers.DataSource = UserSession.FilterData;
                            gvwUsers.DataBind();
                        }
                        else
                        {
                            ((Label)gvwUsers.FooterRow.FindControl("lblFilterErrorMsg")).Visible = true;
                            // lblFilterErrorMsg.Visible = false;

                            
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        #endregion

        protected void cbDownload_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbDownload = sender as CheckBox;
            if (cbDownload.Checked == true)
            {
                GridViewRow gvrow = cbDownload.NamingContainer as GridViewRow;
                int id = Convert.ToInt32(gvwUsers.DataKeys[gvrow.RowIndex].Value.ToString());
                string UserName = gvwUsers.DataKeys[gvrow.RowIndex].Values["UserName"].ToString();
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.DownloadUserTrue(out objdatatable, Convert.ToString(id));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        //GridBinding();
                        Log.AuditLog(HttpContext.Current, "Download Permission Changed to user : " + UserName, "UserView");
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Something Went Wrong .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
            if (cbDownload.Checked == false)
            {
                GridViewRow gvrow = cbDownload.NamingContainer as GridViewRow;
                int id = Convert.ToInt32(gvwUsers.DataKeys[gvrow.RowIndex].Value.ToString());
                string UserName = gvwUsers.DataKeys[gvrow.RowIndex].Values["UserName"].ToString();
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.DownloadUserFalse(out objdatatable, Convert.ToString(id));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        //GridBinding();
                        Log.AuditLog(HttpContext.Current, "Download Permission Denied to user : " + UserName, "UserView");
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Something Went Wrong .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
        }

        protected void cbView_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbView = sender as CheckBox;

            if (cbView.Checked == true)
            {
                GridViewRow gvrow = cbView.NamingContainer as GridViewRow;
                int id = Convert.ToInt32(gvwUsers.DataKeys[gvrow.RowIndex].Value.ToString());
                string UserName = gvwUsers.DataKeys[gvrow.RowIndex].Values["UserName"].ToString();
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.ViewTrue(out objdatatable, Convert.ToString(id));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        
                        Log.AuditLog(HttpContext.Current, "View Permission Given to User : " + UserName, "UserView");
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Something Went Wrong .", MainMasterPage.MessageType.Error);
                        break;
                }

            }
            if (cbView.Checked == false)
            {
                GridViewRow gvrow = cbView.NamingContainer as GridViewRow;
                int id = Convert.ToInt32(gvwUsers.DataKeys[gvrow.RowIndex].Value.ToString());
                string UserName = gvwUsers.DataKeys[gvrow.RowIndex].Values["UserName"].ToString();
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.ViewFalse(out objdatatable, Convert.ToString(id));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:

                        Log.AuditLog(HttpContext.Current, "View Permission Denied to User : " + UserName, "UserView");
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Something Went Wrong .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
        }

        protected void cbEdit_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbEdit = sender as CheckBox;

            if (cbEdit.Checked == true)
            {
                GridViewRow gvrow = cbEdit.NamingContainer as GridViewRow;
                int id = Convert.ToInt32(gvwUsers.DataKeys[gvrow.RowIndex].Value.ToString());
                string UserName = gvwUsers.DataKeys[gvrow.RowIndex].Values["UserName"].ToString();
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.EditTrue(out objdatatable, Convert.ToString(id));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:

                        Log.AuditLog(HttpContext.Current, "Edit Permission Given to User : " + UserName, "UserView");
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Something Went Wrong .", MainMasterPage.MessageType.Error);
                        break;
                }

            }
            if (cbEdit.Checked == false)
            {
                GridViewRow gvrow = cbEdit.NamingContainer as GridViewRow;
                int id = Convert.ToInt32(gvwUsers.DataKeys[gvrow.RowIndex].Value.ToString());
                string UserName = gvwUsers.DataKeys[gvrow.RowIndex].Values["UserName"].ToString();
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.EditFalse(out objdatatable, Convert.ToString(id));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:

                        Log.AuditLog(HttpContext.Current, "Edit Permission Denied to User : " + UserName, "UserView");
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Something Went Wrong .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
        }

        protected void cbMerge_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbMerge = sender as CheckBox;

            if (cbMerge.Checked == true)
            {
                GridViewRow gvrow = cbMerge.NamingContainer as GridViewRow;
                int id = Convert.ToInt32(gvwUsers.DataKeys[gvrow.RowIndex].Value.ToString());
                string UserName = gvwUsers.DataKeys[gvrow.RowIndex].Values["UserName"].ToString();
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.MergeTrue(out objdatatable, Convert.ToString(id));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:

                        Log.AuditLog(HttpContext.Current, "Merge Permission Given to User : " + UserName, "UserView");
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Something Went Wrong .", MainMasterPage.MessageType.Error);
                        break;
                }

            }
            if (cbMerge.Checked == false)
            {
                GridViewRow gvrow = cbMerge.NamingContainer as GridViewRow;
                int id = Convert.ToInt32(gvwUsers.DataKeys[gvrow.RowIndex].Value.ToString());
                string UserName = gvwUsers.DataKeys[gvrow.RowIndex].Values["UserName"].ToString();
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.MergeFalse(out objdatatable, Convert.ToString(id));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:

                        Log.AuditLog(HttpContext.Current, "Merge Permission Denied to User : " + UserName, "UserView");
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Something Went Wrong .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
        }

        protected void cbSplit_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbSplit = sender as CheckBox;

            if (cbSplit.Checked == true)
            {
                GridViewRow gvrow = cbSplit.NamingContainer as GridViewRow;
                int id = Convert.ToInt32(gvwUsers.DataKeys[gvrow.RowIndex].Value.ToString());
                string UserName = gvwUsers.DataKeys[gvrow.RowIndex].Values["UserName"].ToString();
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.SplitTrue(out objdatatable, Convert.ToString(id));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:

                        Log.AuditLog(HttpContext.Current, "Split Permission Given to User : " + UserName, "UserView");
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Something Went Wrong .", MainMasterPage.MessageType.Error);
                        break;
                }

            }
            if (cbSplit.Checked == false)
            {
                GridViewRow gvrow = cbSplit.NamingContainer as GridViewRow;
                int id = Convert.ToInt32(gvwUsers.DataKeys[gvrow.RowIndex].Value.ToString());
                string UserName = gvwUsers.DataKeys[gvrow.RowIndex].Values["UserName"].ToString();
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.SplitFalse(out objdatatable, Convert.ToString(id));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:

                        Log.AuditLog(HttpContext.Current, "Split Permission Denied to User : " + UserName, "UserView");
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Something Went Wrong .", MainMasterPage.MessageType.Error);
                        break;
                }
            }
        }

        protected void cbDelete_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbDelete = sender as CheckBox;

            if (cbDelete.Checked == true)
            {
                GridViewRow gvrow = cbDelete.NamingContainer as GridViewRow;
                int id = Convert.ToInt32(gvwUsers.DataKeys[gvrow.RowIndex].Value.ToString());
                string UserName = gvwUsers.DataKeys[gvrow.RowIndex].Values["UserName"].ToString();
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.DeleteTrue(out objdatatable, Convert.ToString(id));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:

                        Log.AuditLog(HttpContext.Current, "Delete Permission Given to User : " + UserName, "UserView");
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Something Went Wrong .", MainMasterPage.MessageType.Error);
                        break;
                }

            }
            if (cbDelete.Checked == false)
            {
                GridViewRow gvrow = cbDelete.NamingContainer as GridViewRow;
                int id = Convert.ToInt32(gvwUsers.DataKeys[gvrow.RowIndex].Value.ToString());
                string UserName = gvwUsers.DataKeys[gvrow.RowIndex].Values["UserName"].ToString();
                DataTable objdatatable = new DataTable();
                objUtility.Result = objDocumentManager.DeleteFalse(out objdatatable, Convert.ToString(id));
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:

                        Log.AuditLog(HttpContext.Current, "Delete Permission Denied to User : " + UserName, "UserView");
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Something Went Wrong .", MainMasterPage.MessageType.Error);
                        break;
                }
            }

        }

        protected void ibtnExportData_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["ExportUserList"];
                ExportToExcel(dt, string.Format("UserReport{0:dd MMMM yyyy HH mm tt}", DateTime.Now));
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }
        
        private void ExportToExcel(DataTable objTable, string fileName)
        {
            string attachment = "attachment; filename=" + fileName + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
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

                Log.AuditLog(HttpContext.Current, "Download User Report", "UserView");            
            
            Response.End();
        }
      
    }
}