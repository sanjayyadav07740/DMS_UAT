using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.DirectoryServices;
namespace DMS.User
{
    public partial class UserCreation : System.Web.UI.Page
    {

        #region Private Member
        Utility objUtility = new Utility();
        UserManager objUserManager = new UserManager();
        #endregion

        #region Method

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Page.Form.DefaultButton = ibtnSubmit.UniqueID;
                txtUserName.Focus();
                LoadDropdown();

                if (UserSession.IsCreateUser == 0)
                {
                    EnableControls();
                    lblTitle.Text = "Create User";
                }
                else if (UserSession.IsCreateUser != 0)
                {
                    DisableControls();
                    LoadControl(UserSession.IsCreateUser);
                    lblTitle.Text = "Update User";
                }
                Log.AuditLog(HttpContext.Current, "Visit", "User Creation");
            }
        }

        private void EnableControls()
        {
            txtPassword.BackColor = Color.White;
            txtPassword.Enabled = true;
            ddlRole.Enabled = true;
            rfvPassword.Enabled = true;
            ddlStatus.BackColor = Color.White;
            ddlStatus.Enabled = true;
        }

        private void DisableControls()
        {
            txtPassword.BackColor = Color.LightSlateGray;
            txtPassword.Enabled = false;
            ddlRole.Enabled = true;
            rfvPassword.Enabled = false;
            ddlStatus.BackColor = Color.LightSlateGray;
            ddlStatus.Enabled = false;
        }

        private void LoadDropdown()
        {
            Utility.LoadUserType(ddlUserType);

            #region Seema 17 July 2017

            if (Convert.ToInt32(HttpContext.Current.Session["RoleID"]) == 209)
            {
                Utility.LoadRoleIndepey(ddlRole);
            }
            else
            {
                Utility.LoadRole(ddlRole);
            }

            #endregion

            Utility.LoadUserIS(ddlUserIS);
            Utility.LoadCountry(ddlCountry);
            //SELECTING COUNTRY BY DEFAULT AND ITS STATE
            ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByText("India"));
            Utility.LoadState(ddlState, Convert.ToInt32(ddlCountry.SelectedValue));
            Utility.LoadStatus(ddlStatus);

        }

        private void LoadControl(int intUserID)
        {
            try
            {
                DataTable objDataTable = new DataTable();
                objUtility.Result = objUserManager.SelectUser(out objDataTable, intUserID);

                switch (objUtility.Result)
                {
                    case Utility.ResultType.Success:
                        txtUserName.Text = objDataTable.Rows[0]["UserName"].ToString();
                        txtPassword.Text = Utility.Decrypt(objDataTable.Rows[0]["Password"].ToString());
                        ddlUserType.SelectedIndex = ddlUserType.Items.IndexOf(ddlUserType.Items.FindByValue(objDataTable.Rows[0]["UserTypeID"].ToString()));
                        ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByValue(objDataTable.Rows[0]["RoleID"].ToString()));
                        ViewState["RoleID"] = objDataTable.Rows[0]["RoleID"].ToString();
                        ddlUserIS.SelectedIndex = ddlUserIS.Items.IndexOf(ddlUserIS.Items.FindByValue(objDataTable.Rows[0]["UserIs"].ToString()));
                        txtFirstName.Text = objDataTable.Rows[0]["FirstName"].ToString();
                        txtMiddleName.Text = objDataTable.Rows[0]["MiddleName"].ToString();
                        txtLastName.Text = objDataTable.Rows[0]["LastName"].ToString();
                        txtAddress.Text = objDataTable.Rows[0]["Address"].ToString();
                        txtCity.Text = objDataTable.Rows[0]["City"].ToString();
                        ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByValue(objDataTable.Rows[0]["CountryID"].ToString()));
                        ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(objDataTable.Rows[0]["StateID"].ToString()));
                        txtPinCode.Text = objDataTable.Rows[0]["PinCode"].ToString();
                        txtMobileNo.Text = objDataTable.Rows[0]["MobileNo"].ToString();
                        txtEmailID.Text = objDataTable.Rows[0]["EmailID"].ToString();
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(objDataTable.Rows[0]["Status"].ToString()));
                        hdfUserName.Value = txtUserName.Text.Trim();

                        break;
                    case Utility.ResultType.Failure:
                        UserSession.DisplayMessage(this, "Sorry ,No Data Is Available For This User .", MainMasterPage.MessageType.Error);
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
            int intMaxUsers = Utility.MaxUsers;
            DataTable objDataTable = new DataTable();
            objUtility.Result = objUserManager.TotalUsers(out objDataTable);
            try
            {
                //commented because there is no limit for user creation in abbott
                // if (objDataTable != null && Convert.ToInt32(objDataTable.Rows[0][0].ToString()) < intMaxUsers)
                //{
                if (UserSession.IsCreateUser == 0 || hdfUserName.Value.ToLower() != txtUserName.Text.Trim().ToLower())
                {
                    objUtility.Result = objUserManager.UserNameExists(txtUserName.Text.Trim());
                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            UserSession.DisplayMessage(this, "User Name Already Exist .", MainMasterPage.MessageType.Success);
                            return;
                            break;

                        case Utility.ResultType.Error:
                            UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                            return;
                            break;
                    }
                }

                DMS.BusinessLogic.User objUser = new DMS.BusinessLogic.User();

                if (UserSession.IsCreateUser == 0)
                {
                    objUser.UserName = txtUserName.Text.Trim();
                    objUser.Password = Utility.Encrypt(txtPassword.Text.Trim());
                    objUser.UserTypeID = Convert.ToInt32(ddlUserType.SelectedValue);
                    objUser.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
                    objUser.UserIs = Convert.ToInt32(ddlUserIS.SelectedValue);
                    objUser.FirstName = txtFirstName.Text.Trim();
                    objUser.MiddleName = txtMiddleName.Text.Trim();
                    objUser.LastName = txtLastName.Text.Trim();
                    objUser.Address = txtAddress.Text.Trim();
                    objUser.City = txtCity.Text.Trim();
                    objUser.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
                    objUser.StateID = Convert.ToInt32(ddlState.SelectedValue);
                    objUser.PinCode = txtPinCode.Text.Trim();
                    objUser.MobileNo = txtMobileNo.Text.Trim();
                    objUser.EmailID = txtEmailID.Text.Trim();
                    objUser.LoginCount = 0;
                    objUser.CreatedBy = UserSession.UserID;
                    objUser.Status = Convert.ToInt32(ddlStatus.SelectedValue);

                    objUtility.Result = objUserManager.InsertUser(objUser);
                    Log.AuditLog(HttpContext.Current, "User Inserted", "User Creation");
                }
                else if (UserSession.IsCreateUser != 0)
                {
                    objUser.UserID = UserSession.IsCreateUser;
                    objUser.UserName = txtUserName.Text.Trim();
                    objUser.UserTypeID = Convert.ToInt32(ddlUserType.SelectedValue);
                    objUser.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
                    objUser.UserIs = Convert.ToInt32(ddlUserIS.SelectedValue);
                    objUser.FirstName = txtFirstName.Text.Trim();
                    objUser.MiddleName = txtMiddleName.Text.Trim();
                    objUser.LastName = txtLastName.Text.Trim();
                    objUser.Address = txtAddress.Text.Trim();
                    objUser.City = txtCity.Text.Trim();
                    objUser.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
                    objUser.StateID = Convert.ToInt32(ddlState.SelectedValue);
                    objUser.PinCode = txtPinCode.Text.Trim();
                    objUser.MobileNo = txtMobileNo.Text.Trim();
                    objUser.EmailID = txtEmailID.Text.Trim();
                    objUser.UpdatedBy = UserSession.UserID;
                    objUser.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                    hdfUserName.Value = txtUserName.Text.Trim();

                    if (ViewState["RoleID"] != null)
                    {
                        if (ddlRole.SelectedValue.ToString() != ViewState["RoleID"].ToString())
                        {
                            System.Data.Common.DbTransaction objDbTransaction = Utility.GetTransaction;
                            objUserManager.DeleteUserPermission(objUser.UserID, objDbTransaction, BusinessLogic.UserPermission.DeleteIDType.UserID);
                            objUserManager.DeleteUserModule(objUser.UserID, objDbTransaction, UserModule.DeleteIDType.UserID);
                            objDbTransaction.Commit();
                        }
                    }

                    objUtility.Result = objUserManager.UpdateUser(objUser);
                    Log.AuditLog(HttpContext.Current, "User Updated", "User Creation(For Updation)");
                }

                switch (objUtility.Result)
                {

                    case Utility.ResultType.Success:
                        if (UserSession.IsCreateUser == 0)
                        {
                            //Response.Redirect("../User/UserView.aspx?Type=0&ID=1", false);
                            Log.AuditLog(HttpContext.Current, "User Created", "UserCreation");
                            UserSession.DisplayMessage(this, "User Is Created Successfully .", MainMasterPage.MessageType.Success);
                        }                            
                        else                        
                        Response.Redirect("../User/UserView.aspx?Type=1&ID=2", false);
                        break;

                    case Utility.ResultType.Error:
                        UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                        break;

                }

                // }
                // else
                //// {
                //     UserSession.DisplayMessage(this, "Your User Creation Limit Is Over.", MainMasterPage.MessageType.Error);
                // }
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlState.Items.Clear();
            if (ddlCountry.SelectedIndex != 0)
            {
                Utility.LoadState(ddlState, Convert.ToInt32(ddlCountry.SelectedValue));
            }
        }

        protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        {
            if (UserSession.IsCreateUser == 0)
            {
                txtUserName.Text = string.Empty;
                txtPassword.Text=string.Empty;
                txtFirstName.Text=string.Empty;
                txtMiddleName.Text=string.Empty;
                txtLastName.Text=string.Empty;
                txtMobileNo.Text=string.Empty;
                txtPinCode.Text=string.Empty;
                txtEmailID.Text = string.Empty;
                txtAddress.Text=string.Empty;
                txtCity.Text=string.Empty;
                //Response.Redirect("../User/UserView.aspx?ID=1", false);
            }  
            else
            {
                Response.Redirect("../User/UserView.aspx?ID=2", false);
            }
               
        }

        #endregion Method

        protected void ddlUserIS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtPassword.ReadOnly = false;
                rfvPassword.Enabled = true;
                revPassword.Enabled = true;
                vcePassword.Enabled = true;

                if (ddlUserIS.SelectedValue == "1")
                {
                    txtPassword.ReadOnly = true;
                    rfvPassword.Enabled = false;
                    revPassword.Enabled = false;
                    vcePassword.Enabled = false;

                    string connection = ConfigurationManager.ConnectionStrings["ADConnection"].ToString();
                    DirectorySearcher dssearch = new DirectorySearcher(connection);
                    dssearch.Filter = "(sAMAccountName=" + txtUserName.Text + ")";
                    SearchResult sresult = dssearch.FindOne();
                    DirectoryEntry dsresult = sresult.GetDirectoryEntry();
                    txtFirstName.Text = dsresult.Properties["givenName"][0].ToString();
                    txtLastName.Text = dsresult.Properties["sn"][0].ToString();
                    txtEmailID.Text = dsresult.Properties["mail"][0].ToString();
                    txtMobileNo.Text = dsresult.Properties["mobile"][0].ToString();
                    txtCity.Text = dsresult.Properties["l"][0].ToString();
                    txtPinCode.Text = dsresult.Properties["postalCode"][0].ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertmessage", "javascript:alert('" + txtUserName.Text + " information is selected.')", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertmessage", "javascript:alert('Not A AD User')", true);
            }
        }
    }
}
