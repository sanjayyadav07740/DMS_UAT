using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BusinessLogic;
using System.IO;
using System.Data;

namespace DMS.User
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        #region Private Member
        Utility objUtility = new Utility();
        UserManager objUserManager = new UserManager();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Log.AuditLog(HttpContext.Current, "Visit", "Change Password");
        }

        protected void ibtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            //check if entered username and password exist
            DataTable dt = new DataTable();
            string Pwd = Utility.Encrypt(txtOldPwd.Text);
            objUtility.Result = objUserManager.UserExists(out dt, txtUserName.Text, Pwd);
            switch (objUtility.Result)
            {

                case Utility.ResultType.Failure:
                    UserSession.DisplayMessage(this, "The entered username and password do not exist!", MainMasterPage.MessageType.Error);
                    break;
                case Utility.ResultType.Success:
                    //update password
                    DMS.BusinessLogic.User objUser = new DMS.BusinessLogic.User();
                    objUser.UserID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    objUser.Password = Utility.Encrypt(txtNewPwd.Text);
                    objUser.UpdatedBy = UserSession.UserID;
                    objUtility.Result = objUserManager.ChangePassword(objUser);
                    Log.AuditLog(HttpContext.Current, "Password Changed", "Change Password");
                    switch (objUtility.Result)
                    {
                        case Utility.ResultType.Success:
                            UserSession.DisplayMessage(this, "Password has been changed successfully!", MainMasterPage.MessageType.Success);
                            //Response.Redirect("");
                            break;
                    }
                    break;
            }
        }
    }
}