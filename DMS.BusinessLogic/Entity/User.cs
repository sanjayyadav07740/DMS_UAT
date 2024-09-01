using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Web;
namespace DMS.BusinessLogic
{

    public class User
    {
        #region Properties
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserTypeID { get; set; }
        public int RoleID { get; set; }
        public int UserIs { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public string PinCode { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public int LoginCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public int Status { get; set; }
        #endregion
        
        # region Methods

        public static Utility.ResultType Select(out DataTable objDataTable)
        {
            try
            {
                char charQuote = '"';
               

                string strQuery = "SELECT U.ID,U.RoleID,U.UserName,R.RoleName,U.FirstName,U.FirstName +' '+ U.LastName as FullName,U.MiddleName,U.LastName,U.Address,U.City,U.MobileNo,U.EmailID,U.Download,U.IsViewed,U.IsEdit,U.IsMerge,U.IsSplit,U.IsDelete,S.StateName,C.CountryName,"
                       + " (SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=U.Status) AS " + charQuote + "Status" + charQuote + ","
                       + " U.CreatedOn,"
                       + " (SELECT FirstName FROM vwUser WHERE ID = U.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + ","
                       + " U.UpdatedOn,"
                       + " (SELECT FirstName FROM vwUser WHERE ID = U.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + ""
                       + "  FROM [User] U Left Outer join vwCountry C  on U.CountryID=C.ID "
                       + " Left Outer Join vwState S  on U.CountryID=C.ID and U.StateId=S.ID "
                       + " inner join vwRole R on U.roleid=R.id "
                       + " " + (Convert.ToInt32(HttpContext.Current.Session["RoleType"]) == 2 ? " WHERE RoleID IN (SELECT RepositoryUserID FROM vwRoleRights WHERE RepositoryAdminID = " + Convert.ToInt32(HttpContext.Current.Session["RoleID"]) + " )" : string.Empty) + " "
                       + " ORDER BY ID DESC";


                objDataTable = DataHelper.ExecuteDataTable(strQuery, null);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;

            }
        }

        #region Seema 4 July 2017

        public static Utility.ResultType SelectIndepey(out DataTable objDataTable)
        {
            try
            {
                char charQuote = '"';

                string strQuery = "SELECT U.ID,U.RoleID,U.UserName,U.FirstName,U.MiddleName,U.LastName,U.Address,U.City,U.MobileNo,U.EmailID,S.StateName,C.CountryName,"
                       + " (SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=U.Status) AS " + charQuote + "Status" + charQuote + ","
                       + " CreatedOn,"
                       + " (SELECT FirstName FROM vwUser WHERE ID = U.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + ","
                       + " UpdatedOn,"
                       + " (SELECT FirstName FROM vwUser WHERE ID = U.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + ""
                       + "  FROM vwUser U Left Outer join vwCountry C  on U.CountryID=C.ID "
                       + " Left Outer Join vwState S  on U.CountryID=C.ID and U.StateId=S.ID "
                       + " " + (Convert.ToInt32(HttpContext.Current.Session["RoleType"]) == 2 ? " WHERE RoleID IN (SELECT RepositoryUserID FROM vwRoleRights WHERE RepositoryAdminID = " + Convert.ToInt32(HttpContext.Current.Session["RoleID"]) + " )" : string.Empty) + " "
                       + " where U.RoleID=209 or CreatedBy=891 ORDER BY ID DESC";


                objDataTable = DataHelper.ExecuteDataTable(strQuery, null);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;

            }
        }

        #endregion

        public static Utility.ResultType SelectAllByUserID(out DataTable objDataTable, int intUserID)
        {
            try
            {
             char charQuote = '"';

             string strQuery = "SELECT * "
                             + " FROM vwUser U INNER JOIN vwRole R"
                             + " ON U.RoleID=R.Id"
                             + " WHERE U.ID=@ID";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
                objDbParameter[0].Value = intUserID;

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SelectByUserName(out DataTable objDataTable, string strUserName)
        {
            try
            {
                string strQuery = @"SELECT vwUser.ID,RoleID,Password,UserTypeID,RoleType,EmailID FROM vwUser LEFT OUTER JOIN vwRole
                                    ON vwUser.RoleID = vwRole.ID WHERE UPPER(UserName)=@UserName";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserName";
                objDbParameter[0].Value = strUserName.ToUpper().Trim();

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }




        public static Utility.ResultType SelectByEmail(out DataTable objDataTable, string strUserName)
        {
            try
            {
                string strQuery = @"SELECT * FROM Document_Share where EmailID=@UserName";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserName";
                objDbParameter[0].Value = strUserName.Trim();

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType SelectByUserID(out DataTable objDataTable, int UserID)
        {
            try
            {
                string strQuery = @"SELECT * FROM Document_Share where UserID=@UserID";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserID";
                objDbParameter[0].Value = UserID;

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static DataSet  SelectByUserNameLike(string strUserName)
        {
            try
            {
                string strQuery = @"SELECT vwUser.ID,RoleID,Password,UserTypeID,RoleType FROM vwUser LEFT OUTER JOIN vwRole
                                    ON vwUser.RoleID = vwRole.ID WHERE UPPER(UserName) like '%" + strUserName.ToUpper().Trim() + "%'";

               
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return null;
                }
                else
                {
                    return ds;
                }
                //objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                //if (objDataTable.Rows.Count == 0)
                //{
                //    return Utility.ResultType.Failure;
                //}
                //return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);

                return null;
            }
        }

        public static DataSet SearchByUserNameLike(string strUserName)
        {
            try
            {
                char charQuote = '"';
                string strQuery = "SELECT U.ID,U.RoleID,U.UserName,R.rolename,U.FirstName,U.MiddleName,U.LastName,U.Address,U.City,U.MobileNo,U.EmailID,S.StateName,C.CountryName,"
                      + " (SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=U.Status) AS " + charQuote + "Status" + charQuote + ","
                      + " U.CreatedOn,"
                      + " (SELECT FirstName FROM vwUser WHERE ID = U.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + ","
                      + " U.UpdatedOn,"
                      + " (SELECT FirstName FROM vwUser WHERE ID = U.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + ""
                      + "  FROM vwUser U Left Outer join vwCountry C  on U.CountryID=C.ID "
                      + " Left Outer Join vwState S  on U.CountryID=C.ID and U.StateId=S.ID "
                      + " inner join vwRole R on U.roleid=R.id "
                      + " " + (Convert.ToInt32(HttpContext.Current.Session["RoleType"]) == 2 ? " WHERE RoleID IN (SELECT RepositoryUserID FROM vwRoleRights WHERE RepositoryAdminID = " + Convert.ToInt32(HttpContext.Current.Session["RoleID"]) + " )" : string.Empty) + " "
                      + "  and UPPER(UserName) like '%" + strUserName.ToUpper().Trim() + "%'"
                      + " ORDER BY ID DESC";


               
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataSet(strQuery);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return null;
                }
                else
                {
                    return ds;
                }
                //objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                //if (objDataTable.Rows.Count == 0)
                //{
                //    return Utility.ResultType.Failure;
                //}
                //return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);

                return null;
            }
        }

         public static DataSet SearchByRoleNameLike(string strRoleName)
         {
             try
             {
                 char charQuote = '"';
                 string strQuery = "SELECT U.ID,U.RoleID,U.UserName,R.rolename,U.FirstName,U.MiddleName,U.LastName,U.Address,U.City,U.MobileNo,U.EmailID,S.StateName,C.CountryName,"
                       + " (SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=U.Status) AS " + charQuote + "Status" + charQuote + ","
                       + " U.CreatedOn,"
                       + " (SELECT FirstName FROM vwUser WHERE ID = U.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + ","
                       + " U.UpdatedOn,"
                       + " (SELECT FirstName FROM vwUser WHERE ID = U.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + ""
                       + "  FROM vwUser U Left Outer join vwCountry C  on U.CountryID=C.ID "
                       + " Left Outer Join vwState S  on U.CountryID=C.ID and U.StateId=S.ID "
                       + " inner join vwRole R on U.roleid=R.id "
                       + " " + (Convert.ToInt32(HttpContext.Current.Session["RoleType"]) == 2 ? " WHERE RoleID IN (SELECT RepositoryUserID FROM vwRoleRights WHERE RepositoryAdminID = " + Convert.ToInt32(HttpContext.Current.Session["RoleID"]) + " )" : string.Empty) + " "
                       + "  and UPPER(R.rolename) like '%" + strRoleName.ToUpper().Trim() + "%'"
                       + " ORDER BY ID DESC";



                 DataSet ds = new DataSet();
                 ds = DataHelper.ExecuteDataSet(strQuery);
                 if (ds.Tables[0].Rows.Count <= 0)
                 {
                     return null;
                 }
                 else
                 {
                     return ds;
                 }
                 //objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                 //if (objDataTable.Rows.Count == 0)
                 //{
                 //    return Utility.ResultType.Failure;
                 //}
                 //return Utility.ResultType.Success;

             }
             catch (Exception ex)
             {
                 LogManager.ErrorLog(Utility.LogFilePath, ex);

                 return null;
             }
         }

        public static DataSet SearchByRoleUserNameLike(string strUserName, string strRoleName)
         {
             try
             {
                 char charQuote = '"';
                 string strQuery = "SELECT U.ID,U.RoleID,U.UserName,R.rolename,U.FirstName,U.MiddleName,U.LastName,U.Address,U.City,U.MobileNo,U.EmailID,S.StateName,C.CountryName,"
                       + " (SELECT SettingName FROM vwAppSetting WHERE SettingType='STATUS' AND SettingValue=U.Status) AS " + charQuote + "Status" + charQuote + ","
                       + " U.CreatedOn,"
                       + " (SELECT FirstName FROM vwUser WHERE ID = U.CreatedBy) AS " + charQuote + "CreatedBy" + charQuote + ","
                       + " U.UpdatedOn,"
                       + " (SELECT FirstName FROM vwUser WHERE ID = U.UpdatedBy) AS " + charQuote + "UpdatedBy" + charQuote + ""
                       + "  FROM vwUser U Left Outer join vwCountry C  on U.CountryID=C.ID "
                       + " Left Outer Join vwState S  on U.CountryID=C.ID and U.StateId=S.ID "
                       + " inner join vwRole R on U.roleid=R.id "
                       + " " + (Convert.ToInt32(HttpContext.Current.Session["RoleType"]) == 2 ? " WHERE RoleID IN (SELECT RepositoryUserID FROM vwRoleRights WHERE RepositoryAdminID = " + Convert.ToInt32(HttpContext.Current.Session["RoleID"]) + " )" : string.Empty) + " "
                       + "  and UPPER(UserName) like '%" + strUserName.ToUpper().Trim() + "%'"
                       + "  and UPPER(R.rolename) like '%" + strRoleName.ToUpper().Trim() + "%'"
                       + " ORDER BY ID DESC";



                 DataSet ds = new DataSet();
                 ds = DataHelper.ExecuteDataSet(strQuery);
                 if (ds.Tables[0].Rows.Count <= 0)
                 {
                     return null;
                 }
                 else
                 {
                     return ds;
                 }
                 //objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                 //if (objDataTable.Rows.Count == 0)
                 //{
                 //    return Utility.ResultType.Failure;
                 //}
                 //return Utility.ResultType.Success;

             }
             catch (Exception ex)
             {
                 LogManager.ErrorLog(Utility.LogFilePath, ex);

                 return null;
             }
         }

        public static Utility.ResultType SelectUserByEmailID(out DataTable objDataTable, string strEmailID)
        {
            try
            {
                string strQuery = "SELECT * FROM vwUser WHERE EmailID='" + strEmailID + "'";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[0];
                //objDbParameter[0] = objDbProviderFactory.CreateParameter();
                //objDbParameter[0].ParameterName = "EmailID";
                //objDbParameter[0].Value = strEmailID.ToLower().Trim();

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Insert(User objUser)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;


            try
            {
                DbParameter[] objDbParameter = new DbParameter[18];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserName";
                objDbParameter[0].Value = objUser.UserName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "Password";
                objDbParameter[1].Value = objUser.Password;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "UserTypeID";
                objDbParameter[2].Value = objUser.UserTypeID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "RoleID";
                objDbParameter[3].Value = objUser.RoleID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "UserIs";
                objDbParameter[4].Value = objUser.UserIs;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "FirstName";
                objDbParameter[5].Value = objUser.FirstName;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "MiddleName";
                objDbParameter[6].Value = objUser.MiddleName;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "LastName";
                objDbParameter[7].Value = objUser.LastName;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "Address";
                objDbParameter[8].Value = objUser.Address;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "City";
                objDbParameter[9].Value = objUser.City;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "CountryID";
                objDbParameter[10].Value = objUser.CountryID;

                objDbParameter[11] = objDbProviderFactory.CreateParameter();
                objDbParameter[11].ParameterName = "StateID";
                objDbParameter[11].Value = objUser.StateID;

                objDbParameter[12] = objDbProviderFactory.CreateParameter();
                objDbParameter[12].ParameterName = "PinCode";
                objDbParameter[12].Value = objUser.PinCode;

                objDbParameter[13] = objDbProviderFactory.CreateParameter();
                objDbParameter[13].ParameterName = "MobileNo";
                objDbParameter[13].Value = objUser.MobileNo;

                objDbParameter[14] = objDbProviderFactory.CreateParameter();
                objDbParameter[14].ParameterName = "EmailID";
                objDbParameter[14].Value = objUser.EmailID;

                objDbParameter[15] = objDbProviderFactory.CreateParameter();
                objDbParameter[15].ParameterName = "LoginCount";
                objDbParameter[15].Value = objUser.LoginCount;

                //objDbParameter[16] = objDbProviderFactory.CreateParameter();
                //objDbParameter[16].ParameterName = "CreatedOn";
                //objDbParameter[16].Value = DateTime.Now;


                objDbParameter[16] = objDbProviderFactory.CreateParameter();
                objDbParameter[16].ParameterName = "CreatedBy";
                objDbParameter[16].Value = objUser.CreatedBy;


                objDbParameter[17] = objDbProviderFactory.CreateParameter();
                objDbParameter[17].ParameterName = "Status";
                objDbParameter[17].Value = objUser.Status;

                DataHelper.ExecuteNonQuery("SP_I_User", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();

                return Utility.ResultType.Success;

            }

            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }

        }

        public static Utility.ResultType Update(User objUser)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;


            try
            {
                DbParameter[] objDbParameter = new DbParameter[18];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
                objDbParameter[0].Value = objUser.UserID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "UserName";
                objDbParameter[1].Value = objUser.UserName;

               
                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "UserTypeID";
                objDbParameter[2].Value = objUser.UserTypeID;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "RoleID";
                objDbParameter[3].Value = objUser.RoleID;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "UserIs";
                objDbParameter[4].Value = objUser.UserIs;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "FirstName";
                objDbParameter[5].Value = objUser.FirstName;

                objDbParameter[6] = objDbProviderFactory.CreateParameter();
                objDbParameter[6].ParameterName = "MiddleName";
                objDbParameter[6].Value = objUser.MiddleName;

                objDbParameter[7] = objDbProviderFactory.CreateParameter();
                objDbParameter[7].ParameterName = "LastName";
                objDbParameter[7].Value = objUser.LastName;

                objDbParameter[8] = objDbProviderFactory.CreateParameter();
                objDbParameter[8].ParameterName = "Address";
                objDbParameter[8].Value = objUser.Address;

                objDbParameter[9] = objDbProviderFactory.CreateParameter();
                objDbParameter[9].ParameterName = "City";
                objDbParameter[9].Value = objUser.City;

                objDbParameter[10] = objDbProviderFactory.CreateParameter();
                objDbParameter[10].ParameterName = "CountryID";
                objDbParameter[10].Value = objUser.CountryID;

                objDbParameter[11] = objDbProviderFactory.CreateParameter();
                objDbParameter[11].ParameterName = "StateID";
                objDbParameter[11].Value = objUser.StateID;

                objDbParameter[12] = objDbProviderFactory.CreateParameter();
                objDbParameter[12].ParameterName = "PinCode";
                objDbParameter[12].Value = objUser.PinCode;

                objDbParameter[13] = objDbProviderFactory.CreateParameter();
                objDbParameter[13].ParameterName = "MobileNo";
                objDbParameter[13].Value = objUser.MobileNo;

                objDbParameter[14] = objDbProviderFactory.CreateParameter();
                objDbParameter[14].ParameterName = "EmailID";
                objDbParameter[14].Value = objUser.EmailID;

             
                objDbParameter[15] = objDbProviderFactory.CreateParameter();
                objDbParameter[15].ParameterName = "UpdatedOn";
                objDbParameter[15].Value = DateTime.Now;


                objDbParameter[16] = objDbProviderFactory.CreateParameter();
                objDbParameter[16].ParameterName = "UpdatedBy";
                objDbParameter[16].Value = objUser.UpdatedBy;


                objDbParameter[17] = objDbProviderFactory.CreateParameter();
                objDbParameter[17].ParameterName = "Status";
                objDbParameter[17].Value = objUser.Status;

                DataHelper.ExecuteNonQuery("SP_U_User", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();

                return Utility.ResultType.Success;

            }

            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }

        }

        public static Utility.ResultType ChangePassword(User objUser)
        {
            DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
            DbTransaction objDbTransaction = Utility.GetTransaction;


            try
            {
                DbParameter[] objDbParameter = new DbParameter[4];

                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "ID";
                objDbParameter[0].Value = objUser.UserID;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "Password";
                objDbParameter[1].Value = objUser.Password;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "UpdatedOn";
                objDbParameter[2].Value = DateTime.Now;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "UpdatedBy";
                objDbParameter[3].Value = objUser.UpdatedBy;

                DataHelper.ExecuteNonQuery("SP_U_User_Password", objDbTransaction, objDbParameter);
                objDbTransaction.Commit();

                return Utility.ResultType.Success;

            }

            catch (Exception ex)
            {
                objDbTransaction.Rollback();
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }

        }
       
        public static Utility.ResultType Exists(string strUserName)
        {
           try
            {
                string strQuery = "SELECT * FROM vwUser WHERE UPPER(UserName)=@UserName";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[1];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserName";
                objDbParameter[0].Value = strUserName.ToUpper().Trim();

                object objResult = DataHelper.ExecuteScalar(strQuery, objDbParameter);
                if (objResult == null)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;
        
            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
        public static Utility.ResultType UserExists(out DataTable objDataTable, string strUserName, string strPassword)
        {
            try
            {
                string strQuery = "SELECT * FROM vwUser WHERE UserName=@UserName and Password=@Password";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserName";
                objDbParameter[0].Value = strUserName.ToUpper().Trim();

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "Password";
                objDbParameter[1].Value = strPassword.ToUpper().Trim();

                objDataTable = DataHelper.ExecuteDataTable(strQuery, objDbParameter);
                if (objDataTable.Rows.Count == 0)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        /// <summary>
        /// Check For Total Users
        /// </summary>
        /// <param name="objDataTable"></param>
        /// <returns></returns>
        public static Utility.ResultType TotalUsers(out DataTable objDataTable)
        {
            try
            {
                string strQuery = "SELECT COUNT(*) FROM vwUser";
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                objDataTable = DataHelper.ExecuteDataTable(strQuery, null);
                if (objDataTable == null)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                objDataTable = null;
                return Utility.ResultType.Error;
            }
        }

        public static Utility.ResultType Authenticate(string strUserName, string strPassword)
        {
            try
            {
                string strQuery = "SELECT * FROM vwUser U Inner Join vwRole R"
                                  + " ON U.RoleId = R.ID"
                                  + " WHERE UserName=@UserName AND Password=@Password AND  U.Status = 1 AND R.Status=1";
   
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@UserName";
                objDbParameter[0].Value = strUserName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@Password";
                objDbParameter[1].Value = strPassword;

                object objResult = DataHelper.ExecuteScalar(strQuery, objDbParameter);
                if (objResult == null)
                {
                    return Utility.ResultType.Failure;
                }
                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }




        public static Utility.ResultType Authenticate_ShareDocument(string strUserName, string strPassword)
        {
            try
            {
                string strQuery = "SELECT * FROM Document_share WHERE EmailId=@UserName AND Password=@Password ";

                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;

                DbParameter[] objDbParameter = new DbParameter[2];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@UserName";
                objDbParameter[0].Value = strUserName;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@Password";
                objDbParameter[1].Value = strPassword;

                object objResult = DataHelper.ExecuteScalar(strQuery, objDbParameter);
                if (objResult == null)
                {
                    return Utility.ResultType.Failure;
                }
                else
                {
                    DateTime ExpiryDate;
                    DateTime CurrentDate = DateTime.Now.Date;
                    return Utility.ResultType.Success;
                }
                

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }
        }
       


        #endregion

        public static Utility.ResultType AuditLogDetails(string IpAdd, string MacAdd, string Activity, string DocName, int UserId, DbTransaction objDBTransaction1)
        {
            
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[6];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "IpAdd";
                objDbParameter[0].Value = IpAdd;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "MacAdd";
                objDbParameter[1].Value = MacAdd;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "DateOfActivity";
                objDbParameter[2].Value = DateTime.Now;

                objDbParameter[3] = objDbProviderFactory.CreateParameter();
                objDbParameter[3].ParameterName = "DocName";
                objDbParameter[3].Value = DocName;

                objDbParameter[4] = objDbProviderFactory.CreateParameter();
                objDbParameter[4].ParameterName = "UserId";
                objDbParameter[4].Value = UserId;

                objDbParameter[5] = objDbProviderFactory.CreateParameter();
                objDbParameter[5].ParameterName = "Activity";
                objDbParameter[5].Value = Activity;

                DataHelper.ExecuteNonQuery("SP_I_AuditLogMhada", objDBTransaction1, objDbParameter);

                return Utility.ResultType.Success;

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }

        }

        public static Utility.ResultType OTPDetails(string UserEmail, string OTPNum, DbTransaction objDBTransaction1)
        {
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[3];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "UserEmail";
                objDbParameter[0].Value = UserEmail;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "OtpNumber";
                objDbParameter[1].Value = OTPNum;

                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "OtpTime";
                objDbParameter[2].Value = Convert.ToString(DateTime.Now);



                object objResult = DataHelper.ExecuteNonQuery("Sp_OTPDeatils", objDBTransaction1, objDbParameter);
                objDBTransaction1.Commit();
                objDBTransaction1.Dispose();
                
                if (objResult == null)
                {
                    return Utility.ResultType.Failure;
                }
                else
                {

                return Utility.ResultType.Success;
                }

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                return Utility.ResultType.Error;
            }


        }

        public static Utility.ResultType GetOtpDetails(string UserEmail,string otpnumber, out DataTable table)
        {
            
            try
            {
                DbProviderFactory objDbProviderFactory = Utility.GetProviderFactory;
                DbParameter[] objDbParameter = new DbParameter[3];
                objDbParameter[0] = objDbProviderFactory.CreateParameter();
                objDbParameter[0].ParameterName = "@UserEmail";
                objDbParameter[0].Value = UserEmail;

                objDbParameter[1] = objDbProviderFactory.CreateParameter();
                objDbParameter[1].ParameterName = "@type";
                objDbParameter[1].Value = "1";


                objDbParameter[2] = objDbProviderFactory.CreateParameter();
                objDbParameter[2].ParameterName = "@OtpNumber";
                objDbParameter[2].Value = otpnumber;


                table = DataHelper.ExecuteDataTableNew("Sp_OTPDeatils", objDbParameter);
                
                if (table == null)
                {
                    return Utility.ResultType.Failure;
                }
                else
                {

                    return Utility.ResultType.Success;
                }

            }
            catch (Exception ex)
            {
                LogManager.ErrorLog(Utility.LogFilePath, ex);
                table = null;
                return Utility.ResultType.Error;
            }
        }

    }
        
}
