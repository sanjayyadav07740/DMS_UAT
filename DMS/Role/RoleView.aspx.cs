using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DMS.BusinessLogic;

namespace DMS.Role
{
    public partial class RoleView : System.Web.UI.Page
    {
        # region Private Members
        RoleManager objRoleManager = new RoleManager();
        Utility objUtility = new Utility();
      
         
        # endregion

        # region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Filter"] = false;
            Session["isRollAccess"] = false;
           
            if (!IsPostBack)
            {
                LoadRepository();
                if (Request["Type"] != null)
                {
                    if (Request["Type"].ToString().Trim() == "0")
                    {
                        Log.AuditLog(HttpContext.Current, "Role Created", "RoleView");
                        UserSession.DisplayMessage(this, "Role Is Created Successfully .", MainMasterPage.MessageType.Success);
                    }
                    else if (Request["Type"].ToString().Trim() == "1")
                    {
                        Log.AuditLog(HttpContext.Current, "Role Updated", "RoleView(Updated)");
                        UserSession.DisplayMessage(this, "Role Is Updated Successfully .", MainMasterPage.MessageType.Success);
                    }
                    else if (Request["Type"].ToString().Trim() == "2")
                    {
                        UserSession.DisplayMessage(this, "Permissions has been Assigned Successfully.", MainMasterPage.MessageType.Success);
                    }
                }

                if (Request["ID"] != null)
                {
                    switch (Request["ID"].ToString().Trim())
                    {
                        case "1":
                            ibtnCreateRole.Visible = true;
                            gvwRoles.Columns[10].Visible = false;
                            gvwRoles.Columns[11].Visible = false;
                            gvwRoles.Columns[12].Visible = false;
                            //isRollAccess = false;
                            lblSelRepository.Visible = true;
                            DDLSelRepository.Visible = true;
                            Session["isRollAccess"] = false;
                            lblTitle.Text = "Create Role";
                            break;
                        case "2":
                            ibtnCreateRole.Visible = false;
                            gvwRoles.Columns[10].Visible = true;
                            gvwRoles.Columns[11].Visible = false;
                            gvwRoles.Columns[12].Visible = false;
                            Session["isRollAccess"] = false;
                            lblSelRepository.Visible = true;
                            DDLSelRepository.Visible = true;
                            lblTitle.Text = "Update Role";
                            break;
                        case "3":
                            ibtnCreateRole.Visible = false;
                            gvwRoles.Columns[10].Visible = false;
                            gvwRoles.Columns[11].Visible = false;
                            gvwRoles.Columns[12].Visible = false;
                            lblSelRepository.Visible = true;
                            DDLSelRepository.Visible = true;
                            Session["isRollAccess"] = false;
                            lblTitle.Text = "View Role";
                            break;
                        case "4":
                            ibtnCreateRole.Visible = false;
                            gvwRoles.Columns[10].Visible = false;
                            gvwRoles.Columns[11].Visible = true;
                            gvwRoles.Columns[12].Visible = false;
                            Session["isRollAccess"] = true;
                            lblSelRepository.Visible = true;
                            DDLSelRepository.Visible = true;
                          
                          
                            lblTitle.Text = "Role Module Permission";
                            break;
                        case "5":
                            ibtnCreateRole.Visible = false;
                            gvwRoles.Columns[10].Visible = false;
                            gvwRoles.Columns[11].Visible = false;
                            gvwRoles.Columns[12].Visible = true;
                            lblSelRepository.Visible = true;
                            DDLSelRepository.Visible = true;
                            //  Session["isRollAccess"] = true;
                            lblTitle.Text = "Role Access Rights";
                            break;

                    }
                }


                UserSession.GridData = null;
                LoadGridData();
                Log.AuditLog(HttpContext.Current, "Visit", "Role View");
            }

        }

        protected void gvwRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    gvwRoles.PageIndex = e.NewPageIndex;
                    gvwRoles.DataSource = UserSession.GridData;
                    gvwRoles.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }


        }

        protected void gvwRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (Convert.ToInt32(UserSession.RoleID) == 1)
                {
                    if (Convert.ToInt32(DDLSelRepository.SelectedIndex) != 0)
                    {
                        if (e.CommandName.ToLower().Trim() == "editrole")
                        {
                            int intRowIndex = Convert.ToInt32(e.CommandArgument);
                            UserSession.IsCreateRole = Convert.ToInt32(gvwRoles.DataKeys[intRowIndex].Value);
                            Response.Redirect("../Role/RoleCreation.aspx", false);

                        }
                        if (e.CommandName.ToLower().Trim() == "editpermission")
                        {
                            int intRowIndex = Convert.ToInt32(e.CommandArgument);
                            int intRoleID = Convert.ToInt32(gvwRoles.DataKeys[intRowIndex].Value);
                            string RoleName = gvwRoles.DataKeys[intRowIndex].Values[1].ToString();
                            Session["TempRoleName"] = RoleName;
                            Response.Redirect("../Role/RoleModulePermission.aspx?RoleID=" + intRoleID, false);

                        }
                        if (e.CommandName.ToLower().Trim() == "editaccessrights")
                        {
                            int intRowIndex = Convert.ToInt32(e.CommandArgument);
                            int intRoleID = Convert.ToInt32(gvwRoles.DataKeys[intRowIndex].Value);
                            string RoleName = gvwRoles.DataKeys[intRowIndex].Values[1].ToString();
                            Session["TempRoleName"] = RoleName;
                            Response.Redirect("../Role/RoleAccessRights.aspx?RoleID=" + intRoleID + "&RepositoryId=" + DDLSelRepository.SelectedItem.Value, false);

                        }
                        else
                        {
                            if ((bool)Session["Filter"] == true)
                            {
                                lblErrorMsg.Visible = false;
                            }
                            else
                            {
                                lblErrorMsg.Visible = true;
                                ((Label)gvwRoles.FooterRow.FindControl("lblFilterErrorMsg")).Visible = false;
                            }
                        }
                    }
                }
                else
                {
                    if (Convert.ToInt32(DDLSelRepository.SelectedIndex) != 0)
                    {
                        if (e.CommandName.ToLower().Trim() == "editrole")
                        {
                            int intRowIndex = Convert.ToInt32(e.CommandArgument);
                            UserSession.IsCreateRole = Convert.ToInt32(gvwRoles.DataKeys[intRowIndex].Value);
                            Response.Redirect("../Role/RoleCreation.aspx", false);

                        }
                        if (e.CommandName.ToLower().Trim() == "editpermission")
                        {
                            int intRowIndex = Convert.ToInt32(e.CommandArgument);
                            int intRoleID = Convert.ToInt32(gvwRoles.DataKeys[intRowIndex].Value);
                            string RoleName = gvwRoles.DataKeys[intRowIndex].Values[1].ToString();
                            Session["TempRoleName"] = RoleName;
                            Response.Redirect("../Role/RoleModulePermission.aspx?RoleID=" + intRoleID, false);

                        }
                        if (e.CommandName.ToLower().Trim() == "editaccessrights")
                        {
                            int intRowIndex = Convert.ToInt32(e.CommandArgument);
                            int intRoleID = Convert.ToInt32(gvwRoles.DataKeys[intRowIndex].Value);
                            string RoleName = gvwRoles.DataKeys[intRowIndex].Values[1].ToString();
                            Session["TempRoleName"] = RoleName;
                            Response.Redirect("../Role/RoleAccessRights.aspx?RoleID=" + intRoleID + "&RepositoryId=" + DDLSelRepository.SelectedItem.Value, false);

                        }
                    }
                    else
                    {
                        if ((bool)Session["Filter"] == true)
                        {
                            lblErrorMsg.Visible = false;
                        }
                        else
                        {
                            lblErrorMsg.Visible = true;
                            ((Label)gvwRoles.FooterRow.FindControl("lblFilterErrorMsg")).Visible = false;
                        }
                    }
                }
                Session["Filter"] = false;

            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);

            }
        }
        //** Commented by vivek
        //protected void gvwRoles_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //     Utility.SetGridHoverStyle(e);

        //}

        protected void gvwRoles_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (UserSession.GridData != null)
                {
                    if (ViewState[UserSession.SortExpression] == null || ViewState[UserSession.SortExpression].ToString() == "DESC")
                        ViewState[UserSession.SortExpression] = "ASC";
                    else
                        ViewState[UserSession.SortExpression] = "DESC";

                    gvwRoles.DataSource = UserSession.SortedGridData(e.SortExpression, ViewState[UserSession.SortExpression].ToString());
                    gvwRoles.DataBind();
                }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ibtnCreateRole_Click(object sender, ImageClickEventArgs e)
        {
            UserSession.IsCreateRole = 0;
            Response.Redirect("../Role/RoleCreation.aspx", false);

        }

        # endregion

        # region Methods
        //Uncommented and Implimented by Vivek

        private void LoadRepository()
        {
            try
            {
                DataTable dt = new DataTable();
                objUtility.Result = DMS.BusinessLogic.RepositoryManager.SelectRepositoryForDropDown(out dt);
                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        if (dt.Rows.Count > 0)
                        {
                            DDLSelRepository.DataSource = dt;
                            DDLSelRepository.DataTextField = "RepositoryName";
                            DDLSelRepository.DataValueField = "ID";
                            DDLSelRepository.DataBind();
                        }
                        DDLSelRepository.Items.Insert(0, "--Select--");
                        //if(dt.Rows.Count == 1)
                        //{
                        //    DDLSelRepository.Items[1].Selected = true; ;
                        //}
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
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        private void LoadGridDataSearch(string RoleName)
        {
            try
            {
                DMS.BusinessLogic.RoleManager objRoleManager = new RoleManager();
                DataSet dsRoleDetails = new DataSet();
                dsRoleDetails = objRoleManager.SelectRoleLike(RoleName);
                if (dsRoleDetails.Tables[0].Rows.Count > 0)
                {
                    gvwRoles.DataSource = dsRoleDetails.Tables[0];
                    gvwRoles.DataBind();
                    UserSession.GridData = dsRoleDetails.Tables[0];
                }
                else
                    UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void LoadGridData()
        {
            try
            {
                int RoleID = UserSession.RoleID;
                DataTable objDataTable = new DataTable();
                if (UserSession.RoleID != 1)
                    objUtility.Result = objRoleManager.SelectRole(out objDataTable, RoleID);
                else
                    objUtility.Result = objRoleManager.SelectRole(out objDataTable);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        gvwRoles.DataSource = objDataTable;
                        gvwRoles.DataBind();
                        UserSession.GridData = objDataTable;
                        //lblNoofRecords.Text = "No. of Roles : " + Convert.ToString(objDataTable.Rows.Count);
                        break;
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "No Data To Display .", MainMasterPage.MessageType.Warning);
                        //lblNoofRecords.Text = "No. of Roles : 0";
                        break;
                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        //lblNoofRecords.Text = "No. of Roles : 0";                        
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        protected void DDLSelRepository_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLSelRepository.SelectedItem.Text == "--Select--")
            {
                UserSession.GridData = null;
                LoadGridData();
            }
            else
            {
                DataSet ds = new DataSet();
                DMS.BusinessLogic.RoleManager objRoleManager = new RoleManager();
                ds = objRoleManager.SearchRoleByRep(Convert.ToInt32(DDLSelRepository.SelectedItem.Value));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvwRoles.DataSource = ds.Tables[0];
                    UserSession.GridData = ds.Tables[0];
                    gvwRoles.DataBind();
                    //lblNoofRecords.Text = "No. of Roles :" + Convert.ToString(ds.Tables[0].Rows.Count);
                }
                else
                {
                    //lblNoofRecords.Text = "No. of Roles : 0";
                    UserSession.DisplayMessage(this, "No Role existing for selected Repository", MainMasterPage.MessageType.Warning);
                    return;
                }
            }
           
            lblErrorMsg.Visible = false;
            ((Label)gvwRoles.FooterRow.FindControl("lblFilterErrorMsg")).Visible = false;
        }
        # endregion

        protected void gvwRoles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Utility.SetGridHoverStyle(e);
            try
            {
                Utility.SetGridHoverStyle(e);
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].ColumnSpan = 13;
                    for (int i = 1; i < 12; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }

        }
        //**Added by Vivek for Searching users 30-11-2017
        protected void ibtnFilterGrid_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
               
                Session["Filter"] = true;
                if (UserSession.GridData != null)
                {
                    string strFilterText = ((TextBox)gvwRoles.FooterRow.FindControl("txtFilterGrid")).Text.Trim();
                    if (strFilterText == string.Empty)
                    {
                        gvwRoles.DataSource = UserSession.GridData;
                        gvwRoles.DataBind();
                        UserSession.FilterData = null;
                       
                    }
                    else
                    {
                        DataRow[] objRows = null;
                        objRows = UserSession.GridData.Select("ROLENAME LIKE '%" + strFilterText + "%'");
                        if (objRows.Length > 0)
                        {
                            UserSession.FilterData = objRows.CopyToDataTable();
                            gvwRoles.DataSource = UserSession.FilterData;
                            gvwRoles.DataBind();
                        }
                        else
                        {
                            ((Label)gvwRoles.FooterRow.FindControl("lblFilterErrorMsg")).Visible = true;
                           // lblFilterErrorMsg.Visible = false;

                           lblErrorMsg.Visible = false;
                        }
                    }
                }
                lblErrorMsg.Visible = false;
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
            }
        }

        //protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        //{
        //    if(txtRoleName.Text=="")
        //    {
        //        UserSession.DisplayMessage(this, "Please enter full or a part of username", MainMasterPage.MessageType.Error);
        //    }
        //    else
        //    {
        //        LoadGridDataSearch(txtRoleName.Text);
        //    }
        //}



    }
}