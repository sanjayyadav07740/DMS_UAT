using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DMS.BusinessLogic
{
    public class UserManager
    {
        # region Private Members
        Utility objUtility = new Utility();
        #endregion
    
        #region Methods

        public Utility.ResultType SelectUser(out DataTable objDataTable)
        {
            return User.Select(out objDataTable);
        }

        #region Seema 4 july 2017

        public Utility.ResultType SelectUserIndepey(out DataTable objDataTable)
        {
            return User.SelectIndepey(out objDataTable);
        }

        #endregion

        public Utility.ResultType SelectUser(out DataTable objDataTable, int intUserID)
        {
            return User.SelectAllByUserID(out objDataTable, intUserID);
        }

        public Utility.ResultType SelectUser(out DataTable objDataTable,string strUserName)
        {
            return User.SelectByUserName(out objDataTable,strUserName);
        }

       
        public Utility.ResultType SelectUserDetails(out DataTable objDataTable, string strUserName)
        {
            return User.SelectByEmail(out objDataTable, strUserName);
        }

        public Utility.ResultType SelectUserDetailsByID(out DataTable objDataTable, int UserID)
        {
            return User.SelectByUserID(out objDataTable, UserID);
        }

        public DataSet SelectByUserNameLike(string strUserName)
        {
            return User.SelectByUserNameLike(strUserName);
        }

        public DataSet SearchByRoleNameLike(string strRoleName)
        {
            return User.SearchByRoleNameLike(strRoleName);
        }

        public DataSet SearchByUserNameLike(string strUserName)
        {
            return User.SearchByUserNameLike(strUserName);
        }

        public DataSet SearchByRoleUserNameLike(string strUserName, string strRoleName)
        {
            return User.SearchByRoleUserNameLike(strUserName,strRoleName);
        }

        public Utility.ResultType SelectUserByEmailID(out DataTable objDataTable, string strUserName)
        {
            return User.SelectUserByEmailID(out objDataTable, strUserName);
        }

        public Utility.ResultType InsertUser(User objUser)
        {
          return User.Insert(objUser);
        }

        public Utility.ResultType UpdateUser(User objUser)
        {
            return User.Update(objUser);
        }

        public Utility.ResultType ChangePassword(User objUser)
        {
            return User.ChangePassword(objUser);
        }

        public Utility.ResultType UserNameExists(string strUserName)
        {
            return User.Exists(strUserName);
        }

        public Utility.ResultType UserExists(out DataTable objDataTable, string strUserName, string strPassword)
        {
            return User.UserExists(out objDataTable,strUserName, strPassword);
        }
        /// <summary>
        /// Check For Total Users
        /// </summary>
        /// <param name="objDataTable"></param>
        /// <returns></returns>
        public Utility.ResultType TotalUsers(out DataTable objDataTable)
        {
            return User.TotalUsers(out objDataTable);
        }

        public Utility.ResultType AuthenticateUser(string strUserName, string strPassword)
        {
            return User.Authenticate(strUserName, strPassword);
           
        }

        public Utility.ResultType AuthenticateUser_ShareDocument(string strUserName, string strPassword)
        {
            return User.Authenticate_ShareDocument(strUserName, strPassword);

        }

        public Utility.ResultType SelectUserModule(out DataTable objDataTable, int intUserID)
        {
            return UserModule.SelectAllByUserID(out objDataTable, intUserID);
        }

        public Utility.ResultType DeleteUserModule(int intID, DbTransaction objDbTransaction, UserModule.DeleteIDType enumDeleteIDType)
        {
            return UserModule.Delete(intID, objDbTransaction, enumDeleteIDType);
        }

        public Utility.ResultType InsertUserModule(UserModule objUserModule, DbTransaction objDbTransaction)
        {
            return UserModule.Insert(objUserModule, objDbTransaction);
        }

        public List<int>  SelectUserModule(int intUserID)
        {
            return UserModule.SelectByUserID(intUserID);
        }

        public Utility.ResultType SelectUserPermission(out DataTable objDataTable, int intUserID)
        {
            return UserPermission.Select(out objDataTable, intUserID);

        }

        public Utility.ResultType DeleteUserPermission(int intID, DbTransaction objDbTransaction,UserPermission.DeleteIDType enumDeleteIDType)
        {
            return UserPermission.Delete(intID, objDbTransaction,enumDeleteIDType);
        }

        public Utility.ResultType InsertUserPermission(UserPermission objUserPermission, DbTransaction objDbTransaction)
        {
            return UserPermission.Insert(objUserPermission, objDbTransaction);
        }

        public void InsertLoginDetail(LoginDetail objLoginDetail)
        {
             LoginDetail.Insert(objLoginDetail);
        }

        public void UpdateLoginDetail(LoginDetail objLoginDetail)
        {
            LoginDetail.Update(objLoginDetail);
        }

        public string SelectLastLogin(LoginDetail objLoginDetail)
        {
           return LoginDetail.SelectLastLogin(objLoginDetail);
        }

        public Utility.ResultType AuditLogDetails(string IpAdd, string MacAdd, string Activity, string DocName, int UserId, DbTransaction objDBTransaction1)
        {
            return User.AuditLogDetails(IpAdd, MacAdd, Activity, DocName, UserId, objDBTransaction1);
        }

        public Utility.ResultType OTPDetails(string Useremails, string OtpNum, DbTransaction objDBTransaction1)
        {
            return User.OTPDetails(Useremails, OtpNum, objDBTransaction1);
        }

        public Utility.ResultType GetOtpDetails(string Useremails, string otpnumber,out DataTable table)
        {
            return User.GetOtpDetails(Useremails, otpnumber, out table);
        }
        # endregion
    }
}
