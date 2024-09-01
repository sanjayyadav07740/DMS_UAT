using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;

namespace DMS.Shared
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        #region Private Member
        Utility objUtility = new Utility();
        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.DefaultButton = ibtnSubmit.UniqueID;
            if (!IsPostBack)
            {
                if (UserSession.UserPassword != null)
                {
                    cpvExistingPassword.ValueToCompare = UserSession.UserPassword;
                }
            }
            Log.AuditLog(HttpContext.Current, "Visit", "Change Password");
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                UserManager objUserManager = new UserManager();
                BusinessLogic.User objUser = new BusinessLogic.User();
                objUser.UserID = UserSession.UserID;
                objUser.Password = Utility.Encrypt(txtNewPassword.Text.Trim());
                objUser.UpdatedBy = UserSession.UserID;

                objUtility.Result = objUserManager.ChangePassword(objUser);
                if(objUtility.Result == Utility.ResultType.Error)
                {
                    UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                    return;
                }
                Log.AuditLog(HttpContext.Current, "Password Changed", "Change Password");
                UserSession.DisplayMessage(this, "Password Has Been Changed Successfully,Please Relogin .", MainMasterPage.MessageType.Success);
                ((System.Web.UI.HtmlControls.HtmlInputButton)this.Master.FindControl("btnOk")).Attributes.Add("onclick", "document.location.href='../Shared/LoginForm.aspx';");
                HttpContext.Current.Session.Abandon();
            }
            catch (Exception ex)
            {
                UserSession.DisplayMessage(this, "Sorry ,Some Error Has Been Occured .", MainMasterPage.MessageType.Error);
                LogManager.ErrorLog(Utility.LogFilePath, ex);
            }
        }
        
        #endregion

        protected void ibtnBack_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Shared/LoginForm.aspx", false);
        }

    }
}